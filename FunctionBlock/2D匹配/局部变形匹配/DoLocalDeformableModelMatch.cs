using HalconDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using System.IO;

namespace FunctionBlock
{
    [Serializable]
    public class DoLocalDeformableModelMatch2D
    {
        private CreateLocalDeformableModelMethod createDeformableModel;
        private FindLocalDeformableModelMethod findDeformableModel;
        private HDeformableModel HDeformableModelID;
        private double refPoint_row; // 用于记录模型的参考位置 
        private double refPoint_col;// 用于记录模型的参考位置 
        private double refPoint_rad;// 用于记录模型的参考位置 
        private LocalDeformableMatchingResult _DeformableMatchResult;
        private HImage templateImage;
        private HImage searchImageRegion;

        public DoLocalDeformableModelMatch2D()
        {
            this.createDeformableModel = new CreateLocalDeformableModelMethod();
            this.findDeformableModel = new FindLocalDeformableModelMethod();
            this.CreateDeformableModelParam = new CreateLocalDeformableModelParam();
            this.FindDeformableModelParam = new FindLocalDeformableModelParam();

        }

        public HImage SearchImageRegion
        {
            get
            {
                this.searchImageRegion = findDeformableModel.SearchImageRegion;
                return searchImageRegion;
            }
            set
            {
                this.searchImageRegion = value;
            }
        }
        public HImage TemplateImage
        {
            get
            {
                this.templateImage = createDeformableModel.TemplateImage;
                return this.templateImage;
            }
            set
            {
                this.templateImage = value;
            }
        }

        [NonSerialized]
        private HXLDCont _ShapeModelContour;
        public HXLDCont ShapeModelContour
        {
            get
            {
                //this._ShapeModelContour = HShapeModelID?.GetShapeModelContours(1);
                return this._ShapeModelContour;
            }
            set
            {
                this._ShapeModelContour = value;
            }
        }

        public CreateLocalDeformableModelParam CreateDeformableModelParam
        {
            get;
            set;
        }
        public FindLocalDeformableModelParam FindDeformableModelParam
        {
            get;
            set;
        }
        public LocalDeformableMatchingResult LocalDeformableMatchResult //
        {
            get
            {
                return _DeformableMatchResult;
            }
            set
            {
                this._DeformableMatchResult = value;
            }
        }

        public bool CreateLocalDeformableModelFromImage(HImage image)
        {
            bool result = false;
            double row = 0, col = 0; // 模板区域中心
            HHomMat2D hHomMat2D = new HHomMat2D();
            HXLDCont hXLDCont;
            if (image == null || !image.IsInitialized())
            {
                throw new ArgumentNullException("模板图像为空或未被初始化");
            }
            if (HDeformableModelID != null && HDeformableModelID.IsInitialized()) HDeformableModelID.Dispose();
            HDeformableModelID = createDeformableModel.create_local_deformable_model(image, this.CreateDeformableModelParam);
            if (HDeformableModelID != null && HDeformableModelID.IsInitialized())
            {
                this._ShapeModelContour?.Dispose();
                this._ShapeModelContour = HDeformableModelID.GetDeformableModelContours(1); // 获取变形模型轮廓 
                /////////////// 通过图像的方式来创建，一定要用区域重心来确定参考点 /////////////////
                createDeformableModel.TemplateRegion?.AreaCenter(out row, out col);
                this.refPoint_row = row;
                this.refPoint_col = col;
                this.refPoint_rad = 0;//
                //////////////////////////////
                _DeformableMatchResult = new LocalDeformableMatchingResult(1);
                hHomMat2D.VectorAngleToRigid(0, 0, 0, row, col, 0);
                hXLDCont = hHomMat2D.AffineTransContourXld(this._ShapeModelContour);
                _DeformableMatchResult.MatchCont.AddXLDCont(hXLDCont);
                result = true;
                LoggerHelper.Info("CreateLocalDeformableModelFromImage" + "模型创建成功");
            }
            else
            {
                LoggerHelper.Info("CreateLocalDeformableModelFromImage" + "创建模型失败");
                result = false;
            }
            return result;
        }


        public bool CreateLocalDeformableModelFromXLD(HImage image, HXLDCont hXLDCont)
        {
            bool result = false;
            HHomMat2D hHomMat2D = new HHomMat2D();
            if (image == null || !image.IsInitialized())
            {
                throw new ArgumentNullException("image 模板图像为空或未被初始化");
            }
            if (hXLDCont == null || !hXLDCont.IsInitialized())
            {
                throw new ArgumentNullException("hXLDCont 为空或未被初始化");
            }
            if (HDeformableModelID != null && HDeformableModelID.IsInitialized()) HDeformableModelID.Dispose();
            HDeformableModelID = createDeformableModel.create_local_deformable_model(image, this.CreateDeformableModelParam);
            if (HDeformableModelID != null && HDeformableModelID.IsInitialized())
            {
                this._ShapeModelContour?.Dispose();
                this._ShapeModelContour = HDeformableModelID.GetDeformableModelContours(1); // 获取变形模型轮廓 
                /////////////// 通过图像的方式来创建，一定要用区域重心来确定参考点 /////////////////
                result = findDeformableModel.find_local_deformable_model(createDeformableModel.TemplateImage, this.HDeformableModelID, this.FindDeformableModelParam, out this._DeformableMatchResult);
                this.refPoint_row = this._DeformableMatchResult.PixCoordSystem.CurrentPoint.Row; // 因为XLD轮廓有不同的来源，所以这里定位一次来确认 参考点位置
                this.refPoint_col = this._DeformableMatchResult.PixCoordSystem.CurrentPoint.Col;
                this.refPoint_rad = this._DeformableMatchResult.PixCoordSystem.CurrentPoint.Rad;
                //////////////////////////////
                _DeformableMatchResult = new LocalDeformableMatchingResult(1);
                hHomMat2D.VectorAngleToRigid(0, 0, 0, this.refPoint_row, this.refPoint_col, 0);
                hXLDCont = hHomMat2D.AffineTransContourXld(this._ShapeModelContour);
                _DeformableMatchResult.MatchCont.AddXLDCont(hXLDCont);
                result = true;
                LoggerHelper.Info("CreateLocalDeformableModelFromImage" + "模型创建成功");
            }
            else
            {
                LoggerHelper.Info("CreateLocalDeformableModelFromImage" + "创建模型失败");
                result = false;
            }
            return result;
        }
        public bool FindLocalDeformableModel(HImage hImage)
        {
            bool result = false;
            if (this.HDeformableModelID != null || this.HDeformableModelID.IsInitialized())
                result = findDeformableModel.find_local_deformable_model(hImage, this.HDeformableModelID, this.FindDeformableModelParam, out this._DeformableMatchResult);
            else
                throw new ArgumentNullException("模型对象为空或未初始化");
            // 属性的返回值不能当做一个变量来使用;
            if (result)
            {
                this._DeformableMatchResult.PixCoordSystem.CurrentPoint.Rad = this.refPoint_rad; // 如果禁用角度补正，那么当前角度等于参考角度
                this._DeformableMatchResult.PixCoordSystem.ReferencePoint.Row = this.refPoint_row; // 以后的每次匹配都需要将参考点重新写入
                this._DeformableMatchResult.PixCoordSystem.ReferencePoint.Col = this.refPoint_col;
                this._DeformableMatchResult.PixCoordSystem.ReferencePoint.Rad = this.refPoint_rad;
                LoggerHelper.Info("寻找变形模型失败!");
                result = true;
            }
            else // 如果匹配失败，让当前点等于参考点
            {
                this._DeformableMatchResult.PixCoordSystem.CurrentPoint.Row = this.refPoint_row;
                this._DeformableMatchResult.PixCoordSystem.CurrentPoint.Col = this.refPoint_col;
                this._DeformableMatchResult.PixCoordSystem.CurrentPoint.Rad = this.refPoint_rad; // 如果禁用角度补正，那么当前角度等于参考角度
                //////////////////////////////////
                this._DeformableMatchResult.PixCoordSystem.ReferencePoint.Row = this.refPoint_row; // 以后的每次匹配都需要将参考点重新写入
                this._DeformableMatchResult.PixCoordSystem.ReferencePoint.Col = this.refPoint_col;
                this._DeformableMatchResult.PixCoordSystem.ReferencePoint.Rad = this.refPoint_rad;
                LoggerHelper.Info("寻找变形模型成功!");
                result = false;
            }
            return result;
        }



        public void ClearHDeformableModel()
        {
            if (this.HDeformableModelID != null && this.HDeformableModelID.IsInitialized())
                this.HDeformableModelID.ClearDeformableModel();
        }

        public void SaveHDeformableModel(string filePath)
        {
            if (this.HDeformableModelID != null && this.HDeformableModelID.IsInitialized())
                this.HDeformableModelID.WriteDeformableModel(filePath);
        }

        public void ReaHDeformableModel(string filePath)
        {
            if (!File.Exists(filePath)) return;
            if (this.HDeformableModelID == null)
                this.HDeformableModelID = new HDeformableModel();
            else
            {
                if (this.HDeformableModelID.IsInitialized())
                    this.HDeformableModelID.ClearDeformableModel();
                this.HDeformableModelID = new HDeformableModel();
            }
            this.HDeformableModelID.ReadDeformableModel(filePath);
        }



    }





}

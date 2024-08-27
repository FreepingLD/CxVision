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
    public class DoNccModelMatch
    {
        private CreateNccModelMethod createNccModel;
        private FindNccModelMethod findNccModel;
        [NonSerialized]
        private HalconDotNet.HNCCModel HNCCModelID;
        [NonSerialized]
        private HalconDotNet.HNCCModel[] HNCCModelIDS;
        private double []refPoint_row; // 用于记录模型的参考位置 
        private double []refPoint_col;// 用于记录模型的参考位置 
        private double []refPoint_rad;// 用于记录模型的参考位置 
        [NonSerialized]
        private MatchingResult _nccMatchResult;
        [NonSerialized]
        private HImage templateImage;
        [NonSerialized]
        private HImage searchImageRegion;
        private int ModelNum;



        public DoNccModelMatch()
        {
            this.createNccModel = new CreateNccModelMethod();
            this.findNccModel = new FindNccModelMethod();
            this.CreateNccModelParam = new CreateNccModelParam();
            this.FindNccModelParam = new FindNccModelParam();
        }

        public HImage SearchImageRegion
        {
            get
            {
                this.searchImageRegion = findNccModel.SearchImageRegion;
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
                this.templateImage = createNccModel.TemplateImage;
                return this.templateImage;
            }
            set
            {
                this.templateImage = value;
            }
        }

        [NonSerialized]
        private HXLDCont _NccModelContour;
        public HXLDCont NccModelContour
        {
            get
            {
                return this._NccModelContour;
            }
            set
            {
                this._NccModelContour = value;
            }
        }

        public CreateNccModelParam CreateNccModelParam
        {
            get;
            set;
        }

        public FindNccModelParam FindNccModelParam
        {
            get;
            set;
        }

        public MatchingResult NccMatchResult //
        {
            get
            {
                return _nccMatchResult;
            }
            set
            {
                this._nccMatchResult = value;
            }
        }

        public bool CreateNccModelFromImage(HImage image)
        {
            bool result = false;
            if (image == null || !image.IsInitialized())
            {
                throw new ArgumentNullException("模板图像为空或未被初始化");
            }
            if (this.HNCCModelID != null && this.HNCCModelID.IsInitialized()) this.HNCCModelID.ClearNccModel();
            this.HNCCModelID = createNccModel.create_ncc_model(image, this.CreateNccModelParam);
            if (this.NccModelContour != null && this.NccModelContour.IsInitialized())
                this.NccModelContour.Dispose();
            this.NccModelContour = this.HNCCModelID.GetNccModelRegion().GenContourRegionXld("border_holes");
            if (this.HNCCModelID != null && this.HNCCModelID.IsInitialized())
            {
                HTuple areaRow = 0, areaCol = 0;
                this.createNccModel.TemplateRegion.AreaCenter(out areaRow, out areaCol);
                ///  创建模型后，参考点为创建模板图像区域的重心
                switch(areaRow.Type)
                {
                    default:
                    case HTupleType.DOUBLE:
                        this.refPoint_row = areaRow.DArr;
                        this.refPoint_col = areaCol.DArr;
                        this.refPoint_rad = HTuple.TupleGenConst(areaRow.Length, 0);
                        break;
                    case HTupleType.LONG:
                        List<double> listRow = new List<double>();
                        List<double> listCol = new List<double>();
                        List<double> listAngle = new List<double>();
                        for (int i = 0; i < areaRow.Length; i++)
                        {
                            listRow.Add((double)areaRow[i]);
                            listCol.Add((double)areaCol[i]);
                            listAngle.Add(0);
                        }
                        this.refPoint_row = listRow.ToArray();
                        this.refPoint_col = listCol.ToArray();
                        this.refPoint_rad = listAngle.ToArray();
                        break;
                }
                //// 变换模型轮廓 
                HHomMat2D hHomMat2D = new HHomMat2D();
                this._nccMatchResult = new MatchingResult(1);
                for (int i = 0; i < areaRow.Length; i++)
                {
                    hHomMat2D.VectorAngleToRigid(0, 0, 0, areaRow[i].D, areaCol[i].D, 0); //
                    HXLDCont xld = hHomMat2D.AffineTransContourXld(this.NccModelContour);
                    this._nccMatchResult.MatchCont.AddXLDCont(xld);
                }
                result = true;
                LoggerHelper.Info("创建模型成功");
            }
            else
            {
                LoggerHelper.Info(nameof(enModelMatchType.aniso_shape_model) + "创建模型失败");
                result = false;
            }
            return result;
        }

        public bool CreateNccModelsFromImage(HImage image)
        {
            bool result = false;
            if (image == null || !image.IsInitialized())
            {
                throw new ArgumentNullException("模板图像为空或未被初始化");
            }
            if (this.HNCCModelIDS != null)
            {
                foreach (var item in this.HNCCModelIDS)
                {
                    if (item.IsInitialized()) item.ClearNccModel();
                }
            }
            this.HNCCModelIDS = createNccModel.create_ncc_models(image, this.CreateNccModelParam);
            if (this.NccModelContour != null && this.NccModelContour.IsInitialized())
                this.NccModelContour.Dispose();
            this.NccModelContour = new HXLDCont();
            this.NccModelContour.GenEmptyObj();
            foreach (var item in this.HNCCModelIDS)
            {
                this.NccModelContour = this.NccModelContour.ConcatObj(item.GetNccModelRegion().GenContourRegionXld("border_holes"));
            }
            if (this.HNCCModelIDS != null)
            {
                this.ModelNum = this.HNCCModelIDS.Length;
                HTuple areaRow = 0, areaCol = 0;
                this.createNccModel.TemplateRegion.AreaCenter(out areaRow, out areaCol);
                ///  创建模型后，参考点为创建模板图像区域的重心
                switch (areaRow.Type)
                {
                    default:
                    case HTupleType.DOUBLE:
                        this.refPoint_row = areaRow.DArr;
                        this.refPoint_col = areaCol.DArr;
                        this.refPoint_rad = HTuple.TupleGenConst(areaRow.Length, 0);
                        break;
                    case HTupleType.LONG:
                        List<double> listRow = new List<double>();
                        List<double> listCol = new List<double>();
                        List<double> listAngle = new List<double>();
                        for (int i = 0; i < areaRow.Length; i++)
                        {
                            listRow.Add((double)areaRow[i]);
                            listCol.Add((double)areaCol[i]);
                            listAngle.Add(0);
                        }
                        this.refPoint_row = listRow.ToArray();
                        this.refPoint_col = listCol.ToArray();
                        this.refPoint_rad = listAngle.ToArray();
                        break;
                }
                //// 变换模型轮廓 
                HHomMat2D hHomMat2D = new HHomMat2D();
                this._nccMatchResult = new MatchingResult(1);
                int coiunt = this.NccModelContour.CountObj();
                for (int i = 0; i < areaRow.Length; i++)
                {
                    if (areaRow[i].D == 0 && areaCol[i].D == 0) continue;
                    hHomMat2D.VectorAngleToRigid(0, 0, 0, areaRow[i].D, areaCol[i].D, 0); //
                    HXLDCont xld = hHomMat2D.AffineTransContourXld(this.NccModelContour.SelectObj(i + 1));
                    this._nccMatchResult.MatchCont.AddXLDCont(xld);
                }
                result = true;
                LoggerHelper.Info("创建模型成功");
            }
            else
            {
                LoggerHelper.Info(nameof(enModelMatchType.aniso_shape_model) + "创建模型失败");
                result = false;
            }
            return result;
        }

        public bool FindNccModel(HImage hImage)
        {
            bool result = false;
            if (hImage == null || !hImage.IsInitialized())
            {
                throw new ArgumentNullException("模板图像为空或未被初始化");
            }
            if (this.HNCCModelID == null || !this.HNCCModelID.IsInitialized())
            {
                throw new ArgumentNullException("模型对象为空或未初始化");
            }
            result = findNccModel.find_ncc_model(hImage, this.HNCCModelID, this.FindNccModelParam, out this._nccMatchResult);
            // 属性的返回值不能当做一个变量来使用;
            if (result) // this._nccMatchResult.PixCoordSystem != null && this._nccMatchResult.PixCoordSystem.Length > 0
            {
                for (int i = 0; i < this._nccMatchResult.PixCoordSystem.Length; i++)
                {
                    switch (this.FindNccModelParam.AdjustType)
                    {
                        case enAdjustType.X:
                            this._nccMatchResult.PixCoordSystem[i].CurrentPoint.Row = this.refPoint_row[0];
                            this._nccMatchResult.PixCoordSystem[i].CurrentPoint.Rad = this.refPoint_rad[0];
                            break;
                        case enAdjustType.Y:
                            this._nccMatchResult.PixCoordSystem[i].CurrentPoint.Col = this.refPoint_col[0];
                            this._nccMatchResult.PixCoordSystem[i].CurrentPoint.Rad = this.refPoint_rad[0];
                            break;
                        case enAdjustType.XY:
                            this._nccMatchResult.PixCoordSystem[i].CurrentPoint.Rad = this.refPoint_rad[0];
                            break;
                        case enAdjustType.Theta:
                            this._nccMatchResult.PixCoordSystem[i].CurrentPoint.Row = this.refPoint_row[0];
                            this._nccMatchResult.PixCoordSystem[i].CurrentPoint.Col = this.refPoint_col[0];
                            break;
                    }
                    this._nccMatchResult.PixCoordSystem[i].ReferencePoint.Row = this.refPoint_row[0]; // 以后的每次匹配都需要将参考点重新写入
                    this._nccMatchResult.PixCoordSystem[i].ReferencePoint.Col = this.refPoint_col[0];
                    this._nccMatchResult.PixCoordSystem[i].ReferencePoint.Rad = this.refPoint_rad[0];
                    //this._nccMatchResult.PixCoordSystem[i].ReferencePoint.CamParams = hImage.CamParams;
                    //this._nccMatchResult.PixCoordSystem[i].CurrentPoint.CamParams = hImage.CamParams;
                }
                LoggerHelper.Info("寻找模型参考点成功");
                result = true;
            }
            else
            {
                this._nccMatchResult.PixCoordSystem[0].CurrentPoint.Row = this.refPoint_row[0];
                this._nccMatchResult.PixCoordSystem[0].CurrentPoint.Col = this.refPoint_col[0];
                this._nccMatchResult.PixCoordSystem[0].CurrentPoint.Rad = this.refPoint_rad[0]; // 如果禁用角度补正，那么当前角度等于参考角度
                ////////////////////////////////////////////////////////////////////////////
                this._nccMatchResult.PixCoordSystem[0].ReferencePoint.Row = this.refPoint_row[0]; // 以后的每次匹配都需要将参考点重新写入
                this._nccMatchResult.PixCoordSystem[0].ReferencePoint.Col = this.refPoint_col[0];
                this._nccMatchResult.PixCoordSystem[0].ReferencePoint.Rad = this.refPoint_rad[0];
                LoggerHelper.Info("寻找模型参考点失败");
                result = false;
            }
            return result;
        }

        public bool FindNccModels(HImage hImage)
        {
            bool result = false;
            if (hImage == null || !hImage.IsInitialized())
            {
                throw new ArgumentNullException("模板图像为空或未被初始化");
            }
            if (this.HNCCModelIDS == null)
            {
                throw new ArgumentNullException("模型对象为空或未初始化");
            }
            result = findNccModel.find_ncc_models(hImage, this.HNCCModelIDS, this.FindNccModelParam, out this._nccMatchResult);
            // 属性的返回值不能当做一个变量来使用;
            if (result) // this._nccMatchResult.PixCoordSystem != null && this._nccMatchResult.PixCoordSystem.Length > 0
            {
                for (int i = 0; i < this._nccMatchResult.PixCoordSystem.Length; i++)
                {
                    switch (this.FindNccModelParam.AdjustType)
                    {
                        case enAdjustType.X:
                            this._nccMatchResult.PixCoordSystem[i].CurrentPoint.Row = this.refPoint_row[this._nccMatchResult.ModelIndex];
                            this._nccMatchResult.PixCoordSystem[i].CurrentPoint.Rad = this.refPoint_rad[this._nccMatchResult.ModelIndex];
                            break;
                        case enAdjustType.Y:
                            this._nccMatchResult.PixCoordSystem[i].CurrentPoint.Col = this.refPoint_col[this._nccMatchResult.ModelIndex];
                            this._nccMatchResult.PixCoordSystem[i].CurrentPoint.Rad = this.refPoint_rad[this._nccMatchResult.ModelIndex];
                            break;
                        case enAdjustType.XY:
                            this._nccMatchResult.PixCoordSystem[i].CurrentPoint.Rad = this.refPoint_rad[this._nccMatchResult.ModelIndex];
                            break;
                        case enAdjustType.Theta:
                            this._nccMatchResult.PixCoordSystem[i].CurrentPoint.Row = this.refPoint_row[this._nccMatchResult.ModelIndex];
                            this._nccMatchResult.PixCoordSystem[i].CurrentPoint.Col = this.refPoint_col[this._nccMatchResult.ModelIndex];
                            break;
                    }
                    this._nccMatchResult.PixCoordSystem[i].ReferencePoint.Row = this.refPoint_row[this._nccMatchResult.ModelIndex]; // 以后的每次匹配都需要将参考点重新写入
                    this._nccMatchResult.PixCoordSystem[i].ReferencePoint.Col = this.refPoint_col[this._nccMatchResult.ModelIndex];
                    this._nccMatchResult.PixCoordSystem[i].ReferencePoint.Rad = this.refPoint_rad[this._nccMatchResult.ModelIndex];
                    //this._nccMatchResult.PixCoordSystem[i].ReferencePoint.CamParams = hImage.CamParams;
                    //this._nccMatchResult.PixCoordSystem[i].CurrentPoint.CamParams = hImage.CamParams;
                }
                LoggerHelper.Info("寻找模型参考点成功");
                result = true;
            }
            else
            {
                this._nccMatchResult.PixCoordSystem[0].CurrentPoint.Row = this.refPoint_row[this._nccMatchResult.ModelIndex];
                this._nccMatchResult.PixCoordSystem[0].CurrentPoint.Col = this.refPoint_col[this._nccMatchResult.ModelIndex];
                this._nccMatchResult.PixCoordSystem[0].CurrentPoint.Rad = this.refPoint_rad[this._nccMatchResult.ModelIndex]; // 如果禁用角度补正，那么当前角度等于参考角度
                ////////////////////////////////////////////////////////////////////////////
                this._nccMatchResult.PixCoordSystem[0].ReferencePoint.Row = this.refPoint_row[this._nccMatchResult.ModelIndex]; // 以后的每次匹配都需要将参考点重新写入
                this._nccMatchResult.PixCoordSystem[0].ReferencePoint.Col = this.refPoint_col[this._nccMatchResult.ModelIndex];
                this._nccMatchResult.PixCoordSystem[0].ReferencePoint.Rad = this.refPoint_rad[this._nccMatchResult.ModelIndex];
                LoggerHelper.Info("寻找模型参考点失败");
                result = false;
            }
            return result;
        }

        public void ClearNccModel()
        {
            if (this.HNCCModelID != null && this.HNCCModelID.IsInitialized())
                this.HNCCModelID.ClearNccModel();
            //////////////////////////////////////////////////
            if (this.HNCCModelIDS != null)
            {
                foreach (var item in this.HNCCModelIDS)
                {
                    if (item!= null && item.IsInitialized())
                        item.ClearNccModel();
                }
            }
        }
        public void RemoveAtShapeModel(int index)
        {
            ///////////////////////////////
            if (this.HNCCModelIDS != null && this.HNCCModelIDS.Length > 0)
            {
                List<HNCCModel> list = new List<HNCCModel>();
                list.AddRange(this.HNCCModelIDS);
                if (list.Count > index)
                    list.RemoveAt(index);
                this.HNCCModelIDS = list.ToArray();
            }
        }
        public void SaveHShapeModel(string filePath)
        {
            if (this.HNCCModelID != null && this.HNCCModelID.IsInitialized())
                this.HNCCModelID.WriteNccModel(filePath);
            //////////////////////////////////////////////////
            if (this.HNCCModelIDS != null)
            {
                int index = 1;
                foreach (var item in this.HNCCModelIDS)
                {
                    if (item != null && item.IsInitialized())
                        item.WriteNccModel(filePath.Insert(filePath.LastIndexOf('.'), index.ToString()));
                    index++;
                }
            }
        }

        public void ReadHShapeModel(string filePath)
        {
            if (this.HNCCModelID == null)
                this.HNCCModelID = new HNCCModel();
            else
            {
                if (this.HNCCModelID.IsInitialized())
                    this.HNCCModelID.ClearNccModel();
                this.HNCCModelID = new HNCCModel();
            }
            if (File.Exists(filePath))
                this.HNCCModelID.ReadNccModel(filePath);
            //////////////////////////////////////////////////  多模型型  ///////////////////////////////
            if (this.HNCCModelIDS != null)
            {
                foreach (var item in this.HNCCModelIDS)
                {
                    if (item != null && item.IsInitialized())
                        item.ClearNccModel();
                }
            }
            this.HNCCModelIDS = new HNCCModel[this.ModelNum];
            for (int index = 0; index < this.ModelNum; index++)
            {
                if (File.Exists(filePath.Insert(filePath.LastIndexOf('.'), (index + 1).ToString())))
                {
                    this.HNCCModelIDS[index] = new HNCCModel();
                    this.HNCCModelIDS[index].ReadNccModel(filePath.Insert(filePath.LastIndexOf('.'), (index + 1).ToString()));
                }
            }

        }



    }





}

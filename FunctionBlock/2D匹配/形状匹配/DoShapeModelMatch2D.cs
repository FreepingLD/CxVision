using HalconDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using System.Security.Policy;
using System.Xml.Serialization;
using System.IO;

namespace FunctionBlock
{
    [Serializable]
    public class DoShapeModelMatch2D
    {
        private CreateShapeModelMethod createModel;
        private FindShapeModelMethod findModel;
        [NonSerialized]
        private HalconDotNet.HShapeModel HShapeModelID;
        [NonSerialized]
        private HalconDotNet.HShapeModel[] HShapeModelIDS;
        private double[] refPoint_row; // 用于记录模型的参考位置 
        private double[] refPoint_col;// 用于记录模型的参考位置 
        private double[] refPoint_rad;// 用于记录模型的参考位置 
        [NonSerialized]
        private MatchingResult _shapeMatchResult;
        [NonSerialized]
        private HImage templateImage;
        [NonSerialized]
        private HImage searchImageRegion;
        private int ModelNum = 0;
        public DoShapeModelMatch2D()
        {
            this.createModel = new CreateShapeModelMethod();
            this.findModel = new FindShapeModelMethod();
            this._ModelMacthType = enModelMatchType.shape_model;
            ///////////////////////////////////
            this.C_ShapeModelParam = new C_ShapeModelParam();
            this.F_ShapeModelParam = new F_ShapeModelParam();
        }
        [XmlIgnore]
        public HImage SearchImageRegion
        {
            get
            {
                this.searchImageRegion = findModel.SearchImageRegion;
                return searchImageRegion;
            }
            set
            {
                this.searchImageRegion = value;
            }
        }
        [XmlIgnore]
        public HImage TemplateImage
        {
            get
            {
                this.templateImage = createModel.TemplateImage;
                return this.templateImage;
            }
            set
            {
                this.templateImage = value;
            }
        }

        [NonSerialized]
        private HXLDCont _ShapeModelContour;
        [XmlIgnore]
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


        private enModelMatchType _ModelMacthType;
        public enModelMatchType ModelMacthType
        {
            get
            {
                return _ModelMacthType;
            }
            set
            {
                this._ModelMacthType = value;
                switch (this._ModelMacthType)
                {
                    case enModelMatchType.shape_model:
                        this.C_ShapeModelParam = new C_ShapeModelParam();
                        this.F_ShapeModelParam = new F_ShapeModelParam();
                        break;
                    case enModelMatchType.shape_model_xld:
                        this.C_ShapeModelParam = new C_ShapeModelParamXLD();
                        this.F_ShapeModelParam = new F_ShapeModelParam();
                        break;
                    case enModelMatchType.aniso_shape_model:
                        this.C_ShapeModelParam = new C_AnisoShapeModelParam();
                        this.F_ShapeModelParam = new F_AnisoShapeModelParam();
                        break;
                    case enModelMatchType.aniso_shape_model_xld:
                        this.C_ShapeModelParam = new C_AnisoShapeModelParamXLD();
                        this.F_ShapeModelParam = new F_AnisoShapeModelParam();
                        break;
                    case enModelMatchType.scaled_shape_model:
                        this.C_ShapeModelParam = new C_ScaledShapeModelParam();
                        this.F_ShapeModelParam = new F_ScaledShapeModelParam();
                        break;
                    case enModelMatchType.scaled_shape_model_xld:
                        this.C_ShapeModelParam = new C_ScaledShapeModelParamXLD();
                        this.F_ShapeModelParam = new F_ScaledShapeModelParam();
                        break;
                }
            }
        }

        public C_ShapeModelParamBase C_ShapeModelParam
        {
            get;
            set;
        }
        public F_ShapeModelParamBase F_ShapeModelParam
        {
            get;
            set;
        }


        public MatchingResult ShapeMatchResult //
        {
            get
            {
                return _shapeMatchResult;
            }
            set
            {
                this._shapeMatchResult = value;
            }
        }

        public bool CreateShapeModelFromImage(HImage image)
        {
            bool result = false;
            HTuple areaRow = 0, areaCol = 0; // 模板区域中心
            HHomMat2D hHomMat2D = new HHomMat2D();
            HXLDCont hXLDCont;
            if (image == null || !image.IsInitialized())
            {
                throw new ArgumentNullException("模板图像为空或未被初始化");
            }
            switch (ModelMacthType)
            {
                case enModelMatchType.aniso_shape_model:
                    if (HShapeModelID != null && HShapeModelID.IsInitialized()) HShapeModelID.ClearShapeModel();
                    HShapeModelID = createModel.create_aniso_shape_model(image, (C_AnisoShapeModelParam)this.C_ShapeModelParam);

                    break;
                case enModelMatchType.scaled_shape_model:
                    if (HShapeModelID != null && HShapeModelID.IsInitialized()) HShapeModelID.ClearShapeModel();
                    HShapeModelID = createModel.create_scaled_shape_model(image, (C_ScaledShapeModelParam)C_ShapeModelParam);
                    break;
                default:
                case enModelMatchType.shape_model:
                    if (HShapeModelID != null && HShapeModelID.IsInitialized()) HShapeModelID.ClearShapeModel();
                    HShapeModelID = createModel.create_shape_model(image, (C_ShapeModelParam)C_ShapeModelParam);
                    break;
            }
            if (this.HShapeModelID != null && this.HShapeModelID.IsInitialized())
            {
                this.ModelNum = 1;
                this._ShapeModelContour?.Dispose();
                this._ShapeModelContour = HShapeModelID.GetShapeModelContours(1);
                /////////////// 通过图像的方式来创建，一定要用区域重心来确定参考点 /////////////////
                createModel.TemplateRegion?.AreaCenter(out areaRow, out areaCol);
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
                //////////////////////////////
                _shapeMatchResult = new MatchingResult(1);
                for (int i = 0; i < areaRow.Length; i++)
                {
                    if (areaRow[i].D == 0 && areaCol[i].D == 0) continue;
                    hHomMat2D.VectorAngleToRigid(0, 0, 0, areaRow[i].D, areaCol[i].D, 0); //
                    HXLDCont xld = hHomMat2D.AffineTransContourXld(this._ShapeModelContour);
                    this._shapeMatchResult.MatchCont.AddXLDCont(xld);
                }
                result = true;
                LoggerHelper.Info(nameof(ModelMacthType) + "->模型创建成功");
            }
            else
            {
                LoggerHelper.Info(nameof(enModelMatchType.shape_model) + "->创建模型失败");
                result = false;
            }
            return result;
        }
        public bool CreateShapeModelsFromImage(HImage image)
        {
            bool result = false;
            HTuple areaRow = 0, areaCol = 0; // 模板区域中心
            HHomMat2D hHomMat2D = new HHomMat2D();
            //HXLDCont hXLDCont;
            if (image == null || !image.IsInitialized())
            {
                throw new ArgumentNullException("模板图像为空或未被初始化");
            }
            switch (ModelMacthType)
            {
                case enModelMatchType.aniso_shape_model:
                    if (HShapeModelIDS != null)
                    {
                        foreach (var item in HShapeModelIDS)
                        {
                            if (item != null && item.IsInitialized())
                                item.ClearShapeModel();
                        }
                    }
                    HShapeModelIDS = createModel.create_aniso_shape_models(image, (C_AnisoShapeModelParam)this.C_ShapeModelParam);
                    break;
                case enModelMatchType.scaled_shape_model:
                    if (HShapeModelIDS != null)
                    {
                        foreach (var item in HShapeModelIDS)
                        {
                            if (item != null && item.IsInitialized())
                                item.ClearShapeModel();
                        }
                    }
                    HShapeModelIDS = createModel.create_scaled_shape_models(image, (C_ScaledShapeModelParam)C_ShapeModelParam);
                    break;
                default:
                case enModelMatchType.shape_model:
                    if (HShapeModelIDS != null)
                    {
                        foreach (var item in HShapeModelIDS)
                        {
                            if (item != null && item.IsInitialized())
                                item.ClearShapeModel();
                        }
                    }
                    HShapeModelIDS = createModel.create_shape_models(image, (C_ShapeModelParam)C_ShapeModelParam);
                    break;
            }
            if (this.HShapeModelIDS != null && this.HShapeModelIDS.Length > 0)
            {
                this.ModelNum = this.HShapeModelIDS.Length;
                this._ShapeModelContour?.Dispose();
                foreach (var item in this.HShapeModelIDS)
                {
                    if (this._ShapeModelContour != null && this._ShapeModelContour.IsInitialized())
                        this._ShapeModelContour = this._ShapeModelContour.ConcatObj(item.GetShapeModelContours(1));
                    else
                        this._ShapeModelContour = item.GetShapeModelContours(1);
                }
                /////////////// 通过图像的方式来创建，一定要用区域重心来确定参考点 /////////////////
                createModel.TemplateRegion?.AreaCenter(out areaRow, out areaCol);
                switch (areaRow.Type)
                {
                    default:
                    case HTupleType.DOUBLE:
                        this.refPoint_row = areaRow.DArr;
                        this.refPoint_col = areaCol.DArr;
                        this.refPoint_rad = HTuple.TupleGenConst(areaRow.Length, 0.0).DArr;
                        break;
                    case HTupleType.LONG:
                        List<double> listRow = new List<double>();
                        List<double> listCol = new List<double>();
                        List<double> listAngle = new List<double>();
                        for (int i = 0; i < areaRow.Length; i++)
                        {
                            listRow.Add((double)areaRow[i]);
                            listCol.Add((double)areaCol[i]);
                            listAngle.Add(0.0);
                        }
                        this.refPoint_row = listRow.ToArray();
                        this.refPoint_col = listCol.ToArray();
                        this.refPoint_rad = listAngle.ToArray();
                        break;
                }
                //////////////////////////////
                _shapeMatchResult = new MatchingResult(1);
                for (int i = 0; i < areaRow.Length; i++)
                {
                    if (areaRow[i].D == 0 && areaCol[i].D == 0) continue;
                    hHomMat2D.VectorAngleToRigid(0, 0, 0, areaRow[i].D, areaCol[i].D, 0); //
                    HXLDCont xld = hHomMat2D.AffineTransContourXld(this.HShapeModelIDS[i].GetShapeModelContours(1));
                    this._shapeMatchResult.MatchCont.AddXLDCont(xld);
                }
                result = true;
                LoggerHelper.Info(nameof(ModelMacthType) + "->模型创建成功");
            }
            else
            {
                LoggerHelper.Info(nameof(enModelMatchType.shape_model) + "->创建模型失败");
                result = false;
            }
            return result;
        }

        public bool CreateShapeModelFromXLD(HImage image, HXLDCont hXLDCont)
        {
            bool result = false;
            if (hXLDCont == null || !hXLDCont.IsInitialized())
            {
                throw new ArgumentNullException("模板轮廓为空或未被初始化");
            }
            switch (ModelMacthType)
            {
                case enModelMatchType.aniso_shape_model_xld:
                    if (HShapeModelID != null && HShapeModelID.IsInitialized()) HShapeModelID.ClearShapeModel();
                    HShapeModelID = createModel.create_aniso_shape_model_xld(image, hXLDCont, (C_AnisoShapeModelParamXLD)C_ShapeModelParam);
                    if (HShapeModelID != null && HShapeModelID.IsInitialized())
                    {
                        this._ShapeModelContour?.Dispose();
                        this._ShapeModelContour = HShapeModelID.GetShapeModelContours(1); // 用XLD创建模型时，一定要使用
                        ////////////////////////////////
                        F_AnisoShapeModelParam param = ((F_AnisoShapeModelParam)F_ShapeModelParam).Clone();
                        param.SearchRegion.Clear();
                        result = findModel.find_aniso_shape_model(createModel.TemplateImage, this.HShapeModelID, new F_AnisoShapeModelParam(), out this._shapeMatchResult);
                    }
                    else
                    {
                        LoggerHelper.Info(nameof(ModelMacthType) + "创建模型失败");
                        result = false;
                    }
                    break;
                case enModelMatchType.scaled_shape_model_xld:
                    if (HShapeModelID != null && HShapeModelID.IsInitialized()) HShapeModelID.ClearShapeModel();
                    HShapeModelID = createModel.create_scaled_shape_model_xld(image, hXLDCont, (C_ScaledShapeModelParamXLD)C_ShapeModelParam);
                    if (HShapeModelID != null && HShapeModelID.IsInitialized())
                    {
                        this._ShapeModelContour?.Dispose();
                        this._ShapeModelContour = HShapeModelID.GetShapeModelContours(1);
                        ////////////////////////////////
                        F_ScaledShapeModelParam param = ((F_ScaledShapeModelParam)F_ShapeModelParam).Clone();
                        param.SearchRegion.Clear();
                        result = findModel.find_scaled_shape_model(createModel.TemplateImage, this.HShapeModelID, param, out this._shapeMatchResult);
                    }
                    else
                    {
                        LoggerHelper.Info(nameof(ModelMacthType) + "创建模型失败");
                        result = false;
                    }
                    break;
                default:
                case enModelMatchType.shape_model_xld:
                    if (HShapeModelID != null && HShapeModelID.IsInitialized()) HShapeModelID.ClearShapeModel();
                    HShapeModelID = createModel.create_shape_model_xld(image, hXLDCont, (C_ShapeModelParamXLD)C_ShapeModelParam); // 创建时指定模板图像
                    if (HShapeModelID != null && HShapeModelID.IsInitialized())
                    {
                        this._ShapeModelContour?.Dispose();
                        this.ShapeModelContour = HShapeModelID.GetShapeModelContours(1);
                        /////////////////////////////////////
                        F_ShapeModelParam param = ((F_ShapeModelParam)F_ShapeModelParam).Clone();
                        param.SearchRegion.Clear();
                        result = findModel.find_shape_model(createModel.TemplateImage, this.HShapeModelID, param, out this._shapeMatchResult);
                    }
                    else
                    {
                        LoggerHelper.Info(nameof(ModelMacthType) + "创建模型失败");
                        result = false;
                    }
                    break;
            }
            /////////////////////////
            if (this._shapeMatchResult.PixCoordSystem != null && this._shapeMatchResult.PixCoordSystem.Length > 0)
            {
                this.refPoint_row = new double[1];
                this.refPoint_col = new double[1];
                this.refPoint_rad = new double[1];
                this.refPoint_row[0] = _shapeMatchResult.PixCoordSystem[0].CurrentPoint.Row;
                this.refPoint_col[0] = _shapeMatchResult.PixCoordSystem[0].CurrentPoint.Col; // 记录模型参考位置 
                this.refPoint_rad[0] = _shapeMatchResult.PixCoordSystem[0].CurrentPoint.Rad;
                LoggerHelper.Info("创建模型后寻找模型参考点成功");
                result = true;
            }
            else
            {
                LoggerHelper.Info("创建模型后寻找模型参考点失败");
                result = false;
            }
            return result;

        }

        public bool CreateShapeModelsFromXLD(HImage image)
        {
            bool result = false;
            HTuple areaRow = new HTuple(), areaCol = new HTuple(); // 模板区域中心
            HHomMat2D hHomMat2D = new HHomMat2D();
            //HXLDCont hXLDCont;
            switch (ModelMacthType)
            {
                case enModelMatchType.aniso_shape_model_xld:
                    if (this.HShapeModelIDS != null)
                    {
                        foreach (var item in HShapeModelIDS)
                        {
                            if (item != null && item.IsInitialized())
                                item.ClearShapeModel();
                        }
                    }
                    this.HShapeModelIDS = createModel.create_aniso_shape_models_xld(image,(C_AnisoShapeModelParamXLD)C_ShapeModelParam);
                    break;
                case enModelMatchType.scaled_shape_model_xld:
                    if (this.HShapeModelIDS != null)
                    {
                        foreach (var item in HShapeModelIDS)
                        {
                            if (item != null && item.IsInitialized())
                                item.ClearShapeModel();
                        }
                    }
                    this.HShapeModelIDS = createModel.create_scaled_shape_models_xld(image, (C_ScaledShapeModelParamXLD)C_ShapeModelParam);
                    break;
                default:
                case enModelMatchType.shape_model_xld:
                    if (this.HShapeModelIDS != null)
                    {
                        foreach (var item in HShapeModelIDS)
                        {
                            if (item != null && item.IsInitialized())
                                item.ClearShapeModel();
                        }
                    }
                    this.HShapeModelIDS = createModel.create_shape_models_xld(image, (C_ShapeModelParamXLD)C_ShapeModelParam); // 创建时指定模板图像
                    break;
            }
            if (this.HShapeModelIDS != null)
            {
                this.ModelNum = this.HShapeModelIDS.Length;
                this._ShapeModelContour?.Dispose();
                foreach (var item in this.HShapeModelIDS)
                {
                    if (this._ShapeModelContour != null && this._ShapeModelContour.IsInitialized())
                        this._ShapeModelContour = this._ShapeModelContour.ConcatObj(item.GetShapeModelContours(1));
                    else
                        this._ShapeModelContour = item.GetShapeModelContours(1);
                }
                /////////////// 莸取模型轮廓的匹配点 /////////////////
                foreach (var item in this.HShapeModelIDS)
                {
                    switch (ModelMacthType)
                    {
                        case enModelMatchType.aniso_shape_model:
                        case enModelMatchType.aniso_shape_model_xld:
                            if (item != null && item.IsInitialized())
                                result = findModel.find_aniso_shape_model(image, item, (F_AnisoShapeModelParam)this.F_ShapeModelParam, out this._shapeMatchResult);
                            else
                                throw new ArgumentNullException("模型对象 item 为空或未初始化");
                            break;
                        case enModelMatchType.scaled_shape_model_xld:
                        case enModelMatchType.scaled_shape_model:
                            if (item != null && item.IsInitialized())
                                result = findModel.find_scaled_shape_model(image, item, (F_ScaledShapeModelParam)F_ShapeModelParam, out this._shapeMatchResult);
                            else
                                throw new ArgumentNullException("模型对象 item 为空或未初始化");
                            break;
                        default:
                        case enModelMatchType.shape_model_xld:
                        case enModelMatchType.shape_model:
                            if (item != null && item.IsInitialized())
                                result = findModel.find_shape_model(image, item, (F_ShapeModelParam)F_ShapeModelParam, out this._shapeMatchResult);
                            else
                                throw new ArgumentNullException("模型对象 item 为空或未初始化");
                            break;
                    }
                    if(result && this._shapeMatchResult.PixCoordSystem[0].Result)
                    {
                        areaRow.Append(this._shapeMatchResult.PixCoordSystem[0].CurrentPoint.Row);
                        areaCol.Append(this._shapeMatchResult.PixCoordSystem[0].CurrentPoint.Col);
                    }
                }
                //createModel.TemplateRegion?.AreaCenter(out areaRow, out areaCol);
                if(areaRow != null && areaRow.Length > 0)
                {
                    switch (areaRow.Type)
                    {
                        default:
                        case HTupleType.DOUBLE:
                            this.refPoint_row = areaRow.DArr;
                            this.refPoint_col = areaCol.DArr;
                            this.refPoint_rad = HTuple.TupleGenConst(areaRow.Length, 0.0).DArr;
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
                }
                //////////////////////////
                this._shapeMatchResult = new MatchingResult(1);
                for (int i = 0; i < areaRow.Length; i++)
                {
                    if (areaRow[i].D == 0 && areaCol[i].D == 0) continue;
                    hHomMat2D.VectorAngleToRigid(0, 0, 0, areaRow[i].D, areaCol[i].D, 0); //
                    HXLDCont xld = hHomMat2D.AffineTransContourXld(this.HShapeModelIDS[i].GetShapeModelContours(1));
                    this._shapeMatchResult.MatchCont.AddXLDCont(xld);
                }
                result = true;
            }
            else
            {
                LoggerHelper.Info(nameof(ModelMacthType) + "创建模型失败");
                result = false;
            }
            return result;

        }


        public bool FindShapeModel(ImageDataClass hImage)
        {
            bool result = false;
            switch (ModelMacthType)
            {
                case enModelMatchType.aniso_shape_model:
                case enModelMatchType.aniso_shape_model_xld:
                    if (this.HShapeModelID != null && this.HShapeModelID.IsInitialized())
                        result = findModel.find_aniso_shape_model(hImage.Image, this.HShapeModelID, (F_AnisoShapeModelParam)this.F_ShapeModelParam, out this._shapeMatchResult);
                    else
                        throw new ArgumentNullException("模型对象为空或未初始化");
                    break;
                case enModelMatchType.scaled_shape_model_xld:
                case enModelMatchType.scaled_shape_model:
                    if (this.HShapeModelID != null && this.HShapeModelID.IsInitialized())
                        result = findModel.find_scaled_shape_model(hImage.Image, this.HShapeModelID, (F_ScaledShapeModelParam)F_ShapeModelParam, out this._shapeMatchResult);
                    else
                        throw new ArgumentNullException("模型对象为空或未初始化");
                    break;
                default:
                case enModelMatchType.shape_model_xld:
                case enModelMatchType.shape_model:
                    if (this.HShapeModelID != null && this.HShapeModelID.IsInitialized())
                        result = findModel.find_shape_model(hImage.Image, this.HShapeModelID, (F_ShapeModelParam)F_ShapeModelParam, out this._shapeMatchResult);
                    else
                        throw new ArgumentNullException("模型对象为空或未初始化");
                    break;
            }
            // 属性的返回值不能当做一个变量来使用;
            if (result)
            {
                for (int i = 0; i < this._shapeMatchResult.PixCoordSystem.Length; i++)
                {
                    switch (this.F_ShapeModelParam.AdjustType)
                    {
                        case enAdjustType.X:  /// 如果不补正相应的对象，那么将参考点与当前点设置为一样即可
                            this._shapeMatchResult.PixCoordSystem[i].CurrentPoint.Row = this.refPoint_row[_shapeMatchResult.ModelIndex];
                            this._shapeMatchResult.PixCoordSystem[i].CurrentPoint.Rad = this.refPoint_rad[_shapeMatchResult.ModelIndex];
                            break;
                        case enAdjustType.Y:
                            this._shapeMatchResult.PixCoordSystem[i].CurrentPoint.Col = this.refPoint_col[_shapeMatchResult.ModelIndex];
                            this._shapeMatchResult.PixCoordSystem[i].CurrentPoint.Rad = this.refPoint_rad[_shapeMatchResult.ModelIndex];
                            break;
                        case enAdjustType.XY:
                            this._shapeMatchResult.PixCoordSystem[i].CurrentPoint.Rad = this.refPoint_rad[_shapeMatchResult.ModelIndex];
                            break;
                        case enAdjustType.Theta:
                            this._shapeMatchResult.PixCoordSystem[i].CurrentPoint.Row = this.refPoint_row[_shapeMatchResult.ModelIndex];
                            this._shapeMatchResult.PixCoordSystem[i].CurrentPoint.Col = this.refPoint_col[_shapeMatchResult.ModelIndex];
                            break;
                    }
                    this._shapeMatchResult.PixCoordSystem[i].ReferencePoint.Row = this.refPoint_row[_shapeMatchResult.ModelIndex]; // 以后的每次匹配都需要将参考点重新写入
                    this._shapeMatchResult.PixCoordSystem[i].ReferencePoint.Col = this.refPoint_col[_shapeMatchResult.ModelIndex];
                    this._shapeMatchResult.PixCoordSystem[i].ReferencePoint.Rad = this.refPoint_rad[_shapeMatchResult.ModelIndex];
                    this._shapeMatchResult.PixCoordSystem[i].ReferencePoint.CamParams = hImage.CamParams;
                    this._shapeMatchResult.PixCoordSystem[i].CurrentPoint.CamParams = hImage.CamParams;
                }
                LoggerHelper.Info("创建模型后寻找模型参考点成功");
                result = true;
            }
            else // 如果匹配失败，让当前点等于参考点
            {
                this._shapeMatchResult.PixCoordSystem[0].CurrentPoint.Row = this.refPoint_row[_shapeMatchResult.ModelIndex];
                this._shapeMatchResult.PixCoordSystem[0].CurrentPoint.Col = this.refPoint_col[_shapeMatchResult.ModelIndex];
                this._shapeMatchResult.PixCoordSystem[0].CurrentPoint.Rad = this.refPoint_rad[_shapeMatchResult.ModelIndex]; // 如果禁用角度补正，那么当前角度等于参考角度
                this._shapeMatchResult.PixCoordSystem[0].CurrentPoint.CamParams = hImage.CamParams;
                //////////////////////////////////
                this._shapeMatchResult.PixCoordSystem[0].ReferencePoint.Row = this.refPoint_row[_shapeMatchResult.ModelIndex]; // 以后的每次匹配都需要将参考点重新写入
                this._shapeMatchResult.PixCoordSystem[0].ReferencePoint.Col = this.refPoint_col[_shapeMatchResult.ModelIndex];
                this._shapeMatchResult.PixCoordSystem[0].ReferencePoint.Rad = this.refPoint_rad[_shapeMatchResult.ModelIndex];
                this._shapeMatchResult.PixCoordSystem[0].ReferencePoint.CamParams = hImage.CamParams;

                LoggerHelper.Info("创建模型后寻找模型参考点失败");
                result = false;
            }
            return result;
        }

        public bool FindShapeModels(ImageDataClass hImage)
        {
            bool result = false;
            switch (ModelMacthType)
            {
                case enModelMatchType.aniso_shape_model:
                case enModelMatchType.aniso_shape_model_xld:
                    if (this.HShapeModelIDS != null)
                        result = findModel.find_aniso_shape_models(hImage.Image, this.HShapeModelIDS, (F_AnisoShapeModelParam)this.F_ShapeModelParam, out this._shapeMatchResult);
                    else
                        throw new ArgumentNullException("模型对象为空或未初始化");
                    break;
                case enModelMatchType.scaled_shape_model_xld:
                case enModelMatchType.scaled_shape_model:
                    if (this.HShapeModelIDS != null)
                        result = findModel.find_scaled_shape_models(hImage.Image, this.HShapeModelIDS, (F_ScaledShapeModelParam)F_ShapeModelParam, out this._shapeMatchResult);
                    else
                        throw new ArgumentNullException("模型对象为空或未初始化");
                    break;
                default:
                case enModelMatchType.shape_model_xld:
                case enModelMatchType.shape_model:
                    if (this.HShapeModelIDS != null)
                        result = findModel.find_shape_models(hImage.Image, this.HShapeModelIDS, (F_ShapeModelParam)F_ShapeModelParam, out this._shapeMatchResult);
                    else
                        throw new ArgumentNullException("模型对象为空或未初始化");
                    break;
            }
            // 属性的返回值不能当做一个变量来使用;
            if (result)
            {
                for (int i = 0; i < this._shapeMatchResult.PixCoordSystem.Length; i++)
                {
                    switch (this.F_ShapeModelParam.AdjustType)
                    {
                        case enAdjustType.X:  /// 如果不补正相应的对象，那么将参考点与当前点设置为一样即可
                            this._shapeMatchResult.PixCoordSystem[i].CurrentPoint.Row = this.refPoint_row[_shapeMatchResult.ModelIndex];
                            this._shapeMatchResult.PixCoordSystem[i].CurrentPoint.Rad = this.refPoint_rad[_shapeMatchResult.ModelIndex];
                            break;
                        case enAdjustType.Y:
                            this._shapeMatchResult.PixCoordSystem[i].CurrentPoint.Col = this.refPoint_col[_shapeMatchResult.ModelIndex];
                            this._shapeMatchResult.PixCoordSystem[i].CurrentPoint.Rad = this.refPoint_rad[_shapeMatchResult.ModelIndex];
                            break;
                        case enAdjustType.XY:
                            this._shapeMatchResult.PixCoordSystem[i].CurrentPoint.Rad = this.refPoint_rad[_shapeMatchResult.ModelIndex];
                            break;
                        case enAdjustType.Theta:
                            this._shapeMatchResult.PixCoordSystem[i].CurrentPoint.Row = this.refPoint_row[_shapeMatchResult.ModelIndex];
                            this._shapeMatchResult.PixCoordSystem[i].CurrentPoint.Col = this.refPoint_col[_shapeMatchResult.ModelIndex];
                            break;
                    }
                    this._shapeMatchResult.PixCoordSystem[i].ReferencePoint.Row = this.refPoint_row[_shapeMatchResult.ModelIndex]; // 以后的每次匹配都需要将参考点重新写入
                    this._shapeMatchResult.PixCoordSystem[i].ReferencePoint.Col = this.refPoint_col[_shapeMatchResult.ModelIndex];
                    this._shapeMatchResult.PixCoordSystem[i].ReferencePoint.Rad = this.refPoint_rad[_shapeMatchResult.ModelIndex];
                    this._shapeMatchResult.PixCoordSystem[i].ReferencePoint.CamParams = hImage.CamParams;
                    this._shapeMatchResult.PixCoordSystem[i].CurrentPoint.CamParams = hImage.CamParams;
                }
                LoggerHelper.Info("创建模型后寻找模型参考点成功");
                result = true;
            }
            else // 如果匹配失败，让当前点等于参考点
            {
                this._shapeMatchResult.PixCoordSystem[0].CurrentPoint.Row = this.refPoint_row[_shapeMatchResult.ModelIndex];
                this._shapeMatchResult.PixCoordSystem[0].CurrentPoint.Col = this.refPoint_col[_shapeMatchResult.ModelIndex];
                this._shapeMatchResult.PixCoordSystem[0].CurrentPoint.Rad = this.refPoint_rad[_shapeMatchResult.ModelIndex]; // 如果禁用角度补正，那么当前角度等于参考角度
                this._shapeMatchResult.PixCoordSystem[0].CurrentPoint.CamParams = hImage.CamParams;
                //////////////////////////////////
                this._shapeMatchResult.PixCoordSystem[0].ReferencePoint.Row = this.refPoint_row[_shapeMatchResult.ModelIndex]; // 以后的每次匹配都需要将参考点重新写入
                this._shapeMatchResult.PixCoordSystem[0].ReferencePoint.Col = this.refPoint_col[_shapeMatchResult.ModelIndex];
                this._shapeMatchResult.PixCoordSystem[0].ReferencePoint.Rad = this.refPoint_rad[_shapeMatchResult.ModelIndex];
                this._shapeMatchResult.PixCoordSystem[0].ReferencePoint.CamParams = hImage.CamParams;
                //////////////////////////////////
                LoggerHelper.Info("创建模型后寻找模型参考点失败");
                result = false;
            }
            return result;
        }

        public void ClearShapeModel()
        {
            if (this.HShapeModelID != null && this.HShapeModelID.IsInitialized())
                this.HShapeModelID.ClearShapeModel();
            ///////////////////////////////
            if (this.HShapeModelIDS != null)
            {
                foreach (var item in this.HShapeModelIDS)
                {
                    if (item != null && item.IsInitialized())
                        item.ClearShapeModel();
                }
            }
        }

        public void RemoveAtShapeModel(int index)
        {
            ///////////////////////////////
            if (this.HShapeModelIDS != null && this.HShapeModelIDS.Length > 0)
            {
                List<HShapeModel> list = new List<HShapeModel>();
                list.AddRange(this.HShapeModelIDS);
                if (list.Count > index)
                    list.RemoveAt(index);
                this.HShapeModelIDS = list.ToArray();
            }
        }

        public void SaveHShapeModel(string filePath)
        {
            if (this.HShapeModelID != null && this.HShapeModelID.IsInitialized())
                this.HShapeModelID.WriteShapeModel(filePath);
            //////////////////////////////////////////////////
            if (this.HShapeModelIDS != null)
            {
                int index = 1;
                foreach (var item in this.HShapeModelIDS)
                {
                    if (item != null && item.IsInitialized())
                        item.WriteShapeModel(filePath.Insert(filePath.LastIndexOf('.'), index.ToString()));
                    index++;
                }
            }
        }

        public void ReadHShapeModel(string filePath)
        {
            //if (!File.Exists(filePath)) return;
            if (this.HShapeModelID == null)
                this.HShapeModelID = new HShapeModel();
            else
            {
                if (this.HShapeModelID.IsInitialized())
                    this.HShapeModelID.ClearShapeModel();
                this.HShapeModelID = new HShapeModel();
            }
            if (File.Exists(filePath))
                this.HShapeModelID.ReadShapeModel(filePath);

            //////////////////////////////////////////////////  多模型型  ///////////////////////////////
            if (this.HShapeModelIDS != null)
            {
                foreach (var item in this.HShapeModelIDS)
                {
                    if (item != null && item.IsInitialized())
                        item.ClearShapeModel();
                }
            }
            this.HShapeModelIDS = new HShapeModel[this.ModelNum];
            for (int index = 0; index < this.ModelNum; index++)
            {
                if (File.Exists(filePath.Insert(filePath.LastIndexOf('.'), (index + 1).ToString())))
                {
                    this.HShapeModelIDS[index] = new HShapeModel();
                    this.HShapeModelIDS[index].ReadShapeModel(filePath.Insert(filePath.LastIndexOf('.'), (index + 1).ToString()));
                }
            }
        }



    }





}

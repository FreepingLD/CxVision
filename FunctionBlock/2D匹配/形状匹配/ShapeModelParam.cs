using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace FunctionBlock
{

    #region 创建模型参数
    [Serializable]
    public class C_AnisoShapeModelParam : C_ShapeModelParamBase
    {
        protected double _ScaleRMin;
        public double ScaleRMin
        {
            get
            {
                return _ScaleRMin;
            }
            set
            {
                this._ScaleRMin = value;
            }
        }

        protected double _ScaleRMax;
        public double ScaleRMax
        {
            get
            {
                return _ScaleRMax;
            }
            set
            {
                this._ScaleRMax = value;
            }
        }

        protected object _ScaleRStep;
        public object ScaleRStep
        {
            get
            {
                return _ScaleRStep;
            }
            set
            {
                if (value != null)
                {
                    double result;
                    if (double.TryParse(value.ToString(), out result))
                        _ScaleRStep = result;
                    else
                        _ScaleRStep = value;
                }
            }
        }

        public double _ScaleCMin;
        public double ScaleCMin
        {
            get
            {
                return _ScaleCMin;
            }
            set
            {
                this._ScaleCMin = value;
            }
        }

        protected double _ScaleCMax;
        public double ScaleCMax
        {
            get
            {
                return _ScaleCMax;
            }
            set
            {
                this._ScaleCMax = value;
            }
        }

        protected object _ScaleCStep;
        public object ScaleCStep
        {
            get
            {
                return _ScaleCStep;
            }
            set
            {
                if (value != null)
                {
                    double result;
                    if (double.TryParse(value.ToString(), out result))
                        _ScaleCStep = result;
                    else
                        _ScaleCStep = value;
                }
            }
        }

        protected int _Contrast;
        public int Contrast
        {
            get
            {
                return _Contrast;
            }
            set
            {
                this._Contrast = value;
            }
        }
        public int MinLenght { get; set; }
        protected string _MinContrast;
        public string MinContrast
        {
            get
            {
                return _MinContrast;
            }
            set
            {
                this._MinContrast = value;
            }
        }


        public C_AnisoShapeModelParam()
        {
            _NumLevels = "auto";
            _AngleStart = -10;
            _AngleExtent = 10;
            _AngleStep = "auto";
            _ScaleRMin = 0.9;
            _ScaleRMax = 1.1;
            _ScaleRStep = "auto";
            _ScaleCMin = 0.9;
            _ScaleCMax = 1.1;
            _ScaleCStep = "auto";
            _Optimization = "auto,pregeneration";
            _Metric = "use_polarity";
            _Contrast = 15;
            MinLenght = 10;
            _MinContrast = "auto";
            TemplateRegion = new BindingList<ModelParam>();
        }


    }
    [Serializable]
    public class C_AnisoShapeModelParamXLD : C_ShapeModelParamBase
    {
        protected double _ScaleRMin;
        public double ScaleRMin
        {
            get
            {
                return _ScaleRMin;
            }
            set
            {
                this._ScaleRMin = value;
            }
        }

        protected double _ScaleRMax;
        public double ScaleRMax
        {
            get
            {
                return _ScaleRMax;
            }
            set
            {
                this._ScaleRMax = value;
            }
        }

        protected object _ScaleRStep;
        public object ScaleRStep
        {
            get
            {
                return _ScaleCStep;
            }
            set
            {
                if (value != null)
                {
                    double result;
                    if (double.TryParse(value.ToString(), out result))
                        _ScaleRStep = result;
                    else
                        _ScaleRStep = value;
                }
            }
        }

        protected double _ScaleCMin;
        public double ScaleCMin
        {
            get
            {
                return _ScaleCMin;
            }
            set
            {
                this._ScaleCMin = value;
            }
        }
        protected double _ScaleCMax;
        public double ScaleCMax
        {
            get
            {
                return _ScaleCMax;
            }
            set
            {
                this._ScaleCMax = value;
            }
        }

        protected object _ScaleCStep;
        public object ScaleCStep
        {
            get
            {
                return _ScaleCStep;
            }
            set
            {
                if (value != null)
                {
                    double result;
                    if (double.TryParse(value.ToString(), out result))
                        _ScaleCStep = result;
                    else
                        _ScaleCStep = value;
                }
            }
        }


        protected int _MinContrast;
        public int MinContrast
        {
            get
            {
                return _MinContrast;
            }
            set
            {
                this._MinContrast = value;
            }
        }


        public C_AnisoShapeModelParamXLD()
        {
            // 创建模型参数
            _NumLevels = "auto";
            _AngleStart = -10;
            _AngleExtent = 10;
            _AngleStep = "auto";
            _ScaleRMin = 0.9;
            _ScaleRMax = 1.1;
            _ScaleRStep = "auto";
            _ScaleCMin = 0.9;
            _ScaleCMax = 1.1;
            _ScaleCStep = "auto";
            _Optimization = "auto,pregeneration";
            _Metric = "ignore_local_polarity";
            _MinContrast = 5;
            TemplateRegion = new BindingList<ModelParam>();
        }


    }
    [Serializable]
    public class C_ScaledShapeModelParam : C_ShapeModelParamBase
    {
        protected double _ScaleMin;
        public double ScaleMin
        {
            get
            {
                return _ScaleMin;
            }
            set
            {
                this._ScaleMin = value;
            }
        }

        protected double _ScaleMax;
        public double ScaleMax
        {
            get
            {
                return _ScaleMax;
            }
            set
            {
                this._ScaleMax = value;
            }
        }

        protected object _ScaleStep;
        public object ScaleStep
        {
            get
            {
                return _ScaleStep;
            }
            set
            {
                if (value != null)
                {
                    double result;
                    if (double.TryParse(value.ToString(), out result))
                        _ScaleStep = result;
                    else
                        _ScaleStep = value;
                }
            }
        }

        protected int _Contrast;
        public int Contrast
        {
            get
            {
                return _Contrast;
            }
            set
            {
                this._Contrast = value;
            }
        }

        protected string _MinContrast;
        public string MinContrast
        {
            get
            {
                return _MinContrast;
            }
            set
            {
                this._MinContrast = value;
            }
        }

        public int MinLenght
        {
            get;
            set;
        }

        public C_ScaledShapeModelParam()
        {
            // 创建模型参数
            _NumLevels = "auto";
            _AngleStart = -10;
            _AngleExtent = 10;
            _AngleStep = "auto";
            _ScaleMin = 0.8;
            _ScaleMax = 1.2;
            _ScaleStep = "auto";
            _Optimization = "auto,pregeneration";
            _Metric = "use_polarity";
            _Contrast = 15;
            MinLenght = 10;
            _MinContrast = "auto";
            TemplateRegion = new BindingList<ModelParam>();
        }


    }
    [Serializable]
    public class C_ScaledShapeModelParamXLD : C_ShapeModelParamBase
    {
        protected double _ScaleMin;
        public double ScaleMin
        {
            get
            {
                return _ScaleMin;
            }
            set
            {
                this._ScaleMin = value;
            }
        }

        protected double _ScaleMax;
        public double ScaleMax
        {
            get
            {
                return _ScaleMax;
            }
            set
            {
                this._ScaleMax = value;
            }
        }

        protected object _ScaleStep;
        public object ScaleStep
        {
            get
            {
                return _ScaleStep;
            }
            set
            {
                if (value != null)
                {
                    double result;
                    if (double.TryParse(value.ToString(), out result))
                        _ScaleStep = result;
                    else
                        _ScaleStep = value;
                }
            }
        }

        protected int _MinContrast;
        public int MinContrast
        {
            get
            {
                return _MinContrast;
            }
            set
            {
                this._MinContrast = value;
            }
        }

        public C_ScaledShapeModelParamXLD()
        {
            // 创建模型参数
            _NumLevels = "auto";
            _AngleStart = -10;
            _AngleExtent = 10;
            _AngleStep = "auto";
            _ScaleMin = 0.8;
            _ScaleMax = 1.2;
            _ScaleStep = "auto";
            _Optimization = "auto,pregeneration";
            _Metric = "ignore_local_polarity";
            _MinContrast = 5;
            TemplateRegion = new BindingList<ModelParam>();
        }



    }
    [Serializable]
    public class C_ShapeModelParam : C_ShapeModelParamBase
    {

        private int _Contrast;
        public int Contrast
        {
            get
            {
                return _Contrast;
            }
            set
            {
                this._Contrast = value;
            }
        }

        private string _MinContrast;
        public string MinContrast
        {
            get
            {
                return _MinContrast;
            }
            set
            {
                this._MinContrast = value;
            }
        }

        public int MinLenght
        {
            get;
            set;
        }

        public C_ShapeModelParam()
        {
            // 创建模型参数
            this._NumLevels = "auto";
            this._AngleStart = -10;
            this._AngleExtent = 10;
            this._AngleStep = "auto";
            this._Optimization = "auto,pregeneration";
            this._Metric = "use_polarity";
            this._Contrast = 15;
            this._MinContrast = "auto";
            this.MinLenght = 10;
            this.TemplateRegion = new BindingList<ModelParam>();
        }
    }
    [Serializable]
    public class C_ShapeModelParamXLD : C_ShapeModelParamBase
    {
        public int _MinContrast;
        public int MinContrast
        {
            get
            {
                return _MinContrast;
            }
            set
            {
                _MinContrast = value;
            }
        }
        public C_ShapeModelParamXLD()
        {
            // 创建模型参数
            _NumLevels = "auto";
            _AngleStart = -10;
            _AngleExtent = 10;
            _AngleStep = "auto";
            _Optimization = "auto,pregeneration";
            _Metric = "ignore_local_polarity";
            _MinContrast = 5;
            TemplateRegion = new BindingList<ModelParam>();
        }
    }

    [Serializable]
    public class C_ShapeModelParamBase
    {
        protected object _NumLevels;
        public object NumLevels
        {
            get
            {
                return _NumLevels;
            }
            set
            {
                if (value != null)
                {
                    int result;
                    if (int.TryParse(value.ToString(), out result))
                        _NumLevels = result;
                    else
                        _NumLevels = value;
                }
            }
        }

        protected double _AngleStart;
        public double AngleStart
        {
            get
            {
                return _AngleStart;
            }
            set
            {
                _AngleStart = value;
            }
        }

        protected double _AngleExtent;
        public double AngleExtent
        {
            get
            {
                return _AngleExtent;
            }
            set
            {
                this._AngleExtent = value;
            }
        }

        protected object _AngleStep;
        public object AngleStep
        {
            get
            {
                return _AngleStep;
            }
            set
            {
                if (value != null)
                {
                    double result;
                    if (double.TryParse(value.ToString(), out result))
                        _AngleStep = result;
                    else
                        _AngleStep = value;
                }
            }
        }

        protected string _Optimization;
        public string Optimization
        {
            get
            {
                return _Optimization;
            }
            set
            {
                this._Optimization = value;
            }
        }

        protected string _Metric;
        public string Metric
        {
            get
            {
                return _Metric;
            }
            set
            {
                this._Metric = value;
            }
        }
        public BindingList<ModelParam> TemplateRegion
        {
            get;
            set;
        }

    }


    #endregion

    #region 寻找模型参数
    [Serializable]
    public class F_AnisoShapeModelParam : F_ShapeModelParamBase
    {

        protected double _ScaleRMin;
        public double ScaleRMin
        {
            get
            {
                return _ScaleRMin;
            }
            set
            {
                this._ScaleRMin = value;
            }
        }

        protected double _ScaleRMax;
        public double ScaleRMax
        {
            get
            {
                return _ScaleRMax;
            }
            set
            {
                this._ScaleRMax = value;
            }
        }

        protected double _ScaleCMin;
        public double ScaleCMin
        {
            get
            {
                return _ScaleCMin;
            }
            set
            {
                this._ScaleCMin = value;
            }
        }

        protected double _ScaleCMax;
        public double ScaleCMax
        {
            get
            {
                return _ScaleCMax;
            }
            set
            {
                this._ScaleCMax = value;
            }
        }

        public F_AnisoShapeModelParam()
        {
            /// 模型匹配参数
            _AngleStart = -10;
            _AngleExtent = 10;
            _ScaleRMin = 0.9;
            _ScaleRMax = 1.1;
            _ScaleCMin = 0.9;
            _ScaleCMax = 1.1;
            _MinScore = 0.7;
            _NumMatches = 1;
            _MaxOverlap = 0.5;
            _SubPixel = enInterpolationMethod.least_squares; // "least_squares_high";
            _TimeOut = 5000; // 这里不需要设置搜索的金字塔层级数，由创建时自动设置
            _Greediness = 0.9;
            SearchRegion = new BindingList<ModelParam>();
        }

        public F_AnisoShapeModelParam Clone()
        {
            F_AnisoShapeModelParam param = new F_AnisoShapeModelParam();
            /// 模型匹配参数
            param._AngleStart = this._AngleStart;
            param._AngleExtent = this._AngleExtent;
            param._ScaleRMin = this._ScaleRMin;
            param._ScaleRMax = this._ScaleRMax;
            param._ScaleCMin = this._ScaleCMin;
            param._ScaleCMax = this._ScaleCMax;
            param._MinScore = this._MinScore;
            param._NumMatches = this._NumMatches;
            param._MaxOverlap = this._MaxOverlap;
            param._SubPixel = this._SubPixel; // "least_squares_high";
            param._TimeOut = this._TimeOut; // 这里不需要设置搜索的金字塔层级数，由创建时自动设置
            param._Greediness = this._Greediness;
            param.SearchRegion = this.SearchRegion;
            return param;
        }

    }
    [Serializable]
    public class F_ScaledShapeModelParam : F_ShapeModelParamBase
    {

        protected double _ScaleMin;
        public double ScaleMin
        {
            get
            {
                return _ScaleMin;
            }
            set
            {
                this._ScaleMin = value;
            }
        }

        protected double _ScaleMax;
        public double ScaleMax
        {
            get
            {
                return _ScaleMax;
            }
            set
            {
                this._ScaleMax = value;
            }

        }


        public F_ScaledShapeModelParam()
        {
            /// 模型匹配参数
            _AngleStart = -10;
            _AngleExtent = 10;
            _ScaleMin = 0.9;
            _ScaleMax = 1.1;
            _MinScore = 0.7;
            _NumMatches = 1;
            _MaxOverlap = 0.5;
            _SubPixel = enInterpolationMethod.least_squares;// "least_squares_high";
            _TimeOut = 5000; // 这里不需要设置搜索的金字塔层级数，由创建时自动设置
            _Greediness = 0.9;
            SearchRegion = new BindingList<ModelParam>();
        }

        public F_ScaledShapeModelParam Clone()
        {
            F_ScaledShapeModelParam param = new F_ScaledShapeModelParam();
            /// 模型匹配参数
            param._AngleStart = this._AngleStart;
            param._AngleExtent = this._AngleExtent;
            param._ScaleMin = this._ScaleMin;
            param._ScaleMax = this._ScaleMax;
            param._MinScore = this._MinScore;
            param._NumMatches = this._NumMatches;
            param._MaxOverlap = this._MaxOverlap;
            param._SubPixel = this._SubPixel;// "least_squares_high";
            param._TimeOut = this._TimeOut; // 这里不需要设置搜索的金字塔层级数，由创建时自动设置
            param._Greediness = this._Greediness;
            param.SearchRegion = this.SearchRegion;
            return param;
        }

    }
    [Serializable]
    public class F_ShapeModelParam : F_ShapeModelParamBase
    {

        public F_ShapeModelParam()
        {
            /// 模型匹配参数
            _AngleStart = -10;
            _AngleExtent = 10;
            _MinScore = 0.7;
            _NumMatches = 1;
            _MaxOverlap = 0.5;
            _SubPixel = enInterpolationMethod.least_squares;
            _TimeOut = 5000; // 这里不需要设置搜索的金字塔层级数，由创建时自动设置
            _Greediness = 0.9;
            //_Result = new ShapeMatchingResult();
            SearchRegion = new BindingList<ModelParam>();
        }

        public F_ShapeModelParam Clone()
        {
            F_ShapeModelParam param = new F_ShapeModelParam();
            /// 模型匹配参数
            param._AngleStart = this._AngleStart;
            param._AngleExtent = this._AngleExtent;
            param._MinScore = this._MinScore;
            param.NumMatches = this.NumMatches;
            param._MaxOverlap = this._MaxOverlap;
            param._SubPixel = this._SubPixel;
            param._TimeOut = this._TimeOut; // 这里不需要设置搜索的金字塔层级数，由创建时自动设置
            param._Greediness = this._Greediness;
            param.SearchRegion = this.SearchRegion;
            return param;
        }

    }
    [Serializable]
    public class F_ShapeModelParamBase
    {
        public double _AngleStart;
        public double AngleStart
        {
            get
            {
                return _AngleStart;
            }
            set
            {
                this._AngleStart = value;
            }
        }


        protected double _AngleExtent;
        public double AngleExtent
        {
            get
            {
                return _AngleExtent;
            }
            set
            {
                this._AngleExtent = value;
            }
        }

        protected double _MinScore;
        public double MinScore
        {
            get
            {
                return _MinScore;
            }
            set
            {
                this._MinScore = value;
            }
        }


        protected int _NumMatches;
        public int NumMatches
        {
            get
            {
                return _NumMatches;
            }
            set
            {
                this._NumMatches = value;
            }
        }

        protected double _MaxOverlap;
        public double MaxOverlap
        {
            get
            {
                return this._MaxOverlap;
            }
            set
            {
                this._MaxOverlap = value;
            }
        }

        protected enInterpolationMethod _SubPixel;
        public enInterpolationMethod SubPixel
        {
            get
            {
                return _SubPixel;
            }
            set
            {
                this._SubPixel = value;
            }
        }


        protected int _TimeOut;
        public int TimeOut
        {
            get
            {
                return _TimeOut;
            }
            set
            {
                this._TimeOut = value;
            }
        }

        protected double _Greediness;
        public double Greediness
        {
            get
            {
                return _Greediness;
            }
            set
            {
                this._Greediness = value;
            }
        }
        public bool DisableAngle { get; set; }

        public enFilterMethod FilterMethod
        {
            get;
            set;
        }
        public enSearchMethod SearchMethod
        {
            get;
            set;
        }

        public enSortMethod SortMethod { get; set; }

        /// <summary>
        /// 补正类型
        /// </summary>
        public enAdjustType AdjustType { get; set; }
        public enMatchMode MatchMode {get;set;}
        public bool FiilUp { get; set; }
        public userPixCoordSystem PixCoordSystem { get; set; }
        public BindingList<ModelParam> SearchRegion
        {
            get;
            set;
        }

        public F_ShapeModelParamBase()
        {
            this.DisableAngle = false;
            this.FilterMethod = enFilterMethod.NONE;
            this.SearchMethod = enSearchMethod.NONE;
            this.PixCoordSystem = new userPixCoordSystem();
            this.AdjustType = enAdjustType.XYTheta;
            this.MatchMode = enMatchMode.正常模式;
            this.NumMatches = 1;
            this.SortMethod = enSortMethod.NONE;
            this.FiilUp = false;
        }

    }


    #endregion

    [Serializable]
    public enum enInterpolationMethod
    {
        least_squares,
        interpolation,
        least_squares_high,
        least_squares_very_high,
        max_deformation1,
        max_deformation2,
        max_deformation3,
        max_deformation4,
        max_deformation5,
        max_deformation6,
        none,
    }
    [Serializable]
    public enum enMatchMode
    {
        正常模式,
        兼容模式,
    }
    [Serializable]
    public struct MatchingResult
    {
        public userPixCoordSystem[] PixCoordSystem;
        public double[] MatchScore;
        [NonSerialized]
        public XldDataClass MatchCont;

        public int ModelIndex; // 匹配的模型索引
        public MatchingResult(int lenght = 1)
        {
            this.PixCoordSystem = new userPixCoordSystem[lenght];
            this.MatchScore = new double[lenght];
            this.MatchCont = new XldDataClass();
            this.ModelIndex = 0;
            for (int i = 0; i < lenght; i++)
            {
                this.MatchScore[i] = 0;
                this.PixCoordSystem[i] = new userPixCoordSystem();
            }
        }

    }
    public struct LocalDeformableMatchingResult
    {
        public userPixCoordSystem PixCoordSystem;
        public double MatchScore;
        [NonSerialized]
        public XldDataClass MatchCont;
        [NonSerialized]
        public HalconDotNet.HImage RectifiedImage;

        public LocalDeformableMatchingResult(int lenght = 1)
        {
            this.PixCoordSystem = new userPixCoordSystem();
            this.MatchScore = 0;
            this.MatchCont = new XldDataClass();
            this.RectifiedImage = new HalconDotNet.HImage();
        }


    }

    [Serializable]
    public enum enModelMatchType
    {
        aniso_shape_model,
        aniso_shape_model_xld,
        scaled_shape_model,
        scaled_shape_model_xld,
        shape_model,
        shape_model_xld,
    }
    public enum enLocalDeformableType
    {
        create_local_deformable_model,
        create_local_deformable_model_xld,
    }


    public enum enFilterMethod
    {
        NONE,
        最小中心距,
    }
    public enum enSearchMethod
    {
        NONE,
        Single,
        Multy,
    }
    public enum enMatchMethod
    {
        匹配定位,
        匹配测量,
    }
    public enum enSortMethod
    {
        NONE,
        行排序,
        列排序,
        行列排序
    }
    [Serializable]
    public class ModelParam
    {

        private enShapeType shapeType = enShapeType.矩形1;
        private enModelSign _modelSign  = enModelSign.模型1;

        [DisplayNameAttribute("模型对象")]
        public enModelSign ModelSign
        {
            get
            {
                return _modelSign;
            }

            set
            {
                _modelSign = value;
            }
        }

        [DisplayNameAttribute("形状类型")]
        public enShapeType ShapeType
        {
            get
            {
                return shapeType;
            }

            set
            {
                shapeType = value;
            }
        }

        /// <summary>
        /// 2D 中使用像素形状
        /// </summary>
        [DisplayNameAttribute("形状参数")]
        public PixROI RoiShape { get; set; }

        public ModelParam()
        {

        }

        public ModelParam(PixROI RoiShape)
        {
            this.RoiShape = RoiShape;
        }


    }

    public enum enModelSign
    {
        模型1,
        模型2,
        模型3,
        模型4,
        模型5,
    }


}

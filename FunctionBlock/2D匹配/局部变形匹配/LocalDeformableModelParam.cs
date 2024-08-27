using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace FunctionBlock
{

    [Serializable]
    public class CreateLocalDeformableModelParam
    {
        protected string _NumLevels;
        public string NumLevels
        {
            get
            {
                return _NumLevels;
            }
            set
            {
                _NumLevels = value;
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

        protected string _AngleStep;
        public string AngleStep
        {
            get
            {
                return _AngleStep;
            }
            set
            {
                _AngleStep = value;
            }
        }
        public double ScaleRMin
        {
            get;
            set;
        }
        public double ScaleRMax
        {
            get;
            set;
        }

        protected string _ScaleRStep;
        public string ScaleRStep
        {
            get
            {
                return _ScaleRStep;
            }
            set
            {
                        _ScaleRStep = value;
            }
        }
        public double ScaleCMin
        {
            get;
            set;
        }
        public double ScaleCMax
        {
            get;
            set;
        }

        protected string _ScaleCStep;
        public string ScaleCStep
        {
            get
            {
                return _ScaleCStep;
            }
            set
            {
                        _ScaleCStep = value;
            }
        }
        public string Optimization
        {
            get;
            set;
        }
        public string Metric
        {
            get;
            set;
        }

        public string _Contrast;
        public string Contrast
        {
            get
            {
                return _Contrast;
            }
            set
            {
                _Contrast = value;
            }
        }

        public string _MinContrast;
        public string MinContrast
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

        public BindingList<drawPixRect1> TemplateRegion
        {
            get;
            set;
        }

        public CreateLocalDeformableModelParam()
        {
            NumLevels = "auto";
            AngleStart = -10;
            AngleExtent = 10;
            AngleStep = "auto";
            ScaleRMin = 0.9;
            ScaleRMax = 0;
            ScaleRStep = "auto";
            ScaleCMin = 0.9;
            ScaleCMax = 1.1;
            ScaleCStep = "auto";
            Optimization = "auto";
            Metric = "use_polarity";
            Contrast = "auto";
            MinContrast = "auto";
            TemplateRegion = new BindingList<drawPixRect1>();
        }
    }



    [Serializable]
    public class FindLocalDeformableModelParam
    {

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
                _AngleExtent = value;
            }
        }

        protected double _ScaleRMin;
        public double ScaleRMin
        {
            get
            {
                return _ScaleRMin;
            }

            set
            {
                _ScaleRMin = value;
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
                _ScaleRMax = value;
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
                _ScaleCMin = value;
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
                _ScaleCMax = value;
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
                _MinScore = value;
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
                _NumMatches = value;
            }
        }

        protected double _MaxOverlap;
        public double MaxOverlap
        {
            get
            {
                return _MaxOverlap;
            }

            set
            {
                _MaxOverlap = value;
            }
        }

        protected int _NumLevels;
        public int NumLevels
        {
            get
            {
                return _NumLevels;
            }

            set
            {
                _NumLevels = value;
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
                _Greediness = value;
            }
        }



        protected string[] _ParamName;
        public string[] ParamName
        {
            get
            {
                return _ParamName;
            }

            set
            {
                _ParamName = value;
            }
        }

        protected string[] _ParamValue;
        public string[] ParamValue { get => _ParamValue; set => _ParamValue = value; }


        protected string[] _ResultType;
        protected string[] ResultType { get => _ResultType; set => _ResultType = value; }

        protected int _timeOut;
        public int TimeOut
        {
            get
            {
                return _timeOut;
            }
            set
            {
                this._timeOut = value;
            }
        }

        public userPixCoordSystem PixCoordSystem { get; set; }
        public BindingList<drawPixRect1> SearchRegion
        {
            get;
            set;
        }



        public FindLocalDeformableModelParam()
        {
            /// 模型匹配参数
            AngleStart = -10; // 从负 -10度到 +10度范围内
            AngleExtent = 10;
            ScaleRMin = 0.9;
            ScaleRMax = 1.1;
            ScaleCMin = 0.9;
            ScaleCMax = 1.1;
            MinScore = 0.5;
            NumMatches = 1;
            MaxOverlap = 0.5;
            NumLevels = 0;
            Greediness = 0.9;
            ResultType = new string[] { "deformed_contours", "image_rectified" };
            ParamName = new string[] { "subpixel" };
            ParamValue = new string[] { "least_squares_high" };
            TimeOut = 5000;
            SearchRegion = new BindingList<drawPixRect1>();
            PixCoordSystem = new userPixCoordSystem();
        }

    }






}

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
    public class CreateNccModelParam
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
        public enSortMethod SortMethod { get; set; }
        public BindingList<ModelParam> TemplateRegion
        {
            get;
            set;
        }




        public CreateNccModelParam()
        {
            this._NumLevels = "auto";
            this._AngleStart = -10;
            this._AngleExtent = 10;
            this._AngleStep = "auto";
            this._Metric = "use_polarity";
            this.TemplateRegion = new BindingList<ModelParam>();
            this.SortMethod = enSortMethod.NONE;
        }


    }



    [Serializable]
    public class FindNccModelParam
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

        protected string _SubPixel;
        public string SubPixel
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

        protected int _NumLevels;
        public int NumLevels
        {
            get
            {
                return _NumLevels;
            }
            set
            {
                int result;
                if (int.TryParse(value.ToString(), out result))
                    _NumLevels = result;
                else
                    _NumLevels = value;
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

        /// <summary>
        /// 补正类型
        /// </summary>
        public enAdjustType AdjustType { get; set; }
        public userPixCoordSystem PixCoordSystem { get; set; }
        public enMatchMode MatchMode { get; set; }

        public enSortMethod SortMethod { get; set; }
        public bool FiilUp { get; set; }
        public BindingList<ModelParam> SearchRegion
        {
            get;
            set;
        }


        public FindNccModelParam()
        {
            /// 模型匹配参数
            this._AngleStart = -10;
            this._AngleExtent = 10;
            this._MinScore = 0.7;
            this._NumMatches = 1;
            this._MaxOverlap = 0.5;
            this._SubPixel = "true"; // 
            this._NumLevels = 0; // 0:表示使用创建模型时确定的金字塔层数
            this._TimeOut = 5000;
            this.SearchRegion = new BindingList<ModelParam>();
            this.PixCoordSystem = new userPixCoordSystem();
            this.AdjustType = enAdjustType.XYTheta;
            this.MatchMode = enMatchMode.正常模式;
            this.SortMethod = enSortMethod.NONE;
            this.FiilUp = false;
        }

    }






}

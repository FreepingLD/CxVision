using Common;
using HalconDotNet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using userControl;

namespace FunctionBlock
{

    [Serializable]
    public class FlawDetectParam
    {
        public BindingList<DetectROIParam> DetectROI { get; set; }

        public FilterParam FilterParam { get; set; }

        public ThresholParam DetectParam { get; set; }

        public FlawSelectParam SelectParam { get; set; }

        public FlawDetectParam()
        {
            this.DetectROI = new BindingList<DetectROIParam>();
            this.FilterParam = new MeanFilterParam();
            this.DetectParam = new ThresholdParam();
            this.SelectParam = new FlawSelectParam();
        }



        public bool Detect(HImage hImage,out HRegion hRegion)
        {
            bool result = false;
            HImage FilterImage =  new FilterMethod().ImageFilter(hImage, this.FilterParam);
            hRegion =  new FlawDetectMethod().FlawDetect(FilterImage, this.DetectParam, this.DetectROI);
            result = true;
            return result;
        }
    }

    [Serializable]
    public class DetectROIParam
    {
        private enShapeType shapeType = enShapeType.矩形1;
        private enInsideOrOutside insideOrOutside = enInsideOrOutside.保留;

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

        [DisplayNameAttribute("操作方法")]
        public enInsideOrOutside InsideOrOutside
        {
            get
            {
                return insideOrOutside;
            }

            set
            {
                insideOrOutside = value;
            }
        }

        /// <summary>
        /// 2D 中使用像素形状
        /// </summary>
        [DisplayNameAttribute("形状参数")]
        public PixROI RoiShape { get; set; }

    }

    [Serializable]
    public class FlawSelectParam
    {
        /// <summary> 瑕疵面积阈值 </summary>       
        public St_Condition FlawArea { get; set; }
        /// <summary> 瑕疵长度阈值 </summary>        
        public St_Condition FlawLen1 { get; set; }
        public St_Condition FlawLen2 { get; set; }
        /// <summary> 长宽比阈值 </summary>       
        public St_Condition FlawLwRate { get; set; }
        /// <summary>瑕疵灰度均值阈值 </summary>        
        public St_Condition FlawGrayMean { get; set; }
        /// <summary> 瑕疵灰度偏差   </summary>      
        public St_Condition FlawGrayDeviation { get; set; }
        /// <summary>瑕疵相对于检测区域灰度均值的偏差 </summary>    
        public St_Condition FlawGrayDiff { get; set; }
        /// <summary>  检测区域的平均灰度  </summary>      
        public St_Condition InspRoiGrayMean { get; set; }
        /// <summary>   瑕疵凸度 </summary>      
        public St_Condition FlawConvexity { get; set; }
        /// <summary>  瑕疵圆度   </summary>    
        public St_Condition FlawCircularity { get; set; }
        /// <summary>   瑕疵矩形度 </summary>   
        public St_Condition FlawRectangularity { get; set; }
        /// <summary> 瑕疵的紧密度，圆的紧密度1，矩形1.5，线的紧密度 5左右 </summary>       
        public St_Condition FlawCompactness { get; set; }
        /// <summary> 瑕疵数量阈值 ，小瑕疵数量低于某数量，此瑕疵为Ok </summary>
        public St_Condition FlawCountThd { get; set; }
        public St_Condition FlawDiameter { get; set; }
        public St_Condition FlawCountPerUnit { get; set; }

        public FlawSelectParam(bool IsInit = false)
        {
            this.FlawArea = new St_Condition(100, 999999999999, false);
            this.FlawLen1 = new St_Condition(1, 10000000, false);
            this.FlawLen2 = new St_Condition(1, 1000000, false);
            this.FlawLwRate = new St_Condition(0, 1, false);
            this.FlawGrayMean = new St_Condition(0, 255, false);
            this.FlawGrayDeviation = new St_Condition(0, 255, false);
            this.FlawGrayDiff = new St_Condition(0, 255, false);
            this.InspRoiGrayMean = new St_Condition(0, 255, false);
            this.FlawConvexity = new St_Condition(0, 1, false);
            this.FlawCircularity = new St_Condition(0, 1, false);
            this.FlawRectangularity = new St_Condition(0, 1, false);
            this.FlawCompactness = new St_Condition(0, 10, false);
            this.FlawCountThd = new St_Condition(0, 20, false);
            this.FlawDiameter = new St_Condition(1, 2, false);
            this.FlawCountPerUnit = new St_Condition(0, 1, false);
        }

    }


}

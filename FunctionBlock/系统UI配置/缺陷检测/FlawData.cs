using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using HalconDotNet;

namespace FunctionBlock
{
    [Serializable]
    public class FlawData
    {

        /// <summary>
        /// 检测图像
        /// </summary>
        public ImageDataClass ImageData { get; set; }
        public BindingList<FlawMsg> FlawMessage { get; set; }

        public XldDataClass DetectXld { get; set; }
        public string Result { get; set; }

        public FlawData()
        {
            this.ImageData = null;
            this.DetectXld = null;
            this.FlawMessage = new BindingList<FlawMsg>();
        }
        public FlawData (ImageDataClass ImageData, BindingList<FlawMsg> FlawMessage)
        {
            this.ImageData = ImageData;
            this.DetectXld = null;
            this.FlawMessage = FlawMessage;
            this.Result = "OK";
        }
        public FlawData(ImageDataClass ImageData, XldDataClass detectXld, BindingList<FlawMsg> FlawMessage)
        {
            this.ImageData = ImageData;
            this.DetectXld = detectXld;
            this.FlawMessage = FlawMessage;
            this.Result = "OK";
        }
    }



    [Serializable]
    public class FlawMsg
    {
        /// <summary> 瑕疵描述</summary>
         [DisplayNameAttribute("描述")]
        public string FlawDescribe
        {
            get;
            set;
        }

        [DisplayNameAttribute("面积")]
        /// <summary> 瑕疵的面积 </summary>
        public double FlawArea
        {
            get;
            set;
        }

        [DisplayNameAttribute("行中心")]
        public double CenterRow
        {
            get;
            set;
        }

        [DisplayNameAttribute("列中心")]
        public double CenterCol
        {
            get;
            set;
        }

        [DisplayNameAttribute("矩形的角度")]
        /// <summary>  瑕疵最小外接矩形的角度 </summary>
        public double FlawRect2Phi { get; set; }

        [DisplayNameAttribute("矩形长度")]
        /// <summary> 瑕疵最小外接矩形的长度 </summary>
        public double FlawLen1
        {
            get;
            set;
        }

        [DisplayNameAttribute("矩形宽度")]
        /// <summary> 瑕疵最小外接矩形的宽度 </summary>
        public double FlawLen2
        {
            get;
            set;
        }

        [DisplayNameAttribute("矩形长宽比")]
        /// <summary> 瑕疵的最小外接矩形长宽比 </summary>
        public double FlawLw
        {
            get;
            set;
        }

        [DisplayNameAttribute("平均灰度")]
        /// <summary>瑕疵的平均灰度 </summary>
        public double FlawGrayMean
        {
            get;
            set;
        }

        [DisplayNameAttribute("灰度标准差")]
        /// <summary> 瑕疵灰度偏差 </summary>
        public double FlawGrayDeviation
        {
            get;
            set;
        }

        [DisplayNameAttribute("凸度")]
        /// <summary> 瑕疵凸度，区域的面积/区域凸包的面积 </summary>
        public double FlawConvexity
        {
            get;
            set;
        }

        [DisplayNameAttribute("圆度")]
        /// <summary> 瑕疵的圆度 </summary>
        public double FlawCircularity
        {
            get;
            set;
        }

        [DisplayNameAttribute("矩形度")]
        /// <summary> 瑕疵的矩形度 </summary>
        public double FlawRectangularity
        {
            get;
            set;
        }

        [DisplayNameAttribute("紧密度")]
        /// <summary> 瑕疵的紧密度</summary>
        public double FlawCompactness
        {
            get;
            set;
        }

        [DisplayNameAttribute("行坐标")]
        /// <summary> 瑕疵的行坐标</summary>
        public List<double> FlawRow
        {
            get;
            set;
        }

        [DisplayNameAttribute("列坐标")]
        /// <summary> 瑕疵的列坐标 </summary>
        public List<double> FlawCol
        {
            get;
            set;
        }


        public FlawMsg()
        {
            this.FlawDescribe = "NONE";
            this.FlawRow = new List<double>();
            this.FlawCol = new List<double>();
        }







    }
}

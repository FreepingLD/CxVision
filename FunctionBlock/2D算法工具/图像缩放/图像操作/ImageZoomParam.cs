using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HalconDotNet;
using System.Drawing;
using Sensor;
using MotionControlCard;
using System.Windows.Forms;
using System.IO;
using AlgorithmsLibrary;
using System.ComponentModel;
using Common;

namespace FunctionBlock
{
    [Serializable]
    public class ImageZoomParam
    {
        public string Method { get; set; }
    }
    [Serializable]
    public class ZoomImageFactorParam : ImageZoomParam
    {
        public double ScaleWidth { get; set; }
        public double ScaleHeight { get; set; }
        public enInterpolationType Interpolation { get; set; }
        public ZoomImageFactorParam()
        {
            this.ScaleWidth =1;
            this.ScaleHeight = 1;
            this.Interpolation = enInterpolationType.constant;
            this.Method = "zoom_image_factor";
        }
        public ZoomImageFactorParam(string method)
        {
            this.ScaleWidth = 1;
            this.ScaleHeight = 1;
            this.Interpolation = enInterpolationType.constant;
            this.Method = method;
        }
    }
    [Serializable]
    public class ZoomImageSizeParam : ImageZoomParam
    {
        public int ImageWidth { get; set; }
        public int ImageHeight { get; set; }
        public enInterpolationType Interpolation { get; set; }
        public ZoomImageSizeParam()
        {
            this.ImageWidth = 2048;
            this.ImageHeight = 2048;
            this.Interpolation = enInterpolationType.constant;
            this.Method = "zoom_image_size";
        }
        public ZoomImageSizeParam(string method)
        {
            this.ImageWidth = 2048;
            this.ImageHeight = 2048;
            this.Interpolation = enInterpolationType.constant;
            this.Method = method;
        }
    }

    public enum enInterpolationType
    {
         bicubic, 
        bilinear, 
        constant,
        nearest_neighbor, 
        weighted
    }



}

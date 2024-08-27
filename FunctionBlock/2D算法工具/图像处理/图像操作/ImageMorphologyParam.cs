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
    public class ImageMorphologyParam
    {
        public string Method { get; set; }
    }
    [Serializable]
    public class ImageRectParam : ImageMorphologyParam
    {
        public double MaskWidth { get; set; }
        public double MaskHeight { get; set; }
        public ImageRectParam()
        {
            MaskWidth = 3;
            MaskHeight = 3;
            Method = "gray_opening_rect";
        }
        public ImageRectParam(string method)
        {
            MaskWidth = 3;
            MaskHeight = 3;
            Method = method;
        }
    }
    [Serializable]
    public class ImageShapeParam : ImageMorphologyParam
    {
        public double MaskWidth { get; set; }
        public double MaskHeight { get; set; }
        public string MaskShape { get; set; }
        public ImageShapeParam()
        {
            MaskWidth = 3;
            MaskHeight = 3;
            MaskShape = "octagon";
            Method = "gray_opening_rect";
        }
        public ImageShapeParam(string method)
        {
            MaskWidth = 3;
            MaskHeight = 3;
            MaskShape = "octagon";
            Method = method;
        }
    }


}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HalconDotNet;

namespace FunctionBlock
{

    [Serializable]
    public class GapDetectParam
    {
        public int OpenWidth { get; set; }
        public int OpenHeight { get; set; }
        public int CloseWidth { get; set; }
        public int CloseHeight { get; set; }
        public double MinThreshold { get; set; }
        public double MaxThreshold { get; set; }
        public double MinArea { get; set; }

        public enImagePose ImagePose { get; set; }
        public BindingList<GapShapeParam> ShapeParam { get; set; }

        public GapDetectParam()
        {
            this.OpenWidth = 1;
            this.OpenHeight = 1;
            this.CloseWidth = 555;
            this.CloseHeight = 25;
            this.MinThreshold = 20;
            this.MaxThreshold = 255;
            this.ShapeParam = new BindingList<GapShapeParam>();
            this.MinArea = 100;
            this.ImagePose = enImagePose.中间;
        }






    }


    [Serializable]
    public enum enImagePose
    {
        NONE,
        中间,
        首端,
        尾端,
    }
}

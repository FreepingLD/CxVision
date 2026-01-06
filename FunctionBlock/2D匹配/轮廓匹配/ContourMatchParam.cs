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
using System.Data;
using Light;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using System.Diagnostics;

namespace FunctionBlock
{

    [Serializable]
    public class ContourMatchParam
    {
        public double StartPercent { get; set; }
        public double EndPercent { get; set; }
        public  enTransformationType TransformationType { get; set; }
        public string ResampleDist { get; set; }


        public ContourMatchParam()
        {
            this.StartPercent = 0.1;
            this.EndPercent = 0.7;
            this.TransformationType = enTransformationType.affine;
            this.ResampleDist = "auto";// 自动模式下不采样
        }

    }


}

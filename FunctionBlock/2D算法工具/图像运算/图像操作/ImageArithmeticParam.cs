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
    public class ImageArithmeticParam
    {
        public string Method { get; set; }
    }
    [Serializable]
    public class MultAddParam : ImageArithmeticParam
    {
        public double Mult { get; set; }
        public double Add { get; set; }
        public MultAddParam()
        {
            Mult = 1;
            Add = 128;
            Method = "add_image";
        }
        public MultAddParam(string method)
        {
            Mult = 1;
            Add = 128;
            Method = method;
        }
    }

}

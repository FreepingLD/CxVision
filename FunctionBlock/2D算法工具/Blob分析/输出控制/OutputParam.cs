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
    public class OutputParam
    {
        public bool IsOutputRegion { get; set; }
        public bool IsOutputBinaryImage { get; set; }
        public bool IsOutputBinaryMeanImage { get; set; }

        public string DrawMode { get; set; }

        public OutputParam()
        {
            this.IsOutputRegion = true;
            this.IsOutputBinaryImage = false;
            this.IsOutputBinaryMeanImage = false;
            this.DrawMode = "margin";
        }

    }


}

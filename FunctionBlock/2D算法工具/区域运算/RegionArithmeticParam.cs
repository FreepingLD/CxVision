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
    public class RegionArithmeticParam
    {
        public enRegionArithmeticMethod Method { get; set; }
        public enRegionOperate RegionOperate { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public RegionArithmeticParam()
        {
            Method = enRegionArithmeticMethod.差集;
            RegionOperate = enRegionOperate.opening_rectangle1;
            Width = 1;
            Height = 5;
        }

    }
    public enum enRegionArithmeticMethod
    {
        交集,
        并集,
        差集,
        补集,
    }

}

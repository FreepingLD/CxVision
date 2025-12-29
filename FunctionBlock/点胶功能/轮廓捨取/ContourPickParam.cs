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
    public class ContourPickParam
    {
        public enPullMethod PullMethod { get; set; }
        public ContourPickParam()
        {
            this.PullMethod = enPullMethod.图像中心;
        }




    }

    public enum enPullMethod
    {
        图像中心,
        手动捨取,
        机械坐标,
    }

}

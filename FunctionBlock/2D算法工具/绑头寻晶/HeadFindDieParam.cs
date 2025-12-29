using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HalconDotNet;
using Sensor;
using MotionControlCard;
using System.Windows.Forms;
using System.IO;
using AlgorithmsLibrary;
using System.ComponentModel;
using Common;
using System.Data;
using System.Diagnostics;
using System.Security.Policy;

namespace FunctionBlock
{
    [Serializable]
    public class HeadFindDieParam
    {
        public enHeadFindDieMode HeadFindDieMode { get; set; }

        public enHeadFindDieOrientation HeadFindDieOrientation { get; set; }


        public HeadFindDieParam()
        {
            this.HeadFindDieMode = enHeadFindDieMode.纵向寻晶;
            this.HeadFindDieOrientation = enHeadFindDieOrientation.从右到左;
        }


    }

    public enum enHeadFindDieMode
    {
        横向寻晶,
        纵向寻晶,
    }

    public enum enHeadFindDieOrientation
    {
        从左到右,
        从右到左,
        从上到下,
        从下到上,
    }

}

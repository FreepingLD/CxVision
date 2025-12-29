using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MotionControlCard;
using System.Windows.Forms;
using System.Threading;
using HalconDotNet;
using Common;
using System.IO;
using Sensor;
using Command;
using AlgorithmsLibrary;
using System.ComponentModel;
using System.Data;
using System.Reflection;

namespace FunctionBlock
{
    [Serializable]
    /// <summary>
    /// 标签参数类
    /// </summary>
    public class DataLableParam
    {
        public string CamName { get; set; }
        public string ViewName { get; set; }
        public double Start_x { get; set; }
        public double Start_y { get; set; }
        public double Offset_y { get; set; }
        public int Size { get; set; }
        public bool SendResult { get; set; }

        public enLablePosition LablePosition { get; set; }
        public DataLableParam()
        {
            this.CamName = "Cam1";
            this.ViewName = "Cam1";
            this.Start_x = 100;
            this.Start_y = 100;
            this.Offset_y = 200;
            this.Size = 25;
            this.SendResult = false;
            this.LablePosition = enLablePosition.用户定义;
        }


    }

}

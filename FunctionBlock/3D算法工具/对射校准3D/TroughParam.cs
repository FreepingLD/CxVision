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


namespace FunctionBlock
{
    [Serializable]
    /// <summary>
    /// 激光取点采集的数据类
    /// </summary>
    public class TroughParam 
    {

        public string LaserAcqSource1
        {
            get;
            set;
        }
        public string LaserAcqSource2
        {
            get;
            set;
        }

        public double CalibValue { get; set; }

        public double StdValue { get; set; }

        public string AcqSourceName { get; set; }

        public double Laser1Value { get; set; }

        public double Laser2Value { get; set; }

        public TroughParam()
        {

        }



    }

}

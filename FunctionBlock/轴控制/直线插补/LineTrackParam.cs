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
    public class LineTrackParam
    {
        [DefaultValue(true)]
        [DisplayNameAttribute("激活")]
        public bool IsActive { get; set; } // 是否触发传感器

        [DefaultValue(enAxisName.XYZTheta轴)]
        [DisplayNameAttribute("移动轴")]
        public enAxisName MoveAxis { get; set; }

        [DefaultValue(0)]
        [DisplayNameAttribute("X坐标")]
        public double X1 { get; set; } // 是否触发传感器

        [DefaultValue(0)]
        [DisplayNameAttribute("Y坐标")]
        public double Y1 { get; set; } // 是否触发传感器

        [DefaultValue(0)]
        [DisplayNameAttribute("Z坐标")]
        public double Z1 { get; set; } // 是否触发传感器

        [DefaultValue(0)]
        [DisplayNameAttribute("旋转坐标")]
        public double Theta1 { get; set; } // 是否触发传感器

        [DefaultValue(0)]
        [DisplayNameAttribute("IO输出")]
        public int IoOutPort { get; set; }

        [DefaultValue(true)]
        [DisplayNameAttribute("是否同步")]
        public bool IsAsynchronous { get; set; }

    }

}

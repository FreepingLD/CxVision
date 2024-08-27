using Common;
using MotionControlCard;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FunctionBlock
{
    [Serializable]
    public class RobotParam
    {
        public double MinGray { get; set; } 
        public double MaxGray { get; set; }
        public int RectSize { get; set; }
        public int Area { get; set; }
        public enRefGrabPose RefGrabPose { get; set; }
        public enRobotJawEnum RobotJaw { get; set; }
        public enOperation Operate { get; set; }
        // X轴正限位
        public double LimitP_X { get; set; } = 100;
        // X轴负限位
        public double LimitN_X { get; set; } = -100;
        // Y轴正限位
        public double LimitP_Y { get; set; } = 100;
        // Y轴负限位
        public double LimitN_Y { get; set; } = -100;

        public double LimitP_Angle { get; set; } = 5;
        // Y轴负限位
        public double LimitN_Angle { get; set; } = -5;

        public string ViewWindow { get; set; } = "NONE";

        public RobotParam()
        {
            this.RefGrabPose = enRefGrabPose.标定位置;
            this.RectSize = 100;
            this.Area = 100;
            this.MinGray = 20;
            this.MaxGray = 50;
            this.RobotJaw = enRobotJawEnum.Jaw1;
            this.Operate = enOperation.and;
        }



    }



}

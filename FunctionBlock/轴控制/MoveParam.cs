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
    public class MoveParam
    {
        // 从机台读取的坐标值
        private enCoordSysName _coordSysName;
        private enCoordType coordType = enCoordType.机台坐标;

        private double startVel = 0;//起始速度
        private double maxVel = 50;//运行速度
        private double tacc = 0.5;//加速时间
        private double tdec = 0.5;//减速时间
        private double stopVel;//停止速度
        private double s_para = 0.1;//S段时间


        public enCoordSysName CoordSysName { get => _coordSysName; set => _coordSysName = value; }
        public enCoordType CoordType { get => coordType; set => coordType = value; }
        public double StartVel { get => startVel; set => startVel = value; }
        public double MaxVel { get => maxVel; set => maxVel = value; }
        public double Tacc { get => tacc; set => tacc = value; }
        public double Tdec { get => tdec; set => tdec = value; }
        public double StopVel { get => stopVel; set => stopVel = value; }
        public double S_para { get => s_para; set => s_para = value; }


    }


}

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
    public class WaferFindDieParam
    {
        public enWaferFindDie WaferFindDie { get; set; }

        public enCoordQuadrant CoordQuadrant { get; set; }
        public double Dist_X { get; set; }
        public double Dist_Y { get; set; }

        public enRefObject RefObject { get; set; }

        public bool IsOutputMatchPoint { get; set; }
        public double AngleTolerance { get; set; }

        public RfidDieIndex DieIndex { get; set; }

        public enCoordSysName CoordSysName { get; set; }

        public WaferFindDieParam()
        {
            this.WaferFindDie = enWaferFindDie.九宫格寻晶;
            this.Dist_X = 1;
            this.Dist_Y = 1;
            this.CoordQuadrant = enCoordQuadrant.第一象限;
            this.RefObject = enRefObject.示教点;
            this.IsOutputMatchPoint = false;
            this.AngleTolerance = 10;
            this.DieIndex = new RfidDieIndex();
            this.CoordSysName = enCoordSysName.CoordSys_0;
        }


    }

    public enum enWaferFindDie
    {
        九宫格寻晶,
    }



}

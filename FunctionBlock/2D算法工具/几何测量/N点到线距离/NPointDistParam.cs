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
using System.Drawing;
using System.Diagnostics;

namespace FunctionBlock
{
    [Serializable]

    public class NPointDistParam
    {
        public enOutPointMode OutPointMode { get; set; } = enOutPointMode.输出最小点;

        public int AverangeCount { get; set; } = 1;

        public enCalculateDataPoint DataPointSource { get; set; } = enCalculateDataPoint.拟合点;


        public  bool CalculateNPointToLineDist2D(userWcsPoint[] wcsPoint, userWcsLine Line, out double meanDist, out double maxDist, out double minDist, out userWcsPoint minWcsPoint, out userWcsPoint maxWcsPoint)
        {
            bool result = false;
            if (wcsPoint == null)
            {
                throw new ArgumentNullException("wcsPoint");
            }
            if (Line == null)
            {
                throw new ArgumentNullException("Line");
            }
            if (wcsPoint.Length > 0)
            {
                double[] x = new double[wcsPoint.Length];
                double[] y = new double[wcsPoint.Length];
                for (int i = 0; i < wcsPoint.Length; i++)
                {
                    x[i] = wcsPoint[i].X;
                    y[i] = wcsPoint[i].Y;
                }
                HTuple dist = HMisc.DistancePl(y, x, Line.Y1, Line.X1, Line.Y2, Line.X2);
                HTuple sortIndex = dist.TupleSortIndex();
                List<double> list_x = new List<double>();
                List<double> list_y = new List<double>();
                for (int i = 0; i < this.AverangeCount; i++)
                {
                    if(i < x.Length)
                    {
                        list_x.Add(x[sortIndex[i].I]);
                        list_y.Add(y[sortIndex[i].I]);
                    }
                }
                minWcsPoint = new userWcsPoint(list_x.Average(), list_y.Average(), 0, wcsPoint[0].CamParams);
                minWcsPoint.ViewWindow = wcsPoint[0].ViewWindow;
                minWcsPoint.Tag = wcsPoint[0].Tag;
                minWcsPoint.CamName = wcsPoint[0].CamName;
                minWcsPoint.Grab_x = wcsPoint[0].Grab_x;
                minWcsPoint.Grab_y = wcsPoint[0].Grab_y;
                minWcsPoint.Grab_z = wcsPoint[0].Grab_z;
                minWcsPoint.Grab_theta = wcsPoint[0].Grab_theta;
                minWcsPoint.Grab_u = wcsPoint[0].Grab_u;
                minWcsPoint.Grab_v = wcsPoint[0].Grab_v;
                ////////////////////  计算最大值点 ///////////////////////////////////
                list_x.Clear();
                list_y.Clear();
                for (int i = x.Length - 1; i >= x.Length - this.AverangeCount; i--)
                {
                    if (i >= 0)
                    {
                        list_x.Add(x[sortIndex[i].I]);
                        list_y.Add(y[sortIndex[i].I]);
                    }
                }
                maxWcsPoint = new userWcsPoint(list_x.Average(), list_y.Average(), 0, wcsPoint[0].CamParams);
                maxWcsPoint.ViewWindow = wcsPoint[0].ViewWindow;
                maxWcsPoint.Tag = wcsPoint[0].Tag;
                maxWcsPoint.CamName = wcsPoint[0].CamName;
                maxWcsPoint.Grab_x = wcsPoint[0].Grab_x;
                maxWcsPoint.Grab_y = wcsPoint[0].Grab_y;
                maxWcsPoint.Grab_z = wcsPoint[0].Grab_z;
                maxWcsPoint.Grab_theta = wcsPoint[0].Grab_theta;
                maxWcsPoint.Grab_u = wcsPoint[0].Grab_u;
                maxWcsPoint.Grab_v = wcsPoint[0].Grab_v;
                /////////////////////////////////////////
                maxDist = dist.TupleMax().D;
                minDist = dist.TupleMin().D;
                meanDist = dist.TupleMean().D;
            }
            else
            {
                maxWcsPoint = new userWcsPoint();
                minWcsPoint = new userWcsPoint();
                maxDist = 0;
                minDist = 0;
                meanDist = 0;
            }
            result = true;
            //////////////////////////////////////
            return result;
        }

    }

    public enum enOutPointMode
    {
        输出最小点,
        输出最大点,
        输出全部,
    }
    public enum enCalculateDataPoint
    {
        拟合点,
        原始点,
    }


}

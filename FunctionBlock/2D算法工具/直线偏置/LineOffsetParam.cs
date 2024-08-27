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

namespace FunctionBlock
{

    /// <summary>
    /// 将点去数据生成3D对象模型，以便于后续算子操作
    /// </summary>
    [Serializable]
    public class LineOffsetParam
    {
        public double OffsetDist { get; set; }
        public int OffsetCount { get; set; }
        public double Scale { get; set; }


        public LineOffsetParam()
        {
            this.OffsetDist = 0.1;
            this.OffsetCount = 1;
            this.Scale = 1;
        }

        public bool offsetLine(userWcsLine wcsLines, LineOffsetParam param, out userWcsLine[] wcsLine)
        {
            bool result = false;
            if (wcsLines == null)
            {
                throw new ArgumentNullException("wcsLines");
            }
            //////////////////////////////////////////////////////////////////
            wcsLine = new userWcsLine[param.OffsetCount];
            for (int i = 0; i < param.OffsetCount; i++)
            {
                double normalPhi = Math.Atan2(wcsLines.Y2 - wcsLines.Y1, wcsLines.X2 - wcsLines.X1) + Math.PI * 0.5;
                // 已知直线上的点坐标及该点的法线坐标，求法线直线的两点
                wcsLine[i] = new userWcsLine(
                    wcsLines.X1 + param.OffsetDist * (i + 1) * Math.Abs(Math.Cos(normalPhi)),
                    wcsLines.Y1 - param.OffsetDist * (i + 1) * Math.Abs(Math.Sin(normalPhi)),
                    0,
                    wcsLines.X2 + param.OffsetDist * (i + 1) * Math.Abs(Math.Cos(normalPhi)),
                    wcsLines.Y2 - param.OffsetDist * (i + 1) * Math.Abs(Math.Sin(normalPhi)),
                    0,
                    wcsLines.CamParams);
                result = true;
            }
            return result;
        }


        public bool offsetLine(userWcsLine wcsLines,  out userWcsLine wcsLine)
        {
            bool result = false;
            if (wcsLines == null)
            {
                throw new ArgumentNullException("wcsLines");
            }
            //////////////////////////////////////////////////////////////////
            double normalPhi = Math.Atan2(wcsLines.Y2 - wcsLines.Y1, wcsLines.X2 - wcsLines.X1) + Math.PI; //这里需要加上 Math.PI
            wcsLine = new userWcsLine(
                                      wcsLines.X1 + this.OffsetDist * this.Scale * Math.Sin(normalPhi),
                                      wcsLines.Y1 - this.OffsetDist * this.Scale * Math.Cos(normalPhi),
                                      wcsLines.Z1,
                                      wcsLines.X2 + this.OffsetDist * this.Scale * Math.Sin(normalPhi),
                                      wcsLines.Y2 - this.OffsetDist * this.Scale * Math.Cos(normalPhi),
                                      wcsLines.Z2,
                                      wcsLines.CamParams);
            //////////////////////////////////////////////////////////////////
            wcsLine.Grab_x = wcsLines.Grab_x;
            wcsLine.Grab_y = wcsLines.Grab_y;
            wcsLine.Grab_theta = wcsLines.Grab_theta;
            wcsLine.CamName = wcsLines.CamName;
            wcsLine.ViewWindow = wcsLines.ViewWindow;
            wcsLine.Tag = wcsLines.Tag;
            result = true;
            return result;
        }







    }
}

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
    public class LineExtendParam
    {
        public enExtendMethod ExtendMethod { get; set; }
        public double ExtendDist { get; set; }


        public LineExtendParam()
        {
            this.ExtendMethod = enExtendMethod.两端;
            this.ExtendDist = 1;
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


        public bool extendLine(userWcsLine wcsLines, out userWcsLine wcsLine)
        {
            bool result = false;
            if (wcsLines == null)
            {
                throw new ArgumentNullException("wcsLines");
            }
            //////////////////////////////////////////////////////////////////
            wcsLine = new userWcsLine();
            double midx = (wcsLines.X2 + wcsLines.X1) * 0.5;
            double midy = (wcsLines.Y2 + wcsLines.Y1) * 0.5;
            double Phi = Math.Atan2(wcsLines.Y2 - wcsLines.Y1, wcsLines.X2 - wcsLines.X1);
            double startPhi = Math.Atan2(wcsLines.Y1 - midy, wcsLines.X1 - midx);
            double endPhi = Math.Atan2(wcsLines.Y2 - midy, wcsLines.X2 - midx);
            switch (ExtendMethod)
            {
                case enExtendMethod.首端:
                    wcsLine.X1 = wcsLines.X1 + ExtendDist * Math.Cos(startPhi);// row[0].D;
                    wcsLine.Y1 = wcsLines.Y1 + ExtendDist * Math.Sin(startPhi);//col[0].D;
                    wcsLine.X2 = wcsLines.X2 ;//row[1].D;
                    wcsLine.Y2 = wcsLines.Y2 ;//col[1].D;
                    break;
                case enExtendMethod.未端:
                    wcsLine.X1 = wcsLines.X1 ;// row[0].D;
                    wcsLine.Y1 = wcsLines.Y1 ;//col[0].D;
                    wcsLine.X2 = wcsLines.X2 + ExtendDist * Math.Cos(endPhi);//row[1].D;
                    wcsLine.Y2 = wcsLines.Y2 + ExtendDist * Math.Sin(endPhi);//col[1].D;
                    break;
                case enExtendMethod.两端:
                        wcsLine.X1 = wcsLines.X1 + ExtendDist * Math.Cos(startPhi);// row[0].D;
                        wcsLine.Y1 = wcsLines.Y1 + ExtendDist * Math.Sin(startPhi);//col[0].D;
                        wcsLine.X2 = wcsLines.X2 + ExtendDist * Math.Cos(endPhi);//row[1].D;
                        wcsLine.Y2 = wcsLines.Y2 + ExtendDist * Math.Sin(endPhi);//col[1].D;
                    break;
            }
            /////////////////////////////////////////////////
            wcsLine.Grab_x = wcsLines.Grab_x;
            wcsLine.Grab_y = wcsLines.Grab_y;
            wcsLine.Grab_theta = wcsLines.Grab_theta;
            wcsLine.CamName = wcsLines.CamName;
            wcsLine.ViewWindow = wcsLines.ViewWindow;
            wcsLine.CamParams = wcsLines.CamParams;
            wcsLine.Tag = wcsLines.Tag;
            result = true;
            return result;
        }


    }

    public enum enExtendMethod
    {
        首端,
        未端,
        两端,
    }
}

using AlgorithmsLibrary;
using Common;
using FunctionBlock;
using HalconDotNet;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;

namespace FunctionBloc
{
    public class RoundCornerMethod
    {
        public static bool LineRoundCorner(userWcsLine line1, userWcsLine line2, RoundCornerParam param, out userWcsPolyLine wcsPolyLine)
        {
            bool result = false;
            if (line1 == null)
            {
                throw new ArgumentNullException("line1");
            }
            if (line2 == null)
            {
                throw new ArgumentNullException("line2");
            }
            wcsPolyLine = new userWcsPolyLine(line1.CamParams);
            wcsPolyLine.CamName = line1.CamName;
            wcsPolyLine.ViewWindow = line1.ViewWindow;  
            ///////////////////////////////
            userWcsPoint wcsInterPoint,line1ProjPoint, line2ProjPoint,centerPoint,circleMidPoint;
            userWcsLine wcsLine;
            double angle, center_x, center_y,circelMid_x, circelMid_y;
            HalconLibrary.IntersectionPoint(line1, line2, out wcsInterPoint);
            HalconLibrary.AngleBisector(line1, line2, out wcsLine);
            HalconLibrary.LineLineAngle(line1, line2, out angle);
            // 计算角平分线的角度
            double phi = Math.Atan2(wcsLine.Y1 - wcsInterPoint.Y, wcsLine.X1 - wcsInterPoint.X);
            double phideg = phi * 180 / Math.PI;
            double length = param.Radius / Math.Sin(angle * 0.5 * Math.PI / 180);
            center_x = wcsInterPoint.X + length * Math.Cos(phi);
            center_y = wcsInterPoint.Y + length * Math.Sin(phi);
            centerPoint = new userWcsPoint(center_x, center_y, 0);
            // 计算圆心与交点的角度,并计算圆弧中点的坐标
            phi = Math.Atan2(wcsInterPoint.Y - center_y, wcsInterPoint.X - center_x);
            circelMid_x = center_x + param.Radius * Math.Cos(phi);
            circelMid_y = center_y + param.Radius * Math.Sin(phi);
            /////////////////////////////////////////////////////////////////////////////
            HalconLibrary.CalculateProjectionPoint(centerPoint, line1, out line1ProjPoint);
            HalconLibrary.CalculateProjectionPoint(centerPoint, line2, out line2ProjPoint);
            double rad1 = Math.Atan2(line1ProjPoint.Y - centerPoint.Y, line1ProjPoint.X - centerPoint.X);
            double rad2 = Math.Atan2(line2ProjPoint.Y - centerPoint.Y, line2ProjPoint.X - centerPoint.X);
            userWcsCircleSector wcsCircleSector = new userWcsCircleSector(center_x, center_y, 0, param.Radius, rad1 * 180 / Math.PI, rad2 * 180 / Math.PI);
            /// 计算圆弧的极性
            HTuple Row = 0, Column = 0, Radius = 0, StartPhi = 0, EndPhi = 0, PointOrder = 0;
            new HXLDCont(new HTuple(line1ProjPoint.Y, circelMid_y, line2ProjPoint.Y) * -1, new HTuple(line1ProjPoint.X, circelMid_x, line2ProjPoint.X)).FitCircleContourXld("algebraic", -1, 0, 0, 3, 2, out Row, out Column, out Radius, out StartPhi, out EndPhi, out PointOrder);
            wcsCircleSector.PointOrder = PointOrder.S;
            param.PointOrder = PointOrder.S; // 赋值
            wcsCircleSector.CamParams = line1.CamParams;
            wcsPolyLine.Add(line1.X1, line1.Y1, line1.Z1);
            wcsPolyLine.Add(line1ProjPoint.X, line1ProjPoint.Y, line1ProjPoint.Z);
            foreach (var item in wcsCircleSector.GetLineInterpretationPointByCount(param.InterPoint))
            {
                wcsPolyLine.Add(item.X, item.Y, item.Z);
            }
            wcsPolyLine.Add(line2ProjPoint.X, line2ProjPoint.Y, line2ProjPoint.Z);
            wcsPolyLine.Add(line2.X2, line2.Y2, line2.Z2);
            result = true;
            return result;
        }
    }
}

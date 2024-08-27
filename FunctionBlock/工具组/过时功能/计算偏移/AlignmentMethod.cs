using AlgorithmsLibrary;
using Common;
using System;
using System.Collections.Generic;

namespace FunctionBlock
{
    public class AlignmentMethod
    {

        public static bool CalculateOffset(userWcsPoint[] TargetPoint, userWcsPoint[] SourcePoint, CompensationParam Param, out userWcsVector AddXYTheta)
        {
            bool result = false;
            HalconLibrary ha = new HalconLibrary();
            userWcsVector refVectorPoint = new userWcsVector();
            userWcsVector currentVectorPoint = new userWcsVector();
            userWcsVector addVector = new userWcsVector();
            userWcsPoint[] wcsAddRefPoints;
            AddXYTheta = new userWcsVector();
            if (TargetPoint == null)
                throw new ArgumentNullException("TargetPoint");
            if (SourcePoint == null)
                throw new ArgumentNullException("SourcePoint");
            if (Param == null)
                throw new ArgumentNullException("Param");
            ///////////////////////////////////////////
            addVector.X = Param.Add_X;
            addVector.Y = Param.Add_Y;
            addVector.Angle = Param.Add_Angle;
            switch (Param.AlignmentMethod)
            {
                case enAlignmentMethod.单点对齐:
                    switch (Param.RefObject)
                    {
                        default:
                        case enRefObject.视野中心:
                            if (SourcePoint.Length < 1)
                                throw new ArgumentException("CurPoint 的长度小于1 ");
                            AddXYTheta.X = (SourcePoint[0].Grab_x - SourcePoint[0].X) + Param.Add_X;
                            AddXYTheta.Y = (SourcePoint[0].Grab_y - SourcePoint[0].Y) + Param.Add_Y;
                            break;
                        case enRefObject.示教点:
                            if (TargetPoint.Length < 1)
                                throw new ArgumentException("RefPoint 的长度小于1 ");
                            if (SourcePoint.Length < 1)
                                throw new ArgumentException("CurPoint 的长度小于1 ");
                            AddXYTheta.X = (TargetPoint[0].X - SourcePoint[0].X) + Param.Add_X;
                            AddXYTheta.Y = (TargetPoint[0].Y - SourcePoint[0].Y) + Param.Add_Y;
                            break;
                    }
                    result = true;
                    break;
                case enAlignmentMethod.两点对齐_起点角度:
                    if (TargetPoint.Length < 2)
                        throw new ArgumentException("TargetPoint 的长度小于2 ");
                    if (SourcePoint.Length < 2)
                        throw new ArgumentException("SourcePoint 的长度小于2 ");
                    switch (Param.RefObject)
                    {
                        default:
                        case enRefObject.视野中心:
                            double X1 = 0 - SourcePoint[0].CamParams.CaliParam.CalibCenterXy.X; // 视野中心  = 0 - 旋转中心坐标
                            double Y1 = 0 - SourcePoint[0].CamParams.CaliParam.CalibCenterXy.Y; 
                            double X2 = 0 - SourcePoint[1].CamParams.CaliParam.CalibCenterXy.X; 
                            double Y2 = 0 - SourcePoint[1].CamParams.CaliParam.CalibCenterXy.Y; 
                            double Angle = Math.Atan2(Y2 - Y1, X2 - X1);
                            refVectorPoint.X = X1;
                            refVectorPoint.Y = Y1;
                            refVectorPoint.Angle = Angle + Param.Add_Angle * Math.PI / 180;  //这里需要将补偿值转换成弧度
                            currentVectorPoint.X = (SourcePoint[0].X);
                            currentVectorPoint.Y = (SourcePoint[0].Y);
                            currentVectorPoint.Angle = Math.Atan2(SourcePoint[1].Y - SourcePoint[0].Y, SourcePoint[1].X - SourcePoint[0].X);
                            AddXYTheta = ha.VectorAngleToMotionXYTheta(currentVectorPoint, refVectorPoint);
                            ////////////////////////////////////////////////
                            AddXYTheta.X = AddXYTheta.X + Param.Add_X;
                            AddXYTheta.Y = AddXYTheta.Y + Param.Add_Y;
                            AddXYTheta.Angle = AddXYTheta.Angle + Param.Add_Angle;
                            break;
                        case enRefObject.示教点:
                            refVectorPoint.X = TargetPoint[0].X;
                            refVectorPoint.Y = TargetPoint[0].Y;
                            refVectorPoint.Angle = Math.Atan2(TargetPoint[1].Y - TargetPoint[0].Y, TargetPoint[1].X - TargetPoint[0].X);  //这里需要将补偿值转换成弧度
                            currentVectorPoint.X = (SourcePoint[0].X);
                            currentVectorPoint.Y = (SourcePoint[0].Y);
                            currentVectorPoint.Angle = Math.Atan2(SourcePoint[1].Y - SourcePoint[0].Y, SourcePoint[1].X - SourcePoint[0].X);
                            AddXYTheta = ha.VectorAngleToMotionXYTheta(currentVectorPoint, refVectorPoint);
                            ////////////////////////////////////////////////
                            AddXYTheta.X = AddXYTheta.X + Param.Add_X;
                            AddXYTheta.Y = AddXYTheta.Y + Param.Add_Y;
                            AddXYTheta.Angle = AddXYTheta.Angle + Param.Add_Angle;
                            break;
                    }
                    result = true;
                    break;
                case enAlignmentMethod.两点对齐:
                    if (TargetPoint.Length < 2)
                        throw new ArgumentException("TargetPoint 的长度小于2 ");
                    if (SourcePoint.Length < 2)
                        throw new ArgumentException("SourcePoint 的长度小于2 ");
                    switch (Param.RefObject)
                    {
                        case enRefObject.视野中心:
                            double X1 = 0 - SourcePoint[0].CamParams.CaliParam.CalibCenterXy.X; // 视野中心  = 0 - 旋转中心坐标
                            double Y1 = 0 - SourcePoint[0].CamParams.CaliParam.CalibCenterXy.Y;
                            double X2 = 0 - SourcePoint[1].CamParams.CaliParam.CalibCenterXy.X;
                            double Y2 = 0 - SourcePoint[1].CamParams.CaliParam.CalibCenterXy.Y;
                            userWcsPoint[] wcsPoints = new userWcsPoint[2];
                            wcsPoints[0].X = X1;
                            wcsPoints[0].Y = Y1;
                            wcsPoints[1].X = X2;
                            wcsPoints[1].Y = Y2;
                            /////////////////////////////////////////////////////////
                            wcsAddRefPoints = ha.AffineTransPoint2d(wcsPoints, addVector);
                            AddXYTheta = ha.PointToMotionXYTheta(SourcePoint, wcsAddRefPoints);
                            break;
                        case enRefObject.示教点:
                            wcsAddRefPoints = ha.AffineTransPoint2d(SourcePoint, addVector);
                            AddXYTheta = ha.PointToMotionXYTheta(SourcePoint, wcsAddRefPoints);
                            break;
                    }
                    result = true;
                    break;
                case enAlignmentMethod.三点对齐:
                    if (TargetPoint.Length < 3)
                        throw new ArgumentException("TargetPoint 的长度小于3 ");
                    if (SourcePoint.Length < 3)
                        throw new ArgumentException("SourcePoint 的长度小于3 ");
                    switch (Param.RefObject)
                    {
                        case enRefObject.视野中心:
                            double X1 = 0 - SourcePoint[0].CamParams.CaliParam.CalibCenterXy.X; // 视野中心  = 0 - 旋转中心坐标
                            double Y1 = 0 - SourcePoint[0].CamParams.CaliParam.CalibCenterXy.Y;
                            double X2 = 0 - SourcePoint[1].CamParams.CaliParam.CalibCenterXy.X;
                            double Y2 = 0 - SourcePoint[1].CamParams.CaliParam.CalibCenterXy.Y;
                            double X3 = 0 - SourcePoint[0].CamParams.CaliParam.CalibCenterXy.X; // 视野中心  = 0 - 旋转中心坐标
                            double Y3 = 0 - SourcePoint[0].CamParams.CaliParam.CalibCenterXy.Y;

                            userWcsPoint[] wcsPoints = new userWcsPoint[2];
                            wcsPoints[0].X = X1;
                            wcsPoints[0].Y = Y1;
                            wcsPoints[1].X = X2;
                            wcsPoints[1].Y = Y2;
                            wcsPoints[2].X = X3;
                            wcsPoints[2].Y = Y3;
                            /////////////////////////////////////////////////////////
                            wcsAddRefPoints = ha.AffineTransPoint2d(wcsPoints, addVector);
                            AddXYTheta = ha.PointToMotionXYTheta(SourcePoint, wcsAddRefPoints);
                            break;
                        case enRefObject.示教点:
                            wcsAddRefPoints = ha.AffineTransPoint2d(SourcePoint, addVector);
                            AddXYTheta = ha.PointToMotionXYTheta(SourcePoint, wcsAddRefPoints);
                            break;
                    }
                    result = true;
                    break;

                case enAlignmentMethod.四点对齐:
                    if (TargetPoint.Length < 4)
                        throw new ArgumentException("TargetPoint 的长度小于4 ");
                    if (SourcePoint.Length < 4)
                        throw new ArgumentException("SourcePoint 的长度小于4 ");
                    switch (Param.RefObject)
                    {
                        case enRefObject.视野中心:
                            double X1 = 0 - SourcePoint[0].CamParams.CaliParam.CalibCenterXy.X; // 视野中心  = 0 - 旋转中心坐标
                            double Y1 = 0 - SourcePoint[0].CamParams.CaliParam.CalibCenterXy.Y;
                            double X2 = 0 - SourcePoint[1].CamParams.CaliParam.CalibCenterXy.X;
                            double Y2 = 0 - SourcePoint[1].CamParams.CaliParam.CalibCenterXy.Y;
                            double X3 = 0 - SourcePoint[0].CamParams.CaliParam.CalibCenterXy.X; // 视野中心  = 0 - 旋转中心坐标
                            double Y3 = 0 - SourcePoint[0].CamParams.CaliParam.CalibCenterXy.Y;
                            double X4 = 0 - SourcePoint[1].CamParams.CaliParam.CalibCenterXy.X;
                            double Y4 = 0 - SourcePoint[1].CamParams.CaliParam.CalibCenterXy.Y;
                            userWcsPoint[] wcsPoints = new userWcsPoint[2];
                            wcsPoints[0].X = X1;
                            wcsPoints[0].Y = Y1;
                            wcsPoints[1].X = X2;
                            wcsPoints[1].Y = Y2;
                            wcsPoints[2].X = X3;
                            wcsPoints[2].Y = Y3;
                            wcsPoints[3].X = X4;
                            wcsPoints[3].Y = Y4;
                            /////////////////////////////////////////////////////////
                            wcsAddRefPoints = ha.AffineTransPoint2d(wcsPoints, addVector);
                            AddXYTheta = ha.PointToMotionXYTheta(SourcePoint, wcsAddRefPoints);
                            break;
                        case enRefObject.示教点:
                            wcsAddRefPoints = ha.AffineTransPoint2d(SourcePoint, addVector);
                            AddXYTheta = ha.PointToMotionXYTheta(SourcePoint, wcsAddRefPoints);
                            break;
                    }
                    result = true;
                    break;
                case enAlignmentMethod.N点对齐:
                    if (TargetPoint.Length != SourcePoint.Length)
                        throw new ArgumentException("TargetPoint 的长度与 SourcePoint 的长度不相等 ");
                    switch (Param.RefObject)
                    {
                        case enRefObject.视野中心:
                            List<userWcsPoint> listWcsPoint = new List<userWcsPoint>();
                            foreach (var item in SourcePoint)
                            {
                                listWcsPoint.Add(new userWcsPoint(0 - item.CamParams.CaliParam.CalibCenterXy.X, 0 - item.CamParams.CaliParam.CalibCenterXy.Y, 0));
                            }
                            /////////////////////////////////////////////////////////
                            wcsAddRefPoints = ha.AffineTransPoint2d(listWcsPoint.ToArray(), addVector);
                            AddXYTheta = ha.PointToMotionXYTheta(SourcePoint, wcsAddRefPoints); // 当前点到参考点的对齐
                            break;
                        case enRefObject.示教点:
                            wcsAddRefPoints = ha.AffineTransPoint2d(SourcePoint, addVector);
                            AddXYTheta = ha.PointToMotionXYTheta(SourcePoint, wcsAddRefPoints);
                            break;
                    }
                    result = true;
                    break;
                default:
                    throw new ArgumentException("小于所需的对齐点");
            }
            return result;
        }


    }
}

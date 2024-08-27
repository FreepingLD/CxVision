using AlgorithmsLibrary;
using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionBlock
{
    public class AlignmentGuidedMethod
    {

        public static bool CalculateOffset(userWcsPoint[] RefPoint, userWcsPoint[] CurPoint, AlignmentGuidedParam Param, out userWcsVector OffsetXYTheta)
        {
            bool result = false;
            HalconLibrary ha = new HalconLibrary();
            userWcsVector refVectorPoint = new userWcsVector();
            userWcsVector currentVectorPoint = new userWcsVector();
            userWcsVector addVector = new userWcsVector();
            userWcsPoint[] wcsAffineRefPoints;
            OffsetXYTheta = new userWcsVector();
            if (RefPoint == null)
                throw new ArgumentNullException("RefPoint");
            if (CurPoint == null)
                throw new ArgumentNullException("CurPoint");
            if (Param == null)
                throw new ArgumentNullException("Param");
            ///////////////////////////////////////////
            addVector.X = Param.Add_X;
            addVector.Y = Param.Add_Y;
            addVector.Angle = Param.Add_Theta;
            switch (Param.AlignmentMethod)
            {
                case enAlignmentMethod.单点对齐:
                    switch (Param.RefObject)
                    {
                        default:
                        case enRefObject.视野中心:
                            if (CurPoint.Length < 1)
                                throw new ArgumentException("CurPoint 的长度小于1 ");
                            OffsetXYTheta.X = (CurPoint[0].Grab_x - CurPoint[0].X) + Param.Add_X;
                            OffsetXYTheta.Y = (CurPoint[0].Grab_y - CurPoint[0].Y) + Param.Add_Y;
                            break;
                        case enRefObject.示教点:
                            if (RefPoint.Length < 1)
                                throw new ArgumentException("RefPoint 的长度小于1 ");
                            if (CurPoint.Length < 1)
                                throw new ArgumentException("CurPoint 的长度小于1 ");
                            OffsetXYTheta.X = (RefPoint[0].X - CurPoint[0].X) + Param.Add_X;
                            OffsetXYTheta.Y = (RefPoint[0].Y - CurPoint[0].Y) + Param.Add_Y;
                            break;
                    }
                    result = true;
                    break;
                case enAlignmentMethod.两点对齐_起点角度:
                    if (RefPoint.Length < 2)
                        throw new ArgumentException("RefPoint 的长度小于2 ");
                    if (CurPoint.Length < 2)
                        throw new ArgumentException("CurPoint 的长度小于2 ");
                    switch (Param.RefObject)
                    {
                        default:
                        case enRefObject.视野中心:
                            double X1 = 0 - CurPoint[0].CamParams.CaliParam.CalibCenterXy.X; // 视野中心  = 0 - 旋转中心坐标
                            double Y1 = 0 - CurPoint[0].CamParams.CaliParam.CalibCenterXy.Y;
                            double X2 = 0 - CurPoint[1].CamParams.CaliParam.CalibCenterXy.X;
                            double Y2 = 0 - CurPoint[1].CamParams.CaliParam.CalibCenterXy.Y;
                            double Angle = Math.Atan2(Y2 - Y1, X2 - X1);
                            refVectorPoint.X = X1;
                            refVectorPoint.Y = Y1;
                            refVectorPoint.Angle = Angle + Param.Add_Theta * Math.PI / 180;  //这里需要将补偿值转换成弧度
                            currentVectorPoint.X = (CurPoint[0].X);
                            currentVectorPoint.Y = (CurPoint[0].Y);
                            currentVectorPoint.Angle = Math.Atan2(CurPoint[1].Y - CurPoint[0].Y, CurPoint[1].X - CurPoint[0].X);
                            OffsetXYTheta = ha.VectorAngleToMotionXYTheta(currentVectorPoint, refVectorPoint);
                            ////////////////////////////////////////////////
                            OffsetXYTheta.X = OffsetXYTheta.X + Param.Add_X;
                            OffsetXYTheta.Y = OffsetXYTheta.Y + Param.Add_Y;
                            OffsetXYTheta.Angle = OffsetXYTheta.Angle + Param.Add_Theta;
                            break;
                        case enRefObject.示教点:
                            refVectorPoint.X = RefPoint[0].X;
                            refVectorPoint.Y = RefPoint[0].Y;
                            refVectorPoint.Angle = Math.Atan2(RefPoint[1].Y - RefPoint[0].Y, RefPoint[1].X - RefPoint[0].X);  //这里需要将补偿值转换成弧度
                            currentVectorPoint.X = (CurPoint[0].X);
                            currentVectorPoint.Y = (CurPoint[0].Y);
                            currentVectorPoint.Angle = Math.Atan2(CurPoint[1].Y - CurPoint[0].Y, CurPoint[1].X - CurPoint[0].X);
                            OffsetXYTheta = ha.VectorAngleToMotionXYTheta(currentVectorPoint, refVectorPoint);
                            ////////////////////////////////////////////////
                            OffsetXYTheta.X = OffsetXYTheta.X + Param.Add_X;
                            OffsetXYTheta.Y = OffsetXYTheta.Y + Param.Add_Y;
                            OffsetXYTheta.Angle = OffsetXYTheta.Angle + Param.Add_Theta;
                            break;
                    }
                    result = true;
                    break;
                case enAlignmentMethod.两点对齐:
                    if (RefPoint.Length < 2)
                        throw new ArgumentException("RefPoint 的长度小于2 ");
                    if (CurPoint.Length < 2)
                        throw new ArgumentException("CurPoint 的长度小于2 ");
                    switch (Param.RefObject)
                    {
                        case enRefObject.视野中心:
                            double X1 = 0 - CurPoint[0].CamParams.CaliParam.CalibCenterXy.X; // 视野中心  = 0 - 旋转中心坐标
                            double Y1 = 0 - CurPoint[0].CamParams.CaliParam.CalibCenterXy.Y;
                            double X2 = 0 - CurPoint[1].CamParams.CaliParam.CalibCenterXy.X;
                            double Y2 = 0 - CurPoint[1].CamParams.CaliParam.CalibCenterXy.Y;
                            userWcsPoint[] wcsPoints = new userWcsPoint[2];
                            wcsPoints[0].X = X1;
                            wcsPoints[0].Y = Y1;
                            wcsPoints[1].X = X2;
                            wcsPoints[1].Y = Y2;
                            /////////////////////////////////////////////////////////
                            wcsAffineRefPoints = ha.AffineTransPoint2d(wcsPoints, addVector);
                            OffsetXYTheta = ha.PointToMotionXYTheta(CurPoint, wcsAffineRefPoints);
                            break;
                        case enRefObject.示教点:
                            wcsAffineRefPoints = ha.AffineTransPoint2d(CurPoint, addVector);
                            OffsetXYTheta = ha.PointToMotionXYTheta(CurPoint, wcsAffineRefPoints);
                            break;
                    }
                    result = true;
                    break;
                case enAlignmentMethod.三点对齐:
                    if (RefPoint.Length < 3)
                        throw new ArgumentException("RefPoint 的长度小于3 ");
                    if (CurPoint.Length < 3)
                        throw new ArgumentException("CurPoint 的长度小于3 ");
                    switch (Param.RefObject)
                    {
                        case enRefObject.视野中心:
                            double X1 = 0 - CurPoint[0].CamParams.CaliParam.CalibCenterXy.X; // 视野中心  = 0 - 旋转中心坐标
                            double Y1 = 0 - CurPoint[0].CamParams.CaliParam.CalibCenterXy.Y;
                            double X2 = 0 - CurPoint[1].CamParams.CaliParam.CalibCenterXy.X;
                            double Y2 = 0 - CurPoint[1].CamParams.CaliParam.CalibCenterXy.Y;
                            double X3 = 0 - CurPoint[0].CamParams.CaliParam.CalibCenterXy.X; // 视野中心  = 0 - 旋转中心坐标
                            double Y3 = 0 - CurPoint[0].CamParams.CaliParam.CalibCenterXy.Y;

                            userWcsPoint[] wcsPoints = new userWcsPoint[2];
                            wcsPoints[0].X = X1;
                            wcsPoints[0].Y = Y1;
                            wcsPoints[1].X = X2;
                            wcsPoints[1].Y = Y2;
                            wcsPoints[2].X = X3;
                            wcsPoints[2].Y = Y3;
                            /////////////////////////////////////////////////////////
                            wcsAffineRefPoints = ha.AffineTransPoint2d(wcsPoints, addVector);
                            OffsetXYTheta = ha.PointToMotionXYTheta(CurPoint, wcsAffineRefPoints);
                            break;
                        case enRefObject.示教点:
                            wcsAffineRefPoints = ha.AffineTransPoint2d(CurPoint, addVector);
                            OffsetXYTheta = ha.PointToMotionXYTheta(CurPoint, wcsAffineRefPoints);
                            break;
                    }
                    result = true;
                    break;

                case enAlignmentMethod.四点对齐:
                    if (RefPoint.Length < 4)
                        throw new ArgumentException("RefPoint 的长度小于4 ");
                    if (CurPoint.Length < 4)
                        throw new ArgumentException("CurPoint 的长度小于4 ");
                    switch (Param.RefObject)
                    {
                        case enRefObject.视野中心:
                            double X1 = 0 - CurPoint[0].CamParams.CaliParam.CalibCenterXy.X; // 视野中心  = 0 - 旋转中心坐标
                            double Y1 = 0 - CurPoint[0].CamParams.CaliParam.CalibCenterXy.Y;
                            double X2 = 0 - CurPoint[1].CamParams.CaliParam.CalibCenterXy.X;
                            double Y2 = 0 - CurPoint[1].CamParams.CaliParam.CalibCenterXy.Y;
                            double X3 = 0 - CurPoint[0].CamParams.CaliParam.CalibCenterXy.X; // 视野中心  = 0 - 旋转中心坐标
                            double Y3 = 0 - CurPoint[0].CamParams.CaliParam.CalibCenterXy.Y;
                            double X4 = 0 - CurPoint[1].CamParams.CaliParam.CalibCenterXy.X;
                            double Y4 = 0 - CurPoint[1].CamParams.CaliParam.CalibCenterXy.Y;
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
                            wcsAffineRefPoints = ha.AffineTransPoint2d(wcsPoints, addVector);
                            OffsetXYTheta = ha.PointToMotionXYTheta(CurPoint, wcsAffineRefPoints);
                            break;
                        case enRefObject.示教点:
                            wcsAffineRefPoints = ha.AffineTransPoint2d(CurPoint, addVector);
                            OffsetXYTheta = ha.PointToMotionXYTheta(CurPoint, wcsAffineRefPoints);
                            break;
                    }
                    result = true;
                    break;
                case enAlignmentMethod.N点对齐:
                    if (RefPoint.Length != CurPoint.Length)
                        throw new ArgumentException("RefPoint 的长度与 CurPoint 的长度不相等 ");
                    switch (Param.RefObject)
                    {
                        case enRefObject.视野中心:
                            List<userWcsPoint> listWcsPoint = new List<userWcsPoint>();
                            foreach (var item in CurPoint)
                            {
                                listWcsPoint.Add(new userWcsPoint(0 - item.CamParams.CaliParam.CalibCenterXy.X, 0 - item.CamParams.CaliParam.CalibCenterXy.Y, 0));
                            }
                            /////////////////////////////////////////////////////////
                            wcsAffineRefPoints = ha.AffineTransPoint2d(listWcsPoint.ToArray(), addVector);
                            OffsetXYTheta = ha.PointToMotionXYTheta(CurPoint, wcsAffineRefPoints); // 当前点到参考点的对齐
                            break;
                        case enRefObject.示教点:
                            wcsAffineRefPoints = ha.AffineTransPoint2d(CurPoint, addVector);
                            OffsetXYTheta = ha.PointToMotionXYTheta(CurPoint, wcsAffineRefPoints);
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

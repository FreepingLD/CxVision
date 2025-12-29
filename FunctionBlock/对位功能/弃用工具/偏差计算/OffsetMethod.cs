using AlgorithmsLibrary;
using Common;
using HalconDotNet;
using System;
using System.Collections.Generic;

namespace FunctionBlock
{
    public class OffsetMethod
    {

        public static bool CalculateAlign(userWcsPoint[] TargetPoint, userWcsPoint[] SourcePoint, CompensationParam Param,  out userWcsVector AddXYTheta)
        {
            bool result = false;
            userWcsVector refVectorPoint = new userWcsVector();
            userWcsVector currentVectorPoint = new userWcsVector();
            AddXYTheta = new userWcsVector();
            if (SourcePoint == null)
                throw new ArgumentNullException(" SourcePoint ");
            if (Param == null)
                throw new ArgumentNullException(" Param ");
            ///////////////////////////////////////////
            switch (Param.OffsetMethod)
            {
                case enOffsetMethod.两点向量差:
                    switch (Param.RefObject)
                    {
                        default:
                        case enRefObject.视野中心:
                            if (SourcePoint.Length < 2)
                                throw new ArgumentException("SourcePoint 的长度小于2 ");
                            double X1, Y1, Z1, X2, Y2, Z2;
                            SourcePoint[0].CamParams.ImageCenterPointsToWorldPlane(SourcePoint[0].Grab_x, SourcePoint[0].Grab_y, SourcePoint[0].Grab_z, out X1, out Y1, out Z1);
                            SourcePoint[1].CamParams.ImageCenterPointsToWorldPlane(SourcePoint[1].Grab_x, SourcePoint[1].Grab_y, SourcePoint[1].Grab_z, out X2, out Y2, out Z2);
                            double Angle = Math.Atan2(Y2 - Y1, X2 - X1);
                            refVectorPoint.X = (X1 + X2) * 0.5;
                            refVectorPoint.Y = (Y1 + Y2) * 0.5;
                            refVectorPoint.Angle = Angle;  //这里需要将补偿值转换成弧度 + Param.Add_Theta * Math.PI / 180
                            currentVectorPoint.X = (SourcePoint[0].X + SourcePoint[1].X) * 0.5;
                            currentVectorPoint.Y = (SourcePoint[0].Y + SourcePoint[1].Y) * 0.5;
                            currentVectorPoint.Angle = Math.Atan2(SourcePoint[1].Y - SourcePoint[0].Y, SourcePoint[1].X - SourcePoint[0].X);
                            //////////////////////////////////////////////////
                            AddXYTheta = new userWcsVector();
                            AddXYTheta.X = currentVectorPoint.X - refVectorPoint.X;
                            AddXYTheta.Y = currentVectorPoint.Y - refVectorPoint.Y;
                            AddXYTheta.Angle = currentVectorPoint.Angle - refVectorPoint.Angle;
                            break;
                        case enRefObject.示教点:
                            if (TargetPoint.Length < 2)
                                throw new ArgumentException("TargetPoint 的长度小于2 ");
                            if (SourcePoint.Length < 2)
                                throw new ArgumentException("SourcePoint 的长度小于2 ");
                            refVectorPoint.X = (TargetPoint[0].X + TargetPoint[1].X)*0.5;
                            refVectorPoint.Y = (TargetPoint[0].Y + TargetPoint[1].Y)*0.5;
                            refVectorPoint.Angle = Math.Atan2(TargetPoint[1].Y - TargetPoint[0].Y, TargetPoint[1].X - TargetPoint[0].X);  //这里需要将补偿值转换成弧度
                            currentVectorPoint.X = (SourcePoint[0].X + SourcePoint[1].X) * 0.5;
                            currentVectorPoint.Y = (SourcePoint[0].Y + SourcePoint[1].Y) * 0.5;
                            currentVectorPoint.Angle = Math.Atan2(SourcePoint[1].Y - SourcePoint[0].Y, SourcePoint[1].X - SourcePoint[0].X);
                            //////////////////////////////////////////////////
                            AddXYTheta = new userWcsVector();
                            AddXYTheta.X = currentVectorPoint.X - refVectorPoint.X;
                            AddXYTheta.Y = currentVectorPoint.Y - refVectorPoint.Y;
                            AddXYTheta.Angle= currentVectorPoint.Angle - refVectorPoint.Angle;
                            break;
                    }
                    result = true;
                    break;
                case enOffsetMethod.三点向量差:
                    switch (Param.RefObject)
                    {
                        default:
                        case enRefObject.视野中心:
                            if (SourcePoint.Length < 3)
                                throw new ArgumentException("SourcePoint 的长度小于3 ");
                            double X1, Y1, Z1, X2, Y2, Z2, X3, Y3, Z3;
                            SourcePoint[0].CamParams.ImageCenterPointsToWorldPlane(SourcePoint[0].Grab_x, SourcePoint[0].Grab_y, SourcePoint[0].Grab_z, out X1, out Y1, out Z1);
                            SourcePoint[1].CamParams.ImageCenterPointsToWorldPlane(SourcePoint[1].Grab_x, SourcePoint[1].Grab_y, SourcePoint[1].Grab_z, out X2, out Y2, out Z2);
                            SourcePoint[2].CamParams.ImageCenterPointsToWorldPlane(SourcePoint[2].Grab_x, SourcePoint[2].Grab_y, SourcePoint[2].Grab_z, out X3, out Y3, out Z3);
                            double Angle = Math.Atan2(Y2 - Y1, X2 - X1);
                            refVectorPoint.X = (X1 + X2 + X3) / 3.0;
                            refVectorPoint.Y = (Y1 + Y2 + Y3) / 3.0;
                            refVectorPoint.Angle = Angle;  //这里需要将补偿值转换成弧度 + Param.Add_Theta * Math.PI / 180
                            currentVectorPoint.X = (SourcePoint[0].X + SourcePoint[1].X + SourcePoint[2].X) / 3.0;
                            currentVectorPoint.Y = (SourcePoint[0].Y + SourcePoint[1].Y + SourcePoint[2].Y) / 3.0;
                            currentVectorPoint.Angle = Math.Atan2(SourcePoint[1].Y - SourcePoint[0].Y, SourcePoint[1].X - SourcePoint[0].X);
                            //////////////////////////////////////////////////
                            AddXYTheta = new userWcsVector();
                            AddXYTheta.X = currentVectorPoint.X - refVectorPoint.X;
                            AddXYTheta.Y = currentVectorPoint.Y - refVectorPoint.Y;
                            AddXYTheta.Angle = currentVectorPoint.Angle - refVectorPoint.Angle;
                            break;
                        case enRefObject.示教点:
                            if (TargetPoint.Length < 3)
                                throw new ArgumentException("TargetPoint 的长度小于3 ");
                            if (SourcePoint.Length < 3)
                                throw new ArgumentException("SourcePoint 的长度小于3 ");
                            refVectorPoint.X = (TargetPoint[0].X + TargetPoint[1].X + TargetPoint[2].X) / 3.0;
                            refVectorPoint.Y = (TargetPoint[0].Y + TargetPoint[1].Y + TargetPoint[2].Y) / 3.0;
                            refVectorPoint.Angle = Math.Atan2(TargetPoint[1].Y - TargetPoint[0].Y, TargetPoint[1].X - TargetPoint[0].X);  //这里需要将补偿值转换成弧度
                            currentVectorPoint.X = (SourcePoint[0].X + SourcePoint[1].X + SourcePoint[2].X) / 3.0;
                            currentVectorPoint.Y = (SourcePoint[0].Y + SourcePoint[1].Y + SourcePoint[2].Y) / 3.0;
                            currentVectorPoint.Angle = Math.Atan2(SourcePoint[1].Y - SourcePoint[0].Y, SourcePoint[1].X - SourcePoint[0].X);
                            //////////////////////////////////////////////////
                            AddXYTheta = new userWcsVector();
                            AddXYTheta.X = currentVectorPoint.X - refVectorPoint.X;
                            AddXYTheta.Y = currentVectorPoint.Y - refVectorPoint.Y;
                            AddXYTheta.Angle = currentVectorPoint.Angle - refVectorPoint.Angle;
                            break;
                    }
                    result = true;
                    break;
                case enOffsetMethod.四点向量差:
                    switch (Param.RefObject)
                    {
                        default:
                        case enRefObject.视野中心:
                            if (SourcePoint.Length < 4)
                                throw new ArgumentException("SourcePoint 的长度小于4 ");
                            double X1, Y1, Z1, X2, Y2, Z2, X3, Y3, Z3, X4, Y4, Z4;
                            SourcePoint[0].CamParams.ImageCenterPointsToWorldPlane(SourcePoint[0].Grab_x, SourcePoint[0].Grab_y, SourcePoint[0].Grab_z, out X1, out Y1, out Z1);
                            SourcePoint[1].CamParams.ImageCenterPointsToWorldPlane(SourcePoint[1].Grab_x, SourcePoint[1].Grab_y, SourcePoint[1].Grab_z, out X2, out Y2, out Z2);
                            SourcePoint[2].CamParams.ImageCenterPointsToWorldPlane(SourcePoint[2].Grab_x, SourcePoint[2].Grab_y, SourcePoint[2].Grab_z, out X3, out Y3, out Z3);
                            SourcePoint[3].CamParams.ImageCenterPointsToWorldPlane(SourcePoint[3].Grab_x, SourcePoint[3].Grab_y, SourcePoint[3].Grab_z, out X4, out Y4, out Z4);
                            double Angle = Math.Atan2(Y2 - Y1, X2 - X1);
                            refVectorPoint.X = (X1 + X2 + X3 + X4) * 0.25;
                            refVectorPoint.Y = (Y1 + Y2 + Y3 + Y4) * 0.25;
                            refVectorPoint.Angle = Angle;  //这里需要将补偿值转换成弧度 + Param.Add_Theta * Math.PI / 180
                            currentVectorPoint.X = (SourcePoint[0].X + SourcePoint[1].X + SourcePoint[2].X + SourcePoint[3].X) * 0.25;
                            currentVectorPoint.Y = (SourcePoint[0].Y + SourcePoint[1].Y + SourcePoint[2].Y + SourcePoint[3].Y) * 0.25;
                            currentVectorPoint.Angle = Math.Atan2(SourcePoint[1].Y - SourcePoint[0].Y, SourcePoint[1].X - SourcePoint[0].X);
                            //////////////////////////////////////////////////
                            AddXYTheta = new userWcsVector();
                            AddXYTheta.X = currentVectorPoint.X - refVectorPoint.X;
                            AddXYTheta.Y = currentVectorPoint.Y - refVectorPoint.Y;
                            AddXYTheta.Angle = currentVectorPoint.Angle - refVectorPoint.Angle;
                            break;
                        case enRefObject.示教点:
                            if (TargetPoint.Length < 4)
                                throw new ArgumentException("TargetPoint 的长度小于4 ");
                            if (SourcePoint.Length < 4)
                                throw new ArgumentException("SourcePoint 的长度小于4 ");
                            refVectorPoint.X = (TargetPoint[0].X + TargetPoint[1].X + TargetPoint[2].X + TargetPoint[3].X) * 0.25;
                            refVectorPoint.Y = (TargetPoint[0].Y + TargetPoint[1].Y + TargetPoint[2].Y + TargetPoint[3].Y) * 0.25;
                            refVectorPoint.Angle = Math.Atan2(TargetPoint[1].Y - TargetPoint[0].Y, TargetPoint[1].X - TargetPoint[0].X);  //这里需要将补偿值转换成弧度
                            currentVectorPoint.X = (SourcePoint[0].X + SourcePoint[1].X + SourcePoint[2].X + SourcePoint[3].X) * 0.25;
                            currentVectorPoint.Y = (SourcePoint[0].Y + SourcePoint[1].Y + SourcePoint[2].Y + SourcePoint[3].Y) * 0.25;
                            currentVectorPoint.Angle = Math.Atan2(SourcePoint[1].Y - SourcePoint[0].Y, SourcePoint[1].X - SourcePoint[0].X);
                            //////////////////////////////////////////////////
                            AddXYTheta = new userWcsVector();
                            AddXYTheta.X = currentVectorPoint.X - refVectorPoint.X;
                            AddXYTheta.Y = currentVectorPoint.Y - refVectorPoint.Y;
                            AddXYTheta.Angle = currentVectorPoint.Angle - refVectorPoint.Angle;
                            break;
                    }
                    result = true;
                    break;
                default:
                    throw new ArgumentException("小于所需的对齐点");
            }
            return result;
        }

        public static bool CalculateAlign2(userWcsPoint[] TargetPoint, userWcsPoint[] SourcePoint, CompensationParam Param, out userWcsVector AddXYTheta)
        {
            bool result = false;
            userWcsVector refVectorPoint = new userWcsVector();
            userWcsVector currentVectorPoint = new userWcsVector();
            AddXYTheta = new userWcsVector();
            if (SourcePoint == null)
                throw new ArgumentNullException(" SourcePoint ");
            if (TargetPoint == null)
                throw new ArgumentNullException(" TargetPoint ");
            if (Param == null)
                throw new ArgumentNullException(" Param ");
            ///////////////////////////////////////////
            switch (Param.OffsetMethod)
            {
                case enOffsetMethod.两点向量差:
                    switch (Param.RefObject)
                    {
                        default:
                        case enRefObject.视野中心:
                            if (SourcePoint.Length < 2)
                                throw new ArgumentException("SourcePoint 的长度小于2 ");
                            double X1, Y1, Z1, X2, Y2, Z2;
                            SourcePoint[0].CamParams.ImageCenterPointsToWorldPlane(SourcePoint[0].Grab_x, SourcePoint[0].Grab_y, SourcePoint[0].Grab_z, out X1, out Y1, out Z1);
                            SourcePoint[1].CamParams.ImageCenterPointsToWorldPlane(SourcePoint[1].Grab_x, SourcePoint[1].Grab_y, SourcePoint[1].Grab_z, out X2, out Y2, out Z2);
                            double Angle = Math.Atan2(Y2 - Y1, X2 - X1);
                            refVectorPoint.X = (X1 + X2) * 0.5;
                            refVectorPoint.Y = (Y1 + Y2) * 0.5;
                            refVectorPoint.Angle = Angle;  //这里需要将补偿值转换成弧度 + Param.Add_Theta * Math.PI / 180
                            currentVectorPoint.X = (SourcePoint[0].X + SourcePoint[1].X) * 0.5;
                            currentVectorPoint.Y = (SourcePoint[0].Y + SourcePoint[1].Y) * 0.5;
                            currentVectorPoint.Angle = Math.Atan2(SourcePoint[1].Y - SourcePoint[0].Y, SourcePoint[1].X - SourcePoint[0].X);
                            //////////////////////////////////////////////////
                            AddXYTheta = new userWcsVector();
                            AddXYTheta.X = currentVectorPoint.X - refVectorPoint.X;
                            AddXYTheta.Y = currentVectorPoint.Y - refVectorPoint.Y;
                            AddXYTheta.Angle = currentVectorPoint.Angle - refVectorPoint.Angle;
                            break;
                        case enRefObject.示教点:
                        case enRefObject.目标点:
                            if (TargetPoint.Length < 2)
                                throw new ArgumentException("TargetPoint 的长度小于2 ");
                            if (SourcePoint.Length < 2)
                                throw new ArgumentException("SourcePoint 的长度小于2 ");
                            refVectorPoint.X = (TargetPoint[0].X + TargetPoint[1].X) * 0.5;
                            refVectorPoint.Y = (TargetPoint[0].Y + TargetPoint[1].Y) * 0.5;
                            refVectorPoint.Angle = Math.Atan2(TargetPoint[1].Y - TargetPoint[0].Y, TargetPoint[1].X - TargetPoint[0].X);  //这里需要将补偿值转换成弧度
                            currentVectorPoint.X = (SourcePoint[0].X + SourcePoint[1].X) * 0.5;
                            currentVectorPoint.Y = (SourcePoint[0].Y + SourcePoint[1].Y) * 0.5;
                            currentVectorPoint.Angle = Math.Atan2(SourcePoint[1].Y - SourcePoint[0].Y, SourcePoint[1].X - SourcePoint[0].X);
                            //////////////////////////////////////////////////
                            AddXYTheta = new userWcsVector();
                            AddXYTheta.X = currentVectorPoint.X - refVectorPoint.X;
                            AddXYTheta.Y = currentVectorPoint.Y - refVectorPoint.Y;
                            AddXYTheta.Angle = currentVectorPoint.Angle - refVectorPoint.Angle;
                            break;
                    }
                    result = true;
                    break;
                case enOffsetMethod.三点向量差:
                    switch (Param.RefObject)
                    {
                        default:
                        case enRefObject.视野中心:
                            if (SourcePoint.Length < 3)
                                throw new ArgumentException("SourcePoint 的长度小于3 ");
                            double X1, Y1, Z1, X2, Y2, Z2, X3, Y3, Z3;
                            SourcePoint[0].CamParams.ImageCenterPointsToWorldPlane(SourcePoint[0].Grab_x, SourcePoint[0].Grab_y, SourcePoint[0].Grab_z, out X1, out Y1, out Z1);
                            SourcePoint[1].CamParams.ImageCenterPointsToWorldPlane(SourcePoint[1].Grab_x, SourcePoint[1].Grab_y, SourcePoint[1].Grab_z, out X2, out Y2, out Z2);
                            SourcePoint[2].CamParams.ImageCenterPointsToWorldPlane(SourcePoint[2].Grab_x, SourcePoint[2].Grab_y, SourcePoint[2].Grab_z, out X3, out Y3, out Z3);
                            double Angle = Math.Atan2(Y2 - Y1, X2 - X1);
                            refVectorPoint.X = (X1 + X2 + X3) / 3.0;
                            refVectorPoint.Y = (Y1 + Y2 + Y3) / 3.0;
                            refVectorPoint.Angle = Angle;  //这里需要将补偿值转换成弧度 + Param.Add_Theta * Math.PI / 180
                            currentVectorPoint.X = (SourcePoint[0].X + SourcePoint[1].X + SourcePoint[2].X) / 3.0;
                            currentVectorPoint.Y = (SourcePoint[0].Y + SourcePoint[1].Y + SourcePoint[2].Y) / 3.0;
                            currentVectorPoint.Angle = Math.Atan2(SourcePoint[1].Y - SourcePoint[0].Y, SourcePoint[1].X - SourcePoint[0].X);
                            //////////////////////////////////////////////////
                            AddXYTheta = new userWcsVector();
                            AddXYTheta.X = currentVectorPoint.X - refVectorPoint.X;
                            AddXYTheta.Y = currentVectorPoint.Y - refVectorPoint.Y;
                            AddXYTheta.Angle = currentVectorPoint.Angle - refVectorPoint.Angle;
                            break;
                        case enRefObject.示教点:
                        case enRefObject.目标点:
                            if (TargetPoint.Length < 3)
                                throw new ArgumentException("TargetPoint 的长度小于3 ");
                            if (SourcePoint.Length < 3)
                                throw new ArgumentException("SourcePoint 的长度小于3 ");
                            refVectorPoint.X = (TargetPoint[0].X + TargetPoint[1].X + TargetPoint[2].X) / 3.0;
                            refVectorPoint.Y = (TargetPoint[0].Y + TargetPoint[1].Y + TargetPoint[2].Y) / 3.0;
                            refVectorPoint.Angle = Math.Atan2(TargetPoint[1].Y - TargetPoint[0].Y, TargetPoint[1].X - TargetPoint[0].X);  //这里需要将补偿值转换成弧度
                            currentVectorPoint.X = (SourcePoint[0].X + SourcePoint[1].X + SourcePoint[2].X) / 3.0;
                            currentVectorPoint.Y = (SourcePoint[0].Y + SourcePoint[1].Y + SourcePoint[2].Y) / 3.0;
                            currentVectorPoint.Angle = Math.Atan2(SourcePoint[1].Y - SourcePoint[0].Y, SourcePoint[1].X - SourcePoint[0].X);
                            //////////////////////////////////////////////////
                            AddXYTheta = new userWcsVector();
                            AddXYTheta.X = currentVectorPoint.X - refVectorPoint.X;
                            AddXYTheta.Y = currentVectorPoint.Y - refVectorPoint.Y;
                            AddXYTheta.Angle = currentVectorPoint.Angle - refVectorPoint.Angle;
                            break;
                    }
                    result = true;
                    break;
                case enOffsetMethod.四点向量差:
                    switch (Param.RefObject)
                    {
                        default:
                        case enRefObject.视野中心:
                            if (SourcePoint.Length < 4)
                                throw new ArgumentException("SourcePoint 的长度小于4 ");
                            double X1, Y1, Z1, X2, Y2, Z2, X3, Y3, Z3, X4, Y4, Z4;
                            SourcePoint[0].CamParams.ImageCenterPointsToWorldPlane(SourcePoint[0].Grab_x, SourcePoint[0].Grab_y, SourcePoint[0].Grab_z, out X1, out Y1, out Z1);
                            SourcePoint[1].CamParams.ImageCenterPointsToWorldPlane(SourcePoint[1].Grab_x, SourcePoint[1].Grab_y, SourcePoint[1].Grab_z, out X2, out Y2, out Z2);
                            SourcePoint[2].CamParams.ImageCenterPointsToWorldPlane(SourcePoint[2].Grab_x, SourcePoint[2].Grab_y, SourcePoint[2].Grab_z, out X3, out Y3, out Z3);
                            SourcePoint[3].CamParams.ImageCenterPointsToWorldPlane(SourcePoint[3].Grab_x, SourcePoint[3].Grab_y, SourcePoint[3].Grab_z, out X4, out Y4, out Z4);
                            double Angle = Math.Atan2(Y2 - Y1, X2 - X1);
                            refVectorPoint.X = (X1 + X2 + X3 + X4) * 0.25;
                            refVectorPoint.Y = (Y1 + Y2 + Y3 + Y4) * 0.25;
                            refVectorPoint.Angle = Angle;  //这里需要将补偿值转换成弧度 + Param.Add_Theta * Math.PI / 180
                            currentVectorPoint.X = (SourcePoint[0].X + SourcePoint[1].X + SourcePoint[2].X + SourcePoint[3].X) * 0.25;
                            currentVectorPoint.Y = (SourcePoint[0].Y + SourcePoint[1].Y + SourcePoint[2].Y + SourcePoint[3].Y) * 0.25;
                            currentVectorPoint.Angle = Math.Atan2(SourcePoint[1].Y - SourcePoint[0].Y, SourcePoint[1].X - SourcePoint[0].X);
                            //////////////////////////////////////////////////
                            AddXYTheta = new userWcsVector();
                            AddXYTheta.X = currentVectorPoint.X - refVectorPoint.X;
                            AddXYTheta.Y = currentVectorPoint.Y - refVectorPoint.Y;
                            AddXYTheta.Angle = currentVectorPoint.Angle - refVectorPoint.Angle;
                            break;
                        case enRefObject.示教点:
                        case enRefObject.目标点:
                            if (TargetPoint.Length < 4)
                                throw new ArgumentException("TargetPoint 的长度小于4 ");
                            if (SourcePoint.Length < 4)
                                throw new ArgumentException("SourcePoint 的长度小于4 ");
                            refVectorPoint.X = (TargetPoint[0].X + TargetPoint[1].X + TargetPoint[2].X + TargetPoint[3].X) * 0.25;
                            refVectorPoint.Y = (TargetPoint[0].Y + TargetPoint[1].Y + TargetPoint[2].Y + TargetPoint[3].Y) * 0.25;
                            refVectorPoint.Angle = Math.Atan2(TargetPoint[1].Y - TargetPoint[0].Y, TargetPoint[1].X - TargetPoint[0].X);  //这里需要将补偿值转换成弧度
                            currentVectorPoint.X = (SourcePoint[0].X + SourcePoint[1].X + SourcePoint[2].X + SourcePoint[3].X) * 0.25;
                            currentVectorPoint.Y = (SourcePoint[0].Y + SourcePoint[1].Y + SourcePoint[2].Y + SourcePoint[3].Y) * 0.25;
                            currentVectorPoint.Angle = Math.Atan2(SourcePoint[1].Y - SourcePoint[0].Y, SourcePoint[1].X - SourcePoint[0].X);
                            //////////////////////////////////////////////////
                            AddXYTheta = new userWcsVector();
                            AddXYTheta.X = currentVectorPoint.X - refVectorPoint.X;
                            AddXYTheta.Y = currentVectorPoint.Y - refVectorPoint.Y;
                            AddXYTheta.Angle = currentVectorPoint.Angle - refVectorPoint.Angle;
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

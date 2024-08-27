using AlgorithmsLibrary;
using Common;
using HalconDotNet;
using System;
using System.Collections.Generic;

namespace FunctionBlock
{
    public class AlignMethod
    {

        public static bool CalculateAlign(userWcsPoint[] TargetPoint, userWcsPoint[] SourcePoint, CompensationParam Param, out userWcsPoint[] affinePoints, out userWcsVector AddXYTheta)
        {
            bool result = false;
            HalconLibrary ha = new HalconLibrary();
            userWcsVector refVectorPoint = new userWcsVector();
            userWcsVector currentVectorPoint = new userWcsVector();
            userWcsVector addVector = new userWcsVector();
            HalconDotNet.HHomMat2D hHomMat2D = null, hHomMat2DAdd = null, homMat2DCompose = null;
            AddXYTheta = new userWcsVector();
            affinePoints = new userWcsPoint[0];
            if (SourcePoint == null)
                throw new ArgumentNullException(" SourcePoint ");
            if (Param == null)
                throw new ArgumentNullException(" Param ");
            ///////////////////////////////////////////
            addVector.X = Param.Add_X;
            addVector.Y = Param.Add_Y;
            addVector.Angle = Param.Add_Angle;
            switch (Param.AlignmentMethod)
            {
                case enAlignmentMethod.单点对齐:
                    affinePoints = new userWcsPoint[1];
                    switch (Param.RefObject)
                    {
                        default:
                        case enRefObject.视野中心:
                            if (SourcePoint.Length < 1)
                                throw new ArgumentException("CurPoint 的长度小于1 ");
                            double X, Y, Z;
                            SourcePoint[0].CamParams.ImageCenterPointsToWorldPlane(SourcePoint[0].Grab_x, SourcePoint[0].Grab_y, SourcePoint[0].Grab_z, out X, out Y, out Z);
                            AddXYTheta.X = (X - SourcePoint[0].X) + Param.Add_X;
                            AddXYTheta.Y = (Y - SourcePoint[0].Y) + Param.Add_Y;
                            AddXYTheta.Angle =  0;
                            break;
                        case enRefObject.示教点:
                            if (TargetPoint.Length < 1)
                                throw new ArgumentException("RefPoint 的长度小于1 ");
                            if (SourcePoint.Length < 1)
                                throw new ArgumentException("CurPoint 的长度小于1 ");
                            AddXYTheta.X = (TargetPoint[0].X - SourcePoint[0].X) + Param.Add_X;
                            AddXYTheta.Y = (TargetPoint[0].Y - SourcePoint[0].Y) + Param.Add_Y;
                            AddXYTheta.Angle = 0;
                            break;
                    }
                    affinePoints[0].X = SourcePoint[0].X + AddXYTheta.X;
                    affinePoints[0].Y = SourcePoint[0].Y + AddXYTheta.Y;
                    result = true;
                    break;
                case enAlignmentMethod.两点对齐_起点角度:
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
                            refVectorPoint.X = X1;
                            refVectorPoint.Y = Y1;
                            refVectorPoint.Angle = Angle;  //这里需要将补偿值转换成弧度 + Param.Add_Theta * Math.PI / 180
                            currentVectorPoint.X = (SourcePoint[0].X);
                            currentVectorPoint.Y = (SourcePoint[0].Y);
                            currentVectorPoint.Angle = Math.Atan2(SourcePoint[1].Y - SourcePoint[0].Y, SourcePoint[1].X - SourcePoint[0].X);
                            //////////////////////////////////////////////////
                            hHomMat2D = ha.GetHomMat2D(currentVectorPoint, refVectorPoint);
                            hHomMat2DAdd = ha.GetHomMat2D(new userWcsVector(), addVector);
                            homMat2DCompose = hHomMat2D.HomMat2dCompose(hHomMat2DAdd);
                            AddXYTheta = ha.GetHomMat2DXYTheta(homMat2DCompose);
                            break;
                        case enRefObject.示教点:
                            if (TargetPoint.Length < 2)
                                throw new ArgumentException("TargetPoint 的长度小于2 ");
                            if (SourcePoint.Length < 2)
                                throw new ArgumentException("SourcePoint 的长度小于2 ");
                            refVectorPoint.X = TargetPoint[0].X;
                            refVectorPoint.Y = TargetPoint[0].Y;
                            refVectorPoint.Angle = Math.Atan2(TargetPoint[1].Y - TargetPoint[0].Y, TargetPoint[1].X - TargetPoint[0].X);  //这里需要将补偿值转换成弧度
                            currentVectorPoint.X = (SourcePoint[0].X);
                            currentVectorPoint.Y = (SourcePoint[0].Y);
                            currentVectorPoint.Angle = Math.Atan2(SourcePoint[1].Y - SourcePoint[0].Y, SourcePoint[1].X - SourcePoint[0].X);
                            //////////////////////////////////////////////////
                            hHomMat2D = ha.GetHomMat2D(currentVectorPoint, refVectorPoint);
                            hHomMat2DAdd = ha.GetHomMat2D(new userWcsVector(), addVector);
                            homMat2DCompose = hHomMat2D.HomMat2dCompose(hHomMat2DAdd);
                            AddXYTheta = ha.GetHomMat2DXYTheta(homMat2DCompose);
                            break;
                    }
                    if (homMat2DCompose != null)
                    {
                        HTuple Qx, Qy;
                        Qx = hHomMat2D.AffineTransPoint2d(currentVectorPoint.X, currentVectorPoint.Y, out Qy);
                        affinePoints = new userWcsPoint[1];
                        affinePoints[0].X = Qx.D;
                        affinePoints[0].Y = Qy.D;
                        affinePoints[0].Z = 0;
                    }
                    result = true;
                    break;
                case enAlignmentMethod.两点对齐:
                    switch (Param.RefObject)
                    {
                        case enRefObject.视野中心:
                            if (SourcePoint.Length < 2)
                                throw new ArgumentException("SourcePoint 的长度小于2 ");
                            double X1, Y1, Z1, X2, Y2, Z2;
                            SourcePoint[0].CamParams.ImageCenterPointsToWorldPlane(SourcePoint[0].Grab_x, SourcePoint[0].Grab_y, SourcePoint[0].Grab_z, out X1, out Y1, out Z1);
                            SourcePoint[1].CamParams.ImageCenterPointsToWorldPlane(SourcePoint[1].Grab_x, SourcePoint[1].Grab_y, SourcePoint[1].Grab_z, out X2, out Y2, out Z2);
                            userWcsPoint[] wcsPoints = new userWcsPoint[2];
                            wcsPoints[0].X = X1;
                            wcsPoints[0].Y = Y1;
                            wcsPoints[1].X = X2;
                            wcsPoints[1].Y = Y2;
                            /////////////////////////////////////////////////////////
                            hHomMat2D = ha.GetHomMat2D(SourcePoint, wcsPoints);
                            hHomMat2DAdd = ha.GetHomMat2D(new userWcsVector(), addVector);
                            homMat2DCompose = hHomMat2D.HomMat2dCompose(hHomMat2DAdd);
                            AddXYTheta = ha.GetHomMat2DXYTheta(homMat2DCompose);
                            break;
                        case enRefObject.示教点:
                        case enRefObject.目标点:
                            if (TargetPoint.Length < 2)
                                throw new ArgumentException("TargetPoint 的长度小于2 ");
                            if (SourcePoint.Length < 2)
                                throw new ArgumentException("SourcePoint 的长度小于2 ");
                            hHomMat2D = ha.GetHomMat2D(SourcePoint, TargetPoint);
                            hHomMat2DAdd = ha.GetHomMat2D(new userWcsVector(), addVector);
                            homMat2DCompose = hHomMat2D.HomMat2dCompose(hHomMat2DAdd);
                            AddXYTheta = ha.GetHomMat2DXYTheta(homMat2DCompose);
                            break;
                    }
                    if (homMat2DCompose != null)
                        affinePoints = ha.AffineTransPoint2d(homMat2DCompose, SourcePoint);
                    result = true;
                    break;
                case enAlignmentMethod.三点对齐:
                    switch (Param.RefObject)
                    {
                        case enRefObject.视野中心:
                            if (SourcePoint.Length < 3)
                                throw new ArgumentException("SourcePoint 的长度小于3 ");
                            double X1, Y1, Z1, X2, Y2, Z2,X3,Y3,Z3;
                            SourcePoint[0].CamParams.ImageCenterPointsToWorldPlane(SourcePoint[0].Grab_x, SourcePoint[0].Grab_y, SourcePoint[0].Grab_z, out X1, out Y1, out Z1);
                            SourcePoint[1].CamParams.ImageCenterPointsToWorldPlane(SourcePoint[1].Grab_x, SourcePoint[1].Grab_y, SourcePoint[1].Grab_z, out X2, out Y2, out Z2);
                            SourcePoint[2].CamParams.ImageCenterPointsToWorldPlane(SourcePoint[2].Grab_x, SourcePoint[2].Grab_y, SourcePoint[2].Grab_z, out X3, out Y3, out Z3);
                            userWcsPoint[] wcsPoints = new userWcsPoint[3];
                            wcsPoints[0].X = X1;
                            wcsPoints[0].Y = Y1;
                            wcsPoints[1].X = X2;
                            wcsPoints[1].Y = Y2;
                            wcsPoints[2].X = X3;
                            wcsPoints[2].Y = Y3;
                            /////////////////////////////////////////////////////////
                            hHomMat2D = ha.GetHomMat2D(SourcePoint, wcsPoints);
                            hHomMat2DAdd = ha.GetHomMat2D(new userWcsVector(), addVector);
                            homMat2DCompose = hHomMat2D.HomMat2dCompose(hHomMat2DAdd);
                            AddXYTheta = ha.GetHomMat2DXYTheta(homMat2DCompose);
                            break;
                        case enRefObject.示教点:
                        case enRefObject.目标点:
                            if (TargetPoint.Length < 3)
                                throw new ArgumentException("TargetPoint 的长度小于3 ");
                            if (SourcePoint.Length < 3)
                                throw new ArgumentException("SourcePoint 的长度小于3 ");
                            hHomMat2D = ha.GetHomMat2D(SourcePoint, TargetPoint);
                            hHomMat2DAdd = ha.GetHomMat2D(new userWcsVector(), addVector);
                            homMat2DCompose = hHomMat2D.HomMat2dCompose(hHomMat2DAdd);
                            AddXYTheta = ha.GetHomMat2DXYTheta(homMat2DCompose);
                            break;
                    }
                    if (homMat2DCompose != null)
                        affinePoints = ha.AffineTransPoint2d(homMat2DCompose, SourcePoint);
                    result = true;
                    break;

                case enAlignmentMethod.四点对齐:
                    switch (Param.RefObject)
                    {
                        case enRefObject.视野中心:
                            if (SourcePoint.Length < 4)
                                throw new ArgumentException("SourcePoint 的长度小于4 ");
                            double X1, Y1, Z1, X2, Y2, Z2, X3, Y3, Z3, X4, Y4, Z4;
                            SourcePoint[0].CamParams.ImageCenterPointsToWorldPlane(SourcePoint[0].Grab_x, SourcePoint[0].Grab_y, SourcePoint[0].Grab_z, out X1, out Y1, out Z1);
                            SourcePoint[1].CamParams.ImageCenterPointsToWorldPlane(SourcePoint[1].Grab_x, SourcePoint[1].Grab_y, SourcePoint[1].Grab_z, out X2, out Y2, out Z2);
                            SourcePoint[2].CamParams.ImageCenterPointsToWorldPlane(SourcePoint[2].Grab_x, SourcePoint[2].Grab_y, SourcePoint[2].Grab_z, out X3, out Y3, out Z3);
                            SourcePoint[3].CamParams.ImageCenterPointsToWorldPlane(SourcePoint[3].Grab_x, SourcePoint[3].Grab_y, SourcePoint[3].Grab_z, out X4, out Y4, out Z4);
                            userWcsPoint[] wcsPoints = new userWcsPoint[4];
                            wcsPoints[0].X = X1;
                            wcsPoints[0].Y = Y1;
                            wcsPoints[1].X = X2;
                            wcsPoints[1].Y = Y2;
                            wcsPoints[2].X = X3;
                            wcsPoints[2].Y = Y3;
                            wcsPoints[3].X = X4;
                            wcsPoints[3].Y = Y4;
                            /////////////////////////////////////////////////////////
                            hHomMat2D = ha.GetHomMat2D(SourcePoint, wcsPoints);
                            hHomMat2DAdd = ha.GetHomMat2D(new userWcsVector(), addVector);
                            homMat2DCompose = hHomMat2D.HomMat2dCompose(hHomMat2DAdd);
                            AddXYTheta = ha.GetHomMat2DXYTheta(homMat2DCompose);
                            break;
                        case enRefObject.示教点:
                        case enRefObject.目标点:
                            ///////////////////////////////////////////////
                            if (TargetPoint.Length < 4)
                                throw new ArgumentException("TargetPoint 的长度小于4 ");
                            if (SourcePoint.Length < 4)
                                throw new ArgumentException("SourcePoint 的长度小于4 ");
                            hHomMat2D = ha.GetHomMat2D(SourcePoint, TargetPoint);
                            hHomMat2DAdd = ha.GetHomMat2D(new userWcsVector(), addVector);
                            homMat2DCompose = hHomMat2D.HomMat2dCompose(hHomMat2DAdd);
                            AddXYTheta = ha.GetHomMat2DXYTheta(homMat2DCompose);
                            break;
                    }
                    if (homMat2DCompose != null)
                        affinePoints = ha.AffineTransPoint2d(homMat2DCompose, SourcePoint);
                    result = true;
                    break;
                case enAlignmentMethod.N点对齐:
                    switch (Param.RefObject)
                    {
                        case enRefObject.视野中心:
                            double X, Y, Z;
                            List<userWcsPoint> listWcsPoint = new List<userWcsPoint>();
                            foreach (var item in SourcePoint)
                            {
                                item.CamParams.ImageCenterPointsToWorldPlane(item.Grab_x, item.Grab_y, item.Grab_z, out X, out Y, out Z);
                                listWcsPoint.Add(new userWcsPoint(X, Y, Z));
                            }
                            /////////////////////////////////////////////////////////
                            hHomMat2D = ha.GetHomMat2D(SourcePoint, listWcsPoint.ToArray()); // 计算源点到目标点间的变换，
                            hHomMat2DAdd = ha.GetHomMat2D(new userWcsVector(), addVector); // 给源点到目标点间的变换添加补偿值
                            homMat2DCompose = hHomMat2D.HomMat2dCompose(hHomMat2DAdd);
                            AddXYTheta = ha.GetHomMat2DXYTheta(homMat2DCompose);
                            break;
                        case enRefObject.示教点:
                        case enRefObject.目标点:
                            if (TargetPoint.Length != SourcePoint.Length)
                                throw new ArgumentException("TargetPoint 的长度与 SourcePoint 的长度不相等 ");
                            hHomMat2D = ha.GetHomMat2D(SourcePoint, TargetPoint); // 计算源点到目标点间的变换，
                            hHomMat2DAdd = ha.GetHomMat2D(new userWcsVector(), addVector); // 给源点到目标点间的变换添加补偿值
                            homMat2DCompose = hHomMat2D.HomMat2dCompose(hHomMat2DAdd);
                            AddXYTheta = ha.GetHomMat2DXYTheta(homMat2DCompose);
                            break;
                    }
                    if (homMat2DCompose != null)
                        affinePoints = ha.AffineTransPoint2d(homMat2DCompose, SourcePoint);
                    result = true;
                    break;
                default:
                    throw new ArgumentException("小于所需的对齐点");
            }
            return result;
        }

        public static bool CalculateAlign2(userWcsPoint[] TargetPoint, userWcsPoint[] SourcePoint, CompensationParam Param, out userWcsPoint[] affinePoints, out userWcsVector AddXYTheta)
        {
            bool result = false;
            HalconLibrary ha = new HalconLibrary();
            userWcsVector refVectorPoint = new userWcsVector();
            userWcsVector currentVectorPoint = new userWcsVector();
            userWcsVector addVector = new userWcsVector();
            HalconDotNet.HHomMat2D hHomMat2D = null;
            AddXYTheta = new userWcsVector();
            affinePoints = new userWcsPoint[0];
            if (SourcePoint == null)
                throw new ArgumentNullException(" SourcePoint ");
            if (Param == null)
                throw new ArgumentNullException(" Param ");
            ///////////////////////////////////////////
            addVector.X = Param.Add_X;
            addVector.Y = Param.Add_Y;
            addVector.Angle = Param.Add_Angle;
            switch (Param.AlignmentMethod)
            {
                case enAlignmentMethod.单点对齐:
                    affinePoints = new userWcsPoint[1];
                    switch (Param.RefObject)
                    {
                        default:
                        case enRefObject.视野中心:
                            if (SourcePoint.Length < 1)
                                throw new ArgumentException("CurPoint 的长度小于1 ");
                            double X, Y, Z;
                            SourcePoint[0].CamParams.ImageCenterPointsToWorldPlane(SourcePoint[0].Grab_x, SourcePoint[0].Grab_y, SourcePoint[0].Grab_z, out X, out Y, out Z);
                            AddXYTheta.X = (X - SourcePoint[0].X) + Param.Add_X;
                            AddXYTheta.Y = (Y - SourcePoint[0].Y) + Param.Add_Y;
                            AddXYTheta.Angle = 0;
                            break;
                        case enRefObject.示教点:
                        case enRefObject.目标点:
                            if (TargetPoint.Length < 1)
                                throw new ArgumentException("RefPoint 的长度小于1 ");
                            if (SourcePoint.Length < 1)
                                throw new ArgumentException("CurPoint 的长度小于1 ");
                            AddXYTheta.X = (TargetPoint[0].X - SourcePoint[0].X) + Param.Add_X;
                            AddXYTheta.Y = (TargetPoint[0].Y - SourcePoint[0].Y) + Param.Add_Y;
                            AddXYTheta.Angle = 0;
                            break;
                    }
                    affinePoints[0].X = SourcePoint[0].X + AddXYTheta.X;
                    affinePoints[0].Y = SourcePoint[0].Y + AddXYTheta.Y;
                    result = true;
                    break;
                case enAlignmentMethod.两点对齐_起点角度:
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
                            refVectorPoint.X = X1;
                            refVectorPoint.Y = Y1;
                            refVectorPoint.Angle = Angle;  //这里需要将补偿值转换成弧度 + Param.Add_Theta * Math.PI / 180
                            currentVectorPoint.X = (SourcePoint[0].X);
                            currentVectorPoint.Y = (SourcePoint[0].Y);
                            currentVectorPoint.Angle = Math.Atan2(SourcePoint[1].Y - SourcePoint[0].Y, SourcePoint[1].X - SourcePoint[0].X);
                            //////////////////////////////////////////////////
                            hHomMat2D = ha.GetHomMat2D(currentVectorPoint, refVectorPoint);
                            AddXYTheta = ha.GetHomMat2DXYTheta(hHomMat2D) + addVector;
                            break;
                        case enRefObject.示教点:
                        case enRefObject.目标点:
                            if (TargetPoint.Length < 2)
                                throw new ArgumentException("TargetPoint 的长度小于2 ");
                            if (SourcePoint.Length < 2)
                                throw new ArgumentException("SourcePoint 的长度小于2 ");
                            refVectorPoint.X = TargetPoint[0].X;
                            refVectorPoint.Y = TargetPoint[0].Y;
                            refVectorPoint.Angle = Math.Atan2(TargetPoint[1].Y - TargetPoint[0].Y, TargetPoint[1].X - TargetPoint[0].X);  //这里需要将补偿值转换成弧度
                            currentVectorPoint.X = (SourcePoint[0].X);
                            currentVectorPoint.Y = (SourcePoint[0].Y);
                            currentVectorPoint.Angle = Math.Atan2(SourcePoint[1].Y - SourcePoint[0].Y, SourcePoint[1].X - SourcePoint[0].X);
                            //////////////////////////////////////////////////
                            hHomMat2D = ha.GetHomMat2D(currentVectorPoint, refVectorPoint);
                            AddXYTheta = ha.GetHomMat2DXYTheta(hHomMat2D) + addVector;
                            break;
                    }
                    if (AddXYTheta != null)
                    {
                        HTuple Qx, Qy;
                        Qx = AddXYTheta.GetHomMat2D().AffineTransPoint2d(currentVectorPoint.X, currentVectorPoint.Y, out Qy);
                        affinePoints = new userWcsPoint[1];
                        affinePoints[0].X = Qx.D;
                        affinePoints[0].Y = Qy.D;
                        affinePoints[0].Z = 0;
                    }
                    result = true;
                    break;
                case enAlignmentMethod.两点对齐:
                    switch (Param.RefObject)
                    {
                        case enRefObject.视野中心:
                            if (SourcePoint.Length < 2)
                                throw new ArgumentException("SourcePoint 的长度小于2 ");
                            double X1, Y1, Z1, X2, Y2, Z2;
                            SourcePoint[0].CamParams.ImageCenterPointsToWorldPlane(SourcePoint[0].Grab_x, SourcePoint[0].Grab_y, SourcePoint[0].Grab_z, out X1, out Y1, out Z1);
                            SourcePoint[1].CamParams.ImageCenterPointsToWorldPlane(SourcePoint[1].Grab_x, SourcePoint[1].Grab_y, SourcePoint[1].Grab_z, out X2, out Y2, out Z2);
                            userWcsPoint[] wcsPoints = new userWcsPoint[2];
                            wcsPoints[0].X = X1;
                            wcsPoints[0].Y = Y1;
                            wcsPoints[1].X = X2;
                            wcsPoints[1].Y = Y2;
                            /////////////////////////////////////////////////////////
                            hHomMat2D = ha.GetHomMat2D(SourcePoint, wcsPoints);
                            //hHomMat2DAdd = ha.GetHomMat2D(new userWcsVector(), addVector);
                            //homMat2DCompose = hHomMat2D.HomMat2dCompose(hHomMat2DAdd);
                            AddXYTheta = ha.GetHomMat2DXYTheta(hHomMat2D) + addVector;
                            break;
                        case enRefObject.示教点:
                        case enRefObject.目标点:
                            if (TargetPoint.Length < 2)
                                throw new ArgumentException("TargetPoint 的长度小于2 ");
                            if (SourcePoint.Length < 2)
                                throw new ArgumentException("SourcePoint 的长度小于2 ");
                            hHomMat2D = ha.GetHomMat2D(SourcePoint, TargetPoint);
                            //hHomMat2DAdd = ha.GetHomMat2D(new userWcsVector(), addVector);
                            //homMat2DCompose = hHomMat2D.HomMat2dCompose(hHomMat2DAdd);
                            AddXYTheta = ha.GetHomMat2DXYTheta(hHomMat2D) + addVector;
                            break;
                    }
                    if (AddXYTheta != null)
                        affinePoints = ha.AffineTransPoint2d(AddXYTheta.GetHomMat2D(), SourcePoint);
                    result = true;
                    break;
                case enAlignmentMethod.三点对齐:
                    switch (Param.RefObject)
                    {
                        case enRefObject.视野中心:
                            if (SourcePoint.Length < 3)
                                throw new ArgumentException("SourcePoint 的长度小于3 ");
                            double X1, Y1, Z1, X2, Y2, Z2, X3, Y3, Z3;
                            SourcePoint[0].CamParams.ImageCenterPointsToWorldPlane(SourcePoint[0].Grab_x, SourcePoint[0].Grab_y, SourcePoint[0].Grab_z, out X1, out Y1, out Z1);
                            SourcePoint[1].CamParams.ImageCenterPointsToWorldPlane(SourcePoint[1].Grab_x, SourcePoint[1].Grab_y, SourcePoint[1].Grab_z, out X2, out Y2, out Z2);
                            SourcePoint[2].CamParams.ImageCenterPointsToWorldPlane(SourcePoint[2].Grab_x, SourcePoint[2].Grab_y, SourcePoint[2].Grab_z, out X3, out Y3, out Z3);
                            userWcsPoint[] wcsPoints = new userWcsPoint[3];
                            wcsPoints[0].X = X1;
                            wcsPoints[0].Y = Y1;
                            wcsPoints[1].X = X2;
                            wcsPoints[1].Y = Y2;
                            wcsPoints[2].X = X3;
                            wcsPoints[2].Y = Y3;
                            /////////////////////////////////////////////////////////
                            hHomMat2D = ha.GetHomMat2D(SourcePoint, wcsPoints);
                            AddXYTheta = ha.GetHomMat2DXYTheta(hHomMat2D) + addVector;
                            break;
                        case enRefObject.示教点:
                        case enRefObject.目标点:
                            if (TargetPoint.Length < 3)
                                throw new ArgumentException("TargetPoint 的长度小于3 ");
                            if (SourcePoint.Length < 3)
                                throw new ArgumentException("SourcePoint 的长度小于3 ");
                            hHomMat2D = ha.GetHomMat2D(SourcePoint, TargetPoint);
                            AddXYTheta = ha.GetHomMat2DXYTheta(hHomMat2D) + addVector;
                            break;
                    }
                    if (AddXYTheta != null)
                        affinePoints = ha.AffineTransPoint2d(AddXYTheta.GetHomMat2D(), SourcePoint);
                    result = true;
                    break;

                case enAlignmentMethod.四点对齐:
                    switch (Param.RefObject)
                    {
                        case enRefObject.视野中心:
                            if (SourcePoint.Length < 4)
                                throw new ArgumentException("SourcePoint 的长度小于4 ");
                            double X1, Y1, Z1, X2, Y2, Z2, X3, Y3, Z3, X4, Y4, Z4;
                            SourcePoint[0].CamParams.ImageCenterPointsToWorldPlane(SourcePoint[0].Grab_x, SourcePoint[0].Grab_y, SourcePoint[0].Grab_z, out X1, out Y1, out Z1);
                            SourcePoint[1].CamParams.ImageCenterPointsToWorldPlane(SourcePoint[1].Grab_x, SourcePoint[1].Grab_y, SourcePoint[1].Grab_z, out X2, out Y2, out Z2);
                            SourcePoint[2].CamParams.ImageCenterPointsToWorldPlane(SourcePoint[2].Grab_x, SourcePoint[2].Grab_y, SourcePoint[2].Grab_z, out X3, out Y3, out Z3);
                            SourcePoint[3].CamParams.ImageCenterPointsToWorldPlane(SourcePoint[3].Grab_x, SourcePoint[3].Grab_y, SourcePoint[3].Grab_z, out X4, out Y4, out Z4);
                            userWcsPoint[] wcsPoints = new userWcsPoint[4]; // 目标点
                            wcsPoints[0].X = X1;
                            wcsPoints[0].Y = Y1;
                            wcsPoints[1].X = X2;
                            wcsPoints[1].Y = Y2;
                            wcsPoints[2].X = X3;
                            wcsPoints[2].Y = Y3;
                            wcsPoints[3].X = X4;
                            wcsPoints[3].Y = Y4;
                            /////////////////////////////////////////////////////////
                            hHomMat2D = ha.GetHomMat2D(SourcePoint, wcsPoints);
                            AddXYTheta = ha.GetHomMat2DXYTheta(hHomMat2D) + addVector;
                            break;
                        case enRefObject.示教点:
                        case enRefObject.目标点:
                            ///////////////////////////////////////////////
                            if (TargetPoint.Length < 4)
                                throw new ArgumentException("TargetPoint 的长度小于4 ");
                            if (SourcePoint.Length < 4)
                                throw new ArgumentException("SourcePoint 的长度小于4 ");
                            hHomMat2D = ha.GetHomMat2D(SourcePoint, TargetPoint);
                            AddXYTheta = ha.GetHomMat2DXYTheta(hHomMat2D) + addVector;
                            break;
                    }
                    if (AddXYTheta != null)
                        affinePoints = ha.AffineTransPoint2d(AddXYTheta.GetHomMat2D(), SourcePoint);
                    result = true;
                    break;
                case enAlignmentMethod.N点对齐:
                    switch (Param.RefObject)
                    {
                        case enRefObject.视野中心:
                            double X, Y, Z;
                            List<userWcsPoint> listWcsPoint = new List<userWcsPoint>();
                            foreach (var item in SourcePoint)
                            {
                                item.CamParams.ImageCenterPointsToWorldPlane(item.Grab_x, item.Grab_y, item.Grab_z, out X, out Y, out Z);
                                listWcsPoint.Add(new userWcsPoint(X, Y, Z));
                            }
                            /////////////////////////////////////////////////////////
                            hHomMat2D = ha.GetHomMat2D(SourcePoint, listWcsPoint.ToArray()); // 计算源点到目标点间的变换，
                            AddXYTheta = ha.GetHomMat2DXYTheta(hHomMat2D) + addVector;
                            break;
                        case enRefObject.示教点:
                        case enRefObject.目标点:
                            if (TargetPoint.Length != SourcePoint.Length)
                                throw new ArgumentException("TargetPoint 的长度与 SourcePoint 的长度不相等 ");
                            hHomMat2D = ha.GetHomMat2D(SourcePoint, TargetPoint); // 计算源点到目标点间的变换，
                            AddXYTheta = ha.GetHomMat2DXYTheta(hHomMat2D) + addVector;
                            break;
                    }
                    if (AddXYTheta != null)
                        affinePoints = ha.AffineTransPoint2d(AddXYTheta.GetHomMat2D(), SourcePoint);
                    result = true;
                    break;
                default:
                    throw new ArgumentException("小于所需的对齐点");
            }
            return result;
        }

        public static bool CalculateAlign2(userWcsVector[] TargetPoint, userWcsVector[] SourcePoint, CompensationParam Param, out userWcsVector[] affinePoints, out userWcsVector AddXYTheta)
        {
            bool result = false;
            HalconLibrary ha = new HalconLibrary();
            userWcsVector refVectorPoint = new userWcsVector();
            userWcsVector currentVectorPoint = new userWcsVector();
            userWcsVector addVector = new userWcsVector();
            HalconDotNet.HHomMat2D hHomMat2D = null, hHomMat2DAdd = null, homMat2DCompose = null;
            AddXYTheta = new userWcsVector();
            affinePoints = new userWcsVector[0];
            if (SourcePoint == null)
                throw new ArgumentNullException(" SourcePoint ");
            if (Param == null)
                throw new ArgumentNullException(" Param ");
            ///////////////////////////////////////////
            addVector.X = Param.Add_X;
            addVector.Y = Param.Add_Y;
            addVector.Angle = Param.Add_Angle;
            switch (Param.AlignmentMethod)
            {
                case enAlignmentMethod.单点对齐:
                    affinePoints = new userWcsVector[1];
                    switch (Param.RefObject)
                    {
                        default:
                        case enRefObject.视野中心:
                            if (SourcePoint.Length < 1)
                                throw new ArgumentException("CurPoint 的长度小于1 ");
                            double X, Y, Z;
                            SourcePoint[0].CamParams.ImageCenterPointsToWorldPlane(SourcePoint[0].Grab_x, SourcePoint[0].Grab_y, SourcePoint[0].Grab_z, out X, out Y, out Z);
                            AddXYTheta.X = (X - SourcePoint[0].X) + Param.Add_X;
                            AddXYTheta.Y = (Y - SourcePoint[0].Y) + Param.Add_Y;
                            AddXYTheta.Angle = 0;
                            break;
                        case enRefObject.示教点:
                        case enRefObject.目标点:
                            if (TargetPoint.Length < 1)
                                throw new ArgumentException("RefPoint 的长度小于1 ");
                            if (SourcePoint.Length < 1)
                                throw new ArgumentException("CurPoint 的长度小于1 ");
                            AddXYTheta.X = (TargetPoint[0].X - SourcePoint[0].X) + Param.Add_X;
                            AddXYTheta.Y = (TargetPoint[0].Y - SourcePoint[0].Y) + Param.Add_Y;
                            AddXYTheta.Angle = 0;
                            break;
                    }
                    affinePoints[0].X = SourcePoint[0].X + AddXYTheta.X;
                    affinePoints[0].Y = SourcePoint[0].Y + AddXYTheta.Y;
                    result = true;
                    break;
                case enAlignmentMethod.两点对齐_起点角度:
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
                            refVectorPoint.X = X1;
                            refVectorPoint.Y = Y1;
                            refVectorPoint.Angle = Angle;  //这里需要将补偿值转换成弧度 + Param.Add_Theta * Math.PI / 180
                            currentVectorPoint.X = (SourcePoint[0].X);
                            currentVectorPoint.Y = (SourcePoint[0].Y);
                            currentVectorPoint.Angle = Math.Atan2(SourcePoint[1].Y - SourcePoint[0].Y, SourcePoint[1].X - SourcePoint[0].X);
                            //////////////////////////////////////////////////
                            hHomMat2D = ha.GetHomMat2D(currentVectorPoint, refVectorPoint);
                            AddXYTheta = ha.GetHomMat2DXYTheta(hHomMat2D) + addVector;
                            break;
                        case enRefObject.示教点:
                        case enRefObject.目标点:
                            if (TargetPoint.Length < 2)
                                throw new ArgumentException("TargetPoint 的长度小于2 ");
                            if (SourcePoint.Length < 2)
                                throw new ArgumentException("SourcePoint 的长度小于2 ");
                            refVectorPoint.X = TargetPoint[0].X;
                            refVectorPoint.Y = TargetPoint[0].Y;
                            refVectorPoint.Angle = Math.Atan2(TargetPoint[1].Y - TargetPoint[0].Y, TargetPoint[1].X - TargetPoint[0].X);  //这里需要将补偿值转换成弧度
                            currentVectorPoint.X = (SourcePoint[0].X);
                            currentVectorPoint.Y = (SourcePoint[0].Y);
                            currentVectorPoint.Angle = Math.Atan2(SourcePoint[1].Y - SourcePoint[0].Y, SourcePoint[1].X - SourcePoint[0].X);
                            //////////////////////////////////////////////////
                            hHomMat2D = ha.GetHomMat2D(currentVectorPoint, refVectorPoint);
                            AddXYTheta = ha.GetHomMat2DXYTheta(hHomMat2D) + addVector;
                            break;
                    }
                    if (AddXYTheta != null)
                    {
                        HTuple Qx, Qy;
                        Qx = AddXYTheta.GetHomMat2D().AffineTransPoint2d(currentVectorPoint.X, currentVectorPoint.Y, out Qy);
                        affinePoints = new userWcsVector[1];
                        affinePoints[0].X = Qx.D;
                        affinePoints[0].Y = Qy.D;
                        affinePoints[0].Z = 0;
                    }
                    result = true;
                    break;
                case enAlignmentMethod.两点对齐:
                    switch (Param.RefObject)
                    {
                        case enRefObject.视野中心:
                            if (SourcePoint.Length < 2)
                                throw new ArgumentException("SourcePoint 的长度小于2 ");
                            double X1, Y1, Z1, X2, Y2, Z2;
                            SourcePoint[0].CamParams.ImageCenterPointsToWorldPlane(SourcePoint[0].Grab_x, SourcePoint[0].Grab_y, SourcePoint[0].Grab_z, out X1, out Y1, out Z1);
                            SourcePoint[1].CamParams.ImageCenterPointsToWorldPlane(SourcePoint[1].Grab_x, SourcePoint[1].Grab_y, SourcePoint[1].Grab_z, out X2, out Y2, out Z2);
                            userWcsVector[] wcsPoints = new userWcsVector[2];
                            wcsPoints[0].X = X1;
                            wcsPoints[0].Y = Y1;
                            wcsPoints[1].X = X2;
                            wcsPoints[1].Y = Y2;
                            /////////////////////////////////////////////////////////
                            hHomMat2D = ha.GetHomMat2D(SourcePoint, wcsPoints);
                            AddXYTheta = ha.GetHomMat2DXYTheta(hHomMat2D) + addVector;
                            break;
                        case enRefObject.示教点:
                        case enRefObject.目标点:
                            if (TargetPoint.Length < 2)
                                throw new ArgumentException("TargetPoint 的长度小于2 ");
                            if (SourcePoint.Length < 2)
                                throw new ArgumentException("SourcePoint 的长度小于2 ");
                            hHomMat2D = ha.GetHomMat2D(SourcePoint, TargetPoint);
                            AddXYTheta = ha.GetHomMat2DXYTheta(hHomMat2D) + addVector;
                            break;
                    }
                    if (AddXYTheta != null)
                        affinePoints = ha.AffineTransPoint2d(AddXYTheta.GetHomMat2D(), SourcePoint);
                    result = true;
                    break;

                case enAlignmentMethod.向量对齐:
                    if (TargetPoint.Length < 1)
                        throw new ArgumentException("TargetPoint 的长度小于1 ");
                    if (SourcePoint.Length < 1)
                        throw new ArgumentException("SourcePoint 的长度小于1 ");
                    switch (Param.RefObject)
                    {
                        case enRefObject.视野中心:
                            if (SourcePoint.Length < 1)
                                throw new ArgumentException("SourcePoint 的长度小于1 ");
                            double X1, Y1, Z1;
                            SourcePoint[0].CamParams.ImageCenterPointsToWorldPlane(SourcePoint[0].Grab_x, SourcePoint[0].Grab_y, SourcePoint[0].Grab_z, out X1, out Y1, out Z1);
                            userWcsVector wcsPoints = new userWcsVector(X1, Y1,0) + addVector;
                            /////////////////////////////////////////////////////////
                            hHomMat2D = ha.GetHomMat2D(SourcePoint[0], wcsPoints);
                            AddXYTheta = ha.GetHomMat2DXYTheta(hHomMat2D) ;
                            break;
                        case enRefObject.示教点:
                        case enRefObject.目标点:
                            HHomMat2D hHomMat2DCompose = new HHomMat2D();
                            AddXYTheta = new userWcsVector();
                            userWcsVector targetVector = TargetPoint[0];
                            userWcsVector sourceVector = SourcePoint[0];
                            //////////////////////
                            userWcsVector wcsVector = targetVector + new userWcsVector(Param.Add_X, Param.Add_Y, 0, Param.Add_Angle); // 目标向量加上补偿值
                            hHomMat2DCompose.VectorAngleToRigid(sourceVector.X, sourceVector.Y, sourceVector.Angle, wcsVector.X, wcsVector.Y, wcsVector.Angle);
                            ////////////////////////////////////
                            double Sx, Sy, Phi, Theta, Tx, Ty;
                            Sx = hHomMat2DCompose.HomMat2dToAffinePar(out Sy, out Phi, out Theta, out Tx, out Ty);
                            AddXYTheta.X = Tx;
                            AddXYTheta.Y = Ty;
                            AddXYTheta.Angle = Phi * 180 / Math.PI;
                            break;
                    }
                    /////////////////////////////////////////
                    if (AddXYTheta != null)
                        affinePoints = ha.AffineTransPoint2d(AddXYTheta.GetHomMat2D(), SourcePoint);
                    result = true;
                    break;

                case enAlignmentMethod.三点对齐:
                    switch (Param.RefObject)
                    {
                        case enRefObject.视野中心:
                            if (SourcePoint.Length < 3)
                                throw new ArgumentException("SourcePoint 的长度小于3 ");
                            double X1, Y1, Z1, X2, Y2, Z2, X3, Y3, Z3;
                            SourcePoint[0].CamParams.ImageCenterPointsToWorldPlane(SourcePoint[0].Grab_x, SourcePoint[0].Grab_y, SourcePoint[0].Grab_z, out X1, out Y1, out Z1);
                            SourcePoint[1].CamParams.ImageCenterPointsToWorldPlane(SourcePoint[1].Grab_x, SourcePoint[1].Grab_y, SourcePoint[1].Grab_z, out X2, out Y2, out Z2);
                            SourcePoint[2].CamParams.ImageCenterPointsToWorldPlane(SourcePoint[2].Grab_x, SourcePoint[2].Grab_y, SourcePoint[2].Grab_z, out X3, out Y3, out Z3);
                            userWcsVector[] wcsPoints = new userWcsVector[3];
                            wcsPoints[0].X = X1;
                            wcsPoints[0].Y = Y1;
                            wcsPoints[1].X = X2;
                            wcsPoints[1].Y = Y2;
                            wcsPoints[2].X = X3;
                            wcsPoints[2].Y = Y3;
                            /////////////////////////////////////////////////////////
                            hHomMat2D = ha.GetHomMat2D(SourcePoint, wcsPoints);
                            AddXYTheta = ha.GetHomMat2DXYTheta(hHomMat2D) + addVector;
                            break;
                        case enRefObject.示教点:
                        case enRefObject.目标点:
                            if (TargetPoint.Length < 3)
                                throw new ArgumentException("TargetPoint 的长度小于3 ");
                            if (SourcePoint.Length < 3)
                                throw new ArgumentException("SourcePoint 的长度小于3 ");
                            hHomMat2D = ha.GetHomMat2D(SourcePoint, TargetPoint);
                            AddXYTheta = ha.GetHomMat2DXYTheta(hHomMat2D) + addVector; // 补偿值一定要单独加上去，他也是相对于坐标原点来变化的，不能以位姿合成的方式
                            break;
                    }
                    if (AddXYTheta != null)
                        affinePoints = ha.AffineTransPoint2d(AddXYTheta.GetHomMat2D(), SourcePoint);
                    result = true;
                    break;

                case enAlignmentMethod.四点对齐:
                    switch (Param.RefObject)
                    {
                        case enRefObject.视野中心:
                            if (SourcePoint.Length < 4)
                                throw new ArgumentException("SourcePoint 的长度小于4 ");
                            double X1, Y1, Z1, X2, Y2, Z2, X3, Y3, Z3, X4, Y4, Z4;
                            SourcePoint[0].CamParams.ImageCenterPointsToWorldPlane(SourcePoint[0].Grab_x, SourcePoint[0].Grab_y, SourcePoint[0].Grab_z, out X1, out Y1, out Z1);
                            SourcePoint[1].CamParams.ImageCenterPointsToWorldPlane(SourcePoint[1].Grab_x, SourcePoint[1].Grab_y, SourcePoint[1].Grab_z, out X2, out Y2, out Z2);
                            SourcePoint[2].CamParams.ImageCenterPointsToWorldPlane(SourcePoint[2].Grab_x, SourcePoint[2].Grab_y, SourcePoint[2].Grab_z, out X3, out Y3, out Z3);
                            SourcePoint[3].CamParams.ImageCenterPointsToWorldPlane(SourcePoint[3].Grab_x, SourcePoint[3].Grab_y, SourcePoint[3].Grab_z, out X4, out Y4, out Z4);
                            userWcsVector[] wcsPoints = new userWcsVector[4]; // 目标点
                            wcsPoints[0].X = X1;
                            wcsPoints[0].Y = Y1;
                            wcsPoints[1].X = X2;
                            wcsPoints[1].Y = Y2;
                            wcsPoints[2].X = X3;
                            wcsPoints[2].Y = Y3;
                            wcsPoints[3].X = X4;
                            wcsPoints[3].Y = Y4;
                            /////////////////////////////////////////////////////////
                            hHomMat2D = ha.GetHomMat2D(SourcePoint, wcsPoints);
                            AddXYTheta = ha.GetHomMat2DXYTheta(hHomMat2D) + addVector;
                            break;
                        case enRefObject.示教点:
                        case enRefObject.目标点:
                            ///////////////////////////////////////////////
                            if (TargetPoint.Length < 4)
                                throw new ArgumentException("TargetPoint 的长度小于4 ");
                            if (SourcePoint.Length < 4)
                                throw new ArgumentException("SourcePoint 的长度小于4 ");
                            hHomMat2D = ha.GetHomMat2D(SourcePoint, TargetPoint);
                            AddXYTheta = ha.GetHomMat2DXYTheta(hHomMat2D) + addVector;
                            break;
                    }
                    if (AddXYTheta != null)
                        affinePoints = ha.AffineTransPoint2d(AddXYTheta.GetHomMat2D(), SourcePoint);
                    result = true;
                    break;
                case enAlignmentMethod.N点对齐:
                    switch (Param.RefObject)
                    {
                        case enRefObject.视野中心:
                            double X, Y, Z;
                            List<userWcsVector> listWcsPoint = new List<userWcsVector>();
                            foreach (var item in SourcePoint)
                            {
                                item.CamParams.ImageCenterPointsToWorldPlane(item.Grab_x, item.Grab_y, item.Grab_z, out X, out Y, out Z);
                                listWcsPoint.Add(new userWcsVector(X, Y, Z));
                            }
                            /////////////////////////////////////////////////////////
                            hHomMat2D = ha.GetHomMat2D(SourcePoint, listWcsPoint.ToArray()); // 计算源点到目标点间的变换，
                            AddXYTheta = ha.GetHomMat2DXYTheta(hHomMat2D) + addVector;
                            break;
                        case enRefObject.示教点:
                        case enRefObject.目标点:
                            if (TargetPoint.Length != SourcePoint.Length)
                                throw new ArgumentException("TargetPoint 的长度与 SourcePoint 的长度不相等 ");
                            hHomMat2D = ha.GetHomMat2D(SourcePoint, TargetPoint); // 计算源点到目标点间的变换，
                            AddXYTheta = ha.GetHomMat2DXYTheta(hHomMat2D) + addVector;
                            break;
                    }
                    if (AddXYTheta != null)
                        affinePoints = ha.AffineTransPoint2d(AddXYTheta.GetHomMat2D(), SourcePoint);
                    result = true;
                    break;
                default:
                    throw new ArgumentException("小于所需的对齐点");
            }
            return result;
        }


        public static bool CalculateAlign(userWcsVector targetVector, userWcsVector sourceVector, CompensationParam param, out userWcsVector AddXYTheta)
        {
            if (targetVector == null)
                throw new ArgumentNullException(" targetVector ");
            if (sourceVector == null)
                throw new ArgumentNullException(" sourceVector ");
            HHomMat2D hHomMat2DTarget = new HHomMat2D();
            HHomMat2D hHomMat2DSource = new HHomMat2D();
            HHomMat2D hHomMat2DAdd = new HHomMat2D();
            HHomMat2D hHomMat2DCompose = new HHomMat2D();
            AddXYTheta = new userWcsVector();
            hHomMat2DTarget.VectorAngleToRigid(0.0,0,0, targetVector.X, targetVector.Y, targetVector.Angle * Math.PI / 180);
            hHomMat2DSource.VectorAngleToRigid(0.0, 0, 0, sourceVector.X, sourceVector.Y, sourceVector.Angle * Math.PI / 180);
            hHomMat2DAdd.VectorAngleToRigid(0.0, 0, 0, param.Add_X, param.Add_Y, param.Add_Angle * Math.PI / 180);
            hHomMat2DCompose = hHomMat2DTarget.HomMat2dCompose(hHomMat2DSource).HomMat2dCompose(hHomMat2DAdd);
            ////////////////////////////////////
            double Sx, Sy, Phi, Theta, Tx, Ty;
            Sx = hHomMat2DCompose.HomMat2dToAffinePar(out Sy, out Phi, out Theta, out Tx, out Ty);
            AddXYTheta.X = Tx;
            AddXYTheta.Y = Ty;
            AddXYTheta.Angle = Phi * 180 / Math.PI;
            return true;
        }
        public static bool CalculateAlign2(userWcsVector targetVector, userWcsVector sourceVector, CompensationParam param, out userWcsVector AddXYTheta)
        {
            if (targetVector == null)
                throw new ArgumentNullException(" targetVector ");
            if (sourceVector == null)
                throw new ArgumentNullException(" sourceVector ");
            HHomMat2D hHomMat2DCompose = new HHomMat2D();
            AddXYTheta = new userWcsVector();
            userWcsVector wcsVector = targetVector + new userWcsVector(param.Add_X, param.Add_Y,0, param.Add_Angle); // 目标向量加上补偿值
            hHomMat2DCompose.VectorAngleToRigid(sourceVector.X, sourceVector.Y, sourceVector.Angle, wcsVector.X, wcsVector.Y, wcsVector.Angle);
            ////////////////////////////////////
            double Sx, Sy, Phi, Theta, Tx, Ty;
            Sx = hHomMat2DCompose.HomMat2dToAffinePar(out Sy, out Phi, out Theta, out Tx, out Ty);
            AddXYTheta.X = Tx;
            AddXYTheta.Y = Ty;
            AddXYTheta.Angle = Phi * 180 / Math.PI;
            return true;
        }



    }
}

using AlgorithmsLibrary;
using Common;
using HalconDotNet;
using MotionControlCard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Media.Animation;

namespace FunctionBlock
{
    public class AlignMethod
    {
        /// <summary>
        /// 应用于单相机对位或双相机关联后的对位
        /// </summary>
        /// <param name="TargetPixVector"></param>
        /// <param name="SourcePixVector"></param>
        /// <param name="Param"></param>
        /// <param name="affinePoints"></param>
        /// <param name="AddXYTheta"></param>
        /// <returns></returns>
        public static bool CalculateAlign2(userPixVector[] TargetPixVector, userPixVector[] SourcePixVector, CompensationParam Param, out userWcsVector[] affinePoints, out userWcsVector AddXYTheta)
        {
            bool result = false;
            HalconLibrary ha = new HalconLibrary();
            userWcsVector refVectorPoint = new userWcsVector();
            userWcsVector currentVectorPoint = new userWcsVector();
            userWcsVector addVector = new userWcsVector();
            HalconDotNet.HHomMat2D hHomMat2D = null;
            AddXYTheta = new userWcsVector();
            affinePoints = new userWcsVector[0];
            if (SourcePixVector == null)
                throw new ArgumentNullException(" SourcePixVector ");
            if (Param == null)
                throw new ArgumentNullException(" Param ");
            ////////////////////////// 将像素坐标转换成世界坐标 ////////////////////////
            userWcsVector[] SourceWcsPoint = new userWcsVector[SourcePixVector.Length];
            for (int i = 0; i < SourcePixVector.Length; i++)
            {
                SourceWcsPoint[i] = SourcePixVector[i].GetWcsVector();
            }
            if (TargetPixVector == null) TargetPixVector = new userPixVector[0];
            userWcsVector[] TargetWcsPoint = new userWcsVector[TargetPixVector.Length];
            for (int i = 0; i < TargetPixVector.Length; i++)
            {
                switch (Param.RefObject)
                {
                    case enRefObject.视野中心:
                    case enRefObject.示教点: // 示教点常用的单相机的校正
                    default:
                        TargetWcsPoint[i] = TargetPixVector[i].GetWcsVector(SourcePixVector[i].Grab_x, SourcePixVector[i].Grab_y); //
                        break;
                    case enRefObject.目标点:
                    case enRefObject.映射相机: // 如果是坐标映射，因为是两个不同的相机，所以这里不需要使用源相机的当前拍照位置坐标来计算目标位置 
                        TargetWcsPoint[i] = TargetPixVector[i].GetWcsVector();
                        break;
                }
            }
            ///////////// 补偿值 ///////////////////////////////////////////////////////////////
            addVector.X = Param.Add_X;
            addVector.Y = Param.Add_Y;
            addVector.Angle = Param.Add_Angle;
            switch (Param.AlignmentMethod)
            {
                case enAlignmentMethod.单点对齐:
                    affinePoints = new userWcsVector[1];
                    if (TargetWcsPoint.Length < 1)
                        throw new ArgumentException("RefPoint 的长度小于1 ");
                    if (SourceWcsPoint.Length < 1)
                        throw new ArgumentException("CurPoint 的长度小于1 ");
                    AddXYTheta.X = (TargetWcsPoint[0].X - SourceWcsPoint[0].X) + addVector.X;
                    AddXYTheta.Y = (TargetWcsPoint[0].Y - SourceWcsPoint[0].Y) + addVector.Y;
                    AddXYTheta.Angle = 0;
                    //////////////////////////////////////////////////
                    affinePoints[0] = new userWcsVector();
                    affinePoints[0].X = SourceWcsPoint[0].X + AddXYTheta.X;
                    affinePoints[0].Y = SourceWcsPoint[0].Y + AddXYTheta.Y;
                    result = true;
                    break;
                case enAlignmentMethod.向量对齐:
                    if (TargetWcsPoint.Length < 1)
                        throw new ArgumentException("TargetPoint 的长度小于1 ");
                    if (SourceWcsPoint.Length < 1)
                        throw new ArgumentException("SourcePoint 的长度小于1 ");
                    refVectorPoint.X = TargetWcsPoint[0].X + addVector.X;
                    refVectorPoint.Y = TargetWcsPoint[0].Y + addVector.Y;
                    refVectorPoint.Angle = TargetWcsPoint[0].Angle + addVector.Angle;
                    currentVectorPoint.X = (SourceWcsPoint[0].X);
                    currentVectorPoint.Y = (SourceWcsPoint[0].Y);
                    currentVectorPoint.Angle = SourceWcsPoint[0].Angle;
                    hHomMat2D = ha.GetHomMat2D(currentVectorPoint, refVectorPoint);
                    AddXYTheta = ha.GetHomMat2DXYTheta(hHomMat2D);
                    //////////////////////////////////////////////////
                    if (AddXYTheta != null)
                    {
                        HTuple Qx, Qy;
                        Qx = AddXYTheta.GetHomMat2D().AffineTransPoint2d(currentVectorPoint.X, currentVectorPoint.Y, out Qy);
                        affinePoints = new userWcsVector[1];
                        affinePoints[0] = new userWcsVector();
                        affinePoints[0].X = Qx.D;
                        affinePoints[0].Y = Qy.D;
                        affinePoints[0].Z = 0;
                    }
                    result = true;
                    break;
                case enAlignmentMethod.向量对位_放料计算:
                    if (TargetWcsPoint.Length < 1)
                        throw new ArgumentException("TargetPoint 的长度小于1 ");
                    if (SourceWcsPoint.Length < 1)
                        throw new ArgumentException("SourcePoint 的长度小于1 ");
                    refVectorPoint.X = TargetWcsPoint[0].X + addVector.X;
                    refVectorPoint.Y = TargetWcsPoint[0].Y + addVector.Y;
                    refVectorPoint.Angle = TargetWcsPoint[0].Angle + addVector.Angle;
                    currentVectorPoint.X = (SourceWcsPoint[0].X);
                    currentVectorPoint.Y = (SourceWcsPoint[0].Y);
                    currentVectorPoint.Angle = SourceWcsPoint[0].Angle;
                    //////////////////////////////////////////////////
                    hHomMat2D = ha.GetHomMat2D(currentVectorPoint, refVectorPoint);
                    AddXYTheta = ha.GetHomMat2DXYTheta(hHomMat2D);
                    /////////////////////  ////////////////////////////////
                    CoordSysAxisPosParam axisParam = new CoordSysAxisPosParam();
                    axisParam.UpdataAxisPosition(SourceWcsPoint[0].CamParams.CaliParam.CoordSysName);
                    AddXYTheta.X += (SourceWcsPoint[0].Grab_x - axisParam.X);
                    AddXYTheta.Y += (SourceWcsPoint[0].Grab_y - axisParam.Y);
                    //////////////////////////////////////////
                    if (AddXYTheta != null)
                    {
                        HTuple Qx, Qy;
                        Qx = AddXYTheta.GetHomMat2D().AffineTransPoint2d(currentVectorPoint.X, currentVectorPoint.Y, out Qy);
                        affinePoints = new userWcsVector[1];
                        affinePoints[0] = new userWcsVector();
                        affinePoints[0].X = Qx.D;
                        affinePoints[0].Y = Qy.D;
                        affinePoints[0].Z = 0;
                    }
                    result = true;
                    break;
                case enAlignmentMethod.两点对齐_起点角度:
                    if (TargetWcsPoint.Length < 2)
                        throw new ArgumentException("TargetPoint 的长度小于2 ");
                    if (SourceWcsPoint.Length < 2)
                        throw new ArgumentException("SourcePoint 的长度小于2 ");
                    refVectorPoint.X = TargetWcsPoint[0].X + addVector.X;
                    refVectorPoint.Y = TargetWcsPoint[0].Y + addVector.Y;
                    refVectorPoint.Angle = Math.Atan2(TargetWcsPoint[1].Y - TargetWcsPoint[0].Y, TargetWcsPoint[1].X - TargetWcsPoint[0].X) * 180 / Math.PI + addVector.Angle;  //这里需要将补偿值转换成弧度
                    currentVectorPoint.X = (SourceWcsPoint[0].X);
                    currentVectorPoint.Y = (SourceWcsPoint[0].Y);
                    currentVectorPoint.Angle = Math.Atan2(SourceWcsPoint[1].Y - SourceWcsPoint[0].Y, SourceWcsPoint[1].X - SourceWcsPoint[0].X) * 180 / Math.PI;
                    hHomMat2D = ha.GetHomMat2D(currentVectorPoint, refVectorPoint);
                    AddXYTheta = ha.GetHomMat2DXYTheta(hHomMat2D);
                    //////////////////////////////////////////////////
                    if (AddXYTheta != null)
                    {
                        HTuple Qx, Qy;
                        Qx = AddXYTheta.GetHomMat2D().AffineTransPoint2d(currentVectorPoint.X, currentVectorPoint.Y, out Qy);
                        affinePoints = new userWcsVector[1];
                        affinePoints[0] = new userWcsVector();
                        affinePoints[0].X = Qx.D;
                        affinePoints[0].Y = Qy.D;
                        affinePoints[0].Z = 0;
                    }
                    result = true;
                    break;
                case enAlignmentMethod.两点对齐:
                    if (TargetWcsPoint.Length < 2)
                        throw new ArgumentException("TargetPoint 的长度小于2 ");
                    if (SourceWcsPoint.Length < 2)
                        throw new ArgumentException("SourcePoint 的长度小于2 ");
                    hHomMat2D = ha.GetHomMat2D(SourceWcsPoint, TargetWcsPoint, addVector);
                    AddXYTheta = ha.GetHomMat2DXYTheta(hHomMat2D);
                    //////////////////////////////////////////////
                    if (AddXYTheta != null)
                        affinePoints = ha.AffineTransPoint2d(AddXYTheta.GetHomMat2D(), SourceWcsPoint);
                    result = true;
                    break;
                case enAlignmentMethod.三点对齐:
                    if (TargetWcsPoint.Length < 3)
                        throw new ArgumentException("TargetPoint 的长度小于3 ");
                    if (SourceWcsPoint.Length < 3)
                        throw new ArgumentException("SourcePoint 的长度小于3 ");
                    hHomMat2D = ha.GetHomMat2D(SourceWcsPoint, TargetWcsPoint, addVector);
                    AddXYTheta = ha.GetHomMat2DXYTheta(hHomMat2D);
                    ///////////////////////////////
                    if (AddXYTheta != null)
                        affinePoints = ha.AffineTransPoint2d(AddXYTheta.GetHomMat2D(), SourceWcsPoint);
                    result = true;
                    break;

                case enAlignmentMethod.四点对齐:
                    if (TargetWcsPoint.Length < 4)
                        throw new ArgumentException("TargetPoint 的长度小于4 ");
                    if (SourceWcsPoint.Length < 4)
                        throw new ArgumentException("SourcePoint 的长度小于4 ");
                    hHomMat2D = ha.GetHomMat2D(SourceWcsPoint, TargetWcsPoint, addVector);
                    AddXYTheta = ha.GetHomMat2DXYTheta(hHomMat2D);
                    //////////////////////////////////////////////////////////
                    if (AddXYTheta != null)
                        affinePoints = ha.AffineTransPoint2d(AddXYTheta.GetHomMat2D(), SourceWcsPoint);
                    result = true;
                    break;
                case enAlignmentMethod.四点对齐_两点平移:
                    if (TargetWcsPoint.Length < 4)
                        throw new ArgumentException("TargetPoint 的长度小于4 ");
                    if (SourceWcsPoint.Length < 4)
                        throw new ArgumentException("SourcePoint 的长度小于4 ");
                    hHomMat2D = ha.GetHomMat2D(SourceWcsPoint, TargetWcsPoint, addVector);
                    AddXYTheta = ha.GetHomMat2DXYTheta(hHomMat2D);
                    affinePoints = ha.AffineTransPoint2d(AddXYTheta.GetHomMat2D(), SourceWcsPoint);
                    double Tx = (TargetWcsPoint[0].X + TargetWcsPoint[1].X) * 0.5 - (affinePoints[0].X + affinePoints[1].X) * 0.5;
                    double Ty = (TargetWcsPoint[0].Y + TargetWcsPoint[1].Y) * 0.5 - (affinePoints[0].Y + affinePoints[1].Y) * 0.5;
                    userWcsVector userVector = new userWcsVector(Tx, Ty);
                    AddXYTheta = ha.GetHomMat2DXYTheta(hHomMat2D) + userVector;
                    ///////////////////////////////////////////////
                    if (AddXYTheta != null)
                        affinePoints = ha.AffineTransPoint2d(AddXYTheta.GetHomMat2D(), SourceWcsPoint);
                    result = true;
                    break;
                case enAlignmentMethod.四点对齐_Rigid:
                    if (TargetWcsPoint.Length < 4)
                        throw new ArgumentException("TargetPoint 的长度小于4 ");
                    if (SourceWcsPoint.Length < 4)
                        throw new ArgumentException("SourcePoint 的长度小于4 ");
                    hHomMat2D = ha.GetRigidHomMat2D(SourceWcsPoint, TargetWcsPoint, addVector);
                    AddXYTheta = ha.GetHomMat2DXYTheta(hHomMat2D);
                    ///////////////////////////////////////////////
                    if (AddXYTheta != null)
                        affinePoints = ha.AffineTransPoint2d(AddXYTheta.GetHomMat2D(), SourceWcsPoint);
                    result = true;
                    break;
                case enAlignmentMethod.N点对齐:
                    if (TargetWcsPoint.Length != SourceWcsPoint.Length)
                        throw new ArgumentException("TargetPoint 的长度与 SourcePoint 的长度不相等 ");
                    hHomMat2D = ha.GetHomMat2D(SourceWcsPoint, TargetWcsPoint, addVector); // 计算源点到目标点间的变换，
                    AddXYTheta = ha.GetHomMat2DXYTheta(hHomMat2D);
                    ///////////////////////////////////////////////////////////
                    if (AddXYTheta != null)
                        affinePoints = ha.AffineTransPoint2d(AddXYTheta.GetHomMat2D(), SourceWcsPoint);
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
            hHomMat2DTarget.VectorAngleToRigid(0.0, 0, 0, targetVector.X, targetVector.Y, targetVector.Angle * Math.PI / 180);
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

        public static bool CalculateAlign(userPixPoint[] plateTeachPixPoint, userPixPoint[] plateCurPixPoint, userPixPoint[] bandTeachPixPoint, userPixPoint[] bandCurPixPoint, CompensationParam param, out userWcsVector AddXYTheta)
        {
            if (plateTeachPixPoint == null || plateTeachPixPoint.Length < 2)
                throw new ArgumentNullException(" plateTeachPointPix 为空或长度小于2 ");
            if (plateCurPixPoint == null || plateCurPixPoint.Length < 2)
                throw new ArgumentNullException(" plateCurPointPix 为空或长度小于2 ");
            if (bandTeachPixPoint == null || bandTeachPixPoint.Length < 2)
                throw new ArgumentNullException(" bandTeachPointPix 为空或长度小于2 ");
            if (bandCurPixPoint == null || bandCurPixPoint.Length < 2)
                throw new ArgumentNullException(" bandCurPointPix 为空或长度小于2 ");
            if (plateTeachPixPoint.Length != plateCurPixPoint.Length || plateTeachPixPoint.Length != bandTeachPixPoint.Length ||
                plateTeachPixPoint.Length != bandCurPixPoint.Length)
                throw new ArgumentNullException(" 元素长度不相等 ");
            ////////////////////////////////////////////////////////////////////////
            HalconLibrary ha = new HalconLibrary();
            HalconDotNet.HHomMat2D hHomMat2DPlate = null, hHomMat2DBand = null;
            /////// 将像素坐标转换为世界坐标,平台数据转换 ///////////////////////////////////////
            userWcsPoint[] plateCurPoint = new userWcsPoint[plateCurPixPoint.Length];
            for (int i = 0; i < plateCurPixPoint.Length; i++)
            {
                plateCurPoint[i] = plateCurPixPoint[i].GetWcsPoint();
            }
            userWcsPoint[] plateTeachPoint = new userWcsPoint[plateTeachPixPoint.Length];
            for (int i = 0; i < plateTeachPixPoint.Length; i++)
            {
                plateTeachPoint[i] = plateTeachPixPoint[i].GetWcsPoint(plateCurPixPoint[i].Grab_x, plateCurPixPoint[i].Grab_y, 0);
            }
            //// 载带上的数据转换
            userWcsPoint[] bandCurPoint = new userWcsPoint[bandCurPixPoint.Length];
            for (int i = 0; i < bandCurPixPoint.Length; i++)
            {
                bandCurPoint[i] = bandCurPixPoint[i].GetWcsPoint();
            }
            userWcsPoint[] bandTeachPoint = new userWcsPoint[bandTeachPixPoint.Length];
            for (int i = 0; i < plateTeachPixPoint.Length; i++)
            {
                bandTeachPoint[i] = bandTeachPixPoint[i].GetWcsPoint(bandCurPixPoint[i].Grab_x, bandCurPixPoint[i].Grab_y, 0);
            }
            ////////////////////////// 用4点计算角度
            hHomMat2DPlate = ha.GetHomMat2D(plateTeachPoint, plateCurPoint, null); // 计算源点到目标点间的变换，
            hHomMat2DBand = ha.GetHomMat2D(bandTeachPoint, bandCurPoint, null);  // 计算源点到目标点间的变换，
            userWcsVector vectorPlate = ha.GetHomMat2DXYTheta(hHomMat2DPlate);
            userWcsVector vectorBand = ha.GetHomMat2DXYTheta(hHomMat2DBand); // 用4点来算角度
            ////// 计算中心点 
            List<double> plateTeach_x = new List<double>();
            List<double> plateTeach_y = new List<double>();
            List<double> plateCur_x = new List<double>();
            List<double> plateCur_y = new List<double>();
            List<double> bandTeach_x = new List<double>();
            List<double> bandTeach_y = new List<double>();
            List<double> bandCur_x = new List<double>();
            List<double> bandCur_y = new List<double>();
            foreach (var item in plateTeachPoint)
            {
                plateTeach_x.Add(item.X);
                plateTeach_y.Add(item.Y);
            }
            foreach (var item in plateCurPoint)
            {
                plateCur_x.Add(item.X);
                plateCur_y.Add(item.Y);
            }
            foreach (var item in bandTeachPoint)
            {
                bandTeach_x.Add(item.X);
                bandTeach_y.Add(item.Y);
            }
            foreach (var item in bandCurPoint)
            {
                bandCur_x.Add(item.X);
                bandCur_y.Add(item.Y);
            }
            ////////////////////////////////////////////  这种方式，将示教向量归一化为0值 /////////////////////////////////
            userWcsVector plateTeachVector = new userWcsVector(plateTeach_x.Average(), plateTeach_y.Average(), 0, 0);
            userWcsVector plateCurVector = new userWcsVector(plateCur_x.Average(), plateCur_y.Average(), 0, vectorPlate.Angle); // 这里使用的角度是变化角度，而不是实际角度
            userWcsVector bandTeachVector = new userWcsVector(bandTeach_x.Average(), bandTeach_y.Average(), 0, 0);
            userWcsVector bandCurVector = new userWcsVector(bandCur_x.Average(), bandCur_y.Average(), 0, vectorBand.Angle);
            ////// 这种方式，计算各自的向量，类似于向量对位，只不过角度使用多点来计算 ////////////
            //userWcsVector plateTeachVector = ha.GetVectorPoint(plateTeachPoint);
            //userWcsVector plateCurVector = ha.GetVectorPoint(plateCurPoint);
            //userWcsVector bandTeachVector = ha.GetVectorPoint(bandTeachPoint);
            //userWcsVector bandCurVector = ha.GetVectorPoint(bandCurPoint);
            ///////////////////////////////////
            userWcsVector subVectorBand = bandCurVector - bandTeachVector; // 贴头的偏差
            userWcsVector addVector = plateTeachVector + subVectorBand; // 平台的示教位置 + 贴头偏差 + 补偿值
            addVector.X += param.Add_X;
            addVector.Y += param.Add_Y;
            addVector.Angle += param.Add_Angle;
            ////////////////////////////////////////////
            HHomMat2D hHomMat2DAdd = new HHomMat2D();
            AddXYTheta = new userWcsVector();
            hHomMat2DAdd.VectorAngleToRigid(plateCurVector.X, plateCurVector.Y, plateCurVector.Angle * Math.PI / 180, addVector.X, addVector.Y, addVector.Angle * Math.PI / 180);
            ////////////////////////////////////
            double Sx, Sy, Phi, Theta, Tx, Ty;
            Sx = hHomMat2DAdd.HomMat2dToAffinePar(out Sy, out Phi, out Theta, out Tx, out Ty);
            AddXYTheta.X = Tx;
            AddXYTheta.Y = Ty;
            AddXYTheta.Angle = Phi * 180 / Math.PI;
            return true;
        }

        public static bool CalculateAlign(userPixCoordSystem[] sourcePixCoordSystem, userPixCoordSystem[] targetPixCoordSystem, CompensationParam param, out userWcsVector AddXYTheta)
        {
            if (sourcePixCoordSystem == null || sourcePixCoordSystem.Length < 2)
                throw new ArgumentNullException(" sourcePixCoordSystem 为空或长度小于2 ");
            if (targetPixCoordSystem == null || targetPixCoordSystem.Length < 2)
                throw new ArgumentNullException(" targetPixCoordSystem 为空或长度小于2 ");
            if (sourcePixCoordSystem.Length != targetPixCoordSystem.Length)
                throw new ArgumentNullException(" 元素长度不相等 ");
            ////////////////////////////////////////////////////////////////////////
            HalconLibrary ha = new HalconLibrary();
            HalconDotNet.HHomMat2D hHomMat2DSource = null, hHomMat2DTarget = null;
            /////// 将像素坐标转换为世界坐标,平台数据转换 ///////////////////////////////////////
            userWcsVector[] sourceWcsCurPoint = new userWcsVector[sourcePixCoordSystem.Length];
            userWcsVector[] sourceWcsTeachPoint = new userWcsVector[sourcePixCoordSystem.Length];
            userWcsVector[] targetWcsCurPoint = new userWcsVector[targetPixCoordSystem.Length];
            userWcsVector[] targetWcsTeachPoint = new userWcsVector[targetPixCoordSystem.Length];
            for (int i = 0; i < sourcePixCoordSystem.Length; i++)
            {
                sourceWcsCurPoint[i] = sourcePixCoordSystem[i].CurrentPoint.GetWcsVector();
                sourceWcsTeachPoint[i] = sourcePixCoordSystem[i].ReferencePoint.GetWcsVector(sourceWcsCurPoint[i].Grab_x, sourceWcsCurPoint[i].Grab_y);
            }
            /////////////////////////////////////////////
            for (int i = 0; i < targetPixCoordSystem.Length; i++)
            {
                targetWcsCurPoint[i] = targetPixCoordSystem[i].CurrentPoint.GetWcsVector();
                targetWcsTeachPoint[i] = targetPixCoordSystem[i].ReferencePoint.GetWcsVector(targetWcsCurPoint[i].Grab_x, targetWcsCurPoint[i].Grab_y);
            }
            ////////////////////////// 用4点计算角度
            hHomMat2DSource = ha.GetHomMat2D(sourceWcsTeachPoint, sourceWcsCurPoint, null); // 计算源点到目标点间的变换，
            hHomMat2DTarget = ha.GetHomMat2D(targetWcsTeachPoint, targetWcsCurPoint, null);  // 计算源点到目标点间的变换，
            userWcsVector sourceVector = ha.GetHomMat2DXYTheta(hHomMat2DSource);
            userWcsVector targetVector = ha.GetHomMat2DXYTheta(hHomMat2DTarget); // 用N点来算角度
            ////// 计算中心点 
            List<double> sourceTeach_x = new List<double>();
            List<double> sourceTeach_y = new List<double>();
            List<double> sourceCur_x = new List<double>();
            List<double> sourceCur_y = new List<double>();
            List<double> targetTeach_x = new List<double>();
            List<double> targetTeach_y = new List<double>();
            List<double> targetCur_x = new List<double>();
            List<double> targetCur_y = new List<double>();
            foreach (var item in sourceWcsTeachPoint)
            {
                sourceTeach_x.Add(item.X);
                sourceTeach_y.Add(item.Y);
            }
            foreach (var item in sourceWcsCurPoint)
            {
                sourceCur_x.Add(item.X);
                sourceCur_y.Add(item.Y);
            }
            foreach (var item in targetWcsTeachPoint)
            {
                targetTeach_x.Add(item.X);
                targetTeach_y.Add(item.Y);
            }
            foreach (var item in targetWcsCurPoint)
            {
                targetCur_x.Add(item.X);
                targetCur_y.Add(item.Y);
            }
            ////////////////////////////////////////////  这种方式，将示教向量归一化为0值 /////////////////////////////////
            userWcsVector sourceTeachVector = new userWcsVector(sourceTeach_x.Average(), sourceTeach_y.Average(), 0, 0);
            userWcsVector sourceCurVector = new userWcsVector(sourceCur_x.Average(), sourceCur_y.Average(), 0, sourceVector.Angle); // 这里使用的角度是变化角度，而不是实际角度
            userWcsVector targetTeachVector = new userWcsVector(targetTeach_x.Average(), targetTeach_y.Average(), 0, 0);
            userWcsVector targetCurVector = new userWcsVector(targetCur_x.Average(), targetCur_y.Average(), 0, targetVector.Angle);
            ////// 这种方式，计算各自的向量，类似于向量对位，只不过角度使用多点来计算 ////////////
            ///////////////////////////////////
            userWcsVector subVectorTarget = targetCurVector - targetTeachVector; // 贴头的偏差
            userWcsVector addVector = sourceTeachVector + subVectorTarget; // 平台的示教位置 + 贴头偏差 + 补偿值
            addVector.X += param.Add_X;
            addVector.Y += param.Add_Y;
            addVector.Angle += param.Add_Angle;
            ////////////////////////////////////////////
            HHomMat2D hHomMat2DAdd = new HHomMat2D();
            AddXYTheta = new userWcsVector();
            hHomMat2DAdd.VectorAngleToRigid(sourceCurVector.X, sourceCurVector.Y, sourceCurVector.Angle * Math.PI / 180, addVector.X, addVector.Y, addVector.Angle * Math.PI / 180);
            ////////////////////////////////////
            double Sx, Sy, Phi, Theta, Tx, Ty;
            Sx = hHomMat2DAdd.HomMat2dToAffinePar(out Sy, out Phi, out Theta, out Tx, out Ty);
            AddXYTheta.X = Tx;
            AddXYTheta.Y = Ty;
            AddXYTheta.Angle = Phi * 180 / Math.PI;
            return true;
        }


        /// <summary>
        /// 向量对位
        /// </summary>
        /// <param name="plateTeachVectorPix"></param>
        /// <param name="plateCurVectorPix"></param>
        /// <param name="bandTeachVectorPix"></param>
        /// <param name="bandCurVectorPix"></param>
        /// <param name="param"></param>
        /// <param name="AddXYTheta"></param>
        /// <returns></returns>
        public static bool CalculateAlign(userPixVector plateTeachVectorPix, userPixVector plateCurVectorPix, userPixVector bandTeachVectorPix, userPixVector bandCurVectorPix, CompensationParam param, out userWcsVector AddXYTheta)
        {
            if (plateTeachVectorPix == null)
                throw new ArgumentNullException(" plateTeachVector ");
            if (plateCurVectorPix == null)
                throw new ArgumentNullException(" plateCurVector ");
            if (bandTeachVectorPix == null)
                throw new ArgumentNullException(" bandTeachVector ");
            if (bandCurVectorPix == null)
                throw new ArgumentNullException(" bandCurVector ");
            //////////////// 要用当前向量的拍照坐标来计算 /////////////////////////////
            userWcsVector plateCurVector = plateCurVectorPix.GetWcsVector();
            userWcsVector plateTeachVector = plateTeachVectorPix.GetWcsVector(plateCurVectorPix.Grab_x, plateCurVectorPix.Grab_y);
            userWcsVector bandCurVector = bandCurVectorPix.GetWcsVector();
            userWcsVector bandTeachVector = bandTeachVectorPix.GetWcsVector(bandCurVectorPix.Grab_x, bandCurVectorPix.Grab_y);
            //////////////////////////////////////////////////////          
            userWcsVector subVectorBand = bandCurVector - bandTeachVector; // 贴头的偏差
            userWcsVector addVector = plateTeachVector + subVectorBand; // 平台的示教位置 + 贴头偏差 + 补偿值
            addVector.X += param.Add_X;
            addVector.Y += param.Add_Y;
            addVector.Angle += param.Add_Angle;
            ////////////////////////////////////////////
            HHomMat2D hHomMat2DAdd = new HHomMat2D();
            AddXYTheta = new userWcsVector();
            hHomMat2DAdd.VectorAngleToRigid(plateCurVector.X, plateCurVector.Y, plateCurVector.Angle * Math.PI / 180, addVector.X, addVector.Y, addVector.Angle * Math.PI / 180);
            ////////////////////////////////////
            double Sx, Sy, Phi, Theta, Tx, Ty;
            Sx = hHomMat2DAdd.HomMat2dToAffinePar(out Sy, out Phi, out Theta, out Tx, out Ty);
            AddXYTheta.X = Tx;
            AddXYTheta.Y = Ty;
            AddXYTheta.Angle = Phi * 180 / Math.PI;
            return true;
        }

        public static bool Rectify(userPixCoordSystem[] pixCoordSystem, RectifyCalculateParam param, out userWcsVector AddXYTheta)
        {
            if (pixCoordSystem == null)
                throw new ArgumentNullException(nameof(pixCoordSystem));
            AddXYTheta = new userWcsVector();
            //////////////// 要用当前向量的拍照坐标来计算 /////////////////////////////
            userWcsVector plateCurVector = null;
            userWcsVector plateTeachVector = null;
            double[] plateCur_x = new double[pixCoordSystem.Length];
            double[] plateCur_y = new double[pixCoordSystem.Length];
            double[] plateCur_angle = new double[pixCoordSystem.Length];
            double[] plateTeach_x = new double[pixCoordSystem.Length];
            double[] plateTeach_y = new double[pixCoordSystem.Length];
            double[] plateTeach_angle = new double[pixCoordSystem.Length];
            for (int i = 0; i < pixCoordSystem.Length; i++)
            {
                plateCurVector = pixCoordSystem[i].CurrentPoint.GetWcsVector();
                plateTeachVector = pixCoordSystem[i].ReferencePoint.GetWcsVector();
                plateCur_x[i] = plateCurVector.X;
                plateCur_y[i] = plateCurVector.Y;
                plateCur_angle[i] = plateCurVector.Angle;
                plateTeach_x[i] = plateTeachVector.X;
                plateTeach_y[i] = plateTeachVector.Y;
                plateTeach_angle[i] = plateTeachVector.Angle;
                AddXYTheta.CamName = pixCoordSystem[i].CurrentPoint.CamName;
                AddXYTheta.CamParams = pixCoordSystem[i].CurrentPoint.CamParams;
                AddXYTheta.ViewWindow = pixCoordSystem[i].CurrentPoint.ViewWindow;
            }
            ////////////////////////////////////////////
            HHomMat2D hHomMat2DAdd = new HHomMat2D();
            switch (plateCur_x.Length)
            {
                case 0:
                    break;
                case 1:
                    if (param.Mode == "自动")
                    {
                        //param.Angle = plateTeach_angle[0];
                        hHomMat2DAdd.VectorAngleToRigid(plateCur_x[0], plateCur_y[0], plateCur_angle[0] * Math.PI / 180, plateTeach_x[0], plateTeach_y[0], plateTeach_angle[0] * Math.PI / 180);
                    }
                    else
                    {
                        if (Math.Abs(plateCur_angle[0] - param.Angle) < 0.007)
                            hHomMat2DAdd.VectorAngleToRigid(plateCur_x[0], plateCur_y[0], plateCur_angle[0], plateTeach_x[0], plateTeach_y[0], param.Angle * Math.PI / 180);
                        else
                            hHomMat2DAdd = new HHomMat2D();
                    }
                    break;
                case 2:   // 两个点使用Mark点的中间点来计算 
                    double radCur = Math.Atan2(plateCur_y[1] - plateCur_y[0], plateCur_x[1] - plateCur_x[0]);
                    double radTeach = Math.Atan2(plateTeach_y[1] - plateTeach_y[0], plateTeach_x[1] - plateTeach_x[0]);
                    double meanCur_x = (plateCur_x[0] + plateCur_x[1]) * 0.5;
                    double meanCur_y = (plateCur_y[0] + plateCur_y[1]) * 0.5;
                    double meanTeach_x = (plateTeach_x[0] + plateTeach_x[1]) * 0.5;
                    double meanTeach_y = (plateTeach_y[0] + plateTeach_y[1]) * 0.5;
                    if (param.Mode == "自动")
                    {
                        //param.Angle = radTeach * 180 / Math.PI;
                        hHomMat2DAdd.VectorAngleToRigid(meanCur_x, meanCur_y, radCur, meanTeach_x, meanTeach_y, radTeach);
                    }
                    else
                        hHomMat2DAdd.VectorAngleToRigid(meanCur_x, meanCur_y, radCur, meanTeach_x, meanTeach_y, param.Angle * Math.PI / 180);
                    break;
                default: // 定位点大于2个以上，不能指定角度
                    if (param.Mode == "自动")
                    {
                        hHomMat2DAdd.VectorToRigid(plateCur_x, plateCur_y, plateTeach_x, plateTeach_y);
                    }
                    else
                    {
                        HTuple affine_x, affine_y;
                        double[] Px = new double[4] { -1, 1, 1, -1 };
                        double[] Py = new double[4] { 1, 1, -1, -1 };
                        double[] Qx = new double[4];
                        double[] Qy = new double[4];
                        for (int i = 0; i < plateCur_x.Length; i++)
                        {
                            Qx[i] = plateCur_x[i];
                            Qy[i] = plateCur_y[i];
                        }
                        hHomMat2DAdd.VectorAngleToRigid(0, 0, 90 * Math.PI / 180, 0, 0, param.Angle * Math.PI / 180);
                        affine_x = hHomMat2DAdd.AffineTransPoint2d(Px, Py, out affine_y);
                        hHomMat2DAdd = new HHomMat2D();
                        hHomMat2DAdd.VectorToHomMat2d(Qx, Qy, affine_x, affine_y);

                        HTuple hTuple_x, hTuple_y;
                        hTuple_x = hHomMat2DAdd.AffineTransPoint2d(Qx, Qy, out hTuple_y);
                        HTuple dist = HMisc.DistancePp(hTuple_x, hTuple_y, affine_x, affine_y);
                    }
                    break;
            }
            ////////////////////////////////////
            double Sx, Sy, Phi, Theta, Tx, Ty;
            if (param.Orientation == "当前位")
                Sx = hHomMat2DAdd.HomMat2dInvert().HomMat2dToAffinePar(out Sy, out Phi, out Theta, out Tx, out Ty);
            else
                Sx = hHomMat2DAdd.HomMat2dToAffinePar(out Sy, out Phi, out Theta, out Tx, out Ty);
            AddXYTheta.X = Tx;
            AddXYTheta.Y = Ty;
            AddXYTheta.Angle = Phi * 180 / Math.PI;
            return true;
        }

        public static bool Rectify(userWcsCoordSystem[] wcsCoordSystem, RectifyCalculateParam param, out userWcsVector AddXYTheta, out userWcsVector curWcsVector, out userWcsVector teachWcsVector, out double maxError)
        {
            if (wcsCoordSystem == null)
                throw new ArgumentNullException(nameof(wcsCoordSystem));
            AddXYTheta = new userWcsVector();
            curWcsVector = new userWcsVector();
            teachWcsVector = new userWcsVector();
            maxError = 0;
            //////////////// 要用当前向量的拍照坐标来计算 /////////////////////////////
            userWcsVector plateCurVector = null;
            userWcsVector plateTeachVector = null;
            double[] plateCur_x = new double[wcsCoordSystem.Length];
            double[] plateCur_y = new double[wcsCoordSystem.Length];
            double[] plateCur_angle = new double[wcsCoordSystem.Length];
            double[] plateTeach_x = new double[wcsCoordSystem.Length];
            double[] plateTeach_y = new double[wcsCoordSystem.Length];
            double[] plateTeach_angle = new double[wcsCoordSystem.Length];
            for (int i = 0; i < wcsCoordSystem.Length; i++)
            {
                //switch (param.Mode)
                //{
                //    // 这两种模式需要将坐标转换为以旋转中心为原点的坐标
                //    case "自动":
                //    case "手动":
                //        plateCurVector = wcsCoordSystem[i].CurrentPoint.GetPixVector().GetWcsVector(enCoordOriginType.旋转中心);
                //        plateTeachVector = wcsCoordSystem[i].ReferencePoint.GetPixVector().GetWcsVector(plateCurVector.Grab_x, plateCurVector.Grab_y, enCoordOriginType.旋转中心);
                //        LoggerHelper.Info($"->纠偏计算,示教坐标:x ={plateTeachVector.X},y ={plateTeachVector.Y}", plateCurVector.CamParams.SensorName);
                //        LoggerHelper.Info($"->纠偏计算,当前坐标:x ={plateCurVector.X},y ={plateCurVector.Y}", plateCurVector.CamParams.SensorName);
                //        break;
                //    default:
                //    case "当前坐标系":
                //        plateCurVector = wcsCoordSystem[i].CurrentPoint;
                //        plateTeachVector = wcsCoordSystem[i].ReferencePoint.GetPixVector().GetWcsVector(plateCurVector.Grab_x, plateCurVector.Grab_y);
                //        break;
                //}
                plateCurVector = wcsCoordSystem[i].CurrentPoint;
                plateTeachVector = wcsCoordSystem[i].ReferencePoint.GetPixVector().GetWcsVector(plateCurVector.Grab_x, plateCurVector.Grab_y);
                plateCur_x[i] = plateCurVector.X;
                plateCur_y[i] = plateCurVector.Y;
                plateCur_angle[i] = plateCurVector.Angle;
                plateTeach_x[i] = plateTeachVector.X;
                plateTeach_y[i] = plateTeachVector.Y;
                plateTeach_angle[i] = plateTeachVector.Angle;
                AddXYTheta.CamName = wcsCoordSystem[i].CurrentPoint.CamName;
                AddXYTheta.CamParams = wcsCoordSystem[i].CurrentPoint.CamParams;
                AddXYTheta.ViewWindow = wcsCoordSystem[i].CurrentPoint.ViewWindow;
                ////////////////////
                curWcsVector.CamName = wcsCoordSystem[i].CurrentPoint.CamName;
                curWcsVector.CamParams = wcsCoordSystem[i].CurrentPoint.CamParams;
                curWcsVector.ViewWindow = wcsCoordSystem[i].CurrentPoint.ViewWindow;
                /////////////////////////
                teachWcsVector.CamName = wcsCoordSystem[i].CurrentPoint.CamName;
                teachWcsVector.CamParams = wcsCoordSystem[i].CurrentPoint.CamParams;
                teachWcsVector.ViewWindow = wcsCoordSystem[i].CurrentPoint.ViewWindow;
            }
            ////////////////////////////////////////////
            HHomMat2D hHomMat2DAdd = new HHomMat2D();
            HHomMat2D hHomMat2D = new HHomMat2D();
            HHomMat2D hHomMat2DRotate = new HHomMat2D();
            ////////////////////////////////////////
            switch (plateCur_x.Length)
            {
                case 0:
                    break;
                case 1:
                    switch (param.Mode)
                    {
                        case "自动":
                            hHomMat2DAdd.VectorAngleToRigid(plateTeach_x[0], plateTeach_y[0], plateTeach_angle[0] * Math.PI / 180, plateCur_x[0], plateCur_y[0], plateCur_angle[0] * Math.PI / 180);
                            //////////////////////////////////////////////////////////////////////////////////
                            curWcsVector.X = plateCur_x[0];
                            curWcsVector.Y = plateCur_y[0];
                            curWcsVector.Angle = plateCur_angle[0];
                            teachWcsVector.X = plateTeach_x[0];
                            teachWcsVector.Y = plateTeach_y[0];
                            teachWcsVector.Angle = plateTeach_angle[0];
                            //curWcsVector = new userWcsVector(plateCur_x[0], plateCur_y[0],0, plateCur_angle[0]);
                            //teachWcsVector = new userWcsVector(plateTeach_x[0], plateTeach_y[0], 0, plateTeach_angle[0]);
                            break;
                        case "手动":
                            hHomMat2DAdd.VectorAngleToRigid(plateTeach_x[0], plateTeach_y[0], param.Angle * Math.PI / 180, plateCur_x[0], plateCur_y[0], plateCur_angle[0] * Math.PI / 180);
                            //////////////////////////////////////////////////////////////////////////////////
                            curWcsVector.X = plateCur_x[0];
                            curWcsVector.Y = plateCur_y[0];
                            curWcsVector.Angle = plateCur_angle[0];
                            teachWcsVector.X = plateTeach_x[0];
                            teachWcsVector.Y = plateTeach_y[0];
                            teachWcsVector.Angle = plateTeach_angle[0];
                            //curWcsVector = new userWcsVector(plateCur_x[0], plateCur_y[0], 0, plateCur_angle[0]);
                            //teachWcsVector = new userWcsVector(plateTeach_x[0], plateTeach_y[0], 0, plateTeach_angle[0]);
                            break;
                        case "视野中心":
                            hHomMat2DAdd.VectorAngleToRigid(plateTeach_x[0], plateTeach_y[0], param.Angle * Math.PI / 180, plateCur_x[0], plateCur_y[0], plateCur_angle[0] * Math.PI / 180);
                            //////////////////////////////////////////////////////////////////////////////////
                            curWcsVector.X = plateCur_x[0];
                            curWcsVector.Y = plateCur_y[0];
                            curWcsVector.Angle = plateCur_angle[0];
                            teachWcsVector.X = plateTeach_x[0];
                            teachWcsVector.Y = plateTeach_y[0];
                            teachWcsVector.Angle = plateTeach_angle[0];
                            //curWcsVector = new userWcsVector(plateCur_x[0], plateCur_y[0], 0, plateCur_angle[0]);
                            //teachWcsVector = new userWcsVector(plateTeach_x[0], plateTeach_y[0], 0, plateTeach_angle[0]);
                            break;
                        case "当前坐标系":
                        case "参考坐标系":
                            hHomMat2DAdd.VectorAngleToRigid(0, 0, 0, plateCur_x[0], plateCur_y[0], plateCur_angle[0] * Math.PI / 180);
                            //////////////////////////////////////////////////////////////////////////////////
                            curWcsVector.X = plateCur_x[0];
                            curWcsVector.Y = plateCur_y[0];
                            curWcsVector.Angle = plateCur_angle[0];
                            teachWcsVector.X = plateTeach_x[0];
                            teachWcsVector.Y = plateTeach_y[0];
                            teachWcsVector.Angle = plateTeach_angle[0];
                            //curWcsVector = new userWcsVector(plateCur_x[0], plateCur_y[0], 0, plateCur_angle[0]);
                            //teachWcsVector = new userWcsVector(plateTeach_x[0], plateTeach_y[0], 0, plateTeach_angle[0]);
                            break;
                    }
                    break;
                case 2:   // 两个点使用Mark点的中间点来计算 
                    double radCur = Math.Atan2(plateCur_y[1] - plateCur_y[0], plateCur_x[1] - plateCur_x[0]);
                    double radTeach = Math.Atan2(plateTeach_y[1] - plateTeach_y[0], plateTeach_x[1] - plateTeach_x[0]);
                    double meanCur_x = (plateCur_x[0] + plateCur_x[1]) * 0.5;
                    double meanCur_y = (plateCur_y[0] + plateCur_y[1]) * 0.5;
                    double meanTeach_x = (plateTeach_x[0] + plateTeach_x[1]) * 0.5;
                    double meanTeach_y = (plateTeach_y[0] + plateTeach_y[1]) * 0.5;
                    switch (param.Mode)
                    {
                        case "自动":
                            hHomMat2DAdd.VectorAngleToRigid(meanTeach_x, meanTeach_y, radTeach, meanCur_x, meanCur_y, radCur);
                            //////////////////////////////////////////////////////////////////////////////////
                            curWcsVector.X = meanCur_x;
                            curWcsVector.Y = meanCur_y;
                            curWcsVector.Angle = radCur * 180 / Math.PI;
                            teachWcsVector.X = meanTeach_x;
                            teachWcsVector.Y = meanTeach_y;
                            teachWcsVector.Angle = radTeach * 180 / Math.PI;
                            //curWcsVector = new userWcsVector(meanCur_x, meanCur_y, 0, radCur * 180 / Math.PI);
                            //teachWcsVector = new userWcsVector(meanTeach_x, meanTeach_y, 0, radTeach * 180 / Math.PI);
                            break;
                        case "手动":
                            hHomMat2DAdd.VectorAngleToRigid(meanTeach_x, meanTeach_y, param.Angle * Math.PI / 180, meanCur_x, meanCur_y, radCur);
                            curWcsVector.X = meanCur_x;
                            curWcsVector.Y = meanCur_y;
                            curWcsVector.Angle = radCur * 180 / Math.PI;
                            teachWcsVector.X = meanTeach_x;
                            teachWcsVector.Y = meanTeach_y;
                            teachWcsVector.Angle = radTeach * 180 / Math.PI;
                            //curWcsVector = new userWcsVector(meanCur_x, meanCur_y, 0, radCur * 180 / Math.PI);
                            //teachWcsVector = new userWcsVector(meanTeach_x, meanTeach_y, 0, radTeach * 180 / Math.PI);
                            break;
                        case "当前坐标系":
                            hHomMat2DAdd.VectorAngleToRigid(0, 0, 0, meanCur_x, meanCur_y, radCur);
                            curWcsVector.X = meanCur_x;
                            curWcsVector.Y = meanCur_y;
                            curWcsVector.Angle = radCur * 180 / Math.PI;
                            teachWcsVector.X = meanTeach_x;
                            teachWcsVector.Y = meanTeach_y;
                            teachWcsVector.Angle = radTeach * 180 / Math.PI;
                            //curWcsVector = new userWcsVector(meanCur_x, meanCur_y, 0, radCur * 180 / Math.PI);
                            //teachWcsVector = new userWcsVector(meanTeach_x, meanTeach_y, 0, radTeach * 180 / Math.PI);
                            break;
                    }
                    break;
                default: // 定位点大于2个以上，不能指定角度
                    switch (param.Mode)
                    {
                        case "自动":
                            hHomMat2DAdd.VectorToRigid(plateTeach_x, plateTeach_y, plateCur_x, plateCur_y);
                            break;
                        case "手动":
                            HTuple affineTeach_x, affineTeach_y;
                            double[] Px = new double[4] { -1, 1, 1, -1 };
                            double[] Py = new double[4] { 1, 1, -1, -1 };
                            double[] Qx = new double[4];
                            double[] Qy = new double[4];
                            for (int i = 0; i < plateCur_x.Length; i++)
                            {
                                Qx[i] = plateCur_x[i];
                                Qy[i] = plateCur_y[i];
                            }
                            hHomMat2DAdd.VectorAngleToRigid(0.0, 0.0, 0.0, 0, 0, param.Angle * Math.PI / 180);
                            affineTeach_x = hHomMat2DAdd.AffineTransPoint2d(Px, Py, out affineTeach_y);
                            //////////////////////////////////////
                            hHomMat2DAdd = new HHomMat2D();
                            hHomMat2DAdd.VectorToHomMat2d(affineTeach_x, affineTeach_y, Qx, Qy);
                            HTuple hTuple_x, hTuple_y;
                            hTuple_x = hHomMat2DAdd.AffineTransPoint2d(affineTeach_x, affineTeach_y, out hTuple_y);
                            HTuple dist = HMisc.DistancePp(hTuple_x, hTuple_y, Qx, Qy);
                            maxError = Math.Round(dist.TupleMax().D);
                            break;
                    }
                    break;
            }
            ////////////////////////////////////
            double Sx, Sy, Phi, Theta, Tx, Ty;
            if (param.Orientation == "To示教位")  //"To当前位"
                Sx = hHomMat2DAdd.HomMat2dInvert().HomMat2dToAffinePar(out Sy, out Phi, out Theta, out Tx, out Ty);
            else
                Sx = hHomMat2DAdd.HomMat2dToAffinePar(out Sy, out Phi, out Theta, out Tx, out Ty);
            AddXYTheta.X = Tx;
            AddXYTheta.Y = Ty;
            AddXYTheta.Angle = Phi * 180 / Math.PI;
            return true;
        }

        /// <summary>
        /// 向量对位,采用坐标系传递的方式
        /// </summary>
        /// <param name="plateTeachVectorPix"></param>
        /// <param name="sourcePixCoordSystem"></param>
        /// <param name="bandTeachVectorPix"></param>
        /// <param name="targetPixCoordSystem"></param>
        /// <param name="param"></param>
        /// <param name="AddXYTheta"></param>
        /// <returns></returns>
        public static bool CalculateAlign(userPixCoordSystem sourcePixCoordSystem, userPixCoordSystem targetPixCoordSystem, CompensationParam param, out userWcsVector AddXYTheta)
        {
            if (sourcePixCoordSystem == null)
                throw new ArgumentNullException(nameof(sourcePixCoordSystem));
            if (targetPixCoordSystem == null)
                throw new ArgumentNullException(nameof(targetPixCoordSystem));
            //////////////// 要用当前向量的拍照坐标来计算 /////////////////////////////
            userWcsVector plateCurVector = sourcePixCoordSystem.CurrentPoint.GetWcsVector();
            userWcsVector plateTeachVector = sourcePixCoordSystem.ReferencePoint.GetWcsVector();
            userWcsVector bandCurVector = targetPixCoordSystem.CurrentPoint.GetWcsVector();
            userWcsVector bandTeachVector = targetPixCoordSystem.ReferencePoint.GetWcsVector();
            //////////////////////////////////////////////////////          
            userWcsVector subVectorBand = bandCurVector - bandTeachVector; // 贴头的偏差
            userWcsVector addVector = plateTeachVector + subVectorBand; // 平台的示教位置 + 贴头偏差 + 补偿值
            addVector.X += param.Add_X;
            addVector.Y += param.Add_Y;
            addVector.Angle += param.Add_Angle;
            ////////////////////////////////////////////
            HHomMat2D hHomMat2DAdd = new HHomMat2D();
            AddXYTheta = new userWcsVector();
            hHomMat2DAdd.VectorAngleToRigid(plateCurVector.X, plateCurVector.Y, plateCurVector.Angle * Math.PI / 180, addVector.X, addVector.Y, addVector.Angle * Math.PI / 180);
            ////////////////////////////////////
            double Sx, Sy, Phi, Theta, Tx, Ty;
            Sx = hHomMat2DAdd.HomMat2dToAffinePar(out Sy, out Phi, out Theta, out Tx, out Ty);
            AddXYTheta.X = Tx;
            AddXYTheta.Y = Ty;
            AddXYTheta.Angle = Phi * 180 / Math.PI;
            return true;
        }


        /// <summary>
        /// 根据点跟角度，计算一个向量点
        /// </summary>
        /// <param name="TargetPoint"></param>
        /// <param name="vectorAngle"></param>
        /// <param name="wcsVector"></param>
        /// <returns></returns>
        public static bool CalculateVector(userWcsPoint[] TargetPoint, userWcsVector vectorAngle, out userWcsVector wcsVector)
        {
            bool result = false;
            userWcsVector refVectorPoint = new userWcsVector();
            userWcsVector currentVectorPoint = new userWcsVector();
            wcsVector = new userWcsVector();
            if (TargetPoint == null)
                throw new ArgumentNullException(" TargetPoint ");
            if (vectorAngle == null)
                throw new ArgumentNullException(" vectorAngle ");
            ///////////////////////////////////////////
            List<double> list_x = new List<double>();
            List<double> list_y = new List<double>();
            List<double> list_grabx = new List<double>();
            List<double> list_graby = new List<double>();
            List<double> list_grabTheta = new List<double>();
            CameraParam camera = null;
            string ViewWindow = "";
            string CamName = "";
            foreach (var item in TargetPoint)
            {
                list_x.Add(item.X);
                list_y.Add(item.Y);
                list_grabx.Add(item.Grab_x);
                list_graby.Add(item.Grab_y);
                list_grabTheta.Add(item.Grab_theta);
                camera = item.CamParams;
                ViewWindow = item.ViewWindow;
                CamName = item.CamName;
            }
            wcsVector = new userWcsVector();
            wcsVector.X = list_x.Average();
            wcsVector.Y = list_y.Average();
            wcsVector.Z = 0;
            wcsVector.Angle = vectorAngle.Angle;
            wcsVector.Grab_x = list_grabx.Average();
            wcsVector.Grab_y = list_graby.Average();
            wcsVector.Grab_theta = list_grabTheta.Average();
            wcsVector.ViewWindow = ViewWindow;
            wcsVector.CamName = CamName;
            wcsVector.CamParams = camera;
            result = true;
            return result;
        }





    }
}

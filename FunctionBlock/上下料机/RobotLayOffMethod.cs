using Common;
using HalconDotNet;
using MotionControlCard;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionBlock
{
    public class RobotLayOffMethod
    {

        /// <summary>
        /// 计算Try盘坐标
        /// </summary>
        /// <param name="wcsPoints"></param>
        /// <param name="param"></param>
        /// <param name="platformParam"></param>
        /// <returns></returns>
        public static bool CalculateTryCoord(userWcsPoint[] wcsPoints, TryPlateParam param, out UserTryPlateHoleParam[] platformParam)
        {
            bool result = false;
            platformParam = new UserTryPlateHoleParam[0];
            ///////////////////////////////////////////////
            if (wcsPoints == null || wcsPoints.Length == 0)
                return result;
            //throw new ArgumentNullException(" wcsPoints ");
            if (param == null)
                return result;
            //throw new ArgumentNullException(" param ");
            // 先排序4个角点
            double[] phi = new double[wcsPoints.Length];
            double[] X = new double[wcsPoints.Length];
            double[] Y = new double[wcsPoints.Length];
            for (int i = 0; i < wcsPoints.Length; i++)
            {
                X[i] = wcsPoints[i].X;
                Y[i] = wcsPoints[i].Y;
            }
            double mean_x = X.Average();
            double mean_y = Y.Average();
            Dictionary<double, userWcsPoint> dic = new Dictionary<double, userWcsPoint>();
            for (int i = 0; i < wcsPoints.Length; i++)
            {
                double rad = Math.Atan2(wcsPoints[i].Y - mean_y, wcsPoints[i].X - mean_x);
                if (rad < 0)
                    phi[i] = rad + Math.PI * 2;
                else
                    phi[i] = rad;
                dic.Add(phi[i], wcsPoints[i]);
            }
            userWcsPoint[] wcsPointSort = new userWcsPoint[wcsPoints.Length];
            Array.Sort(phi);
            for (int i = 0; i < phi.Length; i++)
            {
                wcsPointSort[i] = new userWcsPoint(dic[phi[i]].X, dic[phi[i]].Y, dic[phi[i]].Z, dic[phi[i]].CamParams);
            }
            for (int i = 0; i < wcsPointSort.Length; i++)
            {
                X[i] = wcsPointSort[i].X;
                Y[i] = wcsPointSort[i].Y;
            }
            ///////////////// 计算Try盘的长宽尺寸 //////////  右上角为第一个坐标点
            double width1 = HalconDotNet.HMisc.DistancePp(X[0], Y[0], X[1], Y[1]);
            double width2 = HalconDotNet.HMisc.DistancePp(X[2], Y[2], X[3], Y[3]);
            double height1 = HalconDotNet.HMisc.DistancePp(X[2], Y[2], X[1], Y[1]);
            double height2 = HalconDotNet.HMisc.DistancePp(X[0], Y[0], X[3], Y[3]);
            double productWidth = (width1 + width2) * 0.5;
            double productHeight = (height1 + height2) * 0.5;
            //// 计算变换矩阵
            double[] std_x = new double[X.Length];
            double[] std_y = new double[X.Length];
            std_x[0] = productWidth * 0.5;
            std_y[0] = productHeight * 0.5;
            std_x[1] = productWidth * -0.5;
            std_y[1] = productHeight * 0.5;
            std_x[2] = productWidth * -0.5;
            std_y[2] = productHeight * -0.5;
            std_x[3] = productWidth * 0.5;
            std_y[3] = productHeight * -0.5;
            HalconDotNet.HHomMat2D hHomMat2D = new HalconDotNet.HHomMat2D();
            hHomMat2D.VectorToHomMat2d(std_x, std_y, X, Y); // 相当于中心对中心的映射
            //// 变换标准坐标点到Try盘中来 ////////// 所有穴的实际位置
            int index = 0;
            platformParam = new UserTryPlateHoleParam[param.CoordsList.Count];
            foreach (var item in param.CoordsList)
            {
                platformParam[index] = (item.Affine(hHomMat2D));
                index++;
            }
            result = true;
            return result;
        }
        /// <summary>
        /// 计算夹抓下料位姿
        /// </summary>
        /// <param name="hImage"></param>
        /// <param name="platformParam"></param>
        /// <param name="param"></param>
        /// <param name="addVector"></param>
        /// <param name="hXLDCont"></param>
        /// <param name="HoleParam"></param>
        /// <returns></returns>
        public static bool JawLayOffPose(ImageDataClass hImage, UserTryPlateHoleParam[] platformParam, RobotParam param, out userWcsVector addVector, out HalconDotNet.HXLDCont hXLDCont, out UserTryPlateHoleParam HoleParam)
        {
            bool result = false;
            hXLDCont = new HalconDotNet.HXLDCont();
            addVector = new userWcsVector();
            HoleParam = new UserTryPlateHoleParam();
            ///////////////////////////////////////////////
            if (hImage == null) return result;
           //throw new ArgumentNullException(" hImage ");
            if (platformParam == null) return result;
            //throw new ArgumentNullException(" platformParam ");
            if (param == null) return result;
            //throw new ArgumentNullException(" param ");
            //////////////  获取可放置产品的穴坐标  //////// 
            double row, col;
            userWcsVector productPose = new userWcsVector();  // 夹抓放置产品的位姿
            userWcsVector jawPose = new userWcsVector();
            HalconDotNet.HRegion hRegion;
            double sy = 0, phi1 = 0, theta = 0, tx = 0, ty = 0, deviation;
            HHomMat2D hHomMat2D = new HHomMat2D();
            foreach (var item in platformParam)
            {
                if (item.RobotJaw != param.RobotJaw || item.RobotJaw != enRobotJawEnum.All) continue;
                hImage.CamParams.WorldPointsToImagePlane(item.X, item.Y, hImage.Grab_X, hImage.Grab_Y, out row, out col);
                hRegion = new HalconDotNet.HRegion();
                hRegion.GenRectangle2(row, col, item.Angle * Math.PI / 180, param.RectSize, param.RectSize);
                hXLDCont = hRegion.GenContourRegionXld("border");
                double grayValue = hImage.Image.Intensity(hRegion, out deviation);
                switch (param.Operate)
                {
                    case enOperation.and:
                        if (param.MinGray < grayValue && grayValue < param.MaxGray) // 表示这个穴里没有产品
                        {
                            jawPose = RobotJawParaManager.Instance.GetJawValue(item.RobotJaw);
                            hHomMat2D.VectorAngleToRigid(jawPose.X, jawPose.Y, jawPose.Angle * Math.PI / 180, item.X, item.Y, item.Angle * Math.PI / 180);
                            hHomMat2D.HomMat2dToAffinePar(out sy, out phi1, out theta, out tx, out ty);
                            productPose = new userWcsVector(tx, ty, 0, phi1 * 180 / Math.PI);
                            HoleParam = item;
                            break;
                        }
                        break;
                    case enOperation.or:
                        if (grayValue < param.MinGray || param.MaxGray < grayValue) // 表示这个穴里没有产品
                        {
                            jawPose = RobotJawParaManager.Instance.GetJawValue(item.RobotJaw);
                            hHomMat2D.VectorAngleToRigid(jawPose.X, jawPose.Y, jawPose.Angle * Math.PI / 180, item.X, item.Y, item.Angle * Math.PI / 180);
                            hHomMat2D.HomMat2dToAffinePar(out sy, out phi1, out theta, out tx, out ty);
                            productPose = new userWcsVector(tx, ty, 0, phi1 * 180 / Math.PI);
                            HoleParam = item;
                            break;
                        }
                        break;
                }
            }
            //////////////////////////////////////////////
            if (HoleParam.Describe != "NONE")
            {
                switch (param.RefGrabPose)
                {
                    case enRefGrabPose.当前位置:
                        CoordSysAxisParam axisParam = new CoordSysAxisParam();
                        axisParam.UpdataAxisPosition(hImage.CamParams.CaliParam.CoordSysName); // 实时使用当前位置
                        addVector.X = productPose.X - axisParam.X;
                        addVector.Y = productPose.Y - axisParam.Y;
                        addVector.Angle = phi1 * 180 / Math.PI;
                        break;
                    default:
                    case enRefGrabPose.标定位置:
                        addVector.X = productPose.X - hImage.CamParams.CaliParam.RotateCalibPoint.X;
                        addVector.Y = productPose.Y - hImage.CamParams.CaliParam.RotateCalibPoint.Y;
                        addVector.Angle = phi1 * 180 / Math.PI;
                        break;
                }
                /// 超限判断
                if (addVector.X < param.LimitN_X || addVector.X > param.LimitP_X || addVector.Y < param.LimitN_Y || addVector.Y > param.LimitP_Y)
                    HoleParam.Result = "NG";
                else
                    HoleParam.Result = "OK";
            }
            result = true;
            return result;
        }
        /// <summary>
        /// 计算夹抓上料位姿
        /// </summary>
        /// <param name="productPose"></param>
        /// <param name="platformParam"></param>
        /// <param name="param"></param>
        /// <param name="addVector"></param>
        /// <param name="HoleParam"></param>
        /// <returns></returns>
        public static bool JawLoadPose(userWcsVector productPose, UserTryPlateHoleParam[] platformParam, RobotParam param, out userWcsVector addVector, out UserTryPlateHoleParam HoleParam)
        {
            bool result = false;
            addVector = new userWcsVector();
            HoleParam = new UserTryPlateHoleParam();
            if (productPose == null) return result;
            //throw new ArgumentNullException(" productPose ");
            if (platformParam == null) return result;
            //throw new ArgumentNullException(" platformParam ");
            if (param == null) return result;
            //throw new ArgumentNullException(" param ");
            //////////////  获取可放置产品的穴坐标  //////// 
            userWcsVector jawPose = new userWcsVector();
            userWcsVector jawAddPose = new userWcsVector();
            double sy = 0, phi1 = 0, theta = 0, tx = 0, ty = 0;
            HHomMat2D hHomMat2D = new HHomMat2D();
            double initDist = double.MaxValue;
            foreach (var item in platformParam)
            {
                if (item.RobotJaw != param.RobotJaw || item.RobotJaw != enRobotJawEnum.All) continue;
                if (item.PixRoi == null)
                {
                    drawPixRect1 pixrect1 = new drawPixRect1(0, 0, productPose.CamParams.DataHeight, productPose.CamParams.DataWidth);
                    item.PixRoi = pixrect1;
                }
                /// 找到距离产品位置最近的穴位点
                double row, col;
                item.PixRoi.GetRoiCenter(out row, out col);
                userPixVector pixVector = productPose.GetPixVector();
                double dist = HMisc.DistancePp(pixVector.Row, pixVector.Col, row, col);
                if (dist <= initDist)
                {
                    initDist = dist;
                    HoleParam = item;
                }
            }
            //////////////////////////////////////////////
            if (HoleParam.Describe != "NONE")
            {
                jawPose = RobotJawParaManager.Instance.GetJawValue(param.RobotJaw);
                hHomMat2D.VectorAngleToRigid(jawPose.X, jawPose.Y, jawPose.Angle * Math.PI / 180, HoleParam.X + productPose.X, HoleParam.Y + productPose.Y, HoleParam.Angle * Math.PI / 180 + productPose.Angle * Math.PI / 180);
                hHomMat2D.HomMat2dToAffinePar(out sy, out phi1, out theta, out tx, out ty);
                jawAddPose = new userWcsVector(tx, ty, 0, phi1 * 180 / Math.PI);
                switch (param.RefGrabPose)
                {
                    case enRefGrabPose.当前位置:
                        CoordSysAxisParam axisParam = new CoordSysAxisParam();
                        axisParam.UpdataAxisPosition(productPose.CamParams.CaliParam.CoordSysName); // 实时使用当前位置
                        addVector.X = jawAddPose.X - axisParam.X;
                        addVector.Y = jawAddPose.Y - axisParam.Y;
                        addVector.Angle = phi1 * 180 / Math.PI;
                        break;
                    default:
                    case enRefGrabPose.标定位置:
                        addVector.X = jawAddPose.X - productPose.CamParams.CaliParam.RotateCalibPoint.X;
                        addVector.Y = jawAddPose.Y - productPose.CamParams.CaliParam.RotateCalibPoint.Y;
                        addVector.Angle = phi1 * 180 / Math.PI;
                        break;
                }
                /// 超限判断
                if (addVector.X < param.LimitN_X || addVector.X > param.LimitP_X || addVector.Y < param.LimitN_Y || addVector.Y > param.LimitP_Y)
                    HoleParam.Result = "NG";
                else
                    HoleParam.Result = "OK";
            }
            result = true;
            return result;
        }

    }
}

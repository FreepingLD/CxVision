using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MotionControlCard;
using System.Windows.Forms;
using System.Threading;
using HalconDotNet;
using Common;
using System.IO;
using Sensor;
using Command;
using AlgorithmsLibrary;
using System.ComponentModel;
using System.Data;


namespace FunctionBlock
{
    [Serializable]

    /// <summary>
    /// 激光取点采集的数据类
    /// </summary>
    public class CalculateMethod
    {

        public static bool CalculateWcsCenter(userWcsPoint[] sourcePoint, userWcsPoint[] targetPoint, out userWcsCircle wcsCircle)
        {
            bool result = false;
            if (sourcePoint == null)
            {
                throw new ArgumentNullException("sourcePoint");
            }
            if (targetPoint == null)
            {
                throw new ArgumentNullException("targetPoint");
            }
            if (sourcePoint.Length != targetPoint.Length)
            {
                throw new ArgumentException("数组长度不相等");
            }
            if (sourcePoint.Length < 4)
                throw new ArgumentException("sourcePoint 长度小于4");
            if (targetPoint.Length < 4)
                throw new ArgumentException(" targetPoint 长度小于4");
            ///////////////////////////////////////////////////////////
            wcsCircle = new userWcsCircle();
            double[] Px = new double[sourcePoint.Length];
            double[] Py = new double[sourcePoint.Length];
            double[] Qx = new double[sourcePoint.Length];
            double[] Qy = new double[sourcePoint.Length];
            for (int i = 0; i < sourcePoint.Length; i++)
            {
                Px[i] = sourcePoint[i].X;
                Py[i] = sourcePoint[i].Y;
                Qx[i] = targetPoint[i].X;
                Qy[i] = targetPoint[i].Y;
            }
            double x = 0, y = 0, sy = 0, phi = 0, theta = 0, tx = 0, ty = 0;
            HHomMat2D hHomMat2D = new HHomMat2D();
            hHomMat2D.VectorToHomMat2d(Px, Py, Qx, Qy);
            hHomMat2D.HomMat2dToAffinePar(out sy, out phi, out theta, out tx, out ty);
            UserHomMat2D HomMat = new UserHomMat2D(hHomMat2D);
            //x* (1 - HomMat.c00) = (HomMat.c01 * y + HomMat.c02);
            //y = (HomMat.c10 * x + HomMat.c12)/(1- HomMat.c11);
            //x * (1 - HomMat.c00) = (HomMat.c01 * (HomMat.c10 * x + HomMat.c12) / (1 - HomMat.c11) + HomMat.c02);
            x = ((HomMat.c01 * HomMat.c12) + HomMat.c02 * (1 - HomMat.c11)) / ((1 - HomMat.c00) * (1 - HomMat.c11) - HomMat.c01 * HomMat.c10);
            y = (HomMat.c10 * x + HomMat.c12) / (1 - HomMat.c11);
            /// 方法 2
            //x = ((hHomMat2D[1] * hHomMat2D[5]) + hHomMat2D[2] * (1 - hHomMat2D[4])) / ((1 - hHomMat2D[0]) * (1 - hHomMat2D[4]) - hHomMat2D[1] * hHomMat2D[3]);
            //y = (hHomMat2D[3] * x + hHomMat2D[5]) / (1 - hHomMat2D[4]);
            //////////////////////////////
            wcsCircle.X = x;
            wcsCircle.Y = y;
            wcsCircle.Z = 0;
            wcsCircle.Grab_theta = phi * 180 / Math.PI;

            result = true;
            return result;

        }

        public static bool CalculatePixCenter(userWcsPoint[] sourcePoint, userWcsPoint[] targetPoint, out userPixCircle pixCircle)
        {
            bool result = false;
            if (sourcePoint == null)
            {
                throw new ArgumentNullException("sourcePoint");
            }
            if (targetPoint == null)
            {
                throw new ArgumentNullException("targetPoint");
            }
            if (sourcePoint.Length != targetPoint.Length)
            {
                throw new ArgumentException("数组长度不相等");
            }
            if (sourcePoint.Length < 4)
                throw new ArgumentException("sourcePoint 长度小于4");
            if (targetPoint.Length < 4)
                throw new ArgumentException(" targetPoint 长度小于4");
            ///////////////////////////////////////////////////////////
            pixCircle = new userPixCircle();
            double[] Px = new double[sourcePoint.Length];
            double[] Py = new double[sourcePoint.Length];
            double[] Qx = new double[sourcePoint.Length];
            double[] Qy = new double[sourcePoint.Length];
            for (int i = 0; i < sourcePoint.Length; i++)
            {
                userPixPoint pixPoint = sourcePoint[i].GetPixPoint();
                Px[i] = pixPoint.Row;
                Py[i] = pixPoint.Col;
                pixPoint = targetPoint[i].GetPixPoint();
                Qx[i] = pixPoint.Row;
                Qy[i] = pixPoint.Col;
            }
            double x = 0, y = 0, sy = 0, phi = 0, theta = 0, tx = 0, ty = 0;
            HHomMat2D hHomMat2D = new HHomMat2D();
            hHomMat2D.VectorToHomMat2d(Px, Py, Qx, Qy);
            hHomMat2D.HomMat2dToAffinePar(out sy, out phi, out theta, out tx, out ty);
            UserHomMat2D HomMat = new UserHomMat2D(hHomMat2D);
            //x* (1 - HomMat.c00) = (HomMat.c01 * y + HomMat.c02);
            //y = (HomMat.c10 * x + HomMat.c12)/(1- HomMat.c11);
            //x * (1 - HomMat.c00) = (HomMat.c01 * (HomMat.c10 * x + HomMat.c12) / (1 - HomMat.c11) + HomMat.c02);
            x = ((HomMat.c01 * HomMat.c12) + HomMat.c02 * (1 - HomMat.c11)) / ((1 - HomMat.c00) * (1 - HomMat.c11) - HomMat.c01 * HomMat.c10);
            y = (HomMat.c10 * x + HomMat.c12) / (1 - HomMat.c11);
            /// 方法 2
            //x = ((hHomMat2D[1] * hHomMat2D[5]) + hHomMat2D[2] * (1 - hHomMat2D[4])) / ((1 - hHomMat2D[0]) * (1 - hHomMat2D[4]) - hHomMat2D[1] * hHomMat2D[3]);
            //y = (hHomMat2D[3] * x + hHomMat2D[5]) / (1 - hHomMat2D[4]);
            //////////////////////////////
            pixCircle.Row = x;
            pixCircle.Col = y;
            pixCircle.Grab_theta = phi * 180 / Math.PI;
            result = true;
            return result;

        }


        public static bool CalculatePix(userPixPoint[] sourcePoint, userPixPoint[] targetPoint, out userPixCircle pixCircle)
        {
            bool result = false;
            if (sourcePoint == null)
            {
                throw new ArgumentNullException("sourcePoint");
            }
            if (targetPoint == null)
            {
                throw new ArgumentNullException("targetPoint");
            }
            if (sourcePoint.Length != targetPoint.Length)
            {
                throw new ArgumentException("数组长度不相等");
            }
            if (sourcePoint.Length < 4)
                throw new ArgumentException("sourcePoint 长度小于4");
            if (targetPoint.Length < 4)
                throw new ArgumentException(" targetPoint 长度小于4");
            ///////////////////////////////////////////////////////////
            pixCircle = new userPixCircle();
            double[] Px = new double[sourcePoint.Length];
            double[] Py = new double[sourcePoint.Length];
            double[] Qx = new double[sourcePoint.Length];
            double[] Qy = new double[sourcePoint.Length];
            for (int i = 0; i < sourcePoint.Length; i++)
            {
                Px[i] = sourcePoint[i].Row;
                Py[i] = sourcePoint[i].Col;
                Qx[i] = targetPoint[i].Row;
                Qy[i] = targetPoint[i].Col;
            }
            double x = 0, y = 0, sy = 0, phi = 0, theta = 0, tx = 0, ty = 0;
            HHomMat2D hHomMat2D = new HHomMat2D();
            hHomMat2D.VectorToHomMat2d(Px, Py, Qx, Qy);
            hHomMat2D.HomMat2dToAffinePar(out sy, out phi, out theta, out tx, out ty);
            UserHomMat2D HomMat = new UserHomMat2D(hHomMat2D);
            //x* (1 - HomMat.c00) = (HomMat.c01 * y + HomMat.c02);
            //y = (HomMat.c10 * x + HomMat.c12)/(1- HomMat.c11);
            //x * (1 - HomMat.c00) = (HomMat.c01 * (HomMat.c10 * x + HomMat.c12) / (1 - HomMat.c11) + HomMat.c02);
            x = ((HomMat.c01 * HomMat.c12) + HomMat.c02 * (1 - HomMat.c11)) / ((1 - HomMat.c00) * (1 - HomMat.c11) - HomMat.c01 * HomMat.c10);
            y = (HomMat.c10 * x + HomMat.c12) / (1 - HomMat.c11);
            /// 方法 2
            //x = ((hHomMat2D[1] * hHomMat2D[5]) + hHomMat2D[2] * (1 - hHomMat2D[4])) / ((1 - hHomMat2D[0]) * (1 - hHomMat2D[4]) - hHomMat2D[1] * hHomMat2D[3]);
            //y = (hHomMat2D[3] * x + hHomMat2D[5]) / (1 - hHomMat2D[4]);
            //////////////////////////////
            pixCircle.Row = x;
            pixCircle.Col = y;
            pixCircle.Grab_theta = phi * 180 / Math.PI;

            result = true;
            return result;

        }


        public static bool MapCalib(userWcsPoint[] sourcePoint, userWcsPoint[] targetPoint, MapCalibParam param, out UserHomMat2D homMat2D)
        {
            bool result = false;
            if (sourcePoint == null)
            {
                throw new ArgumentNullException("sourcePoint");
            }
            if (targetPoint == null)
            {
                throw new ArgumentNullException("targetPoint");
            }
            if (sourcePoint.Length != targetPoint.Length)
            {
                throw new ArgumentException("数组长度不相等");
            }
            if (sourcePoint.Length < 4)
                throw new ArgumentException("sourcePoint 长度小于4");
            if (targetPoint.Length < 4)
                throw new ArgumentException(" targetPoint 长度小于4");
            ///////////////////////////////////////////////////////////
            homMat2D = new UserHomMat2D();
            HHomMat2D hHomMat2D = new HHomMat2D();
            double[] Px = new double[sourcePoint.Length];
            double[] Py = new double[sourcePoint.Length];
            double[] Qx = new double[sourcePoint.Length];
            double[] Qy = new double[sourcePoint.Length];
            switch (param.MapCalibMethod)
            {
                case enMapCalibMethod.PixToWcs:
                    for (int i = 0; i < sourcePoint.Length; i++)
                    {
                        userPixPoint pixPoint = sourcePoint[i].GetPixPoint();
                        Px[i] = pixPoint.Col;
                        Py[i] = pixPoint.Row;
                        Qx[i] = targetPoint[i].X;
                        Qy[i] = targetPoint[i].Y;
                    }
                    hHomMat2D.VectorToHomMat2d(Px, Py, Qx, Qy);
                    sourcePoint[0].CamParams.MapHomMat2D = new UserHomMat2D();
                    sourcePoint[0].CamParams.HomMat2D = new UserHomMat2D(hHomMat2D);
                    break;
                case enMapCalibMethod.WcsToWcs:     // 这种模式一般用于相机与相机间的映射标定
                    for (int i = 0; i < sourcePoint.Length; i++)
                    {
                        Px[i] = sourcePoint[i].X;
                        Py[i] = sourcePoint[i].Y;
                        Qx[i] = targetPoint[i].X;
                        Qy[i] = targetPoint[i].Y;
                    }
                    hHomMat2D.VectorToHomMat2d(Px, Py, Qx, Qy);
                    sourcePoint[0].CamParams.MapHomMat2D = new UserHomMat2D(hHomMat2D);
                    break;
                case enMapCalibMethod.PixToPix:
                    for (int i = 0; i < sourcePoint.Length; i++)
                    {
                        userPixPoint pixPoint = sourcePoint[i].GetPixPoint();
                        Px[i] = pixPoint.Row;
                        Py[i] = pixPoint.Col;
                        pixPoint = targetPoint[i].GetPixPoint();
                        Qx[i] = pixPoint.Row;
                        Qy[i] = pixPoint.Col;
                    }
                    hHomMat2D.VectorToHomMat2d(Px, Py, Qx, Qy);
                    sourcePoint[0].CamParams.MapHomMat2D = new UserHomMat2D(hHomMat2D);
                    break;
            }
            result = true;
            return result;

        }


    }

}

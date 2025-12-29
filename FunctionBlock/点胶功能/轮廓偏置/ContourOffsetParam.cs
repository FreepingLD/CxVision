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
    public class ContourOffsetParam
    {
        public double OffsetDist { get; set; }
        public int OffsetCount { get; set; }
        public double Scale { get; set; }
        public string DataSource { get; set; }

        public ContourOffsetParam()
        {
            this.OffsetDist = 0.1;
            this.OffsetCount = 1;
            this.Scale = 1;
        }

        public bool OffsetLine(userWcsLine wcsLines, userWcsLine offsetDist, out userWcsLine wcsLine)
        {
            bool result = false;
            if (wcsLines == null)
            {
                throw new ArgumentNullException(nameof(wcsLines));
            }
            //////////////////////////////////////////////////////////////////
            if (this.Scale == 0) this.Scale = 1;
            if (offsetDist != null)
                this.OffsetDist = offsetDist.GetLength();
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


        public bool OffsetContour(userWcsPoint[] wcsPoint, userWcsLine offsetDist, out userWcsPolyLine wcsPolyLine)
        {
            bool result = false;
            wcsPolyLine = new userWcsPolyLine();
            if (wcsPoint == null)
                throw new ArgumentNullException(nameof(wcsPoint));
            List<double> list_x = new List<double>();
            List<double> list_y = new List<double>();
            List<double> list_z = new List<double>();
            //////////  排序轨迹    ///////////
            foreach (var item in wcsPoint)
            {
                list_x.Add(item.X);
                list_y.Add(item.Y);
                list_z.Add(item.Z);
            }
            /////////////////////////
            if (offsetDist != null)
                this.OffsetDist = offsetDist.GetLength();
            HTuple rows, cols;
            HXLDCont hXLDCont = new HXLDCont(new HTuple(list_y.ToArray()) * -1, new HTuple(list_x.ToArray()));
            HXLDCont hXLDCont1 = hXLDCont.GenParallelContourXld("regression_normal", this.OffsetDist);
            hXLDCont1.GetContourXld(out rows, out cols);
            ///////////////////////////////////////////////////////////
            wcsPolyLine.CamParams = wcsPoint[0].CamParams;
            for (int i = 0; i < rows.Length; i++)
            {
                wcsPolyLine.Add(cols[i].D, rows[i].D * -1, 0);
            }
            result = true;
            return result;
        }

        private static void CalculateContNormal(double[] x, double[] y, double normalLength, out double[] normal_x, out double[] normal_y)
        {
            normal_x = new double[x.Length];
            normal_y = new double[x.Length];
            double rad1 = 0;
            double rad2 = 0;
            HHomMat2D hHomMat2D = new HHomMat2D();
            HHomMat2D hHomMat2D1;
            double sx, sy, phi, theta, tx, ty;
            for (int i = 0; i < x.Length; i++)
            {
                if (i == 0)
                {
                    rad1 = Math.Atan2(y[i + 1] - y[i], x[i + 1] - x[i]); //法向角 + Math.PI * 0.5
                    hHomMat2D.VectorAngleToRigid(0, 0, 0, 0, 0, rad1);
                    hHomMat2D1 = hHomMat2D.HomMat2dRotateLocal(Math.PI * 0.5);
                    sx = hHomMat2D1.HomMat2dToAffinePar(out sy, out phi, out theta, out tx, out ty);
                    /////////////////////////
                    if (phi < 0)
                        phi += Math.PI * 2;
                    normal_x[i] = x[i] + normalLength * Math.Cos(phi);
                    normal_y[i] = y[i] + normalLength * Math.Sin(phi);
                }
                else
                {
                    if (x.Length - 1 == i)
                    {
                        rad1 = Math.Atan2(y[i] - y[i - 1], x[i] - x[i - 1]); //法向角 + Math.PI * 0.5
                        hHomMat2D.VectorAngleToRigid(0, 0, 0, 0, 0, rad1);
                        hHomMat2D1 = hHomMat2D.HomMat2dRotateLocal(Math.PI * 0.5);
                        sx = hHomMat2D1.HomMat2dToAffinePar(out sy, out phi, out theta, out tx, out ty);
                        //////////////////////////
                        if (phi < 0)
                            phi += Math.PI * 2;
                        normal_x[i] = x[i] + normalLength * Math.Cos(phi);
                        normal_y[i] = y[i] + normalLength * Math.Sin(phi);
                    }
                    else
                    {
                        rad1 = Math.Atan2(y[i] - y[i - 1], x[i] - x[i - 1]); //法向角 + Math.PI * 0.5
                        rad2 = Math.Atan2(y[i + 1] - y[i], x[i + 1] - x[i]); //法向角
                        hHomMat2D.VectorAngleToRigid(0, 0, 0, 0, 0, rad1);
                        hHomMat2D1 = hHomMat2D.HomMat2dRotateLocal(Math.PI * 0.5);
                        sx = hHomMat2D1.HomMat2dToAffinePar(out sy, out rad1, out theta, out tx, out ty);
                        ///////////////////////
                        hHomMat2D.VectorAngleToRigid(0, 0, 0, 0, 0, rad2);
                        hHomMat2D1 = hHomMat2D.HomMat2dRotateLocal(Math.PI * 0.5);
                        sx = hHomMat2D1.HomMat2dToAffinePar(out sy, out rad2, out theta, out tx, out ty);
                        //if (rad1 < 0)
                        //    rad1 += Math.PI * 2;
                        //if (rad2 < 0)
                        //    rad2 += Math.PI * 2;
                        hHomMat2D.VectorAngleToRigid(0, 0, rad2, 0, 0, rad1); // 计算 rad2 到 rad1 的角度范围
                        sx = hHomMat2D.HomMat2dToAffinePar(out sy, out phi, out theta, out tx, out ty);
                        hHomMat2D.VectorAngleToRigid(0, 0, 0, 0, 0, rad2);
                        hHomMat2D1 = hHomMat2D.HomMat2dRotateLocal(phi * 0.5); // 再将角度变化 角度范围的一半
                        sx = hHomMat2D1.HomMat2dToAffinePar(out sy, out phi, out theta, out tx, out ty);
                        //double rad;
                        //if (i == 75)
                        //    rad = (rad1 + rad2) * 0.5;
                        normal_x[i] = x[i] + normalLength * Math.Cos((phi));
                        normal_y[i] = y[i] + normalLength * Math.Sin((phi));
                    }
                }
            }

        }

    }
}

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
    public class ContourAffineParam
    {
        public double Offset_X { get; set; }
        public double Offset_Y { get; set; }
        public string Orientation { get; set; }
        public bool InvertAffineOrientation { get; set; }

        public ContourAffineParam()
        {
            this.Offset_X = 0;
            this.Offset_Y = 0;
            this.Orientation = "示教位到当前位";
            this.InvertAffineOrientation = false;
        }

        public bool Affine(userWcsPoint[] wcsPoints, userWcsCoordSystem coordSystem, out userWcsPolyLine wcsPolyLine)
        {
            bool result = false;
            if (wcsPoints == null)
                throw new ArgumentNullException(nameof(wcsPoints));
            if (coordSystem == null)
                coordSystem = new userWcsCoordSystem();
            wcsPolyLine = new userWcsPolyLine();
            //////////////////////////////////////////////////////////////////
            double[] x = new double[wcsPoints.Length];
            double[] y = new double[wcsPoints.Length];
            double[] z = new double[wcsPoints.Length];
            for (int i = 0; i < wcsPoints.Length; i++)
            {
                x[i] = wcsPoints[i].X + this.Offset_X; // 加上偏移值X
                y[i] = wcsPoints[i].Y + this.Offset_Y; // 加上偏移值Y
                z[i] = wcsPoints[i].Z;
                wcsPolyLine.Grab_x = wcsPoints[i].Grab_x;
                wcsPolyLine.Grab_y = wcsPoints[i].Grab_y;
                wcsPolyLine.Grab_theta = wcsPoints[i].Grab_theta;
                wcsPolyLine.CamName = wcsPoints[i].CamName;
                wcsPolyLine.CamParams = wcsPoints[i].CamParams;
                wcsPolyLine.ViewWindow = wcsPoints[i].ViewWindow;
            }
            HTuple Qx, Qy;
            if (this.InvertAffineOrientation)
                Qx = coordSystem.GetHomMat2D().HomMat2dInvert().AffineTransPoint2d(x, y, out Qy);
            else
                Qx = coordSystem.GetHomMat2D().AffineTransPoint2d(x, y, out Qy);
            ///////////////////////////////////////////////
            for (int i = 0; i < Qx.Length; i++)
            {
                wcsPolyLine.Add(Qx[i].D, Qy[i].D, z[i]);
            }
            //////////////////////////////////////////////////////////////////
            result = true;
            return result;
        }

        public bool Affine(userWcsPoint[] wcsPoints, userWcsCoordSystem[] coordSystem, out userWcsPolyLine wcsPolyLine)
        {
            bool result = false;
            if (wcsPoints == null)
                throw new ArgumentNullException(nameof(wcsPoints));
            if (coordSystem == null || coordSystem.Length == 0)
            {
                coordSystem = new userWcsCoordSystem[1];
                coordSystem[0] = new userWcsCoordSystem();
            }
            wcsPolyLine = new userWcsPolyLine();
            //////////////////////////////////////////////////////////////////
            double[] x = new double[wcsPoints.Length];
            double[] y = new double[wcsPoints.Length];
            double[] z = new double[wcsPoints.Length];
            for (int i = 0; i < wcsPoints.Length; i++)
            {
                x[i] = wcsPoints[i].X;// + this.Offset_X; // 加上偏移值X
                y[i] = wcsPoints[i].Y;// + this.Offset_Y; // 加上偏移值Y
                z[i] = wcsPoints[i].Z;
                wcsPolyLine.Grab_x = wcsPoints[i].Grab_x;
                wcsPolyLine.Grab_y = wcsPoints[i].Grab_y;
                wcsPolyLine.Grab_theta = wcsPoints[i].Grab_theta;
                wcsPolyLine.CamName = wcsPoints[i].CamParams?.SensorName;
                wcsPolyLine.CamParams = wcsPoints[i].CamParams;
                wcsPolyLine.ViewWindow = wcsPoints[i].ViewWindow;
            }
            ///////////////////// 处理坐标系  /////////////////////////////////
            userWcsVector plateCurVector = null;
            userWcsVector plateTeachVector = null;
            double[] plateCur_x = new double[coordSystem.Length];
            double[] plateCur_y = new double[coordSystem.Length];
            double[] plateCur_angle = new double[coordSystem.Length];
            double[] plateTeach_x = new double[coordSystem.Length];
            double[] plateTeach_y = new double[coordSystem.Length];
            double[] plateTeach_angle = new double[coordSystem.Length];
            for (int i = 0; i < coordSystem.Length; i++)
            {
                plateCurVector = coordSystem[i].CurrentPoint;
                plateTeachVector = coordSystem[i].ReferencePoint.GetPixVector().GetWcsVector(plateCurVector.Grab_x, plateCurVector.Grab_y);
                plateCur_x[i] = plateCurVector.X;
                plateCur_y[i] = plateCurVector.Y;
                plateCur_angle[i] = plateCurVector.Angle;
                plateTeach_x[i] = plateTeachVector.X;
                plateTeach_y[i] = plateTeachVector.Y;
                plateTeach_angle[i] = plateTeachVector.Angle;
            }
            HHomMat2D hHomMat2DAdd = new HHomMat2D();
            switch (plateCur_x.Length)
            {
                case 1:
                    hHomMat2DAdd.VectorAngleToRigid(plateTeach_x[0], plateTeach_y[0], plateTeach_angle[0] * Math.PI / 180, plateCur_x[0], plateCur_y[0], plateCur_angle[0] * Math.PI / 180);
                    //switch (this.Orientation)
                    //{
                    //    default:
                    //    case "示教位到当前位":
                    //        hHomMat2DAdd.VectorAngleToRigid(plateTeach_x[0], plateTeach_y[0], plateTeach_angle[0] * Math.PI / 180, plateCur_x[0], plateCur_y[0], plateCur_angle[0] * Math.PI / 180);
                    //        break;
                    //    case "参考位到当前位":
                    //        hHomMat2DAdd.VectorAngleToRigid(0, 0, 0, plateCur_x[0], plateCur_y[0], plateCur_angle[0] * Math.PI / 180);
                    //        break;
                    //    case "To当前位":
                    //        HHomMat2D homMat2D = new HHomMat2D();
                    //        HHomMat2D homMat2DRotate = homMat2D.HomMat2dRotate((plateCur_angle[0] - plateTeach_angle[0]) * Math.PI / 180, plateTeach_x[0], plateTeach_y[0]);
                    //        hHomMat2DAdd = homMat2DRotate.HomMat2dTranslate(plateCur_x[0] - plateTeach_x[0], plateCur_y[0] - plateTeach_y[0]);
                    //        break;
                    //}
                    break;
                case 2:   // 两个点使用Mark点的中间点来计算 
                    double radCur = Math.Atan2(plateCur_y[1] - plateCur_y[0], plateCur_x[1] - plateCur_x[0]);
                    double radTeach = Math.Atan2(plateTeach_y[1] - plateTeach_y[0], plateTeach_x[1] - plateTeach_x[0]);
                    double meanCur_x = (plateCur_x[0] + plateCur_x[1]) * 0.5;
                    double meanCur_y = (plateCur_y[0] + plateCur_y[1]) * 0.5;
                    double meanTeach_x = (plateTeach_x[0] + plateTeach_x[1]) * 0.5;
                    double meanTeach_y = (plateTeach_y[0] + plateTeach_y[1]) * 0.5;
                    hHomMat2DAdd.VectorAngleToRigid(meanTeach_x, meanTeach_y, radTeach, meanCur_x, meanCur_y, radCur);
                    //switch (this.Orientation)
                    //{
                    //    default:
                    //    case "示教位到当前位":
                    //        hHomMat2DAdd.VectorAngleToRigid(meanTeach_x, meanTeach_y, radTeach, meanCur_x, meanCur_y, radCur);
                    //        break;
                    //    case "参考位到当前位":
                    //        hHomMat2DAdd.VectorAngleToRigid(0, 0, 0, meanCur_x, meanCur_y, radCur);
                    //        break;
                    //    case "To当前位":
                    //        HHomMat2D homMat2D = new HHomMat2D();
                    //        HHomMat2D homMat2DRotate = homMat2D.HomMat2dRotate((radCur - radTeach), meanTeach_x, meanTeach_y);
                    //        hHomMat2DAdd = homMat2DRotate.HomMat2dTranslate(meanCur_x - meanTeach_x, meanCur_y - meanTeach_y);
                    //        break;
                    //}
                    break;
                default: // 定位点大于2个以上，不能指定角度
                    hHomMat2DAdd.VectorToRigid(plateTeach_x, plateTeach_y, plateCur_x, plateCur_y);
                    break;
            }
            /////////////////////////////////////////////////////////////////////
            HTuple Qx, Qy;
            if (this.InvertAffineOrientation)
                Qx = hHomMat2DAdd.HomMat2dInvert().AffineTransPoint2d(x, y, out Qy);
            else
                Qx = hHomMat2DAdd.AffineTransPoint2d(x, y, out Qy);
            ///////////////////////////////////////////////
            for (int i = 0; i < Qx.Length; i++)
            {
                wcsPolyLine.Add(Qx[i].D, Qy[i].D, z[i]);
            }
            //////////////////////////////////////////////////////////////////
            result = true;
            return result;
        }

        public bool AffineTransPoint(userWcsPoint[] wcsPoints,   userWcsCoordSystem coordSystem, string pointOrder, out double[] Qx, out double[] Qy, out double[] Qdeg)
        {
            Qx = new double[0];
            Qy = new double[0];
            Qdeg = new double[0];
            if (wcsPoints == null) throw new ArgumentNullException(nameof(wcsPoints));
            if (coordSystem == null) coordSystem = new userWcsCoordSystem();
            double[] Px = new double[wcsPoints.Length];
            double[] Py = new double[wcsPoints.Length];
            for (int i = 0; i < wcsPoints.Length; i++)
            {
                Px[i] = wcsPoints[i].X;
                Py[i] = wcsPoints[i].Y;
            }
            HHomMat2D hHomMat2D = new HHomMat2D();
            hHomMat2D.VectorAngleToRigid(0.0, 0.0, 0.0, coordSystem.CurrentPoint.X, coordSystem.CurrentPoint.Y, coordSystem.CurrentPoint.Angle * Math.PI / 180);
            HTuple hTuple_x, hTuple_y;
            hTuple_x = hHomMat2D.AffineTransPoint2d(Px, Py, out hTuple_y);
            Qx = hTuple_x.DArr;
            Qy = hTuple_y.DArr;
            ///////////////////////////////////////////////////////////
            HTuple rows, cols;
            HXLDCont hXLDCont = new HXLDCont(new HTuple(Qy) * -1, new HTuple(Qx));
            double offsetDist = 0.02;
            if (pointOrder == "顺时针")
                offsetDist *= -1;
            HXLDCont hXLDCont1 = hXLDCont.GenParallelContourXld("regression_normal", offsetDist);
            hXLDCont1.GetContourXld(out rows, out cols);
            ///////////////////////////////////////////////////////////
            Qdeg = new double[Qx.Length];
            string PointOrder = pointOrder;  //  negative, positive
            for (int i = 0; i < Qx.Length; i++)
            {
                double deg = Math.Atan2(rows[i].D * -1 - Qy[i], cols[i].D - Qx[i]) * 180 / Math.PI;
                ///////////////////////////////
                if (i > 0 && deg - Qdeg[i - 1] < -180) // 逆时针方向，后一个角度一定要大于前一个角
                {
                    PointOrder = "positive";
                    pointOrder = "逆时针";
                }
                if (i > 0 && deg - Qdeg[i - 1] > 180) // 逆时针方向，后一个角度一定要大于前一个角
                {
                    PointOrder = "negative";
                    pointOrder = "顺时针";
                }
                ////////////////
                switch (PointOrder)
                {
                    default:
                        Qdeg[i] = Math.Round(deg, 3);
                        break;
                    case "逆时针":
                    case "positive":
                        if (deg < 0)
                            deg += 360;
                        //////////////////
                        Qdeg[i] = Math.Round(deg, 3);
                        break;
                    case "顺时针":
                    case "negative":
                        if (deg > 0)
                            deg -= 360;
                        //////////////////
                        Qdeg[i] = Math.Round(deg, 3);
                        break;
                }
            }
            //////////// 去除角度的突变 //////////////////////
            for (int i = 1; i < Qdeg.Length; i++)
            {
                /////////////////////////////////////////////////
                if (Math.Abs(Qdeg[i]) < Math.Abs(Qdeg[i - 1]))
                    Qdeg[i] = Qdeg[i - 1];
            }
            return true;
        }

        public bool AffineTransPoint(double[] Px, double[] Py, double cur_x, double cur_y, double cur_angle, double teach_x, double teach_y, double teach_angle, string pointOrder, out double[] Qx, out double[] Qy, out double[] Qdeg)
        {
            Qx = new double[0];
            Qy = new double[0];
            Qdeg = new double[0];
            if (Px == null) throw new ArgumentNullException(nameof(Px));
            if (Py == null) throw new ArgumentNullException(nameof(Py));
            if (Px.Length != Py.Length) throw new ArgumentException("输入数组长度不相等");
            double[] x = new double[Px.Length];
            double[] y = new double[Py.Length];
            for (int i = 0; i < Px.Length; i++)
            {
                x[i] = Px[i] + (cur_x - teach_x);
                y[i] = Py[i] + (cur_y - teach_y);
            }
            double angle = cur_angle - teach_angle;
            HHomMat2D hHomMat2D = new HHomMat2D();
            HHomMat2D hHomMat2DRotate = hHomMat2D.HomMat2dRotate(angle * Math.PI / 180, cur_x, cur_y);
            HTuple hTuple_x, hTuple_y;
            hTuple_x = hHomMat2DRotate.AffineTransPoint2d(x, y, out hTuple_y);
            Qx = hTuple_x.DArr;
            Qy = hTuple_y.DArr;
            ///////////////////////////////////////////////////////////
            HTuple rows, cols;
            HXLDCont hXLDCont = new HXLDCont(new HTuple(Qy) * -1, new HTuple(Qx));
            double offsetDist = 0.02;
            if (pointOrder == "顺时针")
                offsetDist *= -1;
            HXLDCont hXLDCont1 = hXLDCont.GenParallelContourXld("regression_normal", offsetDist);
            hXLDCont1.GetContourXld(out rows, out cols);
            ///////////////////////////////////////////////////////////
            Qdeg = new double[Qx.Length];
            string PointOrder = pointOrder;  //  negative, positive
            for (int i = 0; i < Qx.Length; i++)
            {
                double deg = Math.Atan2(rows[i].D * -1 - Qy[i], cols[i].D - Qx[i]) * 180 / Math.PI;
                ///////////////////////////////
                if (i > 0 && deg - Qdeg[i - 1] < -180) // 逆时针方向，后一个角度一定要大于前一个角
                {
                    PointOrder = "positive";
                    pointOrder = "逆时针";
                }
                if (i > 0 && deg - Qdeg[i - 1] > 180) // 逆时针方向，后一个角度一定要大于前一个角
                {
                    PointOrder = "negative";
                    pointOrder = "顺时针";
                }
                ////////////////
                switch (PointOrder)
                {
                    default:
                        Qdeg[i] = Math.Round(deg, 3);
                        break;
                    case "逆时针":
                    case "positive":
                        if (deg < 0)
                            deg += 360;
                        //////////////////
                        Qdeg[i] = Math.Round(deg, 3);
                        break;
                    case "顺时针":
                    case "negative":
                        if (deg > 0)
                            deg -= 360;
                        //////////////////
                        Qdeg[i] = Math.Round(deg, 3);
                        break;
                }
            }
            //////////// 去除角度的突变 //////////////////////
            for (int i = 1; i < Qdeg.Length; i++)
            {
                /////////////////////////////////////////////////
                if (Math.Abs(Qdeg[i]) < Math.Abs(Qdeg[i - 1]))
                    Qdeg[i] = Qdeg[i - 1];
            }
            return true;
        }


        public bool AffineTransPoint(double[] Px, double[] Py, double x, double y, double angle, string pointOrder, out double[] Qx, out double[] Qy, out double[] Qdeg)
        {
            Qx = new double[0];
            Qy = new double[0];
            Qdeg = new double[0];
            if (Px == null) throw new ArgumentNullException(nameof(Px));
            if (Py == null) throw new ArgumentNullException(nameof(Py));
            if (Px.Length != Py.Length) throw new ArgumentException("输入数组长度不相等");

            HHomMat2D hHomMat2D = new HHomMat2D();
            hHomMat2D.VectorAngleToRigid(0.0, 0.0, 0.0, x, y, angle * Math.PI / 180);
            HTuple hTuple_x, hTuple_y;
            hTuple_x = hHomMat2D.AffineTransPoint2d(Px, Py, out hTuple_y);
            Qx = hTuple_x.DArr;
            Qy = hTuple_y.DArr;
            ///////////////////////////////////////////////////////////
            HTuple rows, cols;
            HXLDCont hXLDCont = new HXLDCont(new HTuple(Qy) * -1, new HTuple(Qx));
            double offsetDist = 0.02;
            if (pointOrder == "顺时针")
                offsetDist *= -1;
            HXLDCont hXLDCont1 = hXLDCont.GenParallelContourXld("regression_normal", offsetDist);
            hXLDCont1.GetContourXld(out rows, out cols);
            ///////////////////////////////////////////////////////////
            Qdeg = new double[Qx.Length];
            string PointOrder = pointOrder;  //  negative, positive
            for (int i = 0; i < Qx.Length; i++)
            {
                double deg = Math.Atan2(rows[i].D * -1 - Qy[i], cols[i].D - Qx[i]) * 180 / Math.PI;
                ///////////////////////////////
                if (i > 0 && deg - Qdeg[i - 1] < -180) // 逆时针方向，后一个角度一定要大于前一个角
                {
                    PointOrder = "positive";
                    pointOrder = "逆时针";
                }
                if (i > 0 && deg - Qdeg[i - 1] > 180) // 逆时针方向，后一个角度一定要大于前一个角
                {
                    PointOrder = "negative";
                    pointOrder = "顺时针";
                }
                ////////////////
                switch (PointOrder)
                {
                    default:
                        Qdeg[i] = Math.Round(deg, 3);
                        break;
                    case "逆时针":
                    case "positive":
                        if (deg < 0)
                            deg += 360;
                        //////////////////
                        Qdeg[i] = Math.Round(deg, 3);
                        break;
                    case "顺时针":
                    case "negative":
                        if (deg > 0)
                            deg -= 360;
                        //////////////////
                        Qdeg[i] = Math.Round(deg, 3);
                        break;
                }
            }
            //////////// 去除角度的突变 //////////////////////
            for (int i = 1; i < Qdeg.Length; i++)
            {
                /////////////////////////////////////////////////
                if (Math.Abs(Qdeg[i]) < Math.Abs(Qdeg[i - 1]))
                    Qdeg[i] = Qdeg[i - 1];
            }
            return true;
        }


    }
}

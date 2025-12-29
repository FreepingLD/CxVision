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
    public class ContourSendParam
    {
        public string ViewWindow { get; set; }
        public enCoordSysName CoordSysName { get; set; }
        public enCommunicationCommand Adress { get; set; }
        public enCommunicationCommand LengthAdress { get; set; }


        public bool EnableCAngleCalCulate { get; set; }

        public double OffsetAngle { get; set; }
        public double InitAngle { get; set; }

        public bool InvertAngle { get; set; }
        public string Orientation { get; set; }

        public double OffsetDist { get; set; }


        public string NormalOrientation { get; set; }
        public string InterMethod { get; set; }
        public double InterParam { get; set; }

        public ContourSendParam()
        {
            this.ViewWindow = "NONE";
            this.CoordSysName = enCoordSysName.CoordSys_0;
            this.Adress = enCommunicationCommand.TrackToPlc;
            this.LengthAdress = enCommunicationCommand.TrackLengthToPlc;
            this.Orientation = "NONE";
            this.InvertAngle = false;
            this.EnableCAngleCalCulate = false;
            this.InitAngle = 0;
            this.OffsetAngle = 0;
            this.OffsetDist = 0.02;
            this.NormalOrientation = "向外";
            this.InterMethod = "NONE";
            this.InterParam = 0.1;
        }


        public bool CalculateNormal(userWcsPoint[] wcsPoint, out double[] angle, out string pointOrder)
        {
            bool result = false;
            angle = new double[0];
            pointOrder = "NONE";
            if (wcsPoint == null)
                throw new ArgumentNullException(nameof(wcsPoint));
            if (wcsPoint.Length == 0) return false;
            //////////////////////////////////////////////////////
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
            HTuple rows, cols;
            HXLDCont hXLDCont = new HXLDCont(new HTuple(list_y.ToArray()) * -1, new HTuple(list_x.ToArray()));
            HXLDCont hXLDCont1 = hXLDCont.GenParallelContourXld("regression_normal", this.OffsetDist);
            hXLDCont1.GetContourXld(out rows, out cols);
            ///////////////////////////////////////////////////////////
            angle = new double[wcsPoint.Length];
            string PointOrder = "none"; //  negative, positive
            List<int> list = new List<int>();
            for (int i = 0; i < wcsPoint.Length; i++)
            {
                double deg = Math.Atan2(rows[i].D * -1 - wcsPoint[i].Y, cols[i].D - wcsPoint[i].X) * 180 / Math.PI;
                ///////////////////////////////
                if (i > 0 && deg - angle[i - 1] < -180) // 逆时针方向，后一个角度一定要大于前一个角
                {
                    PointOrder = "positive";
                    pointOrder = "逆时针";
                }
                if (i > 0 && deg - angle[i - 1] > 180) // 逆时针方向，后一个角度一定要大于前一个角
                {
                    PointOrder = "negative";
                    pointOrder = "顺时针";
                }
                ////////////////
                switch (PointOrder)
                {
                    default:
                        angle[i] = Math.Round(deg, 3);
                        break;
                    case "逆时针":
                    case "positive":
                        deg += 360;
                        //////////////////
                        angle[i] = Math.Round(deg, 3);
                        break;
                    case "顺时针":
                    case "negative":
                        deg -= 360;
                        //////////////////
                        angle[i] = Math.Round(deg, 3);
                        break;
                }
                //////////////////////////////////////////////
                if (i > 0 && deg >= angle[i - 1])
                    list.Add(1);
                else
                    list.Add(-1);
            }
            ////////////  表示没有跳变  ////////
            if (list.Sum() >= 0 && PointOrder != "positive") // 表示轮廓是逆时针
            {
                pointOrder = "逆时针";
                for (int i = 0; i < angle.Length; i++)
                {
                    if (angle[i] < 0)
                        angle[i] += 360;
                }
            }
            ////////////  表示没有跳变   ///////
            if (list.Sum() < 0 && PointOrder != "negative") // 表示轮廓是逆时针
            {
                pointOrder = "顺时针";
                for (int i = 0; i < angle.Length; i++)
                {
                    if (angle[i] > 0)
                        angle[i] -= 360;
                }
            }
            //////////// 去除角度的突变 //////////////////////
            for (int i = 1; i < angle.Length; i++)
            {
                /////////////////////////////////////////////////
                if (Math.Abs(angle[i]) < Math.Abs(angle[i - 1]))
                    angle[i] = angle[i - 1];
            }
            result = true;
            return result;
        }


        public bool CalculateNormal(userWcsPoint[] wcsPoint, string pointOrder, out double[] angle)
        {
            bool result = false;
            angle = new double[0];
            if (wcsPoint == null)
                throw new ArgumentNullException(nameof(wcsPoint));
            if (wcsPoint.Length == 0) return false;
            //////////////////////////////////////////////////////
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
            HTuple rows, cols;
            HXLDCont hXLDCont = new HXLDCont(new HTuple(list_y.ToArray()) * -1, new HTuple(list_x.ToArray()));
            double offsetDist = this.OffsetDist;
            if (pointOrder == "顺时针")
                offsetDist = this.OffsetDist * -1;
            HXLDCont hXLDCont1 = hXLDCont.GenParallelContourXld("regression_normal", offsetDist);
            hXLDCont1.GetContourXld(out rows, out cols);
            ///////////////////////////////////////////////////////////
            angle = new double[wcsPoint.Length];
            string PointOrder = pointOrder;  //  negative, positive
            for (int i = 0; i < wcsPoint.Length; i++)
            {
                double deg = Math.Atan2(rows[i].D * -1 - wcsPoint[i].Y, cols[i].D - wcsPoint[i].X) * 180 / Math.PI;
                ///////////////////////////////
                if (i > 0 && deg - angle[i - 1] < -180) // 逆时针方向，后一个角度一定要大于前一个角
                {
                    PointOrder = "positive";
                    pointOrder = "逆时针";
                }
                if (i > 0 && deg - angle[i - 1] > 180) // 逆时针方向，后一个角度一定要大于前一个角
                {
                    PointOrder = "negative";
                    pointOrder = "顺时针";
                }
                ////////////////
                switch (PointOrder)
                {
                    default:
                        angle[i] = Math.Round(deg, 3);
                        break;
                    case "逆时针":
                    case "positive":
                        if (deg < 0)
                            deg += 360;
                        //////////////////
                        angle[i] = Math.Round(deg, 3);
                        break;
                    case "顺时针":
                    case "negative":
                        if (deg > 0)
                            deg -= 360;
                        //////////////////
                        angle[i] = Math.Round(deg, 3);
                        break;
                }
            }
            //////////// 去除角度的突变 //////////////////////
            for (int i = 1; i < angle.Length; i++)
            {
                /////////////////////////////////////////////////
                if (Math.Abs(angle[i]) < Math.Abs(angle[i - 1]))
                    angle[i] = angle[i - 1];
            }
            ////////////////// 变换角度方向 ////////////////////
            if (pointOrder == "顺时针")
            {
                switch (this.NormalOrientation)
                {
                    case "向内":
                        for (int i = 0; i < angle.Length; i++)
                        {
                            angle[i] = angle[i] + 180;
                        }
                        break;
                    default:
                    case "向外": // 不需要处理

                        break;
                }
            }
            if (pointOrder == "逆时针")
            {
                switch (this.NormalOrientation)
                {
                    case "向内":
                        for (int i = 0; i < angle.Length; i++)
                        {
                            angle[i] = angle[i] - 180;
                        }
                        break;
                    default:
                    case "向外": // 不需要处理
                        break;
                }
            }
            result = true;
            return result;
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
            hHomMat2D.VectorAngleToRigid(0, 0, 0, x, y, angle * Math.PI / 180);
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

        public void LineInterpretationByCount(userWcsPoint [] wcsPoints, int pointNum, out userWcsPoint [] interWcsPoint)
        {
            interWcsPoint = new userWcsPoint[0];
            if (wcsPoints == null)
            {
                interWcsPoint = new userWcsPoint[0];
                return;
            }
            if ( wcsPoints.Length == 1)
            {
                interWcsPoint = new userWcsPoint[1];
                interWcsPoint[0] = wcsPoints[0].Clone();
                return;
            }
            ////////////////
            List<double> list_x = new List<double>();
            List<double> list_y = new List<double>();
            double sumLength = 0; // 计算轮廓的总长度
            for (int i = 0; i < wcsPoints.Length - 1; i++)
            {
                sumLength += Math.Sqrt((wcsPoints[i + 1].X - wcsPoints[i].X) * (wcsPoints[i + 1].X - wcsPoints[i].X) + (wcsPoints[i + 1].Y - wcsPoints[i].Y) * (wcsPoints[i + 1].Y - wcsPoints[i].Y));
            }
            ////// 计算插值 //////////////////
            double step = sumLength / (Math.Abs(pointNum) - 1);
            double phi = 0;
            list_x.Add(wcsPoints[0].X);
            list_y.Add(wcsPoints[0].Y);
            double start_x = wcsPoints[0].X;
            double start_y = wcsPoints[0].Y;
            sumLength = 0;
            for (int i = 1; i < wcsPoints.Length; i++)
            {
                sumLength += Math.Sqrt((wcsPoints[i].X - start_x) * (wcsPoints[i].X - start_x) + (wcsPoints[i].Y - start_y) * (wcsPoints[i].Y - start_y));
                if (sumLength <= step)
                {
                    if (i < wcsPoints.Length - 1)
                    {
                        start_x = wcsPoints[i].X;
                        start_y = wcsPoints[i].Y;
                        continue;
                    }
                    else  // 执行到最后一个点
                    {
                        list_x.Add(wcsPoints[i].X);
                        list_y.Add(wcsPoints[i].Y);
                        break;
                    }
                }
                else
                {
                    phi = Math.Atan2(wcsPoints[i].Y - wcsPoints[i - 1].Y, wcsPoints[i].X - wcsPoints[i - 1].X);
                    list_x.Add(wcsPoints[i].X - (sumLength - step) * Math.Cos(phi));
                    list_y.Add(wcsPoints[i].Y - (sumLength - step) * Math.Sin(phi));
                    start_x = list_x.Last();
                    start_y = list_y.Last();
                    sumLength = 0;
                    if (i < wcsPoints.Length) // 当这个条件不满足时，表示已遍历到了最后一个点
                        i--;
                }
            }
            /////////////////////////////////////
            interWcsPoint = new userWcsPoint[list_x.Count];
            for (int i = 0; i < list_x.Count; i++)
            {
                interWcsPoint[i] = new userWcsPoint(list_x[i], list_y[i],0, wcsPoints[0].CamParams);
                interWcsPoint[i].ViewWindow = wcsPoints[0].ViewWindow;
                interWcsPoint[i].CamName = wcsPoints[0].CamName;
            }
        }
        public void LineInterpretationByStep(userWcsPoint[] wcsPoints, double step, out userWcsPoint[] interWcsPoint)
        {
            interWcsPoint = new userWcsPoint[0];
            if (wcsPoints == null)
            {
                interWcsPoint = new userWcsPoint[0];
                return;
            }
            if (wcsPoints.Length == 1)
            {
                interWcsPoint = new userWcsPoint[1];
                interWcsPoint[0] = wcsPoints[0].Clone();
                return;
            }
            ////////////////
            List<double> list_x = new List<double>();
            List<double> list_y = new List<double>();
            ////// 计算插值 //////////////////
            double phi = 0;
            list_x.Add(wcsPoints[0].X);
            list_y.Add(wcsPoints[0].Y);
            double start_x = wcsPoints[0].X;
            double start_y = wcsPoints[0].Y;
            double sumLength = 0;
            for (int i = 1; i < wcsPoints.Length; i++)
            {
                sumLength += Math.Sqrt((wcsPoints[i].X - start_x) * (wcsPoints[i].X - start_x) + (wcsPoints[i].Y - start_y) * (wcsPoints[i].Y - start_y));
                if (sumLength <= Math.Abs(step))
                {
                    if (i < wcsPoints.Length - 1)
                    {
                        start_x = wcsPoints[i].X;
                        start_y = wcsPoints[i].Y;
                        continue;
                    }
                    else  // 执行到最后一个点
                    {
                        list_x.Add(wcsPoints[i].X);
                        list_y.Add(wcsPoints[i].Y);
                        break;
                    }
                }
                else
                {
                    phi = Math.Atan2(wcsPoints[i].Y - wcsPoints[i - 1].Y, wcsPoints[i].X - wcsPoints[i - 1].X);
                    list_x.Add(wcsPoints[i].X - (sumLength - Math.Abs(step)) * Math.Cos(phi));
                    list_y.Add(wcsPoints[i].Y - (sumLength - Math.Abs(step)) * Math.Sin(phi));
                    start_x = list_x.Last();
                    start_y = list_y.Last();
                    sumLength = 0;
                    if (i < wcsPoints.Length) // 当这个条件不满足时，表示已遍历到了最后一个点
                        i--;
                }
            }
            /////////////////////////////////////
            interWcsPoint = new userWcsPoint[list_x.Count];
            for (int i = 0; i < list_x.Count; i++)
            {
                interWcsPoint[i] = new userWcsPoint(list_x[i], list_y[i], 0, wcsPoints[0].CamParams);
                interWcsPoint[i].ViewWindow = wcsPoints[0].ViewWindow;
                interWcsPoint[i].CamName = wcsPoints[0].CamName;
            }
        }



    }
}

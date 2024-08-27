using Common;
using HalconDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace FunctionBlock
{
    public class TrackCalculateMethod
    {
        public static bool CalculateTrack(userWcsPoint[] wcsPoint, TrackCalculateParam param, out userWcsVector[] wcsVector, out HXLDCont hXLDContArrow)
        {
            bool result = false;
            wcsVector = new userWcsVector[0];
            hXLDContArrow = new HXLDCont();
            if (wcsPoint == null)
                throw new ArgumentNullException("wcsPoint");
            List<double> list_x = new List<double>();
            List<double> list_y = new List<double>();
            List<double> list_z = new List<double>();
            ///////////////////////////////////////////////////////  排序轨迹    ///////////
            userWcsPoint[] wcsPointSort;
            SortTrackPoint(wcsPoint, param.SortMethod, out wcsPointSort);
            foreach (var item in wcsPointSort)
            {
                list_x.Add(item.X);
                list_y.Add(item.Y);
                list_z.Add(item.Z);
            }
            //// 启用插值
            if (param.IsInterpretation)
            {
                List<double> list_x2 = new List<double>();
                List<double> list_y2 = new List<double>();
                List<double> list_z2 = new List<double>();
                double k = 0, b = 0, y = 0;
                int index = 0;
                for (int i = 0; i < list_x.Count - 1; i++)
                {
                    index = 0;
                    if (list_x[i + 1] < list_x[i])
                        param.InterpretationDist = Math.Abs(param.InterpretationDist) * -1; // 如果终点小于起点，那么递增值要小于 0 ，否则要大于0
                    else
                        param.InterpretationDist = Math.Abs(param.InterpretationDist) * 1;
                    //////////////////////////////////////////////////////////////////////
                    if (list_x[i] != list_x[i + 1])
                    {
                        k = (list_y[i + 1] - list_y[i]) / (list_x[i + 1] - list_x[i]);
                        b = list_y[i] - k * list_x[i];
                        while (true)
                        {
                            if (Math.Abs(list_x[i] + index * param.InterpretationDist) > Math.Abs(list_x[i + 1])) break;
                            y = k * (list_x[i] + index * param.InterpretationDist) + b;
                            list_x2.Add(list_x[i] + index * param.InterpretationDist);
                            list_y2.Add(y);
                            list_z2.Add(list_z[i]);
                            index++;
                        }
                    }
                    else
                    {
                        while (true)
                        {
                            if (Math.Abs(list_y[i] + index * param.InterpretationDist) > Math.Abs(list_y[i + 1])) break;
                            y = list_y[i] + index * param.InterpretationDist;
                            list_x2.Add(list_x[i]);
                            list_y2.Add(y);
                            list_z2.Add(list_z[i]);
                            index++;
                        }
                    }
                }
                /////////////////////////////////////////
                list_x2.Add(list_x.Last());
                list_y2.Add(list_y.Last());
                list_z2.Add(list_z.Last());
                /////
                list_x.Clear();
                list_y.Clear();
                list_z.Clear();
                list_x.AddRange(list_x2.ToArray());
                list_y.AddRange(list_y2.ToArray());
                list_z.AddRange(list_z2.ToArray());
            }
            ///
            HTuple normal_x, normal_y;
            HXLDCont hXLDCont = new HXLDCont(list_y.ToArray(), list_x.ToArray());
            HXLDCont hXLDContParallel = hXLDCont.GenParallelContourXld(param.Mode, param.Distance);
            hXLDContParallel.GetContourXld(out normal_y, out normal_x);
            ///////////////////////////////////////////////////////////
            double deg, phi, Qx, Qy, Qz, angle_x, angle_y, angle_z;
            List<double> list_angle = new List<double>();
            HHomMat3D hHomMat3D = new HHomMat3D();
            HHomMat3D hHomMat3D_z, hHomMat3D_y, hHomMat3D_x, hHomMat3D_t;
            CameraParam camera = wcsPoint[0].CamParams;
            wcsVector = new userWcsVector[list_x.Count];
            RobotJawParam jawParam = RobotJawParaManager.Instance.GetJawParam(param.Jaw);
            for (int i = 0; i < list_x.Count; i++)
            {
                phi = Math.Atan2(normal_y[i].D - list_y[i], normal_x[i].D - list_x[i]);
                if (phi < 0)
                    phi = (phi + Math.PI * 2); // 角度逆时针来排列
                deg = phi * 180 / Math.PI;
                if (param.IsShowNormalCont) // 判断是否显示法向箭头轮廓
                {
                    if (hXLDContArrow.IsInitialized())
                        hXLDContArrow = GenArrowContourXld(list_y[i], list_x[i], normal_y[i].D, normal_x[i].D, 10, 10);
                    else
                        hXLDContArrow = hXLDContArrow.ConcatObj(GenArrowContourXld(list_y[i], list_x[i], normal_y[i].D, normal_x[i].D, 10, 10));
                }
                if (param.RotateAngle_z == "auto")
                {
                    angle_z = deg;
                    if (camera.CaliParam.CalibCenterXy == null) camera.CaliParam.CalibCenterXy = new userWcsPoint();
                    hHomMat3D_z = hHomMat3D.HomMat3dRotate(phi, "z", camera.CaliParam.CalibCenterXy.X, camera.CaliParam.CalibCenterXy.Y, camera.CaliParam.CalibCenterXy.Z);
                }
                else
                {
                    double.TryParse(param.RotateAngle_z, out angle_z);
                    if (camera.CaliParam.CalibCenterXy == null) camera.CaliParam.CalibCenterXy = new userWcsPoint();
                    hHomMat3D_z = hHomMat3D.HomMat3dRotate(angle_z * Math.PI / 180, "z", camera.CaliParam.CalibCenterXy.X, camera.CaliParam.CalibCenterXy.Y, camera.CaliParam.CalibCenterXy.Z);
                }
                if (param.RotateAngle_y == "auto")
                {
                    angle_y = 45;
                    if (camera.CaliParam.CalibCenterXz == null) camera.CaliParam.CalibCenterXz = new userWcsPoint();
                    hHomMat3D_y = hHomMat3D_z.HomMat3dRotate(45 * Math.PI / 180, "y", camera.CaliParam.CalibCenterXz.X, camera.CaliParam.CalibCenterXz.Y, camera.CaliParam.CalibCenterXz.Z);
                }
                else
                {
                    double.TryParse(param.RotateAngle_y, out angle_y);
                    if (camera.CaliParam.CalibCenterXz == null) camera.CaliParam.CalibCenterXz = new userWcsPoint();
                    hHomMat3D_y = hHomMat3D_z.HomMat3dRotate(angle_y * Math.PI / 180, "y", camera.CaliParam.CalibCenterXz.X, camera.CaliParam.CalibCenterXz.Y, camera.CaliParam.CalibCenterXz.Z);

                }
                if (param.RotateAngle_x == "auto")
                {
                    angle_x = 45;
                    if (camera.CaliParam.CalibCenterYz == null) camera.CaliParam.CalibCenterYz = new userWcsPoint();
                    hHomMat3D_x = hHomMat3D_y.HomMat3dRotate(45 * Math.PI / 180, "x", camera.CaliParam.CalibCenterYz.X, camera.CaliParam.CalibCenterYz.Y, camera.CaliParam.CalibCenterYz.Z);
                }
                else
                {
                    double.TryParse(param.RotateAngle_y, out angle_x);
                    if (camera.CaliParam.CalibCenterYz == null) camera.CaliParam.CalibCenterYz = new userWcsPoint();
                    hHomMat3D_x = hHomMat3D_y.HomMat3dRotate(angle_x * Math.PI / 180, "x", camera.CaliParam.CalibCenterYz.X, camera.CaliParam.CalibCenterYz.Y, camera.CaliParam.CalibCenterYz.Z);
                }
                hHomMat3D_t = hHomMat3D_x.HomMat3dTranslateLocal(jawParam.X * -1, jawParam.Y * -1, jawParam.Z * -1); // 平移
                Qx = hHomMat3D_t.AffineTransPoint3d(list_x[i], list_y[i], list_z[i], out Qy, out Qz);
                wcsVector[i] = new userWcsVector(Qx, Qy, Qz, angle_x, angle_y, angle_z, camera);
            }
            result = true;
            return result;

        }

        /// <summary>
        /// 生成法向箭头
        /// </summary>
        /// <param name="Row1"></param>
        /// <param name="Column1"></param>
        /// <param name="Row2"></param>
        /// <param name="Column2"></param>
        /// <param name="HeadLength"></param>
        /// <param name="HeadWidth"></param>
        /// <returns></returns>
        public static HXLDCont GenArrowContourXld(HTuple Row1, HTuple Column1, HTuple Row2, HTuple Column2, double HeadLength, double HeadWidth)
        {
            if (Row1.Length != Row2.Length) return new HXLDCont();
            HXLDCont arrows = new HXLDCont();
            arrows.GenEmptyObj();
            HTuple Length = HMisc.DistancePp(Row1, Column1, Row2, Column2);
            HTuple ZeroLengthIndices = Length.TupleFind(0);
            if (ZeroLengthIndices != -1)
                Length[ZeroLengthIndices] = -1;
            // Calculate auxiliary variables.
            HTuple DR = 1.0 * (Row2 - Row1) / Length;
            HTuple DC = 1.0 * (Column2 - Column1) / Length;
            HTuple HalfHeadWidth = HeadWidth / 2.0;
            // Calculate end points of the arrow head.
            HTuple RowP1 = Row1 + (Length - HeadLength) * DR + HalfHeadWidth * DC;
            HTuple ColP1 = Column1 + (Length - HeadLength) * DC - HalfHeadWidth * DR;
            HTuple RowP2 = Row1 + (Length - HeadLength) * DR - HalfHeadWidth * DC;
            HTuple ColP2 = Column1 + (Length - HeadLength) * DC + HalfHeadWidth * DR;
            // Finally create output XLD contour for each input point pair
            for (int Index = 0; Index < Length.Length; Index++)
            {
                if (Length[Index].D == -1)
                    arrows = arrows.ConcatObj(new HXLDCont(Row1[Index], Column1[Index]));
                else
                    arrows = arrows.ConcatObj(new HXLDCont(new HTuple(Row1[Index].D, Row2[Index].D, RowP1[Index].D, Row2[Index].D, RowP2[Index].D, Row2[Index].D), new HTuple(Column1[Index].D, Column2[Index].D, ColP1[Index].D, Column2[Index].D, ColP2[Index].D, Column2[Index].D)));
            }
            return arrows;
        }

        /// <summary>
        /// 排序轮廓点
        /// </summary>
        /// <param name="wcsPoints"></param>
        /// <param name="sortMethod"></param>
        /// <param name="wcsPointSort"></param>
        public static void SortTrackPoint(userWcsPoint[] wcsPoints, enSortPoint sortMethod, out userWcsPoint[] wcsPointSort)
        {
            wcsPointSort = new userWcsPoint[0];
            if (wcsPoints == null)
                throw new ArgumentNullException("wcsPoints");
            /////////////////////////////////////////////////////////
            double[] dist1 = new double[wcsPoints.Length];
            double[] dist2 = new double[wcsPoints.Length];
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
            /////////////////////////////////////
            Dictionary<double, userWcsPoint> dic = new Dictionary<double, userWcsPoint>();
            Dictionary<double, userWcsPoint> dic1 = new Dictionary<double, userWcsPoint>();
            Dictionary<double, userWcsPoint> dic2 = new Dictionary<double, userWcsPoint>();
            switch (sortMethod)
            {
                case enSortPoint.角度降序:
                case enSortPoint.角度升序:
                    for (int i = 0; i < wcsPoints.Length; i++)
                    {
                        double rad = Math.Atan2(wcsPoints[i].Y - mean_y, wcsPoints[i].X - mean_x);
                        if (rad < 0)
                            phi[i] = rad + Math.PI * 2;
                        else
                            phi[i] = rad;
                        dic.Add(phi[i], wcsPoints[i]);
                    }
                    break;
                case enSortPoint.距离升序: // 用于直线排序
                case enSortPoint.距离降序:
                    for (int i = 0; i < wcsPoints.Length; i++)
                    {
                        // 以中点为参考点来计算角度，角度按方向分类
                        double rad = Math.Atan2(wcsPoints[i].Y - mean_y, wcsPoints[i].X - mean_x);
                        phi[i] = rad;
                        dic.Add(phi[i], wcsPoints[i]);
                    }
                    /// 将直线上的点从中间分成两段
                    List<userWcsPoint> listPoint1 = new List<userWcsPoint>();
                    List<userWcsPoint> listPoint2 = new List<userWcsPoint>();
                    for (int i = 0; i < phi.Length; i++)
                    {
                        if (Math.Abs(phi[i] - phi[0]) < 0.5)
                            listPoint1.Add(dic[phi[i]]);
                        else
                            listPoint2.Add(dic[phi[i]]);
                    }
                    // 计算线段1区到直线中点的距离
                    dist1 = new double[listPoint1.Count];
                    for (int i = 0; i < listPoint1.Count; i++)
                    {
                        dist1[i] = HMisc.DistancePp(listPoint1[i].X, listPoint1[i].Y, mean_x, mean_y);
                        dic1.Add(dist1[i], listPoint1[i]);
                    }
                    // 计算线段2区到直线中点的距离
                    dist2 = new double[listPoint2.Count];
                    for (int i = 0; i < listPoint2.Count; i++)
                    {
                        dist2[i] = HMisc.DistancePp(listPoint2[i].X, listPoint2[i].Y, mean_x, mean_y);
                        dic2.Add(dist2[i], listPoint2[i]);
                    }
                    break;
            }
            /////////////////////////////////////////////
            List<userWcsPoint> listSortPoint = new List<userWcsPoint>();
            //wcsPointSort = new userWcsPoint[dist1.Length + dist2.Length];
            switch (sortMethod)
            {
                case enSortPoint.NONE:
                    for (int i = 0; i < wcsPoints.Length; i++)
                    {
                        listSortPoint.Add( new userWcsPoint(wcsPoints[i].X, wcsPoints[i].Y, wcsPoints[i].Z, wcsPoints[i].CamParams));
                    }
                    break;
                case enSortPoint.角度降序:
                    Array.Sort(phi);
                    for (int i = phi.Length - 1; i > 0; i--)
                    {
                        listSortPoint.Add(new userWcsPoint(dic[phi[i]].X, dic[phi[i]].Y, dic[phi[i]].Z, dic[phi[i]].CamParams));
                    }
                    break;
                case enSortPoint.角度升序:
                    Array.Sort(phi);
                    for (int i = 0; i < phi.Length; i++)
                    {
                        listSortPoint.Add(new userWcsPoint(dic[phi[i]].X, dic[phi[i]].Y, dic[phi[i]].Z, dic[phi[i]].CamParams));
                    }
                    break;
                case enSortPoint.距离升序:
                    Array.Sort(dist1); // 
                    Array.Sort(dist2);
                    for (int i = dist2.Length - 1; i >= 0; i--)
                    {
                        listSortPoint.Add(new userWcsPoint(dic2[dist2[i]].X, dic2[dist2[i]].Y, dic2[dist2[i]].Z, dic2[dist2[i]].CamParams));
                    }
                    for (int i = 0; i < dist1.Length; i++)
                    {
                        listSortPoint.Add(new userWcsPoint(dic1[dist1[i]].X, dic1[dist1[i]].Y, dic1[dist1[i]].Z, dic1[dist1[i]].CamParams));
                    }
                    break;
                case enSortPoint.距离降序:
                    Array.Sort(dist1); // 
                    Array.Sort(dist2);
                    for (int i = dist1.Length - 1; i >= 0; i--)
                    {
                        listSortPoint.Add(new userWcsPoint(dic1[dist1[i]].X, dic1[dist1[i]].Y, dic1[dist1[i]].Z, dic1[dist1[i]].CamParams));
                    }
                    for (int i = 0; i < dist2.Length; i++)
                    {
                        listSortPoint.Add(new userWcsPoint(dic2[dist2[i]].X, dic2[dist2[i]].Y, dic2[dist2[i]].Z, dic2[dist2[i]].CamParams));
                    }
                    break;
            }
            ////
            wcsPointSort = listSortPoint.ToArray();
            listSortPoint.Clear();
        }


    }

}

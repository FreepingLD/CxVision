using Common;
using HalconDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.ComponentModel;

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
                throw new ArgumentNullException(nameof(wcsPoint));
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
            //HXLDCont hXLDCont = new HXLDCont(new HTuple(list_y.ToArray()), new HTuple(list_x.ToArray()));
            //HXLDCont hXLDContParallel = hXLDCont.GenParallelContourXld(param.Mode, param.Distance);
            //hXLDContParallel.GetContourXld(out normal_y, out normal_x);

            double[] norma_x, norma_y;
            CalculateContNormal(list_x.ToArray(), list_y.ToArray(), param.Distance, out norma_x, out norma_y);
            normal_x = new HTuple(norma_x);
            normal_y = new HTuple(norma_y);
            //normal_y *= -1;
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
                phi = Math.Atan2((normal_y[i].D - list_y[i]), normal_x[i].D - list_x[i]);
                //if (phi < -1 * Math.PI / 180)
                //    phi = (phi + Math.PI * 2);  // 角度逆时针来排列
                deg = phi * 180 / Math.PI;
                if (param.IsShowNormalCont)     // 判断是否显示法向箭头轮廓
                {
                    if (!hXLDContArrow.IsInitialized())
                    {
                        userPixPoint pixPoint1 = wcsPointSort[i].GetPixPoint();
                        userPixPoint pixPoint2 = new userWcsPoint(normal_x[i].D, normal_y[i].D, 0, wcsPointSort[i].CamParams).GetPixPoint();
                        hXLDContArrow = GenArrowContourXld(pixPoint1.Row, pixPoint1.Col, pixPoint2.Row, pixPoint2.Col, 0.1, 0.1);
                    }
                    else
                    {
                        userPixPoint pixPoint1 = wcsPointSort[i].GetPixPoint();
                        userPixPoint pixPoint2 = new userWcsPoint(normal_x[i].D, normal_y[i].D, 0, wcsPointSort[i].CamParams).GetPixPoint();
                        hXLDContArrow = hXLDContArrow.ConcatObj(GenArrowContourXld(pixPoint1.Row, pixPoint1.Col, pixPoint2.Row, pixPoint2.Col, 0.1, 0.1));
                    }
                }
                if (param.RotateAngle_z == "auto")
                {
                    angle_z = deg;
                    if (camera == null) camera = new CameraParam();
                    if (camera.CaliParam.CalibCenterXy == null) camera.CaliParam.CalibCenterXy = new userWcsPoint();
                    hHomMat3D_z = hHomMat3D.HomMat3dRotate(phi, "z", camera.CaliParam.CalibCenterXy.X, camera.CaliParam.CalibCenterXy.Y, camera.CaliParam.CalibCenterXy.Z);
                }
                else
                {
                    double.TryParse(param.RotateAngle_z, out angle_z);
                    if (camera == null) camera = new CameraParam();
                    if (camera?.CaliParam?.CalibCenterXy == null) camera.CaliParam.CalibCenterXy = new userWcsPoint();
                    hHomMat3D_z = hHomMat3D.HomMat3dRotate(angle_z * Math.PI / 180, "z", camera.CaliParam.CalibCenterXy.X, camera.CaliParam.CalibCenterXy.Y, camera.CaliParam.CalibCenterXy.Z);
                }
                if (param.RotateAngle_y == "auto")
                {
                    angle_y = 45;
                    if (camera == null) camera = new CameraParam();
                    if (camera?.CaliParam?.CalibCenterXz == null) camera.CaliParam.CalibCenterXz = new userWcsPoint();
                    hHomMat3D_y = hHomMat3D_z.HomMat3dRotate(45 * Math.PI / 180, "y", camera.CaliParam.CalibCenterXz.X, camera.CaliParam.CalibCenterXz.Y, camera.CaliParam.CalibCenterXz.Z);
                }
                else
                {
                    double.TryParse(param.RotateAngle_y, out angle_y);
                    if (camera == null) camera = new CameraParam();
                    if (camera.CaliParam.CalibCenterXz == null) camera.CaliParam.CalibCenterXz = new userWcsPoint();
                    hHomMat3D_y = hHomMat3D_z.HomMat3dRotate(angle_y * Math.PI / 180, "y", camera.CaliParam.CalibCenterXz.X, camera.CaliParam.CalibCenterXz.Y, camera.CaliParam.CalibCenterXz.Z);

                }
                if (param.RotateAngle_x == "auto")
                {
                    angle_x = 45;
                    if (camera == null) camera = new CameraParam();
                    if (camera?.CaliParam?.CalibCenterYz == null) camera.CaliParam.CalibCenterYz = new userWcsPoint();
                    hHomMat3D_x = hHomMat3D_y.HomMat3dRotate(45 * Math.PI / 180, "x", camera.CaliParam.CalibCenterYz.X, camera.CaliParam.CalibCenterYz.Y, camera.CaliParam.CalibCenterYz.Z);
                }
                else
                {
                    double.TryParse(param.RotateAngle_y, out angle_x);
                    if (camera == null) camera = new CameraParam();
                    if (camera?.CaliParam?.CalibCenterYz == null) camera.CaliParam.CalibCenterYz = new userWcsPoint();
                    hHomMat3D_x = hHomMat3D_y.HomMat3dRotate(angle_x * Math.PI / 180, "x", camera.CaliParam.CalibCenterYz.X, camera.CaliParam.CalibCenterYz.Y, camera.CaliParam.CalibCenterYz.Z);
                }
                hHomMat3D_t = hHomMat3D_x.HomMat3dTranslateLocal(jawParam.X * -1, jawParam.Y * -1, jawParam.Z * -1); // 平移
                Qx = hHomMat3D_t.AffineTransPoint3d(list_x[i], list_y[i], list_z[i], out Qy, out Qz);
                wcsVector[i] = new userWcsVector(list_x[i], list_y[i], list_z[i], angle_x, angle_y, angle_z, wcsPointSort[i].CamParams);
                //wcsVector[i] = new userWcsVector(Qx, Qy, Qz, angle_x, angle_y, angle_z, wcsPointSort[i].CamParams);
            }
            result = true;
            return result;

        }

        public static bool ExtractTrack(userWcsPoint[] wcsPoints, userWcsCoordSystem wcsCoordSystem, BindingList<ContourExtractParam> param, out userWcsPolyLine wcsPolyLine)
        {
            bool result = false;
            wcsPolyLine = new userWcsPolyLine();
            if (wcsPoints == null) throw new ArgumentNullException(nameof(wcsPoints));
            if (param == null) throw new ArgumentNullException(nameof(param));
            if (wcsCoordSystem == null) wcsCoordSystem = new userWcsCoordSystem();
            //userWcsPoint[] wcsPointSort;
            //SortTrackPoint(wcsPoints, enSortPoint.角度升序, out wcsPointSort); // 先对轨迹排序
            List<double> list_x = new List<double>();
            List<double> list_y = new List<double>();
            List<double> list_z = new List<double>();
            foreach (var item in wcsPoints)
            {
                list_x.Add(item.X);
                list_y.Add(item.Y);
                list_z.Add(0);
            }
            PointCloudData PointCloudModel3D = new PointCloudData(new HObjectModel3D(list_x.ToArray(), list_y.ToArray(), list_z.ToArray()));
            //wcsPolyLine.X.AddRange(list_x);
            //wcsPolyLine.Y.AddRange(list_y);
            //wcsPolyLine.Z.AddRange(list_z);
            if (wcsPolyLine.CamParams == null)
                wcsPolyLine.CamParams = new CameraParam();
            wcsPolyLine.CamParams.CamParam = PointCloudModel3D.LaserParams.CamParam.Clone();
            wcsPolyLine.CamParams.CamPose = PointCloudModel3D.LaserParams.CamPose.Clone();
            wcsPolyLine.CamParams.CaliParam.CamCaliModel = enCamCaliModel.CamParamPose;
            PointCloudModel3D.Dispose();
            List<double> select_x = new List<double>();
            List<double> select_y = new List<double>();
            //foreach (var item in param)
            //{
            //    select_x.Clear();
            //    select_y.Clear();
            //    switch (item.RoiShape.GetType().Name)
            //    {
            //        case nameof(drawWcsCircle):
            //            drawWcsCircle wcsCircle = item.RoiShape as drawWcsCircle;
            //            //drawWcsCircle afineWcsCircle = wcsCircle.AffineTransWcsCircle(wcsCoordSystem.GetVariationHomMat2D());
            //            drawWcsCircle afineWcsCircle = wcsCircle.AffineWcsROI(wcsCoordSystem.GetVariationHomMat2DNew()) as drawWcsCircle;
            //            for (int i = 0; i < list_x.Count; i++)
            //            {
            //                if (Math.Sqrt((list_x[i] - afineWcsCircle.X) * (list_x[i] - afineWcsCircle.X) + (list_y[i] - afineWcsCircle.Y) * (list_y[i] - afineWcsCircle.Y)) <= afineWcsCircle.Radius)
            //                {
            //                    select_x.Add(list_x[i]);
            //                    select_y.Add(list_y[i]);
            //                }
            //            }
            //            break;
            //        default:
            //            break;
            //    }
            //    //////////////
            //    if (select_x.Count > 0)
            //    {
            //        wcsPolyLine.Add(select_x.Average(), select_y.Average());
            //    }
            //}
            //////////////////////////////////////////////////////////////////////////
            result = true;
            return result;
        }

        public static bool OffsetTrack(userWcsPoint[] wcsPoint, TrackOffsetParam param, out userWcsPolyLine wcsPolyLine)
        {
            bool result = false;
            wcsPolyLine = new userWcsPolyLine();
            if (wcsPoint == null)
                throw new ArgumentNullException(nameof(wcsPoint));
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
            /////////////////////////
            HTuple normal_x, normal_y;
            //HXLDCont hXLDCont = new HXLDCont(new HTuple(list_y.ToArray()), new HTuple(list_x.ToArray()));
            //HXLDCont hXLDContParallel = hXLDCont.GenParallelContourXld("regression_normal", param.Distance);
            //hXLDContParallel.GetContourXld(out normal_y, out normal_x);
            //normal_y *= -1;
            double[] norma_x, norma_y;
            CalculateContNormal(list_x.ToArray(), list_y.ToArray(), param.Distance, out norma_x, out norma_y);
            normal_x = new HTuple(norma_x);
            normal_y = new HTuple(norma_y);
            ///////////////////////////////////////////////////////////
            for (int i = 0; i < normal_x.Length; i++)
            {
                wcsPolyLine.Add(normal_x[i].D, normal_y[i].D);
            }
            if (wcsPolyLine.CamParams == null)
            {
                PointCloudData pointCloudModel3D = new PointCloudData(new HObjectModel3D(list_x.ToArray(), list_y.ToArray(), list_z.ToArray()));
                wcsPolyLine.CamParams = new CameraParam();
                wcsPolyLine.CamParams.CamParam = pointCloudModel3D.LaserParams.CamParam.Clone();
                wcsPolyLine.CamParams.CamPose = pointCloudModel3D.LaserParams.CamPose.Clone();
                wcsPolyLine.CamParams.CaliParam.CamCaliModel = enCamCaliModel.CamParamPose;
                pointCloudModel3D.Dispose();
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
                        listSortPoint.Add(new userWcsPoint(wcsPoints[i].X, wcsPoints[i].Y, wcsPoints[i].Z, wcsPoints[i].CamParams));
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


        public static void CalculateContNormal(double[] x, double[] y, double normalLength, out double[] normal_x, out double[] normal_y)
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


        public static void CalculateContNormal(double[] rows, double[] cols, double normalLength, out double[] normal_angle)
        {
            normal_angle = new double[rows.Length];
            double rad1 = 0;
            double rad2 = 0;
            HHomMat2D hHomMat2D = new HHomMat2D();
            HHomMat2D hHomMat2D1;
            double sx, sy, phi, theta, tx, ty;
            for (int i = 0; i < rows.Length; i++)
            {
                if (i == 0)
                {
                    rad1 = Math.Atan2(cols[i + 1] - cols[i], rows[i + 1] - rows[i]); //法向角 + Math.PI * 0.5
                    hHomMat2D.VectorAngleToRigid(0, 0, 0, 0, 0, rad1);
                    hHomMat2D1 = hHomMat2D.HomMat2dRotateLocal(Math.PI * 0.5);
                    sx = hHomMat2D1.HomMat2dToAffinePar(out sy, out phi, out theta, out tx, out ty);
                    /////////////////////////
                    if (phi < 0)
                        phi += Math.PI * 2;
                    normal_angle[i] = phi;
                }
                else
                {
                    if (rows.Length - 1 == i)
                    {
                        rad1 = Math.Atan2(cols[i] - cols[i - 1], rows[i] - rows[i - 1]); //法向角 + Math.PI * 0.5
                        hHomMat2D.VectorAngleToRigid(0, 0, 0, 0, 0, rad1);
                        hHomMat2D1 = hHomMat2D.HomMat2dRotateLocal(Math.PI * 0.5);
                        sx = hHomMat2D1.HomMat2dToAffinePar(out sy, out phi, out theta, out tx, out ty);
                        //////////////////////////
                        if (phi < 0)
                            phi += Math.PI * 2;
                        normal_angle[i] = phi;
                    }
                    else
                    {
                        rad1 = Math.Atan2(cols[i] - cols[i - 1], rows[i] - rows[i - 1]); //法向角 + Math.PI * 0.5
                        rad2 = Math.Atan2(cols[i + 1] - cols[i], rows[i + 1] - rows[i]); //法向角
                        hHomMat2D.VectorAngleToRigid(0, 0, 0, 0, 0, rad1);
                        hHomMat2D1 = hHomMat2D.HomMat2dRotateLocal(Math.PI * 0.5);
                        sx = hHomMat2D1.HomMat2dToAffinePar(out sy, out rad1, out theta, out tx, out ty);
                        ///////////////////////
                        hHomMat2D.VectorAngleToRigid(0, 0, 0, 0, 0, rad2);
                        hHomMat2D1 = hHomMat2D.HomMat2dRotateLocal(Math.PI * 0.5);
                        sx = hHomMat2D1.HomMat2dToAffinePar(out sy, out rad2, out theta, out tx, out ty);
                        hHomMat2D.VectorAngleToRigid(0, 0, rad2, 0, 0, rad1); // 计算 rad2 到 rad1 的角度范围
                        sx = hHomMat2D.HomMat2dToAffinePar(out sy, out phi, out theta, out tx, out ty);
                        hHomMat2D.VectorAngleToRigid(0, 0, 0, 0, 0, rad2);
                        hHomMat2D1 = hHomMat2D.HomMat2dRotateLocal(phi * 0.5); // 再将角度变化 角度范围的一半
                        sx = hHomMat2D1.HomMat2dToAffinePar(out sy, out phi, out theta, out tx, out ty);
                        ////////////////////
                        normal_angle[i] = phi;
                    }
                }
            }

        }


    }

}

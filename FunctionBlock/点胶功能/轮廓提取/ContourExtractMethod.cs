using Common;
using HalconDotNet;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;

namespace FunctionBlock
{
    [Serializable]
    public class ContourExtractMethod
    {
        public static bool ExtractTrack(userWcsPoint[] curWcsPoints, userWcsPoint[] stdWcsPoint, ContourExtractParam param, out userWcsPolyLine wcsPolyLine, out userWcsPolyLine wcsPolyLineAffine, out int errorCount)
        {
            bool result = false;
            wcsPolyLine = new userWcsPolyLine();
            wcsPolyLineAffine = new userWcsPolyLine();
            errorCount = 0;
            if (curWcsPoints == null) return result;
            if (param == null) return result;
            HHomMat2D hHomMat2D;
            userWcsPoint[] extractPoints = null, curInterPoints = null, stdInterPoints = null, affineWcsPoint = null, affineTempWcsPoint = null;
            ///// 提取轨迹点 ////////////////
            if (param.RemoveRepetitivePoint)
                extractPoints = RemoveRepetitivePoint(curWcsPoints, param.UnionStep);
            else
                extractPoints = curWcsPoints;
            /// 轨迹自检/////////////
            if (param.EnableSelfCheck)
            {
                // 这里需判断下，标准轨迹是否存在，
                if (stdWcsPoint != null && stdWcsPoint.Length > 0)
                {
                    double initValue = double.MaxValue;
                    for (double percent = param.EndPercent; percent < 0.99; percent += 0.1)
                    {
                        curInterPoints = extractPoints;// InterpretationPoint(extractPoints, param.InterStep * 0.5); // 密集采集点用于轮廓对齐  
                        stdInterPoints = stdWcsPoint;// InterpretationPoint(stdWcsPoint, param.InterStep * 0.5);
                        hHomMat2D = TrackAlign(curInterPoints, stdInterPoints, param.StartPercent, percent, param.TransformationType);
                        affineTempWcsPoint = AffinePoint2D(hHomMat2D, extractPoints);
                        HTuple dist = DistancePointToContour(affineTempWcsPoint, stdWcsPoint, 2);
                        double meanValue = dist.TupleMean().D;
                        double maxValue = dist.TupleMax().D;
                        double deviation = dist.TupleDeviation();
                        if (deviation < initValue)
                        {
                            initValue = deviation;
                            affineWcsPoint = Copy(affineTempWcsPoint);
                        }
                    }
                    ////////////////////////////////////////////
                    List<int> index = DistancePointToPoint(affineWcsPoint, stdWcsPoint, param.AnomalyThreshold);
                    for (int i = 0; i < extractPoints.Length; i++)
                    {
                        wcsPolyLine.CamParams = extractPoints[0].CamParams;
                        wcsPolyLine.CamName = extractPoints[0].CamName;
                        wcsPolyLine.ViewWindow = extractPoints[0].ViewWindow;
                        if (index.IndexOf(i) < 0)
                        {
                            wcsPolyLine.Add(extractPoints[i].X, extractPoints[i].Y, extractPoints[i].Z);
                        }
                    }
                    //////////////////////////////////////////////////
                    for (int i = 0; i < affineWcsPoint.Length; i++)
                    {
                        wcsPolyLineAffine.CamParams = extractPoints[0].CamParams;
                        wcsPolyLineAffine.CamName = extractPoints[0].CamName;
                        wcsPolyLineAffine.ViewWindow = extractPoints[0].ViewWindow;
                        wcsPolyLineAffine.Add(affineWcsPoint[i].X, affineWcsPoint[i].Y, affineWcsPoint[i].Z);
                    }
                    errorCount = index.Count;
                }
                else
                {
                    wcsPolyLineAffine.CamParams = extractPoints[0].CamParams;
                    wcsPolyLineAffine.CamName = extractPoints[0].CamName;
                    wcsPolyLineAffine.ViewWindow = extractPoints[0].ViewWindow;
                    for (int i = 0; i < extractPoints.Length; i++)
                    {
                        wcsPolyLine.Add(extractPoints[i].X, extractPoints[i].Y, extractPoints[i].Z);
                        wcsPolyLineAffine.Add(extractPoints[i].X, extractPoints[i].Y, extractPoints[i].Z);
                    }
                    errorCount = 0;
                }
            }
            else
            {
                wcsPolyLineAffine.CamParams = extractPoints[0].CamParams;
                wcsPolyLineAffine.CamName = extractPoints[0].CamName;
                wcsPolyLineAffine.ViewWindow = extractPoints[0].ViewWindow;
                wcsPolyLine.CamParams = extractPoints[0].CamParams;
                wcsPolyLine.CamName = extractPoints[0].CamName;
                wcsPolyLine.ViewWindow = extractPoints[0].ViewWindow;
                for (int i = 0; i < extractPoints.Length; i++)
                {
                    wcsPolyLine.Add(extractPoints[i].X, extractPoints[i].Y, extractPoints[i].Z);
                    wcsPolyLineAffine.Add(extractPoints[i].X, extractPoints[i].Y, extractPoints[i].Z);
                }
            }
            result = true;
            return result;
        }

        /// <summary>
        /// 移除重复点
        /// </summary>
        /// <param name="wcsPoints"></param>
        /// <param name="unionDist"></param>
        /// <returns></returns>
        private static userWcsPoint[] RemoveRepetitivePoint(userWcsPoint[] wcsPoints, double unionDist)
        {
            userWcsPoint[] newWcsPoint = new userWcsPoint[0];
            List<double> removeIndex = new List<double>();
            double temp_x = 0, temp_y = 0;
            for (int i = 0; i < wcsPoints.Length; i++)
            {
                if (removeIndex.IndexOf(i) >= 0) continue;
                temp_x = wcsPoints[i].X;
                temp_y = wcsPoints[i].Y;
                for (int j = 0; j < wcsPoints.Length; j++)
                {
                    if (removeIndex.IndexOf(j) >= 0 || i == j) continue;
                    double dist = Math.Sqrt((wcsPoints[j].X - temp_x) * (wcsPoints[j].X - temp_x) + (wcsPoints[j].Y - temp_y) * (wcsPoints[j].Y - temp_y));
                    if (0 < dist && dist < unionDist)
                        removeIndex.Add(j);
                }
            }
            /////////////////////////////
            List<double> list_x = new List<double>();
            List<double> list_y = new List<double>();
            for (int i = 0; i < wcsPoints.Length; i++)
            {
                if (removeIndex.IndexOf(i) >= 0)
                    continue;
                list_x.Add(wcsPoints[i].X);
                list_y.Add(wcsPoints[i].Y);
            }
            newWcsPoint = new userWcsPoint[list_x.Count];
            for (int i = 0; i < list_x.Count; i++)
            {
                newWcsPoint[i] = new userWcsPoint(list_x[i], list_y[i], wcsPoints[0].Z, wcsPoints[0].Grab_x, wcsPoints[0].Grab_y, wcsPoints[0].CamParams);
            }
            return newWcsPoint;
        }

        /// <summary>
        /// 轨迹对齐
        /// </summary>
        /// <param name="sourceWcsPoint"></param>
        /// <param name="targetWcsPoint"></param>
        /// <param name="startPercent"></param>
        /// <param name="endPercent"></param>
        /// <param name="transformationType"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        private static HHomMat2D TrackAlign(userWcsPoint[] sourceWcsPoint, userWcsPoint[] targetWcsPoint, double startPercent, double endPercent, enTransformationType transformationType = enTransformationType.affine)
        {
            if (sourceWcsPoint == null) throw new ArgumentNullException(nameof(sourceWcsPoint));
            if (targetWcsPoint == null) throw new ArgumentNullException(nameof(targetWcsPoint));
            double[] cur_x = new double[sourceWcsPoint.Length];
            double[] cur_y = new double[sourceWcsPoint.Length];
            double[] cur_z = new double[sourceWcsPoint.Length];
            double[] std_x = new double[targetWcsPoint.Length];
            double[] std_y = new double[targetWcsPoint.Length];
            double[] std_z = new double[targetWcsPoint.Length];
            ////////////////////////////////////////////
            for (int i = 0; i < sourceWcsPoint.Length; i++)
            {
                cur_x[i] = sourceWcsPoint[i].X;
                cur_y[i] = sourceWcsPoint[i].Y;
                cur_z[i] = sourceWcsPoint[i].Z;
            }
            ////////////////////////////////////////////
            for (int i = 0; i < targetWcsPoint.Length; i++)
            {
                std_x[i] = targetWcsPoint[i].X;
                std_y[i] = targetWcsPoint[i].Y;
                std_z[i] = targetWcsPoint[i].Z;
            }
            ////////////////////////////////////////////
            HTuple Qx = 0, Qy = 0, Qz = 0, dist = 0;
            HHomMat2D hHomMat2D = new HHomMat2D();
            HTuple hTuple_cur_x = new HTuple(cur_x);
            HTuple hTuple_cur_y = new HTuple(cur_y);
            ////////////////////////////////////////////
            int index = 0;
            int length = std_x.Length; // 使用标准点
            int matchCount = 0, startIndex = 0, endIndex = 0;// (int)(percent * length);
            double tempStartPercent = startPercent;
            double tempEndPercent = endPercent;
            ////////////////////////////////////
            if (tempStartPercent < 0)
                startIndex = 0;
            else
                startIndex = (int)(tempStartPercent * length);
            if (tempEndPercent >= 1)
                endIndex = (int)(tempEndPercent * length) - 1;
            else
                endIndex = (int)(tempEndPercent * length) - 1;
            matchCount = (endIndex - startIndex) + 1;
            List<double> list_std_x = new List<double>();
            List<double> list_std_y = new List<double>();
            List<double> list_error = new List<double>();
            for (int i = 0; i < std_x.Length; i++) // 遍在模板轮廓的每一个点,找到误差最小的一个点变换矩阵
            {
                index = i;
                list_std_x.Clear();
                list_std_y.Clear();
                while (true)
                {
                    if (list_std_x.Count == matchCount) break;
                    list_std_x.Add(std_x[index % length]);
                    list_std_y.Add(std_y[index % length]);
                    index++;
                }
                //////////////////////////////////////////
                switch (transformationType)
                {
                    case enTransformationType.rigid:
                        hHomMat2D.VectorToRigid(hTuple_cur_x.TupleSelectRange(startIndex, endIndex), hTuple_cur_y.TupleSelectRange(startIndex, endIndex), list_std_x.ToArray(), list_std_y.ToArray());
                        break;
                    case enTransformationType.affine:
                        hHomMat2D.VectorToHomMat2d(hTuple_cur_x.TupleSelectRange(startIndex, endIndex), hTuple_cur_y.TupleSelectRange(startIndex, endIndex), list_std_x.ToArray(), list_std_y.ToArray());
                        break;
                    case enTransformationType.similarity:
                        hHomMat2D.VectorToSimilarity(hTuple_cur_x.TupleSelectRange(startIndex, endIndex), hTuple_cur_y.TupleSelectRange(startIndex, endIndex), list_std_x.ToArray(), list_std_y.ToArray());
                        break;
                    case enTransformationType.projective:
                        hHomMat2D.VectorToProjHomMat2d(hTuple_cur_x.TupleSelectRange(startIndex, endIndex), hTuple_cur_y.TupleSelectRange(startIndex, endIndex), list_std_x.ToArray(), list_std_y.ToArray(),
                            "normalized_dlt", new HTuple(), new HTuple(), new HTuple(), new HTuple(), new HTuple(), new HTuple());
                        break;
                }
                Qx = hHomMat2D.AffineTransPoint2d(hTuple_cur_x, hTuple_cur_y, out Qy);
                dist = DistancePointToContour(Qx.TupleSelectRange(startIndex, endIndex), Qy.TupleSelectRange(startIndex, endIndex), std_x, std_y);
                double meanValue = dist.TupleMean().D;
                list_error.Add(meanValue);
            }
            // 用误差最小点的变换矩阵来变换计算
            HTuple hTupleIndex = new HTuple(list_error.ToArray()).TupleSortIndex();
            index = hTupleIndex[0].I;
            list_std_x.Clear();
            list_std_y.Clear();
            while (true)
            {
                if (list_std_x.Count == matchCount) break;
                list_std_x.Add(std_x[index % length]);
                list_std_y.Add(std_y[index % length]);
                index++;
            }
            //////////////////////////////////////////
            switch (transformationType)
            {
                case enTransformationType.rigid:
                    hHomMat2D.VectorToRigid(hTuple_cur_x.TupleSelectRange(startIndex, endIndex), hTuple_cur_y.TupleSelectRange(startIndex, endIndex), list_std_x.ToArray(), list_std_y.ToArray());
                    break;
                case enTransformationType.affine:
                    hHomMat2D.VectorToHomMat2d(hTuple_cur_x.TupleSelectRange(startIndex, endIndex), hTuple_cur_y.TupleSelectRange(startIndex, endIndex), list_std_x.ToArray(), list_std_y.ToArray());
                    break;
                case enTransformationType.similarity:
                    hHomMat2D.VectorToSimilarity(hTuple_cur_x.TupleSelectRange(startIndex, endIndex), hTuple_cur_y.TupleSelectRange(startIndex, endIndex), list_std_x.ToArray(), list_std_y.ToArray());
                    break;
                case enTransformationType.projective:
                    hHomMat2D.VectorToProjHomMat2d(hTuple_cur_x.TupleSelectRange(startIndex, endIndex), hTuple_cur_y.TupleSelectRange(startIndex, endIndex), list_std_x.ToArray(), list_std_y.ToArray(),
                        "normalized_dlt", new HTuple(), new HTuple(), new HTuple(), new HTuple(), new HTuple(), new HTuple());
                    break;
            }
            /////////////////////////////
            return hHomMat2D;
        }

        private static userWcsPoint[] AffinePoint2D(HHomMat2D hHomMat2D, userWcsPoint[] wcsPoint)
        {
            double sy, phi, theta, tx, ty;
            hHomMat2D.HomMat2dToAffinePar(out sy, out phi, out theta, out tx, out ty);
            double angle = phi * 180 / Math.PI;
            userWcsPoint[] wcsPointAffine = new userWcsPoint[0];
            if (hHomMat2D == null)
            {
                wcsPointAffine = wcsPoint;
                return wcsPointAffine;
            }
            if (wcsPoint == null || wcsPoint.Length == 0) return wcsPointAffine;
            double[] x = new double[wcsPoint.Length];
            double[] y = new double[wcsPoint.Length];
            for (int i = 0; i < wcsPoint.Length; i++)
            {
                x[i] = wcsPoint[i].X;
                y[i] = wcsPoint[i].Y;
            }
            HTuple Qx, Qy;
            Qx = hHomMat2D.AffineTransPoint2d(x, y, out Qy);
            wcsPointAffine = new userWcsPoint[Qx.Length];
            for (int i = 0; i < Qx.Length; i++)
            {
                wcsPointAffine[i] = new userWcsPoint(Qx[i].D, Qy[i].D, wcsPoint[0].Z, wcsPoint[0].Grab_x, wcsPoint[0].Grab_y, wcsPoint[0].CamParams);
            }
            return wcsPointAffine;
        }

        private static userWcsPoint[] InterpretationPoint(userWcsPoint[] wcsPoint, double step)
        {
            userWcsPoint[] interPoint = new userWcsPoint[0];
            if (wcsPoint == null || wcsPoint.Length == 0) return interPoint;
            double[] x = new double[wcsPoint.Length];
            double[] y = new double[wcsPoint.Length];
            double[] inter_x, inter_y;
            for (int i = 0; i < wcsPoint.Length; i++)
            {
                x[i] = wcsPoint[i].X;
                y[i] = wcsPoint[i].Y;
            }
            new WcsData().LineInterpretationByStep(x, y, step, out inter_x, out inter_y);
            interPoint = new userWcsPoint[inter_x.Length];
            for (int i = 0; i < inter_x.Length; i++)
            {
                interPoint[i] = new userWcsPoint(inter_x[i], inter_y[i], wcsPoint[0].Z, wcsPoint[0].Grab_x, wcsPoint[0].Grab_y, wcsPoint[0].CamParams);
            }
            return interPoint;
        }


        private static List<int> DistancePointToPoint(userWcsPoint[] wcsPoint, userWcsPoint[] wcsPointStd, double threshold)
        {
            double[] dist = new double[wcsPoint.Length];
            double[] std_x = new double[wcsPointStd.Length];
            double[] std_y = new double[wcsPointStd.Length];
            List<int> list = new List<int>();
            ////////////////////////////////////////////////
            for (int i = 0; i < wcsPointStd.Length; i++)
            {
                std_x[i] = wcsPointStd[i].X;
                std_y[i] = wcsPointStd[i].Y;
            }
            HXLDCont hXLDCont = new HXLDCont(std_y, std_x);
            double distMin = 0, distMax = 0;
            for (int i = 0; i < wcsPoint.Length; i++)
            {
                hXLDCont.DistancePc(wcsPoint[i].Y, wcsPoint[i].X, out distMin, out distMax);
                dist[i] = distMin;
                if (distMin > threshold)
                    list.Add(i);
            }
            return list;
        }


        private static HTuple DistancePointToContour(userWcsPoint[] curWcsPoint, userWcsPoint[] stdWcsPointStd, int ignoreCount = 0)
        {
            HTuple dist = new HTuple();
            double[] std_x = new double[stdWcsPointStd.Length];
            double[] std_y = new double[stdWcsPointStd.Length];
            ////////////////////////////////////////////////
            for (int i = 0; i < stdWcsPointStd.Length; i++)
            {
                std_x[i] = stdWcsPointStd[i].X;
                std_y[i] = stdWcsPointStd[i].Y;
            }
            HXLDCont hXLDCont = new HXLDCont(std_y, std_x);
            double distMin = 0, distMax = 0;
            for (int i = ignoreCount; i < curWcsPoint.Length - ignoreCount; i++)
            {
                hXLDCont.DistancePc(curWcsPoint[i].Y, curWcsPoint[i].X, out distMin, out distMax);
                dist[i] = distMin;
            }
            return dist;
        }

        private static HTuple DistancePointToContour(HTuple cur_x, HTuple cur_y, HTuple std_x, HTuple std_y)
        {
            HTuple dist = new HTuple();
            ////////////////////////////////////////////////
            HXLDCont hXLDCont = new HXLDCont(std_y, std_x);
            double distMin = 0, distMax = 0;
            for (int i = 0; i < cur_x.Length; i++)
            {
                hXLDCont.DistancePc(cur_y[i], cur_x[i], out distMin, out distMax);
                dist[i] = distMin;
            }
            double minvalue = dist.TupleMin();
            return dist;
        }


        private static userWcsPoint[] Copy(userWcsPoint[] points)
        {
            userWcsPoint[] wcsPointsCopy = new userWcsPoint[points.Length];
            if (points == null || points.Length == 0) return wcsPointsCopy;
            for (int i = 0; i < points.Length; i++)
            {
                wcsPointsCopy[i] = points[i].Clone();
            }
            return wcsPointsCopy;
        }



    }
}

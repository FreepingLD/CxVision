using Common;
using HalconDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace FunctionBlock
{
    [Serializable]
    public class ContourMatchMethod
    {

        public static bool ContourMatch(userWcsPoint[] wcsPoint, userWcsPoint[] wcsPointStd, ContourMatchParam param, out userWcsCoordSystem wcsCoordSystem, out userWcsPolyLine wcsPolyLine, out double[] error)
        {
            bool result = false;
            wcsCoordSystem = new userWcsCoordSystem();
            wcsPolyLine = new userWcsPolyLine();
            error = new double[0];
            if (wcsPoint == null) throw new ArgumentNullException(nameof(wcsPoint));
            if (wcsPointStd == null) throw new ArgumentNullException(nameof(wcsPointStd));
            if (param == null) throw new ArgumentNullException("param");
            if (wcsPoint.Length == 0) return false;
            ////////////////////////////////////////////
            double[] cur_x = new double[wcsPoint.Length];
            double[] cur_y = new double[wcsPoint.Length];
            double[] cur_z = new double[wcsPoint.Length];
            double[] std_x = new double[wcsPointStd.Length];
            double[] std_y = new double[wcsPointStd.Length];
            double[] std_z = new double[wcsPointStd.Length];
            ////////////////////////////////////////////
            for (int i = 0; i < wcsPoint.Length; i++)
            {
                cur_x[i] = wcsPoint[i].X;
                cur_y[i] = wcsPoint[i].Y;
                cur_z[i] = wcsPoint[i].Z;
                wcsPolyLine.CamParams = wcsPoint[i].CamParams;
                wcsPolyLine.CamName = wcsPoint[i].CamName;
                wcsPolyLine.ViewWindow = wcsPoint[i].ViewWindow;
            }
            ////////////////////////////////////////////
            for (int i = 0; i < wcsPointStd.Length; i++)
            {
                std_x[i] = wcsPointStd[i].X;
                std_y[i] = wcsPointStd[i].Y;
                std_z[i] = wcsPointStd[i].Z;
            }
            HTuple Qx = 0, Qy = 0, Qz = 0, dist = 0;
            HHomMat2D hHomMat2D = new HHomMat2D();
            HTuple hTuple_x = new HTuple(cur_x);
            HTuple hTuple_y = new HTuple(cur_y);
            ////////////////////////////////////////////
            int index = 0;
            int length = std_x.Length; // 必需使用当前点
            int matchCount = 0, startIndex = 0, endIndex = 0;// (int)(percent * length);
            if (param.StartPercent < 0)
                startIndex = 0;
            else
                startIndex = (int)(param.StartPercent * length);
            if (param.EndPercent >= 1)
                endIndex = (int)(param.EndPercent * length) - 1;
            else
                endIndex = (int)(param.EndPercent * length) - 1;
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
                /////////////// 变换方式 ///////////////////////////
                switch (param.TransformationType)
                {
                    case enTransformationType.rigid:
                        hHomMat2D.VectorToRigid(hTuple_x.TupleSelectRange(startIndex, endIndex), hTuple_y.TupleSelectRange(startIndex, endIndex), list_std_x.ToArray(), list_std_y.ToArray());
                        break;
                    case enTransformationType.affine:
                        hHomMat2D.VectorToHomMat2d(hTuple_x.TupleSelectRange(startIndex, endIndex), hTuple_y.TupleSelectRange(startIndex, endIndex), list_std_x.ToArray(), list_std_y.ToArray());
                        break;
                    case enTransformationType.similarity:
                        hHomMat2D.VectorToSimilarity(hTuple_x.TupleSelectRange(startIndex, endIndex), hTuple_y.TupleSelectRange(startIndex, endIndex), list_std_x.ToArray(), list_std_y.ToArray());
                        break;
                    case enTransformationType.projective:
                        hHomMat2D.VectorToProjHomMat2d(hTuple_x.TupleSelectRange(startIndex, endIndex), hTuple_y.TupleSelectRange(startIndex, endIndex), list_std_x.ToArray(), list_std_y.ToArray(),
                            "normalized_dlt", new HTuple(), new HTuple(), new HTuple(), new HTuple(), new HTuple(), new HTuple());
                        break;
                }
                Qx = hHomMat2D.AffineTransPoint2d(cur_x, cur_y, out Qy);
                dist = HMisc.DistancePp(Qx.TupleSelectRange(startIndex, endIndex), Qy.TupleSelectRange(startIndex, endIndex), list_std_x.ToArray(), list_std_y.ToArray());
                double meanValue = dist.TupleMean().D;
                list_error.Add(meanValue);
            }
            // 用误差最小点的变换矩阵来变换计算
            hHomMat2D = new HHomMat2D();
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
            switch (param.TransformationType)
            {
                case enTransformationType.rigid:
                    hHomMat2D.VectorToRigid(hTuple_x.TupleSelectRange(startIndex, endIndex), hTuple_y.TupleSelectRange(startIndex, endIndex), list_std_x.ToArray(), list_std_y.ToArray());
                    break;
                case enTransformationType.affine:
                    hHomMat2D.VectorToHomMat2d(hTuple_x.TupleSelectRange(startIndex, endIndex), hTuple_y.TupleSelectRange(startIndex, endIndex), list_std_x.ToArray(), list_std_y.ToArray());
                    break;
                case enTransformationType.similarity:
                    hHomMat2D.VectorToSimilarity(hTuple_x.TupleSelectRange(startIndex, endIndex), hTuple_y.TupleSelectRange(startIndex, endIndex), list_std_x.ToArray(), list_std_y.ToArray());
                    break;
                case enTransformationType.projective:
                    hHomMat2D.VectorToProjHomMat2d(hTuple_x.TupleSelectRange(startIndex, endIndex), hTuple_y.TupleSelectRange(startIndex, endIndex), list_std_x.ToArray(), list_std_y.ToArray(),
                        "normalized_dlt", new HTuple(), new HTuple(), new HTuple(), new HTuple(), new HTuple(), new HTuple());
                    break;
            }
            //hHomMat2D.VectorToRigid(hTuple_x.TupleSelectRange(startIndex, endIndex), hTuple_y.TupleSelectRange(startIndex, endIndex), list_cur_x.ToArray(), list_cur_y.ToArray());
            Qx = hHomMat2D.HomMat2dInvert().AffineTransPoint2d(std_x, std_y, out Qy);
            dist = HMisc.DistancePp(hTuple_x.TupleSelectRange(startIndex, endIndex), hTuple_y.TupleSelectRange(startIndex, endIndex), Qx.TupleSelectRange(startIndex, endIndex), Qy.TupleSelectRange(startIndex, endIndex));
            error = dist.ToDArr();
            for (int i = 0; i < Qx.Length; i++)
            {
                wcsPolyLine.Add(Qx[i], Qy[i]);
            }
            double sx, sy, phi, theta, tx, ty;
            hHomMat2D.HomMat2dInvert().HomMat2dToAffinePar(out sy, out phi, out theta, out tx, out ty);
            wcsCoordSystem.CurrentPoint = new userWcsVector(tx, ty, 0, phi * 180 / Math.PI);
            result = true;
            return result;
        }


    }


}

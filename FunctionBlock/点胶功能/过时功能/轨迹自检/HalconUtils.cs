using Common;
using HalconDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionBlock
{
    public class HalconUtils
    {
        /// <summary>
        /// 自己实现的等弧长重采样函数，模拟 HALCON 的 sample_contours_xld
        /// 只处理单条 XLD 轮廓（一个 HObject，只含一个对象）
        /// </summary>
        public static void SampleContoursXld(HObject hoContour, out HObject hoSampled, double sampleDist)
        {
            HOperatorSet.GenEmptyObj(out hoSampled);

            // 1. 取轮廓点
            HTuple rowsT, colsT;
            HOperatorSet.GetContourXld(hoContour, out rowsT, out colsT);

            int n = rowsT.Length;
            if (n < 2)
            {
                // 点太少，直接返回原轮廓
                hoSampled.Dispose();
                hoSampled = hoContour.Clone();
                return;
            }

            double[] rows = rowsT.DArr;
            double[] cols = colsT.DArr;

            // 2. 计算每个点的累积弧长
            double[] cumLen = new double[n];
            cumLen[0] = 0.0;
            for (int i = 1; i < n; i++)
            {
                double dy = rows[i] - rows[i - 1];
                double dx = cols[i] - cols[i - 1];
                double seg = Math.Sqrt(dx * dx + dy * dy);
                cumLen[i] = cumLen[i - 1] + seg;
            }

            double totalLen = cumLen[n - 1];
            if (totalLen == 0)
            {
                hoSampled.Dispose();
                hoSampled = hoContour.Clone();
                return;
            }

            // 3. 按 sampleDist 产生弧长采样位置
            List<double> sampRows = new List<double>();
            List<double> sampCols = new List<double>();

            int numSamples = Math.Max(2, (int)(totalLen / sampleDist) + 1);
            for (int k = 0; k < numSamples; k++)
            {
                double d = k * sampleDist;
                if (d > totalLen) d = totalLen;

                // 在 cumLen 中找到区间 [i-1, i]，满足 cumLen[i-1] <= d <= cumLen[i]
                int i = 1;
                while (i < n && cumLen[i] < d) i++;

                if (i == n)
                {
                    // 超出最后一点，直接用最后一个点
                    sampRows.Add(rows[n - 1]);
                    sampCols.Add(cols[n - 1]);
                }
                else if (cumLen[i] == d || i == 0)
                {
                    // 刚好落在一个原始点上
                    sampRows.Add(rows[i]);
                    sampCols.Add(cols[i]);
                }
                else
                {
                    // 在两点之间插值
                    double d1 = cumLen[i - 1];
                    double d2 = cumLen[i];
                    double t = (d - d1) / (d2 - d1);

                    double r = rows[i - 1] + t * (rows[i] - rows[i - 1]);
                    double c = cols[i - 1] + t * (cols[i] - cols[i - 1]);

                    sampRows.Add(r);
                    sampCols.Add(c);
                }
            }

            // 确保最后一个点就是原轮廓的终点
            if (sampRows.Count == 0 ||
                sampRows[sampRows.Count - 1] != rows[n - 1] ||
                sampCols[sampCols.Count - 1] != cols[n - 1])
            {
                sampRows.Add(rows[n - 1]);
                sampCols.Add(cols[n - 1]);
            }

            // 4. 用采样点生成新的 XLD 轮廓
            HTuple newRows = new HTuple(sampRows.ToArray());
            HTuple newCols = new HTuple(sampCols.ToArray());

            HOperatorSet.GenContourPolygonXld(out hoSampled, newRows, newCols);
        }

        public static bool  SampleContoursXld(double[] x,double[] y , double sampleDist, out double[] sample_x, out double[] sample_y)
        {
            sample_x = new double[0];
            sample_y = new double[0];
            int n = x.Length;
            if (x.Length < 2)
            {
                // 点太少，直接返回原轮廓
                return false;
            }
            // 2. 计算每个点的累积弧长
            double[] cumLen = new double[n];
            cumLen[0] = 0.0;
            for (int i = 1; i < n; i++)
            {
                double dy = y[i] - y[i - 1];
                double dx = x[i] - x[i - 1];
                double seg = Math.Sqrt(dx * dx + dy * dy);
                cumLen[i] = cumLen[i - 1] + seg;
            }

            double totalLen = cumLen[n - 1];
            // 3. 按 sampleDist 产生弧长采样位置
            List<double> samp_y = new List<double>();
            List<double> samp_x = new List<double>();

            int numSamples = Math.Max(2, (int)(totalLen / sampleDist) + 1);
            for (int k = 0; k < numSamples; k++)
            {
                double d = k * sampleDist;
                if (d > totalLen) d = totalLen;
                // 在 cumLen 中找到区间 [i-1, i]，满足 cumLen[i-1] <= d <= cumLen[i]
                int i = 1;
                while (i < n && cumLen[i] < d) i++;

                if (i == n)
                {
                    // 超出最后一点，直接用最后一个点
                    samp_y.Add(y[n - 1]);
                    samp_x.Add(x[n - 1]);
                }
                else if (cumLen[i] == d || i == 0)
                {
                    // 刚好落在一个原始点上
                    samp_y.Add(y[i]);
                    samp_x.Add(x[i]);
                }
                else
                {
                    // 在两点之间插值
                    double d1 = cumLen[i - 1];
                    double d2 = cumLen[i];
                    double t = (d - d1) / (d2 - d1);

                    double r = y[i - 1] + t * (y[i] - y[i - 1]);
                    double c = x[i - 1] + t * (x[i] - x[i - 1]);

                    samp_y.Add(r);
                    samp_x.Add(c);
                }
            }

            // 确保最后一个点就是原轮廓的终点
            if (samp_y.Count == 0 ||
                samp_y[samp_y.Count - 1] != y[n - 1] ||
                samp_x[samp_x.Count - 1] != x[n - 1])
            {
                samp_y.Add(y[n - 1]);
                samp_x.Add(x[n - 1]);
            }
            sample_x = samp_x.ToArray();
            sample_y = samp_y.ToArray();
            return true;
        }

        public static HTuple ShiftAndAppend(HTuple input, int i)
        {
            double[] data = input.DArr;
            int n = data.Length;
            i = ((i % n) + n) % n; // 支持负数偏移

            if (i == 0) return input;

            double[] result = new double[n];
            Array.Copy(data, i, result, 0, n - i);       // 后半部分
            Array.Copy(data, 0, result, n - i, i);       // 前 i 个拼到后面

            return new HTuple(result);
        }

        /// <summary>
        /// 对应 Python 的 resample_contour：等弧长重采样为固定点数
        /// </summary>
        public static void ResampleContour(HXLDCont contour, int numPoints, out HTuple rows, out HTuple cols)
        {
            HObject contourSampled;
            HOperatorSet.GenEmptyObj(out contourSampled);

            // 计算轮廓长度
            HOperatorSet.LengthXld(contour, out HTuple length);
            double step = length.D / (double)numPoints;

            // 按弧长步长重采样
            HalconUtils.SampleContoursXld(contour, out contourSampled, step);

            // 取采样点坐标 (row=y, col=x)
            HOperatorSet.GetContourXld(contourSampled, out rows, out cols);

            contourSampled.Dispose();
        }


        public static void newCompareContours(HXLDCont contourA, HXLDCont contourB, int numPoints, out HObject resampledCntA, out HObject resampledCntB)
        {
            // Step1: 重采样成固定点数
            ResampleContour(contourA, numPoints, out HTuple rowA, out HTuple colA);
            ResampleContour(contourB, numPoints, out HTuple rowB, out HTuple colB);

            //HXLDCont AlignedCntA;
            HOperatorSet.GenContourPolygonXld(out resampledCntB, rowB, colB);
            HOperatorSet.GenContourPolygonXld(out resampledCntA, rowA, colA);

            // 2. 提取几何特征：面积、中心点坐标、点序
            HTuple areaB, rowCenterB, colCenterB, pointOrderB;
            HTuple areaA, rowCenterA, colCenterA, pointOrderA;

            HOperatorSet.AreaCenterXld(resampledCntB, out areaB, out rowCenterB, out colCenterB, out pointOrderB);
            HOperatorSet.AreaCenterXld(resampledCntA, out areaA, out rowCenterA, out colCenterA, out pointOrderA);

            // 3. 提取方向角 (基于等效椭圆的方向)
            HTuple phiB, phiA;
            HOperatorSet.OrientationXld(resampledCntB, out phiB);
            HOperatorSet.OrientationXld(resampledCntA, out phiA);

            // 4. 计算刚性变换矩阵 (VectorAngleToRigid)
            //    将 B 的中心和角度 变换到 A 的中心和角度
            HOperatorSet.VectorAngleToRigid(
                rowCenterB, colCenterB, phiB,
                rowCenterA, colCenterA, phiA,
                out HTuple homMat2D);

            // 5. 应用变换到原始坐标点
            //HOperatorSet.AffineTransPoint2d(homMat2D, rowB, colB, out rowBAligned, out colBAligned);


            //此时A与B的轮廓已经重叠，但是构成轮廓的数组起点不一样，
            //因此还需要对数组进行滚动，以对齐起点
            HOperatorSet.AffineTransPoint2d(
                homMat2D,
                rowB, colB,
                out HTuple rowBAligned,
                out HTuple colBAligned);






            // 滚动寻找最佳对齐， 遍历所有可能的滚动偏移量 (0 到 n-1)
            int bestRollingOffset = 0;
            double minError = double.MaxValue;
            int n = rowA.Length-10;
            for (int offset = 0; offset < n; offset++)
            {
                double currentError = 0;

                // 2. 计算在当前偏移量下的累计误差（以欧式距离的平方和为例）
                for (int i = 0; i < n; i++)
                {
                    // 计算滚动后B的索引（实现循环）
                    int idxB = (i + offset) % n;
                    double diffRow = rowA[i] - rowBAligned[idxB];
                    double diffCol = colA[i] - colBAligned[idxB];
                    currentError += (diffRow * diffRow + diffCol * diffCol); // 平方误差，避免开方运算
                }

                // 3. 记录最小误差对应的偏移量
                if (currentError < minError)
                {
                    minError = currentError;
                    bestRollingOffset = offset;
                }
            }
            //将偏移量运用于原数组，使得坐标数组每一位保持对齐 （算法可优化）
            rowBAligned = HalconUtils.ShiftAndAppend(rowBAligned, bestRollingOffset);
            colBAligned = HalconUtils.ShiftAndAppend(colBAligned, bestRollingOffset);

            // Step3: 点距 (对应你 Python 里的 dist = sqrt((mtx1-mtx2)^2))
            HTuple dRow = rowA - rowBAligned;
            HTuple dCol = colA - colBAligned;
            HTuple dist = (dRow * dRow + dCol * dCol).TupleSqrt();
            double mean = dist.TupleMean();
            double max = dist.TupleMax();
            double std = dist.TupleDeviation();
            HOperatorSet.GenContourPolygonXld(out resampledCntB, rowBAligned, colBAligned);
            Console.WriteLine("mean_diff = " + mean);
            Console.WriteLine("max_diff  = " + max);
            Console.WriteLine("std_diff  = " + std);
            // Console.WriteLine("rowBAligned = " + rowBAligned.ToString() + "\n");
            //Console.WriteLine("colBAligned = " + colBAligned.ToString());
            //Console.WriteLine("rowAAligned = " + rowA.ToString() + "\n");
            //Console.WriteLine("colAAligned = " + colA.ToString());
            Console.WriteLine("lengthB = " + colBAligned.Length + " " + rowBAligned.Length + "\n");
            Console.WriteLine("lengthA = " + rowA.Length + " " + colA.Length + "\n");
            // disparity (越小越相似) 可以用 dist 的均方来定义
            double shapeScore = (dist * dist).TupleMean();
            Console.WriteLine("shape_score (类似 disparity) = " + shapeScore);
        }

        public static void newCompareContours2(HXLDCont contourA, HXLDCont contourB,double[] cur_x,double[] cur_y, double[] std_x, double[] std_y, int numPoints, out HObject resampledCntA, out HObject resampledCntB)
        {
            // Step1: 重采样成固定点数
            //SampleContoursXld(contourA, numPoints, out HTuple rowA, out HTuple colA);
            //SampleContoursXld(contourB, numPoints, out HTuple rowB, out HTuple colB);

            HTuple rowA = 0, colA = 0, rowB = 0, colB = 0;
            //HXLDCont AlignedCntA;
           HOperatorSet.GenContourPolygonXld(out resampledCntB, rowB, colB);
            HOperatorSet.GenContourPolygonXld(out resampledCntA, rowA, colA);

            // 2. 提取几何特征：面积、中心点坐标、点序
            HTuple areaB, rowCenterB, colCenterB, pointOrderB;
            HTuple areaA, rowCenterA, colCenterA, pointOrderA;

            HOperatorSet.AreaCenterXld(resampledCntB, out areaB, out rowCenterB, out colCenterB, out pointOrderB);
            HOperatorSet.AreaCenterXld(resampledCntA, out areaA, out rowCenterA, out colCenterA, out pointOrderA);

            // 3. 提取方向角 (基于等效椭圆的方向)
            HTuple phiB, phiA;
            HOperatorSet.OrientationXld(resampledCntB, out phiB);
            HOperatorSet.OrientationXld(resampledCntA, out phiA);

            // 4. 计算刚性变换矩阵 (VectorAngleToRigid)
            //    将 B 的中心和角度 变换到 A 的中心和角度
            HOperatorSet.VectorAngleToRigid(
                rowCenterB, colCenterB, phiB,
                rowCenterA, colCenterA, phiA,
                out HTuple homMat2D);

            // 5. 应用变换到原始坐标点
            //HOperatorSet.AffineTransPoint2d(homMat2D, rowB, colB, out rowBAligned, out colBAligned);


            //此时A与B的轮廓已经重叠，但是构成轮廓的数组起点不一样，
            //因此还需要对数组进行滚动，以对齐起点
            HOperatorSet.AffineTransPoint2d(
                homMat2D,
                rowB, colB,
                out HTuple rowBAligned,
                out HTuple colBAligned);






            // 滚动寻找最佳对齐， 遍历所有可能的滚动偏移量 (0 到 n-1)
            int bestRollingOffset = 0;
            double minError = double.MaxValue;
            int n = rowA.Length;
            for (int offset = 0; offset < n; offset++)
            {
                double currentError = 0;

                // 2. 计算在当前偏移量下的累计误差（以欧式距离的平方和为例）
                for (int i = 0; i < n; i++)
                {
                    // 计算滚动后B的索引（实现循环）
                    int idxB = (i + offset) % n;
                    double diffRow = rowA[i] - rowBAligned[idxB];
                    double diffCol = colA[i] - colBAligned[idxB];
                    currentError += (diffRow * diffRow + diffCol * diffCol); // 平方误差，避免开方运算
                }

                // 3. 记录最小误差对应的偏移量
                if (currentError < minError)
                {
                    minError = currentError;
                    bestRollingOffset = offset;
                }
            }
            //将偏移量运用于原数组，使得坐标数组每一位保持对齐 （算法可优化）
            rowBAligned = HalconUtils.ShiftAndAppend(rowBAligned, bestRollingOffset);
            colBAligned = HalconUtils.ShiftAndAppend(colBAligned, bestRollingOffset);

            // Step3: 点距 (对应你 Python 里的 dist = sqrt((mtx1-mtx2)^2))
            HTuple dRow = rowA - rowBAligned;
            HTuple dCol = colA - colBAligned;
            HTuple dist = (dRow * dRow + dCol * dCol).TupleSqrt();
            double mean = dist.TupleMean();
            double max = dist.TupleMax();
            double std = dist.TupleDeviation();
            HOperatorSet.GenContourPolygonXld(out resampledCntB, rowBAligned, colBAligned);
            Console.WriteLine("mean_diff = " + mean);
            Console.WriteLine("max_diff  = " + max);
            Console.WriteLine("std_diff  = " + std);
            // Console.WriteLine("rowBAligned = " + rowBAligned.ToString() + "\n");
            //Console.WriteLine("colBAligned = " + colBAligned.ToString());
            //Console.WriteLine("rowAAligned = " + rowA.ToString() + "\n");
            //Console.WriteLine("colAAligned = " + colA.ToString());
            Console.WriteLine("lengthB = " + colBAligned.Length + " " + rowBAligned.Length + "\n");
            Console.WriteLine("lengthA = " + rowA.Length + " " + colA.Length + "\n");
            // disparity (越小越相似) 可以用 dist 的均方来定义
            double shapeScore = (dist * dist).TupleMean();
            Console.WriteLine("shape_score (类似 disparity) = " + shapeScore);
        }



    }

}

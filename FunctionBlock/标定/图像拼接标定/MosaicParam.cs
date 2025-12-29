using HalconDotNet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionBlock
{
    public class MosaicParam
    {
        [DisplayNameAttribute("FilterType"), Category("特征点提取参数"), Description("鞍点提取的滤波类型")]
        public enFilter SaddleFilter { get; set; }

        [DisplayNameAttribute("Sigma"), Category("特征点提取参数"), Description("鞍点提取的Sigma平滑系数")]
        public double SaddleSigma { get; set; }

        [DisplayNameAttribute("Threshold"), Category("特征点提取参数"), Description("特征点的最小绝对阈值")]
        public double SaddleThreshold { get; set; }




        [DisplayNameAttribute("GrayMatchMethod"), Category("投影匹配参数"), Description("灰度值匹配方法")]
        public enGrayMatchMethod GrayMatchMethod { get; set; }

        [DisplayNameAttribute("MaskSize"), Category("投影匹配参数"), Description("用于灰度值匹配的掩膜大小，即只使用该区域内的灰度值")]
        public int MaskSize { get; set; }

        [DisplayNameAttribute("RowMove"), Category("投影匹配参数"), Description("行移动")]
        public int RowMove { get; set; }

        [DisplayNameAttribute("ColMove"), Category("投影匹配参数"), Description("列移动")]
        public int ColMove { get; set; }

        [DisplayNameAttribute("RowTolerance"), Category("投影匹配参数"), Description("匹配搜索窗口的半高")]
        public int RowTolerance { get; set; }

        [DisplayNameAttribute("ColTolerance"), Category("投影匹配参数"), Description("匹配搜索窗口的半宽")]
        public int ColTolerance { get; set; }

        [DisplayNameAttribute("Rotation"), Category("投影匹配参数"), Description("旋转角度范围")]
        public double Rotation { get; set; }

        [DisplayNameAttribute("MatchThreshold"), Category("投影匹配参数"), Description("灰度值匹配阈值")]
        public int MatchThreshold { get; set; }

        [DisplayNameAttribute("EstimationMethod"), Category("投影匹配参数"), Description("变换矩阵估计算法")]
        public enEstimationMethod EstimationMethod { get; set; }

        [DisplayNameAttribute("DistanceThreshold"), Category("投影匹配参数"), Description("变换一致性检查阈值")]
        public double DistanceThreshold { get; set; }

        [DisplayNameAttribute("RandSeed"), Category("投影匹配参数"), Description("随机数生成器种子")]
        public int RandSeed { get; set; }




        [DisplayNameAttribute("MovePath"), Category("控制参数"), Description("移动路径")]
        public enMovePath MovePath { get; set; }

        [DisplayNameAttribute("RowCount"), Category("控制参数"), Description("阵列行数量")]
        public int RowCount { get; set; }

        [DisplayNameAttribute("ColCount"), Category("控制参数"), Description("阵列列数量")]
        public int ColCount { get; set; }



        public MosaicParam()
        {
            this.SaddleFilter = enFilter.facet;
            this.SaddleSigma = 0.7;
            this.SaddleThreshold = 5;
            ///////////////
            this.GrayMatchMethod = enGrayMatchMethod.ssd;
            this.MaskSize = 15;
            this.RowMove = 0;
            this.ColMove = 0;
            this.RowTolerance = 20;
            this.ColTolerance = 20;
            this.Rotation = 0;
            this.MatchThreshold = 30;
            this.EstimationMethod = enEstimationMethod.gold_standard;
            this.DistanceThreshold = 0.4;
            this.RandSeed = 1314;
            /// 控制参数
            this.RowCount = 2;
            this.ColCount = 2;
            this.MovePath = enMovePath.行扫描;

        }

        /// <summary>
        /// 只做右边和下方的映射
        /// </summary>
        /// <param name="From"></param>
        /// <param name="To"></param>
        public HMatrix GenMapIndex(out int[] From, out int[] To)
        {
            From = new int[0];
            To = new int[0];
            HMatrix hMatrix = new HMatrix(this.RowCount, this.ColCount, 0.0);
            switch (this.MovePath)
            {
                case enMovePath.行扫描:
                    for (int i = 0; i < this.RowCount; i++)
                    {
                        for (int j = 0; j < this.ColCount; j++)
                        {
                            hMatrix[i, j] = i * this.ColCount + j + 1;
                        }
                    }
                    break;
                case enMovePath.Z型扫描:
                    for (int i = 0; i < this.RowCount; i++)
                    {
                        for (int j = 0; j < this.ColCount; j++)
                        {
                            if (i % 2 == 0)
                                hMatrix[i, j] = i * this.ColCount + j + 1;
                            else
                                hMatrix[i, j] = (i + 1) * this.ColCount - j;
                        }
                    }
                    break;
            }
            ///////////////////////////////////////// 将阵列转换成索引映射,只与正右边和正下方的映射
            List<int> listFrom = new List<int>();
            List<int> listTo = new List<int>();
            for (int i = 0; i < this.RowCount; i++)
            {
                for (int j = 0; j < this.ColCount; j++)
                {
                    #region 生成2领域
                    //if ((j + 1) < this.ColCount)
                    //{
                    //    listFrom.Add((int)hMatrix[i, j]);
                    //    listTo.Add((int)hMatrix[i, j + 1]);
                    //}
                    /////////////////////
                    //if ((i + 1) < this.RowCount)
                    //{
                    //    listFrom.Add((int)hMatrix[i, j]);
                    //    listTo.Add((int)hMatrix[i + 1, j]);
                    //}
                    #endregion

                    #region 生成4领域
                    // 正右方位置
                    if ((j + 1) < this.ColCount)
                    {
                        listFrom.Add((int)hMatrix[i, j]);
                        listTo.Add((int)hMatrix[i, j + 1]);  // 正右方位置
                    }
                    // 左下角位置
                    if ((i + 1) < this.RowCount && (j - 1) >= 0)
                    {
                        listFrom.Add((int)hMatrix[i, j]);
                        listTo.Add((int)hMatrix[i + 1, j - 1]); // 左下角位置
                    }
                    ///////////////////正下方位置
                    if ((i + 1) < this.RowCount)
                    {
                        listFrom.Add((int)hMatrix[i, j]);
                        listTo.Add((int)hMatrix[i + 1, j]); // 正下方位置
                    }
                    // 右下角位置
                    if ((i + 1) < this.RowCount && (j + 1) < this.ColCount)
                    {
                        listFrom.Add((int)hMatrix[i, j]);
                        listTo.Add((int)hMatrix[i + 1, j + 1]); // 右下角位置
                    }
                    #endregion
                }
            }
            /////////////////////////////////////
            From = listFrom.ToArray();
            To = listTo.ToArray();
            return   hMatrix;
        }

        /// <summary>
        /// 在矩阵中查找指定值的第一个行列位置
        /// </summary>
        /// <param name="hMatrix"></param>
        /// <param name="value"></param>
        /// <param name="rowIndex"></param>
        /// <param name="colIndex"></param>
        public void IndexOf(HMatrix hMatrix ,int value, out int rowIndex,out int colIndex)
        {
            rowIndex = -1;
            colIndex = -1;
            int tempValue;
            for (int i = 0; i < this.RowCount; i++)
            {
                for (int j = 0; j < this.ColCount; j++)
                {
                    tempValue = (int) hMatrix[i, j];
                    if(tempValue == value)
                    {
                        rowIndex = i;
                        colIndex = j;
                    }
                }
            }
        }

        /// <summary>
        /// 转置一个矩阵
        /// </summary>
        /// <param name="hMatrix"></param>
        /// <returns></returns>
        public int[] InvertMatrixRow (HMatrix hMatrix)
        {
            HMatrix subMatrix;
            List<int> listInt = new List<int>();
            for (int i = this.RowCount -1; i >= 0; i--)
            {
                subMatrix = hMatrix.GetSubMatrix(i, 0, 1, this.ColCount);
                foreach (var item in subMatrix.GetFullMatrix().DArr)
                {
                    listInt.Add((int)item);
                }          
                subMatrix?.Dispose();
            }
            return listInt.ToArray();
        }


    }


    public enum enGrayMatchMethod
    {
        ncc,
        sad,
        ssd,
    }

    public enum enEstimationMethod
    {
        gold_standard,
        normalized_dlt,
    }

    public enum enMovePath
    {
        行扫描,
        Z型扫描,
        N型扫描,
    }
}

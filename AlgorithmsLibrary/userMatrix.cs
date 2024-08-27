using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgorithmsLibrary
{

    /// <summary>
    /// 自定矩阵类，用于以矩阵方式操作数组
    /// </summary>
    public class userMatrix
    {
        private int width;
        private int height;
        private double[] _matrixValue;


        public userMatrix()
        {

        }
        public userMatrix(int width, int height, double value)
        {
            _matrixValue = new double[width * height];
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    _matrixValue[i * width + j] = value;
                }
            }
        }
        public userMatrix(int width, int height, double[] value)
        {
            _matrixValue = new double[width * height];
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    _matrixValue[i * width + j] = value[i * width + j]; // 在指令的行列位置处设置值
                }
            }
        }
        public void CreateMatrix(int width, int height, double value)
        {
            _matrixValue = new double[width * height];
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    _matrixValue[i * width + j] = value;
                }
            }
        }
        public void CreateMatrix(int width, int height, double[] value)
        {
            _matrixValue = new double[width * height];
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    _matrixValue[i * width + j] = value[i * width + j]; // 在指令的行列位置处设置值
                }
            }
        }
        public void SetValueMatrix(int RowIndex, int ColumnIndex, double value)
        {
            if (RowIndex < 0) throw new ArgumentException("RowIndex 值小于0");
            if (ColumnIndex < 0) throw new ArgumentException("ColumnIndex 值小于0");
            if (RowIndex > this.height) throw new ArgumentException("RowIndex 值大于矩阵的高度");
            if (ColumnIndex > this.width) throw new ArgumentException("ColumnIndex 值大于矩阵的宽度");
            _matrixValue[RowIndex * width + ColumnIndex] = value;
        }
        public void SetValueMatrix(int[] RowIndex, int[] ColumnIndex, double[] value)
        {
            if (RowIndex == null) throw new ArgumentNullException("RowIndex");
            if (ColumnIndex == null) throw new ArgumentNullException("ColumnIndex");
            if (value == null) throw new ArgumentNullException("value");
            if (RowIndex.Length != ColumnIndex.Length) throw new ArgumentException("RowIndex与ColumnIndex 长度不相等");
            if (RowIndex.Length != value.Length) throw new ArgumentException("索引长度与给定的值长度不相等");

            //////////////////////////////////////////////
            for (int i = 0; i < RowIndex.Length; i++)
            {
                _matrixValue[RowIndex[i] * width + ColumnIndex[i]] = value[i];
            }
        }
        public void ResetValueMatrix(double[] value)
        {
            if (value == null) throw new ArgumentNullException("value");
            if (value.Length != this._matrixValue.Length) throw new ArgumentNullException("value 元素长度与矩阵长度不相等");
            //////////////////////////////////////////////
            for (int i = 0; i < this._matrixValue.Length; i++)
            {
                _matrixValue[i] = value[i];
            }
        }
        public double[] GetFullMatrix()
        {
            double[] Values = new double[this._matrixValue.Length];
            for (int i = 0; i < this._matrixValue.Length; i++)
            {
                Values[i] = this._matrixValue[i];
            }
            return Values;
        }
        public void GetValueMatrix(int[] RowIndex, int[] ColumnIndex, out double[] value)
        {
            if (RowIndex == null) throw new ArgumentNullException("RowIndex");
            if (ColumnIndex == null) throw new ArgumentNullException("ColumnIndex");
            if (RowIndex.Length != ColumnIndex.Length) throw new ArgumentException("RowIndex与ColumnIndex 长度不相等");

            //////////////////////////////////////////////
            value = new double[RowIndex.Length];
            for (int i = 0; i < RowIndex.Length; i++)
            {
                if (RowIndex[i] < 0) throw new ArgumentException("RowIndex 值小于0");
                if (ColumnIndex[i] < 0) throw new ArgumentException("ColumnIndex 值小于0");
                if (RowIndex[i] > this.height) throw new ArgumentException("RowIndex 值大于矩阵的高度");
                if (ColumnIndex[i] > this.width) throw new ArgumentException("ColumnIndex 值大于矩阵的宽度");
                value[i] = _matrixValue[RowIndex[i] * width + ColumnIndex[i]];
            }

        }
        public void GetValueMatrix(int RowIndex, int ColumnIndex, out double value)
        {
            if (RowIndex < 0) throw new ArgumentException("RowIndex 值小于0");
            if (ColumnIndex < 0) throw new ArgumentException("ColumnIndex 值小于0");
            if (RowIndex > this.height) throw new ArgumentException("RowIndex 值大于矩阵的高度");
            if (ColumnIndex > this.width) throw new ArgumentException("ColumnIndex 值大于矩阵的宽度");


            value = _matrixValue[RowIndex * width + ColumnIndex];
        }

        /// <summary>
        /// 返回新矩阵对象
        /// </summary>
        /// <param name="RowIndex"></param>
        /// <param name="ColumnIndex"></param>
        /// <param name="RowsSub"></param>
        /// <param name="ColumnsSub"></param>
        /// <returns></returns>
        public userMatrix GetSubMatrix(int RowIndex, int ColumnIndex, int RowsSub, int ColumnsSub)
        {
            if (RowIndex < 0) throw new ArgumentException("RowIndex 值小于0");
            if (ColumnIndex < 0) throw new ArgumentException("ColumnIndex 值小于0");
            if (RowIndex > this.height) throw new ArgumentException("RowIndex 值大于矩阵的高度");
            if (ColumnIndex > 0) throw new ArgumentException("ColumnIndex 值大于矩阵的宽度");
            if (RowsSub > this.height - RowIndex) throw new ArgumentException("RowIndex 值大于矩阵的高度");
            if (ColumnsSub > this.width - ColumnIndex) throw new ArgumentException("ColumnIndex 值大于矩阵的宽度");

            //////////////////////////////
            double[] values = new double[RowsSub * ColumnsSub];
            for (int i = RowIndex; i < height - RowsSub; i++)
            {
                for (int j = ColumnIndex; j < width - ColumnsSub; j++)
                {
                    values[i * width + j] = _matrixValue[i * width + j]; // i * width + j : 表示在指令的行列位置处设置或获取值
                }
            }
            return new userMatrix(ColumnsSub, RowsSub, values);
        }
        public double [] GetSubMatrixValue(int RowIndex, int ColumnIndex, int RowsSub, int ColumnsSub)
        {
            if (RowIndex < 0) throw new ArgumentException("RowIndex 值小于0");
            if (ColumnIndex < 0) throw new ArgumentException("ColumnIndex 值小于0");
            if (RowIndex > this.height) throw new ArgumentException("RowIndex 值大于矩阵的高度");
            if (ColumnIndex > 0) throw new ArgumentException("ColumnIndex 值大于矩阵的宽度");
            if (RowsSub > this.height - RowIndex) throw new ArgumentException("RowIndex 值大于矩阵的高度");
            if (ColumnsSub > this.width - ColumnIndex) throw new ArgumentException("ColumnIndex 值大于矩阵的宽度");

            //////////////////////////////
            double[] values = new double[RowsSub * ColumnsSub];
            for (int i = RowIndex; i < height - RowsSub; i++)
            {
                for (int j = ColumnIndex; j < width - ColumnsSub; j++)
                {
                    values[i * width + j] = _matrixValue[i * width + j]; // i * width + j : 表示在指令的行列位置处设置或获取值
                }
            }
            return values;
        }

        /// <summary>
        ///返回当前矩阵对象
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public userMatrix MultValueMatrix(double value)
        {
           // double[] multValue = new double[width * height];
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    this._matrixValue[i * width + j] *= value; // multValue[i * width + j] = 
                }
            }
            //return new userMatrix(this.width, this.height, multValue);
            return this;
        }
        public double SumValueMatrix()
        {
            double sumValue = 0;
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    sumValue += this._matrixValue[i * width + j];
                }
            }
            return sumValue;
        }

        /// <summary>
        /// 返回当前矩阵对象
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public userMatrix AddValueMatrix(double[] value)
        {
            if (this._matrixValue.Length != value.Length) throw new ArgumentException("value 元素长度与矩阵长度不相等");
            //double sumValue = 0;
            for (int j = 0; j < value.Length; j++)
            {
                this._matrixValue[j] += value[j];
            }
            //return sumValue;
            return this;
        }





    }
}

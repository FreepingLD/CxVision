using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;

namespace Common
{
    [Serializable]
    [System.Xml.Serialization.XmlInclude(typeof(AxisCalibration))]
    public class AxisCalibration
    {
        public Dictionary<double, XyValuePairs> Calibrate_X { get; set; }
        public Dictionary<double, XyValuePairs> Calibrate_Y { get; set; }
        public Dictionary<double, XyValuePairs> Calibrate_Z { get; set; }
        public Dictionary<double, XyValuePairs> Calibrate_Theta { get; set; }
        public Dictionary<double, XyValuePairs> Calibrate_U { get; set; }
        public Dictionary<double, XyValuePairs> Calibrate_V { get; set; }

        public AxisCalibration(Dictionary<double, XyValuePairs> dic_x, Dictionary<double, XyValuePairs> dic_y)
        {
            this.Calibrate_X = dic_x;
            this.Calibrate_Y = dic_y;
        }


        public AxisCalibration()
        {

        }

        private double GetIndex_X(double calibrateValue)
        {
            double diff = double.MaxValue;
            double index = 0;
            foreach (var item in Calibrate_X)
            {
                if (Math.Abs(item.Key - calibrateValue) < diff)
                {
                    diff = Math.Abs(item.Key - calibrateValue);
                    index = item.Key;
                }
            }
            return index;
        }
        private double GetIndex_Y(double calibrateValue)
        {
            double diff = double.MaxValue;
            double index = 0;
            foreach (var item in Calibrate_Y)
            {
                if (Math.Abs(item.Key - calibrateValue) < diff)
                {
                    diff = Math.Abs(item.Key - calibrateValue);
                    index = item.Key;
                }
            }
            return index;
        }
        private double GetIndex_Z(double calibrateValue)
        {
            double diff = double.MaxValue;
            double index = 0;
            foreach (var item in Calibrate_Z)
            {
                if (Math.Abs(item.Key - calibrateValue) < diff)
                {
                    diff = Math.Abs(item.Key - calibrateValue);
                    index = item.Key;
                }
            }
            return index;
        }
        private double GetIndex_Theta(double calibrateValue)
        {
            double diff = double.MaxValue;
            double index = 0;
            foreach (var item in Calibrate_Theta)
            {
                if (Math.Abs(item.Key - calibrateValue) < diff)
                {
                    diff = Math.Abs(item.Key - calibrateValue);
                    index = item.Key;
                }
            }
            return index;
        }
        private double GetIndex_U(double calibrateValue)
        {
            double diff = double.MaxValue;
            double index = 0;
            foreach (var item in Calibrate_U)
            {
                if (Math.Abs(item.Key - calibrateValue) < diff)
                {
                    diff = Math.Abs(item.Key - calibrateValue);
                    index = item.Key;
                }
            }
            return index;
        }
        private double GetIndex_V(double calibrateValue)
        {
            double diff = double.MaxValue;
            double index = 0;
            foreach (var item in Calibrate_V)
            {
                if (Math.Abs(item.Key - calibrateValue) < diff)
                {
                    diff = Math.Abs(item.Key - calibrateValue);
                    index = item.Key;
                }
            }
            return index;
        }
        private void CalibrateValueToSourceValueAxisX(double calibrateValue, out double sourceValue)
        {
            if (this.Calibrate_X == null || this.Calibrate_X.Count == 0)
                sourceValue = calibrateValue;
            else
            {
                // 这么做是为了兼容多段补偿
                double keyIndex = GetIndex_X(calibrateValue);
                if (Calibrate_X.ContainsKey(keyIndex))
                {
                    XyValuePairs xyValuePairs = Calibrate_X[keyIndex];
                    sourceValue = GetYValueFunct1d(xyValuePairs.StdValue, xyValuePairs.CurValue, calibrateValue);
                }
                else
                {
                    sourceValue = calibrateValue;
                }
            }
        }
        private void CalibrateValueToSourceValueAxisY(double calibrateValue, out double sourceValue)
        {
            if (this.Calibrate_Y == null || this.Calibrate_Y.Count == 0)
                sourceValue = calibrateValue;
            else
            {
                // 这么做是为了兼容多段补偿
                double keyIndex = GetIndex_Y(calibrateValue);
                if (Calibrate_Y.ContainsKey(keyIndex))
                {
                    XyValuePairs xyValuePairs = Calibrate_Y[GetIndex_Y(calibrateValue)];
                    sourceValue = GetYValueFunct1d(xyValuePairs.StdValue, xyValuePairs.CurValue, calibrateValue);
                }
                else
                {
                    sourceValue = calibrateValue;
                }
            }
        }
        private void CalibrateValueToSourceValueAxisZ(double calibrateValue, out double sourceValue)
        {
            if (this.Calibrate_Z == null || this.Calibrate_Z.Count == 0)
                sourceValue = calibrateValue;
            else
            {
                double keyIndex = GetIndex_Z(calibrateValue);
                if (Calibrate_Z.ContainsKey(keyIndex))
                {
                    XyValuePairs xyValuePairs = Calibrate_Z[GetIndex_Z(calibrateValue)];
                    sourceValue = GetYValueFunct1d(xyValuePairs.StdValue, xyValuePairs.CurValue, calibrateValue);
                }
                else
                {
                    sourceValue = calibrateValue;
                }
            }
        }


        private void CalibrateValueToSourceValueAxisTheta(double calibrateValue, out double sourceValue)
        {
            if (this.Calibrate_Theta == null || this.Calibrate_Theta.Count == 0)
                sourceValue = calibrateValue;
            else
            {
                double keyIndex = GetIndex_Theta(calibrateValue);
                if (Calibrate_Theta.ContainsKey(keyIndex))
                {
                    XyValuePairs xyValuePairs = Calibrate_Theta[GetIndex_Theta(calibrateValue)];
                    sourceValue = GetYValueFunct1d(xyValuePairs.StdValue, xyValuePairs.CurValue, calibrateValue);
                }
                else
                {
                    sourceValue = calibrateValue;
                }

            }
        }
        private void CalibrateValueToSourceValueAxisU(double calibrateValue, out double sourceValue)
        {
            if (this.Calibrate_U == null || this.Calibrate_U.Count == 0)
                sourceValue = calibrateValue;
            else
            {
                double keyIndex = GetIndex_U(calibrateValue);
                if (Calibrate_U.ContainsKey(keyIndex))
                {
                    XyValuePairs xyValuePairs = Calibrate_U[GetIndex_Theta(calibrateValue)];
                    sourceValue = GetYValueFunct1d(xyValuePairs.StdValue, xyValuePairs.CurValue, calibrateValue);
                }
                else
                {
                    sourceValue = calibrateValue;
                }

            }
        }
        private void CalibrateValueToSourceValueAxisV(double calibrateValue, out double sourceValue)
        {
            if (this.Calibrate_V == null || this.Calibrate_V.Count == 0)
                sourceValue = calibrateValue;
            else
            {
                double keyIndex = GetIndex_V(calibrateValue);
                if (Calibrate_V.ContainsKey(keyIndex))
                {
                    XyValuePairs xyValuePairs = Calibrate_V[GetIndex_Theta(calibrateValue)];
                    sourceValue = GetYValueFunct1d(xyValuePairs.StdValue, xyValuePairs.CurValue, calibrateValue);
                }
                else
                {
                    sourceValue = calibrateValue;
                }

            }
        }
        public void CalibrateValueToSourceValueAxisXY(double calibrateValue_x, double calibrateValue_y, out double sourceValue_x, out double sourceValue_y)
        {
            ////////////////////  补偿X轴
            CalibrateValueToSourceValueAxisX(calibrateValue_x, out sourceValue_x);
            ////////////////////  补偿Y轴
            CalibrateValueToSourceValueAxisY(calibrateValue_y, out sourceValue_y);
        }
        private void SourceValueToCalibrateValueAxisX(double sourceValue, out double calibrateValue)
        {
            if (this.Calibrate_X == null || this.Calibrate_X.Count == 0)
                calibrateValue = sourceValue;
            else
            {
                // 这么做是为了兼容多段补偿
                double keyIndex = GetIndex_X(sourceValue);
                if (Calibrate_X.ContainsKey(keyIndex))
                {
                    XyValuePairs xyValuePairs = Calibrate_X[GetIndex_X(sourceValue)];
                    calibrateValue = GetYValueFunct1d(xyValuePairs.CurValue, xyValuePairs.StdValue, sourceValue);
                }
                else
                {
                    calibrateValue = sourceValue;
                }
            }
        }
        private void SourceValueToCalibrateValueAxisY(double sourceValue, out double calibrateValue)
        {
            if (this.Calibrate_Y == null || this.Calibrate_Y.Count == 0)
                calibrateValue = sourceValue;
            else
            {
                // 这么做是为了兼容多段补偿
                double keyIndex = GetIndex_Y(sourceValue);
                if (Calibrate_Y.ContainsKey(keyIndex))
                {
                    XyValuePairs xyValuePairs = Calibrate_Y[GetIndex_Y(sourceValue)];
                    calibrateValue = GetYValueFunct1d(xyValuePairs.CurValue, xyValuePairs.StdValue, sourceValue);
                }
                else
                {
                    calibrateValue = sourceValue;
                }
            }
        }
        private void SourceValueToCalibrateValueAxisZ(double sourceValue, out double calibrateValue)
        {
            if (this.Calibrate_Z == null || this.Calibrate_Z.Count == 0)
                calibrateValue = sourceValue;
            else
            {
                double keyIndex = GetIndex_Z(sourceValue);
                if (Calibrate_Z.ContainsKey(keyIndex))
                {
                    XyValuePairs xyValuePairs = Calibrate_Z[GetIndex_Z(sourceValue)];
                    calibrateValue = GetYValueFunct1d(xyValuePairs.CurValue, xyValuePairs.StdValue, sourceValue);
                }
                else
                {
                    calibrateValue = sourceValue;
                }
            }
        }
        private void SourceValueToCalibrateValueAxisTheta(double sourceValue, out double calibrateValue)
        {
            if (this.Calibrate_Theta == null || this.Calibrate_Theta.Count == 0)
                calibrateValue = sourceValue;
            else
            {
                double keyIndex = GetIndex_Theta(sourceValue);
                if (Calibrate_Theta.ContainsKey(keyIndex))
                {
                    XyValuePairs xyValuePairs = Calibrate_Theta[GetIndex_Theta(sourceValue)];
                    calibrateValue = GetYValueFunct1d(xyValuePairs.CurValue, xyValuePairs.StdValue, sourceValue);
                }
                else
                {
                    calibrateValue = sourceValue;
                }
            }
        }
        private void SourceValueToCalibrateValueAxisU(double sourceValue, out double calibrateValue)
        {
            if (this.Calibrate_U == null || this.Calibrate_U.Count == 0)
                calibrateValue = sourceValue;
            else
            {
                double keyIndex = GetIndex_U(sourceValue);
                if (Calibrate_U.ContainsKey(keyIndex))
                {
                    XyValuePairs xyValuePairs = Calibrate_U[GetIndex_Theta(sourceValue)];
                    calibrateValue = GetYValueFunct1d(xyValuePairs.CurValue, xyValuePairs.StdValue, sourceValue);
                }
                else
                {
                    calibrateValue = sourceValue;
                }
            }
        }
        private void SourceValueToCalibrateValueAxisV(double sourceValue, out double calibrateValue)
        {
            if (this.Calibrate_V == null || this.Calibrate_V.Count == 0)
                calibrateValue = sourceValue;
            else
            {
                double keyIndex = GetIndex_Theta(sourceValue);
                if (Calibrate_V.ContainsKey(keyIndex))
                {
                    XyValuePairs xyValuePairs = Calibrate_V[GetIndex_Theta(sourceValue)];
                    calibrateValue = GetYValueFunct1d(xyValuePairs.CurValue, xyValuePairs.StdValue, sourceValue);
                }
                else
                {
                    calibrateValue = sourceValue;
                }
            }
        }
        public void SourceValueToCalibrateValueAxisXY(double sourceValue_x, double sourceValue_y, out double calibrateValue_x, out double calibrateValue_y)
        {
            ////////////////////  补偿X轴
            if (this.Calibrate_X == null || this.Calibrate_X.Count == 0)
                calibrateValue_x = sourceValue_x;
            else
            {
                double[] keysValue = this.Calibrate_X.Keys.ToArray();
                XyValuePairs[] Value = this.Calibrate_X.Values.ToArray();
                int index = 0;
                double diff_value = 100000;
                for (int i = 0; i < keysValue.Length; i++) // 查找距离最近的值
                {
                    if (Math.Abs(sourceValue_y - keysValue[i]) < diff_value)
                    {
                        diff_value = Math.Abs(sourceValue_y - keysValue[i]);
                        index = i;
                    }
                }
                calibrateValue_x = GetYValueFunct1d(Value[index].CurValue, Value[index].StdValue, sourceValue_x);
            }
            ////////////////////  补偿Y轴
            if (this.Calibrate_Y == null || this.Calibrate_Y.Count == 0)
                calibrateValue_y = sourceValue_y;
            else
            {
                double[] keysValue = this.Calibrate_Y.Keys.ToArray();
                XyValuePairs[] Value = this.Calibrate_Y.Values.ToArray();
                int index = 0;
                double diff_value = 100000;
                for (int i = 0; i < keysValue.Length; i++) // 查找距离最近的值
                {
                    if (Math.Abs(sourceValue_x - keysValue[i]) < diff_value)
                    {
                        diff_value = Math.Abs(sourceValue_x - keysValue[i]);
                        index = i;
                    }
                }
                calibrateValue_y = GetYValueFunct1d(Value[index].CurValue, Value[index].StdValue, sourceValue_y);
            }
        }
        public void CalibrateValueToSourceValue(string AxisName, double calibrateValue, out double sourceValue)
        {
            switch (AxisName)
            {
                default:
                    sourceValue = calibrateValue;
                    break;
                case "x":
                case "X":
                case "X轴":
                case "x轴":
                    CalibrateValueToSourceValueAxisX(calibrateValue, out sourceValue);
                    break;
                case "y":
                case "Y":
                case "Y轴":
                case "y轴":
                    CalibrateValueToSourceValueAxisY(calibrateValue, out sourceValue);
                    break;
                case "z":
                case "Z":
                case "Z轴":
                case "z轴":
                    CalibrateValueToSourceValueAxisZ(calibrateValue, out sourceValue);
                    break;
                case "W":
                case "theta":
                case "Theta":
                case "Theta轴":
                case "Angle轴":
                case "Angle":
                    CalibrateValueToSourceValueAxisTheta(calibrateValue, out sourceValue);
                    break;
                case "U":
                case "A":
                case "u":
                case "a":
                    CalibrateValueToSourceValueAxisU(calibrateValue, out sourceValue);
                    break;
                case "V":
                case "B":
                case "v":
                case "b":
                    CalibrateValueToSourceValueAxisV(calibrateValue, out sourceValue);
                    break;
            }
        }
        public void SourceValueToCalibrateValue(string AxisName, double sourceValue, out double calibrateValue)
        {
            switch (AxisName)
            {
                default:
                    calibrateValue = sourceValue;
                    break;
                case "x":
                case "X":
                case "X轴":
                    SourceValueToCalibrateValueAxisX(sourceValue, out calibrateValue);
                    break;
                case "y":
                case "Y":
                case "Y轴":
                    SourceValueToCalibrateValueAxisY(sourceValue, out calibrateValue);
                    break;
                case "z":
                case "Z":
                case "Z轴":
                    SourceValueToCalibrateValueAxisZ(sourceValue, out calibrateValue);
                    break;
                case "W":
                case "theta":
                case "Theta":
                case "Theta轴":
                    SourceValueToCalibrateValueAxisTheta(sourceValue, out calibrateValue);
                    break;
                case "U":
                case "A":
                case "u":
                case "a":
                    SourceValueToCalibrateValueAxisU(sourceValue, out calibrateValue);
                    break;
                case "V":
                case "B":
                case "v":
                case "b":
                    SourceValueToCalibrateValueAxisV(sourceValue, out calibrateValue);
                    break;
            }
        }



        /// <summary>
        ///获取指定位置处的Y值，要求数据都为升序或都为降序
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="x"></param>
        public double GetYValueFunct1d(double[] X, double[] Y, double x)
        {
            if (X == null) throw new ArgumentNullException("X");
            if (Y == null) throw new ArgumentNullException("Y");
            if (X.Length != Y.Length) throw new ArgumentException("X/Y元素个数不相等");
            if (Y.Length < 2) throw new ArgumentException("长度不能小于2");
            double k = 0, b = 0, y = 0;
            //////////////////////////
            for (int i = 0; i < X.Length - 1; i++)
            {
                // X在两点之间
                if (x > X[i] && x < X[i + 1])
                {
                    k = (Y[i + 1] - Y[i]) / (X[i + 1] - X[i]);
                    b = Y[i] - k * X[i];
                    y = k * x + b;
                }
                // x 在第一个点之外
                if (x < X[0])
                {
                    k = (Y[1] - Y[0]) / (X[1] - X[0]);
                    b = Y[0] - k * X[0];
                    y = k * x + b;
                }
                if (x > X[X.Length - 1])
                {
                    k = (Y[Y.Length - 2] - Y[Y.Length - 1]) / (X[X.Length - 2] - X[X.Length - 1]);
                    b = Y[X.Length - 1] - k * X[X.Length - 1];
                    y = k * x + b;
                }
            }
            return y;
        }


        public bool Save(string filePath)
        {
            BinaryFormatter binFormat = new BinaryFormatter(); //
            try
            {
                using (FileStream fStream = new FileStream(filePath, FileMode.Create, FileAccess.ReadWrite))
                {
                    binFormat.Serialize(fStream, this);
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                LoggerHelper.Error("保存校准文件失败", ex);
                throw;
            }
        }
        public AxisCalibration Read(string filePath)
        {
            string path = filePath;
            if (!File.Exists(path)) return new AxisCalibration();
            AxisCalibration axisCalibration = new AxisCalibration();
            BinaryFormatter binFormat = new BinaryFormatter();
            try
            {
                using (Stream fStream = File.OpenRead(path))
                {
                    axisCalibration = (AxisCalibration)binFormat.Deserialize(fStream);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                LoggerHelper.Error("读取校准文件失败", ex);
            }
            return axisCalibration;
        }


    }

    [Serializable]
    public struct XyValuePairs
    {
        public double[] CurValue { get; set; }
        public double[] StdValue { get; set; }

        public XyValuePairs(double[] curValue, double[] stdValue)
        {
            this.CurValue = curValue;
            this.StdValue = stdValue;
        }

    }

}

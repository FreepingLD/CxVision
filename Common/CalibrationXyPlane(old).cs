using HalconDotNet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Common
{
    [Serializable]
    public class CalibrationXyPlaneOld
    {
        // 根据实际值来获取理论值
        private HMatrix hMatrix_x_std; // 表示X方向上的标准距离
        private HMatrix hMatrix_y_std; // 表示Y方向上的标准距离
        private HMatrix hMatrix_x_cur; // 表示X方向上的实测距离
        private HMatrix hMatrix_y_cur; // 表示Y方向上的实测距离

        [NonSerialized]
        private HFunction1D _calibrateAxis_x = null;
        [NonSerialized]
        private HFunction1D _calibrateAxis_y = null;
        [NonSerialized]
        private HFunction1D _calibrateAxis_z = null;

        private Dictionary<double, XyValuePairs> calibrate_x = new Dictionary<double, XyValuePairs>();
        private Dictionary<double, XyValuePairs> calibrate_y = new Dictionary<double, XyValuePairs>();
        private Dictionary<double, XyValuePairs> calibrate_z = new Dictionary<double, XyValuePairs>();

        public HFunction1D CalibrateAxis_x { get => _calibrateAxis_x; set => _calibrateAxis_x = value; }
        public HFunction1D CalibrateAxis_y { get => _calibrateAxis_y; set => _calibrateAxis_y = value; }
        public HFunction1D CalibrateAxis_z { get => _calibrateAxis_z; set => _calibrateAxis_z = value; }


        public CalibrationXyPlaneOld(Dictionary<double, XyValuePairs> dic_x, Dictionary<double, XyValuePairs> dic_y)
        {
            this.calibrate_x = dic_x;
            this.calibrate_y = dic_y;
        }


        public CalibrationXyPlaneOld()
        {

        }
        public CalibrationXyPlaneOld(HFunction1D calibrateAxis_x, HFunction1D calibrateAxis_y, HFunction1D calibrateAxis_z)
        {
            this._calibrateAxis_x = calibrateAxis_x;
            this._calibrateAxis_y = calibrateAxis_y;
            this._calibrateAxis_z = calibrateAxis_z;
        }
        public CalibrationXyPlaneOld(HMatrix hMatrix_x_std, HMatrix hMatrix_x_cur, HMatrix hMatrix_y_std, HMatrix hMatrix_y_cur)
        {
            this.hMatrix_x_std = hMatrix_x_std;
            this.hMatrix_x_cur = hMatrix_x_cur;
            this.hMatrix_y_std = hMatrix_y_std;
            this.hMatrix_y_cur = hMatrix_y_cur;
        }


        public void GetCalibratePointsFromWorldPlaneToCalibratePlane(HTuple rows, HTuple cols, HTuple camParam, HTuple camPose, out HTuple calibratePlane_x, out HTuple calibratePlane_y)
        {
            HTuple std_x, std_y, cur_x, cur_y, x_range, y_range, wcsPlane_x, wcsPlane_y;
            int rowCount, colCount;
            HFunction1D hFunction1D_x, hFunction1D_y;
            if (camParam == null || camPose == null)
            {
                calibratePlane_x = cols;
                calibratePlane_y = rows;
                return;
            }
            HOperatorSet.ImagePointsToWorldPlane(camParam, new HPose(camPose), rows, cols, 1, out wcsPlane_x, out wcsPlane_y);
            //HCamPar.ImagePointsToWorldPlane(camParam, new HPose(camPose), rows, cols, 1, out wcsPlane_x, out wcsPlane_y);
            if (hMatrix_x_std == null)
            {
                calibratePlane_x = wcsPlane_x;
                calibratePlane_y = wcsPlane_y;
                return;
            }
            this.hMatrix_x_std.GetSizeMatrix(out rowCount, out colCount);
            if (rowCount == 0)
            {
                calibratePlane_x = wcsPlane_x;
                calibratePlane_y = wcsPlane_y;
            }
            ///////////
            x_range = this.hMatrix_x_cur.GetValueMatrix(HTuple.TupleGenConst(colCount, 0), HTuple.TupleGenSequence(0, colCount - 1, 1));
            y_range = this.hMatrix_y_cur.GetValueMatrix(HTuple.TupleGenSequence(0, rowCount - 1, 1), HTuple.TupleGenConst(rowCount, 0));
            calibratePlane_x = new HTuple();
            calibratePlane_y = new HTuple();
            int index_x = 0;
            int index_y = 0;
            for (int i = 0; i < wcsPlane_x.Length; i++)
            {
                if (x_range[0] < x_range[x_range.Length - 1]) // 表示数据为升序排列
                {
                    index_y = FindIndex(y_range * -1, wcsPlane_y[i].D * -1);
                    std_x = this.hMatrix_x_std.GetValueMatrix(HTuple.TupleGenConst(colCount, index_y), HTuple.TupleGenSequence(0, colCount - 1, 1));
                    cur_x = this.hMatrix_x_cur.GetValueMatrix(HTuple.TupleGenConst(colCount, index_y), HTuple.TupleGenSequence(0, colCount - 1, 1));
                    ///////////////////////// 获取X坐标
                    hFunction1D_x = new HFunction1D(cur_x, std_x);// X坐标只能升序排列
                    if (wcsPlane_x[i].D < cur_x.TupleMin())
                    {
                        calibratePlane_x.Append(std_x.TupleMin().D * 2 - hFunction1D_x.GetYValueFunct1d(wcsPlane_x[i].D, "mirror"));
                    }
                    else
                    {
                        if (wcsPlane_x[i].D > cur_x.TupleMax())
                        {
                            calibratePlane_x.Append(std_x.TupleMax().D * 2 - hFunction1D_x.GetYValueFunct1d(wcsPlane_x[i].D, "mirror"));
                        }
                        else
                            calibratePlane_x.Append(hFunction1D_x.GetYValueFunct1d(wcsPlane_x[i].D, "mirror"));
                    }
                }
                else // 表示数据为降序排列
                {
                    index_y = FindIndex(y_range * -1, wcsPlane_y[i].D * -1);
                    std_x = this.hMatrix_x_std.GetValueMatrix(HTuple.TupleGenConst(colCount, index_y), HTuple.TupleGenSequence(0, colCount - 1, 1)) * -1;
                    cur_x = this.hMatrix_x_cur.GetValueMatrix(HTuple.TupleGenConst(colCount, index_y), HTuple.TupleGenSequence(0, colCount - 1, 1)) * -1;
                    ///////////////////////// 获取X坐标
                    hFunction1D_x = new HFunction1D(cur_x, std_x);// X坐标只能升序排列
                    if (wcsPlane_x[i].D * -1 < cur_x.TupleMin())
                    {
                        calibratePlane_x.Append((std_x.TupleMin().D * 2 - hFunction1D_x.GetYValueFunct1d(wcsPlane_x[i].D, "mirror")) * -1);
                    }
                    else
                    {
                        if (wcsPlane_x[i].D * -1 > cur_x.TupleMax())
                        {
                            calibratePlane_x.Append((std_x.TupleMax().D * 2 - hFunction1D_x.GetYValueFunct1d(wcsPlane_x[i].D, "mirror")) * -1);
                        }
                        else
                            calibratePlane_x.Append(hFunction1D_x.GetYValueFunct1d(wcsPlane_x[i].D, "mirror") * -1);
                    }
                }
                if (y_range[0] < y_range[y_range.Length - 1]) // 选择相应的轮廓，索引是相反的
                {
                    index_x = FindIndex(x_range, wcsPlane_x[i]);
                    std_y = this.hMatrix_y_std.GetValueMatrix(HTuple.TupleGenSequence(0, rowCount - 1, 1), HTuple.TupleGenConst(rowCount, index_x)); //这里不能用绝对值
                    cur_y = this.hMatrix_y_cur.GetValueMatrix(HTuple.TupleGenSequence(0, rowCount - 1, 1), HTuple.TupleGenConst(rowCount, index_x));
                    ///////////// 获取Y坐标
                    hFunction1D_y = new HFunction1D(cur_y, std_y);
                    if (wcsPlane_y[i].D < cur_y.TupleMin())
                    {
                        calibratePlane_y.Append((std_y.TupleMin().D * 2 - hFunction1D_y.GetYValueFunct1d(wcsPlane_y[i].D, "mirror")));
                    }
                    else
                    {
                        if (wcsPlane_y[i].D > cur_y.TupleMax())
                        {
                            calibratePlane_y.Append((std_y.TupleMax().D * 2 - hFunction1D_y.GetYValueFunct1d(wcsPlane_y[i].D, "mirror")));
                        }
                        else
                            calibratePlane_y.Append(hFunction1D_y.GetYValueFunct1d(wcsPlane_y[i].D, "mirror"));
                    }
                }
                else
                {
                    index_x = FindIndex(x_range, wcsPlane_x[i]);
                    std_y = this.hMatrix_y_std.GetValueMatrix(HTuple.TupleGenSequence(0, rowCount - 1, 1), HTuple.TupleGenConst(rowCount, index_x)) * -1; //这里不能用绝对值
                    cur_y = this.hMatrix_y_cur.GetValueMatrix(HTuple.TupleGenSequence(0, rowCount - 1, 1), HTuple.TupleGenConst(rowCount, index_x)) * -1;
                    ///////////// 获取Y坐标
                    hFunction1D_y = new HFunction1D(cur_y, std_y);
                    if (wcsPlane_y[i].D * -1 < cur_y.TupleMin())
                    {
                        calibratePlane_y.Append((std_y.TupleMin().D * 2 - hFunction1D_y.GetYValueFunct1d(wcsPlane_y[i].D * -1, "mirror")) * -1);
                    }
                    else
                    {
                        if (wcsPlane_y[i].D * -1 > cur_y.TupleMax())
                        {
                            calibratePlane_y.Append((std_y.TupleMax().D * 2 - hFunction1D_y.GetYValueFunct1d(wcsPlane_y[i].D * -1, "mirror")) * -1);
                        }
                        else
                            calibratePlane_y.Append(hFunction1D_y.GetYValueFunct1d(wcsPlane_y[i].D * -1, "mirror") * -1);
                    }
                }

            }

        }
        public void GetCalibratePointsFromWorldPlaneToCalibratePlane(HTuple wcsPlane_x, HTuple wcsPlane_y, out HTuple calibratePlane_x, out HTuple calibratePlane_y)
        {
            HTuple std_x, std_y, cur_x, cur_y, x_range, y_range;
            int rowCount, colCount;
            HFunction1D hFunction1D_x, hFunction1D_y;
            if (hMatrix_x_std == null)
            {
                calibratePlane_x = wcsPlane_x;
                calibratePlane_y = wcsPlane_y;
                return;
            }
            this.hMatrix_x_std.GetSizeMatrix(out rowCount, out colCount);
            if (rowCount == 0)
            {
                calibratePlane_x = wcsPlane_x;
                calibratePlane_y = wcsPlane_y;
            }
            ///////////
            x_range = this.hMatrix_x_cur.GetValueMatrix(HTuple.TupleGenConst(colCount, 0), HTuple.TupleGenSequence(0, colCount - 1, 1));
            y_range = this.hMatrix_y_cur.GetValueMatrix(HTuple.TupleGenSequence(0, rowCount - 1, 1), HTuple.TupleGenConst(rowCount, 0));
            calibratePlane_x = new HTuple();
            calibratePlane_y = new HTuple();
            int index_x = 0;
            int index_y = 0;
            for (int i = 0; i < wcsPlane_x.Length; i++)
            {
                if (x_range[0] < x_range[x_range.Length - 1]) // 表示数据为升序排列
                {
                    index_y = FindIndex(y_range * -1, wcsPlane_y[i].D * -1);
                    std_x = this.hMatrix_x_std.GetValueMatrix(HTuple.TupleGenConst(colCount, index_y), HTuple.TupleGenSequence(0, colCount - 1, 1));
                    cur_x = this.hMatrix_x_cur.GetValueMatrix(HTuple.TupleGenConst(colCount, index_y), HTuple.TupleGenSequence(0, colCount - 1, 1));
                    ///////////////////////// 获取X坐标
                    hFunction1D_x = new HFunction1D(cur_x, std_x);// X坐标只能升序排列
                    if (wcsPlane_x[i].D < cur_x.TupleMin())
                    {
                        calibratePlane_x.Append(std_x.TupleMin().D * 2 - hFunction1D_x.GetYValueFunct1d(wcsPlane_x[i].D, "mirror"));
                    }
                    else
                    {
                        if (wcsPlane_x[i].D > cur_x.TupleMax())
                        {
                            calibratePlane_x.Append(std_x.TupleMax().D * 2 - hFunction1D_x.GetYValueFunct1d(wcsPlane_x[i].D, "mirror"));
                        }
                        else
                            calibratePlane_x.Append(hFunction1D_x.GetYValueFunct1d(wcsPlane_x[i].D, "mirror"));
                    }
                }
                else // 表示数据为降序排列
                {
                    index_y = FindIndex(y_range * -1, wcsPlane_y[i].D * -1);
                    std_x = this.hMatrix_x_std.GetValueMatrix(HTuple.TupleGenConst(colCount, index_y), HTuple.TupleGenSequence(0, colCount - 1, 1)) * -1;
                    cur_x = this.hMatrix_x_cur.GetValueMatrix(HTuple.TupleGenConst(colCount, index_y), HTuple.TupleGenSequence(0, colCount - 1, 1)) * -1;
                    ///////////////////////// 获取X坐标
                    hFunction1D_x = new HFunction1D(cur_x, std_x);// X坐标只能升序排列
                    if (wcsPlane_x[i].D * -1 < cur_x.TupleMin())
                    {
                        calibratePlane_x.Append((std_x.TupleMin().D * 2 - hFunction1D_x.GetYValueFunct1d(wcsPlane_x[i].D, "mirror")) * -1);
                    }
                    else
                    {
                        if (wcsPlane_x[i].D * -1 > cur_x.TupleMax())
                        {
                            calibratePlane_x.Append((std_x.TupleMax().D * 2 - hFunction1D_x.GetYValueFunct1d(wcsPlane_x[i].D, "mirror")) * -1);
                        }
                        else
                            calibratePlane_x.Append(hFunction1D_x.GetYValueFunct1d(wcsPlane_x[i].D, "mirror") * -1);
                    }
                }
                if (y_range[0] < y_range[y_range.Length - 1]) // 选择相应的轮廓，索引是相反的
                {
                    index_x = FindIndex(x_range, wcsPlane_x[i]);
                    std_y = this.hMatrix_y_std.GetValueMatrix(HTuple.TupleGenSequence(0, rowCount - 1, 1), HTuple.TupleGenConst(rowCount, index_x)); //这里不能用绝对值
                    cur_y = this.hMatrix_y_cur.GetValueMatrix(HTuple.TupleGenSequence(0, rowCount - 1, 1), HTuple.TupleGenConst(rowCount, index_x));
                    ///////////// 获取Y坐标
                    hFunction1D_y = new HFunction1D(cur_y, std_y);
                    if (wcsPlane_y[i].D < cur_y.TupleMin())
                    {
                        calibratePlane_y.Append((std_y.TupleMin().D * 2 - hFunction1D_y.GetYValueFunct1d(wcsPlane_y[i].D, "mirror")));
                    }
                    else
                    {
                        if (wcsPlane_y[i].D > cur_y.TupleMax())
                        {
                            calibratePlane_y.Append((std_y.TupleMax().D * 2 - hFunction1D_y.GetYValueFunct1d(wcsPlane_y[i].D, "mirror")));
                        }
                        else
                            calibratePlane_y.Append(hFunction1D_y.GetYValueFunct1d(wcsPlane_y[i].D, "mirror"));
                    }
                }
                else
                {
                    index_x = FindIndex(x_range, wcsPlane_x[i]);
                    std_y = this.hMatrix_y_std.GetValueMatrix(HTuple.TupleGenSequence(0, rowCount - 1, 1), HTuple.TupleGenConst(rowCount, index_x)) * -1; //这里不能用绝对值
                    cur_y = this.hMatrix_y_cur.GetValueMatrix(HTuple.TupleGenSequence(0, rowCount - 1, 1), HTuple.TupleGenConst(rowCount, index_x)) * -1;
                    ///////////// 获取Y坐标
                    hFunction1D_y = new HFunction1D(cur_y, std_y);
                    if (wcsPlane_y[i].D * -1 < cur_y.TupleMin())
                    {
                        calibratePlane_y.Append((std_y.TupleMin().D * 2 - hFunction1D_y.GetYValueFunct1d(wcsPlane_y[i].D * -1, "mirror")) * -1);
                    }
                    else
                    {
                        if (wcsPlane_y[i].D * -1 > cur_y.TupleMax())
                        {
                            calibratePlane_y.Append((std_y.TupleMax().D * 2 - hFunction1D_y.GetYValueFunct1d(wcsPlane_y[i].D * -1, "mirror")) * -1);
                        }
                        else
                            calibratePlane_y.Append(hFunction1D_y.GetYValueFunct1d(wcsPlane_y[i].D * -1, "mirror") * -1);
                    }
                }
            }

        }
        public void GetCalibratePointsFromCalibratePlaneToWorldPlane(HTuple calibratePlane_x, HTuple calibratePlane_y, out HTuple wcsPlane_x, out HTuple wcsPlane_y)
        {
            HTuple std_x, std_y, cur_x, cur_y, x_range, y_range;
            int rowCount, colCount;
            HFunction1D hFunction1D_x, hFunction1D_y;
            if (hMatrix_x_std == null)
            {
                wcsPlane_x = calibratePlane_x;
                wcsPlane_y = calibratePlane_y;
                return;
            }
            this.hMatrix_x_std.GetSizeMatrix(out rowCount, out colCount);
            if (rowCount == 0)
            {
                wcsPlane_x = calibratePlane_x;
                wcsPlane_y = calibratePlane_y;
            }
            ///////////
            x_range = this.hMatrix_x_std.GetValueMatrix(HTuple.TupleGenConst(colCount, 0), HTuple.TupleGenSequence(0, colCount - 1, 1));
            y_range = this.hMatrix_y_std.GetValueMatrix(HTuple.TupleGenSequence(0, rowCount - 1, 1), HTuple.TupleGenConst(rowCount, 0));
            wcsPlane_x = new HTuple();
            wcsPlane_y = new HTuple();
            int index_x = 0;
            int index_y = 0;
            for (int i = 0; i < calibratePlane_x.Length; i++)
            {
                if (x_range[0] < x_range[x_range.Length - 1]) // 表示数据为升序排列
                {
                    index_y = FindIndex(y_range * -1, calibratePlane_y[i].D * -1);
                    std_x = this.hMatrix_x_std.GetValueMatrix(HTuple.TupleGenConst(colCount, index_y), HTuple.TupleGenSequence(0, colCount - 1, 1));
                    cur_x = this.hMatrix_x_cur.GetValueMatrix(HTuple.TupleGenConst(colCount, index_y), HTuple.TupleGenSequence(0, colCount - 1, 1));
                    ///////////////////////// 获取X坐标
                    hFunction1D_x = new HFunction1D(std_x, cur_x);// X坐标只能升序排列
                    if (calibratePlane_x[i].D < std_x.TupleMin())
                    {
                        wcsPlane_x.Append(cur_x.TupleMin().D * 2 - hFunction1D_x.GetYValueFunct1d(calibratePlane_x[i].D, "mirror"));
                    }
                    else
                    {
                        if (calibratePlane_x[i].D > std_x.TupleMax())
                        {
                            wcsPlane_x.Append(cur_x.TupleMax().D * 2 - hFunction1D_x.GetYValueFunct1d(calibratePlane_x[i].D, "mirror"));
                        }
                        else
                            wcsPlane_x.Append(hFunction1D_x.GetYValueFunct1d(calibratePlane_x[i].D, "mirror"));
                    }
                }
                else // 表示数据为降序排列
                {
                    index_y = FindIndex(y_range * -1, calibratePlane_y[i].D * -1);
                    std_x = this.hMatrix_x_std.GetValueMatrix(HTuple.TupleGenConst(colCount, index_y), HTuple.TupleGenSequence(0, colCount - 1, 1)) * -1;
                    cur_x = this.hMatrix_x_cur.GetValueMatrix(HTuple.TupleGenConst(colCount, index_y), HTuple.TupleGenSequence(0, colCount - 1, 1)) * -1;
                    ///////////////////////// 获取X坐标
                    hFunction1D_x = new HFunction1D(std_x, cur_x);// X坐标只能升序排列
                    if (calibratePlane_x[i].D * -1 < std_x.TupleMin())
                    {
                        wcsPlane_x.Append((cur_x.TupleMin().D * 2 - hFunction1D_x.GetYValueFunct1d(calibratePlane_x[i].D, "mirror")) * -1);
                    }
                    else
                    {
                        if (calibratePlane_x[i].D * -1 > std_x.TupleMax())
                        {
                            wcsPlane_x.Append((cur_x.TupleMax().D * 2 - hFunction1D_x.GetYValueFunct1d(calibratePlane_x[i].D, "mirror")) * -1);
                        }
                        else
                            wcsPlane_x.Append(hFunction1D_x.GetYValueFunct1d(calibratePlane_x[i].D, "mirror") * -1);
                    }
                }
                ////////
                if (y_range[0] < y_range[y_range.Length - 1]) // 选择相应的轮廓，索引是相反的
                {
                    index_x = FindIndex(x_range, calibratePlane_x[i]);
                    std_y = this.hMatrix_y_std.GetValueMatrix(HTuple.TupleGenSequence(0, rowCount - 1, 1), HTuple.TupleGenConst(rowCount, index_x)); //这里不能用绝对值
                    cur_y = this.hMatrix_y_cur.GetValueMatrix(HTuple.TupleGenSequence(0, rowCount - 1, 1), HTuple.TupleGenConst(rowCount, index_x));
                    ///////////// 获取Y坐标
                    hFunction1D_y = new HFunction1D(std_y, cur_y);
                    if (calibratePlane_y[i].D < std_y.TupleMin())
                    {
                        wcsPlane_y.Append((cur_y.TupleMin().D * 2 - hFunction1D_y.GetYValueFunct1d(calibratePlane_y[i].D, "mirror")));
                    }
                    else
                    {
                        if (calibratePlane_y[i].D > std_y.TupleMax())
                        {
                            wcsPlane_y.Append((cur_y.TupleMax().D * 2 - hFunction1D_y.GetYValueFunct1d(calibratePlane_y[i].D, "mirror")));
                        }
                        else
                            wcsPlane_y.Append(hFunction1D_y.GetYValueFunct1d(calibratePlane_y[i].D, "mirror"));
                    }
                }
                else
                {
                    index_x = FindIndex(x_range, calibratePlane_x[i]);
                    std_y = this.hMatrix_y_std.GetValueMatrix(HTuple.TupleGenSequence(0, rowCount - 1, 1), HTuple.TupleGenConst(rowCount, index_x)) * -1; //这里不能用绝对值
                    cur_y = this.hMatrix_y_cur.GetValueMatrix(HTuple.TupleGenSequence(0, rowCount - 1, 1), HTuple.TupleGenConst(rowCount, index_x)) * -1;
                    ///////////// 获取Y坐标
                    hFunction1D_y = new HFunction1D(std_y, cur_y);
                    if (calibratePlane_y[i].D * -1 < std_y.TupleMin())
                    {
                        wcsPlane_y.Append((cur_y.TupleMin().D * 2 - hFunction1D_y.GetYValueFunct1d(calibratePlane_y[i].D * -1, "mirror")) * -1);
                    }
                    else
                    {
                        if (wcsPlane_y[i].D * -1 > std_y.TupleMax())
                        {
                            wcsPlane_y.Append((cur_y.TupleMax().D * 2 - hFunction1D_y.GetYValueFunct1d(calibratePlane_y[i].D * -1, "mirror")) * -1);
                        }
                        else
                            wcsPlane_y.Append(hFunction1D_y.GetYValueFunct1d(calibratePlane_y[i].D * -1, "mirror") * -1);
                    }
                }


            }

        }
        private int FindIndex(HTuple data, double value)
        {
            int index = 0;
            double halfWidth = ((data[1] - data[0]) * 0.5).D;
            if (value <= data.TupleMin())
            {
                index = 0; //表示当前值在定义域的最左边
            }
            else
            {
                if (value >= data.TupleMax())
                {
                    index = data.Length - 1; // 表示当前值在定义域的最右边
                }
                //////////////////////
                else
                {
                    for (int i = 0; i < data.Length; i++)
                    {
                        if (value >= data[i] - halfWidth && value <= data[i] + halfWidth) // 表示当前值在定义域内
                        {
                            index = i;
                            break;
                        }
                    }
                }
            }
            return index;
        }


        private void CalibrateValueToSourceValueAxisX(double calibrateValue, out double sourceValue)
        {
            if (this.calibrate_x == null || this.calibrate_x.Count == 0)
                sourceValue = calibrateValue;
            else
            {
                // 这么做是为了兼容多段补偿
                XyValuePairs[] Value = this.calibrate_x.Values.ToArray();
                sourceValue = GetYValueFunct1d(Value[0].StdValue, Value[0].CurValue, calibrateValue);

                //HFunction1D invertHFunction1D = this._calibrateAxis_x.InvertFunct1d();
                //sourceValue = invertHFunction1D.GetYValueFunct1d(calibrateValue, "mirror");
            }
        }
        private void CalibrateValueToSourceValueAxisY(double calibrateValue, out double sourceValue)
        {
            if (this.calibrate_y == null || this.calibrate_y.Count == 0)
                sourceValue = calibrateValue;
            else
            {
                // 这么做是为了兼容多段补偿
                XyValuePairs[] Value = this.calibrate_y.Values.ToArray();
                sourceValue = GetYValueFunct1d(Value[0].StdValue, Value[0].CurValue, calibrateValue);

                //HFunction1D invertHFunction1D = this._calibrateAxis_y.InvertFunct1d();
                //sourceValue = invertHFunction1D.GetYValueFunct1d(calibrateValue, "mirror");
            }
        }
        private void CalibrateValueToSourceValueAxisZ(double calibrateValue, out double sourceValue)
        {
            if (this.calibrate_z == null || this.calibrate_z.Count == 0)
                sourceValue = calibrateValue;
            else
            {
                XyValuePairs[] Value = this.calibrate_z.Values.ToArray();
                sourceValue = GetYValueFunct1d(Value[0].StdValue, Value[0].CurValue, calibrateValue);

               // HFunction1D invertHFunction1D = this._calibrateAxis_z.InvertFunct1d();
               // sourceValue = invertHFunction1D.GetYValueFunct1d(calibrateValue, "mirror");
            }
        }

        public void CalibrateValueToSourceValueAxisXY(double calibrateValue_x, double calibrateValue_y, out double sourceValue_x, out double sourceValue_y)
        { 
            ////////////////////  补偿X轴
            if (this.calibrate_x == null || this.calibrate_x.Count == 0)
                sourceValue_x = calibrateValue_x;
            else
            {
                double[] keysValue = this.calibrate_x.Keys.ToArray();
                XyValuePairs[] Value = this.calibrate_x.Values.ToArray();
                int index = 0;
                double diff_value = 100000;
                for (int i = 0; i < keysValue.Length; i++) // 查找距离最近的值
                {
                    if(Math.Abs(calibrateValue_y - keysValue[i]) < diff_value)
                    {
                        diff_value = Math.Abs(calibrateValue_y - keysValue[i]);
                        index = i;
                    }
                }
                sourceValue_x = GetYValueFunct1d(Value[index].StdValue, Value[index].CurValue, calibrateValue_x);
            }
            ////////////////////  补偿Y轴
            if (this.calibrate_y == null || this.calibrate_y.Count == 0)
                sourceValue_y = calibrateValue_y;
            else
            {
                double[] keysValue = this.calibrate_y.Keys.ToArray();
                XyValuePairs[] Value = this.calibrate_y.Values.ToArray();
                int index = 0;
                double diff_value = 100000;
                for (int i = 0; i < keysValue.Length; i++) // 查找距离最近的值
                {
                    if (Math.Abs(calibrateValue_x - keysValue[i]) < diff_value)
                    {
                        diff_value = Math.Abs(calibrateValue_x - keysValue[i]);
                        index = i;
                    }
                }
                sourceValue_y = GetYValueFunct1d(Value[index].StdValue, Value[index].CurValue, calibrateValue_y);
            }
        }


        private void SourceValueToCalibrateValueAxisX(double sourceValue, out double calibrateValue)
        {
            if (this.calibrate_x == null || this.calibrate_x.Count == 0)
                calibrateValue = sourceValue;
            else
            {
                // 这么做是为了兼容多段补偿
                XyValuePairs[] Value = this.calibrate_x.Values.ToArray();
                calibrateValue = GetYValueFunct1d(Value[0].CurValue, Value[0].StdValue, sourceValue);
                //calibrateValue = this._calibrateAxis_x.GetYValueFunct1d(sourceValue, "mirror");
            }
        }
        private void SourceValueToCalibrateValueAxisY(double sourceValue, out double calibrateValue)
        {
            if (this.calibrate_y == null || this.calibrate_y.Count == 0)
                calibrateValue = sourceValue;
            else
            {
                // 这么做是为了兼容多段补偿
                XyValuePairs[] Value = this.calibrate_y.Values.ToArray();
                calibrateValue = GetYValueFunct1d(Value[0].CurValue, Value[0].StdValue, sourceValue);
                //calibrateValue = this._calibrateAxis_y.GetYValueFunct1d(sourceValue, "mirror");
            }
        }
        private void SourceValueToCalibrateValueAxisZ(double sourceValue, out double calibrateValue)
        {
            if (this.calibrate_z == null || this.calibrate_z.Count == 0)
                calibrateValue = sourceValue;
            else
            {
                XyValuePairs[] Value = this.calibrate_z.Values.ToArray();
                calibrateValue = GetYValueFunct1d(Value[0].CurValue, Value[0].StdValue, sourceValue);
                // calibrateValue = this._calibrateAxis_z.GetYValueFunct1d(sourceValue, "mirror");
            }
        }

        public void SourceValueToCalibrateValueAxisXY(double sourceValue_x, double sourceValue_y, out double calibrateValue_x, out double calibrateValue_y)
        {
            ////////////////////  补偿X轴
            if (this.calibrate_x == null || this.calibrate_x.Count == 0)
                calibrateValue_x = sourceValue_x;
            else
            {
                double[] keysValue = this.calibrate_x.Keys.ToArray();
                XyValuePairs[] Value = this.calibrate_x.Values.ToArray();
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
            if (this.calibrate_y == null || this.calibrate_y.Count == 0)
                calibrateValue_y = sourceValue_y;
            else
            {
                double[] keysValue = this.calibrate_y.Keys.ToArray();
                XyValuePairs[] Value = this.calibrate_y.Values.ToArray();
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
                case "x":
                case "X":
                case "X轴":
                    CalibrateValueToSourceValueAxisX(calibrateValue, out sourceValue);
                    break;
                case "y":
                case "Y":
                case "Y轴":
                    CalibrateValueToSourceValueAxisY(calibrateValue, out sourceValue);
                    break;
                case "z":
                case "Z":
                case "Z轴":
                    CalibrateValueToSourceValueAxisZ(calibrateValue, out sourceValue);
                    break;
            }
        }
        public void SourceValueToCalibrateValue(string AxisName, double sourceValue, out double calibrateValue)
        {
            switch (AxisName)
            {
                default:
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
            }
        }

        //public bool Save(string fileName)
        //{
        //    BinaryFormatter binFormat = new BinaryFormatter(); //
        //    try
        //    {
        //        using (FileStream fStream = new FileStream(fileName, FileMode.Create, FileAccess.ReadWrite))
        //        {
        //            binFormat.Serialize(fStream, this);
        //        }
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        //MessageBox.Show(ex.ToString());
        //        LoggerHelper.Error("保存校准文件失败", ex);
        //        throw;
        //        //return false;
        //    }
        //}
        //public CalibrationXyPlane Read(string fileName)
        //{
        //    string path = fileName;
        //    if (!File.Exists(path)) return null;
        //    BinaryFormatter binFormat = new BinaryFormatter();
        //    try
        //    {
        //        using (Stream fStream = File.OpenRead(path))
        //        {
        //            return (CalibrationXyPlane)binFormat.Deserialize(fStream);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        //MessageBox.Show(ex.ToString());
        //        LoggerHelper.Error("读取校准文件失败", ex);
        //        throw;
        //        //return null;
        //    }
        //}

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

        public bool Save(string foldPath)
        {
            //bool IsOk = true;
            //if (!DirectoryEx.Exist(foldPath)) DirectoryEx.Create(foldPath);
            //IsOk = IsOk && XML<CalibrationXyPlane>.Save(this, foldPath + @"\" + "CalibrateParam.xml");
            //// 如果可能XMA化，那到就直接XAMl保存，否则只能用自带的方法
            //return IsOk;
            this._calibrateAxis_x?.WriteFunct1d(foldPath + @"\" + "CalibrateParam_x");
            this._calibrateAxis_y?.WriteFunct1d(foldPath + @"\" + "CalibrateParam_y");
            this._calibrateAxis_z?.WriteFunct1d(foldPath + @"\" + "CalibrateParam_z");
            return true;
        }
        public CalibrationXyPlaneOld Read(string foldPath)
        {
            //CalibrationXyPlane CameraList0 = XML<CalibrationXyPlane>.Read(foldPath + @"\" + "CalibrateParam.xml");
            //if (CameraList0 == null) CameraList0 = new CalibrationXyPlane();
            //return CameraList0;

            this._calibrateAxis_x = new HFunction1D();
            this._calibrateAxis_y = new HFunction1D();
            this._calibrateAxis_z = new HFunction1D();
            ///////////////
            this._calibrateAxis_x.ReadFunct1d(foldPath + @"\" + "CalibrateParam_x");
            this._calibrateAxis_y.ReadFunct1d(foldPath + @"\" + "CalibrateParam_y");
            this._calibrateAxis_z.ReadFunct1d(foldPath + @"\" + "CalibrateParam_z");

            return this;

        }

    }

}

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
    [Serializable]
    public class AngleLineLine3D : BaseFunction, IFunction
    {
        private double[] result;
        private double stdValue = 0;
        private double upTolerance = 0;
        private double downTolerance = 0;
        private string[] state;
        public double StdValue { get => stdValue; set => stdValue = value; }
        public double UpTolerance { get => upTolerance; set => upTolerance = value; }
        public double DownTolerance { get => downTolerance; set => downTolerance = value; }
        public AngleLineLine3D()
        {
            //this.ResultDataTable.Rows.Add(this.name, "角度", 0, this.stdValue, this.upTolerance, this.downTolerance, 0);
            //this.ResultDataTable.ColumnChanged += new DataColumnChangeEventHandler(resultDataTable_DataColumnChange);
        }

        private HObjectModel3D[] extractRefSource1Data()
        {
            List<HObjectModel3D> listObjectModel3D = new List<HObjectModel3D>();
            object object3D = null;
            // 获取所有3D对象模型
            foreach (var item in this.RefSource1.Keys)
            {
                if (item.Split('.').Length == 1)
                    object3D = this.RefSource1[item].GetPropertyValues(item);
                else
                    object3D = this.RefSource1[item].GetPropertyValues(item.Split('.')[1]);
                ////////////////////////////////////////////////////
                if (object3D != null) // 这样做是为了动态获取名称
                {
                    switch (object3D.GetType().Name)
                    {
                        case "HTuple":
                            listObjectModel3D.Add(new HObjectModel3D(((HTuple)object3D).IP));
                            break;
                        case "HObjectModel3D":
                            listObjectModel3D.Add((HObjectModel3D)object3D);
                            break;
                        case "HObjectModel3D[]":
                            listObjectModel3D.AddRange((HObjectModel3D[])object3D); // HObjectModel3D.UnionObjectModel3d((HObjectModel3D[])object3D, "points_surface");
                            break;
                    }
                }
            }
            return listObjectModel3D.ToArray();
        }
        private HObjectModel3D[] extractRefSource2Data()
        {
            List<HObjectModel3D> listObjectModel3D = new List<HObjectModel3D>();
            object object3D = null;
            // 获取所有3D对象模型
            foreach (var item in this.RefSource2.Keys)
            {
                if (item.Split('.').Length == 1)
                    object3D = this.RefSource2[item].GetPropertyValues(item);
                else
                    object3D = this.RefSource2[item].GetPropertyValues(item.Split('.')[1]);
                //////////////////////////////
                if (object3D != null) // 这样做是为了动态获取名称
                {
                    switch (object3D.GetType().Name)
                    {
                        case "HTuple":
                            listObjectModel3D.Add(new HObjectModel3D(((HTuple)object3D).IP));
                            break;
                        case "HObjectModel3D":
                            listObjectModel3D.Add((HObjectModel3D)object3D);
                            break;
                        case "HObjectModel3D[]":
                            listObjectModel3D.AddRange((HObjectModel3D[])object3D); // HObjectModel3D.UnionObjectModel3d((HObjectModel3D[])object3D, "points_surface");
                            break;
                    }
                }
            }
            return listObjectModel3D.ToArray();
        }

        private bool AngleLineLine(HObjectModel3D[] line1_objectModel, HObjectModel3D[] line2_objectModel, out double[] deg)
        {
            bool result = false;
            deg = new double[0];
            if (line1_objectModel == null) throw new ArgumentNullException("line1_objectModel");
            if (line2_objectModel == null) throw new ArgumentNullException("line2_objcetModel");
            if (line1_objectModel.Length != line2_objectModel.Length) throw new ArgumentException("两参数的长度不一致");
            ///////////////////////////
            double RowBegin, ColBegin, RowEnd, ColEnd, Row2Begin, Col2Begin, Row2End, Col2End;
            HalconLibrary ha = new HalconLibrary();
            //////////////////////
            deg = new double[line1_objectModel.Length];
            for (int i = 0; i < line1_objectModel.Length; i++)
            {
                if (line1_objectModel[i].GetObjectModel3dParams("num_points").I == 0) throw new ArgumentException("对象中不包含数据点", "planeObjectModel1");
                if (line2_objectModel[i].GetObjectModel3dParams("num_points").I == 0) throw new ArgumentException("对象中不包含数据点", "planeObjectModel2");
                ha.FitLine3D(line1_objectModel[i], 3, out RowBegin, out ColBegin, out RowEnd, out ColEnd);
                ha.FitLine3D(line2_objectModel[i], 3, out Row2Begin, out Col2Begin, out Row2End, out Col2End);
                /////////////////////////////////////////////////////////////////
                double angle = HMisc.AngleLl(RowBegin, ColBegin, RowEnd, ColEnd, Row2Begin, Col2Begin, Row2End, Col2End);
                if (Math.Abs(180 / Math.PI * angle) > 90)
                    deg[i] = 180 - Math.Abs(180 / Math.PI * angle);
                else
                    deg[i] = Math.Abs(180 / Math.PI * angle);
            }
            result = true;
            return result;
        }
        private bool AngleLineToLine(HObjectModel3D[] line1_objectModel, HObjectModel3D[] line2_objcetModel, out double[] deg)
        {
            bool result = false;
            deg = null;
            ///////////////////////////
            HalconLibrary ha = new HalconLibrary();
            HTuple line1_x, line1_y, line1_z, line2_x, line2_y, line2_z, num1, num2;
            HObject Line1Contour = null, Line2Contour = null;
            HTuple RowBegin, ColBegin, RowEnd, ColEnd, Row2Begin, Col2Begin, Row2End, Col2End, Nr, Nc, Dist;
            HTuple angle = 0;
            HTuple Sort_line1X, Sort_line1Y, Sort_line2X, Sort_line2Y;
            ////////////////////////////
            try
            {
                if (line1_objectModel == null || line2_objcetModel == null) // 都执行一次合并
                {
                    deg = new double[0];
                    return result;
                }
                int length = line1_objectModel.Length > line2_objcetModel.Length ? line2_objcetModel.Length : line1_objectModel.Length;
                deg = new double[length];
                //////////////////////
                for (int i = 0; i < length; i++)
                {
                    // 测量对象
                    HOperatorSet.GetObjectModel3dParams(line1_objectModel[i], "point_coord_x", out line1_x);
                    HOperatorSet.GetObjectModel3dParams(line1_objectModel[i], "point_coord_y", out line1_y);
                    HOperatorSet.GetObjectModel3dParams(line1_objectModel[i], "point_coord_z", out line1_z);
                    HOperatorSet.GetObjectModel3dParams(line1_objectModel[i], "num_points", out num1);
                    //////////////////////// 基准直线对象
                    HOperatorSet.GetObjectModel3dParams(line2_objcetModel[i], "point_coord_x", out line2_x);
                    HOperatorSet.GetObjectModel3dParams(line2_objcetModel[i], "point_coord_y", out line2_y);
                    HOperatorSet.GetObjectModel3dParams(line2_objcetModel[i], "point_coord_z", out line2_z);
                    HOperatorSet.GetObjectModel3dParams(line2_objcetModel[i], "num_points", out num2);
                    if (num1 < 2 || num2 < 2)
                    {
                        MessageBox.Show("计算的对象点数小于等于2");
                        deg[i] = 0;
                        continue;
                    }
                    // 第一种情况，两对象都为2D轮廓
                    if (line1_z.TupleMean().D > 0.001 && line2_z.TupleMean().D > 0.001) // 如果Z值的和都为0，表示该轮廓已是2D轮廓
                    {
                        for (int j = 0; j < line1_z.Length; j++)
                            line1_y[j] = line1_z[j];
                        for (int j = 0; j < line2_z.Length; j++)
                            line2_y[j] = line2_z[j];
                    }
                    else
                    {
                        ha.sortPairs(line1_x, line1_y, 1, out Sort_line1X, out Sort_line1Y); // 排一次序，便与轮廓绘制
                        ha.sortPairs(line2_x, line2_y, 1, out Sort_line2X, out Sort_line2Y); // 排一次序，便与轮廓绘制
                        /////////////////////////////////////////////////////////////////
                        HOperatorSet.GenContourPolygonXld(out Line1Contour, Sort_line1Y, Sort_line1X);
                        HOperatorSet.GenContourPolygonXld(out Line2Contour, Sort_line2Y, Sort_line2X);
                        HOperatorSet.FitLineContourXld(Line1Contour, "tukey", -1, 0, 5, 2, out RowBegin, out ColBegin, out RowEnd, out ColEnd, out Nr, out Nc, out Dist);
                        HOperatorSet.FitLineContourXld(Line2Contour, "tukey", -1, 0, 5, 2, out Row2Begin, out Col2Begin, out Row2End, out Col2End, out Nr, out Nc, out Dist);
                        HOperatorSet.AngleLl(RowBegin, ColBegin, RowEnd, ColEnd, Row2Begin, Col2Begin, Row2End, Col2End, out angle);
                        if (Math.Abs(angle.TupleDeg().D) > 90)
                            deg[i] = 180 - Math.Abs(angle.TupleDeg().D);
                        else
                            deg[i] = Math.Abs(angle.TupleDeg().D);
                    }
                }
                result = true;
            }
            catch
            {
                //throw new HalconException();
            }
            return result;
        }

        private void resultDataTable_DataColumnChange(object sender, DataColumnChangeEventArgs e)
        {
            switch (e.Column.ColumnName)
            {
                case "标准值":
                    if (!double.TryParse(e.Row[e.Column].ToString(), out this.stdValue))
                        MessageBox.Show("输入的数据无效");
                    break;
                case "上偏差":
                    if (!double.TryParse(e.Row[e.Column].ToString(), out this.upTolerance))
                        MessageBox.Show("输入的数据无效");
                    break;
                case "下偏差":
                    if (!double.TryParse(e.Row[e.Column].ToString(), out this.downTolerance))
                        MessageBox.Show("输入的数据无效");
                    break;
            }
        }


        #region  实现接口的部分
        public OperateResult Execute(params object[] param)
        {
            this.Result.Succss = false;
            try
            {
                Result.Succss = AngleLineLine(extractRefSource1Data(), extractRefSource2Data(), out this.result);
                //this.ResultDataTable.Clear();
                this.state = new string[this.result.Length];
                for (int i = 0; i < this.result.Length; i++)
                {
                    if (this.stdValue + this.downTolerance <= this.result[i] && this.result[i] <= this.stdValue + this.upTolerance)
                        this.state[i] = "OK";
                    else
                        this.state[i] = "NG";
                    //this.ResultDataTable.Rows.Add(this.name, "角度", this.result[i], this.stdValue, this.upTolerance, this.downTolerance,  this.state[i]);
                }
                OnExcuteCompleted(this.name, new HTuple(extractRefSource1Data(), extractRefSource2Data()));
                ///////////////////////
            }
            catch (Exception ex)
            {
                LoggerHelper.Error(this.name + "->执行错误" + ex);
                return Result;
            }
            if (Result.Succss)
                LoggerHelper.Info(this.name + "->执行成功");
            else
                LoggerHelper.Error(this.name + "->执行失败");
            // 更改UI字体　
            UpdataNodeElementStyle(param, Result.Succss);
            return Result;
        }

        public object GetPropertyValues(string propertyName)
        {
            switch (propertyName)
            {
                case "名称":
                    return this.name;
                case "直线3D对象1":
                    return extractRefSource1Data();
                case "直线3D对象2":
                    return extractRefSource2Data();
                case "角度":
                    return this.result; //
                default:
                    if (this.name == propertyName)
                        return this.result;
                    else return null;
            }
        }
        public bool SetPropertyValues(string propertyName, params object[] value)
        {
            switch (propertyName)
            {
                case "名称":
                    this.name = value[0].ToString();
                    return true;
                default:
                    return true;
            }
        }
        public void ReleaseHandle()
        {
            try
            {
                //this.ResultDataTable.ColumnChanged -= new DataColumnChangeEventHandler(resultDataTable_DataColumnChange);
                OnItemDeleteEvent(this, this.name);
            }
            catch
            {
                LoggerHelper.Error(this.name + "->删除该对象报错");
            }
        }
        public void Read(string path)
        {
            //throw new NotImplementedException();
        }
        public void Save(string path)
        {
            //throw new NotImplementedException();
        }




        #endregion

        public enum enShowItems
        {
            输入对象,
            输出对象,
        }
    }
}

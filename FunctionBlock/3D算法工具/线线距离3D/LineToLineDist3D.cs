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
    public class LineToLineDist3D : BaseFunction, IFunction
    {
        private int average_count = 5;
        private double[] max_dist_value;
        private double[] min_dist_value;
        private double[] mean_dist_value;
        private enDistOrientation distOrientation = enDistOrientation.XYZ;

        private double stdValue = 0;
        private double stdMaxValue = 0;
        private double stdMinValue = 0;
        private double upTolerance = 0;
        private double downTolerance = 0;
        private string state;
        public double StdValue { get => stdValue; set => stdValue = value; }
        public double UpTolerance { get => upTolerance; set => upTolerance = value; }
        public double DownTolerance { get => downTolerance; set => downTolerance = value; }
        public double StdMinValue { get => stdMinValue; set => stdMinValue = value; }
        public double StdMaxValue { get => stdMaxValue; set => stdMaxValue = value; }
        public int Average_count
        {
            get
            {
                return average_count;
            }

            set
            {
                average_count = value;
            }
        }
        public enDistOrientation DistOrientation { get => distOrientation; set => distOrientation = value; }




        public LineToLineDist3D()
        {
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

        private bool CalculaeLineToLineDist3D(HObjectModel3D[] targetObjectModel, HObjectModel3D[] refLineObjcetModel, out double[] maxValue, out double[] minValue, out double[] meanValue)
        {
            bool result = false;
            maxValue = new double[0];
            minValue = new double[0];
            meanValue = new double[0];
            ///////////////////////////
            if (targetObjectModel == null || targetObjectModel.Length == 0)
            {
                throw new ArgumentNullException("targetObjectModel", "参数为空或长度为0");
            }
            if (refLineObjcetModel == null || refLineObjcetModel.Length == 0)
            {
                throw new ArgumentNullException("lineObjcetModel", "参数为空或长度为0");
            }
            if (targetObjectModel.Length > 1 && refLineObjcetModel.Length != targetObjectModel.Length)
            {
                throw new ArgumentException("两参数的长度不一致,当两对象长度不一致时，第一个对象长度只能为1");
            }
            double start_y1, start_x1, start_z1, start_y2, start_x2, start_z2,end_x1,end_y1,end_z1, end_x2, end_y2, end_z2;
            double[] x, y;
            HalconLibrary ha = new HalconLibrary();
            ////////////////////////////
            maxValue = new double[refLineObjcetModel.Length];
            minValue = new double[refLineObjcetModel.Length];
            meanValue = new double[refLineObjcetModel.Length];
            for (int i = 0; i < targetObjectModel.Length; i++)
            {
                if (targetObjectModel.Length > 1)
                {
                    if (targetObjectModel[i].GetObjectModel3dParams("num_points").I == 0)
                    {
                        throw new ArgumentException("targetObjectModel对象中不包含数据点", "targetObjectModel");
                    }
                }
                else
                {
                    if (targetObjectModel[0].GetObjectModel3dParams("num_points").I == 0)
                    {
                        throw new ArgumentException("targetObjectModel对象中不包含数据点", "targetObjectModel");
                    }
                }
                if (refLineObjcetModel[i].GetObjectModel3dParams("num_points").I == 0)
                {
                    throw new ArgumentException("refLineObjcetModel对象中不包含数据点", "refLineObjcetModel");
                }
                HTuple value=0;
                switch (this.distOrientation)
                {
                    case enDistOrientation.Y:
                        // 拟合目标直线对象1
                        if (targetObjectModel.Length > 1)
                            ha.FitLine3D(targetObjectModel[i],"XY", out start_x1, out start_y1, out start_z1, out end_x1, out end_y1, out end_z1);
                        else
                            ha.FitLine3D(targetObjectModel[0], "XY", out start_x1, out start_y1, out start_z1, out end_x1, out end_y1, out end_z1);
                        // 拟合参考直线对象2
                        ha.FitLine3D(refLineObjcetModel[i], "XY", out start_x2, out start_y2, out start_z2, out end_x2, out end_y2, out end_z2);
                        /////////////////////////////////////////////////////
                        value = HMisc.DistancePl((start_y1 + end_y1) * 0.5, (end_x1 + start_x1) * 0.5, start_y2, start_x2, end_y2, end_x2); // 点到线，实际是中点到直线的距离
                        break;
                    case enDistOrientation.XZ:
                        // 拟合目标直线对象1
                        if (targetObjectModel.Length > 1)
                            ha.FitLine3D(targetObjectModel[i], "XZ", out start_x1, out start_y1, out start_z1, out end_x1, out end_y1, out end_z1);
                        else
                            ha.FitLine3D(targetObjectModel[0], "XZ", out start_x1, out start_y1, out start_z1, out end_x1, out end_y1, out end_z1);
                        // 拟合参考直线对象2
                        ha.FitLine3D(refLineObjcetModel[i], "XZ", out start_x2, out start_y2, out start_z2, out end_x2, out end_y2, out end_z2);
                        /////////////////////////////////////////////////////
                        value = HMisc.DistancePl((start_z1 + end_z1) * 0.5, (end_x1 + start_x1) * 0.5, start_z2, start_x2, end_z2, end_x2); // 点到线，实际是中点到直线的距离
                        break;
                    case enDistOrientation.YZ:
                        // 拟合目标直线对象1
                        if (targetObjectModel.Length > 1)
                            ha.FitLine3D(targetObjectModel[i], "YZ", out start_x1, out start_y1, out start_z1, out end_x1, out end_y1, out end_z1);
                        else
                            ha.FitLine3D(targetObjectModel[0], "YZ", out start_x1, out start_y1, out start_z1, out end_x1, out end_y1, out end_z1);
                        // 拟合参考直线对象2
                        ha.FitLine3D(refLineObjcetModel[i], "YZ", out start_x2, out start_y2, out start_z2, out end_x2, out end_y2, out end_z2);
                        /////////////////////////////////////////////////////
                        value = HMisc.DistancePl((start_z1 + end_z1) * 0.5, (end_y1 + start_y1) * 0.5, start_z2, start_y2, end_z2, end_y2); // 点到线，实际是中点到直线的距离
                        break;
                    default:
                    case enDistOrientation.XYZ:
                        // 拟合目标直线对象1
                        if (targetObjectModel.Length > 1)
                            ha.FitLine3D(targetObjectModel[i], "XYZ", out start_x1, out start_y1, out start_z1, out end_x1, out end_y1, out end_z1);
                        else
                            ha.FitLine3D(targetObjectModel[0], "XYZ", out start_x1, out start_y1, out start_z1, out end_x1, out end_y1, out end_z1);
                        // 拟合参考直线对象2
                        ha.FitLine3D(refLineObjcetModel[i], "XYZ", out start_x2, out start_y2, out start_z2, out end_x2, out end_y2, out end_z2);
                        /////////////////////////////////////////////////////
                        value = HMisc.DistancePl((start_z1 + end_z1) * 0.5, (start_x1 + end_x1) * 0.5, start_z2, start_x2, end_z2, end_x2); // 点到线，实际是中点到直线的距离
                        break;
                }
                /////////////////////////////
                maxValue[i] =Math.Round( value.D,5);
                minValue[i] = Math.Round(value.D, 5); 
                meanValue[i] = Math.Round(value.D, 5); // 计算所有点的平面值
            }
            result = true;
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
                Result.Succss = CalculaeLineToLineDist3D(extractRefSource1Data(), extractRefSource2Data(), out this.max_dist_value, out this.min_dist_value, out this.mean_dist_value);
                ///////////////////////
               // this.ResultDataTable.Clear();
                for (int i = 0; i < this.mean_dist_value.Length; i++)
                {
                    if ((this.stdValue + this.downTolerance) <= this.mean_dist_value[i] && this.mean_dist_value[i] <= (this.stdValue + this.upTolerance))
                        this.state = "OK";
                    else
                        this.state = "NG";
                    //this.ResultDataTable.Rows.Add(this.name + i.ToString(), "平均值", this.mean_dist_value[i], this.stdValue, this.upTolerance, this.downTolerance, this.state);
                    ////////////////////////////////////
                    if ((this.stdMaxValue + this.downTolerance) <= this.max_dist_value[i] && this.max_dist_value[i] <= (this.stdMaxValue + this.upTolerance))
                        this.state = "OK";
                    else
                        this.state= "NG";
                    //this.ResultDataTable.Rows.Add(this.name + i.ToString(), "最大值", this.max_dist_value[i], this.stdValue, this.upTolerance, this.downTolerance, this.state);
                    ////////////////////////
                    if ((this.stdMinValue + this.downTolerance) <= this.min_dist_value[i] && this.min_dist_value[i] <= (this.stdMinValue + this.upTolerance))
                        this.state = "OK";
                    else
                        this.state = "NG";
                   // this.ResultDataTable.Rows.Add(this.name + i.ToString(), "最小值", this.min_dist_value[i], this.stdValue, this.upTolerance, this.downTolerance, this.state);
                }
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
                case "最大值":
                    return this.max_dist_value; //
                case "最小值":
                    return this.min_dist_value; //
                case "平均值":
                    return this.mean_dist_value; //
                case "输入直线3D对象1":
                    return extractRefSource1Data(); // 输入点云3D对象
                case "输入直线3D对象2":
                    return extractRefSource2Data(); // 输入点云3D对象
                default:
                    if (this.name == propertyName)
                        return this.mean_dist_value;
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
               // this.ResultDataTable.ColumnChanged -= new DataColumnChangeEventHandler(resultDataTable_DataColumnChange);
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
            输入直线3D对象1,
            输入直线3D对象2,
        }
    }
}

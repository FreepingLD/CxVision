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
using System.Drawing;

namespace FunctionBlock
{
    [Serializable]
    public class NumericalCalculation : BaseFunction, IFunction, INotifyPropertyChanged
    {
        private double[] resultValue;
        private enOperateMethod operateMethod = enOperateMethod.加;

        private double stdValue = 0;
        private double upTolerance = 0;
        private double downTolerance = 0;
        private string state = "OK";
        public double StdValue { get => stdValue; set => stdValue = value; }
        public double UpTolerance { get => upTolerance; set => upTolerance = value; }
        public double DownTolerance { get => downTolerance; set => downTolerance = value; }
        public enOperateMethod OperateMethod { get => operateMethod; set => operateMethod = value; }

        private void GetResultValue(Dictionary<string, IFunction> RefSource, out List<double[]> dataList)
        {
            dataList = new List<double[]>();
            object dataSource = null;
            BaseFunction _function;
            DataRow[] dataRows;
            string defaulteName;
            // 获取所有3D对象模型
            foreach (var item in RefSource.Keys)
            {
                if (RefSource[item] is OutputData) return;
                if (item.Split('.').Length == 1)// 如果拖进的是父节点，其处理方法
                {
                    _function = (BaseFunction)RefSource[item];
                    dataSource = RefSource[item].GetPropertyValues(item);
                    /////////////
                   // defaulteName = _function.ResultDataTable.Rows[0]["属性名称"].ToString();
                    //dataRows = _function.ResultDataTable.Select("属性名称 =" + "'" + defaulteName + "'"); //获取单元格名称为：defaulteName的所有row集合
                }
                else // 如果拖进的是子节点，其处理方法
                {
                    _function = (BaseFunction)RefSource[item];
                    dataSource = RefSource[item].GetPropertyValues(item.Split('.')[1]);
                    defaulteName = item.Split('.')[1];
                    //dataRows = _function.ResultDataTable.Select("属性名称 =" + "'" + defaulteName + "'");
                }
                //if (dataRows != null)
                //{
                //    List<double> listValue = new List<double>();
                //    for (int i = 0; i < dataRows.Length; i++)
                //    {
                //        if (dataRows[i][0] == dataRows[0][0]) // 表示属于同一个产品的测量数据
                //        {
                //            listValue.Add(Convert.ToDouble(dataRows[i]["测量值"]));
                //        }
                //        else
                //        {
                //            if (listValue.Count > 0)
                //            {
                //                dataList.Add(listValue.ToArray()); // 每一个数组表示一个产品的数据
                //                listValue.Clear();
                //            }
                //            dataList.Add(new double[] { Convert.ToDouble(dataRows[i]["测量值"]) }); // 每一个数组表示一个产品的数据
                //        }
                //    }
                //    if (listValue.Count > 0)
                //    {
                //        dataList.Add(listValue.ToArray());
                //        listValue.Clear();
                //    }
                //}
            }
        }
        public NumericalCalculation()
        {
            //this.ResultDataTable.ColumnChanged += new DataColumnChangeEventHandler(resultDataTable_DataColumnChange);
        }

        private double[] extractRefSource1Data()
        {
            List<double> listObjectModel3D = new List<double>();
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
                        case "Double":
                            listObjectModel3D.Add((Double)object3D);
                            break;
                        case "Double[]":
                            listObjectModel3D.AddRange((Double[])object3D); // HObjectModel3D.UnionObjectModel3d((HObjectModel3D[])object3D, "points_surface");
                            break;
                    }
                }
            }
            return listObjectModel3D.ToArray();
        }
        private double[] extractRefSource2Data()
        {
            List<double> listObjectModel3D = new List<double>();
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
                        case "Double":
                            listObjectModel3D.Add((double)object3D);
                            break;
                        case "Double[]":
                            listObjectModel3D.AddRange((double[])object3D);
                            break;
                    }
                }
            }
            return listObjectModel3D.ToArray();
        }

        public bool Calculation(double[] data1, double[] data2, out double[] distValue)
        {
            bool result = false;
            distValue = new double[0];
            if (data1 == null || data1.Length == 0)
            {
                throw new ArgumentNullException("planeObjectModel1", "参数为空或长度为0");
            }
            if (data2 == null || data2.Length == 0)
            {
                throw new ArgumentNullException("planeObjectModel2", "参数为空或长度为0");
            }
            if ((data1.Length != data2.Length))
            {
                throw new ArgumentException("两参数的长度不一致");
            }
            int length = Math.Min(data1.Length, data2.Length);
            /////////////////////////
            switch (this.operateMethod)
            {
                default:
                case enOperateMethod.加:
                    distValue = (new HTuple(data1).TupleAdd(new HTuple(data2)).DArr);
                    result = true;
                    break;
                case enOperateMethod.减:
                    distValue = (new HTuple(data1).TupleSub(new HTuple(data2)).DArr);
                    result = true;
                    break;
                case enOperateMethod.乘:
                    distValue = (new HTuple(data1).TupleMult(new HTuple(data2)).DArr);
                    result = true;
                    break;
                case enOperateMethod.除:
                    distValue = (new HTuple(data1).TupleDiv(new HTuple(data2)).DArr);
                    result = true;
                    break;
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
                Result.Succss = Calculation(extractRefSource1Data(), extractRefSource2Data(), out this.resultValue);
                //this.ResultDataTable.Clear();
                for (int i = 0; i < this.resultValue.Length; i++)
                {
                    if (this.stdValue + this.downTolerance <= this.resultValue[i] && this.resultValue[i] <= this.stdValue + this.upTolerance)
                        this.state = "OK";
                    else
                        this.state = "NG";
                    //this.ResultDataTable.Rows.Add(this.name, "结果", this.resultValue[i], this.stdValue, this.upTolerance, this.downTolerance, this.state);
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
                case "平均值":
                case "距离":
                    return this.resultValue; //
                case "输入对象1":
                    return extractRefSource1Data();
                case "输入对象2":
                    return extractRefSource2Data();
                default:
                    if (this.name == propertyName)
                        return this.resultValue; // 默认返回值为绘制的距离对象，用于点击操作
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
           // throw new NotImplementedException();
        }



        #endregion

        public enum enShowItems
        {
            输入对象1,
            输入对象2,
            输出对象,
        }

    }
}

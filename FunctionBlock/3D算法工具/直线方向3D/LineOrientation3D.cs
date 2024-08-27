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
    public class LineOrientation3D : BaseFunction, IFunction
    {
        private double[] Rx;
        private double[] Ry;
        private double[] Rz;


        private double stdValue = 0;
        private double upTolerance = 0;
        private double downTolerance = 0;
        private string state;
        public double StdValue { get => stdValue; set => stdValue = value; }
        public double UpTolerance { get => upTolerance; set => upTolerance = value; }
        public double DownTolerance { get => downTolerance; set => downTolerance = value; }


        public LineOrientation3D()
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

        public bool LineOrientation(HObjectModel3D[] line1_objectModel, out double[] Rx, out double[] Ry, out double[] Rz)
        {
            bool result = false;
            HalconLibrary ha = new HalconLibrary();
            ///////////////////////////
            if (line1_objectModel == null)
            {
                throw new ArgumentNullException("line1_objectModel");
            }
            if (line1_objectModel.Length == 0)
            {
                throw new ArgumentException("line1_objectModel对象中不包含元素");
            }
            Rx = new double[line1_objectModel.Length];
            Ry = new double[line1_objectModel.Length];
            Rz = new double[line1_objectModel.Length];
            double start_x, start_y, start_z, end_x, end_y, end_z;
            for (int i = 0; i < line1_objectModel.Length; i++)
            {
                ha.FitLine3D(line1_objectModel[i], out start_x, out start_y, out start_z, out end_x, out end_y, out end_z);
                Rx[i] = Math.Atan2(end_y - start_y, end_x - start_x) * 180 / Math.PI;
                Ry[i] = Math.Atan2(end_x - start_x, end_y - start_y) * 180 / Math.PI;
                Rz[i] = Math.Atan2(end_z - start_z, Math.Sqrt((end_x - start_x) * (end_x - start_x) + (end_y - start_y) * (end_y - start_y))) * 180 / Math.PI;
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
                Result.Succss = LineOrientation(extractRefSource1Data(), out this.Rx, out this.Ry, out this.Rz);
                //this.ResultDataTable.Clear();
                for (int i = 0; i < this.Rx.Length; i++)
                {
                    if (this.stdValue + this.downTolerance <= this.Rx[i] && this.Rx[i] <= this.stdValue + this.upTolerance)
                        this.state = "OK";
                    else
                        this.state = "NG";
                    //this.ResultDataTable.Rows.Add(this.name + i.ToString(), "角度X", this.Rx[i], this.stdValue, this.upTolerance, this.downTolerance, this.state);
                    ////////////////////////////////////////////
                    if (this.stdValue + this.downTolerance <= this.Ry[i] && this.Ry[i] <= this.stdValue + this.upTolerance)
                        this.state = "OK";
                    else
                        this.state = "NG";
                   // this.ResultDataTable.Rows.Add(this.name + i.ToString(), "角度Y", this.Ry[i], this.stdValue, this.upTolerance, this.downTolerance, this.state);
                    /////////////////////////////////////////////////
                    if (this.stdValue + this.downTolerance <= this.Rz[i] && this.Rz[i] <= this.stdValue + this.upTolerance)
                        this.state = "OK";
                    else
                        this.state = "NG";
                    //this.ResultDataTable.Rows.Add(this.name + i.ToString(), "角度Z", this.Rz[i], this.stdValue, this.upTolerance, this.downTolerance, this.state);
                }
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
                case "源对象":
                case "输入对象":
                    return extractRefSource1Data(); //
                case "角度X":
                    return this.Rx; //
                case "角度Y":
                    return this.Ry; //
                case "角度Z":
                    return this.Rz; //
                default:
                    if (this.name == propertyName)
                        return this.Rz;
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
            输入3D对象,
        }
    }
}

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
using System.ComponentModel;
using AlgorithmsLibrary;
using Common;
using System.Data;


namespace FunctionBlock
{
    /// <summary>
    /// 体积的计算是需要基准面的
    /// </summary>
    [Serializable]
    public class LineLength3D : BaseFunction, IFunction
    {

        private double length;

        private double stdValue = 0;
        private double upTolerance = 0;
        private double downTolerance = 0;
        private string  state ;
        public double StdValue { get => stdValue; set => stdValue = value; }
        public double UpTolerance { get => upTolerance; set => upTolerance = value; }
        public double DownTolerance { get => downTolerance; set => downTolerance = value; }


        public LineLength3D()
        {
            //this.ResultDataTable.Rows.Add(this.name, "3D直线长度", 0, this.stdValue, this.upTolerance, this.downTolerance, 0);
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
        public userWcsCoordSystem extractRefSource2Data()
        {
            userWcsCoordSystem wcsPose = new userWcsCoordSystem();
            object object3D = null;
            // 获取所有3D对象模型
            foreach (var item in this.RefSource2.Keys)
            {
                if (item.Split('.').Length == 1)
                    object3D = this.RefSource2[item].GetPropertyValues(item);
                else
                    object3D = this.RefSource2[item].GetPropertyValues(item.Split('.')[1]);
                //////////////////////////////
                if (object3D != null)
                {
                    switch (object3D.GetType().Name)
                    {
                        case "userWcsPose":
                            wcsPose = new userWcsCoordSystem((userWcsPose)object3D);
                            break;
                        case "userWcsCoordSystem":
                            wcsPose = ((userWcsCoordSystem)object3D);
                            break;
                        case "userWcsCoordSystem[]":
                            wcsPose = ((userWcsCoordSystem[])object3D)[0];
                            break;
                    }
                }
            }
            return wcsPose;
        }


        public bool CalculateLineLength(HObjectModel3D[] objectModel, out double length)
        {
            bool result = false;
            length = 0;
            if (objectModel == null || objectModel.Length == 0) return result;
            ////////////////
            HTuple x = new HTuple();
            HTuple y = new HTuple();
            HTuple z = new HTuple();
            double sum = 0;
            if (objectModel.Length == 1)
            {
                x = objectModel[0].GetObjectModel3dParams("point_coord_x");
                y = objectModel[0].GetObjectModel3dParams("point_coord_y");
                z = objectModel[0].GetObjectModel3dParams("point_coord_z");
                for (int i = 0; i < x.Length - 1; i++)
                {
                    sum += Math.Sqrt((x[i + 1] - x[i]) * (x[i + 1] - x[i]) + (y[i + 1] - y[i]) * (y[i + 1] - y[i]) + (z[i + 1] - z[i]) * (z[i + 1] - z[i]));
                }
                length = sum;
            }
            else
            {
                for (int i = 0; i < objectModel.Length; i++)
                {
                    if (objectModel[i].GetObjectModel3dParams("num_points").I > 0)
                    {
                        x.Append(objectModel[i].GetObjectModel3dParams("point_coord_x").TupleMean());
                        y.Append(objectModel[i].GetObjectModel3dParams("point_coord_y").TupleMean());
                        z.Append(objectModel[i].GetObjectModel3dParams("point_coord_z").TupleMean());
                    }
                }
                for (int i = 0; i < x.Length - 1; i++)
                {
                    sum += Math.Sqrt((x[i + 1] - x[i]) * (x[i + 1] - x[i]) + (y[i + 1] - y[i]) * (y[i + 1] - y[i]) + (z[i + 1] - z[i]) * (z[i + 1] - z[i]));
                }
                length = sum;
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


        #region 实现接口
        public OperateResult Execute(params object[] param)
        {
           this.Result.Succss = false;
            try
            {
                ////////////////////////
                Result.Succss = CalculateLineLength(extractRefSource1Data(), out this.length);
                //this.ResultDataTable.Clear();
                if (this.stdValue + this.downTolerance <= this.length && this.length <= this.stdValue + this.upTolerance)
                    this.state = "OK";
                else
                    this.state = "NG";
                //this.ResultDataTable.Rows.Add(this.name, "3D直线长度", this.length, this.stdValue, this.upTolerance, this.downTolerance, this.state);
                //////////////////////////////////
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
                case "源3D对象":
                case "输入3D对象":
                    return extractRefSource1Data(); // 
                case "长度3D":
                    return this.length; // 
                default:
                    if (this.name == propertyName)
                        return this.length;
                    else return 0;
            }
        }
        public bool SetPropertyValues(string propertyName, params object[] value)
        {
            switch (propertyName)
            {
                case "名称":
                    this.name = value[0].ToString();
                    return true;
                //////////////////////              
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
            输入3D对象,
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sensor;
using MotionControlCard;
using HalconDotNet;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using AlgorithmsLibrary;
using System.ComponentModel;
using Common;
using System.Data;
namespace FunctionBlock
{

    /// <summary>
    /// 计算平面度，并返回平面度的值及位姿
    /// </summary>
    [Serializable]
    public class Straightness3D : BaseFunction, IFunction
    {
        private double[] value; // 测量结果
        private HObjectModel3D[] dataHandle3D = null;
        private double[] Y;

        private double stdValue = 0;
        private double upTolerance = 0;
        private double downTolerance = 0;
        private string[] state;
        public double StdValue { get => stdValue; set => stdValue = value; }
        public double UpTolerance { get => upTolerance; set => upTolerance = value; }
        public double DownTolerance { get => downTolerance; set => downTolerance = value; }

        public Straightness3D()
        {
            // this.ResultDataTable.Rows.Add(this.name, "直线度", 0, this.stdValue, this.upTolerance, this.downTolerance, 0);
           // this.ResultDataTable.ColumnChanged += new DataColumnChangeEventHandler(resultDataTable_DataColumnChange);
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
                    object3D = this.RefSource1[item.Split('.')[0]].GetPropertyValues(item.Split('.')[1]);
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
                            listObjectModel3D.AddRange((HObjectModel3D[])object3D);
                            break;
                        case "userWcsPose3D": // 如果是位姿，将其转换成3D对象
                            HObjectModel3D plane3D = new HObjectModel3D();
                            plane3D.GenPlaneObjectModel3d(((userWcsPose)object3D).GetHPose(), new HTuple(), new HTuple());
                            listObjectModel3D.Add(plane3D);
                            break;
                    }
                }
            }
            return listObjectModel3D.ToArray();
        }
        private bool CalculateStraightness(HObjectModel3D[] objectModel3D, out double[] straightness, out HObjectModel3D[] dataHandle3D)
        {
            bool result = false;
            straightness = new double[0];
            HHomMat2D hHomMat2D;
            HTuple tar_x, tar_y, tar_z;
            HTuple Qx = new HTuple();
            HTuple Qy = new HTuple();
            double[] x, y, z;
            /////////////////////////////
            if (objectModel3D == null)
            {
                straightness = new double[0];
                dataHandle3D = new HObjectModel3D[0];
                return result;// 都执行一次合并
            }
            /////////////////////////////
            straightness = new double[objectModel3D.Length];
            dataHandle3D = new HObjectModel3D[objectModel3D.Length];
            for (int i = 0; i < objectModel3D.Length; i++)
            {
                tar_x = objectModel3D[i].GetObjectModel3dParams("point_coord_x");
                tar_y = objectModel3D[i].GetObjectModel3dParams("point_coord_y");
                tar_z = objectModel3D[i].GetObjectModel3dParams("point_coord_z");
                if (Math.Abs(tar_z.TupleMean().D) > 0.01)
                {
                    double dist;
                    x = new double[tar_x.Length];
                    y = new double[tar_x.Length];
                    z = new double[tar_x.Length];
                    for (int ii = 0; ii < tar_x.Length; ii++)
                    {
                        dist = Math.Sqrt((tar_x[ii].D - tar_x[0].D) * (tar_x[ii].D - tar_x[0].D) + (tar_y[ii].D - tar_y[0].D) * (tar_y[ii].D - tar_y[0].D));
                        x[ii] = dist;
                        y[ii] = tar_z[ii];
                        z[ii] = 0.0;
                    }
                }
                else
                {
                    x = tar_x.DArr;
                    y = tar_y.DArr;
                    z = tar_z.DArr;
                }
                hHomMat2D = new HHomMat2D();
                for (int ii = 0; ii < 3; ii++)
                {
                    hHomMat2D.VectorToRigid(x, y, x, z);
                    Qx = hHomMat2D.AffineTransPoint2d(new HTuple(x), new HTuple(y), out Qy);
                    x = Qx;
                    y = Qy;
                }
                straightness[i] = (Qy.TupleMax() - Qy.TupleMin()).D;
                dataHandle3D[i] = new HObjectModel3D(Qx, Qy, z);
                //////////////////////////////////////////
                result = true;
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

        #region 实现接口

        public OperateResult Execute(params object[] param)
        {
            this.Result.Succss = false;
            try
            {
                ClearHandle(this.dataHandle3D);
                Result.Succss = CalculateStraightness(extractRefSource1Data(), out this.value, out this.dataHandle3D);
               // this.ResultDataTable.Clear();
                this.state = new string[this.value.Length];
                for (int i = 0; i < this.value.Length; i++)
                {
                    if (this.stdValue + this.downTolerance <= this.value[i] && this.value[i] <= this.stdValue + this.upTolerance)
                        this.state[i] = "OK";
                    else
                        this.state[i] = "NG";
                    //this.ResultDataTable.Rows.Add(this.name + i.ToString(), "直线度", this.value[i], this.stdValue, this.upTolerance, this.downTolerance, this.state); // string.Join(",", this.value)
                    OnExcuteCompleted(this.name + i.ToString(), dataHandle3D);
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
                case "源3D对象":
                case "输入3D对象":
                    return extractRefSource1Data(); //
                case "计算结果":
                    return this.value; //
                default:
                    if (this.name == propertyName)
                        return this.value;
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
                //////////////////////////////////////////////                        
                default:
                    return true;
            }
        }

        public void ReleaseHandle()
        {
            try
            {
                //this.ResultDataTable.ColumnChanged -= new DataColumnChangeEventHandler(resultDataTable_DataColumnChange);
                if (this.dataHandle3D != null)
                {
                    for (int i = 0; i < this.dataHandle3D.Length; i++)
                    {
                        this.dataHandle3D[i].Dispose();
                    }
                }
                OnItemDeleteEvent(this, this.name);
            }
            catch
            {
                LoggerHelper.Error(this.name + "->删除该对象报错");
            }
        }

        public void Read(string path)
        {
           // throw new NotImplementedException();
        }
        public void Save(string path)
        {
          //  throw new NotImplementedException();
        }

        #endregion



        public enum enShowItems
        {
            输入3D对象,
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HalconDotNet;
using System.Drawing;
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
    public class PointToFaceDist : BaseFunction, IFunction
    {
        private int average_count = 5;
        private double[] max_dist_value;
        private double[] min_dist_value;
        private double[] mean_dist_value;

        private double stdValue = 0;
        private double stdMaxValue = 0;
        private double stdMinValue = 0;
        private double upTolerance = 0;
        private double downTolerance = 0;


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


        public PointToFaceDist()
        {
            this.ResultInfo = new BindingList<MeasureResultInfo>();
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo());
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo());
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo());
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

        public bool CalculatePointToFaceDist3D(HObjectModel3D[] pointObjectModel, HObjectModel3D[] planeObjectModel, int average_count, out double[] maxValue, out double[] minValue, out double[] meanValue)
        {
            bool result = false;
            maxValue = new double[0];
            minValue = new double[0];
            meanValue = new double[0];
            ///////////////////////////
            if (pointObjectModel == null )
            {
                throw new ArgumentNullException("targetObjectModel", "参数planeObjectModel为空值");
            }
            if (planeObjectModel == null )
            {
                throw new ArgumentNullException("planeObjectModel", "参数planeObjectModel为空值");
            }
            if (pointObjectModel.Length == 0)
            {
                throw new ArgumentNullException("targetObjectModel", "参数planeObjectModel不包含元素");
            }
            if ( planeObjectModel.Length == 0)
            {
                throw new ArgumentNullException("planeObjectModel", "参数planeObjectModel不包含元素");
            }
            if (planeObjectModel.Length > 1 && pointObjectModel.Length != planeObjectModel.Length)
            {
                throw new ArgumentException("两参数的长度不一致");
            }
            HalconLibrary ha = new HalconLibrary();
            HPose pose;
            ////////////////////////////
            maxValue = new double[pointObjectModel.Length];
            minValue = new double[pointObjectModel.Length];
            meanValue = new double[pointObjectModel.Length];
            for (int i = 0; i < pointObjectModel.Length; i++)
            {
                if (planeObjectModel.Length > 1)
                {
                    if (planeObjectModel[i].GetObjectModel3dParams("has_primitive_data").S == "false" && planeObjectModel[i].GetObjectModel3dParams("num_points").I == 0)
                    {
                        throw new ArgumentException("对象中不包含数据点且对象中不包含基本体对象", "planeObjectModel");
                    }
                }
                else
                {
                    if (planeObjectModel[0].GetObjectModel3dParams("has_primitive_data").S == "false" && planeObjectModel[0].GetObjectModel3dParams("num_points").I == 0)
                    {
                        throw new ArgumentException("对象中不包含数据点且对象中不包含基本体对象", "planeObjectModel");
                    }
                }
                if (pointObjectModel[i].GetObjectModel3dParams("num_points").I == 0)
                {
                    throw new ArgumentException("对象中不包含数据点", "pointObjectModel");
                }
                if (planeObjectModel.Length > 1)
                    ha.GetPlaneObjectModel3DPose(planeObjectModel[i], out pose);
                else
                    ha.GetPlaneObjectModel3DPose(planeObjectModel[0], out pose); // 当只有一个元素时，所有的以他为参考面
                HObjectModel3D ref_plane3D = new HObjectModel3D();
                ref_plane3D.GenPlaneObjectModel3d(pose, new HTuple(), new HTuple()); // 
                // 计算平面到平面距离
                pointObjectModel[i].DistanceObjectModel3d(ref_plane3D, new HPose(), 0, new HTuple("distance_to", "signed_distances"), new HTuple("primitive", "true"));
                HTuple value = pointObjectModel[i].GetObjectModel3dParams("&distance");
                ref_plane3D.Dispose();
                // 计算最大值、最小值、平均值
                if (value.Length > this.average_count)
                {
                    maxValue[i] = ((value.TupleSort().TupleSelectRange(value.Length - average_count, value.Length - 1).TupleSum().D / average_count));
                    minValue[i] = ((value.TupleSort().TupleSelectRange(0, average_count - 1).TupleSum().D / average_count));
                    meanValue[i] = ((value.TupleMean()).D); // 计算所有点的平面值
                }
                else // 当总点数小于平均点数的时候
                {
                    maxValue[i] = ((value.TupleSort().TupleSelect(value.Length - 1)).D);
                    minValue[i] = ((value.TupleSort().TupleSelect(0)).D);
                    meanValue[i] = ((value.TupleMean()).D); // 计算所有点的平面值
                }
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
                Result.Succss = CalculatePointToFaceDist3D(extractRefSource1Data(), extractRefSource2Data(), this.average_count, out this.max_dist_value, out this.min_dist_value, out this.mean_dist_value);
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
                case "输入点云3D对象":
                    return extractRefSource1Data(); // 输入点云3D对象
                case "输入平面3D对象":
                    return extractRefSource2Data(); // 输入点云3D对象
                default:
                    if (this.name == propertyName)
                        return this.max_dist_value;
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
           // throw new NotImplementedException();
        }
        public void Save(string path)
        {
            //throw new NotImplementedException();
        }

        #endregion


        public enum enShowItems
        {
            输入点云3D对象,
            输入平面3D对象,
        }
    }
}

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
    public class PointToLineDist3D : BaseFunction, IFunction
    {
        private int average_count = 5;
        private double [] maxDistValue;
        private double [] minDistValue;
        private double [] meanDistValue;
        [NonSerialized]
        private HObjectModel3D [] dataHandle3D1 = null;
        [NonSerialized]
        private HObjectModel3D [] dataHandle3D2 = null;
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


        [DisplayName("最大距离")]
        [DescriptionAttribute("输出属性")]
        public double[] MaxDistValue { get => maxDistValue; set => maxDistValue = value; }
        [DisplayName("最小距离")]
        [DescriptionAttribute("输出属性")]
        public double[] MinDistValue { get => minDistValue; set => minDistValue = value; }
        [DisplayName("平均距离")]
        [DescriptionAttribute("输出属性")]
        public double[] MeanDistValue { get => meanDistValue; set => meanDistValue = value; }

        [DisplayName("输入3D对象1")]
        [DescriptionAttribute("输入属性1")]
        public HObjectModel3D [] DataHandle3D1
        {
            get
            {
                try
                {
                    if (this.RefSource1.Count > 0)
                    {
                        var oo = this.GetPropertyValue(this.RefSource1);
                        List<HObjectModel3D> list = new List<HObjectModel3D>();
                        if (oo != null)
                        {
                            if (oo.Length > 1)
                            {
                                foreach (var item in oo)
                                {
                                    if (item is HObjectModel3D)
                                        list.Add(item as HObjectModel3D);
                                }
                                this.dataHandle3D1 = list.ToArray();
                            }
                            else
                            {
                                if (oo.Length == 1)
                                {
                                    if (oo.Last() is HObjectModel3D)
                                        this.dataHandle3D1 = new HObjectModel3D[] { (oo.Last() as HObjectModel3D) };
                                }
                            }
                        }
                        else
                        {
                            this.dataHandle3D1 = new HObjectModel3D[0];
                        }
                    }
                    //else
                    //{
                    //    this.dataHandle3D1 = new HObjectModel3D[0];
                    //}
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return dataHandle3D1;
            }
            set
            {
                dataHandle3D1 = value;
            }
        }

        [DisplayName("输入3D对象2")]
        [DescriptionAttribute("输入属性2")]
        public HObjectModel3D [] DataHandle3D2
        {
            get
            {
                try
                {
                    if (this.RefSource2.Count > 0)
                    {
                        var oo = this.GetPropertyValue(this.RefSource2);
                        List<HObjectModel3D> list = new List<HObjectModel3D>();
                        if (oo != null)
                        {
                            if (oo.Length > 1)
                            {
                                foreach (var item in oo)
                                {
                                    if (item is HObjectModel3D)
                                        list.Add(item as HObjectModel3D);
                                }
                                this.dataHandle3D2 = list.ToArray();
                            }
                            else
                            {
                                if (oo.Length == 1)
                                {
                                    if (oo.Last() is HObjectModel3D)
                                        this.dataHandle3D2 = new HObjectModel3D[] { (oo.Last() as HObjectModel3D) };
                                }
                            }
                        }
                        else
                        {
                            this.dataHandle3D2 = new HObjectModel3D[0];
                        }
                    }
                    //else
                    //{
                    //    this.dataHandle3D2 = new HObjectModel3D[0];
                    //}
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return dataHandle3D1;
            }
            set
            {
                dataHandle3D1 = value;
            }
        }


        public PointToLineDist3D()
        {
            this.ResultInfo = new BindingList<MeasureResultInfo>();
        }


        private bool CalculaePointToLine3DDist(HObjectModel3D[] pointObjectModel, HObjectModel3D[] lineObjcetModel, int aveNum, out double[] maxValue, out double[] minValue, out double[] meanValue)
        {
            bool result = false;
            maxValue = new double[0];
            minValue = new double[0];
            meanValue = new double[0];
            ///////////////////////////
            if (pointObjectModel == null)
            {
                throw new ArgumentNullException("pointObjectModel", "参数pointObjectModel为空");
            }
            if (lineObjcetModel == null)
            {
                throw new ArgumentNullException("lineObjcetModel", "参数lineObjcetModel为空");
            }
            if ( pointObjectModel.Length == 0)
            {
                throw new ArgumentNullException("pointObjectModel", "参数pointObjectModel长度为0");
            }
            if ( lineObjcetModel.Length == 0)
            {
                throw new ArgumentNullException("lineObjcetModel", "参数lineObjcetModel长度为0");
            }
            if (lineObjcetModel.Length > 1 && lineObjcetModel.Length != pointObjectModel.Length)
            {
                throw new ArgumentException("两参数pointObjectModel和lineObjcetModel的长度不一致");
            }
            double RowBegin, ColBegin, RowEnd, ColEnd;
            double[] x, y;
            HalconLibrary ha = new HalconLibrary();
            ////////////////////////////
            maxValue = new double[lineObjcetModel.Length];
            minValue = new double[lineObjcetModel.Length];
            meanValue = new double[lineObjcetModel.Length];
            for (int i = 0; i < pointObjectModel.Length; i++)
            {
                if(lineObjcetModel.Length>1)
                {
                    if (lineObjcetModel[i].GetObjectModel3dParams("num_points").I == 0)
                    {
                        throw new ArgumentException("对象中不包含数据点", "lineObjcetModel");
                    }
                }
                else
                {
                    if (lineObjcetModel[0].GetObjectModel3dParams("num_points").I == 0)
                    {
                        throw new ArgumentException("对象中不包含数据点", "lineObjcetModel");
                    }
                }
                if (pointObjectModel[i].GetObjectModel3dParams("num_points").I == 0)
                {
                    throw new ArgumentException("对象中不包含数据点", "targetObjectModel");
                }
                if (lineObjcetModel.Length > 1)
                    ha.FitLine3D(lineObjcetModel[i], 3, out RowBegin, out ColBegin, out RowEnd, out ColEnd);
                else
                    ha.FitLine3D(lineObjcetModel[0], 3, out RowBegin, out ColBegin, out RowEnd, out ColEnd);
                ha.TransformModel3DTo2DPoint(pointObjectModel[i], 3, out x, out y);
                HTuple value = HMisc.DistancePl(y, x, RowBegin, ColBegin, RowEnd, ColEnd);
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




        #region  实现接口的部分
        public OperateResult Execute(params object[] param)
        {
            this.Result.Succss = false;
            try
            {
                this.Result.Succss = CalculaePointToLine3DDist(this.DataHandle3D1, this.DataHandle3D2, this.average_count, out this.maxDistValue, out this.minDistValue, out this.meanDistValue);
                ///////////////////////
                this.ResultInfo = MeasureResultInfo.InitList(this.maxDistValue.Length * 3);
                for (int i = 0; i < this.maxDistValue.Length; i++)
                {
                    ((BindingList<MeasureResultInfo>)this.ResultInfo)[i].SetValue(this.name, "最大距离", this.maxDistValue[i]);
                }
                for (int i = this.maxDistValue.Length; i < this.maxDistValue.Length*2; i++)
                {
                    ((BindingList<MeasureResultInfo>)this.ResultInfo)[i].SetValue(this.name, "最小距离",this.minDistValue[i - this.maxDistValue.Length]);
                }
                for (int i = this.maxDistValue.Length * 2 ; i < this.maxDistValue.Length*3; i++)
                {
                    ((BindingList<MeasureResultInfo>)this.ResultInfo)[i].SetValue(this.name, "平均距离",  this.meanDistValue[i - this.maxDistValue.Length*2]);
                }

            }
            catch (Exception ex)
            {
                LoggerHelper.Error(this.name + "->执行错误" + ex);
                return this.Result;
            }
            if (this.Result.Succss)
                LoggerHelper.Info(this.name + "->执行成功");
            else
                LoggerHelper.Error(this.name + "->执行失败");
            // 更改UI字体　
            UpdataNodeElementStyle(param, this.Result.Succss);
            return this.Result;
        }

        public object GetPropertyValues(string propertyName)
        {
            switch (propertyName)
            {
                case "名称":
                case nameof(this.Name):
                    return this.name;
                default:
                case "最大值":
                case nameof(this.MaxDistValue):
                    return this.maxDistValue; //
                case "最小值":
                case nameof(this.MinDistValue):
                    return this.minDistValue; //
                case "平均值":
                case nameof(this.MeanDistValue):
                    return this.meanDistValue; //
            }
        }
        public bool SetPropertyValues(string propertyName, params object[] value)
        {
            switch (propertyName)
            {
                case "名称":
                case nameof(this.Name):
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
            输入点云3D对象,
            输入直线3D对象,
        }
    }
}

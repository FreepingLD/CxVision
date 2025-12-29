using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HalconDotNet;
using Sensor;
using MotionControlCard;
using System.Windows.Forms;
using Common;
using System.IO;
using System.ComponentModel;
using AlgorithmsLibrary;
using System.Data;
using System.Diagnostics;

namespace FunctionBlock
{
    [Serializable]
    [DefaultProperty(nameof(WcsPlane))]
    public class FittingPlane3D : BaseFunction, IFunction
    {
        [NonSerialized]
        private PointCloudData _dataObjectModel = null;
        private userWcsPlane _wcsPlane;
        private userWcsPose _wcsPose;
        public FitPlaneParam3D FtiParam
        {
            get;
            set;
        }
        public FittingPlane3D()
        {
            this.FtiParam = new FitPlaneParam3D();
            this.ResultInfo = new BindingList<MeasureResultInfo>();
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name));
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name));
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name));
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name));
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name));
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name));
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name));
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name));
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name));
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name));
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name));
        }

        [DisplayName("输入对象3D")]
        [DescriptionAttribute("输入属性1")]
        public PointCloudData DataObjectModel
        {
            get
            {
                try
                {
                    if (this.RefSource1.Count > 0)
                    {
                        List<PointCloudData> listObjectModel3D = new List<PointCloudData>();
                        object[] oo = this.GetPropertyValue(this.RefSource1);
                        if (oo != null && oo.Length > 0)
                        {
                            foreach (var item in oo)
                            {
                                switch (item.GetType().Name)
                                {
                                    case nameof(HTuple):
                                        this._dataObjectModel = (new PointCloudData(new HObjectModel3D(((HTuple)item).IP)));
                                        break;
                                    case nameof(HObjectModel3D):
                                        this._dataObjectModel = (new PointCloudData((HObjectModel3D)item));
                                        break;
                                    case "HObjectModel3D[]":
                                        this._dataObjectModel = (new PointCloudData(((HObjectModel3D[])item)[0]));
                                        break;
                                    case nameof(PointCloudData):
                                        this._dataObjectModel = ((PointCloudData)item);
                                        break;
                                    case "PointCloudData[]":
                                        this._dataObjectModel = ((PointCloudData[])item)[0];
                                        break;
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return this._dataObjectModel;
            }
            set
            {
                this._dataObjectModel = value;
            }
        }

        [DisplayName("平面")]
        [DescriptionAttribute("输出属性")]
        public userWcsPlane WcsPlane { get { return _wcsPlane; } set { this._wcsPlane = value; } }

        [DisplayName("位姿")]
        [DescriptionAttribute("输出属性")]
        public userWcsPose WcsPose { get { return _wcsPose; } set { this._wcsPose = value; } }


        #region  实现接口的部分

        public OperateResult Execute(params object[] param)
        {
            this.Result.Succss = false;
            try
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                string index = "";
                if (param != null)
                {
                    foreach (var item in param)
                    {
                        switch (item.GetType().Name)
                        {

                            case nameof(String):
                                if (item.ToString().Split('=').Length > 0)
                                    index = item.ToString().Split('=').Last();
                                break;
                        }
                    }
                }
                Result.Succss = FitMethod3D.FitPlane(this.DataObjectModel.ObjectModel3D, this.FtiParam, out this._wcsPlane, out this._wcsPose);
                stopwatch.Stop();
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[0].SetValue(this.name, "X", this._wcsPlane.X);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[1].SetValue(this.name, "Y", this._wcsPlane.Y);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[2].SetValue(this.name, "Z", this._wcsPlane.Z);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[3].SetValue(this.name, "Tx", this._wcsPose.Tx);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[4].SetValue(this.name, "Ty", this._wcsPose.Ty);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[5].SetValue(this.name, "Tz", this._wcsPose.Tz);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[6].SetValue(this.name, "Rx", this._wcsPose.Rx);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[7].SetValue(this.name, "Ry", this._wcsPose.Ry);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[8].SetValue(this.name, "Rz", this._wcsPose.Rz);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[9].SetValue(this.name, "Type", this._wcsPose.Type);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[10].SetValue(this.name, "Time(ms)", stopwatch.ElapsedMilliseconds);
                //OnExcuteCompleted(this.name, this.wcsSphere.WcsPose);
                OnExcuteCompleted(this._dataObjectModel.SensorName, this._dataObjectModel?.ViewWindow, this.name + index, this._wcsPlane);
            }
            catch (Exception ex)
            {
                LoggerHelper.Error(this.name + "执行错误" + ex);
                return Result;
            }
            if (Result.Succss)
                LoggerHelper.Info(this.name + "->执行成功");
            else
                LoggerHelper.Error(this.name + "->执行失败");
            // 更改UI字体　
            UpdataNodeElementStyle(param, this.Result.Succss);
            return Result;
        }
        public object GetPropertyValues(string propertyName)
        {
            switch (propertyName)
            {
                case "平面对象": // "可视化项"
                    return this._wcsPlane;
                case "名称":
                    return this.name;
                default:
                    if (this.name == propertyName)
                        return this._wcsPlane;
                    else
                        return null;
            }
        }
        public bool SetPropertyValues(string propertyName, params object[] value)
        {
            try
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
            catch
            {
                return false;
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


    }
}


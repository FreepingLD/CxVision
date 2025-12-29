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
using System.Diagnostics;

namespace FunctionBlock
{
    [Serializable]
    [DefaultProperty(nameof(WcsCircle))]
    public class FitProfileCircle3D : BaseFunction, IFunction
    {
        [NonSerialized]
        private PointCloudData _dataObjectModel = null;
        private userWcsCircle _wcsCircle;
        private userWcsPose _wcsPose;
        public FitProfileParam3D FtiParam
        {
            get;
            set;
        }
        public FitProfileCircle3D()
        {
            this.FtiParam = new FitProfileParam3D();
            this.ResultInfo = new BindingList<MeasureResultInfo>();
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

        [DisplayName("圆")]
        [DescriptionAttribute("输出属性")]
        public userWcsCircle WcsCircle { get { return _wcsCircle; } set { this._wcsCircle = value; } }

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
                Result.Succss = FitMethod3D.FitProfile(this.DataObjectModel.ObjectModel3D, this.FtiParam, out this._wcsCircle);
                stopwatch.Stop();
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[0].SetValue(this.name, "X", this._wcsCircle.X);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[1].SetValue(this.name, "Y", this._wcsCircle.Y);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[2].SetValue(this.name, "Z", this._wcsCircle.Z);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[3].SetValue(this.name, "Time(ms)", stopwatch.ElapsedMilliseconds);
                //OnExcuteCompleted(this.name, this.wcsSphere.WcsPose);
                OnExcuteCompleted(this._dataObjectModel.SensorName, this._dataObjectModel?.ViewWindow, this.name + index, this._wcsCircle);
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
                case "名称":
                    return this.name;
                case "输出对象":
                case "圆对象":
                    return this._wcsCircle; //
                default:
                    if (this.name == propertyName)
                        return this._wcsCircle;
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

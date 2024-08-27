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
    [DefaultProperty(nameof(WcsVector))]
    public class VectorComposition : BaseFunction, IFunction
    {
        private userWcsVector _wcsVector;
        private userWcsPoint _wcsPoint;
        private userWcsLine _wcsLine;

        [DisplayName("向量点")]
        [DescriptionAttribute("输出属性")]
        public userWcsVector WcsVector { get => _wcsVector; set => _wcsVector = value; }


        [DisplayName("输入点")]
        [DescriptionAttribute("输入属性1")]
        public userWcsPoint WcsPoint
        {
            get
            {
                if (this.RefSource1.Count > 0)
                {
                    object[] oo = this.GetPropertyValue(this.RefSource1);
                    if (oo != null)
                    {
                        foreach (var item in oo)
                        {
                            switch (item.GetType().Name)
                            {
                                case nameof(userWcsPoint):
                                    this._wcsPoint = item as userWcsPoint;
                                    break;
                                case nameof(userWcsRectangle2):
                                    userWcsRectangle2 wcsRect2 = item as userWcsRectangle2;
                                    this._wcsPoint = new userWcsPoint(wcsRect2.X, wcsRect2.Y, 0, wcsRect2.CamParams);
                                    this._wcsPoint.CamName = wcsRect2.CamName;
                                    this._wcsPoint.Grab_x = wcsRect2.Grab_x;
                                    this._wcsPoint.Grab_y = wcsRect2.Grab_y;
                                    break;
                                case nameof(userWcsCircle):
                                    userWcsCircle wcsCircle = item as userWcsCircle;
                                    this._wcsPoint = new userWcsPoint(wcsCircle.X, wcsCircle.Y, 0, wcsCircle.CamParams);
                                    this._wcsPoint.CamName = wcsCircle.CamName;
                                    this._wcsPoint.Grab_x = wcsCircle.Grab_x;
                                    this._wcsPoint.Grab_y = wcsCircle.Grab_y;
                                    break;
                            }
                        }
                    }
                    else
                        this._wcsPoint = new userWcsPoint();
                }
                else
                    this._wcsPoint = new userWcsPoint();
                return this._wcsPoint;
            }
            set
            {
                this._wcsPoint = value;
            }
        }

        [DisplayName("输入直线")]
        [DescriptionAttribute("输入属性2")]
        public userWcsLine WcsLine
        {
            get
            {
                if (this.RefSource2.Count > 0)
                {
                    object[] oo = this.GetPropertyValue(this.RefSource2);
                    if (oo != null)
                    {
                        foreach (var item in oo)
                        {
                            switch (item.GetType().Name)
                            {
                                case nameof(userWcsLine):
                                    this._wcsLine = item as userWcsLine;
                                    break;
                                case nameof(userPixLine):
                                    userPixLine pixLine  = item as userPixLine;
                                    this._wcsLine = pixLine.GetWcsLine();
                                    break;
                            }
                        }
                    }
                    else
                        this._wcsLine = new userWcsLine();
                }
                else
                    this._wcsLine = new userWcsLine();
                return this._wcsLine;
            }
            set
            {
                this._wcsLine = value;
            }
        }



        public VectorComposition()
        {
            this.ResultInfo = new BindingList<MeasureResultInfo>();
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name, "X", 0));
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name, "Y", 0));
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name, "Z", 0));
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name, "Angle", 0));
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name, "Time(ms)", 0));
        }



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
                Result.Succss = HalconLibrary.CalculateVectorPoint(this.WcsPoint, this.WcsLine, out this._wcsVector);
                stopwatch.Stop();
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[0].SetValue(this.name, "X", this._wcsVector.X);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[0].SetValue(this.name, "Y", this._wcsVector.Y);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[0].SetValue(this.name, "Z", this._wcsVector.Z);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[0].SetValue(this.name, "Angle", this._wcsVector.Angle);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[1].SetValue(this.name, "Time(ms)", stopwatch.ElapsedMilliseconds);
                //OnExcuteCompleted(this.name, _DistLine);
                OnExcuteCompleted(this._wcsVector.CamName, this._wcsVector?.ViewWindow, this.name + index, this._wcsVector);
            }
            catch (Exception ex)
            {
                LoggerHelper.Error(this.name + "->执行报错", ex);
                return Result;
            }
            if (Result.Succss)
                LoggerHelper.Info(this.name + "->执行成功:" + string.Format("Dist={0}", this._wcsVector));
            else
                LoggerHelper.Error(this.name + "->执行失败:" + string.Format("Dist={0}", this._wcsVector));
            // 更改UI字体　
            UpdataNodeElementStyle(param, Result.Succss);
            return Result;
        }
        public object GetPropertyValues(string propertyName)
        {
            switch (propertyName)
            {
                case "名称":
                default:
                case nameof(Name):
                    return this.name;
                case nameof(WcsVector):
                    return this.WcsVector;
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

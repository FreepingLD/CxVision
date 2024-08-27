using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlgorithmsLibrary;
using HalconDotNet;
using System.Data;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Common;
using System.Diagnostics;

namespace FunctionBlock
{
    [Serializable]
    [DefaultProperty(nameof(WcsPoint))]
    public class ProjectionPoint : BaseFunction, IFunction
    {
        private userWcsPoint _wcsPoint;
        private userWcsPoint _WcsPoint1;
        private userWcsLine _WcsLine2;


        [DisplayName("投影点")]
        [DescriptionAttribute("输出属性")]
        public userWcsPoint WcsPoint { get => _wcsPoint; set => _wcsPoint = value; }


        [DisplayName("输入点")]
        [DescriptionAttribute("输入属性1")]
        public userWcsPoint WcsPoint1
        {
            get
            {
                if (this.RefSource1.Count > 0)
                {
                    object[] oo = this.GetPropertyValue(this.RefSource1);
                    if (oo != null && oo.Length > 0)
                    {
                        foreach (var item in oo)
                        {
                            switch (item.GetType().Name)
                            {
                                case nameof(userWcsPoint):
                                    this._WcsPoint1 = item as userWcsPoint;
                                    break;
                                case nameof(userWcsVector):
                                    userWcsVector wcsVector = item as userWcsVector;
                                    this._WcsPoint1 = new userWcsPoint(wcsVector.X, wcsVector.Y, wcsVector.Z, wcsVector.CamParams);
                                    this._WcsPoint1.Grab_x = wcsVector.Grab_x;
                                    this._WcsPoint1.Grab_y = wcsVector.Grab_y;
                                    this._WcsPoint1.Grab_theta = wcsVector.Grab_theta;
                                    break;
                                case nameof(userPixPoint):
                                    userPixPoint pixPoint = item as userPixPoint;
                                    this._WcsPoint1 = pixPoint.GetWcsPoint();
                                    break;
                                case nameof(userWcsRectangle2):
                                    userWcsRectangle2 wcsRect2  = item as userWcsRectangle2;
                                    this._WcsPoint1 = new userWcsPoint(wcsRect2.X, wcsRect2.Y,0, wcsRect2.CamParams);
                                    this._WcsPoint1.Grab_x = wcsRect2.Grab_x;
                                    this._WcsPoint1.Grab_y = wcsRect2.Grab_y;
                                    this._WcsPoint1.Grab_theta = wcsRect2.Grab_theta;
                                    break;
                                case nameof(userWcsCircle):
                                    userWcsCircle wcsCircle  = item as userWcsCircle;
                                    this._WcsPoint1 = new userWcsPoint(wcsCircle.X, wcsCircle.Y, 0, wcsCircle.CamParams);
                                    this._WcsPoint1.Grab_x = wcsCircle.Grab_x;
                                    this._WcsPoint1.Grab_y = wcsCircle.Grab_y;
                                    this._WcsPoint1.Grab_theta = wcsCircle.Grab_theta;
                                    break;
                                case nameof(userWcsCircleSector):
                                    userWcsCircleSector wcsCircleSector  = item as userWcsCircleSector;
                                    this._WcsPoint1 = new userWcsPoint(wcsCircleSector.X, wcsCircleSector.Y, wcsCircleSector.Z, wcsCircleSector.CamParams);
                                    this._WcsPoint1.Grab_x = wcsCircleSector.Grab_x;
                                    this._WcsPoint1.Grab_y = wcsCircleSector.Grab_y;
                                    this._WcsPoint1.Grab_theta = wcsCircleSector.Grab_theta;
                                    break;
                                case nameof(userWcsEllipse):
                                    userWcsEllipse wcsEllipse  = item as userWcsEllipse;
                                    this._WcsPoint1 = new userWcsPoint(wcsEllipse.X, wcsEllipse.Y, wcsEllipse.Z, wcsEllipse.CamParams);
                                    this._WcsPoint1.Grab_x = wcsEllipse.Grab_x;
                                    this._WcsPoint1.Grab_y = wcsEllipse.Grab_y;
                                    this._WcsPoint1.Grab_theta = wcsEllipse.Grab_theta;
                                    break;
                                case nameof(userWcsEllipseSector):
                                    userWcsEllipseSector wcsEllipseSector  = item as userWcsEllipseSector;
                                    this._WcsPoint1 = new userWcsPoint(wcsEllipseSector.X, wcsEllipseSector.Y, wcsEllipseSector.Z, wcsEllipseSector.CamParams);
                                    this._WcsPoint1.Grab_x = wcsEllipseSector.Grab_x;
                                    this._WcsPoint1.Grab_y = wcsEllipseSector.Grab_y;
                                    this._WcsPoint1.Grab_theta = wcsEllipseSector.Grab_theta;
                                    break;
                            }
                        }
                    }
                    if (this._WcsPoint1 == null)
                        this._WcsPoint1 = new userWcsPoint();
                }
                //else
                //    this._WcsLine1 = new userWcsLine();
                return this._WcsPoint1;
            }
            set
            {
                this._WcsPoint1 = value;
            }
        }

        [DisplayName("输入直线")]
        [DescriptionAttribute("输入属性2")]
        public userWcsLine WcsLine2
        {
            get
            {
                if (this.RefSource2.Count > 0)
                {
                    object[] oo = this.GetPropertyValue(this.RefSource2);
                    if (oo != null && oo.Length > 0)
                    {
                        foreach (var item in oo)
                        {
                            switch (item.GetType().Name)
                            {
                                case nameof(userWcsLine):
                                    this._WcsLine2 = item as userWcsLine;
                                    break;
                                case nameof(userPixLine):
                                    userPixLine pixLine = item as userPixLine;
                                    this._WcsLine2 = pixLine.GetWcsLine();
                                    break;
                            }
                        }
                    }
                    if (this._WcsLine2 == null)
                        this._WcsLine2 = new userWcsLine();
                }
                return this._WcsLine2;
            }
            set
            {
                this._WcsLine2 = value;
            }
        }


        public ProjectionPoint()
        {
            this.ResultInfo = new BindingList<MeasureResultInfo>();
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo());
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo());
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo());
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo());
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo());
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo());
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo());
        }




        #region 实现接口

        public OperateResult Execute(params object[] param)
        {
            this.Result.Succss = false;
            try
            {
                string index = "";
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                if (param != null)
                {
                    foreach (var item in param)
                    {
                        if (item == null) continue;
                        switch (item.GetType().Name)
                        {
                            case nameof(String):
                                if (item.ToString().Split('=').Length > 0)
                                    index = item.ToString().Split('=').Last();
                                break;
                        }
                    }
                }
                stopwatch.Stop();
                this.Result.Succss = HalconLibrary.CalculateProjectionPoint(this.WcsPoint1, this.WcsLine2, out this._wcsPoint);
                //this.Result.DataContent = this._wcsLine;
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[0].SetValue(this.name, "X", this._wcsPoint.X);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[1].SetValue(this.name, "Y", this._wcsPoint.Y);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[2].SetValue(this.name, "Z", this._wcsPoint.Z);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[3].SetValue(this.name, "Grab_x", this._wcsPoint.Grab_x);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[4].SetValue(this.name, "Grab_y", this._wcsPoint.Grab_y);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[5].SetValue(this.name, "Grab_theta", this._wcsPoint.Grab_theta);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[6].SetValue(this.name, "Time(ms)", stopwatch.ElapsedMilliseconds);
                OnExcuteCompleted(this.WcsPoint.CamName, this.name + index, this._wcsPoint);
            }
            catch (Exception ex)
            {
                LoggerHelper.Error(this.name + "->执行报错", ex);
                this.Result.Succss = false;
            }
            if (this.Result.Succss)
                LoggerHelper.Info(this.name + "->执行成功:" + this._wcsPoint.ToString());
            else
                LoggerHelper.Error(this.name + "->执行失败:" + this._wcsPoint.ToString());
            // 更改UI字体　
            UpdataNodeElementStyle(param, this.Result.Succss);
            return this.Result;
        }
        public object GetPropertyValues(string propertyName)
        {
            switch (propertyName)
            {
                case "名称":
                default:
                case nameof(Name):
                    return this.name;
                case nameof(WcsPoint):
                    return this._wcsPoint;
            }
        }
        public bool SetPropertyValues(string propertyName, params object[] value)
        {
            HalconLibrary ha = new HalconLibrary();
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
                if (this._wcsPoint != null)
                {
                    OnItemDeleteEvent(this, this.name);
                }
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

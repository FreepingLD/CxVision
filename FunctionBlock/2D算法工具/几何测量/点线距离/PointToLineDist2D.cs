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
    [DefaultProperty(nameof(Dist))]
    public class PointToLineDist2D : BaseFunction, IFunction
    {
        private double _Dist;
        private userWcsLine _DistLine;
        private userWcsPoint _WcsPoint;
        private userWcsLine _WcsLine;

        [DisplayName("距离")]
        [DescriptionAttribute("输出属性")]
        public double Dist { get => _Dist; set => _Dist = value; }


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
                                    this._WcsPoint = item as userWcsPoint;
                                    break;
                                case nameof(userWcsVector):
                                    userWcsVector wcsVector = item as userWcsVector;
                                    this._WcsPoint = new userWcsPoint(wcsVector.X, wcsVector.Y, wcsVector.Z, wcsVector.CamParams);
                                    break;
                                case nameof(userWcsRectangle2):
                                    userWcsRectangle2 wcsRect2 = item as userWcsRectangle2;
                                    this._WcsPoint = new userWcsPoint(wcsRect2.X, wcsRect2.Y, wcsRect2.Z, wcsRect2.CamParams);
                                    this._WcsPoint.CamName = wcsRect2.CamName;
                                    this._WcsPoint.Grab_x = wcsRect2.Grab_x;
                                    this._WcsPoint.Grab_y = wcsRect2.Grab_y;
                                    break;
                                case nameof(userWcsCircle):
                                    userWcsCircle wcsCircle = item as userWcsCircle;
                                    this._WcsPoint = new userWcsPoint(wcsCircle.X, wcsCircle.Y, wcsCircle.Z, wcsCircle.CamParams);
                                    this._WcsPoint.CamName = wcsCircle.CamName;
                                    this._WcsPoint.Grab_x = wcsCircle.Grab_x;
                                    this._WcsPoint.Grab_y = wcsCircle.Grab_y;
                                    break;
                                case nameof(userWcsCircleSector):
                                    userWcsCircleSector wcsCircleSector = item as userWcsCircleSector;
                                    this._WcsPoint = new userWcsPoint(wcsCircleSector.X, wcsCircleSector.Y, wcsCircleSector.Z, wcsCircleSector.CamParams);
                                    this._WcsPoint.CamName = wcsCircleSector.CamName;
                                    this._WcsPoint.Grab_x = wcsCircleSector.Grab_x;
                                    this._WcsPoint.Grab_y = wcsCircleSector.Grab_y;
                                    break;
                                case nameof(userWcsEllipse):
                                    userWcsEllipse wcsEllipse = item as userWcsEllipse;
                                    this._WcsPoint = new userWcsPoint(wcsEllipse.X, wcsEllipse.Y, wcsEllipse.Z, wcsEllipse.CamParams);
                                    this._WcsPoint.CamName = wcsEllipse.CamName;
                                    this._WcsPoint.Grab_x = wcsEllipse.Grab_x;
                                    this._WcsPoint.Grab_y = wcsEllipse.Grab_y;
                                    break;
                                case nameof(userWcsEllipseSector):
                                    userWcsEllipseSector wcsEllipseSector = item as userWcsEllipseSector;
                                    this._WcsPoint = new userWcsPoint(wcsEllipseSector.X, wcsEllipseSector.Y, wcsEllipseSector.Z, wcsEllipseSector.CamParams);
                                    this._WcsPoint.CamName = wcsEllipseSector.CamName;
                                    this._WcsPoint.Grab_x = wcsEllipseSector.Grab_x;
                                    this._WcsPoint.Grab_y = wcsEllipseSector.Grab_y;
                                    break;
                                case nameof(userWcsCoordSystem):
                                    userWcsCoordSystem wcsSys = item as userWcsCoordSystem;
                                    this._WcsPoint = new userWcsPoint(wcsSys.CurrentPoint.X, wcsSys.CurrentPoint.Y, wcsSys.CurrentPoint.Z, wcsSys.CurrentPoint.CamParams);
                                    this._WcsPoint.Grab_x = 0;
                                    this._WcsPoint.Grab_y = 0;
                                    this._WcsPoint.Grab_theta = 0;
                                    break;
                            }
                        }
                    }
                    else
                        this._WcsPoint = new userWcsPoint();
                }
                else
                    this._WcsPoint = new userWcsPoint();
                return this._WcsPoint;
            }
            set
            {
                this._WcsPoint = value;
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
                    if (oo != null && oo.Length > 0)
                    {
                        foreach (var item in oo)
                        {
                            switch (item.GetType().Name)
                            {
                                case nameof(userWcsLine):
                                    this._WcsLine = item as userWcsLine;
                                    break;
                                case nameof(userPixLine):
                                    userPixLine pixLine = item as userPixLine;
                                    this._WcsLine = pixLine.GetWcsLine();
                                    break;
                            }
                        }
                    }
                    else
                        this._WcsLine = new userWcsLine();
                }
                else
                    this._WcsLine = new userWcsLine();
                return this._WcsLine;
            }
            set
            {
                this._WcsLine = value;
            }
        }

        [DisplayName("距离直线")]
        [DescriptionAttribute("输出属性")]
        public userWcsLine DistLine { get => _DistLine; set => _DistLine = value; }


        public PointToLineDist2D()
        {
            this.ResultInfo = new BindingList<MeasureResultInfo>();
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name, "距离", 0));
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
                Result.Succss = HalconLibrary.CalculatePointToLineDist2D(this.WcsPoint, this.WcsLine, out this._Dist, out this._DistLine);
                stopwatch.Stop();
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[0].SetValue(this.name, "点到线距离", this._Dist);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[1].SetValue(this.name, "Time(ms)", stopwatch.ElapsedMilliseconds);
                //OnExcuteCompleted(this.name, _DistLine);
                OnExcuteCompleted(this._DistLine.CamName, this._DistLine?.ViewWindow, this.name + index, this._DistLine);
            }
            catch (Exception ex)
            {
                LoggerHelper.Error(this.name + "->执行报错", ex);
                return Result;
            }
            if (Result.Succss)
                LoggerHelper.Info(this.name + "->执行成功:" + string.Format("Dist={0}", this._Dist));
            else
                LoggerHelper.Error(this.name + "->执行失败:" + string.Format("Dist={0}", this._Dist));
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
                case nameof(Dist):
                    return this.Dist;
                case nameof(DistLine):
                    return this.DistLine;
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
                if (this._DistLine != null)
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

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
    [DefaultProperty(nameof(DistLine))]
    public class PointToCoordSysDist2D : BaseFunction, IFunction, INotifyPropertyChanged
    {
        private double _levelDist;
        private double _verticalDist;
        private userWcsLine _distLine1;
        private userWcsLine _distLine2;
        private userWcsLine _distLine;
        private userWcsPoint _wcsPoint;
        private userWcsCoordSystem _coordSystem;
        // private userWcsLine _distLine;

        [DisplayName("水平距离")]
        [DescriptionAttribute("输出属性")]
        public double LevelDist { get; set; }

        [DisplayName("垂直距离")]
        [DescriptionAttribute("输出属性")]
        public double VerticalDist { get; set; }

        [DisplayName("距离直线")]
        [DescriptionAttribute("输出属性")]
        public userWcsLine DistLine { get { return _distLine; } set { this._distLine = value; } }




        [DisplayName("点对象")]
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
                                case nameof(userWcsVector):
                                    userWcsVector wcsVector = item as userWcsVector;
                                    this._wcsPoint = new userWcsPoint(wcsVector.X, wcsVector.Y, wcsVector.Z, wcsVector.CamParams);
                                    break;
                                case nameof(userWcsCircle):
                                    userWcsCircle wcsCircle = item as userWcsCircle;
                                    this._wcsPoint = new userWcsPoint(wcsCircle.X, wcsCircle.Y, wcsCircle.Z, wcsCircle.CamParams);
                                    this._wcsPoint.Grab_x = wcsCircle.Grab_x;
                                    this._wcsPoint.Grab_y = wcsCircle.Grab_y;
                                    this._wcsPoint.Grab_theta = wcsCircle.Grab_theta;
                                    break;
                                case nameof(userWcsCircleSector):
                                    userWcsCircleSector wcsCircleSector = item as userWcsCircleSector;
                                    this._wcsPoint = new userWcsPoint(wcsCircleSector.X, wcsCircleSector.Y, wcsCircleSector.Z, wcsCircleSector.CamParams);
                                    this._wcsPoint.CamName = wcsCircleSector.CamName;
                                    this._wcsPoint.Grab_x = wcsCircleSector.Grab_x;
                                    this._wcsPoint.Grab_y = wcsCircleSector.Grab_y;
                                    break;
                                case nameof(userWcsRectangle2):
                                    userWcsRectangle2 wcsRect2 = item as userWcsRectangle2;
                                    this._wcsPoint = new userWcsPoint(wcsRect2.X, wcsRect2.Y, wcsRect2.Z, wcsRect2.CamParams);
                                    this._wcsPoint.Grab_x = wcsRect2.Grab_x;
                                    this._wcsPoint.Grab_y = wcsRect2.Grab_y;
                                    this._wcsPoint.Grab_theta = wcsRect2.Grab_theta;
                                    break;
                                case nameof(userWcsCoordSystem):
                                    userWcsCoordSystem wcsSys = item as userWcsCoordSystem;
                                    this._wcsPoint = new userWcsPoint(wcsSys.CurrentPoint.X, wcsSys.CurrentPoint.Y, wcsSys.CurrentPoint.Z, wcsSys.CurrentPoint.CamParams);
                                    this._wcsPoint.Grab_x = 0;
                                    this._wcsPoint.Grab_y = 0;
                                    this._wcsPoint.Grab_theta = 0;
                                    break;
                                case nameof(userWcsEllipse):
                                    userWcsEllipse wcsEllipse = item as userWcsEllipse;
                                    this._wcsPoint = new userWcsPoint(wcsEllipse.X, wcsEllipse.Y, wcsEllipse.Z, wcsEllipse.CamParams);
                                    this._wcsPoint.CamName = wcsEllipse.CamName;
                                    this._wcsPoint.Grab_x = wcsEllipse.Grab_x;
                                    this._wcsPoint.Grab_y = wcsEllipse.Grab_y;
                                    break;
                                case nameof(userWcsEllipseSector):
                                    userWcsEllipseSector wcsEllipseSector = item as userWcsEllipseSector;
                                    this._wcsPoint = new userWcsPoint(wcsEllipseSector.X, wcsEllipseSector.Y, wcsEllipseSector.Z, wcsEllipseSector.CamParams);
                                    this._wcsPoint.CamName = wcsEllipseSector.CamName;
                                    this._wcsPoint.Grab_x = wcsEllipseSector.Grab_x;
                                    this._wcsPoint.Grab_y = wcsEllipseSector.Grab_y;
                                    break;
                                default:
                                    this._wcsPoint = new userWcsPoint();
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

        [DisplayName("坐标系")]
        [DescriptionAttribute("输入属性2")]
        public userWcsCoordSystem WcsCoordSystem
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
                                case nameof(userWcsCoordSystem):
                                    this._coordSystem = item as userWcsCoordSystem;
                                    break;
                            }
                        }
                    }
                }
                else
                    this._coordSystem = new userWcsCoordSystem();
                return this._coordSystem;
            }
            set
            {
                this._coordSystem = value;
            }
        }



        public PointToCoordSysDist2D()
        {

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
                this.Result.Succss = HalconLibrary.PointToCoordSysDist2D(this.WcsPoint, this.WcsCoordSystem, out this._levelDist, out this._verticalDist,out this._distLine, out this._distLine1, out this._distLine2);
                this.CreateResultInfo(3);
                stopwatch.Stop();
                /////////////////////
                //this.InitBindingTable();
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[0].SetValue(this.name, "水平距离", this._levelDist);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[1].SetValue(this.name, "垂直距离", this._verticalDist);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[2].SetValue(this.name, "Time(ms)", stopwatch.ElapsedMilliseconds);
                OnExcuteCompleted(this._distLine1.CamName, this._distLine1?.ViewWindow, this.name, this._distLine1);
                OnExcuteCompleted(this._distLine1.CamName, this._distLine1?.ViewWindow, this.name + 2, this._distLine2);
            }
            catch (Exception ex)
            {
                LoggerHelper.Error(this.name + "->执行报错", ex);
                return this.Result;
            }
            if (this.Result.Succss)
                LoggerHelper.Info(this.name + "->执行成功:");
            else
                LoggerHelper.Error(this.name + "->执行失败:");
            // 更改UI字体　
            UpdataNodeElementStyle(param, this.Result.Succss);
            return this.Result;
        }
        public object GetPropertyValues(string propertyName)
        {
            switch (propertyName)
            {
                default:
                case "名称":
                case nameof(Name):
                    return this.name;
                case nameof(VerticalDist):
                    return this.VerticalDist; //
                case nameof(LevelDist):
                    return this.LevelDist; //
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
                if (this.DistLine != null)
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

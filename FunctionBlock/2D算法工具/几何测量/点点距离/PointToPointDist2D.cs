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
    [DefaultProperty(nameof(MaxDist))]
    public class PointToPointDist2D : BaseFunction, IFunction, INotifyPropertyChanged
    {
        private double _MaxDist;
        private double _LevelDist;
        private double _VerticalDist;
        private userWcsLine _DistLine;
        private userWcsPoint _WcsPoint1;
        private userWcsPoint _WcsPoint2;

        [DisplayName("最大距离")]
        [DescriptionAttribute("输出属性")]
        public double MaxDist { get; set; }

        [DisplayName("水平距离")]
        [DescriptionAttribute("输出属性")]
        public double LevelDist { get; set; }

        [DisplayName("垂直距离")]
        [DescriptionAttribute("输出属性")]
        public double VerticalDist { get; set; }

        [DisplayName("距离直线")]
        [DescriptionAttribute("输出属性")]
        public userWcsLine DistLine { get; set; }

        [DisplayName("CamDist_X")]
        [DescriptionAttribute("输出属性")]
        public double CamDist_X { get; set; }
        [DisplayName("CamDist_Y")]
        [DescriptionAttribute("输出属性")]
        public double CamDist_Y { get; set; }



        [DisplayName("点对象1")]
        [DescriptionAttribute("输入属性1")]
        public userWcsPoint WcsPoint1
        {
            get
            {
                if (this.RefSource1.Count > 0)
                {
                    object [] oo = this.GetPropertyValue(this.RefSource1);
                    if(oo != null)
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
                                    break;
                                case nameof(userWcsCircle):
                                    userWcsCircle wcsCircle = item as userWcsCircle;
                                    this._WcsPoint1 = new userWcsPoint(wcsCircle.X, wcsCircle.Y, wcsCircle.Z, wcsCircle.CamParams);
                                    this._WcsPoint1.Grab_x = wcsCircle.Grab_x;
                                    this._WcsPoint1.Grab_y = wcsCircle.Grab_y;
                                    this._WcsPoint1.Grab_theta = wcsCircle.Grab_theta;
                                    break;
                                case nameof(userWcsCircleSector):
                                    userWcsCircleSector wcsCircleSector = item as userWcsCircleSector;
                                    this._WcsPoint1 = new userWcsPoint(wcsCircleSector.X, wcsCircleSector.Y, wcsCircleSector.Z, wcsCircleSector.CamParams);
                                    this._WcsPoint1.CamName = wcsCircleSector.CamName;
                                    this._WcsPoint1.Grab_x = wcsCircleSector.Grab_x;
                                    this._WcsPoint1.Grab_y = wcsCircleSector.Grab_y;
                                    break;
                                case nameof(userWcsRectangle2):
                                    userWcsRectangle2 wcsRect2 = item as userWcsRectangle2;
                                    this._WcsPoint1 = new userWcsPoint(wcsRect2.X, wcsRect2.Y, wcsRect2.Z, wcsRect2.CamParams);
                                    this._WcsPoint1.Grab_x = wcsRect2.Grab_x;
                                    this._WcsPoint1.Grab_y = wcsRect2.Grab_y;
                                    this._WcsPoint1.Grab_theta = wcsRect2.Grab_theta;
                                    break;
                                case nameof(userWcsCoordSystem):
                                    userWcsCoordSystem wcsSys = item as userWcsCoordSystem;
                                    this._WcsPoint1 = new userWcsPoint(wcsSys.CurrentPoint.X, wcsSys.CurrentPoint.Y, wcsSys.CurrentPoint.Z, wcsSys.CurrentPoint.CamParams);
                                    this._WcsPoint1.Grab_x = 0;
                                    this._WcsPoint1.Grab_y = 0;
                                    this._WcsPoint1.Grab_theta = 0;
                                    break;
                                case nameof(userWcsEllipse):
                                    userWcsEllipse wcsEllipse = item as userWcsEllipse;
                                    this._WcsPoint1 = new userWcsPoint(wcsEllipse.X, wcsEllipse.Y, wcsEllipse.Z, wcsEllipse.CamParams);
                                    this._WcsPoint1.CamName = wcsEllipse.CamName;
                                    this._WcsPoint1.Grab_x = wcsEllipse.Grab_x;
                                    this._WcsPoint1.Grab_y = wcsEllipse.Grab_y;
                                    break;
                                case nameof(userWcsEllipseSector):
                                    userWcsEllipseSector wcsEllipseSector = item as userWcsEllipseSector;
                                    this._WcsPoint1 = new userWcsPoint(wcsEllipseSector.X, wcsEllipseSector.Y, wcsEllipseSector.Z, wcsEllipseSector.CamParams);
                                    this._WcsPoint1.CamName = wcsEllipseSector.CamName;
                                    this._WcsPoint1.Grab_x = wcsEllipseSector.Grab_x;
                                    this._WcsPoint1.Grab_y = wcsEllipseSector.Grab_y;
                                    break;
                                default:
                                    this._WcsPoint1 = new userWcsPoint();
                                    break;
                            }
                        }
                    }
                    else
                        this._WcsPoint1 = new userWcsPoint();
                }
                else
                    this._WcsPoint1 = new userWcsPoint();
                return this._WcsPoint1;
            }
            set
            {
                this._WcsPoint1 = value;
            }
        }

        [DisplayName("点对象2")]
        [DescriptionAttribute("输入属性2")]
        public userWcsPoint WcsPoint2
        {
            get
            {
                if (this.RefSource2.Count > 0)
                {
                    object [] oo = this.GetPropertyValue(this.RefSource2);
                    if (oo != null)
                    {
                        foreach (var item in oo)
                        {
                            switch (item.GetType().Name)
                            {
                                case nameof(userWcsPoint):
                                    this._WcsPoint2 = item as userWcsPoint;
                                    break;
                                case nameof(userWcsVector):
                                    userWcsVector wcsVector = item as userWcsVector;
                                    this._WcsPoint2 = new userWcsPoint(wcsVector.X, wcsVector.Y, wcsVector.Z, wcsVector.CamParams);
                                    break;
                                case nameof(userWcsCircle):
                                    userWcsCircle wcsCircle = item as userWcsCircle;
                                    this._WcsPoint2 = new userWcsPoint(wcsCircle.X, wcsCircle.Y, wcsCircle.Z, wcsCircle.CamParams);
                                    this._WcsPoint2.Grab_x = wcsCircle.Grab_x;
                                    this._WcsPoint2.Grab_y = wcsCircle.Grab_y;
                                    this._WcsPoint2.Grab_theta = wcsCircle.Grab_theta;
                                    break;
                                case nameof(userWcsCircleSector):
                                    userWcsCircleSector wcsCircleSector = item as userWcsCircleSector;
                                    this._WcsPoint2 = new userWcsPoint(wcsCircleSector.X, wcsCircleSector.Y, wcsCircleSector.Z, wcsCircleSector.CamParams);
                                    this._WcsPoint2.CamName = wcsCircleSector.CamName;
                                    this._WcsPoint2.Grab_x = wcsCircleSector.Grab_x;
                                    this._WcsPoint2.Grab_y = wcsCircleSector.Grab_y;
                                    break;
                                case nameof(userWcsRectangle2):
                                    userWcsRectangle2 wcsRect2 = item as userWcsRectangle2;
                                    this._WcsPoint2 = new userWcsPoint(wcsRect2.X, wcsRect2.Y, wcsRect2.Z, wcsRect2.CamParams);
                                    this._WcsPoint2.Grab_x = wcsRect2.Grab_x;
                                    this._WcsPoint2.Grab_y = wcsRect2.Grab_y;
                                    this._WcsPoint2.Grab_theta = wcsRect2.Grab_theta;
                                    break;
                                case nameof(userWcsCoordSystem):
                                    userWcsCoordSystem wcsSys = item as userWcsCoordSystem;
                                    this._WcsPoint2 = new userWcsPoint(wcsSys.CurrentPoint.X, wcsSys.CurrentPoint.Y, wcsSys.CurrentPoint.Z, wcsSys.CurrentPoint.CamParams);
                                    this._WcsPoint2.Grab_x = 0;
                                    this._WcsPoint2.Grab_y = 0;
                                    this._WcsPoint2.Grab_theta = 0;
                                    break;

                                case nameof(userWcsEllipse):
                                    userWcsEllipse wcsEllipse = item as userWcsEllipse;
                                    this._WcsPoint2 = new userWcsPoint(wcsEllipse.X, wcsEllipse.Y, wcsEllipse.Z, wcsEllipse.CamParams);
                                    this._WcsPoint2.CamName = wcsEllipse.CamName;
                                    this._WcsPoint2.Grab_x = wcsEllipse.Grab_x;
                                    this._WcsPoint2.Grab_y = wcsEllipse.Grab_y;
                                    break;
                                case nameof(userWcsEllipseSector):
                                    userWcsEllipseSector wcsEllipseSector = item as userWcsEllipseSector;
                                    this._WcsPoint2 = new userWcsPoint(wcsEllipseSector.X, wcsEllipseSector.Y, wcsEllipseSector.Z, wcsEllipseSector.CamParams);
                                    this._WcsPoint2.CamName = wcsEllipseSector.CamName;
                                    this._WcsPoint2.Grab_x = wcsEllipseSector.Grab_x;
                                    this._WcsPoint2.Grab_y = wcsEllipseSector.Grab_y;
                                    break;
                                default:
                                    this._WcsPoint2 = new userWcsPoint();
                                    break;
                            }
                        }
                    }
                    else
                        this._WcsPoint2 = new userWcsPoint();
                }
                else
                    this._WcsPoint2 = new userWcsPoint();
                return this._WcsPoint2;
            }
            set
            {
                this._WcsPoint2 = value;
            }
        }

        private void InitBindingTable()
        {
            if (this.ResultInfo == null)
            {
                this.ResultInfo = new BindingList<MeasureResultInfo>();
                ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name, "MaxDist", 0));
                ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name, "LevelDist", 0));
                ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name, "VerticalDist", 0));
                ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name, "CamDist_X", 0));
                ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name, "CamDist_Y", 0));
                ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name, "TIme(ms)", 0));
            }
        }

        public PointToPointDist2D()
        {
            InitBindingTable();
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
                this.Result.Succss = HalconLibrary.CalculatePointToPointDist(this.WcsPoint1, this.WcsPoint2, out this._MaxDist, out this._LevelDist, out this._VerticalDist, out this._DistLine);
                this.CamDist_X = (this.WcsPoint1.X - this.WcsPoint1.Grab_x) - (this.WcsPoint2.X - this.WcsPoint2.Grab_x);
                this.CamDist_Y = (this.WcsPoint1.Y - this.WcsPoint1.Grab_y) - (this.WcsPoint2.Y - this.WcsPoint2.Grab_y);
                stopwatch.Stop();
                /////////////////////
                //this.InitBindingTable();
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[0].SetValue(this.name, "最大距离", this._MaxDist);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[1].SetValue(this.name, "水平距离", this._LevelDist);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[2].SetValue(this.name, "垂直距离", this._VerticalDist);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[3].SetValue(this.name, "CamDist_X", this.CamDist_X);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[4].SetValue(this.name, "CamDist_Y", this.CamDist_Y);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[5].SetValue(this.name, "TIme(ms)", stopwatch.ElapsedMilliseconds);
                //OnExcuteCompleted(this.name, this.DistLine);
                OnExcuteCompleted(this._DistLine.CamName, this._DistLine?.ViewWindow, this.name + index, this._DistLine);
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
                case nameof(MaxDist):
                    return this.MaxDist; //
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

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MotionControlCard;
using System.Windows.Forms;
using System.Threading;
using HalconDotNet;
using Common;
using System.IO;
using Sensor;
using Command;
using AlgorithmsLibrary;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;

namespace FunctionBlock
{
    [Serializable]
    [DefaultProperty(nameof(HomMat2D))]
    /// <summary>
    /// 激光取点采集的数据类
    /// </summary>
    public class MapCalib : BaseFunction, IFunction
    {
        private UserHomMat2D _homMat2D;
        public userWcsPoint[] _sourcePoint;
        public userWcsPoint[] _targetPoint;

        [DisplayName("输入点1")]
        [DescriptionAttribute("输入属性1")]
        public userWcsPoint[] SourcePoint
        {
            get
            {
                if (this.RefSource1.Count > 0)
                {
                    object[] oo = this.GetPropertyValue(this.RefSource1);
                    List<userWcsPoint> list = new List<userWcsPoint>();
                    foreach (var item in oo)
                    {
                        switch (item.GetType().Name)
                        {
                            case nameof(userWcsPoint):
                                list.Add(item as userWcsPoint);
                                break;
                            case nameof(userWcsCircle):
                                userWcsCircle wcsCircle = item as userWcsCircle;
                                list.Add(new userWcsPoint(wcsCircle.X, wcsCircle.Y, wcsCircle.Z, wcsCircle.Grab_x, wcsCircle.Grab_y, wcsCircle.CamParams));
                                break;
                            case nameof(userWcsCircleSector):
                                userWcsCircleSector wcsCircleSector = item as userWcsCircleSector;
                                list.Add(new userWcsPoint(wcsCircleSector.X, wcsCircleSector.Y, wcsCircleSector.Z, wcsCircleSector.Grab_x, wcsCircleSector.Grab_y, wcsCircleSector.CamParams));
                                break;
                            case nameof(userWcsRectangle2):
                                userWcsRectangle2 wcsRect2 = item as userWcsRectangle2;
                                list.Add(new userWcsPoint(wcsRect2.X, wcsRect2.Y, wcsRect2.Z, wcsRect2.Grab_x, wcsRect2.Grab_y, wcsRect2.CamParams));
                                break;
                            case nameof(userWcsCoordSystem):
                                userWcsCoordSystem wcsCoord = item as userWcsCoordSystem;
                                list.Add(new userWcsPoint(wcsCoord.CurrentPoint.X, wcsCoord.CurrentPoint.Y, wcsCoord.CurrentPoint.Z, wcsCoord.CurrentPoint.CamParams));
                                break;
                            case nameof(userWcsVector):
                                userWcsVector wcsVector = item as userWcsVector;
                                list.Add(new userWcsPoint(wcsVector.X, wcsVector.Y, wcsVector.Z, wcsVector.CamParams));
                                break;
                            default:
                                throw new ArgumentException("参数类型错误");
                        }
                    }
                    this._sourcePoint = list.ToArray();
                    list.Clear();
                }
                else
                    this._sourcePoint = new userWcsPoint[0];
                return this._sourcePoint;
            }
            set
            {
                this._sourcePoint = value;
            }
        }

        [DisplayName("输入点2")]
        [DescriptionAttribute("输入属性2")]
        public userWcsPoint[] TargetPoint
        {
            get
            {
                if (this.RefSource2.Count > 0)
                {
                    object[] oo = this.GetPropertyValue(this.RefSource2);
                    List<userWcsPoint> list = new List<userWcsPoint>();
                    foreach (var item in oo)
                    {
                        switch (item.GetType().Name)
                        {
                            case nameof(userWcsPoint):
                                list.Add(item as userWcsPoint);
                                break;
                            case nameof(userWcsCircle):
                                userWcsCircle wcsCircle = item as userWcsCircle;
                                list.Add(new userWcsPoint(wcsCircle.X, wcsCircle.Y, wcsCircle.Z, wcsCircle.Grab_x, wcsCircle.Grab_y, wcsCircle.CamParams));
                                break;
                            case nameof(userWcsCircleSector):
                                userWcsCircleSector wcsCircleSector = item as userWcsCircleSector;
                                list.Add(new userWcsPoint(wcsCircleSector.X, wcsCircleSector.Y, wcsCircleSector.Z, wcsCircleSector.Grab_x, wcsCircleSector.Grab_y, wcsCircleSector.CamParams));
                                break;
                            case nameof(userWcsRectangle2):
                                userWcsRectangle2 wcsRect2 = item as userWcsRectangle2;
                                list.Add(new userWcsPoint(wcsRect2.X, wcsRect2.Y, wcsRect2.Z, wcsRect2.Grab_x, wcsRect2.Grab_y, wcsRect2.CamParams));
                                break;
                            case nameof(userWcsCoordSystem):
                                userWcsCoordSystem wcsCoord = item as userWcsCoordSystem;
                                list.Add(new userWcsPoint(wcsCoord.CurrentPoint.X, wcsCoord.CurrentPoint.Y, wcsCoord.CurrentPoint.Z, wcsCoord.CurrentPoint.CamParams));
                                break;
                            case nameof(userWcsVector):
                                userWcsVector wcsVector = item as userWcsVector;
                                list.Add(new userWcsPoint(wcsVector.X, wcsVector.Y, wcsVector.Z, wcsVector.CamParams));
                                break;
                            default:
                                throw new ArgumentException("参数类型错误");
                        }
                    }
                    this._targetPoint = list.ToArray();
                    list.Clear();
                }
                else
                    this._targetPoint = new userWcsPoint[0];
                return this._targetPoint;
            }
            set
            {
                this._targetPoint = value;
            }
        }

        [DisplayName("映射矩阵")]
        [DescriptionAttribute("输出属性")]
        public UserHomMat2D HomMat2D { get => _homMat2D; set => _homMat2D = value; }

        public MapCalibParam Param { get; set; }


        public MapCalib()
        {
            this.Param = new MapCalibParam();
            this.ResultInfo = new BindingList<MeasureResultInfo>();
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name, "C00", 0));
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name, "C01", 0));
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name, "C02", 0));
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name, "C10", 0));
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name, "C11", 0));
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name, "C12", 0));
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name, "Time(ms)", 0));
        }



        #region 实现接口,各自实现自己的逻辑，就可实现通用的逻辑
        public OperateResult Execute(params object[] param)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            this.Result.Succss = false;
            try
            {
                this.Result.Succss = CalculateMethod.MapCalib(this.SourcePoint, this.TargetPoint, this.Param ,out this._homMat2D);
                stopwatch.Stop();
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[0].SetValue(this.name, "C00", this._homMat2D.c00);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[1].SetValue(this.name, "C01", this._homMat2D.c01);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[2].SetValue(this.name, "C02", this._homMat2D.c02);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[3].SetValue(this.name, "C10", this._homMat2D.c10);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[4].SetValue(this.name, "C11", this._homMat2D.c11);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[5].SetValue(this.name, "C12", this._homMat2D.c11);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[6].SetValue(this.name, "Time(ms)", stopwatch.ElapsedMilliseconds);
                // 使用发命令的方式来更新视图  
                if (this.Result.Succss)
                    LoggerHelper.Info(this.name + "N点标定成功->" + this._homMat2D.ToString());
                else
                    LoggerHelper.Error(this.name + "N点标定成功失败");
            }
            catch (Exception ex)
            {
                LoggerHelper.Error(this.name + "N点标定" + "报错" + ex);
                this.Result.Succss = false;
            }
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
                default:
                    return this.name;
            }
        }

        public bool SetPropertyValues(string propertyName, params object[] value)
        {
            string name = "";
            if (value != null)
                name = value[0].ToString();
            switch (propertyName)
            {
                default:
                case "名称":
                case nameof(this.Name):
                    this.name = value[0].ToString();
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

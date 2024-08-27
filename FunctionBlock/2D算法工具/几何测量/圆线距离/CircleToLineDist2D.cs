﻿using System;
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
using System.Drawing;
using System.Diagnostics;

namespace FunctionBlock
{
    [Serializable]
    [DefaultProperty("CircleDist")]
    public class CircleToLineDist2D : BaseFunction, IFunction
    {
        private double _MaxDist;
        private double _MinDist;
        private double _CircleDist;
        private userWcsLine _DistLine;

        private userWcsCircle _WcsCircle;
        private userWcsLine _WcsLine;

        [DisplayName("最大距离")]
        [DescriptionAttribute("输出属性")]
        public double MaxDist { get => _MaxDist; set => _MaxDist = value; }

        [DisplayName("最小距离")]
        [DescriptionAttribute("输出属性")]
        public double MinDist { get => _MinDist; set => _MinDist = value; }

        [DisplayName("圆心距离")]
        [DescriptionAttribute("输出属性")]
        public double CircleDist { get => _CircleDist; set => _CircleDist = value; }


        [DisplayName("输入圆")]
        [DescriptionAttribute("输入属性1")]
        public userWcsCircle WcsCircle
        {
            get
            {
                if (this.RefSource1.Count > 0)
                {
                    object oo = this.GetPropertyValue(this.RefSource1)[0];
                    if (oo != null)
                    {
                        switch (oo.GetType().Name)
                        {
                            case nameof(userWcsCircle):
                                this._WcsCircle = oo as userWcsCircle;
                                break;
                            case nameof(userWcsCircleSector):
                                this._WcsCircle = (oo as userWcsCircleSector).GetWcsCircle();
                                break;
                            default:
                                this._WcsCircle = new userWcsCircle();
                                break;
                        }
                    }
                    else
                        this._WcsCircle = new userWcsCircle();
                }
                else
                    this._WcsCircle = new userWcsCircle();
                return this._WcsCircle;
            }
            set
            {
                this._WcsCircle = value;
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
                    this._WcsLine = this.GetPropertyValue(this.RefSource2)[0] as userWcsLine;
                    if (this._WcsLine == null)
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


        public CircleToLineDist2D()
        {
            this.ResultInfo = new BindingList<MeasureResultInfo>();
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo());
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo());
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo());
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo());
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
                this.Result.Succss = HalconLibrary.CalculateCircleToLineDist2D(this.WcsCircle, this.WcsLine, out this._CircleDist, out this._MaxDist, out this._MinDist, out this._DistLine);
                stopwatch.Stop();
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[0].SetValue(this.name, "圆心距", this._CircleDist);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[1].SetValue(this.name, "最大距离", this._MaxDist);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[2].SetValue(this.name, "最小距离", this._MinDist);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[3].SetValue(this.name, "Time(ms)", this._MinDist);
                //OnExcuteCompleted(this.name, this._DistLine);
                OnExcuteCompleted(this._DistLine.CamName, this._DistLine.ViewWindow, this.name + index, this._DistLine);
            }
            catch (Exception ex)
            {
                LoggerHelper.Error(this.name + "->执行报错", ex);
                this.Result.Succss = false;
            }
            if (this.Result.Succss)
                LoggerHelper.Info(this.name + "执行成功:" + string.Format("最大值：{0},最小值：{1},圆心距：{2}", this._CircleDist, this._MaxDist, this._MinDist));
            else
                LoggerHelper.Error(this.name + "执行失败:" + string.Format("最大值：{0},最小值：{1},圆心距：{2}", this._CircleDist, this._MaxDist, this._MinDist));
            // 更改UI字体　
            UpdataNodeElementStyle(param, this.Result.Succss);
            return this.Result;
        }

        public object GetPropertyValues(string propertyName)
        {
            switch (propertyName)
            {
                default:
                case nameof(Name):
                    return this.name;
                case nameof(CircleDist):
                    return this.CircleDist; // 默认返回值为绘制的距离对象
                case nameof(MaxDist):
                    return this.MaxDist;
                case nameof(MinDist):
                    return this.MinDist;
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

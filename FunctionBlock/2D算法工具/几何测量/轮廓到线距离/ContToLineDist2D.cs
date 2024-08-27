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
using System.Drawing;
using System.Diagnostics;

namespace FunctionBlock
{
    [Serializable]
    [DefaultProperty(nameof(MinWcsPoint))]
    public class ContToLineDist2D : BaseFunction, IFunction
    {
        private double _MaxDist;
        private double _MinDist;
        private double _meanDist;
        private userWcsPoint _minWcsPoint;
        private userWcsPoint _maxWcsPoint;

        private userWcsPoint[] _wcsPoint;
        private userWcsLine _WcsLine;

        [DisplayName("最大距离")]
        [DescriptionAttribute("输出属性")]
        public double MaxDist { get => _MaxDist; set => _MaxDist = value; }

        [DisplayName("最小距离")]
        [DescriptionAttribute("输出属性")]
        public double MinDist { get => _MinDist; set => _MinDist = value; }

        [DisplayName("平均距离")]
        [DescriptionAttribute("输出属性")]
        public double MeanDist { get => _meanDist; set => _meanDist = value; }

        [DisplayName("最小值点")]
        [DescriptionAttribute("输出属性")]
        public userWcsPoint MinWcsPoint { get => _minWcsPoint; set => _minWcsPoint = value; }

        [DisplayName("最大值点")]
        [DescriptionAttribute("输出属性")]
        public userWcsPoint MaxWcsPoint { get => _maxWcsPoint; set => _maxWcsPoint = value; }



        [DisplayName("输入点")]
        [DescriptionAttribute("输入属性1")]
        public userWcsPoint[] WcsPoint
        {
            get
            {
                if (this.RefSource1.Count > 0)
                {
                    List<userWcsPoint> list = new List<userWcsPoint>();
                    object oo = this.GetPropertyValue(this.RefSource1)[0];
                    userWcsPoint[] fitWcsPoint = null;
                    if (oo != null)
                    {
                        switch (oo.GetType().Name)
                        {
                            case nameof(userWcsCircle):
                                userWcsCircle wcsCircle = oo as userWcsCircle;
                                if (wcsCircle != null && wcsCircle.EdgesPoint_xyz != null)
                                {
                                    switch(this.Param.DataPointSource)
                                    {
                                        default:
                                        case enCalculateDataPoint.拟合点:
                                            fitWcsPoint = wcsCircle.GetFitWcsPoint();
                                            foreach (var item in fitWcsPoint)
                                                list.Add(item);
                                            break;
                                        case enCalculateDataPoint.原始点:
                                            foreach (var item in wcsCircle.EdgesPoint_xyz)
                                                list.Add(item);
                                            break;
                                    }
                                }
                                break;
                            case nameof(userWcsCircleSector):
                                userWcsCircleSector wcsCircleSector = oo as userWcsCircleSector;
                                if (wcsCircleSector != null && wcsCircleSector.EdgesPoint_xyz != null)
                                {
                                    switch (this.Param.DataPointSource)
                                    {
                                        default:
                                        case enCalculateDataPoint.拟合点:
                                            fitWcsPoint = wcsCircleSector.GetFitWcsPoint();
                                            foreach (var item in fitWcsPoint)
                                                list.Add(item);
                                            break;
                                        case enCalculateDataPoint.原始点:
                                            foreach (var item in wcsCircleSector.EdgesPoint_xyz)
                                                list.Add(item);
                                            break;
                                    }
                                }
                                break;
                            case nameof(userWcsEllipseSector):
                                userWcsEllipseSector wcsEllipseSector = oo as userWcsEllipseSector;
                                if (wcsEllipseSector != null && wcsEllipseSector.EdgesPoint_xyz != null)
                                {
                                    switch (this.Param.DataPointSource)
                                    {
                                        default:
                                        case enCalculateDataPoint.拟合点:
                                            fitWcsPoint = wcsEllipseSector.GetFitWcsPoint();
                                            foreach (var item in fitWcsPoint)
                                                list.Add(item);
                                            break;
                                        case enCalculateDataPoint.原始点:
                                            foreach (var item in wcsEllipseSector.EdgesPoint_xyz)
                                                list.Add(item);
                                            break;
                                    }
                                }
                                break;
                            case nameof(userWcsEllipse):
                                userWcsEllipse wcsEllipse = oo as userWcsEllipse;
                                if (wcsEllipse != null && wcsEllipse.EdgesPoint_xyz != null)
                                {
                                    switch (this.Param.DataPointSource)
                                    {
                                        default:
                                        case enCalculateDataPoint.拟合点:
                                            fitWcsPoint = wcsEllipse.GetFitWcsPoint();
                                            foreach (var item in fitWcsPoint)
                                                list.Add(item);
                                            break;
                                        case enCalculateDataPoint.原始点:
                                            foreach (var item in wcsEllipse.EdgesPoint_xyz)
                                                list.Add(item);
                                            break;
                                    }
                                }
                                break;
                        }
                        /////////////////////
                        this._wcsPoint = list.ToArray();
                        list.Clear();
                    }
                    else
                        this._wcsPoint = new userWcsPoint[0];
                }
                else
                    this._wcsPoint = new userWcsPoint[0];
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


        public NPointDistParam Param { get; set; }

        public ContToLineDist2D()
        {
            this.Param = new NPointDistParam();
            this.ResultInfo = new BindingList<MeasureResultInfo>();
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo());
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo());
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo());
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo());
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
                this.Result.Succss = HalconLibrary.CalculateNPointToLineDist2D(this.WcsPoint, this.WcsLine, out this._meanDist, out this._MaxDist, out this._MinDist, out this._minWcsPoint, out this._maxWcsPoint);
                stopwatch.Stop();
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[0].SetValue(this.name, "最大距离", this._meanDist);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[1].SetValue(this.name, "最小距离", this._MaxDist);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[2].SetValue(this.name, "平均距离", this._MinDist);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[3].SetValue(this.name, "最小点_X", this._minWcsPoint.X);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[4].SetValue(this.name, "最小点_Y", this._minWcsPoint.Y);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[5].SetValue(this.name, "最大点_X", this._maxWcsPoint.X);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[6].SetValue(this.name, "最大点_Y", this._maxWcsPoint.Y);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[7].SetValue(this.name, "Time(ms)", stopwatch.ElapsedMilliseconds);
                ////////////////////////////////
                switch (this.Param.OutPointMode)
                {
                    case enOutPointMode.输出最小点:
                        OnExcuteCompleted(this._minWcsPoint.CamName, this._minWcsPoint.ViewWindow, this.name + index, this._minWcsPoint);
                        break;
                    case enOutPointMode.输出最大点:
                        OnExcuteCompleted(this._minWcsPoint.CamName, this._minWcsPoint.ViewWindow, this.name + index, this._maxWcsPoint);
                        break;
                    case enOutPointMode.输出全部:
                        OnExcuteCompleted(this._minWcsPoint.CamName, this._minWcsPoint.ViewWindow, this.name + "min" + index, this._minWcsPoint);
                        OnExcuteCompleted(this._minWcsPoint.CamName, this._minWcsPoint.ViewWindow, this.name + "max" + index, this._maxWcsPoint);
                        break;
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.Error(this.name + "->执行报错", ex);
                this.Result.Succss = false;
            }
            if (this.Result.Succss)
                LoggerHelper.Info(this.name + "执行成功:" + string.Format("最大值：{0},最小值：{1},圆心距：{2}", this._meanDist, this._MaxDist, this._MinDist));
            else
                LoggerHelper.Error(this.name + "执行失败:" + string.Format("最大值：{0},最小值：{1},圆心距：{2}", this._meanDist, this._MaxDist, this._MinDist));
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
                case nameof(MeanDist):
                    return this.MeanDist; // 默认返回值为绘制的距离对象
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

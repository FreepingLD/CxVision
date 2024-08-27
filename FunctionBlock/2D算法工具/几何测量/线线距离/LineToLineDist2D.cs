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
    [DefaultProperty(nameof(MeanDist))]
    public class LineToLineDist2D : BaseFunction, IFunction, INotifyPropertyChanged
    {
        private double _MaxDist;
        private double _MinDist;
        private double _MeanDist;
        private userWcsLine _DistLine;
        private userWcsLine _WcsLine1;
        private userWcsLine _WcsLine2;

        [DisplayName("最大距离")]
        [DescriptionAttribute("输出属性")]
        public double MaxDist { get => _MaxDist; set => _MaxDist = value; }

        [DisplayName("最小距离")]
        [DescriptionAttribute("输出属性")]
        public double MinDist { get => _MinDist; set => _MinDist = value; }

        [DisplayName("平均距离")]
        [DescriptionAttribute("输出属性")]
        public double MeanDist { get => _MeanDist; set => _MeanDist = value; }

        [DisplayName("输入直线1")]
        [DescriptionAttribute("输入属性1")]
        public userWcsLine WcsLine1
        {
            get
            {
                if (this.RefSource1.Count > 0)
                {
                    this._WcsLine1 = this.GetPropertyValue(this.RefSource1)[0] as userWcsLine;
                    if (this._WcsLine1 == null)
                        this._WcsLine1 = new userWcsLine();
                }
                else
                    this._WcsLine1 = new userWcsLine();
                return this._WcsLine1;
            }
            set
            {
                this._WcsLine1 = value;
            }
        }

        [DisplayName("输入直线2")]
        [DescriptionAttribute("输入属性2")]
        public userWcsLine WcsLine2
        {
            get
            {
                if (this.RefSource2.Count > 0)
                {
                    this._WcsLine2 = this.GetPropertyValue(this.RefSource2)[0] as userWcsLine;
                    if (this._WcsLine2 == null)
                        this._WcsLine2 = new userWcsLine();
                }
                else
                    this._WcsLine2 = new userWcsLine();
                return this._WcsLine2;
            }
            set
            {
                this._WcsLine2 = value;
            }
        }

        [DisplayName("距离直线")]
        [DescriptionAttribute("输出属性")]
        public userWcsLine DistLine { get => _DistLine; set => _DistLine = value; }

        public LineToLineDist2D()
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
                this.Result.Succss = HalconLibrary.CalculateLineToLineDist2D(this.WcsLine1, this.WcsLine2, out this._MaxDist, out this._MinDist, out this._MeanDist, out this._DistLine);
                stopwatch.Stop();
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[0].SetValue(this.name + index, "平均距离", this._MeanDist);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[1].SetValue(this.name + index, "最大距离", this._MaxDist);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[2].SetValue(this.name + index, "最小距离", this._MinDist);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[3].SetValue(this.name + index, "Time(ms)", stopwatch.ElapsedMilliseconds);
                //((BindingList<MeasureResultInfo>)this.ResultInfo)[3].SetValue(this.name + index, "数据标签", 0);
                //if (((BindingList<MeasureResultInfo>)this.ResultInfo)[3].IsOutput)
                //{
                //    userPixLine pixLine = this._DistLine.GetPixLine();
                //    string content = this._MeanDist.ToString("f3"); // "距离=" + 
                //    double size = ((BindingList<MeasureResultInfo>)this.ResultInfo)[3].Std_Value;
                //    double offsetCol = ((BindingList<MeasureResultInfo>)this.ResultInfo)[3].LimitUp;
                //    double offsetRow = ((BindingList<MeasureResultInfo>)this.ResultInfo)[3].LimitDown;
                //    if (size == 0) size = 15;
                //    userTextLable _TextLable;
                //    if (((BindingList<MeasureResultInfo>)this.ResultInfo)[0].State == "OK")
                //        _TextLable = new userTextLable(content, (pixLine.Col1 + pixLine.Col2) * 0.5 + offsetCol, (pixLine.Row1 + pixLine.Row2) * 0.5 + offsetRow, (int)size, "green");
                //    else
                //        _TextLable = new userTextLable(content, (pixLine.Col1 + pixLine.Col2) * 0.5 + offsetCol, (pixLine.Row1 + pixLine.Row2) * 0.5 + offsetRow, (int)size, "red");
                //    OnExcuteCompleted(this._DistLine.CamName, this._DistLine.ViewWindow, this.name + ".数据标签" + index, _TextLable);
                //}
                OnExcuteCompleted(this._DistLine.CamName, this._DistLine.ViewWindow, this.name + index, this._DistLine);
            }
            catch (Exception ex)
            {
                LoggerHelper.Error(this.name + "->执行报错", ex);
                this.Result.Succss = false;
            }
            if (this.Result.Succss)
                LoggerHelper.Info(this.name + "->执行成功:" + string.Format("Dist={0}", this.MeanDist));
            else
                LoggerHelper.Error(this.name + "->执行失败:" + string.Format("Dist={0}", this.MeanDist));
            // 更改UI字体　
            UpdataNodeElementStyle(param, this.Result.Succss);
            return this.Result;
        }
        public object GetPropertyValues(string propertyName)
        {
            switch (propertyName)
            {
                case "名称":
                case nameof(Name):
                    return this.name;
                case nameof(MeanDist):
                default:
                    return this.MeanDist; // 默认返回值为绘制的距离对象，用于点击操作
                case nameof(MaxDist):
                    return this.MaxDist;
                case nameof(MinDist):
                    return this.MinDist;
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

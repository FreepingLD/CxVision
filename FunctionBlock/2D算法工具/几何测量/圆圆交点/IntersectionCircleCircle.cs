using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlgorithmsLibrary;
using HalconDotNet;
using System.Data;
using System.ComponentModel;
using Common;
using System.Diagnostics;

namespace FunctionBlock
{
    [Serializable]
    [DefaultProperty("CircleDist")]
    public class IntersectionCircleCircle : BaseFunction, IFunction, INotifyPropertyChanged
    {
        private userWcsPoint[] wcsPoints;
        private userWcsCircle _WcsCircle1;
        private userWcsCircle _WcsCircle2;


        [DisplayName("交点")]
        [DescriptionAttribute("输出属性")]
        public userWcsPoint[] WcsPoints { get => wcsPoints; set => wcsPoints = value; }


        [DisplayName("输入圆1")]
        [DescriptionAttribute("输入属性1")]
        public userWcsCircle WcsCircle1
        {
            get
            {
                if (this.RefSource1.Count > 0)
                {
                    object oo = this.GetPropertyValue(this.RefSource1).Last();
                    if (oo != null)
                    {
                        switch (oo.GetType().Name)
                        {
                            default:
                            case nameof(userWcsCircle):
                                this._WcsCircle2 = oo as userWcsCircle;
                                break;
                            case nameof(userWcsCircleSector):
                                this._WcsCircle2 = (oo as userWcsCircleSector).GetWcsCircle();
                                break;
                        }
                    }
                }
                else
                    this._WcsCircle1 = new userWcsCircle();
                return this._WcsCircle1;
            }
            set
            {
                this._WcsCircle1 = value;
            }
        }

        [DisplayName("输入圆2")]
        [DescriptionAttribute("输入属性2")]
        public userWcsCircle WcsCircle2
        {
            get
            {
                if (this.RefSource2.Count > 0)
                {
                    object oo = this.GetPropertyValue(this.RefSource2).Last();
                    if (oo != null)
                    {
                        switch (oo.GetType().Name)
                        {
                            default:
                            case nameof(userWcsCircle):
                                this._WcsCircle2 = oo as userWcsCircle;
                                break;
                            case nameof(userWcsCircleSector):
                                this._WcsCircle2 = (oo as userWcsCircleSector).GetWcsCircle();
                                break;
                        }
                    }
                }
                else
                    this._WcsCircle2 = new userWcsCircle();
                return this._WcsCircle2;
            }
            set
            {
                this._WcsCircle2 = value;
            }
        }

        public IntersectionCircleCircle()
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
                Result.Succss = HalconLibrary.IntersectionPoint(this.WcsCircle1, this.WcsCircle2, out this.wcsPoints);
                stopwatch.Stop();
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[0].SetValue(this.name, "X1", this.wcsPoints[0].X);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[1].SetValue(this.name, "Y1", this.wcsPoints[0].Y);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[2].SetValue(this.name, "Z1", this.wcsPoints[0].Z);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[3].SetValue(this.name, "X2", this.wcsPoints[1].X);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[4].SetValue(this.name, "Y2", this.wcsPoints[1].Y);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[5].SetValue(this.name, "Z2", this.wcsPoints[1].Z);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[6].SetValue(this.name, "Time(ms)", stopwatch.ElapsedMilliseconds);
                //OnExcuteCompleted(this.name, this.wcsPoints);
                int i = 0;
                foreach (var item in this.wcsPoints)
                {
                    OnExcuteCompleted(item.CamName, item.ViewWindow, this.name + index + i.ToString(), item);
                    i++;
                }
            }
            catch (Exception ee)
            {
                LoggerHelper.Error(this.name + "->执行报错", ee);
                this.Result.Succss = false;
            }
            if (Result.Succss)
                LoggerHelper.Info(this.name + "->执行成功:" + string.Format("x={0},y={1},z={2},x={3},y={4},z={5}", this.wcsPoints[0].X, this.wcsPoints[0].Y, 0, this.wcsPoints[1].X, this.wcsPoints[1].Y, 0));
            else
                LoggerHelper.Error(this.name + "->执行失败:" + string.Format("x={0},y={1},z={2},x={3},y={4},z={5}", this.wcsPoints[0].X, this.wcsPoints[0].Y, 0, this.wcsPoints[1].X, this.wcsPoints[1].Y, 0));
            // 更改UI字体　
            UpdataNodeElementStyle(param, Result.Succss);
            return Result;
        }
        public object GetPropertyValues(string propertyName)
        {
            switch (propertyName)
            {
                default:
                case "名称":
                case nameof(this.Name):
                    return this.name;
                case "点对象":
                    return this.wcsPoints;
                case "输出对象":
                case nameof(this.WcsPoints):
                    return this.wcsPoints;
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

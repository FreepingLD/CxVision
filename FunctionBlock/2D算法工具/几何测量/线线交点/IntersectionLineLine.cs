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
    [DefaultProperty(nameof(WcsPoints))]
    public class IntersectionLineLine : BaseFunction, IFunction, INotifyPropertyChanged
    {
        private userWcsPoint wcsPoints;
        private userWcsLine _WcsLine1;
        private userWcsLine _WcsLine2;


        [DisplayName("交点")]
        [DescriptionAttribute("输出属性")]
        public userWcsPoint WcsPoints { get => wcsPoints; set => wcsPoints = value; }

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

        [DisplayName("Wcs_X")]
        [DescriptionAttribute("输出属性")]
        public double Wcs_X
        {
            get
            {
                return this.wcsPoints.X;
            }
        }

        [DisplayName("Wcs_Y")]
        [DescriptionAttribute("输出属性")]
        public double Wcs_Y
        {
            get
            {
                return this.wcsPoints.Y;
            }
        }

        [DisplayName("Cam_X")]
        [DescriptionAttribute("输出属性")]
        public double Cam_X { get; set; }

        [DisplayName("Cam_Y")]
        [DescriptionAttribute("输出属性")]
        public double Cam_Y { get; set; }

        public IntersectionLineLine()
        {
            this.ResultInfo = new BindingList<MeasureResultInfo>();
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name, "X", 0));
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name, "Y", 0));
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name, "Z", 0));
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name, "Cam_X", 0));
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name, "Cam_X", 0));
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name, "Time(ms)", 0));
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
                this.Result.Succss = HalconLibrary.IntersectionPoint(this.WcsLine1, this.WcsLine2, out this.wcsPoints);
                stopwatch.Stop();
                //this.InitBindingTable();
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[0].SetValue(this.name, "X", this.wcsPoints.X);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[1].SetValue(this.name, "Y", this.wcsPoints.Y);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[2].SetValue(this.name, "Z", this.wcsPoints.Z);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[3].SetValue(this.name, "Cam_X", this.wcsPoints.X - this.wcsPoints.Grab_x);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[4].SetValue(this.name, "Cam_Y", this.wcsPoints.Y - this.wcsPoints.Grab_y);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[5].SetValue(this.name, "Time(ms)", stopwatch.ElapsedMilliseconds);
                OnExcuteCompleted(this.name, this.wcsPoints);
            }
            catch (Exception ee)
            {
                LoggerHelper.Error(this.name + "->执行报错", ee);
                this.Result.Succss = false;
            }
            if (this.Result.Succss)
                LoggerHelper.Info(this.name + "执行成功:" + string.Format("x={0},y={1},z={2}", this.wcsPoints.X, this.wcsPoints.Y, 0));
            else
                LoggerHelper.Error(this.name + "执行失败:" + string.Format("x={0},y={1},z={2}", this.wcsPoints.X, this.wcsPoints.Y, 0));
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
                case nameof(this.Name):
                    return this.name;
                case "输出对象":
                case nameof(this.WcsPoints):
                    return this.wcsPoints;
            }
        }
        public bool SetPropertyValues(string propertyName, params object[] value)
        {
            HalconLibrary ha = new HalconLibrary();
            switch (propertyName)
            {
                case "名称":
                case nameof(this.Name):
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

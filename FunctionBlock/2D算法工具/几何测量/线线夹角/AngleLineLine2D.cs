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
    [DefaultProperty(nameof(Deg))]
    public class AngleLineLine2D : BaseFunction, IFunction, INotifyPropertyChanged
    {
        private double deg;
        private userWcsLine _WcsLine1;
        private userWcsLine _WcsLine2;

        [DisplayName("夹角")]
        [DescriptionAttribute("输出属性")]
        public double Deg { get => deg; set => deg = value; }

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



        public AngleLineLine2D()
        {
            this.ResultInfo = new BindingList<MeasureResultInfo>();
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
                Result.Succss = HalconLibrary.LineLineAngle(this.WcsLine1, this.WcsLine2, out this.deg);
                stopwatch.Stop();
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[0].SetValue(this.name, "夹角", this.deg);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[1].SetValue(this.name, "Time(ms)", stopwatch.ElapsedMilliseconds);
            }
            catch (Exception ex)
            {
                LoggerHelper.Error(this.name + "->执行报错", ex);
                this.Result.Succss = false;
            }
            if (Result.Succss)
                LoggerHelper.Info(this.name + "->执行成功:" + this.deg.ToString());
            else
                LoggerHelper.Error(this.name + "->执行失败:" + this.deg.ToString());
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
                case "角度":
                case nameof(this.Deg):
                    return this.deg; //
            }
        }
        public bool SetPropertyValues(string propertyName, params object[] value)
        {
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

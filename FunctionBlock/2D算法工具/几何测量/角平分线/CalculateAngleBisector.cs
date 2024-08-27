using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlgorithmsLibrary;
using HalconDotNet;
using System.Data;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Common;
using System.Diagnostics;

namespace FunctionBlock
{
    [Serializable]
    [DefaultProperty("WcsLine")]
    public class CalculateAngleBisector : BaseFunction, IFunction
    {
        private userWcsLine _wcsLine;
        private userWcsLine _WcsLine1;
        private userWcsLine _WcsLine2;


        [DisplayName("直线对象")]
        [DescriptionAttribute("输出属性")]
        public userWcsLine WcsLine { get => _wcsLine; set => _wcsLine = value; }


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
                //else
                //    this._WcsLine1 = new userWcsLine();
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
                //else
                //    this._WcsLine2 = new userWcsLine();
                return this._WcsLine2;
            }
            set
            {
                this._WcsLine2 = value;
            }
        }


        public CalculateAngleBisector()
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
                string index = "";
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                if (param != null)
                {
                    foreach (var item in param)
                    {
                        if (item == null) continue;
                        switch (item.GetType().Name)
                        {
                            case nameof(String):
                                if (item.ToString().Split('=').Length > 0)
                                    index = item.ToString().Split('=').Last();
                                break;
                        }
                    }
                }
                stopwatch.Stop();
                this.Result.Succss = HalconLibrary.AngleBisector(this.WcsLine1, this.WcsLine2, out this._wcsLine);
                //this.Result.DataContent = this._wcsLine;
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[0].SetValue(this.name, "X1", this._wcsLine.X1);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[1].SetValue(this.name, "Y1", this._wcsLine.Y1);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[2].SetValue(this.name, "Z1", this._wcsLine.Z1);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[3].SetValue(this.name, "X2", this._wcsLine.X2);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[4].SetValue(this.name, "Y2", this._wcsLine.Y2);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[5].SetValue(this.name, "Z2", this._wcsLine.Z2);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[6].SetValue(this.name, "Time(ms)", stopwatch.ElapsedMilliseconds);
                OnExcuteCompleted(this.WcsLine.CamName, this.WcsLine?.ViewWindow, this.name + index, this._wcsLine);
            }
            catch (Exception ex)
            {
                LoggerHelper.Error(this.name + "->执行报错", ex);
                this.Result.Succss = false;
            }
            if (this.Result.Succss)
                LoggerHelper.Info(this.name + "->执行成功:" + this._wcsLine.ToString());
            else
                LoggerHelper.Error(this.name + "->执行失败:" + this._wcsLine.ToString());
            // 更改UI字体　
            UpdataNodeElementStyle(param, this.Result.Succss);
            return this.Result;
        }
        public object GetPropertyValues(string propertyName)
        {
            switch (propertyName)
            {
                case "名称":
                default:
                case nameof(Name):
                    return this.name;
                case nameof(WcsLine):
                    return this._wcsLine;
            }
        }
        public bool SetPropertyValues(string propertyName, params object[] value)
        {
            HalconLibrary ha = new HalconLibrary();
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
                if (this._wcsLine != null)
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

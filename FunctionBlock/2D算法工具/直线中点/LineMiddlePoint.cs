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
    [DefaultProperty(nameof(WcsVector))]
    public class LineMiddlePoint : BaseFunction, IFunction, INotifyPropertyChanged
    {
        private userWcsVector _wcsVector;
        private userWcsLine _WcsLine;

        [DisplayName("中点")]
        [DescriptionAttribute("输出属性")]
        public userWcsVector WcsVector { get => _wcsVector; set => _wcsVector = value; }

        [DisplayName("输入直线")]
        [DescriptionAttribute("输入属性1")]
        public userWcsLine WcsLine
        {
            get
            {
                if (this.RefSource1.Count > 0)
                {
                    object[] oo = this.GetPropertyValue(this.RefSource1);
                    if (oo != null && oo.Length > 0)
                    {
                        foreach (var item in oo)
                        {
                            switch (item.GetType().Name)
                            {
                                case nameof(userWcsLine):
                                    this._WcsLine = item as userWcsLine;
                                    break;
                                case nameof(userPixLine):
                                    userPixLine pixLine = item as userPixLine;
                                    this._WcsLine = pixLine.GetWcsLine();
                                    break;
                            }
                        }
                    }
                }
                //else
                //    this._WcsLine = new userWcsLine();
                return this._WcsLine;
            }
            set
            {
                this._WcsLine = value;
            }
        }



        public LineMiddlePoint()
        {
            this.ResultInfo = new BindingList<MeasureResultInfo>();
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo());
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo());
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo());
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo());
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo());
        }






        #region 实现接口

        public OperateResult Execute(params object[] param)
        {
            this.Result.Succss = true;
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Restart();
            try
            {
                this.Result.Succss = HalconLibrary.LineMiddlePoint(this.WcsLine, out this._wcsVector);
                stopwatch.Stop();
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[0].SetValue(this.name, "X", this._wcsVector.X);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[1].SetValue(this.name, "Y", this._wcsVector.Y);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[2].SetValue(this.name, "Z", this._wcsVector.Z);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[3].SetValue(this.name, "Angle", this._wcsVector.Angle);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[4].SetValue(this.name, "Time(ms)", stopwatch.ElapsedMilliseconds);
                OnExcuteCompleted(this._wcsVector.CamName, this._wcsVector?.ViewWindow, this.name, this._wcsVector);
            }
            catch (Exception ee)
            {
                LoggerHelper.Error(this.name + "->执行报错", ee);
                this.Result.Succss = false;
            }
            if (this.Result.Succss)
                LoggerHelper.Info(this.name + "执行成功");
            else
                LoggerHelper.Error(this.name + "执行失败");
            // 更改UI字体　
            UpdataNodeElementStyle(param, this.Result.Succss);
            return this.Result;
        }
        public object GetPropertyValues(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(Name):
                    return this.name;
                case nameof(WcsVector):
                default:
                    return this._wcsVector;
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

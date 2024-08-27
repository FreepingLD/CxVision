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

namespace FunctionBlock
{
    [Serializable]
    [DefaultProperty(nameof(ObtuseAngle))]
    public class LineAngle2D : BaseFunction, IFunction, INotifyPropertyChanged
    {
        private double _obtuseAngle;
        private double _acuteAngle;
        private userWcsLine _WcsLine;


        [DisplayName("角度1")]
        [DescriptionAttribute("输出属性")]
        public double ObtuseAngle { get => _obtuseAngle; set => _obtuseAngle = value; }

        [DisplayName("角度2")]
        [DescriptionAttribute("输出属性")]
        public double AcuteAngle { get => _obtuseAngle; set => _obtuseAngle = value; }


        [DisplayName("输入直线")]
        [DescriptionAttribute("输入属性1")]
        public userWcsLine WcsLine
        {
            get
            {
                if (this.RefSource1.Count > 0)
                {
                    this._WcsLine = this.GetPropertyValue(this.RefSource1)[0] as userWcsLine;
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

        public string Axis_Ref { get; set; }



        public LineAngle2D()
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
                Result.Succss = HalconLibrary.LineAngle(this.WcsLine, Axis_Ref, out this._obtuseAngle, out this._acuteAngle);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[0].SetValue(this.name, "角度1", this._obtuseAngle);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[0].SetValue(this.name, "角度2", this._acuteAngle);
            }
            catch (Exception ex)
            {
                LoggerHelper.Error(this.name + "->计算直线角度执行报错", ex);
                this.Result.Succss = false;
            }
            if (Result.Succss)
                LoggerHelper.Info(this.name + "->计算直线角度执行成功:" + this._obtuseAngle.ToString());
            else
                LoggerHelper.Error(this.name + "->计算直线角度执行失败:" + this._obtuseAngle.ToString());
            // 更改UI字体　
            UpdataNodeElementStyle(param, Result.Succss);
            return Result;
        }

        public object GetPropertyValues(string propertyName)
        {
            switch (propertyName)
            {
                case "名称":
                case nameof(this.Name):
                    return this.name;
                default:
                case "角度":
                case nameof(this.ObtuseAngle):
                    return this._obtuseAngle; //
                case "直线对象":
                    return this.WcsLine; //
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

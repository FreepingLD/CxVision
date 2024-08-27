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
    [DefaultProperty(nameof(Line))]
    public class FitLine : BaseFunction, IFunction
    {
        private userWcsLine _Line;

        private userWcsLine [] _WcsLine;
        public LineFitParam FitParam
        {
            get;
            set;
        }
        [DisplayName("输出直线对象")]
        [DescriptionAttribute("输出属性")]
        public userWcsLine Line { get => _Line; set => _Line = value; }

        [DisplayName("输入直线对象")]
        [DescriptionAttribute("输入属性1")]
        public userWcsLine [] WcsLine
        {
            get
            {
                try
                {
                    if (this.RefSource1.Count > 0)
                    {
                        var oo = this.GetPropertyValue(this.RefSource1);
                        List<userWcsLine> listLine = new List<userWcsLine>();
                        foreach (var item in oo)
                        {
                           switch(item.GetType().Name)
                            {
                                case "userWcsLine":
                                    listLine.Add((userWcsLine)item);
                                    break;
                            }
                        }
                        this._WcsLine = listLine.ToArray();
                        listLine.Clear();
                    }                  
                    else
                        this._WcsLine = null;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return _WcsLine;
            }
            set { _WcsLine = value; }
        }

        public FitLine()
        {
            this.FitParam = new LineFitParam();
            this.ResultInfo = new BindingList<MeasureResultInfo>();
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
                this.Result.Succss = GeometryFitMethod.Instance.FitLine(this.WcsLine,this.FitParam,out this._Line);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[0].SetValue(this.name, "X1", this._Line.X1);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[1].SetValue(this.name, "Y1", this._Line.Y1);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[2].SetValue(this.name, "Z1", this._Line.Z1);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[3].SetValue(this.name, "X2", this._Line.X2);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[4].SetValue(this.name, "Y2", this._Line.Y2);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[5].SetValue(this.name, "Z2", this._Line.Z2);
                /////////////////////////////////////////////////
                OnExcuteCompleted(this._Line.CamName, this._Line.CamParams?.ViewWindow, this.name, this._Line);
            }
            catch (Exception ex)
            {
                LoggerHelper.Error(this.name + "->执行错误" + ex);
                return this.Result;
            }
            if (this.Result.Succss)
                LoggerHelper.Info(this.name + "->执行成功");
            else
                LoggerHelper.Error(this.name + "->执行失败");
            return this.Result;
        }

        public object GetPropertyValues(string propertyName)
        {
            switch (propertyName)
            {
                case "名称":
                case "Name":
                    return this.name;       
                case nameof(this.Line):
                    return this._Line; //
                default:
                        return this._Line;
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

            }
        }
        public void Read(string path)
        {
            //throw new NotImplementedException();
        }
        public void Save(string path)
        {
           // throw new NotImplementedException();
        }
        #endregion




    }
}

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

    /// <summary>
    /// 将点去数据生成3D对象模型，以便于后续算子操作
    /// </summary>
    [Serializable]
    [DefaultProperty(nameof(ExtendWcsLine))]
    public class LineExtend : BaseFunction, IFunction
    {
        private userWcsLine _extendWcsLine;

        private userWcsLine _WcsLine;
        public LineExtendParam ExtendParam { get; set; }

        [DisplayName("输出直线")]
        [DescriptionAttribute("输出属性")]
        public userWcsLine ExtendWcsLine { get => _extendWcsLine; set => _extendWcsLine = value; }


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


        public LineExtend()
        {
            this.ExtendParam = new LineExtendParam();
            this.ResultInfo = new BindingList<MeasureResultInfo>();
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo());
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo());
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo());
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
            this.Result.Succss = true;
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
                this.Result.Succss = this.ExtendParam.extendLine(this.WcsLine, out _extendWcsLine);
                stopwatch.Stop();
                ///////////////////////////////////////////
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[0].SetValue(this.name, "X1", this._extendWcsLine.X1);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[1].SetValue(this.name, "Y1", this._extendWcsLine.Y1);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[2].SetValue(this.name, "Z1", this._extendWcsLine.Z1);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[3].SetValue(this.name, "X2", this._extendWcsLine.X2);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[4].SetValue(this.name, "Y2", this._extendWcsLine.Y2);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[5].SetValue(this.name, "Z2", this._extendWcsLine.Z2);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[6].SetValue(this.name, "Grab_X", this._extendWcsLine.Grab_x);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[7].SetValue(this.name, "Grab_Y", this._extendWcsLine.Grab_y);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[8].SetValue(this.name, "Grab_Theta", this._extendWcsLine.Grab_theta);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[9].SetValue(this.name, "时间(ms)", stopwatch.ElapsedMilliseconds);
                ////////////////////////////////////////////
                OnExcuteCompleted(this.WcsLine.CamName, this.WcsLine?.ViewWindow, this.name + index, this._extendWcsLine);
            }
            catch (Exception ex)
            {
                LoggerHelper.Error(this.name + "->执行错误", ex);
                this.Result.Succss = false;
            }
            if (this.Result.Succss)
                LoggerHelper.Info(this.name + "->执行成功：" + this._extendWcsLine.ToString());
            else
                LoggerHelper.Error(this.name + "->执行失败：" + this._extendWcsLine.ToString());
            // 更改UI字体　
            UpdataNodeElementStyle(param, this.Result.Succss);
            return this.Result;
        }
        public object GetPropertyValues(string propertyName)
        {
            switch (propertyName)
            {
                case "名称":
                case nameof(this.Name):
                    return this.name;
                case "直线对象":
                default:
                case nameof(this.ExtendWcsLine):
                    return this.ExtendWcsLine; //
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

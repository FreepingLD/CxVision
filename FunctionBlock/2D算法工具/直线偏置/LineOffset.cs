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
    [DefaultProperty(nameof(OffsetWcsLine))]
    public class LineOffset : BaseFunction, IFunction
    {
        private userWcsLine _offsetWcsLine;

        private userWcsLine _WcsLine;
        public LineOffsetParam OffsetParam { get; set; }

        [DisplayName("输出直线")]
        [DescriptionAttribute("输出属性")]
        public userWcsLine OffsetWcsLine { get => _offsetWcsLine; set => _offsetWcsLine = value; }


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


        public LineOffset()
        {
            this.OffsetParam = new LineOffsetParam();
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
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                this.OffsetParam.Scale = 1;
                ///// 检测传入的参数是否包含有图像 ///
                if (param != null)
                {
                    foreach (var item in param)
                    {
                        if (item != null)
                        {
                            switch (item.GetType().Name)
                            {
                                case nameof(Int32):
                                    int result = 0;
                                    if (int.TryParse(item.ToString(), out result))
                                        this.OffsetParam.Scale = result; // 如果循环，那么则倍增 
                                    break;
                                case nameof(String):
                                    string index = "1";
                                    if (item.ToString().Split('=').Length > 0)
                                        index = item.ToString().Split('=').Last();
                                    if (int.TryParse(index, out result))
                                        this.OffsetParam.Scale = result;
                                    break;
                            }
                        }
                    }
                }
                this.Result.Succss = this.OffsetParam.offsetLine(this.WcsLine, out _offsetWcsLine);
                stopwatch.Stop();
                ///////////////////////////////////////////
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[0].SetValue(this.name, "X1", this._offsetWcsLine.X1);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[1].SetValue(this.name, "Y1", this._offsetWcsLine.Y1);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[2].SetValue(this.name, "Z1", this._offsetWcsLine.Z1);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[3].SetValue(this.name, "X2", this._offsetWcsLine.X2);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[4].SetValue(this.name, "Y2", this._offsetWcsLine.Y2);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[5].SetValue(this.name, "Z2", this._offsetWcsLine.Z2);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[6].SetValue(this.name, "Grab_X", this._offsetWcsLine.Grab_x);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[7].SetValue(this.name, "Grab_Y", this._offsetWcsLine.Grab_y);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[8].SetValue(this.name, "Grab_Theta", this._offsetWcsLine.Grab_theta);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[9].SetValue(this.name, "时间(ms)", stopwatch.ElapsedMilliseconds);
                ////////////////////////////////////////////
                //OnExcuteCompleted(this.name + this.OffsetParam.Scale.ToString(), this._offsetWcsLine);
                OnExcuteCompleted(this.WcsLine.CamName, this.WcsLine?.ViewWindow, this.name + this.OffsetParam.Scale.ToString(), this._offsetWcsLine);
            }
            catch (Exception ex)
            {
                LoggerHelper.Error(this.name + "->执行错误", ex);
                this.Result.Succss = false;
            }
            if (this.Result.Succss)
                LoggerHelper.Info(this.name + "->执行成功：" + this._offsetWcsLine.ToString());
            else
                LoggerHelper.Error(this.name + "->执行失败：" + this._offsetWcsLine.ToString());
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
                case nameof(this.OffsetWcsLine):
                    return this.OffsetWcsLine; //
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

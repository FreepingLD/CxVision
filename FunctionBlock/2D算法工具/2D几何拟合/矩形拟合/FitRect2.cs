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
    [DefaultProperty(nameof(Rect2))]
    public class FitRect2 : BaseFunction, IFunction
    {
        private userWcsRectangle2 _Rect2;
        private userWcsLine[] _WcsLine;

        [DisplayName("输入直对象")]
        [DescriptionAttribute("输入属性1")]
        public userWcsLine[] WcsLine
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
                            switch (item.GetType().Name)
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
        public Rect2FitParam FitParam
        {
            get;
            set;
        }

        [DisplayName("矩形对象")]
        [DescriptionAttribute("输出属性")]
        public userWcsRectangle2 Rect2 { get => _Rect2; set => _Rect2 = value; }

        public FitRect2()
        {
            this.FitParam = new Rect2FitParam();
            InitBindingTable();
        }
        private void InitBindingTable()
        {
            if (this.ResultInfo == null)
            {
                this.ResultInfo = new BindingList<MeasureResultInfo>();
                ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name, "X", 0));
                ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name, "Y", 0));
                ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name, "Z", 0));
                ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name, "角度", 0));
                ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name, "半宽", 0));
                ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name, "半高", 0));
            }
        }


        #region  实现接口的部分
        public OperateResult Execute(params object[] param)
        {
            this.Result.Succss = false;
            try
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                this.Result.Succss = GeometryFitMethod.Instance.FitRect2(this.WcsLine,this.FitParam,out this._Rect2);
                stopwatch.Stop();
                InitBindingTable();
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[0].SetValue(this.name, "X", this._Rect2.X);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[1].SetValue(this.name, "Y", this._Rect2.Y);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[2].SetValue(this.name, "Z", this._Rect2.Z);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[3].SetValue(this.name, "角度", this._Rect2.Deg);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[4].SetValue(this.name, "半宽", this._Rect2.Length1);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[5].SetValue(this.name, "半高", this._Rect2.Length2);
                OnExcuteCompleted(this.WcsLine[0].CamName, this.WcsLine[0].CamParams?.ViewWindow, this.name, this._Rect2);
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
                case nameof(this.Rect2):
                    return this._Rect2; //
                default:
                        return this._Rect2;
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
            //throw new NotImplementedException();
        }



        #endregion


    }
}

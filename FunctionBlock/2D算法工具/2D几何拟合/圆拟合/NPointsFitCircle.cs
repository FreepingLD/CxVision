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
    [DefaultProperty(nameof(Circle))]
    public class NPointsFitCircle : BaseFunction, IFunction
    {
        private userWcsCircle _Circle;
        private userWcsPoint[] _WcsPoint;

        public CircleFitParam FitParam
        {
            get;
            set;
        }

        [DisplayName("输入点对象")]
        [DescriptionAttribute("输入属性1")]
        public userWcsPoint[] WcsPoint
        {
            get
            {
                try
                {
                    if (this.RefSource1.Count > 0)
                    {
                        var oo = this.GetPropertyValue(this.RefSource1);
                        List<userWcsPoint> list = new List<userWcsPoint>();
                        foreach (var item in oo)
                        {
                            switch(item.GetType().Name)
                            {
                                case "userWcsPoint":
                                    list.Add((userWcsPoint)item);
                                    break;
                            }
                        }
                        this._WcsPoint = list.ToArray();
                        list.Clear();
                    }
                    else
                        this._WcsPoint = null;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return _WcsPoint;
            }
            set { _WcsPoint = value; }
        }

        [DisplayName("输出圆对象")]
        [DescriptionAttribute("输出属性")]
        public userWcsCircle Circle { get => _Circle; set => _Circle = value; }

        public NPointsFitCircle()
        {
            this.FitParam = new CircleFitParam();
            this.ResultInfo = new BindingList<MeasureResultInfo>();
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
                this.Result.Succss = GeometryFitMethod.Instance.NPointFitCircle(this.WcsPoint,this.FitParam,out this._Circle);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[0].SetValue(this.name, "X", this._Circle.X);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[1].SetValue(this.name, "Y", this._Circle.Y);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[2].SetValue(this.name, "Z", this._Circle.Z);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[3].SetValue(this.name, "半径", this._Circle.Radius);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[4].SetValue(this.name, "直径", this._Circle.Radius * 2);
                OnExcuteCompleted(this.WcsPoint[0].CamName, this.WcsPoint[0].CamParams?.ViewWindow, this.name, this._Circle);
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
                case nameof(this.Circle):
                    return this._Circle; //
                default:
                    return this._Circle;
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

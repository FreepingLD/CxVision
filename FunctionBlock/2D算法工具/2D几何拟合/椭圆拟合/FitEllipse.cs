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
    [DefaultProperty(nameof(Ellipse))]
    public class FitEllipse : BaseFunction, IFunction
    {
        private userWcsEllipseSector[] _WcsEllipseSector;

        private userWcsEllipse _Ellipse;
        public EllipseFitParam FitParam
        {
            get;
            set;
        }

        [DisplayName("输出椭圆对象")]
        [DescriptionAttribute("输出属性")]
        public userWcsEllipse Ellipse { get => _Ellipse; set => _Ellipse = value; }

        [DisplayName("输入椭圆弧对象")]
        [DescriptionAttribute("输入属性1")]
        public userWcsEllipseSector[] WcsEllipseSector
        {
            get
            {
                try
                {
                    if (this.RefSource1.Count > 0)
                    {
                        var oo = this.GetPropertyValue(this.RefSource1);
                        List<userWcsEllipseSector> list = new List<userWcsEllipseSector>();
                        foreach (var item in oo)
                        {
                            switch(item.GetType().Name)
                            {
                                case "userWcsEllipseSector":
                                    list.Add((userWcsEllipseSector)item);
                                    break;
                            }
                        }
                        this._WcsEllipseSector = list.ToArray();
                        list.Clear();
                    }   
                    else
                        this._WcsEllipseSector = null;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return _WcsEllipseSector;
            }
            set { _WcsEllipseSector = value; }
        }

        public FitEllipse()
        {
            this.FitParam = new EllipseFitParam();
            this.ResultInfo = new BindingList<MeasureResultInfo>();
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo());
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo());
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
                this.Result.Succss = GeometryFitMethod.Instance.FitEllipse(this.WcsEllipseSector,this.FitParam,out this._Ellipse);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[0].SetValue(this.name, "X", this._Ellipse.X);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[1].SetValue(this.name, "Y", this._Ellipse.Y);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[2].SetValue(this.name, "Z", this._Ellipse.Z);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[3].SetValue(this.name, "角度", this._Ellipse.Deg);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[4].SetValue(this.name, "半径R1", this._Ellipse.Radius1);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[5].SetValue(this.name, "半径R2", this._Ellipse.Radius2);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[6].SetValue(this.name, "直径D1", this._Ellipse.Radius1 * 2);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[7].SetValue(this.name, "直径D2", this._Ellipse.Radius2 * 2);
                OnExcuteCompleted(this.WcsEllipseSector[0].CamName, this.WcsEllipseSector[0].CamParams?.ViewWindow, this.name, this._Ellipse);
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
                case nameof(this.Ellipse):
                    return this.Ellipse; //
                default:
                    return this.Ellipse;
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

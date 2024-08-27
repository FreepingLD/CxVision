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
    [DefaultProperty(nameof(Rect2))]
    public class FitRect2Region : BaseFunction, IFunction
    {
        private userWcsRectangle2[] _Rect2;
        private RegionDataClass _regionData;

        [DisplayName("输入区域")]
        [DescriptionAttribute("输入属性1")]
        public RegionDataClass RegionData
        {
            get
            {
                try
                {
                    if (this.RefSource1.Count > 0)
                    {
                        var oo = this.GetPropertyValue(this.RefSource1);
                        List<RegionDataClass> listLine = new List<RegionDataClass>();
                        foreach (var item in oo)
                        {
                            if (item == null) continue;
                            switch (item.GetType().Name)
                            {
                                case nameof(RegionDataClass):
                                    listLine.Add((RegionDataClass)item);
                                    break;
                            }
                        }
                        this._regionData = listLine.Last();
                        listLine.Clear();
                    }
                    else
                        this._regionData = null;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return _regionData;
            }
            set { _regionData = value; }
        }
        public Rect2FitParam FitParam
        {
            get;
            set;
        }

        [DisplayName("输出矩形")]
        [DescriptionAttribute("输出属性")]
        public userWcsRectangle2[] Rect2 { get => _Rect2; set => _Rect2 = value; }

        [DisplayName("最大矩形")]
        [DescriptionAttribute("输出属性")]
        public userWcsRectangle2 MaxRect2 { get; set; }
        [DisplayName("最小矩形")]
        [DescriptionAttribute("输出属性")]
        public userWcsRectangle2 MinRect2 { get; set; }
        [DisplayName("平均矩形")]
        [DescriptionAttribute("输出属性")]
        public userWcsRectangle2 MeanRect2 { get; set; }

        public FitRect2Region()
        {
            this.FitParam = new Rect2FitParam();
            this.ResultInfo = new BindingList<MeasureResultInfo>();
            /////////////////////////////////////////////////////
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name, "X", 0));
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name, "Y", 0));
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name, "Z", 0));
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name, "角度", 0));
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name, "半宽", 0));
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name, "半高", 0));
            /////////////////////////////////////////////////////
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name, "X", 0));
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name, "Y", 0));
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name, "Z", 0));
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name, "角度", 0));
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name, "半宽", 0));
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name, "半高", 0));
            /////////////////////////////////////////////////////
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name, "X", 0));
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name, "Y", 0));
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name, "Z", 0));
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name, "角度", 0));
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name, "半宽", 0));
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name, "半高", 0));
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name, "Time(ms)", 0));
        }



        #region  实现接口的部分
        public OperateResult Execute(params object[] param)
        {
            this.Result.Succss = false;
            try
            {
                userPixRectangle2[] pixRectangle2;
                this.Result.Succss = FitRegionMethod.Instance.FitRect2Region(this.RegionData.Region, this.FitParam, out pixRectangle2);
                //this.ResultInfo = MeasureResultInfo.InitList(pixRectangle2.Length * 6);
                this._Rect2 = new userWcsRectangle2[pixRectangle2.Length];
                for (int i = 0; i < pixRectangle2.Length; i++)
                {
                    pixRectangle2[i].CamParams = this._regionData.CamParams;
                    this._Rect2[i] = pixRectangle2[i].GetWcsRectangle2(this._regionData.Grab_X, this._regionData.Grab_Y);
                    //////////////////////////////////
                    //((BindingList<MeasureResultInfo>)this.ResultInfo)[i * 6].SetValue(this.name, "X", this._Rect2[i].x);
                    //((BindingList<MeasureResultInfo>)this.ResultInfo)[i * 6 + 1].SetValue(this.name, "Y", this._Rect2[i].y);
                    //((BindingList<MeasureResultInfo>)this.ResultInfo)[i * 6 + 2].SetValue(this.name, "Z", this._Rect2[i].z);
                    //((BindingList<MeasureResultInfo>)this.ResultInfo)[i * 6 + 3].SetValue(this.name, "角度", this._Rect2[i].deg);
                    //((BindingList<MeasureResultInfo>)this.ResultInfo)[i * 6 + 4].SetValue(this.name, "半宽", this._Rect2[i].length1);
                    //((BindingList<MeasureResultInfo>)this.ResultInfo)[i * 6 + 5].SetValue(this.name, "半高", this._Rect2[i].length2);
                }

                double MaxValue = 0;
                double MinValue = double.MaxValue;
                this.MaxRect2 = new userWcsRectangle2();
                this.MinRect2 = new userWcsRectangle2();
                this.MeanRect2 = new userWcsRectangle2();
                List<double> listX = new List<double>();
                List<double> listY = new List<double>();
                List<double> listDeg = new List<double>();
                List<double> listLen1 = new List<double>();
                List<double> listLen2 = new List<double>();
                foreach (var item in this._Rect2)
                {
                    listX.Add(item.X);
                    listY.Add(item.Y);
                    listDeg.Add(item.Deg);
                    listLen1.Add(item.Length1);
                    listLen2.Add(item.Length2);
                    if (item.GetArea() > MaxValue)
                    {
                        MaxValue = item.GetArea();
                        this.MaxRect2 = item;
                    }
                    if (item.GetArea() < MinValue)
                    {
                        MinValue = item.GetArea();
                        this.MinRect2 = item;
                    }
                }
                if (listX.Count > 0)
                    this.MeanRect2 = new userWcsRectangle2(listX.Average(), listY.Average(), 0, listDeg.Average(), listLen1.Average(), listLen2.Average(), 0, 0, this._regionData.CamParams);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[0].SetValue(this.name, "X", this.MaxRect2.X);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[1].SetValue(this.name, "Y", this.MaxRect2.Y);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[2].SetValue(this.name, "Z", this.MaxRect2.Z);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[3].SetValue(this.name, "角度", this.MaxRect2.Deg);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[4].SetValue(this.name, "长度1", this.MaxRect2.Length1*2);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[5].SetValue(this.name, "长度2", this.MaxRect2.Length2 * 2);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[6].SetValue(this.name, "X", this.MinRect2.X);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[7].SetValue(this.name, "Y", this.MinRect2.Y);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[8].SetValue(this.name, "Z", this.MinRect2.Z);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[9].SetValue(this.name, "角度", this.MinRect2.Deg);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[10].SetValue(this.name, "长度1", this.MinRect2.Length1 * 2);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[11].SetValue(this.name, "长度2", this.MinRect2.Length2 * 2);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[12].SetValue(this.name, "X", this.MeanRect2.X);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[13].SetValue(this.name, "Y", this.MeanRect2.Y);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[14].SetValue(this.name, "Z", this.MeanRect2.Z);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[15].SetValue(this.name, "角度", this.MeanRect2.Deg);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[16].SetValue(this.name, "长度1", this.MeanRect2.Length1 * 2);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[17].SetValue(this.name, "长度2", this.MeanRect2.Length2 * 2);
                OnExcuteCompleted(this._regionData.CamName, this._regionData.CamParams?.ViewWindow, this.name, this._Rect2);
            }
            catch (Exception ex)
            {
                LoggerHelper.Error(this.name + "->执行错误" + ex);
                this.Result.Succss = false;
            }
            if (this.Result.Succss)
                LoggerHelper.Info(this.name + "->执行成功");
            else
                LoggerHelper.Error(this.name + "->执行失败");
            // 更改UI字体　
            UpdataNodeElementStyle(param, Result.Succss);
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

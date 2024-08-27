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
    [DefaultProperty(nameof(XldData))]
    public class LineDetect : BaseFunction, IFunction
    {
        [NonSerialized]

        private RegionDataClass _regionData;

        [DisplayName("NG轮廓")]
        [DescriptionAttribute("输出属性")]
        public RegionDataClass XldData { get => _regionData; set => _regionData = value; }

        private userWcsLine[] _WcsLine;
        public LineDetectParam DetectParam
        {
            get;
            set;
        }


        [DisplayName("输入直线")]
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
                        List<userWcsLine> listPoint = new List<userWcsLine>();
                        foreach (var item in oo)
                        {
                            switch (item.GetType().Name)
                            {
                                case "userWcsLine":
                                    listPoint.Add((userWcsLine)item);
                                    break;
                            }
                        }
                        this._WcsLine = listPoint.ToArray();
                        listPoint.Clear();
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



        public LineDetect()
        {
            this.DetectParam = new LineDetectParam();
            this.ResultInfo = new BindingList<MeasureResultInfo>();
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name, "NG区域数量", 0));
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name, "时间", 0));
            //((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name, "NG点数", 0));
        }




        #region  实现接口的部分
        public OperateResult Execute(params object[] param)
        {
            this.Result.Succss = false;
            try
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                HXLDCont hXLDCont;
                userWcsLine wcsLine;
                int num = 0;
               this.Result.Succss = LineMethod.DetectLine(this.WcsLine, this.DetectParam, out hXLDCont, out wcsLine);
                if (hXLDCont != null && hXLDCont.IsInitialized())
                {
                    num = hXLDCont.CountObj();
                    this._regionData = new RegionDataClass(hXLDCont.GenRegionContourXld("margin"));
                }
                else
                    this._regionData = new RegionDataClass(new HRegion());
                this._regionData.CamName = this._WcsLine[0]?.CamName;
                this._regionData.ViewWindow = this._WcsLine[0]?.ViewWindow;
                this._regionData.Color = enColor.red;
                stopwatch.Stop();
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[0].SetValue(this.name, "NG区域数量", num);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[1].SetValue(this.name, "执行时间", stopwatch.ElapsedMilliseconds);
                OnExcuteCompleted(this._WcsLine[0].CamName, this._regionData.ViewWindow, this.name, this._regionData);
                OnExcuteCompleted(this._WcsLine[0].CamName, this._regionData.ViewWindow, this.name + ".拟合直线", wcsLine);
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
                case nameof(this.XldData):
                    return this.XldData; //
                default:
                    return this.XldData;
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

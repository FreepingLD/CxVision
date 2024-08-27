using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HalconDotNet;
using System.Drawing;
using Sensor;
using MotionControlCard;
using System.Windows.Forms;
using System.IO;
using AlgorithmsLibrary;
using System.ComponentModel;
using Common;
using System.Diagnostics;

namespace FunctionBlock
{
    [Serializable]

    [DefaultProperty(nameof(OutRegionData))]
    public class RegionArithmetic : BaseFunction, IFunction
    {
        [NonSerialized]
        private RegionDataClass _regionData;
        [NonSerialized]
        private RegionDataClass _regionData2;
        [NonSerialized]
        private RegionDataClass _outRegionData;



        [DisplayName("输入区域1")]
        [DescriptionAttribute("输入属性1")]
        public RegionDataClass RegionData
        {
            get
            {
                try
                {
                    if (this.RefSource1.Count > 0)
                    {
                        object[] oo = this.GetPropertyValue(this.RefSource1);
                        if(oo != null && oo.Length > 0)
                        {
                            this._regionData = oo[0] as RegionDataClass;
                        }
                    }  
                    //else
                    //{
                    //    this._imageData = null;
                    //}
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return _regionData;
            }
            set
            {
                _regionData = value;
            }
        }

        [DisplayName("输入区域2")]
        [DescriptionAttribute("输入属性2")]
        public RegionDataClass RegionData2
        {
            get
            {
                try
                {
                    if (this.RefSource2.Count > 0)
                    {
                        object[] oo = this.GetPropertyValue(this.RefSource2);
                        if (oo != null && oo.Length > 0)
                        {
                            this._regionData2 = oo[0] as RegionDataClass;
                        }
                    }
                    //else
                    //{
                    //    this._imageData2 = null;
                    //}
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return _regionData2;
            }
            set
            {
                _regionData2 = value;
            }
        }

        [DisplayName("输出区域")]
        [DescriptionAttribute("输出属性")]
        public RegionDataClass OutRegionData { get => _outRegionData; set => _outRegionData = value; }
        public RegionArithmeticParam Param { get; set; }

        public RegionArithmetic()
        {
            this.Param = new RegionArithmeticParam();
            this.ResultInfo = new BindingList<MeasureResultInfo>();
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name));
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name));
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name));
        }



        #region 实现接口
        public OperateResult Execute(params object[] param)
        {
            this.Result.Succss = false;
            try
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Restart();
                HRegion hRegion = new HRegion();
                this.Result.Succss = RegionArithmeticMethod.ArithmeticRegion(this.RegionData.Region, this.RegionData2.Region,this.Param, out hRegion);
                this._outRegionData = new RegionDataClass(hRegion, this._regionData.CamParams);
                this._outRegionData.CamName = this._regionData.CamName;
                this._outRegionData.ViewWindow = this._regionData.ViewWindow;
                int count = hRegion.CountObj();
                double area = hRegion.Area.D;
                stopwatch.Stop();
                if (this._outRegionData != null && this._outRegionData.Region.IsInitialized())
                {
                    ((BindingList<MeasureResultInfo>)this.ResultInfo)[0].SetValue(this.name, "区域数量", count);
                    ((BindingList<MeasureResultInfo>)this.ResultInfo)[1].SetValue(this.name, "区域面积", area);
                    ((BindingList<MeasureResultInfo>)this.ResultInfo)[2].SetValue(this.name, "Time", stopwatch.ElapsedMilliseconds);
                }
                else
                {
                    ((BindingList<MeasureResultInfo>)this.ResultInfo)[0].SetValue(this.name, "区域数量", 0);
                    ((BindingList<MeasureResultInfo>)this.ResultInfo)[1].SetValue(this.name, "区域面积", 0);
                    ((BindingList<MeasureResultInfo>)this.ResultInfo)[2].SetValue(this.name, "Time", 0);
                }
                OnExcuteCompleted(this._outRegionData.CamName,this.name, this._outRegionData);
            }
            catch (Exception ex)
            {
                LoggerHelper.Error(this.name + "->执行错误", ex);
                this.Result.Succss = false;
            }
            if (this.Result.Succss)
                LoggerHelper.Info(this.name + "->执行成功");
            else
                LoggerHelper.Error(this.name + "->执行失败");
            // 更改UI字体　
            UpdataNodeElementStyle(param, this.Result.Succss);
            return this.Result;
        }
        public object GetPropertyValues(string propertyName)
        {
            switch (propertyName)
            {
                case "名称":
                case "Name":
                    return this.name;
                case "OutImageData":
                case "输出区域":
                    return this._outRegionData; //
                case "输入区域1":
                    return this.RegionData; //
                case "输入区域2":
                    return this.RegionData2; //
                default:
                    return this._outRegionData;
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

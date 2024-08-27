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
    [DefaultProperty(nameof(FlawRegion))]
    public class MeasureDetect : BaseFunction, IFunction
    {
        [NonSerialized]
        private ImageDataClass _imageData;
        [NonSerialized]
        private RegionDataClass _flawRegion;

        public MeasureDetectParam DetectParam
        {
            get;
            set;
        }

        [DisplayName("输入图像")]
        [DescriptionAttribute("输入属性1")]
        public ImageDataClass ImageData
        {
            get
            {
                try
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
                                    case nameof(ImageDataClass):
                                        this._imageData = item as ImageDataClass;
                                        break;
                                    case nameof(HImage):
                                        this._imageData = new ImageDataClass((HImage)item);
                                        break;
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return _imageData;
            }
            set
            {
                _imageData = value;
            }
        }


        [DisplayName("输入区域")]
        [DescriptionAttribute("输入属性1")]
        public RegionDataClass FlawRegion { get => _flawRegion; set => _flawRegion = value; }

        public MeasureDetect()
        {
            this.DetectParam = new MeasureDetectParam();
            this.ResultInfo = new BindingList<MeasureResultInfo>();
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name, "NG区域数量", 0));
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name, "NG区域面积", 0));
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name, "时间", 0));
        }




        #region  实现接口的部分
        public OperateResult Execute(params object[] param)
        {
            this.Result.Succss = false;
            try
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                HRegion hRegion;
                int num = 0;
                double area = 0;
                this.Result.Succss = MeasureMethod.Detect(this.ImageData?.Image, this.DetectParam, out hRegion);
                if (hRegion != null && hRegion.IsInitialized())
                {
                    num = hRegion.CountObj();
                    if (num > 0)
                        area = hRegion.Union1().Area;
                    this._flawRegion = new RegionDataClass(hRegion);
                }
                else
                    this._flawRegion = new RegionDataClass(new HRegion());
                this._flawRegion.CamName = this.ImageData?.CamName;
                this._flawRegion.ViewWindow = this.ImageData?.ViewWindow;
                this._flawRegion.Tag = this.ImageData?.Tag;
                this._flawRegion.CamParams = this.ImageData?.CamParams;
                this._flawRegion.Color = enColor.red;
                stopwatch.Stop();
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[0].SetValue(this.name, "NG区域数量", num);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[1].SetValue(this.name, "NG区域面积", area);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[2].SetValue(this.name, "执行时间", stopwatch.ElapsedMilliseconds);
                OnExcuteCompleted(this._flawRegion.CamName, this._flawRegion.ViewWindow, this.name, this._flawRegion);
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
                case nameof(this.FlawRegion):
                    return this.FlawRegion; //
                case "输入对象1":
                case "输入图像":
                    return ImageData;
                case "输出对象":
                case "输出区域":
                    return FlawRegion;
                default:
                    return this.FlawRegion;
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

using Common;
using HalconDotNet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionBlock
{
    [Serializable]
    [DefaultProperty(nameof(OcrResult))]
    public class Ocr : BaseFunction, IFunction
    {
        [NonSerialized]
        private ImageDataClass _imageData;
        [NonSerialized]
        private RegionDataClass _chartRegion;

    
        public DoOcr CharDetection { get; set; }

        [DisplayName("Ocr字符")]
        [DescriptionAttribute("输出属性")]
        public string OcrResult { get; set; }

        [DisplayName("Ocr得分")]
        [DescriptionAttribute("输出属性")]
        public double [] OcrScore { get; set; }

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
        [DescriptionAttribute("输入属性2")]
        public RegionDataClass ChartRegion
        {
            get
            {
                try
                {
                    if (this.RefSource2.Count > 0)
                    {
                        this._chartRegion?.Clear();
                        if (this._chartRegion == null)
                            this._chartRegion = new RegionDataClass();
                        var oo = this.GetPropertyValue(this.RefSource2);
                        foreach (var item in oo)
                        {
                            switch (item.GetType().Name)
                            {
                                case nameof(RegionDataClass):
                                    this._chartRegion.AddRegion(((RegionDataClass)item).Region);
                                    break;
                                case nameof(HRegion):
                                    this._chartRegion.AddRegion(((HRegion)item));
                                    break;
                            }
                        }
                    }
                    else
                        this._chartRegion?.Clear();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return _chartRegion;
            }
            set { _chartRegion = value; }
        }

        public Ocr()
        {
            CharDetection = new DoOcr();
            this.ResultInfo = new BindingList<OcrResultInfo>();
            ((BindingList<OcrResultInfo>)this.ResultInfo).Add(new OcrResultInfo(this.name, "Ocr字符", ""));
            ((BindingList<OcrResultInfo>)this.ResultInfo).Add(new OcrResultInfo(this.name, "字符得分", ""));
            ((BindingList<OcrResultInfo>)this.ResultInfo).Add(new OcrResultInfo(this.name, "字符得分", ""));
        }

        public OperateResult Execute(params object[] param)
        {
            this.Result.Succss = false;
            try
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Restart();
                this.CharDetection.DoOcrDetect(this.ImageData.Image,this.ChartRegion.Region);
                this.OcrResult = this.CharDetection.OcrResult.Character;
                this.Result.Succss = this.CharDetection.OcrResult.Result;
                this.OcrScore = this.CharDetection.OcrResult.Score.ToArray();
                stopwatch.Stop();
                /////////////////////////////////////////
                ((BindingList<OcrResultInfo>)this.ResultInfo)[0].SetValue(this.name, "Ocr字符", this.OcrResult);
                ((BindingList<OcrResultInfo>)this.ResultInfo)[1].SetValue(this.name, "字符得分", string.Join(",", this.OcrScore));
                ((BindingList<OcrResultInfo>)this.ResultInfo)[2].SetValue(this.name, "Time(ms)", stopwatch.ElapsedMilliseconds.ToString());
            }
            catch (Exception ex)
            {
                LoggerHelper.Fatal(this.name + "执行报错" + ex);
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
                case nameof(this.Name):
                    return this.name;

                case nameof(this.ImageData):
                    return this._imageData;

                case nameof(this.OcrScore):
                    return this.OcrScore;

                default:
                case nameof(this.OcrResult):
                    return this.OcrResult;
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



    }
}

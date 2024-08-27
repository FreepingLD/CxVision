using Common;
using HalconDotNet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionBlock
{
    [Serializable]
    [DefaultProperty("OcrResult")]
    public class DoOcr : BaseFunction, IFunction
    {
        private ImageDataClass _imageData;
        public DoOcr()
        {
            OcrCharDetection = new Ocr();
            this.ResultInfo = new BindingList<OcrResultInfo>();
            ((BindingList<OcrResultInfo>)this.ResultInfo).Add(new OcrResultInfo(this.name, "Ocr字符", ""));
            ((BindingList<OcrResultInfo>)this.ResultInfo).Add(new OcrResultInfo(this.name, "字符得分", ""));
        }

      
        public Ocr OcrCharDetection { get; set; }


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
                        this._imageData = this.GetPropertyValue(this.RefSource1).Last() as ImageDataClass;
                    else
                        this._imageData = null;
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

        public bool Execute(params object[] param)
        {
            this.Result = false;
            try
            {
                this.OcrCharDetection.DoOcr(this.ImageData.Image);
                this.OcrResult = this.OcrCharDetection.OcrResult.Character;
                this.Result = this.OcrCharDetection.OcrResult.Result;
                this.OcrScore = this.OcrCharDetection.OcrResult.Score.ToArray();
                /////////////////////////////////////////
                ((BindingList<OcrResultInfo>)this.ResultInfo)[0].SetValue(this.name, "Ocr字符", this.OcrResult);
                ((BindingList<OcrResultInfo>)this.ResultInfo)[1].SetValue(this.name, "字符得分", string.Join(",", this.OcrScore));
            }
            catch (Exception ex)
            {
                LoggerHelper.Error(this.name + "执行报错" + ex);
            }
            if (this.Result)
                LoggerHelper.Info(this.name + "->执行成功");
            else
                LoggerHelper.Error(this.name + "->执行失败");
            // 更改UI字体　
            UpDataElementStyle(param, this.Result);
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
    }
}

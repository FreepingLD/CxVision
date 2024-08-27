using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HalconDotNet;
using Sensor;
using MotionControlCard;
using System.Windows.Forms;
using Common;
using System.IO;
using AlgorithmsLibrary;
using System.ComponentModel;
using System.Diagnostics;

namespace FunctionBlock
{
    [Serializable]
    [DefaultProperty(nameof(SavePath))]
    public class SaveImage : BaseFunction, IFunction
    {
        [NonSerialized]
        private ImageDataClass _imageData;

        [NonSerialized]
        private RegionDataClass _inputRegion;
        public ImageSaveParam SaveParam { get; set; }

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
        public RegionDataClass InputRegion
        {
            get
            {
                try
                {
                    if (this.RefSource2.Count > 0)
                    {
                        this._inputRegion?.Clear();
                        if (this._inputRegion == null)
                            this._inputRegion = new RegionDataClass();
                        var oo = this.GetPropertyValue(this.RefSource2);
                        foreach (var item in oo)
                        {
                            switch (item.GetType().Name)
                            {
                                case nameof(RegionDataClass):
                                    this._inputRegion.AddRegion(((RegionDataClass)item).Region);
                                    break;
                                case nameof(HRegion):
                                    this._inputRegion.AddRegion(((HRegion)item));
                                    break;
                            }
                        }
                    }
                    else
                        this._inputRegion?.Clear();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return _inputRegion;
            }
            set { _inputRegion = value; }
        }

        [DisplayName("保存路径")]
        [DescriptionAttribute("输出属性")]
        public string SavePath
        {
            get;
            set;
        }

        public SaveImage()
        {
            this.SaveParam = new ImageSaveParam();
            this.ResultInfo = new BindingList<OcrResultInfo>();
            ((BindingList<OcrResultInfo>)this.ResultInfo).Add(new OcrResultInfo());
            ((BindingList<OcrResultInfo>)this.ResultInfo).Add(new OcrResultInfo());
        }

        #region  实现接口的部分

        public OperateResult Execute(params object[] param)
        {
            this.Result.Succss = false;
            try
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Restart();
                string index = "1";
                if (param != null)
                {
                    foreach (var item in param)
                    {
                        if (item != null)
                        {
                            switch (item.GetType().Name)
                            {
                                case nameof(UInt32):
                                case nameof(Double):
                                case nameof(Int32):
                                case nameof(String):
                                    index = item.ToString();
                                    break;
                            }
                        }
                    }
                }
                this.Result.Succss = this.SaveParam.SaveImage(this.ImageData, this.InputRegion, index);
                stopwatch.Stop();
                this.SavePath = this.SaveParam.SavePath;
                if (((BindingList<OcrResultInfo>)this.ResultInfo).Count > 0)
                    ((BindingList<OcrResultInfo>)this.ResultInfo)[0].SetValue(this.name, "图像路径", this.SaveParam.SavePath);
                else
                    ((BindingList<OcrResultInfo>)this.ResultInfo).Add(new OcrResultInfo());
                if (((BindingList<OcrResultInfo>)this.ResultInfo).Count > 1)
                    ((BindingList<OcrResultInfo>)this.ResultInfo)[1].SetValue(this.name, "Time(ms)", stopwatch.ElapsedMilliseconds.ToString());
                else
                    ((BindingList<OcrResultInfo>)this.ResultInfo).Add(new OcrResultInfo());

            }
            catch (Exception ex)
            {
                LoggerHelper.Error(this.name + "->执行错误", ex);
                return this.Result;
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
                //case "文件路径":
                //    return this.folderPath;
                default:
                case nameof(this.ImageData):
                    return this.ImageData;
            }
        }

        public bool SetPropertyValues(string propertyName, params object[] value)
        {
            switch (propertyName)
            {
                case "名称":
                case nameof(this.Name):
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

        }
        public void Save(string path)
        {

        }

        #endregion




    }
}

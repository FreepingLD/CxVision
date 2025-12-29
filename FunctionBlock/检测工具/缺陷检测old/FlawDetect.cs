using Common;
using HalconDotNet;
using MotionControlCard;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FunctionBlock
{
    [Serializable]
    [DefaultProperty(nameof(BlobRegion))] // 表示这个类是工具类
    public class FlawDetect : BaseFunction, IFunction
    {
        [NonSerialized]
        private ImageDataClass _imageData;
        [NonSerialized]
        private RegionDataClass threshouldRegion;

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

        public Dictionary<string, FlawDetectParam> DicParam { get; set; }

        [DisplayName("输出区域")]
        [DescriptionAttribute("输出属性")]
        public RegionDataClass BlobRegion { get => threshouldRegion; set => threshouldRegion = value; }

        public FlawDetect()
        {
            this.DicParam = new Dictionary<string, FlawDetectParam>();
            this.ResultInfo = new BindingList<OcrResultInfo>();
            ((BindingList<OcrResultInfo>)this.ResultInfo).Add(new OcrResultInfo(this.name, "TIme(ms)", "0"));
        }

        public OperateResult Execute(params object[] param)
        {
            Stopwatch stopwatch = new Stopwatch();
            this.Result.Succss = false;
            this.Result.ErrorMessage = "";
            this.Result.ExcuteState = enExcuteState.NONE;
            ImageDataClass _imageData = new ImageDataClass();
            try
            {
                stopwatch.Start();
                if (param != null)
                {
                    foreach (var item in param)
                    {
                        if (item != null)
                        {
                            switch (item.GetType().Name)
                            {
                                case nameof(ImageDataClass):
                                    _imageData = item as ImageDataClass;
                                    break;
                            }
                        }
                    }
                }
                /////////////////////////////////////////////
                List<bool> listResult = new List<bool>();
                List<HRegion> listRegion = new List<HRegion>();
                Parallel.ForEach(this.DicParam, item =>
                {
                    HRegion hRegion1;
                    bool tempResult = item.Value.Detect(this.ImageData.Image.Clone(), out hRegion1);
                    listResult.Add(tempResult);
                    listRegion.Add(hRegion1);
                });
                Result.Succss = true;
                foreach (var item in listResult)
                {
                    if (item == false)
                        Result.Succss = false;
                }
                HRegion hRegion = new HRegion();
                hRegion.GenEmptyRegion();
                foreach (var item in listRegion)
                {
                    if (item != null && item.IsInitialized())
                        hRegion = hRegion.ConcatObj(item);
                }
                ///////////////////////////////////////////////////////////////
                this.threshouldRegion = new RegionDataClass(hRegion, this._imageData.Grab_X, this._imageData.Grab_Y, this._imageData.Grab_Z, this._imageData.CamParams);
                //this.threshouldRegion.Draw = BlobParam.OutParam.DrawMode;
                this.threshouldRegion.CamName = this._imageData.CamName;
                this.threshouldRegion.ViewWindow = this._imageData.ViewWindow;
                this.threshouldRegion.Tag = this._imageData.Tag;
                ///////////////////////////////////////////////
                ((BindingList<OcrResultInfo>)this.ResultInfo)[0].SetValue(this.name, "Time(ms)", stopwatch.ElapsedMilliseconds.ToString());
                stopwatch.Stop();
            }
            catch (Exception ex)
            {
                LoggerHelper.Error(this.name + "->执行错误" + ex);
                this.Result.Succss = false;
            }
            if (this.Result.Succss)
                LoggerHelper.Info(this.name + "->执行成功");
            else
                LoggerHelper.Error(this.name + "->执行失败;" + this.Result.ErrorMessage);
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
                default:
                    return ""; // this.FeaturePoint;
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
            catch (Exception ex)
            {
                LoggerHelper.Error(this.name + "删除节点出错" + ex.ToString());
            }
        }
        public void Read(string path)
        {
            // throw new NotImplementedException();
        }
        public void Save(string path)
        {
            //throw new NotImplementedException();
        }

    }
}

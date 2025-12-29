using Common;
using HalconDotNet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FunctionBlock
{
    [Serializable]
    [DefaultProperty(nameof(OcrResult))]
    public class Ocr : BaseFunction, IFunction
    {
        [NonSerialized]
        private ImageDataClass _imageData;
        [NonSerialized]
        private userPixCoordSystem _pixCoordSystem;
        public OcrMethod DeepOcr { get; set; }

        public string[] _ocrResult;
       
        public DeepOcrCreateParam CreateParam { get; set; }
        public DeepOcrRecognitionParam RecognizeParam { get; set; }

        [DisplayName("字符")]
        [DescriptionAttribute("输出属性")]
        public string[] OcrResult { get { return this._ocrResult; } set { this._ocrResult = value; } }

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

        [DisplayName("坐标系")]
        [DescriptionAttribute("输入属性2")]
        public userPixCoordSystem PixCoordSystem
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
                            foreach (var item in oo)
                            {
                                switch (item.GetType().Name)
                                {
                                    case nameof(userWcsCoordSystem):
                                        this._pixCoordSystem = ((userWcsCoordSystem)item).GetPixCoordSystem();
                                        break;
                                    case nameof(userPixCoordSystem):
                                        this._pixCoordSystem = ((userPixCoordSystem)item);
                                        break;
                                }
                            }
                        }
                    }
                    if (this._pixCoordSystem == null)
                        this._pixCoordSystem = new userPixCoordSystem();
                    return _pixCoordSystem;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            set
            {
                this._pixCoordSystem = value;
            }
        }

        public Ocr()
        {
            this.DeepOcr = new OcrMethod();
            this.CreateParam = new DeepOcrCreateParam();
            this.RecognizeParam = new DeepOcrRecognitionParam();
            this.ResultInfo = new BindingList<OcrResultInfo>();
            ((BindingList<OcrResultInfo>)this.ResultInfo).Add(new OcrResultInfo(this.name, "Ocr字符", ""));
            ((BindingList<OcrResultInfo>)this.ResultInfo).Add(new OcrResultInfo(this.name, "字符得分", ""));
        }

        public OperateResult Execute(params object[] param)
        {
            this.Result.Succss = false;
            try
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Restart();
                this.Result.Succss = this.DeepOcr.ApplyDeepOcr(this.ImageData.Image,this.PixCoordSystem, this.RecognizeParam, out _ocrResult);
                stopwatch.Stop();
                /////////////////////////////////////////
                this.CreateResultInfo(_ocrResult.Length + 1);
                for (int i = 0; i < _ocrResult.Length; i++)
                {
                    ((BindingList<OcrResultInfo>)this.ResultInfo)[0].SetValue(this.name, $"字符{i + 1}", _ocrResult[i]);
                }
                ((BindingList<OcrResultInfo>)this.ResultInfo)[_ocrResult.Length].SetValue(this.name, "Time(ms)", stopwatch.ElapsedMilliseconds.ToString());
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
                this.DeepOcr?.ClearDlModelOcr();
            }
            catch
            {
                LoggerHelper.Error(this.name + "->删除该对象报错");
            }
        }


        public void Read(string path)
        {
            try
            {
                string modelPath = path.Replace(".txt", ".hdo");
                this.DeepOcr?.ReadDlModelOcr(modelPath);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.name + "->读取形状模型失败" + ex.ToString());
            }
        }
        public void Save(string path)
        {
            try
            {
                string modelPath = path.Replace(".txt", ".hdo");
                this.DeepOcr?.SaveDlModelOcr(modelPath);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.name + "->保存形状模型失败" + ex.ToString());
            }
            /////////////////////////////////////////////// 以xml的形式保存参数 //////////////////
        }



    }
}

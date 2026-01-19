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
using System.Diagnostics;

namespace FunctionBlock
{

    [Serializable]
    [DefaultProperty(nameof(MirrorImage))]
    public class ImageMirror : BaseFunction, IFunction
    {
        [NonSerialized]
        private ImageDataClass _imageData = null; // 
        [NonSerialized]
        private ImageDataClass mirrorImage = null; // 

        public ImageMirrorParam Param
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


        [DisplayName("输出图像")]
        [DescriptionAttribute("输出属性")]
        public ImageDataClass MirrorImage
        {
            get
            {
                return this.mirrorImage;
            }
            set
            {
                this.mirrorImage = value;
            }
        }

        public ImageMirror()
        {
            this.Param = new ImageMirrorParam();
            this.ResultInfo = new BindingList<MeasureResultInfo>();
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name));
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name));
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name));
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
                HImage hImage;
                this.Result.Succss = this.Param.mirror_image(this.ImageData.Image, out hImage);
                this.mirrorImage = this._imageData.Clone();
                this.mirrorImage.Image = hImage.Clone();
                hImage?.Dispose();
                stopwatch.Stop();
                this.CreateResultInfo(6);
                if (this.mirrorImage != null && this.mirrorImage.Image.IsInitialized())
                {
                    ((BindingList<MeasureResultInfo>)this.ResultInfo)[0].SetValue(this.name, "Width", this.mirrorImage.Width);
                    ((BindingList<MeasureResultInfo>)this.ResultInfo)[1].SetValue(this.name, "Height", this.mirrorImage.Height);
                    ((BindingList<MeasureResultInfo>)this.ResultInfo)[2].SetValue(this.name, "Grab_X", this.mirrorImage.Grab_X);
                    ((BindingList<MeasureResultInfo>)this.ResultInfo)[3].SetValue(this.name, "Grab_Y", this.mirrorImage.Grab_Y);
                    ((BindingList<MeasureResultInfo>)this.ResultInfo)[4].SetValue(this.name, "Grab_Theta", this.mirrorImage.Grab_Theta);
                    ((BindingList<MeasureResultInfo>)this.ResultInfo)[5].SetValue(this.name, "Time(ms)", stopwatch.ElapsedMilliseconds);
                }
                else
                {
                    ((BindingList<MeasureResultInfo>)this.ResultInfo)[0].SetValue(this.name, "Width", 0);
                    ((BindingList<MeasureResultInfo>)this.ResultInfo)[1].SetValue(this.name, "Height", 0);
                    ((BindingList<MeasureResultInfo>)this.ResultInfo)[2].SetValue(this.name, "Grab_X", 0);
                    ((BindingList<MeasureResultInfo>)this.ResultInfo)[3].SetValue(this.name, "Grab_Y", 0);
                    ((BindingList<MeasureResultInfo>)this.ResultInfo)[4].SetValue(this.name, "Grab_Theta", 0);
                    ((BindingList<MeasureResultInfo>)this.ResultInfo)[5].SetValue(this.name, "Time(ms)", 0);
                }
                OnExcuteCompleted(this.name, this.mirrorImage);
            }
            catch (Exception ex)
            {
                LoggerHelper.Error(this.name + "->执行报错", ex);
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
                case nameof(this.Name):
                    return this.name;
                case "变换对象":
                case "输出对象":
                case "镜像图像":
                case nameof(this.MirrorImage):
                    return this.mirrorImage;
                case "输入对象":
                case "输入图像":
                case nameof(this.ImageData):
                    return this.ImageData;
                default:
                    return this.mirrorImage;
            }
        }
        public bool SetPropertyValues(string propertyName, params object[] value)
        {
            switch (propertyName)
            {
                case "名称":
                    this.name = value[0].ToString();
                    return true;
                /////////////////////             
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

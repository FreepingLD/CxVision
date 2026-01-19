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
    [DefaultProperty(nameof(RotateImage))]
    public class ImageRotate : BaseFunction, IFunction
    {
        [NonSerialized]
        private ImageDataClass _imageData = null; // 
        [NonSerialized]
        private ImageDataClass _rotateImage = null; // 

        private userPixCoordSystem _pixCoordSystem;
        public ImageRotateParam Param
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
                        if (oo != null)
                        {
                            foreach (var item in oo)
                            {
                                switch (item.GetType().Name)
                                {
                                    case nameof(userWcsCoordSystem):
                                        this._pixCoordSystem = ((userWcsCoordSystem)item).GetPixCoordSystem();
                                        return _pixCoordSystem;
                                    case nameof(userPixCoordSystem):
                                        this._pixCoordSystem = item as userPixCoordSystem;
                                        return _pixCoordSystem;
                                }
                            }
                        }
                    }
                    else
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


        [DisplayName("输出图像")]
        [DescriptionAttribute("输出属性")]
        public ImageDataClass RotateImage
        {
            get
            {
                return this._rotateImage;
            }
            set
            {
                this._rotateImage = value;
            }
        }

        public ImageRotate()
        {
            this.Param = new ImageRotateParam();
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
                this.Result.Succss = this.Param.ImageRotate(this.ImageData.Image, this.PixCoordSystem, this.Param, out hImage);
                this._rotateImage = this._imageData.Clone();
                this._rotateImage.Image = hImage.Clone();
                hImage?.Dispose();
                stopwatch.Stop();
                if (this._rotateImage != null && this._rotateImage.Image.IsInitialized())
                {
                    ((BindingList<MeasureResultInfo>)this.ResultInfo)[0].SetValue(this.name, "Width", this._rotateImage.Width);
                    ((BindingList<MeasureResultInfo>)this.ResultInfo)[1].SetValue(this.name, "Height", this._rotateImage.Height);
                    ((BindingList<MeasureResultInfo>)this.ResultInfo)[2].SetValue(this.name, "Grab_X", this._rotateImage.Grab_X);
                    ((BindingList<MeasureResultInfo>)this.ResultInfo)[3].SetValue(this.name, "Grab_Y", this._rotateImage.Grab_Y);
                    ((BindingList<MeasureResultInfo>)this.ResultInfo)[4].SetValue(this.name, "Grab_Theta", this._rotateImage.Grab_Theta);
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
                OnExcuteCompleted(this.name, this._rotateImage);
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
                case "旋转图像":
                case nameof(this.RotateImage):
                    return this._rotateImage;
                case "输入对象":
                case "输入图像":
                case nameof(this.ImageData):
                    return this.ImageData;
                default:
                    return this._rotateImage;
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

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
    [DefaultProperty(nameof(OutImageData1))]
    public class ImageDecompose : BaseFunction, IFunction
    {
        [NonSerialized]
        private ImageDataClass _imageData;
        [NonSerialized]
        private ImageDataClass _outImageData1;
        [NonSerialized]
        private ImageDataClass _outImageData2;
        [NonSerialized]
        private ImageDataClass _outImageData3;


        [DisplayName("输入图像1")]
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
                        if(oo != null && oo.Length > 0)
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

        [DisplayName("输出图像1")]
        [DescriptionAttribute("输出属性")]
        public ImageDataClass OutImageData1 { get => _outImageData1; set => _outImageData1 = value; }

        [DisplayName("输出图像2")]
        [DescriptionAttribute("输出属性")]
        public ImageDataClass OutImageData2 { get => _outImageData2; set => _outImageData2 = value; }

        [DisplayName("输出图像3")]
        [DescriptionAttribute("输出属性")]
        public ImageDataClass OutImageData3 { get => _outImageData3; set => _outImageData3 = value; }


        public DecomposeParam Param { get; set; }


        public ImageDecompose()
        {
            this.ResultInfo = new BindingList<MeasureResultInfo>();
            this.Param = new DecomposeParam();
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
                HImage hImge1 = new HImage();
                HImage hImge2 = new HImage();
                HImage hImge3 = new HImage();
                this.Result.Succss = this.Param.Decompose3(this.ImageData.Image,out hImge1, out hImge2, out hImge3);
                this._outImageData1 = this._imageData.Clone();
                this._outImageData2 = this._imageData.Clone();
                this._outImageData3 = this._imageData.Clone();
                this._outImageData1.Image = hImge1.Clone();
                this._outImageData2.Image = hImge2.Clone();
                this._outImageData3.Image = hImge3.Clone();
                hImge1?.Dispose();
                hImge2?.Dispose();
                hImge3?.Dispose();
                stopwatch.Stop();
                if (this._outImageData1 != null && this._outImageData1.Image.IsInitialized())
                {
                    ((BindingList<MeasureResultInfo>)this.ResultInfo)[0].SetValue(this.name, "Width", this._outImageData1.Width);
                    ((BindingList<MeasureResultInfo>)this.ResultInfo)[1].SetValue(this.name, "Height", this._outImageData1.Height);
                    ((BindingList<MeasureResultInfo>)this.ResultInfo)[2].SetValue(this.name, "Grab_X", this._outImageData1.Grab_X);
                    ((BindingList<MeasureResultInfo>)this.ResultInfo)[3].SetValue(this.name, "Grab_Y", this._outImageData1.Grab_Y);
                    ((BindingList<MeasureResultInfo>)this.ResultInfo)[4].SetValue(this.name, "Grab_Theta", this._outImageData1.Grab_Theta);
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
                //OnExcuteCompleted(this._outImageData1.CamName,this.name, this._outImageData1);
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
                    return this._outImageData1; //
                default:
                    return this._outImageData1;
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

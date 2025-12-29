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
    [DefaultProperty(nameof(ReduceImage))]
    public class ImageReduceROI : BaseFunction, IFunction
    {
        [NonSerialized]
        private ImageDataClass _imageData;
        [NonSerialized]
        private ImageDataClass _reduceImage;
        [NonSerialized]
        private PixROI[] _roi;

        [DisplayName("输出图像")]
        [DescriptionAttribute("输出属性")]
        public ImageDataClass ReduceImage { get => _reduceImage; set => _reduceImage = value; }

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

        [DisplayName("截剪区域")]
        [DescriptionAttribute("输入属性2")]
        public PixROI[] ROI
        {
            get
            {
                try
                {
                    if (this.RefSource2.Count > 0)
                    {
                        object[] oo = this.GetPropertyValue(this.RefSource2);
                        List<PixROI> listROI = new List<PixROI>();
                        if (oo != null && oo.Length > 0)
                        {
                            foreach (var item in oo)
                            {
                                switch (item.GetType().Name)
                                {
                                    case nameof(userPixCircle):
                                        userPixCircle pixCircle = item as userPixCircle;
                                        listROI.Add(new drawPixCircle(pixCircle.Row, pixCircle.Col, pixCircle.Radius));
                                        break;
                                    case nameof(userWcsCircle):
                                        pixCircle = (item as userWcsCircle).GetPixCircle();
                                        listROI.Add(new drawPixCircle(pixCircle.Row, pixCircle.Col, pixCircle.Radius));
                                        break;
                                    case nameof(userPixEllipse):
                                        userPixEllipse pixEllipse = item as userPixEllipse;
                                        listROI.Add(new drawPixEllipse(pixEllipse.Row, pixEllipse.Col, pixEllipse.Rad, pixEllipse.Radius1, pixEllipse.Radius2));
                                        break;
                                    case nameof(userWcsEllipse):
                                        pixEllipse = (item as userWcsEllipse).GetPixEllipse();
                                        listROI.Add(new drawPixEllipse(pixEllipse.Row, pixEllipse.Col, pixEllipse.Rad, pixEllipse.Radius1, pixEllipse.Radius2));
                                        break;
                                    case nameof(userPixRectangle2):
                                        userPixRectangle2 pixRec2 = item as userPixRectangle2;
                                        listROI.Add(new drawPixRect2(pixRec2.Row, pixRec2.Col, pixRec2.Rad, pixRec2.Length1, pixRec2.Length2));
                                        break;
                                    case nameof(userWcsRectangle2):
                                        pixRec2 = (item as userWcsRectangle2).GetPixRectangle2();
                                        listROI.Add(new drawPixRect2(pixRec2.Row, pixRec2.Col, pixRec2.Rad, pixRec2.Length1, pixRec2.Length2));
                                        break;
                                    case nameof(userPixRectangle1):
                                        userPixRectangle1 pixRec1 = item as userPixRectangle1;
                                        listROI.Add(new drawPixRect1(pixRec1.Row1, pixRec1.Col1, pixRec1.Row2, pixRec1.Col2));
                                        break;
                                    case nameof(userWcsRectangle1):
                                        pixRec1 = (item as userWcsRectangle1).GetPixRectangle1();
                                        listROI.Add(new drawPixRect1(pixRec1.Row1, pixRec1.Col1, pixRec1.Row2, pixRec1.Col2));
                                        break;
                                    case nameof(userPixPolyLine):
                                        userPixPolyLine pixPolyLine  = item as userPixPolyLine;
                                        listROI.Add(new drawPixPolyLine(pixPolyLine.Row.ToArray(), pixPolyLine.Col.ToArray()));
                                        break;
                                    case nameof(userWcsPolyLine):
                                        pixPolyLine = (item as userWcsPolyLine).GetPixPolyLine();
                                        listROI.Add(new drawPixPolyLine(pixPolyLine.Row.ToArray(), pixPolyLine.Col.ToArray()));
                                        break;
                                }
                            }
                        }
                        this._roi = listROI.ToArray();
                    }
                    if (this._roi == null)
                        this._roi = new PixROI[0];
                    return _roi;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            set
            {
                this._roi = value;
            }
        }

        public ImageReduceMethod ReduceMethod { get; set; }

        public ImageReduceROI()
        {
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
                userPixCoordSystem pixCoordSystem = null;
                if (param != null)
                {
                    foreach (var item in param)
                    {
                        switch (item.GetType().Name)
                        {
                            case nameof(userWcsCoordSystem):
                                pixCoordSystem = ((userWcsCoordSystem)item).GetPixCoordSystem();
                                break;
                            case nameof(userPixCoordSystem):
                                pixCoordSystem = ((userPixCoordSystem)item);
                                break;
                        }
                    }
                }
                HImage hImage;
                Result.Succss =  ImageReduceMethod.ReduceImageDomain(this.ImageData.Image, this.ROI, out hImage);
                this._reduceImage = new ImageDataClass(hImage, this._imageData.CamParams);
                this._reduceImage.CamName = this._imageData.CamName;
                this._reduceImage.ViewWindow = this._imageData.ViewWindow;
                this._reduceImage.Grab_X = this._imageData.Grab_X;
                this._reduceImage.Grab_Y = this._imageData.Grab_Y;
                this._reduceImage.Grab_Theta = this._imageData.Grab_Theta;
                this._reduceImage.Tag = this._imageData.Tag;
                stopwatch.Stop();
                if (this._reduceImage != null && this._reduceImage.Image.IsInitialized())
                {
                    ((BindingList<MeasureResultInfo>)this.ResultInfo)[0].SetValue(this.name, "Width", this._reduceImage.Width);
                    ((BindingList<MeasureResultInfo>)this.ResultInfo)[1].SetValue(this.name, "Height", this._reduceImage.Height);
                    ((BindingList<MeasureResultInfo>)this.ResultInfo)[2].SetValue(this.name, "Grab_X", this._reduceImage.Grab_X);
                    ((BindingList<MeasureResultInfo>)this.ResultInfo)[3].SetValue(this.name, "Grab_Y", this._reduceImage.Grab_Y);
                    ((BindingList<MeasureResultInfo>)this.ResultInfo)[4].SetValue(this.name, "Grab_Theta", this._reduceImage.Grab_Theta);
                    if (((BindingList<MeasureResultInfo>)this.ResultInfo).Count > 5)
                        ((BindingList<MeasureResultInfo>)this.ResultInfo)[5].SetValue(this.name, "Time(ms)", stopwatch.ElapsedMilliseconds);
                    else
                        ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name));
                }
                else
                {
                    ((BindingList<MeasureResultInfo>)this.ResultInfo)[0].SetValue(this.name, "Width", 0);
                    ((BindingList<MeasureResultInfo>)this.ResultInfo)[1].SetValue(this.name, "Height", 0);
                    ((BindingList<MeasureResultInfo>)this.ResultInfo)[2].SetValue(this.name, "Grab_X", 0);
                    ((BindingList<MeasureResultInfo>)this.ResultInfo)[3].SetValue(this.name, "Grab_Y", 0);
                    ((BindingList<MeasureResultInfo>)this.ResultInfo)[4].SetValue(this.name, "Grab_Theta", 0);
                    if (((BindingList<MeasureResultInfo>)this.ResultInfo).Count > 5)
                        ((BindingList<MeasureResultInfo>)this.ResultInfo)[5].SetValue(this.name, "Time(ms)", stopwatch.ElapsedMilliseconds);
                    else
                        ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name));
                }
                OnExcuteCompleted(this.name, this._reduceImage);   // this._reduceImage.CamName, this._reduceImage.ViewWindow,
            }
            catch (Exception ex)
            {
                this.Result.Succss = false;
                LoggerHelper.Error(this.name + "->执行错误：", ex);
            }
            if (this.Result.Succss)
                LoggerHelper.Info(this.name + "->执行成功：" );
            else
                LoggerHelper.Error(this.name + "->执行失败：" );
            // 更改UI字体　
            UpdataNodeElementStyle(param, this.Result.Succss);
            return this.Result;
        }

        public object GetPropertyValues(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(this.Name):
                case "名称":
                    return this.name;
                case "图像对象":
                case "输出对象":
                    return this._reduceImage; //
                default:
                    if (this.name == propertyName)
                        return this._reduceImage;
                    else return null;
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

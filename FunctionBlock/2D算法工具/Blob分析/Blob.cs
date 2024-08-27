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
    [DefaultProperty(nameof(BlobRegion))]
    public class Blob : BaseFunction, IFunction
    {
        [NonSerialized]
        private ImageDataClass _imageData;
        [NonSerialized]
        private ImageDataClass _binaryImage;
        [NonSerialized]
        private ImageDataClass _binaryMeanImage;
        [NonSerialized]
        private ImageDataClass _regionImage;

        [NonSerialized]
        private userPixCoordSystem _pixCoordSystem;
        [NonSerialized]
        private RegionDataClass threshouldRegion;
        public DoBlobAnalyse BlobParam { get; set; }


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
                        if (this._pixCoordSystem == null)
                            this._pixCoordSystem = new userPixCoordSystem();
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

        [DisplayName("输出区域")]
        [DescriptionAttribute("输出属性")]
        public RegionDataClass BlobRegion { get => threshouldRegion; set => threshouldRegion = value; }

        [DisplayName("二值图像")]
        [DescriptionAttribute("输出属性")]
        public ImageDataClass BinaryImage { get => _binaryImage; set => _binaryImage = value; }

        [DisplayName("均值图像")]
        [DescriptionAttribute("输出属性")]
        public ImageDataClass BinaryMeanImage { get => _binaryMeanImage; set => _binaryMeanImage = value; }

        [DisplayName("区域图像")]
        [DescriptionAttribute("输出属性")]
        public ImageDataClass RegionImage { get => _regionImage; set => _regionImage = value; }
        public Blob()
        {
            this.BlobParam = new DoBlobAnalyse();
            this.ResultInfo = new BindingList<MeasureResultInfo>();
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name, "斑块数量", 0));
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name, "斑块面积", 0));
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name, "TIme(ms)", 0));
        }



        #region 实现接口
        public OperateResult Execute(params object[] param)
        {
            this.Result.Succss = false;
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            try
            {
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
                HRegion hRegion = new HRegion();
                List<FlawMsg> FlawMsg = new List<FlawMsg>();
                /// 先变换分割区域
                if (pixCoordSystem == null)
                    BlobParam.SegParam.PixCoordSys = this.PixCoordSystem;
                else
                    BlobParam.SegParam.PixCoordSys = pixCoordSystem;
                Result.Succss = BlobParam.Do(this.ImageData?.Image, out hRegion);

                if (hRegion != null && hRegion.IsInitialized() && hRegion.Area > 0 && hRegion.Area.Length == 1)
                {
                    this._regionImage = new ImageDataClass(this._imageData?.Image.ReduceDomain(hRegion.Union1()), this._imageData.CamParams, this._imageData.Grab_X, this._imageData.Grab_Y, this._imageData.Grab_Z, 0);
                    this._binaryMeanImage = new ImageDataClass(hRegion.RegionToMean(this._imageData?.Image), this._imageData.CamParams, this._imageData.Grab_X, this._imageData.Grab_Y, this._imageData.Grab_Z, 0);
                    this._binaryImage = new ImageDataClass(hRegion.RegionToBin(255, 0, this._imageData.Width, this._imageData.Height), this._imageData.CamParams, this._imageData.Grab_X, this._imageData.Grab_Y, this._imageData.Grab_Z, 0);
                    this._binaryMeanImage.CamName = this._imageData.CamName;
                    this._binaryMeanImage.ViewWindow = this._imageData.ViewWindow;
                    this._binaryImage.CamName = this._imageData.CamName;
                    this._binaryImage.ViewWindow = this._imageData.ViewWindow;
                }
                else
                {
                    this._regionImage = new ImageDataClass();
                    this._binaryMeanImage = new ImageDataClass();
                    this._binaryImage = new ImageDataClass();
                }
                this.threshouldRegion = new RegionDataClass(hRegion, this._imageData.Grab_X, this._imageData.Grab_Y, this._imageData.Grab_Z, this._imageData.CamParams);
                this.threshouldRegion.Draw = BlobParam.OutParam.DrawMode;
                this.threshouldRegion.CamName = this._imageData.CamName;
                this.threshouldRegion.ViewWindow = this._imageData.ViewWindow;
                this.threshouldRegion.Tag = this._imageData.Tag;
                stopwatch.Stop();
                if (hRegion.IsInitialized())
                {
                    ((BindingList<MeasureResultInfo>)this.ResultInfo)[0].SetValue(this.name, "斑块数量", hRegion.CountObj());
                    ((BindingList<MeasureResultInfo>)this.ResultInfo)[1].SetValue(this.name, "斑块面积", hRegion.Union1().Area.D);
                    ((BindingList<MeasureResultInfo>)this.ResultInfo)[2].SetValue(this.name, "Time(ms)", stopwatch.ElapsedMilliseconds);
                }
                /////////////////////////////////////////////////////////////////
                if (BlobParam.OutParam == null)
                    BlobParam.OutParam = new OutputParam();
                if (BlobParam.OutParam.IsOutputRegion)
                    OnExcuteCompleted(this.ImageData.CamName, this.threshouldRegion.ViewWindow, this.name, this.threshouldRegion); // 输出区域
                if (BlobParam.OutParam.IsOutputBinaryImage)
                    OnExcuteCompleted(this.ImageData.CamName, this.threshouldRegion.ViewWindow, this.name, this._binaryImage);
                if (BlobParam.OutParam.IsOutputBinaryMeanImage)
                    OnExcuteCompleted(this.ImageData.CamName, this.threshouldRegion.ViewWindow, this.name, this._binaryMeanImage);
                ///////////////////////////////////////////////////////
                ////OnExcuteCompleted(this.ImageData.CamName, this.threshouldRegion.ViewWindow, this.name, FlawMsg);
            }
            catch (Exception ex)
            {
                LoggerHelper.Error(this.name + "->执行错误", ex);
                //return Result;
            }
            if (Result.Succss)
                LoggerHelper.Info(this.name + "->执行成功");
            else
                LoggerHelper.Error(this.name + "->执行失败");
            // 更改UI字体　
            UpdataNodeElementStyle(param, Result.Succss);
            return Result;
        }
        public object GetPropertyValues(string propertyName)
        {
            switch (propertyName)
            {
                case "名称":
                    return this.name;
                case nameof(BlobRegion):
                    return this.threshouldRegion; //
                default:
                    if (this.name == propertyName)
                        return this.threshouldRegion;
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

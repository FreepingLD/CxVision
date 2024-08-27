using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Basler.Pylon;
using Common;
using HalconDotNet;


namespace Sensor
{
    class BaslerCam : SensorBase, ISensor
    {
        //private HImage _grabImage;
        //private HImage _grabDarkImage;
        private Basler.Pylon.ICamera camera;
        private Basler.Pylon.IStreamGrabber StreamGrabber;
        /// if >= Sfnc2_0_0,说明是ＵＳＢ３的相机
        private Version Sfnc2_0_0 = new Version(2, 0, 0);
        private bool IsSoftwareFinish = false;
        public bool Connect(SensorConnectConfigParam configParam) // 传入的名称
        {
            bool result = false;
            try
            {
                this.ConfigParam = configParam;
                this.Name = configParam.SensorName;
                this.CameraParam = (CameraParam)new CameraParam().Read(configParam.SensorName);
                this.CameraParam.SensorName = configParam.SensorName;
                ///////////////////////////////////
                switch (configParam.ConnectType)
                {
                    /// 海康面阵相机接口
                    case enUserConnectType.SerialNumber:
                        //List<ICameraInfo> deviceInfos = CameraFinder.Enumerate(DeviceType.GigE);// 枚举网络中的所有相机
                        this.camera = new Basler.Pylon.Camera(configParam.ConnectAddress).Open();
                        this.camera.Parameters[PLTransportLayer.HeartbeatTimeout].SetValue(20000); // 设置心跳时间，可以提早释放占用的资源 , 心跳时间不宜设置过短  
                        this.StreamGrabber = this.camera.StreamGrabber;
                        switch (this.CameraParam.TriggerSource)
                        {
                            case enUserTriggerSource.NONE:
                                // 设置触发模式
                                this.camera.Parameters[Basler.Pylon.PLCamera.TriggerMode].SetValue(Basler.Pylon.PLCamera.TriggerMode.Off);
                                result = true;
                                break;
                            case enUserTriggerSource.软触发:
                                SetSoftwareTrigger();
                                //camera.StreamGrabber.ImageGrabbed += OnImageGrabbed;
                                result = true;
                                break;
                            default:
                                SetExternTrigger();
                                camera.StreamGrabber.ImageGrabbed += OnImageGrabbed;
                                result = true;
                                break;
                        }
                        break;
                    case enUserConnectType.DeviceName:
                        List<ICameraInfo> deviceInfos = CameraFinder.Enumerate(DeviceType.GigE);// 枚举网络中的所有相机
                        foreach (var item in deviceInfos)
                        {
                            if (item[CameraInfoKey.UserDefinedName] == this.Name)
                            {
                                this.camera = new Basler.Pylon.Camera(item).Open();
                                this.camera.Parameters[PLTransportLayer.HeartbeatTimeout].SetValue(20000); // 设置心跳时间，可以提早释放占用的资源 , 心跳时间不宜设置过短  
                                this.StreamGrabber = this.camera.StreamGrabber;
                                switch (this.CameraParam.TriggerSource)
                                {
                                    case enUserTriggerSource.NONE:
                                        // 设置触发模式
                                        this.camera.Parameters[Basler.Pylon.PLCamera.TriggerMode].SetValue(Basler.Pylon.PLCamera.TriggerMode.Off);
                                        result = true;
                                        break;
                                    case enUserTriggerSource.软触发:
                                        SetSoftwareTrigger();
                                        //camera.StreamGrabber.ImageGrabbed += OnImageGrabbed;
                                        result = true;
                                        break;
                                    default:
                                        SetExternTrigger();
                                        camera.StreamGrabber.ImageGrabbed += OnImageGrabbed;
                                        result = true;
                                        break;
                                }
                                break;
                            }
                        }
                        break;
                    case enUserConnectType.Map:
                        this._MapName = configParam.ConnectAddress;
                        if (SensorManage.GetSensor(configParam.ConnectAddress) != null &&
                            SensorManage.GetSensor(configParam.ConnectAddress).ConfigParam.ConnectState)  // 只有在映射源找开成功的情总下，映射对象才能打开成功
                            result = true;
                        else
                            result = false;
                        break;
                    /// halcon接口适用于所有支持 Gige 接口的相机 
                    default:
                        MessageBox.Show(configParam.ConnectType.ToString() + "连接类型未实现，请将连接类型设置为：" + enUserConnectType.SerialNumber + "并指定相机序列号!");
                        break;
                }
            }
            catch (HalconException ex)
            {
                LoggerHelper.Error(this.CameraParam.SensorName + "打开相机失败", ex);
                result = false;
            }
            configParam.ConnectState = result;  // 连接状态
            return result;
        }

        public bool Disconnect()
        {
            bool result = false;
            try
            {
                if (this.camera != null)
                {
                    camera.StreamGrabber.ImageGrabbed -= OnImageGrabbed;
                    this.camera.Close();
                }
                result = true;
                LoggerHelper.Info(this.CameraParam.SensorName + "关闭相机成功");
            }
            catch (Exception ex)
            {
                LoggerHelper.Error(this.CameraParam.SensorName + "关闭相机失败", ex);
                result = false;
            }
            return result;
        }

        public bool Init()
        {
            try
            {
                return true;
            }
            catch
            {
                return false;
            }

        }

        public Dictionary<enDataItem, object> ReadData()
        {
            Dictionary<enDataItem, object> list = new Dictionary<enDataItem, object>();
            list.Add(enDataItem.Image, new ImageDataClass(this._grabImage.Clone(), this.CameraParam)); //this.imageData
            return list;
        }
        public object GetParam(object paramType)
        {
            object value = "";
            switch (paramType.ToString())
            {
                case "曝光":
                    if (camera == null) return value;
                    if (camera.GetSfncVersion() < Sfnc2_0_0)
                    {
                        value = camera.Parameters[PLCamera.ExposureTimeRaw].GetValue();
                    }
                    else
                    {
                        value = (long)camera.Parameters[PLCamera.ExposureTime].GetValue();
                    }
                    break;
                case "增益":
                    if (camera == null) return value;
                    if (camera.GetSfncVersion() < Sfnc2_0_0)
                    {
                        value = camera.Parameters[PLCamera.GainRaw].GetValue();
                    }
                    else
                    {
                        value = (long)camera.Parameters[PLCamera.Gain].GetValue();
                    }
                    break;
            }
            return value;
        }
        public bool SetParam(object paramType, object value)
        {
            bool result = false;
            try
            {
                switch (paramType.ToString())
                {
                    case "曝光":
                        if (camera == null) return result;
                        // Some camera models may have auto functions enabled. To set the ExposureTime value to a specific value,
                        // the ExposureAuto function must be disabled first (if ExposureAuto is available).
                        camera.Parameters[PLCamera.ExposureAuto].TrySetValue(PLCamera.ExposureAuto.Off); // Set ExposureAuto to Off if it is writable.
                        camera.Parameters[PLCamera.ExposureMode].TrySetValue(PLCamera.ExposureMode.Timed); // Set ExposureMode to Timed if it is writable.
                        long value1 = 0;
                        if (!long.TryParse(value.ToString(), out value1)) return false;
                        if (camera.GetSfncVersion() < Sfnc2_0_0)
                        {
                            // In previous SFNC versions, ExposureTimeRaw is an integer parameter,单位us
                            // integer parameter的数据，设置之前，需要进行有效值整合，否则可能会报错
                            long min = camera.Parameters[PLCamera.ExposureTimeRaw].GetMinimum();
                            long max = camera.Parameters[PLCamera.ExposureTimeRaw].GetMaximum();
                            long incr = camera.Parameters[PLCamera.ExposureTimeRaw].GetIncrement();
                            if (value1 < min)
                            {
                                value1 = min;
                            }
                            else if (value1 > max)
                            {
                                value1 = max;
                            }
                            else
                            {
                                value1 = min + (((value1 - min) / incr) * incr);
                            }
                            camera.Parameters[PLCamera.ExposureTimeRaw].SetValue(value1);
                        }
                        else // For SFNC 2.0 cameras, e.g. USB3 Vision cameras
                        {
                            // In SFNC 2.0, ExposureTimeRaw is renamed as ExposureTime,is a float parameter, 单位us.
                            camera.Parameters[PLUsbCamera.ExposureTime].SetValue((double)value1);
                        }
                        break;
                    case "增益":
                        if (camera == null) return result;
                        camera.Parameters[PLCamera.GainAuto].TrySetValue(PLCamera.GainAuto.Off); // Set GainAuto to Off if it is writable.
                        value1 = 0;
                        if (!long.TryParse(value.ToString(), out value1)) return result;
                        if (camera.GetSfncVersion() < Sfnc2_0_0)
                        {
                            // Some parameters have restrictions. You can use GetIncrement/GetMinimum/GetMaximum to make sure you set a valid value.                              
                            // In previous SFNC versions, GainRaw is an integer parameter.
                            // integer parameter的数据，设置之前，需要进行有效值整合，否则可能会报错
                            long min = camera.Parameters[PLCamera.GainRaw].GetMinimum();
                            long max = camera.Parameters[PLCamera.GainRaw].GetMaximum();
                            long incr = camera.Parameters[PLCamera.GainRaw].GetIncrement();

                            if (value1 < min)
                            {
                                value = min;
                            }
                            else if (value1 > max)
                            {
                                value = max;
                            }
                            else
                            {
                                value = min + (((value1 - min) / incr) * incr);
                            }
                            camera.Parameters[PLCamera.GainRaw].SetValue(value1);
                        }
                        else // For SFNC 2.0 cameras, e.g. USB3 Vision cameras
                        {
                            // In SFNC 2.0, Gain is a float parameter.
                            camera.Parameters[PLUsbCamera.Gain].SetValue(value1);
                        }
                        break;
                }
            }
            catch (Exception e)
            {
                result = false;
            }
            return result;
        }

        public bool StartTrigger()
        {
            bool result = false;
            //lock (this._lockState)   //锁加在这里比较好
            //{
            switch (this.ConfigParam.ConnectType)
            {
                case enUserConnectType.Map:
                    SensorManage.GetSensor(this._MapName).StartTrigger();
                    if (this._grabImage != null && this._grabImage.IsInitialized())
                        this._grabImage.Dispose();
                    if (this._grabDarkImage != null && this._grabDarkImage.IsInitialized())
                        this._grabDarkImage.Dispose();
                    this._grabImage = (SensorManage.GetSensor(this._MapName)).GrabImage; // 用于远程调用的映射处理
                    this._grabDarkImage = (SensorManage.GetSensor(this._MapName)).GrabDarkImage; // 用于远程调用的映射处理
                    break;
                default:
                    switch (this.CameraParam.TriggerSource)
                    {
                        case enUserTriggerSource.NONE: // 实时采集
                            if (this._grabImage != null && this._grabImage.IsInitialized())
                                this._grabImage.Dispose();
                            if (this.camera.StreamGrabber.IsGrabbing) return false;
                            result = this.GetImageSyn(out this._grabImage, out _grabDarkImage);
                            if (result)
                                LoggerHelper.Info(this.CameraParam.SensorName + "图像采集成功");
                            else
                                LoggerHelper.Info(this.CameraParam.SensorName + "图像采集失败");
                            break;
                        case enUserTriggerSource.外部IO触发:
                        case enUserTriggerSource.内部IO触发:
                        case enUserTriggerSource.编码器触发:
                            break;
                        case enUserTriggerSource.软触发:
                            this.SendSoftwareExecute();// 软触发
                            result = this.GetImageSyn(out this._grabImage, out _grabDarkImage);
                            this.IsSoftwareFinish = true;
                            break;
                    }
                    break;
            }
            //}
            return result;
        }

        public bool StopTrigger()
        {
            bool result = false;
            try
            {
                switch (this.ConfigParam.ConnectType)
                {
                    case enUserConnectType.Map:
                        SensorManage.GetSensor(this._MapName).StopTrigger();
                        //if (this._grabImage != null && this._grabImage.IsInitialized())
                        //    this._grabImage.Dispose();
                        //this._grabImage = (SensorManage.GetSensor(this._MapName)).GrabImage; // 用于远程调用的映射处理
                        break;
                    default:
                        switch (this.CameraParam.TriggerSource)
                        {
                            case enUserTriggerSource.NONE: // 实时采集
                                result = true;
                                break;
                            case enUserTriggerSource.外部IO触发:
                            case enUserTriggerSource.内部IO触发:
                            case enUserTriggerSource.编码器触发:
                                result = true;
                                break;
                            case enUserTriggerSource.软触发:
                                Stopwatch stopwatch = new Stopwatch();
                                stopwatch.Start();
                                result = true;
                                while (!this.IsSoftwareFinish)
                                {
                                    if (stopwatch.ElapsedMilliseconds > 5000)
                                    {
                                        result = false;
                                        break; // 等待5秒，没采集到图像，跳出
                                    }
                                    Thread.Sleep(100);
                                }
                                stopwatch.Stop();
                                break;
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                result = false;
                LoggerHelper.Error(this.Name + ":触发相机失败" + ex);
            }
            return result;
        }

        private void SetExternTrigger()
        {
            try
            {
                if (camera.GetSfncVersion() < Sfnc2_0_0)
                {
                    camera.Parameters[Basler.Pylon.PLCamera.TriggerSelector].SetValue(Basler.Pylon.PLCamera.TriggerSelector.AcquisitionStart);
                    camera.Parameters[Basler.Pylon.PLCamera.TriggerMode].SetValue(Basler.Pylon.PLCamera.TriggerMode.Off);
                    camera.Parameters[Basler.Pylon.PLCamera.TriggerSelector].SetValue(Basler.Pylon.PLCamera.TriggerSelector.FrameStart);
                    camera.Parameters[Basler.Pylon.PLCamera.TriggerMode].SetValue(Basler.Pylon.PLCamera.TriggerMode.On);
                    camera.Parameters[Basler.Pylon.PLCamera.TriggerSource].SetValue(Basler.Pylon.PLCamera.TriggerSource.Line1);
                }
                else
                {
                    camera.Parameters[Basler.Pylon.PLCamera.TriggerSelector].SetValue(Basler.Pylon.PLCamera.TriggerSelector.FrameBurstStart);
                    camera.Parameters[Basler.Pylon.PLCamera.TriggerMode].SetValue(Basler.Pylon.PLCamera.TriggerMode.Off);
                    camera.Parameters[Basler.Pylon.PLCamera.TriggerSelector].SetValue(Basler.Pylon.PLCamera.TriggerSelector.FrameStart);
                    camera.Parameters[Basler.Pylon.PLCamera.TriggerMode].SetValue(Basler.Pylon.PLCamera.TriggerMode.On);
                    camera.Parameters[Basler.Pylon.PLCamera.TriggerSource].SetValue(Basler.Pylon.PLCamera.TriggerSource.Line1);

                }
            }
            catch (Exception e)
            {
                LoggerHelper.Error(e);
            }
        }

        private void SetSoftwareTrigger()
        {
            try
            {
                if (camera.GetSfncVersion() < Sfnc2_0_0)
                {
                    camera.Parameters[Basler.Pylon.PLCamera.TriggerSelector].SetValue(Basler.Pylon.PLCamera.TriggerSelector.AcquisitionStart);
                    camera.Parameters[Basler.Pylon.PLCamera.TriggerMode].SetValue(Basler.Pylon.PLCamera.TriggerMode.Off);
                    camera.Parameters[Basler.Pylon.PLCamera.TriggerSelector].SetValue(Basler.Pylon.PLCamera.TriggerSelector.FrameStart);
                    camera.Parameters[Basler.Pylon.PLCamera.TriggerMode].SetValue(Basler.Pylon.PLCamera.TriggerMode.On);
                    camera.Parameters[Basler.Pylon.PLCamera.TriggerSource].SetValue(Basler.Pylon.PLCamera.TriggerSource.Software);
                }
                else // For SFNC 2.0 cameras, e.g. USB3 Vision cameras
                {
                    camera.Parameters[Basler.Pylon.PLCamera.TriggerSelector].SetValue(Basler.Pylon.PLCamera.TriggerSelector.FrameBurstStart);
                    camera.Parameters[Basler.Pylon.PLCamera.TriggerMode].SetValue(Basler.Pylon.PLCamera.TriggerMode.Off);
                    camera.Parameters[Basler.Pylon.PLCamera.TriggerSelector].SetValue(Basler.Pylon.PLCamera.TriggerSelector.FrameStart);
                    camera.Parameters[Basler.Pylon.PLCamera.TriggerMode].SetValue(Basler.Pylon.PLCamera.TriggerMode.On);
                    camera.Parameters[Basler.Pylon.PLCamera.TriggerSource].SetValue(Basler.Pylon.PLCamera.TriggerSource.Software);
                }
            }
            catch (Exception e)
            {
                LoggerHelper.Error(e);
            }
        }

        private void SendSoftwareExecute()
        {
            try
            {
                if (camera.WaitForFrameTriggerReady(1000, Basler.Pylon.TimeoutHandling.ThrowException))
                {
                    camera.ExecuteSoftwareTrigger();
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.Error("软触发失败" + ex);
            }
        }
        private void OnImageGrabbed(Object sender, ImageGrabbedEventArgs e)
        {
            if (e.GrabResult.GrabSucceeded)
            {
                HImage image = new HImage();
                image.GenImage1("byte", e.GrabResult.Width, e.GrabResult.Height, e.GrabResult.PixelDataPointer);
                this.AdjImg(image, out this._grabImage);  // 调整图像的方向及图像畸变
                this.CameraParam.DataWidth = e.GrabResult.Width;
                this.CameraParam.DataHeight = e.GrabResult.Height;
                image?.Dispose();
                this.IsSoftwareFinish = true;
                //OnImageAcqComplete(this.CameraParam.SensorName, new ImageDataClass(this._grabImage, this.CameraParam));
            }
        }

        protected override bool GetImageSyn(out HImage hImage, out HImage darkImage)
        {
            hImage = new HImage();
            darkImage = new HImage();
            bool result = true;
            this.camera.Parameters[PLCamera.AcquisitionMode].SetValue("SingleFrame");
            List<Basler.Pylon.IGrabResult> imageListInfo = new List<IGrabResult>();
            for (int i = 0; i < this.CameraParam.AverangeCount; i++)
            {
                imageListInfo.Add(this.StreamGrabber.GrabOne(this.CameraParam.Timeout, Basler.Pylon.TimeoutHandling.ThrowException));
            }
            List<HImage> imageList = new List<HImage>();
            foreach (var item in imageListInfo)
            {
                if (item.GrabSucceeded)
                {
                    HImage image = new HImage();
                    HImage adjImage = new HImage();
                    image.GenImage1("byte", item.Width, item.Height, item.PixelDataPointer);
                    this.AdjImg(image, out adjImage);  // 调整图像的方向及图像畸变
                    image?.Dispose();
                    this.CameraParam.DataWidth = item.Width;
                    this.CameraParam.DataHeight = item.Height;
                    imageList.Add(adjImage);
                }
            }
            if (imageList.Count > 0)
            {
                if (this.ConfigParam.IsAutoFocus)
                    this.AutoFocus(imageList.ToArray(), out hImage);
                else
                    hImage = AveImage(imageList);
                result = true;
            }
            else
            {
                result = false;
            }
            foreach (var item in imageListInfo)
            {
                item.Dispose();
            }
            foreach (var item in imageList)
            {
                item.Dispose();
            }
            imageListInfo.Clear();
            imageList.Clear();
            return result;
        }



    }
}

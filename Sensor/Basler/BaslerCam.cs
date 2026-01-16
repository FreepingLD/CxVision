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
    public class BaslerCam : SensorBase, ISensor
    {
        private Basler.Pylon.ICamera camera = null;
        private Basler.Pylon.IStreamGrabber StreamGrabber;
        /// if >= Sfnc2_0_0,说明是ＵＳＢ３的相机
        private Version Sfnc2_0_0 = new Version(2, 0, 0);
        private bool IsSoftwareFinish = false;
        private SocketBase _socket;
        private int ImageIndex = 0;
        private int imageWidth = 0;
        private int imageHeight = 0;

        public bool Connect(SensorConnectConfigParam configParam) // 传入的名称
        {
            bool result = false;
            try
            {
                this.ConfigParam = configParam;
                this.Name = configParam.SensorName;
                this.CameraParam = (CameraParam)new CameraParam().Read(configParam.SensorName);
                this.CameraParam.SensorName = configParam.SensorName;
                this._MapImage = this.CameraParam.Map?.Clone();
                this.IsLiveState = false;
                if (!configParam.IsActive) return false;
                this.cts?.Cancel();
                ///////////////////////////////////
                switch (configParam.ConnectType)
                {
                    /// 海康面阵相机接口
                    case enUserConnectType.SerialNumber:
                        List<ICameraInfo> deviceInfos2 = CameraFinder.Enumerate(DeviceType.GigE);// 枚举网络中的所有相机
                        this.camera = new Basler.Pylon.Camera(configParam.ConnectAddress).Open();
                        this.camera.Parameters[PLTransportLayer.HeartbeatTimeout].SetValue(20000); // 设置心跳时间，可以提早释放占用的资源 , 心跳时间不宜设置过短  
                        this.StreamGrabber = this.camera.StreamGrabber;
                        this.imageWidth = (int)this.camera.Parameters[Basler.Pylon.PLCamera.SensorWidth].GetValue();
                        this.imageHeight = (int)this.camera.Parameters[Basler.Pylon.PLCamera.SensorHeight].GetValue();
                        this._imageManage.FramWidth = (int)this.imageWidth;
                        this._imageManage.FramHeight = (int)this.imageHeight;
                        // 设置采集方式
                        result = this.SetAcqParam();
                        break;
                    case enUserConnectType.DeviceName:
                        List<ICameraInfo> deviceInfos = CameraFinder.Enumerate(DeviceType.GigE);// 枚举网络中的所有相机
                        foreach (var item in deviceInfos)
                        {
                            if (item[CameraInfoKey.UserDefinedName] == this.ConfigParam.ConnectAddress)
                            {
                                this.camera = new Basler.Pylon.Camera(item).Open();
                                this.camera.Parameters[PLTransportLayer.HeartbeatTimeout].SetValue(20000); // 设置心跳时间，可以提早释放占用的资源 , 心跳时间不宜设置过短  
                                this.StreamGrabber = this.camera.StreamGrabber;
                                this.imageWidth = (int)this.camera.Parameters[Basler.Pylon.PLCamera.SensorWidth].GetValue();
                                this.imageHeight = (int)this.camera.Parameters[Basler.Pylon.PLCamera.SensorHeight].GetValue();
                                this._imageManage.FramWidth = (int)this.imageWidth;
                                this._imageManage.FramHeight = (int)this.imageHeight;
                                // 设置采集方式
                                result = this.SetAcqParam();
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
                    case enUserConnectType.Socket:
                        this._socket = SocketConnectManager.Instance.GetSocket(this.ConfigParam.ConnectAddress);
                        if (this._socket != null)
                            result = true;
                        break;
                    case enUserConnectType.NONE:       // 如果为NONE，那就默认OK 
                        result = true;
                        break;
                    /// halcon接口适用于所有支持 Gige 接口的相机 
                    default:
                        new UserMessageForm().ShowDialog(configParam.ConnectType.ToString() + "连接类型未实现，请将连接类型设置为：" + enUserConnectType.SerialNumber + "并指定相机序列号!");
                        break;
                }
            }
            catch (Exception ex)
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
                this.cts?.Cancel(); // 取消异步采图线程
                if (this.camera != null)
                {
                    this.camera.StreamGrabber.Stop();
                    this.camera.StreamGrabber.ImageGrabbed -= OnImageGrabbed;
                    this.camera.Close();
                }
                result = true;
                LoggerHelper.Info(this.CameraParam?.SensorName + "关闭相机成功");
            }
            catch (Exception ex)
            {
                LoggerHelper.Error(this.CameraParam?.SensorName + "关闭相机失败", ex);
                result = false;
            }
            return result;
        }

        public bool Init()
        {
            try
            {
                this._imageManage = new ImageManage(200);
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
            switch (this.CameraParam.AcqMode)
            {
                case enAcqMode.异步采集:
                    if (this.IsLiveState) // 如果在实时采集状态，那么将执行
                    {
                        this.GetImageSyn(out this._grabImage, out _grabDarkImage);
                        if (this._grabImage != null && this._grabImage.IsInitialized())
                            list.Add(enDataItem.Image, new ImageDataClass(this._grabImage.Clone(), this.CameraParam));
                    }
                    break;
                case enAcqMode.异步取图:
                    switch (this.ConfigParam.ConnectType)
                    {
                        case enUserConnectType.Socket:
                            if (this._imageData != null && this._imageData.IsInitialized())
                                list.Add(enDataItem.Image, this._imageData.Clone());
                            break;
                        default:
                            if (this.IsLiveState) // 如果在实时采集状态，那么将执行
                            {
                                this.GetImageSyn(out this._grabImage, out _grabDarkImage);
                                if (this._grabImage != null && this._grabImage.IsInitialized())
                                    list.Add(enDataItem.Image, new ImageDataClass(this._grabImage.Clone(), this.CameraParam));
                            }
                            else
                            {
                                this.GetImageAsyn(out this._grabImage, out _grabDarkImage); // 异步取图也是从绶存中取
                                if (this._grabImage != null && this._grabImage.IsInitialized())
                                {
                                    list.Add(enDataItem.Image, new ImageDataClass(this._grabImage.Clone(), this.CameraParam, this.ImageIndex + 1));
                                    this.ImageIndex++;
                                }
                            }
                            break;
                    }
                    break;
                case enAcqMode.实时采集: // 获取图像与显示要加锁
                    if (this.IsLiveState)
                    {
                        lock (this._lockState)
                        {
                            this.GetImageSyn(out this._grabImage, out _grabDarkImage);
                            if (this._grabImage != null && this._grabImage.IsInitialized())
                                list.Add(enDataItem.Image, new ImageDataClass(this._grabImage.Clone(), this.CameraParam));
                        }
                    }
                    else
                    {
                        if (this._grabImage != null && this._grabImage.IsInitialized()) // 如果不为空，那么表示通过开始采集指令来启动
                            list.Add(enDataItem.Image, new ImageDataClass(this._grabImage.Clone(), this.CameraParam));
                    }
                    break;
                default:
                    switch (this.ConfigParam.ConnectType)
                    {
                        case enUserConnectType.Socket:
                            if (this._imageData != null && this._imageData.IsInitialized())
                                list.Add(enDataItem.Image, this._imageData.Clone());
                            else
                            {
                                this.GetImageSyn(out this._grabImage, out _grabDarkImage);
                                list.Add(enDataItem.Image, new ImageDataClass(this._grabImage.Clone(), this.CameraParam));
                            }
                            break;
                        default:
                            if (this.IsLiveState) // 如果在实时采集状态，那么将执行
                            {
                                this.GetImageSyn(out this._grabImage, out _grabDarkImage);
                                if (this._grabImage != null && this._grabImage.IsInitialized())
                                    list.Add(enDataItem.Image, new ImageDataClass(this._grabImage.Clone(), this.CameraParam));
                            }
                            else
                            {
                                if (this._grabImage != null && this._grabImage.IsInitialized())
                                    list.Add(enDataItem.Image, new ImageDataClass(this._grabImage.Clone(), this.CameraParam));
                            }
                            break;
                    }
                    break;
            }
            return list;
        }
        public object GetParam(object paramType)
        {
            object value = "";
            switch (this.ConfigParam.ConnectType)
            {
                case enUserConnectType.Map:
                    if (this.camera == null) return value;
                    value = SensorManage.GetSensor(this.ConfigParam.ConnectAddress).GetParam(paramType);
                    break;
                case enUserConnectType.Socket:
                    switch (paramType.ToString())
                    {
                        case "曝光":
                        case "获取曝光":
                            if (this._socket != null)
                            {
                                SocketMessage message = this._socket.GetDataAsync(new SocketMessage(enSocketInfo.获取曝光, this.Name, new object()), true) as SocketMessage; // 先获取值，再设置
                                if (message != null)
                                    value = message.MesContent;
                            }
                            break;
                        case "增益":
                        case "增溢":
                        case "获取增益":
                        case "获取增溢":
                            if (this._socket != null)
                            {
                                SocketMessage message = this._socket.GetDataAsync(new SocketMessage(enSocketInfo.获取增溢, this.Name, new object()), true) as SocketMessage;  // 先获取值，再设置
                                if (message != null)
                                    value = message.MesContent;
                            }
                            break;
                        case "触发模式":
                        case "获取触发模式":
                            if (this._socket != null)
                            {
                                SocketMessage message = this._socket.GetDataAsync(new SocketMessage(enSocketInfo.获取触发模式, this.Name, new object()), true) as SocketMessage;  // 先获取值，再设置
                                if (message != null)
                                    value = message.MesContent;
                            }
                            break;
                            //case "图像":
                            //    if (this._socket != null)
                            //    {
                            //        SocketMessage message = _socket.GetDataAsync(new SocketMessage(enSocketInfo.获取图像, new object()), true) as SocketMessage; // 先获取值，再设置
                            //        if (message != null)
                            //            value = message.Content;
                            //    }
                            //    break;
                    }
                    break;
                default:
                    if (this.camera == null) return value;
                    switch (paramType.ToString())
                    {
                        case "曝光":
                        case "获取曝光":
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
                        case "增溢":
                        case "获取增益":
                        case "获取增溢":
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
                        case "触发模式":
                        case "获取触发模式":
                            if (this.camera == null) return value;
                            value = this.camera.Parameters[Basler.Pylon.PLCamera.TriggerMode].GetValue();
                            break;
                        case "图像":
                            if (this.camera == null) return value;
                            value = new ImageDataClass(this._grabImage?.Clone(), this.CameraParam);
                            break;
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
                switch (this.ConfigParam.ConnectType)
                {
                    case enUserConnectType.Map:
                        if (this.camera == null) return result;
                        result = SensorManage.GetSensor(this.ConfigParam.ConnectAddress).SetParam(paramType, value);
                        break;
                    case enUserConnectType.Socket:
                        switch (paramType.ToString())
                        {
                            case "曝光":
                            case "设置曝光":
                                if (this._socket != null)
                                {
                                    SocketMessage message = new SocketMessage(enSocketInfo.设置曝光); // 先获取值，再设置
                                    message.MesContent = value;
                                    if (this._socket.SendDataAsync(message, true))
                                        result = true;
                                    else
                                        result = false;
                                }
                                break;
                            case "增益":
                            case "增溢":
                            case "设置增益":
                            case "设置增溢":
                                if (this._socket != null)
                                {
                                    SocketMessage message = new SocketMessage(enSocketInfo.设置增溢); // 先获取值，再设置
                                    message.MesContent = value;
                                    if (this._socket.SendDataAsync(message, true))
                                        result = true;
                                    else
                                        result = false;
                                }
                                break;
                            case "外部触发":
                            case "触发模式":
                                if (this._socket != null)
                                {
                                    SocketMessage message = new SocketMessage(enSocketInfo.获取触发模式); // 先获取值，再设置
                                    message.MesContent = value;
                                    if (this._socket.SendDataAsync(message, true))
                                        result = true;
                                    else
                                        result = false;
                                }
                                break;
                            case "软触发":
                                if (this._socket != null)
                                {
                                    SocketMessage message = new SocketMessage(enSocketInfo.NONE); // 先获取值，再设置
                                    message.MesContent = value;
                                    if (this._socket.SendDataAsync(message, true))
                                        result = true;
                                    else
                                        result = false;
                                }
                                break;
                            case "实时采集":
                            case "开始实时采集":
                                if (this._socket != null)
                                {
                                    SocketMessage message = new SocketMessage(enSocketInfo.开始实时采集); // 先获取值，再设置
                                    message.MesContent = value;
                                    if (this._socket.SendDataAsync(message, true))
                                        result = true;
                                    else
                                        result = false;
                                }
                                break;
                        }
                        break;
                    default:
                        if (this.camera == null) return result;
                        switch (paramType.ToString())
                        {
                            case "曝光":
                            case "设置曝光":
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
                                result = true;
                                break;
                            case "增益":
                            case "增溢":
                            case "设置增益":
                            case "设置增溢":
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
                                result = true;
                                break;
                            case "外部触发":
                            case "触发模式":
                                if (this.camera == null) return result;
                                SetExternTrigger();
                                result = true;
                                break;
                            case "软触发":
                                if (this.camera == null) return result;
                                this.SetSoftwareTrigger();
                                result = true;
                                break;
                            case "实时采集":
                            case "实时模式":
                            case "开始实时采集":
                                if (this.camera == null) return result;
                                if (!this.IsLiveState)
                                {
                                    this.IsLiveState = true; // 开始实时采集状态
                                    this.cts?.Cancel();
                                    Thread.Sleep(500);
                                    this.camera.StreamGrabber.ImageGrabbed -= OnImageGrabbed; // 取消回调注册
                                    //this._imageManage?.Init();
                                    if (this.camera.StreamGrabber.IsGrabbing)
                                        this.camera.StreamGrabber.Stop();
                                    this.camera.Parameters[Basler.Pylon.PLCamera.TriggerSelector].SetValue(Basler.Pylon.PLCamera.TriggerSelector.FrameStart);
                                    this.camera.Parameters[Basler.Pylon.PLCamera.TriggerMode].SetValue(Basler.Pylon.PLCamera.TriggerMode.Off);
                                    this.camera.StreamGrabber.Start(GrabStrategy.LatestImages, GrabLoop.ProvidedByUser);
                                }
                                ////////////////////////////////////////////////////////////
                                result = true;
                                break;
                            case "停止采集":
                                if (this.camera == null) return result;
                                if (this.IsLiveState)
                                {
                                    this.IsLiveState = false; // 停止实时采集状态
                                    this.cts?.Cancel();
                                    Thread.Sleep(500);
                                    if (this.camera.StreamGrabber.IsGrabbing)
                                        this.camera.StreamGrabber.Stop();
                                    // 设置采集参数
                                    this.SetAcqParam(false);
                                }
                                result = true;
                                break;
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
            switch (this.ConfigParam.ConnectType)
            {
                case enUserConnectType.Map:
                    if (this.camera == null) return result;
                    result = SensorManage.GetSensor(this._MapName).StartTrigger();
                    if (this._grabImage != null && this._grabImage.IsInitialized())
                        this._grabImage.Dispose();
                    if (this._grabDarkImage != null && this._grabDarkImage.IsInitialized())
                        this._grabDarkImage.Dispose();
                    this._grabImage = (SensorManage.GetSensor(this._MapName)).GrabImage; // 用于远程调用的映射处理
                    this._grabDarkImage = (SensorManage.GetSensor(this._MapName)).GrabDarkImage; // 用于远程调用的映射处理
                    break;
                case enUserConnectType.Socket:
                    if (this._socket != null)
                    {
                        SocketMessage message = new SocketMessage(enSocketInfo.图像采集, this.Name);
                        message.Name = this.Name;
                        object readValue = this._socket.GetDataAsync(message, true);
                        message = readValue as SocketMessage;
                        if (message != null)
                        {
                            switch (message.MesContent.GetType().Name)
                            {
                                case nameof(HImage):
                                    if (this._grabImage != null && this._grabImage.IsInitialized())
                                        this._grabImage.Dispose();
                                    this._grabImage = message.MesContent as HImage;
                                    this._imageData = new ImageDataClass(this._grabImage, this.CameraParam);
                                    result = true;
                                    break;
                                case nameof(ImageDataClass):
                                    if (this._grabImage != null && this._grabImage.IsInitialized())
                                        this._grabImage.Dispose();
                                    this._imageData = message.MesContent as ImageDataClass;
                                    result = true;
                                    break;
                            }
                        }
                    }
                    break;
                default:
                    if (this.camera == null) return result;
                    switch (this.CameraParam.AcqMode)
                    {
                        case enAcqMode.同步采集:
                            switch (this.CameraParam.TriggerSource)
                            {
                                case enUserTriggerSource.NONE:    // 实时采集
                                    if (this.camera.Parameters[Basler.Pylon.PLCamera.TriggerMode].GetValue() == "On")
                                        this.camera.Parameters[Basler.Pylon.PLCamera.TriggerMode].SetValue(Basler.Pylon.PLCamera.TriggerMode.Off);
                                    /////////////////////////////////
                                    if (this._grabImage != null && this._grabImage.IsInitialized())
                                        this._grabImage.Dispose();
                                    if (this._grabDarkImage != null && this._grabDarkImage.IsInitialized())
                                        this._grabDarkImage.Dispose();
                                    if (this.camera.StreamGrabber.IsGrabbing)
                                        this.camera.StreamGrabber.Stop();
                                    this.camera.StreamGrabber.Start(GrabStrategy.LatestImages, GrabLoop.ProvidedByUser);
                                    result = this.GetImageSyn(out this._grabImage, out _grabDarkImage);
                                    this.camera.StreamGrabber.Stop();
                                    if (result)
                                        LoggerHelper.Info(this.CameraParam.SensorName + "图像采集成功");
                                    else
                                        LoggerHelper.Info(this.CameraParam.SensorName + "图像采集失败");
                                    break;
                                case enUserTriggerSource.软触发:
                                    if (this._grabImage != null && this._grabImage.IsInitialized())
                                        this._grabImage.Dispose();
                                    if (!this.camera.StreamGrabber.IsGrabbing)
                                        this.camera.StreamGrabber.Start();
                                    if (this.camera.Parameters[Basler.Pylon.PLCamera.TriggerMode].GetValue() == "Off")
                                        this.SetSoftwareTrigger();
                                    ////////////////////////////////////////////////
                                    if (camera.WaitForFrameTriggerReady(this.CameraParam.Timeout, TimeoutHandling.Return))
                                        this.SendSoftwareExecute();// 软触发
                                    result = this.GetImageSyn(out this._grabImage, out _grabDarkImage);
                                    if (result)
                                        LoggerHelper.Info(this.CameraParam.SensorName + "图像采集成功");
                                    else
                                        LoggerHelper.Info(this.CameraParam.SensorName + "图像采集失败");
                                    this.IsSoftwareFinish = true;
                                    break;
                                default:
                                    if (this._grabImage != null && this._grabImage.IsInitialized())
                                        this._grabImage.Dispose();
                                    result = GetImageAsyn(out this._grabImage, out this._grabDarkImage);
                                    break;
                            }
                            break;
                        case enAcqMode.异步取图:
                            switch (this.CameraParam.TriggerSource)
                            {
                                case enUserTriggerSource.NONE: // 实时采集
                                    if (this.camera.StreamGrabber.IsGrabbing)
                                        this.camera.StreamGrabber.Stop();
                                    this._imageManage?.Init();
                                    this.ImageIndex = 0; //
                                    this.camera.StreamGrabber.Start(GrabStrategy.OneByOne, GrabLoop.ProvidedByStreamGrabber); // GrabLoop.ProvidedByUser:默认的抓取循环
                                    result = true;
                                    break;
                                case enUserTriggerSource.软触发: // 实时采集                            
                                    this._imageManage?.Init();
                                    this.ImageIndex = 0;
                                    this.SendSoftwareExecute();// 软触发
                                    result = true;
                                    break;
                                default:
                                    if (this._grabImage != null && this._grabImage.IsInitialized())
                                        this._grabImage.Dispose();
                                    result = GetImageAsyn(out this._grabImage, out this._grabDarkImage);
                                    break;
                            }
                            break;
                        case enAcqMode.异步采集:
                            this._imageManage.Init();
                            break;
                        case enAcqMode.实时采集:
                            lock (this._lockState)
                            {
                                if (this._grabImage != null && this._grabImage.IsInitialized())
                                    this._grabImage.Dispose();
                                result = GetImageSyn(out this._grabImage, out this._grabDarkImage);
                            }
                            break;
                    }
                    break;
            }
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
                        if (this.camera == null) return result;
                        result = SensorManage.GetSensor(this._MapName).StopTrigger();
                        break;
                    case enUserConnectType.Socket:
                        result = true;
                        break;
                    default:
                        if (this.camera == null) return result;
                        switch (this.CameraParam.AcqMode)
                        {
                            case enAcqMode.同步采集:
                                result = true;
                                break;
                            case enAcqMode.异步取图:
                                switch (this.CameraParam.TriggerSource)
                                {
                                    case enUserTriggerSource.NONE: // 实时采集
                                        camera.StreamGrabber.Stop();
                                        result = true;
                                        break;
                                    case enUserTriggerSource.外部IO触发:
                                    case enUserTriggerSource.内部IO触发:
                                    case enUserTriggerSource.编码器触发:
                                    case enUserTriggerSource.软触发:
                                        break;
                                }
                                break;
                            case enAcqMode.异步采集:
                                result = true;
                                break;
                            case enAcqMode.实时采集:
                                result = true;
                                break;
                            default:
                                result = true;
                                break;
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                result = false;
                LoggerHelper.Error(this.Name + ":停止相机采集失败" + ex);
            }
            return result;
        }


        private bool SetAcqParam(bool initAcq = false)
        {
            bool result = false;
            switch (this.CameraParam.AcqMode)
            {
                default:
                case enAcqMode.同步采集:
                    switch (this.CameraParam.TriggerSource)
                    {
                        case enUserTriggerSource.NONE:
                            // 设置触发模式
                            this.camera.Parameters[Basler.Pylon.PLCamera.TriggerSelector].SetValue(Basler.Pylon.PLCamera.TriggerSelector.FrameStart);
                            this.camera.Parameters[Basler.Pylon.PLCamera.TriggerMode].SetValue(Basler.Pylon.PLCamera.TriggerMode.Off);
                            result = true;
                            break;
                        case enUserTriggerSource.软触发:
                            SetSoftwareTrigger();
                            if (!this.camera.StreamGrabber.IsGrabbing)
                                this.camera.StreamGrabber.Start(GrabStrategy.OneByOne, GrabLoop.ProvidedByUser);
                            result = true;
                            break;
                        default:
                            SetExternTrigger();
                            if (!this.camera.StreamGrabber.IsGrabbing)
                                this.camera.StreamGrabber.Start(GrabStrategy.OneByOne, GrabLoop.ProvidedByUser); // 同步采集通过循环来取图
                            result = true;
                            break;
                    }
                    break;
                case enAcqMode.异步取图: // 理解异步取图与异步采集的差异，异步取图要手动开启图像采集和图像停止
                    this.camera.StreamGrabber.ImageGrabbed += OnImageGrabbed;
                    switch (this.CameraParam.TriggerSource)
                    {
                        case enUserTriggerSource.NONE:
                            // 设置触发模式
                            this.camera.Parameters[Basler.Pylon.PLCamera.TriggerSelector].SetValue(Basler.Pylon.PLCamera.TriggerSelector.FrameStart);
                            this.camera.Parameters[Basler.Pylon.PLCamera.TriggerMode].SetValue(Basler.Pylon.PLCamera.TriggerMode.Off);
                            result = true;
                            break;
                        case enUserTriggerSource.软触发:
                            SetSoftwareTrigger();
                            if (!this.camera.StreamGrabber.IsGrabbing)
                                this.camera.StreamGrabber.Start(GrabStrategy.OneByOne, GrabLoop.ProvidedByStreamGrabber); // 抓取循环设置为：ProvidedByStreamGrabber才能注册回调函数
                            result = true;
                            break;
                        default:
                            SetExternTrigger();
                            if (!this.camera.StreamGrabber.IsGrabbing)
                                this.camera.StreamGrabber.Start(GrabStrategy.OneByOne, GrabLoop.ProvidedByStreamGrabber); // 抓取循环设置为：ProvidedByStreamGrabber才能注册回调函数
                            result = true;
                            break;
                    }
                    break;
                case enAcqMode.异步采集:
                    this.camera.StreamGrabber.ImageGrabbed += OnImageGrabbed;
                    this._imageManage?.Init();
                    this.GetImageAsynRun();
                    switch (this.CameraParam.TriggerSource)
                    {
                        case enUserTriggerSource.NONE:
                            // 设置触发模式
                            this.camera.Parameters[Basler.Pylon.PLCamera.TriggerSelector].SetValue(Basler.Pylon.PLCamera.TriggerSelector.FrameStart);
                            this.camera.Parameters[Basler.Pylon.PLCamera.TriggerMode].SetValue(Basler.Pylon.PLCamera.TriggerMode.Off);
                            result = true;
                            break;
                        case enUserTriggerSource.软触发:
                            SetSoftwareTrigger();
                            if (!this.camera.StreamGrabber.IsGrabbing)
                                this.camera.StreamGrabber.Start(GrabStrategy.OneByOne, GrabLoop.ProvidedByStreamGrabber); // 抓取循环设置为：ProvidedByStreamGrabber才能注册回调函数
                            result = true;
                            break;
                        default:
                            SetExternTrigger();
                            if (!this.camera.StreamGrabber.IsGrabbing)
                                this.camera.StreamGrabber.Start(GrabStrategy.OneByOne, GrabLoop.ProvidedByStreamGrabber); // 抓取循环设置为：ProvidedByStreamGrabber才能注册回调函数
                            result = true;
                            break;
                    }
                    break;
                case enAcqMode.实时采集:
                    this.camera.Parameters[Basler.Pylon.PLCamera.TriggerSelector].SetValue(Basler.Pylon.PLCamera.TriggerSelector.FrameStart);
                    this.camera.Parameters[Basler.Pylon.PLCamera.TriggerMode].SetValue(Basler.Pylon.PLCamera.TriggerMode.Off);
                    if (initAcq)
                    {
                        this.camera.StreamGrabber.Start(GrabStrategy.LatestImages, GrabLoop.ProvidedByUser);
                        if (this.camera.IsOpen)
                            this.GetImageAsynRun();
                    }
                    result = true;
                    break;
            }
            return result;
        }
        private void SetExternTrigger()
        {
            try
            {
                if (this.camera != null && this.camera.GetSfncVersion() < Sfnc2_0_0)
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
                if (this.camera != null && this.camera.GetSfncVersion() < Sfnc2_0_0)
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
                if (this.camera != null && this.camera.WaitForFrameTriggerReady(1000, Basler.Pylon.TimeoutHandling.ThrowException))
                {
                    this.camera.ExecuteSoftwareTrigger();
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.Error("软触发失败" + ex);
            }
        }

        private void OnImageGrabbed(Object sender, ImageGrabbedEventArgs e)
        {
            if (!this.IsLiveState && e.GrabResult.GrabSucceeded)
            {
                this.CameraParam.DataWidth = e.GrabResult.Width;
                this.CameraParam.DataHeight = e.GrabResult.Height;
                this._imageManage.FramWidth = e.GrabResult.Width;
                this._imageManage.FramHeight = e.GrabResult.Height;
                switch (e.GrabResult.PixelTypeValue)
                {
                    case PixelType.Mono8:
                        this._imageManage.AddImage(e.GrabResult.PixelDataPointer, e.GrabResult.Width, e.GrabResult.Height, 1, enPixFormat.Mono8);
                        break;
                    case PixelType.RGB8planar:
                        this._imageManage.AddImage(e.GrabResult.PixelDataPointer, e.GrabResult.Width, e.GrabResult.Height * 3, 1, enPixFormat.RGB8);
                        break;
                }
            }
        }

        protected override bool GetImageAsyn(out HImage hImage, out HImage darkImage)
        {
            bool result = false;
            int bufferNum = 0;
            result = this._imageManage.GetHImage(enAcqMode.异步取图, enImageAcqMethod.明场, this.CameraParam.Timeout, out hImage, out darkImage, out bufferNum);
            return result;
        }

        protected override bool GetImageSyn(out HImage hImage, out HImage darkImage)
        {
            hImage = new HImage();
            darkImage = new HImage();
            bool result = true;
            if (this.camera == null) return false;
            //this.camera.Parameters[PLCamera.AcquisitionMode].SetValue("SingleFrame");
            List<Basler.Pylon.IGrabResult> imageListInfo = new List<IGrabResult>();
            for (int i = 0; i < this.CameraParam.AverangeCount; i++)
            {
                if (this.StreamGrabber.IsGrabbing)
                    imageListInfo.Add(this.StreamGrabber.RetrieveResult(this.CameraParam.Timeout, Basler.Pylon.TimeoutHandling.Return));
                //this.StreamGrabber.GrabOne(this.CameraParam.Timeout, Basler.Pylon.TimeoutHandling.ThrowException)
            }
            List<HImage> imageList = new List<HImage>();
            foreach (var item in imageListInfo)
            {
                if (item != null && item.GrabSucceeded)
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
                item?.Dispose();
            }
            foreach (var item in imageList)
            {
                item?.Dispose();
            }
            imageListInfo.Clear();
            imageList.Clear();
            return result;
        }

        protected override void GetImageAsynRun()
        {
            this.cts?.Cancel();
            this.cts = new CancellationTokenSource();
            Task.Run(() =>
            {
                while (true)
                {
                    if (this.cts.IsCancellationRequested) break;
                    HImage darkImage = null;
                    HImage image = null;
                    switch (this.CameraParam.AcqMode)
                    {
                        case enAcqMode.实时采集:
                            lock (this._lockState)
                            {
                                if (this.GetImageSyn(out image, out darkImage))
                                {
                                    if (image != null && image.IsInitialized())
                                        this.OnImageAcqComplete(this.Name, new ImageDataClass(image.Clone(), this.CameraParam, this._imageManage.CurImageIndex)); // 异步发送图像出去
                                    image?.Dispose();
                                    darkImage?.Dispose();
                                }
                            }
                            break;
                        default:
                            int bufferNum = 0;
                            if (this._imageManage.GetHImage(enAcqMode.异步采集, enImageAcqMethod.明场, this.CameraParam.Timeout, out image, out darkImage, out bufferNum))
                            {
                                this.AdjImg(image, out this._grabImage);
                                this.AdjImg(darkImage, out this._grabDarkImage);
                                image?.Dispose();
                                darkImage?.Dispose();
                                if (this._grabImage != null && this._grabImage.IsInitialized())
                                    this.OnImageAcqComplete(this.Name, new ImageDataClass(this._grabImage, this.CameraParam, this._imageManage.CurImageIndex)); // 异步发送图像出去
                                if (this._grabDarkImage != null && this._grabDarkImage.IsInitialized())
                                    this.OnImageAcqComplete(this.Name, new ImageDataClass(this._grabDarkImage, this.CameraParam, this._imageManage.CurImageIndex)); // 异步发送图像出去
                                LoggerHelper.Debug("Buffer绶存数量 = " + bufferNum.ToString());
                            }
                            break;
                    }
                    //HImage darkImage = null;
                    //HImage lightImage = null;
                    //int bufferNum = 0;
                    //if (this._imageManage.GetHImage(enAcqMode.异步采集, enImageAcqMethod.明场, this.CameraParam.Timeout, out lightImage, out darkImage, out bufferNum))
                    //{
                    //    this.AdjImg(lightImage, out this._grabImage);
                    //    this.AdjImg(darkImage, out this._grabDarkImage);
                    //    lightImage?.Dispose();
                    //    darkImage?.Dispose();
                    //    if (this._grabImage != null && this._grabImage.IsInitialized())
                    //        this.OnImageAcqComplete(this.Name, new ImageDataClass(this._grabImage, this.CameraParam, (this._imageManage.CurImageIndex + 1))); // 异步发送图像出去
                    //    if (this._grabDarkImage != null && this._grabDarkImage.IsInitialized())
                    //        this.OnImageAcqComplete(this.Name, new ImageDataClass(this._grabDarkImage, this.CameraParam, (this._imageManage.CurImageIndex + 1))); // 异步发送图像出去
                    //    LoggerHelper.Debug("Buffer绶存数量 = " + bufferNum.ToString());
                    //}
                    Thread.Sleep(20);
                }
            });
        }





    }
}

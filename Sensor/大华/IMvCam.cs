using Common;
using HalconDotNet;
using MVSDK_Net;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static MVSDK_Net.IMVDefine;

namespace Sensor
{
    public class IMvCam : SensorBase, ISensor
    {
        private IMVDefine.IMV_Frame m_frame;
        private MyCamera camera = null;
        private int res = IMVDefine.IMV_OK;
        private IMVDefine.IMV_FrameCallBack frameCallBack;
        private Dictionary<int, ImageByteData> dicImage = new Dictionary<int, ImageByteData>();
        private int _CurrentImageIndex = 0;
        private int m_CurImageIndex = 0; //
        private int dataFrameIndex = 0;
        private int imageBufferCount = 100; // 图像绶存数量即字典的个数
        private int m_FrameNumByImage = 1; // 对于面阵相机，该值等于1
        private Stopwatch stopwatch = new Stopwatch();
        private int ImageIndex = 0;
        private ulong imageWidth = 0;
        private ulong imageHeight = 0;

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
                if (!configParam.IsActive) return result;
                this.cts?.Cancel();
                ///////////////////////////////////
                switch (configParam.ConnectType)
                {
                    /// 大华面阵相机接口
                    case enUserConnectType.DeviceName:
                        result = this.OpenByDeviceUserID(this.ConfigParam.ConnectAddress);
                        if (result)
                        {
                            this.camera.IMV_GetEnumFeatureValue("Width", ref this.imageWidth);
                            this.camera.IMV_GetEnumFeatureValue("Height", ref this.imageHeight);
                            this._imageManage.FramWidth = (int)this.imageWidth;
                            this._imageManage.FramHeight = (int)this.imageHeight;
                            // 设置采集方式
                            this.SetAcqParam();
                        }
                        break;
                    case enUserConnectType.TcpIp:
                        result = this.OpenByIPAddress(configParam.ConnectAddress);
                        if (result)
                        {
                            this.camera.IMV_GetEnumFeatureValue("Width", ref this.imageWidth);
                            this.camera.IMV_GetEnumFeatureValue("Height", ref this.imageHeight);
                            this._imageManage.FramWidth = (int)this.imageWidth;
                            this._imageManage.FramHeight = (int)this.imageHeight;
                            // 设置采集方式
                            this.SetAcqParam();
                        }
                        break;
                    case enUserConnectType.SerialNumber:
                        result = this.OpenByCameraKey(configParam.ConnectAddress);
                        if (result)
                        {
                            this.camera.IMV_GetEnumFeatureValue("Width", ref this.imageWidth);
                            this.camera.IMV_GetEnumFeatureValue("Height", ref this.imageHeight);
                            this._imageManage.FramWidth = (int)this.imageWidth;
                            this._imageManage.FramHeight = (int)this.imageHeight;
                            // 设置采集方式
                            this.SetAcqParam();
                        }
                        break;
                    case enUserConnectType.USB:
                        result = this.OpenByUSB(this.Name);
                        if (result)
                        {
                            this.camera.IMV_GetEnumFeatureValue("Width", ref this.imageWidth);
                            this.camera.IMV_GetEnumFeatureValue("Height", ref this.imageHeight);
                            this._imageManage.FramWidth = (int)this.imageWidth;
                            this._imageManage.FramHeight = (int)this.imageHeight;
                            // 设置采集方式
                            this.SetAcqParam();
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
                    case enUserConnectType.NONE: // 如果为NONE，那就默认OK 
                        result = true;
                        break;
                    /// halcon接口适用于所有支持 Gige 接口的相机 
                    default:
                        new UserMessageForm().ShowDialog(configParam?.ConnectType.ToString() + "连接类型未实现，请将连接类型设置为：" + enUserConnectType.DeviceName.ToString() + "或" + enUserConnectType.SerialNumber);
                        break;
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.Error(this.Name + "打开相机失败", ex);
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
                    res = this.camera.IMV_StopGrabbing();
                    res = this.camera.IMV_Close();
                    if (res != IMVDefine.IMV_OK)
                        result = true;
                    else
                        result = false;
                    // 销毁设备句柄
                    // Destroy Device Handle
                    res = this.camera.IMV_DestroyHandle();
                    if (res != IMVDefine.IMV_OK)
                        result = true;
                    else
                        result = false;
                }
                result = true;
                LoggerHelper.Info(this.CameraParam.SensorName + "关闭相机成功");
            }
            catch (Exception ex)
            {
                LoggerHelper.Error(this.Name + "关闭相机失败", ex);
                result = false;
            }
            return result;
        }

        public bool Init()
        {
            try
            {
                this._imageManage = new ImageManage(100);
                this.camera = new MyCamera();
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
                    if (this.IsLiveState)
                    {
                        this.GetImageSyn(out this._grabImage, out _grabDarkImage);
                        if (this._grabImage != null && this._grabImage.IsInitialized())
                            list.Add(enDataItem.Image, new ImageDataClass(this._grabImage.Clone(), this.CameraParam));
                    }
                    break;
                case enAcqMode.异步取图:
                    if (this.IsLiveState)
                    {
                        this.GetImageSyn(out this._grabImage, out _grabDarkImage);
                        if (this._grabImage != null && this._grabImage.IsInitialized())
                            list.Add(enDataItem.Image, new ImageDataClass(this._grabImage.Clone(), this.CameraParam));
                    }
                    else
                    {
                        this.GetImageAsyn(out this._grabImage, out _grabDarkImage);
                        if (this._grabImage != null && this._grabImage.IsInitialized())
                        {
                            list.Add(enDataItem.Image, new ImageDataClass(this._grabImage.Clone(), this.CameraParam, this.ImageIndex + 1)); // 异步取图时，传入获取的图像数量索引
                            this.ImageIndex++;
                        }
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
                    if (this.IsLiveState)
                    {
                        this.GetImageSyn(out this._grabImage, out _grabDarkImage);
                        if (this._grabImage != null && this._grabImage.IsInitialized())
                            list.Add(enDataItem.Image, new ImageDataClass(this._grabImage.Clone(), this.CameraParam));
                    }
                    else
                    {
                        if (this._grabImage != null && this._grabImage.IsInitialized()) // 如果不为空，那么表示通过开始采集指令来启动
                            list.Add(enDataItem.Image, new ImageDataClass(this._grabImage.Clone(), this.CameraParam));
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
                    value = SensorManage.GetSensor(this.ConfigParam.ConnectAddress).GetParam(paramType);
                    break;
                default:
                    switch (paramType.ToString())
                    {
                        case "曝光":
                            if (camera == null) return value;
                            double expose1 = 0;
                            res = this.camera.IMV_GetDoubleFeatureValue("ExposureTime", ref expose1);
                            value = expose1;
                            break;
                        case "增益":
                        case "增溢":
                            if (camera == null) return value;
                            double Gain = 0;
                            res = this.camera.IMV_GetDoubleFeatureValue("Gain", ref Gain);
                            value = Gain;
                            break;
                        case "Image":
                            value = this._grabImage;
                            break;
                        case "实时采集":
                        case "开始实时采集":
                            if (this.camera == null) return value;

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
                        result = SensorManage.GetSensor(this.ConfigParam.ConnectAddress).SetParam(paramType, value);
                        break;
                    default:
                        switch (paramType.ToString())
                        {
                            case "曝光":
                            case "Expose":
                            case "ExposureTime":
                                if (this.camera == null) return result;
                                double expose = 0;
                                double.TryParse(value.ToString(), out expose);
                                res = this.camera.IMV_SetDoubleFeatureValue("ExposureTime", expose);
                                if (res == 0) result = true;
                                break;
                            case "增益":
                                if (this.camera == null) return result;
                                double Gain = 0;
                                double.TryParse(value.ToString(), out Gain);
                                res = this.camera.IMV_SetDoubleFeatureValue("Gain", Gain);
                                if (res == 0) result = true;
                                break;
                            case "清空数据":
                            case "重置图像采集":
                            case "启动采集":
                                this._CurrentImageIndex = 0;
                                this.camera.IMV_StopGrabbing();
                                this.ClearData();
                                this.camera.IMV_StartGrabbing();
                                break;
                            case "实时采集":
                            case "开始实时采集":
                                if (this.camera == null) return result;
                                if (!this.IsLiveState)
                                {
                                    this.IsLiveState = true;
                                    this.cts?.Cancel();
                                    this.camera.IMV_StopGrabbing();
                                    res = this.camera.IMV_SetEnumFeatureSymbol("TriggerMode", "Off");
                                    res = this.camera.IMV_StartGrabbingEx(0, IMVDefine.IMV_EGrabStrategy.grabStrartegyLatestImage);
                                }
                                if (res == 0) result = true;
                                break;
                            case "停止采集":
                                if (this.camera == null) return result;
                                if (this.IsLiveState)
                                {
                                    this.IsLiveState = false;
                                    if (this.camera.IMV_IsGrabbing())
                                        res = camera.IMV_StopGrabbing();
                                    // 设置采集参数
                                    this.SetAcqParam(true);
                                }
                                break;
                        }
                        break;
                }
            }
            catch
            {
                result = false;
            }
            return result;
        }

        public bool StartTrigger()
        {
            bool result = false;
            this._CurrentImageIndex = 0;
            this.m_CurImageIndex = 0;
            this.dataFrameIndex = 0; // 统计在一次采集中帧的总数量
            switch (this.ConfigParam.ConnectType)
            {
                case enUserConnectType.Map:
                    result = SensorManage.GetSensor(this._MapName).StartTrigger();
                    if (this._grabImage != null && this._grabImage.IsInitialized())
                        this._grabImage.Dispose();
                    if (this._grabDarkImage != null && this._grabDarkImage.IsInitialized())
                        this._grabDarkImage.Dispose();
                    this._grabImage = (SensorManage.GetSensor(this._MapName)).GrabImage;   // 用于远程调用的映射处理
                    this._grabDarkImage = (SensorManage.GetSensor(this._MapName)).GrabDarkImage;  // 用于远程调用的映射处理
                    break;
                default:
                    if (this.camera == null) return result;
                    switch (this.CameraParam.AcqMode)
                    {
                        case enAcqMode.同步采集:
                            switch (this.CameraParam.TriggerSource)
                            {
                                case enUserTriggerSource.NONE:    // 实时采集
                                    if (this._grabImage != null && this._grabImage.IsInitialized())
                                        this._grabImage.Dispose();
                                    if (!camera.IMV_IsGrabbing())
                                        res = camera.IMV_StartGrabbing();
                                    result = GetImageSyn(out this._grabImage, out this._grabDarkImage);
                                    res = camera.IMV_StopGrabbing();
                                    break;
                                case enUserTriggerSource.软触发:
                                    if (this._grabImage != null && this._grabImage.IsInitialized())
                                        this._grabImage.Dispose();
                                    if (!camera.IMV_IsGrabbing())
                                        res = camera.IMV_StartGrabbing();
                                    this.SendSoftwareExecute();   // 软触发
                                    result = GetImageSyn(out this._grabImage, out this._grabDarkImage);
                                    break;
                                default:
                                    if (this._grabImage != null && this._grabImage.IsInitialized())
                                        this._grabImage.Dispose();
                                    result = GetImageSyn(out this._grabImage, out this._grabDarkImage);
                                    break;
                            }
                            break;
                        case enAcqMode.异步取图:
                            switch (this.CameraParam.TriggerSource)
                            {
                                case enUserTriggerSource.NONE: // 实时采集
                                    if (camera.IMV_IsGrabbing())
                                        res = camera.IMV_StopGrabbing();
                                    this._imageManage?.Init();
                                    this.ImageIndex = 0;
                                    res = camera.IMV_StartGrabbing();
                                    if (res == 0) result = true;
                                    break;
                                case enUserTriggerSource.软触发: // 实时采集                            
                                    this._imageManage?.Init();
                                    this.ImageIndex = 0;
                                    if (!camera.IMV_IsGrabbing())
                                        res = camera.IMV_StartGrabbing();
                                    this.SendSoftwareExecute();// 软触发
                                    if (res == 0) result = true;
                                    break;
                                default:
                                    if (this._grabImage != null && this._grabImage.IsInitialized())
                                        this._grabImage.Dispose();
                                    result = GetImageSyn(out this._grabImage, out this._grabDarkImage);
                                    break;
                            }
                            break;
                        case enAcqMode.异步采集:
                            this._imageManage?.Init();
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
                    //////////////////////////////////////////////////////////////////////////
                    if (result)
                        LoggerHelper.Info(this.CameraParam.SensorName + "图像采集成功");
                    else
                        LoggerHelper.Info(this.CameraParam.SensorName + "图像采集失败");
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
                        result = SensorManage.GetSensor(this._MapName).StopTrigger();
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
                                        res = camera.IMV_StopGrabbing();
                                        if (res == 0) result = true;
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
                LoggerHelper.Error(this.Name + ":相机停止采集失败" + ex);
            }
            return result;
        }

        protected bool OpenByDeviceUserID(string camName)
        {
            bool result = true;
            IMVDefine.IMV_DeviceList deviceList = new IMVDefine.IMV_DeviceList();
            IMVDefine.IMV_EInterfaceType interfaceTp = IMVDefine.IMV_EInterfaceType.interfaceTypeAll;
            res = MyCamera.IMV_EnumDevices(ref deviceList, (uint)interfaceTp);
            if (res != IMVDefine.IMV_OK)
            {
                result = false;
            }
            if (deviceList.nDevNum < 1)
            {
                result = false;
            }
            // 创建设备句柄
            // Create Device Handle
            res = camera.IMV_CreateHandle(IMVDefine.IMV_ECreateHandleMode.modeByDeviceUserID, 0, camName);
            if (res != IMVDefine.IMV_OK)
            {
                result = false;
            }
            // 打开相机设备 
            // Connect to camera 
            res = camera.IMV_Open();
            if (res != IMVDefine.IMV_OK)
            {
                result = false;
            }
            //IMVDefine.IMV_DeviceInfo pDevInfo = new IMVDefine.IMV_DeviceInfo();
            //camera.IMV_GetDeviceInfo(ref pDevInfo);
            //设置缓存个数为8
            //set buffer count to 8
            res = camera.IMV_SetBufferCount(8);
            if (res != IMVDefine.IMV_OK)
            {
                result = false;
            }
            return result;
        }

        protected bool OpenByCameraKey(string cameraKey)
        {
            bool result = true;
            IMVDefine.IMV_DeviceList deviceList = new IMVDefine.IMV_DeviceList();
            IMVDefine.IMV_EInterfaceType interfaceTp = IMVDefine.IMV_EInterfaceType.interfaceTypeAll;
            res = MyCamera.IMV_EnumDevices(ref deviceList, (uint)interfaceTp);
            if (res != IMVDefine.IMV_OK)
            {
                result = false;
            }
            if (deviceList.nDevNum < 1)
            {
                result = false;
            }
            // 创建设备句柄
            // Create Device Handle
            res = camera.IMV_CreateHandle(IMVDefine.IMV_ECreateHandleMode.modeByCameraKey, 0, cameraKey);
            if (res != IMVDefine.IMV_OK)
            {
                result = false;
            }
            // 打开相机设备 
            // Connect to camera 
            res = camera.IMV_Open();
            if (res != IMVDefine.IMV_OK)
            {
                result = false;
            }
            //IMVDefine.IMV_DeviceInfo pDevInfo = new IMVDefine.IMV_DeviceInfo();
            //camera.IMV_GetDeviceInfo(ref pDevInfo);
            //设置缓存个数为8
            //set buffer count to 8
            res = camera.IMV_SetBufferCount(8);
            if (res != IMVDefine.IMV_OK)
            {
                result = false;
            }
            return result;
        }

        protected bool OpenByIPAddress(string IPAddress)
        {
            bool result = true;
            IMVDefine.IMV_DeviceList deviceList = new IMVDefine.IMV_DeviceList();
            IMVDefine.IMV_EInterfaceType interfaceTp = IMVDefine.IMV_EInterfaceType.interfaceTypeGige;
            res = MyCamera.IMV_EnumDevices(ref deviceList, (uint)interfaceTp);
            if (res != IMVDefine.IMV_OK)
            {
                result = false;
            }
            if (deviceList.nDevNum < 1)
            {
                result = false;
            }
            // 创建设备句柄
            // Create Device Handle
            res = camera.IMV_CreateHandle(IMVDefine.IMV_ECreateHandleMode.modeByIPAddress, 0, IPAddress);
            if (res != IMVDefine.IMV_OK)
            {
                result = false;
            }
            // 打开相机设备 
            // Connect to camera 
            res = camera.IMV_Open();
            if (res != IMVDefine.IMV_OK)
            {
                result = false;
            }
            //IMVDefine.IMV_DeviceInfo pDevInfo = new IMVDefine.IMV_DeviceInfo();
            //camera.IMV_GetDeviceInfo(ref pDevInfo);
            //设置缓存个数为8
            //set buffer count to 8
            res = camera.IMV_SetBufferCount(8);
            if (res != IMVDefine.IMV_OK)
            {
                result = false;
            }
            return result;
        }

        protected bool OpenByUSB(string camName)
        {
            bool result = true;
            IMVDefine.IMV_DeviceList deviceList = new IMVDefine.IMV_DeviceList();
            IMVDefine.IMV_EInterfaceType interfaceTp = IMVDefine.IMV_EInterfaceType.interfaceTypeUsb3;
            res = MyCamera.IMV_EnumDevices(ref deviceList, (uint)interfaceTp);
            if (res != IMVDefine.IMV_OK)
            {
                result = false;
            }
            if (deviceList.nDevNum < 1)
            {
                result = false;
            }
            // 创建设备句柄
            // Create Device Handle
            res = camera.IMV_CreateHandle(IMVDefine.IMV_ECreateHandleMode.modeByDeviceUserID, 0, camName);
            if (res != IMVDefine.IMV_OK)
            {
                result = false;
            }
            // 打开相机设备 
            // Connect to camera 
            res = camera.IMV_Open();
            if (res != IMVDefine.IMV_OK)
            {
                result = false;
            }
            //IMVDefine.IMV_DeviceInfo pDevInfo = new IMVDefine.IMV_DeviceInfo();
            //camera.IMV_GetDeviceInfo(ref pDevInfo);
            //设置缓存个数为8
            //set buffer count to 8
            res = camera.IMV_SetBufferCount(8);
            if (res != IMVDefine.IMV_OK)
            {
                result = false;
            }
            return result;
        }

        private int SetAcqParam(bool initAcq = false)
        {
            int res = 0;
            switch (this.CameraParam.AcqMode)
            {
                case enAcqMode.同步采集:
                    switch (this.CameraParam.TriggerSource)
                    {
                        case enUserTriggerSource.NONE:
                            // 设置触发模式
                            res = this.camera.IMV_SetEnumFeatureSymbol("TriggerMode", "Off");
                            break;
                        case enUserTriggerSource.软触发:
                            SetSoftwareTrigger();
                            if (!camera.IMV_IsGrabbing())
                                res = camera.IMV_StartGrabbing();
                            break;
                        default:
                            SetExternTrigger();
                            if (!camera.IMV_IsGrabbing())
                                res = camera.IMV_StartGrabbing();
                            break;
                    }
                    break;
                case enAcqMode.异步取图: // 理解异步取图与异步采集的差异，异步取图要手动开启图像采集和图像停止
                    frameCallBack = OnGetFrame;
                    res = camera.IMV_AttachGrabbing(frameCallBack, IntPtr.Zero);
                    switch (this.CameraParam.TriggerSource)
                    {
                        case enUserTriggerSource.NONE:
                            // 设置触发模式
                            res = this.camera.IMV_SetEnumFeatureSymbol("TriggerMode", "Off");
                            break;
                        case enUserTriggerSource.软触发:
                            SetSoftwareTrigger();
                            if (!camera.IMV_IsGrabbing())
                                res = camera.IMV_StartGrabbing();
                            break;
                        default:
                            SetExternTrigger();
                            if (!camera.IMV_IsGrabbing())
                                res = camera.IMV_StartGrabbing();
                            break;
                    }
                    break;
                case enAcqMode.异步采集:
                    frameCallBack = OnGetFrame;
                    res = camera.IMV_AttachGrabbing(frameCallBack, IntPtr.Zero);
                    this.GetImageAsynRun();
                    switch (this.CameraParam.TriggerSource)
                    {
                        case enUserTriggerSource.NONE:
                            // 设置触发模式
                            res = this.camera.IMV_SetEnumFeatureSymbol("TriggerMode", "Off");
                            break;
                        case enUserTriggerSource.软触发:
                            SetSoftwareTrigger();
                            if (!camera.IMV_IsGrabbing())
                                res = camera.IMV_StartGrabbing();
                            break;
                        default:
                            SetExternTrigger();
                            if (!camera.IMV_IsGrabbing())
                                res = camera.IMV_StartGrabbing();
                            break;
                    }
                    break;
                case enAcqMode.实时采集: // 
                    res = this.camera.IMV_SetEnumFeatureSymbol("TriggerMode", "Off");
                    if (initAcq)
                    {
                        res = this.camera.IMV_StartGrabbingEx(0, IMVDefine.IMV_EGrabStrategy.grabStrartegyLatestImage); // 嵌入时，这里不内能开始采集
                        if (res == 0)
                            this.GetImageAsynRun();
                    }
                    break;
            }
            return res;
        }
        protected void SetExternTrigger()
        {
            try
            {
                res = this.camera.IMV_SetEnumFeatureSymbol("TriggerMode", "On");
            }
            catch (Exception e)
            {
                LoggerHelper.Error(e);
            }
        }
        protected bool SetSoftwareTrigger()
        {
            bool result = true;
            try
            {
                int res = this.camera.IMV_SetEnumFeatureSymbol("TriggerSource", "Software");
                if (res != IMVDefine.IMV_OK)
                    result = true;
                else
                    result = false;
                // 设置触发器 
                // Set trigger selector to FrameStart 
                res = this.camera.IMV_SetEnumFeatureSymbol("TriggerSelector", "FrameStart");
                if (res != IMVDefine.IMV_OK)
                    result = true;
                else
                    result = false;
                // 设置触发模式 
                // Set trigger mode to On 
                res = this.camera.IMV_SetEnumFeatureSymbol("TriggerMode", "On");
                if (res != IMVDefine.IMV_OK)
                    result = true;
                else
                    result = false;
            }
            catch (Exception e)
            {
                LoggerHelper.Error(e);
            }
            return result;
        }
        protected bool SendSoftwareExecute()
        {
            bool result = true;
            try
            {
                res = this.camera.IMV_ExecuteCommandFeature("TriggerSoftware");
                if (res != IMVDefine.IMV_OK)
                    result = true;
                else
                    result = false;
            }
            catch (Exception ex)
            {
                result = false;
                LoggerHelper.Error("软触发失败" + ex);
            }
            return result;
        }
        protected void OnGetFrame(ref IMVDefine.IMV_Frame frame, IntPtr pUser)
        {
            if (frame.frameHandle != IntPtr.Zero)
            {
                this.m_frame = frame;
                this.CameraParam.DataWidth = (int)frame.frameInfo.width;
                this.CameraParam.DataHeight = (int)frame.frameInfo.height; // 重置参数,不能在这里赋值
                this._imageManage.FramWidth = (int)frame.frameInfo.width;
                this._imageManage.FramHeight = (int)frame.frameInfo.height;
                switch (frame.frameInfo.pixelFormat)
                {
                    case IMVDefine.IMV_EPixelType.gvspPixelMono8:
                        this._imageManage.AddImage(frame.pData, (int)frame.frameInfo.width, (int)frame.frameInfo.height, 1, enPixFormat.Mono8);
                        break;
                    case IMVDefine.IMV_EPixelType.gvspPixelRGB8:
                        this._imageManage.AddImage(frame.pData, (int)frame.frameInfo.width, (int)frame.frameInfo.height * 3, 1, enPixFormat.RGB8);
                        break;
                }
            }
        }

        /// <summary>
        ///  异步采集图像，从绶存中获取图像数据
        /// </summary>
        /// <param name="hImage"></param>
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
            List<HImage> imageList = new List<HImage>();
            for (int i = 0; i < this.CameraParam.AverangeCount; i++)
            {
                res = this.camera.IMV_GetFrame(ref this.m_frame, (uint)this.CameraParam.Timeout);
                if (res == IMVDefine.IMV_OK)
                {
                    HImage image = new HImage();
                    HImage adjImage = new HImage();
                    switch (this.m_frame.frameInfo.pixelFormat)
                    {
                        case IMVDefine.IMV_EPixelType.gvspPixelMono8:
                            image.GenImage1("byte", (int)this.m_frame.frameInfo.width, (int)this.m_frame.frameInfo.height, this.m_frame.pData);
                            break;
                        case IMVDefine.IMV_EPixelType.gvspPixelRGB8:
                            image.GenImage1("byte", (int)this.m_frame.frameInfo.width * 3, (int)this.m_frame.frameInfo.height * 3, this.m_frame.pData);
                            break;
                    }
                    this.AdjImg(image, out adjImage);  // 调整图像的方向及图像畸变
                    image?.Dispose();
                    this.CameraParam.DataWidth = (int)this.m_frame.frameInfo.width;
                    this.CameraParam.DataHeight = (int)this.m_frame.frameInfo.height;
                    this.camera.IMV_ReleaseFrame(ref this.m_frame); // 这里必需手动释放 
                    imageList.Add(adjImage);
                }
            }
            //////////////////
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
            foreach (var item in imageList)
            {
                item.Dispose();
            }
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
                    //HImage image = null;
                    //int bufferNum = 0;
                    //if (this._imageManage.GetHImage(enAcqMode.异步采集, enImageAcqMethod.明场, this.CameraParam.Timeout, out image, out darkImage, out bufferNum))
                    //{
                    //    this.AdjImg(image, out this._grabImage);
                    //    this.AdjImg(darkImage, out this._grabDarkImage);
                    //    image?.Dispose();
                    //    darkImage?.Dispose();
                    //    if (this._grabImage != null && this._grabImage.IsInitialized())
                    //        this.OnImageAcqComplete(this.Name, new ImageDataClass(this._grabImage, this.CameraParam, this._imageManage.CurImageIndex)); // 异步发送图像出去
                    //    if (this._grabDarkImage != null && this._grabDarkImage.IsInitialized())
                    //        this.OnImageAcqComplete(this.Name, new ImageDataClass(this._grabDarkImage, this.CameraParam, this._imageManage.CurImageIndex)); // 异步发送图像出去
                    //    LoggerHelper.Debug("Buffer绶存数量 = " + bufferNum.ToString());
                    //}
                    Thread.Sleep(50);
                }
            });
        }


        public void ClearData()
        {
            this._imageManage.Init();
        }



    }
}

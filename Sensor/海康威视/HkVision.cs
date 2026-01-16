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
using Common;
using HalconDotNet;
using MvCamCtrl.NET;
using static MvCamCtrl.NET.MyCamera;

namespace Sensor
{
    public class HkVision : SensorBase, ISensor
    {
        private MvCamCtrl.NET.MyCamera camera = null;
        private int res = MyCamera.MV_OK;
        private MvCamCtrl.NET.MyCamera.cbOutputExdelegate frameCallBack;
        private Dictionary<int, ImageByteData> dicImage = new Dictionary<int, ImageByteData>();
        private int _CurrentImageIndex = 0;
        private int m_CurImageIndex = 0; //
        private int dataFrameIndex = 0;
        private int imageBufferCount = 100; // 图像绶存数量即字典的个数
        private int m_FrameNumByImage = 1; // 对于面阵相机，该值等于1
        //private byte[] pSaveDataS;
        //private int _CurrentIndex = 0;
        private Stopwatch stopwatch = new Stopwatch();
        private int ImageIndex = 0;
        private bool IsGrabbing = false;
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
                if (!configParam.IsActive) return result;
                this.cts?.Cancel();
                //if (this.camera != null) return true;
                ////////////////////////////////////////////////
                switch (configParam.ConnectType)
                {
                    /// 海康面阵相机接口
                    case enUserConnectType.DeviceName:
                        result = this.OpenByUserName(this.ConfigParam.ConnectAddress);
                        if (result)
                        {
                            /////////// 设置采集方式
                            this.SetAcqParam();
                        }
                        break;
                    case enUserConnectType.SerialNumber:
                        result = this.OpenBySerialNumber(configParam.ConnectAddress);
                        if (result)
                        {
                            /////////// 设置采集方式
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
                        this.camera = null;
                        result = true;
                        break;
                    default:
                        new UserMessageForm().ShowDialog(configParam.ConnectType.ToString() + "连接类型未实现，请将连接类型设置为：" + enUserConnectType.SerialNumber + "或" + enUserConnectType.DeviceName.ToString() + "依赖于相机名称连接!");
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
                this.cts?.Cancel();
                if (this.camera != null)
                {
                    res = this.camera.MV_CC_StopGrabbing_NET();
                    res = this.camera.MV_CC_CloseDevice_NET();
                    this.IsGrabbing = false;
                    if (res != MvCamCtrl.NET.MyCamera.MV_OK)
                        result = true;
                    else
                        result = false;
                    // 销毁设备句柄
                    // Destroy Device Handle
                    res = this.camera.MV_CC_DestroyDevice_NET();
                    if (res != MvCamCtrl.NET.MyCamera.MV_OK)
                        result = true;
                    else
                        result = false;
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
                this._imageManage = new ImageManage(100);
                this.camera = new MvCamCtrl.NET.MyCamera();
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
                            list.Add(enDataItem.Image, new ImageDataClass(this._grabImage.Clone(), this.CameraParam, this.ImageIndex + 1));
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
                        if (this._grabImage != null && this._grabImage.IsInitialized())
                            list.Add(enDataItem.Image, new ImageDataClass(this._grabImage.Clone(), this.CameraParam)); //this.imageData
                    }
                    break;
            }
            return list;
        }
        public object GetParam(object paramType)
        {
            object value = "";
            try
            {
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
                                MyCamera.MVCC_FLOATVALUE expose1 = new MyCamera.MVCC_FLOATVALUE();
                                res = this.camera.MV_CC_GetExposureTime_NET(ref expose1);
                                value = expose1.fCurValue;
                                break;
                            case "增益":
                                if (camera == null) return value;
                                MyCamera.MVCC_FLOATVALUE Gain = new MyCamera.MVCC_FLOATVALUE();
                                res = this.camera.MV_CC_GetGain_NET(ref Gain);
                                value = Gain.fCurValue;
                                break;
                            case "Image":
                                value = this._grabImage;
                                break;
                        }
                        break;
                }
            }
            catch (Exception e)
            {
                LoggerHelper.Error(this.Name + "曝光获取失败" + e.ToString());
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
                                if (this.camera == null) return result;
                                double expose = 0;
                                double.TryParse(value.ToString(), out expose);
                                res = this.camera.MV_CC_SetExposureTime_NET((float)expose);
                                break;
                            case "增益":
                                if (this.camera == null) return result;
                                double Gain = 0;
                                double.TryParse(value.ToString(), out Gain);
                                res = this.camera.MV_CC_SetGain_NET((float)Gain);
                                break;

                            case "实时采集":
                            case "开始实时采集":
                                if (this.camera == null) return result;
                                res = 0;
                                if (!this.IsLiveState)
                                {
                                    this.IsLiveState = true;
                                    this.cts?.Cancel();
                                    if (this.IsGrabbing)
                                        res = camera.MV_CC_StopGrabbing_NET();
                                    res = this.camera.MV_CC_SetEnumValue_NET("TriggerMode", 0);
                                    res = this.camera.MV_CC_StartGrabbing_NET();
                                    if (res == 0) this.IsGrabbing = true;
                                }
                                if (res == 0) result = true;
                                break;
                            case "停止采集":
                                if (this.camera == null) return result;
                                if (this.IsLiveState)
                                {
                                    this.IsLiveState = false;
                                    if (this.IsGrabbing)
                                        res = camera.MV_CC_StopGrabbing_NET();
                                    this.SetAcqParam(false);
                                }
                                if (res == 0) result = true;
                                break;
                        }
                        break;
                }
            }
            catch (Exception e)
            {
                LoggerHelper.Error(this.Name + "曝光设置失败" + e.ToString());
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
                    result = SensorManage.GetSensor(this._MapName).StartTrigger();
                    if (this._grabImage != null && this._grabImage.IsInitialized())
                        this._grabImage.Dispose();
                    if (this._grabDarkImage != null && this._grabDarkImage.IsInitialized())
                        this._grabDarkImage.Dispose();
                    this._grabImage = (SensorManage.GetSensor(this._MapName)).GrabImage; // 用于远程调用的映射处理
                    this._grabDarkImage = (SensorManage.GetSensor(this._MapName)).GrabDarkImage; // 用于远程调用的映射处理
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
                                    res = camera.MV_CC_StartGrabbing_NET();
                                    if (res == 0) this.IsGrabbing = true;
                                    result = GetImageSyn(out this._grabImage, out this._grabDarkImage);
                                    res = camera.MV_CC_StopGrabbing_NET();
                                    if (res == 0) this.IsGrabbing = false;
                                    break;
                                case enUserTriggerSource.软触发:
                                    if (this._grabImage != null && this._grabImage.IsInitialized())
                                        this._grabImage.Dispose();
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
                                    res = camera.MV_CC_StopGrabbing_NET();
                                    if (res == 0) this.IsGrabbing = false;
                                    this._imageManage?.Init();
                                    this.ImageIndex = 0;
                                    res = camera.MV_CC_StartGrabbing_NET();
                                    if (res == 0) this.IsGrabbing = true;
                                    if (res == 0) result = true;
                                    break;
                                case enUserTriggerSource.软触发: // 实时采集                            
                                    this._imageManage?.Init();
                                    this.ImageIndex = 0;
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
                this.isSaveImage = false;
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
                                        res = camera.MV_CC_StopGrabbing_NET();
                                        if (res == 0) this.IsGrabbing = false;
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
                LoggerHelper.Error(this.Name + ":触发相机失败" + ex);
            }
            return result;
        }


        private bool SetAcqParam(bool initAcq = false)
        {
            bool result = false;
            switch (this.CameraParam.AcqMode)
            {
                case enAcqMode.同步采集:
                    switch (this.CameraParam.TriggerSource)
                    {
                        case enUserTriggerSource.NONE:
                            res = this.camera.MV_CC_SetEnumValue_NET("TriggerMode", 0);
                            break;
                        case enUserTriggerSource.软触发:
                            this.SetSoftwareTrigger();
                            if (!this.IsGrabbing)
                                res = this.camera.MV_CC_StartGrabbing_NET();
                            if (res == 0) this.IsGrabbing = true;
                            break;
                        default:
                            this.SetExternTrigger();
                            if (!this.IsGrabbing)
                                res = this.camera.MV_CC_StartGrabbing_NET();
                            if (res == 0) this.IsGrabbing = true;
                            break;
                    }
                    break;
                case enAcqMode.异步取图:
                    frameCallBack = new MyCamera.cbOutputExdelegate(OnGetFrame);
                    res = this.camera.MV_CC_RegisterImageCallBackEx_NET(frameCallBack, IntPtr.Zero);
                    switch (this.CameraParam.TriggerSource)
                    {
                        case enUserTriggerSource.NONE:
                            res = this.camera.MV_CC_SetEnumValue_NET("TriggerMode", 0);
                            break;
                        case enUserTriggerSource.软触发:
                            this.SetSoftwareTrigger();
                            if (!this.IsGrabbing)
                                res = this.camera.MV_CC_StartGrabbing_NET();
                            if (res == 0) this.IsGrabbing = true;
                            break;
                        default:
                            this.SetExternTrigger();
                            if (!this.IsGrabbing)
                                res = this.camera.MV_CC_StartGrabbing_NET();
                            if (res == 0) this.IsGrabbing = true;
                            break;
                    }
                    if (MyCamera.MV_OK != res) result = false;
                    break;
                case enAcqMode.异步采集: // 通过回调函数来收集图像, 异步触发通常配置外触发来使用
                    frameCallBack = new MyCamera.cbOutputExdelegate(OnGetFrame);
                    res = this.camera.MV_CC_RegisterImageCallBackEx_NET(frameCallBack, IntPtr.Zero);
                    this.GetImageAsynRun();
                    switch (this.CameraParam.TriggerSource)
                    {
                        case enUserTriggerSource.NONE:
                            res = this.camera.MV_CC_SetEnumValue_NET("TriggerMode", 0);
                            break;
                        case enUserTriggerSource.软触发:
                            this.SetSoftwareTrigger();
                            if (!this.IsGrabbing)
                                res = this.camera.MV_CC_StartGrabbing_NET();
                            if (res == 0) this.IsGrabbing = true;
                            break;
                        default:
                            this.SetExternTrigger();
                            if (!this.IsGrabbing)
                                res = this.camera.MV_CC_StartGrabbing_NET();
                            if (res == 0) this.IsGrabbing = true;
                            break;
                    }
                    if (MyCamera.MV_OK != res) result = false;
                    break;
                case enAcqMode.实时采集:
                    res = this.camera.MV_CC_SetEnumValue_NET("TriggerMode", 0);
                    if (initAcq)
                    {
                        res = this.camera.MV_CC_StartGrabbing_NET();
                        if (res == 0)
                            this.GetImageAsynRun();
                    }
                    if (MyCamera.MV_OK != res) result = false;
                    break;
            }
            return result;
        }
        protected bool OpenByUserName(string camName)
        {
            bool result = false;
            MvCamCtrl.NET.MyCamera.MV_CC_DEVICE_INFO_LIST stDevList = new MvCamCtrl.NET.MyCamera.MV_CC_DEVICE_INFO_LIST();
            res = MvCamCtrl.NET.MyCamera.MV_CC_EnumDevices_NET(MvCamCtrl.NET.MyCamera.MV_GIGE_DEVICE | MvCamCtrl.NET.MyCamera.MV_USB_DEVICE, ref stDevList);
            if (res != MvCamCtrl.NET.MyCamera.MV_OK)
            {
                result = false;
            }
            if (stDevList.nDeviceNum < 1)
            {
                result = false;
            }
            MvCamCtrl.NET.MyCamera.MV_CC_DEVICE_INFO stDevInfo;
            string userDefinedName = "";
            for (int i = 0; i < stDevList.nDeviceNum; i++)
            {
                stDevInfo = (MvCamCtrl.NET.MyCamera.MV_CC_DEVICE_INFO)Marshal.PtrToStructure(stDevList.pDeviceInfo[i], typeof(MvCamCtrl.NET.MyCamera.MV_CC_DEVICE_INFO));
                switch (stDevInfo.nTLayerType)
                {
                    case MvCamCtrl.NET.MyCamera.MV_GIGE_DEVICE:
                        MvCamCtrl.NET.MyCamera.MV_GIGE_DEVICE_INFO stGigEDeviceInfo = (MvCamCtrl.NET.MyCamera.MV_GIGE_DEVICE_INFO)MvCamCtrl.NET.MyCamera.ByteToStruct(stDevInfo.SpecialInfo.stGigEInfo, typeof(MvCamCtrl.NET.MyCamera.MV_GIGE_DEVICE_INFO));
                        userDefinedName = stGigEDeviceInfo.chUserDefinedName;
                        break;
                    case MvCamCtrl.NET.MyCamera.MV_USB_DEVICE:
                        MvCamCtrl.NET.MyCamera.MV_USB3_DEVICE_INFO stUsb3DeviceInfo = (MvCamCtrl.NET.MyCamera.MV_USB3_DEVICE_INFO)MvCamCtrl.NET.MyCamera.ByteToStruct(stDevInfo.SpecialInfo.stUsb3VInfo, typeof(MvCamCtrl.NET.MyCamera.MV_USB3_DEVICE_INFO));
                        userDefinedName = stUsb3DeviceInfo.chUserDefinedName;
                        break;
                }
                //stDevInfo = (MvCamCtrl.NET.MyCamera.MV_CC_DEVICE_INFO)Marshal.PtrToStructure(stDevList.pDeviceInfo[i], typeof(MvCamCtrl.NET.MyCamera.MV_CC_DEVICE_INFO));
                if (userDefinedName == camName)
                {
                    // ch:创建设备 | en:Create device
                    res = this.camera.MV_CC_CreateDevice_NET(ref stDevInfo);
                    if (MvCamCtrl.NET.MyCamera.MV_OK != res)
                    {
                        return result;
                    }
                    // ch:打开设备 | en:Open device
                    res = this.camera.MV_CC_OpenDevice_NET();
                    if (MvCamCtrl.NET.MyCamera.MV_OK != res)
                    {
                        return result;
                    }
                    // ch:探测网络最佳包大小(只对GigE相机有效) | en:Detection network optimal package size(It only works for the GigE camera)
                    if (stDevInfo.nTLayerType == MvCamCtrl.NET.MyCamera.MV_GIGE_DEVICE)
                    {
                        int nPacketSize = this.camera.MV_CC_GetOptimalPacketSize_NET();
                        if (nPacketSize > 0)
                        {
                            res = this.camera.MV_CC_SetIntValue_NET("GevSCPSPacketSize", (uint)nPacketSize);
                            if (res != MvCamCtrl.NET.MyCamera.MV_OK)
                            {
                                return result;
                            }
                        }
                        else
                        {
                            return result;
                        }
                        result = true;
                    }
                    ///////////////////////////////////
                    return result;
                }
            }
            return result;
        }
        protected bool OpenBySerialNumber(string serialNumber)
        {
            bool result = false;
            MvCamCtrl.NET.MyCamera.MV_CC_DEVICE_INFO_LIST stDevList = new MvCamCtrl.NET.MyCamera.MV_CC_DEVICE_INFO_LIST();
            res = MvCamCtrl.NET.MyCamera.MV_CC_EnumDevices_NET(MvCamCtrl.NET.MyCamera.MV_GIGE_DEVICE | MvCamCtrl.NET.MyCamera.MV_USB_DEVICE, ref stDevList);
            if (res != MvCamCtrl.NET.MyCamera.MV_OK)
            {
                result = false;
            }
            if (stDevList.nDeviceNum < 1)
            {
                result = false;
            }
            MvCamCtrl.NET.MyCamera.MV_CC_DEVICE_INFO stDevInfo;
            string userDefinedName = "";
            for (int i = 0; i < stDevList.nDeviceNum; i++)
            {
                stDevInfo = (MvCamCtrl.NET.MyCamera.MV_CC_DEVICE_INFO)Marshal.PtrToStructure(stDevList.pDeviceInfo[i], typeof(MvCamCtrl.NET.MyCamera.MV_CC_DEVICE_INFO));
                switch (stDevInfo.nTLayerType)
                {
                    case MvCamCtrl.NET.MyCamera.MV_GIGE_DEVICE:
                        MvCamCtrl.NET.MyCamera.MV_GIGE_DEVICE_INFO stGigEDeviceInfo = (MvCamCtrl.NET.MyCamera.MV_GIGE_DEVICE_INFO)MvCamCtrl.NET.MyCamera.ByteToStruct(stDevInfo.SpecialInfo.stGigEInfo, typeof(MvCamCtrl.NET.MyCamera.MV_GIGE_DEVICE_INFO));
                        userDefinedName = stGigEDeviceInfo.chSerialNumber;
                        break;
                    case MvCamCtrl.NET.MyCamera.MV_USB_DEVICE:
                        MvCamCtrl.NET.MyCamera.MV_USB3_DEVICE_INFO stUsb3DeviceInfo = (MvCamCtrl.NET.MyCamera.MV_USB3_DEVICE_INFO)MvCamCtrl.NET.MyCamera.ByteToStruct(stDevInfo.SpecialInfo.stUsb3VInfo, typeof(MvCamCtrl.NET.MyCamera.MV_USB3_DEVICE_INFO));
                        userDefinedName = stUsb3DeviceInfo.chSerialNumber;
                        break;
                }
                //stDevInfo = (MvCamCtrl.NET.MyCamera.MV_CC_DEVICE_INFO)Marshal.PtrToStructure(stDevList.pDeviceInfo[i], typeof(MvCamCtrl.NET.MyCamera.MV_CC_DEVICE_INFO));
                if (userDefinedName == serialNumber)
                {
                    // ch:创建设备 | en:Create device
                    res = this.camera.MV_CC_CreateDevice_NET(ref stDevInfo);
                    if (MvCamCtrl.NET.MyCamera.MV_OK != res)
                    {
                        return result;
                    }
                    // ch:打开设备 | en:Open device
                    res = this.camera.MV_CC_OpenDevice_NET();
                    if (MvCamCtrl.NET.MyCamera.MV_OK != res)
                    {
                        return result;
                    }
                    // ch:探测网络最佳包大小(只对GigE相机有效) | en:Detection network optimal package size(It only works for the GigE camera)
                    if (stDevInfo.nTLayerType == MvCamCtrl.NET.MyCamera.MV_GIGE_DEVICE)
                    {
                        int nPacketSize = this.camera.MV_CC_GetOptimalPacketSize_NET();
                        if (nPacketSize > 0)
                        {
                            res = this.camera.MV_CC_SetIntValue_NET("GevSCPSPacketSize", (uint)nPacketSize);
                            if (res != MvCamCtrl.NET.MyCamera.MV_OK)
                            {
                                return result;
                            }
                        }
                        else
                        {
                            return result;
                        }
                        result = true;
                    }
                    ///////////////////////////////////
                    return result;
                }
            }
            return result;
        }
        protected void SetExternTrigger()
        {
            try
            {
                res = this.camera.MV_CC_SetTriggerSource_NET(1); // 1：表示硬触发？
                res = this.camera.MV_CC_SetEnumValue_NET("TriggerMode", 1);
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
                int res = this.camera.MV_CC_SetTriggerSource_NET(0);
                if (res != MvCamCtrl.NET.MyCamera.MV_OK)
                    result = true;
                else
                    result = false;
                // 设置触发模式 
                // Set trigger mode to On 
                res = this.camera.MV_CC_SetEnumValue_NET("TriggerMode", 1);
                if (res != MvCamCtrl.NET.MyCamera.MV_OK)
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
                res = this.camera.MV_CC_TriggerSoftwareExecute_NET();
                if (res != MvCamCtrl.NET.MyCamera.MV_OK)
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
        protected void OnGetFrame(IntPtr pData, ref MvCamCtrl.NET.MyCamera.MV_FRAME_OUT_INFO_EX frame, IntPtr pUser)
        {
            if (pData != IntPtr.Zero)
            {
                this.CameraParam.DataWidth = (int)frame.nWidth;
                this.CameraParam.DataHeight = (int)frame.nHeight; // 重置参数
                this._imageManage.FramWidth = (int)frame.nWidth;
                this._imageManage.FramHeight = (int)frame.nHeight;
                switch (frame.enPixelType)
                {
                    case MvGvspPixelType.PixelType_Gvsp_Mono8:
                        this._imageManage.AddImage(pData, (int)frame.nWidth, (int)frame.nHeight, 1, enPixFormat.Mono8);
                        break;
                    case MvGvspPixelType.PixelType_Gvsp_RGB8_Planar:
                        this._imageManage.AddImage(pData, (int)frame.nWidth, (int)frame.nHeight * 3, 1, enPixFormat.RGB8);
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
            MvCamCtrl.NET.MyCamera.MV_FRAME_OUT m_frame = new MvCamCtrl.NET.MyCamera.MV_FRAME_OUT();
            List<HImage> imageList = new List<HImage>();
            for (int i = 0; i < this.CameraParam.AverangeCount; i++)
            {
                res = this.camera.MV_CC_GetImageBuffer_NET(ref m_frame, this.CameraParam.Timeout);
                if (res == MvCamCtrl.NET.MyCamera.MV_OK)
                {
                    HImage image = new HImage();
                    HImage adjImage = new HImage();
                    switch (m_frame.stFrameInfo.enPixelType)
                    {
                        case MvCamCtrl.NET.MyCamera.MvGvspPixelType.PixelType_Gvsp_Mono8:
                            image.GenImage1("byte", (int)m_frame.stFrameInfo.nWidth, (int)m_frame.stFrameInfo.nHeight, m_frame.pBufAddr);
                            break;
                        case MvCamCtrl.NET.MyCamera.MvGvspPixelType.PixelType_Gvsp_RGB8_Planar:
                            image.GenImage1("byte", (int)m_frame.stFrameInfo.nWidth * 3, (int)m_frame.stFrameInfo.nHeight * 3, m_frame.pBufAddr);
                            break;
                    }
                    this.AdjImg(image, out adjImage);  // 调整图像的方向及图像畸变
                    image?.Dispose();
                    this.CameraParam.DataWidth = (int)m_frame.stFrameInfo.nWidth;
                    this.CameraParam.DataHeight = (int)m_frame.stFrameInfo.nHeight;
                    this.camera.MV_CC_FreeImageBuffer_NET(ref m_frame); // 这里必需手动释放 
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

        public void ClearData()
        {
            this._imageManage?.Init();
        }

        protected override void GetImageAsynRun()
        {
            this.cts.Cancel();
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
                    //        this.OnImageAcqComplete(this.Name, new ImageDataClass(this._grabImage, this.CameraParam, this._imageManage.CurImageIndex)); // 异步发送图像出去
                    //    if (this._grabDarkImage != null && this._grabDarkImage.IsInitialized())
                    //        this.OnImageAcqComplete(this.Name, new ImageDataClass(this._grabDarkImage, this.CameraParam, this._imageManage.CurImageIndex)); // 异步发送图像出去
                    //    LoggerHelper.Debug("Buffer绶存数量 = " + bufferNum.ToString());
                    //}
                    Thread.Sleep(50);
                }
            });
        }



    }
}

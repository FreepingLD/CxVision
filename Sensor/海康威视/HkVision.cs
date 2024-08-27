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

namespace Sensor
{
    public class HkVision : SensorBase, ISensor
    {
        private MvCamCtrl.NET.MyCamera camera;
        private int res = MyCamera.MV_OK;
        private MvCamCtrl.NET.MyCamera.cbOutputExdelegate frameCallBack;
        private ConcurrentQueue<byte[]> framDataList = new ConcurrentQueue<byte[]>();
        private byte[] pSaveDataS;
        //private int _CurrentIndex = 0;
        private Stopwatch stopwatch = new Stopwatch();

        public bool Connect(SensorConnectConfigParam configParam) // 传入的名称
        {
            bool result = false;
            try
            {
                this.ConfigParam = configParam;
                this.Name = configParam.SensorName;
                this.CameraParam = (CameraParam)new CameraParam().Read(configParam.SensorName);
                this.CameraParam.SensorName = configParam.SensorName;
                if (!configParam.IsActive) return result;
                ///////////////////////////////////
                switch (configParam.ConnectType)
                {
                    /// 海康面阵相机接口
                    case enUserConnectType.DeviceName:
                        result = this.OpenByUserName(this.Name);
                        if (result)
                        {
                            switch (this.CameraParam.TriggerSource)
                            {
                                case enUserTriggerSource.NONE:
                                    res = this.camera.MV_CC_SetEnumValue_NET("TriggerMode", 0);
                                    break;
                                case enUserTriggerSource.软触发:
                                    SetSoftwareTrigger();
                                    break;
                                default:
                                    SetExternTrigger();
                                    break;
                            }
                            // 设置采集方式
                            switch (this.CameraParam.AcqMode)
                            {
                                case enAcqMode.同步采集:
                                    break;
                                case enAcqMode.异步采集: // 通过回调函数来收集图像, 异步触发通常配置外触发来使用
                                    frameCallBack = new MyCamera.cbOutputExdelegate(OnGetFrame);
                                    res = this.camera.MV_CC_RegisterImageCallBackEx_NET(frameCallBack, IntPtr.Zero);
                                    if (MyCamera.MV_OK != res)
                                    {
                                        result = false;
                                    }
                                    break;
                            }
                        }
                        break;
                    case enUserConnectType.SerialNumber:
                        result = this.OpenBySerialNumber(configParam.ConnectAddress);
                        if (result)
                        {
                            switch (this.CameraParam.TriggerSource)
                            {
                                case enUserTriggerSource.NONE:
                                    res = this.camera.MV_CC_SetEnumValue_NET("TriggerMode", 0);
                                    break;
                                case enUserTriggerSource.软触发:
                                    SetSoftwareTrigger();
                                    break;
                                default:
                                    SetExternTrigger();
                                    break;
                            }
                            // 设置采集方式
                            switch (this.CameraParam.AcqMode)
                            {
                                case enAcqMode.同步采集:
                                    break;
                                case enAcqMode.异步采集: // 通过回调函数来收集图像, 异步触发通常配置外触发来使用
                                    frameCallBack = new MyCamera.cbOutputExdelegate(OnGetFrame);
                                    res = this.camera.MV_CC_RegisterImageCallBackEx_NET(frameCallBack, IntPtr.Zero);
                                    if (MyCamera.MV_OK != res)
                                    {
                                        result = false;
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
                    default:
                        MessageBox.Show(configParam.ConnectType.ToString() + "连接类型未实现，请将连接类型设置为：" + enUserConnectType.SerialNumber + "或" + enUserConnectType.DeviceName.ToString() +
                            "依赖于相机名称连接!");
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
                    res = this.camera.MV_CC_StopGrabbing_NET();
                    res = this.camera.MV_CC_CloseDevice_NET();
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
            if (this._grabImage != null && this._grabImage.IsInitialized())
                list.Add(enDataItem.Image, new ImageDataClass(this._grabImage.Clone(), this.CameraParam)); //this.imageData
            return list;
        }
        public object GetParam(object paramType)
        {
            object value = "";
            try
            {
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
            }
            catch (Exception e)
            {

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
                };
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
                            switch (this.CameraParam.AcqMode)
                            {
                                case enAcqMode.同步采集:
                                    res = camera.MV_CC_StartGrabbing_NET();
                                    result = GetImageSyn(out this._grabImage, out this._grabDarkImage);
                                    res = camera.MV_CC_StopGrabbing_NET();
                                    break;
                                case enAcqMode.异步采集: // 通过回调函数来收集图像, 异步触发通常配置外触发来使用;从绶存中来获取数据
                                    result = GetImageAsyn(out this._grabImage, out this._grabDarkImage);
                                    break;
                            }
                            break;
                        case enUserTriggerSource.外部IO触发:
                        case enUserTriggerSource.内部IO触发:
                        case enUserTriggerSource.编码器触发:
                            switch (this.CameraParam.AcqMode)
                            {
                                case enAcqMode.同步采集:
                                    res = camera.MV_CC_StartGrabbing_NET();
                                    result = GetImageSyn(out this._grabImage, out this._grabDarkImage);
                                    res = camera.MV_CC_StopGrabbing_NET();
                                    break;
                                case enAcqMode.异步采集: // 通过回调函数来收集图像, 异步触发通常配置外触发来使用;
                                    this.isSaveImage = true;
                                    result = GetImageAsyn(out this._grabImage, out this._grabDarkImage);
                                    break;
                            }
                            break;
                        case enUserTriggerSource.软触发:
                            switch (this.CameraParam.AcqMode)
                            {
                                case enAcqMode.同步采集:
                                    res = camera.MV_CC_StartGrabbing_NET();
                                    this.SendSoftwareExecute();// 软触发
                                    result = GetImageSyn(out this._grabImage, out this._grabDarkImage);
                                    res = camera.MV_CC_StopGrabbing_NET();
                                    break;
                                case enAcqMode.异步采集: // 通过回调函数来收集图像, 异步触发通常配置外触发来使用;
                                    this.isSaveImage = true;
                                    result = GetImageAsyn(out this._grabImage, out this._grabDarkImage);
                                    break;
                            }
                            break;
                    }
                    if (result && this._grabImage.IsInitialized())
                        LoggerHelper.Info(this.CameraParam.SensorName + "图像采集成功");
                    else
                        LoggerHelper.Info(this.CameraParam.SensorName + "图像采集失败");
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
                this.isSaveImage = false;
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
                //res = this.camera.MV_CAML_SetEnumFeatureSymbol("TriggerMode", "On");
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
                this.pSaveDataS = new byte[frame.nWidth * frame.nHeight];
                this.CameraParam.DataWidth = (int)frame.nWidth;
                this.CameraParam.DataHeight = (int)frame.nHeight; // 重置参数
                Marshal.Copy(pData, this.pSaveDataS, 0, this.pSaveDataS.Length);
                // 将数据添加到绶存中
                if (this.framDataList.Count < 10) // 最大限制100的绶存
                    this.framDataList.Enqueue(this.pSaveDataS);
                else
                {
                    byte[] value;
                    this.framDataList.TryDequeue(out value);
                    this.framDataList.Enqueue(this.pSaveDataS);
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
            hImage = new HImage();
            darkImage = new HImage();
            this.stopwatch.Restart();
            while (true)
            {
                if (this.framDataList.Count > 0) break; // 如果绶存中数据足够，则停止 
                if (this.stopwatch.ElapsedMilliseconds > this.CameraParam.Timeout) return false; // 如果绶存中数据不够，但时间已达到指定时间，则停止
            }
            this.stopwatch.Stop();
            byte[] data = new byte[0];            // 
            for (int i = 0; i < 10; i++)
            {
                if (this.framDataList.TryDequeue(out data))
                {
                    break;
                }
            }
            /////////////////////////////////////
            HImage image = new HImage();
            IntPtr pixPtr = IntPtr.Zero;
            pixPtr = Marshal.AllocHGlobal(data.Length);
            Marshal.Copy(data, 0, pixPtr, data.Length);
            image.GenImage1("byte", this.CameraParam.DataWidth, this.CameraParam.DataHeight, pixPtr);
            Marshal.FreeHGlobal(pixPtr);
            this.AdjImg(image, out this._grabImage);
            image?.Dispose();
            result = true;
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
            byte[] value;
            while (true)
            {
                this.framDataList.TryDequeue(out value);
            }
        }



    }
}

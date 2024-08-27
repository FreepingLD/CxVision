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
using MVSDK_Net;

namespace Sensor
{
    public class IMvCam : SensorBase, ISensor
    {
        private IMVDefine.IMV_Frame m_frame;
        private MyCamera camera;
        private int res = IMVDefine.IMV_OK;
        private IMVDefine.IMV_FrameCallBack frameCallBack;
        private ConcurrentQueue<byte[]> framDataList = new ConcurrentQueue<byte[]>();
        private byte[] pSaveDataS;
        //private object lockSyn = new object();
        private int _CurrentImageIndex = 0;
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
                    /// 大华面阵相机接口
                    case enUserConnectType.DeviceName:
                        result = this.OpenByDeviceUserID(this.Name);
                        switch (this.CameraParam.TriggerSource)
                        {
                            case enUserTriggerSource.NONE:
                                // 设置触发模式
                                res = this.camera.IMV_SetEnumFeatureSymbol("TriggerMode", "Off");
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
                                frameCallBack = OnGetFrame;
                                res = camera.IMV_AttachGrabbing(frameCallBack, IntPtr.Zero);
                                res = camera.IMV_StartGrabbing();
                                break;
                        }
                        break;
                    case enUserConnectType.TcpIp:
                    case enUserConnectType.Network:
                        result = this.OpenByIPAddress(configParam.ConnectAddress);
                        switch (this.CameraParam.TriggerSource)
                        {
                            case enUserTriggerSource.NONE:
                                // 设置触发模式
                                res = this.camera.IMV_SetEnumFeatureSymbol("TriggerMode", "Off");
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
                                frameCallBack = OnGetFrame;
                                res = camera.IMV_AttachGrabbing(frameCallBack, IntPtr.Zero);
                                res = camera.IMV_StartGrabbing();
                                break;
                        }
                        break;
                    case enUserConnectType.SerialNumber:
                        result = this.OpenByCameraKey(configParam.ConnectAddress);
                        switch (this.CameraParam.TriggerSource)
                        {
                            case enUserTriggerSource.NONE:
                                // 设置触发模式
                                res = this.camera.IMV_SetEnumFeatureSymbol("TriggerMode", "Off");
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
                                frameCallBack = OnGetFrame;
                                res = camera.IMV_AttachGrabbing(frameCallBack, IntPtr.Zero);
                                res = camera.IMV_StartGrabbing();
                                break;
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
                        MessageBox.Show(configParam.ConnectType.ToString() + "连接类型未实现，请将连接类型设置为：" + enUserConnectType.DeviceName.ToString() + "或" + enUserConnectType.SerialNumber);
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
                LoggerHelper.Error(this.CameraParam.SensorName + "关闭相机失败", ex);
                result = false;
            }
            return result;
        }

        public bool Init()
        {
            try
            {
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
            if (this._grabImage != null && this._grabImage.IsInitialized())
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
                    double expose1 = 0;
                    res = this.camera.IMV_GetDoubleFeatureValue("ExposureTime", ref expose1);
                    value = expose1;
                    break;
                case "增益":
                    if (camera == null) return value;
                    double Gain = 0;
                    res = this.camera.IMV_GetDoubleFeatureValue("Gain", ref Gain);
                    value = Gain;
                    break;
                case "Image":
                    value = this._grabImage;
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
                        if (this.camera == null) return result;
                        double expose = 0;
                        double.TryParse(value.ToString(), out expose);
                        res = this.camera.IMV_SetDoubleFeatureValue("ExposureTime", expose);
                        break;
                    case "增益":
                        if (this.camera == null) return result;
                        double Gain = 0;
                        double.TryParse(value.ToString(), out Gain);
                        res = this.camera.IMV_SetDoubleFeatureValue("Gain", Gain);
                        break;
                    case "清空数据":
                    case "重置图像采集":
                    case "启动采集":
                        this._CurrentImageIndex = 0;
                        this.camera.IMV_StopGrabbing();
                        this.ClearData();
                        this.camera.IMV_StartGrabbing();
                        break;
                    case "停止采集":
                        this.camera.IMV_StopGrabbing();
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
                    this._grabImage = (SensorManage.GetSensor(this._MapName)).GrabImage; ; // 用于远程调用的映射处理
                    this._grabDarkImage = (SensorManage.GetSensor(this._MapName)).GrabDarkImage; ; // 用于远程调用的映射处理
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
                                    res = camera.IMV_StartGrabbing();
                                    result = GetImageSyn(out this._grabImage, out this._grabDarkImage);
                                    res = camera.IMV_StopGrabbing();
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
                                    res = camera.IMV_StartGrabbing();
                                    result = GetImageSyn(out this._grabImage, out this._grabDarkImage);
                                    res = camera.IMV_StopGrabbing();
                                    break;
                                case enAcqMode.异步采集: // 通过回调函数来收集图像, 异步触发通常配置外触发来使用;
                                    result = GetImageAsyn(out this._grabImage, out this._grabDarkImage);
                                    break;
                            }
                            break;
                        case enUserTriggerSource.软触发:
                            switch (this.CameraParam.AcqMode)
                            {
                                case enAcqMode.同步采集:
                                    res = camera.IMV_StartGrabbing();
                                    this.SendSoftwareExecute();// 软触发
                                    result = GetImageSyn(out this._grabImage, out this._grabDarkImage);
                                    res = camera.IMV_StopGrabbing();
                                    break;
                                case enAcqMode.异步采集: // 通过回调函数来收集图像, 异步触发通常配置外触发来使用;
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
                switch (this.ConfigParam.ConnectType)
                {
                    case enUserConnectType.Map:
                        SensorManage.GetSensor(this._MapName).StopTrigger();
                        //if (this._grabImage != null && this._grabImage.IsInitialized())
                        //    this._grabImage.Dispose();
                        //this._grabImage = (SensorManage.GetSensor(this._MapName)).GrabImage; ; // 用于远程调用的映射处理
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
                this.pSaveDataS = new byte[frame.frameInfo.width * frame.frameInfo.height];
                this.CameraParam.DataWidth = (int)frame.frameInfo.width;
                this.CameraParam.DataHeight = (int)frame.frameInfo.height; // 重置参数
                Marshal.Copy(frame.pData, this.pSaveDataS, 0, this.pSaveDataS.Length);
                this.camera.IMV_ReleaseFrame(ref m_frame); // 这里必需手动释放 
                // 将数据添加到绶存中
                if (this.framDataList.Count < 100) // 最大限制100的绶存
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
                if (this.framDataList.TryDequeue(out data)) break;
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
            this._CurrentImageIndex++;
            if (this._CurrentImageIndex > int.MaxValue)
                this._CurrentImageIndex = 0;
            LoggerHelper.Info("异步采集当前图片 = " + this._CurrentImageIndex.ToString());
            result = true;
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
                res = this.camera.IMV_GetFrame(ref m_frame, (uint)this.CameraParam.Timeout);
                if (res == IMVDefine.IMV_OK)
                {
                    HImage image = new HImage();
                    HImage adjImage = new HImage();
                    switch (m_frame.frameInfo.pixelFormat)
                    {
                        case IMVDefine.IMV_EPixelType.gvspPixelMono8:
                            image.GenImage1("byte", (int)m_frame.frameInfo.width, (int)m_frame.frameInfo.height, m_frame.pData);
                            break;
                        case IMVDefine.IMV_EPixelType.gvspPixelRGB8:
                            image.GenImage1("byte", (int)m_frame.frameInfo.width * 3, (int)m_frame.frameInfo.height * 3, m_frame.pData);
                            break;
                    }
                    this.AdjImg(image, out adjImage);  // 调整图像的方向及图像畸变
                    image?.Dispose();
                    this.CameraParam.DataWidth = (int)m_frame.frameInfo.width;
                    this.CameraParam.DataHeight = (int)m_frame.frameInfo.height;
                    this.camera.IMV_ReleaseFrame(ref m_frame); // 这里必需手动释放 
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

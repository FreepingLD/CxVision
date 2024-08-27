
using Common;
using HalconDotNet;
using MVSDK;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using CameraHandle = System.Int32;
using MvApi = MVSDK.MvApi;

namespace Sensor
{
    public class MdvsCamera : SensorBase, ISensor
    {
        private CameraHandle m_hCamera = 0;             // 句柄
        private IntPtr m_ImageBuffer;             // 预览通道RGB图像缓存
        private IntPtr ptrPix = IntPtr.Zero;
        private tSdkCameraCapbility tCameraCapability;  // 相机特性描述
        protected IntPtr m_iCaptureCallbackCtx;     //图像回调函数的上下文参数
        //private HImage _grabImage;
        private int _CurrentImageIndex = 0;
        private Stopwatch stopwatch = new Stopwatch();
        private ConcurrentQueue<byte[]> framDataList = new ConcurrentQueue<byte[]>();
        private byte[] pSaveDataS;

        public bool Connect(SensorConnectConfigParam configParam)
        {
            bool result = true;
            try
            {
                this.ConfigParam = configParam;
                this.Name = configParam.SensorName;
                this.CameraParam = (CameraParam)new CameraParam().Read(configParam.SensorName);
                this.CameraParam.SensorName = configParam.SensorName;
                switch (configParam.ConnectType)
                {
                    case enUserConnectType.SerialNumber:
                        CameraSdkStatus status;
                        tSdkCameraDevInfo[] tCameraDevInfoList;
                        if (m_hCamera > 0)
                            result = true;
                        //////////////////////////////////////
                        status = MvApi.CameraEnumerateDevice(out tCameraDevInfoList);
                        if (status == CameraSdkStatus.CAMERA_STATUS_SUCCESS)
                        {
                            if (tCameraDevInfoList != null)//此时iCameraCounts返回了实际连接的相机个数。如果大于1，则初始化第一个相机
                            {
                                status = MvApi.CameraInit(ref tCameraDevInfoList[0], -1, -1, ref m_hCamera);
                                if (status == CameraSdkStatus.CAMERA_STATUS_SUCCESS)
                                {
                                    //获得相机特性描述
                                    MvApi.CameraGetCapability(m_hCamera, out tCameraCapability);
                                    this.m_ImageBuffer = Marshal.AllocHGlobal(tCameraCapability.sResolutionRange.iWidthMax * tCameraCapability.sResolutionRange.iHeightMax * 1 + 1024);
                                    if (tCameraCapability.sIspCapacity.bMonoSensor != 0)
                                    {
                                        // 黑白相机输出8位灰度数据
                                        MvApi.CameraSetIspOutFormat(m_hCamera, (uint)MVSDK.emImageFormat.CAMERA_MEDIA_TYPE_MONO8);
                                    }
                                    //m_CaptureCallback = new CAMERA_SNAP_PROC(ImageCaptureCallback);
                                    //MvApi.CameraSetCallbackFunction(m_hCamera, m_CaptureCallback, m_iCaptureCallbackCtx, ref pCaptureCallOld);
                                    // 开始采集
                                    MvApi.CameraPlay(m_hCamera);
                                    result = true;
                                }
                                else
                                {
                                    m_hCamera = 0;
                                    String errstr = string.Format("相机初始化错误，错误码{0},错误原因是", status);
                                    String errstring = MvApi.CameraGetErrorString(status);
                                    MessageBox.Show(errstr + errstring, "ERROR");
                                    // Environment.Exit(0);
                                    result = false;
                                }
                            }
                        }
                        else
                        {
                            //MessageBox.Show("没有找到相机，如果已经接上相机，可能是权限不够，请尝试使用管理员权限运行程序。");
                            result = false;
                        }
                        // result = false;
                        break;
                    /////////////////////////////////////
                    case enUserConnectType.Map:
                        this._MapName = configParam.ConnectAddress;
                        if (SensorManage.GetSensor(configParam.ConnectAddress) != null &&
                            SensorManage.GetSensor(configParam.ConnectAddress).ConfigParam.ConnectState)  // 只有在映射源找开成功的情总下，映射对象才能打开成功
                            result = true;
                        else
                            result = false;
                        break;
                    default:
                        MessageBox.Show(configParam.ConnectType.ToString() + "连接类型未实现，请将连接类型设置为：" + enUserConnectType.SerialNumber + "并指定相机序列号和相机名称!");
                        result = false;
                        break;
                }
            }
            catch
            {
                result = false;
            }
            configParam.ConnectState = result;
            return result;
        }

        public bool Disconnect()
        {
            if (m_hCamera > 0)
            {
                MvApi.CameraUnInit(m_hCamera);
                Marshal.FreeHGlobal(m_ImageBuffer);
                m_hCamera = 0;
                return true;
            }
            return false;
        }

        public object GetParam(object paramType)
        {
            enSensorParamType type;
            if (paramType is enSensorParamType)
                type = (enSensorParamType)paramType;
            else
                Enum.TryParse(paramType.ToString(), out type);
            switch (type)
            {
                case enSensorParamType.Coom_传感器名称:
                    return this.CameraParam.SensorName;
                case enSensorParamType.Coom_相机内参:
                    return this.CameraParam.CamParam;
                case enSensorParamType.Coom_相机外参:
                    return this.CameraParam.CamPose;
                case enSensorParamType.Coom_每线点数:
                    return 0;
                case enSensorParamType.Coom_相机角度:
                    return this.CameraParam.CamSlant;
                case enSensorParamType.MindVision_相机句柄:
                    return this.m_hCamera;
                case enSensorParamType.Coom_传感器类型:
                    return SensorConnectConfigParamManger.Instance.GetSensorConfigParam(this.Name).SensorType;
                default:
                    return this.CameraParam.SensorName;
            }
        }

        public bool Init()
        {
            CameraSdkStatus status;
            tSdkCameraDevInfo[] tCameraDevInfoList;
            status = MvApi.CameraEnumerateDevice(out tCameraDevInfoList);
            if (status == CameraSdkStatus.CAMERA_STATUS_SUCCESS)
                return true;
            else
                return false;
        }

        public Dictionary<enDataItem, object> ReadData()
        {
            Dictionary<enDataItem, object> list = new Dictionary<enDataItem, object>();
            list.Add(enDataItem.Image, new ImageDataClass(this._grabImage, this.CameraParam));
            list.Add(enDataItem.DarkImage, new ImageDataClass(null));
            return list;
        }

        public bool SetParam(object paramType, object value)
        {
            enSensorParamType type;
            if (paramType is enSensorParamType)
                type = (enSensorParamType)paramType;
            else
                Enum.TryParse(paramType.ToString(), out type);
            switch (type)
            {
                case enSensorParamType.Coom_相机内参:
                    this.CameraParam.CamParam = new userCamParam((HTuple)value);
                    return true;
                case enSensorParamType.Coom_相机外参:
                    this.CameraParam.CamPose = new userCamPose((HTuple)value);
                    return true;
                case enSensorParamType.Coom_相机角度:
                    this.CameraParam.CamSlant = (double)value;
                    return true;
                default:
                    return true;
            }
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
                    this._grabImage = (SensorManage.GetSensor(this._MapName)).GrabImage; ; // 用于远程调用的映射处理
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
                                    result = GetImageSyn(out this._grabImage, out this._grabDarkImage);
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
                                    result = GetImageSyn(out this._grabImage, out this._grabDarkImage);
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
                                    //this.SendSoftwareExecute();// 软触发
                                    result = GetImageSyn(out this._grabImage, out this._grabDarkImage);
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
            }
            catch (Exception ex)
            {
                result = false;
                LoggerHelper.Error(this.Name + ":触发相机失败" + ex);
            }
            return result;
        }

        public void ImageCaptureCallback(CameraHandle hCamera, IntPtr pFrameBuffer, ref tSdkFrameHead pFrameHead, IntPtr pContext)
        {
            this.streamProcessState = true; // 表示开始处理流数据　，不要在回调函数里做太多耗时的事情
            try
            {
                //图像处理，将原始输出转换为RGB格式的位图数据，同时叠加白平衡、饱和度、LUT等ISP处理。 pfnCameraImageProcessEx
                MvApi.CameraImageProcessEx(hCamera, pFrameBuffer, m_ImageBuffer, ref pFrameHead, (uint)MVSDK.emImageFormat.CAMERA_MEDIA_TYPE_MONO8, 0);
                this.pSaveDataS = new byte[pFrameHead.iWidth * pFrameHead.iHeight];
                this.CameraParam.DataWidth = (int)pFrameHead.iWidth;
                this.CameraParam.DataHeight = (int)pFrameHead.iHeight; // 重置参数
                Marshal.Copy(m_ImageBuffer, this.pSaveDataS, 0, this.pSaveDataS.Length);
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
            catch
            {
            }
            this.streamProcessState = false; // 表示结束处理流数据
        }

        private HImage GrabImage1()
        {
            int width = 0, height = 0;
            HImage image = new HImage();
            if (MvApi.CameraSoftTriggerEx(m_hCamera, 1) == CameraSdkStatus.CAMERA_STATUS_SUCCESS)
            {
                //图像处理，将原始输出转换为RGB格式的位图数据，同时叠加白平衡、饱和度、LUT等ISP处理。 pfnCameraImageProcessEx
                ptrPix = MvApi.CameraGetImageBufferEx(this.m_hCamera, ref width, ref height, (uint)this.CameraParam.Timeout);
                image.GenImage1("byte", width, height, ptrPix);
                if (ptrPix != IntPtr.Zero)
                    Marshal.FreeHGlobal(ptrPix);
            }
            return image;
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
            int width = 0, height = 0;
            bool result = true;
            List<HImage> imageList = new List<HImage>();
            for (int i = 0; i < this.CameraParam.AverangeCount; i++)
            {
                if (MvApi.CameraSoftTriggerEx(m_hCamera, 1) == CameraSdkStatus.CAMERA_STATUS_SUCCESS)
                {
                    //图像处理，将原始输出转换为RGB格式的位图数据，同时叠加白平衡、饱和度、LUT等ISP处理。 pfnCameraImageProcessEx
                    ptrPix = MvApi.CameraGetImageBufferEx(this.m_hCamera, ref width, ref height, (uint)this.CameraParam.Timeout);
                    HImage image = new HImage();
                    HImage adjImage = new HImage();
                    ////////////////////////////////
                    this.AdjImg(image, out adjImage);  // 调整图像的方向及图像畸变
                    image?.Dispose();
                    this.CameraParam.DataWidth = (int)width;
                    this.CameraParam.DataHeight = (int)height;
                    imageList.Add(adjImage);
                    if (ptrPix != IntPtr.Zero)
                        Marshal.FreeHGlobal(ptrPix);
                }
            }
            //////////////////
            if (imageList.Count > 0)
            {
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



    }
}

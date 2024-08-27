using HalconDotNet;
using IKapC.NET;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Common;
using System.Windows.Forms;
using System.IO;
using System.Collections.Concurrent;

namespace Sensor
{
    internal class LineCamera
    {
        private bool StreamGrabIsOpen = false;
        private Stopwatch stopwatch = new Stopwatch();
        // 相机设备句柄。
        private IntPtr m_hCamera = new IntPtr(-1);
        private IntPtr intPtrJudge = new IntPtr(-1);
        // 数据流句柄。
        private IntPtr m_hStream = new IntPtr(-1);
        // 图像缓冲区句柄。
        private List<IntPtr> m_hBufferList = new List<IntPtr>();
        // 当前帧索引。
        private int m_nCurFrameIndex = 0;
        // 图像缓冲区申请的帧数。
        private int m_nBufferCountOfStream = 50;
        // 缓冲区数据。
        private IntPtr m_bufferData = new IntPtr(-1);
        private uint res = (uint)ItkStatusErrorId.ITKSTATUS_OK;
        // 图像宽度。
        public int _frameWidth = 2048;
        // 图像高度。
        public int _frameHeight = 1000;

        private Dictionary<int, byte[]> dicImage = new Dictionary<int, byte[]>();
        private int m_CurImageIndex = 0; //
        private int dataFrameIndex = 0;
        private int m_FrameNumByImage = 0;
        private int rowIndex = 0;
        public CameraParam CamParam { get; set; }
        private enImageAcqMethod _imageAcqMethod = enImageAcqMethod.明场;
        public enImageAcqMethod ImageAcqMethod { get => _imageAcqMethod; set => _imageAcqMethod = value; }

        //private byte[] pSaveDataS;
        //private ConcurrentQueue<byte> framDataList = new ConcurrentQueue<byte>();
        //private object lockSyn = new object();

        //* @brief：本函数被注册为一个回调函数。当数据流开始时，函数被调用。
        private IKapCLib.PITKSTREAMCALLBACK cbOnStartOfStreamProc = null;
        //* @brief：本函数被注册为一个回调函数。当一帧图像采集结束时，函数被调用。
        private IKapCLib.PITKSTREAMCALLBACK cbOnEndOfFrameProc = null;
        //* @brief：本函数被注册为一个回调函数。当图像采集超时时，函数被调用。
        private IKapCLib.PITKSTREAMCALLBACK cbOnTimeOutProc = null;
        //* @brief：本函数被注册为一个回调函数。当采集丢帧时，函数被调用。
        private IKapCLib.PITKSTREAMCALLBACK cbOnFrameLostProc = null;
        //* @brief：本函数被注册为一个回调函数。当数据流结束时，函数被调用。
        private IKapCLib.PITKSTREAMCALLBACK cbOnEndOfStreamProc = null;


        #region Callback

        /* @brief：判断函数是否成功调用。
        * @param[in] res：函数返回值。 */
        private static void CheckIKapC(uint res)
        {
            if (res != (uint)ItkStatusErrorId.ITKSTATUS_OK)
            {
                IKapCLib.ItkManTerminate();
                LoggerHelper.Error("指令操作出错!");
            }
        }

        /* @brief：本函数被注册为一个回调函数。当数据流开始时，函数被调用。*/
        public void OnStartOfStreamFunc(uint eventType, IntPtr pContext)
        {
            //Thread.Sleep(200);
        }
        /* @brief：本函数被注册为一个回调函数。当一帧图像采集结束时，函数被调用。*/
        private void OnEndOfFrameFunc(uint eventType, IntPtr pContext)
        {
            //stopwatch.Restart();
            uint res = (uint)ItkStatusErrorId.ITKSTATUS_OK;
            IntPtr bufferStatus = Marshal.AllocHGlobal(4);
            IntPtr nImageSize = Marshal.AllocHGlobal(8);
            IntPtr hBuffer = m_hBufferList.ElementAt(m_nCurFrameIndex); // pContext 这里为流指针，因为在注册回调函数时，传入的是流指针
            uint nStatus = 0;
            uint nBufferSize = 0;
            res = IKapCLib.ItkBufferGetPrm(hBuffer, (uint)ItkBufferPrm.ITKBUFFER_PRM_STATE, (IntPtr)(bufferStatus));
            CheckIKapC(res);
            nStatus = (uint)Marshal.ReadInt32(bufferStatus);
            Marshal.FreeHGlobal(bufferStatus);  // cxh: 2023-11-16
            // 当图像缓冲区满或者图像缓冲区非满但是无法采集完整的一帧图像时。
            switch (nStatus)
            {
                case 2:
                    // 读取缓冲区数据。
                    res = IKapCLib.ItkBufferGetPrm(hBuffer, (uint)ItkBufferPrm.ITKBUFFER_PRM_SIZE, nImageSize);
                    CheckIKapC(res);
                    nBufferSize = (uint)Marshal.ReadInt64(nImageSize);
                    Marshal.FreeHGlobal(nImageSize);
                    res = IKapCLib.ItkBufferRead(hBuffer, 0, m_bufferData, (uint)nBufferSize);
                    CheckIKapC(res);
                    //this.pSaveDataS = new byte[this._frameWidth * this._frameHeight];
                    //this.pSaveDataS = new byte[nBufferSize];
                    //Marshal.Copy(m_bufferData, this.pSaveDataS, 0, this.pSaveDataS.Length);
                    // 将数据添加到绶存中
                    int result = 0;
                    int value = Math.DivRem(this.dataFrameIndex, this.m_FrameNumByImage, out result);
                    if (!this.dicImage.ContainsKey(value)) // 用商值来判断键，用余数来判断 
                        this.dicImage.Add(value, new byte[this._frameHeight * this._frameWidth * this.m_FrameNumByImage]);
                    Marshal.Copy(m_bufferData, this.dicImage[value], result * this._frameWidth * this._frameHeight, this._frameWidth * this._frameHeight);
                    this.dataFrameIndex++; // 每增加一帧 ，索引 + 1

                    LoggerHelper.Info("获取缓冲区数据成功，当前nStatus的值为：" + nStatus.ToString());
                    break;
                case 1:
                    LoggerHelper.Info("图像缓冲区为空，当前nStatus的值为：" + nStatus.ToString());
                    break;
                case 4:
                    LoggerHelper.Info("图像缓冲区覆盖，当前nStatus的值为：" + nStatus.ToString());
                    break;
                case 8:
                    LoggerHelper.Info("图像缓冲区未存满，当前nStatus的值为：" + nStatus.ToString());
                    break;
            }
            /////////////////////////////////
            this.m_nCurFrameIndex++;
            this.m_nCurFrameIndex = this.m_nCurFrameIndex % this.m_nBufferCountOfStream;

            LoggerHelper.Info("当前帧索引 = " + m_nCurFrameIndex.ToString());
            LoggerHelper.Info("当前数据帧索引 = " + dataFrameIndex.ToString());
        }

        /* @brief：本函数被注册为一个回调函数。当图像采集超时时，函数被调用。*/
        private void OnTimeOutFunc(uint eventType, IntPtr pContext)
        {
        }
        /* @brief：本函数被注册为一个回调函数。当采集丢帧时，函数被调用。*/
        private void OnFrameLostFunc(uint eventType, IntPtr pContext)
        {
        }
        /* @brief：本函数被注册为一个回调函数。当数据流结束时，函数被调用。 */
        private void OnEndOfStreamFunc(uint eventType, IntPtr pContext)
        {
        }
        #endregion


        /// <summary>
        /// 打开相机
        /// </summary>
        /// <returns></returns>
        public bool Open(string camName)
        {
            bool result = false;
            try
            {
                //初始化相机
                uint res = (uint)ItkStatusErrorId.ITKSTATUS_OK;
                res = IKapCLib.ItkManInitialize();
                CheckIKapC(res);
                if (res != 0)
                    LoggerHelper.Error("相机初始化失败!");
                uint numCameras = 0;
                // 枚举可用相机的数量。在打开相机前，必须调用 ItkManGetDeviceCount() 函数。
                IKapCLib.ItkManGetDeviceCount(ref numCameras);
                // 当没有连接的相机时。
                if (numCameras == 0)
                {
                    IKapCLib.ItkManTerminate();
                    return false;
                }
                IKapCLib.ITKDEV_INFO di = new IKapCLib.ITKDEV_INFO();
                for (uint i = 0; i < numCameras; i++)
                {  // 获取相机设备信息。
                    IKapCLib.ItkManGetDeviceInfo(i, ref di);
                    if (camName == di.UserDefinedName) // CameraEnum.Cam1
                    {
                        IKapCLib.ITKGIGEDEV_INFO gv_board_info = new IKapCLib.ITKGIGEDEV_INFO();
                        // 打开相机。
                        res = IKapCLib.ItkDevOpen(i, (int)ItkDeviceAccessMode.ITKDEV_VAL_ACCESS_MODE_EXCLUSIVE, ref this.m_hCamera);
                        CheckIKapC(res);
                        if (res != 0)
                            LoggerHelper.Error("Cam1相机打开失败！");
                        // 获取 GigECamera 相机设备信息。
                        IKapCLib.ItkManGetGigEDeviceInfo(i, ref gv_board_info);
                        if (m_hCamera.Equals(intPtrJudge))
                            return false;
                        //this.SetCamPara();
                        //this.CreateStreamAndBuffer();
                        //this.ConfigureStreamAsyn();
                    }
                }
            }
            catch
            {
                result = false;
            }
            if (this.m_hCamera.ToInt64() != -1)
                result = true;
            else
                result = false;
            return result;
        }

        public bool LoadConfigFile(string path)
        {
            if (path != null && File.Exists(path))
                res = IKapCLib.ItkDevLoadConfigurationFromFile(this.m_hCamera, path);
            else
            {
                if (path != null && path.Length > 0 && File.Exists(new FileInfo(path).Name)) // 如果指定路径中不存在，则从执行目录中寻找
                    res = IKapCLib.ItkDevLoadConfigurationFromFile(this.m_hCamera, new FileInfo(path).Name);
            }
            if (res == 0)
                return true;
            else
                return false;
        }

        /// <summary>
        /// 设置图像参数
        /// </summary>
        public void SetCamPara(bool isTrigger = true)
        {
            uint res = (uint)ItkStatusErrorId.ITKSTATUS_OK;
            //设置为连续采集
            res = IKapCLib.ItkDevFromString(m_hCamera, "AcquisitionMode", "Continuous");
            CheckIKapC(res);

            // 关闭帧触发模式。
            res = IKapCLib.ItkDevFromString(m_hCamera, "TriggerSelector", "FrameStart");
            CheckIKapC(res);
            res = IKapCLib.ItkDevFromString(m_hCamera, "TriggerMode", "Off");
            CheckIKapC(res);

            // 打开行触发模式。
            res = IKapCLib.ItkDevFromString(m_hCamera, "TriggerSelector", "LineStart");
            CheckIKapC(res);
            if (!isTrigger)
            {
                res = IKapCLib.ItkDevFromString(m_hCamera, "TriggerMode", "Off");
                CheckIKapC(res);
                return;
            }
            res = IKapCLib.ItkDevFromString(m_hCamera, "TriggerMode", "On");
            CheckIKapC(res);
            ////设置曝光时间
            //res = IKapCLib.ItkDevSetDouble(m_hCamera, "ExposureTime", 17);//速度200，行频28571
            //CheckIKapC(res);

            //设置方向
            res = IKapCLib.ItkDevFromString(m_hCamera, "ScanDirection", "L2R");
            CheckIKapC(res);
            // 选择触发源。
            res = IKapCLib.ItkDevFromString(m_hCamera, "TriggerSource", "RotaryEncoder1");
            CheckIKapC(res);
            // 选择计数方向。
            //res = IKapCLib.ItkDevFromString(m_hCamera, "RotaryEncoderDirection", "Clockwise");
            //CheckIKapC(res);
            //设置上升沿
            res = IKapCLib.ItkDevFromString(m_hCamera, "LineTriggerEdge", "SingleEdge");
            CheckIKapC(res);

            //设置跟随方向
            res = IKapCLib.ItkDevFromString(m_hCamera, "RotaryEncoderCounterMode", "FollowDirection");
            CheckIKapC(res);
            ////设置差分输入
            //res = IKapCLib.ItkDevFromString(m_hCamera, "InputLineFormat", "RS422");
            //CheckIKapC(res);

            //设置去抖动周期
            //res = IKapCLib.ItkDevSetInt32(m_hCamera, "InputLineDebouncingPeriod", 1);
            //CheckIKapC(res);

            ////设置增益
            //res = IKapCLib.ItkDevFromString(m_hCamera, "AnalogueGain", "Gain_x8");
            //CheckIKapC(res);
            //res = IKapCLib.ItkDevFromString(m_hCamera, "DigitalGain", "4.0");
            //CheckIKapC(res);
        }

        /// <summary>
        /// 创建数据流和缓冲区
        /// </summary>      
        public bool CreateStreamAndBuffer()
        {
            if (this.m_hCamera.ToInt64() == -1) return false;
            uint res = (uint)ItkStatusErrorId.ITKSTATUS_OK;
            // 数据流数量。The number of data stream.
            uint streamCount = 0;
            // 图像宽度。Image width.
            Int64 nWidth = 0;
            // 图像高度。Image height.
            Int64 nHeight = 0;
            // 像素格式。Pixel format.
            uint nFormat = (uint)ItkBufferFormat.ITKBUFFER_VAL_FORMAT_MONO8;
            // 图像大小。Image size.
            IntPtr nImageSize = Marshal.AllocHGlobal(8);
            // 像素格式名称。 Pixel format name.
            StringBuilder pixelFormat = new StringBuilder(128);
            // 像素格式名称长度。Pixel format name length.
            uint pixelFormatSize = 128;
            // 缓冲区大小。Buffer size.
            uint nBufferSize = 0;
            // 获取数据流数量。
            res = IKapCLib.ItkDevGetStreamCount(m_hCamera, ref streamCount);
            CheckIKapC(res);
            if (streamCount == 0)
            {
                IKapCLib.ItkManTerminate();
            }
            // 获取图像宽度。Get image width.
            res = IKapCLib.ItkDevGetInt64(m_hCamera, "Width", ref nWidth);
            CheckIKapC(res);
            this._frameWidth = (int)nWidth;
            // 获取图像高度。 Get image height.
            res = IKapCLib.ItkDevGetInt64(m_hCamera, "Height", ref nHeight);
            if (nHeight != this._frameHeight)
            {
                res = IKapCLib.ItkDevSetInt64(m_hCamera, "Height", this._frameHeight); // 这里是否需要强制设置为 2000？
                nHeight = this._frameHeight;
            }
            res = IKapCLib.ItkDevGetInt64(m_hCamera, "Height", ref nHeight); // 如果上一步设置的图像高度，那么这一步需要重新获取一次
            CheckIKapC(res);
            //this._frameHeight = (int)nHeight; // 这几个参数在相机 Dem 软件里设置
            // 获取像素格式。Get pixel format.
            res = IKapCLib.ItkDevToString(m_hCamera, "PixelFormat", pixelFormat, ref pixelFormatSize);
            CheckIKapC(res);
            if (pixelFormat.ToString() == "Mono8")
                nFormat = (uint)ItkBufferFormat.ITKBUFFER_VAL_FORMAT_MONO8;
            else if (pixelFormat.ToString() == "Mono10")
                nFormat = (uint)ItkBufferFormat.ITKBUFFER_VAL_FORMAT_MONO10;
            else if (pixelFormat.ToString() == "Mono10Packed")
                nFormat = (uint)ItkBufferFormat.ITKBUFFER_VAL_FORMAT_MONO10PACKED;
            else if (pixelFormat.ToString() == "BayerGR8")
                nFormat = (uint)ItkBufferFormat.ITKBUFFER_VAL_FORMAT_BAYER_GR8;
            else if (pixelFormat.ToString() == "BayerRG8")
                nFormat = (uint)ItkBufferFormat.ITKBUFFER_VAL_FORMAT_BAYER_RG8;
            else if (pixelFormat.ToString() == "BayerGB8")
                nFormat = (uint)ItkBufferFormat.ITKBUFFER_VAL_FORMAT_BAYER_GB8;
            else if (pixelFormat.ToString() == "BayerBG8")
                nFormat = (uint)ItkBufferFormat.ITKBUFFER_VAL_FORMAT_BAYER_BG8;
            else
            {
                IKapCLib.ItkManTerminate();
            }
            // 创建图像缓冲区。
            IntPtr hBuffer = new IntPtr();
            res = IKapCLib.ItkBufferNew(nWidth, nHeight, nFormat, ref hBuffer);
            CheckIKapC(res);
            m_hBufferList.Add(hBuffer);
            // 获取缓冲区大小。
            res = IKapCLib.ItkBufferGetPrm(hBuffer, (uint)ItkBufferPrm.ITKBUFFER_PRM_SIZE, nImageSize);
            CheckIKapC(res);
            nBufferSize = (uint)Marshal.ReadInt64(nImageSize);
            Marshal.FreeHGlobal(nImageSize); // cxh : 2023-11-16
            // 创建缓冲区数据存储。
            m_bufferData = Marshal.AllocHGlobal((int)nBufferSize);
            if (m_bufferData.Equals(-1))
            {
                IKapCLib.ItkManTerminate();
            }
            // 申请数据流资源。
            res = IKapCLib.ItkDevAllocStream(this.m_hCamera, 0, hBuffer, ref m_hStream);
            CheckIKapC(res);
            // 向数据流中添加缓冲区。
            for (int i = 1; i < m_nBufferCountOfStream; i++)
            {
                res = IKapCLib.ItkBufferNew(nWidth, nHeight, nFormat, ref hBuffer);
                CheckIKapC(res);
                res = IKapCLib.ItkStreamAddBuffer(m_hStream, hBuffer);
                CheckIKapC(res);
                m_hBufferList.Add(hBuffer);
            }
            if (res == 0) return true;
            else return false;
        }

        /// <summary>
        /// 配置数据流
        /// </summary>
        public bool ConfigureStreamAsyn()
        {
            if (this.m_hCamera.ToInt64() == -1) return false;
            uint res = (uint)ItkStatusErrorId.ITKSTATUS_OK;
            // 传输模式。
            IntPtr xferMode = Marshal.AllocHGlobal(4);
            Marshal.WriteInt32(xferMode, 0, (int)ItkStreamTransferMode.ITKSTREAM_VAL_TRANSFER_MODE_SYNCHRONOUS_WITH_PROTECT);
            // 采集模式。
            IntPtr startMode = Marshal.AllocHGlobal(4);
            Marshal.WriteInt32(startMode, 0, (int)ItkStreamStartMode.ITKSTREAM_VAL_START_MODE_NON_BLOCK);
            // 超时时间。
            IntPtr timeOut = Marshal.AllocHGlobal(4);
            Marshal.WriteInt32(timeOut, 0, (int)IKapCLib.ITKSTREAM_CONTINUOUS);
            //包间超时
            IntPtr interTimeout = Marshal.AllocHGlobal(4);
            Marshal.WriteInt32(interTimeout, 0, 36000);
            // 设置采集模式。
            IKapCLib.ItkStreamSetPrm(m_hStream, (uint)ItkStreamPrm.ITKSTREAM_PRM_START_MODE, startMode);
            // 设置传输模式。
            IKapCLib.ItkStreamSetPrm(m_hStream, (uint)ItkStreamPrm.ITKSTREAM_PRM_TRANSFER_MODE, xferMode);
            // 设置超时时间。
            IKapCLib.ItkStreamSetPrm(m_hStream, (uint)ItkStreamPrm.ITKSTREAM_PRM_TIME_OUT, timeOut);
            // 设置包间超时时间。
            // Set time out time.
            res = IKapCLib.ItkStreamSetPrm(m_hStream, (uint)ItkStreamPrm.ITKSTREAM_PRM_GV_PACKET_INTER_TIMEOUT, interTimeout);
            ///// 释放内存
            Marshal.FreeHGlobal(xferMode);
            Marshal.FreeHGlobal(startMode);
            Marshal.FreeHGlobal(timeOut);
            Marshal.FreeHGlobal(interTimeout);
            CheckIKapC(res);
            ///////////////////////////
            this.RegisterCallback();
            if (res == 0)
                return true;
            else
                return false;
        }
        public bool ConfigureStreamSyn()
        {
            uint res = (uint)ItkStatusErrorId.ITKSTATUS_OK;
            // 传输模式。
            IntPtr xferMode = Marshal.AllocHGlobal(4);
            Marshal.WriteInt32(xferMode, 0, (int)ItkStreamTransferMode.ITKSTREAM_VAL_TRANSFER_MODE_SYNCHRONOUS_WITH_PROTECT);
            // 采集模式。
            IntPtr startMode = Marshal.AllocHGlobal(4);
            Marshal.WriteInt32(startMode, 0, (int)ItkStreamStartMode.ITKSTREAM_VAL_START_MODE_BLOCK); // 阻塞采集
            // 超时时间。
            IntPtr timeOut = Marshal.AllocHGlobal(4);
            Marshal.WriteInt32(timeOut, 0, (int)IKapCLib.ITKSTREAM_CONTINUOUS);
            //包间超时
            IntPtr interTimeout = Marshal.AllocHGlobal(4);
            Marshal.WriteInt32(interTimeout, 0, 36000);
            // 设置采集模式。
            IKapCLib.ItkStreamSetPrm(m_hStream, (uint)ItkStreamPrm.ITKSTREAM_PRM_START_MODE, startMode);
            // 设置传输模式。
            IKapCLib.ItkStreamSetPrm(m_hStream, (uint)ItkStreamPrm.ITKSTREAM_PRM_TRANSFER_MODE, xferMode);
            // 设置超时时间。
            IKapCLib.ItkStreamSetPrm(m_hStream, (uint)ItkStreamPrm.ITKSTREAM_PRM_TIME_OUT, timeOut);
            // 设置包间超时时间。
            // Set time out time.
            res = IKapCLib.ItkStreamSetPrm(m_hStream, (uint)ItkStreamPrm.ITKSTREAM_PRM_GV_PACKET_INTER_TIMEOUT, interTimeout);
            CheckIKapC(res);
            if (res == 0)
                return true;
            else
                return false;
        }
        public bool StartGrab()
        {
            bool reault = false;
            if (this.m_hCamera.ToInt64() != -1)
            {
                this.m_nCurFrameIndex = 0;
                this.m_CurImageIndex = 0;
                this.dataFrameIndex = 0; // 统计在一次采集中帧的总数量
                this.rowIndex = 0;
                switch (this._imageAcqMethod)
                {
                    case enImageAcqMethod.明暗场:
                        this.m_FrameNumByImage = (this.CamParam.DataHeight / this._frameHeight) * 2;  // 如果获取的是明暗场图像，那么需要乘以 2 ，因为每帧图像中有一半分别是明场和暗场
                        break;
                    default:
                    case enImageAcqMethod.明场:
                    case enImageAcqMethod.暗场:
                        this.m_FrameNumByImage = (this.CamParam.DataHeight / this._frameHeight);  // 每个图像需要发帧数量
                        break;
                }
                if (this.StreamGrabIsOpen) // 如果流是打开的，先停止再启动
                    res = IKapCLib.ItkStreamStop(m_hStream);
                ///////
                res = IKapCLib.ItkStreamStart(m_hStream, (uint)IKapCLib.ITKSTREAM_CONTINUOUS);
                CheckIKapC(res);
                if (res == 0)
                {
                    reault = true;
                    this.StreamGrabIsOpen = true;
                    LoggerHelper.Info("开始图像采集成功！");
                }
                else
                {
                    LoggerHelper.Error("开始图像采集失败！");
                }
            }
            return reault;
        }

        /// <summary>
        /// 停止采集
        /// </summary>
        /// <returns></returns>
        public bool StopGrab()
        {
            bool result = false;
            try
            {
                if (this.m_hCamera.ToInt64() != -1)
                {
                    this.StreamGrabIsOpen = false;
                    ///////////// 流停止时将缓冲区状态都设置为空 /////////////////////////
                    uint res = (uint)ItkStatusErrorId.ITKSTATUS_OK;
                    res = IKapCLib.ItkStreamStop(m_hStream);
                    if (res == 0)
                    {
                        result = true;
                        LoggerHelper.Info("停止相机采集成功！");
                    }
                    else
                    {
                        LoggerHelper.Error("停止相机流采集失败！");
                        result = false;
                    }
                    this.StreamGrabIsOpen = false;
                }
            }
            catch (Exception e0)
            {
                LoggerHelper.Fatal(e0.Message);
                result = false;
            }
            return result;
        }

        public void RegisterCallback()
        {
            // 注册回调函数 
            cbOnStartOfStreamProc = new IKapCLib.PITKSTREAMCALLBACK(OnStartOfStreamFunc);
            res = IKapCLib.ItkStreamRegisterCallback(m_hStream, (uint)ItkStreamEventType.ITKSTREAM_VAL_EVENT_TYPE_START_OF_STREAM, cbOnStartOfStreamProc, m_hStream);
            CheckIKapC(res);
            cbOnEndOfStreamProc = new IKapCLib.PITKSTREAMCALLBACK(OnEndOfStreamFunc);
            res = IKapCLib.ItkStreamRegisterCallback(m_hStream, (uint)ItkStreamEventType.ITKSTREAM_VAL_EVENT_TYPE_END_OF_STREAM, cbOnEndOfStreamProc, m_hStream);
            CheckIKapC(res);
            cbOnEndOfFrameProc = new IKapCLib.PITKSTREAMCALLBACK(OnEndOfFrameFunc);
            res = IKapCLib.ItkStreamRegisterCallback(m_hStream, (uint)ItkStreamEventType.ITKSTREAM_VAL_EVENT_TYPE_END_OF_FRAME, cbOnEndOfFrameProc, m_hStream);
            CheckIKapC(res);
            cbOnTimeOutProc = new IKapCLib.PITKSTREAMCALLBACK(OnTimeOutFunc);
            res = IKapCLib.ItkStreamRegisterCallback(m_hStream, (uint)ItkStreamEventType.ITKSTREAM_VAL_EVENT_TYPE_TIME_OUT, cbOnTimeOutProc, m_hStream);
            CheckIKapC(res);
            cbOnFrameLostProc = new IKapCLib.PITKSTREAMCALLBACK(OnFrameLostFunc);
            res = IKapCLib.ItkStreamRegisterCallback(m_hStream, (uint)ItkStreamEventType.ITKSTREAM_VAL_EVENT_TYPE_FRAME_LOST, cbOnFrameLostProc, m_hStream);
            CheckIKapC(res);
        }

        /* @brief：清除回调函数。 */
        public void UnRegisterCallback()
        {
            res = IKapCLib.ItkStreamUnregisterCallback(m_hStream, (uint)ItkStreamEventType.ITKSTREAM_VAL_EVENT_TYPE_START_OF_STREAM);
            res = IKapCLib.ItkStreamUnregisterCallback(m_hStream, (uint)ItkStreamEventType.ITKSTREAM_VAL_EVENT_TYPE_END_OF_STREAM);
            res = IKapCLib.ItkStreamUnregisterCallback(m_hStream, (uint)ItkStreamEventType.ITKSTREAM_VAL_EVENT_TYPE_END_OF_FRAME);
            res = IKapCLib.ItkStreamUnregisterCallback(m_hStream, (uint)ItkStreamEventType.ITKSTREAM_VAL_EVENT_TYPE_TIME_OUT);
            res = IKapCLib.ItkStreamUnregisterCallback(m_hStream, (uint)ItkStreamEventType.ITKSTREAM_VAL_EVENT_TYPE_FRAME_LOST);
        }

        /// <summary>
        /// 关闭设备
        /// </summary>
        /// <returns></returns>
        public bool Close() //关闭相机
        {
            try
            {
                //停止采集
                StopGrab();
                //清除回调函数
                UnRegisterCallback();
                // 释放数据流和缓冲区。
                foreach (var it in m_hBufferList)
                {
                    IKapCLib.ItkStreamRemoveBuffer(m_hStream, it);
                    IKapCLib.ItkBufferFree(it);
                }
                m_hBufferList.Clear();
                IKapCLib.ItkDevFreeStream(m_hStream);
                // 关闭相机设备。
                if (!m_hCamera.Equals(-1))
                {
                    IKapCLib.ItkDevClose(m_hCamera);
                    m_hCamera = (IntPtr)(-1);
                }
                if (!m_bufferData.Equals(-1))
                {
                    Marshal.FreeHGlobal(m_bufferData);
                }
                //释放IKapC运行环境
                IKapCLib.ItkManTerminate();
                return true;
            }
            catch (Exception ex)
            {
                LoggerHelper.Error("关闭相机出错" + ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// 测试采图
        /// </summary>
        public double GetExpos()
        {
            double ExposTime = 0;
            try
            {
                //设置曝光时间
                if (this.m_hCamera.ToInt64() != -1)
                {
                    IKapCLib.ItkDevGetDouble(m_hCamera, "ExposureTime", ref ExposTime);
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.Error("获取相机曝光参数出错" + ex.ToString());
                //return false;
            }
            return ExposTime;
        }
        public int GetGain()
        {
            double Gain = 0;
            try
            {
                //设置曝光时间
                if (this.m_hCamera.ToInt64() != -1)
                {
                    IKapCLib.ItkDevGetDouble(m_hCamera, "Gain", ref Gain);
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.Error("获取相机增益参数出错" + ex.ToString());
                //return false;
            }
            return (int)Gain;
        }
        public bool SetGain(int Gain)
        {
            try
            {
                //设置曝光时间
                if (this.m_hCamera.ToInt64() != -1)
                {
                    IKapCLib.ItkDevSetDouble(m_hCamera, "Gain", Gain);
                    return true;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                LoggerHelper.Error("设置相机增益参数出错" + ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// 测试采图
        /// </summary>
        public bool SetExpos(int ExposTime)
        {
            try
            {
                //设置曝光时间
                if (this.m_hCamera.ToInt64() != -1)
                {
                    IKapCLib.ItkDevSetDouble(m_hCamera, "ExposureTime", ExposTime);
                    return true;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                LoggerHelper.Error("设置相机曝光参数出错" + ex.ToString());
                return false;
            }
        }

        public HImage GetHImage(int imageWidth, int imageHeight, string type, int timeout, out HImage darkImage, out enAcqState state)
        {
            state = enAcqState.Continue;
            HImage hImage = new HImage();
            darkImage = new HImage();
            if (imageWidth != this._frameWidth)
                imageWidth = this._frameWidth; // 不管是线阵还是面阵，图像宽度都是一样的
            #region //等待数据是否有更新
            int result = 0, value = 0;
            this.stopwatch.Restart();
            while (true)
            {
                value = Math.DivRem(this.dataFrameIndex, this.m_FrameNumByImage, out result);
                if (value > this.m_CurImageIndex) break; // 表示采集了一幅图像
                if (this.stopwatch.ElapsedMilliseconds >= timeout)
                {
                    if (!this.dicImage.ContainsKey(this.m_CurImageIndex))
                        this.dicImage.Add(this.m_CurImageIndex, new byte[imageWidth * imageHeight]);
                    break; // 超过指定时间还没有采集到完整图像，那么将结束采图
                }
            }
            this.stopwatch.Stop();
            LoggerHelper.Info("等待图像获取时间 = " + this.stopwatch.ElapsedMilliseconds.ToString());
            #endregion
            switch (this._imageAcqMethod)
            {
                case enImageAcqMethod.明暗场:
                    this.rowIndex = 0;
                    byte[] lightData = new byte[imageWidth * imageHeight];// 
                    byte[] darkData = new byte[imageWidth * imageHeight];// 
                    /////////////////////////////////////
                    for (int i = 0; i < this.dicImage[this.m_CurImageIndex].Length; i++)
                    {
                        if (this.rowIndex % 2 == 0) // 偶数行，明场采集 ,这个用与分时频闪取图
                        {
                            lightData[i] = this.dicImage[this.m_CurImageIndex][i];
                        }
                        else // 奇数行，暗场采集
                        {
                            darkData[i] = this.dicImage[this.m_CurImageIndex][i];
                        }
                        if (i > 0 && i % this._frameWidth == 0) this.rowIndex++; // 每取完一行的数据，递增一次行索引
                    }
                    IntPtr pixPtr2 = IntPtr.Zero;
                    pixPtr2 = Marshal.AllocHGlobal(lightData.Length);
                    Marshal.Copy(lightData, 0, pixPtr2, lightData.Length);
                    hImage.GenImage1(type, imageWidth, imageHeight, pixPtr2);
                    ///// 生成暗场图片 /////
                    Marshal.Copy(darkData, 0, pixPtr2, darkData.Length);
                    darkImage.GenImage1(type, imageWidth, imageHeight, pixPtr2);
                    //////////////////////////
                    Marshal.FreeHGlobal(pixPtr2);
                    LoggerHelper.Info("获取图像数据成功，图像索引 = " + this.m_CurImageIndex.ToString());
                    break;
                default:
                case enImageAcqMethod.明场:
                case enImageAcqMethod.暗场:
                    /////////////////////////////////////
                    IntPtr pixPtr = IntPtr.Zero;
                    pixPtr = Marshal.AllocHGlobal(this.dicImage[this.m_CurImageIndex].Length);
                    Marshal.Copy(this.dicImage[this.m_CurImageIndex], 0, pixPtr, this.dicImage[this.m_CurImageIndex].Length);
                    hImage.GenImage1(type, imageWidth, imageHeight, pixPtr);
                    Marshal.FreeHGlobal(pixPtr);
                    LoggerHelper.Info("获取图像数据成功，图像索引 = " + this.m_CurImageIndex.ToString());
                    break;
            }
            /////////////////////////////////////////////////
            this.m_CurImageIndex++; // 获取一次图像后，当前图像索引增 1 
            return hImage;
        }



        public void ClearData()
        {
            //lock (this.lockSyn)
            //{
            //byte value;
            //while (true)
            //{
            //    if (this.framDataList.Count > 0)
            //        this.framDataList.TryDequeue(out value);
            //    else
            //        break;
            //}

            //}
            this.dicImage?.Clear();
        }


    }

}


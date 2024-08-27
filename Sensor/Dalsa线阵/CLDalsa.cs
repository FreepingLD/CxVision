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
using DALSA.SaperaLT.SapClassBasic;

namespace Sensor
{
    public class CLDalsa
    {
        private SapAcqDevice AcqDevice = null;
        /// <summary>sapera sdk</summary>
        private SapAcquisition m_Acquisition = null; //设备的连接地址
        private SapBuffer m_Buffers = null;          //缓存对象
        private SapTransfer m_Xfer = null;           //传输对象
        private int m_nBufferCountOfStream = 50; // 图像绶冲区数量
        private int _frameWidth, _frameHeight;  // 图像帧宽与高
        private Stopwatch stopwatch = new Stopwatch();
        // 当前帧索引。
        private int m_nCurFrameIndex = 0; //  回调函数中当前帧索引 
        private int m_CurImageIndex = 0; //  当前图像索引
        private int m_FrameNumByImage = 0;  // 每图像帧的数量
        private int dataFrameIndex = 0;  // 累积的数据帧索引
        private int rowIndex = 0;  // 分时频闪行索引
        //private byte[] pSaveDataS;
        private Dictionary<int, byte[]> dicImage = new Dictionary<int, byte[]>();
        private IntPtr pImgBufAddress = IntPtr.Zero;  // 绶存指针变量

        private enImageAcqMethod _imageAcqMethod = enImageAcqMethod.明场;
        public enImageAcqMethod ImageAcqMethod { get => _imageAcqMethod; set => _imageAcqMethod = value; }


        public CameraParam CamParam { get; set; }

        public bool Open(string serverName, string camName)
        {
            int DalsaCardCount = SapManager.GetServerCount(); //获取图像采集卡的数量
            for (int i = 0; i < DalsaCardCount; i++)
            {
                if (SapManager.GetResourceCount(i, SapManager.ResourceType.Acq) > 0) //卡的数量大于0
                {
                    string ServerName = SapManager.GetServerName(i); // Xtium-CL_PX_1 ServerName:即采集卡的名称
                    string DeviceName = SapManager.GetResourceName(ServerName, SapManager.ResourceType.Acq, 0);
                    if (camName != DeviceName) continue;
                    SapLocation location = new SapLocation(ServerName, 0);
                    if (File.Exists(Application.StartupPath + "\\Dalsa\\" + camName + ".ccf"))
                        AcqDevice = new SapAcqDevice(location, Application.StartupPath + "\\Dalsa\\" + camName + ".ccf");
                    else
                        MessageBox.Show("未找到文件名为:" + camName + ".ccf" + " 的配置文件!");
                    ////////////////////////////////////////////////////////////////////////////////////////////////////////
                    if (SapBuffer.IsBufferTypeSupported(location, SapBuffer.MemoryType.ScatterGather))
                        m_Buffers = new SapBuffer(m_nBufferCountOfStream, m_Acquisition, SapBuffer.MemoryType.ScatterGather);
                    else
                        m_Buffers = new SapBufferWithTrash(m_nBufferCountOfStream, m_Acquisition, SapBuffer.MemoryType.ScatterGatherPhysical);
                    m_Xfer = new SapAcqToBuf(m_Acquisition, m_Buffers);
                    m_Xfer.Pairs[0].EventType = SapXferPair.XferEventType.EndOfFrame;
                    m_Xfer.XferNotify += new SapXferNotifyHandler(AcqCallback1);
                    m_Xfer.XferNotifyContext = this;
                    if (!SeparaInterface_CreateObjects())
                    {
                        LoggerHelper.Error(" 创建 相关的采集、传输、缓存对象失败");
                        this.SeparaInterface_DisposeObjects();
                        return false;
                    }
                    this._frameHeight = this.SeparaInterface_GetImageHeight();
                    this._frameWidth = this.SeparaInterface_GetImageWidth();
                    // 分配绶存
                    //this.pSaveDataS = new byte[m_nBufferCountOfStream * this._frameWidth * this._frameHeight];
                    return true;
                }
                else // Gige Connect
                {
                    if (SapManager.GetResourceCount(i, SapManager.ResourceType.AcqDevice) > 0)//相机数量大于0
                    {
                        string ServerName = SapManager.GetServerName(i); // Xtium-CL_PX_1 ServerName:即采集卡的名称
                        string DeviceName = SapManager.GetResourceName(ServerName, SapManager.ResourceType.AcqDevice, 0); // 获取用户名
                        if (camName != DeviceName) continue;
                        SapLocation location = new SapLocation(ServerName, 0);
                        if (File.Exists(Application.StartupPath + "\\Dalsa\\" + camName + ".ccf"))
                            AcqDevice = new SapAcqDevice(location, Application.StartupPath + "\\Dalsa\\" + camName + ".ccf");
                        else
                            MessageBox.Show("未找到文件名为:" + camName + ".ccf" + " 的配置文件!");
                        ////////////////////////////////////////////////////////////////////////////////////////////////////////
                        if (SapBuffer.IsBufferTypeSupported(location, SapBuffer.MemoryType.ScatterGather))
                            m_Buffers = new SapBuffer(m_nBufferCountOfStream, AcqDevice, SapBuffer.MemoryType.ScatterGather);
                        else
                            m_Buffers = new SapBufferWithTrash(m_nBufferCountOfStream, AcqDevice, SapBuffer.MemoryType.ScatterGatherPhysical);
                        m_Xfer = new SapAcqDeviceToBuf(AcqDevice, m_Buffers);
                        m_Xfer.Pairs[0].EventType = SapXferPair.XferEventType.EndOfFrame;
                        m_Xfer.XferNotify += new SapXferNotifyHandler(AcqCallback1);
                        m_Xfer.XferNotifyContext = this;
                        if (!SeparaInterface_CreateObjects())
                        {
                            Console.WriteLine("Error during SapAcqDevice creation!\n");
                            SeparaInterface_DisposeObjects();
                            return false;
                        }
                        this._frameHeight = this.SeparaInterface_GetImageHeight();
                        this._frameWidth = this.SeparaInterface_GetImageWidth();

                        // 分配绶存
                        //this.pSaveDataS = new byte[m_nBufferCountOfStream * this._frameWidth * this._frameHeight];
                        return true;
                    }
                }
            }
            return false;
        }

        public bool ResetCamera(string serverName, string camName)
        {
            this.Close();
            return Open(serverName, camName);
        }

        public bool Close()
        {
            try
            {
                if (m_Xfer != null)
                    m_Xfer.XferNotify -= new SapXferNotifyHandler(AcqCallback1);
                this.SeparaInterface_DestroyObjects();
                this.SeparaInterface_DisposeObjects();
                return true;
            }
            catch
            {
                return false;
            }
        }

        private void AcqCallback1(object sender, SapXferNotifyEventArgs argsNotify)
        {
            this.SeparaInterface_GetBufferAddress(m_nCurFrameIndex, out pImgBufAddress); //获取图片的指针
            //Marshal.FreeHGlobal(pImgBufAddress); // 释放     pImgBufAddress: 这个指针是不变的，这里不需要释放   
            ///////////////////////////////////////
            int result = 0;
            int value = Math.DivRem(this.dataFrameIndex, this.m_FrameNumByImage, out result);
            if (!this.dicImage.ContainsKey(value)) // 用商值来判断键，用余数来判断 
                this.dicImage.Add(value, new byte[this._frameHeight * this._frameWidth * this.m_FrameNumByImage]);
            Marshal.Copy(pImgBufAddress, this.dicImage[value], result * this._frameWidth * this._frameHeight, this._frameWidth * this._frameHeight);
            this.dataFrameIndex++;
            /////////////////////////////////
            m_nCurFrameIndex++;
            m_nCurFrameIndex = m_nCurFrameIndex % m_nBufferCountOfStream;
            ////////////////////////////////////////////////////////////
            LoggerHelper.Info("当前帧索引 = " + m_nCurFrameIndex.ToString());
            LoggerHelper.Info("当前数据帧索引 = " + dataFrameIndex.ToString());
        }

        public bool StartGrab()
        {
            this.dataFrameIndex = 0; // 统计在一次采集中帧的总数量
            this.rowIndex = 0;
            this.m_CurImageIndex = 0;
            this.m_nCurFrameIndex = 0; // 在开始时，重置帧索引
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
            if (this._frameWidth <= 0 || this._frameHeight <= 0)
            {
                this.StopGrab();//停止取像
                return false;
            }
            if (m_Xfer != null && m_Xfer.Initialized)
            {
                if (m_Buffers != null)  //重置缓存和帧计数
                    m_Buffers.ResetIndex();
                if (m_Xfer.Grabbing) // 判断相机是否在取图状太
                {
                    this.StopGrab();//停止取像
                }
                if (!m_Xfer.Grab())
                {
                    LoggerHelper.Error("SaperaCenter_Error_GrabFailed ,相机采图失败");
                    return false;
                }
                return true;
            }
            return false;
        }

        public bool StopGrab()
        {
            string Str = string.Empty;
            if (m_Xfer != null && m_Xfer.Initialized)
            {
                if (m_Xfer.Freeze()) // 为什么用这个指令
                {
                    m_Xfer.Abort();
                    return true;
                }
                else
                {
                    LoggerHelper.Error(" SaperaCenter_Error_Grabing 停止采集失败!");
                    return false;
                }
            }
            return false;
        }



        /// <summary>
        /// 重置采集图像计数
        /// </summary>
        private void SeparaInterface_ResetFrameNum()
        {
            if (m_Buffers != null)
                m_Buffers.ResetIndex();
        }

        /// <summary>
        /// 创建 相关的采集、传输、缓存对象
        /// </summary>
        /// <returns></returns>
        private bool SeparaInterface_CreateObjects()
        {
            // Create acquisition object
            if (m_Acquisition != null && !m_Acquisition.Initialized)
            {
                if (m_Acquisition.Create() == false)
                {
                    SeparaInterface_DestroyObjects();
                    return false;
                }
            }
            if (AcqDevice != null && !AcqDevice.Initialized)
            {
                if (AcqDevice.Create() == false)
                {
                    SeparaInterface_DestroyObjects();
                    return false;
                }
            }
            // Create buffer object
            if (m_Buffers != null && !m_Buffers.Initialized)
            {
                if (m_Buffers.Create() == false)
                {
                    SeparaInterface_DestroyObjects();
                    return false;
                }
                m_Buffers.Clear();
            }

            // Create Xfer object
            if (m_Xfer != null && !m_Xfer.Initialized)
            {
                if (m_Xfer.Create() == false)
                {
                    SeparaInterface_DestroyObjects();
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 销毁 采集、传输、缓存对象
        /// </summary>
        private void SeparaInterface_DestroyObjects()
        {
            if (m_Xfer != null && m_Xfer.Initialized) m_Xfer.Destroy();
            if (m_Buffers != null && m_Buffers.Initialized) m_Buffers.Destroy();
            if (m_Acquisition != null && m_Acquisition.Initialized) m_Acquisition.Destroy();
        }

        /// <summary>
        /// 清空 采集、传输、缓存对象
        /// </summary>
        private void SeparaInterface_DisposeObjects()
        {
            if (m_Xfer != null)
            { m_Xfer.Dispose(); m_Xfer = null; }
            if (m_Buffers != null)
            { m_Buffers.Dispose(); m_Buffers = null; }
            if (m_Acquisition != null)
            { m_Acquisition.Dispose(); m_Acquisition = null; }
            if (AcqDevice != null)
            { AcqDevice.Dispose(); AcqDevice = null; }
        }
        private int SeparaInterface_GetImageWidth()
        {
            if (m_Buffers != null) return m_Buffers.Width;
            return 0;
        }
        private int SeparaInterface_GetImageHeight()
        {
            if (m_Buffers != null) return m_Buffers.Height;
            return 0;
        }
        private int SeparaInterface_GetBytesPerPixel()
        {
            if (m_Buffers != null) return m_Buffers.BytesPerPixel;
            return 0;
        }
        private int SeparaInterface_GetFramIndex()
        {
            if (m_Buffers != null) return m_Buffers.Index;
            return 0;
        }
        private void SeparaInterface_GetBufferAddress(int Index, out IntPtr pImgBufAddress)
        {
            pImgBufAddress = IntPtr.Zero;
            if (m_Buffers != null) m_Buffers.GetAddress(Index, out pImgBufAddress);
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
                        if (rowIndex % 2 == 0) // 偶数行，明场采集 ,这个用与分时频闪取图
                        {
                            lightData[i] = this.dicImage[this.m_CurImageIndex][i];
                        }
                        else // 奇数行，暗场采集
                        {
                            darkData[i] = this.dicImage[this.m_CurImageIndex][i];
                        }
                        if (i > 0 && i % this._frameWidth == 0) rowIndex++; // 每取完一行的数据，递增一次行索引
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
            this.dicImage?.Clear();
            //this.framDataList.Clear();
            //this.framDataLightList.Clear();
            //this.framDataDarkList.Clear();
        }
        public double GetExpos()
        {
            double ExposTime = 0;
            try
            {
                if (AcqDevice != null && AcqDevice.IsFeatureAvailable("ExposureTime"))
                    AcqDevice.GetFeatureValue("ExposureTime", out ExposTime);
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
            int ExposTime = 0;
            try
            {
                if (AcqDevice != null && AcqDevice.IsFeatureAvailable("Gain"))
                    AcqDevice.GetFeatureValue("Gain", out ExposTime);
            }
            catch (Exception ex)
            {
                LoggerHelper.Error("获取相机增益参数出错" + ex.ToString());
            }
            return ExposTime;
        }

        /// <summary>
        /// 测试采图
        /// </summary>
        public bool SetExpos(int ExposTime)
        {
            try
            {
                if (AcqDevice != null && AcqDevice.IsFeatureAvailable("ExposureTime"))
                    AcqDevice.SetFeatureValue("ExposureTime", ExposTime);
                //设置曝光时间
                return true;
            }
            catch (Exception ex)
            {
                LoggerHelper.Error("设置相机曝光参数出错" + ex.ToString());
                return false;
            }
        }
        public bool SetGain(int gain)
        {
            try
            {
                if (AcqDevice != null && AcqDevice.IsFeatureAvailable("Gain"))
                    AcqDevice.SetFeatureValue("Gain", gain);
                //设置曝光时间
                return true;
            }
            catch (Exception ex)
            {
                LoggerHelper.Error("设置相机增益参数出错" + ex.ToString());
                return false;
            }
        }



    }

}


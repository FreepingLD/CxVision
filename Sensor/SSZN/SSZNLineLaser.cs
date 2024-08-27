using Sensor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Common;

namespace Sensor
{
    public class SSZNLineLaser : SensorBase, ISensor
    {
        private FileOperate fo = new FileOperate();
        private object monitor = new object();
        private string ipAdress;
        private int DeviceId = 0;
        private bool b_connect = false;
        private bool b_IOTrigger = false;
        SR7IF_ETHERNET_CONFIG pEthernetConfig;
        bool b_HighSpeedInitFail = false;                   //高速通信初始化失败标志
        int Max_col_Rool = 1000;                            //有限循环单次获取最大行数 
        /********回调***********/
        private HighSpeedDataCallBack _callback;       //回调
        private int m_curBatchNo = 0;                   //回调函数中用到--当前批处理行数编号
        private int m_BatchWidth = 0;                   //回调中轮廓宽度
        private int BatchCallBackPoint = 0;             //回调中当前批处理总行数
        private bool b_call = false;                    //回调函数调用标志位 
        private bool b_startTimerFunc = false;          //线程显示响应标志位 
        //private uint[] EncoderDataCall = new uint[15000 * 2];         //回调编码器数据缓存
        private bool b_IntensityDataFail = false;            //灰度数据获取失败标志
        private bool b_EncoderDataFail = false;				//编码器数据获取失败标志
        private bool b_ProfileDataFail = false;             //高度数据获取失败标志
                                                            // private bool b_MemoryErr = false;                    //内存不足
        private bool b_camBOnline = false;                   //相机B在线标志
        private bool acq_Complete = false;
        double[] X; // 存储X坐标 
        //private int numLinePer = 3200;
        //private string name;
        List<double> listDist = new List<double>();
        List<double> listX = new List<double>();
        List<double> listY = new List<double>();
        public string Adress
        {
            get
            {
                return ipAdress;
            }

            set
            {
                ipAdress = value;
            }
        }

        public bool Acq_Complete
        {
            get
            {
                return acq_Complete;
            }

            set
            {
                acq_Complete = value;
            }
        }

        public SSZNLineLaser()
        {

        }
        public SSZNLineLaser(string sensorName)
        {
            this.ipAdress = sensorName;
        }

        /// <summary>
        /// 回调接受数据
        /// </summary>
        /// <param name="buffer"></param>         指向储存概要数据的缓冲区的指针.
        /// <param name="size"></param>           每个单元(行)的字节数量.
        /// <param name="count"></param>          存储在pBuffer中的内存的单元数量.
        /// <param name="notify"></param>         中断或批量结束等中断的通知.
        /// <param name="user"></param>           用户自定义信息.
        private void ReceiveHighSpeedData(IntPtr buffer, uint size, uint count, uint notify, uint user)
        {
            if (notify != 0) // 如果设置的不是批处理，则报错
            {
                if (Convert.ToBoolean(notify & 0x08))
                {
                    SR7LinkFunc.SR7IF_StopMeasure(DeviceId);
                    System.Console.Write("批处理超时!\n");
                    MessageBox.Show("批处理超时", "提示", MessageBoxButtons.OK);
                    m_curBatchNo = 0;
                    b_EncoderDataFail = true;
                    b_IntensityDataFail = true;
                    b_ProfileDataFail = true;
                }
            }
            if (count == 0 || size == 0)
                return;

            int profileSize = this.m_BatchWidth;
            // uint profileSize = (uint)(size / Marshal.SizeOf(typeof(int)));   //轮廓宽度，即线上的点数，Size:一个轮廓的字节数
            IntPtr DataObject = new IntPtr();
            // 数据x方向间距(mm)
            float m_XPitch = (float)SR7LinkFunc.SR7IF_ProfileData_XPitch(DeviceId, DataObject);

            // 数据拷贝
            int[] bufferArray = new int[profileSize * count]; // count:即轮廓的数量 // 开辟轮廓的Buffer
            uint[] TmpEncoderBuffer = new uint[1000]; // 开辟编码器的Buffer
            Marshal.Copy(buffer, bufferArray, 0, (int)(profileSize * count)); // 获取高度数据
            int TmpNum = Convert.ToInt32(m_curBatchNo * profileSize);
            // 获取编码器数据
            if (m_curBatchNo < BatchCallBackPoint)
            {
                if (m_curBatchNo + count > BatchCallBackPoint)
                {
                    int TmpCount = BatchCallBackPoint - m_curBatchNo;
                    using (PinnedObject pin = new PinnedObject(TmpEncoderBuffer))       //内存自动释放接
                    {
                        int Rc = SR7LinkFunc.SR7IF_GetEncoderContiune(DeviceId, DataObject, pin.Pointer, TmpCount);
                        if (Rc < 0)
                        {
                            b_EncoderDataFail = true;
                            //MessageBox.Show("编码器数据获取失败", "提示", MessageBoxButtons.OK);
                        }
                        else
                        {
                            //  Buffer.BlockCopy(TmpEncoderBuffer, 0, EncoderDataCall, m_curBatchNo * sizeof(uint) * m_EncoderColNum, TmpCount * sizeof(uint));
                            b_EncoderDataFail = false;
                        }
                    }
                    // TmpEncoderBuffer = null;
                    GC.Collect();
                }
                else
                {
                    //获取编码器数据
                    using (PinnedObject pin = new PinnedObject(TmpEncoderBuffer))       //内存自动释放接
                    {
                        int Rc = SR7LinkFunc.SR7IF_GetEncoderContiune(DeviceId, DataObject, pin.Pointer, Convert.ToInt32(count)); // 一个轮廓对应一个编码器值
                        if (Rc < 0)
                        {
                            b_EncoderDataFail = true;
                            MessageBox.Show("编码器数据获取失败", "提示", MessageBoxButtons.OK);
                        }
                        else
                        {
                            // Buffer.BlockCopy(TmpEncoderBuffer, 0, EncoderDataCall, m_curBatchNo * sizeof(uint) * m_EncoderColNum, Convert.ToInt32(count) * sizeof(uint));
                            b_EncoderDataFail = false;
                        }
                    }
                    // TmpEncoderBuffer = null;
                    GC.Collect();
                }
            }
            m_curBatchNo += Convert.ToInt32(count);
            ////////////
            bool isLock = false;
            Monitor.Enter(monitor, ref isLock);
            // 添加距离值到集合中
            for (int i = 0; i < bufferArray.Length; i++)
            {
                this.listDist.Add(bufferArray[i] * 1.0f);
            }
            // 添加X值到集合中
            for (int i = 0; i < count; i++)
            {
                this.listX.AddRange(this.X);
            }
            // 添加Y值到集合中
            for (int i = 0; i < count; i++)
            {
                for (int j = 0; j < profileSize; j++)
                {
                    this.listY.Add(TmpEncoderBuffer[i]);
                }
            }
            if (isLock) Monitor.Exit(monitor);
            TmpEncoderBuffer = null;


            // b_startTimerFunc = true;
            b_call = true;
            if (notify != 0)
            {
                if (notify == 0x10000)
                {
                    SR7LinkFunc.SR7IF_StopMeasure(DeviceId);
                    System.Console.Write("数据接收完成!\n");
                    b_call = false;
                    m_curBatchNo = 0;
                    b_ProfileDataFail = false;
                    acq_Complete = true;
                }
                if (Convert.ToBoolean(notify & 0x80000000))
                {
                    System.Console.Write("批处理重新开始!\n");
                    m_curBatchNo = 0;
                }

                if (Convert.ToBoolean(notify & 0x04))
                {
                    System.Console.Write("新批处理!\n");
                }
            }
        }

        private void Clear()
        {
            Monitor.Enter(monitor);
            this.listDist.RemoveRange(0, this.listDist.Count);
            this.listX.RemoveRange(0, this.listX.Count);
            this.listY.RemoveRange(0, this.listY.Count);
            Monitor.Exit(monitor);
        }
        private void getData(out double[] dist, out double[] dist2, out double[] thick, out double[] intensity, out double[] x, out double[] y, out double[] z)
        {
            bool isLock = false;
            int length = this.listDist.Count;
            dist = new double[length];
            dist2 = null;
            thick = null;
            intensity = null;
            x = new double[length];
            y = new double[length];
            z = new double[length];
            // 获取高度范围
            float MeasureRange = GetScalFull();
            /////////////////////////////////////
            Monitor.Enter(monitor, ref isLock); // 多线程操作下必需要加锁
            for (int i = 0; i < length; i++)
            {
                if (this.listDist[i] == -1000000000) // -1000000000:表示无效值
                    dist[i] = -9999;
                else
                    dist[i] = MeasureRange / 2.0f + this.listDist[i] * 0.00001f; // 数值从上往下读变大
                x[i] = this.listX[i];
                y[i] = this.listY[i];
                z[i] = 0;
            }
            if (isLock) Monitor.Exit(monitor);
        }

        private float GetScalFull()
        {
            IntPtr str_Model = SR7LinkFunc.SR7IF_GetModels(DeviceId);
            String s_model = Marshal.PtrToStringAnsi(str_Model);
            float heightRange = 0;
            if (s_model == "SR7050")
                heightRange = 2.5f * 2; // 表示全量程 
            else if (s_model == "SR7080")
                heightRange = 6 * 2;
            else if (s_model == "SR7140")
                heightRange = 12 * 2;
            else if (s_model == "SR7240")
                heightRange = 20 * 2;
            else if (s_model == "SR7400")
                heightRange = 50 * 2;
            return heightRange;
        }


        #region 实现接口
        public bool Connect(SensorConnectConfigParam configParam)
        {
            bool result = true;
            int value = -1; // 
            this.ConfigParam = configParam;
            this.Name = configParam.SensorName;
            this.LaserParam = (LaserParam)new LaserParam().Read(this.Name);
            this.LaserParam.SensorName = configParam.SensorName;
            switch (configParam.ConnectType)
            {
                case enUserConnectType.Network:
                    this.ipAdress = configParam.ConnectAddress;
                    ////////////
                    if (ipAdress == null || ipAdress.Trim() == "")
                    {
                        pEthernetConfig.abyIpAddress = new Byte[] { (byte)192, (byte)168, (byte)0, (byte)10 };
                        value = SR7LinkFunc.SR7IF_EthernetOpen(DeviceId, ref pEthernetConfig);
                    }
                    else
                    {
                        string[] charadress = ipAdress.Split('.');
                        pEthernetConfig.abyIpAddress = new Byte[] { (byte)Convert.ToByte(charadress[0]), (byte)Convert.ToByte(charadress[1]), (byte)Convert.ToByte(charadress[2]), (byte)Convert.ToByte(charadress[3]) };
                        value = SR7LinkFunc.SR7IF_EthernetOpen(DeviceId, ref pEthernetConfig);
                    }
                    if (value < 0)
                    {
                        b_connect = false;
                        result = false;
                    }
                    b_connect = true;

                    // 初始化高速通信 
                    int reT = SR7LinkFunc.SR7IF_HighSpeedDataEthernetCommunicationInitalize(DeviceId, ref pEthernetConfig, 0, _callback, 10, 0); // 10:表示每产生10个轮廓，调用一次回调函数
                    b_HighSpeedInitFail = false;
                    if (reT < 0)
                    {
                        b_HighSpeedInitFail = true;
                        MessageBox.Show("      高速数据以太网通信初始化失败", "提示", MessageBoxButtons.OK);
                        result = false;
                    }
                    //判断相机B是否在线
                    int RT = SR7LinkFunc.SR7IF_GetOnlineCameraB(DeviceId);
                    if (RT == 0)
                        b_camBOnline = true;
                    else
                        b_camBOnline = false;

                    ///初始化一些参数
                    IntPtr DataObject = new IntPtr();
                    BatchCallBackPoint = SR7LinkFunc.SR7IF_ProfilePointSetCount(DeviceId, DataObject); // 批处理的轮廓数
                    m_BatchWidth = SR7LinkFunc.SR7IF_ProfileDataWidth(DeviceId, DataObject); // 激光线上的轮廓点数
                    float m_XPitch = (float)SR7LinkFunc.SR7IF_ProfileData_XPitch(0, DataObject); // 激光线上的点间隔
                    this.X = new double[m_BatchWidth];
                    this.LaserParam.DataWidth = m_BatchWidth;
                    for (int i = 0; i < m_BatchWidth; i++)
                    {
                        this.X[i] = i * m_XPitch;
                    }
                    //////////////////////
                    result = true;
                    break;
                default:
                    result = false;
                    break;
            }
            configParam.ConnectState = result;
            return result;
        }

        public bool Disconnect()
        {
            //批处理停止
            int Rc = SR7LinkFunc.SR7IF_StopMeasure(DeviceId);
            //关闭设备
            int reT = SR7LinkFunc.SR7IF_CommClose(DeviceId);
            if (Rc == 0 && reT == 0) return true;
            return false;
        }

        public Dictionary<enDataItem, object> ReadData()
        {
            Dictionary<enDataItem, object> listdata = new Dictionary<enDataItem, object>();
            double[] dist;
            double[] dist2;
            double[] thick;
            double[] intensity;
            double[] x;
            double[] y;
            double[] z;
            ///////////////////////////////////////////////////////////////////////////
            getData(out dist, out dist2, out thick, out intensity, out x, out y, out z);
            listdata.Add(enDataItem.Dist1,dist);
            listdata.Add(enDataItem.Dist2, dist2);
            listdata.Add(enDataItem.Thick, thick);
            listdata.Add(enDataItem.Intensity, intensity);
            listdata.Add(enDataItem.X, x);
            listdata.Add(enDataItem.Y, y);
            listdata.Add(enDataItem.Z, z);
            ///////////////////////////
            return listdata;
        }

        public bool StartTrigger()
        {
            Clear();
            //开始批处理
            int Rc = -1;
            string userTriggerSource = "";
            switch (userTriggerSource.ToString().Trim())
            {
                case "软触发":
                    Rc = SR7LinkFunc.SR7IF_StartMeasure(DeviceId, 20000); // 软触发
                    break;
                case "外部IO触发":
                case "内部IO触发":
                case "NONE":
                    return true;

                default:
                    return false;
            }
            //if (b_IOTrigger)
            //    Rc = SR7LinkFunc.SR7IF_StartIOTriggerMeasure(DeviceId, 20000, 0);
            //else           
            acq_Complete = false;
            if (Rc < 0) return false;
            return true;
        }

        public bool StopTrigger()
        {
            //批处理停止
            int Rc = SR7LinkFunc.SR7IF_StopMeasure(DeviceId);
            return true;
        }

        public bool Init()
        {
            this.LaserParam.DataWidth = 3200;
            this.LaserParam.DataHeight = 1;
            return true;
        }

        public bool SetParam(object paramType, object value)
        {
            // 根据不同的参数选择不同的分支来设置 
            enSensorParamType type = (enSensorParamType)paramType;
            switch (type)
            {
                case enSensorParamType.Coom_每线点数:
                    this.LaserParam.DataWidth = (int)value;
                    break;
                default:
                    break;
            }
            return true;
        }

        public object GetParam(object paramType)
        {
            // 根据不同的参数选择不同的分支来设置 
            enSensorParamType type = (enSensorParamType)paramType;
            switch (type)
            {
                case enSensorParamType.Coom_每线点数:
                    return this.LaserParam.DataWidth;
                case enSensorParamType.Coom_传感器名称:
                    return this.LaserParam.SensorName;
                case enSensorParamType.Coom_传感器类型:
                    return SensorConnectConfigParamManger.Instance.GetSensorConfigParam(this.Name).SensorType; 
                default:
                    break;
            }
            return true;
        }
        #endregion
        private bool init()
        {
            // 初始化回调函数
            _callback = new HighSpeedDataCallBack(ReceiveHighSpeedData);
            return true;
        }


    }


    /// <summary>
    /// Object pinning class
    /// </summary>
    public sealed class PinnedObject : IDisposable
    {
        #region Field

        private GCHandle _Handle;      // Garbage collector handle

        #endregion

        #region Property

        /// <summary>
        /// Get the address.
        /// </summary>
        public IntPtr Pointer
        {
            // Get the leading address of the current object that is pinned.
            get { return _Handle.AddrOfPinnedObject(); }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="target">Target to protect from the garbage collector</param>
        public PinnedObject(object target)
        {
            // Pin the target to protect it from the garbage collector.
            _Handle = GCHandle.Alloc(target, GCHandleType.Pinned);
        }

        #endregion

        #region Interface
        /// <summary>
        /// Interface
        /// </summary>
        public void Dispose()
        {
            _Handle.Free();
            _Handle = new GCHandle();
        }

        #endregion
    }


}

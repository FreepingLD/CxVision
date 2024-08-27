using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO;
using System.Collections;
using System.Runtime.InteropServices;
using STIL_NET;
using Common;
using System.Windows.Forms;
using System.Collections.Concurrent;

namespace Sensor
{
    /// <summary>
    /// 模型对象，激光控制器相当于一个数据库
    /// </summary>
    public class StilPointLaser
    {
        private double measureRange;
        private int mMode = 0;
        private bool trigState;
        object monitor = new object();
        //声明一些常量
        public const float SEC_TO_MILLISEC = 1000.0f; //SEC :秒;MILLISEC:毫秒；表示秒到毫秒的转换
        public const float TIMEOUT_ACQUISITION_CONST = 500.0f;//数据采集超时                                                                
        //声明一些类的对象
        private static dll_chr m_dll_chr = null;
        private sensor m_sensor = null;
        private cAcqParamMeasurement AcqParamMeasurement = new cAcqParamMeasurement();
        private enSensorError sError = enSensorError.MCHR_ERROR_NONE;
        private sensorManager m_sensor_manager = new sensorManager();
        //创建事件对象
        private cNamedEvent m_measurement_event = new cNamedEvent();
        private cNamedEvent m_exit_event = new cNamedEvent();
        private cNamedEvent m_exit_event_do = new cNamedEvent();
        private StilPointSensorSetting stilPointParamConfig;
        private int acqNumber = 0;
        private LaserParam _LaserParam;

        /// </summary>
        //传感器是否打开
        public bool IsOpen { get; set; }
        //公开一个传感器对象
        public sensor Sensor
        {
            get
            {
                return m_sensor;
            }
            set
            {
                m_sensor = value;
            }
        }
        //  传感器的触发状态
        public bool TrigState
        {
            get
            {
                return trigState;
            }

            set
            {
                trigState = value;
            }
        }
        public int AcqNumber
        {
            get
            {
                return acqNumber;
            }

            set
            {
                acqNumber = value;
            }
        }
        public StilPointSensorSetting StilPointParamConfig { get => stilPointParamConfig; set => stilPointParamConfig = value; }
        public LaserParam LaserParam { get => _LaserParam; set => _LaserParam = value; }

        private bool isSave = false;


        //声明一个线程对象
        private Thread m_thread = null;
        //声明距离模式数据存储集合
        List<float> OutDistance = new List<float>();
        List<float> OutIntensity = new List<float>();
        //输出XYZ坐标 
        List<uint> OutEncoderX = new List<uint>();
        List<uint> OutEncoderY = new List<uint>();
        List<uint> OutEncoderZ = new List<uint>();
        //输出厚度数据
        List<float> OutThickness = new List<float>();
        //  List<float> OutDistance1 = new List<float>();
        List<float> OutDistance2 = new List<float>();
        List<float> OutIntensity2 = new List<float>();
        // 灰度中心
        List<float> OutBaryCenter = new List<float>();

        private ConcurrentQueue<float> List_Dist1 = new ConcurrentQueue<float>();
        private ConcurrentQueue<float> List_Dist2 = new ConcurrentQueue<float>();
        private ConcurrentQueue<float> List_Thick = new ConcurrentQueue<float>();
        private ConcurrentQueue<float> List_Intensity = new ConcurrentQueue<float>();
        private ConcurrentQueue<float> List_Intensity2 = new ConcurrentQueue<float>();
        private ConcurrentQueue<uint> List_X = new ConcurrentQueue<uint>();
        private ConcurrentQueue<uint> List_Y = new ConcurrentQueue<uint>();
        private ConcurrentQueue<uint> List_Z = new ConcurrentQueue<uint>();
        private ConcurrentQueue<float> List_BaryCenter = new ConcurrentQueue<float>();

        /// <summary>
        /// 初始化DLL
        /// </summary>
        /// <returns></returns>
        public bool InitDLL()
        {
            if (m_dll_chr == null)
                m_dll_chr = new dll_chr();//实例化一个对象，其包含有一个DLL。如果对象中不包含DLL，则返回找不到DLL，否显示DLL版本
            else
                return true;
            if (m_dll_chr.Init() == false)
            {
                Console.WriteLine("cExample : Error : DLL Init failed");
                return (false);
            }
            //Display DLL(s) versions               
            Console.WriteLine(string.Format("DLL_CHR.DLL :\t\t {0}", m_sensor_manager.DllChrVersion));
            Console.WriteLine(string.Format("STILSensors.DLL :\t {0}", m_sensor_manager.DllSensorsVersion));
            return (true);
        }

        //-------------------------------------------------------------------------------
        /// <summary>
        /// 连接传感器
        /// </summary>
        /// <param name="UsbDeviceName">连接传感器名称</param>
        /// <returns></returns>
        public bool ConnectUSB(string UsbDeviceName)
        {
            bool result = false;
            if (UsbDeviceName.Length < 17)
            {
                result = ConnectPrimaUsb(STIL_NET.enSensorType.CCS_PRIMA, UsbDeviceName);
                if (result)
                    this.measureRange = this.m_sensor.FullScale * 0.001f;
                return result;
            }
            else
            {
                result = ConnectOptimaUsb(STIL_NET.enSensorType.CCS_OPTIMA_PLUS, UsbDeviceName);
                if (result)
                    this.measureRange = this.m_sensor.FullScale * 0.001f;
                return result;
            }
        }
        //-------------------------------------------------------------------------------
        /// <summary>
        /// 连接传感器
        /// </summary>
        /// <param name="UsbDeviceName">连接传感器名称</param>
        /// <returns></returns>
        public bool ConnectSerial(STIL_NET.enSensorType SensorType, ushort SerialPort, uint BaudRate)
        {
            bool result = false;
            if (SensorType == STIL_NET.enSensorType.CCS_PRIMA)
            {
                result = ConnectPrimaSerial(STIL_NET.enSensorType.CCS_PRIMA, SerialPort, BaudRate);
                if (result)
                    this.measureRange = this.m_sensor.FullScale * 0.001f;
                return result;
            }
            else
            {
                result = ConnectOptimaSerial(STIL_NET.enSensorType.CCS_OPTIMA_PLUS, SerialPort, BaudRate);
                if (result)
                    this.measureRange = this.m_sensor.FullScale * 0.001f;
                return result;
            }
            // return result;
        }
        //-------------------------------------------------------------------------------
        /// <summary>
        /// 打开传感器连接
        /// </summary>
        /// <param name="sensorType">指定传感器的型号</param>
        /// <param name="UsbDeviceName">指定需要连接的传感器</param>
        /// <returns></returns>
        public bool ConnectEnternet(STIL_NET.enSensorType sensorType, string ip_address)
        {
            //open sensor.    sensorname :随便给什么都能连接，Usbdevicename：就不能随便给了
            m_sensor = m_sensor_manager.OpenEthernetconnection("ZENITH", sensorType, ip_address, null);  //使用某种方式打开传感器，如果打开成功则返回一个传感器对象，否则返回空，用来判断传感器是否打开
            if (m_sensor != null)
            {
                m_sensor.OnMeasureModeChange += new EventHandler(OnMeasureModeChang);
                m_sensor.OnError += new sensor.ErrorHandler(OnError);   //把一个方法作为参数传递一个委托类型即初始化一个委托类型实例(第一个OnError表示事件，第二个OnError表示方法）-（为什么不把委托类型单独作为一个类来放置？）
                m_sensor.OnUnPlugged += new EventHandler(OnConnect);
                this.measureRange = this.m_sensor.FullScale * 0.001f;
            }
            else
            {
                Console.WriteLine("cExample : Error : Open (No sensor or bad sensor)");   //没有传感器或传感器错误
                return false;
            }
            return true;
        }
        /// <summary>
        /// 设置传感器参数
        /// </summary>
        /// <param name="TriggerMode">触发模式</param>
        /// <param name="MeasureMode">测量模式</param>
        /// <param name="PointsTRE">TRE触发模式下的测量点数</param>
        /// <returns></returns>
        public bool SetParameter(enUserTrigerMode TriggerMode, STIL_NET.enMeasureMode MeasureMode, uint PointsTRE)
        {
            bool result = false;
            if (MeasureMode == STIL_NET.enMeasureMode.DISTANCE_MODE)
            {
                mMode = 0;
                result = SetDistParameter(TriggerMode, PointsTRE);
                return result;
            }
            if (MeasureMode == STIL_NET.enMeasureMode.THICKNESS_MODE)
            {
                mMode = 1;
                result = SetThicknessParameter(TriggerMode, PointsTRE);
                return result;
            }
            return result;
        }

        //--------------------------------------------------------------------------------
        /// <summary>
        /// 开始数据采集，如果启用的外部触发，则等待信号
        /// </summary>
        /// <param name="SwitchMode"></param>
        /// <returns></returns>
        public bool StartAcquisition()
        {
            Clear();// 开始采集前清空一次数据
            if (m_sensor != null)
            {
                m_exit_event.Reset();     //public bool Reset();将事件状态设置为非终止状态，导致线程阻止
                m_exit_event_do.Reset();    //public bool Set();将事件状态设置为终止状态，允许一个或多个等待线程继续
                sError = m_sensor.StartAcquisition_Measurement(AcqParamMeasurement);   //该函数用于生成采集结束事件（如果采集事件为N点采集事件，则生成对应的事件，如果为Buffer结束事件，则生成对应的事件），执行他，将有对应的事件生成
                if (sError != enSensorError.MCHR_ERROR_NONE)
                {
                    Console.WriteLine(string.Format("cExample : Error : StartAcquisition_Measurement : {0}", sError.ToString()));
                    return false;
                }
            }
            else
            {
                Console.WriteLine("cExample : Error : StartAcquisition (No sensor or bad sensor)");
                return false;
            }
            //创建线程并启动一个线程，在同一个类中调用方法时不需要类名
            if (m_sensor.MeasureMode == STIL_NET.enMeasureMode.DISTANCE_MODE)
            {
                m_thread = new Thread(new ThreadStart(ExecuteDistAcquisition));
                m_thread.IsBackground = true;
            }
            if (m_sensor.MeasureMode == STIL_NET.enMeasureMode.THICKNESS_MODE)
            {
                m_thread = new Thread(new ThreadStart(ExecuteThickAcquisition));
                m_thread.IsBackground = true;
            }
            m_thread.Start();
            return true;
        }

        //---------------------------------------------------------------------------------
        /// <summary>
        /// 停止传感器采集，生成一个测量结束事件
        /// </summary>
        /// <returns></returns>
        public bool StopAcquisition()
        {
            bool Result = false;
            if (m_sensor != null)
            {
                if (m_sensor.StopAcquisition_Measurement())
                    Result = true; //该函数用于生成结束采集事件，执行他，将有对应的采集事件生成
            }
            else
            {
                Console.WriteLine("cExample : Error : StopAcquisition (No sensor or bad sensor)");
                Result = false;
            }
            if (m_thread != null)
                m_thread.Abort();
            return Result;
        }

        //---------------------------------------------------------------------------------
        /// <summary>
        /// 断开传感器连接
        /// </summary>
        /// <returns></returns>
        public bool Disconnect()
        {
            if (m_sensor != null)
            {
                m_sensor.Close();
                m_sensor.OnMeasureModeChange -= new EventHandler(OnMeasureModeChang);
                m_sensor.OnError -= new sensor.ErrorHandler(OnError);   //把一个方法作为参数传递一个委托类型即初始化一个委托类型实例(第一个OnError表示事件，第二个OnError表示方法）-（为什么不把委托类型单独作为一个类来放置？）
                m_sensor.OnUnPlugged -= new EventHandler(OnConnect);
            }
            else
            {
                Console.WriteLine("cExample : Error : Close (No sensor or bad sensor)");
                return false;
            }
            IsOpen = false;
            Sensor = null;
            return true;
        }
        //---------------------------------------------------------------------------------
        /// <summary>
        /// 释放传感器和DLL
        /// </summary>
        /// <returns></returns>
        public bool ReleaseSensorDLL()
        {
            if (m_sensor != null)
            {
                m_sensor.Release();
            }
            m_dll_chr.Release();
            return (true);
        }

        //---------------------------------------------------------------------------------------------------------
        /// <summary>
        /// 软件触发采集,功能类似于发送STR指令，该函数里面应该包含了发送“STR”指令
        /// </summary>
        public bool SoftwareTrigger()
        {
            //必需对指令是否发出进行验证，最好是用标志位来做
            if (m_sensor.StartAcquisition() == enSensorError.MCHR_ERROR_NONE) return true;
            else return false;
            //用于在触发模式下，软触发采集，相当于发送一个软触发命令，//用该指令来执行软触发采集，采集的数与设置数相同，只少个一个
        }
        //---------------------------------------------------------------------------------------------------------
        /// <summary>
        /// TRS模式下开始触发
        /// </summary>
        /// <returns></returns>
        public bool StartTriggerTRS(object trigSource)
        {
            this.isSave = false;
            Clear(); // 开始触发前清空数据
            Thread.Sleep(10);
            enUserTriggerSource triggerSource;
            if (Enum.TryParse(trigSource.ToString().Trim(), out triggerSource))
            {
                switch (triggerSource)
                {
                    case enUserTriggerSource.软触发:
                        this.isSave = true;
                        if (this.m_sensor != null && m_sensor.StartAcquisition() == enSensorError.MCHR_ERROR_NONE)
                            return true;
                        else
                            return false;
                    case enUserTriggerSource.NONE:
                    case enUserTriggerSource.内部IO触发:
                    case enUserTriggerSource.外部IO触发:
                        this.isSave = true;
                        return true;
                    default:
                        return false;
                }
            }
            else
                return false;
        }

        public void StartGrab()
        {
            this.isSave = false;
            this.Clear(); // 开始触发前清空数据
            Thread.Sleep(10);
            this.isSave = true;
        }
        //---------------------------------------------------------------------------------------------------------

        /// <summary>
        /// TRS模式下停止触发
        /// </summary>
        /// <returns></returns>
        public bool StopTriggerTRS(object trigSource, int acqNum)
        {
            this.isSave = true;
            enUserTriggerSource triggerSource;
            if (Enum.TryParse(trigSource.ToString().Trim(), out triggerSource))
            {
                switch (triggerSource)
                {
                    case enUserTriggerSource.软触发:
                        this.isSave = false;
                        if (stilPointParamConfig.TrigMode == enUserTrigerMode.TRS)
                        {
                            if (this.m_sensor != null && m_sensor.StartAcquisition() == enSensorError.MCHR_ERROR_NONE)
                                return true;
                            else
                                return false;
                        }
                        else
                            return true;
                    case enUserTriggerSource.NONE:
                    case enUserTriggerSource.内部IO触发:
                        this.isSave = false;
                        return true;
                    case enUserTriggerSource.外部IO触发: // 当触发源为外部时
                        while (stilPointParamConfig.AcqCount < this.OutDistance.Count)
                        {
                            if (stilPointParamConfig.CancelWaite) break;
                            Application.DoEvents();
                        }
                        this.isSave = false;
                        return true;
                    default:
                        return false;
                }
            }
            else
                return false;
        }
        public void StopGrab()
        {
            this.isSave = false;
        }

        /// <summary>
        /// 设置编码器的参考零点
        /// </summary>
        public void RecenterEncoders()
        {
            m_sensor.RecenterEncoders(true, true, true);
        }

        /// <summary>
        /// 获取电脑上的传感器名称
        /// </summary>
        /// <returns></returns>
        public string[] GetSensorName()
        {
            string[] Name = sensor.UsbDeviceList;
            return Name;
        }

        /// <summary>
        /// 获取测量数据
        /// </summary>
        /// <param name="Distance"></param>
        /// <param name="Thickness"></param>
        /// <param name="Distance1"></param>
        /// <param name="Distance2"></param>
        /// <param name="EncoderX"></param>
        /// <param name="EncoderY"></param>
        /// <param name="EncoderZ"></param>
        /// <param name="EncoderResolution">光栅尺的分辨率</param>
        public void GetData(out double[] Distance, out double[] Distance2, out double[] Thickness, out double[] Intensity, out double[] EncoderX, out double[] EncoderY, out double[] EncoderZ, out double[] BaryCenter)
        {
            Distance = null;
            Thickness = null;
            Distance2 = null;
            EncoderX = null;
            EncoderY = null;
            EncoderZ = null;
            BaryCenter = null;
            Intensity = null;
            if (mMode == 0)
                GetDistData(out Distance, out Intensity, out EncoderX, out EncoderY, out EncoderZ, out BaryCenter);
            if (mMode == 1)
                GetThickData(out Thickness, out Distance, out Distance2, out Intensity, out EncoderX, out EncoderY, out EncoderZ, out BaryCenter);
        }


        private void Clear()
        {
            Monitor.Enter(monitor);
            OutThickness.RemoveRange(0, OutThickness.Count);
            OutDistance.RemoveRange(0, OutDistance.Count);
            OutDistance2.RemoveRange(0, OutDistance2.Count);
            OutEncoderX.RemoveRange(0, OutEncoderX.Count);
            OutEncoderY.RemoveRange(0, OutEncoderY.Count);
            OutEncoderZ.RemoveRange(0, OutEncoderZ.Count);
            OutBaryCenter.RemoveRange(0, OutBaryCenter.Count);
            OutIntensity.RemoveRange(0, OutIntensity.Count);
            OutIntensity2.RemoveRange(0, OutIntensity.Count);

            float value;
            uint coord;
            for (int i = 0; i < List_Dist1.Count; i++)
                List_Dist1.TryDequeue(out value);
            for (int i = 0; i < List_Dist2.Count; i++)
                List_Dist2.TryDequeue(out value);
            for (int i = 0; i < List_Thick.Count; i++)
                List_Thick.TryDequeue(out value);
            for (int i = 0; i < List_Intensity.Count; i++)
                List_Intensity.TryDequeue(out value);
            for (int i = 0; i < List_Intensity2.Count; i++)
                List_Intensity2.TryDequeue(out value);
            for (int i = 0; i < List_BaryCenter.Count; i++)
                List_BaryCenter.TryDequeue(out value);
            for (int i = 0; i < List_X.Count; i++)
                List_X.TryDequeue(out coord);
            for (int i = 0; i < List_Y.Count; i++)
                List_Y.TryDequeue(out coord);
            for (int i = 0; i < List_Z.Count; i++)
                List_Z.TryDequeue(out coord);
            this.acqNumber = 0;
            Monitor.Exit(monitor);
        }


        //-------------------------------------------------------------------------------
        /// <summary>
        /// 一次性获取存储在内存中的数据 
        /// </summary>
        /// <param name="Distance">获取距离值></param>
        /// <param name="Encoder1">获取编码器1</param>
        /// <param name="Encoder2">获取编码器2</param>
        /// <param name="Encoder3">获取编码器3</param>
        /// <param name="Intensity">获取光强值</param>
        private void GetThickData(out double[] Thickness, out double[] Distance, out double[] Distance2, out double[] Intensity, out double[] Encoder1, out double[] Encoder2, out double[] Encoder3, out double[] BaryCenter)
        {
            Thread.Sleep(200);
            //声明数据存放区
            List<double> ListThickness = new List<double>();
            List<double> ListDistance = new List<double>();
            List<double> ListDistance2 = new List<double>();
            List<double> ListBaryCenter = new List<double>();
            List<double> ListEncoderX = new List<double>();
            List<double> ListEncoderY = new List<double>();
            List<double> ListEncoderZ = new List<double>();
            List<double> ListIntensity = new List<double>();
            ///////////////////////////////////////////////         
            bool lockTaken = false;
            try
            {
                Monitor.Enter(monitor, ref lockTaken);
                int[] arrayLength = new int[8] { List_Thick.Count, List_Dist1.Count, List_Dist2 .Count, List_Intensity .Count, List_BaryCenter .Count,
                List_X.Count,List_Y.Count,List_Z.Count};
                int length = arrayLength.Min();
                for (int i = 0; i < length; i++)
                {
                    float value;
                    List_Thick.TryDequeue(out value);
                    ListThickness.Add(value * 0.001);
                    List_Dist1.TryDequeue(out value);
                    ListDistance.Add(measureRange - value * 0.001);
                    List_Dist2.TryDequeue(out value);
                    ListDistance2.Add(measureRange - value * 0.001);
                    List_BaryCenter.TryDequeue(out value);
                    ListBaryCenter.Add(value);
                    List_Intensity.TryDequeue(out value);
                    ListIntensity.Add(value);
                    List_Intensity2.TryDequeue(out value);
                }
                for (int i = 0; i < length; i++)
                {
                    if (this.LaserParam.Enable_x)
                    {
                        uint value;
                        List_X.TryDequeue(out value);
                        ListEncoderX.Add((Convert.ToInt32(OutEncoderX[i]) - 536870912) * this.LaserParam.Resolution_X);
                        //ListEncoderX.Add((Convert.ToInt32(OutEncoderX[i]) - Convert.ToInt32(OutEncoderX[0])) * this.LaserParam.Resolution_X);
                    }
                    else
                        ListEncoderX.Add(0);
                    ////////////////////////////////////////////////////////////
                    if (this.LaserParam.Enable_y)
                    {
                        uint value;
                        List_Y.TryDequeue(out value);
                        ListEncoderY.Add((Convert.ToInt32(OutEncoderY[i]) - 536870912) * this.LaserParam.Resolution_Y);
                        //ListEncoderY.Add((Convert.ToInt32(OutEncoderY[i]) - Convert.ToInt32(OutEncoderY[0])) * this.LaserParam.Resolution_Y);
                    }
                    else
                        ListEncoderY.Add(0);
                    ///////////////////////////////////////////////////
                    if (this.LaserParam.Enable_z)
                    {
                        uint value;
                        List_Z.TryDequeue(out value);
                        ListEncoderZ.Add((Convert.ToInt32(OutEncoderY[i]) - 536870912) * this.LaserParam.Resolution_Y);
                        //ListEncoderZ.Add((Convert.ToInt32(OutEncoderZ[i]) - Convert.ToInt32(OutEncoderZ[0])) * this.LaserParam.Resolution_Z);
                    }
                    else
                        ListEncoderZ.Add(0);
                }
                //将集合转换成数组输出
                Thickness = ListThickness.ToArray();
                Distance = ListDistance.ToArray();
                Distance2 = ListDistance2.ToArray();
                Encoder1 = ListEncoderX.ToArray();
                Encoder2 = ListEncoderY.ToArray();
                Encoder3 = ListEncoderZ.ToArray();
                BaryCenter = ListBaryCenter.ToArray();
                Intensity = ListIntensity.ToArray();
                //获取集合中的数据后对它进行清空
                OutThickness.RemoveRange(0, OutThickness.Count);
                OutDistance.RemoveRange(0, OutDistance.Count);
                OutDistance2.RemoveRange(0, OutDistance2.Count);
                OutEncoderX.RemoveRange(0, OutEncoderX.Count);
                OutEncoderY.RemoveRange(0, OutEncoderY.Count);
                OutEncoderZ.RemoveRange(0, OutEncoderZ.Count);
                OutBaryCenter.RemoveRange(0, OutBaryCenter.Count);
                OutIntensity.RemoveRange(0, OutIntensity.Count);
                ////////////
                this.Clear();
            }
            finally
            {
                ////////////
                this.Clear();
                //获取集合中的数据后对它进行清空
                OutThickness.RemoveRange(0, OutThickness.Count);
                OutDistance.RemoveRange(0, OutDistance.Count);
                OutDistance2.RemoveRange(0, OutDistance2.Count);
                OutEncoderX.RemoveRange(0, OutEncoderX.Count);
                OutEncoderY.RemoveRange(0, OutEncoderY.Count);
                OutEncoderZ.RemoveRange(0, OutEncoderZ.Count);
                OutBaryCenter.RemoveRange(0, OutBaryCenter.Count);
                OutIntensity.RemoveRange(0, OutIntensity.Count);
                if (lockTaken) Monitor.Exit(monitor);
            }
        }

        //-------------------------------------------------------------------------------
        /// <summary>
        /// 一次性获取测量数据
        /// </summary>
        /// <param name="invert">是否取反Z值></param>
        /// <param name="Offset">将Z方向数据进行平移></param>
        /// <param name="Distance">获取距离值></param>
        /// <param name="EncoderX">获取编码器1</param>
        /// <param name="EncoderY">获取编码器2</param>
        /// <param name="EncoderZ">获取编码器3</param>
        /// <param name="Intensity">获取光强值</param>
        private void GetDistData(out double[] Distance, out double[] Intensity, out double[] EncoderX, out double[] EncoderY, out double[] EncoderZ, out double[] BaryCenter)
        {
            Thread.Sleep(200);
            //声明数据存放区
            List<double> ListDistance = new List<double>();
            List<double> ListEncoderX = new List<double>();
            List<double> ListEncoderY = new List<double>();
            List<double> ListEncoderZ = new List<double>();
            List<double> ListIntensity = new List<double>();
            List<double> ListGrayCenter = new List<double>();
            Distance = new double[0];
            Intensity = new double[0];
            EncoderX = new double[0];
            EncoderY = new double[0];
            EncoderZ = new double[0];
            BaryCenter = new double[0];
            bool lockTaken = false;
            try
            {
                Monitor.Enter(monitor, ref lockTaken);
                if (!lockTaken) return;
                int[] arrayLength = new int[8] { List_Dist1.Count, List_Dist2.Count, List_Thick .Count, List_Intensity .Count, List_BaryCenter .Count,
                List_X.Count,List_Y.Count,List_Z.Count};
                int length = arrayLength.Min();
                for (int i = 0; i < length; i++)
                {
                    float value;
                    List_Dist1.TryDequeue(out value);
                    if (this.stilPointParamConfig.IsMirrorZ)
                        ListDistance.Add(value * -0.001); //measureRange-
                    else
                        ListDistance.Add(value * 0.001);
                    List_BaryCenter.TryDequeue(out value);
                    ListGrayCenter.Add(value);
                    List_Intensity.TryDequeue(out value);
                    ListIntensity.Add(value);

                    //if (OutDistance[i] == 0)
                    //{
                    //    ListDistance.Add(-9999); //measureRange-
                    //    ListGrayCenter.Add(0);
                    //    ListIntensity.Add(0);
                    //}
                    //else
                    //{
                    //    if (this.stilPointParamConfig.IsMirrorZ)
                    //        ListDistance.Add((OutDistance[i] * -0.001)); //measureRange-
                    //    else
                    //        ListDistance.Add((OutDistance[i] * 0.001));
                    //    ListGrayCenter.Add(OutBaryCenter[i]);
                    //    ListIntensity.Add(OutIntensity[i]);
                    //}
                }
                for (int i = 0; i < length; i++)
                {
                    if (this.LaserParam.Enable_x)
                    {
                        uint value;
                        List_X.TryDequeue(out value);
                        ListEncoderX.Add((Convert.ToInt32(OutEncoderX[i]) - 536870912) * this.LaserParam.Resolution_X);
                        //ListEncoderX.Add((Convert.ToInt32(OutEncoderX[i]) - Convert.ToInt32(OutEncoderX[0])) * this.LaserParam.Resolution_X);
                    }
                    else
                        ListEncoderX.Add(0);
                    ////////////////////////////////////////////////////////////
                    if (this.LaserParam.Enable_y)
                    {
                        uint value;
                        List_Y.TryDequeue(out value);
                        ListEncoderY.Add((Convert.ToInt32(OutEncoderY[i]) - 536870912) * this.LaserParam.Resolution_Y);
                        //ListEncoderY.Add((Convert.ToInt32(OutEncoderY[i]) - Convert.ToInt32(OutEncoderY[0])) * this.LaserParam.Resolution_Y);
                    }
                    else
                        ListEncoderY.Add(0);
                    ///////////////////////////////////////////////////
                    if (this.LaserParam.Enable_z)
                    {
                        uint value;
                        List_Z.TryDequeue(out value);
                        ListEncoderZ.Add((Convert.ToInt32(OutEncoderY[i]) - 536870912) * this.LaserParam.Resolution_Y);
                        //ListEncoderZ.Add((Convert.ToInt32(OutEncoderZ[i]) - Convert.ToInt32(OutEncoderZ[0])) * this.LaserParam.Resolution_Z);
                    }
                    else
                        ListEncoderZ.Add(0);
                }
                //将集合转换成数组输出
                Distance = ListDistance.ToArray();
                EncoderX = ListEncoderX.ToArray();
                EncoderY = ListEncoderY.ToArray();
                EncoderZ = ListEncoderZ.ToArray();
                BaryCenter = ListGrayCenter.ToArray();
                Intensity = ListIntensity.ToArray();
                //获取集合中的数据后对它进行清空
                OutDistance.RemoveRange(0, OutDistance.Count);
                OutEncoderX.RemoveRange(0, OutEncoderX.Count);
                OutEncoderY.RemoveRange(0, OutEncoderY.Count);
                OutEncoderZ.RemoveRange(0, OutEncoderZ.Count);
                OutIntensity.RemoveRange(0, OutIntensity.Count);
                OutBaryCenter.RemoveRange(0, OutBaryCenter.Count);
                OutIntensity.RemoveRange(0, OutIntensity.Count);
            }
            catch (Exception e)
            {
                Distance = ListDistance.ToArray();
                EncoderX = ListEncoderX.ToArray();
                EncoderY = ListEncoderY.ToArray();
                EncoderZ = ListEncoderZ.ToArray();
                BaryCenter = ListGrayCenter.ToArray();
                Intensity = ListIntensity.ToArray();
                // MessageBox.Show("获取数据出错");
            }
            finally
            {
                ////////////
                this.Clear();
                //获取集合中的数据后对它进行清空
                OutDistance.RemoveRange(0, OutDistance.Count);
                OutEncoderX.RemoveRange(0, OutEncoderX.Count);
                OutEncoderY.RemoveRange(0, OutEncoderY.Count);
                OutEncoderZ.RemoveRange(0, OutEncoderZ.Count);
                OutIntensity.RemoveRange(0, OutIntensity.Count);
                OutBaryCenter.RemoveRange(0, OutBaryCenter.Count);
                OutIntensity.RemoveRange(0, OutIntensity.Count);
                if (lockTaken) Monitor.Exit(monitor);

            }
        }

        //------------------------------------------------------------------------------- Serial
        /// <summary>
        /// 打开传感器连接
        /// </summary>
        /// <param name="sensorType">指定传感器的型号</param>
        /// <param name="UsbDeviceName">指定需要连接的传感器</param>
        /// <returns></returns>
        private bool ConnectPrimaUsb(STIL_NET.enSensorType sensorType, string UsbDeviceName)
        {
            //open sensor.    sensorname :随便给什么都能连接，Usbdevicename：就不能随便给了
            m_sensor = m_sensor_manager.OpenUsbConnection("", sensorType, UsbDeviceName, null);  //使用某种方式打开传感器，如果打开成功则返回一个传感器对象，否则返回空，用来判断传感器是否打开
            if (m_sensor != null)
            {
                m_sensor.OnError += new sensor.ErrorHandler(OnError);   //把一个方法作为参数传递一个委托类型即初始化一个委托类型实例(第一个OnError表示事件，第二个OnError表示方法）-（为什么不把委托类型单独作为一个类来放置？）
                m_sensor.OnMeasureModeChange += new EventHandler(OnMeasureModeChang);
                m_sensor.OnUnPlugged += new EventHandler(OnConnect);
            }
            else
            {
                Console.WriteLine("cExample : Error : Open (No sensor or bad sensor)");   //没有传感器或传感器错误
                return false;
            }
            IsOpen = true;
            return true;
        }
        //-------------------------------------------------------------------------------
        /// <summary>
        /// 打开传感器连接
        /// </summary>
        /// <param name="sensorType">指定传感器的型号</param>
        /// <param name="UsbDeviceName">指定需要连接的传感器</param>
        /// <returns></returns>
        private bool ConnectOptimaUsb(STIL_NET.enSensorType sensorType, string UsbDeviceName)
        {
            //open sensor.    sensorname :随便给什么都能连接，Usbdevicename：就不能随便给了
            m_sensor = m_sensor_manager.OpenUsbConnection("", sensorType, UsbDeviceName, null);  //使用某种方式打开传感器，如果打开成功则返回一个传感器对象，否则返回空，用来判断传感器是否打开
            if (m_sensor != null)
            {
                m_sensor.OnError += new sensor.ErrorHandler(OnError);   //把一个方法作为参数传递一个委托类型即初始化一个委托类型实例(第一个OnError表示事件，第二个OnError表示方法）-（为什么不把委托类型单独作为一个类来放置？）
                m_sensor.OnMeasureModeChange += new EventHandler(OnMeasureModeChang);
                m_sensor.OnUnPlugged += new EventHandler(OnConnect);
            }
            else
            {
                Console.WriteLine("cExample : Error : Open (No sensor or bad sensor)");   //没有传感器或传感器错误
                return false;
            }
            IsOpen = true;
            return true;
        }

        //------------------------------------------------------------------------------- 
        /// <summary>
        /// 打开传感器连接
        /// </summary>
        /// <param name="sensorType">指定传感器的型号</param>
        /// <param name="UsbDeviceName">指定需要连接的传感器</param>
        /// <returns></returns>
        private bool ConnectPrimaSerial(STIL_NET.enSensorType sensorType, ushort SerialPort, uint BaudRate)
        {
            //open sensor.    sensorname :随便给什么都能连接，Usbdevicename：就不能随便给了
            //  m_sensor = m_sensor_manager.OpenUsbConnection("", sensorType, UsbDeviceName, null);  //使用某种方式打开传感器，如果打开成功则返回一个传感器对象，否则返回空，用来判断传感器是否打开

            m_sensor = m_sensor_manager.OpenSerialConnection("", sensorType, SerialPort, BaudRate, null);
            if (m_sensor != null)
            {
                m_sensor.OnError += new sensor.ErrorHandler(OnError);   //把一个方法作为参数传递一个委托类型即初始化一个委托类型实例(第一个OnError表示事件，第二个OnError表示方法）-（为什么不把委托类型单独作为一个类来放置？）
                m_sensor.OnMeasureModeChange += new EventHandler(OnMeasureModeChang);
                m_sensor.OnUnPlugged += new EventHandler(OnConnect);
            }
            else
            {
                Console.WriteLine("cExample : Error : Open (No sensor or bad sensor)");   //没有传感器或传感器错误
                return false;
            }
            IsOpen = true;
            return true;
        }
        //-------------------------------------------------------------------------------
        /// <summary>
        /// 打开传感器连接
        /// </summary>
        /// <param name="sensorType">指定传感器的型号</param>
        /// <param name="UsbDeviceName">指定需要连接的传感器</param>
        /// <returns></returns>
        private bool ConnectOptimaSerial(STIL_NET.enSensorType sensorType, ushort SerialPort, uint BaudRate)
        {
            //open sensor.    sensorname :随便给什么都能连接，Usbdevicename：就不能随便给了
            // m_sensor = m_sensor_manager.OpenUsbConnection("", sensorType, UsbDeviceName, null);  //使用某种方式打开传感器，如果打开成功则返回一个传感器对象，否则返回空，用来判断传感器是否打开

            m_sensor = m_sensor_manager.OpenSerialConnection("", sensorType, SerialPort, BaudRate, null);
            if (m_sensor != null)
            {
                m_sensor.OnError += new sensor.ErrorHandler(OnError);   //把一个方法作为参数传递一个委托类型即初始化一个委托类型实例(第一个OnError表示事件，第二个OnError表示方法）-（为什么不把委托类型单独作为一个类来放置？）
                m_sensor.OnMeasureModeChange += new EventHandler(OnMeasureModeChang);
                m_sensor.OnUnPlugged += new EventHandler(OnConnect);
            }
            else
            {
                Console.WriteLine("cExample : Error : Open (No sensor or bad sensor)");   //没有传感器或传感器错误
                return false;
            }
            IsOpen = true;
            Sensor = m_sensor;
            return true;
        }

        //-------------------------------------------------------------------------------
        /// <summary>
        /// 定义测量中的错误事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnError(object sender, cErrorEventArgs e)
        {
            Console.WriteLine("cExample : Error : {0}-{1}{2}", e.Exception.ErrorType, e.Exception.FunctionName, e.Exception.ErrorDetail);
        }
        private void OnMeasureModeChang(object sender, EventArgs e)
        {
            StopAcquisition();
            SetParameter(this.stilPointParamConfig.TrigMode, this.m_sensor.MeasureMode, (uint)this.stilPointParamConfig.AcqCount);
            StartAcquisition();
        }
        /// <summary>
        /// 定义测量中的错误事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnConnect(object sender, EventArgs e)
        {
            MessageBox.Show("有Stil控制器出现了悼线");
        }
        //------------------------------------------------------------------------------------------------
        /// /// <summary>
        /// 设置OPTIMA采集参数
        /// </summary>
        /// <param name="TriggerMode">触发模式</param>
        /// <param name="Frequency">设置采集频率</param>
        /// <param name="PointsTRE">TRE触发模式下，每次触发采集的点数</param>
        /// <returns></returns>
        private bool SetDistParameter(enUserTrigerMode TriggerMode, uint PointsTRE)
        {
            bool irn = m_sensor.InvertDistance;
            //m_sensor.MeasureMode = STIL_NET.enMeasureMode.DISTANCE_MODE;
            // 设置采集频率
            if (AcqParamMeasurement.Init(m_sensor) == enSensorError.MCHR_ERROR_NONE)     //判断传感器是否连接上,使用传感器来初始AcqParamMeasurement对象，可将一些必要的参数传给它
            {
                AcqParamMeasurement.BufferLength = 100;
                AcqParamMeasurement.NumberOfBuffers = 500;
                // 启用数据输出数绶冲区,启用多少个数据输出，在获取数据的时候也要设置对应的个数
                AcqParamMeasurement.EnableBufferAltitude.Altitude = true; // 因为获取的是高度Buffer中的数据，所以是高度值
                AcqParamMeasurement.EnableBufferAltitude.Intensity = true;
                AcqParamMeasurement.EnableBufferAltitude.BaryCenter = true;
                AcqParamMeasurement.EnableEncoder.Encoder1 = true;
                AcqParamMeasurement.EnableEncoder.Encoder2 = true;
                AcqParamMeasurement.EnableEncoder.Encoder3 = true;

                //设置采集延时；set timeout acquisition : should be at least = ((BufferLength * averaging) / rate) * 1000 + 100
                AcqParamMeasurement.Timeout = (uint)((float)AcqParamMeasurement.NumberOfPointsBeforeSignal / (float)AcqParamMeasurement.Frequency) + (uint)(TIMEOUT_ACQUISITION_CONST + ((SEC_TO_MILLISEC * Convert.ToSingle(m_sensor.Averaging) / Convert.ToSingle(AcqParamMeasurement.Frequency)) * Convert.ToSingle(AcqParamMeasurement.BufferLength)));
                //定义采集事件类型，event type (here end of measurements) and callback function  事件类型(这里是测量的结束)和回调函数,这两个参数是配对使用的
                AcqParamMeasurement.EnableEvent.EventAcquireNPoints = true;  //连续采集模式，只有EventAcquireNPoints和EventEndBuffer这两个事件起作用，相当于设置FuncEventMeasurement函数中的EV变量赋值
                AcqParamMeasurement.NumberOfPointsBeforeSignal = 1;
                //设置采集延时；set timeout acquisition : should be at least = ((BufferLength * averaging) / rate) * 1000 + 100
                //AcqParamMeasurement.Timeout = (uint)SEC_TO_MILLISEC * (AcqParamMeasurement.NumberOfPointsBeforeSignal / (uint)AcqParamMeasurement.Frequency) + (uint)(TIMEOUT_ACQUISITION_CONST + ((SEC_TO_MILLISEC * Convert.ToSingle(m_sensor.Averaging) / Convert.ToSingle(AcqParamMeasurement.Frequency)) * Convert.ToSingle(AcqParamMeasurement.BufferLength)));
                //给事件注册一个方法 
                m_sensor.OnEventMeasurement += new sensor.OnEventMeasurementHandler(FuncEventMeasurement);   //给事件注册一个委托方法,事件的本质是一个封装了的委托类型对象
                                                                                                             //设置触发模式
                switch (TriggerMode)
                {
                    case enUserTrigerMode.NONE:

                        AcqParamMeasurement.Trigger.Enable = false;
                        AcqParamMeasurement.Trigger.Type = STIL_NET.enTriggerType.NONE;
                        trigState = false;
                        break;
                    case enUserTrigerMode.TRG:
                        AcqParamMeasurement.Trigger.Enable = true;
                        AcqParamMeasurement.Trigger.Type = STIL_NET.enTriggerType.TRG;
                        if (this.stilPointParamConfig.LevelEdgeFlag == enUserLevelEdgeFlag.FALLING_EDGE)
                            AcqParamMeasurement.Trigger.HighLevelOrRisingEdgeActivated = enLevelEdgeFlag.FALLING_EDGE; // HIGH LEVEL
                        else
                            AcqParamMeasurement.Trigger.HighLevelOrRisingEdgeActivated = enLevelEdgeFlag.RISING_EDGE; // HIGH LEVEL
                                                                                                                      //if (m_sensor.SensorType == STIL_NET.enSensorType.STIL_ZENITH)
                                                                                                                      //{
                                                                                                                      //    sZenithTriggerParameters trigZenith = (sZenithTriggerParameters)AcqParamMeasurement.Trigger;
                                                                                                                      //    trigZenith.Source = enTriggerSource.TRIGGER_SOURCE_EXTERNAL;
                                                                                                                      //}
                        trigState = true;
                        break;
                    case enUserTrigerMode.TRN:
                        AcqParamMeasurement.Trigger.Enable = true;
                        AcqParamMeasurement.Trigger.Type = STIL_NET.enTriggerType.TRN;
                        if (this.stilPointParamConfig.LevelEdgeFlag == enUserLevelEdgeFlag.FALLING_EDGE)
                            AcqParamMeasurement.Trigger.HighLevelOrRisingEdgeActivated = enLevelEdgeFlag.FALLING_EDGE; // HIGH LEVEL
                        else
                            AcqParamMeasurement.Trigger.HighLevelOrRisingEdgeActivated = enLevelEdgeFlag.RISING_EDGE; // HIGH LEVEL
                                                                                                                      //if (m_sensor.SensorType == STIL_NET.enSensorType.STIL_ZENITH)
                                                                                                                      //{
                                                                                                                      //    sZenithTriggerParameters trigZenith = (sZenithTriggerParameters)AcqParamMeasurement.Trigger;
                                                                                                                      //    trigZenith.Source = enTriggerSource.TRIGGER_SOURCE_EXTERNAL;
                                                                                                                      //}
                        trigState = true;
                        break;
                    case enUserTrigerMode.TRS:
                        AcqParamMeasurement.Trigger.Enable = true;
                        AcqParamMeasurement.Trigger.Type = STIL_NET.enTriggerType.TRS;
                        if (this.stilPointParamConfig.LevelEdgeFlag == enUserLevelEdgeFlag.FALLING_EDGE)
                            AcqParamMeasurement.Trigger.HighLevelOrRisingEdgeActivated = enLevelEdgeFlag.FALLING_EDGE; // HIGH LEVEL
                        else
                            AcqParamMeasurement.Trigger.HighLevelOrRisingEdgeActivated = enLevelEdgeFlag.RISING_EDGE; // HIGH LEVEL
                                                                                                                      //if (m_sensor.SensorType == STIL_NET.enSensorType.STIL_ZENITH)
                                                                                                                      //{
                                                                                                                      //    sZenithTriggerParameters trigZenith = (sZenithTriggerParameters)AcqParamMeasurement.Trigger;
                                                                                                                      //    trigZenith.Source = enTriggerSource.TRIGGER_SOURCE_EXTERNAL;
                                                                                                                      //}
                        trigState = true;
                        break;
                    case enUserTrigerMode.TRE:
                        AcqParamMeasurement.Trigger.Enable = true;
                        AcqParamMeasurement.Trigger.Type = STIL_NET.enTriggerType.TRE;
                        if (this.stilPointParamConfig.LevelEdgeFlag == enUserLevelEdgeFlag.FALLING_EDGE)
                            AcqParamMeasurement.Trigger.HighLevelOrRisingEdgeActivated = enLevelEdgeFlag.FALLING_EDGE; // HIGH LEVEL
                        else
                            AcqParamMeasurement.Trigger.HighLevelOrRisingEdgeActivated = enLevelEdgeFlag.RISING_EDGE; // HIGH LEVEL
                        AcqParamMeasurement.NumberPointsTRE = PointsTRE;
                        //if (m_sensor.SensorType == STIL_NET.enSensorType.STIL_ZENITH)
                        //{
                        //    sZenithTriggerParameters trigZenith = (sZenithTriggerParameters)AcqParamMeasurement.Trigger;
                        //    trigZenith.Source = enTriggerSource.TRIGGER_SOURCE_EXTERNAL;
                        //}
                        trigState = true;
                        break;
                    default:
                        trigState = false;
                        break;
                }
            }
            else
            {
                Console.WriteLine("cExample : SetParameter : Acquisition Param Init failed");
                return (false);
            }
            return (true);
        }

        /// /// <summary>
        /// 设置OPTIMA控制器的采集参数
        /// </summary>
        /// <param name="TriggerMode">触发模式</param>
        /// <param name="Frequency">设置采集频率</param>
        /// <param name="Average">设置平均值</param>
        /// <param name="PointsTRE">TRE触发模式下，每次触发采集的点数</param>
        /// <returns></returns>
        private bool SetThicknessParameter(enUserTrigerMode TriggerMode, uint PointsTRE)
        {
            //m_sensor.MeasureMode = STIL_NET.enMeasureMode.THICKNESS_MODE;
            if (AcqParamMeasurement.Init(m_sensor) == enSensorError.MCHR_ERROR_NONE)     //判断传感器是否连接上,使用传感器来初始AcqParamMeasurement对象，可将一些必要的参数传给它
            {
                //设置buffer的尺寸
                AcqParamMeasurement.BufferLength = 100;
                //设置Buffers数量
                AcqParamMeasurement.NumberOfBuffers = 500;
                // 启用数据输出数绶冲区,启用多少个数据输出，在获取数据的时候也要设置对应的个数
                AcqParamMeasurement.EnableBufferThickness.Thickness = true;
                AcqParamMeasurement.EnableBufferThickness.Distance1 = true;
                AcqParamMeasurement.EnableBufferThickness.Distance2 = true;
                AcqParamMeasurement.EnableBufferThickness.Intensity1 = true;
                AcqParamMeasurement.EnableBufferThickness.Intensity2 = true;
                AcqParamMeasurement.EnableBufferThickness.BaryCenter1 = true;
                AcqParamMeasurement.EnableEncoder.Encoder1 = true;
                AcqParamMeasurement.EnableEncoder.Encoder2 = true;
                AcqParamMeasurement.EnableEncoder.Encoder3 = true;
                //设置采集延时；set timeout acquisition : should be at least = ((BufferLength * averaging) / rate) * 1000 + 100
                AcqParamMeasurement.Timeout = (uint)((float)AcqParamMeasurement.NumberOfPointsBeforeSignal / (float)AcqParamMeasurement.Frequency) + (uint)(TIMEOUT_ACQUISITION_CONST + ((SEC_TO_MILLISEC * Convert.ToSingle(m_sensor.Averaging) / Convert.ToSingle(AcqParamMeasurement.Frequency)) * Convert.ToSingle(AcqParamMeasurement.BufferLength)));
                //定义采集事件类型，event type (here end of measurements) and callback function  事件类型(这里是测量的结束)和回调函数,这两个参数是配对使用的
                AcqParamMeasurement.EnableEvent.EventAcquireNPoints = true;  //连续采集模式，只有EventAcquireNPoints和EventEndBuffer这两个事件起作用               
                AcqParamMeasurement.NumberOfPointsBeforeSignal = 1;
                //给事件注册一个方法 
                m_sensor.OnEventMeasurement += new sensor.OnEventMeasurementHandler(FuncEventMeasurement);   //给事件注册一个委托方法,事件的本质是一个封装了的委托类型对象
                                                                                                             //设置触发模式
                switch (TriggerMode)
                {
                    case enUserTrigerMode.NONE:
                        AcqParamMeasurement.Trigger.Enable = false;
                        AcqParamMeasurement.Trigger.Type = STIL_NET.enTriggerType.NONE;
                        trigState = false;
                        break;
                    case enUserTrigerMode.TRG:
                        AcqParamMeasurement.Trigger.Enable = true;
                        AcqParamMeasurement.Trigger.Type = STIL_NET.enTriggerType.TRG;
                        if (this.stilPointParamConfig.LevelEdgeFlag == enUserLevelEdgeFlag.FALLING_EDGE)
                            AcqParamMeasurement.Trigger.HighLevelOrRisingEdgeActivated = enLevelEdgeFlag.FALLING_EDGE; // HIGH LEVEL
                        else
                            AcqParamMeasurement.Trigger.HighLevelOrRisingEdgeActivated = enLevelEdgeFlag.RISING_EDGE; // HIGH LEVEL
                                                                                                                      //if (m_sensor.SensorType == STIL_NET.enSensorType.STIL_ZENITH)
                                                                                                                      //{
                                                                                                                      //    sZenithTriggerParameters trigZenith = (sZenithTriggerParameters)AcqParamMeasurement.Trigger;
                                                                                                                      //    trigZenith.Source = enTriggerSource.TRIGGER_SOURCE_EXTERNAL;
                                                                                                                      //}
                        trigState = true;
                        break;
                    case enUserTrigerMode.TRN:
                        AcqParamMeasurement.Trigger.Enable = true;
                        AcqParamMeasurement.Trigger.Type = STIL_NET.enTriggerType.TRN;
                        trigState = true;
                        if (this.stilPointParamConfig.LevelEdgeFlag == enUserLevelEdgeFlag.FALLING_EDGE)
                            AcqParamMeasurement.Trigger.HighLevelOrRisingEdgeActivated = enLevelEdgeFlag.FALLING_EDGE; // HIGH LEVEL
                        else
                            AcqParamMeasurement.Trigger.HighLevelOrRisingEdgeActivated = enLevelEdgeFlag.RISING_EDGE; // HIGH LEVEL
                                                                                                                      //if (m_sensor.SensorType == STIL_NET.enSensorType.STIL_ZENITH)
                                                                                                                      //{
                                                                                                                      //    sZenithTriggerParameters trigZenith = (sZenithTriggerParameters)AcqParamMeasurement.Trigger;
                                                                                                                      //    trigZenith.Source = enTriggerSource.TRIGGER_SOURCE_EXTERNAL;
                                                                                                                      //}
                        break;
                    case enUserTrigerMode.TRS:
                        AcqParamMeasurement.Trigger.Enable = true;
                        AcqParamMeasurement.Trigger.Type = STIL_NET.enTriggerType.TRS;
                        if (this.stilPointParamConfig.LevelEdgeFlag == enUserLevelEdgeFlag.FALLING_EDGE)
                            AcqParamMeasurement.Trigger.HighLevelOrRisingEdgeActivated = enLevelEdgeFlag.FALLING_EDGE; // HIGH LEVEL
                        else
                            AcqParamMeasurement.Trigger.HighLevelOrRisingEdgeActivated = enLevelEdgeFlag.RISING_EDGE; // HIGH LEVEL
                                                                                                                      //if (m_sensor.SensorType == STIL_NET.enSensorType.STIL_ZENITH)
                                                                                                                      //{
                                                                                                                      //    sZenithTriggerParameters trigZenith = (sZenithTriggerParameters)AcqParamMeasurement.Trigger;
                                                                                                                      //    trigZenith.Source = enTriggerSource.TRIGGER_SOURCE_EXTERNAL;
                                                                                                                      //}
                        trigState = true;
                        break;
                    case enUserTrigerMode.TRE:
                        AcqParamMeasurement.Trigger.Enable = true;
                        AcqParamMeasurement.Trigger.Type = STIL_NET.enTriggerType.TRE;
                        if (this.stilPointParamConfig.LevelEdgeFlag == enUserLevelEdgeFlag.FALLING_EDGE)
                            AcqParamMeasurement.Trigger.HighLevelOrRisingEdgeActivated = enLevelEdgeFlag.FALLING_EDGE; // HIGH LEVEL
                        else
                            AcqParamMeasurement.Trigger.HighLevelOrRisingEdgeActivated = enLevelEdgeFlag.RISING_EDGE; // HIGH LEVEL
                        AcqParamMeasurement.NumberPointsTRE = PointsTRE;
                        //if (m_sensor.SensorType == STIL_NET.enSensorType.STIL_ZENITH)
                        //{
                        //    sZenithTriggerParameters trigZenith = (sZenithTriggerParameters)AcqParamMeasurement.Trigger;
                        //    trigZenith.Source = enTriggerSource.TRIGGER_SOURCE_EXTERNAL;
                        //}
                        trigState = true;
                        break;
                    default:
                        trigState = false;
                        break;
                }
            }
            else
            {
                Console.WriteLine("cExample : SetParameter : Acquisition Param Init failed");
                return (false);
            }
            return (true);
        }

        /// <summary>
        /// 执行数据采集
        /// </summary>
        private void ExecuteDistAcquisition()
        {
            float[] Distance = null;
            float[] Intensity = null;
            float[] BaryCenter = null;
            float[] BufferNullFloat = null;
            uint[] Encoder1 = null;
            uint[] Encoder2 = null;
            uint[] Encoder3 = null;
            uint Len1 = 0;
            uint Len2 = 0;
            bool lockTaken = false;
            ////////////////////
            while (m_exit_event.Wait(15) == false)
            {
                if (m_measurement_event.Wait(15))
                {
                    // 在Get获取数据时,这里有可能还在写，共享变量会干扰
                    lockTaken = false;
                    Monitor.Enter(monitor, ref lockTaken);
                    sError = m_sensor.GetAltitudeAcquisitionData(ref Distance, ref Intensity, ref BufferNullFloat, ref BaryCenter, ref BufferNullFloat, ref Len1);
                    if (sError == enSensorError.MCHR_ERROR_NONE && Len1 != 0 && isSave)
                    {
                        for (int i = 0; i < Len1; i++)
                        {
                            List_Dist1.Enqueue(Distance[i]);
                            List_Dist2.Enqueue(0);
                            List_Thick.Enqueue(0);
                            List_Intensity.Enqueue(Intensity[i]);
                            List_Intensity2.Enqueue(0);
                            List_BaryCenter.Enqueue(BaryCenter[i]);
                        }
                        //if (Distance != null && Distance.Length > 0)
                        //{
                        //    OutThickness.AddRange(Distance); // 当厚度没有时，让厚度值等于距离值
                        //    OutDistance.AddRange(Distance);  // 当多个地方对同一个数据操作时，要加锁
                        //    OutDistance2.AddRange(Distance);
                        //}
                        //else
                        //{
                        //    for (int i = 0; i < Len1; i++)
                        //    {
                        //        OutThickness.Add(0); // 当厚度没有时，让厚度值等于距离值
                        //        OutDistance.Add(0);  // 当多个地方对同一个数据操作时，要加锁
                        //        OutDistance2.Add(0);
                        //    }
                        //}
                        //if (Intensity != null && Intensity.Length > 0)
                        //    OutIntensity.AddRange(Intensity);
                        //{
                        //    for (int i = 0; i < Len1; i++)
                        //    {
                        //        OutIntensity.Add(0); // 当厚度没有时，让厚度值等于距离值
                        //    }
                        //}
                        //if (BaryCenter != null && BaryCenter.Length > 0)
                        //    OutBaryCenter.AddRange(BaryCenter);
                        //{
                        //    for (int i = 0; i < Len1; i++)
                        //    {
                        //        OutBaryCenter.Add(0); // 当厚度没有时，让厚度值等于距离值
                        //    }
                        //}
                    }
                    Distance = null;
                    Intensity = null;
                    BaryCenter = null;
                    ////////////////////////////////////
                    sError = m_sensor.GetEncoderData(ref Encoder1, ref Encoder2, ref Encoder3, ref Len2);
                    if (sError == enSensorError.MCHR_ERROR_NONE && Len2 > 0 && isSave) //  && Len2 != 0  read
                    {
                        for (int i = 0; i < Len2; i++)
                        {
                            List_X.Enqueue(Encoder1[i]);
                            List_Y.Enqueue(Encoder2[i]);
                            List_Z.Enqueue(Encoder3[i]);
                        }
                        //if (Encoder1 != null && Encoder1.Length > 0)
                        //    OutEncoderX.AddRange(Encoder1);
                        //else
                        //{
                        //    for (int i = 0; i < Len2; i++)
                        //        OutEncoderX.Add(0);
                        //}
                        ///////////////////////////////////////////
                        //if (Encoder2 != null && Encoder2.Length > 0)
                        //    OutEncoderY.AddRange(Encoder2);
                        //else
                        //{
                        //    for (int i = 0; i < Len2; i++)
                        //        OutEncoderY.Add(0);
                        //}
                        ///////////////////////////////////////////
                        //if (Encoder3 != null && Encoder3.Length > 0)
                        //    OutEncoderZ.AddRange(Encoder3);
                        //else
                        //{
                        //    for (int i = 0; i < Len2; i++)
                        //        OutEncoderZ.Add(0);
                        //}
                    }
                    else
                    {
                        //LoggerHelper.Debug(string.Format("FuncEventMeasurement : Error : GetTransmittedData_CssDistance : {0}", sError.ToString()));
                        // Console.WriteLine(string.Format("FuncEventMeasurement : Error : GetTransmittedData_CssDistance : {0}", sError.ToString()));
                    }
                    ////////////////
                    this.acqNumber = this.OutDistance.Count;
                    if (lockTaken)
                        Monitor.Exit(monitor);
                }
            }
            m_exit_event_do.Set();
        }

        /// <summary>
        /// 执行数据采集，作为采集线程调用的一个方法
        /// </summary>
        private void ExecuteThickAcquisition()
        {
            float[] Thickness = null;
            float[] Distance1 = null;
            float[] Distance2 = null;
            float[] Intensity = null;
            float[] Intensity2 = null;
            float[] BaryCenter = null;
            float[] BufferNullFloat = null;
            uint[] Encoder1 = null;
            uint[] Encoder2 = null;
            uint[] Encoder3 = null;
            uint Len1 = 0, Len2 = 0;
            bool lockTaken = false;
            while (m_exit_event.Wait(15) == false)
            {
                if (m_measurement_event.Wait(15))
                {
                    lockTaken = false;
                    Monitor.Enter(monitor, ref lockTaken);
                    sError = m_sensor.GetThicknessAcquisitionData(ref Thickness, ref Distance1, ref Intensity, ref BaryCenter, ref Distance2, ref Intensity2, ref BufferNullFloat, ref BufferNullFloat, ref BufferNullFloat, ref Len1);
                    if (sError == enSensorError.MCHR_ERROR_NONE && Len1 != 0 && isSave)
                    {
                        for (int i = 0; i < Len1; i++)
                        {
                            List_Dist1.Enqueue(Distance1[i]);
                            List_Dist2.Enqueue(Distance2[i]);
                            List_Thick.Enqueue(Thickness[i]);
                            List_Intensity.Enqueue(Intensity[i]);
                            List_Intensity2.Enqueue(Intensity2[i]);
                            List_BaryCenter.Enqueue(BaryCenter[i]);
                        }

                        //if (Thickness != null && Thickness.Length > 0)
                        //    OutThickness.AddRange(Thickness);
                        //else
                        //{
                        //    for (int i = 0; i < Len; i++)
                        //    {
                        //        OutThickness.Add(0);
                        //    }
                        //}
                        //if (Distance1 != null && Distance1.Length > 0)
                        //    OutDistance.AddRange(Distance1);
                        //else
                        //{
                        //    for (int i = 0; i < Len; i++)
                        //    {
                        //        OutDistance.Add(0);
                        //    }
                        //}
                        //if (Distance2 != null && Distance2.Length > 0)
                        //    OutDistance2.AddRange(Distance2);
                        //else
                        //{
                        //    for (int i = 0; i < Len; i++)
                        //    {
                        //        OutDistance2.Add(0);
                        //    }
                        //}
                        //if (Intensity != null && Intensity.Length > 0)
                        //    OutIntensity.AddRange(Intensity);
                        //else
                        //{
                        //    for (int i = 0; i < Len; i++)
                        //    {
                        //        OutIntensity.Add(0);
                        //    }
                        //}
                        //if (Intensity2 != null && Intensity2.Length > 0)
                        //    OutIntensity2.AddRange(Intensity2);
                        //else
                        //{
                        //    for (int i = 0; i < Len; i++)
                        //    {
                        //        OutIntensity2.Add(0);
                        //    }
                        //}
                        //if (BaryCenter != null && BaryCenter.Length > 0)
                        //    OutBaryCenter.AddRange(BaryCenter);
                        //else
                        //{
                        //    for (int i = 0; i < Len; i++)
                        //    {
                        //        OutBaryCenter.Add(0);
                        //    }
                        //}
                        // }
                    }
                    sError = m_sensor.GetEncoderData(ref Encoder1, ref Encoder2, ref Encoder3, ref Len2);
                    if (sError == enSensorError.MCHR_ERROR_NONE && Len2 != 0 && isSave)
                    {
                        for (int i = 0; i < Len2; i++)
                        {
                            List_X.Enqueue(Encoder1[i]);
                            List_Y.Enqueue(Encoder2[i]);
                            List_Z.Enqueue(Encoder3[i]);
                        }

                        //if (this.LaserParam.Enable_x)
                        //{
                        //    for (int i = 0; i < Encoder1.Length; i++)
                        //        OutEncoderX.Add(Encoder1[i]);
                        //}
                        //else
                        //{
                        //    for (int i = 0; i < Encoder1.Length; i++)
                        //        OutEncoderX.Add(0);
                        //}
                        ///////////////////////////////////////////
                        //if (this.LaserParam.Enable_y)
                        //{
                        //    for (int i = 0; i < Encoder2.Length; i++)
                        //        OutEncoderY.Add(Encoder2[i]);
                        //}
                        //else
                        //{
                        //    for (int i = 0; i < Encoder2.Length; i++)
                        //        OutEncoderY.Add(0);
                        //}
                        ///////////////////////////////////////////
                        //if (this.LaserParam.Enable_z)
                        //{
                        //    for (int i = 0; i < Encoder3.Length; i++)
                        //        OutEncoderZ.Add(Encoder3[i]);
                        //}
                        //else
                        //{
                        //    for (int i = 0; i < Encoder3.Length; i++)
                        //        OutEncoderZ.Add(0);
                        //}
                    }
                    //////////////////////////////////
                    Thickness = null;
                    Distance1 = null;
                    Distance2 = null;
                    Intensity = null;
                    Intensity2 = null;
                    BaryCenter = null;
                    ////////////
                    this.acqNumber = this.OutDistance.Count;
                    this.acqNumber = this.List_Dist1.Count;
                    if (lockTaken)
                        Monitor.Exit(monitor);
                }
            }
            m_exit_event_do.Set();
        }

        /// <summary>
        /// 结束数据采集,用于验证传感器是否停止了测量
        /// </summary>
        /// <returns></returns>
        private void EndExecuteAcquisition()
        {
            //MessageBox.Show("Stil传感器"+ this.m_sensor.SerialNumber + "采集线程退出");
            LoggerHelper.Error("Stil传感器" + this.m_sensor.SerialNumber + "采集线程退出");
            // return (m_exit_event_do.Wait(10));
        }
        //------------------------------------------------------------------------------------------------------
        /// <summary>
        /// 测量事件
        /// </summary>
        /// <param name="ev"></param>
        private void FuncEventMeasurement(sensor.enSensorAcquisitionEvent ev)
        {
            switch (ev)
            {
                case sensor.enSensorAcquisitionEvent.EV_END_ACQUIRE:
                    m_exit_event.Set(); //这个事件为什么会生成？
                    EndExecuteAcquisition();
                    break;
                case sensor.enSensorAcquisitionEvent.EV_END_BUFFER:
                    m_measurement_event.Set();
                    break;
                case sensor.enSensorAcquisitionEvent.EV_END_MEASUREMENT:
                    m_measurement_event.Set();
                    break;
                case sensor.enSensorAcquisitionEvent.EV_ACQUIRE_N_POINTS:
                    m_measurement_event.Set();
                    break;
                default:
                    break;
            }
            if (ev == sensor.enSensorAcquisitionEvent.EV_END_ACQUIRE_TIMEOUT)
            {
                //Console.Write(string.Format("."));
            }
            else
            {
                //Console.WriteLine(string.Format("Event : {0}", ev.ToString()));
            }
        }
        //--------------------------------------------------------------------------------------------------------

        /// <summary>
        ///  矫正倾斜数据
        /// </summary>
        /// <param name="Datax"></param>
        /// <param name="Datay"></param>
        /// <param name="Phi"></param>
        /// <param name="Qx"></param>
        /// <param name="Qy"></param>
        /// <returns></returns>
        public bool Rectify(double[] Datax, double[] Datay, double Phi, out double[] Qx, out double[] Qy)
        {
            int length = Datax.Length;
            // 方法内的第一行代码一定要保证可执，否则会被化悼，即，如果第一行代码是判断语句，可执行，可不执行，编译器编译时会将其优化，不执行他
            if (Datax.Length != Datay.Length)
            {
                Qx = null;
                Qy = null;
                return false;
            }
            double[] Tempx = new double[Datax.Length];
            double[] Tempy = new double[Datay.Length];
            int aa = Rectify2DPoint(Datax, Datay, Phi, length, Tempx, Tempy);
            Qx = Tempx;
            Qy = Tempy;
            return true;
        }

        [DllImport("Rectify.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern int Rectify2DPoint(double[] Dx, double[] Dy, double Phi, int length, double[] Qx, double[] Qy);




    }



}

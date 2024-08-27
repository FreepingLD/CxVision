
using STIL.LineSensor;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using HalconDotNet;
using Common;
//using MotionControlCard;

namespace Sensor
{
    public class StilLineLaser
    {
        private StilLineSensorSetting stilLineConfigParam;
        private LaserParam laserParam;
        private bool StartAcq = false;
        int measureMode = 0;
        bool startThread = false;
        public Device DeviceID { get; set; }
        public bool IsTrigState { get; set; }
        uint TimeOut = 2000;
        Thread m_thread = null; //采集线程
        private readonly object monitor = new object();
        // 全局变量用来存储数据
        List<double> ListThick = new List<double>();
        List<double> ListDist = new List<double>();
        List<double> ListDist2 = new List<double>();
        List<double> ListIntensity = new List<double>();
        List<double> ListIntensity2 = new List<double>();
        List<double> ListXpose = new List<double>();
        List<double> ListYpose = new List<double>();
        List<double> ListZpose = new List<double>();
        public bool AcqState
        {
            get
            {
                return acqState;
            }

            set
            {
                acqState = value;
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
        public StilLineSensorSetting StilLineConfigParam { get => stilLineConfigParam; set => stilLineConfigParam = value; }
        public LaserParam LaserParam { get => laserParam; set => laserParam = value; }

        private bool acqState = false;
        private bool isSave = false;
        private int acqNumber;
        double MeasureRange;



        /// <summary>
        /// 按指定的IP地址打开传感器
        /// </summary>
        /// <param name="IPAdress"></param>
        public bool Connect(string IPAdress)
        {
            string ip = IPAdress;
            try
            {
                DeviceID = LineSensor.ConnectGigEByIpAddress(ref ip);
                if (DeviceID.Id > 0) return true;
                else return false;
            }
            catch (Exception ee)
            {
                //MessageBox.Show("打开传感器错误" + ee.ToString());
                return false;
            }


        }

        /// <summary>
        /// 按传感器的默认地址打开传感器
        /// </summary>
        /// <param name="IPAdress"></param>
        public bool Connect()
        {
            DeviceID = LineSensor.ConnectGigE();
            if (DeviceID.Id > 0)
            {
                ResetEncoder();
                return true;
            }
            return false;
        }

        /// <summary>
        /// 加载配置文件并初始化传感器
        /// </summary>
        /// <param name="DeviceID">设备ID号</param>
        /// <param name="configFileName">配置文件的路径</param>
        public void LoadConfig(string configFileName)
        {
            try
            {
                string path = Application.StartupPath + "\\" + configFileName + "\\DeviceConfig.ini";
                LineSensor.LoadConfig(DeviceID, path, true);
            }
            catch
            {

            }
        }

        /// <summary>
        /// 保存配置文件并初始化传感器
        /// </summary>
        /// <param name="DeviceID">设备ID号</param>
        /// <param name="configFileName">配置文件的路径</param>
        public void SaveConfig(string configFileName)
        {
            string path = Application.StartupPath + "\\" + configFileName + "\\DeviceConfig.ini";
            LineSensor.SaveConfig(DeviceID, path);
        }

        /// <summary>
        /// 设置测量模式
        /// </summary>
        /// <param name="DeviceID"></param>
        /// <param name="Mode">0:距离模式;1:厚度模式</param>
        public void SetMeasureMode(string measureMode)
        {
            // 设置测量模式必需要停止传感器采集
            Monitor.Enter(monitor);
            //StopAcquisition();
            object remode;
            switch (measureMode)
            {
                case "Distance":
                    remode = 0;
                    LineSensor.InvokeDeviceFunc(DeviceID, InvokeDeviceFuncParams.DP_SET_MEASUREMENT_MODE, ref remode);
                    break;
                case "Thickness":
                    remode = 1;
                    LineSensor.InvokeDeviceFunc(DeviceID, InvokeDeviceFuncParams.DP_SET_MEASUREMENT_MODE, ref remode);
                    break;
            }
            //StartAcquisition();
            Monitor.Exit(monitor);
        }

        /// <summary>
        /// 获取传感器的测量模式
        /// </summary>
        /// <param name="DeviceID"></param>
        /// <returns></returns>
        //public int GetMeasureMode()
        //{
        //    object remode = -1;
        //    LineSensor.InvokeDeviceFunc(DeviceID, InvokeDeviceFuncParams.DP_GET_MEASUREMENT_MODE, ref remode);
        //    return Convert.ToInt32(remode);
        //}
        public string GetMeasureMode()
        {
            object remode = -1;
            LineSensor.InvokeDeviceFunc(DeviceID, InvokeDeviceFuncParams.DP_GET_MEASUREMENT_MODE, ref remode);
            switch (Convert.ToInt32(remode))
            {
                case 0:
                    return "Distance";
                case 1:
                    return "Thickness";
                default:
                    return "";
            }
        }

        /// <summary>
        /// 设置曝光时间
        /// </summary>
        /// <param name="DeviceID"></param>
        /// <param name="time">曝光时间,单位us</param>
        public void SetExposeTime(int time)
        {
            lock (monitor)
            {
                object retime = time * 1000;
                LineSensor.InvokeDeviceFunc(DeviceID, InvokeDeviceFuncParams.DP_SET_EXPOSURE_TIME, ref retime);
            }
        }

        /// <summary>
        /// 获取曝光时间
        /// </summary>
        /// <param name="DeviceID">设备号ID</param>
        /// <returns></returns>
        public int GetExposeTime()
        {
            object retime = 0; //获取的值单位为ns,所以要转化为us
            LineSensor.InvokeDeviceFunc(DeviceID, InvokeDeviceFuncParams.DP_GET_EXPOSURE_TIME, ref retime);
            return Convert.ToInt32(retime) / 1000;
        }

        /// <summary>
        /// 获取传感器的测量范围
        /// </summary>
        /// <param name="DeviceID">设备号ID</param>
        /// <returns></returns>
        public int GetFullScale()
        {
            object FullScale = 0;
            LineSensor.InvokeDeviceFunc(DeviceID, InvokeDeviceFuncParams.DP_GET_FULLSCALE, ref FullScale);
            return Convert.ToInt32(FullScale);
        }

        /// <summary>
        /// 执行Dark
        /// </summary>
        /// <param name="DeviceID">设备ID</param>
        public void AquisitionDark(object configFileName)
        {
            object state = -1;
            int iNbSpectroDarkFailed = 0;
            LineSensor.AcqDarkSignal(DeviceID);
            for (int i = 0; i < 180; i++)
            {
                LineSensor.InvokeSpectroFunc(DeviceID, i, InvokeSpectroFuncParams.SP_GET_FIBER_STATUS, ref state, 1);
                if ((FiberStatus)state == FiberStatus.FIBER_STATUS_TOO_MUCH_DARK)
                {
                    iNbSpectroDarkFailed++;
                }
            }
            if (iNbSpectroDarkFailed > 0)
            {
                MessageBox.Show("有" + iNbSpectroDarkFailed.ToString() + "根光纤暗信号太高");
            }
            else
            {
                //MessageBox.Show("所有光纤暗信号正常");
            }
            object path = Application.StartupPath + "\\" + configFileName + "\\DarkSignal.bin";
            LineSensor.InvokeDeviceFunc(DeviceID, InvokeDeviceFuncParams.DP_SAVE_DARK_SIGNALS_FROM_SENSORS_TO_FILE, ref path);
            MessageBox.Show("Dark完成");
        }

        /// <summary>
        /// 采集暗信号 
        /// </summary>
        /// <param name="FilePath"></param>
        public void LoadDark(object configFileName)
        {
            object path = Application.StartupPath + "\\" + configFileName + "\\DarkSignal.bin";
            try
            {
                LineSensor.InvokeDeviceFunc(DeviceID, InvokeDeviceFuncParams.DP_LOAD_DARK_SIGNALS_FROM_FILE_INTO_SENSORS, ref path);
            }
            catch
            {
                MessageBox.Show("暗信号文件加载失败");
            }


        }

        /// <summary>
        /// 获取采集的数据中有效轮廓的数量即有多少个测量轮廓
        /// </summary>
        /// <param name="DeviceID"></param>
        /// <returns></returns>
        public int GetNumberProfileAvailable()
        {
            object Num = 0;
            LineSensor.InvokeDeviceFunc(DeviceID, InvokeDeviceFuncParams.DP_GET_NB_PROFILES_AVAILABLE, ref Num);
            return Convert.ToInt32(Num);

        }
        public int ResetEncoder()
        {
            object Num = 0;
            LineSensor.InvokeDeviceFunc(DeviceID, (InvokeDeviceFuncParams)2229, ref Num); //2229 
            return Convert.ToInt32(Num);

        }

        /// <summary>
        /// 设置TRE模式下每次触发采集的线数
        /// </summary>
        /// <param name="DeviceID">设备ID</param>
        /// <param name="TRENumber">每次信号采集的线数</param>
        public void SetTRENumber(int TRENumber)
        {
            object devInParam = TRENumber;
            LineSensor.InvokeDeviceFunc(DeviceID, InvokeDeviceFuncParams.DP_SET_NB_IMAGES_BURST_TRIGGER, ref devInParam);
        }

        /// <summary>
        /// 获取TRE模式下每次触发能采集的最大线数
        /// </summary>
        /// <param name="DeviceID">设备ID</param>
        public int GetTRENumber()
        {
            object TRENumber = -1;
            LineSensor.InvokeDeviceFunc(DeviceID, InvokeDeviceFuncParams.DP_GET_NB_IMAGES_BURST_TRIGGER, ref TRENumber);
            return Convert.ToInt32(TRENumber);
        }

        /// <summary>
        /// 设置软触发模式
        /// </summary>
        /// <param name="DeviceID">设备ID</param>
        /// <param name="triggermode">"TRG","TRS","TRN","TRE"</param>
        public void SetSoftwareTriggerMode(enUserTrigerMode triggermode, int TREpoint)
        {
            try
            {
                if (triggermode == enUserTrigerMode.NONE)
                {
                    IsTrigState = false;
                    TriggerType triggerType = TriggerType.TRIGGER_CONTINUOUS_MODE;
                    object devInParam = Convert.ToInt32(triggerType);
                    LineSensor.InvokeDeviceFunc(DeviceID, InvokeDeviceFuncParams.DP_SET_TRIGGER_IN_MODE, ref devInParam);
                }
                if (triggermode == enUserTrigerMode.TRG)
                {
                    IsTrigState = true;
                    TriggerType triggerType = TriggerType.TRIGGER_SOFT;
                    object devInParam = Convert.ToInt32(triggerType);
                    LineSensor.InvokeDeviceFunc(DeviceID, InvokeDeviceFuncParams.DP_SET_TRIGGER_IN_MODE, ref devInParam);
                }
                if (triggermode == enUserTrigerMode.TRN)
                {
                    IsTrigState = true;
                    TriggerType triggerType = TriggerType.TRIGGER_SOFT;
                    triggerType = TriggerType.START_STOP_SOFTWARE_TRIGGER_LEVEL;
                    object devInParam = Convert.ToInt32(triggerType);
                    LineSensor.InvokeDeviceFunc(DeviceID, InvokeDeviceFuncParams.DP_SET_TRIGGER_IN_MODE, ref devInParam);
                }
                if (triggermode == enUserTrigerMode.TRS)
                {
                    IsTrigState = true;
                    TriggerType triggerType = TriggerType.TRIGGER_SOFT;
                    triggerType = TriggerType.START_STOP_SOFTWARE_TRIGGER_EDGE;
                    object devInParam = Convert.ToInt32(triggerType);
                    LineSensor.InvokeDeviceFunc(DeviceID, InvokeDeviceFuncParams.DP_SET_TRIGGER_IN_MODE, ref devInParam);
                }
                if (triggermode == enUserTrigerMode.TRE)
                {
                    IsTrigState = true;
                    TriggerType triggerType = TriggerType.TRIGGER_SOFT;
                    triggerType = TriggerType.BURST_ON_SOFTWARE_TRIGGER;
                    object devInParam = Convert.ToInt32(triggerType);
                    object TREValue = TREpoint;
                    LineSensor.InvokeDeviceFunc(DeviceID, InvokeDeviceFuncParams.DP_SET_TRIGGER_IN_MODE, ref devInParam);
                    LineSensor.InvokeDeviceFunc(DeviceID, InvokeDeviceFuncParams.DP_SET_NB_IMAGES_BURST_TRIGGER, ref TREValue);
                }
            }
            catch
            {
                MessageBox.Show("参数设置错误,可能是该控制器不支持该功能");
            }
        }

        /// <summary>
        /// 设置硬触发模式
        /// </summary>
        /// <param name="DeviceID">设备ID</param>
        /// <param name="triggermode">"TRG","TRS","TRN","TRE"</param>
        public void SetHardTriggerMode(enUserTrigerMode triggermode)
        {
            try
            {
                if (triggermode == enUserTrigerMode.TRG)
                {
                    IsTrigState = true;
                    TriggerType triggerType = TriggerType.TRIGGER_RISING_EDGE;
                    object devInParam = Convert.ToInt32(triggerType);
                    LineSensor.InvokeDeviceFunc(DeviceID, InvokeDeviceFuncParams.DP_SET_TRIGGER_IN_MODE, ref devInParam);
                }
                if (triggermode == enUserTrigerMode.TRN)
                {
                    IsTrigState = true;
                    TriggerType triggerType = TriggerType.START_STOP_TRIGGER_HIGH_LEVEL;
                    object devInParam = Convert.ToInt32(triggerType);
                    LineSensor.InvokeDeviceFunc(DeviceID, InvokeDeviceFuncParams.DP_SET_TRIGGER_IN_MODE, ref devInParam);
                }
                if (triggermode == enUserTrigerMode.TRS)
                {
                    IsTrigState = true;
                    TriggerType triggerType = TriggerType.START_STOP_TRIGGER_RISING_EDGE;
                    object devInParam = Convert.ToInt32(triggerType);
                    LineSensor.InvokeDeviceFunc(DeviceID, InvokeDeviceFuncParams.DP_SET_TRIGGER_IN_MODE, ref devInParam);
                }
                if (triggermode == enUserTrigerMode.TRE)
                {
                    IsTrigState = true;
                    TriggerType triggerType = TriggerType.BURST_ON_RISING_EDGE;
                    object devInParam = Convert.ToInt32(triggerType);
                    LineSensor.InvokeDeviceFunc(DeviceID, InvokeDeviceFuncParams.DP_SET_TRIGGER_IN_MODE, ref devInParam);
                }
            }
            catch
            {
                MessageBox.Show("参数设置错误,可能是该控制器不支持该功能");
            }
        }

        /// <summary>
        /// 获取传感器的触发模式;DP_GET_TRIGGER_IN_MODE模式或DP_GET_TRIGGER_OUT_MODE模式,这个方法有问题,返回值为什么是0而不是7？
        /// </summary>
        /// <param name="DeviceID">设备ID号</param>
        /// <param name="triggerType">触发模式</param>
        public TriggerType GetTriggerMode()
        {
            //0 trigger rising edge : 1: falling edge trigger ; 2 : continuous acquisition; 3 trigger soft; ' burst on rising 5 burst on falling 6 stopped 7 burst on software trigger
            object devInParam = -1;
            LineSensor.InvokeDeviceFunc(DeviceID, InvokeDeviceFuncParams.DP_GET_TRIGGER_IN_MODE, ref devInParam);
            return (TriggerType)devInParam;
        }

        /// <summary>
        /// 设置峰值检测模式下的检测阈值,用于v2产品
        /// </summary>
        /// <param name="DeviceID">设备ID号</param>
        /// <param name="Value">值范围0-1</param>
        public void SetDetectionThreshold(double Value)
        {
            lock (monitor)
            {
                object ValueParam = (int)(Value * 4095);
                //设置固件检测阈值//DP_SET_MIN_INTENSITY   DP_SET_FIRMWARE_DETECTION_THRESHOLD：这两个参数应该用哪一个？
                LineSensor.InvokeDeviceFunc(DeviceID, InvokeDeviceFuncParams.DP_SET_FIRMWARE_DETECTION_THRESHOLD, ref ValueParam);
                // 设置DLL检测阈值    ;这两个参数必需同步设置才有效
                for (int i = 0; i < 180; i++)
                {
                    LineSensor.InvokeSpectroFunc(DeviceID, i, InvokeSpectroFuncParams.SP_SET_DLL_DETECTION_THRESHOLD, ref ValueParam, 0);
                }
            }
        }

        /// <summary>
        /// 获取峰值检测模式下的检测阈值
        /// </summary>
        /// <param name="DeviceID">设备ID号</param>
        public double GetDetectionThreshold()
        {
            //DP_GET_FIRMWARE_DETECTION_THRESHOLD:return first peak mode Detection Threshold
            object ValueParam = 0;
            object ValueParam2 = 0;  //这两个获取的值会相等吗？
            LineSensor.InvokeDeviceFunc(DeviceID, InvokeDeviceFuncParams.DP_GET_FIRMWARE_DETECTION_THRESHOLD, ref ValueParam);
            LineSensor.InvokeSpectroFunc(DeviceID, 0, InvokeSpectroFuncParams.SP_GET_DLL_DETECTION_THRESHOLD, ref ValueParam2, 0);
            return Convert.ToInt32(ValueParam) / 4095.0;
        }

        /// <summary>
        /// 设置峰值检测模式下的检测阈值,用于v1产品
        /// </summary>
        /// <param name="DeviceID">设备ID号</param>
        /// <param name="Value">值范围0-1</param>
        public void SetDetectionThresholdV1(double Value)
        {
            object ValueParam = Value * 4095;
            LineSensor.InvokeDeviceFunc(DeviceID, InvokeDeviceFuncParams.DP_GET_DETECTION_THRESHOLD, ref ValueParam);
        }

        /// <summary>
        /// 获取峰值检测模式下的检测阈值,用于V1产品
        /// </summary>
        /// <param name="DeviceID">设备ID号</param>
        public double GetDetectionThresholdV1()
        {
            //DP_GET_FIRMWARE_DETECTION_THRESHOLD:return first peak mode Detection Threshold
            object ValueParam = 0;
            LineSensor.InvokeDeviceFunc(DeviceID, InvokeDeviceFuncParams.DP_GET_DETECTION_THRESHOLD, ref ValueParam);
            return Convert.ToInt32(ValueParam) / 4095.0;
        }

        /// <summary>
        /// 选择第一个峰,此函数用于USB接口,用于V1控制器
        /// </summary>
        /// <param name="DeviceID"></param>
        public void SetFirstPeak()
        {
            //此函数用于USB接口
            // DP_ENABLE_FIRST_PEAK_ALGORITHM:Select algorithm detection peak : if 0 : peak with max intensity, 1 : First Peak detection mode
            object ValueParam = 1;
            LineSensor.InvokeDeviceFunc(DeviceID, InvokeDeviceFuncParams.DP_ENABLE_FIRST_PEAK_ALGORITHM, ref ValueParam);
        }

        /// <summary>
        /// 选择第强峰，用于V1控制器
        /// </summary>
        /// <param name="DeviceID"></param>
        public void SetStrongPeak()
        {
            //此函数用于USB接口
            // DP_ENABLE_FIRST_PEAK_ALGORITHM:Select algorithm detection peak : if 0 : peak with max intensity, 1 : First Peak detection mode;//此函数用于USB接口
            object ValueParam = 0;
            LineSensor.InvokeDeviceFunc(DeviceID, InvokeDeviceFuncParams.DP_ENABLE_FIRST_PEAK_ALGORITHM, ref ValueParam);
        }
        /// <summary>
        /// 选择第强峰，用于V1控制器
        /// </summary>
        /// <param name="DeviceID"></param>
        public void SetSecondPeak()
        {
            //此函数用于USB接口
            // DP_ENABLE_FIRST_PEAK_ALGORITHM:Select algorithm detection peak : if 0 : peak with max intensity, 1 : First Peak detection mode;//此函数用于USB接口
            object ValueParam = 2;
            LineSensor.InvokeDeviceFunc(DeviceID, InvokeDeviceFuncParams.DP_ENABLE_FIRST_PEAK_ALGORITHM, ref ValueParam);
        }

        /// <summary>
        /// 获取峰值的检测模式；0：最强峰模式（默认阈值为80）；1：第一峰模式（默认阈值为400）
        /// </summary>
        /// <returns></returns>
        public int GetPeakTypeInDistMode()
        {
            //DP_GET_ENABLE_FIRST_PEAK_MODE：To  get  the  peak  detection  mode;0：最强峰模式（默认阈值为80）；1：第一峰模式（默认阈值为400）
            object ValueParam = -1;
            LineSensor.InvokeDeviceFunc(DeviceID, InvokeDeviceFuncParams.DP_GET_ENABLE_FIRST_PEAK_MODE, ref ValueParam);
            return Convert.ToInt32(ValueParam);
        }

        /// <summary>
        /// 获取厚度测量模式下第二峰的类型;0:最强峰;1:第一峰;2:第二峰;3:第三峰;4:第四峰;9:最后一峰，用于V2控制器
        /// </summary>
        /// <param name="DeviceID"></param>
        public string GetSecondPeakTypeInThicknessMode()
        {
            //DP_GET_TYPE_OF_PEAK2:这是什么意思 ;0:最强峰;1:第一峰;2:第二峰;3:第三峰;4:第四峰;9:最后一峰
            object ValueParam = 2;
            LineSensor.InvokeDeviceFunc(DeviceID, InvokeDeviceFuncParams.DP_GET_TYPE_OF_PEAK2, ref ValueParam);
            switch (Convert.ToInt32(ValueParam))
            {
                case 0:
                    return "Strong Peak";
                case 1:
                    return "First Peak";
                case 2:
                    return "Second Peak";
                case 3:
                    return "Third Peak";
                case 4:
                    return "Fourth Peak";
                case 9:
                    return "Last Peak";
                default:
                    return "";
            }
        }

        /// <summary>
        /// 获取厚度测量模式下第一峰的类型;0:最强峰;1:第一峰;2:第二峰;3:第三峰;4:第四峰;9:最后一峰，用于V2控制器
        /// </summary>
        /// <param name="DeviceID"></param>
        public string GetFirstPeakTypeInThicknessMode()
        {
            //用于网络接口
            //DP_GET_TYPE_OF_PEAK2:这是什么意思 ;0:最强峰;1:第一峰;2:第二峰;3:第三峰;4:第四峰;9:最后一峰
            object ValueParam = 2;
            LineSensor.InvokeDeviceFunc(DeviceID, InvokeDeviceFuncParams.DP_GET_TYPE_OF_PEAK1, ref ValueParam);
            switch (Convert.ToInt32(ValueParam))
            {
                case 0:
                    return "Strong Peak";
                case 1:
                    return "First Peak";
                case 2:
                    return "Second Peak";
                case 3:
                    return "Third Peak";
                case 4:
                    return "Fourth Peak";
                case 9:
                    return "Last Peak";
                default:
                    return "";
            }
        }

        /// <summary>
        /// 获取距离测量模式下第一峰的类型;0:最强峰;1:第一峰;2:第二峰;3:第三峰;4:第四峰;9:最后一峰,用于V2控制器
        /// </summary>
        /// <param name="DeviceID"></param>
        public string GetPeakTypeInAltitudeMode()
        {
            //用于网络接口
            //DP_GET_TYPE_OF_PEAK2:这是什么意思 ;0:最强峰;1:第一峰;2:第二峰;3:第三峰;4:第四峰;9:最后一峰
            lock (monitor)
            {
                object ValueParam = 2;
                LineSensor.InvokeDeviceFunc(DeviceID, InvokeDeviceFuncParams.DP_GET_TYPE_OF_PEAK1, ref ValueParam);
                switch (Convert.ToInt32(ValueParam))
                {
                    case 0:
                        return "Strong Peak";
                    case 1:
                        return "First Peak";
                    case 2:
                        return "Second Peak";
                    case 3:
                        return "Third Peak";
                    case 4:
                        return "Fourth Peak";
                    case 9:
                        return "Last Peak";
                    default:
                        return "";
                }
            }
        }

        /// <summary>
        /// 设置厚度测量模式下第二峰的类型;0:最强峰;1:第一峰;2:第二峰;3:第三峰;4:第四峰;9:最后一峰,用于V2控制器
        /// </summary>
        /// <param name="DeviceID"></param>
        public void SetFirstPeakTypeInThicknessMode(enUserPeakMode peakMode)
        {
            lock (monitor) // 加锁是为了设置参数时不需停止采集
            {
                //DP_GET_TYPE_OF_PEAK2:这是什么意思 ;0:最强峰;1:第一峰;2:第二峰;3:第三峰;4:第四峰;9:最后一峰
                object ValueParam;
                switch (peakMode)
                {
                    case enUserPeakMode.StrongestPeak:
                        ValueParam = 0;
                        LineSensor.InvokeDeviceFunc(DeviceID, InvokeDeviceFuncParams.DP_SET_TYPE_OF_PEAK1, ref ValueParam);
                        break;
                    case enUserPeakMode.FirstPeak:
                        ValueParam = 1;
                        LineSensor.InvokeDeviceFunc(DeviceID, InvokeDeviceFuncParams.DP_SET_TYPE_OF_PEAK1, ref ValueParam);
                        break;
                    case enUserPeakMode.SecondPeak:
                        ValueParam = 2;
                        LineSensor.InvokeDeviceFunc(DeviceID, InvokeDeviceFuncParams.DP_SET_TYPE_OF_PEAK1, ref ValueParam);
                        break;
                    case enUserPeakMode.ThreePeak:
                        ValueParam = 3;
                        LineSensor.InvokeDeviceFunc(DeviceID, InvokeDeviceFuncParams.DP_SET_TYPE_OF_PEAK1, ref ValueParam);
                        break;
                    case enUserPeakMode.FourPeak:
                        ValueParam = 4;
                        LineSensor.InvokeDeviceFunc(DeviceID, InvokeDeviceFuncParams.DP_SET_TYPE_OF_PEAK1, ref ValueParam);
                        break;
                    case enUserPeakMode.LastPeak:
                        ValueParam = 9;
                        LineSensor.InvokeDeviceFunc(DeviceID, InvokeDeviceFuncParams.DP_SET_TYPE_OF_PEAK1, ref ValueParam);
                        break;
                }
            }
        }

        /// <summary>
        /// 设置厚度测量模式下第二峰的类型;0:最强峰;1:第一峰;2:第二峰;3:第三峰;4:第四峰;9:最后一峰
        /// </summary>
        /// <param name="DeviceID"></param>
        /// <param name="PeakIndex">峰值序号</param>
        public void SetSecondPeakTypeInThicknessMode(enUserPeakMode peakMode)
        {
            lock (monitor)
            {
                object ValueParam;
                //DP_GET_TYPE_OF_PEAK2:这是什么意思 ;0:最强峰;1:第一峰;2:第二峰;3:第三峰;4:第四峰;9:最后一峰
                switch (peakMode)
                {
                    case enUserPeakMode.StrongestPeak:
                        ValueParam = 0;
                        LineSensor.InvokeDeviceFunc(DeviceID, InvokeDeviceFuncParams.DP_SET_TYPE_OF_PEAK2, ref ValueParam);
                        break;
                    case enUserPeakMode.FirstPeak:
                        ValueParam = 1;
                        LineSensor.InvokeDeviceFunc(DeviceID, InvokeDeviceFuncParams.DP_SET_TYPE_OF_PEAK2, ref ValueParam);
                        break;
                    case enUserPeakMode.SecondPeak:
                        ValueParam = 2;
                        LineSensor.InvokeDeviceFunc(DeviceID, InvokeDeviceFuncParams.DP_SET_TYPE_OF_PEAK2, ref ValueParam);
                        break;
                    case enUserPeakMode.ThreePeak:
                        ValueParam = 3;
                        LineSensor.InvokeDeviceFunc(DeviceID, InvokeDeviceFuncParams.DP_SET_TYPE_OF_PEAK2, ref ValueParam);
                        break;
                    case enUserPeakMode.FourPeak:
                        ValueParam = 4;
                        LineSensor.InvokeDeviceFunc(DeviceID, InvokeDeviceFuncParams.DP_SET_TYPE_OF_PEAK2, ref ValueParam);
                        break;
                    case enUserPeakMode.FivePeak:
                        ValueParam = 5;
                        LineSensor.InvokeDeviceFunc(DeviceID, InvokeDeviceFuncParams.DP_SET_TYPE_OF_PEAK2, ref ValueParam);
                        break;
                    case enUserPeakMode.LastPeak:
                        ValueParam = 9;
                        LineSensor.InvokeDeviceFunc(DeviceID, InvokeDeviceFuncParams.DP_SET_TYPE_OF_PEAK2, ref ValueParam);
                        break;
                }
            }
        }

        /// <summary>
        /// 设置距离测量模式下峰值的类型;0:最强峰;1:第一峰;2:第二峰;3:第三峰;4:第四峰;9:最后一峰，用于V2控制器
        /// </summary>
        /// <param name="DeviceID"></param>
        public void SetPeakTypeInAltitudeMode(enUserPeakMode peakMode)
        {
            lock (monitor)
            {
                //DP_SET_TYPE_OF_PEAK1:用于V2控制器
                //DP_GET_TYPE_OF_PEAK2:这是什么意思 ;0:最强峰;1:第一峰;2:第二峰;3:第三峰;4:第四峰;9:最后一峰
                object ValueParam;
                //LineSensor.InvokeDeviceFunc(DeviceID, InvokeDeviceFuncParams.DP_SET_TYPE_OF_PEAK1, ref ValueParam);
                switch (peakMode)
                {
                    case enUserPeakMode.StrongestPeak:
                        ValueParam = 0;
                        LineSensor.InvokeDeviceFunc(DeviceID, InvokeDeviceFuncParams.DP_SET_TYPE_OF_PEAK1, ref ValueParam);
                        break;
                    case enUserPeakMode.FirstPeak:
                        ValueParam = 1;
                        LineSensor.InvokeDeviceFunc(DeviceID, InvokeDeviceFuncParams.DP_SET_TYPE_OF_PEAK1, ref ValueParam);
                        break;
                    case enUserPeakMode.SecondPeak:
                        ValueParam = 2;
                        LineSensor.InvokeDeviceFunc(DeviceID, InvokeDeviceFuncParams.DP_SET_TYPE_OF_PEAK1, ref ValueParam);
                        break;
                    case enUserPeakMode.ThreePeak:
                        ValueParam = 3;
                        LineSensor.InvokeDeviceFunc(DeviceID, InvokeDeviceFuncParams.DP_SET_TYPE_OF_PEAK1, ref ValueParam);
                        break;
                    case enUserPeakMode.FourPeak:
                        ValueParam = 4;
                        LineSensor.InvokeDeviceFunc(DeviceID, InvokeDeviceFuncParams.DP_SET_TYPE_OF_PEAK1, ref ValueParam);
                        break;
                    case enUserPeakMode.LastPeak:
                        ValueParam = 9;
                        LineSensor.InvokeDeviceFunc(DeviceID, InvokeDeviceFuncParams.DP_SET_TYPE_OF_PEAK1, ref ValueParam);
                        break;
                }
            }
        }



        /// <summary>
        /// 获取Led灯的亮度
        /// </summary>
        /// <param name="DeviceID"></param>
        /// <returns></returns>
        public int GetLEDLightValue()
        {
            object ValueParam = -1;
            LineSensor.InvokeDeviceFunc(DeviceID, InvokeDeviceFuncParams.DP_GET_LED_INTENSITY, ref ValueParam);
            return Convert.ToInt32(ValueParam);
        }

        /// <summary>
        /// 设置Led灯的亮度
        /// </summary>
        /// <param name="DeviceID"></param>
        /// <param name="LedValue">0-100</param>
        /// <returns></returns>
        public void SetLEDLightValue(int LedValue)
        {
            lock (monitor)
            {
                //DP_SET_LED_INTENSITY
                object ValueParam = LedValue;
                LineSensor.InvokeDeviceFunc(DeviceID, InvokeDeviceFuncParams.DP_SET_LED_INTENSITY, ref ValueParam);
            }
        }

        /// <summary>
        /// 启用测量强度=原始强度而不是预处理强度的模式
        /// </summary>
        /// <param name="DeviceID"></param>
        public void EableRawIntensity()
        {
            //DP_SET_ENABLE_RAW_INTENSITY:enable the mode where measured intensity=raw intensity instead of preprocessed intensity
            object ValueParam = 0; //参数该为0还是1？
            LineSensor.InvokeDeviceFunc(DeviceID, InvokeDeviceFuncParams.DP_GET_ENABLE_RAW_INTENSITY, ref ValueParam);
        }

        /// <summary>
        /// 返回控制器当前处理的峰值数量,如果为距离模式,返回值为1,如果为厚度模式,返回值为2,他并不是返回当前有多少个峰
        /// </summary>
        /// <param name="DeviceID"></param>
        /// <returns></returns>
        public int GetPeakNumber()
        {
            object ValueParam = -1;
            LineSensor.InvokeDeviceFunc(DeviceID, InvokeDeviceFuncParams.DP_GET_NUMBER_OF_PEAK, ref ValueParam);
            return Convert.ToInt32(ValueParam);
        }

        /// <summary>
        /// 启用光强归一化;1:启用;0:禁用
        /// </summary>
        /// <param name="DeviceID"></param>
        public void EableIntensityNormalization()
        {
            //DP_SET_ENABLE_INTENSITY_NORMALIZATION：Enable or disable Intensity normalization 1:启用;0:禁用
            object ValueParam = 1;
            LineSensor.InvokeDeviceFunc(DeviceID, InvokeDeviceFuncParams.DP_SET_ENABLE_INTENSITY_NORMALIZATION, ref ValueParam);
        }

        /// <summary>
        /// 禁用光强归一化
        /// </summary>
        /// <param name="DeviceID"></param>
        public void DisableIntensityNormalization()
        {
            //DP_SET_ENABLE_INTENSITY_NORMALIZATION：Enable or disable Intensity normalization 
            object ValueParam = 0;
            LineSensor.InvokeDeviceFunc(DeviceID, InvokeDeviceFuncParams.DP_SET_ENABLE_INTENSITY_NORMALIZATION, ref ValueParam);
        }

        /// <summary>
        /// 启用执校正;1:启用;0:禁用
        /// </summary>
        /// <param name="DeviceID"></param>
        public void EableThermalCorrection()
        {
            //DP_SET_ENABLE_INTENSITY_NORMALIZATION：Enable or disable Intensity normalization 1:启用;0:禁用
            object ValueParam = 1;
            LineSensor.InvokeDeviceFunc(DeviceID, InvokeDeviceFuncParams.DP_SET_ENABLE_CALIBRATION_COEF, ref ValueParam);
        }

        /// <summary>
        /// 禁用执校正
        /// </summary>
        /// <param name="DeviceID"></param>
        public void DisableThermalCorrection()
        {
            //DP_SET_ENABLE_INTENSITY_NORMALIZATION：Enable or disable Intensity normalization 
            object ValueParam = 0;
            LineSensor.InvokeDeviceFunc(DeviceID, InvokeDeviceFuncParams.DP_SET_ENABLE_CALIBRATION_COEF, ref ValueParam);
        }

        /// <summary>
        /// 执行热校正
        /// </summary>
        /// <param name="Path"></param>
        public void ThermalCorrection(string Path)
        {
            //DP_SET_ENABLE_INTENSITY_NORMALIZATION：Enable or disable Intensity normalization 1:启用;0:禁用
            object ValueParam = -1;
            LineSensor.InvokeDeviceFunc(DeviceID, InvokeDeviceFuncParams.DP_SET_ENABLE_CALIBRATION_COEF, ref ValueParam);
            //采集执校正
            LineSensor.AcqCalibrationCoef(DeviceID);
            string Thermalpath = Path + "\\ThermalCorrection.bin"; //热校正的文件是哪个？
            LineSensor.SaveConfig(DeviceID, Thermalpath);

            object States = -1;
            LineSensor.InvokeDeviceFunc(DeviceID, InvokeDeviceFuncParams.DP_GET_CALIBRATION_COEF_STATE, ref States);
            if ((int)States == 0)
            {
                MessageBox.Show("读取参数失败");
            }
        }

        /// <summary>
        /// 校正强度归一化
        /// </summary>
        /// <param name="Path"></param>
        public void IntensityNormalization(string Path)
        {
            lock (monitor)
            {
                //DP_SET_ENABLE_INTENSITY_NORMALIZATION：Enable or disable Intensity normalization 1:启用;0:禁用
                LineSensor.AcqIntensityNormalization(DeviceID);
                //采集之后即保存
                object ValueParam = Path + "\\IntensityNormalisation.bin";
                LineSensor.InvokeDeviceFunc(DeviceID, InvokeDeviceFuncParams.DP_SAVE_INTENSITY_NORMALIZATION_FROM_SENSORS_TO_FILE, ref ValueParam);

                object States = -1;
                LineSensor.InvokeDeviceFunc(DeviceID, InvokeDeviceFuncParams.DP_GET_INTENSITY_NORMALIZATION_STATE, ref States);
                if ((int)States == 0)
                {
                    MessageBox.Show("读取参数失败");
                }
            }
        }

        /// <summary>
        /// 加载强度归一化文件到传感器
        /// </summary>
        /// <param name="DeviceID"></param>
        /// <param name="Path"></param>
        public void LoadIntensityNormalization(string Path)
        {
            //DP_LOAD_INTENSITY_NORMALIZATION_FROM_FILE_TO_SENSORS
            object ValueParam = Path + "\\IntensityNormalisation.bin";
            LineSensor.InvokeDeviceFunc(DeviceID, InvokeDeviceFuncParams.DP_LOAD_INTENSITY_NORMALIZATION_FROM_FILE_TO_SENSORS, ref ValueParam);
        }

        /// <summary>
        /// 获取线传感器的参数结构，只支持32位系统
        /// </summary>
        /// <param name="DeviceID"></param>
        /// <returns></returns>
        public LineSensorParams GetLineSensorParamsStruct(Device DeviceID)
        {
            object devInParam;
            Int64 ptr;
            LineSensorParams lsParams = new LineSensorParams();
            // Convert LineSensorParams struct into IntPtr for receive configuration parameters from Line Sensor DLL
            IntPtr AccesStruct = Marshal.AllocHGlobal(Marshal.SizeOf(lsParams)); // Memory Allocation for struct space
            Marshal.StructureToPtr(lsParams, AccesStruct, true);  // Convertion into IntPtr
            ptr = AccesStruct.ToInt64();  // use Int64 type  for transmit variant type into LineSensorDLL
            devInParam = ptr;
            // Call Invoke Function with DP_GET_COPY_CONFIG_STRUCT parameter
            LineSensor.InvokeDeviceFunc(DeviceID, InvokeDeviceFuncParams.DP_GET_COPY_CONFIG_STRUCT, ref devInParam);
            // Copy the data values of the struct into C# LineSensorParams struct 
            lsParams = (LineSensorParams)Marshal.PtrToStructure(AccesStruct, typeof(LineSensorParams));
            // Free allocation Memory
            Marshal.FreeHGlobal(AccesStruct);
            return lsParams;
        }

        /// <summary>
        /// 断开传感器连接
        /// </summary>
        /// <param name="ls">是释放该对象的ID号</param>
        public void DisConnect()
        {
            if (DeviceID != null)
            {
                //StopAcuisition
                LineSensor.StopAcquisition(DeviceID);
                //DestroyDevice
                LineSensor.DestroyDevice(DeviceID);
                DeviceID = null;
            }
        }

        /// <summary>
        /// 开始传感器采集
        /// </summary>
        /// <param name="DeviceID"></param>
        public void StartAcquisition()
        {
            //开启线程
            startThread = true;
            ////开始传感器采集
            LineSensor.StartAcquisition(DeviceID);
            //距离模式
            if (GetMeasureMode() == "Distance")
            {
                measureMode = 0;
                m_thread = new Thread(executeDist);
            }
            //厚度模式  
            if (GetMeasureMode() == "Thickness")
            {
                measureMode = 1;
                m_thread = new Thread(executeThick);
            }
            m_thread.IsBackground = true;
            m_thread.Start();
            IsTrigState = false;
            //////////////
            MeasureRange = GetFullScale() * 0.001;
            this.stilLineConfigParam.MeasureRange = MeasureRange;
        }

        /// <summary>
        /// 停止传感器采集
        /// </summary>
        /// <param name="DeviceID"></param>
        public void StopAcquisition()
        {
            // 停止采集线程
            startThread = false;
            ////////////////////
            LineSensor.StopAcquisition(DeviceID);
            if (m_thread.IsAlive) m_thread.Abort();
            IsTrigState = false;
        }

        /// <summary>
        /// 获取采集数据
        /// </summary>
        /// <param name="DeviceID"></param>
        /// <param name="TimeOut"></param>
        /// <returns></returns>
        private void GetDistData(out double[] Dist, out double[] Intensity, out double[] Xpose, out double[] Ypose, out double[] Zpose)
        {
            Thread.Sleep(200);
            /////////////////////
            List<double> listDist = new List<double>();
            List<double> listInsity = new List<double>();
            List<double> listX = new List<double>();
            List<double> listY = new List<double>();
            List<double> listZ = new List<double>();
            bool lockTaken = false;
            //////////////////////
            Monitor.Enter(monitor, ref lockTaken);
            //////////////////////
            for (int i = 0; i < ListDist.Count; i++)
            {
                if (ListDist[i] == 0)
                    listDist.Add(-9999);
                else
                {
                    if((bool)this.StilLineConfigParam?.IsMirrorZ)
                        listDist.Add(ListDist[i] * -0.001);
                    else
                        listDist.Add(ListDist[i]*0.001);
                }    
                listX.Add((ListXpose[i]));
                listY.Add(((ListYpose[i] - ListYpose[0]) * this.laserParam.Resolution_Y));              // - minYvalue
                listZ.Add(((ListZpose[i] - ListZpose[0]) * this.laserParam.Resolution_Z));
                listInsity.Add((ListIntensity[i]));
            }
            ////////////////////////////
            Dist = listDist.ToArray();
            Xpose = listX.ToArray();
            Ypose = listY.ToArray();
            Zpose = listZ.ToArray();
            Intensity = listInsity.ToArray();
            ///////////////////////清空内存中的数据
            ListDist.RemoveRange(0, ListDist.Count);
            ListXpose.RemoveRange(0, ListXpose.Count);
            ListYpose.RemoveRange(0, ListYpose.Count);
            ListZpose.RemoveRange(0, ListZpose.Count);
            ListIntensity.RemoveRange(0, ListIntensity.Count);
            //////////////////
            if (lockTaken) Monitor.Exit(monitor);


        }

        /// <summary>
        /// 获取厚度数据
        /// </summary>
        /// <param name="Xpose"></param>
        /// <param name="Ypose"></param>
        /// <param name="Dist"></param>
        /// <param name="Dist2"></param>
        /// <param name="Intensity"></param>
        /// <param name="Intensity2"></param>
        private void GetThickData(out double[] Dist, out double[] Dist2, out double[] Thick, out double[] Intensity, out double[] Intensity2, out double[] Xpose, out double[] Ypose, out double[] Zpose)
        {
            Thread.Sleep(200);
            ////////////////////////
            List<double> listThick = new List<double>();
            List<double> listDist = new List<double>();
            List<double> listDist2 = new List<double>();
            List<double> listInsity = new List<double>();
            List<double> listInsity2 = new List<double>();
            List<double> listX = new List<double>();
            List<double> listY = new List<double>();
            List<double> listZ = new List<double>();
            bool lockTaken = false;
            double minYvalue;
            double MeasureRange = GetFullScale() * 0.001;
            if (ListYpose.Count > 1)
                minYvalue = Math.Min(ListYpose[0], ListYpose[ListYpose.Count - 1]);
            //////////////////////
            Monitor.Enter(monitor, ref lockTaken);
            //////////////////////
            for (int i = 0; i < ListDist.Count; i++)
            {
                listThick.Add((ListThick[i] * 0.001));
                if (ListDist[i] == 0)
                    listDist.Add(-9999); // 这样就保证了，当高度值为0时值也为0，
                else
                {
                    if ((bool)this.StilLineConfigParam?.IsMirrorZ)
                        listDist.Add((MeasureRange - ListDist[i] * -0.001));
                    else
                        listDist.Add((MeasureRange - ListDist[i] * 0.001));
                }
                if (listDist2[i] == 0)
                    listDist2.Add(-9999); // 这样就保证了，当高度值为0时值也为0，
                else
                {
                    if ((bool)this.StilLineConfigParam?.IsMirrorZ)
                        listDist2.Add((MeasureRange - ListDist2[i] * -0.001));
                    else
                        listDist2.Add((MeasureRange - ListDist2[i] * 0.001));
                }
                ///////////////////
                listDist.Add((ListDist[i]));// 取反数据,因为厚度模式下,零点在上方
                listDist2.Add((ListDist2[i]));
                listX.Add((ListXpose[i]));
                listY.Add((ListYpose[i] - ListYpose[0]) * this.laserParam.Resolution_Y); // Y/Z要取反 GlobalVariable.pConfig.Y_encoderResolution
                listZ.Add((ListZpose[i] - ListZpose[0]) * this.laserParam.Resolution_Z); //GlobalVariable.pConfig.Y_encoderResolution * -1
                listInsity.Add((ListIntensity[i]));
                listInsity2.Add((ListIntensity2[i]));
            }
            Thick = listThick.ToArray();
            Dist = listDist.ToArray();
            Dist2 = listDist2.ToArray();
            Xpose = listX.ToArray();
            Ypose = listY.ToArray();
            Zpose = listZ.ToArray();
            Intensity = listInsity.ToArray();
            Intensity2 = listInsity2.ToArray();
            ///////////////////////清空内存中的数据
            ListThick.RemoveRange(0, ListThick.Count);
            ListDist.RemoveRange(0, ListDist.Count);
            ListDist2.RemoveRange(0, ListDist2.Count);
            ListXpose.RemoveRange(0, ListXpose.Count);
            ListYpose.RemoveRange(0, ListYpose.Count);
            ListZpose.RemoveRange(0, ListZpose.Count);
            ListIntensity.RemoveRange(0, ListIntensity.Count);
            ListIntensity2.RemoveRange(0, ListIntensity2.Count);
            //////////////////////////////////////////
            if (lockTaken) Monitor.Exit(monitor);
        }

        /// <summary>
        /// 获取测量数据
        /// </summary>
        /// <param name="Dist"></param>
        /// <param name="Dist2"></param>
        /// <param name="Thick"></param>
        /// <param name="Intensity"></param>
        /// <param name="Intensity2"></param>
        /// <param name="Xpose"></param>
        /// <param name="Ypose"></param>
        public void GetData(out double[] Dist, out double[] Dist2, out double[] Thick, out double[] Intensity, out double[] Xpose, out double[] Ypose, out double[] Zpose, out double[] Intensity2)
        {
            Dist = null;
            Dist2 = null;
            Thick = null;
            Intensity = null;
            Intensity2 = null;
            Xpose = null;
            Ypose = null;
            if (measureMode == 0)
            {
                GetDistData(out Dist, out Intensity, out Xpose, out Ypose, out Zpose);
            }
            else
            {
                GetThickData(out Dist, out Dist2, out Thick, out Intensity, out Intensity2, out Xpose, out Ypose, out Zpose);
            }
        }


        /// <summary>
        /// 厚度采集线程
        /// </summary>
        private void executeThick()
        {
            AcquiredThicknessData acquiredthickData;
            object Num = 0;
            uint ProfileNumofMeasure;
            double[] X, Y, Z1, Z2;
            try
            {
                while (startThread)
                {
                    acqState = false;
                    LineSensor.InvokeDeviceFunc(DeviceID, InvokeDeviceFuncParams.DP_GET_NB_PROFILES_AVAILABLE, ref Num);
                    ProfileNumofMeasure = Convert.ToUInt32(Num);
                    if (ProfileNumofMeasure > 0)
                    {
                        acqState = true;
                        //////////////////////
                        for (int i = 0; i < ProfileNumofMeasure; i++)
                        {
                            acquiredthickData = LineSensor.ReadAcquiredThicknessData(DeviceID, 180, TimeOut);
                            if (this.isSave && ListDist.Count < 50000000) // 最大两千万个数据
                            {
                                X = GenSequence(180, 0, 179 * GlobalVariable.pConfig.Stil_PointPitch);
                                Z1 = InvertRange(acquiredthickData.Altitudes, this.MeasureRange);
                                Z2 = InvertRange(acquiredthickData.Altitudes2, this.MeasureRange);
                                ListThick.AddRange(acquiredthickData.Thicknesses);
                                ListDist.AddRange(Z1);
                                ListDist2.AddRange(Z2);
                                ListXpose.AddRange(X);
                                if (this.laserParam.Enable_y)
                                {
                                    Y = GenConstantSequence(180, acquiredthickData.EncoderPosition);
                                    ListYpose.AddRange(Y);
                                }
                                else
                                {
                                    Y = GenConstantSequence(180, -1024.0);
                                    ListYpose.AddRange(Y);
                                }
                                ListZpose.AddRange(GenConstantSequence(180, -1024.0));
                                ListIntensity.AddRange(acquiredthickData.Intensities);
                                ListIntensity2.AddRange(acquiredthickData.Intensities2);
                                ////////////////
                                this.acqNumber = this.ListDist.Count; // 生成当前的采集数量
                            }
                        }
                    }
                    Num = 0;
                    Thread.Sleep(10);
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Stil线传感器" + "退出采集线程");
                LoggerHelper.Error("Stil传感器" + "退出采集线程", ex);
            }
        }

        /// <summary>
        /// 距离采集线程
        /// </summary>
        private void executeDist()
        {
            FileOperate fo = new FileOperate();
            AcquiredDataEx acquiredDataEx;
            double[] X, Y, Z;
            uint ProfileNumofMeasure;
            object Num = 0;
            try
            {
                while (startThread)
                {
                    acqState = false;
                    LineSensor.InvokeDeviceFunc(DeviceID, InvokeDeviceFuncParams.DP_GET_NB_PROFILES_AVAILABLE, ref Num);
                    ProfileNumofMeasure = Convert.ToUInt32(Num);
                    if (ProfileNumofMeasure > 0)
                    {
                        acqState = true;
                        //////////////////////
                        for (int i = 0; i < ProfileNumofMeasure; i++)
                        {
                            acquiredDataEx = LineSensor.ReadAcquiredDataEx(DeviceID, 180, TimeOut);
                            ///////////////////////////////////////////////
                            if (this.isSave && ListDist.Count < 10000000)
                            {
                                X = GenSequence(180, 0, 179 * GlobalVariable.pConfig.Stil_PointPitch);
                                Z = InvertRange(acquiredDataEx.Altitudes, this.MeasureRange);
                                ListDist.AddRange(Z);
                                ListDist2.AddRange(Z);
                                ListXpose.AddRange(X);//scale(acquiredDataEx.Xpositions)
                                if (this.laserParam.Enable_y)
                                {
                                    Y = GenConstantSequence(180, acquiredDataEx.EncoderPosition);
                                    ListYpose.AddRange(Y);
                                }
                                else
                                {
                                    Y = GenConstantSequence(180, -1024.0);
                                    ListYpose.AddRange(Y);
                                }
                                ListZpose.AddRange(GenConstantSequence(180, -1024.0));
                                ListIntensity.AddRange(acquiredDataEx.Intensities);
                                //////////////////////
                                this.acqNumber = this.ListDist.Count;
                            }
                        }
                    }
                    Num = 0;
                    Thread.Sleep(10);
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Stil线传感器" + "退出采集线程");
                LoggerHelper.Error("Stil传感器" + "退出采集线程", ex);
            }
        }


        /// <summary>
        /// 触发传感器采集数据
        /// </summary>
        /// <param name="DeviceID"></param>
        public bool TrigSensor()
        {
            try
            {
                object nullParam = new object();
                LineSensor.InvokeDeviceFunc(DeviceID, InvokeDeviceFuncParams.DP_SEND_TRIGGER, ref nullParam);
            }
            catch
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 停止触发传感器采集数据,常用于TRS,TRn触发模式
        /// </summary>
        /// <param name="DeviceID"></param>
        public bool StopTrigSensorTRS(object trigSource, int acqNum)
        {
            this.isSave = true;
            object nullParam = new object();
            enUserTriggerSource triggerSource;
            if (Enum.TryParse(trigSource.ToString().Trim(), out triggerSource))
            {
                switch (triggerSource)
                {
                    case enUserTriggerSource.软触发:
                        this.isSave = false;
                        if (this.DeviceID != null)
                        {
                            LineSensor.InvokeDeviceFunc(DeviceID, InvokeDeviceFuncParams.DP_STOP_TRIGGER, ref nullParam);
                            return true;
                        }
                        else
                            return false;
                    case enUserTriggerSource.NONE:
                    case enUserTriggerSource.内部IO触发:
                        //case enUserTriggerSource.线性比较触发:
                        this.isSave = false;
                        return true;
                    case enUserTriggerSource.外部IO触发: // 当触发源为外部时
                        while (stilLineConfigParam.TreNum < this.ListDist.Count)
                        {
                            if (stilLineConfigParam.CancelWaite) break;
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
        public bool StartTrigSensorTRS(object trigSource)
        {
            this.isSave = false;
            Clear(); // 开始触发前清空数据
            Thread.Sleep(10);
            object nullParam = new object();
            enUserTriggerSource triggerSource;
            if (Enum.TryParse(trigSource.ToString().Trim(), out triggerSource))
            {
                switch (triggerSource)
                {
                    case enUserTriggerSource.软触发:
                        this.isSave = true;
                        if (this.DeviceID != null)
                        {
                            LineSensor.InvokeDeviceFunc(DeviceID, InvokeDeviceFuncParams.DP_STOP_TRIGGER, ref nullParam);
                            return true;
                        }
                        else
                            return false;
                    case enUserTriggerSource.NONE:
                    case enUserTriggerSource.内部IO触发:
                    case enUserTriggerSource.外部IO触发:
                        //case enUserTriggerSource.线性比较触发:
                        this.isSave = true;
                        return true;
                    default:
                        return true;
                }
            }
            else
                return false;
        }
        // 清空数据
        private void Clear()
        {
            lock (monitor)
            {
                ListThick.RemoveRange(0, ListThick.Count);
                ListDist.RemoveRange(0, ListDist.Count);
                ListDist2.RemoveRange(0, ListDist2.Count);
                ListXpose.RemoveRange(0, ListXpose.Count);
                ListYpose.RemoveRange(0, ListYpose.Count);
                ListIntensity.RemoveRange(0, ListIntensity.Count);
                ListIntensity2.RemoveRange(0, ListIntensity2.Count);
                this.acqNumber = 0;
            }
        }

        private double[] GenSequence(int count, double minValue, double maxValue)
        {
            double[] data = new double[count];
            double step = Math.Abs((maxValue - minValue)) / (count - 1);
            for (int i = 0; i < count; i++)
            {
                data[i] = i * step * 1.0;
            }
            data[count - 1] = maxValue; // 让最后一个值等于最大值
            return data;
        }
        private double[] GenConstantSequence(int count, double Value)
        {
            double[] data = new double[count];
            for (int i = 0; i < count; i++)
            {
                data[i] = Value;
            }
            return data;
        }
        private double[] GenConstantSequence(int count, uint Value)
        {
            double[] data = new double[count];
            for (int i = 0; i < count; i++)
            {
                data[i] = Value;
            }
            return data;
        }

        private void smooth(double[] dist, double sigma, out double[] smooth_dist)
        {
            HTuple Xvalue, Yvalue;
            smooth_dist = null;
            HFunction1D functionGauss;
            HFunction1D function1d = new HFunction1D(new HTuple(InvertRange(dist, GlobalVariable.pConfig.Stil_MeasureRange * 1000)));//GlobalVariable.pConfig.Stil_MeasureRange *1000-
            functionGauss = function1d.SmoothFunct1dGauss(sigma);
            functionGauss.Funct1dToPairs(out Xvalue, out Yvalue);
            smooth_dist = InvertRange(Yvalue.ToDArr(), GlobalVariable.pConfig.Stil_MeasureRange * 1000);
        }

        private double[] InvertRange(double[] dist, double range)
        {
            double[] dd = new double[dist.Length];
            for (int i = 0; i < dist.Length; i++)
            {
                if (dist[i] > 0)
                    dd[i] = range - dist[i] * 0.001;
                else
                    dd[i] = dist[i];
            }
            return dd;
        }

        private void SetZero(double[] X, double[] Z, out double[] outZ)
        {
            double RowBegin, ColBegin, RowEnd, ColEnd, Nr, Nc, Dist;
            HTuple dist;
            outZ = new double[180];
            HXLDCont xld = new HXLDCont(new HTuple(Z), new HTuple(X) * 1000);
            xld.FitLineContourXld("tukey", -1, 0, 5, 2, out RowBegin, out ColBegin, out RowEnd, out ColEnd, out Nr, out Nc, out Dist);
            HOperatorSet.DistancePl(Z, new HTuple(X) * 1000, RowBegin, ColBegin, RowEnd, ColEnd, out dist);
            for (int i = 0; i < X.Length; i++)
            {
                if (dist[i] > 50)
                    outZ[i] = 0;
                else
                    outZ[i] = Z[i];
            }
        }

        private double[] scale(double[] x)
        {
            double[] dd = new double[x.Length];
            for (int i = 0; i < x.Length; i++)
            {
                dd[i] = x[i] * 0.001;
            }
            return dd;
        }
        private double[] calibration_zValue(double[] dist, double[] refValue)
        {
            double[] dd = new double[dist.Length];
            for (int i = 0; i < dist.Length; i++)
            {
                dd[i] = (dist[i] * 0.001 - refValue[i + 3]) * Math.Cos(refValue[1]);
            }
            return dd;
        }
        private double[] calibration_xValue(double[] X, double[] dist, double[] refValue)
        {
            double[] dd = new double[X.Length];
            for (int i = 0; i < X.Length; i++)
            {
                if ((dist[i] * 0.001 - refValue[i + 3]) >= 0)
                    dd[i] = X[i] - (dist[i] * 0.001 - refValue[i + 3]) * Math.Sin(refValue[1]);
                else
                    dd[i] = X[i] + (dist[i] * 0.001 - refValue[i + 3]) * Math.Sin(refValue[1]);
            }
            return dd;
        }


        /// <summary>
        /// 插值光强
        /// </summary>
        /// <param name="X"></param>
        /// <param name="Z"></param>
        /// <param name="SampX"></param>
        /// <param name="SampZ"></param>
        //private void Interpolation(double[] X, double[] Z, out double[] SampX, out double[] SampZ)
        //{
        //    HTuple Function;
        //    HTuple SampledFunction;
        //    HTuple XValue, ZValue;
        //    double step = (X[X.Length - 1] - X[0]) / (GlobalVariable.pConfig.LaserLineCount * GlobalVariable.pConfig.SubdivideNum);
        //    if (X.Length > 0)
        //    {
        //        HOperatorSet.CreateFunct1dPairs(X, Z, out Function);
        //        HOperatorSet.SampleFunct1d(Function, X[0], X[X.Length - 1], step, "constant", out SampledFunction);
        //        HOperatorSet.Funct1dToPairs(SampledFunction, out XValue, out ZValue);
        //        ///////////////////////

        //        if (XValue.Length > GlobalVariable.pConfig.LaserLineCount * GlobalVariable.pConfig.SubdivideNum)
        //        {
        //            XValue = XValue.TupleRemove(XValue.Length - 1);
        //            ZValue = ZValue.TupleRemove(ZValue.Length - 1);
        //        }
        //        else
        //        {
        //            XValue[XValue.Length - 1] = X[X.Length - 1];
        //            ZValue[ZValue.Length - 1] = Z[Z.Length - 1];
        //        }
        //        if (XValue.Length == GlobalVariable.pConfig.LaserLineCount * GlobalVariable.pConfig.SubdivideNum)
        //        {
        //            XValue[XValue.Length - 1] = X[X.Length - 1];
        //        }
        //        SampX = XValue.DArr;
        //        SampZ = ZValue.DArr;
        //    }
        //    else
        //    {
        //        SampX = null;
        //        SampZ = null;
        //    }
        //}

        //private void Interpolation(double[] X, double[] Z, double Max_OffsetX, double Max_OffsetZ, out double[] SampX, out double[] SampZ)
        //{
        //    SampX = null;
        //    SampZ = null;
        //    double[] Inter_X = GenSequence(GlobalVariable.pConfig.LaserLineCount * GlobalVariable.pConfig.SubdivideNum, X[0], X[X.Length - 1]);

        //    先去悼0值点
        //    List<double> List_X = new List<double>();
        //    List<double> List_Z = new List<double>();
        //    for (int i = 0; i < X.Length; i++)
        //    {
        //        if (Z[i] > 0)
        //        {
        //            List_X.Add(X[i]);
        //            List_Z.Add(Z[i]);
        //        }
        //    }
        //    if (List_X.Count <= 1) //|| GlobalVariable.pConfig.InterpolationNum==1
        //    {
        //        SampX = X;
        //        SampZ = Z;
        //        return;
        //    }

        //    ////////////
        //    List<double> List_SampX = new List<double>();
        //    List<double> List_SampZ = new List<double>();
        //    double kk, b;
        //    if (Inter_X.Length > 0)
        //    {
        //        左边为0的区间
        //        if (X[0] < List_X[0])
        //        {
        //            for (int k = 0; k < Inter_X.Length; k++)
        //            {
        //                if (X[0] <= Inter_X[k] && Inter_X[k] < List_X[0])
        //                {
        //                    List_SampX.Add(Inter_X[k]);
        //                    List_SampZ.Add(0);
        //                }
        //            }
        //        }
        //        值不为0的区间
        //        for (int i = 0; i < List_X.Count - 1; i++)
        //        {
        //            if (Math.Abs(List_X[i + 1] - List_X[i]) <= Max_OffsetX * 1.5 && Math.Abs(List_Z[i + 1] - List_Z[i]) < Max_OffsetZ * 1.5) // && Math.Abs(List_Z[i + 1] - List_Z[i]) < Max_OffsetZ
        //            {
        //                kk = (List_Z[i + 1] - List_Z[i]) / (List_X[i + 1] - List_X[i]);
        //                b = List_Z[i] - kk * List_X[i];
        //                /////////////////////
        //                for (int k = 0; k < Inter_X.Length; k++)
        //                {
        //                    if (List_X[i] <= Inter_X[k] && Inter_X[k] < List_X[i + 1])
        //                    {
        //                        List_SampX.Add(Inter_X[k]);
        //                        List_SampZ.Add(kk * Inter_X[k] + b);
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                for (int k = 0; k < Inter_X.Length; k++)
        //                {
        //                    if (List_X[i] <= Inter_X[k] && Inter_X[k] < List_X[i + 1])
        //                    {
        //                        List_SampX.Add(Inter_X[k]);
        //                        List_SampZ.Add(0);
        //                    }
        //                }
        //            }
        //        }
        //        右边值为0的区间
        //        左边为0的区间
        //        if (List_X[List_X.Count - 1] <= X[X.Length - 1])
        //        {
        //            for (int k = 0; k < Inter_X.Length; k++)
        //            {
        //                if (List_X[List_X.Count - 1] <= Inter_X[k] && Inter_X[k] < X[X.Length - 1] + 0.00001)
        //                {
        //                    List_SampX.Add(Inter_X[k]);
        //                    if (Z[Z.Length - 1] > 0)
        //                        List_SampZ.Add(Z[Z.Length - 1]);
        //                    else
        //                        List_SampZ.Add(0);
        //                }
        //            }
        //        }
        //        ///////////////
        //        SampX = List_SampX.ToArray();
        //        SampZ = List_SampZ.ToArray();
        //    }
        //    else
        //    {
        //        SampX = null;
        //        SampZ = null;
        //    }
        //}






    }
}
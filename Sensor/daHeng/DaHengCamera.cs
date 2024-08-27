using GxIAPINET;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HalconDotNet;
using Common;
using System.Threading;
using System.Diagnostics;

namespace Sensor
{
    public class DaHengCamera : SensorBase, ISensor
    {
        //private HImage _grabImage;
        private IFrameData imageIFrameData;
        private string ipAdress;
        private bool acqState = true;
        private IGXFactory m_objIGXFactory = null;                            ///<Factory对像
        private IGXDevice m_objIGXDevice = null;                             ///<设备对像
        private IGXStream m_objIGXStream = null;                            ///<流对像
        private IGXFeatureControl m_objIGXFeatureControl = null;           ///<远端设备属性控制器对像
        private IGXFeatureControl m_objIGXStreamFeatureControl = null;

        public IGXFactory ObjIGXFactory { get => m_objIGXFactory; set => m_objIGXFactory = value; }
        public IGXDevice ObjIGXDevice { get => m_objIGXDevice; set => m_objIGXDevice = value; }
        public IGXFeatureControl ObjIGXFeatureControl { get => m_objIGXFeatureControl; set => m_objIGXFeatureControl = value; }


        #region 实现接口
        public bool Connect(SensorConnectConfigParam configParam)
        {
            bool result = true;
            try
            {
                this.ConfigParam = configParam;
                this.Name = configParam.SensorName;
                this.CameraParam = (CameraParam)new CameraParam().Read(configParam.SensorName); // 读取对应的相机参数进来
                this.CameraParam.SensorName = configParam.SensorName;
                ////////////////////////
                switch (configParam.ConnectType)
                {
                    case enUserConnectType.Network:
                        if (!OpenCamera(ipAdress)) result = false;
                        break;
                    default:
                    case enUserConnectType.SerialNumber:
                        if (GetDeviceList().Count == 0) result = false;
                        if (!OpenCamera(GetDeviceList()[0]))
                            result = false;
                        break;
                    case enUserConnectType.Map:
                        this._MapName = configParam.ConnectAddress;
                        if (SensorManage.GetSensor(configParam.ConnectAddress) != null &&
                            SensorManage.GetSensor(configParam.ConnectAddress).ConfigParam.ConnectState)  // 只有在映射源找开成功的情总下，映射对象才能打开成功
                            result = true;
                        else
                            result = false;
                        break;
                }
                ///////////////
                SetTriggerMode("Off");// On
                SetTriggerSource("Software");
                SetTriggerActivation("RisingEdge");
                /////////////
                StartAcq();
                result = true;
            }
            catch (Exception ex)
            {
                LoggerHelper.Error(this.Name + "打开失败", ex);
                result = false;
            }
            configParam.ConnectState = result;
            return result;
        }
        public bool Disconnect()
        {
            CloseCamera();
            Uinit();
            return true;
        }
        public bool Init()
        {
            try
            {
                InitDLL();
                return true;
            }
            catch (Exception ex)
            {
                LoggerHelper.Error("相机初始化失败", ex);
                return false;
            }
        }
        public Dictionary<enDataItem, object> ReadData()
        {
            Dictionary<enDataItem, object> list = new Dictionary<enDataItem, object>();
            list.Add(enDataItem.Image,new ImageDataClass(this._grabImage, this.CameraParam));
            return list;
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
                case enSensorParamType.DaHeng_曝光:
                    return GetShutterValue();
                case enSensorParamType.DaHeng_增益:
                    return GetGainValue();
                case enSensorParamType.DaHeng_触发源:
                    return GetTriggerSource();
                case enSensorParamType.DaHeng_触发模式:
                    return GetTriggerMode();
                case enSensorParamType.DaHeng_触发极性:
                    return GetTriggerActivation();
                case enSensorParamType.Coom_相机内参:
                    return this.CameraParam.CamParam;
                case enSensorParamType.Coom_相机外参:
                    return this.CameraParam.CamPose;
                case enSensorParamType.Coom_每线点数:
                    return 0;
                case enSensorParamType.Coom_相机角度:
                    return this.CameraParam.CamSlant;
                case enSensorParamType.Coom_相机校准档:
                    return this.CameraParam.CalibrateFile;
                default:
                    return null;
            }
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
                case enSensorParamType.DaHeng_曝光:
                    SetShutterValue(Convert.ToDouble(value));
                    return true;
                case enSensorParamType.DaHeng_增益:
                    SetGainValue(Convert.ToDouble(value));
                    return true;
                case enSensorParamType.DaHeng_触发源:
                    SetTriggerSource(value.ToString());
                    return true;
                case enSensorParamType.DaHeng_触发模式:
                    SetTriggerMode(value.ToString());
                    return true;
                case enSensorParamType.DaHeng_触发极性:
                    SetTriggerActivation(value.ToString());
                    return true;
                case enSensorParamType.Coom_相机内参:
                    this.CameraParam.CamParam =new userCamParam((HTuple)value);
                    return true;
                case enSensorParamType.Coom_相机外参:
                    this.CameraParam.CamPose =new userCamPose((HTuple)value);
                    return true;
                case enSensorParamType.Coom_相机角度:
                    this.CameraParam.CamSlant = (double)value;
                    return true;
                case enSensorParamType.Coom_相机校准档:
                    this.CameraParam.CalibrateFile = (AxisCalibration)value;
                    return true;
                default:
                    return true;
            }
        }
        public bool StartTrigger() // 这个参数不再使用
        {
            bool result = false;
            try
            {
                if (this._grabImage != null)
                    this._grabImage.Dispose();
                HImage image = this.GetImage();
                this.AdjImg(image, out this._grabImage);
                image?.Dispose();
                result = true;
                LoggerHelper.Info(this.CameraParam.SensorName + "图像采集成功");
            }
            catch (Exception ex)
            {
                result = false;
                LoggerHelper.Error(this.CameraParam.SensorName + "图像采集失败" + ex);
            }
            return result;
        }
        public bool StopTrigger()
        {
            return true;
        }

        #endregion



        // 初始化DLL
        private void InitDLL()
        {
            m_objIGXFactory = IGXFactory.GetInstance();
            m_objIGXFactory.Init();
        }

        /// <summary>
        /// 获取可用的相机设备列表
        /// </summary>
        /// <returns></returns>
        private List<IGXDeviceInfo> GetDeviceList()
        {
            List<IGXDeviceInfo> listGXDeviceInfo = new List<IGXDeviceInfo>();
            if (m_objIGXFactory != null)
                m_objIGXFactory.UpdateDeviceList(200, listGXDeviceInfo);
            return listGXDeviceInfo;
        }

        /// <summary>
        /// 打开相机
        /// </summary>
        /// <param name="GXDeviceInfo"> 设备信息对象</param>
        /// <param name="e"></param>
        private bool OpenCamera(IGXDeviceInfo GXDeviceInfo)
        {
            try
            {
                // 关闭流
                CloseStream();
                // 如果设备已经打开则关闭，保证相机在初始化出错情况下能再次打开
                CloseDevice();
                // 打开设备
                m_objIGXDevice = m_objIGXFactory.OpenDeviceBySN(GXDeviceInfo.GetSN(), GX_ACCESS_MODE.GX_ACCESS_EXCLUSIVE);
                m_objIGXFeatureControl = m_objIGXDevice.GetRemoteFeatureControl();// 获取远程设备控制器；包含主要设备信息，比如宽高、曝光增益等，一般用户主要使用此属性控制器即可
                // 打开流并获取流属性控制器                                                                 // 打开流
                OpenStream();
                // 建议用户在打开网络相机之后，根据当前网络环境设置相机的流通道包长值，
                // 以提高网络相机的采集性能,设置方法参考以下代码。
                SetNetStream();
                // 设置相机采集模式
                SetAcquisitionMode();
                return true;
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                m_objIGXFactory.GigEResetDevice(GXDeviceInfo.GetMAC(), GX_RESET_DEVICE_MODE.GX_MANUFACTURER_SPECIFIC_RECONNECT); // 重置设备是在没办法的情况下执行
                return false;
            }
        }
        private bool OpenCamera(string iPadress)
        {
            try
            {
                // 关闭流
                CloseStream();
                // 如果设备已经打开则关闭，保证相机在初始化出错情况下能再次打开
                CloseDevice();
                // 打开设备
                GetDeviceList();
                m_objIGXDevice = m_objIGXFactory.OpenDeviceByIP(iPadress, GX_ACCESS_MODE.GX_ACCESS_EXCLUSIVE);
                m_objIGXFeatureControl = m_objIGXDevice.GetRemoteFeatureControl();// 获取远程设备控制器；包含主要设备信息，比如宽高、曝光增益等，一般用户主要使用此属性控制器即可
                // 打开流并获取流属性控制器                                                                 // 打开流
                OpenStream();
                // 建议用户在打开网络相机之后，根据当前网络环境设置相机的流通道包长值，
                // 以提高网络相机的采集性能,设置方法参考以下代码。
                SetNetStream();
                // 设置相机采集模式
                SetAcquisitionMode();
                return true;
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                foreach (var item in GetDeviceList())
                    m_objIGXFactory.GigEResetDevice(item.GetMAC(), GX_RESET_DEVICE_MODE.GX_MANUFACTURER_SPECIFIC_RECONNECT);
                //m_objIGXFactory.GigEResetDevice(iPadress, GX_RESET_DEVICE_MODE.GX_MANUFACTURER_SPECIFIC_RECONNECT); // 重置设备是在没办法的情况下执行
                return false;
            }
        }
        /// <summary>
        /// 设置相机采集模式
        /// </summary>
        private void SetAcquisitionMode()
        {
            if (null != m_objIGXFeatureControl)
            {
                //设置采集模式连续采集
                m_objIGXFeatureControl.GetEnumFeature("AcquisitionMode").SetValue(enAcquisitionMode.Continuous.ToString());//enAcquisitionMode  "Continuous"
            }


        }

        /// <summary>
        /// 打开流
        /// </summary>
        private void OpenStream()
        {
            //打开流
            if (null != m_objIGXDevice)
            {
                m_objIGXStream = m_objIGXDevice.OpenStream(0);
                m_objIGXStreamFeatureControl = m_objIGXStream.GetFeatureControl();
            }
        }
        private bool streamProcessState = false;
        /// <summary>
        /// 关闭流
        /// </summary>
        private void CloseStream()
        {
            try
            {
                //关闭流
                if (null != m_objIGXStream)
                {
                    while (this.streamProcessState) // 等待流数据处理完
                        Application.DoEvents();
                    m_objIGXStream.Close(); // 如果在回调函数中，还在处理流数据，那么这里将无法关闭流，所以必需先让回调函数中的流处理完成
                    m_objIGXStream = null;
                    m_objIGXStreamFeatureControl = null;
                }
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// 关闭设备
        /// </summary>
        private void CloseDevice()
        {
            try
            {
                //关闭设备
                if (null != m_objIGXDevice)
                {
                    m_objIGXDevice.Close();
                    m_objIGXDevice = null;
                }
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// 设置网络流到最优包长
        /// </summary>
        private void SetNetStream()
        {
            // 建议用户在打开网络相机之后，根据当前网络环境设置相机的流通道包长值，
            // 以提高网络相机的采集性能,设置方法参考以下代码。
            GX_DEVICE_CLASS_LIST objDeviceClass = m_objIGXDevice.GetDeviceInfo().GetDeviceClass();
            if (GX_DEVICE_CLASS_LIST.GX_DEVICE_CLASS_GEV == objDeviceClass)
            {
                // 判断设备是否支持流通道数据包功能
                if (true == m_objIGXFeatureControl.IsImplemented("GevSCPSPacketSize"))
                {
                    // 获取当前网络环境的最优包长值
                    uint nPacketSize = m_objIGXStream.GetOptimalPacketSize();
                    // 将最优包长值设置为当前设备的流通道包长值
                    m_objIGXFeatureControl.GetIntFeature("GevSCPSPacketSize").SetValue(nPacketSize);
                }
            }
        }

        /// <summary>
        /// 关闭相机
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void CloseCamera()
        {
            try
            {
                // 先停止采集
                StopAcq();
                //  关闭流
                CloseStream();
                //关闭设备
                CloseDevice();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void Uinit()
        {
            ////////////
            try
            {
                //反初始化
                if (null != m_objIGXFactory)
                {
                    m_objIGXFactory.Uninit();
                }
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// 开始相机采集
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void StartAcq()
        {
            try
            {
                if (null != m_objIGXStreamFeatureControl)
                {
                    //设置流层Buffer处理模式为OldestFirst:即先进先出原则
                    m_objIGXStreamFeatureControl.GetEnumFeature("StreamBufferHandlingMode").SetValue("OldestFirst");
                }
                //开启采集流通道
                if (null != m_objIGXStream)
                {
                    //RegisterCaptureCallback第一个参数属于用户自定参数(类型必须为引用
                    //类型)，若用户想用这个参数可以在委托函数中进行使用
                    // m_objIGXStream.RegisterCaptureCallback(this, __CaptureCallbackPro);  // 回调函数使用的是窗体线程
                    m_objIGXStream.StartGrab();
                    // 用于实时采集图像显示
                }
                //发送开采命令
                if (null != m_objIGXFeatureControl)
                {
                    m_objIGXFeatureControl.GetCommandFeature("AcquisitionStart").Execute();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private CancellationTokenSource cts;
        /// <summary>
        /// 停止采集
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void StopAcq()
        {
            try
            {
                if (this.cts != null)
                    this.cts.Cancel();
                //发送停采命令
                if (null != m_objIGXFeatureControl)
                {
                    m_objIGXFeatureControl.GetCommandFeature("AcquisitionStop").Execute();
                    m_objIGXFeatureControl = null;
                }
                //关闭采集流通道
                if (null != m_objIGXStream)
                {
                    m_objIGXStream.StopGrab();
                    // 注销采集回调函数
                    m_objIGXStream.UnregisterCaptureCallback();
                }
            }
            catch (Exception ex)
            {
                // MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 设置触发模式:On/Off
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void SetTriggerMode(string strValue)
        {
            try
            {
                SetEnumValue("TriggerMode", strValue);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 设置触发源:Software/Line0/Line1/Line2/Line3
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void SetTriggerSource(string strValue)
        {
            try
            {
                SetEnumValue("TriggerSource", strValue);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 设置触发极性：RisingEdge/FallingEdge
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void SetTriggerActivation(string strValue)
        {
            try
            {
                SetEnumValue("TriggerActivation", strValue);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public string GetTriggerMode()
        {
            try
            {
                return GetEnumValue("TriggerMode");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return "";
            }
        }
        public string GetTriggerSource()
        {
            try
            {
                return GetEnumValue("TriggerSource");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return "";
            }
        }
        public string GetTriggerActivation()
        {
            try
            {
                return GetEnumValue("TriggerActivation");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return "";
            }
        }


        /// <summary>
        /// 发送软触发命令
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void SoftTrigger()
        {
            try
            {
                //发送软触发命令
                if (null != m_objIGXFeatureControl)
                {
                    if (GetTriggerMode() == "On")
                        m_objIGXFeatureControl.GetCommandFeature("TriggerSoftware").Execute();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 对枚举型变量按照功能名称设置值,用来设置采集模式、增益、曝光等参数
        /// </summary>
        /// <param name="strFeatureName">枚举功能名称</param>
        /// <param name="strFeatureValue">功能的值</param>
        private void SetEnumValue(string strFeatureName, string strFeatureValue)
        {
            if (null != this.m_objIGXFeatureControl)
            {
                //设置当前功能值，通过枚举属性获取属性，然后给属性赋值
                this.m_objIGXFeatureControl.GetEnumFeature(strFeatureName).SetValue(strFeatureValue);
            }
        }
        private string GetEnumValue(string strFeatureName)
        {
            if (null != this.m_objIGXFeatureControl)
            {
                //设置当前功能值，通过枚举属性获取属性，然后给属性赋值
                return this.m_objIGXFeatureControl.GetEnumFeature(strFeatureName).GetValue();
            }
            return "";
        }

        /// <summary>
        /// 曝光控制界面初始化
        /// </summary>
        public double[] GetShutterValue()
        {
            double dCurShuter = 0.0;                       //当前曝光值
            double dMin = 0.0;                       //最小值
            double dMax = 0.0;                       //最大值
            string strUnit = "";                        //单位
            //获取当前相机的曝光值、最小值、最大值和单位
            if (null != m_objIGXFeatureControl)
            {
                dCurShuter = m_objIGXFeatureControl.GetFloatFeature("ExposureTime").GetValue();
                dMin = m_objIGXFeatureControl.GetFloatFeature("ExposureTime").GetMin();
                dMax = m_objIGXFeatureControl.GetFloatFeature("ExposureTime").GetMax();
                strUnit = m_objIGXFeatureControl.GetFloatFeature("ExposureTime").GetUnit();
            }
            return new double[3] { dCurShuter, dMin, dMax };
        }
        public void SetShutterValue(double dShutterValue)
        {
            // double dShutterValue = 0.0;              //曝光值
            double dMin = 0.0;                       //最小值
            double dMax = 0.0;                       //最大值
            try
            {
                //获取当前相机的曝光值、最小值、最大值和单位
                if (null != m_objIGXFeatureControl)
                {
                    dMin = m_objIGXFeatureControl.GetFloatFeature("ExposureTime").GetMin();
                    dMax = m_objIGXFeatureControl.GetFloatFeature("ExposureTime").GetMax();
                    //判断输入值是否在曝光时间的范围内
                    //若大于最大值则将曝光值设为最大值
                    if (dShutterValue > dMax)
                    {
                        dShutterValue = dMax;
                    }
                    //若小于最小值将曝光值设为最小值
                    if (dShutterValue < dMin)
                    {
                        dShutterValue = dMin;
                    }
                    m_objIGXFeatureControl.GetFloatFeature("ExposureTime").SetValue(dShutterValue);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 增益控制界面初始化
        /// </summary>
        public double[] GetGainValue()
        {
            double dCurGain = 0;             //当前增益值
            double dMin = 0.0;              //最小值
            double dMax = 0.0;             //最大值
            string strUnit = "";          //单位
            //获取当前相机的增益值、最小值、最大值和单位
            if (null != m_objIGXFeatureControl)
            {
                dCurGain = m_objIGXFeatureControl.GetFloatFeature("Gain").GetValue();
                dMin = m_objIGXFeatureControl.GetFloatFeature("Gain").GetMin();
                dMax = m_objIGXFeatureControl.GetFloatFeature("Gain").GetMax();
                strUnit = m_objIGXFeatureControl.GetFloatFeature("Gain").GetUnit();
            }
            return new double[3] { dCurGain, dMin, dMax };
        }
        public void SetGainValue(double dGain)
        {
            //double dGain = 0;            //增益值
            double dMin = 0.0;           //最小值
            double dMax = 0.0;           //最大值
            try
            {
                //当前相机的增益值、最小值、最大值
                if (null != m_objIGXFeatureControl)
                {
                    dMin = m_objIGXFeatureControl.GetFloatFeature("Gain").GetMin();
                    dMax = m_objIGXFeatureControl.GetFloatFeature("Gain").GetMax();
                    //判断输入值是否在增益值的范围内
                    //若输入的值大于最大值则将增益值设置成最大值
                    if (dGain > dMax)
                    {
                        dGain = dMax;
                    }
                    //若输入的值小于最小值则将增益的值设置成最小值
                    if (dGain < dMin)
                    {
                        dGain = dMin;
                    }
                    m_objIGXFeatureControl.GetFloatFeature("Gain").SetValue(dGain);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private  bool isSaveImage = false;
        private List<HImage> listImage = new List<HImage>();
        /// <summary>
        /// 回调函数,用于获取图像信息和显示图像
        /// </summary>
        /// <param name="obj">用户自定义传入参数</param>
        /// <param name="objIFrameData">图像信息对象</param>
        private void __CaptureCallbackPro(object objUserParam, IFrameData objIFrameData)  // 因为这里的回调函数是在UI线程上执行的，所以与其他指令共用一个线程，会发生争夺，产生冲突
        {
            try
            {
                if (m_objIGXFeatureControl != null && null != m_objIGXDevice)
                {
                    this.streamProcessState = true;
                    //获得图像原始数据大小、宽度、高度等; 因为图像是不断刷新的，所以这里不能使用锁来锁定
                    int m_nWidth = (int)m_objIGXDevice.GetRemoteFeatureControl().GetIntFeature("Width").GetValue();
                    int m_nHeigh = (int)m_objIGXDevice.GetRemoteFeatureControl().GetIntFeature("Height").GetValue();
                    HImage image = new HImage();
                    image.GenImage1("byte", m_nWidth, m_nHeigh, objIFrameData.GetBuffer());
                    if (this.isSaveImage)// 通过这个变量将实时显示与采集数据获取分开
                    {
                        this.listImage.Add(image);
                    }
                    else  // 生成一次图像30ms
                    {
                        OnImageAcqComplete(this.CameraParam.SensorName, new ImageDataClass(image, this.CameraParam)); // 将数据发送出去,图像数据应该包含一些相机的参数 DataInteractionClass.getInstance().
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.Error("大恒相机" + this.Name + "退出采集线程", ex);
            }
            this.streamProcessState = false; // 表示结束处理流数据
        }


        private HImage GetImage()
        {
            HImage image = new HImage();
            int m_nWidth, m_nHeigh;
            IImageData imageData;
            if (null != m_objIGXDevice)
            {
                //获得图像原始数据大小、宽度、高度等; 因为图像是不断刷新的，所以这里不能使用锁来锁定
                m_nWidth = (int)m_objIGXDevice.GetRemoteFeatureControl().GetIntFeature("Width").GetValue();
                m_nHeigh = (int)m_objIGXDevice.GetRemoteFeatureControl().GetIntFeature("Height").GetValue();
                imageData = m_objIGXStream.GetImageNoThrow(5000);
                if (imageData != null)
                    image.GenImage1("byte", m_nWidth, m_nHeigh, imageData.GetBuffer());
            }
            return image;
        }

        /// <summary>
        /// 使用内存法创建一幅灰度图像，数据类型为字节数组;内存法：将内存中的数据复制出来放到一个数组中再进行操作
        /// </summary>
        /// <param name="Data"></param>
        /// <param name="imageWidth">给定图像的宽</param>
        /// <param name="imageHeight">给定图像的高</param>
        /// <returns>返回BitMap</returns>
        public Bitmap CreateGrayImage_Memory(IntPtr intptr, int imageWidth, int imageHeight)
        {
            // 新建一个8位灰度位图，并锁定内存区域操作
            Bitmap bitmap = new Bitmap(imageWidth, imageHeight, System.Drawing.Imaging.PixelFormat.Format8bppIndexed);
            BitmapData bmpData = bitmap.LockBits(new Rectangle(0, 0, imageWidth, imageHeight), ImageLockMode.ReadWrite, System.Drawing.Imaging.PixelFormat.Format8bppIndexed);
            byte[] RawValues = new byte[imageWidth * imageHeight];
            Marshal.Copy(intptr, RawValues, 0, imageWidth * imageHeight);
            // 计算图像参数
            int offset = bmpData.Stride - bmpData.Width;        // 计算每行未用空间字节数,Stride:必需为4的倍数
            IntPtr ptr = bmpData.Scan0;                         // 获取首地址
            int scanBytes = bmpData.Stride * bmpData.Height;    // 图像字节数 = 扫描字节数 * 高度
            byte[] grayValues = new byte[scanBytes];            // 为图像数据分配内存
            // 为图像数据赋值
            int posSrc = 0, posScan = 0;                        // rawValues和grayValues的索引
            for (int i = 0; i < imageHeight; i++)
            {
                for (int j = 0; j < imageWidth; j++)
                {
                    grayValues[posScan++] = RawValues[posSrc++];
                }
                // 跳过图像数据每行未用空间的字节，length = stride - width * bytePerPixel
                posScan += offset;
            }
            // 内存解锁
            Marshal.Copy(grayValues, 0, ptr, scanBytes);
            bitmap.UnlockBits(bmpData);  // 解锁内存区域
            // 修改生成位图的索引表，从伪彩修改为灰度
            ColorPalette palette;
            // 获取一个Format8bppIndexed格式图像的Palette对象
            using (Bitmap bmp = new Bitmap(1, 1, System.Drawing.Imaging.PixelFormat.Format8bppIndexed))
            {
                palette = bmp.Palette;
            }
            for (int i = 0; i < 256; i++)
            {
                palette.Entries[i] = Color.FromArgb(i, i, i);
            }
            // 修改生成位图的索引表
            bitmap.Palette = palette;
            return bitmap;
        }

    }


    public enum enAcquisitionMode
    {
        SingleFrame,
        Continuous
    }
    public enum enFeatureName
    {
        DeviceVendorName,
        DeviceModelName,
        DeviceVersion,
        DeviceFirmwareVersion,
        DeviceSerialNumber,
        FactorySettingVersion,
        DeviceUserID,
        DeviceReset,
        TimestampTickFrequency,
        TimestampLatch,
        TimestampReset,
        TimestampLatchReset,
        TimestampLatchValue,
        SensorWidth,
        SensorHeight,
        WidthMax,
        HeightMax,
        Width,
        Height,
        OffsetX,
        OffsetY,
        PixelFormat,
        PixelSize,
        PixelColorFilter,
        TestPatternGeneratorSelector,
        TestPattern,
        BinningHorizontalMode,
        BinningHorizontal,
        BinningVerticalMode,
        BinningVertical,
        DecimationHorizontal,
        DecimationVertical,
        ReverseX,
        ReverseY,
        AcquisitionMode,
        AcquisitionStart,
        AcquisitionStop,
        AcquisitionBurstFrameCount,
        AcquisitionStatusSelector,
        AcquisitionStatus,
        TriggerSelector,
        TriggerMode,
        TriggerSource,
        TriggerSoftware,
        TriggerActivation,
        TriggerDelay,
        TriggerFilterRaisingEdge,
        TriggerFilterFallingEdge,
        ExposureMode,
        ExposureTime,
        ExposureDelay,
        ExposureAuto,
        AutoExposureTimeMin,
        AutoExposureTimeMax,
        AAROIWidth,
        AAROIHeight,
        AAROIOffsetX,
        AAROIOffsetY,
        ExpectedGrayValue,
        AcquisitionFrameRateMode,
        AcquisitionFrameRate,
        CurrentAcquisitionFrameRate,
        LineSelector,
        LineMode,
        LineInverter,
        LineSource,
        LineStatus,
        LineStatusAll,
        UserOutputSelector,
        UserOutputValue,
        TimerSelector,
        TimerDuration,
        TimerDelay,
        TimerTriggerSource,
        CounterSelector,
        CounterEventSource,
        CounterResetSource,
        CounterResetActivation,
        CounterReset,
        EventSelector,
        EventNotification,
        EventExposureEnd,
        EventExposureEndFrameID,
        EventExposureEndTimestamp,
        EventBlockDiscard,
        EventBlockDiscardTimestamp,
        EventFrameStartOvertrigger,
        EventFrameStartOvertriggerTimestamp,
        EventFrameBurstStartOvertrigger,
        EventFrameBurstStartOvertriggerFrameID,
        EventFrameBurstStartOvertriggerTimestamp,
        EventFrameStartWait,
        EventFrameStartWaitTimestamp,
        EventFrameBurstStartWait,
        EventFrameBurstStartWaitTimestamp,
        EventBlockNotEmpty,
        EventBlockNotEmptyTimestamp,
        EventOverrun,
        EventOverrunTimestamp,
        EventInternalError,
        EventInternalErrorTimestamp,
        GainSelector,
        Gain,
        GainAuto,
        AutoGainMin,
        AutoGainMax,
        BalanceRatioSelector,
        BalanceRatio,
        BalanceWhiteAuto,
        AWBLampHouse,
        AWBROIWidth,
        AWBROIHeight,
        AWBROIOffsetX,
        AWBROIOffsetY,
        DeadPixelCorrect,
        FixedPatternNoiseCorrectMode,
        GammaEnable,
        GammaMode,
        Gamma,
        BlackLevelSelector,
        BlackLevel,
        DigitalShift,
        LUTSelector,
        LUTEnable,
        LUTIndex,
        LUTValue,
        LUTValueAll,
        ColorTransformationEnable,
        ColorTransformationMode,
        ColorTransformationValueSelector,
        ColorTransformationValue,
        PayloadSize,
        GevVersionMajor,
        GevVersionMinor,
        GevDeviceModeIsBigEndian,
        GevDeviceModeCharacterSet,
        GevLinkSpeed,
        GevMACAddress,
        GevSupportedOptionSelector,
        GevSupportedOption,
        GevCurrentIPConfigurationLLA,
        GevCurrentIPConfigurationDHCP,
        GevCurrentIPConfigurationPersistentIP,
        GevCurrentIPAddress,
        GevCurrentSubnetMask,
        GevCurrentDefaultGateway,
        GevFirstURL,
        GevSecondURL,
        GevPersistentIPAddress,
        GevPersistentSubnetMask,
        GevPersistentDefaultGateway,
        GevMessageChannelCount,
        GevStreamChannelCount,
        GevHeartbeatTimeout,
        GevSCPSPacketSize,
        GevSCPD,
        BandwidthReserve,
        GevNumberOfInterfaces,
        EstimatedBandwidth,
        TransferControlMode,
        TransferOperationMode,
        TransferBlockCount,
        TransferStart,
        FrameBufferOverwriteActive,
        UserSetSelector,
        UserSetLoad,
        UserSetSave,
        UserSetDefault,
        ChunkModeActive,
        ChunkFrameInterval,
        ChunkFrameID,
        ChunkCounterValue,
        RemoveParameterLimit,
        SharpnessMode,
        Sharpness,
        FlatFieldCorrection
    }

}

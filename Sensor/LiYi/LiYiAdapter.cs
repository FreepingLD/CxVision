using Sensor;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Common;

namespace Sensor
{
    public class LiYiAdapter
    {
        private float[] _standardThickValue = new float[] { 600 }; // 当测量厚度时，这个参数一定要设置
        private uint _mMode = 0;
        private int _deviceCount = 0;
        private int _hDevice;   //控制器句柄
        private int _acqCount = 1000;  //待采集数据点数，这个参数必需要设置 
        private List<List<double[]>> _ChannelData = new List<List<double[]>>();
        private List<List<float[]>> _ChannelIntensity = new List<List<float[]>>();
        private double[] dist;
        private int frequency = 1000;
        private float exposure = 0.01f;
        private float gain = 1.0f;
        private int getChannels = 0;
        private float fullRange = 0;
        private enPeakType peakMode = enPeakType.NONE;
        private enUserMeasureMode MeasureMode = enUserMeasureMode.Distance;
        //
        private enUserTrigerMode _triggerMode = enUserTrigerMode.NONE;
        private enUserTriggerSource _triggerSource = enUserTriggerSource.软触发;
        private int _dark;
        private string[] paramPath;

        public int Frequency
        {
            get
            {
                if (this._hDevice > 0)
                    LEConfocalDLL.LE_GetSampleFrequency(ref this.frequency, this._hDevice);
                else
                    this.frequency = 0;
                return this.frequency;
            }
            set
            {
                if (value < 0)
                    frequency = 100;
                else
                    frequency = value;
                if (this._hDevice > 0)
                {
                    int maxFre = 0;
                    LEConfocalDLL.LE_GetLimitSampleFrequency(ref maxFre, this._hDevice);
                    if (value > maxFre)
                        frequency = maxFre;
                    LEConfocalDLL.LE_SetSampleFrequency(this.frequency, this._hDevice);
                }

            }
        }
        public float Exposure
        {
            get
            {
                if (this._hDevice > 0)
                    LEConfocalDLL.LE_GetExposureTime(ref this.exposure, this._hDevice);
                else
                    this.exposure = 0;
                return exposure;
            }

            set
            {
                float minExposure = 0;
                float maxExposure = 1;
                if (this._hDevice > 0)
                {
                    LEConfocalDLL.LE_GetExposureRange(ref minExposure, ref maxExposure, this._hDevice);
                    if (value < minExposure)
                        value = minExposure;
                    if (value > maxExposure)
                        value = maxExposure;
                    this.exposure = value;
                    LEConfocalDLL.LE_SetExposureTime(this.exposure, this._hDevice);
                }
            }
        }
        public float Gain
        {
            get
            {
                if (this._hDevice > 0)
                    LEConfocalDLL.LE_GetGain(ref this.gain, this._hDevice);
                else
                    this.gain = 0.0f;
                return gain;
            }

            set
            {
                float minGain = 0;
                float maxGain = 1;
                if (this._hDevice > 0)
                {
                    LEConfocalDLL.LE_GetGainRange(ref minGain, ref maxGain, this._hDevice);
                    if (value < minGain)
                        value = minGain;
                    if (value > maxGain)
                        value = maxGain;
                    this.gain = value;
                    //////////////////////////////////////////////
                    LEConfocalDLL.LE_SetExposureTime(this.gain, this._hDevice);
                }
            }
        }
        public int GetChannels
        {
            get
            {
                if (this._hDevice > 0)
                    this.getChannels = LEConfocalDLL.LE_GetChannels(this._hDevice);
                else
                    this.getChannels = 0;
                return getChannels;
            }
        }
        public float FullRange
        {
            get
            {
                if (this._hDevice > 0)
                    LEConfocalDLL.LE_GetDistanceRange(ref this.fullRange, this._hDevice);
                else
                    this.fullRange = 0;
                return fullRange;
            }
        }
        public enPeakType PeakMode
        {
            get
            {
                if (this._hDevice > 0)
                {
                    // 0 - 取最优峰；1 - 取最近峰；2 - 取最远锋
                    int peak = LEConfocalDLL.LE_GetCapturePeakMode(this._hDevice);
                    switch (peak)
                    {
                        case 0:
                            peakMode = enPeakType.最强峰;
                            break;
                        case 1:
                            peakMode = enPeakType.最近峰;
                            break;
                        case 2:
                            peakMode = enPeakType.最远峰;
                            break;
                        case 3: // 自定义模式
                            peak = LEConfocalDLL.LE_GetDistancePeakIdx(this._hDevice);
                            switch (peak)
                            {
                                case 1:
                                    peakMode = enPeakType.第一峰;
                                    break;
                                case 2:
                                    peakMode = enPeakType.第二峰;
                                    break;
                                case 3:
                                    peakMode = enPeakType.第三峰;
                                    break;
                                case 4:
                                    peakMode = enPeakType.第四峰;
                                    break;
                                case 5:
                                    peakMode = enPeakType.第五峰;
                                    break;
                                case 6:
                                    peakMode = enPeakType.第六峰;
                                    break;
                                case 7:
                                    peakMode = enPeakType.第七峰;
                                    break;
                                case 8:
                                    peakMode = enPeakType.第八峰;
                                    break;
                                case 9:
                                    peakMode = enPeakType.第九峰;
                                    break;
                                case 10:
                                    peakMode = enPeakType.第十峰;
                                    break;
                            }
                            break;
                        default:
                            break;
                    }
                    return peakMode;
                }
                else
                    return enPeakType.NONE;

            }

            set
            {
                if (this._hDevice > 0)
                {
                    switch (value)
                    {
                        case enPeakType.最强峰:
                            LEConfocalDLL.LE_SetCapturePeakMode(0, this._hDevice);
                            peakMode = enPeakType.最强峰;
                            break;
                        case enPeakType.最近峰:
                            LEConfocalDLL.LE_SetCapturePeakMode(1, this._hDevice);
                            peakMode = enPeakType.最近峰;
                            break;
                        case enPeakType.最远峰:
                            LEConfocalDLL.LE_SetCapturePeakMode(2, this._hDevice);
                            peakMode = enPeakType.最远峰;
                            break;

                        case enPeakType.第一峰:
                            LEConfocalDLL.LE_SetCapturePeakMode(3, this._hDevice);
                            LEConfocalDLL.LE_SetDistancePeakIdx(1, this._hDevice);
                            peakMode = enPeakType.第一峰;
                            break;
                        case enPeakType.第二峰:
                            LEConfocalDLL.LE_SetCapturePeakMode(3, this._hDevice);
                            LEConfocalDLL.LE_SetDistancePeakIdx(2, this._hDevice);
                            peakMode = enPeakType.第二峰;
                            break;
                        case enPeakType.第三峰:
                            LEConfocalDLL.LE_SetCapturePeakMode(3, this._hDevice);
                            LEConfocalDLL.LE_SetDistancePeakIdx(3, this._hDevice);
                            peakMode = enPeakType.第三峰;
                            break;
                        case enPeakType.第四峰:
                            LEConfocalDLL.LE_SetCapturePeakMode(3, this._hDevice);
                            LEConfocalDLL.LE_SetDistancePeakIdx(4, this._hDevice);
                            peakMode = enPeakType.第四峰;
                            break;
                        case enPeakType.第五峰:
                            LEConfocalDLL.LE_SetCapturePeakMode(3, this._hDevice);
                            LEConfocalDLL.LE_SetDistancePeakIdx(5, this._hDevice);
                            peakMode = enPeakType.第五峰;
                            break;
                        case enPeakType.第六峰:
                            LEConfocalDLL.LE_SetCapturePeakMode(3, this._hDevice);
                            LEConfocalDLL.LE_SetDistancePeakIdx(6, this._hDevice);
                            peakMode = enPeakType.第六峰;
                            break;
                        case enPeakType.第七峰:
                            LEConfocalDLL.LE_SetCapturePeakMode(3, this._hDevice);
                            LEConfocalDLL.LE_SetDistancePeakIdx(7, this._hDevice);
                            peakMode = enPeakType.第七峰;
                            break;
                        case enPeakType.第八峰:
                            LEConfocalDLL.LE_SetCapturePeakMode(3, this._hDevice);
                            LEConfocalDLL.LE_SetDistancePeakIdx(8, this._hDevice);
                            peakMode = enPeakType.第八峰;
                            break;
                        case enPeakType.第九峰:
                            LEConfocalDLL.LE_SetCapturePeakMode(3, this._hDevice);
                            LEConfocalDLL.LE_SetDistancePeakIdx(9, this._hDevice);
                            peakMode = enPeakType.第九峰;
                            break;
                        case enPeakType.第十峰:
                            LEConfocalDLL.LE_SetCapturePeakMode(3, this._hDevice);
                            LEConfocalDLL.LE_SetDistancePeakIdx(10, this._hDevice);
                            peakMode = enPeakType.第十峰;
                            break;
                        default:
                            peakMode = enPeakType.NONE;
                            break;
                    }
                }
            }
        }
        public int DeviceCount
        {
            get
            {
                return _deviceCount;
            }

            set
            {
                _deviceCount = value;
            }
        }
        public float[] StandardThickValue
        {
            get
            {
                return _standardThickValue;
            }

            set
            {
                _standardThickValue = value;
            }
        }
        public int AcqCount
        {
            get
            {
                return _acqCount;
            }

            set
            {
                _acqCount = value;
            }
        }
        public enUserTrigerMode TriggerMode
        {
            get
            {
                return _triggerMode;
            }

            set
            {
                _triggerMode = value;
                switch (_triggerMode)
                {
                    case enUserTrigerMode.NONE:
                        LEConfocalDLL.LE_SetTriggerMode(0, this._hDevice);// 0:实时采集
                        break;
                    case enUserTrigerMode.TRS:
                        LEConfocalDLL.LE_SetTriggerMode(2, this._hDevice);// 2:硬触发
                        LEConfocalDLL.LE_SetTriggerSource(0, this._hDevice); // 上升沿触发 ，每触发一次采集一个点
                        break;
                    case enUserTrigerMode.TRE:
                        LEConfocalDLL.LE_SetTriggerMode(2, this._hDevice);// 2:硬触发
                        LEConfocalDLL.LE_SetTriggerSource(0, this._hDevice); // 上升沿触发 ，每触发一次采集一个点
                        break;
                    case enUserTrigerMode.TRN:
                        LEConfocalDLL.LE_SetTriggerMode(2, this._hDevice); // 2:硬触发
                        LEConfocalDLL.LE_SetTriggerSource(2, this._hDevice); // 高电平采集
                        break;
                    default:
                        break;
                }
                LEConfocalDLL.LE_SetTriggerMode(0, this._hDevice);// 0:实时采集
            }
        }
        public enUserTriggerSource TriggerSource
        {
            get
            {
                return _triggerSource;
            }

            set
            {
                _triggerSource = value;
                switch (_triggerSource)
                {
                    case enUserTriggerSource.NONE:
                        LEConfocalDLL.LE_SetTriggerMode(0, this._hDevice);
                        break;
                    case enUserTriggerSource.软触发:
                        LEConfocalDLL.LE_SetTriggerMode(0, this._hDevice);
                        break;
                    case enUserTriggerSource.外部IO触发:
                        LEConfocalDLL.LE_SetTriggerMode(2, this._hDevice);
                        break;
                    default:
                        break;
                }

            }
        }
        public int Dark
        {
            get
            {
                _dark = LEConfocalDLL.LE_CaptureChannelsIntensityCaliData(this._hDevice);
                return _dark;
            }
        }




        public bool InitDLL()
        {
            try
            {
                LEConfocalDLL.LE_UnInitDLL(); // 每次初始化时保存没有初始化过
                LEConfocalDLL.LE_SelectDeviceType(2);   //选择需使用的控制器类型接口，当前选择USB2代控制器          
                int iSta = LEConfocalDLL.LE_InitDLL();  //初始化传感器DLL
                _deviceCount = LEConfocalDLL.LE_GetSpecCount();
                switch (iSta)
                {
                    case 1:
                        return true;
                    case -29:
                        // MessageBox.Show("未找到匹配的加密狗");
                        return false;
                    case -30:
                        // MessageBox.Show("USB一代控制器初始化失败，请检查是否已安装USB一代最新版驱动");
                        return false;
                    case -31:
                        // MessageBox.Show("以太网控制器初始化失败，请检查是否已安装以太网版本驱动");
                        return false;
                    case -32:
                        //  MessageBox.Show("USB二代控制器初始化失败，请检查是否已安装USB二代最新版驱动");
                        return false;
                    default:
                        // MessageBox.Show("未知错误");
                        return false;
                }
            }
            catch (Exception e)
            {
                // MessageBox.Show(e.ToString());
                return false;
            }
        }

        public bool UnInitDLL()
        {
            if (LEConfocalDLL.LE_UnInitDLL()) return true;
            return false;
        }

        public bool Connect(string sensorName,string configName)
        {
            int iSta = 0;
            if (this._deviceCount > 0)
            {
                //打开第一个序列号的传感器
                if (LEConfocalDLL.LE_Open(sensorName.ToCharArray(), ref _hDevice) == 1)
                {
#if DEBUG
                    //debug模式下加长控制器连接的心跳时间，否则控制器断点调试时程序中断时间超过心跳时间，控制器将断开与程序的连接
                    iSta = LEConfocalDLL.LE_SetHeartTime(30000, _hDevice);
#endif                  
                    StringBuilder paramfilePath = new StringBuilder(Directory.GetFiles(configName, "*.dcfg")[0]); // 获取指定后缀的文件
                    iSta = LEConfocalDLL.LE_LoadDeviceConfigureFromFile(paramfilePath, _hDevice);//载入控制器配置文件，该文件必须路径正确且与当前使用控制器序号匹配
                    int iChannels = LEConfocalDLL.LE_GetChannels(_hDevice);
                    this.paramPath = new string[iChannels];
                    for (int i = 1; i <= iChannels; i++)
                    {
                        paramPath[i-1] = configName; // 每个通道的配置文件
                        iSta = LEConfocalDLL.LE_EnableChannel(_hDevice, i, true);
                        iSta = LEConfocalDLL.LE_LoadChannelConfigureFromFile(paramfilePath, _hDevice, i);  // 控制器配置文件与通道配置文件有啥区别？
                        StringBuilder lwfilePath = new StringBuilder(Directory.GetFiles(configName, "*.hwc")[0]);
                        iSta = LEConfocalDLL.LE_LoadLWCalibrationData(lwfilePath, _hDevice, i);//载入控制器校准文件，该文件必须路径正确且与当前使用控制器序号匹配
                        int count = 0;
                        StringBuilder TKfilePath = new StringBuilder(Directory.GetFiles(configName, "*.tkc")[0]);
                        //StringBuilder TKfilePath = new StringBuilder(@"D:\立移\光谱共焦传感器开发包var1.77.3_X86X64_20200603\开源Demo\C#\ConfocalDemoCSharpVer1.1_X64\ConfocalDemoCSharpVar1.0\Configure\DefaultTKC.tkc");
                        iSta = LEConfocalDLL.LE_LoadTKCalibrationData(ref count, TKfilePath, _hDevice, i);//载入控制器校准文件，该文件必须路径正确且与当前使用控制器序号匹配
                    }
                    return true;
                }
                return false;
            }
            return false;
        }

        public bool DisConnect()
        {
            if (LEConfocalDLL.LE_Close(ref this._hDevice) == 1) return true;
            return false;
        }

        public bool SetParameter( enUserTrigerMode TriggerMode, enUserMeasureMode measureMode, int AcqPoints)
        {
            this.MeasureMode = measureMode;
            int ChannelCount = LEConfocalDLL.LE_GetChannels(this._hDevice);
            for (int i = 0; i < ChannelCount; i++)
            {
                LEConfocalDLL.LE_EnableAppFuncation(0, false, this._hDevice, i);
                LEConfocalDLL.LE_EnableAppFuncation(1, false, this._hDevice, i);
                LEConfocalDLL.LE_SetInvalidDataPrdModel(2, this._hDevice, i);// 无效数据用0填充
            }
            LEConfocalDLL.LE_EnableFillLostFrameStatus(false, this._hDevice);    // 丢帧插补功能    

            //设置触发模式
            switch (TriggerMode)
            {
                case enUserTrigerMode.NONE: //NONE
                    LEConfocalDLL.LE_SetTriggerMode(0, this._hDevice);// 0:实时采集
                    this._acqCount = AcqPoints;
                    break;
                case enUserTrigerMode.TRE: //TRE
                    LEConfocalDLL.LE_SetTriggerMode(2, this._hDevice);// 2:硬触发
                    LEConfocalDLL.LE_SetTriggerSource(0, this._hDevice); // 上升沿触发 ，每触发一次采集一个点
                    this._acqCount = AcqPoints;
                    break;
                case enUserTrigerMode.TRN: // TRN
                    LEConfocalDLL.LE_SetTriggerMode(2, this._hDevice); // 2:硬触发
                    LEConfocalDLL.LE_SetTriggerSource(2, this._hDevice); // 高电平采集
                    this._acqCount = AcqPoints;
                    break;
                default:
                    LEConfocalDLL.LE_SetTriggerMode(0, this._hDevice);// 0:实时采集
                    this._acqCount = AcqPoints;
                    break;
            }
            return true;
        }

        /// <summary>
        /// 适用于多通道控制器
        /// </summary>
        /// <returns></returns>
        public bool StartAcquisition()
        {
            // 获到完该位置的数据后再清空
            this._ChannelData.Clear();
            this._ChannelIntensity.Clear();
            /////////////////
            int ChannelCount = LEConfocalDLL.LE_GetChannels(this._hDevice);
            try
            {
                // 设置距离模式下的数据存储
                if (MeasureMode == enUserMeasureMode.Distance)
                {
                    for (int Channel = 0; Channel < ChannelCount; Channel++)
                    {
                        // 分配距离内存
                        this._ChannelData.Add(new List<double[]>());
                        this._ChannelData[Channel].Add(new double[this._acqCount]);
                        // 分配光强内存
                        this._ChannelIntensity.Add(new List<float[]>());
                        this._ChannelIntensity[Channel].Add(new float[this._acqCount]);
                        /////////////////////////
                        dist = new double[this._acqCount];//this._ChannelData[Channel][0]
                        LEConfocalDLL.LE_SetChannelGetAllValues(this._ChannelData[Channel][0], this._acqCount, this._hDevice, Channel + 1, this._ChannelIntensity[Channel][0]); // 设置每个通道数据存储的位置
                    }
                }
                // 设置厚度模式下的数据存储 
                if (MeasureMode == enUserMeasureMode.Thickness)
                {
                    int tkCount = this._standardThickValue.Length; // 获取要测量的厚度数量,根据标准值的数量来确定要测量的厚度层数
                    TKPeakIdx[] tkPeakIndex = new TKPeakIdx[tkCount];
                    //////////////////////
                    for (int Channel = 0; Channel < ChannelCount; Channel++)
                    {
                        // 分配距离内存
                        this._ChannelData.Add(new List<double[]>());
                        this._ChannelData[Channel].Add(new double[this._acqCount * tkCount]);
                        this._ChannelData[Channel].Add(new double[this._acqCount * tkCount]);
                        this._ChannelData[Channel].Add(new double[this._acqCount * tkCount]);
                        // 分配光强内存
                        this._ChannelIntensity.Add(new List<float[]>());
                        this._ChannelIntensity[Channel].Add(new float[this._acqCount]);
                        /////////////////////////                  
                        int[] pTkIdx = new int[tkCount];
                        for (int i = 0; i < tkCount; i++)
                        {
                            pTkIdx[i] = i + 1;
                            tkPeakIndex[i] = new TKPeakIdx(i + 1, i + 2, i + 1);
                        }
                        SetTKPeakIndexes(Channel + 1, tkPeakIndex);// 这里的变化跟DEM中的设置是不是一致的？还是只是单独会变？
                        int iSta = LEConfocalDLL.LE_SetChannelGetThicknessAllValuesParamEx(this._ChannelData[Channel][0], this._acqCount, tkCount, this._hDevice, Channel + 1, this._ChannelData[Channel][1], this._ChannelData[Channel][2], this._ChannelIntensity[Channel][0]); // 设置每个通道数据存储的位置
                        iSta = LEConfocalDLL.LE_CaliTKRaEx(this._standardThickValue, pTkIdx, tkCount, this._hDevice, Channel + 1); // 测量厚度时这一步必需要
                    }
                }
                if (LEConfocalDLL.LE_StartGetChannelsValues(this._hDevice) == 1) return true;
            }
            catch
            {
            }
            return false;
        }

        /// <summary>
        /// 停止数据记录
        /// </summary>
        /// <returns></returns>
        public bool StopAcquisition()
        {
            try
            {
                if (LEConfocalDLL.LE_StopGetPoints(this._hDevice) == 1) return true;
            }
            catch
            { }
            return false;
        }

        /// <summary>
        /// 获取厚度模式下各层的峰索引
        /// </summary>
        /// <param name="channel"></param>
        /// <returns></returns>
        public TKPeakIdx[] GetTKPeakIndexes(int tkCount, int channel)
        {
            TKPeakIdx[] tKPeakIdx = new TKPeakIdx[tkCount];
            try
            {
                byte[] pTKPeakIdxs = new byte[Marshal.SizeOf(typeof(TKPeakIdx)) * tkCount]; // 默认值5
                LEConfocalDLL.LE_GetTKPeakIdxs(pTKPeakIdxs, tkCount, this._hDevice, channel);
                LEConfocalDLL.ConvertBytesToTKPeakIdx(pTKPeakIdxs, tKPeakIdx, tkCount);
            }
            catch
            {
                throw new Exception();
            }
            return tKPeakIdx;
        }

        /// <summary>
        /// 设置厚度模式下各层的峰索引
        /// </summary>
        /// <param name="pTKPeakIdxs"></param>
        /// <param name="tkCount"></param>
        /// <param name="channel"></param>
        public void SetTKPeakIndexes(int channel, params TKPeakIdx[] pTKPeakIdxs)
        {
            int tkCount = pTKPeakIdxs.Length;
            try
            {
                LEConfocalDLL.LE_SetTKPeakIdxs(pTKPeakIdxs, tkCount, this._hDevice, channel);
            }
            catch
            {
                throw new Exception();
            }
        }

        public void GetData(out double[] Distance, out double[] Distance2, out double[] Thickness, out double[] Intensity, out double[] EncoderX, out double[] EncoderY, out double[] EncoderZ, int channel = 1)
        {
            Thickness = null;
            Distance = null;
            Distance2 = null;
            EncoderX = null;
            EncoderY = null;
            EncoderZ = null;
            Intensity = null;
            List<double> listThickness = new List<double>();
            List<double> listDistance = new List<double>();
            List<double> listDistance2 = new List<double>();
            List<double> listEncoderX = new List<double>();
            List<double> listEncoderY = new List<double>();
            List<double> listEncoderZ = new List<double>();
            List<double> listIntensity = new List<double>();
            //////////////////////////
            try
            {
                int Num = GetCapturedPoints();
                if (MeasureMode == enUserMeasureMode.Distance)
                {
                    for (int i = 0; i < Num; i++)
                    {
                        if (this._ChannelData[channel - 1][0][i] > 0)
                        {
                            listDistance.Add((this._ChannelData[channel - 1][0][i] * 0.001));
                            listIntensity.Add((this._ChannelIntensity[channel - 1][0][i]));
                        }
                    }
                    Distance = listDistance.ToArray();//
                    Intensity = listIntensity.ToArray();
                }
                //////////////////////////
                if (MeasureMode == enUserMeasureMode.Thickness)
                {
                    for (int i = 0; i < Num; i++)
                    {
                        if (this._ChannelData[channel - 1][1][i] > 0)
                        {
                            listThickness.Add((this._ChannelData[channel - 1][0][i] * 0.001));
                            listDistance.Add((this._ChannelData[channel - 1][1][i] * 0.001));
                            listDistance2.Add((this._ChannelData[channel - 1][2][i] * 0.001));
                            listIntensity.Add((this._ChannelIntensity[channel - 1][0][i]));
                        }
                    }
                    Thickness = listThickness.ToArray();
                    Distance = listDistance.ToArray();
                    Distance2 = listDistance2.ToArray();
                    Intensity = listIntensity.ToArray();
                }
                // 获到完该位置的数据后再清空
                this._ChannelData.Clear();
                this._ChannelIntensity.Clear();
            }
            catch
            {
               // throw new Exception();
            }
        }

        public string[] GetSensorName()
        {
            string[] sensorName = null;
            char[] mAryChar = new char[this._deviceCount * 32];
            if (this._deviceCount == 0) return sensorName;
            bool bSta = LEConfocalDLL.LE_GetSpecSN(mAryChar, this._deviceCount); //获取已连接控制器序列号 
            string name = "";
            for (int i = 0; i < mAryChar.Length / 32; i++)
            {
                byte[] B = Encoding.Default.GetBytes(mAryChar, i * 32, 32);
                name += Encoding.Default.GetString(B);
            }
            sensorName = new string[name.Length / 32];
            for (int i = 0; i < name.Length / 32; i++)
            {
                sensorName[i] = name.Substring(i * 32, 32).Trim('\0');
            }
            return sensorName;
        }

        public int GetCapturedPoints()
        {
            int points = 0;
            LEConfocalDLL.LE_GetCapturedPoints(ref points, this._hDevice, 1);
            return points;
        }

        public int GetDeviceStatus()
        {
            int state = LEConfocalDLL.LE_GetDeviceStatus(this._hDevice, 1);
            return state;
        }

        public bool SaveParam()
        {
            try
            {
                for (int i = 0; i < this.getChannels; i++)
                {
                    StringBuilder paramfilePath = new StringBuilder(paramPath[i] + ".dcfg");
                    LEConfocalDLL.LE_SaveParamToFile(paramfilePath, this._hDevice, i);
                }
                return true;
            }
            catch
            {
            }
            return false;
        }


    }








}

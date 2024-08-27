using Common;
using LJV7_DllSampleAll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Sensor
{
    public class LJV7000LineLaser : SensorBase, ISensor
    {
        private LJV7_DllSampleAll.HighSpeedDataCallBack _callback;
        private LJV7IF_PROFILE_INFO[] _profileInfo;
        private ThreadSafeBuffer threadSafeBuffer;
        private bool isSave = false;
        private int _currentDeviceId = 0;
        private enUserTriggerSource triggerSource = enUserTriggerSource.NONE;
        private KeyEnceLJV7000Setting keyEnceLJV7000ParamConfig;
        public KeyEnceLJV7000Setting KeyEnceLJV7000ParamConfig { get => keyEnceLJV7000ParamConfig; set => keyEnceLJV7000ParamConfig = value; }

        private SensorConnectConfigParam configParam;
        public LJV7000LineLaser()
        {
            _callback = new LJV7_DllSampleAll.HighSpeedDataCallBack(ReceiveHighSpeedData);
            threadSafeBuffer = new ThreadSafeBuffer();
            _profileInfo = new LJV7IF_PROFILE_INFO[NativeMethods.DeviceCount];
        }
        #region 实现接口
        public bool Connect(SensorConnectConfigParam configParam)
        {
            bool result = true;
            try
            {
                this.configParam = configParam;
                this.Name = configParam.SensorName;
                this.LaserParam = (LaserParam)new LaserParam().Read(this.Name);
                this.LaserParam.SensorName = configParam.SensorName;
                //base.readSensorParam(param); // 读取参数
                // 读取参数配置文件
                this.keyEnceLJV7000ParamConfig = new KeyEnceLJV7000Setting().ReadParamConfig(Application.StartupPath + "\\" + "LaserParam" + "\\" + configParam.SensorName + ".txt");
                int rc = -1;
                string connectAddress = configParam.ConnectAddress;//[1];
                ////////////////////////////////
                switch (configParam.ConnectType)
                {
                    case enUserConnectType.Network:
                        LJV7IF_ETHERNET_CONFIG ethernetConfig = new LJV7IF_ETHERNET_CONFIG();
                        ethernetConfig.abyIpAddress = new byte[4] {Convert.ToByte(connectAddress.Split(',','.')[0]) , Convert.ToByte(connectAddress.Split(',', '.')[1]),
                            Convert.ToByte(connectAddress.Split(',','.')[2]), Convert.ToByte(connectAddress.Split(',','.')[3]) };
                        ethernetConfig.wPortNo = 24691;
                        /////////////////////////////////
                        rc = NativeMethods.LJV7IF_EthernetOpen(_currentDeviceId, ref ethernetConfig);
                        if (rc == (int)Rc.Ok)
                        {
                            LoggerHelper.Info(this.LaserParam.SensorName + "网线连接成功");
                            result = true;
                        }
                        else
                        {
                            LoggerHelper.Error(this.LaserParam.SensorName + "网线连接失败");
                            result = false;
                        }
                        break;
                    case enUserConnectType.USB:
                        rc = NativeMethods.LJV7IF_UsbOpen(_currentDeviceId);
                        if (rc == (int)Rc.Ok)
                        {
                            LoggerHelper.Info(this.LaserParam.SensorName + "USB连接成功");
                            result = true;
                        }
                        else
                        {
                            LoggerHelper.Error(this.LaserParam.SensorName + "USB连接失败");
                            result = false;
                        }
                        break;
                    default:
                        result = false;
                        break;
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.Error(this.LaserParam.SensorName + "打开失败", ex);
                result = false;
            }
            configParam.ConnectState = result;
            return result;
        }
        public bool Disconnect()
        {
            bool result = false;
            try
            {
                int rc = NativeMethods.LJV7IF_CommClose(_currentDeviceId);
                if (rc == (int)Rc.Ok)
                {
                    LoggerHelper.Info(this.LaserParam.SensorName + "控制器断开成功");
                    result = true;
                }
                else
                {
                    LoggerHelper.Error(this.LaserParam.SensorName + "控制器断开失败");
                    result = false;
                }
                rc = NativeMethods.LJV7IF_Finalize();
                if (rc == (int)Rc.Ok)
                {
                    LoggerHelper.Info(this.LaserParam.SensorName + "DLL释放成功");
                    result = true;
                }
                else
                {
                    LoggerHelper.Error(this.LaserParam.SensorName + "DLL释放失败");
                    result = false;
                }
            }
            catch
            {
                result = false;
            }
            return result;
        }
        public bool Init()
        {
            try
            {
                int rc = NativeMethods.LJV7IF_Initialize();
                if (rc == (int)Rc.Ok)
                {
                    LoggerHelper.Info(this.LaserParam.SensorName + "DLL初始化成功");
                    return true;
                }
                else
                {
                    LoggerHelper.Error(this.LaserParam.SensorName + "DLL初始化失败");
                    return false;
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.Error("相机初始化失败", ex);
                return false;
            }
        }
        public Dictionary<enDataItem, object> ReadData()
        {
            uint notify = 0;
            int batchNo = 0;
            int _triggerCnt = 0;
            int _encoderCnt = 0;
            Dictionary<enDataItem, object> list = new Dictionary<enDataItem, object>();
            List<double> x = new List<double>();
            List<double> y = new List<double>();
            List<double> dist = new List<double>();
            List<double> z = new List<double>();
            //////////////////////////////////
            this.LaserParam.DataWidth = this._profileInfo[_currentDeviceId].wProfDataCnt;
            this.LaserParam.DataHeight = 1;
            List<int[]> data = threadSafeBuffer.GetData(0, out notify, out batchNo); // 获取第i个控制器的数据
                                                                                     //////////////////////////// foreach (var item in data)
            for (int i = 0; i < data.Count; i++)
            {
                // Extract the header.
                int headerSize = Utility.GetByteSize(Utility.TypeOfStruct.PROFILE_HEADER) / Marshal.SizeOf(typeof(int));
                int[] headerData = new int[headerSize];
                Array.Copy(data[i], 0, headerData, 0, headerSize);
                _triggerCnt = headerData[1];
                _encoderCnt = headerData[2];
                // Extract the footer.
                int footerSize = Utility.GetByteSize(Utility.TypeOfStruct.PROFILE_FOOTER) / Marshal.SizeOf(typeof(int));
                //int[] footerData = new int[footerSize];
                //Array.Copy(item, item.Length - footerSize, footerData, 0, footerSize);
                // Extract the profile data.
                int profSize = data[i].Length - headerSize - footerSize;
                for (int j = 0; j < profSize; j++)
                {
                    switch (this.LaserParam.ScanAxis)
                    {
                        case enScanAxis.X轴:
                            y.Add((this._profileInfo[0].lXPitch * j + this._profileInfo[0].lXStart) * 0.00001);
                            if(_triggerCnt== _encoderCnt)
                                x.Add(this.LaserParam.ScanStep * i);
                            else
                                x.Add((_encoderCnt / (_triggerCnt+1)) * 0.001*i);
                            dist.Add(data[i][j + headerSize] * 0.00001);
                            z.Add(0);
                            break;
                        case enScanAxis.Y轴:
                            x.Add((this._profileInfo[0].lXPitch * j + this._profileInfo[0].lXStart) * 0.00001);
                            if (_triggerCnt == _encoderCnt)
                                y.Add(this.LaserParam.ScanStep * i);
                            else
                                y.Add((_encoderCnt / (_triggerCnt + 1)) * 0.001 * i);
                            dist.Add(data[i][j + headerSize] * 0.00001);
                            z.Add(0);
                            break;
                        case enScanAxis.Z轴:
                            x.Add((this._profileInfo[0].lXPitch * j + this._profileInfo[0].lXStart) * 0.00001);
                            if (_triggerCnt == _encoderCnt)
                                z.Add(this.LaserParam.ScanStep * i);
                            else
                                z.Add((_encoderCnt / (_triggerCnt + 1)) * 0.001 * i);
                            dist.Add(data[i][j + headerSize] * 0.00001);
                            y.Add(0);
                            break;
                    }
                }
            }
            list.Add(enDataItem.Dist1,dist.ToArray());
            list.Add(enDataItem.X, x.ToArray());
            list.Add(enDataItem.Y, y.ToArray());
            list.Add(enDataItem.Z, z.ToArray());
            x.Clear();
            y.Clear();
            dist.Clear();
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
                case enSensorParamType.Coom_传感器名称:
                    return this.LaserParam.SensorName;
                case enSensorParamType.Coom_每线点数:
                    return this.LaserParam.DataWidth;
                case enSensorParamType.Coom_传感器类型:
                    return SensorConnectConfigParamManger.Instance.GetSensorConfigParam(this.Name).SensorType;
                case enSensorParamType.Coom_激光位姿:
                   // return this.LaserParam.laserPose;
                default:
                    return this.LaserParam.SensorName;
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
                case enSensorParamType.Coom_传感器名称:
                    this.LaserParam.SensorName = value.ToString();
                    return true;
                case enSensorParamType.Coom_每线点数:
                    this.LaserParam.DataWidth = (int)value;
                    return true;
                default:
                    return true;
            }
        }
        public bool StartTrigger() // 这个参数不再使用
        {
            switch (this.keyEnceLJV7000ParamConfig.TriggerSource)
            {
                case enUserTriggerSource.编码器触发:
                case enUserTriggerSource.NONE:
                    this.isSave = true;
                    return SetHighSpeedDataUsbCommunication(2);
                case enUserTriggerSource.外部IO触发:
                    this.isSave = true; // 开始保存图像
                    SetHighSpeedDataUsbCommunication(2);
                    int rc = NativeMethods.LJV7IF_Trigger(this._currentDeviceId);
                    if (rc == (int)Rc.Ok)
                        return true;
                    else
                        return false;
                case enUserTriggerSource.软触发: // 相当于批处理
                    this.isSave = true; // 开始保存图像
                    SetHighSpeedDataUsbCommunication(2);
                    rc = NativeMethods.LJV7IF_StartMeasure(this._currentDeviceId);
                    if (rc == (int)Rc.Ok)
                        return true;
                    else
                        return false;
                default:
                    return false;
            }
        }
        public bool StopTrigger()
        {
            switch (this.keyEnceLJV7000ParamConfig.TriggerSource)
            {
                case enUserTriggerSource.编码器触发:
                case enUserTriggerSource.NONE:
                    this.isSave = false;
                    return CancelHighSpeedDataUsbCommunication();
                ///////////////////////////////////
                case enUserTriggerSource.外部IO触发:
                    this.isSave = false;
                    return CancelHighSpeedDataUsbCommunication();
                ///////////////////////////////////
                case enUserTriggerSource.软触发:
                    this.isSave = false;
                    NativeMethods.LJV7IF_StopMeasure(this._currentDeviceId);
                    return CancelHighSpeedDataUsbCommunication();
                default:
                    return false;
            }
        }

        #endregion

        private void ReceiveHighSpeedData(IntPtr buffer, uint size, uint count, uint notify, uint user)
        {
            uint profileSize = (uint)(size / Marshal.SizeOf(typeof(int)));
            List<int[]> receiveBuffer = new List<int[]>();
            int[] bufferArray = new int[profileSize * count];
            Marshal.Copy(buffer, bufferArray, 0, (int)(profileSize * count));
            // Profile data retention
            for (int i = 0; i < count; i++)
            {
                int[] oneProfile = new int[profileSize];
                Array.Copy(bufferArray, i * profileSize, oneProfile, 0, profileSize);
                receiveBuffer.Add(oneProfile);
            }
            threadSafeBuffer.Add((int)user, receiveBuffer, notify);
        }

        private bool SetHighSpeedDataUsbCommunication(uint notifyProCount)
        {
            int rc;
            // 初始化开始高速数据通信
            if (this.configParam .ConnectType == enUserConnectType.USB)
                rc = NativeMethods.LJV7IF_HighSpeedDataUsbCommunicationInitalize(_currentDeviceId, _callback, notifyProCount, (uint)_currentDeviceId);
            else
            {
                LJV7IF_ETHERNET_CONFIG ethernetConfig = new LJV7IF_ETHERNET_CONFIG();
                ethernetConfig.abyIpAddress = new byte[4] {Convert.ToByte(this.configParam.ConnectAddress.Split(',','.')[0]) , Convert.ToByte(this.configParam.ConnectAddress.Split(',', '.')[1]),
                            Convert.ToByte(this.configParam.ConnectAddress.Split(',','.')[2]), Convert.ToByte(this.configParam.ConnectAddress.Split(',','.')[3]) };
                ethernetConfig.wPortNo = 24691;
                rc = NativeMethods.LJV7IF_HighSpeedDataEthernetCommunicationInitalize(_currentDeviceId, ref ethernetConfig, ethernetConfig.wPortNo, _callback, notifyProCount, (uint)_currentDeviceId);
            }
            // 准备开始高速数据通信
            LJV7IF_HIGH_SPEED_PRE_START_REQ req = new LJV7IF_HIGH_SPEED_PRE_START_REQ();
            req.bySendPos = 2;
            LJV7IF_PROFILE_INFO profileInfo = new LJV7IF_PROFILE_INFO();
            rc = NativeMethods.LJV7IF_PreStartHighSpeedDataCommunication(_currentDeviceId, ref req, ref profileInfo);
            _profileInfo[_currentDeviceId] = profileInfo; // 在准备时，
            // 开始高速数据通信
            threadSafeBuffer.ClearBuffer(_currentDeviceId);
            rc = NativeMethods.LJV7IF_StartHighSpeedDataCommunication(_currentDeviceId);
            if (rc == (int)Rc.Ok)
            {
                LoggerHelper.Info(this.LaserParam.SensorName + "开始高速数据通信成功");
                return true;
            }
            else
            {
                LoggerHelper.Error(this.LaserParam.SensorName + "开始高速数据通信失败");
                return false;
            }

        }
        private bool CancelHighSpeedDataUsbCommunication()
        {
            int rc;
            rc = NativeMethods.LJV7IF_StopHighSpeedDataCommunication(_currentDeviceId);
            rc = NativeMethods.LJV7IF_HighSpeedDataCommunicationFinalize(_currentDeviceId);
            //if (rc == (int)Rc.Ok)
            //    LoggerHelper.Info(this.name + "反初始化高速数据通信成功");
            //else
            //    LoggerHelper.Info(this.name + "反初始化高速数据通信失败");
            if (rc == (int)Rc.Ok)
            {
                LoggerHelper.Info(this.LaserParam.SensorName + "停止高速数据通信成功");
                return true;
            }
            else
            {
                LoggerHelper.Error(this.LaserParam.SensorName + "停止高速数据通信失败");
                return false;
            }

        }
    }
}

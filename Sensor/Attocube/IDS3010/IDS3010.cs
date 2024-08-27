using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Attocube.IDS;
using Attocube.API.Error;
using Attocube.API.Data;
using Attocube.API.JsonRpc;
using Attocube.API.Utils;
using System.Threading;
using System.Runtime.InteropServices;
using System.Diagnostics;
using HalconDotNet;
using Common;


namespace Sensor
{
    public class IDS3010 : SensorBase, ISensor
    {
        private AttocubeIDS attocube = new AttocubeIDS();
        private bool isSave = false;
        private Tuple<int, double, double, double> items;
        private CancellationTokenSource cts;
        private List<double> dist_axis1 = new List<double>();
        private List<double> dist_axis2 = new List<double>();
        private List<double> dist_axis3 = new List<double>();
        private enIDS3010AcqMode acqMode = enIDS3010AcqMode.单点采集;
        // 流参数
        private IntPtr stream;
        private int bufferLength = 1024;// (单位)kb



        public bool Init()
        {
           
            this.LaserParam.DataHeight = 1;
            this.LaserParam.DataWidth = 1;
            return true;
        }
        public bool Connect(SensorConnectConfigParam configParam)
        {
            bool result = false;
            try
            {
                this.ConfigParam = configParam;
                this.Name = this.ConfigParam.SensorName;
                this.LaserParam = (LaserParam)new LaserParam().Read(this.Name);
                this.LaserParam.SensorName = this.ConfigParam.SensorName; 
                switch (configParam.ConnectType)
                {
                    default:
                    case enUserConnectType.Network:
                        this.attocube.Connect(configParam.ConnectAddress, 9090);
                        if (this.attocube.Displacement_GetMeasurementEnabled()) // 表示是否测量
                            result = true;
                        else
                        {
                            if (this.attocube.Adjustment_GetAdjustmentEnabled()) // 如果在调整状态，那么要先停止再开始测量
                            {
                                this.attocube.System_StopOpticsAlignment();
                                this.attocube.System_StartMeasurement();
                            }
                            else
                                this.attocube.System_StartMeasurement();
                            /////////////////////////////////////
                            result = true;
                        }               
                        break;
                }
            }
            catch (AttocubeAPIException e1)
            {
                if (e1.Message == "Attocube.API is already connected")
                {
                    if (this.attocube.Displacement_GetMeasurementEnabled()) // 表示是否测量
                        result = true;
                    else
                    {
                        if (this.attocube.Adjustment_GetAdjustmentEnabled()) // 如果在调整状态，那么要先停止再开始测量
                        {
                            this.attocube.System_StopOpticsAlignment();
                            this.attocube.System_StartMeasurement();
                        }
                        else
                            this.attocube.System_StartMeasurement();
                        /////////////////////////////////////
                        result = true;
                    }
                }
                else
                    result = false;
            }
            catch (Exception e3)
            {
                result = false;
            }
            //// 启动一个线程来采集数据,当前面的初始化都成功时
            if (result && this.acqMode == enIDS3010AcqMode.单点采集)
            {
                this.cts = new CancellationTokenSource();
                Task.Run(() =>
                 {
                     while (true)
                     {
                         if (cts.IsCancellationRequested) break;
                         if (this.isSave && this.attocube.Displacement_GetMeasurementEnabled()) // 传感器在采集状态才开始存储数据
                         {
                             this.items = this.attocube.Displacement_GetAbsolutePositions();
                             this.dist_axis1.Add(this.items.Item2);
                             this.dist_axis2.Add(this.items.Item3);
                             this.dist_axis3.Add(this.items.Item4);
                         }
                     }
                 });
            }

            configParam.ConnectState = result;
            return result;
        }
        public Dictionary<enDataItem, object> ReadData()
        {
            Dictionary<enDataItem, object> listdata = new  Dictionary<enDataItem, object>();
            listdata.Add(enDataItem.Dist1,this.dist_axis1.ToArray());
            listdata.Add(enDataItem.Dist2, this.dist_axis2.ToArray());
            listdata.Add(enDataItem.Dist3, this.dist_axis3.ToArray());
            //////
            this.dist_axis1.Clear();
            this.dist_axis2.Clear();
            this.dist_axis3.Clear();
            ///////////////////
            return listdata;
        }
        public bool StartTrigger()
        {
            bool result = false;
            try
            {
                switch (this.acqMode)
                {
                    case enIDS3010AcqMode.单点采集:
                        this.isSave = true;
                        result = true;
                        break;
                    case enIDS3010AcqMode.流采集:
                        this.isSave = true;
                        this.stream = IDSStreamLibrary.OpenStream(this.ConfigParam.ConnectAddress, true, 10, 1 | 4);
                        result = true;
                        break;
                    default:
                        break;
                }
            }
            catch (AttocubeAPIException e)
            {
                result = false;
            }
            return result;
        }
        public bool StopTrigger()
        {
            bool result = false;
            try
            {
                switch (this.acqMode)
                {
                    case enIDS3010AcqMode.单点采集:
                        this.isSave = false;
                        result = true;
                        break;
                    case enIDS3010AcqMode.流采集:
                        this.isSave = false;
                        /////////////
                        byte[] dataBuffer = new byte[this.bufferLength * 1024];
                        int WordSize = this.attocube.Realtime_GetResolutionHsslHigh() - this.attocube.Realtime_GetResolutionHsslLow() + 1 + this.attocube.Realtime_GetPeriodHsslGap(); // 一个字的数据位
                        long[] channelX, channelY, channelZ;
                        //////////////////////////////////////////
                        int bytesInBuffer = IDSStreamLibrary.ReadStream(this.stream, dataBuffer, dataBuffer.Length);
                        int legnth = bytesInBuffer * 8 / WordSize;
                        channelX = new long[legnth];
                        channelY = new long[legnth];
                        channelZ = new long[legnth];
                        int decodedSamplesCount;
                        int decodedBytes = IDSStreamLibrary.DecodeStreamSingle(this.stream, dataBuffer, bytesInBuffer, ref channelX, ref channelY, ref channelZ, 10000, out decodedSamplesCount);
                        IDSStreamLibrary.CloseStream(this.stream);
                        ////////////
                        for (int i = 0; i < decodedSamplesCount; i++)
                        {
                            this.dist_axis1.Add(channelX[i]);
                            this.dist_axis2.Add(channelY[i]);
                            this.dist_axis3.Add(channelZ[i]);
                        }
                        result = true;
                        break;
                    default:
                        break;
                }
            }
            catch (AttocubeAPIException e)
            {
                result = false;
            }
            return result;
        }
        public bool Disconnect()
        {
            bool result = false;
            try
            {
                if (cts != null)
                    this.cts.Cancel();
                this.attocube.Disconnect();
                result = true;
            }
            catch
            {
                result = false;
            }
            return result;
        }
        public bool SetParam(object paramType, object value)
        {
            // 根据不同的参数选择不同的分支来设置 
            enSensorParamType type;
            if (paramType is enSensorParamType)
                type = (enSensorParamType)paramType;
            else
                Enum.TryParse(paramType.ToString(), out type);
            //////////////////////
            switch (type)
            {
                case enSensorParamType.Coom_每线点数:
                    this.LaserParam.DataWidth = (int)value;
                    break;
                case enSensorParamType.Coom_激光位姿: //this.measureRange
                    //this.LaserParam.LaserCalibrationParam = (HTuple)value;
                    break;
                default:
                    break;
            }
            return true;
        }

        public object GetParam(object paramType)
        {
            enSensorParamType type;
            if (paramType is enSensorParamType)
                type = (enSensorParamType)paramType;
            else
                Enum.TryParse(paramType.ToString(), out type);
            //////////////////////
            switch (type)
            {
                case enSensorParamType.Coom_每线点数: //this.measureRange
                    return this.LaserParam.DataWidth;

                case enSensorParamType.Coom_传感器名称: //this.measureRange
                    return this.ConfigParam.SensorName;

                case enSensorParamType.Coom_激光位姿: //this.measureRange
                    return this.LaserParam.LaserCalibrationParam;

                case enSensorParamType.Coom_激光校准参数:
                    return this.LaserParam.LaserCalibrationParam;

                case enSensorParamType.Coom_传感器类型: //this.measureRange
                    return this.ConfigParam.SensorType;

                default:
                    return null;
            }
        }

    }

    public enum enIDS3010AcqMode
    {
        流采集,
        单点采集,
    }

    public struct attocubeParamStruct  // 用来存储参数
    {

    }


}

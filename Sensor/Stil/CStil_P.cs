
using Common;
using STIL_NET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HalconDotNet;

namespace Sensor
{

    /// <summary>
    /// 定义一个适配器类，实现新接口并封装原对象，由于每家定义的接口都不一样，所以需要自己定义一个统一的接口
    /// </summary>
    public class CStil_P : SensorBase, ISensor
    {
        private FileOperate fo = new FileOperate();
        private StilPointSensorSetting stilPointParamConfig;
        private StilPointLaser stil_P = new StilPointLaser();  // 对StilPointLaser类的封装,对被适配的类进行封装
        private StilPointLaserAdaptive spla;
        public StilPointLaser Stil_P
        {
            get
            {
                return stil_P;
            }

            set
            {
                stil_P = value;
            }
        }
        public StilPointSensorSetting StilPointParamConfig { get => stilPointParamConfig; set => stilPointParamConfig = value; }

        public CStil_P()
        {
            spla = new StilPointLaserAdaptive(stil_P);
        }

        #region 实现接口
        public bool Init()
        {
            if (!stil_P.InitDLL()) return false;
            return true;
        }

        /// <summary>
        /// 连接传感器
        /// </summary>
        /// <returns></returns>
        public bool Connect(SensorConnectConfigParam configParam)
        {
            bool result = false;
            try
            {
                this.ConfigParam = configParam;
                this.Name = this.ConfigParam.SensorName;
                this.LaserParam = (LaserParam)new LaserParam().Read(this.ConfigParam.SensorName);
                this.LaserParam.DataHeight = 1;
                this.LaserParam.DataWidth = 1;
                this.stil_P.LaserParam = this.LaserParam;
                this.LaserParam.SensorName = configParam.SensorName;
                // 读取参数配置文件
                this.stil_P.StilPointParamConfig = this.stilPointParamConfig;
                /////////////        
                switch (configParam.ConnectType)
                {
                    case enUserConnectType.Network:
                        break;
                    default:
                    case enUserConnectType.USB:
                        result = stil_P.ConnectUSB(configParam.ConnectAddress);
                        break;
                    case enUserConnectType.SerialPort:
                        switch (configParam.DeviceDescribe)
                        {
                            case "CCS_PRIMA":
                            case "PRIMA":
                                result = stil_P.ConnectSerial(STIL_NET.enSensorType.CCS_PRIMA, Convert.ToUInt16(configParam.PortName.Split('M')[1]), (uint)configParam.BaudRate);
                                break;
                            case "CCS_OPTIMA_PLUS":
                            case "OPTIMA":
                                result = stil_P.ConnectSerial(STIL_NET.enSensorType.CCS_OPTIMA_PLUS, Convert.ToUInt16(configParam.PortName.Split('M')[1]), (uint)configParam.BaudRate);
                                break;
                            case "ZENITH":

                                break;
                        }
                        break;
                    case enUserConnectType.SerialNumber:
                        break;
                }
                // 如果连接这里很快就过了，有可能是DLL版本不能
                if (stil_P.Sensor == null)
                {
                    result = false;
                    return result;
                }
                this.LaserParam.MeasureRange = stil_P.Sensor.FullScale * 0.001;
                /////////////////////////////////////////////////////////////////////////
                switch (this.LaserParam.TriggerMode)
                {
                    case enUserTrigerMode.NONE: //       
                        if (stil_P.Sensor.MeasureMode == STIL_NET.enMeasureMode.DISTANCE_MODE)
                            result = stil_P.SetParameter(enUserTrigerMode.NONE, STIL_NET.enMeasureMode.DISTANCE_MODE, 100);
                        else
                            result = stil_P.SetParameter(enUserTrigerMode.NONE, STIL_NET.enMeasureMode.THICKNESS_MODE, 100);
                        break;
                    case enUserTrigerMode.TRS:
                        if (stil_P.Sensor.MeasureMode == STIL_NET.enMeasureMode.DISTANCE_MODE)
                            result = stil_P.SetParameter(enUserTrigerMode.TRS, STIL_NET.enMeasureMode.DISTANCE_MODE, 100);
                        else
                            result = stil_P.SetParameter(enUserTrigerMode.TRS, STIL_NET.enMeasureMode.THICKNESS_MODE, 100);
                        break;
                    case enUserTrigerMode.TRE:
                        if (stil_P.Sensor.MeasureMode == STIL_NET.enMeasureMode.DISTANCE_MODE)
                            result = stil_P.SetParameter(enUserTrigerMode.TRE, STIL_NET.enMeasureMode.DISTANCE_MODE, (uint)stilPointParamConfig.AcqCount);
                        else
                            result = stil_P.SetParameter(enUserTrigerMode.TRE, STIL_NET.enMeasureMode.THICKNESS_MODE, (uint)stilPointParamConfig.AcqCount);
                        break;
                    case enUserTrigerMode.TRN:
                        if (stil_P.Sensor.MeasureMode == STIL_NET.enMeasureMode.DISTANCE_MODE)
                            result = stil_P.SetParameter(enUserTrigerMode.TRN, STIL_NET.enMeasureMode.DISTANCE_MODE, 100);
                        else
                            result = stil_P.SetParameter(enUserTrigerMode.TRN, STIL_NET.enMeasureMode.THICKNESS_MODE, 100);
                        break;
                }
                result = stil_P.StartAcquisition();
                //return result;
            }
            catch (Exception ee)
            {
                LoggerHelper.Error(this.ConfigParam.SensorName + "打开失败", ee);
                result = false;
            }
            configParam.ConnectState = result;
            return result;
        }

        /// <summary>
        /// 断开传感器采集
        /// </summary>
        /// <returns></returns>
        public bool Disconnect()
        {
            bool result = false;
            if (stil_P.Sensor != null)
            {
                result = stil_P.StopAcquisition();
                result = stil_P.Disconnect();
                result = stil_P.ReleaseSensorDLL();
            }
            return result;
        }

        /// <summary>
        /// 触发传感器采集
        /// </summary>
        /// <returns></returns>
        public bool StartTrigger()
        {
            //return stil_P.StartTriggerTRS(stilPointParamConfig.TriggerSource);
            bool result = false;
            switch (this.ConfigParam.ConnectType)
            {
                case enUserConnectType.Map:
                    SensorManage.GetSensor(this._MapName).StartTrigger();
                    //if (this._grabImage != null && this._grabImage.IsInitialized())
                    //    this._grabImage.Dispose();
                    //this._grabImage = SensorManage.GetSensor(this._MapName).GetParam(enDataItem.Image) as HImage; // 用于远程调用的映射处理
                    break;
                default:
                    switch (this.CameraParam.TriggerSource)
                    {
                        case enUserTriggerSource.NONE: // 实时采集
                            this.stil_P.StartGrab();
                            switch (this.CameraParam.AcqMode)
                            {
                                case enAcqMode.同步采集:
                                    //this._camera.ClearData();
                                    //this._camera.StartGrab();
                                    //result = this.GetImageSyn(out this._grabImage);
                                    //this._camera.StopGrab();
                                    break;
                                case  enAcqMode.异步采集:
                                    //result = this.GetImageAsyn(out this._grabImage);
                                    break;
                            }
                            break;
                        case enUserTriggerSource.外部IO触发:
                        case enUserTriggerSource.内部IO触发:
                        case enUserTriggerSource.编码器触发:
                            this.stil_P.StopGrab();
                            switch (this.CameraParam.AcqMode)
                            {
                                case enAcqMode.同步采集:
                                    break;
                                case enAcqMode.异步采集:
                                    break;
                            }
                            break;
                        case enUserTriggerSource.软触发:
                            this.stil_P.StartGrab();
                            enSensorError sensorError = this.stil_P.Sensor.StartAcquisition();
                            if (sensorError == enSensorError.MCHR_ERROR_NONE)
                                result = true;
                            else
                                result = false;
                            break;
                    }
                    if (result)
                        LoggerHelper.Info(this.CameraParam.SensorName + "点云采集成功");
                    else
                        LoggerHelper.Info(this.CameraParam.SensorName + "点云采集失败");
                    break;
            }
            return result;
        }

        /// <summary>
        /// 停止采集
        /// </summary>
        /// <returns></returns>
        public bool StopTrigger()
        {
            // return stil_P.StopTriggerTRS(stilPointParamConfig.TriggerSource, this.stilPointParamConfig.AcqCount);
            bool result = false;
            switch (this.ConfigParam.ConnectType)
            {
                case enUserConnectType.Map:
                    SensorManage.GetSensor(this._MapName).StartTrigger();
                    //if (this._grabImage != null && this._grabImage.IsInitialized())
                    //    this._grabImage.Dispose();
                    //this._grabImage = SensorManage.GetSensor(this._MapName).GetParam(enDataItem.Image) as HImage; // 用于远程调用的映射处理
                    break;
                default:
                    switch (this.CameraParam.TriggerSource)
                    {
                        case enUserTriggerSource.NONE: // 实时采集
                            this.stil_P.StopGrab();
                            switch (this.CameraParam.AcqMode)
                            {
                                case enAcqMode.同步采集:
                                    break;
                                case enAcqMode.异步采集:
                                    break;
                            }
                            break;
                        case enUserTriggerSource.外部IO触发:
                        case enUserTriggerSource.内部IO触发:
                        case enUserTriggerSource.编码器触发:
                            this.stil_P.StopGrab();
                            switch (this.CameraParam.AcqMode)
                            {
                                case enAcqMode.同步采集:
                                    break;
                                case enAcqMode.异步采集:
                                    break;
                            }
                            break;
                        case enUserTriggerSource.软触发:
                            switch (this.CameraParam.TriggerMode)
                            {
                                case enUserTrigerMode.TRS: // TRS 在起点终点位置都需要触发一次
                                    this.stil_P.StopGrab();
                                    enSensorError sensorError = this.stil_P.Sensor.StartAcquisition();
                                    if (sensorError == enSensorError.MCHR_ERROR_NONE)
                                        result = true;
                                    else
                                        result = false;
                                    break;
                                default:
                                    this.stil_P.StartGrab();
                                    break;
                            }
                            break;
                    }
                    if (result)
                        LoggerHelper.Info(this.CameraParam.SensorName + "点云采集成功");
                    else
                        LoggerHelper.Info(this.CameraParam.SensorName + "点云采集失败");
                    break;
            }
            return result;
        }

        /// <summary>
        ///  获取测量数据  ：0:dist;/ 1:dist2; /2:thick;/ 3:intensity;/ 4:x;/ 5:y; /6:z;/ 7:baryCenter
        /// </summary>
        /// <returns></returns>
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
            double[] baryCenter;
            stil_P.GetData(out dist, out dist2, out thick, out intensity, out x, out y, out z, out baryCenter);
            listdata.Add(enDataItem.Dist1, dist);
            listdata.Add(enDataItem.Dist2, dist2);
            listdata.Add(enDataItem.Thick, thick);
            listdata.Add(enDataItem.Intensity, intensity);
            listdata.Add(enDataItem.X, x);
            listdata.Add(enDataItem.Y, y);
            listdata.Add(enDataItem.Z, z);
            //////////////////////////////////
            //if (dist != null && dist.Length > 0)
            //    listdata.Add(enDataItem.Dist1Modle, new HObjectModel3D(x, y, dist));
            //if (dist2 != null && dist2.Length > 0)
            //    listdata.Add(enDataItem.Dist2Modle, new HObjectModel3D(x, y, dist2));
            //if (thick != null && thick.Length > 0)
            //    listdata.Add(enDataItem.ThickModle, new HObjectModel3D(x, y, thick));

            return listdata;
        }

        /// <summary>
        /// 设置传感器参数
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
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
                case enSensorParamType.Stil_设置光学笔:
                    //this.stil_P.Sensor.OpticalPen = (int)value;
                    spla.SetOpticalPen((int)value);
                    break;
                case enSensorParamType.Stil_设置手动光源模式:
                    spla.SetManualLightMode();
                    break;
                case enSensorParamType.Stil_设置最强峰值:
                    spla.SetStrongPeak();
                    break;
                case enSensorParamType.Stil_设置第一峰值:
                    spla.SetFirstPeak();
                    break;
                case enSensorParamType.Stil_设置距离模式:
                    spla.SetDistMeasureMode();
                    break;
                case enSensorParamType.Stil_设置厚度模式:
                    spla.SetThickMeasureMode();
                    break;
                case enSensorParamType.Stil_设置测量阈值:
                    spla.SetThreahoudl((double)value);
                    break;
                case enSensorParamType.Stil_设置自动光源模式:
                    spla.SetAutoLightMode();
                    break;
                case enSensorParamType.Stil_设置预置频率:
                    spla.SetPresetRate((int)value);
                    break;
                case enSensorParamType.Stil_保存参数:
                    spla.SaveParam();
                    break;
                case enSensorParamType.Stil_设置自动光源模式下的亮度:
                    spla.SetManualModeLightValue((int)value);
                    break;
                case enSensorParamType.Stil_设置手动光源模式下的亮度:
                    spla.SetAutoModeLightValue((int)value);
                    break;
                case enSensorParamType.Stil_暗校正:
                    spla.Dark();
                    break;
                case enSensorParamType.Coom_每线点数:
                    this.LaserParam.DataWidth = (int)value;
                    break;
                case enSensorParamType.Coom_激光位姿: //this.measureRange
                    //this.LaserParam.LaserPose = (HTuple)value;
                    break;



                default:
                    break;
            }
            return true;
        }

        public object GetParam(object paramType)
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
                case enSensorParamType.Stil_获取光学笔列表:
                    return spla.GetOpticalPenList();
                ////////////////
                case enSensorParamType.Stil_获取当前光学笔:
                    return spla.GetCurrentOpticalPen();
                //////////
                case enSensorParamType.Stil_获取当前光源模式:
                    return spla.GetLightMode();
                ///////////
                case enSensorParamType.Stil_获取当前测量峰值:
                    return spla.GetCurrentMeasurePeak();
                //////////
                case enSensorParamType.Stil_获取当前测量模式:
                    return spla.GetCurrentMeasureMode();

                case enSensorParamType.Stil_获取当前测量阈值:
                    return spla.GetCurrentThreshoud();

                case enSensorParamType.Stil_获取当前频率:
                    return spla.GetCurrentRate();

                case enSensorParamType.Stil_获取最小暗黑频率:
                    return spla.GetMinDarkRate();

                case enSensorParamType.Stil_获取频率列表:
                    return spla.GetRateList();

                case enSensorParamType.Stil_获取光源模式:
                    return spla.GetLightMode();

                case enSensorParamType.Stil_获取手动光源模式下的亮度:
                    return spla.GetManualModeLightValue();

                case enSensorParamType.Stil_获取自动光源模式下的亮度:
                    return spla.GetAutoModeLightValue();
                /////////////
                case enSensorParamType.this对象:
                    return this;
                /////////////////
                case enSensorParamType.Coom_每线点数: //this.measureRange
                    return this.LaserParam.DataWidth;

                case enSensorParamType.Stil_测量范围: //this.measureRange
                    return this.LaserParam.MeasureRange;

                case enSensorParamType.Coom_当前采集数量: //this.measureRange
                    return stil_P.AcqNumber;

                case enSensorParamType.Coom_传感器名称: //this.measureRange
                    return this.ConfigParam.SensorName;

                case enSensorParamType.Coom_激光位姿: //this.measureRange
                                                  //return this.laserPose;

                case enSensorParamType.Coom_激光校准参数:
                    return this.LaserParam.LaserCalibrationParam;

                case enSensorParamType.Coom_传感器类型: //this.measureRange
                    return this.ConfigParam.SensorType;


                default:
                    return null;
            }
            //return 1;
        }

        #endregion


    }


}

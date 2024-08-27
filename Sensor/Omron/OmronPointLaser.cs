
using Common;
using STIL_NET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HalconDotNet;
using Omron.Cxap.Modules.DisplacementSensorSDK;
using Omron.Cxap.Modules.DisplacementSensorSDK.CommHelper;

namespace Sensor
{

    /// <summary>
    /// 定义一个适配器类，实现新接口并封装原对象，由于每家定义的接口都不一样，所以需要自己定义一个统一的接口
    /// </summary>
    public class OmronPointLaser : SensorBase, ISensor
    {
        private DSComm dsCom = null;
        private DisConnectDelegate myDisConnectDelegate = null;


        public OmronPointLaser()
        {

        }

        #region 实现接口
        public bool Init()
        {
            this.dsCom = new DSComm(DSComm.Version.ZW2);
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
                this.LaserParam.SensorName = configParam.SensorName;
                // 读取参数配置文件
                /////////////        
                switch (configParam.ConnectType)
                {
                    case enUserConnectType.Network:
                        string[] strIpAdress = configParam.ConnectAddress.Split('.');
                        byte[] ip = new byte[strIpAdress.Length];
                        for (int i = 0; i < strIpAdress.Length; i++)
                        {
                            ip[i] = byte.Parse(strIpAdress[i]);
                        }
                        int ret = this.dsCom.Open(ip, myDisConnectDelegate);
                        if (ret == CommErr.OK)
                            result = true;
                        break;
                    case enUserConnectType.USB:
                        ;
                        break;
                    case enUserConnectType.SerialPort:
                        break;
                    case enUserConnectType.SerialNumber:
                        break;
                    default:
                        MessageBox.Show(configParam.ConnectType.ToString() + "该连接类型未实现!!");
                        break;
                }
                /////////////////////////////////////////////////////////////////////////
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
            if (this.dsCom != null)
            {
                this.dsCom.Close();
                result = true;
            }
            return result;
        }

        /// <summary>
        /// 触发传感器采集
        /// </summary>
        /// <returns></returns>
        public bool StartTrigger()
        {
            bool result = false;
            switch (this.ConfigParam.ConnectType)
            {
                case enUserConnectType.Map:
                    SensorManage.GetSensor(this._MapName).StartTrigger();
                    break;
                default:
                    switch (this.LaserParam.TriggerSource)
                    {
                        case enUserTriggerSource.NONE: // 实时采集
                            if (this.dsCom != null)
                            {
                                this.dsCom.SetSystemData(1120, 0); // 0：内部同步测量；1：外部同步测量
                                this.dsCom.StartStorage(1, this.LaserParam.DataHeight * this.LaserParam.DataWidth);
                            }
                            switch (this.LaserParam.AcqMode)
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
                            if (this.dsCom != null)
                            {
                                this.dsCom.SetSystemData(1120, 1); // 0：内部同步测量；1：外部同步测量
                                this.dsCom.StartStorage(0, this.LaserParam.DataHeight * this.LaserParam.DataWidth);
                            }
                            switch (this.LaserParam.AcqMode)
                            {
                                case enAcqMode.同步采集:
                                    break;
                                case enAcqMode.异步采集:
                                    break;
                            }
                            break;
                        case enUserTriggerSource.软触发:


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
            bool result = false;
            switch (this.ConfigParam.ConnectType)
            {
                case enUserConnectType.Map:
                    SensorManage.GetSensor(this._MapName).StopTrigger();
                    break;
                default:
                    switch (this.CameraParam.TriggerSource)
                    {
                        case enUserTriggerSource.NONE: // 实时采集
                            if(this.dsCom != null)
                            {
                                this.dsCom.StopStorage();
                            }    
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
                            if (this.dsCom != null)
                            {
                                this.dsCom.StopStorage();
                            }
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

                                    break;
                                default:

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
            int[] data = new int[0];
            if (this.dsCom != null)
            {
                this.dsCom.GetStorageData(DSComm.Out.O1, out data);
            }
            for (int i = 0; i < data.Length; i++)
            {
                listdata.Add(enDataItem.Dist1, data[i]);
                listdata.Add(enDataItem.Dist2, data[i]);
                listdata.Add(enDataItem.Thick, data[i]);
                listdata.Add(enDataItem.Intensity, data[i]);
                listdata.Add(enDataItem.X, 0);
                listdata.Add(enDataItem.Y, 0);
                listdata.Add(enDataItem.Z, 0);
            }
            /////////////////////////////////
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
                    //spla.SetOpticalPen((int)value);
                    break;
                case enSensorParamType.Stil_设置手动光源模式:
                    //spla.SetManualLightMode();
                    break;
                case enSensorParamType.Stil_设置最强峰值:
                    //spla.SetStrongPeak();
                    break;
                case enSensorParamType.Stil_设置第一峰值:
                    //spla.SetFirstPeak();
                    break;
                case enSensorParamType.Stil_设置距离模式:
                    //spla.SetDistMeasureMode();
                    break;
                case enSensorParamType.Stil_设置厚度模式:
                    //spla.SetThickMeasureMode();
                    break;
                case enSensorParamType.Stil_设置测量阈值:
                    //spla.SetThreahoudl((double)value);
                    break;
                case enSensorParamType.Stil_设置自动光源模式:
                    //spla.SetAutoLightMode();
                    break;
                case enSensorParamType.Stil_设置预置频率:
                    //spla.SetPresetRate((int)value);
                    break;
                case enSensorParamType.Stil_保存参数:
                    //spla.SaveParam();
                    break;
                case enSensorParamType.Stil_设置自动光源模式下的亮度:
                    //spla.SetManualModeLightValue((int)value);
                    break;
                case enSensorParamType.Stil_设置手动光源模式下的亮度:
                    //spla.SetAutoModeLightValue((int)value);
                    break;
                case enSensorParamType.Stil_暗校正:
                    //spla.Dark();
                    break;
                case enSensorParamType.Coom_每线点数:
                    //this.LaserParam.DataWidth = (int)value;
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
            return 1;
        }

        #endregion


    }


}

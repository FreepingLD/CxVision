using Common;
using Sensor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Sensor
{
    public class LiYiPointLaser : SensorBase, ISensor
    {
        private LiYiAdapter liyi = new LiYiAdapter();
        private FileOperate fo = new FileOperate();
        private LiyiPointSensorSetting liyiPointParamConfig;


        public LiYiPointLaser()
        {

        }


        public bool Connect(SensorConnectConfigParam configParam)
        {
            bool result = false;
            try
            {
                this.ConfigParam = configParam;
                this.Name = configParam.SensorName;
                this.LaserParam = (LaserParam)new LaserParam().Read(this.Name);
                this.LaserParam.SensorName = configParam.SensorName;
                ///////////////
                switch (configParam.ConnectType)
                {
                    case enUserConnectType.Network:
                        return result;

                    case enUserConnectType.USB:
                        if (liyi.Connect(configParam.SensorName, ""))
                        {
                            switch (liyiPointParamConfig.TrigMode)
                            {
                                case enUserTrigerMode.NONE:
                                    if (liyiPointParamConfig.MeasureMode == enUserMeasureMode.Distance)
                                        result = liyi.SetParameter(enUserTrigerMode.NONE, enUserMeasureMode.Distance, 20000);
                                    else
                                        result = liyi.SetParameter(enUserTrigerMode.NONE, enUserMeasureMode.Thickness, 20000);
                                    break;
                                case enUserTrigerMode.TRE: // 单点触发采集
                                    if (liyiPointParamConfig.MeasureMode == enUserMeasureMode.Distance)
                                        result = liyi.SetParameter(enUserTrigerMode.TRE, enUserMeasureMode.Distance, (int)GlobalVariable.pConfig.TreNum);
                                    else
                                        result = liyi.SetParameter(enUserTrigerMode.TRE, enUserMeasureMode.Thickness, (int)GlobalVariable.pConfig.TreNum);
                                    break;
                                case enUserTrigerMode.TRN: // 高电平采集
                                    if (liyiPointParamConfig.MeasureMode == enUserMeasureMode.Distance)
                                        result = liyi.SetParameter(enUserTrigerMode.TRN, enUserMeasureMode.Distance, 20000);
                                    else
                                        result = liyi.SetParameter(enUserTrigerMode.TRN, enUserMeasureMode.Thickness, 20000);
                                    break;
                            }
                        }
                        return result;
                }
            }
            catch
            {
                LoggerHelper.Error(this.LaserParam.SensorName + "连接出错");
            }
            configParam.ConnectState = result;
            return result;
        }

        public bool Disconnect()
        {
            if (liyi.DisConnect()) return true;//关闭设备
            return false;
        }


        public bool Init()
        {
            this.LaserParam.DataWidth = 1;
            this.LaserParam.DataHeight = 1;
            if (liyi.InitDLL()) return true;
            else return false;
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
            liyi.GetData(out dist, out dist2, out thick, out intensity, out x, out y, out z);
            listdata.Add(enDataItem.Dist1,dist);
            listdata.Add(enDataItem.Dist2, dist2);
            listdata.Add(enDataItem.Thick, thick);
            listdata.Add(enDataItem.Intensity, intensity);
            listdata.Add(enDataItem.X, x);
            listdata.Add(enDataItem.Y, y);
            listdata.Add(enDataItem.Z, z);
            ///////////////
            return listdata;
        }

        public object GetParam(object paramType)
        {
            enSensorParamType name = (enSensorParamType)paramType;
            switch (name)
            {
                case enSensorParamType.Liyi_厚度模式下峰值1:
                    break;
                case enSensorParamType.Liyi_厚度模式下峰值2:
                    break;
                case enSensorParamType.Liyi_曝光:
                    return liyi.Exposure;
                case enSensorParamType.Liyi_检测阈值:
                    break;
                case enSensorParamType.Liyi_测量模式:
                    break;
                case enSensorParamType.Liyi_测量范围:
                    return liyi.FullRange;
                case enSensorParamType.Liyi_触发模式:
                    return liyi.TriggerMode;
                case enSensorParamType.Liyi_触发源:
                    return liyi.TriggerSource;
                case enSensorParamType.Liyi_采集频率:
                    return liyi.Frequency;
                case enSensorParamType.Liyi_高度模式的峰值选择:
                    return liyi.PeakMode;
                case enSensorParamType.Liyi_增益:
                    return liyi.Gain;
                case enSensorParamType.Liyi_标准厚度:
                    return string.Join(";", liyi.StandardThickValue);
                case enSensorParamType.Liyi_暗黑校正:
                    return liyi.Dark;
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
        public bool SetParam(object paramType, object value)
        {
            enSensorParamType name = (enSensorParamType)paramType;
            switch (name)
            {
                case enSensorParamType.Liyi_保存配置:
                    this.liyi.SaveParam();
                    break;
                case enSensorParamType.Liyi_厚度模式下峰值1:
                    break;
                case enSensorParamType.Liyi_厚度模式下峰值2:
                    break;
                case enSensorParamType.Liyi_曝光:
                    liyi.Exposure = Convert.ToSingle(value);
                    break;
                case enSensorParamType.Liyi_检测阈值:
                    break;
                case enSensorParamType.Liyi_测量模式:
                    break;
                case enSensorParamType.Liyi_触发模式:
                    liyi.TriggerMode = (enUserTrigerMode)value;
                    break;
                case enSensorParamType.Liyi_触发源:
                    liyi.TriggerSource = (enUserTriggerSource)value;
                    break;
                case enSensorParamType.Liyi_采集频率:
                    liyi.Frequency = (int)value;
                    break;
                case enSensorParamType.Liyi_高度模式的峰值选择:
                    liyi.PeakMode = (enPeakType)value;
                    break;
                case enSensorParamType.Liyi_增益:
                    liyi.Gain = (float)value;
                    break;
                case enSensorParamType.Liyi_标准厚度:
                    string[] array = value.ToString().Split(';');
                    List<float> list = new List<float>();
                    for (int i = 0; i < array.Length; i++)
                    {
                        list.Add(float.Parse(array[i]));
                    }
                    liyi.StandardThickValue = list.ToArray();
                    break;
                case enSensorParamType.Coom_每线点数:
                    this.LaserParam.DataWidth = (int)value;
                    break;
                default:
                    break;
            }
            return true;
        }

        public bool StartTrigger()
        {
            if (liyi.StartAcquisition()) return true;
            else return false;
        }

        public bool StopTrigger()
        {
            if (liyi.StopAcquisition()) return true;
            else return false;
        }


    }

    public enum enPeakType
    {
        最强峰,
        最近峰,
        最远峰,
        第一峰,
        第二峰,
        第三峰,
        第四峰,
        第五峰,
        第六峰,
        第七峰,
        第八峰,
        第九峰,
        第十峰,
        NONE,
    }
}

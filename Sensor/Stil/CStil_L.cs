using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HalconDotNet;
using System.Windows.Forms;


namespace Sensor
{
    /// <summary>
    /// StilLineLaser:封装一个对象，用于来实现接口
    /// </summary>
    public class CStil_L :SensorBase, ISensor
    {
        private string configPath;
        private string ipAdress;
        private FileOperate fo = new FileOperate();
        private StilLineSensorSetting stilLineConfigParam;
        private StilLineLaser stil_L = new StilLineLaser();

        public StilLineLaser Stil_L
        {
            get
            {
                return stil_L;
            }

            set
            {
                stil_L = value;
            }
        }
        public StilLineSensorSetting StilLineConfigParam { get => stilLineConfigParam; set => stilLineConfigParam = value; }

        public CStil_L()
        {

        }
        public bool Init()
        {
           
            this.LaserParam.DataHeight = 1;
            this.LaserParam.DataWidth = 1;
            this.stil_L.StilLineConfigParam = (StilLineSensorSetting)this.LaserParam;
            this.stil_L.LaserParam = this.LaserParam;
            return true;
        }

        // 连接线激光时要提供多个文件路径
        public bool Connect(SensorConnectConfigParam configParam)
        {
            //////////////////////
            bool result = false; // 
            ///////////
            this.ConfigParam = configParam;
            this.Name = this.ConfigParam.SensorName;
            this.LaserParam = (LaserParam)new LaserParam().Read(this.Name);
            this.LaserParam.SensorName = configParam.SensorName;
            ////////////
            result = stil_L.Connect(configParam.ConnectAddress);
            if (stil_L.DeviceID != null)
            {
                result = true;
                stil_L.LoadConfig(this.configPath);
                stil_L.LoadDark(this.configPath);
                InitParam();
                stil_L.StartAcquisition();
                this.LaserParam.MeasureRange = stil_L.GetFullScale() * 0.001;
            }          
            GlobalVariable.pConfig.Stil_MeasureRange =Convert.ToSingle(this.LaserParam.MeasureRange);

            configParam.ConnectState = result;
            return result;
        }
        public bool Disconnect()
        {
            if (stil_L.DeviceID != null)
            {
                stil_L.StopAcquisition();
                stil_L.DisConnect();
            }
            return true;
        }
        public bool StartTrigger()
        {
            if (stil_L.StartTrigSensorTRS(stilLineConfigParam.TriggerSource)) return true;
            return false;
        }
        public bool StopTrigger()
        {
            if (stil_L.StopTrigSensorTRS(stilLineConfigParam.TriggerSource, stilLineConfigParam.WaiteAcqCount)) return true;
            return false;
        }
        public Dictionary<enDataItem, object>  ReadData()
        {
            Dictionary<enDataItem, object> listdata = new Dictionary<enDataItem, object>();
            double[] dist;
            double[] dist2;
            double[] thick;
            double[] intensity;
            double[] intensity2;
            double[] x;
            double[] y;
            double[] z;
            stil_L.GetData(out dist, out dist2, out thick, out intensity, out x, out y, out z, out intensity2);
            listdata.Add(enDataItem.Dist1,dist);
            listdata.Add(enDataItem.Dist2, dist2);
            listdata.Add(enDataItem.Thick, thick);
            listdata.Add(enDataItem.Intensity, intensity);
            listdata.Add(enDataItem.X, x);
            listdata.Add(enDataItem.Y, y);
            listdata.Add(enDataItem.Z, z);


            return listdata;
        }
        public bool SetParam(object paramType, object value)
        {
            enUserPeakMode peakMode;
            enSensorParamType type;
            if (paramType is enSensorParamType)
                type = (enSensorParamType)paramType;
            else
                Enum.TryParse(paramType.ToString(), out type);
            ///////////////////////////////////
            switch (type)
            {
                case enSensorParamType.Stil_光源亮度:
                    stil_L.SetLEDLightValue((int)value);
                    break;
                case enSensorParamType.Stil_保存配置:
                    stil_L.StopAcquisition();
                    stil_L.SaveConfig(this.configPath);
                    stil_L.StartAcquisition();
                    break;
                case enSensorParamType.Stil_光强校正:
                    stil_L.IntensityNormalization(this.configPath);
                    break;
                case enSensorParamType.Stil_厚度模式下峰值1:
                    peakMode = (enUserPeakMode)value ;
                    stil_L.SetFirstPeakTypeInThicknessMode(peakMode);
                    break;
                case enSensorParamType.Stil_厚度模式下峰值2:
                    peakMode = (enUserPeakMode)value;
                    stil_L.SetSecondPeakTypeInThicknessMode(peakMode);
                    break;
                case enSensorParamType.Stil_暗黑校正:
                    stil_L.AquisitionDark(value.ToString());
                    break;
                case enSensorParamType.Stil_曝光:
                    stil_L.SetExposeTime((int)value);
                    break;
                case enSensorParamType.Stil_检测阈值:
                    stil_L.SetDetectionThreshold((double)value);
                    break;
                case enSensorParamType.Stil_测量模式:
                    string Measuregmode = value.ToString();
                    stil_L.StopAcquisition();
                    stil_L.SetMeasureMode(Measuregmode);
                    stil_L.StartAcquisition();
                    break;
                case enSensorParamType.Stil_热校正:
                    stil_L.ThermalCorrection(value.ToString());
                    break;
                case enSensorParamType.Stil_高度模式的峰值选择:
                    peakMode = (enUserPeakMode)value;
                    stil_L.SetPeakTypeInAltitudeMode(peakMode);
                    break;
                case enSensorParamType.Stil_触发模式:
                    string[] param = value.ToString().Split(';'); // 触发模式需要传入两个参数
                    enUserTrigerMode TrigerMode;
                    stil_L.StopAcquisition();
                    Enum.TryParse(param[0],out TrigerMode);
                    stil_L.SetSoftwareTriggerMode(TrigerMode, int.Parse(param[1]));
                    stil_L.StartAcquisition();
                    break;
                case enSensorParamType.Coom_每线点数:
                    this.LaserParam.DataWidth = (int)value;                  
                    break;
                case enSensorParamType.Stil_置零编码器:
                    stil_L.StopAcquisition();
                    stil_L.ResetEncoder();
                    stil_L.StartAcquisition();
                    break;

                case enSensorParamType.Coom_激光位姿:
                    //this.LaserParam.LaserCalibrationParam=(HTuple)value;
                    break;

                //case enSensorParamType.Coom_激光校准参数: //this.measureRange
                //    this.sensorCalibrationParam=(double [])value;
                //    break;

                default:
                    break;
            }
            return true;
        }
        public object GetParam(object paramType)
        {
            int value;
            // 根据不同的参数选择不同的分支来设置 
            enSensorParamType type;
            if (paramType is enSensorParamType)
                type = (enSensorParamType)paramType;
            else
                Enum.TryParse(paramType.ToString(), out type);
            //////////////////////
            switch (type)
            {
                case enSensorParamType.Stil_测量范围:
                    return this.LaserParam.MeasureRange;

                case enSensorParamType.Stil_待处理的峰值数量:
                    return stil_L.GetPeakNumber();

                case enSensorParamType.Stil_光源亮度:
                    return stil_L.GetLEDLightValue();

                case enSensorParamType.Stil_检测阈值:
                    return stil_L.GetDetectionThreshold();

                case enSensorParamType.Stil_曝光:
                    return stil_L.GetExposeTime();

                case enSensorParamType.Stil_厚度模式下峰值1:
                    return stil_L.GetFirstPeakTypeInThicknessMode();

                case enSensorParamType.Stil_厚度模式下峰值2:
                    return stil_L.GetSecondPeakTypeInThicknessMode();

                case enSensorParamType.Stil_测量模式:
                    return stil_L.GetMeasureMode();

                case enSensorParamType.Stil_触发模式:
                    return stil_L.GetTriggerMode();

                case enSensorParamType.Stil_高度模式的峰值选择:
                    return stil_L.GetPeakTypeInAltitudeMode();

                case enSensorParamType.Coom_每线点数:
                    return this.LaserParam.DataWidth;

                case enSensorParamType.Coom_当前采集数量:
                    return stil_L.AcqNumber; ;

                case enSensorParamType.Coom_传感器名称:
                    return this.ConfigParam.SensorName;

                case enSensorParamType.Coom_激光位姿:
                    //return this.LaserParam.LaserPose;

                case enSensorParamType.Coom_激光校准参数: //this.measureRange
                    return this.LaserParam.LaserCalibrationParam ;

                case enSensorParamType.Coom_传感器类型: //this.measureRange
                    return this.ConfigParam.SensorType;
                default:
                    return null;
            }
           // return true;
        }
        private bool InitParam() // 初始化参应该将所有的参数都重设置一次
        {
            stil_L.SetExposeTime(stilLineConfigParam.Stil_ExposureTime);
            stil_L.SetLEDLightValue(stilLineConfigParam.Stil_LedBrightness);
            stil_L.SetSoftwareTriggerMode(stilLineConfigParam.TrigMode, (int)stilLineConfigParam.TreNum);
            stil_L.SetDetectionThreshold(stilLineConfigParam.Stil_DetectionThreshold);
            if (stilLineConfigParam.MeasureMode == enUserMeasureMode.Distance)
            {
                stil_L.SetMeasureMode("Distance");
                stil_L.SetPeakTypeInAltitudeMode(stilLineConfigParam.Stil_AltitudeModePeak);
            }
            else
            {
                stil_L.SetMeasureMode("Thickness");
                stil_L.SetFirstPeakTypeInThicknessMode(stilLineConfigParam.Stil_ThickModePeak1);
                stil_L.SetSecondPeakTypeInThicknessMode(stilLineConfigParam.Stil_ThickModePeak2);
            }
            return true;
        }

    }
}

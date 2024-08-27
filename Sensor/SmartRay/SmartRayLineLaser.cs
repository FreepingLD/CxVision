using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Smartray;
using Smartray.Sample;
using System.Windows.Forms;
using HalconDotNet;
using Common;
using static Smartray.Api;

namespace Sensor
{
    public class SmartRayLineLaser : SensorBase, ISensor
    {
        private SmartRaySensor smartSensor;
        private static SmartRaySensorManager smartSensorManager = new SmartRaySensorManager();
        private uint scanLineCount; // 采集的轮廓数量
        private Api.ImageAcquisitionType imageAcqType = Api.ImageAcquisitionType.PointCloud;

        public SmartRaySensor SmartSensor
        {
            get
            {
                return smartSensor;
            }

            set
            {
                smartSensor = value;
            }
        }



        public SmartRayLineLaser()
        {

        }
        public  bool Connect(SensorConnectConfigParam configParam)
        {
           // return false;
            bool result = false; // 
            this.ConfigParam = configParam;
            this.Name = configParam.SensorName;
            this.LaserParam = (LaserParam)new LaserParam().Read(this.Name);
            this.LaserParam.SensorName = configParam.SensorName;
            switch (configParam.ConnectType)
            {
                case enUserConnectType.Network:
                    //int.TryParse(paramName[2], out lineCount);
                    //this.SensorParam.LaserParam.LineCount = lineCount;
                    smartSensor = smartSensorManager.CreateSensor(configParam.SensorName, 0, configParam.ConnectAddress, 40);
                    smartSensor.Connect();
                    smartSensor.LoadCalibrationDataFromSensor(); // 这个只需要加载一次就可以了
                    break;
            }
            ////////////////
            ////////////////////////////
            switch (imageAcqType) //smartSensor.GetImageAquisitionType()
            {
                case Api.ImageAcquisitionType.LiveImage:
                    smartSensor.LoadParameterSetFromFile(SmartRaySensor.ParameterSet.LiveImage);
                    smartSensor.SendParameterSet();
                    result = true;
                    break;

                case Api.ImageAcquisitionType.ProfileIntensityLaserLineThickness:
                    smartSensor.LoadParameterSetFromFile(SmartRaySensor.ParameterSet.Snapshot3d);
                    scanLineCount = smartSensor.GetNumberOfProfilesToCapture();
                    smartSensor.ConfigureImageAquisition(Api.ImageAcquisitionType.ProfileIntensityLaserLineThickness, scanLineCount);
                    smartSensor.SendParameterSet();
                    result = true;
                    break;

                case Api.ImageAcquisitionType.PointCloud:
                    int triggerDivider;
                    int triggerDelay;
                    TriggerEdgeMode triggerDirection;
                    smartSensor.LoadParameterSetFromFile(SmartRaySensor.ParameterSet.Snapshot3d);
                    // refine image acquiring parameters
                    scanLineCount = smartSensor.GetNumberOfProfilesToCapture();
                    smartSensor.ConfigureImageAquisition(Api.ImageAcquisitionType.PointCloud, scanLineCount);
                    // smartSensor.SetMetaDataExportEnable(true);
                    Api.GetDataTriggerExternalTriggerParameters(smartSensor._sensorObject, out triggerDivider, out triggerDelay, out triggerDirection);
                    if (triggerDirection == Api.TriggerEdgeMode.RisingEdge)
                        Api.SetTransportResolution(smartSensor._sensorObject, (triggerDivider + triggerDelay) * 0.0002f * 4); // 这里要乘以4倍，因为是4倍频
                    else
                        Api.SetTransportResolution(smartSensor._sensorObject, (triggerDivider + triggerDelay) * -0.0002f * 4);
                    smartSensor.SendParameterSet();
                    result = true;
                    break;

                case Api.ImageAcquisitionType.ZMapIntensityLaserLineThickness:
                    smartSensor.LoadParameterSetFromFile(SmartRaySensor.ParameterSet.Snapshot3d);
                    // refine image aquiring parameters
                    scanLineCount = smartSensor.GetNumberOfProfilesToCapture();
                    smartSensor.ConfigureImageAquisition(Api.ImageAcquisitionType.ZMapIntensityLaserLineThickness, scanLineCount);
                    // smartSensor.SetMetaDataExportEnable(true);
                    smartSensor.SetTransportResolution(0.1f);
                    smartSensor.SetZmapResolution(0.056f, 0.0085f);
                    smartSensor.SendParameterSet();
                    result = true;
                    break;

                default:
                    result = true;
                    break;
            }
            configParam.ConnectState = result;
            return result;
        }

        public  bool Disconnect()
        {
            smartSensor.StopAcquisition();
            smartSensor.Disconnect();
            return true;
        }

        public  object GetParam(object paramType)
        {
            enSensorParamType content;
            if (paramType is enSensorParamType)
                content = (enSensorParamType)paramType;
            else
                Enum.TryParse(paramType.ToString(), out content);
            switch (content)
            {
                case enSensorParamType.smartRay_曝光时间:
                    return smartSensor.GetExposureParameters();

                case enSensorParamType.smartRay_增益:
                    return smartSensor.GetGainParameters();

                case enSensorParamType.smartRay_触发参数:
                    return smartSensor.GetDataTriggerParameters();

                case enSensorParamType.smartRay_触发模式:
                    return smartSensor.GetTriggerMode();

                case enSensorParamType.smartRay_图像采集类型:
                    return smartSensor.GetImageAquisitionType();

                case enSensorParamType.smartRay_激光模式:
                    return smartSensor.GetLaserMode();

                case enSensorParamType.smartRay_激光亮度:
                    return smartSensor.GetLaserBrightnessPercent();

                case enSensorParamType.smartRay_兴趣区域:
                    return smartSensor.GetRegionOfInterest();

                case enSensorParamType.smartRay_扫描轮廓数量:
                    return smartSensor.GetNumberOfProfilesToCapture();

                case enSensorParamType.smartRay_激光线阈值:
                    return smartSensor.GetlaserLineThreshold();

                case enSensorParamType.smartRay_曝光模式:
                    return smartSensor.GetExposureMode();

                case enSensorParamType.smartRay_多重曝光合并模式:
                    return smartSensor.GetMultiExposureMode();

                case enSensorParamType.Coom_每线点数:
                    return this.LaserParam.DataWidth;

                case enSensorParamType.smartRay_内部触发频率:
                    return smartSensor.GetDataTriggerInternalToFrequencyHz();

                case enSensorParamType.Coom_传感器名称:
                    return this.LaserParam.SensorName;
                case enSensorParamType.Coom_传感器类型:
                    return SensorConnectConfigParamManger.Instance.GetSensorConfigParam(this.Name).SensorType;

                case enSensorParamType.Coom_激光位姿: //this.measureRange
                    //return this.laserPose;

                case enSensorParamType.Coom_激光校准参数: //this.measureRange
                    return this.LaserParam.LaserCalibrationParam;

                default:
                    return null;
            }
        }

        public  bool Init()
        {
            this.LaserParam.DataWidth = 1920;
            this.LaserParam.DataHeight = 1;
            return true; ;
        }

        public Dictionary<enDataItem, object> ReadData()
        {
            return smartSensor.getData();
        }

        public  bool SetParam(object paramType, object value)
        {
            enSensorParamType content;
            if (paramType is enSensorParamType)
                content = (enSensorParamType)paramType;
            else
                Enum.TryParse(paramType.ToString(), out content);
            switch (content)
            {
                case enSensorParamType.smartRay_图像采集类型:
                    smartSensor.StopAcquisition();
                    setImageAcqType((Api.ImageAcquisitionType)(value));
                    return true;
                case enSensorParamType.smartRay_激光亮度:   //不需要停止采集
                    smartSensor.ConfigureLaserBrightnessPercent(Convert.ToInt32(value));
                    return true;
                case enSensorParamType.smartRay_曝光时间:   //不需要停止采集
                    if (smartSensor.GetExposureMode() > 1)
                        smartSensor.ConfigureDoubleExposureTimesMicroS((Api.MultipleExposureMergeModeType)(((object[])value)[2]), Convert.ToInt32(((object[])value)[0]), Convert.ToInt32(((object[])value)[1]));
                    else
                        smartSensor.ConfigureSingleExposureTimesMicroS(Convert.ToInt32(value));
                    return true;
                case enSensorParamType.smartRay_增益:   //不需要停止采集
                    smartSensor.SetGainParameters(Convert.ToInt32(value));
                    return true;
                case enSensorParamType.smartRay_内部触发频率:
                    smartSensor.StopAcquisition();
                    smartSensor.ConfigureDataTriggerInternalToFrequencyHz(Convert.ToInt32(value));
                    // smartSensor.StartAcquisition();
                    return true;
                case enSensorParamType.smartRay_外部触发:
                    smartSensor.StopAcquisition();
                    smartSensor.ConfigureDataTriggerExternal((Api.DataTriggerSource)((object[])value)[0], Convert.ToInt32(((object[])value)[1]), Convert.ToInt32(((object[])value)[2]), (Api.TriggerEdgeMode)(((object[])value)[3]));
                    //  smartSensor.StartAcquisition();
                    return true;
                case enSensorParamType.smartRay_内部触发:
                    smartSensor.StopAcquisition();
                    smartSensor.ConfigureDataTriggerInternalToFrequencyHz(Convert.ToInt32(value));
                    //  smartSensor.StartAcquisition();
                    return true;
                case enSensorParamType.smartRay_自由触发:
                    smartSensor.StopAcquisition();
                    smartSensor.ConfigureDataTriggerFreeRun();
                    //  smartSensor.StartAcquisition();
                    return true;
                case enSensorParamType.smartRay_激光模式:
                    smartSensor.StopAcquisition();
                    smartSensor.SetLaserMode((Api.LaserMode)value);
                    // smartSensor.StartAcquisition();
                    return true;

                case enSensorParamType.smartRay_兴趣区域:
                    // smartSensor.StopAcquisition();
                    smartSensor.ConfigureRegionOfInterestDivider(((int[])value)[0], ((int[])value)[1], ((int[])value)[2], ((int[])value)[3]);
                    // smartSensor.StartAcquisition();
                    return true;

                case enSensorParamType.smartRay_激光线阈值:
                    smartSensor.StopAcquisition();
                    smartSensor.SetlaserLineThreshold((int[])value);
                    //  smartSensor.StartAcquisition();
                    return true;

                case enSensorParamType.smartRay_曝光模式:
                    smartSensor.StopAcquisition();
                    smartSensor.SetExposureMode(Convert.ToInt32(value));
                    // smartSensor.StartAcquisition();
                    return true;

                case enSensorParamType.smartRay_多重曝光合并模式:
                    smartSensor.StopAcquisition();
                    smartSensor.SetMultiExposureMode((Api.MultipleExposureMergeModeType)(value));
                    //  smartSensor.StartAcquisition();
                    return true;

                case enSensorParamType.smartRay_扫描轮廓数量:
                    smartSensor.StopAcquisition();
                    smartSensor.SetNumberOfProfilesToCapture(Convert.ToUInt32(value));
                    // smartSensor.StartAcquisition();
                    return true;

                case enSensorParamType.Coom_保存参数:
                    smartSensor.SaveParameterSetToFile(SmartRaySensor.ParameterSet.Snapshot3d);
                    // smartSensor.StartAcquisition();
                    return true;

                case enSensorParamType.Coom_激光位姿: //this.measureRange
                    //this.laserPose = (HTuple)value;
                    return true;

                //case enSensorParamType.Coom_激光校准参数: //this.measureRange
                //     this.sensorCalibrationParam=(double[])value;
                //    return true;
                default:
                    return false;
            }
        }
        public  bool StartTrigger() //enUserTriggerSource param
        {
            Api.DataTriggerMode triggerMode = (Api.DataTriggerMode)smartSensor.GetDataTriggerParameters()[0];
            Api.ImageAcquisitionType type = (Api.ImageAcquisitionType)smartSensor.GetImageAquisitionType();
            if (type == Api.ImageAcquisitionType.LiveImage)
                setImageAcqType(Api.ImageAcquisitionType.PointCloud);
            switch (triggerMode.ToString().Trim())
            {
                case "FreeRunning":
                    smartSensor.ClearBuffer();
                    smartSensor.StartAcquisition();
                    break;
                case "Internal":
                    smartSensor.ClearBuffer();
                    smartSensor.StartAcquisition();
                    break;
                case "External":
                    smartSensor.ClearBuffer();
                    smartSensor.StartAcquisition();
                    break;
            }
            return true;
        }
        public  bool StopTrigger() //enUserTriggerSource param
        {
            Api.DataTriggerMode triggerMode = (Api.DataTriggerMode)smartSensor.GetDataTriggerParameters()[0];
            if (triggerMode == Api.DataTriggerMode.FreeRunning)
            {
                smartSensor.StopAcquisition();
            }
            else
            {
                switch (smartSensor.GetImageAquisitionType().ToString().Trim())
                {
                    case "ProfileIntensityLaserLineThickness":
                    case "ZMapIntensityLaserLineThickness":
                        smartSensor.WaitForImage(1);
                        smartSensor.StopAcquisition();
                        break;
                    case "PointCloud":
                        smartSensor.AcquirePointCloud();
                        smartSensor.StopAcquisition();
                        break;
                }
            }
            return true;
        }
        private void setImageAcqType(Api.ImageAcquisitionType imageAcqType)
        {
            int triggerDivider;
            int triggerDelay;
            TriggerEdgeMode triggerDirection;
            smartSensor.StopAcquisition();
            switch (imageAcqType)
            {
                case Api.ImageAcquisitionType.LiveImage:
                    smartSensor.LoadParameterSetFromFile(SmartRaySensor.ParameterSet.LiveImage);
                    // smartSensor.ConfigureImageAquisition(Api.ImageAcquisitionType.LiveImage, 1200);
                    smartSensor.SendParameterSet();
                    smartSensor.StartAcquisition();
                    break;

                case Api.ImageAcquisitionType.ProfileIntensityLaserLineThickness:
                    smartSensor.LoadParameterSetFromFile(SmartRaySensor.ParameterSet.Snapshot3d);
                    scanLineCount = smartSensor.GetNumberOfProfilesToCapture();
                    smartSensor.ConfigureImageAquisition(Api.ImageAcquisitionType.ProfileIntensityLaserLineThickness, this.scanLineCount);
                    smartSensor.SetMetaDataExportEnable(true);
                    smartSensor.SendParameterSet();
                    break;

                case Api.ImageAcquisitionType.PointCloud:
                    //smartSensor.StopAcquisition();
                    smartSensor.LoadParameterSetFromFile(SmartRaySensor.ParameterSet.Snapshot3d);
                    // refine image acquiring parameters
                    scanLineCount = smartSensor.GetNumberOfProfilesToCapture();
                    smartSensor.ConfigureImageAquisition(Api.ImageAcquisitionType.PointCloud, this.scanLineCount);
                    Api.GetDataTriggerExternalTriggerParameters(smartSensor._sensorObject, out triggerDivider, out triggerDelay, out triggerDirection);
                    smartSensor.SetMetaDataExportEnable(true);
                    smartSensor.SetTransportResolution(1.0f);
                    smartSensor.SendParameterSet();
                    break;

                case Api.ImageAcquisitionType.ZMapIntensityLaserLineThickness:
                    smartSensor.LoadParameterSetFromFile(SmartRaySensor.ParameterSet.Snapshot3d);
                    // refine image aquiring parameters
                    scanLineCount = smartSensor.GetNumberOfProfilesToCapture();
                    smartSensor.ConfigureImageAquisition(Api.ImageAcquisitionType.ZMapIntensityLaserLineThickness, this.scanLineCount);
                    smartSensor.SetMetaDataExportEnable(true);
                    smartSensor.SetTransportResolution(0.0065f);
                    smartSensor.SetZmapResolution(0.056f, 0.0085f);
                    smartSensor.SendParameterSet();
                    break;
                default:
                    break;

            }
        }


    }


}

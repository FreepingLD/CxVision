using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartRemoteProxy;
using System.Windows.Forms;
using WLIState;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;
using System.Threading;
using Common;

namespace Sensor
{
    public class GbsFaceWliRemote :SensorBase, ISensor
    {
        private CameraConfigProxy cameraConfig;
        private LightConfigProxy lightConfig;
        private ScanDeviceConfigProxy scanDeviceConfig;
        private MeasProcedureConfigProxy measProcedureConfig;
        private MeasurementConfigProxy measurementConfig;
        private ObjectiveConfigProxy objectiveConfig;
        private ServerConfigProxy serverConfig;
        private DataProxy dataConfig;

        private bool isFinish = true;
        private bool useGPUForComputation_p = true;
        private bool enableEPSIMode_p = false;
        private bool enablePSIMode_p = false;
        private uint scanStepSizeMultiplier = 1;
        private uint imageWidth = 0, imageHeight = 0;

        public CameraConfigProxy CameraConfig { get => cameraConfig; set => cameraConfig = value; }
        public LightConfigProxy LightConfig { get => lightConfig; set => lightConfig = value; }
        public ScanDeviceConfigProxy ScanDeviceConfig { get => scanDeviceConfig; set => scanDeviceConfig = value; }
        public MeasProcedureConfigProxy MeasProcedureConfig { get => measProcedureConfig; set => measProcedureConfig = value; }
        public MeasurementConfigProxy MeasurementConfig { get => measurementConfig; set => measurementConfig = value; }
        public ObjectiveConfigProxy ObjectiveConfig { get => objectiveConfig; set => objectiveConfig = value; }
        public ServerConfigProxy ServerConfig { get => serverConfig; set => serverConfig = value; }
        public DataProxy DataConfig { get => dataConfig; set => dataConfig = value; }
        public bool EnableEPSIMode_p
        {
            get => enableEPSIMode_p;
            set
            {
                enableEPSIMode_p = value;
            }
        }
        public bool EnablePSIMode_p
        {
            get => enablePSIMode_p;
            set
            {
                enablePSIMode_p = value;
            }
        }
        public uint ScanStepSizeMultiplier { get => scanStepSizeMultiplier; set => scanStepSizeMultiplier = value; }
        public bool IsFinish { get => isFinish; set => isFinish = value; }


        public GbsFaceWliRemote()
        {
            cameraConfig = new CameraConfigProxy();
            lightConfig = new LightConfigProxy();
            scanDeviceConfig = new ScanDeviceConfigProxy();
            measProcedureConfig = new MeasProcedureConfigProxy();
            measurementConfig = new MeasurementConfigProxy();
            objectiveConfig = new ObjectiveConfigProxy();
            serverConfig = new ServerConfigProxy();
            dataConfig = new DataProxy();
            dataConfig.OnMeasDataReady += new DataProxy.MeasDataReady(MeasureComplete);
            dataConfig.OnFrameArrived += new DataProxy.FrameArrived(FramArrive);
            dataConfig.MeasDataTimeout = 10000;
        }


        public bool Connect(SensorConnectConfigParam configParam)
        {
            bool result = false; // 
            try
            {
                this.ConfigParam = configParam;
                this.Name = this.ConfigParam.SensorName;
                this.LaserParam = (LaserParam)new LaserParam().Read(this.Name);
                this.LaserParam.SensorName = configParam.SensorName;
                //////////////////////////////////////////
                switch (configParam.ConnectType)
                {
                    case Common.enUserConnectType.Network:
                        //this.ConfigParam.ConnectAddress = configParam.ConnectAddress;
                        // 远程服务器和远程DLL只能使用２．４．１９里面的
                        if (this.serverConfig.IsInit())
                            result = true;
                        else
                        {
                            //this.readSensorParam(param);
                            result = this.serverConfig.InitializeSmartRemoteProxy(configParam.ConnectAddress, "SVSettings.xml", GetUserResponse, ProcessProgramMessage);
                            this.MeasurementConfig.IsEPSIModeEnabled(out enableEPSIMode_p);
                            this.MeasurementConfig.IsPSIModeEnabled(out enablePSIMode_p);
                            uint[] pScanStepSizeMultiplierList_p;
                            uint pScanStepSizeMultiplierID_p;
                            this.MeasurementConfig.GetSupportedScanStepSizeMultiplierList(out pScanStepSizeMultiplierList_p);
                            this.MeasurementConfig.GetScanStepSizeMultiplierID(out pScanStepSizeMultiplierID_p);
                            if (pScanStepSizeMultiplierList_p != null && pScanStepSizeMultiplierList_p.Length > 0)
                                this.scanStepSizeMultiplier = pScanStepSizeMultiplierList_p[pScanStepSizeMultiplierID_p];
                            cameraConfig.GetCameraImageSize(out imageWidth, out imageHeight);
                            this.LaserParam.DataWidth = (int)this.imageWidth;
                            this.LaserParam.DataHeight = (int)this.imageHeight;
                        }
                        break;

                    default:
                        return result;
                }
            }
            catch (Exception ee)
            {
                result = false;
            }
            configParam.ConnectState = result;
            return result;
        }

        public bool Init()
        {
            //this.SensorParam.SensorType = enUserSensorType.面激光;
            
            return true;
        }

        /// <summary>
        /// 设置传感器参数
        /// </summary>
        /// <returns></returns>
        public bool SetParam(object paramType, object value)
        {
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
                case enSensorParamType.Coom_每线点数:
                    return this.LaserParam.DataWidth;

                case enSensorParamType.Coom_传感器名称:
                    return this.ConfigParam.SensorName;

                case enSensorParamType.Coom_激光位姿:
                    //return this.laserPose;

                case enSensorParamType.Coom_激光校准参数: //this.measureRange
                    return this.LaserParam.LaserCalibrationParam;


                case enSensorParamType.Coom_传感器类型: //this.measureRange
                    return this.ConfigParam.SensorType;
                default:
                    return null;
            }
        }


        /// <summary>
        /// 断开传感器连接
        /// </summary>
        /// <returns></returns>
        public bool Disconnect()
        {
            bool result = false;
            try
            {
                if (serverConfig.IsInit())
                {
                    dataConfig.OnMeasDataReady -= new DataProxy.MeasDataReady(MeasureComplete);
                    dataConfig.OnFrameArrived -= new DataProxy.FrameArrived(FramArrive);
                    serverConfig.DeInit();
                    serverConfig.CloseServer();
                }
                result = true;
            }
            catch
            {

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
            if (serverConfig.IsInit())
            {
                double pScanRangeMinimumPosition_p, pScanRangeMaximumPosition_p;
                scanDeviceConfig.GetScanRange(out pScanRangeMinimumPosition_p, out pScanRangeMaximumPosition_p);// 获取锁定的扫描范围
                if ((pScanRangeMaximumPosition_p - pScanRangeMinimumPosition_p) < 0.05)
                {
                    scanDeviceConfig.LockScanRangeMinimumPosition(0.1);
                    scanDeviceConfig.LockScanRangeMaximumPosition(0.2); // 扫描范围需要重新设置
                }
                // : VSI =“Rough”, ePSI =“Smooth” or phase unwrapping =“Smooth(U)”
                // EnableEPSIMode_p :用于测量粗糙表面或光滑表面，测量光滑表面为true,测量粗糙表面为false
                // EnablePSIMode_p: 当测量光滑表面时设置为true,可以更快更精度的测量，如果测量粗糙表面，则要设置为false
                measurementConfig.SetMeasurementProcedureSettings(useGPUForComputation_p, enableEPSIMode_p, enablePSIMode_p, scanStepSizeMultiplier);
                bool minLock, maxLock;
                this.scanDeviceConfig.IsScanRangeMaximumPositionLocked(out maxLock);
                this.scanDeviceConfig.IsScanRangeMinimumPositionLocked(out minLock);
                if (maxLock && minLock)
                {
                    this.isFinish = false;
                    measurementConfig.ExecuteMeasurementProcedure();
                    result = true;
                }
                else
                {
                    // 这里要提示扫描范围没有锁定
                }
            }
            return result;
        }

        /// <summary>
        /// 停止触发传感器采集
        /// </summary>
        /// <returns></returns>
        public bool StopTrigger()
        {
            double startpScanPosition_p = 0, currentpScanPosition_p, pScanRangeMinimumPosition_p, pScanRangeMaximumPosition_p;
            scanDeviceConfig.GetScanRange(out pScanRangeMinimumPosition_p, out pScanRangeMaximumPosition_p);// 获取锁定的扫描范围
            int count = 0;
            while (true)
            {
                Application.DoEvents();
                scanDeviceConfig.GetScanPosition(out currentpScanPosition_p);
                if (Math.Abs(pScanRangeMaximumPosition_p - currentpScanPosition_p) < 0.002)
                {
                    count++;
                }
                if (count > 200)
                    break;
            }
            return true;
        }

        /// <summary>
        /// 从控制器中读取采集数据     0:dist;/ 1:dist2; /2:thick;/ 3:intensity;/ 4:x;/ 5:y; /6:z;/ 7:baryCenter
        /// </summary>
        /// <returns></returns>
        public Dictionary<enDataItem, object> ReadData()
        {
            Dictionary<enDataItem, object> list = new Dictionary<enDataItem, object>();
            float pHeightDataOffset_p;
            float[] pHeightData_p, pQualityData_p;
            byte[] pSurfaceIntensityData_p;
            //uint pObjectiveID_p;
            double pXMetricResolution_p, pYMetricResolution_p;
            double[] x, y, z, Quality;
            cameraConfig.GetCameraImageSize(out imageWidth, out imageHeight);
            this.LaserParam.DataWidth = (int)this.imageWidth;
            this.LaserParam.DataHeight = (int)this.imageHeight;
            objectiveConfig.GetMetricResolution(out pXMetricResolution_p, out pYMetricResolution_p);
            //dataConfig.GetMeasurementResultData(out pHeightData_p, out pQualityData_p, ref imageWidth, ref imageHeight, out pHeightDataOffset_p);
            dataConfig.GetExtendedMeasurementResultData(out pHeightData_p, out pQualityData_p, out pSurfaceIntensityData_p, ref imageWidth, ref imageHeight, out pHeightDataOffset_p); // 执行完获取数据函数后将触发测量完成事件
            //////////
            x = new double[pHeightData_p.Length];
            y = new double[pHeightData_p.Length];
            z = new double[pHeightData_p.Length];
            Quality = new double[pHeightData_p.Length];
            for (int i = 0; i < imageHeight; i++)
            {
                for (int j = 0; j < imageWidth; j++)
                {
                    x[i * imageWidth + j] = j * pXMetricResolution_p;
                    y[i * imageWidth + j] = i * pYMetricResolution_p;
                    z[i * imageWidth + j] = pHeightData_p[i * imageWidth + j];
                    Quality[i * imageWidth + j] = pQualityData_p[i * imageWidth + j];
                }
            }
            list.Add(enDataItem.Dist1,z);
            list.Add(enDataItem.Quality,Quality);
            list.Add(enDataItem.X,x);
            list.Add(enDataItem.Y,y);
            return list;
        }













        private void MeasureComplete(object sender, MeasDataEventArgs e)
        {
            this.isFinish = true; // 只有在获取数据后才会触发这个事件
        }
        private void FramArrive(object sender, ImgDataEventArgs e)
        {
            this.isFinish = true;
        }
        private void GetUserResponse(object sender, UserResponseEventArgs ea)
        {
            int a = 10;
        }
        private void ProcessProgramMessage(object sender, ProgramMessageEventArgs ea)
        {
            int a = 10;
        }



    }


}

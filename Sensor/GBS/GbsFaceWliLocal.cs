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
using HalconDotNet;
using Common;

namespace Sensor
{
    public class GbsFaceWliLocal : SensorBase, ISensor
    {
        private bool useGPUForComputation_p = true;
        private bool enableEPSIMode_p = false;
        private bool enablePSIMode_p = false;
        private int scanStepSizeMultiplier = 1;

        private IntPtr height_pointer = IntPtr.Zero, qualiti_pointer = IntPtr.Zero;
        private uint imageWidth = 0, imageHeight = 0;
        private int image_size;
        private IntPtr intPtr_ready = IntPtr.Zero;
        private IntPtr intPtr_error = IntPtr.Zero;
        private AutoResetEvent ready_Event = new AutoResetEvent(false);
        private AutoResetEvent error_Event = new AutoResetEvent(false);
        private WaitHandle[] waitHandles;



        public bool EnablePSIMode_p { get => enablePSIMode_p; set => enablePSIMode_p = value; }
        public int ScanStepSizeMultiplier { get => scanStepSizeMultiplier; set => scanStepSizeMultiplier = value; }
        public bool EnableEPSIMode_p { get => enableEPSIMode_p; set => enableEPSIMode_p = value; }

        public GbsFaceWliLocal()
        {

        }

        public bool Init()
        {
           
            this.LaserParam.DataWidth = 960;
            return true;
        }
        public bool Connect(SensorConnectConfigParam configParam)
        {
            bool result = false; // 
            try
            {
                this.ConfigParam = configParam;
                this.Name = this.ConfigParam.SensorName;
                this.LaserParam = (LaserParam)new LaserParam().Read(this.Name);
                this.LaserParam.SensorName = this.ConfigParam.SensorName;
                switch (configParam.ConnectType)
                {
                    case Common.enUserConnectType.SerialNumber:
                    case Common.enUserConnectType.Network:
                    case Common.enUserConnectType.USB:
                        if (smartVISwrap.SV3D_isInit())
                            result = true;
                        else
                        {
                            if (smartVISwrap.SV3D_init("SVSettings.xml"))
                            {
                                intPtr_ready = smartVISwrap.CreateEvent(IntPtr.Zero, false, false, "ReadyHandler");
                                intPtr_error = smartVISwrap.CreateEvent(IntPtr.Zero, false, false, "ErrorHandler");
                                ready_Event.SafeWaitHandle = new SafeWaitHandle(intPtr_ready, true); // 这里是否要放到初始化之后？
                                error_Event.SafeWaitHandle = new SafeWaitHandle(intPtr_error, true); // 这里是否要放到初始化之后？
                                waitHandles = new WaitHandle[] { ready_Event, error_Event };
                                result = smartVISwrap.SV3D_setResultReadyNotification(intPtr_ready, intPtr_error);
                                // 初始一些参数
                                smartVISwrap.SV3D_IsEPSIModeEnabled(out this.enableEPSIMode_p);
                                smartVISwrap.SV3D_IsPSIModeEnabled(out this.enablePSIMode_p);
                                uint[] pScanStepSizeMultiplierList_p;
                                uint pScanStepSizeMultiplierID_p;
                                uint zPosMultiplierListSize;
                                smartVISwrap.SV3D_getSupportedZPosMultiplierList(out pScanStepSizeMultiplierList_p, out zPosMultiplierListSize);
                                smartVISwrap.SV3D_getMultiplier(out pScanStepSizeMultiplierID_p);
                                if (pScanStepSizeMultiplierList_p != null && pScanStepSizeMultiplierList_p.Length > 0)
                                    this.scanStepSizeMultiplier = (int)pScanStepSizeMultiplierList_p[pScanStepSizeMultiplierID_p];
                                smartVISwrap.SV3D_getImageSize(out this.imageWidth, out this.imageHeight);
                                this.LaserParam.DataWidth = (int)this.imageWidth;
                                this.LaserParam.DataHeight = (int)this.imageHeight;
                                ////////////////////////////////
                                result = true;
                            }
                        }
                        break;

                    default:
                        return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            configParam.ConnectState = result;
            return result;
        }

        /// <summary>
        /// 设置传感器参数
        /// </summary>
        /// <returns></returns>
        public bool SetParam(object paramType, object value)
        {
            enSensorParamType type;
            if (paramType is enSensorParamType)
                type = (enSensorParamType)paramType;
            else
                Enum.TryParse(paramType.ToString(), out type);
            //////////////////////
            switch (type)
            {
                case enSensorParamType.Coom_每线点数:
                    this.LaserParam.DataWidth=(int)value;
                    break;
                case enSensorParamType.Coom_传感器名称:
                     this.ConfigParam.SensorName=value.ToString();
                    break;
                case enSensorParamType.Coom_激光位姿:
                    //this.LaserParam.laserPose=(HTuple)value;
                    break;
                //case enSensorParamType.Coom_激光校准参数: //this.measureRange
                //     this.sensorCalibrationParam=(ensensorca)value;
                //    break;

                case enSensorParamType.Coom_传感器类型: //this.measureRange
                     this.ConfigParam.SensorType = (enUserSensorType)value;
                    break;
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
                case enSensorParamType.Coom_每线点数:
                    return this.LaserParam.DataWidth;

                case enSensorParamType.Coom_传感器名称:
                    return this.ConfigParam.SensorName;

                case enSensorParamType.Coom_激光位姿:
                   // return this.laserPose;

                case enSensorParamType.Coom_激光校准参数: //this.measureRange
                    return this.LaserParam.LaserCalibrationParam;


                case enSensorParamType.Coom_传感器类型: //this.measureRange
                    return this.ConfigParam.SensorType;

                case enSensorParamType.Coom_点云宽度:
                    return (int)this.imageWidth;

                case enSensorParamType.Coom_点云高度:
                    return (int)this.imageHeight;

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
                if (smartVISwrap.SV3D_isInit())
                {
                    if (smartVISwrap.SV3D_deInit())
                        result = true;
                    if (height_pointer != null && height_pointer.ToInt64() > 0)
                        Marshal.FreeHGlobal(height_pointer);
                    if (qualiti_pointer != null && qualiti_pointer.ToInt64() > 0)
                        Marshal.FreeHGlobal(qualiti_pointer);
                    if(this.waitHandles!=null)
                    {
                        foreach (var item in this.waitHandles)
                        {
                            if(item!=null)
                                item.Dispose(); 
                        }
                    }

                }
            }
            catch
            {
                throw new Exception();
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
            if (smartVISwrap.SV3D_isInit())
            {
                double pScanRangeMinimumPosition_p, pScanRangeMaximumPosition_p;
                smartVISwrap.SV3D_getZrange(out pScanRangeMinimumPosition_p, out pScanRangeMaximumPosition_p);// 获取最大的扫描范围
                if ((pScanRangeMaximumPosition_p - pScanRangeMinimumPosition_p) < 0.05)
                {
                    smartVISwrap.SV3D_setZmin(0.1);
                    smartVISwrap.SV3D_setZmax(0.2); // 扫描范围需要重新设置
                }
                // : VSI =“Rough”, ePSI =“Smooth” or phase unwrapping =“Smooth(U)”
                // EnableEPSIMode_p :用于测量粗糙表面或光滑表面，测量光滑表面为true,测量粗糙表面为false
                // EnablePSIMode_p: 当测量光滑表面时设置为true,可以更快更精度的测量，如果测量粗糙表面，则要设置为false
                smartVISwrap.SV3D_configMeasurement(useGPUForComputation_p, enableEPSIMode_p, enablePSIMode_p, scanStepSizeMultiplier);
                bool minLock, maxLock;
                smartVISwrap.SV3D_isZmaxLocked(out maxLock);
                smartVISwrap.SV3D_isZminLocked(out minLock);
                if (maxLock && minLock)
                {
                    smartVISwrap.SV3D_measure();
                    result = true;
                }
                result = true;
            }
            return result;
        }

        /// <summary>
        /// 停止触发传感器采集
        /// </summary>
        /// <returns></returns>
        public bool StopTrigger()
        {
            bool result = false;
            //StopWatch 
            int waitResult = WaitHandle.WaitAny(waitHandles, 20000, false); // 等待采集完成，等待20S
            if (waitResult == 0)
                result = true;
            ///////////////////////////////
            if (waitResult == 1)
                result = false;
            return result;
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
            double pXMetricResolution_p, pYMetricResolution_p;
            double[] x, y, z, Quality;
            try
            {
                smartVISwrap.SV3D_getImageSize(out this.imageWidth, out this.imageHeight);
                this.LaserParam.DataWidth = (int)this.imageWidth;
                this.LaserParam.DataHeight = (int)this.imageHeight; 
                this.image_size = (int)(this.imageWidth * this.imageHeight);
                height_pointer = Marshal.AllocHGlobal(image_size * 4);
                qualiti_pointer = Marshal.AllocHGlobal(image_size * 4);
                pHeightData_p = new float[image_size]; // float类型占用4个字节
                pQualityData_p = new float[image_size];
                smartVISwrap.SV3D_getResolution(out pXMetricResolution_p, out pYMetricResolution_p);
                smartVISwrap.SV3D_getHeightData(height_pointer, qualiti_pointer, ref this.imageWidth, ref this.imageHeight, out pHeightDataOffset_p);// 本地算子在这里会不会阻塞？
                Marshal.Copy(height_pointer, pHeightData_p, 0, image_size);
                Marshal.Copy(qualiti_pointer, pQualityData_p, 0, image_size);
                //////////
                x = new double[pHeightData_p.Length];
                y = new double[pHeightData_p.Length];
                z = new double[pHeightData_p.Length];
                Quality = new double[pHeightData_p.Length];
                for (int i = 0; i < this.imageHeight; i++)
                {
                    for (int j = 0; j < this.imageWidth; j++)
                    {
                        if(pQualityData_p[i * this.imageWidth + j]>-1)
                        {
                            x[i * this.imageWidth + j] = j * pXMetricResolution_p;
                            y[i * this.imageWidth + j] = i * pYMetricResolution_p * -1;
                            if (this.LaserParam.IsMirrorZ)
                                z[i * this.imageWidth + j] = pHeightData_p[i * this.imageWidth + j] * -1;
                            else
                                z[i * this.imageWidth + j] = pHeightData_p[i * this.imageWidth + j];
                        }
                        else
                        {
                            x[i * this.imageWidth + j] = j * pXMetricResolution_p;
                            y[i * this.imageWidth + j] = i * pYMetricResolution_p * -1;
                            z[i * this.imageWidth + j] = -9999;
                        }
                        //Quality[i * this.imageWidth + j] = pQualityData_p[i * this.imageWidth + j];
                    }
                }
                list.Add(enDataItem.Dist1,z);
                list.Add(enDataItem.X,x);
                list.Add(enDataItem.Y,y);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Marshal.FreeHGlobal(height_pointer);
                Marshal.FreeHGlobal(qualiti_pointer);
            }
            return list;
        }





        private void frameCallback(object data)
        {
            //uint ImageDataSize_p;
            //byte[] pImageData_p = null;
            //smartSharpInterface.GetCameraImageData(out pImageData_p, out ImageDataSize_p);
            //if (OnFrameArrived != null)
            //{
            //    ImgDataEventArgs imageData = new ImgDataEventArgs();
            //    imageData.size = (int)ImageDataSize_p;
            //    pImageData_p.CopyTo(imageData.data, ImageDataSize_p);
            //    OnFrameArrived(this, imageData);
            //}

        }

        private UserResponseType GetUserResponse(int ErrorCode_p, string pAdditionalErrorMessage_p, UserResponseType AvailableUserResponseTypes_p, UserNotificationType UserNotificationType_p)
        {
            string notificationTypeAstring;

            if (UserNotificationType_p.Equals(UserNotificationType.ErrorNotificationType))
                notificationTypeAstring = "Error";
            else if (UserNotificationType_p.Equals(UserNotificationType.InfoNotificationType))
                notificationTypeAstring = "Info";
            else
                notificationTypeAstring = "Warning";

            MessageBoxButtons messageBoxButtons = MessageBoxButtons.OK;

            if ((UserNotificationType_p.HasFlag(UserResponseType.OKResponseType)) && (UserNotificationType_p.HasFlag(UserResponseType.CancelResponseType)))
                messageBoxButtons = MessageBoxButtons.OKCancel;
            else if (UserNotificationType_p.HasFlag(UserResponseType.OKResponseType))
                messageBoxButtons = MessageBoxButtons.OK;
            else if ((UserNotificationType_p.HasFlag(UserResponseType.CancelResponseType)) && (UserNotificationType_p.HasFlag(UserResponseType.RetryResponseType)) && (UserNotificationType_p.HasFlag(UserResponseType.IgnoreResponseType)))
                messageBoxButtons = MessageBoxButtons.AbortRetryIgnore;
            else if ((UserNotificationType_p.HasFlag(UserResponseType.YesResponseType)) && (UserNotificationType_p.HasFlag(UserResponseType.NoResponseType)) && (UserNotificationType_p.HasFlag(UserResponseType.CancelResponseType)))
                messageBoxButtons = MessageBoxButtons.YesNoCancel;
            else if ((UserNotificationType_p.HasFlag(UserResponseType.YesResponseType)) && (UserNotificationType_p.HasFlag(UserResponseType.NoResponseType)))
                messageBoxButtons = MessageBoxButtons.YesNo;
            else if ((UserNotificationType_p.HasFlag(UserResponseType.RetryResponseType)) && (UserNotificationType_p.HasFlag(UserResponseType.CancelResponseType)))
                messageBoxButtons = MessageBoxButtons.RetryCancel;

            DialogResult selectedMessageBoxOption = MessageBox.Show(pAdditionalErrorMessage_p, "Dialog " + notificationTypeAstring, messageBoxButtons);

            if (DialogResult.OK == selectedMessageBoxOption)
                return UserResponseType.OKResponseType;

            if ((DialogResult.Cancel == selectedMessageBoxOption) || (DialogResult.Abort == selectedMessageBoxOption))
                return UserResponseType.CancelResponseType;

            if (DialogResult.Yes == selectedMessageBoxOption)
                return UserResponseType.YesResponseType;

            if (DialogResult.No == selectedMessageBoxOption)
                return UserResponseType.NoResponseType;

            if (DialogResult.Ignore == selectedMessageBoxOption)
                return UserResponseType.IgnoreResponseType;

            if (DialogResult.Retry == selectedMessageBoxOption)
                return UserResponseType.RetryResponseType;

            return UserResponseType.CancelResponseType;
        }
        private void ProcessProgramMessage(int ErrorCode_p, string pAdditionalErrorMessage_p, UserNotificationType UserNotificationType_p)
        {
            string notificationTypeAstring;
            if (UserNotificationType_p.Equals(UserNotificationType.ErrorNotificationType))
                notificationTypeAstring = "Error";
            else if (UserNotificationType_p.Equals(UserNotificationType.InfoNotificationType))
                notificationTypeAstring = "Info";
            else
                notificationTypeAstring = "Warning";
            MessageBox.Show(pAdditionalErrorMessage_p, "Dialog " + notificationTypeAstring, MessageBoxButtons.OK);
        }


    }


}

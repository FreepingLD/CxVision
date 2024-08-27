using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using HalconDotNet;


namespace Sensor
{
    class hAcqCamClass : SensorBase, ISensor
    {
        private HFramegrabber hFramegrabber;
        //private HImage _grabImage;
        public bool Connect(SensorConnectConfigParam configParam) // 传入的名称
        {
            bool result = false;
            try
            {
                this.ConfigParam = configParam;
                this.Name = configParam.SensorName;
                this.CameraParam = (CameraParam)new CameraParam().Read(configParam.SensorName);
                this.CameraParam.SensorName = configParam.SensorName;
                ///////////////////////////////////
                switch (configParam.HalInterfaceType)
                {
                    /// 海康面阵相机接口
                    case enHalconInterfaceType.MVision:
                        switch (configParam.ConnectType)
                        {
                            default:
                                this.hFramegrabber.OpenFramegrabber(enHalconInterfaceType.MVision.ToString(), 0, 0, 0, 0, 0, 0, "default", -1, "default", -1,
                                                                    "false", "default", this.CameraParam.SensorName, 0, -1);
                                result = true;
                                break;
                            case enUserConnectType.USB:
                                this.hFramegrabber.OpenFramegrabber("USB3Vision", 0, 0, 0, 0, 0, 0, "default", -1, "default", -1,
                                       "false", "default", this.CameraParam.SensorName, 0, -1);
                                result = true;
                                break;
                            case enUserConnectType.Map:
                                this._MapName = configParam.ConnectAddress;
                                if (SensorManage.GetSensor(configParam.ConnectAddress) != null &&
                                    SensorManage.GetSensor(configParam.ConnectAddress).ConfigParam.ConnectState)  // 只有在映射源找开成功的情总下，映射对象才能打开成功
                                    result = true;
                                else
                                    result = false;
                                break;
                        }
                        break;
                    //////// Basler面阵接口
                    case enHalconInterfaceType.Pylon:
                        switch (configParam.ConnectType)
                        {
                            default:
                                this.hFramegrabber.OpenFramegrabber(enHalconInterfaceType.Pylon.ToString(), 0, 0, 0, 0, 0, 0, "default", -1, "default", -1,
                                    "false", "default", this.CameraParam.SensorName, 0, -1);
                                result = true;
                                break;
                            case enUserConnectType.USB:
                                this.hFramegrabber.OpenFramegrabber("USB3Vision", 0, 0, 0, 0, 0, 0, "default", -1, "default", -1,
                                       "false", "default", this.CameraParam.SensorName, 0, -1);
                                result = true;
                                break;
                            case enUserConnectType.Map:
                                this._MapName = configParam.ConnectAddress;
                                if (SensorManage.GetSensor(configParam.ConnectAddress) != null &&
                                    SensorManage.GetSensor(configParam.ConnectAddress).ConfigParam.ConnectState)  // 只有在映射源找开成功的情总下，映射对象才能打开成功
                                    result = true;
                                else
                                    result = false;
                                break;
                        }
                        break;
                    //////// 大华面阵相机接口
                    case enHalconInterfaceType.HMV3rdParty:
                        switch (configParam.ConnectType)
                        {
                            default:
                                this.hFramegrabber.OpenFramegrabber(enHalconInterfaceType.HMV3rdParty.ToString(), 0, 0, 0, 0, 0, 0, "default", -1, "default", -1,
                            "false", "default", this.CameraParam.SensorName, 0, -1);
                                result = true;
                                break;
                            case enUserConnectType.USB:
                                this.hFramegrabber.OpenFramegrabber("USB3Vision", 0, 0, 0, 0, 0, 0, "default", -1, "default", -1,
                                       "false", "default", this.CameraParam.SensorName, 0, -1);
                                result = true;
                                break;
                            case enUserConnectType.Map:
                                this._MapName = configParam.ConnectAddress;
                                if (SensorManage.GetSensor(configParam.ConnectAddress) != null &&
                                    SensorManage.GetSensor(configParam.ConnectAddress).ConfigParam.ConnectState)  // 只有在映射源找开成功的情总下，映射对象才能打开成功
                                    result = true;
                                else
                                    result = false;
                                break;
                        }
                        break;
                    /// halcon接口适用于所有支持 Gige 接口的相机 
                    default:
                    case enHalconInterfaceType.HalconGigeE:
                        switch (configParam.ConnectType)
                        {
                            default:
                                this.hFramegrabber.OpenFramegrabber("GigEVision2", 0, 0, 0, 0, 0, 0, "default", -1, "default", -1,
                                                                    "false", "default", this.CameraParam.SensorName, 0, -1); // GigEVision2
                                this.hFramegrabber.SetFramegrabberParam("grab_timeout", 5000);
                                result = true;
                                break;
                            case enUserConnectType.USB:
                                this.hFramegrabber.OpenFramegrabber("USB3Vision", 0, 0, 0, 0, 0, 0, "default", -1, "default", -1,
                                       "false", "default", this.CameraParam.SensorName, 0, -1);
                                result = true;
                                break;
                            case enUserConnectType.Map:
                                this._MapName = configParam.ConnectAddress;
                                if (SensorManage.GetSensor(configParam.ConnectAddress) != null &&
                                    SensorManage.GetSensor(configParam.ConnectAddress).ConfigParam.ConnectState)  // 只有在映射源找开成功的情总下，映射对象才能打开成功
                                    result = true;
                                else
                                    result = false;
                                break;
                        }
                        break;
                }
            }
            catch (HalconException ex)
            {
                LoggerHelper.Error(this.CameraParam.SensorName + "打开相机失败", ex);
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
                if (this.hFramegrabber != null && this.hFramegrabber.IsInitialized())
                {
                    this.hFramegrabber.CloseFramegrabber();
                }
                result = true;
                LoggerHelper.Info(this.CameraParam.SensorName + "关闭相机成功");
            }
            catch (Exception ex)
            {
                LoggerHelper.Error(this.CameraParam.SensorName + "关闭相机失败", ex);
                result = false;
            }
            return result;
        }

        public bool Init()
        {
            try
            {
                this.hFramegrabber = new HFramegrabber();
                return true;
            }
            catch
            {
                return false;
            }

        }

        public Dictionary<enDataItem, object> ReadData()
        {
            Dictionary<enDataItem, object> list = new Dictionary<enDataItem, object>();
            list.Add(enDataItem.Image, new ImageDataClass(this._grabImage, this.CameraParam)); //this.imageData
            return list;
        }
        public object GetParam(object paramType)
        {
            object value = "";
            try
            {
                switch (paramType.ToString())
                {
                    case "曝光":
                        if (this.hFramegrabber == null) return value;
                        HTuple hTuple = this.hFramegrabber.GetFramegrabberParam("ExposureTime");
                        value = hTuple.D;
                        break;
                    case "增益":
                        if (this.hFramegrabber == null) return value;
                        HTuple Gain = this.hFramegrabber.GetFramegrabberParam("Gain");
                        value = Gain.D;
                        break;
                    case "Image":
                        value = this._grabImage;
                        break;
                }
            }
            catch (Exception e)
            {
            }
            return value;
        }
        public bool SetParam(object paramType, object value)
        {
            bool result = false;
            try
            {
                switch (paramType.ToString())
                {
                    case "曝光":
                        if (this.hFramegrabber == null) return result;
                        double expose = 0;
                        double.TryParse(value.ToString(), out expose);
                        this.hFramegrabber.SetFramegrabberParam("ExposureTime", expose);
                        result = true;
                        break;
                    case "增益":
                        if (this.hFramegrabber == null) return result;
                        double Gain = 0;
                        double.TryParse(value.ToString(), out Gain);
                        this.hFramegrabber.SetFramegrabberParam("ExposureTime", Gain);
                        result = true;
                        break;
                };
            }
            catch (Exception e)
            {
                result = false;
            }
            return result;
        }

        public bool StartTrigger()
        {
            bool result = false;
            //lock (this._lockState)
            //{
            switch (this.ConfigParam.ConnectType)
            {
                case enUserConnectType.Map:
                    result = SensorManage.GetSensor(this._MapName).StartTrigger();
                    if (this._grabImage != null && this._grabImage.IsInitialized())
                        this._grabImage.Dispose();
                    this._grabImage = ((SensorBase)SensorManage.GetSensor(this._MapName)).GrabImage; // 用于远程调用的映射处理
                    if (result)
                        LoggerHelper.Error(this.CameraParam.SensorName + "图像采集成功");
                    else
                        LoggerHelper.Error(this.CameraParam.SensorName + "图像采集失败");
                    break;
                default:
                    switch (this.CameraParam.TriggerSource)
                    {
                        case enUserTriggerSource.NONE:
                            if (this._grabImage != null && this._grabImage.IsInitialized())
                                this._grabImage.Dispose();
                            HImage image = this.hFramegrabber?.GrabImage();
                            this.AdjImg(image, out this._grabImage);  // 调整图像的方向及图像畸变
                            int width, height;
                            this._grabImage.GetImageSize(out width, out height);
                            this.CameraParam.DataWidth = width;
                            this.CameraParam.DataHeight = height;
                            image?.Dispose();
                            result = true;
                            LoggerHelper.Info(this.CameraParam.SensorName + "图像采集成功");
                            break;
                        case enUserTriggerSource.外部IO触发:
                        case enUserTriggerSource.内部IO触发:
                        case enUserTriggerSource.编码器触发:

                            break;
                    }
                    break;
            }
            //}
            return result;
        }


        public bool StopTrigger()
        {
            bool result = false;
            try
            {
                //if (!this.SensorParam.CamParam.IsTrigger)
                //    this.hImage = this.hFramegrabber.GrabImage();
                result = true;
            }
            catch (Exception ex)
            {
                result = false;
                LoggerHelper.Error(this.Name + ":触发相机失败" + ex);
            }
            return result;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;


namespace Sensor
{
    public class BoMingPointLaser : SensorBase, ISensor
    {
        private SCDev mDev = new SCDev();
        private int mFirmwareVersion = 0;
        private ConnectionSetting_s connectionSettings;
        private Dictionary<enDataItem, object> listdata = new Dictionary<enDataItem, object>();

        public bool Connect(SensorConnectConfigParam configParam)
        {
            bool result = true;
            try
            {
                this.ConfigParam = configParam;
                this.Name = this.ConfigParam.SensorName;
                this.LaserParam = (LaserParam)new LaserParam().Read(configParam.SensorName);
                this.CameraParam.SensorName = this.ConfigParam.SensorName;
                switch (configParam.ConnectType)
                {
                    default:
                    case enUserConnectType.Network:
                        connectionSettings.connectionType = enConnectionType.Net;
                        connectionSettings.sofwareId = configParam.ConnectAddress;
                        if (mDev.Open(connectionSettings))
                        {
                            mFirmwareVersion = mDev.GetFirmwareVersion();
                            result = true;
                        }
                        else
                            result = false;
                        break;
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.Error(this.ConfigParam.SensorName + "打开失败", ex);
                result = false;
            }
            configParam.ConnectState = result;
            return result;
        }
        public bool Disconnect()
        {
            bool ok = false;
            ok = mDev.StopAcquisition();
            ok = mDev.Close();
            return ok;
        }
        public bool Init()
        {
         
            this.LaserParam.DataHeight = 1;
            this.LaserParam.DataWidth = 1;
            return true;
        }
        public Dictionary<enDataItem, object> ReadData()
        {
            listdata.Clear();
            double[] dist = null;
            double[] dist2 = null;
            double[] thick = null;
            double[] intensity = null;
            double[] encoder_X = null;
            double[] encoder_Y = null;
            double[] encoder_Z = null;
            StoredMeasureValuesEx_s[] pBufEx;
            StoredMeasureValues_s[] pBuf;
            GSMEx_s dataEx;
            GSM_s data;
            object obj = new object();
            bool ok = false;
            int len = 0;
            ok = mDev.GetStreamBufferLen(enPropId.StoredMeasureValues, ref len, -1);
            if (ok)
            {
                dist = new double[len];
                dist2 = new double[len];
                thick = new double[len];
                intensity = new double[len];
                if (this.mFirmwareVersion > 1500)
                {
                    pBufEx = new StoredMeasureValuesEx_s[len];
                    mDev.GetProperty(enPropId.GSM, typeof(GSMEx_s), ref obj);
                    ok = mDev.GetStreamBuffer(enPropId.StoredMeasureValues, pBufEx, len);
                    dataEx = (GSMEx_s)obj;
                    /////////
                    for (int i = 0; i < len; i++)
                    {
                        if (pBufEx[i].upperSurface == 0)
                            dist[i] = -9999;
                        else
                        {
                            if(this.LaserParam.IsMirrorZ)
                                dist[i] = pBufEx[i].upperSurface * -0.001f;
                            else
                                dist[i] = pBufEx[i].upperSurface * 0.001f;
                        }                         
                        if (pBufEx[i].lowerSurface == 0)
                            dist2[i] = -9999;
                        else
                        {
                            if (this.LaserParam.IsMirrorZ)
                                dist2[i] = pBufEx[i].lowerSurface * -0.001f;
                            else
                                dist2[i] = pBufEx[i].lowerSurface * 0.001f;
                        }                           
                        thick[i] = pBufEx[i].value * 0.001f;
                        intensity[i] = dataEx.strength1;
                    }
                }
                else
                {
                    pBuf = new StoredMeasureValues_s[len];
                    mDev.GetProperty(enPropId.GSM, typeof(GSM_s), ref obj);
                    ok = mDev.GetStreamBuffer(enPropId.StoredMeasureValues, pBuf, len);
                    data = (GSM_s)obj;
                    for (int i = 0; i < len; i++)
                    {
                        dist[i] = pBuf[i].value;
                        dist2[i] = pBuf[i].value;
                        thick[i] = pBuf[i].value;
                        intensity[i] = data.strength1;
                    }
                }
            }
            listdata.Add(enDataItem.Dist1,dist);
            listdata.Add(enDataItem.Dist2, dist2);
            listdata.Add(enDataItem.Thick, thick);
            listdata.Add(enDataItem.Intensity, intensity);
            listdata.Add(enDataItem.X, encoder_X);
            listdata.Add(enDataItem.Y, encoder_Y);
            listdata.Add(enDataItem.Z, encoder_Z);
            return listdata;
        }
        public object GetParam(object paramType)
        {
            bool ok = false;
            object oo = new object();
            enPropId propID;
            if ((paramType is enPropId))
                propID = (enPropId)paramType;
            else
                return null;
            switch (propID)
            {
                case enPropId.RAMSettings:
                    ok = mDev.GetProperty(propID, typeof(RAMSettings_s), ref oo);
                    return oo;

                case enPropId.ThresholdValue:
                    ok = mDev.GetProperty(propID, typeof(ThresholdValue_s), ref oo);
                    return oo;

                case enPropId.TrigMode:
                    ok = mDev.GetProperty(propID, typeof(TrigMode_s), ref oo);
                    return oo;

                case enPropId.TrigPeriod:
                    ok = mDev.GetProperty(propID, typeof(TrigPeriod_s), ref oo);
                    return oo;

                case enPropId.Exposure:
                    ok = mDev.GetProperty(propID, typeof(Exposure_s), ref oo);
                    return oo;

                case enPropId.LedValue:
                    ok = mDev.GetProperty(propID, typeof(LedValue_s), ref oo);
                    return oo;

                case enPropId.PGAGain:
                    ok = mDev.GetProperty(propID, typeof(PGAGain_s), ref oo);
                    return oo;

                case enPropId.FirstPeakMode:
                    ok = mDev.GetProperty(propID, typeof(FirstPeakMode_s), ref oo);
                    return oo;

                case enPropId.AverageValue:
                    ok = mDev.GetProperty(propID, typeof(AverageValue_s), ref oo);
                    return oo;

                case enPropId.每线点数:
                    return 1;

                case enPropId.传感器名称:
                    return this.ConfigParam.SensorName;

                case enPropId.传感器类型:
                    return this.ConfigParam.SensorType;

                default:
                    return ok;
            }
        }
        public bool SetParam(object paramType, object value)
        {
            bool ok = false;
            object oo = new object();
            RAMSettings_s ramSetting;
            enPropId propID;
            if ((paramType is enPropId))
                propID = (enPropId)paramType;
            else
                Enum.TryParse(paramType.ToString(), out propID);
            /////////////////////////
            switch (propID)
            {
                case enPropId.SaveSettings:
                    ok = mDev.SetProperty(propID, null);
                    return ok;

                case enPropId.StoredMeasureValuesCount:
                    StoredMeasureValuesCount_s count = new StoredMeasureValuesCount_s { value = Convert.ToInt32(value) };
                    ok = mDev.SetProperty(propID, count);
                    return ok;

                case enPropId.MeasureMode:
                    MeasureMode_s measure = new MeasureMode_s { value = (enMeasureMode)(value) };
                    ok = mDev.SetProperty(propID, measure);
                    ok = mDev.SetProperty(enPropId.TrigPeriod, 10000);// 只要更改了测量模式，那么就需要更改一次触发周期
                    return ok;

                case enPropId.ThresholdValue:
                    ThresholdValue_s thred = new ThresholdValue_s { value = Convert.ToUInt16(value) };
                    ok = mDev.SetProperty(propID, thred);
                    return ok;

                case enPropId.TrigMode:
                    TrigMode_s trig = new TrigMode_s { value = (enTrigMode)(value) };
                    ok = mDev.SetProperty(propID, trig);
                    return ok;

                case enPropId.TrigPeriod:
                    TrigPeriod_s per = new TrigPeriod_s { value = Convert.ToInt32(value) };
                    ok = mDev.SetProperty(propID, per);
                    return ok;

                case enPropId.Exposure:
                    // 获取当前测量模式
                    mDev.GetProperty(enPropId.RAMSettings, typeof(RAMSettings_s), ref oo);
                    ramSetting = (RAMSettings_s)oo;
                    ///////////////设置曝光
                    Exposure_s ex = new Exposure_s { value = Convert.ToInt32(value) }; // 设置曝光，当值为0时表示是自动曝光
                    TrigPeriod_s period = new TrigPeriod_s();
                    if (ramSetting.measureMode == (int)enMeasureMode.Distance)
                    {
                        if (ex.value == 0)
                            period.value = 10000;
                        else
                            if (ex.value < 80)
                            period.value = 200;
                        else
                            period.value = ex.value + 120;
                    }
                    else
                    {
                        if (ex.value == 0)
                            period.value = 10000;
                        else
                            if (ex.value < 100)
                            period.value = 600;
                        else
                            period.value = ex.value + 500;
                    }
                    ok = mDev.SetProperty(propID, ex);
                    ok = mDev.SetProperty(enPropId.TrigPeriod, period);
                    return ok;

                case enPropId.LedValue:
                    LedValue_s led = new LedValue_s { value = Convert.ToByte(value) };
                    ok = mDev.SetProperty(propID, led);
                    return ok;

                case enPropId.PGAGain:
                    PGAGain_s pg = new PGAGain_s { value = Convert.ToByte(value) };
                    ok = mDev.SetProperty(propID, pg);
                    return ok;

                case enPropId.FirstPeakMode:
                    FirstPeakMode_s peak = new FirstPeakMode_s { value = (enFirstPeakMode)(value) };
                    ok = mDev.SetProperty(propID, peak);
                    return ok;

                case enPropId.AverageValue:
                    AverageValue_s ave = new AverageValue_s { value = Convert.ToByte(value) };
                    ok = mDev.SetProperty(propID, ave);
                    return ok;

                case enPropId.RefractiveIndexMode:
                    RefractiveIndexMode_s refMode = new RefractiveIndexMode_s { value = Convert.ToByte(value) };
                    ok = mDev.SetProperty(propID, refMode);
                    return ok;

                case enPropId.InertiaAverage:
                    InertiaAverage_s smooth = new InertiaAverage_s { value = Convert.ToByte(value) };
                    ok = mDev.SetProperty(propID, smooth);
                    return ok;

                default:
                    return ok;
            }
        }
        public bool StartTrigger()
        {
            return mDev.StartAcquisition();
        }
        public bool StopTrigger()
        {
            return mDev.StopAcquisition();
        }



    }
}

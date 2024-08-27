using NvtLvmSdkDemo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sensor
{
    [Serializable]
    public class SensorFactory
    {

        /// <summary>
        /// 获取传感器对象 
        /// </summary>
        /// <param name="sensorType"></param>
        /// <returns></returns>
        public static ISensor GetSensor(enSensorLinkLibrary sensorType)
        {
            switch (sensorType)
            {
                case enSensorLinkLibrary.Stil点激光: //"Stil点激光":
                    return new CStil_P();
                case enSensorLinkLibrary.Stil线激光://"Stil线激光":
                    return new CStil_L();
                case enSensorLinkLibrary.SSZN线激光://"SSZN线激光":
                    return new SSZNLineLaser();
                case enSensorLinkLibrary.LiYi点激光://"LiYi点激光":
                    return new LiYiPointLaser();
                case enSensorLinkLibrary.大恒相机://"大恒相机":
                    return new DaHengCamera();
                case enSensorLinkLibrary.博明点激光://"博明点激光":
                    return new BoMingPointLaser();
                case enSensorLinkLibrary.博明结构光://"博明结构光":
                    return new BoMingStructuredLight();
                case enSensorLinkLibrary.SmartRay激光://"SmartRay激光":
                    return new SmartRayLineLaser();
                case enSensorLinkLibrary.LVM线激光://"LVM线激光":
                    return new LVM_LineLaser();
                case enSensorLinkLibrary.迈德威视相机://"迈德威视相机":
                    return new MdvsCamera();
                case enSensorLinkLibrary.GBSWliRemote://"GBSWliRemote":
                    return new GbsFaceWliRemote(); 
                case enSensorLinkLibrary.GBSWliLocal://"GBSWliLocal":
                    return new GbsFaceWliLocal(); //
                case enSensorLinkLibrary.KeyEnceLJV7000://"KeyEnceLJV7000":
                    return new LJV7000LineLaser();
                case enSensorLinkLibrary.Halcon接口://"Halcon接口":
                    return new hAcqCamClass();
                case enSensorLinkLibrary.Basler:
                    return new BaslerCam();
                case enSensorLinkLibrary.埃科线阵:
                    return new IKapLineCam();
                case enSensorLinkLibrary.大华面阵:
                    return new IMvCam();
                case enSensorLinkLibrary.DalsaLine:
                    return new DalsaLineCam();
                case enSensorLinkLibrary.海康威视:
                    return new HkVision();
                default:
                    return null; 
            }
        }

    }

    /// <summary>
    /// 传感器品牌
    /// </summary>
    public enum enSensorLinkLibrary
    {
        NONE,
        Stil点激光,
        Stil线激光,
        SSZN线激光,
        LiYi点激光,
        大恒相机,
        博明点激光,
        博明结构光,
        SmartRay激光,
        LVM线激光,
        迈德威视相机,
        GBSWli,
        GBSWliRemote,
        GBSWliLocal,
        KeyEnceLJV7000,
        海康威视,
        Halcon接口, // 使用Halcon库开发的接口
        Basler,
        埃科线阵,
        大华面阵,
        DalsaLine,
    }


    public enum enImageAcqMethod
    {
        明场,
        暗场,
        明暗场
    }

}

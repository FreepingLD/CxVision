using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotionControlCard
{
    [Serializable]
    public class MotionCardFactory
    {

        /// <summary>
        /// 获取相应的运动控制卡对象
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static IMotionControl GetMotionCard(enDeviceType type)
        {
            switch (type)
            {
                case enDeviceType.UWC4000://"UWC_4000":
                    return new Uwc4000MotionControl();
                ////////////////////////////////////////
                case enDeviceType.NMC_lib200://"NMC_lib20":
                    return new nmc_lib20MotionControl();
                ////////////////////////////////////////
                case enDeviceType.APT: // "APT":
                    return new AptMotionControl();
                ////////////////////////////////////////
                case enDeviceType.AMC100: // "AMC100":
                    return new Amc100_MotionControl();
                ////////////////////////////////////////
                case enDeviceType.LeiSai_DMC5000: // 
                    return new LeisaiDMC5000MotionCard();
                ////////////////////////////////////////
                case enDeviceType.ACS: // "ACS":
                    return new AcsMotionControl();
                ////////////////////////////////////////
                case enDeviceType.ZMC://  
                    return new ZmcMotionCard();
                ////////////////////////////////////////
                case enDeviceType.SiemensPlc://  
                    return new SiemensPlc();
                ////////////////////////////////////////
                case enDeviceType.InovancePlc://  
                    return new InovancePlc();
                ////////////////////////////////////////
                case enDeviceType.MelsecPlc://  
                    return new MelsecPlc();
                ////////////////////////////////////////
                case enDeviceType.KeyencePlc://  
                    return new KeyencePlc();
                ////////////////////////////////////////
                case enDeviceType.UserDefine://  
                    return new UserDefinedCard();
                ////////////////////////////////////////
                case enDeviceType.SocketClient://  
                    return new SocketClientDevice();
                ////////////////////////////////////////
                case enDeviceType.SocketServer://  
                    return new SocketServerDevice();
                ////////////////////////////////////////
                default:
                    return null;
            }
        }
    }

    //public enum enMotionCardType
    //{
    //    NONE,
    //    UWC_4000,
    //    NMC_lib20,
    //    APT,
    //    AMC100,
    //    LeiSai,
    //    ACS,
    //    ZMC,
    //    Melsec, // 三菱
    //    Siemens, // 西门子
    //    Modbus,  // Modbus协议
    //    Inovance, // 汇川
    //}




}

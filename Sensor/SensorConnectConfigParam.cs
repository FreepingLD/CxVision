using Common;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sensor
{
    /// <summary>
    /// 用于连接传感器的配置
    /// </summary>
    [Serializable]
    public class SensorConnectConfigParam
    {
        public string SensorName 
        {
            get;
            set;
        }
        public enSensorLinkLibrary SensorLinkLibrary { get; set; }
        public enUserSensorType SensorType
        {
            get;
            set;
        }
        public enUserConnectType ConnectType { get; set; }
        public string ConnectAddress { set; get; }

        public int ChannelCount { get; set; }  //
        public enImageAcqMethod ImageAcqMethod { set; get; }

        /// <summary>
        /// 图像采集接口
        /// </summary>
        public enHalconInterfaceType HalInterfaceType { set; get; } // 这个是用来传入参数的


        /// <summary>
        /// 相机设备描述
        /// </summary>
        public string DeviceDescribe { get; set; }

        public bool IsActive
        {
            set;
            get;
        }
        public bool ConnectState
        {
            set;
            get;
        }
        public bool IsAutoFocus
        {
            set;
            get;
        }

        public bool IsActiveDistortionCorrect 
        { get; 
            set; 
        }
        public object NONE { get; set; }
        private string _PortName;
        /// <summary>
        /// 串口
        /// </summary>
        public string PortName
        {
            get
            {
                return _PortName;
            }
            set
            {
                if (value != null && value.Length > 0)
                    this._PortName = value.ToUpper();
            }
        }

        /// <summary>
        /// 波特率
        /// </summary>
        public int BaudRate { set; get; }

        /// <summary>
        /// 停止位
        /// </summary>
        public StopBits StopBits
        {
            set;
            get;
        }

        /// <summary>
        /// 校验
        /// </summary>
        public Parity Parity
        {
            set;
            get;
        }

        /// <summary>
        /// 数据位
        /// </summary>
        public int DataBits
        {
            set;
            get;
        }
        public SensorConnectConfigParam()
        {
            this.IsActive = true;
            this.ConnectType = enUserConnectType.NONE;
            this.SensorLinkLibrary = enSensorLinkLibrary.NONE;
            this.SensorType = enUserSensorType.NONE;
            this.SensorName = "NONE";
            this.HalInterfaceType = enHalconInterfaceType.HalconGigeE;
            this.ConnectAddress = "192.18.1.10";
            this.DeviceDescribe = "";
            this.ConnectState = false;
            this.PortName = "COM1";
            this.BaudRate = 115200;
            this.StopBits = System.IO.Ports.StopBits.One;
            this.DataBits = 8;
            this.Parity = System.IO.Ports.Parity.None;
            this.ImageAcqMethod = enImageAcqMethod.明场;
            this.ChannelCount = 1;
            this.IsAutoFocus = false;
            this.IsActiveDistortionCorrect = false;
        }


    }

    /// <summary>
    /// 相机接口枚举,使用 Halcon 库时
    /// </summary>
    public enum enHalconInterfaceType
    {
        NONE,
        /// <summary>Halcon默认采图接口 </summary>
        HalconGigeE,
        /// <summary> balser接口 </summary>
        Pylon,
        /// <summary>大华接口 </summary>
        HMV3rdParty,
        /// <summary> dalsa接口 </summary>
        SaperaLT,
        /// <summary>dalsa图像采集卡接口 </summary>
        GenICamTL,

        Basler,
        /// <summary> Dalsa线阵相机</summary>
        CameraCLDalsa,
        // 海康威视
        MVision,

    }


}

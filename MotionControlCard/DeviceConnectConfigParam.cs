using Common;
using HslCommunication.Profinet.Inovance;
using HslCommunication.Profinet.Siemens;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotionControlCard
{

    public class DeviceConnectConfigParam
    {
        public string DeviceName { get; set; }
        // 设备类型
        public enDeviceType DeviceType { set; get; } // 公共参数

        // 设备类型
        public enDeviceModel DeviceModel { set; get; } // 公共参数

        /// <summary>
        /// 连接类型
        /// </summary>
        public enUserConnectType ConnectType { set; get; }
        public string IpAdress { get; set; }
        public int Port { get; set; }
        public byte StationNo { get; set; }
        public int HeartTime { get; set; }
        public bool IsActive
        {
            set;
            get;
        }
        public bool ConnectState { get; set; }
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


        public string ReceiveData
        {
            get;
            set;
        }
        public string SendData
        {
            get;
            set;
        }
        public object NONE { get; set; }
        public DeviceConnectConfigParam()
        {
            this.ConnectType = enUserConnectType.NONE;
            this.DeviceType = enDeviceType.NONE;
            this.DeviceModel = enDeviceModel.NONE;
            this.IsActive = true;
            this.DeviceName = "NONE";
            this.IpAdress = "192.18.1.10";
            this.Port = 502;
            this.ConnectState = false;
            this.HeartTime = 10000;
            //////////////////////
            this.PortName = "COM1";
            this.BaudRate = 115200;
            this.StopBits = System.IO.Ports.StopBits.One;
            this.DataBits = 8;
            this.Parity = System.IO.Ports.Parity.None;
            this.StationNo = 1;
        }



    }

    /// <summary>
    /// 设备类型
    /// </summary>
    public enum enDeviceType
    {
        NONE,
        UWC4000,
        LeiSai_DMC5000,
        AMC100,
        APT,
        ZMC,
        NMC_lib200,
        ACS,
        MelsecPlc, // 三菱
        SiemensPlc, // 西门子
        InovancePlc, // 汇川
        KeyencePlc,
        UserDefine,
        SocketClient,
        SocketServer,
    }
    public class InovancePlcPara
    {
        public InovanceSeries InovanceSerie { set; get; }
        public string CommunicationProtocol { set; get; }
        public InovancePlcPara()
        {
            this.InovanceSerie = InovanceSeries.H5U;
            this.CommunicationProtocol = "InovanceTcpNet";
        }

    }
    public class SiemensPlcPara
    {
        public SiemensPLCS SiemensSerie { set; get; }
        public string CommunicationProtocol { set; get; } // HslCommunication.Profinet.Siemens.

        public SiemensPlcPara()
        {
            this.SiemensSerie = SiemensPLCS.S200;
            this.CommunicationProtocol = "SiemensS7Net";
        }
    }
    public class MelsecPlcPara
    {
        public string CommunicationProtocol { set; get; }
        public MelsecPlcPara()
        {
            this.CommunicationProtocol = "";
        }
    }

    /// <summary>
    /// 某一设备的具体型号
    /// </summary>
    public enum enDeviceModel
    {
        // 汇川
        //
        // 摘要:
        //     适用于AM400、 AM400_800、 AC800 等系列
        AM = 0,
        //
        // 摘要:
        //     适用于H3U, XP 等系列
        H3U = 1,
        //
        // 摘要:
        //     适用于H5U 系列
        H5U = 2,
        // 西门子
        //
        // 摘要:
        //     1200系列
        S1200 ,
        //
        // 摘要:
        //     300系列
        S300 ,
        //
        // 摘要:
        //     400系列
        S400 ,
        //
        // 摘要:
        //     1500系列PLC
        S1500 ,
        //
        // 摘要:
        //     200的smart系列
        S200Smart ,
        //
        // 摘要:
        //     200系统，需要额外配置以太网模块
        S200,
        // 雷赛控制卡
        DMC5000,
        NONE,
        SocketClient,
        SocketServer,
        // 基恩士
        KeyenceMcAsciiNet,
        KeyenceMcBinaryNet,
        KeyenceNanoSerial,
        KeyenceNanoSerialOverTcp,
        KeyenceNanoServer,
    }



}



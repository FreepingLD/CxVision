using Common;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Light
{

    public class LightConnectConfigParam
    {        
        // 设备类型
        public enLightType LightType { set; get; } // 公共参数

        // 设备类型
        public enLightModel LightModel { set; get; } // 公共参数

        /// <summary>
        /// 连接类型
        /// </summary>
        public enUserConnectType ConnectType { set; get; }
        public string IpAdress { get; set; }  //
        public int ChannelCount { get; set; }  //
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
        public string LightName { get; set; }

        /// <summary>
        /// 公共绑定属性
        /// </summary>
        public object NONE { get; set; }
        public LightConnectConfigParam()
        {
            this.ConnectType = enUserConnectType.NONE;
            this.LightType = enLightType.NONE;
            this.LightModel = enLightModel.NONE;
            this.IsActive = true;
            this.LightName = "NONE";
            this.IpAdress = "192.18.1.10";
            //////////////////////
            this.PortName = "COM1";
            this.BaudRate = 115200;
            this.StopBits = System.IO.Ports.StopBits.One;
            this.DataBits = 8;
            this.Parity = System.IO.Ports.Parity.None;
            this.ChannelCount = 4;
            this.ConnectState = false;
        }



    }

    /// <summary>
    /// 光源品牌
    /// </summary>
    public enum enLightType
    {
        NONE,
        沃德谱,
        OPT,
        IO板卡, 
        盟拓,
        PPX盘鑫,
    }

    /// <summary>
    /// 光源型号
    /// </summary>
    public enum enLightModel
    {       
        NONE,
        OPT_4CH_,    
        OPT_8CH_,    
        OPT_16CH_,    
        PPX_4Ch,
        PPX_8Ch,
        PPX_16Ch,
        PD5_8CH_TYPE_,

    }



}



using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using System.IO.Ports;
using static SerialCommunicate.ASCII_Data;
using System.Threading;
using System.Windows.Forms;
using System.ComponentModel;

namespace Light
{
    public class LightBase 
    {
        protected string name;
        protected BindingList<LightParam> lightParam;
        protected int baudRate;
        protected enUserConnectType connectType;
        protected int channelCount;
        protected string connectAdress;
        protected LightConnectConfigParam _ConfigParam;
        protected System.IO.Ports.SerialPort serialPort;

        public LightConnectConfigParam ConfigParam { get=> _ConfigParam; set=> _ConfigParam = value; }
        public string Name { get => name; set => name = value; }
        public BindingList<LightParam> LightParamList { get => lightParam; set => lightParam = value; }
        public int BaudRate { get => baudRate; set => baudRate = value; }


    }

}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using System.IO.Ports;

namespace MotionControlCard
{
    public class SerialParam:INotifyPropertyChanged
    {
        public  event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged( [CallerMemberName] String PropertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(PropertyName));
            }
        }


        /// <summary>
        /// 串口
        /// </summary>
        public string PortName { set; get; }

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

        public SerialParam()
        {
            this.PortName = "COM1";
            this.BaudRate = 115200;
            this.StopBits = System.IO.Ports.StopBits.One;
            this.DataBits = 8;
            this.Parity = System.IO.Ports.Parity.None;
        }



    }
}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotionControlCard
{
    public class SocketParam
    {
        public event EventHandler ConnectEvent;
        public event EventHandler ReceiveDataEvent;
        public event EventHandler DataUpdata;
        public string IpAdress { get; set; }
        public int Port { get; set; }

        private bool _IsConnect = false;
        public bool IsConnect 
        {
            get {return _IsConnect; }
            set 
            {
                this._IsConnect =value;
                //this.ConnectEvent?.Invoke(this, new EventArgs());
                this.DataUpdata?.Invoke(this, new EventArgs());
            }
        }
        public int Timeout { get; set; }
        public int HeartTime { get; set; }
        public int DataLength { get; set; }

        private string _ReceiveData;
        public string ReceiveData 
        {
            get {return _ReceiveData; }
            set 
            {
                this._ReceiveData = value;
                //this.ReceiveDataEvent?.Invoke(this, new EventArgs());
                this.DataUpdata?.Invoke(this, new EventArgs());
            } 
        }
        public string SendData 
        {
            get;
            set;
        } 
        public string Describe { get; set; }
        public enSocketType SocketType { get; set; }

        private string _ClientIpEndPoint;
        public string  ClientIpEndPoint 
        {
            get {return _ClientIpEndPoint; }
            set {this._ClientIpEndPoint =value;
                this.DataUpdata?.Invoke(this, new EventArgs());
            }
        }

        public SocketParam()
        {
            this.IpAdress = "127.0.0.1";
            this.Port = 1024;
            this.IsConnect = false;
            this.Timeout = 5000;
            this.HeartTime = 5000;
            this.DataLength = 1024;
            this.ReceiveData = "";
            this.SendData = "";
            this.Describe = "NONE";
            this.SocketType = enSocketType.NONE;
            this.ClientIpEndPoint = "";
        }


    }

    public enum  enSocketType
    {
        客户端,
        服务器,
        NONE
    }

}

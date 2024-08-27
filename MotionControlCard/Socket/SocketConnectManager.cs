using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.ComponentModel;

namespace MotionControlCard
{
    public class SocketConnectManager
    {
        private static object sycnObj = new object();
        private static SocketConnectManager _Instance;
        public static SocketConnectManager Instance
        {
            get
            {
                if (_Instance == null)
                {
                    lock (sycnObj)
                    {
                        _Instance = new SocketConnectManager();
                    }
                }
                return _Instance;
            }
        }

        
        public BindingList<ClientSocket> ClientSocketList { get; set; }
        public BindingList<ServerSocket> ServerSocketList { get; set; }
        public bool InitSocket()
        {
            bool result = true;
            this.ClientSocketList = new BindingList<ClientSocket>();
            this.ServerSocketList = new BindingList<ServerSocket>();
            SocketConfigParamManger.Instance.Read();
            //if (SocketConfigParamManger.Instance.SocketConfigParamList == null) return false;
            if (SocketConfigParamManger.Instance.SocketConfigParamList == null)
                SocketConfigParamManger.Instance.SocketConfigParamList = new BindingList<SocketParam>();
            foreach (var item in SocketConfigParamManger.Instance.SocketConfigParamList)
            {
                switch(item.SocketType)
                {
                    case enSocketType.服务器:
                        ServerSocket serverSocket = new ServerSocket(item);
                        serverSocket.AcceptAsync();
                        ServerSocketList.Add(serverSocket);
                        break;
                }
            }
            // 先开启服务器
            foreach (var item in SocketConfigParamManger.Instance.SocketConfigParamList)
            {
                switch (item.SocketType)
                {
                    case enSocketType.客户端:
                        ClientSocket clientSocket = new ClientSocket(item);
                        clientSocket.ConnectAsync();
                        ClientSocketList.Add(clientSocket);
                        break;
                }
            }
            return result;
        }
        public bool UnInitSocket()
        {
            bool result = true;
            if(this.ClientSocketList!=null)
            {
                foreach (var item in this.ClientSocketList)
                {
                    item.Disconnect();
                    item.Close();
                }
                   
            }
            if (this.ServerSocketList != null)
            {
                foreach (var item in this.ServerSocketList)
                {
                    item.Close();
                }
                    
            }
            return result;
        }


        public ClientSocket GetClientSocket(string ip,int port)
        {
            if(ClientSocketList!=null)
            {
                foreach (var item in ClientSocketList)
                {
                    if (item.SocketParams.IpAdress == ip && item.SocketParams.Port == port) return item;
                }
            }
            return null;
        }
        public ServerSocket GetServerSocket(string ip, int port)
        {
            if (ServerSocketList != null)
            {
                foreach (var item in ServerSocketList)
                {
                    if (item.SocketParams.IpAdress == ip && item.SocketParams.Port == port) return item;
                }
            }
            return null;
        }


    }
}

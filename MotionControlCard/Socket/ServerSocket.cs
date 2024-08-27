using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MotionControlCard
{
    public class ServerSocket
    {
        private Socket _socket; // 用于客户端的侦听
        private Socket _clientConnectSocket; // 用于与客户端通信的 Socket 
        private List<Socket> _clientList;
        private IPEndPoint iPEndPoint;
        private SocketParam _socketParams;
        private byte[] ReceiveDataBuffer;
        private bool IsConnectClient = false;
        private int count = 0;
        private bool IsInit = false;
        private Stopwatch stopwatch;
        public bool IsSendFinish { get; set; }
        public SocketParam SocketParams { get => _socketParams; set => _socketParams = value; }

        public ServerSocket(SocketParam socketParam)
        {
            this.stopwatch = new Stopwatch();
            this._socketParams = socketParam;
            this.ReceiveDataBuffer = new byte[this._socketParams.DataLength];
            this._clientList = new List<Socket>();
        }

        public bool AcceptAsync()
        {
            IPAddress ip;
            IPAddress.TryParse(this._socketParams.IpAdress, out ip);
            this.iPEndPoint = new IPEndPoint(ip, this._socketParams.Port);
            this.IsInit = true;
            this._socket?.Close();
            this._socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            this._socket?.Bind(this.iPEndPoint); // 将侦听所有连接到该端口的客户端  Bind() 用于绑定 IPEndPoint 对象，在服务端使用。
            this._socket?.Listen(1000); // 监控所有发送到此主机的、特点端口的连接请求。服务端使用，客户端不需要
            this._socket?.BeginAccept(this._socketParams.DataLength, new AsyncCallback(OnAccepCompleted), this._socket);
            return true;
        }

        private void SendDataAsync(string data)
        {
            try
            {
                this.IsSendFinish = false;
                // 要使用接受的客户端Socket来发送数据,服务器与客户端的通信是通过接受的的Socket来通信的
                if (this._clientConnectSocket != null)
                {
                    byte[] data1 = Encoding.Default.GetBytes(data);
                    this._clientConnectSocket.SendBufferSize = data1.Length;
                    this._clientConnectSocket.BeginSend(data1, 0, data1.Length, SocketFlags.None, new AsyncCallback(OnSendCompleted), data1);
                }
                else
                    Console.WriteLine("服务器:" + this._socketParams.Describe + "发送数据失败，没有客户端连接或客户端连接已断开");
            }
            catch (SocketException ex)
            {
                this.IsSendFinish = false;
                Console.WriteLine("服务器:" + this._socketParams.Describe + this.iPEndPoint.ToString() + "发送数据出错" + ex.ToString());
            }
        }
        public bool SendDataAsync(byte[] data)
        {
            bool result = true;
            try
            {
                this.IsSendFinish = false;
                // 要使用接受的客户端Socket来发送数据,服务器与客户端的通信是通过接受的的Socket来通信的
                if (this._clientConnectSocket != null)
                {
                    this._clientConnectSocket.SendBufferSize = data.Length;
                    this._clientConnectSocket.BeginSend(data, 0, data.Length, SocketFlags.None, new AsyncCallback(OnSendCompleted), data);
                }
                else
                    Console.WriteLine("服务器:" + this._socketParams.Describe + "发送数据失败，没有客户端连接或客户端连接已断开");
            }
            catch (SocketException ex)
            {
                result = false;
                this.IsSendFinish = false;
                Console.WriteLine("服务器:" + this._socketParams.Describe + this.iPEndPoint.ToString() + "发送数据出错" + ex.ToString());
            }
            return result;
        }
        public void Close()
        {
            try
            {
                this.IsInit = false;
                this.IsConnectClient = false;
                Socket socketTemp = this._clientConnectSocket;
                this._clientConnectSocket = null;
                if (this._socket != null && this._socket.Connected)
                    this._socket?.Shutdown(SocketShutdown.Both);
                socketTemp?.Close();
                this._socket?.Close(5);
                // this._clientConnectSocket?.Disconnect(true);// 断开客户端的连接
                // this._socket?.Disconnect(true); // 服务器是被连接对象，所以它不能主动断开
            }
            catch (SocketException ex)
            {
                Console.WriteLine("关闭服务器:" + this._socketParams.Describe + this.iPEndPoint.ToString() + "出错" + ex.ToString());
            }
        }

        public bool WaiteSend()
        {
            this.stopwatch.Restart();
            while (!this.IsSendFinish)
            {
                if (this.stopwatch.ElapsedMilliseconds > this._socketParams.Timeout) break;
                Thread.Sleep(100);
            }
            this.stopwatch.Stop();
            return this.IsSendFinish;
        }
        private void BeginStartReceiveMessage()
        {
            try
            {
                //启动接收消息
                if (this._clientConnectSocket != null && IsConnectClient)
                    this._clientConnectSocket?.BeginReceive(ReceiveDataBuffer, 0, ReceiveDataBuffer.Length, SocketFlags.None, new AsyncCallback(OnReceiveCompleted), "");
            }
            catch (SocketException ex)
            {
                Console.WriteLine("服务器:" + this._socketParams.Describe + this.iPEndPoint.ToString() + "接收数据出错" + ex.ToString());
            }
        }
        private void OnReceiveCompleted(IAsyncResult ar)
        {
            try
            {
                if (ar.IsCompleted)
                {
                    if (this._clientConnectSocket != null)
                    {
                        //创建读取数据的缓存
                        int length = (int)this._clientConnectSocket?.EndReceive(ar);
                        if (length == 0) // 如果接收的数据长度为0：表示连接断开
                        {
                            count++;
                            if (count > 10) // 如果连续10次接收的数据为0，那么表示客户端断开了
                            {
                                this.IsConnectClient = false;
                                count = 0;
                            }
                        }
                        byte[] bytes = new byte[length];
                        //将数据复制到缓存中
                        Buffer.BlockCopy(ReceiveDataBuffer, 0, bytes, 0, bytes.Length);
                        this.ReceiveDataBuffer = new byte[this._socketParams.DataLength];
                        this._socketParams.ReceiveData = Encoding.Default.GetString(bytes);
                        if (this._socketParams.ReceiveData == "Connect")
                            this.SendDataAsync(Encoding.UTF8.GetBytes("OK\r\n")); // 客户端连接后发送的数据用于判断是否连接成功
                        Console.WriteLine("服务器:" + this._socketParams.Describe + "收数据：" + this._socketParams.ReceiveData);
                    }
                    else
                        Console.WriteLine("客户端已断开连接");
                }
            }
            catch (SocketException ex)
            {
                Console.WriteLine("服务器接收数据出错" + ex.ToString());
            }
            finally
            {
                //再次启动接收数据监听，接收下次的数据。
                BeginStartReceiveMessage();
            }
        }
        private void OnAccepCompleted(IAsyncResult ar) // 关闭 Socket 时会触发这个事件，关闭与之相连的客户端时也会触发这个事件
        {
            try
            {
                if (ar.IsCompleted && this.IsInit)
                {
                    this._socketParams.IsConnect = true;
                    this._clientConnectSocket?.Close();
                    this._clientConnectSocket = this._socket.EndAccept(ar);
                    this.IsConnectClient = true;
                    this._socketParams.ClientIpEndPoint = this._clientConnectSocket.RemoteEndPoint.ToString();
                    SendDataAsync(Encoding.Default.GetBytes("客户端:" + this._clientConnectSocket.RemoteEndPoint.ToString() + "连接到服务器:" + this.iPEndPoint.ToString()));
                    this.SendDataAsync(Encoding.UTF8.GetBytes("OK\r\n"));
                }
                else
                {
                    this._socketParams.IsConnect = false;
                    this.IsConnectClient = false;
                }
            }
            catch (SocketException ex)
            {
                Console.WriteLine("服务器端接受客户端连接请求出错" + ex.ToString());
            }
            finally
            {
                //启动接收消息,连接成功后，将开始接收数据 
                BeginStartReceiveMessage();
                if (this.IsInit) // 服务器初始化状态
                    this._socket?.BeginAccept(this._socketParams.DataLength, new AsyncCallback(OnAccepCompleted), this._socket);
            }
        }
        private void OnSendCompleted(IAsyncResult ar)
        {
            this.IsSendFinish = true;
            Console.WriteLine("服务器:" + this._socketParams.Describe + "->" + this.iPEndPoint.ToString() + "发送数据 = " + Encoding.UTF8.GetString((byte[])ar.AsyncState));
        }


    }
}

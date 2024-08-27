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
    public class ClientSocket
    {
        private Socket _socket;
        private IPEndPoint serverIpEndPoint;
        private SocketParam _socketParams;
        private byte[] ReceiveDataBuffer;
        public bool IsInit = false;
        private Timer HeartTimer;

        public bool IsConnectServer { get; set; }
        public bool IsReceiveFinish { get; set; }
        public bool IsSendFinish { get; set; }

        private Stopwatch stopwatch;
        public SocketParam SocketParams { get => _socketParams; set => _socketParams = value; }

        public EventHandler ResetConnect;
        public ClientSocket(SocketParam socketParam)
        {
            this.stopwatch = new Stopwatch();
            this._socketParams = socketParam;
            this.ReceiveDataBuffer = new byte[this._socketParams.DataLength];
        }

        public bool ConnectAsync()
        {
            IPAddress ip;
            IPAddress.TryParse(this._socketParams.IpAdress, out ip);
            this.serverIpEndPoint = new IPEndPoint(ip, this._socketParams.Port);
            this.IsInit = true;
            this._socket?.Close(5);
            this._socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            this._socket?.BeginConnect(this.serverIpEndPoint, new AsyncCallback(OnConnectedCompleted), this._socket); //异步创建连接
            this.IsConnectServer = WaiteConnect();
            //this.HeartTimer = new Timer(this.HeartCallback, "Test", this._socketParams.HeartTime * 2, this._socketParams.HeartTime * 3);
            return this.IsConnectServer;
        }

        public void Close()
        {
            try
            {
                this.IsInit = false;
                this.IsConnectServer = false;
                this._socketParams.IsConnect = false;
                if (this._socket != null && this._socket.Connected)
                    this._socket?.Shutdown(SocketShutdown.Both);
                this._socket?.Close(5);
                this._socket = null;
            }
            catch (Exception ex)
            {
                Console.WriteLine("关闭Socket出错" + ex.ToString());
            }
        }
        public void Disconnect()
        {
            try
            {
                this.IsConnectServer = false;
                this._socketParams.IsConnect = false;
                this.HeartTimer?.Change(System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite);
                this.HeartTimer?.Dispose();
                this._socket?.Disconnect(false);
                //this._socket?.BeginDisconnect(true, OnDisConnectedCompleted, this._socket); // 当服务器断开后执行该方法不会
            }
            catch (Exception ex)
            {
                Console.WriteLine("客户端:" + this._socketParams.Describe + "断开连接出错" + ex.ToString());
            }
        }
        public bool SendDataAsync(byte[] data)
        {
            bool result = true;
            try
            {
                if (this._socket != null)
                {
                    //bool re = this._socket.Connected;
                    this._socket.SendTimeout = this._socketParams.Timeout;
                    this._socket.SendBufferSize = data.Length;
                    this._socket?.BeginSend(data, 0, data.Length, SocketFlags.None, new AsyncCallback(OnSendDataCompleted), data);
                    //////////////////////////////////////////////
                    this.IsSendFinish = false;
                    Console.WriteLine("客户端:" + this._socketParams.Describe + "向服务器端发出数据" + Encoding.UTF8.GetString(data));
                }
                else
                    Console.WriteLine("客户端:" + this._socketParams.Describe + "Socket未初始化");
            }
            catch (Exception ex)
            {
                result = false;
                this.IsSendFinish = false;
                Console.WriteLine("客户端:" + this._socketParams.Describe + "发送数据出错:" + ex.ToString());
            }
            return result;
        }

        public bool GetConnectState()
        {
            bool state = true;
            try
            {
                object oo = this._socket.GetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.AcceptConnection);
            }
            catch (Exception ex)
            {
            }
            return state;
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
        public bool WaiteReceive()
        {
            this.stopwatch.Restart();
            while (!this.IsReceiveFinish)
            {
                if (this.stopwatch.ElapsedMilliseconds > this._socketParams.Timeout) break;
                Thread.Sleep(100);
            }
            this.stopwatch.Stop();
            return this.IsReceiveFinish;
        }
        public bool WaiteConnect()
        {
            // 监控是否连接
            this.stopwatch.Restart();
            while (!this.IsConnectServer)
            {
                if (this.stopwatch.ElapsedMilliseconds > this._socketParams.Timeout) break;
                Thread.Sleep(100);
            }
            this.stopwatch.Stop();
            return this.IsConnectServer;
        }

        private void BeginStartReceiveMessage()
        {
            try
            {
                //启动接收消息
                //this.IsReceiveServer = false;
                if (this._socket != null && IsConnectServer)
                    this._socket?.BeginReceive(this.ReceiveDataBuffer, 0, this.ReceiveDataBuffer.Length, SocketFlags.None, new AsyncCallback(OnReceiveCompleted), "");
                /// 是否接收完成
                this.IsReceiveFinish = false;
                Console.WriteLine("客户端：" + this._socketParams.Describe + this._socket.LocalEndPoint + "->开始接收数据:");   /// 对客户端来说，远程端口是服务器，对服务器来说，远程端口是客户端
            }
            catch (SocketException ex)
            {
                Console.WriteLine("客户端：" + this._socketParams.Describe + this._socket.LocalEndPoint + "->接收数据出错:" + ex.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine("客户端：" + this._socketParams.Describe + this._socket.LocalEndPoint + "->接收数据出错:" + ex.ToString());
            }
        }
        private void OnConnectedCompleted(IAsyncResult ar) // 关闭Socket时会引发这个事件
        {
            try
            {
                if (ar.IsCompleted && this.IsInit)
                {
                    this._socket.EndConnect(ar);
                    this._socketParams.ClientIpEndPoint = this._socket.LocalEndPoint.ToString(); // 客户端本地端口
                    this.IsConnectServer = true;
                    this._socketParams.IsConnect = true;
                    //启动接收消息,连接成功后，将开始接收数据
                    BeginStartReceiveMessage();
                    Console.WriteLine("客户端：" + this._socketParams.Describe + "连接服务器：" + this._socketParams.IpAdress + "  " + this._socketParams.Port.ToString() + "成功");
                    SendDataAsync(Encoding.Default.GetBytes("客户端：" + this._socketParams.Describe + ((Socket)ar.AsyncState).LocalEndPoint)); // 连接成功后向服务器发送数据
                }
                else
                {
                    this.IsConnectServer = false;
                    this._socketParams.IsConnect = false;
                }
            }
            catch (SocketException ex)
            {
                Console.WriteLine("连接服务器：" + this._socketParams.IpAdress + "-失败" + ex.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine("连接服务器：" + this._socketParams.IpAdress + "-失败" + ex.ToString());
            }
        }

        private void OnDisConnectedCompleted(IAsyncResult ar)
        {
            try
            {
                if (ar.IsCompleted)
                {
                    //if (ar.CompletedSynchronously)
                    this._socket?.EndDisconnect(ar);
                    this._socketParams.IsConnect = false;
                    this.IsConnectServer = false;
                }
            }
            catch (SocketException ex)
            {
                Console.WriteLine("客户端:" + this._socketParams.Describe + "断开服务器连接出错" + ex.ToString());
            }
        }
        private void OnReceiveCompleted(IAsyncResult ar)  // 服务器的客户端通信 Socket 关闭时也会触发这个方法
        {
            try
            {
                if (ar.IsCompleted && this.IsInit)
                {
                    int length = this._socket.EndReceive(ar); // begion与end编程，内部使用了一个事件来实现同步
                    //BeginStartReceiveMessage();
                    //创建读取数据的缓存
                    byte[] bytes = new byte[length];
                    //将数据复制到缓存中
                    Buffer.BlockCopy(this.ReceiveDataBuffer, 0, bytes, 0, bytes.Length);
                    this._socketParams.ReceiveData = Encoding.Default.GetString(bytes);
                    //再次启动接收数据监听，接收下次的数据。
                    Console.WriteLine("客户端：" + this._socketParams.Describe + "-接收服务端发送数据:" + this._socketParams.ReceiveData);
                    this.IsReceiveFinish = true;
                }
            }
            catch (SocketException ex)
            {
                this.IsConnectServer = false;
                this._socketParams.ReceiveData = ex.Message;
                Console.WriteLine("客户端：" + this._socketParams.Describe + "接收服务端数据出错,将终止消息的接收" + ex.ToString());
            }
            catch (Exception ex)
            {
                this.IsConnectServer = false;
                Console.WriteLine("客户端：" + this._socketParams.Describe + "接收服务端数据出错" + ex.ToString());
            }
            finally
            {
                if (this.IsConnectServer)
                    BeginStartReceiveMessage();
            }
        }
        private void OnSendDataCompleted(IAsyncResult ar)
        {
            this.IsSendFinish = true;
            Console.WriteLine("客户端:" + this._socketParams.Describe + " 发送数据：" + Encoding.Default.GetString((byte[])ar.AsyncState));
        }

        private void HeartCallback(object state)
        {
            this.SendDataAsync(Encoding.UTF8.GetBytes("Test"));
            this.WaiteSend();
            if (!this.IsSendFinish) // 表示数据发送失败
            {
                //if (ResetConnect != null)
                //    ResetConnect.Invoke(this, new EventArgs());
                //this.ConnectAsync();
            }
        }

    }

}

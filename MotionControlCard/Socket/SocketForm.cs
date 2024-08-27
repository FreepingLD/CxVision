using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Common;
using HalconDotNet;

namespace MotionControlCard
{
    public partial class SocketForm : Form
    {
        public SocketForm()
        {
            InitializeComponent();
        }

        BindingList<SocketParam> _clientParam = new BindingList<SocketParam>();
        BindingList<SocketParam> _serverParam = new BindingList<SocketParam>();
        private void frmSocket_Load(object sender, EventArgs e)
        {
            try
            {
                this.ClientSocketTypeColumn.ValueType = typeof(enSocketType);
                this.ClientSocketTypeColumn.Items.Clear();
                foreach (var item in Enum.GetValues(typeof(enSocketType)))
                {
                    this.ClientSocketTypeColumn.Items.Add(item);
                }
                this.ServerSocketTypeColumn.ValueType = typeof(enSocketType);
                this.ServerSocketTypeColumn.Items.Clear();
                foreach (var item in Enum.GetValues(typeof(enSocketType)))
                {
                    this.ServerSocketTypeColumn.Items.Add(item);
                }
                ///////////////////////////////////      
                if (SocketConfigParamManger.Instance.SocketConfigParamList != null)
                {
                    foreach (var item in SocketConfigParamManger.Instance.SocketConfigParamList)
                    {
                        switch (item.SocketType)
                        {
                            case enSocketType.客户端:
                                _clientParam.Add(item);
                                item.DataUpdata += ClientReciveData;
                                break;
                            case enSocketType.服务器:
                                _serverParam.Add(item);
                                item.DataUpdata += ServerReciveData;
                                break;
                        }
                    }
                    this.dgvClient.DataSource = _clientParam;
                    this.dgvServer.DataSource = _serverParam;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        #region 客户端
        private void gvClient_EditingControlShowing(object sender, DataGridViewCellFormattingEventArgs e)
        {
            try
            {
                this.dgvClient.Controls.Clear();
                //DataGridViewRow curRow = this.dgvClient.CurrentRow;
                //if (this.dgvClient.CurrentCell.ColumnIndex.Equals(11))
                //{
                Button btnSendServe = new Button();
                btnSendServe.BackColor = SystemColors.Control;
                btnSendServe.Text = "发送";
                btnSendServe.Visible = true;
                btnSendServe.AutoSize = true;
                btnSendServe.Width = this.dgvClient.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true).Height * 2;
                btnSendServe.Height = this.dgvClient.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true).Height;
                //btnSendServe.Click += new EventHandler(btnSendServe_Click);
                //btnSendServe.Dock = DockStyle.Right;
                this.dgvClient.Controls.Add(btnSendServe);
                btnSendServe.Location = new System.Drawing.Point(((this.dgvClient.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true).Right) -
                        (btnSendServe.Width)), this.dgvClient.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true).Top);
                //}
            }
            catch
            {

            }

        }

        private void dgvClient_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    switch (dgvClient.Columns[e.ColumnIndex].Name)
                    {
                        case "btnDelete":
                            //删除
                            if (MessageBox.Show("是否删除该项设备？    ", "删除提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
                            this._clientParam.RemoveAt(e.RowIndex);
                            break;

                        case "btnSendClient":
                            //发送
                            bool result = SocketConnectManager.Instance.GetClientSocket(this._clientParam[e.RowIndex].IpAdress, this._clientParam[e.RowIndex].Port).SendDataAsync(Encoding.Default.GetBytes(this._clientParam[e.RowIndex].SendData));
                            //bool result = SocketConnectManager.Instance.GetClientSocket(this._clientParam[e.RowIndex].IpAdress, this._clientParam[e.RowIndex].Port).WaiteSend();
                            if (result)
                                LoggerHelper.Info("向服务器发送数据成功");
                            else
                                LoggerHelper.Info("向服务器发送数据失败");
                            break;

                        case "ConBtnClient":
                            SocketConnectManager.Instance.GetClientSocket(this._clientParam[e.RowIndex].IpAdress, this._clientParam[e.RowIndex].Port).ConnectAsync();
                            this._clientParam[e.RowIndex].IsConnect = true;
                            //SocketConnectManager.Instance.GetClientSocket(this._clientParam[e.RowIndex].IpAdress, this._clientParam[e.RowIndex].Port).SocketParams.DataUpdata += ClientReciveData;
                            if (SocketConfigParamManger.Instance.SocketConfigParamList != null)
                            {
                                foreach (var item in SocketConfigParamManger.Instance.SocketConfigParamList)
                                {
                                    switch (item.SocketType)
                                    {
                                        case enSocketType.客户端:
                                            item.DataUpdata -= ClientReciveData;
                                            item.DataUpdata += ClientReciveData;
                                            break;
                                        case enSocketType.服务器:
                                            item.DataUpdata -= ServerReciveData;
                                            item.DataUpdata += ServerReciveData;
                                            break;
                                    }
                                }
                                this.dgvClient.DataSource = null;
                                this.dgvServer.DataSource = null;
                                this.dgvClient.DataSource = _clientParam;
                                this.dgvServer.DataSource = _serverParam;
                            }
                            break;

                        case "DisConBtnClient":
                            //////////////////////////
                            //SocketConnectManager.Instance.GetClientSocket(this._clientParam[e.RowIndex].IpAdress, this._clientParam[e.RowIndex].Port).SocketParams.DataUpdata -= ClientReciveData;
                            SocketConnectManager.Instance.GetClientSocket(this._clientParam[e.RowIndex].IpAdress, this._clientParam[e.RowIndex].Port).Disconnect();
                            this._clientParam[e.RowIndex].IsConnect = false;
                            ////////////////////////////////////
                            if (SocketConfigParamManger.Instance.SocketConfigParamList != null)
                            {
                                foreach (var item in SocketConfigParamManger.Instance.SocketConfigParamList)
                                {
                                    switch (item.SocketType)
                                    {
                                        case enSocketType.客户端:
                                            item.DataUpdata -= ClientReciveData;
                                            break;
                                        case enSocketType.服务器:
                                            item.DataUpdata -= ServerReciveData;
                                            break;
                                    }
                                }
                            }
                            break;

                        default:
                            break;
                    }
                }
            }
            catch
            {

            }
        }

        private void dgvClient_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false;
        }

        private void dgvClient_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            try
            {
                if (this.dgvClient.Columns[e.ColumnIndex].Name.Equals("isConnected"))
                {
                    //Config.SocketCItem sck = (Config.SocketCItem)this.dgvClient.Rows[e.RowIndex].DataBoundItem;
                    //if (sck == null)
                    //    e.Value = this.imageList1.Images[3];
                    //else
                    //    e.Value = (sck.IsConnected == true) ? this.imageList1.Images[1] : this.imageList1.Images[2];
                }
            }
            catch
            { }
        }

        private void ClientReciveData(object send, EventArgs e)
        {
            this.BeginInvoke(new Action(() => this.dgvClient.Refresh()));// 事件处理方法运行在引发该事件的线程上,以异步的方式处理事件处理程序,防止阻塞
        }
        private void ClientConnectData(object send, EventArgs e)
        {
            this.BeginInvoke(new Action(() => this.dgvClient.Refresh())); // 以异步的方式处理事件处理程序
        }
        #endregion

        #region 服务器
        private void dgvServer_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    //当前网络客户端
                    //Config.SocketSItem sck = (Config.SocketSItem)this.dgvServer.CurrentRow.DataBoundItem;

                    switch (dgvServer.Columns[e.ColumnIndex].Name)
                    {
                        case "btnDeleteS":
                            //删除
                            if (MessageBox.Show("是否删除该项设备？    ", "删除提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
                            this._serverParam.RemoveAt(e.RowIndex);
                            break;

                        case "btnSendServer":
                            //发送
                            SocketConnectManager.Instance.GetServerSocket(this._serverParam[e.RowIndex].IpAdress, this._serverParam[e.RowIndex].Port)?.SendDataAsync(Encoding.Default.GetBytes(this._serverParam[e.RowIndex].SendData));
                            break;

                        case "ConBtnServer":
                            SocketConnectManager.Instance.GetServerSocket(this._serverParam[e.RowIndex].IpAdress, this._serverParam[e.RowIndex].Port).AcceptAsync();
                            this._serverParam[e.RowIndex].IsConnect = true;
                            ////////////////////////////////////////
                            if (SocketConfigParamManger.Instance.SocketConfigParamList != null)
                            {
                                foreach (var item in SocketConfigParamManger.Instance.SocketConfigParamList)
                                {
                                    switch (item.SocketType)
                                    {
                                        case enSocketType.客户端:
                                            item.DataUpdata -= ClientReciveData;
                                            item.DataUpdata += ClientReciveData;
                                            break;
                                        case enSocketType.服务器:
                                            item.DataUpdata -= ServerReciveData;
                                            item.DataUpdata += ServerReciveData;
                                            break;
                                    }
                                }
                                this.dgvClient.DataSource = null;
                                this.dgvServer.DataSource = null;
                                this.dgvClient.DataSource = _clientParam;
                                this.dgvServer.DataSource = _serverParam;
                            }
                            break;

                        case "DisConBtnServer":
                            SocketConnectManager.Instance.GetServerSocket(this._serverParam[e.RowIndex].IpAdress, this._serverParam[e.RowIndex].Port).Close();
                            this._serverParam[e.RowIndex].IsConnect = false;
                            /////////////////////////////
                            if (SocketConfigParamManger.Instance.SocketConfigParamList != null)
                            {
                                foreach (var item in SocketConfigParamManger.Instance.SocketConfigParamList)
                                {
                                    switch (item.SocketType)
                                    {
                                        case enSocketType.客户端:
                                            item.DataUpdata -= ClientReciveData;
                                            break;
                                        case enSocketType.服务器:
                                            item.DataUpdata -= ServerReciveData;
                                            break;
                                    }
                                }
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
            catch
            {

            }
        }

        private void dgvServer_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false;
        }

        private void dgvServer_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            try
            {
                if (this.dgvServer.Columns[e.ColumnIndex].Name.Equals("isConnectedS"))
                {
                    //Config.SocketSItem sck = (Config.SocketSItem)this.dgvServer.Rows[e.RowIndex].DataBoundItem;
                    //if (sck == null)
                    //    e.Value = this.imageList1.Images[3];
                    //else
                    //    e.Value = (sck.IsConnected == true) ? this.imageList1.Images[1] : this.imageList1.Images[2];
                }
            }
            catch
            { }
        }
        private void ServerReciveData(object send, EventArgs e)
        {
            this.BeginInvoke(new Action(() => this.dgvServer.Refresh())); // 事件处理方法的线程运行在引发该事件的线程上
        }
        private void ServerConectData(object send, EventArgs e)
        {
            this.BeginInvoke(new Action(() => this.dgvServer.Refresh()));
        }
        #endregion

        private void SaveButton_Click(object sender, EventArgs e)
        {
            try
            {
                SocketConfigParamManger.Instance.SocketConfigParamList.Clear();
                if (this._clientParam != null)
                {
                    foreach (var item in this._clientParam)
                    {
                        SocketConfigParamManger.Instance.SocketConfigParamList.Add(item);
                    }
                }
                if (this._serverParam != null)
                {
                    foreach (var item in this._serverParam)
                    {
                        SocketConfigParamManger.Instance.SocketConfigParamList.Add(item);
                    }
                }
                SocketConfigParamManger.Instance.Save();
            }
            catch(Exception ex)
            {
                MessageBox.Show("保存Socket配置参数出错:" + ex.ToString());
            }
        }

        private void SocketForm_FormClosing(object sender, FormClosingEventArgs e)
        {            // 断开所有客户端的连接
            foreach (var item in SocketConnectManager.Instance.ServerSocketList)
            {
                item.Close();
            }
            // 断开服务的连接
            foreach (var item in SocketConnectManager.Instance.ClientSocketList)
            {
                item.Disconnect();
            }
            if (SocketConfigParamManger.Instance.SocketConfigParamList != null)
            {
                foreach (var item in SocketConfigParamManger.Instance.SocketConfigParamList)
                {
                    switch (item.SocketType)
                    {
                        case enSocketType.客户端:
                            item.DataUpdata -= ClientReciveData;
                            break;
                        case enSocketType.服务器:
                            item.DataUpdata -= ServerReciveData;
                            break;
                    }
                }
                this.dgvClient.DataSource = null;
                this.dgvServer.DataSource = null;
            }
        }

        private void 服务器连接button_Click(object sender, EventArgs e)
        {
            foreach (var item in SocketConfigParamManger.Instance.SocketConfigParamList)
            {
                switch (item.SocketType)
                {
                    case enSocketType.服务器:
                        item.DataUpdata -= ServerReciveData;
                        break;
                }
            }
            foreach (var item in SocketConnectManager.Instance.ServerSocketList)
            {
                item.Close();
            }
            ////  创建对应的客户端与服务端
            SocketConnectManager.Instance.ServerSocketList.Clear();
            foreach (var item in SocketConfigParamManger.Instance.SocketConfigParamList)
            {
                switch (item.SocketType)
                {
                    case enSocketType.服务器:
                        item.DataUpdata += ServerReciveData;
                        ServerSocket serverSocket = new ServerSocket(item);
                        SocketConnectManager.Instance.ServerSocketList.Add(serverSocket);
                        break;
                }
            }
            /////////////////////////
            foreach (var item in SocketConnectManager.Instance.ServerSocketList)
            {
                item.AcceptAsync();
            }
        }

        private void 客户端连接button_Click(object sender, EventArgs e)
        {
            foreach (var item in SocketConfigParamManger.Instance.SocketConfigParamList)
            {
                switch (item.SocketType)
                {
                    case enSocketType.客户端:
                        item.DataUpdata -= ClientReciveData;
                        break;
                }
            }
            foreach (var item in SocketConnectManager.Instance.ClientSocketList)
            {
                item.Close();
            }
            ////  创建对应的客户端与服务端
            SocketConnectManager.Instance.ClientSocketList.Clear();
            foreach (var item in SocketConfigParamManger.Instance.SocketConfigParamList)
            {
                switch (item.SocketType)
                {
                    case enSocketType.客户端:
                        item.DataUpdata += ClientReciveData;
                        ClientSocket clientSocket = new ClientSocket(item);
                        SocketConnectManager.Instance.ClientSocketList.Add(clientSocket);
                        break;
                }
            }
            foreach (var item in SocketConnectManager.Instance.ClientSocketList)
            {
                item.ConnectAsync();
            }
        }

        private void 服务器断开button_Click(object sender, EventArgs e)
        {
            foreach (var item in SocketConfigParamManger.Instance.SocketConfigParamList)
            {
                switch (item.SocketType)
                {
                    case enSocketType.服务器:
                        item.IsConnect = false;
                        item.ReceiveData = "";
                        item.SendData = "";
                        item.ClientIpEndPoint = "";
                        item.DataUpdata -= ServerReciveData;
                        break;
                }
            }
            foreach (var item in SocketConnectManager.Instance.ServerSocketList)
            {
                item.Close();
            }
            this.dgvServer.Refresh();
        }

        private void 客户端断开button_Click(object sender, EventArgs e)
        {
            foreach (var item in SocketConfigParamManger.Instance.SocketConfigParamList)
            {
                switch (item.SocketType)
                {
                    case enSocketType.客户端:
                        item.IsConnect = false;
                        item.ReceiveData = "";
                        item.SendData = "";
                        item.ClientIpEndPoint = "";
                        item.DataUpdata -= ClientReciveData;
                        break;
                }
            }
            foreach (var item in SocketConnectManager.Instance.ClientSocketList)
            {
                item.Disconnect();
                item.Close();
            }
            this.dgvClient.Refresh();
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (var item in SocketConnectManager.Instance.ClientSocketList)
                {
                    bool rr = item.GetConnectState();
                }
            }
            catch(Exception ex)
            {

            }

        }
    }
}

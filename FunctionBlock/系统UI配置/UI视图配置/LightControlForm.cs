using Common;
using FunctionBlock;
using Light;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FunctionBlock
{
    public partial class LightControlForm : Form
    {
        private ViewConfigParam _viewConfigParam;
        private bool IsLoad = false; // 窗体加载后，设置为true
        public LightControlForm(ViewConfigParam viewConfigParam)
        {
            InitializeComponent();
            this._viewConfigParam = viewConfigParam;
            //UserLoginParamManager.Instance.LoginParam.UserChange += new EventHandler(this.UserChange_Event);
            UserLoginParamManager.Instance.UserChange += new EventHandler(this.UserChange_Event);
        }
        public LightControlForm()
        {
            InitializeComponent();
        }
        private void LightControlForm_Load(object sender, EventArgs e)
        {
            BindData();
            this._viewConfigParam = this._viewConfigParam == null ? new ViewConfigParam() : this._viewConfigParam;
            this.Location = this._viewConfigParam.Location;
            this.Size = this._viewConfigParam.FormSize;
            this.IsLoad = true;
            //this.addContextMenu();
        }
        private void BindData()
        {
            try
            {
                // 这里不要可以选
                if (LightConnectManage.LightList.Count > 0)
                {
                    this.控制器名称comboBox.DataSource = LightConnectManage.LightList; // new BindingSource(SensorSource.SensorList, null);
                    this.控制器名称comboBox.DisplayMember = "Name";
                    this.控制器名称comboBox.SelectedItem = LightConnectManage.CurrentLight;
                    //this.控制器名称comboBox.DataBindings.Add("SelectedItem", this, "Sensor", true, DataSourceUpdateMode.OnPropertyChanged);
                }
            }
            catch (Exception e)
            {
                throw new Exception();
            }
        }
        public void UserChange_Event(object sender, EventArgs e)
        {
            try
            {
                UserLoginParam loginParam = sender as UserLoginParam;
                switch (loginParam.User)
                {
                    case enUserName.操作员:
                        //this.传感器comboBox1.Enabled = false;
                        //this.程序节点comboBox.Enabled = false;
                        this.buttonClose.Enabled = false;
                        break;
                    case enUserName.工程师:
                        //this.传感器comboBox1.Enabled = true;
                        //this.程序节点comboBox.Enabled = true;
                        this.buttonClose.Enabled = false;
                        break;
                    case enUserName.开发人员:
                        //this.传感器comboBox1.Enabled = true;
                        //this.程序节点comboBox.Enabled = true;
                        this.buttonClose.Enabled = true;
                        break;
                }
            }
            catch
            {
            }
        }
        private void 通道1checkBox_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.通道1checkBox.Checked)
                {
                    if (LightConnectManage.CurrentLight == null) throw new ArgumentNullException("LightSource.CurrentLight");
                    //this.通道1checkBox.Text = "通道1开";
                    this.通道1checkBox.BackgroundImage = global::FunctionBlock.Properties.Resources.openLight;
                    LightConnectManage.CurrentLight.LightParamList[0].LightName = LightConnectManage.CurrentLight.Name;
                    LightConnectManage.CurrentLight.LightParamList[0].LightValue = this.通道1trackBar.Value;
                    LightConnectManage.CurrentLight.LightParamList[0].ChannelState = true;
                    LightConnectManage.CurrentLight.Open(enLightChannel.Channel_1);
                }
                else
                {
                    //this.通道1checkBox.Text = "通道1关";
                    this.通道1checkBox.BackgroundImage = global::FunctionBlock.Properties.Resources.closeLight;
                    LightConnectManage.CurrentLight.LightParamList[0].ChannelState = false;
                    LightConnectManage.CurrentLight.LightParamList[0].LightName = LightConnectManage.CurrentLight.Name;
                    LightConnectManage.CurrentLight.LightParamList[0].LightValue = this.通道1trackBar.Value;
                    LightConnectManage.CurrentLight.Close(enLightChannel.Channel_1);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }

        private void 通道2checkBox_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                LightParam lightParam = new LightParam();
                if (this.通道2checkBox.Checked)
                {
                    if (LightConnectManage.CurrentLight == null) throw new ArgumentNullException("LightSource.CurrentLight");
                    //this.通道2checkBox.Text = "通道2开";
                    this.通道2checkBox.BackgroundImage = global::FunctionBlock.Properties.Resources.openLight;
                    LightConnectManage.CurrentLight.LightParamList[1].ChannelState = true;
                    LightConnectManage.CurrentLight.LightParamList[1].LightName = LightConnectManage.CurrentLight.Name;
                    LightConnectManage.CurrentLight.LightParamList[1].LightValue = this.通道2trackBar.Value;
                    LightConnectManage.CurrentLight.Open(enLightChannel.Channel_2);
                }
                else
                {
                    //this.通道2checkBox.Text = "通道2关";
                    this.通道2checkBox.BackgroundImage = global::FunctionBlock.Properties.Resources.closeLight;
                    LightConnectManage.CurrentLight.LightParamList[1].ChannelState = false;
                    LightConnectManage.CurrentLight.LightParamList[1].LightName = LightConnectManage.CurrentLight.Name;
                    LightConnectManage.CurrentLight.LightParamList[1].LightValue = this.通道2trackBar.Value;
                    LightConnectManage.CurrentLight.Close(enLightChannel.Channel_2);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void 通道3checkBox_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                LightParam lightParam = new LightParam();
                if (this.通道3checkBox.Checked)
                {
                    if (LightConnectManage.CurrentLight == null) throw new ArgumentNullException("LightSource.CurrentLight");
                    //this.通道3checkBox.Text = "通道3开";
                    this.通道3checkBox.BackgroundImage = global::FunctionBlock.Properties.Resources.openLight;
                    LightConnectManage.CurrentLight.LightParamList[2].ChannelState = true;
                    LightConnectManage.CurrentLight.LightParamList[2].LightName = LightConnectManage.CurrentLight.Name;
                    LightConnectManage.CurrentLight.LightParamList[2].LightValue = this.通道3trackBar.Value;
                    LightConnectManage.CurrentLight.Open(enLightChannel.Channel_3);
                }
                else
                {
                    //this.通道3checkBox.Text = "通道3关";
                    this.通道3checkBox.BackgroundImage = global::FunctionBlock.Properties.Resources.closeLight;
                    LightConnectManage.CurrentLight.LightParamList[2].ChannelState = false;
                    LightConnectManage.CurrentLight.LightParamList[2].LightName = LightConnectManage.CurrentLight.Name;
                    LightConnectManage.CurrentLight.LightParamList[2].LightValue = this.通道3trackBar.Value;
                    LightConnectManage.CurrentLight.Close(enLightChannel.Channel_3);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void 通道4checkBox_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                LightParam lightParam = new LightParam();
                if (this.通道4checkBox.Checked)
                {
                    if (LightConnectManage.CurrentLight == null) throw new ArgumentNullException("LightSource.CurrentLight");
                    //this.通道4checkBox.Text = "通道4开";
                    this.通道4checkBox.BackgroundImage = global::FunctionBlock.Properties.Resources.openLight;
                    LightConnectManage.CurrentLight.LightParamList[3].ChannelState = true;
                    LightConnectManage.CurrentLight.LightParamList[3].LightName = LightConnectManage.CurrentLight.Name;
                    LightConnectManage.CurrentLight.LightParamList[3].LightValue = this.通道4trackBar.Value;
                    LightConnectManage.CurrentLight.Open(enLightChannel.Channel_4);
                }
                else
                {
                    //this.通道4checkBox.Text = "通道4关";
                    this.通道4checkBox.BackgroundImage = global::FunctionBlock.Properties.Resources.closeLight;
                    LightConnectManage.CurrentLight.LightParamList[3].ChannelState = false;
                    LightConnectManage.CurrentLight.LightParamList[3].LightName = LightConnectManage.CurrentLight.Name;
                    LightConnectManage.CurrentLight.LightParamList[3].LightValue = this.通道4trackBar.Value;
                    LightConnectManage.CurrentLight.Close(enLightChannel.Channel_4);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void 控制器名称comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.控制器名称comboBox.SelectedIndex == -1) return;
            LightConnectManage.CurrentLight.Close(enLightChannel.Channel_All); // 切换光源时，先关闭之前的光源，再打开新的光源
            LightConnectManage.CurrentLight = (ILightControl)this.控制器名称comboBox.SelectedItem;
            LightConnectManage.CurrentLight.Open(enLightChannel.Channel_All); // 打开所有通道
        }

        private void 通道1trackBar_Scroll(object sender, EventArgs e)
        {
            if (LightConnectManage.CurrentLight == null) return;
            this.通道1值label.Text = this.通道1trackBar.Value.ToString();
            LightConnectManage.CurrentLight.LightParamList[0].LightValue = this.通道1trackBar.Value;
            if(LightConnectManage.CurrentLight.LightParamList[0].ChannelState)
            LightConnectManage.CurrentLight.SetLight(enLightChannel.Channel_1, this.通道1trackBar.Value);
        }

        private void 通道2trackBar_Scroll(object sender, EventArgs e)
        {
            if (LightConnectManage.CurrentLight == null) return;
            this.通道2值label.Text = this.通道2trackBar.Value.ToString();
            LightConnectManage.CurrentLight.LightParamList[1].LightValue = this.通道2trackBar.Value;
            if (LightConnectManage.CurrentLight.LightParamList[1].ChannelState)
                LightConnectManage.CurrentLight.SetLight(enLightChannel.Channel_2, this.通道2trackBar.Value);
        }

        private void 通道3trackBar_Scroll(object sender, EventArgs e)
        {
            if (LightConnectManage.CurrentLight == null) return;
            this.通道3值label.Text = this.通道3trackBar.Value.ToString();
            LightConnectManage.CurrentLight.LightParamList[2].LightValue = this.通道3trackBar.Value;
            if (LightConnectManage.CurrentLight.LightParamList[2].ChannelState)
                LightConnectManage.CurrentLight.SetLight(enLightChannel.Channel_3, this.通道3trackBar.Value);
        }

        private void 通道4trackBar_Scroll(object sender, EventArgs e)
        {
            if (LightConnectManage.CurrentLight == null) return;
            this.通道4值label.Text = this.通道4trackBar.Value.ToString();
            LightConnectManage.CurrentLight.LightParamList[3].LightValue = this.通道4trackBar.Value;
            if (LightConnectManage.CurrentLight.LightParamList[3].ChannelState)
                LightConnectManage.CurrentLight.SetLight(enLightChannel.Channel_4, this.通道4trackBar.Value);
        }

        private void LightControlForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                this.IsLoad = false;
                //UserLoginParamManager.Instance.LoginParam.UserChange -= new EventHandler(this.UserChange_Event);
                UserLoginParamManager.Instance.UserChange -= new EventHandler(this.UserChange_Event);
                ViewConfigParamManager.Instance.ViewParamList.Remove(this._viewConfigParam); // 关闭窗体时要删除相应的对象
            }
            catch
            {

            }
        }

        private void LightControlForm_Move(object sender, EventArgs e)
        {
            if (this._viewConfigParam != null)
                this._viewConfigParam.Location = this.Location;
        }

        private void LightControlForm_Resize(object sender, EventArgs e)
        {
            if (this._viewConfigParam != null && this.IsLoad) // 在窗体加载后这个事件才起作用
                this._viewConfigParam.FormSize = this.Size;
        }

        private void LightControlForm_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, WM_SYSCOMMAND, SC_MOVE + HTCAPTION, 0);
        }
        private void label5_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, WM_SYSCOMMAND, SC_MOVE + HTCAPTION, 0);
        }

        private void 控制器名称comboBox_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, WM_SYSCOMMAND, SC_MOVE + HTCAPTION, 0);
        }



        #region  窗体移动功能
        private const int WM_SYSCOMMAND = 0x0112;
        private const int SC_MOVE = 0xF010;
        private const int HTCAPTION = 0x0002;
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();
        [DllImport("user32.dll")]
        public static extern bool SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);

        #endregion

        #region  窗体绽放功能 
        private const int Guying_HTLEFT = 10;
        private const int Guying_HTRIGHT = 11;
        private const int Guying_HTTOP = 12;
        private const int Guying_HTTOPLEFT = 13;
        private const int Guying_HTTOPRIGHT = 14;
        private const int Guying_HTBOTTOM = 15;
        private const int Guying_HTBOTTOMLEFT = 0x10;
        private const int Guying_HTBOTTOMRIGHT = 17;
        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case 0x0084:
                    base.WndProc(ref m);
                    Point vPoint = new Point((int)m.LParam & 0xFFFF,
                        (int)m.LParam >> 16 & 0xFFFF);
                    vPoint = PointToClient(vPoint);
                    if (vPoint.X <= 5)
                        if (vPoint.Y <= 5)
                            m.Result = (IntPtr)Guying_HTTOPLEFT;
                        else if (vPoint.Y >= ClientSize.Height - 5)
                            m.Result = (IntPtr)Guying_HTBOTTOMLEFT;
                        else m.Result = (IntPtr)Guying_HTLEFT;
                    else if (vPoint.X >= ClientSize.Width - 5)
                        if (vPoint.Y <= 5)
                            m.Result = (IntPtr)Guying_HTTOPRIGHT;
                        else if (vPoint.Y >= ClientSize.Height - 5)
                            m.Result = (IntPtr)Guying_HTBOTTOMRIGHT;
                        else m.Result = (IntPtr)Guying_HTRIGHT;
                    else if (vPoint.Y <= 2)
                        m.Result = (IntPtr)Guying_HTTOP;
                    else if (vPoint.Y >= ClientSize.Height - 5)
                        m.Result = (IntPtr)Guying_HTBOTTOM;
                    break;
                default:
                    base.WndProc(ref m);
                    break;
            }
        }
        #endregion

        #region 防止改变窗口大小时控件闪烁功能
        protected override CreateParams CreateParams   //
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000; // Turn on WS_EX_COMPOSITED
                return cp;
            }
        }
        #endregion

        #region 窗体控制盒功能，关闭，最大化，最小化
        private void buttonClose_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("确定关闭窗体吗？", "关闭窗体", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                this.Close();  //关闭窗口
            }
        }
        private void buttonMax_Click(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Maximized)   //如果处于最大化，则还原
            {
                this.WindowState = FormWindowState.Normal;
            }
            else
            {
                this.WindowState = FormWindowState.Maximized;   //如果处于普通状态，则最大化
            }
        }
        private void buttonMin_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;  //最小化
        }
        #endregion


        #region 右键菜单项
        private void addContextMenu()
        {
            ContextMenuStrip ContextMenuStrip1 = new ContextMenuStrip();
            // 添加右键菜单 
            ToolStripItem[] items = new ToolStripMenuItem[]
            {
                 new ToolStripMenuItem("显示拖动区"),
                 new ToolStripMenuItem("隐藏拖动区"),
                 new ToolStripMenuItem("关闭窗体"),
            };
            ContextMenuStrip1.Items.AddRange(items); // 在滑赋值前不能调用 
            ContextMenuStrip1.ItemClicked += new ToolStripItemClickedEventHandler(label1ContextMenuStrip_ItemClicked);
            this.ContextMenuStrip = ContextMenuStrip1;
        }
        private void label1ContextMenuStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            string name = e.ClickedItem.Text;
            try
            {
                ((ContextMenuStrip)sender).Close();
                switch (name)
                {
                    default:
                    case "显示拖动区":
                        this.titleLabel.Show();
                        break;
                    case "隐藏拖动区":
                        this.titleLabel.Hide();
                        break;
                    case "关闭窗体":
                        this.Close();
                        break;
                        ///////////////////////////////////////////////                 
                }
            }
            catch
            {
            }
        }
        #endregion
        private void titleLabel_MouseDown(object sender, MouseEventArgs e)
        {
            LightControlForm_MouseDown(null, null);
        }

        private void titleLabel_MouseEnter(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Hand;
            this.titleLabel.BackColor = System.Drawing.Color.Orange;
        }

        private void titleLabel_MouseLeave(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Default;
            this.titleLabel.BackColor = System.Drawing.Color.LightGray;
        }


    }
}

using Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using MotionControlCard;
using Sensor;
using System.Runtime.InteropServices;

namespace FunctionBlock
{
    public partial class DisplayPositionForm : Form
    {
        IMotionControl i_McCard;
        AutoReSizeFormControls arfc = new AutoReSizeFormControls();
        CancellationTokenSource cts;
        private ViewConfigParam _viewConfigParam;
        private bool IsLoad = false; // 窗体加载后，设置为true


        public DisplayPositionForm(ViewConfigParam viewConfigParam)
        {
            InitializeComponent();
            this._viewConfigParam = viewConfigParam;
            this.titleLabel.Text = viewConfigParam.ViewName;
            this.i_McCard = MotionCardManage.CurrentCard;
            this.ContextMenu = new ContextMenu();
            //UserLoginParamManager.Instance.LoginParam.UserChange += new EventHandler(this.UserChange_Event);
            UserLoginParamManager.Instance.UserChange += new EventHandler(this.UserChange_Event);
        }
        public DisplayPositionForm()
        {
            InitializeComponent();
            this.i_McCard = MotionCardManage.CurrentCard;
        }

        /// <summary>
        /// 监控机台状态 
        /// </summary>
        public void GetCurrentPosition(CancellationToken ct)
        {
            try
            {
                while (true)
                {
                    ct.ThrowIfCancellationRequested();
                    double[] X_Scale = new double[4];
                    double[] Y_Scale = new double[4];
                    double[] Z_Scale = new double[4];
                    double[] U_Scale = new double[4];
                    double[] V_Scale = new double[4];
                    double[] W_Scale = new double[4];
                    /////////////////////////
                    if (MotionCardManage.CurrentCard == null) break;
                    MotionCardManage.CurrentCard.GetAxisPosition(MotionCardManage.CurrentCoordSys, enAxisName.X轴, out X_Scale[0]);
                    MotionCardManage.CurrentCard.GetAxisPosition(MotionCardManage.CurrentCoordSys, enAxisName.Y轴, out Y_Scale[0]);
                    MotionCardManage.CurrentCard.GetAxisPosition(MotionCardManage.CurrentCoordSys, enAxisName.Z轴, out Z_Scale[0]);
                    MotionCardManage.CurrentCard.GetAxisPosition(MotionCardManage.CurrentCoordSys, enAxisName.U轴, out U_Scale[0]);
                    MotionCardManage.CurrentCard.GetAxisPosition(MotionCardManage.CurrentCoordSys, enAxisName.V轴, out V_Scale[0]);
                    MotionCardManage.CurrentCard.GetAxisPosition(MotionCardManage.CurrentCoordSys, enAxisName.W轴, out W_Scale[0]);
                    //  跨线程调用对象
                    this.Invoke(new Action(() =>
                    {
                        X轴textBox.Text = X_Scale[0].ToString();
                        Y轴textBox.Text = Y_Scale[0].ToString();
                        Z轴textBox.Text = Z_Scale[0].ToString();
                        U轴textBox.Text = U_Scale[0].ToString();
                        V轴textBox.Text = V_Scale[0].ToString();
                        W轴textBox.Text = W_Scale[0].ToString();
                    }));
                    Thread.Sleep(50);
                }
            }
            catch
            {

            }
        }

        private void DisplayPositionForm_Load(object sender, EventArgs e)
        {
            cts = new CancellationTokenSource();
            Task.Run(() => GetCurrentPosition(cts.Token));
            if(this._viewConfigParam != null)
            {
                this.Location = this._viewConfigParam.Location;
                this.Size = this._viewConfigParam.FormSize;
            }
            this.IsLoad = true;
            //this.addContextMenu();
        }
        // 窗体关闭时处理
        private void MotionControl_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.IsLoad = false;
            //UserLoginParamManager.Instance.LoginParam.UserChange -= new EventHandler(this.UserChange_Event);
            UserLoginParamManager.Instance.UserChange -= new EventHandler(this.UserChange_Event);
            ViewConfigParamManager.Instance.ViewParamList.Remove(this._viewConfigParam); // 关闭窗体时要删除相应的对象
            if (MotionCardManage.CurrentCard != null)
                MotionCardManage.CurrentCard.UnInit();
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
        // 执行X轴回零
        private void xHomButton_Click(object sender, EventArgs e)
        {
            MotionCardManage.CurrentCard.SingleAxisHome(MotionCardManage.CurrentCoordSys, enAxisName.X轴, 10);
        }
        // 执行Y轴回零
        private void yHomButton_Click(object sender, EventArgs e)
        {
            MotionCardManage.CurrentCard.SingleAxisHome(MotionCardManage.CurrentCoordSys, enAxisName.Y轴, 10);
        }
        // 执行Z轴回零
        private void zHomButton_Click(object sender, EventArgs e)
        {
            MotionCardManage.CurrentCard.SingleAxisHome(MotionCardManage.CurrentCoordSys, enAxisName.Z轴, 10);
        }

        private void uHomButton_Click(object sender, EventArgs e)
        {
            MotionCardManage.CurrentCard.SingleAxisHome(MotionCardManage.CurrentCoordSys, enAxisName.U轴, 10);
        }

        private void vHomButton_Click(object sender, EventArgs e)
        {
            MotionCardManage.CurrentCard.SingleAxisHome(MotionCardManage.CurrentCoordSys, enAxisName.V轴, 10);
        }

        private void wHomButton_Click(object sender, EventArgs e)
        {
            MotionCardManage.CurrentCard.SingleAxisHome(MotionCardManage.CurrentCoordSys, enAxisName.W轴, 10);
        }

        private void DisplayPositionForm_Move(object sender, EventArgs e)
        {
            if (this._viewConfigParam != null)
                this._viewConfigParam.Location = this.Location;
        }

        private void DisplayPositionForm_Resize(object sender, EventArgs e)
        {
            if (this._viewConfigParam != null && this.IsLoad) // 在窗体加载后这个事件才起作用
                this._viewConfigParam.FormSize = this.Size;
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
            ReleaseCapture();
            SendMessage(this.Handle, WM_SYSCOMMAND, SC_MOVE + HTCAPTION, 0);
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

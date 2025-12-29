
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using FunctionBlock;
using Common;
using System.Runtime.InteropServices;
using MotionControlCard;

namespace FunctionBlock
{
    public partial class RectifyCompensateForm : Form
    {
        private bool IsLoad = false; // 窗体加载后，设置为true
        private RectifyCalculateParam _param;
        public ZoneRectifyParam _zoneParam;
        public RectifyCompensateForm()
        {
            InitializeComponent();
            ///// 构造时就初始化
        }

        public RectifyCompensateForm(RectifyCalculateParam param,ZoneRectifyParam zoneParam)
        {
            InitializeComponent();
            this._param = param;
            this._zoneParam = zoneParam;  
        }

        private void CompensateForm_Load(object sender, EventArgs e)
        {
            if (this._param != null)
            {
                this.视图窗口comboBox.DataSource = HWindowManage.GetKeysList();
                this.纠偏坐标系comboBox.DataSource = Enum.GetValues(typeof(enCoordSysName));
                this.纠偏坐标系comboBox.DataBindings.Add("Text", this._param, nameof(this._param.CoordSysName), true, DataSourceUpdateMode.OnPropertyChanged);
                this.目标角度XtextBox.DataBindings.Add("Text", this._param, nameof(this._param.Angle), true, DataSourceUpdateMode.OnPropertyChanged);
                this.模式comboBox.DataBindings.Add("Text", this._param, nameof(this._param.Mode), true, DataSourceUpdateMode.OnPropertyChanged);
                this.视图窗口comboBox.DataBindings.Add("Text", this._param, nameof(this._param.ViewWindow), true, DataSourceUpdateMode.OnPropertyChanged);
                this.启用分区纠偏checkBox.DataBindings.Add(nameof(this.启用分区纠偏checkBox.Checked), this._param, nameof(this._param.IsZoneCompensation), true, DataSourceUpdateMode.OnPropertyChanged);
            }
            else
            {
                new UserMessageForm().ShowDialog("RectifyCalculateParam 参数对象为NULL");
            }
            //// 读取补偿数据
            this.IsLoad = true;
        }



        private void CompensateForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                this.IsLoad = false;
                //ViewConfigParamManager.Instance.ViewParamList.Remove(this._viewConfigParam); // 关闭窗体时要删除相应的对象
            }
            catch
            {

            }
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
            //DialogResult dialogResult = new Common.UserMessageForm().ShowDialog("确定关闭窗体吗？", "关闭窗体");
            //if (dialogResult == DialogResult.OK)
            //{
            this.Close();  //关闭窗口
            //}
        }
        private void buttonMax_Click(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Maximized)   //如果处于最大化，则还原
            {
                //this.WindowState = FormWindowState.Normal;
            }
            else
            {
                //this.WindowState = FormWindowState.Maximized;   //如果处于普通状态，则最大化
            }
        }
        private void buttonMin_Click(object sender, EventArgs e)
        {
            //this.WindowState = FormWindowState.Minimized;  //最小化
        }

        #endregion



        private void CompensateForm_Move(object sender, EventArgs e)
        {

        }

        private void CompensateForm_Resize(object sender, EventArgs e)
        {

        }

        private void CompensateForm_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, WM_SYSCOMMAND, SC_MOVE + HTCAPTION, 0);
        }

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

        private void 分区纠偏设置Btn_Click(object sender, EventArgs e)
        {
            try
            {
                if (this._zoneParam == null)
                    this._zoneParam = new ZoneRectifyParam();
                new ZoneRectifyForm(this._zoneParam).ShowDialog();
            }
            catch (Exception ex)
            {
                new Common.UserMessageForm(ex.ToString()).ShowDialog();
            }
        }
    }
}

using MotionControlCard;
using Sensor;
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
using Common;

namespace FunctionBlock
{
    public partial class ProjectManagerFormNewOld2 : Form
    {
        private ViewConfigParam _viewConfigParam;
        private bool IsLoad = false; // 窗体加载后，设置为true
        private static ProjectManagerFormNewOld2 _instance;
        private static object lockState = new object();
        private DeviceCommunicationConfigForm _deviceForm;
        public static ProjectManagerFormNewOld2 Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (lockState)
                    {
                        _instance = new ProjectManagerFormNewOld2();
                    }
                }
                return _instance;
            }
        }
        private ProjectManagerFormNewOld2()
        {
            InitializeComponent();
            UserLoginParamManager.Instance.UserChange += new EventHandler(this.UserChange_Event);
        }

        public ProjectManagerFormNewOld2(ViewConfigParam viewConfigParam)
        {
            InitializeComponent();
            /////////////
            this._viewConfigParam = viewConfigParam;
            this.ContextMenu = new ContextMenu();
            //this.titleLabel.Text = viewConfigParam.ViewName;
            //this.titleLabel.TextAlign = ContentAlignment.MiddleLeft;
            ///////////////////////////////////////////////////////
            UserLoginParamManager.Instance.UserChange += new EventHandler(this.UserChange_Event);
        }
        private void ProjectManagerFormNew_Load(object sender, EventArgs e)
        {
            this.IsLoad = true;
            AddForm(this.坐标系TabPage, new CoordSysConfigParamManageForm()); //
            AddForm(this.标定参数tabPage, new CaliParaManagerForm());
            AddForm(this.夹抓tabPage, new RobotJawParaManagerForm(false));
            //if (!SystemParamManager.Instance.SysConfigParam.IsFormTopMost)
            AddForm(this.通信配置tabPage, new DeviceCommunicationConfigForm()); // new DeviceCommunicationConfigForm()
            this.DoubleBuffered = true;
            this.工程配置tabControl.DoubleBuffere(true);
            this.坐标系TabPage.DoubleBuffere(true);
            this.夹抓tabPage.DoubleBuffere(true);
            this.通信配置tabPage.DoubleBuffere(true);
            this.标定参数tabPage.DoubleBuffere(true);
        }
        public void UserChange_Event(object sender, EventArgs e)
        {
            try
            {
                UserLoginParam loginParam = sender as UserLoginParam;
                switch (loginParam.User)
                {
                    case enUserName.操作员:
                        foreach (Control item in this.坐标系TabPage.Controls)
                        {
                            CoordSysConfigParamManageForm form = item as CoordSysConfigParamManageForm;
                            form.UserEnable = false;
                        }
                        foreach (Control item in this.标定参数tabPage.Controls)
                        {
                            CaliParaManagerForm form = item as CaliParaManagerForm;
                            form.UserEnable = false;
                        }
                        foreach (Control item in this.夹抓tabPage.Controls)
                        {
                            RobotJawParaManagerForm form = item as RobotJawParaManagerForm;
                            form.UserEnable = true;
                        }
                        foreach (Control item in this.通信配置tabPage.Controls)
                        {
                            DeviceCommunicationConfigForm form = item as DeviceCommunicationConfigForm;
                            form.UserEnable = false;
                        }
                        break;
                    case enUserName.工程师:
                        foreach (Control item in this.坐标系TabPage.Controls)
                        {
                            CoordSysConfigParamManageForm form = item as CoordSysConfigParamManageForm;
                            form.UserEnable = false;
                        }
                        foreach (Control item in this.标定参数tabPage.Controls)
                        {
                            CaliParaManagerForm form = item as CaliParaManagerForm;
                            form.UserEnable = true;
                        }
                        foreach (Control item in this.夹抓tabPage.Controls)
                        {
                            RobotJawParaManagerForm form = item as RobotJawParaManagerForm;
                            form.UserEnable = true;
                        }
                        foreach (Control item in this.通信配置tabPage.Controls)
                        {
                            DeviceCommunicationConfigForm form = item as DeviceCommunicationConfigForm;
                            form.UserEnable = true;
                        }
                        break;
                    case enUserName.开发人员:
                        foreach (Control item in this.坐标系TabPage.Controls)
                        {
                            CoordSysConfigParamManageForm form = item as CoordSysConfigParamManageForm;
                            form.UserEnable = true;
                        }
                        foreach (Control item in this.标定参数tabPage.Controls)
                        {
                            CaliParaManagerForm form = item as CaliParaManagerForm;
                            form.UserEnable = true;
                        }
                        foreach (Control item in this.夹抓tabPage.Controls)
                        {
                            RobotJawParaManagerForm form = item as RobotJawParaManagerForm;
                            form.UserEnable = true;
                        }
                        foreach (Control item in this.通信配置tabPage.Controls)
                        {
                            DeviceCommunicationConfigForm form = item as DeviceCommunicationConfigForm;
                            form.UserEnable = true;
                        }
                        break;
                }
            }
            catch
            {
            }
        }

        public Form AddForm(Panel MastPanel, Form form)
        {
            if (MastPanel.Controls.Count > 0)
                MastPanel.Controls.Clear();
            form.TopLevel = false;
            //form.TopMost = true;
            form.FormBorderStyle = FormBorderStyle.None;
            form.MaximizeBox = false;
            form.MinimizeBox = false;
            form.Dock = DockStyle.Fill;
            form.Padding = new Padding(0);
            MastPanel.Margin = new Padding(0);
            MastPanel.Controls.Add(form);
            //this.ListForm.Add(form);
            form.Show();
            return form;
        }
        public Form AddForm(TabPage MastPanel, Form form)
        {
            if (MastPanel.Controls.Count > 0)
                MastPanel.Controls.Clear();
            form.TopLevel = false;
            //form.TopMost = true;
            form.FormBorderStyle = FormBorderStyle.None;
            form.MaximizeBox = false;
            form.MinimizeBox = false;
            form.Dock = DockStyle.Fill;
            form.Padding = new Padding(0);
            MastPanel.Margin = new Padding(0);
            MastPanel.Controls.Add(form);
            //this.ListForm.Add(form);
            form.Show();
            return form;
        }
        public void AddForm(TableLayoutPanel MastPanel, Form form, int rowPose, int colPose, int rowSpan, int colSpan)
        {
            form.TopLevel = false;
            form.FormBorderStyle = FormBorderStyle.None;
            form.MaximizeBox = false;
            form.MinimizeBox = false;
            form.Dock = DockStyle.Fill;
            form.Padding = new Padding(0);
            /////////////////////////////
            MastPanel.Margin = new Padding(0);
            MastPanel.Controls.Add(form);
            MastPanel.SetRow(form, rowPose);
            MastPanel.SetColumn(form, colPose);
            MastPanel.SetRowSpan(form, rowSpan);
            MastPanel.SetColumnSpan(form, colSpan);
            //this.ListForm.Add(form);
            form.Show();
        }

        private void ReportQueryForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            UserLoginParamManager.Instance.UserChange -= new EventHandler(this.UserChange_Event);
        }

        private void ReportQueryForm_Move(object sender, EventArgs e)
        {
            if (this._viewConfigParam != null)
                this._viewConfigParam.Location = this.Location;
        }

        private void ReportQueryForm_Resize(object sender, EventArgs e)
        {
            this.IsLoad = false;
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
            DialogResult dialogResult = new Common.UserMessageForm().ShowDialog("确定关闭窗体吗？", "关闭窗体");
            if (dialogResult == DialogResult.OK)
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

        }
        #endregion

        #region Lable右键菜单项
        private void addContextMenu()
        {
            ContextMenuStrip ContextMenuStrip1 = new ContextMenuStrip();
            // 添加右键菜单 
            ToolStripItem[] items = new ToolStripMenuItem[]
            {
                new ToolStripMenuItem("重命名"),
            };
            ContextMenuStrip1.Items.AddRange(items); // 在滑赋值前不能调用 
            ContextMenuStrip1.ItemClicked += new ToolStripItemClickedEventHandler(label1ContextMenuStrip_ItemClicked);
            //this.SignLabel.ContextMenuStrip = ContextMenuStrip1;
            //this.titleLabel.ContextMenuStrip = ContextMenuStrip1;
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
                    case "重命名":
                        RenameForm renameForm = new RenameForm("");
                        DialogResult dialogResult = renameForm.ShowDialog();
                        if (dialogResult == DialogResult.OK)
                        {
                            this._viewConfigParam.Tag = renameForm.ReName;
                            this._viewConfigParam.Text = renameForm.ReName;
                            this._viewConfigParam.ViewName = renameForm.ReName;
                            //this.SignLabel.Text = this._viewConfigParam.Tag;
                            //this.titleLabel.Text = this._viewConfigParam.Tag;
                        }
                        renameForm.Dispose();
                        break;
                        ///////////////////////////////////////////////                 
                }
            }
            catch
            {
            }
        }

        #endregion

        private void ReportQueryForm_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, WM_SYSCOMMAND, SC_MOVE + HTCAPTION, 0);
        }

        private void titleLabel_MouseDown(object sender, MouseEventArgs e)
        {
            ReportQueryForm_MouseDown(null, null);
        }

        private void titleLabel_MouseEnter(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Hand;
            //this.titleLabel.BackColor = System.Drawing.Color.Orange;// 
        }

        private void titleLabel_MouseLeave(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Default;
            //this.titleLabel.BackColor = System.Drawing.Color.LightGray;// 
        }

        private void 工程配置tabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                return;
                if (!SystemParamManager.Instance.SysConfigParam.IsFormTopMost) return;
                switch (工程配置tabControl.SelectedTab.Name)
                {
                    default:
                    case nameof(坐标系TabPage):
                        //AddForm(this.坐标系TabPage, new CoordSysConfigParamManageForm()); //
                        this._deviceForm?.Close();
                        this._deviceForm = null;
                        break;
                    case nameof(标定参数tabPage):
                        //AddForm(this.标定参数tabPage, new CaliParaManagerForm());
                        this._deviceForm?.Close();
                        this._deviceForm = null;
                        break;
                    case nameof(夹抓tabPage):
                        //AddForm(this.夹抓tabPage, new RobotJawParaManagerForm(false));
                        this._deviceForm?.Close();
                        this._deviceForm = null;
                        break;
                    case nameof(通信配置tabPage):
                        //AddForm(this.通信配置tabPage, new DeviceCommunicationConfigForm());
                        this._deviceForm = new DeviceCommunicationConfigForm();
                        //this._deviceForm.FormBorderStyle = FormBorderStyle.None;
                        //this._deviceForm.MaximizeBox = false;
                        //this._deviceForm.MinimizeBox = false;
                        //this._deviceForm.Dock = DockStyle.Fill;
                        this._deviceForm.TopMost = true;
                        this.ShowInTaskbar = false;
                        this._deviceForm.Padding = new Padding(0);
                        this._deviceForm.StartPosition = FormStartPosition.Manual;
                        Point point = this.工程配置tabControl.SelectedTab.PointToScreen(this.工程配置tabControl.Location);
                        this._deviceForm.Location = point;
                        this.Width = this.工程配置tabControl.SelectedTab.Width;
                        this.Height = this.工程配置tabControl.SelectedTab.Height;
                        this._deviceForm.Show();
                        break;
                }
            }
            catch (Exception ex)
            {
                new UserMessageForm(ex.ToString()).ShowDialog();
            }
        }


    }
}

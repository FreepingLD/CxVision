
using Common;
using FunctionBlock;
using HalconDotNet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;


namespace FunctionBlock
{
    public partial class LogViewForm : Form
    {
        private ViewConfigParam _viewConfigParam;
        private bool IsLoad = false; // 窗体加载后，设置为true
        private string keyName = "";
        public LogViewForm(ViewConfigParam viewConfigParam)
        {
            InitializeComponent();
            this._viewConfigParam = viewConfigParam;
            this.ContextMenu = new ContextMenu();
            UserLoginParamManager.Instance.UserChange += new EventHandler(this.UserChange_Event);
            this.keyName = viewConfigParam.CamName.Replace("采集源(", "").Replace(")", "");
            this.传感器comboBox1.Text = this.keyName;
            //this.titleLabel.Text = this.keyName + ":日志视图";
            //if (LoggerHelper.Logger.MyListView == null) LoggerHelper.Logger.MyListView = new Dictionary<string, ListView>();
            //if (LoggerHelper.Logger.MyListView.ContainsKey(this.keyName))
            //    LoggerHelper.Logger.MyListView[this.keyName] = this.listView1;
            //else
            //    LoggerHelper.Logger.MyListView.Add(this.keyName, this.listView1);
            //LoggerHelper.Logger.IsListView = true;
        }
        public LogViewForm(string name)
        {
            InitializeComponent();
            this.keyName = name;
            this.传感器comboBox1.Text = this.keyName;
            //this.titleLabel.Text = this.keyName + ":日志视图";
            this.ContextMenu = new ContextMenu();
            UserLoginParamManager.Instance.UserChange += new EventHandler(this.UserChange_Event);
            //LoggerHelper.Logger.MyListView = this.listView1;
            //if (LoggerHelper.Logger.MyListView == null) LoggerHelper.Logger.MyListView = new Dictionary<string, ListView>();
            //if (LoggerHelper.Logger.MyListView.ContainsKey(this.keyName))
            //    LoggerHelper.Logger.MyListView[this.keyName] = this.listView1;
            //else
            //    LoggerHelper.Logger.MyListView.Add(this.keyName, this.listView1);
            //LoggerHelper.Logger.IsListView = true;
        }

        public LogViewForm()
        {
            InitializeComponent();
            /////////////
            this.Padding = new Padding(0);
        }

        private void LogViewForm_Load(object sender, EventArgs e)
        {
            this._viewConfigParam = this._viewConfigParam == null ? new ViewConfigParam() : this._viewConfigParam;
            this.Location = this._viewConfigParam.Location;
            this.Size = this._viewConfigParam.FormSize;
            this.IsLoad = true;
            this.listView1.DoubleBuffere(true);
            this.DoubleBuffered = true;
            this.addContextMenu();
            ////////////////////////////////
            this.传感器comboBox1.Items.Clear();
            this.传感器comboBox1.Items.Add("NONE");
            this.传感器comboBox1.Items.Add("All");
            this.传感器comboBox1.Items.AddRange(Sensor.SensorManage.GetSensorName());
            for (int i = 0; i < this.传感器comboBox1.Items.Count; i++)
            {
                if(this._viewConfigParam.CamName == this.传感器comboBox1.Items[i].ToString())
                {
                    this.传感器comboBox1.SelectedIndex = i;
                }   
            }
            /////////////////////////////////////////
            switch (this._viewConfigParam.CamName)
            {
                case "NONE":
                    LoggerHelper.Logger.MyListView?.Clear();
                    if (LoggerHelper.Logger.MyListView == null)
                        LoggerHelper.Logger.MyListView = new Dictionary<string, ListView>();
                    ////////////////////////////////////////////////////////////////////////
                    LoggerHelper.Logger.MyListView.Add("NONE", this.listView1);
                    LoggerHelper.Logger.IsListView = true;
                    break;
                case "All":
                    LoggerHelper.Logger.MyListView?.Clear();
                    if (LoggerHelper.Logger.MyListView == null)
                        LoggerHelper.Logger.MyListView = new Dictionary<string, ListView>();
                    ////////////////////////////////////////////////////////////////////////
                    LoggerHelper.Logger.MyListView.Add("NONE", this.listView1);
                    foreach (var item in Sensor.SensorManage.GetSensorName())
                    {
                        if (LoggerHelper.Logger.MyListView.ContainsKey(item))
                            LoggerHelper.Logger.MyListView[item] = this.listView1;
                        else
                            LoggerHelper.Logger.MyListView.Add(item, this.listView1);
                    }
                    LoggerHelper.Logger.IsListView = true;
                    break;
                default:
                    LoggerHelper.Logger.MyListView?.Clear();
                    if (LoggerHelper.Logger.MyListView == null)
                        LoggerHelper.Logger.MyListView = new Dictionary<string, ListView>();
                    ////////////////////////////////////////////////////////////////////////
                    if (LoggerHelper.Logger.MyListView.ContainsKey(this.keyName))
                        LoggerHelper.Logger.MyListView[this.keyName] = this.listView1;
                    else
                        LoggerHelper.Logger.MyListView.Add(this.keyName, this.listView1);
                    LoggerHelper.Logger.IsListView = true;
                    break;
            }
        }

        public void AddForm(Panel MastPanel, Form form)
        {
            if (MastPanel.Controls.Count > 0)
                MastPanel.Controls.Clear();
            form.TopLevel = false;
            form.FormBorderStyle = FormBorderStyle.None;
            form.MaximizeBox = false;
            form.MinimizeBox = false;
            form.Dock = DockStyle.Fill;
            form.Padding = new Padding(0);
            MastPanel.Margin = new Padding(1);
            MastPanel.Controls.Add(form);
            form.Show();
        }

        public void AddForm(TabPage MastPanel, Form form)
        {
            if (MastPanel.Controls.Count > 0)
                MastPanel.Controls.Clear();
            form.TopLevel = false;
            form.FormBorderStyle = FormBorderStyle.None;
            form.MaximizeBox = false;
            form.MinimizeBox = false;
            form.Dock = DockStyle.Fill;
            form.Padding = new Padding(0);
            MastPanel.Margin = new Padding(1);
            MastPanel.Controls.Add(form);
            form.Show();
        }

        public void AddForm(TableLayoutPanel MastPanel, Form form, int rowPose, int colPose, int rowSpan, int colSpan)
        {
            form.TopLevel = false;
            form.FormBorderStyle = FormBorderStyle.None;
            form.MaximizeBox = false;
            form.MinimizeBox = false;
            form.Dock = DockStyle.Fill;
            form.Padding = new Padding(0);
            MastPanel.Margin = new Padding(1);
            MastPanel.Controls.Add(form);
            MastPanel.SetRow(form, rowPose);
            MastPanel.SetColumn(form, colPose);
            MastPanel.SetRowSpan(form, rowSpan);
            MastPanel.SetColumnSpan(form, colSpan);
            form.Show();
        }
        public void UserChange_Event(object sender, EventArgs e)
        {
            try
            {
                UserLoginParam loginParam = sender as UserLoginParam;
                switch (loginParam.User)
                {
                    case enUserName.操作员:
                        if (this._viewConfigParam != null)
                            this.buttonClose.Enabled = false;
                        else
                            this.buttonClose.Enabled = true;
                        ////////////////////////////////////////////////////
                        this.buttonMax.Hide();
                        this.buttonMin.Hide();
                        this.buttonClose.Hide();
                        this.tableLayoutPanel1.SetColumnSpan(this.titleLabel, 4);
                        break;
                    case enUserName.工程师:
                        this.buttonClose.Enabled = true;
                        ////////////////////////////////////////////////////
                        this.buttonMax.Hide();
                        this.buttonMin.Hide();
                        this.buttonClose.Hide();
                        this.tableLayoutPanel1.SetColumnSpan(this.titleLabel, 4);
                        break;
                    case enUserName.开发人员:
                        this.buttonClose.Enabled = true;
                        ////////////////////////////////////////////////////
                        this.buttonMax.Show();
                        this.buttonMin.Show();
                        this.buttonClose.Show();
                        this.tableLayoutPanel1.SetColumnSpan(this.titleLabel, 1);
                        break;
                }
            }
            catch
            {
            }
        }

        private void ElementViewForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                this.IsLoad = false;
                if (LoggerHelper.Logger.MyListView.ContainsKey(this.keyName))
                    LoggerHelper.Logger.MyListView.Remove(this.keyName);
                LoggerHelper.Logger.MyListView?.Clear();
                //LoggerHelper.Logger.IsListView = false;
                UserLoginParamManager.Instance.UserChange -= new EventHandler(this.UserChange_Event);
                //ViewConfigParamManager.Instance.ViewParamList.Remove(this._viewConfigParam); // 关闭窗体时要删除相应的对象
            }
            catch
            {

            }
        }
        private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false;
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
                    if (UserLoginParamManager.Instance.CurrentUser == enUserName.操作员) return;
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
            if (this._viewConfigParam != null)
            {
                DialogResult dialogResult = new Common.UserMessageForm().ShowDialog("确定关闭窗体吗？", "关闭窗体");
                if (dialogResult == DialogResult.OK)
                {
                    ViewConfigParamManager.Instance.ViewParamList.Remove(this._viewConfigParam);
                    this.Close();  //关闭窗口
                }
            }
            else
                this.Close();  //关闭窗口
        }
        private void buttonMax_Click(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Maximized)   //如果处于最大化，则还原
            {
                this.WindowState = FormWindowState.Normal;
            }
            else
            {
                //Point point = this.Parent.Location;
                this._viewConfigParam.Location = this.Parent.Location;
                this._viewConfigParam.FormSize = this.Parent.Size;
                this.WindowState = FormWindowState.Maximized;   //如果处于普通状态，则最大化
            }
        }
        private void buttonMin_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;  //最小化
        }


        #endregion

        private void ElementViewForm_Move(object sender, EventArgs e)
        {
            if (this._viewConfigParam != null)
                this._viewConfigParam.Location = this.Location;
        }

        private void ElementViewForm_Resize(object sender, EventArgs e)
        {
            if (this._viewConfigParam != null && this.IsLoad) // 在窗体加载后这个事件才起作用
                this._viewConfigParam.FormSize = this.Size;
        }

        private void ElementViewForm_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, WM_SYSCOMMAND, SC_MOVE + HTCAPTION, 0);
        }


        #region 右键菜单项
        private void addContextMenu()
        {
            ContextMenuStrip ContextMenuStrip1 = new ContextMenuStrip();
            // 添加右键菜单 
            ToolStripItem[] items = new ToolStripMenuItem[]
            {
                 new ToolStripMenuItem("详细信息"),
                 new ToolStripMenuItem("清空信息"),
                 //new ToolStripMenuItem("隐藏拖动区"),
                 //new ToolStripMenuItem("关闭窗体"),
            };
            ContextMenuStrip1.Items.AddRange(items); // 在滑赋值前不能调用 
            ContextMenuStrip1.ItemClicked += new ToolStripItemClickedEventHandler(label1ContextMenuStrip_ItemClicked);
            this.listView1.ContextMenuStrip = ContextMenuStrip1;
        }
        private void label1ContextMenuStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            string name = e.ClickedItem.Text;
            try
            {
                ((ContextMenuStrip)sender).Close();
                switch (name)
                {
                    case "详细信息":
                        new ContentForm(this.listView1.SelectedItems[0].SubItems[1].Text).Show();
                        break;
                    case "清空信息":
                        this.listView1?.Items.Clear();
                        break;
                    default:
                        break;
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
            catch (Exception ex)
            {
                new Common.UserMessageForm(ex.ToString()).ShowDialog(); ;
            }
        }

        #endregion

        private void 拖动label_MouseEnter(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Hand;
            this.titleLabel.BackColor = System.Drawing.Color.Orange;
        }

        private void 拖动label_MouseLeave(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Default;
            this.titleLabel.BackColor = System.Drawing.Color.LightGray;
        }

        private void 拖动label_MouseDown(object sender, MouseEventArgs e)
        {
            ElementViewForm_MouseDown(null, null);
        }

        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                ListView listView = sender as ListView;
                ContentForm form = new ContentForm(listView.SelectedItems[0].SubItems[1].Text);//.Show();
                form.TopMost = true;
                form.Show();
            }
            catch (Exception ex)
            {
                new Common.UserMessageForm(ex.ToString()).ShowDialog(); ;
            }
        }

        private void buttonClose_Click_1(object sender, EventArgs e)
        {
            DialogResult dialogResult = new UserMessageForm("确定关闭窗体吗？", "关闭窗体").ShowDialog();
            if (dialogResult == DialogResult.OK)
            {
                
                this.Close();  //关闭窗口
            }
        }

        private void buttonMax_Click_1(object sender, EventArgs e)
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

        private void buttonMin_Click_1(object sender, EventArgs e)
        {

        }

        private void 传感器comboBox1_SelectionChangeCommitted(object sender, EventArgs e)
        {
            try
            {
                if (this.传感器comboBox1.Text == "" || this.传感器comboBox1.Text == null) return;
                ///////////////////////////////////////////////////
                this.keyName = this.传感器comboBox1.Text;
                this._viewConfigParam.CamName = this.keyName;
                //this.titleLabel.Text = this.keyName + ":日志视图";
                switch (this.传感器comboBox1.Text)
                {
                    case "NONE": 
                        LoggerHelper.Logger.MyListView?.Clear();
                        if (LoggerHelper.Logger.MyListView == null)
                            LoggerHelper.Logger.MyListView = new Dictionary<string, ListView>();
                        ////////////////////////////////////////////////////////////////////////
                        LoggerHelper.Logger.MyListView.Add("NONE", this.listView1);
                        LoggerHelper.Logger.IsListView = true;
                        break;
                    case "All": 
                        LoggerHelper.Logger.MyListView?.Clear();
                        if (LoggerHelper.Logger.MyListView == null)
                            LoggerHelper.Logger.MyListView = new Dictionary<string, ListView>();
                        ////////////////////////////////////////////////////////////////////////
                        LoggerHelper.Logger.MyListView.Add("NONE", this.listView1);
                        foreach (var item in Sensor.SensorManage.GetSensorName())
                        {
                            if (LoggerHelper.Logger.MyListView.ContainsKey(item))
                                LoggerHelper.Logger.MyListView[item] = this.listView1;
                            else
                                LoggerHelper.Logger.MyListView.Add(item, this.listView1);
                        }
                        LoggerHelper.Logger.IsListView = true;
                        break;
                    default:
                        LoggerHelper.Logger.MyListView?.Clear();
                        if (LoggerHelper.Logger.MyListView == null)
                            LoggerHelper.Logger.MyListView = new Dictionary<string, ListView>();
                        ////////////////////////////////////////////////////////////////////////
                        if (LoggerHelper.Logger.MyListView.ContainsKey(this.keyName))
                            LoggerHelper.Logger.MyListView[this.keyName] = this.listView1;
                        else
                            LoggerHelper.Logger.MyListView.Add(this.keyName, this.listView1);
                        LoggerHelper.Logger.IsListView = true;
                        break;
                }
                ////////////////////////////////
                ViewConfigParamManager.Instance.Save();
            }
            catch (Exception ex)
            {
                new UserMessageForm().ShowDialog(ex.ToString());
            }
        }
    }
}

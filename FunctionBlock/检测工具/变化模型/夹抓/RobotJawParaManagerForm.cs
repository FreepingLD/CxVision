using Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Sensor;
using System.Runtime.InteropServices;

namespace FunctionBlock
{
    public partial class RobotJawParaManagerForm : Form
    {
        public bool _userEnable;
        public bool UserEnable
        {
            get
            {
                return _userEnable;
            }
            set
            {
                this._userEnable = value;
                if (this._userEnable)
                {
                    this.dataGridView1.ReadOnly = false;
                    this.夹抓label.Enabled = true;
                }
                else
                {
                    this.dataGridView1.ReadOnly = true;
                    this.夹抓label.Enabled = false;
                }
            }
        }

        private ViewConfigParam _viewConfigParam;
        private bool IsLoad = false; // 窗体加载后，设置为true
        // 相机标定参数是跟相机的数量挂钩的
        public RobotJawParaManagerForm(bool isShowTitle = true)
        {
            InitializeComponent();
            if (!isShowTitle)
            {
                this.titleLabel.Hide();
                this.buttonMin.Hide();
                this.buttonMax.Hide();
                this.buttonClose.Hide();
                this.Text = "";
            }
        }

        public RobotJawParaManagerForm(ViewConfigParam viewConfigParam)
        {
            InitializeComponent();
            this._viewConfigParam = viewConfigParam;
            //this.titleLabel.Text = "夹抓&胶枪参数";
        }

        private void RobotJawParaManagerForm_Load(object sender, EventArgs e)
        {
            //////////////////////////////////////////////
            this.JawNameColumn.ValueType = typeof(enRobotJawEnum);
            this.JawNameColumn.Items.Clear();
            foreach (enRobotJawEnum temp in Enum.GetValues(typeof(enRobotJawEnum)))
                this.JawNameColumn.Items.Add(temp);
            //this.坐标系Col.ValueType = typeof(enCoordSysName);
            //this.坐标系Col.Items.Clear();
            //foreach (enCoordSysName temp in Enum.GetValues(typeof(enCoordSysName)))
            //    this.坐标系Col.Items.Add(temp);
            /////////////////////////////////////////////////////////
            if (RobotJawParaManager.Instance.RobotJawParaItems.Count == 0)
                RobotJawParaManager.Instance.Read();
            this.dataGridView1.DataSource = RobotJawParaManager.Instance.RobotJawParaItems;
            // 用于动态添加窗体
            this._viewConfigParam = this._viewConfigParam == null ? new ViewConfigParam() : this._viewConfigParam;
            this.Location = this._viewConfigParam.Location;
            this.Size = this._viewConfigParam.FormSize;
            this.IsLoad = true;
            this.dataGridView1.DoubleBuffere(true);
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    switch (dataGridView1.Columns[e.ColumnIndex].Name)
                    {
                        case "SaveBtn":
                            RobotJawParaManager.Instance.Save(); // [e.RowIndex].Save() ;
                            break;
                        case "InsertBtn":
                            RobotJawParaManager.Instance.RobotJawParaItems.Insert(e.RowIndex, new RobotJawParam());
                            break;
                        case "DeletBtn":
                            RobotJawParaManager.Instance.RobotJawParaItems.RemoveAt(e.RowIndex);
                            break;
                    }
                    this.dataGridView1.Refresh();
                }
            }
            catch
            {
            }
        }

        private void RobotJawParaManagerForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                //DialogResult dialogResult = new UserMessageForm().ShowDialog("确定要退也程序吗？", "退出程序");
                //if (dialogResult == DialogResult.OK)
                //{
                this.IsLoad = false;
                //ViewConfigParamManager.Instance.ViewParamList.Remove(this._viewConfigParam); // 关闭窗体时要删除相应的对象
                //}
                //else
                //    e.Cancel = true;
            }
            catch
            {

            }
        }

        private void RobotJawParaManagerForm_Move(object sender, EventArgs e)
        {
            if (this._viewConfigParam != null)
                this._viewConfigParam.Location = this.Location;
        }

        private void RobotJawParaManagerForm_Resize(object sender, EventArgs e)
        {
            if (this._viewConfigParam != null && this.IsLoad) // 在窗体加载后这个事件才起作用
                this._viewConfigParam.FormSize = this.Size;
        }

        private void RobotJawParaManagerForm_MouseDown(object sender, MouseEventArgs e)
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
            DialogResult dialogResult = new Common.UserMessageForm().ShowDialog("确定关闭窗体吗？", "关闭窗体");
            if (dialogResult == DialogResult.OK)
            {
                ViewConfigParamManager.Instance.ViewParamList.Remove(this._viewConfigParam);
                this.Close();  //关闭窗口
            }
        }
        private void buttonMax_Click(object sender, EventArgs e)
        {

        }
        private void buttonMin_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;  //最小化
        }

        #endregion

        private void 拖动label_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, WM_SYSCOMMAND, SC_MOVE + HTCAPTION, 0);
            this.titleLabel.BackColor = System.Drawing.Color.Orange;
        }

        private void 拖动label_MouseEnter(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Hand;
            this.titleLabel.BackColor = System.Drawing.Color.Orange;  //System.Drawing.SystemColors.HotTrack;
        }

        private void 拖动label_MouseLeave(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Default;
            this.titleLabel.BackColor = System.Drawing.Color.LightGray;
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
            this.WindowState = FormWindowState.Minimized;  //最小化
        }

        private void buttonClose_Click_1(object sender, EventArgs e)
        {
            DialogResult dialogResult = new UserMessageForm().ShowDialog("确定关闭窗体吗？", "关闭窗体");
            if (dialogResult == DialogResult.OK)
            {
                this.Close();  //关闭窗口
            }
        }


    }
}

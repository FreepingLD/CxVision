
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
    public partial class SizeControlForm : Form
    {
        private ViewConfigParam _viewConfigParam;
        private bool IsLoad = false; // 窗体加载后，设置为true
        private BindingList<CompensationResultInfo> _compensaData;
        public SizeControlForm(BindingList<MeasureResultInfo> list)
        {
            InitializeComponent();
            ///// 构造时就初始化
            //this.AddXyThetaFrm_Paint();
            //this.InitDataGridView(this.dataGridView1);
            if (SystemParamManager.Instance.SysConfigParam.IsFormTopMost)
            {
                this.TopMost = true;
                this.ShowInTaskbar = true;
            }
            this.dataGridView1.DataSource = list;
        }
        public SizeControlForm(BindingList<OcrResultInfo> list)
        {
            InitializeComponent();
            if (SystemParamManager.Instance.SysConfigParam.IsFormTopMost)
            {
                this.TopMost = true;
                this.ShowInTaskbar = true;
            }
            ///// 构造时就初始化
            //this.AddXyThetaFrm_Paint();
            //this.InitDataGridView(this.dataGridView1);
            this.dataGridView1.DataSource = list;
        }

        public SizeControlForm(BindingList<MeasureResultInfo> list1, BindingList<OcrResultInfo> list2)
        {
            InitializeComponent();
            if (SystemParamManager.Instance.SysConfigParam.IsFormTopMost)
            {
                this.TopMost = true;
                this.ShowInTaskbar = true;
            }
            ///// 构造时就初始化
            //this.AddXyThetaFrm_Paint();
            //this.InitDataGridView(this.dataGridView1);
            this.dataGridView1.DataSource = list1;
            this.dataGridView2.DataSource = list2;
        }

        public SizeControlForm(BindingList<MeasureResultInfo> list, ViewConfigParam viewConfigParam)
        {
            InitializeComponent();
            if (SystemParamManager.Instance.SysConfigParam.IsFormTopMost)
            {
                this.TopMost = true;
                this.ShowInTaskbar = true;
            }
            this._viewConfigParam = viewConfigParam;
            if (viewConfigParam != null)
                this.titleLabel.Text = viewConfigParam?.ViewName;
            ///// 构造时就初始化
            //this.AddXyThetaFrm_Paint();
            //this.InitDataGridView(this.dataGridView1);
            this.dataGridView1.DataSource = list;
        }

        private void AlignDataForm_Load(object sender, EventArgs e)
        {
            if (this._viewConfigParam != null)
            {
                this.Location = this._viewConfigParam.Location;
                this.Size = this._viewConfigParam.FormSize;
            }
            this.IsLoad = true;
            //// 读取补偿数据
            if (this._viewConfigParam != null)
                this._compensaData = CompensationDataManager.Instance.Read(this._viewConfigParam.ViewName.Replace(":[", "_").Replace("]", ""));
        }

        private void InitDataGridView(DataGridView dataGridView)
        {
            dataGridView.AllowUserToAddRows = false;
            dataGridView.RowHeadersWidth = 5;
            dataGridView.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView.RowHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView.TopLeftHeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView.Margin = new Padding(0);
        }

        private void SizeControlForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                this.IsLoad = false;
                // ViewConfigParamManager.Instance.ViewParamList.Remove(this._viewConfigParam); // 关闭窗体时要删除相应的对象
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

        private void AddXyThetaFrm_Paint()
        {
            //Graphics g = this.groupBox2.CreateGraphics(); // e.Graphics;
            //g.DrawLine(new Pen(System.Drawing.Color.Red), 50, 220, 300, 220);
            //g.DrawLine(new Pen(System.Drawing.Color.Red), 280, 210, 300, 220);
            //g.DrawLine(new Pen(System.Drawing.Color.Red), 280, 230, 300, 220);

            //g.DrawLine(new Pen(System.Drawing.Color.Red), 70, 240, 70, 50);

            //g.DrawLine(new Pen(System.Drawing.Color.Red), 60, 70, 70, 50);
            //g.DrawLine(new Pen(System.Drawing.Color.Red), 80, 70, 70, 50);
            //g.DrawArc(new Pen(System.Drawing.Color.Red), 20, 170, 100, 100, 0, -90);

            //g.DrawLine(new Pen(System.Drawing.Color.Red), 70 + 35, 220 - 35, 70 + 35, 220 - 35 + 10);
            //g.DrawLine(new Pen(System.Drawing.Color.Red), 70 + 35, 220 - 35, 70 + 35 + 10, 220 - 35);
            //g.DrawString("X轴", this.Font, new SolidBrush(Color.Black), 150, 230);
            //g.DrawString("Y轴", this.Font, new SolidBrush(Color.Black), 40, 130);

            //g.DrawString("说明：补偿的坐标系为笛卡尔坐标系，X向右为正，Y向上为正，角度逆时针为正。",
            //    this.Font, new SolidBrush(Color.Red), 60, 260);
        }

        private void AlignDataForm_Move(object sender, EventArgs e)
        {
            if (this._viewConfigParam != null)
                this._viewConfigParam.Location = this.Location;
        }

        private void AlignDataForm_Resize(object sender, EventArgs e)
        {
            if (this._viewConfigParam != null && this.IsLoad) // 在窗体加载后这个事件才起作用
                this._viewConfigParam.FormSize = this.Size;
        }

        private void AlignDataForm_MouseDown(object sender, MouseEventArgs e)
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

        private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false;
        }

        private void dataGridView2_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false;
        }


    }
}

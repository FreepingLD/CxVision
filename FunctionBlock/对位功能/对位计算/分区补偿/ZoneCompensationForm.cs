
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
    public partial class ZoneCompensationForm : Form
    {
        private bool IsLoad = false; // 窗体加载后，设置为true
        //private CompensationData _param;
        private ViewConfigParam _viewConfigParam;
        private ZoneCompensationParam _compensaData;
        public ZoneCompensationForm(ZoneCompensationParam param, bool isShow = true)
        {
            InitializeComponent();
            this._compensaData = param;
            if (!isShow)
            {
                this.titleLabel.Hide();
                this.buttonMin.Hide();
                this.buttonMax.Hide();
                this.buttonClose.Hide();
            }
            ///// 构造时就初始化
        }


        private void CompensateListSubForm_Load(object sender, EventArgs e)
        {
            if (this._compensaData != null)
            {
                this.坐标系comboBox.DataSource = Enum.GetValues(typeof(enCoordSysName));
                this.坐标系comboBox.DataBindings.Add("Text", this._compensaData, nameof(this._compensaData.CoordSysName), true, DataSourceUpdateMode.OnPropertyChanged);
                ////////////////////////////////////////
                this.补偿X1_textBox.DataBindings.Add("Text", this._compensaData, nameof(this._compensaData.X1), true, DataSourceUpdateMode.OnPropertyChanged);
                this.补偿Y1_textBox.DataBindings.Add("Text", this._compensaData, nameof(this._compensaData.Y1), true, DataSourceUpdateMode.OnPropertyChanged);
                this.补偿Theta1_textBox.DataBindings.Add("Text", this._compensaData, nameof(this._compensaData.Angle1), true, DataSourceUpdateMode.OnPropertyChanged);
                ////////////////////////////////////////////////////////////////////////////////////////////////
                this.补偿X2_textBox.DataBindings.Add("Text", this._compensaData, nameof(this._compensaData.X2), true, DataSourceUpdateMode.OnPropertyChanged);
                this.补偿Y2_textBox.DataBindings.Add("Text", this._compensaData, nameof(this._compensaData.Y2), true, DataSourceUpdateMode.OnPropertyChanged);
                this.补偿Theta2_textBox.DataBindings.Add("Text", this._compensaData, nameof(this._compensaData.Angle2), true, DataSourceUpdateMode.OnPropertyChanged);
                ////////////////////////////////////////////////////////////////////////////////////////////////
                this.补偿X3_textBox.DataBindings.Add("Text", this._compensaData, nameof(this._compensaData.X3), true, DataSourceUpdateMode.OnPropertyChanged);
                this.补偿Y3_textBox.DataBindings.Add("Text", this._compensaData, nameof(this._compensaData.Y3), true, DataSourceUpdateMode.OnPropertyChanged);
                this.补偿Theta3_textBox.DataBindings.Add("Text", this._compensaData, nameof(this._compensaData.Angle3), true, DataSourceUpdateMode.OnPropertyChanged);
                ////////////////////////////////////////////////////////////////////////////////////////////////
                this.补偿X4_textBox.DataBindings.Add("Text", this._compensaData, nameof(this._compensaData.X4), true, DataSourceUpdateMode.OnPropertyChanged);
                this.补偿Y4_textBox.DataBindings.Add("Text", this._compensaData, nameof(this._compensaData.Y4), true, DataSourceUpdateMode.OnPropertyChanged);
                this.补偿Theta4_textBox.DataBindings.Add("Text", this._compensaData, nameof(this._compensaData.Angle4), true, DataSourceUpdateMode.OnPropertyChanged);
                ////////////////////////////////////////////////////////////////////////////////////////////////
                this.补偿X5_textBox.DataBindings.Add("Text", this._compensaData, nameof(this._compensaData.X5), true, DataSourceUpdateMode.OnPropertyChanged);
                this.补偿Y5_textBox.DataBindings.Add("Text", this._compensaData, nameof(this._compensaData.Y5), true, DataSourceUpdateMode.OnPropertyChanged);
                this.补偿Theta5_textBox.DataBindings.Add("Text", this._compensaData, nameof(this._compensaData.Angle5), true, DataSourceUpdateMode.OnPropertyChanged);
                ////////////////////////////////////////////////////////////////////////////////////////////////
                this.补偿X6_textBox.DataBindings.Add("Text", this._compensaData, nameof(this._compensaData.X6), true, DataSourceUpdateMode.OnPropertyChanged);
                this.补偿Y6_textBox.DataBindings.Add("Text", this._compensaData, nameof(this._compensaData.Y6), true, DataSourceUpdateMode.OnPropertyChanged);
                this.补偿Theta6_textBox.DataBindings.Add("Text", this._compensaData, nameof(this._compensaData.Angle6), true, DataSourceUpdateMode.OnPropertyChanged);
                ////////////////////////////////////////////////////////////////////////////////////////////////
                this.补偿X7_textBox.DataBindings.Add("Text", this._compensaData, nameof(this._compensaData.X7), true, DataSourceUpdateMode.OnPropertyChanged);
                this.补偿Y7_textBox.DataBindings.Add("Text", this._compensaData, nameof(this._compensaData.Y7), true, DataSourceUpdateMode.OnPropertyChanged);
                this.补偿Theta7_textBox.DataBindings.Add("Text", this._compensaData, nameof(this._compensaData.Angle7), true, DataSourceUpdateMode.OnPropertyChanged);
                ////////////////////////////////////////////////////////////////////////////////////////////////
                this.补偿X8_textBox.DataBindings.Add("Text", this._compensaData, nameof(this._compensaData.X8), true, DataSourceUpdateMode.OnPropertyChanged);
                this.补偿Y8_textBox.DataBindings.Add("Text", this._compensaData, nameof(this._compensaData.Y8), true, DataSourceUpdateMode.OnPropertyChanged);
                this.补偿Theta8_textBox.DataBindings.Add("Text", this._compensaData, nameof(this._compensaData.Angle8), true, DataSourceUpdateMode.OnPropertyChanged);
                ////////////////////////////////////////////////////////////////////////////////////////////////
                //this.启用自动补偿CheckBox.DataBindings.Add(nameof(this.启用自动补偿CheckBox.Checked), this._compensaData, nameof(this._compensaData.IsAuto), true, DataSourceUpdateMode.OnPropertyChanged);
                //this.取反补偿CheckBox.DataBindings.Add(nameof(this.取反补偿CheckBox.Checked), this._compensaData, nameof(this._compensaData.IsInvert), true, DataSourceUpdateMode.OnPropertyChanged);
                //this.补偿阈值textBox.DataBindings.Add("Text", this._compensaData, nameof(this._compensaData.Threshold), true, DataSourceUpdateMode.OnPropertyChanged);
            }
            else
                new UserMessageForm().ShowDialog("ZoneCompensationParam：对象为空");
            /////////////////////////////////         读取补偿数据
            this.IsLoad = true;
        }



        private void CompensateForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                this.IsLoad = false;
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
            if (this._viewConfigParam != null)
                this._viewConfigParam.Location = this.Location;
        }

        private void CompensateForm_Resize(object sender, EventArgs e)
        {
            if (this._viewConfigParam != null && this.IsLoad) // 在窗体加载后这个事件才起作用
                this._viewConfigParam.FormSize = this.Size;
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

        private void 补偿XtextBox_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                //if (e.KeyCode == Keys.Enter)
                //{
                //    if (this._compensaData.Count > 0)
                //    {
                //        CompensationResultInfo data = this._compensaData.Last();
                //        double X = 0, Y = 0, Angle = 0;
                //        double.TryParse(补偿X1_textBox.Text, out X);
                //        double.TryParse(补偿Y1_textBox.Text, out Y);
                //        double.TryParse(补偿Theta1_textBox.Text, out Angle);
                //        if (data.X != X || data.Y != Y || data.Angle != Angle)
                //        {
                //            CompensationResultInfo info = new CompensationResultInfo();
                //            info.DateTime = DateTime.Now.ToString("yyyy-MM-dd-hh:mm:ss");
                //            info.X = X;
                //            info.Y = Y;
                //            info.Angle = Angle;
                //            this._compensaData.Add(info);
                //            if (this._compensaData.Count > 1000) this._compensaData.RemoveAt(0);
                //            CompensationDataManager.Instance.Save(this._compensaData, this._viewConfigParam.ViewName.Replace(":[", "_").Replace("]", ""));
                //        }
                //    }
                //    else
                //    {
                //        CompensationResultInfo info = new CompensationResultInfo();
                //        info.DateTime = DateTime.Now.ToString("yyyy-MM-dd-hh:mm:ss");
                //        this._compensaData.Add(info);
                //        CompensationDataManager.Instance.Save(this._compensaData, this._viewConfigParam.ViewName.Replace(":[", "_").Replace("]", ""));
                //    }
                //}
            }
            catch (Exception ex)
            {
                //new UserMessageForm().ShowDialog(ex.ToString());
            }
        }

        private void 补偿YtextBox_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    //if (this._compensaData.Count > 0)
                    //{
                    //    CompensationResultInfo data = this._compensaData.Last();
                    //    double X = 0, Y = 0, Angle = 0;
                    //    double.TryParse(补偿X1_textBox.Text, out X);
                    //    double.TryParse(补偿Y1_textBox.Text, out Y);
                    //    double.TryParse(补偿Theta1_textBox.Text, out Angle);
                    //    if (data.X != X || data.Y != Y || data.Angle != Angle)
                    //    {
                    //        CompensationResultInfo info = new CompensationResultInfo();
                    //        info.DateTime = DateTime.Now.ToString("yyyy-MM-dd-hh:mm:ss");
                    //        info.X = X;
                    //        info.Y = Y;
                    //        info.Angle = Angle;
                    //        this._compensaData.Add(info);
                    //        if (this._compensaData.Count > 1000) this._compensaData.RemoveAt(0);
                    //        CompensationDataManager.Instance.Save(this._compensaData, this._viewConfigParam.ViewName.Replace(":[", "_").Replace("]", ""));
                    //    }
                    //}
                    //else
                    //{
                    //    CompensationResultInfo info = new CompensationResultInfo();
                    //    info.DateTime = DateTime.Now.ToString("yyyy-MM-dd-hh:mm:ss");
                    //    this._compensaData.Add(info);
                    //    CompensationDataManager.Instance.Save(this._compensaData, this._viewConfigParam.ViewName.Replace(":[", "_").Replace("]", ""));
                    //}
                }
            }
            catch (Exception ex)
            {
                //new UserMessageForm().ShowDialog(ex.ToString());
            }
        }

        private void 补偿ThetatextBox_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                //if (e.KeyCode == Keys.Enter)
                //{
                //    if (this._compensaData.Count > 0)
                //    {
                //        CompensationResultInfo data = this._compensaData.Last();
                //        double X = 0, Y = 0, Angle = 0;
                //        double.TryParse(补偿X1_textBox.Text, out X);
                //        double.TryParse(补偿Y1_textBox.Text, out Y);
                //        double.TryParse(补偿Theta1_textBox.Text, out Angle);
                //        if (data.X != X || data.Y != Y || data.Angle != Angle)
                //        {
                //            CompensationResultInfo info = new CompensationResultInfo();
                //            info.DateTime = DateTime.Now.ToString("yyyy-MM-dd-hh:mm:ss");
                //            info.X = X;
                //            info.Y = Y;
                //            info.Angle = Angle;
                //            this._compensaData.Add(info);
                //            if (this._compensaData.Count > 1000) this._compensaData.RemoveAt(0);
                //            CompensationDataManager.Instance.Save(this._compensaData, this._viewConfigParam.ViewName.Replace(":[", "_").Replace("]", ""));
                //        }
                //    }
                //    else
                //    {
                //        CompensationResultInfo info = new CompensationResultInfo();
                //        info.DateTime = DateTime.Now.ToString("yyyy-MM-dd-hh:mm:ss");
                //        this._compensaData.Add(info);
                //        CompensationDataManager.Instance.Save(this._compensaData, this._viewConfigParam.ViewName.Replace(":[", "_").Replace("]", ""));
                //    }
                //}
            }
            catch (Exception ex)
            {
                //new UserMessageForm().ShowDialog(ex.ToString());
            }
        }


        private void 确定button_Click(object sender, EventArgs e)
        {
            try
            {
                //if (this._compensaData.Count > 0)
                //{
                //    CompensationResultInfo data = this._compensaData.Last();
                //    double X = 0, Y = 0, Angle = 0;
                //    double.TryParse(补偿X1_textBox.Text, out X);
                //    double.TryParse(补偿Y1_textBox.Text, out Y);
                //    double.TryParse(补偿Theta1_textBox.Text, out Angle);
                //    if (data.X != X || data.Y != Y || data.Angle != Angle)
                //    {
                //        CompensationResultInfo info = new CompensationResultInfo();
                //        info.DateTime = DateTime.Now.ToString("yyyy-MM-dd-hh:mm:ss");
                //        info.X = X;
                //        info.Y = Y;
                //        info.Angle = Angle;
                //        this._compensaData.Add(info);
                //        if (this._compensaData.Count > 1000) this._compensaData.RemoveAt(0);
                //        CompensationDataManager.Instance.Save(this._compensaData, this._viewConfigParam.ViewName.Replace(":[", "_").Replace("]", ""));
                //    }
                //}
                //else
                //{
                //    CompensationResultInfo info = new CompensationResultInfo();
                //    info.DateTime = DateTime.Now.ToString("yyyy-MM-dd-hh:mm:ss");
                //    this._compensaData.Add(info);
                //    CompensationDataManager.Instance.Save(this._compensaData, this._viewConfigParam.ViewName.Replace(":[", "_").Replace("]", ""));
                //}
                this.Close();
            }
            catch (Exception ex)
            {
               new Common.UserMessageForm(ex.ToString()).ShowDialog();;
            }


        }

        private void 启用自动补偿CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            //if (this.启用自动补偿CheckBox.Checked)
            //{
            //    this.取反补偿CheckBox.Enabled = true;
            //    this.补偿阈值textBox.Enabled = true;
            //}
            //else
            //{
            //    this.取反补偿CheckBox.Enabled = false;
            //    this.补偿阈值textBox.Enabled = false;
            //}
        }




    }
}

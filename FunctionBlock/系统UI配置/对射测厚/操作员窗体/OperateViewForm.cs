
using Common;
using HalconDotNet;
using MotionControlCard;
using Sensor;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using View;

namespace FunctionBlock
{
    public partial class OperateViewForm : Form
    {
        private ViewConfigParam _viewConfigParam;
        private bool IsLoad = false; // 窗体加载后，设置为true
        private string programPath = "";

        public OperateViewForm(ViewConfigParam viewConfigParam)
        {
            InitializeComponent();
            /////////////
            this._viewConfigParam = viewConfigParam;
            this.titleLabel.Text = viewConfigParam.ViewName;
            this.titleLabel.TextAlign = ContentAlignment.MiddleLeft;
            this.ContextMenu = new ContextMenu();
        }
        private void OperateViewForm_Load(object sender, EventArgs e)
        {
            //////////////
            BindProperty();
            //////////////////////////////////////
            this.Location = this._viewConfigParam.Location;
            this.Size = this._viewConfigParam.FormSize;
            //this.SignLabel.Text = this._viewConfigParam.Tag;   // 图形窗口:" + "[" + this.Handle.ToString() + "]";
            this.IsLoad = true;
        }
        private void BindProperty()
        {
            try
            {
                OperateParam param;
                OperateManager.Instance.Read();
                if (OperateManager.Instance.ProgramList.Count > 0)
                    param = OperateManager.Instance.ProgramList[0];
                else
                {
                    param = new OperateParam();
                    OperateManager.Instance.ProgramList.Add(param);
                }
                this.操作员comboBox.DataBindings.Add(nameof(this.操作员comboBox.Text), param, nameof(param.Operate), true, DataSourceUpdateMode.OnPropertyChanged);
                this.产品尺寸comboBox.DataBindings.Add(nameof(this.产品尺寸comboBox.Text), param, nameof(param.ProductSize), true, DataSourceUpdateMode.OnPropertyChanged);
                this.产品标识号textBox.DataBindings.Add(nameof(this.产品标识号textBox.Text), param, nameof(param.ProductID), true, DataSourceUpdateMode.OnPropertyChanged);
                this.多文件目录textBox.DataBindings.Add(nameof(this.多文件目录textBox.Text), param, nameof(param.SaveDirect), true, DataSourceUpdateMode.OnPropertyChanged);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }



        /// <summary>
        /// 窗体移动事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ViewForm_Move(object sender, EventArgs e)
        {
            if (this._viewConfigParam != null)
                this._viewConfigParam.Location = this.Location;
        }

        /// <summary>
        /// 窗体尺寸改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ViewForm_Resize(object sender, EventArgs e)
        {
            if (this._viewConfigParam != null && this.IsLoad) // 在窗体加载后这个事件才起作用
                this._viewConfigParam.FormSize = this.Size;
        }

        private void ViewForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                this.IsLoad = false;
                ViewConfigParamManager.Instance.ViewParamList.Remove(this._viewConfigParam); // 关闭窗体时要删除相应的对象
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
            DialogResult dialogResult = MessageBox.Show("确定关闭窗体吗？", "关闭窗体", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                this.Close();  //关闭窗口
            }
        }
        private void buttonMax_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;  //最小化
        }
        private void buttonMin_Click(object sender, EventArgs e)
        {
            //this.WindowState = FormWindowState.Minimized;  //最小化
        }
        #endregion


        private void ViewForm_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, WM_SYSCOMMAND, SC_MOVE + HTCAPTION, 0);
        }

        private void label1_MouseDown(object sender, MouseEventArgs e)
        {
            ViewForm_MouseDown(null, null);
        }

        private void label1_MouseEnter(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Hand;
            this.titleLabel.BackColor = System.Drawing.Color.Orange;// System.Drawing.SystemColors.HotTrack;
        }

        private void label1_MouseLeave(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Default;
            this.titleLabel.BackColor = System.Drawing.Color.LightGray;// System.Drawing.SystemColors.Control;
        }

        private void 保存button_Click(object sender, EventArgs e)
        {
            try
            {
                OperateManager.Instance.Save();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void 运行but_Click(object sender, EventArgs e)
        {
            try
            {
                ProgramForm.Instance.Run(null, 1);
                this.停止but.Enabled = true;
                this.运行but.Enabled = false;
                this.停止but.BackColor = System.Drawing.Color.Red;
                this.运行but.BackColor = System.Drawing.Color.Green;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void 停止but_Click(object sender, EventArgs e)
        {
            try
            {
                ProgramForm.Instance.Stop();
                this.停止but.Enabled = false;
                this.运行but.Enabled = true;
                this.停止but.BackColor = System.Drawing.Color.DarkGray;
                this.运行but.BackColor = System.Drawing.Color.Yellow;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }

        private void readDirectoryButton_Click(object sender, EventArgs e)
        {
            try
            {
                FolderBrowserDialog fold = new FolderBrowserDialog();
                fold.ShowDialog();
                if (fold.SelectedPath != null)
                    this.多文件目录textBox.Text = fold.SelectedPath;
            }
            catch
            {
                MessageBox.Show(new Exception().ToString());
            }
        }

        private void 多文件目录textBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void 产品标识号textBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void 产品尺寸comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void 操作员comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
    }
}

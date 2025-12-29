
using Common;
using HalconDotNet;
using MotionControlCard;
using Sensor;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using View;

namespace FunctionBlock
{
    public partial class CamParamForm : Form
    {
        private ViewConfigParam _viewConfigParam;
        private bool IsLoad = false; // 窗体加载后，设置为true
        public string ReName { get; set; }
        private string _camName = "";

        private ISensor _sensor;    

        public CamParamForm(ViewConfigParam viewConfigParam)
        {
            InitializeComponent();
            ///////////////////////////////////////////
            this._viewConfigParam = viewConfigParam;
            this.ContextMenu = new ContextMenu();
            //this.titleLabel.Text = viewConfigParam.CamName;
            this.titleLabel.TextAlign = ContentAlignment.MiddleLeft;

        }
        private void CamParamForm_Load(object sender, EventArgs e)
        {
            //////////////////////////////////////
            this.Location = this._viewConfigParam.Location;
            this.Size = this._viewConfigParam.FormSize;
            this.IsLoad = true;
            this.相机comboBox.Items.Clear();
            this.相机comboBox.Items.Add("NONE");
            foreach (string item in SensorManage.GetCamSensorName())
            {
                this.相机comboBox.Items.Add(item); 
            }
            this.相机comboBox.Text = this._viewConfigParam.CamName;
            this._sensor = SensorManage.GetSensor(this._viewConfigParam.CamName);
            this.曝光值textBox.Text = this._sensor?.GetParam("曝光").ToString();
            this.增益textBox.Text = this._sensor?.GetParam("增益").ToString();
            int result = 0;
            int.TryParse(this.曝光值textBox.Text, out result);
            this.trackBar1.Value = result;
            this.addContextMenu();
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
            DialogResult dialogResult = new UserMessageForm().ShowDialog("确定关闭窗体吗？", "关闭窗体");
            if (dialogResult == DialogResult.OK)
            {
                ViewConfigParamManager.Instance.ViewParamList.Remove(this._viewConfigParam);
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
                this.WindowState = FormWindowState.Normal;
                //this.WindowState = FormWindowState.Maximized;   //如果处于普通状态，则最大化
            }
        }
        private void buttonMin_Click(object sender, EventArgs e)
        {
            //this.WindowState = FormWindowState.Minimized;  //最小化
        }
        #endregion



        #region 右键菜单项
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
            this.titleLabel.ContextMenuStrip = ContextMenuStrip1;
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
                        RenameForm renameForm = new RenameForm(this.titleLabel.Text);
                        DialogResult dialogResult = renameForm.ShowDialog();
                        if (dialogResult == DialogResult.OK)
                        {
                            this._viewConfigParam.Tag = renameForm.ReName;
                            this._viewConfigParam.Text = renameForm.ReName;
                            this._viewConfigParam.ViewName = renameForm.ReName;
                            //this.SignLabel.Text = this._viewConfigParam.Tag;
                            this.titleLabel.Text = this._viewConfigParam.Tag;
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




        private void ViewForm_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, WM_SYSCOMMAND, SC_MOVE + HTCAPTION, 0);
        }

        private void titleLabel_MouseDown(object sender, MouseEventArgs e)
        {
            ViewForm_MouseDown(null, null);
        }

        private void titleLabel_MouseEnter(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Hand;
            this.titleLabel.BackColor = System.Drawing.Color.Orange;// System.Drawing.SystemColors.HotTrack;
        }

        private void titleLabel_MouseLeave(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Default;
            this.titleLabel.BackColor = System.Drawing.Color.LightGray;// System.Drawing.SystemColors.Control;
        }

        private void 曝光值textBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                int result = 0;
                int.TryParse(this.曝光值textBox.Text, out result);
                this.trackBar1.Value = result;
                this._sensor?.SetParam("曝光", result);
            }
            catch (Exception ex)
            {
                new UserMessageForm(ex.ToString()).ShowDialog();
            }
        }

        private void 相机comboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            try
            {
                if (this.相机comboBox.Text == null) return;
                this._sensor = SensorManage.GetSensor(this.相机comboBox.Text);
                this._viewConfigParam.CamName = this.相机comboBox.Text;
                //this.titleLabel.Text = this.相机comboBox.Text;
                this.曝光值textBox.Text = this._sensor?.GetParam("曝光").ToString();
                this.增益textBox.Text = this._sensor?.GetParam("增益").ToString();
                int result = 0;
                int.TryParse(this.曝光值textBox.Text, out result);
                this.trackBar1.Value = result;
                ViewConfigParamManager.Instance.Save();
            }
            catch(Exception ex)
            {
                new UserMessageForm(ex.ToString()).ShowDialog(); 
            }
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            try
            {
                this.曝光值textBox.Text = this.trackBar1.Value.ToString();
                this._sensor?.SetParam("曝光", this.曝光值textBox.Text);
            }
            catch (Exception ex)
            {
                new UserMessageForm(ex.ToString()).ShowDialog();
            }
        }

        private void 增益textBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                int result = 0;
                int.TryParse(this.增益textBox.Text, out result);
                this._sensor?.SetParam("增益", result);
            }
            catch (Exception ex)
            {
                new UserMessageForm(ex.ToString()).ShowDialog();
            }
        }


    }




}

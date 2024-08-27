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
    public partial class JogMotionForm : Form
    {
        private IMotionControl i_McCard;
        private enCoordSysName CoordSysName;
        public IMotionControl I_McCard { get => i_McCard; set => i_McCard = value; }
        private ViewConfigParam _viewConfigParam;
        private bool IsLoad = false; // 窗体加载后，设置为true

        public JogMotionForm(ViewConfigParam viewConfigParam)
        {
            InitializeComponent();
            this.groupBox4.Margin = new Padding(0);
            this.CoordSysName = enCoordSysName.CoordSys_1;
            this._viewConfigParam = viewConfigParam;
            this.titleLabel.Text = viewConfigParam.ViewName;
            this.ContextMenu = new ContextMenu();
            //UserLoginParamManager.Instance.LoginParam.UserChange += new EventHandler(this.UserChange_Event);
            UserLoginParamManager.Instance.UserChange += new EventHandler(this.UserChange_Event);
        }

        public JogMotionForm()
        {
            InitializeComponent();
            this.groupBox4.Margin = new Padding(0);
            this.CoordSysName = enCoordSysName.CoordSys_1;
        }

        private void JogMotionForm_Load(object sender, EventArgs e)
        {
            this.坐标系名称comboBox.DataSource = Enum.GetValues(typeof(enCoordSysName));
            this.坐标系名称comboBox.SelectedIndex = 1;
            //////////////
            if (this._viewConfigParam != null)
                this.Location = this._viewConfigParam.Location;
            if (this._viewConfigParam != null)
                this.Size = this._viewConfigParam.FormSize;
            this.IsLoad = true;
            //this.addContextMenu();
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
        // 正向移动X轴
        private void MoveXAddbutton_MouseDown(object sender, MouseEventArgs e)
        {
            double speed = double.Parse(速度lable.Text);
            try
            {
                if (i_McCard == null)
                {
                    throw new ArgumentNullException("i_McCard");
                }
                i_McCard.JogAxisStart(this.CoordSysName, enAxisName.X轴, speed);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }
        private void MoveXAddbutton_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                if (i_McCard == null)
                {
                    throw new ArgumentNullException("i_McCard");
                }
                i_McCard.JogAxisStop();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        // 负向移动X轴
        private void MoveXminusbutton_MouseDown(object sender, MouseEventArgs e)
        {
            double speed = double.Parse(速度lable.Text);
            try
            {
                if (i_McCard == null)
                {
                    throw new ArgumentNullException("i_McCard");
                }
                i_McCard.JogAxisStart(this.CoordSysName, enAxisName.X轴, -speed);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void MoveXminusbutton_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                if (i_McCard == null)
                {
                    throw new ArgumentNullException("i_McCard");
                }
                i_McCard.JogAxisStop();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        // 正向移动Y轴
        private void MoveYAddbutton_MouseDown(object sender, MouseEventArgs e)
        {
            double speed = double.Parse(速度lable.Text);
            try
            {
                if (i_McCard == null)
                {
                    throw new ArgumentNullException("i_McCard");
                }
                i_McCard.JogAxisStart(this.CoordSysName, enAxisName.Y轴, speed);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void MoveYAddbutton_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                if (i_McCard == null)
                {
                    throw new ArgumentNullException("i_McCard");
                }
                i_McCard.JogAxisStop();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        // 负向移动Y轴
        private void MoveYminusbutton_MouseDown(object sender, MouseEventArgs e)
        {
            double speed = double.Parse(速度lable.Text);
            try
            {
                if (i_McCard == null)
                {
                    throw new ArgumentNullException("i_McCard");
                }
                i_McCard.JogAxisStart(this.CoordSysName, enAxisName.Y轴, -speed);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void MoveYminusbutton_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                if (i_McCard == null)
                {
                    throw new ArgumentNullException("i_McCard");
                }
                i_McCard.JogAxisStop();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        // 正向移动Z轴
        private void MoveZAddbutton_MouseDown(object sender, MouseEventArgs e)
        {

            try
            {
                if (i_McCard == null)
                {
                    throw new ArgumentNullException("i_McCard");
                }
                double speed = double.Parse(速度lable.Text);
                i_McCard.JogAxisStart(this.CoordSysName, enAxisName.Z轴, speed);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void MoveZAddbutton_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                if (i_McCard == null)
                {
                    throw new ArgumentNullException("i_McCard");
                }
                i_McCard.JogAxisStop();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        // 负向移动Z轴
        private void MoveZminusbutton_MouseDown(object sender, MouseEventArgs e)
        {
            double speed = double.Parse(速度lable.Text);
            try
            {
                if (i_McCard == null)
                {
                    throw new ArgumentNullException("i_McCard");
                }
                i_McCard.JogAxisStart(this.CoordSysName, enAxisName.Z轴, -speed);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void MoveZminusbutton_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                if (i_McCard == null)
                {
                    throw new ArgumentNullException("i_McCard");
                }
                i_McCard.JogAxisStop();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        // 窗体关闭时处理
        private void MotionControl_FormClosing(object sender, FormClosingEventArgs e)
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


        #region 轴移动
        private void MoveU轴AddButton_MouseDown(object sender, MouseEventArgs e)
        {
            double speed = double.Parse(速度lable.Text);
            if (i_McCard != null)
                i_McCard.JogAxisStart(this.CoordSysName, enAxisName.U轴, speed);
        }
        private void MoveU轴AddButton_MouseUp(object sender, MouseEventArgs e)
        {
            if (i_McCard != null)
                i_McCard.JogAxisStop();
        }
        private void MoveU轴minusButton_MouseDown(object sender, MouseEventArgs e)
        {
            double speed = double.Parse(速度lable.Text);
            if (i_McCard != null)
                i_McCard.JogAxisStart(this.CoordSysName, enAxisName.U轴, -speed);
        }
        private void MoveU轴minusButton_MouseUp(object sender, MouseEventArgs e)
        {
            if (i_McCard != null)
                i_McCard.JogAxisStop();
        }
        private void MoveV轴AddButton_MouseDown(object sender, MouseEventArgs e)
        {
            double speed = double.Parse(速度lable.Text);
            if (i_McCard != null)
                i_McCard.JogAxisStart(this.CoordSysName, enAxisName.V轴, speed);
        }
        private void MoveV轴AddButton_MouseUp(object sender, MouseEventArgs e)
        {
            if (i_McCard != null)
                i_McCard.JogAxisStop();
        }
        private void MoveVminusbutton_MouseDown(object sender, MouseEventArgs e)
        {
            double speed = double.Parse(速度lable.Text);
            if (i_McCard != null)
                i_McCard.JogAxisStart(this.CoordSysName, enAxisName.V轴, -speed);
        }
        private void MoveVminusbutton_MouseUp(object sender, MouseEventArgs e)
        {
            if (i_McCard != null)
                i_McCard.JogAxisStop();
        }
        private void MoveWAddButton_MouseDown(object sender, MouseEventArgs e)
        {
            double speed = double.Parse(速度lable.Text);
            if (i_McCard != null)
                i_McCard.JogAxisStart(this.CoordSysName, enAxisName.W轴, speed);
        }
        private void MoveWAddButton_MouseUp(object sender, MouseEventArgs e)
        {
            if (i_McCard != null)
                i_McCard.JogAxisStop();
        }
        private void MoveWminusButton_MouseDown(object sender, MouseEventArgs e)
        {
            double speed = double.Parse(速度lable.Text);
            if (i_McCard != null)
                i_McCard.JogAxisStart(this.CoordSysName, enAxisName.W轴, -speed);
        }
        private void MoveWminusButton_MouseUp(object sender, MouseEventArgs e)
        {
            if (i_McCard != null)
                i_McCard.JogAxisStop();
        }
        private void StopButton_Click(object sender, EventArgs e)
        {
            if (i_McCard != null)
                i_McCard.EmgStopAxis();


        }
        private void 速度trackBar_Scroll(object sender, EventArgs e)
        {
            this.速度lable.Text = this.速度trackBar.Value.ToString();
        }

        #endregion

        private void 参数button_Click(object sender, EventArgs e)
        {
            try
            {
                //new MachineParamForm().Show();
                new CoordSysConfigParamManageForm().Show();
            }
            catch
            {

            }
        }

        private void 坐标系comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.坐标系名称comboBox.SelectedIndex == -1) return;
                Enum.TryParse(this.坐标系名称comboBox.SelectedItem.ToString(), out this.CoordSysName);
                MotionCardManage.CurrentCoordSys = this.CoordSysName;
                this.i_McCard = MotionCardManage.GetCard(this.CoordSysName);
                if (MotionCardManage.CurrentCard != null && MotionCardManage.CurrentCard.CoordSysConfigParam != null)
                    MotionCardManage.CurrentCard.CoordSysConfigParam.CoordSysName = this.CoordSysName;
            }
            catch
            {

            }
        }

        private void JogMotionForm_Move(object sender, EventArgs e)
        {
            if (this._viewConfigParam != null)
                this._viewConfigParam.Location = this.Location;
        }

        private void JogMotionForm_Resize(object sender, EventArgs e)
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

        private void JogMotionForm_MouseEnter(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Hand;
            this.titleLabel.BackColor = System.Drawing.Color.Orange;
        }

        private void JogMotionForm_MouseLeave(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Default;
            this.titleLabel.BackColor = System.Drawing.Color.LightGray;
        }

        private void JogMotionForm_MouseDown(object sender, MouseEventArgs e)
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
            JogMotionForm_MouseDown(null, null);
        }


    }

}

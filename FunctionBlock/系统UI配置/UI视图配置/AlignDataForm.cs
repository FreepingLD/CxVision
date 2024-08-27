
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
    public partial class AlignDataForm : Form
    {
        private ViewConfigParam _viewConfigParam;
        private bool IsLoad = false; // 窗体加载后，设置为true
        private BindingList<AlignResultInfo> _alignData;
        public AlignDataForm()
        {
            InitializeComponent();
            ///// 构造时就初始化
        }

        public AlignDataForm(ViewConfigParam viewConfigParam)
        {
            InitializeComponent();
            this._viewConfigParam = viewConfigParam;
            this.titleLabel.Text = viewConfigParam.ViewName;
            ///// 构造时就初始化
            //UserLoginParamManager.Instance.LoginParam.UserChange += new EventHandler(this.UserChange_Event);
            UserLoginParamManager.Instance.UserChange += new EventHandler(this.UserChange_Event);
            this.ContextMenu = new ContextMenu();
        }

        private void AlignDataForm_Load(object sender, EventArgs e)
        {
            this.Location = this._viewConfigParam.Location;
            this.Size = this._viewConfigParam.FormSize;
            this.IsLoad = true;
            ////////////////////////////////
            this.DataColumn.Items.Clear();
            this.DataColumn.ValueType = typeof(string);
            this.DataColumn.Items.Add("NONE");
            foreach (var item in MemoryManager.Instance.GetKeysValue())
                this.DataColumn.Items.Add(item);
            ////////////////////////////////
            AlignDataManager.Instance.Read(this._viewConfigParam.ViewName.Replace(":[", "_").Replace("]", ""));
            this._alignData = AlignDataManager.Instance.AlignData;
            this.数据写入dataGridView.DataSource = this._alignData; 
        }

        public void UserChange_Event(object sender, EventArgs e)
        {
            UserLoginParam loginParam = sender as UserLoginParam;
            switch (loginParam.User)
            {
                case enUserName.操作员:
                    this.buttonClose.Enabled = false;
                    this.数据写入dataGridView.ReadOnly = true;
                    break;
                case enUserName.工程师:
                    this.buttonClose.Enabled = false;
                    this.数据写入dataGridView.ReadOnly = false;
                    break;
                case enUserName.开发人员:
                    this.buttonClose.Enabled = true;
                    this.数据写入dataGridView.ReadOnly = false;
                    break;
            }
        }

        private void AlignDataForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                //UserLoginParamManager.Instance.LoginParam.UserChange -= new EventHandler(this.UserChange_Event);
                UserLoginParamManager.Instance.UserChange -= new EventHandler(this.UserChange_Event);
                DataPrefixConfigManager.Instance.Save();
                this.IsLoad = false;
                ViewConfigParamManager.Instance.ViewParamList.Remove(this._viewConfigParam); // 关闭窗体时要删除相应的对象
                AlignDataManager.Instance.Save();
                this.Hide();
                e.Cancel = true;
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

        private void 数据写入dataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    switch (数据写入dataGridView.Columns[e.ColumnIndex].Name)
                    {
                        case "DeleteBtn": //InsertBtn
                            this._alignData.RemoveAt(e.RowIndex);
                            break;
                        case "InsertBtn": //
                            this._alignData.Insert(e.RowIndex, new AlignResultInfo());
                            break;
                        case "SaveBtn": //
                            AlignDataManager.Instance.AlignData = this._alignData;
                            AlignDataManager.Instance.Save(this._viewConfigParam.ViewName.Replace(":[","_").Replace("]",""));
                            break;
                        case "UpMoveCol":
                            if (e.RowIndex > 0)
                            {
                                AlignResultInfo up = this._alignData[e.RowIndex - 1];
                                AlignResultInfo cur = this._alignData[e.RowIndex];
                                this._alignData[e.RowIndex - 1] = cur;
                                this._alignData[e.RowIndex] = up;
                            }
                            break;
                        case "DownMoveCol":
                            if (e.RowIndex < this._alignData.Count - 1)
                            {
                                AlignResultInfo down = this._alignData[e.RowIndex + 1];
                                AlignResultInfo cur = this._alignData[e.RowIndex];
                                this._alignData[e.RowIndex + 1] = cur;
                                this._alignData[e.RowIndex] = down;
                            }
                            break;
                        case "UpdataBtn":
                            if (e.RowIndex < this._alignData.Count - 1)
                            {
                                this.DataColumn.Items.Clear();
                                this.DataColumn.ValueType = typeof(string);
                                this.DataColumn.Items.Add("NONE");
                                foreach (var item in MemoryManager.Instance.GetKeysValue())
                                    this.DataColumn.Items.Add(item);
                            }
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }


    }
}

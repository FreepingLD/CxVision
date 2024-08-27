
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


namespace FunctionBlock
{
    public partial class ElementViewForm : Form
    {
        private ViewConfigParam _viewConfigParam;
        private bool IsLoad = false; // 窗体加载后，设置为true
        public ElementViewForm(ViewConfigParam viewConfigParam)
        {
            InitializeComponent();
            this._viewConfigParam = viewConfigParam;
            this.titleLabel.Text = "元素视图";
            this.ContextMenu = new ContextMenu();
            //UserLoginParamManager.Instance.LoginParam.UserChange += new EventHandler(this.UserChange_Event);
            UserLoginParamManager.Instance.UserChange += new EventHandler(this.UserChange_Event);
        }

        public ElementViewForm()
        {
            InitializeComponent();
            /////////////
            InitDataGridView(this.dataGridView1);
            this.Padding = new Padding(0);
        }

        private void ElementViewForm_Load(object sender, EventArgs e)
        {
            this._viewConfigParam = this._viewConfigParam == null ? new ViewConfigParam() : this._viewConfigParam;
            this.Location = this._viewConfigParam.Location;
            this.Size = this._viewConfigParam.FormSize;
            this.IsLoad = true;
            TreeViewWrapClass.ClickNode += new ClickNodeEventHandler(DisplayMeasureResult);
            this.addContextMenu();
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
        /// <summary>
        /// 鼠标点击相应节点时发生
        /// </summary>
        /// <param name="e"></param>
        public void DisplayMeasureResult(object send, TreeNodeMouseClickEventArgs e)  //DisplayMetrolegyObjectEventArgs
        {
            if (e.Node.Tag == null) return;
            try
            {
                if (!(e.Node.Tag is IFunction)) return;
                if (this.dataGridView1.DataSource != null && this.dataGridView1.DataSource.GetType().Name == e.Node.Tag.GetType().Name) return;
                this.dataGridView1.DataSource = null;
                if (((BaseFunction)e.Node.Tag).ResultInfo != null)
                    this.dataGridView1.DataSource = ((BaseFunction)e.Node.Tag).ResultInfo;
                else
                {
                    //DescriptionAttribute descriptionCustomAttribute;
                    //PropertyInfo[] propertyInfos = e.Node.Tag?.GetType().GetProperties();  //Parent.
                    //foreach (var item in propertyInfos)
                    //{
                    //    descriptionCustomAttribute = Attribute.GetCustomAttribute(item, typeof(DescriptionAttribute)) as DescriptionAttribute;  // 获取属性的描述特性
                    //    if (descriptionCustomAttribute != null && descriptionCustomAttribute.Description == "绑定属性")
                    //    {
                    //        object value = item.GetValue(e.Node.Tag);
                    //        if (value != null)
                    //        {
                    //            this.dataGridView1.DataSource = value;
                    //            break;
                    //        }
                    //    }
                    //}
                }
            }
            catch (Exception he)
            {
                MessageBox.Show(he.ToString());
            }
        }
        private void ElementViewForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                this.IsLoad = false;
                //UserLoginParamManager.Instance.LoginParam.UserChange -= new EventHandler(this.UserChange_Event);
                UserLoginParamManager.Instance.UserChange -= new EventHandler(this.UserChange_Event);
                ViewConfigParamManager.Instance.ViewParamList.Remove(this._viewConfigParam); // 关闭窗体时要删除相应的对象
                TreeViewWrapClass.ClickNode -= new ClickNodeEventHandler(DisplayMeasureResult);
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
            this.Close();  //关闭窗口
        }
        private void buttonMax_Click(object sender, EventArgs e)
        {

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

            // winform 中颜色设置
            // this.BackColor = Color.FromArgb(255, 255, 255); 自定设置颜色
            // this.BackColor = Color.Lime; // 设置web中的预定义颜色
            // this.BackColor = System.Drawing.SystemColors.Control; // 设置系统中的颜色
        }

        private void 拖动label_MouseDown(object sender, MouseEventArgs e)
        {
            ElementViewForm_MouseDown(null, null);
        }

        private void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            try
            {
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    dataGridView1.Rows[i].HeaderCell.Value = (i + 1).ToString();
                }
            }
            catch
            {
            }
        }

        private TabletForm tabletForm = null;
        private void dataGridView1_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    object cellValue = this.dataGridView1[e.ColumnIndex, e.RowIndex].Value;
                    this.tabletForm?.Close();
                    this.tabletForm = new TabletForm(cellValue.ToString());
                    System.Drawing.Point mp = System.Windows.Forms.Control.MousePosition;
                    int mousex = mp.X;  //鼠标当前X坐标
                    int mousey = mp.Y;  //鼠标当前Y坐标
                    this.tabletForm.Location = new Point(mousex, mousey);
                    this.tabletForm.TopLevel = true;
                    this.tabletForm.Show();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }



    }
}

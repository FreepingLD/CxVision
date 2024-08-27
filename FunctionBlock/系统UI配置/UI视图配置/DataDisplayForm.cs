
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

namespace FunctionBlock
{
    public partial class DataDisplayForm : Form
    {
        private DataTable dataTable = new DataTable("result"); // 如果要序列化他，那么必需要设置表名称
        private string templateFilePath;
        private int initIndex;
        private ViewConfigParam _viewConfigParam;
        private bool IsLoad = false; // 窗体加载后，设置为true
        private string SavePath = "D:\\测量数据";

        public DataDisplayForm()
        {
            InitializeComponent();
            ///// 构造时就初始化
            addContextMenu();
        }
        public DataDisplayForm(ViewConfigParam viewConfigParam)
        {
            InitializeComponent();
            this._viewConfigParam = viewConfigParam;
            this.titleLabel.Text = viewConfigParam.ViewName;
            ///// 构造时就初始化
            addContextMenu();
            OutputData.DataSend += new DataSendEventHandler(UpDataGridView);
            //UserLoginParamManager.Instance.LoginParam.UserChange += new EventHandler(this.UserChange_Event);
            UserLoginParamManager.Instance.UserChange += new EventHandler(this.UserChange_Event);
            this.ContextMenu = new ContextMenu();
        }
        private void DataDisplayForm_Load(object sender, EventArgs e)
        {
            this.Location = this._viewConfigParam.Location;
            this.Size = this._viewConfigParam.FormSize;
            this.IsLoad = true;
            Init();
        }
        private void InitDataGridView()
        {
            this.templateFilePath = Application.StartupPath + "\\" + "报表模板" + "\\" + "ReportTemplate.xlsx";
            //this.initIndex = ExcelHelper.ReadExcelToDataGridView(filePath, ref this.dataGridView1);
            //this.initIndex = ExcelHelper.ReadExcelToDataTable(filePath, ref this.dataTable);
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.RowHeadersWidth = 5;
            this.dataGridView1.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.dataGridView1.RowHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.dataGridView1.TopLeftHeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.dataGridView1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            //this.dataGridView1.ColumnHeadersHeight = 5;
            //this.dataGridView1.DataSource = this.dataTable;
            //this.dataGridView1.DataSource = OutPutDataConfigParamManager.Instance.DataItemParamList;
        }
        private void InitDataTable()
        {
            this.templateFilePath = Application.StartupPath + "\\" + "报表模板" + "\\" + "ReportTemplate.xlsx";
            //this.initIndex = ExcelHelper.ReadExcelToDataTable(templateFilePath, ref this.dataTable);  // 先读取模板，再读取备份数据
            if (File.Exists(this.SavePath + "\\" + DateTime.Now.ToString("yyyMMdd") + ".xml"))
            {
                this.dataTable.Rows.Clear();
                this.dataTable.ReadXml(this.SavePath + "\\" + DateTime.Now.ToString("yyyMMdd") + ".xml");
            }
            this.dataGridView1.DataSource = this.dataTable;
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.RowHeadersWidth = 5;
            this.dataGridView1.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.dataGridView1.RowHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.dataGridView1.TopLeftHeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.dataGridView1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dataGridView1.ColumnHeadersHeight = 5;
        }
        private void Init()
        {
            this.templateFilePath = Application.StartupPath + "\\" + "报表模板" + "\\" + "ReportTemplate.xlsx";
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.RowHeadersWidth = 5;
            this.dataGridView1.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.dataGridView1.RowHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.dataGridView1.TopLeftHeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.dataGridView1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dataGridView1.DataSource = OutputDataConfigParamManager.Instance.DataItemParamList;
            this.dataGridView2.DataSource = DataPrefixConfigManager.Instance.DataItemParamList;
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

        // 右键菜单项点击事件
        private void dataGridViewContextMenuStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            try
            {
                string path;
                string name = e.ClickedItem.Text;
                switch (name)
                {
                    case "保存":
                        path = SaveFile(0);
                        ExcelHelper.WriteDataGridViewToExcel(this.dataGridView1, path);// 以datatable作为数据保存介质，以XML文件作为备份保存
                        break;
                    case "删除":
                        int index = this.dataGridView1.CurrentRow.Index;
                        if (index > 2)
                        {
                            OutputDataConfigParamManager.Instance.DataItemParamList.RemoveAt(index);
                            for (int i = 3; i < OutputDataConfigParamManager.Instance.DataItemParamList.Count; i++)
                            {
                                OutputDataConfigParamManager.Instance.DataItemParamList[i].DataItem1 = (i - 2).ToString();
                            }
                        }
                        //deleteRow();
                        break;
                    case "清空":
                        //while (this.dataTable.Rows.Count > this.initIndex)
                        //this.dataTable.Rows.RemoveAt(this.initIndex);
                        //this.dataTable.WriteXml(Application.StartupPath + "\\" + "报表模板" + "\\" + DateTime.Now.ToString("yyyMMdd") + ".xml"); // 清空的同时，备份也要清空                    

                        if (OutputDataConfigParamManager.Instance.DataItemParamList.Count >= 3)
                        {
                            List<OutputDataConfigParam> list = new List<OutputDataConfigParam>();
                            for (int i = 0; i < 3; i++)
                            {
                                list.Add(OutputDataConfigParamManager.Instance.DataItemParamList[i]);
                            }
                            OutputDataConfigParamManager.Instance.DataItemParamList.Clear();
                            foreach (var item in list)
                            {
                                OutputDataConfigParamManager.Instance.DataItemParamList.Add(item);
                            }
                            list.Clear();
                        }
                        else
                            OutputDataConfigParamManager.Instance.DataItemParamList.Clear();
                        ///////////////////////////////////////////////
                        OutputDataConfigParamManager.Instance.Save();
                        break;
                    case "保存模板":
                        //path = SaveFile(0);
                        if (this.templateFilePath == null)
                            this.templateFilePath = Application.StartupPath + "\\" + "报表模板" + "\\" + "ReportTemplate.xlsx";
                        //ExcelHelper.WriteDataGridViewToExcel(this.dataGridView1, this.filePath);

                        OutputDataConfigParamManager.Instance.Save("报表模板");
                        break;
                    case "显示拖动区":
                        //this.拖动label.Show();
                        break;
                    case "隐藏拖动区":
                        //this.拖动label.Hide();
                        break;
                    case "关闭窗体":
                        this.Close();
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        //添加右键菜单
        private void addContextMenu()
        {
            ContextMenuStrip ContextMenuStrip1 = new ContextMenuStrip();
            ContextMenuStrip1.Items.Add("保存");
            ContextMenuStrip1.Items.Add("删除");
            ContextMenuStrip1.Items.Add("清空");
            ContextMenuStrip1.Items.Add("保存模板");
            ContextMenuStrip1.Items.Add("显示拖动区");
            ContextMenuStrip1.Items.Add("隐藏拖动区");
            ContextMenuStrip1.Items.Add("关闭窗体");
            ContextMenuStrip1.ItemClicked += new ToolStripItemClickedEventHandler(dataGridViewContextMenuStrip_ItemClicked);
            this.dataGridView1.ContextMenuStrip = ContextMenuStrip1;
        }
        private string SaveFile(int index)
        {
            string path = "";
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.DefaultExt = ".xlsx";
            sfd.Filter = "xls文件(*.xls)|*.xls|xlsx文件(*.xlsx)|*.xlsx|csv文件(.csv)|*.csv|所有文件(*.*)|*.**";
            sfd.RestoreDirectory = false;
            sfd.FilterIndex = index;
            if (sfd.ShowDialog() == DialogResult.OK)
                path = sfd.FileName;
            return path;
        }
        public void UpDataGridView(object sender, DataSendEventArgs e)
        {
            this.Invoke(new Action(() =>
            {
                string[] listData = ((List<string>)e.DataContent).ToArray();
                string[] limitUpData = ((List<string>)e.LimitUpContent).ToArray();
                string[] limitDownData = ((List<string>)e.LimitDownContent).ToArray();
                string[] stdValueData = ((List<string>)e.StdValueContent).ToArray();
                ////////////////////////////////////////////
                if (OutputDataConfigParamManager.Instance.FindConfigParam("标准值:"))
                    OutputDataConfigParamManager.Instance.DataItemParamList[0] = new OutputDataConfigParam(stdValueData);
                else
                    OutputDataConfigParamManager.Instance.DataItemParamList.Insert(0, new OutputDataConfigParam(stdValueData));
                /////////////////////////////////////////
                if (OutputDataConfigParamManager.Instance.FindConfigParam("上偏差:"))
                    OutputDataConfigParamManager.Instance.DataItemParamList[1] = new OutputDataConfigParam(limitUpData);
                else
                    OutputDataConfigParamManager.Instance.DataItemParamList.Insert(1, new OutputDataConfigParam(limitUpData));
                ////////////////////////////////
                if (OutputDataConfigParamManager.Instance.FindConfigParam("下偏差:"))
                    OutputDataConfigParamManager.Instance.DataItemParamList[2] = new OutputDataConfigParam(limitDownData);
                else
                    OutputDataConfigParamManager.Instance.DataItemParamList.Insert(2, new OutputDataConfigParam(limitDownData));
                ////////////////////////////////
                listData[0] = (OutputDataConfigParamManager.Instance.DataItemParamList.Count - 2).ToString();
                OutputDataConfigParamManager.Instance.DataItemParamList.Add(new OutputDataConfigParam(listData));
                if (OutputDataConfigParamManager.Instance.DataItemParamList.Count % GlobalVariable.pConfig.DataSaveGap == 0)
                    OutputDataConfigParamManager.Instance.Save("报表模板");
            }));
        }
        public void UpDataDataTable(object sender, DataSendEventArgs e)
        {
            this.Invoke(new Action(() =>
            {
                string[] listData = ((List<string>)e.DataContent).ToArray();
                string[] limitUpData = ((List<string>)e.LimitUpContent).ToArray();
                string[] limitDownData = ((List<string>)e.LimitDownContent).ToArray();
                string[] stdValueData = ((List<string>)e.StdValueContent).ToArray();
                ////////////////////////////////////////////

            }));
        }
        public void deleteRow()
        {
            if (this.dataGridView1.CurrentRow.IsNewRow || this.dataGridView1.CurrentRow == null) return;
            int index = this.dataGridView1.CurrentRow.Index;

            for (int i = 3; i < this.dataGridView1.Rows.Count; i++)
            {
                this.dataGridView1.Rows[i].Cells[0].Value = i - 2; // 更新序列号
            }

            for (int i = 3; i < OutputDataConfigParamManager.Instance.DataItemParamList.Count; i++)
            {
                OutputDataConfigParamManager.Instance.DataItemParamList[i].DataItem1 = (i - 2).ToString();
            }
        }
        private void DataDisplayForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                OutputData.DataSend -= new DataSendEventHandler(UpDataGridView);
                //UserLoginParamManager.Instance.LoginParam.UserChange -= new EventHandler(this.UserChange_Event);
                UserLoginParamManager.Instance.UserChange -= new EventHandler(this.UserChange_Event);
                DataPrefixConfigManager.Instance.Save();
                this.IsLoad = false;
                ViewConfigParamManager.Instance.ViewParamList.Remove(this._viewConfigParam); // 关闭窗体时要删除相应的对象
                this.Hide();
                e.Cancel = true;
            }
            catch
            {

            }
        }
        private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false;
        }
        private void dataGridView2_DataError(object sender, DataGridViewDataErrorEventArgs e)
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

        private void DataDisplayForm_Move(object sender, EventArgs e)
        {
            if (this._viewConfigParam != null)
                this._viewConfigParam.Location = this.Location;
        }

        private void DataDisplayForm_Resize(object sender, EventArgs e)
        {
            if (this._viewConfigParam != null && this.IsLoad) // 在窗体加载后这个事件才起作用
                this._viewConfigParam.FormSize = this.Size;
        }
        private void DataDisplayForm_MouseDown(object sender, MouseEventArgs e)
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


    }
}

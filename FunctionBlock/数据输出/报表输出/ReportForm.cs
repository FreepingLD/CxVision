using Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FunctionBlock
{
    public partial class ReportForm : Form
    {
        private ReportConfigParam ConfigParam;
        private ViewConfigParam _viewConfigParam;
        private bool IsLoad = false; // 窗体加载后，设置为true
        public ReportForm(ViewConfigParam viewConfigParam)
        {
            InitializeComponent();
            this._viewConfigParam = viewConfigParam;
            this.ConfigParam = ReportConfigParam.Instance.Read();
           ReportDataManage.Instance.Read(this.ConfigParam.FolderPath + "\\" + DateTime.Now.ToString("yyyyMMdd") + "\\" + this.ConfigParam.ProductName + ".xml");
            this.dataGridView1.DataSource = ReportDataManage.Instance.ReportParamList;
        }

        private void ReportForm_Load(object sender, EventArgs e)
        {
            try
            {
                this._viewConfigParam = this._viewConfigParam == null ? new ViewConfigParam() : this._viewConfigParam;
                this.Location = this._viewConfigParam.Location;
                this.Size = this._viewConfigParam.FormSize;
                this.IsLoad = true;
                this.addContextMenu();
                /////////////////////////////////////////////////// 
                BaseFunction.ExcuteCompleted += new ExcuteCompletedEventHandler(DisplayExcuteResult);
                this.保存类型comboBox.DataSource = Enum.GetValues(typeof(enSaveType));
                this.显示类型comboBox.DataSource = Enum.GetValues(typeof(enSaveType));
                /////////////////////////////////////////////////////
                this.文件目录textBox.DataBindings.Add("Text", this.ConfigParam, nameof(this.ConfigParam.FolderPath), true, DataSourceUpdateMode.OnPropertyChanged);
                this.产品名称TextBox.DataBindings.Add("Text", this.ConfigParam, nameof(this.ConfigParam.ProductName), true, DataSourceUpdateMode.OnPropertyChanged);
                this.保存类型comboBox.DataBindings.Add("Text", this.ConfigParam, nameof(this.ConfigParam.SaveType), true, DataSourceUpdateMode.OnPropertyChanged);
                this.显示类型comboBox.DataBindings.Add("Text", this.ConfigParam, nameof(this.ConfigParam.DisplayType), true, DataSourceUpdateMode.OnPropertyChanged);
                this.日期checkBox.DataBindings.Add(nameof(this.日期checkBox.Checked), this.ConfigParam, nameof(this.ConfigParam.AddDataTime), true, DataSourceUpdateMode.OnPropertyChanged);
                this.读取文件路径textBox.DataBindings.Add("Text", this.ConfigParam, nameof(this.ConfigParam.FilePath), true, DataSourceUpdateMode.OnPropertyChanged);
                this.图像列宽comboBox.DataBindings.Add("Text", this.ConfigParam, nameof(this.ConfigParam.ImageWidth), true, DataSourceUpdateMode.OnPropertyChanged);
                this.图像列高comboBox.DataBindings.Add("Text", this.ConfigParam, nameof(this.ConfigParam.Imageheight), true, DataSourceUpdateMode.OnPropertyChanged);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public void DisplayExcuteResult(object sender, ExcuteCompletedEventArgs e)
        {
            try
            {
                if (e.DataContent == null) return;// 在图形窗口只显示世界坐标元素，像素元素在图像窗口显示
                ReportData[] reportData;
                switch (e.DataContent.GetType().Name) //这里只接受XLD轮廓或3D对象轮廓
                {
                    case "MiniLedReportData[]":
                        switch (this.ConfigParam.SaveType)
                        {
                            case enSaveType.All:
                                reportData = (ReportData[])e.DataContent;
                                if (reportData != null)
                                {
                                    foreach (var item in reportData)
                                    {
                                        this.Invoke(new Action(() => ReportDataManage.Instance.ReportParamList.Add(item)));
                                    }
                                }
                                break;
                            case enSaveType.OK:
                                reportData = (ReportData[])e.DataContent;
                                if (reportData != null)
                                {
                                    foreach (var item in reportData)
                                    {
                                        if (item.Result == "OK")
                                            this.Invoke(new Action(() => ReportDataManage.Instance.ReportParamList.Add(item)));
                                    }
                                }
                                break;
                            case enSaveType.NG:
                                reportData = (ReportData[])e.DataContent;
                                if (reportData != null)
                                {
                                    foreach (var item in reportData)
                                    {
                                        if (item.Result == "NG")
                                            this.Invoke(new Action(() => ReportDataManage.Instance.ReportParamList.Add(item)));

                                    }
                                }
                                break;
                        }
                        if (ReportDataManage.Instance.ReportParamList.Count / 10 == 0)
                        {
                            if (this.ConfigParam.FolderPath == null)
                            {
                                ReportDataManage.Instance.Save();
                            }
                            else
                            {
                                if (this.ConfigParam.AddDataTime)
                                {
                                    this.Invoke(new Action(() => ReportDataManage.Instance.Save(this.ConfigParam.FolderPath + "\\" + DateTime.Now.ToString("yyyyMMdd") + "\\" + this.ConfigParam.ProductName + ".xml")));
                                }
                                else
                                {
                                    this.Invoke(new Action(() => ReportDataManage.Instance.Save(this.ConfigParam.FolderPath + "\\" + this.ConfigParam.ProductName + ".xml")));
                                }
                            }
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
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
                        this.拖动label.Show();
                        break;
                    case "隐藏拖动区":
                        this.拖动label.Hide();
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

        private void readFileButton_Click(object sender, EventArgs e)
        {
            try
            {
                FileOperate fo = new FileOperate();
                string path = fo.OpenXmlFile();
                if (path != null && path.Trim().Length > 0)
                {
                    this.ConfigParam.FilePath = path;
                    this.读取文件路径textBox.Text = path;
                    this.ConfigParam.FolderPath = "";
                }
                else
                {
                    ReportDataManage.Instance.Read(path);
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
            }
        }

        private void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            try
            {
                int index = 0;
                foreach (DataGridViewRow item in dataGridView1.Rows)
                {
                    item.HeaderCell.Value = (index + 1).ToString();
                    item.Height = this.ConfigParam.Imageheight;
                    dataGridView1.Columns[0].Width = this.ConfigParam.ImageWidth;
                    index++;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void directoryButton_Click(object sender, EventArgs e)
        {
            try
            {
                FolderBrowserDialog fold = new FolderBrowserDialog();
                fold.ShowDialog();
                this.文件目录textBox.Text = fold.SelectedPath;
                this.ConfigParam.FilePath = fold.SelectedPath;
            }
            catch
            {
                MessageBox.Show(new Exception().ToString());
            }
        }

        private void 导出数据button_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog fold = new SaveFileDialog();
                fold.ShowDialog();
                string path = fold.FileName;
                using (StreamWriter sw = new StreamWriter(path, false))
                {
                    foreach (var item in ReportDataManage.Instance.ReportParamList)
                    {
                        sw.WriteLine(item.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void 保存配置button_Click(object sender, EventArgs e)
        {
            try
            {
                this.ConfigParam.Save();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void 显示类型comboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            try
            {
                List<ReportData> ledReportData = new List<ReportData>();
                if (ReportDataManage.Instance.ReportParamList == null) return;
                foreach (var item in ReportDataManage.Instance.ReportParamList)
                {
                    switch (this.显示类型comboBox.SelectedItem.ToString())
                    {
                        case "OK":
                            if (item.Result == "OK")
                                ledReportData.Add(item);
                            break;
                        case "NG":
                            if (item.Result == "NG")
                                ledReportData.Add(item);
                            break;
                        default:
                        case "All":
                            ledReportData.Add(item);
                            break;
                    }
                }
                foreach (var item in ledReportData)
                {
                    this.dataGridView1.Rows.Add(item.Image, item.X1, item.Y1, item.X2, item.Y2, item.Result);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void ReportForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                ViewConfigParamManager.Instance.ViewParamList.Remove(this._viewConfigParam); // 关闭窗体时要删除相应的对象
            }
            catch (Exception ex)
            {

            }
        }

        private void 拖动label_MouseEnter(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Hand;
            this.拖动label.BackColor = System.Drawing.SystemColors.HotTrack;
        }

        private void 拖动label_MouseDown(object sender, MouseEventArgs e)
        {
            ReportForm_MouseDown(null, null);
        }

        private void 拖动label_MouseLeave(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Default;
            this.拖动label.BackColor = System.Drawing.SystemColors.Control;
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
            if (this.WindowState == FormWindowState.Maximized)   //如果处于最大化，则还原
            {
                this.WindowState = FormWindowState.Normal;
                //Image backImage = Resources.最大化;
                //buttonMax.BackgroundImage = backImage;
            }
            else
            {
                this.WindowState = FormWindowState.Maximized;   //如果处于普通状态，则最大化
                //Image backImage = Resources.还原;
                //buttonMax.BackgroundImage = backImage;
            }
        }
        private void buttonMin_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;  //最小化
        }



        #endregion

        private void ReportForm_Move(object sender, EventArgs e)
        {
            if (this._viewConfigParam != null)
                this._viewConfigParam.Location = this.Location;
        }

        private void ReportForm_MouseDown(object sender, MouseEventArgs e)
        {
            if (this._viewConfigParam != null && this.IsLoad) // 在窗体加载后这个事件才起作用
                this._viewConfigParam.FormSize = this.Size;
        }

        private void ReportForm_Resize(object sender, EventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, WM_SYSCOMMAND, SC_MOVE + HTCAPTION, 0);
        }


    }
}

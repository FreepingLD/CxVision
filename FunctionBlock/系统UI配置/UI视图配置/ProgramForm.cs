using Common;
using FunctionBlock;
using MotionControlCard;
using Sensor;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace FunctionBlock
{
    public partial class ProgramForm : Form
    {
        private ToolForm form = null;
        private TreeViewWrapClass treeViewWrapClass;
        private string ProgramPath;
        private static ProgramForm _Instance;
        public TreeViewWrapClass TreeViewWrapClass { get => treeViewWrapClass; set => treeViewWrapClass = value; }
        private static object lockState = new object();
        public Dictionary<string, TreeViewWrapClass> ProgramDic { get; set; } // 程序字典集
        private ViewConfigParam _viewConfigParam;
        private bool IsLoad = false; // 窗体加载后，设置为true
        public ViewConfigParam ViewConfigParam { get => _viewConfigParam; set => _viewConfigParam = value; }

        public static ProgramForm Instance
        {
            get
            {
                if (_Instance == null)
                {
                    lock (lockState)
                    {
                        _Instance = new ProgramForm();
                    }
                }
                return _Instance;
            }
        }



        private ProgramForm()
        {
            InitializeComponent();
            this.ProgramDic = new Dictionary<string, TreeViewWrapClass>();
            ProgramConfigParamManager.Instance.Read();
            foreach (var item in ProgramConfigParamManager.Instance.ProgramParamList)
            {
                TabPage tabPage = new TabPage(item.Name);
                this.程序tabControl.TabPages.Add(tabPage);
                tabPage.Controls.Clear();
                TreeView treeView = new TreeView();
                treeView.Dock = DockStyle.Fill;
                treeView.ShowLines = true;
                treeView.ShowPlusMinus = false;
                treeView.ShowRootLines = false;
                treeView.Font = new Font("Microsoft Sans Serif", 10);
                tabPage.Controls.Add(treeView);
                ////////////
                ProgramDic.Add(item.Name, new TreeViewWrapClass(treeView, this));
            }
            if (this.ProgramDic.Count == 0)
                this.AddTaskbutton_Click("default", null);
            this.addContextMenu();
            this.ContextMenu = new ContextMenu();
        }

        private void ProgramForm_Load(object sender, EventArgs e)
        {
            if (this._viewConfigParam == null)
                this._viewConfigParam = new ViewConfigParam();
            this.Location = this._viewConfigParam.Location;
            this.Size = this._viewConfigParam.FormSize;
            this.IsLoad = true;
            ///////////////////////////////////////////////////////
            if (ProgramConfigParamManager.Instance.ProgramParamList != null) // 如果程序设置为自动打开，那么将在加载时自动执行
            {
                foreach (var item in ProgramConfigParamManager.Instance.ProgramParamList)
                {
                    if (item.IsAuto)
                    {
                        this.ProgramPath = item.ProgramPath;
                        if (Directory.Exists(item.ProgramPath))
                            this.OpenProgram(item.ProgramPath);
                        else
                            MessageBox.Show(item.Name + "->自动打开程序失败，指定的程序目录不存在");
                    }
                }
            }
        }

        public bool SaveProgram(string folderPath)
        {
            bool result = false;
            ProgramConfigParamManager.Instance.SetValue("ProgramPath", folderPath);
            this.ProgramPath = folderPath;
            foreach (KeyValuePair<string, TreeViewWrapClass> item in this.ProgramDic)
            {
                result = item.Value.SaveProgram(this.ProgramPath + "\\" + item.Key); // +"_" + fileName
            }
            return result;
        }
        public bool OpenProgram(string folderPath)
        {
            bool result = false;
            string fileName = folderPath;
            if (folderPath.Contains("任务"))
                fileName = new FileInfo(folderPath).DirectoryName;
            if (!Directory.Exists(fileName)) return false;
            string[] folderName = Directory.GetDirectories(fileName);
            if (ProgramConfigParamManager.Instance.ProgramParamList != null)
            {
                foreach (var item in ProgramConfigParamManager.Instance.ProgramParamList)
                {
                    item.ProgramPath = fileName;
                }
                ProgramConfigParamManager.Instance.Save();
            }
            ////////////////////////////////////////////////////
            foreach (KeyValuePair<string, TreeViewWrapClass> item in this.ProgramDic)
            {
                foreach (var item2 in folderName)
                {
                    if (item2.Contains("任务"))
                        result = item.Value.OpenProgram(item2);
                }
            }
            return result;
        }
        public void NewProgram()
        {
            string[] p = new string[this.ProgramDic.Count];
            this.ProgramDic.Keys.CopyTo(p, 0);
            foreach (var item in p)
            {
                this.ProgramDic[item].ClearTreeView();
            }
        }
        public void Run(ToolStripItem toolItem, int Count)
        {
            string[] p = new string[this.ProgramDic.Count];
            this.ProgramDic.Keys.CopyTo(p, 0);
            foreach (var item in p)
            {
                this.ProgramDic[item].RunAsyn(toolItem, Count);
            }
        }
        public void Stop()
        {
            string[] p = new string[this.ProgramDic.Count];
            this.ProgramDic.Keys.CopyTo(p, 0);
            foreach (var item in p)
            {
                this.ProgramDic[item].Stop();
            }
        }
        private void 程序tabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.程序tabControl.SelectedIndex == -1) return;
            if (form == null) return;
            try
            {
                if (ProgramDic[this.程序tabControl.SelectedTab.Text] == null) throw new ArgumentNullException("未找到指定的TreeView视图");
                this.form.TreeViewTarget = ProgramDic[this.程序tabControl.SelectedTab.Text];
            }
            catch
            {

            }
        }

        private void AddTaskbutton_Click(object sender, EventArgs e)
        {
            try
            {
                TabPage tabPage = null;
                TreeView treeView = null;
                TaskForm taskForm = new TaskForm();
                if (sender == null )
                {
                    taskForm.ShowDialog();
                }
                else
                    taskForm.content = null;
                ///////////////////////////////////////////////
                if (taskForm.content == null || taskForm.content.ToString().Trim().Length == 0)
                {
                    int count = this.程序tabControl.TabPages.Count;
                    tabPage = new TabPage("任务" + (count + 1).ToString());
                    this.程序tabControl.TabPages.Add(tabPage);
                    tabPage.Controls.Clear();
                    treeView = new TreeView();
                    treeView.Dock = DockStyle.Fill;
                    treeView.ShowLines = true;
                    treeView.ShowPlusMinus = false;
                    treeView.ShowRootLines = false;
                    treeView.Font = new Font("Microsoft Sans Serif", 10);
                    tabPage.Controls.Add(treeView);
                }
                else
                {
                    tabPage = new TabPage(taskForm.content.ToString());
                    this.程序tabControl.TabPages.Add(tabPage);
                    treeView = new TreeView();
                    treeView.Dock = DockStyle.Fill;
                    treeView.ShowLines = true;
                    treeView.ShowPlusMinus = false;
                    treeView.ShowRootLines = false;
                    treeView.Font = new Font("Microsoft Sans Serif", 10);
                    tabPage.Controls.Add(treeView);
                }
                if (tabPage != null && treeView != null)
                {
                    if (ProgramDic.ContainsKey(tabPage.Text))
                    {
                        MessageBox.Show("不能添加具有相同名称的任务");
                        this.程序tabControl.TabPages.Remove(tabPage);
                    }
                    else
                    {
                        ProgramConfigParamManager.Instance.ProgramParamList.Add(new ProgramConfigParam(tabPage.Text));
                        ProgramDic.Add(tabPage.Text, new TreeViewWrapClass(treeView, this));
                    }
                    ProgramConfigParamManager.Instance.Save(); // 添加操作后保存
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("添加任务失败" + ex.ToString());
            }
        }

        private void DeletTaskbutton_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.程序tabControl.SelectedIndex == -1) return;
                if (ProgramDic.ContainsKey(this.程序tabControl.SelectedTab.Text))
                    ProgramDic.Remove(this.程序tabControl.SelectedTab.Text);
                if (ProgramConfigParamManager.Instance.GetParam(this.程序tabControl.SelectedTab.Text) != null)
                    ProgramConfigParamManager.Instance.ProgramParamList.Remove(ProgramConfigParamManager.Instance.GetParam(this.程序tabControl.SelectedTab.Text));
                ProgramConfigParamManager.Instance.Save(); // 删除操作后保存
                this.程序tabControl.TabPages.Remove(this.程序tabControl.SelectedTab);
                this.程序tabControl.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show("添加任务失败" + ex.ToString());
            }
        }

        private void ProgramForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                ViewConfigParamManager.Instance.ViewParamList.Remove(this._viewConfigParam); // 关闭窗体时要删除相应的对象
                this.treeViewWrapClass?.Uinit();
                _Instance = null;
            }
            catch
            {
                LoggerHelper.Error("程序配置参数保存失败");
            }
        }

        private void 程序toolStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            try
            {
                switch (e.ClickedItem.Text.Trim())
                {
                    case "添加(A)":
                        //AddTaskbutton_Click(null, null);
                        break;
                    case "删除(D)":
                        //DeletTaskbutton_Click(null, null);
                        break;
                    case "编辑(E)":
                        TaskForm taskForm = new TaskForm();
                        taskForm.ShowDialog();
                        if (taskForm.content != null && taskForm.content.ToString().Trim().Length > 0)
                        {
                            ProgramConfigParam programConfigParam = ProgramConfigParamManager.Instance.GetParam(this.程序tabControl.SelectedTab.Text);
                            if (programConfigParam != null)
                                programConfigParam.Name = taskForm.content.ToString();
                            this.程序tabControl.SelectedTab.Text = taskForm.content.ToString();
                            ProgramConfigParamManager.Instance.Save();
                        }
                        break;
                    case "工具(T)":
                        if (ProgramDic[this.程序tabControl.SelectedTab.Text] == null) throw new ArgumentNullException("未找到指定的TreeView视图");
                        form = new ToolForm(ProgramDic[this.程序tabControl.SelectedTab.Text]);
                        form.Owner = this;
                        form.Show();
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
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

        private void ProgramForm_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, WM_SYSCOMMAND, SC_MOVE + HTCAPTION, 0);
        }

        private void ProgramForm_Move(object sender, EventArgs e)
        {
            if (this._viewConfigParam != null)
                this._viewConfigParam.Location = this.Location;
        }

        private void ProgramForm_Resize(object sender, EventArgs e)
        {
            if (this._viewConfigParam != null && this.IsLoad) // 在窗体加载后这个事件才起作用
                this._viewConfigParam.FormSize = this.Size;
        }

        private void 拖动label_MouseDown(object sender, MouseEventArgs e)
        {
            ProgramForm_MouseDown(null, null);
        }

        private void 拖动label_MouseEnter(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Hand;
            this.拖动label.BackColor = System.Drawing.SystemColors.HotTrack;
        }

        private void 拖动label_MouseLeave(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Default;
            this.拖动label.BackColor = System.Drawing.SystemColors.Control;
        }

        private void 程序toolStrip_MouseDown(object sender, MouseEventArgs e)
        {
            ProgramForm_MouseDown(null, null);
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
                        this.拖动label.Enabled = true; ;
                        break;
                    case "隐藏拖动区":
                        this.拖动label.Enabled = false;
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



    }
}

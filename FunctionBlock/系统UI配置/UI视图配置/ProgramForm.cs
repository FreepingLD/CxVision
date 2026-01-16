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
using System.Runtime.InteropServices.ComTypes;
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
        public string ProgramPath { get; set; }
        private static ProgramForm _Instance;
        public TreeViewWrapClass TreeViewWrapClass { get => treeViewWrapClass; set => treeViewWrapClass = value; }
        private static object lockState = new object();
        public Dictionary<string, TreeViewWrapClass> ProgramDic { get; set; } // 程序字典集
        private ViewConfigParam _viewConfigParam;
        private bool IsLoad = false; // 窗体加载后，设置为true
        public ViewConfigParam ViewConfigParam { get => _viewConfigParam; set => _viewConfigParam = value; }
        private string _acqSourceName = "NONE";
        public string AcqSourceName { get => _acqSourceName; set => _acqSourceName = value; }

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
                tabPage.DoubleBuffere(true);
                this.程序tabControl.TabPages.Add(tabPage);
                tabPage.Controls.Clear();
                TreeView treeView = new TreeView();
                treeView.Dock = DockStyle.Fill;
                treeView.ShowLines = true;
                treeView.ShowPlusMinus = false;
                treeView.ShowRootLines = false;
                treeView.Font = new Font("Microsoft Sans Serif", 12);
                treeView.DoubleBuffere(true);
                // treeView.ImageList = this.imageList1;  // 在这里添加图像图标
                tabPage.Controls.Add(treeView);
                ////////////
                if (!ProgramDic.ContainsKey(item.Name))
                    ProgramDic.Add(item.Name, new TreeViewWrapClass(treeView, this));
            }
            if (this.ProgramDic.Count == 0)
                this.AddTaskbutton_Click("default", null);
            this.addContextMenu();
            this.ContextMenu = new ContextMenu();
            ///////////////////////
            UserLoginParamManager.Instance.UserChange += new EventHandler(this.UserChange_Event);
        }

        private void ProgramForm_Load(object sender, EventArgs e)
        {
            if (this._viewConfigParam == null)
                this._viewConfigParam = new ViewConfigParam();
            this.Location = this._viewConfigParam.Location;
            this.Size = this._viewConfigParam.FormSize;
            this.IsLoad = true;
            this.程序tabControl.DoubleBuffere(true);
            this.DoubleBuffered = true;
            ///////////////////////////////////////////////////////
            if (ProgramConfigParamManager.Instance.ProgramParamList != null) // 如果程序设置为自动打开，那么将在加载时自动执行
            {
                foreach (var item in ProgramConfigParamManager.Instance.ProgramParamList)
                {
                    if (item.ProgramPath == null) continue;
                    this.ProgramPath = new FileInfo(item.ProgramPath).DirectoryName;
                    if (Directory.Exists(item.ProgramPath))
                        this.OpenProgram(item.ProgramPath);
                    else
                        new UserMessageForm().ShowDialog(item.Name + "->自动打开程序失败，指定的程序目录不存在");
                    break;
                }
            }
            //////////  同步夹爪参数 //////////
            //ReadJawParam();
            ////////////////////////////////////////
            this.采集源comboBox.DataSource = FunctionBlock.AcqSourceManage.Instance.GetAcqSourceName();
            if (AcqSourceManage.Instance.GetAcqSourceName().Length > 1)
                this._acqSourceName = AcqSourceManage.Instance.GetAcqSourceName()[1];
            this.采集源comboBox.Text = this._acqSourceName;
        }
        public void UserChange_Event(object sender, EventArgs e)
        {
            try
            {
                UserLoginParam loginParam = sender as UserLoginParam;
                switch (loginParam.User)
                {
                    case enUserName.操作员:
                        this.添加toolStripButton.Enabled = false;
                        this.删除toolStripButton.Enabled = false;
                        this.编辑toolStripButton.Enabled = false;
                        break;
                    case enUserName.工程师:
                        this.添加toolStripButton.Enabled = false;
                        this.删除toolStripButton.Enabled = false;
                        this.编辑toolStripButton.Enabled = false;
                        break;
                    case enUserName.开发人员:
                        this.添加toolStripButton.Enabled = true;
                        this.删除toolStripButton.Enabled = true;
                        this.编辑toolStripButton.Enabled = true;
                        break;
                }
            }
            catch
            {
            }
        }

        public bool SaveProgram(string folderPath = null)
        {
            bool result = true;
            foreach (KeyValuePair<string, TreeViewWrapClass> item in this.ProgramDic)
            {
                string savePath = ProgramConfigParamManager.Instance.GetParam(item.Key)?.ProgramPath; // = this.ProgramPath + "\\" + item.Key;
                if (savePath != null)
                {
                    if (folderPath != null)
                    {
                        this.ProgramPath = folderPath;
                        result = result && item.Value.SaveProgram(folderPath + "\\" + item.Key);
                        if (ProgramConfigParamManager.Instance.ContainKey(item.Key)) // 另存为时也更新路径
                        {
                            ProgramConfigParamManager.Instance.GetParam(item.Key).ProgramPath = folderPath + "\\" + item.Key;
                        }
                        else
                        {
                            ProgramConfigParam config = new ProgramConfigParam(item.Key);
                            config.IsAuto = true;
                            config.ProgramPath = folderPath + "\\" + item.Key;
                            ProgramConfigParamManager.Instance.ProgramParamList.Add(config);
                        }
                    }
                    else
                        result = result && item.Value.SaveProgram(savePath);
                }
                else
                {
                    this.ProgramPath = folderPath;
                    if (ProgramConfigParamManager.Instance.ContainKey(item.Key)) // 另存为时也更新路径
                    {
                        ProgramConfigParamManager.Instance.GetParam(item.Key).ProgramPath = folderPath + "\\" + item.Key;
                    }
                    else
                    {
                        ProgramConfigParam config = new ProgramConfigParam(item.Key);
                        config.IsAuto = true;
                        config.ProgramPath = folderPath + "\\" + item.Key;
                        ProgramConfigParamManager.Instance.ProgramParamList.Add(config);
                    }
                    result = result && item.Value.SaveProgram(folderPath + "\\" + item.Key);
                }
            }
            //ProgramConfigParamManager.Instance.Save();
            //////////  同步夹爪参数 //////////
            ////////////////////  每次保存发送一次标签更新  //////////////////// 
            foreach (var item2 in AcqSourceManage.Instance.AcqSourceList)
            {
                string[] lable = this.GetLableName((int)item2.CoordSysName);
                if (lable != null && lable.Length > 0)
                {
                    CommunicationConfigParamManger.Instance.WriteValue(item2.CoordSysName, enCommunicationCommand.Lable, string.Join("|", lable));
                    CommunicationConfigParamManger.Instance.WriteValue(item2.CoordSysName, enCommunicationCommand.LableLength, string.Join("|", lable).Length * 2);
                    CommunicationConfigParamManger.Instance.WriteValue(item2.CoordSysName, enCommunicationCommand.ResultToPlc, 1);
                    CommunicationConfigParamManger.Instance.WriteValue(item2.CoordSysName, enCommunicationCommand.ResultToSocket, "OK");
                    CommunicationConfigParamManger.Instance.WriteValue(item2.CoordSysName, enCommunicationCommand.LableTrigger, 1);
                    LoggerHelper.Info($"标签内容:{string.Join("|", lable)}", item2.Sensor?.Name);
                    LoggerHelper.Info("发送标签成功", item2.Sensor?.Name);
                }
                else
                {
                    //CommunicationConfigParamManger.Instance.WriteValue(item2.CoordSysName, enCommunicationCommand.LableLength, 0);
                    CommunicationConfigParamManger.Instance.WriteValue(item2.CoordSysName, enCommunicationCommand.ResultToPlc, 2);
                    CommunicationConfigParamManger.Instance.WriteValue(item2.CoordSysName, enCommunicationCommand.ResultToSocket, "NG");
                    CommunicationConfigParamManger.Instance.WriteValue(item2.CoordSysName, enCommunicationCommand.LableTrigger, 1);
                    LoggerHelper.Error($"标签内容为空或长度为0", item2.Sensor?.Name);
                    LoggerHelper.Error("发送标签失败", item2.Sensor?.Name);
                }
            }

            return result;
        }

        public bool OpenProgram(string folderPath)
        {
            bool result = false;
            string fileName = folderPath;
            this.ProgramPath = folderPath;
            if (!Directory.Exists(folderPath))
            {
                new UserMessageForm().ShowDialog(fileName + "路径不存在(File Path No Exists!)");
                return false;
            }
            ///////////////////////////////////////////////////////////////////
            if (folderPath.Contains("任务")) // 只打开对应的任务
            {
                this.ProgramPath = folderPath;
                fileName = new FileInfo(folderPath).Name;
                this.ProgramDic[this.程序tabControl.SelectedTab.Text]?.OpenProgram(folderPath);
                /// 替换对应路经
                if (ProgramConfigParamManager.Instance.ContainKey(this.程序tabControl.SelectedTab.Text))
                {
                    ProgramConfigParamManager.Instance.GetParam(this.程序tabControl.SelectedTab.Text).ProgramPath = folderPath;
                }
                else
                {
                    ProgramConfigParam config = new ProgramConfigParam(this.程序tabControl.SelectedTab.Text);
                    config.IsAuto = true;
                    config.ProgramPath = folderPath;
                    ProgramConfigParamManager.Instance.ProgramParamList.Add(config);
                }
            }
            else // 打开所有任务
            {
                string[] folderName = Directory.GetDirectories(folderPath);
                foreach (var item2 in folderName)
                {
                    this.ProgramPath = item2;
                    string key = item2.Replace(folderPath + "\\", "");
                    if (!key.Contains("任务")) continue;
                    if (this.ProgramDic.ContainsKey(key))
                    {
                        result = (bool)this.ProgramDic[key]?.OpenProgram(item2);
                        /// 替换对应路经
                        if (ProgramConfigParamManager.Instance.ContainKey(this.程序tabControl.SelectedTab.Text))
                        {
                            ProgramConfigParamManager.Instance.GetParam(this.程序tabControl.SelectedTab.Text).ProgramPath = folderPath + "\\" + key;
                        }
                        else
                        {
                            ProgramConfigParam config = new ProgramConfigParam(this.程序tabControl.SelectedTab.Text);
                            config.IsAuto = true;
                            config.ProgramPath = item2;
                            ProgramConfigParamManager.Instance.ProgramParamList.Add(config);
                        }
                    }
                    else
                        new UserMessageForm().ShowDialog($"程序面板中不包含{key}", "警告!!!");
                    if (!result)
                        new UserMessageForm().ShowDialog("程序面板不包含名称为 " + key + "的任务");
                }
            }           
            
            ////////////////////  每次保存发送一次标签更新  //////////////////// 
            foreach (var item2 in AcqSourceManage.Instance.AcqSourceList)
            {
                string[] lable = this.GetLableName((int)item2.CoordSysName);
                if (lable != null && lable.Length > 0)
                {
                    CommunicationConfigParamManger.Instance.WriteValue(item2.CoordSysName, enCommunicationCommand.Lable, string.Join("|", lable));
                    CommunicationConfigParamManger.Instance.WriteValue(item2.CoordSysName, enCommunicationCommand.LableLength, string.Join("|", lable).Length * 2);
                    CommunicationConfigParamManger.Instance.WriteValue(item2.CoordSysName, enCommunicationCommand.ResultToPlc, 1);
                    CommunicationConfigParamManger.Instance.WriteValue(item2.CoordSysName, enCommunicationCommand.ResultToSocket, "OK");
                    CommunicationConfigParamManger.Instance.WriteValue(item2.CoordSysName, enCommunicationCommand.LableTrigger, 1);
                    LoggerHelper.Info($"标签内容:{string.Join("|", lable)}", item2.Sensor?.Name);
                    LoggerHelper.Info("发送标签成功", item2.Sensor?.Name);
                }
                else
                {
                    //CommunicationConfigParamManger.Instance.WriteValue(item2.CoordSysName, enCommunicationCommand.LableLength, 0);
                    CommunicationConfigParamManger.Instance.WriteValue(item2.CoordSysName, enCommunicationCommand.ResultToPlc, 2);
                    CommunicationConfigParamManger.Instance.WriteValue(item2.CoordSysName, enCommunicationCommand.ResultToSocket, "NG");
                    CommunicationConfigParamManger.Instance.WriteValue(item2.CoordSysName, enCommunicationCommand.LableTrigger, 1);
                    LoggerHelper.Error($"标签内容为空或长度为0", item2.Sensor?.Name);
                    LoggerHelper.Error("发送标签失败", item2.Sensor?.Name);
                }
            }
            
            return result;
        }

        /// <summary>
        /// 获取标签名称
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public string[] GetLableName(int index)
        {
            string[] lables = new string[0];
            List<string> listLable = new List<string>();
            List<TreeNode> listTreeNode = new List<TreeNode>();
            List<TreeNode> listToolNode = new List<TreeNode>();
            // 获取面板上的节点
            foreach (var item in this.ProgramDic.Values)
            {
                listTreeNode.AddRange(item.GetTreeViewNodeTag());
            }
            // 获取流程单元下的标签
            foreach (TreeNode item in listTreeNode)
            {
                switch (item.Tag?.GetType().Name)
                {
                    case nameof(JobUnit):
                        BindingList<PlcCommunicateInfo> plcInfo = ((BaseFunction)item.Tag).ResultInfo as BindingList<PlcCommunicateInfo>;
                        if (plcInfo.Count > 0 && (int)plcInfo[0].CoordSysName == index)
                        {
                            foreach (TreeNode node in item.Nodes)
                            {
                                switch (node.Tag?.GetType().Name)
                                {
                                    case nameof(UserLable):
                                        listLable.Add(node.Text);
                                        break;
                                    default:
                                        if (node.Name.Contains("Tool"))
                                            listToolNode.Add(node);
                                        break;
                                }
                            }
                        }
                        else
                            continue;
                        break;
                }
            }
            // 获取特征定位下的标签
            foreach (TreeNode item in listToolNode)
            {
                foreach (TreeNode node in item.Nodes)
                {
                    switch (node?.Tag?.GetType().Name)
                    {
                        case nameof(UserLable):
                            listLable.Add($"{item.Text}.{node.Text}");
                            break;
                        default:
                            break;
                    }
                }
            }
            lables = listLable.ToArray();
            return lables;
        }

        private void ReadJawParam()
        {
            if (ProgramConfigParamManager.Instance.ProgramParamList.Count > 0)
            {
                if (SystemParamManager.Instance.SysConfigParam.IsSynJawParam)
                {
                    string savePath = ProgramConfigParamManager.Instance.ProgramParamList[0].ProgramPath;
                    string key = ProgramConfigParamManager.Instance.ProgramParamList[0].Name;
                    if (savePath.Contains("任务"))
                    {
                        string directName = new FileInfo(savePath.Replace("\\" + key, "")).Name;
                        RobotJawParaManager.Instance.Read(new FileInfo(directName).Name);
                    }
                    else
                        RobotJawParaManager.Instance.Read(new FileInfo(savePath).Name);
                }
            }
        }

        private void SaveJawParam()
        {
            if (ProgramConfigParamManager.Instance.ProgramParamList.Count > 0)
            {
                if (SystemParamManager.Instance.SysConfigParam.IsSynJawParam)
                {
                    string savePath = ProgramConfigParamManager.Instance.ProgramParamList[0].ProgramPath;
                    string key = ProgramConfigParamManager.Instance.ProgramParamList[0].Name;
                    if (savePath.Contains("任务"))
                    {
                        string directName = new FileInfo(savePath.Replace("\\" + key, "")).Name;
                        RobotJawParaManager.Instance.Save(new FileInfo(directName).Name);
                    }
                    else
                        RobotJawParaManager.Instance.Save(new FileInfo(savePath).Name);
                }
            }
        }

        public void NewProgram()
        {
            this.ProgramPath = null;
            ProgramConfigParamManager.Instance.ProgramParamList?.Clear();
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
                if (sender == null)
                {
                    taskForm.ShowDialog();
                }
                else
                    taskForm.content = null;
                if (taskForm.content == null) return;
                if (ProgramDic.ContainsKey(taskForm.content.ToString().Trim()))
                {
                    new UserMessageForm().ShowDialog("不能添加具有相同名称的任务");
                    return;
                }
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
                    if (!taskForm.content.ToString().Contains("任务"))
                        taskForm.content = "任务-" + taskForm.content;
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
                        new UserMessageForm().ShowDialog("不能添加具有相同名称的任务");
                        this.程序tabControl.TabPages.Remove(tabPage);
                    }
                    else
                    {
                        ProgramConfigParamManager.Instance.ProgramParamList.Add(new ProgramConfigParam(tabPage.Text));
                        foreach (var item in ProgramConfigParamManager.Instance.ProgramParamList)
                        {
                            item.ProgramPath = null;
                        }
                        this.ProgramPath = null;
                        ProgramDic.Add(tabPage.Text, new TreeViewWrapClass(treeView, this));
                    }
                    ProgramConfigParamManager.Instance.Save(); // 添加操作后保存
                }
            }
            catch (Exception ex)
            {
                new UserMessageForm().ShowDialog("添加任务失败" + ex.ToString());
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
                {
                    ProgramConfigParamManager.Instance.ProgramParamList.Remove(ProgramConfigParamManager.Instance.GetParam(this.程序tabControl.SelectedTab.Text));
                    foreach (var item in ProgramConfigParamManager.Instance.ProgramParamList)
                    {
                        item.ProgramPath = null;
                    }
                    this.ProgramPath = null;
                }
                ProgramConfigParamManager.Instance.Save(); // 删除操作后保存
                this.程序tabControl.TabPages.Remove(this.程序tabControl.SelectedTab);
                this.程序tabControl.Refresh();
            }
            catch (Exception ex)
            {
                new UserMessageForm().ShowDialog("删除任务失败" + ex.ToString());
            }
        }

        private void ProgramForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                UserLoginParamManager.Instance.UserChange -= new EventHandler(this.UserChange_Event);
                //ViewConfigParamManager.Instance.ViewParamList.Remove(this._viewConfigParam); // 关闭窗体时要删除相应的对象
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
                        AddTaskbutton_Click(null, null);
                        break;
                    case "删除(D)":
                        DeletTaskbutton_Click(null, null);
                        break;
                    case "编辑(E)":
                        TaskForm taskForm = new TaskForm();
                        if (SystemParamManager.Instance.SysConfigParam.IsFormTopMost)
                        {
                            taskForm.TopMost = true;
                            taskForm.ShowInTaskbar = true;
                        }
                        taskForm.content = this.程序tabControl.SelectedTab.Text;
                        taskForm.ShowDialog();
                        if (taskForm.content != null && taskForm.content.ToString().Trim().Length > 0)
                        {
                            if (!taskForm.content.ToString().Contains("任务"))
                                taskForm.content = "任务" + taskForm.content;
                            ProgramConfigParam programConfigParam = ProgramConfigParamManager.Instance.GetParam(this.程序tabControl.SelectedTab.Text);
                            if (programConfigParam != null)
                                programConfigParam.Name = taskForm.content.ToString();
                            if (!this.ProgramDic.ContainsKey(taskForm.content.ToString()))
                            {
                                this.ProgramDic.Add(taskForm.content.ToString(), this.ProgramDic[this.程序tabControl.SelectedTab.Text]);
                                this.ProgramDic.Remove(this.程序tabControl.SelectedTab.Text);
                            }
                            this.程序tabControl.SelectedTab.Text = taskForm.content.ToString();
                            ProgramConfigParamManager.Instance.Save();
                        }
                        break;
                    case "工具(T)":
                        if (ProgramDic[this.程序tabControl.SelectedTab.Text] == null)
                        {
                            new UserMessageForm().ShowDialog("未找到指定的TreeView视图");
                            // throw new ArgumentNullException();
                            return;
                        }
                        form = new ToolForm(ProgramDic[this.程序tabControl.SelectedTab.Text]);
                        form.Owner = this;
                        form.Show();
                        break;
                }
            }
            catch (Exception ex)
            {
               new Common.UserMessageForm(ex.ToString()).ShowDialog();;
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
                    if (m.Msg == 0x0014) // 禁掉清除背景消息WM_ERASEBKGND
                        return;
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

        private void 采集源comboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            try
            {
                this._acqSourceName = this.采集源comboBox.Text;
            }
            catch (Exception ex)
            {
               new Common.UserMessageForm(ex.ToString()).ShowDialog();;
            }
        }


    }
}

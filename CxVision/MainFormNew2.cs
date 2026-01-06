using Common;
using FunctionBlock;
using HalconDotNet;
using Light;
using MotionControlCard;
using Sensor;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Interop;

namespace CxVision
{
    public partial class MainFormNew2 : Form
    {
        private Form programForm;
        private SensorManage sensorlist = new SensorManage();
        private MotionCardManage card = new MotionCardManage();
        private LightConnectManage light = new LightConnectManage();
        private string programPath = ""; // 程序文件路径
        private List<Form> ListForm = new List<Form>();
        private System.Threading.Timer timer;
        private bool IsLoad = false;
        private SocketBase _socket;
        private Stopwatch stopwatch = new Stopwatch();

        public MainFormNew2()
        {
            InitializeComponent();
            //this.TopLevel = false;
            //this.TopMost = false;
            HalconDotNet.HSystem.ResetObjDb(50000, 50000, 0); // 初始化Halcon系统
            //this.InitScript(); // 初始化脚本引擎
            // 读取配置文件
            SystemParamManager.Instance.SysConfigParam.IsAutoRun = false;
            GlobalVariable.pConfig = new Common.ParamConfig().ReadParamConfig(); //Application.StartupPath + "\\" + "ParamConfig.txt"
            CommandConfigManger.Instance.Read(); // 读取命令配置
            ViewConfigParamManager.Instance.Read();
            SocketConnectManager.Instance.InitSocket();
            CoordSysConfigParamManger.Instance.Read();
            CommunicationConfigParamManger.Instance.Read();
            RobotJawParaManager.Instance.Read();
            if (SystemParamManager.Instance.SysConfigParam == null)
                SystemParamManager.Instance.Read();
            FlawClassManage.Instance.Read();
            HWindowManage.Init();// 初始化视图窗口，添加一个自动功能 
            ////////////////////////////////////////////////////////////////////////////
            this.sensorlist.Connect();
            this.card.Connect();
            this.light.Connect();
            // 初始化采集源
            AcqSourceManage.Instance.Read();
            CPUMonitor.Instance.Init();
            //// 读取用户
            UserLoginParamManager.Instance.Read();
            /////////////////////////////////////
            this.MaximumSize = new Size(Screen.PrimaryScreen.WorkingArea.Width, Screen.PrimaryScreen.WorkingArea.Height);
            this.WindowState = FormWindowState.Maximized;
            this.语言toolStripComboBox.Text = SystemParamManager.Instance.SysConfigParam.Language;// 语言设置
            switch (SystemParamManager.Instance.SysConfigParam.Language)
            {
                case "zh-CN":
                    this.语言toolStripComboBox.Text = "中文(Chinise)";
                    break;
                case "en-US":
                    this.语言toolStripComboBox.Text = "英文(English)";
                    break;
                default:
                    break;
            }
            //Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(SystemParamManager.Instance.SysConfigParam.Language);
            //if (new UserMessageForm().ShowDialog("机台上电后必需执行机台回零,如已回零可忽略", "机台回零", MessageBoxButtons.OKCancel) == DialogResult.OK)
            //{
            //    if (MotionCardManage.CurrentCard != null)
            //        MotionCardManage.CurrentCard.MultyAxisHome(enAxisName.XY轴, 10);
            //    else
            //        new UserMessageForm().ShowDialog("运动控制卡打开失败");
            //}
            ViewConfigParamManager.Instance.Init(this.视图TabControl);
            //SocketMessageThread.Instance.Init();
            //////////////////////////////////////////////////////
            foreach (TabPage item in this.视图TabControl.TabPages)
            {
                foreach (var form in item.Controls)
                {
                    if (form is Form)
                        this.ListForm.Add(form as Form);
                }
            }
            /////////////////
            this.timer = new System.Threading.Timer(this.DateTimeMonitor, null, 1000, 2000);
            ////////
            this.视图TabControl.DoubleBuffere(true);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            Rectangle rect = Screen.GetWorkingArea(this);
            ScreenManager.Instance.ScreenParam.SetScaleParam(rect.Width, rect.Height);
            foreach (TabPage item in this.视图TabControl.TabPages)
            {
                this.视图TabControl.SelectedTab = item;  // 激活每一个Page页
            }
            this.视图TabControl.SelectedIndex = 0;
            ////////////////////////////////////////////
            //UserLoginParamManager.Instance.LoginParam.UserChange += new EventHandler(this.UserChange_Event);
            UserLoginParamManager.Instance.UserChange += new EventHandler(this.UserChange_Event);
            UserLoginParamManager.Instance.CurrentUser = enUserName.操作员; // 读取成功后需要触发一次事件 
            AutoRunThreadPlc.Instance.RecipeInfo += new RecipeEventHandler(this.Recipe_Event);
            this.addLableContextMenu();
            this.IsLoad = true;
            if (SystemParamManager.Instance.SysConfigParam.ProjectName == null || SystemParamManager.Instance.SysConfigParam.ProjectName.Length == 0)
                this.项目名称label.Text = "项目名称:";
            else
                this.项目名称label.Text = SystemParamManager.Instance.SysConfigParam.ProjectName;
            ////////////
            //this._socket = Common.SocketConnectManager.Instance.GetSocket(SystemParamManager.Instance.SysConfigParam.GlobalSocketName);
            //if (this._socket != null)
            //    this._socket.SocketMessage += new SocketMessageEventHandler(this.SocketMessage_Receiver);
        }

        /// <summary>
        /// 初始化脚本引擎
        /// </summary>
        public void InitScript()
        {
            //dynamic loadcode = CSScript.Evaluator.LoadCode(
            //    @"using System;
            //      using System.Text;
            //      public class ScriptCC
            //        {
            //             public void LoadCode(string greeting)
            //               {
            //                  Console.WriteLine(""LoadCode:"" + greeting);
            //                }
            //     }");
        }

        private void addContextMenu()
        {
            ContextMenuStrip ContextMenuStrip1 = new ContextMenuStrip();
            // 添加右键菜单 
            ToolStripItem[] items = new ToolStripMenuItem[]
            {
                new ToolStripMenuItem("添加"),
                 new ToolStripMenuItem("移除"),
                  new ToolStripMenuItem("左移"),
                   new ToolStripMenuItem("右移"),
                    new ToolStripMenuItem("重命名"),
            };
            ContextMenuStrip1.Items.AddRange(items); // 在滑赋值前不能调用 
            ContextMenuStrip1.ItemClicked += new ToolStripItemClickedEventHandler(tabControl1ContextMenuStrip_ItemClicked);
            this.视图TabControl.ContextMenuStrip = ContextMenuStrip1;
        }
        private void tabControl1ContextMenuStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            string name = e.ClickedItem.Text;
            try
            {
                ((ContextMenuStrip)sender).Close();
                switch (name)
                {
                    default:
                        break;
                    case "添加":
                        AddViewForm addViewForm = new AddViewForm("TabPage_视图页面");
                        addViewForm.ShowDialog();
                        if (addViewForm.IsCancel) return;
                        ViewConfigParamManager.Instance.AddTabPage(this.视图TabControl, addViewForm);
                        break;
                    ///////////////////////////////////////////////          
                    case "移除":
                        ViewConfigParamManager.Instance.RemoveTabPage(this.视图TabControl, this.视图TabControl.SelectedTab);
                        break;
                    ///////////////////////////////////////////////    
                    case "左移":
                        ViewConfigParamManager.Instance.LeftShiftTabPage(this.视图TabControl, this.视图TabControl.SelectedTab);
                        break;
                    ///////////////////////////////////////////////    
                    case "右移":
                        ViewConfigParamManager.Instance.RightShiftTabPage(this.视图TabControl, this.视图TabControl.SelectedTab);
                        break;
                    case "重命名":
                        ViewConfigParam configParam = ViewConfigParamManager.Instance.GetViewConfigParam(this.视图TabControl.SelectedTab.Name);
                        if (configParam == null)
                        {
                            configParam = ViewConfigParamManager.Instance.GetViewConfigParam("tabPage" + this.视图TabControl.SelectedTab.Text);
                            if (configParam == null)
                                return;
                        }
                        RenameForm renameForm = new RenameForm(this.视图TabControl.SelectedTab.Text);
                        DialogResult dialogResult = renameForm.ShowDialog();
                        if (dialogResult == DialogResult.OK)
                        {
                            if (renameForm.ReName != null && renameForm.ReName.Length > 0)
                            {
                                this.视图TabControl.SelectedTab.Text = renameForm.ReName;
                                configParam.Text = renameForm.ReName;  // tabPage: 的Text属性
                                configParam.ViewName = "tabPage" + renameForm.ReName;
                                ///// 将所有视图参数的 ContainerName 名称，指定为当前的选择项的名称
                                foreach (var item in ViewConfigParamManager.Instance.ViewParamList)
                                {
                                    if (item.ContainerName == this.视图TabControl.SelectedTab.Name) // ||  item.ContainerName == "tabPageUpAlignCam"
                                        item.ContainerName = configParam.ViewName;
                                }
                                this.视图TabControl.SelectedTab.Name = configParam.ViewName;
                            }
                        }
                        break;
                        ///////////////////////////////////////////////    
                }
            }
            catch (Exception ex)
            {
                new UserMessageForm(ex.ToString()).ShowDialog();
            }
        }
        public void UserChange_Event(object sender, EventArgs e)
        {
            try
            {
                UserLoginParam loginParam = sender as UserLoginParam;
                switch (loginParam.User)
                {
                    case enUserName.操作员:
                        this.视图TabControl.ContextMenuStrip = null;
                        this.工程配置toolStripButton.Enabled = true;
                        this.连接配置toolStripButton.Enabled = true;
                        //this.程序tabPage.en
                        break;
                    case enUserName.工程师:
                        this.视图TabControl.ContextMenuStrip = null;
                        this.工程配置toolStripButton.Enabled = true;
                        this.连接配置toolStripButton.Enabled = true;
                        break;
                    case enUserName.开发人员:
                        this.addContextMenu();
                        this.工程配置toolStripButton.Enabled = true;
                        this.连接配置toolStripButton.Enabled = true;
                        break;
                }
            }
            catch
            {
            }
        }
        public void Recipe_Event(RecipeEventArgs e)
        {
            string function = "";
            try
            {
                string FunctionNo = CommunicationConfigParamManger.Instance.ReadValue(e.CoordSysName, enCommunicationCommand.FunctionNo).ToString();
                string path = CommunicationConfigParamManger.Instance.ReadValue(e.CoordSysName, enCommunicationCommand.ProgramNo).ToString();
                function = FunctionNo;
                switch (FunctionNo)
                {
                    case "Run":
                    case "run":
                        LoggerHelper.Info("运行开始");
                        this.Invoke(new Action(() =>
                        {
                            ToolStripItem toolStripMenu2 = new ToolStripMenuItem();
                            toolStripMenu2.Name = "联机运行toolStripButton";
                            this.运行工具条toolStrip_ItemClicked_1(null, new ToolStripItemClickedEventArgs(toolStripMenu2));
                            CommunicationConfigParamManger.Instance.WriteValue(e.CoordSysName, enCommunicationCommand.ResultToPlc, 1);
                            CommunicationConfigParamManger.Instance.WriteValue(e.CoordSysName, enCommunicationCommand.ResultToSocket, "OK");
                            CommunicationConfigParamManger.Instance.WriteValue(e.CoordSysName, enCommunicationCommand.TriggerToPlc, 1);
                        }));
                        break;

                    case "Stop":
                    case "stop":
                        LoggerHelper.Info("运行停止");
                        this.Invoke(new Action(() =>
                        {
                            ToolStripItem toolStripMenu = new ToolStripMenuItem();
                            toolStripMenu.Name = "断开连机toolStripButton";
                            this.运行工具条toolStrip_ItemClicked_1(null, new ToolStripItemClickedEventArgs(toolStripMenu));
                            CommunicationConfigParamManger.Instance.WriteValue(e.CoordSysName, enCommunicationCommand.ResultToPlc, 1);
                            CommunicationConfigParamManger.Instance.WriteValue(e.CoordSysName, enCommunicationCommand.ResultToSocket, "OK");
                            CommunicationConfigParamManger.Instance.WriteValue(e.CoordSysName, enCommunicationCommand.TriggerToPlc, 1);
                        }));
                        break;

                    case "New":
                    case "new":
                        LoggerHelper.Info("配方新建");
                        this.programPath = "";
                        ProgramForm.Instance.NewProgram();
                        GlobalProgram.ProgramItems.Clear();
                        CommunicationConfigParamManger.Instance.WriteValue(e.CoordSysName, enCommunicationCommand.ResultToPlc, 1);
                        CommunicationConfigParamManger.Instance.WriteValue(e.CoordSysName, enCommunicationCommand.ResultToSocket, "OK");
                        CommunicationConfigParamManger.Instance.WriteValue(e.CoordSysName, enCommunicationCommand.TriggerToPlc, 1);
                        break;

                    case "Open":
                    case "open":
                        LoggerHelper.Info("配方打开");
                        this.programPath = path;
                        if (ProgramForm.Instance.OpenProgram(this.programPath))
                        {
                            CommunicationConfigParamManger.Instance.WriteValue(e.CoordSysName, enCommunicationCommand.ResultToPlc, 1);
                            CommunicationConfigParamManger.Instance.WriteValue(e.CoordSysName, enCommunicationCommand.ResultToSocket, "OK");
                            CommunicationConfigParamManger.Instance.WriteValue(e.CoordSysName, enCommunicationCommand.TriggerToPlc, 1);
                        }
                        else
                        {
                            CommunicationConfigParamManger.Instance.WriteValue(e.CoordSysName, enCommunicationCommand.ResultToPlc, 2);
                            CommunicationConfigParamManger.Instance.WriteValue(e.CoordSysName, enCommunicationCommand.ResultToSocket, "NG");
                            CommunicationConfigParamManger.Instance.WriteValue(e.CoordSysName, enCommunicationCommand.TriggerToPlc, 1);
                        }
                        /////////////////////////////////////////////
                        if (this.programPath.Contains("任务")) // 只打开对应的任务
                            this.配方toolStripStatusLabel.Text = new FileInfo(this.programPath).DirectoryName;
                        else
                            this.配方toolStripStatusLabel.Text = this.programPath;
                        break;
                    case "Save":
                    case "save":
                        LoggerHelper.Info("配方保存");
                        this.programPath = path;
                        if (ProgramForm.Instance.SaveProgram(this.programPath))
                        {
                            CommunicationConfigParamManger.Instance.WriteValue(e.CoordSysName, enCommunicationCommand.ResultToPlc, 1);
                            CommunicationConfigParamManger.Instance.WriteValue(e.CoordSysName, enCommunicationCommand.ResultToSocket, "OK");
                            CommunicationConfigParamManger.Instance.WriteValue(e.CoordSysName, enCommunicationCommand.TriggerToPlc, 1);
                        }
                        else
                        {
                            CommunicationConfigParamManger.Instance.WriteValue(e.CoordSysName, enCommunicationCommand.ResultToPlc, 2);
                            CommunicationConfigParamManger.Instance.WriteValue(e.CoordSysName, enCommunicationCommand.ResultToSocket, "NG");
                            CommunicationConfigParamManger.Instance.WriteValue(e.CoordSysName, enCommunicationCommand.TriggerToPlc, 1);
                        }
                        /////////////////////////////////////////
                        if (this.programPath.Contains("任务")) // 只打开对应的任务
                            this.配方toolStripStatusLabel.Text = new FileInfo(this.programPath).DirectoryName;
                        else
                            this.配方toolStripStatusLabel.Text = this.programPath;
                        break;

                    case "SaveAs":
                    case "saveas":
                        LoggerHelper.Info("配方另存为");
                        this.programPath = path;
                        if (ProgramForm.Instance.SaveProgram(this.programPath))
                        {
                            CommunicationConfigParamManger.Instance.WriteValue(e.CoordSysName, enCommunicationCommand.ResultToPlc, 1);
                            CommunicationConfigParamManger.Instance.WriteValue(e.CoordSysName, enCommunicationCommand.ResultToSocket, "OK");
                            CommunicationConfigParamManger.Instance.WriteValue(e.CoordSysName, enCommunicationCommand.TriggerToPlc, 1);
                        }
                        else
                        {
                            CommunicationConfigParamManger.Instance.WriteValue(e.CoordSysName, enCommunicationCommand.ResultToPlc, 2);
                            CommunicationConfigParamManger.Instance.WriteValue(e.CoordSysName, enCommunicationCommand.ResultToSocket, "NG");
                            CommunicationConfigParamManger.Instance.WriteValue(e.CoordSysName, enCommunicationCommand.TriggerToPlc, 1);
                        }
                        /////////////////////////////////////////
                        if (this.programPath.Contains("任务")) // 只打开对应的任务
                            this.配方toolStripStatusLabel.Text = new FileInfo(this.programPath).DirectoryName;
                        else
                            this.配方toolStripStatusLabel.Text = this.programPath;
                        break;

                    case "Recipe":
                    case "recipe":
                        LoggerHelper.Info("配方切换");
                        // 自动切换配方  , 必需要触发才能换型
                        if (path != null && path.Length > 0)
                        {
                            this.Invoke(new Action(() =>
                            {
                                ToolStripItem toolStripMenu = new ToolStripMenuItem();
                                toolStripMenu.Name = "断开连机";
                                toolStripMenu.Text = "断开连机";
                                //this.运行工具条toolStrip_ItemClicked_1(null, new ToolStripItemClickedEventArgs(toolStripMenu));
                                this.调试toolStripMenuItem44_DropDownItemClicked(null, new ToolStripItemClickedEventArgs(toolStripMenu));
                                Thread.Sleep(1000);
                                if (ProgramForm.Instance.OpenProgram(path))
                                {
                                    this.programPath = path;
                                    this.程序1toolStripStatusLabel.Text = path;
                                    ////////////////////////////////////////// 换型成功，写入信号 /////////////////////////////////////////
                                    CommunicationConfigParamManger.Instance.WriteValue(e.CoordSysName, enCommunicationCommand.ProgramNoToPlc, path);
                                    CommunicationConfigParamManger.Instance.WriteValue(e.CoordSysName, enCommunicationCommand.ResultToPlc, 1);
                                    CommunicationConfigParamManger.Instance.WriteValue(e.CoordSysName, enCommunicationCommand.ResultToSocket, "OK");
                                    CommunicationConfigParamManger.Instance.WriteValue(e.CoordSysName, enCommunicationCommand.TriggerToPlc, 1);
                                }
                                else
                                {
                                    this.programPath = ProgramForm.Instance.ProgramPath;
                                    this.程序1toolStripStatusLabel.Text = ProgramForm.Instance.ProgramPath;
                                    LoggerHelper.Error("指定的程序路径不存在(Program Path No Exist!!!)");
                                    /////////////////////////////////////////////////////////////////////////
                                    CommunicationConfigParamManger.Instance.WriteValue(e.CoordSysName, enCommunicationCommand.ProgramNoToPlc, path);
                                    CommunicationConfigParamManger.Instance.WriteValue(e.CoordSysName, enCommunicationCommand.ResultToPlc, 2);
                                    CommunicationConfigParamManger.Instance.WriteValue(e.CoordSysName, enCommunicationCommand.ResultToSocket, "NG");
                                    CommunicationConfigParamManger.Instance.WriteValue(e.CoordSysName, enCommunicationCommand.TriggerToPlc, 1);
                                }
                            }));
                            ///////////////////////////////
                            this.Invoke(new Action(() =>
                            {
                                ToolStripItem toolStripMenu2 = new ToolStripMenuItem();
                                toolStripMenu2.Name = "联机运行";
                                toolStripMenu2.Text = "联机运行";
                                this.调试toolStripMenuItem44_DropDownItemClicked(null, new ToolStripItemClickedEventArgs(toolStripMenu2));
                                //this.运行工具条toolStrip_ItemClicked_1(null, new ToolStripItemClickedEventArgs(toolStripMenu2));
                            }));
                        }
                        else
                        {
                            CommunicationConfigParamManger.Instance.WriteValue(e.CoordSysName, enCommunicationCommand.ProgramNo, path);
                            CommunicationConfigParamManger.Instance.WriteValue(e.CoordSysName, enCommunicationCommand.ResultToPlc, 2);
                            CommunicationConfigParamManger.Instance.WriteValue(e.CoordSysName, enCommunicationCommand.ResultToSocket, "NG");
                            CommunicationConfigParamManger.Instance.WriteValue(e.CoordSysName, enCommunicationCommand.TriggerToPlc, 1);
                        }
                        break;

                    case "Login":
                    case "login":
                        switch (path)
                        {
                            default:
                            case "操作员":
                            case "operate":
                            case "Operate":
                            case "Operator":
                            case "operator":
                                UserLoginParamManager.Instance.CurrentUser = enUserName.操作员;
                                this.Invoke(new Action(() =>
                                {
                                    this.用户label.Text = "操作员";
                                }));
                                CommunicationConfigParamManger.Instance.WriteValue(e.CoordSysName, enCommunicationCommand.ProgramNo, path);
                                CommunicationConfigParamManger.Instance.WriteValue(e.CoordSysName, enCommunicationCommand.ResultToPlc, 1);
                                CommunicationConfigParamManger.Instance.WriteValue(e.CoordSysName, enCommunicationCommand.ResultToSocket, "OK");
                                CommunicationConfigParamManger.Instance.WriteValue(e.CoordSysName, enCommunicationCommand.ProgramNoToPlc, 1);
                                break;
                            case "supper":
                            case "Supper":
                            case "开发人员":
                                UserLoginParamManager.Instance.CurrentUser = enUserName.开发人员;
                                this.Invoke(new Action(() =>
                                {
                                    this.用户label.Text = "开发人员";
                                }));
                                CommunicationConfigParamManger.Instance.WriteValue(e.CoordSysName, enCommunicationCommand.ProgramNo, path);
                                CommunicationConfigParamManger.Instance.WriteValue(e.CoordSysName, enCommunicationCommand.ResultToPlc, 1);
                                CommunicationConfigParamManger.Instance.WriteValue(e.CoordSysName, enCommunicationCommand.ResultToSocket, "OK");
                                CommunicationConfigParamManger.Instance.WriteValue(e.CoordSysName, enCommunicationCommand.ProgramNoToPlc, 1);
                                break;
                        }
                        break;

                    case "Lable":
                    case "lable":
                        int index = 1;
                        int.TryParse(path, out index);
                        string[] lable = this.GetLableName(index);
                        if (lable != null && lable.Length > 0)
                        {
                            CommunicationConfigParamManger.Instance.WriteValue(e.CoordSysName, enCommunicationCommand.ProgramNo, string.Join(",", lable));
                            CommunicationConfigParamManger.Instance.WriteValue(e.CoordSysName, enCommunicationCommand.ResultToPlc, 1);
                            CommunicationConfigParamManger.Instance.WriteValue(e.CoordSysName, enCommunicationCommand.ResultToSocket, "OK");
                            CommunicationConfigParamManger.Instance.WriteValue(e.CoordSysName, enCommunicationCommand.ProgramNoToPlc, 1);
                            LoggerHelper.Info("必送标签成功");
                        }
                        else
                        {
                            LoggerHelper.Info("获取标签名称失败,标签名称为空或长度为0");
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                CommunicationConfigParamManger.Instance.WriteValue(e.CoordSysName, enCommunicationCommand.ResultToSocket, "NG");
                CommunicationConfigParamManger.Instance.WriteValue(e.CoordSysName, enCommunicationCommand.ResultToPlc, 1);
                switch (function)
                {
                    case "Login":
                    case "login":
                        new UserMessageForm().ShowDialog("用户登录时出错：" + ex.ToString());
                        break;
                    case "Recipe":
                    case "recipe":
                        new UserMessageForm().ShowDialog("自动切换配方时出错：" + ex.ToString());
                        break;
                    default:
                        new UserMessageForm().ShowDialog("未知操作错误!" + ex.ToString());
                        break;
                }

            }

        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                DialogResult dialogResult = new UserMessageForm().ShowDialog("确定要退出程序吗？", "退出程序");
                if (dialogResult == DialogResult.OK)
                {
                    AutoRunThreadPlc.Instance.RecipeInfo -= new RecipeEventHandler(this.Recipe_Event);
                    UserLoginParamManager.Instance.UserChange -= new EventHandler(this.UserChange_Event);
                    //if (this._socket != null)
                    //    this._socket.SocketMessage -= new SocketMessageEventHandler(this.SocketMessage_Receiver);
                    UserLoginParamManager.Instance.CurrentUser = enUserName.操作员; // 读取成功后需要触发一次事件 .UserChange -= new EventHandler(this.UserChange_Event);
                    SocketMessageThread.Instance.UnInit();
                    AutoRunThreadPlc.Instance.UnInit();
                    //AutoRunThreadSocket.Instance.UnInit();
                    ////////////////////////
                    this.timer?.Change(0, Timeout.Infinite);
                    this.sensorlist.DisConnect();
                    this.card.DisConnect();
                    this.light.DisConnect();
                    this.timer?.Dispose();
                    SocketConnectManager.Instance.UnInitSocket();
                    /////////////////
                    for (int i = this.ListForm.Count - 1; i >= 0; i--)
                    {
                        this.ListForm[i]?.Close();
                    }
                    Thread.Sleep(1000);
                    ////////////////////// 关闭控制台
                    ConsoleHelper.FreeConsole();
                }
                else
                    e.Cancel = true;
            }
            catch (Exception ex)
            {

            }
        }
        public void AddForm2(Control MastPanel, Form form)
        {
            if (MastPanel.Controls.Count > 0)
                MastPanel.Controls.Clear();
            form.TopLevel = false;
            form.FormBorderStyle = FormBorderStyle.None;
            form.MaximizeBox = false;
            form.MinimizeBox = false;
            form.Dock = DockStyle.Fill;
            form.Padding = new Padding(0);
            MastPanel.Margin = new Padding(0);
            MastPanel.Controls.Add(form);
            this.ListForm.Add(form);
            form.Show();
        }

        public Form AddForm(Panel MastPanel, Form form)
        {
            if (MastPanel.Controls.Count > 0)
                MastPanel.Controls.Clear();
            form.TopLevel = false;
            form.FormBorderStyle = FormBorderStyle.None;
            form.MaximizeBox = false;
            form.MinimizeBox = false;
            form.Dock = DockStyle.Fill;
            form.Padding = new Padding(0);
            MastPanel.Margin = new Padding(0);
            MastPanel.Controls.Add(form);
            this.ListForm.Add(form);
            form.Show();
            return form;
        }
        public Form AddForm(TabPage MastPanel, Form form)
        {
            if (MastPanel.Controls.Count > 0)
                MastPanel.Controls.Clear();
            form.TopLevel = false;
            form.FormBorderStyle = FormBorderStyle.None;
            form.MaximizeBox = false;
            form.MinimizeBox = false;
            form.Dock = DockStyle.Fill;
            form.Padding = new Padding(0);
            MastPanel.Margin = new Padding(0);
            MastPanel.Controls.Add(form);
            //this.ListForm.Add(form);
            form.Show();
            return form;
        }
        public void AddForm(TableLayoutPanel MastPanel, Form form, int rowPose, int colPose, int rowSpan, int colSpan)
        {
            form.TopLevel = false;
            form.FormBorderStyle = FormBorderStyle.None;
            form.MaximizeBox = false;
            form.MinimizeBox = false;
            form.Dock = DockStyle.Fill;
            form.Padding = new Padding(0);
            /////////////////////////////
            MastPanel.Margin = new Padding(0);
            MastPanel.Controls.Add(form);
            MastPanel.SetRow(form, rowPose);
            MastPanel.SetColumn(form, colPose);
            MastPanel.SetRowSpan(form, rowSpan);
            MastPanel.SetColumnSpan(form, colSpan);
            this.ListForm.Add(form);
            form.Show();
        }


        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.F5:
                    //ProgramForm.Instance.Run(this.运行toolStripButton, 1);
                    break;
                case Keys.Escape:
                    ProgramForm.Instance.Stop();
                    break;
            }
        }

        private void toolStripMenuItem2_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            Form form;
            string name = e.ClickedItem.Text;
            switch (name)
            {
                case "相机9点标定":
                    //form = new Cam9PointCalibrateForm();  //new JogMotionControlForm()
                    //form.Owner = this;
                    //form.Show();
                    break;
                case "面阵补偿":
                    form = new MachineCalibForm();  //new JogMotionControlForm()
                    form.Owner = this;
                    form.Show();
                    break;
                default:
                    break;
            }
        }



        private int Count = 0;
        private void DateTimeMonitor(object state)
        {
            try
            {
                this.Invoke(new Action(() =>
                {
                    //this.日期toolStripStatusLabel.Text = DateTime.Now.ToString("yyyy/MM/dd");
                    //this.时间toolStripStatusLabel.Text = DateTime.Now.ToString("HH:mm:ss");
                    this.日期label.Text = DateTime.Now.ToString("yyyy/MM/dd");
                    this.时间label.Text = DateTime.Now.ToString("HH:mm:ss");
                    //////////////////////////////////////////////////////////////////////
                    this.用户label.Text = "登录:" + UserLoginParamManager.Instance.CurrentUser.ToString();
                    ////////////////////
                    float cpu = CPUMonitor.Instance.GetValue();
                    MemoryInfo memoryInfo = MemoryMonitor.Instance.getMemoryInfo();
                    this.Cpu分辨率toolStripStatusLabel.Text = Math.Round(cpu, 1) + "%";
                    this.总内存toolStripStatusLabel.Text = Math.Round(memoryInfo.totalPhys / Math.Pow(1024, 3), 2) + " G" + " / " + Math.Round(memoryInfo.availPhys / Math.Pow(1024, 3), 2) + " G";
                    this.内存利用率toolStripStatusLabel.Text = memoryInfo.memoryLoad + "%";
                    ////////////////////////////////////////
                    if (Directory.Exists("C:"))
                        this.C盘大小toolStripStatusLabel.Text = MemoryMonitor.Instance.GetHardDiskSpace("C").ToString() + " G" + " / " + MemoryMonitor.Instance.GetHardDiskFreeSpace("C").ToString() + " G"; ;
                    if (Directory.Exists("D:"))
                        this.D盘大小toolStripStatusLabel.Text = MemoryMonitor.Instance.GetHardDiskSpace("D").ToString() + " G" + " / " + MemoryMonitor.Instance.GetHardDiskFreeSpace("D").ToString() + " G";
                    if (Directory.Exists("E:"))
                        this.E盘大小toolStripStatusLabel.Text = MemoryMonitor.Instance.GetHardDiskSpace("E").ToString() + " G" + " / " + MemoryMonitor.Instance.GetHardDiskFreeSpace("E").ToString() + " G";
                    if (Directory.Exists("F:"))
                        this.F盘大小toolStripStatusLabel.Text = MemoryMonitor.Instance.GetHardDiskSpace("F").ToString() + " G" + " / " + MemoryMonitor.Instance.GetHardDiskFreeSpace("F").ToString() + " G";
                    string path = "";
                    foreach (var item in ProgramConfigParamManager.Instance.ProgramParamList)
                    {
                        if (path == "")
                            path += item.ProgramPath;
                        else
                            path += "; " + item.ProgramPath;
                    }
                    this.配方toolStripStatusLabel.Text = "";
                    this.配方toolStripStatusLabel.Text = path;
                    /////////////////////////////////////////////////
                    if (SystemParamManager.Instance.SysConfigParam.IsAutoRun)
                    {
                        if (this.联机运行toolStripButton.Text == "联机运行")
                        {
                            this.联机运行toolStripButton.Text = "断开连机";
                            this.联机运行toolStripButton.Image = global::CxVision.Properties.Resources.停止_50_X_50;
                        }
                    }
                    else
                    {
                        if (this.联机运行toolStripButton.Text == "断开连机")
                        {
                            this.联机运行toolStripButton.Text = "联机运行";
                            this.联机运行toolStripButton.Image = global::CxVision.Properties.Resources.开始2_50_X_50;
                        }
                    }
                }));
            }
            catch (Exception ex)
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


        #region 控件随窗体尺寸缩放代码

        private void Form1_Resize(object sender, EventArgs e)
        {
            string[] mytag = ((Form)sender).Tag.ToString().Split(new char[] { ':' });//获取控件的Tag属性值，并分割后存储字符串数组
            float newx = (((Form)sender).Width) / Convert.ToSingle(mytag[0]); //窗体宽度缩放比例
            float newy = ((Form)sender).Height / Convert.ToSingle(mytag[1]);//窗体高度缩放比例
            ResetControlsSize((Form)sender, newx, newy);//随窗体改变控件大小
        }

        /// <summary>
        /// 放在窗体加载函数里，用于记录初始的控件信息
        /// </summary>
        /// <param name="cons"></param>
        private void SetTag(Control cons)
        {
            // 先记录容器的相关数据
            cons.Tag = cons.Width + ":" + cons.Height;
            //遍历窗体中的控件
            foreach (Control con in cons.Controls)
            {
                con.Tag = con.Width + ":" + con.Height + ":" + con.Left + ":" + con.Top + ":" + con.Font.Size;
                if (con.Controls.Count > 0)
                    SetTag(con);
            }
        }

        /// <summary>
        /// 放在窗体变化的事件方法里，当窗体尺寸发生变化时，重置控件的尺寸
        /// </summary>
        /// <param name="cons"></param>
        /// <param name="newx"></param>
        /// <param name="newy"></param>
        private void ResetControlsSize(Control cons, float newx, float newy)
        {
            string[] mytag;
            //遍历窗体中的控件，重新设置控件的值
            foreach (Control con in cons.Controls)
            {
                mytag = con.Tag.ToString().Split(new char[] { ':' });//获取控件的Tag属性值，并分割后存储字符串数组
                float value = Convert.ToSingle(mytag[0]) * newx;//宽度,根据窗体缩放比例确定控件的值
                con.Width = (int)value;//宽度
                value = Convert.ToSingle(mytag[1]) * newy;//高度
                con.Height = (int)(value);
                value = Convert.ToSingle(mytag[2]) * newx;//左边距离
                con.Left = (int)(value);
                value = Convert.ToSingle(mytag[3]) * newy;//上边缘距离
                con.Top = (int)(value);
                Single currentSize = Convert.ToSingle(mytag[4]) * newy;//字体大小
                con.Font = new Font(con.Font.Name, currentSize, con.Font.Style, con.Font.Unit);
                if (con.Controls.Count > 0)
                {
                    ResetControlsSize(con, newx, newy);
                }
            }
        }




        #endregion



        private void buttonClose_Click(object sender, EventArgs e)
        {
            //DialogResult dialogResult = new Common.UserMessageForm().ShowDialog("确定关闭窗体吗？", "关闭窗体");
            //if (dialogResult == DialogResult.OK)
            //{
            this.Close();  //关闭窗口
                           // }
        }

        private void buttonMax_Click(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Maximized)   //如果处于最大化，则还原
            {
                this.WindowState = FormWindowState.Normal;
            }
            else
            {
                this.FormBorderStyle = FormBorderStyle.None;
                this.MaximumSize = new Size(Screen.PrimaryScreen.WorkingArea.Width, Screen.PrimaryScreen.WorkingArea.Height);
                this.WindowState = FormWindowState.Maximized;   //如果处于普通状态，则最大化
            }
        }

        private void buttonMin_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;  //最小化
        }


        private void MainFormNew_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, WM_SYSCOMMAND, SC_MOVE + HTCAPTION, 0);
        }

        private void titleLabel_MouseDown_1(object sender, MouseEventArgs e)
        {
            MainFormNew_MouseDown(null, null);
        }

        private void 文件toolStripMenuItem1_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            ToolStripItem item = e.ClickedItem;
            string text = item.Text;
            FileOperate fo = new FileOperate();
            switch (text)
            {
                case "新建":
                    this.programPath = "";
                    ProgramForm.Instance.NewProgram();
                    GlobalProgram.ProgramItems.Clear();
                    this.配方toolStripStatusLabel.Text = "";
                    ///////////////////////////////  重置配方程序 
                    //foreach (ClientSocket socket in SocketConnectManager.Instance.ClientSocketList)
                    //{
                    //    SocketMessage message = new SocketMessage(enSocketInfo.新建配方);
                    //    message.MesContent = this.programPath;
                    //    object mesgContent = socket.GetDataAsync(message, true);
                    //    message = mesgContent as SocketMessage;
                    //    new UserMessageForm().ShowDialog(message.MesContent.ToString());
                    //}
                    break;

                case "打开":
                    FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
                    folderBrowserDialog.ShowDialog();
                    this.programPath = folderBrowserDialog.SelectedPath;
                    ///////////////////////////////////
                    if (this.programPath == null || this.programPath.Trim().Length == 0) return;
                    //pf.TreeViewWrapClass.OpenProgram(this.programPath);
                    ProgramForm.Instance.OpenProgram(this.programPath);
                    if (this.programPath.Contains("任务")) // 只打开对应的任务
                        this.配方toolStripStatusLabel.Text = new FileInfo(this.programPath).DirectoryName;
                    else
                        this.配方toolStripStatusLabel.Text = this.programPath;
                    ///////////////////////////////  重置配方程序 
                    //foreach (ClientSocket socket in SocketConnectManager.Instance.ClientSocketList)
                    //{
                    //    SocketMessage message = new SocketMessage(enSocketInfo.打开配方);
                    //    message.MesContent = this.programPath;
                    //    object mesgContent = socket.GetDataAsync(message, true);
                    //    message = mesgContent as SocketMessage;
                    //    new UserMessageForm().ShowDialog(message.MesContent.ToString());
                    //}
                    break;

                case "保存":
                    this.programPath = ProgramForm.Instance.ProgramPath;
                    if (this.programPath == null || this.programPath.Length == 0)
                    {
                        FolderBrowserDialog saveFileDialog2 = new FolderBrowserDialog();
                        saveFileDialog2.ShowDialog();
                        this.programPath = saveFileDialog2.SelectedPath;
                        string savePath = saveFileDialog2.SelectedPath;
                        if (saveFileDialog2.SelectedPath.Contains("任务"))
                        {
                            this.programPath = new FileInfo(saveFileDialog2.SelectedPath).DirectoryName;// ;
                            savePath = this.programPath;
                        }
                        if (this.programPath == null || this.programPath.Length == 0) return;
                        //////////////////////////////////
                        if (ProgramForm.Instance.SaveProgram(savePath))
                            new Common.UserMessageForm("保存成功").ShowDialog();
                        else
                            //new UserMessageForm().ShowDialog("保存失败" + new Exception().ToString());
                            new Common.UserMessageForm("保存失败" + new Exception().ToString()).ShowDialog();
                    }
                    else
                    {
                        if (ProgramForm.Instance.SaveProgram()) // 如果在已有路径下保存，则不需要给路径
                            new Common.UserMessageForm("保存成功").ShowDialog();
                        else
                            //new UserMessageForm().ShowDialog("保存失败" + new Exception().ToString());
                            new Common.UserMessageForm("保存失败" + new Exception().ToString()).ShowDialog();
                    }
                    this.配方toolStripStatusLabel.Text = this.programPath;
                    /////////////////////////////////////
                    //foreach (ClientSocket socket in SocketConnectManager.Instance.ClientSocketList)
                    //{
                    //    SocketMessage message = new SocketMessage(enSocketInfo.保存配方);
                    //    message.MesContent = this.programPath;
                    //    object mesgContent = socket.GetDataAsync(message, true, 30000);
                    //    message = mesgContent as SocketMessage;
                    //    new UserMessageForm().ShowDialog(message.MesContent.ToString());
                    //}
                    break;
                case "另存为":
                    FolderBrowserDialog saveFileDialog = new FolderBrowserDialog();
                    saveFileDialog.ShowDialog();
                    this.programPath = saveFileDialog.SelectedPath;
                    if (saveFileDialog.SelectedPath.Contains("任务"))
                        this.programPath = new FileInfo(saveFileDialog.SelectedPath).DirectoryName;// ;
                    if (this.programPath == null || this.programPath.Length == 0) return;
                    //////////////////////////////////
                    if (ProgramForm.Instance.SaveProgram(this.programPath))
                    {
                        new Common.UserMessageForm("保存成功").ShowDialog();
                        ProgramForm.Instance.OpenProgram(this.programPath); // 如果是另存为，那么需要打另存为的程序
                    }
                    else
                        //new UserMessageForm().ShowDialog("保存失败" + new Exception().ToString());
                        new Common.UserMessageForm("保存失败" + new Exception().ToString()).ShowDialog();
                    ///////////////////////////////////////////////////////////////////////////
                    this.配方toolStripStatusLabel.Text = this.programPath;
                    /////////////////////////////////////
                    //foreach (ClientSocket socket in SocketConnectManager.Instance.ClientSocketList)
                    //{
                    //    SocketMessage message = new SocketMessage(enSocketInfo.保存配方);
                    //    message.MesContent = this.programPath;
                    //    object mesgContent = socket.GetDataAsync(message, true, 30000);
                    //    message = mesgContent as SocketMessage;
                    //    new UserMessageForm().ShowDialog(message.MesContent.ToString());
                    //}
                    break;
                case "加载配方":
                    //foreach (ClientSocket socket in SocketConnectManager.Instance.ClientSocketList)
                    //{
                    //    SocketMessage message = new SocketMessage(enSocketInfo.加载配方);
                    //    message.MesContent = this.programPath;
                    //    object mesgContent = socket.GetDataAsync(message, true, 30000);
                    //    message = mesgContent as SocketMessage;
                    //    ////////////////////////////////////////////////////////////
                    //    if (message != null)
                    //    {
                    //        Dictionary<string, List<TreeNode>> dic2 = message.MesContent as Dictionary<string, List<TreeNode>>;
                    //        if (dic2 != null)
                    //        {
                    //            foreach (KeyValuePair<string, List<TreeNode>> item2 in dic2)
                    //            {
                    //                if (ProgramForm.Instance.ProgramDic.ContainsKey(item2.Key))
                    //                {
                    //                    ProgramForm.Instance.ProgramDic[item2.Key].TreeView.Nodes.Clear();
                    //                    ProgramForm.Instance.ProgramDic[item2.Key].TreeView.Nodes.AddRange(item2.Value.ToArray());
                    //                }
                    //            }
                    //        }
                    //    }
                    //}
                    break;
                case "下载配方":
                    //Dictionary<string, List<TreeNode>> dic = new Dictionary<string, List<TreeNode>>();
                    //List<TreeNode> list = null;
                    //foreach (KeyValuePair<string, TreeViewWrapClass> item2 in ProgramForm.Instance.ProgramDic)
                    //{
                    //    list = new List<TreeNode>();
                    //    TreeNode[] nodes = new TreeNode[item2.Value.TreeView.Nodes.Count];
                    //    item2.Value.TreeView.Nodes.CopyTo(nodes, 0);
                    //    list.AddRange(nodes);
                    //    dic.Add(item2.Key, list);
                    //}
                    /////////////////////////////////
                    //foreach (ClientSocket socket in SocketConnectManager.Instance.ClientSocketList)
                    //{
                    //    SocketMessage message = new SocketMessage(enSocketInfo.下载配方);
                    //    message.Name = this.programPath;
                    //    message.MesContent = dic;
                    //    object mesgContent = socket.GetDataAsync(message, true, 30000);
                    //    message = mesgContent as SocketMessage;
                    //    new UserMessageForm().ShowDialog(message.MesContent.ToString());
                    //}
                    break;
                case "软件退出":
                    this.Close();
                    break;

                default:
                    break;
            }
        }

        private void 语言toolStripComboBox_DropDownClosed(object sender, EventArgs e)
        {
            if (this.语言toolStripComboBox.SelectedItem == null) return;
            switch (语言toolStripComboBox.SelectedItem.ToString())
            {
                case "中文(Chinise)":
                    SystemParamManager.Instance.SysConfigParam.Language = "zh-CN";// 中文简体
                    break;
                case "英文(English)":
                    SystemParamManager.Instance.SysConfigParam.Language = "en-US"; // 美国
                    break;
                default:
                    break;
            }
            SystemParamManager.Instance.Save();
        }

        private void 配置toolStripMenuItem37_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            Form form;
            string text = e.ClickedItem.Text;
            switch (text.Trim())
            {
                //////////////////////////////////
                case "传感器配置":
                    // form = new SensorConfigForm();
                    form = new SensorConnectConfigParamMangerForm();
                    form.StartPosition = FormStartPosition.Manual;
                    form.Location = System.Windows.Forms.Cursor.Position;
                    form.Owner = this;
                    form.Show();
                    break;
                //////////////////////////////////
                case "参数设置":
                    form = new ParamConfigForm();
                    form.StartPosition = FormStartPosition.Manual;
                    form.Location = System.Windows.Forms.Cursor.Position;
                    form.Owner = this;
                    form.Show();
                    break;
                //////////////////////////////////
                case "光源配置":
                    form = new LightConnectConfigManageForm();
                    form.StartPosition = FormStartPosition.Manual;
                    form.Location = System.Windows.Forms.Cursor.Position;
                    form.Owner = this;
                    form.Show();
                    break;
                //////////////////////////////////
                case "运动控制卡配置":
                    form = new DeviceConnectConfigParamManageForm();
                    form.StartPosition = FormStartPosition.Manual;
                    form.Location = System.Windows.Forms.Cursor.Position;
                    form.Owner = this;
                    form.Show();
                    break;
                case "采集源配置":
                    if (UserLoginParamManager.Instance.CurrentUser != enUserName.开发人员)
                    {
                        new UserMessageForm().ShowDialog("非开发人员用户不能进行采集源配置!", "采集源配置");
                        return;
                    }
                    form = new AcqSourceConfigForm();
                    form.StartPosition = FormStartPosition.Manual;
                    form.Location = System.Windows.Forms.Cursor.Position;
                    form.Owner = this;
                    form.Show();
                    break;
                case "程序配置":
                    form = new ProgramConfigParamForm();
                    form.StartPosition = FormStartPosition.Manual;
                    form.Location = System.Windows.Forms.Cursor.Position;
                    form.Owner = this;
                    form.Show();
                    break;
                case "工程配置":
                    this.视图TabControl.SelectedTab = this.系统配置tabPage;
                    this.AddForm(this.系统配置tabPage, ProjectManagerFormNew.Instance);
                    switch (UserLoginParamManager.Instance.CurrentUser)
                    {
                        case enUserName.操作员:
                            UserLoginParamManager.Instance.CurrentUser = enUserName.操作员;
                            break;
                        case enUserName.工程师:
                            UserLoginParamManager.Instance.CurrentUser = enUserName.工程师;
                            break;
                        case enUserName.开发人员:
                            UserLoginParamManager.Instance.CurrentUser = enUserName.开发人员;
                            break;
                        case enUserName.管理员:
                            UserLoginParamManager.Instance.CurrentUser = enUserName.管理员;
                            break;
                    }
                    break;
                case "连接配置":
                    this.视图TabControl.SelectedTab = this.系统配置tabPage;
                    this.AddForm(this.系统配置tabPage, ConnectManagerFormNew.Instance);
                    switch (UserLoginParamManager.Instance.CurrentUser)
                    {
                        case enUserName.操作员:
                            UserLoginParamManager.Instance.CurrentUser = enUserName.操作员;
                            break;
                        case enUserName.工程师:
                            UserLoginParamManager.Instance.CurrentUser = enUserName.工程师;
                            break;
                        case enUserName.开发人员:
                            UserLoginParamManager.Instance.CurrentUser = enUserName.开发人员;
                            break;
                        case enUserName.管理员:
                            UserLoginParamManager.Instance.CurrentUser = enUserName.管理员;
                            break;
                    }
                    break;

                default:
                    break;

            }
        }

        private void 调试toolStripMenuItem44_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            Form form;
            string text = e.ClickedItem.Text;
            switch (text.Trim())
            {
                case "联机运行":
                case "断开连机":
                    if (e.ClickedItem.Text == "联机运行")
                    {
                        e.ClickedItem.Text = "断开连机";
                        this.联机运行toolStripButton.Image = global::CxVision.Properties.Resources.停止_50_X_50;
                        AutoRunThreadPlc.Instance.Init();
                        SystemParamManager.Instance.SysConfigParam.IsAutoRun = true;
                    }
                    else
                    {
                        e.ClickedItem.Text = "联机运行";
                        this.联机运行toolStripButton.Image = global::CxVision.Properties.Resources.开始2_50_X_50;
                        AutoRunThreadPlc.Instance.UnInit();
                        SystemParamManager.Instance.SysConfigParam.IsAutoRun = false;
                        SystemParamManager.Instance.SysConfigParam.InterruptSingle = enInterruptType.用户复位中断;
                    }
                    break;
                case "查看点激光":
                    form = new PointLaserForm();
                    form.Owner = this;
                    form.Show();
                    break;
                case "查看线激光":
                    form = new LineLaserForm();
                    form.Owner = this;
                    form.Show();
                    break;
                default:
                    break;
            }
        }

        private void 参数toolStripMenuItem47_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            string text = e.ClickedItem.Text;
            switch (text.Trim())
            {
                case "保存视图布局":
                    if (ViewConfigParamManager.Instance.Save())  // 尽量不要将保存放在 窗体关闭里，因为有可能会强制停止
                        new UserMessageForm().ShowDialog("保存成功!!!");
                    break;
                case "重置屏幕参数":
                    Rectangle rect = Screen.GetWorkingArea(this);
                    ScreenManager.Instance.ScreenParam.InitParam(rect.Width, rect.Height);
                    ScreenManager.Instance.Save();
                    break;
                default:
                    break;
            }
        }

        private void 用户toolStripMenuItem50_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            try
            {
                LoginForm loginForm = new LoginForm();
                loginForm.ShowDialog();
                //LoginForm.Instance.Show();
            }
            catch (Exception ex)
            {
                new UserMessageForm(ex.ToString()).ShowDialog();
            }
        }

        private void 工具toolStripMenuItem10_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            Form form;
            string text = e.ClickedItem.Text;
            switch (text)
            {
                case "检测工具":
                    form = new ToolForm();
                    form.Owner = this;
                    form.Show();
                    break;
                case "对射标定工具":
                    form = new CalibrateDoubleLaserForm();
                    form.Owner = this;
                    form.Show();
                    break;
                case "运动控制":
                    form = new JogMotionForm();  //new JogMotionControlForm()
                    form.Owner = this;
                    form.Show();
                    break;
                case "线性校准":
                    form = new LinearCalibrateForm();  //new JogMotionControlForm()
                    form.Owner = this;
                    form.Show();
                    break;
                case "面阵校准":
                    form = new MachineCalibForm();  //new JogMotionControlForm()
                    form.Owner = this;
                    form.Show();
                    break;
                default:
                    break;
            }
        }

        private void 相机标定toolStripMenuItem13_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            Form form;
            string name = e.ClickedItem.Text;
            switch (name)
            {
                case "FA相机标定":
                    form = new SpaceCalibrateCameraParamForm();
                    form.Owner = this;
                    form.Show();
                    break;
                case "矩阵标定相机":
                    form = new MatrixCalibrateCameraParamForm();
                    form.Owner = this;
                    form.Show();
                    break;
                case "N点标定相机":
                    form = new CamNPointCalibParamSimpleForm();
                    form.Owner = this;
                    form.Show();
                    break;
                //case "标定相机角度":
                //    form = new CameraSlantCalibrateForm();
                //    form.Owner = this;
                //    form.Show();
                //    break;
                default:
                    break;
            }
        }

        private void 相机激光标定toolStripMenuItem18_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            Form form;
            string text = e.ClickedItem.Name;
            switch (text)
            {
                case nameof(this.相机点激光标定toolStripMenuItem):
                case "相机&点激光标定":
                    form = new CameraPointLaserCalibrateForm();  //new JogMotionControlForm()
                    form.Owner = this;
                    form.Show();
                    break;
                case nameof(this.相机线激光标定toolStripMenuItem):
                case "相机&线激光标定":
                    form = new CameraLineLaserCalibrateForm();
                    form.Owner = this;
                    form.Show();
                    break;
                case nameof(this.相机面激光标定toolStripMenuItem):
                case "相机&面激光标定":
                    form = new CameraFaceLaserCalibrateForm();
                    form.Owner = this;
                    form.Show();
                    break;
                case nameof(this.相机胶枪标定toolStripMenuItem):
                case "相机&胶枪标定":
                    form = new CameraGlueGunCalibrateForm();
                    form.Owner = this;
                    form.Show();
                    break;
                case nameof(this.机器人夹抓标定toolStripMenuItem):
                case "相机&机器人&夹抓标定":
                    form = new RebotJawCalibrateForm();
                    form.Owner = this;
                    form.Show();
                    break;
                case nameof(this.相机映射标定ToolStripMenuItem):
                    form = new CamMapCalibParamSimpleForm();
                    form.Owner = this;
                    form.Show();
                    break;
                default:
                    break;
            }
        }

        private void 激光标定toolStripMenuItem25_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            Form form;
            string name = e.ClickedItem.Text;
            switch (name)
            {
                case "标定线激光":
                    form = new LineSensorCalibrateForm();
                    form.Owner = this;
                    form.Show();
                    break;
                case "标定点激光":
                    form = new FaceSensorCalibrateForm();
                    form.Owner = this;
                    form.Show();
                    break;
                case "标定面激光":
                    form = new FaceSensorCalibrateForm();
                    form.Owner = this;
                    form.Show();
                    break;
                default:
                    break;
            }
        }

        private void 机台toolStripMenuItem29_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            Form form;
            string name = e.ClickedItem.Text;
            switch (name)
            {
                case "线性补偿":
                    form = new LinearCalibrateForm();  //new JogMotionControlForm()
                    form.Owner = this;
                    form.Show();
                    break;
                case "面阵补偿":
                    form = new MachineCalibForm();  //new JogMotionControlForm()
                    form.Owner = this;
                    form.Show();
                    break;
                default:
                    break;
            }
        }

        private void Nine点标定toolStripMenuItem32_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            Form form;
            string name = e.ClickedItem.Text;
            switch (name)
            {
                case "相机9点标定":
                    //form = new Cam9PointCalibrateForm();  //new JogMotionControlForm()
                    //form.Owner = this;
                    //form.Show();
                    break;
                case "面阵补偿":
                    form = new MachineCalibForm();  //new JogMotionControlForm()
                    form.Owner = this;
                    form.Show();
                    break;
                default:
                    break;
            }
        }

        private void panel2_MouseDown(object sender, MouseEventArgs e)
        {
            MainFormNew_MouseDown(null, null);
        }
        private void menuStrip1_MouseDown(object sender, MouseEventArgs e)
        {
            MainFormNew_MouseDown(null, null);
        }



        private void toolStripMenuItem9_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (UserLoginParamManager.Instance.CurrentUser == enUserName.操作员)
            {
                new UserMessageForm().ShowDialog("操作员没有此权限!");
                return;
            }
            string text = e.ClickedItem.Text;
            switch (text.Trim())
            {
                case "保存视图布局":
                    if (ViewConfigParamManager.Instance.Save())  // 尽量不要将保存放在 窗体关闭里，因为有可能会强制停止
                    {
                        new UserMessageForm().ShowDialog("保存成功!!!");
                    }
                    break;
                case "添加视图":
                    if (UserLoginParamManager.Instance.CurrentUser != enUserName.开发人员)
                    {
                        new UserMessageForm().ShowDialog("非开发人员用户不能添加视图!", "添加视图");
                        return;
                    }
                    AddViewForm addViewForm = new AddViewForm();
                    addViewForm.ShowDialog();
                    if (addViewForm.IsCancel) return;
                    Form form = ViewConfigParamManager.Instance.AddFormView(this.视图TabControl.SelectedTab, addViewForm);
                    this.ListForm.Add(form); // 将在主窗体上打开的所有窗体保存下来，先关闭他们再关闭主窗体
                    break;
                case "编辑视图项":
                    if (UserLoginParamManager.Instance.CurrentUser != enUserName.开发人员)
                    {
                        new UserMessageForm().ShowDialog("非开发人员用户不能编辑视图项!", "编辑视图项");
                        return;
                    }
                    new ViewElementForm().Show();
                    break;
                case "清空视图配置":
                    if (UserLoginParamManager.Instance.CurrentUser != enUserName.开发人员)
                    {
                        new UserMessageForm().ShowDialog("非开发人员用户不能清空视图配置!", "清空视图配置");
                        return;
                    }
                    ViewConfigParamManager.Instance.Clear();
                    ViewConfigParamManager.Instance.Save();
                    break;
                default:
                    break;
            }
        }

        private void 运行工具条toolStrip_ItemClicked_1(object sender, ToolStripItemClickedEventArgs e)
        {
            ToolStripItem item = e.ClickedItem;
            string name = item.Name;
            int count = 1;
            switch (name)
            {
                case "工程配置toolStripButton":
                    this.视图TabControl.SelectedTab = this.系统配置tabPage;
                    this.AddForm(this.系统配置tabPage, ProjectManagerFormNew.Instance);
                    switch (UserLoginParamManager.Instance.CurrentUser)
                    {
                        case enUserName.操作员:
                            UserLoginParamManager.Instance.CurrentUser = enUserName.操作员;
                            break;
                        case enUserName.工程师:
                            UserLoginParamManager.Instance.CurrentUser = enUserName.工程师;
                            break;
                        case enUserName.开发人员:
                            UserLoginParamManager.Instance.CurrentUser = enUserName.开发人员;
                            break;
                        case enUserName.管理员:
                            UserLoginParamManager.Instance.CurrentUser = enUserName.管理员;
                            break;
                    }
                    break;

                case "连接配置toolStripButton":
                    this.视图TabControl.SelectedTab = this.系统配置tabPage;
                    this.AddForm(this.系统配置tabPage, ConnectManagerFormNew.Instance);
                    switch (UserLoginParamManager.Instance.CurrentUser)
                    {
                        case enUserName.操作员:
                            UserLoginParamManager.Instance.CurrentUser = enUserName.操作员;
                            break;
                        case enUserName.工程师:
                            UserLoginParamManager.Instance.CurrentUser = enUserName.工程师;
                            break;
                        case enUserName.开发人员:
                            UserLoginParamManager.Instance.CurrentUser = enUserName.开发人员;
                            break;
                        case enUserName.管理员:
                            UserLoginParamManager.Instance.CurrentUser = enUserName.管理员;
                            break;
                    }
                    break;

                case "联机运行toolStripButton":
                    if (e.ClickedItem.Text == "联机运行")
                    {
                        AutoRunThreadPlc.Instance.UnInit();
                        Thread.Sleep(200);
                        e.ClickedItem.Text = "断开连机";
                        this.联机运行toolStripButton.Image = global::CxVision.Properties.Resources.停止_50_X_50;
                        AutoRunThreadPlc.Instance.Init();
                        SystemParamManager.Instance.SysConfigParam.IsAutoRun = true;
                        ///////////////////////////////////////////////////////////////////////////////
                        //foreach (ClientSocket socket in SocketConnectManager.Instance.ClientSocketList)
                        //{
                        //    SocketMessage message = new SocketMessage(enSocketInfo.联机运行);
                        //    message.MesContent = "联机运行";
                        //    object mesgContent = socket.GetDataAsync(message, true);
                        //    message = mesgContent as SocketMessage;
                        //    if (message != null || message.MesContent?.ToString() == "OK")
                        //        new UserMessageForm().ShowDialog("联机成功", "联机操作");
                        //    else
                        //        new UserMessageForm().ShowDialog("联机失败", "联机操作");
                        //}
                    }
                    else
                    {
                        e.ClickedItem.Text = "联机运行";
                        this.联机运行toolStripButton.Image = global::CxVision.Properties.Resources.开始2_50_X_50;
                        AutoRunThreadPlc.Instance.UnInit();
                        SystemParamManager.Instance.SysConfigParam.IsAutoRun = false;
                        SystemParamManager.Instance.SysConfigParam.InterruptSingle = enInterruptType.NONE;
                        ///////////////////////////////////////////////////////////////////////////////
                        //foreach (ClientSocket socket in SocketConnectManager.Instance.ClientSocketList)
                        //{
                        //    SocketMessage message = new SocketMessage(enSocketInfo.断开联机);
                        //    message.MesContent = "断开联机";
                        //    object mesgContent = socket.GetDataAsync(message, true);
                        //    message = mesgContent as SocketMessage;
                        //    if (message != null || message.MesContent?.ToString() == "OK")
                        //        new UserMessageForm().ShowDialog("断开联机成功", "联机操作");
                        //    else
                        //        new UserMessageForm().ShowDialog("断开联机失败", "联机操作");
                        //}
                    }
                    break;

                case "断开连机toolStripButton":
                    AutoRunThreadPlc.Instance.UnInit();
                    SystemParamManager.Instance.SysConfigParam.IsAutoRun = false;
                    SystemParamManager.Instance.SysConfigParam.InterruptSingle = enInterruptType.NONE;
                    ///////////////////////////////////////////////////////////////////////////////
                    //foreach (ClientSocket socket in SocketConnectManager.Instance.ClientSocketList)
                    //{
                    //    SocketMessage message = new SocketMessage(enSocketInfo.断开联机);
                    //    message.MesContent = "断开联机";
                    //    object mesgContent = socket.GetDataAsync(message, true);
                    //    message = mesgContent as SocketMessage;
                    //    if (message != null || message.MesContent?.ToString() == "OK")
                    //        new UserMessageForm().ShowDialog("断开联机成功", "联机操作");
                    //    else
                    //        new UserMessageForm().ShowDialog("断开联机失败", "联机操作");
                    //}
                    break;

                case nameof(this.单次执行toolStripButton):
                    ProgramForm.Instance.Run(this.单次执行toolStripButton, 1);
                    break;
                default:
                    break;
            }
        }

        private void 用户登录toolStripStatusLabe_Click(object sender, EventArgs e)
        {
            try
            {
                LoginForm loginForm = new LoginForm();
                loginForm.ShowDialog();
                //LoginForm.Instance.Show();
            }
            catch (Exception ex)
            {
                new UserMessageForm(ex.ToString()).ShowDialog();
            }
        }
        #region Lable右键菜单项
        private void addLableContextMenu()
        {
            ContextMenuStrip ContextMenuStrip1 = new ContextMenuStrip();
            // 添加右键菜单 
            ToolStripItem[] items = new ToolStripMenuItem[]
            {
                new ToolStripMenuItem("重命名"),
            };
            ContextMenuStrip1.Items.AddRange(items); // 在滑赋值前不能调用 
            ContextMenuStrip1.ItemClicked += new ToolStripItemClickedEventHandler(label1ContextMenuStrip_ItemClicked);
            this.项目名称label.ContextMenuStrip = ContextMenuStrip1;
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
                        RenameForm renameForm = new RenameForm(this.项目名称label.Text);
                        DialogResult dialogResult = renameForm.ShowDialog();
                        if (dialogResult == DialogResult.OK)
                        {
                            this.项目名称label.Text = "项目名称:" + renameForm.ReName;
                            SystemParamManager.Instance.SysConfigParam.ProjectName = this.项目名称label.Text;
                            SystemParamManager.Instance.Save();
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

        private void 用户label_MouseEnter(object sender, EventArgs e)
        {
            this.用户panel.BackColor = System.Drawing.Color.White;
        }
        private void 用户label_MouseLeave(object sender, EventArgs e)
        {
            this.用户panel.BackColor = System.Drawing.Color.Gainsboro;
        }
        private void 用户label_Click(object sender, EventArgs e)
        {
            try
            {
                LoginForm loginForm = new LoginForm();
                loginForm.StartPosition = FormStartPosition.CenterScreen;
                //loginForm.TopMost = true;
                //loginForm.ShowInTaskbar = true;
                loginForm.Owner = this;
                loginForm.Show();
                //LoginForm.Instance.Show();
            }
            catch (Exception ex)
            {
                new UserMessageForm(ex.ToString()).ShowDialog();
            }
        }

        private void 编辑工具条toolStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            ToolStripItem item = e.ClickedItem;
            string name = item.Name;
            FileOperate fo = new FileOperate();
            try
            {
                switch (name)
                {
                    case "新建NToolStripButton":
                        this.programPath = "";
                        ProgramForm.Instance.NewProgram();
                        GlobalProgram.ProgramItems.Clear();
                        this.配方toolStripStatusLabel.Text = "";
                        /////////////////////////////////////
                        foreach (ClientSocket socket in SocketConnectManager.Instance.ClientSocketList)
                        {
                            SocketMessage message = new SocketMessage(enSocketInfo.新建配方);
                            message.MesContent = this.programPath;
                            object mesgContent = socket.GetDataAsync(message, true);
                            message = mesgContent as SocketMessage;
                            if (message != null && message.MesContent?.ToString() == "OK")
                                new UserMessageForm().ShowDialog("新建配方成功", "配方操作");
                            else
                                new UserMessageForm().ShowDialog("新建配方失败", "配方操作");
                        }
                        break;

                    case "打开ToolStripButton":
                        FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
                        //folderBrowserDialog.RootFolder = Environment.SpecialFolder.History;
                        folderBrowserDialog.ShowDialog();
                        this.programPath = folderBrowserDialog.SelectedPath;
                        //if (folderBrowserDialog.SelectedPath.Contains("任务"))
                        //    this.programPath = new FileInfo(folderBrowserDialog.SelectedPath).DirectoryName;  // 不能到任务那一级
                        ///////////////////////////////////
                        if (this.programPath == null || this.programPath.Trim().Length == 0) return;
                        ProgramForm.Instance.OpenProgram(this.programPath);
                        if (this.programPath.Contains("任务")) // 只打开对应的任务
                            this.配方toolStripStatusLabel.Text = new FileInfo(this.programPath).DirectoryName;
                        else
                            this.配方toolStripStatusLabel.Text = this.programPath;
                        //this.toolStripStatusLabel1.Text = this.programPath;
                        /////////////////////////////////////
                        foreach (ClientSocket socket in SocketConnectManager.Instance.ClientSocketList)
                        {
                            SocketMessage message = new SocketMessage(enSocketInfo.打开配方);
                            message.MesContent = this.programPath;
                            object mesgContent = socket.GetDataAsync(message, true, 10000);
                            message = mesgContent as SocketMessage;
                            if (message != null && message.MesContent?.ToString() == "OK")
                                new UserMessageForm().ShowDialog("打开配方成功", "配方操作");
                            else
                                new UserMessageForm().ShowDialog("打开配方失败", "配方操作");
                        }
                        break;

                    case "保存ToolStripButton":
                        this.programPath = ProgramForm.Instance.ProgramPath;
                        //if (!Directory.Exists(new FileInfo(this.programPath).DirectoryName))
                        //    Directory.CreateDirectory(new FileInfo(this.programPath).DirectoryName);
                        if (this.programPath == null || this.programPath.Length == 0)
                        {
                            FolderBrowserDialog saveFileDialog = new FolderBrowserDialog();
                            saveFileDialog.ShowDialog();
                            this.programPath = saveFileDialog.SelectedPath;
                            string savePath = saveFileDialog.SelectedPath;
                            if (saveFileDialog.SelectedPath.Contains("任务"))
                            {
                                this.programPath = new FileInfo(saveFileDialog.SelectedPath).DirectoryName;// 
                                savePath = this.programPath;
                            }
                            if (this.programPath == null || this.programPath.Length == 0) return;
                            //////////////////////////////////
                            if (ProgramForm.Instance.SaveProgram(savePath)) //this.programPath
                                new Common.UserMessageForm("保存成功").ShowDialog();
                            else
                                new UserMessageForm().ShowDialog("保存失败" + new Exception().ToString());
                        }
                        else
                        {
                            if (ProgramForm.Instance.SaveProgram()) //this.programPath
                                new Common.UserMessageForm("保存成功").ShowDialog();
                            else
                                new UserMessageForm().ShowDialog("保存失败" + new Exception().ToString());
                        }
                        this.配方toolStripStatusLabel.Text = this.programPath;
                        /////////////////////////////////////
                        foreach (ClientSocket socket in SocketConnectManager.Instance.ClientSocketList)
                        {
                            SocketMessage message = new SocketMessage(enSocketInfo.保存配方);
                            message.MesContent = this.programPath;
                            object mesgContent = socket.GetDataAsync(message, true, 30000);
                            message = mesgContent as SocketMessage;
                            if (message != null && message.MesContent?.ToString() == "OK")
                                new UserMessageForm().ShowDialog("保存配方成功", "配方操作");
                            else
                                new UserMessageForm().ShowDialog("保存配方失败", "配方操作");
                        }
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                new UserMessageForm(ex.ToString()).ShowDialog();
            }
        }

        private void 视图TabControl_Selecting(object sender, TabControlCancelEventArgs e)
        {
            if (!this.IsLoad) return;
            switch (UserLoginParamManager.Instance.CurrentUser)
            {
                case enUserName.操作员: // 操作员没有权限处理
                    if (e.TabPage.Text == "程序编辑1")
                    {
                        e.Cancel = true;
                        new UserMessageForm().ShowDialog("操作员无权限点击该页面! 请登录基账户");
                    }
                    if (e.TabPage.Text == "系统配置1")
                    {
                        e.Cancel = true;
                        new UserMessageForm().ShowDialog("操作员无权限点击该页面! 请登录基账户");
                    }
                    break;
                default:
                    break;
            }
        }



        private void ViewForm_SocketMessage(object send, SocketMessageEventArgs e)
        {
            try
            {
                if (e.Message != null)
                {
                    switch (e.Message.GetType().Name)
                    {
                        case nameof(SocketMessage):
                            SocketMessage mesg = e.Message as SocketMessage;
                            switch (mesg.Lable)
                            {
                                case enSocketInfo.下载设备连接配置:
                                    if (this._socket != null && this._socket.Type == enSocketType.服务器)
                                    {
                                        BindingList<DeviceConnectConfigParam> deviceList = mesg.MesContent as BindingList<DeviceConnectConfigParam>;
                                        if (deviceList != null)
                                        {
                                            MotionControlCard.DeviceConnectConfigParamManger.Instance.DeviceConfigParamList.Clear();
                                            foreach (var item in deviceList)
                                            {
                                                MotionControlCard.DeviceConnectConfigParamManger.Instance.DeviceConfigParamList.Add(item);
                                            }
                                            MotionControlCard.DeviceConnectConfigParamManger.Instance.Save();
                                            this._socket.SendDataAsync("下载设备连接配置成功!", true);
                                        }
                                    }
                                    break;
                                case enSocketInfo.加载设备连接配置:
                                    if (this._socket != null && this._socket.Type == enSocketType.服务器)
                                    {
                                        SocketMessage message1 = new SocketMessage(enSocketInfo.加载设备连接配置);
                                        message1.MesContent = MotionControlCard.DeviceConnectConfigParamManger.Instance.DeviceConfigParamList;
                                        this._socket.SendDataAsync(message1, true);
                                    }
                                    break;
                                case enSocketInfo.下载相机连接配置:
                                    if (this._socket != null && this._socket.Type == enSocketType.服务器)
                                    {
                                        BindingList<SensorConnectConfigParam> sensorList = mesg.MesContent as BindingList<SensorConnectConfigParam>;
                                        if (sensorList != null)
                                        {
                                            Sensor.SensorConnectConfigParamManger.Instance.ConfigParamList.Clear();
                                            foreach (var item in sensorList)
                                            {
                                                Sensor.SensorConnectConfigParamManger.Instance.ConfigParamList.Add(item);
                                            }
                                            Sensor.SensorConnectConfigParamManger.Instance.Save();
                                            this._socket.SendDataAsync("下载相机连接配置成功!", true);
                                        }
                                    }
                                    break;
                                case enSocketInfo.加载相机连接配置:
                                    if (this._socket != null && this._socket.Type == enSocketType.服务器)
                                    {
                                        SocketMessage message1 = new SocketMessage(enSocketInfo.加载相机连接配置);
                                        message1.MesContent = Sensor.SensorConnectConfigParamManger.Instance.ConfigParamList;
                                        this._socket.SendDataAsync(message1, true);
                                    }
                                    break;
                                case enSocketInfo.下载光源连接配置:
                                    if (this._socket != null && this._socket.Type == enSocketType.服务器)
                                    {
                                        BindingList<LightConnectConfigParam> lightList = mesg.MesContent as BindingList<LightConnectConfigParam>;
                                        if (lightList != null)
                                        {
                                            LightConnectConfigParamManger.Instance.LightConfigParamList.Clear();
                                            foreach (var item in lightList)
                                            {
                                                LightConnectConfigParamManger.Instance.LightConfigParamList.Add(item);
                                            }
                                            LightConnectConfigParamManger.Instance.Save();
                                            this._socket.SendDataAsync("下载光源连接配置成功!", true);
                                        }
                                    }
                                    break;
                                case enSocketInfo.加载光源连接配置:
                                    if (this._socket != null && this._socket.Type == enSocketType.服务器)
                                    {
                                        SocketMessage message1 = new SocketMessage(enSocketInfo.加载光源连接配置);
                                        message1.MesContent = LightConnectConfigParamManger.Instance.LightConfigParamList;
                                        this._socket.SendDataAsync(message1, true);
                                    }
                                    break;
                                case enSocketInfo.下载坐标系配置:
                                    if (this._socket != null && this._socket.Type == enSocketType.服务器)
                                    {
                                        BindingList<CoordAxisConfigParam> coordSysList = mesg.MesContent as BindingList<CoordAxisConfigParam>;
                                        if (coordSysList != null)
                                        {
                                            CoordSysConfigParamManger.Instance.CoordSysConfigParamList.Clear();
                                            foreach (var item in coordSysList)
                                            {
                                                CoordSysConfigParamManger.Instance.CoordSysConfigParamList.Add(item);
                                            }
                                            CoordSysConfigParamManger.Instance.Save();
                                            this._socket.SendDataAsync("下载坐标系配置成功!", true);
                                        }
                                    }
                                    break;
                                case enSocketInfo.加载坐标系配置:
                                    if (this._socket != null && this._socket.Type == enSocketType.服务器)
                                    {
                                        SocketMessage message1 = new SocketMessage(enSocketInfo.加载坐标系配置);
                                        message1.MesContent = CoordSysConfigParamManger.Instance.CoordSysConfigParamList;
                                        this._socket.SendDataAsync(message1, true);
                                    }
                                    break;
                                case enSocketInfo.下载通信配置:
                                    if (this._socket != null && this._socket.Type == enSocketType.服务器)
                                    {
                                        BindingList<BindingList<CommunicationConfigParam>> communiteList = mesg.MesContent as BindingList<BindingList<CommunicationConfigParam>>;
                                        if (communiteList != null)
                                        {
                                            CommunicationConfigParamManger.Instance.CommunicationParamList.Clear();
                                            foreach (var item in communiteList)
                                            {
                                                CommunicationConfigParamManger.Instance.CommunicationParamList.Add(item);
                                            }
                                            CommunicationConfigParamManger.Instance.Save();
                                            this._socket.SendDataAsync("下载通信配置成功!", true);
                                        }
                                    }
                                    break;
                                case enSocketInfo.加载通信配置:
                                    if (this._socket != null && this._socket.Type == enSocketType.服务器)
                                    {
                                        SocketMessage message1 = new SocketMessage(enSocketInfo.加载通信配置);
                                        message1.MesContent = CommunicationConfigParamManger.Instance.CommunicationParamList;
                                        this._socket.SendDataAsync(message1, true);
                                    }
                                    break;
                                case enSocketInfo.下载相机参数配置:
                                    if (this._socket != null && this._socket.Type == enSocketType.服务器)
                                    {
                                        BindingList<CameraParam> cameraList = mesg.MesContent as BindingList<CameraParam>;
                                        if (cameraList != null)
                                        {
                                            foreach (var item in cameraList)
                                            {
                                                foreach (var item2 in SensorManage.SensorList)
                                                {
                                                    if (item.SensorName == item2.CameraParam.SensorName)
                                                    {
                                                        item2.CameraParam = item;
                                                        item2.CameraParam.Save();
                                                    }
                                                }
                                            }
                                            this._socket.SendDataAsync("下载相机参数配置成功!", true);
                                        }
                                    }
                                    break;
                                case enSocketInfo.加载相机参数配置:
                                    if (this._socket != null && this._socket.Type == enSocketType.服务器)
                                    {
                                        BindingList<CameraParam> cameraList = new BindingList<CameraParam>();
                                        foreach (var item in SensorManage.CameraList)
                                        {
                                            cameraList.Add(item.CameraParam);
                                        }
                                        SocketMessage message1 = new SocketMessage(enSocketInfo.加载相机参数配置);
                                        message1.MesContent = cameraList;
                                        this._socket.SendDataAsync(message1, true);
                                    }
                                    break;
                                case enSocketInfo.下载激光参数配置:
                                    if (this._socket != null && this._socket.Type == enSocketType.服务器)
                                    {
                                        BindingList<LaserParam> laserList = mesg.MesContent as BindingList<LaserParam>;
                                        if (laserList != null)
                                        {
                                            foreach (var item in laserList)
                                            {
                                                foreach (var item2 in SensorManage.SensorList)
                                                {
                                                    if (item.SensorName == item2.LaserParam.SensorName)
                                                    {
                                                        item2.LaserParam = item;
                                                        item2.LaserParam.Save();
                                                    }
                                                }
                                            }
                                            this._socket.SendDataAsync("下载激光参数配置成功!", true);
                                        }
                                    }
                                    break;
                                case enSocketInfo.加载激光参数配置:
                                    if (this._socket != null && this._socket.Type == enSocketType.服务器)
                                    {
                                        BindingList<LaserParam> laserList = new BindingList<LaserParam>();
                                        foreach (var item in SensorManage.LaserList)
                                        {
                                            laserList.Add(item.LaserParam);
                                        }
                                        SocketMessage message1 = new SocketMessage(enSocketInfo.加载激光参数配置);
                                        message1.MesContent = laserList;
                                        this._socket.SendDataAsync(message1, true);
                                    }
                                    break;
                                case enSocketInfo.下载配方:
                                    if (this._socket != null && this._socket.Type == enSocketType.服务器)
                                    {
                                        Dictionary<string, List<TreeNode>> dic = mesg.MesContent as Dictionary<string, List<TreeNode>>;
                                        if (dic != null)
                                        {
                                            foreach (KeyValuePair<string, List<TreeNode>> item in dic)
                                            {
                                                if (ProgramForm.Instance.ProgramDic.ContainsKey(item.Key))
                                                {
                                                    ProgramForm.Instance.ProgramDic[item.Key].TreeView.Nodes.Clear();
                                                    ProgramForm.Instance.ProgramDic[item.Key].TreeView.Nodes.AddRange(item.Value.ToArray());
                                                }
                                            }
                                        }
                                    }
                                    break;
                                case enSocketInfo.保存配方:
                                    if (this._socket != null && this._socket.Type == enSocketType.服务器)
                                    {
                                        string path = mesg.MesContent.ToString();
                                        if (ProgramForm.Instance.SaveProgram(path))
                                            this._socket.SendDataAsync("保存成功", true);
                                        else
                                            this._socket.SendDataAsync("保存失败", true);
                                    }
                                    break;
                                case enSocketInfo.打开配方:
                                    if (this._socket != null && this._socket.Type == enSocketType.服务器)
                                    {
                                        string path = mesg.MesContent.ToString();
                                        if (ProgramForm.Instance.OpenProgram(path))
                                            this._socket.SendDataAsync("打开配方成功", true);
                                        else
                                            this._socket.SendDataAsync("打开配方失败", true);
                                    }
                                    break;
                                case enSocketInfo.加载配方:
                                    if (this._socket != null && this._socket.Type == enSocketType.服务器)
                                    {
                                        Dictionary<string, List<TreeNode>> dic = mesg.MesContent as Dictionary<string, List<TreeNode>>;
                                        List<TreeNode> list = null;
                                        foreach (KeyValuePair<string, TreeViewWrapClass> item in ProgramForm.Instance.ProgramDic)
                                        {
                                            list = new List<TreeNode>();
                                            TreeNode[] nodes = new TreeNode[item.Value.TreeView.Nodes.Count];
                                            item.Value.TreeView.Nodes.CopyTo(nodes, 0);
                                            list.AddRange(nodes);
                                            dic.Add(item.Key, list);
                                        }
                                        SocketMessage message1 = new SocketMessage(enSocketInfo.加载配方);
                                        message1.MesContent = dic;
                                        this._socket.SendDataAsync(message1, true);
                                    }
                                    break;
                                case enSocketInfo.程序运行:
                                case enSocketInfo.程序停止:
                                    if (mesg.MesContent.ToString() == "联机运行")
                                    {
                                        AutoRunThreadPlc.Instance.UnInit();
                                        Thread.Sleep(200);
                                        联机运行toolStripButton.Text = "断开连机";
                                        this.联机运行toolStripButton.Image = global::CxVision.Properties.Resources.停止_50_X_50;
                                        AutoRunThreadPlc.Instance.Init();
                                        SystemParamManager.Instance.SysConfigParam.IsAutoRun = true;
                                    }
                                    else
                                    {
                                        联机运行toolStripButton.Text = "联机运行";
                                        this.联机运行toolStripButton.Image = global::CxVision.Properties.Resources.开始2_50_X_50;
                                        AutoRunThreadPlc.Instance.UnInit();
                                        SystemParamManager.Instance.SysConfigParam.IsAutoRun = false;
                                        SystemParamManager.Instance.SysConfigParam.InterruptSingle = enInterruptType.NONE;
                                    }
                                    break;

                                default: // 默认方法 
                                    LoggerHelper.Error(this.Name + $" 取图时间:{mesg?.Time}");
                                    break;
                            }
                            break;
                        case nameof(String):
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                new UserMessageForm().ShowDialog("ViewForm_SocketMessage" + ex.ToString());
            }
        }

        private void SocketMessage_Receiver(object send, SocketMessageEventArgs e)
        {
            try
            {
                if (e.Message != null)
                {
                    SocketBase _socket = send as SocketBase;
                    if (_socket == null) return;
                    switch (e.Message.GetType().Name)
                    {
                        case nameof(SocketMessage):
                            SocketMessage mesg = e.Message as SocketMessage;
                            switch (mesg.Lable)
                            {
                                case enSocketInfo.设置曝光:
                                    if (_socket != null && _socket.Type == enSocketType.服务器)
                                    {
                                        ISensor _sensor = SensorManage.GetSensor(mesg.Name);
                                        if (_sensor == null)
                                        {
                                            LoggerHelper.Error($"未获取到指定相机名称:{mesg.Name} 的相机");
                                            //new UserMessageForm().ShowDialog($"未获取到指定相机名称:{mesg.Name} 的相机");
                                            return;
                                        }
                                        bool result = _sensor.SetParam("曝光", mesg.MesContent);
                                        if (result)
                                            mesg.MesContent = "OK";
                                        else
                                            mesg.MesContent = "NG";
                                        _socket.SendDataAsync(mesg, true);
                                    }
                                    break;
                                case enSocketInfo.获取曝光:
                                    if (_socket != null && _socket.Type == enSocketType.服务器)
                                    {
                                        ISensor _sensor = SensorManage.GetSensor(mesg.Name);
                                        if (_sensor == null)
                                        {
                                            LoggerHelper.Error($"未获取到指定相机名称:{mesg.Name} 的相机");
                                            //new UserMessageForm().ShowDialog($"未获取到指定相机名称:{mesg.Name} 的相机");
                                            return;
                                        }
                                        object expose = _sensor?.GetParam("曝光");
                                        mesg.MesContent = expose;
                                        _socket.SendDataAsync(mesg, true);
                                    }
                                    else
                                        LoggerHelper.Error(" Socket对象为空或Socket类型为客户端");
                                    break;
                                case enSocketInfo.设置增溢:
                                    if (_socket != null && _socket.Type == enSocketType.服务器)
                                    {
                                        ISensor _sensor = SensorManage.GetSensor(mesg.Name);
                                        if (_sensor == null)
                                        {
                                            LoggerHelper.Error($"未获取到指定相机名称:{mesg.Name} 的相机");
                                            //new UserMessageForm().ShowDialog($"未获取到指定相机名称:{mesg.Name} 的相机");
                                            return;
                                        }
                                        bool result = _sensor.SetParam("增溢", mesg.MesContent);
                                        if (result)
                                            mesg.MesContent = "OK";
                                        else
                                            mesg.MesContent = "NG";
                                        _socket.SendDataAsync(mesg, true);
                                    }
                                    break;
                                case enSocketInfo.获取增溢:
                                    if (_socket != null && _socket.Type == enSocketType.服务器)
                                    {
                                        ISensor _sensor = SensorManage.GetSensor(mesg.Name);
                                        if (_sensor == null)
                                        {
                                            LoggerHelper.Error($"未获取到指定相机名称:{mesg.Name} 的相机");
                                            //new UserMessageForm().ShowDialog($"未获取到指定相机名称:{mesg.Name} 的相机");
                                            return;
                                        }
                                        object gain = _sensor?.GetParam("增溢");
                                        mesg.MesContent = gain;
                                        _socket.SendDataAsync(mesg, true);
                                    }
                                    else
                                        LoggerHelper.Error(" Socket对象为空或Socket类型为客户端");
                                    break;
                                case enSocketInfo.开始实时采集:
                                    if (_socket != null && _socket.Type == enSocketType.服务器)
                                    {
                                        ISensor _sensor = SensorManage.GetSensor(mesg.Name);
                                        if (_sensor == null)
                                        {
                                            LoggerHelper.Error($"未获取到指定相机名称:{mesg.Name} 的相机");
                                            //new UserMessageForm().ShowDialog($"未获取到指定相机名称:{mesg.Name} 的相机");
                                            return;
                                        }
                                        this.stopwatch.Restart();
                                        object trigMode = _sensor?.GetParam(enSocketInfo.获取触发模式);
                                        if (trigMode?.ToString() == "On")
                                            _sensor.SetParam("实时采集", 0);
                                        /////////////////////////////////////////
                                        _sensor.StartTrigger();
                                        _sensor.StopTrigger();
                                        Dictionary<enDataItem, object> dic = _sensor.ReadData();
                                        if (dic.ContainsKey(enDataItem.Image))
                                            mesg.MesContent = dic[enDataItem.Image];
                                        else
                                            mesg.MesContent = null;
                                        this.stopwatch.Stop();
                                        mesg.Time = this.stopwatch.ElapsedMilliseconds;
                                        _socket.SendDataAsync(mesg, true);
                                    }
                                    else
                                        LoggerHelper.Error(" Socket对象为空或Socket类型为客户端");
                                    break;
                                case enSocketInfo.停止实时采集:
                                case enSocketInfo.停止采集:
                                    if (_socket != null && _socket.Type == enSocketType.服务器)
                                    {
                                        ISensor _sensor = SensorManage.GetSensor(mesg.Name);
                                        if (_sensor == null)
                                        {
                                            LoggerHelper.Error($"未获取到指定相机名称:{mesg.Name} 的相机");
                                            //new UserMessageForm().ShowDialog($"未获取到指定相机名称:{mesg.Name} 的相机");
                                            return;
                                        }
                                        this.stopwatch.Restart();
                                        bool result = _sensor.SetParam("外部触发", 0);
                                        this.stopwatch.Stop();
                                        mesg.Time = this.stopwatch.ElapsedMilliseconds;
                                        if (result)
                                            mesg.MesContent = "OK";
                                        else
                                            mesg.MesContent = "NG";
                                        _socket.SendDataAsync(mesg, true);
                                    }
                                    else
                                        LoggerHelper.Error(" Socket对象为空或Socket类型为客户端");
                                    break;
                                case enSocketInfo.获取触发模式:
                                    if (_socket != null && _socket.Type == enSocketType.服务器)
                                    {
                                        ISensor _sensor = SensorManage.GetSensor(mesg.Name);
                                        if (_sensor == null)
                                        {
                                            LoggerHelper.Error($"未获取到指定相机名称:{mesg.Name} 的相机");
                                            //new UserMessageForm().ShowDialog($"未获取到指定相机名称:{mesg.Name} 的相机");
                                            return;
                                        }
                                        object trigMode = _sensor?.GetParam("触发模式");
                                        mesg.MesContent = trigMode;
                                        _socket.SendDataAsync(mesg, true);
                                    }
                                    else
                                        LoggerHelper.Error(" Socket对象为空或Socket类型为客户端");
                                    break;
                                case enSocketInfo.设置触发模式:
                                    if (_socket != null && _socket.Type == enSocketType.服务器)
                                    {
                                        /////////////////////////////////////////////////////
                                        ISensor _sensor = SensorManage.GetSensor(mesg.Name);
                                        if (_sensor == null)
                                        {
                                            LoggerHelper.Error($"未获取到指定相机名称:{mesg.Name} 的相机");
                                            //new UserMessageForm().ShowDialog($"未获取到指定相机名称:{mesg.Name} 的相机");
                                            return;
                                        }
                                        bool result = _sensor.SetParam("触发模式", 0);
                                        if (result)
                                            mesg.MesContent = "OK";
                                        else
                                            mesg.MesContent = "NG";
                                        _socket.SendDataAsync(mesg, true);
                                    }
                                    else
                                        LoggerHelper.Error(" Socket对象为空或Socket类型为客户端");
                                    break;
                                case enSocketInfo.图像采集: // 从相机采集一幅图像
                                    if (_socket != null && _socket.Type == enSocketType.服务器)
                                    {
                                        ISensor _sensor = SensorManage.GetSensor(mesg.Name);
                                        if (_sensor == null)
                                        {
                                            LoggerHelper.Error($"未获取到指定相机名称:{mesg.Name} 的相机");
                                            //new UserMessageForm().ShowDialog($"未获取到指定相机名称:{mesg.Name} 的相机");
                                            return;
                                        }
                                        this.stopwatch.Restart();
                                        ///////////////////////////////////////////////
                                        //object trigMode = _sensor?.GetParam("触发模式");
                                        //if (trigMode?.ToString() == "On" || trigMode?.ToString() == "on")
                                        //    _sensor?.SetParam("实时采集", 0);
                                        _sensor.StartTrigger();
                                        _sensor.StopTrigger();
                                        Dictionary<enDataItem, object> data = _sensor.ReadData();
                                        if (data?.Count > 0)
                                            mesg.MesContent = data[enDataItem.Image];
                                        else
                                            mesg.MesContent = null;
                                        this.stopwatch.Stop();
                                        mesg.Time = this.stopwatch.ElapsedMilliseconds;
                                        _socket.SendDataAsync(mesg, true);
                                    }
                                    else
                                        LoggerHelper.Error(" Socket对象为空或Socket类型为客户端");
                                    break;
                                case enSocketInfo.获取窗口图像: // 从指定窗口上获取一幅当前图像
                                    if (_socket != null && _socket.Type == enSocketType.服务器)
                                    {
                                        if (HWindowManage.HVisualizeView.ContainsKey(mesg.ViewName))
                                            mesg.MesContent = HWindowManage.HVisualizeView[mesg.ViewName].BackImage;
                                        else
                                            mesg.MesContent = null;
                                        ///////////////////////////////////////////////////
                                        _socket.SendDataAsync(mesg, true);
                                    }
                                    else
                                        LoggerHelper.Error(" Socket对象为空或Socket类型为客户端");
                                    break;
                                case enSocketInfo.设置窗口图像: // 从指定窗口上获取一幅当前图像
                                    if (_socket != null && _socket.Type == enSocketType.服务器)
                                    {
                                        switch (mesg.MesContent?.GetType().Name)
                                        {
                                            case nameof(ImageDataClass):
                                                if (HWindowManage.HVisualizeView.ContainsKey(mesg.ViewName))
                                                    HWindowManage.HVisualizeView[mesg.ViewName].BackImage = mesg.MesContent as ImageDataClass;
                                                break;
                                            case nameof(HImage):
                                                HWindowManage.HVisualizeView[mesg.ViewName].BackImage = new ImageDataClass(mesg.MesContent as HImage);
                                                break;
                                        }
                                    }
                                    else
                                        LoggerHelper.Error(" Socket对象为空或Socket类型为客户端");
                                    break;
                                case enSocketInfo.写入相机参数:
                                    if (_socket != null && _socket.Type == enSocketType.服务器)
                                    {
                                        bool result = false;
                                        ISensor _sensor = SensorManage.GetSensor(mesg.Name);
                                        if (_sensor == null)
                                        {
                                            LoggerHelper.Error($"未获取到指定相机名称:{mesg.Name} 的相机");
                                            //new UserMessageForm().ShowDialog($"未获取到指定相机名称:{mesg.Name} 的相机");
                                            return;
                                        }
                                        _sensor.CameraParam.CaliParam.AdjHomMatC02X = 100;
                                        _sensor.CameraParam = mesg.MesContent as CameraParam;
                                        if (_sensor.CameraParam != null)
                                            result = _sensor.CameraParam.Save();
                                        if (result)
                                            mesg.MesContent = "OK";
                                        else
                                            mesg.MesContent = "NG";
                                        _socket.SendDataAsync(mesg, true);
                                    }
                                    break;
                                case enSocketInfo.读取相机参数:
                                    if (_socket != null && _socket.Type == enSocketType.服务器)
                                    {
                                        ISensor _sensor = SensorManage.GetSensor(mesg.Name);
                                        if (_sensor == null)
                                        {
                                            LoggerHelper.Error($"未获取到指定相机名称:{mesg.Name} 的相机");
                                            //new UserMessageForm().ShowDialog($"未获取到指定相机名称:{mesg.Name} 的相机");
                                            return;
                                        }
                                        mesg.MesContent = _sensor?.CameraParam;
                                        _socket.SendDataAsync(mesg, true);
                                    }
                                    else
                                        LoggerHelper.Error(" Socket对象为空或Socket类型为客户端");
                                    break;

                                case enSocketInfo.设置光源亮度:
                                    if (_socket != null && _socket.Type == enSocketType.服务器)
                                    {
                                        ILightControl _light = Light.LightConnectManage.GetLight(mesg.Name);
                                        if (_light == null)
                                        {
                                            LoggerHelper.Error($"未获取到指定光源名称:{mesg.Name} 的光源");
                                            //new UserMessageForm().ShowDialog($"未获取到指定光源名称:{mesg.Name} 的光源");
                                            return;
                                        }
                                        if (_light.SetLight((enLightChannel)mesg.Channel, Convert.ToInt32(mesg.MesContent)))
                                            mesg.MesContent = "OK";
                                        else
                                            mesg.MesContent = "NG";
                                        _socket.SendDataAsync(mesg, true);
                                    }
                                    else
                                        LoggerHelper.Error(" Socket对象为空或Socket类型为客户端");
                                    break;

                                case enSocketInfo.获取光源亮度:
                                    if (_socket != null && _socket.Type == enSocketType.服务器)
                                    {
                                        ILightControl _light = Light.LightConnectManage.GetLight(mesg.Name);
                                        if (_light == null)
                                        {
                                            LoggerHelper.Error($"未获取到指定光源名称:{mesg.Name} 的光源");
                                            //new UserMessageForm().ShowDialog($"未获取到指定光源名称:{mesg.Name} 的光源");
                                            return;
                                        }
                                        int value = _light.GetLight((enLightChannel)mesg.Channel);
                                        mesg.MesContent = value;
                                        _socket.SendDataAsync(mesg, true);
                                    }
                                    else
                                        LoggerHelper.Error(" Socket对象为空或Socket类型为客户端");
                                    break;

                                case enSocketInfo.设置光源参数:
                                    if (_socket != null && _socket.Type == enSocketType.服务器)
                                    {
                                        ILightControl _light = Light.LightConnectManage.GetLight(mesg.Name);
                                        if (_light == null)
                                        {
                                            LoggerHelper.Error($"未获取到指定光源名称:{mesg.Name} 的光源");
                                            //new UserMessageForm().ShowDialog($"未获取到指定光源名称:{mesg.Name} 的光源");
                                            return;
                                        }
                                        string paramType = "", paramValue = "";
                                        string[] paramName = mesg.MesContent.ToString().Split(',');
                                        if (paramName.Length > 0)
                                            paramType = paramName[0];
                                        if (paramName.Length > 1)
                                            paramValue = paramName[1];
                                        if (_light.SetParam(paramType, paramValue))
                                            mesg.MesContent = "OK";
                                        else
                                            mesg.MesContent = "NG";
                                        _socket.SendDataAsync(mesg, true);
                                    }
                                    else
                                        LoggerHelper.Error(" Socket对象为空或Socket类型为客户端");
                                    break;

                                case enSocketInfo.获取光源参数:
                                    if (_socket != null && _socket.Type == enSocketType.服务器)
                                    {
                                        ILightControl _light = Light.LightConnectManage.GetLight(mesg.Name);
                                        if (_light == null)
                                        {
                                            LoggerHelper.Error($"未获取到指定光源名称:{mesg.Name} 的光源");
                                            //new UserMessageForm().ShowDialog($"未获取到指定光源名称:{mesg.Name} 的光源");
                                            return;
                                        }
                                        object value = _light.GetParam(mesg.MesContent);
                                        mesg.MesContent = value;
                                        _socket.SendDataAsync(mesg, true);
                                    }
                                    else
                                        LoggerHelper.Error(" Socket对象为空或Socket类型为客户端");
                                    break;

                                case enSocketInfo.关闭光源:
                                    if (_socket != null && _socket.Type == enSocketType.服务器)
                                    {
                                        ILightControl _light = Light.LightConnectManage.GetLight(mesg.Name);
                                        if (_light == null)
                                        {
                                            LoggerHelper.Error($"未获取到指定光源名称:{mesg.Name} 的光源");
                                            //new UserMessageForm().ShowDialog($"未获取到指定光源名称:{mesg.Name} 的光源");
                                            return;
                                        }
                                        if (_light.Close((enLightChannel)mesg.Channel))
                                            mesg.MesContent = "OK";
                                        else
                                            mesg.MesContent = "NG";
                                        _socket.SendDataAsync(mesg, true);
                                    }
                                    else
                                        LoggerHelper.Error(" Socket对象为空或Socket类型为客户端");
                                    break;

                                case enSocketInfo.打开光源:
                                    if (_socket != null && _socket.Type == enSocketType.服务器)
                                    {
                                        ILightControl _light = Light.LightConnectManage.GetLight(mesg.Name);
                                        if (_light == null)
                                        {
                                            LoggerHelper.Error($"未获取到指定光源名称:{mesg.Name} 的光源");
                                            //new UserMessageForm().ShowDialog($"未获取到指定光源名称:{mesg.Name} 的光源");
                                            return;
                                        }
                                        if (_light.Open((enLightChannel)mesg.Channel))
                                            mesg.MesContent = "OK";
                                        else
                                            mesg.MesContent = "NG";
                                        _socket.SendDataAsync(mesg, true);
                                    }
                                    else
                                        LoggerHelper.Error(" Socket对象为空或Socket类型为客户端");
                                    break;

                                case enSocketInfo.通信指令: // 用于中转&分发运控端发过来的指令
                                    if (_socket != null && _socket.Type == enSocketType.客户端)
                                    {
                                        IMotionControl _card = MotionCardManage.GetCard(mesg.Name);
                                        if (_card != null)
                                        {
                                            switch (_card.GetType().Name)
                                            {
                                                case nameof(SocketClientDeviceRFID):
                                                    SocketClientDeviceRFID clientDeviceRFID = _card as SocketClientDeviceRFID;
                                                    if (clientDeviceRFID != null)
                                                    {
                                                        SocketCommandRfid commandRfid = mesg.MesContent as SocketCommandRfid;
                                                        if (commandRfid == null) return;
                                                        if (clientDeviceRFID.Dic.ContainsKey(commandRfid.CamStation))
                                                        {
                                                            clientDeviceRFID.Dic[commandRfid.CamStation] = commandRfid;
                                                            /////////////////////////////////////////////////////////
                                                            clientDeviceRFID.Dic[commandRfid.CamStation].TriggerFromSocket = 1;
                                                        }
                                                        else
                                                        {
                                                            clientDeviceRFID.Dic.Add(commandRfid.CamStation, commandRfid);
                                                            /////////////////////////////////////////////////////////
                                                            clientDeviceRFID.Dic[commandRfid.CamStation].TriggerFromSocket = 1;
                                                        }
                                                    }
                                                    break;
                                                case nameof(SocketServerDeviceRFID):
                                                    _card = MotionCardManage.GetCard(mesg.Name);
                                                    SocketServerDeviceRFID serverDeviceRFID = _card as SocketServerDeviceRFID;
                                                    if (serverDeviceRFID != null)
                                                    {
                                                        SocketCommandRfid commandRfid = mesg.MesContent as SocketCommandRfid;
                                                        if (commandRfid == null) return;
                                                        serverDeviceRFID.ServerSocket.SendDataAsync(commandRfid); //serverDeviceRFID.ServerSocket.Serialize(commandRfid)
                                                    }
                                                    break;
                                            }
                                        }
                                        else
                                            LoggerHelper.Error($"未获取到指定光源名称:{mesg.Name} 的光源");
                                    }
                                    break;

                                //// 多种设备配置  
                                case enSocketInfo.下载设备连接配置:
                                    if (_socket != null && _socket.Type == enSocketType.服务器)
                                    {
                                        BindingList<DeviceConnectConfigParam> deviceList = mesg.MesContent as BindingList<DeviceConnectConfigParam>;
                                        if (deviceList != null)
                                        {
                                            this.Invoke(new Action(() =>
                                            {
                                                MotionControlCard.DeviceConnectConfigParamManger.Instance.DeviceConfigParamList.Clear();
                                                foreach (var item in deviceList)
                                                {
                                                    MotionControlCard.DeviceConnectConfigParamManger.Instance.DeviceConfigParamList.Add(item);
                                                }
                                            }));
                                            bool result = MotionControlCard.DeviceConnectConfigParamManger.Instance.Save();
                                            if (result)
                                                mesg.MesContent = "OK";
                                            else
                                                mesg.MesContent = "NG";
                                            _socket.SendDataAsync(mesg, true);
                                        }
                                    }
                                    break;
                                case enSocketInfo.加载设备连接配置:
                                    if (_socket != null && _socket.Type == enSocketType.服务器)
                                    {
                                        SocketMessage message1 = new SocketMessage(enSocketInfo.加载设备连接配置);
                                        message1.MesContent = MotionControlCard.DeviceConnectConfigParamManger.Instance.DeviceConfigParamList;
                                        _socket.SendDataAsync(message1, true);
                                    }
                                    break;
                                case enSocketInfo.下载相机连接配置:
                                    if (_socket != null && _socket.Type == enSocketType.服务器)
                                    {
                                        BindingList<SensorConnectConfigParam> sensorList = mesg.MesContent as BindingList<SensorConnectConfigParam>;
                                        if (sensorList != null)
                                        {
                                            this.Invoke(new Action(() =>
                                            {
                                                Sensor.SensorConnectConfigParamManger.Instance.ConfigParamList.Clear();
                                                foreach (var item in sensorList)
                                                {
                                                    Sensor.SensorConnectConfigParamManger.Instance.ConfigParamList.Add(item);
                                                }
                                            }));
                                            bool result = Sensor.SensorConnectConfigParamManger.Instance.Save();
                                            if (result)
                                                mesg.MesContent = "OK";
                                            else
                                                mesg.MesContent = "NG";
                                            _socket.SendDataAsync(mesg, true);
                                        }
                                    }
                                    break;
                                case enSocketInfo.加载相机连接配置:
                                    if (_socket != null && _socket.Type == enSocketType.服务器)
                                    {
                                        SocketMessage message1 = new SocketMessage(enSocketInfo.加载相机连接配置);
                                        message1.MesContent = Sensor.SensorConnectConfigParamManger.Instance.ConfigParamList;
                                        _socket.SendDataAsync(message1, true);
                                    }
                                    break;
                                case enSocketInfo.下载光源连接配置:
                                    if (_socket != null && _socket.Type == enSocketType.服务器)
                                    {
                                        BindingList<LightConnectConfigParam> lightList = mesg.MesContent as BindingList<LightConnectConfigParam>;
                                        if (lightList != null)
                                        {
                                            this.Invoke(new Action(() =>
                                            {
                                                LightConnectConfigParamManger.Instance.LightConfigParamList.Clear();
                                                foreach (var item in lightList)
                                                {
                                                    LightConnectConfigParamManger.Instance.LightConfigParamList.Add(item);
                                                }
                                            }));
                                            bool result = LightConnectConfigParamManger.Instance.Save();
                                            if (result)
                                                mesg.MesContent = "OK";
                                            else
                                                mesg.MesContent = "NG";
                                            _socket.SendDataAsync(mesg, true);
                                        }
                                    }
                                    break;
                                case enSocketInfo.加载光源连接配置:
                                    if (_socket != null && _socket.Type == enSocketType.服务器)
                                    {
                                        SocketMessage message1 = new SocketMessage(enSocketInfo.加载光源连接配置);
                                        message1.MesContent = LightConnectConfigParamManger.Instance.LightConfigParamList;
                                        _socket.SendDataAsync(message1, true);
                                    }
                                    break;
                                case enSocketInfo.下载坐标系配置:
                                    if (_socket != null && _socket.Type == enSocketType.服务器)
                                    {
                                        BindingList<CoordAxisConfigParam> coordSysList = mesg.MesContent as BindingList<CoordAxisConfigParam>;
                                        if (coordSysList != null)
                                        {
                                            this.Invoke(new Action(() =>
                                            {
                                                CoordSysConfigParamManger.Instance.CoordSysConfigParamList.Clear();
                                                foreach (var item in coordSysList)
                                                {
                                                    CoordSysConfigParamManger.Instance.CoordSysConfigParamList.Add(item);
                                                }
                                            }));
                                            bool result = CoordSysConfigParamManger.Instance.Save();
                                            if (result)
                                                mesg.MesContent = "OK";
                                            else
                                                mesg.MesContent = "NG";
                                            _socket.SendDataAsync(mesg, true);
                                        }
                                    }
                                    break;
                                case enSocketInfo.加载坐标系配置:
                                    if (_socket != null && _socket.Type == enSocketType.服务器)
                                    {
                                        SocketMessage message1 = new SocketMessage(enSocketInfo.加载坐标系配置);
                                        message1.MesContent = CoordSysConfigParamManger.Instance.CoordSysConfigParamList;
                                        _socket.SendDataAsync(message1, true);
                                    }
                                    break;
                                case enSocketInfo.下载通信配置:
                                    if (_socket != null && _socket.Type == enSocketType.服务器)
                                    {
                                        BindingList<BindingList<CommunicationConfigParam>> communiteList = mesg.MesContent as BindingList<BindingList<CommunicationConfigParam>>;
                                        if (communiteList != null)
                                        {
                                            this.Invoke(new Action(() =>
                                            {
                                                foreach (var item in CommunicationConfigParamManger.Instance.CommunicationParamList)
                                                {
                                                    item?.Clear();
                                                }
                                                for (int i = 0; i < communiteList.Count; i++)
                                                {
                                                    foreach (var item2 in communiteList[i])
                                                    {
                                                        CommunicationConfigParamManger.Instance.CommunicationParamList[i].Add(item2);
                                                    }
                                                }
                                            }));
                                            bool result = CommunicationConfigParamManger.Instance.Save();
                                            if (result)
                                                mesg.MesContent = "OK";
                                            else
                                                mesg.MesContent = "NG";
                                            _socket.SendDataAsync(mesg, true);
                                        }
                                    }
                                    break;
                                case enSocketInfo.加载通信配置:
                                    if (_socket != null && _socket.Type == enSocketType.服务器)
                                    {
                                        SocketMessage message1 = new SocketMessage(enSocketInfo.加载通信配置);
                                        message1.MesContent = CommunicationConfigParamManger.Instance.CommunicationParamList;
                                        _socket.SendDataAsync(message1, true);
                                    }
                                    break;
                                case enSocketInfo.下载相机参数配置:
                                    if (_socket != null && _socket.Type == enSocketType.服务器)
                                    {
                                        bool result = true;
                                        BindingList<CameraParam> cameraList = mesg.MesContent as BindingList<CameraParam>;
                                        if (cameraList != null)
                                        {
                                            foreach (var item in cameraList)
                                            {
                                                foreach (var item2 in SensorManage.SensorList)
                                                {
                                                    if (item.SensorName == item2.CameraParam.SensorName)
                                                    {
                                                        item2.CameraParam = item;
                                                        result = result && item2.CameraParam.Save();
                                                    }
                                                }
                                            }
                                            if (result)
                                                mesg.MesContent = "OK";
                                            else
                                                mesg.MesContent = "NG";
                                            _socket.SendDataAsync(mesg, true);
                                        }
                                    }
                                    break;
                                case enSocketInfo.加载相机参数配置:
                                    if (_socket != null && _socket.Type == enSocketType.服务器)
                                    {
                                        BindingList<CameraParam> cameraList = new BindingList<CameraParam>();
                                        foreach (var item in SensorManage.CameraList)
                                        {
                                            cameraList.Add(item.CameraParam);
                                        }
                                        SocketMessage message1 = new SocketMessage(enSocketInfo.加载相机参数配置);
                                        message1.MesContent = cameraList;
                                        _socket.SendDataAsync(message1, true);
                                    }
                                    break;
                                case enSocketInfo.下载激光参数配置:
                                    if (_socket != null && _socket.Type == enSocketType.服务器)
                                    {
                                        bool result = true;
                                        BindingList<LaserParam> laserList = mesg.MesContent as BindingList<LaserParam>;
                                        if (laserList != null)
                                        {
                                            foreach (var item in laserList)
                                            {
                                                foreach (var item2 in SensorManage.SensorList)
                                                {
                                                    if (item.SensorName == item2.LaserParam.SensorName)
                                                    {
                                                        item2.LaserParam = item;
                                                        result = result && item2.LaserParam.Save();
                                                    }
                                                }
                                            }
                                            if (result)
                                                mesg.MesContent = "OK";
                                            else
                                                mesg.MesContent = "NG";
                                            _socket.SendDataAsync(mesg, true);
                                        }
                                    }
                                    break;
                                case enSocketInfo.加载激光参数配置:
                                    if (_socket != null && _socket.Type == enSocketType.服务器)
                                    {
                                        BindingList<LaserParam> laserList = new BindingList<LaserParam>();
                                        foreach (var item in SensorManage.LaserList)
                                        {
                                            laserList.Add(item.LaserParam);
                                        }
                                        SocketMessage message1 = new SocketMessage(enSocketInfo.加载激光参数配置);
                                        message1.MesContent = laserList;
                                        _socket.SendDataAsync(message1, true);
                                    }
                                    break;
                                case enSocketInfo.保存配方:
                                    if (_socket != null && _socket.Type == enSocketType.服务器)
                                    {
                                        string path = mesg?.MesContent?.ToString();
                                        if (ProgramForm.Instance.SaveProgram(path))
                                            mesg.MesContent = "OK";
                                        else
                                            mesg.MesContent = "NG";
                                        _socket.SendDataAsync(mesg, true);
                                    }
                                    break;
                                case enSocketInfo.打开配方:
                                    if (_socket != null && _socket.Type == enSocketType.服务器)
                                    {
                                        string path = mesg?.MesContent?.ToString();
                                        if (ProgramForm.Instance.OpenProgram(path))
                                            mesg.MesContent = "OK";
                                        else
                                            mesg.MesContent = "NG";
                                        _socket.SendDataAsync(mesg, true);
                                    }
                                    break;
                                case enSocketInfo.新建配方:
                                    if (_socket != null && _socket.Type == enSocketType.服务器)
                                    {
                                        ProgramForm.Instance.NewProgram();
                                        GlobalProgram.ProgramItems.Clear();
                                        mesg.MesContent = "OK";
                                        _socket.SendDataAsync(mesg, true);
                                    }
                                    break;
                                case enSocketInfo.下载配方:
                                    if (_socket != null && _socket.Type == enSocketType.服务器)
                                    {
                                        bool result = false;
                                        Dictionary<string, List<TreeNode>> dic = mesg.MesContent as Dictionary<string, List<TreeNode>>;
                                        if (dic != null)
                                        {
                                            ProgramForm.Instance.ProgramPath = mesg.Name;
                                            foreach (KeyValuePair<string, List<TreeNode>> item in dic)
                                            {
                                                if (ProgramForm.Instance.ProgramDic.ContainsKey(item.Key))
                                                {
                                                    this.Invoke(new Action(() =>
                                                    {
                                                        ProgramForm.Instance.ProgramDic[item.Key]?.TreeView?.Nodes?.Clear();
                                                        ProgramForm.Instance.ProgramDic[item.Key].TreeView.Nodes.AddRange(item.Value.ToArray());
                                                        result = true;
                                                    }));
                                                }
                                            }
                                            if (result)
                                                mesg.MesContent = "OK";
                                            else
                                                mesg.MesContent = "NG";
                                            _socket.SendDataAsync(mesg, true);
                                        }
                                    }
                                    break;
                                case enSocketInfo.加载配方:
                                    if (_socket != null && _socket.Type == enSocketType.服务器)
                                    {
                                        Dictionary<string, List<TreeNode>> dic = new Dictionary<string, List<TreeNode>>();
                                        List<TreeNode> list = null;
                                        foreach (KeyValuePair<string, TreeViewWrapClass> item in ProgramForm.Instance.ProgramDic)
                                        {
                                            list = new List<TreeNode>();
                                            TreeNode[] nodes = new TreeNode[item.Value.TreeView.Nodes.Count];
                                            item.Value.TreeView.Nodes.CopyTo(nodes, 0);
                                            list.AddRange(nodes);
                                            dic.Add(item.Key, list);
                                        }
                                        SocketMessage message1 = new SocketMessage(enSocketInfo.加载配方);
                                        message1.Name = ProgramForm.Instance.ProgramPath;
                                        message1.MesContent = dic;
                                        _socket.SendDataAsync(message1, true);
                                    }
                                    break;
                                case enSocketInfo.程序运行:
                                case enSocketInfo.程序停止:
                                case enSocketInfo.联机运行:
                                case enSocketInfo.断开联机:
                                    if (mesg.MesContent?.ToString() == "联机运行")
                                    {
                                        AutoRunThreadPlc.Instance.UnInit();
                                        Thread.Sleep(200);
                                        联机运行toolStripButton.Text = "断开连机";
                                        this.联机运行toolStripButton.Image = global::CxVision.Properties.Resources.停止_50_X_50;
                                        AutoRunThreadPlc.Instance.Init();
                                        SystemParamManager.Instance.SysConfigParam.IsAutoRun = true;
                                        mesg.MesContent = "OK";
                                        _socket.SendDataAsync(mesg, true);
                                    }
                                    else
                                    {
                                        联机运行toolStripButton.Text = "联机运行";
                                        this.联机运行toolStripButton.Image = global::CxVision.Properties.Resources.开始2_50_X_50;
                                        AutoRunThreadPlc.Instance.UnInit();
                                        SystemParamManager.Instance.SysConfigParam.IsAutoRun = false;
                                        SystemParamManager.Instance.SysConfigParam.InterruptSingle = enInterruptType.NONE;
                                        mesg.MesContent = "OK";
                                        _socket.SendDataAsync(mesg, true);
                                    }
                                    break;

                                default: // 默认方法 
                                    LoggerHelper.Error($" 取图时间:{mesg?.Time}");
                                    break;
                            }
                            break;
                        case nameof(String):
                            break;
                        case nameof(SocketCommandRfid):

                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                new UserMessageForm().ShowDialog("ViewForm_SocketMessage" + ex.ToString());
            }
        }

        #region 公开接口

        /// <summary>
        /// 启动方法
        /// </summary>
        public void Run(bool isRun = false)
        {
            if (isRun)
            {
                AutoRunThreadPlc.Instance.UnInit();
                Thread.Sleep(200);
                this.联机运行toolStripButton.Text = "断开连机";
                this.联机运行toolStripButton.Image = global::CxVision.Properties.Resources.停止_50_X_50;
                AutoRunThreadPlc.Instance.Init();
                SystemParamManager.Instance.SysConfigParam.IsAutoRun = true;
            }
            else
            {
                this.联机运行toolStripButton.Text = "联机运行";
                this.联机运行toolStripButton.Image = global::CxVision.Properties.Resources.开始2_50_X_50;
                AutoRunThreadPlc.Instance.UnInit();
                SystemParamManager.Instance.SysConfigParam.IsAutoRun = false;
                SystemParamManager.Instance.SysConfigParam.InterruptSingle = enInterruptType.用户复位中断;
            }
        }
        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="userName"></param>
        public void SetUser(string userName = "Operator")
        {
            switch (userName)
            {
                case "Engineer":
                    UserLoginParamManager.Instance.CurrentUser = enUserName.工程师;
                    break;
                case "Manager":
                    UserLoginParamManager.Instance.CurrentUser = enUserName.管理员;
                    break;
                case "Manufacturer":
                    UserLoginParamManager.Instance.CurrentUser = enUserName.开发人员;
                    break;
                case "Unsign":
                case "Operator":
                default:
                    UserLoginParamManager.Instance.CurrentUser = enUserName.操作员;
                    break;
            }
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
            foreach (var item in ProgramForm.Instance.ProgramDic.Values)
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

        #endregion

    }


}

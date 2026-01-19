using Common;
using FunctionBlock;
using Light;
using MotionControlCard;
using Sensor;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;
using System.Threading;
using System.Windows.Forms;

namespace CxVision
{
    public partial class MainFormEmbed : Form
    {
        private Form programForm;
        private SensorManage sensorlist = new SensorManage();
        private MotionCardManage card = new MotionCardManage();
        private LightConnectManage light = new LightConnectManage();
        private string programPath = ""; // 程序文件路径
        private List<Form> ListForm = new List<Form>();
        private System.Threading.Timer timer;
        private bool IsLoad = false;
        private bool _initSensor = false;
        public MainFormEmbed()
        {
            InitializeComponent();
            this.TopLevel = false;
            this.TopMost = false;
            SystemParamManager.Instance.SysConfigParam.IsFormTopMost = true;
            this.panel2.Hide();
            this.项目名称label.Hide();
            this.statusStrip1.Hide();
            HalconDotNet.HSystem.ResetObjDb(5000, 5000, 0); // 初始化Halcon系统
            //this.InitScript();
            // 读取配置文件
            SystemParamManager.Instance.SysConfigParam.IsAutoRun = false;
            GlobalVariable.pConfig = new Common.ParamConfig().ReadParamConfig();
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
            // CPUMonitor.Instance.Init();
            //// 读取用户
            UserLoginParamManager.Instance.Read();
            UserManager.Read();
            /////////////////////////////////////
            this.MaximumSize = new Size(Screen.PrimaryScreen.WorkingArea.Width, Screen.PrimaryScreen.WorkingArea.Height);
            this.WindowState = FormWindowState.Normal;
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
            ViewConfigParamManager.Instance.Init(this.视图TabControl);
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
            this.timer = new System.Threading.Timer(this.DateTimeMonitor, null, 0, 2000);
            ////////
            this.视图TabControl.DoubleBuffere(true);
        }



        private void MainForm_Load(object sender, EventArgs e)
        {
            ///////////////////////////////////////////////
            Rectangle rect = Screen.GetWorkingArea(this);
            ScreenManager.Instance.ScreenParam.SetScaleParam(rect.Width, rect.Height);
            foreach (TabPage item in this.视图TabControl.TabPages)
            {
                this.视图TabControl.SelectedTab = item; // 激活每一个Page页
            }
            this.视图TabControl.SelectedIndex = 0;
            ////////////////////////////////////////////
            // UserLoginParamManager.Instance.LoginParam.UserChange += new EventHandler(this.UserChange_Event);
            UserLoginParamManager.Instance.UserChange += new EventHandler(this.UserChange_Event);
            UserLoginParamManager.Instance.CurrentUser = enUserName.操作员; // 读取成功后需要触发一次事件 
            AutoRunThreadPlc.Instance.RecipeInfo += new RecipeEventHandler(this.Recipe_Event);
            this.addLableContextMenu();
            this.IsLoad = true;
            if (string.IsNullOrEmpty(SystemParamManager.Instance.SysConfigParam.ProjectName))
                this.项目名称label.Text = "项目名称:";
            else
                this.项目名称label.Text = SystemParamManager.Instance.SysConfigParam.ProjectName;
            /////////////////////////////////////////////////////
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
                        //addViewForm.Close();
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
                        if (configParam == null) return;
                        RenameForm renameForm = new RenameForm(this.视图TabControl.SelectedTab.Text);
                        DialogResult dialogResult = renameForm.ShowDialog();
                        if (dialogResult == DialogResult.OK)
                        {
                            if (renameForm.ReName != null && renameForm.ReName.Length > 0)
                            {
                                this.视图TabControl.SelectedTab.Text = renameForm.ReName;
                                configParam.Text = renameForm.ReName;
                            }
                        }
                        break;
                        ///////////////////////////////////////////////    
                }
            }
            catch (Exception ex)
            {
                new Common.UserMessageForm(ex.ToString()).ShowDialog();
            }
        }
        public void UserChange_Event(object sender, EventArgs e)
        {
            try
            {
                if (sender == null) return;
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
                            LoggerHelper.Info("配方另存为成功");
                        }
                        else
                        {
                            CommunicationConfigParamManger.Instance.WriteValue(e.CoordSysName, enCommunicationCommand.ResultToPlc, 2);
                            CommunicationConfigParamManger.Instance.WriteValue(e.CoordSysName, enCommunicationCommand.ResultToSocket, "NG");
                            CommunicationConfigParamManger.Instance.WriteValue(e.CoordSysName, enCommunicationCommand.TriggerToPlc, 1);
                            LoggerHelper.Info("配方另存为失败");
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
                                    LoggerHelper.Info("配方切换成功");
                                }
                                else
                                {
                                    this.programPath = ProgramForm.Instance.ProgramPath;
                                    this.程序1toolStripStatusLabel.Text = ProgramForm.Instance.ProgramPath;
                                    /////////////////////////////////////////////////////////////////////////
                                    CommunicationConfigParamManger.Instance.WriteValue(e.CoordSysName, enCommunicationCommand.ProgramNoToPlc, path);
                                    CommunicationConfigParamManger.Instance.WriteValue(e.CoordSysName, enCommunicationCommand.ResultToPlc, 2);
                                    CommunicationConfigParamManger.Instance.WriteValue(e.CoordSysName, enCommunicationCommand.ResultToSocket, "NG");
                                    CommunicationConfigParamManger.Instance.WriteValue(e.CoordSysName, enCommunicationCommand.TriggerToPlc, 1);
                                    LoggerHelper.Error("配方切换失败,指定的程序路径不存在(Program Path No Exist!!!)");
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
                            LoggerHelper.Info("配方切换失败,路径为空");
                        }

                        ///////////// 切换配方时初始化一次 ///////////////
                       if(!this._initSensor && SystemParamManager.Instance.SysConfigParam.IsInitSensor)
                        {
                            foreach (var item in SensorManage.SensorList)
                            {
                                item?.SetParam("实时采集", 0);
                                Thread.Sleep(1000);
                                item?.SetParam("停止采集", 0);
                            }
                            this._initSensor = true;
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
                                CommunicationConfigParamManger.Instance.WriteValue(e.CoordSysName, enCommunicationCommand.TriggerToPlc, 1);
                                LoggerHelper.Info($"{path}登录成功");
                                break;
                            case "supper":
                            case "Supper":
                            case "engineer":
                            case "Engineer":
                            case "工程师":
                            case "管理员":
                                UserLoginParamManager.Instance.CurrentUser = enUserName.工程师;
                                this.Invoke(new Action(() =>
                                {
                                    this.用户label.Text = path;
                                }));
                                CommunicationConfigParamManger.Instance.WriteValue(e.CoordSysName, enCommunicationCommand.ProgramNo, path);
                                CommunicationConfigParamManger.Instance.WriteValue(e.CoordSysName, enCommunicationCommand.ResultToPlc, 1);
                                CommunicationConfigParamManger.Instance.WriteValue(e.CoordSysName, enCommunicationCommand.ResultToSocket, "OK");
                                CommunicationConfigParamManger.Instance.WriteValue(e.CoordSysName, enCommunicationCommand.TriggerToPlc, 1);
                                LoggerHelper.Info($"{path}登录成功");
                                break;
                            case "开发人员":
                            case "厂商人员":
                            case "manufacturer":
                            case "Manufacturer":
                                UserLoginParamManager.Instance.CurrentUser = enUserName.开发人员;
                                this.Invoke(new Action(() =>
                                {
                                    this.用户label.Text = path;
                                }));
                                CommunicationConfigParamManger.Instance.WriteValue(e.CoordSysName, enCommunicationCommand.ProgramNo, path);
                                CommunicationConfigParamManger.Instance.WriteValue(e.CoordSysName, enCommunicationCommand.ResultToPlc, 1);
                                CommunicationConfigParamManger.Instance.WriteValue(e.CoordSysName, enCommunicationCommand.ResultToSocket, "OK");
                                CommunicationConfigParamManger.Instance.WriteValue(e.CoordSysName, enCommunicationCommand.TriggerToPlc, 1);
                                LoggerHelper.Info($"{path}登录成功");
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
                            CommunicationConfigParamManger.Instance.WriteValue(e.CoordSysName, enCommunicationCommand.Lable, string.Join(",", lable));
                            CommunicationConfigParamManger.Instance.WriteValue(e.CoordSysName, enCommunicationCommand.LableLength, string.Join(",", lable).Length);
                            CommunicationConfigParamManger.Instance.WriteValue(e.CoordSysName, enCommunicationCommand.ResultToPlc, 1);
                            CommunicationConfigParamManger.Instance.WriteValue(e.CoordSysName, enCommunicationCommand.ResultToSocket, "OK");
                            CommunicationConfigParamManger.Instance.WriteValue(e.CoordSysName, enCommunicationCommand.LableTrigger, 1);
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
                CommunicationConfigParamManger.Instance.WriteValue(e.CoordSysName, enCommunicationCommand.ResultToPlc, 2);
                CommunicationConfigParamManger.Instance.WriteValue(e.CoordSysName, enCommunicationCommand.ResultToSocket, "NG");
                CommunicationConfigParamManger.Instance.WriteValue(e.CoordSysName, enCommunicationCommand.TriggerToPlc, 1);
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
                //DialogResult dialogResult = new UserMessageForm().ShowDialog("确定要退出程序吗？", "退出程序");
                //if (dialogResult == DialogResult.OK)
                //{
                AutoRunThreadPlc.Instance.RecipeInfo -= new RecipeEventHandler(this.Recipe_Event);
                UserLoginParamManager.Instance.UserChange -= new EventHandler(this.UserChange_Event);
                UserLoginParamManager.Instance.CurrentUser = enUserName.操作员; // 读取成功后需要触发一次事件 .UserChange -= new EventHandler(this.UserChange_Event);
                AutoRunThreadPlc.Instance.UnInit();
                //AutoRunThreadSocket.Instance.UnInit();
                ////////////////////////
                this.timer.Change(0, Timeout.Infinite);
                this.sensorlist.DisConnect();
                this.card.DisConnect();
                this.light.DisConnect();
                this.timer.Dispose();
                SocketConnectManager.Instance.UnInitSocket();
                /////////////////
                for (int i = this.ListForm.Count - 1; i >= 0; i--)
                {
                    this.ListForm[i]?.Close();
                }
                //foreach (var item in this.ListForm)
                //{
                //    item?.Close();
                //}
                Thread.Sleep(1000);
                ////////////////////// 关闭控制台
                ConsoleHelper.FreeConsole();
                //}
                //else
                //    e.Cancel = true;
                //ConsoleHelper.FreeConsole();
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
                    if (SystemParamManager.Instance.SysConfigParam.IsAutoRun) // 用一个颜色来提示程序的状态
                        this.运行状态checkBox.Image = CxVision.Properties.Resources.绿色灯炮_25_X_25;
                    else
                        this.运行状态checkBox.Image = CxVision.Properties.Resources.黄色灯炮_25_X_25;
                    this.日期toolStripStatusLabel.Text = DateTime.Now.ToString("yyyy/MM/dd");
                    this.时间toolStripStatusLabel.Text = DateTime.Now.ToString("HH:mm:ss");
                    //////////////////////////////////////////////////////////////////////
                    this.用户label.Text = "登录:" + UserLoginParamManager.Instance.CurrentUser.ToString();
                    //////////////////////////////////////////////////////////////////////
                    float cpu = CPUMonitor.Instance.GetValue();
                    MemoryInfo memoryInfo = MemoryMonitor.Instance.getMemoryInfo();
                    this.Cpu分辨率toolStripStatusLabel.Text = Math.Round(cpu, 1) + "%";
                    this.总内存toolStripStatusLabel.Text = Math.Round(memoryInfo.totalPhys / Math.Pow(1024, 3), 2) + " G" + " / " + Math.Round(memoryInfo.availPhys / Math.Pow(1024, 3), 2) + " G";
                    this.内存利用率toolStripStatusLabel.Text = memoryInfo.memoryLoad + "%";
                    //////////////////////////////////////////////////////////////////////
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

        #region  窗体绽放功能 
        //private const int Guying_HTLEFT = 10;
        //private const int Guying_HTRIGHT = 11;
        //private const int Guying_HTTOP = 12;
        //private const int Guying_HTTOPLEFT = 13;
        //private const int Guying_HTTOPRIGHT = 14;
        //private const int Guying_HTBOTTOM = 15;
        //private const int Guying_HTBOTTOMLEFT = 0x10;
        //private const int Guying_HTBOTTOMRIGHT = 17;
        //protected override void WndProc(ref Message m)
        //{
        //    switch (m.Msg)
        //    {
        //        case 0x0084:
        //            base.WndProc(ref m);
        //            Point vPoint = new Point((int)m.LParam & 0xFFFF,
        //                (int)m.LParam >> 16 & 0xFFFF);
        //            vPoint = PointToClient(vPoint);
        //            if (vPoint.X <= 5)
        //                if (vPoint.Y <= 5)
        //                    m.Result = (IntPtr)Guying_HTTOPLEFT;
        //                else if (vPoint.Y >= ClientSize.Height - 5)
        //                    m.Result = (IntPtr)Guying_HTBOTTOMLEFT;
        //                else m.Result = (IntPtr)Guying_HTLEFT;
        //            else if (vPoint.X >= ClientSize.Width - 5)
        //                if (vPoint.Y <= 5)
        //                    m.Result = (IntPtr)Guying_HTTOPRIGHT;
        //                else if (vPoint.Y >= ClientSize.Height - 5)
        //                    m.Result = (IntPtr)Guying_HTBOTTOMRIGHT;
        //                else m.Result = (IntPtr)Guying_HTRIGHT;
        //            else if (vPoint.Y <= 2)
        //                m.Result = (IntPtr)Guying_HTTOP;
        //            else if (vPoint.Y >= ClientSize.Height - 5)
        //                m.Result = (IntPtr)Guying_HTBOTTOM;
        //            break;
        //        default:
        //            base.WndProc(ref m);
        //            break;
        //    }
        //}
        #endregion

        #region 防止改变窗口大小时控件闪烁功能
        //protected override CreateParams CreateParams   //
        //{
        //    get
        //    {
        //        CreateParams cp = base.CreateParams;
        //        cp.ExStyle |= 0x02000000; // Turn on WS_EX_COMPOSITED
        //        return cp;
        //    }
        //}
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




        private void eventLog1_EntryWritten(object sender, System.Diagnostics.EntryWrittenEventArgs e)
        {
            string name = e.Entry.Message;
        }

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
                            //new Common.UserMessageForm("保存成功").ShowDialog();
                            new Common.UserMessageForm("保存成功").ShowDialog();
                        else
                            //new UserMessageForm().ShowDialog("保存失败" + new Exception().ToString());
                            new Common.UserMessageForm("保存失败" + new Exception().ToString()).ShowDialog();
                    }
                    else
                    {
                        if (ProgramForm.Instance.SaveProgram()) // 如果在已有路径下保存，则不需要给路径
                            //new Common.UserMessageForm("保存成功").ShowDialog();
                            new Common.UserMessageForm("保存成功").ShowDialog();
                        else
                            //new UserMessageForm().ShowDialog("保存失败" + new Exception().ToString());
                            new Common.UserMessageForm("保存失败" + new Exception().ToString()).ShowDialog();
                    }
                    this.配方toolStripStatusLabel.Text = this.programPath;
                    ////////////////////  每次保存发送一次标签更新  //////////////////// 
                    //foreach (var item2 in AcqSourceManage.Instance.AcqSourceList)
                    //{
                    //    string[] lable = this.GetLableName((int)item2.CoordSysName);
                    //    if (lable != null && lable.Length > 0)
                    //    {
                    //        CommunicationConfigParamManger.Instance.WriteValue(item2.CoordSysName, enCommunicationCommand.Lable, string.Join("|", lable));
                    //        CommunicationConfigParamManger.Instance.WriteValue(item2.CoordSysName, enCommunicationCommand.LableLength, string.Join("|", lable).Length * 2);
                    //        CommunicationConfigParamManger.Instance.WriteValue(item2.CoordSysName, enCommunicationCommand.ResultToPlc, 1);
                    //        CommunicationConfigParamManger.Instance.WriteValue(item2.CoordSysName, enCommunicationCommand.ResultToSocket, "OK");
                    //        CommunicationConfigParamManger.Instance.WriteValue(item2.CoordSysName, enCommunicationCommand.LableTrigger, 1);
                    //        LoggerHelper.Info($"标签内容:{string.Join("|", lable)}", item2.Sensor?.Name);
                    //        LoggerHelper.Info("发送标签成功", item2.Sensor?.Name);
                    //    }
                    //    else
                    //    {
                    //        //CommunicationConfigParamManger.Instance.WriteValue(item2.CoordSysName, enCommunicationCommand.LableLength, 0);
                    //        CommunicationConfigParamManger.Instance.WriteValue(item2.CoordSysName, enCommunicationCommand.ResultToPlc, 2);
                    //        CommunicationConfigParamManger.Instance.WriteValue(item2.CoordSysName, enCommunicationCommand.ResultToSocket, "NG");
                    //        CommunicationConfigParamManger.Instance.WriteValue(item2.CoordSysName, enCommunicationCommand.LableTrigger, 1);
                    //        LoggerHelper.Error($"标签内容为空或长度为0", item2.Sensor?.Name);
                    //        LoggerHelper.Error("发送标签失败", item2.Sensor?.Name);
                    //    }
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
                        //new Common.UserMessageForm("保存成功").ShowDialog();
                        new Common.UserMessageForm("保存成功").ShowDialog();
                        ProgramForm.Instance.OpenProgram(this.programPath); // 如果是另存为，那么需要打另存为的程序
                    }
                    else
                        new Common.UserMessageForm("保存失败" + new Exception().ToString()).ShowDialog();
                    //new UserMessageForm().ShowDialog("保存失败" + new Exception().ToString());
                    ///////////////////////////////////////////////////////////////////////////
                    this.配方toolStripStatusLabel.Text = this.programPath;
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
            switch (this.语言toolStripComboBox.SelectedItem.ToString()) //
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
                    form.TopMost = true;
                    form.ShowInTaskbar = true;
                    form.Show();
                    break;

                //////////////////////////////////
                case "参数设置":
                    form = new ParamConfigForm();
                    form.StartPosition = FormStartPosition.Manual;
                    form.Location = System.Windows.Forms.Cursor.Position;
                    form.Owner = this;
                    form.TopMost = true;
                    form.ShowInTaskbar = true;
                    form.Show();
                    break;

                //////////////////////////////////
                case "光源配置":
                    form = new LightConnectConfigManageForm();
                    form.StartPosition = FormStartPosition.Manual;
                    form.Location = System.Windows.Forms.Cursor.Position;
                    form.Owner = this;
                    form.TopMost = true;
                    form.ShowInTaskbar = true;
                    form.Show();
                    break;

                //////////////////////////////////
                case "运动控制卡配置":
                    form = new DeviceConnectConfigParamManageForm();
                    form.StartPosition = FormStartPosition.Manual;
                    form.Location = System.Windows.Forms.Cursor.Position;
                    form.Owner = this;
                    form.TopMost = true;
                    form.ShowInTaskbar = true;
                    form.Show();
                    break;

                case "采集源配置":
                    form = new AcqSourceConfigForm();
                    form.StartPosition = FormStartPosition.Manual;
                    form.Location = System.Windows.Forms.Cursor.Position;
                    form.Owner = this;
                    form.TopMost = true;
                    form.ShowInTaskbar = true;
                    form.Show();
                    break;

                case "程序配置":
                    form = new ProgramConfigParamForm();
                    form.StartPosition = FormStartPosition.Manual;
                    form.Location = System.Windows.Forms.Cursor.Position;
                    form.Owner = this;
                    form.TopMost = true;
                    form.ShowInTaskbar = true;
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

                case "通信配置":
                    case nameof(通信配置ToolStripMenuItem):
                    form = new DeviceCommunicationConfigForm();
                    form.StartPosition = FormStartPosition.Manual;
                    Point point = ProjectManagerFormNew.Instance.工程配置tabControl.SelectedTab.PointToScreen(ProjectManagerFormNew.Instance.Location);
                    form.Owner = this;
                    form.TopMost = true;
                    form.ShowInTaskbar = true;
                    form.Location = point;
                    form.Width = ProjectManagerFormNew.Instance.工程配置tabControl.SelectedTab.Width;
                    form.Height = ProjectManagerFormNew.Instance.工程配置tabControl.SelectedTab.Height;
                    form.Show();
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
                        this.Invoke(new Action(() =>
                        {
                            e.ClickedItem.Text = "断开连机";
                            e.ClickedItem.ForeColor = Color.Red;
                            this.联机运行toolStripButton.Image = global::CxVision.Properties.Resources.停止_50_X_50;
                        }));
                        //this.联机运行toolStripButton.Image = global::CxVision.Properties.Resources.停止_50_X_50;
                        AutoRunThreadPlc.Instance.Init();
                        SystemParamManager.Instance.SysConfigParam.IsAutoRun = true;
                    }
                    else
                    {
                        this.Invoke(new Action(() =>
                        {
                            e.ClickedItem.Text = "联机运行";
                            e.ClickedItem.ForeColor = Color.Black;
                            this.联机运行toolStripButton.Image = global::CxVision.Properties.Resources.开始2_50_X_50;
                        }));
                        //this.联机运行toolStripButton.Image = global::CxVision.Properties.Resources.开始2_50_X_50;
                        AutoRunThreadPlc.Instance.UnInit();
                        SystemParamManager.Instance.SysConfigParam.IsAutoRun = false;
                        SystemParamManager.Instance.SysConfigParam.InterruptSingle = enInterruptType.用户复位中断;
                    }
                    break;
                case "查看点激光":
                    form = new PointLaserForm();
                    form.Owner = this;
                    form.TopMost = true;
                    form.ShowInTaskbar = true;
                    form.Show();
                    break;
                case "查看线激光":
                    form = new LineLaserForm();
                    form.Owner = this;
                    form.TopMost = true;
                    form.ShowInTaskbar = true;
                    form.Show();
                    break;

                case "程序节点":
                    List<TreeNode> listTreeNode = new List<TreeNode>();
                    foreach (var item in ProgramForm.Instance.ProgramDic.Values)
                    {
                        listTreeNode.AddRange(item.GetTreeViewNodeTag());
                    }
                    UserMessageForm messageForm = new UserMessageForm();
                    messageForm.ShowDialog(listTreeNode.Count.ToString());
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
                loginForm.TopMost = true;
                loginForm.ShowInTaskbar = true;
                loginForm.Show();
                //LoginForm.Instance.Show();
            }
            catch (Exception ex)
            {
                new Common.UserMessageForm(ex.ToString()).ShowDialog();
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
                    form.TopMost = true;
                    form.ShowInTaskbar = true;
                    form.Owner = this;
                    form.Show();
                    break;
                case "对射标定工具":
                    form = new CalibrateDoubleLaserForm();
                    form.TopMost = true;
                    form.ShowInTaskbar = true;
                    form.Owner = this;
                    form.Show();
                    break;
                case "运动控制":
                    form = new JogMotionForm();  //new JogMotionControlForm()
                    form.TopMost = true;
                    form.ShowInTaskbar = true;
                    form.Owner = this;
                    form.Show();
                    break;
                case "线性校准":
                    form = new LinearCalibrateForm();  //new JogMotionControlForm()
                    form.TopMost = true;
                    form.ShowInTaskbar = true;
                    form.Owner = this;
                    form.Show();
                    break;
                case "面阵校准":
                    form = new MachineCalibForm();  //new JogMotionControlForm()
                    form.TopMost = true;
                    form.ShowInTaskbar = true;
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
                    form.TopMost = true;
                    form.ShowInTaskbar = true;
                    form.Owner = this;
                    form.Show();
                    break;
                case "矩阵标定相机":
                    form = new MatrixCalibrateCameraParamForm();
                    form.TopMost = true;
                    form.ShowInTaskbar = true;
                    form.Owner = this;
                    form.Show();
                    break;
                case "N点标定相机":
                    form = new CamNPointCalibParamSimpleForm();
                    form.TopMost = true;
                    form.ShowInTaskbar = true;
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
            string text = e.ClickedItem.Text;
            switch (text)
            {
                case "相机&点激光标定":
                    form = new CameraPointLaserCalibrateForm();  //new JogMotionControlForm()
                    form.TopMost = true;
                    form.ShowInTaskbar = true;
                    form.Owner = this;
                    form.Show();
                    break;
                case "相机&线激光标定":
                    form = new CameraLineLaserCalibrateForm();
                    form.TopMost = true;
                    form.ShowInTaskbar = true;
                    form.Owner = this;
                    form.Show();
                    break;
                case "相机&面激光标定":
                    form = new CameraFaceLaserCalibrateForm();
                    form.TopMost = true;
                    form.ShowInTaskbar = true;
                    form.Owner = this;
                    form.Show();
                    break;
                case "相机&胶枪标定":
                    form = new CameraGlueGunCalibrateForm();
                    form.TopMost = true;
                    form.ShowInTaskbar = true;
                    form.Owner = this;
                    form.Show();
                    break;
                case "相机&机器人&夹抓标定":
                    form = new RebotJawCalibrateForm();
                    form.TopMost = true;
                    form.ShowInTaskbar = true;
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
                    form.TopMost = true;
                    form.ShowInTaskbar = true;
                    form.Owner = this;
                    form.Show();
                    break;
                case "标定点激光":
                    form = new FaceSensorCalibrateForm();
                    form.TopMost = true;
                    form.ShowInTaskbar = true;
                    form.Owner = this;
                    form.Show();
                    break;
                case "标定面激光":
                    form = new FaceSensorCalibrateForm();
                    form.TopMost = true;
                    form.ShowInTaskbar = true;
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
                    form.TopMost = true;
                    form.ShowInTaskbar = true;
                    form.Owner = this;
                    form.Show();
                    break;
                case "面阵补偿":
                    form = new MachineCalibForm();  //new JogMotionControlForm()
                    form.TopMost = true;
                    form.ShowInTaskbar = true;
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
                    form.TopMost = true;
                    form.ShowInTaskbar = true;
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
            string name = e.ClickedItem.Text;
            switch (name.Trim())
            {
                case "保存视图布局":
                    if (ViewConfigParamManager.Instance.Save())  // 尽量不要将保存放在 窗体关闭里，因为有可能会强制停止
                    {
                        new UserMessageForm().ShowDialog("保存成功!!!");
                    }
                    else
                    {
                        new UserMessageForm().ShowDialog("保存失败!!!");
                    }
                    break;
                case "添加视图":
                    AddViewForm addViewForm = new AddViewForm();
                    addViewForm.TopMost = true;
                    addViewForm.ShowInTaskbar = true;
                    addViewForm.ShowDialog();
                    if (addViewForm.IsCancel) return;
                    Form form = ViewConfigParamManager.Instance.AddFormView(this.视图TabControl.SelectedTab, addViewForm);
                    this.ListForm.Add(form); // 将在主窗体上打开的所有窗体保存下来，先关闭他们再关闭主窗体
                    break;
                case "编辑视图项":
                    ViewElementForm viewElement = new ViewElementForm();
                    viewElement.TopMost = true;
                    viewElement.ShowInTaskbar = true;
                    viewElement.Show();
                    break;
                case "清空视图配置":
                    ViewConfigParamManager.Instance.Clear();
                    ViewConfigParamManager.Instance.Save();
                    break;
                default:
                    break;
            }
        }

        public void 运行工具条toolStrip_ItemClicked_1(object sender, ToolStripItemClickedEventArgs e)
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

                default:
                case "联机运行toolStripButton":
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

                case "断开连机toolStripButton":
                    AutoRunThreadPlc.Instance.UnInit();
                    SystemParamManager.Instance.SysConfigParam.IsAutoRun = false;
                    SystemParamManager.Instance.SysConfigParam.InterruptSingle = enInterruptType.用户复位中断;
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
                new Common.UserMessageForm(ex.ToString()).ShowDialog();
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
        public void 用户label_Click(object sender, EventArgs e)
        {
            try
            {
                LoginForm loginForm = new LoginForm();
                loginForm.StartPosition = FormStartPosition.CenterScreen;
                loginForm.TopMost = true;
                loginForm.ShowInTaskbar = true;
                loginForm.ShowDialog();
                //LoginForm.Instance.Show();
            }
            catch (Exception ex)
            {
                new Common.UserMessageForm(ex.ToString()).ShowDialog();
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
                        ///////////////////////////////
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
                        ///////////////////////////////
                        break;

                    case "保存ToolStripButton":
                        this.programPath = ProgramForm.Instance.ProgramPath;
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
                                //new Common.UserMessageForm("保存成功").ShowDialog();
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
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                new Common.UserMessageForm(ex.ToString()).ShowDialog();
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


        #region 公开接口
        /// <summary>
        /// 
        /// 启动方法
        /// </summary>
        public void Run(bool isRun = false)
        {
            try
            {
                if (isRun)
                {
                    AutoRunThreadPlc.Instance.UnInit();
                    Thread.Sleep(200);
                    this.联机运行ToolStripMenuItem.Text = "断开连机";
                    this.联机运行ToolStripMenuItem.ForeColor = Color.Red;
                    this.联机运行toolStripButton.Image = global::CxVision.Properties.Resources.停止_50_X_50;
                    AutoRunThreadPlc.Instance.Init();
                    SystemParamManager.Instance.SysConfigParam.IsAutoRun = true;
                    LoggerHelper.Info("启用联机运行");
                }
                else
                {
                    this.联机运行ToolStripMenuItem.Text = "联机运行";
                    this.联机运行ToolStripMenuItem.ForeColor = Color.Black;
                    this.联机运行toolStripButton.Image = global::CxVision.Properties.Resources.开始2_50_X_50;
                    AutoRunThreadPlc.Instance.UnInit();
                    SystemParamManager.Instance.SysConfigParam.IsAutoRun = false;
                    SystemParamManager.Instance.SysConfigParam.InterruptSingle = enInterruptType.用户复位中断;
                    LoggerHelper.Info("断开联机运行");
                }
            }
            catch (Exception ex)
            {
                new UserMessageForm().ShowDialog(ex.ToString());
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
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public string[] GetLableName(int index)
        {
            string[] lables = new string[0];
            try
            {
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
            }
            catch (Exception ex)
            {
                LoggerHelper.Error("获取程序标签失败:", ex);
            }
            return lables;
        }

        #endregion

        private void 运行状态checkBox_Click(object sender, EventArgs e)
        {

            //try
            //{
            //    if (this.联机运行ToolStripMenuItem.Text == "联机运行")
            //    {
            //        this.Invoke(new Action(() =>
            //        {
            //            this.联机运行ToolStripMenuItem.Text = "断开连机";
            //            this.联机运行ToolStripMenuItem.ForeColor = Color.Red;
            //            this.联机运行toolStripButton.Image = global::CxVision.Properties.Resources.停止_50_X_50;
            //        }));
            //        //this.联机运行toolStripButton.Image = global::CxVision.Properties.Resources.停止_50_X_50;
            //        AutoRunThreadPlc.Instance.Init();
            //        SystemParamManager.Instance.SysConfigParam.IsAutoRun = true;
            //    }
            //    else
            //    {
            //        this.Invoke(new Action(() =>
            //        {
            //            this.联机运行ToolStripMenuItem.Text = "联机运行";
            //            this.联机运行ToolStripMenuItem.ForeColor = Color.Black;
            //            this.联机运行toolStripButton.Image = global::CxVision.Properties.Resources.开始2_50_X_50;
            //        }));
            //        this.联机运行toolStripButton.Image = global::CxVision.Properties.Resources.开始2_50_X_50;
            //        AutoRunThreadPlc.Instance.UnInit();
            //        SystemParamManager.Instance.SysConfigParam.IsAutoRun = false;
            //        SystemParamManager.Instance.SysConfigParam.InterruptSingle = enInterruptType.用户复位中断;
            //    }
            //}
            //catch (Exception ex)
            //{
            //    new UserMessageForm(ex.ToString()).ShowDialog();
            //}

        }




    }

}

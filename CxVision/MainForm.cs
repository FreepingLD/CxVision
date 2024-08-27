using Common;
using FunctionBlock;
using Light;
using MotionControlCard;
using Sensor;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using System.Runtime.CompilerServices;



namespace CxVision
{
    public partial class MainForm : Form
    {
        private Form programForm;
        private SensorManage sensorlist = new SensorManage();
        private MotionCardManage card = new MotionCardManage();
        private LightConnectManage light = new LightConnectManage();
        private string programPath = ""; // 程序文件路径
        private List<Form> ListForm = new List<Form>();
        private System.Threading.Timer timer;
        public MainForm()
        {
            InitializeComponent();
            HalconDotNet.HTuple hTuple = HalconDotNet.HSystem.GetSystem("halcon_xl");
            HalconDotNet.HOperatorSet.ResetObjDb(50000, 50000, 0); // 初始化Halcon系统
            // 读取配置文件
            GlobalVariable.pConfig = new Common.ParamConfig().ReadParamConfig(Application.StartupPath + "\\" + "ParamConfig.txt");
            CommandConfigManger.Instance.Read(); // 读取命令配置
            ViewConfigParamManager.Instance.Read();
            SocketConnectManager.Instance.InitSocket();
            CoordSysConfigParamManger.Instance.Read();
            CommunicationConfigParamManger.Instance.Read();
            RobotJawParaManager.Instance.Read();
            SystemParamManager.Instance.Read();
            ////////////////////////////////////////////////////////////////////////////
            sensorlist.Connect();
            card.Connect();
            light.Connect();
            // 初始化采集源
            AcqSourceManage.Instance.Read();
            /////////////////////////
            AddForm(this.坐标系TabPage, new CoordSysConfigParamManageForm()); //
            AddForm(this.标定参数tabPage, new CaliParaManagerForm());
            AddForm(this.夹抓tabPage, new RobotJawParaManagerForm());
            AddForm(this.通信配置tabPage, new DeviceCommunicationConfigForm());
            AddForm(this.SensorTabPage, new SensorConnectConfigParamMangerForm());
            AddForm(this.设备splitContainer.Panel1, new DeviceConnectConfigParamManageForm());
            AddForm(this.设备splitContainer.Panel2, new LightConnectConfigManageForm());
            AddForm(this.SocketTabPage, new SocketForm());

            /////////////////////////////////////
            this.WindowState = FormWindowState.Maximized;

            //if (MessageBox.Show("机台上电后必需执行机台回零,如已回零可忽略", "机台回零", MessageBoxButtons.OKCancel) == DialogResult.OK)
            //{
            //    if (MotionCardManage.CurrentCard != null)
            //        MotionCardManage.CurrentCard.MultyAxisHome(enAxisName.XY轴, 10);
            //    else
            //        MessageBox.Show("运动控制卡打开失败");
            //}
            this.timer = new System.Threading.Timer(this.DateTimeMonitor, null, 0, 2000);
            ViewConfigParamManager.Instance.Init(this.tabControl1);
            //////////////////////////////////////////////////////
            foreach (TabPage item in this.tabControl1.TabPages)
            {
                foreach (var form in item.Controls)
                {
                    if (form is Form)
                        this.ListForm.Add(form as Form);
                }
            }



        }


        private void MainForm_Load(object sender, EventArgs e)
        {
            Rectangle rect = Screen.GetWorkingArea(this);
            ScreenManager.Instance.ScreenParam.SetScaleParam(rect.Width, rect.Height);

            foreach (TabPage item in this.tabControl1.TabPages)
            {
                this.tabControl1.SelectedTab = item; // 激活每一个Page页
            }
            this.tabControl1.SelectedIndex = 0;
            //////////////
            if (SystemParamManager.Instance.SysConfigParam.ShieldDetect)
                this.屏蔽checkBox.Text = "启用检测";
            else
                this.屏蔽checkBox.Text = "屏蔽检测";
            //// 登录事件
            UserLoginParamManager.Instance.Read();
            //UserLoginParamManager.Instance.LoginParam.UserChange += new EventHandler(this.UserChange_Event);
            UserLoginParamManager.Instance.UserChange += new EventHandler(this.UserChange_Event);
            UserLoginParamManager.Instance.CurrentUser = enUserName.操作员; // 读取成功后需要触发一次事件 
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
            this.tabControl1.ContextMenuStrip = ContextMenuStrip1;
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
                        ViewConfigParamManager.Instance.AddTabPage(this.tabControl1, addViewForm);
                        //addViewForm.Close();
                        break;
                    ///////////////////////////////////////////////          
                    case "移除":
                        ViewConfigParamManager.Instance.RemoveTabPage(this.tabControl1, this.tabControl1.SelectedTab);
                        break;
                    ///////////////////////////////////////////////    
                    case "左移":
                        ViewConfigParamManager.Instance.LeftShiftTabPage(this.tabControl1, this.tabControl1.SelectedTab);
                        break;
                    ///////////////////////////////////////////////    
                    case "右移":
                        ViewConfigParamManager.Instance.RightShiftTabPage(this.tabControl1, this.tabControl1.SelectedTab);
                        break;
                    case "重命名":
                        ViewConfigParam configParam = ViewConfigParamManager.Instance.GetViewConfigParam(this.tabControl1.SelectedTab.Name);
                        if (configParam == null) return;
                        RenameForm renameForm = new RenameForm(this.tabControl1.SelectedTab.Text);
                        DialogResult dialogResult = renameForm.ShowDialog();
                        if (dialogResult == DialogResult.OK)
                        {
                            if (renameForm.ReName != null && renameForm.ReName.Length > 0)
                            {
                                this.tabControl1.SelectedTab.Text = renameForm.ReName;
                                configParam.Text = renameForm.ReName;
                            }
                        }
                        break;
                        ///////////////////////////////////////////////    
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
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
                        this.tabControl1.ContextMenuStrip = null;
                        break;
                    case enUserName.工程师:
                        this.tabControl1.ContextMenuStrip = null;
                        break;
                    case enUserName.开发人员:
                        this.addContextMenu();
                        break;
                }
            }
            catch
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

        private void 工具toolStripMenuItem_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            Form form;
            string name = e.ClickedItem.Text;
            switch (name)
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
        private void 机台校准toolStripMenuItem_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
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

        private void 相机标定toolStripMenuItem_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
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
                    form = new CamNPointCalibParamForm();
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
        private void 激光标定toolStripMenuItem_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
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
        private void 相机激光标定toolStripMenuItem1_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

            Form form;
            string text = e.ClickedItem.Text;
            switch (text)
            {
                case "相机&点激光标定":
                    form = new CameraPointLaserCalibrateForm();  //new JogMotionControlForm()
                    form.Owner = this;
                    form.Show();
                    break;
                case "相机&线激光标定":
                    form = new CameraLineLaserCalibrateForm();
                    form.Owner = this;
                    form.Show();
                    break;
                case "相机&面激光标定":
                    form = new CameraFaceLaserCalibrateForm();
                    form.Owner = this;
                    form.Show();
                    break;
                case "相机&胶枪标定":
                    form = new CameraGlueGunCalibrateForm();
                    form.Owner = this;
                    form.Show();
                    break;
                case "相机&机器人&夹抓标定":
                    form = new RebotJawCalibrateForm();
                    form.Owner = this;
                    form.Show();
                    break;
                    
                default:
                    break;
            }
        }

        private void 配置toolStripMenuItem_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            Form form;
            string name = e.ClickedItem.Text;
            switch (name.Trim())
            {
                //////////////////////////////////
                case "传感器配置":
                    // form = new SensorConfigForm();
                    form = new SensorConnectConfigParamMangerForm();
                    form.Owner = this;
                    form.Show();
                    break;
                //////////////////////////////////
                case "参数设置":
                    form = new ParamConfigForm();
                    form.Owner = this;
                    form.Show();
                    break;
                //////////////////////////////////
                case "光源配置":
                    form = new LightConnectConfigManageForm();
                    form.Owner = this;
                    form.Show();
                    break;
                //////////////////////////////////
                case "运动控制卡配置":
                    form = new DeviceConnectConfigParamManageForm();
                    form.Owner = this;
                    form.Show();
                    break;
                case "采集源配置":
                    form = new AcqSourceConfigForm();
                    form.Owner = this;
                    form.Show();
                    break;
                case "程序配置":
                    form = new ProgramConfigParamForm();
                    form.Owner = this;
                    form.Show();
                    break;
                default:
                    break;
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                DialogResult dialogResult = MessageBox.Show("确定要退也程序吗？", "退出程序", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
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
                    for (int i = this.ListForm.Count - 1; i >=0 ; i--)
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
                }
                else
                    e.Cancel = true;
            }
            catch (Exception ex)
            {

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
                        this.toolStripStatusLabel1.Text = "";
                        ///////////////////////////////
                        foreach (var item2 in ProgramManager.Instance.ProgramList)
                        {
                            item2.IsActive = false;
                        }
                        ProgramManager.Instance.Save();
                        break;

                    case "打开ToolStripButton":
                        FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
                        folderBrowserDialog.ShowDialog();
                        this.programPath = folderBrowserDialog.SelectedPath;
                        if (folderBrowserDialog.SelectedPath.Contains("任务"))
                            this.programPath = new FileInfo(folderBrowserDialog.SelectedPath).DirectoryName;  // 不能到任务那一级
                        ///////////////////////////////////
                        if (this.programPath == null || this.programPath.Trim().Length == 0) return;
                        ProgramForm.Instance.OpenProgram(this.programPath);
                        this.toolStripStatusLabel1.Text = this.programPath;
                        ///////////////////////////////
                        foreach (var item2 in ProgramManager.Instance.ProgramList)
                        {
                            item2.IsActive = false;
                        }
                        ProgramManager.Instance.Save();
                        break;

                    case "保存ToolStripButton":
                        if (this.programPath == null || this.programPath.Length == 0)
                        {
                            if (ProgramConfigParamManager.Instance.ProgramParamList != null && ProgramConfigParamManager.Instance.ProgramParamList.Count > 0)
                            {
                                if (ProgramConfigParamManager.Instance.ProgramParamList[0].IsAuto)
                                {
                                    if (ProgramForm.Instance.SaveProgram(ProgramConfigParamManager.Instance.ProgramParamList[0].ProgramPath))
                                        MessageBox.Show("保存成功");
                                    else
                                        MessageBox.Show("保存失败" + new Exception().ToString());
                                }
                                else
                                {
                                    FolderBrowserDialog saveFileDialog = new FolderBrowserDialog();
                                    saveFileDialog.ShowDialog();
                                    this.programPath = saveFileDialog.SelectedPath;
                                    if (saveFileDialog.SelectedPath.Contains("任务"))
                                        this.programPath = new FileInfo(saveFileDialog.SelectedPath).DirectoryName;// ;
                                    if (this.programPath == null || this.programPath.Length == 0) return;
                                    //////////////////////////////////
                                    if (ProgramForm.Instance.SaveProgram(this.programPath))
                                        MessageBox.Show("保存成功");
                                    else
                                        MessageBox.Show("保存失败" + new Exception().ToString());
                                }
                            }
                        }
                        else
                        {
                            if (ProgramForm.Instance.SaveProgram(this.programPath))
                                MessageBox.Show("保存成功");
                            else
                                MessageBox.Show("保存失败" + new Exception().ToString());
                        }
                        this.toolStripStatusLabel1.Text = this.programPath;
                        break;

                    case "打印PToolStripButton":

                        break;

                    case "剪切UToolStripButton":

                        break;

                    case "复制CToolStripButton":

                        break;

                    case "粘贴PToolStripButton":

                        break;

                    case "帮助LToolStripButton":

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

        private void 运行工具条toolStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            ToolStripItem item = e.ClickedItem;
            string name = item.Name;
            int count = 1;
            switch (name)
            {
                case "运行toolStripButton":
                    ProgramForm.Instance.Run(this.运行toolStripButton, count);
                    this.停止toolStripButton.Enabled = true;
                    this.运行toolStripButton.Enabled = false;
                    break;

                case "停止toolStripButton":
                    ProgramForm.Instance.Stop();
                    this.运行toolStripButton.Enabled = true;
                    this.停止toolStripButton.Enabled = true;
                    break;

                case "联机运行toolStripButton":
                    AutoRunThreadPlc.Instance.Init();
                    //AutoRunThreadSocket.Instance.Init(this.tabControl1);
                    this.停止toolStripButton.Enabled = true;
                    this.断开连机toolStripButton.Enabled = true;
                    this.联机运行toolStripButton.Enabled = false;
                    this.运行toolStripButton.Enabled = false;
                    this.停止toolStripButton.Enabled = false;
                    this.循环次数toolStripButton.Enabled = false;
                    break;

                case "断开连机toolStripButton":
                    AutoRunThreadPlc.Instance.UnInit();
                    //AutoRunThreadSocket.Instance.UnInit();
                    this.运行toolStripButton.Enabled = true;
                    this.停止toolStripButton.Enabled = true;
                    this.断开连机toolStripButton.Enabled = true;
                    this.联机运行toolStripButton.Enabled = true;
                    this.循环次数toolStripButton.Enabled = true;
                    break;

                case "循环次数toolStripButton":
                    LoopForm loopForm = new LoopForm();
                    loopForm.ShowDialog(); // 显示对话框来响应等待
                    if (loopForm.IsClose) return;
                    count = loopForm.loopCount;
                    loopForm?.Dispose();
                    ProgramForm.Instance.Run(this.运行toolStripButton, count);
                    this.停止toolStripButton.Enabled = true;
                    this.运行toolStripButton.Enabled = false;
                    break;

                case "视图toolStripButton":
                    /////////////////////////////
                    AddViewForm addViewForm = new AddViewForm();
                    addViewForm.ShowDialog();
                    if (addViewForm.IsCancel) return;
                    Form form = ViewConfigParamManager.Instance.AddFormView(this.tabControl1.SelectedTab, addViewForm);
                    this.ListForm.Add(form); // 将在主窗体上打开的所有窗体保存下来，先关闭他们再关闭主窗体
                    //addViewForm.Close();
                    break;

                default:
                    break;
            }
        }

        private void 调试toolStripMenuItem_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            Form form;
            string name = e.ClickedItem.Text;
            switch (name.Trim())
            {
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
        private void 参数toolStripMenuItem_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            string name = e.ClickedItem.Text;
            switch (name.Trim())
            {
                case "保存视图布局":
                    ViewConfigParamManager.Instance.Save();  // 尽量不要将保存放在 窗体关闭里，因为有可能会强制停止
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
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            // if (this.programForm == null) return;
            try
            {
                switch (GlobalVariable.pConfig.MeasureEnvironment)
                {
                    case Common.enMeasureEnvironmentConfig.影像仪测量:
                        if (ViewConfigParamManager.Instance.DataForm == null)
                        {
                            ViewConfigParamManager.Instance.DataForm = new DataDisplayForm(new ViewConfigParam());
                        }
                        if (this.显示数据checkBox.Checked)
                        {
                            ViewConfigParamManager.Instance.DataForm?.Show();
                            ViewConfigParamManager.Instance.DataForm.Owner = this;
                            //ProjectorForm.dataForm.Show();
                            //ProjectorForm.dataForm.Owner = this;
                        }
                        else
                            ViewConfigParamManager.Instance.DataForm?.Hide();
                        //ProjectorForm.dataForm.Hide();
                        break;
                    case Common.enMeasureEnvironmentConfig.相机激光测量:

                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.F5:
                    ProgramForm.Instance.Run(this.运行toolStripButton, 1);
                    break;
                case Keys.Escape:
                    ProgramForm.Instance.Stop();
                    break;
            }
        }

        private void 文件toolStripMenuItem_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            ToolStripItem item = e.ClickedItem;
            string name = item.Name;
            FileOperate fo = new FileOperate();
            switch (name)
            {
                case "新建toolStripMenuItem":
                    this.programPath = "";
                    ProgramForm.Instance.NewProgram();
                    GlobalProgram.ProgramItems.Clear();
                    this.toolStripStatusLabel1.Text = "";
                    ///////////////////////////////  重置配方程序 
                    foreach (var item2 in ProgramManager.Instance.ProgramList)
                    {
                        item2.IsActive = false;
                    }
                    ProgramManager.Instance.Save();
                    break;

                case "打开toolStripMenuItem":
                    FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
                    folderBrowserDialog.ShowDialog();
                    this.programPath = folderBrowserDialog.SelectedPath;
                    if (folderBrowserDialog.SelectedPath.Contains("任务"))
                        this.programPath = new FileInfo(folderBrowserDialog.SelectedPath).DirectoryName;  // 不能到任务那一级
                                                                                                          ///////////////////////////////////
                    if (this.programPath == null || this.programPath.Trim().Length == 0) return;
                    //pf.TreeViewWrapClass.OpenProgram(this.programPath);
                    ProgramForm.Instance.OpenProgram(this.programPath);
                    this.toolStripStatusLabel1.Text = this.programPath;
                    ///////////////////////////////  重置配方程序 
                    foreach (var item2 in ProgramManager.Instance.ProgramList)
                    {
                        item2.IsActive = false;
                    }
                    ProgramManager.Instance.Save();
                    break;

                case "保存toolStripMenuItem":
                    if (this.programPath == null || this.programPath.Length == 0)
                    {
                        if (ProgramConfigParamManager.Instance.ProgramParamList != null && ProgramConfigParamManager.Instance.ProgramParamList.Count > 0)
                        {
                            if (ProgramConfigParamManager.Instance.ProgramParamList[0].IsAuto)
                            {
                                if (ProgramForm.Instance.SaveProgram(ProgramConfigParamManager.Instance.ProgramParamList[0].ProgramPath))
                                    MessageBox.Show("保存成功");
                                else
                                    MessageBox.Show("保存失败" + new Exception().ToString());
                            }
                            else
                            {
                                FolderBrowserDialog saveFileDialog2 = new FolderBrowserDialog();
                                saveFileDialog2.ShowDialog();
                                this.programPath = saveFileDialog2.SelectedPath;
                                if (saveFileDialog2.SelectedPath.Contains("任务"))
                                    this.programPath = new FileInfo(saveFileDialog2.SelectedPath).DirectoryName;// ;
                                if (this.programPath == null || this.programPath.Length == 0) return;
                                //////////////////////////////////
                                if (ProgramForm.Instance.SaveProgram(this.programPath))
                                    MessageBox.Show("保存成功");
                                else
                                    MessageBox.Show("保存失败" + new Exception().ToString());
                            }
                        }
                    }
                    else
                    {
                        if (ProgramForm.Instance.SaveProgram(this.programPath))
                            MessageBox.Show("保存成功");
                        else
                            MessageBox.Show("保存失败" + new Exception().ToString());
                    }
                    this.toolStripStatusLabel1.Text = this.programPath;
                    break;
                case "另存为toolStripMenuItem":
                    FolderBrowserDialog saveFileDialog = new FolderBrowserDialog();
                    saveFileDialog.ShowDialog();
                    this.programPath = saveFileDialog.SelectedPath;
                    if (saveFileDialog.SelectedPath.Contains("任务"))
                        this.programPath = new FileInfo(saveFileDialog.SelectedPath).DirectoryName;// ;
                    if (this.programPath == null || this.programPath.Length == 0) return;
                    //////////////////////////////////
                    if (ProgramForm.Instance.SaveProgram(this.programPath))
                        MessageBox.Show("保存成功");
                    else
                        MessageBox.Show("保存失败" + new Exception().ToString());
                    ///////////////////////////////////////////////////////////////////////////
                    //if (ProgramConfigParamManager.Instance.ProgramParamList != null && ProgramConfigParamManager.Instance.ProgramParamList.Count > 0)
                    //{
                    //    if (ProgramConfigParamManager.Instance.ProgramParamList[0].IsAuto)
                    //    {
                    //        if (ProgramForm.Instance.SaveProgram(ProgramConfigParamManager.Instance.ProgramParamList[0].ProgramPath))
                    //            MessageBox.Show("保存成功");
                    //        else
                    //            MessageBox.Show("保存失败" + new Exception().ToString());
                    //    }
                    //    else
                    //    {
                    //        FolderBrowserDialog saveFileDialog = new FolderBrowserDialog();
                    //        saveFileDialog.ShowDialog();
                    //        this.programPath = saveFileDialog.SelectedPath;
                    //        if (saveFileDialog.SelectedPath.Contains("任务"))
                    //            this.programPath = new FileInfo(saveFileDialog.SelectedPath).DirectoryName;// ;
                    //        if (this.programPath == null || this.programPath.Length == 0) return;
                    //        //////////////////////////////////
                    //        if (ProgramForm.Instance.SaveProgram(this.programPath))
                    //            MessageBox.Show("保存成功");
                    //        else
                    //            MessageBox.Show("保存失败" + new Exception().ToString());
                    //    }
                    //}
                    this.toolStripStatusLabel1.Text = this.programPath;
                    break;
                default:
                    break;
            }
        }


        private void 用户toolStripMenuItem_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            try
            {
                LoginForm loginForm = new LoginForm();
                loginForm.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
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
                this.日期toolStripStatusLabel.Text = DateTime.Now.ToString("yyyy/MM/dd");
                this.时间toolStripStatusLabel.Text = DateTime.Now.ToString("HH:mm:ss");
                //////////////////////////////////////////////////////////////////////
                this.用户toolStripStatusLabel.Text = UserLoginParamManager.Instance.CurrentUser.ToString();
                if (ProgramConfigParamManager.Instance.ProgramParamList != null)  // 自动切换配方
                {
                    foreach (var item in ProgramConfigParamManager.Instance.ProgramParamList)
                    {
                        if (item.IsAuto && this.toolStripStatusLabel1.Text != item.ProgramPath) // 只有在不等于时才切换
                        {
                            this.toolStripStatusLabel1.Text = item.ProgramPath;
                            this.Invoke(new Action(() =>
                            {
                                ToolStripItem toolStripMenu = new ToolStripMenuItem();
                                toolStripMenu.Name = "断开连机toolStripButton";
                                this.运行工具条toolStrip_ItemClicked(null, new ToolStripItemClickedEventArgs(toolStripMenu));
                                ProgramForm.Instance.OpenProgram(item.ProgramPath);
                                this.programPath = item.ProgramPath;
                            }));
                            ///////////////////////////////
                            this.Invoke(new Action(() =>
                            {
                                ToolStripItem toolStripMenu2 = new ToolStripMenuItem();
                                toolStripMenu2.Name = "联机运行toolStripButton";
                                this.运行工具条toolStrip_ItemClicked(null, new ToolStripItemClickedEventArgs(toolStripMenu2));
                            }));
                        }
                    }
                }
            }
            catch
            {

            }
        }

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



        private void 连接服务器Btn_Click(object sender, EventArgs e)
        {
            try
            {
                if (DeviceConnectConfigParamManger.Instance.DeviceConfigParamList != null)
                {
                    IMotionControl _card = null;
                    bool result = false;
                    foreach (var item in DeviceConnectConfigParamManger.Instance.DeviceConfigParamList)
                    {
                        _card = MotionCardManage.GetCard(item.DeviceName);
                        if (_card == null) continue;
                        result = _card.WriteValue(enDataTypes.String, "", "isConnect");
                        if (!result)
                        {
                            MotionCardManage.GetCard(item.DeviceName)?.Init(item);
                        }
                        else
                        {
                            MessageBox.Show("已连接服务器：");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }


        }

        private void 屏蔽checkBox_CheckedChanged_1(object sender, EventArgs e)
        {
            try
            {
                if (this.屏蔽checkBox.Checked)
                {
                    this.屏蔽checkBox.Text = "启用检测";
                    SystemParamManager.Instance.SysConfigParam.ShieldDetect = true;
                }
                else
                {
                    this.屏蔽checkBox.Text = "屏蔽检测";
                    SystemParamManager.Instance.SysConfigParam.ShieldDetect = false;
                }
                SystemParamManager.Instance.Save();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }


    }
}

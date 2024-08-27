using System;

namespace CxVision
{
    partial class MainForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            this.Invoke(new Action(() => base.Dispose(disposing)));
            //base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.文件toolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.新建toolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.打开toolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.保存toolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.另存为toolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.编辑toolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.视图toolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.工具toolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.检测工具toolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.标定工具toolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.相机标定toolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.FA相机标定toolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.矩阵标定相机toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.N点标定相机toolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.标定相机角度toolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.相机激光标定toolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.相机点激光toolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.相机线激光标定toolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.相机面激光标定toolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.相机胶枪标定ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.机器人夹抓标定ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.运动toolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.激光标定toolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.标定线激光toolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.标定点激光toolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.标定面激光toolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.机台校准toolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.面阵补偿toolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.线性补偿toolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.相机9点标定toolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.面激光9点标定toolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.线激光9点标定toolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.点激光9点标定toolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.配置toolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.参数配置toolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.传感器配置toolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.光源配置toolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.运动控制卡配置toolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.采集源配置ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.程序配置ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.调试toolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.查看点激光toolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.查看线激光toolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.参数toolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.保存布局toolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.重置屏幕参数toolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.用户toolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.登录toolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.日期toolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel4 = new System.Windows.Forms.ToolStripStatusLabel();
            this.时间toolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel3 = new System.Windows.Forms.ToolStripStatusLabel();
            this.用户toolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.运行工具条toolStrip = new System.Windows.Forms.ToolStrip();
            this.运行toolStripButton = new System.Windows.Forms.ToolStripButton();
            this.停止toolStripButton = new System.Windows.Forms.ToolStripButton();
            this.循环次数toolStripButton = new System.Windows.Forms.ToolStripButton();
            this.联机运行toolStripButton = new System.Windows.Forms.ToolStripButton();
            this.断开连机toolStripButton = new System.Windows.Forms.ToolStripButton();
            this.视图toolStripButton = new System.Windows.Forms.ToolStripButton();
            this.编辑工具条toolStrip = new System.Windows.Forms.ToolStrip();
            this.新建NToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.打开ToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.保存ToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.打印PToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.剪切UToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.复制CToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.粘贴PToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.帮助LToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.屏蔽checkBox = new System.Windows.Forms.CheckBox();
            this.连接服务器Btn = new System.Windows.Forms.Button();
            this.显示数据checkBox = new System.Windows.Forms.CheckBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.程序tabPage = new System.Windows.Forms.TabPage();
            this.工程配置tabPage = new System.Windows.Forms.TabPage();
            this.工程配置tabControl = new System.Windows.Forms.TabControl();
            this.坐标系TabPage = new System.Windows.Forms.TabPage();
            this.标定参数tabPage = new System.Windows.Forms.TabPage();
            this.夹抓tabPage = new System.Windows.Forms.TabPage();
            this.通信配置tabPage = new System.Windows.Forms.TabPage();
            this.通讯tabPage = new System.Windows.Forms.TabPage();
            this.tabControl3 = new System.Windows.Forms.TabControl();
            this.SensorTabPage = new System.Windows.Forms.TabPage();
            this.DeviceTabPage = new System.Windows.Forms.TabPage();
            this.设备splitContainer = new System.Windows.Forms.SplitContainer();
            this.SocketTabPage = new System.Windows.Forms.TabPage();
            this.系统配置tabPage = new System.Windows.Forms.TabPage();
            this.menuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.运行工具条toolStrip.SuspendLayout();
            this.编辑工具条toolStrip.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.工程配置tabPage.SuspendLayout();
            this.工程配置tabControl.SuspendLayout();
            this.通讯tabPage.SuspendLayout();
            this.tabControl3.SuspendLayout();
            this.DeviceTabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.设备splitContainer)).BeginInit();
            this.设备splitContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.SystemColors.Control;
            this.menuStrip1.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.文件toolStripMenuItem,
            this.编辑toolStripMenuItem,
            this.视图toolStripMenuItem,
            this.工具toolStripMenuItem,
            this.配置toolStripMenuItem,
            this.调试toolStripMenuItem,
            this.参数toolStripMenuItem,
            this.用户toolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(4, 2, 0, 2);
            this.menuStrip1.Size = new System.Drawing.Size(1121, 29);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // 文件toolStripMenuItem
            // 
            this.文件toolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.新建toolStripMenuItem,
            this.打开toolStripMenuItem,
            this.保存toolStripMenuItem,
            this.另存为toolStripMenuItem});
            this.文件toolStripMenuItem.Name = "文件toolStripMenuItem";
            this.文件toolStripMenuItem.Size = new System.Drawing.Size(54, 25);
            this.文件toolStripMenuItem.Text = "文件";
            this.文件toolStripMenuItem.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.文件toolStripMenuItem_DropDownItemClicked);
            // 
            // 新建toolStripMenuItem
            // 
            this.新建toolStripMenuItem.Name = "新建toolStripMenuItem";
            this.新建toolStripMenuItem.Size = new System.Drawing.Size(128, 26);
            this.新建toolStripMenuItem.Text = "新建";
            // 
            // 打开toolStripMenuItem
            // 
            this.打开toolStripMenuItem.Name = "打开toolStripMenuItem";
            this.打开toolStripMenuItem.Size = new System.Drawing.Size(128, 26);
            this.打开toolStripMenuItem.Text = "打开";
            // 
            // 保存toolStripMenuItem
            // 
            this.保存toolStripMenuItem.Name = "保存toolStripMenuItem";
            this.保存toolStripMenuItem.Size = new System.Drawing.Size(128, 26);
            this.保存toolStripMenuItem.Text = "保存";
            // 
            // 另存为toolStripMenuItem
            // 
            this.另存为toolStripMenuItem.Name = "另存为toolStripMenuItem";
            this.另存为toolStripMenuItem.Size = new System.Drawing.Size(128, 26);
            this.另存为toolStripMenuItem.Text = "另存为";
            // 
            // 编辑toolStripMenuItem
            // 
            this.编辑toolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem});
            this.编辑toolStripMenuItem.Name = "编辑toolStripMenuItem";
            this.编辑toolStripMenuItem.Size = new System.Drawing.Size(54, 25);
            this.编辑toolStripMenuItem.Text = "编辑";
            // 
            // toolStripMenuItem
            // 
            this.toolStripMenuItem.Name = "toolStripMenuItem";
            this.toolStripMenuItem.Size = new System.Drawing.Size(234, 26);
            this.toolStripMenuItem.Text = "toolStripMenuItem3";
            // 
            // 视图toolStripMenuItem
            // 
            this.视图toolStripMenuItem.Name = "视图toolStripMenuItem";
            this.视图toolStripMenuItem.Size = new System.Drawing.Size(54, 25);
            this.视图toolStripMenuItem.Text = "视图";
            // 
            // 工具toolStripMenuItem
            // 
            this.工具toolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.检测工具toolStripMenuItem,
            this.标定工具toolStripMenuItem,
            this.相机标定toolStripMenuItem,
            this.相机激光标定toolStripMenuItem,
            this.运动toolStripMenuItem,
            this.激光标定toolStripMenuItem,
            this.机台校准toolStripMenuItem,
            this.toolStripMenuItem2});
            this.工具toolStripMenuItem.Name = "工具toolStripMenuItem";
            this.工具toolStripMenuItem.Size = new System.Drawing.Size(54, 25);
            this.工具toolStripMenuItem.Text = "工具";
            this.工具toolStripMenuItem.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.工具toolStripMenuItem_DropDownItemClicked);
            // 
            // 检测工具toolStripMenuItem
            // 
            this.检测工具toolStripMenuItem.Name = "检测工具toolStripMenuItem";
            this.检测工具toolStripMenuItem.Size = new System.Drawing.Size(176, 26);
            this.检测工具toolStripMenuItem.Text = "检测工具";
            // 
            // 标定工具toolStripMenuItem
            // 
            this.标定工具toolStripMenuItem.Name = "标定工具toolStripMenuItem";
            this.标定工具toolStripMenuItem.Size = new System.Drawing.Size(176, 26);
            this.标定工具toolStripMenuItem.Text = "对射标定工具";
            // 
            // 相机标定toolStripMenuItem
            // 
            this.相机标定toolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.FA相机标定toolStripMenuItem,
            this.矩阵标定相机toolStripMenuItem2,
            this.N点标定相机toolStripMenuItem3,
            this.标定相机角度toolStripMenuItem});
            this.相机标定toolStripMenuItem.Name = "相机标定toolStripMenuItem";
            this.相机标定toolStripMenuItem.Size = new System.Drawing.Size(176, 26);
            this.相机标定toolStripMenuItem.Text = "相机标定";
            this.相机标定toolStripMenuItem.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.相机标定toolStripMenuItem_DropDownItemClicked);
            // 
            // FA相机标定toolStripMenuItem
            // 
            this.FA相机标定toolStripMenuItem.Enabled = false;
            this.FA相机标定toolStripMenuItem.Name = "FA相机标定toolStripMenuItem";
            this.FA相机标定toolStripMenuItem.Size = new System.Drawing.Size(176, 26);
            this.FA相机标定toolStripMenuItem.Text = "FA相机标定";
            // 
            // 矩阵标定相机toolStripMenuItem2
            // 
            this.矩阵标定相机toolStripMenuItem2.Name = "矩阵标定相机toolStripMenuItem2";
            this.矩阵标定相机toolStripMenuItem2.Size = new System.Drawing.Size(176, 26);
            this.矩阵标定相机toolStripMenuItem2.Text = "矩阵标定相机";
            // 
            // N点标定相机toolStripMenuItem3
            // 
            this.N点标定相机toolStripMenuItem3.Enabled = false;
            this.N点标定相机toolStripMenuItem3.Name = "N点标定相机toolStripMenuItem3";
            this.N点标定相机toolStripMenuItem3.Size = new System.Drawing.Size(176, 26);
            this.N点标定相机toolStripMenuItem3.Text = "N点标定相机";
            // 
            // 标定相机角度toolStripMenuItem
            // 
            this.标定相机角度toolStripMenuItem.Enabled = false;
            this.标定相机角度toolStripMenuItem.Name = "标定相机角度toolStripMenuItem";
            this.标定相机角度toolStripMenuItem.Size = new System.Drawing.Size(176, 26);
            this.标定相机角度toolStripMenuItem.Text = "标定相机角度";
            // 
            // 相机激光标定toolStripMenuItem
            // 
            this.相机激光标定toolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.相机点激光toolStripMenuItem,
            this.相机线激光标定toolStripMenuItem,
            this.相机面激光标定toolStripMenuItem,
            this.相机胶枪标定ToolStripMenuItem,
            this.机器人夹抓标定ToolStripMenuItem});
            this.相机激光标定toolStripMenuItem.Name = "相机激光标定toolStripMenuItem";
            this.相机激光标定toolStripMenuItem.Size = new System.Drawing.Size(176, 26);
            this.相机激光标定toolStripMenuItem.Text = "相机激光标定";
            this.相机激光标定toolStripMenuItem.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.相机激光标定toolStripMenuItem1_DropDownItemClicked);
            // 
            // 相机点激光toolStripMenuItem
            // 
            this.相机点激光toolStripMenuItem.Name = "相机点激光toolStripMenuItem";
            this.相机点激光toolStripMenuItem.Size = new System.Drawing.Size(192, 26);
            this.相机点激光toolStripMenuItem.Text = "相机&点激光标定";
            // 
            // 相机线激光标定toolStripMenuItem
            // 
            this.相机线激光标定toolStripMenuItem.Name = "相机线激光标定toolStripMenuItem";
            this.相机线激光标定toolStripMenuItem.Size = new System.Drawing.Size(192, 26);
            this.相机线激光标定toolStripMenuItem.Text = "相机&线激光标定";
            // 
            // 相机面激光标定toolStripMenuItem
            // 
            this.相机面激光标定toolStripMenuItem.Name = "相机面激光标定toolStripMenuItem";
            this.相机面激光标定toolStripMenuItem.Size = new System.Drawing.Size(192, 26);
            this.相机面激光标定toolStripMenuItem.Text = "相机&面激光标定";
            // 
            // 相机胶枪标定ToolStripMenuItem
            // 
            this.相机胶枪标定ToolStripMenuItem.Name = "相机胶枪标定ToolStripMenuItem";
            this.相机胶枪标定ToolStripMenuItem.Size = new System.Drawing.Size(192, 26);
            this.相机胶枪标定ToolStripMenuItem.Text = "相机&胶枪标定";
            // 
            // 机器人夹抓标定ToolStripMenuItem
            // 
            this.机器人夹抓标定ToolStripMenuItem.Name = "机器人夹抓标定ToolStripMenuItem";
            this.机器人夹抓标定ToolStripMenuItem.Size = new System.Drawing.Size(192, 26);
            this.机器人夹抓标定ToolStripMenuItem.Text = "机器人&夹抓标定";
            // 
            // 运动toolStripMenuItem
            // 
            this.运动toolStripMenuItem.Name = "运动toolStripMenuItem";
            this.运动toolStripMenuItem.Size = new System.Drawing.Size(176, 26);
            this.运动toolStripMenuItem.Text = "运动控制";
            // 
            // 激光标定toolStripMenuItem
            // 
            this.激光标定toolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.标定线激光toolStripMenuItem,
            this.标定点激光toolStripMenuItem,
            this.标定面激光toolStripMenuItem});
            this.激光标定toolStripMenuItem.Name = "激光标定toolStripMenuItem";
            this.激光标定toolStripMenuItem.Size = new System.Drawing.Size(176, 26);
            this.激光标定toolStripMenuItem.Text = "激光标定";
            this.激光标定toolStripMenuItem.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.激光标定toolStripMenuItem_DropDownItemClicked);
            // 
            // 标定线激光toolStripMenuItem
            // 
            this.标定线激光toolStripMenuItem.Name = "标定线激光toolStripMenuItem";
            this.标定线激光toolStripMenuItem.Size = new System.Drawing.Size(160, 26);
            this.标定线激光toolStripMenuItem.Text = "标定线激光";
            // 
            // 标定点激光toolStripMenuItem
            // 
            this.标定点激光toolStripMenuItem.Name = "标定点激光toolStripMenuItem";
            this.标定点激光toolStripMenuItem.Size = new System.Drawing.Size(160, 26);
            this.标定点激光toolStripMenuItem.Text = "标定点激光";
            // 
            // 标定面激光toolStripMenuItem
            // 
            this.标定面激光toolStripMenuItem.Name = "标定面激光toolStripMenuItem";
            this.标定面激光toolStripMenuItem.Size = new System.Drawing.Size(160, 26);
            this.标定面激光toolStripMenuItem.Text = "标定面激光";
            // 
            // 机台校准toolStripMenuItem
            // 
            this.机台校准toolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.面阵补偿toolStripMenuItem,
            this.线性补偿toolStripMenuItem});
            this.机台校准toolStripMenuItem.Name = "机台校准toolStripMenuItem";
            this.机台校准toolStripMenuItem.Size = new System.Drawing.Size(176, 26);
            this.机台校准toolStripMenuItem.Text = "机台校准";
            this.机台校准toolStripMenuItem.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.机台校准toolStripMenuItem_DropDownItemClicked);
            // 
            // 面阵补偿toolStripMenuItem
            // 
            this.面阵补偿toolStripMenuItem.Name = "面阵补偿toolStripMenuItem";
            this.面阵补偿toolStripMenuItem.Size = new System.Drawing.Size(144, 26);
            this.面阵补偿toolStripMenuItem.Text = "面阵补偿";
            // 
            // 线性补偿toolStripMenuItem
            // 
            this.线性补偿toolStripMenuItem.Name = "线性补偿toolStripMenuItem";
            this.线性补偿toolStripMenuItem.Size = new System.Drawing.Size(144, 26);
            this.线性补偿toolStripMenuItem.Text = "线性补偿";
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.相机9点标定toolStripMenuItem,
            this.面激光9点标定toolStripMenuItem,
            this.线激光9点标定toolStripMenuItem,
            this.点激光9点标定toolStripMenuItem});
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(176, 26);
            this.toolStripMenuItem2.Text = "9点标定";
            this.toolStripMenuItem2.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.toolStripMenuItem2_DropDownItemClicked);
            // 
            // 相机9点标定toolStripMenuItem
            // 
            this.相机9点标定toolStripMenuItem.Name = "相机9点标定toolStripMenuItem";
            this.相机9点标定toolStripMenuItem.Size = new System.Drawing.Size(185, 26);
            this.相机9点标定toolStripMenuItem.Text = "相机9点标定";
            // 
            // 面激光9点标定toolStripMenuItem
            // 
            this.面激光9点标定toolStripMenuItem.Name = "面激光9点标定toolStripMenuItem";
            this.面激光9点标定toolStripMenuItem.Size = new System.Drawing.Size(185, 26);
            this.面激光9点标定toolStripMenuItem.Text = "面激光9点标定";
            // 
            // 线激光9点标定toolStripMenuItem
            // 
            this.线激光9点标定toolStripMenuItem.Name = "线激光9点标定toolStripMenuItem";
            this.线激光9点标定toolStripMenuItem.Size = new System.Drawing.Size(185, 26);
            this.线激光9点标定toolStripMenuItem.Text = "线激光9点标定";
            // 
            // 点激光9点标定toolStripMenuItem
            // 
            this.点激光9点标定toolStripMenuItem.Name = "点激光9点标定toolStripMenuItem";
            this.点激光9点标定toolStripMenuItem.Size = new System.Drawing.Size(185, 26);
            this.点激光9点标定toolStripMenuItem.Text = "点激光9点标定";
            // 
            // 配置toolStripMenuItem
            // 
            this.配置toolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.参数配置toolStripMenuItem,
            this.传感器配置toolStripMenuItem,
            this.光源配置toolStripMenuItem,
            this.运动控制卡配置toolStripMenuItem,
            this.采集源配置ToolStripMenuItem,
            this.程序配置ToolStripMenuItem});
            this.配置toolStripMenuItem.Name = "配置toolStripMenuItem";
            this.配置toolStripMenuItem.Size = new System.Drawing.Size(54, 25);
            this.配置toolStripMenuItem.Text = "配置";
            this.配置toolStripMenuItem.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.配置toolStripMenuItem_DropDownItemClicked);
            // 
            // 参数配置toolStripMenuItem
            // 
            this.参数配置toolStripMenuItem.Name = "参数配置toolStripMenuItem";
            this.参数配置toolStripMenuItem.Size = new System.Drawing.Size(192, 26);
            this.参数配置toolStripMenuItem.Text = "参数设置 ";
            // 
            // 传感器配置toolStripMenuItem
            // 
            this.传感器配置toolStripMenuItem.Name = "传感器配置toolStripMenuItem";
            this.传感器配置toolStripMenuItem.Size = new System.Drawing.Size(192, 26);
            this.传感器配置toolStripMenuItem.Text = "传感器配置";
            // 
            // 光源配置toolStripMenuItem
            // 
            this.光源配置toolStripMenuItem.Name = "光源配置toolStripMenuItem";
            this.光源配置toolStripMenuItem.Size = new System.Drawing.Size(192, 26);
            this.光源配置toolStripMenuItem.Text = "光源配置";
            // 
            // 运动控制卡配置toolStripMenuItem
            // 
            this.运动控制卡配置toolStripMenuItem.Name = "运动控制卡配置toolStripMenuItem";
            this.运动控制卡配置toolStripMenuItem.Size = new System.Drawing.Size(192, 26);
            this.运动控制卡配置toolStripMenuItem.Text = "运动控制卡配置";
            // 
            // 采集源配置ToolStripMenuItem
            // 
            this.采集源配置ToolStripMenuItem.Name = "采集源配置ToolStripMenuItem";
            this.采集源配置ToolStripMenuItem.Size = new System.Drawing.Size(192, 26);
            this.采集源配置ToolStripMenuItem.Text = "采集源配置";
            // 
            // 程序配置ToolStripMenuItem
            // 
            this.程序配置ToolStripMenuItem.Name = "程序配置ToolStripMenuItem";
            this.程序配置ToolStripMenuItem.Size = new System.Drawing.Size(192, 26);
            this.程序配置ToolStripMenuItem.Text = "程序配置";
            // 
            // 调试toolStripMenuItem
            // 
            this.调试toolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.查看点激光toolStripMenuItem,
            this.查看线激光toolStripMenuItem,
            this.toolStripSeparator2});
            this.调试toolStripMenuItem.Name = "调试toolStripMenuItem";
            this.调试toolStripMenuItem.Size = new System.Drawing.Size(54, 25);
            this.调试toolStripMenuItem.Text = "调试";
            this.调试toolStripMenuItem.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.调试toolStripMenuItem_DropDownItemClicked);
            // 
            // 查看点激光toolStripMenuItem
            // 
            this.查看点激光toolStripMenuItem.Name = "查看点激光toolStripMenuItem";
            this.查看点激光toolStripMenuItem.Size = new System.Drawing.Size(160, 26);
            this.查看点激光toolStripMenuItem.Text = "查看点激光";
            // 
            // 查看线激光toolStripMenuItem
            // 
            this.查看线激光toolStripMenuItem.Name = "查看线激光toolStripMenuItem";
            this.查看线激光toolStripMenuItem.Size = new System.Drawing.Size(160, 26);
            this.查看线激光toolStripMenuItem.Text = "查看线激光";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(157, 6);
            // 
            // 参数toolStripMenuItem
            // 
            this.参数toolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.保存布局toolStripMenuItem,
            this.重置屏幕参数toolStripMenuItem});
            this.参数toolStripMenuItem.Name = "参数toolStripMenuItem";
            this.参数toolStripMenuItem.Size = new System.Drawing.Size(54, 25);
            this.参数toolStripMenuItem.Text = "参数";
            this.参数toolStripMenuItem.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.参数toolStripMenuItem_DropDownItemClicked);
            // 
            // 保存布局toolStripMenuItem
            // 
            this.保存布局toolStripMenuItem.Name = "保存布局toolStripMenuItem";
            this.保存布局toolStripMenuItem.Size = new System.Drawing.Size(176, 26);
            this.保存布局toolStripMenuItem.Text = "保存视图布局";
            // 
            // 重置屏幕参数toolStripMenuItem
            // 
            this.重置屏幕参数toolStripMenuItem.Name = "重置屏幕参数toolStripMenuItem";
            this.重置屏幕参数toolStripMenuItem.Size = new System.Drawing.Size(176, 26);
            this.重置屏幕参数toolStripMenuItem.Text = "重置屏幕参数";
            // 
            // 用户toolStripMenuItem
            // 
            this.用户toolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.登录toolStripMenuItem});
            this.用户toolStripMenuItem.Name = "用户toolStripMenuItem";
            this.用户toolStripMenuItem.Size = new System.Drawing.Size(54, 25);
            this.用户toolStripMenuItem.Text = "用户";
            this.用户toolStripMenuItem.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.用户toolStripMenuItem_DropDownItemClicked);
            // 
            // 登录toolStripMenuItem
            // 
            this.登录toolStripMenuItem.Name = "登录toolStripMenuItem";
            this.登录toolStripMenuItem.Size = new System.Drawing.Size(112, 26);
            this.登录toolStripMenuItem.Text = "登录";
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.toolStripStatusLabel2,
            this.日期toolStripStatusLabel,
            this.toolStripStatusLabel4,
            this.时间toolStripStatusLabel,
            this.toolStripStatusLabel3,
            this.用户toolStripStatusLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 797);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1121, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(89, 17);
            this.toolStripStatusLabel1.Text = "程序路径+名称";
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(44, 17);
            this.toolStripStatusLabel2.Text = "日期：";
            // 
            // 日期toolStripStatusLabel
            // 
            this.日期toolStripStatusLabel.Name = "日期toolStripStatusLabel";
            this.日期toolStripStatusLabel.Size = new System.Drawing.Size(131, 17);
            this.日期toolStripStatusLabel.Text = "toolStripStatusLabel3";
            // 
            // toolStripStatusLabel4
            // 
            this.toolStripStatusLabel4.Name = "toolStripStatusLabel4";
            this.toolStripStatusLabel4.Size = new System.Drawing.Size(44, 17);
            this.toolStripStatusLabel4.Text = "时间：";
            // 
            // 时间toolStripStatusLabel
            // 
            this.时间toolStripStatusLabel.Name = "时间toolStripStatusLabel";
            this.时间toolStripStatusLabel.Size = new System.Drawing.Size(131, 17);
            this.时间toolStripStatusLabel.Text = "toolStripStatusLabel5";
            // 
            // toolStripStatusLabel3
            // 
            this.toolStripStatusLabel3.Name = "toolStripStatusLabel3";
            this.toolStripStatusLabel3.Size = new System.Drawing.Size(68, 17);
            this.toolStripStatusLabel3.Text = "当前用户：";
            // 
            // 用户toolStripStatusLabel
            // 
            this.用户toolStripStatusLabel.Name = "用户toolStripStatusLabel";
            this.用户toolStripStatusLabel.Size = new System.Drawing.Size(131, 17);
            this.用户toolStripStatusLabel.Text = "toolStripStatusLabel5";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.BackColor = System.Drawing.SystemColors.Control;
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 221F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 546F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 2.259887F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 97.74011F));
            this.tableLayoutPanel1.Controls.Add(this.运行工具条toolStrip, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.编辑工具条toolStrip, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.tabControl1, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 29);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 54F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 170F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1121, 768);
            this.tableLayoutPanel1.TabIndex = 2;
            // 
            // 运行工具条toolStrip
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.运行工具条toolStrip, 2);
            this.运行工具条toolStrip.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.运行工具条toolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.运行工具条toolStrip.ImageScalingSize = new System.Drawing.Size(25, 25);
            this.运行工具条toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.运行toolStripButton,
            this.停止toolStripButton,
            this.循环次数toolStripButton,
            this.联机运行toolStripButton,
            this.断开连机toolStripButton,
            this.视图toolStripButton});
            this.运行工具条toolStrip.Location = new System.Drawing.Point(221, 0);
            this.运行工具条toolStrip.Name = "运行工具条toolStrip";
            this.运行工具条toolStrip.Size = new System.Drawing.Size(553, 53);
            this.运行工具条toolStrip.TabIndex = 9;
            this.运行工具条toolStrip.Text = "toolStrip2";
            this.运行工具条toolStrip.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.运行工具条toolStrip_ItemClicked);
            // 
            // 运行toolStripButton
            // 
            this.运行toolStripButton.Image = global::CxVision.Properties.Resources.Start;
            this.运行toolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.运行toolStripButton.Name = "运行toolStripButton";
            this.运行toolStripButton.Size = new System.Drawing.Size(92, 50);
            this.运行toolStripButton.Text = "运行(Start)";
            this.运行toolStripButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            // 
            // 停止toolStripButton
            // 
            this.停止toolStripButton.Image = global::CxVision.Properties.Resources.stopRum;
            this.停止toolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.停止toolStripButton.Name = "停止toolStripButton";
            this.停止toolStripButton.Size = new System.Drawing.Size(91, 50);
            this.停止toolStripButton.Text = "停止(Stop)";
            this.停止toolStripButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            // 
            // 循环次数toolStripButton
            // 
            this.循环次数toolStripButton.Image = global::CxVision.Properties.Resources.Loop;
            this.循环次数toolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.循环次数toolStripButton.Name = "循环次数toolStripButton";
            this.循环次数toolStripButton.Size = new System.Drawing.Size(78, 50);
            this.循环次数toolStripButton.Text = "循环执行";
            this.循环次数toolStripButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            // 
            // 联机运行toolStripButton
            // 
            this.联机运行toolStripButton.Image = global::CxVision.Properties.Resources.Start;
            this.联机运行toolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.联机运行toolStripButton.Name = "联机运行toolStripButton";
            this.联机运行toolStripButton.Size = new System.Drawing.Size(78, 50);
            this.联机运行toolStripButton.Text = "联机运行";
            this.联机运行toolStripButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            // 
            // 断开连机toolStripButton
            // 
            this.断开连机toolStripButton.Image = global::CxVision.Properties.Resources.Stop;
            this.断开连机toolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.断开连机toolStripButton.Name = "断开连机toolStripButton";
            this.断开连机toolStripButton.Size = new System.Drawing.Size(78, 50);
            this.断开连机toolStripButton.Text = "断开连机";
            this.断开连机toolStripButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            // 
            // 视图toolStripButton
            // 
            this.视图toolStripButton.Image = global::CxVision.Properties.Resources._1606739385_1_;
            this.视图toolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.视图toolStripButton.Name = "视图toolStripButton";
            this.视图toolStripButton.Size = new System.Drawing.Size(78, 50);
            this.视图toolStripButton.Text = "添加视图";
            this.视图toolStripButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            // 
            // 编辑工具条toolStrip
            // 
            this.编辑工具条toolStrip.Dock = System.Windows.Forms.DockStyle.Fill;
            this.编辑工具条toolStrip.Font = new System.Drawing.Font("Microsoft YaHei UI", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.编辑工具条toolStrip.ImageScalingSize = new System.Drawing.Size(25, 25);
            this.编辑工具条toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.新建NToolStripButton,
            this.打开ToolStripButton,
            this.保存ToolStripButton,
            this.打印PToolStripButton,
            this.toolStripSeparator,
            this.剪切UToolStripButton,
            this.复制CToolStripButton,
            this.粘贴PToolStripButton,
            this.toolStripSeparator1,
            this.帮助LToolStripButton});
            this.编辑工具条toolStrip.Location = new System.Drawing.Point(0, 0);
            this.编辑工具条toolStrip.Name = "编辑工具条toolStrip";
            this.编辑工具条toolStrip.Size = new System.Drawing.Size(221, 54);
            this.编辑工具条toolStrip.TabIndex = 2;
            this.编辑工具条toolStrip.Text = "toolStrip1";
            this.编辑工具条toolStrip.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.编辑工具条toolStrip_ItemClicked);
            // 
            // 新建NToolStripButton
            // 
            this.新建NToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("新建NToolStripButton.Image")));
            this.新建NToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.新建NToolStripButton.Name = "新建NToolStripButton";
            this.新建NToolStripButton.Size = new System.Drawing.Size(62, 51);
            this.新建NToolStripButton.Text = "新建(&N)";
            this.新建NToolStripButton.TextDirection = System.Windows.Forms.ToolStripTextDirection.Horizontal;
            this.新建NToolStripButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            // 
            // 打开ToolStripButton
            // 
            this.打开ToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("打开ToolStripButton.Image")));
            this.打开ToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.打开ToolStripButton.Name = "打开ToolStripButton";
            this.打开ToolStripButton.Size = new System.Drawing.Size(62, 51);
            this.打开ToolStripButton.Text = "打开(&O)";
            this.打开ToolStripButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            // 
            // 保存ToolStripButton
            // 
            this.保存ToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("保存ToolStripButton.Image")));
            this.保存ToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.保存ToolStripButton.Name = "保存ToolStripButton";
            this.保存ToolStripButton.Size = new System.Drawing.Size(59, 51);
            this.保存ToolStripButton.Text = "保存(&S)";
            this.保存ToolStripButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            // 
            // 打印PToolStripButton
            // 
            this.打印PToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("打印PToolStripButton.Image")));
            this.打印PToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.打印PToolStripButton.Name = "打印PToolStripButton";
            this.打印PToolStripButton.Size = new System.Drawing.Size(60, 49);
            this.打印PToolStripButton.Text = "打印(&P)";
            this.打印PToolStripButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            // 
            // toolStripSeparator
            // 
            this.toolStripSeparator.Name = "toolStripSeparator";
            this.toolStripSeparator.Size = new System.Drawing.Size(6, 68);
            // 
            // 剪切UToolStripButton
            // 
            this.剪切UToolStripButton.Font = new System.Drawing.Font("宋体", 5.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.剪切UToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("剪切UToolStripButton.Image")));
            this.剪切UToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.剪切UToolStripButton.Name = "剪切UToolStripButton";
            this.剪切UToolStripButton.Size = new System.Drawing.Size(34, 36);
            this.剪切UToolStripButton.Text = "剪切(&U)";
            this.剪切UToolStripButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            // 
            // 复制CToolStripButton
            // 
            this.复制CToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("复制CToolStripButton.Image")));
            this.复制CToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.复制CToolStripButton.Name = "复制CToolStripButton";
            this.复制CToolStripButton.Size = new System.Drawing.Size(60, 49);
            this.复制CToolStripButton.Text = "复制(&C)";
            this.复制CToolStripButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            // 
            // 粘贴PToolStripButton
            // 
            this.粘贴PToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("粘贴PToolStripButton.Image")));
            this.粘贴PToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.粘贴PToolStripButton.Name = "粘贴PToolStripButton";
            this.粘贴PToolStripButton.Size = new System.Drawing.Size(60, 49);
            this.粘贴PToolStripButton.Text = "粘贴(&P)";
            this.粘贴PToolStripButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 54);
            // 
            // 帮助LToolStripButton
            // 
            this.帮助LToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("帮助LToolStripButton.Image")));
            this.帮助LToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.帮助LToolStripButton.Name = "帮助LToolStripButton";
            this.帮助LToolStripButton.Size = new System.Drawing.Size(58, 49);
            this.帮助LToolStripButton.Text = "帮助(&L)";
            this.帮助LToolStripButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.屏蔽checkBox);
            this.panel1.Controls.Add(this.连接服务器Btn);
            this.panel1.Controls.Add(this.显示数据checkBox);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(774, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(347, 54);
            this.panel1.TabIndex = 11;
            // 
            // 屏蔽checkBox
            // 
            this.屏蔽checkBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.屏蔽checkBox.Appearance = System.Windows.Forms.Appearance.Button;
            this.屏蔽checkBox.Enabled = false;
            this.屏蔽checkBox.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.屏蔽checkBox.Location = new System.Drawing.Point(91, 0);
            this.屏蔽checkBox.Name = "屏蔽checkBox";
            this.屏蔽checkBox.Size = new System.Drawing.Size(95, 54);
            this.屏蔽checkBox.TabIndex = 2;
            this.屏蔽checkBox.Text = "屏蔽检测";
            this.屏蔽checkBox.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.屏蔽checkBox.UseVisualStyleBackColor = true;
            this.屏蔽checkBox.CheckedChanged += new System.EventHandler(this.屏蔽checkBox_CheckedChanged_1);
            // 
            // 连接服务器Btn
            // 
            this.连接服务器Btn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.连接服务器Btn.Enabled = false;
            this.连接服务器Btn.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.连接服务器Btn.Location = new System.Drawing.Point(192, 0);
            this.连接服务器Btn.Name = "连接服务器Btn";
            this.连接服务器Btn.Size = new System.Drawing.Size(91, 54);
            this.连接服务器Btn.TabIndex = 1;
            this.连接服务器Btn.Text = "连接服务器";
            this.连接服务器Btn.UseVisualStyleBackColor = true;
            this.连接服务器Btn.Click += new System.EventHandler(this.连接服务器Btn_Click);
            // 
            // 显示数据checkBox
            // 
            this.显示数据checkBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.显示数据checkBox.Appearance = System.Windows.Forms.Appearance.Button;
            this.显示数据checkBox.BackgroundImage = global::CxVision.Properties.Resources.Excel;
            this.显示数据checkBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.显示数据checkBox.Location = new System.Drawing.Point(289, 0);
            this.显示数据checkBox.Name = "显示数据checkBox";
            this.显示数据checkBox.Size = new System.Drawing.Size(55, 54);
            this.显示数据checkBox.TabIndex = 0;
            this.显示数据checkBox.UseVisualStyleBackColor = true;
            this.显示数据checkBox.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // tabControl1
            // 
            this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
            this.tableLayoutPanel1.SetColumnSpan(this.tabControl1, 4);
            this.tabControl1.Controls.Add(this.程序tabPage);
            this.tabControl1.Controls.Add(this.工程配置tabPage);
            this.tabControl1.Controls.Add(this.通讯tabPage);
            this.tabControl1.Controls.Add(this.系统配置tabPage);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Font = new System.Drawing.Font("宋体", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tabControl1.Location = new System.Drawing.Point(0, 54);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.Padding = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.SetRowSpan(this.tabControl1, 2);
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1121, 714);
            this.tabControl1.TabIndex = 12;
            // 
            // 程序tabPage
            // 
            this.程序tabPage.BackColor = System.Drawing.SystemColors.Control;
            this.程序tabPage.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.程序tabPage.Location = new System.Drawing.Point(4, 4);
            this.程序tabPage.Name = "程序tabPage";
            this.程序tabPage.Padding = new System.Windows.Forms.Padding(3);
            this.程序tabPage.Size = new System.Drawing.Size(1113, 679);
            this.程序tabPage.TabIndex = 3;
            this.程序tabPage.Text = "程序编辑";
            // 
            // 工程配置tabPage
            // 
            this.工程配置tabPage.BackColor = System.Drawing.SystemColors.Control;
            this.工程配置tabPage.Controls.Add(this.工程配置tabControl);
            this.工程配置tabPage.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.工程配置tabPage.Location = new System.Drawing.Point(4, 4);
            this.工程配置tabPage.Name = "工程配置tabPage";
            this.工程配置tabPage.Padding = new System.Windows.Forms.Padding(3);
            this.工程配置tabPage.Size = new System.Drawing.Size(1113, 679);
            this.工程配置tabPage.TabIndex = 4;
            this.工程配置tabPage.Text = "工程配置";
            // 
            // 工程配置tabControl
            // 
            this.工程配置tabControl.Alignment = System.Windows.Forms.TabAlignment.Bottom;
            this.工程配置tabControl.Controls.Add(this.坐标系TabPage);
            this.工程配置tabControl.Controls.Add(this.标定参数tabPage);
            this.工程配置tabControl.Controls.Add(this.夹抓tabPage);
            this.工程配置tabControl.Controls.Add(this.通信配置tabPage);
            this.工程配置tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.工程配置tabControl.Location = new System.Drawing.Point(3, 3);
            this.工程配置tabControl.Name = "工程配置tabControl";
            this.工程配置tabControl.SelectedIndex = 0;
            this.工程配置tabControl.Size = new System.Drawing.Size(1107, 673);
            this.工程配置tabControl.TabIndex = 0;
            // 
            // 坐标系TabPage
            // 
            this.坐标系TabPage.BackColor = System.Drawing.SystemColors.Control;
            this.坐标系TabPage.Location = new System.Drawing.Point(4, 4);
            this.坐标系TabPage.Margin = new System.Windows.Forms.Padding(0);
            this.坐标系TabPage.Name = "坐标系TabPage";
            this.坐标系TabPage.Size = new System.Drawing.Size(1099, 647);
            this.坐标系TabPage.TabIndex = 0;
            this.坐标系TabPage.Text = "坐标系配置";
            // 
            // 标定参数tabPage
            // 
            this.标定参数tabPage.BackColor = System.Drawing.SystemColors.Control;
            this.标定参数tabPage.Location = new System.Drawing.Point(4, 4);
            this.标定参数tabPage.Name = "标定参数tabPage";
            this.标定参数tabPage.Size = new System.Drawing.Size(1099, 647);
            this.标定参数tabPage.TabIndex = 1;
            this.标定参数tabPage.Text = "标定管理";
            // 
            // 夹抓tabPage
            // 
            this.夹抓tabPage.BackColor = System.Drawing.SystemColors.Control;
            this.夹抓tabPage.Location = new System.Drawing.Point(4, 4);
            this.夹抓tabPage.Name = "夹抓tabPage";
            this.夹抓tabPage.Size = new System.Drawing.Size(1099, 647);
            this.夹抓tabPage.TabIndex = 2;
            this.夹抓tabPage.Text = "夹抓配置";
            // 
            // 通信配置tabPage
            // 
            this.通信配置tabPage.BackColor = System.Drawing.SystemColors.Control;
            this.通信配置tabPage.Location = new System.Drawing.Point(4, 4);
            this.通信配置tabPage.Name = "通信配置tabPage";
            this.通信配置tabPage.Size = new System.Drawing.Size(1099, 647);
            this.通信配置tabPage.TabIndex = 3;
            this.通信配置tabPage.Text = "通信配置";
            // 
            // 通讯tabPage
            // 
            this.通讯tabPage.BackColor = System.Drawing.SystemColors.Control;
            this.通讯tabPage.Controls.Add(this.tabControl3);
            this.通讯tabPage.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.通讯tabPage.Location = new System.Drawing.Point(4, 4);
            this.通讯tabPage.Name = "通讯tabPage";
            this.通讯tabPage.Size = new System.Drawing.Size(1113, 679);
            this.通讯tabPage.TabIndex = 6;
            this.通讯tabPage.Text = "连接配置";
            // 
            // tabControl3
            // 
            this.tabControl3.Alignment = System.Windows.Forms.TabAlignment.Bottom;
            this.tabControl3.Controls.Add(this.SensorTabPage);
            this.tabControl3.Controls.Add(this.DeviceTabPage);
            this.tabControl3.Controls.Add(this.SocketTabPage);
            this.tabControl3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl3.Location = new System.Drawing.Point(0, 0);
            this.tabControl3.Name = "tabControl3";
            this.tabControl3.SelectedIndex = 0;
            this.tabControl3.Size = new System.Drawing.Size(1113, 679);
            this.tabControl3.TabIndex = 0;
            // 
            // SensorTabPage
            // 
            this.SensorTabPage.BackColor = System.Drawing.SystemColors.Control;
            this.SensorTabPage.Location = new System.Drawing.Point(4, 4);
            this.SensorTabPage.Name = "SensorTabPage";
            this.SensorTabPage.Size = new System.Drawing.Size(1105, 653);
            this.SensorTabPage.TabIndex = 2;
            this.SensorTabPage.Text = "传感器连接";
            // 
            // DeviceTabPage
            // 
            this.DeviceTabPage.BackColor = System.Drawing.SystemColors.Control;
            this.DeviceTabPage.Controls.Add(this.设备splitContainer);
            this.DeviceTabPage.Location = new System.Drawing.Point(4, 4);
            this.DeviceTabPage.Name = "DeviceTabPage";
            this.DeviceTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.DeviceTabPage.Size = new System.Drawing.Size(1105, 653);
            this.DeviceTabPage.TabIndex = 0;
            this.DeviceTabPage.Text = "设备连接";
            // 
            // 设备splitContainer
            // 
            this.设备splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.设备splitContainer.Location = new System.Drawing.Point(3, 3);
            this.设备splitContainer.Name = "设备splitContainer";
            this.设备splitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.设备splitContainer.Size = new System.Drawing.Size(1099, 647);
            this.设备splitContainer.SplitterDistance = 304;
            this.设备splitContainer.TabIndex = 0;
            // 
            // SocketTabPage
            // 
            this.SocketTabPage.BackColor = System.Drawing.SystemColors.Control;
            this.SocketTabPage.Location = new System.Drawing.Point(4, 4);
            this.SocketTabPage.Name = "SocketTabPage";
            this.SocketTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.SocketTabPage.Size = new System.Drawing.Size(1105, 653);
            this.SocketTabPage.TabIndex = 1;
            this.SocketTabPage.Text = "Socket连接";
            // 
            // 系统配置tabPage
            // 
            this.系统配置tabPage.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.系统配置tabPage.Location = new System.Drawing.Point(4, 4);
            this.系统配置tabPage.Name = "系统配置tabPage";
            this.系统配置tabPage.Size = new System.Drawing.Size(1113, 679);
            this.系统配置tabPage.TabIndex = 9;
            this.系统配置tabPage.Text = "系统配置";
            this.系统配置tabPage.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1121, 819);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MainMenuStrip = this.menuStrip1;
            this.MinimumSize = new System.Drawing.Size(999, 556);
            this.Name = "MainForm";
            this.Text = "CxVision";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MainForm_KeyDown);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.运行工具条toolStrip.ResumeLayout(false);
            this.运行工具条toolStrip.PerformLayout();
            this.编辑工具条toolStrip.ResumeLayout(false);
            this.编辑工具条toolStrip.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.工程配置tabPage.ResumeLayout(false);
            this.工程配置tabControl.ResumeLayout(false);
            this.通讯tabPage.ResumeLayout(false);
            this.tabControl3.ResumeLayout(false);
            this.DeviceTabPage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.设备splitContainer)).EndInit();
            this.设备splitContainer.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 文件toolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 编辑toolStripMenuItem;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.ToolStrip 编辑工具条toolStrip;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem;
        private System.Windows.Forms.ToolStripButton 新建NToolStripButton;
        private System.Windows.Forms.ToolStripButton 打开ToolStripButton;
        private System.Windows.Forms.ToolStripButton 保存ToolStripButton;
        private System.Windows.Forms.ToolStripButton 打印PToolStripButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator;
        private System.Windows.Forms.ToolStripButton 剪切UToolStripButton;
        private System.Windows.Forms.ToolStripButton 复制CToolStripButton;
        private System.Windows.Forms.ToolStripButton 粘贴PToolStripButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton 帮助LToolStripButton;
        private System.Windows.Forms.ToolStripMenuItem 视图toolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 工具toolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 检测工具toolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 标定工具toolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 配置toolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 参数配置toolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 调试toolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 传感器配置toolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 新建toolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 打开toolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 保存toolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 另存为toolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 查看点激光toolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 查看线激光toolStripMenuItem;
        private System.Windows.Forms.ToolStrip 运行工具条toolStrip;
        private System.Windows.Forms.ToolStripButton 运行toolStripButton;
        private System.Windows.Forms.ToolStripButton 停止toolStripButton;
        private System.Windows.Forms.ToolStripMenuItem 相机标定toolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem FA相机标定toolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 矩阵标定相机toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem N点标定相机toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem 相机激光标定toolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 运动toolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 标定相机角度toolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 激光标定toolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 标定线激光toolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 标定点激光toolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 标定面激光toolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 相机点激光toolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 相机线激光标定toolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 相机面激光标定toolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 机台校准toolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 光源配置toolStripMenuItem;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ToolStripMenuItem 面阵补偿toolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 线性补偿toolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 运动控制卡配置toolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 参数toolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 用户toolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 登录toolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem 相机9点标定toolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 面激光9点标定toolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 线激光9点标定toolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 点激光9点标定toolStripMenuItem;
        private System.Windows.Forms.ToolStripButton 循环次数toolStripButton;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.ToolStripStatusLabel 日期toolStripStatusLabel;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel4;
        private System.Windows.Forms.ToolStripStatusLabel 时间toolStripStatusLabel;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage 程序tabPage;
        private System.Windows.Forms.TabPage 工程配置tabPage;
        private System.Windows.Forms.TabControl 工程配置tabControl;
        private System.Windows.Forms.TabPage 坐标系TabPage;
        private System.Windows.Forms.TabPage 通讯tabPage;
        private System.Windows.Forms.TabControl tabControl3;
        private System.Windows.Forms.TabPage DeviceTabPage;
        private System.Windows.Forms.TabPage SocketTabPage;
        private System.Windows.Forms.TabPage 标定参数tabPage;
        private System.Windows.Forms.TabPage 夹抓tabPage;
        private System.Windows.Forms.TabPage SensorTabPage;
        private System.Windows.Forms.SplitContainer 设备splitContainer;
        private System.Windows.Forms.ToolStripButton 视图toolStripButton;
        private System.Windows.Forms.ToolStripMenuItem 采集源配置ToolStripMenuItem;
        private System.Windows.Forms.TabPage 通信配置tabPage;
        private System.Windows.Forms.ToolStripButton 联机运行toolStripButton;
        private System.Windows.Forms.ToolStripButton 断开连机toolStripButton;
        private System.Windows.Forms.ToolStripMenuItem 保存布局toolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 程序配置ToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem 相机胶枪标定ToolStripMenuItem;
        private System.Windows.Forms.CheckBox 显示数据checkBox;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel3;
        private System.Windows.Forms.ToolStripStatusLabel 用户toolStripStatusLabel;
        private System.Windows.Forms.Button 连接服务器Btn;
        private System.Windows.Forms.TabPage 系统配置tabPage;
        private System.Windows.Forms.CheckBox 屏蔽checkBox;
        private System.Windows.Forms.ToolStripMenuItem 重置屏幕参数toolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 机器人夹抓标定ToolStripMenuItem;
    }
}


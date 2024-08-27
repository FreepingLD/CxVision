using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VisionBase;
using System.Threading;
using HalconDotNet;
using LD.Config;
using LD.Ui;
using MainVision.WorkFlow;
using System.Web;
using System.Net.Http;
using System.Diagnostics;

namespace MainVision
{
    public partial class MainForm : WeifenLuo.WinFormsUI.Docking.DockContent
    {
        public MainForm()
        {
            InitializeComponent();
            HOperatorSet.SetSystem("clip_region", "false");
        }

     
        /// <summary>
        /// PLC
        /// </summary>
        //public LD.Ui.frmSiemens frmSiemens ;
        public LD.Ui.frmSocket frmSocket ;
        public LD.Ui.frmSerial frmSerial;
        public LD.Ui.frmReport frmReport ;
        public PinReport PinReport;

        CalliperParaProjItem myCam1CalliperPara = new CalliperParaProjItem();
        CalliperParaProjItem myCam2CalliperPara = new CalliperParaProjItem();
        OcrParaItem myCam2OcrPara = new OcrParaItem();
        VariModelProjParaItem myVariModelProjParaItem = new VariModelProjParaItem();

        CalliperMeasureProject myCam1CalliperMeasureProj ;
        CalliperMeasureProject myCam2CalliperMeasureProj;
        OcrCtrl myCam2OcrCtrl ;
        VariModelProjInsp myCam2VariModelProjInsp;


        private void MainForm_Load(object sender, EventArgs e)
        {
            //frmSiemens =  LD.Ui.frmSiemens.Instance;
            frmSocket = new LD.Ui.frmSocket();
            frmSerial = new LD.Ui.frmSerial();
            frmReport = new LD.Ui.frmReport();
            PinReport = new PinReport(); 

            InitLogListView();
            FileLib.Logger.OnLogHappenedEvent += Logger_OnLogHappenedEvent;
            Thread.Sleep(100);
            FileLib.Logger.Pop("打开软件");
            LD.Config.ConfigManager.Instance.Load();

            ConfigParaManager.Read();
            CameraCtrl.Instance.Init();
            LightCtrlManager.Instance.DoInit();
            LightCtrlManager.Instance.DoStart();
            //if( !VisionBase.Camera.Instance.DoInit())
            //    FileLib.Logger.Pop("打开相机失败");            
            PlcDockPanel.Dock = DockStyle.Fill;
            ReportDockPanel1.Dock = DockStyle.Fill;
            LD.Ui.frmSiemens.Instance.Show(this.PlcDockPanel, WeifenLuo.WinFormsUI.Docking.DockState.Document);
            frmSocket.Show(this.PlcDockPanel, WeifenLuo.WinFormsUI.Docking.DockState.Document);
            frmSerial.Show(this.PlcDockPanel, WeifenLuo.WinFormsUI.Docking.DockState.Document);
            PinReport.Show(this.ReportDockPanel1, WeifenLuo.WinFormsUI.Docking.DockState.Document);
            LD.Device.DeviceManager.Instance.DeviceInit();
            LD.Device.DeviceManager.Instance.DeviceStart();
            FrmLightCrlManager.Instance.Show(this.LightDockPanel, WeifenLuo.WinFormsUI.Docking.DockState.Document);
            FrmCameraManager.Instance.Show(this.LightDockPanel, WeifenLuo.WinFormsUI.Docking.DockState.Document);
            FrmVisionProjectPara.Instance.Show(this.ProjectPanel, WeifenLuo.WinFormsUI.Docking.DockState.Document);
            FrmCaliParaManager.Instance.Show(this.ProjectPanel, WeifenLuo.WinFormsUI.Docking.DockState.Document);
            FrmRobtJawManager.Instance.Show(this.ProjectPanel, WeifenLuo.WinFormsUI.Docking.DockState.Document);
            FrmCoordiParaManager.Instance.Show(this.ProjectPanel, WeifenLuo.WinFormsUI.Docking.DockState.Document);
            //FrmOcrParaManager.Instance.Show(this.ProjectPanel, WeifenLuo.WinFormsUI.Docking.DockState.Document);
            //FrmVariModelParaManager.Instance.Show(this.ProjectPanel, WeifenLuo.WinFormsUI.Docking.DockState.Document);
            //FrmCalliperParaManager.Instance.Show(this.ProjectPanel, WeifenLuo.WinFormsUI.Docking.DockState.Document);
            FrmCalliperParaManagerLine.Instance.Show(this.ProjectPanel, WeifenLuo.WinFormsUI.Docking.DockState.Document);
            InitCCDPanel();//显示系统初始化
            StartBtn.BackColor = Color.LightGray;
            this.FormClosing += MyFormClosing;
           // InitVisionWorkFlow();
        }

        private int LogIndex = 0;
        private void InitLogListView()
        {
            foreach (var item in FileLib.Logger.LogContentList)
            {
                string dateTm = string.Format(DateTime.Now.ToString("HH:mm:ss;fff"));
                ListViewItem vitem = new ListViewItem();
                vitem.Text = (++LogIndex).ToString();
                vitem.SubItems.Add(item);
                listViewLog.Items.Add(vitem);
                int index = listViewLog.Items.Count;
                listViewLog.EnsureVisible(index - 1);
            }
        }

        void Logger_OnLogHappenedEvent(string content, bool isError)
        {
            if (listViewLog.IsDisposed) return;

            listViewLog.BeginInvoke(new Action(() => {
                string dateTm = string.Format(DateTime.Now.ToString("HH:mm:ss;fff"));
                ListViewItem item = new ListViewItem();
                item.Text = (++LogIndex).ToString();
                item.SubItems.Add(content);
                listViewLog.Items.Add(item);
                int index = listViewLog.Items.Count;
                listViewLog.EnsureVisible(index - 1);
                if (index > 2000)
                {
                    listViewLog.Items.RemoveAt(1);
                    LogIndex = 2000;
                }
                  
            }));
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            LD.Logic.PlcHandle.Instance.WriteValue(LD.Common.PlcDevice.FOF_start_insp, 0);
            FileLib.Logger.OnLogHappenedEvent -= Logger_OnLogHappenedEvent;
            try{
                //PLC读取线程
                LD.Config.ConfigManager.Instance.ConfigPlc.bReadThread = false;
                //释放设备
                LD.Device.DeviceManager.Instance.DeviceStop();
                LD.Device.DeviceManager.Instance.DeviceRelease();
                LD.Config.ConfigManager.Instance.Save();
                HOperatorSet.CloseAllFramegrabbers();
            }
            catch
            {
            }
            //this.Close();
        }

        /// <summary>
        /// 手动加载一张图片到控件上
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StartBtn_Click(object sender, EventArgs e)
        {
            try
            {

            StartBtn.Enabled = false;
            StartBtn.BackColor = Color.Green;
            StopBtn.BackColor = Color.Red;
            RobotLayOffWorkFlow.Instance.Start();
            AoiWorkFlow2Line.Instance.Start();
            AoiWorkFlowLine.Instance.Start();

           // this.PlcDockPanel.Enabled = false;
            this.ReportDockPanel1.Enabled = false;
            this.LightDockPanel.Enabled = false;
            this.ProjectPanel.Enabled = false;

            this.SaveIMageCheckBox.Checked = true;

            LD.Logic.PlcHandle.Instance.WriteValue(LD.Common.PlcDevice.FOF_start_insp,1);
                ///////////////////
            //VisionStateBaseCalliperLine.AveDataValue.Add(new DataMonitor());
            //this.dataGridView1.DataSource =  VisionStateBaseCalliperLine.AveDataValue;
            //this.dataGridView2.DataSource = AoiWorkFlow2Line.Instance.AveDataValue;

            AoiWorkFlowLine.Instance.ErrorEvent += new EventHandler(Error);
            AoiWorkFlow2Line.Instance.ErrorEvent += new EventHandler(Error);
                RobotLayOffWorkFlow.Instance.ErrorEvent += new EventHandler(Error);
                VisionStateBaseCalliperLine.InfoEvent += new EventHandler(InfoID);

            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void InfoID(object send, EventArgs e)
        {
            try
            { 
            this.Invoke(new Action(() =>
            {
                this.StepIDtextBox.Text = VisionStateBaseCalliperLine.bc_stepID;
                this.UnitIDtextBox.Text = VisionStateBaseCalliperLine.bc_unitID;
            }));
            }
            catch
            {

            }
        }
        private void Error(object send,EventArgs e)
        {
            this.StopBtn_Click(null, e);
            //this.StartBtn_Click(null, null);
        }
        private void InitCCDPanel()
        {
            DisplaySystem.InitCCDPanel();
            DisplaySystem.CreateView("工位1角1");
            DisplaySystem.CreateView("工位1角2");
            DisplaySystem.CreateView("工位1角3");
            DisplaySystem.CreateView("工位1角4");

            DisplaySystem.CreateView("工位2角1");
            DisplaySystem.CreateView("工位2角2");
            DisplaySystem.CreateView("工位2角3");
            DisplaySystem.CreateView("工位2角4");
            DisplaySystem.CreateView("下料机");

            DisplaySystem.AddPanelForCCDView("工位1角1", Panel1Panel) ;
            DisplaySystem.AddPanelForCCDView("工位1角2", Panel2Panel);
            DisplaySystem.AddPanelForCCDView("工位1角3", Panel3Panel);
            DisplaySystem.AddPanelForCCDView("工位1角4", Panel4Panel);

            DisplaySystem.AddPanelForCCDView("工位2角1", Panel5Panel);
            DisplaySystem.AddPanelForCCDView("工位2角2", Panel6Panel);
            DisplaySystem.AddPanelForCCDView("工位2角3", Panel7Panel);
            DisplaySystem.AddPanelForCCDView("工位2角4", Panel8Panel);
            DisplaySystem.AddPanelForCCDView("下料机", LayOffPanel);

            DisplaySystem.SetViewBindingCam("下料机", CameraEnum.Cam0);
            DisplaySystem.SetViewBindingCam("工位1角1", CameraEnum.Cam1);
            DisplaySystem.SetViewBindingCam("工位1角2", CameraEnum.Cam2);
            DisplaySystem.SetViewBindingCam("工位1角3", CameraEnum.Cam1);
            DisplaySystem.SetViewBindingCam("工位1角4", CameraEnum.Cam2);
            DisplaySystem.SetViewBindingCam("工位2角1", CameraEnum.Cam3);
            DisplaySystem.SetViewBindingCam("工位2角2", CameraEnum.Cam4);
            DisplaySystem.SetViewBindingCam("工位2角3", CameraEnum.Cam3);
            DisplaySystem.SetViewBindingCam("工位2角4", CameraEnum.Cam4);
        }
        HObject NowImg = new HObject();
        public void Changejurisdiction()
        {
            ConfigSystem cfg = ConfigManager.Instance.ConfigSystem;
            MainForm1.mainfrm.Invoke(new EventHandler(delegate
            {
                try {
                    if (cfg.State == 3)//操作员operator无示教和保存参数权限
                    {
                        //VisionTeachBtn.Enabled = false;
                        //VisionParaSaveBtn.Enabled = false;
                        //dataGridView1.Enabled = false;
                        //dataGridView2.Enabled = false;
                    }
                    if (cfg.State == 2)//用户User只能示教熟悉流程，不能保存参数权限
                    {
                        //VisionTeachBtn.Enabled = true;
                        //VisionParaSaveBtn.Enabled = false;
                    }
                    if (cfg.State == 1)//管理员Administrator具有示教和保存参数的权限
                    {
                        //VisionTeachBtn.Enabled = true;
                        //VisionParaSaveBtn.Enabled = true;
                    }
                }
                catch (Exception)
                { }
            }));
        }

        //抓图
        private HTuple AcqHandle1;
        private HObject Img;

        private void StopBtn_Click(object sender, EventArgs e)
        {
            try
            {

            StopBtn.BackColor = Color.DarkGray;
            StartBtn.Enabled = true;
            StartBtn.BackColor = Color.LightGray;
            RobotLayOffWorkFlow.Instance.Stop();
            AoiWorkFlow2Line.Instance.Stop();
            AoiWorkFlowLine.Instance.Stop();
            LD.Logic.PlcHandle.Instance.WriteValue(LD.Common.PlcDevice.FOF_start_insp, 0);

            this.dataGridView1.DataSource = null;
            this.dataGridView2.DataSource = null;

            this.PlcDockPanel.Enabled = true;
            this.ReportDockPanel1.Enabled = true;
            this.LightDockPanel.Enabled = true;
            this.ProjectPanel.Enabled = true;

            this.SaveIMageCheckBox.Checked = false;

                AoiWorkFlowLine.Instance.ErrorEvent -= new EventHandler(Error);
                AoiWorkFlow2Line.Instance.ErrorEvent -= new EventHandler(Error);
                RobotLayOffWorkFlow.Instance.ErrorEvent -= new EventHandler(Error);
                VisionStateBaseCalliperLine.InfoEvent -= new EventHandler(InfoID);

            }
            catch
            {

            }
        }
        bool IsContinue = true;
        private void MyFormClosing(object sender, FormClosingEventArgs e)
        {
            //Dialog MyDlg = 
            DialogResult DlgReslult = MessageBox.Show("是否关闭程序", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (DlgReslult == DialogResult.No) e.Cancel = true;
            CameraCtrl.Instance.CloseAllCamera();
           // CameraManager.Instance.Close();
            LightCtrlManager.Instance.DoStop();
            LD.Device.DeviceManager.Instance.DeviceStop();
            LD.Device.DeviceManager.Instance.DeviceRelease();
        }

        private void Button1_Click_2(object sender, EventArgs e)
        {           
            LD.Logic.PlcHandle.Instance.WriteValue(LD.Common.PlcDevice.V_02_TriggerGrab, 1); 
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try {
                string fileName;
                OpenFileDialog openFileDialog1 = new OpenFileDialog();
                openFileDialog1.Filter = "All files (*.*)|*.*|bmp files (*.bmp)|*.bmp";
                openFileDialog1.Multiselect = false;
                openFileDialog1.FilterIndex = 1;
                openFileDialog1.Title = "打开图片文件";
                openFileDialog1.RestoreDirectory = false;
                openFileDialog1.InitialDirectory = @"D:\Image\";
                if (openFileDialog1.ShowDialog() == DialogResult.OK){
                    fileName = openFileDialog1.FileName;
                    HObject img;
                    HTuple wid, hei;
                    HOperatorSet.GenEmptyObj(out img);
                    HOperatorSet.ReadImage(out img, fileName);
                    HOperatorSet.GetImageSize(img, out wid, out hei);
                    ViewControl view1 = DisplaySystem.GetViewControl("上料机");

                    view1.Refresh();
                    view1.ResetView();
                    view1.AddViewImage(img);
                    view1.Repaint();              
                }
                openFileDialog1.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }


        private void InitVisionWorkFlow()
        {
            myCam1CalliperPara = CalliperParaManager.Instance.GetCalliperParaProj("相机1Pin1测距判别");
            myCam2CalliperPara = CalliperParaManager.Instance.GetCalliperParaProj("相机2Pin1测距判别");
            this.myCam1CalliperMeasureProj.setCalliperPara(myCam1CalliperPara);
            this.myCam2CalliperMeasureProj.setCalliperPara(myCam2CalliperPara);
            // Cam1LocalManager.SetLocalModel()
        }

        HObject Cam1Img = new HObject();

        /// <summary>
        /// 相机2 对应 字符识别 logo检测  位置测量
        /// </summary>
        HObject Cam2Img = new HObject();
        private void ShowRlt(bool IsOk)
        {
            this.BeginInvoke(new Action(() =>
            {
                if (IsOk) {
                    //this.RltBtn.Text = "Ok";
                    //this.RltBtn.BackColor = Color.Green;
                }
                else {
                    //this.RltBtn.Text = "Ng";
                    //this.RltBtn.BackColor = Color.Red;
                }
            }));
                
        }
        string curMsg = "工位1角3";
        private void SwitchViewBtn_Click(object sender, EventArgs e)
        {
            if (curMsg.Equals("工位1角4")) {
                DisplaySystem.AddPanelForCCDView("工位1角3", Panel3Panel);
                curMsg = "工位1角3";
            }
            else if (curMsg.Equals("工位1角3")) {
                DisplaySystem.AddPanelForCCDView("工位1角4", Panel3Panel);
                curMsg = "工位1角4";
            }
              
        }
        // 设置图像保存
        private void SaveIMageCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            VisionStateBase.IsSave = this.SaveIMageCheckBox.Checked;
            VisionStateBaseCalliperLine.IsSave = this.SaveIMageCheckBox.Checked;
        }

        private void Button2_Click_1(object sender, EventArgs e)
        {
            //try
            //{
            //    string fileName;
            //    SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            //    saveFileDialog1.Filter = "bmp files (*.bmp)|*.bmp|All files (*.*)|*.*";
            //    saveFileDialog1.FilterIndex = 1;
            //    saveFileDialog1.Title = "保存图片文件";
            //    saveFileDialog1.RestoreDirectory = true;
            //    if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            //    {
            //        fileName = saveFileDialog1.FileName;
            //        if (GrabedImg == null)
            //        {
            //            MessageBox.Show("图片为空，请先采集一张图片");
            //            return;
            //        }
            //        HalconDotNet.HOperatorSet.WriteImage(GrabedImg, "bmp", 0, fileName);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message.ToString());
            //}
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                bool result = false;
                Process[] processes = Process.GetProcesses();
                foreach (var item in processes)
                {
                    if (item.ProcessName == "FtpForm") //
                        result = true;
                }
                if (!result)
                    Process.Start(@"E:\Aoi精度检测\FtpForm\FtpForm\bin\Debug\FtpForm.exe");
            }
            catch
            {

            }
        }

    }
}

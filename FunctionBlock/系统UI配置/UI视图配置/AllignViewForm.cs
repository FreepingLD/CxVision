
using Common;
using HalconDotNet;
using MotionControlCard;
using Sensor;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using View;

namespace FunctionBlock
{
    public partial class AllignViewForm : Form
    {
        private List<ImageDataClass> imageList = new List<ImageDataClass>();
        private Dictionary<string, object> listData = new Dictionary<string, object>();
        private VisualizeView drawObject;
        private ViewConfigParam _viewConfigParam;
        private bool IsLoad = false; // 窗体加载后，设置为true
        private AcqSource acqSource;
        private Queue<object> DataList = new Queue<object>();
        private object lockSys = new object();
        private List<userPixVector> _teachVectorPoint = new List<userPixVector>();
        private BindingList<MeasureResultInfo> ResultInfoList = new BindingList<MeasureResultInfo>();
        private BindingList<OcrResultInfo> OcrResultInfoList = new BindingList<OcrResultInfo>();
        private CompensationParam _param = null;
        private PixData _ROI = null;
        private ISensor _sensor = null;
        private SocketBase _socket;

        public AllignViewForm(ViewConfigParam viewConfigParam)
        {
            InitializeComponent();
            /////////////
            this.drawObject = new VisualizeView(this.hWindowControl1, false);
            this.drawObject.HMouseDoubleClick += new HMouseEventHandler(this.hWindowControl1_DoubleClick);
            BaseFunction.ExcuteCompleted += new ExcuteCompletedEventHandler(DisplayExcuteResult);
            this.drawObject.GrayValueInfo += new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
            this._viewConfigParam = viewConfigParam;
            if (!HWindowManage.HWindowList.ContainsKey(viewConfigParam.ViewName))
            {
                HWindowManage.HWindowList.Add(viewConfigParam.ViewName, this.hWindowControl1.HalconWindow);
            }
            if (!HWindowManage.HWindowControlList.ContainsKey(viewConfigParam.ViewName))
            {
                HWindowManage.HWindowControlList.Add(viewConfigParam.ViewName, this.hWindowControl1);
            }
            if (!HWindowManage.HVisualizeView.ContainsKey(viewConfigParam.ViewName))
            {
                HWindowManage.HVisualizeView.Add(viewConfigParam.ViewName, this.drawObject);
            }
            this.ContextMenu = new ContextMenu();
            this.titleLabel.Text = viewConfigParam.ViewName;
            this.titleLabel.TextAlign = ContentAlignment.MiddleLeft;
            ////
            //UserLoginParamManager.Instance.LoginParam.UserChange += new EventHandler(this.UserChange_Event);
            UserLoginParamManager.Instance.UserChange += new EventHandler(this.UserChange_Event);
            // 绑定触发信号事件
            //AutoRunThreadPlc.Instance.TriggerInfo += new PoseInfoEventHandler(this.WaiteTriggerSingle);
            //if (AcqSourceManage.Instance.GetAcqSource(this._viewConfigParam.CamName)?.Sensor != null)
            //    AcqSourceManage.Instance.GetAcqSource(this._viewConfigParam.CamName).Sensor.ImageAcqComplete += new ImageAcqCompleteEventHandler(ImageAcqComplete_Event);
            /////////////////// 绑定触发信号事件
            AutoRunThreadPlc.Instance.TriggerInfo += new PoseInfoEventHandler(this.WaiteTriggerSingle);
            this._sensor = AcqSourceManage.Instance.GetAcqSource(this._viewConfigParam.CamName)?.Sensor;
            if (this._sensor != null)
                this._sensor.ImageAcqComplete += new ImageAcqCompleteEventHandler(ImageAcqComplete_Event);
            ///// 订阅 Socket 信息 
            if (this._sensor != null)
            {
                switch (this._sensor.ConfigParam.ConnectType)
                {
                    case enUserConnectType.Socket:
                        this._socket = Common.SocketConnectManager.Instance.GetSocket(this._sensor?.ConfigParam.ConnectAddress);
                        break;
                    default:
                        //this._socket = Common.SocketConnectManager.Instance.GetSocket(SystemParamManager.Instance.SysConfigParam.GlobalSocketName);
                        break;
                }
            }
        }
        private void AllignViewForm_Load(object sender, EventArgs e)
        {
            //HOperatorSet.SetWindowAttr("background_color", "sky blue");
            //////////////
            BindProperty();
            //////////////////////////////////////
            this.Location = this._viewConfigParam.Location;
            this.Size = this._viewConfigParam.FormSize;
            this.SignLabel.Text = "";
            //this.SignLabel.Text = this._viewConfigParam.Tag;   // 图形窗口:" + "[" + this.Handle.ToString() + "]";
            this.IsLoad = true;
            this.addContextMenu();
            this.addContextMenu(this.hWindowControl1);
            //this.DisplayData();
            SetLut();
        }
        private void SetLut()
        {
            if (this._viewConfigParam != null)
            {
                switch (this._viewConfigParam.LookUpTable)
                {
                    case enLookUpTable.Default:
                        this.hWindowControl1.HalconWindow.SetLut("default");
                        break;
                    default:
                        this.hWindowControl1.HalconWindow.SetLut(this._viewConfigParam.LookUpTable.ToString());
                        break;
                }
            }
        }
        private void BindProperty()
        {
            try
            {
                this.传感器comboBox1.Items.Clear();
                this.传感器comboBox1.Items.AddRange(AcqSourceManage.Instance.GetAcqSourceName());
                this.传感器comboBox1.Text = this._viewConfigParam.CamName;
                this.程序节点comboBox.Items.Add(this._viewConfigParam.ProgramNode);
                this.程序节点comboBox.SelectedItem = this._viewConfigParam.ProgramNode;
                //this.程序节点comboBox.DataBindings.Add(nameof(this.程序节点comboBox.SelectedItem), this._viewConfigParam, nameof(this._viewConfigParam.ProgramNode), true, DataSourceUpdateMode.OnPropertyChanged);
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
                        this.传感器comboBox1.Enabled = false;
                        this.程序节点comboBox.Enabled = false;
                        this.buttonClose.Enabled = false;
                        //this.标定工具栏toolStrip.Enabled = false;
                        this.执行toolStripButton.Enabled = true;
                        //this.标定配置toolStripButton.Enabled = false;
                        this.示教toolStripButton.Enabled = false;
                        this.相机toolStripDropDownButton.Enabled = false;
                        this.标定toolStripButton.Enabled = false;
                        this.示教toolStripButton.Enabled = false;
                        this.编辑toolStripButton.Enabled = true;
                        this.Roi绘制toolStripDropDownButton.Enabled = false;
                        this.传感器comboBox1.Enabled = false;
                        this.程序节点comboBox.Enabled = false;
                        ////////////////////////////////////////////////////
                        this.buttonMax.Hide();
                        this.buttonMin.Hide();
                        this.buttonClose.Hide();
                        this.tableLayoutPanel2.SetColumnSpan(this.titleLabel, 8);
                        break;
                    case enUserName.工程师:
                        this.传感器comboBox1.Enabled = true;
                        this.程序节点comboBox.Enabled = true;
                        this.buttonClose.Enabled = false;
                        this.执行toolStripButton.Enabled = true;
                        //this.标定工具栏toolStrip.Enabled = true;
                        //this.标定配置toolStripButton.Enabled = true;
                        this.示教toolStripButton.Enabled = true;
                        this.相机toolStripDropDownButton.Enabled = true;
                        this.标定toolStripButton.Enabled = true;
                        this.示教toolStripButton.Enabled = true;
                        this.编辑toolStripButton.Enabled = true;
                        this.Roi绘制toolStripDropDownButton.Enabled = true;
                        this.传感器comboBox1.Enabled = false;
                        this.程序节点comboBox.Enabled = false;
                        ////////////////////////////////////////////////////
                        this.buttonMax.Hide();
                        this.buttonMin.Hide();
                        this.buttonClose.Hide();
                        this.tableLayoutPanel2.SetColumnSpan(this.titleLabel, 8);
                        break;
                    case enUserName.开发人员:
                        this.传感器comboBox1.Enabled = true;
                        this.程序节点comboBox.Enabled = true;
                        this.buttonClose.Enabled = true;
                        this.执行toolStripButton.Enabled = true;
                        //this.标定工具栏toolStrip.Enabled = true;
                        //this.标定配置toolStripButton.Enabled = true;
                        this.示教toolStripButton.Enabled = true;
                        this.相机toolStripDropDownButton.Enabled = true;
                        this.标定toolStripButton.Enabled = true;
                        this.示教toolStripButton.Enabled = true;
                        this.编辑toolStripButton.Enabled = true;
                        this.Roi绘制toolStripDropDownButton.Enabled = true;
                        this.传感器comboBox1.Enabled = true;
                        this.程序节点comboBox.Enabled = true;
                        ////////////////////////////////////////////////////
                        this.buttonMax.Show();
                        this.buttonMin.Show();
                        this.buttonClose.Show();
                        this.tableLayoutPanel2.SetColumnSpan(this.titleLabel, 5);
                        break;
                }
            }
            catch
            {
            }
        }

        private void ClearGraphic(object send, EventArgs e)
        {
            this.listData.Clear();
            this.drawObject.AttachPropertyData.Clear();
            this.drawObject.DrawingGraphicObject(); // 背影不刷新
        }

        private bool SelectTabpage()
        {
            bool result = false;
            Control container = this.Parent;
            if (container == null) return result;
            switch (container.GetType().Name)
            {
                case nameof(TabPage):
                    this.Invoke(new Action(() =>
                    {
                        TabControl tabControl = ((TabPage)container).Parent as TabControl;
                        if (tabControl.SelectedTab != ((TabPage)container))
                        {
                            tabControl.SelectedTab = ((TabPage)container);
                            result = true;
                        }
                        else
                            result = false;
                    }));
                    break;
            }
            return result;
        }

        private bool IsSelect()
        {
            bool result = true;
            Control container = this.Parent;
            if (container == null) return result;
            switch (container.GetType().Name)
            {
                case nameof(TabPage):
                    if (!SystemParamManager.Instance.SysConfigParam.DisablePageSwitch)
                    {
                        this.Invoke(new Action(() =>
                        {
                            TabControl tabControl = ((TabPage)container).Parent as TabControl;
                            if (SystemParamManager.Instance.SysConfigParam.IsAutoRun)        // 自动运行状态下才自动切换
                            {
                                if (tabControl.SelectedTab != ((TabPage)container))
                                    tabControl.SelectedTab = ((TabPage)container);
                                result = true;
                            }
                        }));
                    }
                    break;
            }
            return result;
        }


        private void DisplayExcuteResult(object sender, ExcuteCompletedEventArgs e)
        {
            if (e.SensorName == null) return;
            //if (!this._viewConfigParam.CamName.Contains(e.SensorName) && e.ViewWindow != "ALL") return;  // 这里的相机名称是否要判断?
            if ((e.ViewWindow == null || e.ViewWindow != this._viewConfigParam.ViewName) && e.ViewWindow != "ALL") return;
            if (!IsSelect()) return;  // 当前面的条件都满足时，设置当前选项卡
            if (e.DataContent != null)
            {
                ViewData viewData;
                switch (e.DataContent.GetType().Name)
                {
                    case nameof(ImageDataClass):
                        this.ClearGraphic(null, null);
                        this._teachVectorPoint?.Clear();
                        this.ResultInfoList.Clear();
                        this.DataList.Clear();
                        this.drawObject.AttachPropertyData.Clear(); // 更新图像时清空
                        this.drawObject.BackImage = (ImageDataClass)e.DataContent;
                        if (this._viewConfigParam.IsShowCross)
                            this.drawObject.AddViewObject(new ViewData(this.GenCrossLine(this.drawObject.BackImage.Image), "green"));
                        /////////////////////////////////////////////////////////////////////////////////////////////
                        break;
                    case nameof(HXLDCont):
                        this.drawObject.AddViewObject(new ViewData((HXLDCont)e.DataContent, "red"));
                        break;
                    case nameof(XldDataClass):
                        this.drawObject.AddViewObject(new ViewData(((XldDataClass)e.DataContent).HXldCont, "red"));
                        break;
                    case nameof(RegionDataClass):
                        viewData = new ViewData(((RegionDataClass)e.DataContent).Region, "red", ((RegionDataClass)e.DataContent).Draw);
                        viewData.Draw = "margin";
                        viewData.Color = "red";
                        this.drawObject.AddViewObject(viewData);
                        break;
                    case nameof(userWcsRectangle2):
                        userWcsRectangle2 wcsRect2 = (userWcsRectangle2)e.DataContent;
                        this.drawObject.AddViewObject(new ViewData((wcsRect2).GetPixRectangle2().GetXLD(), wcsRect2.Color.ToString()));
                        break;
                    case nameof(userWcsRectangle1):
                        userWcsRectangle1 wcsRect1 = (userWcsRectangle1)e.DataContent;
                        this.drawObject.AddViewObject(new ViewData((wcsRect1).GetPixRectangle1().GetXLD(), wcsRect1.Color.ToString()));
                        break;

                    case nameof(userWcsPoint):
                        userWcsPoint wcsPoint = (userWcsPoint)e.DataContent;
                        this.drawObject.AddViewObject(new ViewData((wcsPoint).GetPixPoint(), wcsPoint.Color.ToString()));
                        this._teachVectorPoint.Add(new userWcsVector(wcsPoint.X, wcsPoint.Y, wcsPoint.Z, 0, 0, 0, wcsPoint.Grab_x, wcsPoint.Grab_y, wcsPoint.CamParams).GetPixVector());
                        break;
                    case nameof(userWcsVector):
                        userWcsVector wcsVector = (userWcsVector)e.DataContent;
                        userWcsPoint wcsPoint1 = new userWcsPoint(wcsVector.X, wcsVector.Y, wcsVector.Z, wcsVector.CamParams);
                        this.drawObject.AddViewObject(new ViewData((wcsPoint1).GetPixPoint(), wcsPoint1.Color.ToString()));
                        this._teachVectorPoint.Add(wcsVector.GetPixVector());
                        break;
                    case nameof(userWcsLine):
                        userWcsLine wcsLine = (userWcsLine)e.DataContent;
                        if (this._viewConfigParam.IsExtendLine) // 显示延长直线
                        {
                            userWcsLine wcsLine1 = ExtendLine(wcsLine);
                            this.drawObject.AddViewObject(new ViewData(wcsLine1.GetPixLine().GetXLD(), wcsLine1.Color.ToString()));
                        }
                        else
                            this.drawObject.AddViewObject(new ViewData((wcsLine).GetPixLine().GetXLD(), wcsLine.Color.ToString()));
                        break;

                    case nameof(userWcsCircle):
                        userWcsCircle wcsCircle = (userWcsCircle)e.DataContent;
                        this.drawObject.AddViewObject(new ViewData((wcsCircle).GetPixCircle().GetXLD(), wcsCircle.Color.ToString()));
                        this._teachVectorPoint.Add(new userWcsVector(wcsCircle.X, wcsCircle.Y, wcsCircle.Z, 0, 0, 0, wcsCircle.Grab_x, wcsCircle.Grab_y, wcsCircle.CamParams).GetPixVector());
                        break;
                    case nameof(userWcsCircleSector):
                        userWcsCircleSector wcsCircleSector = (userWcsCircleSector)e.DataContent;
                        this.drawObject.AddViewObject(new ViewData((wcsCircleSector).GetPixCircleSector().GetXLD(), wcsCircleSector.Color.ToString()));
                        break;
                    case nameof(userWcsEllipse):
                        userWcsEllipse wcsEllipse = (userWcsEllipse)e.DataContent;
                        this.drawObject.AddViewObject(new ViewData((wcsEllipse).GetPixEllipse().GetXLD(), wcsEllipse.Color.ToString()));
                        break;
                    case nameof(userWcsEllipseSector):
                        userWcsEllipseSector wcsEllipseSector = (userWcsEllipseSector)e.DataContent;
                        this.drawObject.AddViewObject(new ViewData((wcsEllipseSector).GetPixEllipseSector().GetXLD(), wcsEllipseSector.Color.ToString()));
                        break;
                    case nameof(userWcsPolyLine):
                        userWcsPolyLine wcsPolyLine = (userWcsPolyLine)e.DataContent;
                        this.drawObject.AddViewObject(new ViewData((wcsPolyLine).GetPixPolyLine().GetXLD(), wcsPolyLine.Color.ToString()));
                        break;
                    case nameof(userWcsArrow):
                        userWcsArrow wcsArrow = (userWcsArrow)e.DataContent;
                        this.drawObject.AddViewObject(new ViewData((wcsArrow).GetPixArrow().GetArrow(), wcsArrow.Color.ToString()));
                        break;
                    case nameof(userWcsCoordSystem): // 这个条件是否需要添加？
                        userWcsCoordSystem wcsCoordSystem = (userWcsCoordSystem)e.DataContent;
                        this.drawObject.AddViewObject(new ViewData(wcsCoordSystem, "red"));
                        break;
                    case nameof(userPixCoordSystem):
                        userPixCoordSystem pixCoordSystem = e.DataContent as userPixCoordSystem;
                        this.drawObject.AddViewObject(new ViewData(pixCoordSystem.GetWcsCoordSystem(), "red"));
                        break;
                    case nameof(userPixPoint):
                        this.drawObject.AddViewObject(new ViewData(((userPixPoint)e.DataContent).GetXLD(), ((userPixPoint)e.DataContent).Color.ToString()));
                        break;
                    case nameof(userPixLine):
                        this.drawObject.AddViewObject(new ViewData(((userPixLine)e.DataContent).GetXLD(), ((userPixLine)e.DataContent).Color.ToString()));
                        break;
                    case nameof(userPixCircle):
                        this.drawObject.AddViewObject(new ViewData(((userPixCircle)e.DataContent).GetXLD(), ((userPixCircle)e.DataContent).Color.ToString()));
                        break;
                    case nameof(userPixCircleSector):
                        this.drawObject.AddViewObject(new ViewData(((userPixCircleSector)e.DataContent).GetXLD(), ((userPixCircleSector)e.DataContent).Color.ToString()));
                        break;
                    case nameof(userPixEllipse):
                        this.drawObject.AddViewObject(new ViewData(((userPixEllipse)e.DataContent).GetXLD(), ((userPixEllipse)e.DataContent).Color.ToString()));
                        break;
                    case nameof(userPixEllipseSector):
                        this.drawObject.AddViewObject(new ViewData(((userPixEllipseSector)e.DataContent).GetXLD(), ((userPixEllipseSector)e.DataContent).Color.ToString()));
                        break;
                    case nameof(userPixRectangle1):
                        this.drawObject.AddViewObject(new ViewData(((userPixRectangle1)e.DataContent).GetXLD(), ((userPixRectangle1)e.DataContent).Color.ToString()));
                        break;
                    case nameof(userPixRectangle2):
                        this.drawObject.AddViewObject(new ViewData(((userPixRectangle2)e.DataContent).GetXLD(), ((userPixRectangle2)e.DataContent).Color.ToString()));
                        break;
                    case nameof(userOkNgText):
                        this.drawObject.AddViewObject(new ViewData(e.DataContent, "red"));
                        break;
                    case nameof(userTextLable):
                        this.drawObject.AddViewObject(new ViewData(e.DataContent, "red"));
                        break;
                    default:
                        this.drawObject.AddViewObject(new ViewData(e.DataContent, "red"));
                        break;

                    case nameof(MeasureResultInfo):
                        this.Invoke(new Action(() =>
                        {
                            ResultInfoList.Add((MeasureResultInfo)e.DataContent);
                        }));
                        /// 标签这里是否需要实时显示?
                        break;

                    case nameof(OcrResultInfo):
                        this.Invoke(new Action(() =>
                        {
                            this.管控checkBox.Enabled = true;
                            OcrResultInfoList.Add((OcrResultInfo)e.DataContent);
                        }));
                        /// 标签这里是否需要实时显示?
                        break;
                    case nameof(CompensationParam):
                        this.Invoke(new Action(() =>
                        {
                            this.补偿checkBox.Enabled = true;
                            this._param = ((CompensationParam)e.DataContent);
                        }));   // 如果这个窗口接收到了补偿值，那么该窗口对应的按钮将显示
                        break;
                }
            }
        }

        /// <summary>
        /// 窗体移动事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ViewForm_Move(object sender, EventArgs e)
        {
            if (this._viewConfigParam != null)
                this._viewConfigParam.Location = this.Location;
        }

        /// <summary>
        /// 窗体尺寸改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ViewForm_Resize(object sender, EventArgs e)
        {
            if (this._viewConfigParam != null && this.IsLoad) // 在窗体加载后这个事件才起作用
                this._viewConfigParam.FormSize = this.Size;
        }

        private void ViewForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                this.cts?.Cancel();
                this.IsLoad = false;
                //ViewConfigParamManager.Instance.ViewParamList.Remove(this._viewConfigParam); // 关闭窗体时要删除相应的对象
                if (AcqSourceManage.Instance.GetAcqSource(this._viewConfigParam.CamName)?.Sensor != null)
                    AcqSourceManage.Instance.GetAcqSource(this._viewConfigParam.CamName).Sensor.ImageAcqComplete -= new ImageAcqCompleteEventHandler(ImageAcqComplete_Event);
                BaseFunction.ExcuteCompleted -= new ExcuteCompletedEventHandler(DisplayExcuteResult);
                this.drawObject.GrayValueInfo -= new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                this.drawObject.HMouseDoubleClick -= new HMouseEventHandler(this.hWindowControl1_DoubleClick);
                UserLoginParamManager.Instance.UserChange -= new EventHandler(this.UserChange_Event);
                AutoRunThreadPlc.Instance.TriggerInfo -= new PoseInfoEventHandler(this.WaiteTriggerSingle);
                if (this.drawObject != null)
                    this.drawObject.ClearDrawingObject();
            }
            catch
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
                    if (UserLoginParamManager.Instance.CurrentUser == enUserName.操作员) return;
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

                //case 0x0201: //鼠标左键按下的消息
                //    m.Msg = 0x00A1; //更改消息为非客户区按下鼠标
                //    m.LParam = IntPtr.Zero; //默认值
                //    m.WParam = new IntPtr(2); //鼠标放在标题栏内
                //    base.WndProc(ref m);
                //    break;

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
            DialogResult dialogResult = new UserMessageForm().ShowDialog("确定关闭窗体吗？", "关闭窗体");
            if (dialogResult == DialogResult.OK)
            {
                ViewConfigParamManager.Instance.ViewParamList.Remove(this._viewConfigParam);
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
            //this.WindowState = FormWindowState.Minimized;  //最小化
        }
        #endregion

        #region  数据实时采集
        private CancellationTokenSource cts;
        private void 实时采集checkBox_CheckedChangedOld(object sender, EventArgs e)
        {
            try
            {
                switch (this.实时采集checkBox.CheckState)
                {
                    case CheckState.Checked:
                        this.实时采集checkBox.BackColor = Color.Red;
                        this.acqSource = AcqSourceManage.Instance.GetCamAcqSource(this._viewConfigParam.CamName);
                        if (this.acqSource == null) return;
                        cts = new CancellationTokenSource();
                        Dictionary<enDataItem, object> data;
                        Task.Run(() =>
                        {
                            this.drawObject.IsLiveState = true;
                            while (!cts.IsCancellationRequested)
                            {
                                if (SystemParamManager.Instance.SysConfigParam.IsAutoRun && !this._viewConfigParam.IsRunLiveTime)
                                {
                                    this.实时采集checkBox.BackColor = Color.Lime;
                                    cts.Cancel();
                                }
                                /////////////////////////////////////////////////////////////
                                data = this.acqSource.AcqImageData(null);
                                switch (this.acqSource.Sensor?.ConfigParam.SensorType)
                                {
                                    case enUserSensorType.面阵相机:
                                        if (data?.Count > 0)
                                        {
                                            this.drawObject.BackImage = (ImageDataClass)data[enDataItem.Image];
                                            this.drawObject.AttachPropertyData.Clear();
                                            this.drawObject.AttachPropertyData.Add((this.GenCrossLine(this.drawObject.BackImage.Image)));
                                        }
                                        break;
                                    case enUserSensorType.点激光:
                                        data = acqSource.AcqPointData();
                                        if (data?.Count > 0)
                                        {
                                            double[] dist1 = (double[])data[enDataItem.Dist1];
                                            if (dist1 != null && dist1.Length > 0)
                                            {
                                                this.drawObject.ClearViewObject();
                                                if (dist1 != null && dist1.Length > 0)
                                                    this.drawObject.AddViewObject(new ViewData(new userTextLable("距离 = " + dist1.Average().ToString("f5"), 0, 0, 25, "red", enLablePosition.左上角), "green"));
                                            }
                                        }
                                        break;
                                }
                                Thread.Sleep(100);
                            }
                            this.drawObject.IsLiveState = false;
                        });
                        break;
                    default:
                        cts?.Cancel();
                        this.实时采集checkBox.BackColor = Color.Lime;
                        break;
                }
            }
            catch (Exception ex)
            {
                new UserMessageForm(ex.ToString()).ShowDialog();
            }
        }
        private void 实时采集checkBox_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                switch (this.实时采集checkBox.CheckState)
                {
                    case CheckState.Checked:
                        this.实时采集checkBox.BackColor = Color.Red;
                        //this.acqSource = AcqSourceManage.Instance.GetCamAcqSource(this._viewConfigParam.CamName);
                        //if (this.acqSource == null) return;
                        //this._sensor = AcqSourceManage.Instance.GetAcqSource(this._viewConfigParam.CamName)?.Sensor;
                        this.cts = new CancellationTokenSource();
                        Dictionary<enDataItem, object> data = null;
                        Task.Run(() =>
                        {
                            /// 打开光源
                            if (this.acqSource != null && this.acqSource.LightParamList != null)
                            {
                                foreach (var item in this.acqSource.LightParamList)
                                    item.Open();
                            }
                            this.drawObject.IsLiveState = true;
                            while (!this.cts.IsCancellationRequested)
                            {
                                if (SystemParamManager.Instance.SysConfigParam.IsAutoRun && !this._viewConfigParam.IsRunLiveTime)
                                {
                                    this.实时采集checkBox.BackColor = Color.Lime;
                                    this.cts.Cancel();
                                }
                                /////////////////////////////////////////////////////////////
                                switch (this._sensor?.ConfigParam.SensorType)
                                {
                                    case enUserSensorType.面阵相机:
                                        //object trigMode = this._sensor?.GetParam(enSocketInfo.获取触发模式);
                                        //if (trigMode.ToString() == "On")
                                        this._sensor.SetParam("实时采集", 0);
                                        data = this._sensor.ReadData();// this.acqSource.AcqImageData(null);
                                        if (data?.Count > 0)
                                        {
                                            this.drawObject.BackImage = (ImageDataClass)data[enDataItem.Image];
                                            this.drawObject.AttachPropertyData.Clear();
                                            this.drawObject.AttachPropertyData.Add((this.GenCrossLine(this.drawObject.BackImage?.Image)));
                                        }
                                        break;
                                    case enUserSensorType.点激光:
                                        data = this._sensor.ReadData();// acqSource.AcqPointData();
                                        if (data?.Count > 0)
                                        {
                                            double[] dist1 = (double[])data[enDataItem.Dist1];
                                            if (dist1 != null && dist1.Length > 0)
                                            {
                                                this.drawObject.ClearViewObject();
                                                if (dist1 != null && dist1.Length > 0)
                                                    this.drawObject.AddViewObject(new ViewData(new userTextLable("距离 = " + dist1.Average().ToString("f4"), 0, 0, 25, "red", enLablePosition.左上角), "green"));
                                            }
                                        }
                                        break;
                                }
                                Thread.Sleep(100);
                            }
                            this.drawObject.IsLiveState = false;
                        });
                        break;
                    default:
                        this.cts?.Cancel();
                        this.实时采集checkBox.BackColor = Color.Lime;
                        if (this._sensor != null && this._sensor.ConfigParam.ConnectType == enUserConnectType.Socket && this._socket != null) // 表示相机的联接类型为 Socket 
                        {
                            //this._socket.SendDataAsync(new SocketMessage(enSocketInfo.停止实时采集, this.acqSource.Sensor.Name), true);
                        }
                        else
                        {
                            //object trigMode = this._sensor?.GetParam(enSocketInfo.获取触发模式);
                            //if (trigMode.ToString() == "Off")
                            this._sensor?.SetParam("停止采集", 0);
                        }
                        /// 打开光源
                        if (this.acqSource != null && this.acqSource.LightParamList != null)
                        {
                            foreach (var item in this.acqSource.LightParamList)
                                item.Close();
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                new UserMessageForm(ex.ToString()).ShowDialog();
            }
        }
        #endregion


        #region 右键菜单项
        private void addContextMenu()
        {
            ContextMenuStrip ContextMenuStrip1 = new ContextMenuStrip();
            // 添加右键菜单 
            ToolStripItem[] items = new ToolStripMenuItem[]
            {
                new ToolStripMenuItem("重命名"),
            };
            ContextMenuStrip1.Items.AddRange(items); // 在滑赋值前不能调用 
            ContextMenuStrip1.ItemClicked += new ToolStripItemClickedEventHandler(label1ContextMenuStrip_ItemClicked);
            //this.SignLabel.ContextMenuStrip = ContextMenuStrip1;
            this.titleLabel.ContextMenuStrip = ContextMenuStrip1;
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
                        RenameForm renameForm = new RenameForm(this.titleLabel.Text);
                        DialogResult dialogResult = renameForm.ShowDialog();
                        if (dialogResult == DialogResult.OK)
                        {
                            this._viewConfigParam.Tag = renameForm.ReName;
                            this._viewConfigParam.Text = renameForm.ReName;
                            this._viewConfigParam.ViewName = renameForm.ReName;
                            //this.SignLabel.Text = this._viewConfigParam.Tag;
                            this.titleLabel.Text = this._viewConfigParam.Tag;
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

        #region 右键菜单项

        private void addContextMenu(HWindowControl hWindowControl)
        {
            ContextMenuStrip ContextMenuStrip1 = new ContextMenuStrip();
            ContextMenuStrip1.Name = hWindowControl.Name;
            ToolStripItem[] items = null;
            switch (SystemParamManager.Instance.SysConfigParam.Language)
            {
                default:
                case "zh-CN":
                    // 添加右键菜单 
                    items = new ToolStripMenuItem[]
                   {
                     new ToolStripMenuItem("自适应图像(Auto)",null,null,"自适应图像(Auto)"),
                     new ToolStripMenuItem("执行程序",null,null,"执行程序"),
                     new ToolStripMenuItem("配置程序节点",null,null,"配置程序节点"),
                     new ToolStripMenuItem("编辑程序节点",null,null,"编辑程序节点"),
                     new ToolStripMenuItem("编辑示教节点",null,null,"编辑示教节点"),
                     new ToolStripMenuItem("编辑对位节点",null,null,"编辑对位节点"),
                     new ToolStripMenuItem("设置曝光",null,null,"设置曝光"),
                     new ToolStripMenuItem("设置增益",null,null,"设置增益"),
                     new ToolStripMenuItem("设置光源亮度",null,null,"设置光源亮度"),
                     new ToolStripMenuItem("十字线",null,null,"十字线"),
                     new ToolStripMenuItem("启用运行实时",null,null,"启用运行实时"),
                     new ToolStripMenuItem("显示延长直线",null,null,"显示延长直线"),
                     new ToolStripMenuItem("启用绑定节点执行",null,null,"启用绑定节点执行"),
                     new ToolStripMenuItem("启用触发信号绑定",null,null,"启用触发信号绑定"),
                     new ToolStripMenuItem("相机标定",null,null,"相机标定"),
                     new ToolStripMenuItem("相机映射标定",null,null,"相机映射标定"),
                     new ToolStripMenuItem("尺寸管控",null,null,"尺寸管控"),
                     new ToolStripMenuItem("对位补偿设置",null,null,"对位补偿设置"),
                     new ToolStripMenuItem("设置相机参数",null,null,"设置相机参数"),
                     new ToolStripMenuItem("3D(View)",null,null,"3D(View)"),
                     new ToolStripMenuItem("保存图像",null,null,"保存图像") ,
                     new ToolStripMenuItem("保存点云",null,null,"保存点云"),
                     new ToolStripMenuItem("加载图像",null,null,"加载图像"),
                     new ToolStripMenuItem("清除窗口(Clear)",null,null,"清除窗口(Clear)"),
                      new ToolStripMenuItem("设置查找表(Lut)",null,null,"设置查找表(Lut)"),
                   };
                    if (this._viewConfigParam.IsShowCross)
                        items[9].Text = "隐藏十字线";
                    else
                        items[9].Text = "显示十字线";
                    if (this._viewConfigParam.IsRunLiveTime)
                        items[10].Text = "禁用运行实时";
                    else
                        items[10].Text = "启用运行实时";
                    if (this._viewConfigParam.IsExtendLine)
                        items[11].Text = "隐藏延长直线";
                    else
                        items[11].Text = "显示延长直线";
                    if (this._viewConfigParam.IsNodeBindingExcute)
                        items[12].Text = "禁用绑定节点执行";
                    else
                        items[12].Text = "启用绑定节点执行";
                    if (this._viewConfigParam.EnableTriggerSingleBinding)
                        items[13].Text = "禁用触发信号绑定";
                    else
                        items[13].Text = "启用触发信号绑定";
                    break;
                case "en-US":
                    // 添加右键菜单 
                    items = new ToolStripMenuItem[]
                   {
                     new ToolStripMenuItem("Auto Window Image",null,null,"自适应图像(Auto)"),
                     new ToolStripMenuItem("Excute Program Node",null,null,"执行程序"),
                     new ToolStripMenuItem("Config Program Node",null,null,"配置程序节点"),
                     new ToolStripMenuItem("Edite Program Node",null,null,"编辑程序节点"),
                     new ToolStripMenuItem("Edite Teach Node",null,null,"编辑示教节点"),
                     new ToolStripMenuItem("Edite Align Node",null,null,"编辑对位节点"),
                     new ToolStripMenuItem("Set Expose",null,null,"设置曝光"),
                     new ToolStripMenuItem("Set Gain",null,null,"设置增益"),
                     new ToolStripMenuItem("Set Light Brightness",null,null,"设置光源亮度"),
                     new ToolStripMenuItem("Cross Line",null,null,"十字线"),
                     new ToolStripMenuItem("Enable Run Live",null,null,"启用运行实时"),
                     new ToolStripMenuItem("Show Extented Line",null,null,"显示延长直线"),
                     new ToolStripMenuItem("Enable Binding Node Excute",null,null,"启用绑定节点执行"),
                     new ToolStripMenuItem("Enable Trigger Single Binding",null,null,"启用触发信号绑定"),
                     new ToolStripMenuItem("Map Calib",null,null,"相机标定"),
                     new ToolStripMenuItem("Camera Map Calib",null,null,"相机映射标定"),
                     new ToolStripMenuItem("Size Control",null,null,"尺寸管控"),
                     new ToolStripMenuItem("Align Compensate Data ",null,null,"对位补偿设置"),
                     new ToolStripMenuItem("Set Camera Param",null,null,"设置相机参数"),
                     new ToolStripMenuItem("3D(View)",null,null,"3D(View)"),
                     new ToolStripMenuItem("Save Image",null,null,"保存图像") ,
                     new ToolStripMenuItem("Save Point Cloud",null,null,"保存点云"),
                     new ToolStripMenuItem("Load Image",null,null,"加载图像"),
                     new ToolStripMenuItem("Clear Window Image",null,null,"清除窗口(Clear)"),
                       new ToolStripMenuItem("Set Lut(Lut)",null,null,"设置查找表(Lut)"),
                   };
                    if (this._viewConfigParam.IsShowCross)
                        items[9].Text = "Hide Cross Line";
                    else
                        items[9].Text = "Show Cross Line";
                    if (this._viewConfigParam.IsRunLiveTime)
                        items[10].Text = "Disable Run Live";
                    else
                        items[10].Text = "Enable Run Live";
                    if (this._viewConfigParam.IsExtendLine)
                        items[11].Text = "Hide Extented Line";
                    else
                        items[11].Text = "Show Extented Line";
                    if (this._viewConfigParam.IsNodeBindingExcute)
                        items[12].Text = "Disable Binding Node Excute";
                    else
                        items[12].Text = "Enable Binding Node Excute";
                    if (this._viewConfigParam.EnableTriggerSingleBinding)
                        items[13].Text = "Disable Trigger Single Binding";
                    else
                        items[13].Text = "Enable Trigger Single Binding";
                    break;
            }
            ContextMenuStrip1.Items.AddRange(items); // 在滑赋值前不能调用 
            ContextMenuStrip1.ItemClicked += new ToolStripItemClickedEventHandler(hWindowControlContextMenuStrip_ItemClicked);
            hWindowControl.ContextMenuStrip = ContextMenuStrip1;
        }
        private void hWindowControlContextMenuStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            string name = e.ClickedItem.Name;
            try
            {
                ((ContextMenuStrip)sender).Close();
                switch (name)
                {
                    case "自适应图像(Auto)":
                        this.drawObject.AutoWindows();
                        break;
                    case "配置程序节点":
                    case "配置视图程序":
                        switch (UserLoginParamManager.Instance.CurrentUser)
                        {
                            default:
                            case enUserName.操作员:
                            case enUserName.工程师:
                                new UserMessageForm().ShowDialog("无权限操作，请登录开发人员账户!");
                                return;

                            case enUserName.开发人员:
                                if (new WindowConfigForm(this._viewConfigParam).ShowDialog() == DialogResult.OK)
                                {
                                    this.程序节点comboBox.Items.Clear();
                                    this.程序节点comboBox.Items.Add(this._viewConfigParam.ProgramNode);
                                    this.程序节点comboBox.SelectedIndex = 0;
                                }
                                break;
                        }
                        break;
                    case "编辑程序节点":
                    case "编辑程序":
                        bool isSucess = false;
                        foreach (KeyValuePair<string, TreeViewWrapClass> item in ProgramForm.Instance.ProgramDic)
                        {
                            foreach (TreeNode item2 in item.Value.TreeView.Nodes)
                            {
                                TreeNode node = null;
                                //GetEditeNode(item2, item.Key, this._viewConfigParam.ProgramNode, out node);
                                if (this._viewConfigParam.ProgramNode == null || this._viewConfigParam.ProgramNode == "" || this._viewConfigParam.ProgramNode == "NONE")
                                    GetEditeNode(item2, item.Key, this.drawObject.BackImage.PathNode, out node); // 如果没有指定节点，则使用图像自带的节点路径
                                else
                                    GetEditeNode(item2, item.Key, this._viewConfigParam.ProgramNode, out node);

                                if (node != null)
                                {
                                    isSucess = true;
                                    item.Value.treeView1_Edite(this, node, item.Key, this.drawObject.BackImage);
                                    return;
                                }
                            }
                        }
                        if (!isSucess)
                            new UserMessageForm().ShowDialog("未获取到可执行节点!!");
                        break;
                    case "编辑示教节点":
                    case "编辑示教":
                        if (UserLoginParamManager.Instance.CurrentUser == enUserName.操作员)
                        {
                            new UserMessageForm().ShowDialog("请登录工程师或研发人员账户");
                            return;
                        }
                        isSucess = false;
                        foreach (KeyValuePair<string, TreeViewWrapClass> item in ProgramForm.Instance.ProgramDic)
                        {
                            foreach (TreeNode item2 in item.Value.TreeView.Nodes)
                            {
                                TreeNode node;
                                GetEditeNode(item2, item.Key, this._viewConfigParam.TeachNode, out node);
                                if (node != null)
                                {
                                    isSucess = true;
                                    item.Value.treeView1_Edite(this, node, item.Key);
                                    return;
                                }
                            }
                        }
                        if (!isSucess)
                            new UserMessageForm().ShowDialog("未获取到可执行节点!!");
                        break;
                    case "编辑对位节点":
                    case "编辑对位":
                        if (UserLoginParamManager.Instance.CurrentUser == enUserName.操作员)
                        {
                            new UserMessageForm().ShowDialog("请登录工程师或研发人员账户");
                            return;
                        }
                        isSucess = false;
                        foreach (KeyValuePair<string, TreeViewWrapClass> item in ProgramForm.Instance.ProgramDic)
                        {
                            foreach (TreeNode item2 in item.Value.TreeView.Nodes)
                            {
                                TreeNode node;
                                GetEditeNode(item2, item.Key, this._viewConfigParam.AlignNode, out node);
                                if (node != null)
                                {
                                    isSucess = true;
                                    item.Value.treeView1_Edite(this, node, item.Key);
                                    return;
                                }
                            }
                        }
                        if (!isSucess)
                            new UserMessageForm().ShowDialog("未获取到可执行节点!!");
                        break;
                    case "执行程序":
                        isSucess = false;
                        foreach (KeyValuePair<string, TreeViewWrapClass> item in ProgramForm.Instance.ProgramDic)
                        {
                            foreach (TreeNode item2 in item.Value.TreeView.Nodes)
                            {
                                TreeNode node;
                                GetEditeNode(item2, item.Key, this._viewConfigParam.ProgramNode, out node);
                                if (node != null)
                                {
                                    isSucess = true;
                                    IFunction function = item2.Tag as IFunction;
                                    function?.Execute(item2, "manual"); // manual: 表示手动模式操作
                                    return;
                                }
                            }
                        }
                        if (!isSucess)
                            new UserMessageForm().ShowDialog("未获取到可执行节点!!");
                        break;
                    case "设置曝光":
                        if (AcqSourceManage.Instance.GetAcqSource(this._viewConfigParam.CamName) == null || AcqSourceManage.Instance.GetAcqSource(this._viewConfigParam.CamName).Sensor == null) return;
                        string value = AcqSourceManage.Instance.GetAcqSource(this._viewConfigParam.CamName).Sensor.GetParam("曝光").ToString();
                        RenameForm renameForm = new RenameForm(value);
                        renameForm.ShowDialog();
                        AcqSourceManage.Instance.GetAcqSource(this._viewConfigParam.CamName).Sensor.SetParam("曝光", renameForm.ReName);
                        break;
                    case "设置增益":
                        if (AcqSourceManage.Instance.GetAcqSource(this._viewConfigParam.CamName) == null || AcqSourceManage.Instance.GetAcqSource(this._viewConfigParam.CamName).Sensor == null) return;
                        value = AcqSourceManage.Instance.GetAcqSource(this._viewConfigParam.CamName).Sensor.GetParam("增益").ToString();
                        renameForm = new RenameForm(value);
                        renameForm.ShowDialog();
                        AcqSourceManage.Instance.GetAcqSource(this._viewConfigParam.CamName).Sensor.SetParam("增益", renameForm.ReName);
                        break;
                    case "设置光源亮度":
                        if (AcqSourceManage.Instance.GetAcqSource(this._viewConfigParam.CamName) == null) return;
                        LightSetForm lightForm = new LightSetForm(AcqSourceManage.Instance.GetAcqSource(this._viewConfigParam.CamName));
                        lightForm.ShowDialog();
                        break;
                    case "十字线":
                    case "隐藏十字线":
                    case "显示十字线":
                        if (this._viewConfigParam.IsShowCross)
                        {
                            e.ClickedItem.Text = "显示十字线";
                            this._viewConfigParam.IsShowCross = false;
                        }
                        else
                        {
                            e.ClickedItem.Text = "隐藏十字线";
                            this._viewConfigParam.IsShowCross = true;
                        }
                        ViewConfigParamManager.Instance.Save();
                        break;
                    case "启用运行实时":
                    case "禁用运行实时":
                        if (this._viewConfigParam.IsRunLiveTime)
                        {
                            e.ClickedItem.Text = "启用运行实时";
                            this._viewConfigParam.IsRunLiveTime = false;
                        }
                        else
                        {
                            e.ClickedItem.Text = "禁用运行实时";
                            this._viewConfigParam.IsRunLiveTime = true;
                        }
                        ViewConfigParamManager.Instance.Save();
                        break;
                    case "隐藏延长直线":
                    case "显示延长直线":
                        if (this._viewConfigParam.IsExtendLine)
                        {
                            e.ClickedItem.Text = "显示延长直线";
                            this._viewConfigParam.IsExtendLine = false;
                        }
                        else
                        {
                            e.ClickedItem.Text = "隐藏延长直线";
                            this._viewConfigParam.IsExtendLine = true;
                        }
                        ViewConfigParamManager.Instance.Save();
                        break;
                    case "禁用绑定节点执行":
                    case "启用绑定节点执行":
                        if (this._viewConfigParam.IsNodeBindingExcute)
                        {
                            e.ClickedItem.Text = "启用绑定节点执行";
                            this._viewConfigParam.IsNodeBindingExcute = false;
                        }
                        else
                        {
                            e.ClickedItem.Text = "禁用绑定节点执行";
                            this._viewConfigParam.IsNodeBindingExcute = true;
                        }
                        ViewConfigParamManager.Instance.Save();
                        break;
                    case "启用触发信号绑定":
                    case "禁用触发信号绑定":
                        if (this._viewConfigParam.EnableTriggerSingleBinding)
                        {
                            e.ClickedItem.Text = "启用触发信号绑定";
                            this._viewConfigParam.EnableTriggerSingleBinding = false;
                        }
                        else
                        {
                            e.ClickedItem.Text = "禁用触发信号绑定";
                            this._viewConfigParam.EnableTriggerSingleBinding = true;
                        }
                        ViewConfigParamManager.Instance.Save();
                        break;
                    case "相机标定":
                        if (this.传感器comboBox1.SelectedItem == null)
                        {
                            new UserMessageForm().ShowDialog("窗口上未指定相机名!!!");
                            return;
                        }
                        CameraParam NowCaliPara = AcqSourceManage.Instance.GetAcqSource(this.传感器comboBox1.SelectedItem.ToString())?.Sensor?.CameraParam;
                        CameraParam MapTargetNowCaliPara = null;
                        if (NowCaliPara == null) return;
                        //////////////////////////////////////////
                        switch (NowCaliPara.CaliParam.CamCaliModel)
                        {
                            case enCamCaliModel.UpDnCamCalibWcs:
                            case enCamCaliModel.UpDnCamCalibPix:
                                if (NowCaliPara == null) return;
                                if (MapTargetNowCaliPara == null)
                                {
                                    new UserMessageForm().ShowDialog("未指定目标相机，不能进行映射标定!");
                                    return;
                                }
                                UpDnCamCalibSimpleForm frmCaliNow = new UpDnCamCalibSimpleForm(NowCaliPara, MapTargetNowCaliPara);
                                frmCaliNow.ShowDialog();
                                break;
                            case enCamCaliModel.NPointCali:
                                CamNPointCalibParamSimpleForm npointFrmCali = new CamNPointCalibParamSimpleForm(NowCaliPara);
                                npointFrmCali.ShowDialog();
                                break;
                            case enCamCaliModel.HomMat2D:
                            case enCamCaliModel.HandEyeCali:
                            case enCamCaliModel.Cali9PtCali:
                                Cam9PointCalibrateSimpleForm frmCali = new Cam9PointCalibrateSimpleForm(NowCaliPara);
                                frmCali.ShowDialog();
                                break;

                            case enCamCaliModel.CaliCaliBoard:
                                CaliCaliboardSimpleForm frmCaliboard = new CaliCaliboardSimpleForm(NowCaliPara);
                                frmCaliboard.ShowDialog();
                                break;

                            case enCamCaliModel.CamParamPose:
                                AreaScanDivisionCalibrateForm matrixCalibrateForm = new AreaScanDivisionCalibrateForm(NowCaliPara);
                                matrixCalibrateForm.ShowDialog();
                                break;

                            case enCamCaliModel.RefPose:
                                CameraGlueGunCalibrateForm matrixCalibrateForm2 = new CameraGlueGunCalibrateForm(NowCaliPara);
                                matrixCalibrateForm2.ShowDialog();
                                break;
                            default:
                                frmCali = new Cam9PointCalibrateSimpleForm(NowCaliPara);   //
                                break;
                        }
                        break;
                    //case "相机映射标定":
                    //    if (this.传感器comboBox1.SelectedItem == null)
                    //    {
                    //        new UserMessageForm().ShowDialog("窗口上未指定相机名!!!,请在标定管理中配置映射目标相机……");
                    //        return;
                    //    }
                    //    NowCaliPara = AcqSourceManage.Instance.GetAcqSource(this.传感器comboBox1.SelectedItem.ToString()).Sensor.CameraParam; ;
                    //    if (NowCaliPara == null) return;
                    //    MapTargetNowCaliPara = AcqSourceManage.Instance.GetAcqSource(NowCaliPara.CaliParam.MapCamName)?.Sensor?.CameraParam;
                    //    if (MapTargetNowCaliPara == null)
                    //    {
                    //        new UserMessageForm().ShowDialog("未指定目标相机，不能进行映射标定!");
                    //        return;
                    //    }
                    //    UpDnCamCaliForm frmCaliNow2 = new UpDnCamCaliForm(NowCaliPara, MapTargetNowCaliPara);
                    //    frmCaliNow2.ShowDialog();
                    //    break;

                    case "3D(View)":

                        break;

                    case "保存图像":
                        SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                        saveFileDialog1.Filter = "bmp files (*.bmp)|*.bmp|All files (*.*)|*.*";
                        saveFileDialog1.FilterIndex = 0;
                        saveFileDialog1.ShowDialog();
                        if (this.drawObject.BackImage != null && this.drawObject.BackImage.Image.IsInitialized())
                            this.drawObject.BackImage.Image.WriteImage("bmp", 0, saveFileDialog1.FileName);
                        else
                        {
                            if (saveFileDialog1.FileName != null && saveFileDialog1.FileName.Length > 0)
                                new UserMessageForm().ShowDialog("图像内容为空");
                        }
                        break;
                    case "保存点云":
                        saveFileDialog1 = new SaveFileDialog();
                        saveFileDialog1.Filter = "ply files (*.ply)|*.ply|txt files (*.txt)|*.txt|om3 files (*.om3)|*.om3|stl files (*.stl)|*.stl|obj files (*.obj)|*.obj|dxf files (*.dxf)|*.dxf|All files (*.*)|*.*";
                        saveFileDialog1.FilterIndex = 3;
                        saveFileDialog1.ShowDialog();
                        if (this.drawObject.PointCloudModel3D != null)
                        {
                            HObjectModel3D hObjectModel3D = HObjectModel3D.UnionObjectModel3d(this.drawObject.PointCloudModel3D.ObjectModel3D, "points_surface");
                            hObjectModel3D.WriteObjectModel3d(new FileInfo(saveFileDialog1.FileName).Extension, saveFileDialog1.FileName, new HTuple(), new HTuple());
                            hObjectModel3D.Dispose();
                        }
                        else
                        {
                            if (saveFileDialog1.FileName != null && saveFileDialog1.FileName.Length > 0)
                                new UserMessageForm().ShowDialog("点云句柄内容为空");
                        }
                        break;
                    case "清除窗口(Clear)":
                        this.drawObject.ClearWindow();
                        break;
                    case "尺寸管控":
                        //new ElementViewForm(this.ResultInfo).Show();
                        new SizeControlForm(this.ResultInfoList).Show();
                        break;
                    case "设置查找表(Lut)":
                        LookTableForm tableForm = new LookTableForm();
                        this._viewConfigParam.LookUpTable = tableForm.Lut;
                        this.SetLut();
                        break;
                    case "对位补偿设置":
                        new CompensateForm(this._param, this._viewConfigParam).Show();
                        break;
                    case "设置相机参数":
                        new CameraParamForm(AcqSourceManage.Instance.GetAcqSource(this._viewConfigParam.CamName).Sensor.CameraParam).Show();
                        break;
                    case "读取图像":
                    case "加载图像":
                        //string path = new FileOperate().OpenImage();
                        //ReadImageParam imageParam = new ReadImageParam();
                        //imageParam.ReadImage(path);
                        //HImage sourceImage = new HImage(path);
                        //this.drawObject.BackImage = new ImageDataClass(sourceImage);
                        OpenFileDialog ofd = new OpenFileDialog();
                        ofd.Filter = "bmp文件(*.bmp)|*.bmp|hobj文件(*.hobj)|*.hobj|tiff文件(*.tiff)|*.tiff|jpg文件(*.jpg)|*.jpg|jpeg文件(*.jpeg)|*.jpeg|png文件(*.png)|*.png|ras文件(*.ras)|*.ras|dxf文件(.dxf)|*.dxf|hdev文件(.hdev)|*.hdev|所有文件(*.*)|*.**";
                        ofd.RestoreDirectory = false;
                        ofd.FilterIndex = 0;
                        if (ofd.ShowDialog() == DialogResult.OK)
                        {
                            this.drawObject.BackImage = new ImageDataClass(new HImage(ofd.FileName)); //, this._acqSource.Sensor.CameraParam
                            this.drawObject.BackImage.ViewWindow = this._viewConfigParam.ViewName;
                            this.drawObject.BackImage.Tag = 1;
                        }

                        break;
                }
            }
            catch (Exception ex)
            {
                new UserMessageForm(ex.ToString()).ShowDialog();
            }
        }
        private void hWindowControl1_MouseMove(object sender, GrayValueInfoEventArgs e)
        {
            if (e.GaryValue.Length > 0)
            {
                int row1, col1, row2, col2;
                this.hWindowControl1.HalconWindow.GetPart(out row1, out col1, out row2, out col2);
                this.hWindowControl1.HalconWindow.SetTposition((int)(row1 + (row2 - row1) * 0.025), (int)(col1 + (col2 - col1) * 0.015));
                this.hWindowControl1.HalconWindow.SetFont("-Consolas-" + 12 + "- *-0-*-*-1-");
                this.hWindowControl1.HalconWindow.SetColor("red");
                this.drawObject.CopyBufferWindowView();
                string content;
                switch (e.GaryValue.Length)
                {
                    default:
                    case 1:
                        content = string.Join("", "Row:", e.Row, " ", "Col:", e.Col, " ", "Gray:", e.GaryValue[0]);
                        break;
                    case 2:
                        content = string.Join("", "Row:", e.Row, " ", "Col:", e.Col, " ", "Gray:", e.GaryValue[0], ", ", e.GaryValue[1]);
                        break;
                    case 3:
                        content = string.Join("", "Row:", e.Row, " ", "Col:", e.Col, " ", "Gray:", e.GaryValue[0], ", ", e.GaryValue[1], ", ", e.GaryValue[2]);
                        break;
                }
                this.hWindowControl1.HalconWindow.WriteString(content);
                //this.hWindowControl1.HalconWindow.WriteString(string.Join("", "Row:", e.Row, " ", "Col:", e.Col, " ", "Gray:", e.GaryValue[0]));
            }
        }

        #endregion

        private HXLDCont GenCrossLine(HImage hImage)
        {
            HXLDCont hXLDCont = new HXLDCont();
            if (hImage != null && hImage.IsInitialized())
            {
                hXLDCont.GenEmptyObj();
                int width, height;
                hImage.GetImageSize(out width, out height);
                hXLDCont = hXLDCont.ConcatObj(new HXLDCont(new HTuple(height * 0.5, height * 0.5), new HTuple(0, width)));
                hXLDCont = hXLDCont.ConcatObj(new HXLDCont(new HTuple(0, height), new HTuple(width * 0.5, width * 0.5)));
            }
            return hXLDCont;
        }

        private void ViewForm_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, WM_SYSCOMMAND, SC_MOVE + HTCAPTION, 0);
        }

        private void titleLabel_MouseDown(object sender, MouseEventArgs e)
        {
            ViewForm_MouseDown(null, null);
        }

        private void titleLabel_MouseEnter(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Hand;
            this.titleLabel.BackColor = System.Drawing.Color.Orange;// System.Drawing.SystemColors.HotTrack;
        }

        private void titleLabel_MouseLeave(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Default;
            this.titleLabel.BackColor = System.Drawing.Color.LightGray;// System.Drawing.SystemColors.Control;
        }


        private void hWindowControl1_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy; // 在进入该控件时必需要设置拖放效果，不然拖放完成事件不会执行 ，， 将要显示的元素拖放到该窗口上来
        }


        private void 传感器comboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            try
            {
                if (this.IsLoad)
                {
                    this.cts?.Cancel(); // 他改变时一定要去悼  
                    if (AcqSourceManage.Instance.GetAcqSource(this._viewConfigParam.CamName)?.Sensor != null)
                        AcqSourceManage.Instance.GetAcqSource(this._viewConfigParam.CamName).Sensor.ImageAcqComplete -= new ImageAcqCompleteEventHandler(ImageAcqComplete_Event);
                    this._viewConfigParam.CamName = this.传感器comboBox1.SelectedItem.ToString();
                    ViewConfigParamManager.Instance.Save();
                    if (AcqSourceManage.Instance.GetAcqSource(this._viewConfigParam.CamName)?.Sensor != null)
                        AcqSourceManage.Instance.GetAcqSource(this._viewConfigParam.CamName).Sensor.ImageAcqComplete += new ImageAcqCompleteEventHandler(ImageAcqComplete_Event);
                    this._sensor = AcqSourceManage.Instance.GetAcqSource(this._viewConfigParam.CamName)?.Sensor;
                    ///// 订阅 Socket 信息 
                    if (this._sensor != null)
                    {
                        switch (this._sensor.ConfigParam.ConnectType)
                        {
                            case enUserConnectType.Socket:
                                this._socket = Common.SocketConnectManager.Instance.GetSocket(this._sensor?.ConfigParam.ConnectAddress);
                                break;
                            default:
                                this._socket = Common.SocketConnectManager.Instance.GetSocket(SystemParamManager.Instance.SysConfigParam.GlobalSocketName);
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                new UserMessageForm().ShowDialog("保存视图配置参数出错" + ex.ToString());
            }

        }


        private void hWindowControl1_DoubleClick(Object sender, HalconDotNet.HMouseEventArgs e)
        {
            //int a = 10;
            this.buttonMax_Click(null, null);
        }


        /// <summary>
        /// 目前只支持选择工具节点
        /// </summary>
        /// <param name="node"></param>
        /// <param name="key"></param>
        /// <param name="programNode"></param>
        /// <param name="selectNode"></param>
        private void GetEditeNode(TreeNode node, string key, string programNode, out TreeNode selectNode)
        {
            selectNode = null;
            if (node.Name.Contains("Tool"))
            {
                if (key + "." + node.FullPath.Replace("\\", ".") == programNode)
                {
                    selectNode = node;
                    return;
                }
                else
                {
                    foreach (TreeNode item in node.Nodes)
                    {
                        if (selectNode == null)
                            GetEditeNode(item, key, programNode, out selectNode); // 递归调用
                    }
                }
            }
        }


        private void 标定工具栏toolStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            try
            {
                if (this.传感器comboBox1.SelectedItem == null)
                {
                    new UserMessageForm().ShowDialog("窗口上未指定相机名!!!");
                    return;
                }
                CameraParam NowCaliPara = AcqSourceManage.Instance.GetAcqSource(this.传感器comboBox1.SelectedItem.ToString())?.Sensor?.CameraParam;
                CameraParam MapTargetNowCaliPara = null;
                if (NowCaliPara == null) return;
                ////////////////////////////////////////////////////////
                switch (e.ClickedItem.Name)
                {
                    case nameof(this.执行toolStripButton):
                        //////////////////////////////////////////
                        bool isSucess = false;
                        foreach (KeyValuePair<string, TreeViewWrapClass> item in ProgramForm.Instance.ProgramDic)
                        {
                            foreach (TreeNode item2 in item.Value.TreeView.Nodes)
                            {
                                TreeNode node;
                                GetEditeNode(item2, item.Key, this._viewConfigParam.ProgramNode, out node);
                                if (node != null)
                                {
                                    isSucess = true;
                                    IFunction function = item2.Tag as IFunction;
                                    function?.Execute(item2, "manual"); // manual: 表示手动模式操作
                                    return;
                                }
                            }
                        }
                        if (!isSucess)
                            new UserMessageForm().ShowDialog("未获取到可执行节点!!");
                        break;
                    case nameof(this.标定toolStripButton):
                        //case nameof(this.标定配置toolStripButton):
                        switch (NowCaliPara.CaliParam.CamCaliModel)
                        {
                            case enCamCaliModel.UpDnCamCalibWcs:
                            case enCamCaliModel.UpDnCamCalibPix:
                                MapTargetNowCaliPara = AcqSourceManage.Instance.GetAcqSource(NowCaliPara.SensorName)?.Sensor?.CameraParam;
                                if (MapTargetNowCaliPara == null)
                                {
                                    new UserMessageForm().ShowDialog("未指定目标相机，不能进行映射标定!");
                                    return;
                                }
                                UpDnCamCalibSimpleForm frmCaliNow = new UpDnCamCalibSimpleForm(NowCaliPara, MapTargetNowCaliPara);
                                frmCaliNow.ShowDialog();
                                break;
                            case enCamCaliModel.NPointCali:
                                CamNPointCalibParamSimpleForm Instance = new CamNPointCalibParamSimpleForm(NowCaliPara);
                                Instance.ShowDialog();
                                break;
                            case enCamCaliModel.HomMat2D:
                            case enCamCaliModel.HandEyeCali:
                            case enCamCaliModel.Cali9PtCali:
                                Cam9PointCalibrateSimpleForm Instance2 = new Cam9PointCalibrateSimpleForm(NowCaliPara);
                                Instance2.ShowDialog();
                                break;

                            case enCamCaliModel.CaliCaliBoard:
                                CaliCaliboardSimpleForm Instance3 = new CaliCaliboardSimpleForm(NowCaliPara);
                                Instance3.ShowDialog();
                                break;
                        }
                        break;
                    case nameof(this.示教toolStripButton):
                        isSucess = false;
                        foreach (KeyValuePair<string, TreeViewWrapClass> item in ProgramForm.Instance.ProgramDic)
                        {
                            foreach (TreeNode item2 in item.Value.TreeView.Nodes)
                            {
                                TreeNode node;
                                GetEditeNode(item2, item.Key, this._viewConfigParam.TeachNode, out node);
                                if (node != null)
                                {
                                    isSucess = true;
                                    item.Value.treeView1_Edite(this, node, item.Key);
                                    return;
                                }
                            }
                        }
                        if (!isSucess)
                            new UserMessageForm().ShowDialog("未获取到可执行节点!!");
                        break;
                    case nameof(this.编辑toolStripButton):
                        isSucess = false;
                        foreach (KeyValuePair<string, TreeViewWrapClass> item in ProgramForm.Instance.ProgramDic)
                        {
                            foreach (TreeNode item2 in item.Value.TreeView.Nodes)
                            {
                                TreeNode node = null;
                                GetEditeNode(item2, item.Key, this._viewConfigParam.ProgramNode, out node);
                                if (node != null)
                                {
                                    isSucess = true;
                                    item.Value.treeView1_Edite(this, node, item.Key, this.drawObject.BackImage);
                                    return;
                                }
                            }
                        }
                        if (!isSucess)
                            new UserMessageForm().ShowDialog("未获取到可执行节点!!");
                        break;
                    case nameof(this.对位toolStripButton):
                        isSucess = false;
                        foreach (KeyValuePair<string, TreeViewWrapClass> item in ProgramForm.Instance.ProgramDic)
                        {
                            foreach (TreeNode item2 in item.Value.TreeView.Nodes)
                            {
                                TreeNode node = null;
                                GetEditeNode(item2, item.Key, this._viewConfigParam.AlignNode, out node);
                                if (node != null)
                                {
                                    isSucess = true;
                                    item.Value.treeView1_Edite(this, node, item.Key);
                                    return;
                                }
                            }
                        }
                        if (!isSucess)
                            new UserMessageForm().ShowDialog("未获取到可执行节点!!");
                        break;

                    case nameof(this.相机toolStripDropDownButton):

                        break;
                    case nameof(this.Roi绘制toolStripDropDownButton):

                        break;
                    default:
                        new UserMessageForm().ShowDialog("该操作未实现!!!");
                        break;
                }
            }
            catch (Exception ex)
            {
                new UserMessageForm().ShowDialog(e.ClickedItem.Name + "运行失败!" + ex.ToString());
            }
        }

        /// <summary>
        /// 绘图按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Roi绘制toolStripDropDownButton_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            try
            {
                switch (e.ClickedItem.Name)
                {
                    case nameof(this.绘制矩形toolStripMenuItem):
                        userPixRectangle1 rectangle1;
                        this.drawObject.DrawPixRect1OnWindow(enColor.red, out rectangle1);
                        this._ROI = rectangle1;
                        break;
                    case nameof(this.绘制圆形ToolStripMenuItem):
                        userPixCircle pixCircle;
                        this.drawObject.DrawPixCircleOnWindow(enColor.red, out pixCircle);
                        this._ROI = pixCircle;
                        break;
                }
            }
            catch (Exception ex)
            {
                new UserMessageForm(ex.ToString()).ShowDialog();
            }

        }

        /// <summary>
        /// 下拉菜单项
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 相机toolStripDropDownButton_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            try
            {
                switch (e.ClickedItem.Name)
                {

                    case nameof(this.设置曝光toolStripMenuItem):
                        if (AcqSourceManage.Instance.GetAcqSource(this._viewConfigParam.CamName) == null || AcqSourceManage.Instance.GetAcqSource(this._viewConfigParam.CamName).Sensor == null) return;
                        string value = AcqSourceManage.Instance.GetAcqSource(this._viewConfigParam.CamName).Sensor.GetParam("曝光").ToString();
                        RenameForm renameForm = new RenameForm(value);
                        renameForm.ShowDialog();
                        AcqSourceManage.Instance.GetAcqSource(this._viewConfigParam.CamName).Sensor.SetParam("曝光", renameForm.ReName);
                        break;
                    case nameof(this.设置增益ToolStripMenuItem):
                        if (AcqSourceManage.Instance.GetAcqSource(this._viewConfigParam.CamName) == null || AcqSourceManage.Instance.GetAcqSource(this._viewConfigParam.CamName).Sensor == null) return;
                        value = AcqSourceManage.Instance.GetAcqSource(this._viewConfigParam.CamName).Sensor.GetParam("增益").ToString();
                        renameForm = new RenameForm(value);
                        renameForm.ShowDialog();
                        AcqSourceManage.Instance.GetAcqSource(this._viewConfigParam.CamName).Sensor.SetParam("增益", renameForm.ReName);
                        break;
                }
            }
            catch (Exception ex)
            {
                new UserMessageForm(ex.ToString()).ShowDialog();
            }
        }


        private void 补偿checkBox_Click(object sender, EventArgs e)
        {
            try
            {
                CompensateForm form = new CompensateForm(this._param, this._viewConfigParam); //.Show();
                form.Show();
            }
            catch (Exception ex)
            {
                new UserMessageForm(ex.ToString()).ShowDialog();
            }
        }

        private void 管控checkBox_Click(object sender, EventArgs e)
        {
            try
            {
                new SizeControlForm(this.ResultInfoList, this.OcrResultInfoList).Show();
            }
            catch (Exception ex)
            {
                new UserMessageForm(ex.ToString()).ShowDialog();
            }
        }

        private TreeNode GetBindingNode(enBindingNodeName bindingNode)
        {
            TreeNode node = null;
            foreach (KeyValuePair<string, TreeViewWrapClass> item in ProgramForm.Instance.ProgramDic)
            {
                if (node != null) break;
                foreach (TreeNode item2 in item.Value.TreeView.Nodes)
                {
                    if (node != null) break;
                    switch (bindingNode)
                    {
                        case enBindingNodeName.程序节点:
                            GetEditeNode(item2, item.Key, this._viewConfigParam.ProgramNode, out node);
                            break;
                        case enBindingNodeName.程序附属节点1:
                            GetEditeNode(item2, item.Key, this._viewConfigParam.ProgramAttachNode1, out node);
                            break;
                        case enBindingNodeName.程序附属节点2:
                            GetEditeNode(item2, item.Key, this._viewConfigParam.ProgramAttachNode2, out node);
                            break;
                        case enBindingNodeName.对位节点:
                            GetEditeNode(item2, item.Key, this._viewConfigParam.AlignNode, out node);
                            break;
                        case enBindingNodeName.示教节点:
                            GetEditeNode(item2, item.Key, this._viewConfigParam.TeachNode, out node);
                            break;
                        default:
                            return node;
                    }
                }
            }
            return node;
        }

        public userWcsLine ExtendLine(userWcsLine wcsLines)
        {
            if (wcsLines == null)
            {
                throw new ArgumentNullException("wcsLines");
            }
            //////////////////////////////////////////////////////////////////
            userWcsLine wcsLine = new userWcsLine();
            double midx = (wcsLines.X2 + wcsLines.X1) * 0.5;
            double midy = (wcsLines.Y2 + wcsLines.Y1) * 0.5;
            double Phi = Math.Atan2(wcsLines.Y2 - wcsLines.Y1, wcsLines.X2 - wcsLines.X1);
            double startPhi = Math.Atan2(wcsLines.Y1 - midy, wcsLines.X1 - midx);
            double endPhi = Math.Atan2(wcsLines.Y2 - midy, wcsLines.X2 - midx);
            //// 计算延长的端点  
            wcsLine.X1 = wcsLines.X1 + 10000 * Math.Cos(startPhi);// row[0].D;
            wcsLine.Y1 = wcsLines.Y1 + 10000 * Math.Sin(startPhi);//col[0].D;
            wcsLine.X2 = wcsLines.X2 + 10000 * Math.Cos(endPhi);//row[1].D;
            wcsLine.Y2 = wcsLines.Y2 + 10000 * Math.Sin(endPhi);//col[1].D;
            /////////////////////////////////////////////////
            wcsLine.Grab_x = wcsLines.Grab_x;
            wcsLine.Grab_y = wcsLines.Grab_y;
            wcsLine.Grab_theta = wcsLines.Grab_theta;
            wcsLine.CamName = wcsLines.CamName;
            wcsLine.ViewWindow = wcsLines.ViewWindow;
            wcsLine.CamParams = wcsLines.CamParams;
            wcsLine.Tag = wcsLines.Tag;
            return wcsLine;
        }

        private void ShowLable(MeasureResultInfo info, int index, enFlag flag)
        {
            object readContent = "";
            switch (flag)
            {
                default:
                case enFlag.NONE:
                case enFlag.测量值:
                    readContent = info.Mea_Value;
                    break;
                case enFlag.标准值:
                    readContent = info.Std_Value;
                    break;
                case enFlag.上偏差:
                    readContent = info.LimitUp;
                    break;
                case enFlag.下偏差:
                    readContent = info.LimitDown;
                    break;
                case enFlag.结果:
                    readContent = info.State;
                    break;
                case enFlag.OK_NG:
                case enFlag.JudgeData:
                case enFlag.Int1_2:
                case enFlag.测量值_结果:
                    readContent = string.Join(",", info.Mea_Value, info.State);
                    break;
                case enFlag.测量值_标准值:
                    readContent = string.Join(",", info.Mea_Value, info.Std_Value);
                    break;
                case enFlag.测量值_标准值_结果:
                    readContent = string.Join(",", info.Mea_Value, info.Std_Value, info.State);
                    break;
                case enFlag.测量值_标准值_上偏差:
                    readContent = string.Join(",", info.Mea_Value, info.Std_Value, info.LimitUp);
                    break;
                case enFlag.测量值_标准值_上偏差_下偏差_结果:
                    readContent = string.Join(",", info.Mea_Value, info.Std_Value, info.LimitUp, info.LimitDown, info.State);
                    break;
                case enFlag.测量值_标准值_上偏差_结果:
                    readContent = string.Join(",", info.Mea_Value, info.Std_Value, info.LimitUp, info.State);
                    break;
                case enFlag.测量值_标准值_下偏差_结果:
                    readContent = string.Join(",", info.Mea_Value, info.Std_Value, info.LimitDown, info.State);
                    break;
                case enFlag.数据标签:
                    //oo = string.Join(",", info.Mea_Value, info.Std_Value, info.LimitUp, info.LimitDown, info.State);
                    break;
            }
            /////////////////////
            if (readContent != null)
            {
                //item.WriteValue = readContent.ToString();
                //string[] desValue = item?.Describe?.Split(',', ';', '，');
                //string[] readValue = readContent.ToString().Split(',', ';', ':', '，');
                //string content = "";
                //for (int i = 0; i < readValue.Length; i++)
                //{
                //    if (desValue != null && desValue.Length > i)
                //    {
                //        content += desValue[i] + readValue[i] + " ";
                //    }
                //    else
                //    {
                //        content += readValue[i] + " ";
                //    }
                //}
                //dic.Add(item.DataSource, content);
            }


        }


        /// <summary>
        /// 接收图像生成事件，用于外部触发的飞拍场合
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ImageAcqComplete_Event(object sender, ImageAcqCompleteEventArgs e)
        {
            try
            {
                if (!this._viewConfigParam.IsNodeBindingExcute) return; // 该变量用于异步取图的场景
                if (AcqSourceManage.Instance.GetAcqSource(this._viewConfigParam.CamName) == null) return; // 如果没有采集源，返回
                enCoordSysName coordSysName = AcqSourceManage.Instance.GetAcqSource(this._viewConfigParam.CamName).CoordSysName;
                string ProductID_s = CommunicationConfigParamManger.Instance.ReadValue(coordSysName, enCommunicationCommand.ProductID).ToString();
                string GrabNo_s = CommunicationConfigParamManger.Instance.ReadValue(coordSysName, enCommunicationCommand.GrabNo).ToString();
                int GrabNo = 0;
                int.TryParse(GrabNo_s, out GrabNo);
                switch (this._viewConfigParam.GrabNo)  // 比较工位的模式
                {
                    case "奇数工位":
                        int result = 0;
                        Math.DivRem(GrabNo, 2, out result);
                        if (result != 1) return;
                        break;
                    case "偶数工位":
                        result = 0;
                        Math.DivRem(GrabNo, 2, out result);
                        if (result != 0) return;
                        break;
                    default:
                        if (this._viewConfigParam.GrabNo.Trim() != "0" && (this._viewConfigParam.GrabNo.Trim() != GrabNo.ToString())) return;
                        break;
                }
                //////////   开始处理图像   /////////////
                this.drawObject.ClearViewObject(); // 更新图像时清空绘图对象 
                this.drawObject.BackImage = e.ImageData.Clone();
                this.drawObject.BackImage.ViewWindow = this._viewConfigParam.ViewName;
                this.drawObject.BackImage.Tag = GrabNo;
                this.drawObject.BackImage.ProductID = ProductID_s;
                TreeNode node = GetBindingNode(enBindingNodeName.程序节点);
                if (node != null)
                {
                    IFunction item = node.Tag as IFunction;
                    Task.Run(() => item?.Execute(node, this.drawObject.BackImage)); // 在这里传入图像
                }
                else
                {
                    LoggerHelper.Error("没有绑定程序节点!");
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.Error("图像回调函数执行失败!" + ex.ToString());
            }

        }

        /// <summary>
        /// 这里用于实时采集模式，单次获取图像的场景，图像回调函数用于外触发场景
        /// </summary>
        /// <param name="send"></param>
        /// <param name="e"></param>
        private void WaiteTriggerSingle(object send, PoseInfoEventArgs e)
        {
            if (!this._viewConfigParam.EnableTriggerSingleBinding) return;  // 如果没有绑定触发信号，将返回
            if (AcqSourceManage.Instance.GetAcqSource(this._viewConfigParam.CamName)?.CoordSysName != e.CoordSysName) return;  // 如果坐标系不相等，将返回
            switch (this._viewConfigParam.GrabNo)  // 比较工位的模式
            {
                case "奇数工位":
                    int result = 0;
                    Math.DivRem(e.GrabNo, 2, out result);
                    if (result != 1) return;
                    break;
                case "偶数工位":
                    result = 0;
                    Math.DivRem(e.GrabNo, 2, out result);
                    if (result != 0) return;
                    break;
                default:
                    if (this._viewConfigParam.GrabNo != e.GrabNo.ToString()) return;
                    break;
            }
            ///////  根据触发源来决定怎么处理 
            switch (AcqSourceManage.Instance.GetAcqSource(this._viewConfigParam.CamName).Sensor.CameraParam.TriggerSource)
            {
                case enUserTriggerSource.NONE:
                    TreeNode node = GetBindingNode(enBindingNodeName.程序节点);
                    TreeNode attachNode = GetBindingNode(enBindingNodeName.程序附属节点1);
                    TreeNode attachNode2 = GetBindingNode(enBindingNodeName.程序附属节点2);
                    Task.Run(() =>
                    {
                        IFunction item = null;
                        // 执行主节点
                        if (node != null)
                        {
                            item = node.Tag as IFunction;
                            item?.Execute(node, "GrabNo=" + e.GrabNo, "ViewName=" + this._viewConfigParam.ViewName, "ProductID=" + e.ProductID); // 在这里传入图像
                        }
                        /// 执行附属节点1
                        if (attachNode != null)
                        {
                            item = attachNode.Tag as IFunction;
                            item?.Execute(attachNode, "GrabNo=" + e.GrabNo, "ViewName=" + this._viewConfigParam.ViewName, "ProductID=" + e.ProductID); // 在这里传入图像
                        }
                        /// 执行附属节点2
                        if (attachNode2 != null)
                        {
                            item = attachNode2.Tag as IFunction;
                            item?.Execute(attachNode2, "GrabNo=" + e.GrabNo, "ViewName=" + this._viewConfigParam.ViewName, "ProductID=" + e.ProductID); // 在这里传入图像
                        }
                    });
                    break;
                case enUserTriggerSource.编码器触发:
                case enUserTriggerSource.外部IO触发:
                case enUserTriggerSource.软触发:
                    switch (e.PoseInfo)
                    {
                        case "start":
                            AcqSourceManage.Instance.GetAcqSource(this._viewConfigParam.CamName).Sensor.StopTrigger();
                            AcqSourceManage.Instance.GetAcqSource(this._viewConfigParam.CamName).Sensor.StartTrigger();
                            break;
                        case "end":
                            AcqSourceManage.Instance.GetAcqSource(this._viewConfigParam.CamName).Sensor.StopTrigger();
                            break;
                    }
                    break;
                default:
                    break;
            }


        }




    }




}

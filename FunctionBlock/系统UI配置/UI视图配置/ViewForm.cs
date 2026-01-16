
using Common;
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
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using View;

namespace FunctionBlock
{
    public partial class ViewForm : Form
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
        private ZoneCompensationParam _zoneParam = null;
        private RectifyCalculateParam _rectifyParam = null;
        private ZoneRectifyParam _zoneRectifyeParam = null;
        private PixROI _pixROI = null;
        private WcsROI _wcsROI = null;
        private System.Threading.Timer HeartTimer;
        private SocketBase _socket;
        private ISensor _sensor = null;
        private AutoResetEvent _triggerSingleEvent;
        private ManualResetEventSlim _manualEvent;
        private Stopwatch stopwatch = new Stopwatch();
        private int _imageIndex = 0;
        private double _curRow = 0, _curCol = 0;
        //private bool _triggerFlag = false;  

        public ViewForm(ViewConfigParam viewConfigParam)
        {
            InitializeComponent();
            /////////////
            this._triggerSingleEvent = new AutoResetEvent(false);
            this._manualEvent = new ManualResetEventSlim(false);
            //this.HeartTimer = new System.Threading.Timer(this.HeartCallback, "Test", 2000, 10000);
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
            /////////////////////////////////////////////////////////////////////////////////////////////////
            //UserLoginParamManager.Instance.LoginParam.UserChange += new EventHandler(this.UserChange_Event);
            UserLoginParamManager.Instance.UserChange += new EventHandler(this.UserChange_Event);
            /////////////////// 绑定触发信号事件
            AutoRunThreadPlc.Instance.TriggerInfo += new PoseInfoEventHandler(this.WaitTriggerSingle);
            this._sensor = AcqSourceManage.Instance.GetAcqSource(this._viewConfigParam.CamName)?.Sensor;
            if (this._sensor != null)
                this._sensor.ImageAcqComplete += new ImageAcqCompleteEventHandler(ImageAcqCompleteRFID_Event);
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
            //if (this._socket != null)
            //    this._socket.SocketMessage += new SocketMessageEventHandler(this.ViewForm_SocketMessage);
        }
        private void ViewForm_Load(object sender, EventArgs e)
        {
            //HOperatorSet.SetWindowAttr("background_color", "sky blue");
            ///////////////////////////////////////
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
                new Common.UserMessageForm(ex.ToString()).ShowDialog();
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
                        this.程序节点comboBox.Enabled = true;
                        this.buttonClose.Enabled = false;
                        //this.标定工具栏toolStrip.Enabled = false;
                        this.执行toolStripButton.Enabled = true;
                        //this.标定配置toolStripButton.Enabled = false;
                        //this.示教toolStripButton.Enabled = false;
                        this.相机toolStripDropDownButton.Enabled = true;
                        this.标定toolStripDropDownButton.Enabled = false;
                        //this.示教toolStripButton.Enabled = false;
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
                        //this.示教toolStripButton.Enabled = true;
                        this.相机toolStripDropDownButton.Enabled = true;
                        this.标定toolStripDropDownButton.Enabled = true;
                        //this.示教toolStripButton.Enabled = true;
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
                        //this.示教toolStripButton.Enabled = true;
                        this.相机toolStripDropDownButton.Enabled = true;
                        this.标定toolStripDropDownButton.Enabled = true;
                        //this.示教toolStripButton.Enabled = true;
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
                        this.drawObject.BackImage?.Dispose();
                        this.drawObject.BackImage = ((ImageDataClass)e.DataContent).Clone();
                        if (this._viewConfigParam.IsShowCross)
                            this.drawObject.AddViewObject(new ViewData(ScaleParamForm.GetCrossIcon(this.drawObject.CameraParam, this.drawObject.BackImage.Width, this.drawObject.BackImage.Height), ScaleParamManager.Instance.Param.Color));
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
                    case nameof(userWcsPolygon):
                        userWcsPolygon wcsPolygon = (userWcsPolygon)e.DataContent;
                        this.drawObject.AddViewObject(new ViewData(wcsPolygon.GetPixPolygon().GetXLD(), wcsPolygon.Color.ToString()));
                        break;
                    case nameof(userWcsArrow):
                        userWcsArrow wcsArrow = (userWcsArrow)e.DataContent;
                        this.drawObject.AddViewObject(new ViewData((wcsArrow).GetPixArrow(), wcsArrow.Color.ToString()));
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
                            this.管控checkBox.Enabled = true;
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
                    case nameof(ZoneCompensationParam):
                        this.Invoke(new Action(() =>
                        {
                            this.补偿checkBox.Enabled = true;
                            this._zoneParam = ((ZoneCompensationParam)e.DataContent);
                        }));   // 如果这个窗口接收到了补偿值，那么该窗口对应的按钮将显示  
                        break;
                    case nameof(RectifyCalculateParam):
                        this.Invoke(new Action(() =>
                        {
                            this.补偿checkBox.Enabled = true;
                            this._rectifyParam = ((RectifyCalculateParam)e.DataContent);
                        }));   // 如果这个窗口接收到了补偿值，那么该窗口对应的按钮将显示  
                        break;
                    case nameof(ZoneRectifyParam):
                        this.Invoke(new Action(() =>
                        {
                            this.补偿checkBox.Enabled = true;
                            this._zoneRectifyeParam = ((ZoneRectifyParam)e.DataContent);
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
                if (AcqSourceManage.Instance.GetAcqSource(this._viewConfigParam.CamName)?.Sensor != null)
                    AcqSourceManage.Instance.GetAcqSource(this._viewConfigParam.CamName).Sensor.ImageAcqComplete -= new ImageAcqCompleteEventHandler(ImageAcqCompleteRFID_Event);
                BaseFunction.ExcuteCompleted -= new ExcuteCompletedEventHandler(DisplayExcuteResult);
                this.drawObject.GrayValueInfo -= new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                this.drawObject.HMouseDoubleClick -= new HMouseEventHandler(this.hWindowControl1_DoubleClick);
                UserLoginParamManager.Instance.UserChange -= new EventHandler(this.UserChange_Event);
                AutoRunThreadPlc.Instance.TriggerInfo -= new PoseInfoEventHandler(this.WaitTriggerSingle);
                //if (this._socket != null)
                //    this._socket.SocketMessage -= new SocketMessageEventHandler(this.ViewForm_SocketMessage);
                if (this.drawObject != null)
                    this.drawObject.ClearDrawingObject();
                this.HeartTimer?.Dispose();
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
            DialogResult dialogResult = new UserMessageForm("确定关闭窗体吗？", "关闭窗体").ShowDialog();
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
                //Point point = this.Parent.Location;
                this._viewConfigParam.Location = this.Parent.Location;
                this._viewConfigParam.FormSize = this.Parent.Size;
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
        private void 实时采集checkBox_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                switch (this.实时采集checkBox.CheckState)
                {
                    case CheckState.Checked:
                        this.实时采集checkBox.BackColor = Color.Red;
                        //this.drawObject.AttachPropertyData.Clear();
                        //this.drawObject.AttachPropertyData.Add(new ViewData(ScaleParamForm.GetCrossIcon(this.drawObject.CameraParam, this.drawObject.BackImage.Width, this.drawObject.BackImage.Height), ScaleParamManager.Instance.Param.Color));
                        //return;
                        //this.acqSource = AcqSourceManage.Instance.GetCamAcqSource(this._viewConfigParam.CamName);
                        //if (this.acqSource == null) return;
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
                            ///////////////////////
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
                                        stopwatch.Restart();
                                        //object trigMode = this._sensor?.GetParam(enSocketInfo.获取触发模式);
                                        //if (trigMode.ToString() == "On")
                                        this._sensor.SetParam("实时采集", 0);
                                        data = this._sensor.ReadData();// this.acqSource.AcqImageData(null);
                                        if (data?.Count > 0)
                                        {
                                            this.drawObject.BackImage = (ImageDataClass)data[enDataItem.Image];
                                            this.drawObject.AttachPropertyData.Clear();
                                            this.drawObject.AttachPropertyData.Add(new ViewData(ScaleParamForm.GetCrossIcon(this.drawObject.CameraParam, this.drawObject.BackImage.Width, this.drawObject.BackImage.Height), ScaleParamManager.Instance.Param.Color)); //(this.GenCrossLine2(this.drawObject.BackImage?.Image))
                                        }
                                        stopwatch.Stop();
                                        long time = stopwatch.ElapsedMilliseconds;
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
                            this._sensor?.SetParam("停止采集", 0);
                        }
                        /// 关闭光源
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
                new Common.UserMessageForm(ex.ToString()).ShowDialog();
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
                     new ToolStripMenuItem("移动到当前位",null,null,"移动到当前位"),
                     new ToolStripMenuItem("发送当前坐标",null,null,"发送当前坐标"),
                     //new ToolStripMenuItem("执行程序",null,null,"执行程序"),
                     new ToolStripMenuItem("配置程序节点",null,null,"配置程序节点"),
                     //new ToolStripMenuItem("编辑程序节点",null,null,"编辑程序节点"),
                     //new ToolStripMenuItem("编辑示教节点",null,null,"编辑示教节点"),
                     //new ToolStripMenuItem("编辑对位节点",null,null,"编辑对位节点"),
                     //new ToolStripMenuItem("设置曝光",null,null,"设置曝光"),
                     //new ToolStripMenuItem("设置增益",null,null,"设置增益"),
                     //new ToolStripMenuItem("设置光源亮度",null,null,"设置光源亮度"),
                     new ToolStripMenuItem("十字线",null,null,"十字线"),
                     new ToolStripMenuItem("启用运行实时",null,null,"启用运行实时"),
                     new ToolStripMenuItem("显示延长直线",null,null,"显示延长直线"),
                     new ToolStripMenuItem("启用绑定节点执行",null,null,"启用绑定节点执行"),
                     new ToolStripMenuItem("启用触发信号绑定",null,null,"启用触发信号绑定"),
                     new ToolStripMenuItem("启用Socket信号绑定",null,null,"启用Socket信号绑定"),
                     //new ToolStripMenuItem("相机标定",null,null,"相机标定"),
                     //new ToolStripMenuItem("相机映射标定",null,null,"相机映射标定"),
                     new ToolStripMenuItem("尺寸管控",null,null,"尺寸管控"),
                     new ToolStripMenuItem("对位补偿设置",null,null,"对位补偿设置"),
                     new ToolStripMenuItem("设置相机参数",null,null,"设置相机参数"),
                     new ToolStripMenuItem("3D(View)",null,null,"3D(View)"),
                     new ToolStripMenuItem("保存图像",null,null,"保存图像"),
                     new ToolStripMenuItem("保存点云",null,null,"保存点云"),
                     new ToolStripMenuItem("加载图像",null,null,"加载图像"),
                     new ToolStripMenuItem("清除窗口(Clear)",null,null,"清除窗口(Clear)"),
                     new ToolStripMenuItem("设置查找表(Lut)",null,null,"设置查找表(Lut)"),
                     new ToolStripMenuItem("设置光标参数",null,null,"设置光标参数"),
                    };
                    if (this._viewConfigParam.IsShowCross)
                        items[4].Text = "隐藏十字线";
                    else
                        items[4].Text = "显示十字线";
                    if (this._viewConfigParam.IsRunLiveTime)
                        items[5].Text = "禁用运行实时";
                    else
                        items[5].Text = "启用运行实时";
                    if (this._viewConfigParam.IsExtendLine)
                        items[6].Text = "隐藏延长直线";
                    else
                        items[6].Text = "显示延长直线";
                    if (this._viewConfigParam.IsNodeBindingExcute)
                        items[7].Text = "禁用绑定节点执行";
                    else
                        items[7].Text = "启用绑定节点执行";
                    if (this._viewConfigParam.EnableTriggerSingleBinding)
                        items[8].Text = "禁用触发信号绑定";
                    else
                        items[8].Text = "启用触发信号绑定";
                    if (this._viewConfigParam.EnableSocketSingleBinding)
                        items[9].Text = "禁用Socket信号绑定";
                    else
                        items[9].Text = "启用Socket信号绑定";
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
                     new ToolStripMenuItem("启用Socket信号绑定",null,null,"启用Socket信号绑定"),
                     new ToolStripMenuItem("Move Position",null,null,"移动到当前位"),
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
                    if (this._viewConfigParam.EnableSocketSingleBinding)
                        items[14].Text = "Disable Socket Single Binding";
                    else
                        items[14].Text = "Enable Socket Single Binding";
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
                            case enUserName.管理员:
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
                                ///////////////////////////////////////////////////////////////////////////////////
                                //if (node != null)
                                //{
                                //    isSucess = true;
                                //    item.Value.treeView1_Edite(this, node, item.Key, this.drawObject.BackImage);
                                //    return;
                                //}
                                if (node != null)
                                {
                                    isSucess = true;
                                    if (this._socket != null && this._socket.Type == enSocketType.客户端) // 如果相机连接类型是Socket，那么编辑时将从 Socket 网络获取图像
                                    {
                                        SocketMessage message = new SocketMessage();
                                        message.Lable = enSocketInfo.获取窗口图像;
                                        message.Name = this._sensor.Name;
                                        message.ViewName = this._viewConfigParam.ViewName;
                                        object mesgContent = this._socket.GetDataAsync(message, true);
                                        message = mesgContent as SocketMessage;
                                        item.Value.treeView1_Edite(this, node, item.Key, message.MesContent as ImageDataClass);
                                    }
                                    else
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
                                //if (node != null)
                                //{
                                //    isSucess = true;
                                //    item.Value.treeView1_Edite(this, node, item.Key);
                                //    return;
                                //}
                                if (node != null)
                                {
                                    isSucess = true;
                                    if (this._socket != null && this._socket.Type == enSocketType.客户端) // 如果相机连接类型是Socket，那么编辑时将从 Socket 网络获取图像
                                    {
                                        SocketMessage message = new SocketMessage();
                                        message.Lable = enSocketInfo.获取窗口图像;
                                        message.Name = this._sensor.Name;
                                        message.ViewName = this._viewConfigParam.ViewName;
                                        object mesgContent = this._socket.GetDataAsync(message, true);
                                        message = mesgContent as SocketMessage;
                                        item.Value.treeView1_Edite(this, node, item.Key, message.MesContent as ImageDataClass);
                                    }
                                    else
                                        item.Value.treeView1_Edite(this, node, item.Key, this.drawObject.BackImage);
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
                                //if (node != null)
                                //{
                                //    isSucess = true;
                                //    item.Value.treeView1_Edite(this, node, item.Key);
                                //    return;
                                //}
                                if (node != null)
                                {
                                    isSucess = true;
                                    if (this._socket != null && this._socket.Type == enSocketType.客户端) // 如果相机连接类型是Socket，那么编辑时将从 Socket 网络获取图像
                                    {
                                        SocketMessage message = new SocketMessage();
                                        message.Lable = enSocketInfo.获取窗口图像;
                                        message.Name = this._sensor.Name;
                                        message.ViewName = this._viewConfigParam.ViewName;
                                        object mesgContent = this._socket.GetDataAsync(message, true);
                                        message = mesgContent as SocketMessage;
                                        item.Value.treeView1_Edite(this, node, item.Key, message.MesContent as ImageDataClass);
                                    }
                                    else
                                        item.Value.treeView1_Edite(this, node, item.Key, this.drawObject.BackImage);
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
                        RenameForm renameForm = new RenameForm(value, "设置曝光");
                        renameForm.StartPosition = FormStartPosition.CenterScreen;
                        if (renameForm.ShowDialog() == DialogResult.OK)
                        {
                            AcqSourceManage.Instance.GetAcqSource(this._viewConfigParam.CamName).Sensor.SetParam("曝光", renameForm.ReName);
                        }
                        break;
                    case "设置增益":
                    case "设置增溢":
                        if (AcqSourceManage.Instance.GetAcqSource(this._viewConfigParam.CamName) == null || AcqSourceManage.Instance.GetAcqSource(this._viewConfigParam.CamName).Sensor == null) return;
                        value = AcqSourceManage.Instance.GetAcqSource(this._viewConfigParam.CamName).Sensor.GetParam("增益").ToString();
                        renameForm = new RenameForm(value, "设置增溢");
                        renameForm.StartPosition = FormStartPosition.CenterScreen;
                        if (renameForm.ShowDialog() == DialogResult.OK)
                        {
                            AcqSourceManage.Instance.GetAcqSource(this._viewConfigParam.CamName).Sensor.SetParam("增益", renameForm.ReName);
                        }
                        break;
                    case "设置光源亮度":
                        if (AcqSourceManage.Instance.GetAcqSource(this._viewConfigParam.CamName) == null) return;
                        LightSetForm lightForm = new LightSetForm(AcqSourceManage.Instance.GetAcqSource(this._viewConfigParam.CamName));
                        //LightControlForm controlForm = new LightControlForm(AcqSourceManage.Instance.GetAcqSource(this._viewConfigParam.CamName));
                        lightForm.Show();
                        break; //设置光标参数
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
                    case "启用Socket信号绑定":
                    case "禁用Socket信号绑定":
                        if (this._viewConfigParam.EnableSocketSingleBinding)
                        {
                            e.ClickedItem.Text = "启用Socket信号绑定";
                            this._viewConfigParam.EnableSocketSingleBinding = false;
                        }
                        else
                        {
                            e.ClickedItem.Text = "禁用Socket信号绑定";
                            this._viewConfigParam.EnableSocketSingleBinding = true;
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
                        //if (this._zoneParam != null && this._zoneParam.IsZoneCompensation)
                        //    new ZoneCompensationForm(this._zoneParam).Show();
                        //else
                        new CompensateForm(this._param, this._viewConfigParam).Show();
                        break;
                    case "设置相机参数":
                        new CameraParamForm(AcqSourceManage.Instance.GetAcqSource(this._viewConfigParam.CamName).Sensor.CameraParam).Show();
                        break;

                    case "读取图像":
                    case "加载图像":
                        OpenFileDialog ofd = new OpenFileDialog();
                        ofd.Filter = "bmp文件(*.bmp)|*.bmp|hobj文件(*.hobj)|*.hobj|tiff文件(*.tiff)|*.tiff|jpg文件(*.jpg)|*.jpg|jpeg文件(*.jpeg)|*.jpeg|png文件(*.png)|*.png|ras文件(*.ras)|*.ras|dxf文件(.dxf)|*.dxf|hdev文件(.hdev)|*.hdev|所有文件(*.*)|*.**";
                        ofd.RestoreDirectory = false;
                        ofd.FilterIndex = 0;
                        if (ofd.ShowDialog() == DialogResult.OK)
                        {
                            this.drawObject.BackImage = new ImageDataClass(new HImage(ofd.FileName), AcqSourceManage.Instance.GetAcqSource(this._viewConfigParam.CamName)?.Sensor?.CameraParam); //, this._acqSource.Sensor.CameraParam
                            this.drawObject.BackImage.ViewWindow = this._viewConfigParam.ViewName;
                            this.drawObject.BackImage.Tag = 1;
                        }
                        break;

                    case "移动到当前位":
                        enCoordSysName coordSysName = AcqSourceManage.Instance.GetAcqSource(this.传感器comboBox1.SelectedItem.ToString()).CoordSysName;
                        CoordSysAxisPosParam axisParam = new CoordSysAxisPosParam();
                        axisParam.UpdataAxisPosition(coordSysName); // 实时使用当前位置
                        if (this.drawObject.BackImage == null || this.传感器comboBox1.SelectedItem.ToString() == "NONE")
                        {
                            LoggerHelper.Info(this._viewConfigParam.ViewName + "->执行移动到当前位功能时,没有采集图像或传感器名称为空", AcqSourceManage.Instance.GetAcqSource(this.传感器comboBox1.SelectedItem.ToString())?.Sensor?.Name);
                            CommunicationConfigParamManger.Instance.WriteValue(coordSysName, enCommunicationCommand.FunctionNoToPlc, "Move");
                            CommunicationConfigParamManger.Instance.WriteValue(coordSysName, enCommunicationCommand.ResultToSocket, "NG");
                            CommunicationConfigParamManger.Instance.WriteValue(coordSysName, enCommunicationCommand.TriggerToSocket, 1);
                            CommunicationConfigParamManger.Instance.WriteValue(coordSysName, enCommunicationCommand.TriggerToPlc, 1);
                            return;
                        }
                        double x, y, z;
                        this.drawObject.CameraParam.ImagePointsToWorldPlane(this._curRow, this._curCol, axisParam.X, axisParam.Y, 0, out x, out y, out z);
                        CommunicationConfigParamManger.Instance.WriteValue(coordSysName, enCommunicationCommand.Compensation_X, x);
                        CommunicationConfigParamManger.Instance.WriteValue(coordSysName, enCommunicationCommand.Compensation_Y, y);
                        //CommunicationConfigParamManger.Instance.WriteValue(coordSysName, enCommunicationCommand.Compensation_Z, this.axisParam.Z);
                        CommunicationConfigParamManger.Instance.WriteValue(coordSysName, enCommunicationCommand.FunctionNoToPlc, "Move");
                        CommunicationConfigParamManger.Instance.WriteValue(coordSysName, enCommunicationCommand.ResultToSocket, "OK");
                        CommunicationConfigParamManger.Instance.WriteValue(coordSysName, enCommunicationCommand.TriggerToSocket, 1);
                        CommunicationConfigParamManger.Instance.WriteValue(coordSysName, enCommunicationCommand.TriggerToPlc, 1);
                        LoggerHelper.Info(this._viewConfigParam.ViewName + "->功能位置写入:Move", AcqSourceManage.Instance.GetAcqSource(this.传感器comboBox1.SelectedItem.ToString())?.Sensor?.Name);
                        LoggerHelper.Info(this._viewConfigParam.ViewName + $"->移动到目标位置:x={Math.Round(x, 5)},y={Math.Round(y, 5)}", AcqSourceManage.Instance.GetAcqSource(this.传感器comboBox1.SelectedItem.ToString())?.Sensor?.Name);
                        break;

                    case "发送当前坐标":
                        coordSysName = AcqSourceManage.Instance.GetAcqSource(this.传感器comboBox1.SelectedItem.ToString()).CoordSysName;
                        switch (this._wcsROI?.GetType().Name)
                        {
                            case nameof(drawWcsCircle):
                                drawWcsCircle wcsCircle = this._wcsROI as drawWcsCircle;
                                CommunicationConfigParamManger.Instance.WriteValue(coordSysName, enCommunicationCommand.Compensation_X, wcsCircle.X);
                                CommunicationConfigParamManger.Instance.WriteValue(coordSysName, enCommunicationCommand.Compensation_Y, wcsCircle.Y);
                                CommunicationConfigParamManger.Instance.WriteValue(coordSysName, enCommunicationCommand.ResultToSocket, "OK");
                                CommunicationConfigParamManger.Instance.WriteValue(coordSysName, enCommunicationCommand.ResultToPlc, 1);
                                CommunicationConfigParamManger.Instance.WriteValue(coordSysName, enCommunicationCommand.TriggerToSocket, 1);
                                CommunicationConfigParamManger.Instance.WriteValue(coordSysName, enCommunicationCommand.TriggerToPlc, 1);
                                LoggerHelper.Info(this._viewConfigParam.ViewName + $"->发送当前位置:x={Math.Round(wcsCircle.X, 5)},y={Math.Round(wcsCircle.Y, 5)}", AcqSourceManage.Instance.GetAcqSource(this.传感器comboBox1.SelectedItem.ToString())?.Sensor?.Name);
                                break;
                            case nameof(drawWcsRect2):
                                drawWcsRect2 wcsRect2 = this._wcsROI as drawWcsRect2;
                                CommunicationConfigParamManger.Instance.WriteValue(coordSysName, enCommunicationCommand.Compensation_X, wcsRect2.X);
                                CommunicationConfigParamManger.Instance.WriteValue(coordSysName, enCommunicationCommand.Compensation_Y, wcsRect2.Y);
                                CommunicationConfigParamManger.Instance.WriteValue(coordSysName, enCommunicationCommand.ResultToSocket, "OK");
                                CommunicationConfigParamManger.Instance.WriteValue(coordSysName, enCommunicationCommand.ResultToPlc, 1);
                                CommunicationConfigParamManger.Instance.WriteValue(coordSysName, enCommunicationCommand.TriggerToSocket, 1);
                                CommunicationConfigParamManger.Instance.WriteValue(coordSysName, enCommunicationCommand.TriggerToPlc, 1);
                                LoggerHelper.Info(this._viewConfigParam.ViewName + $"->发送当前位置:x={Math.Round(wcsRect2.X, 5)},y={Math.Round(wcsRect2.Y, 5)}", AcqSourceManage.Instance.GetAcqSource(this.传感器comboBox1.SelectedItem.ToString())?.Sensor?.Name);
                                break;
                            case nameof(drawWcsPoint):
                                drawWcsPoint wcsPoint = this._wcsROI as drawWcsPoint;
                                CommunicationConfigParamManger.Instance.WriteValue(coordSysName, enCommunicationCommand.Compensation_X, wcsPoint.X);
                                CommunicationConfigParamManger.Instance.WriteValue(coordSysName, enCommunicationCommand.Compensation_Y, wcsPoint.Y);
                                CommunicationConfigParamManger.Instance.WriteValue(coordSysName, enCommunicationCommand.ResultToSocket, "OK");
                                CommunicationConfigParamManger.Instance.WriteValue(coordSysName, enCommunicationCommand.ResultToPlc, 1);
                                CommunicationConfigParamManger.Instance.WriteValue(coordSysName, enCommunicationCommand.TriggerToSocket, 1);
                                CommunicationConfigParamManger.Instance.WriteValue(coordSysName, enCommunicationCommand.TriggerToPlc, 1);
                                LoggerHelper.Info(this._viewConfigParam.ViewName + $"->发送当前位置:x={Math.Round(wcsPoint.X, 5)},y={Math.Round(wcsPoint.Y, 5)}", AcqSourceManage.Instance.GetAcqSource(this.传感器comboBox1.SelectedItem.ToString())?.Sensor?.Name);
                                break;
                            case nameof(drawWcsEllipse):
                                drawWcsEllipse wcsEllipse = this._wcsROI as drawWcsEllipse;
                                CommunicationConfigParamManger.Instance.WriteValue(coordSysName, enCommunicationCommand.Compensation_X, wcsEllipse.X);
                                CommunicationConfigParamManger.Instance.WriteValue(coordSysName, enCommunicationCommand.Compensation_Y, wcsEllipse.Y);
                                CommunicationConfigParamManger.Instance.WriteValue(coordSysName, enCommunicationCommand.ResultToSocket, "OK");
                                CommunicationConfigParamManger.Instance.WriteValue(coordSysName, enCommunicationCommand.ResultToPlc, 1);
                                CommunicationConfigParamManger.Instance.WriteValue(coordSysName, enCommunicationCommand.TriggerToSocket, 1);
                                CommunicationConfigParamManger.Instance.WriteValue(coordSysName, enCommunicationCommand.TriggerToPlc, 1);
                                LoggerHelper.Info(this._viewConfigParam.ViewName + $"->发送当前位置:x={Math.Round(wcsEllipse.X, 5)},y={Math.Round(wcsEllipse.Y, 5)}", AcqSourceManage.Instance.GetAcqSource(this.传感器comboBox1.SelectedItem.ToString())?.Sensor?.Name);
                                break;
                        }
                        break;

                    case "设置光标参数":
                        Point point = this.hWindowControl1.PointToScreen(this.hWindowControl1.Location);  //new Point((int)this._curCol, (int)this._curRow)
                        new ScaleParamForm(point).Show();
                        break; //
                }
            }
            catch (Exception ex)
            {
                new Common.UserMessageForm(ex.ToString()).ShowDialog();
            }
        }
        private void hWindowControl1_MouseMove(object sender, GrayValueInfoEventArgs e)
        {
            if (e.GaryValue.Length > 0)
            {
                this._curRow = e.Row;
                this._curCol = e.Col;
                int row1, col1, row2, col2;
                this.hWindowControl1.HalconWindow.GetPart(out row1, out col1, out row2, out col2);
                this.hWindowControl1.HalconWindow.SetTposition((int)(row1 + (row2 - row1) * 0.025), (int)(col1 + (col2 - col1) * 0.015));
                //this.hWindowControl1.HalconWindow.SetFont("-Consolas-" + 12 + "- *-0-*-*-1-");
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

        private HXLDCont GenCrossLine2(HImage hImage)
        {
            HXLDCont hXLDCont = new HXLDCont();
            if (hImage != null && hImage.IsInitialized())
            {
                hXLDCont.GenEmptyObj();
                int width, height;
                hImage.GetImageSize(out width, out height);
                hXLDCont = hXLDCont.ConcatObj(new HXLDCont(new HTuple(height * 0.5, height * 0.5), new HTuple(0, width)));
                hXLDCont = hXLDCont.ConcatObj(new HXLDCont(new HTuple(0, height), new HTuple(width * 0.5, width * 0.5)));
                //////////////////////////////////////////////////////////////////////////////////
                double step = ScaleParamManager.Instance.Param.StepDist, radisu = ScaleParamManager.Instance.Param.Radius;
                double pixStep = this.drawObject.CameraParam.TransWcsLengthToPixLength(step);
                double pixRadius = this.drawObject.CameraParam.TransWcsLengthToPixLength(radisu);
                double center_row = height * 0.5;
                double center_col = width * 0.5;
                if (pixStep < 5) pixStep = 5;
                if (pixRadius < 5) pixRadius = 5;
                int index = 1;
                if (ScaleParamManager.Instance.Param.IsShowCircleMark)
                {
                    HXLDCont hXLDCont1 = new HXLDCont();
                    hXLDCont1.GenCircleContourXld(height * 0.5, width * 0.5, pixRadius, 0.0, Math.PI * 2, "positive", 0.01);
                    hXLDCont = hXLDCont.ConcatObj(hXLDCont1);
                }
                /////////////////////////////////////////
                if (ScaleParamManager.Instance.Param.IsShowScaleMark)
                {
                    // 绘制行方向
                    index = 1;
                    while (true)
                    {
                        hXLDCont = hXLDCont.ConcatObj(new HXLDCont(new HTuple(center_row - pixStep * index, center_row - pixStep * index), new HTuple(center_col, center_col + pixStep)));
                        if (center_row - pixStep * index < 0) break;
                        index++;
                    }
                    index = 1;
                    while (true)
                    {
                        hXLDCont = hXLDCont.ConcatObj(new HXLDCont(new HTuple(center_row + pixStep * index, center_row + pixStep * index), new HTuple(center_col, center_col + pixStep)));
                        if (center_row + pixStep * index > height) break;
                        index++;
                    }
                    // 绘制列方向
                    index = 1;
                    while (true)
                    {
                        hXLDCont = hXLDCont.ConcatObj(new HXLDCont(new HTuple(center_row, center_row + pixStep), new HTuple(center_col - pixStep * index, center_col - pixStep * index)));
                        if (center_col - pixStep * index < 0) break;
                        index++;
                    }
                    index = 1;
                    while (true)
                    {
                        hXLDCont = hXLDCont.ConcatObj(new HXLDCont(new HTuple(center_row, center_row + pixStep), new HTuple(center_col + pixStep * index, center_col + pixStep * index)));
                        if (center_col + pixStep * index > width) break;
                        index++;
                    }
                }
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
                        AcqSourceManage.Instance.GetAcqSource(this._viewConfigParam.CamName).Sensor.ImageAcqComplete -= new ImageAcqCompleteEventHandler(ImageAcqCompleteRFID_Event);
                    this._viewConfigParam.CamName = this.传感器comboBox1.SelectedItem.ToString();
                    ViewConfigParamManager.Instance.Save();
                    if (AcqSourceManage.Instance.GetAcqSource(this._viewConfigParam.CamName)?.Sensor != null)
                        AcqSourceManage.Instance.GetAcqSource(this._viewConfigParam.CamName).Sensor.ImageAcqComplete += new ImageAcqCompleteEventHandler(ImageAcqCompleteRFID_Event);
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

        #region 标定工具栏
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
                    case nameof(this.标定toolStripDropDownButton):

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
                                    //if (this._socket != null && this._socket.Type == enSocketType.客户端) // 如果相机连接类型是Socket，那么编辑时将从 Socket 网络获取图像
                                    //{
                                    //    SocketMessage message = new SocketMessage(enSocketInfo.获取窗口图像, this._sensor?.Name);
                                    //    message.ViewName = this._viewConfigParam.ViewName;
                                    //    object value = this._socket.GetDataAsync(message, true);
                                    //    message = value as SocketMessage;
                                    //    item.Value.treeView1_Edite(this, node, item.Key, message.MesContent as ImageDataClass);
                                    //}
                                    //else
                                    item.Value.treeView1_Edite(this, node, item.Key, this.drawObject.BackImage);
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
                    case nameof(this.Socket连接状态):
                        if (this._socket != null)
                        {
                            switch (this._socket.Type)
                            {
                                case enSocketType.客户端:
                                    this._socket.SendDataAsync("Connect", true); // 确认是否连接 
                                    if (!this._socket.SocketParams.IsConnect)
                                    {
                                        if (((ClientSocket)this._socket).ConnectAsync())
                                            new UserMessageForm().ShowDialog("连接服务器成功!");
                                        else
                                            new UserMessageForm().ShowDialog("连接服务器失败!");
                                    }
                                    else
                                        new UserMessageForm().ShowDialog("客户端处于连接状态");
                                    break;
                                case enSocketType.服务器:
                                    if (!this._socket.SocketParams.IsConnect)
                                        new UserMessageForm().ShowDialog("服务器在线!");
                                    else
                                        new UserMessageForm().ShowDialog("服务器离线!");
                                    break;
                            }
                        }
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
                if (this.drawObject.BackImage == null || this.drawObject.BackImage.Image == null) return;
                int width, height;
                this.drawObject.BackImage.Image.GetImageSize(out width, out height);
                //////////////////////
                switch (e.ClickedItem.Name)
                {
                    case nameof(this.绘制矩形toolStripMenuItem):
                        drawPixRect2 pixRect2 = new drawPixRect2(height * 0.5, width * 0.5, 0, 100, 100);
                        if (this._pixROI != null && this._pixROI is drawPixRect2)
                        {
                            drawPixRect2 pixRect21 = this._pixROI as drawPixRect2;
                            pixRect2 = new drawPixRect2(pixRect21.Row, pixRect21.Col, pixRect21.Rad, pixRect21.Length1, pixRect21.Length2);
                        }
                        else
                            pixRect2 = new drawPixRect2(height * 0.5, width * 0.5, 0, 100, 100);
                        ManualMeasureRect2Form manualMeasureRect2Form = new ManualMeasureRect2Form(this.drawObject.BackImage, pixRect2);
                        manualMeasureRect2Form.StartPosition = FormStartPosition.Manual;
                        manualMeasureRect2Form.Location = this.Location;
                        if (SystemParamManager.Instance.SysConfigParam.IsFormTopMost)
                        {
                            manualMeasureRect2Form.TopMost = true;
                            manualMeasureRect2Form.ShowInTaskbar = true;
                        }
                        if (manualMeasureRect2Form.ShowDialog() == DialogResult.OK)
                        {
                            this._pixROI = manualMeasureRect2Form.PixRect2;
                            this.drawObject.ClearViewObject();
                            this.drawObject.DrawingGraphicObject();
                            this.drawObject.AddViewObject(new ViewData(this._pixROI.GetXLD(), "red"));
                            //////////////////////// 显示测量值 ////////////////////////////
                            CoordSysAxisPosParam coordSysAxisPos = new CoordSysAxisPosParam();
                            coordSysAxisPos.UpdataAxisPosition(AcqSourceManage.Instance.GetAcqSource(this._viewConfigParam.CamName).CoordSysName);
                            drawWcsRect2 wcsRect2 = manualMeasureRect2Form.PixRect2.GetWcsRect2(this.drawObject.CameraParam, coordSysAxisPos.X, coordSysAxisPos.Y);
                            this._wcsROI = wcsRect2;
                            string content = string.Join("", $"X:{Math.Round(wcsRect2.X, 3)},Y:{Math.Round(wcsRect2.Y, 3)},Len1:{Math.Round(wcsRect2.Length1, 3)},Len2:{Math.Round(wcsRect2.Length2, 3)}");
                            userTextLable textLable = new userTextLable(content, "red");
                            textLable.X = manualMeasureRect2Form.PixRect2.Col + manualMeasureRect2Form.PixRect2.Length1 + 20;
                            textLable.Y = manualMeasureRect2Form.PixRect2.Row - manualMeasureRect2Form.PixRect2.Length2;
                            textLable.LablePose = enLablePosition.用户定义;
                            this.drawObject.AddViewObject(new ViewData(textLable, "red"));
                        }
                        break;
                    case nameof(this.绘制圆形ToolStripMenuItem):
                        drawPixCircle pixCircle = new drawPixCircle(height * 0.5, width * 0.5, 100);
                        if (this._pixROI != null && this._pixROI is drawPixCircle)
                        {
                            drawPixCircle pixCircle1 = this._pixROI as drawPixCircle;
                            pixCircle = new drawPixCircle(pixCircle1.Row, pixCircle1.Col, pixCircle1.Radius);
                        }
                        else
                            pixCircle = new drawPixCircle(height * 0.5, width * 0.5, 100);
                        ManualMeasureCircleForm manualMeasureCircleForm = new ManualMeasureCircleForm(this.drawObject.BackImage, pixCircle);
                        manualMeasureCircleForm.StartPosition = FormStartPosition.Manual;
                        manualMeasureCircleForm.Location = this.Location;
                        if (SystemParamManager.Instance.SysConfigParam.IsFormTopMost)
                        {
                            manualMeasureCircleForm.TopMost = true;
                            manualMeasureCircleForm.ShowInTaskbar = true;
                        }
                        if (manualMeasureCircleForm.ShowDialog() == DialogResult.OK)
                        {
                            this._pixROI = manualMeasureCircleForm.PixCircle;
                            this.drawObject.ClearViewObject();
                            this.drawObject.DrawingGraphicObject();
                            this.drawObject.AddViewObject(new ViewData(this._pixROI.GetXLD(), "red"));
                            //////////////////////// 显示测量值 ////////////////////////////
                            CoordSysAxisPosParam coordSysAxisPos = new CoordSysAxisPosParam();
                            coordSysAxisPos.UpdataAxisPosition(AcqSourceManage.Instance.GetAcqSource(this._viewConfigParam.CamName).CoordSysName);
                            drawWcsCircle wcsCircle = manualMeasureCircleForm.PixCircle.GetWcsCircle(this.drawObject.CameraParam, coordSysAxisPos.X, coordSysAxisPos.Y);
                            this._wcsROI = wcsCircle;
                            string content = string.Join("", $"X:{Math.Round(wcsCircle.X, 3)},Y:{Math.Round(wcsCircle.Y, 3)},Radius:{Math.Round(wcsCircle.Radius, 3)}");
                            userTextLable textLable = new userTextLable(content, "red");
                            textLable.X = manualMeasureCircleForm.PixCircle.Col + manualMeasureCircleForm.PixCircle.Radius + 20;
                            textLable.Y = manualMeasureCircleForm.PixCircle.Row - manualMeasureCircleForm.PixCircle.Radius;
                            textLable.LablePose = enLablePosition.用户定义;
                            this.drawObject.AddViewObject(new ViewData(textLable, "red"));
                        }
                        break;
                    case nameof(this.绘制椭圆ToolStripMenuItem):
                        drawPixEllipse pixEllipse = new drawPixEllipse(height * 0.5, width * 0.5, 0, 100, 100);
                        if (this._pixROI != null && this._pixROI is drawPixEllipse)
                        {
                            drawPixEllipse pixEllipse1 = this._pixROI as drawPixEllipse;
                            pixEllipse = new drawPixEllipse(pixEllipse1.Row, pixEllipse1.Col, pixEllipse1.Rad, pixEllipse1.Radius1, pixEllipse1.Radius2);
                        }
                        else
                            pixEllipse = new drawPixEllipse(height * 0.5, width * 0.5, 0, 100, 100);
                        ManualMeasureEllipseForm manualMeasureEllipseForm = new ManualMeasureEllipseForm(this.drawObject.BackImage, pixEllipse);
                        manualMeasureEllipseForm.StartPosition = FormStartPosition.Manual;
                        manualMeasureEllipseForm.Location = this.Location;
                        if (SystemParamManager.Instance.SysConfigParam.IsFormTopMost)
                        {
                            manualMeasureEllipseForm.TopMost = true;
                            manualMeasureEllipseForm.ShowInTaskbar = true;
                        }
                        if (manualMeasureEllipseForm.ShowDialog() == DialogResult.OK)
                        {
                            this._pixROI = manualMeasureEllipseForm.PixEllipse;
                            this.drawObject.ClearViewObject();
                            this.drawObject.DrawingGraphicObject();
                            this.drawObject.AddViewObject(new ViewData(this._pixROI.GetXLD(), "red"));
                            //////////////////////// 显示测量值 ////////////////////////////
                            CoordSysAxisPosParam coordSysAxisPos = new CoordSysAxisPosParam();
                            coordSysAxisPos.UpdataAxisPosition(AcqSourceManage.Instance.GetAcqSource(this._viewConfigParam.CamName).CoordSysName);
                            drawWcsEllipse wcsEllipse = manualMeasureEllipseForm.PixEllipse.GetWcsEllipse(this.drawObject.CameraParam, coordSysAxisPos.X, coordSysAxisPos.Y);
                            this._wcsROI = wcsEllipse;
                            string content = string.Join("", $"X:{Math.Round(wcsEllipse.X, 3)},Y:{Math.Round(wcsEllipse.Y, 3)},Radius1:{Math.Round(wcsEllipse.Radius1, 3)},Radius2:{Math.Round(wcsEllipse.Radius2, 3)}");
                            userTextLable textLable = new userTextLable(content, "red");
                            textLable.X = manualMeasureEllipseForm.PixEllipse.Col + manualMeasureEllipseForm.PixEllipse.Radius1 + 20;
                            textLable.Y = manualMeasureEllipseForm.PixEllipse.Row - manualMeasureEllipseForm.PixEllipse.Radius2;
                            textLable.LablePose = enLablePosition.用户定义;
                            this.drawObject.AddViewObject(new ViewData(textLable, "red"));
                        }
                        break;
                    case nameof(this.绘制直线ToolStripMenuItem):
                        drawPixLine pixLine = new drawPixLine(height * 0.5, width * 0.5, height * 0.5 + 200, width * 0.5);
                        if (this._pixROI != null && this._pixROI is drawPixLine)
                        {
                            drawPixLine pixLine1 = this._pixROI as drawPixLine;
                            pixLine = new drawPixLine(pixLine1.Row1, pixLine1.Col1, pixLine1.Row2, pixLine1.Col2);
                        }
                        else
                            pixLine = new drawPixLine(height * 0.5, width * 0.5, height * 0.5 + 200, width * 0.5);
                        ManualMeasureLineForm manualMeasureLineForm = new ManualMeasureLineForm(this.drawObject.BackImage, pixLine);
                        manualMeasureLineForm.StartPosition = FormStartPosition.Manual;
                        manualMeasureLineForm.Location = this.Location;
                        if (SystemParamManager.Instance.SysConfigParam.IsFormTopMost)
                        {
                            manualMeasureLineForm.TopMost = true;
                            manualMeasureLineForm.ShowInTaskbar = true;
                        }
                        if (manualMeasureLineForm.ShowDialog() == DialogResult.OK)
                        {
                            this._pixROI = manualMeasureLineForm.PixLine;
                            this.drawObject.ClearViewObject();
                            this.drawObject.DrawingGraphicObject();
                            this.drawObject.AddViewObject(new ViewData(this._pixROI.GetXLD(), "red"));
                            //////////////////////// 显示测量值 ////////////////////////////
                            CoordSysAxisPosParam coordSysAxisPos = new CoordSysAxisPosParam();
                            coordSysAxisPos.UpdataAxisPosition(AcqSourceManage.Instance.GetAcqSource(this._viewConfigParam.CamName).CoordSysName);
                            drawWcsLine wcsLine = manualMeasureLineForm.PixLine.GetWcsLine(this.drawObject.CameraParam, coordSysAxisPos.X, coordSysAxisPos.Y);
                            this._wcsROI = wcsLine;
                            string content = string.Join("", $"X1:{Math.Round(wcsLine.X1, 3)},Y1:{Math.Round(wcsLine.Y1, 3)},X2:{Math.Round(wcsLine.X2, 3)},Y2:{Math.Round(wcsLine.Y2, 3)}");
                            userTextLable textLable = new userTextLable(content, "red");
                            textLable.X = manualMeasureLineForm.PixLine.Col1 + 20;
                            textLable.Y = manualMeasureLineForm.PixLine.Row1;
                            textLable.LablePose = enLablePosition.用户定义;
                            this.drawObject.AddViewObject(new ViewData(textLable, "red"));
                        }
                        break;
                    case nameof(this.绘制点ToolStripMenuItem):
                        drawPixPoint pixPoint = new drawPixPoint(height * 0.5, width * 0.5);
                        if (this._pixROI != null && this._pixROI is drawPixPoint)
                        {
                            drawPixPoint pixPoint1 = this._pixROI as drawPixPoint;
                            pixPoint = new drawPixPoint(pixPoint1.Row, pixPoint1.Col);
                        }
                        else
                            pixPoint = new drawPixPoint(height * 0.5, width * 0.5);
                        ManualMeasurePointForm manualMeasurePointForm = new ManualMeasurePointForm(this.drawObject.BackImage, pixPoint);
                        manualMeasurePointForm.StartPosition = FormStartPosition.Manual;
                        manualMeasurePointForm.Location = this.Location;
                        if (SystemParamManager.Instance.SysConfigParam.IsFormTopMost)
                        {
                            manualMeasurePointForm.TopMost = true;
                            manualMeasurePointForm.ShowInTaskbar = true;
                        }
                        if (manualMeasurePointForm.ShowDialog() == DialogResult.OK)
                        {
                            this._pixROI = manualMeasurePointForm.PixPoint;
                            this.drawObject.ClearViewObject();
                            this.drawObject.DrawingGraphicObject();
                            this.drawObject.AddViewObject(new ViewData(this._pixROI.GetXLD(), "red"));
                            //////////////////////// 显示测量值 ////////////////////////////
                            CoordSysAxisPosParam coordSysAxisPos = new CoordSysAxisPosParam();
                            coordSysAxisPos.UpdataAxisPosition(AcqSourceManage.Instance.GetAcqSource(this._viewConfigParam.CamName).CoordSysName);
                            drawWcsPoint wcsPoint = manualMeasurePointForm.PixPoint.GetWcsPoint(this.drawObject.CameraParam, coordSysAxisPos.X, coordSysAxisPos.Y);
                            this._wcsROI = wcsPoint;
                            string content = string.Join("", $"X:{Math.Round(wcsPoint.X, 3)},Y:{Math.Round(wcsPoint.Y, 3)}");
                            userTextLable textLable = new userTextLable(content, "red");
                            textLable.X = manualMeasurePointForm.PixPoint.Col + 20;
                            textLable.Y = manualMeasurePointForm.PixPoint.Row;
                            textLable.LablePose = enLablePosition.用户定义;
                            this.drawObject.AddViewObject(new ViewData(textLable, "red"));
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                new Common.UserMessageForm(ex.ToString()).ShowDialog();
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
                        //string value = AcqSourceManage.Instance.GetAcqSource(this._viewConfigParam.CamName).Sensor.GetParam("曝光").ToString();
                        SetCamExposeForm renameForm = new SetCamExposeForm(this._viewConfigParam.CamName);
                        if (SystemParamManager.Instance.SysConfigParam.IsFormTopMost)
                        {
                            renameForm.TopMost = true;
                            renameForm.ShowInTaskbar = true;
                        }
                        renameForm.ShowDialog();
                        break;
                    case nameof(this.设置增益ToolStripMenuItem):
                        if (AcqSourceManage.Instance.GetAcqSource(this._viewConfigParam.CamName) == null || AcqSourceManage.Instance.GetAcqSource(this._viewConfigParam.CamName).Sensor == null) return;
                        //value = AcqSourceManage.Instance.GetAcqSource(this._viewConfigParam.CamName).Sensor.GetParam("增益").ToString();
                        SetCamGainForm renameForm2 = new SetCamGainForm(this._viewConfigParam.CamName);
                        if (SystemParamManager.Instance.SysConfigParam.IsFormTopMost)
                        {
                            renameForm2.TopMost = true;
                            renameForm2.ShowInTaskbar = true;
                        }
                        renameForm2.ShowDialog();
                        break;
                    case nameof(this.日志面板ToolStripMenuItem):
                        LogViewForm logForm = new LogViewForm(this._sensor?.Name);
                        if (SystemParamManager.Instance.SysConfigParam.IsFormTopMost)
                        {
                            logForm.TopMost = true;
                            logForm.ShowInTaskbar = true;
                        }
                        logForm.Show();
                        break;
                }
            }
            catch (Exception ex)
            {
                new Common.UserMessageForm(ex.ToString()).ShowDialog();
            }
        }

        private void 标定toolStripDropDownButton_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

            try
            {
                CameraParam NowCaliPara = AcqSourceManage.Instance.GetAcqSource(this.传感器comboBox1.SelectedItem.ToString())?.Sensor?.CameraParam;
                if (NowCaliPara == null)
                {
                    new UserMessageForm().ShowDialog("获取的相机参数为NULL");
                    return;
                }
                switch (e.ClickedItem.Name)
                {
                    case nameof(this.九点标定ToolStripMenuItem):
                        CamNPointCalibParamSimpleForm Instance = new CamNPointCalibParamSimpleForm(NowCaliPara);
                        if (SystemParamManager.Instance.SysConfigParam.IsFormTopMost)
                        {
                            Instance.TopMost = true;
                            Instance.ShowInTaskbar = true;
                        }
                        Instance.Show();
                        break;
                    case nameof(this.旋转标定ToolStripMenuItem):
                        CamRotatetCalibrateSimpleForm InstanceRotate = new CamRotatetCalibrateSimpleForm(NowCaliPara);
                        if (SystemParamManager.Instance.SysConfigParam.IsFormTopMost)
                        {
                            InstanceRotate.TopMost = true;
                            InstanceRotate.ShowInTaskbar = true;
                        }
                        InstanceRotate.Show();
                        break;
                    case nameof(this.九点旋转标定ToolStripMenuItem):
                        Cam9PointCalibrateSimpleForm Instance2 = new Cam9PointCalibrateSimpleForm(NowCaliPara);
                        if (SystemParamManager.Instance.SysConfigParam.IsFormTopMost)
                        {
                            Instance2.TopMost = true;
                            Instance2.ShowInTaskbar = true;
                        }
                        Instance2.Show();
                        break;
                    case nameof(this.世界坐标映射标定ToolStripMenuItem):
                        CamMapCalibParamSimpleForm Instance4 = new CamMapCalibParamSimpleForm(NowCaliPara);
                        if (SystemParamManager.Instance.SysConfigParam.IsFormTopMost)
                        {
                            Instance4.TopMost = true;
                            Instance4.ShowInTaskbar = true;
                        }
                        Instance4.Show();
                        break;
                    case nameof(this.像素坐标映射标定toolStripMenuItem):
                        CameraParam TargetCaliPara = AcqSourceManage.Instance.GetCamAcqSource(NowCaliPara.CaliParam.MapCamName).Sensor.CameraParam;
                        UpDnCamCalibSimpleForm Instance6 = new UpDnCamCalibSimpleForm(NowCaliPara, TargetCaliPara);
                        if (SystemParamManager.Instance.SysConfigParam.IsFormTopMost)
                        {
                            Instance6.TopMost = true;
                            Instance6.ShowInTaskbar = true;
                        }
                        Instance6.Show();
                        break;
                    case nameof(this.标定板映射标定toolStripMenuItem):
                        TargetCaliPara = AcqSourceManage.Instance.GetCamAcqSource(NowCaliPara.CaliParam.MapCamName).Sensor.CameraParam;
                        CaliboardMapSimpleForm Instance7 = new CaliboardMapSimpleForm(NowCaliPara, TargetCaliPara);
                        if (SystemParamManager.Instance.SysConfigParam.IsFormTopMost)
                        {
                            Instance7.TopMost = true;
                            Instance7.ShowInTaskbar = true;
                        }
                        Instance7.Show();
                        break;
                    case nameof(this.标定板标定ToolStripMenuItem):
                        CaliCaliboardSimpleForm Instance3 = new CaliCaliboardSimpleForm(NowCaliPara);
                        if (SystemParamManager.Instance.SysConfigParam.IsFormTopMost)
                        {
                            Instance3.TopMost = true;
                            Instance3.ShowInTaskbar = true;
                        }
                        Instance3.Show();
                        break;

                    case nameof(this.手动九点标定ToolStripMenuItem):
                        ManualCalibForm Instance8 = new ManualCalibForm(this.drawObject.BackImage, NowCaliPara);
                        if (SystemParamManager.Instance.SysConfigParam.IsFormTopMost)
                        {
                            Instance8.TopMost = true;
                            Instance8.ShowInTaskbar = true;
                        }
                        Instance8.Show();
                        break;

                    case nameof(this.相机针头标定ToolStripMenuItem):
                        CameraGlueGunCalibrateFormNew Instance9 = new CameraGlueGunCalibrateFormNew(NowCaliPara);
                        if (SystemParamManager.Instance.SysConfigParam.IsFormTopMost)
                        {
                            Instance9.TopMost = true;
                            Instance9.ShowInTaskbar = true;
                        }
                        Instance9.Show();
                        break;

                }
            }
            catch (Exception ex)
            {
                new Common.UserMessageForm(ex.ToString()).ShowDialog();
            }

        }
        #endregion 标定工具栏
        private void 补偿checkBox_Click(object sender, EventArgs e)
        {
            try
            {
                if (this._param != null)
                {
                    CompensateForm form = new CompensateForm(this._param, this._zoneParam, this._viewConfigParam);
                    form.Show();
                }
                //////////////////////////
                if (this._rectifyParam != null)
                {
                    RectifyCompensateForm form = new RectifyCompensateForm(this._rectifyParam, this._zoneRectifyeParam);
                    form.Show();
                }
            }
            catch (Exception ex)
            {
                new Common.UserMessageForm(ex.ToString()).ShowDialog();
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
                new Common.UserMessageForm(ex.ToString()).ShowDialog();
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


        /// <summary>
        /// 接收图像生成事件，用于外部触发的飞拍场合
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ImageAcqCompleteRFID_Event(object sender, ImageAcqCompleteEventArgs e)
        {
            try
            {
                this.drawObject.IsLiveState = true;
                this.drawObject.BackImage?.Dispose();
                this.drawObject.BackImage = e.ImageData;
                if (this._viewConfigParam.IsShowCross)
                    this.drawObject.AddViewObject(new ViewData(ScaleParamForm.GetCrossIcon(this.drawObject.CameraParam, this.drawObject.BackImage.Width, this.drawObject.BackImage.Height), ScaleParamManager.Instance.Param.Color));
            }
            catch (Exception ex)
            {
                LoggerHelper.Error($"相机:{this._sensor?.Name}:接收到图像失败，错误信息:{ex.ToString()}", this._sensor?.Name);
            }
        }

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
                this._triggerSingleEvent.Set(); // 异步采集，每生成一张图像，发送一次事件信号
                if (node != null && "On" == Sensor.SensorManage.GetSensor(e.ImageData.CamName)?.GetParam(enSocketInfo.获取触发模式).ToString())  //  Off : 只有在触发模式下获取的图像，这里才执行 
                {
                    IFunction item = node.Tag as IFunction;
                    Task.Run(() =>
                    {
                        item?.Execute(node, this.drawObject.BackImage);
                        /////////////////////////////// 执行完后发送窗口图像 ///////////////////////////////////////////////////
                        if (this._viewConfigParam.EnableSocketSingleBinding) // 如果启用了 Socket 消息信号
                        {
                            SocketMessage message = new SocketMessage();
                            message.Name = this.drawObject.BackImage.CamName;
                            message.ViewName = this._viewConfigParam.ViewName;
                            message.MesContent = new ImageDataClass(this.hWindowControl1.HalconWindow.DumpWindowImage());
                            message.Lable = enSocketInfo.设置窗口图像;
                            if (this._socket != null)
                                this._socket.SendDataAsync(message, false);
                            else
                                LoggerHelper.Error(this._viewConfigParam.ViewName + "Socket 对象为空!");
                        }
                    });
                }
                else
                {
                    LoggerHelper.Error(this._viewConfigParam.ViewName + "窗口没有绑定程序节点，或都窗口处于实时采集状态!");
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.Error(this._viewConfigParam.ViewName + "图像回调函数执行失败!" + ex.ToString());
            }
        }

        /// <summary>
        /// 这里用于实时采集模式，单次获取图像的场景，图像回调函数用于外触发场景
        /// </summary>
        /// <param name="send"></param>
        /// <param name="e"></param>
        private void WaitTriggerSingle(object send, PoseInfoEventArgs e)
        {
            /////////////////////////////////////////////////////////////
            if (AcqSourceManage.Instance.GetAcqSource(this._viewConfigParam.CamName)?.CoordSysName != e.CoordSysName)
            {
                return;  // 如果坐标系不相等，将返回
            }
            else
            {
                this._manualEvent.Reset();
                this._manualEvent.Set();
                this._imageIndex = 0; // 收到触发信号，表示新的一行开始，将图像索引置零
                this._sensor.StartTrigger();
                CommunicationConfigParamManger.Instance.WriteValue(e.CoordSysName, enCommunicationCommand.TriggerToPlc, 1); // 与运控确认指令信号
                LoggerHelper.Info($"相机:{this._sensor.Name}:接收到触发信号，并回复运控确认信号", this._sensor.Name);
            }
            ///////////////////////////////////////////////////////////////////////
            if (!this._viewConfigParam.EnableTriggerSingleBinding) return;  // 如果没有绑定触发信号，将返回
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

        private void HeartCallback(object state)
        {
            try
            {
                this.Invoke(new Action(() =>
                {
                    if (this._socket != null)
                    {
                        if (this._socket.SocketParams.IsConnect)
                        {
                            this.Socket连接状态.Text = this._socket?.Type.ToString();
                            this.Socket连接状态.Image = FunctionBlock.Properties.Resources.green1;
                        }
                        else
                        {
                            this.Socket连接状态.Text = this._socket?.Type.ToString();
                            this.Socket连接状态.Image = FunctionBlock.Properties.Resources.red1;
                        }
                    }
                    else
                    {
                        this.Socket连接状态.Text = this._socket?.Type.ToString();
                        this.Socket连接状态.Image = FunctionBlock.Properties.Resources.red1;
                    }
                }));
            }
            catch
            {

            }
        }

        private void 程序节点comboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {

        }


        private void 程序节点comboBox_Click(object sender, EventArgs e)
        {
            //List<TreeNode> listTreeNode = new List<TreeNode>();
            //// 获取面板上的节点
            //foreach (var item in ProgramForm.Instance.ProgramDic.Values)
            //{
            //    listTreeNode.AddRange(item.GetTreeViewNodeTag());
            //}
            //// 获取流程单元下的标签
            //foreach (TreeNode item in listTreeNode)
            //{
            //    switch (item.Tag?.GetType().Name)
            //    {
            //        case nameof(JobUnit):
            //            BindingList<PlcCommunicateInfo> plcInfo = ((BaseFunction)item.Tag).ResultInfo as BindingList<PlcCommunicateInfo>;
            //            if (plcInfo.Count > 0)
            //            {
            //                this.程序节点comboBox.Items.Clear();
            //                this.程序节点comboBox.Items.Add("NONE");
            //                foreach (TreeNode node in item.Nodes)
            //                {
            //                    if (node.Name.Contains("Tool"))
            //                        this.程序节点comboBox.Items.Add(node.Text);
            //                }
            //            }
            //            else
            //                continue;
            //            break;
            //    }
            //}
        }

        /// <summary>
        /// SocketMessage: 消息事件处理方法, 回调函数只处理服务器接收到的请求信息
        /// </summary>
        /// <param name="send"></param>
        /// <param name="e"></param>
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
                                case enSocketInfo.设置曝光:
                                    if (this._socket != null && this._socket.Type == enSocketType.服务器)
                                    {
                                        this._sensor?.SetParam("曝光", mesg.MesContent);
                                    }
                                    break;
                                case enSocketInfo.获取曝光:
                                    if (this._socket != null && this._socket.Type == enSocketType.服务器)
                                    {
                                        SocketMessage message1 = new SocketMessage(enSocketInfo.获取曝光);
                                        message1.Name = this._sensor?.Name;
                                        message1.ViewName = this._viewConfigParam.ViewName;
                                        object expose = this._sensor?.GetParam("曝光");
                                        message1.MesContent = expose;
                                        this._socket.SendDataAsync(message1, true);
                                    }
                                    else
                                        LoggerHelper.Error(this._viewConfigParam.ViewName + " Socket对象为空或Socket类型为客户端");
                                    break;
                                case enSocketInfo.设置增溢:
                                    if (this._socket != null && this._socket.Type == enSocketType.服务器)
                                    {
                                        this._sensor?.SetParam("增溢", mesg.MesContent);
                                    }
                                    break;
                                case enSocketInfo.获取增溢:
                                    if (this._socket != null && this._socket.Type == enSocketType.服务器)
                                    {
                                        SocketMessage message1 = new SocketMessage(enSocketInfo.设置窗口图像);
                                        message1.Name = this._sensor?.Name;
                                        message1.ViewName = this._viewConfigParam.ViewName;
                                        object gain = this._sensor?.GetParam("增溢");
                                        message1.MesContent = gain;
                                        this._socket.SendDataAsync(message1, true);
                                    }
                                    else
                                        LoggerHelper.Error(this._viewConfigParam.ViewName + " Socket对象为空或Socket类型为客户端");
                                    break;
                                case enSocketInfo.实时采集:
                                    if (this._socket != null && this._socket.Type == enSocketType.服务器)
                                    {
                                        this.stopwatch.Restart();
                                        SocketMessage message1 = new SocketMessage(enSocketInfo.设置窗口图像);
                                        message1.Name = this._sensor?.Name;
                                        message1.ViewName = this._viewConfigParam.ViewName;
                                        ///////////////////////////////
                                        if (this._sensor.CameraParam.AcqMode == enAcqMode.异步采集)
                                        {
                                            if (this._triggerSingleEvent.WaitOne(this._sensor.CameraParam.Timeout))
                                                message1.MesContent = this.drawObject.BackImage;
                                            else
                                                message1.MesContent = null;
                                        }
                                        else
                                        {
                                            if (mesg.TrigerMode == enUserTrigerMode.NONE) // 查看传递过来的触发模式 
                                                this._sensor?.SetParam(enSocketInfo.实时采集, 0);
                                            else
                                                this._sensor?.SetParam(enSocketInfo.外部触发, 0);
                                            this._sensor.StartTrigger();
                                            this._sensor.StopTrigger();
                                            message1.MesContent = this._sensor.ReadData();
                                        }
                                        this.stopwatch.Stop();
                                        message1.Time = this.stopwatch.ElapsedMilliseconds;
                                        this._socket.SendDataAsync(message1, true); // 这里还是需要新建一个 SocketMessage
                                    }
                                    else
                                        LoggerHelper.Error(this._viewConfigParam.ViewName + " Socket对象为空或Socket类型为客户端");
                                    break;
                                case enSocketInfo.获取触发模式:
                                    if (this._socket != null && this._socket.Type == enSocketType.服务器)
                                    {
                                        SocketMessage message1 = new SocketMessage(enSocketInfo.设置窗口图像);
                                        message1.Name = this._sensor?.Name;
                                        message1.ViewName = this._viewConfigParam.ViewName;
                                        /////////////////////////////////////////////////////
                                        object trigMode = this._sensor?.GetParam(enSocketInfo.获取触发模式);
                                        message1.MesContent = trigMode;
                                        this._socket.SendDataAsync(message1, true);
                                    }
                                    else
                                        LoggerHelper.Error(this._viewConfigParam.ViewName + " Socket对象为空或Socket类型为客户端");
                                    break;
                                case enSocketInfo.软触发:
                                    if (this._socket != null && this._socket.Type == enSocketType.服务器)
                                    {
                                        this._sensor?.SetParam(enSocketInfo.软触发, 0);
                                    }
                                    break;
                                case enSocketInfo.外部触发:
                                    if (this._socket != null && this._socket.Type == enSocketType.服务器)
                                    {
                                        this._sensor?.SetParam(enSocketInfo.外部触发, 0);
                                    }
                                    break;
                                case enSocketInfo.设置窗口图像:  // 这里只做接收用
                                    ImageDataClass imageData = mesg.MesContent as ImageDataClass;
                                    this.drawObject.BackImage = (imageData);
                                    break;
                                case enSocketInfo.图像采集:
                                case enSocketInfo.获取窗口图像: // 将图像传送出去
                                    if (this._socket != null && this._socket.Type == enSocketType.服务器)
                                    {
                                        this.stopwatch.Restart();
                                        SocketMessage message2 = new SocketMessage();
                                        message2.Lable = enSocketInfo.NONE;
                                        message2.Name = this._sensor?.Name;
                                        ///////////////////////////////////////////////
                                        if (this._sensor.CameraParam.AcqMode == enAcqMode.异步采集)
                                        {
                                            if (this._triggerSingleEvent.WaitOne(this._sensor.CameraParam.Timeout))
                                                message2.MesContent = this.drawObject.BackImage;
                                            else
                                                message2.MesContent = null;
                                        }
                                        else
                                        {
                                            if (mesg.TrigerMode == enUserTrigerMode.NONE) // 查看传递过来的触发模式 
                                                this._sensor?.SetParam(enSocketInfo.实时采集, 0);
                                            else
                                                this._sensor?.SetParam(enSocketInfo.外部触发, 0);
                                            this._sensor.StartTrigger();
                                            this._sensor.StopTrigger();
                                            Dictionary<enDataItem, object> data = this._sensor.ReadData();
                                            if (data?.Count > 0)
                                                message2.MesContent = data[enDataItem.Image];
                                        }
                                        this.stopwatch.Stop();
                                        message2.Time = this.stopwatch.ElapsedMilliseconds;
                                        this._socket.SendDataAsync(message2, true);
                                    }
                                    else
                                        LoggerHelper.Error(this._viewConfigParam.ViewName + " Socket对象为空或Socket类型为客户端");
                                    break;
                                case enSocketInfo.写入相机参数:
                                    if (this._socket != null && this._socket.Type == enSocketType.服务器)
                                    {
                                        this._sensor.CameraParam = mesg.MesContent as CameraParam;
                                        this._sensor?.CameraParam.Save();
                                        SocketMessage message1 = new SocketMessage(enSocketInfo.写入相机参数);
                                        message1.Name = this._sensor?.Name;
                                        message1.ViewName = this._viewConfigParam.ViewName;
                                        message1.MesContent = "OK";
                                        this._socket.SendDataAsync(message1, true);
                                    }
                                    break;
                                case enSocketInfo.读取相机参数:
                                    if (this._socket != null && this._socket.Type == enSocketType.服务器)
                                    {
                                        SocketMessage message1 = new SocketMessage(enSocketInfo.读取相机参数);
                                        message1.Name = this._sensor?.Name;
                                        message1.ViewName = this._viewConfigParam.ViewName;
                                        message1.MesContent = this._sensor?.CameraParam;
                                        this._socket.SendDataAsync(message1, true);
                                    }
                                    else
                                        LoggerHelper.Error(this._viewConfigParam.ViewName + " Socket对象为空或Socket类型为客户端");
                                    break;

                                case enSocketInfo.设置光源亮度:
                                    if (this._socket != null && this._socket.Type == enSocketType.服务器)
                                    {
                                        if (Light.LightConnectManage.GetLight(mesg.Name).SetLight((enLightChannel)mesg.Channel, Convert.ToInt32(mesg.MesContent)))
                                            mesg.MesContent = "OK";
                                        else
                                            mesg.MesContent = "NG";
                                        this._socket.SendDataAsync(mesg, true);
                                    }
                                    else
                                        LoggerHelper.Error(this._viewConfigParam.ViewName + " Socket对象为空或Socket类型为客户端");
                                    break;
                                case enSocketInfo.获取光源亮度:
                                    if (this._socket != null && this._socket.Type == enSocketType.服务器)
                                    {
                                        int value = Light.LightConnectManage.GetLight(mesg.Name).GetLight((enLightChannel)mesg.Channel);
                                        mesg.MesContent = value;
                                        this._socket.SendDataAsync(mesg, true);
                                    }
                                    else
                                        LoggerHelper.Error(this._viewConfigParam.ViewName + " Socket对象为空或Socket类型为客户端");
                                    break;
                                case enSocketInfo.关闭光源:
                                    if (this._socket != null && this._socket.Type == enSocketType.服务器)
                                    {
                                        if (Light.LightConnectManage.GetLight(mesg.Name).Close((enLightChannel)mesg.Channel))
                                            mesg.MesContent = "OK";
                                        else
                                            mesg.MesContent = "NG";
                                        this._socket.SendDataAsync(mesg, true);
                                    }
                                    else
                                        LoggerHelper.Error(this._viewConfigParam.ViewName + " Socket对象为空或Socket类型为客户端");
                                    break;
                                case enSocketInfo.打开光源:
                                    if (this._socket != null && this._socket.Type == enSocketType.服务器)
                                    {
                                        if (Light.LightConnectManage.GetLight(mesg.Name).Open((enLightChannel)mesg.Channel))
                                            mesg.MesContent = "OK";
                                        else
                                            mesg.MesContent = "NG";
                                        this._socket.SendDataAsync(mesg, true);
                                    }
                                    else
                                        LoggerHelper.Error(this._viewConfigParam.ViewName + " Socket对象为空或Socket类型为客户端");
                                    break;
                                default: // 默认方法 
                                    LoggerHelper.Error(this._viewConfigParam.ViewName + $" 取图时间:{mesg?.Time}");
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


    }

}

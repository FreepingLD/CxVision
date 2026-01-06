
using Common;
using FunctionBlock;
using HalconDotNet;
using MotionControlCard;
using Sensor;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using userControl;
using View;

namespace FunctionBlock
{
    public partial class ImageViewForm : Form
    {
        private CancellationTokenSource cts;
        private DrawingBaseMeasure drawObject;
        private AcqSource acqSource;
        private ISensor _Sensor;
        private ViewConfigParam _viewConfigParam;
        private bool IsLoad = false; // 窗体加载后，设置为true
        private MetrolegyParamForm metrolegyParamForm;
        private IFunction _currFunction;
        private ImageDataClass CurrentImageData;
        private Dictionary<string, object> listData = new Dictionary<string, object>();
        private string viewName = "";
        private object lockSyn = new object();
        private TreeNode _refNode = null;
        private SocketBase _socket;
        private ISensor _sensor = null;
        private PixROI _pixROI = null;
        private WcsROI _wcsROI = null;
        private double _curRow = 0;
        private double _curCol = 0;
        public ImageViewForm(ViewConfigParam viewConfigParam)
        {
            InitializeComponent();
            this._viewConfigParam = viewConfigParam;
            /////////////
            this.drawObject = new DrawingBaseMeasure(this.hWindowControl1, false);
            this.drawObject.HMouseDoubleClick += new HMouseEventHandler(this.hWindowControl1_DoubleClick);
            //UserLoginParamManager.Instance.LoginParam.UserChange += new EventHandler(this.UserChange_Event);
            UserLoginParamManager.Instance.UserChange += new EventHandler(this.UserChange_Event);
            this.hWindowControl1.Margin = new Padding(0);
            this.Padding = new Padding(0);
            if (!HWindowManage.HWindowList.ContainsKey(_viewConfigParam.ViewName))
            {
                HWindowManage.HWindowList.Add(_viewConfigParam.ViewName, this.hWindowControl1.HalconWindow);
            }
            if (!HWindowManage.HWindowControlList.ContainsKey(_viewConfigParam.ViewName))
            {
                HWindowManage.HWindowControlList.Add(_viewConfigParam.ViewName, this.hWindowControl1);
            }
            this.titleLabel.Text = "图像视图";
            this.ContextMenu = new ContextMenu();
        }
        public ImageViewForm(IFunction _function, ImageDataClass image)
        {
            this._currFunction = _function;
            InitializeComponent();
            /////////////
            this.drawObject = new DrawingBaseMeasure(this.hWindowControl1, false);
            this.drawObject.HMouseDoubleClick += new HMouseEventHandler(this.hWindowControl1_DoubleClick);
            this.drawObject.BackImage = image;
            this.hWindowControl1.Margin = new Padding(0);
            this.Padding = new Padding(0);
            if (!HWindowManage.HWindowList.ContainsKey(_viewConfigParam.ViewName))
            {
                HWindowManage.HWindowList.Add(_viewConfigParam.ViewName, this.hWindowControl1.HalconWindow);
            }
            if (!HWindowManage.HWindowControlList.ContainsKey(_viewConfigParam.ViewName))
            {
                HWindowManage.HWindowControlList.Add(_viewConfigParam.ViewName, this.hWindowControl1);
            }
            this.titleLabel.Text = "图像视图";
        }
        public ImageViewForm(bool isShowMultipleElement, bool isShowJudgeResult)
        {
            InitializeComponent();
            /////////////
            this.drawObject = new DrawingBaseMeasure(this.hWindowControl1, false);
            this.hWindowControl1.Margin = new Padding(1);
            this.Padding = new Padding(0);

        }
        private void ImageViewForm_Load(object sender, EventArgs e)
        {
            this.drawObject.GrayValueInfo += new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
            BaseFunction.ExcuteCompleted += new ExcuteCompletedEventHandler(DisplayExcuteResult);
            TreeViewWrapClass.ClickNode += new ClickNodeEventHandler(this.DisplayClickObject);
            this.metrolegyParamForm = new MetrolegyParamForm(this.drawObject);
            //////////////
            BindProperty();
            this.addContextMenu(this.hWindowControl1);
            // 用于动态添加窗体
            this._viewConfigParam = this._viewConfigParam == null ? new ViewConfigParam() : this._viewConfigParam;
            this.Location = this._viewConfigParam.Location;
            this.Size = this._viewConfigParam.FormSize;
            this.IsLoad = true;
        }

        private void BindProperty()
        {
            try
            {
                this.传感器comboBox.Items.Clear();
                this.传感器comboBox.Items.AddRange(AcqSourceManage.Instance.GetAcqSourceName());
                this.传感器comboBox.Text = this._viewConfigParam.CamName;
                //this.传感器comboBox.DataSource = AcqSourceManage.Instance.GetAcqSourceName();// SensorManage.GetSensorName();
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
                        //this.传感器comboBox1.Enabled = false;
                        //this.程序节点comboBox.Enabled = false;
                        this.buttonClose.Enabled = false;
                        break;
                    case enUserName.工程师:
                        //this.传感器comboBox1.Enabled = true;
                        //this.程序节点comboBox.Enabled = true;
                        this.buttonClose.Enabled = false;
                        break;
                    case enUserName.开发人员:
                        //this.传感器comboBox1.Enabled = true;
                        //this.程序节点comboBox.Enabled = true;
                        this.buttonClose.Enabled = true;
                        break;
                }
            }
            catch
            {
            }
        }
        private void ImageViewForm_Move(object sender, EventArgs e)
        {
            if (this._viewConfigParam != null)
                this._viewConfigParam.Location = this.Location;
        }

        private void ImageViewForm_Resize(object sender, EventArgs e)
        {
            if (this._viewConfigParam != null && this.IsLoad) // 在窗体加载后这个事件才起作用
                this._viewConfigParam.FormSize = this.Size;
        }

        private void ImageViewForm_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, WM_SYSCOMMAND, SC_MOVE + HTCAPTION, 0);
        }

        private void hWindowControl1_DoubleClick(Object sender, HalconDotNet.HMouseEventArgs e)
        {
            //int a = 10;
            this.buttonMax_Click(null, null);
        }
        private void ImageViewForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                this.cts?.Cancel();
                this.cts2?.Cancel();
                this.IsLoad = false;
                //ViewConfigParamManager.Instance.ViewParamList.Remove(this._viewConfigParam); // 关闭窗体时要删除相应的对象
                if (this.drawObject != null)
                    this.drawObject.ClearDrawingObject();
                //UserLoginParamManager.Instance.LoginParam.UserChange -= new EventHandler(this.UserChange_Event);
                UserLoginParamManager.Instance.UserChange -= new EventHandler(this.UserChange_Event);
                this.drawObject.GrayValueInfo -= new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                BaseFunction.ExcuteCompleted -= new ExcuteCompletedEventHandler(DisplayExcuteResult);
                TreeViewWrapClass.ClickNode -= new ClickNodeEventHandler(this.DisplayClickObject);
                this.drawObject.HMouseDoubleClick -= new HMouseEventHandler(this.hWindowControl1_DoubleClick);
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
                    if (UserLoginParamManager.Instance.CurrentUser != enUserName.开发人员) return;
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
            DialogResult dialogResult = new Common.UserMessageForm().ShowDialog("确定关闭窗体吗？", "关闭窗体");
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
            this.WindowState = FormWindowState.Minimized;  //最小化
        }

        #endregion
        private bool IsSelect()
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
                            result = false;
                        else
                            result = true;
                    }));
                    break;
            }
            return result;
        }
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
        #region  数据实时采集
        private CancellationTokenSource cts2;
        private void 实时采集checkBox_CheckedChangedOld(object sender, EventArgs e)
        {
            if (!IsSelect()) return;
            /////////////////////////////////////////
            switch (this.实时采集checkBox.CheckState)
            {
                case CheckState.Checked:
                    this.实时采集checkBox.BackColor = Color.Red;
                    cts2 = new CancellationTokenSource();
                    Dictionary<enDataItem, object> data;
                    this.acqSource = AcqSourceManage.Instance.GetAcqSource(this._viewConfigParam.CamName);
                    Task.Run(() =>
                    {
                        this.drawObject.IsLiveState = true;
                        while (!cts2.IsCancellationRequested)
                        {
                            if (SystemParamManager.Instance.SysConfigParam.IsAutoRun)
                            {
                                this.实时采集checkBox.BackColor = Color.Lime;
                                cts2?.Cancel();
                            }
                            /////////////////////////////////////////////////////////
                            if (this.acqSource == null) return;
                            data = this.acqSource?.AcqPointData();
                            switch (this.acqSource?.Sensor.ConfigParam.SensorType)
                            {
                                case enUserSensorType.点激光:
                                    break;
                                case enUserSensorType.线激光:
                                    break;
                                case enUserSensorType.面激光:
                                    break;
                                case enUserSensorType.面阵相机:
                                    if (data?.Count > 0)
                                    {
                                        double[] axisPose = this.acqSource.GetAxisPosition();
                                        this.drawObject.BackImage = (ImageDataClass)data[enDataItem.Image];
                                        this.drawObject.BackImage.Grab_X = axisPose[0];
                                        this.drawObject.BackImage.Grab_Y = axisPose[1];
                                        this.drawObject.BackImage.Grab_Z = axisPose[2];
                                        this.drawObject.BackImage.Grab_Theta = axisPose[3];
                                        this.drawObject.AttachPropertyData.Clear();
                                        this.drawObject.AttachPropertyData.Add(new ViewData(ScaleParamForm.GetCrossIcon(this.drawObject.CameraParam, this.drawObject.BackImage.Width, this.drawObject.BackImage.Height), ScaleParamManager.Instance.Param.Color));
                                    }
                                    break;
                            }
                            Thread.Sleep(20);
                        }
                        this.drawObject.IsLiveState = false;
                    });
                    break;
                default:
                    this.cts2.Cancel();
                    this.实时采集checkBox.BackColor = Color.Lime;
                    break;
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
                        this.acqSource = AcqSourceManage.Instance.GetCamAcqSource(this._viewConfigParam.CamName);
                        if (this.acqSource == null) return;
                        this._sensor = this.acqSource.Sensor;
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
                                        this._sensor.SetParam("实时采集", 0);
                                        data = this._sensor.ReadData();// this.acqSource.AcqImageData(null);
                                        if (data?.Count > 0)
                                        {
                                            this.drawObject.BackImage = (ImageDataClass)data[enDataItem.Image];
                                            this.drawObject.AttachPropertyData.Clear();
                                            this.drawObject.AttachPropertyData.Add(new ViewData(ScaleParamForm.GetCrossIcon(this.drawObject.CameraParam, this.drawObject.BackImage.Width, this.drawObject.BackImage.Height), ScaleParamManager.Instance.Param.Color));
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

        #region 视图交互
        private void hWindowControl1_MouseMove(object sender, GrayValueInfoEventArgs e)
        {
            try
            {
                if (e.GaryValue.Length > 0)
                    this.灰度值1Label.Text = e.GaryValue[0].ToString();
                else
                    this.灰度值1Label.Text = 0.ToString();
                ///////////////////////////////////////////
                if (e.GaryValue.Length > 1)
                    this.灰度值2Label.Text = e.GaryValue[1].ToString();
                else
                    this.灰度值2Label.Text = 0.ToString();
                /////////////////////////////////////////
                if (e.GaryValue.Length > 2)
                    this.灰度值3Label.Text = e.GaryValue[2].ToString();
                else
                    this.灰度值3Label.Text = 0.ToString();
                ///////////////////////////////////////////////
                this._curRow = e.Row;
                this._curCol = e.Col;
                this.行坐标Label.Text = "Row:" + e.Row.ToString();
                this.列坐标Label.Text = "Col:" + e.Col.ToString();
            }
            catch
            {
            }
        }

        public void DisplayExcuteResult(object sender, ExcuteCompletedEventArgs e)  // 这里一定要加一个键值对变量来收集，以免出现重合 ImageAcqCompleteEventArgs e
        {
            try
            {
                //if (SystemParamManager.Instance.SysConfigParam.IsAutoRun) return; // 如果在运行状态，不更新图像窗口界面
                if (!IsSelect()) return; // 如果不是当前选择的，则返回
                if (e.DataContent == null) return;// 在图形窗口只显示世界坐标元素，像素元素在图像窗口显示
                /////////////////////////////////////////////
                lock (this.lockSyn)
                {
                    switch (e.DataContent.GetType().Name) //这里只接受XLD轮廓或3D对象轮廓
                    {
                        case nameof(ImageDataClass):
                            this.listData.Clear();
                            this.drawObject.ClearViewObject();
                            this.drawObject.BackImage = (ImageDataClass)e.DataContent;
                            this.CurrentImageData = this.drawObject.BackImage;
                            break;
                        case nameof(RegionDataClass):
                            if (this.listData.ContainsKey(e.ItemName))
                                this.listData[e.ItemName] = ((RegionDataClass)e.DataContent);
                            else
                                this.listData.Add(e.ItemName, ((RegionDataClass)e.DataContent));
                            //this.drawObject.AddViewObject(new ViewData(((RegionDataClass)e.DataContent)?.Region, "red"));
                            break;
                        case nameof(XldDataClass):
                            if (this.listData.ContainsKey(e.ItemName))
                                this.listData[e.ItemName] = ((XldDataClass)e.DataContent);//.HXldCont;
                            else
                                this.listData.Add(e.ItemName, ((XldDataClass)e.DataContent)); //.HXldCont
                            //this.drawObject.AddViewObject(new ViewData(((XldDataClass)e.DataContent)?.HXldCont, "red"));
                            break;
                        case nameof(HXLDCont):
                            if (this.listData.ContainsKey(e.ItemName))
                                this.listData[e.ItemName] = (HXLDCont)e.DataContent;
                            else
                                this.listData.Add(e.ItemName, (HXLDCont)e.DataContent);
                            //this.drawObject.AddViewObject(new ViewData(((HXLDCont)e.DataContent), "green"));
                            break;
                        case nameof(userWcsCircle):
                            //this.RemoveItem(e.ItemName);
                            userWcsCircle wcsCircle = (userWcsCircle)e.DataContent;
                            if (this.listData.ContainsKey(e.ItemName))
                                this.listData[e.ItemName] = wcsCircle.GetPixCircle().GetXLD();
                            else
                                this.listData.Add(e.ItemName, wcsCircle.GetPixCircle().GetXLD());
                            //this.drawObject.AddViewObject(new ViewData(wcsCircle.GetPixCircle().GetXLD(), "green"));
                            /// 添加点
                            if (wcsCircle.EdgesPoint_xyz != null && this._viewConfigParam.IsShowCross)
                            {

                                for (int i = 0; i < wcsCircle.EdgesPoint_xyz.Length; i++)
                                {
                                    if (this.listData.ContainsKey(e.ItemName + "_" + i.ToString()))
                                        this.listData[e.ItemName + "_" + i.ToString()] = wcsCircle.EdgesPoint_xyz[i].GetPixPoint();
                                    else
                                        this.listData.Add(e.ItemName + "_" + i.ToString(), wcsCircle.EdgesPoint_xyz[i].GetPixPoint());
                                    //////////////////////////////////////////////////////////
                                    //this.drawObject.AddViewObject(new ViewData(wcsCircle.EdgesPoint_xyz[i].GetPixPoint(), "green"));
                                }
                            }
                            break;
                        case nameof(userWcsCircleSector):
                            //this.RemoveItem(e.ItemName);
                            userWcsCircleSector wcsCircleSector = ((userWcsCircleSector)e.DataContent);
                            if (this.listData.ContainsKey(e.ItemName))
                                this.listData[e.ItemName] = wcsCircleSector.GetPixCircleSector().GetXLD();
                            else
                                this.listData.Add(e.ItemName, wcsCircleSector.GetPixCircleSector().GetXLD());
                            //////////////////////////////////////////////////////////
                            //this.drawObject.AddViewObject(new ViewData(wcsCircleSector.GetPixCircleSector().GetXLD(), "green"));
                            /// 添加点
                            if (wcsCircleSector.EdgesPoint_xyz != null && this._viewConfigParam.IsShowCross)
                            {

                                for (int i = 0; i < wcsCircleSector.EdgesPoint_xyz.Length; i++)
                                {
                                    if (this.listData.ContainsKey(e.ItemName + "_" + i.ToString()))
                                        this.listData[e.ItemName + "_" + i.ToString()] = wcsCircleSector.EdgesPoint_xyz[i].GetPixPoint();
                                    else
                                        this.listData.Add(e.ItemName + "_" + i.ToString(), wcsCircleSector.EdgesPoint_xyz[i].GetPixPoint());
                                    //////////////////////////////////////////////////////////
                                    //this.drawObject.AddViewObject(new ViewData(wcsCircleSector.EdgesPoint_xyz[i].GetPixPoint(), "green"));
                                }
                            }
                            break;
                        case nameof(userWcsEllipse):
                            //this.RemoveItem(e.ItemName);
                            userWcsEllipse wcsEllipse = ((userWcsEllipse)e.DataContent);
                            if (this.listData.ContainsKey(e.ItemName))
                                this.listData[e.ItemName] = wcsEllipse.GetPixEllipse().GetXLD();
                            else
                                this.listData.Add(e.ItemName, wcsEllipse.GetPixEllipse().GetXLD());
                            //this.drawObject.AddViewObject(new ViewData(wcsEllipse.GetPixEllipse().GetXLD(), "green"));
                            /// 添加点
                            if (wcsEllipse.EdgesPoint_xyz != null && this._viewConfigParam.IsShowCross)
                            {

                                for (int i = 0; i < wcsEllipse.EdgesPoint_xyz.Length; i++)
                                {
                                    if (this.listData.ContainsKey(e.ItemName + "_" + i.ToString()))
                                        this.listData[e.ItemName + "_" + i.ToString()] = wcsEllipse.EdgesPoint_xyz[i].GetPixPoint();
                                    else
                                        this.listData.Add(e.ItemName + "_" + i.ToString(), wcsEllipse.EdgesPoint_xyz[i].GetPixPoint());
                                    ////////////////////////////////////////////////
                                    //this.drawObject.AddViewObject(new ViewData(wcsEllipse.EdgesPoint_xyz[i].GetPixPoint(), "green"));
                                }
                            }
                            break;
                        case nameof(userWcsEllipseSector):
                            //this.RemoveItem(e.ItemName);
                            userWcsEllipseSector wcsEllipseSector = ((userWcsEllipseSector)e.DataContent);
                            if (this.listData.ContainsKey(e.ItemName))
                                this.listData[e.ItemName] = wcsEllipseSector.GetPixEllipseSector().GetXLD();
                            else
                                this.listData.Add(e.ItemName, wcsEllipseSector.GetPixEllipseSector().GetXLD());
                            //this.drawObject.AddViewObject(new ViewData(wcsEllipseSector.GetPixEllipseSector().GetXLD(), "green"));
                            /// 添加点
                            if (wcsEllipseSector.EdgesPoint_xyz != null && this._viewConfigParam.IsShowCross)
                            {
                                for (int i = 0; i < wcsEllipseSector.EdgesPoint_xyz.Length; i++)
                                {
                                    if (this.listData.ContainsKey(e.ItemName + "_" + i.ToString()))
                                        this.listData[e.ItemName + "_" + i.ToString()] = wcsEllipseSector.EdgesPoint_xyz[i].GetPixPoint();
                                    else
                                        this.listData.Add(e.ItemName + "_" + i.ToString(), wcsEllipseSector.EdgesPoint_xyz[i].GetPixPoint());
                                    ////////////////////////////////////////////////
                                    ////  this.drawObject.AddViewObject(new ViewData(wcsEllipseSector.EdgesPoint_xyz[i].GetPixPoint(), "green"));
                                }
                            }
                            break;
                        case nameof(userWcsLine):
                            //this.RemoveItem(e.ItemName);
                            userWcsLine wcsLine = ((userWcsLine)e.DataContent);
                            if (this.listData.ContainsKey(e.ItemName))
                                this.listData[e.ItemName] = wcsLine.GetPixLine().GetXLD();
                            else
                                this.listData.Add(e.ItemName, wcsLine.GetPixLine().GetXLD());
                            //this.drawObject.AddViewObject(new ViewData(wcsLine.GetPixLine().GetXLD(), "green"));
                            /// 添加点
                            if (wcsLine.EdgesPoint_xyz != null && this._viewConfigParam.IsShowCross)
                            {

                                for (int i = 0; i < wcsLine.EdgesPoint_xyz.Length; i++)
                                {
                                    if (this.listData.ContainsKey(e.ItemName + "_" + i.ToString()))
                                        this.listData[e.ItemName + "_" + i.ToString()] = wcsLine.EdgesPoint_xyz[i].GetPixPoint();
                                    else
                                        this.listData.Add(e.ItemName + "_" + i.ToString(), wcsLine.EdgesPoint_xyz[i].GetPixPoint());
                                    ////////////////////////////////////////////////
                                    // this.drawObject.AddViewObject(new ViewData(wcsLine.EdgesPoint_xyz[i].GetPixPoint(), "green"));
                                }
                            }
                            break;

                        case nameof(userWcsArrow):
                            //this.RemoveItem(e.ItemName);
                            userWcsArrow wcsArrow = ((userWcsArrow)e.DataContent);
                            if (this.listData.ContainsKey(e.ItemName))
                                this.listData[e.ItemName] = wcsArrow.GetPixArrow();
                            else
                                this.listData.Add(e.ItemName, wcsArrow.GetPixArrow());
                            break;

                        case nameof(userWcsPoint):
                            //this.RemoveItem(e.ItemName);
                            userWcsPoint wcsPoint = ((userWcsPoint)e.DataContent);
                            if (this.listData.ContainsKey(e.ItemName))
                                this.listData[e.ItemName] = wcsPoint.GetPixPoint();
                            else
                                this.listData.Add(e.ItemName, wcsPoint.GetPixPoint());
                            //this.drawObject.AddViewObject(new ViewData(wcsPoint.GetPixPoint().GetXLD(), "green"));
                            break;
                        case nameof(userWcsVector):
                            //this.RemoveItem(e.ItemName);
                            userWcsVector wcsVector = ((userWcsVector)e.DataContent);
                            userWcsPoint wcsPoint1 = new userWcsPoint(wcsVector.X, wcsVector.Y, wcsVector.Z, wcsVector.CamParams);
                            wcsPoint1.Grab_x = wcsVector.Grab_x;
                            wcsPoint1.Grab_y = wcsVector.Grab_y;
                            wcsPoint1.Grab_z = wcsVector.Grab_z;
                            wcsPoint1.Grab_theta = wcsVector.Grab_theta;
                            if (this.listData.ContainsKey(e.ItemName))
                                this.listData[e.ItemName] = wcsPoint1.GetPixPoint();
                            else
                                this.listData.Add(e.ItemName, wcsPoint1.GetPixPoint());
                            //this.drawObject.AddViewObject(new ViewData(wcsPoint.GetPixPoint().GetXLD(), "green"));
                            break;
                        case nameof(userWcsRectangle1):
                            //this.RemoveItem(e.ItemName);
                            userWcsRectangle1 wcsRectangle1 = ((userWcsRectangle1)e.DataContent);
                            if (this.listData.ContainsKey(e.ItemName))
                                this.listData[e.ItemName] = wcsRectangle1.GetPixRectangle1().GetXLD();
                            else
                                this.listData.Add(e.ItemName, wcsRectangle1.GetPixRectangle1().GetXLD());
                            // this.drawObject.AddViewObject(new ViewData(wcsRectangle1.GetPixRectangle1().GetXLD(), "green"));
                            /// 添加点
                            if (wcsRectangle1.EdgesPoint_xyz != null && this._viewConfigParam.IsShowCross)
                            {
                                for (int i = 0; i < wcsRectangle1.EdgesPoint_xyz.Length; i++)
                                {
                                    if (this.listData.ContainsKey(e.ItemName + "_" + i.ToString()))
                                        this.listData[e.ItemName + "_" + i.ToString()] = wcsRectangle1.EdgesPoint_xyz[i].GetPixPoint();
                                    else
                                        this.listData.Add(e.ItemName + "_" + i.ToString(), wcsRectangle1.EdgesPoint_xyz[i].GetPixPoint());
                                    ////////////////////////////////////////////
                                    //this.drawObject.AddViewObject(new ViewData(wcsRectangle1.EdgesPoint_xyz[i].GetPixPoint(), "green"));
                                }
                            }
                            break;
                        case nameof(userWcsRectangle2):
                            //this.RemoveItem(e.ItemName);
                            userWcsRectangle2 wcsRect2 = ((userWcsRectangle2)e.DataContent);
                            if (this.listData.ContainsKey(e.ItemName))
                                this.listData[e.ItemName] = wcsRect2.GetPixRectangle2().GetXLD();
                            else
                                this.listData.Add(e.ItemName, wcsRect2.GetPixRectangle2().GetXLD());
                            //this.drawObject.AddViewObject(new ViewData(wcsRect2.GetPixRectangle2().GetXLD(), "green"));
                            /// 添加点
                            if (wcsRect2.EdgesPoint_xyz != null && this._viewConfigParam.IsShowCross)
                            {
                                for (int i = 0; i < wcsRect2.EdgesPoint_xyz.Length; i++)
                                {
                                    if (this.listData.ContainsKey(e.ItemName + "_" + i.ToString()))
                                        this.listData[e.ItemName + "_" + i.ToString()] = wcsRect2.EdgesPoint_xyz[i].GetPixPoint();
                                    else
                                        this.listData.Add(e.ItemName + "_" + i.ToString(), wcsRect2.EdgesPoint_xyz[i].GetPixPoint());
                                    ////////////////////////////////////////////
                                    //this.drawObject.AddViewObject(new ViewData(wcsRect2.EdgesPoint_xyz[i].GetPixPoint(), "green"));
                                }
                            }
                            break;

                        case nameof(userWcsPolyLine):
                            //this.RemoveItem(e.ItemName);
                            userWcsPolyLine userWcsPoly = ((userWcsPolyLine)e.DataContent);
                            if (this.listData.ContainsKey(e.ItemName))
                                this.listData[e.ItemName] = userWcsPoly.GetPixPolyLine().GetXLD();
                            else
                                this.listData.Add(e.ItemName, userWcsPoly.GetPixPolyLine().GetXLD());
                            /// 添加点
                            if (userWcsPoly.X != null && this._viewConfigParam.IsShowCross)
                            {
                                for (int i = 0; i < userWcsPoly.X.Count; i++)
                                {
                                    if (this.listData.ContainsKey(e.ItemName + "_" + i.ToString()))
                                        this.listData[e.ItemName + "_" + i.ToString()] = new userWcsPoint(userWcsPoly.X[i], userWcsPoly.Y[i], 0, userWcsPoly.Grab_x, userWcsPoly.Grab_y, userWcsPoly.CamParams).GetPixPoint();
                                    else
                                        this.listData.Add(e.ItemName + "_" + i.ToString(), new userWcsPoint(userWcsPoly.X[i], userWcsPoly.Y[i], 0, userWcsPoly.Grab_x, userWcsPoly.Grab_y, userWcsPoly.CamParams).GetPixPoint());
                                }
                            }
                            break;

                        case nameof(userWcsPolygon):
                            //this.RemoveItem(e.ItemName);
                            userWcsPolygon wcsPolygon = ((userWcsPolygon)e.DataContent);
                            if (this.listData.ContainsKey(e.ItemName))
                                this.listData[e.ItemName] = wcsPolygon.GetPixPolygon().GetXLD();
                            else
                                this.listData.Add(e.ItemName, wcsPolygon.GetPixPolygon().GetXLD());
                            /// 添加点
                            if (wcsPolygon.X != null && this._viewConfigParam.IsShowCross)
                            {
                                for (int i = 0; i < wcsPolygon.X.Count; i++)
                                {
                                    if (this.listData.ContainsKey(e.ItemName + "_" + i.ToString()))
                                        this.listData[e.ItemName + "_" + i.ToString()] = new userWcsPoint(wcsPolygon.X[i], wcsPolygon.Y[i], 0, wcsPolygon.Grab_x, wcsPolygon.Grab_y, wcsPolygon.CamParams).GetPixPoint();
                                    else
                                        this.listData.Add(e.ItemName + "_" + i.ToString(), new userWcsPoint(wcsPolygon.X[i], wcsPolygon.Y[i], 0, wcsPolygon.Grab_x, wcsPolygon.Grab_y, wcsPolygon.CamParams).GetPixPoint());
                                }
                            }
                            break;

                        case nameof(userWcsCoordSystem):
                            if (this.listData.ContainsKey(e.ItemName))
                                this.listData[e.ItemName] = e.DataContent;
                            else
                                this.listData.Add(e.ItemName, e.DataContent);

                            //this.drawObject.AddViewObject(new ViewData(e.DataContent, "green"));
                            break;

                        case nameof(userPixCoordSystem):
                            userPixCoordSystem pixCoordSystem = e.DataContent as userPixCoordSystem;
                            if (this.listData.ContainsKey(e.ItemName))
                                this.listData[e.ItemName] = pixCoordSystem.GetWcsCoordSystem();
                            else
                                this.listData.Add(e.ItemName, pixCoordSystem.GetWcsCoordSystem());
                            break;

                        case nameof(userOkNgText):
                            if (this.listData.ContainsKey(e.ItemName))
                                this.listData[e.ItemName] = e.DataContent;
                            else
                                this.listData.Add(e.ItemName, e.DataContent);
                            //this.drawObject.AddViewObject(new ViewData(e.DataContent, "green"));
                            ///////////////////////////////////////////////////////
                            int row, col, row222, col222;
                            this.hWindowControl1.HalconWindow.GetPart(out row, out col, out row222, out col222);
                            userOkNgText OkNGText = (userOkNgText)e.DataContent;
                            switch (GlobalVariable.pConfig.OKNgPosition)
                            {
                                case enFontPosition.左上角:
                                    (OkNGText).WriteString(this.hWindowControl1.HalconWindow, row + GlobalVariable.pConfig.OKNgRowOffset, col + GlobalVariable.pConfig.OKNgColOffset, GlobalVariable.pConfig.OKNgSize); // 字体放在右上角,文本的参考角为左上角,在图形窗口中不能在代码定文本位置
                                    break;
                                case enFontPosition.右上角:
                                    (OkNGText).WriteString(this.hWindowControl1.HalconWindow, row + GlobalVariable.pConfig.OKNgRowOffset, col222 - GlobalVariable.pConfig.OKNgColOffset, GlobalVariable.pConfig.OKNgSize); // 字体放在右上角,文本的参考角为左上角,在图形窗口中不能在代码定文本位置
                                    break;
                                case enFontPosition.左下角:
                                    (OkNGText).WriteString(this.hWindowControl1.HalconWindow, row222 - GlobalVariable.pConfig.OKNgRowOffset, col + GlobalVariable.pConfig.OKNgColOffset, GlobalVariable.pConfig.OKNgSize); // 字体放在右上角,文本的参考角为左上角,在图形窗口中不能在代码定文本位置
                                    break;
                                case enFontPosition.右下角:
                                    (OkNGText).WriteString(this.hWindowControl1.HalconWindow, row222 - GlobalVariable.pConfig.OKNgRowOffset, col222 - GlobalVariable.pConfig.OKNgColOffset, GlobalVariable.pConfig.OKNgSize); // 字体放在右上角,文本的参考角为左上角,在图形窗口中不能在代码定文本位置
                                    break;
                            }
                            // this.drawObject.AddViewObject(OkNGText, "red");
                            break;

                        case nameof(userTextLable):
                            if (this.listData.ContainsKey(e.ItemName))
                                this.listData[e.ItemName] = e.DataContent;
                            else
                                this.listData.Add(e.ItemName, e.DataContent);
                            this.drawObject.AddViewObject(new ViewData(e.DataContent, "green"));
                            ////////////////////////////////////////////
                            //userTextLable lable1 = this.listData[e.ItemName] as userTextLable;
                            //lable1.WriteString(this.hWindowControl1.HalconWindow);
                            //this.drawObject.AddViewObject(lable1, "green");
                            break;

                        case "userTextLable[]":
                            userTextLable[] lable = e.DataContent as userTextLable[];
                            if (lable != null)
                            {
                                for (int i = 0; i < lable.Length; i++)
                                {
                                    if (this.listData.ContainsKey(e.ItemName + i.ToString()))
                                        this.listData[e.ItemName + i.ToString()] = lable[i];
                                    else
                                        this.listData.Add(e.ItemName + i.ToString(), lable[i]);
                                    //this.drawObject.AddViewObject(new ViewData(lable[i], "green"));
                                }
                            }
                            break;
                    }
                    ///////////////////////////////////////
                    // this.hWindowControl1.HalconWindow.DispObj(this.listData[e.ItemName] as HObject);
                }
                /////////////////////////////
                //Task.Run(() =>
                //{
                lock (this.lockSyn)
                {
                    this.drawObject.AttachPropertyData.Clear();
                    //this.drawObject.ClearViewObject();
                    foreach (KeyValuePair<string, object> item in this.listData)
                    {
                        this.drawObject.AttachPropertyData.Add(item.Value);
                        //this.drawObject.AddViewObject(new ViewData(item.Value));
                    }
                    this.drawObject.DetachDrawingObjectFromWindow();
                }
                //});
            }
            catch (Exception ex)
            {
                new Common.UserMessageForm(ex.ToString()).ShowDialog();
            }
        }

        private void RemoveItem(string name)
        {
            string[] kes = new string[this.listData.Count];
            this.listData.Keys.CopyTo(kes, 0);
            foreach (var item in kes)
            {
                if (item.Contains(name)) // 旧的项中包含了指定项，则先删除!
                    this.listData.Remove(item);
            }
        }
        private void AddData(string key, object value)
        {
            lock (this.lockSyn)
            {
                if (this.listData.ContainsKey(key))
                    this.listData[key] = value;
                else
                    this.listData.Add(key, value);
            }
        }

        public void DisplayClickObject(object sender, TreeNodeMouseClickEventArgs e)  //
        {
            if (!IsSelect()) return; // 如果不是当前选择的，则返回
            if (e.Node.Tag == null) return;
            if (e.Button != MouseButtons.Left) return; // 点击右键时不变
            this._refNode = e.Node;
            try
            {
                switch (e.Node.Tag.GetType().Name)
                {
                    case nameof(CircleMeasure):
                        this.drawObject?.AttachPropertyData.Clear();
                        if (!(this.drawObject is userDrawCircleMeasure))
                        {
                            this.drawObject.GrayValueInfo -= new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                            this.drawObject.ClearDrawingObject();
                            this.drawObject = new userDrawCircleMeasure(this.hWindowControl1, ((CircleMeasure)e.Node.Tag).FindCircle.CirclePixPosition, ((CircleMeasure)e.Node.Tag).PixCoordSystem);
                            this.drawObject.GrayValueInfo += new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                        }
                        this.drawObject.AttachPropertyData.Clear();
                        foreach (KeyValuePair<string, object> item in this.listData)
                        {
                            if (item.Key != e.Node.Text)
                                this.drawObject.AttachPropertyData.Add(item.Value);
                        }
                        this.drawObject.SetParam(((CircleMeasure)e.Node.Tag).PixCoordSystem);
                        this.drawObject.SetParam(((CircleMeasure)e.Node.Tag).FindCircle.CirclePixPosition.AffineTransPixCircle(((CircleMeasure)e.Node.Tag).PixCoordSystem?.GetVariationHomMat2D()));
                        this.drawObject.BackImage = ((CircleMeasure)e.Node.Tag).ImageData != null ? ((CircleMeasure)e.Node.Tag).ImageData : this.CurrentImageData;
                        this.drawObject.AttachDrawingObjectToWindow();
                        this.metrolegyParamForm.drawObject = this.drawObject;
                        this._currFunction = (CircleMeasure)e.Node.Tag;
                        //DisplayClickItem(sender, e);
                        break;
                    case nameof(CircleSectorMeasure):
                        this.drawObject?.AttachPropertyData.Clear();
                        if (!(this.drawObject is userDrawCircleSectorMeasure))
                        {
                            this.drawObject.GrayValueInfo -= new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                            this.drawObject.ClearDrawingObject();
                            this.drawObject = new userDrawCircleSectorMeasure(this.hWindowControl1, ((CircleSectorMeasure)e.Node.Tag).FindCircleSector.CircleSectorPixPosition, ((CircleSectorMeasure)e.Node.Tag).PixCoordSystem);
                            this.drawObject.GrayValueInfo += new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                        }
                        this.drawObject.AttachPropertyData.Clear();
                        foreach (KeyValuePair<string, object> item in this.listData)
                        {
                            if (item.Key != e.Node.Text)
                                this.drawObject.AttachPropertyData.Add(item.Value);
                        }
                        this.drawObject.SetParam(((CircleSectorMeasure)e.Node.Tag).PixCoordSystem);
                        this.drawObject.SetParam(((CircleSectorMeasure)e.Node.Tag).FindCircleSector.CircleSectorPixPosition.AffineTransPixCircleSector(((CircleSectorMeasure)e.Node.Tag).PixCoordSystem?.GetVariationHomMat2D()));
                        this.drawObject.BackImage = ((CircleSectorMeasure)e.Node.Tag).ImageData != null ? ((CircleSectorMeasure)e.Node.Tag).ImageData : this.CurrentImageData;
                        this.drawObject.AttachDrawingObjectToWindow();
                        this.metrolegyParamForm.drawObject = this.drawObject;
                        this._currFunction = (CircleSectorMeasure)e.Node.Tag;
                        //DisplayClickItem(sender, e);
                        break;
                    case nameof(EllipseMeasure):
                        this.drawObject?.AttachPropertyData.Clear();
                        if (!(this.drawObject is userDrawEllipseMeasure))
                        {
                            this.drawObject.GrayValueInfo -= new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                            this.drawObject.ClearDrawingObject();
                            this.drawObject = new userDrawEllipseMeasure(this.hWindowControl1, ((EllipseMeasure)e.Node.Tag).FindEllipse.EllipsePixPosition, ((EllipseMeasure)e.Node.Tag).PixCoordSystem);
                            this.drawObject.GrayValueInfo += new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                        }
                        this.drawObject.AttachPropertyData.Clear();
                        foreach (KeyValuePair<string, object> item in this.listData)
                        {
                            if (item.Key != e.Node.Text)
                                this.drawObject.AttachPropertyData.Add(item.Value);
                        }
                        this.drawObject.SetParam(((EllipseMeasure)e.Node.Tag).PixCoordSystem);
                        this.drawObject.SetParam(((EllipseMeasure)e.Node.Tag).FindEllipse.EllipsePixPosition.AffineTransPixEllipse(((EllipseMeasure)e.Node.Tag).PixCoordSystem?.GetVariationHomMat2D()));
                        this.drawObject.BackImage = ((EllipseMeasure)e.Node.Tag).ImageData != null ? ((EllipseMeasure)e.Node.Tag).ImageData : this.CurrentImageData;
                        this.drawObject.AttachDrawingObjectToWindow();
                        this.metrolegyParamForm.drawObject = this.drawObject;
                        this._currFunction = (EllipseMeasure)e.Node.Tag;
                        //DisplayClickItem(sender, e);
                        break;
                    case nameof(EllipseSectorMeasure):
                        this.drawObject?.AttachPropertyData.Clear();
                        if (!(this.drawObject is userDrawEllipseSectorMeasure))
                        {
                            this.drawObject.GrayValueInfo -= new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                            this.drawObject.ClearDrawingObject();
                            this.drawObject = new userDrawEllipseSectorMeasure(this.hWindowControl1, ((EllipseSectorMeasure)e.Node.Tag).FindEllipseSector.EllipseSectorPixPosition, ((EllipseSectorMeasure)e.Node.Tag).PixCoordSystem);
                            this.drawObject.GrayValueInfo += new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                        }
                        this.drawObject.AttachPropertyData.Clear();
                        foreach (KeyValuePair<string, object> item in this.listData)
                        {
                            if (item.Key != e.Node.Text)
                                this.drawObject.AttachPropertyData.Add(item.Value);
                        }
                        this.drawObject.SetParam(((EllipseSectorMeasure)e.Node.Tag).PixCoordSystem);
                        this.drawObject.SetParam(((EllipseSectorMeasure)e.Node.Tag).FindEllipseSector.EllipseSectorPixPosition.AffineTransPixEllipseSector(((EllipseSectorMeasure)e.Node.Tag).PixCoordSystem?.GetVariationHomMat2D()));
                        this.drawObject.BackImage = ((EllipseSectorMeasure)e.Node.Tag).ImageData != null ? ((EllipseSectorMeasure)e.Node.Tag).ImageData : this.CurrentImageData;
                        this.drawObject.AttachDrawingObjectToWindow();
                        this.metrolegyParamForm.drawObject = this.drawObject;
                        this._currFunction = (EllipseSectorMeasure)e.Node.Tag;
                        //DisplayClickItem(sender, e);
                        break;
                    case nameof(LineMeasure):
                        this.drawObject?.AttachPropertyData.Clear();
                        if (!(this.drawObject is userDrawLineMeasure))
                        {
                            this.drawObject.GrayValueInfo -= new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                            this.drawObject.ClearDrawingObject();
                            this.drawObject = new userDrawLineMeasure(this.hWindowControl1, ((LineMeasure)e.Node.Tag).FindLine.LinePixPosition, ((LineMeasure)e.Node.Tag).PixCoordSystem);
                            this.drawObject.GrayValueInfo += new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                        }
                        this.drawObject.AttachPropertyData.Clear();
                        foreach (KeyValuePair<string, object> item in this.listData)
                        {
                            if (item.Key != e.Node.Text)
                                this.drawObject.AttachPropertyData.Add(item.Value);
                        }
                        this.drawObject.SetParam(((LineMeasure)e.Node.Tag).PixCoordSystem);
                        this.drawObject.SetParam(((LineMeasure)e.Node.Tag).FindLine.LinePixPosition.AffinePixLine2D(((LineMeasure)e.Node.Tag).PixCoordSystem?.GetVariationHomMat2D()));
                        this.drawObject.BackImage = ((LineMeasure)e.Node.Tag).ImageData != null ? ((LineMeasure)e.Node.Tag).ImageData : this.CurrentImageData;
                        this.drawObject.AttachDrawingObjectToWindow();
                        this.metrolegyParamForm.drawObject = this.drawObject;
                        this._currFunction = (LineMeasure)e.Node.Tag;
                        //DisplayClickItem(sender, e);
                        break;
                    case nameof(PointMeasure):
                        this.drawObject?.AttachPropertyData.Clear();
                        if (!(this.drawObject is userDrawPointMeasure))
                        {
                            this.drawObject.GrayValueInfo -= new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                            this.drawObject.ClearDrawingObject();
                            this.drawObject = new userDrawPointMeasure(this.hWindowControl1, ((PointMeasure)e.Node.Tag).FindPoint.LinePixPosition, ((PointMeasure)e.Node.Tag).PixCoordSystem);
                            this.drawObject.GrayValueInfo += new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                        }
                        this.drawObject.AttachPropertyData.Clear();
                        foreach (KeyValuePair<string, object> item in this.listData)
                        {
                            if (item.Key != e.Node.Text)
                                this.drawObject.AttachPropertyData.Add(item.Value);
                        }
                        this.drawObject.SetParam(((PointMeasure)e.Node.Tag).PixCoordSystem);
                        this.drawObject.SetParam(((PointMeasure)e.Node.Tag).FindPoint.LinePixPosition.AffinePixLine2D(((PointMeasure)e.Node.Tag).PixCoordSystem?.GetVariationHomMat2D()));
                        this.drawObject.BackImage = ((PointMeasure)e.Node.Tag).ImageData != null ? ((PointMeasure)e.Node.Tag).ImageData : this.CurrentImageData;
                        this.drawObject.AttachDrawingObjectToWindow();
                        this.metrolegyParamForm.drawObject = this.drawObject;
                        this._currFunction = (PointMeasure)e.Node.Tag;
                        //DisplayClickItem(sender, e);
                        break;
                    case nameof(Rectangle2Measure):
                        this.drawObject?.AttachPropertyData.Clear();
                        if (!(this.drawObject is userDrawRect2Measure))
                        {
                            this.drawObject.GrayValueInfo -= new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                            this.drawObject.ClearDrawingObject();
                            this.drawObject = new userDrawRect2Measure(this.hWindowControl1, ((Rectangle2Measure)e.Node.Tag).FindRect2.Rect2PixPosition, ((Rectangle2Measure)e.Node.Tag).PixCoordSystem);
                            this.drawObject.GrayValueInfo += new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                        }
                        this.drawObject.AttachPropertyData.Clear();
                        foreach (KeyValuePair<string, object> item in this.listData)
                        {
                            if (item.Key != e.Node.Text)
                                this.drawObject.AttachPropertyData.Add(item.Value);
                        }
                        this.drawObject.SetParam(((Rectangle2Measure)e.Node.Tag).PixCoordSystem);
                        this.drawObject.SetParam(((Rectangle2Measure)e.Node.Tag).FindRect2.Rect2PixPosition.AffineTransPixRect2(((Rectangle2Measure)e.Node.Tag).PixCoordSystem?.GetVariationHomMat2D())); //?.AffineWcsRectangle2D(((Rectangle2Measure)e.Node.Tag).WcsCoordSystem.GetVariationHomMat2D())
                        this.drawObject.BackImage = ((Rectangle2Measure)e.Node.Tag).ImageData != null ? ((Rectangle2Measure)e.Node.Tag).ImageData : this.CurrentImageData;
                        this.drawObject.AttachDrawingObjectToWindow();
                        this.metrolegyParamForm.drawObject = this.drawObject;
                        this._currFunction = (Rectangle2Measure)e.Node.Tag;
                        //DisplayClickItem(sender, e);
                        break;
                    case nameof(WidthMeasure):
                        this.drawObject?.AttachPropertyData.Clear();
                        if (!(this.drawObject is userDrawWidthMeasure))
                        {
                            this.drawObject.GrayValueInfo -= new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                            this.drawObject.ClearDrawingObject();
                            this.drawObject = new userDrawWidthMeasure(this.hWindowControl1, ((WidthMeasure)e.Node.Tag).FindWidth.Rect2PixPosition, ((WidthMeasure)e.Node.Tag).PixCoordSystem);
                            this.drawObject.GrayValueInfo += new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                        }
                        this.drawObject.AttachPropertyData.Clear();
                        foreach (KeyValuePair<string, object> item in this.listData)
                        {
                            if (item.Key != e.Node.Text)
                                this.drawObject.AttachPropertyData.Add(item.Value);
                        }
                        this.drawObject.SetParam(((WidthMeasure)e.Node.Tag).PixCoordSystem);
                        this.drawObject.SetParam(((WidthMeasure)e.Node.Tag).FindWidth.Rect2PixPosition.AffineTransPixRect2(((WidthMeasure)e.Node.Tag).PixCoordSystem?.GetVariationHomMat2D())); //?.AffineWcsRectangle2D(((Rectangle2Measure)e.Node.Tag).WcsCoordSystem.GetVariationHomMat2D())
                        this.drawObject.BackImage = ((WidthMeasure)e.Node.Tag).ImageData != null ? ((WidthMeasure)e.Node.Tag).ImageData : this.CurrentImageData;
                        this.drawObject.AttachDrawingObjectToWindow();
                        this.metrolegyParamForm.drawObject = this.drawObject;
                        this._currFunction = (WidthMeasure)e.Node.Tag;
                        //DisplayClickItem(sender, e);
                        break;
                    case nameof(CrossPointMeasure):
                        this.drawObject?.AttachPropertyData.Clear();
                        if (!(this.drawObject is userDrawCrossMeasure))
                        {
                            this.drawObject.GrayValueInfo -= new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                            this.drawObject.ClearDrawingObject();
                            this.drawObject = new userDrawCrossMeasure(this.hWindowControl1, ((CrossPointMeasure)e.Node.Tag).FindCrossPoint.LinePixPosition, ((CrossPointMeasure)e.Node.Tag).PixCoordSystem);
                            this.drawObject.GrayValueInfo += new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                        }
                        this.drawObject.AttachPropertyData.Clear();
                        foreach (KeyValuePair<string, object> item in this.listData)
                        {
                            if (item.Key != e.Node.Text)
                                this.drawObject.AttachPropertyData.Add(item.Value);
                        }
                        this.drawObject.SetParam(((CrossPointMeasure)e.Node.Tag).PixCoordSystem);
                        this.drawObject.SetParam(((CrossPointMeasure)e.Node.Tag).FindCrossPoint.LinePixPosition.AffinePixLine2D(((CrossPointMeasure)e.Node.Tag).PixCoordSystem.GetVariationHomMat2D()));
                        this.drawObject.BackImage = ((CrossPointMeasure)e.Node.Tag).ImageData != null ? ((CrossPointMeasure)e.Node.Tag).ImageData : this.CurrentImageData;
                        this.drawObject.AttachDrawingObjectToWindow();
                        this.metrolegyParamForm.drawObject = this.drawObject;
                        this._currFunction = (CrossPointMeasure)e.Node.Tag;
                        //DisplayClickItem(sender, e);
                        break;
                    case nameof(PolyLineMeasure):
                        this.drawObject?.AttachPropertyData.Clear();
                        if (!(this.drawObject is userDrawPolyLineMeasure))
                        {
                            this.drawObject.GrayValueInfo -= new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                            this.drawObject.ClearDrawingObject();
                            this.drawObject = new userDrawPolyLineMeasure(this.hWindowControl1, ((PolyLineMeasure)e.Node.Tag).FindPolyLine.PolyLinePixPosition, ((PolyLineMeasure)e.Node.Tag).PixCoordSystem);
                            this.drawObject.GrayValueInfo += new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                        }
                        this.drawObject.AttachPropertyData.Clear();
                        foreach (KeyValuePair<string, object> item in this.listData)
                        {
                            if (item.Key != e.Node.Text)
                                this.drawObject.AttachPropertyData.Add(item.Value);
                        }
                        this.drawObject.SetParam(((PolyLineMeasure)e.Node.Tag).PixCoordSystem);
                        this.drawObject.SetParam(((PolyLineMeasure)e.Node.Tag).FindPolyLine.PolyLinePixPosition.AffinePixPolyLine(((PolyLineMeasure)e.Node.Tag).PixCoordSystem.GetVariationHomMat2D()));
                        this.drawObject.BackImage = ((PolyLineMeasure)e.Node.Tag).ImageData != null ? ((PolyLineMeasure)e.Node.Tag).ImageData : this.CurrentImageData;
                        this.drawObject.AttachDrawingObjectToWindow();
                        this.metrolegyParamForm.drawObject = this.drawObject;
                        this._currFunction = (PolyLineMeasure)e.Node.Tag;
                        //DisplayClickItem(sender, e);
                        break;
                    case nameof(PolygonMeasure):
                        this.drawObject?.AttachPropertyData.Clear();
                        if (!(this.drawObject is userDrawPolygonMeasure))
                        {
                            this.drawObject.GrayValueInfo -= new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                            this.drawObject.ClearDrawingObject();
                            this.drawObject = new userDrawPolygonMeasure(this.hWindowControl1, ((PolygonMeasure)e.Node.Tag).FindPolygon.PolygonPixPosition, ((PolygonMeasure)e.Node.Tag).PixCoordSystem);
                            this.drawObject.GrayValueInfo += new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                        }
                        this.drawObject.AttachPropertyData.Clear();
                        foreach (KeyValuePair<string, object> item in this.listData)
                        {
                            if (item.Key != e.Node.Text)
                                this.drawObject.AttachPropertyData.Add(item.Value);
                        }
                        this.drawObject.SetParam(((PolygonMeasure)e.Node.Tag).PixCoordSystem);
                        this.drawObject.SetParam(((PolygonMeasure)e.Node.Tag).FindPolygon.PolygonPixPosition.AffinePixPolygon(((PolygonMeasure)e.Node.Tag).PixCoordSystem.GetVariationHomMat2D()));
                        this.drawObject.BackImage = ((PolygonMeasure)e.Node.Tag).ImageData != null ? ((PolygonMeasure)e.Node.Tag).ImageData : this.CurrentImageData;
                        this.drawObject.AttachDrawingObjectToWindow();
                        this.metrolegyParamForm.drawObject = this.drawObject;
                        this._currFunction = (PolygonMeasure)e.Node.Tag;
                        //DisplayClickItem(sender, e);
                        break;
                    case nameof(ManualPointMeasure):
                        this.drawObject?.AttachPropertyData.Clear();
                        if (!(this.drawObject is userDrawManualPointMeasure))
                        {
                            this.drawObject.GrayValueInfo -= new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                            this.drawObject.ClearDrawingObject();
                            this.drawObject = new userDrawManualPointMeasure(this.hWindowControl1, ((ManualPointMeasure)e.Node.Tag).FindCrossPoint.PointPixPosition, ((ManualPointMeasure)e.Node.Tag).PixCoordSystem);
                            this.drawObject.GrayValueInfo += new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                        }
                        this.drawObject.AttachPropertyData.Clear();
                        foreach (KeyValuePair<string, object> item in this.listData)
                        {
                            if (item.Key != e.Node.Text)
                                this.drawObject.AttachPropertyData.Add(item.Value);
                        }
                        this.drawObject.SetParam(((ManualPointMeasure)e.Node.Tag).PixCoordSystem);
                        this.drawObject.SetParam(((ManualPointMeasure)e.Node.Tag).FindCrossPoint.PointPixPosition.AffineTransPixPoint(((ManualPointMeasure)e.Node.Tag).PixCoordSystem.GetVariationHomMat2D()));
                        this.drawObject.BackImage = ((ManualPointMeasure)e.Node.Tag).ImageData != null ? ((ManualPointMeasure)e.Node.Tag).ImageData : this.CurrentImageData;
                        this.drawObject.AttachDrawingObjectToWindow();
                        this.metrolegyParamForm.drawObject = this.drawObject;
                        this._currFunction = (ManualPointMeasure)e.Node.Tag;
                        //DisplayClickItem(sender, e);
                        break;
                    case nameof(ManualPolyLineMeasure):
                        this.drawObject?.AttachPropertyData.Clear();
                        if (!(this.drawObject is userDrawManualPolyLineMeasure))
                        {
                            this.drawObject.GrayValueInfo -= new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                            this.drawObject.ClearDrawingObject();
                            this.drawObject = new userDrawManualPolyLineMeasure(this.hWindowControl1, ((ManualPolyLineMeasure)e.Node.Tag).FindPolyLine.PolyLinePixPosition, ((ManualPolyLineMeasure)e.Node.Tag).PixCoordSystem);
                            this.drawObject.GrayValueInfo += new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                        }
                        this.drawObject.AttachPropertyData.Clear();
                        foreach (KeyValuePair<string, object> item in this.listData)
                        {
                            if (item.Key != e.Node.Text)
                                this.drawObject.AttachPropertyData.Add(item.Value);
                        }
                        this.drawObject.SetParam(((ManualPolyLineMeasure)e.Node.Tag).PixCoordSystem);
                        this.drawObject.SetParam(((ManualPolyLineMeasure)e.Node.Tag).FindPolyLine.PolyLinePixPosition.AffinePixPolyLine(((ManualPolyLineMeasure)e.Node.Tag).PixCoordSystem.GetVariationHomMat2D()));
                        this.drawObject.BackImage = ((ManualPolyLineMeasure)e.Node.Tag).ImageData != null ? ((ManualPolyLineMeasure)e.Node.Tag).ImageData : this.CurrentImageData;
                        this.drawObject.AttachDrawingObjectToWindow();
                        this.metrolegyParamForm.drawObject = this.drawObject;
                        this._currFunction = (ManualPolyLineMeasure)e.Node.Tag;
                        //DisplayClickItem(sender, e);
                        break;
                    case nameof(ManualPolygonMeasure):
                        this.drawObject?.AttachPropertyData.Clear();
                        if (!(this.drawObject is userDrawManualPolygonMeasure))
                        {
                            this.drawObject.GrayValueInfo -= new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                            this.drawObject.ClearDrawingObject();
                            this.drawObject = new userDrawManualPolygonMeasure(this.hWindowControl1, ((ManualPolygonMeasure)e.Node.Tag).FindPolygon.PolygonPixPosition, ((ManualPolygonMeasure)e.Node.Tag).PixCoordSystem);
                            this.drawObject.GrayValueInfo += new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                        }
                        this.drawObject.AttachPropertyData.Clear();
                        foreach (KeyValuePair<string, object> item in this.listData)
                        {
                            if (item.Key != e.Node.Text)
                                this.drawObject.AttachPropertyData.Add(item.Value);
                        }
                        this.drawObject.SetParam(((ManualPolygonMeasure)e.Node.Tag).PixCoordSystem);
                        this.drawObject.SetParam(((ManualPolygonMeasure)e.Node.Tag).FindPolygon.PolygonPixPosition.AffinePixPolygon(((ManualPolygonMeasure)e.Node.Tag).PixCoordSystem.GetVariationHomMat2D()));
                        this.drawObject.BackImage = ((ManualPolygonMeasure)e.Node.Tag).ImageData != null ? ((ManualPolygonMeasure)e.Node.Tag).ImageData : this.CurrentImageData;
                        this.drawObject.AttachDrawingObjectToWindow();
                        this.metrolegyParamForm.drawObject = this.drawObject;
                        this._currFunction = (ManualPolygonMeasure)e.Node.Tag;
                        //DisplayClickItem(sender, e);
                        break;
                    case nameof(ManualCircleSectorMeasure):
                        this.drawObject?.AttachPropertyData.Clear();
                        if (!(this.drawObject is userDrawManualCircleSectorMeasure))
                        {
                            this.drawObject.GrayValueInfo -= new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                            this.drawObject.ClearDrawingObject();
                            this.drawObject = new userDrawManualCircleSectorMeasure(this.hWindowControl1, ((ManualCircleSectorMeasure)e.Node.Tag).FindCircleSector.CircleSectorPixPosition, ((ManualCircleSectorMeasure)e.Node.Tag).PixCoordSystem);
                            this.drawObject.GrayValueInfo += new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                        }
                        this.drawObject.AttachPropertyData.Clear();
                        foreach (KeyValuePair<string, object> item in this.listData)
                        {
                            if (item.Key != e.Node.Text)
                                this.drawObject.AttachPropertyData.Add(item.Value);
                        }
                        this.drawObject.SetParam(((ManualCircleSectorMeasure)e.Node.Tag).PixCoordSystem);
                        this.drawObject.SetParam(((ManualCircleSectorMeasure)e.Node.Tag).FindCircleSector.CircleSectorPixPosition.AffineTransPixCircleSector(((ManualCircleSectorMeasure)e.Node.Tag).PixCoordSystem.GetVariationHomMat2D()));
                        this.drawObject.BackImage = ((ManualCircleSectorMeasure)e.Node.Tag).ImageData != null ? ((ManualCircleSectorMeasure)e.Node.Tag).ImageData : this.CurrentImageData;
                        this.drawObject.AttachDrawingObjectToWindow();
                        this.metrolegyParamForm.drawObject = this.drawObject;
                        this._currFunction = (ManualCircleSectorMeasure)e.Node.Tag;
                        //DisplayClickItem(sender, e);
                        break;
                    ///////////////////////////////////////// 显示测量距离对象
                    case "CircleToCircleDist2D":
                    case "CircleToLineDist2D":
                    case "LineToLineDist2D":
                    case "PointToLineDist2D":
                        // DisplayClickItem(sender, e);
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

        #endregion

        #region 右键菜单项
        private void addContextMenu(HWindowControl hWindowControl)
        {
            ContextMenuStrip ContextMenuStrip1 = new ContextMenuStrip();
            ContextMenuStrip1.Name = hWindowControl.Name;
            // 添加右键菜单 
            ToolStripItem[] items = null;
            switch (SystemParamManager.Instance.SysConfigParam.Language)
            {
                default:
                case "zh-CN":
                    items = new ToolStripMenuItem[]
                    {
                     new ToolStripMenuItem("执行",null,null,"执行"),
                     new ToolStripMenuItem("设置抓边参数",null,null,"设置抓边参数"),
                     new ToolStripMenuItem("边缘点",null,null,"边缘点"),
                     new ToolStripMenuItem("设置绘图参数",null,null,"设置绘图参数"),
                     new ToolStripMenuItem("移动到当前位",null,null,"移动到当前位"), 
                     new ToolStripMenuItem("发送当前坐标",null,null,"发送当前坐标"),
                     new ToolStripMenuItem("------------"),
                     new ToolStripMenuItem("自适应窗口",null,null,"自适应窗口"),
                     new ToolStripMenuItem("清除窗口",null,null,"清除窗口"),
                     new ToolStripMenuItem("保存图像",null,null,"保存图像"),
                     new ToolStripMenuItem("加载图像",null,null,"加载图像"), //
                     new ToolStripMenuItem("设置光标参数",null,null,"设置光标参数"), 
                    };
                    if (this._viewConfigParam.IsShowCross)
                        items[2].Text = "隐藏边缘点";
                    else
                        items[2].Text = "显示边缘点";
                    break;
                case "en-US":
                    items = new ToolStripMenuItem[]
                    {
                     new ToolStripMenuItem("Excute Measure",null,null,"执行"),
                     new ToolStripMenuItem("Set Edge Param",null,null,"设置抓边参数"),
                     new ToolStripMenuItem("Edge Point",null,null,"边缘点"),
                     new ToolStripMenuItem("Set Draw Param",null,null,"设置绘图参数"),
                     new ToolStripMenuItem("Move Position",null,null,"移动到当前位"),
                     new ToolStripMenuItem("Send Cur Position",null,null,"发送当前坐标"),
                     new ToolStripMenuItem("------------"),
                     new ToolStripMenuItem("Auto Window ",null,null,"自适应窗口"),
                     new ToolStripMenuItem("Clear Window",null,null,"清除窗口"),
                     new ToolStripMenuItem("Save Image",null,null,"保存图像"),
                     new ToolStripMenuItem("Load Image",null,null,"加载图像"),
                      new ToolStripMenuItem("Set Cursor Param",null,null,"设置光标参数"),
                    };
                    if (this._viewConfigParam.IsShowCross)
                        items[2].Text = "Hide Edge Point";
                    else
                        items[2].Text = "Show Edge Point";
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
                    case "执行":
                        switch (this._currFunction.GetType().Name)
                        {
                            case nameof(CircleMeasure):
                                this._currFunction?.Execute(this.drawObject.GetPixCircleParam(), this._refNode);
                                //this.drawObject.DetachDrawingObjectFromWindow();
                                break;
                            case nameof(CircleSectorMeasure):
                                this._currFunction?.Execute(this.drawObject.GetPixCircleSectorParam(), this._refNode);
                                break;
                            case nameof(EllipseMeasure):
                                this._currFunction?.Execute(this.drawObject.GetPixEllipseParam(), this._refNode);
                                break;
                            case nameof(EllipseSectorMeasure):
                                this._currFunction?.Execute(this.drawObject.GetPixEllipseSectorParam(), this._refNode);
                                break;
                            case nameof(LineMeasure):
                                this._currFunction?.Execute(this.drawObject.GetPixLineParam(), this._refNode);
                                //this.drawObject.DetachDrawingObjectFromWindow();
                                break;
                            case nameof(PointMeasure):
                                this._currFunction?.Execute(this.drawObject.GetPixLineParam(), this._refNode);
                                break;
                            case nameof(Rectangle2Measure):
                                this._currFunction?.Execute(this.drawObject.GetPixRectangle2Param(), this._refNode);
                                break;
                            case nameof(WidthMeasure):
                                this._currFunction?.Execute(this.drawObject.GetPixRectangle2Param(), this._refNode);
                                break;
                            case nameof(CrossPointMeasure):
                                this._currFunction?.Execute(this.drawObject.GetPixLineParam(), this._refNode);
                                break;
                            case nameof(PolyLineMeasure):
                                this._currFunction?.Execute(this.drawObject.GetPixPolyLineParam(), this._refNode);
                                break;
                            case nameof(PolygonMeasure):
                                this._currFunction?.Execute(this.drawObject.GetPixPolygonParam(), this._refNode);
                                break;
                            case nameof(ManualPointMeasure):
                                this._currFunction?.Execute(this.drawObject.GetPixPointParam(), this._refNode);
                                break;
                            case nameof(ManualPolygonMeasure):
                                this._currFunction?.Execute(this.drawObject.GetPixPolygonParam(), this._refNode);
                                break;
                            case nameof(ManualPolyLineMeasure):
                                this._currFunction?.Execute(this.drawObject.GetPixPolyLineParam(), this._refNode);
                                break;
                            case nameof(ManualCircleSectorMeasure):
                                this._currFunction?.Execute(this.drawObject.GetPixCircleSectorParam(), this._refNode);
                                break;
                        }
                        break;
                    //////////////////////////////////////
                    case "自适应窗口":
                        this.drawObject?.AutoImage();
                        break;
                    case "清除窗口":
                        this.drawObject?.ClearWindow();
                        this.listData.Clear(); // 清除窗口时,对象也清除
                        break;
                    case "边缘点":
                    case "隐藏边缘点":
                    case "显示边缘点":
                        if (this._viewConfigParam.IsShowCross)
                        {
                            e.ClickedItem.Text = "显示边缘点";
                            this._viewConfigParam.IsShowCross = false;
                        }
                        else
                        {
                            e.ClickedItem.Text = "隐藏边缘点";
                            this._viewConfigParam.IsShowCross = true;
                        }
                        ViewConfigParamManager.Instance.Save();
                        break;
                    case "设置抓边参数":
                        MetrolegyParamForm paramForm = new MetrolegyParamForm(this._currFunction, this.drawObject);
                        paramForm.Show();
                        paramForm.Owner = this;
                        break;

                    case "设置绘图参数":
                        switch (this.drawObject?.GetType().Name)
                        {
                            case nameof(userDrawCircleMeasure):

                                break;
                            case nameof(userDrawCircleSectorMeasure):

                                break;

                            case nameof(userDrawEllipseMeasure):

                                break;
                            case nameof(userDrawEllipseSectorMeasure):

                                break;
                            case nameof(userDrawLineMeasure):

                                break;
                            case nameof(userDrawPointMeasure):

                                break;
                            case nameof(userDrawRect2Measure):

                                break;
                            case nameof(userDrawWidthMeasure):

                                break;
                            case nameof(userDrawCrossMeasure):

                                break;
                            case nameof(userDrawPolyLineMeasure):

                                break;
                            case nameof(userDrawPolygonMeasure):

                                break;
                            case nameof(userDrawManualPointMeasure):

                                break;
                            case nameof(userDrawManualPolyLineMeasure):

                                break;
                            case nameof(userDrawManualPolygonMeasure):

                                break;

                            case nameof(userDrawManualCircleSectorMeasure):
                                DrawManualCircleParamForm form = new DrawManualCircleParamForm(this.drawObject);
                                form.ShowDialog();
                                break;

                            default:

                                break;
                        }
                        break;

                    case "移动到当前位":
                        enCoordSysName coordSysName = AcqSourceManage.Instance.GetAcqSource(this.传感器comboBox.SelectedItem.ToString()).CoordSysName;
                        CoordSysAxisPosParam axisParam = new CoordSysAxisPosParam();
                        axisParam.UpdataAxisPosition(coordSysName); // 实时使用当前位置
                        if (this.drawObject.BackImage == null || this.传感器comboBox.SelectedItem.ToString() == "NONE")
                        {
                            LoggerHelper.Info(this._viewConfigParam.ViewName + "->执行移动到当前位功能时,没有采集图像或传感器名称为空", AcqSourceManage.Instance.GetAcqSource(this.传感器comboBox.SelectedItem.ToString())?.Sensor?.Name);
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
                        LoggerHelper.Info(this._viewConfigParam.ViewName + "->功能位置写入:Move", AcqSourceManage.Instance.GetAcqSource(this.传感器comboBox.SelectedItem.ToString())?.Sensor?.Name);
                        LoggerHelper.Info(this._viewConfigParam.ViewName + $"->移动到目标位置:x={Math.Round(x, 5)},y={Math.Round(y, 5)}", AcqSourceManage.Instance.GetAcqSource(this.传感器comboBox.SelectedItem.ToString())?.Sensor?.Name);
                        break;

                    case "发送当前坐标":
                        coordSysName = AcqSourceManage.Instance.GetAcqSource(this.传感器comboBox.SelectedItem.ToString()).CoordSysName;
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
                                break;
                            case nameof(drawWcsRect2):
                                drawWcsRect2 wcsRect2 = this._wcsROI as drawWcsRect2;
                                CommunicationConfigParamManger.Instance.WriteValue(coordSysName, enCommunicationCommand.Compensation_X, wcsRect2.X);
                                CommunicationConfigParamManger.Instance.WriteValue(coordSysName, enCommunicationCommand.Compensation_Y, wcsRect2.Y);
                                CommunicationConfigParamManger.Instance.WriteValue(coordSysName, enCommunicationCommand.ResultToSocket, "OK");
                                CommunicationConfigParamManger.Instance.WriteValue(coordSysName, enCommunicationCommand.ResultToPlc, 1);
                                CommunicationConfigParamManger.Instance.WriteValue(coordSysName, enCommunicationCommand.TriggerToSocket, 1);
                                CommunicationConfigParamManger.Instance.WriteValue(coordSysName, enCommunicationCommand.TriggerToPlc, 1);
                                break;
                            case nameof(drawWcsPoint):
                                drawWcsPoint wcsPoint = this._wcsROI as drawWcsPoint;
                                CommunicationConfigParamManger.Instance.WriteValue(coordSysName, enCommunicationCommand.Compensation_X, wcsPoint.X);
                                CommunicationConfigParamManger.Instance.WriteValue(coordSysName, enCommunicationCommand.Compensation_Y, wcsPoint.Y);
                                CommunicationConfigParamManger.Instance.WriteValue(coordSysName, enCommunicationCommand.ResultToSocket, "OK");
                                CommunicationConfigParamManger.Instance.WriteValue(coordSysName, enCommunicationCommand.ResultToPlc, 1);
                                CommunicationConfigParamManger.Instance.WriteValue(coordSysName, enCommunicationCommand.TriggerToSocket, 1);
                                CommunicationConfigParamManger.Instance.WriteValue(coordSysName, enCommunicationCommand.TriggerToPlc, 1);
                                break;
                            case nameof(drawWcsEllipse):
                                drawWcsEllipse wcsEllipse = this._wcsROI as drawWcsEllipse;
                                CommunicationConfigParamManger.Instance.WriteValue(coordSysName, enCommunicationCommand.Compensation_X, wcsEllipse.X);
                                CommunicationConfigParamManger.Instance.WriteValue(coordSysName, enCommunicationCommand.Compensation_Y, wcsEllipse.Y);
                                CommunicationConfigParamManger.Instance.WriteValue(coordSysName, enCommunicationCommand.ResultToSocket, "OK");
                                CommunicationConfigParamManger.Instance.WriteValue(coordSysName, enCommunicationCommand.ResultToPlc, 1);
                                CommunicationConfigParamManger.Instance.WriteValue(coordSysName, enCommunicationCommand.TriggerToSocket, 1);
                                CommunicationConfigParamManger.Instance.WriteValue(coordSysName, enCommunicationCommand.TriggerToPlc, 1);
                                break;
                        }
                        break;

                    case "保存图像":
                        ((ContextMenuStrip)sender).Close();
                        SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                        saveFileDialog1.Filter = "bmp files (*.bmp)|*.bmp|All files (*.*)|*.*";
                        saveFileDialog1.FilterIndex = 0;
                        saveFileDialog1.ShowDialog();
                        if (this.drawObject.BackImage != null && this.drawObject.BackImage.Image.IsInitialized())
                            this.drawObject.BackImage.Image.WriteImage("bmp", 0, saveFileDialog1.FileName);
                        else
                            new UserMessageForm().ShowDialog("图像内容为空");
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

                    case "设置光标参数":
                        Point point = this.hWindowControl1.PointToScreen(new Point((int)this._curCol, (int)this._curRow));
                        new ScaleParamForm(point).Show();
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

        #endregion



        private void titleLabel_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, WM_SYSCOMMAND, SC_MOVE + HTCAPTION, 0);
        }

        private void titleLabel_MouseEnter(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Hand;
            this.titleLabel.BackColor = System.Drawing.Color.Orange;
        }

        private void titleLabel_MouseLeave(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Default;
            this.titleLabel.BackColor = System.Drawing.Color.LightGray;
        }

        private void 传感器comboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            try
            {
                this.cts?.Cancel(); // 他改变时一定要去悼  
                this._viewConfigParam.CamName = this.传感器comboBox.SelectedItem?.ToString();
                this.acqSource = AcqSourceManage.Instance.GetCamAcqSource(this._viewConfigParam.CamName);
                this._sensor = AcqSourceManage.Instance.GetAcqSource(this._viewConfigParam.CamName)?.Sensor;
            }
            catch (Exception ex)
            {
                new Common.UserMessageForm(ex.ToString()).ShowDialog();
            }
        }

        private void 标定工具栏toolStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            try
            {
                if (this.传感器comboBox.SelectedItem == null)
                {
                    new UserMessageForm().ShowDialog("窗口上未指定相机名!!!");
                    return;
                }
                CameraParam NowCaliPara = AcqSourceManage.Instance.GetAcqSource(this.传感器comboBox.SelectedItem.ToString())?.Sensor?.CameraParam;
                CameraParam MapTargetNowCaliPara = null;
                if (NowCaliPara == null) return;
                ////////////////////////////////////////////////////////
                switch (e.ClickedItem.Name)
                {
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



    }
}

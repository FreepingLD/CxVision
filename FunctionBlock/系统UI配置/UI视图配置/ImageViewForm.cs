
using Common;
using FunctionBlock;
using HalconDotNet;
using MotionControlCard;
using Sensor;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
        private AcqSource _camAcqSource;
        private ISensor _Sensor;
        private ViewConfigParam _viewConfigParam;
        private bool IsLoad = false; // 窗体加载后，设置为true
        private MetrolegyParamForm metrolegyParamForm;
        private IFunction _currFunction;
        private ImageDataClass CurrentImageData;
        private Dictionary<string, object> listData = new Dictionary<string, object>();
        private string viewName = "";
        private object lockSyn = new object();


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
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
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
                ViewConfigParamManager.Instance.ViewParamList.Remove(this._viewConfigParam); // 关闭窗体时要删除相应的对象
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
            DialogResult dialogResult = MessageBox.Show("确定关闭窗体吗？", "关闭窗体", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
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
        private void 实时采集checkBox_CheckedChanged(object sender, EventArgs e)
        {
            if (!IsSelect()) return;
            /////////////////////////////////////////
            switch (this.实时采集checkBox.CheckState)
            {
                case CheckState.Checked:
                    this.实时采集checkBox.BackColor = Color.Red;
                    cts2 = new CancellationTokenSource();
                    Dictionary<enDataItem, object> data;
                    this._camAcqSource = AcqSourceManage.Instance.GetAcqSource(this._viewConfigParam.CamName);
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
                            if (this._camAcqSource == null) return;
                            data = this._camAcqSource?.AcqPointData();
                            switch (this._camAcqSource?.Sensor.ConfigParam.SensorType)
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
                                        double[] axisPose = this._camAcqSource.GetAxisPosition();
                                        this.drawObject.BackImage = (ImageDataClass)data[enDataItem.Image];
                                        this.drawObject.BackImage.Grab_X = axisPose[0];
                                        this.drawObject.BackImage.Grab_Y = axisPose[1];
                                        this.drawObject.BackImage.Grab_Z = axisPose[2];
                                        this.drawObject.BackImage.Grab_Theta = axisPose[3];
                                        this.drawObject.AttachPropertyData.Clear();
                                        this.drawObject.AttachPropertyData.Add((this.GenCrossLine(this.drawObject.BackImage.Image)));
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
        #endregion

        #region 视图交互
        private void hWindowControl1_MouseMove(object sender, GrayValueInfoEventArgs e)
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
            this.行坐标Label.Text = e.Row.ToString();
            this.列坐标Label.Text = e.Col.ToString();
        }
        public void DisplayExcuteResult(object sender, ExcuteCompletedEventArgs e)  // 这里一定要加一个键值对变量来收集，以免出现重合 ImageAcqCompleteEventArgs e
        {
            try
            {
                if (!IsSelect()) return; // 如果不是当前选择的，则返回
                if (e.DataContent == null) return;// 在图形窗口只显示世界坐标元素，像素元素在图像窗口显示
                /////////////////////////////////////////////
                lock (this.lockSyn)
                {
                    switch (e.DataContent.GetType().Name) //这里只接受XLD轮廓或3D对象轮廓
                    {
                        case "ImageDataClass":
                            this.listData.Clear();
                            this.drawObject.ClearViewObject();
                            this.drawObject.BackImage = (ImageDataClass)e.DataContent;
                            this.CurrentImageData = this.drawObject.BackImage;
                            break;
                        case nameof(RegionDataClass):
                            if (this.listData.ContainsKey(e.ItemName))
                                this.listData[e.ItemName] = ((RegionDataClass)e.DataContent)?.Region;
                            else
                                this.listData.Add(e.ItemName, ((RegionDataClass)e.DataContent)?.Region);
                            //this.drawObject.AddViewObject(new ViewData(((RegionDataClass)e.DataContent)?.Region, "red"));
                            break;
                        case nameof(XldDataClass):
                            if (this.listData.ContainsKey(e.ItemName))
                                this.listData[e.ItemName] = ((XldDataClass)e.DataContent).HXldCont;//.HXldCont;
                            else
                                this.listData.Add(e.ItemName, ((XldDataClass)e.DataContent).HXldCont); //.HXldCont
                            //this.drawObject.AddViewObject(new ViewData(((XldDataClass)e.DataContent)?.HXldCont, "red"));
                            break;
                        case "HXLDCont":
                            if (this.listData.ContainsKey(e.ItemName))
                                this.listData[e.ItemName] = (HXLDCont)e.DataContent;
                            else
                                this.listData.Add(e.ItemName, (HXLDCont)e.DataContent);
                            this.drawObject.AddViewObject(new ViewData(((HXLDCont)e.DataContent), "green"));
                            break;
                        case "userWcsCircle":
                            this.RemoveItem(e.ItemName);
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
                        case "userWcsCircleSector":
                            this.RemoveItem(e.ItemName);
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
                        case "userWcsEllipse":
                            this.RemoveItem(e.ItemName);
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
                        case "userWcsEllipseSector":
                            this.RemoveItem(e.ItemName);
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
                                    //this.drawObject.AddViewObject(new ViewData(wcsEllipseSector.EdgesPoint_xyz[i].GetPixPoint(), "green"));
                                }
                            }
                            break;
                        case "userWcsLine":
                            this.RemoveItem(e.ItemName);
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

                        case "userWcsPoint":
                            this.RemoveItem(e.ItemName);
                            userWcsPoint wcsPoint = ((userWcsPoint)e.DataContent);
                            if (this.listData.ContainsKey(e.ItemName))
                                this.listData[e.ItemName] = wcsPoint.GetPixPoint();
                            else
                                this.listData.Add(e.ItemName, wcsPoint.GetPixPoint());
                            //this.drawObject.AddViewObject(new ViewData(wcsPoint.GetPixPoint().GetXLD(), "green"));
                            break;
                        case "userWcsVector":
                            this.RemoveItem(e.ItemName);
                            userWcsVector wcsVector  = ((userWcsVector)e.DataContent);
                            userWcsPoint wcsPoint1 = new userWcsPoint(wcsVector.X, wcsVector.Y, wcsVector.Z, wcsVector.CamParams);
                            if (this.listData.ContainsKey(e.ItemName))
                                this.listData[e.ItemName] = wcsPoint1.GetPixPoint();
                            else
                                this.listData.Add(e.ItemName, wcsPoint1.GetPixPoint());
                            //this.drawObject.AddViewObject(new ViewData(wcsPoint.GetPixPoint().GetXLD(), "green"));
                            break;
                        case "userWcsRectangle1":
                            this.RemoveItem(e.ItemName);
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
                        case "userWcsRectangle2":
                            this.RemoveItem(e.ItemName);
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
                        case "userWcsCoordSystem":
                            if (this.listData.ContainsKey(e.ItemName))
                                this.listData[e.ItemName] = e.DataContent;
                            else
                                this.listData.Add(e.ItemName, e.DataContent);

                            //this.drawObject.AddViewObject(new ViewData(e.DataContent, "green"));
                            break;
                        case "userOkNgText":
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
                        case "userTextLable":
                            if (this.listData.ContainsKey(e.ItemName))
                                this.listData[e.ItemName] = e.DataContent;
                            else
                                this.listData.Add(e.ItemName, e.DataContent);
                            //this.drawObject.AddViewObject(new ViewData(e.DataContent, "green"));
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
                        foreach (KeyValuePair<string, object> item in this.listData)
                        {
                            this.drawObject.AttachPropertyData.Add(item.Value);
                        }
                        this.drawObject.DetachDrawingObjectFromWindow();
                    }
                //});
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
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
            try
            {
                switch (e.Node.Tag.GetType().Name)
                {
                    case "CircleMeasure":
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
                    case "CircleSectorMeasure":
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
                    case "EllipseMeasure":
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
                    case "EllipseSectorMeasure":
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
                    case "LineMeasure":
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
                    case "PointMeasure":
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
                    case "Rectangle2Measure":
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
                    case "WidthMeasure":
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
                    case "CrossPointMeasure":
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
                MessageBox.Show(ex.ToString());
            }
        }

        #endregion

        #region 右键菜单项
        private void addContextMenu(HWindowControl hWindowControl)
        {
            ContextMenuStrip ContextMenuStrip1 = new ContextMenuStrip();
            ContextMenuStrip1.Name = hWindowControl.Name;
            // 添加右键菜单 
            ToolStripItem[] items = new ToolStripMenuItem[]
            {
                new ToolStripMenuItem("执行"),
                new ToolStripMenuItem("设置抓边参数"),
                new ToolStripMenuItem("边缘点"),
                new ToolStripMenuItem("------------"),
                new ToolStripMenuItem("自适应窗口"),
                new ToolStripMenuItem("清除窗口"),
                new ToolStripMenuItem("保存图像"),
            };
            if (this._viewConfigParam.IsShowCross)
                items[2].Text = "隐藏边缘点";
            else
                items[2].Text = "显示边缘点";
            ContextMenuStrip1.Items.AddRange(items); // 在滑赋值前不能调用 
            ContextMenuStrip1.ItemClicked += new ToolStripItemClickedEventHandler(hWindowControlContextMenuStrip_ItemClicked);
            hWindowControl.ContextMenuStrip = ContextMenuStrip1;
        }
        private void hWindowControlContextMenuStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            string name = e.ClickedItem.Text;
            try
            {
                ((ContextMenuStrip)sender).Close();
                switch (name)
                {
                    case "执行":
                        switch (this._currFunction.GetType().Name)
                        {
                            case "CircleMeasure":
                                this._currFunction?.Execute(this.drawObject.GetPixCircleParam());
                                //this.drawObject.DetachDrawingObjectFromWindow();
                                break;
                            case "CircleSectorMeasure":
                                this._currFunction?.Execute(this.drawObject.GetPixCircleSectorParam());
                                break;
                            case "EllipseMeasure":
                                this._currFunction?.Execute(this.drawObject.GetPixEllipseParam());
                                break;
                            case "EllipseSectorMeasure":
                                this._currFunction?.Execute(this.drawObject.GetPixEllipseSectorParam());
                                break;
                            case "LineMeasure":
                                this._currFunction?.Execute(this.drawObject.GetPixLineParam());
                                //this.drawObject.DetachDrawingObjectFromWindow();
                                break;
                            case "PointMeasure":
                                this._currFunction?.Execute(this.drawObject.GetPixLineParam());
                                break;
                            case "Rectangle2Measure":
                                this._currFunction?.Execute(this.drawObject.GetPixRectangle2Param());
                                break;
                            case "WidthMeasure":
                                this._currFunction?.Execute(this.drawObject.GetPixRectangle2Param());
                                break;
                            case "CrossPointMeasure":
                                this._currFunction?.Execute(this.drawObject.GetPixLineParam());
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
                    case "保存图像":
                        ((ContextMenuStrip)sender).Close();
                        SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                        saveFileDialog1.Filter = "bmp files (*.bmp)|*.bmp|All files (*.*)|*.*";
                        saveFileDialog1.FilterIndex = 0;
                        saveFileDialog1.ShowDialog();
                        if (this.drawObject.BackImage != null && this.drawObject.BackImage.Image.IsInitialized())
                            this.drawObject.BackImage.Image.WriteImage("bmp", 0, saveFileDialog1.FileName);
                        else
                            MessageBox.Show("图像内容为空");
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
                this._viewConfigParam.CamName = this.传感器comboBox.SelectedItem?.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }


    }
}


using Common;
using FunctionBlock;
using HalconDotNet;
using MotionControlCard;
using Sensor;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using View;

namespace FunctionBlock
{
    public partial class FlawDetecteViewForm : Form
    {
        private ConcurrentQueue<ImageDataClass> imageList = new ConcurrentQueue<ImageDataClass>();
        private Dictionary<string, FlawData> DicFlaw = new Dictionary<string, FlawData>();
        private VisualizeView drawObject;
        private ViewConfigParam _viewConfigParam;
        private bool IsLoad = false; // 窗体加载后，设置为true
        private AcqSource acqSource;
        private HImage sourceImage;
        private System.Windows.Forms.Button[] StationBtns;
        private int currentImageIndex = 1;


        public FlawDetecteViewForm(ViewConfigParam viewConfigParam)
        {
            InitializeComponent();
            /////////////
            this._viewConfigParam = viewConfigParam;
            this.drawObject = new userDrawPointROI(this.hWindowControl1, false);
            this.drawObject.HMouseDoubleClick += new HMouseEventHandler(this.hWindowControl1_DoubleClick);
            BaseFunction.ExcuteCompleted += new ExcuteCompletedEventHandler(DisplayExcuteResult);
            this.drawObject.GrayValueInfo += new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
            //if (AcqSourceManage.Instance.GetAcqSource(this._viewConfigParam.CamName) != null && AcqSourceManage.Instance.GetAcqSource(this._viewConfigParam.CamName).Sensor != null)
            //    AcqSourceManage.Instance.GetAcqSource(this._viewConfigParam.CamName).Sensor.ImageAcqComplete += new ImageAcqCompleteEventHandler(ImageAcqComplete_Event);
            this._viewConfigParam = viewConfigParam;
            if (!HWindowManage.HWindowList.ContainsKey(viewConfigParam.ViewName))
            {
                HWindowManage.HWindowList.Add(viewConfigParam.ViewName, this.hWindowControl1.HalconWindow);
            }
            this.titleLabel.Text = viewConfigParam.ViewName;
            this.titleLabel.TextAlign = ContentAlignment.MiddleLeft;
            this.ContextMenu = new ContextMenu();
            UserLoginParamManager.Instance.UserChange += new EventHandler(this.UserChange_Event);
        }
        private void FlawDetecteViewForm_Load(object sender, EventArgs e)
        {
            //////////////
            BindProperty();
            //////////////////////////////////////
            this.Location = this._viewConfigParam.Location;
            this.Size = this._viewConfigParam.FormSize;
            this.SignLabel.Text = this._viewConfigParam.Tag;
            this.IsLoad = true;
            this.addContextMenu();
            this.addContextMenu(this.hWindowControl1);
            this.InitButn();
            ///////////////////////////

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
                ///////////////////////////////////////////////////////////////
                this.按钮宽度textBox.DataBindings.Add(nameof(this.按钮宽度textBox.Text), this._viewConfigParam.PageParam, nameof(this._viewConfigParam.PageParam.BtnWid), true, DataSourceUpdateMode.OnPropertyChanged);
                this.按钮高度textBox.DataBindings.Add(nameof(this.按钮高度textBox.Text), this._viewConfigParam.PageParam, nameof(this._viewConfigParam.PageParam.BtnHei), true, DataSourceUpdateMode.OnPropertyChanged);
                //this.边缘1图像数量textBox.DataBindings.Add(nameof(this.边缘1图像数量textBox.Text), this._viewConfigParam.PageParam, nameof(this._viewConfigParam.PageParam.ImageNum), true, DataSourceUpdateMode.OnPropertyChanged);
                this.边缘1图像数量textBox.DataBindings.Add(nameof(this.边缘1图像数量textBox.Text), this._viewConfigParam.PageParam, nameof(this._viewConfigParam.PageParam.ImageNumEdge1), true, DataSourceUpdateMode.OnPropertyChanged);
                this.边缘2图像数量textBox.DataBindings.Add(nameof(this.边缘1图像数量textBox.Text), this._viewConfigParam.PageParam, nameof(this._viewConfigParam.PageParam.ImageNumEdge2), true, DataSourceUpdateMode.OnPropertyChanged);
                this.边缘3图像数量textBox.DataBindings.Add(nameof(this.边缘1图像数量textBox.Text), this._viewConfigParam.PageParam, nameof(this._viewConfigParam.PageParam.ImageNumEdge3), true, DataSourceUpdateMode.OnPropertyChanged);
                this.边缘数量textBox.DataBindings.Add(nameof(this.边缘数量textBox.Text), this._viewConfigParam.PageParam, nameof(this._viewConfigParam.PageParam.DetectEdgeNum), true, DataSourceUpdateMode.OnPropertyChanged);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        public void InitButn()
        {
            try
            {
                //int i = 0;
                switch (this._viewConfigParam.PageParam.DetectEdgeNum)
                {
                    default:
                    case 1:
                        this.edge2Panel.Hide();
                        this.edge3Panel.Hide();
                        this.StationBtns = new Button[this._viewConfigParam.PageParam.ImageNumEdge1];
                        for (int j = 0; j < this.StationBtns.Length; j++)
                        {
                            this.StationBtns[j] = new Button();
                        }
                        for (int i = 0; i < this.StationBtns.Length; i++)
                        {
                            this.StationBtns[i].Tag = i + 1;
                            this.StationBtns[i].Name = "btn" + i.ToString();
                            this.SetBtnPos(this.StationBtns[i], this._viewConfigParam.PageParam.BtnWid, this._viewConfigParam.PageParam.BtnHei, 10, (this._viewConfigParam.PageParam.BtnWid + (int)(this._viewConfigParam.PageParam.BtnWid * 0.1)) * i);
                            this.StationBtns[i].TabIndex = i;
                            this.StationBtns[i].Text = "图像" + (i + 1).ToString();
                            //i++;
                            this.StationBtns[i].Click += new EventHandler(BtnClick);
                            this.edge1Panel.Controls.Add(this.StationBtns[i]);
                        }
                        foreach (var item in this.StationBtns)
                        {

                        }
                        break;
                    case 2:
                        this.edge3Panel.Hide();
                        this.StationBtns = new Button[this._viewConfigParam.PageParam.ImageNumEdge1 + this._viewConfigParam.PageParam.ImageNumEdge2];
                        for (int j = 0; j < this.StationBtns.Length; j++)
                        {
                            this.StationBtns[j] = new Button();
                        }
                        for (int i = 0; i < this._viewConfigParam.PageParam.ImageNumEdge1; i++)
                        {
                            this.StationBtns[i].Tag = i + 1;
                            this.StationBtns[i].Name = "btn" + i.ToString();
                            this.SetBtnPos(this.StationBtns[i], this._viewConfigParam.PageParam.BtnWid, this._viewConfigParam.PageParam.BtnHei, 10, (this._viewConfigParam.PageParam.BtnWid + (int)(this._viewConfigParam.PageParam.BtnWid * 0.1)) * i);
                            this.StationBtns[i].TabIndex = i;
                            this.StationBtns[i].Text = "图像" + (i + 1).ToString();
                            //i++;
                            this.StationBtns[i].Click += new EventHandler(BtnClick);
                            this.edge1Panel.Controls.Add(this.StationBtns[i]);
                        }
                        for (int i = this._viewConfigParam.PageParam.ImageNumEdge1; i < this.StationBtns.Length; i++)
                        {
                            this.StationBtns[i].Tag = i + 1;
                            this.StationBtns[i].Name = "btn" + i.ToString();
                            this.SetBtnPos(this.StationBtns[i], this._viewConfigParam.PageParam.BtnWid, this._viewConfigParam.PageParam.BtnHei, 10, (this._viewConfigParam.PageParam.BtnWid + (int)(this._viewConfigParam.PageParam.BtnWid * 0.1)) * (i - this._viewConfigParam.PageParam.ImageNumEdge1));
                            this.StationBtns[i].TabIndex = i;
                            this.StationBtns[i].Text = "图像" + (i + 1 - this._viewConfigParam.PageParam.ImageNumEdge1).ToString();
                            //i++;
                            this.StationBtns[i].Click += new EventHandler(BtnClick);
                            this.edge2Panel.Controls.Add(this.StationBtns[i]);
                        }
                        break;
                    case 3:
                        this.StationBtns = new Button[this._viewConfigParam.PageParam.ImageNumEdge1 + this._viewConfigParam.PageParam.ImageNumEdge2 + this._viewConfigParam.PageParam.ImageNumEdge3];
                        for (int j = 0; j < this.StationBtns.Length; j++)
                        {
                            this.StationBtns[j] = new Button();
                        }
                        for (int i = 0; i < this._viewConfigParam.PageParam.ImageNumEdge1; i++)
                        {
                            this.StationBtns[i].Tag = i + 1;
                            this.StationBtns[i].Name = "btn" + i.ToString();
                            this.SetBtnPos(this.StationBtns[i], this._viewConfigParam.PageParam.BtnWid, this._viewConfigParam.PageParam.BtnHei, 10, (this._viewConfigParam.PageParam.BtnWid + (int)(this._viewConfigParam.PageParam.BtnWid * 0.1)) * i);
                            this.StationBtns[i].TabIndex = i;
                            this.StationBtns[i].Text = "图像" + (i + 1).ToString();
                            //i++;
                            this.StationBtns[i].Click += new EventHandler(BtnClick);
                            this.edge1Panel.Controls.Add(this.StationBtns[i]);
                        }
                        for (int i = this._viewConfigParam.PageParam.ImageNumEdge1; i < this.StationBtns.Length - this._viewConfigParam.PageParam.ImageNumEdge3; i++)
                        {
                            this.StationBtns[i].Tag = i + 1;
                            this.StationBtns[i].Name = "btn" + i.ToString();
                            this.SetBtnPos(this.StationBtns[i], this._viewConfigParam.PageParam.BtnWid, this._viewConfigParam.PageParam.BtnHei, 10, (this._viewConfigParam.PageParam.BtnWid + (int)(this._viewConfigParam.PageParam.BtnWid * 0.1)) * (i - this._viewConfigParam.PageParam.ImageNumEdge1));
                            this.StationBtns[i].TabIndex = i;
                            this.StationBtns[i].Text = "图像" + (i + 1 - this._viewConfigParam.PageParam.ImageNumEdge1).ToString();
                            //i++;
                            this.StationBtns[i].Click += new EventHandler(BtnClick);
                            this.edge2Panel.Controls.Add(this.StationBtns[i]);
                        }
                        for (int i = this._viewConfigParam.PageParam.ImageNumEdge1 + this._viewConfigParam.PageParam.ImageNumEdge2; i < this.StationBtns.Length; i++)
                        {
                            this.StationBtns[i].Tag = i + 1;
                            this.StationBtns[i].Name = "btn" + i.ToString();
                            this.SetBtnPos(this.StationBtns[i], this._viewConfigParam.PageParam.BtnWid, this._viewConfigParam.PageParam.BtnHei, 10, (this._viewConfigParam.PageParam.BtnWid + (int)(this._viewConfigParam.PageParam.BtnWid * 0.1)) * (i - this._viewConfigParam.PageParam.ImageNumEdge1 - this._viewConfigParam.PageParam.ImageNumEdge2));
                            this.StationBtns[i].TabIndex = i;
                            this.StationBtns[i].Text = "图像" + (i + 1 - this._viewConfigParam.PageParam.ImageNumEdge1 - this._viewConfigParam.PageParam.ImageNumEdge2).ToString();
                            //i++;
                            this.StationBtns[i].Click += new EventHandler(BtnClick);
                            this.edge3Panel.Controls.Add(this.StationBtns[i]);
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
            }
        }
        private void SetBtnPos(Button buttonIn, int BtnWid, int BtnHei, int LeftRow, int LeftCol)
        {
            buttonIn.Location = new System.Drawing.Point(LeftCol, LeftRow);
            buttonIn.Size = new System.Drawing.Size(BtnWid, BtnHei);
            buttonIn.UseVisualStyleBackColor = true;

        }

        /// <summary>
        /// 登录用户改变
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                        break;
                    case enUserName.工程师:
                        this.传感器comboBox1.Enabled = true;
                        this.程序节点comboBox.Enabled = true;
                        this.buttonClose.Enabled = false;
                        break;
                    case enUserName.开发人员:
                        this.传感器comboBox1.Enabled = true;
                        this.程序节点comboBox.Enabled = true;
                        this.buttonClose.Enabled = true;
                        break;
                }
            }
            catch(Exception ex)
            {
                LoggerHelper.Error(ex.ToString());
            }
        }

        /// <summary>
        /// 当前选项卡被选择的
        /// </summary>
        /// <returns></returns>
        //private bool IsSelect()
        //{
        //    bool result = false;
        //    Control container = this.Parent;
        //    if (container == null) return result;
        //    switch (container.GetType().Name)
        //    {
        //        case nameof(TabPage):
        //            this.Invoke(new Action(() =>
        //            {
        //                TabControl tabControl = ((TabPage)container).Parent as TabControl;
        //                if (tabControl.SelectedTab != ((TabPage)container))
        //                    result = false;
        //                else
        //                    result = true;
        //            }));
        //            break;
        //    }
        //    return result;
        //}
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
                        if (SystemParamManager.Instance.SysConfigParam.IsAutoRun) // 自动运行状态下才自动切换
                            tabControl.SelectedTab = ((TabPage)container);
                        result = true;
                    }));
                    break;
            }
            return result;
        }

        private void DisplayExcuteResult(object sender, ExcuteCompletedEventArgs e)
        {
            if (e.SensorName == null) return;
            if (!this._viewConfigParam.CamName.Contains(e.SensorName)) return;
            if (e.ViewWindow == null || e.ViewWindow != this._viewConfigParam.ViewName) return;
            if (!IsSelect()) return;  // 只有在当前选项页时才显示
            if (e.DataContent != null)
            {
                switch (e.DataContent.GetType().Name)
                {
                    case nameof(ImageDataClass):
                        this.drawObject.ClearViewObject(); // 更新图像时清空
                        this.drawObject.BackImage = (ImageDataClass)e.DataContent;
                        if (this._viewConfigParam.IsShowCross)
                            this.drawObject.AddViewObject(new ViewData(this.GenCrossLine(this.drawObject.BackImage.Image), "green"));
                        ///////////////////////
                        int.TryParse(this.drawObject.BackImage.Tag.ToString(), out this.currentImageIndex);
                        if (this.currentImageIndex == 1)
                        {
                            this.InitStation();
                            this.Invoke(new Action(() => this.产品编号textBox.Text = CommunicationConfigParamManger.Instance.ReadValue(enCoordSysName.CoordSys_0, enCommunicationCommand.ProductID).ToString()));
                            //this.产品编号textBox.Text = CommunicationConfigParamManger.Instance.ReadValue(enCoordSysName.CoordSys_0, enCommunicationCommand.ProductID).ToString();
                        }
                        //////////////////////////////////
                        if (this.DicFlaw.ContainsKey(this.drawObject.BackImage.Tag.ToString()))
                        {
                            this.DicFlaw[this.drawObject.BackImage.Tag.ToString()].ImageData = this.drawObject.BackImage.Clone();
                            this.Invoke(new Action(() =>
                            {
                                this.DicFlaw[this.drawObject.BackImage.Tag.ToString()].FlawMessage?.Clear(); // 有图像更新时，清空缺陷信息
                                this.DicFlaw[this.drawObject.BackImage.Tag.ToString()].DetectXld?.Clear(); // 有图像更新时，清空缺陷信息
                            }));
                            this.DicFlaw[this.drawObject.BackImage.Tag.ToString()].Result = "OK"; // 初始值赋 OK 
                        }
                        else
                            this.DicFlaw.Add(this.drawObject.BackImage.Tag.ToString(), new FlawData(this.drawObject.BackImage, new XldDataClass(), new BindingList<FlawMsg>()));
                        /////////////////////////////////////////////////////////////////////////////////////////////
                        break;

                    case nameof(RegionDataClass):
                        RegionDataClass regionData = (RegionDataClass)e.DataContent;
                        int count = regionData.Region.CountObj();
                        this.drawObject.AddViewObject(new ViewData(regionData.Region, regionData.Color.ToString()));
                        if (this.DicFlaw.ContainsKey(regionData.Tag.ToString())) // 在这里 Tag对应的即为图像索引
                        {
                            List<FlawMsg> list = this.GetFlawMsg(this.DicFlaw[regionData.Tag.ToString()].ImageData.Image, regionData.Region);
                            foreach (var item in list)
                            {
                                this.Invoke(new Action(() => this.DicFlaw[regionData.Tag.ToString()].FlawMessage.Add(item)));
                                //this.DicFlaw[regionData.Tag.ToString()].FlawMessage.Add(item);
                            }
                        }
                        break;

                    case nameof(XldDataClass):
                        XldDataClass xldData = (XldDataClass)e.DataContent;
                        count = xldData.HXldCont.CountObj();
                        this.drawObject.AddViewObject(new ViewData(xldData.HXldCont, xldData.Color.ToString()));
                        if (this.DicFlaw.ContainsKey(xldData.Tag.ToString()))  // 在这里 Tag对应的即为图像索引
                        {
                            if (this.DicFlaw[xldData.Tag.ToString()].DetectXld == null)
                                this.DicFlaw[xldData.Tag.ToString()].DetectXld = new XldDataClass();
                            this.Invoke(new Action(() => this.DicFlaw[xldData.Tag.ToString()].DetectXld.AddXLDCont(new HXLDCont(xldData?.HXldCont?.Clone()))));
                            //this.DicFlaw[xldData.Tag.ToString()].DetectXld.AddXLDCont(xldData?.HXldCont?.Clone() as HXLDCont);
                        }
                        break;

                    case nameof(userOkNgText):
                        this.drawObject.AddViewObject(new ViewData(e.DataContent, "red"));
                        break;

                    case nameof(userTextLable):
                        userTextLable textLable = e.DataContent as userTextLable;
                        this.drawObject.AddViewObject(new ViewData(textLable, textLable.Color));
                        if (textLable.Text.Contains("NG") || textLable.Text.Contains("ng"))
                        {
                            if (this.DicFlaw.ContainsKey(this.currentImageIndex.ToString()))
                                this.DicFlaw[this.currentImageIndex.ToString()].Result = "NG";
                            this.StationShow(false, currentImageIndex - 1);
                        }
                        else
                        {
                            this.StationShow(true, currentImageIndex - 1);
                        }

                        break;

                    default:
                        //this.drawObject.AddViewObject(new ViewData(e.DataContent, "red"));
                        break;
                }
            }
        }

        /// <summary>
        /// 窗体移动事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FlawDetecteViewForm_Move(object sender, EventArgs e)
        {
            if (this._viewConfigParam != null)
                this._viewConfigParam.Location = this.Location;
        }

        /// <summary>
        /// 窗体尺寸改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FlawDetecteViewForm_Resize(object sender, EventArgs e)
        {
            if (this._viewConfigParam != null && this.IsLoad) // 在窗体加载后这个事件才起作用
                this._viewConfigParam.FormSize = this.Size;
        }

        private void FlawDetecteViewForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                this.cts?.Cancel();
                this.cts2?.Cancel();
                this.IsLoad = false;
                ViewConfigParamManager.Instance.ViewParamList.Remove(this._viewConfigParam); // 关闭窗体时要删除相应的对象
                UserLoginParamManager.Instance.UserChange -= new EventHandler(this.UserChange_Event);
                BaseFunction.ExcuteCompleted -= new ExcuteCompletedEventHandler(DisplayExcuteResult);
                this.drawObject.GrayValueInfo -= new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                this.drawObject.HMouseDoubleClick -= new HMouseEventHandler(this.hWindowControl1_DoubleClick);
                //if (AcqSourceManage.Instance.GetAcqSource(this._viewConfigParam.CamName) != null && AcqSourceManage.Instance.GetAcqSource(this._viewConfigParam.CamName).Sensor != null)
                //    AcqSourceManage.Instance.GetAcqSource(this._viewConfigParam.CamName).Sensor.ImageAcqComplete -= new ImageAcqCompleteEventHandler(ImageAcqComplete_Event);
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

        #region  数据实时采集
        private CancellationTokenSource cts;
        private void 实时采集checkBox_CheckedChanged(object sender, EventArgs e)
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
                        while (!cts.IsCancellationRequested)
                        {
                            data = this.acqSource.AcqImageData(null);
                            switch (this.acqSource.Sensor?.ConfigParam.SensorType)
                            {
                                case enUserSensorType.线阵相机:
                                case enUserSensorType.面阵相机:
                                    if (data?.Count > 0)
                                    {
                                        this.drawObject.BackImage = (ImageDataClass)data[enDataItem.Image];
                                        this.drawObject.ClearViewObject();
                                        if (this._viewConfigParam.IsShowCross)
                                            this.drawObject.AddViewObject(new ViewData(this.GenCrossLine(this.drawObject.BackImage.Image), "green"));
                                    }
                                    break;
                            }
                            Thread.Sleep(100);
                        }
                    });
                    break;
                default:
                    cts?.Cancel();
                    this.实时采集checkBox.BackColor = Color.Lime;
                    break;
            }
        }
        #endregion

        #region 标签右键菜单项
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
            this.SignLabel.ContextMenuStrip = ContextMenuStrip1;
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
                        RenameForm renameForm = new RenameForm(this.SignLabel.Text);
                        renameForm.ShowDialog();
                        this._viewConfigParam.Tag = renameForm.Name;
                        renameForm.Dispose();
                        this.SignLabel.Text = this._viewConfigParam.Tag;
                        break;
                        ///////////////////////////////////////////////                 
                }
            }
            catch
            {
            }
        }
        #endregion

        #region 窗口右键菜单项

        private void addContextMenu(HWindowControl hWindowControl)
        {
            ContextMenuStrip ContextMenuStrip1 = new ContextMenuStrip();
            ContextMenuStrip1.Name = hWindowControl.Name;
            // 添加右键菜单 
            ToolStripItem[] items = new ToolStripMenuItem[]
            {
             new ToolStripMenuItem("自适应图像(Auto)"),
             new ToolStripMenuItem("编辑程序"),
             new ToolStripMenuItem("设置曝光"),
             new ToolStripMenuItem("设置增益"),
             new ToolStripMenuItem("设置光源亮度"),
             new ToolStripMenuItem("十字线"),
             new ToolStripMenuItem("3D(View)"),
             new ToolStripMenuItem("保存图像") ,
             new ToolStripMenuItem("加载图像") ,
             new ToolStripMenuItem("保存点云"),
             new ToolStripMenuItem("清除窗口(Clear)"),
            };
            if (this._viewConfigParam.IsShowCross)
                items[5].Text = "隐藏十字线";
            else
                items[5].Text = "显示十字线";
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
                    case "自适应图像(Auto)":
                        this.drawObject.AutoImage();
                        break;
                    case "编辑程序":
                        foreach (KeyValuePair<string, TreeViewWrapClass> item in ProgramForm.Instance.ProgramDic)
                        {
                            foreach (TreeNode item2 in item.Value.TreeView.Nodes)
                            {
                                TreeNode node;
                                GetEditeNode(item2, out node);
                                if (node != null)
                                {
                                    item.Value.treeView1_Edite(this, node);
                                    break;
                                }
                            }
                        }
                        break;
                    case "设置曝光":
                        if (AcqSourceManage.Instance.GetAcqSource(this._viewConfigParam.CamName) == null || AcqSourceManage.Instance.GetAcqSource(this._viewConfigParam.CamName).Sensor == null) return;
                        string value = AcqSourceManage.Instance.GetAcqSource(this._viewConfigParam.CamName).Sensor.GetParam("曝光").ToString();
                        RenameForm renameForm = new RenameForm(value);
                        renameForm.ShowDialog();
                        AcqSourceManage.Instance.GetAcqSource(this._viewConfigParam.CamName).Sensor.SetParam("曝光", renameForm.Name);
                        break;
                    case "设置增益":
                        if (AcqSourceManage.Instance.GetAcqSource(this._viewConfigParam.CamName) == null || AcqSourceManage.Instance.GetAcqSource(this._viewConfigParam.CamName).Sensor == null) return;
                        value = AcqSourceManage.Instance.GetAcqSource(this._viewConfigParam.CamName).Sensor.GetParam("增益").ToString();
                        renameForm = new RenameForm(value);
                        renameForm.ShowDialog();
                        AcqSourceManage.Instance.GetAcqSource(this._viewConfigParam.CamName).Sensor.SetParam("增益", renameForm.Name);
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
                                MessageBox.Show("图像内容为空");
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
                                MessageBox.Show("点云句柄内容为空");
                        }
                        break;

                    case "加载图像":
                        string path = new FileOperate().OpenImage();
                        if (path == null || path.Length == 0) return;
                        this.sourceImage = new HImage(path);
                        if (this.sourceImage != null)
                        {
                            this.drawObject.BackImage = new ImageDataClass(this.sourceImage); //, this._acqSource.Sensor.CameraParam
                            this.drawObject.BackImage.CamName = this._viewConfigParam.CamName;
                            if (this.DicFlaw.ContainsKey(1.ToString()))
                                this.DicFlaw[1.ToString()].ImageData = this.drawObject.BackImage;
                            else
                                this.DicFlaw[1.ToString()] = new FlawData(this.drawObject.BackImage, new BindingList<FlawMsg>());
                            this.DicFlaw[1.ToString()].FlawMessage.Clear();
                            this.瑕疵数据dataGridView.DataSource = this.DicFlaw[1.ToString()].FlawMessage;
                        }
                        else
                            throw new ArgumentException("读取的图像为空");
                        break;
                    case "清除窗口(Clear)":
                        this.drawObject.ClearWindow();
                        this.hWindowControl1.HalconWindow.ClearWindow();
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void hWindowControl1_MouseMove(object sender, GrayValueInfoEventArgs e)
        {
            if (e.GaryValue.Length > 0)
            {
                this.hWindowControl1.HalconWindow.SetTposition((int)(e.Row), (int)e.Col);
                this.hWindowControl1.HalconWindow.SetFont("-Consolas-" + 10 + "- *-0-*-*-1-");
                this.hWindowControl1.HalconWindow.SetColor("red");

                this.SignLabel.Text = string.Join("", "Row:", e.Row, " ", "Col:", e.Col, " ", "Gray:", e.GaryValue[0]);
                //string nn = string.Join("","Row:", e.Row, " ", "Col:", e.Col, " ", "Gray:", e.GaryValue[0]);
                //this.hWindowControl1.HalconWindow.WriteString(string.Join("","Row:", e.Row, " ", "Col:", e.Col, " ", "Gray:",e.GaryValue[0]));
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

        private void FlawDetecteViewForm_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, WM_SYSCOMMAND, SC_MOVE + HTCAPTION, 0);
        }

        private void titleLabel_MouseDown(object sender, MouseEventArgs e)
        {
            FlawDetecteViewForm_MouseDown(null, null);
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

        private void 传感器comboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            try
            {
                if (this.IsLoad)
                {
                    this.cts?.Cancel(); // 他改变时一定要去悼  
                    //if (AcqSourceManage.Instance.GetAcqSource(this._viewConfigParam.CamName) != null && AcqSourceManage.Instance.GetAcqSource(this._viewConfigParam.CamName).Sensor != null)
                    //    AcqSourceManage.Instance.GetAcqSource(this._viewConfigParam.CamName).Sensor.ImageAcqComplete -= new ImageAcqCompleteEventHandler(ImageAcqComplete_Event); // 先取消
                    this._viewConfigParam.CamName = this.传感器comboBox1.SelectedItem.ToString();
                    ViewConfigParamManager.Instance.Save();
                    //if (AcqSourceManage.Instance.GetAcqSource(this._viewConfigParam.CamName) != null && AcqSourceManage.Instance.GetAcqSource(this._viewConfigParam.CamName).Sensor != null)
                    //    AcqSourceManage.Instance.GetAcqSource(this._viewConfigParam.CamName).Sensor.ImageAcqComplete += new ImageAcqCompleteEventHandler(ImageAcqComplete_Event); // 再创建
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("保存视图配置参数出错" + ex.ToString());
            }
        }

        private void hWindowControl1_DoubleClick(Object sender, HalconDotNet.HMouseEventArgs e)
        {
            this.buttonMax_Click(null, null);
        }

        private void 程序节点comboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            try
            {
                if (this.程序节点comboBox.SelectedItem == null)
                    this.程序节点comboBox.SelectedItem = this._viewConfigParam.ProgramNode;
                else
                    this._viewConfigParam.ProgramNode = this.程序节点comboBox.SelectedItem.ToString();
                ViewConfigParamManager.Instance.Save();
            }
            catch (Exception ex)
            {
                MessageBox.Show("保存视图配置参数出错" + ex.ToString());
            }
        }

        private void 程序节点comboBox_DropDown(object sender, EventArgs e)
        {
            this.程序节点comboBox.Items.Clear();
            foreach (KeyValuePair<string, TreeViewWrapClass> item in ProgramForm.Instance.ProgramDic)
            {
                foreach (TreeNode item2 in item.Value.TreeView.Nodes)
                {
                    GetToolNode(item2);
                }
            }
        }

        private void GetToolNode(TreeNode node)
        {
            if (node.Name.Contains("Tool"))
            {
                this.程序节点comboBox.Items.Add(node.Text);
                foreach (TreeNode item in node.Nodes)
                {
                    GetToolNode(item);
                }
            }
        }

        private void GetEditeNode(TreeNode node, out TreeNode selectNode)
        {
            selectNode = null;
            if (node.Name.Contains("Tool"))
            {
                if (node.Text == this._viewConfigParam.ProgramNode)
                {
                    selectNode = node;
                    return;
                }
                else
                {
                    foreach (TreeNode item in node.Nodes)
                    {
                        if (selectNode == null)
                            GetEditeNode(item, out selectNode);
                    }
                }
            }
        }


        public void StationShow(bool IsOk, int Num)
        {
            this.BeginInvoke(new Action(() =>
            {
                if (IsOk)
                {
                    if (Num >= 0 && Num < this.StationBtns.Length)
                        this.StationBtns[Num].BackColor = Color.Green;
                }
                else
                {
                    if (Num >= 0 && Num < this.StationBtns.Length)
                        this.StationBtns[Num].BackColor = Color.Red;
                }
            }));
        }

        public void InitStation()
        {
            this.BeginInvoke(new Action(() =>
            {
                for (int i = 0; i < this.StationBtns.Length; i++)
                {
                    this.StationBtns[i].BackColor = Color.Yellow;
                }
                ///////////////////////
                if (this.DicFlaw != null)
                {
                    foreach (var item in this.DicFlaw)
                    {
                        item.Value?.ImageData?.Dispose();
                    }
                }
            }));
        }

        /// <summary>
        /// 显示对应的图像及瑕疵
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnClick(object sender, EventArgs e)
        {
            try
            {
                Button but = sender as Button;
                if (this.DicFlaw.ContainsKey(but.Tag.ToString()))
                {
                    this.drawObject.BackImage = this.DicFlaw[but.Tag.ToString()].ImageData;
                    this.瑕疵数据dataGridView.DataSource = null;
                    this.瑕疵数据dataGridView.DataSource = this.DicFlaw[but.Tag.ToString()].FlawMessage;
                    this.drawObject.ClearViewObject();
                    this.drawObject.AddViewObject(new ViewData(this.DicFlaw[but.Tag.ToString()].DetectXld?.HXldCont, "green"));


                    //int count = this.DicFlaw[but.Tag.ToString()].DetectXld.HXldCont.CountObj();
                    //double row, col;
                    //string point;
                    //this.DicFlaw[but.Tag.ToString()].DetectXld?.HXldCont.AreaCenterXld(out row,out col,out point);
                    int index = 0;
                    foreach (var item in this.DicFlaw[but.Tag.ToString()].FlawMessage)
                    {
                        index++;
                        if (item.FlawRow != null && item.FlawCol != null && item.FlawCol.Count == item.FlawRow.Count && item.FlawCol.Count > 0)
                            this.drawObject.AddViewObject(new ViewData(new HXLDCont(item.FlawRow.ToArray(), item.FlawCol.ToArray()), "red"));
                    }
                }
                else
                {
                    MessageBox.Show("集合中不包含有该对象!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        /// <summary>
        /// 接收图像生成事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ImageAcqComplete_Event(object sender, ImageAcqCompleteEventArgs e)
        {
            try
            {
                if (this.DicFlaw.ContainsKey(e.ImageIndex.ToString()))
                {
                    this.DicFlaw[e.ImageIndex.ToString()].ImageData = e.ImageData;
                    this.DicFlaw[e.ImageIndex.ToString()].FlawMessage?.Clear(); // 有图像更新时，清空缺陷信息
                }
                else
                    this.DicFlaw.Add(e.ImageIndex.ToString(), new FlawData(e.ImageData, new BindingList<FlawMsg>()));
                ///////////////////////////////////////////
                int.TryParse(e.ImageData.Tag.ToString(), out this.currentImageIndex);
                if (this.currentImageIndex == 1)
                    this.InitStation();
                Run(e.ImageData);
            }
            catch (Exception ex)
            {
                LoggerHelper.Error("采图后执行失败!" + ex.ToString());
            }
        }




        public List<FlawMsg> GetFlawMsg(HImage hImage, HRegion hRegion)
        {
            List<FlawMsg> FlawMsgList = new List<FlawMsg>();
            if (hImage == null)
            {
                throw new ArgumentNullException("hImage");
            }
            if (hRegion == null)
            {
                throw new ArgumentNullException("hRegion");
            }
            HTuple FlawArea = new HTuple(), FlawRow = new HTuple(), FlawCol = new HTuple(), FlawAngle = new HTuple(), FlawLen1 = new HTuple(), FlawLen2 = new HTuple();
            HTuple rectRow = new HTuple(), rectCol = new HTuple();
            HTuple LineFlawGrayMean = new HTuple();        //线瑕疵灰度均值
            HTuple LineFlawGrayDeviation = new HTuple();   //线瑕疵灰度偏差
            HTuple LineFlawGrayDif = new HTuple();         //线瑕疵相对于检测区域的灰度偏差
            HTuple InspRoiMean = new HTuple();             //检测区域的平均灰度
            HTuple LineFlawConvexity = new HTuple();       //线瑕疵凸度
            HTuple LineFlawCircularity = new HTuple();     //瑕疵圆度
            HTuple LineFlawRectangularity = new HTuple();
            HTuple LineFlawRadioLw = new HTuple();         //长宽比
            HTuple LineFlawCompactness = new HTuple();     //瑕疵紧密度
            HTuple Divation = new HTuple();
            /////////////////////////////////////////////
            int count = hRegion.CountObj();
            for (int i = 1; i <= count; i++)
            {
                FlawMsg FlawMsgI;  //= new FlawInfo();
                FlawArea = hRegion.SelectObj(i).AreaCenter(out FlawRow, out FlawCol);
                if (FlawArea.D == 0) continue;
                hRegion.SelectObj(i).GetRegionContour(out FlawRow, out FlawCol);
                hRegion.SelectObj(i).SmallestRectangle2(out rectRow, out rectCol, out FlawAngle, out FlawLen1, out FlawLen2);//最小外接矩形
                LineFlawGrayMean = hRegion.SelectObj(i).Intensity(hImage, out LineFlawGrayDeviation);
                InspRoiMean = hRegion.SelectObj(i).Intensity(hImage, out Divation); //检测区域的平均灰度
                LineFlawGrayDif = LineFlawGrayMean - InspRoiMean; //灰度相对于平均灰度的灰度偏差
                LineFlawConvexity = hRegion.SelectObj(i).Convexity();   //凸度
                LineFlawCircularity = hRegion.SelectObj(i).Circularity(); //圆度 
                LineFlawRectangularity = hRegion.SelectObj(i).Rectangularity(); //矩形度
                LineFlawCompactness = hRegion.Compactness();      //瑕疵紧密度
                /////////////////////////////////////////////
                if (FlawLen2.D == 0)
                    FlawLen2 = 1.0;
                LineFlawRadioLw = FlawLen1 / FlawLen2;
                /////////////////////////////////////////////
                FlawMsgI = new FlawMsg();
                FlawMsgI.FlawDescribe = "Flaw" + i.ToString();
                switch (FlawRow.Type)
                {
                    case HTupleType.DOUBLE:
                        for (int k = 0; k < FlawRow.Length; k++)
                        {
                            FlawMsgI.FlawRow.Add(FlawRow[k]);
                        }
                        break;
                    case HTupleType.LONG:
                        for (int k = 0; k < FlawRow.Length; k++)
                        {
                            FlawMsgI.FlawRow.Add(FlawRow[k]);
                        }
                        break;
                }
                switch (FlawRow.Type)
                {
                    case HTupleType.DOUBLE:
                        for (int k = 0; k < FlawRow.Length; k++)
                        {
                            FlawMsgI.FlawCol.Add(FlawCol[k]);
                        }
                        break;
                    case HTupleType.LONG:
                        for (int k = 0; k < FlawRow.Length; k++)
                        {
                            FlawMsgI.FlawCol.Add(FlawCol[k]);
                        }
                        break;
                }
                FlawMsgI.FlawArea = FlawArea.D;
                FlawMsgI.FlawLen1 = FlawLen1.D;
                FlawMsgI.FlawLen2 = FlawLen2.D;
                FlawMsgI.FlawLw = Math.Round(LineFlawRadioLw.D, 3);
                FlawMsgI.FlawGrayMean = Math.Round(LineFlawGrayMean.D, 3);
                FlawMsgI.FlawGrayDeviation = Math.Round(LineFlawGrayDeviation.D, 3);
                //FlawMsgI.FlawGrayDif = Math.Round(LineFlawGrayDif.D, 3);
                //FlawMsgI.InspRoiGrayMean = Math.Round(InspRoiMean.D, 3);
                FlawMsgI.FlawConvexity = Math.Round(LineFlawConvexity.D, 3);
                FlawMsgI.FlawCircularity = Math.Round(LineFlawCircularity.D, 3);
                FlawMsgI.FlawRectangularity = Math.Round(LineFlawRectangularity.D, 3);
                FlawMsgI.FlawCompactness = Math.Round(LineFlawCompactness.D, 3);
                FlawMsgList.Add(FlawMsgI);
            }
            return FlawMsgList;

        }
        public List<FlawMsg> GetFlawMsg(HImage hImage, HXLDCont xldCont)
        {
            List<FlawMsg> FlawMsgList = new List<FlawMsg>();
            if (hImage == null)
            {
                throw new ArgumentNullException("hImage");
            }
            if (xldCont == null)
            {
                throw new ArgumentNullException("hRegion");
            }
            HTuple FlawArea = new HTuple(), FlawRow = new HTuple(), FlawCol = new HTuple(), FlawAngle = new HTuple(), FlawLen1 = new HTuple(), FlawLen2 = new HTuple();
            HTuple rectRow = new HTuple(), rectCol = new HTuple();
            HTuple LineFlawGrayMean = new HTuple();        //线瑕疵灰度均值
            HTuple LineFlawGrayDeviation = new HTuple();   //线瑕疵灰度偏差
            HTuple LineFlawGrayDif = new HTuple();         //线瑕疵相对于检测区域的灰度偏差
            HTuple InspRoiMean = new HTuple();             //检测区域的平均灰度
            HTuple LineFlawConvexity = new HTuple();       //线瑕疵凸度
            HTuple LineFlawCircularity = new HTuple();     //瑕疵圆度
            HTuple LineFlawRectangularity = new HTuple();
            HTuple LineFlawRadioLw = new HTuple();         //长宽比
            HTuple LineFlawCompactness = new HTuple();     //瑕疵紧密度
            HTuple Divation = new HTuple();
            HRegion hRegion = new HRegion();
            /////////////////////////////////////////////
            int count = xldCont.CountObj();
            FlawMsg FlawMsgI;  //= new FlawInfo();
            for (int i = 1; i <= count; i++)
            {
                hRegion = xldCont.SelectObj(i).GenRegionContourXld("filled");
                FlawArea = hRegion.AreaCenter(out FlawRow, out FlawCol);
                hRegion.GetRegionContour(out FlawRow, out FlawCol);
                hRegion.SmallestRectangle2(out rectRow, out rectCol, out FlawAngle, out FlawLen1, out FlawLen2);//最小外接矩形
                LineFlawGrayMean = hRegion.Intensity(hImage, out LineFlawGrayDeviation);
                InspRoiMean = hRegion.Intensity(hImage, out Divation); //检测区域的平均灰度
                LineFlawGrayDif = LineFlawGrayMean - InspRoiMean; //灰度相对于平均灰度的灰度偏差
                LineFlawConvexity = hRegion.Convexity();   //凸度
                LineFlawCircularity = hRegion.Circularity(); //圆度 
                LineFlawRectangularity = hRegion.Rectangularity(); //矩形度
                LineFlawCompactness = hRegion.Compactness();      //瑕疵紧密度
                /////////////////////////////////////////////
                if (FlawLen2.D == 0)
                    FlawLen2 = 1.0;
                LineFlawRadioLw = FlawLen1 / FlawLen2;
                /////////////////////////////////////////////
                FlawMsgI = new FlawMsg();
                FlawMsgI.FlawDescribe = "NONE";
                switch (FlawRow.Type)
                {
                    case HTupleType.DOUBLE:
                        for (int k = 0; k < FlawRow.Length; k++)
                        {
                            FlawMsgI.FlawRow.Add(FlawRow[k]);
                        }
                        break;
                    case HTupleType.LONG:
                        for (int k = 0; k < FlawRow.Length; k++)
                        {
                            FlawMsgI.FlawRow.Add(FlawRow[k]);
                        }
                        break;
                }
                switch (FlawRow.Type)
                {
                    case HTupleType.DOUBLE:
                        for (int k = 0; k < FlawRow.Length; k++)
                        {
                            FlawMsgI.FlawCol.Add(FlawCol[k]);
                        }
                        break;
                    case HTupleType.LONG:
                        for (int k = 0; k < FlawRow.Length; k++)
                        {
                            FlawMsgI.FlawCol.Add(FlawCol[k]);
                        }
                        break;
                }
                FlawMsgI.FlawArea = FlawArea.D;
                FlawMsgI.FlawLen1 = FlawLen1.D;
                FlawMsgI.FlawLen2 = FlawLen2.D;
                FlawMsgI.CenterRow = Math.Round(rectRow.D, 3);
                FlawMsgI.CenterCol = Math.Round(rectCol.D, 3);
                FlawMsgI.FlawRect2Phi = Math.Round(FlawAngle.D, 3);
                FlawMsgI.FlawLw = Math.Round(LineFlawRadioLw.D, 3);
                FlawMsgI.FlawGrayMean = Math.Round(LineFlawGrayMean.D, 3);
                FlawMsgI.FlawGrayDeviation = Math.Round(LineFlawGrayDeviation.D, 3);
                //FlawMsgI.FlawGrayDif = Math.Round(LineFlawGrayDif.D, 3);
                //FlawMsgI.InspRoiGrayMean = Math.Round(InspRoiMean.D, 3);
                FlawMsgI.FlawConvexity = Math.Round(LineFlawConvexity.D, 3);
                FlawMsgI.FlawCircularity = Math.Round(LineFlawCircularity.D, 3);
                FlawMsgI.FlawRectangularity = Math.Round(LineFlawRectangularity.D, 3);
                FlawMsgI.FlawCompactness = Math.Round(LineFlawCompactness.D, 3);
                FlawMsgList.Add(FlawMsgI);
            }
            return FlawMsgList;

        }

        /// <summary>
        /// 高亮显示点击的缺陷
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 瑕疵数据dataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0) return;
                ViewData data = this.drawObject.AttachPropertyData[e.RowIndex] as ViewData;
                for (int i = 0; i < this.drawObject.AttachPropertyData.Count; i++)
                {
                    if (i == e.RowIndex)
                    {
                        data = this.drawObject.AttachPropertyData[i] as ViewData;
                        data.Color = enColor.orange.ToString();
                    }
                    else
                    {
                        data = this.drawObject.AttachPropertyData[i] as ViewData;
                        data.Color = enColor.red.ToString();
                    }
                }
                this.drawObject.DrawingGraphicObject();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void 测试Btn_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.DicFlaw.ContainsKey("1"))
                    this.DicFlaw["1"].FlawMessage.Clear();
                this.drawObject.ClearViewObject();
                TreeNode node = null;
                foreach (KeyValuePair<string, TreeViewWrapClass> item in ProgramForm.Instance.ProgramDic)
                {
                    foreach (TreeNode item2 in item.Value.TreeView.Nodes)
                    {
                        GetEditeNode(item2, out node);
                        if (node != null) break;
                    }
                }
                if (node != null)
                {
                    foreach (TreeNode item2 in node.Nodes)
                    {
                        ((IFunction)item2.Tag).Execute(this.drawObject.BackImage);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }


        private CancellationTokenSource cts2;
        private void Run(ImageDataClass imageData)
        {
            this.imageList.TryDequeue(out imageData);
            TreeNode node = null;
            foreach (KeyValuePair<string, TreeViewWrapClass> item in ProgramForm.Instance.ProgramDic)
            {
                foreach (TreeNode item2 in item.Value.TreeView.Nodes)
                {
                    GetEditeNode(item2, out node);
                    if (node != null) break;
                }
            }
            if (node != null)
            {
                foreach (TreeNode item2 in node.Nodes)
                {
                    ((IFunction)item2.Tag).Execute(item2, imageData);
                }
            }
        }


        private HImage GenFlawImage(HObject hObject, HImage sourceImage)
        {
            HImage flawImge = new HImage();
            if (hObject == null || !hObject.IsInitialized()) throw new ArgumentNullException("hObject 为空或未初始化!");
            if (sourceImage == null || !sourceImage.IsInitialized()) throw new ArgumentNullException("sourceImage 为空或未初始化!");
            switch (hObject.GetType().Name)
            {
                case nameof(HXLDCont):
                    HXLDCont hXLDCont = new HXLDCont(hObject);
                    HImage redImage = hXLDCont.PaintXld(sourceImage, 255.0);
                    HImage darkImage = hXLDCont.PaintXld(sourceImage, 0.0);
                    flawImge = redImage.Compose3(darkImage, darkImage);
                    break;
                case nameof(HRegion):
                    HRegion hRegion = new HRegion(hObject);
                    redImage = hRegion.PaintRegion(sourceImage, 255.0, "margin");
                    darkImage = hRegion.PaintRegion(sourceImage, 0.0, "margin");
                    flawImge = redImage.Compose3(darkImage, darkImage);
                    break;
                default:
                    throw new ArgumentException("hObject 参数类型错误，请指定为 HRegion 或 HXLDCont!");
            }
            return flawImge;
        }

        private void 加载图片button_Click(object sender, EventArgs e)
        {
            try
            {
                string path = new FileOperate().OpenImage();
                ReadImageParam imageParam = new ReadImageParam();
                imageParam.ReadImage(path);
                this.sourceImage = new HImage(path);
                if (this.sourceImage != null)
                    this.drawObject.BackImage = new ImageDataClass(this.sourceImage); //, this._acqSource.Sensor.CameraParam
                else
                    throw new ArgumentException("读取的图像为空");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void 瑕疵数据dataGridView_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false;
        }
    }
}


using Common;
using HalconDotNet;
using MotionControlCard;
using Sensor;
using System;
using System.Collections.Generic;
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
    public partial class LoadViewForm : Form
    {
        private List<ImageDataClass> imageList = new List<ImageDataClass>();
        private Dictionary<string, object> listData = new Dictionary<string, object>();
        private VisualizeView drawObject;
        private ViewConfigParam _viewConfigParam;
        private bool IsLoad = false; // 窗体加载后，设置为true
        private AcqSource acqSource;
        private Queue<object> DataList = new Queue<object>();
        private object lockSys = new object();
        private PixROI _ROI;
        public PixROI ROI { get => _ROI; set => _ROI = value; }


        public LoadViewForm(ViewConfigParam viewConfigParam)
        {
            InitializeComponent();
            /////////////
            this.drawObject = new VisualizeView(this.hWindowControl1, false);
            this.drawObject.HMouseDoubleClick += new HMouseEventHandler(this.hWindowControl1_DoubleClick);
            BaseFunction.ExcuteCompleted += new ExcuteCompletedEventHandler(DisplayExcuteResult);
            this.drawObject.GrayValueInfo += new GrayValueInfoEventHandler(GetGrayValueInfo);
            LoadTryForm.EditorEvent += new EventHandler(LoadTryForm_EditorEvent);
            this._viewConfigParam = viewConfigParam;
            if (!HWindowManage.HWindowList.ContainsKey(viewConfigParam.ViewName))
            {
                HWindowManage.HWindowList.Add(viewConfigParam.ViewName, this.hWindowControl1.HalconWindow);
            }
            if (!HWindowManage.HWindowControlList.ContainsKey(viewConfigParam.ViewName))
            {
                HWindowManage.HWindowControlList.Add(viewConfigParam.ViewName, this.hWindowControl1);
            }
            this.ContextMenu = new ContextMenu();
            this.titleLabel.Text = viewConfigParam.ViewName;
            this.titleLabel.TextAlign = ContentAlignment.MiddleLeft;
            ////
            //UserLoginParamManager.Instance.LoginParam.UserChange += new EventHandler(this.UserChange_Event);
            UserLoginParamManager.Instance.UserChange += new EventHandler(this.UserChange_Event);
        }
        private void LoadViewForm_Load(object sender, EventArgs e)
        {
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
        }
        private void BindProperty()
        {
            try
            {
                this.传感器comboBox1.Items.Clear();
                this.传感器comboBox1.Items.AddRange(AcqSourceManage.Instance.GetAcqSourceName());
                this.传感器comboBox1.Text = this._viewConfigParam.CamName;
                //this.程序节点comboBox.Items.Add(this._viewConfigParam.ProgramNode);
                //this.程序节点comboBox.SelectedItem = this._viewConfigParam.ProgramNode;
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
                        this.传感器comboBox1.Enabled = false;
                        //this.程序节点comboBox.Enabled = false;
                        this.buttonClose.Enabled = false;
                        break;
                    case enUserName.工程师:
                        this.传感器comboBox1.Enabled = true;
                        //this.程序节点comboBox.Enabled = true;
                        this.buttonClose.Enabled = false;
                        break;
                    case enUserName.开发人员:
                        this.传感器comboBox1.Enabled = true;
                        //this.程序节点comboBox.Enabled = true;
                        this.buttonClose.Enabled = true;
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
            if (!IsSelect()) return;  // 当前面的条件都满足时，设置当前选项卡
            if (e.DataContent != null)
            {
                ViewData viewData;
                switch (e.DataContent.GetType().Name)
                {
                    case nameof(ImageDataClass):
                        this.ClearGraphic(null, null);
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
                        break;
                    case nameof(userWcsVector):
                        userWcsVector wcsVector = (userWcsVector)e.DataContent;
                        userWcsPoint wcsPoint1 = new userWcsPoint(wcsVector.X, wcsVector.Y, wcsVector.Z, wcsVector.CamParams);
                        this.drawObject.AddViewObject(new ViewData((wcsPoint1).GetPixPoint(), wcsPoint1.Color.ToString()));
                        break;
                    case nameof(userWcsLine):
                        userWcsLine wcsLine = (userWcsLine)e.DataContent;
                        this.drawObject.AddViewObject(new ViewData((wcsLine).GetPixLine().GetXLD(), wcsLine.Color.ToString()));
                        break;

                    case nameof(userWcsCircle):
                        userWcsCircle wcsCircle = (userWcsCircle)e.DataContent;
                        this.drawObject.AddViewObject(new ViewData((wcsCircle).GetPixCircle().GetXLD(), wcsCircle.Color.ToString()));
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
                    case nameof(userWcsCoordSystem):
                        userWcsCoordSystem wcsCoordSystem = (userWcsCoordSystem)e.DataContent;
                        this.drawObject.AddViewObject(new ViewData(wcsCoordSystem, "red"));
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

        private void ImageViewForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                this.cts?.Cancel();
                this.IsLoad = false;
                ViewConfigParamManager.Instance.ViewParamList.Remove(this._viewConfigParam); // 关闭窗体时要删除相应的对象
                BaseFunction.ExcuteCompleted -= new ExcuteCompletedEventHandler(DisplayExcuteResult);
                this.drawObject.GrayValueInfo -= new GrayValueInfoEventHandler(GetGrayValueInfo);
                this.drawObject.HMouseDoubleClick -= new HMouseEventHandler(this.hWindowControl1_DoubleClick);
                LoadTryForm.EditorEvent -= new EventHandler(LoadTryForm_EditorEvent);
                //UserLoginParamManager.Instance.LoginParam.UserChange -= new EventHandler(this.UserChange_Event);
                UserLoginParamManager.Instance.UserChange -= new EventHandler(this.UserChange_Event);
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
                        this.acqSource = AcqSourceManage.Instance.GetCamAcqSource(this._viewConfigParam.CamName);
                        if (this.acqSource == null) return;
                        cts = new CancellationTokenSource();
                        Dictionary<enDataItem, object> data;
                        Task.Run(() =>
                        {
                            this.drawObject.IsLiveState = true;
                            while (!cts.IsCancellationRequested)
                            {
                                if (SystemParamManager.Instance.SysConfigParam.IsAutoRun)
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
                MessageBox.Show(ex.ToString());
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
                                //if (item2.Text == this._viewConfigParam.ProgramNode)
                                //    item.Value.treeView1_Edite(this, item2);
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
                    case "清除窗口(Clear)":
                        this.drawObject.ClearWindow();
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void GetGrayValueInfo(object sender, GrayValueInfoEventArgs e)
        {
            if (e.GaryValue.Length > 0)
            {
                int row1, col1, row2, col2;
                this.hWindowControl1.HalconWindow.GetPart(out row1, out col1, out row2, out col2);
                this.hWindowControl1.HalconWindow.SetTposition((int)(row1 + (row2 - row1) * 0.025), (int)(col1 + (col2 - col1) * 0.015));
                //this.hWindowControl1.HalconWindow.SetTposition((int)(row2 - (row2 - row1) * 0.08), (int)(col2 - (col2 - col1) * 0.5));
                //this.hWindowControl1.HalconWindow.SetTposition((int)(e.Row), (int)e.Col + 50);
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

        private void label1_MouseDown(object sender, MouseEventArgs e)
        {
            ViewForm_MouseDown(null, null);
        }

        private void label1_MouseEnter(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Hand;
            this.titleLabel.BackColor = System.Drawing.Color.Orange;// System.Drawing.SystemColors.HotTrack;
        }

        private void label1_MouseLeave(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Default;
            this.titleLabel.BackColor = System.Drawing.Color.LightGray;// System.Drawing.SystemColors.Control;
        }


        private void hWindowControl1_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy; // 在进入该控件时必需要设置拖放效果，不然拖放完成事件不会执行 ，， 将要显示的元素拖放到该窗口上来
        }


        private void 传感器comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        private void 传感器comboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            try
            {
                if (this.IsLoad)
                {
                    this._viewConfigParam.CamName = this.传感器comboBox1.SelectedItem.ToString();
                    ViewConfigParamManager.Instance.Save();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("保存视图配置参数出错" + ex.ToString());
            }

        }


        private void hWindowControl1_DoubleClick(Object sender, HalconDotNet.HMouseEventArgs e)
        {
            //int a = 10;
            this.buttonMax_Click(null, null);
        }

        private void 程序节点comboBox_SelectionChangeCommitted_1(object sender, EventArgs e)
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

        private void 程序节点comboBox_DropDown_1(object sender, EventArgs e)
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
                foreach (TreeNode item in node.Nodes)
                {
                    GetToolNode(item);
                }
            }
            else
            {
                if (node.Parent == null)
                {
                    if (!this.程序节点comboBox.Items.Contains(node.FullPath.Replace("\\", ".")))
                        this.程序节点comboBox.Items.Add(node.FullPath.Replace("\\", "."));
                    else
                        return;
                }
                else
                {
                    if (!this.程序节点comboBox.Items.Contains(node.Parent.FullPath.Replace("\\", ".")))
                        this.程序节点comboBox.Items.Add(node.Parent.FullPath.Replace("\\", "."));
                    else
                        return;
                }
            }
        }

        private void GetEditeNode(TreeNode node, out TreeNode selectNode)
        {
            selectNode = null;
            if (node.Name.Contains("Tool"))
            {
                if (node.FullPath.Replace("\\", ".") == this._viewConfigParam.ProgramNode)
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

        private void 视图工具toolStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            //string name = e.ClickedItem.Name;
            //switch (name)
            //{
            //    case "toolStripButton_Clear":
            //        this.drawObject.ClearWindow();
            //        this.toolStripButton_Translate.CheckState = CheckState.Unchecked;
            //        break;
            //    case "toolStripButton_Select":
            //        this.drawObject.Select();
            //        this.toolStripButton_Translate.CheckState = CheckState.Unchecked;
            //        break;
            //    case "toolStripButton_Translate":
            //        this.drawObject.TranslateScaleImage();
            //        this.toolStripButton_Translate.CheckState = CheckState.Checked;
            //        break;
            //    case "toolStripButton_Auto":
            //        this.drawObject.AutoImage();
            //        this.toolStripButton_Translate.CheckState = CheckState.Unchecked;

            //        break;
            //    case "toolStripButton_3D":
            //        this.drawObject.Show3D();
            //        this.toolStripButton_Translate.CheckState = CheckState.Unchecked;
            //        break;
            //    case "绘图toolStripDropDownButton":
            //        ToolStripItemClickedEventArgs toolStripItem = new ToolStripItemClickedEventArgs(new ToolStripMenuItem());
            //        toolStripItem.ClickedItem.Text = this.绘图toolStripDropDownButton.Text;
            //        //this.绘图toolStripDropDownButton_DropDownItemClicked(this, toolStripItem);
            //        break;
            //    case "删除ROItoolStripButton":
            //        this._ROI = null;
            //        this.drawObject?.ClearViewObject();
            //        break;
            //    default:
            //        break;
            //}
        }

        private void LoadTryForm_EditorEvent(object sender, EventArgs e)
        {
            UserTryPlateHoleParam param1 = sender as UserTryPlateHoleParam;
            this._ROI = param1.PixRoi;
            PixROI pixShape;
            enShapeType ShapeType = enShapeType.矩形1;
            //this._ROI = null;
            /////////////////////////////////////////////////////
            if (this._ROI != null)
            {
                switch (this._ROI.GetType().Name)
                {
                    case nameof(drawPixCircle):
                        ShapeType = enShapeType.圆;
                        break;
                    case nameof(drawPixRect1):
                        ShapeType = enShapeType.矩形1;
                        break;
                    case nameof(drawPixRect2):
                        ShapeType = enShapeType.矩形2;
                        break;
                    case nameof(drawPixEllipse):
                        ShapeType = enShapeType.椭圆;
                        break;
                    case nameof(drawPixLine):
                        ShapeType = enShapeType.线;
                        break;
                    case nameof(drawPixPolygon):
                        ShapeType = enShapeType.多边形;
                        break;
                }
            }
            Task.Run(() =>
            {
                switch (ShapeType)
                {
                    case enShapeType.矩形2:
                        this.drawObject.AttachPropertyData.Clear();
                        if (!(this.drawObject is userDrawRect2ROI))
                        {
                            this._ROI = null;
                            this.drawObject.HMouseDoubleClick -= new HMouseEventHandler(this.hWindowControl1_DoubleClick);
                            this.drawObject.GrayValueInfo -= new GrayValueInfoEventHandler(GetGrayValueInfo);
                            this.drawObject.ClearDrawingObject();
                            this.drawObject = new userDrawRect2ROI(this.hWindowControl1, false);
                            this.drawObject.GrayValueInfo += new GrayValueInfoEventHandler(GetGrayValueInfo);
                            this.drawObject.HMouseDoubleClick += new HMouseEventHandler(this.hWindowControl1_DoubleClick);
                        }
                        break;
                    case enShapeType.矩形1:
                        this.drawObject.AttachPropertyData.Clear();
                        if (!(this.drawObject is userDrawRect1ROI))
                        {
                            this._ROI = null;
                            this.drawObject.HMouseDoubleClick -= new HMouseEventHandler(this.hWindowControl1_DoubleClick);
                            this.drawObject.GrayValueInfo -= new GrayValueInfoEventHandler(GetGrayValueInfo);
                            this.drawObject.ClearDrawingObject();
                            this.drawObject = new userDrawRect1ROI(this.hWindowControl1, false);
                            this.drawObject.GrayValueInfo += new GrayValueInfoEventHandler(GetGrayValueInfo);
                            this.drawObject.HMouseDoubleClick += new HMouseEventHandler(this.hWindowControl1_DoubleClick);
                        }
                        break;
                    case enShapeType.圆:
                        this.drawObject.AttachPropertyData.Clear();
                        if (!(this.drawObject is userDrawCircleROI))
                        {
                            this._ROI = null;
                            this.drawObject.HMouseDoubleClick -= new HMouseEventHandler(this.hWindowControl1_DoubleClick);
                            this.drawObject.GrayValueInfo -= new GrayValueInfoEventHandler(GetGrayValueInfo);
                            this.drawObject.ClearDrawingObject();
                            this.drawObject = new userDrawCircleROI(this.hWindowControl1, false);
                            this.drawObject.GrayValueInfo += new GrayValueInfoEventHandler(GetGrayValueInfo);
                            this.drawObject.HMouseDoubleClick += new HMouseEventHandler(this.hWindowControl1_DoubleClick);
                        }
                        break;
                    case enShapeType.椭圆:
                        this.drawObject.AttachPropertyData.Clear();
                        if (!(this.drawObject is userDrawEllipseROI))
                        {
                            this._ROI = null;
                            this.drawObject.HMouseDoubleClick -= new HMouseEventHandler(this.hWindowControl1_DoubleClick);
                            this.drawObject.GrayValueInfo -= new GrayValueInfoEventHandler(GetGrayValueInfo);
                            this.drawObject.ClearDrawingObject();
                            this.drawObject = new userDrawEllipseROI(this.hWindowControl1, false);
                            this.drawObject.GrayValueInfo += new GrayValueInfoEventHandler(GetGrayValueInfo);
                            this.drawObject.HMouseDoubleClick += new HMouseEventHandler(this.hWindowControl1_DoubleClick);
                        }
                        break;
                    case enShapeType.多边形:
                        this.drawObject.AttachPropertyData.Clear();
                        if (!(this.drawObject is userDrawPolygonROI))
                        {
                            this._ROI = null;
                            this.drawObject.HMouseDoubleClick -= new HMouseEventHandler(this.hWindowControl1_DoubleClick);
                            this.drawObject.GrayValueInfo -= new GrayValueInfoEventHandler(GetGrayValueInfo);
                            this.drawObject.ClearDrawingObject();
                            this.drawObject = new userDrawPolygonROI(this.hWindowControl1, false);
                            this.drawObject.GrayValueInfo += new GrayValueInfoEventHandler(GetGrayValueInfo);
                            this.drawObject.HMouseDoubleClick += new HMouseEventHandler(this.hWindowControl1_DoubleClick);
                        }
                        break;
                    case enShapeType.点:
                        this.drawObject.AttachPropertyData.Clear();
                        if (!(this.drawObject is userDrawPointROI))
                        {
                            this._ROI = null;
                            this.drawObject.HMouseDoubleClick -= new HMouseEventHandler(this.hWindowControl1_DoubleClick);
                            this.drawObject.GrayValueInfo -= new GrayValueInfoEventHandler(GetGrayValueInfo);
                            this.drawObject.ClearDrawingObject();
                            this.drawObject = new userDrawPointROI(this.hWindowControl1, false);
                            this.drawObject.GrayValueInfo += new GrayValueInfoEventHandler(GetGrayValueInfo);
                            this.drawObject.HMouseDoubleClick += new HMouseEventHandler(this.hWindowControl1_DoubleClick);
                        }
                        break;
                    case enShapeType.线:
                        this.drawObject.AttachPropertyData.Clear();
                        if (!(this.drawObject is userDrawLineROI))
                        {
                            this._ROI = null;
                            this.drawObject.HMouseDoubleClick -= new HMouseEventHandler(this.hWindowControl1_DoubleClick);
                            this.drawObject.GrayValueInfo -= new GrayValueInfoEventHandler(GetGrayValueInfo);
                            this.drawObject.ClearDrawingObject();
                            this.drawObject = new userDrawLineROI(this.hWindowControl1, false);
                            this.drawObject.GrayValueInfo += new GrayValueInfoEventHandler(GetGrayValueInfo);
                            this.drawObject.HMouseDoubleClick += new HMouseEventHandler(this.hWindowControl1_DoubleClick);
                        }
                        break;
                    default:
                        throw new NotImplementedException("未实现的绘图类型!");
                }
                this.drawObject.IsLiveState = true;
                //////////////////////////
                //if (this.drawObject.BackImage == null)
                //    this.drawObject.BackImage = this._imageData;
                if (this._ROI == null)
                    this.drawObject.SetParam(null);
                else
                {
                    this.drawObject.SetParam(this._ROI);
                }
                this.drawObject.DrawPixRoiShapeOnWindow(enColor.red, out pixShape);
                this.drawObject.AddViewObject(new ViewData(pixShape.GetXLD(), enColor.orange.ToString()));
                /////////////////////////////////////////////////////////
                this._ROI = pixShape; //.GetWcsROI(this.drawObject.CameraParam);  // 这个地方的添加不能使用变换后数据
                param1.PixRoi = pixShape;
            });
        }
        private void 绘图toolStripDropDownButton_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            try
            {

                string name = e.ClickedItem.Text;
                //Task.Run(() =>
                //{
                //    switch (name)
                //    {
                //        case "矩形1":
                //            ShapeType = enShapeType.矩形1;
                //            this.Invoke(new Action(() => this.绘图toolStripDropDownButton.Text = enShapeType.矩形1.ToString()));
                //            this.绘图toolStripDropDownButton.Text = enShapeType.矩形1.ToString();
                //            break;
                //        case "矩形2":
                //            ShapeType = enShapeType.矩形2;
                //            this.Invoke(new Action(() => this.绘图toolStripDropDownButton.Text = enShapeType.矩形2.ToString()));
                //            break;
                //        case "圆形":
                //            ShapeType = enShapeType.圆;
                //            this.Invoke(new Action(() => this.绘图toolStripDropDownButton.Text = enShapeType.圆.ToString()));
                //            break;
                //        case "直线":
                //            ShapeType = enShapeType.线;
                //            this.Invoke(new Action(() => this.绘图toolStripDropDownButton.Text = enShapeType.线.ToString()));
                //            break;
                //        case "椭圆":
                //            ShapeType = enShapeType.椭圆;
                //            this.Invoke(new Action(() => this.绘图toolStripDropDownButton.Text = enShapeType.椭圆.ToString()));
                //            break;
                //        case "多边形":
                //            ShapeType = enShapeType.多边形;
                //            this.Invoke(new Action(() => this.绘图toolStripDropDownButton.Text = enShapeType.多边形.ToString()));
                //            break;
                //        default:
                //            break;
                //    }
                //    if (ShapeType == enShapeType.NONE) return;
                //    switch (ShapeType)
                //    {
                //        case enShapeType.矩形2:
                //            this.drawObject.AttachPropertyData.Clear();
                //            if (!(this.drawObject is userDrawRect2ROI))
                //            {
                //                this._ROI = null;
                //                this.drawObject.HMouseDoubleClick -= new HMouseEventHandler(this.hWindowControl1_DoubleClick);
                //                this.drawObject.GrayValueInfo -= new GrayValueInfoEventHandler(GetGrayValueInfo);
                //                this.drawObject.ClearDrawingObject();
                //                this.drawObject = new userDrawRect2ROI(this.hWindowControl1, false);
                //                this.drawObject.GrayValueInfo += new GrayValueInfoEventHandler(GetGrayValueInfo);
                //                this.drawObject.HMouseDoubleClick += new HMouseEventHandler(this.hWindowControl1_DoubleClick);
                //            }
                //            break;
                //        case enShapeType.矩形1:
                //            this.drawObject.AttachPropertyData.Clear();
                //            if (!(this.drawObject is userDrawRect1ROI))
                //            {
                //                this._ROI = null;
                //                this.drawObject.HMouseDoubleClick -= new HMouseEventHandler(this.hWindowControl1_DoubleClick);
                //                this.drawObject.GrayValueInfo -= new GrayValueInfoEventHandler(GetGrayValueInfo);
                //                this.drawObject.ClearDrawingObject();
                //                this.drawObject = new userDrawRect1ROI(this.hWindowControl1, false);
                //                this.drawObject.GrayValueInfo += new GrayValueInfoEventHandler(GetGrayValueInfo);
                //                this.drawObject.HMouseDoubleClick += new HMouseEventHandler(this.hWindowControl1_DoubleClick);
                //            }
                //            break;
                //        case enShapeType.圆:
                //            this.drawObject.AttachPropertyData.Clear();
                //            if (!(this.drawObject is userDrawCircleROI))
                //            {
                //                this._ROI = null;
                //                this.drawObject.HMouseDoubleClick -= new HMouseEventHandler(this.hWindowControl1_DoubleClick);
                //                this.drawObject.GrayValueInfo -= new GrayValueInfoEventHandler(GetGrayValueInfo);
                //                this.drawObject.ClearDrawingObject();
                //                this.drawObject = new userDrawCircleROI(this.hWindowControl1, false);
                //                this.drawObject.GrayValueInfo += new GrayValueInfoEventHandler(GetGrayValueInfo);
                //                this.drawObject.HMouseDoubleClick += new HMouseEventHandler(this.hWindowControl1_DoubleClick);
                //            }
                //            break;
                //        case enShapeType.椭圆:
                //            this.drawObject.AttachPropertyData.Clear();
                //            if (!(this.drawObject is userDrawEllipseROI))
                //            {
                //                this._ROI = null;
                //                this.drawObject.HMouseDoubleClick -= new HMouseEventHandler(this.hWindowControl1_DoubleClick);
                //                this.drawObject.GrayValueInfo -= new GrayValueInfoEventHandler(GetGrayValueInfo);
                //                this.drawObject.ClearDrawingObject();
                //                this.drawObject = new userDrawEllipseROI(this.hWindowControl1, false);
                //                this.drawObject.GrayValueInfo += new GrayValueInfoEventHandler(GetGrayValueInfo);
                //                this.drawObject.HMouseDoubleClick += new HMouseEventHandler(this.hWindowControl1_DoubleClick);
                //            }
                //            break;
                //        case enShapeType.多边形:
                //            this.drawObject.AttachPropertyData.Clear();
                //            if (!(this.drawObject is userDrawPolygonROI))
                //            {
                //                this._ROI = null;
                //                this.drawObject.HMouseDoubleClick -= new HMouseEventHandler(this.hWindowControl1_DoubleClick);
                //                this.drawObject.GrayValueInfo -= new GrayValueInfoEventHandler(GetGrayValueInfo);
                //                this.drawObject.ClearDrawingObject();
                //                this.drawObject = new userDrawPolygonROI(this.hWindowControl1, false);
                //                this.drawObject.GrayValueInfo += new GrayValueInfoEventHandler(GetGrayValueInfo);
                //                this.drawObject.HMouseDoubleClick += new HMouseEventHandler(this.hWindowControl1_DoubleClick);
                //            }
                //            break;
                //        case enShapeType.点:
                //            this.drawObject.AttachPropertyData.Clear();
                //            if (!(this.drawObject is userDrawPointROI))
                //            {
                //                this._ROI = null;
                //                this.drawObject.HMouseDoubleClick -= new HMouseEventHandler(this.hWindowControl1_DoubleClick);
                //                this.drawObject.GrayValueInfo -= new GrayValueInfoEventHandler(GetGrayValueInfo);
                //                this.drawObject.ClearDrawingObject();
                //                this.drawObject = new userDrawPointROI(this.hWindowControl1, false);
                //                this.drawObject.GrayValueInfo += new GrayValueInfoEventHandler(GetGrayValueInfo);
                //                this.drawObject.HMouseDoubleClick += new HMouseEventHandler(this.hWindowControl1_DoubleClick);
                //            }
                //            break;
                //        case enShapeType.线:
                //            this.drawObject.AttachPropertyData.Clear();
                //            if (!(this.drawObject is userDrawLineROI))
                //            {
                //                this._ROI = null;
                //                this.drawObject.HMouseDoubleClick -= new HMouseEventHandler(this.hWindowControl1_DoubleClick);
                //                this.drawObject.GrayValueInfo -= new GrayValueInfoEventHandler(GetGrayValueInfo);
                //                this.drawObject.ClearDrawingObject();
                //                this.drawObject = new userDrawLineROI(this.hWindowControl1, false);
                //                this.drawObject.GrayValueInfo += new GrayValueInfoEventHandler(GetGrayValueInfo);
                //                this.drawObject.HMouseDoubleClick += new HMouseEventHandler(this.hWindowControl1_DoubleClick);
                //            }
                //            break;
                //        default:
                //            throw new NotImplementedException("未实现的绘图类型!");
                //    }
                //    this.drawObject.IsLiveState = true;
                //    //////////////////////////
                //    //if (this.drawObject.BackImage == null)
                //    //    this.drawObject.BackImage = this._imageData;
                //    if (this._ROI == null)
                //        this.drawObject.SetParam(null);
                //    else
                //    {
                //        this.drawObject.SetParam(this._ROI);
                //    }
                //    this.drawObject.DrawPixRoiShapeOnWindow(enColor.red, out pixShape);
                //    this.drawObject.AddViewObject(new ViewData(pixShape.GetXLD(), enColor.orange.ToString()));
                //    /////////////////////////////////////////////////////////
                //    this._ROI = pixShape; //.GetWcsROI(this.drawObject.CameraParam);  // 这个地方的添加不能使用变换后数据
                //});
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }


    }
}


using AlgorithmsLibrary;
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
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using View;

namespace FunctionBlock
{

    public partial class CamMapCalibParamSimpleForm : Form
    {
        private AcqSource _acqSource;
        private CancellationTokenSource cts;
        //private CameraCalibrateTool calibrateCamera;
        private ImageDataClass image = null;
        private double error = 0;
        private DrawingBaseMeasure drawObject;
        private TreeViewWrapClass _treeViewWrapClass;
        private ImageDataClass CurrentImageData;
        private CameraParam CamParam;
        private CameraParam MapCamParam;
        private MetrolegyParamForm metrolegyParamForm;
        private IFunction _currFunction;
        private CalibCoordConfigParamManager calibCoordConfigParamRota;
        private Dictionary<string, object> listData = new Dictionary<string, object>();
        //private Dictionary<string, userPixPoint> listPixPoint = new Dictionary<string, userPixPoint>();
        private BindingList<userPixPoint> listPixPoint = new BindingList<userPixPoint>();
        private BindingList<CoordSysAxisPosParam> listWcsPoint = new BindingList<CoordSysAxisPosParam>();
        private BindingList<CoordSysAxisPosParam> listGrabPoint = new BindingList<CoordSysAxisPosParam>();
        private BindingList<userWcsPoint> listWcsSourcePoint = new BindingList<userWcsPoint>();
        private BindingList<userWcsPoint> listWcsTargetPoint = new BindingList<userWcsPoint>();
        private double Grab_x = 0, Grab_y = 0;
        private bool isStop = false;
        private bool isLoad = false;
        private string programPath = "VisionParam\\标定程序\\映射标定程序"; // 程序文件路径
        private static object lockSynState = new object();
        private static CamNPointCalibParamSimpleForm _Instance;

        public CamMapCalibParamSimpleForm()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.Manual;
            this.Location = System.Windows.Forms.Cursor.Position;
            this.drawObject = new DrawingBaseMeasure(this.hWindowControl1, false);
            this._treeViewWrapClass = new TreeViewWrapClass(this.treeView1, this);
            this.metrolegyParamForm = new MetrolegyParamForm(this.drawObject);
            // 自动打开程序
            if (this.CamParam != null)
            {
                this.programPath += "\\" + this.CamParam.SensorName;
                this._treeViewWrapClass.OpenProgram(this.programPath);
            }
            this.addContextMenu(this.hWindowControl1);
            this.drawObject.GrayValueInfo += new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
            BaseFunction.ExcuteCompleted += new ExcuteCompletedEventHandler(DisplayExcuteResult);
            TreeViewWrapClass.ClickNode += new ClickNodeEventHandler(this.DisplayClickObject);
            BindProperty();
            this.AddForm(this.元素信息tabPage, new ElementViewForm(false));
        }
        public CamMapCalibParamSimpleForm(CameraParam CamParam)
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.Manual;
            this.Location = System.Windows.Forms.Cursor.Position;
            this.CamParam = CamParam;
            this.MapCamParam = AcqSourceManage.Instance.GetAcqSource(this.CamParam.CaliParam.MapCamName)?.Sensor?.CameraParam;
            this.drawObject = new DrawingBaseMeasure(this.hWindowControl1, false);
            this._treeViewWrapClass = new TreeViewWrapClass(this.treeView1, this);
            this.metrolegyParamForm = new MetrolegyParamForm(this.drawObject);
            // 自动打开程序
            this.programPath += "\\" + CamParam?.SensorName;
            this._treeViewWrapClass.OpenProgram(this.programPath);
            this.addContextMenu(this.hWindowControl1);
            this.drawObject.GrayValueInfo += new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
            BaseFunction.ExcuteCompleted += new ExcuteCompletedEventHandler(DisplayExcuteResult);
            TreeViewWrapClass.ClickNode += new ClickNodeEventHandler(this.DisplayClickObject);
            BindProperty();
            this.AddForm(this.元素信息tabPage, new ElementViewForm(false));
        }
        private void CamNPointCalibParamSimpleForm_Load(object sender, EventArgs e)
        {
            //this.PaintCoordSys(this.panel1);
            this.isLoad = true;
            this.AutoForm();
            this._acqSource = AcqSourceManage.Instance.GetAcqSource(CamParam.SensorName);
        }
        private void AutoForm()
        {
            try
            {
                if (this.Tag != null)
                {
                    string[] value = this.Tag.ToString().Split(',', ';', ':');
                    double width = 0, height = 0;
                    if (value.Length > 0)
                        double.TryParse(value[0], out width);
                    if (value.Length > 1)
                        double.TryParse(value[1], out height);
                    if (width > 0 && height > 0)
                    {
                        double scsleWidth = (Screen.PrimaryScreen.WorkingArea.Width * 1.0) / width;
                        double scsleHeight = (Screen.PrimaryScreen.WorkingArea.Height * 1.0) / height;
                        this.Width = (int)(this.Width * scsleWidth);
                        this.Height = (int)(this.Height * scsleHeight);
                    }
                }
            }
            catch
            { }
        }
        private void BindProperty()
        {
            try
            {
                //this.世界点dataGridView.DataSource = this.listWcsPoint;
                //this.图像点dataGridView.DataSource = this.listPixPoint;
                this.标定轴comboBox.DataSource = Enum.GetValues(typeof(enCalibAxis)); // 这里不需要更改本标定坐标系，只需要显示即可
                this.标定平面comboBox.DataSource = Enum.GetValues(typeof(enCalibPlane));
                this.运控平台comboBox.DataSource = Enum.GetValues(typeof(enMoveStage));
                this.坐标值类型comboBox.DataSource = Enum.GetValues(typeof(enCoordValueType)); // 这里不需要更改本标定坐标系，只需要显示即可
                this.标定平面comboBox.DataBindings.Add(nameof(this.标定平面comboBox.Text), this.CamParam.CaliParam, nameof(this.CamParam.CaliParam.CalibPlane), true, DataSourceUpdateMode.OnPropertyChanged);
                this.标定轴comboBox.DataBindings.Add(nameof(this.标定轴comboBox.Text), this.CamParam.CaliParam, nameof(this.CamParam.CaliParam.CalibAxis), true, DataSourceUpdateMode.OnPropertyChanged);
                this.坐标值类型comboBox.DataBindings.Add(nameof(this.坐标值类型comboBox.Text), this.CamParam.CaliParam, nameof(this.CamParam.CaliParam.CoordValueType), true, DataSourceUpdateMode.OnPropertyChanged);
                this.运控平台comboBox.DataBindings.Add(nameof(this.运控平台comboBox.Text), this.CamParam.CaliParam, nameof(this.CamParam.CaliParam.MoveStage), true, DataSourceUpdateMode.OnPropertyChanged);
                this.取反X轴checkBox.DataBindings.Add(nameof(this.取反X轴checkBox.Checked), this.CamParam.CaliParam, nameof(this.CamParam.CaliParam.InvertX), true, DataSourceUpdateMode.OnPropertyChanged);
                this.取反Y轴checkBox.DataBindings.Add(nameof(this.取反Y轴checkBox.Checked), this.CamParam.CaliParam, nameof(this.CamParam.CaliParam.InvertY), true, DataSourceUpdateMode.OnPropertyChanged);
                this.取反Z轴checkBox.DataBindings.Add(nameof(this.取反Z轴checkBox.Checked), this.CamParam.CaliParam, nameof(this.CamParam.CaliParam.InvertZ), true, DataSourceUpdateMode.OnPropertyChanged);
                this.归一化原点checkBox.DataBindings.Add(nameof(this.归一化原点checkBox.Checked), this.CamParam.CaliParam, nameof(this.CamParam.CaliParam.IsNormalOrigion), true, DataSourceUpdateMode.OnPropertyChanged);
                this.映射方法comboBox.DataBindings.Add(nameof(this.映射方法comboBox.Text), this.CamParam, nameof(this.CamParam.MapType), true, DataSourceUpdateMode.OnPropertyChanged);
            }
            catch (Exception ex)
            {
                new Common.UserMessageForm(ex.ToString()).ShowDialog();
            }
        }

        public static CamNPointCalibParamSimpleForm Instance(CameraParam CamParam)
        {
            //get
            //{
            if (_Instance == null)
            {
                lock (lockSynState)
                {
                    _Instance = new CamNPointCalibParamSimpleForm(CamParam);
                }
            }
            return _Instance;
            //}
        }
        public void SetParam(CameraParam CamParam)
        {
            this.CamParam = CamParam;
            // 自动打开程序
            if (this.CamParam != null)
            {
                this.programPath += "\\" + CamParam.SensorName;
                this._treeViewWrapClass.OpenProgram(this.programPath);
            }
        }


        #region 右键菜单项
        private void addContextMenu(HWindowControl hWindowControl)
        {
            ContextMenuStrip ContextMenuStrip1 = new ContextMenuStrip();
            ContextMenuStrip1.Name = hWindowControl.Name;

            // 添加右键菜单 
            ToolStripItem[] items = new ToolStripMenuItem[]
            {
                new ToolStripMenuItem("执行"),
                new ToolStripMenuItem("自适应窗口"),
                new ToolStripMenuItem("设置抓边参数"),
                new ToolStripMenuItem("------------"),
                new ToolStripMenuItem("清除窗口"),
                new ToolStripMenuItem("保存图像"),
                //new ToolStripMenuItem("显示拖动区"),
                //new ToolStripMenuItem("隐藏拖动区"),
                //new ToolStripMenuItem("关闭窗体"),
            };
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
                    case "设置抓边参数":
                        MetrolegyParamForm paramForm = new MetrolegyParamForm(this._currFunction, this.drawObject);
                        paramForm.Show();
                        paramForm.Owner = this;
                        break;
                    //case "显示拖动区":
                    //    this.拖动label.Show();
                    //    break;
                    //case "隐藏拖动区":
                    //    this.拖动label.Hide();
                    //    break;
                    //case "关闭窗体":
                    //    this.Close();
                    //    break;
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
        private void CalibrateCameraParamForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.isStop = true;
            this.drawObject.GrayValueInfo -= new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
            BaseFunction.ExcuteCompleted -= new ExcuteCompletedEventHandler(DisplayExcuteResult);
            TreeViewWrapClass.ClickNode -= new ClickNodeEventHandler(this.DisplayClickObject);
            this._treeViewWrapClass?.Uinit();
        }


        private void AddForm(TabPage MastPanel, Form form)
        {
            if (MastPanel == null) return;
            if (MastPanel.Controls.Count > 0)
                MastPanel.Controls.Clear();
            if (form == null) return;
            form.TopLevel = false;
            form.FormBorderStyle = FormBorderStyle.None;
            form.MaximizeBox = false;
            form.MinimizeBox = false;
            form.Dock = DockStyle.Fill;
            MastPanel.Controls.Add(form);
            form.Show();
        }
        private void AddForm(GroupBox MastPanel, Form form)
        {
            if (MastPanel == null) return;
            if (MastPanel.Controls.Count > 0)
                MastPanel.Controls.Clear();
            if (form == null) return;
            form.TopLevel = false;
            form.FormBorderStyle = FormBorderStyle.None;
            form.MaximizeBox = false;
            form.MinimizeBox = false;
            form.Dock = DockStyle.Fill;
            MastPanel.Controls.Add(form);
            form.Show();
        }
        private void AddForm(Panel MastPanel, Form form)
        {
            if (MastPanel == null) return;
            if (MastPanel.Controls.Count > 0)
                MastPanel.Controls.Clear();
            if (form == null) return;
            form.TopLevel = false;
            form.FormBorderStyle = FormBorderStyle.None;
            form.MaximizeBox = false;
            form.MinimizeBox = false;
            form.Dock = DockStyle.Fill;
            MastPanel.Controls.Add(form);
            form.Show();
        }


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
        public void DisplayExcuteResult(object sender, ExcuteCompletedEventArgs e)  // 这里一定要加一个键值对变量来收集，以免出现重合
        {
            try
            {
                if (e.DataContent == null) return;// 在图形窗口只显示世界坐标元素，像素元素在图像窗口显示
                switch (e.DataContent.GetType().Name) //这里只接受XLD轮廓或3D对象轮廓
                {
                    case "ImageDataClass":
                        this.drawObject.BackImage = (ImageDataClass)e.DataContent;
                        this.CurrentImageData = this.drawObject.BackImage;
                        break;
                    case nameof(RegionDataClass):
                        if (this.listData.ContainsKey(e.ItemName))
                            this.listData[e.ItemName] = ((RegionDataClass)e.DataContent).Region;
                        else
                            this.listData.Add(e.ItemName, ((RegionDataClass)e.DataContent).Region);
                        break;
                    case nameof(XldDataClass):
                        if (this.listData.ContainsKey(e.ItemName))
                            this.listData[e.ItemName] = ((XldDataClass)e.DataContent).HXldCont;
                        else
                            this.listData.Add(e.ItemName, ((XldDataClass)e.DataContent).HXldCont);
                        break;
                    case "userWcsCircle":
                        this.drawObject.AttachPropertyData.Clear();
                        if (this.listData.ContainsKey(e.ItemName))
                            this.listData[e.ItemName] = e.DataContent;
                        else
                            this.listData.Add(e.ItemName, e.DataContent);
                        this.drawObject.AttachPropertyData.AddRange(this.listData.Values.ToArray()); //(userWcsCircle)e.DataContent
                        userWcsCircle wcsCircle = ((userWcsCircle)e.DataContent);
                        if (wcsCircle.EdgesPoint_xyz != null)
                        {
                            for (int i = 0; i < wcsCircle.EdgesPoint_xyz.Length; i++)
                            {
                                this.drawObject.AttachPropertyData.Add(new userWcsPoint(wcsCircle.EdgesPoint_xyz[i].X, wcsCircle.EdgesPoint_xyz[i].Y, 0, wcsCircle.CamParams));
                            }
                        }
                        this.listPixPoint.Add((new userWcsPoint(wcsCircle.X, wcsCircle.Y, wcsCircle.Z, wcsCircle.Grab_x, wcsCircle.Grab_y, wcsCircle.CamParams)).GetPixPoint());
                        ///////////////////////////////////////////////
                        string FunctionNoSource = CommunicationConfigParamManger.Instance.ReadValue(this.CamParam.CaliParam.CoordSysName, enCommunicationCommand.FunctionNo)?.ToString();
                        string FunctionNoTarget = "";
                        if (this.MapCamParam != null)
                            FunctionNoTarget = CommunicationConfigParamManger.Instance.ReadValue(this.MapCamParam.CaliParam.CoordSysName, enCommunicationCommand.FunctionNo)?.ToString();
                        switch (FunctionNoSource)
                        {
                            case "source":
                            case "Source":
                                this.listWcsSourcePoint.Add(new userWcsPoint(wcsCircle.X, wcsCircle.Y, wcsCircle.Z, wcsCircle.CamParams));
                                break;
                                //case "target":
                                //case "Target":
                                //    this.listWcsTargetPoint.Add(new userWcsPoint(wcsCircle.X, wcsCircle.Y, wcsCircle.Z, wcsCircle.CamParams));
                                //    break;
                        }
                        switch (FunctionNoTarget)
                        {
                            //case "source":
                            //case "Source":
                            //    this.listWcsSourcePoint.Add(new userWcsPoint(wcsCircle.X, wcsCircle.Y, wcsCircle.Z, wcsCircle.CamParams));
                            //    break;
                            case "target":
                            case "Target":
                                this.listWcsTargetPoint.Add(new userWcsPoint(wcsCircle.X, wcsCircle.Y, wcsCircle.Z, wcsCircle.CamParams));
                                break;
                        }
                        break;
                    case "userWcsCircleSector":
                        this.drawObject.AttachPropertyData.Clear();
                        if (this.listData.ContainsKey(e.ItemName))
                            this.listData[e.ItemName] = e.DataContent;
                        else
                            this.listData.Add(e.ItemName, e.DataContent);
                        this.drawObject.AttachPropertyData.AddRange(this.listData.Values.ToArray());
                        userWcsCircleSector wcsCircleSector = ((userWcsCircleSector)e.DataContent);
                        if (wcsCircleSector.EdgesPoint_xyz != null)
                        {
                            for (int i = 0; i < wcsCircleSector.EdgesPoint_xyz.Length; i++)
                            {
                                this.drawObject.AttachPropertyData.Add(new userWcsPoint(wcsCircleSector.EdgesPoint_xyz[i].X, wcsCircleSector.EdgesPoint_xyz[i].Y, 0, wcsCircleSector.CamParams));
                            }
                        }
                        ///////////////////////////////////////////////
                        FunctionNoSource = CommunicationConfigParamManger.Instance.ReadValue(this.CamParam.CaliParam.CoordSysName, enCommunicationCommand.FunctionNo)?.ToString();
                        FunctionNoTarget = "";
                        if (this.MapCamParam != null)
                            FunctionNoTarget = CommunicationConfigParamManger.Instance.ReadValue(this.MapCamParam.CaliParam.CoordSysName, enCommunicationCommand.FunctionNo)?.ToString();
                        switch (FunctionNoSource)
                        {
                            case "source":
                            case "Source":
                                this.listWcsSourcePoint.Add(new userWcsPoint(wcsCircleSector.X, wcsCircleSector.Y, wcsCircleSector.Z, wcsCircleSector.CamParams));
                                break;
                                //case "target":
                                //case "Target":
                                //    this.listWcsTargetPoint.Add(new userWcsPoint(wcsCircleSector.X, wcsCircleSector.Y, wcsCircleSector.Z, wcsCircleSector.CamParams));
                                //    break;
                        }
                        switch (FunctionNoTarget)
                        {
                            //case "source":
                            //case "Source":
                            //    this.listWcsSourcePoint.Add(new userWcsPoint(wcsCircleSector.X, wcsCircleSector.Y, wcsCircleSector.Z, wcsCircleSector.CamParams));
                            //    break;
                            case "target":
                            case "Target":
                                this.listWcsTargetPoint.Add(new userWcsPoint(wcsCircleSector.X, wcsCircleSector.Y, wcsCircleSector.Z, wcsCircleSector.CamParams));
                                break;
                        }
                        break;
                    case "userWcsEllipse":
                        this.drawObject.AttachPropertyData.Clear();
                        if (this.listData.ContainsKey(e.ItemName))
                            this.listData[e.ItemName] = e.DataContent;
                        else
                            this.listData.Add(e.ItemName, e.DataContent);
                        this.drawObject.AttachPropertyData.AddRange(this.listData.Values.ToArray());
                        userWcsEllipse wcsEllips1 = ((userWcsEllipse)e.DataContent);
                        if (wcsEllips1.EdgesPoint_xyz != null)
                        {
                            for (int i = 0; i < wcsEllips1.EdgesPoint_xyz.Length; i++)
                            {
                                this.drawObject.AttachPropertyData.Add(new userWcsPoint(wcsEllips1.EdgesPoint_xyz[i].X, wcsEllips1.EdgesPoint_xyz[i].Y, 0, wcsEllips1.CamParams));
                            }
                        }
                        ///////////////////////////////////////////////
                        FunctionNoSource = CommunicationConfigParamManger.Instance.ReadValue(this.CamParam.CaliParam.CoordSysName, enCommunicationCommand.FunctionNo)?.ToString();
                        FunctionNoTarget = "";
                        if (this.MapCamParam != null)
                            FunctionNoTarget = CommunicationConfigParamManger.Instance.ReadValue(this.MapCamParam.CaliParam.CoordSysName, enCommunicationCommand.FunctionNo)?.ToString();
                        switch (FunctionNoSource)
                        {
                            case "source":
                            case "Source":
                                this.listWcsSourcePoint.Add(new userWcsPoint(wcsEllips1.X, wcsEllips1.Y, wcsEllips1.Z, wcsEllips1.CamParams));
                                break;
                                //case "target":
                                //case "Target":
                                //    this.listWcsTargetPoint.Add(new userWcsPoint(wcsEllips1.X, wcsEllips1.Y, wcsEllips1.Z, wcsEllips1.CamParams));
                                //    break;
                        }
                        switch (FunctionNoTarget)
                        {
                            //case "source":
                            //case "Source":
                            //    this.listWcsSourcePoint.Add(new userWcsPoint(wcsEllips1.X, wcsEllips1.Y, wcsEllips1.Z, wcsEllips1.CamParams));
                            //    break;
                            case "target":
                            case "Target":
                                this.listWcsTargetPoint.Add(new userWcsPoint(wcsEllips1.X, wcsEllips1.Y, wcsEllips1.Z, wcsEllips1.CamParams));
                                break;
                        }
                        break;
                    case "userWcsEllipseSector":
                        this.drawObject.AttachPropertyData.Clear();
                        if (this.listData.ContainsKey(e.ItemName))
                            this.listData[e.ItemName] = e.DataContent;
                        else
                            this.listData.Add(e.ItemName, e.DataContent);
                        this.drawObject.AttachPropertyData.AddRange(this.listData.Values.ToArray());
                        userWcsEllipseSector wcsEllipseSector = ((userWcsEllipseSector)e.DataContent);
                        if (wcsEllipseSector.EdgesPoint_xyz != null)
                        {
                            for (int i = 0; i < wcsEllipseSector.EdgesPoint_xyz.Length; i++)
                            {
                                this.drawObject.AttachPropertyData.Add(new userWcsPoint(wcsEllipseSector.EdgesPoint_xyz[i].X, wcsEllipseSector.EdgesPoint_xyz[i].Y, 0, wcsEllipseSector.CamParams));
                            }
                        }
                        //////////////////////////////////////////
                        FunctionNoSource = CommunicationConfigParamManger.Instance.ReadValue(this.CamParam.CaliParam.CoordSysName, enCommunicationCommand.FunctionNo)?.ToString();
                        FunctionNoTarget = "";
                        if (this.MapCamParam != null)
                            FunctionNoTarget = CommunicationConfigParamManger.Instance.ReadValue(this.MapCamParam.CaliParam.CoordSysName, enCommunicationCommand.FunctionNo)?.ToString();
                        switch (FunctionNoSource)
                        {
                            case "source":
                            case "Source":
                                this.listWcsSourcePoint.Add(new userWcsPoint(wcsEllipseSector.X, wcsEllipseSector.Y, wcsEllipseSector.Z, wcsEllipseSector.CamParams));
                                break;
                                //case "target":
                                //case "Target":
                                //    this.listWcsTargetPoint.Add(new userWcsPoint(wcsEllipseSector.X, wcsEllipseSector.Y, wcsEllipseSector.Z, wcsEllipseSector.CamParams));
                                //    break;
                        }
                        switch (FunctionNoTarget)
                        {
                            //case "source":
                            //case "Source":
                            //    this.listWcsSourcePoint.Add(new userWcsPoint(wcsEllipseSector.X, wcsEllipseSector.Y, wcsEllipseSector.Z, wcsEllipseSector.CamParams));
                            //    break;
                            case "target":
                            case "Target":
                                this.listWcsTargetPoint.Add(new userWcsPoint(wcsEllipseSector.X, wcsEllipseSector.Y, wcsEllipseSector.Z, wcsEllipseSector.CamParams));
                                break;
                        }
                        break;


                    case "userWcsPoint":
                        this.drawObject.AttachPropertyData.Clear();
                        if (this.listData.ContainsKey(e.ItemName))
                            this.listData[e.ItemName] = e.DataContent;
                        else
                            this.listData.Add(e.ItemName, e.DataContent);
                        this.drawObject.AttachPropertyData.AddRange(this.listData.Values.ToArray()); // 点对象本身就是一个点，所以这里不再考虑显示子元素
                        ////////////////////////// 
                        FunctionNoSource = CommunicationConfigParamManger.Instance.ReadValue(this.CamParam.CaliParam.CoordSysName, enCommunicationCommand.FunctionNo)?.ToString();
                        FunctionNoTarget = "";
                        if (this.MapCamParam != null)
                            FunctionNoTarget = CommunicationConfigParamManger.Instance.ReadValue(this.MapCamParam.CaliParam.CoordSysName, enCommunicationCommand.FunctionNo)?.ToString();
                        switch (FunctionNoSource)
                        {
                            case "source":
                            case "Source":
                                this.listWcsSourcePoint.Add((userWcsPoint)e.DataContent);
                                break;
                                //case "target":
                                //case "Target":
                                //    this.listWcsTargetPoint.Add((userWcsPoint)e.DataContent);
                                //    break;
                        }
                        switch (FunctionNoTarget)
                        {
                            //case "source":
                            //case "Source":
                            //    this.listWcsSourcePoint.Add((userWcsPoint)e.DataContent);
                            //    break;
                            case "target":
                            case "Target":
                                this.listWcsTargetPoint.Add((userWcsPoint)e.DataContent);
                                break;
                        }
                        break;

                    case "userWcsRectangle2":
                        this.drawObject.AttachPropertyData.Clear();
                        if (this.listData.ContainsKey(e.ItemName))
                            this.listData[e.ItemName] = e.DataContent;
                        else
                            this.listData.Add(e.ItemName, e.DataContent);
                        this.drawObject.AttachPropertyData.AddRange(this.listData.Values.ToArray());
                        userWcsRectangle2 wcsRect2 = ((userWcsRectangle2)e.DataContent);
                        if (wcsRect2.EdgesPoint_xyz != null)
                        {
                            for (int i = 0; i < wcsRect2.EdgesPoint_xyz.Length; i++)
                            {
                                this.drawObject.AttachPropertyData.Add(new userWcsPoint(wcsRect2.EdgesPoint_xyz[i].X, wcsRect2.EdgesPoint_xyz[i].Y, 0, wcsRect2.CamParams));
                            }
                        }
                        //////////////////////////////////////////
                        FunctionNoSource = CommunicationConfigParamManger.Instance.ReadValue(this.CamParam.CaliParam.CoordSysName, enCommunicationCommand.FunctionNo)?.ToString();
                        FunctionNoTarget = "";
                        if (this.MapCamParam != null)
                            FunctionNoTarget = CommunicationConfigParamManger.Instance.ReadValue(this.MapCamParam.CaliParam.CoordSysName, enCommunicationCommand.FunctionNo)?.ToString();
                        switch (FunctionNoSource)
                        {
                            case "source":
                            case "Source":
                                this.listWcsSourcePoint.Add(new userWcsPoint(wcsRect2.X, wcsRect2.Y, wcsRect2.Z, wcsRect2.CamParams));
                                break;
                                //case "target":
                                //case "Target":
                                //    this.listWcsTargetPoint.Add(new userWcsPoint(wcsRect2.X, wcsRect2.Y, wcsRect2.Z, wcsRect2.CamParams));
                                //    break;
                        }
                        switch (FunctionNoTarget)
                        {
                            //case "source":
                            //case "Source":
                            //    this.listWcsSourcePoint.Add(new userWcsPoint(wcsRect2.X, wcsRect2.Y, wcsRect2.Z, wcsRect2.CamParams));
                            //    break;
                            case "target":
                            case "Target":
                                this.listWcsTargetPoint.Add(new userWcsPoint(wcsRect2.X, wcsRect2.Y, wcsRect2.Z, wcsRect2.CamParams));
                                break;
                        }
                        break;

                    case "userWcsCoordSystem":
                        this.drawObject.AttachPropertyData.Clear();
                        if (this.listData.ContainsKey(e.ItemName))
                            this.listData[e.ItemName] = e.DataContent;
                        else
                            this.listData.Add(e.ItemName, e.DataContent);
                        this.drawObject.AttachPropertyData.AddRange(this.listData.Values.ToArray());
                        //this.drawObject.DetachDrawingObjectFromWindow();
                        break;

                }
                this.drawObject.AttachPropertyData.Clear();
                foreach (KeyValuePair<string, object> item in this.listData)
                {
                    this.drawObject.AttachPropertyData.Add(item.Value);
                }
                this.drawObject.DetachDrawingObjectFromWindow();
                ///////////////////////////////
            }
            catch (Exception ex)
            {
                new Common.UserMessageForm(ex.ToString()).ShowDialog();
            }
        }

        public void DisplayClickObject(object sender, TreeNodeMouseClickEventArgs e)  //
        {
            //if (!IsSelect()) return; // 如果不是当前选择的，则返回
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

                    default:

                        break;
                }
            }
            catch (Exception ex)
            {
                new Common.UserMessageForm(ex.ToString()).ShowDialog();
            }
        }

        public void DisplayClickItem(object sender, TreeNodeMouseClickEventArgs e)  // 这里一定要加一个键值对变量来收集，以免出现重合
        {
            try
            {
                if (e.Node.Tag == null) return;
                if (e.Node.Name == null) return;// 在图形窗口只显示世界坐标元素，像素元素在图像窗口显示
                this.drawObject.AttachPropertyData.Clear();
                switch (e.Node.Tag.GetType().Name)
                {
                    case "CircleMeasure":
                    case "CircleSectorMeasure":
                    case "EllipseMeasure":
                    case "EllipseSectorMeasure":
                    case "LineMeasure":
                    case "PointMeasure":
                    case "Rectangle2Measure":
                    case "WidthMeasure":
                        this.drawObject.AttachPropertyData.Clear(); // 清空附加属性
                        this.drawObject.IsDispalyAttachEdgesProperty = true;
                        // 添加需要显示的元素
                        foreach (var items in this.listData.Keys)
                        {
                            if (items.Split('-')[0] == e.Node.Name) continue; // + "-" + "0"
                            this.drawObject.AttachPropertyData.Add(this.listData[items]);
                        }
                        this.drawObject.DrawingGraphicObject();
                        break;
                }
            }
            catch (Exception he)
            {

            }
        }
        #endregion
        private void 保存相机内参button_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.CamParam.Save())
                    new UserMessageForm().ShowDialog("保存成功!");
                else
                    new UserMessageForm().ShowDialog("保存失败!");
            }
            catch (Exception ex)
            {
                new Common.UserMessageForm(ex.ToString()).ShowDialog();
            }
        }

        public void 标定button_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.listPixPoint.Count < 4)
                    new UserMessageForm().ShowDialog("不能少于4点");
                HTuple rows, cols;
                HTuple x, y, z;
                CalibrateMethod.Instance.PixPointToHtuple(this.listPixPoint, out rows, out cols);
                CalibrateMethod.Instance.WcsPointToHtuple(this.listWcsPoint, this.CamParam.CaliParam.CalibPlane, out x, out y, out z);
                x = x - (x.TupleMax() + x.TupleMin()) * 0.5;
                y = y - (y.TupleMax() + y.TupleMin()) * 0.5;
                z = z - (z.TupleMax() + z.TupleMin()) * 0.5;

                this.平均误差textBox.Text = error.ToString();
                if (new UserMessageForm().ShowDialog("是否更新并保存参数？", "标定成功:" + this.CamParam.HomMat2D.ToString() + " 最大误 = " + error.ToString()) == DialogResult.OK)
                {
                    this.CamParam?.Save();
                }
                /// 写到表格中
                this.映射参数dataGridView.Rows.Clear();
                for (int i = 0; i < 3; i++)
                {
                    this.映射参数dataGridView.Columns.Add("Col" + i.ToString(), (i + 1).ToString());
                }
                this.映射参数dataGridView.Rows.Add(this.CamParam.HomMat2D.c00, this.CamParam.HomMat2D.c01, this.CamParam.HomMat2D.c02);
                this.映射参数dataGridView.Rows.Add(this.CamParam.HomMat2D.c10, this.CamParam.HomMat2D.c11, this.CamParam.HomMat2D.c12);
                this.映射参数dataGridView.Rows.Add(0, 0, 1);
                /////////////////////////////验证坐标
                HTuple Qx, Qy;
                GetWcsPoint(this.listPixPoint, this.CamParam, out Qx, out Qy);
                this.源点坐标dataGridView.Rows.Clear();
                this.源点坐标dataGridView.Columns.Clear();
                int rowCount = 3;// (int)this.行数numericUpDown.Value;
                int colCount = 3;// (int)this.列数numericUpDown.Value;
                for (int i = 0; i < colCount; i++)
                {
                    this.源点坐标dataGridView.Columns.Add("Col" + i.ToString(), (i + 1).ToString());
                }
                this.目标点dataGridView.Rows.Clear();
                this.目标点dataGridView.Columns.Clear();
                for (int i = 0; i < colCount; i++)
                {
                    this.目标点dataGridView.Columns.Add("Col" + i.ToString(), (i + 1).ToString());
                }
                /////////////////////////////////
                switch (this.CamParam.CaliParam.CalibPlane)
                {
                    case enCalibPlane.XY:
                        for (int i = 0; i < rowCount; i++)
                        {
                            for (int j = 0; j < colCount; j++)
                            {
                                this.源点坐标dataGridView.Rows.Add(Qx[i * colCount + j].D, Qy[i * colCount + j].D, 0);
                            }
                        }
                        for (int i = 0; i < rowCount; i++)
                        {
                            for (int j = 0; j < colCount; j++)
                            {
                                this.目标点dataGridView.Rows.Add(Qx[i * colCount + j].D + this.listWcsPoint[i * colCount + j].X, Qy[i * colCount + j].D + this.listWcsPoint[i * colCount + j].Y, this.listWcsPoint[i * colCount + j].Z);
                            }
                        }
                        break;
                    case enCalibPlane.XZ:
                        for (int i = 0; i < rowCount; i++)
                        {
                            for (int j = 0; j < colCount; j++)
                            {
                                this.源点坐标dataGridView.Rows.Add(Qx[i * colCount + j].D, 0, Qy[i * colCount + j].D);
                            }
                        }
                        for (int i = 0; i < rowCount; i++)
                        {
                            for (int j = 0; j < colCount; j++)
                            {
                                this.目标点dataGridView.Rows.Add(Qx[i * colCount + j].D + this.listWcsPoint[i * colCount + j].X, this.listWcsPoint[i * colCount + j].Y, Qy[i * colCount + j].D + this.listWcsPoint[i * colCount + j].Z);
                            }
                        }
                        break;
                    case enCalibPlane.YZ:
                        for (int i = 0; i < rowCount; i++)
                        {
                            for (int j = 0; j < colCount; j++)
                            {
                                this.源点坐标dataGridView.Rows.Add(0, Qx[i * colCount + j].D, Qy[i * colCount + j].D);
                            }
                        }
                        for (int i = 0; i < rowCount; i++)
                        {
                            for (int j = 0; j < colCount; j++)
                            {
                                this.目标点dataGridView.Rows.Add(this.listWcsPoint[i * colCount + j].X, Qx[i * colCount + j].D + this.listWcsPoint[i * colCount + j].Y, Qy[i * colCount + j].D + this.listWcsPoint[i * colCount + j].Z);
                            }
                        }
                        break;
                }
                /////////////////////////////////
                this.drawObject.AddViewObject(new ViewData(new userWcsCoordSystem(this.CamParam)));
                //this.drawObject.AddViewObject(new ViewData(new userWcsCoordSystem(this.CamParam)));
            }
            catch (Exception ex)
            {
                new Common.UserMessageForm(ex.ToString()).ShowDialog();
            }
        }

        public void ShowCoordsys(VisualizeView view)
        {
            try
            {
                view.AddViewObject(new ViewData(new userWcsCoordSystem(this.CamParam, this.Grab_x, this.Grab_y)));
            }
            catch
            {

            }
        }

        public void GetWcsPoint(BindingList<userPixPoint> pixPoint, CameraParam camParam, out HTuple Qx, out HTuple Qy)
        {
            Qx = new HTuple();
            Qy = new HTuple();
            if (pixPoint == null || pixPoint.Count == 0) return;
            if (camParam == null) return;
            double wcs_x, wcs_y, wcs_z;
            foreach (var item in pixPoint)
            {
                camParam.ImagePointsToWorldPlane(item.Row, item.Col, 0, 0, 0, out wcs_x, out wcs_y, out wcs_z);
                Qx.Append(wcs_x);
                Qy.Append(wcs_y);
            }
        }

        public void PaintCoordSys(Panel panel)
        {
            Graphics g = panel.CreateGraphics();

            g.DrawLine(new Pen(System.Drawing.Color.Red), 50, panel.Height - 20, 300, panel.Height - 20);
            g.DrawLine(new Pen(System.Drawing.Color.Red), 280, 210, 300, 220);
            g.DrawLine(new Pen(System.Drawing.Color.Red), 280, 230, 300, 220);

            g.DrawLine(new Pen(System.Drawing.Color.Red), 70, 240, 70, 50);

            g.DrawLine(new Pen(System.Drawing.Color.Red), 60, 70, 70, 50);
            g.DrawLine(new Pen(System.Drawing.Color.Red), 80, 70, 70, 50);
            g.DrawArc(new Pen(System.Drawing.Color.Red), 20, 170, 100, 100, 0, -90);

            g.DrawLine(new Pen(System.Drawing.Color.Red), 70 + 35, 220 - 35, 70 + 35, 220 - 35 + 10);
            g.DrawLine(new Pen(System.Drawing.Color.Red), 70 + 35, 220 - 35, 70 + 35 + 10, 220 - 35);
            g.DrawString("X轴", this.Font, new SolidBrush(Color.Black), 150, 230);
            g.DrawString("Y轴", this.Font, new SolidBrush(Color.Black), 40, 130);

            g.DrawString("说明：坐标系为笛卡尔坐标系，X向右为正，Y向上为正，角度逆时针为正。",
                this.Font, new SolidBrush(Color.Red), 60, 260);
        }

        private void Read(out double X, out double Y, out double Z, out double Theta)
        {
            X = 0; Y = 0; Z = 0; Theta = 0;
            // string _cardName = CoordSysConfigParamManger.Instance.GetCardName(nameof(this.MyCaliPara.CoordSysName));
            MotionCardManage.GetCard(this.CamParam.CaliParam.CoordSysName).GetAxisPosition(this.CamParam.CaliParam.CoordSysName, enAxisName.X轴, out X);
            MotionCardManage.GetCard(this.CamParam.CaliParam.CoordSysName).GetAxisPosition(this.CamParam.CaliParam.CoordSysName, enAxisName.Y轴, out Y);
            MotionCardManage.GetCard(this.CamParam.CaliParam.CoordSysName).GetAxisPosition(this.CamParam.CaliParam.CoordSysName, enAxisName.Z轴, out Z);
            MotionCardManage.GetCard(this.CamParam.CaliParam.CoordSysName).GetAxisPosition(this.CamParam.CaliParam.CoordSysName, enAxisName.Theta轴, out Theta);
        }
        private void Write(double X, double Y, double Z, double Theta)
        {
            MotionCardManage.GetCard(this.CamParam.CaliParam.CoordSysName).MoveMultyAxis(this.CamParam.CaliParam.CoordSysName, enAxisName.XYZTheta轴, 0, new CoordSysAxisPosParam(X, Y, Z, Theta, 0, 0));
        }

        private void 获取起始点button_Click(object sender, EventArgs e)
        {
            try
            {
                double X = 0, Y = 0, Z = 0, Theta = 0;
                Read(out X, out Y, out Z, out Theta);
                /////////////////////
                this.CamParam.CaliParam.StartCaliPoint = new userWcsVector(X + this.CamParam.CaliParam.Offset_X, Y + this.CamParam.CaliParam.Offset_Y, Z, Theta);
                //this.起始点textBox.Text = "X:" + this.CamParam.CaliParam.StartCaliPoint.X.ToString("f3") +
                //                          "   Y:" + this.CamParam.CaliParam.StartCaliPoint.Y.ToString("f3") +
                //                          "   Z:" + Z.ToString("f3") +
                //                          "   Theta:" + Theta.ToString("f3");
            }
            catch (Exception ex)
            {
                new Common.UserMessageForm(ex.ToString()).ShowDialog();
            }
        }

        private void 获取终止点button_Click(object sender, EventArgs e)
        {
            try
            {
                double X = 0, Y = 0, Z = 0, Theta = 0;
                Read(out X, out Y, out Z, out Theta);
                ///////////////////////
                this.CamParam.CaliParam.EndCalibPoint = new userWcsVector(X + this.CamParam.CaliParam.Offset_X * -1, Y + this.CamParam.CaliParam.Offset_Y * -1, Z, Theta);
                //this.终止点textBox.Text = "X:" + this.CamParam.CaliParam.EndCalibPoint.X.ToString("f3") +
                //                          "   Y:" + this.CamParam.CaliParam.EndCalibPoint.Y.ToString("f3") +
                //                          "   Z:" + Z.ToString("f3") +
                //                          "   Theta:" + Theta.ToString("f3");
            }
            catch (Exception ex)
            {
                new Common.UserMessageForm(ex.ToString()).ShowDialog();
            }
        }

        private void 保存参数button_Click(object sender, EventArgs e)
        {
            try
            {
                this.CamParam.Save();
                //this.calibCoordConfigParamRota.Save(programPath + "\\" + this.CamParam.SensorName + "\\" + "RotaCalibConfigParam.xml");
            }
            catch (Exception ex)
            {
                new Common.UserMessageForm(ex.ToString()).ShowDialog();
            }
        }

        public void 标定执行button_Click(object sender, EventArgs e)
        {
            //this.标定停止button.Enabled = true;
            //this.标定执行button.Enabled = false;
            this.Invoke(new Action(() => 
            {
                this.toolStripButton_Run.Enabled = false;
                this.toolStripButton_Stop.Enabled = true;
            }));
            switch (this.CamParam.CaliParam.MoveStage)
            {
                case enMoveStage.PLC:
                //this.CalibPlc();
                //break;
                case enMoveStage.Card:
                //this.CalibCard();
                //break;
                case enMoveStage.Socket:
                    this.CalibSocketNew();
                    break;
            }
        }

        private void CalibCard()
        {
            this.isStop = false;
            this.listPixPoint.Clear();
            this.listWcsPoint.Clear();
            //this.标定执行button.Enabled = false;
            int rowCount = 3;// (int)this.行数numericUpDown.Value;
            int colCount = 3;// (int)this.列数numericUpDown.Value;
            double X = 0, Y = 0, Z = 0, Theta = 0;
            try
            {
                CalibrateMethod.Instance.GetNPointCoord(this.CamParam.CaliParam.StartCaliPoint, this.CamParam.CaliParam.EndCalibPoint, rowCount, colCount, out listWcsPoint);
                foreach (var item in listWcsPoint)
                {
                    if (this.isStop) break; // 控制标定停止
                    ///////////////////////////////////////////////////////////////////////////////
                    MotionCardManage.GetCard(this.CamParam.CaliParam.CoordSysName)?.GetAxisPosition(this.CamParam.CaliParam.CoordSysName, enAxisName.X轴, out X);
                    MotionCardManage.GetCard(this.CamParam.CaliParam.CoordSysName)?.GetAxisPosition(this.CamParam.CaliParam.CoordSysName, enAxisName.Y轴, out Y);
                    MotionCardManage.GetCard(this.CamParam.CaliParam.CoordSysName)?.GetAxisPosition(this.CamParam.CaliParam.CoordSysName, enAxisName.Theta轴, out Theta);
                    if (this.CamParam.CaliParam.CoordValueType == enCoordValueType.绝对坐标)
                        MotionCardManage.GetCard(this.CamParam.CaliParam.CoordSysName)?.MoveMultyAxis(this.CamParam.CaliParam.CoordSysName,
                                                 enAxisName.XYTheta轴, 10, new CoordSysAxisPosParam(item.X, item.Y, item.Z, item.Theta, 0, 0));
                    else
                        MotionCardManage.GetCard(this.CamParam.CaliParam.CoordSysName)?.MoveMultyAxis(this.CamParam.CaliParam.CoordSysName,
                                                 enAxisName.XYTheta轴, 10, new CoordSysAxisPosParam(item.X - X, item.Y - Y, 0, item.Theta - Theta, 0, 0));
                    this._treeViewWrapClass.RunSyn(null, 1);
                }
                //////////////////////////////////////////////////////////////////
                //foreach (var item in this.listPixPoint)
                //{
                //    this.图像点dataGridView.Rows.Add(item.Row, item.Col);
                //}
                //foreach (var item in this.listWcsPoint)
                //{
                //    this.世界点dataGridView.Rows.Add(item.X, item.Y, item.Z, item.Theta);
                //}
                ///////  执行标定流程  ///////////////////////
                标定button_Click(null, null);
            }
            catch (Exception ex)
            {
                new Common.UserMessageForm(ex.ToString()).ShowDialog();
            }
            finally
            {
                // this.标定执行button.Enabled = true;
            }
        }
        private void CalibPlc()
        {
            TreeNode node;
            string info = "";
            this.isStop = false;
            this.listPixPoint.Clear();
            this.listWcsPoint.Clear();
            // this.标定执行button.Enabled = false;
            int rowCount = 3;// (int)this.行数numericUpDown.Value;
            int colCount = 3;// (int)this.列数numericUpDown.Value;
            double X = 0, Y = 0, Z = 0, Theta = 0;
            try
            {
                CommunicationConfigParamManger.Instance.WriteValue(this.CamParam.CaliParam.CoordSysName, enCommunicationCommand.TriggerFromPlc, 0); // 第一次进来清零
                while (true)
                {
                    if (this.isStop) return; // 控制标定停止
                    Application.DoEvents();
                    object value = CommunicationConfigParamManger.Instance.ReadValue(this.CamParam.CaliParam.CoordSysName, enCommunicationCommand.TriggerFromPlc);
                    if (value != null && value.ToString() == "1")
                    {
                        CommunicationConfigParamManger.Instance.WriteValue(this.CamParam.CaliParam.CoordSysName, enCommunicationCommand.TriggerFromPlc, 0);
                        this.Invoke(new Action(() =>
                        {
                            this.获取起始点button_Click(null, null); // 运控点击标定启用按钮时，表示在标定起始位置，这时开始标定
                            this.获取终止点button_Click(null, null);
                        }));
                        break;
                    }
                    Thread.Sleep(1000);
                    LoggerHelper.Info("等待PLC开始标定触发信号");
                }
                CalibrateMethod.Instance.GetNPointCoord(this.CamParam.CaliParam.StartCaliPoint, this.CamParam.CaliParam.EndCalibPoint, rowCount, colCount, out listWcsPoint);
                if (listWcsPoint.Count == 0)
                {
                    new UserMessageForm().ShowDialog("没有可供移动的坐标点!!!");
                    return;
                }
                foreach (var item in listWcsPoint)
                {
                    if (this.isStop) return; // 控制标定停止
                    Application.DoEvents();
                    ///////////////////////////////////////////////////////////////////////////////
                    switch (this.CamParam.CaliParam.CoordValueType)
                    {
                        default:
                        case enCoordValueType.绝对坐标:
                            MotionCardManage.GetCard(this.CamParam.CaliParam.CoordSysName).MoveMultyAxis(this.CamParam.CaliParam.CoordSysName,
                                                     enAxisName.Compensation_XYTheta轴, 10, new CoordSysAxisPosParam(item.X, item.Y, item.Z, item.Theta, 0, 0));
                            break;
                        case enCoordValueType.相对坐标:
                            MotionCardManage.GetCard(this.CamParam.CaliParam.CoordSysName).GetAxisPosition(this.CamParam.CaliParam.CoordSysName, enAxisName.X轴, out X);
                            MotionCardManage.GetCard(this.CamParam.CaliParam.CoordSysName).GetAxisPosition(this.CamParam.CaliParam.CoordSysName, enAxisName.Y轴, out Y);
                            MotionCardManage.GetCard(this.CamParam.CaliParam.CoordSysName).GetAxisPosition(this.CamParam.CaliParam.CoordSysName, enAxisName.Theta轴, out Theta);
                            MotionCardManage.GetCard(this.CamParam.CaliParam.CoordSysName).MoveMultyAxis(this.CamParam.CaliParam.CoordSysName,
                                                     enAxisName.Compensation_XYTheta轴, 10, new CoordSysAxisPosParam((item.X - X), (item.Y - Y), 0, (item.Theta - Theta), 0, 0)); // 写入的是补偿轴地址
                            break;
                    }
                    //////////////////////////////////////////////////////////写入数据后触发PLC//////////////////////////////////////////////////////////////////////////
                    Thread.Sleep(100);
                    LoggerHelper.Info("等待轴运动到指定位置:" + "X = " + item.X.ToString("f3") + "  Y = " + item.Y.ToString("f3") + "  Theta = " + item.Theta.ToString("f3"));
                    CommunicationConfigParamManger.Instance.WriteValue(this.CamParam.CaliParam.CoordSysName, enCommunicationCommand.ResultToPlc, 3);// 写入3 表示继续
                    CommunicationConfigParamManger.Instance.WriteValue(this.CamParam.CaliParam.CoordSysName, enCommunicationCommand.FunctionNoToPlc, 5);
                    CommunicationConfigParamManger.Instance.WriteValue(this.CamParam.CaliParam.CoordSysName, enCommunicationCommand.TriggerToPlc, 1);
                    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    LoggerHelper.Info("等待轴运动到指定位置:" + "X = " + (item.X).ToString() + "  Y = " + (item.Y).ToString() + "  Z = " + (item.Z).ToString() + "  Theta = " + (item.Theta).ToString());
                    Thread.Sleep(100);
                    // 执行一次定位
                    // 等待PLC触发采图
                    while (true)
                    {
                        if (this.isStop) return; // 控制标定停止
                        Application.DoEvents();
                        object value = CommunicationConfigParamManger.Instance.ReadValue(this.CamParam.CaliParam.CoordSysName, enCommunicationCommand.TriggerFromPlc);
                        if (value != null && value.ToString() == "1")
                        {
                            MotionCardManage.GetCard(this.CamParam.CaliParam.CoordSysName).GetAxisPosition(this.CamParam.CaliParam.CoordSysName, enAxisName.X轴, out X);
                            MotionCardManage.GetCard(this.CamParam.CaliParam.CoordSysName).GetAxisPosition(this.CamParam.CaliParam.CoordSysName, enAxisName.Y轴, out Y);
                            MotionCardManage.GetCard(this.CamParam.CaliParam.CoordSysName).GetAxisPosition(this.CamParam.CaliParam.CoordSysName, enAxisName.Theta轴, out Theta);
                            if (Math.Abs(item.X - X) > 0.05 || Math.Abs(item.Y - Y) > 0.05 || Math.Abs(item.Theta - Theta) > 0.1)
                            {
                                new UserMessageForm().ShowDialog("PLC 没有运动到指定位置，请确认是否到限位或轴是否有移动!");
                                CommunicationConfigParamManger.Instance.WriteValue(this.CamParam.CaliParam.CoordSysName, enCommunicationCommand.ResultToPlc, 2);
                                CommunicationConfigParamManger.Instance.WriteValue(this.CamParam.CaliParam.CoordSysName, enCommunicationCommand.FunctionNoToPlc, 5);
                                CommunicationConfigParamManger.Instance.WriteValue(this.CamParam.CaliParam.CoordSysName, enCommunicationCommand.TriggerToPlc, 1);
                                return;
                            }
                            else
                                break;
                        }
                        Thread.Sleep(1000);
                        LoggerHelper.Info("等待PLC触发信号!!!");
                    }
                    LoggerHelper.Info("接收PLC触发信号!!!" + info);
                    node = this.GetExecuteNode(this.CamParam.CaliParam.CoordSysName, out info);
                    if (node != null)
                        ((IFunction)node.Tag)?.Execute(node);
                    else
                    {
                        this._treeViewWrapClass.RunSyn(null, 1);
                    }
                    CommunicationConfigParamManger.Instance.WriteValue(this.CamParam.CaliParam.CoordSysName, enCommunicationCommand.TriggerFromPlc, 0);
                }
                CommunicationConfigParamManger.Instance.WriteValue(this.CamParam.CaliParam.CoordSysName, enCommunicationCommand.ResultToPlc, 1);// 写入1 表示OK
                CommunicationConfigParamManger.Instance.WriteValue(this.CamParam.CaliParam.CoordSysName, enCommunicationCommand.FunctionNoToPlc, 5);
                CommunicationConfigParamManger.Instance.WriteValue(this.CamParam.CaliParam.CoordSysName, enCommunicationCommand.TriggerToPlc, 1);
                //////////////////////////////////////////////////////////////////
                //this.图像点dataGridView.Rows.Clear();
                //this.世界点dataGridView.Rows.Clear();
                //foreach (var item in this.listPixPoint)
                //{
                //    this.图像点dataGridView.Rows.Add(item.Row, item.Col);
                //}
                //foreach (var item in this.listWcsPoint)
                //{
                //    this.世界点dataGridView.Rows.Add(item.X, item.Y, item.Z, item.Theta);
                //}
                ///////  执行标定流程  ///////////////////////
                标定button_Click(null, null);
            }
            catch (Exception ex)
            {
                CommunicationConfigParamManger.Instance.WriteValue(this.CamParam.CaliParam.CoordSysName, enCommunicationCommand.ResultToPlc, 2);// 写入1 表示OK
                CommunicationConfigParamManger.Instance.WriteValue(this.CamParam.CaliParam.CoordSysName, enCommunicationCommand.FunctionNoToPlc, 5);
                CommunicationConfigParamManger.Instance.WriteValue(this.CamParam.CaliParam.CoordSysName, enCommunicationCommand.TriggerToPlc, 1);
                new Common.UserMessageForm(ex.ToString()).ShowDialog();
            }
            finally
            {
                //this.标定执行button.Enabled = true;
            }
        }

        private void CalibSocketNew()
        {
            TreeNode node;
            string info = "";
            double X, Y;
            this.isStop = false;
            this.listPixPoint?.Clear();
            this.listWcsPoint?.Clear();
            this.listGrabPoint?.Clear();
            this.listWcsSourcePoint?.Clear();
            this.listWcsTargetPoint?.Clear();
            try
            {
                if (AcqSourceManage.Instance.GetAcqSource(this.CamParam.CaliParam.CamName)?.Sensor.CameraParam.AcqMode != enAcqMode.同步采集)
                    AcqSourceManage.Instance.GetAcqSource(this.CamParam.CaliParam.CamName)?.Sensor?.SetParam("实时采集", 0);
                if (AcqSourceManage.Instance.GetAcqSource(this.CamParam.CaliParam.MapCamName)?.Sensor.CameraParam.AcqMode != enAcqMode.同步采集)
                    AcqSourceManage.Instance.GetAcqSource(this.CamParam.CaliParam.MapCamName)?.Sensor?.SetParam("实时采集", 0);
                CommunicationConfigParamManger.Instance.WriteValue(this.CamParam.CaliParam.CoordSysName, enCommunicationCommand.TriggerFromPlc, 0); // 第一次进来先清零
                if (this.MapCamParam != null)
                    CommunicationConfigParamManger.Instance.WriteValue(this.MapCamParam.CaliParam.CoordSysName, enCommunicationCommand.TriggerFromPlc, 0); // 第一次进来先清零
                this.CamParam.MapType = "NONE"; // 标定时需原始数据
                if (this.CamParam.CaliParam.MapCamName != "NONE")
                {
                    while (true)
                    {
                        if (this.isStop) return;
                        //Application.DoEvents();
                        if (this.listWcsSourcePoint.Count != 0 && this.listWcsTargetPoint.Count != 0 && this.listWcsSourcePoint.Count == this.listWcsTargetPoint.Count) break;
                        /////////////////////////  等待触发信号  //////////////////////////////////////////
                        object sourceValue = CommunicationConfigParamManger.Instance.ReadValue(this.CamParam.CaliParam.CoordSysName, enCommunicationCommand.TriggerFromPlc);
                        object targetValue = CommunicationConfigParamManger.Instance.ReadValue(this.MapCamParam.CaliParam.CoordSysName, enCommunicationCommand.TriggerFromPlc);
                        //////////////////// 源相机 ///////////////////////////////////
                        if (sourceValue != null && sourceValue.ToString() == "1")
                        {
                            LoggerHelper.Info("接收PLC触发信号!!!" + info, this.CamParam.SensorName);
                            CommunicationConfigParamManger.Instance.WriteValue(this.CamParam.CaliParam.CoordSysName, enCommunicationCommand.TriggerFromPlc, 0);
                            CommunicationConfigParamManger.Instance.WriteValue(this.MapCamParam.CaliParam.CoordSysName, enCommunicationCommand.FunctionNo, "calib"); // 映射相机
                            List<RfidCommand> listValue = CommunicationConfigParamManger.Instance.ReadValue(this.CamParam.CaliParam.CoordSysName, enCommunicationCommand.RFIDInfo) as List<RfidCommand>;
                            MotionCardManage.GetCard(this.CamParam.CaliParam.CoordSysName).GetAxisPosition(this.CamParam.CaliParam.CoordSysName, enAxisName.X轴, out X);
                            MotionCardManage.GetCard(this.CamParam.CaliParam.CoordSysName).GetAxisPosition(this.CamParam.CaliParam.CoordSysName, enAxisName.Y轴, out Y);
                            // 读取目标对象坐标
                            if (listValue != null)
                            {
                                if (listValue.Count > 0 && this.listWcsTargetPoint.Count == 0)
                                {
                                    foreach (var item in listValue)
                                        this.listWcsTargetPoint.Add(new userWcsPoint(item.X, item.Y, item.Z));
                                }
                            }
                            ////////////////////////
                            node = this.GetExecuteNode(this.CamParam.CaliParam.CoordSysName, out info);
                            if (node != null)
                                ((IFunction)node.Tag)?.Execute(node);
                            else
                            {
                                this._treeViewWrapClass.RunSyn(null, 1);
                            }
                            /////////////////////////////////////////////
                            CommunicationConfigParamManger.Instance.WriteValue(this.CamParam.CaliParam.CoordSysName, enCommunicationCommand.ResultToPlc, "OK");
                            CommunicationConfigParamManger.Instance.WriteValue(this.CamParam.CaliParam.CoordSysName, enCommunicationCommand.TriggerToPlc, "");
                        }
                        /////////// 目标相机 //////////////////////////
                        if (targetValue != null && targetValue.ToString() == "1")
                        {
                            LoggerHelper.Info("接收PLC触发信号!" + info, this.CamParam.SensorName);
                            CommunicationConfigParamManger.Instance.WriteValue(this.MapCamParam.CaliParam.CoordSysName, enCommunicationCommand.TriggerFromPlc, 0);
                            CommunicationConfigParamManger.Instance.WriteValue(this.CamParam.CaliParam.CoordSysName, enCommunicationCommand.FunctionNo, "calib");
                            List<RfidCommand> listValue = CommunicationConfigParamManger.Instance.ReadValue(this.MapCamParam.CaliParam.CoordSysName, enCommunicationCommand.RFIDInfo) as List<RfidCommand>;
                            MotionCardManage.GetCard(this.MapCamParam.CaliParam.CoordSysName).GetAxisPosition(this.MapCamParam.CaliParam.CoordSysName, enAxisName.X轴, out X);
                            MotionCardManage.GetCard(this.MapCamParam.CaliParam.CoordSysName).GetAxisPosition(this.MapCamParam.CaliParam.CoordSysName, enAxisName.Y轴, out Y);
                            // 读取目标对象坐标
                            if (listValue != null)
                            {
                                if (listValue.Count > 0 && this.listWcsTargetPoint.Count == 0)
                                {
                                    foreach (var item in listValue)
                                        this.listWcsTargetPoint.Add(new userWcsPoint(item.X, item.Y, item.Z));
                                }
                            }
                            ////////////////////////
                            node = this.GetExecuteNode(this.MapCamParam.CaliParam.CoordSysName, out info);
                            if (node != null)
                                ((IFunction)node.Tag)?.Execute(node);
                            else
                            {
                                this._treeViewWrapClass.RunSyn(null, 1);
                            }
                            /////////////////////////////////////////////
                            CommunicationConfigParamManger.Instance.WriteValue(this.MapCamParam.CaliParam.CoordSysName, enCommunicationCommand.ResultToPlc, "OK");
                            CommunicationConfigParamManger.Instance.WriteValue(this.MapCamParam.CaliParam.CoordSysName, enCommunicationCommand.TriggerToPlc, "");
                        }

                        Thread.Sleep(100);
                        LoggerHelper.Info("等待PLC触发信号!!!" + info, this.CamParam.SensorName);
                    }
                }
                else // 如果相机名称为 NONE，则目标对象为非相机
                {
                    while (true)
                    {
                        if (this.isStop) return;
                        //Application.DoEvents();
                        if (this.listWcsSourcePoint.Count != 0 && this.listWcsTargetPoint.Count != 0 && this.listWcsSourcePoint.Count == this.listWcsTargetPoint.Count) break;
                        /////////////////////////  等待触发信号  //////////////////////////////////////////
                        object sourceValue = CommunicationConfigParamManger.Instance.ReadValue(this.CamParam.CaliParam.CoordSysName, enCommunicationCommand.TriggerFromPlc);
                        ///////////////////////////////////////////////////////
                        if (sourceValue != null && sourceValue.ToString() == "1")
                        {
                            LoggerHelper.Info("接收PLC触发信号!!!" + info, this.CamParam.SensorName);
                            CommunicationConfigParamManger.Instance.WriteValue(this.CamParam.CaliParam.CoordSysName, enCommunicationCommand.TriggerFromPlc, 0);
                            List<RfidCommand> listValue = CommunicationConfigParamManger.Instance.ReadValue(this.CamParam.CaliParam.CoordSysName, enCommunicationCommand.RFIDInfo) as List<RfidCommand>;
                            MotionCardManage.GetCard(this.CamParam.CaliParam.CoordSysName).GetAxisPosition(this.CamParam.CaliParam.CoordSysName, enAxisName.X轴, out X);
                            MotionCardManage.GetCard(this.CamParam.CaliParam.CoordSysName).GetAxisPosition(this.CamParam.CaliParam.CoordSysName, enAxisName.Y轴, out Y);
                            // 读取目标对象坐标
                            if (listValue != null)
                            {
                                if (listValue.Count > 0 && this.listWcsTargetPoint.Count == 0)
                                {
                                    foreach (var item in listValue)
                                        this.listWcsTargetPoint.Add(new userWcsPoint(item.X, item.Y, item.Z));
                                }
                            }
                            ////////////////////////
                            node = this.GetExecuteNode(this.CamParam.CaliParam.CoordSysName, out info);
                            if (node != null)
                                ((IFunction)node.Tag)?.Execute(node);
                            else
                            {
                                this._treeViewWrapClass.RunSyn(null, 1);
                            }
                            /////////////////////////////////////////////
                            CommunicationConfigParamManger.Instance.WriteValue(this.CamParam.CaliParam.CoordSysName, enCommunicationCommand.ResultToPlc, "OK");
                            CommunicationConfigParamManger.Instance.WriteValue(this.CamParam.CaliParam.CoordSysName, enCommunicationCommand.TriggerToPlc, "");
                        }

                        Thread.Sleep(100);
                        LoggerHelper.Info("等待PLC触发信号!!!" + info, this.CamParam.SensorName);
                    }
                }
                //////////////////////////////////////////////////////////////////
                this.Invoke(new Action(() =>
                {
                    this.toolStripButton_Run.Enabled = true;
                    this.toolStripButton_Stop.Enabled = false;
                }));
                List<double> listSourceWcs_x = new List<double>();
                List<double> listSourceWcs_y = new List<double>();
                List<double> listSourceWcs_z = new List<double>();
                List<double> listTargetWcs_x = new List<double>();
                List<double> listTargetWcs_y = new List<double>();
                List<double> listTargetWcs_z = new List<double>();
                this.Invoke(new Action(() =>
                {
                    this.源点坐标dataGridView.Rows.Clear();
                    foreach (var item in this.listWcsSourcePoint)
                    {
                        int index = this.源点坐标dataGridView.Rows.Add(item.X, item.Y, item.Z);
                        this.源点坐标dataGridView.Rows[index].HeaderCell.Value = (index + 1).ToString();
                        listSourceWcs_x.Add(item.X);
                        listSourceWcs_y.Add(item.Y);
                        listSourceWcs_z.Add(item.Z);
                    }
                    this.目标点dataGridView.Rows.Clear();
                    foreach (var item in this.listWcsTargetPoint)
                    {
                        int index = this.目标点dataGridView.Rows.Add(item.X, item.Y, item.Z);
                        this.目标点dataGridView.Rows[index].HeaderCell.Value = (index + 1).ToString();
                        listTargetWcs_x.Add(item.X);
                        listTargetWcs_y.Add(item.Y);
                        listTargetWcs_z.Add(item.Z);
                    }
                }));
                ///////////////////////////////  世界坐标变换必需使用刚性变换  ///////////////////////////////
                HHomMat2D hHomMat2D = new HHomMat2D();
                switch (listSourceWcs_x.Count)
                {
                    case 0:
                        new UserMessageForm().ShowDialog("点数量为0,不能标定");
                        return;
                    case 1:
                        hHomMat2D.VectorAngleToRigid(listSourceWcs_x[0], listSourceWcs_y[0], 0, listTargetWcs_x[0], listTargetWcs_y[0], 0);
                        break;
                    default:
                        if (listSourceWcs_x.Max() - listSourceWcs_x.Min() > 1 && listSourceWcs_y.Max() - listSourceWcs_y.Min() > 1) // 如果点数量大于4且不共线，则使用仿射变换，否则使用刚性变换
                            hHomMat2D.VectorToHomMat2d(listSourceWcs_x.ToArray(), listSourceWcs_y.ToArray(), listTargetWcs_x.ToArray(), listTargetWcs_y.ToArray());
                        else
                            hHomMat2D.VectorToRigid(listSourceWcs_x.ToArray(), listSourceWcs_y.ToArray(), listTargetWcs_x.ToArray(), listTargetWcs_y.ToArray());
                        break;
                }
                this.CamParam.MapHomMat2D = new UserHomMat2D(hHomMat2D);
                //this.CamParam.MapType = this.映射方法comboBox.Text;
                if (this.CamParam.DicMapHomMat2D.ContainsKey(this.映射方法comboBox.Text))
                    this.CamParam.DicMapHomMat2D[this.映射方法comboBox.Text] = new UserHomMat2D(hHomMat2D);
                else
                    this.CamParam.DicMapHomMat2D.Add(this.映射方法comboBox.Text, new UserHomMat2D(hHomMat2D));
                this.CamParam.CaliParam.CoordOriginType = enCoordOriginType.映射变换; // WcsToWcs ： 一定要是这样
                HTuple Qx, Qy;
                Qx = hHomMat2D.AffineTransPoint2d(listSourceWcs_x.ToArray(), listSourceWcs_y.ToArray(), out Qy);
                HTuple dist = HMisc.DistancePp(Qx, Qy, listTargetWcs_x.ToArray(), listTargetWcs_y.ToArray());
                this.Invoke(new Action(() =>
                {
                    this.平均误差textBox.Text = dist.TupleMax().D.ToString();
                }));
                double sy, phi, theta, tx, ty;
                this.CamParam.DicMapHomMat2D[this.映射方法comboBox.Text].GetHHomMat().HomMat2dToAffinePar(out sy, out phi, out theta, out tx, out ty);
                double deg = phi * 180 / Math.PI;
                if (new UserMessageForm().ShowDialog("标定成功:" + this.CamParam.DicMapHomMat2D[this.映射方法comboBox.Text].ToString() + " 最大误 = " + this.平均误差textBox.Text + ", 轴夹角:" + Math.Round(deg, 3).ToString(), "是否更新并保存参数？") == DialogResult.OK)
                {
                    this.CamParam?.Save();
                }
                /////
                this.Invoke(new Action(() =>
                {
                    this.映射坐标dataGridView.Rows.Clear();
                    for (int i = 0; i < Qx.Length; i++)
                    {
                        int index = this.映射坐标dataGridView.Rows.Add(Qx[i].D, Qy[i].D);
                        this.映射坐标dataGridView.Rows[index].HeaderCell.Value = (index + 1).ToString();
                    }
                }));
                /////////////////////////////////
            }
            catch (Exception ex)
            {
                CommunicationConfigParamManger.Instance.WriteValue(this.CamParam.CaliParam.CoordSysName, enCommunicationCommand.ResultToPlc, "NG");
                CommunicationConfigParamManger.Instance.WriteValue(this.CamParam.CaliParam.CoordSysName, enCommunicationCommand.TriggerToPlc, "");
                new Common.UserMessageForm(ex.ToString()).ShowDialog();
            }
            finally
            {
                //this.CamParam.MapType = this.映射方法comboBox.Text;
                if (AcqSourceManage.Instance.GetAcqSource(this.CamParam.CaliParam.CamName)?.Sensor.CameraParam.AcqMode != enAcqMode.同步采集)
                    AcqSourceManage.Instance.GetAcqSource(this.CamParam.CaliParam.CamName)?.Sensor?.SetParam("停止采集", 0);
                if (AcqSourceManage.Instance.GetAcqSource(this.CamParam.CaliParam.MapCamName)?.Sensor.CameraParam.AcqMode != enAcqMode.同步采集)
                    AcqSourceManage.Instance.GetAcqSource(this.CamParam.CaliParam.MapCamName)?.Sensor?.SetParam("停止采集", 0);
                //this.标定执行button.Enabled = true;
            }
        }

        public void 标定停止button_Click(object sender, EventArgs e)
        {
            try
            {
                this.isStop = true;
                //this.标定执行button.Enabled = true;
                //this.标定停止button.Enabled = false;
                this.toolStripButton_Run.Enabled = true;
                this.toolStripButton_Stop.Enabled = false;
                this._treeViewWrapClass.Stop();
            }
            catch (Exception ex)
            {
                new Common.UserMessageForm(ex.ToString()).ShowDialog();
            }
        }

        private void 检测工具button_Click(object sender, EventArgs e)
        {
            try
            {
                ToolForm tool = new ToolForm(this._treeViewWrapClass);
                tool.Owner = this;
                tool.Show();
            }
            catch (Exception ex)
            {
                new Common.UserMessageForm(ex.ToString()).ShowDialog();
            }
        }

        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            switch (e.ClickedItem.Name)
            {
                case nameof(this.工具toolStripButton):
                case "检测工具":
                    ToolForm tool = new ToolForm(this._treeViewWrapClass);
                    tool.Owner = this;
                    tool.Show();
                    break;
                case "执行":
                case nameof(this.toolStripButton_Run):
                    Task.Run(() =>
                    {
                        标定执行button_Click(null, null);
                    });
                    break;
                case "停止":
                case nameof(this.toolStripButton_Stop):
                    标定停止button_Click(null, null);
                    break;
                case "打开":
                case nameof(this.打开toolStripButton):
                    this.programPath = new FileOperate().OpenFile(2);
                    if (this.programPath == null || this.programPath.Trim().Length == 0) return;
                    this._treeViewWrapClass.OpenProgram(this.programPath);
                    break;
                case "保存":
                case nameof(this.保存toolStripButton):
                    if (this.programPath == null || this.programPath.Length == 0)
                    {
                        this.programPath = new FileOperate().SaveFile(2);
                        if (this.programPath == null || this.programPath.Length == 0) return;
                        if (this._treeViewWrapClass.SaveProgram(this.programPath + this.CamParam.SensorName))  //ProgramItemsSource.getInstance().GetTreeViewNode()
                            new Common.UserMessageForm("保存成功").ShowDialog();
                        else
                            new UserMessageForm().ShowDialog("保存失败" + new Exception().ToString());
                    }
                    else
                    {
                        if (this._treeViewWrapClass.SaveProgram(this.programPath)) //+ this.CamParam.SensorName
                            new Common.UserMessageForm("保存成功").ShowDialog();
                        else
                            new UserMessageForm().ShowDialog("保存失败" + new Exception().ToString());
                    }
                    break;
            }
        }

        private void 坐标值类型comboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            try
            {
                enCoordValueType coordValueType = enCoordValueType.相对坐标;
                Enum.TryParse(this.坐标值类型comboBox.SelectedItem.ToString(), out coordValueType);
                this.CamParam.CaliParam.CoordValueType = coordValueType;
            }
            catch (Exception ex)
            {
                new Common.UserMessageForm(ex.ToString()).ShowDialog();
            }
        }

        private void 起始点textBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (!this.isLoad) return;
                //string[] name = 起始点textBox.Text.Split(new string[] { "X:", "Y:", "Z:", "Theta:" }, StringSplitOptions.RemoveEmptyEntries);
                //if (name.Length < 4) return;
                //double X, Y, Z, Theta;
                //bool result1 = double.TryParse(name[0].Trim(), out X);
                //bool result2 = double.TryParse(name[1].Trim(), out Y);
                //bool result3 = double.TryParse(name[2].Trim(), out Z);
                //bool result4 = double.TryParse(name[3].Trim(), out Theta);
                //if (result1 && result2 && result3 && result4)
                //    this.CamParam.CaliParam.StartCaliPoint = new userWcsVector(X, Y, Z, Theta);
                //else
                //    new UserMessageForm().ShowDialog("数据转换报错");
            }
            catch (Exception ex)
            {
                new Common.UserMessageForm(ex.ToString()).ShowDialog();
            }
        }

        private void 终止点textBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (!this.isLoad) return;
                //string[] name = 终止点textBox.Text.Split(new string[] { "X:", "Y:", "Z:", "Theta:" }, StringSplitOptions.RemoveEmptyEntries);
                //if (name.Length < 4) return;
                //double X, Y, Z, Theta;
                //bool result1 = double.TryParse(name[0].Trim(), out X);
                //bool result2 = double.TryParse(name[1].Trim(), out Y);
                //bool result3 = double.TryParse(name[2].Trim(), out Z);
                //bool result4 = double.TryParse(name[3].Trim(), out Theta);
                //if (result1 && result2 && result3 && result4)
                //    this.CamParam.CaliParam.EndCalibPoint = new userWcsVector(X, Y, Z, Theta);
                //else
                //    new UserMessageForm().ShowDialog("数据转换报错");
            }
            catch (Exception ex)
            {
                new Common.UserMessageForm(ex.ToString()).ShowDialog();
            }
        }

        private void 起始点textBox_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    //string[] content = this.起始点textBox.Text.Split(new string[] { "X:", "Y:", "Z:", "Theta:" }, StringSplitOptions.RemoveEmptyEntries);
                    //if (content.Length > 0)
                    //    this.CamParam.CaliParam.StartCaliPoint.X = Convert.ToDouble(content[0]);
                    //if (content.Length > 1)
                    //    this.CamParam.CaliParam.StartCaliPoint.Y = Convert.ToDouble(content[1]);
                    //if (content.Length > 2)
                    //    this.CamParam.CaliParam.StartCaliPoint.Z = Convert.ToDouble(content[2]);
                    //if (content.Length > 3)
                    //    this.CamParam.CaliParam.StartCaliPoint.Angle = Convert.ToDouble(content[3]);
                }
            }
            catch (Exception ex)
            {
                new Common.UserMessageForm(ex.ToString()).ShowDialog();
            }
        }

        private void 终止点textBox_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    //string[] content = this.终止点textBox.Text.Split(new string[] { "X:", "Y:", "Z:", "Theta:" }, StringSplitOptions.RemoveEmptyEntries);
                    //if (content.Length > 0)
                    //    this.CamParam.CaliParam.EndCalibPoint.X = Convert.ToDouble(content[0]);
                    //if (content.Length > 1)
                    //    this.CamParam.CaliParam.EndCalibPoint.Y = Convert.ToDouble(content[1]);
                    //if (content.Length > 2)
                    //    this.CamParam.CaliParam.EndCalibPoint.Z = Convert.ToDouble(content[2]);
                    //if (content.Length > 3)
                    //    this.CamParam.CaliParam.EndCalibPoint.Angle = Convert.ToDouble(content[3]);
                }
            }
            catch (Exception ex)
            {
                new Common.UserMessageForm(ex.ToString()).ShowDialog();
            }
        }

        private void 归一化原点checkBox_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.CamParam.CaliParam.CoordOriginType == enCoordOriginType.IsLoading) return; // 上料模式，原点不做归一化处理
                this.归一化原点checkBox.Checked = false;     //
                if (!归一化原点checkBox.Checked) return;
                HTuple Qx = new HTuple();
                HTuple Qy = new HTuple();
                int width, height;
                if (this.drawObject.BackImage == null || this.drawObject.BackImage.Image == null)
                {
                    new UserMessageForm().ShowDialog("请先采集图像");
                    return;
                }
                this.drawObject.BackImage.Image.GetImageSize(out width, out height);
                double wcs_x, wcs_y, wcs_z;
                this.CamParam.ImagePointsToWorldPlane(height * 0.5, width * 0.5, 0, 0, 0, out wcs_x, out wcs_y, out wcs_z);
                if (Math.Abs(wcs_x) > 0.0005)
                    this.CamParam.HomMat2D.c02 -= wcs_x;
                if (Math.Abs(wcs_y) > 0.0005)
                    this.CamParam.HomMat2D.c12 -= wcs_y;
                //////// 平移原点后需要改变拍照点的XY坐标，这样就相当于，原点在图像中心位置来进行标定
                //this.CamParam.CaliParam.StartCaliPoint.X += wcs_x;
                //this.CamParam.CaliParam.StartCaliPoint.Y += wcs_y;
                //this.CamParam.CaliParam.EndCalibPoint.X += wcs_x;
                //this.CamParam.CaliParam.EndCalibPoint.Y += wcs_y;
                ///////////////////////////
                this.CamParam.ImagePointsToWorldPlane(height * 0.5, width * 0.5, 0, 0, 0, out wcs_x, out wcs_y, out wcs_z);
                new UserMessageForm().ShowDialog("图像中点/视野中心坐标 ：" + wcs_x.ToString("f5") + "  " + wcs_y.ToString("f5"));

                this.drawObject.AddViewObject(new ViewData(new userWcsCoordSystem(this.CamParam)));
            }
            catch (Exception ex)
            {
                new Common.UserMessageForm(ex.ToString()).ShowDialog();
            }
        }

        private CancellationTokenSource cts1;
        private void 上相机实时采集checkBox_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                switch (this.实时采集checkBox.CheckState)
                {
                    case CheckState.Checked:
                        this.实时采集checkBox.BackColor = Color.Red;
                        AcqSource acqSource = AcqSourceManage.Instance.GetCamAcqSource(this.CamParam.SensorName);
                        if (acqSource == null) return;
                        cts1 = new CancellationTokenSource();
                        Dictionary<enDataItem, object> data;
                        Task.Run(() =>
                        {
                            this.drawObject.IsLiveState = true;
                            while (!cts1.IsCancellationRequested)
                            {
                                data = acqSource.AcqImageData(null);
                                switch (acqSource.Sensor?.ConfigParam.SensorType)
                                {
                                    case enUserSensorType.面阵相机:
                                        if (data?.Count > 0)
                                        {
                                            this.drawObject.BackImage = (ImageDataClass)data[enDataItem.Image];
                                            this.drawObject.AttachPropertyData.Clear();
                                            this.drawObject.AttachPropertyData.Add((this.GenCrossLine(this.drawObject.BackImage.Image)));
                                        }
                                        break;
                                }
                                Thread.Sleep(100);
                            }
                            this.drawObject.IsLiveState = false;
                        });
                        break;
                    default:
                        cts1?.Cancel();
                        this.实时采集checkBox.BackColor = Color.Lime;
                        break;
                }
            }
            catch (Exception ex)
            {
                new Common.UserMessageForm(ex.ToString()).ShowDialog();
            }
        }
        private void 实时采集checkBox_CheckedChanged_1(object sender, EventArgs e)
        {
            try
            {
                switch (this.实时采集checkBox.CheckState)
                {
                    case CheckState.Checked:
                        this.实时采集checkBox.BackColor = Color.Red;
                        this.cts = new CancellationTokenSource();
                        Stopwatch stopwatch = new Stopwatch();
                        Dictionary<enDataItem, object> data = null;
                        Task.Run(() =>
                        {
                            /// 打开光源
                            if (this._acqSource != null && this._acqSource.LightParamList != null)
                            {
                                foreach (var item in this._acqSource.LightParamList)
                                    item.Open();
                            }
                            this.drawObject.IsLiveState = true;
                            ///////////////////////
                            while (!this.cts.IsCancellationRequested)
                            {
                                /////////////////////////////////////////////////////////////
                                switch (this._acqSource?.Sensor?.ConfigParam.SensorType)
                                {
                                    case enUserSensorType.面阵相机:
                                        stopwatch.Restart();
                                        this._acqSource?.Sensor?.SetParam("实时采集", 0);
                                        data = this._acqSource?.Sensor?.ReadData();// this.acqSource.AcqImageData(null);
                                        if (data?.Count > 0)
                                        {
                                            this.drawObject.BackImage = (ImageDataClass)data[enDataItem.Image];
                                            this.drawObject.AttachPropertyData.Clear();
                                            this.drawObject.AttachPropertyData.Add((this.GenCrossLine(this.drawObject.BackImage?.Image)));
                                        }
                                        stopwatch.Stop();
                                        long time = stopwatch.ElapsedMilliseconds;
                                        break;
                                    case enUserSensorType.点激光:
                                        data = this._acqSource?.Sensor?.ReadData();// acqSource.AcqPointData();
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
                        this._acqSource?.Sensor?.SetParam("停止采集", 0);
                        /// 关闭光源
                        if (this._acqSource != null && this._acqSource.LightParamList != null)
                        {
                            foreach (var item in this._acqSource.LightParamList)
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



        public TreeNode GetExecuteNode(enCoordSysName coordSysName, out string Info)
        {
            TreeNode node = null;
            Info = "";
            bool IsOk = true;
            List<TreeNode> listTreeNode;
            listTreeNode = this._treeViewWrapClass.GetTreeViewNodeTag(); // 获取所有节点
            foreach (var item in listTreeNode)
            {
                if (item.Checked) continue; // 如果节点是禁用的，该属性为 true;
                IsOk = true;
                object oo = ((BaseFunction)item.Tag).ResultInfo;
                if (oo == null) continue;
                switch (oo.GetType().Name)
                {
                    case "BindingList`1":
                        switch (oo.GetType().GetGenericArguments()[0].Name)
                        {
                            case nameof(PlcCommunicateInfo):
                                BindingList<PlcCommunicateInfo> plcInfo = ((BaseFunction)item.Tag).ResultInfo as BindingList<PlcCommunicateInfo>;
                                foreach (var item2 in plcInfo)
                                {
                                    string value = CommunicationConfigParamManger.Instance.ReadValue(item2.CoordSysName, item2.CommunicationCommand)?.ToString();
                                    if (value == null)
                                    {
                                        IsOk = false;
                                        break;
                                    }
                                    item2.ReadValue = value;
                                    if (Info.Length == 0)
                                        Info = value;
                                    else
                                        Info += "," + value;
                                    ///////////////////////////////////////////////
                                    if (item2.IsCompare)
                                    {
                                        if (item2.TargetValue.Trim() == value.Trim() && item2.CoordSysName == coordSysName)
                                            IsOk = IsOk && true;
                                        else
                                            IsOk = IsOk && false;
                                    }
                                }
                                break;
                            default:
                                IsOk = false;
                                break;
                        }
                        break;
                    default:
                        IsOk = false;
                        break;

                }
                if (IsOk)
                {
                    node = item;
                    break;
                }
            }
            return node;
        }



    }
}

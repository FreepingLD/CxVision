using Sensor;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Common;
using FunctionBlock;
using MotionControlCard;
using View;
using System.Threading;
using HalconDotNet;

namespace FunctionBlock
{
    public partial class UpDnCamCalibSimpleForm : Form
    {
        private TreeViewWrapClass _treeViewWrapClass_Up, _treeViewWrapClass_Down;
        private string programPath = "VisionParam\\标定程序\\上下相机映射标定";
        private CameraParam CamParam_Map, CamParam_Target;
        private Dictionary<string, userWcsVector> listWcsPoint = new Dictionary<string, userWcsVector>();
        private Dictionary<string, userPixVector> listPixPoint = new Dictionary<string, userPixVector>();
        private DrawingBaseMeasure drawObject;
        private MetrolegyParamForm metrolegyParamForm;
        private IFunction _currFunction;
        private Dictionary<string, object> listData = new Dictionary<string, object>();
        private string selectPath1 = "";
        private string selectPath2 = "";
        private static object lockSynState = new object();
        private static UpDnCamCalibSimpleForm _Instance;

        public UpDnCamCalibSimpleForm()
        {
            InitializeComponent();
            _treeViewWrapClass_Up = new TreeViewWrapClass(this.上相机treeView, this);
            this._treeViewWrapClass_Up = new TreeViewWrapClass(this.上相机treeView, this);
            this._treeViewWrapClass_Down = new TreeViewWrapClass(this.下相机treeView, this);
            this.drawObject = new DrawingBaseMeasure(this.hWindowControl1, false);
            this.metrolegyParamForm = new MetrolegyParamForm(this.drawObject);
            this.CamParam_Map = null;
            this.CamParam_Target = null;
            ///////////////////////////////////////////////
            if (this.CamParam_Target != null)
                this._treeViewWrapClass_Up.OpenProgram(this.programPath + this.CamParam_Target?.SensorName); // 自动打开程序
            else
                this._treeViewWrapClass_Up.OpenProgram(this.programPath + "none"); // 自动打开程序
            if (this.CamParam_Map != null)
                this._treeViewWrapClass_Down.OpenProgram(this.programPath + this.CamParam_Map?.SensorName);
            else
                this._treeViewWrapClass_Down.OpenProgram(this.programPath + "none");
            this.下相机label.Text = this.CamParam_Map == null ? "none" : this.CamParam_Map.SensorName;
            this.上相机label.Text = this.CamParam_Target == null ? "none" : this.CamParam_Target.SensorName;
            BindProperty();
            this.addContextMenu(this.hWindowControl1);
            this.addDataGridViewContextMenu(this.目标相机坐标dataGridView);
            this.drawObject.GrayValueInfo += new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
            BaseFunction.ExcuteCompleted += new ExcuteCompletedEventHandler(DisplayExcuteResult);
            TreeViewWrapClass.ClickNode += new ClickNodeEventHandler(this.DisplayClickObject);
        }

        public UpDnCamCalibSimpleForm(CameraParam camParamMap, CameraParam camParamTarget)
        {
            InitializeComponent();
            this._treeViewWrapClass_Up = new TreeViewWrapClass(this.上相机treeView, this);
            this._treeViewWrapClass_Down = new TreeViewWrapClass(this.下相机treeView, this);
            this.drawObject = new DrawingBaseMeasure(this.hWindowControl1, false);
            this.metrolegyParamForm = new MetrolegyParamForm(this.drawObject);
            this.CamParam_Map = camParamMap;
            this.CamParam_Target = camParamTarget;
            ///////////////////////////////////////////////
            if (this.CamParam_Target != null)
                this._treeViewWrapClass_Up.OpenProgram(this.programPath + this.CamParam_Target?.SensorName); // 自动打开程序
            else
                this._treeViewWrapClass_Up.OpenProgram(this.programPath + "none"); // 自动打开程序
            if (this.CamParam_Map != null)
                this._treeViewWrapClass_Down.OpenProgram(this.programPath + this.CamParam_Map?.SensorName);
            else
                this._treeViewWrapClass_Down.OpenProgram(this.programPath + "none");
            this.下相机label.Text = this.CamParam_Map == null ? "none" : this.CamParam_Map.SensorName;
            this.上相机label.Text = this.CamParam_Target == null ? "none" : this.CamParam_Target.SensorName;
            BindProperty();
            this.addContextMenu(this.hWindowControl1);
            this.addDataGridViewContextMenu(this.目标相机坐标dataGridView);
            this.drawObject.GrayValueInfo += new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
            BaseFunction.ExcuteCompleted += new ExcuteCompletedEventHandler(DisplayExcuteResult);
            TreeViewWrapClass.ClickNode += new ClickNodeEventHandler(this.DisplayClickObject);
        }

        public static UpDnCamCalibSimpleForm Instance
        {
            get
            {
                if (_Instance == null)
                {
                    lock (lockSynState)
                    {
                        _Instance = new UpDnCamCalibSimpleForm();
                    }
                }
                return _Instance;
            }
        }
        private void UpDnCamCaliSimpleForm_Load(object sender, EventArgs e)
        {
            // this.映射方法comboBox.Text = this.CamParam_Map?.MapType; // 在窗体加载时来更新
            this.AutoForm();
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
        public void SetParam(CameraParam camParamMap, CameraParam camParamTarget)
        {
            this.CamParam_Map = camParamMap;
            this.CamParam_Target = camParamTarget;
        }

        private void BindProperty()
        {
            try
            {
                this.映射方法comboBox.DataBindings.Add(nameof(this.映射方法comboBox.Text), this.CamParam_Map.CaliParam, nameof(this.CamParam_Map.MapType), true, DataSourceUpdateMode.OnPropertyChanged);
                this.AddForm(this.元素tabPage, new ElementViewForm(false));
                this.LoadMapParam();
                /////////////////////////////////////
                this.标定位textBox.Text = $"X:{this.CamParam_Map.CaliParam.StartCaliPoint.X}   Y:{this.CamParam_Map.CaliParam.StartCaliPoint.Y}   Z:{this.CamParam_Map.CaliParam.StartCaliPoint.Z}   Theta:{this.CamParam_Map.CaliParam.StartCaliPoint.Angle}";

            }
            catch (Exception ex)
            {
                new UserMessageForm(ex.ToString()).ShowDialog();
            }
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
        private void 上相机toolStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            switch (e.ClickedItem.Text)
            {
                case "检测工具":
                    ToolForm tool = new ToolForm(this._treeViewWrapClass_Up, "");
                    tool.Owner = this;
                    tool.Show();
                    break;
                case "执行":
                    this.listWcsPoint?.Clear();
                    this.listPixPoint?.Clear();
                    this.listData?.Clear();
                    this.目标相机坐标dataGridView.Rows.Clear();
                    this._treeViewWrapClass_Up.RunSyn(this.toolStripButton_Run, 1);
                    ///////////////////////////////////////////////////////////
                    string[] method = this.映射方法comboBox.SelectedItem.ToString().Split(new string[] { "To" }, StringSplitOptions.RemoveEmptyEntries);
                    if (method != null && method.Length == 2)
                    {
                        switch (method[1])
                        {
                            default:
                            case "Wcs":
                                foreach (KeyValuePair<string, userWcsVector> item in this.listWcsPoint)
                                {
                                    int index = this.目标相机坐标dataGridView.Rows.Add(item.Value.X, item.Value.Y, item.Value.Angle, item.Value.Grab_x, item.Value.Grab_y, item.Value.Grab_z, item.Value.Grab_theta);
                                    this.目标相机坐标dataGridView.Rows[index].HeaderCell.Value = (index + 1).ToString();
                                }
                                break;
                            case "Pix":
                                foreach (KeyValuePair<string, userPixVector> item in this.listPixPoint)
                                {
                                    int index = this.目标相机坐标dataGridView.Rows.Add(item.Value.Row, item.Value.Col, item.Value.Rad, item.Value.Grab_x, item.Value.Grab_y, item.Value.Grab_z, item.Value.Grab_theta);
                                    this.目标相机坐标dataGridView.Rows[index].HeaderCell.Value = (index + 1).ToString();
                                }
                                break;
                        }
                    }
                    break;
                case "打开":
                    //string _programPath = new FileOperate().OpenFile(2);
                    //if (_programPath == null || _programPath.Trim().Length == 0) return;
                    FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
                    if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                    {
                        this.selectPath1 = folderBrowserDialog.SelectedPath;
                        this._treeViewWrapClass_Up.OpenProgram(folderBrowserDialog.SelectedPath);
                    }
                    break;
                case "保存":
                    string camName = "none";
                    if (this.selectPath1 != null && this.selectPath1.Length > 0)
                    {
                        if (this._treeViewWrapClass_Up.SaveProgram(this.selectPath1))
                            new Common.UserMessageForm("保存成功").ShowDialog();
                        else
                            new UserMessageForm().ShowDialog("保存失败" + new Exception().ToString());
                    }
                    else
                    {
                        if (this.programPath == null || this.programPath.Length == 0)
                        {
                            if (this.programPath == null || this.programPath.Length == 0) return;
                            if (this.CamParam_Target != null)
                                camName = this.CamParam_Target.SensorName;
                            if (this._treeViewWrapClass_Up.SaveProgram(this.programPath + this.CamParam_Target.SensorName))
                                new Common.UserMessageForm("保存成功").ShowDialog();
                            else
                                new UserMessageForm().ShowDialog("保存失败" + new Exception().ToString());
                        }
                        else
                        {
                            if (this.CamParam_Target != null)
                                camName = this.CamParam_Target.SensorName;
                            if (this._treeViewWrapClass_Up.SaveProgram(this.programPath + this.CamParam_Target.SensorName))
                                new Common.UserMessageForm("保存成功").ShowDialog();
                            else
                                new UserMessageForm().ShowDialog("保存失败" + new Exception().ToString());
                        }
                    }

                    break;
            }
        }

        private void 下相机toolStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            switch (e.ClickedItem.Text)
            {
                case "检测工具":
                    ToolForm tool = new ToolForm(this._treeViewWrapClass_Down, "");
                    tool.Owner = this;
                    tool.Show();
                    break;
                case "执行":
                    this.listWcsPoint?.Clear();
                    this.listPixPoint?.Clear();
                    this.listData?.Clear();
                    this.源相机坐标dataGridView.Rows.Clear();
                    this._treeViewWrapClass_Down.RunSyn(this.toolStripButton_Run, 1);
                    ///////////////////////////////////////////////////////////
                    string[] method = this.映射方法comboBox.SelectedItem.ToString().Split(new string[] { "To" }, StringSplitOptions.RemoveEmptyEntries);
                    if (method != null && method.Length == 2)
                    {
                        switch (method[0])
                        {
                            default:
                            case "Wcs":
                                foreach (KeyValuePair<string, userWcsVector> item in this.listWcsPoint)
                                {
                                    int index = this.源相机坐标dataGridView.Rows.Add(item.Value.X, item.Value.Y, item.Value.Angle, item.Value.Grab_x, item.Value.Grab_y, item.Value.Grab_z, item.Value.Grab_theta);
                                    this.源相机坐标dataGridView.Rows[index].HeaderCell.Value = (index + 1).ToString();
                                }
                                break;
                            case "Pix":
                                foreach (KeyValuePair<string, userPixVector> item in this.listPixPoint)
                                {
                                    int index = this.源相机坐标dataGridView.Rows.Add(item.Value.Row, item.Value.Col, item.Value.Rad, item.Value.Grab_x, item.Value.Grab_y, item.Value.Grab_z, item.Value.Grab_theta);
                                    this.源相机坐标dataGridView.Rows[index].HeaderCell.Value = (index + 1).ToString();
                                }
                                break;
                        }
                    }
                    break;
                case "打开":
                    //string _programPath = new FileOperate().OpenFile(2);
                    //if (_programPath == null || _programPath.Trim().Length == 0) return;
                    //this._treeViewWrapClass_Down.OpenProgram(_programPath);
                    FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
                    if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                    {
                        this.selectPath2 = folderBrowserDialog.SelectedPath;
                        this._treeViewWrapClass_Down.OpenProgram(folderBrowserDialog.SelectedPath);
                    }
                    break;
                case "保存":
                    string camName = "none";
                    if (this.selectPath2 != null && this.selectPath2.Length > 0)
                    {
                        if (this._treeViewWrapClass_Down.SaveProgram(this.selectPath2))
                            new Common.UserMessageForm("保存成功").ShowDialog();
                        else
                            new UserMessageForm().ShowDialog("保存失败" + new Exception().ToString());
                    }
                    else
                    {
                        if (this.programPath == null || this.programPath.Length == 0)
                        {
                            if (this.programPath == null || this.programPath.Length == 0) return;
                            if (this.CamParam_Map != null)
                                camName = this.CamParam_Map.SensorName;
                            if (this._treeViewWrapClass_Down.SaveProgram(this.programPath + camName))
                                new Common.UserMessageForm("保存成功").ShowDialog();
                            else
                                new UserMessageForm().ShowDialog("保存失败" + new Exception().ToString());
                        }
                        else
                        {
                            if (this.CamParam_Map != null)
                                camName = this.CamParam_Map.SensorName;
                            if (this._treeViewWrapClass_Down.SaveProgram(this.programPath + camName))
                                new Common.UserMessageForm("保存成功").ShowDialog();
                            else
                                new UserMessageForm().ShowDialog("保存失败" + new Exception().ToString());
                        }
                    }
                    break;
            }
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
            this.行坐标Label.Text = "Row:" + e.Row.ToString();
            this.列坐标Label.Text = "Col:" + e.Col.ToString();
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
                        break;

                    case nameof(userWcsCircle):
                        this.drawObject.AttachPropertyData.Clear();
                        if (this.listData.ContainsKey(e.ItemName))
                            this.listData[e.ItemName] = e.DataContent;
                        else
                            this.listData.Add(e.ItemName, e.DataContent);
                        this.drawObject.AttachPropertyData.AddRange(this.listData.Values.ToArray()); //(userWcsCircle)e.DataContent
                        userWcsCircle wcsCircle = ((userWcsCircle)e.DataContent);
                        userPixCircle pixCircle = wcsCircle.GetPixCircle();
                        if (wcsCircle.EdgesPoint_xyz != null)
                        {
                            for (int i = 0; i < wcsCircle.EdgesPoint_xyz.Length; i++)
                            {
                                this.drawObject.AttachPropertyData.Add(new userWcsPoint(wcsCircle.EdgesPoint_xyz[i].X, wcsCircle.EdgesPoint_xyz[i].Y, 0, wcsCircle.CamParams));
                            }
                        }
                        this.drawObject.DetachDrawingObjectFromWindow();
                        /////////////////////////////////
                        if (!this.listWcsPoint.ContainsKey(e.ItemName))
                            this.listWcsPoint.Add(e.ItemName, new userWcsVector(wcsCircle.X, wcsCircle.Y, wcsCircle.Z,0, wcsCircle.Grab_x, wcsCircle.Grab_y, wcsCircle.CamParams));
                        else
                            this.listWcsPoint[e.ItemName] = new userWcsVector(wcsCircle.X, wcsCircle.Y, wcsCircle.Z,0, wcsCircle.Grab_x, wcsCircle.Grab_y, wcsCircle.CamParams);
                        ///////////////////////////////////
                        if (!this.listPixPoint.ContainsKey(e.ItemName))
                            this.listPixPoint.Add(e.ItemName, new userPixVector(pixCircle.Row, pixCircle.Col,0, pixCircle.Grab_x, pixCircle.Grab_y, pixCircle.Grab_theta, pixCircle.CamParams));
                        else
                            this.listPixPoint[e.ItemName] = new userPixVector(pixCircle.Row, pixCircle.Col,0, pixCircle.Grab_x, pixCircle.Grab_y, pixCircle.Grab_theta, pixCircle.CamParams);
                        break;
                    //case nameof(userWcsCircleSector):
                    //    this.drawObject.AttachPropertyData.Clear();
                    //    if (this.listData.ContainsKey(e.ItemName))
                    //        this.listData[e.ItemName] = e.DataContent;
                    //    else
                    //        this.listData.Add(e.ItemName, e.DataContent);
                    //    this.drawObject.AttachPropertyData.AddRange(this.listData.Values.ToArray());
                    //    userWcsCircleSector wcsCircleSector = ((userWcsCircleSector)e.DataContent);
                    //    if (wcsCircleSector.EdgesPoint_xyz != null)
                    //    {
                    //        for (int i = 0; i < wcsCircleSector.EdgesPoint_xyz.Length; i++)
                    //        {
                    //            this.drawObject.AttachPropertyData.Add(new userWcsPoint(wcsCircleSector.EdgesPoint_xyz[i].X, wcsCircleSector.EdgesPoint_xyz[i].Y, 0, wcsCircleSector.CamParams));
                    //        }
                    //    }
                    //    this.drawObject.DetachDrawingObjectFromWindow();
                    //    break;
                    //case nameof(userWcsEllipse):
                    //    this.drawObject.AttachPropertyData.Clear();
                    //    if (this.listData.ContainsKey(e.ItemName))
                    //        this.listData[e.ItemName] = e.DataContent;
                    //    else
                    //        this.listData.Add(e.ItemName, e.DataContent);
                    //    this.drawObject.AttachPropertyData.AddRange(this.listData.Values.ToArray());
                    //    userWcsEllipse wcsEllips1 = ((userWcsEllipse)e.DataContent);
                    //    if (wcsEllips1.EdgesPoint_xyz != null)
                    //    {
                    //        for (int i = 0; i < wcsEllips1.EdgesPoint_xyz.Length; i++)
                    //        {
                    //            this.drawObject.AttachPropertyData.Add(new userWcsPoint(wcsEllips1.EdgesPoint_xyz[i].X, wcsEllips1.EdgesPoint_xyz[i].Y, 0, wcsEllips1.CamParams));
                    //        }
                    //    }
                    //    this.drawObject.DetachDrawingObjectFromWindow();
                    //    break;
                    //case nameof(userWcsEllipseSector):
                    //    this.drawObject.AttachPropertyData.Clear();
                    //    if (this.listData.ContainsKey(e.ItemName))
                    //        this.listData[e.ItemName] = e.DataContent;
                    //    else
                    //        this.listData.Add(e.ItemName, e.DataContent);
                    //    this.drawObject.AttachPropertyData.AddRange(this.listData.Values.ToArray());
                    //    userWcsEllipseSector wcsEllipseSector = ((userWcsEllipseSector)e.DataContent);
                    //    if (wcsEllipseSector.EdgesPoint_xyz != null)
                    //    {
                    //        for (int i = 0; i < wcsEllipseSector.EdgesPoint_xyz.Length; i++)
                    //        {
                    //            this.drawObject.AttachPropertyData.Add(new userWcsPoint(wcsEllipseSector.EdgesPoint_xyz[i].X, wcsEllipseSector.EdgesPoint_xyz[i].Y, 0, wcsEllipseSector.CamParams));
                    //        }
                    //    }
                    //    this.drawObject.DetachDrawingObjectFromWindow();
                    //    break;
                    //case nameof(userWcsLine):
                    //    this.drawObject.AttachPropertyData.Clear();
                    //    if (this.listData.ContainsKey(e.ItemName))
                    //        this.listData[e.ItemName] = e.DataContent;
                    //    else
                    //        this.listData.Add(e.ItemName, e.DataContent);
                    //    this.drawObject.AttachPropertyData.AddRange(this.listData.Values.ToArray());
                    //    userWcsLine wcsLine = ((userWcsLine)e.DataContent);
                    //    if (wcsLine.EdgesPoint_xyz != null)
                    //    {
                    //        for (int i = 0; i < wcsLine.EdgesPoint_xyz.Length; i++)
                    //        {
                    //            this.drawObject.AttachPropertyData.Add(new userWcsPoint(wcsLine.EdgesPoint_xyz[i].X, wcsLine.EdgesPoint_xyz[i].Y, 0, wcsLine.CamParams));
                    //        }
                    //    }
                    //    this.drawObject.DetachDrawingObjectFromWindow();
                    //    break;

                    case nameof(userWcsPoint):
                        this.drawObject.AttachPropertyData.Clear();
                        if (this.listData.ContainsKey(e.ItemName))
                            this.listData[e.ItemName] = e.DataContent;
                        else
                            this.listData.Add(e.ItemName, e.DataContent);
                        this.drawObject.AttachPropertyData.AddRange(this.listData.Values.ToArray()); // 点对象本身就是一个点，所以这里不再考虑显示子元素
                        this.drawObject.DetachDrawingObjectFromWindow();
                        /////////////////////////////////////////////////
                        userWcsPoint wcsPoint = (userWcsPoint)e.DataContent;
                        userPixPoint pixPoint = wcsPoint.GetPixPoint();
                        /////////////////////////////////////////////////
                        if (!this.listWcsPoint.ContainsKey(e.ItemName))
                            this.listWcsPoint.Add(e.ItemName, new userWcsVector(wcsPoint.X, wcsPoint.Y, wcsPoint.Z, 0, wcsPoint.Grab_x, wcsPoint.Grab_y, wcsPoint.CamParams));
                        else
                            this.listWcsPoint[e.ItemName] = new userWcsVector(wcsPoint.X, wcsPoint.Y, wcsPoint.Z, 0, wcsPoint.Grab_x, wcsPoint.Grab_y, wcsPoint.CamParams);
                        ///////////////////////////////////
                        if (!this.listPixPoint.ContainsKey(e.ItemName))
                            this.listPixPoint.Add(e.ItemName, new userPixVector(pixPoint.Row, pixPoint.Col, 0, pixPoint.Grab_x, pixPoint.Grab_y, pixPoint.Grab_theta, pixPoint.CamParams));
                        else
                            this.listPixPoint[e.ItemName] = new userPixVector(pixPoint.Row, pixPoint.Col, 0, pixPoint.Grab_x, pixPoint.Grab_y, pixPoint.Grab_theta, pixPoint.CamParams);
                        break;
                    ////////////////////////////////////////////////////////////////
                    case nameof(userWcsVector):
                        this.drawObject.AttachPropertyData.Clear();
                        if (this.listData.ContainsKey(e.ItemName))
                            this.listData[e.ItemName] = e.DataContent;
                        else
                            this.listData.Add(e.ItemName, e.DataContent);
                        this.drawObject.AttachPropertyData.AddRange(this.listData.Values.ToArray()); // 点对象本身就是一个点，所以这里不再考虑显示子元素
                        this.drawObject.DetachDrawingObjectFromWindow();
                        /////////////////////
                        if (!this.listWcsPoint.ContainsKey(e.ItemName))
                            this.listWcsPoint.Add(e.ItemName, ((userWcsVector)e.DataContent));
                        else
                            this.listWcsPoint[e.ItemName] = ((userWcsVector)e.DataContent);
                        ///////////////////////////////////
                        if (!this.listPixPoint.ContainsKey(e.ItemName))
                            this.listPixPoint.Add(e.ItemName, ((userWcsVector)e.DataContent).GetPixVector());
                        else
                            this.listPixPoint[e.ItemName] = ((userWcsVector)e.DataContent).GetPixVector();
                        break;
                    ////////////////////////////////////////////////////////////////
                    case nameof(userWcsRectangle2):
                        this.drawObject.AttachPropertyData.Clear();
                        if (this.listData.ContainsKey(e.ItemName))
                            this.listData[e.ItemName] = e.DataContent;
                        else
                            this.listData.Add(e.ItemName, e.DataContent);
                        this.drawObject.AttachPropertyData.AddRange(this.listData.Values.ToArray());
                        this.drawObject.DetachDrawingObjectFromWindow();
                        userWcsRectangle2 wcsRect2 = ((userWcsRectangle2)e.DataContent);
                        userWcsVector wcsVector = new userWcsVector(wcsRect2.X, wcsRect2.Y, wcsRect2.Z, wcsRect2.Deg, wcsRect2.CamParams);
                        wcsVector.Grab_x = wcsRect2.Grab_x;
                        wcsVector.Grab_y = wcsRect2.Grab_y;
                        wcsVector.Grab_theta = wcsRect2.Grab_theta;
                        /////////////////////
                        if (!this.listWcsPoint.ContainsKey(e.ItemName))
                            this.listWcsPoint.Add(e.ItemName, wcsVector);
                        else
                            this.listWcsPoint[e.ItemName] = wcsVector;
                        ///////////////////////////////////
                        if (!this.listPixPoint.ContainsKey(e.ItemName))
                            this.listPixPoint.Add(e.ItemName, wcsVector.GetPixVector());
                        else
                            this.listPixPoint[e.ItemName] = wcsVector.GetPixVector();
                        break;
                }
            }
            catch (Exception ex)
            {
                new UserMessageForm(ex.ToString()).ShowDialog();
            }
        }

        public void 映射标定button_Click(object sender, EventArgs e)
        {
            try
            {
                List<double> listTargetWcs_x = new List<double>();
                List<double> listTargetWcs_y = new List<double>();
                List<double> listTargetWcs_angle = new List<double>();
                List<double> listTargetPix_Row = new List<double>();
                List<double> listTargetPix_Col = new List<double>();
                List<double> listTargetPix_rad = new List<double>();
                List<double> listTarget_GrabX = new List<double>();
                List<double> listTarget_GrabY = new List<double>();
                List<double> listTarget_GrabZ = new List<double>();
                List<double> listTarget_GrabTheta = new List<double>();
                List<double> listSourceWcs_x = new List<double>();
                List<double> listSourceWcs_y = new List<double>();
                List<double> listSourceWcs_angle = new List<double>();
                List<double> listSourcePix_Row = new List<double>();
                List<double> listSourcePix_Col = new List<double>();
                List<double> listSourcePix_rad = new List<double>();
                List<double> listSource_GrabX = new List<double>();
                List<double> listSource_GrabY = new List<double>();
                List<double> listSource_GrabZ = new List<double>();
                List<double> listSource_GrabTheta = new List<double>();
                DataGridViewCellCollection CellCollection;
                HHomMat2D hHomMat2D = new HHomMat2D();
                HTuple Qx = 0, Qy = 0, Qz = 0, dist = 0;
                enCoordOriginType coordOriginType = enCoordOriginType.机械原点;
                switch (this.映射方法comboBox.SelectedItem.ToString())
                {
                    default:
                    case "WcsToWcs":
                        ///// 提取上相机坐标
                        for (int i = 0; i < this.目标相机坐标dataGridView.Rows.Count; i++)
                        {
                            CellCollection = this.目标相机坐标dataGridView.Rows[i].Cells;
                            listTargetWcs_x.Add(Convert.ToDouble(CellCollection[0].Value));
                            listTargetWcs_y.Add(Convert.ToDouble(CellCollection[1].Value));
                            listTargetWcs_angle.Add(Convert.ToDouble(CellCollection[2].Value));
                            listTarget_GrabX.Add(Convert.ToDouble(CellCollection[3].Value));
                            listTarget_GrabY.Add(Convert.ToDouble(CellCollection[4].Value));
                            listTarget_GrabZ.Add(Convert.ToDouble(CellCollection[5].Value));
                            listTarget_GrabTheta.Add(Convert.ToDouble(CellCollection[6].Value));
                        }
                        //////提取下相机坐标
                        for (int i = 0; i < this.源相机坐标dataGridView.Rows.Count; i++)
                        {
                            CellCollection = this.源相机坐标dataGridView.Rows[i].Cells;
                            listSourceWcs_x.Add(Convert.ToDouble(CellCollection[0].Value));
                            listSourceWcs_y.Add(Convert.ToDouble(CellCollection[1].Value));
                            listSourceWcs_angle.Add(Convert.ToDouble(CellCollection[2].Value));
                            listSource_GrabX.Add(Convert.ToDouble(CellCollection[3].Value));
                            listSource_GrabY.Add(Convert.ToDouble(CellCollection[4].Value));
                            listSource_GrabZ.Add(Convert.ToDouble(CellCollection[5].Value));
                            listSource_GrabTheta.Add(Convert.ToDouble(CellCollection[6].Value));
                        }
                        ///////////////////////////////  世界坐标变换必需使用刚性变换  ///////////////////////////////
                        hHomMat2D.VectorToRigid(listSourceWcs_x.ToArray(), listSourceWcs_y.ToArray(), listTargetWcs_x.ToArray(), listTargetWcs_y.ToArray());
                        this.CamParam_Map.MapHomMat2D = new UserHomMat2D(hHomMat2D);
                        this.CamParam_Map.MapType = this.映射方法comboBox.Text;
                        if (this.CamParam_Map.DicMapHomMat2D.ContainsKey(this.CamParam_Map.MapType))
                            this.CamParam_Map.DicMapHomMat2D[this.CamParam_Map.MapType] = new UserHomMat2D(hHomMat2D);
                        else
                            this.CamParam_Map.DicMapHomMat2D.Add(this.CamParam_Map.MapType, new UserHomMat2D(hHomMat2D));
                        this.CamParam_Map.CaliParam.CoordOriginType = enCoordOriginType.映射变换; // WcsToWcs ： 一定要是这样
                        Qx = hHomMat2D.AffineTransPoint2d(listSourceWcs_x.ToArray(), listSourceWcs_y.ToArray(), out Qy);
                        dist = HMisc.DistancePp(Qx, Qy, listTargetWcs_x.ToArray(), listTargetWcs_y.ToArray());
                        break;
                    case "PixToWcs":
                        ///// 提取上相机坐标
                        double offset_x = 0, offset_y = 0; // 
                        double.TryParse(this.X偏移textBox.Text, out offset_x);
                        double.TryParse(this.Y偏移textBox.Text, out offset_y);
                        for (int i = 0; i < this.目标相机坐标dataGridView.Rows.Count; i++)
                        {
                            CellCollection = this.目标相机坐标dataGridView.Rows[i].Cells;
                            listTargetWcs_x.Add(Convert.ToDouble(CellCollection[0].Value) + offset_x);
                            listTargetWcs_y.Add(Convert.ToDouble(CellCollection[1].Value) + offset_y);
                            listTargetWcs_angle.Add(Convert.ToDouble(CellCollection[2].Value) + offset_y);

                            listTarget_GrabX.Add(Convert.ToDouble(CellCollection[3].Value));
                            listTarget_GrabY.Add(Convert.ToDouble(CellCollection[4].Value));
                            listTarget_GrabZ.Add(Convert.ToDouble(CellCollection[5].Value));
                            listTarget_GrabTheta.Add(Convert.ToDouble(CellCollection[6].Value));
                        }
                        /// 提取下相机坐标
                        for (int i = 0; i < this.源相机坐标dataGridView.Rows.Count; i++)
                        {
                            CellCollection = this.源相机坐标dataGridView.Rows[i].Cells;
                            listSourcePix_Row.Add(Convert.ToDouble(CellCollection[0].Value));
                            listSourcePix_Col.Add(Convert.ToDouble(CellCollection[1].Value));
                            listSourcePix_rad.Add(Convert.ToDouble(CellCollection[2].Value));

                            listSource_GrabX.Add(Convert.ToDouble(CellCollection[3].Value));
                            listSource_GrabY.Add(Convert.ToDouble(CellCollection[4].Value));
                            listSource_GrabZ.Add(Convert.ToDouble(CellCollection[5].Value));
                            listSource_GrabTheta.Add(Convert.ToDouble(CellCollection[6].Value));
                        }
                        ///////////////////////////////
                        hHomMat2D.VectorToHomMat2d(listSourcePix_Col.ToArray(), listSourcePix_Row.ToArray(), listTargetWcs_x.ToArray(), listTargetWcs_y.ToArray());
                        this.CamParam_Map.HomMat2D = new UserHomMat2D(hHomMat2D);
                        this.CamParam_Map.MapHomMat2D = new UserHomMat2D();
                        //this.CamParam_Map.CaliParam.CoordOriginType = enCoordOriginType.映射变换; // PixToWcs ： 一定要是这样
                        this.CamParam_Map.MapType = this.映射方法comboBox.Text;
                        this.CamParam_Map.ImagePointsToWorldPlane(listSourcePix_Row.ToArray(), listSourcePix_Col.ToArray(), 0, 0, 0, out Qx, out Qy, out Qz);
                        dist = HMisc.DistancePp(Qx, Qy, listTargetWcs_x.ToArray(), listTargetWcs_y.ToArray());
                        break;
                    case "PixToPix":
                        ///// 提取上相机坐标
                        for (int i = 0; i < this.目标相机坐标dataGridView.Rows.Count; i++)
                        {
                            CellCollection = this.目标相机坐标dataGridView.Rows[i].Cells;
                            listTargetPix_Row.Add(Convert.ToDouble(CellCollection[0].Value));
                            listTargetPix_Col.Add(Convert.ToDouble(CellCollection[1].Value));
                            listTargetPix_rad.Add(Convert.ToDouble(CellCollection[2].Value));

                            listTarget_GrabX.Add(Convert.ToDouble(CellCollection[3].Value));
                            listTarget_GrabY.Add(Convert.ToDouble(CellCollection[4].Value));
                            listTarget_GrabZ.Add(Convert.ToDouble(CellCollection[5].Value));
                            listTarget_GrabTheta.Add(Convert.ToDouble(CellCollection[6].Value));
                        }
                        //////提取下相机坐标
                        for (int i = 0; i < this.源相机坐标dataGridView.Rows.Count; i++)
                        {
                            CellCollection = this.源相机坐标dataGridView.Rows[i].Cells;
                            listSourcePix_Row.Add(Convert.ToDouble(CellCollection[0].Value));
                            listSourcePix_Col.Add(Convert.ToDouble(CellCollection[1].Value));
                            listSourcePix_rad.Add(Convert.ToDouble(CellCollection[2].Value));

                            listSource_GrabX.Add(Convert.ToDouble(CellCollection[3].Value));
                            listSource_GrabY.Add(Convert.ToDouble(CellCollection[4].Value));
                            listSource_GrabZ.Add(Convert.ToDouble(CellCollection[5].Value));
                            listSource_GrabTheta.Add(Convert.ToDouble(CellCollection[6].Value));
                        }
                        ///////////////////////////////
                        switch (listSourcePix_Col.Count)
                        {                            
                            // 刚性变换 
                            case 1:
                                hHomMat2D.VectorAngleToRigid(listSourcePix_Row[0], listSourcePix_Col[0], listSourcePix_rad[0], listTargetPix_Row[0], listTargetPix_Col[0], listTargetPix_rad[0]);
                                this.CamParam_Map.HomMat2D = this.CamParam_Target.HomMat2D.Clone(); // 复制目标相机的标定参数
                                this.CamParam_Map.MapHomMat2D = new UserHomMat2D(hHomMat2D); // 映射矩阵 
                                this.CamParam_Map.CaliParam.RotateCalibPoint = new userWcsVector(listSource_GrabX[0], listSource_GrabY[0], listSource_GrabZ[0], listSource_GrabTheta[0]);
                                this.CamParam_Map.CaliParam.CalibCenterXy = this.CamParam_Target.CaliParam.CalibCenterXy.Clone();
                                this.CamParam_Map.CaliParam.CalibCenterXz = this.CamParam_Target.CaliParam.CalibCenterXz.Clone();
                                this.CamParam_Map.CaliParam.CalibCenterYz = this.CamParam_Target.CaliParam.CalibCenterYz.Clone();
                                this.CamParam_Map.CaliParam.AdjHomMatC02X = this.CamParam_Target.CaliParam.AdjHomMatC02X;
                                this.CamParam_Map.CaliParam.AdjHomMatC12Y = this.CamParam_Target.CaliParam.AdjHomMatC12Y;
                                this.CamParam_Map.MapType = this.映射方法comboBox.Text;
                                break;
                            // 刚性变换 
                            case 2:
                                hHomMat2D.VectorToRigid(listSourcePix_Row.ToArray(), listSourcePix_Col.ToArray(), listTargetPix_Row.ToArray(), listTargetPix_Col.ToArray());
                                this.CamParam_Map.HomMat2D = this.CamParam_Target.HomMat2D.Clone(); // 复制目标相机的标定参数
                                this.CamParam_Map.MapHomMat2D = new UserHomMat2D(hHomMat2D); // 映射矩阵 
                                this.CamParam_Map.CaliParam.RotateCalibPoint = new userWcsVector(listSource_GrabX[0], listSource_GrabY[0], listSource_GrabZ[0], listSource_GrabTheta[0]);
                                this.CamParam_Map.CaliParam.CalibCenterXy = this.CamParam_Target.CaliParam.CalibCenterXy.Clone();
                                this.CamParam_Map.CaliParam.CalibCenterXz = this.CamParam_Target.CaliParam.CalibCenterXz.Clone();
                                this.CamParam_Map.CaliParam.CalibCenterYz = this.CamParam_Target.CaliParam.CalibCenterYz.Clone();
                                this.CamParam_Map.CaliParam.AdjHomMatC02X = this.CamParam_Target.CaliParam.AdjHomMatC02X;
                                this.CamParam_Map.CaliParam.AdjHomMatC12Y = this.CamParam_Target.CaliParam.AdjHomMatC12Y;
                                this.CamParam_Map.MapType = this.映射方法comboBox.Text;
                                break;
                            // 仿射变换
                            default:
                                hHomMat2D.VectorToHomMat2d(listSourcePix_Row.ToArray(), listSourcePix_Col.ToArray(), listTargetPix_Row.ToArray(), listTargetPix_Col.ToArray());
                                this.CamParam_Map.HomMat2D = this.CamParam_Target.HomMat2D.Clone(); // 复制目标相机的标定参数
                                this.CamParam_Map.MapHomMat2D = new UserHomMat2D(hHomMat2D); // 映射矩阵 
                                this.CamParam_Map.CaliParam.RotateCalibPoint = new userWcsVector(listSource_GrabX[0], listSource_GrabY[0], listSource_GrabZ[0], listSource_GrabTheta[0]); // 映射相机以当前拍照作为旋转标定点
                                this.CamParam_Map.CaliParam.CalibCenterXy = this.CamParam_Target.CaliParam.CalibCenterXy.Clone();
                                this.CamParam_Map.CaliParam.CalibCenterXz = this.CamParam_Target.CaliParam.CalibCenterXz.Clone();
                                this.CamParam_Map.CaliParam.CalibCenterYz = this.CamParam_Target.CaliParam.CalibCenterYz.Clone();
                                this.CamParam_Map.CaliParam.AdjHomMatC02X = this.CamParam_Target.CaliParam.AdjHomMatC02X;
                                this.CamParam_Map.CaliParam.AdjHomMatC12Y = this.CamParam_Target.CaliParam.AdjHomMatC12Y;
                                this.CamParam_Map.MapType = this.映射方法comboBox.Text;
                                break;
                        }
                        Qx = hHomMat2D.AffineTransPoint2d(listSourcePix_Row.ToArray(), listSourcePix_Col.ToArray(), out Qy);
                        dist = HMisc.DistancePp(Qx, Qy, listTargetPix_Row.ToArray(), listTargetPix_Col.ToArray());
                        break;
                }
                this.源相机映射坐标dataGridView.Rows.Clear();
                for (int i = 0; i < Qx.Length; i++)
                {
                    int index = this.源相机映射坐标dataGridView.Rows.Add(Qx[i].D, Qy[i].D);
                    this.源相机映射坐标dataGridView.Rows[index].HeaderCell.Value = (index + 1).ToString();
                }
                ///////////////////////////////////////
                this.映射dataGridView.Rows.Clear();
                this.映射dataGridView.Rows.Add(this.CamParam_Map.MapHomMat2D.c00, this.CamParam_Map.MapHomMat2D.c01, this.CamParam_Map.MapHomMat2D.c02);
                this.映射dataGridView.Rows.Add(this.CamParam_Map.MapHomMat2D.c10, this.CamParam_Map.MapHomMat2D.c11, this.CamParam_Map.MapHomMat2D.c12);
                //switch (this.CamParam_Map.MapType) //this.映射方法comboBox.SelectedItem.ToString() 
                //{
                //    case this.映射方法comboBox.Text:
                //        this.映射dataGridView.Rows.Add(this.CamParam_Map.MapHomMat2D.c00, this.CamParam_Map.MapHomMat2D.c01, this.CamParam_Map.MapHomMat2D.c02);
                //        this.映射dataGridView.Rows.Add(this.CamParam_Map.MapHomMat2D.c10, this.CamParam_Map.MapHomMat2D.c11, this.CamParam_Map.MapHomMat2D.c12);
                //        break;
                //    case enMapMethod.PixToWcs:
                //        this.映射dataGridView.Rows.Add(this.CamParam_Map.HomMat2D.c00, this.CamParam_Map.HomMat2D.c01, this.CamParam_Map.HomMat2D.c02);
                //        this.映射dataGridView.Rows.Add(this.CamParam_Map.HomMat2D.c10, this.CamParam_Map.HomMat2D.c11, this.CamParam_Map.HomMat2D.c12);
                //        break;
                //    case enMapMethod.PixToPix:
                //        this.映射dataGridView.Rows.Add(this.CamParam_Map.MapHomMat2D.c00, this.CamParam_Map.MapHomMat2D.c01, this.CamParam_Map.MapHomMat2D.c02);
                //        this.映射dataGridView.Rows.Add(this.CamParam_Map.MapHomMat2D.c10, this.CamParam_Map.MapHomMat2D.c11, this.CamParam_Map.MapHomMat2D.c12);
                //        break;
                //}
                // new UserMessageForm().ShowDialog("最大标定误差 = " + dist.TupleMax().D.ToString());
                if (new UserMessageForm().ShowDialog( "最大标定误差 = " + dist.TupleMax().D.ToString() + "; 映射标定矩阵:" + this.CamParam_Map.MapHomMat2D.ToString(),"是否更新并保存参数？") == DialogResult.OK)
                {
                    this.CamParam_Map?.Save();
                }
            }
            catch (Exception ex)
            {
                new UserMessageForm(ex.ToString()).ShowDialog();
            }
        }

        public void DisplayClickObject(object sender, TreeNodeMouseClickEventArgs e)  //
        {
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
                        this.drawObject.SetParam(((CircleMeasure)e.Node.Tag).PixCoordSystem);
                        this.drawObject.SetParam(((CircleMeasure)e.Node.Tag).FindCircle.CirclePixPosition.AffineTransPixCircle(((CircleMeasure)e.Node.Tag).PixCoordSystem.GetVariationHomMat2D()));
                        this.drawObject.BackImage = ((CircleMeasure)e.Node.Tag).ImageData;// != null ? ((CircleMeasure)e.Node.Tag).ImageData : this.CurrentImageData;
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
                        this.drawObject.SetParam(((CircleSectorMeasure)e.Node.Tag).PixCoordSystem);
                        this.drawObject.SetParam(((CircleSectorMeasure)e.Node.Tag).FindCircleSector.CircleSectorPixPosition.AffineTransPixCircleSector(((CircleSectorMeasure)e.Node.Tag).PixCoordSystem.GetVariationHomMat2D()));
                        this.drawObject.BackImage = ((CircleSectorMeasure)e.Node.Tag).ImageData;// != null ? ((CircleSectorMeasure)e.Node.Tag).ImageData : this.CurrentImageData;
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
                        this.drawObject.SetParam(((EllipseMeasure)e.Node.Tag).PixCoordSystem);
                        this.drawObject.SetParam(((EllipseMeasure)e.Node.Tag).FindEllipse.EllipsePixPosition.AffineTransPixEllipse(((EllipseMeasure)e.Node.Tag).PixCoordSystem.GetVariationHomMat2D()));
                        this.drawObject.BackImage = ((EllipseMeasure)e.Node.Tag).ImageData;// != null ? ((EllipseMeasure)e.Node.Tag).ImageData : this.CurrentImageData;
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
                        this.drawObject.SetParam(((EllipseSectorMeasure)e.Node.Tag).PixCoordSystem);
                        this.drawObject.SetParam(((EllipseSectorMeasure)e.Node.Tag).FindEllipseSector.EllipseSectorPixPosition.AffineTransPixEllipseSector(((EllipseSectorMeasure)e.Node.Tag).PixCoordSystem.GetVariationHomMat2D()));
                        this.drawObject.BackImage = ((EllipseSectorMeasure)e.Node.Tag).ImageData;// != null ? ((EllipseSectorMeasure)e.Node.Tag).ImageData : this.CurrentImageData;
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
                        this.drawObject.SetParam(((LineMeasure)e.Node.Tag).PixCoordSystem);
                        this.drawObject.SetParam(((LineMeasure)e.Node.Tag).FindLine.LinePixPosition.AffinePixLine2D(((LineMeasure)e.Node.Tag).PixCoordSystem.GetVariationHomMat2D()));
                        this.drawObject.BackImage = ((LineMeasure)e.Node.Tag).ImageData;// != null ? ((LineMeasure)e.Node.Tag).ImageData : this.CurrentImageData;
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
                        this.drawObject.SetParam(((PointMeasure)e.Node.Tag).PixCoordSystem);
                        this.drawObject.SetParam(((PointMeasure)e.Node.Tag).FindPoint.LinePixPosition.AffinePixLine2D(((PointMeasure)e.Node.Tag).PixCoordSystem.GetVariationHomMat2D()));
                        this.drawObject.BackImage = ((PointMeasure)e.Node.Tag).ImageData;// != null ? ((PointMeasure)e.Node.Tag).ImageData : this.CurrentImageData;
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
                        this.drawObject.SetParam(((Rectangle2Measure)e.Node.Tag).PixCoordSystem);
                        this.drawObject.SetParam(((Rectangle2Measure)e.Node.Tag).FindRect2.Rect2PixPosition.AffineTransPixRect2(((Rectangle2Measure)e.Node.Tag).PixCoordSystem.GetVariationHomMat2D())); //?.AffineWcsRectangle2D(((Rectangle2Measure)e.Node.Tag).WcsCoordSystem.GetVariationHomMat2D())
                        this.drawObject.BackImage = ((Rectangle2Measure)e.Node.Tag).ImageData;// != null ? ((Rectangle2Measure)e.Node.Tag).ImageData : this.CurrentImageData;
                        this.drawObject.AttachDrawingObjectToWindow();
                        this.metrolegyParamForm.drawObject = this.drawObject;
                        this._currFunction = (Rectangle2Measure)e.Node.Tag;
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
                        this.drawObject.SetParam(((CrossPointMeasure)e.Node.Tag).PixCoordSystem);
                        this.drawObject.SetParam(((CrossPointMeasure)e.Node.Tag).FindCrossPoint.LinePixPosition.AffinePixLine2D(((CrossPointMeasure)e.Node.Tag).PixCoordSystem.GetVariationHomMat2D()));
                        this.drawObject.BackImage = ((CrossPointMeasure)e.Node.Tag).ImageData;// != null ? ((CrossPointMeasure)e.Node.Tag).ImageData : this.CurrentImageData;
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
                        //DisplayClickItem(sender, e);
                        break;
                    default:

                        break;
                }
            }
            catch (Exception ex)
            {
                new UserMessageForm(ex.ToString()).ShowDialog();
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
            catch (Exception ex)
            {

            }
        }

        private void UpDnCamCaliForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                this.drawObject.GrayValueInfo -= new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                BaseFunction.ExcuteCompleted -= new ExcuteCompletedEventHandler(DisplayExcuteResult);
                TreeViewWrapClass.ClickNode -= new ClickNodeEventHandler(this.DisplayClickObject);
                this._treeViewWrapClass_Down?.Uinit();
                this._treeViewWrapClass_Up?.Uinit();
            }
            catch
            {

            }
        }
        private CancellationTokenSource cts1;
        private CancellationTokenSource cts2;
        private void 实时采集checkBox_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                switch (this.上相机实时采集checkBox.CheckState)
                {
                    case CheckState.Checked:
                        this.上相机实时采集checkBox.BackColor = Color.Red;
                        AcqSource acqSource = AcqSourceManage.Instance.GetCamAcqSource(this.CamParam_Target.SensorName);
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
                        this.上相机实时采集checkBox.BackColor = Color.Lime;
                        break;
                }
            }
            catch (Exception ex)
            {
                new UserMessageForm(ex.ToString()).ShowDialog();
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

        private void 下相机实时采集checkBox_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                switch (this.上相机实时采集checkBox.CheckState)
                {
                    case CheckState.Checked:
                        this.上相机实时采集checkBox.BackColor = Color.Red;
                        AcqSource acqSource = AcqSourceManage.Instance.GetCamAcqSource(this.CamParam_Map.SensorName);
                        if (acqSource == null) return;
                        cts2 = new CancellationTokenSource();
                        Dictionary<enDataItem, object> data;
                        Task.Run(() =>
                        {
                            this.drawObject.IsLiveState = true;
                            while (!cts2.IsCancellationRequested)
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
                        cts2?.Cancel();
                        this.上相机实时采集checkBox.BackColor = Color.Lime;
                        break;
                }
            }
            catch (Exception ex)
            {
                new UserMessageForm(ex.ToString()).ShowDialog();
            }
        }

        public void 重置下相机映射but_Click(object sender, EventArgs e)
        {
            try
            {
                this.映射dataGridView.Rows.Clear();
                this.CamParam_Map.MapHomMat2D = new UserHomMat2D();
                this.映射dataGridView.Rows.Add(this.CamParam_Map.MapHomMat2D.c00, this.CamParam_Map.MapHomMat2D.c01, this.CamParam_Map.MapHomMat2D.c02);
                this.映射dataGridView.Rows.Add(this.CamParam_Map.MapHomMat2D.c10, this.CamParam_Map.MapHomMat2D.c11, this.CamParam_Map.MapHomMat2D.c12);
                //switch (this.映射方法comboBox.SelectedItem.ToString())
                //{
                //    case "WcsToWcs":
                //        this.映射dataGridView.Rows.Clear();
                //        this.CamParam_Map.MapHomMat2D = new UserHomMat2D();
                //        this.映射dataGridView.Rows.Add(this.CamParam_Map.MapHomMat2D.c00, this.CamParam_Map.MapHomMat2D.c01, this.CamParam_Map.MapHomMat2D.c02);
                //        this.映射dataGridView.Rows.Add(this.CamParam_Map.MapHomMat2D.c10, this.CamParam_Map.MapHomMat2D.c11, this.CamParam_Map.MapHomMat2D.c12);
                //        break;
                //    case "PixToWcs":
                //        this.映射dataGridView.Rows.Clear();
                //        this.CamParam_Map.HomMat2D = new UserHomMat2D();
                //        this.映射dataGridView.Rows.Add(this.CamParam_Map.HomMat2D.c00, this.CamParam_Map.HomMat2D.c01, this.CamParam_Map.HomMat2D.c02);
                //        this.映射dataGridView.Rows.Add(this.CamParam_Map.HomMat2D.c10, this.CamParam_Map.HomMat2D.c11, this.CamParam_Map.HomMat2D.c12);
                //        break;
                //    case "PixToPix":
                //        this.映射dataGridView.Rows.Clear();
                //        this.CamParam_Map.MapHomMat2D = new UserHomMat2D();
                //        this.映射dataGridView.Rows.Add(this.CamParam_Map.MapHomMat2D.c00, this.CamParam_Map.MapHomMat2D.c01, this.CamParam_Map.MapHomMat2D.c02);
                //        this.映射dataGridView.Rows.Add(this.CamParam_Map.MapHomMat2D.c10, this.CamParam_Map.MapHomMat2D.c11, this.CamParam_Map.MapHomMat2D.c12);
                //        break;
                //}
            }
            catch (Exception ex)
            {
                new UserMessageForm(ex.ToString()).ShowDialog();
            }
        }

        private void LoadMapParam()
        {
            try
            {
                switch (this.映射方法comboBox.SelectedItem.ToString())
                {
                    case "WcsToWcs":
                        this.映射dataGridView.Rows.Clear();
                        this.映射dataGridView.Rows.Add(this.CamParam_Map.MapHomMat2D.c00, this.CamParam_Map.MapHomMat2D.c01, this.CamParam_Map.MapHomMat2D.c02);
                        this.映射dataGridView.Rows.Add(this.CamParam_Map.MapHomMat2D.c10, this.CamParam_Map.MapHomMat2D.c11, this.CamParam_Map.MapHomMat2D.c12);
                        break;
                    case "PixToWcs":
                        this.映射dataGridView.Rows.Clear();
                        this.映射dataGridView.Rows.Add(this.CamParam_Map.HomMat2D.c00, this.CamParam_Map.HomMat2D.c01, this.CamParam_Map.HomMat2D.c02);
                        this.映射dataGridView.Rows.Add(this.CamParam_Map.HomMat2D.c10, this.CamParam_Map.HomMat2D.c11, this.CamParam_Map.HomMat2D.c12);
                        break;
                    case "PixToPix":
                        this.映射dataGridView.Rows.Clear();
                        this.映射dataGridView.Rows.Add(this.CamParam_Map.MapHomMat2D.c00, this.CamParam_Map.MapHomMat2D.c01, this.CamParam_Map.MapHomMat2D.c02);
                        this.映射dataGridView.Rows.Add(this.CamParam_Map.MapHomMat2D.c10, this.CamParam_Map.MapHomMat2D.c11, this.CamParam_Map.MapHomMat2D.c12);
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
        private void addContextMenu(HWindowControl hWindowControl)
        {
            ContextMenuStrip ContextMenuStrip1 = new ContextMenuStrip();
            ContextMenuStrip1.Name = hWindowControl.Name;

            // 添加右键菜单 
            ToolStripItem[] items = new ToolStripMenuItem[]
            {
                new ToolStripMenuItem("执行"),
                new ToolStripMenuItem("设置抓边参数"),
                new ToolStripMenuItem("------------"),
                new ToolStripMenuItem("自适应窗口"),
                new ToolStripMenuItem("清除窗口"),
                new ToolStripMenuItem("保存图像"),
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
                new UserMessageForm(ex.ToString()).ShowDialog();
            }
        }

        private void 映射方法comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.映射方法comboBox.Text == "PixToWcs")   // 只有在PixToWcs时才会设置
            {
                this.X偏移textBox.Enabled = true;
                this.Y偏移textBox.Enabled = true;
            }
            else
            {
                this.X偏移textBox.Enabled = false;
                this.Y偏移textBox.Enabled = false;
            }
        }

        private void 获取标定位button_Click(object sender, EventArgs e)
        {
            try
            {
                double X = 0, Y = 0, Z = 0, Theta = 0;
                Read(out X, out Y, out Z, out Theta);
                /////////////////////
                this.CamParam_Map.CaliParam.StartCaliPoint = new userWcsVector(X, Y, Z, Theta);
                this.CamParam_Map.CaliParam.EndCalibPoint = new userWcsVector(X, Y, Z, Theta);
                this.CamParam_Map.CaliParam.RotateCalibPoint = new userWcsVector(X, Y, Z, Theta);
                this.标定位textBox.Text = "X:" + X.ToString("f3") + "   Y:" + Y.ToString("f3") + "   Z:" + Z.ToString("f3") + "   Theta:" + Theta.ToString("f3");
            }
            catch (Exception ex)
            {
                new UserMessageForm(ex.ToString()).ShowDialog();
            }
        }

        private void Read(out double X, out double Y, out double Z, out double Theta)
        {
            X = 0; Y = 0; Z = 0; Theta = 0;
            MotionCardManage.GetCard(this.CamParam_Map.CaliParam.CoordSysName).GetAxisPosition(this.CamParam_Map.CaliParam.CoordSysName, enAxisName.X轴, out X);
            MotionCardManage.GetCard(this.CamParam_Map.CaliParam.CoordSysName).GetAxisPosition(this.CamParam_Map.CaliParam.CoordSysName, enAxisName.Y轴, out Y);
            MotionCardManage.GetCard(this.CamParam_Map.CaliParam.CoordSysName).GetAxisPosition(this.CamParam_Map.CaliParam.CoordSysName, enAxisName.Z轴, out Z);
            MotionCardManage.GetCard(this.CamParam_Map.CaliParam.CoordSysName).GetAxisPosition(this.CamParam_Map.CaliParam.CoordSysName, enAxisName.Theta轴, out Theta);
        }

        private void 标定位textBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string[] name = 标定位textBox.Text.Split(new string[] { "X:", "Y:", "Z:", "Theta:" }, StringSplitOptions.RemoveEmptyEntries);
                if (name.Length < 4) return;
                double X, Y, Z, Theta;
                bool result1 = double.TryParse(name[0].Trim(), out X);
                bool result2 = double.TryParse(name[1].Trim(), out Y);
                bool result3 = double.TryParse(name[2].Trim(), out Z);
                bool result4 = double.TryParse(name[3].Trim(), out Theta);
                if (result1 && result2 && result3 && result4)
                {
                    this.CamParam_Map.CaliParam.StartCaliPoint = new userWcsVector(X, Y, Z, Theta);
                    this.CamParam_Map.CaliParam.EndCalibPoint = new userWcsVector(X, Y, Z, Theta);
                    this.CamParam_Map.CaliParam.RotateCalibPoint = new userWcsVector(X, Y, Z, Theta);
                }
                else
                    new UserMessageForm().ShowDialog("数据转换报错");
            }
            catch (Exception ex)
            {
                new UserMessageForm(ex.ToString()).ShowDialog();
            }
        }




        #endregion

        #region 数据视图右键菜单项
        private void addDataGridViewContextMenu(DataGridView dataGridView)
        {
            ContextMenuStrip ContextMenuStrip1 = new ContextMenuStrip();
            ContextMenuStrip1.Name = dataGridView.Name;
            // 添加右键菜单 
            ToolStripItem[] items = new ToolStripMenuItem[]
            {
                new ToolStripMenuItem("读取点位"),
                new ToolStripMenuItem("删除"),
                new ToolStripMenuItem("清空"),
            };
            ContextMenuStrip1.Items.AddRange(items); // 在滑赋值前不能调用 
            ContextMenuStrip1.ItemClicked += new ToolStripItemClickedEventHandler(dataGridViewContextMenuStrip_ItemClicked);
            dataGridView.ContextMenuStrip = ContextMenuStrip1;
        }
        private void dataGridViewContextMenuStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            string name = e.ClickedItem.Text;
            try
            {
                switch (name)
                {
                    case "读取点位":
                        //string name2 = ((ContextMenuStrip)sender).Name;  文本文件(*.txt)|*.txt|所有文件(*.*)|*.*
                        ((ContextMenuStrip)sender).Close();
                        OpenFileDialog openFileDialog = new OpenFileDialog();
                        openFileDialog.Filter = "文本文件(*.txt)|*.txt|CSV文件(*.csv)|*.csv|所有文件(*.*)|*.*";
                        if (openFileDialog.ShowDialog() == DialogResult.OK)
                        {
                            double[] X, Y;
                            new FileOperate().ReadTxt(openFileDialog.FileName, out X, out Y);
                            this.目标相机坐标dataGridView.Rows.Clear();
                            this.listWcsPoint.Clear();
                            for (int i = 0; i < X.Length; i++)
                            {
                                this.目标相机坐标dataGridView.Rows.Add(X[i], Y[i], 0, 0);
                                //this.listWcsPoint.Add(new userWcsPoint(X[i], Y[i], 0));
                            }
                        }
                        break;
                    case "删除":
                        ((ContextMenuStrip)sender).Close();
                        int currIndex = this.目标相机坐标dataGridView.CurrentRow.Index;
                        if (currIndex > 0)
                        {
                            this.目标相机坐标dataGridView.Rows.RemoveAt(currIndex);
                            //this.listWcsPoint.RemoveAt(currIndex);
                        }
                        break;
                    //////////////////////////////////////
                    case "清空":
                        ((ContextMenuStrip)sender).Close();
                        this.目标相机坐标dataGridView.Rows.Clear();
                        //this.listWcsPoint.Clear();
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                new UserMessageForm(ex.ToString()).ShowDialog();
            }
        }
        #endregion




    }
}

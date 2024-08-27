
using FunctionBlock;
using HalconDotNet;
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
using System.Threading;
using System.IO;
using Common;
using MotionControlCard;
using AlgorithmsLibrary;
using View;

namespace FunctionBlock
{

    public partial class AreaScanDivisionCalibrateForm : Form
    {
        private AcqSource _acqSource;
        private CancellationTokenSource cts;
        private CameraCalibrateTool calibrateCamera;
        private ImageDataClass image = null;
        private userCamParam camPara;
        private userCamPose camPose;
        private double error;
        private AxisCalibration calibrateFile;
        private userWcsPoint ref_Point;
        private double camSlant;
        private bool isUpdata = true;
        private VisualizeView drawObject;
        private TreeViewWrapClass _treeViewWrapClass;
        private string programPath = ""; // 程序文件路径
        private string _CamName = "";
        private CameraParam CamParam;
        public AreaScanDivisionCalibrateForm()
        {
            InitializeComponent();
            this._acqSource = new AcqSource(SensorManage.CurrentCamSensor);
            calibrateCamera = new CameraCalibrateTool();
            this.drawObject = new VisualizeView(this.hWindowControl1);
        }
        public AreaScanDivisionCalibrateForm(CameraParam CamParam)
        {
            InitializeComponent();
            this.CamParam = CamParam;
            //this._acqSource = AcqSourceManage.GetAcqSource(this._CamName);//  new AcqSource(SensorManage.CurrentCamSensor);
            calibrateCamera = new CameraCalibrateTool();
            this.drawObject = new VisualizeView(this.hWindowControl1);
        }
        private void MatrixCalibrateCameraParamForm_Load(object sender, EventArgs e)
        {
            BindProperty();
        }

        private void BindProperty()
        {
            try
            {
                this.采集源comboBox.DataSource = SensorManage.CameraList; // GetCamSensor();
                this.采集源comboBox.DisplayMember = "Name";
                this.摄像机模型comboBox.DataSource = Enum.GetNames(typeof(enCameraModel));
                this.标定配置类型comboBox.DataSource = Enum.GetNames(typeof(enCalibrationSetupType));
                this.标定类型comboBox.DataSource = Enum.GetNames(typeof(enCamCalibrateType));
                //////////////////
                this.圆心距textBox.DataBindings.Add("Text", this.calibrateCamera, "CircleDist", true, DataSourceUpdateMode.OnPropertyChanged); //CircleRadius
                this.圆半径textBox.DataBindings.Add("Text", this.calibrateCamera, "CircleRadius", true, DataSourceUpdateMode.OnPropertyChanged); //
                this.行数domainUpDown.DataBindings.Add("Text", this.calibrateCamera, "RowCount", true, DataSourceUpdateMode.OnPropertyChanged);
                this.列数domainUpDown.DataBindings.Add("Text", this.calibrateCamera, "ColCount", true, DataSourceUpdateMode.OnPropertyChanged);
                this.摄像机模型comboBox.DataBindings.Add("Text", this.calibrateCamera, "CameraType", true, DataSourceUpdateMode.OnPropertyChanged);
                this.阈值textBox.DataBindings.Add("Text", this.calibrateCamera, "Threshold", true, DataSourceUpdateMode.OnPropertyChanged);
                this.计算方法comboBox.DataBindings.Add("Text", this.calibrateCamera, "Method", true, DataSourceUpdateMode.OnPropertyChanged);
                this.轴长度textBox.DataBindings.Add("Text", this.calibrateCamera, "Axis_length", true, DataSourceUpdateMode.OnPropertyChanged);
                /////////////
                this.标定类型comboBox.DataBindings.Add("Text", this.calibrateCamera, "CamCalibrateType", true, DataSourceUpdateMode.OnPropertyChanged);
                this.左上行textBox.DataBindings.Add("Text", this.calibrateCamera, "X_offset", true, DataSourceUpdateMode.OnPropertyChanged);
                this.左上列textBox.DataBindings.Add("Text", this.calibrateCamera, "Y_offset", true, DataSourceUpdateMode.OnPropertyChanged);
                ///// 相机参数
                this.像元初始宽domainUpDown.DataBindings.Add("Text", this.calibrateCamera, "PixWidth", true, DataSourceUpdateMode.OnPropertyChanged);
                this.像元初始高domainUpDown.DataBindings.Add("Text", this.calibrateCamera, "PixHeight", true, DataSourceUpdateMode.OnPropertyChanged);
                this.镜头理论焦距domainUpDown.DataBindings.Add("Text", this.calibrateCamera, "Focus", true, DataSourceUpdateMode.OnPropertyChanged);
                this.中心点CxDomainUpDown.DataBindings.Add("Text", this.calibrateCamera, "Tilt", true, DataSourceUpdateMode.OnPropertyChanged);
                this.中心点CyDomainUpDown.DataBindings.Add("Text", this.calibrateCamera, "Rot", true, DataSourceUpdateMode.OnPropertyChanged);
                //////////////////////////
                this.图像宽度domainUpDown.DataBindings.Add("Text", this.calibrateCamera, "ImageWidth", true, DataSourceUpdateMode.OnPropertyChanged);
                this.图像高度domainUpDown.DataBindings.Add("Text", this.calibrateCamera, "ImageHeight", true, DataSourceUpdateMode.OnPropertyChanged);
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
            }
        }



        private void 采集源comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.采集源comboBox.SelectedItem == null) return;
        }
        private void CalibrateCameraParamForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (this.cts != null)
                    this.cts.Cancel();
            }
            catch
            {

            }
        }

        private void 摄像机模型comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string text = this.摄像机模型comboBox.SelectedItem.ToString();
            switch (text)
            {
                case "area_scan_division":
                    this.镜头理论焦距domainUpDown.Enabled = true;
                    AddForm(this.相机内参splitContainer.Panel1, new DivisionParamForm());
                    break;
                case "area_scan_telecentric_division":
                    this.镜头理论焦距domainUpDown.Enabled = false;
                    AddForm(this.相机内参splitContainer.Panel1, new PolynomialParamForm());
                    break;
              
                default:
                    break;
            }
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
        private void 标定区域button_Click(object sender, EventArgs e)
        {
            HalconLibrary ha = new HalconLibrary();
            userPixRectangle2 rect2;
            this.drawObject.AttachPropertyData.Clear();
            ha.DrawRectangle2OnWindow(this.hWindowControl1.HalconWindow, out rect2);
            this.drawObject.AttachPropertyData.Add(rect2);
            this.drawObject.ShowAttachProperty();
            this.calibrateCamera.Rect2 = rect2;
        }

        private void 标定button_Click_1(object sender, EventArgs e)
        {
            HalconLibrary ha = new HalconLibrary();
            try
            {
                //////////////////////////
                if (this.image == null) return;
                HXLDCont hXLDCont = this.calibrateCamera.CalibrateCamera(this.image.Image, out this.camPara, out this.camPose, out this.error);
                if (hXLDCont != null && hXLDCont.IsInitialized())
                {
                    this.drawObject.AttachPropertyData.Add(hXLDCont);
                    this.drawObject.ShowAttachProperty();
                }
                if (error <= 0.1)
                    this.标定状态textBox.Text = "标定结果良好";
                else
                    this.标定状态textBox.Text = "标定结果误差较大";
                this.平均误差textBox.Text = error.ToString();
                ////////////////////////////////////////////////
                switch (this.calibrateCamera.CameraType)
                {
                    case enCameraModel.area_scan_division:
                        AddForm(this.相机内参splitContainer.Panel1, new DivisionParamForm(this.camPara));
                        UpdataCamPose();
                        break;
                    case enCameraModel.area_scan_telecentric_division:
                        AddForm(this.相机内参splitContainer.Panel1, new DivisionParamForm(this.camPara));
                        UpdataCamPose();
                        break;
                    default:
                        break;
                }
                ha.Gen3DCoordSystem(this.hWindowControl1.HalconWindow, this.camPara.GetHtuple(), this.camPose.GetHtuple(), this.calibrateCamera.Axis_length);
            }
            catch (Exception ee)
            {
                MessageBox.Show("标定失败" + ee.ToString());
            }

        }


        private void 保存相机内参button_Click(object sender, EventArgs e)
        {
            FileOperate fo = new FileOperate();
            try
            {
                this._acqSource.Sensor.CameraParam.CamParam = this.camPara;
                this._acqSource.Sensor.CameraParam.Save();
                MessageBox.Show("保存成功");
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
            }
        }
        private void 保存相机外参button_Click(object sender, EventArgs e)
        {
            try
            {
                this._acqSource.Sensor.CameraParam.CamPose = this.camPose;
                this._acqSource.Sensor.CameraParam.Save();
                MessageBox.Show("保存成功");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }



        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            switch (e.ClickedItem.Text)
            {
                case "工具":
                    ToolForm tool = new ToolForm(this._treeViewWrapClass);
                    tool.Owner = this;
                    tool.Show();
                    break;
                case "执行":
                    break;
                case "打开":
                    this.programPath = new FileOperate().OpenFile(2);
                    if (this.programPath == null || this.programPath.Trim().Length == 0) return;
                    this._treeViewWrapClass.OpenProgram(this.programPath);
                    break;
                case "保存":
                    if (this.programPath == null || this.programPath.Length == 0)
                    {
                        this.programPath = new FileOperate().SaveFile(2);
                        if (this.programPath == null || this.programPath.Length == 0) return;
                        if (this._treeViewWrapClass.SaveProgram(this.programPath))  //ProgramItemsSource.getInstance().GetTreeViewNode()
                            MessageBox.Show("保存成功");
                        else
                            MessageBox.Show("保存失败" + new Exception().ToString());
                    }
                    else
                    {
                        if (this._treeViewWrapClass.SaveProgram(this.programPath))
                            MessageBox.Show("保存成功");
                        else
                            MessageBox.Show("保存失败" + new Exception().ToString());
                    }
                    break;
            }
        }

        private void UpdataCamPose()
        {
            this.X平移textBox.Text = this.camPose.Tx.ToString();
            this.Y平移textBox.Text = this.camPose.Ty.ToString();
            this.Z平移textBox.Text = this.camPose.Tz.ToString();
            this.X轴旋转textBox.Text = this.camPose.Rx.ToString();
            this.Y轴旋转textBox.Text = this.camPose.Ry.ToString();
            this.Z轴旋转textBox.Text = this.camPose.Rz.ToString();
            this.位姿类型textBox.Text = this.camPose.Type.ToString();
        }


        private void 采集图像button_Click(object sender, EventArgs e)
        {
            try
            {
                if (this._acqSource != null)
                {
                    Dictionary<enDataItem, object> listData = this._acqSource.AcqPointData();
                    if (listData == null || listData.Count == 0) return;
                    switch (listData[0].GetType().Name)
                    {
                        case "ImageDataClass":
                            this.drawObject.BackImage = (ImageDataClass)listData[enDataItem.Image];
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }

        private void 加载图像button_Click(object sender, EventArgs e)
        {
            try
            {
                string path = new FileOperate().OpenImage();
                HImage sourceImage = new HImage(path);
                if (sourceImage != null)
                    this.drawObject.BackImage = new ImageDataClass(sourceImage); //, this._acqSource.Sensor.CameraParam
                else
                    throw new ArgumentException("读取的图像为空");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog fileDialog = new SaveFileDialog();
                fileDialog.Filter = "bmp文件(*.bmp)|*.bmp|hobj文件(*.hobj)|*.hobj|tiff文件(*.tiff)|*.tiff|jpg文件(*.jpg)|*.jpg|jpeg文件(*.jpeg)|*.jpeg|png文件(*.png)|*.png|ras文件(*.ras)|*.ras|dxf文件(.dxf)|*.dxf|所有文件(*.*)|*.**";
                fileDialog.RestoreDirectory = false;
                fileDialog.FilterIndex = 0;
                fileDialog.ShowDialog();
                //this.sourceImage?.WriteImage("bmp", 0, fileDialog.FileName);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}

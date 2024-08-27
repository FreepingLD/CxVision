
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

    public partial class MatrixCalibrateCameraParamForm : Form
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
        //private userDrawMultipleRect2Class drawObject;
        private VisualizeView drawObject;
        private TreeViewWrapClass _treeViewWrapClass;
        private string programPath = ""; // 程序文件路径
        public MatrixCalibrateCameraParamForm()
        {
            InitializeComponent();
            this._acqSource = new AcqSource(SensorManage.CurrentCamSensor);
            calibrateCamera = new CameraCalibrateTool();
            this.drawObject = new VisualizeView(this.hWindowControl1);
            this._treeViewWrapClass = new TreeViewWrapClass(this.treeView1, this);
        }

        private void MatrixCalibrateCameraParamForm_Load(object sender, EventArgs e)
        {
            //SensorBase.ImageAcqComplete += new ImageAcqCompleteEventHandler(ImageAcqComplete_Event);
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
                this.像元初始宽domainUpDown.DataBindings.Add("Text", this.calibrateCamera, "PixWidth", true, DataSourceUpdateMode.OnPropertyChanged);
                this.像元初始高domainUpDown.DataBindings.Add("Text", this.calibrateCamera, "PixHeight", true, DataSourceUpdateMode.OnPropertyChanged);
                this.镜头理论焦距domainUpDown.DataBindings.Add("Text", this.calibrateCamera, "Focus", true, DataSourceUpdateMode.OnPropertyChanged);
                this.倾斜角度domainUpDown.DataBindings.Add("Text", this.calibrateCamera, "Tilt", true, DataSourceUpdateMode.OnPropertyChanged);
                this.旋转角度domainUpDown.DataBindings.Add("Text", this.calibrateCamera, "Rot", true, DataSourceUpdateMode.OnPropertyChanged);
                //////////////////////////
                this.图像宽度domainUpDown.DataBindings.Add("Text", this.calibrateCamera, "ImageWidth", true, DataSourceUpdateMode.OnPropertyChanged);
                this.图像高度domainUpDown.DataBindings.Add("Text", this.calibrateCamera, "ImageHeight", true, DataSourceUpdateMode.OnPropertyChanged);
                this.阈值textBox.DataBindings.Add("Text", this.calibrateCamera, "Threshold", true, DataSourceUpdateMode.OnPropertyChanged);
                this.计算方法comboBox.DataBindings.Add("Text", this.calibrateCamera, "Method", true, DataSourceUpdateMode.OnPropertyChanged);
                this.坐标系绕Z旋转textBox.DataBindings.Add("Text", this.calibrateCamera, "R_z", true, DataSourceUpdateMode.OnPropertyChanged);
                this.轴长度textBox.DataBindings.Add("Text", this.calibrateCamera, "Axis_length", true, DataSourceUpdateMode.OnPropertyChanged);
                /////////////
                this.标定类型comboBox.DataBindings.Add("Text", this.calibrateCamera, "CamCalibrateType", true, DataSourceUpdateMode.OnPropertyChanged);
                this.原点X偏移textBox.DataBindings.Add("Text", this.calibrateCamera, "X_offset", true, DataSourceUpdateMode.OnPropertyChanged);
                this.原点Y偏移textBox.DataBindings.Add("Text", this.calibrateCamera, "Y_offset", true, DataSourceUpdateMode.OnPropertyChanged);
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
            }
        }


        private void ImageAcqComplete_Event(ImageAcqCompleteEventArgs e)
        {
            try
            {
                double x, y, z;
                if (!isUpdata) return;
                if (this._acqSource.Sensor.ConfigParam.SensorName != e.CamName) return; // 不是当前相机的图像不刷新
                if (this._acqSource != null)
                {
                    this._acqSource.GetAxisPosition(enAxisName.X轴,  out x);
                    this._acqSource.GetAxisPosition(enAxisName.Y轴,  out y);
                    this._acqSource.GetAxisPosition(enAxisName.Z轴,  out z);
                    e.ImageData.Grab_X = x;
                    e.ImageData.Grab_Y = y;
                    e.ImageData.Grab_Z = z;
                    this.drawObject.BackImage = e.ImageData;
                    this.image = e.ImageData;
                }
                else
                {
                    this.drawObject.BackImage = e.ImageData;
                    this.image = e.ImageData;
                }
            }
            catch
            {
                MessageBox.Show("图像刷新失败");
            }
        }
        private void 采集源comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.采集源comboBox.SelectedItem == null) return;
            //this._acqSource.Sensor = (ISensor)this.采集源comboBox.SelectedItem;
        }
        private void CalibrateCameraParamForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                //SensorBase.ImageAcqComplete -= new ImageAcqCompleteEventHandler(ImageAcqComplete_Event);
                this._treeViewWrapClass?.Uinit();
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
                    this.倾斜角度domainUpDown.Enabled = false;
                    this.旋转角度domainUpDown.Enabled = false;
                    AddForm(this.相机内参splitContainer.Panel1, new DivisionParamForm());
                    break;
                //case "area_scan_polynomial":
                //    this.镜头理论焦距domainUpDown.Enabled = true;
                //    this.倾斜角度domainUpDown.Enabled = false;
                //    this.旋转角度domainUpDown.Enabled = false;
                //    AddForm(this.相机内参splitContainer.Panel1, new PolynomialParamForm());
                //    break;
                case "area_scan_telecentric_division":
                    this.镜头理论焦距domainUpDown.Enabled = false;
                    this.倾斜角度domainUpDown.Enabled = false;
                    this.旋转角度domainUpDown.Enabled = false;
                    AddForm(this.相机内参splitContainer.Panel1, new PolynomialParamForm());
                    break;
                //case "area_scan_telecentric_polynomial":
                //    this.镜头理论焦距domainUpDown.Enabled = false;
                //    this.倾斜角度domainUpDown.Enabled = false;
                //    this.旋转角度domainUpDown.Enabled = false;
                //    AddForm(this.相机内参splitContainer.Panel1, new PolynomialParamForm());
                //    break;
                //case "area_scan_tilt_division":
                //    this.镜头理论焦距domainUpDown.Enabled = true;
                //    this.倾斜角度domainUpDown.Enabled = true;
                //    this.旋转角度domainUpDown.Enabled = true;
                //    AddForm(this.相机内参splitContainer.Panel1, new TiltDivisionForm());
                //    break;
                //case "area_scan_tilt_polynomial":
                //    this.镜头理论焦距domainUpDown.Enabled = true;
                //    this.倾斜角度domainUpDown.Enabled = true;
                //    this.旋转角度domainUpDown.Enabled = true;
                //    AddForm(this.相机内参splitContainer.Panel1, new TiltPolynomialForm());
                //    break;
                //case "area_scan_telecentric_tilt_division":
                //    this.镜头理论焦距domainUpDown.Enabled = false;
                //    this.倾斜角度domainUpDown.Enabled = true;
                //    this.旋转角度domainUpDown.Enabled = true;
                //    AddForm(this.相机内参splitContainer.Panel1, new TiltDivisionForm());
                //    break;
                //case "area_scan_telecentric_tilt_polynomial":
                //    this.镜头理论焦距domainUpDown.Enabled = false;
                //    this.倾斜角度domainUpDown.Enabled = true;
                //    this.旋转角度domainUpDown.Enabled = true;
                //    AddForm(this.相机内参splitContainer.Panel1, new TiltPolynomialForm());
                //    break;
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
            禁止刷新checkBox.Checked = true;
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
                //double camSlant = (double)this._acqSource.Sensor.GetParam(enSensorParamType.Coom_相机角度);
                if (this.image == null) return;
                //HXLDCont hXLDCont = this.calibrateCamera.CalibrateCamera(this.image.Image, camSlant, out this.camPara, out this.camPose, out this.error, out this.calibrateFile);
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
                    //case enCameraModel.area_scan_polynomial:
                    //    AddForm(this.相机内参splitContainer.Panel1, new PolynomialParamForm(this.camPara));
                    //    UpdataCamPose();
                    //    break;
                    case enCameraModel.area_scan_telecentric_division:
                        AddForm(this.相机内参splitContainer.Panel1, new DivisionParamForm(this.camPara));
                        UpdataCamPose();
                        break;
                    //case enCameraModel.area_scan_telecentric_polynomial:
                    //    AddForm(this.相机内参splitContainer.Panel1, new PolynomialParamForm(this.camPara));
                    //    UpdataCamPose();
                    //    break;
                    //case enCameraModel.area_scan_telecentric_tilt_division:
                    //    AddForm(this.相机内参splitContainer.Panel1, new TiltDivisionForm(this.camPara));
                    //    UpdataCamPose();
                    //    break;
                    //case enCameraModel.area_scan_telecentric_tilt_polynomial:
                    //    AddForm(this.相机内参splitContainer.Panel1, new TiltPolynomialForm(this.camPara));
                    //    UpdataCamPose();
                    //    break;
                    //case enCameraModel.area_scan_tilt_division:
                    //    AddForm(this.相机内参splitContainer.Panel1, new TiltDivisionForm(this.camPara));
                    //    UpdataCamPose();
                    //    break;
                    //case enCameraModel.area_scan_tilt_polynomial:
                    //    AddForm(this.相机内参splitContainer.Panel1, new TiltPolynomialForm(this.camPara));
                    //    UpdataCamPose();
                    //    break;
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
        private void 加载button_Click_1(object sender, EventArgs e)
        {
            //string path = null;
            //HObject image;
            //try
            //{
            //    FileOperate fo = new FileOperate();
            //    path = fo.OpenImage();
            //    if (path != null && path.Length > 0)
            //    {
            //        HOperatorSet.ReadImage(out image, path);
            //        this.image = new ImageDataClass(new HImage(image), null, null); // 使用图片来标定，那么内外参应该为空
            //        DisplayImage(this.hWindowControl1.HalconWindow, this.image.Image);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.ToString());
            //}
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

        /// <summary>
        /// 标定相机角度
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 标定执行button_Click(object sender, EventArgs e)
        {
            try
            {
                double X1, Y1, X2, Y2;
                double step = 0;
                HTuple Rows = new HTuple();
                HTuple Cols = new HTuple();
                double.TryParse(this.X1_textBox.Text, out X1);
                double.TryParse(this.Y1_textBox.Text, out Y1);
                double.TryParse(this.X2_textBox.Text, out X2);
                double.TryParse(this.Y2_textBox.Text, out Y2);
                double.TryParse(this.步长textBox.Text, out step);
                IFunction _function;
                userPixPoint pixPoint;
                int count_x = (int)(Math.Abs(X2 - X1) / step);
                int count_y = (int)(Math.Abs(Y2 - Y1) / step);
                int count = count_x > count_y ? count_x : count_y;
                ///////////////////////    
                for (int ii = 0; ii < count; ii++)
                {
                    Thread.Sleep(100);
                    Application.DoEvents();
                    if (count_x > count_y)
                        this._acqSource.Card?.MoveSingleAxis(this._acqSource.CoordSysName,enAxisName.X轴, 50, X1 + step);
                    else
                        this._acqSource.Card?.MoveSingleAxis(this._acqSource.CoordSysName,enAxisName.Y轴, 50, Y1 + step);
                    // 执行一次定位
                    int length = (int)(this.treeView1.Invoke(new Func<object>(() => this.treeView1.Nodes.Count))); //程序条目   
                    for (int i = 0; i < length; i++) // 按顺序执行一次
                    {
                        if (!(this.treeView1.Nodes[i].Tag is IFunction)) continue;
                        this.treeView1.Invoke(new Action(() => this.treeView1.Focus()));
                        this.treeView1.Invoke(new Action(() => this.treeView1.SelectedNode = this.treeView1.Nodes[i]));
                        _function = (IFunction)this.treeView1.Nodes[i].Tag;
                        if (!_function.Execute(null).Succss) ;
                    }
                    _function = (IFunction)this.treeView1.Nodes[length - 1].Tag;
                    pixPoint = ((userWcsPoint)_function.GetPropertyValues("")).GetPixPoint(); // 最后一个定要是返回一个点的
                    Rows.Append(pixPoint.Row);
                    Cols.Append(pixPoint.Col);
                }
                /////////////////////////////
                HTuple RowBegin, ColBegin, RowEnd, ColEnd, Nr, Nc, Dist, phi;
                HXLDCont line = new HXLDCont(Rows, Cols);
                HTuple camPara = (HTuple)this._acqSource?.Sensor.GetParam(enSensorParamType.Coom_相机内参);
                HTuple camPose = (HTuple)this._acqSource?.Sensor.GetParam(enSensorParamType.Coom_相机外参);
                //////////////////////////////////////
                line.FitLineContourXld("tukey", -1, 0, 5, 2, out RowBegin, out ColBegin, out RowEnd, out ColEnd, out Nr, out Nc, out Dist); // 标定相机倾斜角时，只能用像素坐标，因相机还未标定
                HOperatorSet.LineOrientation(RowBegin, ColBegin, RowEnd, ColEnd, out phi);
                ///////////;
                this.camSlant = phi.TupleDeg().D;
                this.角度textBox.Text = this.camSlant.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void 记录坐标1button_Click(object sender, EventArgs e)
        {
            try
            {
                double x_pos, y_pos, z_pos;
                this._acqSource.GetAxisPosition(enAxisName.X轴, out x_pos);
                this._acqSource.GetAxisPosition(enAxisName.Y轴, out y_pos);
                this._acqSource.GetAxisPosition(enAxisName.Z轴,  out z_pos);
                this.X1_textBox.Text = x_pos.ToString();
                this.Y1_textBox.Text = y_pos.ToString();
                this.Z1_textBox.Text = z_pos.ToString();
            }
            catch
            {
                MessageBox.Show(new Exception().ToString());
            }
        }

        private void 记录坐标2button_Click(object sender, EventArgs e)
        {
            try
            {
                double x_pos, y_pos, z_pos;
                this._acqSource.GetAxisPosition(enAxisName.X轴,  out x_pos);
                this._acqSource.GetAxisPosition(enAxisName.Y轴,  out y_pos);
                this._acqSource.GetAxisPosition(enAxisName.Z轴,  out z_pos);
                this.X2_textBox.Text = x_pos.ToString();
                this.Y2_textBox.Text = y_pos.ToString();
                this.Z2_textBox.Text = z_pos.ToString();
            }
            catch
            {
                MessageBox.Show(new Exception().ToString());
            }
        }

        private void 保存参数button_Click(object sender, EventArgs e)
        {
            FileOperate fo = new FileOperate();
            try
            {
                this._acqSource.Sensor.CameraParam.CamSlant = this.camSlant;
                this._acqSource.Sensor.CameraParam.Save();
                MessageBox.Show("保存成功");
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
            }
        }

        private void 加载参数button_Click(object sender, EventArgs e)
        {
            try
            {
                this.camSlant = this._acqSource.Sensor.CameraParam.CamSlant;
                this.角度textBox.Text = this.camSlant.ToString();
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
            }
        }

        private void 禁止刷新checkBox_CheckedChanged(object sender, EventArgs e)
        {
            if (禁止刷新checkBox.Checked)
                this.isUpdata = false;
            else
            {
                this.drawObject.AttachPropertyData.Clear();
                this.isUpdata = true;
            }

        }

        private void 检测工具button_Click(object sender, EventArgs e)
        {
            ToolForm tool = new ToolForm(this._treeViewWrapClass);
            tool.Owner = this;
            tool.Show();
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
                    //this._treeViewWrapClass.Run(this.toolStripButton_Run, 1);
                    标定执行button_Click(null, null);
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
        private void DisplayImage(HWindow window, HImage image)
        {
            int width, height;
            image.GetImageSize(out width, out height);
            window.SetPart(0, 0, height - 1, width - 1);
            window.DispObj(image);
            if (this.calibrateCamera.ImageWidth != width || this.calibrateCamera.ImageHeight != height)
            {
                this.calibrateCamera.ImageWidth = width;
                this.calibrateCamera.ImageHeight = height;        
            }
        }



    }
}

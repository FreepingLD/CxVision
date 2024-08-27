
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

namespace FunctionBlock
{

    public partial class SpaceCalibrateCameraParamForm : Form
    {
        private AcqSource _acqSource;
        private CancellationTokenSource cts;
        private SpaceCalibrateCameraParam calibrateCamera;
        private ImageDataClass image = null;
        private HTuple camPara, error;
        private object monitor = new object();
        private string state = "";
        public SpaceCalibrateCameraParamForm()
        {
            calibrateCamera = new SpaceCalibrateCameraParam();
            InitializeComponent();
        }

        private void CalibrateCameraParamForm_Load(object sender, EventArgs e)
        {
            BindProperty();
        }




        private void BindProperty()
        {
            try
            {
                this.采集源comboBox.DataSource = AcqSourceManage.Instance.GetCamAcqSourceList();// GetCamSensor();
                this.采集源comboBox.DisplayMember = "Name";
                this.摄像机模型comboBox.DataSource = Enum.GetNames(typeof(enCameraModel));
                this.标定配置类型comboBox.DataSource = Enum.GetNames(typeof(enCalibrationSetupType));
                //////////////////
                this.标定配置类型comboBox.DataBindings.Add("SelectedItem", this.calibrateCamera, "CalibrationSetupType", true, DataSourceUpdateMode.OnPropertyChanged);
                this.标定板厚度domainUpDown.DataBindings.Add("Text", this.calibrateCamera, "CalibrateObjectThick", true, DataSourceUpdateMode.OnPropertyChanged);
                this.标定板描述文件textBox.DataBindings.Add("Text", this.calibrateCamera, "CalibObjDescr", true, DataSourceUpdateMode.OnPropertyChanged);
                this.摄像机模型comboBox.DataBindings.Add("Text", this.calibrateCamera, "CameraType", true, DataSourceUpdateMode.OnPropertyChanged);
                this.像元初始宽domainUpDown.DataBindings.Add("Text", this.calibrateCamera, "PixWidth", true, DataSourceUpdateMode.OnPropertyChanged);
                this.像元初始高domainUpDown.DataBindings.Add("Text", this.calibrateCamera, "PixHeight", true, DataSourceUpdateMode.OnPropertyChanged);
                this.镜头理论焦距domainUpDown.DataBindings.Add("Text", this.calibrateCamera, "Focus", true, DataSourceUpdateMode.OnPropertyChanged);
                this.倾斜角度domainUpDown.DataBindings.Add("Text", this.calibrateCamera, "Tilt", true, DataSourceUpdateMode.OnPropertyChanged);
                this.旋转角度domainUpDown.DataBindings.Add("Text", this.calibrateCamera, "Rot", true, DataSourceUpdateMode.OnPropertyChanged);
                //////////////////////////
                this.图像宽度domainUpDown.DataBindings.Add("Text", this.calibrateCamera, "ImageWidth", true, DataSourceUpdateMode.OnPropertyChanged);
                this.图像高度domainUpDown.DataBindings.Add("Text", this.calibrateCamera, "ImageHeight", true, DataSourceUpdateMode.OnPropertyChanged);
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
            }
        }
     

        private void 采集button_Click(object sender, EventArgs e)
        {
            this.calibrateCamera.ListImage.Add(this.image.Image);
            this.dataGridView1.Rows.Add("图像" + this.calibrateCamera.ListImage.Count.ToString(), state);
            this.hWindowControl1.HalconWindow.DispObj(this.image.Image);
        }
        private void 实时checkBox_CheckedChanged(object sender, EventArgs e)
        {
            ImageDataClass image =null;
            if (实时checkBox.Checked)
            {
                cts = new CancellationTokenSource();
                Task.Run(() =>
                {
                    while (true)
                    {
                        try
                        {
                            if (cts.IsCancellationRequested) break;
                            image = (ImageDataClass)_acqSource.AcqPointData()[0];
                            DisplayImage(this.hWindowControl1.HalconWindow,this.image.Image);
                            this.calibrateCamera.FindCalibObject(image.Image, this.hWindowControl1.HalconWindow);
                            this.image = image;
                            state = "找到校准对象";
                            this.Invoke(new Action(() => this.dataGridView1.Rows[this.calibrateCamera.ListImage.Count].SetValues("图像" + (this.calibrateCamera.ListImage.Count).ToString(), state)));
                        }
                        catch(Exception ee)
                        {
                            state = "未找到校准对象";
                            if (image !=null)
                                DisplayImage(this.hWindowControl1.HalconWindow, this.image.Image);
                            this.Invoke(new Action(() => this.dataGridView1.Rows[this.calibrateCamera.ListImage.Count].SetValues("图像" + (this.calibrateCamera.ListImage.Count).ToString(), state)));
                        }
                        Thread.Sleep(200);
                    }
                });
            }
            else
            {
                if (cts != null)
                    cts.Cancel();
                this.dataGridView1.Rows[this.calibrateCamera.ListImage.Count].SetValues("", "");
            }
        }
        private void DisplayImage(HWindow window, HImage image)
        {
            HTuple width, height;
            image.GetImageSize(out width, out height);
            window.SetPart(0, 0, height.D, width.D);
            window.DispObj(image);
            if (this.calibrateCamera.ImageWidth != width.D || this.calibrateCamera.ImageHeight != height.D)
            {
                this.calibrateCamera.ImageWidth = width.D;
                this.calibrateCamera.ImageHeight = height.D;
            }
        }
        private void 采集源comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            this._acqSource = (AcqSource)this.采集源comboBox.SelectedItem;
        }
        private void 加载button_Click(object sender, EventArgs e)
        {
            string[] path = null;
            HObject image;
            try
            {
                FolderBrowserDialog fold = new FolderBrowserDialog();
                fold.ShowDialog();
                if (fold.SelectedPath.Trim().Length > 0)
                    path = Directory.GetFiles(fold.SelectedPath);
                for (int i = 0; i < path.Length; i++)
                {
                    HOperatorSet.ReadImage(out image, path[i]);
                    this.calibrateCamera.ListImage.Add(new HImage(image));
                }
                for (int i = 0; i < this.calibrateCamera.ListImage.Count; i++)
                {
                    this.dataGridView1.Rows.Add("图像"+i.ToString(),"未找到校准对象");
                    // addControlToFlowLayoutPanel(this.flowLayoutPanel1, this.calibrateCamera.ListImage[i]);
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show(new Exception().ToString());
            }
        }

        private void 上一张button_Click(object sender, EventArgs e)
        {
            int index = 0;
            //int imageWidth, imageHeight;
            int.TryParse(this.当前图像textBox.Text, out index);
            if (this.calibrateCamera.ListImage.Count > 0)
            {
                if (index >= 0 && index < this.calibrateCamera.ListImage.Count)
                {
                    DisplayImage(this.hWindowControl1.HalconWindow, this.calibrateCamera.ListImage[index]);
                    //this.calibrateCamera.ListImage[index].GetImageSize(out imageWidth, out imageHeight);
                    //this.hWindowControl1.HalconWindow.SetPart(0, 0, imageHeight - 1, imageWidth - 1);
                    //this.hWindowControl1.HalconWindow.DispObj(this.calibrateCamera.ListImage[index]);
                    this.calibrateCamera.FindCalibObject(this.calibrateCamera.ListImage[index], this.hWindowControl1.HalconWindow);
                    index--;
                }
                else
                {
                    index = this.calibrateCamera.ListImage.Count - 1;
                    DisplayImage(this.hWindowControl1.HalconWindow, this.calibrateCamera.ListImage[index]);
                    //this.hWindowControl1.HalconWindow.DispObj(this.calibrateCamera.ListImage[index]);
                    this.calibrateCamera.FindCalibObject(this.calibrateCamera.ListImage[index], this.hWindowControl1.HalconWindow);
                }
                this.当前图像textBox.Text = index.ToString();
            }
        }
        private void 下一张button_Click(object sender, EventArgs e)
        {
            int index = 0;
            int imageWidth, imageHeight;
            int.TryParse(this.当前图像textBox.Text, out index);
            if (this.calibrateCamera.ListImage.Count > 0)
            {
                if (index >= 0 && index < this.calibrateCamera.ListImage.Count)
                {
                    DisplayImage(this.hWindowControl1.HalconWindow, this.calibrateCamera.ListImage[index]);
                    //this.calibrateCamera.ListImage[index].GetImageSize(out imageWidth, out imageHeight);
                    //this.hWindowControl1.HalconWindow.SetPart(0, 0, imageHeight - 1, imageWidth - 1);
                    //this.hWindowControl1.HalconWindow.DispObj(this.calibrateCamera.ListImage[index]);
                    this.calibrateCamera.FindCalibObject(this.calibrateCamera.ListImage[index], this.hWindowControl1.HalconWindow);
                    index++;
                }
                else
                {
                    index = 0;
                    DisplayImage(this.hWindowControl1.HalconWindow, this.calibrateCamera.ListImage[index]);
                    //this.hWindowControl1.HalconWindow.DispObj(this.calibrateCamera.ListImage[index]);
                    this.calibrateCamera.FindCalibObject(this.calibrateCamera.ListImage[index], this.hWindowControl1.HalconWindow);
                }
                this.当前图像textBox.Text = index.ToString();
            }
        }
        private void CalibrateCameraParamForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.calibrateCamera.ClearHandle();
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
                case "area_scan_polynomial":
                    this.镜头理论焦距domainUpDown.Enabled = true;
                    this.倾斜角度domainUpDown.Enabled = false;
                    this.旋转角度domainUpDown.Enabled = false;
                    AddForm(this.相机内参splitContainer.Panel1, new PolynomialParamForm());
                    break;
                case "area_scan_telecentric_division":
                    this.镜头理论焦距domainUpDown.Enabled = false;
                    this.倾斜角度domainUpDown.Enabled = false;
                    this.旋转角度domainUpDown.Enabled = false;
                    AddForm(this.相机内参splitContainer.Panel1, new DivisionParamForm());
                    break;
                case "area_scan_telecentric_polynomial":
                    this.镜头理论焦距domainUpDown.Enabled = false;
                    this.倾斜角度domainUpDown.Enabled = false;
                    this.旋转角度domainUpDown.Enabled = false;
                    AddForm(this.相机内参splitContainer.Panel1, new PolynomialParamForm());
                    break;
                case "area_scan_tilt_division":
                    this.镜头理论焦距domainUpDown.Enabled = true;
                    this.倾斜角度domainUpDown.Enabled = true;
                    this.旋转角度domainUpDown.Enabled = true;
                    AddForm(this.相机内参splitContainer.Panel1, new TiltDivisionForm());
                    break;
                case "area_scan_tilt_polynomial":
                    this.镜头理论焦距domainUpDown.Enabled = true;
                    this.倾斜角度domainUpDown.Enabled = true;
                    this.旋转角度domainUpDown.Enabled = true;
                    AddForm(this.相机内参splitContainer.Panel1, new TiltPolynomialForm());
                    break;
                case "area_scan_telecentric_tilt_division":
                    this.镜头理论焦距domainUpDown.Enabled = false;
                    this.倾斜角度domainUpDown.Enabled = true;
                    this.旋转角度domainUpDown.Enabled = true;
                    AddForm(this.相机内参splitContainer.Panel1, new TiltDivisionForm());
                    break;
                case "area_scan_telecentric_tilt_polynomial":
                    this.镜头理论焦距domainUpDown.Enabled = false;
                    this.倾斜角度domainUpDown.Enabled = true;
                    this.旋转角度domainUpDown.Enabled = true;
                    AddForm(this.相机内参splitContainer.Panel1, new TiltPolynomialForm());
                    break;
                default:
                    break;
            }
        }

        private void 移除所有button_Click(object sender, EventArgs e)
        {
            this.calibrateCamera.ListImage.Clear();
            this.dataGridView1.Rows.Clear();
        }

        private void 标定button_Click(object sender, EventArgs e)
        {
            this.calibrateCamera.CalibrateCamera(this.hWindowControl1.HalconWindow, out this.camPara, out this.error);
            if (error.D <= 0.1)
                this.标定状态textBox.Text = "标定结果良好";
            else
                this.标定状态textBox.Text = "标定结果误差较大";
            this.平均误差textBox.Text = error.D.ToString();
            switch (this.calibrateCamera.CameraType)
            {
                case enCameraModel.area_scan_division:
                    AddForm(this.相机内参splitContainer.Panel1, new DivisionParamForm(this.camPara));
                    break;
                case enCameraModel.area_scan_polynomial:
                    AddForm(this.相机内参splitContainer.Panel1, new PolynomialParamForm(this.camPara));
                    break;
                case enCameraModel.area_scan_telecentric_division:
                    AddForm(this.相机内参splitContainer.Panel1, new DivisionParamForm(this.camPara));
                    break;
                case enCameraModel.area_scan_telecentric_polynomial:
                    AddForm(this.相机内参splitContainer.Panel1, new PolynomialParamForm(this.camPara));
                    break;
                case enCameraModel.area_scan_telecentric_tilt_division:
                    AddForm(this.相机内参splitContainer.Panel1, new TiltDivisionForm(this.camPara));
                    break;
                case enCameraModel.area_scan_telecentric_tilt_polynomial:
                    AddForm(this.相机内参splitContainer.Panel1, new TiltPolynomialForm(this.camPara));
                    break;
                case enCameraModel.area_scan_tilt_division:
                    AddForm(this.相机内参splitContainer.Panel1, new TiltDivisionForm(this.camPara));
                    break;
                case enCameraModel.area_scan_tilt_polynomial:
                    AddForm(this.相机内参splitContainer.Panel1, new TiltPolynomialForm(this.camPara));
                    break;
                default:
                    break;
            }

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
        private void 文件路径button_Click(object sender, EventArgs e)
        {
            FileOperate fo = new FileOperate();
            string path = fo.OpenCalibObjDescr();
            try
            {
                this.标定板描述文件textBox.Text = path;
            }
            catch
            {
                MessageBox.Show(new Exception().ToString());
            }
        }

        private void 移除当前项button_Click(object sender, EventArgs e)
        {
            if (this.dataGridView1.CurrentRow != null)
            {
                int index = this.dataGridView1.CurrentRow.Index;
                this.dataGridView1.Rows.Remove(this.dataGridView1.CurrentRow);
                this.calibrateCamera.ListImage.RemoveAt(index);
            }
        }

        private void 保存所有button_Click(object sender, EventArgs e)
        {
            try
            {
                FolderBrowserDialog fold = new FolderBrowserDialog();
                fold.ShowDialog();
                string path = fold.SelectedPath;
                for (int i = 0; i < this.calibrateCamera.ListImage.Count; i++)
                {
                    HOperatorSet.WriteImage(this.calibrateCamera.ListImage[i], "bmp",0,path+"\\"+"图像"+i.ToString());
                }
            }
            catch(Exception ee)
            {
                MessageBox.Show(ee.ToString());
            }
        }

        private void 保存相机内参button_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.camPara != null )
                {
                    string path = Application.StartupPath + "\\" + "camPara" + "\\" + this._acqSource.Sensor.GetParam("传感器名称").ToString() + ".dat";
                    this._acqSource.Sensor.SetParam(enSensorParamType.Coom_相机外参, this.camPara);
                    HOperatorSet.WriteCamPar(this.camPara, path);
                    MessageBox.Show("保存成功");
                }
            }
            catch
            {
                MessageBox.Show(new Exception().ToString());
            }

        }


    }
}

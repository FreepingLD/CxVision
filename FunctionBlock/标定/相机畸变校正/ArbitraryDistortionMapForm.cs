
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

    public partial class ArbitraryDistortionMapForm : Form
    {
        //private AcqSource _acqSource;
        private CameraCalibrateTool calibrateCamera;
        private ImageDataClass image = null;
        private bool isUpdata = true;
        private VisualizeView drawObject;
        private userPixRectangle2 rect2;
        private HImage Map, MapImage, sourceImage;
        private CameraParam CamParam;
        public ArbitraryDistortionMapForm()
        {
            InitializeComponent();
            //this._acqSource = new AcqSource(SensorManage.CurrentCamSensor);
            calibrateCamera = new CameraCalibrateTool();
            this.drawObject = new VisualizeView(this.hWindowControl1);
        }
        public ArbitraryDistortionMapForm(CameraParam CamParam)
        {
            InitializeComponent();
            this.CamParam = CamParam;
            //this._acqSource = AcqSourceManage.GetAcqSource(camName); //new AcqSource(SensorManage.CurrentCamSensor);
            this.drawObject = new VisualizeView(this.hWindowControl1, false);
        }
        private void DistortionMapParamForm_Load(object sender, EventArgs e)
        {
            BindProperty();
        }

        private void BindProperty()
        {
            try
            {
                this.采集源textBox.Text = this.CamParam.SensorName; // SensorManage.CameraList; // GetCamSensor();
                this.comboBox1.SelectedIndex = 0;
                this.drawObject.GrayValueInfo += new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void DistortionMapParamForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                this.drawObject.GrayValueInfo -= new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
            }
            catch
            {

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

        private HTuple rows, cols;
        private int rowCount, colCount, threshold;
        private HalconLibrary ha = new HalconLibrary();

        private void 保存button_Click(object sender, EventArgs e)
        {
            try
            {
                this.CamParam.Map = this.Map;
            }
            catch (Exception ee)
            {
                MessageBox.Show("标定失败" + ee.ToString());
            }
        }

        private void 视图工具toolStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            string name = e.ClickedItem.Name;
            switch (name)
            {
                case "toolStripButton_Clear":
                    this.drawObject.ClearWindow();
                    this.toolStripButton_Translate.CheckState = CheckState.Unchecked;
                    break;
                case "toolStripButton_Select":
                    this.drawObject.Select();
                    this.toolStripButton_Translate.CheckState = CheckState.Unchecked;
                    break;
                case "toolStripButton_Translate":
                    this.drawObject.TranslateScaleImage();
                    this.toolStripButton_Translate.CheckState = CheckState.Checked;
                    break;
                case "toolStripButton_Auto":
                    this.drawObject.AutoImage();
                    this.toolStripButton_Translate.CheckState = CheckState.Unchecked;
                    break;
                default:
                    break;
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
                this.sourceImage?.WriteImage("bmp", 0, fileDialog.FileName);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void 保存映射Button_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog fileDialog = new SaveFileDialog();
                fileDialog.Filter = "bmp文件(*.bmp)|*.bmp|hobj文件(*.hobj)|*.hobj|tiff文件(*.tiff)|*.tiff|jpg文件(*.jpg)|*.jpg|jpeg文件(*.jpeg)|*.jpeg|png文件(*.png)|*.png|ras文件(*.ras)|*.ras|dxf文件(.dxf)|*.dxf|所有文件(*.*)|*.**";
                fileDialog.RestoreDirectory = false;
                fileDialog.FilterIndex = 2;
                fileDialog.ShowDialog();
                this.Map?.WriteImage("tiff", 0, fileDialog.FileName);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void 标定区域button_Click(object sender, EventArgs e)
        {
            try
            {
                this.drawObject?.AttachPropertyData.Clear();
                this.drawObject?.DrawingGraphicObject();
                this.drawObject?.DrawPixRect2OnWindow(enColor.red, out rect2);
                this.drawObject?.AttachPropertyData.Add(rect2);
                // this.drawObject?.DrawingGraphicObject();
                ////////////////////
                int.TryParse(this.行数domainUpDown.Text, out rowCount);
                int.TryParse(this.列数domainUpDown.Text, out colCount);
                int.TryParse(this.阈值textBox.Text, out threshold);
                HRegion hRegion = new HRegion();
                hRegion.GenRectangle2(this.rect2.Row, this.rect2.Col, this.rect2.Rad, this.rect2.Length1, this.rect2.Length2);
                ha.FindCircleMarkCoord(this.drawObject.BackImage.Image, hRegion, rowCount, colCount, threshold, out rows, out cols);
                this.drawObject?.AttachPropertyData.Clear();
                for (int i = 0; i < rows.Length; i++)
                {
                    this.drawObject?.AttachPropertyData.Add(new userPixPoint(rows[i].D, cols[i].D));
                }
                this.drawObject?.DrawingGraphicObject();
            }
            catch (Exception ee)
            {
                MessageBox.Show("标定失败" + ee.ToString());
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.comboBox1.SelectedItem == null) return;
                switch (this.comboBox1.SelectedItem.ToString())
                {
                    default:
                    case "源图像":
                        if (this.sourceImage != null)
                            this.drawObject.BackImage = new ImageDataClass(this.sourceImage);
                        break;
                    case "映射图像":
                        if (this.Map != null)
                            this.drawObject.BackImage = new ImageDataClass(this.Map);
                        break;
                    case "校正图像":
                        if (this.MapImage != null)
                            this.drawObject.BackImage = new ImageDataClass(this.MapImage);
                        break;
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show("标定失败" + ee.ToString());
            }
        }

        private void 标定button_Click_1(object sender, EventArgs e)
        {
            try
            {
                double cirDist;
                double.TryParse(this.圆心距textBox.Text, out cirDist);
                int gridSpace = (int)HMisc.DistancePp(rows[0].D, cols[0].D, rows[1].D, cols[1].D);  // 网格间距
                ////////////////////////////////////////////////
                int gridWidth = colCount;
                int imageWidth, imageHeight;
                this.drawObject.BackImage.Image.GetImageSize(out imageWidth, out imageHeight);
                this.Map = HMisc.GenArbitraryDistortionMap(gridSpace, rows, cols, gridWidth, imageWidth, imageHeight, "bilinear");
                this.MapImage = this.drawObject.BackImage.Image?.MapImage(this.Map);
                this.drawObject.BackImage = new ImageDataClass(this.MapImage);
                this.CamParam.PixScale = cirDist / gridSpace;
                //this._acqSource.Sensor.CameraParam.CaliParam.HomMat2D = new UserHomMat2D(cirDist / gridSpace, cirDist / gridSpace); // 这里不设置 矩阵标定
                ////////////////////////////////////////////////
            }
            catch (Exception ee)
            {
                MessageBox.Show("标定失败" + ee.ToString());
            }
        }


        private void 采集图像button_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.CamParam != null)
                {
                    Dictionary<enDataItem, object> listData = AcqSourceManage.Instance.GetAcqSource(this.CamParam.SensorName).AcqPointData();
                    if (listData == null || listData.Count == 0) return;
                    switch (listData[0].GetType().Name)
                    {
                        case "ImageDataClass":
                            this.drawObject.BackImage = (ImageDataClass)listData[enDataItem.Image];
                            this.sourceImage = this.drawObject.BackImage.Image;
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




    }
}

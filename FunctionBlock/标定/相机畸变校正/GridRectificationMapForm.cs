using Common;
using HalconDotNet;
using Sensor;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using View;

namespace FunctionBlock
{

    public partial class GridRectificationMapForm : Form
    {
        // private AcqSource _acqSource;
        private ImageDataClass image = null;
        private bool isUpdata = true;
        private VisualizeView drawObject;
        //private userPixRectangle2 rect2;
        private PixROI rect2;
        private HImage Map, MapImage, sourceImage, reduceImage;
        private GridRectificationParam _GridRectificationParam;
        private HTuple SaddleRows, SaddleCols;
        private HXLD ConnectingLines;
        private CameraParam CamParam;
        public GridRectificationMapForm()
        {
            InitializeComponent();
            // this._acqSource = new AcqSource(SensorManage.CurrentCamSensor);
            this.drawObject = new userDrawRect1ROI(this.hWindowControl1,false);
            this._GridRectificationParam = new GridRectificationParam();
        }
        public GridRectificationMapForm(CameraParam CamParam)
        {
            InitializeComponent();
            this.CamParam = CamParam;
            // this._acqSource = AcqSourceManage.GetAcqSource(CamParam.SensorName);
            this.drawObject = new userDrawRect1ROI(this.hWindowControl1, false);
            this._GridRectificationParam = new GridRectificationParam();
        }
        private void GridRectificationParamForm_Load(object sender, EventArgs e)
        {
            BindProperty();
            this.propertyGrid1.SelectedObject = this._GridRectificationParam;
        }

        private void BindProperty()
        {
            try
            {
                this.采集源textBox.Text = this.CamParam.SensorName; // 
                this.comboBox1.SelectedIndex = 0;
                this.drawObject.GrayValueInfo += new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void GridRectificationParamForm_FormClosing(object sender, FormClosingEventArgs e)
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
                this.MapImage.WriteImage("bmp", 0, fileDialog.FileName);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void 连接网格点Button_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.sourceImage == null) throw new ArgumentNullException("this.sourceImage");
                if (this.SaddleRows == null) throw new ArgumentNullException("this.SaddleRows");
                if (this.SaddleCols == null) throw new ArgumentNullException("this.SaddleCols");
                HRegion hRegion = new HRegion();
                if (this.rect2 == null)
                {
                    int width, height;
                    this.sourceImage.GetImageSize(out width, out height);
                    hRegion.GenRectangle2(height * 0.5, width * 0.5, 0, width * 0.5, height * 0.5);
                }
                else
                    hRegion = this.rect2.GetRegion();
                this.reduceImage = this.sourceImage.ReduceDomain(hRegion);
                /////////////////////////////////////////////////////////
                this.ConnectingLines = this.reduceImage.ConnectGridPoints(this.SaddleRows, this.SaddleCols, this._GridRectificationParam.ConnectSigma, this._GridRectificationParam.ConnectMaxDist);
                this.drawObject?.AttachPropertyData.Clear();
                this.drawObject?.AttachPropertyData.Add(ConnectingLines);
                this.drawObject?.DrawingGraphicObject();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void 提取网格点Button_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.sourceImage == null) throw new ArgumentNullException("this.sourceImage");
                HRegion hRegion = new HRegion();
                if (this.rect2 == null)
                {
                    int width, height;
                    this.sourceImage.GetImageSize(out width, out height);
                    hRegion.GenRectangle2(height * 0.5, width * 0.5, 0, width * 0.5, height * 0.5);
                }
                else
                    hRegion = this.rect2.GetRegion();
                this.reduceImage = this.sourceImage.ReduceDomain(hRegion);
                this.reduceImage.SaddlePointsSubPix(this._GridRectificationParam.SaddleFilter.ToString(),
                                                    this._GridRectificationParam.SaddleSigma,
                                                    this._GridRectificationParam.SaddleThreshold,
                                                    out SaddleRows, out SaddleCols);
                ///////////////////////////////////////////////////
                this.drawObject?.AttachPropertyData.Clear();
                this.drawObject?.AttachPropertyData.Add(this.rect2?.GetXLD());
                for (int i = 0; i < SaddleRows.Length; i++)
                {
                    this.drawObject?.AttachPropertyData.Add(new userPixPoint(SaddleRows[i].D, SaddleCols[i].D));
                }
                this.drawObject?.DrawingGraphicObject();
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
                //this.drawObject?.DrawPixRect2OnWindow(enColor.red, out rect2);
                this.drawObject.SetParam(null);
                this.drawObject?.DrawPixRoiShapeOnWindow(enColor.red, out rect2);
                //this.drawObject?.AttachPropertyData.Add(rect2);
                ////////////////////
                //HRegion hRegion = new HRegion();
                //hRegion.GenRectangle2(this.rect2.Row, this.rect2.Col, this.rect2.Rad, this.rect2.Length1, this.rect2.Length2);
                this.drawObject?.AttachPropertyData.Add(rect2?.GetXLD());
                this.drawObject?.DrawingGraphicObject();
            }
            catch (Exception ex)
            {
                MessageBox.Show("标定失败" + ex.ToString());
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
                        this.drawObject.AttachPropertyData.Clear();
                        if (this.sourceImage != null)
                            this.drawObject.BackImage = new ImageDataClass(this.sourceImage);
                        break;
                    case "映射图像":
                        this.drawObject.AttachPropertyData.Clear();
                        if (this.Map != null)
                            this.drawObject.BackImage = new ImageDataClass(this.Map);
                        break;
                    case "校正图像":
                        this.drawObject.AttachPropertyData.Clear();
                        if (this.MapImage != null)
                            this.drawObject.BackImage = new ImageDataClass(this.MapImage);
                        break;
                    case "网格点":
                        this.drawObject.AttachPropertyData.Clear();
                        if (this.SaddleRows == null || this.SaddleCols == null) throw new ArgumentNullException("this.SaddleRows或this.SaddleCols");
                        for (int i = 0; i < this.SaddleRows.Length; i++)
                            this.drawObject.AttachPropertyData.Add(new userPixPoint(this.SaddleRows[i].D, this.SaddleCols[i].D));
                        this.drawObject.DrawingGraphicObject();
                        break;
                    case "网格线":
                        this.drawObject.AttachPropertyData.Clear();
                        if (this.ConnectingLines == null) throw new ArgumentNullException("this.SaddleRows或this.SaddleCols");
                        this.drawObject.AttachPropertyData.Add(this.ConnectingLines);
                        this.drawObject.DrawingGraphicObject();
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
                HXLD meshes;
                this.Map = this.sourceImage.GenGridRectificationMap(this.ConnectingLines, out meshes,
                                                         this._GridRectificationParam.GridSpacing,
                                                         this._GridRectificationParam.Rotation,
                                                         this.SaddleRows, this.SaddleCols,
                                                         this._GridRectificationParam.MapType.ToString());
                /////////////////////////////////////////////////////////////////////
                this.drawObject.AttachPropertyData.Clear();
                this.MapImage = this.sourceImage?.MapImage(this.Map);
                //int width, height;
                //this.sourceImage.GetImageSize(out width, out height);
                this.drawObject.BackImage = new ImageDataClass(this.MapImage);
                ////////////////////////////////////////////////
            }
            catch (Exception ex)
            {
                MessageBox.Show("标定失败" + ex.ToString());
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

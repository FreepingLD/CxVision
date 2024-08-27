
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

    public partial class MosaicParamForm : Form
    {
        private AcqSource _acqSource;
        private VisualizeView drawObject;
        private userPixRectangle2 rect2;
        private HImage MapImage, sourceImage, reduceImage;
        private MosaicParam _MosaicParam;
        private HTuple SaddleRows, SaddleCols;
        private BindingSource bs = new BindingSource();
        private List<HImage> listImage = new List<HImage>();


        public MosaicParamForm()
        {
            InitializeComponent();
            this._acqSource = new AcqSource(SensorManage.CurrentCamSensor);
            this.drawObject = new VisualizeView(this.hWindowControl1, true);
            this._MosaicParam = new MosaicParam();
        }
        public MosaicParamForm(string camName)
        {
            InitializeComponent();
            this._acqSource = AcqSourceManage.Instance.GetAcqSource(camName);
            this.drawObject = new VisualizeView(this.hWindowControl1, false);
            this._MosaicParam = new MosaicParam();
        }
        private void GridRectificationParamForm_Load(object sender, EventArgs e)
        {
            BindProperty();
            this.propertyGrid1.SelectedObject = this._MosaicParam;
        }

        private void BindProperty()
        {
            try
            {
                this.采集源textBox.Text = this._acqSource.Name; // 
                this.drawObject.GrayValueInfo += new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                ///////////
                this.bindingNavigator1.BindingSource = this.bs;
                this.bs.DataSourceChanged += new EventHandler(this.bindingNavigatorDataSourceChanged);
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
                this.bs.DataSourceChanged -= new EventHandler(this.bindingNavigatorDataSourceChanged);
            }
            catch
            {

            }
        }


        private void bindingNavigatorDataSourceChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.bs.Current != null)
                {
                    this.drawObject.BackImage?.Dispose();
                    this.drawObject.BackImage = new ImageDataClass(new HImage(this.bs.Current.ToString()));
                }
                this.PathLabel.Text = this.bs.Current.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void bindingNavigatorMoveNextItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.bs.Current != null)
                {
                    this.drawObject.BackImage?.Dispose();
                    this.drawObject.BackImage = new ImageDataClass(new HImage(this.bs.Current.ToString()));
                }
                this.PathLabel.Text = this.bs.Current.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void bindingNavigatorMovePreviousItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.bs.Current != null)
                {
                    this.drawObject.BackImage?.Dispose();
                    this.drawObject.BackImage = new ImageDataClass(new HImage(this.bs.Current.ToString()));
                }
                this.PathLabel.Text = this.bs.Current.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void MoveFirstItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.bs.Current != null)
                {
                    this.drawObject.BackImage?.Dispose();
                    this.drawObject.BackImage = new ImageDataClass(new HImage(this.bs.Current.ToString()));
                }
                this.PathLabel.Text = this.bs.Current.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void bindingNavigatorMoveLastItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.bs.Current != null)
                {
                    this.drawObject.BackImage?.Dispose();
                    this.drawObject.BackImage = new ImageDataClass(new HImage(this.bs.Current.ToString()));
                }
                this.PathLabel.Text = this.bs.Current.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
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
                //this._acqSource.Sensor.CameraParam.Map = this.Map;
            }
            catch (Exception ee)
            {
                MessageBox.Show("标定失败" + ee.ToString());
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
                for (int i = 0; i < this.listImage.Count; i++)
                {
                    this.MapImage.WriteImage("bmp", 0, new FileInfo(fileDialog.FileName).DirectoryName + (i + 1).ToString());
                }
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
                hRegion.GenRectangle2(this.rect2.Row, this.rect2.Col, this.rect2.Rad, this.rect2.Length1, this.rect2.Length2);
                this.reduceImage = this.sourceImage.ReduceDomain(hRegion);
                this.reduceImage.SaddlePointsSubPix(this._MosaicParam.SaddleFilter.ToString(),
                                                    this._MosaicParam.SaddleSigma,
                                                    this._MosaicParam.SaddleThreshold,
                                                    out SaddleRows, out SaddleCols);
                ///////////////////////////////////////////////////
                this.drawObject?.AttachPropertyData.Clear();
                this.drawObject?.AttachPropertyData.Add(this.rect2);
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

        private void 清空图像集Button_Click(object sender, EventArgs e)
        {
            try
            {
                this.listImage.Clear();
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
                ////////////////////
                HRegion hRegion = new HRegion();
                hRegion.GenRectangle2(this.rect2.Row, this.rect2.Col, this.rect2.Rad, this.rect2.Length1, this.rect2.Length2);
                this.drawObject?.AttachPropertyData.Add(rect2);
                this.drawObject?.DrawingGraphicObject();
            }
            catch (Exception ex)
            {
                MessageBox.Show("标定失败" + ex.ToString());
            }
        }

        private void 显示拼接图Button_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.listImage.Count == 0) throw new ArgumentException("图像数量为0，不能拼接");
                if (this.listImage.Count < (this._MosaicParam.ColCount * this._MosaicParam.RowCount)) throw new ArgumentException("图像数量小于行列的乖积，不能拼接");
                int imageWidth, imageHeight;
                listImage.Last().GetImageSize(out imageWidth, out imageHeight);
                int totalImageWidth = this._MosaicParam.ColCount * imageWidth;
                int totalImageHeight = this._MosaicParam.RowCount * imageHeight;
                HTuple offsetRow = new HTuple();
                HTuple offsetCol = new HTuple();
                HTuple row1 = new HTuple();
                HTuple col1 = new HTuple();
                HTuple row2 = new HTuple();
                HTuple col2 = new HTuple();
                //////////////////////////////////////////////////
                HImage grayImages = new HImage();
                grayImages.GenEmptyObj();
                for (int i = 0; i < this._MosaicParam.RowCount; i++)
                {
                    for (int j = 0; j < this._MosaicParam.ColCount; j++)
                    {
                        grayImages = grayImages.ConcatObj(listImage[i * this._MosaicParam.ColCount + j]);
                        offsetCol.Append(imageWidth * j); //Math.Round
                        offsetRow.Append(imageHeight * i); //Math.Round
                        row1.Append(-1);
                        col1.Append(-1);
                        row2.Append(-1);
                        col2.Append(-1);
                    }
                }
                ///////////////////////////////////
                if (totalImageWidth > 30000)
                    totalImageWidth = 30000;
                if (totalImageHeight > 30000)
                    totalImageHeight = 30000;
                HImage tiledImagesGray = grayImages.TileImagesOffset(offsetRow, offsetCol, row1, col1, row2, col2, totalImageWidth, totalImageHeight);
                this.drawObject.BackImage = new ImageDataClass(tiledImagesGray);
            }
            catch (Exception ex)
            {
                MessageBox.Show("标定失败" + ex.ToString());
            }
        }

        private void 标定button_Click_1(object sender, EventArgs e)
        {
            try
            {
                this.drawObject.AttachPropertyData.Clear();
                this.显示拼接图Button_Click(null, null);
                /////////////////////////////////////////
                HImage images = new HImage();
                images.GenEmptyObj();
                foreach (var item in this.listImage)
                {
                    images = images.ConcatObj(item);
                }
                //We need variables to accumulate the projective transformation matrices,
                //the coordinates of the matched points, and the number of matched
                //points for each image pair.
                int count = images.CountObj();
                int[] From, To;
                HTuple matchF_Rows1 = new HTuple();
                HTuple matchF_Cols1 = new HTuple();
                HTuple matchT_Rows2 = new HTuple();
                HTuple matchT_Cols2 = new HTuple();
                HTuple NumCorrespondences = new HTuple();
                HMatrix hMatrix = this._MosaicParam.GenMapIndex(out From, out To); // 生成图像映射索引
                int F_index, T_index;
                HImage ImageF, ImageT;
                HTuple RowFAll, ColFAll, RowTAll, ColTAll, Points1, Points2;
                int imageWidth, imageHeight;
                images.SelectObj(1).GetImageSize(out imageWidth, out imageHeight);
                int FShiftR = 0, FShiftC = 0, TShiftR = 0, TShiftC = 0, RowIndex = 0, ColIndex = 0;
                HHomMat2D[] ProjMatrices = new HHomMat2D[From.Length];
                for (int i = 0; i < From.Length; i++)
                {
                    F_index = From[i];
                    T_index = To[i];
                    ImageF = images.SelectObj(F_index);
                    ImageT = images.SelectObj(T_index);
                    /// 提取兴趣点
                    ImageF.PointsHarris(1, 3, 0.04, 0.0, out RowFAll, out ColFAll);
                    ImageT.PointsHarris(1, 3, 0.04, 0.0, out RowTAll, out ColTAll);
                    //////////////
                    this._MosaicParam.IndexOf(hMatrix, F_index, out RowIndex, out ColIndex);
                    FShiftR = imageHeight * RowIndex;
                    FShiftC = ColIndex * imageWidth;
                    ////////////////////
                    this._MosaicParam.IndexOf(hMatrix, T_index, out RowIndex, out ColIndex);
                    TShiftR = imageHeight * RowIndex;
                    TShiftC = ColIndex * imageWidth;
                    ////////////////////////
                    this._MosaicParam.RowMove = (int)((FShiftR - TShiftR) / 3);
                    this._MosaicParam.ColMove = (int)((FShiftC - TShiftC) / 2);
                    //////////////////////////
                    HHomMat2D ProjMatrix = ImageF.ProjMatchPointsRansac(ImageT, RowFAll, ColFAll, RowTAll, ColTAll,
                          this._MosaicParam.GrayMatchMethod.ToString(), this._MosaicParam.MaskSize,
                          this._MosaicParam.RowMove, this._MosaicParam.ColMove, this._MosaicParam.RowTolerance,
                          this._MosaicParam.ColTolerance, this._MosaicParam.Rotation, this._MosaicParam.MatchThreshold,
                          this._MosaicParam.EstimationMethod.ToString(), this._MosaicParam.DistanceThreshold, this._MosaicParam.RandSeed,
                          out Points1, out Points2);
                    /////////////////////////////////////
                    ProjMatrices[i] = ProjMatrix;
                    matchF_Rows1 = matchF_Rows1.TupleConcat(RowFAll.TupleSelect(Points1)); //;= [Rows1, subset(RowFAll, Points1)]
                    matchF_Cols1 = matchF_Cols1.TupleConcat(ColFAll.TupleSelect(Points1));//[Cols1, subset(ColFAll, Points1)]
                    matchT_Rows2 = matchT_Rows2.TupleConcat(RowTAll.TupleSelect(Points2));//[Rows2, subset(RowTAll, Points2)]
                    matchT_Cols2 = matchT_Cols2.TupleConcat(ColTAll.TupleSelect(Points2));//[Cols2, subset(ColTAll, Points2)]
                    NumCorrespondences = NumCorrespondences.TupleConcat(Points1.Length);//[NumCorrespondences,| Points1 |]

                    HXLDCont PointsF = new HXLDCont();
                    HXLDCont PointsT = new HXLDCont();
                    PointsF.GenCrossContourXld(RowFAll + FShiftR, ColFAll + FShiftC, 6,  0.78);
                    PointsT.GenCrossContourXld(RowTAll + TShiftR, ColTAll + TShiftC, 6, 0.78);
                    ///////////////
                    HTuple RowF = RowFAll.TupleSelect(Points1) + FShiftR;
                    HTuple ColF = ColFAll.TupleSelect(Points1) + FShiftC;
                    HTuple RowT = RowTAll.TupleSelect(Points2) + TShiftR;
                    HTuple ColT = ColTAll.TupleSelect(Points2) + TShiftC;

                    HXLDCont Matches = new HXLDCont();
                    Matches.GenEmptyObj();
                    for (int ii = 0; ii < RowF.Length; ii++)
                    {
                        Matches = Matches.ConcatObj(new HXLDCont(new HTuple(RowF[ii].D, RowT[ii].D), new HTuple(ColF[ii].D, ColT[ii].D)));
                    }
                    this.drawObject.AttachPropertyData.Add(PointsF);
                    this.drawObject.AttachPropertyData.Add(PointsT);
                    this.drawObject.AttachPropertyData.Add(Matches);
                    this.drawObject.DrawingGraphicObject();
                    Thread.Sleep(100);
                }
                /// 执行拼接
                HTuple Rows, Cols, Error;
                HHomMat2D [] hHomMat2Ds = HHomMat2D.BundleAdjustMosaic(this._MosaicParam.RowCount * this._MosaicParam.ColCount, 1, From, To, ProjMatrices,
                    matchF_Rows1, matchF_Cols1, matchT_Rows2, matchT_Cols2, NumCorrespondences, "projective", out Rows, out Cols, out Error);
                HTuple stackingOrder = this._MosaicParam.InvertMatrixRow(hMatrix); // 只能是整数
                string transformDomain = "false";
                HHomMat2D transMat2D;
                HImage mosicImage =  images.GenBundleAdjustedMosaic(hHomMat2Ds, stackingOrder, transformDomain, out transMat2D);
                this.drawObject.AttachPropertyData.Clear();
                this.drawObject.BackImage = new ImageDataClass(mosicImage);

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
                if (this._acqSource != null)
                {
                    Dictionary<enDataItem, object> listData = this._acqSource.AcqPointData();
                    if (listData == null || listData.Count == 0) return;
                    switch (listData[0].GetType().Name)
                    {
                        case "ImageDataClass":
                            this.drawObject.BackImage = (ImageDataClass)listData[enDataItem.Image];
                            //this.sourceImage = this.drawObject.BackImage.Image;
                            this.listImage.Add(this.drawObject.BackImage.Image);
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
                FolderBrowserDialog fold = new FolderBrowserDialog();
                fold.ShowDialog();
                if (fold.SelectedPath == null || fold.SelectedPath.Trim().Length == 0) return;
                string[] path = Directory.GetFiles(fold.SelectedPath);
                this.bs.DataSource = path;
                this.listImage.Clear();
                foreach (var item in path)
                {
                    string extenName = new FileInfo(item).Extension;
                    if (extenName == ".bmp" || extenName == ".png" || extenName == ".jpeg")
                        this.listImage.Add(new HImage(item));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }




    }
}


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
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using View;

namespace FunctionBlock
{

    public partial class FaceSensorCalibrateForm : Form
    {
        private Form form;
        private ISensor _sensor;
        private FunctionBlock.AcqSource _laserAcqSource;
        //private double[] laserParam;
        private userWcsPose laserPoseParam;
        private VisualizeView drawObject;
        private HTuple camSlant;
        private HPose planePose;
        public FaceSensorCalibrateForm()
        {
            InitializeComponent();
            drawObject = new VisualizeView(this.hWindowControl1, true);
        }
        private void FaceSensorCalibrateForm_Load(object sender, EventArgs e)
        {
            BindProperty();
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

        private void BindProperty()
        {
            try
            {
                this.激光采集源comboBox.DataSource = FunctionBlock.AcqSourceManage.Instance.LaserAcqSourceList();
                this.激光采集源comboBox.DisplayMember = "Name";
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
            }
        }


        private void 激光采集源comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.激光采集源comboBox.SelectedItem == null) return;
            this._laserAcqSource = (FunctionBlock.AcqSource)this.激光采集源comboBox.SelectedItem;
        }

        private void 保存button_Click(object sender, EventArgs e)
        {
            try
            {
                //FileOperate fo = new FileOperate();
                string path = Application.StartupPath + "\\" + "激光标定参数" + "\\" + this._laserAcqSource.Sensor.Name;
                this._laserAcqSource.Sensor.LaserParam.LaserCalibrationParam.LaserPose = laserPoseParam;
                this._laserAcqSource.Sensor.LaserParam.LaserCalibrationParam.Save(path);
                // fo.WriteTxt(path, this.laserParam, false);
                MessageBox.Show("保存成功");
            }
            catch
            {
                MessageBox.Show(new Exception().ToString());
            }
        }

        private void 标定button_Click(object sender, EventArgs e)
        {
            try
            {
                if (this._laserAcqSource == null)
                {
                    throw new ArgumentNullException("this._laserAcqSource.Sensor");
                }
                if (this._laserAcqSource.Sensor == null)
                {
                    throw new ArgumentNullException("this._laserAcqSource.Sensor");
                }
                this.Cursor = Cursors.WaitCursor;
                HPose pose;
                Dictionary<enDataItem, object> list = this._laserAcqSource.AcqPointData();
                double[] laserDist1 = (double[])list[enDataItem.Dist1];
                double[] encoder_X = (double[])list[enDataItem.X];
                double[] encoder_Y = (double[])list[enDataItem.Y];
                //////////////////////
                HObjectModel3D hObjectModel3DRef = new HObjectModel3D(encoder_X, encoder_Y, laserDist1);
                HalconLibrary ha = new HalconLibrary();
                ha.GetPlaneObjectModel3DPose(hObjectModel3DRef, out pose);
                this._laserAcqSource.Sensor.LaserParam.LaserCalibrationParam.LaserType = "面激光";
                this.laserPoseParam = new userWcsPose(pose);

                this.drawObject.PointCloudModel3D = new PointCloudData(hObjectModel3DRef);
                ///////////////////////////////////
                this.激光dataGridView.Rows.Clear();
                for (int i = 0; i < 10; i++)
                {
                    switch (i)
                    {
                        case 0:
                            this.激光dataGridView.Rows.Add(this.laserPoseParam.Tx.ToString());
                            //this.激光dataGridView.Rows[i].HeaderCell.Value = "X平移";
                            break;
                        case 1:
                            this.激光dataGridView.Rows.Add(this.laserPoseParam.Ty.ToString());
                            //this.激光dataGridView.Rows[i].HeaderCell.Value = "Y平移";
                            break;
                        case 2:
                            this.激光dataGridView.Rows.Add(this.laserPoseParam.Tz.ToString());
                            //this.激光dataGridView.Rows[i].HeaderCell.Value = "Z平移";
                            break;
                        case 3:
                            this.激光dataGridView.Rows.Add(this.laserPoseParam.Rx.ToString());
                            //this.激光dataGridView.Rows[i].HeaderCell.Value = "X旋转";
                            break;
                        case 4:
                            this.激光dataGridView.Rows.Add(this.laserPoseParam.Ry.ToString());
                            //this.激光dataGridView.Rows[i].HeaderCell.Value = "Y旋转";
                            break;
                        case 5:
                            this.激光dataGridView.Rows.Add(this.laserPoseParam.Rz.ToString());
                            //this.激光dataGridView.Rows[i].HeaderCell.Value = "Z旋转";
                            break;
                        case 6:
                            this.激光dataGridView.Rows.Add(this.laserPoseParam.ToString());
                            //this.激光dataGridView.Rows[i].HeaderCell.Value = "参考平面";
                            break;
                        case 7:
                            this.激光dataGridView.Rows.Add(this._laserAcqSource.Sensor.LaserParam.LaserCalibrationParam.LaserType);
                            //this.激光dataGridView.Rows[i].HeaderCell.Value = "激光类型";
                            break;
                    }
                }
                this.Cursor = Cursors.Default;
            }
            catch (Exception e1)
            {
                MessageBox.Show(e1.ToString());
            }
        }

        private void 加载参数button_Click(object sender, EventArgs e)
        {
            try
            {
                //FileOperate fo = new FileOperate();
                //string path = Application.StartupPath + "\\" + "LaserCalibrationParam" + "\\" + this._laserAcqSource.Sensor.Name + ".txt";
                //fo.ReadTxt(path, out this.laserParam);
                if (this._laserAcqSource.Sensor == null)
                {
                    throw new ArgumentNullException("this._laserAcqSource.Sensor");
                }
                this.laserPoseParam = this._laserAcqSource.Sensor.LaserParam.LaserCalibrationParam.LaserPose;
                ///////////////////////////////////
                this.激光dataGridView.Rows.Clear();
                for (int i = 0; i < 10; i++)
                {
                    switch (i)
                    {
                        case 0:
                            this.激光dataGridView.Rows.Add(this.laserPoseParam.Tx.ToString());
                            this.激光dataGridView.Rows[i].HeaderCell.Value = "X平移";
                            break;
                        case 1:
                            this.激光dataGridView.Rows.Add(this.laserPoseParam.Ty.ToString());
                            this.激光dataGridView.Rows[i].HeaderCell.Value = "Y平移";
                            break;
                        case 2:
                            this.激光dataGridView.Rows.Add(this.laserPoseParam.Tz.ToString());
                            this.激光dataGridView.Rows[i].HeaderCell.Value = "Z平移";
                            break;
                        case 3:
                            this.激光dataGridView.Rows.Add(this.laserPoseParam.Rx.ToString());
                            this.激光dataGridView.Rows[i].HeaderCell.Value = "X旋转";
                            break;
                        case 4:
                            this.激光dataGridView.Rows.Add(this.laserPoseParam.Ry.ToString());
                            this.激光dataGridView.Rows[i].HeaderCell.Value = "Y旋转";
                            break;
                        case 5:
                            this.激光dataGridView.Rows.Add(this.laserPoseParam.Rz.ToString());
                            this.激光dataGridView.Rows[i].HeaderCell.Value = "Z旋转";
                            break;
                        case 6:
                            this.激光dataGridView.Rows.Add(this.laserPoseParam.ToString());
                            this.激光dataGridView.Rows[i].HeaderCell.Value = "参考平面";
                            break;
                        case 7:
                            this.激光dataGridView.Rows.Add(this._laserAcqSource.Sensor.LaserParam.LaserCalibrationParam.LaserType);
                            this.激光dataGridView.Rows[i].HeaderCell.Value = "激光类型";
                            break;
                    }
                }
            }
            catch (Exception e1)
            {
                MessageBox.Show(e1.ToString());
            }
        }

        private void LaserCameraCalibrateForm_FormClosing(object sender, FormClosingEventArgs e)
        {

        }

        // 归一化处理，先获取 像素值 max，min
        void GetMaxMinPixel(double[] pGray16, int size, ref ushort max, ref ushort min)
        {
            ushort max1 = 0;
            ushort min1 = 65535;

            for (int i = 0; i < size; i++)
            {
                if (max1 < (ushort)pGray16[i])
                    max1 = (ushort)pGray16[i];

                if (min1 > (ushort)pGray16[i])
                    min1 = (ushort)pGray16[i];
            }

            max = max1;
            min = min1;
        }
        // 16 bit to 8bit
        void Gray16To8(double[] pGray16, byte[] pGray8, int width, int height, bool rotate)
        {
            ushort max = 0;
            ushort min = 0;
            int size = width * height;
            GetMaxMinPixel(pGray16, size, ref max, ref min);
            for (var i = 0; i < size; i++)
            {
                double pix = ((double)(pGray16[i] - min)) / ((double)(max - min));
                if (rotate)
                    pGray8[size - 1 - i] = (byte)(pix * (double)255);
                else
                    pGray8[i] = (byte)(pix * (double)255);
            }
        }

        private void 标定角度button_Click(object sender, EventArgs e)
        {
            try
            {
                IntPtr imgPtr = IntPtr.Zero;
                HTuple RowBegin, ColBegin, RowEnd, ColEnd, Nr, Nc, Dist, phi;
                double Threshold = 128, row, col;
                double start_pos, end_pos, X1, X2;
                double step = 0;
                HTuple Rows = new HTuple();
                HTuple Cols = new HTuple();
                HTuple camPara;
                HTuple camPose;
                double.TryParse(this.X1_textBox.Text, out X1);
                double.TryParse(this.X2_textBox.Text, out X2);
                double.TryParse(this.步长textBox.Text, out step);
                double.TryParse(this.阈值textBox.Text, out Threshold);
                double resolution_x = double.Parse(this.X分辨率textBox.Text);
                double resolution_y = double.Parse(this.Y分辨率textBox.Text);
                start_pos = Math.Max(X1, X2);
                end_pos = Math.Min(X1, X2);
                HalconLibrary ha = new HalconLibrary();
                ImageDataClass imageData;
                if (this._laserAcqSource.Sensor == null)
                {
                    throw new ArgumentNullException("this._laserAcqSource.Sensor");
                }
                Task.Run(() =>
                {
                    while (true)
                    {
                        MotionCardManage.CurrentCard.MoveSingleAxis(MotionCardManage.CurrentCoordSys, enAxisName.X轴, 50, start_pos); // 这个地方要重写
                        Dictionary<enDataItem, object> list = this._laserAcqSource.AcqPointData();
                        //double[] laserDist2 = (double[])list[1];
                        double[] laserDist1 = (double[])list[enDataItem.Dist1];
                        double[] encoder_X = (double[])list[enDataItem.X];
                        double[] encoder_Y = (double[])list[enDataItem.Y];
                        //byte[] pGray8;
                        //int width, height;
                        ha.TransformObject3DToRealImage(new HObjectModel3D(encoder_X, encoder_Y, laserDist1), resolution_x, resolution_y,out imageData);
                        //width = (int)this._laserAcqSource.Sensor.DataWidth;
                        //height = (int)this._laserAcqSource.Sensor.DataHeight;
                        //camPara = new HTuple(0.0, 0.0, 0.0035, 0.0035, width * 0.5, height * 0.5, width, height);
                        //camPose = new HTuple(0, 0, 100, 0, 0, 0, 0);
                        //pGray8 = new byte[width * height];  // 将点去转换成图像
                        //Gray16To8(laserDist2, pGray8, width, height, true);
                        //imgPtr = Marshal.AllocHGlobal(width * height);
                        //Marshal.Copy(pGray8, 0, imgPtr, width * height);
                        //HImage image = new HImage("byte", width, height, imgPtr).ScaleImageMax(); // 将实时图像转换为字节图像
                        //if (imgPtr != IntPtr.Zero)
                        //    Marshal.FreeHGlobal(imgPtr);
                        this.drawObject.BackImage = imageData;// new ImageDataClass(image, camPara, camPose);
                        extractCircle(imageData.Image, Threshold, out row, out col);
                        Rows.Append(row);
                        Cols.Append(col);
                        ///////////
                        start_pos -= step;
                        if (start_pos < end_pos) break;
                    }
                    if (Rows.Length > 1)
                    {
                        HXLDCont line = new HXLDCont(Rows, Cols);
                        //////////////////////////////////////
                        line.FitLineContourXld("tukey", -1, 0, 5, 2, out RowBegin, out ColBegin, out RowEnd, out ColEnd, out Nr, out Nc, out Dist); // 标定相机倾斜角时，只能用像素坐标，因相机还未标定
                        HOperatorSet.LineOrientation(RowBegin, ColBegin, RowEnd, ColEnd, out phi);
                        HOperatorSet.SetColor(this.hWindowControl1.HalconWindow, "red");
                        HOperatorSet.DispObj(line, this.hWindowControl1.HalconWindow);
                        this.camSlant = phi.TupleDeg().D;
                        this.角度textBox.Text = this.camSlant.ToString();
                        //if (this.laserParam != null)
                        //    this.laserParam[2] = phi.TupleDeg().D;
                        this.laserPoseParam.Rz = phi.TupleDeg().D;
                    }
                }
            );
            }
            catch (Exception e3)
            {
                MessageBox.Show(new Exception().ToString());
            }
        }
        private void extractCircle(HImage image, double Threshold, out double row, out double col)
        {
            row = 0;
            col = 0;
            if (image == null) return;
            HTuple Row, Column, Radius, StartPhi, EndPhi, PointOrder;
            HXLDCont circle = image.ThresholdSubPix(Threshold).UnionCocircularContoursXld(0.5, 0.1, 0.2, 30, 10, 10, "true", 3).SelectShapeXld("contlength", "and", 0.7, 99999); // .SelectShapeXld("circularity", "and", 0.7, 99999)            
            if (circle.CountObj() == 0) return;
            HTuple length = circle.LengthXld();
            circle = circle.SelectShapeXld("contlength", "and", length.TupleMax().D * 0.6, length.TupleMax().D * 1.1);
            ////////////////////////////
            circle.SelectShapeXld("contlength", "and", length.TupleMax().D * 0.66, length.TupleMax().D * 1.5).FitCircleContourXld("algebraic", -1, 0, 0, 3, 2, out Row, out Column, out Radius, out StartPhi, out EndPhi, out PointOrder);
            if (Row != null && Row.Length > 0)
            {
                row = Row[Column.TupleSortIndex()].D;
                col = Column[Column.TupleSortIndex()].D;
            }
            else
            {
                row = 0;
                col = 0;
            }
            HXLDCont cross = new HXLDCont();
            cross.GenCrossContourXld(row, col, 20, 0);
            this.drawObject.AttachPropertyData.Clear();
            this.drawObject.AttachPropertyData.Add(circle.ConcatObj(cross));
        }
        private void 记录坐标1button_Click(object sender, EventArgs e)
        {
            double x_pos, y_pos, z_pos;
            if (this._laserAcqSource == null) return;
            this._laserAcqSource.GetAxisPosition(enAxisName.X轴, out x_pos);
            this._laserAcqSource.GetAxisPosition(enAxisName.Y轴, out y_pos);
            this._laserAcqSource.GetAxisPosition(enAxisName.Z轴, out z_pos);
            this.X1_textBox.Text = x_pos.ToString();
            this.Y1_textBox.Text = y_pos.ToString();
            this.Z1_textBox.Text = z_pos.ToString();
        }

        private void 记录坐标2button_Click(object sender, EventArgs e)
        {
            double x_pos, y_pos, z_pos;
            if (this._laserAcqSource == null) return;
            this._laserAcqSource.GetAxisPosition(enAxisName.X轴, out x_pos);
            this._laserAcqSource.GetAxisPosition(enAxisName.Y轴, out y_pos);
            this._laserAcqSource.GetAxisPosition(enAxisName.Z轴, out z_pos);
            this.X2_textBox.Text = x_pos.ToString();
            this.Y2_textBox.Text = y_pos.ToString();
            this.Z2_textBox.Text = z_pos.ToString();
        }


    }
}

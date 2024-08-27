
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
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using View;

namespace FunctionBlock
{

    public partial class LineSensorCalibrateForm : Form
    {
        private ImageExtractLineParamForm camForm, laserForm;
        private double[] laserParam = new double[10000]; //给一个大点的值，长些没关系，后面的数据不会使用
        private FunctionBlock.AcqSource _cameraAcqSource;
        private FunctionBlock.AcqSource _laserAcqSource;
        private ImageDataClass camImageData;
        private ImageDataClass laserImageData;
        private LineMeasure camLineMeasure;
        private LineMeasure laserLineMeasure;
        /// //////////////////////////////////
        private userDrawLineMeasure drawCamObject;
        private userDrawLineMeasure drawLaserObject;
        private userWcsLine fitCamLie;
        private userWcsLine fitLaserLine;
        private userWcsPose laserPoseParam;

        public LineSensorCalibrateForm()
        {
            InitializeComponent();
            camLineMeasure = new LineMeasure();
            laserLineMeasure = new LineMeasure();
            drawCamObject = new userDrawLineMeasure(this.相机视图hWindowControl);
            drawLaserObject = new userDrawLineMeasure(this.激光视图hWindowControl);
            //this.laserPoseParam = new userLaserCalibrateParam();
        }
        private void LineSensorCalibrateModifyForm_Load(object sender, EventArgs e)
        {
            DataInteractionClass.getInstance().MetrolegyComplete += new MetrolegyCompletedEventHandler(this.MetrolegyCompleted);
            //SensorBase.ImageAcqComplete += new ImageAcqCompleteEventHandler(ImageAcqComplete_Event);
            BindProperty();
            AddForm(this.运动panel, new JogMotionForm());
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
                this.相机传感器comboBox.DataSource = FunctionBlock.AcqSourceManage.Instance.GetCamAcqSourceList();// GetCamSensor();
                this.相机传感器comboBox.DisplayMember = "Name";
                this.激光传感器comboBox.DataSource = FunctionBlock.AcqSourceManage.Instance.LaserAcqSourceList();// GetLaserSensor();
                this.激光传感器comboBox.DisplayMember = "Name";
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
            }
        }


        private void MetrolegyCompleted(MetrolegyCompletedEventArgs e)
        {
            if (e.EdgeData == null) return;
            switch (e.EdgeData.GetType().Name)
            {

                case "userWcsCircle":
                    this.drawCamObject.AttachPropertyData.Clear();
                    this.drawLaserObject.AttachPropertyData.Clear();
                    userWcsCircle wcsCircle = (userWcsCircle)e.EdgeData;
                    this.drawCamObject.AttachPropertyData.Add(wcsCircle);
                    this.drawLaserObject.AttachPropertyData.Add(wcsCircle);
                    for (int i = 0; i < e.EdgePoint_x.Length; i++)
                    {
                        this.drawLaserObject.AttachPropertyData.Add(new userWcsPoint(e.EdgePoint_x[i], e.EdgePoint_y[i], 0, wcsCircle.CamParams));
                        this.drawCamObject.AttachPropertyData.Add(new userWcsPoint(e.EdgePoint_x[i], e.EdgePoint_y[i], 0, wcsCircle.CamParams));
                    }
                    this.drawCamObject.DetachDrawingObjectFromWindow();
                    this.drawLaserObject.DetachDrawingObjectFromWindow();
                    break;
                case "userWcsCircleSector":
                    this.drawCamObject.AttachPropertyData.Clear();
                    this.drawLaserObject.AttachPropertyData.Clear();
                    userWcsCircleSector wcsCircleSector = (userWcsCircleSector)e.EdgeData;
                    this.drawCamObject.AttachPropertyData.Add(wcsCircleSector);
                    this.drawLaserObject.AttachPropertyData.Add(wcsCircleSector);
                    for (int i = 0; i < e.EdgePoint_x.Length; i++)
                    {
                        this.drawCamObject.AttachPropertyData.Add(new userWcsPoint(e.EdgePoint_x[i], e.EdgePoint_y[i], 0, wcsCircleSector.CamParams));
                        this.drawLaserObject.AttachPropertyData.Add(new userWcsPoint(e.EdgePoint_x[i], e.EdgePoint_y[i], 0, wcsCircleSector.CamParams));
                    }
                    this.drawCamObject.DetachDrawingObjectFromWindow();
                    this.drawLaserObject.DetachDrawingObjectFromWindow();
                    break;
            }
        }
        private void addData(DataGridView dataGridView1, params object[] value) // 这里一定要用object类型，只有这样才可以直接添加到dataGridView1 控件上
        {
            try
            {
                dataGridView1.Rows.Add(value);
                /////////////////////////////////////////////////////////
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    dataGridView1.Rows[i].HeaderCell.Value = i.ToString();
                }
            }
            catch
            {

            }
        }
        private void deletedData(DataGridView dataGridView1)
        {
            if (dataGridView1.CurrentRow == null) return;
            dataGridView1.Rows.Remove(dataGridView1.CurrentRow);
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                dataGridView1.Rows[i].HeaderCell.Value = i.ToString();
            }
        }

        private void 激光采集源comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.激光传感器comboBox.SelectedItem == null) return;
            this._laserAcqSource = (FunctionBlock.AcqSource)this.激光传感器comboBox.SelectedItem;
        }
        private void 相机传感器comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.相机传感器comboBox.SelectedItem == null) return;
            this._cameraAcqSource = (FunctionBlock.AcqSource)this.相机传感器comboBox.SelectedItem;
        }

        private void ImageAcqComplete_Event(ImageAcqCompleteEventArgs e)
        {
            double x, y, z;
            //if (!实时图像checkBox.Checked) return;
            if (MotionCardManage.CurrentCard != null)
            {
                MotionCardManage.CurrentCard.GetAxisPosition(MotionCardManage.CurrentCoordSys, enAxisName.X轴,  out x);
                MotionCardManage.CurrentCard.GetAxisPosition(MotionCardManage.CurrentCoordSys, enAxisName.Y轴,  out y);
                MotionCardManage.CurrentCard.GetAxisPosition(MotionCardManage.CurrentCoordSys, enAxisName.Z轴,  out z);
                e.ImageData.Grab_X = x;
                e.ImageData.Grab_Y = y;
                e.ImageData.Grab_Z = z;
                this.drawCamObject.BackImage = e.ImageData;
                this.camImageData = e.ImageData;
            }
            else
            {
                this.camImageData = e.ImageData;
                this.drawCamObject.BackImage = e.ImageData;
            }
        }
        private void 保存button_Click(object sender, EventArgs e)
        {
            try
            {
                FileOperate fo = new FileOperate();
                string path = Application.StartupPath + "\\" + "激光标定参数" + "\\" + this._laserAcqSource.Sensor.LaserParam.SensorName;
                //this._laserAcqSource.Sensor.SetParam(enSensorParamType.Coom_激光校准参数, this.laserCalibrateParam);
                this._laserAcqSource.Sensor.LaserParam.LaserCalibrationParam.LaserPose = this.laserPoseParam;
                this._laserAcqSource.Sensor.LaserParam.LaserCalibrationParam.Save(path);
                MessageBox.Show("保存成功");
            }
            catch
            {
                MessageBox.Show(new Exception().ToString());
            }
        }
        private void 加载参数button_Click(object sender, EventArgs e)
        {
            try
            {
                //FileOperate fo = new FileOperate();
                //string path = Application.StartupPath + "\\" + "激光标定参数" + "\\" + this._laserAcqSource.Sensor.LaserParam.SensorName + ".txt";
                this.laserPoseParam = this._laserAcqSource.Sensor.LaserParam.LaserCalibrationParam.LaserPose;// new userLaserCalibrateParam().Read(path);
                /////////
                this.激光dataGridView.Rows.Clear();
                for (int i = 0; i < 10; i++)
                {
                    switch (i)
                    {
                        case 0:
                            this.激光dataGridView.Rows.Add(this.laserPoseParam.Tx); //.ToString()
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
            }
            catch (Exception ex)
            {
                MessageBox.Show("加载参数button_Click操作失败" + ex.ToString());
            }
        }

        private void LineSensorCalibrateForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            DataInteractionClass.getInstance().MetrolegyComplete -= new MetrolegyCompletedEventHandler(this.MetrolegyCompleted);
            //SensorBase.ImageAcqComplete -= new ImageAcqCompleteEventHandler(ImageAcqComplete_Event);
        }

        private void 获取当前位置1button_Click(object sender, EventArgs e)
        {
            double x, y, z;
            try
            {
                this._laserAcqSource.GetAxisPosition(enAxisName.X轴, out x);
                this._laserAcqSource.GetAxisPosition(enAxisName.Y轴, out y);
                this._laserAcqSource.GetAxisPosition(enAxisName.Z轴, out z);
                this.X1坐标textBox.Text = x.ToString();
                this.Y1坐标textBox.Text = y.ToString();
                this.Z1坐标textBox.Text = z.ToString();
            }
            catch
            {
                MessageBox.Show("获取当前位置1button_Click操作失败" + new Exception().ToString());
            }
        }
        private void 获取当前位置2button_Click(object sender, EventArgs e)
        {
            double x, y, z;
            try
            {
                this._laserAcqSource.GetAxisPosition(enAxisName.X轴, out x);
                this._laserAcqSource.GetAxisPosition(enAxisName.Y轴, out y);
                this._laserAcqSource.GetAxisPosition(enAxisName.Z轴, out z);
                this.X2坐标textBox.Text = x.ToString();
                this.Y2坐标textBox.Text = y.ToString();
                this.Z2坐标textBox.Text = z.ToString();
            }
            catch
            {
                MessageBox.Show("获取当前位置2button_Click操作失败" + new Exception().ToString());
            }
        }
        private void 激光扫描button_Click(object sender, EventArgs e)
        {
            try
            {
                double x1 = double.Parse(this.X1坐标textBox.Text);
                double y1 = double.Parse(this.Y1坐标textBox.Text);
                double z1 = double.Parse(this.Z1坐标textBox.Text);
                double x2 = double.Parse(this.X2坐标textBox.Text);
                double y2 = double.Parse(this.Y2坐标textBox.Text);
                double z2 = double.Parse(this.Z2坐标textBox.Text);
                double resolution_x = double.Parse(this.X分辨率textBox.Text);
                double resolution_y = double.Parse(this.Y分辨率textBox.Text);
                ImageDataClass image1, image2, image3;
                MoveCommandParam CommandParam1 = new MoveCommandParam();
                MoveCommandParam CommandParam2 = new MoveCommandParam();
                FunctionBlock.TransformLaserPointCloudDataHandle pointData1 = new FunctionBlock.TransformLaserPointCloudDataHandle();
                HalconLibrary ha = new HalconLibrary();
                CommandParam1.PoseInfo = "1";
                CommandParam1.MoveAxis = enAxisName.XYZ轴; // 运动到位
                CommandParam1.MoveSpeed = GlobalVariable.pConfig.MoveSpeed;
                CommandParam1.AxisParam = new CoordSysAxisParam( x1, y1, z1 ,0,0,0);
                ////////////
                CommandParam2.PoseInfo = "1";
                CommandParam2.MoveAxis = enAxisName.XYZ轴; // 运动到位
                CommandParam2.MoveSpeed = GlobalVariable.pConfig.ScanSpeed;
                CommandParam2.AxisParam = new CoordSysAxisParam (x2, y2, z2 ,0,0,0);
                Dictionary<enDataItem, object> list = this._laserAcqSource.AcqScanData(CommandParam1, CommandParam2);
                pointData1.TransformData(list, _laserAcqSource.Sensor.ConfigParam.SensorType, _laserAcqSource.Sensor.LaserParam.DataWidth, enScanAxis.Y轴, CommandParam1.AxisParam, CommandParam2.AxisParam); // 这里不要使用校区准
                ha.RenderObjectModel3DTo3ImageModify(new HObjectModel3D[] { pointData1.Dist1DataHandle }, resolution_x, resolution_y, out image1, out this.laserImageData, out image3);
                this.laserImageData.Grab_X = 0;// CommandParam1.targetPosition[0]; 从点云创建的图像，这里不需要指定参考位置
                this.laserImageData.Grab_Y = 0;// CommandParam1.targetPosition[1];
                this.drawLaserObject.BackImage = laserImageData;// laserImageData;
            }
            catch
            {
                MessageBox.Show("激光扫描button_Click操作失败" + new Exception().ToString());
            }
        }

        private void 标定Rybutton_Click(object sender, EventArgs e)
        {
            HTuple RowBegin, ColBegin, RowEnd, ColEnd, Nr, Nc, Dist;
            try
            {
                HalconLibrary ha = new HalconLibrary();
                Dictionary<enDataItem, object> list = this._laserAcqSource.AcqPointData();
                double[] laserDist1 = (double[])list[enDataItem.Dist1];
                double[] encoder_X = (double[])list[enDataItem.Dist2];
                double[] encoder_Y = (double[])list[enDataItem.Y];
                if (laserDist1.Length == 0)
                {
                    MessageBox.Show("采集点数为0");
                    return;
                }
                ////////////////////////////////////////////////////////////////
                int count = this._laserAcqSource.Sensor.LaserParam.DataWidth;
                HXLDCont line;
                HTuple phi,x,y,z;
                HObjectModel3D lineObjectModel = null ;
                double deg,start_x,start_y,start_z,end_x,end_y,end_z;
                switch (this._laserAcqSource.Sensor.LaserParam.ScanAxis)
                {
                    case enScanAxis.X轴:
                         x = HTuple.TupleGenConst(count, 0);
                         y = new HTuple(encoder_Y).TupleSelectRange(0, count - 1);
                         z = new HTuple(laserDist1).TupleSelectRange(0, count - 1);
                        lineObjectModel = new HObjectModel3D(x, y, z);
                        ha.FitLine3D(lineObjectModel, out start_x, out start_y, out start_z, out end_x, out end_y, out end_z);
                        lineObjectModel?.Dispose();
                        //line = new HXLDCont(new HTuple(laserDist1).TupleSelectRange(0, count - 1), new HTuple(encoder_Y).TupleSelectRange(0, count - 1));
                        //line.FitLineContourXld("tukey", -1, 0, 5, 2, out RowBegin, out ColBegin, out RowEnd, out ColEnd, out Nr, out Nc, out Dist);
                        //phi = HMisc.LineOrientation(RowBegin, ColBegin, RowEnd, ColEnd);
                        deg = Math.Atan2(end_z - start_z, Math.Sqrt((end_x - start_x) * (end_x - start_x) + (end_y - start_y) * (end_y - start_y))) * 180 / Math.PI;
                        this._laserAcqSource.Sensor.LaserParam.LaserCalibrationParam.LaserType = "线激光";
                        this.laserPoseParam.Rx = deg;
                        this.laserPoseParam = new userWcsPose((start_y + end_y) * 0.5, 0, 0, deg, 0, 0);
                        break;
                    case enScanAxis.Y轴:
                        y = HTuple.TupleGenConst(count, 0);
                        x = new HTuple(encoder_X).TupleSelectRange(0, count - 1);
                        z = new HTuple(laserDist1).TupleSelectRange(0, count - 1);
                        lineObjectModel = new HObjectModel3D(x, y, z);
                        ha.FitLine3D(lineObjectModel, out start_x, out start_y, out start_z, out end_x, out end_y, out end_z);
                        lineObjectModel?.Dispose();
                        //line = new HXLDCont(new HTuple(laserDist1).TupleSelectRange(0, count - 1), new HTuple(encoder_X).TupleSelectRange(0, count - 1));
                        //line.FitLineContourXld("tukey", -1, 0, 5, 2, out RowBegin, out ColBegin, out RowEnd, out ColEnd, out Nr, out Nc, out Dist);
                        //phi = HMisc.LineOrientation(RowBegin, ColBegin, RowEnd, ColEnd);
                        deg = Math.Atan2(end_z - start_z, Math.Sqrt((end_x - start_x) * (end_x - start_x) + (end_y - start_y) * (end_y - start_y))) * 180 / Math.PI;
                        this._laserAcqSource.Sensor.LaserParam.LaserCalibrationParam.LaserType = "线激光";
                        this.laserPoseParam.Rx = deg;
                        this.laserPoseParam = new userWcsPose((start_x + end_x) * 0.5, 0, 0, 0, deg, 0);
                        break;
                }
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
            }
            catch (Exception ex)
            {
                MessageBox.Show("加载参数button_Click操作失败" + ex.ToString());
            }
        }

        private void 相机采图button_Click(object sender, EventArgs e)
        {
            if (this._cameraAcqSource != null)
            {
                this._cameraAcqSource.AcqPointData(); // 只需要触发即可
            }
        }

        private void 激光直线测量button_Click(object sender, EventArgs e)
        {
            try
            {
                if (laserImageData == null)
                {
                    MessageBox.Show("laserImageData，请先采集图像");
                    return;
                }
                if (this.Z1坐标textBox.Text == string.Empty)
                    laserImageData.Grab_Z = 0;
                else
                    laserImageData.Grab_Z = double.Parse(this.Z1坐标textBox.Text);
                if (this.drawLaserObject != null)
                    this.drawLaserObject.ClearDrawingObject();
                this.drawLaserObject.BackImage = this.laserImageData;
                this.drawLaserObject.AttachDrawingObjectToWindow();
                if (this.laserForm != null)
                {
                    this.laserForm.Dispose();
                    this.laserForm = null;
                }
                this.laserForm = new ImageExtractLineParamForm(this.laserLineMeasure, this.drawLaserObject, this.laserImageData, this.激光数据dataGridView);
                this.laserForm.Owner = this;
                this.laserForm.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }
        private void 相机直线测量button_Click(object sender, EventArgs e)
        {
            try
            {
                if (camImageData == null)
                {
                    MessageBox.Show("camImageData为空，请先采集图像");
                    return;
                }
                if (this.Z1坐标textBox.Text == string.Empty)
                    camImageData.Grab_Z = 0;
                else
                    camImageData.Grab_Z = double.Parse(this.Z1坐标textBox.Text);
                if (this.drawLaserObject != null)
                    this.drawLaserObject.ClearDrawingObject();
                this.drawLaserObject.BackImage = this.camImageData;
                drawCamObject.AttachDrawingObjectToWindow();
                if (this.camForm != null)
                {
                    this.camForm.Dispose();
                    this.camForm = null;
                }
                this.camForm = new ImageExtractLineParamForm(this.camLineMeasure, this.drawCamObject, this.camImageData, this.相机数据dataGridView);
                this.camForm.Owner = this;
                this.camForm.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void 激光dataGridView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (this.激光dataGridView.Rows.Count == 0) return;
            object value = this.激光dataGridView[e.ColumnIndex, e.RowIndex].Value;
            if (value is DBNull) return;
            if (value is null) return;
            switch (e.RowIndex)
            {
                case 0:
                    this.laserPoseParam.Tx = Convert.ToDouble(value);
                    break;
                case 1:
                    this.laserPoseParam.Ty = Convert.ToDouble(value);
                    break;
                case 2:
                    this.laserPoseParam.Tz = Convert.ToDouble(value);
                    break;
                case 3:
                    this.laserPoseParam.Rx = Convert.ToDouble(value);
                    break;
                case 4:
                    this.laserPoseParam.Ry = Convert.ToDouble(value);
                    this.laserPoseParam = new userWcsPose(0, 0, 0, 0, this.laserPoseParam.Ry, 0);
                    break;
                case 5:
                    this.laserPoseParam.Rz = Convert.ToDouble(value);
                    break;
                case 6:
                    this._laserAcqSource.Sensor.LaserParam.LaserCalibrationParam.LaserType = value.ToString();
                    break;
            }
        }

        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }

        private void 标定Rzbutton_Click(object sender, EventArgs e)
        {
            try
            {
                HTuple camPhi, laserPhi;
                //this.fitCamLie = this.camForm.GetMeasureResult();
                //this.fitLaserLine = this.laserForm.GetMeasureResult();
                if (this.激光dataGridView.RowCount == 0 || this.相机数据dataGridView.RowCount == 0) return;
                this.fitLaserLine.X1 = Convert.ToDouble(this.激光dataGridView[0, 0].Value);
                this.fitLaserLine.Y1 = Convert.ToDouble(this.激光dataGridView[1, 0].Value);
                this.fitLaserLine.Z1 = Convert.ToDouble(this.激光dataGridView[2, 0].Value);
                this.fitLaserLine.X2 = Convert.ToDouble(this.激光dataGridView[3, 0].Value);
                this.fitLaserLine.Y2 = Convert.ToDouble(this.激光dataGridView[4, 0].Value);
                this.fitLaserLine.Z2 = Convert.ToDouble(this.激光dataGridView[5, 0].Value);
                //////////////////////
                this.fitCamLie.X1 = Convert.ToDouble(this.相机数据dataGridView[0, 0].Value);
                this.fitCamLie.Y1 = Convert.ToDouble(this.相机数据dataGridView[1, 0].Value);
                this.fitCamLie.Z1 = Convert.ToDouble(this.相机数据dataGridView[2, 0].Value);
                this.fitCamLie.X2 = Convert.ToDouble(this.相机数据dataGridView[3, 0].Value);
                this.fitCamLie.Y2 = Convert.ToDouble(this.相机数据dataGridView[4, 0].Value);
                this.fitCamLie.Z2 = Convert.ToDouble(this.相机数据dataGridView[5, 0].Value);
                //////////////////////////////////////////////////////////////////
                HOperatorSet.LineOrientation(this.fitCamLie.Y1 * -1, this.fitCamLie.X1, this.fitCamLie.Y2 * -1, this.fitCamLie.X2, out camPhi);
                HOperatorSet.LineOrientation(this.fitLaserLine.Y1 * -1, this.fitLaserLine.X1, this.fitLaserLine.Y2 * -1, this.fitLaserLine.X2, out laserPhi);
                this.laserPoseParam.Rz = laserPhi.TupleDeg().D - camPhi.TupleDeg().D;
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
            catch
            {
                MessageBox.Show("标定button_Click操作失败");
            }

        }




    }
}

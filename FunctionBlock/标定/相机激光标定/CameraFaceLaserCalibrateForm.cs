
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
using AlgorithmsLibrary;

namespace FunctionBlock
{
    public partial class CameraFaceLaserCalibrateForm : Form
    {
        private FunctionBlock.AcqSource _cameraAcqSource;
        private FunctionBlock.AcqSource _laserAcqSource;
        private CancellationTokenSource cts;
        // private HTuple camPara, laserPose;
        private userWcsPose LaserAffineParam;
        private ImageDataClass imageData;
        private LineMeasure lineMeasure;
        private userDrawLineMeasure drawObject;
        private userWcsLine fitLine1, fitLine2, fitLine3, fitLine4;
        private ImageExtractLineParamForm form1, form2, form3, form4;
        public CameraFaceLaserCalibrateForm()
        {
            InitializeComponent();
            this.drawObject = new userDrawLineMeasure(this.相机视图hWindowControl);
        }
        private void CameraFaceLaserCalibrateForm_Load(object sender, EventArgs e)
        {
            // SensorBase.ImageAcqComplete += new ImageAcqCompleteEventHandler(ImageAcqComplete_Event);
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
                this.激光传感器comboBox.DataSource = FunctionBlock.AcqSourceManage.Instance.LaserAcqSourceList(); //GetLaserSensor();
                this.激光传感器comboBox.DisplayMember = "Name";
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
            }
        }


        private void 采集源comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            this._cameraAcqSource = (FunctionBlock.AcqSource)this.相机传感器comboBox.SelectedItem;
        }
        private void 激光采集源comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            this._laserAcqSource = (FunctionBlock.AcqSource)this.激光传感器comboBox.SelectedItem;
        }
        private void 激光直线测量1button_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.imageData == null)
                {
                    MessageBox.Show("图像数据为空");
                    return;
                }
                if (this.Z1坐标textBox.Text == string.Empty)
                    this.Z1坐标textBox.Text = "0";
                this.imageData.Grab_Z = double.Parse(this.Z1坐标textBox.Text);
                if (this.drawObject != null)
                    this.drawObject.ClearDrawingObject();
                this.lineMeasure = new LineMeasure();
                this.drawObject = new userDrawLineMeasure(this.相机视图hWindowControl);
                this.drawObject.BackImage = this.imageData;
                this.drawObject.AttachDrawingObjectToWindow();
                if (this.form1 != null)
                {
                    this.form1.Dispose();
                    this.form1 = null;
                }
                /////////////////////////////////////
                this.form1 = new ImageExtractLineParamForm(this.lineMeasure, this.drawObject, this.imageData, this.激光数据dataGridView);
                this.form1.Owner = this;
                this.form1.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void 激光直线测量2button_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.imageData == null)
                {
                    MessageBox.Show("图像数据为空");
                    return;
                }
                if (this.Z1坐标textBox.Text == string.Empty)
                    this.Z1坐标textBox.Text = "0";
                this.imageData.Grab_Z = double.Parse(this.Z1坐标textBox.Text);
                if (this.drawObject != null)
                    this.drawObject.ClearDrawingObject();
                this.lineMeasure = new LineMeasure();
                this.drawObject = new userDrawLineMeasure(this.相机视图hWindowControl);
                this.drawObject.BackImage = this.imageData;
                this.drawObject.AttachDrawingObjectToWindow();
                if (this.form2 != null)
                {
                    this.form2.Dispose();
                    this.form2 = null;
                }
                ///////////////////////////////
                this.form2 = new ImageExtractLineParamForm(this.lineMeasure, this.drawObject, this.imageData, this.激光数据dataGridView);
                this.form2.Owner = this;
                this.form2.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private double[] selectDataRange(double[] data, int length)
        {
            double[] dist = new double[length];
            for (int i = 0; i < length; i++)
            {
                if (data == null) break;
                dist[i] = data[i];
            }
            return dist;
        }
        private void 激光扫描button_Click(object sender, EventArgs e)
        {
            try
            {
                double x1 = double.Parse(this.X1坐标textBox.Text);
                double y1 = double.Parse(this.Y1坐标textBox.Text);
                double z1 = double.Parse(this.Z1坐标textBox.Text);
                double resolution_x = double.Parse(this.X分辨率textBox.Text);
                double resolution_y = double.Parse(this.Y分辨率textBox.Text);
                ImageDataClass image1, image2, image3;
                MoveCommandParam CommandParam1 = new MoveCommandParam();
                FunctionBlock.TransformLaserPointCloudDataHandle pointData1 = new FunctionBlock.TransformLaserPointCloudDataHandle();
                HalconLibrary ha = new HalconLibrary();
                CommandParam1.PoseInfo = "1";
                CommandParam1.MoveAxis = enAxisName.XYZ轴; // 运动到位
                CommandParam1.MoveSpeed = GlobalVariable.pConfig.MoveSpeed;
                CommandParam1.AxisParam = new CoordSysAxisParam(  x1, y1, z1,0,0,0) ;
                Dictionary<enDataItem, object> list = this._laserAcqSource.AcqPointData(CommandParam1);
                double[] refCalibrateDataValue = this._laserAcqSource.Sensor.GetParam(enSensorParamType.Coom_激光校准参数) as double[];
                pointData1.TransformDataAndCalibrateData(list, _laserAcqSource.Sensor.ConfigParam.SensorType, _laserAcqSource.Sensor.LaserParam.DataWidth, _laserAcqSource.Sensor.LaserParam.DataHeight, CommandParam1.AxisParam,  refCalibrateDataValue);
                //ha.TransformObject3DToRealImage(HObjectModel3D.UnionObjectModel3d(pointData1.Dist1DataHandle.ToArray(), "points_surface"), resolution_x, resolution_y, out this.imageData);
                ha.RenderObjectModel3DTo3ImageModify(new HObjectModel3D[] { pointData1.Dist1DataHandle }, resolution_x, resolution_y, out image1, out this.imageData, out image3);
                this.imageData.Grab_X = 0;// CommandParam1.targetPosition[0]; // 对于从点云创建的图像，这里不需要增加参考位置
                this.imageData.Grab_Y = 0;// CommandParam1.targetPosition[1];
                this.drawObject.BackImage = this.imageData;
                this.imageData.Grab_Z = double.Parse(this.Z1坐标textBox.Text);

            }
            catch
            {
                MessageBox.Show("激光扫描button_Click操作失败" + new Exception().ToString());
            }
        }
        private void 相机直线测量1button_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.imageData == null)
                {
                    MessageBox.Show("图像数据为空");
                    return;
                }
                this.相机采图button_Click(null, null);
                ///////////////////////////////////////////
                if (this.drawObject != null)
                    this.drawObject.ClearDrawingObject();
                this.lineMeasure = new LineMeasure();
                this.drawObject = new userDrawLineMeasure(this.相机视图hWindowControl);
                this.drawObject.BackImage = this.imageData;
                this.drawObject.AttachDrawingObjectToWindow();
                if (this.form3 != null)
                {
                    this.form3.Dispose();
                    this.form3 = null;
                }
                ////////////////////////////////
                this.form3 = new ImageExtractLineParamForm(this.lineMeasure, this.drawObject, this.imageData, this.相机数据dataGridView);
                this.form3.Owner = this;
                this.form3.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void 相机直线测量2button_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.imageData == null)
                {
                    MessageBox.Show("图像数据为空");
                    return;
                }
                this.相机采图button_Click(null, null);
                ///////////////////////////////////
                if (this.drawObject != null)
                    this.drawObject.ClearDrawingObject();
                this.lineMeasure = new LineMeasure();
                this.drawObject = new userDrawLineMeasure(this.相机视图hWindowControl);
                this.drawObject.BackImage = this.imageData;
                this.drawObject.AttachDrawingObjectToWindow();
                if (this.form4 != null)
                {
                    this.form4.Dispose();
                    this.form4 = null;
                }
                ///////////////////////////////
                this.form4 = new ImageExtractLineParamForm(this.lineMeasure, this.drawObject, this.imageData, this.相机数据dataGridView);
                this.form4.Owner = this;
                this.form4.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void ImageAcqComplete_Event(ImageAcqCompleteEventArgs e)
        {
            double x, y, z;
            if (!this.实时图像checkBox.Checked) return;
            if (MotionCardManage.CurrentCard != null)
            {
                MotionCardManage.CurrentCard.GetAxisPosition(MotionCardManage.CurrentCoordSys, enAxisName.X轴, out x);
                MotionCardManage.CurrentCard.GetAxisPosition(MotionCardManage.CurrentCoordSys, enAxisName.Y轴, out y);
                MotionCardManage.CurrentCard.GetAxisPosition(MotionCardManage.CurrentCoordSys, enAxisName.Z轴,  out z);
                e.ImageData.Grab_X = x;
                e.ImageData.Grab_Y = y;
                e.ImageData.Grab_Z = z;
                this.drawObject.BackImage = e.ImageData;
                this.imageData = e.ImageData;
            }
            else
            {
                this.imageData = e.ImageData;
                this.drawObject.BackImage = e.ImageData;
            }
        }
        private void 保存button_Click(object sender, EventArgs e)
        {
            try
            {
                string path = Application.StartupPath + "\\" + "激光标定参数" + "\\" + this._laserAcqSource.Sensor.Name + this._cameraAcqSource.Sensor.Name;
                this._laserAcqSource.Sensor.LaserParam.LaserCalibrationParam.LaserAffinePose = this.LaserAffineParam;
                this._laserAcqSource.Sensor.LaserParam.LaserCalibrationParam.Save(path);
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
                int isParallel;
                double cam_x, cam_y, cam_z, laser_x, laser_y, laser_z;
                // 激光数据
                this.fitLine1 = this.form1.GetMeasureResult();
                this.fitLine2 = this.form2.GetMeasureResult();
                // 相机数据
                this.fitLine3 = this.form3.GetMeasureResult();
                this.fitLine4 = this.form4.GetMeasureResult();
                cam_z = this.fitLine3.Z1;
                laser_z = this.fitLine1.Z1;
                ////////////
                HMisc.IntersectionLl(fitLine1.X1, fitLine1.Y1 * 1, fitLine1.X2, fitLine1.Y2 * 1, fitLine2.X1, fitLine2.Y1 * 1, fitLine2.X2, fitLine2.Y2 * 1, out laser_x, out laser_y,  out isParallel);
                HMisc.IntersectionLl(fitLine3.X1, fitLine3.Y1 * 1, fitLine3.X2, fitLine3.Y2 * 1, fitLine4.X1, fitLine4.Y1 * 1, fitLine4.X2, fitLine4.Y2 * 1, out cam_x, out cam_y, out isParallel);
                ////////////////////////////////////////////////////////
                //this.laserPose = new HTuple(laser_x - cam_x, laser_y - cam_y, laser_z - cam_z, 0.0, 0.0, 0.0, 0);
                //HHomMat3D hHomMat3D = new HHomMat3D();
                //hHomMat3D.VectorToHomMat3d("rigid", laser_x, laser_y, laser_z, cam_x, cam_y, cam_z); // 至少三个点才能构成刚体变换

                this.LaserAffineParam = new userWcsPose(laser_x - cam_x, laser_y - cam_y, laser_z - cam_z);
                this.X平移textBox.Text = this.LaserAffineParam.Tx.ToString();
                this.Y平移textBox.Text = this.LaserAffineParam.Ty.ToString();
                this.Z平移textBox.Text = this.LaserAffineParam.Tz.ToString();
                this.X轴旋转textBox.Text = this.LaserAffineParam.Rx.ToString();
                this.Y轴旋转textBox.Text = this.LaserAffineParam.Ry.ToString();
                this.Z轴旋转textBox.Text = this.LaserAffineParam.Rz.ToString();
                this.位姿类型textBox.Text = this.LaserAffineParam.Type.ToString();
            }
            catch
            {
                MessageBox.Show("标定button_Click操作失败");
            }
        }
        private void LaserCameraCalibrateForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                //SensorBase.ImageAcqComplete -= new ImageAcqCompleteEventHandler(ImageAcqComplete_Event);
                if (cts != null)
                    cts.Cancel();
            }
            catch
            {

            }

        }
        private void 加载参数button_Click(object sender, EventArgs e)
        {
            try
            {
                this.LaserAffineParam = this._laserAcqSource.Sensor.LaserParam.LaserCalibrationParam.LaserAffinePose;
                this.X平移textBox.Text = this.LaserAffineParam.Tx.ToString();
                this.Y平移textBox.Text = this.LaserAffineParam.Ty.ToString();
                this.Z平移textBox.Text = this.LaserAffineParam.Tz.ToString();
                this.X轴旋转textBox.Text = this.LaserAffineParam.Rx.ToString();
                this.Y轴旋转textBox.Text = this.LaserAffineParam.Ry.ToString();
                this.Z轴旋转textBox.Text = this.LaserAffineParam.Rz.ToString();
                this.位姿类型textBox.Text = this.LaserAffineParam.Type.ToString();
            }
            catch
            {
                MessageBox.Show(new Exception().ToString());
            }
        }
        private void X平移textBox_TextChanged(object sender, EventArgs e)
        {
            double result;
            if (double.TryParse(this.X平移textBox.Text, out result))
            {
                this.LaserAffineParam.Tx = result;
            }
        }
        private void Y平移textBox_TextChanged(object sender, EventArgs e)
        {
            double result;
            if (double.TryParse(this.Y平移textBox.Text, out result))
            {
                this.LaserAffineParam.Ty = result;
            }
        }
        private void Z平移textBox_TextChanged(object sender, EventArgs e)
        {
            double result;
            if (double.TryParse(this.Z平移textBox.Text, out result))
            {
                this.LaserAffineParam.Tz = result;
            }
        }
        private void X轴旋转textBox_TextChanged(object sender, EventArgs e)
        {
            double result;
            if (double.TryParse(this.X轴旋转textBox.Text, out result))
            {
                this.LaserAffineParam.Rx = result;
            }
        }
        private void Y轴旋转textBox_TextChanged(object sender, EventArgs e)
        {
            double result;
            if (double.TryParse(this.Y轴旋转textBox.Text, out result))
            {
                this.LaserAffineParam.Ry = result;
            }
        }

        private void Z轴旋转textBox_TextChanged(object sender, EventArgs e)
        {
            double result;
            if (double.TryParse(this.Z轴旋转textBox.Text, out result))
            {
                this.LaserAffineParam.Rz = result;
            }
        }
        private void hWindowControl1_MouseEnter(object sender, EventArgs e)
        {
            this.相机视图hWindowControl.Cursor = Cursors.Cross;
        }
        private void hWindowControl1_MouseLeave(object sender, EventArgs e)
        {
            this.相机视图hWindowControl.Cursor = Cursors.Default;
        }



        private void 获取当前激光位置1button_Click(object sender, EventArgs e)
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
                MessageBox.Show(new Exception().ToString());
            }
        }


        private void 相机采图button_Click(object sender, EventArgs e)
        {
            if (this._cameraAcqSource != null)
            {
                this._cameraAcqSource.AcqPointData(); // 只需要触发即可
            }
        }

        private void CalculateIntersectPoint(double[] x, double[] z, out double inter_x, out double inter_z)
        {
            inter_x = 0;
            inter_z = 0;
            if (x == null || z == null) return;
            List<double> list_x = new List<double>();
            List<double> list_z = new List<double>();
            List<double> left_x = new List<double>();
            List<double> left_z = new List<double>();
            List<double> right_x = new List<double>();
            List<double> right_z = new List<double>();
            for (int i = 0; i < x.Length; i++)
            {
                if (z[i] != 0)
                {
                    list_x.Add(x[i]);
                    list_z.Add(z[i]);
                }
            }
            for (int i = 1; i < list_z.Count; i++)
            {
                if (list_z[i] > list_z[i - 1])
                {
                    left_x.Add(x[i]);
                    left_z.Add(z[i]);
                }
                else
                {
                    right_x.Add(x[i]);
                    right_z.Add(z[i]);
                }
            }
            if (left_x.Count < 2 || right_x.Count < 2) return;
            /////////////////////////////////
            HTuple Row1Begin, Col1Begin, Row1End, Col1End, Row2Begin, Col2Begin, Row2End, Col2End, Nr, Nc, Dist;
            new HXLDCont(new HTuple(left_z.ToArray()), new HTuple(left_x.ToArray())).FitLineContourXld("tukey", -1, 0, 5, 2, out Row1Begin, out Col1Begin, out Row1End, out Col1End, out Nr, out Nc, out Dist);
            new HXLDCont(new HTuple(right_z.ToArray()), new HTuple(right_x.ToArray())).FitLineContourXld("tukey", -1, 0, 5, 2, out Row2Begin, out Col2Begin, out Row2End, out Col2End, out Nr, out Nc, out Dist);
            int aa;
            HMisc.IntersectionLl(Row1Begin.D, Col1Begin.D, Row1End.D, Col1End.D, Row2Begin.D, Col2Begin.D, Row2End.D, Col2End.D, out inter_z, out inter_x, out aa);
        }

    }
}

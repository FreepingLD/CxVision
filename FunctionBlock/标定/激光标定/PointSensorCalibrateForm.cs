
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

    public partial class PointSensorCalibrateForm : Form
    {
        private Form form;
        private ISensor _sensor;
        private AcqSource _laserAcqSource;
        //private double[] laserParam;
        private userWcsPose laserPoseParam;
        private VisualizeView visualizeView;
        public PointSensorCalibrateForm()
        {
            InitializeComponent();
            this.visualizeView = new VisualizeView(this.hWindowControl1);
        }
        private void PointSensorCalibrateForm_Load(object sender, EventArgs e)
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
                this.激光采集源comboBox.DataSource = AcqSourceManage.Instance.LaserAcqSourceList();// GetLaserSensor();
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
            this._laserAcqSource = (AcqSource)this.激光采集源comboBox.SelectedItem;
            if (form != null) form.Close();
            this._sensor = this._laserAcqSource.Sensor; //  ((KeyValuePair<string, ISensor>)this.激光采集源comboBox.SelectedItem).Value
            /////////////////////////////////////////////////////////////////////////////////////////////
            try
            {
                switch (_sensor.GetType().Name)
                {
                    case "CStil_L":
                        form = new PointLaserForm(this._laserAcqSource); // 这里的代码比绑定中的代码先执行，所以这里只能通过键值来获取，基于事件的程序执行存在随机性
                        AddForm(this.FormPanel, form);
                        break;
                    case "CStil_P":
                        form = new PointLaserForm(this._laserAcqSource); // 这里的代码比绑定中的代码先执行，所以这里只能通过键值来获取，基于事件的程序执行存在随机性
                        AddForm(this.FormPanel, form);
                        break;

                    case "SSZNLineLaser":
                        break;

                    case "SmartRayLineLaser":
                        form = new SmartRayLineLaserMonitorForm(this._laserAcqSource);
                        AddForm(this.FormPanel, form);
                        break;

                    //case "LVM_LineLaser":
                    //    form = new LVMLineLaserForm(this._sensor);
                    //    AddForm(this.FormPanel, form);
                    //    break;

                    default:
                        AddForm(this.FormPanel, new Form());
                        break;
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
            }
        }

        private void 保存button_Click(object sender, EventArgs e)
        {
            try
            {
                //FileOperate fo = new FileOperate();
                string path = Application.StartupPath + "\\" + "激光标定参数" + "\\" + this._sensor.ConfigParam.SensorName;
                this._sensor.LaserParam.LaserCalibrationParam.LaserPose = this.laserPoseParam;
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
                //this._sensor.SetParam(enSensorParamType.Coom_激光校准参数, laserCalibrateParam);
                //fo.WriteTxt(path, this.laserCalibrateParam, false);
                this._sensor.LaserParam.LaserCalibrationParam.Save(path);
                MessageBox.Show("保存成功");
            }
            catch
            {
                MessageBox.Show(new Exception().ToString());
            }
        }

        private void 标定button_Click(object sender, EventArgs e)
        {
            double degRy;
            double standerValue;
            if(!double.TryParse(this.台阶标准高度textBox.Text, out standerValue))return ;
            try
            {
                double Z1 = double.Parse(Z1坐标textBox.Text);
                double Z2 = double.Parse(Z2坐标textBox.Text);
                degRy = Math.Acos(standerValue/Math.Abs(Z1- Z2))*180/Math.PI;
                this.laserPoseParam.Ry = degRy;
                this._sensor.LaserParam.LaserCalibrationParam.LaserType = "点激光";
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
                            this.激光dataGridView.Rows.Add(this._sensor.LaserParam.LaserCalibrationParam.LaserType);
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

        private void 加载参数button_Click(object sender, EventArgs e)
        {
            try
            {
                //FileOperate fo = new FileOperate();
                //string path = Application.StartupPath + "\\" + "LaserCalibrationParam" + "\\" + this._sensor.ConfigParam.SensorName + ".txt";
                //fo.ReadTxt(path, out this.laserCalibrateParam);
                //if (this.laserCalibrateParam == null) return;
                this.laserPoseParam = this._sensor.LaserParam.LaserCalibrationParam.LaserPose;
                /////////
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
                            this.激光dataGridView.Rows.Add(this._sensor.LaserParam.LaserCalibrationParam.LaserType);
                            this.激光dataGridView.Rows[i].HeaderCell.Value = "激光类型";
                            break;
                    }
                }
            }
            catch
            {
                MessageBox.Show(new Exception().ToString());
            }
        }

        private void LaserCameraCalibrateForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            string path = Application.StartupPath + "\\" + "激光标定参数" + "\\" + this._sensor.LaserParam.SensorName;
            this._sensor.LaserParam.LaserCalibrationParam.Save(path);
        }

        private void 获取当前位置1button_Click(object sender, EventArgs e)
        {
            double x, y, z;
            try
            {
                if (this._laserAcqSource == null)
                {
                    throw new ArgumentNullException("_laserAcqSource");
                }
                Dictionary<enDataItem, object> list1 = this._laserAcqSource.AcqPointData();
                this._laserAcqSource.GetAxisPosition(enAxisName.X轴, out x);
                this._laserAcqSource.GetAxisPosition(enAxisName.Y轴, out y);
                //this._laserAcqSource.GetAxisPosition(enAxisName.Z轴, out z);
                z = ((double[])list1[0]).Average();
                this.X1坐标textBox.Text = x.ToString();
                this.Y1坐标textBox.Text = y.ToString();
                this.Z1坐标textBox.Text = z.ToString();

                ////// 显示3D对象
                double x1, y1, z1;
                x1 = double.Parse(this.X2坐标textBox.Text);
                y1 = double.Parse(this.Y2坐标textBox.Text);
                z1 = double.Parse(this.Z2坐标textBox.Text);
                this.visualizeView.PointCloudModel3D = new PointCloudData (new HObjectModel3D(new HTuple(x, x1), new HTuple(y, y1), new HTuple(z, z1)));
            }
            catch (Exception ex)
            {
                MessageBox.Show("获取当前位置2button_Click操作失败" + ex.ToString());
            }
        }

        private void 获取当前位置2button_Click(object sender, EventArgs e)
        {
            double x, y, z;
            try
            {
                if(this._laserAcqSource==null)
                {
                    throw new ArgumentNullException("_laserAcqSource");
                }
                Dictionary<enDataItem, object> list1 = this._laserAcqSource.AcqPointData();
                this._laserAcqSource.GetAxisPosition(enAxisName.X轴, out x);
                this._laserAcqSource.GetAxisPosition(enAxisName.Y轴, out y);
                //this._laserAcqSource.GetAxisPosition(enAxisName.Z轴, out z);
                z = ((double[])list1[enDataItem.Dist1]).Average();
                this.X2坐标textBox.Text = x.ToString();
                this.Y2坐标textBox.Text = y.ToString();
                this.Z2坐标textBox.Text = z.ToString();

                ////// 显示3D对象
                double x1, y1, z1;
                x1 = double.Parse(this.X1坐标textBox.Text);
                y1 = double.Parse(this.Y1坐标textBox.Text);
                z1 = double.Parse(this.Z1坐标textBox.Text);
                this.visualizeView.PointCloudModel3D = new PointCloudData( new HObjectModel3D(new HTuple(x,x1), new HTuple(y, y1), new HTuple(z, z1)) );
            }
            catch(Exception ex)
            {
                MessageBox.Show("获取当前位置2button_Click操作失败" + ex.ToString());
            }
        }




    }
}

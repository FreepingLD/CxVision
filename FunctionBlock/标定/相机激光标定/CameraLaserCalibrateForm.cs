
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
    public partial class CameraLaserCalibrateForm : Form
    {
        private FunctionBlock.AcqSource _cameraAcqSource;
        private FunctionBlock.AcqSource _laserAcqSource;
        private CancellationTokenSource cts;
        private HTuple camPara, laserPose;
        private ImageDataClass imageData;
        private LineMeasure lineMeasure;
        private userDrawLineMeasure drawObject;
        private userWcsLine fitLine1, fitLine2, fitLine3, fitLine4;
        private ImageExtractLineParamForm form1, form2, form3, form4;
        private string programPath = "标定程序";                     // 程序文件路径
        private TreeViewWrapClass _treeViewWrapClass;
        public CameraLaserCalibrateForm()
        {
            InitializeComponent();
            this.drawObject = new userDrawLineMeasure(this.相机视图hWindowControl);
            this._treeViewWrapClass = new TreeViewWrapClass(this.treeView1, this);
        }
        private void CameraLineLaserCalibrateForm_Load(object sender, EventArgs e)
        {
            //SensorBase.ImageAcqComplete += new ImageAcqCompleteEventHandler(ImageAcqComplete_Event);
            BindProperty();
            //AddForm(this.运动panel, new JogMotionForm());
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
            this.programPath += "\\" + this._laserAcqSource.Sensor.Name + this._cameraAcqSource.Sensor.Name + ".txt";
            this._treeViewWrapClass.OpenProgram(this.programPath);
            this.toolStripStatusLabel1.Text = this.programPath;
        }
        private void 激光采集源comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            this._laserAcqSource = (FunctionBlock.AcqSource)this.激光传感器comboBox.SelectedItem;
            this.programPath += "\\" + this._laserAcqSource.Sensor.Name + this._cameraAcqSource.Sensor.Name + ".txt";
            this._treeViewWrapClass.OpenProgram(this.programPath);
            this.toolStripStatusLabel1.Text = this.programPath;
        }

        private void ImageAcqComplete_Event(ImageAcqCompleteEventArgs e)
        {
            double x, y, z;
            //if (!this.实时图像checkBox.Checked) return;
            if (MotionCardManage.CurrentCard != null)
            {
                MotionCardManage.CurrentCard.GetAxisPosition(MotionCardManage.CurrentCoordSys, enAxisName.X轴,  out x);
                MotionCardManage.CurrentCard.GetAxisPosition(MotionCardManage.CurrentCoordSys, enAxisName.Y轴,  out y);
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
                if (this.laserPose != null)
                {
                    string path = Application.StartupPath + "\\" + this.programPath + "\\" + this._laserAcqSource.Sensor.Name +this._cameraAcqSource.Sensor.Name  + ".txt";
                    // this._laserAcqSource.Sensor.SetParam(enSensorParamType.Coom_激光位姿, this.laserPose);
                    // this._cameraAcqSource.Sensor.CameraParam.CamParam.HomMat2D = ;
                    // HOperatorSet.WritePose(this.laserPose, path);
                    MessageBox.Show("保存成功");
                }
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
                // SensorBase.ImageAcqComplete -= new ImageAcqCompleteEventHandler(ImageAcqComplete_Event);
                this._treeViewWrapClass?.Uinit();
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
                string path = Application.StartupPath + "\\" + "laserPose" + "\\" + this._laserAcqSource.Sensor.GetParam(enSensorParamType.Coom_传感器名称).ToString() + ".dat";
                HOperatorSet.ReadPose(path, out this.laserPose);
                if (this.laserPose != null && this.laserPose.Length == 7)
                {
                    this.X平移textBox.Text = this.laserPose[0].O.ToString();
                    this.Y平移textBox.Text = this.laserPose[1].O.ToString();
                    this.Z平移textBox.Text = this.laserPose[2].O.ToString();
                    this.X轴旋转textBox.Text = this.laserPose[3].O.ToString();
                    this.Y轴旋转textBox.Text = this.laserPose[4].O.ToString();
                    this.Z轴旋转textBox.Text = this.laserPose[5].O.ToString();
                    this.位姿类型textBox.Text = this.laserPose[6].O.ToString();
                }
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
                if (this.laserPose != null)
                    this.laserPose[0] = result;
            }
        }
        private void Y平移textBox_TextChanged(object sender, EventArgs e)
        {
            double result;
            if (double.TryParse(this.Y平移textBox.Text, out result))
            {
                if (this.laserPose != null)
                    this.laserPose[1] = result;
            }
        }
        private void Z平移textBox_TextChanged(object sender, EventArgs e)
        {
            double result;
            if (double.TryParse(this.Z平移textBox.Text, out result))
            {
                if (this.laserPose != null)
                    this.laserPose[2] = result;
            }
        }
        private void X轴旋转textBox_TextChanged(object sender, EventArgs e)
        {
            double result;
            if (double.TryParse(this.X轴旋转textBox.Text, out result))
            {
                if (this.laserPose != null)
                    this.laserPose[3] = result;
            }
        }
        private void Y轴旋转textBox_TextChanged(object sender, EventArgs e)
        {
            double result;
            if (double.TryParse(this.Y轴旋转textBox.Text, out result))
            {
                if (this.laserPose != null)
                    this.laserPose[4] = result;
            }
        }

        private void 编辑工具条toolStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            ToolStripItem item = e.ClickedItem;
            string name = item.Name;
            FileOperate fo = new FileOperate();
            try
            {
                switch (name)
                {
                    case "新建NToolStripButton":
                        this._treeViewWrapClass.ClearTreeView();
                        this.toolStripStatusLabel1.Text = "";
                        break;

                    case "打开ToolStripButton":
                        this.programPath = fo.OpenFile(2);
                        if (this.programPath == null || this.programPath.Trim().Length == 0) return;
                        this._treeViewWrapClass.OpenProgram(this.programPath);
                        this.toolStripStatusLabel1.Text = this.programPath;
                        break;

                    case "保存ToolStripButton":
                        if (this.programPath == null || this.programPath.Length == 0)
                        {
                          // this.programPath = fo.SaveFile(2);
                            this.programPath += "\\" + this._laserAcqSource.Sensor.Name + this._cameraAcqSource.Sensor.Name;
                           // if (this.programPath == null || this.programPath.Length == 0) return;
                            if (this._treeViewWrapClass.SaveProgram(this.programPath)) 
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
                        this.toolStripStatusLabel1.Text = this.programPath;

                        break;

                    case "打印PToolStripButton":

                        break;

                    case "剪切UToolStripButton":

                        break;

                    case "复制CToolStripButton":

                        break;

                    case "粘贴PToolStripButton":

                        break;

                    case "帮助LToolStripButton":

                        break;

                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void Z轴旋转textBox_TextChanged(object sender, EventArgs e)
        {
            double result;
            if (double.TryParse(this.Z轴旋转textBox.Text, out result))
            {
                if (this.laserPose != null)
                    this.laserPose[5] = result;
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

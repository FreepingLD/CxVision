
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
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using View;

namespace FunctionBlock
{
    public partial class RectangleScanForm : Form
    {
        private CancellationTokenSource cts;
        private Form form;
        private IFunction _function;
        private VisualizeView drawObject;
        private VisualizeView drawModelObject3D;
        private bool isFormClose = false;
        public RectangleScanForm(IFunction function,TreeNode node)
        {
            this._function = function;
            InitializeComponent();
            drawObject = new VisualizeView(this.hWindowControl1, true);
            new ListBoxWrapClass().InitListBox(this.listBox1, node, 2);
            //new DataGridViewWrapClass().InitDataGridView(this, this._function, this.dataGridView1, ((FunctionBlock.RectangleScanAcq)this._function).Coord1Table);
        }
        private void RectangleScanForm_Load(object sender, EventArgs e)
        {
            BaseFunction.PointsCloudAcqComplete += new PointCloudAcqCompleteEventHandler(PointCloudAcqComplete_Event);
            //SensorBase.ImageAcqComplete += new ImageAcqCompleteEventHandler(ImageAcqComplete_Event);
            BindProperty();
            AddForm(this.运动panel, new JogMotionForm());
        }
        private void BindProperty()
        {
            try
            {
                ////////////////
                this.dataGridView1.DataSource = ((FunctionBlock.RectangleScanAcq)this._function).AcqCoordRect2;
                this.激光采集源comboBox.DataSource = SensorManage.GetLaserSensorName();// FunctionBlock.AcqSourceManage.LaserAcqSourceList;//SensorManage.LaserList;

                this.相机采集源comboBox.DataSource = SensorManage.GetCamSensorName();// FunctionBlock.AcqSourceManage.CamAcqSourceList;// SensorManage.CameraList;

                //////////////////
                this.加速度时间numericUpDown.DataBindings.Add("Value", (FunctionBlock.RectangleScanAcq)_function, "Tacc", true, DataSourceUpdateMode.OnPropertyChanged); //   
                this.减速度时间numericUpDown.DataBindings.Add("Value", (FunctionBlock.RectangleScanAcq)_function, "Tdec", true, DataSourceUpdateMode.OnPropertyChanged); // 
                this.激光采集源comboBox.DataBindings.Add("SelectedItem", ((FunctionBlock.RectangleScanAcq)this._function), "LaserSensorName", true, DataSourceUpdateMode.OnPropertyChanged);
                this.相机采集源comboBox.DataBindings.Add("SelectedItem", ((FunctionBlock.RectangleScanAcq)this._function), "CamSensorName", true, DataSourceUpdateMode.OnPropertyChanged);
                this.Y轴向量numericUpDown.DataBindings.Add("Value", (FunctionBlock.RectangleScanAcq)this._function, "Vector_y", true, DataSourceUpdateMode.OnPropertyChanged);
                this.X轴向量numericUpDown.DataBindings.Add("Value", (FunctionBlock.RectangleScanAcq)_function, "Vector_x", true, DataSourceUpdateMode.OnPropertyChanged); //   
                this.行数量numericUpDown.DataBindings.Add("Value", (FunctionBlock.RectangleScanAcq)_function, "LineCount", true, DataSourceUpdateMode.OnPropertyChanged); //   
                this.插补模式comboBox.DataBindings.Add("Text", (FunctionBlock.RectangleScanAcq)_function, "InterpolateMode", true, DataSourceUpdateMode.OnPropertyChanged); //  
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
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
        private void AddForm(GroupBox groupBox, Form form)
        {
            if (groupBox == null) return;
            if (groupBox.Controls.Count > 0)
                groupBox.Controls.Clear();
            if (form == null) return;
            form.TopLevel = false;
            form.FormBorderStyle = FormBorderStyle.None;
            form.MaximizeBox = false;
            form.MinimizeBox = false;
            form.Dock = DockStyle.Fill;
            groupBox.Controls.Add(form);
            form.Show();
        }
        public enum enShowItems
        {
            距离1对象,
            距离2对象,
            厚度对象,
        }
        private void 显示条目comboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
           enShowItems item;
            Enum.TryParse(this.显示条目comboBox.Text.Trim(), out item);
        }
        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            try
            {
                switch (e.ClickedItem.Text)
                {
                    case "执行":
                        if (this.toolStripStatusLabel2.Text == "等待……") break;
                        this.toolStripStatusLabel2.Text = "等待……";
                        this.toolStripStatusLabel2.ForeColor = Color.Yellow;
                        this.cts = new CancellationTokenSource();
                        Task.Run(() =>
                        {
                            if (this._function.Execute(null).Succss)
                            {
                                if (!this.cts.IsCancellationRequested)
                                {
                                    this.Invoke(new Action(() =>
                                    {
                                        this.toolStripStatusLabel1.Text = "执行结果:";
                                        this.toolStripStatusLabel2.Text = "成功";
                                        this.toolStripStatusLabel2.ForeColor = Color.Green;
                                    }));
                                }
                            }
                            else
                            {
                                if (!this.cts.IsCancellationRequested)
                                {
                                    this.Invoke(new Action(() =>
                                   {
                                       this.toolStripStatusLabel1.Text = "执行结果:";
                                       this.toolStripStatusLabel2.Text = "失败";
                                       this.toolStripStatusLabel2.ForeColor = Color.Red;
                                   }));
                                }
                            }
                        }
                        );
                        break;
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
            }
        }
        private void 视图工具toolStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            AlgorithmsLibrary.HalconLibrary ha = new AlgorithmsLibrary.HalconLibrary();
            string name = e.ClickedItem.Name;
            switch (name)
            {
                case "toolStripButton_Clear":
                    this.drawModelObject3D.ClearWindow();
                    this.toolStripButton_Translate.CheckState = CheckState.Unchecked;
                    break;
                case "toolStripButton_Select":
                    this.drawModelObject3D.Select();
                    this.toolStripButton_Translate.CheckState = CheckState.Unchecked;
                    break;
                case "toolStripButton_Translate":
                    this.drawModelObject3D.TranslateScaleImage();
                    this.toolStripButton_Translate.CheckState = CheckState.Checked;
                    break;
                case "toolStripButton_Auto":
                    this.drawModelObject3D.AutoImage();
                    this.toolStripButton_Translate.CheckState = CheckState.Unchecked;

                    break;
                case "toolStripButton_3D":
                    this.toolStripButton_Translate.CheckState = CheckState.Unchecked;

                    break;
                default:
                    break;
            }
        }

        private void PointCloudAcqComplete_Event(object send, PointCloudAcqCompleteEventArgs e)
        {
            //if (((FunctionBlock.RectangleScanAcq)this._function).LaserAcqSource == null) return;
            //string name = ((FunctionBlock.RectangleScanAcq)this._function).LaserAcqSource.Sensor.ConfigParam.SensorName;
            //switch (e.SensorName.Split('(')[0])
            //{
            //    case "读取3D对象":
            //        if (this.drawModelObject3D != null)
            //            this.drawModelObject3D.PointCloudModel3D = new HObjectModel3D[] { e.PointsCloudData };
            //        break;
            //    default:
            //        if (name == e.SensorName) // 因为是用同一个事件来发送图像，所以这里使用名字来判断是不是同一个相机发送的
            //        {
            //            if (this.drawModelObject3D != null)
            //                this.drawModelObject3D.PointCloudModel3D = new HObjectModel3D[] { e.PointsCloudData };
            //        }
            //        break;
            //}
        }
        private void ImageAcqComplete_Event(ImageAcqCompleteEventArgs e)
        {
        //    double x, y, z;
        //    if (((FunctionBlock.RectangleScanAcq)this._function).CamAcqSource == null) return;
        //    string name = ((FunctionBlock.RectangleScanAcq)this._function).CamAcqSource.Sensor.ConfigParam.SensorName;
        //    switch (e.CamName.Split('(')[0])
        //    {
        //        case "读取图像":
        //        case "图像采集":
        //            if (this.drawObject != null)
        //                this.drawObject.BackImage = e.ImageData;
        //            break;
        //        default:
        //            if (name == e.CamName) // 因为是用同一个事件来发送图像，所以这里使用名字来判断是不是同一个相机发送的
        //            {
        //                if (((FunctionBlock.RectangleScanAcq)this._function).CamAcqSource != null)
        //                {
        //                    ((FunctionBlock.RectangleScanAcq)this._function).CamAcqSource.GetAxisPosition(enAxisName.X轴, out x);
        //                    ((FunctionBlock.RectangleScanAcq)this._function).CamAcqSource.GetAxisPosition(enAxisName.Y轴, out y);
        //                    ((FunctionBlock.RectangleScanAcq)this._function).CamAcqSource.GetAxisPosition(enAxisName.Z轴,out z);
        //                    e.ImageData.Grab_X = x;
        //                    e.ImageData.Grab_Y = y;
        //                    e.ImageData.Grab_Z = z;
        //                }
        //                if (this.drawObject != null)
        //                    this.drawObject.BackImage = e.ImageData;
        //            }
        //            break;
        //    }
        }

        private void LaserLineScanForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (cts != null)
                    cts.Cancel();
                BaseFunction.PointsCloudAcqComplete -= new PointCloudAcqCompleteEventHandler(PointCloudAcqComplete_Event);
                //SensorBase.ImageAcqComplete -= new ImageAcqCompleteEventHandler(ImageAcqComplete_Event);
            }
            catch
            {

            }
        }


        private void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            //try
            //{
            //    HTuple Qx, Qy;
            //    userWcsLine wcsLine = new userWcsLine();
            //    this.drawObject.AttachPropertyData.Clear();
            //    if (((RectangleScanAcq)this._function).LaserAcqSource == null) return;
            //    userWcsCoordSystem userWcsCoordSystem = ((RectangleScanAcq)_function).extractRefSource2Data();
            //    userWcsPose3D  laserAffinePose = ((RectangleScanAcq)this._function).LaserAcqSource.Sensor.LaserParam.LaserCalibrationParam.LaserAffinePose;
            //    ///////////////////////////////////////////////////////////////////
            //    this.drawObject.AttachPropertyData.Add(userWcsCoordSystem);
            //    DataGridViewCellCollection dataGridViewCellCollection;
            //    for (int i = 0; i < this.dataGridView1.Rows.Count; i++)
            //    {
            //        this.dataGridView1.Rows[i].HeaderCell.Value = i.ToString();
            //        dataGridViewCellCollection = this.dataGridView1.Rows[i].Cells;
            //        if (dataGridViewCellCollection.Count > 0)
            //            wcsLine.x1 = Convert.ToDouble(dataGridViewCellCollection[0].Value);
            //        if (dataGridViewCellCollection.Count > 1)
            //            wcsLine.y1 = Convert.ToDouble(dataGridViewCellCollection[1].Value);
            //        if (dataGridViewCellCollection.Count > 2)
            //            wcsLine.z1 = Convert.ToDouble(dataGridViewCellCollection[2].Value);
            //        if (dataGridViewCellCollection.Count > 3)
            //            wcsLine.u1 = Convert.ToDouble(dataGridViewCellCollection[3].Value);
            //        if (dataGridViewCellCollection.Count > 4)
            //            wcsLine.v1 = Convert.ToDouble(dataGridViewCellCollection[4].Value);
            //        if (dataGridViewCellCollection.Count > 5)
            //            wcsLine.theta1 = Convert.ToDouble(dataGridViewCellCollection[5].Value);
            //        ////////////////////////
            //        if (dataGridViewCellCollection.Count > 6)
            //            wcsLine.x2 = Convert.ToDouble(dataGridViewCellCollection[6].Value);
            //        if (dataGridViewCellCollection.Count > 7)
            //            wcsLine.y2 = Convert.ToDouble(dataGridViewCellCollection[7].Value);
            //        if (dataGridViewCellCollection.Count > 8)
            //            wcsLine.z2 = Convert.ToDouble(dataGridViewCellCollection[8].Value);
            //        if (dataGridViewCellCollection.Count > 9)
            //            wcsLine.u2 = Convert.ToDouble(dataGridViewCellCollection[9].Value);
            //        if (dataGridViewCellCollection.Count > 10)
            //            wcsLine.v2 = Convert.ToDouble(dataGridViewCellCollection[10].Value);
            //        if (dataGridViewCellCollection.Count > 11)
            //            wcsLine.theta2 = Convert.ToDouble(dataGridViewCellCollection[11].Value);
            //        // 将点绘制到图像上                  
            //        HOperatorSet.AffineTransPoint2d(userWcsCoordSystem.GetVariationHomMat2D(), new HTuple(wcsLine.x1 - laserAffinePose.Tx, wcsLine.x2 - laserAffinePose.Tx), new HTuple(wcsLine.y1 - laserAffinePose.Ty, wcsLine.y2 - laserAffinePose.Ty), out Qx, out Qy);
            //        wcsLine.x1 = Qx[0].D;
            //        wcsLine.y1 = Qy[0].D;
            //        wcsLine.x2 = Qx[1].D;
            //        wcsLine.y2 = Qy[1].D;
            //        this.drawObject.AttachPropertyData.Add(wcsLine);
            //    }
            //    /////////////////////
            //    if (this.drawObject.BackImage == null)
            //        this.drawObject.UpdataGraphicView();
            //    else
            //        this.drawObject.ShowAttachProperty();
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.ToString());
            //}
        }

        private void 激光采集源comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            //try
            //{
            //    if (this.激光采集源comboBox.SelectedIndex == -1) return;
            //    this.dataGridView1.Tag = this.激光采集源comboBox.SelectedItem;// ((LaserScanAcq)this._function).LaserAcqSource; //用于右键移动到相机位置 
            //    //this.dataGridView2.Tag = this.激光采集源comboBox.SelectedItem;//((LaserScanAcq)this._function).LaserAcqSource; //用于右键移动 
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.ToString());
            //}
        }


        private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false;
        }


        private void 图像取线button_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    if (((RectangleScanAcq)this._function).CamAcqSource == null) return;
            //    double x_Coord = 0, y_Coord = 0, z_Coord = 0, u_Coord = 0, v_Coord = 0, w_Coord = 0;
            //    userWcsLine wcsLine;
            //    double Qx1, Qy1, Qz1, Qx2, Qy2, Qz2;
            //    if (((RectangleScanAcq)this._function).CamAcqSource != null) throw new ArgumentNullException("CamAcqSource");
            //    ////////////////////////////////////
            //    this.hWindowControl1.Cursor = Cursors.Cross;
            //    this.drawObject.DrawWcsLineOnWindow(enColor.white, 0, 0, out wcsLine);
            //    this.hWindowControl1.Cursor = Cursors.Default;
            //    if (((RectangleScanAcq)this._function).CamAcqSource.Card != null)
            //    {
            //        ((RectangleScanAcq)this._function).CamAcqSource.GetAxisPosition(enAxisName.X轴, out x_Coord);
            //        ((RectangleScanAcq)this._function).CamAcqSource.GetAxisPosition(enAxisName.Y轴,  out y_Coord);
            //        ((RectangleScanAcq)this._function).CamAcqSource.GetAxisPosition(enAxisName.Z轴,  out z_Coord);
            //        ((RectangleScanAcq)this._function).CamAcqSource.GetAxisPosition(enAxisName.U轴,  out u_Coord);
            //        ((RectangleScanAcq)this._function).CamAcqSource.GetAxisPosition(enAxisName.V轴,  out v_Coord);
            //        ((RectangleScanAcq)this._function).CamAcqSource.GetAxisPosition(enAxisName.W轴,  out w_Coord);
            //    }
            //    ////////////////////////////
            //    wcsLine.x1 = wcsLine.x1 + x_Coord;
            //    wcsLine.y1 = wcsLine.y1 + y_Coord;
            //    wcsLine.z1 = wcsLine.z1 + z_Coord;
            //    wcsLine.u1 = u_Coord;
            //    wcsLine.v1 = v_Coord;
            //    wcsLine.theta1 = w_Coord;
            //    wcsLine.x2 = wcsLine.x2 + x_Coord;
            //    wcsLine.y2 = wcsLine.y2 + y_Coord;
            //    wcsLine.z2 = wcsLine.z2 + z_Coord;
            //    wcsLine.u2 = u_Coord;
            //    wcsLine.v2 = v_Coord;
            //    wcsLine.theta2 = w_Coord;
            //    wcsLine.CamParams = this.drawObject.CameraParam;
            //    //wcsLine.camPose = this.drawObject.CamPose;
            //    ///////////////////////////////
            //    ((RectangleScanAcq)_function).Coord1Table.Rows.Add(wcsLine.x1, wcsLine.y1, wcsLine.z1, u_Coord, v_Coord, w_Coord, wcsLine.x2, wcsLine.y2, wcsLine.z2, u_Coord, v_Coord, w_Coord);
            //    this.drawObject.AttachPropertyData.Add(wcsLine);
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.ToString());
            //}
        }

        private void 添加点button_Click(object sender, EventArgs e)
        {
            try
            {
                double X1 = 0, Y1 = 0, Z1 = 0, U1 = 0, V1 = 0, W1 = 0, X2 = 0, Y2 = 0, Z2 = 0, U2 = 0, V2 = 0, W2 = 0;
                double.TryParse(this.X1坐标textBox.Text, out X1);
                double.TryParse(this.Y1坐标textBox.Text, out Y1);
                double.TryParse(this.Z1坐标textBox.Text, out Z1);
                double.TryParse(this.U1坐标textBox.Text, out U1);
                double.TryParse(this.V1坐标textBox.Text, out V1);
                double.TryParse(this.W1坐标textBox.Text, out W1);
                /////////////////////////////////
                double.TryParse(this.X2坐标textBox.Text, out X2);
                double.TryParse(this.Y2坐标textBox.Text, out Y2);
                double.TryParse(this.Z2坐标textBox.Text, out Z2);
                double.TryParse(this.U2坐标textBox.Text, out U2);
                double.TryParse(this.V2坐标textBox.Text, out V2);
                double.TryParse(this.W2坐标textBox.Text, out W2);
                //if (((LaserScanAcq)this._function).LaserAcqSource.Card != null)
                //{
                //    ((LaserScanAcq)this._function).LaserAcqSource.Card.GetAxisCurentPosition(enAxisName.U轴, enCoordType.机台坐标, out U);
                //    ((LaserScanAcq)this._function).LaserAcqSource.Card.GetAxisCurentPosition(enAxisName.V轴, enCoordType.机台坐标, out V);
                //    ((LaserScanAcq)this._function).LaserAcqSource.Card.GetAxisCurentPosition(enAxisName.W轴, enCoordType.机台坐标, out W);
                //}
                //((RectangleScanAcq)_function).AcqCoordRect2.Add(X1, Y1, Z1, U1, V1, W1, X2, Y2, Z2, U2, V2, W2);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void 删除点button_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.dataGridView1.CurrentRow == null) return;
                int index = this.dataGridView1.CurrentRow.Index;
                ((RectangleScanAcq)_function).AcqCoordRect2.RemoveAt(index);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void 清空点button_Click(object sender, EventArgs e)
        {
            try
            {
                ((RectangleScanAcq)_function).AcqCoordRect2.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void 图像取点1button_Click_1(object sender, EventArgs e)
        {
            try
            {
                //double x_Coord = 0, y_Coord = 0, z_Coord = 0, u_Coord = 0, v_Coord = 0, w_Coord = 0;
                //userWcsPoint wcsPoint;
                //double Qx, Qy, Qz;
                //AlgorithmsLibrary.HalconLibrary ha = new AlgorithmsLibrary.HalconLibrary();
                //if (((RectangleScanAcq)this._function).CamAcqSource != null) throw new ArgumentNullException("CamAcqSource");
                //////////////////////////////////////
                //this.hWindowControl1.Cursor = Cursors.Cross;
                //this.drawObject.DrawWcsPointOnWindow(enColor.white, 0, 0, out wcsPoint);
                ////ha.DrawPointOnWindow(this.hWindowControl1.HalconWindow, this.drawObject.CamParam, this.drawObject.CamPose, out wcsPoint);
                //this.hWindowControl1.Cursor = Cursors.Default;
                //if (((RectangleScanAcq)this._function).CamAcqSource.Card != null)
                //{
                //    ((RectangleScanAcq)this._function).CamAcqSource.GetAxisPosition(enAxisName.X轴, out x_Coord);
                //    ((RectangleScanAcq)this._function).CamAcqSource.GetAxisPosition(enAxisName.Y轴,  out y_Coord);
                //    ((RectangleScanAcq)this._function).CamAcqSource.GetAxisPosition(enAxisName.Z轴,  out z_Coord);
                //    ((RectangleScanAcq)this._function).CamAcqSource.GetAxisPosition(enAxisName.U轴,  out u_Coord);
                //    ((RectangleScanAcq)this._function).CamAcqSource.GetAxisPosition(enAxisName.V轴, out v_Coord);
                //    ((RectangleScanAcq)this._function).CamAcqSource.GetAxisPosition(enAxisName.W轴, out w_Coord);
                //}
                //wcsPoint.x += x_Coord;
                //wcsPoint.y += y_Coord;
                //wcsPoint.z += z_Coord;
                //wcsPoint.refPoint_x += x_Coord;
                //wcsPoint.refPoint_y += y_Coord;
                /////////////////////////
                //this.X1坐标textBox.Text = wcsPoint.x.ToString();
                //this.Y1坐标textBox.Text = wcsPoint.y.ToString();
                //this.Z1坐标textBox.Text = wcsPoint.z.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void 图像取点2button_Click_1(object sender, EventArgs e)
        {
            try
            {
                //double x_Coord = 0, y_Coord = 0, z_Coord = 0, u_Coord = 0, v_Coord = 0, w_Coord = 0;
                //userWcsPoint wcsPoint;
                //double Qx, Qy, Qz;
                //if (((RectangleScanAcq)this._function).LaserAcqSource != null) throw new ArgumentNullException("CamAcqSource");
                //////////////////////////////////////
                //this.hWindowControl1.Cursor = Cursors.Cross;
                //this.drawObject.DrawWcsPointOnWindow(enColor.white, 0, 0, out wcsPoint);
                //this.hWindowControl1.Cursor = Cursors.Default;
                //if (((RectangleScanAcq)this._function).CamAcqSource.Card != null)
                //{
                //    ((RectangleScanAcq)this._function).CamAcqSource.GetAxisPosition(enAxisName.X轴,  out x_Coord);
                //    ((RectangleScanAcq)this._function).CamAcqSource.GetAxisPosition(enAxisName.Y轴,  out y_Coord);
                //    ((RectangleScanAcq)this._function).CamAcqSource.GetAxisPosition(enAxisName.Z轴,  out z_Coord);
                //    ((RectangleScanAcq)this._function).CamAcqSource.GetAxisPosition(enAxisName.U轴,  out u_Coord);
                //    ((RectangleScanAcq)this._function).CamAcqSource.GetAxisPosition(enAxisName.V轴, out v_Coord);
                //    ((RectangleScanAcq)this._function).CamAcqSource.GetAxisPosition(enAxisName.W轴,  out w_Coord);
                //}
                //wcsPoint.x += x_Coord;
                //wcsPoint.y += y_Coord;
                //wcsPoint.z += z_Coord;
                //wcsPoint.refPoint_x += x_Coord;
                //wcsPoint.refPoint_y += y_Coord;
                /////////////////////////     
                //this.X2坐标textBox.Text = wcsPoint.x.ToString();
                //this.Y2坐标textBox.Text = wcsPoint.y.ToString();
                //this.Z2坐标textBox.Text = wcsPoint.z.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void 当前位置1button_Click(object sender, EventArgs e)
        {
            try
            {
                //double X = 0, Y = 0, Z = 0, U = 0, V = 0, W = 0;
                //if (((RectangleScanAcq)_function).LaserAcqSource != null)
                //{
                //    ((RectangleScanAcq)_function).LaserAcqSource.GetAxisPosition(enAxisName.X轴, out X);
                //    ((RectangleScanAcq)_function).LaserAcqSource.GetAxisPosition(enAxisName.Y轴, out Y);
                //    ((RectangleScanAcq)_function).LaserAcqSource.GetAxisPosition(enAxisName.Z轴, out Z);
                //    ((RectangleScanAcq)_function).LaserAcqSource.GetAxisPosition(enAxisName.U轴,out U);
                //    ((RectangleScanAcq)_function).LaserAcqSource.GetAxisPosition(enAxisName.V轴,out V);
                //    ((RectangleScanAcq)_function).LaserAcqSource.GetAxisPosition(enAxisName.W轴,out W);
                //}
                //this.X1坐标textBox.Text = X.ToString();
                //this.Y1坐标textBox.Text = Y.ToString();
                //this.Z1坐标textBox.Text = Z.ToString();
                //this.U1坐标textBox.Text = U.ToString();
                //this.V1坐标textBox.Text = V.ToString();
                //this.W1坐标textBox.Text = W.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void 当前位置2button_Click(object sender, EventArgs e)
        {
            try
            {
                //double X = 0, Y = 0, Z = 0, U = 0, V = 0, W = 0;
                //if (((FunctionBlock.RectangleScanAcq)_function).LaserAcqSource != null)
                //{
                //    ((FunctionBlock.RectangleScanAcq)_function).LaserAcqSource.GetAxisPosition(enAxisName.X轴, out X);
                //    ((FunctionBlock.RectangleScanAcq)_function).LaserAcqSource.GetAxisPosition(enAxisName.Y轴,out Y);
                //    ((FunctionBlock.RectangleScanAcq)_function).LaserAcqSource.GetAxisPosition(enAxisName.Z轴,out Z);
                //    ((FunctionBlock.RectangleScanAcq)_function).LaserAcqSource.GetAxisPosition(enAxisName.U轴,out U);
                //    ((FunctionBlock.RectangleScanAcq)_function).LaserAcqSource.GetAxisPosition(enAxisName.V轴,out V);
                //    ((FunctionBlock.RectangleScanAcq)_function).LaserAcqSource.GetAxisPosition(enAxisName.W轴,out W);
                //}
                //this.X2坐标textBox.Text = X.ToString();
                //this.Y2坐标textBox.Text = Y.ToString();
                //this.Z2坐标textBox.Text = Z.ToString();
                //this.U2坐标textBox.Text = U.ToString();
                //this.V2坐标textBox.Text = V.ToString();
                //this.W2坐标textBox.Text = W.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.dataGridView1.CurrentRow == null) return;
                userWcsLine wcsLine;
                int index = this.dataGridView1.CurrentRow.Index;
                for (int i = 0; i < this.drawObject.AttachPropertyData.Count; i++)
                {
                    if (i == index + 1)
                    {
                        if (this.drawObject.AttachPropertyData[i] is userWcsLine)
                        {
                            wcsLine = (userWcsLine)this.drawObject.AttachPropertyData[i];
                            wcsLine.Color = enColor.orange;
                            this.drawObject.AttachPropertyData[i] = wcsLine;
                        }
                    }
                    else
                    {
                        if (this.drawObject.AttachPropertyData[i] is userWcsLine)
                        {
                            wcsLine = (userWcsLine)this.drawObject.AttachPropertyData[i];
                            wcsLine.Color = enColor.white;
                            this.drawObject.AttachPropertyData[i] = wcsLine;
                        }
                    }
                }
                /////////////////////
                if (this.drawObject.BackImage == null)
                    this.drawObject.UpdataGraphicView();
                else
                    this.drawObject.ShowAttachProperty();
            }
            catch (Exception ee)
            {
                MessageBox.Show("dataGridView1_SelectionChanged操作失败" + ee.ToString());
            }
        }



    }
}

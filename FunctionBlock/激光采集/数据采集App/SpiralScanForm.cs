
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
    public partial class SpiralScanForm : Form
    {
        private CancellationTokenSource cts;
        private Form form;
        private IFunction _function;
        private VisualizeView drawImageObject;
        private VisualizeView drawModelObject3D;
        private FunctionBlock.AcqSource _laserAcqSource = null;
        public SpiralScanForm(IFunction function, TreeNode node)
        {
            this._function = function;
            InitializeComponent();
            this.Text = ((FunctionBlock.SpiralScanAcq)this._function).Name;
            this.drawImageObject = new VisualizeView(this.相机hWindowControl, true);
            this.drawModelObject3D = new VisualizeView(this.激光hWindowControl);
            new ListBoxWrapClass().InitListBox(this.listBox1, node, 2);
            //new DataGridViewWrapClass().InitDataGridView(this, this._function, this.dataGridView1, ((FunctionBlock.SpiralScanAcq)this._function).AcqCoordPoint);
        }
        private void SpiralScanForm_Load(object sender, EventArgs e)
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
                this.dataGridView1.DataSource = ((FunctionBlock.SpiralScanAcq)this._function).AcqCoordPoint;
                this.激光采集源comboBox.DataSource = SensorManage.GetLaserSensorName();// FunctionBlock.AcqSourceManage.LaserAcqSourceList;//  SensorManage.LaserList; 
                this.相机采集源comboBox.DataSource = SensorManage.GetCamSensorName();// AcqSourceManage.CamAcqSourceList;//  SensorManage.CameraList;
                this.运动轴comboBox.DataSource = Enum.GetNames(typeof(enAxisName));
                //////////////////
                this.激光采集源comboBox.DataBindings.Add("SelectedItem", ((SpiralScanAcq)this._function), "LaserSensorName", true, DataSourceUpdateMode.OnPropertyChanged);
                this.相机采集源comboBox.DataBindings.Add("SelectedItem", ((SpiralScanAcq)this._function), "CamSensorName", true, DataSourceUpdateMode.OnPropertyChanged);
                this.运动轴comboBox.DataBindings.Add("Text", (SpiralScanAcq)this._function, "MoveAxis", true, DataSourceUpdateMode.OnPropertyChanged);
                this.圆弧方向comboBox.DataBindings.Add("Text", (SpiralScanAcq)this._function, "CircleDir", true, DataSourceUpdateMode.OnPropertyChanged);
                this.圆弧数量numericUpDown.DataBindings.Add("Value", (SpiralScanAcq)_function, "CircleCount", true, DataSourceUpdateMode.OnPropertyChanged); //
                this.圆弧半径numericUpDown.DataBindings.Add("Value", (SpiralScanAcq)_function, "CircleRadius", true, DataSourceUpdateMode.OnPropertyChanged); //
                this.Z向偏移numericUpDown.DataBindings.Add("Value", (SpiralScanAcq)_function, "Offset_z", true, DataSourceUpdateMode.OnPropertyChanged); //   
                this.加速度时间numericUpDown.DataBindings.Add("Value", (SpiralScanAcq)_function, "Tacc", true, DataSourceUpdateMode.OnPropertyChanged); //   
                this.减速度时间numericUpDown.DataBindings.Add("Value", (SpiralScanAcq)_function, "Tdec", true, DataSourceUpdateMode.OnPropertyChanged); //   StartPointOffset
                this.起点偏移numericUpDown.DataBindings.Add("Value", (SpiralScanAcq)_function, "StartPointOffset", true, DataSourceUpdateMode.OnPropertyChanged); //   
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
            }
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
            switch (item)
            {
                case enShowItems.距离1对象:
                    this.drawModelObject3D.PointCloudModel3D = new PointCloudData((HObjectModel3D)this._function.GetPropertyValues("距离1对象"));
                    break;
                case enShowItems.距离2对象:
                    this.drawModelObject3D.PointCloudModel3D = new PointCloudData((HObjectModel3D)this._function.GetPropertyValues("距离2对象"));
                    break;
                case enShowItems.厚度对象:
                    this.drawModelObject3D.PointCloudModel3D = new PointCloudData((HObjectModel3D)this._function.GetPropertyValues("厚度对象"));
                    break;
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
        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            try
            {
                switch (e.ClickedItem.Text)
                {
                    case "执行":
                        if (this.运行结果toolStripStatusLabel.Text == "等待……") break;
                        this.运行结果toolStripStatusLabel.Text = "等待……";
                        this.运行结果toolStripStatusLabel.ForeColor = Color.Yellow;
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
                                        this.运行结果toolStripStatusLabel.Text = "成功";
                                        this.运行结果toolStripStatusLabel.ForeColor = Color.Green;
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
                                    this.运行结果toolStripStatusLabel.Text = "失败";
                                    this.运行结果toolStripStatusLabel.ForeColor = Color.Red;
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

        private void PointCloudAcqComplete_Event(object send, PointCloudAcqCompleteEventArgs e)
        {
            if (this._laserAcqSource == null) return;
            string name = this._laserAcqSource.Sensor.ConfigParam.SensorName;
            switch (e.SensorName.Split('(')[0])
            {
                case "读取3D对象":
                    if (this.drawModelObject3D != null)
                        this.drawModelObject3D.PointCloudModel3D = new PointCloudData(e.PointsCloudData);
                    break;
                default:
                    if (name == e.SensorName) // 因为是用同一个事件来发送图像，所以这里使用名字来判断是不是同一个相机发送的
                    {
                        if (this.drawModelObject3D != null)
                            this.drawModelObject3D.PointCloudModel3D = new PointCloudData(e.PointsCloudData);
                    }
                    break;
            }
        }
        private void ImageAcqComplete_Event(ImageAcqCompleteEventArgs e)
        {
            //double x, y, z;
            //if (((SpiralScanAcq)this._function).CamAcqSource == null) return;
            //string name = ((SpiralScanAcq)this._function).CamAcqSource.Sensor.ConfigParam.SensorName;
            //switch (e.CamName.Split('(')[0])
            //{
            //    case "读取图像":
            //    case "图像采集":
            //        if (this.drawImageObject != null)
            //            this.drawImageObject.BackImage = e.ImageData;
            //        break;
            //    default:
            //        if (name == e.CamName) // 因为是用同一个事件来发送图像，所以这里使用名字来判断是不是同一个相机发送的
            //        {
            //            if (((SpiralScanAcq)this._function).CamAcqSource.Card != null)
            //            {
            //                ((SpiralScanAcq)this._function).CamAcqSource.GetAxisPosition(enAxisName.X轴, out x);
            //                ((SpiralScanAcq)this._function).CamAcqSource.GetAxisPosition(enAxisName.Y轴, out y);
            //                ((SpiralScanAcq)this._function).CamAcqSource.GetAxisPosition(enAxisName.Z轴, out z);
            //                e.ImageData.Grab_X = x;
            //                e.ImageData.Grab_Y = y;
            //                e.ImageData.Grab_Z = z;
            //            }
            //            if (this.drawImageObject != null)
            //                this.drawImageObject.BackImage = e.ImageData;
            //        }
            //        break;
            //}
        }

        private void SpiralScanForm_FormClosing(object sender, FormClosingEventArgs e)
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

        private void 添加点Button_Click(object sender, EventArgs e)
        {
            try
            {
                //double x_Coord, y_Coord, z_Coord, u_Coord, v_Coord, w_Coord;
                //if (((SpiralScanAcq)_function).LaserAcqSource == null)
                //{
                //    throw new ArgumentNullException("LaserAcqSource");
                //}
                //((SpiralScanAcq)_function).LaserAcqSource.GetAxisPosition(enAxisName.X轴, out x_Coord);
                //((SpiralScanAcq)_function).LaserAcqSource.GetAxisPosition(enAxisName.Y轴, out y_Coord);
                //((SpiralScanAcq)_function).LaserAcqSource.GetAxisPosition(enAxisName.Z轴, out z_Coord); // 为相机在机台中的坐标
                //((SpiralScanAcq)_function).LaserAcqSource.GetAxisPosition(enAxisName.U轴, out u_Coord);
                //((SpiralScanAcq)_function).LaserAcqSource.GetAxisPosition(enAxisName.V轴, out v_Coord);
                //((SpiralScanAcq)_function).LaserAcqSource.GetAxisPosition(enAxisName.W轴, out w_Coord); // 为相机在机台中的坐
                ////////////////////////////////
                //((SpiralScanAcq)_function).AcqCoordPoint.Rows.Add(new object[] { x_Coord, y_Coord, z_Coord, u_Coord, v_Coord, w_Coord });
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void 删除点Button_Click(object sender, EventArgs e)
        {
            if (this.dataGridView1.CurrentRow == null) return;
            try
            {
                int index = this.dataGridView1.CurrentRow.Index;
                ((SpiralScanAcq)_function).AcqCoordPoint.RemoveAt(index);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void 清空点Button_Click(object sender, EventArgs e)
        {
            try
            {
                ((SpiralScanAcq)this._function).AcqCoordPoint.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void 图像取点button_Click(object sender, EventArgs e)
        {
            try
            {
                //double x_Coord, y_Coord, z_Coord, u_Coord, v_Coord, w_Coord;
                //userWcsPoint wcsPoint;
                //double Qx, Qy, Qz;
                //////////////////////////////////////
                //this.相机hWindowControl.Cursor = Cursors.Cross;
                //this.drawImageObject.DrawWcsPointOnWindow(enColor.white, 0, 0, out wcsPoint);
                //this.相机hWindowControl.Cursor = Cursors.Default;

                //wcsPoint.x += 0;
                //wcsPoint.y += 0;
                //wcsPoint.z += 0;
                //wcsPoint.refPoint_x = x_Coord;
                //wcsPoint.refPoint_y = y_Coord;
                //wcsPoint.CamParams = this.drawImageObject.CameraParam;
                ////wcsPoint.camPose = this.drawImageObject.CamPose;
                //this.drawImageObject.AttachPropertyData.Add(wcsPoint);
                /////////////////////////////////
                //((SpiralScanAcq)_function).AcqCoordPoint.Add(new object[] { wcsPoint.x, wcsPoint.y, wcsPoint.z, u_Coord, v_Coord, w_Coord });
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void 视图工具toolStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            //HalconLibrary ha = new HalconLibrary();
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

        private void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            try
            {
                HTuple Qx, Qy;
                userWcsPoint wcsPoint = new userWcsPoint();
                userWcsCoordSystem userWcsCoordSystem = ((SpiralScanAcq)_function).extractRefSource2Data();
                this.drawImageObject.AttachPropertyData.Clear();
                this.drawImageObject.AttachPropertyData.Add(userWcsCoordSystem);
                DataGridViewCellCollection dataGridViewCellCollection;
                for (int i = 0; i < this.dataGridView1.Rows.Count; i++)
                {
                    this.dataGridView1.Rows[i].HeaderCell.Value = i.ToString();
                    dataGridViewCellCollection = this.dataGridView1.Rows[i].Cells;
                    ////////////////////////////////////////
                    wcsPoint.CamParams = this.drawImageObject.CameraParam;
                    //wcsPoint.camPose = this.drawImageObject.CamPose;
                    if (dataGridViewCellCollection.Count > 0)
                        wcsPoint.X = Convert.ToDouble(dataGridViewCellCollection[0].Value);
                    if (dataGridViewCellCollection.Count > 1)
                        wcsPoint.Y = Convert.ToDouble(dataGridViewCellCollection[1].Value);
                    if (dataGridViewCellCollection.Count > 2)
                        wcsPoint.Z = Convert.ToDouble(dataGridViewCellCollection[2].Value);
                    if (dataGridViewCellCollection.Count > 3)
                        wcsPoint.U = Convert.ToDouble(dataGridViewCellCollection[3].Value);
                    if (dataGridViewCellCollection.Count > 4)
                        wcsPoint.V = Convert.ToDouble(dataGridViewCellCollection[4].Value);
                    if (dataGridViewCellCollection.Count > 5)
                        wcsPoint.Theta = Convert.ToDouble(dataGridViewCellCollection[5].Value);
                    // 将点绘制到图像上                  
                    HOperatorSet.AffineTransPoint2d(userWcsCoordSystem.GetVariationHomMat2D(), wcsPoint.X, wcsPoint.Y, out Qx, out Qy);
                    wcsPoint.X = Qx.D;
                    wcsPoint.Y = Qy.D;
                    this.drawImageObject.AttachPropertyData.Add(wcsPoint);
                }
                if (this.drawImageObject.BackImage == null)
                    this.drawImageObject.UpdataGraphicView();
                else
                    this.drawImageObject.ShowAttachProperty();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }

        private void 激光采集源comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.激光采集源comboBox.SelectedIndex == -1) return;
            this.dataGridView1.Tag = this.激光采集源comboBox.SelectedItem; //((LaserPointAcq)this._function).LaserAcqSource;
        }

        private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false;
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.dataGridView1.CurrentRow == null) return;
                userWcsPoint wcsPoint;
                int index = this.dataGridView1.CurrentRow.Index;
                for (int i = 0; i < this.drawImageObject.AttachPropertyData.Count; i++)
                {
                    if (i == index + 1)
                    {
                        if (this.drawImageObject.AttachPropertyData[i] is userWcsPoint)
                        {
                            wcsPoint = (userWcsPoint)this.drawImageObject.AttachPropertyData[i];
                            wcsPoint.Color = enColor.orange;
                            this.drawImageObject.AttachPropertyData[i] = wcsPoint;
                        }
                    }
                    else
                    {
                        if (this.drawImageObject.AttachPropertyData[i] is userWcsPoint)
                        {
                            wcsPoint = (userWcsPoint)this.drawImageObject.AttachPropertyData[i];
                            wcsPoint.Color = enColor.white;
                            this.drawImageObject.AttachPropertyData[i] = wcsPoint;
                        }
                    }
                }
                /////////////////////
                if (this.drawImageObject.BackImage == null)
                    this.drawImageObject.UpdataGraphicView();
                else
                    this.drawImageObject.ShowAttachProperty();
            }
            catch (Exception ee)
            {
                MessageBox.Show("dataGridView1_SelectionChanged操作失败" + ee.ToString());
            }
        }



        #region 右键菜单项
        private void addContextMenu()
        {
            ContextMenuStrip ContextMenuStrip1 = new ContextMenuStrip();
            // 添加右键菜单 
            ToolStripItem[] items = new ToolStripMenuItem[]
            {
                new ToolStripMenuItem("删除"),
                new ToolStripMenuItem("清空"),
                new ToolStripMenuItem("矩形阵列"),
                new ToolStripMenuItem("圆形阵列"),
                new ToolStripMenuItem("移动到激光位置"),
                new ToolStripMenuItem("移动到相机位置"),
            };
            ContextMenuStrip1.Items.AddRange(items); // 在滑赋值前不能调用 
            ContextMenuStrip1.ItemClicked += new ToolStripItemClickedEventHandler(dataGridViewContextMenuStrip_ItemClicked);
            this.dataGridView1.ContextMenuStrip = ContextMenuStrip1;
        }
        private void dataGridViewContextMenuStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            string name = e.ClickedItem.Text;
            int index = 0;
            userWcsLine wcsLine, wcsLineAdd;
            try
            {
                ((ContextMenuStrip)sender).Close();
                switch (name)
                {
                    case "删除":
                        ((ContextMenuStrip)sender).Close();
                        if (this.dataGridView1.CurrentRow == null) return;
                        this.dataGridView1.Rows.Remove(this.dataGridView1.CurrentRow);
                        for (int i = 0; i < this.dataGridView1.Rows.Count; i++)
                        {
                            this.dataGridView1.Rows[i].HeaderCell.Value = i.ToString();
                        }
                        break;
                    //////////////////////////////////////
                    case "清空":
                        ((ContextMenuStrip)sender).Close();
                        ((FunctionBlock.LaserPointAcq)this._function).AcqCoordPointList.Clear();
                        break;

                    case "矩形阵列":
                        if (this.dataGridView1.CurrentRow == null) return;
                        index = this.dataGridView1.CurrentRow.Index;
                        RectangleArrayDataForm rectform = new RectangleArrayDataForm();
                        rectform.Owner = this;
                        rectform.Show();
                        wcsLine = ((FunctionBlock.LaserLineScanAcq)this._function).AcqCoordLine[index];
                        for (int i = 0; i < rectform.RowCount; i++)
                        {
                            for (int j = 0; j < rectform.ColCount; j++)
                            {
                                if (i == 0 && j == 0) continue; //选定行不变
                                wcsLineAdd = wcsLine.Clone();
                                wcsLineAdd.CamParams = wcsLine.CamParams;
                                wcsLineAdd.X1 = wcsLine.X1 + rectform.OffsetX * j;
                                wcsLineAdd.Y1 = wcsLine.Y1 + rectform.OffsetY * i;
                                wcsLineAdd.X2 = wcsLine.X2 + rectform.OffsetX * j;
                                wcsLineAdd.Y2 = wcsLine.Y2 + rectform.OffsetY * i;
                                ((FunctionBlock.LaserLineScanAcq)this._function).AcqCoordLine.Add(wcsLineAdd);
                                Thread.Sleep(100);
                            }
                        }
                        rectform.Close();
                        break;

                    case "圆形阵列":
                        CircleArrayDataForm circleForm = new CircleArrayDataForm();
                        circleForm.Owner = this;
                        circleForm.Show();
                        wcsLine = ((FunctionBlock.LaserLineScanAcq)this._function).AcqCoordLine[index];
                        HTuple homMat2dIdentity, homMat2dRotate, Qx, Qy;
                        for (int i = 1; i < circleForm.ArrayNum; i++)
                        {
                            HOperatorSet.HomMat2dIdentity(out homMat2dIdentity);
                            HOperatorSet.HomMat2dRotate(homMat2dIdentity, circleForm.Add_Deg * i * Math.PI / 180, circleForm.Ref_X, circleForm.Ref_Y, out homMat2dRotate);
                            HOperatorSet.AffineTransPoint2d(homMat2dRotate, new HTuple(wcsLine.X1 + circleForm.Ref_X, wcsLine.X2 + circleForm.Ref_X), new HTuple(wcsLine.Y1 + circleForm.Ref_Y, wcsLine.Y2 + circleForm.Ref_Y), out Qx, out Qy);
                            wcsLineAdd = wcsLine.Clone();
                            wcsLineAdd.CamParams = wcsLine.CamParams;
                            wcsLineAdd.X1 = Qx[0].D;
                            wcsLineAdd.Y1 = Qy[0].D;
                            wcsLineAdd.X2 = Qx[1].D;
                            wcsLineAdd.Y2 = Qy[1].D;
                        }
                        break;

                    case "移动到激光位置":
                        if (this.dataGridView1.CurrentRow == null) return;
                        index = this.dataGridView1.CurrentRow.Index;
                        MoveCommandParam CommandParam, affineCommandParam;
                        CommandParam = new MoveCommandParam(((FunctionBlock.LaserLineScanAcq)this._function).MotionType, GlobalVariable.pConfig.MoveSpeed);
                        CommandParam.CoordSysName = ((FunctionBlock.LaserLineScanAcq)this._function).CoordSysName;
                        CommandParam.AxisParam.X = ((FunctionBlock.LaserLineScanAcq)this._function).AcqCoordLine[index].X1;
                        CommandParam.AxisParam.Y = ((FunctionBlock.LaserLineScanAcq)this._function).AcqCoordLine[index].Y1;
                        CommandParam.AxisParam.Z = ((FunctionBlock.LaserLineScanAcq)this._function).AcqCoordLine[index].Z1;
                        ///////////////////////////////////////////////////
                        affineCommandParam = CommandParam.Affine2DCommandParam((userWcsCoordSystem)this._function.GetPropertyValues("坐标系"));
                        MotionCardManage.GetCard(CommandParam.CoordSysName).MoveMultyAxis(affineCommandParam.CoordSysName, affineCommandParam.MoveAxis, affineCommandParam.MoveSpeed, affineCommandParam.AxisParam);
                        break;

                    case "移动到相机位置":
                        if (this.dataGridView1.CurrentRow == null) return;
                        index = this.dataGridView1.CurrentRow.Index;
                        CommandParam = new MoveCommandParam(((FunctionBlock.LaserLineScanAcq)this._function).MotionType, GlobalVariable.pConfig.MoveSpeed);
                        CommandParam.CoordSysName = ((FunctionBlock.LaserLineScanAcq)this._function).CoordSysName;
                        CommandParam.AxisParam.X = ((FunctionBlock.LaserLineScanAcq)this._function).AcqCoordLine[index].Grab_x;
                        CommandParam.AxisParam.Y = ((FunctionBlock.LaserLineScanAcq)this._function).AcqCoordLine[index].Grab_y;
                        ///////////////////////////////////////////////
                        affineCommandParam = CommandParam.Affine2DCommandParam((userWcsCoordSystem)this._function.GetPropertyValues("坐标系"));
                        MotionCardManage.GetCard(CommandParam.CoordSysName).MoveMultyAxis(affineCommandParam.CoordSysName, affineCommandParam.MoveAxis, affineCommandParam.MoveSpeed, affineCommandParam.AxisParam);
                        break;
                    ///////////////////////////////////////////////
                    default:
                        break;
                }
            }
            catch
            {
            }
        }
        #endregion




    }
}

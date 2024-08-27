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
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using View;

namespace FunctionBlock
{
    public partial class LaserPointAcqForm : Form
    {
        private CancellationTokenSource cts;
        private Form form;
        private IFunction _function;
        private VisualizeView drawImageObject;
        private VisualizeView drawModelObject3D;
        private FunctionBlock.AcqSource _laserAcqSource = null;
        public LaserPointAcqForm(IFunction function, TreeNode node)
        {
            this._function = function;
            InitializeComponent();
            this.Text = ((FunctionBlock.LaserPointAcq)this._function).Name;
            this.drawImageObject = new VisualizeView(this.相机hWindowControl, true);
            this.drawModelObject3D = new VisualizeView(this.激光hWindowControl);
            new ListBoxWrapClass().InitListBox(this.listBox1, node, 2);
            addContextMenu();
        }
        private void LaserPointAcqForm_Load(object sender, EventArgs e)
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
                this.dataGridView1.DataSource = ((FunctionBlock.LaserPointAcq)this._function).AcqCoordPointList;
                this.激光采集源comboBox.DataSource = FunctionBlock.AcqSourceManage.Instance.LaserAcqSourceList();//  SensorManage.LaserList; 
                this.激光采集源comboBox.DisplayMember = "Name";
                this.相机采集源comboBox.DataSource = FunctionBlock.AcqSourceManage.Instance.GetCamAcqSourceList();//  SensorManage.CameraList;
                this.相机采集源comboBox.DisplayMember = "Name";
                //////////////////
                this.激光采集源comboBox.DataBindings.Add("SelectedItem", ((FunctionBlock.LaserPointAcq)this._function), "LaserAcqSource", true, DataSourceUpdateMode.OnPropertyChanged);
                this.相机采集源comboBox.DataBindings.Add("SelectedItem", ((FunctionBlock.LaserPointAcq)this._function), "CamAcqSource", true, DataSourceUpdateMode.OnPropertyChanged);
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
                    this.drawModelObject3D.PointCloudModel3D = new PointCloudData (this._function.GetPropertyValues("距离1对象"));
                    break;
                case enShowItems.距离2对象:
                    this.drawModelObject3D.PointCloudModel3D = new PointCloudData(this._function.GetPropertyValues("距离2对象"));
                    break;
                case enShowItems.厚度对象:
                    this.drawModelObject3D.PointCloudModel3D = new PointCloudData (this._function.GetPropertyValues("厚度对象"));
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


        private void LaserPointAcqForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (cts != null)
                    cts.Cancel();
                BaseFunction.PointsCloudAcqComplete -= new PointCloudAcqCompleteEventHandler(PointCloudAcqComplete_Event);
            }
            catch
            {

            }
        }

        private void 添加点Button_Click(object sender, EventArgs e)
        {
            try
            {
                ////////////////////////////////////
                CoordSysAxisParam coord = new CoordSysAxisParam(((FunctionBlock.LaserPointAcq)this._function).CoordSysName);
                userWcsPoint wcsPoint = new userWcsPoint(coord.X, coord.Y, coord.Z, coord.X, coord.Y, this.drawImageObject.CameraParam);
                ((FunctionBlock.LaserPointAcq)this._function).AcqCoordPointList.Add(wcsPoint);
                this.drawImageObject.AttachPropertyData.Add(wcsPoint);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void 删除点Button_Click(object sender, EventArgs e)
        {
            //if (this.dataGridView1.CurrentRow == null) return;
            //try
            //{
            //    int index = this.dataGridView1.CurrentRow.Index;
            //    ((FunctionBlock.LaserPointAcq)_function).Coord1Table.Rows.RemoveAt(index);
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.ToString());
            //}
        }

        private void 清空点Button_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    ((FunctionBlock.LaserPointAcq)this._function).Coord1Table.Rows.Clear();
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.ToString());
            //}
        }

        private void 图像取点button_Click(object sender, EventArgs e)
        {
            try
            {
                userWcsPoint wcsPoint;
                ////////////////////////////////////
                CoordSysAxisParam coordSysAxisParam = new CoordSysAxisParam(((FunctionBlock.LaserPointAcq)this._function).CoordSysName);
                this.相机hWindowControl.Cursor = Cursors.Cross;
                this.drawImageObject.DrawWcsPointOnWindow(enColor.white, coordSysAxisParam.X, coordSysAxisParam.Y, out wcsPoint);
                this.相机hWindowControl.Cursor = Cursors.Default;
                ((FunctionBlock.LaserPointAcq)this._function).AcqCoordPointList.Add(wcsPoint);
                this.drawImageObject.AttachPropertyData.Add(wcsPoint);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void 视图工具toolStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            HalconLibrary ha = new HalconLibrary();
            string name = e.ClickedItem.Name;
            switch (name)
            {
                case "toolStripButton_Clear":
                    this.drawImageObject.ClearWindow();
                    this.toolStripButton_Translate.CheckState = CheckState.Unchecked;
                    break;
                case "toolStripButton_Select":
                    this.drawImageObject.Select();
                    this.toolStripButton_Translate.CheckState = CheckState.Unchecked;
                    break;
                case "toolStripButton_Translate":
                    this.drawImageObject.TranslateScaleImage();
                    this.toolStripButton_Translate.CheckState = CheckState.Checked;
                    break;
                case "toolStripButton_Auto":
                    this.drawImageObject.AutoImage();
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
                userWcsCoordSystem userWcsCoordSystem = ((FunctionBlock.LaserPointAcq)_function).extractRefSource2Data();
                this.drawImageObject.AttachPropertyData.Clear();
                this.drawImageObject.AttachPropertyData.Add(userWcsCoordSystem);
                DataGridViewCellCollection dataGridViewCellCollection;
                for (int i = 0; i < this.dataGridView1.Rows.Count; i++)
                {
                    this.dataGridView1.Rows[i].HeaderCell.Value = i.ToString();
                    dataGridViewCellCollection = this.dataGridView1.Rows[i].Cells;
                    ////////////////////////////////////////
                    wcsPoint.CamParams = this.drawImageObject.CameraParam;
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
                    if (dataGridViewCellCollection.Count > 6)
                        wcsPoint.Grab_x = Convert.ToDouble(dataGridViewCellCollection[6].Value);
                    if (dataGridViewCellCollection.Count > 7)
                        wcsPoint.Grab_y = Convert.ToDouble(dataGridViewCellCollection[7].Value);
                    // 将点绘制到图像上                  
                    HOperatorSet.AffineTransPoint2d(userWcsCoordSystem.GetVariationHomMat2D(), wcsPoint.X, wcsPoint.Y, out Qx, out Qy);
                    wcsPoint.X = Qx.D;
                    wcsPoint.Y = Qy.D;
                    wcsPoint.Color = enColor.white;
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

        #region  数据实时采集
        CancellationTokenSource cts2;
        private void 实时采集checkBox_CheckedChanged(object sender, EventArgs e)
        {
            cts2 = new CancellationTokenSource();
            List<object> data;
            Task.Run(() =>
            {
                while (!cts2.IsCancellationRequested)
                {
                    //this.drawImageObject.BackImage = (ImageDataClass)data[0];
                    Thread.Sleep(100);
                }
            });
        }
        #endregion

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
                new ToolStripMenuItem("偏置"),
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
            userWcsPoint wcsPoint, wcsPointAdd;
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
                        rectform.ShowDialog();
                        wcsPoint = ((FunctionBlock.LaserPointAcq)this._function).AcqCoordPointList[index];
                        for (int i = 0; i < rectform.RowCount; i++)
                        {
                            for (int j = 0; j < rectform.ColCount; j++)
                            {
                                if (i == 0 && j == 0) continue; //选定行不变
                                wcsPointAdd = new userWcsPoint();
                                wcsPointAdd.CamParams = wcsPoint.CamParams;
                                wcsPointAdd.X = wcsPoint.X + rectform.OffsetX * j;
                                wcsPointAdd.Y = wcsPoint.Y + rectform.OffsetY * i;
                                ((FunctionBlock.LaserPointAcq)this._function).AcqCoordPointList.Add(wcsPointAdd);
                                Thread.Sleep(100);
                            }
                        }
                        rectform.Close();
                        break;

                    case "圆形阵列":
                        CircleArrayDataForm circleForm = new CircleArrayDataForm();
                        circleForm.Owner = this;
                        circleForm.ShowDialog();
                        wcsPoint = ((FunctionBlock.LaserPointAcq)this._function).AcqCoordPointList[index];
                        HTuple homMat2dIdentity, homMat2dRotate, Qx, Qy;
                        for (int i = 1; i < circleForm.ArrayNum; i++)
                        {
                            HOperatorSet.HomMat2dIdentity(out homMat2dIdentity);
                            HOperatorSet.HomMat2dRotate(homMat2dIdentity, circleForm.Add_Deg * i * Math.PI / 180, circleForm.Ref_X, circleForm.Ref_Y, out homMat2dRotate);
                            HOperatorSet.AffineTransPoint2d(homMat2dRotate, new HTuple(wcsPoint.X + circleForm.Ref_X), new HTuple(wcsPoint.Y + circleForm.Ref_Y), out Qx, out Qy);
                            wcsPointAdd = new userWcsPoint();
                            wcsPointAdd.CamParams = wcsPoint.CamParams;
                            wcsPointAdd.X = Qx[0].D;
                            wcsPointAdd.Y = Qy[0].D;
                        }
                        break;

                    case "移动到激光位置":
                        if (this.dataGridView1.CurrentRow == null) return;
                        index = this.dataGridView1.CurrentRow.Index;
                        MoveCommandParam CommandParam, affineCommandParam;
                        CommandParam = new MoveCommandParam(enAxisName.XYZ轴, GlobalVariable.pConfig.MoveSpeed);
                        CommandParam.CoordSysName = ((FunctionBlock.LaserPointAcq)this._function).CoordSysName;
                        CommandParam.AxisParam.X = ((FunctionBlock.LaserPointAcq)this._function).AcqCoordPointList[index].X;
                        CommandParam.AxisParam.Y = ((FunctionBlock.LaserPointAcq)this._function).AcqCoordPointList[index].Y;
                        CommandParam.AxisParam.Z = ((FunctionBlock.LaserPointAcq)this._function).AcqCoordPointList[index].Z;
                        ///////////////////////////////////////////////////
                        affineCommandParam = CommandParam.Affine2DCommandParam((userWcsCoordSystem)this._function.GetPropertyValues("坐标系"));
                        MotionCardManage.GetCard(CommandParam.CoordSysName).MoveMultyAxis(affineCommandParam.CoordSysName, affineCommandParam.MoveAxis, affineCommandParam.MoveSpeed, affineCommandParam.AxisParam);
                        break;

                    case "移动到相机位置":
                        if (this.dataGridView1.CurrentRow == null) return;
                        index = this.dataGridView1.CurrentRow.Index;
                        CommandParam = new MoveCommandParam(enAxisName.XY轴, GlobalVariable.pConfig.MoveSpeed);
                        CommandParam.CoordSysName = ((FunctionBlock.LaserPointAcq)this._function).CoordSysName;
                        CommandParam.AxisParam.X = ((FunctionBlock.LaserPointAcq)this._function).AcqCoordPointList[index].Grab_x;
                        CommandParam.AxisParam.Y = ((FunctionBlock.LaserPointAcq)this._function).AcqCoordPointList[index].Grab_y;
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

        private void 激光采集源comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {

            }
            catch
            {

            }
        }

        private void 相机采集源comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {

            }
            catch
            {

            }
        }


    }
}

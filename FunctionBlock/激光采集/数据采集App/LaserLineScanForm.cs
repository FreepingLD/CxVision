using AlgorithmsLibrary;
using Common;
using FunctionBlock;
using HalconDotNet;
using MotionControlCard;
using Sensor;
using System;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using View;

namespace FunctionBlock
{
    public partial class LaserLineScanForm : Form
    {
        private CancellationTokenSource cts;
        private Form form;
        private IFunction _function;
        private VisualizeView drawObject;
        private VisualizeView drawModelObject3D;
        private bool isFormClose = false;
        private userWcsPoint wcsPoint2, wcsPoint1;
        public LaserLineScanForm(IFunction function,TreeNode node)
        {
            this._function = function;
            InitializeComponent();
            drawObject = new VisualizeView(this.hWindowControl1, false);
            new ListBoxWrapClass().InitListBox(this.listBox1, node, 2);
            // new DataGridViewWrapClass().InitDataGridView(this, this._function, this.dataGridView1, ((FunctionBlock.LaserLineScanAcq)this._function).Coord1Table);
        }
        private void LaserLineScanForm_Load(object sender, EventArgs e)
        {
            BaseFunction.PointsCloudAcqComplete += new PointCloudAcqCompleteEventHandler(PointCloudAcqComplete_Event);
            BindProperty();
            AddForm(this.运动panel, new JogMotionForm());
        }
        private void BindProperty()
        {
            try
            {
                ////////////////
                this.dataGridView1.DataSource = ((FunctionBlock.LaserLineScanAcq)this._function).AcqCoordLine;
                this.激光采集源comboBox.DataSource = FunctionBlock.AcqSourceManage.Instance.LaserAcqSourceList();//SensorManage.LaserList;
                this.激光采集源comboBox.DisplayMember = "Name";
                this.相机采集源comboBox.DataSource = FunctionBlock.AcqSourceManage.Instance.GetCamAcqSourceList();// SensorManage.CameraList;
                this.相机采集源comboBox.DisplayMember = "Name";
                //this.运动类型comboBox.DataSource = Enum.GetNames(typeof(enAxisName));
                //////////////////
                this.激光采集源comboBox.DataBindings.Add("SelectedItem", ((FunctionBlock.LaserLineScanAcq)this._function), "LaserAcqSource", true, DataSourceUpdateMode.OnPropertyChanged);
                this.相机采集源comboBox.DataBindings.Add("SelectedItem", ((FunctionBlock.LaserLineScanAcq)this._function), "CamAcqSource", true, DataSourceUpdateMode.OnPropertyChanged);
                //this.运动类型comboBox.DataBindings.Add("Text", (FunctionBlock.LaserLineScanAcq)this._function, "MotionType", true, DataSourceUpdateMode.OnPropertyChanged);
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
            HalconLibrary ha = new HalconLibrary();
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
                case "toolStripButton_3D":
                    this.toolStripButton_Translate.CheckState = CheckState.Unchecked;

                    break;
                default:
                    break;
            }
        }

        private void PointCloudAcqComplete_Event(object send, PointCloudAcqCompleteEventArgs e)
        {
            //if (((FunctionBlock.LaserLineScanAcq)this._function).LaserAcqSource == null) return;
            //string name = ((FunctionBlock.LaserLineScanAcq)this._function).LaserAcqSource.Sensor.ConfigParam.SensorName;
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

        private void LaserLineScanForm_FormClosing(object sender, FormClosingEventArgs e)
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


        private void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            try
            {
                //HTuple Qx, Qy;
                //userWcsLine wcsLine = new userWcsLine();
                //object[] itemArray = this.drawObject.AttachPropertyData.ToArray();
                //this.drawObject.AttachPropertyData.Clear();
                ////if (((FunctionBlock.LaserLineScanAcq)this._function).LaserAcqSource == null) return;
                ////userWcsCoordSystem userWcsCoordSystem = ((FunctionBlock.LaserLineScanAcq)_function).extractRefSource2Data();
                ////userWcsPose3D laserAffinePose = ((FunctionBlock.LaserLineScanAcq)this._function).LaserAcqSource.Sensor.LaserParam.LaserCalibrationParam.LaserAffinePose;

                /////////////////////////////////////////////////////////////////////
                ////this.drawObject.AttachPropertyData.Add(userWcsCoordSystem);
                //DataGridViewCellCollection dataGridViewCellCollection;
                //for (int i = 0; i < this.dataGridView1.Rows.Count; i++)
                //{
                //    this.dataGridView1.Rows[i].HeaderCell.Value = i.ToString();
                //    dataGridViewCellCollection = this.dataGridView1.Rows[i].Cells;
                //    if (dataGridViewCellCollection.Count > 0)
                //        wcsLine.x1 = Convert.ToDouble(dataGridViewCellCollection[0].Value);
                //    if (dataGridViewCellCollection.Count > 1)
                //        wcsLine.y1 = Convert.ToDouble(dataGridViewCellCollection[1].Value);
                //    if (dataGridViewCellCollection.Count > 2)
                //        wcsLine.z1 = Convert.ToDouble(dataGridViewCellCollection[2].Value);
                //    if (dataGridViewCellCollection.Count > 3)
                //        wcsLine.u1 = Convert.ToDouble(dataGridViewCellCollection[3].Value);
                //    if (dataGridViewCellCollection.Count > 4)
                //        wcsLine.v1 = Convert.ToDouble(dataGridViewCellCollection[4].Value);
                //    if (dataGridViewCellCollection.Count > 5)
                //        wcsLine.theta1 = Convert.ToDouble(dataGridViewCellCollection[5].Value);
                //    ////////////////////////
                //    if (dataGridViewCellCollection.Count > 6)
                //        wcsLine.x2 = Convert.ToDouble(dataGridViewCellCollection[6].Value);
                //    if (dataGridViewCellCollection.Count > 7)
                //        wcsLine.y2 = Convert.ToDouble(dataGridViewCellCollection[7].Value);
                //    if (dataGridViewCellCollection.Count > 8)
                //        wcsLine.z2 = Convert.ToDouble(dataGridViewCellCollection[8].Value);
                //    if (dataGridViewCellCollection.Count > 9)
                //        wcsLine.u2 = Convert.ToDouble(dataGridViewCellCollection[9].Value);
                //    if (dataGridViewCellCollection.Count > 10)
                //        wcsLine.v2 = Convert.ToDouble(dataGridViewCellCollection[10].Value);
                //    if (dataGridViewCellCollection.Count > 11)
                //        wcsLine.theta2 = Convert.ToDouble(dataGridViewCellCollection[11].Value);
                //    if (dataGridViewCellCollection.Count > 12)
                //        wcsLine.refPoint_x = Convert.ToDouble(dataGridViewCellCollection[12].Value);
                //    if (dataGridViewCellCollection.Count > 13)
                //        wcsLine.refPoint_y = Convert.ToDouble(dataGridViewCellCollection[13].Value);
                //    // 将点绘制到图像上                  
                //    HOperatorSet.AffineTransPoint2d(userWcsCoordSystem.GetVariationHomMat2D(), new HTuple(wcsLine.x1 - laserAffinePose.Tx, wcsLine.x2 - laserAffinePose.Tx), new HTuple(wcsLine.y1 - laserAffinePose.Ty, wcsLine.y2 - laserAffinePose.Ty), out Qx, out Qy);
                //    wcsLine.x1 = Qx[0].D;
                //    wcsLine.y1 = Qy[0].D;
                //    wcsLine.x2 = Qx[1].D;
                //    wcsLine.y2 = Qy[1].D;
                //    wcsLine.color = enColor.white;
                ////    this.drawObject.AttachPropertyData.Add(wcsLine);
                //}
                ///////////////////////
                //if (this.drawObject.BackImage == null)
                //    this.drawObject.UpdataGraphicView();
                //else
                //    this.drawObject.ShowAttachProperty();
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


        private void 图像取线button_Click(object sender, EventArgs e)
        {
            try
            {
                userWcsLine wcsLine;
                ////////////////////////////////////
                CoordSysAxisParam coordSysAxisParam = new CoordSysAxisParam(((FunctionBlock.LaserLineScanAcq)this._function).CoordSysName);
                this.hWindowControl1.Cursor = Cursors.Cross;
                this.drawObject.DrawWcsLineOnWindow(enColor.white, coordSysAxisParam.X, coordSysAxisParam.Y, out wcsLine);
                this.hWindowControl1.Cursor = Cursors.Default;
                ////////////////////////////
                ((FunctionBlock.LaserLineScanAcq)_function).AcqCoordLine.Add(wcsLine);
                this.drawObject.AttachPropertyData.Add(wcsLine);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
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
                userWcsLine wcsLine = new userWcsLine(X1, Y1, Z1, X2, Y2, Z2, this.drawObject.CameraParam);
                wcsLine.U1 = U1;
                wcsLine.V1 = V1;
                wcsLine.Theta1 = W1;
                wcsLine.U2 = U2;
                wcsLine.V2 = V2;
                wcsLine.Theta2 = W2;
                ///////////////////////////////////////
                ((FunctionBlock.LaserLineScanAcq)_function).AcqCoordLine.Add(wcsLine);
                this.drawObject.AttachPropertyData.Add(wcsLine);
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
                ((FunctionBlock.LaserLineScanAcq)_function).AcqCoordLine.RemoveAt(index);
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
                ((FunctionBlock.LaserLineScanAcq)_function).AcqCoordLine.Clear();
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
                ////////////////////////////////////
                CoordSysAxisParam coordSysAxisParam = new CoordSysAxisParam(((FunctionBlock.LaserLineScanAcq)this._function).CoordSysName);
                this.hWindowControl1.Cursor = Cursors.Cross;
                this.drawObject.DrawWcsPointOnWindow(enColor.white, coordSysAxisParam.X, coordSysAxisParam.Y, out wcsPoint1);
                this.hWindowControl1.Cursor = Cursors.Default;
                this.drawModelObject3D.AttachPropertyData.Add(wcsPoint1);
                this.X1坐标textBox.Text = wcsPoint1.X.ToString();
                this.Y1坐标textBox.Text = wcsPoint1.Y.ToString();
                this.Z1坐标textBox.Text = wcsPoint1.Z.ToString();
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
                ////////////////////////////////////
                CoordSysAxisParam coordSysAxisParam = new CoordSysAxisParam(((FunctionBlock.LaserLineScanAcq)this._function).CoordSysName);
                this.hWindowControl1.Cursor = Cursors.Cross;
                this.drawObject.DrawWcsPointOnWindow(enColor.white, coordSysAxisParam.X, coordSysAxisParam.Y, out wcsPoint2);
                this.hWindowControl1.Cursor = Cursors.Default;
                ////////////////////////////////
                this.drawObject.AttachPropertyData.Add(wcsPoint2);  
                this.X2坐标textBox.Text = wcsPoint2.X.ToString();
                this.Y2坐标textBox.Text = wcsPoint2.Y.ToString();
                this.Z2坐标textBox.Text = wcsPoint2.Grab_theta.ToString();
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
                CoordSysAxisParam coordSysAxisParam = new CoordSysAxisParam(((FunctionBlock.LaserLineScanAcq)this._function).CoordSysName);
                this.X1坐标textBox.Text = coordSysAxisParam.X.ToString();
                this.Y1坐标textBox.Text = coordSysAxisParam.Y.ToString();
                this.Z1坐标textBox.Text = coordSysAxisParam.Z.ToString();
                this.U1坐标textBox.Text = coordSysAxisParam.U.ToString();
                this.V1坐标textBox.Text = coordSysAxisParam.V.ToString();
                this.W1坐标textBox.Text = coordSysAxisParam.Theta.ToString();
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
                CoordSysAxisParam coordSysAxisParam = new CoordSysAxisParam(((FunctionBlock.LaserLineScanAcq)this._function).CoordSysName);
                this.X2坐标textBox.Text = coordSysAxisParam.X.ToString();
                this.Y2坐标textBox.Text = coordSysAxisParam.Y.ToString();
                this.Z2坐标textBox.Text = coordSysAxisParam.Z.ToString();
                this.U2坐标textBox.Text = coordSysAxisParam.U.ToString();
                this.V2坐标textBox.Text = coordSysAxisParam.V.ToString();
                this.W2坐标textBox.Text = coordSysAxisParam.Theta.ToString();
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


        #region  数据实时采集
        CancellationTokenSource cts2;
        private void 实时采集checkBox_CheckedChanged(object sender, EventArgs e)
        {
            //cts2 = new CancellationTokenSource();
            //List<object> data;
            //Task.Run(() =>
            //{
            //    while (!cts2.IsCancellationRequested)
            //    {
            //        //this.drawImageObject.BackImage = (ImageDataClass)data[0];
            //        Thread.Sleep(100);
            //    }
            //});
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
                        rectform.ShowDialog();
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
                        circleForm.ShowDialog();
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

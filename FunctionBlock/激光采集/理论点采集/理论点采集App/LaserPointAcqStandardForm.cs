
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
    public partial class LaserPointAcqStandardForm : Form
    {
        private object _objectDataModel;
        private CancellationTokenSource cts;
        private FunctionBlock.AcqSource _camAcqSource;
        private Form form;
        private IFunction _function;
        private VisualizeView drawObject;
        private bool isFormClose = false;
        public LaserPointAcqStandardForm(IFunction function,TreeNode node)
        {
            this._function = function;
            InitializeComponent();
            this.dataGridView1.Tag = ((FunctionBlock.LaserPointAcqStandard)this._function).LaserAcqSource;
            this.Text = function.GetPropertyValues("名称").ToString();
            this.drawObject = new VisualizeView(this.hWindowControl1);
            new ListBoxWrapClass().InitListBox(this.listBox1, node, 2);
            new DataGridViewWrapClass().InitDataGridView(this, function, this.dataGridView1, ((FunctionBlock.LaserPointAcqStandard)this._function).Coord1Table);
        }
        private void LaserPointAcqStandardForm_Load(object sender, EventArgs e)
        {
            // 注册事件
            BaseFunction.PointsCloudAcqComplete += new PointCloudAcqCompleteEventHandler(PointCloudAcqComplete_Event);
            //SensorBase.ImageAcqComplete += new ImageAcqCompleteEventHandler(ImageAcqComplete_Event);
            BindProperty();
        }
        private void BindProperty()
        {
            try
            {
                this.dataGridView1.DataSource = ((FunctionBlock.LaserPointAcqStandard)this._function).Coord1Table;
                this.激光采集源comboBox.DataSource = FunctionBlock.AcqSourceManage.Instance.LaserAcqSourceList();// SensorManage.LaserList;
                this.激光采集源comboBox.DisplayMember = "Name";
                //////////////////
                this.激光采集源comboBox.DataBindings.Add("SelectedItem", ((FunctionBlock.LaserPointAcqStandard)this._function), "LaserAcqSource", true, DataSourceUpdateMode.OnPropertyChanged);
                // this.坐标系comboBox.DataBindings.Add("SelectedItem", (LaserPointAcqStandard)this._function, "CoordSystem", true, DataSourceUpdateMode.OnPropertyChanged);
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
            }
        }


        private void PointCloudAcqComplete_Event(object send, PointCloudAcqCompleteEventArgs e)
        {
            //if (((FunctionBlock.LaserPointAcq)this._function).LaserAcqSource == null) return;
            //string name = ((FunctionBlock.LaserPointAcq)this._function).LaserAcqSource.Sensor.ConfigParam.SensorName;
            //switch (e.SensorName.Split('(')[0])
            //{
            //    case "读取3D对象":
            //        if (this.drawObject != null)
            //            this.drawObject.PointCloudModel3D = new HObjectModel3D[] { e.PointsCloudData };
            //        break;
            //    default:
            //        if (name == e.SensorName) // 因为是用同一个事件来发送图像，所以这里使用名字来判断是不是同一个相机发送的
            //        {
            //            if (this.drawObject != null)
            //                this.drawObject.PointCloudModel3D = new HObjectModel3D[] { e.PointsCloudData };
            //        }
            //        break;
            //}
        }
        private void ImageAcqComplete_Event(ImageAcqCompleteEventArgs e)
        {
            //double x, y, z;
            //if (((FunctionBlock.LaserPointAcq)this._function).CamAcqSource == null) return;
            //string name = ((FunctionBlock.LaserPointAcq)this._function).CamAcqSource.Sensor.ConfigParam.SensorName;
            //switch (e.CamName.Split('(')[0])
            //{
            //    case "读取图像":
            //    case "图像采集":
            //        if (this.drawObject != null)
            //            this.drawObject.BackImage = e.ImageData;
            //        break;
            //    default:
            //        if (name == e.CamName) // 因为是用同一个事件来发送图像，所以这里使用名字来判断是不是同一个相机发送的
            //        {
            //            if (((FunctionBlock.LaserPointAcq)this._function).CamAcqSource.Card != null)
            //            {
            //                ((FunctionBlock.LaserPointAcq)this._function).CamAcqSource.Card.GetAxisPosition(MotionCardManage.CurrentCoordSys, enAxisName.X轴, out x);
            //                ((FunctionBlock.LaserPointAcq)this._function).CamAcqSource.Card.GetAxisPosition(MotionCardManage.CurrentCoordSys, enAxisName.Y轴, out y);
            //                ((FunctionBlock.LaserPointAcq)this._function).CamAcqSource.Card.GetAxisPosition(MotionCardManage.CurrentCoordSys, enAxisName.Z轴, out z);
            //                e.ImageData.Grab_X = x;
            //                e.ImageData.Grab_Y = y;
            //                e.ImageData.Grab_Z = z;
            //            }
            //            if (this.drawObject != null)
            //                this.drawObject.BackImage = e.ImageData;
            //        }
            //        break;
            //}
        }

        private void 显示条目comboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            FunctionBlock.LaserPointAcqStandard.enShowItems item;
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
                        Task.Run(() =>
                        {
                            if (this._function.Execute(null).Succss)
                            {
                                if (this.isFormClose)return;
                                this.Invoke(new Action(() =>
                                {
                                    this.toolStripStatusLabel1.Text = "执行结果:";
                                    this.toolStripStatusLabel2.Text = "成功";
                                    this.toolStripStatusLabel2.ForeColor = Color.Green;
                                }));
                            }
                            else
                            {
                                if (this.isFormClose) return;
                                this.Invoke(new Action(() =>
                                {
                                    this.toolStripStatusLabel1.Text = "执行结果:";
                                    this.toolStripStatusLabel2.Text = "失败";
                                    this.toolStripStatusLabel2.ForeColor = Color.Red;
                                }));
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
                    //this.drawObject.Show3D();
                    break;
                default:
                    break;
            }
        }

        private void LaserPointAcqStandardForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                this.isFormClose = true;
                if (cts != null)
                    cts.Cancel();
                BaseFunction.PointsCloudAcqComplete -= new PointCloudAcqCompleteEventHandler(PointCloudAcqComplete_Event);
                //SensorBase.ImageAcqComplete -= new ImageAcqCompleteEventHandler(ImageAcqComplete_Event);
            }
            catch
            {

            }
        }
        private void deletePointButton_Click(object sender, EventArgs e)
        {

            try
            {
                if (this.dataGridView1.CurrentRow == null) return;
                int index = this.dataGridView1.CurrentRow.Index;
                ((FunctionBlock.LaserPointAcqStandard)_function).Coord1Table.Rows.RemoveAt(index);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void ClearButton_Click(object sender, EventArgs e)
        {
            try
            {
                ((FunctionBlock.LaserPointAcqStandard)_function).Coord1Table.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void 导入坐标点button_Click(object sender, EventArgs e)
        {
            FileOperate fo = new FileOperate();
            ImportData(fo.OpenFile());
        }
        private void 添加点button_Click(object sender, EventArgs e)
        {
            try
            {
                double X, Y, Z, U, V, W,Qx,Qy,Qz;
                double.TryParse(this.X轴坐标textBox.Text, out X);
                double.TryParse(this.Y轴坐标textBox.Text, out Y);
                double.TryParse(this.Z轴坐标textBox.Text, out Z);
                double.TryParse(this.U轴坐标textBox.Text, out U);
                double.TryParse(this.V轴坐标textBox.Text, out V);
                double.TryParse(this.W轴坐标textBox.Text, out W);
                ////////////////////////////////////////////
                //userWcsPose3D laserAffinePose = ((FunctionBlock.LaserPointAcqStandard)_function).LaserAcqSource.Sensor.LaserParam.LaserCalibrationParam.LaserAffinePose;
                //laserAffinePose.RigidTranslatePoint3D(X, Y, Z, out Qx, out Qy, out Qz);
                ((FunctionBlock.LaserPointAcqStandard)_function).Coord1Table.Rows.Add(X, Y, Z, U, V, W); // 这里是使用相机点好还是激光点好？
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void ImportData(string Path)
        {
            try
            {
                double X = 0, Y = 0, Z = 0, U = 0, V = 0, W = 0,Qx,Qy,Qz;
                if (Path == null || Path.Length == 0) return;
                using (StreamReader reader = new StreamReader(Path))
                {
                    userWcsPose laserAffinePose = ((FunctionBlock.LaserPointAcqStandard)_function).LaserAcqSource.Sensor.LaserParam.LaserCalibrationParam.LaserAffinePose;
                    string line;
                    while (true)
                    {
                        line = reader.ReadLine();
                        if (line == null) break;
                        string[] st = line.Split(',', '\t', ';'); //如果其他分隔符替换掉就OK了 
                        if (st.Length > 0)
                            X = Convert.ToDouble(st[0]);
                        if (st.Length > 1)
                            Y = Convert.ToDouble(st[1]);
                        if (st.Length > 2)
                            Z = Convert.ToDouble(st[2]);
                        if (st.Length > 3)
                            U = Convert.ToDouble(st[3]);
                        if (st.Length > 4)
                            V = Convert.ToDouble(st[4]);
                        if (st.Length > 5)
                            W = Convert.ToDouble(st[5]);
                        ///////////////////////////////
                        //laserAffinePose.RigidTranslatePoint3D(X, Y, Z, out Qx, out Qy, out Qz);
                        ((FunctionBlock.LaserPointAcqStandard)_function).Coord1Table.Rows.Add(new object[] { X, Y, Z, U, V, W }); // 每更改一次数据源会触发一次dataGridView的数据绑定完成事件
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        // 在dataTable中添加删除数据时都会触发该事件
        private void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            try
            {
                HTuple Qx, Qy;
                userWcsPoint wcsPoint = new userWcsPoint();
                this.drawObject.AttachPropertyData.Clear();
                userWcsCoordSystem userWcsCoordSystem = ((FunctionBlock.LaserPointAcqStandard)_function).extractRefSource2Data();
                this.drawObject.AttachPropertyData.Add(userWcsCoordSystem);
                DataGridViewCellCollection dataGridViewCellCollection;
                for (int i = 0; i < this.dataGridView1.Rows.Count; i++)
                {
                    this.dataGridView1.Rows[i].HeaderCell.Value = i.ToString();
                    dataGridViewCellCollection = this.dataGridView1.Rows[i].Cells;
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
                    HOperatorSet.AffineTransPoint2d(userWcsCoordSystem.GetCurrentHomMat2D(), wcsPoint.X, wcsPoint.Y, out Qx, out Qy);
                    wcsPoint.X = Qx.D;
                    wcsPoint.Y = Qy.D;
                    this.drawObject.AttachPropertyData.Add(wcsPoint);
                }
                /////////////////////
                if (this.drawObject.BackImage == null)
                    this.drawObject.UpdataGraphicView();
                else
                    this.drawObject.ShowAttachProperty();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void 激光采集源comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.激光采集源comboBox.SelectedIndex == -1) return;
            this.dataGridView1.Tag = this.激光采集源comboBox.SelectedItem; // ((LaserPointAcqStandard)this._function).LaserAcqSource;
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
                for (int i = 0; i < this.drawObject.AttachPropertyData.Count; i++)
                {
                    if (i == index + 1)
                    {
                        if (this.drawObject.AttachPropertyData[i] is userWcsPoint)
                        {
                            wcsPoint = (userWcsPoint)this.drawObject.AttachPropertyData[i];
                            wcsPoint.Color = enColor.orange;
                            this.drawObject.AttachPropertyData[i] = wcsPoint;
                        }
                    }
                    else
                    {
                        if (this.drawObject.AttachPropertyData[i] is userWcsPoint)
                        {
                            wcsPoint = (userWcsPoint)this.drawObject.AttachPropertyData[i];
                            wcsPoint.Color = enColor.white;
                            this.drawObject.AttachPropertyData[i] = wcsPoint;
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

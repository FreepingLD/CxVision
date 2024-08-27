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
    public partial class PointCloudAcqForm : Form
    {
        private CancellationTokenSource cts;
        private Form form;
        private IFunction _function;
        private VisualizeView drawObject;
        private AcqSource acqSource;
        private BindingSource bs = new BindingSource();

        public PointCloudAcqForm(IFunction function, TreeNode node)
        {
            this._function = function;
            InitializeComponent();
            this.Text = ((FunctionBlock.PointCloudAcq)this._function).Name;
            this.drawObject = new VisualizeView(this.激光hWindowControl,true);
        }
        private void PointCloudAcqForm_Load(object sender, EventArgs e)
        {
            BaseFunction.ExcuteCompleted += new ExcuteCompletedEventHandler(DisplayExcuteResult);
            this.drawObject.GrayValueInfo += new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
            BindProperty();
        }
        private void BindProperty()
        {
            try
            {
                this.bs.DataSource = ((PointCloudAcq)this._function).AcqParam.FilePath;
                this.bindingNavigator1.BindingSource = this.bs;
                this.bs.DataSourceChanged += new EventHandler(this.bindingNavigatorDataSourceChanged);
                this.采集源comboBox.DataSource = FunctionBlock.AcqSourceManage.Instance.GetAcqSourceName();//  SensorManage.LaserList; 
                //////////////////
                this.多文件目录textBox.DataBindings.Add("Text", ((PointCloudAcq)this._function).AcqParam, "FolderPath", true, DataSourceUpdateMode.OnPropertyChanged);
                this.单文件路径textBox.DataBindings.Add("Text", ((PointCloudAcq)this._function).AcqParam, "SingleFilePath", true, DataSourceUpdateMode.OnPropertyChanged);
                this.激光源radioButton.DataBindings.Add("Checked", ImageAcqDevice.Instance, "IsCamSource", true, DataSourceUpdateMode.OnPropertyChanged);
                this.文件源radioButton.DataBindings.Add("Checked", ImageAcqDevice.Instance, "IsFileSource", true, DataSourceUpdateMode.OnPropertyChanged);
                this.目录源radioButton.DataBindings.Add("Checked", ImageAcqDevice.Instance, "IsDirectorySource", true, DataSourceUpdateMode.OnPropertyChanged);
                this.拍照点textBox.Text = "X:" + ((PointCloudAcq)this._function).AcqParam.GrabPoint.X.ToString("f5") + "   Y:" +
                ((PointCloudAcq)this._function).AcqParam.GrabPoint.Y.ToString("f5") + "   Theta:" + ((PointCloudAcq)this._function).AcqParam.GrabPoint.Angle.ToString("f5");
                //////////////////////////////
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

        public void DisplayExcuteResult(object sender, ExcuteCompletedEventArgs e)  // 这里一定要加一个键值对变量来收集，以免出现重合
        {
            try
            {
                if (e.DataContent == null) return;     // 在图形窗口只显示世界坐标元素，像素元素在图像窗口显示
                switch (e.DataContent.GetType().Name) //这里只接受XLD轮廓或3D对象轮廓
                {
                    case nameof(HObjectModel3D):
                        this.drawObject.PointCloudModel3D = new PointCloudData((HObjectModel3D)e.DataContent);
                        break;
                    case nameof(PointCloudData):
                        this.drawObject.PointCloudModel3D = ((PointCloudData)e.DataContent);
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }


        private void PointCloudAcqForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (cts != null)
                    cts.Cancel();
                ImageAcqDevice.Instance.Save(); // 保存全局参数
                this.bs.DataSourceChanged -= new EventHandler(this.bindingNavigatorDataSourceChanged);
                BaseFunction.ExcuteCompleted -= new ExcuteCompletedEventHandler(DisplayExcuteResult);
                this.drawObject.GrayValueInfo -= new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
            }
            catch
            {

            }
        }
        private void hWindowControl1_MouseMove(object sender, GrayValueInfoEventArgs e)
        {
            if (e.GaryValue.Length > 0)
                this.灰度值1Label.Text = e.GaryValue[0].ToString();
            else
                this.灰度值1Label.Text = 0.ToString();
            ///////////////////////////////////////////
            if (e.GaryValue.Length > 1)
                this.灰度值2Label.Text = e.GaryValue[1].ToString();
            else
                this.灰度值2Label.Text = 0.ToString();
            /////////////////////////////////////////
            if (e.GaryValue.Length > 2)
                this.灰度值3Label.Text = e.GaryValue[2].ToString();
            else
                this.灰度值3Label.Text = 0.ToString();
            ///////////////////////////////////////////////
            this.行坐标Label.Text = "x: " + e.Row.ToString();
            this.列坐标Label.Text = "y: " + e.Col.ToString();
        }
        private void bindingNavigatorDataSourceChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.bs.Current != null)
                {
                    ((PointCloudAcq)this._function).FileIndex = 0;
                    this.drawObject.PointCloudModel3D = ((PointCloudAcq)this._function).AcqParam.ReadPointCloud(this.bs.Current.ToString(), this.采集源comboBox.Text);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }




        #region 数据视图右键菜单项
        //private void addContextMenu()
        //{
        //    ContextMenuStrip ContextMenuStrip1 = new ContextMenuStrip();
        //    // 添加右键菜单 
        //    ToolStripItem[] items = new ToolStripMenuItem[]
        //    {
        //        new ToolStripMenuItem("删除"),
        //        new ToolStripMenuItem("清空"),
        //        new ToolStripMenuItem("矩形阵列"),
        //        new ToolStripMenuItem("圆形阵列"),
        //        new ToolStripMenuItem("移动选定位置"),
        //    };
        //    ContextMenuStrip1.Items.AddRange(items); // 在滑赋值前不能调用 
        //    ContextMenuStrip1.ItemClicked += new ToolStripItemClickedEventHandler(dataGridViewContextMenuStrip_ItemClicked);
        //    this.dataGridView1.ContextMenuStrip = ContextMenuStrip1;
        //}
        //private void dataGridViewContextMenuStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        //{
        //    string name = e.ClickedItem.Text;
        //    int index = 0;
        //    MoveCommandParam wcsPoint, wcsPointAdd;
        //    try
        //    {
        //        ((ContextMenuStrip)sender).Close();
        //        switch (name)
        //        {
        //            case "删除":
        //                ((ContextMenuStrip)sender).Close();
        //                if (this.dataGridView1.CurrentRow == null) return;
        //                this.dataGridView1.Rows.Remove(this.dataGridView1.CurrentRow);
        //                for (int i = 0; i < this.dataGridView1.Rows.Count; i++)
        //                {
        //                    this.dataGridView1.Rows[i].HeaderCell.Value = i.ToString();
        //                }
        //                break;
        //            //////////////////////////////////////
        //            case "清空":
        //                ((ContextMenuStrip)sender).Close();
        //                ((FunctionBlock.PointCloudAcq)this._function).CommandParamt.Clear();
        //                break;

        //            case "矩形阵列":
        //                if (this.dataGridView1.CurrentRow == null) return;
        //                index = this.dataGridView1.CurrentRow.Index;
        //                RectangleArrayDataForm rectform = new RectangleArrayDataForm();
        //                rectform.Owner = this;
        //                rectform.ShowDialog();
        //                wcsPoint = ((FunctionBlock.PointCloudAcq)this._function).CommandParamt[index];
        //                for (int i = 0; i < rectform.RowCount; i++)
        //                {
        //                    for (int j = 0; j < rectform.ColCount; j++)
        //                    {
        //                        if (i == 0 && j == 0) continue; //选定行不变
        //                        wcsPointAdd = new MoveCommandParam();
        //                        switch(wcsPoint.MoveType)
        //                        {
        //                            case enMoveType.点运动:
        //                                wcsPointAdd.AxisParam.X = wcsPoint.AxisParam.X + rectform.OffsetX * j;
        //                                wcsPointAdd.AxisParam.Y = wcsPoint.AxisParam.Y + rectform.OffsetY * i;
        //                                break;
        //                            case enMoveType.直线运动:
        //                                wcsPointAdd.AxisParam.X = wcsPoint.AxisParam.X + rectform.OffsetX * j;
        //                                wcsPointAdd.AxisParam.Y = wcsPoint.AxisParam.Y + rectform.OffsetY * i;
        //                                wcsPointAdd.AxisParam2.X = wcsPoint.AxisParam2.X + rectform.OffsetX * j;
        //                                wcsPointAdd.AxisParam2.Y = wcsPoint.AxisParam2.Y + rectform.OffsetY * i;
        //                                break;
        //                        }
        //                        ((FunctionBlock.PointCloudAcq)this._function).CommandParamt.Add(wcsPointAdd);
        //                        Thread.Sleep(100);
        //                    }
        //                }
        //                rectform.Close();
        //                break;

        //            case "圆形阵列":
        //                CircleArrayDataForm circleForm = new CircleArrayDataForm();
        //                circleForm.Owner = this;
        //                circleForm.ShowDialog();
        //                wcsPoint = ((FunctionBlock.PointCloudAcq)this._function).CommandParamt[index];
        //                HTuple homMat2dIdentity, homMat2dRotate, Qx1, Qy1, Qx2, Qy2;
        //                for (int i = 1; i < circleForm.ArrayNum; i++)
        //                {
        //                    HOperatorSet.HomMat2dIdentity(out homMat2dIdentity);
        //                    HOperatorSet.HomMat2dRotate(homMat2dIdentity, circleForm.Add_Deg * i * Math.PI / 180, circleForm.Ref_X, circleForm.Ref_Y, out homMat2dRotate);
        //                    HOperatorSet.AffineTransPoint2d(homMat2dRotate, new HTuple(wcsPoint.AxisParam.X + circleForm.Ref_X), new HTuple(wcsPoint.AxisParam.Y + circleForm.Ref_Y), out Qx1, out Qy1);
        //                    HOperatorSet.AffineTransPoint2d(homMat2dRotate, new HTuple(wcsPoint.AxisParam2.X + circleForm.Ref_X), new HTuple(wcsPoint.AxisParam2.Y + circleForm.Ref_Y), out Qx2, out Qy2);
        //                    wcsPointAdd = new MoveCommandParam();
        //                    switch (wcsPoint.MoveType)
        //                    {
        //                        case enMoveType.点运动:
        //                            wcsPointAdd.AxisParam.X = Qx1[0].D;
        //                            wcsPointAdd.AxisParam.Y = Qy1[0].D;
        //                            break;
        //                        case enMoveType.直线运动:
        //                            wcsPointAdd.AxisParam.X = Qx1[0].D;
        //                            wcsPointAdd.AxisParam.Y = Qy1[0].D;
        //                            wcsPointAdd.AxisParam2.X = Qx2[0].D;
        //                            wcsPointAdd.AxisParam2.Y = Qy2[0].D;
        //                            break;
        //                    }
        //                }
        //                break;

        //            case "移动到激光位置":
        //                if (this.dataGridView1.CurrentRow == null) return;
        //                index = this.dataGridView1.CurrentRow.Index;
        //                MoveCommandParam CommandParam, affineCommandParam;
        //                CommandParam = ((FunctionBlock.PointCloudAcq)this._function).CommandParamt[index];
        //                MotionCardManage.GetCard(CommandParam.CoordSysName).MoveMultyAxis(CommandParam.CoordSysName, CommandParam.MoveAxis, CommandParam.MoveSpeed, CommandParam.AxisParam);
        //                break;
        //            ///////////////////////////////////////////////
        //            default:
        //                break;
        //        }
        //    }
        //    catch
        //    {
        //    }
        //}
        #endregion


        private void 激光源radioButton_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (激光源radioButton.Checked)
                {
                    this.采集源comboBox.Enabled = true;
                    this.单文件路径textBox.Enabled = false;
                    this.readFileButton.Enabled = false;
                }
            }
            catch
            {

            }
        }

        private void 文件源radioButton_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (文件源radioButton.Checked)
                {
                    this.采集源comboBox.Enabled = false;
                    this.单文件路径textBox.Enabled = true;
                    this.readFileButton.Enabled = true; ;
                }
            }
            catch
            {

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
        private void 激光采集源comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (采集源comboBox.SelectedIndex == -1) return;
                this.acqSource = AcqSourceManage.Instance.GetCamAcqSource(((PointCloudAcq)this._function).AcqParam.AcqSourceName);
                //this.acqSource = ((ImageAcq)this._function).AcqSourceName;
                if (this.acqSource == null) // return;
                    this.acqSource = new AcqSource();
                if (this.acqSource.Sensor == null) return;
                this.采集源comboBox.Text = this.acqSource.Name;
                switch (this.acqSource.Sensor?.ConfigParam.SensorType)
                {
                    case enUserSensorType.点激光:
                    case enUserSensorType.线激光:
                    case enUserSensorType.面激光:
                        AddForm(this.panel1, new LaserParamForm(this.acqSource.Sensor?.LaserParam, ((PointCloudAcq)this._function).AcqParam));
                        break;
                    case enUserSensorType.面阵相机:
                    case enUserSensorType.线阵相机:
                        AddForm(this.panel1, new CameraParamForm(this.acqSource.Sensor?.CameraParam));
                        break;
                    default:
                        AddForm(this.panel1, new LaserParamForm(this.acqSource.Sensor?.LaserParam, ((PointCloudAcq)this._function).AcqParam));
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void 采集源comboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            try
            {
                if (采集源comboBox.SelectedIndex == -1) return;
                if (采集源comboBox.SelectedItem == null) return;
                ((PointCloudAcq)this._function).AcqParam.AcqSourceName = this.采集源comboBox.SelectedItem.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }


        private void readFileButton_Click(object sender, EventArgs e)
        {
            FileOperate fo = new FileOperate();
            string path = fo.OpenFile();
            try
            {
                if (path != null && path.Trim().Length > 0)
                {
                    //////////////////////////
                    ((PointCloudAcq)_function).AcqParam.FilePath.Clear();
                    ((PointCloudAcq)_function).AcqParam.FilePath.Add(path);
                    ((PointCloudAcq)_function).AcqParam.SingleFilePath = path;
                    this.单文件路径textBox.Text = path;
                    ((PointCloudAcq)_function).AcqParam.FolderPath = "";
                    ((PointCloudAcq)_function).Dist1DataHandle = ((PointCloudAcq)_function).AcqParam.ReadPointCloud(path, ((PointCloudAcq)_function).AcqParam.AcqSourceName);
                    this.drawObject.PointCloudModel3D = ((PointCloudAcq)_function).Dist1DataHandle;
                    /////////////////////////////////
                    this.bs.DataSource = null;
                    this.bs.DataSource = ((PointCloudAcq)this._function).AcqParam.FilePath;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void 拍照点textBox_TextChanged(object sender, EventArgs e)
        {
            string[] name = 拍照点textBox.Text.Split(new string[] { "X:", "Y:", "Theta:" }, StringSplitOptions.RemoveEmptyEntries);
            if (name.Length < 3) return;
            double X, Y, Theta;
            bool result1 = double.TryParse(name[0].Trim(), out X);
            bool result2 = double.TryParse(name[1].Trim(), out Y);
            bool result3 = double.TryParse(name[2].Trim(), out Theta);
            if (result1 && result2 && result3)
            {
                ((PointCloudAcq)this._function).AcqParam.GrabPoint = new userWcsVector(X, Y, 0, Theta);
            }
            else
                MessageBox.Show("数据转换报错");
        }

        private void readDirectoryButton_Click(object sender, EventArgs e)
        {
            try
            {
                string[] path = null;
                FolderBrowserDialog fold = new FolderBrowserDialog();
                fold.ShowDialog();
                this.多文件目录textBox.Text = fold.SelectedPath;
                if (this.多文件目录textBox.Text.Trim().Length > 0)
                {
                    ((PointCloudAcq)_function).AcqParam.FolderPath = this.多文件目录textBox.Text;
                    if (fold.SelectedPath.Trim().Length > 0 && this.文件类型comboBox.SelectedItem.ToString().Length > 0)
                    {
                        path = Directory.GetFiles(fold.SelectedPath, this.文件类型comboBox.SelectedItem.ToString());
                        if (path != null)
                        {
                            ((PointCloudAcq)_function).AcqParam.FilePath.Clear();
                            foreach (var item in path)
                            {
                                ((PointCloudAcq)_function).AcqParam.FilePath.Add(item);
                            }
                            this.bs.DataSource = null;
                            this.bs.DataSource = ((PointCloudAcq)this._function).AcqParam.FilePath;
                        }
                    }

                }
            }
            catch
            {
                MessageBox.Show(new Exception().ToString());
            }
        }

        private void 文件类型comboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            try
            {
                /////////////
                string[] path = null;
                if (this.多文件目录textBox.Text.Trim().Length > 0 && this.文件类型comboBox.SelectedItem.ToString().Length > 0)
                {
                    path = Directory.GetFiles(this.多文件目录textBox.Text, this.文件类型comboBox.SelectedItem.ToString());
                    if (path != null)
                    {
                        ((PointCloudAcq)_function).AcqParam.FilePath.Clear();
                        foreach (var item in path)
                        {
                            ((PointCloudAcq)_function).AcqParam.FilePath.Add(item);
                        }
                        this.bs.DataSource = null;
                        this.bs.DataSource = ((PointCloudAcq)this._function).AcqParam.FilePath;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void bindingNavigatorMoveNextItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.bs.Current != null)
                {
                    ((PointCloudAcq)this._function).FileIndex = this.bs.Position;
                    this.drawObject.PointCloudModel3D = ((PointCloudAcq)this._function).AcqParam.ReadPointCloud(this.bs.Current.ToString(), this.采集源comboBox.Text);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void bindingNavigatorMovePreviousItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.bs.Current != null)
                {
                    ((PointCloudAcq)this._function).FileIndex = this.bs.Position;
                    this.drawObject.PointCloudModel3D = ((PointCloudAcq)this._function).AcqParam.ReadPointCloud(this.bs.Current.ToString(), this.采集源comboBox.Text);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void bindingNavigatorMoveFirstItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.bs.Current != null)
                {
                    ((PointCloudAcq)this._function).FileIndex = this.bs.Position;
                    this.drawObject.PointCloudModel3D = ((PointCloudAcq)this._function).AcqParam.ReadPointCloud(this.bs.Current.ToString(), this.采集源comboBox.Text);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void bindingNavigatorMoveLastItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.bs.Current != null)
                {
                    ((PointCloudAcq)this._function).FileIndex = this.bs.Position;
                    this.drawObject.PointCloudModel3D = ((PointCloudAcq)this._function).AcqParam.ReadPointCloud(this.bs.Current.ToString(), this.采集源comboBox.Text);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }



    }
}

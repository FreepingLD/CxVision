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
    public partial class LaserLineScanStandardForm : Form
    {
        private bool isFormClose = false;
        private object _objectDataModel;
        private CancellationTokenSource cts;
        private Form form;
        private IFunction _function;
        private AutoReSizeFormControls arfc = new AutoReSizeFormControls();
        private VisualizeView drawImageObject;
        private VisualizeView drawModelObject3D;

        public LaserLineScanStandardForm(IFunction function,TreeNode node)
        {
            this._function = function;
            InitializeComponent();
            this.Text = function.GetPropertyValues("名称").ToString();
            this.drawImageObject = new VisualizeView(this.hWindowControl1);
            //initEvent(this.hWindowControl1);
            new ListBoxWrapClass().InitListBox(this.listBox1, node, 2);
            new DataGridViewWrapClass().InitDataGridView(this, function, this.dataGridView1, ((FunctionBlock.LaserScanAcqStandard)this._function).Coord1Table);
        }
        private void LaserLineScanStandardForm_Load(object sender, EventArgs e)
        {
            // 注册事件
            BaseFunction.PointsCloudAcqComplete += new PointCloudAcqCompleteEventHandler(this.PointCloudAcqComplete_Event);
            //DataInteractionClass.getInstance().ExcuteCompleted += new ExcuteCompletedEventHandler(this.DisplayObjectModel);
            BindProperty();

        }
        private void BindProperty()
        {
            try
            {
                this.dataGridView1.DataSource = ((FunctionBlock.LaserScanAcqStandard)this._function).Coord1Table;
                //this.dataGridView2.DataSource = ((LaserScanAcqStandard)this._function).Coord2Table;
                this.激光采集源comboBox.DataSource = FunctionBlock.AcqSourceManage.Instance.LaserAcqSourceList();// SensorManage.LaserList;
                this.激光采集源comboBox.DisplayMember = "Name";
                this.运动类型comboBox.DataSource = Enum.GetNames(typeof(enAxisName));
                // this.运动类型comboBox.DisplayMember = "Key";
                ///////////////////////////////////////////////
                this.激光采集源comboBox.DataBindings.Add("SelectedItem", ((FunctionBlock.LaserScanAcqStandard)this._function), "LaserAcqSource", true, DataSourceUpdateMode.OnPropertyChanged);
                //this.Y间隔textBox.DataBindings.Add("Text", (LaserScanAcqStandard)_function, "Move_Step", true, DataSourceUpdateMode.OnPropertyChanged);
                this.运动类型comboBox.DataBindings.Add("Text", (FunctionBlock.LaserScanAcqStandard)this._function, "MotionType", true, DataSourceUpdateMode.OnPropertyChanged);
                this.起点偏置textBox.DataBindings.Add("Text", (FunctionBlock.LaserScanAcqStandard)_function, "OffsetDist", true, DataSourceUpdateMode.OnPropertyChanged); //   
                this.圆数量textBox.DataBindings.Add("Text", (FunctionBlock.LaserScanAcqStandard)_function, "CircleNum", true, DataSourceUpdateMode.OnPropertyChanged); //   
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
        private void 显示条目comboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            FunctionBlock.LaserScanAcqStandard.enShowItems item;
            Enum.TryParse(this.显示条目comboBox.Text.Trim(), out item);
            //switch (item)
            //{
            //    case LaserScanAcqStandard.enShowItems.距离1对象:
            //        this.DisplayObjectModel(this._function.GetPropertyValues("距离1对象"));
            //        break;
            //    case LaserScanAcqStandard.enShowItems.距离2对象:
            //        this.DisplayObjectModel(this._function.GetPropertyValues("距离2对象"));
            //        break;
            //    case LaserScanAcqStandard.enShowItems.厚度对象:
            //        this.DisplayObjectModel(this._function.GetPropertyValues("厚度对象"));
            //        break;
            //}
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
                                if (this.isFormClose) return;
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
        private void PointCloudAcqComplete_Event(object send, PointCloudAcqCompleteEventArgs e)
        {
            if (((FunctionBlock.LaserScanAcqStandard)this._function).LaserAcqSource == null) return;
            string name = ((FunctionBlock.LaserScanAcqStandard)this._function).LaserAcqSource.Sensor.ConfigParam.SensorName;
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

        private void Clearbutton1_Click(object sender, EventArgs e)
        {
            try
            {
                ((FunctionBlock.LaserScanAcqStandard)this._function).Coord1Table.Clear();
                //_function.SetPropertyValues("清空坐标点1", null); //添加坐标点1
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void deletePointButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.dataGridView1.CurrentRow == null) return;
                int index = this.dataGridView1.CurrentRow.Index;
                ((FunctionBlock.LaserScanAcqStandard)this._function).Coord1Table.Rows.RemoveAt(index);
                //this.dataGridView1.Rows.Remove(this.dataGridView1.CurrentRow);
                //for (int i = 0; i < this.dataGridView1.Rows.Count; i++)
                //{
                //    this.dataGridView1.Rows[i].HeaderCell.Value = i.ToString();
                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void LaserLineScanForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                this.isFormClose = true;
                // 注消事件
                //DataInteractionClass.getInstance().ExcuteCompleted -= new ExcuteCompletedEventHandler(this.DisplayObjectModel);
                BaseFunction.PointsCloudAcqComplete -= new PointCloudAcqCompleteEventHandler(this.PointCloudAcqComplete_Event);
                if (cts != null)
                    cts.Cancel();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }



        private void 导入坐标点1button_Click(object sender, EventArgs e)
        {
            try
            {
                FileOperate fo = new FileOperate();
                ImportData(fo.OpenFile());
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
                double X = 0, Y = 0, Z = 0, U = 0, V = 0, W = 0, X2 = 0, Y2 = 0, Z2 = 0, U2 = 0, V2 = 0, W2 = 0; ;
                if (Path == null || Path.Length == 0) return;
                using (StreamReader reader = new StreamReader(Path))
                {
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
                        if (st.Length > 6)
                            X2 = Convert.ToDouble(st[6]);
                        if (st.Length > 7)
                            Y2 = Convert.ToDouble(st[7]);
                        if (st.Length > 8)
                            Z2 = Convert.ToDouble(st[8]);
                        if (st.Length > 9)
                            U2 = Convert.ToDouble(st[9]);
                        if (st.Length > 10)
                            V2 = Convert.ToDouble(st[10]);
                        if (st.Length > 11)
                            W2 = Convert.ToDouble(st[11]);
                        ((FunctionBlock.LaserScanAcqStandard)_function).Coord1Table.Rows.Add(new object[] { X, Y, Z, U, V, W, X2, Y2, Z2, U2, V2, W2 }); // 每更改一次数据源会触发一次dataGridView的数据绑定完成事件
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void 添加点1button_Click(object sender, EventArgs e)
        {
            try
            {
                double X1 = 0, Y1 = 0, Z1 = 0, U1 = 0, V1 = 0, W1 = 0, X2 = 0, Y2 = 0, Z2 = 0, U2 = 0, V2 = 0, W2 = 0, U=0, V = 0, W = 0;
                double.TryParse(this.X轴坐标textBox.Text, out X1);
                double.TryParse(this.Y轴坐标textBox.Text, out Y1);
                double.TryParse(this.Z轴坐标textBox.Text, out Z1);
                //double.TryParse(this.U轴坐标textBox.Text, out U1);
                //double.TryParse(this.V轴坐标textBox.Text, out V1);
                //double.TryParse(this.W轴坐标textBox.Text, out W1);
                /////////////////////////////////
                double.TryParse(this.X轴坐标2textBox.Text, out X2);
                double.TryParse(this.Y轴坐标2textBox.Text, out Y2);
                double.TryParse(this.Z轴坐标2textBox.Text, out Z2);
                //double.TryParse(this.U轴坐标2textBox.Text, out U2);
                //double.TryParse(this.V轴坐标2textBox.Text, out V2);
                //double.TryParse(this.W轴坐标2textBox.Text, out W2);
                if (((FunctionBlock.LaserScanAcqStandard)_function).LaserAcqSource != null)
                {
                    ((FunctionBlock.LaserScanAcqStandard)_function).LaserAcqSource.GetAxisPosition(enAxisName.U轴, out U);
                    ((FunctionBlock.LaserScanAcqStandard)_function).LaserAcqSource.GetAxisPosition(enAxisName.V轴, out V);
                    ((FunctionBlock.LaserScanAcqStandard)_function).LaserAcqSource.GetAxisPosition(enAxisName.W轴, out W);
                }
                ((FunctionBlock.LaserScanAcqStandard)_function).Coord1Table.Rows.Add(X1, Y1, Z1, U, V, W, X2, Y2, Z2, U, V, W);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            try
            {
                HTuple Qx, Qy;
                userWcsLine wcsLine = new userWcsLine();
                this.drawImageObject.AttachPropertyData.Clear();
                userWcsCoordSystem userWcsCoordSystem = ((FunctionBlock.LaserScanAcqStandard)_function).extractRefSource2Data();
                this.drawImageObject.AttachPropertyData.Add(userWcsCoordSystem);
                DataGridViewCellCollection dataGridViewCellCollection;
                for (int i = 0; i < this.dataGridView1.Rows.Count; i++)
                {
                    this.dataGridView1.Rows[i].HeaderCell.Value = i.ToString();
                    dataGridViewCellCollection = this.dataGridView1.Rows[i].Cells;
                    if (dataGridViewCellCollection.Count > 0)
                        wcsLine.X1 = Convert.ToDouble(dataGridViewCellCollection[0].Value);
                    if (dataGridViewCellCollection.Count > 1)
                        wcsLine.Y1 = Convert.ToDouble(dataGridViewCellCollection[1].Value);
                    if (dataGridViewCellCollection.Count > 2)
                        wcsLine.Z1 = Convert.ToDouble(dataGridViewCellCollection[2].Value);
                    if (dataGridViewCellCollection.Count > 3)
                        wcsLine.U1 = Convert.ToDouble(dataGridViewCellCollection[3].Value);
                    if (dataGridViewCellCollection.Count > 4)
                        wcsLine.V1 = Convert.ToDouble(dataGridViewCellCollection[4].Value);
                    if (dataGridViewCellCollection.Count > 5)
                        wcsLine.Theta1 = Convert.ToDouble(dataGridViewCellCollection[5].Value);
                    ////////////////////////
                    if (dataGridViewCellCollection.Count > 6)
                        wcsLine.X2 = Convert.ToDouble(dataGridViewCellCollection[6].Value);
                    if (dataGridViewCellCollection.Count > 7)
                        wcsLine.Y2 = Convert.ToDouble(dataGridViewCellCollection[7].Value);
                    if (dataGridViewCellCollection.Count > 8)
                        wcsLine.Z2 = Convert.ToDouble(dataGridViewCellCollection[8].Value);
                    if (dataGridViewCellCollection.Count > 9)
                        wcsLine.U2 = Convert.ToDouble(dataGridViewCellCollection[9].Value);
                    if (dataGridViewCellCollection.Count > 10)
                        wcsLine.V2 = Convert.ToDouble(dataGridViewCellCollection[10].Value);
                    if (dataGridViewCellCollection.Count > 11)
                        wcsLine.Theta2 = Convert.ToDouble(dataGridViewCellCollection[11].Value);
                    // 将点绘制到图像上                  
                    HOperatorSet.AffineTransPoint2d(userWcsCoordSystem.GetCurrentHomMat2D(),new HTuple(wcsLine.X1, wcsLine.X2), new HTuple(wcsLine.Y1, wcsLine.Y2), out Qx, out Qy);
                    wcsLine.X1 = Qx[0].D;
                    wcsLine.Y1 = Qy[0].D;
                    wcsLine.X2 = Qx[1].D;
                    wcsLine.Y2 = Qy[1].D;
                    this.drawImageObject.AttachPropertyData.Add(wcsLine);
                }
                /////////////////////
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
                userWcsLine wcsLine;
                int index = this.dataGridView1.CurrentRow.Index;
                for (int i = 0; i < this.drawImageObject.AttachPropertyData.Count; i++)
                {
                    if (i == index + 1)
                    {
                        if (this.drawImageObject.AttachPropertyData[i] is userWcsLine)
                        {
                            wcsLine = (userWcsLine)this.drawImageObject.AttachPropertyData[i];
                            wcsLine.Color = enColor.orange;
                            this.drawImageObject.AttachPropertyData[i] = wcsLine;
                        }
                    }
                    else
                    {
                        if (this.drawImageObject.AttachPropertyData[i] is userWcsLine)
                        {
                            wcsLine = (userWcsLine)this.drawImageObject.AttachPropertyData[i];
                            wcsLine.Color = enColor.white;
                            this.drawImageObject.AttachPropertyData[i] = wcsLine;
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


    }
}

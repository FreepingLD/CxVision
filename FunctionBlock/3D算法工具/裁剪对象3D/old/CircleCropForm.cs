using AlgorithmsLibrary;
using Common;
using FunctionBlock;
using HalconDotNet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using View;

namespace FunctionBlock
{
    public partial class CircleCropForm : Form
    {
        private bool isFormClose = false;
        private IFunction _function;
        private VisualizeView drawObject;
        private ListBoxWrapClass ListBoxWrapClass1, ListBoxWrapClass2;
        public CircleCropForm(IFunction function,TreeNode node)
        {
            this._function = function;
            InitializeComponent();
            this.Text = ((FunctionBlock.CircleCrop)this._function).Name;
            ListBoxWrapClass1 = new ListBoxWrapClass();
            ListBoxWrapClass1.InitListBox(this.listBox1, node);
            ListBoxWrapClass2 = new ListBoxWrapClass();
            ListBoxWrapClass2.InitListBox(this.listBox2, node, 2);
            this.drawObject = new VisualizeView(this.hWindowControl1, false); //, ((CircleCrop)this._function).ListCircle
            new DataGridViewWrapClass().InitDataGridView(this, function, this.dataGridView1, ((FunctionBlock.CircleCrop)this._function).Coord1Table);
        }
        private void CircleCropForm_Load(object sender, EventArgs e)
        {
            BaseFunction.ExcuteCompleted += new ExcuteCompletedEventHandler(this.DisplayObjectModel);
            ListBoxWrapClass.ItemsChangeToForm += new ItemsChangeEventHandler(listbox_AddItems);
            BindProperty();
        }
        private void BindProperty()
        {
            try
            {
                this.显示条目comboBox.DataSource = Enum.GetNames(typeof(FunctionBlock.CircleCrop.enShowItems));
                this.dataGridView1.DataSource = ((FunctionBlock.CircleCrop)this._function).Coord1Table;
                this.半径textBox.DataBindings.Add("Text", (FunctionBlock.CircleCrop)this._function, "Radius", true, DataSourceUpdateMode.OnPropertyChanged);
                this.截取方向comboBox.DataBindings.Add("Text", (FunctionBlock.CircleCrop)this._function, "InsideOrOutside", true, DataSourceUpdateMode.OnPropertyChanged);
                //////////////
                for (int i = 0; i < this.dataGridView1.Rows.Count; i++)
                {
                    this.dataGridView1.Rows[i].HeaderCell.Value = i.ToString();
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
            }
        }
        private void DisplayObjectModel(object send, ExcuteCompletedEventArgs e)
        {
            try
            {
                switch (e.DataContent.GetType().Name)
                {
                    case "HObjectModel3D":
                        this.drawObject.PointCloudModel3D = new PointCloudData( (HObjectModel3D)e.DataContent );
                        break;
                    case "HObjectModel3D[]":
                        this.drawObject.PointCloudModel3D = new PointCloudData((HObjectModel3D[])e.DataContent);
                        break;
                }
            }
            catch
            {

            }
        }
        private void listbox_AddItems(object send, ItemsChangeEventArgs e)
        {
            try
            {
                object object3D;
                if (e.ItemName == null || e.ItemName.Trim().Length == 0) return;
                if (e.ItemName.Split('.').Length == 1)
                    object3D = ((IFunction)e.Function).GetPropertyValues(e.ItemName);
                else
                    object3D = ((IFunction)e.Function).GetPropertyValues(e.ItemName.Split('.')[1]);
                ///////////////////////////////////////////
                if (object3D != null)
                {
                    switch (object3D.GetType().Name)
                    {
                        case "HObjectModel3D":
                            this.drawObject.PointCloudModel3D = new PointCloudData((HObjectModel3D)object3D);
                            break;
                        case "HObjectModel3D[]":
                            this.drawObject.PointCloudModel3D = new PointCloudData((HObjectModel3D[])object3D);
                            break;
                    }
                }
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
        private void 显示条目comboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            try
            {
                object object3D = this._function.GetPropertyValues(this.显示条目comboBox.SelectedItem.ToString().Trim());
                ///////////////////////////////////////////
                if (object3D != null)
                {
                    switch (object3D.GetType().Name)
                    {
                        case "HObjectModel3D":
                            this.drawObject.PointCloudModel3D = new PointCloudData((HObjectModel3D)object3D);
                            break;
                        case "HObjectModel3D[]":
                            this.drawObject.PointCloudModel3D = new PointCloudData((HObjectModel3D[])object3D);
                            break;
                        case "PointCloudData":
                            this.drawObject.PointCloudModel3D = ((PointCloudData)object3D);
                            break;
                        case "ImageDataClass":
                            this.drawObject.BackImage = (ImageDataClass)object3D;
                            break;
                        case "HImage":
                            this.drawObject.BackImage = new ImageDataClass((HImage)object3D); // 图形窗口不显示图像
                            break;
                        case "XldDataClass":
                            this.drawObject.XldContourData = (XldDataClass)object3D;
                            break;
                        case "HXLDCont":
                            this.drawObject.XldContourData =  new XldDataClass((HXLDCont)object3D);
                            break;
                        case "XldDataClass[]":
                            this.drawObject.XldContourData = (XldDataClass)object3D;
                            break;
                        case "RegionDataClass":
                            this.drawObject.RegionData =  (RegionDataClass)object3D;
                            break;
                        case "RegionDataClass[]":
                            this.drawObject.RegionData = (RegionDataClass)object3D;
                            break;
                        case "userPixPoint[]":
                            this.drawObject.AttachPropertyData.Clear();
                            userPixPoint[] pixPoint = (userPixPoint[])object3D;
                            for (int i = 0; i < pixPoint.Length; i++)
                            {
                                this.drawObject.AttachPropertyData.Add(pixPoint[i]);
                            }
                            break;
                        case "userWcsPoint[]":
                            this.drawObject.AttachPropertyData.Clear();
                            userWcsPoint[] wcsPoint = (userWcsPoint[])object3D;
                            for (int i = 0; i < wcsPoint.Length; i++)
                            {
                                this.drawObject.AttachPropertyData.Add(wcsPoint[i]);
                            }
                            break;
                    }
                }
            }
            catch
            {

            }
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
                        Task.Run(() =>
                        {
                            this.drawObject.AttachPropertyData.Clear();
                            if (this._function.Execute(null).Succss)
                            {
                                if (this.isFormClose) return;
                                this.Invoke(new Action(() =>
                                {
                                    this.toolStripStatusLabel1.Text = "执行结果:";
                                    this.运行结果toolStripStatusLabel.Text = "成功";
                                    this.运行结果toolStripStatusLabel.ForeColor = Color.Green;
                                }));
                            }
                            else
                            {
                                if (this.isFormClose) return;
                                this.Invoke(new Action(() =>
                                {
                                    this.toolStripStatusLabel1.Text = "执行结果:";
                                    this.运行结果toolStripStatusLabel.Text = "失败";
                                    this.运行结果toolStripStatusLabel.ForeColor = Color.Red;
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
                    this.drawObject.Show3D();
                    break;

                default:
                    break;
            }
        }



        private void 绘制圆形button_Click(object sender, EventArgs e)
        {
            HalconLibrary ha = new HalconLibrary();
            userWcsCircle wcsCircle;
            //////////////////////////////////////
            try
            {
                this.drawObject.DrawWcsCircleOnWindow(enColor.white, 0, 0, out wcsCircle);
                /////////////
                ((FunctionBlock.CircleCrop)this._function).Coord1Table.Rows.Add(wcsCircle.X, wcsCircle.Y, wcsCircle.Z, wcsCircle.Radius);
            }
            catch (Exception ee)
            {
                MessageBox.Show("绘制圆形button_Click操作失败" + ee.ToString());
            }
        }

        private void 图形取点button_Click(object sender, EventArgs e)
        {
            HalconLibrary ha = new HalconLibrary();
            double radius;
            userWcsPoint wcsPoint;
            try
            {
                while (true)
                {
                    if (double.TryParse(this.半径textBox.Text, out radius))
                    {
                        this.drawObject.DrawWcsPointOnWindow(enColor.white, 0, 0, out wcsPoint);
                        //ha.DrawPointOnWindow(this.hWindowControl1.HalconWindow, this.drawObject.CamParam, this.drawObject.CamPose, out wcsPoint);
                        ((FunctionBlock.CircleCrop)this._function).Coord1Table.Rows.Add(wcsPoint.X, wcsPoint.Y, wcsPoint.Z, radius);
                    }
                    if (!this.循环添加checkBox.Checked) break;
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show("图形取点button_Click操作报错" + ee.ToString());
            }
        }
        private void 删点button_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.dataGridView1.CurrentRow == null) return;
                int index = this.dataGridView1.CurrentRow.Index;
                ((FunctionBlock.CircleCrop)this._function).Coord1Table.Rows.RemoveAt(index);
                //this.drawObject.ShowAttachProperty();
            }
            catch (Exception ee)
            {
                MessageBox.Show("删点button_Click操作报错" + ee.ToString());
            }
        }

        private void 清空button_Click(object sender, EventArgs e)
        {
            try
            {
                ((FunctionBlock.CircleCrop)this._function).Coord1Table.Rows.Clear();
                //this.drawObject.ShowAttachProperty();

            }
            catch (Exception ee)
            {
                MessageBox.Show("删除点button_Click" + ee.ToString());
            }
        }


        private void CircleCropForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                this.isFormClose = true;
                if (this.drawObject != null)
                    this.drawObject.ClearDrawingObject();
                BaseFunction.ExcuteCompleted -= new ExcuteCompletedEventHandler(this.DisplayObjectModel);
                ListBoxWrapClass.ItemsChangeToForm -= new ItemsChangeEventHandler(listbox_AddItems);
            }
            catch (Exception ee)
            {
                MessageBox.Show("删除点button_Click" + ee.ToString());
            }
        }

        private void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            try
            {
                HTuple Qx, Qy;
                userWcsCircle wcsCircle = new userWcsCircle();
                this.drawObject.AttachPropertyData.Clear();
                userWcsCoordSystem userWcsCoordSystem = ((FunctionBlock.CircleCrop)_function).extractRefSource2Data();
                this.drawObject.AttachPropertyData.Add(userWcsCoordSystem);
                DataGridViewCellCollection dataGridViewCellCollection;
                for (int i = 0; i < this.dataGridView1.Rows.Count; i++)
                {
                    this.dataGridView1.Rows[i].HeaderCell.Value = i.ToString();
                    dataGridViewCellCollection = this.dataGridView1.Rows[i].Cells;
                    if (dataGridViewCellCollection.Count > 0)
                        wcsCircle.X = Convert.ToDouble(dataGridViewCellCollection[0].Value);
                    if (dataGridViewCellCollection.Count > 1)
                        wcsCircle.Y = Convert.ToDouble(dataGridViewCellCollection[1].Value);
                    if (dataGridViewCellCollection.Count > 2)
                        wcsCircle.Z = Convert.ToDouble(dataGridViewCellCollection[2].Value);
                    if (dataGridViewCellCollection.Count > 3)
                        wcsCircle.Radius = Convert.ToDouble(dataGridViewCellCollection[3].Value);
                    //将点绘制到图像上                  
                    HOperatorSet.AffineTransPoint2d(userWcsCoordSystem.GetCurrentHomMat2D(), wcsCircle.X, wcsCircle.Y, out Qx, out Qy);
                    wcsCircle.X = Qx.D;
                    wcsCircle.Y = Qy.D;
                    wcsCircle.Color = enColor.white;
                    this.drawObject.AttachPropertyData.Add(wcsCircle);
                }
                if (this.drawObject.PointCloudModel3D == null)
                    this.drawObject.UpdataGraphicView();
                else
                    this.drawObject.DisplayModelObject3D();
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
                userWcsCircle wcsCircel;
                int index = this.dataGridView1.CurrentRow.Index;
                for (int i = 0; i < this.drawObject.AttachPropertyData.Count; i++)
                {
                    if (i == index)
                    {
                        if (this.drawObject.AttachPropertyData[i] is userWcsCircle)
                        {
                            wcsCircel = (userWcsCircle)this.drawObject.AttachPropertyData[i];
                            wcsCircel.Color = enColor.orange;
                            this.drawObject.AttachPropertyData[i] = wcsCircel;
                        }
                    }
                    else
                    {
                        if (this.drawObject.AttachPropertyData[i] is userWcsCircle)
                        {
                            wcsCircel = (userWcsCircle)this.drawObject.AttachPropertyData[i];
                            wcsCircel.Color = enColor.white;
                            this.drawObject.AttachPropertyData[i] = wcsCircel;
                        }
                    }
                }
                ///
                if (this.drawObject.PointCloudModel3D == null)
                    this.drawObject.UpdataGraphicView();
                else
                    this.drawObject.ShowAttachProperty();
            }
            catch (Exception ee)
            {
                MessageBox.Show("dataGridView1_SelectionChanged操作失败" + ee.ToString());
            }
        }

        private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false;
        }







    }
}

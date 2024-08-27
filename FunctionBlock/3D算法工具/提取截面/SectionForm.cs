
using Common;
using FunctionBlock;
using HalconDotNet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using View;

namespace FunctionBlock
{
    public partial class SectionForm : Form
    {
        private IFunction _function;
        private userDrawLineROI drawObject;
        private bool isFormClose = false;
        TreeNode node;
        public SectionForm(IFunction function, TreeNode node)
        {
            this.node = node;
            this._function = function;
            InitializeComponent();
            this.Text = ((Section)this._function).Name;
            drawObject = new userDrawLineROI(this.hWindowControl1, false);
            new ListBoxWrapClass().InitListBox(this.listBox1, node);
            new ListBoxWrapClass().InitListBox(this.listBox2, node, 2);
            //new DataGridViewWrapClass().InitDataGridView(this, this._function, this.dataGridView1, ((Section)this._function).Coord1Table);
        }
        private void AppBaseForm_Load(object sender, EventArgs e)
        {
            BaseFunction.ExcuteCompleted += new ExcuteCompletedEventHandler(this.DisplayObjectModel);
            ListBoxWrapClass.ItemsChangeToForm += new ItemsChangeEventHandler(listbox_AddItems);
            BindProperty();
            this.drawObject.PointCloudModel3D = new PointCloudData(((Section)this._function).DataHandle3D);
        }
        public enum enShowItems
        {
            输入3D对象,
            轮廓3D对象,
        }
        private void BindProperty()
        {
            try
            {
                // 绑定机台坐标点 ，绑定类型只能为C#使用的数据类型
                this.显示条目comboBox.DataSource = Enum.GetNames(typeof(enShowItems));
                this.dataGridView1.DataSource = ((Section)this._function).ListLine;
                this.截面范围textBox.DataBindings.Add("Text", (Section)this._function, "Dist_offset", true, DataSourceUpdateMode.OnPropertyChanged);
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

        public void DisplayObjectModel(object sender, ExcuteCompletedEventArgs e)
        {
            try
            {
                if (e.DataContent != null) // 在图形窗口只显示世界坐标元素，像素元素在图像窗口显示
                {
                    switch (e.DataContent.GetType().Name) //这里只接受XLD轮廓或3D对象轮廓
                    {
                        case "HObjectModel3D":
                            this.drawObject.PointCloudModel3D = new PointCloudData((HObjectModel3D)e.DataContent);
                            break;
                        case "HObjectModel3D[]":
                            this.drawObject.PointCloudModel3D = new PointCloudData((HObjectModel3D[])e.DataContent);
                            break;
                        case "PointCloudData":
                            this.drawObject.PointCloudModel3D = ((PointCloudData)e.DataContent);
                            break;
                        case "HImage":
                            this.drawObject.BackImage = new ImageDataClass((HImage)e.DataContent); // 图形窗口不显示图像
                            break;
                        case "ImageDataClass":
                            this.drawObject.BackImage = (ImageDataClass)e.DataContent;
                            break;
                        case "XldDataClass":
                            this.drawObject.XldContourData = (XldDataClass)e.DataContent;
                            break;
                        case "XldDataClass[]":
                            this.drawObject.XldContourData =                             this.drawObject.XldContourData = (XldDataClass)e.DataContent;;
                            break;
                        case "HXLDCont":
                            this.drawObject.XldContourData = new XldDataClass((HXLDCont)e.DataContent) ;
                            break;
                        case "RegionDataClass":
                            this.drawObject.RegionData =  (RegionDataClass)e.DataContent ;
                            break;
                        case "RegionDataClass[]":
                            this.drawObject.RegionData = (RegionDataClass)e.DataContent;
                            break;
                        case "userWcsCircle":
                            this.drawObject.AttachPropertyData.Clear();
                            this.drawObject.AttachPropertyData.Add((userWcsCircle)e.DataContent); //(userWcsCircle)e.DataContent
                            this.drawObject.UpdataGraphicView(); // 背影不刷新
                            break;
                        case "userWcsCircleSector":
                            this.drawObject.AttachPropertyData.Clear();
                            this.drawObject.AttachPropertyData.Add((userWcsCircleSector)e.DataContent);
                            this.drawObject.UpdataGraphicView();
                            break;
                        case "userWcsEllipse":
                            this.drawObject.AttachPropertyData.Clear();
                            this.drawObject.AttachPropertyData.Add((userWcsEllipse)e.DataContent);
                            this.drawObject.UpdataGraphicView();
                            break;
                        case "userWcsEllipseSector":
                            this.drawObject.AttachPropertyData.Clear();
                            this.drawObject.AttachPropertyData.Add((userWcsEllipseSector)e.DataContent);
                            this.drawObject.UpdataGraphicView();
                            break;
                        case "userWcsLine":
                            this.drawObject.AttachPropertyData.Clear();
                            this.drawObject.AttachPropertyData.Add((userWcsLine)e.DataContent);
                            this.drawObject.UpdataGraphicView();
                            break;
                        case "userWcsPoint":
                            this.drawObject.AttachPropertyData.Clear();
                            this.drawObject.AttachPropertyData.Add((userWcsPoint)e.DataContent);
                            this.drawObject.UpdataGraphicView();
                            break;
                        case "userWcsRectangle1":
                            this.drawObject.AttachPropertyData.Clear();
                            this.drawObject.AttachPropertyData.Add((userWcsRectangle1)e.DataContent);
                            this.drawObject.UpdataGraphicView();
                            break;
                        case "userWcsRectangle2":
                            this.drawObject.AttachPropertyData.Clear();
                            this.drawObject.AttachPropertyData.Add((userWcsRectangle2)e.DataContent);
                            this.drawObject.UpdataGraphicView();
                            break;
                        case "userPixPoint[]":
                            this.drawObject.AttachPropertyData.Clear();
                            userPixPoint[] pixPoint = (userPixPoint[])e.DataContent;
                            for (int i = 0; i < pixPoint.Length; i++)
                            {
                                this.drawObject.AttachPropertyData.Add(pixPoint[i]);
                            }
                            break;
                        case "userWcsPoint[]":
                            this.drawObject.AttachPropertyData.Clear();
                            userWcsPoint[] wcsPoint = (userWcsPoint[])this._function.GetPropertyValues(this.显示条目comboBox.Text.Trim());
                            for (int i = 0; i < wcsPoint.Length; i++)
                            {
                                this.drawObject.AttachPropertyData.Add(wcsPoint[i]);
                            }
                            break;
                    }
                }
            }
            catch (Exception he)
            {

            }
        }
        private void listbox_AddItems(object send, ItemsChangeEventArgs e)
        {
            try
            {
                object object3D = null;
                if (e.ItemName == null || e.ItemName.Trim().Length == 0) return;
                object3D = ((Section)this._function).DataHandle3D;
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
                        case "XldDataClass":
                            this.drawObject.XldContourData = (XldDataClass)object3D;
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
        private void 显示条目comboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            try
            {
                object object3D = null;
                switch(this.显示条目comboBox.SelectedItem.ToString())
                {
                    default:
                    case nameof( enShowItems.输入3D对象):
                         object3D = ((Section)this._function).DataHandle3D;
                        break;
                    case nameof(enShowItems.轮廓3D对象):
                         object3D = ((Section)this._function).OutObjectModel3D;
                        break;
                }
                ///////////////////////////////////////////
                if (object3D != null)
                {
                    switch (object3D.GetType().Name)
                    {
                        case "HObjectModel3D":
                            this.drawObject.PointCloudModel3D = new PointCloudData((HObjectModel3D)object3D );
                            break;
                        case "HObjectModel3D[]":
                            this.drawObject.PointCloudModel3D =new PointCloudData((HObjectModel3D[])object3D);
                            break;
                        case "ImageDataClass":
                            this.drawObject.BackImage = (ImageDataClass)object3D;
                            break;
                        case "HImage":
                            this.drawObject.BackImage = new ImageDataClass((HImage)object3D); // 图形窗口不显示图像
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
                        if (this.toolStripStatusLabel2.Text == "等待……") break;
                        this.toolStripStatusLabel2.Text = "等待……";
                        this.toolStripStatusLabel2.ForeColor = Color.Yellow;
                        Task.Run(() =>
                        {
                            this.drawObject.AttachPropertyData.Clear();
                            if (this._function.Execute(this.node).Succss)
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
                                this.Invoke(new Action(() =>
                                {
                                    if (this.isFormClose) return;
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
            AlgorithmsLibrary.HalconLibrary ha = new AlgorithmsLibrary.HalconLibrary();
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
                    this.drawObject.Show3D();
                    this.toolStripButton_Translate.CheckState = CheckState.Unchecked;
                    break;
                default:
                    break;
            }
        }

        // 更新3D对象模型 ；响应测量完成/及响应鼠标点击事件
        private void 添加截面button_Click(object sender, EventArgs e)
        {
            AlgorithmsLibrary.HalconLibrary ha = new AlgorithmsLibrary.HalconLibrary();
            userWcsLine wcsLine;
            //////////////////////////////////////
            try
            {
                this.drawObject.DrawWcsLineOnWindow(enColor.white, 0, 0, out wcsLine);
                /////////////    
                ((Section)this._function).ListLine.Add(new drawWcsLine(wcsLine.X1, wcsLine.Y1, wcsLine.Z1, wcsLine.X2, wcsLine.Y2, wcsLine.Z2));
            }
            catch (Exception ee)
            {
                MessageBox.Show("添加截面button_Click操作失败" + ee.ToString());
            }
        }
        private void 删除截面button_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.dataGridView1.CurrentRow == null) return;
                int index = this.dataGridView1.CurrentRow.Index;
                ((Section)this._function).ListLine.RemoveAt(index);
            }
            catch (Exception ee)
            {
                MessageBox.Show("删除截面button_Click操作失败" + ee.ToString());
            }
        }
        private void 清空截面button_Click(object sender, EventArgs e)
        {
            try
            {
                ((Section)this._function).ListLine.Clear();
            }
            catch (Exception ee)
            {
                MessageBox.Show("清空截面button_Click操作失败" + ee.ToString());
            }
        }

        private void SectionForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // 注消事件
            try
            {
                this.isFormClose = true;
                if (this.drawObject != null)
                    this.drawObject.ClearDrawingObject();
                BaseFunction.ExcuteCompleted -= new ExcuteCompletedEventHandler(this.DisplayObjectModel);
                ListBoxWrapClass.ItemsChangeToForm -= new ItemsChangeEventHandler(listbox_AddItems);
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
                //this.drawObject.AttachPropertyData.Clear();
                //userWcsCoordSystem userWcsCoordSystem = ((Section)_function).extractRefSource2Data();
                /////////////////////////////////////////////////////////////////////
                //this.drawObject.AttachPropertyData.Add(userWcsCoordSystem);
                //DataGridViewCellCollection dataGridViewCellCollection;
                //for (int i = 0; i < this.dataGridView1.Rows.Count; i++)
                //{
                //    this.dataGridView1.Rows[i].HeaderCell.Value = (i+1).ToString();
                //    dataGridViewCellCollection = this.dataGridView1.Rows[i].Cells;
                //    if (dataGridViewCellCollection.Count > 0)
                //        wcsLine.x1 = Convert.ToDouble(dataGridViewCellCollection[0].Value);
                //    if (dataGridViewCellCollection.Count > 1)
                //        wcsLine.y1 = Convert.ToDouble(dataGridViewCellCollection[1].Value);
                //    if (dataGridViewCellCollection.Count > 2)
                //        wcsLine.z1 = Convert.ToDouble(dataGridViewCellCollection[2].Value);
                //    if (dataGridViewCellCollection.Count > 3)
                //        wcsLine.x2 = Convert.ToDouble(dataGridViewCellCollection[3].Value);
                //    if (dataGridViewCellCollection.Count > 4)
                //        wcsLine.y2 = Convert.ToDouble(dataGridViewCellCollection[4].Value);
                //    if (dataGridViewCellCollection.Count > 5)
                //        wcsLine.z2 = Convert.ToDouble(dataGridViewCellCollection[5].Value);
                //    // 将点绘制到图像上                  
                //    HOperatorSet.AffineTransPoint2d(userWcsCoordSystem.GetVariationHomMat2D(), new HTuple(wcsLine.x1, wcsLine.x2), new HTuple(wcsLine.y1, wcsLine.y2), out Qx, out Qy);
                //    wcsLine.x1 = Qx[0].D;
                //    wcsLine.y1 = Qy[0].D;
                //    wcsLine.x2 = Qx[1].D;
                //    wcsLine.y2 = Qy[1].D;
                //    wcsLine.color = enColor.white;
                //    this.drawObject.AttachPropertyData.Add(wcsLine);
                //}
                //if (this.drawObject.PointCloudModel3D == null)
                //    this.drawObject.UpdataGraphicView();
                //else
                //    this.drawObject.DisplayModelObject3D();
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

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (this.dataGridView1.SelectedRows == null) return;
                if (this.dataGridView1.CurrentRow == null) return;
                int index = this.dataGridView1.CurrentRow.Index;
                if (this.drawObject.PointCloudModel3D == null) return;
                this.drawObject.SetParam(((FunctionBlock.Section)this._function).ListLine[index]);
                this.drawObject.AttachDrawingObjectToWindow();
            }
            catch (Exception ee)
            {
                MessageBox.Show("dataGridView1_SelectionChanged操作失败" + ee.ToString());
            }
        }
    }
}

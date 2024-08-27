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
    public partial class ImageReduceFormOld : Form
    {
        private Form form;
        private IFunction _function;
        private userDrawRect2ROI drawObject;
        private object _objectDataModel;
        public ImageReduceFormOld(IFunction function, TreeNode node)
        {
            this._function = function;
            InitializeComponent();
            this.drawObject = new userDrawRect2ROI(this.hWindowControl1, false);
            new ListBoxWrapClass().InitListBox(this.listBox1, node);
            this.addContextMenu(this.标准_dataGridView);
        }
        private void ImageReduceForm_Load(object sender, EventArgs e)
        {
            // 注册事件
            this.drawObject.GrayValueInfo += new GrayValueInfoEventHandler(GetGrayValueInfo);
            BaseFunction.ExcuteCompleted += new ExcuteCompletedEventHandler(this.DisplayObjectModel);
            ListBoxWrapClass.ItemsChangeToForm += new ItemsChangeEventHandler(listbox_AddItems);
            BindProperty();
            //////////////////////////////////////////////////////////
        }
        private void BindProperty()
        {
            try
            {
                //this.标准_dataGridView.DataSource = ((ImageReduce)this._function).ReduceParam.ReduceRegion;
                //this.显示条目comboBox.DataSource = Enum.GetValues(typeof(enShowItem));
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

        private void addContextMenu(DataGridView dataGridView)
        {
            ContextMenuStrip ContextMenuStrip1 = new ContextMenuStrip();
            // 添加右键菜单 
            ToolStripItem[] items = new ToolStripMenuItem[]
            {
                new ToolStripMenuItem("删除"),
                new ToolStripMenuItem("清空"),
            };
            ContextMenuStrip1.Name = dataGridView.Name;
            ContextMenuStrip1.Items.AddRange(items); // 在滑赋值前不能调用 
            ContextMenuStrip1.ItemClicked += new ToolStripItemClickedEventHandler(dataGridViewContextMenuStrip_ItemClicked);
            dataGridView.ContextMenuStrip = ContextMenuStrip1;
        }
        private void dataGridViewContextMenuStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            string name = e.ClickedItem.Text;
            try
            {
                //((ContextMenuStrip)sender).Close();
                //switch (name)
                //{
                //    case "删除":
                //        ((ContextMenuStrip)sender).Close();
                //        if (this.标准_dataGridView.CurrentRow == null || this.标准_dataGridView.CurrentRow.Index < 0) return;
                //        ((FunctionBlock.ImageReduce)this._function).ReduceParam.ReduceRegion.RemoveAt(this.标准_dataGridView.CurrentRow.Index);
                //        ///////////////
                //        this.drawObject.AttachPropertyData.Clear();
                //        foreach (var item in ((FunctionBlock.ImageReduce)this._function).ReduceParam.ReduceRegion)
                //            this.drawObject.AttachPropertyData.Add(item.GetXLD());
                //        this.drawObject.DrawingGraphicObject();
                //        break;
                //    //////////////////////////////////////
                //    case "清空":
                //        ((ContextMenuStrip)sender).Close();
                //        ((FunctionBlock.ImageReduce)this._function).ReduceParam.ReduceRegion.Clear();
                //        ///////////////
                //        this.drawObject.AttachPropertyData.Clear();
                //        foreach (var item in ((FunctionBlock.ImageReduce)this._function).ReduceParam.ReduceRegion)
                //            this.drawObject.AttachPropertyData.Add(item.GetXLD());
                //        this.drawObject.DrawingGraphicObject();
                //        break;
                //    ///////////////////////////////////////////////
                //    default:
                //        break;
                //}
            }
            catch
            {
            }
        }

        // 获取鼠标位置处的高度值
        private void GetGrayValueInfo(object sender, GrayValueInfoEventArgs e)
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
            this.行坐标Label.Text = e.Row.ToString();
            this.列坐标Label.Text = e.Col.ToString();
        }
        private void DisplayObjectModel(object sender, ExcuteCompletedEventArgs e)  // 这里一定要加一个键值对变量来收集，以免出现重合
        {
            try
            {
                if (e.DataContent != null)
                {
                    switch (e.DataContent.GetType().Name) //这里只接受XLD轮廓或3D对象轮廓
                    {
                        case "HImage":
                            this.drawObject.BackImage = new ImageDataClass((HImage)e.DataContent);
                            this._objectDataModel = this.drawObject.BackImage;
                            this.drawObject.AttachPropertyData.Clear();
                            break;
                        case "ImageDataClass":
                            this.drawObject.BackImage = (ImageDataClass)e.DataContent;
                            this._objectDataModel = this.drawObject.BackImage;
                            break;
                        case "XldDataClass":
                            this.drawObject.XldContourData = (XldDataClass)e.DataContent;
                            break;
                        case "XldDataClass[]":
                            this.drawObject.XldContourData = (XldDataClass)e.DataContent; ;
                            break;
                        case "HXLDCont":
                            this.drawObject.XldContourData = new XldDataClass((HXLDCont)e.DataContent);
                            break;
                        case "HRegion":
                            this.drawObject.RegionData = new RegionDataClass((HRegion)e.DataContent);
                            break;
                        case "RegionDataClass[]":
                            this.drawObject.RegionData = (RegionDataClass)e.DataContent;
                            break;
                        case "RegionDataClass":
                            //this.drawObject.RegionData = (RegionDataClass)e.DataContent;
                            this.drawObject.AttachPropertyData.Clear();
                            this.drawObject.AttachPropertyData.Add(e.DataContent);
                            this.drawObject.DrawingGraphicObject();
                            break;
                    }
                }
            }
            catch (Exception he)
            {
                MessageBox.Show("DisplayObjectModel->操作失败");
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
                            this.drawObject.RegionData = (RegionDataClass)object3D;
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
                        if (this.toolStripStatusLabel2.Text == "等待……") break;
                        this.toolStripStatusLabel2.Text = "等待……";
                        this.toolStripStatusLabel2.ForeColor = Color.Yellow;
                        Task.Run(() =>
                        {
                            if (this._function.Execute(null).Succss)
                            {
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
                    this.drawObject.Show3D();
                    this.toolStripButton_Translate.CheckState = CheckState.Unchecked;

                    break;
                default:
                    break;
            }
        }

        private void BlobForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (this.drawObject != null)
                    this.drawObject.ClearDrawingObject();
                // 注消事件
                this.drawObject.GrayValueInfo -= new GrayValueInfoEventHandler(GetGrayValueInfo);
                ListBoxWrapClass.ItemsChangeToForm -= new ItemsChangeEventHandler(listbox_AddItems);
                BaseFunction.ExcuteCompleted -= new ExcuteCompletedEventHandler(this.DisplayObjectModel);
            }
            catch
            {

            }
        }

        private void 显示条目comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.显示条目comboBox.SelectedIndex == -1) return;
                switch (this.显示条目comboBox.SelectedItem.ToString())
                {
                    case nameof(enShowItem.源图像):
                        this.drawObject.AttachPropertyData.Clear();
                        this.drawObject.BackImage = ((ImageReduce)this._function).ImageData;
                        break;
                    case nameof(enShowItem.区域):
                        this.drawObject.AttachPropertyData.Clear();
                        this.drawObject.BackImage = ((ImageReduce)this._function).ReduceImage;
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void 设置分割区域button_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    userPixRectangle2 pixRecrt2;
            //    this.drawObject.SetParam(null);
            //    this.drawObject.DrawPixRect2OnWindow(enColor.red, out pixRecrt2);
            //    ((FunctionBlock.ImageReduce)this._function).ReduceParam.ReduceRegion.Add(new drawPixRect2(pixRecrt2.row, pixRecrt2.col, pixRecrt2.rad, pixRecrt2.length1, pixRecrt2.length2));
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.ToString());
            //}
        }
        private void 示教Blob区域_Btn_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    userPixRectangle2 outPixRect2;
            //    if (this.标准_dataGridView.CurrentRow.Index == -1) return;
            //    drawPixRect2 pixRect2 = ((FunctionBlock.ImageReduce)this._function).ReduceParam.ReduceRegion[this.标准_dataGridView.CurrentRow.Index];
            //    this.drawObject.AttachPropertyData.Clear();
            //    this.drawObject.ModifyPixRect2OnWindow(pixRect2.GetUserPixRectangle2(),out outPixRect2);
            //    ((FunctionBlock.ImageReduce)this._function).ReduceParam.ReduceRegion[this.标准_dataGridView.CurrentRow.Index] = outPixRect2.GetDrawPixRect2();
            //    this.drawObject.AttachPropertyData.Add(outPixRect2);
            //    this.drawObject.DrawingGraphicObject();
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.ToString());
            //}
        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //try
            //{
            //    if (e.RowIndex < 0) return;
            //    this.drawObject.AttachPropertyData.Clear();
            //    drawPixRect2 pixRect2;
            //    foreach (var item in ((FunctionBlock.ImageReduce)this._function).ReduceParam.ReduceRegion)
            //    {
            //        pixRect2 = item.AffinePixRect2(((FunctionBlock.ImageReduce)this._function).ReduceParam.PixCoordSys.GetVariationHomMat2D());
            //        this.drawObject.AttachPropertyData.Add(pixRect2.GetUserPixRectangle2());
            //    }
            //    this.drawObject.DrawingGraphicObject();
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.ToString());
            //}
        }

        private void dataGridView1_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            try
            {
                //this.drawObject.AttachPropertyData.Clear();
                //foreach (var item in ((FunctionBlock.ImageReduce)this._function).ReduceParam.ReduceRegion)
                //    this.drawObject.AttachPropertyData.Add(item.GetXLD());
                //this.drawObject.DrawingGraphicObject();
            }
            catch(Exception ex)
            {
                MessageBox.Show("添加行数据时出错" + ex.ToString());
            }
        }

    }

}

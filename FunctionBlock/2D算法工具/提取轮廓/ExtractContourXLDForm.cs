using AlgorithmsLibrary;
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
    public partial class ExtractContourXLDForm : Form
    {
        private object _objectDataModel;
        //
        private Form form;
        private IFunction _function;
        private VisualizeView drawObject;

        public ExtractContourXLDForm(IFunction function,TreeNode node)
        {
            this._function = function;
            InitializeComponent();
            this.Text = function.GetPropertyValues("名称").ToString();
            this.drawObject = new VisualizeView(this.hWindowControl1,true);
            initEvent(this.hWindowControl1);
            new ListBoxWrapClass().InitListBox(this.listBox1, node);
        }
        private void ExtractContourXLDForm_Load(object sender, EventArgs e)
        {
            BindProperty();
            BaseFunction.ExcuteCompleted += new ExcuteCompletedEventHandler(this.DisplayObjectModel);
            ListBoxWrapClass.ItemsChangeToForm += new ItemsChangeEventHandler(listbox_AddItems);
        }
        private void DisplayObjectModel(object sender, ExcuteCompletedEventArgs e)  // 这里一定要加一个键值对变量来收集，以免出现重合
        {
            try
            {
                if (e.DataContent != null)
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
                            this.drawObject.BackImage = new ImageDataClass((HImage)e.DataContent);
                            this._objectDataModel = this.drawObject.BackImage;
                            break;
                        case "ImageDataClass":
                            this.drawObject.BackImage = (ImageDataClass)e.DataContent;
                            this._objectDataModel = this.drawObject.BackImage;
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
                            this.drawObject.XldContourData  = new XldDataClass((HXLDCont)e.DataContent) ;
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
                            this.drawObject.RegionData =  (RegionDataClass)object3D;
                            break;
                        case "RegionDataClass[]":
                            this.drawObject.RegionData = (RegionDataClass)object3D;
                            break;
                    }
                }
            }
            catch
            {

            }
        }
        private void BindProperty()
        {
            try
            {
                this.显示条目comboBox.DataSource = Enum.GetNames(typeof(FunctionBlock.ExtractXLD.enShowItems));
                this.提取轮廓方法comboBox.DataSource = Enum.GetNames(typeof(FunctionBlock.ExtractXLD.enExtractMethodXLD));
                // 创建匹配参数
                this.提取轮廓方法comboBox.DataBindings.Add("Text", (FunctionBlock.ExtractXLD)this._function, "ExtractMethodXLD", true, DataSourceUpdateMode.OnPropertyChanged);
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
            //try
            //{
            //    switch (this.显示条目comboBox.Text.Trim())
            //    {
            //        case "输入对象":
            //            this.drawObject.BackImage = (ImageDataClass)this._function.GetPropertyValues(this.显示条目comboBox.Text.Trim());
            //            break;
            //        case "输出对象":
            //            this.drawObject.XldContourData =new XldDataClass[] { ((XldDataClass)this._function.GetPropertyValues(this.显示条目comboBox.Text.Trim())) };
            //            break;
            //    }
            //}
            //catch (HalconException e9)
            //{
            //    MessageBox.Show("显示条目comboBox_SelectionChangeCommitted" + e9.ToString());
            //}
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

        private void initEvent(HWindowControl hWindowControl)
        {
            this.hWindowControl1 = hWindowControl;
            this.hWindowControl1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.hWindowControl1_MouseMove);
        }

        // 获取鼠标位置处的高度值
        private void hWindowControl1_MouseMove(object sender, MouseEventArgs e)
        {
            HTuple value = -1.0;
            HalconLibrary ha = new HalconLibrary();
            if (this._objectDataModel == null) return;
            ////////////////////
            switch (this._objectDataModel.GetType().Name)
            {
                case "HImage": //HObject
                    ha.GetGrayValueOnWindow(this.hWindowControl1.HalconWindow, (HImage)this._objectDataModel, e.Y, e.X, out value);
                    break;

                case "ImageDataClass": //HObject
                    ha.GetGrayValueOnWindow(this.hWindowControl1.HalconWindow, ((ImageDataClass)this._objectDataModel).Image, e.Y, e.X, out value);
                    break;
            }
            if (value != null && value.Length > 0)
            {
                this.灰度值1Label.Text = value[2].D.ToString();
                this.灰度值2Label.Text = 0.ToString();
                this.灰度值2Label.Text = 0.ToString();
                this.行坐标Label.Text = value[0].D.ToString();
                this.列坐标Label.Text = value[1].D.ToString();
            }
        }

        private void 提取轮廓方法comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (form != null) form.Dispose();
            if (this.提取轮廓方法comboBox.Text == "") return;
            switch (this.提取轮廓方法comboBox.Text.Trim())
            {
                case "edges_sub_pix":
                    form = new EdgesSubPixForm(this._function);
                    AddForm(this.参数groupBox, form);
                    break;
                case "edges_color_sub_pix":
                    form = new EdgesColorSubPixForm(this._function);
                    AddForm(this.参数groupBox, form);
                    break;
                case "threshold_sub_pix":
                    form = new ThresholdSubPixForm(this._function);
                    AddForm(this.参数groupBox, form);
                    break;
                case "zero_crossing_sub_pix":
                    form = new ZeroCrossingSubPixForm(this._function);
                    AddForm(this.参数groupBox, form);
                    break;
                case "lines_gauss":
                    form = new LinesGaussForm(this._function);
                    AddForm(this.参数groupBox, form);
                    break;
            }
        }

        private void DeformableMatchForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (this.drawObject != null)
                    this.drawObject.ClearDrawingObject();
                // 注消事件
                ListBoxWrapClass.ItemsChangeToForm -= new ItemsChangeEventHandler(listbox_AddItems);
                BaseFunction.ExcuteCompleted -= new ExcuteCompletedEventHandler(this.DisplayObjectModel);
            }
            catch
            {

            }
        }






    }
}

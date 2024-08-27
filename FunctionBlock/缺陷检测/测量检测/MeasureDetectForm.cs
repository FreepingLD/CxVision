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
    public partial class MeasureDetectForm : Form
    {
        protected Form form;
        protected IFunction _function;
        private VisualizeView drawObject;
        public MeasureDetectForm(TreeNode node)
        {
            InitializeComponent();
            this._function = node.Tag as IFunction;
            this.Text = this._function.GetPropertyValues("名称").ToString();
            this.drawObject = new VisualizeView(this.hWindowControl1, true);
            new ListBoxWrapClass().InitListBox(this.listBox1, node);
        }
        public void MeasureDetectForm_Load(object sender, EventArgs e)
        {
            // 注册事件
            this.drawObject.GrayValueInfo += new GrayValueInfoEventHandler(GetGrayValueInfo);
            BaseFunction.ExcuteCompleted += new ExcuteCompletedEventHandler(this.DisplayObjectModel);
            ListBoxWrapClass.ItemsChangeToForm += new ItemsChangeEventHandler(listbox_AddItems);
            BindProperty();
                       
        }
        public MeasureDetectForm()
        {

        }
        protected void AddForm(Panel MastPanel, Form form)
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
        protected void AddForm(GroupBox groupBox, Form form)
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
            输入对象1,
            输出对象,
        }
        protected  void BindProperty()
        {
            try
            {
                // 创建匹配参数
                this.显示条目comboBox.DataSource = Enum.GetNames(typeof(enShowItems));
                this.采样方向comboBox.DataSource = Enum.GetNames(typeof(enMeasureMethod));
                MeasureDetectParam param = ((MeasureDetect)this._function).DetectParam as MeasureDetectParam;
                this.采样方向comboBox.DataBindings.Add(nameof(this.采样方向comboBox.Text), param, nameof(param.MeasureMethod), true, DataSourceUpdateMode.OnPropertyChanged);
                this.采样间隔comboBox.DataBindings.Add(nameof(this.采样间隔comboBox.Text), param, nameof(param.SampleDist), true, DataSourceUpdateMode.OnPropertyChanged);
                this.采样间隔缩放comboBox.DataBindings.Add(nameof(this.采样间隔comboBox.Text), param, nameof(param.SampleScale), true, DataSourceUpdateMode.OnPropertyChanged);
                this.平滑系数comboBox.DataBindings.Add(nameof(this.平滑系数comboBox.Text), param, nameof(param.Sigma), true, DataSourceUpdateMode.OnPropertyChanged);
               
                this.边缘振幅comboBox.DataBindings.Add(nameof(this.边缘振幅comboBox.Text), param, nameof(param.Threshold), true, DataSourceUpdateMode.OnPropertyChanged);
                this.宽度合并距离comboBox.DataBindings.Add(nameof(this.宽度合并距离comboBox.Text), param, nameof(param.Width), true, DataSourceUpdateMode.OnPropertyChanged);
                this.高度合并距离comboBox.DataBindings.Add(nameof(this.高度合并距离comboBox.Text), param, nameof(param.Height), true, DataSourceUpdateMode.OnPropertyChanged);

                this.最小面积comboBox.DataBindings.Add(nameof(this.最小面积comboBox.Text), param, nameof(param.Area), true, DataSourceUpdateMode.OnPropertyChanged);
                
                /////////////////////////////////////////////////////////
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
            }
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
                            this.drawObject.PointCloudModel3D.ObjectModel3D = new HObjectModel3D[] { (HObjectModel3D)e.DataContent };
                            break;
                        case "HObjectModel3D[]":
                            this.drawObject.PointCloudModel3D.ObjectModel3D = (HObjectModel3D[])e.DataContent;
                            break;
                        case "PointCloudData":
                            this.drawObject.PointCloudModel3D = (PointCloudData)e.DataContent;
                            break;
                        case "HImage":
                            this.drawObject.ClearViewObject();
                            this.drawObject.BackImage = new ImageDataClass((HImage)e.DataContent); // 图形窗口不显示图像
                            break;
                        case "ImageDataClass":
                            this.drawObject.ClearViewObject();
                            this.drawObject.BackImage = (ImageDataClass)e.DataContent;
                            break;
                        case "XldDataClass":
                            this.drawObject.AddViewObject(new ViewData(((XldDataClass)e.DataContent).HXldCont, "red"));
                            break;
                        case "HXLDCont":
                            this.drawObject.AddViewObject(new ViewData(((HXLDCont)e.DataContent), "red"));
                            break;
                        case "RegionDataClass":
                            this.drawObject.AddViewObject(new ViewData(((RegionDataClass)e.DataContent).Region, "red"));
                            break;
                        case "RegionDataClass[]":
                            foreach (var item in (RegionDataClass[])e.DataContent)
                            {
                                this.drawObject.AddViewObject(new ViewData(item.Region, "red"));
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
                            this.drawObject.PointCloudModel3D.ObjectModel3D = new HObjectModel3D[] { (HObjectModel3D)object3D };
                            break;
                        case "HObjectModel3D[]":
                            this.drawObject.PointCloudModel3D.ObjectModel3D = (HObjectModel3D[])object3D;
                            break;
                        case "PointCloudData":
                            this.drawObject.PointCloudModel3D = (PointCloudData)object3D;
                            break;
                        case "ImageDataClass":
                            this.drawObject.BackImage = (ImageDataClass)object3D;
                            break;
                        case "XldDataClass":
                            this.drawObject.AddViewObject(new ViewData(((XldDataClass)object3D).HXldCont, "red"));
                            break;
                        case "HXLDCont":
                            this.drawObject.AddViewObject(new ViewData(((HXLDCont)object3D), "red"));
                            break;
                        case "RegionDataClass":
                            this.drawObject.AddViewObject(new ViewData(((RegionDataClass)object3D).Region, "red"));
                            break;
                        case "RegionDataClass[]":
                            foreach (var item in (RegionDataClass[])object3D)
                            {
                                this.drawObject.AddViewObject(new ViewData(item.Region, "red"));
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
                object object3D = this._function.GetPropertyValues(this.显示条目comboBox.Text.Trim());
                ///////////////////////////////////////////
                if (object3D != null)
                {
                    switch (object3D.GetType().Name)
                    {
                        case nameof(ImageDataClass):
                            this.drawObject.ClearViewObject();
                            this.drawObject.BackImage = (ImageDataClass)object3D;
                            break;
                        case nameof(RegionDataClass):
                            this.drawObject.ClearViewObject();
                            this.drawObject.AddViewObject(new ViewData(((RegionDataClass)object3D).Region, "red"));
                            break;
                    }
                }
            }
            catch(Exception ex)
            {

            }
        }
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
        private void 视图工具toolStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
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
        private void 运行toolStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
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
        protected void DeformableMatchForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // 注消事件
            try
            {
                if (this.drawObject != null)
                    this.drawObject.ClearDrawingObject();
                this.drawObject.GrayValueInfo -= new GrayValueInfoEventHandler(GetGrayValueInfo);
                BaseFunction.ExcuteCompleted -= new ExcuteCompletedEventHandler(this.DisplayObjectModel);
                ListBoxWrapClass.ItemsChangeToForm -= new ItemsChangeEventHandler(listbox_AddItems);
            }
            catch
            {

            }
        }


    }
}

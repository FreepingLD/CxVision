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
    public partial class GapDetectFormOld : Form
    {
        protected Form form;
        protected IFunction _function;
        private VisualizeView drawObject;
        public GapDetectFormOld(TreeNode node)
        {
            InitializeComponent();
            this._function = node.Tag as IFunction;
            this.Text = this._function.GetPropertyValues("名称").ToString();
            this.drawObject = new VisualizeView(this.hWindowControl1, true);
            new ListBoxWrapClass().InitListBox(this.listBox1, node);
        }
        public void FitLineForm_Load(object sender, EventArgs e)
        {
            // 注册事件
            BaseFunction.ExcuteCompleted += new ExcuteCompletedEventHandler(this.DisplayObjectModel);
            ListBoxWrapClass.ItemsChangeToForm += new ItemsChangeEventHandler(listbox_AddItems);
            BindProperty();

        }
        public GapDetectFormOld()
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
            输入区域,
            输出区域,
        }
        protected void BindProperty()
        {
            try
            {
                // 创建匹配参数
                this.显示条目comboBox.DataSource = Enum.GetNames(typeof(enShowItems));
                GapDetectParam param = ((GapDetect)this._function).Param as GapDetectParam;
                this.开运算宽度comboBox.DataBindings.Add(nameof(this.开运算宽度comboBox.Text), param, nameof(param.OpenWidth), true, DataSourceUpdateMode.OnPropertyChanged);
                this.开运算高度comboBox.DataBindings.Add(nameof(this.开运算高度comboBox.Text), param, nameof(param.OpenHeight), true, DataSourceUpdateMode.OnPropertyChanged);
                this.闭运算宽度comboBox.DataBindings.Add(nameof(this.闭运算宽度comboBox.Text), param, nameof(param.CloseWidth), true, DataSourceUpdateMode.OnPropertyChanged);
                this.闭运算高度comboBox.DataBindings.Add(nameof(this.闭运算高度comboBox.Text), param, nameof(param.CloseHeight), true, DataSourceUpdateMode.OnPropertyChanged);
                //this.启用开运算checkBox.DataBindings.Add(nameof(this.启用开运算checkBox.Checked), param, nameof(param.EnableOpen), true, DataSourceUpdateMode.OnPropertyChanged);
                //this.启用闭运算checkBox.DataBindings.Add(nameof(this.启用闭运算checkBox.Checked), param, nameof(param.EnableClose), true, DataSourceUpdateMode.OnPropertyChanged);
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
                            this.drawObject.BackImage = new ImageDataClass((HImage)e.DataContent); // 图形窗口不显示图像
                            break;
                        case "ImageDataClass":
                            this.drawObject.BackImage = (ImageDataClass)e.DataContent;
                            break;
                        case "RegionDataClass":
                            RegionDataClass regionDataClass = e.DataContent as RegionDataClass;
                            this.drawObject.ClearViewObject();
                            this.drawObject.AddViewObject(new ViewData(regionDataClass.Region, regionDataClass.Color.ToString()));
                            break;
                        case "RegionDataClass[]":
                            this.drawObject.ClearViewObject();
                            RegionDataClass[] regionDataClassArry = e.DataContent as RegionDataClass[];
                            if (regionDataClassArry != null)
                            {
                                foreach (var item in regionDataClassArry)
                                {
                                    this.drawObject.AddViewObject(new ViewData(item.Region, item.Color.ToString()));
                                }
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
                        case "HObjectModel3D":
                            this.drawObject.PointCloudModel3D = new PointCloudData((HObjectModel3D)object3D);
                            break;
                        case "HObjectModel3D[]":
                            this.drawObject.PointCloudModel3D = new PointCloudData((HObjectModel3D[])object3D);
                            break;
                        case "PointCloudData":
                            this.drawObject.PointCloudModel3D = (PointCloudData)object3D;
                            break;
                        case "ImageDataClass":
                            this.drawObject.BackImage = (ImageDataClass)object3D;
                            break;
                        case "RegionDataClass":
                            RegionDataClass regionDataClass = object3D as RegionDataClass;
                            this.drawObject.ClearViewObject();
                            this.drawObject.AddViewObject(new ViewData(regionDataClass.Region, regionDataClass.Color.ToString()));
                            break;
                        case "RegionDataClass[]":
                            this.drawObject.ClearViewObject();
                            RegionDataClass[] regionDataClassArry = object3D as RegionDataClass[];
                            if (regionDataClassArry != null)
                            {
                                foreach (var item in regionDataClassArry)
                                {
                                    this.drawObject.AddViewObject(new ViewData(item.Region, item.Color.ToString()));
                                }
                            }
                            break;
                    }
                }
            }
            catch
            {

            }
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
        protected void GapDetectForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // 注消事件
            try
            {
                if (this.drawObject != null)
                    this.drawObject.ClearDrawingObject();
                BaseFunction.ExcuteCompleted -= new ExcuteCompletedEventHandler(this.DisplayObjectModel);
                ListBoxWrapClass.ItemsChangeToForm -= new ItemsChangeEventHandler(listbox_AddItems);
            }
            catch
            {

            }
        }

        private void 示教Btn_Click(object sender, EventArgs e)
        {

        }

        private void 启用开运算checkBox_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if(this.启用开运算checkBox.Checked)
                {
                    this.开运算宽度comboBox.Enabled = true;
                    this.开运算高度comboBox.Enabled = true;
                }
                else
                {
                    this.开运算宽度comboBox.Enabled = false;
                    this.开运算高度comboBox.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void 启用闭运算checkBox_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.启用闭运算checkBox.Checked)
                {
                    this.闭运算宽度comboBox.Enabled = true;
                    this.闭运算高度comboBox.Enabled = true;
                }
                else
                {
                    this.闭运算宽度comboBox.Enabled = false;
                    this.闭运算高度comboBox.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }


    }
}

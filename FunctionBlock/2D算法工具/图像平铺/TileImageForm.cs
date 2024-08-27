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
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using View;

namespace FunctionBlock
{
    public partial class TileImageForm : Form
    {
        protected Form form;
        protected IFunction _function;
        private VisualizeView drawObject;
        public TileImageForm(IFunction function, TreeNode node)
        {
            InitializeComponent();
            this._function = function;
            this.drawObject = new VisualizeView(this.hWindowControl1);

        }
        private void MosaicImageForm_Load(object sender, EventArgs e)
        {
            BaseFunction.ExcuteCompleted += new ExcuteCompletedEventHandler(this.DisplayExcuteResult);
            ListBoxWrapClass.ItemsChangeToForm += new ItemsChangeEventHandler(listbox_AddItems);
            this.drawObject.GrayValueInfo += new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
            BindProperty();
        }
        public TileImageForm()
        {

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
            输入对象,
            输出对象,
        }
        private void BindProperty()
        {
            try
            {
                TileImageParam param = ((FunctionBlock.TileImage)this._function).Param;
                this.图像宽textBox.DataBindings.Add("Text", param, nameof(param.Width), true, DataSourceUpdateMode.OnPropertyChanged);
                this.图像高textBox.DataBindings.Add("Text", param, nameof(param.Height), true, DataSourceUpdateMode.OnPropertyChanged);
                this.行数量textBox.DataBindings.Add("Text", param, nameof(param.RowCount), true, DataSourceUpdateMode.OnPropertyChanged);
                this.列数量textBox.DataBindings.Add("Text", param, nameof(param.ColCount), true, DataSourceUpdateMode.OnPropertyChanged);
                this.行偏移textBox.DataBindings.Add("Text", param, nameof(param.OffsetRow), true, DataSourceUpdateMode.OnPropertyChanged);
                this.列偏移textBox.DataBindings.Add("Text", param, nameof(param.OffsetCol), true, DataSourceUpdateMode.OnPropertyChanged);
                this.Row1textBox.DataBindings.Add("Text", param, nameof(param.Row1), true, DataSourceUpdateMode.OnPropertyChanged);
                this.Col1textBox.DataBindings.Add("Text", param, nameof(param.Col1), true, DataSourceUpdateMode.OnPropertyChanged);
                this.Row2textBox.DataBindings.Add("Text", param, nameof(param.Row2), true, DataSourceUpdateMode.OnPropertyChanged);
                this.Col2textBox.DataBindings.Add("Text", param, nameof(param.Col2), true, DataSourceUpdateMode.OnPropertyChanged);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public void DisplayExcuteResult(object sender, ExcuteCompletedEventArgs e)  // 这里一定要加一个键值对变量来收集，以免出现重合
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
                    }
                }
            }
            catch
            {

            }
        }

        private void MosaicImageForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (this.drawObject != null)
                    this.drawObject.ClearDrawingObject();
                // 注消事件
                ListBoxWrapClass.ItemsChangeToForm -= new ItemsChangeEventHandler(listbox_AddItems);
                BaseFunction.ExcuteCompleted -= new ExcuteCompletedEventHandler(this.DisplayExcuteResult);
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
            this.行坐标Label.Text = e.Row.ToString();
            this.列坐标Label.Text = e.Col.ToString();
        }

        private void 运行toolStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            ToolStripItem item = e.ClickedItem;
            string name = item.Name;
            //int count = 1;
            switch (name)
            {
                case "运行toolStripButton":
                    if (this.运行结果toolStripStatusLabel.Text == "等待……") break;
                    this.运行结果toolStripStatusLabel.Text = "等待……";
                    this.运行结果toolStripStatusLabel.ForeColor = Color.Yellow;
                    Task.Run(() =>
                    {
                        if (this._function.Execute(null).Succss)
                        {
                            this.statusStrip1.Invoke(new Action(() =>
                            {
                                this.运行结果toolStripStatusLabel.Text = "成功";
                                this.运行结果toolStripStatusLabel.ForeColor = Color.Green;
                            }));
                        }
                        else
                        {
                            this.statusStrip1.Invoke(new Action(() =>
                            {
                                this.运行结果toolStripStatusLabel.Text = "失败";
                                this.运行结果toolStripStatusLabel.ForeColor = Color.Red;
                            }));
                        }
                    }
                    );
                    break;

                case "停止toolStripButton":
                    this.运行toolStripButton.Enabled = true;
                    break;

                default:
                    break;
            }
        }


    }
}

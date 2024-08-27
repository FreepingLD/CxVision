
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
    public partial class SaveImageForm : Form
    {
        private IFunction _function;
        private VisualizeView drawObject;

        public SaveImageForm(IFunction function,TreeNode node)
        {      
            this._function = function;
            InitializeComponent();
            this.drawObject = new VisualizeView(this.hWindowControl1,true);
            new ListBoxWrapClass().InitListBox(this.listBox1, node);
            new ListBoxWrapClass().InitListBox(this.listBox2, node,2);
            this.drawObject.BackImage = ((SaveImage)_function).ImageData;
        }
        private void ReadImageForm_Load(object sender, EventArgs e)
        {
            // 注册事件
            ListBoxWrapClass.ItemsChangeToForm += new ItemsChangeEventHandler(listbox_AddItems);
            this.drawObject.GrayValueInfo += new GrayValueInfoEventHandler(GetGrayValueInfo);
            BindProperty();
            this.文件类型comboBox.SelectedItem = ".";
        }
        public enum enShowItems
        {
            输入对象,
        }
        private void BindProperty()
        {
            try
            {
                this.窗口名称comboBox.DataSource = HWindowManage.GetKeysList();
                this.显示条目comboBox.DataSource = Enum.GetNames(typeof(enShowItems));
                this.保存方式comboBox.DataSource = Enum.GetNames(typeof(enSaveMethod));
                // 绑定机台坐标点 ，绑定类型只能为C#使用的数据类型
                this.文件目录textBox.DataBindings.Add("Text", ((SaveImage)this._function).SaveParam, "FolderPath", true, DataSourceUpdateMode.OnPropertyChanged);
                this.NG存储目录textBox.DataBindings.Add("Text", ((SaveImage)this._function).SaveParam, "FolderPathNg", true, DataSourceUpdateMode.OnPropertyChanged);
                this.文件类型comboBox.DataBindings.Add("Text", ((SaveImage)this._function).SaveParam, "ExtendName", true, DataSourceUpdateMode.OnPropertyChanged);
                this.日期checkBox.DataBindings.Add("Checked", ((SaveImage)this._function).SaveParam, "AddDataTime", true, DataSourceUpdateMode.OnPropertyChanged);
                this.文件名称TextBox.DataBindings.Add("Text", ((SaveImage)this._function).SaveParam, "FileName", true, DataSourceUpdateMode.OnPropertyChanged);
                this.保存方式comboBox.DataBindings.Add("Text", ((SaveImage)this._function).SaveParam, "SaveMethod", true, DataSourceUpdateMode.OnPropertyChanged);
                this.保存窗口图像checkBox.DataBindings.Add("Checked", ((SaveImage)this._function).SaveParam, "IsSaveWindowImage", true, DataSourceUpdateMode.OnPropertyChanged);
                this.保存原图checkBox.DataBindings.Add("Checked", ((SaveImage)this._function).SaveParam, "IsSaveSourceImage", true, DataSourceUpdateMode.OnPropertyChanged);
                this.保存区域图像checkBox.DataBindings.Add("Checked", ((SaveImage)this._function).SaveParam, "IsSaveRegionImage", true, DataSourceUpdateMode.OnPropertyChanged);
                this.按产品ID存图checkBox.DataBindings.Add("Checked", ((SaveImage)this._function).SaveParam, "EnableProductID", true, DataSourceUpdateMode.OnPropertyChanged);
                this.缩减图像域checkBox.DataBindings.Add("Checked", ((SaveImage)this._function).SaveParam, "IsReducedomain", true, DataSourceUpdateMode.OnPropertyChanged);
                ////////////////////////////////////////
                this.窗口名称comboBox.DataBindings.Add(nameof(this.窗口名称comboBox.SelectedItem), ((SaveImage)this._function).SaveParam, "WindowName", true, DataSourceUpdateMode.OnPropertyChanged);
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
            }
        }

        private void listbox_AddItems(object send, ItemsChangeEventArgs e)
        {
            try
            {
                object object3D = ((SaveImage)this._function).ImageData;
                ///////////////////////////////////////////
                if (object3D != null)
                {
                    switch (object3D.GetType().Name)
                    {
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
                        case "ImageDataClass":
                            this.drawObject.BackImage = ((SaveImage)this._function).ImageData;
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
                    this.toolStripButton_Translate.CheckState = CheckState.Unchecked;

                    break;
                default:
                    break;
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
        private void directoryButton_Click(object sender, EventArgs e)
        {
            try
            {
                FolderBrowserDialog fold = new FolderBrowserDialog();
                fold.ShowDialog();
                this.文件目录textBox.Text = fold.SelectedPath;
            }
            catch
            {
                MessageBox.Show(new Exception().ToString());
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
                this.drawObject.GrayValueInfo -= new GrayValueInfoEventHandler(GetGrayValueInfo);
            }
            catch
            {

            }
        }

        private void 保存窗口图像checkBox_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.保存窗口图像checkBox.Checked)
                    this.窗口名称comboBox.Enabled = true;
                else
                    this.窗口名称comboBox.Enabled = false;
            }
            catch
            {

            }

        }

        private void NgButton_Click(object sender, EventArgs e)
        {
            try
            {
                FolderBrowserDialog fold = new FolderBrowserDialog();
                fold.ShowDialog();
                this.文件目录textBox.Text = fold.SelectedPath;
            }
            catch
            {
                MessageBox.Show(new Exception().ToString());
            }
        }
    }
}

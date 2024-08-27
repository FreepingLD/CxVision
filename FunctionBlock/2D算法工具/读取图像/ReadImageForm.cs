
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
using System.Threading.Tasks;
using System.Windows.Forms;
using View;

namespace FunctionBlock
{
    public partial class ReadImageForm : Form
    {
        private object _objectDataModel;
        private Form form;
        private IFunction _function;
        private VisualizeView drawObject;
        private BindingSource bs = new BindingSource();
        public ReadImageForm(IFunction function,TreeNode node)
        {
            this._function = function;
            InitializeComponent();
            drawObject = new VisualizeView(this.hWindowControl1, true);
        }
        private void ReadImageForm_Load(object sender, EventArgs e)
        {
            // 注册事件
            this.drawObject.GrayValueInfo += new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
            //SensorBase.ImageAcqComplete += new ImageAcqCompleteEventHandler(this.ImageAcqComplete_Event);
            BindProperty();
            this.文件类型comboBox.SelectedItem = ".";
        }
        private void ImageAcqComplete_Event(ImageAcqCompleteEventArgs e)
        {
            try
            {
                // 当相机为空，或相机名称不相等时，返回
                if (this._function.Name != e.CamName) return;
                double x, y, z;
                if (MotionControlCard.MotionCardManage.CurrentCard != null)
                {
                    //////////////////////
                    this.drawObject.BackImage = e.ImageData;
                    _objectDataModel = e.ImageData;
                }
                else
                {
                    this.drawObject.BackImage = e.ImageData;
                    _objectDataModel = e.ImageData;
                }
            }
            catch
            {
                MessageBox.Show("图像刷新失败");
            }

        }
        private void BindProperty()
        {
            try
            {
                foreach (var item in ((ReadImage)this._function).ReadParam.FilePath)
                    this.listBox1.Items.Add(item);
                this.bs.DataSource = ((ReadImage)this._function).ReadParam.FilePath;
                this.bindingNavigator1.BindingSource = this.bs;
                this.bs.DataSourceChanged += new EventHandler(this.bindingNavigatorDataSourceChanged);
                ////////////////
                //this.文件总数textBox.DataBindings.Add("Text", (ReadImage)this._function, "FileCount", true, DataSourceUpdateMode.OnPropertyChanged);
                this.单文件路径textBox.DataBindings.Add("Text", ((ReadImage)this._function).ReadParam, "SingleFilePath", true, DataSourceUpdateMode.OnPropertyChanged);
                this.多文件目录textBox.DataBindings.Add("Text", ((ReadImage)this._function).ReadParam, "FolderPath", true, DataSourceUpdateMode.OnPropertyChanged);
                ////////////////////
                this.拍照点textBox.Text = "X:" + ((ReadImage)this._function).ReadParam.GrabPoint.X.ToString("f3") + "   Y:" +
                ((ReadImage)this._function).ReadParam.GrabPoint.Y.ToString("f3") + "   Theta:" + ((ReadImage)this._function).ReadParam.GrabPoint.Angle.ToString("f3");
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

        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            try
            {
                switch (e.ClickedItem.Text)
                {
                    case "执行":
                        /////
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

        // 更新3D对象模型 ；响应测量完成/及响应鼠标点击事件
        private void initEvent(HWindowControl hWindowControl)
        {
            this.hWindowControl1 = hWindowControl;
            this.hWindowControl1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.hWindowControl1_MouseMove);
        }
        // 获取鼠标位置处的高度值

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
        private void hWindowControl1_MouseMove(object sender, MouseEventArgs e)
        {
            HTuple value = -1.0;
            AlgorithmsLibrary.HalconLibrary ha = new AlgorithmsLibrary.HalconLibrary();
            if (this._objectDataModel == null) return;
            try
            {
                switch (this._objectDataModel.GetType().Name)
                {
                    case "HImage": //HObject
                        ha.GetGrayValueOnWindow(this.hWindowControl1.HalconWindow, (HImage)this._objectDataModel, e.Y, e.X, out value);
                        break;

                    case "ImageDataClass": //HObject
                        ha.GetGrayValueOnWindow(this.hWindowControl1.HalconWindow, ((ImageDataClass)this._objectDataModel).Image, e.Y, e.X, out value);
                        break;
                }
                this.灰度值1Label.Text = value[2].D.ToString();
                this.灰度值2Label.Text = 0.ToString();
                this.灰度值3Label.Text = 0.ToString();
                this.行坐标Label.Text = value[0].D.ToString();
                this.列坐标Label.Text = value[1].D.ToString();
            }
            catch
            {

            }
        }
        private void DeformableMatchForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                this.drawObject.ClearDrawingObject();
                this.drawObject.GrayValueInfo -= new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                //SensorBase.ImageAcqComplete -= new ImageAcqCompleteEventHandler(this.ImageAcqComplete_Event);
                this.bs.DataSourceChanged -= new EventHandler(this.bindingNavigatorDataSourceChanged);
            }
            catch
            {

            }

        }

        private void readDirectoryButton_Click(object sender, EventArgs e)
        {
            string[] path = null;
            try
            {
                FolderBrowserDialog fold = new FolderBrowserDialog();
                fold.ShowDialog();
                this.多文件目录textBox.Text = fold.SelectedPath;
                if (this.多文件目录textBox.Text.Trim().Length > 0)
                {
                    ((ReadImage)_function).ReadParam.FolderPath = this.多文件目录textBox.Text;
                    ((ReadImage)_function).ReadParam.SingleFilePath = "";
                    this.单文件路径textBox.Text = "";
                    if (fold.SelectedPath.Trim().Length > 0 && this.文件类型comboBox.SelectedItem.ToString().Length > 0)
                    {
                        path = Directory.GetFiles(fold.SelectedPath, this.文件类型comboBox.SelectedItem.ToString());
                        if (path != null)
                        {
                            this.listBox1.Items.Clear();
                            ((ReadImage)_function).ReadParam.FilePath.Clear();
                            foreach (var item in path)
                            {
                                ((ReadImage)_function).ReadParam.FilePath.Add(item);
                                this.listBox1.Items.Add(item);
                            }
                            this.bs.DataSource = null;
                            this.bs.DataSource = ((ReadImage)this._function).ReadParam.FilePath;
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
            string[] path = null;
            try
            {
                /////////////
                if (this.多文件目录textBox.Text.Trim().Length > 0 && this.文件类型comboBox.SelectedItem.ToString().Length > 0)
                {
                    path = Directory.GetFiles(this.多文件目录textBox.Text, this.文件类型comboBox.SelectedItem.ToString());
                    if (path != null)
                    {
                        this.listBox1.Items.Clear();
                        ((ReadImage)_function).ReadParam.FilePath.Clear();
                        foreach (var item in path)
                        {
                            ((ReadImage)_function).ReadParam.FilePath.Add(item);
                            this.listBox1.Items.Add(item);
                        }
                        this.bs.DataSource = null;
                        this.bs.DataSource = ((ReadImage)this._function).ReadParam.FilePath;
                    }
                }
            }
            catch
            {
                MessageBox.Show(new Exception().ToString());
            }
        }

        private void readFileButton_Click(object sender, EventArgs e)
        {
            FileOperate fo = new FileOperate();
            string path = fo.OpenImage();
            try
            {
                if (path != null && path.Trim().Length > 0)
                {
                    this.listBox1.Items.Clear();
                    ((ReadImage)_function).ReadParam.FilePath.Clear();
                    ((ReadImage)_function).ReadParam.FilePath.Add(path);
                    this.listBox1.Items.Add(path);
                    ((ReadImage)_function).ReadParam.SingleFilePath = path;
                    this.单文件路径textBox.Text = path;
                    ((ReadImage)_function).ReadParam.FolderPath = "";
                    this.多文件目录textBox.Text = "";
                    /////
                    this.bs.DataSource = null;
                    this.bs.DataSource = ((ReadImage)this._function).ReadParam.FilePath;
                }
            }
            catch
            {
                // MessageBox.Show(new Exception().ToString());
            }
        }
        private void bindingNavigatorDataSourceChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.bs.Current != null)
                    this.drawObject.BackImage = ((ReadImage)this._function).ReadParam.ReadImage(this.bs.Current.ToString());
            }
            catch
            {

            }
        }
        private void bindingNavigatorMovePreviousItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.bs.Current != null)
                    this.drawObject.BackImage = ((ReadImage)this._function).ReadParam.ReadImage(this.bs.Current.ToString());
            }
            catch
            {

            }
        }

        private void bindingNavigatorMoveFirstItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.bs.Current != null)
                    this.drawObject.BackImage = ((ReadImage)this._function).ReadParam.ReadImage(this.bs.Current.ToString());
            }
            catch
            {

            }
        }

        private void bindingNavigatorMoveNextItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.bs.Current != null)
                    this.drawObject.BackImage = ((ReadImage)this._function).ReadParam.ReadImage(this.bs.Current.ToString());
            }
            catch
            {

            }
        }

        private void bindingNavigatorMoveLastItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.bs.Current != null)
                    this.drawObject.BackImage = ((ReadImage)this._function).ReadParam.ReadImage(this.bs.Current.ToString());
            }
            catch
            {

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
                ((ReadImage)this._function).ReadParam.GrabPoint = new userWcsVector(X, Y, 0, Theta);
            }
            else
                MessageBox.Show("数据转换报错");
        }
    }
}

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
    public partial class ImageAcqAppForm : Form
    {
        private CancellationTokenSource cts;
        private Form form;
        private IFunction _function;
        private VisualizeView drawObject;
        private TreeNode _refNode;
        private AcqSource acqSource;
        private BindingSource bs = new BindingSource();
        private HWindowControl hWindowControl1;
        private BindingNavigator bindingNavigator1;
        private ToolStripStatusLabel executeStatusLabel;
        public VisualizeView DrawObject { get => drawObject; set => drawObject = value; }

        public ImageAcqAppForm(TreeNode node)
        {
            this._refNode = node;
            this._function = this._refNode.Tag as IFunction;
            this.bindingNavigator1 = new BindingNavigator();
            InitializeComponent();
            drawObject = new VisualizeView(this.hWindowControl1, false);
        }
        public ImageAcqAppForm(TreeNode node, HWindowControl hWindowControl)
        {
            InitializeComponent();
            this._refNode = node;
            this._function = node.Tag as IFunction;
            this.hWindowControl1 = hWindowControl;
            this.bindingNavigator1 = new BindingNavigator();
            this.drawObject = new VisualizeView(this.hWindowControl1, false);
        }
        private void ImageAcqSimpleForm_Load(object sender, EventArgs e)
        {
            // 注册事件
            BaseFunction.ExcuteCompleted += new ExcuteCompletedEventHandler(DisplayExcuteResult);
            this.drawObject.GrayValueInfo += new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
            BindProperty();
            this.AutoForm();
        }
        private void AutoForm()
        {
            try
            {
                if (this.Tag != null)
                {
                    string[] value = this.Tag.ToString().Split(',', ';', ':');
                    double width = 0, height = 0;
                    if (value.Length > 0)
                        double.TryParse(value[0], out width);
                    if (value.Length > 1)
                        double.TryParse(value[1], out height);
                    if (width > 0 && height > 0)
                    {
                        double scsleWidth = (Screen.PrimaryScreen.WorkingArea.Width * 1.0) / width;
                        double scsleHeight = (Screen.PrimaryScreen.WorkingArea.Height * 1.0) / height;
                        this.Width = (int)(this.Width * scsleWidth);
                        this.Height = (int)(this.Height * scsleHeight);
                    }
                }
            }
            catch
            { }
        }
        private void BindProperty()
        {
            try
            {
                this.bs.DataSource = ((ImageAcq)this._function).AcqParam.FilePath;
                this.bindingNavigator1.BindingSource = this.bs;
                this.bs.DataSourceChanged += new EventHandler(this.bindingNavigatorDataSourceChanged);
                //////////////////
                this.采集源comboBox.DataSource = AcqSourceManage.Instance.GetAcqSourceName();
                this.多文件目录textBox.DataBindings.Add("Text", ((ImageAcq)this._function).AcqParam, "FolderPath", true, DataSourceUpdateMode.OnPropertyChanged);
                this.单文件路径textBox.DataBindings.Add("Text", ((ImageAcq)this._function).AcqParam, "SingleFilePath", true, DataSourceUpdateMode.OnPropertyChanged);
                this.相机源radioButton.DataBindings.Add("Checked", ImageAcqDevice.Instance, "IsCamSource", true, DataSourceUpdateMode.OnPropertyChanged);
                this.文件源radioButton.DataBindings.Add("Checked", ImageAcqDevice.Instance, "IsFileSource", true, DataSourceUpdateMode.OnPropertyChanged);
                this.目录源radioButton.DataBindings.Add("Checked", ImageAcqDevice.Instance, "IsDirectorySource", true, DataSourceUpdateMode.OnPropertyChanged);
                this.拍照点textBox.Text = "X:" + ((ImageAcq)this._function).AcqParam.GrabPoint.X.ToString("f5") + "   Y:" +
                         ((ImageAcq)this._function).AcqParam.GrabPoint.Y.ToString("f5") + "   Theta:" + ((ImageAcq)this._function).AcqParam.GrabPoint.Angle.ToString("f5");
            }
            catch (Exception ex)
            {
                new Common.UserMessageForm(ex.ToString()).ShowDialog();
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


        public void DisplayExcuteResult(object sender, ExcuteCompletedEventArgs e)  // 这里一定要加一个键值对变量来收集，以免出现重合
        {
            try
            {
                if (e.DataContent == null) return;// 在图形窗口只显示世界坐标元素，像素元素在图像窗口显示
                switch (e.DataContent.GetType().Name) //这里只接受XLD轮廓或3D对象轮廓
                {
                    case "ImageDataClass":
                        this.drawObject.BackImage = (ImageDataClass)e.DataContent;
                        break;
                }
            }
            catch (Exception ex)
            {
                new Common.UserMessageForm(ex.ToString()).ShowDialog(); ;
            }
        }

        private void ImageAcqForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (this.drawObject != null)
                    this.drawObject.ClearDrawingObject();
                ImageAcqDevice.Instance.Save(); // 保存全局参数
                this.acqSource = AcqSourceManage.Instance.GetCamAcqSource(this.采集源comboBox.SelectedItem.ToString());
                if (this.acqSource != null)
                {
                    this.acqSource?.Sensor?.CameraParam?.Save();
                }
                this.bs.DataSourceChanged -= new EventHandler(this.bindingNavigatorDataSourceChanged);
                BaseFunction.ExcuteCompleted -= new ExcuteCompletedEventHandler(DisplayExcuteResult);
                this.drawObject.GrayValueInfo -= new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                //this.drawObject.Uninit();
            }
            catch
            {

            }
        }

        private void 相机采集源comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (采集源comboBox.SelectedIndex == -1) return;
                this.acqSource = AcqSourceManage.Instance.GetCamAcqSource(((ImageAcq)this._function)?.AcqSourceName);
                //this.acqSource = ((ImageAcq)this._function).AcqSourceName;
                if (this.acqSource == null)  // return;
                    this.acqSource = new AcqSource();
                this.采集源comboBox.Text = this.acqSource.Name;
                if (this.acqSource.Sensor == null) return;
                switch (this.acqSource.Sensor.ConfigParam.SensorType)
                {
                    case enUserSensorType.点激光:
                    case enUserSensorType.线激光:
                    case enUserSensorType.面激光:
                        AddForm(this.panel1, new LaserParamForm(this.acqSource.Sensor.LaserParam));
                        break;
                    case enUserSensorType.面阵相机:
                    case enUserSensorType.线阵相机:
                        AddForm(this.panel1, new CameraParamForm(this.acqSource.Sensor.CameraParam, ((ImageAcq)this._function).AcqParam));
                        break;
                    default:
                        AddForm(this.panel1, new CameraParamForm(null));
                        break;
                }
            }
            catch (Exception ex)
            {
                new Common.UserMessageForm(ex.ToString()).ShowDialog(); ;
            }
        }

        private void 采集源comboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            try
            {
                if (采集源comboBox.SelectedIndex == -1) return;
                if (采集源comboBox.SelectedItem == null) return;
                ((ImageAcq)this._function).AcqSourceName = this.采集源comboBox.SelectedItem.ToString();
                //this.acqSource = AcqSourceManage.Instance.GetCamAcqSource(this.采集源comboBox.SelectedItem.ToString()); // 这个事件发生后才会触发 SelectedIndexChanged 事件 
                //if (this.acqSource != null)
                //    ((ImageAcq)this._function).AcqSourceName = this.acqSource.Name;
            }
            catch (Exception ex)
            {
                new Common.UserMessageForm(ex.ToString()).ShowDialog(); ;
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
                    ((ImageAcq)_function).AcqParam.FilePath.Clear();
                    ((ImageAcq)_function).AcqParam.FilePath.Clear();
                    //((ImageAcq)_function).AcqParam.FilePath.Add(path);
                    ((ImageAcq)_function).AcqParam.SingleFilePath = path;
                    this.单文件路径textBox.Text = path;
                    //((ImageAcq)_function).AcqParam.FolderPath = "";
                    ((ImageAcq)_function).ImageData = ((ImageAcq)_function).AcqParam.ReadImage(path, ((ImageAcq)_function).AcqSourceName);
                    /////
                    this.bs.DataSource = null;
                    this.bs.DataSource = ((ImageAcq)this._function).AcqParam.FilePath;
                }
            }
            catch
            {
                new Common.UserMessageForm(new Exception().ToString()).ShowDialog();
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
                ((ImageAcq)this._function).AcqParam.GrabPoint = new userWcsVector(X, Y, 0, Theta);
            }
            else
                new UserMessageForm().ShowDialog("数据转换报错");
        }

        private void 相机源radioButton_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.相机源radioButton.Checked)
                {
                    this.采集源comboBox.Enabled = true;
                    this.单文件路径textBox.Enabled = false;
                    this.readFileButton.Enabled = false;
                    this.拍照点textBox.Enabled = false;
                }
            }
            catch
            {
            }
        }

        private void 文件源radioButton_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.文件源radioButton.Checked)
                {
                    if (SystemParamManager.Instance.SysConfigParam.IsAutoRun)
                    {
                        new UserMessageForm().ShowDialog("请先断开联机运行!!!");
                        this.文件源radioButton.Checked = false;
                        this.相机源radioButton.Checked = true;
                    }
                    else
                    {
                        this.采集源comboBox.Enabled = false;
                        this.单文件路径textBox.Enabled = true;
                        this.readFileButton.Enabled = true;
                        this.拍照点textBox.Enabled = true;
                    }
                }
            }
            catch (Exception ex)
            {
                new Common.UserMessageForm(ex.ToString()).ShowDialog(); ;
            }
        }

        private void readDirectoryButton_Click(object sender, EventArgs e)
        {
            try
            {
                string[] path = null;
                FolderBrowserDialog fold = new FolderBrowserDialog();
                fold.ShowDialog();
                this.多文件目录textBox.Text = fold.SelectedPath;
                if (this.多文件目录textBox.Text.Trim().Length > 0)
                {
                    ((ImageAcq)_function).AcqParam.FolderPath = this.多文件目录textBox.Text;
                    //((ImageAcq)_function).AcqParam.SingleFilePath = "";
                    //this.单文件路径textBox.Text = "";
                    if (fold.SelectedPath.Trim().Length > 0 && this.文件类型comboBox.SelectedItem.ToString().Length > 0)
                    {
                        path = Directory.GetFiles(fold.SelectedPath, this.文件类型comboBox.SelectedItem.ToString());
                        if (path != null)
                        {
                            ((ImageAcq)_function).AcqParam.FilePath.Clear();
                            foreach (var item in path)
                            {
                                ((ImageAcq)_function).AcqParam.FilePath.Add(item);
                            }
                            this.bs.DataSource = null;
                            this.bs.DataSource = ((ImageAcq)this._function).AcqParam.FilePath;
                        }
                    }

                }
            }
            catch
            {
                new Common.UserMessageForm(new Exception().ToString()).ShowDialog();
            }

        }
        private void bindingNavigatorDataSourceChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.bs.Current != null)
                {
                    ((ImageAcq)this._function).FileIndex = 0;
                    this.drawObject.BackImage = ((ImageAcq)this._function).AcqParam.ReadImage(this.bs.Current.ToString(), this.采集源comboBox.Text);
                }

            }
            catch (Exception ex)
            {
                new Common.UserMessageForm(ex.ToString()).ShowDialog(); ;
            }
        }
        private void bindingNavigatorMoveNextItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.bs.Current != null)
                {
                    ((ImageAcq)this._function).FileIndex = this.bs.Position;
                    this.drawObject.BackImage = ((ImageAcq)this._function).AcqParam.ReadImage(this.bs.Current.ToString(), this.采集源comboBox.Text);
                }

            }
            catch (Exception ex)
            {
                new Common.UserMessageForm(ex.ToString()).ShowDialog(); ;
            }
        }

        private void bindingNavigatorMoveLastItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.bs.Current != null)
                {
                    ((ImageAcq)this._function).FileIndex = this.bs.Position;
                    this.drawObject.BackImage = ((ImageAcq)this._function).AcqParam.ReadImage(this.bs.Current.ToString(), this.采集源comboBox.Text);
                }
            }
            catch (Exception ex)
            {
                new Common.UserMessageForm(ex.ToString()).ShowDialog(); ;
            }
        }

        private void bindingNavigatorMovePreviousItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.bs.Current != null)
                {
                    ((ImageAcq)this._function).FileIndex = this.bs.Position;
                    this.drawObject.BackImage = ((ImageAcq)this._function).AcqParam.ReadImage(this.bs.Current.ToString(), this.采集源comboBox.Text);
                }

            }
            catch (Exception ex)
            {
                new Common.UserMessageForm(ex.ToString()).ShowDialog(); ;
            }
        }

        private void bindingNavigatorMoveFirstItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.bs.Current != null)
                {
                    ((ImageAcq)this._function).FileIndex = this.bs.Position;
                    this.drawObject.BackImage = ((ImageAcq)this._function).AcqParam.ReadImage(this.bs.Current.ToString(), this.采集源comboBox.Text);
                }
            }
            catch (Exception ex)
            {
                new Common.UserMessageForm(ex.ToString()).ShowDialog(); ;
            }
        }

        private void 文件类型comboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            try
            {
                /////////////
                string[] path = null;
                if (this.多文件目录textBox.Text.Trim().Length > 0 && this.文件类型comboBox.SelectedItem.ToString().Length > 0)
                {
                    path = Directory.GetFiles(this.多文件目录textBox.Text, this.文件类型comboBox.SelectedItem.ToString());
                    if (path != null)
                    {
                        ((ImageAcq)_function).AcqParam.FilePath.Clear();
                        foreach (var item in path)
                        {
                            ((ImageAcq)_function).AcqParam.FilePath.Add(item);
                        }
                        this.bs.DataSource = null;
                        this.bs.DataSource = ((ImageAcq)this._function).AcqParam.FilePath;
                    }
                }
            }
            catch (Exception ex)
            {
                new Common.UserMessageForm(ex.ToString()).ShowDialog(); ;
            }
        }

        private void 多文件目录textBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                /////////////
                string[] path = null;
                if (this.多文件目录textBox.Text.Trim().Length > 0 && this.文件类型comboBox.SelectedItem.ToString().Length > 0)
                {
                    path = Directory.GetFiles(this.多文件目录textBox.Text, this.文件类型comboBox.SelectedItem.ToString());
                    if (path != null)
                    {
                        ((ImageAcq)_function).AcqParam.FilePath.Clear();
                        foreach (var item in path)
                        {
                            ((ImageAcq)_function).AcqParam.FilePath.Add(item);
                        }
                        this.bs.DataSource = null;
                        this.bs.DataSource = ((ImageAcq)this._function).AcqParam.FilePath;
                    }
                }
            }
            catch (Exception ex)
            {
                //new UserMessageForm().ShowDialog(ex.ToString());
            }
        }



    }
}


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
    public partial class ReadFileForm : Form
    {
        private object _objectDataModel;
        private Form form;
        private IFunction _function;
        private VisualizeView drawObject;
        private BindingSource bs = new BindingSource();
        public ReadFileForm(TreeNode node)
        {
            this._function = node.Tag as IFunction;
            InitializeComponent();
            drawObject = new VisualizeView(this.hWindowControl1, true);
            this.Text = node.Text;
        }
        private void ReadFileForm_Load(object sender, EventArgs e)
        {
            // 注册事件
            //this.drawObject.GrayValueInfo += new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
            BaseFunction.ExcuteCompleted += new ExcuteCompletedEventHandler(this.DisplayObjectModel);
            BindProperty();
            this.文件类型comboBox.SelectedItem = ".";
        }

        private void BindProperty()
        {
            try
            {
                foreach (var item in ((ReadFile)this._function).ReadParam.FilePath)
                    this.listBox1.Items.Add(item);
                this.bs.DataSource = ((ReadFile)this._function).ReadParam.FilePath;
                this.bindingNavigator1.BindingSource = this.bs;
                this.bs.DataSourceChanged += new EventHandler(this.bindingNavigatorDataSourceChanged);
                ////////////////
                //this.文件总数textBox.DataBindings.Add("Text", (ReadFile)this._function, "FileCount", true, DataSourceUpdateMode.OnPropertyChanged);
                this.单文件路径textBox.DataBindings.Add("Text", ((ReadFile)this._function).ReadParam, "SingleFilePath", true, DataSourceUpdateMode.OnPropertyChanged);
                this.多文件目录textBox.DataBindings.Add("Text", ((ReadFile)this._function).ReadParam, "FolderPath", true, DataSourceUpdateMode.OnPropertyChanged);
                ////////////////////
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

        private void ReadFileForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                this.drawObject.ClearDrawingObject();
                BaseFunction.ExcuteCompleted -= new ExcuteCompletedEventHandler(this.DisplayObjectModel);
                this.bs.DataSourceChanged -= new EventHandler(this.bindingNavigatorDataSourceChanged);
            }
            catch
            {

            }

        }
        public void DisplayObjectModel(object sender, ExcuteCompletedEventArgs e)  // 这里一定要加一个键值对变量来收集，以免出现重合
        {
            try
            {
                if (e.DataContent != null) // 在图形窗口只显示世界坐标元素，像素元素在图像窗口显示
                {
                    switch (e.DataContent.GetType().Name) //这里只接受XLD轮廓或3D对象轮廓
                    {
                        case "CoordPoint[]":
                            this.drawObject.AttachPropertyData.Clear();
                            foreach (var item in (CoordPoint[])e.DataContent)
                            {
                                for (int i = 0; i < item.Count; i++)
                                {
                                    this.drawObject.AttachPropertyData.Add(new HXLDCont(item.Y[i], item.X[i]));
                                }                    
                            }
                            this.drawObject.UpdataGraphicView(); // 背影不刷新
                            break;
                    }
                }
            }
            catch (Exception he)
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
                    ((ReadFile)_function).ReadParam.FolderPath = this.多文件目录textBox.Text;
                    ((ReadFile)_function).ReadParam.SingleFilePath = "";
                    this.单文件路径textBox.Text = "";
                    if (fold.SelectedPath.Trim().Length > 0 && this.文件类型comboBox.SelectedItem.ToString().Length > 0)
                    {
                        path = Directory.GetFiles(fold.SelectedPath, this.文件类型comboBox.SelectedItem.ToString());
                        if (path != null)
                        {
                            this.listBox1.Items.Clear();
                            ((ReadFile)_function).ReadParam.FilePath.Clear();
                            foreach (var item in path)
                            {
                                ((ReadFile)_function).ReadParam.FilePath.Add(item);
                                this.listBox1.Items.Add(item);
                            }
                            this.bs.DataSource = null;
                            this.bs.DataSource = ((ReadFile)this._function).ReadParam.FilePath;
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
                        ((ReadFile)_function).ReadParam.FilePath.Clear();
                        foreach (var item in path)
                        {
                            ((ReadFile)_function).ReadParam.FilePath.Add(item);
                            this.listBox1.Items.Add(item);
                        }
                        this.bs.DataSource = null;
                        this.bs.DataSource = ((ReadFile)this._function).ReadParam.FilePath;
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
                    ((ReadFile)_function).ReadParam.FilePath.Clear();
                    ((ReadFile)_function).ReadParam.FilePath.Add(path);
                    this.listBox1.Items.Add(path);
                    ((ReadFile)_function).ReadParam.SingleFilePath = path;
                    this.单文件路径textBox.Text = path;
                    ((ReadFile)_function).ReadParam.FolderPath = "";
                    this.多文件目录textBox.Text = "";
                    /////
                    this.bs.DataSource = null;
                    this.bs.DataSource = ((ReadFile)this._function).ReadParam.FilePath;
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
                if (this.bs.Current != null) ;
                    //this.drawObject.BackImage = ((ReadFile)this._function).ReadParam.ReadImage(this.bs.Current.ToString());
            }
            catch
            {

            }
        }
        private void bindingNavigatorMovePreviousItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.bs.Current != null) ;
                   // this.drawObject.BackImage = ((ReadFile)this._function).ReadParam.ReadFile(this.bs.Current.ToString());
            }
            catch
            {

            }
        }

        private void bindingNavigatorMoveFirstItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.bs.Current != null) ;
                   // this.drawObject.BackImage = ((ReadFile)this._function).ReadParam.ReadImage(this.bs.Current.ToString());
            }
            catch
            {

            }
        }

        private void bindingNavigatorMoveNextItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.bs.Current != null) ;
                    //this.drawObject.BackImage = ((ReadImage)this._function).ReadParam.ReadImage(this.bs.Current.ToString());
            }
            catch
            {

            }
        }

        private void bindingNavigatorMoveLastItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.bs.Current != null) ;
                    //this.drawObject.BackImage = ((ReadImage)this._function).ReadParam.ReadImage(this.bs.Current.ToString());
            }
            catch
            {

            }
        }

    }
}

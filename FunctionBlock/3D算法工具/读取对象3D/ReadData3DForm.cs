
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
    public partial class ReadData3DForm : Form
    {

        private object _objectDataModel;
        private Form form;
        private IFunction _function;
        private VisualizeView drawObject;
        private BindingSource bs = new BindingSource();
        private ReadObjectModelParam readParam;
        public ReadData3DForm(IFunction function)
        {      
            this._function = function;
            InitializeComponent();
            this.drawObject = new VisualizeView(this.hWindowControl1, true);
        }
        private void AppBaseForm_Load(object sender, EventArgs e)
        {
            // 注册事件
            BaseFunction.ExcuteCompleted += new ExcuteCompletedEventHandler(this.DisplayObjectModel);
            ListBoxWrapClass.ItemsChangeToForm += new ItemsChangeEventHandler(listbox_AddItems);
            BindProperty();
            //////////////////
        }
        private void BindProperty()
        {
            try
            {
                this.readParam = ((ReadObjectModel3D)this._function).ReadParam;
                foreach (var item in this.readParam.FilePath)
                    this.listBox1.Items.Add(item);
                this.bs.DataSource = this.readParam.FilePath;
                this.bindingNavigator1.BindingSource = this.bs;
                this.bs.DataSourceChanged += new EventHandler(this.bindingNavigatorDataSourceChanged);
                // 绑定机台坐标点 ，绑定类型只能为C#使用的数据类型,  只有在执行了绑定后，PropertyChanged事件才被订阅
                this.单文件路径textBox.DataBindings.Add("Text", this.readParam, "SingleFilePath", true, DataSourceUpdateMode.OnPropertyChanged);
                this.多文件目录textBox.DataBindings.Add("Text", this.readParam, "FolderPath", true, DataSourceUpdateMode.OnPropertyChanged);

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
                        //case "userWcsPoint[]":
                        //    this.drawObject.AttachPropertyData.Clear();
                        //    userWcsPoint[] wcsPoint = (userWcsPoint[])this._function.GetPropertyValues(this.显示条目comboBox.Text.Trim());
                        //    for (int i = 0; i < wcsPoint.Length; i++)
                        //    {
                        //        this.drawObject.AttachPropertyData.Add(wcsPoint[i]);
                        //    }
                        //    break;
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
        private void readFileButton_Click(object sender, EventArgs e)
        {
            FileOperate fo = new FileOperate();
            string path = fo.OpenFile();
            try
            {
                if (path != null && path.Trim().Length > 0)
                {
                    this.listBox1.Items.Clear();
                    this.readParam.FilePath.Clear();
                    this.readParam.FilePath.Add(path);
                    this.listBox1.Items.Add(path);
                    this.readParam.SingleFilePath = path;
                    this.单文件路径textBox.Text = path;
                    this.readParam.FolderPath = "";
                    this.多文件目录textBox.Text = "";
                    /////
                    this.bs.DataSource = null;
                    this.bs.DataSource = this.readParam.FilePath;
                }
            }
            catch
            {
                // MessageBox.Show(new Exception().ToString());
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
                    this.readParam.FolderPath = this.多文件目录textBox.Text;
                    this.readParam.SingleFilePath = "";
                    this.单文件路径textBox.Text = "";
                    if (this.文件类型comboBox.SelectedItem == null) return;
                    if (fold.SelectedPath.Trim().Length > 0 && this.文件类型comboBox.SelectedItem.ToString().Length > 0)
                    {
                        path = Directory.GetFiles(fold.SelectedPath, this.文件类型comboBox.SelectedItem.ToString());
                        if (path != null)
                        {
                            this.listBox1.Items.Clear();
                            this.readParam.FilePath.Clear();
                            foreach (var item in path)
                            {
                                this.readParam.FilePath.Add(item);
                                this.listBox1.Items.Add(item);
                            }
                            this.bs.DataSource = null;
                            this.bs.DataSource = this.readParam.FilePath;
                        }
                    }

                }
            }
            catch
            {
                MessageBox.Show(new Exception().ToString());
            }
        }
        private void 文件类型comboBox_SelectedIndexChanged(object sender, EventArgs e)
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
                        this.readParam.FilePath.Clear();
                        foreach (var item in path)
                        {
                            this.readParam.FilePath.Add(item);
                            this.listBox1.Items.Add(item);
                        }
                        this.bs.DataSource = null;
                        this.bs.DataSource = this.readParam.FilePath;
                    }
                }
            }
            catch
            {
                MessageBox.Show(new Exception().ToString());
            }
        }
        private void DeformableMatchForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // 注消事件
            try
            {
                if (this.drawObject != null)
                    this.drawObject.ClearDrawingObject();
                BaseFunction.ExcuteCompleted -= new ExcuteCompletedEventHandler(this.DisplayObjectModel);
                ListBoxWrapClass.ItemsChangeToForm -= new ItemsChangeEventHandler(listbox_AddItems);
                this.bs.DataSourceChanged -= new EventHandler(this.bindingNavigatorDataSourceChanged);
            }
            catch
            {

            }
        }

        private void bindingNavigatorDataSourceChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.bs.Current != null)
                {
                    int index = this.readParam.FilePath.IndexOf(this.bs.Current.ToString());
                    this._function.Execute(index);
                }
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
                {
                    int index = this.readParam.FilePath.IndexOf(this.bs.Current.ToString());
                    this._function.Execute(index);
                }
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
                {
                    int index = this.readParam.FilePath.IndexOf(this.bs.Current.ToString());
                    this._function.Execute(index);
                }
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
                {
                    int index = this.readParam.FilePath.IndexOf(this.bs.Current.ToString());
                    this._function.Execute(index);
                }
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
                {
                    int index = this.readParam.FilePath.IndexOf(this.bs.Current.ToString());
                    this._function.Execute(index);
                }
            }
            catch
            {

            }
        }
    }
}

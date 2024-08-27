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
    public partial class PlanenessForm : Form
    {
        private object _objectDataModel;
        private Form form;
        private IFunction _function;
        private VisualizeView drawObject;

        public PlanenessForm(IFunction function,TreeNode node)
        {      
            this._function = function;
            InitializeComponent();
            this.drawObject = new VisualizeView(this.hWindowControl1, true);
            new ListBoxWrapClass().InitListBox(this.listBox1, node);  
        }
        private void AppBaseForm_Load(object sender, EventArgs e)
        {
            // 注册事件
            this.drawObject.GrayValueInfo += new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
            BaseFunction.ExcuteCompleted += new ExcuteCompletedEventHandler(this.DisplayObjectModel);
            ListBoxWrapClass.ItemsChangeToForm += new ItemsChangeEventHandler(listbox_AddItems);
            BindProperty();
            this.drawObject.PointCloudModel3D = ((Planeness)this._function).DataObjectModel;
        }
        public enum enShowItems
        {
            输入3D对象,
        }
        private void BindProperty()
        {
            try
            {
                this.显示条目comboBox.DataSource = Enum.GetNames(typeof(enShowItems));
                this.dataGridView1.DataSource = ((Planeness)this._function).ResultDataTable;
                this.拟合算法textBox.DataBindings.Add("Text", ((Planeness)this._function).Param, "FittingAlgorithm", true, DataSourceUpdateMode.OnPropertyChanged);
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
            if (sender is FunctionBlock.Planeness)
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
                            case "userWcsPoint[]":
                                this.drawObject.AttachPropertyData.Clear();
                                userWcsPoint[] wcsPoint = (userWcsPoint[])this._function.GetPropertyValues(this.显示条目comboBox.Text.Trim());
                                for (int i = 0; i < wcsPoint.Length; i++)
                                {
                                    this.drawObject.AttachPropertyData.Add(wcsPoint[i]);
                                }
                                break;
                        }
                    }
                }
                catch (Exception he)
                {

                }
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
            this.行坐标Label.Text = "x: " + e.Row.ToString();
            this.列坐标Label.Text = "y: " + e.Col.ToString();
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
        private void dataGridView1_DataSourceChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < this.dataGridView1.Rows.Count; i++)
            {
                this.dataGridView1.Rows[i].HeaderCell.Value = i.ToString();
            }
        }

        private void DeformableMatchForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // 注消事件
            try
            {
                if (this.drawObject != null)
                    this.drawObject.ClearDrawingObject();
                this.drawObject.GrayValueInfo -= new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                BaseFunction.ExcuteCompleted -= new ExcuteCompletedEventHandler(this.DisplayObjectModel);
                ListBoxWrapClass.ItemsChangeToForm -= new ItemsChangeEventHandler(listbox_AddItems);
            }
            catch
            {

            }
        }

        private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false;
        }
    }
}

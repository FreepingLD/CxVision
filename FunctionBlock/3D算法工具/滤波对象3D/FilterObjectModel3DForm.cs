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
    public partial class FilterObjectModel3DForm : Form
    {
        private Form form;
        private IFunction _function;
        private VisualizeView drawObject;
        private ListBoxWrapClass ListBoxWrapClass1;
        public FilterObjectModel3DForm(IFunction function,TreeNode node)
        {
            this._function = function;
            InitializeComponent();
            this.drawObject = new VisualizeView(this.hWindowControl1, true);
            ListBoxWrapClass1 = new ListBoxWrapClass();
            ListBoxWrapClass1.InitListBox(this.listBox1, node);
        }
        private void AppBaseForm_Load(object sender, EventArgs e)
        {
            // 注册事件
            BaseFunction.ExcuteCompleted += new ExcuteCompletedEventHandler(this.DisplayObjectModel);
            ListBoxWrapClass.ItemsChangeToForm += new ItemsChangeEventHandler(listbox_AddItems);
            BindProperty();

        }
        private void BindProperty()
        {
            try
            {
                // 绑定机台坐标点 ，绑定类型只能为C#使用的数据类型
                this.显示条目comboBox.DataSource = Enum.GetNames(typeof(FunctionBlock.FilterObjectModel3D.enShowItems));
                this.连通方式comboBox.DataSource = Enum.GetNames(typeof(FunctionBlock.FilterObjectModel3D.enConnectFeatue));
                this.选择特征comboBox.DataSource = Enum.GetNames(typeof(FunctionBlock.FilterObjectModel3D.enSelecctFeatue));
                //////////////////
                this.连通方式comboBox.DataBindings.Add("Text", (FunctionBlock.FilterObjectModel3D)this._function, "ConnectFeature", true, DataSourceUpdateMode.OnPropertyChanged);
                this.连通值textBox.DataBindings.Add("Text", (FunctionBlock.FilterObjectModel3D)this._function, "ConnectValue", true, DataSourceUpdateMode.OnPropertyChanged);
                this.选择特征comboBox.DataBindings.Add("Text", (FunctionBlock.FilterObjectModel3D)this._function, "SelectFeature", true, DataSourceUpdateMode.OnPropertyChanged);
                this.最小特征值comboBox.DataBindings.Add("Text", (FunctionBlock.FilterObjectModel3D)this._function, "MinFeatureValue", true, DataSourceUpdateMode.OnPropertyChanged);
                this.最大特征值comboBox.DataBindings.Add("Text", (FunctionBlock.FilterObjectModel3D)this._function, "MaxFeatureValue", true, DataSourceUpdateMode.OnPropertyChanged);
                this.特征操作comboBox.DataBindings.Add("Text", (FunctionBlock.FilterObjectModel3D)this._function, "SelectOperation", true, DataSourceUpdateMode.OnPropertyChanged);
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
            }
        }
        private void DisplayObjectModel(object send, ExcuteCompletedEventArgs e)
        {
            if (send is FunctionBlock.FilterObjectModel3D)
            {
                try
                {
                    switch (e.DataContent.GetType().Name)
                    {
                        case "HObjectModel3D":
                            this.drawObject.PointCloudModel3D = new PointCloudData( (HObjectModel3D)e.DataContent );
                            break;
                        case "HObjectModel3D[]":
                            this.drawObject.PointCloudModel3D = new PointCloudData((HObjectModel3D[])e.DataContent);
                            break;
                    }
                }
                catch
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
                            this.drawObject.PointCloudModel3D = new PointCloudData( (HObjectModel3D)object3D );
                            break;
                        case "HObjectModel3D[]":
                            this.drawObject.PointCloudModel3D =new PointCloudData( (HObjectModel3D[])object3D);
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
            //try
            //{
            //    object objectModel = this._function.GetPropertyValues(this.显示条目comboBox.SelectedItem.ToString());
            //    if (objectModel != null)
            //    {
            //        switch (objectModel.GetType().Name)
            //        {
            //            case "HObjectModel3D":
            //                this.drawObject.PointCloudModel3D = new HObjectModel3D[] { (HObjectModel3D)objectModel };
            //                break;
            //            case "HObjectModel3D[]":
            //                this.drawObject.PointCloudModel3D = (HObjectModel3D[])objectModel;
            //                break;
            //        }
            //    }
            //}
            //catch
            //{

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




        private void DeformableMatchForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                // 注消事件
                if (drawObject != null)
                    this.drawObject.ClearDrawingObject();
                BaseFunction.ExcuteCompleted -= new ExcuteCompletedEventHandler(this.DisplayObjectModel);
                ListBoxWrapClass.ItemsChangeToForm -= new ItemsChangeEventHandler(listbox_AddItems);
            }
            catch
            {

            }
        }








    }
}

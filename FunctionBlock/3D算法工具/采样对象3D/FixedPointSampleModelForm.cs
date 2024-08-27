using AlgorithmsLibrary;
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
    public partial class FixedPointSampleModelForm : Form
    {
        private IFunction _function;
        private AutoReSizeFormControls arfc = new AutoReSizeFormControls();
        private VisualizeView drawObject;
        private ListBoxWrapClass ListBoxWrapClass1, ListBoxWrapClass2;
        public FixedPointSampleModelForm(IFunction function,TreeNode node)
        {
            this._function = function;
            InitializeComponent();
            ListBoxWrapClass1 = new ListBoxWrapClass();
            ListBoxWrapClass1.InitListBox(this.listBox1, node);
            ListBoxWrapClass2 = new ListBoxWrapClass();
            ListBoxWrapClass2.InitListBox(this.listBox2, node, 2);
            this.drawObject = new VisualizeView(this.hWindowControl1,true);
        }
        private void FixedPointSampleModelForm_Load(object sender, EventArgs e)
        {
            BindProperty();
            BaseFunction.ExcuteCompleted += new ExcuteCompletedEventHandler(DisplayObjectModel);
            ListBoxWrapClass.ItemsChangeToForm += new ItemsChangeEventHandler(listbox_AddItems);
        }
        private void BindProperty()
        {
            try
            {
                this.显示条目comboBox.DataSource = Enum.GetNames(typeof(FunctionBlock.FixedPointSampleModel.enShowItems));
                this.区域类型comboBox.DataSource = Enum.GetNames(typeof(FunctionBlock.FixedPointSampleModel.enRegionType));
                ////////////////
                this.角度textBox.DataBindings.Add("Text", (FunctionBlock.FixedPointSampleModel)this._function, "Deg", true, DataSourceUpdateMode.OnPropertyChanged);
                this.半长textBox.DataBindings.Add("Text", (FunctionBlock.FixedPointSampleModel)this._function, "Length1", true, DataSourceUpdateMode.OnPropertyChanged);
                this.半宽textBox.DataBindings.Add("Text", (FunctionBlock.FixedPointSampleModel)this._function, "Length2", true, DataSourceUpdateMode.OnPropertyChanged);
                this.半径textBox.DataBindings.Add("Text", (FunctionBlock.FixedPointSampleModel)this._function, "Radius", true, DataSourceUpdateMode.OnPropertyChanged);
                ///////////
                this.区域类型comboBox.DataBindings.Add("Text", (FunctionBlock.FixedPointSampleModel)this._function, "RegionType", true, DataSourceUpdateMode.OnPropertyChanged);
                this.截取方向comboBox.DataBindings.Add("Text", (FunctionBlock.FixedPointSampleModel)this._function, "InsideOrOutside", true, DataSourceUpdateMode.OnPropertyChanged);
                //////////////
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
        private void DisplayObjectModel(object sender, ExcuteCompletedEventArgs e)  // 这里一定要加一个键值对变量来收集，以免出现重合
        {
            try
            {
                if (e.DataContent != null)
                {
                    switch (e.DataContent.GetType().Name) //这里只接受XLD轮廓或3D对象轮廓
                    {
                        case "HImage":
                            this.drawObject.BackImage = new ImageDataClass((HImage)e.DataContent);
                            break;
                        case "ImageDataClass":
                            this.drawObject.BackImage = (ImageDataClass)e.DataContent;
                            break;
                        case "XldDataClass":
                            this.drawObject.XldContourData = (XldDataClass)e.DataContent;
                            break;
                        case "XldDataClass[]":
                            this.drawObject.XldContourData = (XldDataClass)e.DataContent;;
                            break;
                        case "HXLDCont":
                            this.drawObject.XldContourData = new XldDataClass((HXLDCont)e.DataContent) ;
                            break;
                    }
                }
            }
            catch (Exception he)
            {

            }
        }
        private void 显示条目comboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            //ExtractCircle.enShowItems content;
            //if (Enum.TryParse(this.显示条目comboBox.Text.Trim(), out content))
            //    switch (content)
            //    {
            //        case ExtractCircle.enShowItems.输出3D对象:
            //            this.drawObject.DisplayAttachProperty = false;
            //            this.drawObject.PointCloudModel3D = (HObjectModel3D[])this._function.GetPropertyValues("截取3D对象");
            //            break;
            //        case ExtractCircle.enShowItems.输入3D对象:
            //            this.drawObject.DisplayAttachProperty = true;
            //            this.drawObject.PointCloudModel3D = (HObjectModel3D[])this._function.GetPropertyValues("输入3D对象");
            //            break;
            //    }
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
                    this.toolStripButton_Translate.CheckState = CheckState.Unchecked;
                    this.drawObject.Show3D();
                    break;

                default:
                    break;
            }
        }

        private void FixedPointSampleModelForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (this.drawObject != null)
                this.drawObject.ClearDrawingObject();
                BaseFunction.ExcuteCompleted -= new ExcuteCompletedEventHandler(DisplayObjectModel);
                ListBoxWrapClass.ItemsChangeToForm -= new ItemsChangeEventHandler(listbox_AddItems);
            }
            catch
            {

            }
        }

        private void 区域类型comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch(this.区域类型comboBox.SelectedItem.ToString())
            {
                case "circle":
                    this.角度textBox.Enabled = false;
                    this.半宽textBox.Enabled = false;
                    this.半长textBox.Enabled = false;
                    this.半径textBox.Enabled = true;
                    break;

                case "rect2":
                    this.角度textBox.Enabled = true;
                    this.半宽textBox.Enabled = true;
                    this.半长textBox.Enabled = true;
                    this.半径textBox.Enabled = false;
                    break;
            }
        }
    }
}

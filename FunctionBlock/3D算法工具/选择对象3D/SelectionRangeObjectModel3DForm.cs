
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
    public partial class SelectionRangeObjectModel3DForm : Form
    {
        //
        private IFunction _function;
        private VisualizeView drawObject;
        private ListBoxWrapClass listBoxWrapClass1;
        private SelectObjectParam param;
        public SelectionRangeObjectModel3DForm(IFunction function,TreeNode node)
        {
            this._function = function;
            InitializeComponent();
            this.drawObject = new VisualizeView(this.hWindowControl1, true);
            listBoxWrapClass1 = new ListBoxWrapClass();
            listBoxWrapClass1.InitListBox(this.listBox1, node);
        }
        private void AppBaseForm_Load(object sender, EventArgs e)
        {
            // 注册事件
            this.drawObject.GrayValueInfo += new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
            BaseFunction.ExcuteCompleted += new ExcuteCompletedEventHandler(this.DisplayObjectModel);
            ListBoxWrapClass.ItemsChangeToForm += new ItemsChangeEventHandler(listbox_AddItems);
            BindProperty();
            this.drawObject.PointCloudModel3D = ((SelectObjectModelRange3D)this._function).DataObjectModel;
        }
        public enum enShowItems
        {
            输入对象3D,
            选择对象3D,
        }
        private void BindProperty()
        {
            try
            {
                this.显示条目comboBox.DataSource = Enum.GetNames(typeof(enShowItems));
                this.param = ((SelectObjectModelRange3D)this._function).SelectParam;
                this.最小值label.DataBindings.Add("Text", this.param, "MinZ_range", true, DataSourceUpdateMode.OnPropertyChanged);
                this.最大值label.DataBindings.Add("Text", this.param, "MaxZ_range", true, DataSourceUpdateMode.OnPropertyChanged);
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
            }
        }

        private void DisplayObjectModel(object send,ExcuteCompletedEventArgs e)
        {
            try
            {
                switch (e.DataContent.GetType().Name)
                {
                    case "HObjectModel3D":
                        this.drawObject.PointCloudModel3D = new PointCloudData ( (HObjectModel3D)e.DataContent );
                        break;
                    case "PointCloudData":
                        this.drawObject.PointCloudModel3D = ((PointCloudData)e.DataContent);
                        break;
                    case "HObjectModel3D[]":
                        this.drawObject.PointCloudModel3D =  new PointCloudData( (HObjectModel3D[])e.DataContent);
                        break;
                }
            }
            catch
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
                            this.drawObject.PointCloudModel3D = new PointCloudData ( (HObjectModel3D)object3D );
                            break;
                        case "PointCloudData":
                            this.drawObject.PointCloudModel3D = ((PointCloudData)object3D);
                            break;
                        case "HObjectModel3D[]":
                            this.drawObject.PointCloudModel3D = new PointCloudData((HObjectModel3D[])object3D);
                            break;
                    }
                }
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
            this.行坐标Label.Text = "x: " + e.Row.ToString();
            this.列坐标Label.Text = "y: " + e.Col.ToString();
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
                    this.drawObject.Show3D();
                    this.toolStripButton_Translate.CheckState = CheckState.Unchecked;
                    break;
                default:
                    break;
            }
        }



        private void 控制最小值trackBar_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                double Minvalue = this.控制最小值trackBar.Value * 0.001;
                this.param.Min_segment_value = Minvalue;
                this.当前最小值label.Text = this.param.Min_segment_value.ToString("f5");
                this._function.Execute(null);
            }
            catch
            {

            }
        }

        private void 控制最大值trackBar_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                double Maxvalue = this.控制最大值trackBar.Value * 0.001;
                this.param.Max_segment_value = Maxvalue;
                this.当前最大值label.Text = this.param.Max_segment_value.ToString("f5");
                this._function.Execute(null);
            }
            catch
            {

            }
        }
        private void 最小值label_TextChanged_1(object sender, EventArgs e)
        {
            try
            {      
                this.控制最小值trackBar.Minimum = (int)(param.MinZ_range * 1000);
                this.控制最小值trackBar.Maximum = (int)(param.MaxZ_range * 1000);
                this.控制最小值trackBar.Value = (int)(param.Min_segment_value * 1000);
                this.当前最小值label.Text = param.Min_segment_value.ToString("f5");
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
            }
        }

        private void 最大值label_TextChanged_1(object sender, EventArgs e)
        {
            try
            {
                this.控制最大值trackBar.Minimum = (int)(this.param.MinZ_range * 1000);
                this.控制最大值trackBar.Maximum = (int)(this.param.MaxZ_range * 1000);
                this.控制最大值trackBar.Value = (int)(this.param.Max_segment_value * 1000);
                this.当前最大值label.Text = this.param.Max_segment_value.ToString("f5");
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
            }
        }

        private void 获取范围button_Click(object sender, EventArgs e)
        {
            try
            {
                HTuple value = this.drawObject.PointCloudModel3D.GetObjectModel3dParams("point_coord_z");
                this.最小值label.Text = value.TupleMin().D.ToString("f5");
                this.最大值label.Text = value.TupleMax().D.ToString("f5");
                this.param.MinZ_range = value.TupleMin().D;
                this.param.MaxZ_range = value.TupleMax().D;
                this.param.Min_segment_value = value.TupleMin().D;
                this.param.Max_segment_value = value.TupleMax().D;
                /////////////////////
                this.当前最小值label.Text = this.param.Min_segment_value.ToString("f5");
                this.当前最大值label.Text = this.param.Max_segment_value.ToString("f5");
                this.控制最小值trackBar.Minimum = (int)(value.TupleMin().D * 1000);
                this.控制最小值trackBar.Maximum = (int)(value.TupleMax().D * 1000);
                this.控制最大值trackBar.Minimum = (int)(value.TupleMin().D * 1000);
                this.控制最大值trackBar.Maximum = (int)(value.TupleMax().D * 1000);
                this.控制最小值trackBar.Value = (int)(this.param.Min_segment_value * 1000);
                this.控制最大值trackBar.Value = (int)(this.param.Max_segment_value * 1000);
            }
            catch
            {
                MessageBox.Show("获取范围button_Click操作失败");
            }
        }

        private void SelectionRangeObjectModel3DForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                // 注消事件
                if(this.drawObject!=null)
                this.drawObject.ClearDrawingObject();
                this.drawObject.GrayValueInfo -= new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                BaseFunction.ExcuteCompleted -= new ExcuteCompletedEventHandler(this.DisplayObjectModel);
                ListBoxWrapClass.ItemsChangeToForm -= new ItemsChangeEventHandler(listbox_AddItems);
            }
            catch
            {

            }
        }


    }
}

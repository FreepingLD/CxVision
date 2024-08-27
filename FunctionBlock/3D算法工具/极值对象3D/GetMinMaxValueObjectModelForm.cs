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
    public partial class GetMinMaxValueObjectModelForm : Form
    {

        private Form form;
        private IFunction _function;
        private VisualizeView drawObject;
        private ListBoxWrapClass ListBoxWrapClass1, ListBoxWrapClass2;
        public GetMinMaxValueObjectModelForm(IFunction function,TreeNode node)
        {      
            this._function = function;
            InitializeComponent();
            this.drawObject = new VisualizeView(this.hWindowControl1,true);
            ListBoxWrapClass1 = new ListBoxWrapClass();
            ListBoxWrapClass1.InitListBox(this.listBox1, node);
            ListBoxWrapClass2 = new ListBoxWrapClass();
            ListBoxWrapClass2.InitListBox(this.listBox2, node, 2);
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
                this.显示条目comboBox.DataSource = Enum.GetNames(typeof(FunctionBlock.GetMinMaxValueObjectModel.enShowItems));
                this.直线偏置方式comboBox.DataSource = Enum.GetNames(typeof(enOffsetType));
                this.极值模式comboBox.DataSource = Enum.GetNames(typeof(enMinMaxMode));
                this.dataGridView1.DataSource = ((FunctionBlock.GetMinMaxValueObjectModel)this._function).SectionDataTable;
                //////////////////
                //this.激光线点间隔textBox.DataBindings.Add("Text", (GetMaxValueObjectModel)this._function, "PointPitch", true, DataSourceUpdateMode.OnPropertyChanged);
                this.极值模式comboBox.DataBindings.Add("Text", (FunctionBlock.GetMinMaxValueObjectModel)this._function, "MinMaxMode", true, DataSourceUpdateMode.OnPropertyChanged);
                this.平面距离阈值textBox.DataBindings.Add("Text", (FunctionBlock.GetMinMaxValueObjectModel)this._function, "PlaneOffsetDist", true, DataSourceUpdateMode.OnPropertyChanged);
                this.直线偏置距离textBox.DataBindings.Add("Text", (FunctionBlock.GetMinMaxValueObjectModel)this._function, "LineOffsetDist", true, DataSourceUpdateMode.OnPropertyChanged); // LineOffsetCount
                this.直线偏置数量textBox.DataBindings.Add("Text", (FunctionBlock.GetMinMaxValueObjectModel)this._function, "LineOffsetCount", true, DataSourceUpdateMode.OnPropertyChanged); // LineOffsetCount
                this.直线偏置方式comboBox.DataBindings.Add("Text", (FunctionBlock.GetMinMaxValueObjectModel)this._function, "LineOffsetMethod", true, DataSourceUpdateMode.OnPropertyChanged);
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
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
                            this.drawObject.XldContourData =                             this.drawObject.XldContourData = (XldDataClass)e.DataContent;;
                            break;
                        case "HXLDCont":
                            this.drawObject.XldContourData = new XldDataClass((HXLDCont)e.DataContent) ;
                            break;                            
                        case "userWcsLine":
                            this.drawObject.AttachPropertyData.Add((userWcsLine)e.DataContent);
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

        private void 添加直线button_Click(object sender, EventArgs e)
        {
            HalconLibrary ha = new HalconLibrary();
            userWcsLine wcsLine;
            //////////////////////////////////////
            try
            {
                this.drawObject.DrawWcsLineOnWindow(enColor.white, 0, 0, out wcsLine);
                /////////////    
                this._function.SetPropertyValues("添加直线", wcsLine); // "位置" + index.ToString()
                /////////////
                for (int i = 0; i < this.dataGridView1.Rows.Count; i++)
                {
                    this.dataGridView1.Rows[i].HeaderCell.Value = i.ToString();
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
            }
        }

        private void 删除截面button_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.dataGridView1.CurrentRow == null) return;
                this.dataGridView1.Rows.Remove(this.dataGridView1.CurrentRow);
                for (int i = 0; i < this.dataGridView1.Rows.Count; i++)
                {
                    this.dataGridView1.Rows[i].HeaderCell.Value = i.ToString();
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
            }
        }

        private void 清空直线button_Click(object sender, EventArgs e)
        {
            try
            {
                _function.SetPropertyValues("清空直线", null); //添加坐标点1
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
            }
        }

        private void 阵列直线button_Click(object sender, EventArgs e)
        {
            try
            {
                HalconLibrary ha = new HalconLibrary();
                int count = int.Parse(this.直线偏置数量textBox.Text);
                double dist = double.Parse(this.直线偏置距离textBox.Text);
                if (this.dataGridView1.CurrentRow == null) return;
                double x1 = Convert.ToDouble(this.dataGridView1.CurrentRow.Cells[0].Value);
                double y1 = Convert.ToDouble(this.dataGridView1.CurrentRow.Cells[1].Value);
                double z1 = Convert.ToDouble(this.dataGridView1.CurrentRow.Cells[2].Value);
                double x2 = Convert.ToDouble(this.dataGridView1.CurrentRow.Cells[3].Value);
                double y2 = Convert.ToDouble(this.dataGridView1.CurrentRow.Cells[4].Value);
                double z2 = Convert.ToDouble(this.dataGridView1.CurrentRow.Cells[5].Value);
                userWcsLine lineIn = new userWcsLine(x1, y1, z1, x2, y2, z2, null, null);
                this._function.SetPropertyValues("阵列直线", lineIn); // "位置" + index.ToString()
            }
            catch
            {

            }
        }

        private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false;
        }

        private void DeformableMatchForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // 注消事件
            try
            {
                if(this.drawObject!=null)
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

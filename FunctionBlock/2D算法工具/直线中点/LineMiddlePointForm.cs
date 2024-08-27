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
    public partial class LineMiddlePointForm : Form
    {
        private object _objectDataModel;
        private Form form;
        private IFunction _function;
        private VisualizeView drawObject;
        public LineMiddlePointForm(IFunction function,TreeNode node)
        {      
            this._function = function;
            InitializeComponent();
            this.Text = function.GetPropertyValues("名称").ToString();
            this.drawObject = new VisualizeView(this.hWindowControl1,false);
            new ListBoxWrapClass().InitListBox(this.listBox1, node);
        }
        private void LineMiddlePointForm_Load(object sender, EventArgs e)
        {
            // 注册事件
            BaseFunction.ExcuteCompleted += new ExcuteCompletedEventHandler(this.DisplayObjectModel);
            ListBoxWrapClass.ItemsChangeToForm += new ItemsChangeEventHandler(listbox_AddItems);
            BindProperty();
           
        }

        public enum enShowItems
        {
            输入对象,
            输出对象,
        }
        private void BindProperty()
        {
            try
            {
                this.显示条目comboBox.DataSource = Enum.GetNames(typeof(enShowItems));
                //this.结果dataGridView.DataSource = ((FunctionBlock.LineMiddlePoint)this._function).ResultDataTable;
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
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
                        case "PointCloudData":
                            this.drawObject.PointCloudModel3D = ((PointCloudData)e.DataContent);
                            break;
                        case "HImage":
                            this.drawObject.ClearViewObject();
                            this.drawObject.BackImage = new ImageDataClass((HImage)e.DataContent); // 图形窗口不显示图像
                            break;
                        case "ImageDataClass":
                            this.drawObject.ClearViewObject();
                            this.drawObject.BackImage = (ImageDataClass)e.DataContent;
                            break;
                        case "XldDataClass":
                            this.drawObject.AddViewObject(new ViewData(((XldDataClass)e.DataContent).HXldCont, ((XldDataClass)e.DataContent).Color.ToString()));
                            break;
                        case "HXLDCont":
                            this.drawObject.AddViewObject(new ViewData(e.DataContent, "red"));
                            break;
                        case "RegionDataClass":
                            this.drawObject.AddViewObject(new ViewData(((RegionDataClass)e.DataContent).Region, ((RegionDataClass)e.DataContent).Color.ToString()));
                            break;
                        case "userWcsCircle":
                            this.drawObject.AddViewObject(new ViewData(((userWcsCircle)e.DataContent), ((userWcsCircle)e.DataContent).Color.ToString()));
                            break;
                        case "userWcsCircleSector":
                            this.drawObject.AddViewObject(new ViewData(((userWcsCircleSector)e.DataContent), ((userWcsCircleSector)e.DataContent).Color.ToString()));
                            break;
                        case "userWcsEllipse":
                            this.drawObject.AddViewObject(new ViewData(((userWcsEllipse)e.DataContent), ((userWcsEllipse)e.DataContent).Color.ToString()));
                            break;
                        case "userWcsEllipseSector":
                            this.drawObject.AddViewObject(new ViewData(((userWcsEllipseSector)e.DataContent), ((userWcsEllipseSector)e.DataContent).Color.ToString()));
                            break;
                        case "userWcsLine":
                            this.drawObject.AddViewObject(new ViewData(((userWcsLine)e.DataContent), ((userWcsLine)e.DataContent).Color.ToString()));
                            break;
                        case "userWcsPoint":
                            this.drawObject.AddViewObject(new ViewData(((userWcsPoint)e.DataContent), ((userWcsPoint)e.DataContent).Color.ToString()));
                            break;
                        case "userWcsRectangle1":
                            this.drawObject.AddViewObject(new ViewData(((userWcsRectangle1)e.DataContent), ((userWcsRectangle1)e.DataContent).Color.ToString()));
                            break;
                        case "userWcsRectangle2":
                            this.drawObject.AddViewObject(new ViewData(((userWcsRectangle2)e.DataContent), ((userWcsRectangle2)e.DataContent).Color.ToString()));
                            break;
                    }
                }
            }
            catch (Exception he)
            {
                MessageBox.Show(he.ToString());
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
                    switch (object3D.GetType().Name) //这里只接受XLD轮廓或3D对象轮廓
                    {
                        case "PointCloudData":
                            this.drawObject.PointCloudModel3D = ((PointCloudData)object3D);
                            break;
                        case "HImage":
                            this.drawObject.ClearViewObject();
                            this.drawObject.BackImage = new ImageDataClass((HImage)object3D); // 图形窗口不显示图像
                            break;
                        case "ImageDataClass":
                            this.drawObject.ClearViewObject();
                            this.drawObject.BackImage = (ImageDataClass)object3D;
                            break;
                        case "XldDataClass":
                            this.drawObject.AddViewObject(new ViewData(((XldDataClass)object3D).HXldCont, ((XldDataClass)object3D).Color.ToString()));
                            break;
                        case "HXLDCont":
                            this.drawObject.AddViewObject(new ViewData(object3D, "green"));
                            break;
                        case "RegionDataClass":
                            this.drawObject.AddViewObject(new ViewData(((RegionDataClass)object3D).Region, ((RegionDataClass)object3D).Color.ToString()));
                            break;
                        case "userWcsCircle":
                            this.drawObject.AddViewObject(new ViewData(((userWcsCircle)object3D), ((userWcsCircle)object3D).Color.ToString()));
                            break;
                        case "userWcsCircleSector":
                            this.drawObject.AddViewObject(new ViewData(((userWcsCircleSector)object3D), ((userWcsCircleSector)object3D).Color.ToString()));
                            break;
                        case "userWcsEllipse":
                            this.drawObject.AddViewObject(new ViewData(((userWcsEllipse)object3D), ((userWcsEllipse)object3D).Color.ToString()));
                            break;
                        case "userWcsEllipseSector":
                            this.drawObject.AddViewObject(new ViewData(((userWcsEllipseSector)object3D), ((userWcsEllipseSector)object3D).Color.ToString()));
                            break;
                        case "userWcsLine":
                            this.drawObject.AddViewObject(new ViewData(((userWcsLine)object3D), ((userWcsLine)object3D).Color.ToString()));
                            break;
                        case "userWcsPoint":
                            this.drawObject.AddViewObject(new ViewData(((userWcsPoint)object3D), ((userWcsPoint)object3D).Color.ToString()));
                            break;
                        case "userWcsRectangle1":
                            this.drawObject.AddViewObject(new ViewData(((userWcsRectangle1)object3D), ((userWcsRectangle1)object3D).Color.ToString()));
                            break;
                        case "userWcsRectangle2":
                            this.drawObject.AddViewObject(new ViewData(((userWcsRectangle2)object3D), ((userWcsRectangle2)object3D).Color.ToString()));
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
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
                    switch (object3D.GetType().Name) //这里只接受XLD轮廓或3D对象轮廓
                    {
                        case "PointCloudData":
                            this.drawObject.PointCloudModel3D = ((PointCloudData)object3D);
                            break;
                        case "HImage":
                            this.drawObject.ClearViewObject();
                            this.drawObject.BackImage = new ImageDataClass((HImage)object3D); // 图形窗口不显示图像
                            break;
                        case "ImageDataClass":
                            this.drawObject.ClearViewObject();
                            this.drawObject.BackImage = (ImageDataClass)object3D;
                            break;
                        case "XldDataClass":
                            this.drawObject.AddViewObject(new ViewData(((XldDataClass)object3D).HXldCont, ((XldDataClass)object3D).Color.ToString()));
                            break;
                        case "HXLDCont":
                            this.drawObject.AddViewObject(new ViewData(object3D, "green"));
                            break;
                        case "RegionDataClass":
                            this.drawObject.AddViewObject(new ViewData(((RegionDataClass)object3D).Region, ((RegionDataClass)object3D).Color.ToString()));
                            break;
                        case "userWcsCircle":
                            this.drawObject.AddViewObject(new ViewData(((userWcsCircle)object3D), ((userWcsCircle)object3D).Color.ToString()));
                            break;
                        case "userWcsCircleSector":
                            this.drawObject.AddViewObject(new ViewData(((userWcsCircleSector)object3D), ((userWcsCircleSector)object3D).Color.ToString()));
                            break;
                        case "userWcsEllipse":
                            this.drawObject.AddViewObject(new ViewData(((userWcsEllipse)object3D), ((userWcsEllipse)object3D).Color.ToString()));
                            break;
                        case "userWcsEllipseSector":
                            this.drawObject.AddViewObject(new ViewData(((userWcsEllipseSector)object3D), ((userWcsEllipseSector)object3D).Color.ToString()));
                            break;
                        case "userWcsLine":
                            this.drawObject.AddViewObject(new ViewData(((userWcsLine)object3D), ((userWcsLine)object3D).Color.ToString()));
                            break;
                        case "userWcsPoint":
                            this.drawObject.AddViewObject(new ViewData(((userWcsPoint)object3D), ((userWcsPoint)object3D).Color.ToString()));
                            break;
                        case "userWcsRectangle1":
                            this.drawObject.AddViewObject(new ViewData(((userWcsRectangle1)object3D), ((userWcsRectangle1)object3D).Color.ToString()));
                            break;
                        case "userWcsRectangle2":
                            this.drawObject.AddViewObject(new ViewData(((userWcsRectangle2)object3D), ((userWcsRectangle2)object3D).Color.ToString()));
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

                    break;
                default:
                    break;
            }
        }
        private void LineMiddlePointForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (this.drawObject != null)
                    this.drawObject.ClearDrawingObject();
                // 注消事件
                ListBoxWrapClass.ItemsChangeToForm -= new ItemsChangeEventHandler(listbox_AddItems);
                BaseFunction.ExcuteCompleted -= new ExcuteCompletedEventHandler(this.DisplayObjectModel);
            }
            catch
            {

            }
        }

        private void 结果dataGridView_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false;
        }
    }
}

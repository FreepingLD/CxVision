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
    public partial class CoordSystemLineLineAppForm : Form
    {

        private object _objectDataModel;
        private Form form;
        private IFunction _function;
        private VisualizeView drawObject;
        private HWindowControl hWindowControl1;

        public CoordSystemLineLineAppForm(TreeNode node,HWindowControl hWindowControl)
        {
            InitializeComponent();
            this._function = node.Tag as IFunction;
            this.hWindowControl1 = hWindowControl;  
            this.drawObject = new VisualizeView(this.hWindowControl1, true);
            //new ListBoxWrapClass().InitListBox(this.listBox1, node);
            //new ListBoxWrapClass().InitListBox(this.listBox2, node, 2);
        }
        private void CoordSystemLineLineForm_Load(object sender, EventArgs e)
        {
            // 注册事件
            BaseFunction.ExcuteCompleted += new ExcuteCompletedEventHandler(this.DisplayObjectModel);
            ListBoxWrapClass.ItemsChangeToForm += new ItemsChangeEventHandler(listbox_AddItems);
            BindProperty();
            ///////////////////////////////
            this.drawObject.AttachPropertyData.Add(((FunctionBlock.CoordSystemLineLine)this._function).WcsLine1);
            this.drawObject.AttachPropertyData.Add(((FunctionBlock.CoordSystemLineLine)this._function).WcsLine2);
        }

        public enum enShowItems
        {
            输入对象1,
            输入对象2,
            输出对象,
        }

        private void BindProperty()
        {
            try
            {
                // 创建匹配参数
                //this.显示条目comboBox.DataSource = Enum.GetNames(typeof(enShowItems));
                //this.dataGridView1.DataSource = ((FunctionBlock.CoordSystemLineLine)this._function).ResultDataTable;  //resetShapeModel
                this.初始化参考点comboBox.DataBindings.Add("Text", ((FunctionBlock.CoordSystemLineLine)this._function).Param, "IsInit", true, DataSourceUpdateMode.OnPropertyChanged);
                this.坐标原点comboBox.DataSource = Enum.GetNames(typeof(enOrigionType));  //resetShapeModel
                this.补正类型comboBox.DataSource = Enum.GetNames(typeof(enAdjustType));  //resetShapeModel
                this.坐标原点comboBox.DataBindings.Add("Text", ((FunctionBlock.CoordSystemLineLine)this._function).Param, "OrigionType", true, DataSourceUpdateMode.OnPropertyChanged);
                this.补正类型comboBox.DataBindings.Add("Text", ((FunctionBlock.CoordSystemLineLine)this._function).Param, "AdjustType", true, DataSourceUpdateMode.OnPropertyChanged);
                /////////////////////////////////////////////////////////////////////////////////////////////////////
                userWcsCoordSystem coordSystem = ((FunctionBlock.CoordSystemLineLine)this._function).WcsCoordSystem;
                userPixCoordSystem coordSystemPix = ((FunctionBlock.CoordSystemLineLine)this._function).PixCoordSystem;
                if (coordSystem != null)
                {
                    this.当前世界坐标textBox.Text = $"X:={Math.Round(coordSystem.CurrentPoint.X, 3)},Y:={Math.Round(coordSystem.CurrentPoint.Y, 3)},Angle:={Math.Round(coordSystem.CurrentPoint.Angle, 3)}";
                    this.示教世界坐标textBox.Text = $"X:={Math.Round(coordSystem.ReferencePoint.X, 3)},Y:={Math.Round(coordSystem.ReferencePoint.Y, 3)},Angle:={Math.Round(coordSystem.ReferencePoint.Angle, 3)}";
                }
                if (coordSystemPix != null)
                {
                    this.当前像素坐标textBox.Text = $"Row:={Math.Round(coordSystemPix.CurrentPoint.Row, 3)},Col:={Math.Round(coordSystemPix.CurrentPoint.Col, 3)},Theta:={Math.Round(coordSystemPix.CurrentPoint.Rad, 3)}";
                    this.示教像素坐标textBox.Text = $"Row:={Math.Round(coordSystemPix.ReferencePoint.Row, 3)},Col:={Math.Round(coordSystemPix.ReferencePoint.Col, 3)},Theta:={Math.Round(coordSystemPix.ReferencePoint.Rad, 3)}";
                }
            }
            catch (Exception ex)
            {
                new Common.UserMessageForm(ex.ToString()).ShowDialog();
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
                        case "userWcsPoint[]":
                            //this.drawObject.AttachPropertyData.Clear();
                            //userWcsPoint[] wcsPoint = (userWcsPoint[])this._function.GetPropertyValues(this.显示条目comboBox.Text.Trim());
                            //for (int i = 0; i < wcsPoint.Length; i++)
                            //{
                            //    this.drawObject.AttachPropertyData.Add(wcsPoint[i]);
                            //}
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
                            this.drawObject.PointCloudModel3D = new PointCloudData( (HObjectModel3D)object3D );
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

        private void CoordSystemLineLineAppForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // 注消事件
            try
            {
                if (this.drawObject != null)
                    this.drawObject.ClearDrawingObject();
                BaseFunction.ExcuteCompleted -= new ExcuteCompletedEventHandler(this.DisplayObjectModel);
                ListBoxWrapClass.ItemsChangeToForm -= new ItemsChangeEventHandler(listbox_AddItems);
            }
            catch
            {

            }
        }


        private void 重置坐标系but_Click(object sender, EventArgs e)
        {
            try
            {
                ((FunctionBlock.CoordSystemLineLine)this._function).Param.IsInit = false;
                this._function.Execute(null);
                /////////////////////////////////////////////////////////////////////////////////////////////////////
                userWcsCoordSystem coordSystem = ((FunctionBlock.CoordSystemLineLine)this._function).WcsCoordSystem;
                userPixCoordSystem coordSystemPix = ((FunctionBlock.CoordSystemLineLine)this._function).PixCoordSystem;
                if (coordSystem != null)
                {
                    this.当前世界坐标textBox.Text = $"X:={Math.Round(coordSystem.CurrentPoint.X, 3)},Y:={Math.Round(coordSystem.CurrentPoint.Y, 3)},Angle:={Math.Round(coordSystem.CurrentPoint.Angle, 3)}";
                    this.示教世界坐标textBox.Text = $"X:={Math.Round(coordSystem.ReferencePoint.X, 3)},Y:={Math.Round(coordSystem.ReferencePoint.Y, 3)},Angle:={Math.Round(coordSystem.ReferencePoint.Angle, 3)}";
                }
                if (coordSystemPix != null)
                {
                    this.当前像素坐标textBox.Text = $"Row:={Math.Round(coordSystemPix.CurrentPoint.Row, 3)},Col:={Math.Round(coordSystemPix.CurrentPoint.Col, 3)},Theta:={Math.Round(coordSystemPix.CurrentPoint.Rad, 3)}";
                    this.示教像素坐标textBox.Text = $"Row:={Math.Round(coordSystemPix.ReferencePoint.Row, 3)},Col:={Math.Round(coordSystemPix.ReferencePoint.Col, 3)},Theta:={Math.Round(coordSystemPix.ReferencePoint.Rad, 3)}";
                }
            }
            catch (Exception ex)
            {
               new Common.UserMessageForm(ex.ToString()).ShowDialog();;
            }
        }

        private void 执行Btn_Click(object sender, EventArgs e)
        {
            try
            {
                switch (this.执行Btn.Text)
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
            catch (Exception ex)
            {
                new Common.UserMessageForm(ex.ToString()).ShowDialog();
            }
        }



    }
}

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
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using View;

namespace FunctionBlock
{
    public partial class ImageReduceForm : Form
    {
        private Form form;
        private IFunction _function;
        private VisualizeView drawObject;
        private object _objectDataModel;
        public ImageReduceForm(IFunction function, TreeNode node)
        {
            this._function = function;
            InitializeComponent();
            this.drawObject = new VisualizeView(this.hWindowControl1, false);
            new ListBoxWrapClass().InitListBox(this.listBox1, node);
            new ListBoxWrapClass().InitListBox(this.listBox2, node,2);
            //this.addContextMenu(this.dataGridView1);
        }
        private void ImageReduceForm_Load(object sender, EventArgs e)
        {
            // 注册事件
            this.drawObject.GrayValueInfo += new GrayValueInfoEventHandler(GetGrayValueInfo);
            BaseFunction.ExcuteCompleted += new ExcuteCompletedEventHandler(this.DisplayObjectModel);
            ListBoxWrapClass.ItemsChangeToForm += new ItemsChangeEventHandler(listbox_AddItems);
            BindProperty();
            //////////////////////////////////////////////////////////
            this.addDataGridViewContextMenu(this.dataGridView1);
        }
        private void BindProperty()
        {
            try
            {
                this.ShapeCol.Items.Clear();
                this.ShapeCol.ValueType = typeof(enShapeType);
                foreach (enShapeType item in Enum.GetValues(typeof(enShapeType)))
                {
                    this.ShapeCol.Items.Add(item);
                }
                this.OperateCol.Items.Clear();
                this.OperateCol.ValueType = typeof(enInsideOrOutside);
                foreach (enInsideOrOutside item in Enum.GetValues(typeof(enInsideOrOutside)))
                {
                    this.OperateCol.Items.Add(item);
                }
                this.dataGridView1.TopLeftHeaderCell.Value = "序号";
                this.dataGridView1.DataSource = ((ImageReduce)this._function).Param;
                this.显示条目comboBox.DataSource = Enum.GetValues(typeof(enShowItem));
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

        #region 数据视图右键菜单项
        private void addDataGridViewContextMenu(DataGridView dataGridView)
        {
            ContextMenuStrip ContextMenuStrip1 = new ContextMenuStrip();
            // 添加右键菜单 
            ToolStripItem[] items = new ToolStripMenuItem[]
            {
                new ToolStripMenuItem("删除"),
                new ToolStripMenuItem("清空"),
                new ToolStripMenuItem("矩形阵列"),
                new ToolStripMenuItem("圆形阵列"),
            };
            ContextMenuStrip1.Items.AddRange(items); // 在滑赋值前不能调用 
            ContextMenuStrip1.ItemClicked += new ToolStripItemClickedEventHandler(dataGridViewContextMenuStrip_ItemClicked);
            dataGridView.ContextMenuStrip = ContextMenuStrip1;
        }
        private void dataGridViewContextMenuStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            string name = e.ClickedItem.Text;
            int index = 0;
            try
            {
                BindingList<ReduceParam> listShape = ((ImageReduce)this._function).Param;
                ((ContextMenuStrip)sender).Close();
                switch (name)
                {
                    case "删除":
                        ((ContextMenuStrip)sender).Close();
                        if (this.dataGridView1.CurrentRow != null)
                            index = this.dataGridView1.CurrentRow.Index;
                        listShape.RemoveAt(index);
                        if (this.drawObject.AttachPropertyData.Count > index)
                            this.drawObject.AttachPropertyData.RemoveAt(index);
                        this.drawObject.DrawingGraphicObject();
                        for (int i = 0; i < this.dataGridView1.Rows.Count; i++)
                        {
                            if (this.dataGridView1.Rows.Count > i)
                                this.dataGridView1.Rows[i].HeaderCell.Value = (i + 1).ToString();
                        }
                        break;
                    //////////////////////////////////////
                    case "清空":
                        ((ContextMenuStrip)sender).Close();
                        this.drawObject.AttachPropertyData.Clear();
                        listShape.Clear();
                        this.drawObject.DrawingGraphicObject();
                        break;

                    case "矩形阵列":
                        if (this.dataGridView1.CurrentRow == null) return;
                        index = this.dataGridView1.CurrentRow.Index;
                        RectangleArrayDataForm rectform = new RectangleArrayDataForm();
                        rectform.Owner = this;
                        rectform.ShowDialog();
                        HHomMat2D hHomMat2D = new HHomMat2D();
                        HHomMat2D hHomMat_tras;
                        //////////////////////////////////////////
                        Task.Run(() =>
                        {
                            for (int i = 0; i < rectform.RowCount; i++)
                            {
                                for (int j = 0; j < rectform.ColCount; j++)
                                {
                                    if (i == 0 && j == 0) continue; //选定行不变
                                    hHomMat_tras = hHomMat2D.HomMat2dTranslate(rectform.OffsetX * j, rectform.OffsetY * i);
                                    switch (listShape[index].RoiShape.GetType().Name)
                                    {
                                        case nameof(drawPixPoint):
                                            this.Invoke(new Action(() => { listShape.Add(new ReduceParam(((drawPixPoint)listShape[index].RoiShape).AffinePixPoint(hHomMat_tras))); }));
                                            break;
                                        case nameof(drawPixLine):
                                            this.Invoke(new Action(() => { listShape.Add(new ReduceParam(((drawPixLine)listShape[index].RoiShape).AffinePixLine(hHomMat_tras))); }));
                                            break;
                                        case nameof(drawPixCircle):
                                            this.Invoke(new Action(() => { listShape.Add(new ReduceParam(((drawPixCircle)listShape[index].RoiShape).AffinePixCircle(hHomMat_tras))); }));
                                            break;
                                        case nameof(drawPixEllipse):
                                            this.Invoke(new Action(() => { listShape.Add(new ReduceParam(((drawPixEllipse)listShape[index].RoiShape).AffinePixEllipse(hHomMat_tras))); }));
                                            break;
                                        case nameof(drawPixRect1):
                                            this.Invoke(new Action(() => { listShape.Add(new ReduceParam(((drawPixRect1)listShape[index].RoiShape).AffineTransPixRect1(hHomMat_tras))); }));
                                            break;
                                        case nameof(drawPixRect2):
                                            this.Invoke(new Action(() => { listShape.Add(new ReduceParam(((drawPixRect2)listShape[index].RoiShape).AffinePixRect2(hHomMat_tras))); }));
                                            break;
                                        case nameof(drawPixPolygon):
                                            this.Invoke(new Action(() => { listShape.Add(new ReduceParam(((drawPixPolygon)listShape[index].RoiShape).AffinePixPolygon(hHomMat_tras))); }));
                                            break;
                                    }
                                    Thread.Sleep(100);
                                }
                            }
                        });
                        rectform.Close();
                        break;
                    case "圆形阵列":
                        CircleArrayDataForm circleForm = new CircleArrayDataForm();
                        circleForm.Owner = this;
                        circleForm.ShowDialog();
                        hHomMat2D = new HHomMat2D();
                        HHomMat2D hHomMat_Rota;
                        Task.Run(() =>
                        {
                            for (int i = 1; i < circleForm.ArrayNum; i++)
                            {
                                hHomMat_Rota = hHomMat2D.HomMat2dRotate(circleForm.Add_Deg * i * Math.PI / 180, circleForm.Radius + circleForm.Ref_X, circleForm.Radius + circleForm.Ref_Y);
                                switch (listShape[index].RoiShape.GetType().Name)
                                {
                                    case nameof(drawPixPoint):
                                        this.Invoke(new Action(() => { listShape.Add(new ReduceParam(((drawPixPoint)listShape[index].RoiShape).AffinePixPoint(hHomMat_Rota))); }));
                                        break;
                                    case nameof(drawPixLine):
                                        this.Invoke(new Action(() => { listShape.Add(new ReduceParam(((drawPixLine)listShape[index].RoiShape).AffinePixLine(hHomMat_Rota))); }));
                                        break;
                                    case nameof(drawPixCircle):
                                        this.Invoke(new Action(() => { listShape.Add(new ReduceParam(((drawPixCircle)listShape[index].RoiShape).AffinePixCircle(hHomMat_Rota))); }));
                                        break;
                                    case nameof(drawPixEllipse):
                                        this.Invoke(new Action(() => { listShape.Add(new ReduceParam(((drawPixEllipse)listShape[index].RoiShape).AffinePixEllipse(hHomMat_Rota))); }));
                                        break;
                                    case nameof(drawPixRect1):
                                        this.Invoke(new Action(() => { listShape.Add(new ReduceParam(((drawPixRect1)listShape[index].RoiShape).AffineTransPixRect1(hHomMat_Rota))); }));
                                        break;
                                    case nameof(drawPixRect2):
                                        this.Invoke(new Action(() => { listShape.Add(new ReduceParam(((drawPixRect2)listShape[index].RoiShape).AffinePixRect2(hHomMat_Rota))); }));
                                        break;
                                    case nameof(drawPixPolygon):
                                        this.Invoke(new Action(() => { listShape.Add(new ReduceParam(((drawPixPolygon)listShape[index].RoiShape).AffinePixPolygon(hHomMat_Rota))); }));
                                        break;
                                }
                            }
                        });
                        break;
                    ///////////////////////////////////////////////
                    default:
                        break;
                }
            }
            catch
            {
            }
        }
        #endregion

        // 获取鼠标位置处的高度值
        private void GetGrayValueInfo(object sender, GrayValueInfoEventArgs e)
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
        private void DisplayObjectModel(object sender, ExcuteCompletedEventArgs e)  // 这里一定要加一个键值对变量来收集，以免出现重合
        {
            try
            {
                if (e.DataContent != null)
                {
                    switch (e.DataContent.GetType().Name) //这里只接受XLD轮廓或3D对象轮廓
                    {
                        case "HImage":
                            this.drawObject.ClearViewObject();
                            this.drawObject.BackImage = new ImageDataClass((HImage)e.DataContent);
                            this._objectDataModel = this.drawObject.BackImage;
                            this.drawObject.AttachPropertyData.Clear();
                            break;
                        case "ImageDataClass":
                            this.drawObject.ClearViewObject();
                            this.drawObject.BackImage = (ImageDataClass)e.DataContent;
                            this._objectDataModel = this.drawObject.BackImage;
                            break;
                        case "XldDataClass":
                            this.drawObject.AddViewObject(new ViewData(((XldDataClass)e.DataContent).HXldCont,"red"));
                            break;
                        case "XldDataClass[]":
                            XldDataClass[] xldDataClasses = (XldDataClass[])e.DataContent;
                            foreach (var item in xldDataClasses)
                            {
                                this.drawObject.AddViewObject(new ViewData(((XldDataClass)item).HXldCont, "red"));
                            }                         
                            break;
                        case "HXLDCont":
                            this.drawObject.AddViewObject(new ViewData(((HXLDCont)e.DataContent), "red"));
                            break;
                        case "HRegion":
                            this.drawObject.AddViewObject(new ViewData(((HRegion)e.DataContent), "red"));
                            break;
                        case "RegionDataClass[]":
                            RegionDataClass[] regionDataClasses = (RegionDataClass[])e.DataContent;
                            foreach (var item in regionDataClasses)
                            {
                                this.drawObject.AddViewObject(new ViewData(item, "red"));
                            }
                            break;
                        case "RegionDataClass":
                            this.drawObject.AddViewObject(new ViewData(((RegionDataClass)e.DataContent).Region, "red"));
                            break;
                    }
                }
            }
            catch (Exception he)
            {
                MessageBox.Show("DisplayObjectModel->操作失败");
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
                        case "HImage":
                            this.drawObject.ClearViewObject();
                            this.drawObject.BackImage = new ImageDataClass((HImage)object3D);
                            this._objectDataModel = this.drawObject.BackImage;
                            this.drawObject.AttachPropertyData.Clear();
                            break;
                        case "ImageDataClass":
                            this.drawObject.ClearViewObject();
                            this.drawObject.BackImage = (ImageDataClass)object3D;
                            this._objectDataModel = this.drawObject.BackImage;
                            break;
                        case "XldDataClass":
                            this.drawObject.AddViewObject(new ViewData(((XldDataClass)object3D).HXldCont, "red"));
                            break;
                        case "XldDataClass[]":
                            XldDataClass[] xldDataClasses = (XldDataClass[])object3D;
                            foreach (var item in xldDataClasses)
                            {
                                this.drawObject.AddViewObject(new ViewData(((XldDataClass)item).HXldCont, "red"));
                            }
                            break;
                        case "HXLDCont":
                            this.drawObject.AddViewObject(new ViewData(((HXLDCont)object3D), "red"));
                            break;
                        case "HRegion":
                            this.drawObject.AddViewObject(new ViewData(((HRegion)object3D), "red"));
                            break;
                        case "RegionDataClass[]":
                            RegionDataClass[] regionDataClasses = (RegionDataClass[])object3D;
                            foreach (var item in regionDataClasses)
                            {
                                this.drawObject.AddViewObject(new ViewData(item, "red"));
                            }
                            break;
                        case "RegionDataClass":
                            this.drawObject.AddViewObject(new ViewData(((RegionDataClass)object3D).Region, "red"));
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

        private void BlobForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (this.drawObject != null)
                    this.drawObject.ClearDrawingObject();
                // 注消事件
                this.drawObject.GrayValueInfo -= new GrayValueInfoEventHandler(GetGrayValueInfo);
                ListBoxWrapClass.ItemsChangeToForm -= new ItemsChangeEventHandler(listbox_AddItems);
                BaseFunction.ExcuteCompleted -= new ExcuteCompletedEventHandler(this.DisplayObjectModel);
            }
            catch
            {

            }
        }

        private void 显示条目comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.显示条目comboBox.SelectedIndex == -1) return;
                switch (this.显示条目comboBox.SelectedItem.ToString())
                {
                    case nameof(enShowItem.源图像):
                        this.drawObject.AttachPropertyData.Clear();
                        this.drawObject.BackImage = ((ImageReduce)this._function).ImageData;
                        break;
                    case nameof(enShowItem.区域):
                        this.drawObject.AttachPropertyData.Clear();
                        this.drawObject.BackImage = ((ImageReduce)this._function).ReduceImage;
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void 设置分割区域button_Click(object sender, EventArgs e)
        {
            try
            {
                userPixRectangle2 pixRecrt2;
                this.drawObject.SetParam(null);
                this.drawObject.DrawPixRect2OnWindow(enColor.red, out pixRecrt2);
                //((FunctionBlock.ImageReduce)this._function).ReduceParam.Add(new drawPixRect2(pixRecrt2.row, pixRecrt2.col, pixRecrt2.rad, pixRecrt2.length1, pixRecrt2.length2));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void 示教Blob区域_Btn_Click(object sender, EventArgs e)
        {
            try
            {
                //userPixRectangle2 outPixRect2;
                //if (this.dataGridView1.CurrentRow.Index == -1) return;
                //drawPixRect2 pixRect2 = ((FunctionBlock.ImageReduce)this._function).ReduceParam[this.dataGridView1.CurrentRow.Index];
                //this.drawObject.AttachPropertyData.Clear();
                //this.drawObject.ModifyPixRect2OnWindow(pixRect2.GetUserPixRectangle2(),out outPixRect2);
                //((FunctionBlock.ImageReduce)this._function).ReduceParam[this.dataGridView1.CurrentRow.Index] = outPixRect2.GetDrawPixRect2();
                //this.drawObject.AttachPropertyData.Add(outPixRect2);
                //this.drawObject.DrawingGraphicObject();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    int index = 0;
                    PixROI pixShape;
                    if (((ImageReduce)this._function).Param == null)
                        ((ImageReduce)this._function).Param = new BindingList<ReduceParam>();
                    BindingList<ReduceParam> listShape = ((ImageReduce)this._function).Param;
                    userPixCoordSystem pixCoordSystem = ((ImageReduce)this._function).PixCoordSystem;
                    switch (dataGridView1.Columns[e.ColumnIndex].Name)
                    {
                        case "TeachCol":
                            if (this.dataGridView1.Rows[e.RowIndex].DataBoundItem == null)
                            {
                                MessageBox.Show("未设置必需的图形参数，请先设置参数!");
                                return;
                            }
                            switch (listShape[e.RowIndex].ShapeType)
                            {
                                case enShapeType.矩形2:
                                    this.drawObject.AttachPropertyData.Clear();
                                    if (!(this.drawObject is userDrawRect2ROI))
                                    {
                                        this.drawObject.GrayValueInfo -= new GrayValueInfoEventHandler(GetGrayValueInfo);
                                        this.drawObject.ClearDrawingObject();
                                        this.drawObject = new userDrawRect2ROI(this.hWindowControl1, false);
                                        this.drawObject.GrayValueInfo += new GrayValueInfoEventHandler(GetGrayValueInfo);
                                    }
                                    break;
                                case enShapeType.矩形1:
                                    this.drawObject.AttachPropertyData.Clear();
                                    if (!(this.drawObject is userDrawRect1ROI))
                                    {
                                        this.drawObject.GrayValueInfo -= new GrayValueInfoEventHandler(GetGrayValueInfo);
                                        this.drawObject.ClearDrawingObject();
                                        this.drawObject = new userDrawRect1ROI(this.hWindowControl1, false);
                                        this.drawObject.GrayValueInfo += new GrayValueInfoEventHandler(GetGrayValueInfo);
                                    }
                                    break;
                                case enShapeType.圆:
                                    this.drawObject.AttachPropertyData.Clear();
                                    if (!(this.drawObject is userDrawCircleROI))
                                    {
                                        this.drawObject.GrayValueInfo -= new GrayValueInfoEventHandler(GetGrayValueInfo);
                                        this.drawObject.ClearDrawingObject();
                                        this.drawObject = new userDrawCircleROI(this.hWindowControl1, false);
                                        this.drawObject.GrayValueInfo += new GrayValueInfoEventHandler(GetGrayValueInfo);
                                    }
                                    break;
                                case enShapeType.椭圆:
                                    this.drawObject.AttachPropertyData.Clear();
                                    if (!(this.drawObject is userDrawEllipseROI))
                                    {
                                        this.drawObject.GrayValueInfo -= new GrayValueInfoEventHandler(GetGrayValueInfo);
                                        this.drawObject.ClearDrawingObject();
                                        this.drawObject = new userDrawEllipseROI(this.hWindowControl1, false);
                                        this.drawObject.GrayValueInfo += new GrayValueInfoEventHandler(GetGrayValueInfo);
                                    }
                                    break;
                                case enShapeType.多边形:
                                    this.drawObject.AttachPropertyData.Clear();
                                    if (!(this.drawObject is userDrawPolygonROI))
                                    {
                                        this.drawObject.GrayValueInfo -= new GrayValueInfoEventHandler(GetGrayValueInfo);
                                        this.drawObject.ClearDrawingObject();
                                        this.drawObject = new userDrawPolygonROI(this.hWindowControl1, false);
                                        this.drawObject.GrayValueInfo += new GrayValueInfoEventHandler(GetGrayValueInfo);
                                    }
                                    break;
                                case enShapeType.点:
                                    this.drawObject.AttachPropertyData.Clear();
                                    if (!(this.drawObject is userDrawPointROI))
                                    {
                                        this.drawObject.GrayValueInfo -= new GrayValueInfoEventHandler(GetGrayValueInfo);
                                        this.drawObject.ClearDrawingObject();
                                        this.drawObject = new userDrawPointROI(this.hWindowControl1, false);
                                        this.drawObject.GrayValueInfo += new GrayValueInfoEventHandler(GetGrayValueInfo);
                                    }
                                    break;
                                case enShapeType.线:
                                    this.drawObject.AttachPropertyData.Clear();
                                    if (!(this.drawObject is userDrawLineROI))
                                    {
                                        this.drawObject.GrayValueInfo -= new GrayValueInfoEventHandler(GetGrayValueInfo);
                                        this.drawObject.ClearDrawingObject();
                                        this.drawObject = new userDrawLineROI(this.hWindowControl1, false);
                                        this.drawObject.GrayValueInfo += new GrayValueInfoEventHandler(GetGrayValueInfo);
                                    }
                                    break;
                                default:
                                    throw new NotImplementedException(listShape[e.RowIndex].ShapeType.ToString() + "未实现!");
                            }
                            this.drawObject.IsLiveState = true;
                            //////////////////////////
                            foreach (var item in listShape)
                            {
                                if (index != e.RowIndex && item.RoiShape != null)
                                {
                                    this.drawObject.AddViewObject(new ViewData(item.RoiShape.AffinePixROI(pixCoordSystem?.GetHomMat2D()).GetXLD(), enColor.orange.ToString()));
                                }
                                index++;
                            }
                            if (this.drawObject.BackImage == null)
                                this.drawObject.BackImage = ((ImageReduce)this._function).ImageData;
                            if (listShape[e.RowIndex].RoiShape == null)
                                this.drawObject.SetParam(null);
                            else
                            {
                                this.drawObject.SetParam(pixCoordSystem);
                                this.drawObject.SetParam(listShape[e.RowIndex].RoiShape.AffinePixROI(pixCoordSystem?.GetHomMat2D()));
                            }                              
                            this.drawObject.DrawPixRoiShapeOnWindow(enColor.red, out pixShape);
                            this.drawObject.AddViewObject(new ViewData(pixShape.AffinePixROI(pixCoordSystem?.GetHomMat2D()).GetXLD(), enColor.orange.ToString()));
                            /////////////////////////////////////////////////////////
                            listShape[e.RowIndex].RoiShape = pixShape;  // 这个地方的添加不能使用变换后数据
                            //////////////////////////////////////////////////////////
                            for (int i = 0; i < this.dataGridView1.Rows.Count; i++)
                            {
                                this.dataGridView1.Rows[i].HeaderCell.Value = (i + 1).ToString();
                            }
                            break;
                        case "DeletCol":
                            if (listShape == null) return;
                            if (listShape.Count > e.RowIndex)
                                listShape.RemoveAt(e.RowIndex);
                            if (this.drawObject.AttachPropertyData.Count > e.RowIndex)
                                this.drawObject.AttachPropertyData.RemoveAt(e.RowIndex);
                            this.drawObject.DrawingGraphicObject();
                            for (int i = 0; i < this.dataGridView1.Rows.Count; i++)
                            {
                                if (this.dataGridView1.Rows.Count > i)
                                    this.dataGridView1.Rows[i].HeaderCell.Value = (i + 1).ToString();
                            }
                            break;
                        default:
                            this.drawObject.AttachPropertyData.Clear();
                            foreach (var item in listShape)
                            {
                                if (item.RoiShape == null) return;
                                if (index == e.RowIndex)
                                {
                                    this.drawObject.AddViewObject(new ViewData(item.RoiShape.AffinePixROI(pixCoordSystem?.GetHomMat2D()).GetXLD(), enColor.green.ToString()));
                                }
                                else
                                {
                                    this.drawObject.AddViewObject(new ViewData(item.RoiShape.AffinePixROI(pixCoordSystem?.GetHomMat2D()).GetXLD(), enColor.orange.ToString()));
                                }
                                index++;
                            }
                            this.drawObject.DrawingGraphicObject();
                            break;
                    }
                    this.dataGridView1.Refresh();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            try
            {
                if (this.dataGridView1.Columns[e.ColumnIndex].DataPropertyName == "RoiShape")
                {
                    /////////////////////////
                    e.Value = EvaluateValue(this.dataGridView1.Rows[e.RowIndex].DataBoundItem, this.dataGridView1.Columns[e.ColumnIndex].DataPropertyName);
                    if (e.Value != null)
                        e.FormattingApplied = true;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        public string EvaluateValue(object obj, string property)
        {
            string prop = property;
            string ret = string.Empty;
            if (obj == null) return ret;
            if (property.Contains("."))
            {
                prop = property.Substring(0, property.IndexOf("."));
                System.Reflection.PropertyInfo[] props = obj.GetType().GetProperties();
                foreach (System.Reflection.PropertyInfo propa in props)
                {
                    object obja = propa.GetValue(obj, new object[] { });
                    if (obja.GetType().Name.Contains(prop))
                    {
                        ret = this.EvaluateValue(obja, property.Substring(property.IndexOf(".") + 1)); // 回调
                        break;
                    }
                }
            }
            else
            {
                System.Reflection.PropertyInfo pi = obj.GetType().GetProperty(prop);
                ret = pi?.GetValue(obj, new object[] { })?.ToString();
            }
            return ret;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                ((ImageReduce)this._function).Param.Add(new ReduceParam(new drawPixRect1(100, 100, 200, 200)));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false;
        }

    }

}

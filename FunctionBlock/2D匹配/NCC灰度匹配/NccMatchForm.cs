using Common;
using HalconDotNet;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using View;

namespace FunctionBlock
{
    public partial class NccMatchForm : Form
    {
        private Form Creatform, Findform;
        private IFunction _function;
        private VisualizeView drawObject;
        private HImage sourceImage;
        public NccMatchForm(IFunction function)
        {
            this._function = function;
            InitializeComponent();
            this.drawObject = new userDrawRect1ROI(this.hWindowControl1, false);
            new ListBoxWrapClass().InitListBox(this.listBox1, function);
            // 注册事件
            this.drawObject.GrayValueInfo += new GrayValueInfoEventHandler(GetGrayValueInfo);
            BaseFunction.ExcuteCompleted += new ExcuteCompletedEventHandler(this.DisplayObjectModel);
            ListBoxWrapClass.ItemsChangeToForm += new ItemsChangeEventHandler(listbox_AddItems);
            addContextMenu(this.模型区域dataGridView);
            addContextMenu(this.搜索区域dataGridView);
        }
        public NccMatchForm(IFunction function, TreeNode node)
        {
            this._function = function;
            InitializeComponent();
            this.drawObject = new userDrawRect1ROI(this.hWindowControl1, false);
            new ListBoxWrapClass().InitListBox(this.listBox1, node);
            // 注册事件
            this.drawObject.GrayValueInfo += new GrayValueInfoEventHandler(GetGrayValueInfo);
            BaseFunction.ExcuteCompleted += new ExcuteCompletedEventHandler(this.DisplayObjectModel);
            ListBoxWrapClass.ItemsChangeToForm += new ItemsChangeEventHandler(listbox_AddItems);
            addContextMenu(this.模型区域dataGridView);
            addContextMenu(this.搜索区域dataGridView);
            this.dataGridView1.DataSource = ((FunctionBlock.NccModelMatch)this._function).ResultInfo;
        }
        private void addContextMenu(DataGridView dataGridView)
        {
            ContextMenuStrip ContextMenuStrip1 = new ContextMenuStrip();
            // 添加右键菜单 
            ToolStripItem[] items = new ToolStripMenuItem[]
            {
                new ToolStripMenuItem("删除"),
                new ToolStripMenuItem("清空"),
            };
            ContextMenuStrip1.Name = dataGridView.Name;
            ContextMenuStrip1.Items.AddRange(items); // 在滑赋值前不能调用 
            ContextMenuStrip1.ItemClicked += new ToolStripItemClickedEventHandler(dataGridViewContextMenuStrip_ItemClicked);
            dataGridView.ContextMenuStrip = ContextMenuStrip1;
        }
        private void dataGridViewContextMenuStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            string name = e.ClickedItem.Text;
            try
            {
                ((ContextMenuStrip)sender).Close();
                switch (name)
                {
                    case "删除":
                        ((ContextMenuStrip)sender).Close();
                        switch (((ContextMenuStrip)sender).Name)
                        {
                            case "搜索区域dataGridView":
                                if (this.搜索区域dataGridView.CurrentRow == null || this.搜索区域dataGridView.CurrentRow.Index < 0) return;
                                ((FunctionBlock.NccModelMatch)this._function).DoNccModel.FindNccModelParam.SearchRegion.RemoveAt(this.搜索区域dataGridView.CurrentRow.Index);
                                break;
                            case "模型区域dataGridView":
                                if (this.模型区域dataGridView.CurrentRow == null || this.模型区域dataGridView.CurrentRow.Index < 0) return;
                                ((FunctionBlock.NccModelMatch)this._function).DoNccModel.CreateNccModelParam.TemplateRegion.RemoveAt(this.模型区域dataGridView.CurrentRow.Index);
                                break;
                        }
                        break;
                    //////////////////////////////////////
                    case "清空":
                        ((ContextMenuStrip)sender).Close();
                        switch (((ContextMenuStrip)sender).Name)
                        {
                            case "搜索区域dataGridView":
                                ((FunctionBlock.NccModelMatch)this._function).DoNccModel.FindNccModelParam.SearchRegion.Clear();
                                break;
                            case "模型区域dataGridView":
                                ((FunctionBlock.NccModelMatch)this._function).DoNccModel.CreateNccModelParam.TemplateRegion.Clear();
                                break;
                        }
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
        private void NccMatchForm_Load(object sender, EventArgs e)
        {
            BindProperty();
            //////////        
            AddForm(this.splitContainer2.Panel2, new CreateNccModelParamForm(((FunctionBlock.NccModelMatch)this._function).DoNccModel.CreateNccModelParam));
            AddForm(this.参数groupBox, new FindNccModelParamForm(((FunctionBlock.NccModelMatch)this._function).DoNccModel.FindNccModelParam));
        }



        private void BindProperty()
        {
            try
            {
                this.CShapeTypeCol.Items.Clear();
                this.CShapeTypeCol.ValueType = typeof(enShapeType);
                foreach (enShapeType item in Enum.GetValues(typeof(enShapeType)))
                {
                    this.CShapeTypeCol.Items.Add(item);
                }
                this.CModelSignCol.Items.Clear();
                this.CModelSignCol.ValueType = typeof(enModelSign);
                foreach (enModelSign item in Enum.GetValues(typeof(enModelSign)))
                {
                    this.CModelSignCol.Items.Add(item);
                }
                ////////////////////////////
                this.FShapeTypeCol.Items.Clear();
                this.FShapeTypeCol.ValueType = typeof(enShapeType);
                foreach (enShapeType item in Enum.GetValues(typeof(enShapeType)))
                {
                    this.FShapeTypeCol.Items.Add(item);
                }
                this.FModelSignCOl.Items.Clear();
                this.FModelSignCOl.ValueType = typeof(enModelSign);
                foreach (enModelSign item in Enum.GetValues(typeof(enModelSign)))
                {
                    this.FModelSignCOl.Items.Add(item);
                }
                //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                this.搜索区域dataGridView.DataSource = ((FunctionBlock.NccModelMatch)this._function).DoNccModel.FindNccModelParam.SearchRegion;
                this.模型区域dataGridView.DataSource = ((FunctionBlock.NccModelMatch)this._function).DoNccModel.CreateNccModelParam.TemplateRegion;
                this.显示条目comboBox.DataSource = Enum.GetNames(typeof(enShowItems));
                // 创建匹配参数
                this.dataGridView1.DataSource = ((FunctionBlock.NccModelMatch)this._function).ResultInfo;
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
                            this.drawObject.PointCloudModel3D.ObjectModel3D = new HObjectModel3D[] { (HObjectModel3D)e.DataContent };
                            break;
                        case "HObjectModel3D[]":
                            this.drawObject.PointCloudModel3D.ObjectModel3D = (HObjectModel3D[])e.DataContent;
                            break;
                        case "PointCloudData":
                            this.drawObject.PointCloudModel3D = (PointCloudData)e.DataContent; 
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
                            this.drawObject.ClearViewObject();
                            this.drawObject.AddViewObject(new ViewData(((XldDataClass)e.DataContent).HXldCont, "green"));
                            break;
                        case "HXLDCont":
                            this.drawObject.ClearViewObject();
                            this.drawObject.AddViewObject(new ViewData((HXLDCont)e.DataContent, "green"));
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
                object3D = ((NccModelMatch)e.Function).ImageData;
                ///////////////////////////////////////////
                if (object3D != null)
                {
                    switch (object3D.GetType().Name)
                    {
                        case "HObjectModel3D":
                            this.drawObject.PointCloudModel3D = new PointCloudData ((HObjectModel3D)object3D );
                            break;
                        case "HObjectModel3D[]":
                            this.drawObject.PointCloudModel3D = new PointCloudData((PointCloudData[])object3D);
                            break;
                        case "PointCloudData":
                            this.drawObject.PointCloudModel3D = (PointCloudData)object3D;
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
                            this.drawObject.RegionData = (RegionDataClass)object3D;
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
                        case "userPixPoint":
                            this.drawObject.AttachPropertyData.Clear();
                            this.drawObject.AttachPropertyData.Add((userPixPoint[])object3D);
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

        private void NccModelMatchForm_FormClosing(object sender, FormClosingEventArgs e)
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

        private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false;
        }



        private void 区域dataGridView_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false;
        }

        private void 设置搜索区域button_Click_1(object sender, EventArgs e)
        {
            try
            {
                ((NccModelMatch)this._function).DoNccModel.FindNccModelParam.SearchRegion.Add(new ModelParam(null));
                this.搜索区域dataGridView_CellContentClick(null, new DataGridViewCellEventArgs(0, ((NccModelMatch)this._function).DoNccModel.FindNccModelParam.SearchRegion.Count - 1));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void 设置模型区域button_Click_1(object sender, EventArgs e)
        {
            try
            {
                ((NccModelMatch)this._function).DoNccModel.CreateNccModelParam.TemplateRegion.Add(new ModelParam(null));
                this.模型区域dataGridView_CellContentClick(null, new DataGridViewCellEventArgs(0, ((NccModelMatch)this._function).DoNccModel.CreateNccModelParam.TemplateRegion.Count - 1));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void LocadImageButton_Click(object sender, EventArgs e)
        {
            try
            {
                string path = new FileOperate().OpenImage();
                if (path == null || path.Length == 0) return;
                this.sourceImage = new HImage(path);
                if (this.sourceImage != null)
                    this.drawObject.BackImage = new ImageDataClass(this.sourceImage); //, this._acqSource.Sensor.CameraParam
                else
                    throw new ArgumentException("读取的图像为空");
                ((FunctionBlock.NccModelMatch)this._function).ImageData = this.drawObject.BackImage;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void 创建模型button_Click_1(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                if (((FunctionBlock.NccModelMatch)this._function).DoNccModel.CreateNccModelsFromImage(this.drawObject.BackImage.Image))
                {
                    this.drawObject.ClearViewObject();
                    this.drawObject.AddViewObject(new ViewData(((FunctionBlock.NccModelMatch)this._function).DoNccModel.NccMatchResult.MatchCont.HXldCont, "green"));
                    //this.drawObject.XldContourData = ;
                    MessageBox.Show("模型创建成功");
                }
                this.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                MessageBox.Show(ex.ToString());
            }
        }



        private void 显示条目comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.显示条目comboBox.SelectedIndex == -1) return;
                enShowItems showItem;
                Enum.TryParse(this.显示条目comboBox.SelectedItem.ToString(), out showItem);
                switch (showItem)
                {
                    case enShowItems.输入对象:
                        if (this.sourceImage == null || !this.sourceImage.IsInitialized())
                            this.drawObject.BackImage = ((FunctionBlock.NccModelMatch)this._function).ImageData;
                        else
                            this.drawObject.BackImage = new ImageDataClass(this.sourceImage);
                        break;
                    case enShowItems.模板图像:
                        this.drawObject.BackImage = new ImageDataClass(((FunctionBlock.NccModelMatch)this._function).DoNccModel.TemplateImage);
                        break;
                    //case enShowItems.模型轮廓:
                    //    this.drawObject.XldContourData = new XldDataClass(((FunctionBlock.NccModelMatch)this._function).DoNccModel.);
                    //    break;
                    case enShowItems.搜索区域:
                        this.drawObject.BackImage = new ImageDataClass(((FunctionBlock.NccModelMatch)this._function).DoNccModel.SearchImageRegion);
                        break;
                }
            }
            catch
            {

            }
        }

        private void 模型区域dataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    int index = 0;
                    PixROI pixShape;
                    if (((NccModelMatch)this._function).DoNccModel.CreateNccModelParam.TemplateRegion == null)
                        ((NccModelMatch)this._function).DoNccModel.CreateNccModelParam.TemplateRegion = new BindingList<ModelParam>();
                    BindingList<ModelParam> listShape = ((NccModelMatch)this._function).DoNccModel.CreateNccModelParam.TemplateRegion;
                    switch (this.模型区域dataGridView.Columns[e.ColumnIndex].Name)
                    {
                        case "CTeachCol":
                            if (this.模型区域dataGridView.Rows[e.RowIndex].DataBoundItem == null)
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
                                    this.drawObject.AddViewObject(new ViewData(item.RoiShape.GetXLD(), enColor.orange.ToString()));
                                }
                                index++;
                            }
                            if (this.drawObject.BackImage == null)
                                this.drawObject.BackImage = ((NccModelMatch)this._function).ImageData;
                            if (listShape[e.RowIndex].RoiShape == null)
                                this.drawObject.SetParam(null);
                            else
                                this.drawObject.SetParam(listShape[e.RowIndex].RoiShape);
                            this.drawObject.DrawPixRoiShapeOnWindow(enColor.red, out pixShape);
                            this.drawObject.AddViewObject(new ViewData(pixShape.GetXLD(), enColor.orange.ToString()));
                            /////////////////////////////////////////////////////////
                            listShape[e.RowIndex].RoiShape = pixShape;  // 这个地方的添加不能使用变换后数据
                            //////////////////////////////////////////////////////////
                            for (int i = 0; i < this.模型区域dataGridView.Rows.Count; i++)
                            {
                                this.模型区域dataGridView.Rows[i].HeaderCell.Value = (i + 1).ToString();
                            }
                            break;
                        case "CDeletCol":
                            if (listShape == null) return;
                            this.drawObject.CurrentButton = MouseButtons.Right;
                            if (listShape.Count > e.RowIndex)
                                listShape.RemoveAt(e.RowIndex);
                            if (this.drawObject.AttachPropertyData.Count > e.RowIndex)
                                this.drawObject.AttachPropertyData.RemoveAt(e.RowIndex);
                            this.drawObject.DrawingGraphicObject();
                            for (int i = 0; i < this.模型区域dataGridView.Rows.Count; i++)
                            {
                                if (this.模型区域dataGridView.Rows.Count > i)
                                    this.模型区域dataGridView.Rows[i].HeaderCell.Value = (i + 1).ToString();
                            }
                            break;
                        default:
                            this.drawObject.AttachPropertyData.Clear();
                            foreach (var item in listShape)
                            {
                                if (item.RoiShape == null) return;
                                if (index == e.RowIndex)
                                {
                                    this.drawObject.AddViewObject(new ViewData(item.RoiShape.GetXLD(), enColor.green.ToString()));
                                }
                                else
                                {
                                    this.drawObject.AddViewObject(new ViewData(item.RoiShape.GetXLD(), enColor.orange.ToString()));
                                }
                                index++;
                            }
                            this.drawObject.DrawingGraphicObject();
                            break;
                    }
                    this.模型区域dataGridView.Refresh();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void 模型区域dataGridView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            try
            {
                if (this.模型区域dataGridView.Columns[e.ColumnIndex].DataPropertyName == "CRoiShape")
                {
                    /////////////////////////
                    e.Value = EvaluateValue(this.模型区域dataGridView.Rows[e.RowIndex].DataBoundItem, this.模型区域dataGridView.Columns[e.ColumnIndex].DataPropertyName);
                    if (e.Value != null)
                        e.FormattingApplied = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void 搜索区域dataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    int index = 0;
                    PixROI pixShape;
                    if (((NccModelMatch)this._function).DoNccModel.FindNccModelParam.SearchRegion == null)
                        ((NccModelMatch)this._function).DoNccModel.FindNccModelParam.SearchRegion = new BindingList<ModelParam>();
                    BindingList<ModelParam> listShape = ((NccModelMatch)this._function).DoNccModel.FindNccModelParam.SearchRegion;
                    switch (this.搜索区域dataGridView.Columns[e.ColumnIndex].Name)
                    {
                        case "FTeachCol":
                            if (this.搜索区域dataGridView.Rows[e.RowIndex].DataBoundItem == null)
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
                                    this.drawObject.AddViewObject(new ViewData(item.RoiShape.GetXLD(), enColor.orange.ToString()));
                                }
                                index++;
                            }
                            if (this.drawObject.BackImage == null)
                                this.drawObject.BackImage = ((ShapeModelMatch2D)this._function).ImageData;
                            if (listShape[e.RowIndex].RoiShape == null)
                                this.drawObject.SetParam(null);
                            else
                                this.drawObject.SetParam(listShape[e.RowIndex].RoiShape);
                            this.drawObject.DrawPixRoiShapeOnWindow(enColor.red, out pixShape);
                            this.drawObject.AddViewObject(new ViewData(pixShape.GetXLD(), enColor.orange.ToString()));
                            /////////////////////////////////////////////////////////
                            listShape[e.RowIndex].RoiShape = pixShape;  // 这个地方的添加不能使用变换后数据
                            //////////////////////////////////////////////////////////
                            for (int i = 0; i < this.搜索区域dataGridView.Rows.Count; i++)
                            {
                                this.搜索区域dataGridView.Rows[i].HeaderCell.Value = (i + 1).ToString();
                            }
                            break;
                        case "FDeletCol":
                            if (listShape == null) return;
                            if (listShape.Count > e.RowIndex)
                                listShape.RemoveAt(e.RowIndex);
                            if (this.drawObject.AttachPropertyData.Count > e.RowIndex)
                                this.drawObject.AttachPropertyData.RemoveAt(e.RowIndex);
                            this.drawObject.DrawingGraphicObject();
                            for (int i = 0; i < this.搜索区域dataGridView.Rows.Count; i++)
                            {
                                if (this.搜索区域dataGridView.Rows.Count > i)
                                    this.搜索区域dataGridView.Rows[i].HeaderCell.Value = (i + 1).ToString();
                            }
                            break;
                        default:
                            this.drawObject.AttachPropertyData.Clear();
                            foreach (var item in listShape)
                            {
                                if (item.RoiShape == null) return;
                                if (index == e.RowIndex)
                                {
                                    this.drawObject.AddViewObject(new ViewData(item.RoiShape.GetXLD(), enColor.green.ToString()));
                                }
                                else
                                {
                                    this.drawObject.AddViewObject(new ViewData(item.RoiShape.GetXLD(), enColor.orange.ToString()));
                                }
                                index++;
                            }
                            this.drawObject.DrawingGraphicObject();
                            break;
                    }
                    this.搜索区域dataGridView.Refresh();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void 搜索区域dataGridView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            try
            {
                if (this.搜索区域dataGridView.Columns[e.ColumnIndex].DataPropertyName == "FRoiShape")
                {
                    /////////////////////////
                    e.Value = EvaluateValue(this.搜索区域dataGridView.Rows[e.RowIndex].DataBoundItem, this.搜索区域dataGridView.Columns[e.ColumnIndex].DataPropertyName);
                    if (e.Value != null)
                        e.FormattingApplied = true;
                }
            }
            catch (Exception ex)
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

        private void 模型区域dataGridView_DataSourceChanged(object sender, EventArgs e)
        {
            try
            {
                /// 绑定数据后，必需重新排序列
                this.模型区域dataGridView.Columns[nameof(this.CModelSignCol)].DisplayIndex = 0;
                this.模型区域dataGridView.Columns[nameof(this.CShapeTypeCol)].DisplayIndex = 1;
                this.模型区域dataGridView.Columns[nameof(this.CRoiShapeCol)].DisplayIndex = 2;
                this.模型区域dataGridView.Columns[nameof(this.CTeachCol)].DisplayIndex = 3;
                this.模型区域dataGridView.Columns[nameof(this.CDeletCol)].DisplayIndex = 4;
            }
            catch (Exception ex)
            {

            }
        }

        private void 搜索区域dataGridView_DataSourceChanged(object sender, EventArgs e)
        {
            try
            {
                /// 绑定数据后，必需重新排序列
                this.搜索区域dataGridView.Columns[nameof(this.FModelSignCOl)].DisplayIndex = 0;
                this.搜索区域dataGridView.Columns[nameof(this.FShapeTypeCol)].DisplayIndex = 1;
                this.搜索区域dataGridView.Columns[nameof(this.FRoiShapeCol)].DisplayIndex = 2;
                this.搜索区域dataGridView.Columns[nameof(this.FTeachCol)].DisplayIndex = 3;
                this.搜索区域dataGridView.Columns[nameof(this.FDeletCol)].DisplayIndex = 4;
            }
            catch (Exception ex)
            {

            }
        }

        public enum enShowItems
        {
            输入对象,
            模板图像,
            模板轮廓,
            模型轮廓,
            搜索区域,
            参考点,
            匹配点
        }


    }
}

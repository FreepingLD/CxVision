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
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using View;

namespace FunctionBlock
{
    public partial class ShapeModelMatch2DAppForm : Form
    {
        private Form Creatform, Findform;
        private IFunction _function;
        private VisualizeView drawObject;
        private HImage sourceImage;
        private bool IsLoad = false;
        private TreeNode _refNode;
        private HWindowControl hWindowControl1;
        public VisualizeView DrawObject { get => drawObject; set => drawObject = value; }

        public ShapeModelMatch2DAppForm(TreeNode node)
        {
            InitializeComponent();
            this._refNode = node;
            this._function = node.Tag as IFunction;
            //initEvent(this.hWindowControl1);
            this.drawObject = new userDrawRect1ROI(this.hWindowControl1, false);
            // 注册事件
            //this.drawObject.GrayValueInfo += new GrayValueInfoEventHandler(GetGrayValueInfo);
            //BaseFunction.ExcuteCompleted += new ExcuteCompletedEventHandler(this.DisplayObjectModel);
            //ListBoxWrapClass.ItemsChangeToForm += new ItemsChangeEventHandler(listbox_AddItems);
            addContextMenu(this.模型区域dataGridView);
            addContextMenu(this.搜索区域dataGridView);
            this.drawObject.BackImage = ((FunctionBlock.ShapeModelMatch2D)this._function).ImageData;
            //int width = this.Size.Width;
            //int height = this.Size.Height;
        }

        public ShapeModelMatch2DAppForm(TreeNode node, HWindowControl hWindowControl)
        {
            InitializeComponent();
            this._refNode = node;
            this._function = node.Tag as IFunction;
            this.hWindowControl1 = hWindowControl;
            this.drawObject = new userDrawRect1ROI(this.hWindowControl1, false);
            // 注册事件
            //this.drawObject.GrayValueInfo += new GrayValueInfoEventHandler(GetGrayValueInfo);
            //BaseFunction.ExcuteCompleted += new ExcuteCompletedEventHandler(this.DisplayObjectModel);
            //ListBoxWrapClass.ItemsChangeToForm += new ItemsChangeEventHandler(listbox_AddItems);
            addContextMenu(this.模型区域dataGridView);
            addContextMenu(this.搜索区域dataGridView);
            this.drawObject.BackImage = ((FunctionBlock.ShapeModelMatch2D)this._function).ImageData;
            //int width = this.Size.Width;
            //int height = this.Size.Height;
        }

        private void addContextMenu(DataGridView dataGridView)
        {
            ContextMenuStrip ContextMenuStrip1 = new ContextMenuStrip();
            // 添加右键菜单 
            ToolStripItem[] items = null;
            switch (SystemParamManager.Instance.SysConfigParam.Language)
            {
                default:
                case "zh-CN":
                    items = new ToolStripMenuItem[]
                    {
                    //new ToolStripMenuItem("复制",null,null,"复制"),
                    new ToolStripMenuItem("删除",null,null,"删除"),
                    new ToolStripMenuItem("清空",null,null,"清空"),
                    new ToolStripMenuItem("设置形状参数",null,null,"设置形状参数"),
                    new ToolStripMenuItem("导入轮廓(dxf)",null,null,"导入轮廓(dxf)"),
                    };
                    break;
                case "en-US":
                    items = new ToolStripMenuItem[]
                    {
                    //new ToolStripMenuItem("Copy",null,null,"复制"),
                    new ToolStripMenuItem("Delete",null,null,"删除"),
                    new ToolStripMenuItem("Clear",null,null,"清空"),
                    new ToolStripMenuItem("SetShapeParam",null,null,"设置形状参数"),
                    new ToolStripMenuItem("Import Contour(dxf)",null,null,"导入轮廓(dxf)"),
                    };
                    break;
            }
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
                int index = 0;
                BindingList<ModelParam> listShape = ((FunctionBlock.ShapeModelMatch2D)this._function).ModelMatch.C_ShapeModelParam.TemplateRegion;
                ((ContextMenuStrip)sender).Close();
                switch (name)
                {
                    case "删除":
                        //((ContextMenuStrip)sender).Close();
                        switch (((ContextMenuStrip)sender).Name)
                        {
                            case "搜索区域dataGridView":
                                if (this.搜索区域dataGridView.CurrentRow == null || this.搜索区域dataGridView.CurrentRow.Index < 0) return;
                                ((FunctionBlock.ShapeModelMatch2D)this._function).ModelMatch.F_ShapeModelParam.SearchRegion.RemoveAt(this.搜索区域dataGridView.CurrentRow.Index);
                                break;
                            case "模型区域dataGridView":
                                if (this.模型区域dataGridView.CurrentRow == null || this.模型区域dataGridView.CurrentRow.Index < 0) return;
                                ((FunctionBlock.ShapeModelMatch2D)this._function).ModelMatch.C_ShapeModelParam.TemplateRegion.RemoveAt(this.模型区域dataGridView.CurrentRow.Index);
                                break;
                        }
                        break;
                    //////////////////////////////////////
                    case "清空":
                        //((ContextMenuStrip)sender).Close();
                        switch (((ContextMenuStrip)sender).Name)
                        {
                            case "搜索区域dataGridView":
                                if (this.搜索区域dataGridView.CurrentRow == null || this.搜索区域dataGridView.CurrentRow.Index < 0) return;
                                ((FunctionBlock.ShapeModelMatch2D)this._function).ModelMatch.F_ShapeModelParam.SearchRegion.Clear(); // RemoveAt(this.搜索区域dataGridView.CurrentRow.Index);
                                break;
                            case "模型区域dataGridView":
                                if (this.模型区域dataGridView.CurrentRow == null || this.模型区域dataGridView.CurrentRow.Index < 0) return;
                                ((FunctionBlock.ShapeModelMatch2D)this._function).ModelMatch.C_ShapeModelParam.TemplateRegion.Clear(); //.RemoveAt(this.模型区域dataGridView.CurrentRow.Index);
                                break;
                        }
                        break;

                    case "设置形状参数":
                        switch (((ContextMenuStrip)sender).Name)
                        {
                            case "搜索区域dataGridView":
                                // new UserMessageForm().ShowDialog("未实现的操作");
                                new Common.UserMessageForm("未实现的操作").ShowDialog();
                                break;
                            case "模型区域dataGridView":
                                if (this.模型区域dataGridView.CurrentRow != null)
                                    index = this.模型区域dataGridView.CurrentRow.Index;
                                switch (listShape[index].ShapeType)
                                {
                                    case enShapeType.矩形1:
                                        WcsRet1ParamForm ret1Param = new WcsRet1ParamForm(listShape[index].RoiShape.GetWcsROI(this.drawObject.CameraParam) as drawWcsRect1);
                                        if (ret1Param.ShowDialog() == DialogResult.OK)
                                        {
                                            drawPixRect1 pixRect1 = listShape[index].RoiShape as drawPixRect1;
                                            drawPixRect1 pixRect1New = ret1Param.WcsRect1.GetPixRect1(this.drawObject.CameraParam);
                                            pixRect1.Row1 = pixRect1New.Row1;
                                            pixRect1.Col1 = pixRect1New.Col1;
                                            pixRect1.Row2 = pixRect1New.Row2;
                                            pixRect1.Col2 = pixRect1New.Col2;
                                            /////////////////////////////////////////////
                                            userPixCoordSystem pixCoordSystem = new userPixCoordSystem();
                                            this.drawObject.ClearViewObject();
                                            this.drawObject.AddViewObject(new ViewData(listShape[index].RoiShape.AffinePixROI(pixCoordSystem?.GetHomMat2D()).GetXLD(), enColor.orange.ToString()));
                                        }
                                        break;
                                    case enShapeType.矩形2:
                                        WcsRet2ParamForm ret2Param = new WcsRet2ParamForm(listShape[index].RoiShape.GetWcsROI(this.drawObject.CameraParam) as drawWcsRect2);
                                        if (ret2Param.ShowDialog() == DialogResult.OK)
                                        {
                                            drawPixRect2 pixRect2 = listShape[index].RoiShape as drawPixRect2;
                                            drawPixRect2 pixRect2New = ret2Param.WcsRect2.GetPixRect2(this.drawObject.CameraParam);
                                            pixRect2.Row = pixRect2New.Row;
                                            pixRect2.Col = pixRect2New.Col;
                                            pixRect2.Length1 = pixRect2New.Length1;
                                            pixRect2.Length2 = pixRect2New.Length2;
                                            pixRect2.Rad = pixRect2New.Rad;
                                            /////////////////////////////////////////////
                                            userPixCoordSystem pixCoordSystem = new userPixCoordSystem();
                                            this.drawObject.ClearViewObject();
                                            this.drawObject.AddViewObject(new ViewData(listShape[index].RoiShape.AffinePixROI(pixCoordSystem?.GetHomMat2D()).GetXLD(), enColor.orange.ToString()));
                                        }
                                        break;
                                    case enShapeType.圆:
                                        WcsCircleParamForm circleParamForm = new WcsCircleParamForm(listShape[index].RoiShape.GetWcsROI(this.drawObject.CameraParam) as drawWcsCircle);
                                        if (circleParamForm.ShowDialog() == DialogResult.OK)
                                        {
                                            drawPixCircle pixCirlce = listShape[index].RoiShape as drawPixCircle;
                                            drawPixCircle pixCircleNew = circleParamForm.WcsCircle.GetPixCircle(this.drawObject.CameraParam);
                                            pixCirlce.Row = pixCircleNew.Row;
                                            pixCirlce.Col = pixCircleNew.Col;
                                            pixCirlce.Radius = pixCircleNew.Radius;
                                            /////////////////////////////////////////////
                                            userPixCoordSystem pixCoordSystem = new userPixCoordSystem();
                                            this.drawObject.ClearViewObject();
                                            this.drawObject.AddViewObject(new ViewData(listShape[index].RoiShape.AffinePixROI(pixCoordSystem?.GetHomMat2D()).GetXLD(), enColor.orange.ToString()));
                                        }
                                        break;
                                }
                                break;
                        }
                        break;

                    case "导入轮廓(dxf)":
                        switch (((ContextMenuStrip)sender).Name)
                        {
                            case "搜索区域dataGridView":
                                new Common.UserMessageForm("未实现的操作").ShowDialog();
                                //new UserMessageForm().ShowDialog("未实现的操作");
                                break;
                            case "模型区域dataGridView":
                                if (this.模型区域dataGridView.CurrentRow != null)
                                    index = this.模型区域dataGridView.CurrentRow.Index;
                                switch (listShape[index].ShapeType)
                                {
                                    case enShapeType.矩形1:

                                        break;
                                    case enShapeType.矩形2:

                                        break;
                                    case enShapeType.圆:

                                        break;
                                    case enShapeType.多边形:
                                        string path = new FileOperate().OpenFileDxf();
                                        if (path != null)
                                        {
                                            drawPixPolygon pixPolyLine = listShape[index].RoiShape as drawPixPolygon;
                                            if (pixPolyLine != null)
                                                pixPolyLine.Clear();
                                            else
                                            {
                                                listShape[index].RoiShape = new drawPixPolygon();
                                                pixPolyLine = listShape[index].RoiShape as drawPixPolygon;
                                            }
                                            HXLDCont hXLDCont = new HalconDotNet.HXLDCont();
                                            hXLDCont.ReadContourXldDxf(path, "max_approx_error", 0.01);
                                            HTuple row, col;
                                            if (hXLDCont != null && hXLDCont.IsInitialized())
                                            {
                                                int num = hXLDCont.CountObj();
                                                for (int i = 1; i <= num; i++)
                                                {
                                                    hXLDCont.SelectObj(i).GetContourXld(out row, out col);
                                                    pixPolyLine.Row.AddRange(row.DArr);
                                                    pixPolyLine.Col.AddRange(col.DArr);
                                                }
                                            }
                                            this.drawObject.ClearViewObject();
                                            this.drawObject.AddViewObject(new ViewData(listShape[index].RoiShape.GetXLD(), enColor.orange.ToString()));
                                        }
                                        break;
                                    case enShapeType.多段线:
                                        path = new FileOperate().OpenFileDxf();
                                        if (path != null)
                                        {
                                            drawPixPolyLine pixPolyLine = listShape[index].RoiShape as drawPixPolyLine;
                                            if (pixPolyLine != null)
                                                pixPolyLine.Clear();
                                            else
                                            {
                                                listShape[index].RoiShape = new drawPixPolyLine();
                                                pixPolyLine = listShape[index].RoiShape as drawPixPolyLine;
                                            }
                                            HXLDCont hXLDCont = new HalconDotNet.HXLDCont();
                                            hXLDCont.ReadContourXldDxf(path, "max_approx_error", 0.01);
                                            HTuple row, col;
                                            if (hXLDCont != null && hXLDCont.IsInitialized())
                                            {
                                                int num = hXLDCont.CountObj();
                                                for (int i = 1; i <= num; i++)
                                                {
                                                    hXLDCont.SelectObj(i).GetContourXld(out row, out col);
                                                    pixPolyLine.Row.AddRange(row.DArr);
                                                    pixPolyLine.Col.AddRange(col.DArr);
                                                }
                                            }
                                            this.drawObject.ClearViewObject();
                                            this.drawObject.AddViewObject(new ViewData(listShape[index].RoiShape.GetXLD(), enColor.orange.ToString()));
                                        }
                                        break;
                                }
                                break;
                        }
                        break;

                    ///////////////////////////////////////////////
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
            }
        }
        private void ShapeModelMatchForm_Load(object sender, EventArgs e)
        {
            //this.AutoForm();
            BindProperty();
            //////////        
            switch (((FunctionBlock.ShapeModelMatch2D)this._function).ModelMatch.ModelMacthType)
            {
                case FunctionBlock.enModelMatchType.shape_model:
                    AddForm(this.创建参数panel, new CShapeModelForm(((FunctionBlock.ShapeModelMatch2D)this._function).ModelMatch.C_ShapeModelParam));
                    AddForm(this.搜索参数groupBox, new FShapeModelParamForm(((FunctionBlock.ShapeModelMatch2D)this._function).ModelMatch.F_ShapeModelParam));  //CreateShapeModel2D.
                    break;
                case FunctionBlock.enModelMatchType.shape_model_xld:

                    AddForm(this.创建参数panel, new CShapeModelXLDForm(((FunctionBlock.ShapeModelMatch2D)this._function).ModelMatch.C_ShapeModelParam));
                    AddForm(this.搜索参数groupBox, new FShapeModelParamForm(((FunctionBlock.ShapeModelMatch2D)this._function).ModelMatch.F_ShapeModelParam));  //CreateShapeModel2D.
                    break;
                case FunctionBlock.enModelMatchType.scaled_shape_model:
                    AddForm(this.创建参数panel, new CScaledShapeModelForm(((FunctionBlock.ShapeModelMatch2D)this._function).ModelMatch.C_ShapeModelParam));
                    AddForm(this.搜索参数groupBox, new FScaledShapeModelParamForm(((FunctionBlock.ShapeModelMatch2D)this._function).ModelMatch.F_ShapeModelParam));
                    break;
                case FunctionBlock.enModelMatchType.scaled_shape_model_xld:
                    AddForm(this.创建参数panel, new CScaledShapeModelXLDForm(((FunctionBlock.ShapeModelMatch2D)this._function).ModelMatch.C_ShapeModelParam));
                    AddForm(this.搜索参数groupBox, new FScaledShapeModelParamForm(((FunctionBlock.ShapeModelMatch2D)this._function).ModelMatch.F_ShapeModelParam));
                    break;
                case FunctionBlock.enModelMatchType.aniso_shape_model:
                    AddForm(this.创建参数panel, new CAnisoShapeModelForm(((FunctionBlock.ShapeModelMatch2D)this._function).ModelMatch.C_ShapeModelParam));
                    AddForm(this.搜索参数groupBox, new FAnisoShapeModelParamForm(((FunctionBlock.ShapeModelMatch2D)this._function).ModelMatch.F_ShapeModelParam));
                    break;
                case FunctionBlock.enModelMatchType.aniso_shape_model_xld:
                    AddForm(this.创建参数panel, new CAnisoShapeModelXLDForm(((FunctionBlock.ShapeModelMatch2D)this._function).ModelMatch.C_ShapeModelParam));
                    AddForm(this.搜索参数groupBox, new FAnisoShapeModelParamForm(((FunctionBlock.ShapeModelMatch2D)this._function).ModelMatch.F_ShapeModelParam));
                    break;
            }
        }

        private void AutoForm()
        {
            try
            {
                if (this.Tag != null)
                {
                    string[] value = this.Tag.ToString().Split(',', ';', ':');
                    double width = 0, height = 0;
                    if (value.Length > 0)
                        double.TryParse(value[0], out width);
                    if (value.Length > 1)
                        double.TryParse(value[1], out height);
                    if (width > 0 && height > 0)
                    {
                        double scsleWidth = (Screen.PrimaryScreen.WorkingArea.Width * 1.0) / width;
                        double scsleHeight = (Screen.PrimaryScreen.WorkingArea.Height * 1.0) / height;
                        this.Width = (int)(this.Width * scsleWidth);
                        this.Height = (int)(this.Height * scsleHeight);
                    }
                }
            }
            catch
            { }
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
                this.模型区域dataGridView.TopLeftHeaderCell.Value = "序号";
                this.搜索区域dataGridView.TopLeftHeaderCell.Value = "序号";
                this.模型区域dataGridView.DataSource = ((FunctionBlock.ShapeModelMatch2D)this._function).ModelMatch.C_ShapeModelParam.TemplateRegion;
                this.搜索区域dataGridView.DataSource = ((FunctionBlock.ShapeModelMatch2D)this._function).ModelMatch.F_ShapeModelParam.SearchRegion;
                //this.显示条目comboBox.DataSource = Enum.GetNames(typeof(enShowItems));
                // 创建匹配参数
                this.模型创建方法comboBox.DataSource = Enum.GetNames(typeof(FunctionBlock.enModelMatchType));
                //this.匹配方式comboBox.DataSource = Enum.GetNames(typeof(FunctionBlock.enMatchMethod));
                //this.显示条目comboBox.DataSource = Enum.GetNames(typeof(ShapeMatch2D.enShowItems));
                // 创建匹配参数 
                this.模型创建方法comboBox.DataBindings.Add("Text", ((FunctionBlock.ShapeModelMatch2D)this._function).ModelMatch, "ModelMacthType", true, DataSourceUpdateMode.OnPropertyChanged);
                //this.匹配方式comboBox.DataBindings.Add("Text", ((FunctionBlock.ShapeModelMatch2D)this._function).ModelMatch, "MatchMethod", true, DataSourceUpdateMode.OnPropertyChanged);
                //this.dataGridView1.DataSource = ((FunctionBlock.DoShapeModelMatch2D)this._function).ResultDataTable;
                this.dataGridView1.DataSource = ((FunctionBlock.ShapeModelMatch2D)this._function).ResultInfo;
            }
            catch (Exception ex)
            {
                new Common.UserMessageForm(ex.ToString()).ShowDialog();
                //new UserMessageForm().ShowDialog(ex.ToString());
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
                        case "XldDataClass[]":
                            XldDataClass[] xldDataClasses = (XldDataClass[])e.DataContent;
                            foreach (var item in xldDataClasses)
                            {
                                this.drawObject.AddViewObject(new ViewData(((XldDataClass)item).HXldCont, "green"));
                            }
                            break;
                        case "HXLDCont":
                            this.drawObject.AddViewObject(new ViewData(((HXLDCont)e.DataContent), "green"));
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

        // 获取鼠标位置处的高度值
        private void GetGrayValueInfo(object sender, GrayValueInfoEventArgs e)
        {
            //if (e.GaryValue.Length > 0)
            //    this.灰度值1Label.Text = e.GaryValue[0].ToString();
            //else
            //    this.灰度值1Label.Text = 0.ToString();
            /////////////////////////////////////////////
            //if (e.GaryValue.Length > 1)
            //    this.灰度值2Label.Text = e.GaryValue[1].ToString();
            //else
            //    this.灰度值2Label.Text = 0.ToString();
            ///////////////////////////////////////////
            //if (e.GaryValue.Length > 2)
            //    this.灰度值3Label.Text = e.GaryValue[2].ToString();
            //else
            //    this.灰度值3Label.Text = 0.ToString();
            /////////////////////////////////////////////////
            //this.行坐标Label.Text = e.Row.ToString();
            //this.列坐标Label.Text = e.Col.ToString();
        }

        private void ShapeModelMatchForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (this.drawObject != null)
                    this.drawObject.ClearDrawingObject();
                // 注消事件
                //this.drawObject.GrayValueInfo -= new GrayValueInfoEventHandler(GetGrayValueInfo);
                //ListBoxWrapClass.ItemsChangeToForm -= new ItemsChangeEventHandler(listbox_AddItems);
                //BaseFunction.ExcuteCompleted -= new ExcuteCompletedEventHandler(this.DisplayObjectModel);
                //this.hWindowControl1.MouseMove -= new System.Windows.Forms.MouseEventHandler(this.hWindowControl1_MouseMove);
            }
            catch
            {

            }
        }

        private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false;
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

        private void 模型创建方法comboBox_SelectionChangeCommitted_1(object sender, EventArgs e)
        {
            if (this.模型创建方法comboBox.Text == "") return;
            FunctionBlock.enModelMatchType result;
            if (!Enum.TryParse(this.模型创建方法comboBox.SelectedItem.ToString(), out result)) return;
            if (Creatform != null) Creatform.Dispose();
            if (Findform != null) Findform.Dispose();
            ((FunctionBlock.ShapeModelMatch2D)this._function).ModelMatch.ModelMacthType = result;  // 设置匹配类型时会创建相应的参数
            switch (result)
            {
                case FunctionBlock.enModelMatchType.shape_model:
                    this.Creatform = new CShapeModelForm(((FunctionBlock.ShapeModelMatch2D)this._function).ModelMatch.C_ShapeModelParam);
                    this.Findform = new FShapeModelParamForm(((FunctionBlock.ShapeModelMatch2D)this._function).ModelMatch.F_ShapeModelParam);
                    break;
                case FunctionBlock.enModelMatchType.shape_model_xld:
                    this.Creatform = new CShapeModelXLDForm(((FunctionBlock.ShapeModelMatch2D)this._function).ModelMatch.C_ShapeModelParam);
                    this.Findform = new FShapeModelParamForm(((FunctionBlock.ShapeModelMatch2D)this._function).ModelMatch.F_ShapeModelParam);
                    break;
                case FunctionBlock.enModelMatchType.scaled_shape_model:
                    this.Creatform = new CScaledShapeModelForm(((FunctionBlock.ShapeModelMatch2D)this._function).ModelMatch.C_ShapeModelParam);
                    this.Findform = new FScaledShapeModelParamForm(((FunctionBlock.ShapeModelMatch2D)this._function).ModelMatch.F_ShapeModelParam);
                    break;
                case FunctionBlock.enModelMatchType.scaled_shape_model_xld:
                    this.Creatform = new CScaledShapeModelXLDForm(((FunctionBlock.ShapeModelMatch2D)this._function).ModelMatch.C_ShapeModelParam);
                    this.Findform = new FScaledShapeModelParamForm(((FunctionBlock.ShapeModelMatch2D)this._function).ModelMatch.F_ShapeModelParam);
                    break;
                case FunctionBlock.enModelMatchType.aniso_shape_model:
                    this.Creatform = new CAnisoShapeModelForm(((FunctionBlock.ShapeModelMatch2D)this._function).ModelMatch.C_ShapeModelParam);
                    this.Findform = new FAnisoShapeModelParamForm(((FunctionBlock.ShapeModelMatch2D)this._function).ModelMatch.F_ShapeModelParam);
                    break;
                case FunctionBlock.enModelMatchType.aniso_shape_model_xld:
                    this.Creatform = new CAnisoShapeModelXLDForm(((FunctionBlock.ShapeModelMatch2D)this._function).ModelMatch.C_ShapeModelParam);
                    this.Findform = new FAnisoShapeModelParamForm(((FunctionBlock.ShapeModelMatch2D)this._function).ModelMatch.F_ShapeModelParam);
                    break;
            }
            //////////////
            AddForm(this.创建参数panel, this.Creatform);
            AddForm(this.搜索参数groupBox, this.Findform);  //.

            /////////////////////////////////////////////////
            this.搜索区域dataGridView.DataSource = null;
            this.模型区域dataGridView.DataSource = null;
            this.搜索区域dataGridView.DataSource = ((FunctionBlock.ShapeModelMatch2D)this._function).ModelMatch.F_ShapeModelParam.SearchRegion;
            this.模型区域dataGridView.DataSource = ((FunctionBlock.ShapeModelMatch2D)this._function).ModelMatch.C_ShapeModelParam.TemplateRegion;
            this.搜索区域dataGridView.Refresh();
            this.模型区域dataGridView.Refresh();
        }

        private void 创建模型button_Click(object sender, EventArgs e)
        {
            try
            {
                switch (((FunctionBlock.ShapeModelMatch2D)this._function).ModelMatch.ModelMacthType)
                {
                    default:
                    case FunctionBlock.enModelMatchType.aniso_shape_model:
                    case FunctionBlock.enModelMatchType.scaled_shape_model:
                    case FunctionBlock.enModelMatchType.shape_model:
                        this.Cursor = Cursors.WaitCursor;
                        if (((FunctionBlock.ShapeModelMatch2D)this._function).ModelMatch.CreateShapeModelsFromImage(this.drawObject.BackImage.GetImage("byte")))
                        {
                            this.drawObject.ClearViewObject();
                            this.drawObject.AddViewObject(new ViewData(((ShapeModelMatch2D)this._function).ModelMatch.ShapeMatchResult.MatchCont.HXldCont, "green"));
                            //this.drawObject.XldContourData = ((FunctionBlock.ShapeModelMatch2D)this._function).ModelMatch.ShapeMatchResult.MatchCont;
                            //new UserMessageForm().ShowDialog("模型创建成功");
                            new Common.UserMessageForm("模型创建成功").ShowDialog();
                        }
                        else
                            new Common.UserMessageForm("模型创建成功").ShowDialog(this);
                        this.Cursor = Cursors.Default;
                        break;
                    case enModelMatchType.aniso_shape_model_xld:
                    case enModelMatchType.scaled_shape_model_xld:
                    case enModelMatchType.shape_model_xld:
                        this.Cursor = Cursors.WaitCursor;
                        if (((ShapeModelMatch2D)this._function).ModelMatch.CreateShapeModelsFromXLD(((ShapeModelMatch2D)this._function).ImageData.GetImage("byte")))
                        {
                            this.drawObject.ClearViewObject();
                            this.drawObject.AddViewObject(new ViewData(((ShapeModelMatch2D)this._function).ModelMatch.ShapeMatchResult.MatchCont.HXldCont, "green"));
                            //this.drawObject.XldContourData = ((ShapeModelMatch2D)this._function).ModelMatch.ShapeMatchResult.MatchCont;
                            //new UserMessageForm().ShowDialog("模型创建成功");
                        }
                        else
                            new Common.UserMessageForm("模型创建失败").ShowDialog();
                        //new UserMessageForm().ShowDialog("模型创建失败");
                        this.Cursor = Cursors.Default;
                        break;
                }
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                new Common.UserMessageForm("模型创建报错：" + ex.ToString()).ShowDialog();
                //new UserMessageForm().ShowDialog("模型创建报错：" + ex.ToString());
            }

        }

        private void 模型区域dataGridView_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    int index = 0;
                    PixROI pixShape;
                    if (((ShapeModelMatch2D)this._function).ModelMatch.C_ShapeModelParam.TemplateRegion == null)
                        ((ShapeModelMatch2D)this._function).ModelMatch.C_ShapeModelParam.TemplateRegion = new BindingList<ModelParam>();
                    BindingList<ModelParam> listShape = ((ShapeModelMatch2D)this._function).ModelMatch.C_ShapeModelParam.TemplateRegion;
                    switch (this.模型区域dataGridView.Columns[e.ColumnIndex].Name)
                    {
                        case "CTeachCol":
                            if (this.模型区域dataGridView.Rows[e.RowIndex].DataBoundItem == null)
                            {
                                new Common.UserMessageForm("未设置必需的图形参数，请先设置参数!").ShowDialog();
                                //new UserMessageForm().ShowDialog("未设置必需的图形参数，请先设置参数!");
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
                                case enShapeType.多段线:
                                    this.drawObject.AttachPropertyData.Clear();
                                    if (!(this.drawObject is userDrawPolyLineROI))
                                    {
                                        this.drawObject.GrayValueInfo -= new GrayValueInfoEventHandler(GetGrayValueInfo);
                                        this.drawObject.ClearDrawingObject();
                                        this.drawObject = new userDrawPolyLineROI(this.hWindowControl1, false);
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
                new Common.UserMessageForm(ex.ToString()).ShowDialog();
                // new Common.UserMessageForm(ex.ToString()).ShowDialog();
            }
        }

        private void 搜索区域dataGridView_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    int index = 0;
                    PixROI pixShape;
                    if (((ShapeModelMatch2D)this._function).ModelMatch.F_ShapeModelParam.SearchRegion == null)
                        ((ShapeModelMatch2D)this._function).ModelMatch.F_ShapeModelParam.SearchRegion = new BindingList<ModelParam>();
                    BindingList<ModelParam> listShape = ((ShapeModelMatch2D)this._function).ModelMatch.F_ShapeModelParam.SearchRegion;
                    switch (this.搜索区域dataGridView.Columns[e.ColumnIndex].Name)
                    {
                        case "FTeachCol":
                            if (this.搜索区域dataGridView.Rows[e.RowIndex].DataBoundItem == null)
                            {
                                new Common.UserMessageForm("未设置必需的图形参数，请先设置参数!").ShowDialog();
                                //new UserMessageForm().ShowDialog("未设置必需的图形参数，请先设置参数!");
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
                new Common.UserMessageForm(ex.ToString()).ShowDialog();
                //new UserMessageForm().ShowDialog(ex.ToString());
            }
        }

        private void 模型区域dataGridView_CellFormatting_1(object sender, DataGridViewCellFormattingEventArgs e)
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
                new Common.UserMessageForm(ex.ToString()).ShowDialog();
                //new UserMessageForm().ShowDialog(ex.ToString());
            }
        }

        private void 搜索区域dataGridView_CellFormatting_1(object sender, DataGridViewCellFormattingEventArgs e)
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
                new Common.UserMessageForm(ex.ToString()).ShowDialog();
                //new UserMessageForm().ShowDialog(ex.ToString());
            }
        }

        private void 模型区域dataGridView_DataSourceChanged_1(object sender, EventArgs e)
        {
            try
            {
                /// 绑定数据后，必需重新排序列
                if (this.模型区域dataGridView.Columns[nameof(this.CModelSignCol)] != null)
                    this.模型区域dataGridView.Columns[nameof(this.CModelSignCol)].DisplayIndex = 0;
                if (this.模型区域dataGridView.Columns[nameof(this.CShapeTypeCol)] != null)
                    this.模型区域dataGridView.Columns[nameof(this.CShapeTypeCol)].DisplayIndex = 1;
                if (this.模型区域dataGridView.Columns[nameof(this.CRoiShapeCol)] != null)
                    this.模型区域dataGridView.Columns[nameof(this.CRoiShapeCol)].DisplayIndex = 2;
                if (this.模型区域dataGridView.Columns[nameof(this.CTeachCol)] != null)
                    this.模型区域dataGridView.Columns[nameof(this.CTeachCol)].DisplayIndex = 3;
                if (this.模型区域dataGridView.Columns[nameof(this.CDeletCol)] != null)
                    this.模型区域dataGridView.Columns[nameof(this.CDeletCol)].DisplayIndex = 4;
            }
            catch (Exception ex)
            {

            }
        }

        private void 搜索区域dataGridView_DataSourceChanged_1(object sender, EventArgs e)
        {
            try
            {
                /// 绑定数据后，必需重新排序列
                if (this.搜索区域dataGridView.Columns[nameof(this.FModelSignCOl)] != null)
                    this.搜索区域dataGridView.Columns[nameof(this.FModelSignCOl)].DisplayIndex = 0;
                if (this.搜索区域dataGridView.Columns[nameof(this.FShapeTypeCol)] != null)
                    this.搜索区域dataGridView.Columns[nameof(this.FShapeTypeCol)].DisplayIndex = 1;
                if (this.搜索区域dataGridView.Columns[nameof(this.FRoiShapeCol)] != null)
                    this.搜索区域dataGridView.Columns[nameof(this.FRoiShapeCol)].DisplayIndex = 2;
                if (this.搜索区域dataGridView.Columns[nameof(this.FTeachCol)] != null)
                    this.搜索区域dataGridView.Columns[nameof(this.FTeachCol)].DisplayIndex = 3;
                if (this.搜索区域dataGridView.Columns[nameof(this.FDeletCol)] != null)
                    this.搜索区域dataGridView.Columns[nameof(this.FDeletCol)].DisplayIndex = 4;
            }
            catch (Exception ex)
            {

            }
        }

        private void 模型区域dataGridView_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false;
        }

        private void 搜索区域dataGridView_DataError_1(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false;
        }

        private void 设置模型区域button_Click(object sender, EventArgs e)
        {
            try
            {
                if (((ShapeModelMatch2D)this._function).ModelMatch.C_ShapeModelParam.TemplateRegion.Count == 0)
                    ((ShapeModelMatch2D)this._function).ModelMatch.C_ShapeModelParam.TemplateRegion.Add(new ModelParam(null)); //new drawPixRect1(300,300,100,100)
                this.模型区域dataGridView_CellContentClick_1(null, new DataGridViewCellEventArgs(0, 0));
            }
            catch (Exception ex)
            {
                new Common.UserMessageForm(ex.ToString()).ShowDialog();
                // new Common.UserMessageForm(ex.ToString()).ShowDialog();
            }
        }

        private void 设置搜索区域button_Click(object sender, EventArgs e)
        {
            try
            {
                if (((ShapeModelMatch2D)this._function).ModelMatch.F_ShapeModelParam.SearchRegion.Count == 0)
                    ((ShapeModelMatch2D)this._function).ModelMatch.F_ShapeModelParam.SearchRegion.Add(new ModelParam(null));
                this.搜索区域dataGridView_CellContentClick_1(null, new DataGridViewCellEventArgs(0, 0));
            }
            catch (Exception ex)
            {
                new Common.UserMessageForm(ex.ToString()).ShowDialog();
                //new UserMessageForm().ShowDialog(ex.ToString());
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

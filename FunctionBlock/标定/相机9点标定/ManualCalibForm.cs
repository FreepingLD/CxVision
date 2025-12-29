using AlgorithmsLibrary;
using Common;
using FunctionBlock;
using HalconDotNet;
using MotionControlCard;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Shapes;
using View;

namespace FunctionBlock
{
    public partial class ManualCalibForm : Form
    {
        private Form form;
        private VisualizeView drawObject;
        private ImageDataClass _imageData;
        private CameraParam _cameraParam;
        private string programPath = "VisionParam\\标定程序\\手动N点标定";
        private BindingList<ShapeParamPixROI> Param { get; set; }
        public ManualCalibForm(ImageDataClass imageData, CameraParam cameraParam)
        {
            InitializeComponent();
            this._cameraParam = cameraParam;
            this._imageData = imageData;
            this.Param = XML<BindingList<ShapeParamPixROI>>.Read(programPath + "\\" + this._cameraParam.SensorName + ".xml");
            if (Param == null)
                this.Param = new BindingList<ShapeParamPixROI>();
            this.drawObject = new VisualizeView(this.hWindowControl1, false);
            this.drawObject.BackImage = this._imageData;
        }
        private void ManualCalibForm_Load(object sender, EventArgs e)
        {
            // 注册事件
            this.drawObject.GrayValueInfo += new GrayValueInfoEventHandler(GetGrayValueInfo);
            BaseFunction.ExcuteCompleted += new ExcuteCompletedEventHandler(this.DisplayObjectModel);
            BindProperty();
            //////////////////////////////////////////////////////////
            this.addDataGridViewContextMenu(this.dataGridView1);

            ///////////////////////////////////
            this.映射dataGridView.Rows.Clear();
            this.映射dataGridView.Rows.Add(this._cameraParam.HomMat2D.c00, this._cameraParam.HomMat2D.c01, this._cameraParam.HomMat2D.c02);
            this.映射dataGridView.Rows.Add(this._cameraParam.HomMat2D.c10, this._cameraParam.HomMat2D.c11, this._cameraParam.HomMat2D.c12);
        }
        private void BindProperty()
        {
            try
            {
                this.标定轴comboBox.DataSource = Enum.GetValues(typeof(enCalibAxis));
                this.ShapeCol.Items.Clear();
                this.ShapeCol.ValueType = typeof(enShapeType);
                foreach (enShapeType item in Enum.GetValues(typeof(enShapeType)))
                {
                    this.ShapeCol.Items.Add(item);
                }
                this.dataGridView1.TopLeftHeaderCell.Value = "序号";
                this.dataGridView1.DataSource = this.Param;
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

        #region 数据视图右键菜单项
        private void addDataGridViewContextMenu(DataGridView dataGridView)
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
                    new ToolStripMenuItem("复制",null,null,"复制"),
                    new ToolStripMenuItem("设置形状参数",null,null,"设置形状参数"),
                    new ToolStripMenuItem("删除",null,null,"删除"),
                    new ToolStripMenuItem("清空",null,null,"清空"),
                    new ToolStripMenuItem("矩形阵列",null,null,"矩形阵列"),
                    new ToolStripMenuItem("圆形阵列",null,null,"圆形阵列"),
                    };
                    break;
                case "en-US":
                    items = new ToolStripMenuItem[]
                    {

                    };
                    break;

            }
            ContextMenuStrip1.Items.AddRange(items); // 在滑赋值前不能调用 
            ContextMenuStrip1.ItemClicked += new ToolStripItemClickedEventHandler(dataGridViewContextMenuStrip_ItemClicked);
            dataGridView.ContextMenuStrip = ContextMenuStrip1;
        }
        private void dataGridViewContextMenuStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            string name = e.ClickedItem.Name;
            int index = 0;
            try
            {
                BindingList<ShapeParamPixROI> listShape = this.Param;
                ((ContextMenuStrip)sender).Close();
                switch (name)
                {
                    case "复制":
                        //((ContextMenuStrip)sender).Close();
                        if (this.dataGridView1.CurrentRow != null)
                            index = this.dataGridView1.CurrentRow.Index;
                        CopyForm copyForm = new CopyForm();
                        if (copyForm.ShowDialog() == DialogResult.OK)
                        {
                            for (int i = 0; i < copyForm.Count; i++)
                            {
                                ShapeParamPixROI roi = this.Param[index];
                                ShapeParamPixROI roiParam = new ShapeParamPixROI();
                                roiParam.ShapeType = roi.ShapeType;
                                roiParam.RoiShape = roi.RoiShape.Clone();
                                this.Param.Add(roiParam);
                            }
                        }
                        break;
                    case "设置形状参数":
                        if (this.dataGridView1.CurrentRow != null)
                            index = this.dataGridView1.CurrentRow.Index;
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
                    case "删除":
                        //((ContextMenuStrip)sender).Close();
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
                        //((ContextMenuStrip)sender).Close();
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
                                            this.Invoke(new Action(() => { listShape.Add(new ShapeParamPixROI(((drawPixPoint)listShape[index].RoiShape).AffinePixPoint(hHomMat_tras))); }));
                                            break;
                                        case nameof(drawPixLine):
                                            this.Invoke(new Action(() => { listShape.Add(new ShapeParamPixROI(((drawPixLine)listShape[index].RoiShape).AffinePixLine(hHomMat_tras))); }));
                                            break;
                                        case nameof(drawPixCircle):
                                            this.Invoke(new Action(() => { listShape.Add(new ShapeParamPixROI(((drawPixCircle)listShape[index].RoiShape).AffinePixCircle(hHomMat_tras))); }));
                                            break;
                                        case nameof(drawPixEllipse):
                                            this.Invoke(new Action(() => { listShape.Add(new ShapeParamPixROI(((drawPixEllipse)listShape[index].RoiShape).AffinePixEllipse(hHomMat_tras))); }));
                                            break;
                                        case nameof(drawPixRect1):
                                            this.Invoke(new Action(() => { listShape.Add(new ShapeParamPixROI(((drawPixRect1)listShape[index].RoiShape).AffineTransPixRect1(hHomMat_tras))); }));
                                            break;
                                        case nameof(drawPixRect2):
                                            this.Invoke(new Action(() => { listShape.Add(new ShapeParamPixROI(((drawPixRect2)listShape[index].RoiShape).AffinePixRect2(hHomMat_tras))); }));
                                            break;
                                        case nameof(drawPixPolygon):
                                            this.Invoke(new Action(() => { listShape.Add(new ShapeParamPixROI(((drawPixPolygon)listShape[index].RoiShape).AffinePixPolygon(hHomMat_tras))); }));
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
                                        this.Invoke(new Action(() => { listShape.Add(new ShapeParamPixROI(((drawPixPoint)listShape[index].RoiShape).AffinePixPoint(hHomMat_Rota))); }));
                                        break;
                                    case nameof(drawPixLine):
                                        this.Invoke(new Action(() => { listShape.Add(new ShapeParamPixROI(((drawPixLine)listShape[index].RoiShape).AffinePixLine(hHomMat_Rota))); }));
                                        break;
                                    case nameof(drawPixCircle):
                                        this.Invoke(new Action(() => { listShape.Add(new ShapeParamPixROI(((drawPixCircle)listShape[index].RoiShape).AffinePixCircle(hHomMat_Rota))); }));
                                        break;
                                    case nameof(drawPixEllipse):
                                        this.Invoke(new Action(() => { listShape.Add(new ShapeParamPixROI(((drawPixEllipse)listShape[index].RoiShape).AffinePixEllipse(hHomMat_Rota))); }));
                                        break;
                                    case nameof(drawPixRect1):
                                        this.Invoke(new Action(() => { listShape.Add(new ShapeParamPixROI(((drawPixRect1)listShape[index].RoiShape).AffineTransPixRect1(hHomMat_Rota))); }));
                                        break;
                                    case nameof(drawPixRect2):
                                        this.Invoke(new Action(() => { listShape.Add(new ShapeParamPixROI(((drawPixRect2)listShape[index].RoiShape).AffinePixRect2(hHomMat_Rota))); }));
                                        break;
                                    case nameof(drawPixPolygon):
                                        this.Invoke(new Action(() => { listShape.Add(new ShapeParamPixROI(((drawPixPolygon)listShape[index].RoiShape).AffinePixPolygon(hHomMat_Rota))); }));
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
                            this._imageData = this.drawObject.BackImage;
                            this.drawObject.AttachPropertyData.Clear();
                            break;
                        case "ImageDataClass":
                            this.drawObject.ClearViewObject();
                            this.drawObject.BackImage = (ImageDataClass)e.DataContent;
                            this._imageData = this.drawObject.BackImage;
                            break;
                        case "XldDataClass":
                            this.drawObject.AddViewObject(new ViewData(((XldDataClass)e.DataContent).HXldCont, "red"));
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
                new UserMessageForm().ShowDialog("DisplayObjectModel->操作失败");
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
                    this.drawObject.AutoWindows();
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

        private void ManualCalibForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (this.drawObject != null)
                    this.drawObject.ClearDrawingObject();
                // 注消事件
                this.drawObject.GrayValueInfo -= new GrayValueInfoEventHandler(GetGrayValueInfo);
                BaseFunction.ExcuteCompleted -= new ExcuteCompletedEventHandler(this.DisplayObjectModel);
                if (!DirectoryEx.Exist(new FileInfo(this.programPath + "\\" + this._cameraParam.SensorName + ".xml").DirectoryName))
                    DirectoryEx.Create(new FileInfo(this.programPath + "\\" + this._cameraParam.SensorName + ".xml").DirectoryName);
                XML<BindingList<ShapeParamPixROI>>.Save(this.Param, this.programPath + "\\" + this._cameraParam.SensorName + ".xml");
            }
            catch
            {

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
                    if (this.Param == null)
                        this.Param = new BindingList<ShapeParamPixROI>();
                    BindingList<ShapeParamPixROI> listShape = this.Param;
                    userPixCoordSystem pixCoordSystem = new userPixCoordSystem();
                    switch (dataGridView1.Columns[e.ColumnIndex].Name)
                    {
                        case "TeachCol":
                            if (this.dataGridView1.Rows[e.RowIndex].DataBoundItem == null)
                            {
                                new UserMessageForm().ShowDialog("未设置必需的图形参数，请先设置参数!");
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
                            if (this.drawObject.BackImage == null && this._imageData == null)
                            {
                                new UserMessageForm().ShowDialog("请先加载一幅图像");
                                return;
                            }
                            else
                                this.drawObject.BackImage = this._imageData;
                            /////////////////////////////////////////////////
                            if (listShape[e.RowIndex].RoiShape == null)
                                this.drawObject.SetParam(null);
                            else
                            {
                                this.drawObject.SetParam(pixCoordSystem);
                                this.drawObject.SetParam(listShape[e.RowIndex].RoiShape.AffinePixROI(pixCoordSystem?.GetHomMat2D()));
                            }
                            this.drawObject.DrawPixRoiShapeOnWindow(enColor.red, out pixShape);
                            this.drawObject.AddViewObject(new ViewData(pixShape.AffinePixROI(pixCoordSystem?.GetHomMat2D()).GetXLD(), enColor.orange.ToString())); //这里也要做下变换
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
               new Common.UserMessageForm(ex.ToString()).ShowDialog();;
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
            catch (Exception ex)
            {
               new Common.UserMessageForm(ex.ToString()).ShowDialog();;
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


        private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false;
        }

        private void 保存Btn_Click(object sender, EventArgs e)
        {
            try
            {
                if (this._cameraParam != null)
                {
                    if (this._cameraParam.Save())
                        new UserMessageForm().ShowDialog("保存成功!");
                    else
                        new UserMessageForm().ShowDialog("保存失败!");
                }
            }
            catch (Exception ex)
            {
               new Common.UserMessageForm(ex.ToString()).ShowDialog();;
            }
        }

        private void 标定Btn_Click(object sender, EventArgs e)
        {
            try
            {
                UserHomMat2D homMat2D;
                double error = 0;
                int rowCount = (int)this.行数numericUpDown.Value;
                int colCount = (int)this.列数numericUpDown.Value;
                double offset_x = (int)this.行数numericUpDown.Value;
                double offset_y = (int)this.列数numericUpDown.Value;
                double.TryParse(this.X轴偏移_textBox.Text, out offset_x);
                double.TryParse(this.Y轴偏移_textBox.Text, out offset_y);
                double[] rows = new double[this.Param.Count];
                double[] cols = new double[this.Param.Count];
                double[] x = new double[this.Param.Count];
                double[] y = new double[this.Param.Count];
                ///////////////////////////////////////////////////////////
                for (int i = 0; i < this.Param.Count; i++)
                {
                    switch (this.Param[i].RoiShape.GetType().Name)
                    {
                        case nameof(drawPixPoint):
                            drawPixPoint pixPoint = this.Param[i].RoiShape as drawPixPoint;
                            rows[i] = pixPoint.Row;
                            cols[i] = pixPoint.Col;
                            break;
                        case nameof(drawPixCircle):
                            drawPixCircle pixCircle = this.Param[i].RoiShape as drawPixCircle;
                            rows[i] = pixCircle.Row;
                            cols[i] = pixCircle.Col;
                            break;
                        case nameof(drawPixRect2):
                            drawPixRect2 pixRect2 = this.Param[i].RoiShape as drawPixRect2;
                            rows[i] = pixRect2.Row;
                            cols[i] = pixRect2.Col;
                            break;
                        default:
                            throw new NotImplementedException();
                    }
                }
                ///////////////////////////////////////////////////////////
                int index = 0;
                for (int i = (int)((rowCount - 1) * -0.5); i <= (int)((rowCount - 1) * 0.5); i++)
                {
                    for (int j = (int)((colCount - 1) * -0.5); j <= (int)((colCount - 1) * 0.5); j++)
                    {
                        x[index] = j * offset_x;
                        y[index] = i * offset_y * -1;
                        index++;
                    }
                }
                ///////////////////////////////////////////////////////////////
                switch (this.标定轴comboBox.SelectedItem.ToString())
                {
                    case "X轴":
                        homMat2D = CalibrateMethod.Instance.NpointCalibSingleAxis(rows, cols, x, y, false, false, out error);
                        this._cameraParam.HomMat2D.c00 = homMat2D.c00;
                        break;
                    case "Y轴":
                        homMat2D = CalibrateMethod.Instance.NpointCalibSingleAxis(rows, cols, x, y, false, false, out error);
                        this._cameraParam.HomMat2D.c11 = homMat2D.c11;
                        break;
                    case "XY轴":
                        if (x.Length > 3)
                        {
                            homMat2D = CalibrateMethod.Instance.NpointCalib(rows, cols, x, y, out error); // 更新了矩阵
                            this._cameraParam.HomMat2D = homMat2D;
                        }
                        else
                            new UserMessageForm().ShowDialog("标定点数小于3，不能进行XY轴标定");
                        break;
                }
                ///////////////////////////////////
                this.映射dataGridView.Rows.Clear();
                this.映射dataGridView.Rows.Add(this._cameraParam.HomMat2D.c00, this._cameraParam.HomMat2D.c01, this._cameraParam.HomMat2D.c02);
                this.映射dataGridView.Rows.Add(this._cameraParam.HomMat2D.c10, this._cameraParam.HomMat2D.c11, this._cameraParam.HomMat2D.c12);
                if (new UserMessageForm().ShowDialog("标定完成,最大标定误差：" + error.ToString("f3") + "; 映射标定矩阵:" + this._cameraParam?.HomMat2D.ToString(), "是否更新并保存参数？") == DialogResult.OK)
                {
                    this._cameraParam?.Save();
                }
            }
            catch (Exception ex)
            {
               new Common.UserMessageForm(ex.ToString()).ShowDialog();;
            }

        }

        private void 加载图像Btn_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Filter = "bmp文件(*.bmp)|*.bmp|hobj文件(*.hobj)|*.hobj|tiff文件(*.tiff)|*.tiff|jpg文件(*.jpg)|*.jpg|jpeg文件(*.jpeg)|*.jpeg|png文件(*.png)|*.png|ras文件(*.ras)|*.ras|dxf文件(.dxf)|*.dxf|hdev文件(.hdev)|*.hdev|所有文件(*.*)|*.**";
                ofd.RestoreDirectory = false;
                ofd.FilterIndex = 0;
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    this.drawObject.BackImage = new ImageDataClass(new HImage(ofd.FileName)); //, this._acqSource.Sensor.CameraParam
                    this._imageData = this.drawObject.BackImage;
                    this.drawObject.BackImage.Tag = 1;
                }
            }
            catch (Exception ex)
            {
               new Common.UserMessageForm(ex.ToString()).ShowDialog();;
            }
        }




    }
}

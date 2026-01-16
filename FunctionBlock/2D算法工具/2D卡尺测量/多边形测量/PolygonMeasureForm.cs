using AlgorithmsLibrary;
using Common;
using FunctionBlock;
using HalconDotNet;
using MotionControlCard;
using Sensor;
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
    public partial class PolygonMeasureForm : Form
    {
        private CancellationTokenSource cts; // 提供了标准的协作式取消
        private object _objectDataModel;
        private Form form;
        private IFunction _function;
        private userDrawPolygonMeasure drawObject;
        private ImageViewForm viewForm;
        private TreeNode _refNode;
        private MetrolegyParamForm metrolegyParam;
        public PolygonMeasureForm(IFunction function)
        {
            this._function = function;
            InitializeComponent();
            this.Text = function.GetPropertyValues("名称").ToString();
            this.drawObject = new userDrawPolygonMeasure(this.hWindowControl1, ((FunctionBlock.PolygonMeasure)_function).FindPolygon.PolygonPixPosition.AffinePixPolygon(((FunctionBlock.PolygonMeasure)_function).PixCoordSystem?.GetVariationHomMat2D()), ((FunctionBlock.PolygonMeasure)_function).PixCoordSystem);
            new ListBoxWrapClass().InitListBox(this.listBox1, function);
            new ListBoxWrapClass().InitListBox(this.listBox2, function, 2);
        }
        public PolygonMeasureForm(IFunction function, TreeNode node)
        {
            this._refNode = node;
            this._function = function;
            InitializeComponent();
            this.Text = node.Text; //function.GetPropertyValues("名称").ToString();
            this.drawObject = new userDrawPolygonMeasure(this.hWindowControl1, ((FunctionBlock.PolygonMeasure)_function).FindPolygon.PolygonPixPosition.AffinePixPolygon(((FunctionBlock.PolygonMeasure)_function).PixCoordSystem?.GetVariationHomMat2D()), ((FunctionBlock.PolygonMeasure)_function).PixCoordSystem);
            new ListBoxWrapClass().InitListBox(this.listBox1, node);
            new ListBoxWrapClass().InitListBox(this.listBox2, node, 2);
        }
        private void LineMeasureForm_Load(object sender, EventArgs e)
        {
            ///////////////
            this.drawObject.BackImage = ((PolygonMeasure)_function).ImageData;
            BindProperty();
            this.metrolegyParam = new MetrolegyParamForm(this._function, this);
            this.AddForm(this.参数panel, this.metrolegyParam);
            this.drawObject.GrayValueInfo += new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
            BaseFunction.ExcuteCompleted += new ExcuteCompletedEventHandler(DisplayExcuteResult);
        }
        private void BindProperty()
        {
            try
            {
                this.相机采集源comboBox.DataSource = FunctionBlock.AcqSourceManage.Instance.GetAcqSourceName(); // SensorManage.CameraList;
                this.相机采集源comboBox.DataBindings.Add(nameof(this.相机采集源comboBox.SelectedItem), ((FunctionBlock.PolygonMeasure)this._function), "AcqSourceName", true, DataSourceUpdateMode.OnPropertyChanged);
                ///////////////////
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
        private void DisplayExcuteResult(object sender, ExcuteCompletedEventArgs e)
        {
            if (e.DataContent != null)
            {
                this.drawObject.ClearViewObject(); // 更新图像时清空
                switch (e.DataContent.GetType().Name)
                {
                    case nameof(ImageDataClass):
                        this.drawObject.BackImage = (ImageDataClass)e.DataContent;
                        break;
                    case nameof(userWcsRectangle2):
                        userWcsRectangle2 wcsRect2 = (userWcsRectangle2)e.DataContent;
                        this.drawObject.AddViewObject(new ViewData((wcsRect2).GetPixRectangle2().GetXLD(), wcsRect2.Color.ToString()));
                        break;
                    case nameof(userWcsRectangle1):
                        userWcsRectangle1 wcsRect1 = (userWcsRectangle1)e.DataContent;
                        this.drawObject.AddViewObject(new ViewData((wcsRect1).GetPixRectangle1().GetXLD(), wcsRect1.Color.ToString()));
                        break;
                    case nameof(userWcsPoint):
                        userWcsPoint wcsPoint = (userWcsPoint)e.DataContent;
                        this.drawObject.AddViewObject(new ViewData((wcsPoint).GetPixPoint(), wcsPoint.Color.ToString()));
                        break;
                    case nameof(userWcsVector):
                        userWcsVector wcsVector = (userWcsVector)e.DataContent;
                        userWcsPoint wcsPoint1 = new userWcsPoint(wcsVector.X, wcsVector.Y, wcsVector.Z, wcsVector.CamParams);
                        this.drawObject.AddViewObject(new ViewData((wcsPoint1).GetPixPoint(), wcsPoint1.Color.ToString()));
                        break;
                    case nameof(userWcsLine):
                        userWcsLine wcsLine = (userWcsLine)e.DataContent;
                        this.drawObject.AddViewObject(new ViewData((wcsLine).GetPixLine().GetXLD(), wcsLine.Color.ToString()));
                        break;
                    case nameof(userWcsCircle):
                        userWcsCircle wcsCircle = (userWcsCircle)e.DataContent;
                        this.drawObject.AddViewObject(new ViewData((wcsCircle).GetPixCircle().GetXLD(), wcsCircle.Color.ToString()));
                        break;
                    case nameof(userWcsCircleSector):
                        userWcsCircleSector wcsCircleSector = (userWcsCircleSector)e.DataContent;
                        this.drawObject.AddViewObject(new ViewData((wcsCircleSector).GetPixCircleSector().GetXLD(), wcsCircleSector.Color.ToString()));
                        break;
                    case nameof(userWcsEllipse):
                        userWcsEllipse wcsEllipse = (userWcsEllipse)e.DataContent;
                        this.drawObject.AddViewObject(new ViewData((wcsEllipse).GetPixEllipse().GetXLD(), wcsEllipse.Color.ToString()));
                        break;
                    case nameof(userWcsEllipseSector):
                        userWcsEllipseSector wcsEllipseSector = (userWcsEllipseSector)e.DataContent;
                        this.drawObject.AddViewObject(new ViewData((wcsEllipseSector).GetPixEllipseSector().GetXLD(), wcsEllipseSector.Color.ToString()));
                        break;
                    case nameof(userWcsPolyLine):
                        userWcsPolyLine wcsPolyLine = (userWcsPolyLine)e.DataContent;
                        this.drawObject.AddViewObject(new ViewData((wcsPolyLine).GetPixPolyLine().GetXLD(), wcsPolyLine.Color.ToString()));
                        break;
                    case nameof(userPixPoint):
                        this.drawObject.AddViewObject(new ViewData(((userPixPoint)e.DataContent).GetXLD(), ((userPixPoint)e.DataContent).Color.ToString()));
                        break;
                    case nameof(userPixLine):
                        this.drawObject.AddViewObject(new ViewData(((userPixLine)e.DataContent).GetXLD(), ((userPixLine)e.DataContent).Color.ToString()));
                        break;
                    case nameof(userPixCircle):
                        this.drawObject.AddViewObject(new ViewData(((userPixCircle)e.DataContent).GetXLD(), ((userPixCircle)e.DataContent).Color.ToString()));
                        break;
                    case nameof(userPixCircleSector):
                        this.drawObject.AddViewObject(new ViewData(((userPixCircleSector)e.DataContent).GetXLD(), ((userPixCircleSector)e.DataContent).Color.ToString()));
                        break;
                    case nameof(userPixEllipse):
                        this.drawObject.AddViewObject(new ViewData(((userPixEllipse)e.DataContent).GetXLD(), ((userPixEllipse)e.DataContent).Color.ToString()));
                        break;
                    case nameof(userPixEllipseSector):
                        this.drawObject.AddViewObject(new ViewData(((userPixEllipseSector)e.DataContent).GetXLD(), ((userPixEllipseSector)e.DataContent).Color.ToString()));
                        break;
                    case nameof(userPixRectangle1):
                        this.drawObject.AddViewObject(new ViewData(((userPixRectangle1)e.DataContent).GetXLD(), ((userPixRectangle1)e.DataContent).Color.ToString()));
                        break;
                    case nameof(userPixRectangle2):
                        this.drawObject.AddViewObject(new ViewData(((userPixRectangle2)e.DataContent).GetXLD(), ((userPixRectangle2)e.DataContent).Color.ToString()));
                        break;
                    case nameof(userOkNgText):
                        this.drawObject.AddViewObject(new ViewData(e.DataContent, "red"));
                        break;
                    case nameof(userTextLable):
                        this.drawObject.AddViewObject(new ViewData(e.DataContent, "red"));
                        break;
                    default:
                        this.drawObject.AddViewObject(new ViewData(e.DataContent, "red"));
                        break;
                }
            }
        }
        private void MetrolegyCompleted(MetrolegyCompletedEventArgs e)
        {
            if (e.EdgeData == null) return;
            switch (e.EdgeData.GetType().Name)
            {
                case "userWcsRectangle2":
                    this.drawObject.AttachPropertyData.Clear();
                    userWcsRectangle2 wcsRect2 = (userWcsRectangle2)e.EdgeData;
                    this.drawObject.AttachPropertyData.Add(wcsRect2);
                    if (e.EdgePoint_xyz != null)
                    {
                        for (int i = 0; i < e.EdgePoint_xyz.Length; i++)
                        {
                            this.drawObject.AttachPropertyData.Add(e.EdgePoint_xyz[i]);
                        }
                    }
                    this.drawObject.DetachDrawingObjectFromWindow();
                    break;
                case "userWcsRectangle1":
                    this.drawObject.AttachPropertyData.Clear();
                    userWcsRectangle1 wcsRect1 = (userWcsRectangle1)e.EdgeData;
                    this.drawObject.AttachPropertyData.Add(wcsRect1);
                    for (int i = 0; i < e.EdgePoint_xyz.Length; i++)
                    {
                        this.drawObject.AttachPropertyData.Add(e.EdgePoint_xyz[i]);
                    }
                    this.drawObject.DetachDrawingObjectFromWindow();
                    break;
                case "userWcsPoint":
                    this.drawObject.AttachPropertyData.Clear();
                    userWcsPoint wcsPoint = (userWcsPoint)e.EdgeData;
                    this.drawObject.AttachPropertyData.Add(wcsPoint);
                    if (e.EdgePoint_xyz != null)
                    {
                        for (int i = 0; i < e.EdgePoint_xyz.Length; i++)
                        {
                            this.drawObject.AttachPropertyData.Add(e.EdgePoint_xyz[i]);
                        }
                    }
                    this.drawObject.DetachDrawingObjectFromWindow();
                    break;
                case "userWcsLine":
                    this.drawObject.AttachPropertyData.Clear();
                    userWcsLine wcsLine = (userWcsLine)e.EdgeData;
                    this.drawObject.AttachPropertyData.Add(wcsLine);
                    if (e.EdgePoint_xyz != null)
                    {
                        for (int i = 0; i < e.EdgePoint_xyz.Length; i++)
                        {
                            this.drawObject.AttachPropertyData.Add(e.EdgePoint_xyz[i]);
                        }
                    }
                    this.drawObject.DetachDrawingObjectFromWindow();
                    break;
                case "userWcsCircle":
                    this.drawObject.AttachPropertyData.Clear();
                    userWcsCircle wcsCircle = (userWcsCircle)e.EdgeData;
                    this.drawObject.AttachPropertyData.Add(wcsCircle);
                    if (e.EdgePoint_xyz != null)
                    {
                        for (int i = 0; i < e.EdgePoint_xyz.Length; i++)
                        {
                            this.drawObject.AttachPropertyData.Add(e.EdgePoint_xyz[i]);
                        }
                    }
                    this.drawObject.DetachDrawingObjectFromWindow();
                    break;
                case "userWcsCircleSector":
                    this.drawObject.AttachPropertyData.Clear();
                    userWcsCircleSector wcsCircleSector = (userWcsCircleSector)e.EdgeData;
                    this.drawObject.AttachPropertyData.Add(wcsCircleSector);
                    if (e.EdgePoint_xyz != null)
                    {
                        for (int i = 0; i < e.EdgePoint_xyz.Length; i++)
                        {
                            this.drawObject.AttachPropertyData.Add(e.EdgePoint_xyz[i]);
                        }
                    }
                    this.drawObject.DetachDrawingObjectFromWindow();
                    break;
                case "userWcsEllipse":
                    this.drawObject.AttachPropertyData.Clear();
                    userWcsEllipse wcsEllipse = (userWcsEllipse)e.EdgeData;
                    this.drawObject.AttachPropertyData.Add(wcsEllipse);
                    if (e.EdgePoint_xyz != null)
                    {
                        for (int i = 0; i < e.EdgePoint_xyz.Length; i++)
                        {
                            this.drawObject.AttachPropertyData.Add(e.EdgePoint_xyz[i]);
                        }
                    }
                    this.drawObject.DetachDrawingObjectFromWindow();
                    break;
                case "userWcsEllipseSector":
                    this.drawObject.AttachPropertyData.Clear();
                    userWcsEllipseSector wcsEllipseSector = (userWcsEllipseSector)e.EdgeData;
                    this.drawObject.AttachPropertyData.Add(wcsEllipseSector);
                    if (e.EdgePoint_xyz != null)
                    {
                        for (int i = 0; i < e.EdgePoint_xyz.Length; i++)
                        {
                            this.drawObject.AttachPropertyData.Add(e.EdgePoint_xyz[i]);
                        }
                    }
                    this.drawObject.DetachDrawingObjectFromWindow();
                    break;
                case nameof(userWcsPolyLine):
                    this.drawObject.AttachPropertyData.Clear();
                    userWcsPolyLine wcsPolyLine  = (userWcsPolyLine)e.EdgeData;
                    this.drawObject.AttachPropertyData.Add(wcsPolyLine);
                    if (e.EdgePoint_xyz != null)
                    {
                        for (int i = 0; i < e.EdgePoint_xyz.Length; i++)
                        {
                            this.drawObject.AttachPropertyData.Add(e.EdgePoint_xyz[i]);
                        }
                    }
                    this.drawObject.DetachDrawingObjectFromWindow();
                    break;
            }
        }
        private void UpdataMeasureRegion(object send, EventArgs e)
        {
            this.drawObject.DrawingGraphicObject();
        }
        public void 运行toolStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            try
            {
                switch (e.ClickedItem.Name)
                {
                    case nameof(this.toolStripButton_Run):
                    case "执行":
                        if (this.toolStripStatusLabel2.Text == "等待……") break;
                        this.toolStripStatusLabel2.Text = "等待……";
                        this.toolStripStatusLabel2.ForeColor = Color.Yellow;
                        Task.Run(() =>
                        {
                            if (this._function.Execute(this.drawObject.GetPixPolygonParam(), this._refNode).Succss)
                            {
                                this.Invoke(new Action(() =>
                                {
                                    this.toolStripStatusLabel1.Text = "执行结果:";
                                    this.toolStripStatusLabel2.Text = "成功";
                                    this.toolStripStatusLabel2.ForeColor = Color.Green;
                                    this.drawObject.DetachDrawingObjectFromWindow(); 
                                    this.drawObject.AddViewObject(new ViewData(((FunctionBlock.PolygonMeasure)this._function).WcsPolygon.GetPixPolygon().GetXLD()));
                                }));
                            }
                            else
                            {
                                this.Invoke(new Action(() =>
                                {
                                    this.toolStripStatusLabel1.Text = "执行结果:";
                                    this.toolStripStatusLabel2.Text = "失败";
                                    this.toolStripStatusLabel2.ForeColor = Color.Red;
                                    this.drawObject.DetachDrawingObjectFromWindow();
                                    this.drawObject.ClearViewObject();
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
                //new UserMessageForm().ShowDialog(ex.ToString());
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
            this.行坐标Label.Text = e.Row.ToString();
            this.列坐标Label.Text = e.Col.ToString();
        }

        public void 示教点位button_Click(object sender, EventArgs e)
        {
            try
            {
                this.drawObject?.AttachPropertyData.Clear();
                this.drawObject?.ClearWindow();
                this.drawObject?.SetParam(((PolygonMeasure)this._refNode.Tag).PixCoordSystem);
                this.drawObject?.SetParam(((PolygonMeasure)this._refNode.Tag).FindPolygon.PolygonPixPosition.AffinePixPolygon(((PolygonMeasure)this._refNode.Tag).PixCoordSystem?.GetVariationHomMat2D()));
                this.drawObject.BackImage = ((PolygonMeasure)this._refNode.Tag).ImageData != null ? ((PolygonMeasure)this._refNode.Tag).ImageData : null;
                this.drawObject?.AttachDrawingObjectToWindow();
            }
            catch (Exception ex)
            {
                new Common.UserMessageForm("示教失败！" + ex.ToString()).ShowDialog();
                //new UserMessageForm().ShowDialog("示教失败！" + ex.ToString());
            }
        }

        private void FindLineForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (this.drawObject != null)
                    this.drawObject.ClearDrawingObject();
                if (cts != null)
                    cts.Cancel();
                this.metrolegyParam?.Close();
                this.drawObject.GrayValueInfo -= new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                BaseFunction.ExcuteCompleted -= new ExcuteCompletedEventHandler(DisplayExcuteResult);
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

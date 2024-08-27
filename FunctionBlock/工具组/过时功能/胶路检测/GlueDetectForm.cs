
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
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using View;

namespace FunctionBlock
{
    public partial class GlueDetectForm : Form
    {
        private CancellationTokenSource cts;
        private IFunction _function;
        private IFunction _currFunction;
        private DrawingBaseMeasure drawObject;
        private TreeViewWrapClass treeViewWrapClass;
        private GlueDetect _FeatureLocalization;
        private Dictionary<string, object> listData = new Dictionary<string, object>();
        private MetrolegyParamForm metrolegyParamForm;
        private TreeNode _refNode;
        private ImageDataClass CurrentImageData;

        public GlueDetectForm(IFunction function)
        {
            this._function = function;
            InitializeComponent();
            this.drawObject = new DrawingBaseMeasure(this.hWindowControl1, true);
            this.treeViewWrapClass = new TreeViewWrapClass(this.treeView1, this);
            this._FeatureLocalization = (GlueDetect)function;
        }
        public GlueDetectForm(TreeNode node)
        {
            this._refNode = node;
            this._function = (IFunction)node.Tag;
            InitializeComponent();
            this.drawObject = new DrawingBaseMeasure(this.hWindowControl1, true);
            this.treeViewWrapClass = new TreeViewWrapClass(this.treeView1, this);
            this._FeatureLocalization = (GlueDetect)this._function;
            this.Text = node.Text;
            int count = GlobalProgram.ProgramItems.Count;
            this.treeViewWrapClass.ToolName = this._refNode.Name.Replace(".Tool", "");
        }
        public GlueDetectForm(TreeNode node,ImageDataClass imageData)
        {
            this._refNode = node;
            this._function = (IFunction)node.Tag;
            this.CurrentImageData = imageData;
            InitializeComponent();
            this.drawObject = new DrawingBaseMeasure(this.hWindowControl1, true);
            this.treeViewWrapClass = new TreeViewWrapClass(this.treeView1, this);
            this._FeatureLocalization = (GlueDetect)this._function;
            this.Text = "胶路检测";
            int count = GlobalProgram.ProgramItems.Count;
        }
        private void GlueDetectForm_Load(object sender, EventArgs e)
        {
            // 注册事件
            this.drawObject.GrayValueInfo += new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
            BaseFunction.ExcuteCompleted += new ExcuteCompletedEventHandler(DisplayExcuteResult);
            TreeViewWrapClass.ClickNode += new ClickNodeEventHandler(this.DisplayClickObject);
            this.metrolegyParamForm = new MetrolegyParamForm(this.drawObject);
            this.addContextMenu(this.hWindowControl1);
            this.LoadTreeNode();//
            this.treeView1.ShowPlusMinus = false;
            this.treeView1.ShowRootLines = false;
        }

        public void LoadTreeNode()
        {
            foreach (TreeNode item in this._refNode.Nodes)
            {
                this.treeView1.Nodes.Add(item.Clone() as TreeNode);// 
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


        // 获取鼠标位置处的高度值

        private void GlueDetectForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                /// 转移节点到主面板上来
                this._refNode?.Nodes.Clear();
                foreach (TreeNode item1 in this.treeView1.Nodes)
                {
                    //if(!this._refNode.Nodes.ContainsKey(item1.Name))
                    this._refNode?.Nodes.Add(item1.Clone() as TreeNode);
                }
                //((FeatureLocalization)_refNode.Tag).ParentNode = _refNode;
                /////////////
                //this.treeViewWrapClass.ClearTreeView();
                this.treeView1.Nodes.Clear();
                this.treeView1.Dispose();
                this.drawObject.ClearDrawingObject();
                this.drawObject.GrayValueInfo -= new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                BaseFunction.ExcuteCompleted -= new ExcuteCompletedEventHandler(DisplayExcuteResult);
                TreeViewWrapClass.ClickNode -= new ClickNodeEventHandler(this.DisplayClickObject);
                this.treeViewWrapClass?.Uinit();
            }
            catch
            {

            }

        }

        private void 运行工具条toolStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            ToolStripItem item = e.ClickedItem;
            string name = item.Name;
            //int count = 1;
            switch (name)
            {               
                case "运行toolStripButton":
                    //this.运行toolStripButton.Enabled = false;
                    //this.停止toolStripButton.Enabled = true;
                    //this.treeViewWrapClass.RunSyn(this.运行toolStripButton, 1);
                    //this.停止toolStripButton.Enabled = true;
                    //this.运行toolStripButton.Enabled = true;
                    if (this.toolStripStatusLabel2.Text == "等待……") break;
                    this.toolStripStatusLabel2.Text = "等待……";
                    this.toolStripStatusLabel2.ForeColor = Color.Yellow;
                    this.cts = new CancellationTokenSource();
                    Task.Run(() =>
                    {
                        if (this._function.Execute(this._refNode).Succss)
                        {
                            if (!this.cts.IsCancellationRequested)
                            {
                                this.Invoke(new Action(() =>
                                {
                                    //this.toolStripStatusLabel1.Text = "执行结果:";
                                    this.toolStripStatusLabel2.Text = "成功";
                                    this.toolStripStatusLabel2.ForeColor = Color.Green;
                                }));
                            }
                        }
                        else
                        {
                            if (!this.cts.IsCancellationRequested)
                            {
                                this.Invoke(new Action(() =>
                                {
                                    //this.toolStripStatusLabel1.Text = "执行结果:";
                                    this.toolStripStatusLabel2.Text = "失败";
                                    this.toolStripStatusLabel2.ForeColor = Color.Red;
                                }));
                            }
                        }
                    }
                    );

                    break;

                case "停止toolStripButton":
                    this.treeViewWrapClass.Stop();
                    this.运行toolStripButton.Enabled = true;
                    this.停止toolStripButton.Enabled = false;
                    break;

                case "检测工具toolStripButton":
                    ToolForm tool = new ToolForm(this.treeViewWrapClass, this._refNode.Name.Replace(".Tool", ""));
                    tool.Owner = this;
                    tool.Show();
                    break;

                case "保存配置toolStripButton":
                    this._refNode?.Nodes.Clear();
                    foreach (TreeNode item1 in this.treeView1.Nodes)
                    {
                        this._refNode?.Nodes.Add(item1.Clone() as TreeNode);
                        //if (this._refNode.Nodes.Contains(item1))
                        //    this._refNode?.Nodes.Add(item1);
                    }
                    //((FeatureLocalization)_refNode.Tag).ParentNode = _refNode;
                    MessageBox.Show("保存成功");
                    break;
                //////////////////////////////////////
                default:
                    break;
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
        private bool IsSelect()
        {
            bool result = false;
            Control container = this.Parent;
            if (container == null) return result;
            switch (container.GetType().Name)
            {
                case nameof(TabPage):
                    this.Invoke(new Action(() =>
                    {
                        TabControl tabControl = ((TabPage)container).Parent as TabControl;
                        if (tabControl.SelectedTab != ((TabPage)container))
                            result = false;
                        else
                            result = true;
                    }));
                    break;
            }
            return result;
        }
        public void DisplayExcuteResult(object sender, ExcuteCompletedEventArgs e)  // 这里一定要加一个键值对变量来收集，以免出现重合 ImageAcqCompleteEventArgs e
        {
            try
            {
                //if (!IsSelect()) return; // 如果不是当前选择的，则返回
                if (e.DataContent == null) return;// 在图形窗口只显示世界坐标元素，像素元素在图像窗口显示
                /////////////////////////////////////////////
                switch (e.DataContent.GetType().Name) //这里只接受XLD轮廓或3D对象轮廓
                {
                    case "ImageDataClass":
                        this.listData.Clear();
                        this.drawObject.BackImage = (ImageDataClass)e.DataContent;
                        this.CurrentImageData = this.drawObject.BackImage;
                        break;
                    case nameof(RegionDataClass):
                        if (this.listData.ContainsKey(e.ItemName))
                            this.listData[e.ItemName] = ((RegionDataClass)e.DataContent).Region;
                        else
                            this.listData.Add(e.ItemName, ((RegionDataClass)e.DataContent).Region);
                        break;
                    case nameof(XldDataClass):
                        if (this.listData.ContainsKey(e.ItemName))
                            this.listData[e.ItemName] = ((XldDataClass)e.DataContent);
                        else
                            this.listData.Add(e.ItemName, ((XldDataClass)e.DataContent));
                        break;
                    case "HXLDCont":
                        if (this.listData.ContainsKey(e.ItemName))
                            this.listData[e.ItemName] = (HXLDCont)e.DataContent;
                        else
                            this.listData.Add(e.ItemName, (HXLDCont)e.DataContent); // = new XldDataClass((HXLDCont)e.DataContent);
                        break;
                    case "userWcsCircle":
                        if (this.listData.ContainsKey(e.ItemName))
                            this.listData[e.ItemName] = ((userWcsCircle)e.DataContent).GetPixCircle().GetAllXLD();
                        else
                            this.listData.Add(e.ItemName, ((userWcsCircle)e.DataContent).GetPixCircle().GetAllXLD()); //(userWcsCircle)e.DataContent
                        break;
                    case "userWcsCircleSector":
                        if (this.listData.ContainsKey(e.ItemName))
                            this.listData[e.ItemName] = ((userWcsCircleSector)e.DataContent).GetPixCircleSector().GetAllXLD();
                        else
                            this.listData.Add(e.ItemName, ((userWcsCircleSector)e.DataContent).GetPixCircleSector().GetAllXLD());
                        break;
                    case "userWcsEllipse":
                        if (this.listData.ContainsKey(e.ItemName))
                            this.listData[e.ItemName] = ((userWcsEllipse)e.DataContent).GetPixEllipse().GetAllXLD();
                        else
                            this.listData.Add(e.ItemName, ((userWcsEllipse)e.DataContent).GetPixEllipse().GetAllXLD());
                        break;
                    case "userWcsEllipseSector":
                        if (this.listData.ContainsKey(e.ItemName))
                            this.listData[e.ItemName] = ((userWcsEllipseSector)e.DataContent).GetPixEllipseSector().GetAllXLD();
                        else
                            this.listData.Add(e.ItemName, ((userWcsEllipseSector)e.DataContent).GetPixEllipseSector().GetAllXLD());
                        break;
                    case "userWcsLine":
                        if (this.listData.ContainsKey(e.ItemName))
                            this.listData[e.ItemName] = ((userWcsLine)e.DataContent).GetPixLine().GetAllXLD();
                        else
                            this.listData.Add(e.ItemName, ((userWcsLine)e.DataContent).GetPixLine().GetAllXLD());
                        break;

                    case "userWcsPoint":
                        if (this.listData.ContainsKey(e.ItemName))
                            this.listData[e.ItemName] = ((userWcsPoint)e.DataContent).GetPixPoint();
                        else
                            this.listData.Add(e.ItemName, ((userWcsPoint)e.DataContent).GetPixPoint()); // 点对象本身就是一个点，所以这里不再考虑显示子元素
                        break;
                    case "userWcsRectangle1":
                        if (this.listData.ContainsKey(e.ItemName))
                            this.listData[e.ItemName] = ((userWcsRectangle1)e.DataContent).GetPixRectangle1().GetAllXLD();
                        else
                            this.listData.Add(e.ItemName, ((userWcsRectangle1)e.DataContent).GetPixRectangle1().GetAllXLD());
                        break;
                    case "userWcsRectangle2":
                        if (this.listData.ContainsKey(e.ItemName))
                            this.listData[e.ItemName] = ((userWcsRectangle2)e.DataContent).GetPixRectangle2().GetAllXLD();
                        else
                            this.listData.Add(e.ItemName, ((userWcsRectangle2)e.DataContent).GetPixRectangle2().GetAllXLD());
                        break;
                    case "userWcsRectangle2[]":
                        foreach (var item in (userWcsRectangle2[])e.DataContent)
                        {
                            if (this.listData.ContainsKey(e.ItemName))
                                this.listData[e.ItemName] = item.GetPixRectangle2().GetAllXLD();
                            else
                                this.listData.Add(e.ItemName, item.GetPixRectangle2().GetAllXLD());
                        }
                        break;
                    case "userWcsCoordSystem":
                        if (this.listData.ContainsKey(e.ItemName))
                            this.listData[e.ItemName] = e.DataContent;
                        else
                            this.listData.Add(e.ItemName, e.DataContent);
                        break;
                    case "userOkNgText":
                        if (this.listData.ContainsKey(e.ItemName))
                            this.listData[e.ItemName] = e.DataContent;
                        else
                            this.listData.Add(e.ItemName, e.DataContent);
                        break;
                }
                /////////////////////////////
                Task.Run(() =>
                {
                    this.drawObject.AttachPropertyData.Clear();
                    //this.drawObject.IsDispalyAttachDrawingProperty = false;
                    foreach (KeyValuePair<string, object> item in this.listData)
                    {
                        this.drawObject.AttachPropertyData.Add(item.Value);
                    }
                    this.drawObject.DetachDrawingObjectFromWindow();
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        public void DisplayClickObject(object sender, TreeNodeMouseClickEventArgs e)  //
        {
            //if (!IsSelect()) return; // 如果不是当前选择的，则返回
            if (e.Node.Tag == null) return;
            if (e.Button != MouseButtons.Left) return; // 点击右键时不变
            try
            {
                switch (e.Node.Tag.GetType().Name)
                {
                    case "CircleMeasure":
                        this.drawObject?.AttachPropertyData.Clear();
                        if (!(this.drawObject is userDrawCircleMeasure))
                        {
                            this.drawObject.GrayValueInfo -= new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                            this.drawObject.ClearDrawingObject();
                            this.drawObject = new userDrawCircleMeasure(this.hWindowControl1, ((CircleMeasure)e.Node.Tag).FindCircle.CirclePixPosition, ((CircleMeasure)e.Node.Tag).PixCoordSystem);
                            this.drawObject.GrayValueInfo += new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                        }
                        this.drawObject.AttachPropertyData.Clear();
                        foreach (KeyValuePair<string, object> item in this.listData)
                        {
                            if (item.Key != e.Node.Text)
                                this.drawObject.AttachPropertyData.Add(item.Value);
                        }
                        this.drawObject.SetParam(((CircleMeasure)e.Node.Tag).PixCoordSystem);
                        this.drawObject.SetParam(((CircleMeasure)e.Node.Tag).FindCircle.CirclePixPosition.AffineTransPixCircle(((CircleMeasure)e.Node.Tag).PixCoordSystem.GetVariationHomMat2D()));
                        this.drawObject.BackImage = ((CircleMeasure)e.Node.Tag).ImageData != null ? ((CircleMeasure)e.Node.Tag).ImageData : this.CurrentImageData;
                        this.drawObject.AttachDrawingObjectToWindow();
                        this.metrolegyParamForm.drawObject = this.drawObject;
                        this._currFunction = (CircleMeasure)e.Node.Tag;
                        //DisplayClickItem(sender, e);
                        break;
                    case "CircleSectorMeasure":
                        this.drawObject?.AttachPropertyData.Clear();
                        if (!(this.drawObject is userDrawCircleSectorMeasure))
                        {
                            this.drawObject.GrayValueInfo -= new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                            this.drawObject.ClearDrawingObject();
                            this.drawObject = new userDrawCircleSectorMeasure(this.hWindowControl1, ((CircleSectorMeasure)e.Node.Tag).FindCircleSector.CircleSectorPixPosition, ((CircleSectorMeasure)e.Node.Tag).PixCoordSystem);
                            this.drawObject.GrayValueInfo += new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                        }
                        this.drawObject.AttachPropertyData.Clear();
                        foreach (KeyValuePair<string, object> item in this.listData)
                        {
                            if (item.Key != e.Node.Text)
                                this.drawObject.AttachPropertyData.Add(item.Value);
                        }
                        this.drawObject.SetParam(((CircleSectorMeasure)e.Node.Tag).PixCoordSystem);
                        this.drawObject.SetParam(((CircleSectorMeasure)e.Node.Tag).FindCircleSector.CircleSectorPixPosition.AffineTransPixCircleSector(((CircleSectorMeasure)e.Node.Tag).PixCoordSystem.GetVariationHomMat2D()));
                        this.drawObject.BackImage = ((CircleSectorMeasure)e.Node.Tag).ImageData != null ? ((CircleSectorMeasure)e.Node.Tag).ImageData : this.CurrentImageData;
                        this.drawObject.AttachDrawingObjectToWindow();
                        this.metrolegyParamForm.drawObject = this.drawObject;
                        this._currFunction = (CircleSectorMeasure)e.Node.Tag;
                        //DisplayClickItem(sender, e);
                        break;
                    case "EllipseMeasure":
                        this.drawObject?.AttachPropertyData.Clear();
                        if (!(this.drawObject is userDrawEllipseMeasure))
                        {
                            this.drawObject.GrayValueInfo -= new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                            this.drawObject.ClearDrawingObject();
                            this.drawObject = new userDrawEllipseMeasure(this.hWindowControl1, ((EllipseMeasure)e.Node.Tag).FindEllipse.EllipsePixPosition, ((EllipseMeasure)e.Node.Tag).PixCoordSystem);
                            this.drawObject.GrayValueInfo += new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                        }
                        this.drawObject.AttachPropertyData.Clear();
                        foreach (KeyValuePair<string, object> item in this.listData)
                        {
                            if (item.Key != e.Node.Text)
                                this.drawObject.AttachPropertyData.Add(item.Value);
                        }
                        this.drawObject.SetParam(((EllipseMeasure)e.Node.Tag).PixCoordSystem);
                        this.drawObject.SetParam(((EllipseMeasure)e.Node.Tag).FindEllipse.EllipsePixPosition.AffineTransPixEllipse(((EllipseMeasure)e.Node.Tag).PixCoordSystem.GetVariationHomMat2D()));
                        this.drawObject.BackImage = ((EllipseMeasure)e.Node.Tag).ImageData != null ? ((EllipseMeasure)e.Node.Tag).ImageData : this.CurrentImageData;
                        this.drawObject.AttachDrawingObjectToWindow();
                        this.metrolegyParamForm.drawObject = this.drawObject;
                        this._currFunction = (EllipseMeasure)e.Node.Tag;
                        //DisplayClickItem(sender, e);
                        break;
                    case "EllipseSectorMeasure":
                        this.drawObject?.AttachPropertyData.Clear();
                        if (!(this.drawObject is userDrawEllipseSectorMeasure))
                        {
                            this.drawObject.GrayValueInfo -= new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                            this.drawObject.ClearDrawingObject();
                            this.drawObject = new userDrawEllipseSectorMeasure(this.hWindowControl1, ((EllipseSectorMeasure)e.Node.Tag).FindEllipseSector.EllipseSectorPixPosition, ((EllipseSectorMeasure)e.Node.Tag).PixCoordSystem);
                            this.drawObject.GrayValueInfo += new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                        }
                        this.drawObject.AttachPropertyData.Clear();
                        foreach (KeyValuePair<string, object> item in this.listData)
                        {
                            if (item.Key != e.Node.Text)
                                this.drawObject.AttachPropertyData.Add(item.Value);
                        }
                        this.drawObject.SetParam(((EllipseSectorMeasure)e.Node.Tag).PixCoordSystem);
                        this.drawObject.SetParam(((EllipseSectorMeasure)e.Node.Tag).FindEllipseSector.EllipseSectorPixPosition.AffineTransPixEllipseSector(((EllipseSectorMeasure)e.Node.Tag).PixCoordSystem.GetVariationHomMat2D()));
                        this.drawObject.BackImage = ((EllipseSectorMeasure)e.Node.Tag).ImageData != null ? ((EllipseSectorMeasure)e.Node.Tag).ImageData : this.CurrentImageData;
                        this.drawObject.AttachDrawingObjectToWindow();
                        this.metrolegyParamForm.drawObject = this.drawObject;
                        this._currFunction = (EllipseSectorMeasure)e.Node.Tag;
                        //DisplayClickItem(sender, e);
                        break;
                    case "LineMeasure":
                        this.drawObject?.AttachPropertyData.Clear();
                        if (!(this.drawObject is userDrawLineMeasure))
                        {
                            this.drawObject.GrayValueInfo -= new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                            this.drawObject.ClearDrawingObject();
                            this.drawObject = new userDrawLineMeasure(this.hWindowControl1, ((LineMeasure)e.Node.Tag).FindLine.LinePixPosition, ((LineMeasure)e.Node.Tag).PixCoordSystem);
                            this.drawObject.GrayValueInfo += new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                        }
                        this.drawObject.AttachPropertyData.Clear();
                        foreach (KeyValuePair<string, object> item in this.listData)
                        {
                            if (item.Key != e.Node.Text)
                                this.drawObject.AttachPropertyData.Add(item.Value);
                        }
                        this.drawObject.SetParam(((LineMeasure)e.Node.Tag).PixCoordSystem);
                        this.drawObject.SetParam(((LineMeasure)e.Node.Tag).FindLine.LinePixPosition.AffinePixLine2D(((LineMeasure)e.Node.Tag).PixCoordSystem.GetVariationHomMat2D()));
                        this.drawObject.BackImage = ((LineMeasure)e.Node.Tag).ImageData != null ? ((LineMeasure)e.Node.Tag).ImageData : this.CurrentImageData;
                        this.drawObject.AttachDrawingObjectToWindow();
                        this.metrolegyParamForm.drawObject = this.drawObject;
                        this._currFunction = (LineMeasure)e.Node.Tag;
                        //DisplayClickItem(sender, e);
                        break;
                    case "PointMeasure":
                        this.drawObject?.AttachPropertyData.Clear();
                        if (!(this.drawObject is userDrawPointMeasure))
                        {
                            this.drawObject.GrayValueInfo -= new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                            this.drawObject.ClearDrawingObject();
                            this.drawObject = new userDrawPointMeasure(this.hWindowControl1, ((PointMeasure)e.Node.Tag).FindPoint.LinePixPosition, ((PointMeasure)e.Node.Tag).PixCoordSystem);
                            this.drawObject.GrayValueInfo += new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                        }
                        this.drawObject.AttachPropertyData.Clear();
                        foreach (KeyValuePair<string, object> item in this.listData)
                        {
                            if (item.Key != e.Node.Text)
                                this.drawObject.AttachPropertyData.Add(item.Value);
                        }
                        this.drawObject.SetParam(((PointMeasure)e.Node.Tag).PixCoordSystem);
                        this.drawObject.SetParam(((PointMeasure)e.Node.Tag).FindPoint.LinePixPosition.AffinePixLine2D(((PointMeasure)e.Node.Tag).PixCoordSystem.GetVariationHomMat2D()));
                        this.drawObject.BackImage = ((PointMeasure)e.Node.Tag).ImageData != null ? ((PointMeasure)e.Node.Tag).ImageData : this.CurrentImageData;
                        this.drawObject.AttachDrawingObjectToWindow();
                        this.metrolegyParamForm.drawObject = this.drawObject;
                        this._currFunction = (PointMeasure)e.Node.Tag;
                        //DisplayClickItem(sender, e);
                        break;
                    case "Rectangle2Measure":
                        this.drawObject?.AttachPropertyData.Clear();
                        if (!(this.drawObject is userDrawRect2Measure))
                        {
                            this.drawObject.GrayValueInfo -= new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                            this.drawObject.ClearDrawingObject();
                            this.drawObject = new userDrawRect2Measure(this.hWindowControl1, ((Rectangle2Measure)e.Node.Tag).FindRect2.Rect2PixPosition, ((Rectangle2Measure)e.Node.Tag).PixCoordSystem);
                            this.drawObject.GrayValueInfo += new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                        }
                        this.drawObject.AttachPropertyData.Clear();
                        foreach (KeyValuePair<string, object> item in this.listData)
                        {
                            if (item.Key != e.Node.Text)
                                this.drawObject.AttachPropertyData.Add(item.Value);
                        }
                        this.drawObject.SetParam(((Rectangle2Measure)e.Node.Tag).PixCoordSystem);
                        this.drawObject.SetParam(((Rectangle2Measure)e.Node.Tag).FindRect2.Rect2PixPosition.AffineTransPixRect2(((Rectangle2Measure)e.Node.Tag).PixCoordSystem.GetVariationHomMat2D())); //?.AffineWcsRectangle2D(((Rectangle2Measure)e.Node.Tag).WcsCoordSystem.GetVariationHomMat2D())
                        this.drawObject.BackImage = ((Rectangle2Measure)e.Node.Tag).ImageData != null ? ((Rectangle2Measure)e.Node.Tag).ImageData : this.CurrentImageData;
                        this.drawObject.AttachDrawingObjectToWindow();
                        this.metrolegyParamForm.drawObject = this.drawObject;
                        this._currFunction = (Rectangle2Measure)e.Node.Tag;
                        //DisplayClickItem(sender, e);
                        break;
                    case "CrossPointMeasure":
                        this.drawObject?.AttachPropertyData.Clear();
                        if (!(this.drawObject is userDrawCrossMeasure))
                        {
                            this.drawObject.GrayValueInfo -= new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                            this.drawObject.ClearDrawingObject();
                            this.drawObject = new userDrawCrossMeasure(this.hWindowControl1, ((CrossPointMeasure)e.Node.Tag).FindCrossPoint.LinePixPosition, ((CrossPointMeasure)e.Node.Tag).PixCoordSystem);
                            this.drawObject.GrayValueInfo += new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                        }
                        this.drawObject.AttachPropertyData.Clear();
                        foreach (KeyValuePair<string, object> item in this.listData)
                        {
                            if (item.Key != e.Node.Text)
                                this.drawObject.AttachPropertyData.Add(item.Value);
                        }
                        this.drawObject.SetParam(((CrossPointMeasure)e.Node.Tag).PixCoordSystem);
                        this.drawObject.SetParam(((CrossPointMeasure)e.Node.Tag).FindCrossPoint.LinePixPosition.AffinePixLine2D(((CrossPointMeasure)e.Node.Tag).PixCoordSystem.GetVariationHomMat2D()));
                        this.drawObject.BackImage = ((CrossPointMeasure)e.Node.Tag).ImageData != null ? ((CrossPointMeasure)e.Node.Tag).ImageData : this.CurrentImageData;
                        this.drawObject.AttachDrawingObjectToWindow();
                        this.metrolegyParamForm.drawObject = this.drawObject;
                        this._currFunction = (CrossPointMeasure)e.Node.Tag;
                        //DisplayClickItem(sender, e);
                        break;

                    ///////////////////////////////////////// 显示测量距离对象
                    case "CircleToCircleDist2D":
                    case "CircleToLineDist2D":
                    case "LineToLineDist2D":
                    case "PointToLineDist2D":
                        // DisplayClickItem(sender, e);
                        break;
                    default:

                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public void DisplayClickItem(object sender, TreeNodeMouseClickEventArgs e)  // 这里一定要加一个键值对变量来收集，以免出现重合
        {
            try
            {
                if (e.Node.Tag == null) return;
                if (e.Node.Name == null) return;// 在图形窗口只显示世界坐标元素，像素元素在图像窗口显示
                this.drawObject.AttachPropertyData.Clear();
                switch (e.Node.Tag.GetType().Name)
                {
                    case "CircleMeasure":
                    case "CircleSectorMeasure":
                    case "EllipseMeasure":
                    case "EllipseSectorMeasure":
                    case "LineMeasure":
                    case "PointMeasure":
                    case "Rectangle2Measure":
                    case "WidthMeasure":
                        this.drawObject.AttachPropertyData.Clear(); // 清空附加属性
                        this.drawObject.IsDispalyAttachEdgesProperty = true;
                        // 添加需要显示的元素
                        foreach (var items in this.listData.Keys)
                        {
                            if (items.Split('-')[0] == e.Node.Name) continue; // + "-" + "0"
                            this.drawObject.AttachPropertyData.Add(this.listData[items]);
                        }
                        this.drawObject.DrawingGraphicObject();
                        break;
                }
            }
            catch (Exception he)
            {

            }
        }

        #region 右键菜单项
        private void addContextMenu(HWindowControl hWindowControl)
        {
            ContextMenuStrip ContextMenuStrip1 = new ContextMenuStrip();
            ContextMenuStrip1.Name = hWindowControl.Name;
            // 添加右键菜单 
            ToolStripItem[] items = new ToolStripMenuItem[]
            {
                new ToolStripMenuItem("执行"),
                new ToolStripMenuItem("自适应窗口"),
                new ToolStripMenuItem("设置抓边参数"),
                 new ToolStripMenuItem("保存图像"),
            };
            ContextMenuStrip1.Items.AddRange(items); // 在滑赋值前不能调用 
            ContextMenuStrip1.ItemClicked += new ToolStripItemClickedEventHandler(hWindowControlContextMenuStrip_ItemClicked);
            hWindowControl.ContextMenuStrip = ContextMenuStrip1;
        }
        private void hWindowControlContextMenuStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            string name = e.ClickedItem.Text;
            try
            {
                ((ContextMenuStrip)sender).Close();
                switch (name)
                {
                    case "执行":
                        switch (this._currFunction.GetType().Name)
                        {
                            case "CircleMeasure":
                                this._currFunction?.Execute(this.drawObject.GetPixCircleParam());
                                break;
                            case "CircleSectorMeasure":
                                this._currFunction?.Execute(this.drawObject.GetPixCircleSectorParam());
                                break;
                            case "EllipseMeasure":
                                this._currFunction?.Execute(this.drawObject.GetPixEllipseParam());
                                break;
                            case "EllipseSectorMeasure":
                                this._currFunction?.Execute(this.drawObject.GetPixEllipseSectorParam());
                                break;
                            case "LineMeasure":
                                this._currFunction?.Execute(this.drawObject.GetPixLineParam());
                                break;
                            case "PointMeasure":
                                this._currFunction?.Execute(this.drawObject.GetPixLineParam());
                                break;
                            case "Rectangle2Measure":
                                this._currFunction?.Execute(this.drawObject.GetPixRectangle2Param());
                                break;
                                //case "WidthMeasure":
                                //    this._currFunction?.Execute(null);
                                //break;
                        }
                        break;
                    //////////////////////////////////////
                    case "自适应窗口":
                        this.drawObject?.AutoImage();
                        break;
                    case "设置抓边参数":
                        new MetrolegyParamForm(this._currFunction, this.drawObject).Show();
                        break;
                    case "保存图像":
                        ((ContextMenuStrip)sender).Close();
                        SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                        saveFileDialog1.Filter = "bmp files (*.bmp)|*.bmp|All files (*.*)|*.*";
                        saveFileDialog1.FilterIndex = 0;
                        saveFileDialog1.ShowDialog();
                        if (this.drawObject.BackImage != null && this.drawObject.BackImage.Image.IsInitialized())
                            this.drawObject.BackImage.Image.WriteImage("bmp", 0, saveFileDialog1.FileName);
                        else
                            MessageBox.Show("图像内容为空");
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        #endregion

        private void 数据读取dataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //try
            //{
            //    if (e.RowIndex >= 0)
            //    {
            //        switch (数据读取dataGridView.Columns[e.ColumnIndex].Name)
            //        {
            //            case "ReadDeleteBtn":
            //                this.listConfigPara.RemoveAt(e.RowIndex);
            //                break;
            //            case "ReadBtn":
            //                this.listConfigPara[e.RowIndex].ReadValue = CommunicationParamManger.Instance.ReadValue(this.listConfigPara[e.RowIndex]).ToString();
            //                break;
            //            case "WriteBtn":
            //                CommunicationParamManger.Instance.WriteValue(this.listConfigPara[e.RowIndex]);
            //                break;
            //            case "InsertBtn":
            //                this.listConfigPara.Insert(e.RowIndex, new CommunicationParam());
            //                break;
            //        }
            //    }
            //}
            //catch(Exception ex)
            //{
            //    MessageBox.Show(ex.ToString());
            //}
        }

        private void 数据写入dataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //try
            //{
            //    if (e.RowIndex >= 0)
            //    {
            //        switch (数据写入dataGridView.Columns[e.ColumnIndex].Name)
            //        {
            //            case "WriteDeleteBtn":
            //                this.listConfigPara.RemoveAt(e.RowIndex);
            //                break;
            //            case "ReadBtn":
            //                this.listConfigPara[e.RowIndex].ReadValue = CommunicationParamManger.Instance.ReadValue(this.listConfigPara[e.RowIndex]).ToString();
            //                break;
            //            case "WriteBtn":
            //                CommunicationParamManger.Instance.WriteValue(this.listConfigPara[e.RowIndex]);
            //                break;
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.ToString());
            //}
        }
    }
}

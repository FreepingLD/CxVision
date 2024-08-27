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
    public partial class ConditionaElseForm : Form
    {
        private Form form;
        private IFunction _function;
        private IFunction _currFunction;
        private DrawingBaseMeasure drawObject;
        private TreeViewWrapClass treeViewWrapClass;
        private ConditionaElse _ForLoopControl;
        private TreeNode _refNode;
        private Dictionary<string, object> listData = new Dictionary<string, object>();
        private MetrolegyParamForm metrolegyParamForm;
        private ImageDataClass CurrentImageData;
        public ConditionaElseForm(IFunction function)
        {
            this._function = function;
            InitializeComponent();
            this.drawObject = new DrawingBaseMeasure(this.hWindowControl1, false);
            this.Text = function.GetPropertyValues("名称").ToString();
            this.treeViewWrapClass = new TreeViewWrapClass(this.treeView1, this);
            this._ForLoopControl = (ConditionaElse)this._function;
            this.metrolegyParamForm = new MetrolegyParamForm(this.drawObject);
        }
        public ConditionaElseForm(TreeNode node)
        {       
            this._function = (IFunction)node?.Tag;
            InitializeComponent();
            this.drawObject = new DrawingBaseMeasure(this.hWindowControl1, false);
            this.Text = this._function.GetPropertyValues("名称").ToString();
            this.treeViewWrapClass = new TreeViewWrapClass(this.treeView1, this);
            this._ForLoopControl = (ConditionaElse)this._function;
            this._refNode = node;
            this.metrolegyParamForm = new MetrolegyParamForm(this.drawObject);
            this.treeViewWrapClass.ToolName = this._refNode.Name.Replace(".Tool", "");
        }

        private void ConditionaElseForm_Load(object sender, EventArgs e)
        {            // 注册事件
            this.drawObject.GrayValueInfo += new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
            BaseFunction.ExcuteCompleted += new ExcuteCompletedEventHandler(DisplayExcuteResult);
            TreeViewWrapClass.ClickNode += new ClickNodeEventHandler(this.DisplayClickObject);
            this.metrolegyParamForm = new MetrolegyParamForm(this.drawObject);
            this.addContextMenu(this.hWindowControl1);
            //////////////////////////////////////////
            // 加载程序
            this.LoadTreeNode();
            //this.treeView1.ShowPlusMinus = false;
            //this.treeView1.ShowRootLines = false;
            this.BindProperty();
        }

        private void BindProperty()
        {
            try
            {
                //this.循环次数textBox.DataBindings.Add("Text", ((ForLoopControl)this._function), "MaxCount", true, DataSourceUpdateMode.OnPropertyChanged);
            }
            catch
            {

            }
        }
        public void LoadTreeNode()
        {
            foreach (TreeNode item in this._refNode.Nodes)
            {
                this.treeView1.Nodes.Add(item.Clone() as TreeNode);
            }
        }
        private void ConcurrentExecutionForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                this.drawObject.GrayValueInfo -= new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                BaseFunction.ExcuteCompleted -= new ExcuteCompletedEventHandler(DisplayExcuteResult);
                TreeViewWrapClass.ClickNode -= new ClickNodeEventHandler(this.DisplayClickObject);
                this.treeViewWrapClass?.Uinit();
                this._refNode?.Nodes.Clear();
                foreach (TreeNode item1 in this.treeView1.Nodes)
                {
                    this._refNode?.Nodes.Add(item1.Clone() as TreeNode);
                }
                ((ConditionaElse)_refNode.Tag).ParentNode = _refNode;
                //this.treeViewWrapClass.ClearTreeView();
                this.treeView1.Nodes.Clear();
                this.treeView1.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void PLC信息dataGridView_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false;
        }

        private void 运行工具条toolStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            ToolStripItem item = e.ClickedItem;
            string name = item.Name;
            //int count = 1;
            switch (name)
            {
                case "运行toolStripButton":
                    this.运行toolStripButton.Enabled = false;
                    this.停止toolStripButton.Enabled = true;
                    this.treeViewWrapClass.RunSyn(this.运行toolStripButton, 1);
                    this.停止toolStripButton.Enabled = false;
                    this.运行toolStripButton.Enabled = true;
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
                    }
                    ((ForLoopControl)_refNode.Tag).ParentNode = _refNode;
                    break;

                default:
                    break;
            }
        }
        #region 视图交互
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
        public void DisplayExcuteResult(object sender, ExcuteCompletedEventArgs e)  // 这里一定要加一个键值对变量来收集，以免出现重合 ImageAcqCompleteEventArgs e
        {
            try
            {
                if (e.DataContent == null) return;// 在图形窗口只显示世界坐标元素，像素元素在图像窗口显示
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
                        //this.drawObject.AddViewObject(((RegionDataClass)e.DataContent).Region, "red");
                        break;
                    case nameof(XldDataClass):
                        if (this.listData.ContainsKey(e.ItemName))
                            this.listData[e.ItemName] = ((XldDataClass)e.DataContent);//.HXldCont;
                        else
                            this.listData.Add(e.ItemName, ((XldDataClass)e.DataContent)); //.HXldCont
                                                                                          //this.drawObject.AddViewObject(((RegionDataClass)e.DataContent).Region, "red");
                        break;
                    case "HXLDCont":
                        if (this.listData.ContainsKey(e.ItemName))
                            this.listData[e.ItemName] = (HXLDCont)e.DataContent;
                        else
                            this.listData.Add(e.ItemName, (HXLDCont)e.DataContent);
                        //this.drawObject.AddViewObject(((RegionDataClass)e.DataContent).Region, "green");
                        break;
                    case "userWcsCircle":
                        userWcsCircle wcsCircle = (userWcsCircle)e.DataContent;
                        if (this.listData.ContainsKey(e.ItemName))
                            this.listData[e.ItemName] = wcsCircle.GetPixCircle().GetXLD();
                        else
                            this.listData.Add(e.ItemName, wcsCircle.GetPixCircle().GetXLD());
                        /// 添加点
                        if (wcsCircle.EdgesPoint_xyz != null)
                        {

                            for (int i = 0; i < wcsCircle.EdgesPoint_xyz.Length; i++)
                            {
                                if (this.listData.ContainsKey(e.ItemName + "_" + i.ToString()))
                                    this.listData[e.ItemName + "_" + i.ToString()] = wcsCircle.EdgesPoint_xyz[i].GetPixPoint();
                                else
                                    this.listData.Add(e.ItemName + "_" + i.ToString(), wcsCircle.EdgesPoint_xyz[i].GetPixPoint());
                            }
                        }
                        break;
                    case "userWcsCircleSector":
                        userWcsCircleSector wcsCircleSector = ((userWcsCircleSector)e.DataContent);
                        if (this.listData.ContainsKey(e.ItemName))
                            this.listData[e.ItemName] = wcsCircleSector.GetPixCircleSector().GetXLD();
                        else
                            this.listData.Add(e.ItemName, wcsCircleSector.GetPixCircleSector().GetXLD());
                        /// 添加点
                        if (wcsCircleSector.EdgesPoint_xyz != null)
                        {

                            for (int i = 0; i < wcsCircleSector.EdgesPoint_xyz.Length; i++)
                            {
                                if (this.listData.ContainsKey(e.ItemName + "_" + i.ToString()))
                                    this.listData[e.ItemName + "_" + i.ToString()] = wcsCircleSector.EdgesPoint_xyz[i].GetPixPoint();
                                else
                                    this.listData.Add(e.ItemName + "_" + i.ToString(), wcsCircleSector.EdgesPoint_xyz[i].GetPixPoint());
                            }
                        }
                        break;
                    case "userWcsEllipse":
                        userWcsEllipse wcsEllipse = ((userWcsEllipse)e.DataContent);
                        if (this.listData.ContainsKey(e.ItemName))
                            this.listData[e.ItemName] = wcsEllipse.GetPixEllipse().GetXLD();
                        else
                            this.listData.Add(e.ItemName, wcsEllipse.GetPixEllipse().GetXLD());
                        /// 添加点
                        if (wcsEllipse.EdgesPoint_xyz != null)
                        {

                            for (int i = 0; i < wcsEllipse.EdgesPoint_xyz.Length; i++)
                            {
                                if (this.listData.ContainsKey(e.ItemName + "_" + i.ToString()))
                                    this.listData[e.ItemName + "_" + i.ToString()] = wcsEllipse.EdgesPoint_xyz[i].GetPixPoint();
                                else
                                    this.listData.Add(e.ItemName + "_" + i.ToString(), wcsEllipse.EdgesPoint_xyz[i].GetPixPoint());
                            }
                        }
                        break;
                    case "userWcsEllipseSector":
                        userWcsEllipseSector wcsEllipseSector = ((userWcsEllipseSector)e.DataContent);
                        if (this.listData.ContainsKey(e.ItemName))
                            this.listData[e.ItemName] = wcsEllipseSector.GetPixEllipseSector().GetXLD();
                        else
                            this.listData.Add(e.ItemName, wcsEllipseSector.GetPixEllipseSector().GetXLD());
                        /// 添加点
                        if (wcsEllipseSector.EdgesPoint_xyz != null)
                        {

                            for (int i = 0; i < wcsEllipseSector.EdgesPoint_xyz.Length; i++)
                            {
                                if (this.listData.ContainsKey(e.ItemName + "_" + i.ToString()))
                                    this.listData[e.ItemName + "_" + i.ToString()] = wcsEllipseSector.EdgesPoint_xyz[i].GetPixPoint();
                                else
                                    this.listData.Add(e.ItemName + "_" + i.ToString(), wcsEllipseSector.EdgesPoint_xyz[i].GetPixPoint());
                            }
                        }
                        break;
                    case "userWcsLine":
                        userWcsLine wcsLine = ((userWcsLine)e.DataContent);
                        if (this.listData.ContainsKey(e.ItemName))
                            this.listData[e.ItemName] = wcsLine.GetPixLine().GetXLD();
                        else
                            this.listData.Add(e.ItemName, wcsLine.GetPixLine().GetXLD());
                        /// 添加点
                        if (wcsLine.EdgesPoint_xyz != null)
                        {

                            for (int i = 0; i < wcsLine.EdgesPoint_xyz.Length; i++)
                            {
                                if (this.listData.ContainsKey(e.ItemName + "_" + i.ToString()))
                                    this.listData[e.ItemName + "_" + i.ToString()] = wcsLine.EdgesPoint_xyz[i].GetPixPoint();
                                else
                                    this.listData.Add(e.ItemName + "_" + i.ToString(), wcsLine.EdgesPoint_xyz[i].GetPixPoint());
                            }
                        }
                        break;

                    case "userWcsPoint":
                        userWcsPoint wcsPoint = ((userWcsPoint)e.DataContent);
                        if (this.listData.ContainsKey(e.ItemName))
                            this.listData[e.ItemName] = wcsPoint.GetPixPoint();
                        else
                            this.listData.Add(e.ItemName, wcsPoint.GetPixPoint());
                        break;
                    case "userWcsRectangle1":
                        userWcsRectangle1 wcsRectangle1 = ((userWcsRectangle1)e.DataContent);
                        if (this.listData.ContainsKey(e.ItemName))
                            this.listData[e.ItemName] = wcsRectangle1.GetPixRectangle1().GetXLD();
                        else
                            this.listData.Add(e.ItemName, wcsRectangle1.GetPixRectangle1().GetXLD());
                        /// 添加点
                        if (wcsRectangle1.EdgesPoint_xyz != null)
                        {
                            for (int i = 0; i < wcsRectangle1.EdgesPoint_xyz.Length; i++)
                            {
                                if (this.listData.ContainsKey(e.ItemName + "_" + i.ToString()))
                                    this.listData[e.ItemName + "_" + i.ToString()] = wcsRectangle1.EdgesPoint_xyz[i].GetPixPoint();
                                else
                                    this.listData.Add(e.ItemName + "_" + i.ToString(), wcsRectangle1.EdgesPoint_xyz[i].GetPixPoint());
                            }
                        }
                        break;
                    case "userWcsRectangle2":
                        userWcsRectangle2 wcsRect2 = ((userWcsRectangle2)e.DataContent);
                        if (this.listData.ContainsKey(e.ItemName))
                            this.listData[e.ItemName] = wcsRect2.GetPixRectangle2().GetXLD();
                        else
                            this.listData.Add(e.ItemName, wcsRect2.GetPixRectangle2().GetXLD());
                        /// 添加点
                        if (wcsRect2.EdgesPoint_xyz != null)
                        {
                            for (int i = 0; i < wcsRect2.EdgesPoint_xyz.Length; i++)
                            {
                                if (this.listData.ContainsKey(e.ItemName + "_" + i.ToString()))
                                    this.listData[e.ItemName + "_" + i.ToString()] = wcsRect2.EdgesPoint_xyz[i].GetPixPoint();
                                else
                                    this.listData.Add(e.ItemName + "_" + i.ToString(), wcsRect2.EdgesPoint_xyz[i].GetPixPoint());
                            }
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
                        ///////////////////////////////////////////////////////
                        int row, col, row222, col222;
                        this.hWindowControl1.HalconWindow.GetPart(out row, out col, out row222, out col222);
                        userOkNgText OkNGText = (userOkNgText)e.DataContent;
                        switch (GlobalVariable.pConfig.OKNgPosition)
                        {
                            case enFontPosition.左上角:
                                (OkNGText).WriteString(this.hWindowControl1.HalconWindow, row + GlobalVariable.pConfig.OKNgRowOffset, col + GlobalVariable.pConfig.OKNgColOffset, GlobalVariable.pConfig.OKNgSize); // 字体放在右上角,文本的参考角为左上角,在图形窗口中不能在代码定文本位置
                                break;
                            case enFontPosition.右上角:
                                (OkNGText).WriteString(this.hWindowControl1.HalconWindow, row + GlobalVariable.pConfig.OKNgRowOffset, col222 - GlobalVariable.pConfig.OKNgColOffset, GlobalVariable.pConfig.OKNgSize); // 字体放在右上角,文本的参考角为左上角,在图形窗口中不能在代码定文本位置
                                break;
                            case enFontPosition.左下角:
                                (OkNGText).WriteString(this.hWindowControl1.HalconWindow, row222 - GlobalVariable.pConfig.OKNgRowOffset, col + GlobalVariable.pConfig.OKNgColOffset, GlobalVariable.pConfig.OKNgSize); // 字体放在右上角,文本的参考角为左上角,在图形窗口中不能在代码定文本位置
                                break;
                            case enFontPosition.右下角:
                                (OkNGText).WriteString(this.hWindowControl1.HalconWindow, row222 - GlobalVariable.pConfig.OKNgRowOffset, col222 - GlobalVariable.pConfig.OKNgColOffset, GlobalVariable.pConfig.OKNgSize); // 字体放在右上角,文本的参考角为左上角,在图形窗口中不能在代码定文本位置
                                break;
                        }
                        // this.drawObject.AddViewObject(OkNGText, "red");
                        break;
                    case "userTextLable":
                        if (this.listData.ContainsKey(e.ItemName))
                            this.listData[e.ItemName] = e.DataContent;
                        else
                            this.listData.Add(e.ItemName, e.DataContent);
                        ////////////////////////////////////////////
                        break;
                    case "userTextLable[]":
                        userTextLable[] lable = e.DataContent as userTextLable[];
                        if (lable != null)
                        {
                            for (int i = 0; i < lable.Length; i++)
                            {
                                if (this.listData.ContainsKey(e.ItemName + i.ToString()))
                                    this.listData[e.ItemName + i.ToString()] = lable[i];
                                else
                                    this.listData.Add(e.ItemName + i.ToString(), lable[i]);
                            }
                        }
                        break;
                }
                ///////////////////////////////////////
                this.drawObject.AttachPropertyData.Clear();
                foreach (KeyValuePair<string, object> item in this.listData)
                {
                    this.drawObject.AttachPropertyData.Add(item.Value);
                }
                this.drawObject.DetachDrawingObjectFromWindow();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        public void DisplayClickObject(object sender, TreeNodeMouseClickEventArgs e)  //
        {
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
                        this.drawObject.SetParam(((CircleMeasure)e.Node.Tag).FindCircle.CirclePixPosition.AffineTransPixCircle(((CircleMeasure)e.Node.Tag).PixCoordSystem?.GetVariationHomMat2D()));
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
                        this.drawObject.SetParam(((CircleSectorMeasure)e.Node.Tag).FindCircleSector.CircleSectorPixPosition.AffineTransPixCircleSector(((CircleSectorMeasure)e.Node.Tag).PixCoordSystem?.GetVariationHomMat2D()));
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
                        this.drawObject.SetParam(((EllipseMeasure)e.Node.Tag).FindEllipse.EllipsePixPosition.AffineTransPixEllipse(((EllipseMeasure)e.Node.Tag).PixCoordSystem?.GetVariationHomMat2D()));
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
                        this.drawObject.SetParam(((EllipseSectorMeasure)e.Node.Tag).FindEllipseSector.EllipseSectorPixPosition.AffineTransPixEllipseSector(((EllipseSectorMeasure)e.Node.Tag).PixCoordSystem?.GetVariationHomMat2D()));
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
                        this.drawObject.SetParam(((LineMeasure)e.Node.Tag).FindLine.LinePixPosition.AffinePixLine2D(((LineMeasure)e.Node.Tag).PixCoordSystem?.GetVariationHomMat2D()));
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
                        this.drawObject.SetParam(((PointMeasure)e.Node.Tag).FindPoint.LinePixPosition.AffinePixLine2D(((PointMeasure)e.Node.Tag).PixCoordSystem?.GetVariationHomMat2D()));
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
                        this.drawObject.SetParam(((Rectangle2Measure)e.Node.Tag).FindRect2.Rect2PixPosition.AffineTransPixRect2(((Rectangle2Measure)e.Node.Tag).PixCoordSystem?.GetVariationHomMat2D())); //?.AffineWcsRectangle2D(((Rectangle2Measure)e.Node.Tag).WcsCoordSystem.GetVariationHomMat2D())
                        this.drawObject.BackImage = ((Rectangle2Measure)e.Node.Tag).ImageData != null ? ((Rectangle2Measure)e.Node.Tag).ImageData : this.CurrentImageData;
                        this.drawObject.AttachDrawingObjectToWindow();
                        this.metrolegyParamForm.drawObject = this.drawObject;
                        this._currFunction = (Rectangle2Measure)e.Node.Tag;
                        //DisplayClickItem(sender, e);
                        break;
                    case "WidthMeasure":
                        this.drawObject?.AttachPropertyData.Clear();
                        if (!(this.drawObject is userDrawWidthMeasure))
                        {
                            this.drawObject.GrayValueInfo -= new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                            this.drawObject.ClearDrawingObject();
                            this.drawObject = new userDrawWidthMeasure(this.hWindowControl1, ((WidthMeasure)e.Node.Tag).FindWidth.Rect2PixPosition, ((WidthMeasure)e.Node.Tag).PixCoordSystem);
                            this.drawObject.GrayValueInfo += new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                        }
                        this.drawObject.AttachPropertyData.Clear();
                        foreach (KeyValuePair<string, object> item in this.listData)
                        {
                            if (item.Key != e.Node.Text)
                                this.drawObject.AttachPropertyData.Add(item.Value);
                        }
                        this.drawObject.SetParam(((WidthMeasure)e.Node.Tag).PixCoordSystem);
                        this.drawObject.SetParam(((WidthMeasure)e.Node.Tag).FindWidth.Rect2PixPosition.AffineTransPixRect2(((WidthMeasure)e.Node.Tag).PixCoordSystem?.GetVariationHomMat2D())); //?.AffineWcsRectangle2D(((Rectangle2Measure)e.Node.Tag).WcsCoordSystem.GetVariationHomMat2D())
                        this.drawObject.BackImage = ((WidthMeasure)e.Node.Tag).ImageData != null ? ((WidthMeasure)e.Node.Tag).ImageData : this.CurrentImageData;
                        this.drawObject.AttachDrawingObjectToWindow();
                        this.metrolegyParamForm.drawObject = this.drawObject;
                        this._currFunction = (WidthMeasure)e.Node.Tag;
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
                new ToolStripMenuItem("------------"),
                new ToolStripMenuItem("清除窗口"),
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
                            case "WidthMeasure":
                                this._currFunction?.Execute(this.drawObject.GetPixRectangle2Param());
                                break;
                            case "CrossPointMeasure":
                                this._currFunction?.Execute(this.drawObject.GetPixLineParam());
                                break;
                        }
                        break;
                    //////////////////////////////////////
                    case "自适应窗口":
                        this.drawObject?.AutoImage();
                        break;
                    case "清除窗口":
                        this.drawObject?.ClearWindow();
                        this.listData.Clear(); // 清除窗口时,对象也清除
                        break;
                    case "设置抓边参数":
                        MetrolegyParamForm paramForm = new MetrolegyParamForm(this._currFunction, this.drawObject);
                        paramForm.Show();
                        paramForm.Owner = this;
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


    }
}

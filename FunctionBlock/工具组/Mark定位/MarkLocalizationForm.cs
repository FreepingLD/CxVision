
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
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using View;

namespace FunctionBlock
{
    public partial class MarkLocalizationForm : Form
    {
        private CancellationTokenSource cts;
        private IFunction _function;
        private IFunction _currFunction;
        private DrawingBaseMeasure drawObject;
        private TreeViewWrapClass treeViewWrapClass;
        private MarkLocalization _FeatureLocalization;
        private Dictionary<string, object> listData = new Dictionary<string, object>();
        private MetrolegyParamForm metrolegyParamForm;
        private TreeNode _refNode;
        private TreeNode _clickNode;
        private ImageDataClass CurrentImageData;
        private int _selectIndex = 0;
        private Form _form;

        public MarkLocalizationForm(IFunction function)
        {
            this._function = function;
            InitializeComponent();
            //this.drawObject = new DrawingBaseMeasure(this.hWindowControl1, true);
            this.treeViewWrapClass = new TreeViewWrapClass(this.treeView1, this);
            this._FeatureLocalization = (MarkLocalization)function;
        }
        public MarkLocalizationForm(TreeNode node, ImageDataClass imageData = null, string treeViewName = null)
        {
            this._refNode = node;
            this._function = (IFunction)node.Tag;
            InitializeComponent();
            //this.drawObject = new DrawingBaseMeasure(this.hWindowControl1, true);
            this.treeViewWrapClass = new TreeViewWrapClass(this.treeView1, this);
            //this.drawObject.BackImage = imageData;
            this._FeatureLocalization = (MarkLocalization)this._function;
            //this.Text = node.Text;
            this.treeViewWrapClass.ToolName = this._refNode.Name.Replace(".Tool", "");
            //////////////////////////
            if (treeViewName != null && treeViewName.Length > 0)
                this.Text = treeViewName + "." + node.FullPath.Replace("\\", ".");
            else
                this.Text = node.FullPath.Replace("\\", ".");
        }

        private void MarkLocalizationForm_Load(object sender, EventArgs e)
        {
            // 注册事件
            //this.drawObject.GrayValueInfo += new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
            BaseFunction.ExcuteCompleted += new ExcuteCompletedEventHandler(DisplayExcuteResult);
            TreeViewWrapClass.ClickNode += new ClickNodeEventHandler(this.DisplayClickObject);
            //this.metrolegyParamForm = new MetrolegyParamForm(this.drawObject);
            this.addContextMenu(this.hWindowControl1);
            //////////////////////////////////////////
            //////////////////////////////////////////
            this.CoordSysNameColumn.Items.Clear();
            this.CoordSysNameColumn.ValueType = typeof(enCoordSysName);
            foreach (enCoordSysName item in Enum.GetValues(typeof(enCoordSysName)))
                this.CoordSysNameColumn.Items.Add(item);
            ///////////////////////////////////
            this.CommunicationCommandCol.Items.Clear();
            this.CommunicationCommandCol.ValueType = typeof(enCommunicationCommand);
            foreach (enCommunicationCommand item in Enum.GetValues(typeof(enCommunicationCommand)))
                this.CommunicationCommandCol.Items.Add(item);
            ////////////////////////////////////
            this.LoadTreeNode();//
            this.treeView1.ShowPlusMinus = false;
            this.treeView1.ShowRootLines = false;
            this.数据读取dataGridView.DataSource = ((MarkLocalization)this._function).PlcInfo;
            this.AddForm(this.元素属性tabPage, new ElementViewForm(false));
        }

        public void LoadTreeNode()
        {
            foreach (TreeNode item in this._refNode.Nodes)
            {
                this.treeView1.Nodes.Add(item.Clone() as TreeNode);
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
        public Form AddForm(TabPage MastPanel, Form form)
        {
            if (MastPanel.Controls.Count > 0)
                MastPanel.Controls.Clear();
            form.TopLevel = false;
            form.FormBorderStyle = FormBorderStyle.None;
            form.MaximizeBox = false;
            form.MinimizeBox = false;
            form.Dock = DockStyle.Fill;
            form.Padding = new Padding(0);
            MastPanel.Margin = new Padding(0);
            MastPanel.Controls.Add(form);
            form.Show();
            return form;
        }

        // 获取鼠标位置处的高度值

        private void FeatureLocalizationForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                /// 转移节点到主面板上来
                this._refNode?.Nodes.Clear();
                foreach (TreeNode item1 in this.treeView1.Nodes)
                {
                    this._refNode?.Nodes.Add(item1.Clone() as TreeNode);
                }
                this.treeView1.Nodes.Clear();
                this.treeView1.Dispose();
                this.drawObject?.ClearDrawingObject();
                //this.drawObject.GrayValueInfo -= new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
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
            try
            {
                ToolStripItem item = e.ClickedItem;
                string name = item.Name;
                switch (name)
                {
                    case nameof(运行toolStripButton):
                        if (this.resultStatusLabel.Text == "等待……") break;
                        this.resultStatusLabel.Text = "等待……";
                        this.resultStatusLabel.ForeColor = Color.Yellow;
                        this.cts = new CancellationTokenSource();
                        Task.Run(() =>
                        {
                            if (this._function.Execute(this._refNode, "manual").Succss)
                            {
                                if (!this.cts.IsCancellationRequested)
                                {
                                    this.Invoke(new Action(() =>
                                    {
                                        //this.toolStripStatusLabel1.Text = "执行结果:";
                                        this.resultStatusLabel.Text = "成功";
                                        this.resultStatusLabel.ForeColor = Color.Green;
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
                                        this.resultStatusLabel.Text = "失败";
                                        this.resultStatusLabel.ForeColor = Color.Red;
                                    }));
                                }
                            }
                        }
                        );

                        break;

                    //case "停止toolStripButton":
                    case nameof(停止toolStripButton):
                        this.cts?.Cancel();
                        this.treeViewWrapClass.Stop();
                        this.运行toolStripButton.Enabled = true;
                        this.停止toolStripButton.Enabled = false;
                        break;

                    //case "检测工具toolStripButton":
                    case nameof(检测工具toolStripButton):
                        ToolForm tool = new ToolForm(this.treeViewWrapClass, this._refNode.Name.Replace(".Tool", ""));
                        tool.Owner = this;
                        tool.Show();
                        break;

                    case nameof(this.脚本配置toolStripButton):

                        break;
                    //////////////////////////////////////
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void hWindowControl1_MouseMove(object sender, GrayValueInfoEventArgs e)
        {
            try
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
            catch
            {

            }
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
                //if (SystemParamManager.Instance.SysConfigParam.IsAutoRun) return;
                /////////////////////////////////////////////
                switch (e.DataContent.GetType().Name) //这里只接受XLD轮廓或3D对象轮廓
                {
                    case "ImageDataClass":
                        this.listData.Clear();
                        //this.drawObject.BackImage = (ImageDataClass)e.DataContent;
                        //this.CurrentImageData = this.drawObject.BackImage;
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
                    case nameof(userWcsPolygon):
                        if (this.listData.ContainsKey(e.ItemName))
                            this.listData[e.ItemName] = ((userWcsPolygon)e.DataContent).GetPixPolygon().GetXLD();
                        else
                            this.listData.Add(e.ItemName, ((userWcsPolygon)e.DataContent).GetPixPolygon().GetXLD());
                        break;
                    case nameof(userWcsPolyLine):
                        if (this.listData.ContainsKey(e.ItemName))
                            this.listData[e.ItemName] = ((userWcsPolyLine)e.DataContent).GetPixPolyLine().GetXLD();
                        else
                            this.listData.Add(e.ItemName, ((userWcsPolyLine)e.DataContent).GetPixPolyLine().GetXLD());
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
                    //this.drawObject.AttachPropertyData.Clear();
                    ////this.drawObject.IsDispalyAttachDrawingProperty = false;
                    //foreach (KeyValuePair<string, object> item in this.listData)
                    //{
                    //    this.drawObject.AttachPropertyData.Add(item.Value);
                    //}
                    //this.drawObject.DetachDrawingObjectFromWindow();
                });
            }
            catch (Exception ex)
            {
                new Common.UserMessageForm(ex.ToString()).ShowDialog();
            }
        }
        public void DisplayClickObject(object sender, TreeNodeMouseClickEventArgs e)  //
        {
            if (e.Node.Tag == null) return;
            if (e.Button != MouseButtons.Left) return; // 点击右键时不变
            this._clickNode = e.Node;
            try
            {
                switch (e.Node.Tag.GetType().Name)
                {
                    case nameof(ImageAcq):
                        this._currFunction = (IFunction)this._clickNode.Tag;
                        this._form?.Close();
                        this._form = new ImageAcqAppForm(this._clickNode, this.hWindowControl1);

                        this.AddForm(this.参数tabPage, this._form);
                        break;
                    case nameof(NccModelMatch):
                        this._currFunction = (IFunction)this._clickNode.Tag;
                        this._form?.Close();
                        this._form = new NccMatchAppForm(this._clickNode, this.hWindowControl1);
                        this.AddForm(this.参数tabPage, this._form);
                        break;
                    case nameof(ShapeModelMatch2D):
                        this._currFunction = (IFunction)this._clickNode.Tag;
                        this._form?.Close();
                        this._form = new ShapeModelMatch2DAppForm(this._clickNode, this.hWindowControl1);
                        this.AddForm(this.参数tabPage, this._form);
                        break;

                    default:

                        break;
                }
            }
            catch (Exception ex)
            {
                new Common.UserMessageForm(ex.ToString()).ShowDialog();
            }
        }

        private void 数据读取dataGridView_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    BindingList<PlcCommunicateInfo> ReadDataList = (BindingList<PlcCommunicateInfo>)((FeatureLocalization)this._function).PlcInfo;
                    switch (数据读取dataGridView.Columns[e.ColumnIndex].Name)
                    {
                        case "DeleteBtn":
                            ReadDataList.RemoveAt(e.RowIndex);
                            break;
                        case "ReadBtn":
                            //ReadDataList[e.RowIndex].ReadValue = CommunicationParamManger.Instance.ReadValue(ReadDataList[e.RowIndex]).ToString();
                            break;
                        case "InseterBtn":
                            ReadDataList.Insert(e.RowIndex, new PlcCommunicateInfo());
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                new Common.UserMessageForm(ex.ToString()).ShowDialog();
            }
        }

        #region 右键菜单项
        private void addContextMenu(HWindowControl hWindowControl)
        {
            ContextMenuStrip ContextMenuStrip1 = new ContextMenuStrip();
            ContextMenuStrip1.Name = hWindowControl.Name;
            ToolStripItem[] items = null;
            switch (SystemParamManager.Instance.SysConfigParam.Language)
            {
                default:
                case "zh-CN":
                    // 添加右键菜单 
                    items = new ToolStripMenuItem[]
                   {
                     new ToolStripMenuItem("执行",null,null,"执行"),
                     new ToolStripMenuItem("自适应图像",null,null,"自适应图像"),
                     new ToolStripMenuItem("设置抓边参数",null,null,"设置抓边参数"),
                     new ToolStripMenuItem("------------"),
                     new ToolStripMenuItem("清除窗口",null,null,"清除窗口"),
                     new ToolStripMenuItem("保存图像",null,null,"保存图像"),
                   };
                    break;
                case "en-US":
                    // 添加右键菜单 
                    items = new ToolStripMenuItem[]
                   {
                     new ToolStripMenuItem("Excute",null,null,"执行"),
                     new ToolStripMenuItem("Auto Image Window",null,null,"自适应图像"),
                     new ToolStripMenuItem("Set Callipers Param",null,null,"设置抓边参数"),
                     new ToolStripMenuItem("------------"),
                     new ToolStripMenuItem("Clear Window",null,null,"清除窗口"),
                     new ToolStripMenuItem("Save Image",null,null,"保存图像"),
                   };
                    break;
            }
            ContextMenuStrip1.Items.AddRange(items); // 在滑赋值前不能调用 
            ContextMenuStrip1.ItemClicked += new ToolStripItemClickedEventHandler(hWindowControlContext_ItemClicked);
            hWindowControl.ContextMenuStrip = ContextMenuStrip1;
        }
        private void hWindowControlContext_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            string name = e.ClickedItem.Name;
            try
            {
                ((ContextMenuStrip)sender).Close();
                switch (name)
                {
                    case "执行":
                        switch (this._currFunction.GetType().Name)
                        {
                            case nameof(CircleMeasure):
                                this._currFunction?.Execute(this.drawObject.GetPixCircleParam(), this._clickNode);
                                //this.drawObject.DetachDrawingObjectFromWindow();
                                break;
                            case nameof(CircleSectorMeasure):
                                this._currFunction?.Execute(this.drawObject.GetPixCircleSectorParam(), this._clickNode);
                                break;
                            case nameof(EllipseMeasure):
                                this._currFunction?.Execute(this.drawObject.GetPixEllipseParam(), this._clickNode);
                                break;
                            case nameof(EllipseSectorMeasure):
                                this._currFunction?.Execute(this.drawObject.GetPixEllipseSectorParam(), this._clickNode);
                                break;
                            case nameof(LineMeasure):
                                this._currFunction?.Execute(this.drawObject.GetPixLineParam(), this._clickNode);
                                //this.drawObject.DetachDrawingObjectFromWindow();
                                break;
                            case nameof(PointMeasure):
                                this._currFunction?.Execute(this.drawObject.GetPixLineParam(), this._clickNode);
                                break;
                            case nameof(Rectangle2Measure):
                                this._currFunction?.Execute(this.drawObject.GetPixRectangle2Param(), this._clickNode);
                                break;
                            case nameof(WidthMeasure):
                                this._currFunction?.Execute(this.drawObject.GetPixRectangle2Param(), this._clickNode);
                                break;
                            case nameof(CrossPointMeasure):
                                this._currFunction?.Execute(this.drawObject.GetPixLineParam(), this._clickNode);
                                break;
                            case nameof(PolyLineMeasure):
                                this._currFunction?.Execute(this.drawObject.GetPixPolyLineParam(), this._clickNode);
                                break;
                            case nameof(PolygonMeasure):
                                this._currFunction?.Execute(this.drawObject.GetPixPolygonParam(), this._clickNode);
                                break;
                            case nameof(ManualPointMeasure):
                                this._currFunction?.Execute(this.drawObject.GetPixPointParam(), this._clickNode);
                                break;
                            case nameof(ManualPolygonMeasure):
                                this._currFunction?.Execute(this.drawObject.GetPixPolygonParam(), this._clickNode);
                                break;
                            case nameof(ManualPolyLineMeasure):
                                this._currFunction?.Execute(this.drawObject.GetPixPolyLineParam(), this._clickNode);
                                break;
                            case nameof(ManualCircleSectorMeasure):
                                this._currFunction?.Execute(this.drawObject.GetPixCircleSectorParam(), this._clickNode);
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
                            new UserMessageForm().ShowDialog("图像内容为空");
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                new Common.UserMessageForm(ex.ToString()).ShowDialog();
            }
        }
        #endregion

        #region  窗体移动功能

        private const int WM_SYSCOMMAND = 0x0112;
        private const int SC_MOVE = 0xF010;
        private const int HTCAPTION = 0x0002;
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();
        [DllImport("user32.dll")]
        public static extern bool SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);

        #endregion

        #region 防止改变窗口大小时控件闪烁功能
        protected override CreateParams CreateParams   //
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000; // Turn on WS_EX_COMPOSITED
                return cp;
            }
        }
        #endregion

        #region 窗体控制盒功能，关闭，最大化，最小化
        private void buttonMin_Click(object sender, EventArgs e)
        {
            //this.WindowState = FormWindowState.Minimized;  //最小化
        }

        private void buttonMax_Click(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Maximized)   //如果处于最大化，则还原
            {
                this.WindowState = FormWindowState.Normal;
            }
            else
            {
                this.WindowState = FormWindowState.Maximized;   //如果处于普通状态，则最大化
            }
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            //DialogResult dialogResult = new Common.UserMessageForm().ShowDialog("确定关闭窗体吗？", "关闭窗体");
            //if (dialogResult == DialogResult.OK)
            //{
            this.Close();  //关闭窗口
            //}
        }
        #endregion

        private void titleLabel_MouseDown(object sender, MouseEventArgs e)
        {
            FeatureLocalizationForm_MouseDown(null, null);
        }

        private void titleLabel_MouseEnter(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Hand;
            //this.titleLabel.BackColor = System.Drawing.Color.Orange;
        }

        private void titleLabel_MouseLeave(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Default;
            //this.titleLabel.BackColor = System.Drawing.Color.Orange;
            //this.titleLabel.BackColor = System.Drawing.Color.LightGray;
        }

        private void FeatureLocalizationForm_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, WM_SYSCOMMAND, SC_MOVE + HTCAPTION, 0);
        }

        private void 下一步Btn_Click(object sender, EventArgs e)
        {
            try
            {
                if (-1 < this._selectIndex && this._selectIndex < this.treeView1.Nodes.Count)
                {
                    DisplayClickObject(this.treeView1, new TreeNodeMouseClickEventArgs(this.treeView1.Nodes[this._selectIndex], MouseButtons.Left, 1, 0, 0));
                    this._selectIndex++;
                }
                else
                {
                    this._selectIndex = this.treeView1.Nodes.Count - 1;
                    DisplayClickObject(this.treeView1, new TreeNodeMouseClickEventArgs(this.treeView1.Nodes[this._selectIndex], MouseButtons.Left, 1, 0, 0));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void 上一步Btn_Click(object sender, EventArgs e)
        {
            try
            {
                if (-1 < this._selectIndex && this._selectIndex < this.treeView1.Nodes.Count)
                {
                    DisplayClickObject(this.treeView1, new TreeNodeMouseClickEventArgs(this.treeView1.Nodes[this._selectIndex], MouseButtons.Left, 1, 0, 0));
                    this._selectIndex--;
                }
                else
                {
                    this._selectIndex = 0;
                    DisplayClickObject(this.treeView1, new TreeNodeMouseClickEventArgs(this.treeView1.Nodes[this._selectIndex], MouseButtons.Left, 1, 0, 0));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void 执行Btn_Click(object sender, EventArgs e)
        {
            try
            {
                switch (this.执行Btn.Text)
                {
                    case "执行":
                        if (this.resultStatusLabel.Text == "等待……") break;
                        this.resultStatusLabel.Text = "等待……";
                        this.resultStatusLabel.ForeColor = Color.Yellow;
                        Task.Run(() =>
                        {
                            if (this._currFunction.Execute(this._refNode).Succss)
                            {
                                this.Invoke(new Action(() =>
                                {
                                    //this.toolStripStatusLabel1.Text = "执行结果:";
                                    this.resultStatusLabel.Text = "成功";
                                    this.resultStatusLabel.ForeColor = Color.Green;
                                }));
                            }
                            else
                            {
                                this.Invoke(new Action(() =>
                                {
                                    //this.toolStripStatusLabel1.Text = "执行结果:";
                                    this.resultStatusLabel.Text = "失败";
                                    this.resultStatusLabel.ForeColor = Color.Red;
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





    }
}

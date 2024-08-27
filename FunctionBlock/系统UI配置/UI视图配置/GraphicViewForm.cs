
using Common;
using FunctionBlock;
using HalconDotNet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using View;

namespace FunctionBlock
{
    public partial class GraphicViewForm : Form
    {
        private VisualizeView drawObject;
        private Dictionary<string, object> listData = new Dictionary<string, object>();
        private bool isShowImage = false;
        private bool isShowMultipleElement = true; // 默认显示多个对象
        private ViewConfigParam _viewConfigParam;
        private bool IsLoad = false; // 窗体加载后，设置为true
        public GraphicViewForm(ViewConfigParam viewConfigParam)
        {
            InitializeComponent();
            this._viewConfigParam = viewConfigParam;
            /////////////
            this.drawObject = new VisualizeView(this.图形视图hWindowControl, true);
            this.drawObject.HMouseDoubleClick += new HMouseEventHandler(this.hWindowControl1_DoubleClick);
            this.图形视图hWindowControl.Margin = new Padding(0);
            this.Padding = new Padding(0);
            if (!HWindowManage.HWindowList.ContainsKey(_viewConfigParam.ViewName))
            {
                HWindowManage.HWindowList.Add(_viewConfigParam.ViewName, this.图形视图hWindowControl.HalconWindow);
            }
            if (!HWindowManage.HWindowControlList.ContainsKey(_viewConfigParam.ViewName))
            {
                HWindowManage.HWindowControlList.Add(_viewConfigParam.ViewName, this.图形视图hWindowControl);
            }
            this.ContextMenu = new ContextMenu();
            this.titleLabel.Text = viewConfigParam.ViewName;
        }

        public GraphicViewForm()
        {
            InitializeComponent();
            /////////////
            this.drawObject = new VisualizeView(this.图形视图hWindowControl, true);
            this.drawObject.HMouseDoubleClick += new HMouseEventHandler(this.hWindowControl1_DoubleClick);
            this.图形视图hWindowControl.Margin = new Padding(0);
            this.Padding = new Padding(0);
            if (!HWindowManage.HWindowList.ContainsKey(_viewConfigParam.ViewName))
            {
                HWindowManage.HWindowList.Add(_viewConfigParam.ViewName, this.图形视图hWindowControl.HalconWindow);
            }
            if (!HWindowManage.HWindowControlList.ContainsKey(_viewConfigParam.ViewName))
            {
                HWindowManage.HWindowControlList.Add(_viewConfigParam.ViewName, this.图形视图hWindowControl);
            }
        }
        public GraphicViewForm(bool isShowImage, bool isShowMultipleElement)
        {
            InitializeComponent();
            this.isShowImage = isShowImage;
            this.isShowMultipleElement = isShowMultipleElement;
            /////////////
            this.drawObject = new VisualizeView(this.图形视图hWindowControl, true);
            this.drawObject.HMouseDoubleClick += new HMouseEventHandler(this.hWindowControl1_DoubleClick);
            this.图形视图hWindowControl.Margin = new Padding(0);
            this.Padding = new Padding(0);
            if (!HWindowManage.HWindowList.ContainsKey(_viewConfigParam.ViewName))
            {
                HWindowManage.HWindowList.Add(_viewConfigParam.ViewName, this.图形视图hWindowControl.HalconWindow);
            }
            if (!HWindowManage.HWindowControlList.ContainsKey(_viewConfigParam.ViewName))
            {
                HWindowManage.HWindowControlList.Add(_viewConfigParam.ViewName, this.图形视图hWindowControl);
            }
        }
        private void GraphicViewForm_Load(object sender, EventArgs e)
        {
            TreeViewWrapClass.ClickNode += new ClickNodeEventHandler(this.DisplayClickItem);
            BaseFunction.ExcuteCompleted += new ExcuteCompletedEventHandler(DisplayMeasureResult);
            DataInteractionClass.getInstance().ClearGraphic += new EventHandler(ClearGraphic);
            BaseFunction.ItemDelete += new ItemDeleteEventHandler(DeleteItems);
            /////////////////////////////////////////////////////////////////////
            this._viewConfigParam = this._viewConfigParam == null ? new ViewConfigParam() : this._viewConfigParam;
            this.Location = this._viewConfigParam.Location;
            this.Size = this._viewConfigParam.FormSize;
            this.IsLoad = true;
        }

        public void AddForm(Panel MastPanel, Form form)
        {
            if (MastPanel.Controls.Count > 0)
                MastPanel.Controls.Clear();
            form.TopLevel = false;
            form.FormBorderStyle = FormBorderStyle.None;
            form.MaximizeBox = false;
            form.MinimizeBox = false;
            form.Dock = DockStyle.Fill;
            form.Padding = new Padding(0);
            MastPanel.Margin = new Padding(1);
            MastPanel.Controls.Add(form);
            form.Show();
        }
        public void AddForm(TabPage MastPanel, Form form)
        {
            if (MastPanel.Controls.Count > 0)
                MastPanel.Controls.Clear();
            form.TopLevel = false;
            form.FormBorderStyle = FormBorderStyle.None;
            form.MaximizeBox = false;
            form.MinimizeBox = false;
            form.Dock = DockStyle.Fill;
            form.Padding = new Padding(0);
            MastPanel.Margin = new Padding(1);
            MastPanel.Controls.Add(form);
            form.Show();
        }
        public void AddForm(TableLayoutPanel MastPanel, Form form, int rowPose, int colPose, int rowSpan, int colSpan)
        {
            form.TopLevel = false;
            form.FormBorderStyle = FormBorderStyle.None;
            form.MaximizeBox = false;
            form.MinimizeBox = false;
            form.Dock = DockStyle.Fill;
            form.Padding = new Padding(0);
            MastPanel.Margin = new Padding(1);
            MastPanel.Controls.Add(form);
            MastPanel.SetRow(form, rowPose);
            MastPanel.SetColumn(form, colPose);
            MastPanel.SetRowSpan(form, rowSpan);
            MastPanel.SetColumnSpan(form, colSpan);
            form.Show();
        }

        /// <summary>
        /// 删除程序条目 
        /// </summary>
        /// <param name="send"></param>
        /// <param name="e"></param>
        private void DeleteItems(object send, ItemDeleteEventArgs e)
        {
            try
            {
                if (this.listData.ContainsKey(e.ItemName))
                    this.listData.Remove(e.ItemName);
                this.drawObject.AttachPropertyData.Clear();
                this.drawObject.AttachPropertyData.AddRange(this.listData.Values.ToArray()); //(userWcsCircle)e.DataContent
                this.drawObject.UpdataGraphicView(); // 背影不刷新
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
                    TabControl tabControl = ((TabPage)container).Parent as TabControl;
                    if (tabControl.SelectedTab != ((TabPage)container)) return result;
                    else
                        result = true;
                    break;
            }
            return result;
        }
        private void ClearGraphic(object send, EventArgs e)
        {
            this.listData?.Clear();
            this.drawObject?.AttachPropertyData.Clear();
            this.drawObject?.UpdataGraphicView();
        }
        public void DisplayClickItem(object sender, TreeNodeMouseClickEventArgs e)  // 这里一定要加一个键值对变量来收集，以免出现重合
        {
            try
            {
                if (!IsSelect()) return;
                if (e.Node.Tag == null) return;
                if (e.Node.Tag is AcqSource) return;
                if (e.Button == MouseButtons.Right) return;
                if (e.Node.Name == null) return;// 在图形窗口只显示世界坐标元素，像素元素在图像窗口显示  ,只需要知道点击了哪个节点即可，因为操作的是存储的集合对象，只可能是些结构类型
                this.drawObject.AttachPropertyData.Clear();
                if (isShowMultipleElement)
                {
                    string[] item = new string[this.listData.Count];
                    this.listData.Keys.CopyTo(item, 0);
                    for (int i = 0; i < item.Length; i++)
                    {
                        switch (this.listData[item[i]].GetType().Name)
                        {
                            case "userWcsCircle":
                                if (item[i].Split('-')[0] == e.Node.Text)
                                {
                                    userWcsCircle wcsCircle = (userWcsCircle)this.listData[item[i]];
                                    wcsCircle.Color = enColor.yellow;
                                    //if (this.listData.ContainsKey(item[i]))
                                    this.listData[item[i]] = wcsCircle;
                                }
                                else
                                {
                                    userWcsCircle wcsCircle = (userWcsCircle)this.listData[item[i]];
                                    wcsCircle.Color = enColor.green;
                                    //if (this.listData.ContainsKey(item[i]))
                                    this.listData[item[i]] = wcsCircle;
                                }
                                break;
                            case "userWcsCircleSector":
                                if (item[i].Split('-')[0] == e.Node.Text)
                                {
                                    userWcsCircleSector wcsCircle = (userWcsCircleSector)this.listData[item[i]];
                                    wcsCircle.Color = enColor.yellow;
                                    //if (this.listData.ContainsKey(item[i]))
                                    this.listData[item[i]] = wcsCircle;
                                }
                                else
                                {
                                    userWcsCircleSector wcsCircle = (userWcsCircleSector)this.listData[item[i]];
                                    wcsCircle.Color = enColor.green;
                                    //if (this.listData.ContainsKey(item[i]))
                                    this.listData[item[i]] = wcsCircle;
                                }
                                break;
                            case "userWcsEllipse":
                                if (item[i].Split('-')[0] == e.Node.Text)
                                {
                                    userWcsEllipse wcsCircle = (userWcsEllipse)this.listData[item[i]];
                                    wcsCircle.Color = enColor.yellow;
                                    //if (this.listData.ContainsKey(item[i]))
                                    this.listData[item[i]] = wcsCircle;
                                }
                                else
                                {
                                    userWcsEllipse wcsCircle = (userWcsEllipse)this.listData[item[i]];
                                    wcsCircle.Color = enColor.green;
                                    //if (this.listData.ContainsKey(item[i]))
                                    this.listData[item[i]] = wcsCircle;
                                }
                                break;
                            case "userWcsEllipseSector":
                                if (item[i].Split('-')[0] == e.Node.Text)
                                {
                                    userWcsEllipseSector wcsCircle = (userWcsEllipseSector)this.listData[item[i]];
                                    wcsCircle.Color = enColor.yellow;
                                    if (this.listData.ContainsKey(item[i]))
                                        this.listData[item[i]] = wcsCircle;
                                }
                                else
                                {
                                    userWcsEllipseSector wcsCircle = (userWcsEllipseSector)this.listData[item[i]];
                                    wcsCircle.Color = enColor.green;
                                    //if (this.listData.ContainsKey(item[i]))
                                    this.listData[item[i]] = wcsCircle;
                                }
                                break;
                            case "userWcsLine":
                                if (item[i].Split('-')[0] == e.Node.Text)
                                {
                                    userWcsLine wcsCircle = (userWcsLine)this.listData[item[i]];
                                    wcsCircle.Color = enColor.yellow;
                                    // if (this.listData.ContainsKey(item[i]))
                                    this.listData[item[i]] = wcsCircle;
                                }
                                else
                                {
                                    userWcsLine wcsCircle = (userWcsLine)this.listData[item[i]];
                                    wcsCircle.Color = enColor.green;
                                    //if (this.listData.ContainsKey(item[i]))
                                    this.listData[item[i]] = wcsCircle;
                                }
                                break;
                            case "userWcsPoint":
                                if (item[i].Split('-')[0] == e.Node.Text)
                                {
                                    userWcsPoint wcsCircle = (userWcsPoint)this.listData[item[i]];
                                    wcsCircle.Color = enColor.yellow;
                                    //if (this.listData.ContainsKey(item[i]))
                                    this.listData[item[i]] = wcsCircle;
                                }
                                else
                                {
                                    userWcsPoint wcsCircle = (userWcsPoint)this.listData[item[i]];
                                    wcsCircle.Color = enColor.green;
                                    //if (this.listData.ContainsKey(item[i]))
                                    this.listData[item[i]] = wcsCircle;
                                }
                                break;
                            case "userWcsRectangle1":
                                if (item[i].Split('-')[0] == e.Node.Text)
                                {
                                    userWcsRectangle1 wcsCircle = (userWcsRectangle1)this.listData[item[i]];
                                    wcsCircle.Color = enColor.yellow;
                                    //if (this.listData.ContainsKey(item[i]))
                                    this.listData[item[i]] = wcsCircle;
                                }
                                else
                                {
                                    userWcsRectangle1 wcsCircle = (userWcsRectangle1)this.listData[item[i]];
                                    wcsCircle.Color = enColor.green;
                                    // if (this.listData.ContainsKey(item[i]))
                                    this.listData[item[i]] = wcsCircle;
                                }
                                break;
                            case "userWcsRectangle2":
                                if (item[i].Split('-')[0] == e.Node.Text)
                                {
                                    userWcsRectangle2 wcsCircle = (userWcsRectangle2)this.listData[item[i]];
                                    wcsCircle.Color = enColor.yellow;
                                    //if (this.listData.ContainsKey(item[i]))
                                    this.listData[item[i]] = wcsCircle;
                                }
                                else
                                {
                                    userWcsRectangle2 wcsCircle = (userWcsRectangle2)this.listData[item[i]];
                                    wcsCircle.Color = enColor.green;
                                    //if (this.listData.ContainsKey(item[i]))
                                    this.listData[item[i]] = wcsCircle;
                                }
                                break;
                            case "userWcsCoordSystem":
                                if ((item[i].Split('-')[0] + "-" + item[i].Split('-')[1]) == e.Node.Text)
                                {
                                    userWcsCoordSystem wcsCircle = (userWcsCoordSystem)this.listData[item[i]];
                                    wcsCircle.Color = enColor.yellow;
                                    //if (this.listData.ContainsKey(item[i]))
                                    this.listData[item[i]] = wcsCircle;
                                }
                                else
                                {
                                    userWcsCoordSystem wcsCircle = (userWcsCoordSystem)this.listData[item[i]];
                                    wcsCircle.Color = enColor.orange;
                                    //if (this.listData.ContainsKey(item[i]))
                                    this.listData[item[i]] = wcsCircle;
                                }
                                break;
                        }
                    }
                    this.drawObject.AttachPropertyData.AddRange(this.listData.Values); //(userWcsCircle)e.DataContent
                    this.drawObject.UpdataGraphicView(); // 背影不刷新  
                }
                //else
                //    this.drawObject.AttachPropertyData.Add((userWcsCircle)e.DataContent);

            }
            catch (Exception he)
            {

            }
        }

        // 更新3D对象模型 ；响应测量完成/及响应鼠标点击事件
        public void DisplayMeasureResult(object sender, ExcuteCompletedEventArgs e)  // 这里一定要加一个键值对变量来收集，以免出现重合
        {
            try
            {
                if (!IsSelect()) return;
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
                            if (this.isShowImage)
                                this.drawObject.BackImage = new ImageDataClass((HImage)e.DataContent); // 图形窗口不显示图像
                            else
                                this.drawObject.BackImage = null;
                            break;
                        case "ImageDataClass":
                            if (this.isShowImage)
                                this.drawObject.BackImage = (ImageDataClass)e.DataContent;
                            else
                                this.drawObject.BackImage = null;
                            break;
                        //case "XldDataClass":
                        //    this.drawObject.XldContourData = (XldDataClass)e.DataContent;
                        //    break;
                        //case "XldDataClass[]":
                        //     this.drawObject.XldContourData = (XldDataClass)e.DataContent;;
                        //    break;
                        case "HXLDCont":
                            this.drawObject.XldContourData = new XldDataClass((HXLDCont)e.DataContent) ;
                            break;
                        case "userWcsCircle":
                            this.drawObject.AttachPropertyData.Clear();
                            if (isShowMultipleElement)
                            {
                                if (this.listData.ContainsKey(e.ItemName))
                                    this.listData[e.ItemName] = e.DataContent;
                                else
                                    this.listData.Add(e.ItemName, e.DataContent);
                                this.drawObject.AttachPropertyData.AddRange(this.listData.Values.ToArray()); //(userWcsCircle)e.DataContent
                            }
                            else
                                this.drawObject.AttachPropertyData.Add((userWcsCircle)e.DataContent);
                            this.drawObject.UpdataGraphicView(); // 背影不刷新
                            break;
                        case "userWcsCircleSector":
                            this.drawObject.AttachPropertyData.Clear();
                            if (isShowMultipleElement)
                            {
                                if (this.listData.ContainsKey(e.ItemName))
                                    this.listData[e.ItemName] = e.DataContent;
                                else
                                    this.listData.Add(e.ItemName, e.DataContent);
                                this.drawObject.AttachPropertyData.AddRange(this.listData.Values.ToArray());
                            }
                            else
                                this.drawObject.AttachPropertyData.Add((userWcsCircleSector)e.DataContent);
                            this.drawObject.UpdataGraphicView();
                            break;
                        case "userWcsEllipse":
                            this.drawObject.AttachPropertyData.Clear();
                            if (isShowMultipleElement)
                            {
                                if (this.listData.ContainsKey(e.ItemName))
                                    this.listData[e.ItemName] = e.DataContent;
                                else
                                    this.listData.Add(e.ItemName, e.DataContent);
                                this.drawObject.AttachPropertyData.AddRange(this.listData.Values.ToArray());
                            }
                            else
                                this.drawObject.AttachPropertyData.Add((userWcsEllipse)e.DataContent);
                            this.drawObject.UpdataGraphicView();
                            break;
                        case "userWcsEllipseSector":
                            this.drawObject.AttachPropertyData.Clear();
                            if (isShowMultipleElement)
                            {
                                if (this.listData.ContainsKey(e.ItemName))
                                    this.listData[e.ItemName] = e.DataContent;
                                else
                                    this.listData.Add(e.ItemName, e.DataContent);
                                this.drawObject.AttachPropertyData.AddRange(this.listData.Values.ToArray());
                            }
                            else
                                this.drawObject.AttachPropertyData.Add((userWcsEllipseSector)e.DataContent);
                            this.drawObject.UpdataGraphicView();
                            break;
                        case "userWcsLine":
                            this.drawObject.AttachPropertyData.Clear();
                            if (isShowMultipleElement)
                            {
                                if (this.listData.ContainsKey(e.ItemName))
                                    this.listData[e.ItemName] = e.DataContent;
                                else
                                    this.listData.Add(e.ItemName, e.DataContent);
                                this.drawObject.AttachPropertyData.AddRange(this.listData.Values.ToArray());
                            }
                            else
                                this.drawObject.AttachPropertyData.Add((userWcsLine)e.DataContent);
                            this.drawObject.UpdataGraphicView();
                            break;
                        case "userWcsLine[]":
                            this.drawObject.AttachPropertyData.Clear();
                            if (isShowMultipleElement)
                            {
                                if (this.listData.ContainsKey(e.ItemName))
                                    this.listData[e.ItemName] = e.DataContent;
                                else
                                    this.listData.Add(e.ItemName, e.DataContent);
                                this.drawObject.AttachPropertyData.AddRange(this.listData.Values.ToArray());
                            }
                            else
                                this.drawObject.AttachPropertyData.Add((userWcsLine[])e.DataContent);
                            this.drawObject.UpdataGraphicView();
                            break;
                        case "userWcsPoint":
                            this.drawObject.AttachPropertyData.Clear();
                            if (isShowMultipleElement)
                            {
                                if (this.listData.ContainsKey(e.ItemName))
                                    this.listData[e.ItemName] = e.DataContent;
                                else
                                    this.listData.Add(e.ItemName, e.DataContent);
                                this.drawObject.AttachPropertyData.AddRange(this.listData.Values.ToArray());
                            }
                            else
                                this.drawObject.AttachPropertyData.Add((userWcsPoint)e.DataContent);  //Dictionary<string, HXLDCont>
                            this.drawObject.UpdataGraphicView();
                            break;
                        case "userWcsRectangle1":
                            this.drawObject.AttachPropertyData.Clear();
                            if (isShowMultipleElement)
                            {
                                if (this.listData.ContainsKey(e.ItemName))
                                    this.listData[e.ItemName] = e.DataContent;
                                else
                                    this.listData.Add(e.ItemName, e.DataContent);
                                this.drawObject.AttachPropertyData.AddRange(this.listData.Values.ToArray());
                            }
                            else
                                this.drawObject.AttachPropertyData.Add((userWcsRectangle1)e.DataContent);  //Dictionary<string, HXLDCont>
                            this.drawObject.UpdataGraphicView();
                            break;
                        case "userWcsRectangle2":
                            this.drawObject.AttachPropertyData.Clear();
                            if (isShowMultipleElement)
                            {
                                if (this.listData.ContainsKey(e.ItemName))
                                    this.listData[e.ItemName] = e.DataContent;
                                else
                                    this.listData.Add(e.ItemName, e.DataContent);
                                this.drawObject.AttachPropertyData.AddRange(this.listData.Values.ToArray());
                            }
                            else
                                this.drawObject.AttachPropertyData.Add((userWcsRectangle2)e.DataContent);  //Dictionary<string, HXLDCont>
                            this.drawObject.UpdataGraphicView();
                            break;

                        case "userWcsCoordSystem":
                            this.drawObject.AttachPropertyData.Clear();
                            if (isShowMultipleElement)
                            {
                                if (this.listData.ContainsKey(e.ItemName))
                                    this.listData[e.ItemName] = e.DataContent;
                                else
                                    this.listData.Add(e.ItemName, e.DataContent);
                                this.drawObject.AttachPropertyData.AddRange(this.listData.Values.ToArray());
                            }
                            else
                                this.drawObject.AttachPropertyData.Add((userWcsCoordSystem)e.DataContent);  //Dictionary<string, HXLDCont>
                            this.drawObject.UpdataGraphicView();
                            break;

                        case "userOkNgText":
                            this.drawObject.AttachPropertyData.Clear();
                            if (isShowMultipleElement)
                            {
                                if (this.listData.ContainsKey(e.ItemName))
                                    this.listData[e.ItemName] = e.DataContent;
                                else
                                    this.listData.Add(e.ItemName, e.DataContent);
                                this.drawObject.AttachPropertyData.AddRange(this.listData.Values.ToArray());
                            }
                            else
                                this.drawObject.AttachPropertyData.Add(e.DataContent);  //Dictionary<string, HXLDCont>
                            this.drawObject.UpdataGraphicView();
                            break;
                        case "userTextLable":
                            this.drawObject.AttachPropertyData.Clear();
                            if (isShowMultipleElement)
                            {
                                if (this.listData.ContainsKey(e.ItemName))
                                    this.listData[e.ItemName] = e.DataContent;
                                else
                                    this.listData.Add(e.ItemName, e.DataContent);
                                this.drawObject.AttachPropertyData.AddRange(this.listData.Values.ToArray());
                            }
                            else
                                this.drawObject.AttachPropertyData.Add(e.DataContent);  //Dictionary<string, HXLDCont>
                            this.drawObject.UpdataGraphicView();
                            break;
                            ///////////////////////////////////////// 显示测量距离对象
                            //case "CircleToCircleDist2D":
                            //case "CircleToLineDist2D":
                            //case "LineToLineDist2D":
                            //case "PointToLineDist2D":
                            //case "CircleMeasure":
                            //case "CircleSectorMeasure":
                            //case "EllipseMeasure":
                            //case "EllipseSectorMeasure":
                            //case "LineMeasure":
                            //case "PointMeasure":
                            //case "Rectangle2Measure":
                            //case "WidthMeasure":
                            //    DisplayClickItem(null, new ExcuteCompletedEventArgs(e.ItemName, e.DataContent));
                            //    break;
                    }
                }
            }
            catch (Exception he)
            {
                //LoggerHelper.Warn("显示3D对象错误", he);
            }
        }

        private void GraphicViewForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                this.IsLoad = false;
                ViewConfigParamManager.Instance.ViewParamList.Remove(this._viewConfigParam); // 关闭窗体时要删除相应的对象
                if (this.drawObject != null)
                    this.drawObject.ClearDrawingObject();
                TreeViewWrapClass.ClickNode -= new ClickNodeEventHandler(this.DisplayClickItem);
                //DataInteractionClass.getInstance().NodeClick -= new ExcuteCompletedEventHandler(DisplayMeasureResult);
                BaseFunction.ItemDelete -= new ItemDeleteEventHandler(DeleteItems);
                BaseFunction.ExcuteCompleted -= new ExcuteCompletedEventHandler(DisplayMeasureResult);
                this.drawObject.HMouseDoubleClick -= new HMouseEventHandler(this.hWindowControl1_DoubleClick);
                DataInteractionClass.getInstance().ClearGraphic -= new EventHandler(ClearGraphic);
                //OutputData.JudgeResult -= new ExcuteCompletedEventHandler(DisplayMeasureResult);
            }
            catch
            {

            }
        }
        private void hWindowControl1_DoubleClick(Object sender, HalconDotNet.HMouseEventArgs e)
        {
            //int a = 10;
            this.buttonMax_Click(null, null);
        }
        private void GraphicViewForm_Move(object sender, EventArgs e)
        {
            if (this._viewConfigParam != null)
                this._viewConfigParam.Location = this.Location;
        }

        private void GraphicViewForm_Resize(object sender, EventArgs e)
        {
            if (this._viewConfigParam != null && this.IsLoad) // 在窗体加载后这个事件才起作用
                this._viewConfigParam.FormSize = this.Size;
        }

        private void GraphicViewForm_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, WM_SYSCOMMAND, SC_MOVE + HTCAPTION, 0);
        }


        #region  窗体移动功能
        private const int WM_SYSCOMMAND = 0x0112;
        private const int SC_MOVE = 0xF010;
        private const int HTCAPTION = 0x0002;
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();
        [DllImport("user32.dll")]
        public static extern bool SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);

        #endregion

        #region  窗体绽放功能 
        private const int Guying_HTLEFT = 10;
        private const int Guying_HTRIGHT = 11;
        private const int Guying_HTTOP = 12;
        private const int Guying_HTTOPLEFT = 13;
        private const int Guying_HTTOPRIGHT = 14;
        private const int Guying_HTBOTTOM = 15;
        private const int Guying_HTBOTTOMLEFT = 0x10;
        private const int Guying_HTBOTTOMRIGHT = 17;
        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case 0x0084:
                    base.WndProc(ref m);
                    Point vPoint = new Point((int)m.LParam & 0xFFFF,
                        (int)m.LParam >> 16 & 0xFFFF);
                    vPoint = PointToClient(vPoint);
                    if (vPoint.X <= 5)
                        if (vPoint.Y <= 5)
                            m.Result = (IntPtr)Guying_HTTOPLEFT;
                        else if (vPoint.Y >= ClientSize.Height - 5)
                            m.Result = (IntPtr)Guying_HTBOTTOMLEFT;
                        else m.Result = (IntPtr)Guying_HTLEFT;
                    else if (vPoint.X >= ClientSize.Width - 5)
                        if (vPoint.Y <= 5)
                            m.Result = (IntPtr)Guying_HTTOPRIGHT;
                        else if (vPoint.Y >= ClientSize.Height - 5)
                            m.Result = (IntPtr)Guying_HTBOTTOMRIGHT;
                        else m.Result = (IntPtr)Guying_HTRIGHT;
                    else if (vPoint.Y <= 2)
                        m.Result = (IntPtr)Guying_HTTOP;
                    else if (vPoint.Y >= ClientSize.Height - 5)
                        m.Result = (IntPtr)Guying_HTBOTTOM;
                    break;
                default:
                    base.WndProc(ref m);
                    break;
            }
        }
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
        private void buttonClose_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("确定关闭窗体吗？", "关闭窗体", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                this.Close();  //关闭窗口
            }
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
        private void buttonMin_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;  //最小化
        }

        #endregion
        private void titleLabel_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, WM_SYSCOMMAND, SC_MOVE + HTCAPTION, 0);
        }

        private void titleLabel_MouseEnter(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Hand;
            this.titleLabel.BackColor = System.Drawing.Color.Orange;
        }

        private void titleLabel_MouseLeave(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Default;
            this.titleLabel.BackColor = System.Drawing.Color.LightGray;
        }




    }
}

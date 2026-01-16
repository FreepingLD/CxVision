
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
            if (!HWindowManage.HVisualizeView.ContainsKey(_viewConfigParam.ViewName))
            {
                HWindowManage.HVisualizeView.Add(_viewConfigParam.ViewName, this.drawObject);
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
            if (!HWindowManage.HVisualizeView.ContainsKey(_viewConfigParam.ViewName))
            {
                HWindowManage.HVisualizeView.Add(_viewConfigParam.ViewName, this.drawObject);
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
            if (!HWindowManage.HVisualizeView.ContainsKey(_viewConfigParam.ViewName))
            {
                HWindowManage.HVisualizeView.Add(_viewConfigParam.ViewName, this.drawObject);
            }

        }
        private void GraphicViewForm_Load(object sender, EventArgs e)
        {
            TreeViewWrapClass.ClickNode += new ClickNodeEventHandler(this.DisplayClickItem);
            BaseFunction.ExcuteCompleted += new ExcuteCompletedEventHandler(DisplayExcuteResult);
            DataInteractionClass.getInstance().ClearGraphic += new EventHandler(ClearGraphic);
            BaseFunction.ItemDelete += new ItemDeleteEventHandler(DeleteItems);
            UserLoginParamManager.Instance.UserChange += new EventHandler(this.UserChange_Event);
            this.drawObject.GrayValueInfo += new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
            /////////////////////////////////////////////////////////////////////
            this._viewConfigParam = this._viewConfigParam == null ? new ViewConfigParam() : this._viewConfigParam;
            this.Location = this._viewConfigParam.Location;
            this.Size = this._viewConfigParam.FormSize;
            this.IsLoad = true;
            this.BindProperty();
        }

        private void BindProperty()
        {
            try
            {
                //this.传感器comboBox.Items.Clear();
                //this.传感器comboBox.Items.AddRange(AcqSourceManage.Instance.GetAcqSourceName());
                //this.传感器comboBox.Text = this._viewConfigParam.CamName;
            }
            catch (Exception ex)
            {
                new Common.UserMessageForm(ex.ToString()).ShowDialog();
            }
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
        public void UserChange_Event(object sender, EventArgs e)
        {
            try
            {
                UserLoginParam loginParam = sender as UserLoginParam;
                switch (loginParam.User)
                {
                    case enUserName.操作员:
                        if (this._viewConfigParam != null)
                            this.buttonClose.Enabled = false;
                        else
                            this.buttonClose.Enabled = true;
                        ////////////////////////////////////////////////////
                        this.buttonMax.Hide();
                        this.buttonMin.Hide();
                        this.buttonClose.Hide();
                        this.tableLayoutPanel1.SetColumnSpan(this.titleLabel, 4);
                        break;
                    case enUserName.工程师:
                        this.buttonClose.Enabled = true;
                        ////////////////////////////////////////////////////
                        this.buttonMax.Hide();
                        this.buttonMin.Hide();
                        this.buttonClose.Hide();
                        this.tableLayoutPanel1.SetColumnSpan(this.titleLabel, 4);
                        break;
                    case enUserName.开发人员:
                        this.buttonClose.Enabled = true;
                        ////////////////////////////////////////////////////
                        this.buttonMax.Show();
                        this.buttonMin.Show();
                        this.buttonClose.Show();
                        this.tableLayoutPanel1.SetColumnSpan(this.titleLabel, 1);
                        break;
                }
            }
            catch
            {
            }
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
            bool result = true;
            Control container = this.Parent;
            if (container == null) return result;
            switch (container.GetType().Name)
            {
                case nameof(TabPage):
                    if (!SystemParamManager.Instance.SysConfigParam.DisablePageSwitch)
                    {
                        this.Invoke(new Action(() =>
                        {
                            TabControl tabControl = ((TabPage)container).Parent as TabControl;
                            if (SystemParamManager.Instance.SysConfigParam.IsAutoRun)        // 自动运行状态下才自动切换
                            {
                                if (tabControl.SelectedTab != ((TabPage)container))
                                    tabControl.SelectedTab = ((TabPage)container);
                                result = true;
                            }
                        }));
                    }
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
                return;
                //if (!IsSelect()) return;
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
        public void DisplayExcuteResult(object sender, ExcuteCompletedEventArgs e)  // 这里一定要加一个键值对变量来收集，以免出现重合
        {
            try
            {
                if (e.SensorName == null) return;
                if ((e.ViewWindow == null || e.ViewWindow != this._viewConfigParam.ViewName) && e.ViewWindow != "ALL") return;
                if (!IsSelect()) return;
                if (e.DataContent != null) // 在图形窗口只显示世界坐标元素，像素元素在图像窗口显示
                {
                    switch (e.DataContent.GetType().Name) //这里只接受XLD轮廓或3D对象轮廓
                    {
                        case nameof(HObjectModel3D):
                            this.drawObject.PointCloudModel3D?.ClearObjectModel3d();
                            this.drawObject.PointCloudModel3D = new PointCloudData((HObjectModel3D)e.DataContent);
                            break;
                        case "HObjectModel3D[]":
                            this.drawObject.PointCloudModel3D?.ClearObjectModel3d();
                            this.drawObject.PointCloudModel3D = new PointCloudData((HObjectModel3D[])e.DataContent);
                            break;
                        case nameof(PointCloudData):
                            this.drawObject.PointCloudModel3D?.ClearObjectModel3d();
                            this.drawObject.PointCloudModel3D = ((PointCloudData)e.DataContent);
                            break;
                        case nameof(HXLDCont):
                            this.drawObject.XldContourData = new XldDataClass((HXLDCont)e.DataContent);
                            break;
                        case nameof(userWcsCircle):
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
                        case nameof(userWcsCircleSector):
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
                        case nameof(userWcsPolyLine):
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
                        case nameof(userWcsEllipse):
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
                        case nameof(userWcsEllipseSector):
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
                        case nameof(userWcsLine):
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
                        case nameof(userWcsPoint):
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
                        case nameof(userWcsRectangle1):
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
                        case nameof(userWcsRectangle2):
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

                        case nameof(userWcsCoordSystem):
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
                if (this.drawObject != null)
                    this.drawObject.ClearDrawingObject();
                TreeViewWrapClass.ClickNode -= new ClickNodeEventHandler(this.DisplayClickItem);
                BaseFunction.ItemDelete -= new ItemDeleteEventHandler(DeleteItems);
                BaseFunction.ExcuteCompleted -= new ExcuteCompletedEventHandler(DisplayExcuteResult);
                this.drawObject.HMouseDoubleClick -= new HMouseEventHandler(this.hWindowControl1_DoubleClick);
                DataInteractionClass.getInstance().ClearGraphic -= new EventHandler(ClearGraphic);
                UserLoginParamManager.Instance.UserChange -= new EventHandler(this.UserChange_Event);
                this.drawObject.GrayValueInfo -= new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                //OutputData.JudgeResult -= new ExcuteCompletedEventHandler(DisplayMeasureResult);
            }
            catch
            {

            }
        }
        private void hWindowControl1_DoubleClick(Object sender, HalconDotNet.HMouseEventArgs e)
        {
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
                //this._curRow = e.Row;
                //this._curCol = e.Col;
                this.行坐标Label.Text = "Row:" + e.Row.ToString();
                this.列坐标Label.Text = "Col:" + e.Col.ToString();
            }
            catch
            {
            }
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
                    if (UserLoginParamManager.Instance.CurrentUser == enUserName.操作员) return;
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
            DialogResult dialogResult = new Common.UserMessageForm().ShowDialog("确定关闭窗体吗？", "关闭窗体");
            if (dialogResult == DialogResult.OK)
            {
                ViewConfigParamManager.Instance.ViewParamList.Remove(this._viewConfigParam);
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
                this._viewConfigParam.Location = this.Parent.Location;
                this._viewConfigParam.FormSize = this.Parent.Size;
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

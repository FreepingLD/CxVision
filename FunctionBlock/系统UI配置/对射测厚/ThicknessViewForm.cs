
using AlgorithmsLibrary;
using Common;
using HalconDotNet;
using MotionControlCard;
using Sensor;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using View;

namespace FunctionBlock
{
    public partial class ThicknessViewForm : Form
    {
        private List<ImageDataClass> imageList = new List<ImageDataClass>();
        private VisualizeView drawObject;
        private ViewConfigParam _viewConfigParam;
        private bool IsLoad = false; // 窗体加载后，设置为true
        private AcqSource acqSource;
        private List<object> DataList = new List<object>();
        private SocketCommand command;
        private HImage sourceImage;
        private BindingList<TrackMoveParam> _trackParam;
        private userWcsCoordSystem wcsCoordSystem = new userWcsCoordSystem();
        private bool IsDraw = false;
        private OperateParam currentParam;
        private bool RunState = false;
        public ThicknessViewForm(ViewConfigParam viewConfigParam)
        {
            InitializeComponent();
            /////////////
            this.drawObject = new userDrawPointROI(this.hWindowControl1, false);
            this.drawObject.HMouseDoubleClick += new HMouseEventHandler(this.hWindowControl1_DoubleClick);
            BaseFunction.ExcuteCompleted += new ExcuteCompletedEventHandler(DisplayExcuteResult);
            this.drawObject.GrayValueInfo += new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
            TreeViewWrapClass.ClickNode += new ClickNodeEventHandler(this.DisplayClickObject);
            //UserLoginParamManager.Instance.LoginParam.UserChange += new EventHandler(this.UserChange_Event);
            UserLoginParamManager.Instance.UserChange += new EventHandler(this.UserChange_Event);
            this._viewConfigParam = viewConfigParam;
            if (!HWindowManage.HWindowList.ContainsKey(viewConfigParam.ViewName))
            {
                HWindowManage.HWindowList.Add(viewConfigParam.ViewName, this.hWindowControl1.HalconWindow);
            }
            this.titleLabel.Text = viewConfigParam.ViewName;
            this.titleLabel.TextAlign = ContentAlignment.MiddleLeft;
            this.ContextMenu = new ContextMenu();

            this.MoveCol.Items.Clear();
            this.MoveCol.ValueType = typeof(enMoveType);
            foreach (enMoveType temp in Enum.GetValues(typeof(enMoveType)))
                this.MoveCol.Items.Add(temp);
            this.FunctionCol.Items.Clear();
            this.FunctionCol.ValueType = typeof(enFunction);
            foreach (enFunction temp in Enum.GetValues(typeof(enFunction)))
                this.FunctionCol.Items.Add(temp);
        }
        private void ThicknessViewForm_Load(object sender, EventArgs e)
        {
            //////////////
            BindProperty();
            //////////////////////////////////////
            this.Location = this._viewConfigParam.Location;
            this.Size = this._viewConfigParam.FormSize;
            this.SignLabel.Text = this._viewConfigParam.Tag;
            this.IsLoad = true;
            this.addContextMenu();
            this.addContextMenu(this.hWindowControl1);
            //this.addDataGridViewContextMenu(this.dataGridView2);
            //this.DisplayData();
            this.addDataGridViewContextMenu(this.dataGridView1);
        }
        private void BindProperty()
        {
            try
            {
                this.传感器comboBox1.Items.Clear();
                this.传感器comboBox1.Items.AddRange(AcqSourceManage.Instance.GetAcqSourceName());
                this.坐标系名称comboBox.Items.Clear();
                this.坐标系名称comboBox.Items.AddRange(Enum.GetNames(typeof(enCoordSysName)));
                this.激光采集源1comboBox.Items.Clear();
                this.激光采集源1comboBox.Items.AddRange(AcqSourceManage.Instance.GetAcqSourceName());
                this.激光采集源2comboBox.Items.Clear();
                this.激光采集源2comboBox.Items.AddRange(AcqSourceManage.Instance.GetAcqSourceName());
                this.传感器comboBox1.Text = this._viewConfigParam.CamName;
                this.程序节点comboBox.Text = this._viewConfigParam.ProgramNode;
                /////////////////////////////////////////////////
                OperateManager.Instance.Read();
                if (OperateManager.Instance.ProgramList.Count > 0)
                {
                    this.currentParam = OperateManager.Instance.GetParam(OperateManager.Instance.ProgramList[0].CurrentProductSize); // CurrentProductSize： 表示当前产品尺寸
                    if (this.currentParam == null)
                        this.currentParam = OperateManager.Instance.ProgramList[0]; // CurrentProductSize： 表示当前产品尺寸
                }
                else
                {
                    this.currentParam = new OperateParam();
                    OperateManager.Instance.ProgramList.Add(currentParam);
                }
                this._trackParam = this.currentParam.TrackParam;
                this.dataGridView1.DataSource = _trackParam;
                ////////////////////////////////////////
                this.操作员comboBox.DataBindings.Add(nameof(this.操作员comboBox.Text), currentParam, nameof(currentParam.Operate), true, DataSourceUpdateMode.OnPropertyChanged);
                this.产品尺寸comboBox.DataBindings.Add(nameof(this.产品尺寸comboBox.Text), currentParam, nameof(currentParam.ProductSize), true, DataSourceUpdateMode.OnPropertyChanged);
                this.产品标识号textBox.DataBindings.Add(nameof(this.产品标识号textBox.Text), currentParam, nameof(currentParam.ProductID), true, DataSourceUpdateMode.OnPropertyChanged);
                this.多文件目录textBox.DataBindings.Add(nameof(this.多文件目录textBox.Text), currentParam, nameof(currentParam.SaveDirect), true, DataSourceUpdateMode.OnPropertyChanged);
                //////////////////////  附加参数 
                this.坐标系名称comboBox.DataBindings.Add(nameof(this.坐标系名称comboBox.Text), currentParam, nameof(currentParam.CoordSysName), true, DataSourceUpdateMode.OnPropertyChanged);
                this.激光采集源1comboBox.DataBindings.Add(nameof(this.激光采集源1comboBox.Text), currentParam, nameof(currentParam.AcqSourceName1), true, DataSourceUpdateMode.OnPropertyChanged);
                this.激光采集源2comboBox.DataBindings.Add(nameof(this.激光采集源2comboBox.Text), currentParam, nameof(currentParam.AcqSourceName2), true, DataSourceUpdateMode.OnPropertyChanged);
                this.标准值textBox.DataBindings.Add(nameof(this.标准值textBox.Text), currentParam, nameof(currentParam.StdValue), true, DataSourceUpdateMode.OnPropertyChanged);
                this.上偏差textBox.DataBindings.Add(nameof(this.上偏差textBox.Text), currentParam, nameof(currentParam.UpTolerance), true, DataSourceUpdateMode.OnPropertyChanged);
                this.下偏差textBox.DataBindings.Add(nameof(this.下偏差textBox.Text), currentParam, nameof(currentParam.DownTolerance), true, DataSourceUpdateMode.OnPropertyChanged);
                /////
                this.校准块厚度textBox.DataBindings.Add("Text", GlobalVariable.pConfig, "StandardThickValue", true, DataSourceUpdateMode.OnPropertyChanged);
                this.校准值textBox.DataBindings.Add("Text", GlobalVariable.pConfig, "Cord_Gap", true, DataSourceUpdateMode.OnPropertyChanged);
                //////////////////////////////////
                this.中心XtextBox.DataBindings.Add("Text", currentParam, "Center_X", true, DataSourceUpdateMode.OnPropertyChanged);
                this.中心YtextBox.DataBindings.Add("Text", currentParam, "Center_Y", true, DataSourceUpdateMode.OnPropertyChanged);

                this.厚度系数textBox.DataBindings.Add("Text", currentParam, "ThickScale", true, DataSourceUpdateMode.OnPropertyChanged);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        public void ButtonClick(object sender, ButonClickEventArgs e)
        {
            try
            {
                if (this._viewConfigParam.CamName == e.ClickInfo)
                {
                    if (this.imageList.Count > e.BtnIndex)
                        this.drawObject.BackImage = this.imageList[e.BtnIndex];
                }
            }
            catch
            {
            }
        }
        public void UserChange_Event(object sender, EventArgs e)
        {
            try
            {
                UserLoginParam loginParam = sender as UserLoginParam;
                switch (loginParam.User)
                {
                    case enUserName.操作员:
                        this.传感器comboBox1.Enabled = false;
                        this.程序节点comboBox.Enabled = false;
                        this.buttonClose.Enabled = false;
                        break;
                    case enUserName.工程师:
                        this.传感器comboBox1.Enabled = true;
                        this.程序节点comboBox.Enabled = true;
                        this.buttonClose.Enabled = false;
                        break;
                    case enUserName.开发人员:
                        this.传感器comboBox1.Enabled = true;
                        this.程序节点comboBox.Enabled = true;
                        this.buttonClose.Enabled = true;
                        break;
                }
            }
            catch
            {
            }
        }
        public void AxisINPose(object sender, PoseInfoEventArgs e)
        {
            try
            {
                //command = SocketCommand.GetSocketCommand(e.PoseInfo);
                if (this._viewConfigParam.CamName.Contains(command.CamStation))
                {
                    this.imageList?.Clear();
                }
            }
            catch
            {
            }
        }
        private void ClearGraphic(object send, EventArgs e)
        {
            this.DataList.Clear();
            this.drawObject.AttachPropertyData.Clear();
            this.drawObject.DrawingGraphicObject(); // 背影不刷新
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
        private bool IsJudge()
        {
            if (this._viewConfigParam.ViewName.Contains(this.command.GrabNo.ToString()))
                return true;
            else
                return false;
        }
        private void DisplayExcuteResult(object sender, ExcuteCompletedEventArgs e)
        {
            if (e.SensorName == null) return;
            if (!this._viewConfigParam.CamName.Contains(e.SensorName)) return;
            if (e.ViewWindow == null || e.ViewWindow != this._viewConfigParam.ViewName) return;
            if (!IsSelect()) return;  // 只有在当前选项页时才显示
            if (e.DataContent != null)
            {
                switch (e.DataContent.GetType().Name)
                {
                    case nameof(ImageDataClass):
                        this.ClearGraphic(null, null);
                        this.DataList.Clear();
                        this.drawObject.ClearViewObject(); // 更新图像时清空
                        this.drawObject.BackImage = (ImageDataClass)e.DataContent;
                        if (this._viewConfigParam.IsShowCross)
                            this.drawObject.AddViewObject(new ViewData(this.GenCrossLine(this.drawObject.BackImage.Image), "green"));
                        /////////////////////////////////////////////////////////////////////////////////////////////
                        break;
                    case nameof(RegionDataClass):
                        this.drawObject.AddViewObject(new ViewData(((RegionDataClass)e.DataContent).Region, "red"));
                        break;
                    case nameof(HXLDCont):
                        this.drawObject.AddViewObject(new ViewData((HXLDCont)e.DataContent, "red"));
                        break;
                    case nameof(XldDataClass):
                        this.drawObject.AddViewObject(new ViewData(((XldDataClass)e.DataContent).HXldCont, "red"));
                        break;
                    case nameof(userWcsPoint):
                        userWcsPoint wcsPoint = (userWcsPoint)e.DataContent;
                        this.drawObject.AddViewObject(new ViewData((wcsPoint).GetPixPoint().GetXLD(), wcsPoint.Color.ToString()));
                        break;
                    case nameof(userWcsLine):
                        userWcsLine wcsLine = (userWcsLine)e.DataContent;
                        this.drawObject.AddViewObject(new ViewData((wcsLine).GetPixLine().GetXLD(), wcsLine.Color.ToString()));
                        break;
                    case nameof(userWcsRectangle2):
                        userWcsRectangle2 wcsRect2 = (userWcsRectangle2)e.DataContent;
                        this.drawObject.AddViewObject(new ViewData((wcsRect2).GetPixRectangle2().GetXLD(), wcsRect2.Color.ToString()));
                        break;
                    case nameof(userWcsRectangle1):
                        userWcsRectangle1 wcsRect1 = (userWcsRectangle1)e.DataContent;
                        this.drawObject.AddViewObject(new ViewData((wcsRect1).GetPixRectangle1().GetXLD(), wcsRect1.Color.ToString()));
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
                    case nameof(userWcsCoordSystem):
                        userWcsCoordSystem wcsCoordSystem = (userWcsCoordSystem)e.DataContent;
                        this.wcsCoordSystem = wcsCoordSystem;
                        this.drawObject.AddViewObject(new ViewData(wcsCoordSystem, "red"));
                        break;
                    case nameof(userOkNgText):
                        this.drawObject.AddViewObject(new ViewData(e.DataContent, "red"));
                        break;
                    //case nameof(userTextLable):
                    //    userTextLable textLable = e.DataContent as userTextLable;
                    //    //textLable.x = 
                    //    //this.drawObject.AddViewObject(new ViewData(e.DataContent, "red"));
                    //    break;
                    case nameof(userWcsThick):
                        userTextLable textLable;
                        userPixThick pixThick;
                        userWcsThick wcsThick = e.DataContent as userWcsThick;
                        wcsThick.CamParams = this.drawObject.CameraParam;
                        pixThick = wcsThick.GetPixThick();
                        if (wcsThick.Result == "OK")
                        {
                            this.drawObject.AddViewObject(new ViewData(pixThick, "green"));
                            textLable = new userTextLable(string.Format("Dist1={0},Dist2={1},Thick={2}", pixThick.Dist1, pixThick.Dist2, pixThick.Thick), pixThick.Col, pixThick.Row, 10, "green");
                        }
                        else
                        {
                            this.drawObject.AddViewObject(new ViewData(pixThick, "red"));
                            textLable = new userTextLable(string.Format("Dist1={0},Dist2={1},Thick={2}", pixThick.Dist1, pixThick.Dist2, pixThick.Thick), pixThick.Col, pixThick.Row, 10, "red");
                        }
                        this.drawObject.AddViewObject(new ViewData(textLable, "green"));
                        break;

                    default:
                        this.drawObject.AddViewObject(new ViewData(e.DataContent, "red"));
                        break;
                }
            }
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
                new ToolStripMenuItem("移动到选定位置"),
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
                ((ContextMenuStrip)sender).Close();
                switch (name)
                {
                    case "删除":
                        ((ContextMenuStrip)sender).Close();
                        if (this.dataGridView1.CurrentRow != null)
                            index = this.dataGridView1.CurrentRow.Index;
                        this._trackParam.RemoveAt(index);
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
                        this._trackParam.Clear();
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
                                    switch (this._trackParam[index].RoiShape.GetType().Name)
                                    {
                                        case nameof(drawWcsPoint):
                                            this.Invoke(new Action(() => { this._trackParam.Add(new TrackMoveParam(((drawWcsPoint)this._trackParam[index].RoiShape).AffineTransWcsPoint(hHomMat_tras), enMoveType.点位运动)); }));
                                            break;
                                        case nameof(drawWcsLine):
                                            this.Invoke(new Action(() => { this._trackParam.Add(new TrackMoveParam(((drawWcsLine)this._trackParam[index].RoiShape).AffineTransWcsLine(hHomMat_tras), enMoveType.直线运动)); }));
                                            break;
                                        case nameof(drawWcsCircle):
                                            this.Invoke(new Action(() => { this._trackParam.Add(new TrackMoveParam(((drawWcsCircle)this._trackParam[index].RoiShape).AffineTransWcsCircle(hHomMat_tras), enMoveType.圆运动)); }));
                                            break;
                                        case nameof(drawWcsEllipse):
                                            this.Invoke(new Action(() => { this._trackParam.Add(new TrackMoveParam(((drawWcsEllipse)this._trackParam[index].RoiShape).AffineTransWcsCircle(hHomMat_tras), enMoveType.椭圆运动)); }));
                                            break;
                                        case nameof(drawWcsRect1):
                                            this.Invoke(new Action(() => { this._trackParam.Add(new TrackMoveParam(((drawWcsRect1)this._trackParam[index].RoiShape).AffineTransWcsRect1(hHomMat_tras), enMoveType.矩形1运动)); }));
                                            break;
                                        case nameof(drawWcsRect2):
                                            this.Invoke(new Action(() => { this._trackParam.Add(new TrackMoveParam(((drawWcsRect2)this._trackParam[index].RoiShape).AffineTransWcsRect2(hHomMat_tras), enMoveType.矩形2运动)); }));
                                            break;
                                        case nameof(drawWcsPolygon):
                                            this.Invoke(new Action(() => { this._trackParam.Add(new TrackMoveParam(((drawWcsPolygon)this._trackParam[index].RoiShape).AffineTransWcsPolygon(hHomMat_tras), enMoveType.多边形运动)); }));
                                            break;
                                    }
                                    Thread.Sleep(100);
                                }
                            }
                        });
                        rectform.Close();
                        break;
                    case "圆形阵列":
                        index = this.dataGridView1.CurrentRow.Index;
                        if (index < 0) return;
                        CircleArrayDataForm circleForm = new CircleArrayDataForm();
                        circleForm.Owner = this;
                        circleForm.ShowDialog();
                        circleForm.Ref_X = this.currentParam.Center_X; // 赋值阵列中心
                        circleForm.Ref_Y = this.currentParam.Center_Y; // 赋值阵列中心
                        hHomMat2D = new HHomMat2D();
                        HHomMat2D hHomMat_Rota;
                        Task.Run(() =>
                        {
                            for (int i = 0; i < circleForm.ArrayNum; i++)
                            {
                                if (i == 0) continue; //选定点不变
                                // 以当前点为圆上的点来阵列
                                hHomMat_Rota = hHomMat2D.HomMat2dRotate(circleForm.Add_Deg * i * Math.PI / 180, circleForm.Ref_X, circleForm.Ref_Y);
                                switch (this._trackParam[index].RoiShape.GetType().Name)
                                {
                                    case nameof(drawWcsPoint):
                                        this.Invoke(new Action(() => { this._trackParam.Add(new TrackMoveParam(((drawWcsPoint)this._trackParam[index].RoiShape).AffineTransWcsPoint(hHomMat_Rota), enMoveType.点位运动)); }));
                                        break;
                                    case nameof(drawWcsLine):
                                        this.Invoke(new Action(() => { this._trackParam.Add(new TrackMoveParam(((drawWcsLine)this._trackParam[index].RoiShape).AffineTransWcsLine(hHomMat_Rota), enMoveType.直线运动)); }));
                                        break;
                                    case nameof(drawWcsCircle):
                                        this.Invoke(new Action(() => { this._trackParam.Add(new TrackMoveParam(((drawWcsCircle)this._trackParam[index].RoiShape).AffineTransWcsCircle(hHomMat_Rota), enMoveType.圆运动)); }));
                                        break;
                                    case nameof(drawWcsEllipse):
                                        this.Invoke(new Action(() => { this._trackParam.Add(new TrackMoveParam(((drawWcsEllipse)this._trackParam[index].RoiShape).AffineTransWcsCircle(hHomMat_Rota), enMoveType.椭圆运动)); }));
                                        break;
                                    case nameof(drawWcsRect1):
                                        this.Invoke(new Action(() => { this._trackParam.Add(new TrackMoveParam(((drawWcsRect1)this._trackParam[index].RoiShape).AffineTransWcsRect1(hHomMat_Rota), enMoveType.矩形1运动)); }));
                                        break;
                                    case nameof(drawWcsRect2):
                                        this.Invoke(new Action(() => { this._trackParam.Add(new TrackMoveParam(((drawWcsRect2)this._trackParam[index].RoiShape).AffineTransWcsRect2(hHomMat_Rota), enMoveType.矩形2运动)); }));
                                        break;
                                    case nameof(drawWcsPolygon):
                                        this.Invoke(new Action(() => { this._trackParam.Add(new TrackMoveParam(((drawWcsPolygon)this._trackParam[index].RoiShape).AffineTransWcsPolygon(hHomMat_Rota), enMoveType.多边形运动)); }));
                                        break;
                                }
                            }
                        });
                        break;
                    case "移动到选定位置":
                        index = this.dataGridView1.CurrentRow.Index;
                        if (index < 0 || index > this.currentParam.TrackParam.Count) return;
                        MoveCommandParam CommandParam = new MoveCommandParam();
                        IMotionControl _Card = MotionCardManage.GetCard(this.currentParam.CoordSysName);
                        RobotJawParam jaw = RobotJawParaManager.Instance.GetJawParam(nameof(enRobotJawEnum.Jaw1));
                        switch (this.currentParam.TrackParam[index].MoveType)
                        {
                            case enMoveType.点位运动:
                                drawWcsPoint wcsPoint = this.currentParam.TrackParam[index].RoiShape as drawWcsPoint;
                                if (wcsPoint == null)
                                    throw new ArgumentException("指定的轨迹类型不为点!");
                                CommandParam.MoveType = this.currentParam.TrackParam[index].MoveType;
                                CommandParam.MoveSpeed = GlobalVariable.pConfig.MoveSpeed;
                                CommandParam.MoveAxis = enAxisName.XY轴;
                                CommandParam.CoordSysName = this.currentParam.CoordSysName;      // 运动坐标系里是否需要包含移动指令
                                CommandParam.AxisParam = new CoordSysAxisParam(wcsPoint.X + jaw.X, wcsPoint.Y + jaw.Y, 0, 0, 0, 0);
                                CommandParam.AxisParam2 = new CoordSysAxisParam(wcsPoint.X + jaw.X, wcsPoint.Y + jaw.Y, 0, 0, 0, 0);
                                ////////////////////////////////////////////////////////////
                                _Card?.MoveMultyAxis(CommandParam);
                                break;
                            case enMoveType.直线运动:
                                drawWcsLine wcsLinbe = this.currentParam.TrackParam[index].RoiShape as drawWcsLine;
                                if (wcsLinbe == null)
                                    throw new ArgumentException("指定的轨迹类型不为点!");
                                CommandParam.MoveType = this.currentParam.TrackParam[index].MoveType;
                                CommandParam.MoveSpeed = GlobalVariable.pConfig.MoveSpeed;
                                CommandParam.MoveAxis = enAxisName.XY轴;
                                CommandParam.CoordSysName = this.currentParam.CoordSysName;      // 运动坐标系里是否需要包含移动指令
                                CommandParam.AxisParam = new CoordSysAxisParam(wcsLinbe.X1 + jaw.X, wcsLinbe.Y1 + jaw.Y, 0, 0, 0, 0);
                                CommandParam.AxisParam2 = new CoordSysAxisParam(wcsLinbe.X1 + jaw.X, wcsLinbe.Y1 + jaw.Y, 0, 0, 0, 0);
                                ////////////////////////////////////////////////////////////
                                _Card?.MoveMultyAxis(CommandParam);
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
        #endregion
        /// <summary>
        /// 窗体移动事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ThicknessViewForm_Move(object sender, EventArgs e)
        {
            if (this._viewConfigParam != null)
                this._viewConfigParam.Location = this.Location;
        }

        /// <summary>
        /// 窗体尺寸改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ThicknessViewForm_Resize(object sender, EventArgs e)
        {
            if (this._viewConfigParam != null && this.IsLoad) // 在窗体加载后这个事件才起作用
                this._viewConfigParam.FormSize = this.Size;
        }

        private void ThicknessViewForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                this.cts?.Cancel();
                this.IsLoad = false;
                ViewConfigParamManager.Instance.ViewParamList.Remove(this._viewConfigParam); // 关闭窗体时要删除相应的对象
                BaseFunction.ExcuteCompleted -= new ExcuteCompletedEventHandler(DisplayExcuteResult);
                this.drawObject.GrayValueInfo -= new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                this.drawObject.HMouseDoubleClick -= new HMouseEventHandler(this.hWindowControl1_DoubleClick);
                TreeViewWrapClass.ClickNode -= new ClickNodeEventHandler(this.DisplayClickObject);
                //UserLoginParamManager.Instance.LoginParam.UserChange -= new EventHandler(this.UserChange_Event);
                UserLoginParamManager.Instance.UserChange -= new EventHandler(this.UserChange_Event);
                if (this.drawObject != null)
                    this.drawObject.ClearDrawingObject();
            }
            catch
            {

            }
        }
        public void DisplayClickObject(object sender, TreeNodeMouseClickEventArgs e)  //
        {
            if (!IsSelect()) return; // 如果不是当前选择的，则返回
            if (e.Node.Tag == null) return;
            if (e.Button != MouseButtons.Left) return; // 点击右键时不变
            try
            {
                switch (e.Node.Tag.GetType().Name)
                {
                    case nameof(ThicknessMeasure):
                        this.dataGridView1.DataSource = ((ThicknessMeasure)e.Node.Tag).TrackParam;
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

        #region  数据实时采集
        private CancellationTokenSource cts;
        private void 实时采集checkBox_CheckedChanged(object sender, EventArgs e)
        {
            switch (this.实时采集checkBox.CheckState)
            {
                case CheckState.Checked:
                    this.实时采集checkBox.BackColor = Color.Red;
                    this.acqSource = AcqSourceManage.Instance.GetCamAcqSource(this._viewConfigParam.CamName);
                    if (this.acqSource == null) return;
                    cts = new CancellationTokenSource();
                    Dictionary<enDataItem, object> data;
                    Task.Run(() =>
                    {
                        this.drawObject.IsLiveState = true;
                        while (!cts.IsCancellationRequested)
                        {
                            if (SystemParamManager.Instance.SysConfigParam.IsAutoRun)
                                cts.Cancel();
                            /////////////////////////////////////////////////////////
                            switch (this.acqSource.Sensor?.ConfigParam.SensorType)
                            {
                                case enUserSensorType.面阵相机:
                                    data = this.acqSource.AcqImageData(null);
                                    if (data?.Count > 0)
                                    {
                                        this.drawObject.BackImage = (ImageDataClass)data[enDataItem.Image];
                                        this.drawObject.ClearViewObject();
                                        if (this._viewConfigParam.IsShowCross)
                                            this.drawObject.AddViewObject(new ViewData(this.GenCrossLine(this.drawObject.BackImage.Image), "green"));
                                        this.sourceImage = this.drawObject?.BackImage?.Image;
                                    }
                                    break;
                                case enUserSensorType.点激光:
                                    data = this.acqSource.AcqPointData();
                                    if (data?.Count > 0)
                                    {
                                        double[] dist1 = (double[])data[enDataItem.Dist1];
                                        if (dist1 != null && dist1.Length > 0)
                                        {
                                            this.drawObject.ClearViewObject();
                                            if (dist1 != null && dist1.Length > 0)
                                                this.drawObject.AddViewObject(new ViewData(new userTextLable("距离 = " + dist1.Average().ToString("f5"), 0, 0, 25, "red", enLablePosition.左上角), "green"));
                                        }
                                    }
                                    break;
                            }
                            Thread.Sleep(100);
                        }
                    });
                    break;
                default:
                    this.drawObject.IsLiveState = false;
                    cts?.Cancel();
                    this.实时采集checkBox.BackColor = Color.Lime;
                    break;
            }
        }
        #endregion


        #region 标签右键菜单项
        private void addContextMenu()
        {
            ContextMenuStrip ContextMenuStrip1 = new ContextMenuStrip();
            // 添加右键菜单 
            ToolStripItem[] items = new ToolStripMenuItem[]
            {
                new ToolStripMenuItem("重命名"),
            };
            ContextMenuStrip1.Items.AddRange(items); // 在滑赋值前不能调用 
            ContextMenuStrip1.ItemClicked += new ToolStripItemClickedEventHandler(label1ContextMenuStrip_ItemClicked);
            this.SignLabel.ContextMenuStrip = ContextMenuStrip1;
        }
        private void label1ContextMenuStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            string name = e.ClickedItem.Text;
            try
            {
                ((ContextMenuStrip)sender).Close();
                switch (name)
                {
                    default:
                    case "重命名":
                        RenameForm renameForm = new RenameForm(this.SignLabel.Text);
                        renameForm.ShowDialog();
                        this._viewConfigParam.Tag = renameForm.ReName;
                        renameForm.Dispose();
                        this.SignLabel.Text = this._viewConfigParam.Tag;
                        break;
                        ///////////////////////////////////////////////                 
                }
            }
            catch
            {
            }
        }
        #endregion

        #region 窗口右键菜单项
        private void ClearContextMenu(HWindowControl hWindowControl)
        {
            if (hWindowControl.ContextMenuStrip != null)
                hWindowControl.ContextMenuStrip = null;
        }
        private void addContextMenu(HWindowControl hWindowControl)
        {
            ContextMenuStrip ContextMenuStrip1 = new ContextMenuStrip();
            ContextMenuStrip1.Name = hWindowControl.Name;
            // 添加右键菜单 
            ToolStripItem[] items = new ToolStripMenuItem[]
            {
             new ToolStripMenuItem("自适应图像(Auto)"),
             new ToolStripMenuItem("编辑程序"),
             new ToolStripMenuItem("设置曝光"),
             new ToolStripMenuItem("设置增益"),
             new ToolStripMenuItem("设置光源亮度"),
             new ToolStripMenuItem("十字线"),
             new ToolStripMenuItem("3D(View)"),
             new ToolStripMenuItem("保存图像") ,
             new ToolStripMenuItem("保存点云"),
             new ToolStripMenuItem("清除窗口(Clear)"),
            };
            if (this._viewConfigParam.IsShowCross)
                items[5].Text = "隐藏十字线";
            else
                items[5].Text = "显示十字线";
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
                    case "自适应图像(Auto)":
                        this.drawObject.AutoImage();
                        break;
                    case "编辑程序":
                        foreach (KeyValuePair<string, TreeViewWrapClass> item in ProgramForm.Instance.ProgramDic)
                        {
                            foreach (TreeNode item2 in item.Value.TreeView.Nodes)
                            {
                                //if (item2.Text == this._viewConfigParam.ProgramNode)
                                //    item.Value.treeView1_Edite(this, item2);
                                TreeNode node;
                                GetEditeNode(item2, out node);
                                if (node != null)
                                {
                                    item.Value.treeView1_Edite(this, node);
                                    break;
                                }
                            }
                        }
                        break;
                    case "设置曝光":
                        if (AcqSourceManage.Instance.GetAcqSource(this._viewConfigParam.CamName) == null || AcqSourceManage.Instance.GetAcqSource(this._viewConfigParam.CamName).Sensor == null) return;
                        string value = AcqSourceManage.Instance.GetAcqSource(this._viewConfigParam.CamName).Sensor.GetParam("曝光").ToString();
                        RenameForm renameForm = new RenameForm(value);
                        renameForm.ShowDialog();
                        AcqSourceManage.Instance.GetAcqSource(this._viewConfigParam.CamName).Sensor.SetParam("曝光", renameForm.ReName);
                        break;
                    case "设置增益":
                        if (AcqSourceManage.Instance.GetAcqSource(this._viewConfigParam.CamName) == null || AcqSourceManage.Instance.GetAcqSource(this._viewConfigParam.CamName).Sensor == null) return;
                        value = AcqSourceManage.Instance.GetAcqSource(this._viewConfigParam.CamName).Sensor.GetParam("增益").ToString();
                        renameForm = new RenameForm(value);
                        renameForm.ShowDialog();
                        AcqSourceManage.Instance.GetAcqSource(this._viewConfigParam.CamName).Sensor.SetParam("增益", renameForm.ReName);
                        break;
                    case "设置光源亮度":
                        if (AcqSourceManage.Instance.GetAcqSource(this._viewConfigParam.CamName) == null) return;
                        LightSetForm lightForm = new LightSetForm(AcqSourceManage.Instance.GetAcqSource(this._viewConfigParam.CamName));
                        lightForm.ShowDialog();
                        break;
                    case "十字线":
                    case "隐藏十字线":
                    case "显示十字线":
                        if (this._viewConfigParam.IsShowCross)
                        {
                            e.ClickedItem.Text = "显示十字线";
                            this._viewConfigParam.IsShowCross = false;
                        }
                        else
                        {
                            e.ClickedItem.Text = "隐藏十字线";
                            this._viewConfigParam.IsShowCross = true;
                        }
                        ViewConfigParamManager.Instance.Save();
                        break;
                    case "3D(View)":
                        break;
                    case "保存图像":
                        SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                        saveFileDialog1.Filter = "bmp files (*.bmp)|*.bmp|All files (*.*)|*.*";
                        saveFileDialog1.FilterIndex = 0;
                        saveFileDialog1.ShowDialog();
                        if (this.drawObject.BackImage != null && this.drawObject.BackImage.Image.IsInitialized())
                            this.drawObject.BackImage.Image.WriteImage("bmp", 0, saveFileDialog1.FileName);
                        else
                        {
                            if (saveFileDialog1.FileName != null && saveFileDialog1.FileName.Length > 0)
                                MessageBox.Show("图像内容为空");
                        }

                        break;
                    case "保存点云":
                        saveFileDialog1 = new SaveFileDialog();
                        saveFileDialog1.Filter = "ply files (*.ply)|*.ply|txt files (*.txt)|*.txt|om3 files (*.om3)|*.om3|stl files (*.stl)|*.stl|obj files (*.obj)|*.obj|dxf files (*.dxf)|*.dxf|All files (*.*)|*.*";
                        saveFileDialog1.FilterIndex = 3;
                        saveFileDialog1.ShowDialog();
                        if (this.drawObject.PointCloudModel3D != null)
                        {
                            HObjectModel3D hObjectModel3D = HObjectModel3D.UnionObjectModel3d(this.drawObject.PointCloudModel3D.ObjectModel3D, "points_surface");
                            hObjectModel3D.WriteObjectModel3d(new FileInfo(saveFileDialog1.FileName).Extension, saveFileDialog1.FileName, new HTuple(), new HTuple());
                            hObjectModel3D.Dispose();
                        }
                        else
                        {
                            if (saveFileDialog1.FileName != null && saveFileDialog1.FileName.Length > 0)
                                MessageBox.Show("点云句柄内容为空");
                        }
                        break;
                    case "清除窗口(Clear)":
                        this.drawObject.ClearWindow();
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
            if (e.GaryValue.Length > 0)
            {
                this.hWindowControl1.HalconWindow.SetTposition((int)(e.Row), (int)e.Col);
                this.hWindowControl1.HalconWindow.SetFont("-Consolas-" + 10 + "- *-0-*-*-1-");
                this.hWindowControl1.HalconWindow.SetColor("red");

                this.SignLabel.Text = string.Join("", "Row:", e.Row, " ", "Col:", e.Col, " ", "Gray:", e.GaryValue[0]);
                //this.hWindowControl1.HalconWindow.WriteString(string.Join("","Row:", e.Row, " ", "Col:", e.Col, " ", "Gray:",e.GaryValue[0]));
                //this.drawObject.CopyBufferWindowView();
            }
        }

        #endregion

        private HXLDCont GenCrossLine(HImage hImage)
        {
            HXLDCont hXLDCont = new HXLDCont();
            if (hImage != null && hImage.IsInitialized())
            {
                hXLDCont.GenEmptyObj();
                int width, height;
                hImage.GetImageSize(out width, out height);
                hXLDCont = hXLDCont.ConcatObj(new HXLDCont(new HTuple(height * 0.5, height * 0.5), new HTuple(0, width)));
                hXLDCont = hXLDCont.ConcatObj(new HXLDCont(new HTuple(0, height), new HTuple(width * 0.5, width * 0.5)));
            }
            return hXLDCont;
        }

        private void ThicknessViewForm_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, WM_SYSCOMMAND, SC_MOVE + HTCAPTION, 0);
        }

        private void titleLabel_MouseDown(object sender, MouseEventArgs e)
        {
            ThicknessViewForm_MouseDown(null, null);
        }

        private void titleLabel_MouseEnter(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Hand;
            this.titleLabel.BackColor = System.Drawing.Color.Orange;// System.Drawing.SystemColors.HotTrack;
        }

        private void titleLabel_MouseLeave(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Default;
            this.titleLabel.BackColor = System.Drawing.Color.LightGray;// System.Drawing.SystemColors.Control;
        }

        private void 传感器comboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            try
            {
                if (this.IsLoad)
                {
                    this.cts?.Cancel();
                    this._viewConfigParam.CamName = this.传感器comboBox1.SelectedItem.ToString();
                    ViewConfigParamManager.Instance.Save();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("保存视图配置参数出错" + ex.ToString());
            }
        }

        private void hWindowControl1_DoubleClick(Object sender, HalconDotNet.HMouseEventArgs e)
        {
            this.buttonMax_Click(null, null);
        }

        private void 程序节点comboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            try
            {
                if (this.程序节点comboBox.SelectedItem == null)
                    this.程序节点comboBox.SelectedItem = this._viewConfigParam.ProgramNode;
                else
                    this._viewConfigParam.ProgramNode = this.程序节点comboBox.SelectedItem.ToString();
                ViewConfigParamManager.Instance.Save();
            }
            catch (Exception ex)
            {
                MessageBox.Show("保存视图配置参数出错" + ex.ToString());
            }
        }

        private void 程序节点comboBox_DropDown(object sender, EventArgs e)
        {
            int a = 10;
            this.程序节点comboBox.Items.Clear();
            foreach (KeyValuePair<string, TreeViewWrapClass> item in ProgramForm.Instance.ProgramDic)
            {
                foreach (TreeNode item2 in item.Value.TreeView.Nodes)
                {
                    GetToolNode(item2);
                    //if (item2.Name.Contains("Tool"))
                    //    this.程序节点comboBox.Items.Add(item2.Text);
                }
            }
        }

        private void GetToolNode(TreeNode node)
        {
            if (node.Name.Contains("Tool"))
            {
                this.程序节点comboBox.Items.Add(node.Text);
                foreach (TreeNode item in node.Nodes)
                {
                    GetToolNode(item);
                }
            }
        }

        private void GetEditeNode(TreeNode node, out TreeNode selectNode)
        {
            selectNode = null;
            if (node.Name.Contains("Tool"))
            {
                if (node.Text == this._viewConfigParam.ProgramNode)
                {
                    selectNode = node;
                    return;
                }
                else
                {
                    foreach (TreeNode item in node.Nodes)
                    {
                        if (selectNode == null)
                            GetEditeNode(item, out selectNode);
                    }
                }
            }
        }

        private void 添加点button_Click(object sender, EventArgs e)
        {
            try
            {
                TrackMoveParam trackMove = new TrackMoveParam(new drawWcsPoint());
                trackMove.MoveType = enMoveType.点位运动;
                this._trackParam.Add(trackMove);
                this._trackParam.Last().RoiShape = null;
                this.dataGridView1_CellContentClick(null, new DataGridViewCellEventArgs(0, this._trackParam.Count - 1));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void 添加线button_Click(object sender, EventArgs e)
        {
            try
            {
                TrackMoveParam trackMove = new TrackMoveParam(new drawWcsLine());
                trackMove.MoveType = enMoveType.直线运动;
                this._trackParam.Add(trackMove);
                this._trackParam.Last().RoiShape = null;
                this.dataGridView1_CellContentClick(null, new DataGridViewCellEventArgs(0, this._trackParam.Count - 1));
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
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void 视图工具toolStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            string name = e.ClickedItem.Name;
            switch (name)
            {
                case "toolStripButton_Clear":
                    this.drawObject.ClearWindow();
                    break;
                case "toolStripButton_Select":
                    this.drawObject.Select();
                    break;
                case "toolStripButton_Translate":
                    this.drawObject.TranslateScaleImage();
                    break;
                case "toolStripButton_Auto":
                    this.drawObject.AutoImage();
                    break;
                case "toolStripButton_3D":
                    this.drawObject.Show3D();
                    break;
                default:
                    break;
            }
        }

        private void 清空button_Click(object sender, EventArgs e)
        {
            try
            {
                this._trackParam.Clear();
                this.DataList.Clear();
                this.dataGridView1.Rows.Clear();
                this.drawObject.AttachPropertyData.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void 删除button_Click(object sender, EventArgs e)
        {
            try
            {
                int index = this.dataGridView1.CurrentRow.Index;
                if (index > 0)
                {
                    this._trackParam.RemoveAt(index);
                    this.DataList.RemoveAt(index);
                    this.dataGridView1.Rows.RemoveAt(index);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void 插入button_Click(object sender, EventArgs e)
        {
            try
            {
                int index = this.dataGridView1.CurrentRow.Index;
                if (index < 0) return;
                TrackMoveParam trackMove = new TrackMoveParam(new drawWcsPoint());
                trackMove.MoveType = enMoveType.点位运动;
                this._trackParam.Insert(index, trackMove);
                this._trackParam.Last().RoiShape = null;
                this.dataGridView1_CellContentClick(null, new DataGridViewCellEventArgs(0, this._trackParam.Count - 1));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private List<TreeNode> listTreeNode = new List<TreeNode>();

        public void Init()
        {
            ///// 获取程序视图节点
            foreach (var item in ProgramForm.Instance.ProgramDic.Values)
            {
                listTreeNode = item.GetTreeViewNodeTag();
            }
        }
        private TreeNode GetExecuteNode(string ExecuteNodeInfo)
        {
            TreeNode node = null;
            foreach (var item in listTreeNode)
            {
                if (item.Text.Contains(ExecuteNodeInfo))
                    node = item;
            }
            return node;
        }


        private void 采集图片Btn_Click(object sender, EventArgs e)
        {
            try
            {
                this.acqSource = AcqSourceManage.Instance.GetCamAcqSource(this._viewConfigParam.CamName);
                if (this.acqSource == null) return;
                cts = new CancellationTokenSource();
                Dictionary<enDataItem, object> data;
                data = this.acqSource.AcqImageData(null);
                switch (this.acqSource.Sensor?.ConfigParam.SensorType)
                {
                    case enUserSensorType.面阵相机:
                        if (data?.Count > 0)
                        {
                            this.drawObject.BackImage = (ImageDataClass)data[enDataItem.Image];
                            this.drawObject.AttachPropertyData.Clear();
                            this.sourceImage = this.drawObject?.BackImage?.Image;
                        }
                        break;
                }
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
                    this.addContextMenu(this.hWindowControl1);
                    switch (dataGridView1.Columns[e.ColumnIndex].Name)
                    {
                        case "TeachCol":
                            this.ClearContextMenu(this.hWindowControl1);
                            if (this.dataGridView1.Rows[e.RowIndex].DataBoundItem == null)
                            {
                                MessageBox.Show("未设置必需的图形参数，请先设置轨迹类型参数!");
                                return;
                            }
                            switch (this._trackParam[e.RowIndex].MoveType)
                            {
                                case enMoveType.矩形2运动:
                                    this.drawObject.AttachPropertyData.Clear();
                                    if (!(this.drawObject is userDrawRect2ROI))
                                    {
                                        this.drawObject.GrayValueInfo -= new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                                        this.drawObject.ClearDrawingObject();
                                        this.drawObject = new userDrawRect2ROI(this.hWindowControl1, false);
                                        this.drawObject.GrayValueInfo += new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                                    }
                                    this.drawObject.IsLiveState = true;
                                    break;
                                case enMoveType.矩形1运动:
                                    this.drawObject.AttachPropertyData.Clear();
                                    if (!(this.drawObject is userDrawRect1ROI))
                                    {
                                        this.drawObject.GrayValueInfo -= new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                                        this.drawObject.ClearDrawingObject();
                                        this.drawObject = new userDrawRect1ROI(this.hWindowControl1, false);
                                        this.drawObject.GrayValueInfo += new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                                    }
                                    this.drawObject.IsLiveState = true;
                                    break;
                                case enMoveType.圆运动:
                                    this.drawObject.AttachPropertyData.Clear();
                                    if (!(this.drawObject is userDrawCircleROI))
                                    {
                                        this.drawObject.GrayValueInfo -= new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                                        this.drawObject.ClearDrawingObject();
                                        this.drawObject = new userDrawCircleROI(this.hWindowControl1, false);
                                        this.drawObject.GrayValueInfo += new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                                    }
                                    this.drawObject.IsLiveState = true;
                                    break;
                                case enMoveType.椭圆运动:
                                    this.drawObject.AttachPropertyData.Clear();
                                    if (!(this.drawObject is userDrawEllipseROI))
                                    {
                                        this.drawObject.GrayValueInfo -= new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                                        this.drawObject.ClearDrawingObject();
                                        this.drawObject = new userDrawEllipseROI(this.hWindowControl1, false);
                                        this.drawObject.GrayValueInfo += new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                                    }
                                    this.drawObject.IsLiveState = true;
                                    break;
                                case enMoveType.多边形运动:
                                    this.drawObject.AttachPropertyData.Clear();
                                    if (!(this.drawObject is userDrawPolygonROI))
                                    {
                                        this.drawObject.GrayValueInfo -= new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                                        this.drawObject.ClearDrawingObject();
                                        this.drawObject = new userDrawPolygonROI(this.hWindowControl1, false);
                                        this.drawObject.GrayValueInfo += new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                                    }
                                    this.drawObject.IsLiveState = true;
                                    break;
                                case enMoveType.点位运动:
                                    this.drawObject.AttachPropertyData.Clear();
                                    if (!(this.drawObject is userDrawPointROI))
                                    {
                                        this.drawObject.GrayValueInfo -= new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                                        this.drawObject.ClearDrawingObject();
                                        this.drawObject = new userDrawPointROI(this.hWindowControl1, false);
                                        this.drawObject.GrayValueInfo += new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                                    }
                                    this.drawObject.IsLiveState = true;
                                    break;
                                case enMoveType.直线运动:
                                    this.drawObject.AttachPropertyData.Clear();
                                    if (!(this.drawObject is userDrawLineROI))
                                    {
                                        this.drawObject.GrayValueInfo -= new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                                        this.drawObject.ClearDrawingObject();
                                        this.drawObject = new userDrawLineROI(this.hWindowControl1, false);
                                        this.drawObject.GrayValueInfo += new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                                    }
                                    this.drawObject.IsLiveState = true;
                                    break;
                                default:
                                    throw new NotImplementedException(this._trackParam[e.RowIndex].MoveType.ToString() + "未实现!");
                            }
                            //////////////////////////
                            foreach (var item in this._trackParam)
                            {
                                if (index != e.RowIndex && item.RoiShape != null)
                                {
                                    this.drawObject.AddViewObject(new ViewData(item.RoiShape.GetPixROI(this.drawObject.CameraParam).GetXLD(), enColor.orange.ToString()));
                                }
                                index++;
                            }
                            this.drawObject.IsLiveState = true;
                            if (this.drawObject.BackImage == null)
                            {
                                this.acqSource = AcqSourceManage.Instance.GetCamAcqSource(this._viewConfigParam.CamName);
                                if (this.acqSource != null)
                                {
                                    switch (this.acqSource.Sensor.ConfigParam.SensorType)
                                    {
                                        case enUserSensorType.面阵相机:
                                        case enUserSensorType.线阵相机:
                                            Dictionary<enDataItem, object> data = this.acqSource.AcqImageData(null);
                                            this.drawObject.BackImage = data[enDataItem.Image] as ImageDataClass;
                                            break;
                                    }
                                }
                                else
                                {
                                    AcqSource _camSource = AcqSourceManage.Instance.GetAcqSource(this.传感器comboBox1.SelectedItem.ToString());
                                    this.drawObject.BackImage = new ImageDataClass(this.sourceImage, _camSource?.Sensor.CameraParam);
                                }
                            }
                            /////////////////////////////////////////////////////////////////////////
                            if (this._trackParam[e.RowIndex].RoiShape == null)
                                this.drawObject.SetParam(null);
                            else
                            {
                                this.drawObject.SetParam(this.wcsCoordSystem);
                                this.drawObject.SetParam(this._trackParam[e.RowIndex].RoiShape);
                            }
                            this.drawObject.DrawPixRoiShapeOnWindow(enColor.red, out pixShape);
                            this.drawObject.AddViewObject(new ViewData(pixShape.GetXLD(), enColor.orange.ToString()));
                            /////////////////////////////////////////////////////////
                            this._trackParam[e.RowIndex].RoiShape = pixShape.GetWcsROI(this.drawObject.CameraParam);
                            //////////////////////////////////////////////////////////
                            for (int i = 0; i < this.dataGridView1.Rows.Count; i++)
                            {
                                this.dataGridView1.Rows[i].HeaderCell.Value = (i + 1).ToString();
                            }
                            break;
                        case "DeletCol":
                            if (this._trackParam.Count > e.RowIndex)
                                this._trackParam.RemoveAt(e.RowIndex);
                            if (this.drawObject.AttachPropertyData.Count > e.RowIndex)
                                this.drawObject.AttachPropertyData.RemoveAt(e.RowIndex);
                            this.drawObject.DrawingGraphicObject();
                            for (int i = 0; i < this.dataGridView1.Rows.Count; i++)
                            {
                                if (this.dataGridView1.Rows.Count > i)
                                    this.dataGridView1.Rows[i].HeaderCell.Value = (i + 1).ToString();
                            }
                            break;
                        case "TrackCol":
                            this.drawObject.AttachPropertyData.Clear();
                            foreach (var item in this._trackParam)
                            {
                                if (item.RoiShape == null) return;
                                if (index == e.RowIndex)
                                {
                                    this.drawObject.AddViewObject(new ViewData(item.RoiShape.AffineTransWcsROI(new HHomMat2D(this.wcsCoordSystem?.GetVariationHomMat2D())).GetPixROI(this.drawObject.CameraParam).GetXLD(), enColor.green.ToString()));
                                }
                                else
                                {
                                    this.drawObject.AddViewObject(new ViewData(item.RoiShape.AffineTransWcsROI(new HHomMat2D(this.wcsCoordSystem?.GetVariationHomMat2D())).GetPixROI(this.drawObject.CameraParam).GetXLD(), enColor.orange.ToString()));
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
                //MessageBox.Show(ex.ToString());
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
            catch
            {

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

        private void dataGridView1_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            try
            {
                /////////////////////////
                this.dataGridView1.TopLeftHeaderCell.Value = "索引";
                for (int i = 0; i < this.dataGridView1.Rows.Count; i++)
                {
                    this.dataGridView1.Rows[i].HeaderCell.Value = (i + 1).ToString();
                }
                this.drawObject.ClearViewObject();
                foreach (var item in this._trackParam)
                {
                    if (item.RoiShape != null)
                        this.drawObject.AddViewObject(new ViewData(item.RoiShape.GetPixROI(this.drawObject.CameraParam).GetXLD(), enColor.orange.ToString()));
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void dataGridView1_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            try
            {
                /////////////////////////
                this.dataGridView1.TopLeftHeaderCell.Value = "索引";
                for (int i = 0; i < this.dataGridView1.Rows.Count; i++)
                {
                    this.dataGridView1.Rows[i].HeaderCell.Value = (i + 1).ToString();
                }
                this.drawObject.AttachPropertyData.Clear();
                foreach (var item in this._trackParam)
                {
                    if (item.RoiShape != null)
                        this.drawObject.AddViewObject(new ViewData(item.RoiShape.GetPixROI(this.drawObject.CameraParam).GetXLD(), enColor.orange.ToString()));
                }
                this.drawObject.DrawingGraphicObject();
            }
            catch (Exception ex)
            {

            }
        }

        private void AdThickdData(userWcsThick wcsThick)
        {
            if (wcsThick == null) return;
            if (OperateManager.Instance.ProgramList != null)
            {
                this.Invoke(new Action(() =>
                {
                    OperateParam param = OperateManager.Instance.ProgramList[0];
                    if (OutputDataConfigParamManager.Instance.DataItemParamList.Count == 0)
                    {
                        string[] name = new string[3] { "最大值", "最小值", "平均值" };
                        foreach (var item in name)
                        {
                            OutputDataConfigParamManager.Instance.DataItemParamList.Add(new OutputDataConfigParam(
                            item,
                            DateTime.Now.ToString("yyyy/MM/dd_HH:ss:mm"),
                            param.Operate,
                            param.ProductSize,
                            param.ProductID,
                            0,
                            0,
                            0,
                            0,
                            0));
                        }
                    }
                    OutputDataConfigParamManager.Instance.DataItemParamList.Add(new OutputDataConfigParam(
                    OutputDataConfigParamManager.Instance.DataItemParamList.Count + 1 - 3,
                    DateTime.Now.ToString("yyyy/MM/dd_HH:ss:mm"),
                    param.Operate,
                    param.ProductSize,
                    param.ProductID,
                    wcsThick.Thick,
                    wcsThick.Dist1,
                    wcsThick.Dist2,
                    wcsThick.X,
                    wcsThick.Y));
                    /////////////////////////////////////////
                    List<double> listThick = new List<double>();
                    double maxThick = 0;
                    double maxDist1 = 0;
                    double maxDist2 = 0;
                    double max_x = 0;
                    double max_y = 0;
                    double minThick = int.MaxValue;
                    double minDist1 = int.MaxValue;
                    double minDist2 = int.MaxValue;
                    double min_x = 0;
                    double min_y = 0;
                    foreach (var item in OutputDataConfigParamManager.Instance.DataItemParamList)
                    {
                        if (!item.DataItem1.Contains("值"))
                        {
                            double resultThick = 0, resultDist1 = 0, resultDist2 = 0; ;
                            double.TryParse(item.DataItem6, out resultThick);
                            double.TryParse(item.DataItem7, out resultDist1);
                            double.TryParse(item.DataItem8, out resultDist2);

                            listThick.Add(resultThick);
                            if (resultThick > maxThick)
                            {
                                maxThick = resultThick;
                                maxDist1 = resultDist1;
                                maxDist2 = resultDist2;
                                double.TryParse(item.DataItem9, out max_x);
                                double.TryParse(item.DataItem10, out max_y);
                            }
                            if (resultThick < minThick)
                            {
                                minThick = resultThick;
                                minDist1 = resultDist1;
                                minDist2 = resultDist2;
                                double.TryParse(item.DataItem9, out min_x);
                                double.TryParse(item.DataItem10, out min_y);
                            }
                        }
                    }
                    if (OutputDataConfigParamManager.Instance.DataItemParamList.Count > 0)
                    {
                        OutputDataConfigParamManager.Instance.DataItemParamList[0].DataItem6 = maxThick.ToString();
                        OutputDataConfigParamManager.Instance.DataItemParamList[0].DataItem7 = maxDist1.ToString();
                        OutputDataConfigParamManager.Instance.DataItemParamList[0].DataItem8 = maxDist2.ToString();
                        OutputDataConfigParamManager.Instance.DataItemParamList[0].DataItem9 = max_x.ToString();
                        OutputDataConfigParamManager.Instance.DataItemParamList[0].DataItem10 = max_y.ToString();
                    }
                    if (OutputDataConfigParamManager.Instance.DataItemParamList.Count > 1)
                    {
                        OutputDataConfigParamManager.Instance.DataItemParamList[1].DataItem6 = minThick.ToString();
                        OutputDataConfigParamManager.Instance.DataItemParamList[1].DataItem7 = minDist1.ToString();
                        OutputDataConfigParamManager.Instance.DataItemParamList[1].DataItem8 = minDist2.ToString();
                        OutputDataConfigParamManager.Instance.DataItemParamList[1].DataItem9 = min_x.ToString();
                        OutputDataConfigParamManager.Instance.DataItemParamList[1].DataItem10 = min_y.ToString();
                    }
                    if (OutputDataConfigParamManager.Instance.DataItemParamList.Count > 2)
                    {
                        if (listThick.Count > 0)
                            OutputDataConfigParamManager.Instance.DataItemParamList[2].DataItem6 = listThick.Average().ToString("f1");
                        else
                            OutputDataConfigParamManager.Instance.DataItemParamList[2].DataItem6 = 0.ToString();
                        OutputDataConfigParamManager.Instance.DataItemParamList[2].DataItem7 = 0.ToString();
                        OutputDataConfigParamManager.Instance.DataItemParamList[2].DataItem8 = 0.ToString();
                    }
                }));
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                AdThickdData(new userWcsThick());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }

        private CancellationTokenSource ctsRun;
        private async void 运行but_Click(object sender, EventArgs e)
        {
            try
            {
                this.RunState = true;
                int circleCount = 1;
                int.TryParse(this.循环次数textBox.Text, out circleCount);
                AcqSource _camSource = AcqSourceManage.Instance.GetAcqSource(this.传感器comboBox1.SelectedItem.ToString());
                this.drawObject.CameraParam = _camSource?.Sensor?.CameraParam;
                if (this.currentParam.Image != null && this.currentParam.Image.IsInitialized())
                    this.drawObject.BackImage = new ImageDataClass(this.currentParam.Image, this.drawObject?.CameraParam); // 开始时更新图像
                /////////////////////////////////////////////////////
                for (int k = 0; k < circleCount; k++)
                {
                    this.当前次数textBox.Text = (k + 1).ToString();
                    if (!this.RunState) break;
                    this.停止but.Enabled = true;
                    this.运行but.Enabled = false;
                    this.停止but.BackColor = System.Drawing.Color.Red;
                    this.运行but.BackColor = System.Drawing.Color.Green;
                    this.drawObject.ClearViewObject();
                    this.drawObject.DrawingGraphicObject();
                    OutputDataConfigParamManager.Instance.DataItemParamList.Clear(); // 测量前先清空数据
                    //////////////////////////////////////////////////////
                    OperateParam param = this.currentParam; // 
                    if (param == null)
                    {
                        MessageBox.Show("没有相应的程序配方!!!");
                    }
                    IMotionControl _Card = MotionCardManage.GetCard(param.CoordSysName);
                    if (param == null)
                    {
                        MessageBox.Show("没有获取到坐标系，请检查参数设置!!!");
                    }
                    AcqSource acqSource1 = AcqSourceManage.Instance.GetAcqSource(param.AcqSourceName1);
                    AcqSource acqSource2 = AcqSourceManage.Instance.GetAcqSource(param.AcqSourceName2);
                    if (acqSource1 == null)
                    {
                        MessageBox.Show(" 没有获取到指定的采集源 acqSource1");
                    }
                    if (acqSource2 == null)
                    {
                        MessageBox.Show(" 没有获取到指定的采集源 acqSource2");
                    }
                    MoveCommandParam CommandParam = new MoveCommandParam();
                    drawWcsPoint wcsPoint;
                    drawPixPoint pixPoint;
                    drawWcsLine wcsLine;
                    drawPixLine pixLine;
                    userTextLable textLable;
                    userPixThick pixThick;
                    userWcsThick wcsThick;
                    Dictionary<enDataItem, object> dicList1, dicList2;
                    RobotJawParam jaw = RobotJawParaManager.Instance.GetJawParam(nameof(enRobotJawEnum.Jaw1));
                    this.ctsRun = new CancellationTokenSource(); // 创建对象
                    await Task.Run(() =>
                    {
                        int index = 0;
                        foreach (var item in param.TrackParam)
                        {
                            for (int i = 0; i < this.dataGridView1.Rows.Count; i++)
                            {
                                if (i == index)
                                    this.dataGridView1.Rows[i].Selected = true;
                                else
                                    this.dataGridView1.Rows[i].Selected = false;
                            }
                            index++;
                            if (this.ctsRun.IsCancellationRequested) break;
                            if (!item.IsActive) continue; // 如果没有激活，则不执行
                            if (item.Function == enFunction.定位) continue;
                            switch (item.MoveType)
                            {
                                case enMoveType.点位运动:
                                    wcsPoint = item.RoiShape as drawWcsPoint;
                                    if (wcsPoint == null)
                                        throw new ArgumentException("指定的轨迹类型不为点!");
                                    pixPoint = wcsPoint.GetPixPoint(this.drawObject.CameraParam);
                                    CommandParam.MoveType = item.MoveType;
                                    CommandParam.MoveSpeed = GlobalVariable.pConfig.MoveSpeed;
                                    CommandParam.MoveAxis = enAxisName.XY轴;
                                    CommandParam.CoordSysName = param.CoordSysName;      // 运动坐标系里是否需要包含移动指令
                                    CommandParam.AxisParam = new CoordSysAxisParam(wcsPoint.X + jaw.X, wcsPoint.Y + jaw.Y, 0, 0, 0, 0);
                                    CommandParam.AxisParam2 = new CoordSysAxisParam(wcsPoint.X + jaw.X, wcsPoint.Y + jaw.Y, 0, 0, 0, 0);
                                    ////////////////////////////////////////////////////////////
                                    _Card?.MoveMultyAxis(CommandParam);
                                    LoggerHelper.Info(this.Name + "运动到扫描起点:" + CommandParam.ToString());
                                    Thread.Sleep(acqSource1.Sensor.LaserParam.WaiteTime + acqSource2.Sensor.LaserParam.WaiteTime);
                                    acqSource1.Sensor.StartTrigger();
                                    acqSource2.Sensor.StartTrigger();
                                    acqSource1.SetIoOutput(true);
                                    Thread.Sleep(acqSource1.Sensor.LaserParam.WaiteTime);
                                    acqSource1.Sensor.StopTrigger();
                                    acqSource2.Sensor.StopTrigger();
                                    dicList1 = acqSource1.Sensor.ReadData();
                                    dicList2 = acqSource2.Sensor.ReadData();
                                    double[] dist1 = dicList1[enDataItem.Dist1] as double[];
                                    double[] dist2 = dicList2[enDataItem.Dist1] as double[];
                                    if (dist1 == null || dist1.Length == 0)
                                        MessageBox.Show(param.AcqSourceName1 + "->激光1没有获取到数据");
                                    if (dist2 == null || dist2.Length == 0)
                                        MessageBox.Show(param.AcqSourceName2 + "->激光2没有获取到数据");
                                    /////////////////////////////////////////////////////////
                                    switch (item.Function)
                                    {
                                        case enFunction.圆心: // 执行测量
                                        case enFunction.测量: // 执行测量
                                            pixThick = new userPixThick();
                                            pixThick.CamParams = this.drawObject.CameraParam;
                                            pixThick.Row = pixPoint.Row + 10;
                                            pixThick.Col = pixPoint.Col;
                                            pixThick.Dist1 = Math.Round(dist1.Average() * 1000, 3);
                                            pixThick.Dist2 = Math.Round(dist2.Average() * 1000, 3);
                                            pixThick.Thick = (pixThick.Dist1 + pixThick.Dist2 + Math.Round(GlobalVariable.pConfig.Cord_Gap * 1000,3)) + param.ThickScale;
                                            this.AdThickdData(pixThick.GetWcsThick());  // 测量一个添加一个数据进去  
                                            if ((param.StdValue + param.DownTolerance) * 1000 <= pixThick.Thick && pixThick.Thick <= (param.StdValue + param.UpTolerance) * 1000)
                                                pixThick.Result = "OK";
                                            else
                                                pixThick.Result = "NG";
                                            /////////////////////////////////////////////////
                                            //if (pixThick.Result == "OK")
                                            //    textLable = new userTextLable(string.Format("Dist1={0},Dist2={1},Thick={2}", pixThick.Dist1, pixThick.Dist2, pixThick.Thick), pixThick.Col, pixThick.Row, 10, "green");
                                            //else
                                            //    textLable = new userTextLable(string.Format("Dist1={0},Dist2={1},Thick={2}", pixThick.Dist1, pixThick.Dist2, pixThick.Thick), pixThick.Col, pixThick.Row, 10, "red");
                                            if (pixThick.Result == "OK")
                                                textLable = new userTextLable(string.Format("{0}", pixThick.Thick), pixThick.Col, pixThick.Row, 10, "green");
                                            else
                                                textLable = new userTextLable(string.Format("{0}", pixThick.Thick), pixThick.Col, pixThick.Row, 10, "red");
                                            this.drawObject.AddViewObject(new ViewData(pixThick.GetXLD(), "green"));
                                            this.drawObject.AddViewObject(new ViewData(textLable, "green"));
                                            break;
                                        case enFunction.校准: // 执行校准
                                            GlobalVariable.pConfig.Cord_Gap = GlobalVariable.pConfig.StandardThickValue - (dist1.Average() + dist2.Average());
                                            LoggerHelper.Info("校准成功，校准值 = " + GlobalVariable.pConfig.Cord_Gap.ToString());
                                            pixThick = new userPixThick();
                                            pixThick.CamParams = this.drawObject.CameraParam;
                                            pixThick.Row = pixPoint.Row + 10;
                                            pixThick.Col = pixPoint.Col;
                                            pixThick.Dist1 = Math.Round(dist1.Average() * 1000, 5);
                                            pixThick.Dist2 = Math.Round(dist2.Average() * 1000, 5);
                                            pixThick.Thick = (pixThick.Dist1 + pixThick.Dist2 + Math.Round(GlobalVariable.pConfig.Cord_Gap * 1000,5));
                                            //this.AdThickdData(pixThick.GetWcsThick());  // 测量一个添加一个数据进去  
                                            //if ((param.StdValue + param.DownTolerance) * 1000 <= pixThick.Thick && pixThick.Thick <= (param.StdValue + param.UpTolerance) * 1000)
                                            //    pixThick.Result = "OK";
                                            //else
                                            //    pixThick.Result = "NG";
                                            /////////////////////////////////////////////////
                                            //if (pixThick.Result == "OK")
                                                //textLable = new userTextLable(string.Format("标准块 = {0}", pixThick.Thick), pixThick.Col, pixThick.Row+50, 10, "green");
                                            textLable = new userTextLable(string.Format("标准块 = {0}", pixThick.Thick), 50, 370, 15, "green");
                                            //else
                                            //    textLable = new userTextLable(string.Format("{0}", pixThick.Thick), pixThick.Col, pixThick.Row, 10, "red");
                                            this.drawObject.AddViewObject(new ViewData(pixThick.GetXLD(), "green"));
                                            this.drawObject.AddViewObject(new ViewData(textLable, "green"));

                                            break;
                                    }
                                    break;
                                /////////////////////////////////////////////
                                case enMoveType.直线运动:
                                    wcsLine = item.RoiShape as drawWcsLine;
                                    if (wcsLine == null)
                                        throw new ArgumentException("指定的轨迹类型不为直线!");
                                    pixLine = wcsLine.GetPixLine(this.drawObject.CameraParam);
                                    CommandParam.MoveType = item.MoveType;
                                    CommandParam.MoveSpeed = GlobalVariable.pConfig.MoveSpeed;
                                    CommandParam.CoordSysName = param.CoordSysName;      // 运动坐标系里是否需要包含移动指令
                                    CommandParam.MoveAxis = enAxisName.XY轴;
                                    CommandParam.AxisParam = new CoordSysAxisParam(wcsLine.X1 + jaw.X, wcsLine.Y1 + jaw.Y, 0, 0, 0, 0);
                                    CommandParam.AxisParam2 = new CoordSysAxisParam(wcsLine.X1 + jaw.X, wcsLine.Y1 + jaw.Y, 0, 0, 0, 0);
                                    _Card?.MoveMultyAxis(CommandParam);
                                    LoggerHelper.Info(this.Name + "运动到扫描起点:" + CommandParam.ToString());
                                    CommandParam.MoveAxis = enAxisName.XY轴直线插补;
                                    CommandParam.MoveSpeed = GlobalVariable.pConfig.ScanSpeed;
                                    CommandParam.AxisParam = new CoordSysAxisParam(wcsLine.X1 + jaw.X, wcsLine.Y1 + jaw.Y, 0, 0, 0, 0);
                                    CommandParam.AxisParam2 = new CoordSysAxisParam(wcsLine.X2 + jaw.X, wcsLine.Y2 + jaw.Y, 0, 0, 0, 0);
                                    ////////////////////////////////////////////////////////////
                                    Thread.Sleep(acqSource1.Sensor.LaserParam.WaiteTime);
                                    acqSource1.Sensor.StartTrigger();
                                    acqSource2.Sensor.StartTrigger();
                                    acqSource1.SetIoOutput(true);
                                    Stopwatch stopwatch = new Stopwatch();
                                    stopwatch.Restart();
                                    _Card?.MoveMultyAxis(CommandParam);
                                    stopwatch.Stop();
                                    long value = stopwatch.ElapsedMilliseconds;
                                    LoggerHelper.Info(this.Name + "运动时间:" + value.ToString());
                                    LoggerHelper.Info(this.Name + "运动到扫描终点:" + CommandParam.ToString());
                                    //Thread.Sleep(acqSource1.Sensor.LaserParam.WaiteTime);
                                    acqSource1.Sensor.StopTrigger();
                                    acqSource2.Sensor.StopTrigger();
                                    dicList1 = acqSource1.Sensor.ReadData();
                                    dicList2 = acqSource2.Sensor.ReadData();
                                    List<double> list_x1 = new List<double>();
                                    List<double> list_y1 = new List<double>();
                                    List<double> list_x2 = new List<double>();
                                    List<double> list_y2 = new List<double>();
                                    double[] dist1Line = dicList1[enDataItem.Dist1] as double[];
                                    double[] dist2Line = dicList2[enDataItem.Dist1] as double[];
                                    if (dist1Line == null || dist1Line.Length == 0)
                                    {
                                        MessageBox.Show(param.AcqSourceName1 + "->激光1没有获取到数据");
                                        return;
                                    }
                                    else
                                    {
                                        for (int i = 0; i < dist1Line.Length; i++)
                                        {
                                            list_x1.Add(wcsLine.X1 + i * (wcsLine.X2 - wcsLine.X1));
                                            list_y1.Add(wcsLine.Y1 + i * (wcsLine.Y2 - wcsLine.Y1));
                                        }
                                    }
                                    if (dist2Line == null || dist2Line.Length == 0)
                                    {
                                        MessageBox.Show(param.AcqSourceName2 + "->激光2没有获取到数据");
                                        return;
                                    }
                                    else
                                    {
                                        for (int i = 0; i < dist2Line.Length; i++)
                                        {
                                            list_x2.Add(wcsLine.X1 + i * (wcsLine.X2 - wcsLine.X1));
                                            list_y2.Add(wcsLine.Y1 + i * (wcsLine.Y2 - wcsLine.Y1));
                                        }
                                    }
                                    switch (item.Function)
                                    {
                                        case enFunction.圆心: // 执行测量
                                        case enFunction.测量: // 执行测量
                                            this.Distance2D(list_x1.ToArray(), list_y1.ToArray(), dist1Line, list_x2.ToArray(), list_y2.ToArray(), dist2Line, enThickMeasureMethod.两线距离和, out wcsThick);
                                            wcsThick.X = (wcsLine.X1 + wcsLine.X2) * 0.5;
                                            wcsThick.Y = (wcsLine.Y1 + wcsLine.Y2) * 0.5;
                                            wcsThick.Dist1 = Math.Round(wcsThick.Dist1 * 1000, 3);
                                            wcsThick.Dist2 = Math.Round(wcsThick.Dist2 * 1000, 3);
                                            wcsThick.Thick = Math.Round(wcsThick.Thick * 1000, 3) + param.ThickScale;
                                            wcsThick.CamParams = this.drawObject.CameraParam;
                                            pixThick = wcsThick.GetPixThick();
                                            this.AdThickdData(pixThick.GetWcsThick());  // 测量一个添加一个数据进去  
                                            if ((param.StdValue + param.DownTolerance) * 1000 <= pixThick.Thick && pixThick.Thick <= (param.StdValue + param.UpTolerance) * 1000)
                                                pixThick.Result = "OK";
                                            else
                                                pixThick.Result = "NG";
                                            /////////////////////////////////////////////////
                                            //if (pixThick.Result == "OK")
                                            //    textLable = new userTextLable(string.Format("Dist1={0},Dist2={1},Thick={2}", pixThick.Dist1, pixThick.Dist2, pixThick.Thick), pixThick.Col, pixThick.Row + 10, 10, "green");
                                            //else
                                            //    textLable = new userTextLable(string.Format("Dist1={0},Dist2={1},Thick={2}", pixThick.Dist1, pixThick.Dist2, pixThick.Thick), pixThick.Col, pixThick.Row + 10, 10, "red");
                                            //this.drawObject.AddViewObject(new ViewData(textLable, "green"));

                                            if (pixThick.Result == "OK")
                                                textLable = new userTextLable(string.Format("{0}", pixThick.Thick), pixThick.Col, pixThick.Row, 10, "green");
                                            else
                                                textLable = new userTextLable(string.Format("{0}", pixThick.Thick), pixThick.Col, pixThick.Row, 10, "red");
                                            this.drawObject.AddViewObject(new ViewData(pixLine.GetXLD(), "green"));
                                            this.drawObject.AddViewObject(new ViewData(textLable, "green"));

                                            break;
                                        case enFunction.校准: // 执行校准
                                            GlobalVariable.pConfig.Cord_Gap = GlobalVariable.pConfig.StandardThickValue - (dist1Line.Average() + dist2Line.Average());
                                            LoggerHelper.Info("校准成功，校准值 = " + GlobalVariable.pConfig.Cord_Gap.ToString());
                                            break;
                                    }
                                    break;

                                default:
                                    throw new ArgumentNullException("未实现的运动轨迹!!");
                            }
                        }
                        ////// 保存数据和图像 /////
                        //string path = param.SaveDirect + "\\" + DateTime.Now.ToString("yyyyMMdd") + "\\" + param.ProductID;
                        //if (!Directory.Exists(path))
                        //    Directory.CreateDirectory(path);
                        //string filePath = path + "\\" + "ThickData.csv";// DateTime.Now.ToString("yyyy-MM-dd-HH:mm:ss");
                        //string imagePath = path + "\\" + "Image.jpg";
                        //using (StreamWriter sw = new StreamWriter(filePath, true))
                        //{
                        //    foreach (var item in OutputDataConfigParamManager.Instance.DataItemParamList)
                        //    {
                        //        sw.WriteLine(item.ToString());
                        //    }
                        //}
                        //HImage hImage = this.hWindowControl1.HalconWindow.DumpWindowImage();
                        //if (hImage != null && hImage.IsInitialized())
                        //    hImage.WriteImage("jpg", 0, imagePath);
                    });
                    //////////////////// 在窗口中输出最大、最小值、平均值 //////////////////////////
                    this.停止but.Enabled = false;
                    this.运行but.Enabled = true;
                    this.停止but.BackColor = System.Drawing.Color.DarkGray;
                    this.运行but.BackColor = System.Drawing.Color.Yellow;
                    string maxValue = "0", minValue = "0", meanValue = "0";
                    double maxValue_x = 0, maxValue_y = 0, minValue_x = 0, minValue_y = 0;
                    if (OutputDataConfigParamManager.Instance.DataItemParamList.Count > 0)
                    {
                        maxValue = OutputDataConfigParamManager.Instance.DataItemParamList[0].DataItem6;
                        double.TryParse(OutputDataConfigParamManager.Instance.DataItemParamList[0].DataItem9, out maxValue_x);
                        double.TryParse(OutputDataConfigParamManager.Instance.DataItemParamList[0].DataItem10, out maxValue_y);
                    }
                    if (OutputDataConfigParamManager.Instance.DataItemParamList.Count > 1)
                    {
                        minValue = OutputDataConfigParamManager.Instance.DataItemParamList[1].DataItem6;
                        double.TryParse(OutputDataConfigParamManager.Instance.DataItemParamList[1].DataItem9, out minValue_x);
                        double.TryParse(OutputDataConfigParamManager.Instance.DataItemParamList[1].DataItem10, out minValue_y);
                    }
                    if (OutputDataConfigParamManager.Instance.DataItemParamList.Count > 2)
                    {
                        meanValue = OutputDataConfigParamManager.Instance.DataItemParamList[2].DataItem6;
                    }
                    ///////////////////////////
                    double max = 0, min = 0;
                    double.TryParse(maxValue,out max);
                    double.TryParse(minValue, out min);
                    drawPixPoint maxValuePix = new drawWcsPoint(maxValue_x, maxValue_y, 0).GetPixPoint(this.drawObject.CameraParam);
                    drawPixPoint minValuePix = new drawWcsPoint(minValue_x, minValue_y, 0).GetPixPoint(this.drawObject.CameraParam);
                    userTextLable textLableID = new userTextLable("产品ID = " + this.currentParam.ProductID, 50, 20, 15, "green", enLablePosition.用户定义);
                    userTextLable textLableMax = new userTextLable("最大值 = " + maxValue, 50, 90, 15, "green", enLablePosition.用户定义);
                    userTextLable textLableMin = new userTextLable("最小值 = " + minValue, 50, 160, 15, "green", enLablePosition.用户定义);
                    userTextLable textLableMean = new userTextLable("平均值 = " + minValue, 50, 230, 15, "green", enLablePosition.用户定义);
                    userTextLable textLableDiff = new userTextLable("极差 = " + Math.Round((max - min),1), 50, 300, 15, "green", enLablePosition.用户定义);

                    userTextLable textLableMaxValue = new userTextLable("max", maxValuePix.Col - 70, maxValuePix.Row + 0, 10, "green", enLablePosition.用户定义);
                    userTextLable textLableMinValue = new userTextLable("min", minValuePix.Col - 70, minValuePix.Row + 0, 10, "green", enLablePosition.用户定义);
                    this.drawObject.AddViewObject(new ViewData(textLableID, "green"));
                    this.drawObject.AddViewObject(new ViewData(textLableMax, "green"));
                    this.drawObject.AddViewObject(new ViewData(textLableMin, "green"));
                    this.drawObject.AddViewObject(new ViewData(textLableMean, "green"));
                    this.drawObject.AddViewObject(new ViewData(textLableDiff, "green"));

                    this.drawObject.AddViewObject(new ViewData(textLableMaxValue, "green"));
                    this.drawObject.AddViewObject(new ViewData(textLableMinValue, "green"));

                    //// 保存数据和图像 /////
                    string path = param.SaveDirect + "\\" + DateTime.Now.ToString("yyyyMMdd") + "\\" + param.ProductID;
                    if (!Directory.Exists(path))
                        Directory.CreateDirectory(path);
                    string filePath = path + "\\" + "ThickData.csv";// DateTime.Now.ToString("yyyy-MM-dd-HH:mm:ss");
                    string imagePath = path + "\\" + "Image.jpg";
                    using (StreamWriter sw = new StreamWriter(filePath, true))
                    {
                        foreach (var item in OutputDataConfigParamManager.Instance.DataItemParamList)
                        {
                            sw.WriteLine(item.ToString());
                        }
                    }
                    HImage hImage = this.hWindowControl1.HalconWindow.DumpWindowImage();
                    if (hImage != null && hImage.IsInitialized())
                        hImage.WriteImage("jpg", 0, imagePath);
                }
            }
            catch (Exception ex)
            {
                this.停止but_Click(null, null);
                MessageBox.Show(ex.ToString());
            }
        }

        private void 停止but_Click(object sender, EventArgs e)
        {
            try
            {
                this.RunState = false;
                this.ctsRun?.Cancel();
                this.停止but.Enabled = false;
                this.运行but.Enabled = true;
                this.停止but.BackColor = System.Drawing.Color.DarkGray;
                this.运行but.BackColor = System.Drawing.Color.Yellow;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void 定位btn_Click(object sender, EventArgs e)
        {
            try
            {
                OperateParam param = OperateManager.Instance.GetParam(this.产品尺寸comboBox.Text);
                if (param == null)
                {
                    MessageBox.Show("没有相应的程序配方!!!");
                }
                drawWcsPoint wcsPoint;
                List<double> X = new List<double>();
                List<double> Y = new List<double>();
                foreach (var item in param.TrackParam)
                {
                    switch (item.Function)
                    {
                        case enFunction.测量: // 获取定位点
                        case enFunction.定位: // 获取定位点
                            wcsPoint = item.RoiShape as drawWcsPoint;
                            if (wcsPoint != null)
                            {
                                X.Add(wcsPoint.X);
                                Y.Add(wcsPoint.Y);
                            }
                            break;
                    }
                }
                double Row, Column, Radius1, StartPhi, EndPhi;
                string PointOrder;
                if (X.Count >= 3)
                {
                    new HXLDCont(X.ToArray(), Y.ToArray()).FitCircleContourXld("algebraic", -1, 0, 0, 10, 1, out Row, out Column, out Radius1, out StartPhi, out EndPhi, out PointOrder);
                    drawWcsPoint wcsCenterPoint = new drawWcsPoint(Row, Column, 0);
                    TrackMoveParam moveParam = new TrackMoveParam(wcsCenterPoint);
                    moveParam.Function = enFunction.圆心;
                    bool result = false;
                    int index = 0;
                    foreach (var item in this.currentParam.TrackParam)
                    {
                        if (item.Function == enFunction.圆心)
                        {
                            result = true;
                            break;
                        }
                        index++;
                    }
                    if (result)
                        this.currentParam.TrackParam[index] = moveParam;
                    else
                        this.currentParam.TrackParam.Add(moveParam);
                    /////////////////////////////////////
                    this.currentParam.Center_X = Row;
                    this.currentParam.Center_Y = Column;
                    this.中心XtextBox.Text = Row.ToString();
                    this.中心YtextBox.Text = Column.ToString();
                }
                else
                    MessageBox.Show("计算中心的拟合圆点数必需大于或等于 3 ");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void 产品尺寸comboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            try
            {
                if (this.产品尺寸comboBox.SelectedItem == null) return;
                this.currentParam = OperateManager.Instance.GetParam(this.产品尺寸comboBox.SelectedItem.ToString());
                if (this.currentParam == null)
                {
                    this.currentParam = new OperateParam();
                    this.currentParam.ProductSize = this.产品尺寸comboBox.SelectedItem.ToString();
                    this.currentParam.CurrentProductSize = this.产品尺寸comboBox.SelectedItem.ToString();
                    OperateManager.Instance.ProgramList.Add(this.currentParam);
                    //this.dataGridView1.DataSource = null; // 这里不能赋值 NULL
                }
                foreach (var item in OperateManager.Instance.ProgramList) // 将所有对象的当前产品尺寸设置为 指定的产品尺寸 
                {
                    item.CurrentProductSize = this.产品尺寸comboBox.SelectedItem.ToString();
                }
                this._trackParam = this.currentParam.TrackParam;
                //this.dataGridView1.DataSource = null;
                this.dataGridView1.DataSource = this._trackParam;
                ////////////// 数据取消绑定 ////////////////////
                this.操作员comboBox.DataBindings.Clear();
                this.产品尺寸comboBox.DataBindings.Clear();
                this.产品标识号textBox.DataBindings.Clear();
                this.多文件目录textBox.DataBindings.Clear();
                this.坐标系名称comboBox.DataBindings.Clear();
                this.激光采集源1comboBox.DataBindings.Clear();
                this.激光采集源2comboBox.DataBindings.Clear();
                this.标准值textBox.DataBindings.Clear();
                this.上偏差textBox.DataBindings.Clear();
                this.下偏差textBox.DataBindings.Clear();
                this.中心XtextBox.DataBindings.Clear();
                this.中心YtextBox.DataBindings.Clear();
                ///////////////////// 数据重新绑定 ////////////////////
                this.操作员comboBox.DataBindings.Add(nameof(this.操作员comboBox.Text), currentParam, nameof(currentParam.Operate), true, DataSourceUpdateMode.OnPropertyChanged);
                this.产品尺寸comboBox.DataBindings.Add(nameof(this.产品尺寸comboBox.Text), currentParam, nameof(currentParam.ProductSize), true, DataSourceUpdateMode.OnPropertyChanged);
                this.产品标识号textBox.DataBindings.Add(nameof(this.产品标识号textBox.Text), currentParam, nameof(currentParam.ProductID), true, DataSourceUpdateMode.OnPropertyChanged);
                this.多文件目录textBox.DataBindings.Add(nameof(this.多文件目录textBox.Text), currentParam, nameof(currentParam.SaveDirect), true, DataSourceUpdateMode.OnPropertyChanged);
                //////////////////////  附加参数 
                this.坐标系名称comboBox.DataBindings.Add(nameof(this.坐标系名称comboBox.Text), currentParam, nameof(currentParam.CoordSysName), true, DataSourceUpdateMode.OnPropertyChanged);
                this.激光采集源1comboBox.DataBindings.Add(nameof(this.激光采集源1comboBox.Text), currentParam, nameof(currentParam.AcqSourceName1), true, DataSourceUpdateMode.OnPropertyChanged);
                this.激光采集源2comboBox.DataBindings.Add(nameof(this.激光采集源2comboBox.Text), currentParam, nameof(currentParam.AcqSourceName2), true, DataSourceUpdateMode.OnPropertyChanged);
                this.标准值textBox.DataBindings.Add(nameof(this.标准值textBox.Text), currentParam, nameof(currentParam.StdValue), true, DataSourceUpdateMode.OnPropertyChanged);
                this.上偏差textBox.DataBindings.Add(nameof(this.上偏差textBox.Text), currentParam, nameof(currentParam.UpTolerance), true, DataSourceUpdateMode.OnPropertyChanged);
                this.下偏差textBox.DataBindings.Add(nameof(this.下偏差textBox.Text), currentParam, nameof(currentParam.DownTolerance), true, DataSourceUpdateMode.OnPropertyChanged);
                this.中心XtextBox.DataBindings.Add(nameof(this.中心XtextBox.Text), currentParam, nameof(currentParam.Center_X), true, DataSourceUpdateMode.OnPropertyChanged);
                this.中心YtextBox.DataBindings.Add(nameof(this.中心YtextBox.Text), currentParam, nameof(currentParam.Center_Y), true, DataSourceUpdateMode.OnPropertyChanged);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void 保存button_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.保存模板checkBox.Checked)
                    this.currentParam.Image = this.drawObject.BackImage?.Image;
                OperateManager.Instance.Save();
                MessageBox.Show("保存成功!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void readDirectoryButton_Click(object sender, EventArgs e)
        {
            try
            {
                FolderBrowserDialog fold = new FolderBrowserDialog();
                fold.ShowDialog();
                if (fold.SelectedPath != null)
                    this.多文件目录textBox.Text = fold.SelectedPath;
            }
            catch
            {
                MessageBox.Show(new Exception().ToString());
            }
        }

        private void 校准测试button_Click(object sender, EventArgs e)
        {
            try
            {
                new CalibrateDoubleLaserForm().Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void dataGridView1_DataSourceChanged(object sender, EventArgs e)
        {
            try
            {
                /// 绑定数据后，必需重新排序列
                dataGridView1.Columns[nameof(this.ActiveCol)].DisplayIndex = 0;                      //ID
                dataGridView1.Columns[nameof(this.MoveCol)].DisplayIndex = 1;                  //序号
                dataGridView1.Columns[nameof(this.TrackCol)].DisplayIndex = 2;              //类型
                dataGridView1.Columns[nameof(this.FunctionCol)].DisplayIndex = 3;                 //描述
                dataGridView1.Columns[nameof(this.TeachCol)].DisplayIndex = 4;                    //传输数据
                dataGridView1.Columns[nameof(this.DeletCol)].DisplayIndex = 5;            //处理结果
            }
            catch
            {

            }
        }

        private void 添加中心点Btn_Click(object sender, EventArgs e)
        {
            try
            {
                double X = 0, Y = 0;
                double.TryParse(this.中心XtextBox.Text, out X);
                double.TryParse(this.中心XtextBox.Text, out Y);
                drawWcsPoint wcsCenterPoint = new drawWcsPoint(X, Y, 0);
                TrackMoveParam moveParam = new TrackMoveParam(wcsCenterPoint);
                moveParam.Function = enFunction.圆心;
                this.currentParam.TrackParam.Add(moveParam);
                /////////////////////////////////////
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public bool Distance2D(double[] X1, double[] Y1, double[] Z1, double[] X2, double[] Y2, double[] Z2, enThickMeasureMethod Method, out userWcsThick wcsThick)
        {
            bool result = false;
            ///////////////////////////
            int length;
            List<double> listThick = new List<double>();
            List<double> listDist1 = new List<double>();
            List<double> listDist2 = new List<double>();
            List<double> listX = new List<double>();
            List<double> listY = new List<double>();
            wcsThick = new userWcsThick();
            HalconLibrary ha = new HalconLibrary();
            //////////////////////
            if (X1 == null)
            {
                throw new ArgumentNullException("X1");
            }
            if (Y1 == null)
            {
                throw new ArgumentNullException("Y1");
            }
            if (Z1 == null)
            {
                throw new ArgumentNullException("Z1");
            }
            if (X2 == null)
            {
                throw new ArgumentNullException("X2");
            }
            if (Y2 == null)
            {
                throw new ArgumentNullException("Y2");
            }
            if (Z2 == null)
            {
                throw new ArgumentNullException("Z2");
            }
            if (X1.Length != Y1.Length || X1.Length != Z1.Length)
            {
                throw new ArgumentNullException("数组长度不相等!");
            }
            if (X2.Length != Y2.Length || X2.Length != Z2.Length)
            {
                throw new ArgumentNullException("数组长度不相等!");
            }
            ///////////////////////////////////////////////////////////////////////    
            switch (Method)
            {
                case enThickMeasureMethod.两点距离和:
                    length = Z1.Length > Z2.Length ? Z2.Length : Z1.Length;
                    for (int ii = 0; ii < length; ii++)
                    {
                        listX.Add(X1[ii]);
                        listY.Add(Y1[ii]);
                        listDist1.Add(Z1[ii]);
                        listDist2.Add(Z2[ii]);
                        listThick.Add((Z1[ii] + Z2[ii] + GlobalVariable.pConfig.Cord_Gap));
                    }
                    result = true;
                    break;
                case enThickMeasureMethod.两点距离差:
                    length = Z1.Length > Z2.Length ? Z2.Length : Z1.Length;
                    for (int ii = 0; ii < length; ii++)
                    {
                        listX.Add(X1[ii]);
                        listY.Add(Y1[ii]);
                        listDist1.Add(Z1[ii]);
                        listDist2.Add(Z2[ii]);
                        listThick.Add(Math.Abs(Z1[ii] - Z2[ii]));
                    }
                    result = true;
                    break;
                case enThickMeasureMethod.两线距离和:
                    double start_x1, end_x1, start_y1, end_y1, start_z1, end_z1, start_x2, end_x2, start_y2, end_y2, start_z2, end_z2;
                    ha.FitLine3D(X1, Y1, Z1, out start_x1, out start_y1, out start_z1, out end_x1, out end_y1, out end_z1);
                    ha.FitLine3D(X2, Y2, Z2, out start_x2, out start_y2, out start_z2, out end_x2, out end_y2, out end_z2);
                    listX.Add((start_x1 + end_x1) * 0.5);
                    listY.Add((start_y1 + end_y1) * 0.5);
                    listDist1.Add((start_z1 + end_z1) * 0.5);
                    listDist2.Add((start_z2 + end_z2) * 0.5);
                    listThick.Add(((start_z1 + end_z1) * 0.5 + (start_z2 + end_z2) * 0.5 + GlobalVariable.pConfig.Cord_Gap));
                    result = true;
                    break;
                case enThickMeasureMethod.两线距离差:
                    ha.FitLine3D(X1, Y1, Z1, out start_x1, out start_y1, out start_z1, out end_x1, out end_y1, out end_z1);
                    ha.FitLine3D(X2, Y2, Z2, out start_x2, out start_y2, out start_z2, out end_x2, out end_y2, out end_z2);
                    listX.Add((start_x1 + end_x1) * 0.5);
                    listY.Add((start_y1 + end_y1) * 0.5);
                    listDist1.Add((start_z1 + end_z1) * 0.5);
                    listDist2.Add((start_z2 + end_z2) * 0.5);
                    listThick.Add((Math.Abs((start_z1 + end_z1) * 0.5 - (start_z2 + end_z2) * 0.5) + GlobalVariable.pConfig.Cord_Gap));
                    result = true;
                    break;
                case enThickMeasureMethod.两平面距离和:
                    userWcsPlane wcsPlane1, wcsPlane2;
                    ha.GetPlaneObjectModel3DPose(X1, Y1, Z1, out wcsPlane1);
                    ha.GetPlaneObjectModel3DPose(X2, Y2, Z2, out wcsPlane2);
                    listX.Add(wcsPlane1.X);
                    listY.Add(wcsPlane1.Y);
                    listDist1.Add(wcsPlane1.Z);
                    listDist2.Add(wcsPlane2.Z);
                    listThick.Add((Math.Abs(wcsPlane1.Z + wcsPlane2.Z) + GlobalVariable.pConfig.Cord_Gap));
                    result = true;
                    break;
                case enThickMeasureMethod.两平面距离差:
                    ha.GetPlaneObjectModel3DPose(X1, Y1, Z1, out wcsPlane1);
                    ha.GetPlaneObjectModel3DPose(X2, Y2, Z2, out wcsPlane2);
                    listX.Add(wcsPlane1.X);
                    listY.Add(wcsPlane1.Y);
                    listDist1.Add(wcsPlane1.Z);
                    listDist2.Add(wcsPlane2.Z);
                    listThick.Add((Math.Abs(wcsPlane1.Z - wcsPlane2.Z) + GlobalVariable.pConfig.Cord_Gap));
                    result = true;
                    break;
                default:
                    break;
            }
            wcsThick = new userWcsThick(listX.Average(), listY.Average(), 0);
            wcsThick.Dist1 = listDist1.Average();
            wcsThick.Dist2 = listDist2.Average();
            wcsThick.Thick = listThick.Average();
            ///////////////////////////////////
            listX.Clear();
            listY.Clear();
            listDist1.Clear();
            listDist2.Clear();
            listThick.Clear();
            return result;
        }

        private void 设置参数1Btn_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.激光采集源1comboBox.SelectedItem == null) return;
                AcqSource acqSource = AcqSourceManage.Instance.GetCamAcqSource(this.激光采集源1comboBox.SelectedItem.ToString());
                if (acqSource != null)
                {
                    new LaserParamForm(acqSource.Sensor.LaserParam).Show();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void 设置参数2Btn_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.激光采集源2comboBox.SelectedItem == null) return;
                AcqSource acqSource = AcqSourceManage.Instance.GetCamAcqSource(this.激光采集源2comboBox.SelectedItem.ToString());
                if (acqSource != null)
                {
                    new LaserParamForm(acqSource.Sensor.LaserParam).Show();
                }
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

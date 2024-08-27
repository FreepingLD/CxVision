
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
    public partial class TrackForm : Form
    {
        private List<ImageDataClass> imageList = new List<ImageDataClass>();
        private VisualizeView drawObject;
        private ViewConfigParam _viewConfigParam;
        private bool IsLoad = false; // 窗体加载后，设置为true
        private AcqSource acqSource;
        private List<object> DataList = new List<object>();
        private SocketCommand command;
        private HImage sourceImage;
        private BindingList<TrackMoveParam> _trackParam = new BindingList<TrackMoveParam>();
        private userWcsCoordSystem wcsCoordSystem = new userWcsCoordSystem();
        private bool IsDraw = false;
        private HWindowControl hWindowControl1;
        public TrackForm(ViewConfigParam viewConfigParam, HWindowControl hWindowControl)
        {
            InitializeComponent();
            /////////////
            this.hWindowControl1 = hWindowControl;
            this.drawObject = new userDrawPointROI(this.hWindowControl1, false);
            this.drawObject.GrayValueInfo += new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
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
            this.dataGridView1.DataSource = _trackParam;
        }
        public TrackForm(BindingList<TrackMoveParam> trackParam, HWindowControl hWindowControl)
        {
            InitializeComponent();
            /////////////
            this.hWindowControl1 = hWindowControl;
            this.drawObject = new userDrawPointROI(this.hWindowControl1, false);
            this.drawObject.GrayValueInfo += new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
            this._trackParam = trackParam;
            this.titleLabel.TextAlign = ContentAlignment.MiddleLeft;
            this.ContextMenu = new ContextMenu();

            this.MoveCol.Items.Clear();
            this.MoveCol.ValueType = typeof(enMoveType);
            foreach (enMoveType temp in Enum.GetValues(typeof(enMoveType)))
                this.MoveCol.Items.Add(temp);
            this.dataGridView1.DataSource = _trackParam;
        }
        private void TrackForm_Load(object sender, EventArgs e)
        {
            //////////////
            BindProperty();
            //////////////////////////////////////
            this.Location = this._viewConfigParam.Location;
            this.Size = this._viewConfigParam.FormSize;
            //this.SignLabel.Text = this._viewConfigParam.Tag;
            this.IsLoad = true;
            //this.addContextMenu();
            this.addContextMenu(this.hWindowControl1);
            //this.addDataGridViewContextMenu(this.dataGridView2);
            //this.DisplayData();
            this.addDataGridViewContextMenu(this.dataGridView1);
        }
        private void BindProperty()
        {
            try
            {
                //this.传感器comboBox1.Items.Clear();
                //this.传感器comboBox1.Items.AddRange(AcqSourceManage.Instance.GetAcqSourceName());
                //this.传感器comboBox1.Text = this._viewConfigParam.CamName;
                //this.程序节点comboBox.Text = this._viewConfigParam.ProgramNode;
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
                        CircleArrayDataForm circleForm = new CircleArrayDataForm();
                        circleForm.Owner = this;
                        circleForm.ShowDialog();
                        hHomMat2D = new HHomMat2D();
                        HHomMat2D hHomMat_Rota;
                        index = this.dataGridView1.CurrentRow.Index;
                        if (index < 0) return;
                        Task.Run(() =>
                        {
                            for (int i = 1; i < circleForm.ArrayNum; i++)
                            {
                                hHomMat_Rota = hHomMat2D.HomMat2dRotate(circleForm.Add_Deg * i * Math.PI / 180, circleForm.Radius + circleForm.Ref_X, circleForm.Radius + circleForm.Ref_Y);
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
                this.IsLoad = false;
                ViewConfigParamManager.Instance.ViewParamList.Remove(this._viewConfigParam); // 关闭窗体时要删除相应的对象
                if (this.drawObject != null)
                    this.drawObject.ClearDrawingObject();
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


        private void 更新点位button_Click(object sender, EventArgs e)
        {
            try
            {
                //this.Init();
                //TreeNode node = GetExecuteNode("对射测厚");
                //if (node != null)
                //{
                //    ((ThicknessMeasure)node.Tag).ThickParamList = this._trackParam;
                //    MessageBox.Show("点位更新成功!");
                //    LoggerHelper.Error("点位更新成功!");
                //}
                //else
                //{
                //    MessageBox.Show("程序中没有添加对射测厚节点!");
                //    LoggerHelper.Error("程序中没有添加对射测厚节点");
                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void 加载点位button_Click(object sender, EventArgs e)
        {
            try
            {
                //this.Init();
                //TreeNode node = GetExecuteNode("对射测厚");
                //if (node != null)
                //{
                //    this._trackParam = ((ThicknessMeasure)node.Tag).ThickParamList;
                //    MessageBox.Show("点位加载成功!");
                //    LoggerHelper.Error("点位加载成功!");
                //}
                //else
                //{
                //    MessageBox.Show("程序中没有添加对射测厚节点!");
                //    LoggerHelper.Error("程序中没有添加对射测厚节点");
                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        #region 窗口右键菜单项
        private void ClearContextMenu(HWindowControl hWindowControl)
        {
            if (hWindowControl.ContextMenuStrip != null)
                hWindowControl.ContextMenuStrip = null;
        }
        private void addContextMenu(HWindowControl hWindowControl)
        {
            if (hWindowControl.ContextMenuStrip != null) return;
            ContextMenuStrip ContextMenuStrip1 = new ContextMenuStrip();
            ContextMenuStrip1.Name = hWindowControl.Name;
            // 添加右键菜单 
            ToolStripItem[] items = new ToolStripMenuItem[]
            {
             new ToolStripMenuItem("自适应图像(Auto)"),
             new ToolStripMenuItem("编辑程序(Edite)"),
             new ToolStripMenuItem("设置曝光(Set)"),
             new ToolStripMenuItem("3D(View)"),
             new ToolStripMenuItem("保存图像(Save)") ,
             new ToolStripMenuItem("保存点云(Save)"),
             new ToolStripMenuItem("清除窗口(Clear)"),
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
                    case "自适应图像(Auto)":
                        this.drawObject.AutoImage();
                        break;
                    case "编辑程序":
                        foreach (KeyValuePair<string, TreeViewWrapClass> item in ProgramForm.Instance.ProgramDic)
                        {
                            foreach (TreeNode item2 in item.Value.TreeView.Nodes)
                            {
                                TreeNode node;
                                //GetEditeNode(item2, out node);
                                //if (node != null)
                                //{
                                //    item.Value.treeView1_Edite(this, node);
                                //    break;
                                //}
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
                            MessageBox.Show("图像内容为空");
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

                //string nn = string.Join("","Row:", e.Row, " ", "Col:", e.Col, " ", "Gray:", e.GaryValue[0]);
                //this.hWindowControl1.HalconWindow.WriteString(string.Join("","Row:", e.Row, " ", "Col:", e.Col, " ", "Gray:",e.GaryValue[0]));
            }
        }

        #endregion

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    int index = 0;
                    PixROI pixShape;
                    switch (dataGridView1.Columns[e.ColumnIndex].Name)
                    {
                        case "TeachCol":
                            if (this.dataGridView1.Rows[e.RowIndex].DataBoundItem == null)
                            {
                                MessageBox.Show("未设置必需的图形参数，请先设置参数!");
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
                            this.drawObject.BackImage = new ImageDataClass(this.sourceImage);
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
                OperateParam param = OperateManager.Instance.ProgramList[0];
                OutputDataConfigParamManager.Instance.DataItemParamList.Add(new OutputDataConfigParam(
                    OutputDataConfigParamManager.Instance.DataItemParamList.Count + 1,
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
                OutputDataConfigParam maxParam, minParam, meanParam;
                List<double> listThick = new List<double>();
                foreach (var item in OutputDataConfigParamManager.Instance.DataItemParamList)
                {
                    if(!item.DataItem1.Contains("M"))
                    {
                        double result = 0;
                        double.TryParse(item.DataItem6, out result);
                        listThick.Add(result);
                    }
                }
                HTuple hTuple = new HTuple(listThick.ToArray());
                HTuple hTupleSort = hTuple.TupleSortIndex();
                minParam = OutputDataConfigParamManager.Instance.DataItemParamList[hTupleSort[0]];
                minParam.DataItem1 = "Min";
                maxParam = OutputDataConfigParamManager.Instance.DataItemParamList[hTupleSort[hTuple.Length - 1]];
                maxParam.DataItem1 = "Max";
                bool isContain = false;
                foreach (var item in OutputDataConfigParamManager.Instance.DataItemParamList)
                {
                    if (item.DataItem1 == "Min")
                    {
                        isContain = true;
                        break;
                    }
                }

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
    }
}

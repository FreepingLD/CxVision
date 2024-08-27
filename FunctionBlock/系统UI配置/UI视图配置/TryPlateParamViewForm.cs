﻿
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
    public partial class TryPlateParamViewForm : Form
    {
        private List<ImageDataClass> imageList = new List<ImageDataClass>();
        //private VisualizeView drawObject;
        private ViewConfigParam _viewConfigParam;
        private bool IsLoad = false; // 窗体加载后，设置为true
        private AcqSource acqSource;
        private ImageDataClass _imageDataClass;
        private bool IsDraw = false;


        public TryPlateParamViewForm(ViewConfigParam viewConfigParam)
        {
            InitializeComponent();
            /////////////
            //this.drawObject = new userDrawPointROI(this.hWindowControl1, false);
            //this.drawObject.HMouseDoubleClick += new HMouseEventHandler(this.hWindowControl1_DoubleClick);
            BaseFunction.ExcuteCompleted += new ExcuteCompletedEventHandler(DisplayExcuteResult);
            //this.drawObject.GrayValueInfo += new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
            //TreeViewWrapClass.ClickNode += new ClickNodeEventHandler(this.DisplayClickObject);
            UserLoginParamManager.Instance.UserChange += new EventHandler(this.UserChange_Event);
            this._viewConfigParam = viewConfigParam;
            if (!HWindowManage.HWindowList.ContainsKey(viewConfigParam.ViewName))
            {
                HWindowManage.HWindowList.Add(viewConfigParam.ViewName, null);
            }
            this.titleLabel.Text = viewConfigParam.ViewName;
            this.titleLabel.TextAlign = ContentAlignment.MiddleLeft;
            this.ContextMenu = new ContextMenu();
        }
        private void LoadViewForm_Load(object sender, EventArgs e)
        {
            //////////////
            BindProperty();
            //////////////////////////////////////
            this.Location = this._viewConfigParam.Location;
            this.Size = this._viewConfigParam.FormSize;
            //this.SignLabel.Text = this._viewConfigParam.Tag;
            this.IsLoad = true;
            this.addContextMenu();
            //this.addContextMenu(this.hWindowControl1);
        }
        private void BindProperty()
        {
            try
            {
                //this.传感器comboBox1.Items.Clear();
                //this.传感器comboBox1.Items.AddRange(AcqSourceManage.Instance.GetAcqSourceName());
                //this.传感器comboBox1.Text = this._viewConfigParam.CamName;
                //this.程序节点comboBox.Items.Add(this._viewConfigParam.ProgramNode);
                //this.程序节点comboBox.SelectedItem = this._viewConfigParam.ProgramNode;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
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
                        //this.传感器comboBox1.Enabled = false;
                        this.参数groupBox.Enabled = false;
                        //this.夹抓tableLayoutPanel.Enabled = false;
                        this.buttonClose.Enabled = false;
                        break;
                    case enUserName.工程师:
                        //this.传感器comboBox1.Enabled = true;
                        this.参数groupBox.Enabled = true;
                        //this.夹抓tableLayoutPanel.Enabled = true;
                        this.buttonClose.Enabled = false;
                        break;
                    case enUserName.开发人员:
                        //this.传感器comboBox1.Enabled = true;
                        this.参数groupBox.Enabled = true;
                        //this.夹抓tableLayoutPanel.Enabled = true;
                        this.buttonClose.Enabled = true;
                        break;
                }
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

        private void DisplayExcuteResult(object sender, ExcuteCompletedEventArgs e)
        {
            //if (e.SensorName == null) return;
            //if (!this._viewConfigParam.CamName.Contains(e.SensorName)) return;
            if (e.ViewWindow == null || e.ViewWindow != this._viewConfigParam.ViewName) return;
            if (!IsSelect()) return;  // 只有在当前选项页时才显示
            if (e.DataContent != null)
            {
                switch (e.DataContent.GetType().Name)
                {
                    case nameof(ImageDataClass):
                        this._imageDataClass = (ImageDataClass)e.DataContent;
                        /////////////////////////////////////////////////////////////////////////////////////////////
                        break;
                    case nameof(TryPlateParam):
                        TryPlateParam param = e.DataContent as TryPlateParam;
                        if (this.夹抓tableLayoutPanel.RowCount != param.RowCount && this.夹抓tableLayoutPanel.ColumnCount != param.ColCount)
                        {
                            Task.Run(() =>
                            {
                                this.LoadTryParam(this.夹抓tableLayoutPanel, param.RowCount, param.ColCount, param.CoordsList);
                            });
                        }
                        //else
                        //{
                        //    Task.Run(() =>
                        //    {
                        //        this.UpdataTryParam(this.夹抓tableLayoutPanel, param.CoordsList);
                        //    });

                        //}
                        break;
                    case nameof(RobotParam):
                        this.X轴正限位comboBox.DataBindings.Clear();
                        this.X轴负限位comboBox.DataBindings.Clear();
                        this.Y轴正限位comboBox.DataBindings.Clear();
                        this.Y轴负限位comboBox.DataBindings.Clear();
                        this.角度正限位comboBox.DataBindings.Clear();
                        this.角度负限位comboBox.DataBindings.Clear();
                        RobotParam _param = e.DataContent as RobotParam;
                        this.X轴正限位comboBox.DataBindings.Add("Text", _param, nameof(_param.LimitP_X), true, DataSourceUpdateMode.OnPropertyChanged);
                        this.X轴负限位comboBox.DataBindings.Add("Text", _param, nameof(_param.LimitN_X), true, DataSourceUpdateMode.OnPropertyChanged);
                        this.Y轴正限位comboBox.DataBindings.Add("Text", _param, nameof(_param.LimitP_Y), true, DataSourceUpdateMode.OnPropertyChanged);
                        this.Y轴负限位comboBox.DataBindings.Add("Text", _param, nameof(_param.LimitN_Y), true, DataSourceUpdateMode.OnPropertyChanged);
                        this.角度正限位comboBox.DataBindings.Add("Text", _param, nameof(_param.LimitP_Angle), true, DataSourceUpdateMode.OnPropertyChanged);
                        this.角度负限位comboBox.DataBindings.Add("Text", _param, nameof(_param.LimitN_Angle), true, DataSourceUpdateMode.OnPropertyChanged);
                        break;
                    default:
                        //this.drawObject.AddViewObject(new ViewData(e.DataContent, "red"));
                        break;
                }
            }
        }

        /// <summary>
        /// 窗体移动事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RobotLayOffViewForm_Move(object sender, EventArgs e)
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
                BaseFunction.ExcuteCompleted -= new ExcuteCompletedEventHandler(DisplayExcuteResult);
                //this.drawObject.GrayValueInfo -= new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                //this.drawObject.HMouseDoubleClick -= new HMouseEventHandler(this.hWindowControl1_DoubleClick);
                //TreeViewWrapClass.ClickNode -= new ClickNodeEventHandler(this.DisplayClickObject);
                //UserLoginParamManager.Instance.LoginParam.UserChange -= new EventHandler(this.UserChange_Event);
                UserLoginParamManager.Instance.UserChange -= new EventHandler(this.UserChange_Event);
                //if (this.drawObject != null)
                //this.drawObject.ClearDrawingObject();
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
            //this.SignLabel.ContextMenuStrip = ContextMenuStrip1;
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
                        //RenameForm renameForm = new RenameForm(this.SignLabel.Text);
                        //renameForm.ShowDialog();
                        //this._viewConfigParam.Tag = renameForm.ReName;
                        //renameForm.Dispose();
                        //this.SignLabel.Text = this._viewConfigParam.Tag;
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
        //private void ClearContextMenu(HWindowControl hWindowControl)
        //{
        //    if (hWindowControl.ContextMenuStrip != null)
        //        hWindowControl.ContextMenuStrip = null;
        //}
        //private void addContextMenu(HWindowControl hWindowControl)
        //{
        //    ContextMenuStrip ContextMenuStrip1 = new ContextMenuStrip();
        //    ContextMenuStrip1.Name = hWindowControl.Name;
        //    // 添加右键菜单 
        //    ToolStripItem[] items = new ToolStripMenuItem[]
        //    {
        //     new ToolStripMenuItem("自适应图像(Auto)"),
        //     new ToolStripMenuItem("编辑程序"),
        //     new ToolStripMenuItem("设置曝光"),
        //     new ToolStripMenuItem("设置增益"),
        //     new ToolStripMenuItem("设置光源亮度"),
        //     new ToolStripMenuItem("十字线"),
        //     new ToolStripMenuItem("3D(View)"),
        //     new ToolStripMenuItem("保存图像") ,
        //     new ToolStripMenuItem("保存点云"),
        //     new ToolStripMenuItem("清除窗口(Clear)"),
        //    };
        //    if (this._viewConfigParam.IsShowCross)
        //        items[5].Text = "隐藏十字线";
        //    else
        //        items[5].Text = "显示十字线";
        //    ContextMenuStrip1.Items.AddRange(items); // 在滑赋值前不能调用 
        //    ContextMenuStrip1.ItemClicked += new ToolStripItemClickedEventHandler(hWindowControlContextMenuStrip_ItemClicked);
        //    hWindowControl.ContextMenuStrip = ContextMenuStrip1;
        //}
        //private void hWindowControlContextMenuStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        //{
        //    string name = e.ClickedItem.Text;
        //    try
        //    {
        //        ((ContextMenuStrip)sender).Close();
        //        switch (name)
        //        {
        //            case "自适应图像(Auto)":
        //                this.drawObject.AutoImage();
        //                break;
        //            case "编辑程序":
        //                foreach (KeyValuePair<string, TreeViewWrapClass> item in ProgramForm.Instance.ProgramDic)
        //                {
        //                    foreach (TreeNode item2 in item.Value.TreeView.Nodes)
        //                    {
        //                        //if (item2.Text == this._viewConfigParam.ProgramNode)
        //                        //    item.Value.treeView1_Edite(this, item2);
        //                        TreeNode node;
        //                        GetEditeNode(item2, out node);
        //                        if (node != null)
        //                        {
        //                            item.Value.treeView1_Edite(this, node);
        //                            break;
        //                        }
        //                    }
        //                }
        //                break;
        //            case "设置曝光":
        //                if (AcqSourceManage.Instance.GetAcqSource(this._viewConfigParam.CamName) == null || AcqSourceManage.Instance.GetAcqSource(this._viewConfigParam.CamName).Sensor == null) return;
        //                string value = AcqSourceManage.Instance.GetAcqSource(this._viewConfigParam.CamName).Sensor.GetParam("曝光").ToString();
        //                RenameForm renameForm = new RenameForm(value);
        //                renameForm.ShowDialog();
        //                AcqSourceManage.Instance.GetAcqSource(this._viewConfigParam.CamName).Sensor.SetParam("曝光", renameForm.ReName);
        //                break;
        //            case "设置增益":
        //                if (AcqSourceManage.Instance.GetAcqSource(this._viewConfigParam.CamName) == null || AcqSourceManage.Instance.GetAcqSource(this._viewConfigParam.CamName).Sensor == null) return;
        //                value = AcqSourceManage.Instance.GetAcqSource(this._viewConfigParam.CamName).Sensor.GetParam("增益").ToString();
        //                renameForm = new RenameForm(value);
        //                renameForm.ShowDialog();
        //                AcqSourceManage.Instance.GetAcqSource(this._viewConfigParam.CamName).Sensor.SetParam("增益", renameForm.ReName);
        //                break;
        //            case "设置光源亮度":
        //                if (AcqSourceManage.Instance.GetAcqSource(this._viewConfigParam.CamName) == null) return;
        //                LightConfigForm lightForm = new LightConfigForm(AcqSourceManage.Instance.GetAcqSource(this._viewConfigParam.CamName));
        //                lightForm.ShowDialog();
        //                break;
        //            case "十字线":
        //            case "隐藏十字线":
        //            case "显示十字线":
        //                if (this._viewConfigParam.IsShowCross)
        //                {
        //                    e.ClickedItem.Text = "显示十字线";
        //                    this._viewConfigParam.IsShowCross = false;
        //                }
        //                else
        //                {
        //                    e.ClickedItem.Text = "隐藏十字线";
        //                    this._viewConfigParam.IsShowCross = true;
        //                }
        //                ViewConfigParamManager.Instance.Save();
        //                break;
        //            case "3D(View)":
        //                break;
        //            case "保存图像":
        //                SaveFileDialog saveFileDialog1 = new SaveFileDialog();
        //                saveFileDialog1.Filter = "bmp files (*.bmp)|*.bmp|All files (*.*)|*.*";
        //                saveFileDialog1.FilterIndex = 0;
        //                saveFileDialog1.ShowDialog();
        //                if (this.drawObject.BackImage != null && this.drawObject.BackImage.Image.IsInitialized())
        //                    this.drawObject.BackImage.Image.WriteImage("bmp", 0, saveFileDialog1.FileName);
        //                else
        //                {
        //                    if (saveFileDialog1.FileName != null && saveFileDialog1.FileName.Length > 0)
        //                        MessageBox.Show("图像内容为空");
        //                }

        //                break;
        //            case "保存点云":
        //                saveFileDialog1 = new SaveFileDialog();
        //                saveFileDialog1.Filter = "ply files (*.ply)|*.ply|txt files (*.txt)|*.txt|om3 files (*.om3)|*.om3|stl files (*.stl)|*.stl|obj files (*.obj)|*.obj|dxf files (*.dxf)|*.dxf|All files (*.*)|*.*";
        //                saveFileDialog1.FilterIndex = 3;
        //                saveFileDialog1.ShowDialog();
        //                if (this.drawObject.PointCloudModel3D != null)
        //                {
        //                    HObjectModel3D hObjectModel3D = HObjectModel3D.UnionObjectModel3d(this.drawObject.PointCloudModel3D.ObjectModel3D, "points_surface");
        //                    hObjectModel3D.WriteObjectModel3d(new FileInfo(saveFileDialog1.FileName).Extension, saveFileDialog1.FileName, new HTuple(), new HTuple());
        //                    hObjectModel3D.Dispose();
        //                }
        //                else
        //                {
        //                    if (saveFileDialog1.FileName != null && saveFileDialog1.FileName.Length > 0)
        //                        MessageBox.Show("点云句柄内容为空");
        //                }
        //                break;
        //            case "清除窗口(Clear)":
        //                this.drawObject.ClearWindow();
        //                break;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.ToString());
        //    }
        //}
        //private void hWindowControl1_MouseMove(object sender, GrayValueInfoEventArgs e)
        //{
        //    if (e.GaryValue.Length > 0)
        //    {
        //        //this.hWindowControl1.HalconWindow.SetTposition((int)(e.Row), (int)e.Col);
        //        //this.hWindowControl1.HalconWindow.SetFont("-Consolas-" + 10 + "- *-0-*-*-1-");
        //        //this.hWindowControl1.HalconWindow.SetColor("red");

        //        //this.SignLabel.Text = string.Join("", "Row:", e.Row, " ", "Col:", e.Col, " ", "Gray:", e.GaryValue[0]);
        //        //this.hWindowControl1.HalconWindow.WriteString(string.Join("","Row:", e.Row, " ", "Col:", e.Col, " ", "Gray:",e.GaryValue[0]));
        //        //this.drawObject.CopyBufferWindowView();
        //    }
        //}

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




        private void GetEditeNode(TreeNode node, out TreeNode selectNode)
        {
            selectNode = null;
            if (node.Name.Contains("Tool"))
            {
                if (node.FullPath.Replace("\\", ".") == this._viewConfigParam.ProgramNode)
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

        private void LoadTryParam(TableLayoutPanel tableLayoutPanel, int rowCount, int colCount, BindingList<UserTryPlateHoleParam> list)
        {
            this.Invoke(new Action(() =>
            {
                tableLayoutPanel.Controls.Clear();
                tableLayoutPanel.RowCount = rowCount;
                tableLayoutPanel.ColumnCount = colCount;
                tableLayoutPanel.RowStyles.Clear();
                tableLayoutPanel.ColumnStyles.Clear();
                LoadTryForm testForm;
                for (int i = 0; i < tableLayoutPanel.RowCount; i++)
                {
                    tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize, 10f));
                    for (int j = 0; j < tableLayoutPanel.ColumnCount; j++)
                    {
                        if (i == 0) // 列风格这里只需要设置一次
                            tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10f));
                        //tableLayoutPanel.ColumnStyles[j] = (new ColumnStyle(SizeType.AutoSize, 10f));
                        if (list.Count > i * tableLayoutPanel.ColumnCount + j)
                            testForm = new LoadTryForm(list[i * tableLayoutPanel.ColumnCount + j], this._imageDataClass);
                        else
                            continue;
                        testForm.Dock = DockStyle.Fill;
                        testForm.TopLevel = false;
                        tableLayoutPanel.Controls.Add(testForm, j, i);
                        testForm.Show();
                        //tableLayoutPanel.Controls.Add(testForm, j, i);
                    }
                }
            }));
        }
        private void UpdataTryParam(TableLayoutPanel tableLayoutPanel, BindingList<UserTryPlateHoleParam> list)
        {
            try
            {
                this.Invoke(new Action(() =>
                {
                    int index = 0;
                    foreach (var item in tableLayoutPanel.Controls)
                    {
                        switch (item.GetType().Name)
                        {
                            case nameof(LoadTryForm):
                                LoadTryForm form = item as LoadTryForm;
                                form.Param = list[index];
                                break;
                        }
                        index++;
                    }
                }));
            }
            catch
            {

            }
        }


        private void 程序节点comboBox_DropDown(object sender, EventArgs e)
        {
            //int a = 10;
            //this.程序节点comboBox.Items.Clear();
            //foreach (KeyValuePair<string, TreeViewWrapClass> item in ProgramForm.Instance.ProgramDic)
            //{
            //    foreach (TreeNode item2 in item.Value.TreeView.Nodes)
            //    {
            //        GetToolNode(item2);
            //    }
            //}
        }

        private void 程序节点comboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            //try
            //{
            //    if (this.程序节点comboBox.SelectedItem == null)
            //        this.程序节点comboBox.SelectedItem = this._viewConfigParam.ProgramNode;
            //    else
            //        this._viewConfigParam.ProgramNode = this.程序节点comboBox.SelectedItem.ToString();
            //    ViewConfigParamManager.Instance.Save();
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show("保存视图配置参数出错" + ex.ToString());
            //}
        }




    }
}

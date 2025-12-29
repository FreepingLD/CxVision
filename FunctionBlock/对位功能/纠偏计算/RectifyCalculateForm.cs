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
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media.Animation;
using View;

namespace FunctionBlock
{
    public partial class RectifyCalculateForm : Form
    {
        private IFunction _function;
        private VisualizeView drawObject;
        private TreeNode _refNode;

        public RectifyCalculateForm(TreeNode node)
        {
            InitializeComponent();
            this._refNode = node;
            this._function = this._refNode.Tag as IFunction;
            this._function.SetPropertyValues(nameof(TreeNode), this._refNode);
            //this.titleLabel.Text = node.Text;
            new ListBoxWrapClass().InitListBox(this.listBox1, node);
        }

        private void RectifyCalculateForm_Load(object sender, EventArgs e)
        {
            //////////////////////////////////////////
            List<drawWcsPoint> PlateTeachVector = new List<drawWcsPoint>();
            List<drawWcsPoint> PlateCurVector = new List<drawWcsPoint>();
            List<double> listCur_x = new List<double>();
            List<double> listCur_y = new List<double>();
            List<double> listTeach_x = new List<double>();
            List<double> listTeach_y = new List<double>();
            ///////////////////////////////////////////////////////////////
            if (((RectifyCalculate)this._function).WcsCoordSystem != null)
            {
                userWcsVector _teachVector, _curVector;
                foreach (var item in ((RectifyCalculate)this._function).WcsCoordSystem)
                {
                    _curVector = item.CurrentPoint;
                    _teachVector = item.ReferencePoint.GetPixVector().GetWcsVector(_curVector.Grab_x, _curVector.Grab_y);
                    PlateTeachVector.Add(new drawWcsPoint(_teachVector.X, _teachVector.Y, _teachVector.Z));
                    PlateCurVector.Add(new drawWcsPoint(_curVector.X, _curVector.Y, _curVector.Z));
                    listCur_x.Add(_curVector.X);
                    listCur_y.Add(_curVector.Y);
                    listTeach_x.Add(_teachVector.X);
                    listTeach_y.Add(_teachVector.Y);
                }
            }
            this.示教点dataGridView.DataSource = PlateTeachVector.ToArray();
            this.当前点dataGridView.DataSource = PlateCurVector.ToArray();
            this.BindProperty();
            this.addContextMenu(this.hWindowControl1);
            ////
        }

        private void BindProperty()
        {
            try
            {
                RectifyCalculateParam param = ((RectifyCalculate)this._function).Param; // enAlignmentMethod
                this.视图窗口comboBox.DataSource = HWindowManage.GetKeysList();
                //this.纠偏坐标系comboBox.DataSource = Enum.GetValues(typeof(enCoordSysName));
                //this.纠偏坐标系comboBox.DataBindings.Add("Text", param, nameof(param.CoordSysName), true, DataSourceUpdateMode.OnPropertyChanged);
                this.目标角度XtextBox.DataBindings.Add("Text", param, nameof(param.Angle), true, DataSourceUpdateMode.OnPropertyChanged);
                this.纠偏模式comboBox.DataBindings.Add("Text", param, nameof(param.Mode), true, DataSourceUpdateMode.OnPropertyChanged);
                this.纠偏方向comboBox.DataBindings.Add("Text", param, nameof(param.Orientation), true, DataSourceUpdateMode.OnPropertyChanged);
                this.视图窗口comboBox.DataBindings.Add("Text", param, nameof(param.ViewWindow), true, DataSourceUpdateMode.OnPropertyChanged);
                this.启用分区纠偏checkBox.DataBindings.Add(nameof(this.启用分区纠偏checkBox.Checked), param, nameof(param.IsZoneCompensation), true, DataSourceUpdateMode.OnPropertyChanged);
            }
            catch (Exception ex)
            {
               new Common.UserMessageForm(ex.ToString()).ShowDialog();;
            }
        }

        private void RectifyCalculateForm_FormClosing(object sender, FormClosingEventArgs e)
        {

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
                     new ToolStripMenuItem("自适应图像(Auto)",null,null,"自适应图像(Auto)"),
                     new ToolStripMenuItem("平移",null,null,"平移"),
                     new ToolStripMenuItem("选择",null,null,"选择"),
                     new ToolStripMenuItem("清除窗口(Clear)",null,null,"清除窗口(Clear)"),
                     new ToolStripMenuItem("3D",null,null,"3D"),
                   };
                    break;
                case "en-US":
                    // 添加右键菜单 
                    items = new ToolStripMenuItem[]
                   {
                     new ToolStripMenuItem("Auto Window Image",null,null,"自适应图像(Auto)"),
                     new ToolStripMenuItem("Translation",null,null,"平移"),
                     new ToolStripMenuItem("Select",null,null,"选择"),
                     new ToolStripMenuItem("Clear Window Image",null,null,"清除窗口(Clear)"),
                     new ToolStripMenuItem("3D",null,null,"3D"),
                   };
                    break;
            }
            ContextMenuStrip1.Items.AddRange(items); // 在滑赋值前不能调用 
            ContextMenuStrip1.ItemClicked += new ToolStripItemClickedEventHandler(hWindowControlContextMenuStrip_ItemClicked);
            hWindowControl.ContextMenuStrip = ContextMenuStrip1;
        }
        private void hWindowControlContextMenuStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            string name = e.ClickedItem.Name;
            try
            {
                ((ContextMenuStrip)sender).Close();
                switch (name)
                {
                    case "自适应图像(Auto)":
                        this.drawObject.ClearWindow();
                        //this.toolStripButton_Translate.CheckState = CheckState.Unchecked;
                        break;

                    case "平移":
                        this.drawObject.TranslateScaleImage();
                        //this.toolStripButton_Translate.CheckState = CheckState.Checked;
                        break;

                    case "选择":
                        this.drawObject.Select();
                        //this.toolStripButton_Translate.CheckState = CheckState.Unchecked;
                        break;
                    case "清除窗口(Clear)":
                        this.drawObject.ClearWindow();
                        break;

                    case "3D":
                        this.drawObject.ClearWindow();
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
                                new UserMessageForm().ShowDialog("图像内容为空");
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
                                new UserMessageForm().ShowDialog("点云句柄内容为空");
                        }
                        break;



                }
            }
            catch (Exception ex)
            {
               new Common.UserMessageForm(ex.ToString()).ShowDialog();;
            }
        }
        private void hWindowControl1_MouseMove(object sender, GrayValueInfoEventArgs e)
        {
            if (e.GaryValue.Length > 0)
            {
                int row1, col1, row2, col2;
                this.hWindowControl1.HalconWindow.GetPart(out row1, out col1, out row2, out col2);
                this.hWindowControl1.HalconWindow.SetTposition((int)(row1 + (row2 - row1) * 0.025), (int)(col1 + (col2 - col1) * 0.015));
                this.hWindowControl1.HalconWindow.SetFont("-Consolas-" + 12 + "- *-0-*-*-1-");
                this.hWindowControl1.HalconWindow.SetColor("red");
                this.drawObject.CopyBufferWindowView();
                string content;
                switch (e.GaryValue.Length)
                {
                    default:
                    case 1:
                        content = string.Join("", "Row:", e.Row, " ", "Col:", e.Col, " ", "Gray:", e.GaryValue[0]);
                        break;
                    case 2:
                        content = string.Join("", "Row:", e.Row, " ", "Col:", e.Col, " ", "Gray:", e.GaryValue[0], ", ", e.GaryValue[1]);
                        break;
                    case 3:
                        content = string.Join("", "Row:", e.Row, " ", "Col:", e.Col, " ", "Gray:", e.GaryValue[0], ", ", e.GaryValue[1], ", ", e.GaryValue[2]);
                        break;
                }
                this.hWindowControl1.HalconWindow.WriteString(content);
                //this.hWindowControl1.HalconWindow.WriteString(string.Join("", "Row:", e.Row, " ", "Col:", e.Col, " ", "Gray:", e.GaryValue[0]));
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

        #region  窗体绽放功能 
        //private const int Guying_HTLEFT = 10;
        //private const int Guying_HTRIGHT = 11;
        //private const int Guying_HTTOP = 12;
        //private const int Guying_HTTOPLEFT = 13;
        //private const int Guying_HTTOPRIGHT = 14;
        //private const int Guying_HTBOTTOM = 15;
        //private const int Guying_HTBOTTOMLEFT = 0x10;
        //private const int Guying_HTBOTTOMRIGHT = 17;
        //protected override void WndProc(ref Message m)
        //{
        //    switch (m.Msg)
        //    {
        //        case 0x0084:
        //            base.WndProc(ref m);
        //            Point vPoint = new Point((int)m.LParam & 0xFFFF,
        //                (int)m.LParam >> 16 & 0xFFFF);
        //            vPoint = PointToClient(vPoint);
        //            if (vPoint.X <= 5)
        //                if (vPoint.Y <= 5)
        //                    m.Result = (IntPtr)Guying_HTTOPLEFT;
        //                else if (vPoint.Y >= ClientSize.Height - 5)
        //                    m.Result = (IntPtr)Guying_HTBOTTOMLEFT;
        //                else m.Result = (IntPtr)Guying_HTLEFT;
        //            else if (vPoint.X >= ClientSize.Width - 5)
        //                if (vPoint.Y <= 5)
        //                    m.Result = (IntPtr)Guying_HTTOPRIGHT;
        //                else if (vPoint.Y >= ClientSize.Height - 5)
        //                    m.Result = (IntPtr)Guying_HTBOTTOMRIGHT;
        //                else m.Result = (IntPtr)Guying_HTRIGHT;
        //            else if (vPoint.Y <= 2)
        //                m.Result = (IntPtr)Guying_HTTOP;
        //            else if (vPoint.Y >= ClientSize.Height - 5)
        //                m.Result = (IntPtr)Guying_HTBOTTOM;
        //            break;
        //        default:
        //            base.WndProc(ref m);
        //            break;
        //    }
        //}
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

        private void RectifyCalculateForm_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, WM_SYSCOMMAND, SC_MOVE + HTCAPTION, 0);
        }

        private void titleLabel_MouseDown(object sender, MouseEventArgs e)
        {
            RectifyCalculateForm_MouseDown(null, null);  // 用标签鼠标按下事件来代替窗体鼠标按下事件
        }

        private void titleLabel_MouseEnter(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Hand;
            //this.titleLabel.BackColor = System.Drawing.Color.Orange;
        }

        private void titleLabel_MouseLeave(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Default;
            //this.titleLabel.BackColor = System.Drawing.Color.LightGray;
        }

        private void toolStrip1_ItemClicked_1(object sender, ToolStripItemClickedEventArgs e)
        {
            try
            {
                switch (e.ClickedItem.Name)
                {
                    case nameof(this.toolStripButton_Run):
                    case nameof(this.toolStripButton1):
                        if (this.toolStripStatusLabel2.Text == "等待……") break;
                        this.toolStripStatusLabel2.Text = "等待……";
                        this.toolStripStatusLabel2.ForeColor = Color.Yellow;
                        Task.Run(() =>
                        {
                            if (this._function.Execute(this._refNode).Succss)
                            {
                                this.Invoke(new Action(() =>
                                {
                                    this.toolStripStatusLabel1.Text = "执行结果:";
                                    this.toolStripStatusLabel2.Text = "成功";
                                    this.toolStripStatusLabel2.ForeColor = Color.Green;
                                }));
                            }
                            else
                            {
                                this.Invoke(new Action(() =>
                                {
                                    this.toolStripStatusLabel1.Text = "执行结果:";
                                    this.toolStripStatusLabel2.Text = "失败";
                                    this.toolStripStatusLabel2.ForeColor = Color.Red;
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
            }
        }

        private void 分区纠偏设置Btn_Click(object sender, EventArgs e)
        {
            try
            {
                if (((RectifyCalculate)this._function).ZoneParam == null)
                    ((RectifyCalculate)this._function).ZoneParam = new ZoneRectifyParam();
                new ZoneRectifyForm(((RectifyCalculate)this._function).ZoneParam).ShowDialog();
            }
            catch (Exception ex)
            {
                new Common.UserMessageForm(ex.ToString()).ShowDialog();
            }
        }
    }




}

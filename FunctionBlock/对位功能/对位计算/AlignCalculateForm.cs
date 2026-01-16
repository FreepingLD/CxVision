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
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using View;

namespace FunctionBlock
{
    public partial class AlignCalculateForm : Form
    {
        private Form form;
        private IFunction _function;
        private VisualizeView drawObject;
        private TreeNode _refNode;
        public AlignCalculateForm(IFunction function)
        {
            InitializeComponent();
            this.drawObject = new VisualizeView(this.hWindowControl1, true);
            this._function = function;
            this._function.SetPropertyValues(nameof(TreeNode), this._refNode);
            this.Text = function.GetPropertyValues("名称").ToString();
            new ListBoxWrapClass().InitListBox(this.listBox1, function);
            new ListBoxWrapClass().InitListBox(this.listBox2, function, 2);
        }
        public AlignCalculateForm(TreeNode node)
        {
            InitializeComponent();
            this.drawObject = new VisualizeView(this.hWindowControl1, true);
            this._refNode = node;
            this._function = this._refNode.Tag as IFunction;
            this._function.SetPropertyValues(nameof(TreeNode), this._refNode);
            this.Text = node.Text;
            new ListBoxWrapClass().InitListBox(this.listBox1, node);
            new ListBoxWrapClass().InitListBox(this.listBox2, node, 2);
        }

        private void AlignCalculateForm_Load(object sender, EventArgs e)
        {
            try
            {
                //////////////////////////////////////////
                List<drawWcsPoint> refPoint = new List<drawWcsPoint>();
                List<drawWcsPoint> curPoint = new List<drawWcsPoint>();
                List<drawWcsPoint> affinePoint = new List<drawWcsPoint>();
                List<double> list_target_x = new List<double>();
                List<double> list_target_y = new List<double>();
                List<double> list_target_z = new List<double>();
                List<double> list_affine_x = new List<double>();
                List<double> list_affine_y = new List<double>();
                List<double> list_affine_z = new List<double>();
                //////////////////////////////////////////////////////////
                if (((AlignCalculate)this._function).SourcePoint != null)
                {
                    foreach (var item in ((AlignCalculate)this._function).SourcePoint)
                    {
                        userWcsVector wcsPoint = item.GetWcsVector();
                        curPoint.Add(new drawWcsPoint(wcsPoint.X, wcsPoint.Y, wcsPoint.Z));
                    }
                }
                if (((AlignCalculate)this._function).TargetPoint != null)
                {
                    int index = 0;
                    foreach (var item in ((AlignCalculate)this._function).TargetPoint)
                    {
                        userWcsVector wcsPoint = item.GetWcsVector();
                        refPoint.Add(new drawWcsPoint(wcsPoint.X, wcsPoint.Y, wcsPoint.Z));
                        index++;
                        list_target_x.Add(wcsPoint.X);
                        list_target_y.Add(wcsPoint.Y);
                        list_target_z.Add(wcsPoint.Z);
                    }
                }
                if (((AlignCalculate)this._function).AffinePoint != null)
                {
                    foreach (var item in ((AlignCalculate)this._function).AffinePoint)
                    {
                        affinePoint.Add(new drawWcsPoint(item.X, item.Y, item.Z));
                        list_affine_x.Add(item.X);
                        list_affine_y.Add(item.Y);
                        list_affine_z.Add(item.Z);
                    }
                }
                this.目标点坐标dataGridView.DataSource = refPoint.ToArray();
                this.源点坐标dataGridView.DataSource = curPoint.ToArray();
                this.变换点坐标dataGridView.DataSource = affinePoint.ToArray();
                BindProperty();
                HObjectModel3D hObjectModel3D1 = new HObjectModel3D(list_target_x.ToArray(), list_target_y.ToArray(), list_target_z.ToArray());
                HObjectModel3D hObjectModel3D2 = new HObjectModel3D(list_affine_x.ToArray(), list_affine_y.ToArray(), list_affine_z.ToArray());
                this.drawObject.PointCloudModel3D = new PointCloudData(new HObjectModel3D[] { hObjectModel3D1, hObjectModel3D2 });
            }
            catch (Exception ex)
            {
                new Common.UserMessageForm(ex.ToString()).ShowDialog();
            }
        }

        private void BindProperty()
        {
            try
            {
                CompensationParam param = ((AlignCalculate)this._function).Param;
                this.参考点comboBox.DataSource = Enum.GetValues(typeof(enRefObject));
                this.对齐方式comboBox.DataSource = Enum.GetValues(typeof(enAlignmentMethod));
                //this.坐标系comboBox.DataSource = Enum.GetValues(typeof(enCoordSysName));
                this.视图窗口comboBox.DataSource = HWindowManage.GetKeysList();
                this.数据视图窗口comboBox.DataSource = HWindowManage.GetKeysList();
                this.参考点comboBox.DataBindings.Add("Text", param, nameof(param.RefObject), true, DataSourceUpdateMode.OnPropertyChanged);
                this.对齐方式comboBox.DataBindings.Add("Text", param, nameof(param.AlignmentMethod), true, DataSourceUpdateMode.OnPropertyChanged);
                this.补偿XtextBox.DataBindings.Add("Text", param, nameof(param.Add_X), true, DataSourceUpdateMode.OnPropertyChanged);
                this.补偿YtextBox.DataBindings.Add("Text", param, nameof(param.Add_Y), true, DataSourceUpdateMode.OnPropertyChanged);
                this.补偿ThetatextBox.DataBindings.Add("Text", param, nameof(param.Add_Angle), true, DataSourceUpdateMode.OnPropertyChanged);
                this.视图窗口comboBox.DataBindings.Add("Text", param, nameof(param.ViewWindow), true, DataSourceUpdateMode.OnPropertyChanged);
                this.数据视图窗口comboBox.DataBindings.Add("Text", param, nameof(param.DataViewWindow), true, DataSourceUpdateMode.OnPropertyChanged);
                //this.坐标系comboBox .DataBindings.Add("Text", param, nameof(param.CoordSysName), true, DataSourceUpdateMode.OnPropertyChanged); 
                //this.启用分区补偿checkBox.DataBindings.Add(nameof(this.启用分区补偿checkBox.Checked), param, nameof(param.IsZoneCompensation), true, DataSourceUpdateMode.OnPropertyChanged);
                this.输出UVW坐标checkBox.DataBindings.Add(nameof(this.输出UVW坐标checkBox.Checked), param, nameof(param.IsOutputUvw), true, DataSourceUpdateMode.OnPropertyChanged);
            }
            catch
            {

            }
        }
        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            try
            {
                switch (e.ClickedItem.Text)
                {
                    case "执行":
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
        private void AlignCalculateForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                this.drawObject.PointCloudModel3D?.ClearObjectModel3d();
            }
            catch
            {

            }
        }


        private void PLC信息dataGridView_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false;
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
                new Common.UserMessageForm(ex.ToString()).ShowDialog(); ;
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


        private void titleLabel_MouseDown(object sender, MouseEventArgs e)
        {
            AlignCalculateForm_MouseDown(null, null);  // 用标签鼠标按下事件来代替窗体鼠标按下事件
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

        private void AlignCalculateForm_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, WM_SYSCOMMAND, SC_MOVE + HTCAPTION, 0);
        }

        private void 确定Btn_Click(object sender, EventArgs e)
        {
            try
            {
                //switch (this.确定Btn.Name)
                //{
                //    case nameof(this.确定Btn):
                //        this.确定Btn.Enabled = false;
                //        if (this.toolStripStatusLabel2.Text == "等待……") break;
                //        this.toolStripStatusLabel2.Text = "等待……";
                //        this.toolStripStatusLabel2.ForeColor = Color.Yellow;
                //        Task.Run(() =>
                //        {
                //            if (this._function.Execute(null).Succss)
                //            {
                //                this.Invoke(new Action(() =>
                //                {
                //                    this.确定Btn.Enabled = true;
                //                    this.toolStripStatusLabel1.Text = "执行结果:";
                //                    this.toolStripStatusLabel2.Text = "成功";
                //                    this.toolStripStatusLabel2.ForeColor = Color.Green;
                //                }));
                //            }
                //            else
                //            {
                //                this.Invoke(new Action(() =>
                //                {
                //                    this.确定Btn.Enabled = true;
                //                    this.toolStripStatusLabel1.Text = "执行结果:";
                //                    this.toolStripStatusLabel2.Text = "失败";
                //                    this.toolStripStatusLabel2.ForeColor = Color.Red;
                //                }));
                //            }
                //        }
                //        );
                //        break;
                //}
            }
            catch (Exception ex)
            {
                new Common.UserMessageForm(ex.ToString()).ShowDialog();
            }
        }

        private void toolStrip1_ItemClicked_1(object sender, ToolStripItemClickedEventArgs e)
        {
            try
            {
                switch (e.ClickedItem.Name)
                {
                    case nameof(this.toolStripButton_Run):
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

        private void 分区补偿设置Btn_Click(object sender, EventArgs e)
        {
            try
            {
                if (((AlignCalculate)this._function).ZoneParam == null)
                    ((AlignCalculate)this._function).ZoneParam = new ZoneCompensationParam();
                new ZoneCompensationForm(((AlignCalculate)this._function).ZoneParam).ShowDialog();
            }
            catch (Exception ex)
            {
                new Common.UserMessageForm(ex.ToString()).ShowDialog();
            }
        }

        private void 轮廓匹配参数Btn_Click(object sender, EventArgs e)
        {
            try
            {
                if (((AlignCalculate)this._function).Param.MatchParam == null)
                    ((AlignCalculate)this._function).Param.MatchParam = new AlignMatchParam();
                new AlignParamForm(((AlignCalculate)this._function).Param.MatchParam).Show();
            }
            catch (Exception ex)
            {
                new Common.UserMessageForm(ex.ToString()).ShowDialog();
            }
        }
    }


}

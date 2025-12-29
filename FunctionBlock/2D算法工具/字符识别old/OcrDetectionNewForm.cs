using AlgorithmsLibrary;
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
using System.Threading.Tasks;
using System.Windows.Forms;
using View;

namespace FunctionBlock
{
    public partial class OcrDetectionNewForm : Form
    {
        private object _objectDataModel;
        private Form form;
        private IFunction _function;
        private VisualizeView drawObject;
        private TreeNode node;
        public OcrDetectionNewForm(IFunction function, TreeNode node)
        {
            this._function = function;
            this.node = node;
            InitializeComponent();
            this.titleLabel.Text = function.GetPropertyValues("名称").ToString();
            this.drawObject = new userDrawRect2ROI(this.hWindowControl1, false);
            new ListBoxWrapClass().InitListBox(this.listBox1, node);
            new ListBoxWrapClass().InitListBox(this.listBox2, node, 2);
        }
        private void OcrDetectionForm_Load(object sender, EventArgs e)
        {
            //////////////////////////////////////
            this.drawObject.BackImage = ((FunctionBlock.Ocr)_function).ImageData;
            this.drawObject.GrayValueInfo += new GrayValueInfoEventHandler(GetGrayValueInfo);
            BindProperty();
        }
        private void BindProperty()
        {
            try
            {
                //this.propertyGrid1.SelectedObject = ((FunctionBlock.Ocr)this._function).CharDetection.OcrParam;
                //this.Ocr模型comboBox.DataSource = Enum.GetNames(typeof(FunctionBlock.enOcrModel));
                //this.Ocr模型comboBox.DataBindings.Add("Text", ((FunctionBlock.Ocr)this._function).CharDetection, "OcrModel", true, DataSourceUpdateMode.OnPropertyChanged);
                //this.dataGridView1.DataSource = ((Ocr)this._function).CharDetection.OcrParam.ChartRegion;
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
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
                            if (this._function.Execute(this.node).Succss)
                            {
                                this.Invoke(new Action(() =>
                                {
                                    this.toolStripStatusLabel1.Text = "执行结果:";
                                    this.toolStripStatusLabel2.Text = "成功";
                                    this.toolStripStatusLabel2.ForeColor = Color.Green;
                                }));
                                //this.drawObject.DetachDrawingObjectFromWindow();
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
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
            }
        }

        private void OcrDetectionForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (this.drawObject != null)
                    this.drawObject.ClearDrawingObject();
                this.drawObject.GrayValueInfo -= new GrayValueInfoEventHandler(GetGrayValueInfo);
            }
            catch
            {

            }
        }

        private void 结果dataGridView_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false;
        }




        private void GetGrayValueInfo(object sender, GrayValueInfoEventArgs e)
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

        private void Ocr模型comboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            try
            {
                if (this.Ocr模型comboBox.SelectedIndex == -1) return;
                //switch (this.Ocr模型comboBox.SelectedItem.ToString())
                //{
                //    case nameof(enOcrModel.OcrCNN):
                //        ((FunctionBlock.Ocr)this._function).CharDetection.OcrParam = new OcrCnnParam();
                //        this.propertyGrid1.SelectedObject = ((FunctionBlock.Ocr)this._function).CharDetection.OcrParam;
                //        //this.dataGridView1.DataSource = ((FunctionBlock.Ocr)this._function).CharDetection.OcrParam.ChartRegion;
                //        break;
                //    case nameof(enOcrModel.OcrKNN):
                //        ((FunctionBlock.Ocr)this._function).CharDetection.OcrParam = new OcrKnnParam();
                //        this.propertyGrid1.SelectedObject = ((FunctionBlock.Ocr)this._function).CharDetection.OcrParam;
                //        //this.dataGridView1.DataSource = ((FunctionBlock.Ocr)this._function).CharDetection.OcrParam.ChartRegion;
                //        break;
                //    case nameof(enOcrModel.OcrSVM):
                //        ((FunctionBlock.Ocr)this._function).CharDetection.OcrParam = new OcrSvmParam();
                //        this.propertyGrid1.SelectedObject = ((FunctionBlock.Ocr)this._function).CharDetection.OcrParam;
                //        //this.dataGridView1.DataSource = ((FunctionBlock.Ocr)this._function).CharDetection.OcrParam.ChartRegion;
                //        break;
                //    case nameof(enOcrModel.OcrMLP):
                //        ((FunctionBlock.Ocr)this._function).CharDetection.OcrParam = new OcrMlpParam();
                //        this.propertyGrid1.SelectedObject = ((FunctionBlock.Ocr)this._function).CharDetection.OcrParam;
                //        //this.dataGridView1.DataSource = ((FunctionBlock.Ocr)this._function).CharDetection.OcrParam.ChartRegion;
                //        break;
                //    case nameof(enOcrModel.OcrTextMode):
                //        ((FunctionBlock.Ocr)this._function).CharDetection.OcrParam = new OcrTextModelParam();
                //        this.propertyGrid1.SelectedObject = ((FunctionBlock.Ocr)this._function).CharDetection.OcrParam;
                //        //this.dataGridView1.DataSource = ((FunctionBlock.Ocr)this._function).CharDetection.OcrParam.ChartRegion;
                //        break;
                //}
            }
            catch
            {

            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                HRegion hRegion = null, threRegion = null, operateRegion = null, selectRegion = null, connectionRegion = null, unionRegion = null,
                    sortRegion = null;
                HImage reduceImage = null, invertImage;
                //HImage hImage = ((FunctionBlock.Ocr)this._function).ImageData?.Image;
                //DoOcr ocr = ((FunctionBlock.Ocr)this._function).CharDetection;
                //OcrParam ocrParam = ((FunctionBlock.Ocr)this._function).CharDetection.OcrParam;
                //switch (this.comboBox1.SelectedItem.ToString())
                //{
                //    case "源图像":
                //        this.drawObject.BackImage = ((FunctionBlock.Ocr)this._function).ImageData;
                //        break;
                //    case "取反图像":
                //        this.drawObject.BackImage = ((FunctionBlock.Ocr)this._function).ImageData.InvertImage();
                //        break;
                //    case "字符区域":
                //        //hRegion = new HRegion();
                //        //hRegion.GenEmptyObj();
                //        //foreach (var item in ocrParam.ChartRegion)
                //        //{
                //        //    HRegion hRegion1 = new HRegion();
                //        //    hRegion1 = item.RoiShape.GetRegion();
                //        //    hRegion = hRegion.ConcatObj(hRegion1);
                //        //    hRegion1.Dispose();
                //        //}
                //        //unionRegion = hRegion.Union1();
                //        //invertImage = hImage.InvertImage();
                //        //reduceImage = invertImage.ReduceDomain(unionRegion);
                //        //threRegion = reduceImage.Threshold(ocrParam.MinThreshold, ocrParam.MaxThreshold);
                //        //switch (ocrParam.RegionOperate)
                //        //{
                //        //    default:
                //        //    case enRegionOperate.NONE:
                //        //        operateRegion = threRegion;
                //        //        break;
                //        //    case enRegionOperate.closing_rectangle1:
                //        //        operateRegion = threRegion.ClosingRectangle1(ocrParam.MaskWidth, ocrParam.MaskHeight);
                //        //        break;
                //        //    case enRegionOperate.opening_rectangle1:
                //        //        operateRegion = threRegion.OpeningRectangle1(ocrParam.MaskWidth, ocrParam.MaskHeight);
                //        //        break;
                //        //}
                //        //connectionRegion = operateRegion.Connection();
                //        //selectRegion = connectionRegion.SelectShape("area", "and", ocrParam.MinArea, ocrParam.MaxArea);
                //        /////////////////////////////////
                //        //this.drawObject.AttachPropertyData.Clear();
                //        //this.drawObject.AttachPropertyData.Add(selectRegion);
                //        //this.drawObject.DrawingGraphicObject();
                //        //unionRegion?.Dispose();
                //        //invertImage?.Dispose();
                //        //threRegion?.Dispose();
                //        //reduceImage?.Dispose();
                //        break;
                //}
            }
            catch
            {

            }
        }


        private void 视图工具toolStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            HalconLibrary ha = new HalconLibrary();
            string name = e.ClickedItem.Name;
            switch (name)
            {
                case "toolStripButton_Clear":
                    this.drawObject.ClearWindow();
                    this.toolStripButton_Translate.CheckState = CheckState.Unchecked;
                    break;
                case "toolStripButton_Select":
                    this.drawObject.Select();
                    this.toolStripButton_Translate.CheckState = CheckState.Unchecked;
                    break;
                case "toolStripButton_Translate":
                    this.drawObject.TranslateScaleImage();
                    this.toolStripButton_Translate.CheckState = CheckState.Checked;
                    break;
                case "toolStripButton_Auto":
                    this.drawObject.AutoWindows();
                    this.toolStripButton_Translate.CheckState = CheckState.Unchecked;

                    break;
                case "toolStripButton_3D":
                    this.drawObject.Show3D();
                    this.toolStripButton_Translate.CheckState = CheckState.Unchecked;

                    break;
                default:
                    break;
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
            //DialogResult dialogResult = MessageBox.Show("确定关闭窗体吗？", "关闭窗体", MessageBoxButtons.YesNo);
            //if (dialogResult == DialogResult.Yes)
            //{
            this.Close();  //关闭窗口
            //}
        }
        #endregion


        private void titleLabel_MouseDown(object sender, MouseEventArgs e)
        {
            OcrDetectionForm_MouseDown(null, null);  // 用标签鼠标按下事件来代替窗体鼠标按下事件
        }

        private void titleLabel_MouseEnter(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Hand;
            this.titleLabel.BackColor = System.Drawing.Color.Orange;// 
        }

        private void titleLabel_MouseLeave(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Default;
            this.titleLabel.BackColor = System.Drawing.Color.Orange;// S
            //this.titleLabel.BackColor = System.Drawing.Color.LightGray;//
        }

        private void OcrDetectionForm_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, WM_SYSCOMMAND, SC_MOVE + HTCAPTION, 0);
        }
    }
}

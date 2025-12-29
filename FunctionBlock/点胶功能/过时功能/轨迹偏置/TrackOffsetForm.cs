
using Common;
using FunctionBlock;
using HalconDotNet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
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
    public partial class TrackOffsetForm : Form
    {
        protected Form form;
        protected IFunction _function;
        private VisualizeView drawObject;
        public TrackOffsetForm(IFunction function)
        {
            InitializeComponent();
            this.titleLabel.Text = function.GetPropertyValues("名称").ToString();
            this._function = function;
            this.drawObject = new VisualizeView(this.hWindowControl1, true);
            new ListBoxWrapClass().InitListBox(this.listBox1, function);
            //new ListBoxWrapClass().InitListBox(this.listBox2, function, 2);
        }
        public TrackOffsetForm(TreeNode node)
        {
            InitializeComponent();
            this._function = node.Tag as IFunction;
            this.titleLabel.Text = this._function?.GetPropertyValues("名称").ToString();
            this.drawObject = new VisualizeView(this.hWindowControl1, true);
            new ListBoxWrapClass().InitListBox(this.listBox1, node);
        }  //,TreeNode node
        public void TrackOffsetForm_Load(object sender, EventArgs e)
        {
            // 注册事件
            BaseFunction.ExcuteCompleted += new ExcuteCompletedEventHandler(this.DisplayObjectModel);
            ListBoxWrapClass.ItemsChangeToForm += new ItemsChangeEventHandler(listbox_AddItems);
            BindProperty();
        }
        public enum enShowItems
        {
            输入轨迹,
            偏置轨迹,
            所有轨迹,
        }

        protected void BindProperty()
        {
            try
            {
                // 创建匹配参数
                this.显示条目comboBox.DataSource = Enum.GetNames(typeof(enShowItems));
                this.排序方法comboBox.DataSource = Enum.GetValues(typeof(enSortPoint));
                //this.引导对象comboBox.DataSource = Enum.GetValues(typeof(enRobotJawEnum));
                TrackOffsetParam Param = ((TrackOffset)this._function).Param;
                this.排序方法comboBox.DataBindings.Add("Text", Param, nameof(Param.SortMethod), true, DataSourceUpdateMode.OnPropertyChanged);
                this.偏l置距离comboBox.DataBindings.Add("Text", Param, nameof(Param.Distance), true, DataSourceUpdateMode.OnPropertyChanged);
            }
            catch (Exception ex)
            {
               new Common.UserMessageForm(ex.ToString()).ShowDialog();;
            }
        }
        public void DisplayObjectModel(object sender, ExcuteCompletedEventArgs e)
        {
            try
            {
                if (e.DataContent != null) // 在图形窗口只显示世界坐标元素，像素元素在图像窗口显示
                {
                    switch (e.DataContent.GetType().Name) //这里只接受XLD轮廓或3D对象轮廓
                    {
                        case "PointCloudData":
                            this.drawObject.PointCloudModel3D = ((PointCloudData)e.DataContent);
                            break;
                        case "HImage":
                            this.drawObject.ClearViewObject();
                            this.drawObject.BackImage = new ImageDataClass((HImage)e.DataContent); // 图形窗口不显示图像
                            break;
                        case "ImageDataClass":
                            this.drawObject.ClearViewObject();
                            this.drawObject.BackImage = (ImageDataClass)e.DataContent;
                            break;
                        case "userWcsVector[]":
                            userWcsVector[] wcsVectors = (userWcsVector[])e.DataContent;
                            //this.drawObject.ClearViewObject();
                            foreach (var item in wcsVectors)
                            {
                                this.drawObject.AddViewObject(new ViewData(item.GetPixVector().GetXLD(200), "yellow")); // 默认绿色
                            }
                            break;
                        case nameof(HXLDCont):
                            this.drawObject.AddViewObject(new ViewData((HXLDCont)e.DataContent, "red"));
                            break;
                        case nameof(userWcsPolyLine):
                            //this.drawObject.ClearViewObject(); // 清空视图
                            //userWcsPolyLine wcsPolyLine = (userWcsPolyLine)e.DataContent;
                            //for (int i = 0; i < wcsPolyLine.X.Count; i++)
                            //{
                            //    this.drawObject.AddViewObject(new ViewData(new HXLDCont(wcsPolyLine.X[i], wcsPolyLine.Y[i]))); // 默认绿色
                            //}
                            //this.drawObject.AddViewObject(new ViewData(wcsPolyLine.GetXLD(), "red"));

                            this.Invoke(new Action(() => this.显示条目comboBox.SelectedIndex = 2));
                            break;
                        case nameof(userWcsPolygon):
                            this.drawObject.ClearViewObject(); // 清空视图
                            userWcsPolygon wcsPolygon = (userWcsPolygon)e.DataContent;
                            for (int i = 0; i < wcsPolygon.X.Count; i++)
                            {
                                this.drawObject.AddViewObject(new ViewData(new HXLDCont(wcsPolygon.X[i], wcsPolygon.Y[i]))); // 默认绿色
                            }
                            this.drawObject.AddViewObject(new ViewData(wcsPolygon.GetXLD(), "red"));
                            break;
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
        private void listbox_AddItems(object send, ItemsChangeEventArgs e)
        {
            try
            {
                object object3D;
                if (e.ItemName == null || e.ItemName.Trim().Length == 0) return;
                if (e.ItemName.Split('.').Length == 1)
                    object3D = ((IFunction)e.Function).GetPropertyValues(e.ItemName);
                else
                    object3D = ((IFunction)e.Function).GetPropertyValues(e.ItemName.Split('.')[1]);
                ///////////////////////////////////////////
                if (object3D != null)
                {
                    switch (object3D.GetType().Name) //这里只接受XLD轮廓或3D对象轮廓
                    {
                        case "PointCloudData":
                            this.drawObject.PointCloudModel3D = ((PointCloudData)object3D);
                            break;
                        case "HImage":
                            this.drawObject.ClearViewObject();
                            this.drawObject.BackImage = new ImageDataClass((HImage)object3D); // 图形窗口不显示图像
                            break;
                        case "ImageDataClass":
                            this.drawObject.ClearViewObject();
                            this.drawObject.BackImage = (ImageDataClass)object3D;
                            break;
                        case "XldDataClass":
                            this.drawObject.AddViewObject(new ViewData(((XldDataClass)object3D).HXldCont, ((XldDataClass)object3D).Color.ToString()));
                            break;
                        case "HXLDCont":
                            this.drawObject.AddViewObject(new ViewData(object3D, "red"));
                            break;
                        case "RegionDataClass":
                            this.drawObject.AddViewObject(new ViewData(((RegionDataClass)object3D).Region, ((RegionDataClass)object3D).Color.ToString()));
                            break;
                        case "userWcsCircle":
                            this.drawObject.AddViewObject(new ViewData(((userWcsCircle)object3D), ((userWcsCircle)object3D).Color.ToString()));
                            break;
                        case "userWcsCircleSector":
                            this.drawObject.AddViewObject(new ViewData(((userWcsCircleSector)object3D), ((userWcsCircleSector)object3D).Color.ToString()));
                            break;
                        case "userWcsEllipse":
                            this.drawObject.AddViewObject(new ViewData(((userWcsEllipse)object3D), ((userWcsEllipse)object3D).Color.ToString()));
                            break;
                        case "userWcsEllipseSector":
                            this.drawObject.AddViewObject(new ViewData(((userWcsEllipseSector)object3D), ((userWcsEllipseSector)object3D).Color.ToString()));
                            break;
                        case "userWcsLine":
                            this.drawObject.AddViewObject(new ViewData(((userWcsLine)object3D), ((userWcsLine)object3D).Color.ToString()));
                            break;
                        case "userWcsPoint":
                            this.drawObject.AddViewObject(new ViewData(((userWcsPoint)object3D), ((userWcsPoint)object3D).Color.ToString()));
                            break;
                        case "userWcsRectangle1":
                            this.drawObject.AddViewObject(new ViewData(((userWcsRectangle1)object3D), ((userWcsRectangle1)object3D).Color.ToString()));
                            break;
                        case "userWcsRectangle2":
                            this.drawObject.AddViewObject(new ViewData(((userWcsRectangle2)object3D), ((userWcsRectangle2)object3D).Color.ToString()));
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
               new Common.UserMessageForm(ex.ToString()).ShowDialog();;
            }
        }
        private void 显示条目comboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            //try
            //{
            //    userWcsPolyLine wcsPolyLine ;
            //    userPixPolyLine pixPolyLine;
            //    switch (this.显示条目comboBox.Text.Trim())
            //    {
            //        case nameof(enShowItems.输入轨迹):
            //            this.drawObject.ClearViewObject();
            //            wcsPolyLine = new userWcsPolyLine();
            //            userWcsPoint[] wcsPoints = ((TrackOffset)this._function).TrackPoint;
            //            if (wcsPoints == null) return;
            //            foreach (var item in wcsPoints)
            //            {
            //                wcsPolyLine.Add(item.X, item.Y);
            //            }
            //            PointCloudData pointCloudModel3D = new PointCloudData(new HObjectModel3D(wcsPolyLine.X.ToArray(), wcsPolyLine.Y.ToArray(), wcsPolyLine.Z.ToArray()));
            //            wcsPolyLine.CamParams = new CameraParam();
            //            wcsPolyLine.CamParams.CamParam = pointCloudModel3D.LaserParams.CamParam.Clone();
            //            wcsPolyLine.CamParams.CamPose = pointCloudModel3D.LaserParams.CamPose.Clone();
            //            wcsPolyLine.CamParams.CaliParam.CamCaliModel = enCamCaliModel.CamParamPose;
            //            pointCloudModel3D.Dispose();
            //            this.drawObject.SetViewParam(wcsPolyLine.CamParams.CamParam.Width, wcsPolyLine.CamParams.CamParam.Height);
            //            pixPolyLine = wcsPolyLine.GetPixPolyLine();
            //            for (int i = 0; i < pixPolyLine.Row.Count; i++)
            //            {
            //                this.drawObject.AddViewObject(new ViewData(new userPixPoint(pixPolyLine.Row[i], pixPolyLine.Col[i],enColor.green)));
            //            }
            //            //this.drawObject.AddViewObject(new ViewData(new HXLDCont(pixPolyLine.Row.ToArray(), pixPolyLine.Col.ToArray())));
            //            break;
            //        case nameof(enShowItems.偏置轨迹):
            //            this.drawObject.ClearViewObject();
            //            wcsPolyLine = (((TrackOffset)this._function).WcsPolyLine);
            //            if (wcsPolyLine == null) return;
            //            pixPolyLine = wcsPolyLine.GetPixPolyLine();
            //            this.drawObject.SetViewParam(pixPolyLine.CamParams.CamParam.Width, pixPolyLine.CamParams.CamParam.Height);
            //            for (int i = 0; i < pixPolyLine.Row.Count; i++)
            //            {
            //                this.drawObject.AddViewObject(new ViewData(new userPixPoint(pixPolyLine.Row[i], pixPolyLine.Col[i], enColor.red), "red"));
            //            }
            //            //this.drawObject.AddViewObject(new ViewData(new HXLDCont(pixPolyLine.Row.ToArray(), pixPolyLine.Col.ToArray())));
            //            break;
            //        case nameof(enShowItems.所有轨迹):
            //            this.drawObject.ClearViewObject();
            //            /////////////////////////  先画偏置轨迹，再画输入轨迹  ///////////
            //            wcsPolyLine = (((TrackOffset)this._function).WcsPolyLine);
            //            if (wcsPolyLine == null) return;
            //            pixPolyLine = wcsPolyLine.GetPixPolyLine();
            //            this.drawObject.SetViewParam(pixPolyLine.CamParams.CamParam.Width, pixPolyLine.CamParams.CamParam.Height);
            //            for (int i = 0; i < pixPolyLine.Row.Count; i++)
            //            {
            //                this.drawObject.AddViewObject(new ViewData(new userPixPoint(pixPolyLine.Row[i], pixPolyLine.Col[i], enColor.red)));
            //            }
            //            //this.drawObject.AddViewObject(new ViewData(new HXLDCont(pixPolyLine.Row.ToArray(), pixPolyLine.Col.ToArray()), "green"));
            //            //////////////////////////////// 后画原始轨迹  ////////////////////////
            //            wcsPoints = ((TrackOffset)this._function).TrackPoint; 
            //            if (wcsPoints == null) return;
            //            userWcsPolyLine wcsPolyLine2 = new userWcsPolyLine(pixPolyLine.CamParams);
            //            foreach (var item in wcsPoints)
            //            {
            //                wcsPolyLine2.Add(item.X, item.Y);
            //            }
            //            pixPolyLine = wcsPolyLine2.GetPixPolyLine();
            //            for (int i = 0; i < pixPolyLine.Row.Count; i++)
            //            {
            //                this.drawObject.AddViewObject(new ViewData(new userPixPoint(pixPolyLine.Row[i], pixPolyLine.Col[i], enColor.green)));
            //            }
            //            //this.drawObject.AddViewObject(new ViewData(new HXLDCont(pixPolyLine.Row.ToArray(), pixPolyLine.Col.ToArray()), "red"));
            //            break;
            //    }
            //}
            //catch (Exception ex)
            //{
            //   new Common.UserMessageForm(ex.ToString()).ShowDialog();;
            //}
        }

        private void 显示条目comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.显示条目comboBox.SelectedIndex == -1) return;
            try
            {
                userWcsPolyLine wcsPolyLine;
                userPixPolyLine pixPolyLine;
                switch (this.显示条目comboBox.Text.Trim())
                {
                    case nameof(enShowItems.输入轨迹):
                        this.drawObject.ClearViewObject();
                        wcsPolyLine = new userWcsPolyLine();
                        userWcsPoint[] wcsPoints = ((TrackOffset)this._function).TrackPoint;
                        if (wcsPoints == null) return;
                        foreach (var item in wcsPoints)
                        {
                            wcsPolyLine.Add(item.X, item.Y);
                        }
                        PointCloudData pointCloudModel3D = new PointCloudData(new HObjectModel3D(wcsPolyLine.X.ToArray(), wcsPolyLine.Y.ToArray(), wcsPolyLine.Z.ToArray()));
                        wcsPolyLine.CamParams = new CameraParam();
                        wcsPolyLine.CamParams.CamParam = pointCloudModel3D.LaserParams.CamParam.Clone();
                        wcsPolyLine.CamParams.CamPose = pointCloudModel3D.LaserParams.CamPose.Clone();
                        wcsPolyLine.CamParams.CaliParam.CamCaliModel = enCamCaliModel.CamParamPose;
                        pointCloudModel3D.Dispose();
                        this.drawObject.SetViewParam(wcsPolyLine.CamParams.CamParam.Width, wcsPolyLine.CamParams.CamParam.Height);
                        pixPolyLine = wcsPolyLine.GetPixPolyLine();
                        for (int i = 0; i < pixPolyLine.Row.Count; i++)
                        {
                            this.drawObject.AddViewObject(new ViewData(new userPixPoint(pixPolyLine.Row[i], pixPolyLine.Col[i], enColor.green)));
                        }
                        break;
                    case nameof(enShowItems.偏置轨迹):
                        this.drawObject.ClearViewObject();
                        wcsPolyLine = (((TrackOffset)this._function).WcsPolyLine);
                        if (wcsPolyLine == null) return;
                        pixPolyLine = wcsPolyLine.GetPixPolyLine();
                        this.drawObject.SetViewParam(pixPolyLine.CamParams.CamParam.Width, pixPolyLine.CamParams.CamParam.Height);
                        for (int i = 0; i < pixPolyLine.Row.Count; i++)
                        {
                            this.drawObject.AddViewObject(new ViewData(new userPixPoint(pixPolyLine.Row[i], pixPolyLine.Col[i], enColor.red), "red"));
                        }
                        break;
                    case nameof(enShowItems.所有轨迹):
                        this.drawObject.ClearViewObject();
                        /////////////////////////  先画偏置轨迹，再画输入轨迹  ///////////
                        wcsPolyLine = (((TrackOffset)this._function).WcsPolyLine);
                        if (wcsPolyLine == null) return;
                        pixPolyLine = wcsPolyLine.GetPixPolyLine();
                        this.drawObject.SetViewParam(pixPolyLine.CamParams.CamParam.Width, pixPolyLine.CamParams.CamParam.Height);
                        for (int i = 0; i < pixPolyLine.Row.Count; i++)
                        {
                            this.drawObject.AddViewObject(new ViewData(new userPixPoint(pixPolyLine.Row[i], pixPolyLine.Col[i], enColor.red)));
                        }
                        //////////////////////////////// 后画原始轨迹  ////////////////////////
                        wcsPoints = ((TrackOffset)this._function).TrackPoint;
                        if (wcsPoints == null) return;
                        userWcsPolyLine wcsPolyLine2 = new userWcsPolyLine(pixPolyLine.CamParams);
                        foreach (var item in wcsPoints)
                        {
                            wcsPolyLine2.Add(item.X, item.Y);
                        }
                        pixPolyLine = wcsPolyLine2.GetPixPolyLine();
                        for (int i = 0; i < pixPolyLine.Row.Count; i++)
                        {
                            this.drawObject.AddViewObject(new ViewData(new userPixPoint(pixPolyLine.Row[i], pixPolyLine.Col[i], enColor.green)));
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
               new Common.UserMessageForm(ex.ToString()).ShowDialog();;
            }
        }

        private void 运行toolStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
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
                            if (this._function.Execute(null).Succss)
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
        protected void LineOffsetForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // 注消事件
            try
            {
                if (this.drawObject != null)
                    this.drawObject.ClearDrawingObject();
                BaseFunction.ExcuteCompleted -= new ExcuteCompletedEventHandler(this.DisplayObjectModel);
                ListBoxWrapClass.ItemsChangeToForm -= new ItemsChangeEventHandler(listbox_AddItems);
            }
            catch
            {

            }
        }

        private void 结果dataGridView_DataError(object sender, DataGridViewDataErrorEventArgs e)
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
            //DialogResult dialogResult = new Common.UserMessageForm().ShowDialog("确定关闭窗体吗？", "关闭窗体");
            //if (dialogResult == DialogResult.OK)
            //{
            this.Close();  //关闭窗口
            //}
        }
        #endregion


        private void titleLabel_MouseDown(object sender, MouseEventArgs e)
        {
            TrackOffsetForm_MouseDown(null, null);  // 用标签鼠标按下事件来代替窗体鼠标按下事件
        }

        private void titleLabel_MouseEnter(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Hand;
            this.titleLabel.BackColor = System.Drawing.Color.Orange;
        }

        private void titleLabel_MouseLeave(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Default;
            this.titleLabel.BackColor = System.Drawing.Color.Orange;
            //this.titleLabel.BackColor = System.Drawing.Color.LightGray;
        }

        private void TrackOffsetForm_MouseDown(object sender, MouseEventArgs e)
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

        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
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
                            if (this._function.Execute(null).Succss)
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

        private void 视图工具toolStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            string name = e.ClickedItem.Name;
            switch (name)
            {
                case nameof(toolStripButton_Clear):
                    this.drawObject.ClearWindow();
                    this.toolStripButton_Translate.CheckState = CheckState.Unchecked;
                    break;
                case nameof(toolStripButton_Select):
                    this.drawObject.Select();
                    this.toolStripButton_Translate.CheckState = CheckState.Unchecked;
                    break;
                case nameof(toolStripButton_Translate):
                    this.drawObject.TranslateScaleImage();
                    this.toolStripButton_Translate.CheckState = CheckState.Checked;
                    break;
                case nameof(toolStripButton_Auto):
                    this.drawObject.AutoWindows();
                    this.toolStripButton_Translate.CheckState = CheckState.Unchecked;

                    break;
                case nameof(toolStripButton_3D):
                    this.drawObject.Show3D();
                    this.toolStripButton_Translate.CheckState = CheckState.Unchecked;

                    break;
                default:
                    break;
            }
        }


    }
}

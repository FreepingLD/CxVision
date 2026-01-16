using AlgorithmsLibrary;
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
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using View;
using static FunctionBlock.ContourModelMatch2DForm;

namespace FunctionBlock
{
    public partial class ContourExtractForm : Form
    {
        private Form form;
        private IFunction _function;
        private VisualizeView drawObject;
        private PointCloudData _objectDataModel;
        private HObjectModel3D hObjectModel3D1 = null, hObjectModel3D2 = null;
        private TreeNode _refNode;
        public ContourExtractForm(TreeNode node)
        {
            InitializeComponent();
            this._refNode = node;
            this._function = node.Tag as IFunction;
            this._function.SetPropertyValues(nameof(TreeNode), this._refNode);
            this.drawObject = new VisualizeView(this.hWindowControl1, false);
            new ListBoxWrapClass().InitListBox(this.listBox1, node);
            //new ListBoxWrapClass().InitListBox(this.listBox2, node, 2);
            //this.addContextMenu(this.dataGridView1);
        }
        private void TrackExtractForm_Load(object sender, EventArgs e)
        {
            // 注册事件
            this.drawObject.GrayValueInfo += new GrayValueInfoEventHandler(GetGrayValueInfo);
            BaseFunction.ExcuteCompleted += new ExcuteCompletedEventHandler(this.DisplayObjectModel);
            ListBoxWrapClass.ItemsChangeToForm += new ItemsChangeEventHandler(listbox_AddItems);
            BindProperty();
            /////////////////////////////////////////////////////////////////////////////////////////
        }
        private void BindProperty()
        {
            try
            {
                ContourExtractParam param = ((ContourExtract)this._function).Param;
                this.显示条目comboBox.DataSource = Enum.GetNames(typeof(enShowItemTrackExtract));
                this.变换类型comboBox.DataSource = Enum.GetNames(typeof(enTransformationType));
                this.曲线点间隔textBox.DataBindings.Add(nameof(this.曲线点间隔textBox.Text), param, nameof(param.InterStep), true, DataSourceUpdateMode.OnPropertyChanged);
                this.启用轨迹去重checkBox.DataBindings.Add(nameof(this.启用轨迹去重checkBox.Checked), param, nameof(param.RemoveRepetitivePoint), true, DataSourceUpdateMode.OnPropertyChanged);
                this.点合并距离textBox.DataBindings.Add(nameof(this.点合并距离textBox.Text), param, nameof(param.UnionStep), true, DataSourceUpdateMode.OnPropertyChanged);
                ///////////////////////////////
                this.启用轨迹自检checkBox.DataBindings.Add(nameof(this.启用轨迹自检checkBox.Checked), param, nameof(param.EnableSelfCheck), true, DataSourceUpdateMode.OnPropertyChanged);
                this.变换类型comboBox.DataBindings.Add(nameof(this.变换类型comboBox.Text), param, nameof(param.TransformationType), true, DataSourceUpdateMode.OnPropertyChanged);
                this.起始点百分比textBox.DataBindings.Add(nameof(this.起始点百分比textBox.Text), param, nameof(param.StartPercent), true, DataSourceUpdateMode.OnPropertyChanged);
                this.结束点百分比textBox.DataBindings.Add(nameof(this.结束点百分比textBox.Text), param, nameof(param.EndPercent), true, DataSourceUpdateMode.OnPropertyChanged);
                this.异常点阈值textBox.DataBindings.Add(nameof(this.异常点阈值textBox.Text), param, nameof(param.AnomalyThreshold), true, DataSourceUpdateMode.OnPropertyChanged);
                this.异常点数量textBox.DataBindings.Add(nameof(this.异常点数量textBox.Text), param, nameof(param.AnomalyCount), true, DataSourceUpdateMode.OnPropertyChanged);
            }
            catch (Exception ex)
            {
                new UserMessageForm(ex.ToString()).ShowDialog();
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

        #region 数据视图右键菜单项
        //private void addDataGridViewContextMenu(DataGridView dataGridView)
        //{
        //    ContextMenuStrip ContextMenuStrip1 = new ContextMenuStrip();
        //    // 添加右键菜单 
        //    ToolStripItem[] items = new ToolStripMenuItem[]
        //    {
        //        new ToolStripMenuItem("复制"),
        //        new ToolStripMenuItem("阵列"),
        //        new ToolStripMenuItem("清空"),
        //        new ToolStripMenuItem("圆形阵列"),
        //    };
        //    ContextMenuStrip1.Items.AddRange(items); // 在滑赋值前不能调用 
        //    ContextMenuStrip1.ItemClicked += new ToolStripItemClickedEventHandler(dataGridViewContextMenuStrip_ItemClicked);
        //    dataGridView.ContextMenuStrip = ContextMenuStrip1;
        //}
        //private void dataGridViewContextMenuStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        //{
        //    string name = e.ClickedItem.Text;
        //    int index = 0;
        //    try
        //    {
        //        BindingList<TrackExtractParam> listShape = ((TrackExtract)this._function).Param;
        //        ((ContextMenuStrip)sender).Close();
        //        switch (name)
        //        {
        //            case "复制":
        //                ((ContextMenuStrip)sender).Close();
        //                if (this.dataGridView1.CurrentRow != null)
        //                    index = this.dataGridView1.CurrentRow.Index;
        //                CopyForm copyForm = new CopyForm();
        //                if (copyForm.ShowDialog() == DialogResult.OK)
        //                {
        //                    for (int i = 0; i < copyForm.Count; i++)
        //                    {
        //                        TrackExtractParam roi = ((TrackExtract)this._function).Param[index];
        //                        TrackExtractParam roiParam = new TrackExtractParam();
        //                        roiParam.ShapeType = roi.ShapeType;
        //                        roiParam.RoiShape = roi.RoiShape.Clone();
        //                        ((TrackExtract)this._function).Param.Add(roiParam);
        //                    }
        //                }
        //                break;
        //            ////////////////////////////////////////////////
        //            case "清空":
        //                ((ContextMenuStrip)sender).Close();
        //                ((TrackExtract)this._function).Param.Clear();
        //                break;
        //            ////////////////////////////////////////////////
        //            case "阵列":
        //                ((ContextMenuStrip)sender).Close();
        //                if (this.dataGridView1.CurrentRow != null)
        //                    index = this.dataGridView1.CurrentRow.Index;
        //                RectangleArrayDataForm rectForm = new RectangleArrayDataForm();
        //                if (rectForm.ShowDialog() == DialogResult.OK)
        //                {
        //                    for (int i = 0; i < rectForm.RowCount; i++)
        //                    {
        //                        for (int k = 0; k < rectForm.ColCount; k++)
        //                        {
        //                            TrackExtractParam roi = ((TrackExtract)this._function).Param[index];
        //                            TrackExtractParam roiParam = new TrackExtractParam();
        //                            roiParam.ShapeType = roi.ShapeType;
        //                            roiParam.RoiShape = roi.RoiShape.Clone();
        //                            ((TrackExtract)this._function).Param.Add(roiParam);
        //                        }
        //                    }
        //                }
        //                break;
        //            case "圆形阵列":

        //                break;
        //            ///////////////////////////////////////////////
        //            default:
        //                break;
        //        }
        //    }
        //    catch
        //    {
        //    }
        //}
        #endregion

        // 获取鼠标位置处的高度值
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
            this.行坐标Label.Text = $"row:{e.Row}";// e.Row.ToString();
            this.列坐标Label.Text = $"col:{e.Col}";// e.Col.ToString();
        }
        private void DisplayObjectModel(object sender, ExcuteCompletedEventArgs e)  // 这里一定要加一个键值对变量来收集，以免出现重合
        {
            try
            {
                if (e.DataContent != null)
                {
                    switch (e.DataContent.GetType().Name) //这里只接受XLD轮廓或3D对象轮廓
                    {
                        case nameof(PointCloudData):
                            this.drawObject.ClearViewObject();
                            this.drawObject.PointCloudModel3D = (PointCloudData)e.DataContent;
                            this._objectDataModel = this.drawObject.PointCloudModel3D;
                            break;
                        case nameof(HImage):
                            this.drawObject.ClearViewObject();
                            this.drawObject.BackImage = new ImageDataClass((HImage)e.DataContent);
                            //this._objectDataModel = this.drawObject.BackImage;
                            this.drawObject.AttachPropertyData.Clear();
                            break;
                        case nameof(ImageDataClass):
                            this.drawObject.ClearViewObject();
                            this.drawObject.BackImage = (ImageDataClass)e.DataContent;
                            //this._objectDataModel = this.drawObject.BackImage;
                            break;
                        case nameof(XldDataClass):
                            this.drawObject.AddViewObject(new ViewData(((XldDataClass)e.DataContent).HXldCont, "red"));
                            break;
                        case "userWcsPoint[]":
                            userWcsPoint[] wcsVectors = (userWcsPoint[])e.DataContent;
                            //this.drawObject.ClearViewObject();
                            foreach (var item in wcsVectors)
                            {
                                this.drawObject.AddViewObject(new ViewData(item.GetPixPoint().GetXLD(), "yellow")); // 默认绿色
                            }
                            break;
                        case nameof(HXLDCont):
                            this.drawObject.AddViewObject(new ViewData((HXLDCont)e.DataContent, "red"));
                            break;
                        case nameof(userWcsPolyLine):
                            //this.drawObject.ClearViewObject(); // 清空视图
                            //userWcsPolyLine wcsPolyLine = (userWcsPolyLine)e.DataContent;
                            //userPixPolyLine pixPolyLine = wcsPolyLine.GetPixPolyLine();
                            //for (int i = 0; i < pixPolyLine.Row.Count; i++)
                            //{
                            //    this.drawObject.AddViewObject(new ViewData(new HXLDCont(pixPolyLine.Row[i], pixPolyLine.Col[i]))); // 默认绿色
                            //}
                            this.Invoke(new Action(() =>  this.显示条目comboBox.SelectedIndex = 1 ));
                            
                            break;
                        case nameof(userWcsPolygon):
                            //this.drawObject.ClearViewObject(); // 清空视图
                            //userWcsPolygon wcsPolygon = (userWcsPolygon)e.DataContent;
                            //userPixPolygon pixPolygon = wcsPolygon.GetPixPolygon();
                            //for (int i = 0; i < pixPolygon.Row.Count; i++)
                            //{
                            //    this.drawObject.AddViewObject(new ViewData(new HXLDCont(pixPolygon.Row[i], pixPolygon.Col[i]))); // 默认绿色
                            //}
                            this.Invoke(new Action(() => this.显示条目comboBox.SelectedIndex = 1));
                            break;
                        case nameof(HRegion):
                            this.drawObject.AddViewObject(new ViewData(((HRegion)e.DataContent), "red"));
                            break;
                        case "RegionDataClass[]":
                            RegionDataClass[] regionDataClasses = (RegionDataClass[])e.DataContent;
                            foreach (var item in regionDataClasses)
                            {
                                this.drawObject.AddViewObject(new ViewData(item, "red"));
                            }
                            break;
                        case nameof(RegionDataClass):
                            this.drawObject.AddViewObject(new ViewData(((RegionDataClass)e.DataContent).Region, "red"));
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                new UserMessageForm().ShowDialog("DisplayObjectModel->操作失败" + ex.ToString());
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
                    switch (object3D.GetType().Name)
                    {
                        case "HImage":
                            this.drawObject.ClearViewObject();
                            this.drawObject.BackImage = new ImageDataClass((HImage)object3D);
                            //this._objectDataModel = this.drawObject.BackImage;
                            this.drawObject.AttachPropertyData.Clear();
                            break;
                        case "ImageDataClass":
                            this.drawObject.ClearViewObject();
                            this.drawObject.BackImage = (ImageDataClass)object3D;
                            //this._objectDataModel = this.drawObject.BackImage;
                            break;
                        case "XldDataClass":
                            this.drawObject.AddViewObject(new ViewData(((XldDataClass)object3D).HXldCont, "red"));
                            break;
                        case "XldDataClass[]":
                            XldDataClass[] xldDataClasses = (XldDataClass[])object3D;
                            foreach (var item in xldDataClasses)
                            {
                                this.drawObject.AddViewObject(new ViewData(((XldDataClass)item).HXldCont, "red"));
                            }
                            break;
                        case "HXLDCont":
                            this.drawObject.AddViewObject(new ViewData(((HXLDCont)object3D), "red"));
                            break;
                        case "HRegion":
                            this.drawObject.AddViewObject(new ViewData(((HRegion)object3D), "red"));
                            break;
                        case "RegionDataClass[]":
                            RegionDataClass[] regionDataClasses = (RegionDataClass[])object3D;
                            foreach (var item in regionDataClasses)
                            {
                                this.drawObject.AddViewObject(new ViewData(item, "red"));
                            }
                            break;
                        case "RegionDataClass":
                            this.drawObject.AddViewObject(new ViewData(((RegionDataClass)object3D).Region, "red"));
                            break;
                    }
                }
            }
            catch
            {

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
                new UserMessageForm(ex.ToString()).ShowDialog();
            }
        }
        private void 视图工具toolStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
           // HalconLibrary ha = new HalconLibrary();
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

        private void TrackExtractForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (this.drawObject != null)
                    this.drawObject.ClearDrawingObject();
                this.drawObject?.PointCloudModel3D?.Dispose();
                // 注消事件
                this.drawObject.GrayValueInfo -= new GrayValueInfoEventHandler(GetGrayValueInfo);
                ListBoxWrapClass.ItemsChangeToForm -= new ItemsChangeEventHandler(listbox_AddItems);
                BaseFunction.ExcuteCompleted -= new ExcuteCompletedEventHandler(this.DisplayObjectModel);
            }
            catch
            {

            }
        }

        public enum enShowItemTrackExtract
        {
            输入轨迹,
            提取轨迹,
            注册轨迹,
            自检轨迹,
        }
        private void 显示条目comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.显示条目comboBox.SelectedIndex == -1) return;
                switch (this.显示条目comboBox.Text.Trim())
                {
                    case nameof(enShowItemTrackExtract.输入轨迹):
                        this.drawObject.PointCloudModel3D?.ClearObjectModel3d();
                        userWcsPoint[] wcsPoints = ((ContourExtract)this._function).CurTrackPoint;
                        if (wcsPoints != null)
                        {
                            double[] x = new double[wcsPoints.Length];
                            double[] y = new double[wcsPoints.Length];
                            double[] z = new double[wcsPoints.Length];
                            for (int i = 0; i < wcsPoints.Length; i++)
                            {
                                x[i] = wcsPoints[i].X;
                                y[i] = wcsPoints[i].Y;
                                z[i] = wcsPoints[i].Z;
                            }
                            hObjectModel3D1?.ClearObjectModel3d();
                            hObjectModel3D1 = new HObjectModel3D(x, y, z);
                            this.drawObject.PointCloudModel3D = new PointCloudData(hObjectModel3D1);
                        }
                        break;
                    case nameof(enShowItemTrackExtract.注册轨迹):
                        this.drawObject.PointCloudModel3D?.ClearObjectModel3d();
                        wcsPoints = ((ContourExtract)this._function).StdTrackPoint;
                        if (wcsPoints != null)
                        {
                            double[] x = new double[wcsPoints.Length];
                            double[] y = new double[wcsPoints.Length];
                            double[] z = new double[wcsPoints.Length];
                            for (int i = 0; i < wcsPoints.Length; i++)
                            {
                                x[i] = wcsPoints[i].X;
                                y[i] = wcsPoints[i].Y;
                                z[i] = wcsPoints[i].Z;
                            }
                            hObjectModel3D2?.ClearObjectModel3d();
                            hObjectModel3D2 = new HObjectModel3D(x, y, z);
                            this.drawObject.PointCloudModel3D = new PointCloudData(hObjectModel3D2);
                        }
                        break;
                    case nameof(enShowItemTrackExtract.提取轨迹):
                        this.drawObject.PointCloudModel3D?.ClearObjectModel3d();
                        /////////////////////////////////////////////////////////////////////////
                        userWcsPolyLine wcsPolyLine = ((ContourExtract)this._function).WcsPolyLine;
                        if (wcsPolyLine != null)
                        {
                            double[] x = new double[wcsPolyLine.X.Count];
                            double[] y = new double[wcsPolyLine.X.Count];
                            double[] z = new double[wcsPolyLine.X.Count];
                            for (int i = 0; i < wcsPolyLine.X.Count; i++)
                            {
                                x[i] = wcsPolyLine.X[i];
                                y[i] = wcsPolyLine.Y[i];
                                z[i] = wcsPolyLine.Z[i];
                            }
                            hObjectModel3D2?.ClearObjectModel3d();
                            hObjectModel3D2 = new HObjectModel3D(x, y, z);
                        }
                        this.drawObject.PointCloudModel3D = new PointCloudData(new HObjectModel3D[] { hObjectModel3D2 });
                        break;
                    case nameof(enShowItemTrackExtract.自检轨迹):
                        this.drawObject.PointCloudModel3D?.ClearObjectModel3d();
                        wcsPoints = ((ContourExtract)this._function).StdTrackPoint; // 基准轨迹
                        if (wcsPoints != null)
                        {
                            double[] x = new double[wcsPoints.Length];
                            double[] y = new double[wcsPoints.Length];
                            double[] z = new double[wcsPoints.Length];
                            for (int i = 0; i < wcsPoints.Length; i++)
                            {
                                x[i] = wcsPoints[i].X;
                                y[i] = wcsPoints[i].Y;
                                z[i] = wcsPoints[i].Z;
                            }
                            hObjectModel3D1?.ClearObjectModel3d();
                            hObjectModel3D1 = new HObjectModel3D(x, y, z);
                        }
                        /////////////////////////////////////////////////////////////////////////
                        wcsPolyLine = ((ContourExtract)this._function).WcsPolyLineAffine;
                        if (wcsPolyLine != null)
                        {
                            double[] x = new double[wcsPolyLine.X.Count];
                            double[] y = new double[wcsPolyLine.X.Count];
                            double[] z = new double[wcsPolyLine.X.Count];
                            for (int i = 0; i < wcsPolyLine.X.Count; i++)
                            {
                                x[i] = wcsPolyLine.X[i];
                                y[i] = wcsPolyLine.Y[i];
                                z[i] = wcsPolyLine.Z[i];
                            }
                            hObjectModel3D2?.ClearObjectModel3d();
                            hObjectModel3D2 = new HObjectModel3D(x, y, z);
                        }
                        this.drawObject.PointCloudModel3D = new PointCloudData(new HObjectModel3D[] { hObjectModel3D1, hObjectModel3D2 });
                        ///////////////////////// 显示误差值 /////////////////////////
                        //this.dataGridView1.Rows.Clear();
                        //if (((ContourModelMatch2D)this._function).DataError != null)
                        //{
                        //    foreach (var item in ((ContourModelMatch2D)this._function).DataError)
                        //    {
                        //        int index = this.dataGridView1.Rows.Add(item);
                        //        this.dataGridView1.Rows[index].HeaderCell.Value = (index + 1).ToString();
                        //    }
                        //}
                        break;
                }
            }
            catch (Exception ex)
            {
                new UserMessageForm(ex.ToString()).ShowDialog();
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


        private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
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
                new UserMessageForm(ex.ToString()).ShowDialog();
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
        //protected override void WndProc(ref Message m)
        //{
        //    //switch (m.Msg)
        //    //{
        //    //    case 0x0084:
        //    //        base.WndProc(ref m);
        //    //        Point vPoint = new Point((int)m.LParam & 0xFFFF,
        //    //            (int)m.LParam >> 16 & 0xFFFF);
        //    //        vPoint = PointToClient(vPoint);
        //    //        if (vPoint.X <= 5)
        //    //            if (vPoint.Y <= 5)
        //    //                m.Result = (IntPtr)Guying_HTTOPLEFT;
        //    //            else if (vPoint.Y >= ClientSize.Height - 5)
        //    //                m.Result = (IntPtr)Guying_HTBOTTOMLEFT;
        //    //            else m.Result = (IntPtr)Guying_HTLEFT;
        //    //        else if (vPoint.X >= ClientSize.Width - 5)
        //    //            if (vPoint.Y <= 5)
        //    //                m.Result = (IntPtr)Guying_HTTOPRIGHT;
        //    //            else if (vPoint.Y >= ClientSize.Height - 5)
        //    //                m.Result = (IntPtr)Guying_HTBOTTOMRIGHT;
        //    //            else m.Result = (IntPtr)Guying_HTRIGHT;
        //    //        else if (vPoint.Y <= 2)
        //    //            m.Result = (IntPtr)Guying_HTTOP;
        //    //        else if (vPoint.Y >= ClientSize.Height - 5)
        //    //            m.Result = (IntPtr)Guying_HTBOTTOM;
        //    //        break;
        //    //    default:
        //    //        base.WndProc(ref m);
        //    //        break;
        //    //}
        //}
        #endregion

        #region 防止改变窗口大小时控件闪烁功能
        //protected override CreateParams CreateParams   //
        //{
        //    get
        //    {
        //        CreateParams cp = base.CreateParams;
        //        cp.ExStyle |= 0x02000000; // Turn on WS_EX_COMPOSITED
        //        return cp;
        //    }
        //}
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
            TrackExtractFormOld_MouseDown(null, null);  // 用标签鼠标按下事件来代替窗体鼠标按下事件
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

        private void TrackExtractFormOld_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, WM_SYSCOMMAND, SC_MOVE + HTCAPTION, 0);
        }

        private void 导入基准轮廓Btn_Click(object sender, EventArgs e)
        {
            try
            {
                if (new UserMessageForm().ShowDialog("确定要导入基准轮廓吗?", "导入基准轮廓") == DialogResult.Cancel) return;
                string path = new FileOperate().OpenFileDxf();
                if (path != null)
                {
                    string extendName = new FileInfo(path).Extension;
                    switch (extendName)
                    {
                        case ".txt":
                            double[] Data1, Data2, Data3;
                            new FileOperate().ReadTxt(path, out Data1, out Data2);
                            if (Data1 != null && Data1.Length > 0)
                            {
                                Data3 = new double[Data1.Length];
                                ((ContourExtract)this._function).StdTrackPoint = new userWcsPoint[Data1.Length];
                                for (int i = 0; i < Data1.Length; i++)
                                {
                                    Data3[i] = 0;
                                    ((ContourExtract)this._function).StdTrackPoint[i] = new userWcsPoint(Data1[i], Data2[i], 0);
                                }
                                this.drawObject.PointCloudModel3D?.ClearObjectModel3d();
                                this.drawObject.PointCloudModel3D = new PointCloudData(new HObjectModel3D(Data1, Data2, Data3));
                                new UserMessageForm().ShowDialog("导入基准轮廓成功!");
                            }
                            else
                                new UserMessageForm().ShowDialog("读取的点数量长度为 0");
                            break;
                        case ".dxf":
                            List<double> list_x = new List<double>();
                            List<double> list_y = new List<double>();
                            List<double> list_z = new List<double>();
                            HXLDCont hXLDCont = new HalconDotNet.HXLDCont();
                            hXLDCont.ReadContourXldDxf(path, "max_approx_error", 0.01);
                            HTuple row, col;
                            if (hXLDCont != null && hXLDCont.IsInitialized())
                            {
                                int num = hXLDCont.CountObj();
                                for (int i = 1; i <= num; i++)
                                {
                                    hXLDCont.SelectObj(i).GetContourXld(out row, out col);
                                    list_x.AddRange(row.DArr);
                                    list_y.AddRange(col.DArr);
                                    list_z.AddRange(HTuple.TupleGenConst(row.Length, 0.0).DArr);
                                }
                            }
                            ////////////////////////////////////////////////////////////////////
                            if (list_x != null && list_x.Count > 0)
                            {
                                ((ContourExtract)this._function).StdTrackPoint = new userWcsPoint[list_x.Count];
                                for (int i = 0; i < list_x.Count; i++)
                                {
                                    ((ContourExtract)this._function).StdTrackPoint[i] = new userWcsPoint(list_x[i], list_y[i], 0);
                                }
                                this.drawObject.PointCloudModel3D?.ClearObjectModel3d();
                                this.drawObject.PointCloudModel3D = new PointCloudData(new HObjectModel3D(list_x.ToArray(), list_y.ToArray(), list_z.ToArray()));
                                new UserMessageForm().ShowDialog("导入基准轮廓成功!");
                            }
                            else
                                new UserMessageForm().ShowDialog("读取的点数量长度为 0");
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                new UserMessageForm(ex.ToString()).ShowDialog();
            }
        }

        private void 注册基准轮廓Btn_Click(object sender, EventArgs e)
        {
            try
            {
                if (new UserMessageForm().ShowDialog("确定要注册基准轮廓吗?", "注册基准轮廓") == DialogResult.Cancel) return;
                userWcsPolyLine wcsPolyLine = ((ContourExtract)this._function).WcsPolyLine;
                if (wcsPolyLine != null && wcsPolyLine.X.Count > 0)
                {
                    List<double> list_x = new List<double>();
                    List<double> list_y = new List<double>();
                    List<double> list_z = new List<double>();
                    ((ContourExtract)this._function).StdTrackPoint = new userWcsPoint[wcsPolyLine.X.Count];
                    for (int i = 0; i < wcsPolyLine.X.Count; i++)
                    {
                        list_x.Add(wcsPolyLine.X[i]);
                        list_y.Add(wcsPolyLine.Y[i]);
                        list_z.Add(wcsPolyLine.Z[i]);
                        ((ContourExtract)this._function).StdTrackPoint[i] = new userWcsPoint(wcsPolyLine.X[i], wcsPolyLine.Y[i], wcsPolyLine.Z[i], wcsPolyLine.CamParams);
                    }
                    this.drawObject.PointCloudModel3D?.ClearObjectModel3d();
                    this.drawObject.PointCloudModel3D = new PointCloudData(new HObjectModel3D(list_x.ToArray(), list_y.ToArray(), list_z.ToArray()));
                    new UserMessageForm().ShowDialog("注册基准轮廓成功!");
                }
                else
                    new UserMessageForm().ShowDialog("注册基准轮廓失败!");
            }
            catch (Exception ex)
            {
                new UserMessageForm(ex.ToString()).ShowDialog();
            }
        }




    }

}

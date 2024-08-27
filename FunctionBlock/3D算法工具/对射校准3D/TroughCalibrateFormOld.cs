
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
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using View;

namespace FunctionBlock
{
    public partial class TroughCalibrateFormOld : Form
    {
        private Form form;
        private IFunction _function;
        private VisualizeView drawObject;
        private HImage sourceImage;
        private BindingList<TrackMoveParam> _trackParam = new BindingList<TrackMoveParam>();
        public TroughCalibrateFormOld(TreeNode node)
        {
            this._function = node.Tag as IFunction;
            InitializeComponent();
            this.drawObject = new VisualizeView(this.hWindowControl1, false);
        }
        private void CalibrationForm_Load(object sender, EventArgs e)
        {
            // 注册事件
            BaseFunction.ExcuteCompleted += new ExcuteCompletedEventHandler(this.DisplayObjectModel);
            BindProperty();
            this.MoveCol.Items.Clear();
            this.MoveCol.ValueType = typeof(enMoveType);
            foreach (enMoveType temp in Enum.GetValues(typeof(enMoveType)))
                this.MoveCol.Items.Add(temp);
            this.dataGridView1.DataSource = _trackParam;
        }

        private void BindProperty()
        {
            try
            {
                TroughParam param = ((TroughCalibrate)this._function).Param;
                this.传感器comboBox1.Items.Clear();
                this.传感器comboBox1.Items.AddRange(AcqSourceManage.Instance.GetAcqSourceName());
                //this.传感器comboBox1.Text = param.AcqSourceName;
                this.激光1采集源comboBox.Items.Clear();
                this.激光1采集源comboBox.Items.AddRange(AcqSourceManage.Instance.GetAcqSourceName());
                this.激光2采集源comboBox.Items.Clear();
                this.激光2采集源comboBox.Items.AddRange(AcqSourceManage.Instance.GetAcqSourceName());
                /////////////////////
                this.激光1采集源comboBox.DataBindings.Add(nameof(this.激光1采集源comboBox.Text), param, nameof(param.LaserAcqSource1), true, DataSourceUpdateMode.OnPropertyChanged);
                this.激光2采集源comboBox.DataBindings.Add(nameof(this.激光2采集源comboBox.Text), param, nameof(param.LaserAcqSource2), true, DataSourceUpdateMode.OnPropertyChanged);
                this.标准值textBox.DataBindings.Add(nameof(this.标准值textBox.Text), param, nameof(param.StdValue), true, DataSourceUpdateMode.OnPropertyChanged);
                this.标定值textBox.DataBindings.Add(nameof(this.标定值textBox.Text), param, nameof(param.CalibValue), true, DataSourceUpdateMode.OnPropertyChanged);
                this.传感器comboBox1.DataBindings.Add(nameof(this.传感器comboBox1.SelectedItem), param, nameof(param.AcqSourceName), true, DataSourceUpdateMode.OnPropertyChanged);
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
        public void DisplayObjectModel(object sender, ExcuteCompletedEventArgs e)
        {
            try
            {
                if (e.DataContent != null) // 在图形窗口只显示世界坐标元素，像素元素在图像窗口显示
                {
                    switch (e.DataContent.GetType().Name)
                    {
                        case nameof(ImageDataClass):
                            this.drawObject.ClearViewObject(); // 更新图像时清空
                            this.drawObject.BackImage = (ImageDataClass)e.DataContent;
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
                        case nameof(userOkNgText):
                            this.drawObject.AddViewObject(new ViewData(e.DataContent, "red"));
                            break;
                        default:
                            this.drawObject.AddViewObject(new ViewData(e.DataContent, "red"));
                            break;
                    }
                }
            }
            catch (Exception he)
            {

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
             new ToolStripMenuItem("设置曝光(Set)"),
             new ToolStripMenuItem("保存图像(Save)") ,
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
                    case "设置曝光":
                        if (AcqSourceManage.Instance.GetAcqSource(this.传感器comboBox1.Text) == null || AcqSourceManage.Instance.GetAcqSource(this.传感器comboBox1.Text).Sensor == null) return;
                        string value = AcqSourceManage.Instance.GetAcqSource(this.传感器comboBox1.Text).Sensor.GetParam("曝光").ToString();
                        RenameForm renameForm = new RenameForm(value);
                        renameForm.ShowDialog();
                        AcqSourceManage.Instance.GetAcqSource(this.传感器comboBox1.Text).Sensor.SetParam("曝光", renameForm.ReName);
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


        #endregion
        private void 视图工具toolStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
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
                    this.drawObject.AutoImage();
                    this.toolStripButton_Translate.CheckState = CheckState.Unchecked;

                    break;
                case "toolStripButton_3D":
                    this.toolStripButton_Translate.CheckState = CheckState.Unchecked;

                    break;
                default:
                    break;
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
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
            }
        }
        private void DeformableMatchForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // 注消事件
            try
            {
                if (this.drawObject != null)
                    this.drawObject.ClearDrawingObject();
                BaseFunction.ExcuteCompleted -= new ExcuteCompletedEventHandler(this.DisplayObjectModel);
            }
            catch
            {

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
                            switch(this.传感器comboBox1.Text)
                            {
                                case "NONE":
                                    CoordSysAxisParam sysAxisParam = new CoordSysAxisParam(enCoordSysName.CoordSys_0);
                                    switch (this._trackParam[e.RowIndex].MoveType)
                                    {
                                        case enMoveType.矩形2运动:
                                        case enMoveType.矩形1运动:
                                        case enMoveType.圆运动:
                                        case enMoveType.椭圆运动:
                                        case enMoveType.多边形运动:
                                        case enMoveType.点位运动:
                                        case enMoveType.直线运动:
                                        default:
                                            this._trackParam[e.RowIndex].RoiShape = new drawWcsPoint(sysAxisParam.X, sysAxisParam.Y, sysAxisParam.Z);
                                            break;
                                    }
                                    break;
                                default:
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
                                        this.drawObject.SetParam(this._trackParam[e.RowIndex].RoiShape);
                                    }
                                    this.drawObject.DrawPixRoiShapeOnWindow(enColor.red, out pixShape);
                                    this.drawObject.AddViewObject(new ViewData(pixShape.GetXLD(), enColor.orange.ToString()));
                                    /////////////////////////////////////////////////////////
                                    this._trackParam[e.RowIndex].RoiShape = pixShape.GetWcsROI(this.drawObject.CameraParam);
                                    break;
                            }
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
                                    this.drawObject.AddViewObject(new ViewData(item.RoiShape.GetPixROI(this.drawObject.CameraParam).GetXLD(), enColor.green.ToString()));
                                }
                                else
                                {
                                    this.drawObject.AddViewObject(new ViewData(item.RoiShape.GetPixROI(this.drawObject.CameraParam).GetXLD(), enColor.orange.ToString()));
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

        private void 图像采集Btn_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.传感器comboBox1.SelectedItem == null) return;
                AcqSource acqSource = AcqSourceManage.Instance.GetAcqSource(this.传感器comboBox1.Text);
                if (acqSource == null) return;
                switch (acqSource.Sensor.ConfigParam.SensorType)
                {
                    case enUserSensorType.面阵相机:
                        Dictionary<enDataItem, object> data = acqSource.AcqImageData(null);
                        this.drawObject.BackImage = (ImageDataClass)data[enDataItem.Image];
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }




    }
}


using FunctionBlock;
using HalconDotNet;
using Sensor;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.IO;
using Common;
using MotionControlCard;
using AlgorithmsLibrary;
using View;

namespace FunctionBlock
{

    public partial class MachineCalibForm : Form
    {
        private AcqSource _acqSource;
        private CancellationTokenSource cts;
        //private CameraCalibrateTool calibrateCamera;
        private ImageDataClass image = null;
        private double error;
        private DrawingBaseMeasure drawObject;
        private TreeViewWrapClass _treeViewWrapClass;
        private CameraParam CamParam;
        private MetrolegyParamForm metrolegyParamForm;
        private IFunction _currFunction;
        private Dictionary<string, object> listData = new Dictionary<string, object>();
        private Dictionary<string, userPixPoint> listPixPoint = new Dictionary<string, userPixPoint>();
        private BindingList<CoordSysAxisParam> listWcsPoint;
        private bool isStop = false;
        private string programPath = "标定程序\\N点标定程序"; // 程序文件路径
        public MachineCalibForm()
        {
            InitializeComponent();
            this.drawObject = new DrawingBaseMeasure(this.hWindowControl1);
            this._treeViewWrapClass = new TreeViewWrapClass(this.treeView1, this);
            this.metrolegyParamForm = new MetrolegyParamForm(this.drawObject);
        }
        public MachineCalibForm(CameraParam CamParam)
        {
            this.CamParam = CamParam;
            InitializeComponent();
            this.drawObject = new DrawingBaseMeasure(this.hWindowControl1);
            this._treeViewWrapClass = new TreeViewWrapClass(this.treeView1, this);
            this.metrolegyParamForm = new MetrolegyParamForm(this.drawObject);

            // 自动打开程序
            this.programPath += "\\" + CamParam.SensorName;
            this._treeViewWrapClass.OpenProgram(this.programPath);
        }
        private void MachineCalibForm_Load(object sender, EventArgs e)
        {
            this.drawObject.GrayValueInfo += new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
            BaseFunction.ExcuteCompleted += new ExcuteCompletedEventHandler(DisplayExcuteResult);
            TreeViewWrapClass.ClickNode += new ClickNodeEventHandler(this.DisplayClickObject);
            BindProperty();
            this.PaintCoordSys(this.panel1);
        }

        private void BindProperty()
        {
            try
            {
                this.坐标值类型comboBox.DataSource = Enum.GetValues(typeof(enCoordValueType)); // 这里不需要更改本标定坐标系，只需要显示即可
                this.坐标值类型comboBox.DataBindings.Add(nameof(this.坐标值类型comboBox.Text), this.CamParam.CaliParam, nameof(this.CamParam.CaliParam.CoordValueType), true, DataSourceUpdateMode.OnPropertyChanged);

            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
            }
        }


        private void MachineCalibForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.drawObject.GrayValueInfo -= new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
            BaseFunction.ExcuteCompleted -= new ExcuteCompletedEventHandler(DisplayExcuteResult);
            TreeViewWrapClass.ClickNode -= new ClickNodeEventHandler(this.DisplayClickObject);
            this._treeViewWrapClass?.Uinit();
        }


        private void AddForm(TabPage MastPanel, Form form)
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
        private void AddForm(GroupBox MastPanel, Form form)
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


        #region 视图交互
        private void hWindowControl1_MouseMove(object sender, GrayValueInfoEventArgs e)
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
        public void DisplayExcuteResult(object sender, ExcuteCompletedEventArgs e)  // 这里一定要加一个键值对变量来收集，以免出现重合
        {
            try
            {
                if (e.DataContent == null) return;// 在图形窗口只显示世界坐标元素，像素元素在图像窗口显示
                switch (e.DataContent.GetType().Name) //这里只接受XLD轮廓或3D对象轮廓
                {
                    case "ImageDataClass":
                        this.drawObject.BackImage = (ImageDataClass)e.DataContent;
                        break;

                    case "userWcsCircle":
                        this.drawObject.AttachPropertyData.Clear();
                        if (this.listData.ContainsKey(e.ItemName))
                            this.listData[e.ItemName] = e.DataContent;
                        else
                            this.listData.Add(e.ItemName, e.DataContent);
                        this.drawObject.AttachPropertyData.AddRange(this.listData.Values.ToArray()); //(userWcsCircle)e.DataContent
                        userWcsCircle wcsCircle = ((userWcsCircle)e.DataContent);
                        for (int i = 0; i < wcsCircle.EdgesPoint_xyz.Length; i++)
                        {
                            this.drawObject.AttachPropertyData.Add(new userWcsPoint(wcsCircle.EdgesPoint_xyz[i].X, wcsCircle.EdgesPoint_xyz[i].Y, 0, wcsCircle.CamParams));
                        }
                        if (!this.listPixPoint.ContainsKey(e.ItemName))
                            this.listPixPoint.Add(e.ItemName, (new userWcsPoint(wcsCircle.X, wcsCircle.Y, wcsCircle.Z, wcsCircle.Grab_x, wcsCircle.Grab_y, wcsCircle.CamParams)).GetPixPoint());
                        else
                            this.listPixPoint[e.ItemName] = (new userWcsPoint(wcsCircle.X, wcsCircle.Y, wcsCircle.Z, wcsCircle.Grab_x, wcsCircle.Grab_y, wcsCircle.CamParams)).GetPixPoint();
                        this.drawObject.DetachDrawingObjectFromWindow();
                        break;
                    case "userWcsCircleSector":
                        this.drawObject.AttachPropertyData.Clear();
                        if (this.listData.ContainsKey(e.ItemName))
                            this.listData[e.ItemName] = e.DataContent;
                        else
                            this.listData.Add(e.ItemName, e.DataContent);
                        this.drawObject.AttachPropertyData.AddRange(this.listData.Values.ToArray());
                        userWcsCircleSector wcsCircleSector = ((userWcsCircleSector)e.DataContent);
                        for (int i = 0; i < wcsCircleSector.EdgesPoint_xyz.Length; i++)
                        {
                            this.drawObject.AttachPropertyData.Add(new userWcsPoint(wcsCircleSector.EdgesPoint_xyz[i].X, wcsCircleSector.EdgesPoint_xyz[i].Y, 0, wcsCircleSector.CamParams));
                        }
                        this.drawObject.DetachDrawingObjectFromWindow();
                        break;
                    case "userWcsEllipse":
                        this.drawObject.AttachPropertyData.Clear();
                        if (this.listData.ContainsKey(e.ItemName))
                            this.listData[e.ItemName] = e.DataContent;
                        else
                            this.listData.Add(e.ItemName, e.DataContent);
                        this.drawObject.AttachPropertyData.AddRange(this.listData.Values.ToArray());
                        userWcsEllipse wcsEllips1 = ((userWcsEllipse)e.DataContent);
                        for (int i = 0; i < wcsEllips1.EdgesPoint_xyz.Length; i++)
                        {
                            this.drawObject.AttachPropertyData.Add(new userWcsPoint(wcsEllips1.EdgesPoint_xyz[i].X, wcsEllips1.EdgesPoint_xyz[i].Y, 0, wcsEllips1.CamParams));
                        }
                        this.drawObject.DetachDrawingObjectFromWindow();
                        break;
                    case "userWcsEllipseSector":
                        this.drawObject.AttachPropertyData.Clear();
                        if (this.listData.ContainsKey(e.ItemName))
                            this.listData[e.ItemName] = e.DataContent;
                        else
                            this.listData.Add(e.ItemName, e.DataContent);
                        this.drawObject.AttachPropertyData.AddRange(this.listData.Values.ToArray());
                        userWcsEllipseSector wcsEllipseSector = ((userWcsEllipseSector)e.DataContent);
                        for (int i = 0; i < wcsEllipseSector.EdgesPoint_xyz.Length; i++)
                        {
                            this.drawObject.AttachPropertyData.Add(new userWcsPoint(wcsEllipseSector.EdgesPoint_xyz[i].X, wcsEllipseSector.EdgesPoint_xyz[i].Y, 0, wcsEllipseSector.CamParams));
                        }
                        this.drawObject.DetachDrawingObjectFromWindow();
                        break;
                    case "userWcsLine":
                        this.drawObject.AttachPropertyData.Clear();
                        if (this.listData.ContainsKey(e.ItemName))
                            this.listData[e.ItemName] = e.DataContent;
                        else
                            this.listData.Add(e.ItemName, e.DataContent);
                        this.drawObject.AttachPropertyData.AddRange(this.listData.Values.ToArray());
                        userWcsLine wcsLine = ((userWcsLine)e.DataContent);
                        for (int i = 0; i < wcsLine.EdgesPoint_xyz.Length; i++)
                        {
                            this.drawObject.AttachPropertyData.Add(new userWcsPoint(wcsLine.EdgesPoint_xyz[i].X, wcsLine.EdgesPoint_xyz[i].Y, 0, wcsLine.CamParams));
                        }
                        this.drawObject.DetachDrawingObjectFromWindow();
                        break;

                    case "userWcsPoint":
                        this.drawObject.AttachPropertyData.Clear();
                        if (this.listData.ContainsKey(e.ItemName))
                            this.listData[e.ItemName] = e.DataContent;
                        else
                            this.listData.Add(e.ItemName, e.DataContent);
                        this.drawObject.AttachPropertyData.AddRange(this.listData.Values.ToArray()); // 点对象本身就是一个点，所以这里不再考虑显示子元素
                        this.drawObject.DetachDrawingObjectFromWindow();
                        if (!this.listPixPoint.ContainsKey(e.ItemName))
                            this.listPixPoint.Add(e.ItemName, ((userWcsPoint)e.DataContent).GetPixPoint()); // 添加Mark点
                        else
                            this.listPixPoint[e.ItemName] = ((userWcsPoint)e.DataContent).GetPixPoint(); // 添加Mark点
                        break;
                    case "userWcsRectangle1":
                        this.drawObject.AttachPropertyData.Clear();
                        if (this.listData.ContainsKey(e.ItemName))
                            this.listData[e.ItemName] = e.DataContent;
                        else
                            this.listData.Add(e.ItemName, e.DataContent);
                        this.drawObject.AttachPropertyData.AddRange(this.listData.Values.ToArray());
                        userWcsRectangle1 wcsRect1 = ((userWcsRectangle1)e.DataContent);
                        for (int i = 0; i < wcsRect1.EdgesPoint_xyz.Length; i++)
                        {
                            this.drawObject.AttachPropertyData.Add(new userWcsPoint(wcsRect1.EdgesPoint_xyz[i].X, wcsRect1.EdgesPoint_xyz[i].Y, 0, wcsRect1.CamParams));
                        }
                        this.drawObject.DetachDrawingObjectFromWindow();
                        break;
                    case "userWcsRectangle2":
                        this.drawObject.AttachPropertyData.Clear();
                        if (this.listData.ContainsKey(e.ItemName))
                            this.listData[e.ItemName] = e.DataContent;
                        else
                            this.listData.Add(e.ItemName, e.DataContent);
                        this.drawObject.AttachPropertyData.AddRange(this.listData.Values.ToArray());
                        userWcsRectangle2 wcsRect2 = ((userWcsRectangle2)e.DataContent);
                        for (int i = 0; i < wcsRect2.EdgesPoint_xyz.Length; i++)
                        {
                            this.drawObject.AttachPropertyData.Add(new userWcsPoint(wcsRect2.EdgesPoint_xyz[i].X, wcsRect2.EdgesPoint_xyz[i].Y, 0, wcsRect2.CamParams));
                        }
                        this.drawObject.DetachDrawingObjectFromWindow();
                        break;

                    case "userWcsCoordSystem":
                        this.drawObject.AttachPropertyData.Clear();
                        if (this.listData.ContainsKey(e.ItemName))
                            this.listData[e.ItemName] = e.DataContent;
                        else
                            this.listData.Add(e.ItemName, e.DataContent);
                        this.drawObject.AttachPropertyData.AddRange(this.listData.Values.ToArray());
                        this.drawObject.DetachDrawingObjectFromWindow();
                        break;

                    case "userOkNgText":
                        this.drawObject.AttachPropertyData.Clear();

                        if (this.listData.ContainsKey(e.ItemName))
                            this.listData[e.ItemName] = e.DataContent;
                        else
                            this.listData.Add(e.ItemName, e.DataContent);
                        this.drawObject.AttachPropertyData.AddRange(this.listData.Values.ToArray());
                        this.drawObject.DetachDrawingObjectFromWindow();
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        public void DisplayClickObject(object sender, TreeNodeMouseClickEventArgs e)  //
        {
            if (e.Node.Tag == null) return;
            if (e.Button != MouseButtons.Left) return; // 点击右键时不变
            try
            {
                switch (e.Node.Tag.GetType().Name)
                {
                    case "CircleMeasure":
                        if (!(this.drawObject is userDrawCircleMeasure))
                        {
                            this.drawObject.GrayValueInfo -= new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                            this.drawObject.ClearDrawingObject();
                            this.drawObject = new userDrawCircleMeasure(this.hWindowControl1, ((CircleMeasure)e.Node.Tag).FindCircle.CircleWcsPosition);
                            this.drawObject.GrayValueInfo += new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                        }
                        this.drawObject.SetParam(((CircleMeasure)e.Node.Tag).FindCircle.CircleWcsPosition);
                        this.drawObject.BackImage = ((CircleMeasure)e.Node.Tag).ImageData;
                        this.drawObject.AttachDrawingObjectToWindow();
                        this.metrolegyParamForm.drawObject = this.drawObject;
                        this._currFunction = (CircleMeasure)e.Node.Tag;
                        DisplayClickItem(sender, e);
                        break;
                    case "CircleSectorMeasure":
                        if (!(this.drawObject is userDrawCircleSectorMeasure))
                        {
                            this.drawObject.GrayValueInfo -= new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                            this.drawObject.ClearDrawingObject();
                            this.drawObject = new userDrawCircleSectorMeasure(this.hWindowControl1, ((CircleSectorMeasure)e.Node.Tag).FindCircleSector.CircleSectorWcsPosition);
                            this.drawObject.GrayValueInfo += new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                        }
                        this.drawObject.SetParam(((CircleSectorMeasure)e.Node.Tag).FindCircleSector.CircleSectorWcsPosition);
                        this.drawObject.BackImage = ((CircleSectorMeasure)e.Node.Tag).ImageData;
                        this.drawObject.AttachDrawingObjectToWindow();
                        this.metrolegyParamForm.drawObject = this.drawObject;
                        this._currFunction = (CircleSectorMeasure)e.Node.Tag;
                        DisplayClickItem(sender, e);
                        break;
                    case "EllipseMeasure":
                        if (!(this.drawObject is userDrawEllipseMeasure))
                        {
                            this.drawObject.GrayValueInfo -= new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                            this.drawObject.ClearDrawingObject();
                            this.drawObject = new userDrawEllipseMeasure(this.hWindowControl1, ((EllipseMeasure)e.Node.Tag).FindEllipse.EllipseWcsPosition);
                            this.drawObject.GrayValueInfo += new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                        }
                        this.drawObject.SetParam(((EllipseMeasure)e.Node.Tag).FindEllipse.EllipseWcsPosition);
                        this.drawObject.BackImage = ((EllipseMeasure)e.Node.Tag).ImageData;
                        this.drawObject.AttachDrawingObjectToWindow();
                        this.metrolegyParamForm.drawObject = this.drawObject;
                        this._currFunction = (EllipseMeasure)e.Node.Tag;
                        DisplayClickItem(sender, e);
                        break;
                    case "EllipseSectorMeasure":
                        if (!(this.drawObject is userDrawEllipseSectorMeasure))
                        {
                            this.drawObject.GrayValueInfo -= new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                            this.drawObject.ClearDrawingObject();
                            this.drawObject = new userDrawEllipseSectorMeasure(this.hWindowControl1, ((EllipseSectorMeasure)e.Node.Tag).FindEllipseSector.EllipseSectorWcsPosition);
                            this.drawObject.GrayValueInfo += new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                        }
                        this.drawObject.SetParam(((EllipseSectorMeasure)e.Node.Tag).FindEllipseSector.EllipseSectorWcsPosition);
                        this.drawObject.BackImage = ((EllipseSectorMeasure)e.Node.Tag).ImageData;
                        this.drawObject.AttachDrawingObjectToWindow();
                        this.metrolegyParamForm.drawObject = this.drawObject;
                        this._currFunction = (EllipseSectorMeasure)e.Node.Tag;
                        DisplayClickItem(sender, e);
                        break;
                    case "LineMeasure":
                        if (!(this.drawObject is userDrawLineMeasure))
                        {
                            this.drawObject.GrayValueInfo -= new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                            this.drawObject.ClearDrawingObject();
                            this.drawObject = new userDrawLineMeasure(this.hWindowControl1, ((LineMeasure)e.Node.Tag).FindLine.LineWcsPosition);
                            this.drawObject.GrayValueInfo += new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                        }
                        this.drawObject.SetParam(((LineMeasure)e.Node.Tag).FindLine.LineWcsPosition);
                        this.drawObject.BackImage = ((LineMeasure)e.Node.Tag).ImageData;
                        this.drawObject.AttachDrawingObjectToWindow();
                        this.metrolegyParamForm.drawObject = this.drawObject;
                        this._currFunction = (LineMeasure)e.Node.Tag;
                        DisplayClickItem(sender, e);
                        break;
                    case "PointMeasure":
                        if (!(this.drawObject is userDrawPointMeasure))
                        {
                            this.drawObject.GrayValueInfo -= new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                            this.drawObject.ClearDrawingObject();
                            this.drawObject = new userDrawPointMeasure(this.hWindowControl1, ((PointMeasure)e.Node.Tag).FindPoint.LineWcsPosition);
                            this.drawObject.GrayValueInfo += new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                        }
                        this.drawObject.SetParam(((PointMeasure)e.Node.Tag).FindPoint.LineWcsPosition);
                        this.drawObject.BackImage = ((PointMeasure)e.Node.Tag).ImageData;
                        this.drawObject.AttachDrawingObjectToWindow();
                        this.metrolegyParamForm.drawObject = this.drawObject;
                        this._currFunction = (PointMeasure)e.Node.Tag;
                        DisplayClickItem(sender, e);
                        break;
                    case "Rectangle2Measure":
                        if (!(this.drawObject is userDrawRect2Measure))
                        {
                            this.drawObject.GrayValueInfo -= new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                            this.drawObject.ClearDrawingObject();
                            this.drawObject = new userDrawRect2Measure(this.hWindowControl1, ((Rectangle2Measure)e.Node.Tag).FindRect2.Rect2WcsPosition);
                            this.drawObject.GrayValueInfo += new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                        }
                        this.drawObject.SetParam(((Rectangle2Measure)e.Node.Tag).FindRect2.Rect2WcsPosition);
                        this.drawObject.BackImage = ((Rectangle2Measure)e.Node.Tag).ImageData;
                        this.drawObject.AttachDrawingObjectToWindow();
                        this.metrolegyParamForm.drawObject = this.drawObject;
                        this._currFunction = (Rectangle2Measure)e.Node.Tag;
                        DisplayClickItem(sender, e);
                        break;

                    ///////////////////////////////////////// 显示测量距离对象
                    case "CircleToCircleDist2D":
                    case "CircleToLineDist2D":
                    case "LineToLineDist2D":
                    case "PointToLineDist2D":
                        DisplayClickItem(sender, e);
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


        public void DisplayClickItem(object sender, TreeNodeMouseClickEventArgs e)  // 这里一定要加一个键值对变量来收集，以免出现重合
        {
            try
            {
                if (e.Node.Tag == null) return;
                if (e.Node.Name == null) return;// 在图形窗口只显示世界坐标元素，像素元素在图像窗口显示
                this.drawObject.AttachPropertyData.Clear();
                switch (e.Node.Tag.GetType().Name)
                {
                    case "CircleMeasure":
                    case "CircleSectorMeasure":
                    case "EllipseMeasure":
                    case "EllipseSectorMeasure":
                    case "LineMeasure":
                    case "PointMeasure":
                    case "Rectangle2Measure":
                    case "WidthMeasure":
                        this.drawObject.AttachPropertyData.Clear(); // 清空附加属性
                        this.drawObject.IsDispalyAttachEdgesProperty = true;
                        // 添加需要显示的元素
                        foreach (var items in this.listData.Keys)
                        {
                            if (items.Split('-')[0] == e.Node.Name) continue; // + "-" + "0"
                            this.drawObject.AttachPropertyData.Add(this.listData[items]);
                        }
                        this.drawObject.DrawingGraphicObject();
                        break;
                }
            }
            catch (Exception he)
            {

            }
        }
        #endregion
        private void 保存相机内参button_Click(object sender, EventArgs e)
        {
            try
            {
                this.CamParam.Save();
                MessageBox.Show("保存成功");
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
            }
        }





        private void 标定button_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.listPixPoint.Count < 4)
                    MessageBox.Show("不能少于4点");
                this.CamParam.HomMat2D = CalibrateMethod.Instance.NpointCalib(listPixPoint, listWcsPoint, out this.error);
                this.CamParam.HomMat2D = this.CamParam.HomMat2D.Clone(); // 两个都赋值
                if (error <= Math.Abs(this.CamParam.HomMat2D.c00))
                    this.标定状态textBox.Text = "标定结果良好";
                else
                    this.标定状态textBox.Text = "标定结果误差较大";
                this.平均误差textBox.Text = error.ToString();
                MessageBox.Show("标定成功:" + this.CamParam.HomMat2D.ToString() + "最大误 = " + error.ToString());
                /// 写到表格中
                this.dataGridView1.Rows.Clear();
                this.dataGridView1.Rows.Add(this.CamParam.HomMat2D.c00, this.CamParam.HomMat2D.c01, this.CamParam.HomMat2D.c02);
                this.dataGridView1.Rows.Add(this.CamParam.HomMat2D.c10, this.CamParam.HomMat2D.c11, this.CamParam.HomMat2D.c12);
                this.dataGridView1.Rows.Add(0, 0, 1);
                /////////////////////////////验证坐标
                HTuple Qx, Qy;
                GetWcsPoint(this.listPixPoint, this.CamParam, out Qx, out Qy);
                this.变换点dataGridView.Rows.Clear();
                this.变换点dataGridView.Columns.Clear();
                int rowCount = (int)this.行数numericUpDown.Value;
                int colCount = (int)this.列数numericUpDown.Value;
                for (int i = 0; i < colCount; i++)
                {
                    this.变换点dataGridView.Columns.Add("Col" + i.ToString(), (i + 1).ToString());
                }
                /////////////////////////////////
                for (int i = 0; i < rowCount; i++)
                {
                    for (int j = 0; j < colCount; j++)
                    {
                        this.变换点dataGridView.Rows.Add(Qx[i * colCount + j].D, Qy[i * colCount + j].D);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        public void GetWcsPoint(Dictionary<string, userPixPoint> pixPoint, CameraParam camParam, out HTuple Qx, out HTuple Qy)
        {
            Qx = new HTuple();
            Qy = new HTuple();
            if (pixPoint == null || pixPoint.Count == 0) return;
            if (camParam == null) return;
            double wcs_x, wcs_y, wcs_z;
            foreach (KeyValuePair<string, userPixPoint> item in pixPoint)
            {
                camParam.ImagePointsToWorldPlane(item.Value.Row, item.Value.Col, this.CamParam.CaliParam.StartCaliPoint.X, this.CamParam.CaliParam.StartCaliPoint.Y, this.CamParam.CaliParam.StartCaliPoint.Z,
                    out wcs_x, out wcs_y, out wcs_z);
                Qx.Append(wcs_x);
                Qy.Append(wcs_y);
            }
        }

        public void PaintCoordSys(Panel panel)
        {
            Graphics g = panel.CreateGraphics();

            g.DrawLine(new Pen(System.Drawing.Color.Red), 50, panel.Height - 20, 300, panel.Height - 20);
            g.DrawLine(new Pen(System.Drawing.Color.Red), 280, 210, 300, 220);
            g.DrawLine(new Pen(System.Drawing.Color.Red), 280, 230, 300, 220);

            g.DrawLine(new Pen(System.Drawing.Color.Red), 70, 240, 70, 50);

            g.DrawLine(new Pen(System.Drawing.Color.Red), 60, 70, 70, 50);
            g.DrawLine(new Pen(System.Drawing.Color.Red), 80, 70, 70, 50);
            g.DrawArc(new Pen(System.Drawing.Color.Red), 20, 170, 100, 100, 0, -90);

            g.DrawLine(new Pen(System.Drawing.Color.Red), 70 + 35, 220 - 35, 70 + 35, 220 - 35 + 10);
            g.DrawLine(new Pen(System.Drawing.Color.Red), 70 + 35, 220 - 35, 70 + 35 + 10, 220 - 35);
            g.DrawString("X轴", this.Font, new SolidBrush(Color.Black), 150, 230);
            g.DrawString("Y轴", this.Font, new SolidBrush(Color.Black), 40, 130);

            g.DrawString("说明：坐标系为笛卡尔坐标系，X向右为正，Y向上为正，角度逆时针为正。",
                this.Font, new SolidBrush(Color.Red), 60, 260);
        }
        private UserHomMat2D NpointCalib(BindingList<userPixPoint> pixPoint, BindingList<userWcsPoint> wcsPoint, out double error)
        {
            if (pixPoint == null)
            {
                throw new ArgumentNullException("pixPoint");
            }
            if (wcsPoint == null)
            {
                throw new ArgumentNullException("wcsPoint");
            }
            if (wcsPoint.Count != pixPoint.Count)
            {
                throw new ArgumentException("pixPoint 与 wcsPoint长度不相等");
            }
            HHomMat2D hHomMat2D = new HHomMat2D();
            HTuple Rows = new HTuple();
            HTuple Columns = new HTuple();
            HTuple X = new HTuple();
            HTuple Y = new HTuple();
            foreach (var item in pixPoint)
            {
                Rows.Append(item.Row);
                Columns.Append(item.Col);
            }
            /////////////////////////
            foreach (var item in wcsPoint)
            {
                X.Append(item.X);
                Y.Append(item.Y);
            }
            hHomMat2D.VectorToHomMat2d(Columns, Rows, X, Y);
            // 计算最大误
            HTuple Qx, Qy;
            Qx = hHomMat2D.AffineTransPoint2d(Columns, Rows, out Qy);
            HTuple dist = HMisc.DistancePp(Qx, Qy, X, Y);
            error = dist.TupleMax();
            return new UserHomMat2D(hHomMat2D);
        }

        private void Read(out double X, out double Y, out double Z, out double Theta)
        {
            X = 0; Y = 0; Z = 0; Theta = 0;
            MotionCardManage.GetCard(this.CamParam.CaliParam.CoordSysName).GetAxisPosition(this.CamParam.CaliParam.CoordSysName, enAxisName.X轴, out X);
            MotionCardManage.GetCard(this.CamParam.CaliParam.CoordSysName).GetAxisPosition(this.CamParam.CaliParam.CoordSysName, enAxisName.Y轴, out Y);
            MotionCardManage.GetCard(this.CamParam.CaliParam.CoordSysName).GetAxisPosition(this.CamParam.CaliParam.CoordSysName, enAxisName.Z轴, out Z);
            MotionCardManage.GetCard(this.CamParam.CaliParam.CoordSysName).GetAxisPosition(this.CamParam.CaliParam.CoordSysName, enAxisName.Theta轴, out Theta);
        }
        private void Write(double X, double Y, double Z, double Theta)
        {
            MotionCardManage.GetCard(this.CamParam.CaliParam.CoordSysName).MoveMultyAxis(this.CamParam.CaliParam.CoordSysName, enAxisName.XYZTheta轴, 0, new CoordSysAxisParam(X, Y, Z, Theta, 0, 0));
        }

        private void 获取起始点button_Click(object sender, EventArgs e)
        {
            try
            {
                double X = 0, Y = 0, Z = 0, Theta = 0;
                Read(out X, out Y, out Z, out Theta);
                /////////////////////
                this.CamParam.CaliParam.StartCaliPoint = new userWcsVector(X, Y, Z, Theta);
                this.起始点textBox.Text = "X:" + X.ToString("f3") + "   Y:" + Y.ToString("f3") + "   Theta:" + Theta.ToString("f3");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void 获取终止点button_Click(object sender, EventArgs e)
        {
            try
            {
                double X = 0, Y = 0, Z = 0, Theta = 0;
                Read(out X, out Y, out Z, out Theta);
                ///////////////////////
                this.CamParam.CaliParam.EndCalibPoint = new userWcsVector(X, Y, Z, Theta);
                this.终止点textBox.Text = "X:" + X.ToString("f3") + "   Y:" + Y.ToString("f3") + "   Theta:" + Theta.ToString("f3");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void 保存参数button_Click(object sender, EventArgs e)
        {
            try
            {
                this.CamParam.Save();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void 标定程序button_Click(object sender, EventArgs e)
        {
            this.isStop = false;
            this.listPixPoint.Clear();
            this.listWcsPoint.Clear();
            int rowCount = (int)this.行数numericUpDown.Value;
            int colCount = (int)this.列数numericUpDown.Value;
            double X, Y, Z, Theta;
            CoordSysConfigParam coordSysParam;
            try
            {
                CalibrateMethod.Instance.GetNPointCoord(this.CamParam.CaliParam.StartCaliPoint, this.CamParam.CaliParam.EndCalibPoint, rowCount, colCount, out listWcsPoint);
                foreach (var item in listWcsPoint)
                {
                    if (this.isStop) break; // 控制标定停止
                    ///////////////////////////////////////////////////////////////////////////////
                    MotionCardManage.GetCard(this.CamParam.CaliParam.CoordSysName).GetAxisPosition(this.CamParam.CaliParam.CoordSysName, enAxisName.X轴, out X);
                    MotionCardManage.GetCard(this.CamParam.CaliParam.CoordSysName).GetAxisPosition(this.CamParam.CaliParam.CoordSysName, enAxisName.Y轴, out Y);
                    MotionCardManage.GetCard(this.CamParam.CaliParam.CoordSysName).GetAxisPosition(this.CamParam.CaliParam.CoordSysName, enAxisName.Theta轴, out Theta);
                    if (this.CamParam.CaliParam.CoordValueType == enCoordValueType.绝对坐标)
                        MotionCardManage.GetCard(this.CamParam.CaliParam.CoordSysName).MoveMultyAxis(this.CamParam.CaliParam.CoordSysName,
                            enAxisName.XY轴, 10, new CoordSysAxisParam(item.X, item.Y, item.Z, item.Theta, 0, 0));
                    else
                        MotionCardManage.GetCard(this.CamParam.CaliParam.CoordSysName).MoveMultyAxis(this.CamParam.CaliParam.CoordSysName,
                                                 enAxisName.XY轴, 10, new CoordSysAxisParam(item.X - X, item.Y - Y, 0, item.Theta - Theta, 0, 0));
                    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    CommunicationConfigParamManger.Instance.WriteValue(CommunicationConfigParamManger.Instance.GetCommunicationParam(this.CamParam.CaliParam.CoordSysName,enCommunicationCommand.TriggerToPlc));
                    while (Math.Abs(item.Theta - X) > 0.02 || Math.Abs(item.Y - Y) > 0.02 || Math.Abs(item.Theta - Theta) > 0.01)
                    {
                        MotionCardManage.GetCard(this.CamParam.CaliParam.CoordSysName).GetAxisPosition(this.CamParam.CaliParam.CoordSysName, enAxisName.X轴, out X);
                        MotionCardManage.GetCard(this.CamParam.CaliParam.CoordSysName).GetAxisPosition(this.CamParam.CaliParam.CoordSysName, enAxisName.Y轴, out Y);
                        MotionCardManage.GetCard(this.CamParam.CaliParam.CoordSysName).GetAxisPosition(this.CamParam.CaliParam.CoordSysName, enAxisName.Theta轴, out Theta);
                        Application.DoEvents(); // 响应其他UI操作
                        if (this.isStop) break; // 控制标定停止
                        LoggerHelper.Info("等待运动到指定位置:" + "X = " + (item.X).ToString() + "  Y = " + (item.Y).ToString() + "  Theta = " + (item.Theta).ToString());
                    }
                    // 执行一次定位
                    // 等待PLC触发采图
                    while (true)
                    {
                        if (this.isStop) break; // 控制标定停止
                        Application.DoEvents();
                        object value = CommunicationConfigParamManger.Instance.ReadValue(CommunicationConfigParamManger.Instance.GetCommunicationParam(this.CamParam.CaliParam.CoordSysName, enCommunicationCommand.TriggerFromPlc));
                        if (value == null) break;
                        if (value.ToString() == "1")
                        {
                            CommunicationConfigParamManger.Instance.WriteValue(CommunicationConfigParamManger.Instance.GetCommunicationParam(this.CamParam.CaliParam.CoordSysName, enCommunicationCommand.TriggerFromPlc));
                            break;
                        }
                    }
                    this._treeViewWrapClass.RunSyn(null, 1);
                    CommunicationConfigParamManger.Instance.WriteValue(CommunicationConfigParamManger.Instance.GetCommunicationParam(this.CamParam.CaliParam.CoordSysName, enCommunicationCommand.Continue));
                }
                CommunicationConfigParamManger.Instance.WriteValue(CommunicationConfigParamManger.Instance.GetCommunicationParam(this.CamParam.CaliParam.CoordSysName, enCommunicationCommand.OK));
                //////////////////////////////////////////////////////////////////
                foreach (KeyValuePair<string, userPixPoint> item in this.listPixPoint)
                {
                    this.图像点dataGridView.Rows.Add(item.Value.Row, item.Value.Col);
                }
                foreach (var item in this.listWcsPoint)
                {
                    this.世界点dataGridView.Rows.Add(item.X, item.Y, item.Z, item.Theta);
                }
            }
            catch (Exception ex)
            {
                coordSysParam = CoordSysConfigParamManger.Instance.GetCoordSysConfigParam(this.CamParam.CaliParam.CoordSysName, enAxisName.NG);
                MotionCardManage.GetCard(this.CamParam.CaliParam.CoordSysName).MoveSingleAxis(this.CamParam.CaliParam.CoordSysName, enAxisName.NG, 50, coordSysParam.AxisAddress);
                MessageBox.Show(ex.ToString());
            }
            finally
            {

            }
        }




        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            switch (e.ClickedItem.Text)
            {
                case "检测工具":
                    ToolForm tool = new ToolForm(this._treeViewWrapClass);
                    tool.Owner = this;
                    tool.Show();
                    break;
                case "执行":
                    标定程序button_Click(null, null);
                    break;
                case "打开":
                    this.programPath = new FileOperate().OpenFile(2);
                    if (this.programPath == null || this.programPath.Trim().Length == 0) return;
                    this._treeViewWrapClass.OpenProgram(this.programPath);
                    break;
                case "保存":
                    if (this.programPath == null || this.programPath.Length == 0)
                    {
                        this.programPath = new FileOperate().SaveFile(2);
                        if (this.programPath == null || this.programPath.Length == 0) return;
                        if (this._treeViewWrapClass.SaveProgram(this.programPath + this.CamParam.SensorName))  //ProgramItemsSource.getInstance().GetTreeViewNode()
                            MessageBox.Show("保存成功");
                        else
                            MessageBox.Show("保存失败" + new Exception().ToString());
                    }
                    else
                    {
                        if (this._treeViewWrapClass.SaveProgram(this.programPath + this.CamParam.SensorName))
                            MessageBox.Show("保存成功");
                        else
                            MessageBox.Show("保存失败" + new Exception().ToString());
                    }
                    break;
            }
        }





    }
}

using Common;
using HalconDotNet;
using MotionControlCard;
using Sensor;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using View;

namespace FunctionBlock
{
    public partial class CaliCaliboardForm : Form
    {
        private Dictionary<string, object> listData = new Dictionary<string, object>();
        private TreeViewWrapClass _treeViewWrapClass;
        private string programPath = "标定程序\\标定板9点标定程序";
        private string _CamName = "";
        private CalibCoordConfigParamManager calibCoordConfigParamNPoint;
        private DrawingBaseMeasure drawObject;
        private MetrolegyParamForm metrolegyParamForm;
        private IFunction _currFunction;
        private CameraParam CamParam;
        private ImageDataClass CurrentImageData;
        private BindingList<userWcsPoint> listWcsPoint = new BindingList<userWcsPoint>();
        private Dictionary<string, userPixPoint> listPixPoint = new Dictionary<string, userPixPoint>();
        private Dictionary<string, userPixPoint> listWcsPointDic = new Dictionary<string, userPixPoint>();
        public CaliCaliboardForm(CameraParam CamParam)
        {
            InitializeComponent();
            this.CamParam = CamParam;
            this._treeViewWrapClass = new TreeViewWrapClass(this.treeView1, this);
            this.drawObject = new DrawingBaseMeasure(this.hWindowControl1, false);   // 一定要实例化这个窗体
            this.metrolegyParamForm = new MetrolegyParamForm(this.drawObject);
            this.addContextMenu(this.hWindowControl1);
            // 自动打开程序
            this.programPath += "\\" + CamParam.SensorName;
            this._treeViewWrapClass.OpenProgram(this.programPath);
        }
        public CaliCaliboardForm(string CamName)
        {
            InitializeComponent();
            this._CamName = CamName;
            this._treeViewWrapClass = new TreeViewWrapClass(this.treeView1, this);
            //this.calibCoordConfigParamNPoint = new CalibCoordConfigParamManager();
            //this.calibCoordConfigParamNPoint.Read(this._CamName + "T");
            this.drawObject = new DrawingBaseMeasure(this.hWindowControl1, false);
            this.metrolegyParamForm = new MetrolegyParamForm(this.drawObject);
            this.Text += "-" + CamName;
            this.addContextMenu(this.hWindowControl1);
        }

        private void CaliCaliboardForm_Load(object sender, EventArgs e)
        {
            //this.CamParam.HomMat2D = new UserHomMat2D(true);
            this.drawObject.GrayValueInfo += new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
            BaseFunction.ExcuteCompleted += new ExcuteCompletedEventHandler(DisplayExcuteResult);
            TreeViewWrapClass.ClickNode += new ClickNodeEventHandler(this.DisplayClickObject);
            BindProperty();
            //////////////////
            this.AddForm(this.元素信息tabPage, new ElementViewForm());
        }

        private void BindProperty()
        {
            try
            {
                //this.像素dataGridView.DataSource = new BindingSource()
                起始点textBox.Text = "X:" + this.CamParam.CaliParam.StartCaliPoint.X.ToString("f4") +
                                    "   Y:" + this.CamParam.CaliParam.StartCaliPoint.Y.ToString("f4") +
                                    "   Z:" + this.CamParam.CaliParam.StartCaliPoint.Z.ToString("f4") +
                                    "   Theta:" + this.CamParam.CaliParam.StartCaliPoint.Angle.ToString("f4");
                ////////////////////////////////////////////////////////////////////////
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
            }
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
                    this.listPixPoint.Clear();
                    this.像素dataGridView.Rows.Clear();
                    this.当前Mark点坐标dataGridView.Rows.Clear();
                    this._treeViewWrapClass.RunSyn(this.toolStripButton_Run, 1);
                    ///////////////////////////////////////////////////////////
                    int index = 0;
                    foreach (KeyValuePair<string, userPixPoint> item in this.listPixPoint)
                    {
                        this.像素dataGridView.Rows.Add(item.Value.Row, item.Value.Col);
                        this.像素dataGridView.Rows[index].HeaderCell.Value = (index + 1).ToString();
                        index++;
                    }
                    index = 0;
                    foreach (KeyValuePair<string, userPixPoint> item in this.listPixPoint)
                    {
                        userWcsPoint wcsPoint = item.Value.GetWcsPoint();
                        this.当前Mark点坐标dataGridView.Rows.Add(wcsPoint.X + this.CamParam.CaliParam.StartCaliPoint.X, wcsPoint.Y + this.CamParam.CaliParam.StartCaliPoint.Y);
                        this.当前Mark点坐标dataGridView.Rows[index].HeaderCell.Value = (index + 1).ToString();
                        index++;
                    }
                    break;
                case "打开":
                    this.programPath = new FileOperate().OpenFile(2);
                    if (this.programPath == null || this.programPath.Trim().Length == 0) return;
                    this._treeViewWrapClass.OpenProgram(this.programPath);
                    break;
                case "保存":
                    if (this.programPath == null || this.programPath.Length == 0)
                    {
                        if (this.programPath == null || this.programPath.Length == 0) return;
                        if (this._treeViewWrapClass.SaveProgram(this.programPath))
                            MessageBox.Show("保存成功");
                        else
                            MessageBox.Show("保存失败" + new Exception().ToString());
                    }
                    else
                    {
                        if (this._treeViewWrapClass.SaveProgram(this.programPath))
                            MessageBox.Show("保存成功");
                        else
                            MessageBox.Show("保存失败" + new Exception().ToString());
                    }
                    break;
            }
        }

        private void 标定执行button_Click_1(object sender, EventArgs e)
        {
            try
            {
                double error = 0;
                HTuple Rows = new HTuple();
                HTuple Columns = new HTuple();
                HTuple X = new HTuple();
                HTuple Y = new HTuple();
                foreach (KeyValuePair<string, userPixPoint> item in this.listPixPoint)
                {
                    Rows.Append(item.Value.Row);
                    Columns.Append(item.Value.Col);
                }
                /////////////////////////
                foreach (var item in this.listWcsPoint)
                {
                    X.Append(item.X);
                    Y.Append(item.Y);
                }
                this.CamParam.HomMat2D = CalibrateMethod.Instance.NpointCalib(Rows, Columns, X, Y, out error);
                //this.CamParam.HomMat2D = CalibrateMethod.Instance.NpointCalib(this.listPixPoint, this.listWcsPoint, out error);
                //this.CamParam.HomMat2D = this.CamParam.HomMat2D.Clone();
                this.CamParam.Save();
                this.CamParam.PixScale = this.CamParam.HomMat2D.c00;
                MessageBox.Show(this.CamParam.HomMat2D.ToString() + "最大误 = " + error.ToString());
                ///// 验证坐标
                HTuple Qx, Qy;
                GetWcsPoint(this.listPixPoint, this.CamParam, out Qx, out Qy);
                this.变换dataGridView.Rows.Clear();
                this.变换dataGridView.Columns.Clear();
                int rowCount = (int)this.行数numericUpDown.Value;
                int colCount = (int)this.列数numericUpDown.Value;
                for (int i = 0; i < colCount; i++)
                {
                    this.变换dataGridView.Columns.Add("Col" + i.ToString(), (i + 1).ToString());
                }
                /////////////////////////////////
                for (int i = 0; i < rowCount; i++)
                {
                    for (int j = 0; j < colCount; j++)
                    {
                        this.变换dataGridView.Rows.Add(Qx[i * colCount + j].D, Qy[i * colCount + j].D);
                        this.变换dataGridView.Rows[i * colCount + j].HeaderCell.Value = (i * colCount + j).ToString();
                    }
                }
                /////////////////////////////////
                this.drawObject.AddViewObject(new ViewData(new userWcsCoordSystem(this.CamParam)));
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
        private void 生成世界点button_Click(object sender, EventArgs e)
        {
            try
            {
                this.listWcsPoint.Clear();
                this.世界dataGridView.Rows.Clear();
                int rowCount = (int)this.行数numericUpDown.Value;
                int colCount = (int)this.列数numericUpDown.Value;

                //double step_x = (double).Value;
                //double step_y = (double)this.行距textBox.Value;
                //double start_x = (double)this.X起点textBox.Value;
                //double start_y = (double)this.Y起点textBox.Value;
                double step_x = 0, step_y = 0, start_x = 0, start_y = 0;
                double.TryParse(this.列距textBox.Text, out step_x);
                double.TryParse(this.行距textBox.Text, out step_y);
                double.TryParse(this.X起点textBox.Text, out start_x);
                double.TryParse(this.Y起点textBox.Text, out start_y);
                //double step_x = (this.CamParam.CaliParam.EndCalibPoint.x - this.CamParam.CaliParam.StartCaliPoint.x) / (colCount - 1);
                //double step_y = (this.CamParam.CaliParam.EndCalibPoint.y - this.CamParam.CaliParam.StartCaliPoint.y) / (rowCount - 1);
                for (int i = 0; i < rowCount; i++)
                {
                    for (int j = 0; j < colCount; j++)
                    {
                        this.listWcsPoint.Add(new userWcsPoint(
                            start_x + j * step_x,
                            start_y + i * step_y,
                            0));
                        /////////////////////////////////////////////////////// 
                        this.世界dataGridView.Rows.Add(start_x + j * step_x, start_y + i * step_y, 0);
                        this.世界dataGridView.Rows[i * colCount + j].HeaderCell.Value = (i * colCount + j + 1).ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void 获取起始点button_Click_1(object sender, EventArgs e)
        {
            try
            {
                double X = 0, Y = 0, Z = 0, Theta = 0;
                Read(out X, out Y, out Z, out Theta);
                /////////////////////
                this.CamParam.CaliParam.StartCaliPoint = new userWcsVector(X, Y, Z, Theta);
                this.CamParam.CaliParam.EndCalibPoint = new userWcsVector(X, Y, Z, Theta);
                this.CamParam.CaliParam.RotateCalibPoint = new userWcsVector(X, Y, Z, Theta);
                this.起始点textBox.Text = "X:" + X.ToString("f4") + "   Y:" + Y.ToString("f4") + "   Z:" + Z.ToString("f4") + "   Theta:" + Theta.ToString("f4");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void 起始点textBox_TextChanged(object sender, EventArgs e)
        {
            string[] name = 起始点textBox.Text.Split(new string[] { "X:", "Y:", "Z:", "Theta:" }, StringSplitOptions.RemoveEmptyEntries);
            if (name.Length < 4) return;
            double X, Y, Z, Theta;
            bool result1 = double.TryParse(name[0].Trim(), out X);
            bool result2 = double.TryParse(name[1].Trim(), out Y);
            bool result3 = double.TryParse(name[2].Trim(), out Z);
            bool result4 = double.TryParse(name[3].Trim(), out Theta);
            if (result1 && result2 && result3 && result4)
            {
                this.CamParam.CaliParam.StartCaliPoint = new userWcsVector(X, Y, Z, Theta);
                this.CamParam.CaliParam.EndCalibPoint = new userWcsVector(X, Y, Z, Theta);
            }
            else
                MessageBox.Show("数据转换报错");
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
        private void CaliCaliboardForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.drawObject.GrayValueInfo -= new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
            BaseFunction.ExcuteCompleted -= new ExcuteCompletedEventHandler(DisplayExcuteResult);
            TreeViewWrapClass.ClickNode -= new ClickNodeEventHandler(this.DisplayClickObject);
            this._treeViewWrapClass?.Uinit();
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
                        this.listData.Clear();
                        this.drawObject.BackImage = (ImageDataClass)e.DataContent;
                        this.CurrentImageData = this.drawObject.BackImage;
                        break;

                    case "userWcsCircle":
                        //this.drawObject.AttachPropertyData.Clear();
                        if (this.listData.ContainsKey(e.ItemName))
                            this.listData[e.ItemName] = e.DataContent;
                        else
                            this.listData.Add(e.ItemName, e.DataContent);
                        //this.drawObject.AttachPropertyData.AddRange(this.listData.Values.ToArray()); //(userWcsCircle)e.DataContent
                        userWcsCircle wcsCircle = ((userWcsCircle)e.DataContent);
                        if (wcsCircle.EdgesPoint_xyz != null)
                        {
                            for (int i = 0; i < wcsCircle.EdgesPoint_xyz.Length; i++)
                            {
                                this.drawObject.AttachPropertyData.Add(new userWcsPoint(wcsCircle.EdgesPoint_xyz[i].X, wcsCircle.EdgesPoint_xyz[i].Y, 0, wcsCircle.CamParams));
                            }
                        }
                        //this.drawObject.DetachDrawingObjectFromWindow();
                        /////////////////////////////////
                        if (!this.listPixPoint.ContainsKey(e.ItemName))
                            this.listPixPoint.Add(e.ItemName, new userWcsPoint(wcsCircle.X, wcsCircle.Y, wcsCircle.Z, wcsCircle.Grab_x, wcsCircle.Grab_y, wcsCircle.CamParams).GetPixPoint());
                        else
                            this.listPixPoint[e.ItemName] = new userWcsPoint(wcsCircle.X, wcsCircle.Y, wcsCircle.Z, wcsCircle.Grab_x, wcsCircle.Grab_y, wcsCircle.CamParams).GetPixPoint();
                        break;
                    case "userWcsCircleSector":
                        //this.drawObject.AttachPropertyData.Clear();
                        if (this.listData.ContainsKey(e.ItemName))
                            this.listData[e.ItemName] = e.DataContent;
                        else
                            this.listData.Add(e.ItemName, e.DataContent);
                        //this.drawObject.AttachPropertyData.AddRange(this.listData.Values.ToArray());
                        userWcsCircleSector wcsCircleSector = ((userWcsCircleSector)e.DataContent);
                        if (wcsCircleSector.EdgesPoint_xyz != null)
                        {
                            for (int i = 0; i < wcsCircleSector.EdgesPoint_xyz.Length; i++)
                            {
                                this.drawObject.AttachPropertyData.Add(new userWcsPoint(wcsCircleSector.EdgesPoint_xyz[i].X, wcsCircleSector.EdgesPoint_xyz[i].Y, 0, wcsCircleSector.CamParams));
                            }
                        }
                        //this.drawObject.DetachDrawingObjectFromWindow();
                        break;
                    case "userWcsEllipse":
                        //this.drawObject.AttachPropertyData.Clear();
                        if (this.listData.ContainsKey(e.ItemName))
                            this.listData[e.ItemName] = e.DataContent;
                        else
                            this.listData.Add(e.ItemName, e.DataContent);
                        this.drawObject.AttachPropertyData.AddRange(this.listData.Values.ToArray());
                        userWcsEllipse wcsEllips1 = ((userWcsEllipse)e.DataContent);
                        if (wcsEllips1.EdgesPoint_xyz != null)
                        {
                            for (int i = 0; i < wcsEllips1.EdgesPoint_xyz.Length; i++)
                            {
                                this.drawObject.AttachPropertyData.Add(new userWcsPoint(wcsEllips1.EdgesPoint_xyz[i].X, wcsEllips1.EdgesPoint_xyz[i].Y, 0, wcsEllips1.CamParams));
                            }
                        }
                        //this.drawObject.DetachDrawingObjectFromWindow();
                        break;
                    case "userWcsEllipseSector":
                        //this.drawObject.AttachPropertyData.Clear();
                        if (this.listData.ContainsKey(e.ItemName))
                            this.listData[e.ItemName] = e.DataContent;
                        else
                            this.listData.Add(e.ItemName, e.DataContent);
                        this.drawObject.AttachPropertyData.AddRange(this.listData.Values.ToArray());
                        userWcsEllipseSector wcsEllipseSector = ((userWcsEllipseSector)e.DataContent);
                        if (wcsEllipseSector.EdgesPoint_xyz != null)
                        {
                            for (int i = 0; i < wcsEllipseSector.EdgesPoint_xyz.Length; i++)
                            {
                                this.drawObject.AttachPropertyData.Add(new userWcsPoint(wcsEllipseSector.EdgesPoint_xyz[i].X, wcsEllipseSector.EdgesPoint_xyz[i].Y, 0, wcsEllipseSector.CamParams));
                            }
                        }
                        //this.drawObject.DetachDrawingObjectFromWindow();
                        break;
                    case "userWcsLine":
                        //this.drawObject.AttachPropertyData.Clear();
                        if (this.listData.ContainsKey(e.ItemName))
                            this.listData[e.ItemName] = e.DataContent;
                        else
                            this.listData.Add(e.ItemName, e.DataContent);
                        this.drawObject.AttachPropertyData.AddRange(this.listData.Values.ToArray());
                        userWcsLine wcsLine = ((userWcsLine)e.DataContent);
                        if(wcsLine.EdgesPoint_xyz != null)
                        {
                            for (int i = 0; i < wcsLine.EdgesPoint_xyz.Length; i++)
                            {
                                this.drawObject.AttachPropertyData.Add(new userWcsPoint(wcsLine.EdgesPoint_xyz[i].X, wcsLine.EdgesPoint_xyz[i].Y, 0, wcsLine.CamParams));
                            }
                        }
                        //this.drawObject.DetachDrawingObjectFromWindow();
                        break;

                    case "userWcsPoint":
                        //this.drawObject.AttachPropertyData.Clear();
                        if (this.listData.ContainsKey(e.ItemName))
                            this.listData[e.ItemName] = e.DataContent;
                        else
                            this.listData.Add(e.ItemName, e.DataContent);
                        this.drawObject.AttachPropertyData.AddRange(this.listData.Values.ToArray()); // 点对象本身就是一个点，所以这里不再考虑显示子元素
                        //this.drawObject.DetachDrawingObjectFromWindow();
                        /////////////////////
                        if (!this.listPixPoint.ContainsKey(e.ItemName))
                            this.listPixPoint.Add(e.ItemName, ((userWcsPoint)e.DataContent).GetPixPoint());
                        else
                            this.listPixPoint[e.ItemName] = ((userWcsPoint)e.DataContent).GetPixPoint();
                        break;
                    case "userWcsRectangle1":
                        //this.drawObject.AttachPropertyData.Clear();
                        if (this.listData.ContainsKey(e.ItemName))
                            this.listData[e.ItemName] = e.DataContent;
                        else
                            this.listData.Add(e.ItemName, e.DataContent);
                        this.drawObject.AttachPropertyData.AddRange(this.listData.Values.ToArray());
                        userWcsRectangle1 wcsRect1 = ((userWcsRectangle1)e.DataContent);
                        if (wcsRect1.EdgesPoint_xyz != null)
                        {
                            for (int i = 0; i < wcsRect1.EdgesPoint_xyz.Length; i++)
                            {
                                this.drawObject.AttachPropertyData.Add(new userWcsPoint(wcsRect1.EdgesPoint_xyz[i].X, wcsRect1.EdgesPoint_xyz[i].Y, 0, wcsRect1.CamParams));
                            }
                        }
                        //this.drawObject.DetachDrawingObjectFromWindow();
                        break;
                    case "userWcsRectangle2":
                        //this.drawObject.AttachPropertyData.Clear();
                        if (this.listData.ContainsKey(e.ItemName))
                            this.listData[e.ItemName] = e.DataContent;
                        else
                            this.listData.Add(e.ItemName, e.DataContent);
                        this.drawObject.AttachPropertyData.AddRange(this.listData.Values.ToArray());
                        userWcsRectangle2 wcsRect2 = ((userWcsRectangle2)e.DataContent);
                        if (wcsRect2.EdgesPoint_xyz != null)
                        {
                            for (int i = 0; i < wcsRect2.EdgesPoint_xyz.Length; i++)
                            {
                                this.drawObject.AttachPropertyData.Add(new userWcsPoint(wcsRect2.EdgesPoint_xyz[i].X, wcsRect2.EdgesPoint_xyz[i].Y, 0, wcsRect2.CamParams));
                            }
                        }
                        //this.drawObject.DetachDrawingObjectFromWindow();
                        break;
                }
                //////////////
                this.drawObject.AttachPropertyData.Clear();
                foreach (KeyValuePair<string, object> item in this.listData)
                {
                    this.drawObject.AttachPropertyData.Add(item.Value);
                }
                this.drawObject.DetachDrawingObjectFromWindow();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        public void DisplayClickObject(object sender, TreeNodeMouseClickEventArgs e)  //
        {
            //if (!IsSelect()) return; // 如果不是当前选择的，则返回
            if (e.Node.Tag == null) return;
            if (e.Button != MouseButtons.Left) return; // 点击右键时不变
            try
            {
                switch (e.Node.Tag.GetType().Name)
                {
                    case "CircleMeasure":
                        this.drawObject?.AttachPropertyData.Clear();
                        if (!(this.drawObject is userDrawCircleMeasure))
                        {
                            this.drawObject.GrayValueInfo -= new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                            this.drawObject.ClearDrawingObject();
                            this.drawObject = new userDrawCircleMeasure(this.hWindowControl1, ((CircleMeasure)e.Node.Tag).FindCircle.CirclePixPosition, ((CircleMeasure)e.Node.Tag).PixCoordSystem);
                            this.drawObject.GrayValueInfo += new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                        }
                        this.drawObject.AttachPropertyData.Clear();
                        foreach (KeyValuePair<string, object> item in this.listData)
                        {
                            if (item.Key != e.Node.Text)
                                this.drawObject.AttachPropertyData.Add(item.Value);
                        }
                        this.drawObject.SetParam(((CircleMeasure)e.Node.Tag).PixCoordSystem);
                        this.drawObject.SetParam(((CircleMeasure)e.Node.Tag).FindCircle.CirclePixPosition.AffineTransPixCircle(((CircleMeasure)e.Node.Tag).PixCoordSystem?.GetVariationHomMat2D()));
                        this.drawObject.BackImage = ((CircleMeasure)e.Node.Tag).ImageData != null ? ((CircleMeasure)e.Node.Tag).ImageData : this.CurrentImageData;
                        this.drawObject.AttachDrawingObjectToWindow();
                        this.metrolegyParamForm.drawObject = this.drawObject;
                        this._currFunction = (CircleMeasure)e.Node.Tag;
                        //DisplayClickItem(sender, e);
                        break;
                    case "CircleSectorMeasure":
                        this.drawObject?.AttachPropertyData.Clear();
                        if (!(this.drawObject is userDrawCircleSectorMeasure))
                        {
                            this.drawObject.GrayValueInfo -= new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                            this.drawObject.ClearDrawingObject();
                            this.drawObject = new userDrawCircleSectorMeasure(this.hWindowControl1, ((CircleSectorMeasure)e.Node.Tag).FindCircleSector.CircleSectorPixPosition, ((CircleSectorMeasure)e.Node.Tag).PixCoordSystem);
                            this.drawObject.GrayValueInfo += new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                        }
                        this.drawObject.AttachPropertyData.Clear();
                        foreach (KeyValuePair<string, object> item in this.listData)
                        {
                            if (item.Key != e.Node.Text)
                                this.drawObject.AttachPropertyData.Add(item.Value);
                        }
                        this.drawObject.SetParam(((CircleSectorMeasure)e.Node.Tag).PixCoordSystem);
                        this.drawObject.SetParam(((CircleSectorMeasure)e.Node.Tag).FindCircleSector.CircleSectorPixPosition.AffineTransPixCircleSector(((CircleSectorMeasure)e.Node.Tag).PixCoordSystem?.GetVariationHomMat2D()));
                        this.drawObject.BackImage = ((CircleSectorMeasure)e.Node.Tag).ImageData != null ? ((CircleSectorMeasure)e.Node.Tag).ImageData : this.CurrentImageData;
                        this.drawObject.AttachDrawingObjectToWindow();
                        this.metrolegyParamForm.drawObject = this.drawObject;
                        this._currFunction = (CircleSectorMeasure)e.Node.Tag;
                        //DisplayClickItem(sender, e);
                        break;
                    case "EllipseMeasure":
                        this.drawObject?.AttachPropertyData.Clear();
                        if (!(this.drawObject is userDrawEllipseMeasure))
                        {
                            this.drawObject.GrayValueInfo -= new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                            this.drawObject.ClearDrawingObject();
                            this.drawObject = new userDrawEllipseMeasure(this.hWindowControl1, ((EllipseMeasure)e.Node.Tag).FindEllipse.EllipsePixPosition, ((EllipseMeasure)e.Node.Tag).PixCoordSystem);
                            this.drawObject.GrayValueInfo += new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                        }
                        this.drawObject.AttachPropertyData.Clear();
                        foreach (KeyValuePair<string, object> item in this.listData)
                        {
                            if (item.Key != e.Node.Text)
                                this.drawObject.AttachPropertyData.Add(item.Value);
                        }
                        this.drawObject.SetParam(((EllipseMeasure)e.Node.Tag).PixCoordSystem);
                        this.drawObject.SetParam(((EllipseMeasure)e.Node.Tag).FindEllipse.EllipsePixPosition.AffineTransPixEllipse(((EllipseMeasure)e.Node.Tag).PixCoordSystem?.GetVariationHomMat2D()));
                        this.drawObject.BackImage = ((EllipseMeasure)e.Node.Tag).ImageData != null ? ((EllipseMeasure)e.Node.Tag).ImageData : this.CurrentImageData;
                        this.drawObject.AttachDrawingObjectToWindow();
                        this.metrolegyParamForm.drawObject = this.drawObject;
                        this._currFunction = (EllipseMeasure)e.Node.Tag;
                        //DisplayClickItem(sender, e);
                        break;
                    case "EllipseSectorMeasure":
                        this.drawObject?.AttachPropertyData.Clear();
                        if (!(this.drawObject is userDrawEllipseSectorMeasure))
                        {
                            this.drawObject.GrayValueInfo -= new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                            this.drawObject.ClearDrawingObject();
                            this.drawObject = new userDrawEllipseSectorMeasure(this.hWindowControl1, ((EllipseSectorMeasure)e.Node.Tag).FindEllipseSector.EllipseSectorPixPosition, ((EllipseSectorMeasure)e.Node.Tag).PixCoordSystem);
                            this.drawObject.GrayValueInfo += new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                        }
                        this.drawObject.AttachPropertyData.Clear();
                        foreach (KeyValuePair<string, object> item in this.listData)
                        {
                            if (item.Key != e.Node.Text)
                                this.drawObject.AttachPropertyData.Add(item.Value);
                        }
                        this.drawObject.SetParam(((EllipseSectorMeasure)e.Node.Tag).PixCoordSystem);
                        this.drawObject.SetParam(((EllipseSectorMeasure)e.Node.Tag).FindEllipseSector.EllipseSectorPixPosition.AffineTransPixEllipseSector(((EllipseSectorMeasure)e.Node.Tag).PixCoordSystem?.GetVariationHomMat2D()));
                        this.drawObject.BackImage = ((EllipseSectorMeasure)e.Node.Tag).ImageData != null ? ((EllipseSectorMeasure)e.Node.Tag).ImageData : this.CurrentImageData;
                        this.drawObject.AttachDrawingObjectToWindow();
                        this.metrolegyParamForm.drawObject = this.drawObject;
                        this._currFunction = (EllipseSectorMeasure)e.Node.Tag;
                        //DisplayClickItem(sender, e);
                        break;
                    case "LineMeasure":
                        this.drawObject?.AttachPropertyData.Clear();
                        if (!(this.drawObject is userDrawLineMeasure))
                        {
                            this.drawObject.GrayValueInfo -= new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                            this.drawObject.ClearDrawingObject();
                            this.drawObject = new userDrawLineMeasure(this.hWindowControl1, ((LineMeasure)e.Node.Tag).FindLine.LinePixPosition, ((LineMeasure)e.Node.Tag).PixCoordSystem);
                            this.drawObject.GrayValueInfo += new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                        }
                        this.drawObject.AttachPropertyData.Clear();
                        foreach (KeyValuePair<string, object> item in this.listData)
                        {
                            if (item.Key != e.Node.Text)
                                this.drawObject.AttachPropertyData.Add(item.Value);
                        }
                        this.drawObject.SetParam(((LineMeasure)e.Node.Tag).PixCoordSystem);
                        this.drawObject.SetParam(((LineMeasure)e.Node.Tag).FindLine.LinePixPosition.AffinePixLine2D(((LineMeasure)e.Node.Tag).PixCoordSystem?.GetVariationHomMat2D()));
                        this.drawObject.BackImage = ((LineMeasure)e.Node.Tag).ImageData != null ? ((LineMeasure)e.Node.Tag).ImageData : this.CurrentImageData;
                        this.drawObject.AttachDrawingObjectToWindow();
                        this.metrolegyParamForm.drawObject = this.drawObject;
                        this._currFunction = (LineMeasure)e.Node.Tag;
                        //DisplayClickItem(sender, e);
                        break;
                    case "PointMeasure":
                        this.drawObject?.AttachPropertyData.Clear();
                        if (!(this.drawObject is userDrawPointMeasure))
                        {
                            this.drawObject.GrayValueInfo -= new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                            this.drawObject.ClearDrawingObject();
                            this.drawObject = new userDrawPointMeasure(this.hWindowControl1, ((PointMeasure)e.Node.Tag).FindPoint.LinePixPosition, ((PointMeasure)e.Node.Tag).PixCoordSystem);
                            this.drawObject.GrayValueInfo += new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                        }
                        this.drawObject.AttachPropertyData.Clear();
                        foreach (KeyValuePair<string, object> item in this.listData)
                        {
                            if (item.Key != e.Node.Text)
                                this.drawObject.AttachPropertyData.Add(item.Value);
                        }
                        this.drawObject.SetParam(((PointMeasure)e.Node.Tag).PixCoordSystem);
                        this.drawObject.SetParam(((PointMeasure)e.Node.Tag).FindPoint.LinePixPosition.AffinePixLine2D(((PointMeasure)e.Node.Tag).PixCoordSystem?.GetVariationHomMat2D()));
                        this.drawObject.BackImage = ((PointMeasure)e.Node.Tag).ImageData != null ? ((PointMeasure)e.Node.Tag).ImageData : this.CurrentImageData;
                        this.drawObject.AttachDrawingObjectToWindow();
                        this.metrolegyParamForm.drawObject = this.drawObject;
                        this._currFunction = (PointMeasure)e.Node.Tag;
                        //DisplayClickItem(sender, e);
                        break;
                    case "Rectangle2Measure":
                        this.drawObject?.AttachPropertyData.Clear();
                        if (!(this.drawObject is userDrawRect2Measure))
                        {
                            this.drawObject.GrayValueInfo -= new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                            this.drawObject.ClearDrawingObject();
                            this.drawObject = new userDrawRect2Measure(this.hWindowControl1, ((Rectangle2Measure)e.Node.Tag).FindRect2.Rect2PixPosition, ((Rectangle2Measure)e.Node.Tag).PixCoordSystem);
                            this.drawObject.GrayValueInfo += new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                        }
                        this.drawObject.AttachPropertyData.Clear();
                        foreach (KeyValuePair<string, object> item in this.listData)
                        {
                            if (item.Key != e.Node.Text)
                                this.drawObject.AttachPropertyData.Add(item.Value);
                        }
                        this.drawObject.SetParam(((Rectangle2Measure)e.Node.Tag).PixCoordSystem);
                        this.drawObject.SetParam(((Rectangle2Measure)e.Node.Tag).FindRect2.Rect2PixPosition.AffineTransPixRect2(((Rectangle2Measure)e.Node.Tag).PixCoordSystem?.GetVariationHomMat2D())); //?.AffineWcsRectangle2D(((Rectangle2Measure)e.Node.Tag).WcsCoordSystem.GetVariationHomMat2D())
                        this.drawObject.BackImage = ((Rectangle2Measure)e.Node.Tag).ImageData != null ? ((Rectangle2Measure)e.Node.Tag).ImageData : this.CurrentImageData;
                        this.drawObject.AttachDrawingObjectToWindow();
                        this.metrolegyParamForm.drawObject = this.drawObject;
                        this._currFunction = (Rectangle2Measure)e.Node.Tag;
                        //DisplayClickItem(sender, e);
                        break;
                    case "WidthMeasure":
                        this.drawObject?.AttachPropertyData.Clear();
                        if (!(this.drawObject is userDrawWidthMeasure))
                        {
                            this.drawObject.GrayValueInfo -= new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                            this.drawObject.ClearDrawingObject();
                            this.drawObject = new userDrawWidthMeasure(this.hWindowControl1, ((WidthMeasure)e.Node.Tag).FindWidth.Rect2PixPosition, ((WidthMeasure)e.Node.Tag).PixCoordSystem);
                            this.drawObject.GrayValueInfo += new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                        }
                        this.drawObject.AttachPropertyData.Clear();
                        foreach (KeyValuePair<string, object> item in this.listData)
                        {
                            if (item.Key != e.Node.Text)
                                this.drawObject.AttachPropertyData.Add(item.Value);
                        }
                        this.drawObject.SetParam(((WidthMeasure)e.Node.Tag).PixCoordSystem);
                        this.drawObject.SetParam(((WidthMeasure)e.Node.Tag).FindWidth.Rect2PixPosition.AffineTransPixRect2(((WidthMeasure)e.Node.Tag).PixCoordSystem?.GetVariationHomMat2D())); //?.AffineWcsRectangle2D(((Rectangle2Measure)e.Node.Tag).WcsCoordSystem.GetVariationHomMat2D())
                        this.drawObject.BackImage = ((WidthMeasure)e.Node.Tag).ImageData != null ? ((WidthMeasure)e.Node.Tag).ImageData : this.CurrentImageData;
                        this.drawObject.AttachDrawingObjectToWindow();
                        this.metrolegyParamForm.drawObject = this.drawObject;
                        this._currFunction = (WidthMeasure)e.Node.Tag;
                        //DisplayClickItem(sender, e);
                        break;
                    case "CrossPointMeasure":
                        this.drawObject?.AttachPropertyData.Clear();
                        if (!(this.drawObject is userDrawCrossMeasure))
                        {
                            this.drawObject.GrayValueInfo -= new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                            this.drawObject.ClearDrawingObject();
                            this.drawObject = new userDrawCrossMeasure(this.hWindowControl1, ((CrossPointMeasure)e.Node.Tag).FindCrossPoint.LinePixPosition, ((CrossPointMeasure)e.Node.Tag).PixCoordSystem);
                            this.drawObject.GrayValueInfo += new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                        }
                        this.drawObject.AttachPropertyData.Clear();
                        foreach (KeyValuePair<string, object> item in this.listData)
                        {
                            if (item.Key != e.Node.Text)
                                this.drawObject.AttachPropertyData.Add(item.Value);
                        }
                        this.drawObject.SetParam(((CrossPointMeasure)e.Node.Tag).PixCoordSystem);
                        this.drawObject.SetParam(((CrossPointMeasure)e.Node.Tag).FindCrossPoint.LinePixPosition.AffinePixLine2D(((CrossPointMeasure)e.Node.Tag).PixCoordSystem.GetVariationHomMat2D()));
                        this.drawObject.BackImage = ((CrossPointMeasure)e.Node.Tag).ImageData != null ? ((CrossPointMeasure)e.Node.Tag).ImageData : this.CurrentImageData;
                        this.drawObject.AttachDrawingObjectToWindow();
                        this.metrolegyParamForm.drawObject = this.drawObject;
                        this._currFunction = (CrossPointMeasure)e.Node.Tag;
                        //DisplayClickItem(sender, e);
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
        public void DisplayClickObject2(object sender, TreeNodeMouseClickEventArgs e)  //
        {
            if (e.Node.Tag == null) return;
            if (e.Button != MouseButtons.Left) return; // 点击右键时不变
            try
            {
                switch (e.Node.Tag.GetType().Name)
                {
                    case "CircleMeasure":
                        this.drawObject?.AttachPropertyData.Clear();
                        if (!(this.drawObject is userDrawCircleMeasure))
                        {
                            this.drawObject.GrayValueInfo -= new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                            this.drawObject.ClearDrawingObject();
                            this.drawObject = new userDrawCircleMeasure(this.hWindowControl1, ((CircleMeasure)e.Node.Tag).FindCircle.CirclePixPosition, ((CircleMeasure)e.Node.Tag).PixCoordSystem);
                            this.drawObject.GrayValueInfo += new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                        }
                        this.drawObject.SetParam(((CircleMeasure)e.Node.Tag).PixCoordSystem);
                        this.drawObject.SetParam(((CircleMeasure)e.Node.Tag).FindCircle.CirclePixPosition.AffineTransPixCircle(((CircleMeasure)e.Node.Tag).PixCoordSystem.GetVariationHomMat2D()));
                        this.drawObject.BackImage = ((CircleMeasure)e.Node.Tag).ImageData;// != null ? ((CircleMeasure)e.Node.Tag).ImageData : this.CurrentImageData;
                        this.drawObject.AttachDrawingObjectToWindow();
                        this.metrolegyParamForm.drawObject = this.drawObject;
                        this._currFunction = (CircleMeasure)e.Node.Tag;
                        DisplayClickItem(sender, e);
                        break;
                    case "CircleSectorMeasure":
                        this.drawObject?.AttachPropertyData.Clear();
                        if (!(this.drawObject is userDrawCircleSectorMeasure))
                        {
                            this.drawObject.GrayValueInfo -= new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                            this.drawObject.ClearDrawingObject();
                            this.drawObject = new userDrawCircleSectorMeasure(this.hWindowControl1, ((CircleSectorMeasure)e.Node.Tag).FindCircleSector.CircleSectorPixPosition, ((CircleSectorMeasure)e.Node.Tag).PixCoordSystem);
                            this.drawObject.GrayValueInfo += new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                        }
                        this.drawObject.SetParam(((CircleSectorMeasure)e.Node.Tag).PixCoordSystem);
                        this.drawObject.SetParam(((CircleSectorMeasure)e.Node.Tag).FindCircleSector.CircleSectorPixPosition.AffineTransPixCircleSector(((CircleSectorMeasure)e.Node.Tag).PixCoordSystem.GetVariationHomMat2D()));
                        this.drawObject.BackImage = ((CircleSectorMeasure)e.Node.Tag).ImageData;// != null ? ((CircleSectorMeasure)e.Node.Tag).ImageData : this.CurrentImageData;
                        this.drawObject.AttachDrawingObjectToWindow();
                        this.metrolegyParamForm.drawObject = this.drawObject;
                        this._currFunction = (CircleSectorMeasure)e.Node.Tag;
                        DisplayClickItem(sender, e);
                        break;
                    case "EllipseMeasure":
                        this.drawObject?.AttachPropertyData.Clear();
                        if (!(this.drawObject is userDrawEllipseMeasure))
                        {
                            this.drawObject.GrayValueInfo -= new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                            this.drawObject.ClearDrawingObject();
                            this.drawObject = new userDrawEllipseMeasure(this.hWindowControl1, ((EllipseMeasure)e.Node.Tag).FindEllipse.EllipsePixPosition, ((EllipseMeasure)e.Node.Tag).PixCoordSystem);
                            this.drawObject.GrayValueInfo += new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                        }
                        this.drawObject.SetParam(((EllipseMeasure)e.Node.Tag).PixCoordSystem);
                        this.drawObject.SetParam(((EllipseMeasure)e.Node.Tag).FindEllipse.EllipsePixPosition.AffineTransPixEllipse(((EllipseMeasure)e.Node.Tag).PixCoordSystem.GetVariationHomMat2D()));
                        this.drawObject.BackImage = ((EllipseMeasure)e.Node.Tag).ImageData;// != null ? ((EllipseMeasure)e.Node.Tag).ImageData : this.CurrentImageData;
                        this.drawObject.AttachDrawingObjectToWindow();
                        this.metrolegyParamForm.drawObject = this.drawObject;
                        this._currFunction = (EllipseMeasure)e.Node.Tag;
                        DisplayClickItem(sender, e);
                        break;
                    case "EllipseSectorMeasure":
                        this.drawObject?.AttachPropertyData.Clear();
                        if (!(this.drawObject is userDrawEllipseSectorMeasure))
                        {
                            this.drawObject.GrayValueInfo -= new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                            this.drawObject.ClearDrawingObject();
                            this.drawObject = new userDrawEllipseSectorMeasure(this.hWindowControl1, ((EllipseSectorMeasure)e.Node.Tag).FindEllipseSector.EllipseSectorPixPosition, ((EllipseSectorMeasure)e.Node.Tag).PixCoordSystem);
                            this.drawObject.GrayValueInfo += new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                        }
                        this.drawObject.SetParam(((EllipseSectorMeasure)e.Node.Tag).PixCoordSystem);
                        this.drawObject.SetParam(((EllipseSectorMeasure)e.Node.Tag).FindEllipseSector.EllipseSectorPixPosition.AffineTransPixEllipseSector(((EllipseSectorMeasure)e.Node.Tag).PixCoordSystem.GetVariationHomMat2D()));
                        this.drawObject.BackImage = ((EllipseSectorMeasure)e.Node.Tag).ImageData;// != null ? ((EllipseSectorMeasure)e.Node.Tag).ImageData : this.CurrentImageData;
                        this.drawObject.AttachDrawingObjectToWindow();
                        this.metrolegyParamForm.drawObject = this.drawObject;
                        this._currFunction = (EllipseSectorMeasure)e.Node.Tag;
                        DisplayClickItem(sender, e);
                        break;
                    case "LineMeasure":
                        this.drawObject?.AttachPropertyData.Clear();
                        if (!(this.drawObject is userDrawLineMeasure))
                        {
                            this.drawObject.GrayValueInfo -= new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                            this.drawObject.ClearDrawingObject();
                            this.drawObject = new userDrawLineMeasure(this.hWindowControl1, ((LineMeasure)e.Node.Tag).FindLine.LinePixPosition, ((LineMeasure)e.Node.Tag).PixCoordSystem);
                            this.drawObject.GrayValueInfo += new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                        }
                        this.drawObject.SetParam(((LineMeasure)e.Node.Tag).PixCoordSystem);
                        this.drawObject.SetParam(((LineMeasure)e.Node.Tag).FindLine.LinePixPosition.AffinePixLine2D(((LineMeasure)e.Node.Tag).PixCoordSystem.GetVariationHomMat2D()));
                        this.drawObject.BackImage = ((LineMeasure)e.Node.Tag).ImageData;// != null ? ((LineMeasure)e.Node.Tag).ImageData : this.CurrentImageData;
                        this.drawObject.AttachDrawingObjectToWindow();
                        this.metrolegyParamForm.drawObject = this.drawObject;
                        this._currFunction = (LineMeasure)e.Node.Tag;
                        DisplayClickItem(sender, e);
                        break;
                    case "PointMeasure":
                        this.drawObject?.AttachPropertyData.Clear();
                        if (!(this.drawObject is userDrawPointMeasure))
                        {
                            this.drawObject.GrayValueInfo -= new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                            this.drawObject.ClearDrawingObject();
                            this.drawObject = new userDrawPointMeasure(this.hWindowControl1, ((PointMeasure)e.Node.Tag).FindPoint.LinePixPosition, ((PointMeasure)e.Node.Tag).PixCoordSystem);
                            this.drawObject.GrayValueInfo += new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                        }
                        this.drawObject.SetParam(((PointMeasure)e.Node.Tag).PixCoordSystem);
                        this.drawObject.SetParam(((PointMeasure)e.Node.Tag).FindPoint.LinePixPosition.AffinePixLine2D(((PointMeasure)e.Node.Tag).PixCoordSystem.GetVariationHomMat2D()));
                        this.drawObject.BackImage = ((PointMeasure)e.Node.Tag).ImageData;// != null ? ((PointMeasure)e.Node.Tag).ImageData : this.CurrentImageData;
                        this.drawObject.AttachDrawingObjectToWindow();
                        this.metrolegyParamForm.drawObject = this.drawObject;
                        this._currFunction = (PointMeasure)e.Node.Tag;
                        DisplayClickItem(sender, e);
                        break;
                    case "Rectangle2Measure":
                        this.drawObject?.AttachPropertyData.Clear();
                        if (!(this.drawObject is userDrawRect2Measure))
                        {
                            this.drawObject.GrayValueInfo -= new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                            this.drawObject.ClearDrawingObject();
                            this.drawObject = new userDrawRect2Measure(this.hWindowControl1, ((Rectangle2Measure)e.Node.Tag).FindRect2.Rect2PixPosition, ((Rectangle2Measure)e.Node.Tag).PixCoordSystem);
                            this.drawObject.GrayValueInfo += new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                        }
                        this.drawObject.SetParam(((Rectangle2Measure)e.Node.Tag).PixCoordSystem);
                        this.drawObject.SetParam(((Rectangle2Measure)e.Node.Tag).FindRect2.Rect2PixPosition.AffineTransPixRect2(((Rectangle2Measure)e.Node.Tag).PixCoordSystem.GetVariationHomMat2D())); //?.AffineWcsRectangle2D(((Rectangle2Measure)e.Node.Tag).WcsCoordSystem.GetVariationHomMat2D())
                        this.drawObject.BackImage = ((Rectangle2Measure)e.Node.Tag).ImageData;// != null ? ((Rectangle2Measure)e.Node.Tag).ImageData : this.CurrentImageData;
                        this.drawObject.AttachDrawingObjectToWindow();
                        this.metrolegyParamForm.drawObject = this.drawObject;
                        this._currFunction = (Rectangle2Measure)e.Node.Tag;
                        DisplayClickItem(sender, e);
                        break;
                    case "CrossPointMeasure":
                        this.drawObject?.AttachPropertyData.Clear();
                        if (!(this.drawObject is userDrawCrossMeasure))
                        {
                            this.drawObject.GrayValueInfo -= new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                            this.drawObject.ClearDrawingObject();
                            this.drawObject = new userDrawCrossMeasure(this.hWindowControl1, ((CrossPointMeasure)e.Node.Tag).FindCrossPoint.LinePixPosition, ((CrossPointMeasure)e.Node.Tag).PixCoordSystem);
                            this.drawObject.GrayValueInfo += new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                        }
                        this.drawObject.SetParam(((CrossPointMeasure)e.Node.Tag).PixCoordSystem);
                        this.drawObject.SetParam(((CrossPointMeasure)e.Node.Tag).FindCrossPoint.LinePixPosition.AffinePixLine2D(((CrossPointMeasure)e.Node.Tag).PixCoordSystem.GetVariationHomMat2D()));
                        this.drawObject.BackImage = ((CrossPointMeasure)e.Node.Tag).ImageData; //!= null ? ((CrossPointMeasure)e.Node.Tag).ImageData : this.CurrentImageData;
                        this.drawObject.AttachDrawingObjectToWindow();
                        this.metrolegyParamForm.drawObject = this.drawObject;
                        this._currFunction = (CrossPointMeasure)e.Node.Tag;
                        //DisplayClickItem(sender, e);
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

        #region 右键菜单项
        private void addContextMenu(HWindowControl hWindowControl)
        {
            ContextMenuStrip ContextMenuStrip1 = new ContextMenuStrip();
            ContextMenuStrip1.Name = hWindowControl.Name;
            // 添加右键菜单 
            ToolStripItem[] items = new ToolStripMenuItem[]
            {
                new ToolStripMenuItem("执行"),
                new ToolStripMenuItem("自适应窗口"),
                new ToolStripMenuItem("设置抓边参数"),
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
                    case "执行":
                        switch (this._currFunction.GetType().Name)
                        {
                            case "CircleMeasure":
                                this._currFunction?.Execute(this.drawObject.GetPixCircleParam());
                                break;
                            case "CircleSectorMeasure":
                                this._currFunction?.Execute(this.drawObject.GetPixCircleSectorParam());
                                break;
                            case "EllipseMeasure":
                                this._currFunction?.Execute(this.drawObject.GetPixEllipseParam());
                                break;
                            case "EllipseSectorMeasure":
                                this._currFunction?.Execute(this.drawObject.GetPixEllipseSectorParam());
                                break;
                            case "LineMeasure":
                                this._currFunction?.Execute(this.drawObject.GetPixLineParam());
                                break;
                            case "PointMeasure":
                                this._currFunction?.Execute(this.drawObject.GetPixLineParam());
                                break;
                            case "Rectangle2Measure":
                                this._currFunction?.Execute(this.drawObject.GetPixRectangle2Param());
                                break;
                            case "WidthMeasure":
                                this._currFunction?.Execute(this.drawObject.GetPixRectangle2Param());
                                break;
                            case "CrossPointMeasure":
                                this._currFunction?.Execute(this.drawObject.GetPixLineParam());
                                break;
                        }
                        break;
                    //////////////////////////////////////
                    case "自适应窗口":
                        this.drawObject?.AutoImage();
                        break;
                    case "设置抓边参数":
                        new MetrolegyParamForm(this._currFunction, this.drawObject).Show();
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




        #endregion


        private void 归一化原点checkBox_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.CamParam.CaliParam.CoordOriginType == enCoordOriginType.IsLoading) return; // 上料模式，原点不做归一化处理
                if (!归一化原点checkBox.Checked) return;
                HTuple Qx = new HTuple();
                HTuple Qy = new HTuple();
                int width, height;
                this.drawObject.BackImage.Image.GetImageSize(out width, out height);
                double wcs_x, wcs_y, wcs_z;
                this.CamParam.ImagePointsToWorldPlane(height * 0.5, width * 0.5, 0, 0, 0, out wcs_x, out wcs_y, out wcs_z);
                if (Math.Abs(wcs_x) > 0.0005)
                    this.CamParam.HomMat2D.c02 -= wcs_x;
                if (Math.Abs(wcs_y) > 0.0005)
                    this.CamParam.HomMat2D.c12 -= wcs_y;
                //////// 平移原点后需要改变拍照点的XY坐标，这样就相当于，原点在图像中心位置来进行标定
                this.CamParam.CaliParam.StartCaliPoint.X += wcs_x;
                this.CamParam.CaliParam.StartCaliPoint.Y += wcs_y;
                this.CamParam.CaliParam.EndCalibPoint.X += wcs_x;
                this.CamParam.CaliParam.EndCalibPoint.Y += wcs_y;
                ///////////////////////////
                this.CamParam.ImagePointsToWorldPlane(height * 0.5, width * 0.5, 0, 0, 0, out wcs_x, out wcs_y, out wcs_z);
                MessageBox.Show("图像中点/视野中心坐标 ：" + wcs_x.ToString("f5") + "  " + wcs_y.ToString("f5"));
                ///////////////////////
                /////////////////////////////////
                this.drawObject.AddViewObject(new ViewData(new userWcsCoordSystem(this.CamParam)));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private CancellationTokenSource cts1;
        private void 上相机实时采集checkBox_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                switch (this.上相机实时采集checkBox.CheckState)
                {
                    case CheckState.Checked:
                        this.上相机实时采集checkBox.BackColor = Color.Red;
                        AcqSource acqSource = AcqSourceManage.Instance.GetCamAcqSource(this.CamParam.SensorName);
                        if (acqSource == null) return;
                        cts1 = new CancellationTokenSource();
                        Dictionary<enDataItem, object> data;
                        Task.Run(() =>
                        {
                            this.drawObject.IsLiveState = true;
                            while (!cts1.IsCancellationRequested)
                            {
                                data = acqSource.AcqImageData(null);
                                switch (acqSource.Sensor?.ConfigParam.SensorType)
                                {
                                    case enUserSensorType.面阵相机:
                                        if (data?.Count > 0)
                                        {
                                            this.drawObject.BackImage = (ImageDataClass)data[enDataItem.Image];
                                            this.drawObject.AttachPropertyData.Clear();
                                            this.drawObject.AttachPropertyData.Add((this.GenCrossLine(this.drawObject.BackImage.Image)));
                                        }
                                        break;
                                }
                                Thread.Sleep(100);
                            }
                            this.drawObject.IsLiveState = false;
                        });
                        break;
                    default:
                        cts1?.Cancel();
                        this.上相机实时采集checkBox.BackColor = Color.Lime;
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

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



    }
}

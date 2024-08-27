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
using Common;
using FunctionBlock;
using HalconDotNet;
using MotionControlCard;
using View;
using System.Threading;

namespace FunctionBlock
{
    public partial class UpDnCamCaliForm : Form
    {
        private TreeViewWrapClass _treeViewWrapClass_Up, _treeViewWrapClass_Down;
        private string programPath = "标定程序\\上下相机映射标定\\";
        private CameraParam CamParam_Map, CamParam_Target;
        private Dictionary<string, userWcsPoint> listWcsPoint = new Dictionary<string, userWcsPoint>();
        private Dictionary<string, userPixPoint> listPixPoint = new Dictionary<string, userPixPoint>();
        private DrawingBaseMeasure drawObject;
        private MetrolegyParamForm metrolegyParamForm;
        private IFunction _currFunction;
        private Dictionary<string, object> listData = new Dictionary<string, object>();



        public UpDnCamCaliForm(CameraParam CamParam)
        {
            InitializeComponent();
            _treeViewWrapClass_Up = new TreeViewWrapClass(this.上相机treeView, this);
            this.CamParam_Map = CamParam;
            BindProperty();
        }
        public UpDnCamCaliForm(CameraParam camParamMap, CameraParam camParamTarget)
        {
            InitializeComponent();
            this._treeViewWrapClass_Up = new TreeViewWrapClass(this.上相机treeView, this);
            this._treeViewWrapClass_Down = new TreeViewWrapClass(this.下相机treeView, this);
            this.drawObject = new DrawingBaseMeasure(this.hWindowControl1, false);
            this.metrolegyParamForm = new MetrolegyParamForm(this.drawObject);
            this.CamParam_Map = camParamMap;
            this.CamParam_Target = camParamTarget;
            ///////////////////////////////////////////////
            if (this.CamParam_Target != null)
                this._treeViewWrapClass_Up.OpenProgram(this.programPath + this.CamParam_Target?.SensorName); // 自动打开程序
            else
                this._treeViewWrapClass_Up.OpenProgram(this.programPath + "none"); // 自动打开程序
            if (this.CamParam_Map != null)
                this._treeViewWrapClass_Down.OpenProgram(this.programPath + this.CamParam_Map?.SensorName);
            else
                this._treeViewWrapClass_Down.OpenProgram(this.programPath + "none");
            this.下相机label.Text = this.CamParam_Map == null ? "none" : this.CamParam_Map.SensorName;
            this.上相机label.Text = this.CamParam_Target == null ? "none" : this.CamParam_Target.SensorName;
            BindProperty();
            this.addContextMenu(this.hWindowControl1);
            this.drawObject.GrayValueInfo += new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
            BaseFunction.ExcuteCompleted += new ExcuteCompletedEventHandler(DisplayExcuteResult);
            TreeViewWrapClass.ClickNode += new ClickNodeEventHandler(this.DisplayClickObject);
        }


        private void BindProperty()
        {
            try
            {
                ////////// 通信配置/////////////////
                //上相机拍照位置textBox.Text = "X:" + MyCaliPara.StartCaliPoint.x.ToString("f2") + "   Y:" +
                // MyCaliPara.StartCaliPoint.y.ToString("f2") + "   Theta:" + MyCaliPara.StartCaliPoint.Angle.ToString("f2");
                //////////////////////////////////////////////////////////////////////////
                //下相机拍照位置textBox.Text = "X:" + MyCaliPara.EndCalibPoint.x.ToString("f2") +
                //    "   Y:" + MyCaliPara.EndCalibPoint.y.ToString("f2") + "   Theta:" + MyCaliPara.EndCalibPoint.Angle.ToString("f2");

                this.AddForm(this.元素tabPage, new ElementViewForm());
                this.LoadMapParam();
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
            }
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
        private void 上相机toolStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            switch (e.ClickedItem.Text)
            {
                case "检测工具":
                    ToolForm tool = new ToolForm(this._treeViewWrapClass_Up, "");
                    tool.Owner = this;
                    tool.Show();
                    break;
                case "执行":
                    this.listWcsPoint?.Clear();
                    this.listPixPoint?.Clear();
                    this.listData?.Clear();
                    this.上相机坐标dataGridView.Rows.Clear();
                    this._treeViewWrapClass_Up.RunSyn(this.toolStripButton_Run, 1);
                    ///////////////////////////////////////////////////////////
                    string[] method = this.映射方法comboBox.SelectedItem.ToString().Split(new string[] { "To" }, StringSplitOptions.RemoveEmptyEntries);
                    if (method != null && method.Length == 2)
                    {
                        switch (method[1])
                        {
                            default:
                            case "Wcs":
                                foreach (KeyValuePair<string, userWcsPoint> item in this.listWcsPoint)
                                {
                                    int index = this.上相机坐标dataGridView.Rows.Add(item.Value.X, item.Value.Y, item.Value.Grab_x, item.Value.Grab_y, item.Value.Grab_z, item.Value.Grab_theta);
                                    this.上相机坐标dataGridView.Rows[index].HeaderCell.Value = (index + 1).ToString();
                                }
                                break;
                            case "Pix":
                                foreach (KeyValuePair<string, userPixPoint> item in this.listPixPoint)
                                {
                                    int index = this.上相机坐标dataGridView.Rows.Add(item.Value.Row, item.Value.Col, item.Value.Grab_x, item.Value.Grab_y, item.Value.Grab_z, item.Value.Grab_theta);
                                    this.上相机坐标dataGridView.Rows[index].HeaderCell.Value = (index + 1).ToString();
                                }
                                break;
                        }
                    }
                    break;
                case "打开":
                    string _programPath = new FileOperate().OpenFile(2);
                    if (_programPath == null || _programPath.Trim().Length == 0) return;
                    this._treeViewWrapClass_Up.OpenProgram(_programPath);
                    break;
                case "保存":
                    string camName = "none";
                    if (this.programPath == null || this.programPath.Length == 0)
                    {
                        if (this.programPath == null || this.programPath.Length == 0) return;
                        if (this.CamParam_Target != null)
                            camName = this.CamParam_Target.SensorName;
                        if (this._treeViewWrapClass_Up.SaveProgram(this.programPath + this.CamParam_Target.SensorName))
                            MessageBox.Show("保存成功");
                        else
                            MessageBox.Show("保存失败" + new Exception().ToString());
                    }
                    else
                    {
                        if (this.CamParam_Target != null)
                            camName = this.CamParam_Target.SensorName;
                        if (this._treeViewWrapClass_Up.SaveProgram(this.programPath + this.CamParam_Target.SensorName))
                            MessageBox.Show("保存成功");
                        else
                            MessageBox.Show("保存失败" + new Exception().ToString());
                    }
                    break;
            }
        }

        private void 下相机toolStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            switch (e.ClickedItem.Text)
            {
                case "检测工具":
                    ToolForm tool = new ToolForm(this._treeViewWrapClass_Down, "");
                    tool.Owner = this;
                    tool.Show();
                    break;
                case "执行":
                    this.listWcsPoint?.Clear();
                    this.listPixPoint?.Clear();
                    this.listData?.Clear();
                    this.下相机坐标dataGridView.Rows.Clear();
                    this._treeViewWrapClass_Down.RunSyn(this.toolStripButton_Run, 1);
                    ///////////////////////////////////////////////////////////
                    string[] method = this.映射方法comboBox.SelectedItem.ToString().Split(new string[] { "To" }, StringSplitOptions.RemoveEmptyEntries);
                    if (method != null && method.Length == 2)
                    {
                        switch (method[0])
                        {
                            default:
                            case "Wcs":
                                foreach (KeyValuePair<string, userWcsPoint> item in this.listWcsPoint)
                                {
                                    int index = this.下相机坐标dataGridView.Rows.Add(item.Value.X, item.Value.Y, item.Value.Grab_x, item.Value.Grab_y, item.Value.Grab_z, item.Value.Grab_theta);
                                    this.下相机坐标dataGridView.Rows[index].HeaderCell.Value = (index + 1).ToString();
                                }
                                break;
                            case "Pix":
                                foreach (KeyValuePair<string, userPixPoint> item in this.listPixPoint)
                                {
                                    int index = this.下相机坐标dataGridView.Rows.Add(item.Value.Row, item.Value.Col, item.Value.Grab_x, item.Value.Grab_y, item.Value.Grab_z, item.Value.Grab_theta);
                                    this.下相机坐标dataGridView.Rows[index].HeaderCell.Value = (index + 1).ToString();
                                }
                                break;
                        }
                    }
                    break;
                case "打开":
                    string _programPath = new FileOperate().OpenFile(2);
                    if (_programPath == null || _programPath.Trim().Length == 0) return;
                    this._treeViewWrapClass_Down.OpenProgram(_programPath);
                    break;
                case "保存":
                    string camName = "none";
                    if (this.programPath == null || this.programPath.Length == 0)
                    {
                        if (this.programPath == null || this.programPath.Length == 0) return;
                        if (this.CamParam_Map != null)
                            camName = this.CamParam_Map.SensorName;
                        if (this._treeViewWrapClass_Down.SaveProgram(this.programPath + camName))
                            MessageBox.Show("保存成功");
                        else
                            MessageBox.Show("保存失败" + new Exception().ToString());
                    }
                    else
                    {
                        if (this.CamParam_Map != null)
                            camName = this.CamParam_Map.SensorName;
                        if (this._treeViewWrapClass_Down.SaveProgram(this.programPath + camName))
                            MessageBox.Show("保存成功");
                        else
                            MessageBox.Show("保存失败" + new Exception().ToString());
                    }
                    break;
            }
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
                        userPixCircle pixCircle = wcsCircle.GetPixCircle();
                        if (wcsCircle.EdgesPoint_xyz != null)
                        {
                            for (int i = 0; i < wcsCircle.EdgesPoint_xyz.Length; i++)
                            {
                                this.drawObject.AttachPropertyData.Add(new userWcsPoint(wcsCircle.EdgesPoint_xyz[i].X, wcsCircle.EdgesPoint_xyz[i].Y, 0, wcsCircle.CamParams));
                            }
                        }
                        this.drawObject.DetachDrawingObjectFromWindow();
                        /////////////////////////////////
                        if (!this.listWcsPoint.ContainsKey(e.ItemName))
                            this.listWcsPoint.Add(e.ItemName, new userWcsPoint(wcsCircle.X, wcsCircle.Y, wcsCircle.Z, wcsCircle.Grab_x, wcsCircle.Grab_y, wcsCircle.CamParams));
                        else
                            this.listWcsPoint[e.ItemName] = new userWcsPoint(wcsCircle.X, wcsCircle.Y, wcsCircle.Z, wcsCircle.Grab_x, wcsCircle.Grab_y, wcsCircle.CamParams);
                        ///////////////////////////////////
                        if (!this.listPixPoint.ContainsKey(e.ItemName))
                            this.listPixPoint.Add(e.ItemName, new userPixPoint(pixCircle.Row, pixCircle.Col, pixCircle.Grab_x, pixCircle.Grab_y, pixCircle.Grab_theta, pixCircle.CamParams));
                        else
                            this.listPixPoint[e.ItemName] = new userPixPoint(pixCircle.Row, pixCircle.Col, pixCircle.Grab_x, pixCircle.Grab_y, pixCircle.Grab_theta, pixCircle.CamParams);
                        break;
                    case "userWcsCircleSector":
                        this.drawObject.AttachPropertyData.Clear();
                        if (this.listData.ContainsKey(e.ItemName))
                            this.listData[e.ItemName] = e.DataContent;
                        else
                            this.listData.Add(e.ItemName, e.DataContent);
                        this.drawObject.AttachPropertyData.AddRange(this.listData.Values.ToArray());
                        userWcsCircleSector wcsCircleSector = ((userWcsCircleSector)e.DataContent);
                        if (wcsCircleSector.EdgesPoint_xyz != null)
                        {
                            for (int i = 0; i < wcsCircleSector.EdgesPoint_xyz.Length; i++)
                            {
                                this.drawObject.AttachPropertyData.Add(new userWcsPoint(wcsCircleSector.EdgesPoint_xyz[i].X, wcsCircleSector.EdgesPoint_xyz[i].Y, 0, wcsCircleSector.CamParams));
                            }
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
                        if (wcsEllips1.EdgesPoint_xyz != null)
                        {
                            for (int i = 0; i < wcsEllips1.EdgesPoint_xyz.Length; i++)
                            {
                                this.drawObject.AttachPropertyData.Add(new userWcsPoint(wcsEllips1.EdgesPoint_xyz[i].X, wcsEllips1.EdgesPoint_xyz[i].Y, 0, wcsEllips1.CamParams));
                            }
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
                        if (wcsEllipseSector.EdgesPoint_xyz != null)
                        {
                            for (int i = 0; i < wcsEllipseSector.EdgesPoint_xyz.Length; i++)
                            {
                                this.drawObject.AttachPropertyData.Add(new userWcsPoint(wcsEllipseSector.EdgesPoint_xyz[i].X, wcsEllipseSector.EdgesPoint_xyz[i].Y, 0, wcsEllipseSector.CamParams));
                            }
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
                        if (wcsLine.EdgesPoint_xyz != null)
                        {
                            for (int i = 0; i < wcsLine.EdgesPoint_xyz.Length; i++)
                            {
                                this.drawObject.AttachPropertyData.Add(new userWcsPoint(wcsLine.EdgesPoint_xyz[i].X, wcsLine.EdgesPoint_xyz[i].Y, 0, wcsLine.CamParams));
                            }
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
                        /////////////////////
                        if (!this.listWcsPoint.ContainsKey(e.ItemName))
                            this.listWcsPoint.Add(e.ItemName, ((userWcsPoint)e.DataContent));
                        else
                            this.listWcsPoint[e.ItemName] = ((userWcsPoint)e.DataContent);
                        ///////////////////////////////////
                        if (!this.listPixPoint.ContainsKey(e.ItemName))
                            this.listPixPoint.Add(e.ItemName, ((userWcsPoint)e.DataContent).GetPixPoint());
                        else
                            this.listPixPoint[e.ItemName] = ((userWcsPoint)e.DataContent).GetPixPoint();
                        break;

                    case "userWcsRectangle2":
                        this.drawObject.AttachPropertyData.Clear();
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
                        this.drawObject.DetachDrawingObjectFromWindow();
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void 映射标定button_Click(object sender, EventArgs e)
        {
            try
            {
                List<double> listUpWcs_x = new List<double>();
                List<double> listUpWcs_y = new List<double>();
                List<double> listUpPix_Row = new List<double>();
                List<double> listUpPix_Col = new List<double>();
                List<double> listUp_GrabX = new List<double>();
                List<double> listUp_GrabY = new List<double>();
                List<double> listUp_GrabZ = new List<double>();
                List<double> listUp_GrabTheta = new List<double>();
                List<double> listDownWcs_x = new List<double>();
                List<double> listDownWcs_y = new List<double>();
                List<double> listDownPix_Row = new List<double>();
                List<double> listDownPix_Col = new List<double>();
                List<double> listDown_GrabX = new List<double>();
                List<double> listDown_GrabY = new List<double>();
                List<double> listDown_GrabZ = new List<double>();
                List<double> listDown_GrabTheta = new List<double>();
                DataGridViewCellCollection CellCollection;
                HHomMat2D hHomMat2D = new HHomMat2D();
                HTuple Qx = 0, Qy = 0, Qz = 0, dist = 0;
                switch (this.映射方法comboBox.SelectedItem.ToString())
                {
                    case "WcsToWcs":
                        ///// 提取上相机坐标
                        for (int i = 0; i < this.上相机坐标dataGridView.Rows.Count; i++)
                        {
                            CellCollection = this.上相机坐标dataGridView.Rows[i].Cells;
                            listUpWcs_x.Add(Convert.ToDouble(CellCollection[0].Value));
                            listUpWcs_y.Add(Convert.ToDouble(CellCollection[1].Value));
                            listUp_GrabX.Add(Convert.ToDouble(CellCollection[2].Value));
                            listUp_GrabY.Add(Convert.ToDouble(CellCollection[3].Value));
                            listUp_GrabZ.Add(Convert.ToDouble(CellCollection[4].Value));
                            listUp_GrabTheta.Add(Convert.ToDouble(CellCollection[5].Value));
                        }
                        //////提取下相机坐标
                        for (int i = 0; i < this.下相机坐标dataGridView.Rows.Count; i++)
                        {
                            CellCollection = this.下相机坐标dataGridView.Rows[i].Cells;
                            listDownWcs_x.Add(Convert.ToDouble(CellCollection[0].Value));
                            listDownWcs_y.Add(Convert.ToDouble(CellCollection[1].Value));
                            listDown_GrabX.Add(Convert.ToDouble(CellCollection[2].Value));
                            listDown_GrabY.Add(Convert.ToDouble(CellCollection[3].Value));
                            listDown_GrabZ.Add(Convert.ToDouble(CellCollection[4].Value));
                            listDown_GrabTheta.Add(Convert.ToDouble(CellCollection[5].Value));
                        }
                        ///////////////////////////////
                        hHomMat2D.VectorToHomMat2d(listDownWcs_x.ToArray(), listDownWcs_y.ToArray(), listUpWcs_x.ToArray(), listUpWcs_y.ToArray());
                        this.CamParam_Map.MapHomMat2D = new UserHomMat2D(hHomMat2D);
                        Qx = hHomMat2D.AffineTransPoint2d(listDownWcs_x.ToArray(), listDownWcs_y.ToArray(), out Qy);
                        dist = HMisc.DistancePp(Qx, Qy, listUpWcs_x.ToArray(), listUpWcs_y.ToArray());
                        break;
                    case "PixToWcs":
                        ///// 提取上相机坐标
                        for (int i = 0; i < this.上相机坐标dataGridView.Rows.Count; i++)
                        {
                            CellCollection = this.上相机坐标dataGridView.Rows[i].Cells;
                            listUpWcs_x.Add(Convert.ToDouble(CellCollection[0].Value));
                            listUpWcs_y.Add(Convert.ToDouble(CellCollection[1].Value));

                            listUp_GrabX.Add(Convert.ToDouble(CellCollection[2].Value));
                            listUp_GrabY.Add(Convert.ToDouble(CellCollection[3].Value));
                            listUp_GrabZ.Add(Convert.ToDouble(CellCollection[4].Value));
                            listUp_GrabTheta.Add(Convert.ToDouble(CellCollection[5].Value));
                        }
                        /// 提取下相机坐标
                        for (int i = 0; i < this.下相机坐标dataGridView.Rows.Count; i++)
                        {
                            CellCollection = this.下相机坐标dataGridView.Rows[i].Cells;
                            listDownPix_Row.Add(Convert.ToDouble(CellCollection[0].Value));
                            listDownPix_Col.Add(Convert.ToDouble(CellCollection[1].Value));

                            listDown_GrabX.Add(Convert.ToDouble(CellCollection[2].Value));
                            listDown_GrabY.Add(Convert.ToDouble(CellCollection[3].Value));
                            listDown_GrabZ.Add(Convert.ToDouble(CellCollection[4].Value));
                            listDown_GrabTheta.Add(Convert.ToDouble(CellCollection[5].Value));
                        }
                        ///////////////////////////////
                        //double center_x = (listUpWcs_x.Max() + listUpWcs_x.Min()) * 0.5;
                        //double center_y = (listUpWcs_y.Max() + listUpWcs_y.Min()) * 0.5;
                        //this.CamParam_Map.CaliParam.StartCaliPoint = new userWcsVector(center_x, center_y, 0, 0); // 将映射世界点的中心作为标定位置点及旋转位置点
                        //this.CamParam_Map.CaliParam.EndCalibPoint = new userWcsVector(center_x, center_y, 0, 0);
                        //this.CamParam_Map.CaliParam.RotateCalibPoint = new userWcsVector(center_x, center_y, 0, 0);
                        /// 上面这些是否需要？？？
                        hHomMat2D.VectorToHomMat2d(listDownPix_Col.ToArray(), listDownPix_Row.ToArray(), listUpWcs_x.ToArray(), listUpWcs_y.ToArray());
                        this.CamParam_Map.HomMat2D = new UserHomMat2D(hHomMat2D);
                        this.CamParam_Map.MapHomMat2D = new UserHomMat2D();
                        this.CamParam_Map.ImagePointsToWorldPlane(listDownPix_Row.ToArray(), listDownPix_Col.ToArray(), 0, 0, 0, out Qx, out Qy, out Qz);
                        dist = HMisc.DistancePp(Qx, Qy, listUpWcs_x.ToArray(), listUpWcs_y.ToArray());
                        break;  
                    case "PixToPix":
                        ///// 提取上相机坐标
                        for (int i = 0; i < this.上相机坐标dataGridView.Rows.Count; i++)
                        {
                            CellCollection = this.上相机坐标dataGridView.Rows[i].Cells;
                            listUpPix_Row.Add(Convert.ToDouble(CellCollection[0].Value));
                            listUpPix_Col.Add(Convert.ToDouble(CellCollection[1].Value));

                            listUp_GrabX.Add(Convert.ToDouble(CellCollection[2].Value));
                            listUp_GrabY.Add(Convert.ToDouble(CellCollection[3].Value));
                            listUp_GrabZ.Add(Convert.ToDouble(CellCollection[4].Value));
                            listUp_GrabTheta.Add(Convert.ToDouble(CellCollection[5].Value));
                        }
                        //////提取下相机坐标
                        for (int i = 0; i < this.下相机坐标dataGridView.Rows.Count; i++)
                        {
                            CellCollection = this.下相机坐标dataGridView.Rows[i].Cells;
                            listDownPix_Row.Add(Convert.ToDouble(CellCollection[0].Value));
                            listDownPix_Col.Add(Convert.ToDouble(CellCollection[1].Value));

                            listDown_GrabX.Add(Convert.ToDouble(CellCollection[2].Value));
                            listDown_GrabY.Add(Convert.ToDouble(CellCollection[3].Value));
                            listDown_GrabZ.Add(Convert.ToDouble(CellCollection[4].Value));
                            listDown_GrabTheta.Add(Convert.ToDouble(CellCollection[5].Value));
                        }
                        ///////////////////////////////
                        hHomMat2D.VectorToHomMat2d(listDownPix_Col.ToArray(), listDownPix_Row.ToArray(), listUpPix_Col.ToArray(), listUpPix_Row.ToArray());
                        this.CamParam_Map.HomMat2D = new UserHomMat2D();
                        this.CamParam_Map.MapHomMat2D = new UserHomMat2D(hHomMat2D);
                        Qx = hHomMat2D.AffineTransPoint2d(listDownPix_Col.ToArray(), listDownPix_Row.ToArray(), out Qy);
                        dist = HMisc.DistancePp(Qx, Qy, listUpPix_Col.ToArray(), listUpPix_Row.ToArray());
                        break;
                }
                this.下相机映射dataGridView.Rows.Clear();
                for (int i = 0; i < Qx.Length; i++)
                {
                    int index = this.下相机映射dataGridView.Rows.Add(Qx[i].D, Qy[i].D);
                    this.下相机映射dataGridView.Rows[index].HeaderCell.Value = (index + 1).ToString();
                }
                /////////
                this.映射dataGridView.Rows.Clear();
                switch (this.映射方法comboBox.SelectedItem.ToString())
                {
                    case "WcsToWcs":
                        this.映射dataGridView.Rows.Add(this.CamParam_Map.MapHomMat2D.c00, this.CamParam_Map.MapHomMat2D.c01, this.CamParam_Map.MapHomMat2D.c02);
                        this.映射dataGridView.Rows.Add(this.CamParam_Map.MapHomMat2D.c10, this.CamParam_Map.MapHomMat2D.c11, this.CamParam_Map.MapHomMat2D.c12);
                        break;
                    case "PixToWcs":
                        this.映射dataGridView.Rows.Add(this.CamParam_Map.HomMat2D.c00, this.CamParam_Map.HomMat2D.c01, this.CamParam_Map.HomMat2D.c02);
                        this.映射dataGridView.Rows.Add(this.CamParam_Map.HomMat2D.c10, this.CamParam_Map.HomMat2D.c11, this.CamParam_Map.HomMat2D.c12);
                        break;
                    case "PixToPix":
                        this.映射dataGridView.Rows.Add(this.CamParam_Map.MapHomMat2D.c00, this.CamParam_Map.MapHomMat2D.c01, this.CamParam_Map.MapHomMat2D.c02);
                        this.映射dataGridView.Rows.Add(this.CamParam_Map.MapHomMat2D.c10, this.CamParam_Map.MapHomMat2D.c11, this.CamParam_Map.MapHomMat2D.c12);
                        break;
                }
                MessageBox.Show("最大标定误差 = " + dist.TupleMax().D.ToString());
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
                        this.drawObject.SetParam(((CircleSectorMeasure)e.Node.Tag).PixCoordSystem);
                        this.drawObject.SetParam(((CircleSectorMeasure)e.Node.Tag).FindCircleSector.CircleSectorPixPosition.AffineTransPixCircleSector(((CircleSectorMeasure)e.Node.Tag).PixCoordSystem.GetVariationHomMat2D()));
                        this.drawObject.BackImage = ((CircleSectorMeasure)e.Node.Tag).ImageData;// != null ? ((CircleSectorMeasure)e.Node.Tag).ImageData : this.CurrentImageData;
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
                        this.drawObject.SetParam(((EllipseMeasure)e.Node.Tag).PixCoordSystem);
                        this.drawObject.SetParam(((EllipseMeasure)e.Node.Tag).FindEllipse.EllipsePixPosition.AffineTransPixEllipse(((EllipseMeasure)e.Node.Tag).PixCoordSystem.GetVariationHomMat2D()));
                        this.drawObject.BackImage = ((EllipseMeasure)e.Node.Tag).ImageData;// != null ? ((EllipseMeasure)e.Node.Tag).ImageData : this.CurrentImageData;
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
                        this.drawObject.SetParam(((EllipseSectorMeasure)e.Node.Tag).PixCoordSystem);
                        this.drawObject.SetParam(((EllipseSectorMeasure)e.Node.Tag).FindEllipseSector.EllipseSectorPixPosition.AffineTransPixEllipseSector(((EllipseSectorMeasure)e.Node.Tag).PixCoordSystem.GetVariationHomMat2D()));
                        this.drawObject.BackImage = ((EllipseSectorMeasure)e.Node.Tag).ImageData;// != null ? ((EllipseSectorMeasure)e.Node.Tag).ImageData : this.CurrentImageData;
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
                        this.drawObject.SetParam(((LineMeasure)e.Node.Tag).PixCoordSystem);
                        this.drawObject.SetParam(((LineMeasure)e.Node.Tag).FindLine.LinePixPosition.AffinePixLine2D(((LineMeasure)e.Node.Tag).PixCoordSystem.GetVariationHomMat2D()));
                        this.drawObject.BackImage = ((LineMeasure)e.Node.Tag).ImageData;// != null ? ((LineMeasure)e.Node.Tag).ImageData : this.CurrentImageData;
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
                        this.drawObject.SetParam(((PointMeasure)e.Node.Tag).PixCoordSystem);
                        this.drawObject.SetParam(((PointMeasure)e.Node.Tag).FindPoint.LinePixPosition.AffinePixLine2D(((PointMeasure)e.Node.Tag).PixCoordSystem.GetVariationHomMat2D()));
                        this.drawObject.BackImage = ((PointMeasure)e.Node.Tag).ImageData;// != null ? ((PointMeasure)e.Node.Tag).ImageData : this.CurrentImageData;
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
                        this.drawObject.SetParam(((Rectangle2Measure)e.Node.Tag).PixCoordSystem);
                        this.drawObject.SetParam(((Rectangle2Measure)e.Node.Tag).FindRect2.Rect2PixPosition.AffineTransPixRect2(((Rectangle2Measure)e.Node.Tag).PixCoordSystem.GetVariationHomMat2D())); //?.AffineWcsRectangle2D(((Rectangle2Measure)e.Node.Tag).WcsCoordSystem.GetVariationHomMat2D())
                        this.drawObject.BackImage = ((Rectangle2Measure)e.Node.Tag).ImageData;// != null ? ((Rectangle2Measure)e.Node.Tag).ImageData : this.CurrentImageData;
                        this.drawObject.AttachDrawingObjectToWindow();
                        this.metrolegyParamForm.drawObject = this.drawObject;
                        this._currFunction = (Rectangle2Measure)e.Node.Tag;
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
                        this.drawObject.SetParam(((CrossPointMeasure)e.Node.Tag).PixCoordSystem);
                        this.drawObject.SetParam(((CrossPointMeasure)e.Node.Tag).FindCrossPoint.LinePixPosition.AffinePixLine2D(((CrossPointMeasure)e.Node.Tag).PixCoordSystem.GetVariationHomMat2D()));
                        this.drawObject.BackImage = ((CrossPointMeasure)e.Node.Tag).ImageData;// != null ? ((CrossPointMeasure)e.Node.Tag).ImageData : this.CurrentImageData;
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
            catch (Exception ex)
            {

            }
        }

        private void UpDnCamCaliForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                this.drawObject.GrayValueInfo -= new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                BaseFunction.ExcuteCompleted -= new ExcuteCompletedEventHandler(DisplayExcuteResult);
                TreeViewWrapClass.ClickNode -= new ClickNodeEventHandler(this.DisplayClickObject);
                this._treeViewWrapClass_Down?.Uinit();
                this._treeViewWrapClass_Up?.Uinit();
            }
            catch
            {

            }
        }
        private CancellationTokenSource cts1;
        private CancellationTokenSource cts2;
        private void 实时采集checkBox_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                switch (this.上相机实时采集checkBox.CheckState)
                {
                    case CheckState.Checked:
                        this.上相机实时采集checkBox.BackColor = Color.Red;
                        AcqSource acqSource = AcqSourceManage.Instance.GetCamAcqSource(this.CamParam_Target.SensorName);
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

        private void 下相机实时采集checkBox_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                switch (this.上相机实时采集checkBox.CheckState)
                {
                    case CheckState.Checked:
                        this.上相机实时采集checkBox.BackColor = Color.Red;
                        AcqSource acqSource = AcqSourceManage.Instance.GetCamAcqSource(this.CamParam_Map.SensorName);
                        if (acqSource == null) return;
                        cts2 = new CancellationTokenSource();
                        Dictionary<enDataItem, object> data;
                        Task.Run(() =>
                        {
                            this.drawObject.IsLiveState = true;
                            while (!cts2.IsCancellationRequested)
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
                        cts2?.Cancel();
                        this.上相机实时采集checkBox.BackColor = Color.Lime;
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void 重置下相机映射but_Click(object sender, EventArgs e)
        {
            try
            {
                switch (this.映射方法comboBox.SelectedItem.ToString())
                {
                    case "WcsToWcs":
                        this.映射dataGridView.Rows.Clear();
                        this.CamParam_Map.MapHomMat2D = new UserHomMat2D();
                        this.映射dataGridView.Rows.Add(this.CamParam_Map.MapHomMat2D.c00, this.CamParam_Map.MapHomMat2D.c01, this.CamParam_Map.MapHomMat2D.c02);
                        this.映射dataGridView.Rows.Add(this.CamParam_Map.MapHomMat2D.c10, this.CamParam_Map.MapHomMat2D.c11, this.CamParam_Map.MapHomMat2D.c12);
                        break;
                    case "PixToWcs":
                        this.映射dataGridView.Rows.Clear();
                        this.CamParam_Map.HomMat2D = new UserHomMat2D();
                        this.映射dataGridView.Rows.Add(this.CamParam_Map.HomMat2D.c00, this.CamParam_Map.HomMat2D.c01, this.CamParam_Map.HomMat2D.c02);
                        this.映射dataGridView.Rows.Add(this.CamParam_Map.HomMat2D.c10, this.CamParam_Map.HomMat2D.c11, this.CamParam_Map.HomMat2D.c12);
                        break;
                    case "PixToPix":
                        this.映射dataGridView.Rows.Clear();
                        this.CamParam_Map.MapHomMat2D = new UserHomMat2D();
                        this.映射dataGridView.Rows.Add(this.CamParam_Map.MapHomMat2D.c00, this.CamParam_Map.MapHomMat2D.c01, this.CamParam_Map.MapHomMat2D.c02);
                        this.映射dataGridView.Rows.Add(this.CamParam_Map.MapHomMat2D.c10, this.CamParam_Map.MapHomMat2D.c11, this.CamParam_Map.MapHomMat2D.c12);
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void LoadMapParam()
        {
            try
            {
                switch (this.映射方法comboBox.SelectedItem.ToString())
                {
                    case "WcsToWcs":
                        this.映射dataGridView.Rows.Clear();
                        this.映射dataGridView.Rows.Add(this.CamParam_Map.MapHomMat2D.c00, this.CamParam_Map.MapHomMat2D.c01, this.CamParam_Map.MapHomMat2D.c02);
                        this.映射dataGridView.Rows.Add(this.CamParam_Map.MapHomMat2D.c10, this.CamParam_Map.MapHomMat2D.c11, this.CamParam_Map.MapHomMat2D.c12);
                        break;
                    case "PixToWcs":
                        this.映射dataGridView.Rows.Clear();
                        this.映射dataGridView.Rows.Add(this.CamParam_Map.HomMat2D.c00, this.CamParam_Map.HomMat2D.c01, this.CamParam_Map.HomMat2D.c02);
                        this.映射dataGridView.Rows.Add(this.CamParam_Map.HomMat2D.c10, this.CamParam_Map.HomMat2D.c11, this.CamParam_Map.HomMat2D.c12);
                        break;
                    case "PixToPix":
                        this.映射dataGridView.Rows.Clear();
                        this.映射dataGridView.Rows.Add(this.CamParam_Map.MapHomMat2D.c00, this.CamParam_Map.MapHomMat2D.c01, this.CamParam_Map.MapHomMat2D.c02);
                        this.映射dataGridView.Rows.Add(this.CamParam_Map.MapHomMat2D.c10, this.CamParam_Map.MapHomMat2D.c11, this.CamParam_Map.MapHomMat2D.c12);
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
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
                new ToolStripMenuItem("设置抓边参数"),
                new ToolStripMenuItem("------------"),
                new ToolStripMenuItem("自适应窗口"),
                new ToolStripMenuItem("清除窗口"),
                new ToolStripMenuItem("保存图像"),
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
                    case "清除窗口":
                        this.drawObject?.ClearWindow();
                        this.listData.Clear(); // 清除窗口时,对象也清除
                        break;
                    case "设置抓边参数":
                        MetrolegyParamForm paramForm = new MetrolegyParamForm(this._currFunction, this.drawObject);
                        paramForm.Show();
                        paramForm.Owner = this;
                        break;
                    case "保存图像":
                        ((ContextMenuStrip)sender).Close();
                        SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                        saveFileDialog1.Filter = "bmp files (*.bmp)|*.bmp|All files (*.*)|*.*";
                        saveFileDialog1.FilterIndex = 0;
                        saveFileDialog1.ShowDialog();
                        if (this.drawObject.BackImage != null && this.drawObject.BackImage.Image.IsInitialized())
                            this.drawObject.BackImage.Image.WriteImage("bmp", 0, saveFileDialog1.FileName);
                        else
                            MessageBox.Show("图像内容为空");
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



    }
}

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
    public partial class DistortionCalibForm : Form
    {
        private Dictionary<string, object> listData = new Dictionary<string, object>();
        private TreeViewWrapClass _treeViewWrapClass;
        private string programPath = "标定程序\\相机畸变标定";
        private string _CamName = "";
        private CalibCoordConfigParamManager calibCoordConfigParamNPoint;
        private DrawingBaseMeasure drawObject;
        private MetrolegyParamForm metrolegyParamForm;
        private IFunction _currFunction;
        private CameraParam CamParam;
        private ImageDataClass CurrentImageData;
        private HImage Map, MapImage, sourceImage;
        private BindingList<userWcsPoint> listWcsPoint = new BindingList<userWcsPoint>();
        private Dictionary<string, userPixPoint> listPixPoint = new Dictionary<string, userPixPoint>();
        private Dictionary<string, userPixPoint> listWcsPointDic = new Dictionary<string, userPixPoint>();
        public DistortionCalibForm(CameraParam CamParam)
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
        public DistortionCalibForm(string CamName)
        {
            InitializeComponent();
            this._CamName = CamName;
            this._treeViewWrapClass = new TreeViewWrapClass(this.treeView1, this);
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

            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
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
                    this.listPixPoint.Clear();
                    this._treeViewWrapClass.RunSyn(this.toolStripButton_Run, 1);
                    ///////////////////////////////////////////////////////////
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
                double cirDist;
                double.TryParse(this.圆心距textBox.Text, out cirDist);
                int colCount = 0,rowCount = 0;
                int.TryParse(this.行数domainUpDown.Text, out rowCount);
                int.TryParse(this.列数domainUpDown.Text, out colCount);
                double[] rows = new double[this.listPixPoint.Count];
                double[] cols = new double[this.listPixPoint.Count];
                int index = 0;
                foreach (KeyValuePair<string,userPixPoint> item in this.listPixPoint)
                {
                    rows[index] = item.Value.Row;
                    cols[index] = item.Value.Col;
                    index++;
                }
                HTuple indexs = new HTuple(rows).TupleSortIndex();
                HTuple rowsSort = new HTuple(rows).TupleSelect(indexs);
                HTuple colsSort = new HTuple(cols).TupleSelect(indexs);
                List<double> listCol = new List<double>();
                List<double> listRow = new List<double>();
                for (int i = 0; i < rowCount; i++)
                {                  
                    HTuple subRow = rowsSort.TupleSelectRange(i * colCount, (i + 1) * colCount - 1);
                    HTuple subCol = colsSort.TupleSelectRange(i * colCount, (i + 1) * colCount - 1);
                    HTuple subIndex = subCol.TupleSortIndex();
                    listRow.AddRange(subRow.TupleSelect(subIndex).DArr);
                    listCol.AddRange(subCol.TupleSort().DArr);
                }
                rowsSort = new HTuple(listRow.ToArray());
                colsSort = new HTuple(listCol.ToArray());
                ////////////////////////////////////////////////
                int gridSpace = (int)HMisc.DistancePp(rowsSort[0].D, colsSort[0].D, rowsSort[1].D, colsSort[1].D);  // 网格间距
                int gridWidth = colCount;
                int imageWidth, imageHeight;
                this.drawObject.BackImage.Image.GetImageSize(out imageWidth, out imageHeight);
                this.Map = HMisc.GenArbitraryDistortionMap(gridSpace, rowsSort, colsSort, gridWidth, imageWidth, imageHeight, "bilinear");
                this.MapImage = this.drawObject.BackImage.Image?.MapImage(this.Map);
                this.drawObject.BackImage = new ImageDataClass(this.MapImage);
                this.CamParam.PixScale = cirDist / gridSpace;
                ////////////////////////////////////////////////
            }
            catch (Exception ee)
            {
                MessageBox.Show("标定失败" + ee.ToString());
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
        private void CaliCaliboardForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.cts1?.Cancel();
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
                        this.drawObject.AttachPropertyData.Clear();
                        this.drawObject.BackImage = (ImageDataClass)e.DataContent;
                        this.CurrentImageData = this.drawObject.BackImage;
                        break;

                    case "userWcsCircle":
                        if (this.listData.ContainsKey(e.ItemName))
                            this.listData[e.ItemName] = e.DataContent;
                        else
                            this.listData.Add(e.ItemName, e.DataContent);
                        userWcsCircle wcsCircle = ((userWcsCircle)e.DataContent);
                        this.drawObject.AddViewObject(new ViewData(wcsCircle.GetPixCircle().GetXLD(),"green"));
                        //if (wcsCircle.EdgesPoint_xyz != null)
                        //{
                        //    for (int i = 0; i < wcsCircle.EdgesPoint_xyz.Length; i++)
                        //    {
                        //        this.drawObject.AddViewObject(new ViewData(wcsCircle.EdgesPoint_xyz[i].GetPixPoint(), "green"));
                        //        //this.drawObject.AttachPropertyData.Add(new userWcsPoint(wcsCircle.EdgesPoint_xyz[i].X, wcsCircle.EdgesPoint_xyz[i].Y, 0, wcsCircle.CamParams));
                        //    }
                        //}
                        /////////////////////////////////
                        if (!this.listPixPoint.ContainsKey(e.ItemName))
                            this.listPixPoint.Add(e.ItemName, new userWcsPoint(wcsCircle.X, wcsCircle.Y, wcsCircle.Z, wcsCircle.Grab_x, wcsCircle.Grab_y, wcsCircle.CamParams).GetPixPoint());
                        else
                            this.listPixPoint[e.ItemName] = new userWcsPoint(wcsCircle.X, wcsCircle.Y, wcsCircle.Z, wcsCircle.Grab_x, wcsCircle.Grab_y, wcsCircle.CamParams).GetPixPoint();
                        break;
                    case "userWcsCircleSector":
                        if (this.listData.ContainsKey(e.ItemName))
                            this.listData[e.ItemName] = e.DataContent;
                        else
                            this.listData.Add(e.ItemName, e.DataContent);
                        userWcsCircleSector wcsCircleSector = ((userWcsCircleSector)e.DataContent);
                        this.drawObject.AddViewObject(new ViewData(wcsCircleSector.GetPixCircleSector().GetXLD(), "green"));
                        //if (wcsCircleSector.EdgesPoint_xyz != null)
                        //{
                        //    for (int i = 0; i < wcsCircleSector.EdgesPoint_xyz.Length; i++)
                        //    {
                        //        this.drawObject.AddViewObject(new ViewData(wcsCircleSector.EdgesPoint_xyz[i].GetPixPoint(), "green"));
                        //        //this.drawObject.AttachPropertyData.Add(new userWcsPoint(wcsCircleSector.EdgesPoint_xyz[i].X, wcsCircleSector.EdgesPoint_xyz[i].Y, 0, wcsCircleSector.CamParams));
                        //    }
                        //}
                        if (!this.listPixPoint.ContainsKey(e.ItemName))
                            this.listPixPoint.Add(e.ItemName, new userWcsPoint(wcsCircleSector.X, wcsCircleSector.Y, wcsCircleSector.Z, wcsCircleSector.Grab_x, wcsCircleSector.Grab_y, wcsCircleSector.CamParams).GetPixPoint());
                        else
                            this.listPixPoint[e.ItemName] = new userWcsPoint(wcsCircleSector.X, wcsCircleSector.Y, wcsCircleSector.Z, wcsCircleSector.Grab_x, wcsCircleSector.Grab_y, wcsCircleSector.CamParams).GetPixPoint();
                        break;
                    case "userWcsEllipse":
                        if (this.listData.ContainsKey(e.ItemName))
                            this.listData[e.ItemName] = e.DataContent;
                        else
                            this.listData.Add(e.ItemName, e.DataContent);
                        userWcsEllipse wcsEllips1 = ((userWcsEllipse)e.DataContent);
                        this.drawObject.AddViewObject(new ViewData(wcsEllips1.GetPixEllipse().GetXLD(), "green"));
                        //if (wcsEllips1.EdgesPoint_xyz != null)
                        //{
                        //    for (int i = 0; i < wcsEllips1.EdgesPoint_xyz.Length; i++)
                        //    {
                        //        this.drawObject.AddViewObject(new ViewData(wcsEllips1.EdgesPoint_xyz[i].GetPixPoint(), "green"));
                        //        //this.drawObject.AttachPropertyData.Add(new userWcsPoint(wcsEllips1.EdgesPoint_xyz[i].X, wcsEllips1.EdgesPoint_xyz[i].Y, 0, wcsEllips1.CamParams));
                        //    }
                        //}
                        if (!this.listPixPoint.ContainsKey(e.ItemName))
                            this.listPixPoint.Add(e.ItemName, new userWcsPoint(wcsEllips1.X, wcsEllips1.Y, wcsEllips1.Z, wcsEllips1.Grab_x, wcsEllips1.Grab_y, wcsEllips1.CamParams).GetPixPoint());
                        else
                            this.listPixPoint[e.ItemName] = new userWcsPoint(wcsEllips1.X, wcsEllips1.Y, wcsEllips1.Z, wcsEllips1.Grab_x, wcsEllips1.Grab_y, wcsEllips1.CamParams).GetPixPoint();
                        break;
                    case "userWcsEllipseSector":
                        if (this.listData.ContainsKey(e.ItemName))
                            this.listData[e.ItemName] = e.DataContent;
                        else
                            this.listData.Add(e.ItemName, e.DataContent);
                        userWcsEllipseSector wcsEllipseSector = ((userWcsEllipseSector)e.DataContent);
                        this.drawObject.AddViewObject(new ViewData(wcsEllipseSector.GetPixEllipseSector().GetXLD(), "green"));
                        //if (wcsEllipseSector.EdgesPoint_xyz != null)
                        //{
                        //    for (int i = 0; i < wcsEllipseSector.EdgesPoint_xyz.Length; i++)
                        //    {
                        //        this.drawObject.AddViewObject(new ViewData(wcsEllipseSector.EdgesPoint_xyz[i].GetPixPoint(), "green"));
                        //        //this.drawObject.AttachPropertyData.Add(new userWcsPoint(wcsEllipseSector.EdgesPoint_xyz[i].X, wcsEllipseSector.EdgesPoint_xyz[i].Y, 0, wcsEllipseSector.CamParams));
                        //    }
                        //}
                        if (!this.listPixPoint.ContainsKey(e.ItemName))
                            this.listPixPoint.Add(e.ItemName, new userWcsPoint(wcsEllipseSector.X, wcsEllipseSector.Y, wcsEllipseSector.Z, wcsEllipseSector.Grab_x, wcsEllipseSector.Grab_y, wcsEllipseSector.CamParams).GetPixPoint());
                        else
                            this.listPixPoint[e.ItemName] = new userWcsPoint(wcsEllipseSector.X, wcsEllipseSector.Y, wcsEllipseSector.Z, wcsEllipseSector.Grab_x, wcsEllipseSector.Grab_y, wcsEllipseSector.CamParams).GetPixPoint();
                        break;
                    case "userWcsLine":
                        if (this.listData.ContainsKey(e.ItemName))
                            this.listData[e.ItemName] = e.DataContent;
                        else
                            this.listData.Add(e.ItemName, e.DataContent);
                        userWcsLine wcsLine = ((userWcsLine)e.DataContent);
                        this.drawObject.AddViewObject(new ViewData(wcsLine.GetPixLine().GetXLD(), "green"));
                        //if (wcsLine.EdgesPoint_xyz != null)
                        //{
                        //    for (int i = 0; i < wcsLine.EdgesPoint_xyz.Length; i++)
                        //    {
                        //        this.drawObject.AddViewObject(new ViewData(wcsLine.EdgesPoint_xyz[i].GetPixPoint(), "green"));
                        //        //this.drawObject.AttachPropertyData.Add(new userWcsPoint(wcsLine.EdgesPoint_xyz[i].X, wcsLine.EdgesPoint_xyz[i].Y, 0, wcsLine.CamParams));
                        //    }
                        //}
                        if (!this.listPixPoint.ContainsKey(e.ItemName))
                            this.listPixPoint.Add(e.ItemName, new userWcsPoint(wcsLine.X1, wcsLine.Y1, wcsLine.Z1, wcsLine.Grab_x, wcsLine.Grab_y, wcsLine.CamParams).GetPixPoint());
                        else
                            this.listPixPoint[e.ItemName] = new userWcsPoint(wcsLine.X1, wcsLine.Y1, wcsLine.Z1, wcsLine.Grab_x, wcsLine.Grab_y, wcsLine.CamParams).GetPixPoint();
                        break;

                    case "userWcsPoint":
                        if (this.listData.ContainsKey(e.ItemName))
                            this.listData[e.ItemName] = e.DataContent;
                        else
                            this.listData.Add(e.ItemName, e.DataContent);
                        userWcsPoint wcsPoint  = ((userWcsPoint)e.DataContent);
                        this.drawObject.AddViewObject(new ViewData(wcsPoint.GetPixPoint().GetXLD(), "green"));
                        /////////////////////
                        if (!this.listPixPoint.ContainsKey(e.ItemName))
                            this.listPixPoint.Add(e.ItemName, ((userWcsPoint)e.DataContent).GetPixPoint());
                        else
                            this.listPixPoint[e.ItemName] = ((userWcsPoint)e.DataContent).GetPixPoint();
                        break;
                    case "userWcsRectangle1":
                        if (this.listData.ContainsKey(e.ItemName))
                            this.listData[e.ItemName] = e.DataContent;
                        else
                            this.listData.Add(e.ItemName, e.DataContent);
                        userWcsRectangle1 wcsRect1 = ((userWcsRectangle1)e.DataContent);
                        this.drawObject.AddViewObject(new ViewData(wcsRect1.GetPixRectangle1().GetXLD(), "green"));
                        //if (wcsRect1.EdgesPoint_xyz != null)
                        //{
                        //    for (int i = 0; i < wcsRect1.EdgesPoint_xyz.Length; i++)
                        //    {
                        //        this.drawObject.AddViewObject(new ViewData(wcsRect1.EdgesPoint_xyz[i].GetPixPoint(), "green"));
                        //        //this.drawObject.AttachPropertyData.Add(new userWcsPoint(wcsRect1.EdgesPoint_xyz[i].X, wcsRect1.EdgesPoint_xyz[i].Y, 0, wcsRect1.CamParams));
                        //    }
                        //}

                        break;
                    case "userWcsRectangle2":
                        if (this.listData.ContainsKey(e.ItemName))
                            this.listData[e.ItemName] = e.DataContent;
                        else
                            this.listData.Add(e.ItemName, e.DataContent);
                        userWcsRectangle2 wcsRect2 = ((userWcsRectangle2)e.DataContent);
                        this.drawObject.AddViewObject(new ViewData(wcsRect2.GetPixRectangle2().GetXLD(), "green"));
                        //if (wcsRect2.EdgesPoint_xyz != null)
                        //{
                        //    for (int i = 0; i < wcsRect2.EdgesPoint_xyz.Length; i++)
                        //    {
                        //        this.drawObject.AddViewObject(new ViewData(wcsRect2.EdgesPoint_xyz[i].GetPixPoint(), "green"));
                        //        //this.drawObject.AttachPropertyData.Add(new userWcsPoint(wcsRect2.EdgesPoint_xyz[i].X, wcsRect2.EdgesPoint_xyz[i].Y, 0, wcsRect2.CamParams));
                        //    }
                        //}
                        if (!this.listPixPoint.ContainsKey(e.ItemName))
                            this.listPixPoint.Add(e.ItemName, new userWcsPoint(wcsRect2.X, wcsRect2.Y, wcsRect2.Z, wcsRect2.Grab_x, wcsRect2.Grab_y, wcsRect2.CamParams).GetPixPoint());
                        else
                            this.listPixPoint[e.ItemName] = new userWcsPoint(wcsRect2.X, wcsRect2.Y, wcsRect2.Z, wcsRect2.Grab_x, wcsRect2.Grab_y, wcsRect2.CamParams).GetPixPoint();
                        break;
                }
                //////////////
                //this.drawObject.AttachPropertyData.Clear();
                //foreach (KeyValuePair<string, object> item in this.listData)
                //{
                //    this.drawObject.AttachPropertyData.Add(item.Value);
                //}
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

        private void 原图Btn_Click(object sender, EventArgs e)
        {
            try
            {
                this.drawObject.BackImage = this.CurrentImageData;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void 校正图Btn_Click(object sender, EventArgs e)
        {
            try
            {
                this.drawObject.BackImage = new ImageDataClass(this.MapImage);
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

        private void 保存button_Click(object sender, EventArgs e)
        {
            try
            {
                this.CamParam.Map = this.Map;
                this.CamParam.Save();
            }
            catch (Exception ee)
            {
                MessageBox.Show("标定失败" + ee.ToString());
            }
        }


    }
}

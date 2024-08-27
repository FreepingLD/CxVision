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

namespace FunctionBlock
{
    public partial class CaliCamSlantForm : Form
    {
        private Dictionary<string, object> listData = new Dictionary<string, object>();
        private TreeViewWrapClass _treeViewWrapClass;
        private CameraParam CamParam;
        private string _CamName = "";
        private CalibCoordConfigParamManager calibCoordConfigParamNPoint;
        private DrawingBaseMeasure drawObject;
        private MetrolegyParamForm metrolegyParamForm;
        private IFunction _currFunction;
        private userWcsVector StartCaliPoint, EndCalibPoint;
        private List<userPixPoint> listPoint = new List<userPixPoint>();
        private bool isStop = false;
        private string programPath = "标定程序\\倾斜标定"; // 程序文件路径
        public CaliCamSlantForm(CameraParam camParam)
        {
            InitializeComponent();
            this.CamParam = camParam;
            this._treeViewWrapClass = new TreeViewWrapClass(this.treeView1, this);
            this.calibCoordConfigParamNPoint = new CalibCoordConfigParamManager();
            this.calibCoordConfigParamNPoint.Read(this._CamName + "T");
            this.drawObject = new DrawingBaseMeasure(this.hWindowControl1, false);
            this.addHWindowControlContextMenu(this.hWindowControl1);
            // 自动打开程序
            this.programPath +=  "\\" + camParam.SensorName;
            this._treeViewWrapClass.OpenProgram(this.programPath);
        }


        private void CaliCamSlantForm_Load(object sender, EventArgs e)
        {
            this.drawObject.GrayValueInfo += new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
            BaseFunction.ExcuteCompleted += new ExcuteCompletedEventHandler(DisplayExcuteResult);
            TreeViewWrapClass.ClickNode += new ClickNodeEventHandler(this.DisplayClickObject);
            BindProperty();
        }

        private void BindProperty()
        {
            try
            {
                this.坐标值类型comboBox.DataSource = Enum.GetValues(typeof(enCoordValueType)); // 这里不需要更改本标定坐标系，只需要显示即可
                this.坐标值类型comboBox.DataBindings.Add(nameof(this.坐标值类型comboBox.Text), this.CamParam.CaliParam, nameof(this.CamParam.CaliParam.CoordValueType), true, DataSourceUpdateMode.OnPropertyChanged);
                this.移动轴comboBox.DataSource = Enum.GetValues(typeof(enScanAxis));
                this.移动轴comboBox.DataBindings.Add(nameof(this.移动轴comboBox.Text), this.CamParam.SlantCalibParam, nameof(this.CamParam.SlantCalibParam.MoveAxis), true, DataSourceUpdateMode.OnPropertyChanged);
                this.起始点textBox.Text = this.CamParam.SlantCalibParam.StartCaliPoint.ToString();
                this.终止点textBox.Text = this.CamParam.SlantCalibParam.EndCalibPoint.ToString();
                this.角度textBox.Text = this.CamParam.SlantCalibParam.CamSlantDeg.ToString();
                ////////// 通信配置/////////////////
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


        private void 获取起始点button_Click_1(object sender, EventArgs e)
        {
            try
            {
                double X = 0, Y = 0, Z = 0, Theta = 0;
                Read(out X, out Y, out Z, out Theta);
                /////////////////////
                this.StartCaliPoint = new userWcsVector(X, Y, Z, Theta);
                this.CamParam.SlantCalibParam.StartCaliPoint = this.StartCaliPoint;
                this.起始点textBox.Text = "X:" + X.ToString("f3") + "   Y:" + Y.ToString("f3") + "   Theta:" + Theta.ToString("f3");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void 获取终止点button_Click_1(object sender, EventArgs e)
        {
            try
            {
                double X = 0, Y = 0, Z = 0, Theta = 0;
                Read(out X, out Y, out Z, out Theta);
                ///////////////////////
                this.EndCalibPoint = new userWcsVector(X, Y, Z, Theta);
                this.CamParam.SlantCalibParam.EndCalibPoint = this.EndCalibPoint;
                this.终止点textBox.Text = "X:" + X.ToString("f3") + "   Y:" + Y.ToString("f3") + "   Theta:" + Theta.ToString("f3");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void 起始点textBox_TextChanged(object sender, EventArgs e)
        {
            string[] name = 起始点textBox.Text.Split(new string[] { "X:", "Y:", "Theta:" }, StringSplitOptions.RemoveEmptyEntries);
            if (name.Length < 3) return;
            ////////////////////////////////////
            double X, Y, Theta;
            bool result1 = double.TryParse(name[0].Trim(), out X);
            bool result2 = double.TryParse(name[1].Trim(), out Y);
            bool result3 = double.TryParse(name[2].Trim(), out Theta);
            if (result1 && result2 && result3)
                this.StartCaliPoint = new userWcsVector(X, Y, 0, Theta);
            else
                MessageBox.Show("数据转换报错");
        }
        private void 终止点textBox_TextChanged(object sender, EventArgs e)
        {
            string[] name = 终止点textBox.Text.Split(new string[] { "X:", "Y:", "Theta:" }, StringSplitOptions.RemoveEmptyEntries);
            if (name.Length < 3) return;
            ////////////////////////////////////
            double X, Y, Theta;
            bool result1 = double.TryParse(name[0].Trim(), out X);
            bool result2 = double.TryParse(name[1].Trim(), out Y);
            bool result3 = double.TryParse(name[2].Trim(), out Theta);
            if (result1 && result2 && result3)
                this.EndCalibPoint = new userWcsVector(X, Y, 0, Theta);
            else
                MessageBox.Show("数据转换报错");
        }
        private void CalculateSlantAngle(List<userPixPoint> listPoint,out double deg,out userPixLine pixline)
        {
                HTuple Rows = new HTuple();
                HTuple Cols = new HTuple();
            pixline = new userPixLine();
            for (int i = 0; i < listPoint.Count; i++)
                {
                    Rows[i] = listPoint[i].Row;
                    Cols[i] = listPoint[i].Col;
                }
                /////////////////////////////
                HTuple RowBegin, ColBegin, RowEnd, ColEnd, Nr, Nc, Dist, phi;
                HXLDCont line = new HXLDCont(Rows, Cols);
                //////////////////////////////////////
                line.FitLineContourXld("tukey", -1, 0, 10, 2, out RowBegin, out ColBegin, out RowEnd, out ColEnd, out Nr, out Nc, out Dist); // 标定相机倾斜角时，只能用像素坐标，因相机还未标定
                HOperatorSet.LineOrientation(RowBegin, ColBegin, RowEnd, ColEnd, out phi);              
                pixline = new userPixLine(RowBegin, ColBegin, RowEnd, ColEnd);
                ///////////;
                deg = phi.TupleDeg().D;
        }
        private void CaliCamSlantForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                this.drawObject.GrayValueInfo -= new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                BaseFunction.ExcuteCompleted -= new ExcuteCompletedEventHandler(DisplayExcuteResult);
                TreeViewWrapClass.ClickNode -= new ClickNodeEventHandler(this.DisplayClickObject);
                this._treeViewWrapClass?.Uinit();
                this._treeViewWrapClass?.Uinit();
            }
            catch
            {

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
                        for (int i = 0; i < wcsCircle.EdgesPoint_xyz.Length; i++)
                        {
                            this.drawObject.AttachPropertyData.Add(new userWcsPoint(wcsCircle.EdgesPoint_xyz[i].X, wcsCircle.EdgesPoint_xyz[i].Y, 0, wcsCircle.CamParams));
                        }
                        this.listPoint.Add((new userWcsPoint(wcsCircle.X, wcsCircle.Y, wcsCircle.Z, wcsCircle.Grab_x, wcsCircle.Grab_y, wcsCircle.CamParams)).GetPixPoint()); 
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
                        this.listPoint.Add(((userWcsPoint)e.DataContent).GetPixPoint()); // 添加Mark点
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
                //switch (e.Node.Tag.GetType().Name)
                //{
                //    case "CircleMeasure":
                //        if (!(this.drawObject is userDrawCircleMeasure))
                //        {
                //            this.drawObject.GrayValueInfo -= new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                //            this.drawObject.ClearDrawingObject();
                //            this.drawObject = new userDrawCircleMeasure(this.hWindowControl1, ((CircleMeasure)e.Node.Tag).FindCircle.CircleWcsPosition);
                //            this.drawObject.GrayValueInfo += new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                //        }
                //        this.drawObject.SetParam(((CircleMeasure)e.Node.Tag).FindCircle.CircleWcsPosition);
                //        this.drawObject.BackImage = ((CircleMeasure)e.Node.Tag).ImageData;
                //        this.drawObject.AttachDrawingObjectToWindow();
                //        this.metrolegyParamForm.drawObject = this.drawObject;
                //        this._currFunction = (CircleMeasure)e.Node.Tag;
                //        DisplayClickItem(sender, e);
                //        break;
                //    case "CircleSectorMeasure":
                //        if (!(this.drawObject is userDrawCircleSectorMeasure))
                //        {
                //            this.drawObject.GrayValueInfo -= new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                //            this.drawObject.ClearDrawingObject();
                //            this.drawObject = new userDrawCircleSectorMeasure(this.hWindowControl1, ((CircleSectorMeasure)e.Node.Tag).FindCircleSector.CircleSectorWcsPosition);
                //            this.drawObject.GrayValueInfo += new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                //        }
                //        this.drawObject.SetParam(((CircleSectorMeasure)e.Node.Tag).FindCircleSector.CircleSectorWcsPosition);
                //        this.drawObject.BackImage = ((CircleSectorMeasure)e.Node.Tag).ImageData;
                //        this.drawObject.AttachDrawingObjectToWindow();
                //        this.metrolegyParamForm.drawObject = this.drawObject;
                //        this._currFunction = (CircleSectorMeasure)e.Node.Tag;
                //        DisplayClickItem(sender, e);
                //        break;
                //    case "EllipseMeasure":
                //        if (!(this.drawObject is userDrawEllipseMeasure))
                //        {
                //            this.drawObject.GrayValueInfo -= new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                //            this.drawObject.ClearDrawingObject();
                //            this.drawObject = new userDrawEllipseMeasure(this.hWindowControl1, ((EllipseMeasure)e.Node.Tag).FindEllipse.EllipseWcsPosition);
                //            this.drawObject.GrayValueInfo += new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                //        }
                //        this.drawObject.SetParam(((EllipseMeasure)e.Node.Tag).FindEllipse.EllipseWcsPosition);
                //        this.drawObject.BackImage = ((EllipseMeasure)e.Node.Tag).ImageData;
                //        this.drawObject.AttachDrawingObjectToWindow();
                //        this.metrolegyParamForm.drawObject = this.drawObject;
                //        this._currFunction = (EllipseMeasure)e.Node.Tag;
                //        DisplayClickItem(sender, e);
                //        break;
                //    case "EllipseSectorMeasure":
                //        if (!(this.drawObject is userDrawEllipseSectorMeasure))
                //        {
                //            this.drawObject.GrayValueInfo -= new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                //            this.drawObject.ClearDrawingObject();
                //            this.drawObject = new userDrawEllipseSectorMeasure(this.hWindowControl1, ((EllipseSectorMeasure)e.Node.Tag).FindEllipseSector.EllipseSectorWcsPosition);
                //            this.drawObject.GrayValueInfo += new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                //        }
                //        this.drawObject.SetParam(((EllipseSectorMeasure)e.Node.Tag).FindEllipseSector.EllipseSectorWcsPosition);
                //        this.drawObject.BackImage = ((EllipseSectorMeasure)e.Node.Tag).ImageData;
                //        this.drawObject.AttachDrawingObjectToWindow();
                //        this.metrolegyParamForm.drawObject = this.drawObject;
                //        this._currFunction = (EllipseSectorMeasure)e.Node.Tag;
                //        DisplayClickItem(sender, e);
                //        break;
                //    case "LineMeasure":
                //        if (!(this.drawObject is userDrawLineMeasure))
                //        {
                //            this.drawObject.GrayValueInfo -= new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                //            this.drawObject.ClearDrawingObject();
                //            this.drawObject = new userDrawLineMeasure(this.hWindowControl1, ((LineMeasure)e.Node.Tag).FindLine.LineWcsPosition);
                //            this.drawObject.GrayValueInfo += new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                //        }
                //        this.drawObject.SetParam(((LineMeasure)e.Node.Tag).FindLine.LineWcsPosition);
                //        this.drawObject.BackImage = ((LineMeasure)e.Node.Tag).ImageData;
                //        this.drawObject.AttachDrawingObjectToWindow();
                //        this.metrolegyParamForm.drawObject = this.drawObject;
                //        this._currFunction = (LineMeasure)e.Node.Tag;
                //        DisplayClickItem(sender, e);
                //        break;
                //    case "PointMeasure":
                //        if (!(this.drawObject is userDrawPointMeasure))
                //        {
                //            this.drawObject.GrayValueInfo -= new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                //            this.drawObject.ClearDrawingObject();
                //            this.drawObject = new userDrawPointMeasure(this.hWindowControl1, ((PointMeasure)e.Node.Tag).FindPoint.LineWcsPosition);
                //            this.drawObject.GrayValueInfo += new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                //        }
                //        this.drawObject.SetParam(((PointMeasure)e.Node.Tag).FindPoint.LineWcsPosition);
                //        this.drawObject.BackImage = ((PointMeasure)e.Node.Tag).ImageData;
                //        this.drawObject.AttachDrawingObjectToWindow();
                //        this.metrolegyParamForm.drawObject = this.drawObject;
                //        this._currFunction = (PointMeasure)e.Node.Tag;
                //        DisplayClickItem(sender, e);
                //        break;
                //    case "Rectangle2Measure":
                //        if (!(this.drawObject is userDrawRect2Measure))
                //        {
                //            this.drawObject.GrayValueInfo -= new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                //            this.drawObject.ClearDrawingObject();
                //            this.drawObject = new userDrawRect2Measure(this.hWindowControl1, ((Rectangle2Measure)e.Node.Tag).FindRect2.Rect2WcsPosition);
                //            this.drawObject.GrayValueInfo += new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                //        }
                //        this.drawObject.SetParam(((Rectangle2Measure)e.Node.Tag).FindRect2.Rect2WcsPosition);
                //        this.drawObject.BackImage = ((Rectangle2Measure)e.Node.Tag).ImageData;
                //        this.drawObject.AttachDrawingObjectToWindow();
                //        this.metrolegyParamForm.drawObject = this.drawObject;
                //        this._currFunction = (Rectangle2Measure)e.Node.Tag;
                //        DisplayClickItem(sender, e);
                //        break;

                //    ///////////////////////////////////////// 显示测量距离对象
                //    case "CircleToCircleDist2D":
                //    case "CircleToLineDist2D":
                //    case "LineToLineDist2D":
                //    case "PointToLineDist2D":
                //        DisplayClickItem(sender, e);
                //        break;
                //    default:

                //        break;
                //}
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
        private void addHWindowControlContextMenu(HWindowControl hWindowControl)
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

        private void 标定执行button_Click(object sender, EventArgs e)
        {
            try
            {
                this.isStop = false;
                this.listData.Clear();
                this.运行label.Text += "-> 开始标定";
                double step = 0;
                double.TryParse(this.步长textBox.Text, out step);
                if (step == 0)
                    step = 1;
                this.listPoint.Clear();
                CoordSysAxisParam AxisParam = new CoordSysAxisParam();
                int count = (int)((this.CamParam.SlantCalibParam.EndCalibPoint.X - this.CamParam.SlantCalibParam.StartCaliPoint.X) / step);
                for (int i = 0; i < count; i++)
                {
                    Application.DoEvents();
                    switch (this.CamParam.SlantCalibParam.MoveAxis)
                    {
                        case enScanAxis.X轴:
                            if(this.CamParam.SlantCalibParam.CoordValueType ==  enCoordValueType.绝对坐标)
                            {
                                AxisParam = new CoordSysAxisParam(this.CamParam.SlantCalibParam.StartCaliPoint.X + step, this.CamParam.SlantCalibParam.StartCaliPoint.Y,
                                this.CamParam.SlantCalibParam.StartCaliPoint.Z, this.CamParam.SlantCalibParam.StartCaliPoint.Angle, 0, 0);
                            }
                            else
                            {
                                AxisParam = new CoordSysAxisParam(step, 0, 0, 0, 0, 0);
                            }
                            MotionCardManage.GetCard(this.CamParam.CaliParam.CoordSysName).MoveMultyAxis(this.CamParam.CaliParam.CoordSysName, enAxisName.X轴, 10, AxisParam);
                            break;
                        case enScanAxis.Y轴:
                            if (this.CamParam.SlantCalibParam.CoordValueType == enCoordValueType.绝对坐标)
                            {
                                AxisParam = new CoordSysAxisParam(this.CamParam.SlantCalibParam.StartCaliPoint.X , this.CamParam.SlantCalibParam.StartCaliPoint.Y + step,
                                this.CamParam.SlantCalibParam.StartCaliPoint.Z, this.CamParam.SlantCalibParam.StartCaliPoint.Angle, 0, 0);
                            }
                            else
                            {
                                AxisParam = new CoordSysAxisParam(step, 0, 0, 0, 0, 0);
                            }
                            MotionCardManage.GetCard(this.CamParam.CaliParam.CoordSysName).MoveMultyAxis(this.CamParam.CaliParam.CoordSysName, enAxisName.X轴, 10, AxisParam);
                            break;
                    }
                    if (this.isStop) break;
                    this._treeViewWrapClass.RunSyn(null, 1);
                }
                ////////////////////////////////////////////////////////////计算夹角 ///////////////
                double deg = 0;
                userPixLine pixLine;
                this.CalculateSlantAngle(this.listPoint, out deg, out pixLine); // 
                this.drawObject.AttachPropertyData.Add(pixLine);
                this.drawObject.AttachPropertyData.Add(new userTextLable(deg.ToString(), enColor.red.ToString()));
                this.角度textBox.Text = deg.ToString();
                ///////////////////////////////////////////////////////////
                this.运行label.Text += "-> 标定完成";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void 标定停止button_Click(object sender, EventArgs e)
        {
            try
            {
                this.isStop = true;
                this._treeViewWrapClass.Stop();
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
                double angle = 0;
                double.TryParse(this.角度textBox.Text, out angle);
                this.CamParam.SlantCalibParam.CamSlantDeg = angle;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void toolStrip1_ItemClicked_1(object sender, ToolStripItemClickedEventArgs e)
        {
            switch (e.ClickedItem.Text)
            {
                case "工具":
                    ToolForm tool = new ToolForm(this._treeViewWrapClass);
                    tool.Owner = this;
                    tool.Show();
                    break;
                case "执行":
                    标定执行button_Click(null, null);
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

        private void 坐标值类型comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                enCoordValueType coordValueType = enCoordValueType.绝对坐标;
                Enum.TryParse(this.坐标值类型comboBox.SelectedItem.ToString(), out coordValueType);
                this.CamParam.SlantCalibParam.CoordValueType = coordValueType;
            }
            catch
            {

            }
        }

        private void 检测工具button_Click(object sender, EventArgs e)
        {
            try
            {
                ToolForm tool = new ToolForm(this._treeViewWrapClass);
                tool.Owner = this;
                tool.Show();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }



    }
}

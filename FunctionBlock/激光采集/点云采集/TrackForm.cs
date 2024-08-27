using Common;
using HalconDotNet;
using MotionControlCard;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FunctionBlock
{
    public partial class MoveTrackForm : Form
    {
        private BindingList<AcqTrackParam> trackList = null;
        public MoveTrackForm()
        {
            InitializeComponent();
        }
        public MoveTrackForm(BindingList<AcqTrackParam> trackList)
        {
            InitializeComponent();
            this.trackList = trackList;
        }
        private void MoveTrackForm_Load(object sender, EventArgs e)
        {
            try
            {
                this.CoordCol.Items.Clear();
                this.CoordCol.ValueType = typeof(enCoordSysName);
                foreach (enCoordSysName temp in Enum.GetValues(typeof(enCoordSysName)))
                    this.CoordCol.Items.Add(temp);
                /////////////////////////
                this.AxisCol.Items.Clear();
                this.AxisCol.ValueType = typeof(enAxisName);
                foreach (enAxisName temp in Enum.GetValues(typeof(enAxisName)))
                    this.AxisCol.Items.Add(temp);
                ///////////////////////////////////////
                this.MoveCol.Items.Clear();
                this.MoveCol.ValueType = typeof(enMoveType);
                foreach (enMoveType temp in Enum.GetValues(typeof(enMoveType)))
                    this.MoveCol.Items.Add(temp);
                ///////////////////////////////////////
                this.FunctionCol.Items.Clear();
                this.FunctionCol.ValueType = typeof(enFunction);
                foreach (enFunction temp in Enum.GetValues(typeof(enFunction)))
                    this.FunctionCol.Items.Add(temp);


            }
            catch (Exception ex)
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

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            try
            {
                if (this.dataGridView1.Columns[e.ColumnIndex].DataPropertyName == "Track")
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


        #region 数据视图右键菜单项
        private void addDataGridViewContextMenu(DataGridView dataGridView)
        {
            ContextMenuStrip ContextMenuStrip1 = new ContextMenuStrip();
            // 添加右键菜单 
            ToolStripItem[] items = new ToolStripMenuItem[]
            {
                new ToolStripMenuItem("删除"),
                new ToolStripMenuItem("清空"),
                new ToolStripMenuItem("矩形阵列"),
                new ToolStripMenuItem("圆形阵列"),
                new ToolStripMenuItem("移动到选定位置"),
            };
            ContextMenuStrip1.Items.AddRange(items); // 在滑赋值前不能调用 
            ContextMenuStrip1.ItemClicked += new ToolStripItemClickedEventHandler(dataGridViewContextMenuStrip_ItemClicked);
            dataGridView.ContextMenuStrip = ContextMenuStrip1;
        }
        private void dataGridViewContextMenuStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            string name = e.ClickedItem.Text;
            int index = 0;
            try
            {
                ((ContextMenuStrip)sender).Close();
                switch (name)
                {
                    case "删除":
                        ((ContextMenuStrip)sender).Close();
                        if (this.dataGridView1.CurrentRow != null)
                            index = this.dataGridView1.CurrentRow.Index;
                        this.trackList.RemoveAt(index);
                        //if (this.drawObject.AttachPropertyData.Count > index)
                        //    this.drawObject.AttachPropertyData.RemoveAt(index);
                        //this.drawObject.DrawingGraphicObject();
                        for (int i = 0; i < this.dataGridView1.Rows.Count; i++)
                        {
                            if (this.dataGridView1.Rows.Count > i)
                                this.dataGridView1.Rows[i].HeaderCell.Value = (i + 1).ToString();
                        }
                        break;
                    //////////////////////////////////////
                    case "清空":
                        ((ContextMenuStrip)sender).Close();
                        //this.drawObject.AttachPropertyData.Clear();
                        //this._trackParam.Clear();
                        //this.drawObject.DrawingGraphicObject();
                        this.trackList?.Clear();
                        break;

                    case "矩形阵列":
                        if (this.dataGridView1.CurrentRow == null) return;
                        index = this.dataGridView1.CurrentRow.Index;
                        RectangleArrayDataForm rectform = new RectangleArrayDataForm();
                        rectform.Owner = this;
                        rectform.ShowDialog();
                        HHomMat2D hHomMat2D = new HHomMat2D();
                        HHomMat2D hHomMat_tras;
                        //////////////////////////////////////////
                        Task.Run(() =>
                        {
                            for (int i = 0; i < rectform.RowCount; i++)
                            {
                                for (int j = 0; j < rectform.ColCount; j++)
                                {
                                    if (i == 0 && j == 0) continue; //选定行不变
                                    hHomMat_tras = hHomMat2D.HomMat2dTranslate(rectform.OffsetX * j, rectform.OffsetY * i);
                                    switch (this.trackList[index].RoiShape.GetType().Name)
                                    {
                                        case nameof(drawWcsPoint):
                                            this.Invoke(new Action(() => { this.trackList.Add(new AcqTrackParam(((drawWcsPoint)this.trackList[index].RoiShape).AffineTransWcsPoint(hHomMat_tras), enMoveType.点位运动)); }));
                                            break;
                                        case nameof(drawWcsLine):
                                            this.Invoke(new Action(() => { this.trackList.Add(new AcqTrackParam(((drawWcsLine)this.trackList[index].RoiShape).AffineTransWcsLine(hHomMat_tras), enMoveType.直线运动)); }));
                                            break;
                                        case nameof(drawWcsCircle):
                                            this.Invoke(new Action(() => { this.trackList.Add(new AcqTrackParam(((drawWcsCircle)this.trackList[index].RoiShape).AffineTransWcsCircle(hHomMat_tras), enMoveType.圆运动)); }));
                                            break;
                                        case nameof(drawWcsEllipse):
                                            this.Invoke(new Action(() => { this.trackList.Add(new AcqTrackParam(((drawWcsEllipse)this.trackList[index].RoiShape).AffineTransWcsCircle(hHomMat_tras), enMoveType.椭圆运动)); }));
                                            break;
                                        case nameof(drawWcsRect1):
                                            this.Invoke(new Action(() => { this.trackList.Add(new AcqTrackParam(((drawWcsRect1)this.trackList[index].RoiShape).AffineTransWcsRect1(hHomMat_tras), enMoveType.矩形1运动)); }));
                                            break;
                                        case nameof(drawWcsRect2):
                                            this.Invoke(new Action(() => { this.trackList.Add(new AcqTrackParam(((drawWcsRect2)this.trackList[index].RoiShape).AffineTransWcsRect2(hHomMat_tras), enMoveType.矩形2运动)); }));
                                            break;
                                        case nameof(drawWcsPolygon):
                                            this.Invoke(new Action(() => { this.trackList.Add(new AcqTrackParam(((drawWcsPolygon)this.trackList[index].RoiShape).AffineTransWcsPolygon(hHomMat_tras), enMoveType.多边形运动)); }));
                                            break;
                                        case nameof(drawWcsPolyLine):
                                            this.Invoke(new Action(() => { this.trackList.Add(new AcqTrackParam(((drawWcsPolyLine)this.trackList[index].RoiShape).AffineTransWcsPolygon(hHomMat_tras), enMoveType.多段线)); }));
                                            break;
                                    }
                                    Thread.Sleep(100);
                                }
                            }
                        });
                        rectform.Close();
                        break;
                    case "圆形阵列":
                        index = this.dataGridView1.CurrentRow.Index;
                        if (index < 0) return;
                        CircleArrayDataForm circleForm = new CircleArrayDataForm();
                        circleForm.Owner = this;
                        circleForm.ShowDialog();
                        circleForm.Ref_X = 0; //this.currentParam.Center_X; // 赋值阵列中心
                        circleForm.Ref_Y = 0;// this.currentParam.Center_Y; // 赋值阵列中心
                        hHomMat2D = new HHomMat2D();
                        HHomMat2D hHomMat_Rota;
                        Task.Run(() =>
                        {
                            for (int i = 0; i < circleForm.ArrayNum; i++)
                            {
                                if (i == 0) continue; //选定点不变
                                // 以当前点为圆上的点来阵列
                                hHomMat_Rota = hHomMat2D.HomMat2dRotate(circleForm.Add_Deg * i * Math.PI / 180, circleForm.Ref_X, circleForm.Ref_Y);
                                switch (this.trackList[index].RoiShape.GetType().Name)
                                {
                                    case nameof(drawWcsPoint):
                                        this.Invoke(new Action(() => { this.trackList.Add(new AcqTrackParam(((drawWcsPoint)this.trackList[index].RoiShape).AffineTransWcsPoint(hHomMat_Rota), enMoveType.点位运动)); }));
                                        break;
                                    case nameof(drawWcsLine):
                                        this.Invoke(new Action(() => { this.trackList.Add(new AcqTrackParam(((drawWcsLine)this.trackList[index].RoiShape).AffineTransWcsLine(hHomMat_Rota), enMoveType.直线运动)); }));
                                        break;
                                    case nameof(drawWcsCircle):
                                        this.Invoke(new Action(() => { this.trackList.Add(new AcqTrackParam(((drawWcsCircle)this.trackList[index].RoiShape).AffineTransWcsCircle(hHomMat_Rota), enMoveType.圆运动)); }));
                                        break;
                                    case nameof(drawWcsEllipse):
                                        this.Invoke(new Action(() => { this.trackList.Add(new AcqTrackParam(((drawWcsEllipse)this.trackList[index].RoiShape).AffineTransWcsCircle(hHomMat_Rota), enMoveType.椭圆运动)); }));
                                        break;
                                    case nameof(drawWcsRect1):
                                        this.Invoke(new Action(() => { this.trackList.Add(new AcqTrackParam(((drawWcsRect1)this.trackList[index].RoiShape).AffineTransWcsRect1(hHomMat_Rota), enMoveType.矩形1运动)); }));
                                        break;
                                    case nameof(drawWcsRect2):
                                        this.Invoke(new Action(() => { this.trackList.Add(new AcqTrackParam(((drawWcsRect2)this.trackList[index].RoiShape).AffineTransWcsRect2(hHomMat_Rota), enMoveType.矩形2运动)); }));
                                        break;
                                    case nameof(drawWcsPolygon):
                                        this.Invoke(new Action(() => { this.trackList.Add(new AcqTrackParam(((drawWcsPolygon)this.trackList[index].RoiShape).AffineTransWcsPolygon(hHomMat_Rota), enMoveType.多边形运动)); }));
                                        break;
                                    case nameof(drawWcsPolyLine):
                                        this.Invoke(new Action(() => { this.trackList.Add(new AcqTrackParam(((drawWcsPolyLine)this.trackList[index].RoiShape).AffineTransWcsPolygon(hHomMat_Rota), enMoveType.多段线)); }));
                                        break;
                                }
                            }
                        });
                        break;
                    case "移动到选定位置":
                        index = this.dataGridView1.CurrentRow.Index;
                        if (index < 0 || index > this.trackList.Count) return;
                        MoveCommandParam CommandParam = new MoveCommandParam();
                        IMotionControl _Card = MotionCardManage.GetCard(this.trackList[index].CoordSysName);
                        RobotJawParam jaw = RobotJawParaManager.Instance.GetJawParam(nameof(enRobotJawEnum.Jaw1));
                        switch (this.trackList[index].MoveType)
                        {
                            case enMoveType.点位运动:
                                drawWcsPoint wcsPoint = this.trackList[index].RoiShape as drawWcsPoint;
                                if (wcsPoint == null)
                                    throw new ArgumentException("指定的轨迹类型不为点!");
                                CommandParam.MoveType = this.trackList[index].MoveType;
                                CommandParam.MoveSpeed = GlobalVariable.pConfig.MoveSpeed;
                                CommandParam.MoveAxis = enAxisName.XY轴;
                                CommandParam.CoordSysName = this.trackList[index].CoordSysName;      // 运动坐标系里是否需要包含移动指令
                                CommandParam.AxisParam = new CoordSysAxisParam(wcsPoint.X + jaw.X, wcsPoint.Y + jaw.Y, 0, 0, 0, 0);
                                CommandParam.AxisParam2 = new CoordSysAxisParam(wcsPoint.X + jaw.X, wcsPoint.Y + jaw.Y, 0, 0, 0, 0);
                                ////////////////////////////////////////////////////////////
                                _Card?.MoveMultyAxis(CommandParam);
                                break;
                            case enMoveType.直线运动:
                                drawWcsLine wcsLinbe = this.trackList[index].RoiShape as drawWcsLine;
                                if (wcsLinbe == null)
                                    throw new ArgumentException("指定的轨迹类型不为点!");
                                CommandParam.MoveType = this.trackList[index].MoveType;
                                CommandParam.MoveSpeed = GlobalVariable.pConfig.MoveSpeed;
                                CommandParam.MoveAxis = enAxisName.XY轴;
                                CommandParam.CoordSysName = this.trackList[index].CoordSysName;      // 运动坐标系里是否需要包含移动指令
                                CommandParam.AxisParam = new CoordSysAxisParam(wcsLinbe.X1 + jaw.X, wcsLinbe.Y1 + jaw.Y, 0, 0, 0, 0);
                                CommandParam.AxisParam2 = new CoordSysAxisParam(wcsLinbe.X1 + jaw.X, wcsLinbe.Y1 + jaw.Y, 0, 0, 0, 0);
                                ////////////////////////////////////////////////////////////
                                _Card?.MoveMultyAxis(CommandParam);
                                break;
                        }
                        break;
                    ///////////////////////////////////////////////
                    default:
                        break;
                }
            }
            catch
            {
            }
        }




        #endregion

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    int index = 0;
                    //PixROI pixShape;
                    //this.addContextMenu(this.hWindowControl1);
                    switch (dataGridView1.Columns[e.ColumnIndex].Name)
                    {
                        case "TeachCol":
                            //this.ClearContextMenu(this.hWindowControl1);
                            if (this.dataGridView1.Rows[e.RowIndex].DataBoundItem == null)
                            {
                                MessageBox.Show("未设置必需的图形参数，请先设置轨迹类型参数!");
                                return;
                            }
                            switch (this.trackList[e.RowIndex].MoveType)
                            {
                                case enMoveType.矩形2运动:
                                    //this.drawObject.AttachPropertyData.Clear();
                                    //if (!(this.drawObject is userDrawRect2ROI))
                                    //{
                                    //    this.drawObject.GrayValueInfo -= new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                                    //    this.drawObject.ClearDrawingObject();
                                    //    this.drawObject = new userDrawRect2ROI(this.hWindowControl1, false);
                                    //    this.drawObject.GrayValueInfo += new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                                    //}
                                    //this.drawObject.IsLiveState = true;
                                    break;
                                case enMoveType.矩形1运动:
                                    //this.drawObject.AttachPropertyData.Clear();
                                    //if (!(this.drawObject is userDrawRect1ROI))
                                    //{
                                    //    this.drawObject.GrayValueInfo -= new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                                    //    this.drawObject.ClearDrawingObject();
                                    //    this.drawObject = new userDrawRect1ROI(this.hWindowControl1, false);
                                    //    this.drawObject.GrayValueInfo += new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                                    //}
                                    //this.drawObject.IsLiveState = true;
                                    break;
                                case enMoveType.圆运动:
                                    //this.drawObject.AttachPropertyData.Clear();
                                    //if (!(this.drawObject is userDrawCircleROI))
                                    //{
                                    //    this.drawObject.GrayValueInfo -= new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                                    //    this.drawObject.ClearDrawingObject();
                                    //    this.drawObject = new userDrawCircleROI(this.hWindowControl1, false);
                                    //    this.drawObject.GrayValueInfo += new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                                    //}
                                    //this.drawObject.IsLiveState = true;
                                    break;
                                case enMoveType.椭圆运动:
                                    //this.drawObject.AttachPropertyData.Clear();
                                    //if (!(this.drawObject is userDrawEllipseROI))
                                    //{
                                    //    this.drawObject.GrayValueInfo -= new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                                    //    this.drawObject.ClearDrawingObject();
                                    //    this.drawObject = new userDrawEllipseROI(this.hWindowControl1, false);
                                    //    this.drawObject.GrayValueInfo += new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                                    //}
                                    //this.drawObject.IsLiveState = true;
                                    break;
                                case enMoveType.多边形运动:
                                    //this.drawObject.AttachPropertyData.Clear();
                                    //if (!(this.drawObject is userDrawPolygonROI))
                                    //{
                                    //    this.drawObject.GrayValueInfo -= new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                                    //    this.drawObject.ClearDrawingObject();
                                    //    this.drawObject = new userDrawPolygonROI(this.hWindowControl1, false);
                                    //    this.drawObject.GrayValueInfo += new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                                    //}
                                    //this.drawObject.IsLiveState = true;
                                    break;
                                case enMoveType.点位运动:
                                    //this.drawObject.AttachPropertyData.Clear();
                                    //if (!(this.drawObject is userDrawPointROI))
                                    //{
                                    //    this.drawObject.GrayValueInfo -= new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                                    //    this.drawObject.ClearDrawingObject();
                                    //    this.drawObject = new userDrawPointROI(this.hWindowControl1, false);
                                    //    this.drawObject.GrayValueInfo += new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                                    //}
                                    //this.drawObject.IsLiveState = true;
                                    break;
                                case enMoveType.直线运动:
                                    //this.drawObject.AttachPropertyData.Clear();
                                    //if (!(this.drawObject is userDrawLineROI))
                                    //{
                                    //    this.drawObject.GrayValueInfo -= new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                                    //    this.drawObject.ClearDrawingObject();
                                    //    this.drawObject = new userDrawLineROI(this.hWindowControl1, false);
                                    //    this.drawObject.GrayValueInfo += new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                                    //}
                                    //this.drawObject.IsLiveState = true;
                                    break;
                                default:
                                    throw new NotImplementedException(this.trackList[e.RowIndex].MoveType.ToString() + "未实现!");
                            }
                            //////////////////////////
                            foreach (var item in this.trackList)
                            {
                                //if (index != e.RowIndex && item.RoiShape != null)
                                //{
                                //    this.drawObject.AddViewObject(new ViewData(item.RoiShape.GetPixROI(this.drawObject.CameraParam).GetXLD(), enColor.orange.ToString()));
                                //}
                                index++;
                            }
                            //this.drawObject.IsLiveState = true;
                            //if (this.drawObject.BackImage == null)
                            //{
                            //    this.acqSource = AcqSourceManage.Instance.GetCamAcqSource(this._viewConfigParam.CamName);
                            //    if (this.acqSource != null)
                            //    {
                            //        switch (this.acqSource.Sensor.ConfigParam.SensorType)
                            //        {
                            //            case enUserSensorType.面阵相机:
                            //            case enUserSensorType.线阵相机:
                            //                Dictionary<enDataItem, object> data = this.acqSource.AcqImageData(null);
                            //                this.drawObject.BackImage = data[enDataItem.Image] as ImageDataClass;
                            //                break;
                            //        }
                            //    }
                            //    else
                            //    {
                            //        AcqSource _camSource = AcqSourceManage.Instance.GetAcqSource(this.传感器comboBox1.SelectedItem.ToString());
                            //        this.drawObject.BackImage = new ImageDataClass(this.sourceImage, _camSource?.Sensor.CameraParam);
                            //    }
                            //}
                            /////////////////////////////////////////////////////////////////////////
                            //if (this.trackList[e.RowIndex].RoiShape == null)
                            //    this.drawObject.SetParam(null);
                            //else
                            //{
                            //    this.drawObject.SetParam(this.wcsCoordSystem);
                            //    this.drawObject.SetParam(this._trackParam[e.RowIndex].RoiShape);
                            //}
                            //this.drawObject.DrawPixRoiShapeOnWindow(enColor.red, out pixShape);
                            //this.drawObject.AddViewObject(new ViewData(pixShape.GetXLD(), enColor.orange.ToString()));
                            ///////////////////////////////////////////////////////////
                            //this.trackList[e.RowIndex].RoiShape = pixShape.GetWcsROI(this.drawObject.CameraParam);
                            //////////////////////////////////////////////////////////
                            for (int i = 0; i < this.dataGridView1.Rows.Count; i++)
                            {
                                this.dataGridView1.Rows[i].HeaderCell.Value = (i + 1).ToString();
                            }
                            break;
                        case "DeletCol":
                            if (this.trackList.Count > e.RowIndex)
                                this.trackList.RemoveAt(e.RowIndex);
                            //if (this.drawObject.AttachPropertyData.Count > e.RowIndex)
                            //    this.drawObject.AttachPropertyData.RemoveAt(e.RowIndex);
                            //this.drawObject.DrawingGraphicObject();
                            for (int i = 0; i < this.dataGridView1.Rows.Count; i++)
                            {
                                if (this.dataGridView1.Rows.Count > i)
                                    this.dataGridView1.Rows[i].HeaderCell.Value = (i + 1).ToString();
                            }
                            break;
                        case "TrackCol":
                            //this.drawObject.AttachPropertyData.Clear();
                            //foreach (var item in this._trackParam)
                            //{
                            //    if (item.RoiShape == null) return;
                            //    if (index == e.RowIndex)
                            //    {
                            //        this.drawObject.AddViewObject(new ViewData(item.RoiShape.AffineTransWcsROI(new HHomMat2D(this.wcsCoordSystem?.GetVariationHomMat2D())).GetPixROI(this.drawObject.CameraParam).GetXLD(), enColor.green.ToString()));
                            //    }
                            //    else
                            //    {
                            //        this.drawObject.AddViewObject(new ViewData(item.RoiShape.AffineTransWcsROI(new HHomMat2D(this.wcsCoordSystem?.GetVariationHomMat2D())).GetPixROI(this.drawObject.CameraParam).GetXLD(), enColor.orange.ToString()));
                            //    }
                            //    index++;
                            //}
                            //this.drawObject.DrawingGraphicObject();
                            break;
                        case "AddCol":
                            this.trackList.Add(new AcqTrackParam());
                            break;
                        case "加减速Col":

                            break;
                    }
                    this.dataGridView1.Refresh();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}


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
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using View;


namespace FunctionBlock
{
    public partial class TrackMoveForm : Form
    {
        private CancellationTokenSource cts;
        private Form form;
        private IFunction _function;
        private bool isFormClose = false;
        BindingList<TrackParam> trackList = null;
        public TrackMoveForm(IFunction function)
        {
            this._function = function;
            InitializeComponent();
            trackList = ((TrackMove)this._function).TrackList;
        }
        private void PointMoveForm_Load(object sender, EventArgs e)
        {
            BindProperty();
            this.addDataGridViewContextMenu(this.dataGridView1);
        }
        private void BindProperty()
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

                this.dataGridView1.DataSource = this.trackList;
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
                        this.cts = new CancellationTokenSource();
                        Task.Run(() =>
                        {
                            if (this._function.Execute(null).Succss)
                            {
                                if (!this.cts.IsCancellationRequested)
                                {
                                    this.Invoke(new Action(() =>
                                    {
                                        this.toolStripStatusLabel1.Text = "执行结果:";
                                        this.toolStripStatusLabel2.Text = "成功";
                                        this.toolStripStatusLabel2.ForeColor = Color.Green;
                                    }));
                                }
                            }
                            else
                            {
                                if (!this.cts.IsCancellationRequested)
                                {
                                    this.Invoke(new Action(() =>
                                   {
                                       this.toolStripStatusLabel1.Text = "执行结果:";
                                       this.toolStripStatusLabel2.Text = "失败";
                                       this.toolStripStatusLabel2.ForeColor = Color.Red;
                                   }));
                                }
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





        private void PointMoveForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (cts != null)
                    cts.Cancel();
            }
            catch
            {

            }
        }


        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    if (this.trackList == null) return;
                    switch (this.dataGridView1.Columns[e.ColumnIndex].Name)
                    {
                        case "TeachCol":
                            if (this.dataGridView1.Rows[e.RowIndex].DataBoundItem == null)
                            {
                                MessageBox.Show("未设置必需的图形参数，请先设置轨迹类型参数!");
                                return;
                            }
                            switch (this.trackList[e.RowIndex].MoveType)
                            {
                                case enMoveType.矩形2运动:
                                    if (this.trackList[e.RowIndex].RoiShape == null || !(this.trackList[e.RowIndex].RoiShape is drawWcsRect2))
                                        this.trackList[e.RowIndex].RoiShape = new drawWcsRect2();
                                    /////////////////////////////////////////////////////////////////
                                    new Rect2ParamForm(this.trackList[e.RowIndex]).Show();
                                    break;
                                case enMoveType.矩形1运动:
                                    throw new NotImplementedException(this.trackList[e.RowIndex].MoveType.ToString() + "未实现!");
                                    //break;
                                case enMoveType.圆运动:
                                    if (this.trackList[e.RowIndex].RoiShape == null || !(this.trackList[e.RowIndex].RoiShape is drawWcsCircle))
                                        this.trackList[e.RowIndex].RoiShape = new drawWcsCircle();
                                    /////////////////////////////////////////////////////////////////
                                    new CircleParamForm(this.trackList[e.RowIndex]).Show();
                                    break;
                                case enMoveType.椭圆运动:
                                    throw new NotImplementedException(this.trackList[e.RowIndex].MoveType.ToString() + "未实现!");
                                    //break;
                                case enMoveType.多边形运动:
                                    throw new NotImplementedException(this.trackList[e.RowIndex].MoveType.ToString() + "未实现!");
                                    //break;
                                case enMoveType.点位运动:
                                    if (this.trackList[e.RowIndex].RoiShape == null || !(this.trackList[e.RowIndex].RoiShape is drawWcsPoint))
                                        this.trackList[e.RowIndex].RoiShape = new drawWcsPoint();
                                    new PointParamForm(this.trackList[e.RowIndex]).Show();
                                    break;
                                case enMoveType.直线运动:
                                    if (this.trackList[e.RowIndex].RoiShape == null || !(this.trackList[e.RowIndex].RoiShape is drawWcsLine))
                                        this.trackList[e.RowIndex].RoiShape = new drawWcsLine();
                                    new LineParamForm(this.trackList[e.RowIndex]).Show();
                                    break;
                                default:
                                    throw new NotImplementedException(this.trackList[e.RowIndex].MoveType.ToString() + "未实现!");
                            }
                            break;
                        case "DeletCol":
                            if (this.trackList.Count > e.RowIndex)
                                this.trackList.RemoveAt(e.RowIndex);
                            for (int i = 0; i < this.dataGridView1.Rows.Count; i++)
                            {
                                if (this.dataGridView1.Rows.Count > i)
                                    this.dataGridView1.Rows[i].HeaderCell.Value = (i + 1).ToString();
                            }
                            break;
                        case "TrackCol":

                            break;
                        case "AddCol":
                            //this.dataGridView1.Rows.Add();
                            TrackParam param = new TrackParam(new drawWcsPoint());
                            this.trackList.Add(param);
                            //////////////////////////////////////////////////////////
                            for (int i = 0; i < this.dataGridView1.Rows.Count; i++)
                            {
                                this.dataGridView1.Rows[i].HeaderCell.Value = (i + 1).ToString();
                            }
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
                        for (int i = 0; i < this.dataGridView1.Rows.Count; i++)
                        {
                            if (this.dataGridView1.Rows.Count > i)
                                this.dataGridView1.Rows[i].HeaderCell.Value = (i + 1).ToString();
                        }
                        break;
                    //////////////////////////////////////
                    case "清空":
                        ((ContextMenuStrip)sender).Close();
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
                                            this.Invoke(new Action(() => { this.trackList.Add(new TrackParam(((drawWcsPoint)this.trackList[index].RoiShape).AffineTransWcsPoint(hHomMat_tras), enMoveType.点位运动)); }));
                                            break;
                                        case nameof(drawWcsLine):
                                            this.Invoke(new Action(() => { this.trackList.Add(new TrackParam(((drawWcsLine)this.trackList[index].RoiShape).AffineTransWcsLine(hHomMat_tras), enMoveType.直线运动)); }));
                                            break;
                                        case nameof(drawWcsCircle):
                                            this.Invoke(new Action(() => { this.trackList.Add(new TrackParam(((drawWcsCircle)this.trackList[index].RoiShape).AffineTransWcsCircle(hHomMat_tras), enMoveType.圆运动)); }));
                                            break;
                                        case nameof(drawWcsEllipse):
                                            this.Invoke(new Action(() => { this.trackList.Add(new TrackParam(((drawWcsEllipse)this.trackList[index].RoiShape).AffineTransWcsCircle(hHomMat_tras), enMoveType.椭圆运动)); }));
                                            break;
                                        case nameof(drawWcsRect1):
                                            this.Invoke(new Action(() => { this.trackList.Add(new TrackParam(((drawWcsRect1)this.trackList[index].RoiShape).AffineTransWcsRect1(hHomMat_tras), enMoveType.矩形1运动)); }));
                                            break;
                                        case nameof(drawWcsRect2):
                                            this.Invoke(new Action(() => { this.trackList.Add(new TrackParam(((drawWcsRect2)this.trackList[index].RoiShape).AffineTransWcsRect2(hHomMat_tras), enMoveType.矩形2运动)); }));
                                            break;
                                        case nameof(drawWcsPolygon):
                                            this.Invoke(new Action(() => { this.trackList.Add(new TrackParam(((drawWcsPolygon)this.trackList[index].RoiShape).AffineTransWcsPolygon(hHomMat_tras), enMoveType.多边形运动)); }));
                                            break;
                                        case nameof(drawWcsPolyLine):
                                            this.Invoke(new Action(() => { this.trackList.Add(new TrackParam(((drawWcsPolyLine)this.trackList[index].RoiShape).AffineTransWcsPolygon(hHomMat_tras), enMoveType.多段线)); }));
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
                                        this.Invoke(new Action(() => { this.trackList.Add(new TrackParam(((drawWcsPoint)this.trackList[index].RoiShape).AffineTransWcsPoint(hHomMat_Rota), enMoveType.点位运动)); }));
                                        break;
                                    case nameof(drawWcsLine):
                                        this.Invoke(new Action(() => { this.trackList.Add(new TrackParam(((drawWcsLine)this.trackList[index].RoiShape).AffineTransWcsLine(hHomMat_Rota), enMoveType.直线运动)); }));
                                        break;
                                    case nameof(drawWcsCircle):
                                        this.Invoke(new Action(() => { this.trackList.Add(new TrackParam(((drawWcsCircle)this.trackList[index].RoiShape).AffineTransWcsCircle(hHomMat_Rota), enMoveType.圆运动)); }));
                                        break;
                                    case nameof(drawWcsEllipse):
                                        this.Invoke(new Action(() => { this.trackList.Add(new TrackParam(((drawWcsEllipse)this.trackList[index].RoiShape).AffineTransWcsCircle(hHomMat_Rota), enMoveType.椭圆运动)); }));
                                        break;
                                    case nameof(drawWcsRect1):
                                        this.Invoke(new Action(() => { this.trackList.Add(new TrackParam(((drawWcsRect1)this.trackList[index].RoiShape).AffineTransWcsRect1(hHomMat_Rota), enMoveType.矩形1运动)); }));
                                        break;
                                    case nameof(drawWcsRect2):
                                        this.Invoke(new Action(() => { this.trackList.Add(new TrackParam(((drawWcsRect2)this.trackList[index].RoiShape).AffineTransWcsRect2(hHomMat_Rota), enMoveType.矩形2运动)); }));
                                        break;
                                    case nameof(drawWcsPolygon):
                                        this.Invoke(new Action(() => { this.trackList.Add(new TrackParam(((drawWcsPolygon)this.trackList[index].RoiShape).AffineTransWcsPolygon(hHomMat_Rota), enMoveType.多边形运动)); }));
                                        break;
                                    case nameof(drawWcsPolyLine):
                                        this.Invoke(new Action(() => { this.trackList.Add(new TrackParam(((drawWcsPolyLine)this.trackList[index].RoiShape).AffineTransWcsPolygon(hHomMat_Rota), enMoveType.多段线)); }));
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
                if (this.dataGridView1.Columns[e.ColumnIndex].DataPropertyName == "RoiShape"  || this.dataGridView1.Columns[e.ColumnIndex].DataPropertyName == "AccDecParam")
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




    }
}

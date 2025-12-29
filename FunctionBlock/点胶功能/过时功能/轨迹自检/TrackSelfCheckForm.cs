using AlgorithmsLibrary;
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
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Forms;
using View;

namespace FunctionBlock
{
    public partial class TrackSelfCheckForm : Form
    {
        private Form form;
        private IFunction _function;
        private VisualizeView drawObject;
        private TreeNode _refNode;
        private HObjectModel3D hObjectModel3D1 = null, hObjectModel3D2 = null;
        public TrackSelfCheckForm(TreeNode node)
        {
            InitializeComponent();
            this._refNode = node;
            this._function = node.Tag as IFunction;
            this._function.SetPropertyValues(nameof(TreeNode), this._refNode);
            this.Text = this._function.GetPropertyValues("名称").ToString();
            this.drawObject = new VisualizeView(this.hWindowControl1, false);
            new ListBoxWrapClass().InitListBox(this.listBox1, node);
            //new ListBoxWrapClass().InitListBox(this.listBox2, node, 2);
        }
        private void TrackSelfCheckForm_Load(object sender, EventArgs e)
        {
            this.drawObject.GrayValueInfo += new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
            //BaseFunction.ExcuteCompleted += new ExcuteCompletedEventHandler(DisplayExcuteResult);
            //////////////////////////////////////
            BindProperty();
        }
        private void BindProperty()
        {
            try
            {
                TrackSelfCheckParam param = ((TrackSelfCheck)this._function).SelfCheckParam;
                this.显示条目comboBox.DataSource = Enum.GetNames(typeof(enShowItemSelfCheck));
                this.变换类型comboBox.DataSource = Enum.GetNames(typeof(enTransformationType));
                this.变换类型comboBox.DataBindings.Add(nameof(this.变换类型comboBox.Text), param, nameof(param.TransformationType), true, DataSourceUpdateMode.OnPropertyChanged);
                this.起始点百分比textBox.DataBindings.Add(nameof(this.起始点百分比textBox.Text), param, nameof(param.StartPercent), true, DataSourceUpdateMode.OnPropertyChanged);
                this.结束点百分比textBox.DataBindings.Add(nameof(this.结束点百分比textBox.Text), param, nameof(param.EndPercent), true, DataSourceUpdateMode.OnPropertyChanged);
                /////////////////////////
            }
            catch (Exception ex)
            {
                new Common.UserMessageForm(ex.ToString()).ShowDialog();
            }
        }
        public enum enShowItemSelfCheck
        {
            输入轨迹,
            标准轨迹,
            匹配轨迹,
            变换轨迹,
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

        private void DisplayExcuteResult(object sender, ExcuteCompletedEventArgs e)
        {
            if (e.DataContent != null)
            {
                this.drawObject.ClearViewObject(); // 更新图像时清空
                switch (e.DataContent.GetType().Name)
                {
                    case nameof(ImageDataClass):
                        this.drawObject.BackImage = (ImageDataClass)e.DataContent;
                        break;
                    case nameof(userWcsRectangle2):
                        userWcsRectangle2 wcsRect2 = (userWcsRectangle2)e.DataContent;
                        this.drawObject.AddViewObject(new ViewData((wcsRect2).GetPixRectangle2().GetXLD(), wcsRect2.Color.ToString()));
                        break;
                    case nameof(userWcsRectangle1):
                        userWcsRectangle1 wcsRect1 = (userWcsRectangle1)e.DataContent;
                        this.drawObject.AddViewObject(new ViewData((wcsRect1).GetPixRectangle1().GetXLD(), wcsRect1.Color.ToString()));
                        break;
                    case nameof(userWcsPoint):
                        userWcsPoint wcsPoint = (userWcsPoint)e.DataContent;
                        this.drawObject.AddViewObject(new ViewData((wcsPoint).GetPixPoint(), wcsPoint.Color.ToString()));
                        break;
                    case nameof(userWcsVector):
                        userWcsVector wcsVector = (userWcsVector)e.DataContent;
                        userWcsPoint wcsPoint1 = new userWcsPoint(wcsVector.X, wcsVector.Y, wcsVector.Z, wcsVector.CamParams);
                        this.drawObject.AddViewObject(new ViewData((wcsPoint1).GetPixPoint(), wcsPoint1.Color.ToString()));
                        break;
                    case nameof(userWcsLine):
                        userWcsLine wcsLine = (userWcsLine)e.DataContent;
                        this.drawObject.AddViewObject(new ViewData((wcsLine).GetPixLine().GetXLD(), wcsLine.Color.ToString()));
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
                    case nameof(userWcsPolyLine):
                        userWcsPolyLine wcsPolyLine = (userWcsPolyLine)e.DataContent;
                        this.drawObject.AddViewObject(new ViewData((wcsPolyLine).GetPixPolyLine().GetXLD(), wcsPolyLine.Color.ToString()));
                        break;
                    case nameof(userPixPoint):
                        this.drawObject.AddViewObject(new ViewData(((userPixPoint)e.DataContent).GetXLD(), ((userPixPoint)e.DataContent).Color.ToString()));
                        break;
                    case nameof(userPixLine):
                        this.drawObject.AddViewObject(new ViewData(((userPixLine)e.DataContent).GetXLD(), ((userPixLine)e.DataContent).Color.ToString()));
                        break;
                    case nameof(userPixCircle):
                        this.drawObject.AddViewObject(new ViewData(((userPixCircle)e.DataContent).GetXLD(), ((userPixCircle)e.DataContent).Color.ToString()));
                        break;
                    case nameof(userPixCircleSector):
                        this.drawObject.AddViewObject(new ViewData(((userPixCircleSector)e.DataContent).GetXLD(), ((userPixCircleSector)e.DataContent).Color.ToString()));
                        break;
                    case nameof(userPixEllipse):
                        this.drawObject.AddViewObject(new ViewData(((userPixEllipse)e.DataContent).GetXLD(), ((userPixEllipse)e.DataContent).Color.ToString()));
                        break;
                    case nameof(userPixEllipseSector):
                        this.drawObject.AddViewObject(new ViewData(((userPixEllipseSector)e.DataContent).GetXLD(), ((userPixEllipseSector)e.DataContent).Color.ToString()));
                        break;
                    case nameof(userPixRectangle1):
                        this.drawObject.AddViewObject(new ViewData(((userPixRectangle1)e.DataContent).GetXLD(), ((userPixRectangle1)e.DataContent).Color.ToString()));
                        break;
                    case nameof(userPixRectangle2):
                        this.drawObject.AddViewObject(new ViewData(((userPixRectangle2)e.DataContent).GetXLD(), ((userPixRectangle2)e.DataContent).Color.ToString()));
                        break;
                    case nameof(userOkNgText):
                        this.drawObject.AddViewObject(new ViewData(e.DataContent, "red"));
                        break;
                    case nameof(userTextLable):
                        this.drawObject.AddViewObject(new ViewData(e.DataContent, "red"));
                        break;
                    default:
                        this.drawObject.AddViewObject(new ViewData(e.DataContent, "red"));
                        break;
                }
            }
        }

        public void 运行toolStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            try
            {
                switch (e.ClickedItem.Name)
                {
                    case nameof(this.toolStripButton_Run):
                    case "执行":
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
                new Common.UserMessageForm(ex.ToString()).ShowDialog();
            }
        }


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




        private void TrackSelfCheckForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (this.drawObject != null)
                    this.drawObject.ClearDrawingObject();
                //((FunctionBlock.PointMeasure)_function).FindPoint.MetrolegyComplete -= new MetrolegyCompletedEventHandler(this.MetrolegyCompleted);
                this.drawObject.GrayValueInfo -= new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                //ListBoxWrapClass.ItemsChangeToForm -= new ItemsChangeEventHandler(listbox_ItemsChange);
                //BaseFunction.ExcuteCompleted -= new ExcuteCompletedEventHandler(DisplayExcuteResult);
                this.hObjectModel3D1?.ClearObjectModel3d();
                this.hObjectModel3D2?.ClearObjectModel3d();
                this.drawObject.PointCloudModel3D?.ClearObjectModel3d();
            }
            catch
            {

            }
        }

        private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false;
        }

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

        private void 显示条目comboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            try
            {
                switch (this.显示条目comboBox.Text.Trim())
                {
                    case nameof(enShowItemSelfCheck.输入轨迹):
                        this.drawObject.PointCloudModel3D?.ClearObjectModel3d();
                        userWcsPoint[] wcsPoints = ((TrackSelfCheck)this._function).CurTrackPoint;
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
                    case nameof(enShowItemSelfCheck.标准轨迹):
                        this.drawObject.PointCloudModel3D?.ClearObjectModel3d();
                        wcsPoints = ((TrackSelfCheck)this._function).StdTrackPoint;
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
                    case "输出对象":
                    case "输出轨迹":
                    case nameof(enShowItemSelfCheck.匹配轨迹):
                        this.drawObject.PointCloudModel3D?.ClearObjectModel3d();
                        wcsPoints = ((TrackSelfCheck)this._function).StdTrackPoint; // 基准轨迹
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
                        userWcsPolyLine wcsPolyLine = ((TrackSelfCheck)this._function).WcsPolyLine;
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
                        this.dataGridView1.Rows.Clear();
                        if (((TrackSelfCheck)this._function).DataError != null)
                        {
                            foreach (var item in ((TrackSelfCheck)this._function).DataError)
                            {
                                int index = this.dataGridView1.Rows.Add(item);
                                this.dataGridView1.Rows[index].HeaderCell.Value = (index + 1).ToString();
                            }
                        }
                        break;

                    case nameof(enShowItemSelfCheck.变换轨迹):
                        this.drawObject.PointCloudModel3D?.ClearObjectModel3d();
                        /////////////////////////////////////////////////////////////////////////
                        wcsPolyLine = ((TrackSelfCheck)this._function).WcsPolyLine;
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
                        this.drawObject.PointCloudModel3D = new PointCloudData(new HObjectModel3D[] {hObjectModel3D2 });
                        break;
                }
            }
            catch (Exception ex)
            {
               new Common.UserMessageForm(ex.ToString()).ShowDialog();;
            }
        }

        private void 导入基准轮廓Btn_Click(object sender, EventArgs e)
        {
            try
            {
                if (new UserMessageForm().ShowDialog("确定要更新基准轮廓吗?", "更新基准轮廓") == DialogResult.Cancel) return;
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
                                ((TrackSelfCheck)this._function).StdTrackPoint = new userWcsPoint[Data1.Length];
                                for (int i = 0; i < Data1.Length; i++)
                                {
                                    Data3[i] = 0;
                                    ((TrackSelfCheck)this._function).StdTrackPoint[i] = new userWcsPoint(Data1[i], Data2[i], 0);
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
                                ((TrackSelfCheck)this._function).StdTrackPoint = new userWcsPoint[list_x.Count];
                                for (int i = 0; i < list_x.Count; i++)
                                {
                                    ((TrackSelfCheck)this._function).StdTrackPoint[i] = new userWcsPoint(list_x[i], list_y[i], 0);
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
               new Common.UserMessageForm(ex.ToString()).ShowDialog();;
            }
        }

        private void 注册基准轮廓Btn_Click(object sender, EventArgs e)
        {
            try
            {
                if (new UserMessageForm().ShowDialog("确定要注册基准轮廓吗?", "注册基准轮廓") == DialogResult.Cancel) return;
                userWcsPoint[] wcsPoints = ((TrackSelfCheck)this._function).CurTrackPoint;
                if (wcsPoints != null && wcsPoints.Length > 0)
                {
                    List<double> list_x = new List<double>();
                    List<double> list_y = new List<double>();
                    List<double> list_z = new List<double>();
                    ((TrackSelfCheck)this._function).StdTrackPoint = new userWcsPoint[wcsPoints.Length];
                    for (int i = 0; i < wcsPoints.Length; i++)
                    {
                        list_x.Add(wcsPoints[i].X);
                        list_y.Add(wcsPoints[i].Y);
                        list_z.Add(wcsPoints[i].Z);
                        ((TrackSelfCheck)this._function).StdTrackPoint[i] = new userWcsPoint(wcsPoints[i].X, wcsPoints[i].Y, wcsPoints[i].Z);
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
               new Common.UserMessageForm(ex.ToString()).ShowDialog();;
            }
        }


        //#region 数据视图右键菜单项
        //private void addDataGridViewContextMenu(DataGridView dataGridView)
        //{
        //    ContextMenuStrip ContextMenuStrip1 = new ContextMenuStrip();
        //    // 添加右键菜单 
        //    ToolStripItem[] items = null;
        //    switch (SystemParamManager.Instance.SysConfigParam.Language)
        //    {
        //        default:
        //        case "zh-CN":
        //            items = new ToolStripMenuItem[]
        //            {
        //            new ToolStripMenuItem("移动到当前位",null,null,"移动到当前位"),
        //            new ToolStripMenuItem("移动到捨取位",null,null,"移动到捨取位"),
        //            new ToolStripMenuItem("删除",null,null,"删除"),
        //            new ToolStripMenuItem("清空",null,null,"清空"),
        //            };
        //            break;
        //        case "en-US":
        //            items = new ToolStripMenuItem[]
        //            {

        //            };
        //            break;
        //    }
        //    ContextMenuStrip1.Items.AddRange(items); // 在滑赋值前不能调用 
        //    ContextMenuStrip1.ItemClicked += new ToolStripItemClickedEventHandler(dataGridViewContextMenuStrip_ItemClicked);
        //    dataGridView.ContextMenuStrip = ContextMenuStrip1;
        //}

        //private void dataGridViewContextMenuStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        //{
        //    string name = e.ClickedItem.Name;
        //    try
        //    {
        //        BindingList<userWcsTrackPoint> listShape = ((TrackPick)this._function).TrackPoint;
        //        enCoordSysName coordSysName = enCoordSysName.CoordSys_0;// AcqSourceManage.Instance.GetAcqSource(this.相机采集源comboBox.SelectedItem.ToString()).CoordSysName;
        //        IMotionControl motion = MotionCardManage.GetCard(coordSysName);
        //        ((ContextMenuStrip)sender).Close();
        //        switch (name)
        //        {
        //            case "移动到当前位":
        //                motion.MoveMultyAxis(coordSysName, enAxisName.XYZ轴, 10, new CoordSysAxisPosParam
        //                    (
        //                    listShape[this.dataGridView1.CurrentRow.Index].X,
        //                    listShape[this.dataGridView1.CurrentRow.Index].Y,
        //                    listShape[this.dataGridView1.CurrentRow.Index].Z,
        //                    listShape[this.dataGridView1.CurrentRow.Index].Theta,
        //                    listShape[this.dataGridView1.CurrentRow.Index].U,
        //                    listShape[this.dataGridView1.CurrentRow.Index].V
        //                    ));
        //                break;
        //            case "移动到捨取位":
        //                motion.MoveMultyAxis(coordSysName, enAxisName.XYZ轴, 10, new CoordSysAxisPosParam
        //                    (
        //                    listShape[this.dataGridView1.CurrentRow.Index].Grab_x,
        //                    listShape[this.dataGridView1.CurrentRow.Index].Grab_y,
        //                    listShape[this.dataGridView1.CurrentRow.Index].Grab_z,
        //                    listShape[this.dataGridView1.CurrentRow.Index].Theta,
        //                    listShape[this.dataGridView1.CurrentRow.Index].U,
        //                    listShape[this.dataGridView1.CurrentRow.Index].V
        //                    ));
        //                break;
        //            case "删除":
        //                if (listShape.Count > this.dataGridView1.CurrentRow.Index)
        //                    listShape.RemoveAt(this.dataGridView1.CurrentRow.Index);
        //                break;
        //            case "清空":
        //                listShape.Clear();
        //                break;
        //            default:
        //                break;
        //        }
        //    }
        //    catch
        //    {
        //    }
        //}



        //#endregion


    }
}

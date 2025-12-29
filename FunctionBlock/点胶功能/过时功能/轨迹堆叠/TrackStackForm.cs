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
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using View;

namespace FunctionBlock
{
    public partial class TrackStackForm : Form
    {
        private Form form;
        private IFunction _function;
        private userDrawPointROI drawObject;
        private TreeNode _refNode;
        public TrackStackForm(TreeNode node)
        {
            this._refNode = node;
            this._function = node.Tag as IFunction;
            InitializeComponent();
            this.Text = this._function.GetPropertyValues("名称").ToString();
            this.drawObject = new userDrawPointROI(this.hWindowControl1, false);
            new ListBoxWrapClass().InitListBox(this.listBox1, node);
        }
        private void TrackTransForm_Load(object sender, EventArgs e)
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
                this.堆叠距离comBox.DataBindings.Add(nameof(this.堆叠距离comBox.SelectedItem), ((TrackStack)this._function).StackParam, "Dist", true, DataSourceUpdateMode.OnPropertyChanged);
                this.堆叠数量comBox.DataBindings.Add(nameof(this.堆叠数量comBox.SelectedItem), ((TrackStack)this._function).StackParam, "Count", true, DataSourceUpdateMode.OnPropertyChanged);
                /////////////////////////
                //this.dataGridView1.DataSource = ((TrackPull)this._function).TrackPoint;
                //this.addDataGridViewContextMenu(this.dataGridView1);
            }
            catch (Exception ex)
            {
                new Common.UserMessageForm(ex.ToString()).ShowDialog();
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
                            if (this._function.Execute(this.drawObject.GetPixPointParam(), this._refNode).Succss)
                            {
                                this.Invoke(new Action(() =>
                                {
                                    this.toolStripStatusLabel1.Text = "执行结果:";
                                    this.toolStripStatusLabel2.Text = "成功";
                                    this.toolStripStatusLabel2.ForeColor = Color.Green;
                                    this.drawObject.DetachDrawingObjectFromWindow();
                                    this.drawObject.AddViewObject(new ViewData(((FunctionBlock.ManualPointMeasure)this._function).WcsPoint.GetPixPoint().GetXLD()));
                                }));
                            }
                            else
                            {
                                this.Invoke(new Action(() =>
                                {
                                    this.toolStripStatusLabel1.Text = "执行结果:";
                                    this.toolStripStatusLabel2.Text = "失败";
                                    this.toolStripStatusLabel2.ForeColor = Color.Red;
                                    this.drawObject.DetachDrawingObjectFromWindow();
                                    this.drawObject.ClearViewObject();
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




        private void FindLineForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (this.drawObject != null)
                    this.drawObject.ClearDrawingObject();
                //((FunctionBlock.PointMeasure)_function).FindPoint.MetrolegyComplete -= new MetrolegyCompletedEventHandler(this.MetrolegyCompleted);
                this.drawObject.GrayValueInfo -= new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                //ListBoxWrapClass.ItemsChangeToForm -= new ItemsChangeEventHandler(listbox_ItemsChange);
                //BaseFunction.ExcuteCompleted -= new ExcuteCompletedEventHandler(DisplayExcuteResult);
            }
            catch
            {

            }
        }

        private void 结果dataGridView_DataError(object sender, DataGridViewDataErrorEventArgs e)
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






        #region 数据视图右键菜单项
        private void addDataGridViewContextMenu(DataGridView dataGridView)
        {
            ContextMenuStrip ContextMenuStrip1 = new ContextMenuStrip();
            // 添加右键菜单 
            ToolStripItem[] items = null;
            switch (SystemParamManager.Instance.SysConfigParam.Language)
            {
                default:
                case "zh-CN":
                    items = new ToolStripMenuItem[]
                    {
                    new ToolStripMenuItem("移动到当前位",null,null,"移动到当前位"),
                    new ToolStripMenuItem("移动到捨取位",null,null,"移动到捨取位"),
                    new ToolStripMenuItem("删除",null,null,"删除"),
                    new ToolStripMenuItem("清空",null,null,"清空"),
                    };
                    break;
                case "en-US":
                    items = new ToolStripMenuItem[]
                    {

                    };
                    break;
            }
            ContextMenuStrip1.Items.AddRange(items); // 在滑赋值前不能调用 
            ContextMenuStrip1.ItemClicked += new ToolStripItemClickedEventHandler(dataGridViewContextMenuStrip_ItemClicked);
            dataGridView.ContextMenuStrip = ContextMenuStrip1;
        }
        private void dataGridViewContextMenuStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            string name = e.ClickedItem.Name;
            try
            {
                BindingList<userWcsTrackPoint> listShape = ((ContourPick)this._function).TrackPoint;
                enCoordSysName coordSysName = enCoordSysName.CoordSys_0;// AcqSourceManage.Instance.GetAcqSource(this.相机采集源comboBox.SelectedItem.ToString()).CoordSysName;
                IMotionControl motion = MotionCardManage.GetCard(coordSysName);
                ((ContextMenuStrip)sender).Close();
                switch (name)
                {
                    case "移动到当前位":
                        motion.MoveMultyAxis(coordSysName, enAxisName.XYZ轴, 10, new CoordSysAxisPosParam
                            (
                            listShape[this.dataGridView1.CurrentRow.Index].X,
                            listShape[this.dataGridView1.CurrentRow.Index].Y,
                            listShape[this.dataGridView1.CurrentRow.Index].Z,
                            listShape[this.dataGridView1.CurrentRow.Index].Theta,
                            listShape[this.dataGridView1.CurrentRow.Index].U,
                            listShape[this.dataGridView1.CurrentRow.Index].V
                            ));
                        break;
                    case "移动到捨取位":
                        motion.MoveMultyAxis(coordSysName, enAxisName.XYZ轴, 10, new CoordSysAxisPosParam
                            (
                            listShape[this.dataGridView1.CurrentRow.Index].Grab_x,
                            listShape[this.dataGridView1.CurrentRow.Index].Grab_y,
                            listShape[this.dataGridView1.CurrentRow.Index].Grab_z,
                            listShape[this.dataGridView1.CurrentRow.Index].Theta,
                            listShape[this.dataGridView1.CurrentRow.Index].U,
                            listShape[this.dataGridView1.CurrentRow.Index].V
                            ));
                        break;
                    case "删除":
                        if (listShape.Count > this.dataGridView1.CurrentRow.Index)
                            listShape.RemoveAt(this.dataGridView1.CurrentRow.Index);
                        break;
                    case "清空":
                        listShape.Clear();
                        break;
                    default:
                        break;
                }
            }
            catch
            {
            }
        }
        #endregion



    }
}

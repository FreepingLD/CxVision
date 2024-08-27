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
    public partial class LineMeasureForm : Form
    {
        private CancellationTokenSource cts; // 提供了标准的协作式取消
        private object _objectDataModel;
        private Form form;
        private IFunction _function;
        private userDrawLineMeasure drawObject;
        private ImageViewForm viewForm;
        private TreeNode _refNode;
        private MetrolegyParamForm metrolegyParam;
        public LineMeasureForm(IFunction function)
        {
            this._function = function;
            InitializeComponent();
            this.Text = function.GetPropertyValues("名称").ToString();
            this.drawObject = new userDrawLineMeasure(this.hWindowControl1, ((FunctionBlock.LineMeasure)_function).FindLine.LinePixPosition, ((FunctionBlock.LineMeasure)_function).PixCoordSystem);
            new ListBoxWrapClass().InitListBox(this.listBox1, function);
            new ListBoxWrapClass().InitListBox(this.listBox2, function, 2);
        }
        public LineMeasureForm(IFunction function, TreeNode node)
        {
            this._refNode = node;
            this._function = function;
            InitializeComponent();
            this.Text = node.Text; //function.GetPropertyValues("名称").ToString();
            this.drawObject = new userDrawLineMeasure(this.hWindowControl1, ((FunctionBlock.LineMeasure)_function).FindLine.LinePixPosition, ((FunctionBlock.LineMeasure)_function).PixCoordSystem);
            this.metrolegyParam = new MetrolegyParamForm(this._function, this.drawObject);
            new ListBoxWrapClass().InitListBox(this.listBox1, node);
            new ListBoxWrapClass().InitListBox(this.listBox2, node, 2);
        }
        private void LineMeasureForm_Load(object sender, EventArgs e)
        {
            ((FunctionBlock.LineMeasure)_function).FindLine.MetrolegyComplete += new MetrolegyCompletedEventHandler(this.MetrolegyCompleted);
            ///////////////
            this.drawObject.BackImage = ((LineMeasure)_function).ImageData;
            BindProperty();
            //this.AddForm(this.参数panel, new MetrolegyParamForm(this._function, this.drawObject));
            this.AddForm(this.参数panel, this.metrolegyParam);
            this.drawObject.GrayValueInfo += new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
            //this.viewForm = new ImageViewForm(this._function, ((LineMeasure)_function).ImageData);
            //this.AddForm(this.视图panel, this.viewForm);
        }
        private void BindProperty()
        {
            try
            {
                this.相机采集源comboBox.DataSource = FunctionBlock.AcqSourceManage.Instance.GetAcqSourceName(); // SensorManage.CameraList;
                this.相机采集源comboBox.DataBindings.Add(nameof(this.相机采集源comboBox.SelectedItem), ((FunctionBlock.LineMeasure)this._function), "AcqSourceName", true, DataSourceUpdateMode.OnPropertyChanged);
                ///////////////////
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
        private void MetrolegyCompleted(MetrolegyCompletedEventArgs e)
        {
            if (e.EdgeData == null) return;
            switch (e.EdgeData.GetType().Name)
            {
                case "userWcsRectangle2":
                    this.drawObject.AttachPropertyData.Clear();
                    userWcsRectangle2 wcsRect2 = (userWcsRectangle2)e.EdgeData;
                    this.drawObject.AttachPropertyData.Add(wcsRect2);
                    if (e.EdgePoint_xyz != null)
                    {
                        for (int i = 0; i < e.EdgePoint_xyz.Length; i++)
                        {
                            this.drawObject.AttachPropertyData.Add(e.EdgePoint_xyz[i]);
                        }
                    }
                    this.drawObject.DetachDrawingObjectFromWindow();
                    break;
                case "userWcsRectangle1":
                    this.drawObject.AttachPropertyData.Clear();
                    userWcsRectangle1 wcsRect1 = (userWcsRectangle1)e.EdgeData;
                    this.drawObject.AttachPropertyData.Add(wcsRect1);
                    for (int i = 0; i < e.EdgePoint_xyz.Length; i++)
                    {
                        this.drawObject.AttachPropertyData.Add(e.EdgePoint_xyz[i]);
                    }
                    this.drawObject.DetachDrawingObjectFromWindow();
                    break;
                case "userWcsPoint":
                    this.drawObject.AttachPropertyData.Clear();
                    userWcsPoint wcsPoint = (userWcsPoint)e.EdgeData;
                    this.drawObject.AttachPropertyData.Add(wcsPoint);
                    if (e.EdgePoint_xyz != null)
                    {
                        for (int i = 0; i < e.EdgePoint_xyz.Length; i++)
                        {
                            this.drawObject.AttachPropertyData.Add(e.EdgePoint_xyz[i]);
                        }
                    }
                    this.drawObject.DetachDrawingObjectFromWindow();
                    break;
                case "userWcsLine":
                    this.drawObject.AttachPropertyData.Clear();
                    userWcsLine wcsLine = (userWcsLine)e.EdgeData;
                    this.drawObject.AttachPropertyData.Add(wcsLine);
                    if (e.EdgePoint_xyz != null)
                    {
                        for (int i = 0; i < e.EdgePoint_xyz.Length; i++)
                        {
                            this.drawObject.AttachPropertyData.Add(e.EdgePoint_xyz[i]);
                        }
                    }
                    this.drawObject.DetachDrawingObjectFromWindow();
                    break;
                case "userWcsCircle":
                    this.drawObject.AttachPropertyData.Clear();
                    userWcsCircle wcsCircle = (userWcsCircle)e.EdgeData;
                    this.drawObject.AttachPropertyData.Add(wcsCircle);
                    if (e.EdgePoint_xyz != null)
                    {
                        for (int i = 0; i < e.EdgePoint_xyz.Length; i++)
                        {
                            this.drawObject.AttachPropertyData.Add(e.EdgePoint_xyz[i]);
                        }
                    }
                    this.drawObject.DetachDrawingObjectFromWindow();
                    break;
                case "userWcsCircleSector":
                    this.drawObject.AttachPropertyData.Clear();
                    userWcsCircleSector wcsCircleSector = (userWcsCircleSector)e.EdgeData;
                    this.drawObject.AttachPropertyData.Add(wcsCircleSector);
                    if (e.EdgePoint_xyz != null)
                    {
                        for (int i = 0; i < e.EdgePoint_xyz.Length; i++)
                        {
                            this.drawObject.AttachPropertyData.Add(e.EdgePoint_xyz[i]);
                        }
                    }
                    this.drawObject.DetachDrawingObjectFromWindow();
                    break;
                case "userWcsEllipse":
                    this.drawObject.AttachPropertyData.Clear();
                    userWcsEllipse wcsEllipse = (userWcsEllipse)e.EdgeData;
                    this.drawObject.AttachPropertyData.Add(wcsEllipse);
                    if (e.EdgePoint_xyz != null)
                    {
                        for (int i = 0; i < e.EdgePoint_xyz.Length; i++)
                        {
                            this.drawObject.AttachPropertyData.Add(e.EdgePoint_xyz[i]);
                        }
                    }
                    this.drawObject.DetachDrawingObjectFromWindow();
                    break;
                case "userWcsEllipseSector":
                    this.drawObject.AttachPropertyData.Clear();
                    userWcsEllipseSector wcsEllipseSector = (userWcsEllipseSector)e.EdgeData;
                    this.drawObject.AttachPropertyData.Add(wcsEllipseSector);
                    if (e.EdgePoint_xyz != null)
                    {
                        for (int i = 0; i < e.EdgePoint_xyz.Length; i++)
                        {
                            this.drawObject.AttachPropertyData.Add(e.EdgePoint_xyz[i]);
                        }
                    }
                    this.drawObject.DetachDrawingObjectFromWindow();
                    break;
            }
        }
        private void UpdataMeasureRegion(object send, EventArgs e)
        {
            this.drawObject.DrawingGraphicObject();
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
                            if (this._function.Execute(this.drawObject.GetPixLineParam()).Succss)
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
        private void 示教点位button_Click(object sender, EventArgs e)
        {
            try
            {
                this.drawObject?.AttachPropertyData.Clear();
                this.drawObject?.ClearWindow();
                this.drawObject?.SetParam(((LineMeasure)this._refNode.Tag).PixCoordSystem);
                this.drawObject?.SetParam(((LineMeasure)this._refNode.Tag).FindLine.LinePixPosition.AffinePixLine2D(((LineMeasure)this._refNode.Tag).PixCoordSystem?.GetVariationHomMat2D()));
                this.drawObject.BackImage = ((LineMeasure)this._refNode.Tag).ImageData != null ? ((LineMeasure)this._refNode.Tag).ImageData : null;
                this.drawObject?.AttachDrawingObjectToWindow();
            }
            catch (Exception ex)
            {
                MessageBox.Show("示教失败！" + ex.ToString());
            }
        }
        private void FindLineForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (this.drawObject != null)
                    this.drawObject.ClearDrawingObject();
                if (cts != null)
                    cts.Cancel();
                this.metrolegyParam?.Close();
                this.drawObject.GrayValueInfo -= new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                ((FunctionBlock.LineMeasure)_function).FindLine.MetrolegyComplete -= new MetrolegyCompletedEventHandler(this.MetrolegyCompleted);
            }
            catch
            {

            }
        }
        private void 结果dataGridView_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false;
        }


    }
}

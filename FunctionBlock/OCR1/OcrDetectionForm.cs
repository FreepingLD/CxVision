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
using System.Threading.Tasks;
using System.Windows.Forms;
using View;

namespace FunctionBlock
{
    public partial class OcrDetectionForm : Form
    {
        private object _objectDataModel;
        private Form form;
        private IFunction _function;
        private DrawRect2 drawObject;
        private TreeNode node;
        public OcrDetectionForm(IFunction function, TreeNode node)
        {
            this._function = function;
            this.node = node;
            InitializeComponent();
            this.Text = function.GetPropertyValues("名称").ToString();
            this.drawObject = new DrawRect2(this.hWindowControl1, false, ((FunctionBlock.DoOcr)this._function).OcrCharDetection.OcrParam.ChartRegion);
            initHWindowControlEvent();
            new ListBoxWrapClass().InitListBox(this.listBox1, node);
        }
        private void OcrDetectionForm_Load(object sender, EventArgs e)
        {
            //////////////////////////////////////
            this.drawObject.BackImage = ((FunctionBlock.DoOcr)_function).ImageData;
            BindProperty();
        }
        private void BindProperty()
        {
            try
            {
                this.propertyGrid1.SelectedObject = ((FunctionBlock.DoOcr)this._function).OcrCharDetection.OcrParam;
                this.Ocr模型comboBox.DataSource = Enum.GetNames(typeof(FunctionBlock.enOcrModel));
                this.Ocr模型comboBox.DataBindings.Add("Text", ((FunctionBlock.DoOcr)this._function).OcrCharDetection, "OcrModel", true, DataSourceUpdateMode.OnPropertyChanged);
                this.字符区域dataGridView.DataSource = ((FunctionBlock.DoOcr)this._function).OcrCharDetection.OcrParam.ChartRegion;
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
                            if (this._function.Execute(this.node))
                            {
                                this.Invoke(new Action(() =>
                                {
                                    this.toolStripStatusLabel1.Text = "执行结果:";
                                    this.toolStripStatusLabel2.Text = "成功";
                                    this.toolStripStatusLabel2.ForeColor = Color.Green;
                                }));
                                //this.drawObject.DetachDrawingObjectFromWindow();
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


        // 更新3D对象模型 ；响应测量完成/及响应鼠标点击事件
        private void initHWindowControlEvent()
        {
            this.hWindowControl1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.hWindowControl1_MouseMove);
        }

        // 获取鼠标位置处的高度值
        private void hWindowControl1_MouseMove(object sender, MouseEventArgs e)
        {
            HTuple value = -1.0;
            HalconLibrary ha = new HalconLibrary();
            if (this._objectDataModel == null) return;
            try
            {
                switch (this._objectDataModel.GetType().Name)
                {
                    case "HImage": //HObject
                        ha.GetGrayValueOnWindow(this.hWindowControl1.HalconWindow, (HObject)this._objectDataModel, e.Y, e.X, out value);
                        break;
                    case "ImageDataClass": //HObject
                        ha.GetGrayValueOnWindow(this.hWindowControl1.HalconWindow, ((ImageDataClass)this._objectDataModel).Image, e.Y, e.X, out value);
                        break;
                }
                if (value.Length > 1)
                {
                    this.灰度值1Label.Text = value[2].D.ToString();
                    this.灰度值2Label.Text = 0.ToString();
                    this.灰度值2Label.Text = 0.ToString();
                    this.行坐标Label.Text = value[0].D.ToString();
                    this.列坐标Label.Text = value[1].D.ToString();
                }
            }
            catch
            {

            }
        }


        private void OcrDetectionForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (this.drawObject != null)
                    this.drawObject.ClearDrawingObject();
            }
            catch
            {

            }
        }

        private void 结果dataGridView_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false;
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            try
            {
                double row, column, phi, length1, length2;
                double row1, column1, row2, column2;
                int butn;
                this.hWindowControl1.HalconWindow.SetColor("red");
                //this.hWindowControl1.HalconWindow.DrawRectangle1(out row1, out column1, out row2, out column2);
                //this.hWindowControl1.HalconWindow.DrawRectangle2(out row, out column, out phi, out length1, out length2);
                this.hWindowControl1.HalconWindow.GetMbuttonSubPix(out row, out column, out butn);
                ((FunctionBlock.DoOcr)this._function).OcrCharDetection.OcrParam.ChartRegion.Add(new userPixRect2(row, column, 0, 50, 50));
                //((FunctionBlock.DoOcr)this._function).OcrChar.OcrParam.ChartRegion.Add(new userPixRect2((row1 + row2)*0.5, (column1 + column2) *0.5, 0, Math.Abs((column1 - column2) * 0.5), Math.Abs((row1 - row2) * 0.5)));
                this.drawObject.SetParam(((FunctionBlock.DoOcr)this._function).OcrCharDetection.OcrParam.ChartRegion.Last());
                this.drawObject.AttachDrawingObjectToWindow();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void DeletButton_Click(object sender, EventArgs e)
        {
            try
            {
                int index = this.字符区域dataGridView.CurrentRow.Index;
                ((FunctionBlock.DoOcr)this._function).OcrCharDetection.OcrParam.ChartRegion.RemoveAt(index);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void InsertButton_Click(object sender, EventArgs e)
        {
            try
            {
                int index = this.字符区域dataGridView.CurrentRow.Index;
                ((FunctionBlock.DoOcr)this._function).OcrCharDetection.OcrParam.ChartRegion.Insert(index, new userPixRect2(100, 100, 0, 50, 20));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void ClearButton_Click(object sender, EventArgs e)
        {
            try
            {
                ((FunctionBlock.DoOcr)this._function).OcrCharDetection.OcrParam.ChartRegion.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void 字符区域dataGridView_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false;
        }

        private void 字符区域dataGridView_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.字符区域dataGridView.SelectedRows == null) return;
                if (this.字符区域dataGridView.CurrentRow == null) return;
                int index = this.字符区域dataGridView.CurrentRow.Index;
                this.drawObject.SetParam(((FunctionBlock.DoOcr)this._function).OcrCharDetection.OcrParam.ChartRegion[index]);
                this.drawObject.AttachDrawingObjectToWindow();
            }
            catch
            {

            }
        }

        private void Ocr模型comboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            try
            {
                if (this.Ocr模型comboBox.SelectedIndex == -1) return;
                switch(this.Ocr模型comboBox.SelectedItem.ToString())
                {
                    case nameof(enOcrModel.OcrCNN):
                        ((FunctionBlock.DoOcr)this._function).OcrCharDetection.OcrParam = new OcrCnnParam();
                        this.propertyGrid1.SelectedObject = ((FunctionBlock.DoOcr)this._function).OcrCharDetection.OcrParam;
                        this.字符区域dataGridView.DataSource = ((FunctionBlock.DoOcr)this._function).OcrCharDetection.OcrParam.ChartRegion;
                        break;
                    case nameof(enOcrModel.OcrKNN):
                        ((FunctionBlock.DoOcr)this._function).OcrCharDetection.OcrParam = new OcrKnnParam();
                        this.propertyGrid1.SelectedObject = ((FunctionBlock.DoOcr)this._function).OcrCharDetection.OcrParam;
                        this.字符区域dataGridView.DataSource = ((FunctionBlock.DoOcr)this._function).OcrCharDetection.OcrParam.ChartRegion;
                        break;
                    case nameof(enOcrModel.OcrSVM):
                        ((FunctionBlock.DoOcr)this._function).OcrCharDetection.OcrParam = new OcrSvmParam();
                        this.propertyGrid1.SelectedObject = ((FunctionBlock.DoOcr)this._function).OcrCharDetection.OcrParam;
                        this.字符区域dataGridView.DataSource = ((FunctionBlock.DoOcr)this._function).OcrCharDetection.OcrParam.ChartRegion;
                        break;
                    case nameof(enOcrModel.OcrMLP):
                        ((FunctionBlock.DoOcr)this._function).OcrCharDetection.OcrParam = new OcrMlpParam();
                        this.propertyGrid1.SelectedObject = ((FunctionBlock.DoOcr)this._function).OcrCharDetection.OcrParam;
                        this.字符区域dataGridView.DataSource = ((FunctionBlock.DoOcr)this._function).OcrCharDetection.OcrParam.ChartRegion;
                        break;
                    case nameof(enOcrModel.OcrTextMode):
                        ((FunctionBlock.DoOcr)this._function).OcrCharDetection.OcrParam = new OcrTextModelParam();
                        this.propertyGrid1.SelectedObject = ((FunctionBlock.DoOcr)this._function).OcrCharDetection.OcrParam;
                        this.字符区域dataGridView.DataSource = ((FunctionBlock.DoOcr)this._function).OcrCharDetection.OcrParam.ChartRegion;
                        break;
                }           
            }
            catch
            {

            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {                
                HRegion hRegion = null, threRegion = null, operateRegion = null, selectRegion = null, connectionRegion = null, unionRegion = null,
                    sortRegion = null;
                HImage reduceImage = null,invertImage;
                HImage hImage = ((FunctionBlock.DoOcr)this._function).ImageData?.Image;
                Ocr ocr = ((FunctionBlock.DoOcr)this._function).OcrCharDetection;
                OcrParam ocrParam = ((FunctionBlock.DoOcr)this._function).OcrCharDetection.OcrParam;
                switch (this.comboBox1.SelectedItem.ToString())
                {
                    case "源图像":
                        this.drawObject.BackImage= ((FunctionBlock.DoOcr)this._function).ImageData;
                        break;
                    case "取反图像":
                        this.drawObject.BackImage = ((FunctionBlock.DoOcr)this._function).ImageData.InvertImage();
                        break;
                    case "字符区域":
                        hRegion = new HRegion();
                        hRegion.GenEmptyObj();
                        foreach (var item in ocrParam.ChartRegion)
                        {
                            HRegion hRegion1 = new HRegion();
                            hRegion1.GenRectangle2(item.Row, item.Col, item.Rad, item.Length1, item.Length2);
                            hRegion = hRegion.ConcatObj(hRegion1);
                            hRegion1.Dispose();
                        }
                        unionRegion = hRegion.Union1();
                        invertImage = hImage.InvertImage();
                        reduceImage = invertImage.ReduceDomain(unionRegion);
                        threRegion = reduceImage.Threshold(ocrParam.MinThreshold, ocrParam.MaxThreshold);
                        switch (ocrParam.RegionOperate)
                        {
                            default:
                            case enRegionOperate.NONE:
                                operateRegion = threRegion;
                                break;
                            case enRegionOperate.closing_rectangle1:
                                operateRegion = threRegion.ClosingRectangle1(ocrParam.MaskWidth, ocrParam.MaskHeight);
                                break;
                            case enRegionOperate.opening_rectangle1:
                                operateRegion = threRegion.OpeningRectangle1(ocrParam.MaskWidth, ocrParam.MaskHeight);
                                break;
                        }
                        connectionRegion = operateRegion.Connection();
                        selectRegion = connectionRegion.SelectShape("area", "and", ocrParam.MinArea, ocrParam.MaxArea);
                        ///////////////////////////////
                        this.drawObject.AttachEdgesPropertyData.Clear();
                        this.drawObject.AttachEdgesPropertyData.Add(selectRegion);
                        this.drawObject.DrawingGraphicObject();
                        unionRegion?.Dispose();
                        invertImage?.Dispose();
                        threRegion?.Dispose();
                        reduceImage?.Dispose();
                        break;
                }
            }
            catch
            {

            }
        }
    }
}

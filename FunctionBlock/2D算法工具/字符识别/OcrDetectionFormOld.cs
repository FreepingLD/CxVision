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
    public partial class OcrDetectionFormOld : Form
    {
        private object _objectDataModel;
        private Form form;
        private IFunction _function;
        private VisualizeView drawObject;
        private TreeNode node;
        public OcrDetectionFormOld(IFunction function, TreeNode node)
        {
            this._function = function;
            this.node = node;
            InitializeComponent();
            this.Text = function.GetPropertyValues("名称").ToString();
            this.drawObject = new userDrawRect2ROI(this.hWindowControl1, false);
            new ListBoxWrapClass().InitListBox(this.listBox1, node);
        }
        private void OcrDetectionForm_Load(object sender, EventArgs e)
        {
            //////////////////////////////////////
            this.drawObject.BackImage = ((FunctionBlock.Ocr)_function).ImageData;
            this.drawObject.GrayValueInfo += new GrayValueInfoEventHandler(GetGrayValueInfo);
            BindProperty();
        }
        private void BindProperty()
        {
            try
            {
                this.propertyGrid1.SelectedObject = ((FunctionBlock.Ocr)this._function).CharDetection.OcrParam;
                this.Ocr模型comboBox.DataSource = Enum.GetNames(typeof(FunctionBlock.enOcrModel));
                this.Ocr模型comboBox.DataBindings.Add("Text", ((FunctionBlock.Ocr)this._function).CharDetection, "OcrModel", true, DataSourceUpdateMode.OnPropertyChanged);
                //this.dataGridView1.DataSource = ((Ocr)this._function).CharDetection.OcrParam.ChartRegion;
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
                        Task.Run(() =>
                        {
                            if (this._function.Execute(this.node).Succss)
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

        private void OcrDetectionForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (this.drawObject != null)
                    this.drawObject.ClearDrawingObject();
                this.drawObject.GrayValueInfo -= new GrayValueInfoEventHandler(GetGrayValueInfo);
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
                //userPixRectangle2 pixRecrt2;
                //this.drawObject.SetParam(null);
                //this.drawObject.DrawPixRect2OnWindow(enColor.red, out pixRecrt2);
                //this.drawObject.DrawPixRoiShapeOnWindow(enColor.red, out pixRecrt2);
                //((FunctionBlock.DoOcr)this._function).OcrCharDetection.OcrParam.ChartRegion.Add(new drawPixRect2(pixRecrt2.Row, pixRecrt2.Col, pixRecrt2.Rad, pixRecrt2.Length1, pixRecrt2.Length2));
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
                int index = this.dataGridView1.CurrentRow.Index;
                //((FunctionBlock.Ocr)this._function).CharDetection.OcrParam.ChartRegion.RemoveAt(index);
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

        private void GetGrayValueInfo(object sender, GrayValueInfoEventArgs e)
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

        private void Ocr模型comboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            try
            {
                if (this.Ocr模型comboBox.SelectedIndex == -1) return;
                switch (this.Ocr模型comboBox.SelectedItem.ToString())
                {
                    case nameof(enOcrModel.OcrCNN):
                        ((FunctionBlock.Ocr)this._function).CharDetection.OcrParam = new OcrCnnParam();
                        this.propertyGrid1.SelectedObject = ((FunctionBlock.Ocr)this._function).CharDetection.OcrParam;
                        //this.dataGridView1.DataSource = ((FunctionBlock.Ocr)this._function).CharDetection.OcrParam.ChartRegion;
                        break;
                    case nameof(enOcrModel.OcrKNN):
                        ((FunctionBlock.Ocr)this._function).CharDetection.OcrParam = new OcrKnnParam();
                        this.propertyGrid1.SelectedObject = ((FunctionBlock.Ocr)this._function).CharDetection.OcrParam;
                        //this.dataGridView1.DataSource = ((FunctionBlock.Ocr)this._function).CharDetection.OcrParam.ChartRegion;
                        break;
                    case nameof(enOcrModel.OcrSVM):
                        ((FunctionBlock.Ocr)this._function).CharDetection.OcrParam = new OcrSvmParam();
                        this.propertyGrid1.SelectedObject = ((FunctionBlock.Ocr)this._function).CharDetection.OcrParam;
                        //this.dataGridView1.DataSource = ((FunctionBlock.Ocr)this._function).CharDetection.OcrParam.ChartRegion;
                        break;
                    case nameof(enOcrModel.OcrMLP):
                        ((FunctionBlock.Ocr)this._function).CharDetection.OcrParam = new OcrMlpParam();
                        this.propertyGrid1.SelectedObject = ((FunctionBlock.Ocr)this._function).CharDetection.OcrParam;
                        //this.dataGridView1.DataSource = ((FunctionBlock.Ocr)this._function).CharDetection.OcrParam.ChartRegion;
                        break;
                    case nameof(enOcrModel.OcrTextMode):
                        ((FunctionBlock.Ocr)this._function).CharDetection.OcrParam = new OcrTextModelParam();
                        this.propertyGrid1.SelectedObject = ((FunctionBlock.Ocr)this._function).CharDetection.OcrParam;
                        //this.dataGridView1.DataSource = ((FunctionBlock.Ocr)this._function).CharDetection.OcrParam.ChartRegion;
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
                HImage reduceImage = null, invertImage;
                HImage hImage = ((FunctionBlock.Ocr)this._function).ImageData?.Image;
                DoOcr ocr = ((FunctionBlock.Ocr)this._function).CharDetection;
                OcrParam ocrParam = ((FunctionBlock.Ocr)this._function).CharDetection.OcrParam;
                switch (this.comboBox1.SelectedItem.ToString())
                {
                    case "源图像":
                        this.drawObject.BackImage = ((FunctionBlock.Ocr)this._function).ImageData;
                        break;
                    case "取反图像":
                        this.drawObject.BackImage = ((FunctionBlock.Ocr)this._function).ImageData.InvertImage();
                        break;
                    case "字符区域":
                        //hRegion = new HRegion();
                        //hRegion.GenEmptyObj();
                        //foreach (var item in ocrParam.ChartRegion)
                        //{
                        //    HRegion hRegion1 = new HRegion();
                        //    hRegion1 = item.RoiShape.GetRegion();
                        //    hRegion = hRegion.ConcatObj(hRegion1);
                        //    hRegion1.Dispose();
                        //}
                        //unionRegion = hRegion.Union1();
                        //invertImage = hImage.InvertImage();
                        //reduceImage = invertImage.ReduceDomain(unionRegion);
                        //threRegion = reduceImage.Threshold(ocrParam.MinThreshold, ocrParam.MaxThreshold);
                        //switch (ocrParam.RegionOperate)
                        //{
                        //    default:
                        //    case enRegionOperate.NONE:
                        //        operateRegion = threRegion;
                        //        break;
                        //    case enRegionOperate.closing_rectangle1:
                        //        operateRegion = threRegion.ClosingRectangle1(ocrParam.MaskWidth, ocrParam.MaskHeight);
                        //        break;
                        //    case enRegionOperate.opening_rectangle1:
                        //        operateRegion = threRegion.OpeningRectangle1(ocrParam.MaskWidth, ocrParam.MaskHeight);
                        //        break;
                        //}
                        //connectionRegion = operateRegion.Connection();
                        //selectRegion = connectionRegion.SelectShape("area", "and", ocrParam.MinArea, ocrParam.MaxArea);
                        /////////////////////////////////
                        //this.drawObject.AttachPropertyData.Clear();
                        //this.drawObject.AttachPropertyData.Add(selectRegion);
                        //this.drawObject.DrawingGraphicObject();
                        //unionRegion?.Dispose();
                        //invertImage?.Dispose();
                        //threRegion?.Dispose();
                        //reduceImage?.Dispose();
                        break;
                }
            }
            catch
            {

            }
        }

        private void 字符区域dataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (this.dataGridView1.SelectedRows == null) return;
                if (this.dataGridView1.CurrentRow == null) return;
                int index = this.dataGridView1.CurrentRow.Index;
                //this.drawObject.SetParam(((FunctionBlock.Ocr)this._function).CharDetection.OcrParam.ChartRegion[index]);
                this.drawObject.DrawingGraphicObject();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void 视图工具toolStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            HalconLibrary ha = new HalconLibrary();
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
                    this.drawObject.AutoImage();
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

        private void 字符区域dataGridView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            try
            {
                if (this.dataGridView1.Columns[e.ColumnIndex].DataPropertyName == "RoiShape")
                {
                    /////////////////////////
                    e.Value = EvaluateValue(this.dataGridView1.Rows[e.RowIndex].DataBoundItem, this.dataGridView1.Columns[e.ColumnIndex].DataPropertyName);
                    if (e.Value != null)
                        e.FormattingApplied = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    int index = 0;
                    PixROI pixShape;
                    //if (((Ocr)this._function).CharDetection.OcrParam.ChartRegion == null)
                    //    ((Ocr)this._function).CharDetection.OcrParam.ChartRegion = new BindingList<ReduceParam>();
                    //BindingList<ReduceParam> listShape = ((Ocr)this._function).CharDetection.OcrParam.ChartRegion;
                    //userPixCoordSystem pixCoordSystem = ((Ocr)this._function).PixCoordSystem;
                    BindingList<ReduceParam> listShape = new BindingList<ReduceParam>();
                    userPixCoordSystem pixCoordSystem = new userPixCoordSystem();
                    switch (dataGridView1.Columns[e.ColumnIndex].Name)
                    {
                        case "TeachCol":
                            if (this.dataGridView1.Rows[e.RowIndex].DataBoundItem == null)
                            {
                                MessageBox.Show("未设置必需的图形参数，请先设置参数!");
                                return;
                            }
                            switch (listShape[e.RowIndex].ShapeType)
                            {
                                case enShapeType.矩形2:
                                    this.drawObject.AttachPropertyData.Clear();
                                    if (!(this.drawObject is userDrawRect2ROI))
                                    {
                                        this.drawObject.GrayValueInfo -= new GrayValueInfoEventHandler(GetGrayValueInfo);
                                        this.drawObject.ClearDrawingObject();
                                        this.drawObject = new userDrawRect2ROI(this.hWindowControl1, false);
                                        this.drawObject.GrayValueInfo += new GrayValueInfoEventHandler(GetGrayValueInfo);
                                    }
                                    break;
                                case enShapeType.矩形1:
                                    this.drawObject.AttachPropertyData.Clear();
                                    if (!(this.drawObject is userDrawRect1ROI))
                                    {
                                        this.drawObject.GrayValueInfo -= new GrayValueInfoEventHandler(GetGrayValueInfo);
                                        this.drawObject.ClearDrawingObject();
                                        this.drawObject = new userDrawRect1ROI(this.hWindowControl1, false);
                                        this.drawObject.GrayValueInfo += new GrayValueInfoEventHandler(GetGrayValueInfo);
                                    }
                                    break;
                                case enShapeType.圆:
                                    this.drawObject.AttachPropertyData.Clear();
                                    if (!(this.drawObject is userDrawCircleROI))
                                    {
                                        this.drawObject.GrayValueInfo -= new GrayValueInfoEventHandler(GetGrayValueInfo);
                                        this.drawObject.ClearDrawingObject();
                                        this.drawObject = new userDrawCircleROI(this.hWindowControl1, false);
                                        this.drawObject.GrayValueInfo += new GrayValueInfoEventHandler(GetGrayValueInfo);
                                    }
                                    break;
                                case enShapeType.椭圆:
                                    this.drawObject.AttachPropertyData.Clear();
                                    if (!(this.drawObject is userDrawEllipseROI))
                                    {
                                        this.drawObject.GrayValueInfo -= new GrayValueInfoEventHandler(GetGrayValueInfo);
                                        this.drawObject.ClearDrawingObject();
                                        this.drawObject = new userDrawEllipseROI(this.hWindowControl1, false);
                                        this.drawObject.GrayValueInfo += new GrayValueInfoEventHandler(GetGrayValueInfo);
                                    }
                                    break;
                                case enShapeType.多边形:
                                    this.drawObject.AttachPropertyData.Clear();
                                    if (!(this.drawObject is userDrawPolygonROI))
                                    {
                                        this.drawObject.GrayValueInfo -= new GrayValueInfoEventHandler(GetGrayValueInfo);
                                        this.drawObject.ClearDrawingObject();
                                        this.drawObject = new userDrawPolygonROI(this.hWindowControl1, false);
                                        this.drawObject.GrayValueInfo += new GrayValueInfoEventHandler(GetGrayValueInfo);
                                    }
                                    break;
                                case enShapeType.点:
                                    this.drawObject.AttachPropertyData.Clear();
                                    if (!(this.drawObject is userDrawPointROI))
                                    {
                                        this.drawObject.GrayValueInfo -= new GrayValueInfoEventHandler(GetGrayValueInfo);
                                        this.drawObject.ClearDrawingObject();
                                        this.drawObject = new userDrawPointROI(this.hWindowControl1, false);
                                        this.drawObject.GrayValueInfo += new GrayValueInfoEventHandler(GetGrayValueInfo);
                                    }
                                    break;
                                case enShapeType.线:
                                    this.drawObject.AttachPropertyData.Clear();
                                    if (!(this.drawObject is userDrawLineROI))
                                    {
                                        this.drawObject.GrayValueInfo -= new GrayValueInfoEventHandler(GetGrayValueInfo);
                                        this.drawObject.ClearDrawingObject();
                                        this.drawObject = new userDrawLineROI(this.hWindowControl1, false);
                                        this.drawObject.GrayValueInfo += new GrayValueInfoEventHandler(GetGrayValueInfo);
                                    }
                                    break;
                                default:
                                    throw new NotImplementedException(listShape[e.RowIndex].ShapeType.ToString() + "未实现!");
                            }
                            this.drawObject.IsLiveState = true;
                            //////////////////////////
                            foreach (var item in listShape)
                            {
                                if (index != e.RowIndex && item.RoiShape != null)
                                {
                                    this.drawObject.AddViewObject(new ViewData(item.RoiShape.AffinePixROI(pixCoordSystem?.GetHomMat2D()).GetXLD(), enColor.orange.ToString()));
                                }
                                index++;
                            }
                            if (this.drawObject.BackImage == null)
                                this.drawObject.BackImage = ((ImageReduce)this._function).ImageData;
                            if (listShape[e.RowIndex].RoiShape == null)
                                this.drawObject.SetParam(null);
                            else
                            {
                                this.drawObject.SetParam(pixCoordSystem);
                                this.drawObject.SetParam(listShape[e.RowIndex].RoiShape.AffinePixROI(pixCoordSystem?.GetHomMat2D()));
                            }
                            this.drawObject.DrawPixRoiShapeOnWindow(enColor.red, out pixShape);
                            this.drawObject.AddViewObject(new ViewData(pixShape.AffinePixROI(pixCoordSystem?.GetHomMat2D()).GetXLD(), enColor.orange.ToString()));
                            /////////////////////////////////////////////////////////
                            listShape[e.RowIndex].RoiShape = pixShape;  // 这个地方的添加不能使用变换后数据
                            //////////////////////////////////////////////////////////
                            for (int i = 0; i < this.dataGridView1.Rows.Count; i++)
                            {
                                this.dataGridView1.Rows[i].HeaderCell.Value = (i + 1).ToString();
                            }
                            break;
                        case "DeletCol":
                            if (listShape == null) return;
                            if (listShape.Count > e.RowIndex)
                                listShape.RemoveAt(e.RowIndex);
                            if (this.drawObject.AttachPropertyData.Count > e.RowIndex)
                                this.drawObject.AttachPropertyData.RemoveAt(e.RowIndex);
                            this.drawObject.DrawingGraphicObject();
                            for (int i = 0; i < this.dataGridView1.Rows.Count; i++)
                            {
                                if (this.dataGridView1.Rows.Count > i)
                                    this.dataGridView1.Rows[i].HeaderCell.Value = (i + 1).ToString();
                            }
                            break;
                        case "InseterCol":
                            if (listShape == null) return;
                            listShape.Insert(e.RowIndex, new ReduceParam());
                            //this.drawObject.DrawingGraphicObject();
                            for (int i = 0; i < this.dataGridView1.Rows.Count; i++)
                            {
                                if (this.dataGridView1.Rows.Count > i)
                                    this.dataGridView1.Rows[i].HeaderCell.Value = (i + 1).ToString();
                            }
                            break;
                        default:
                            this.drawObject.AttachPropertyData.Clear();
                            foreach (var item in listShape)
                            {
                                if (item.RoiShape == null) return;
                                if (index == e.RowIndex)
                                {
                                    this.drawObject.AddViewObject(new ViewData(item.RoiShape.AffinePixROI(pixCoordSystem?.GetHomMat2D()).GetXLD(), enColor.green.ToString()));
                                }
                                else
                                {
                                    this.drawObject.AddViewObject(new ViewData(item.RoiShape.AffinePixROI(pixCoordSystem?.GetHomMat2D()).GetXLD(), enColor.orange.ToString()));
                                }
                                index++;
                            }
                            this.drawObject.DrawingGraphicObject();
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

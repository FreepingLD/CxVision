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
        private VisualizeView drawObject;
        private TreeNode node;
        public OcrDetectionForm(IFunction function, TreeNode node)
        {
            this._function = function;
            this.node = node;
            InitializeComponent();
            this.Text = function.GetPropertyValues("名称").ToString();
            this.drawObject = new userDrawRect2ROI(this.hWindowControl1, false);
            new ListBoxWrapClass().InitListBox(this.listBox1, node);
            new ListBoxWrapClass().InitListBox(this.listBox2, node, 2);
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
                //this.propertyGrid1.SelectedObject = ((FunctionBlock.Ocr)this._function).CharDetection.OcrParam;
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



    }
}

using FunctionBlock;
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
    public partial class OcrParamForm : Form
    {
        private bool IsLoad = false;
        private Ocr _ocr;
        private DataGridView _dataGridView;
        public OcrParamForm(Ocr ocr, DataGridView dataGridView)
        {
            InitializeComponent();
            this._ocr = ocr;
            this._dataGridView = dataGridView;
            this.BindProperty();
        }
        public OcrParamForm()
        {
            InitializeComponent();
            this._dataGridView = null;
            this.BindProperty();
        }
        private void BindProperty()
        {
            try
            {

                // 创建匹配参数
                foreach (var item in Enum.GetValues(typeof(enOcrModel)))
                    this.识别算法comboBox.Items.Add(item);
                ////////////////////////////////////////////////
                if (this._ocr.Param == null) return;
                this.识别算法comboBox.DataBindings.Add(nameof(this.识别算法comboBox.Text), (this._ocr.Param), "OcrModel", true, DataSourceUpdateMode.OnPropertyChanged);
                //this.启用检测checkBox.DataBindings.Add(nameof(this.启用检测checkBox.Checked), (this._param), "IsDetect", true, DataSourceUpdateMode.OnPropertyChanged);
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

        private void DetectFlawForm_Load(object sender, EventArgs e)
        {
            this.IsLoad = true;
        }

        private void 检测算法comboBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.识别算法comboBox.Text.Length == 0) return;
                switch (this.识别算法comboBox.Text)
                {
                    case nameof(enOcrModel.DeepOCR):
                        if (this.IsLoad)
                        {
                            if (this._ocr.Param != null && this._ocr.Param.OcrModel != this.识别算法comboBox.Text)
                            {
                                this._ocr.Param = new DeepOcrParam(this.识别算法comboBox.Text);
                                this._dataGridView.DataSource = this._ocr.Param.ReduceParam;
                            }
                        }
                        AddForm(this.参数panel, new DeepOcrForm((DeepOcrParam)this._ocr.Param, this._ocr));
                        break;
                    case nameof(enOcrModel.OcrCNN):
                        if (this.IsLoad)
                        {
                            if (this._ocr.Param != null && this._ocr.Param.OcrModel != this.识别算法comboBox.Text)
                            {
                                this._ocr.Param = new OcrCnnParam(this.识别算法comboBox.Text);
                                this._dataGridView.DataSource = this._ocr.Param.ReduceParam;
                            }
                        }
                        AddForm(this.参数panel, new CnnOcrForm((OcrCnnParam)this._ocr.Param));
                        break;
                    case nameof(enOcrModel.OcrKNN):
                        if (this.IsLoad)
                        {
                            if (this._ocr.Param != null && this._ocr.Param.OcrModel != this.识别算法comboBox.Text)
                            {
                                this._ocr.Param = new OcrKnnParam(this.识别算法comboBox.Text);
                                this._dataGridView.DataSource = this._ocr.Param.ReduceParam;
                            }
                        }
                        AddForm(this.参数panel, new KnnOcrForm((OcrKnnParam)this._ocr.Param));
                        break;
                    case nameof(enOcrModel.OcrMLP):
                        if (this.IsLoad)
                        {
                            if (this._ocr.Param != null && this._ocr.Param.OcrModel != this.识别算法comboBox.Text)
                            {
                                this._ocr.Param = new OcrMlpParam(this.识别算法comboBox.Text);
                                this._dataGridView.DataSource = this._ocr.Param.ReduceParam;
                            }
                        }
                        AddForm(this.参数panel, new MlpOcrForm((OcrMlpParam)this._ocr.Param));
                        break;
                    case nameof(enOcrModel.OcrSVM):
                        if (this.IsLoad)
                        {
                            if (this._ocr.Param != null && this._ocr.Param.OcrModel != this.识别算法comboBox.Text)
                            {
                                this._ocr.Param = new OcrSvmParam(this.识别算法comboBox.Text);
                                this._dataGridView.DataSource = this._ocr.Param.ReduceParam;
                            }
                        }
                        AddForm(this.参数panel, new SvmOcrForm((OcrSvmParam)this._ocr.Param));
                        break;
                        //case nameof(enOcrModel.OcrTextMode):
                        //    if (this.IsLoad)
                        //    {
                        //        if (this._param != null && this._param.OcrModel != this.识别算法comboBox.Text)
                        //            this._param = new OcrTextModelParam(this.识别算法comboBox.Text);
                        //    }
                        //    AddForm(this.参数panel, new TextModelOcrForm((OcrTextModelParam)this._param));
                        //    break;              
                }
            }
            catch (Exception ex)
            {
               new Common.UserMessageForm(ex.ToString()).ShowDialog();;
            }
        }

        private void 测试算法button_Click(object sender, EventArgs e)
        {
            try
            {
                //this._drawObject.ClearViewObject();
                //HalconDotNet.HXLDCont hXLDCont = new HalconDotNet.HXLDCont();
                //HalconDotNet.HRegion hRegion = new FlawDetectMethod().FlawDetect(this._drawObject.BackImage.Image, this.DicParam[this.CurrentItem].DetectROI, this.DicParam[this.CurrentItem].DetectParam, out hXLDCont);
                //this._drawObject.AddViewObject(new ViewData(hRegion, "red"));
                //this._drawObject.AddViewObject(new ViewData(hXLDCont, "green"));
            }
            catch (Exception ex)
            {
               new Common.UserMessageForm(ex.ToString()).ShowDialog();;
            }
        }

        private void 字符提取button_Click(object sender, EventArgs e)
        {
            try
            {
                new ExtractChartForm(this._ocr.Param?.ExtractParam).Show();
            }
            catch (Exception ex)
            {
               new Common.UserMessageForm(ex.ToString()).ShowDialog();;
            }
        }


    }
}

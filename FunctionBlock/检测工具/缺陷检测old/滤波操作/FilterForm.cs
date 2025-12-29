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

namespace FunctionBlock
{
    public partial class FilterForm : Form
    {
        private bool IsLoad = false;
        private FilterParam _param;
        public FilterParam FilterParam { get => _param; set => _param = value; }
        public Dictionary<string, FlawDetectParam> DicParam { get; set; }
        public string CurrentItem { get; set; }

        public FilterForm(FilterParam param)
        {
            this._param = param;
            this.CurrentItem = "";
            InitializeComponent();
            this.BindProperty();
        }
        public FilterForm(Dictionary<string, FlawDetectParam> param)
        {
            this.CurrentItem = "";
            this.DicParam = param;
            this._param = null;
            InitializeComponent();
            this.BindProperty();
        }
        public FilterForm()
        {
            this.CurrentItem = "";
            this._param = null;
            InitializeComponent();
            this.BindProperty();
        }
        private void BindProperty()
        {
            try
            {               
                // 创建匹配参数
                this.方法comboBox.Items.Clear();
                foreach (var item in Enum.GetValues(typeof(enDetectFilterMethod)))
                    this.方法comboBox.Items.Add(item);
                ////////////////////////////////////////////////
                //if (this._param == null) return;
                //this.方法comboBox.DataBindings.Add(nameof(this.方法comboBox.Text), this._param, nameof(this._param.Method), true, DataSourceUpdateMode.OnPropertyChanged);
                //this.启用滤波checkBox.DataBindings.Add(nameof(this.启用滤波checkBox.Checked), this._param, nameof(this._param.IsFilter), true, DataSourceUpdateMode.OnPropertyChanged);
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

        private void FilterForm_Load(object sender, EventArgs e)
        {
            this.IsLoad = true;
        }

        private void 方法comboBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.方法comboBox.Text.Length == 0) return;
                switch (this.方法comboBox.Text)
                {
                    case nameof(enDetectFilterMethod.bilateral_filter):
                        if (this.IsLoad )
                        {
                            if (this.DicParam.ContainsKey(this.CurrentItem) && this.DicParam[this.CurrentItem].FilterParam.Method != this.方法comboBox.Text)
                                this.DicParam[this.CurrentItem].FilterParam = new BilateralFilterParam(this.方法comboBox.Text);
                        }
                        AddForm(this.splitContainer1.Panel2, new BilateralFilterForm(this.DicParam[this.CurrentItem].FilterParam));
                        break;
                    case nameof(enDetectFilterMethod.binomial_filter):
                        if (this.IsLoad )
                        {
                            if (this.DicParam.ContainsKey(this.CurrentItem) && this.DicParam[this.CurrentItem].FilterParam.Method != this.方法comboBox.Text)
                                this.DicParam[this.CurrentItem].FilterParam = new BinomialFilterParam(this.方法comboBox.Text);
                        }
                        AddForm(this.splitContainer1.Panel2, new BinomialFilterForm(this.DicParam[this.CurrentItem].FilterParam));
                        break;
                    case nameof(enDetectFilterMethod.gauss_filter):
                        if (this.IsLoad )
                        {
                            if (this.DicParam.ContainsKey(this.CurrentItem) && this.DicParam[this.CurrentItem].FilterParam.Method != this.方法comboBox.Text)
                                this.DicParam[this.CurrentItem].FilterParam = new GaussFilterParam(this.方法comboBox.Text);
                        }
                        AddForm(this.splitContainer1.Panel2, new GaussFilterForm(this.DicParam[this.CurrentItem].FilterParam));
                        break;
                    case nameof(enDetectFilterMethod.guided_filter):
                        if (this.IsLoad)
                        {
                            if (this.DicParam.ContainsKey(this.CurrentItem) && this.DicParam[this.CurrentItem].FilterParam.Method != this.方法comboBox.Text)
                                this.DicParam[this.CurrentItem].FilterParam = new GuidedFilterParam(this.方法comboBox.Text);
                        }
                        AddForm(this.splitContainer1.Panel2, new GuidedFilterForm(this.DicParam[this.CurrentItem].FilterParam));
                        break;
                    case nameof(enDetectFilterMethod.mean_image):
                        if (this.IsLoad)
                        {
                            if (this.DicParam.ContainsKey(this.CurrentItem) && this.DicParam[this.CurrentItem].FilterParam.Method != this.方法comboBox.Text)
                                this.DicParam[this.CurrentItem].FilterParam = new MeanFilterParam(this.方法comboBox.Text);
                        }
                        AddForm(this.splitContainer1.Panel2, new MeanFilterForm(this.DicParam[this.CurrentItem].FilterParam));
                        break;
                    case nameof(enDetectFilterMethod.mean_n):
                        if (this.IsLoad )
                        {
                            if (this.DicParam.ContainsKey(this.CurrentItem) && this.DicParam[this.CurrentItem].FilterParam.Method != this.方法comboBox.Text)
                                this.DicParam[this.CurrentItem].FilterParam = new MeanNFilterParam(this.方法comboBox.Text);
                        }
                        AddForm(this.splitContainer1.Panel2, new MeanNFilterForm(this.DicParam[this.CurrentItem].FilterParam));
                        break;
                    case nameof(enDetectFilterMethod.median_image):
                        if (this.IsLoad)
                        {
                            if (this.DicParam.ContainsKey(this.CurrentItem) && this.DicParam[this.CurrentItem].FilterParam.Method != this.方法comboBox.Text)
                                this.DicParam[this.CurrentItem].FilterParam = new MedianFilterParam(this.方法comboBox.Text);
                        }
                        AddForm(this.splitContainer1.Panel2, new MedianFilterrForm(this.DicParam[this.CurrentItem].FilterParam));
                        break;
                    case nameof(enDetectFilterMethod.median_rect):
                        if (this.IsLoad)
                        {
                            if (this.DicParam.ContainsKey(this.CurrentItem) && this.DicParam[this.CurrentItem].FilterParam.Method != this.方法comboBox.Text)
                                this.DicParam[this.CurrentItem].FilterParam = new MedianRectFilterParam(this.方法comboBox.Text);
                        }
                        AddForm(this.splitContainer1.Panel2, new MedianRectFilterForm(this.DicParam[this.CurrentItem].FilterParam));
                        break;
                    case nameof(enDetectFilterMethod.median_separate):
                        if (this.IsLoad)
                        {
                            if (this.DicParam.ContainsKey(this.CurrentItem) && this.DicParam[this.CurrentItem].FilterParam.Method != this.方法comboBox.Text)
                                this.DicParam[this.CurrentItem].FilterParam = new MedianSeparateFilterParam(this.方法comboBox.Text);
                        }
                        AddForm(this.splitContainer1.Panel2, new MedianSeparateFilterForm(this.DicParam[this.CurrentItem].FilterParam));
                        break;
                    case nameof(enDetectFilterMethod.median_weighted):
                        if (this.IsLoad)
                        {
                            if (this.DicParam.ContainsKey(this.CurrentItem) && this.DicParam[this.CurrentItem].FilterParam.Method != this.方法comboBox.Text)
                                this.DicParam[this.CurrentItem].FilterParam = new MedianWeightedFilterParam(this.方法comboBox.Text);
                        }
                        AddForm(this.splitContainer1.Panel2, new MedianWeightedFilterForm(this.DicParam[this.CurrentItem].FilterParam));
                        break;  
                    case nameof(enDetectFilterMethod.convol_image):
                        if (this.IsLoad)
                        {
                            if (this.DicParam.ContainsKey(this.CurrentItem) && this.DicParam[this.CurrentItem].FilterParam.Method != this.方法comboBox.Text)
                                this.DicParam[this.CurrentItem].FilterParam = new ConvolFilterParam(this.方法comboBox.Text);
                        }
                        AddForm(this.splitContainer1.Panel2, new ConvolFilterForm(this.DicParam[this.CurrentItem].FilterParam));
                        break;  
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}

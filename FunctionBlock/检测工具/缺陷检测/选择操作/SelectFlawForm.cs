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
    public partial class SelectFlawForm : Form
    {
        private bool IsLoad = false;
        private SelectFlawParam _selectParam;
        public SelectFlawParam SelectParam { get => _selectParam; set => _selectParam = value; }
        public Dictionary<string, FlawDetectParam> DicParam { get; set; }
        public string CurrentItem { get; set; }
        private VisualizeView _drawObject;
        public SelectFlawForm(SelectFlawParam param)
        {
            this._selectParam = param;
            this.CurrentItem = "";
            InitializeComponent();
            this.BindProperty();
        }
        public SelectFlawForm(Dictionary<string, FlawDetectParam> param, VisualizeView drawObject)
        {
            this._drawObject = drawObject;
            this.CurrentItem = "";
            this.DicParam = param;
            this._selectParam = null;
            InitializeComponent();
            this.BindProperty();
        }
        public SelectFlawForm()
        {
            this.CurrentItem = "";
            this._selectParam = null;
            InitializeComponent();
            this.BindProperty();
        }
        private void BindProperty()
        {
            try
            {
                // 创建匹配参数
                this.方法comboBox.Items.Clear();
                foreach (var item in Enum.GetValues(typeof(enFlawSelectMethod)))
                    this.方法comboBox.Items.Add(item);
                ////////////////////////////////////////////////
                if (this._selectParam == null) return;
                this.方法comboBox.DataBindings.Add(nameof(this.方法comboBox.Text), this._selectParam, nameof(this._selectParam.Method), true, DataSourceUpdateMode.OnPropertyChanged);
                //this.启用滤波checkBox.DataBindings.Add(nameof(this.启用滤波checkBox.Checked), this._selectParam, nameof(this._selectParam.IsFilter), true, DataSourceUpdateMode.OnPropertyChanged);
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
                    case nameof(enFlawSelectMethod.none):
                        if (this.IsLoad)
                        {
                            if (this.DicParam.ContainsKey(this.CurrentItem) && this.DicParam[this.CurrentItem].SelectParam.Method != this.方法comboBox.Text)
                                this.DicParam[this.CurrentItem].SelectParam = new SelectFlawParam(this.方法comboBox.Text);
                        }
                        this.参数panel.Controls.Clear();
                        break;
                    case nameof(enFlawSelectMethod.select_shape):
                        if (this.IsLoad)
                        {
                            if (this.DicParam.ContainsKey(this.CurrentItem) && this.DicParam[this.CurrentItem].SelectParam.Method != this.方法comboBox.Text)
                                this.DicParam[this.CurrentItem].SelectParam = new SelectShapeFlawParam(this.方法comboBox.Text);
                        }
                        AddForm(this.参数panel, new SelectShapeFlawForm(this.DicParam[this.CurrentItem].SelectParam));
                        break;
                    case nameof(enFlawSelectMethod.select_shape_proto):
                        if (this.IsLoad)
                        {
                            if (this.DicParam.ContainsKey(this.CurrentItem) && this.DicParam[this.CurrentItem].SelectParam.Method != this.方法comboBox.Text)
                                this.DicParam[this.CurrentItem].SelectParam = new SelectShapeProtoFlawParam(this.方法comboBox.Text);
                        }
                        AddForm(this.参数panel, new SelectShapeProtoFlawForm(this.DicParam[this.CurrentItem].SelectParam));
                        break;
                    case nameof(enFlawSelectMethod.select_shape_std):
                        if (this.IsLoad)
                        {
                            if (this.DicParam.ContainsKey(this.CurrentItem) && this.DicParam[this.CurrentItem].SelectParam.Method != this.方法comboBox.Text)
                                this.DicParam[this.CurrentItem].SelectParam = new SelectShapeStdFlawParam(this.方法comboBox.Text);
                        }
                        AddForm(this.参数panel, new SelectShapeStdFlawForm(this.DicParam[this.CurrentItem].SelectParam));
                        break;
                    default:
                        throw new ArgumentException("未实现的异常");
                }
            }
            catch (Exception ex)
            {
               new Common.UserMessageForm(ex.ToString()).ShowDialog();;
            }
        }

        private void 测试选择button_Click(object sender, EventArgs e)
        {
            try
            {
                HalconDotNet.HRegion hRegion = new SelectMethod().FlawSelect(this._drawObject.BackImage?.Image, this.DicParam[this.CurrentItem].SelectParam);
                this._drawObject.AddViewObject(new ViewData(hRegion, "red"));
            }
            catch (Exception ex)
            {
               new Common.UserMessageForm(ex.ToString()).ShowDialog();;
            }
        }


    }
}

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
    public partial class DetectFlawForm : Form
    {
        private bool IsLoad = false;
        private ThresholParam _param;

        public ThresholParam DetectParam { get => _param; set => _param = value; }

        public Dictionary<string, FlawDetectParam> DicParam { get; set; }

        public string CurrentItem { get; set; }


        public DetectFlawForm(Dictionary<string, FlawDetectParam> param)
        {
            this.DicParam = param;
            this._param = null;
            this.CurrentItem = "";
            InitializeComponent();
            this.BindProperty();
        }
        public DetectFlawForm(ThresholParam param)
        {
            this.CurrentItem = "";
            this._param = param;
            InitializeComponent();
            this.BindProperty();
        }
        public DetectFlawForm()
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
                foreach (var item in Enum.GetValues(typeof(enFlawDetectMethod)))
                    this.检测算法comboBox.Items.Add(item);
                ////////////////////////////////////////////////
                //if (this._param == null) return;
                //this.检测算法comboBox.DataBindings.Add(nameof(this.检测算法comboBox.Text), (this._param), "Method", true, DataSourceUpdateMode.OnPropertyChanged);
                //this.启用检测checkBox.DataBindings.Add(nameof(this.启用检测checkBox.Checked), (this._param), "IsDetect", true, DataSourceUpdateMode.OnPropertyChanged);
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

        private void DetectFlawForm_Load(object sender, EventArgs e)
        {
            this.IsLoad = true;
        }

        private void 检测算法comboBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.检测算法comboBox.Text.Length == 0) return;
                switch (this.检测算法comboBox.Text)
                {
                    case nameof(enFlawDetectMethod.Threshold):
                        if (this.IsLoad )
                        {
                            if (this.DicParam.ContainsKey(this.CurrentItem) && this.DicParam[this.CurrentItem].DetectParam.Method != this.检测算法comboBox.Text)
                                this.DicParam[this.CurrentItem].DetectParam = new ThresholdParam(this.检测算法comboBox.Text);
                        }
                        AddForm(this.splitContainer1.Panel2, new ThresholdDetectForm(this.DicParam[this.CurrentItem].DetectParam));
                        break;
                    case nameof(enFlawDetectMethod.AutoThreshold):
                        if (this.IsLoad )
                        {
                            if (this.DicParam.ContainsKey(this.CurrentItem) && this.DicParam[this.CurrentItem].DetectParam.Method != this.检测算法comboBox.Text)
                                this.DicParam[this.CurrentItem].DetectParam = new AutoThresholdParam(this.检测算法comboBox.Text);
                        }
                        AddForm(this.splitContainer1.Panel2, new AutoThresholdDetectForm(this.DicParam[this.CurrentItem].DetectParam));
                        break;
                    case nameof(enFlawDetectMethod.BinaryThreshold):
                        if (this.IsLoad )
                        {
                            if (this.DicParam.ContainsKey(this.CurrentItem) && this.DicParam[this.CurrentItem].DetectParam.Method != this.检测算法comboBox.Text)
                                this.DicParam[this.CurrentItem].DetectParam = new BinaryThresholdParam(this.检测算法comboBox.Text);
                        }
                        AddForm(this.splitContainer1.Panel2, new BinaryThresholdDetectForm(this.DicParam[this.CurrentItem].DetectParam));
                        break;
                    case nameof(enFlawDetectMethod.CharThreshold):
                        if (this.IsLoad )
                        {
                            if (this.DicParam.ContainsKey(this.CurrentItem) && this.DicParam[this.CurrentItem].DetectParam.Method != this.检测算法comboBox.Text)
                                this.DicParam[this.CurrentItem].DetectParam = new CharThresholdParam(this.检测算法comboBox.Text);
                        }
                        AddForm(this.splitContainer1.Panel2, new CharThresholdDetectForm(this.DicParam[this.CurrentItem].DetectParam));
                        break;
                    case nameof(enFlawDetectMethod.DualThreshold):
                        if (this.IsLoad )
                        {
                            if (this.DicParam.ContainsKey(this.CurrentItem) && this.DicParam[this.CurrentItem].DetectParam.Method != this.检测算法comboBox.Text)
                                this.DicParam[this.CurrentItem].DetectParam = new DualThresholdParam(this.检测算法comboBox.Text);
                        }
                        AddForm(this.splitContainer1.Panel2, new DualThresholdDetectForm(this.DicParam[this.CurrentItem].DetectParam));
                        break;
                    case nameof(enFlawDetectMethod.DynThreshold):
                        if (this.IsLoad )
                        {
                            if (this.DicParam.ContainsKey(this.CurrentItem) && this.DicParam[this.CurrentItem].DetectParam.Method != this.检测算法comboBox.Text)
                                this.DicParam[this.CurrentItem].DetectParam = new DynThresholdParam(this.检测算法comboBox.Text);
                        }
                        AddForm(this.splitContainer1.Panel2, new DynThresholdDetectForm(this.DicParam[this.CurrentItem].DetectParam));
                        break;
                    case nameof(enFlawDetectMethod.FastThreshold):
                        if (this.IsLoad )
                        {
                            if (this.DicParam.ContainsKey(this.CurrentItem) && this.DicParam[this.CurrentItem].DetectParam.Method != this.检测算法comboBox.Text)
                                this.DicParam[this.CurrentItem].DetectParam = new FastThresholdParam(this.检测算法comboBox.Text);
                        }
                        AddForm(this.splitContainer1.Panel2, new FastThresholdDetectForm(this.DicParam[this.CurrentItem].DetectParam));
                        break;
                    case nameof(enFlawDetectMethod.HysteresisThreshold):
                        if (this.IsLoad )
                        {
                            if (this.DicParam.ContainsKey(this.CurrentItem) && this.DicParam[this.CurrentItem].DetectParam.Method != this.检测算法comboBox.Text)
                                this.DicParam[this.CurrentItem].DetectParam = new HysteresisThresholdParam(this.检测算法comboBox.Text);
                        }
                        AddForm(this.splitContainer1.Panel2, new HysteresisThresholdDetectForm(this.DicParam[this.CurrentItem].DetectParam));
                        break;
                    case nameof(enFlawDetectMethod.LocalThreshold):
                        if (this.IsLoad )
                        {
                            if (this.DicParam.ContainsKey(this.CurrentItem) && this.DicParam[this.CurrentItem].DetectParam.Method != this.检测算法comboBox.Text)
                                this.DicParam[this.CurrentItem].DetectParam = new LocalThresholdParam(this.检测算法comboBox.Text);
                        }
                        AddForm(this.splitContainer1.Panel2, new LocalThresholdDetectForm(this.DicParam[this.CurrentItem].DetectParam));
                        break;
                    case nameof(enFlawDetectMethod.VarThreshold):
                        if (this.IsLoad )
                        {
                            if (this.DicParam.ContainsKey(this.CurrentItem) && this.DicParam[this.CurrentItem].DetectParam.Method != this.检测算法comboBox.Text)
                                this.DicParam[this.CurrentItem].DetectParam = new VarThresholdParam(this.检测算法comboBox.Text);
                        }
                        AddForm(this.splitContainer1.Panel2, new VarThresholdDetectForm(this.DicParam[this.CurrentItem].DetectParam));
                        break;
                    case nameof(enFlawDetectMethod.WatershedsThreshold):
                        if (this.IsLoad )
                        {
                            if (this.DicParam.ContainsKey(this.CurrentItem) && this.DicParam[this.CurrentItem].DetectParam.Method != this.检测算法comboBox.Text)
                                this.DicParam[this.CurrentItem].DetectParam = new WatershedsThresholdParam(this.检测算法comboBox.Text);
                        }
                        AddForm(this.splitContainer1.Panel2, new WatershedsThresholdDetectForm(this.DicParam[this.CurrentItem].DetectParam));
                        break;
                    case nameof(enFlawDetectMethod.CallipersInsp):
                        if (this.IsLoad)
                        {
                            if (this.DicParam.ContainsKey(this.CurrentItem) && this.DicParam[this.CurrentItem].DetectParam.Method != this.检测算法comboBox.Text)
                                this.DicParam[this.CurrentItem].DetectParam = new CallipersInspParam(this.检测算法comboBox.Text);
                        }
                        AddForm(this.splitContainer1.Panel2, new CallipersDetectForm(this.DicParam[this.CurrentItem].DetectParam));
                        break;
                    case nameof(enFlawDetectMethod.ScriptInsp):
                        if (this.IsLoad)
                        {
                            if (this.DicParam.ContainsKey(this.CurrentItem) && this.DicParam[this.CurrentItem].DetectParam.Method != this.检测算法comboBox.Text)
                                this.DicParam[this.CurrentItem].DetectParam = new ScriptInspParam(this.检测算法comboBox.Text);
                        }
                        AddForm(this.splitContainer1.Panel2, new ScriptDetectForm(this.DicParam[this.CurrentItem].DetectParam));
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

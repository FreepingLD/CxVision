using Common;
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
    public partial class BarCodeParamForm : Form
    {
        private DoBarCodeDetection _Detection;
        private BarCodeParam _dataCodeParam;
        public BarCodeParamForm(BarCodeParam Params)
        {
            this._dataCodeParam = Params as BarCodeParam;
            InitializeComponent();
            BindProperty();
        }
        public BarCodeParamForm(DoBarCodeDetection detection)
        {
            this._Detection = detection;
            this._dataCodeParam = detection.CodeParam as BarCodeParam;
            InitializeComponent();
            BindProperty();
        }
        private void BindProperty()
        {
            try
            {
                // 创建匹配参数
                /////////////////////////////////////////
                this.ElementSizeMincomboBox.DataBindings.Add("Text", this._dataCodeParam, nameof(this._dataCodeParam.element_size_min), true, DataSourceUpdateMode.OnPropertyChanged);
                this.ElementSizeMaxComboBox.DataBindings.Add("Text", this._dataCodeParam, nameof(this._dataCodeParam.element_size_max), true, DataSourceUpdateMode.OnPropertyChanged);
                this.ElementSizeVariableComboBox.DataBindings.Add("Text", this._dataCodeParam, nameof(this._dataCodeParam.element_size_variable), true, DataSourceUpdateMode.OnPropertyChanged);
                this.BarcodeHeightMinComboBox.DataBindings.Add("Text", this._dataCodeParam, nameof(this._dataCodeParam.barcode_height_min), true, DataSourceUpdateMode.OnPropertyChanged);
                this.BarcodeWidthMinCombox.DataBindings.Add("Text", this._dataCodeParam, nameof(this._dataCodeParam.barcode_width_min), true, DataSourceUpdateMode.OnPropertyChanged);
                this.NumScanlinesComboBox.DataBindings.Add("Text", this._dataCodeParam, nameof(this._dataCodeParam.num_scanlines), true, DataSourceUpdateMode.OnPropertyChanged);
                this.MinIdenticalScanlinesComboBox.DataBindings.Add("Text", this._dataCodeParam, nameof(this._dataCodeParam.min_identical_scanlines), true, DataSourceUpdateMode.OnPropertyChanged);
                this.MajorityVotingComboBox.DataBindings.Add("Text", this._dataCodeParam, nameof(this._dataCodeParam.majority_voting), true, DataSourceUpdateMode.OnPropertyChanged);
                this.OrientationComboBox.DataBindings.Add("Text", this._dataCodeParam, nameof(this._dataCodeParam.orientation), true, DataSourceUpdateMode.OnPropertyChanged);
                this.Orientation_tolComboBox.DataBindings.Add("Text", this._dataCodeParam, nameof(this._dataCodeParam.orientation_tol), true, DataSourceUpdateMode.OnPropertyChanged);
                this.Quiet_zoneComboBox.DataBindings.Add("Text", this._dataCodeParam, nameof(this._dataCodeParam.quiet_zone), true, DataSourceUpdateMode.OnPropertyChanged);
                this.StartStopToleranceComboBox.DataBindings.Add("Text", this._dataCodeParam, nameof(this._dataCodeParam.start_stop_tolerance), true, DataSourceUpdateMode.OnPropertyChanged);
                this.MinCodeLengthComboBox.DataBindings.Add("Text", this._dataCodeParam, nameof(this._dataCodeParam.min_code_length), true, DataSourceUpdateMode.OnPropertyChanged);
                this.MergeScanlinesComboBox.DataBindings.Add("Text", this._dataCodeParam, nameof(this._dataCodeParam.merge_scanlines), true, DataSourceUpdateMode.OnPropertyChanged);
                this.SmallElementsRobustnessComboBox.DataBindings.Add("Text", this._dataCodeParam, nameof(this._dataCodeParam.small_elements_robustness), true, DataSourceUpdateMode.OnPropertyChanged);
                this.MeasThreshComboBox.DataBindings.Add("Text", this._dataCodeParam, nameof(this._dataCodeParam.meas_thresh), true, DataSourceUpdateMode.OnPropertyChanged);
                this.MeasThreshAbsComboBox.DataBindings.Add("Text", this._dataCodeParam, nameof(this._dataCodeParam.meas_thresh_abs), true, DataSourceUpdateMode.OnPropertyChanged);
                this.ContrastMinComboBox.DataBindings.Add("Text", this._dataCodeParam, nameof(this._dataCodeParam.contrast_min), true, DataSourceUpdateMode.OnPropertyChanged);
                this.CheckCharComboBox.DataBindings.Add("Text", this._dataCodeParam, nameof(this._dataCodeParam.check_char), true, DataSourceUpdateMode.OnPropertyChanged);
                this.CompositeCodeComboBox.DataBindings.Add("Text", this._dataCodeParam, nameof(this._dataCodeParam.composite_code), true, DataSourceUpdateMode.OnPropertyChanged);
                this.UpceEncodationComboBox.DataBindings.Add("Text", this._dataCodeParam, nameof(this._dataCodeParam.upce_encodation), true, DataSourceUpdateMode.OnPropertyChanged);

            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
            }

        }

        private void BarCodeParamForm_Load(object sender, EventArgs e)
        {

        }



    }
}

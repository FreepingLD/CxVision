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
    public partial class QRDataCodeParamForm : Form
    {
        private DoDataCodeDetection _Detection;
        private QRDataCodeParam _dataCodeParam;
        public QRDataCodeParamForm(DataCodeParam Params)
        {
            this._dataCodeParam = Params as QRDataCodeParam;
            InitializeComponent();
            BindProperty();
        }
        public QRDataCodeParamForm(DoDataCodeDetection detection)
        {
            this._Detection = detection;
            this._dataCodeParam = detection.CodeParam as QRDataCodeParam;
            InitializeComponent();
            BindProperty();
        }
        private void BindProperty()
        {
            try
            {
                // 创建匹配参数
                /////////////////////////////////////////
                this.默认参数comboBox.DataBindings.Add("Text", this._dataCodeParam, nameof(this._dataCodeParam.default_parameters), true, DataSourceUpdateMode.OnPropertyChanged);
                this.ModelTypeComboBox.DataBindings.Add("Text", this._dataCodeParam, nameof(this._dataCodeParam.model_type), true, DataSourceUpdateMode.OnPropertyChanged);
                this.MinVersionComboBox.DataBindings.Add("Text", this._dataCodeParam, nameof(this._dataCodeParam.version_min), true, DataSourceUpdateMode.OnPropertyChanged);
                this.MaxVersionComboBox.DataBindings.Add("Text", this._dataCodeParam, nameof(this._dataCodeParam.version_max), true, DataSourceUpdateMode.OnPropertyChanged);
                this.MinSymbolSizeComboBox.DataBindings.Add("Text", this._dataCodeParam, nameof(this._dataCodeParam.symbol_size_min), true, DataSourceUpdateMode.OnPropertyChanged);
                this.MaxSymbolSizeComboBox.DataBindings.Add("Text", this._dataCodeParam, nameof(this._dataCodeParam.symbol_size_max), true, DataSourceUpdateMode.OnPropertyChanged);
                this.MinContrastComboBox.DataBindings.Add("Text", this._dataCodeParam, nameof(this._dataCodeParam.contrast_min), true, DataSourceUpdateMode.OnPropertyChanged);
                this.MinModuleSizeComboBox.DataBindings.Add("Text", this._dataCodeParam, nameof(this._dataCodeParam.module_size_min), true, DataSourceUpdateMode.OnPropertyChanged);
                this.MaxModuleSizeComboBox.DataBindings.Add("Text", this._dataCodeParam, nameof(this._dataCodeParam.module_size_max), true, DataSourceUpdateMode.OnPropertyChanged);
                this.MinModuleGapComboBox.DataBindings.Add("Text", this._dataCodeParam, nameof(this._dataCodeParam.module_gap_min), true, DataSourceUpdateMode.OnPropertyChanged);
                this.MaxModuleGapComboBox.DataBindings.Add("Text", this._dataCodeParam, nameof(this._dataCodeParam.module_gap_max), true, DataSourceUpdateMode.OnPropertyChanged);
                this.SmallModulesRobustnessComboBox.DataBindings.Add("Text", this._dataCodeParam, nameof(this._dataCodeParam.small_modules_robustness), true, DataSourceUpdateMode.OnPropertyChanged);
                this.MinPositionPatternComboBox.DataBindings.Add("Text", this._dataCodeParam, nameof(this._dataCodeParam.position_pattern_min), true, DataSourceUpdateMode.OnPropertyChanged);
                this.QualityComboBox.DataBindings.Add("Text", this._dataCodeParam, nameof(this._dataCodeParam.quality_isoiec15415_aperture_size), true, DataSourceUpdateMode.OnPropertyChanged);
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
            }

        }
        private void ClearBindProperty()
        {
            try
            {
                // 创建匹配参数
                /////////////////////////////////////////
                this.默认参数comboBox.DataBindings.Clear();// .Add("Text", this._dataCodeParam, nameof(this._dataCodeParam.default_parameters), true, DataSourceUpdateMode.OnPropertyChanged);
                this.ModelTypeComboBox.DataBindings.Clear();//.Add("Text", this._dataCodeParam, nameof(this._dataCodeParam.model_type), true, DataSourceUpdateMode.OnPropertyChanged);
                this.MinVersionComboBox.DataBindings.Clear();//.Add("Text", this._dataCodeParam, nameof(this._dataCodeParam.version_min), true, DataSourceUpdateMode.OnPropertyChanged);
                this.MaxVersionComboBox.DataBindings.Clear();//.Add("Text", this._dataCodeParam, nameof(this._dataCodeParam.version_max), true, DataSourceUpdateMode.OnPropertyChanged);
                this.MinSymbolSizeComboBox.DataBindings.Clear();//.Add("Text", this._dataCodeParam, nameof(this._dataCodeParam.symbol_size_min), true, DataSourceUpdateMode.OnPropertyChanged);
                this.MaxSymbolSizeComboBox.DataBindings.Clear();//.Add("Text", this._dataCodeParam, nameof(this._dataCodeParam.symbol_size_max), true, DataSourceUpdateMode.OnPropertyChanged);
                this.MinContrastComboBox.DataBindings.Clear();//.Add("Text", this._dataCodeParam, nameof(this._dataCodeParam.contrast_min), true, DataSourceUpdateMode.OnPropertyChanged);
                this.MinModuleSizeComboBox.DataBindings.Clear();//.Add("Text", this._dataCodeParam, nameof(this._dataCodeParam.module_size_min), true, DataSourceUpdateMode.OnPropertyChanged);
                this.MaxModuleSizeComboBox.DataBindings.Clear();//.Add("Text", this._dataCodeParam, nameof(this._dataCodeParam.module_size_max), true, DataSourceUpdateMode.OnPropertyChanged);
                this.MinModuleGapComboBox.DataBindings.Clear();//.Add("Text", this._dataCodeParam, nameof(this._dataCodeParam.module_gap_min), true, DataSourceUpdateMode.OnPropertyChanged);
                this.MaxModuleGapComboBox.DataBindings.Clear();//.Add("Text", this._dataCodeParam, nameof(this._dataCodeParam.module_gap_max), true, DataSourceUpdateMode.OnPropertyChanged);
                this.SmallModulesRobustnessComboBox.DataBindings.Clear();//.Add("Text", this._dataCodeParam, nameof(this._dataCodeParam.small_modules_robustness), true, DataSourceUpdateMode.OnPropertyChanged);
                this.MinPositionPatternComboBox.DataBindings.Clear();//.Add("Text", this._dataCodeParam, nameof(this._dataCodeParam.position_pattern_min), true, DataSourceUpdateMode.OnPropertyChanged);
                this.QualityComboBox.DataBindings.Clear();//.Add("Text", this._dataCodeParam, nameof(this._dataCodeParam.quality_isoiec15415_aperture_size), true, DataSourceUpdateMode.OnPropertyChanged);
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
            }

        }
        private void 默认参数comboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            try
            {
                if (this.默认参数comboBox.SelectedItem == null) return;
                if (this.默认参数comboBox.SelectedItem.ToString() != this._dataCodeParam.default_parameters)
                {
                    DialogResult dialogResult = MessageBox.Show("修改该参数，将重置模型的所有相机参数，确认继续吗?", "重置模型参数", MessageBoxButtons.OKCancel);
                    if (dialogResult == DialogResult.OK)
                    {
                        this._dataCodeParam.default_parameters = this.默认参数comboBox.SelectedItem.ToString();
                        if (this._Detection.Reset_data_code_2d_param())
                        {
                            MessageBox.Show("参数重置成功!");
                            this.ClearBindProperty();
                            this.BindProperty();
                        }
                        else
                        {
                            MessageBox.Show("参数重置失败!");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

    }
}

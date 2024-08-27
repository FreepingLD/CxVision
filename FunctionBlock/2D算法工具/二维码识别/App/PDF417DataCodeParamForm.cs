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
    public partial class PDF417DataCodeParamForm : Form
    {
        private DoDataCodeDetection _Detection;
        private PDF417DataCodeParam _dataCodeParam;
        public PDF417DataCodeParamForm(DataCodeParam Params)
        {
            this._dataCodeParam = Params as PDF417DataCodeParam;
            InitializeComponent();
            BindProperty();
        }
        public PDF417DataCodeParamForm(DoDataCodeDetection detection)
        {
            this._Detection = detection;
            this._dataCodeParam = detection.CodeParam as PDF417DataCodeParam;
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
                this.MinSymbolColsComboBox.DataBindings.Add("Text", this._dataCodeParam, nameof(this._dataCodeParam.symbol_cols_min), true, DataSourceUpdateMode.OnPropertyChanged);
                this.MaxSymbolColsComboBox.DataBindings.Add("Text", this._dataCodeParam, nameof(this._dataCodeParam.symbol_cols_max), true, DataSourceUpdateMode.OnPropertyChanged);
                this.MinSymbolRowsComboBox.DataBindings.Add("Text", this._dataCodeParam, nameof(this._dataCodeParam.symbol_rows_min), true, DataSourceUpdateMode.OnPropertyChanged);
                this.MaxSymbolRowsComboBox.DataBindings.Add("Text", this._dataCodeParam, nameof(this._dataCodeParam.symbol_rows_max), true, DataSourceUpdateMode.OnPropertyChanged);
                this.MinContrastComboBox.DataBindings.Add("Text", this._dataCodeParam, nameof(this._dataCodeParam.contrast_min), true, DataSourceUpdateMode.OnPropertyChanged);
                this.MinModuleWidthComboBox.DataBindings.Add("Text", this._dataCodeParam, nameof(this._dataCodeParam.module_width_min), true, DataSourceUpdateMode.OnPropertyChanged);
                this.MaxModuleWidthComboBox.DataBindings.Add("Text", this._dataCodeParam, nameof(this._dataCodeParam.module_width_max), true, DataSourceUpdateMode.OnPropertyChanged);
                this.MinModuleAspectComboBox.DataBindings.Add("Text", this._dataCodeParam, nameof(this._dataCodeParam.module_aspect_min), true, DataSourceUpdateMode.OnPropertyChanged);
                this.MaxModuleAspectComboBox.DataBindings.Add("Text", this._dataCodeParam, nameof(this._dataCodeParam.module_aspect_max), true, DataSourceUpdateMode.OnPropertyChanged);
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
                this.默认参数comboBox.DataBindings.Clear();//.Add("Text", this._dataCodeParam, nameof(this._dataCodeParam.default_parameters), true, DataSourceUpdateMode.OnPropertyChanged);
                this.MinSymbolColsComboBox.DataBindings.Clear();//.Add("Text", this._dataCodeParam, nameof(this._dataCodeParam.symbol_cols_min), true, DataSourceUpdateMode.OnPropertyChanged);
                this.MaxSymbolColsComboBox.DataBindings.Clear();//.Add("Text", this._dataCodeParam, nameof(this._dataCodeParam.symbol_cols_max), true, DataSourceUpdateMode.OnPropertyChanged);
                this.MinSymbolRowsComboBox.DataBindings.Clear();//.Add("Text", this._dataCodeParam, nameof(this._dataCodeParam.symbol_rows_min), true, DataSourceUpdateMode.OnPropertyChanged);
                this.MaxSymbolRowsComboBox.DataBindings.Clear();//.Add("Text", this._dataCodeParam, nameof(this._dataCodeParam.symbol_rows_max), true, DataSourceUpdateMode.OnPropertyChanged);
                this.MinContrastComboBox.DataBindings.Clear();//.Add("Text", this._dataCodeParam, nameof(this._dataCodeParam.contrast_min), true, DataSourceUpdateMode.OnPropertyChanged);
                this.MinModuleWidthComboBox.DataBindings.Clear();//.Add("Text", this._dataCodeParam, nameof(this._dataCodeParam.module_width_min), true, DataSourceUpdateMode.OnPropertyChanged);
                this.MaxModuleWidthComboBox.DataBindings.Clear();//.Add("Text", this._dataCodeParam, nameof(this._dataCodeParam.module_width_max), true, DataSourceUpdateMode.OnPropertyChanged);
                this.MinModuleAspectComboBox.DataBindings.Clear();//.Add("Text", this._dataCodeParam, nameof(this._dataCodeParam.module_aspect_min), true, DataSourceUpdateMode.OnPropertyChanged);
                this.MaxModuleAspectComboBox.DataBindings.Clear();//.Add("Text", this._dataCodeParam, nameof(this._dataCodeParam.module_aspect_max), true, DataSourceUpdateMode.OnPropertyChanged);
                this.QualityComboBox.DataBindings.Add("Text", this._dataCodeParam, nameof(this._dataCodeParam.quality_isoiec15415_aperture_size), true, DataSourceUpdateMode.OnPropertyChanged);
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
            }

        }
        private void label10_Click(object sender, EventArgs e)
        {

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

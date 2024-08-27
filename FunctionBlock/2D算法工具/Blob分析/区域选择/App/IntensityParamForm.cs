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
    public partial class IntensityParamForm : Form
    {
        private SelectParamValue _param;
        public IntensityParamForm(SelectParamValue param)
        {
            this._param = param;
            InitializeComponent();
            BindProperty();
        }

        private void BindProperty()
        {
            try
            {
                // 创建匹配参数
                this.特征ComboBox.Items.Clear();
                this.特征ComboBox.DataSource = Enum.GetValues(typeof(enSelectIntensityFeatures));
                this.特征ComboBox.DataBindings.Add(nameof(this.特征ComboBox.Text), ((IntensityParam)this._param), "IntensityFeatures", true, DataSourceUpdateMode.OnPropertyChanged);
                this.操作comboBox.DataBindings.Add(nameof(this.操作comboBox.Text), ((IntensityParam)this._param), "Operation", true, DataSourceUpdateMode.OnPropertyChanged);
                this.平均灰度ComboBox.DataBindings.Add(nameof(this.平均灰度ComboBox.Text), ((IntensityParam)this._param), "Mean", true, DataSourceUpdateMode.OnPropertyChanged);
                this.标准差comboBox.DataBindings.Add(nameof(this.标准差comboBox.Text), ((IntensityParam)this._param), "Deviation", true, DataSourceUpdateMode.OnPropertyChanged);
                //this.合并区域checkBox.DataBindings.Add(nameof(this.合并区域checkBox.Checked), ((SelectShapeStdParam)this._param), "IsUnion", true, DataSourceUpdateMode.OnPropertyChanged);
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
            }
        }

        private void IntensityParamForm_Load(object sender, EventArgs e)
        {

        }
    }
}

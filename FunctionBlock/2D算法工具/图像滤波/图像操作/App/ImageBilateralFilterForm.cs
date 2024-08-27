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
    public partial class ImageBilateralFilterForm : Form
    {
        private ImageFilterParam _param;
        public ImageBilateralFilterForm(ImageFilterParam param)
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
                this.高斯大小ComboBox.DataBindings.Add(nameof(this.高斯大小ComboBox.Text), ((BilateralFilterParam)this._param), "SigmaSpatial", true, DataSourceUpdateMode.OnPropertyChanged);
                this.高斯范围comboBox.DataBindings.Add(nameof(this.高斯范围comboBox.Text), ((BilateralFilterParam)this._param), "SigmaRange", true, DataSourceUpdateMode.OnPropertyChanged);
                this.迭代次数comboBox.DataBindings.Add(nameof(this.迭代次数comboBox.Text), ((BilateralFilterParam)this._param), "Count", true, DataSourceUpdateMode.OnPropertyChanged);
                this.参数名称comboBox.DataBindings.Add(nameof(this.参数名称comboBox.Text), ((BilateralFilterParam)this._param), "GenParamName", true, DataSourceUpdateMode.OnPropertyChanged);
                this.参数值comboBox.DataBindings.Add(nameof(this.参数值comboBox.Text), ((BilateralFilterParam)this._param), "GenParamValue", true, DataSourceUpdateMode.OnPropertyChanged);
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
            }
        }
    }
}

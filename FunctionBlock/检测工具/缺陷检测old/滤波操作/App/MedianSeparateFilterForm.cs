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
    public partial class MedianSeparateFilterForm : Form
    {
        private FilterParam _param;
        public MedianSeparateFilterForm(FilterParam param)
        {
            this._param = param;
            InitializeComponent();
            BindProperty();
            this.AutoScroll = true;
        }

        private void BindProperty()
        {
            try
            {
                // 创建匹配参数
                MedianSeparateFilterParam filterParam = ((MedianSeparateFilterParam)this._param);
                this.掩膜宽度ComboBox .DataBindings.Add(nameof(this.掩膜宽度ComboBox.Text), filterParam, nameof(filterParam.MaskWidth), true, DataSourceUpdateMode.OnPropertyChanged);
                this.掩膜高度ComboBox.DataBindings.Add(nameof(this.掩膜高度ComboBox.Text), filterParam, nameof(filterParam.MaskHeight), true, DataSourceUpdateMode.OnPropertyChanged);      
                this.边界处理comboBox.DataBindings.Add(nameof(this.边界处理comboBox.Text), filterParam, nameof(filterParam.Margin), true, DataSourceUpdateMode.OnPropertyChanged);
                this.启用滤波checkBox.DataBindings.Add(nameof(this.启用滤波checkBox.Checked), filterParam, nameof(filterParam.IsFilter), true, DataSourceUpdateMode.OnPropertyChanged);
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
            }
        }
    }
}

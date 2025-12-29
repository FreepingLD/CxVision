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
    public partial class BinomialFilterForm : Form
    {
        private FilterParam _param;
        public BinomialFilterForm(FilterParam param)
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
                this.宽度ComboBox.DataBindings.Add(nameof(this.宽度ComboBox.Text), ((BinomialFilterParam)this._param), "MaskWidth", true, DataSourceUpdateMode.OnPropertyChanged);
                this.高度ComboBox.DataBindings.Add(nameof(this.高度ComboBox.Text), ((BinomialFilterParam)this._param), "MaskHeight", true, DataSourceUpdateMode.OnPropertyChanged);
                this.启用滤波checkBox.DataBindings.Add(nameof(this.启用滤波checkBox.Checked), (BinomialFilterParam)this._param, "IsFilter", true, DataSourceUpdateMode.OnPropertyChanged);
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
            }
        }


    }
}

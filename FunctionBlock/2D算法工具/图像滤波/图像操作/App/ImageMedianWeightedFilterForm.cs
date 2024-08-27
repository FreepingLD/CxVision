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
    public partial class ImageMedianWeightedFilterForm : Form
    {
        private ImageFilterParam _param;
        public ImageMedianWeightedFilterForm(ImageFilterParam param)
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
                MedianWeightedFilterParam filterParam = ((MedianWeightedFilterParam)this._param);
                this.掩膜类型ComboBox .DataBindings.Add(nameof(this.掩膜类型ComboBox.Text), filterParam, nameof(filterParam.MaskType), true, DataSourceUpdateMode.OnPropertyChanged);
                this.掩膜尺寸ComboBox.DataBindings.Add(nameof(this.掩膜尺寸ComboBox.Text), filterParam, nameof(filterParam.MaskSize), true, DataSourceUpdateMode.OnPropertyChanged);      
                //this.边界处理comboBox.DataBindings.Add(nameof(this.边界处理comboBox.Text), filterParam, nameof(filterParam.Margin), true, DataSourceUpdateMode.OnPropertyChanged);
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
            }
        }
    }
}

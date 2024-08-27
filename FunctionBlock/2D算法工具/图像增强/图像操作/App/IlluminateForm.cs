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
    public partial class IlluminateForm : Form
    {
        private IlluminateParam _param;
        public IlluminateForm(ImageEnhancementParam param)
        {
            this._param = param as IlluminateParam;
            InitializeComponent();
            BindProperty();
        }

        private void BindProperty()
        {
            try
            {
                // 创建匹配参数
                IlluminateParam filterParam = ((IlluminateParam)this._param);
                this.掩膜宽度ComboBox.DataBindings.Add(nameof(this.掩膜宽度ComboBox.Text), filterParam, nameof(filterParam.MaskWidth), true, DataSourceUpdateMode.OnPropertyChanged);
                this.掩膜高度ComboBox.DataBindings.Add(nameof(this.掩膜高度ComboBox.Text), filterParam, nameof(filterParam.MaskHeight), true, DataSourceUpdateMode.OnPropertyChanged);
                this.因子comboBox.DataBindings.Add(nameof(this.掩膜高度ComboBox.Text), filterParam, nameof(filterParam.Factor), true, DataSourceUpdateMode.OnPropertyChanged);
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
            }
        }
    }
}

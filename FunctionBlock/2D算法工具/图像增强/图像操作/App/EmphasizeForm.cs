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
    public partial class EmphasizeForm : Form
    {
        private EmphasizeParam _param;
        public EmphasizeForm(ImageEnhancementParam param)
        {
            this._param = param as EmphasizeParam;
            InitializeComponent();
            BindProperty();
        }

        private void BindProperty()
        {
            try
            {
                // 创建匹配参数
                this.掩膜宽度ComboBox.DataBindings.Add(nameof(this.掩膜宽度ComboBox.Text), this._param, nameof(this._param.MaskWidth), true, DataSourceUpdateMode.OnPropertyChanged);
                this.掩膜高度ComboBox.DataBindings.Add(nameof(this.掩膜高度ComboBox.Text), this._param, nameof(this._param.MaskHeight), true, DataSourceUpdateMode.OnPropertyChanged);
                this.因子comboBox.DataBindings.Add(nameof(this.掩膜高度ComboBox.Text), this._param, nameof(this._param.Factor), true, DataSourceUpdateMode.OnPropertyChanged);
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
            }
        }
    }
}

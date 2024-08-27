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
    public partial class ThresholdSubPixForm : Form
    {
        IFunction _function;
        public ThresholdSubPixForm(IFunction function)
        {
            this._function = function;
            InitializeComponent();
            BindProperty();
        }

        private void BindProperty()
        {
            try
            {
                // 创建匹配参数
                this.阈值textBox.DataBindings.Add("Text", ((ExtractXLD)this._function).Threshold_sub_pix, "Threshold", true, DataSourceUpdateMode.OnPropertyChanged);

            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
            }
        }
    }
}

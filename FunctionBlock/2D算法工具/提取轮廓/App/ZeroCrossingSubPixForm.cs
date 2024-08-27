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
    public partial class ZeroCrossingSubPixForm : Form
    {
        IFunction _function;
        public ZeroCrossingSubPixForm(IFunction function)
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
               this.平滑阈值textBox.DataBindings.Add("Text", ((ExtractXLD)this._function).Zero_crossing_sub_pix, "LaplaceSigma", true, DataSourceUpdateMode.OnPropertyChanged);
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
            }
        }


    }
}

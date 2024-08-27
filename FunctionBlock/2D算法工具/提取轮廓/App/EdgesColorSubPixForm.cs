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
    public partial class EdgesColorSubPixForm : Form
    {
        IFunction _function;
        public EdgesColorSubPixForm(IFunction function)
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
                this.滤波器comboBox.DataSource = Enum.GetNames(typeof(EdgesColorSubPix.enEdgesColorSubPixFilterParam));
                this.滤波器comboBox.DataBindings.Add("Text", ((ExtractXLD)this._function).Edges_color_sub_pix, "Filter", true, DataSourceUpdateMode.OnPropertyChanged);
                this.透明度textBox.DataBindings.Add("Text", ((ExtractXLD)this._function).Edges_color_sub_pix, "Alpha", true, DataSourceUpdateMode.OnPropertyChanged);
                this.低阈值textBox.DataBindings.Add("Text", ((ExtractXLD)this._function).Edges_color_sub_pix, "Low", true, DataSourceUpdateMode.OnPropertyChanged);
                this.高阈值textBox.DataBindings.Add("Text", ((ExtractXLD)this._function).Edges_color_sub_pix, "High", true, DataSourceUpdateMode.OnPropertyChanged);
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
            }
        }
    }
}

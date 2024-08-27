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
    public partial class LinesGaussForm : Form
    {
        IFunction _function;
        public LinesGaussForm(IFunction function)
        {
            this._function = function;
            InitializeComponent();
            BindProperty();
        }
        private void BindProperty()
        {
            try
            {
                // 绑定参数数据
                this.LightOrDarkcomboBox.DataSource = LinesGauss.LightDarkParam;
                this.ExtractWidth_comboBox.DataSource = LinesGauss.ExtractWidthParam;
                this.LineModel_comboBox.DataSource = LinesGauss.LineModelParam;
                this.CompleteJunctions_comboBox.DataSource = LinesGauss.CompleteJunctionsParam;
                ///////////
                this.LightOrDarkcomboBox.DataBindings.Add("Text", ((ExtractXLD)this._function).Lines_gauss, "LightDark", true, DataSourceUpdateMode.OnPropertyChanged);
                this.ExtractWidth_comboBox.DataBindings.Add("Text", ((ExtractXLD)this._function).Lines_gauss, "ExtractWidth", true, DataSourceUpdateMode.OnPropertyChanged);
                this.LineModel_comboBox.DataBindings.Add("Text", ((ExtractXLD)this._function).Lines_gauss, "LineModel", true, DataSourceUpdateMode.OnPropertyChanged);
                this.CompleteJunctions_comboBox.DataBindings.Add("Text", ((ExtractXLD)this._function).Lines_gauss, "CompleteJunctions", true, DataSourceUpdateMode.OnPropertyChanged);
                /////////////////////
                this.Sigma_textBox.DataBindings.Add("Text", ((ExtractXLD)this._function).Lines_gauss, "Sigma", true, DataSourceUpdateMode.OnPropertyChanged);
                this.低阈值textBox.DataBindings.Add("Text", ((ExtractXLD)this._function).Lines_gauss, "Low", true, DataSourceUpdateMode.OnPropertyChanged);
                this.高阈值textBox.DataBindings.Add("Text", ((ExtractXLD)this._function).Lines_gauss, "High", true, DataSourceUpdateMode.OnPropertyChanged);
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
            }
        }


    }
}

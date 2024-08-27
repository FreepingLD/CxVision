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
    public partial class CreateNccModelParamForm : Form
    {
       private CreateNccModelParam _nccModelParam;
        public CreateNccModelParamForm(CreateNccModelParam param)
        {
            this._nccModelParam = param;
            InitializeComponent();
            BindProperty();
        }


        private void BindProperty()
        {
            try
            {
                this.金字塔层级comboBox.DataBindings.Add("Text", this._nccModelParam, "NumLevels", true, DataSourceUpdateMode.OnPropertyChanged);
                this.起始角度textBox.DataBindings.Add("Text", this._nccModelParam, "AngleStart", true, DataSourceUpdateMode.OnPropertyChanged);
                this.终止角度textBox.DataBindings.Add("Text", this._nccModelParam, "AngleExtent", true, DataSourceUpdateMode.OnPropertyChanged);
                this.角度步长comboBox.DataBindings.Add("Text", this._nccModelParam, "AngleStep", true, DataSourceUpdateMode.OnPropertyChanged);
                this.极性comboBox.DataBindings.Add("Text", this._nccModelParam, "Metric", true, DataSourceUpdateMode.OnPropertyChanged);
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
            }
        }


        private void CShapeModelForm_Load(object sender, EventArgs e)
        {

        }
    }
}

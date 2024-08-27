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
    public partial class CreateCylinderForm : Form
    {
        private CylinderPrimitiveParam  _param;
        public CreateCylinderForm(PrimitiveParam param)
        {
            this._param = param as CylinderPrimitiveParam;
            InitializeComponent();
            BindProperty();
        }
        private void BindProperty()
        {
            try
            {
                // 创建匹配参数
                this.半径comboBox.DataBindings.Add("Text", this._param, "Radius", true, DataSourceUpdateMode.OnPropertyChanged);
                this.最小长度comboBox.DataBindings.Add("Text", this._param, "MinExtent", true, DataSourceUpdateMode.OnPropertyChanged);
                this.最大长度comboBox.DataBindings.Add("Text", this._param, "MaxExtent", true, DataSourceUpdateMode.OnPropertyChanged);
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
            }
        }
    }
}

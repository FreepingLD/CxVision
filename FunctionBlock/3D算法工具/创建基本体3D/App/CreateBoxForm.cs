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
    public partial class CreateBoxForm : Form
    {
        private BoxPrimitiveParam _param;
        public CreateBoxForm(PrimitiveParam param)
        {
            this._param = param as BoxPrimitiveParam;
            InitializeComponent();
            BindProperty();
        }

        private void BindProperty()
        {
            try
            {
                // 创建匹配参数
                this.X长度comboBox.DataBindings.Add("Text", this._param, "Length_x", true, DataSourceUpdateMode.OnPropertyChanged);
                this.Y长度comboBox.DataBindings.Add("Text", this._param, "Length_y", true, DataSourceUpdateMode.OnPropertyChanged);
                this.Z长度comboBox.DataBindings.Add("Text", this._param, "Length_z", true, DataSourceUpdateMode.OnPropertyChanged);
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
            }
        }
    }
}

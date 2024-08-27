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
    public partial class CreateSphereForm : Form
    {
        private SpherePrimitiveParam _param;
        public CreateSphereForm(PrimitiveParam param)
        {
            this._param = param as SpherePrimitiveParam;
            InitializeComponent();
            BindProperty();
        }
        private void BindProperty()
        {
            try
            {
                // 创建匹配参数
                this.半径comboBox.DataBindings.Add("Text", this._param, "Radius", true, DataSourceUpdateMode.OnPropertyChanged);
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
            }
        }



    }
}

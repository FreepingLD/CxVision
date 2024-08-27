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
    public partial class CreatePlaneForm : Form
    {
        private  PlanePrimitiveParam _param;
        public CreatePlaneForm(PrimitiveParam param)
        {
            this._param = param as PlanePrimitiveParam;
            InitializeComponent();
            BindProperty();
        }
        private void BindProperty()
        {
            try
            {
                // 创建匹配参数
                this.最小XcomboBox.DataBindings.Add("Text", _param, "Min_xExtent", true, DataSourceUpdateMode.OnPropertyChanged);
                this.最大XomboBox.DataBindings.Add("Text", _param, "Max_xExtent", true, DataSourceUpdateMode.OnPropertyChanged);
                this.最小YomboBox.DataBindings.Add("Text", _param, "Min_yExtent", true, DataSourceUpdateMode.OnPropertyChanged);
                this.最大YcomboBox.DataBindings.Add("Text", _param, "Max_yExtent", true, DataSourceUpdateMode.OnPropertyChanged);
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
            }
        }


    }
}

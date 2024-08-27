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
    public partial class SelectShapeStdParamForm : Form
    {
        private SelectParamValue _param;
        public SelectShapeStdParamForm(SelectParamValue param)
        {
            this._param = param;
            InitializeComponent();
            BindProperty();
        }

        private void BindProperty()
        {
            try
            {
                // 创建匹配参数
                this.特征ComboBox.DataBindings.Add(nameof(this.特征ComboBox.Text), ((SelectShapeStdParam)this._param), "StdFeatures", true, DataSourceUpdateMode.OnPropertyChanged);
                this.百分比comboBox.DataBindings.Add(nameof(this.百分比comboBox.Text), ((SelectShapeStdParam)this._param), "Percent", true, DataSourceUpdateMode.OnPropertyChanged);
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
            }
        }


    }
}

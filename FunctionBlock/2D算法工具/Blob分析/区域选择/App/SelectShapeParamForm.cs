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
    public partial class SelectShapeParamForm : Form
    {
        private SelectParamValue _param;
        public SelectShapeParamForm(SelectParamValue param)
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
                this.特征ComboBox.DataBindings.Add(nameof(this.特征ComboBox.Text), ((SelectShapeParam)this._param), "Features", true, DataSourceUpdateMode.OnPropertyChanged);
                this.操作comboBox.DataBindings.Add(nameof(this.操作comboBox.Text), ((SelectShapeParam)this._param), "Operation", true, DataSourceUpdateMode.OnPropertyChanged);
                this.最小值comboBox.DataBindings.Add(nameof(this.最小值comboBox.Text), ((SelectShapeParam)this._param), "Min", true, DataSourceUpdateMode.OnPropertyChanged);
                this.最大值comboBox.DataBindings.Add(nameof(this.最大值comboBox.Text), ((SelectShapeParam)this._param), "Max", true, DataSourceUpdateMode.OnPropertyChanged);
                //this.合并区域checkBox.DataBindings.Add(nameof(this.合并区域checkBox.Checked), ((SelectShapeParam)this._param), "IsUnion", true, DataSourceUpdateMode.OnPropertyChanged);
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
            }
        }
    }
}

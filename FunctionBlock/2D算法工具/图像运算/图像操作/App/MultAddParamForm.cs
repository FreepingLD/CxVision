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
    public partial class MultAddParamForm : Form
    {
        private MultAddParam _param;
        public MultAddParamForm(ImageArithmeticParam param)
        {
            this._param = param as MultAddParam;
            InitializeComponent();
            BindProperty();
        }

        private void BindProperty()
        {
            try
            {
                // 创建匹配参数
                this.MultComboBox.DataBindings.Add(nameof(this.MultComboBox.Text), (this._param), nameof(this._param.Mult), true, DataSourceUpdateMode.OnPropertyChanged);
                this.Add值comboBox.DataBindings.Add(nameof(this.Add值comboBox.Text), (this._param), nameof(this._param.Add), true, DataSourceUpdateMode.OnPropertyChanged);
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
            }
        }
    }
}

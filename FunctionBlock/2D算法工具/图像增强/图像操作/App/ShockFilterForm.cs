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
    public partial class ShockFilterForm : Form
    {
        private ShockFilterParam _param;
        public ShockFilterForm(ImageEnhancementParam param)
        {
            this._param = param as ShockFilterParam;
            InitializeComponent();
            BindProperty();
        }

        private void BindProperty()
        {
            try
            {
                // 创建匹配参数
                ShockFilterParam filterParam = ((ShockFilterParam)this._param);
                this.时间步长ComboBox .DataBindings.Add(nameof(this.时间步长ComboBox.Text), filterParam, nameof(filterParam.Theta), true, DataSourceUpdateMode.OnPropertyChanged);
                this.边缘检测平滑ComboBox.DataBindings.Add(nameof(this.边缘检测平滑ComboBox.Text), filterParam, nameof(filterParam.Sigma), true, DataSourceUpdateMode.OnPropertyChanged);      
                this.边缘检测方式comboBox.DataBindings.Add(nameof(this.边缘检测方式comboBox.Text), filterParam, nameof(filterParam.Mode), true, DataSourceUpdateMode.OnPropertyChanged);
                this.迭代次数comboBox.DataBindings.Add(nameof(this.迭代次数comboBox.Text), filterParam, nameof(filterParam.Iterations), true, DataSourceUpdateMode.OnPropertyChanged);
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
            }
        }
    }
}

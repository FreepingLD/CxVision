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
    public partial class ScaleImageMaxForm : Form
    {
        private ScaleImageMaxParam _param;
        public ScaleImageMaxForm(ImageEnhancementParam param)
        {
            this._param = param as ScaleImageMaxParam;
            InitializeComponent();
            BindProperty();
        }

        private void BindProperty()
        {
            try
            {
                // 创建匹配参数
                this.最小值ComboBox.DataBindings.Add(nameof(this.最小值ComboBox.Text), ((ScaleImageMaxParam)this._param), "Min", true, DataSourceUpdateMode.OnPropertyChanged);
                this.最大值comboBox.DataBindings.Add(nameof(this.最大值comboBox.Text), ((ScaleImageMaxParam)this._param), "Max", true, DataSourceUpdateMode.OnPropertyChanged);
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
            }
        }
    }
}

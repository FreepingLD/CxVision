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
    public partial class EquHistoImageForm : Form
    {
        private EquHistoImageParam _param;
        public EquHistoImageForm(ImageEnhancementParam param)
        {
            this._param = param as EquHistoImageParam;
            InitializeComponent();
            BindProperty();
        }

        private void BindProperty()
        {
            try
            {
                // 创建匹配参数
                //this.高斯尺寸ComboBox.DataBindings.Add(nameof(this.高斯尺寸ComboBox.Text), ((EquHistoImageParam)this._param), "Size", true, DataSourceUpdateMode.OnPropertyChanged);
                //this.高度ComboBox.DataBindings.Add(nameof(this.高度ComboBox.Text), ((GaussFilterParam)this._param), "MaskHeight", true, DataSourceUpdateMode.OnPropertyChanged);
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
            }
        }
    }
}

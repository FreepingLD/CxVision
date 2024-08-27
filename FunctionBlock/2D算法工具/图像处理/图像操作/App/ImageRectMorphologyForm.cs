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
    public partial class ImageRectMorphologyForm : Form
    {
        private ImageMorphologyParam _param;
        public ImageRectMorphologyForm(ImageMorphologyParam param)
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
                this.宽度ComboBox.DataBindings.Add(nameof(this.宽度ComboBox.Text), ((ImageRectParam)this._param), "MaskWidth", true, DataSourceUpdateMode.OnPropertyChanged);
                this.高度comboBox.DataBindings.Add(nameof(this.高度comboBox.Text), ((ImageRectParam)this._param), "MaskHeight", true, DataSourceUpdateMode.OnPropertyChanged);
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
            }
        }
    }
}

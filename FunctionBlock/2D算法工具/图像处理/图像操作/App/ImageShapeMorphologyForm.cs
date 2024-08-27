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
    public partial class ImageShapeMorphologyForm : Form
    {
        private ImageMorphologyParam _param;
        public ImageShapeMorphologyForm(ImageMorphologyParam param)
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
                this.宽度ComboBox.DataBindings.Add(nameof(this.宽度ComboBox.Text), ((ImageShapeParam)this._param), "MaskWidth", true, DataSourceUpdateMode.OnPropertyChanged);
                this.高度ComboBox.DataBindings.Add(nameof(this.高度ComboBox.Text), ((ImageShapeParam)this._param), "MaskHeight", true, DataSourceUpdateMode.OnPropertyChanged);
                this.掩膜形状comboBox.DataBindings.Add(nameof(this.掩膜形状comboBox.Text), ((ImageShapeParam)this._param), "MaskShape", true, DataSourceUpdateMode.OnPropertyChanged);
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
            }
        }
    }
}

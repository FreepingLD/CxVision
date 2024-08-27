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
    public partial class ZoomImageFactorForm : Form
    {
        private ZoomImageFactorParam _param;
        public ZoomImageFactorForm(ImageZoomParam param)
        {
            this._param = param as ZoomImageFactorParam;
            InitializeComponent();
            BindProperty();
        }

        private void BindProperty()
        {
            try
            {
                // 创建匹配参数
                this.插值comboBox.Items.Clear();
                foreach (var item in Enum.GetNames(typeof(enInterpolationType)))
                {
                    this.插值comboBox.Items.Add(item);
                }
                this.宽度缩放ComboBox.DataBindings.Add(nameof(this.宽度缩放ComboBox.Text), this._param, nameof(this._param.ScaleWidth), true, DataSourceUpdateMode.OnPropertyChanged);
                this.高度缩放ComboBox.DataBindings.Add(nameof(this.高度缩放ComboBox.Text), this._param, nameof(this._param.ScaleHeight), true, DataSourceUpdateMode.OnPropertyChanged);
                this.插值comboBox.DataBindings.Add(nameof(this.高度缩放ComboBox.Text), this._param, nameof(this._param.Interpolation), true, DataSourceUpdateMode.OnPropertyChanged);
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
            }
        }
    }
}

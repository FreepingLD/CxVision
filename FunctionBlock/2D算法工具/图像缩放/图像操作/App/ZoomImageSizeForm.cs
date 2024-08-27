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
    public partial class ZoomImageSizeForm : Form
    {
        private ZoomImageSizeParam _param;
        public ZoomImageSizeForm(ImageZoomParam param)
        {
            this._param = param as ZoomImageSizeParam;
            InitializeComponent();
            BindProperty();
        }

        private void BindProperty()
        {
            try
            {
                // 创建匹配参数
                ZoomImageSizeParam filterParam = ((ZoomImageSizeParam)this._param);
                this.图像宽度ComboBox.DataBindings.Add(nameof(this.图像宽度ComboBox.Text), filterParam, nameof(filterParam.ImageWidth), true, DataSourceUpdateMode.OnPropertyChanged);
                this.图像高度ComboBox.DataBindings.Add(nameof(this.图像高度ComboBox.Text), filterParam, nameof(filterParam.ImageHeight), true, DataSourceUpdateMode.OnPropertyChanged);
                this.插值方式comboBox.DataBindings.Add(nameof(this.图像高度ComboBox.Text), filterParam, nameof(filterParam.Interpolation), true, DataSourceUpdateMode.OnPropertyChanged);
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
            }
        }
    }
}

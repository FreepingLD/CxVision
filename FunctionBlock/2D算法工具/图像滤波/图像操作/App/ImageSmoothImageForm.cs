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
    public partial class ImageSmoothImageForm : Form
    {
        private ImageFilterParam _param;
        public ImageSmoothImageForm(ImageFilterParam param)
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
                SmoothImageParam filterParam = ((SmoothImageParam)this._param);
                this.滤波类型ComboBox .DataBindings.Add(nameof(this.滤波类型ComboBox.Text), filterParam, nameof(filterParam.Filter), true, DataSourceUpdateMode.OnPropertyChanged);
                this.透明度ComboBox.DataBindings.Add(nameof(this.透明度ComboBox.Text), filterParam, nameof(filterParam.Alpha), true, DataSourceUpdateMode.OnPropertyChanged);      
                //this.边界处理comboBox.DataBindings.Add(nameof(this.边界处理comboBox.Text), filterParam, nameof(filterParam.Margin), true, DataSourceUpdateMode.OnPropertyChanged);
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
            }
        }
    }
}

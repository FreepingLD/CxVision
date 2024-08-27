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
    public partial class ImageGuidedFilterForm : Form
    {
        private ImageFilterParam _param;
        public ImageGuidedFilterForm(ImageFilterParam param)
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
                GuidedFilterParam filterParam = ((GuidedFilterParam)this._param);
                this.半径ComboBox.DataBindings.Add(nameof(this.半径ComboBox.Text), filterParam, nameof(filterParam.Radius), true, DataSourceUpdateMode.OnPropertyChanged);
                this.振幅ComboBox.DataBindings.Add(nameof(this.振幅ComboBox.Text), filterParam, nameof(filterParam.Amplitude), true, DataSourceUpdateMode.OnPropertyChanged);
                this.迭代次数comboBox.DataBindings.Add(nameof(this.振幅ComboBox.Text), filterParam, nameof(filterParam.Count), true, DataSourceUpdateMode.OnPropertyChanged);
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
            }
        }
    }
}

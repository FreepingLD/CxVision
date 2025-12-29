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
    public partial class ThresholdDetectForm : Form
    {
        private ThresholParam _param;
        public ThresholdDetectForm(ThresholParam param)
        {
            this._param = param;
            InitializeComponent();
            BindProperty();
            this.AutoScroll = true;
        }

        private void BindProperty()
        {
            try
            {
                // 创建匹配参数
                this.最小灰度值comboBox.DataBindings.Add("Text", ((ThresholdParam)this._param), "MinThreshold", true, DataSourceUpdateMode.OnPropertyChanged);
                this.最大灰度值comboBox.DataBindings.Add("Text", ((ThresholdParam)this._param), "MaxThreshold", true, DataSourceUpdateMode.OnPropertyChanged);
                this.最小面积comboBox.DataBindings.Add("Text", ((ThresholdParam)this._param), "MinArea", true, DataSourceUpdateMode.OnPropertyChanged);
                this.操作comboBox.DataBindings.Add("Text", ((ThresholdParam)this._param), "Operate", true, DataSourceUpdateMode.OnPropertyChanged);
                this.启用检测checkBox.DataBindings.Add(nameof(this.启用检测checkBox.Checked), (ThresholdParam)this._param, "IsDetect", true, DataSourceUpdateMode.OnPropertyChanged);
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
            }
        }

        private void ThresholdForm_Load(object sender, EventArgs e)
        {

        }
    }
}

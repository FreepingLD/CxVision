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
    public partial class CharThresholdDetectForm : Form
    {
        private ThresholParam _param;
        public CharThresholdDetectForm(ThresholParam param)
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
                this.SigmaComboBox.DataBindings.Add("Text", ((CharThresholdParam)this._param), "ChartSigma", true, DataSourceUpdateMode.OnPropertyChanged);
                this.百分比comboBox.DataBindings.Add("Text", ((CharThresholdParam)this._param), "ChartPercent", true, DataSourceUpdateMode.OnPropertyChanged);
                this.最小面积comboBox.DataBindings.Add("Text", ((CharThresholdParam)this._param), "MinArea", true, DataSourceUpdateMode.OnPropertyChanged);
                this.启用检测checkBox.DataBindings.Add(nameof(this.启用检测checkBox.Checked), (CharThresholdParam)this._param, "IsDetect", true, DataSourceUpdateMode.OnPropertyChanged);
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
            }
        }
    }
}

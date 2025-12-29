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
    public partial class AutoThresholdDetectForm : Form
    {
        private ThresholParam _param;
        public AutoThresholdDetectForm(ThresholParam param)
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
                this.SigmaComboBox.DataBindings.Add(nameof(this.SigmaComboBox.Text), ((AutoThresholdParam)this._param), "AutoSigma", true, DataSourceUpdateMode.OnPropertyChanged);
                this.启用检测checkBox.DataBindings.Add(nameof(this.启用检测checkBox.Checked), (AutoThresholdParam)this._param, "IsDetect", true, DataSourceUpdateMode.OnPropertyChanged);
                this.最小面积comboBox.DataBindings.Add("Text", ((AutoThresholdParam)this._param), "MinArea", true, DataSourceUpdateMode.OnPropertyChanged);
                //this.区域连通comboBox.DataBindings.Add("Text", ((AutoThresholdParam)this._param), "IsConnection", true, DataSourceUpdateMode.OnPropertyChanged);
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
            }
        }
    }
}

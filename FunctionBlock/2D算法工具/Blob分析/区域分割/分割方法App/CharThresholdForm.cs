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
    public partial class CharThresholdForm : Form
    {
        private SegmentParam _param;
        public CharThresholdForm(SegmentParam param)
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
                this.SigmaComboBox.DataBindings.Add("Text", ((CharThresholdBlob)this._param), "ChartSigma", true, DataSourceUpdateMode.OnPropertyChanged);
                this.百分比comboBox.DataBindings.Add("Text", ((CharThresholdBlob)this._param), "ChartPercent", true, DataSourceUpdateMode.OnPropertyChanged);
                this.区域填充comboBox.DataBindings.Add("Text", ((CharThresholdBlob)this._param), "IsFill", true, DataSourceUpdateMode.OnPropertyChanged);
                this.区域连通comboBox.DataBindings.Add("Text", ((CharThresholdBlob)this._param), "IsConnection", true, DataSourceUpdateMode.OnPropertyChanged);
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
            }
        }
    }
}

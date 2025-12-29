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
    public partial class BinaryThresholdDetectForm : Form
    {
       private ThresholParam _param;
        public BinaryThresholdDetectForm(ThresholParam function)
        {
            this._param = function;
            InitializeComponent();
            BindProperty();
            this.AutoScroll = true;
        }

        private void BindProperty()
        {
            try
            {
                // 创建匹配参数
                this.方法comboBox.DataBindings.Add("Text", ((BinaryThresholdParam)this._param), "BinaryMethod", true, DataSourceUpdateMode.OnPropertyChanged);
                this.亮暗comboBox.DataBindings.Add("Text", ((BinaryThresholdParam)this._param), "BinaryLightDark", true, DataSourceUpdateMode.OnPropertyChanged);
                this.最小面积comboBox.DataBindings.Add("Text", ((BinaryThresholdParam)this._param), "MinArea", true, DataSourceUpdateMode.OnPropertyChanged);
                this.启用检测checkBox.DataBindings.Add(nameof(this.启用检测checkBox.Checked), (BinaryThresholdParam)this._param, "IsDetect", true, DataSourceUpdateMode.OnPropertyChanged);
                //this.区域填充comboBox.DataBindings.Add("Text", ((BinaryThresholdParam)this._param), "IsDetect", true, DataSourceUpdateMode.OnPropertyChanged);
                //this.区域连通comboBox.DataBindings.Add("Text", ((SegmentParam)this._param), "IsConnection", true, DataSourceUpdateMode.OnPropertyChanged);
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
            }
        }
    }
}

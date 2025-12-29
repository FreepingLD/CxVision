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
    public partial class CallipersDetectForm : Form
    {
       private ThresholParam _param;
        public CallipersDetectForm(ThresholParam param)
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
                CallipersInspParam param = ((CallipersInspParam)this._param);
                this.掩膜宽度comboBox.DataBindings.Add("Text", param, nameof(param.MaskWidth), true, DataSourceUpdateMode.OnPropertyChanged);
                this.掩膜高度comboBox.DataBindings.Add("Text", param, nameof(param.MaskHeight), true, DataSourceUpdateMode.OnPropertyChanged);
                this.平滑系数comboBox.DataBindings.Add("Text", param, nameof(param.Sigma), true, DataSourceUpdateMode.OnPropertyChanged);
                this.阈值comboBox.DataBindings.Add("Text", param, nameof(param.Threshold), true, DataSourceUpdateMode.OnPropertyChanged);
                this.最小面积comboBox.DataBindings.Add("Text", param, "MinArea", true, DataSourceUpdateMode.OnPropertyChanged);
                this.启用检测checkBox.DataBindings.Add(nameof(this.启用检测checkBox.Checked), param, nameof(param.IsDetect), true, DataSourceUpdateMode.OnPropertyChanged);
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
            }
        }


    }
}

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
    public partial class LocalThresholdDetectForm : Form
    {
      private  ThresholParam _param;
        public LocalThresholdDetectForm(ThresholParam param)
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
                this.方法comboBox.DataBindings.Add("Text", ((LocalThresholdParam)this._param), "Method", true, DataSourceUpdateMode.OnPropertyChanged);
                this.亮暗comboBox.DataBindings.Add("Text", ((LocalThresholdParam)this._param), "LocalLightDark", true, DataSourceUpdateMode.OnPropertyChanged);
                this.参数名称comboBox.DataBindings.Add("Text", ((LocalThresholdParam)this._param), "GenParamName", true, DataSourceUpdateMode.OnPropertyChanged);
                this.参数值comboBox.DataBindings.Add("Text", ((LocalThresholdParam)this._param), "GenParamValue", true, DataSourceUpdateMode.OnPropertyChanged);
                this.最小面积comboBox.DataBindings.Add("Text", ((LocalThresholdParam)this._param), "MinArea", true, DataSourceUpdateMode.OnPropertyChanged);
                this.启用检测checkBox.DataBindings.Add(nameof(this.启用检测checkBox.Checked), (LocalThresholdParam)this._param, "IsDetect", true, DataSourceUpdateMode.OnPropertyChanged);
                //this.区域填充comboBox.DataBindings.Add("Text", ((SegmentParam)this._blobParam), "IsFill", true, DataSourceUpdateMode.OnPropertyChanged);
                //this.区域连通comboBox.DataBindings.Add("Text", ((SegmentParam)this._blobParam), "IsConnection", true, DataSourceUpdateMode.OnPropertyChanged);
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
            }
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
    }
}

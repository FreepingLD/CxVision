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
    public partial class FastThresholdDetectForm : Form
    {
       private ThresholParam _param;
        public FastThresholdDetectForm(ThresholParam param)
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
                this.最小尺寸comboBox.DataBindings.Add("Text", ((FastThresholdParam)this._param), "FastMinSize", true, DataSourceUpdateMode.OnPropertyChanged);
                this.最小灰度值comboBox.DataBindings.Add("Text", ((FastThresholdParam)this._param), "FastMinGray", true, DataSourceUpdateMode.OnPropertyChanged);
                this.最大灰度值comboBox.DataBindings.Add("Text", ((FastThresholdParam)this._param), "FastMaxGray", true, DataSourceUpdateMode.OnPropertyChanged);
                this.最小面积comboBox.DataBindings.Add("Text", ((FastThresholdParam)this._param), "MinArea", true, DataSourceUpdateMode.OnPropertyChanged);
                this.启用检测checkBox.DataBindings.Add(nameof(this.启用检测checkBox.Checked), (FastThresholdParam)this._param, "IsDetect", true, DataSourceUpdateMode.OnPropertyChanged);
                //this.区域填充comboBox.DataBindings.Add("Text", ((SegmentParam)this._blobParam), "IsFill", true, DataSourceUpdateMode.OnPropertyChanged);
                //this.区域连通comboBox.DataBindings.Add("Text", ((SegmentParam)this._blobParam), "IsConnection", true, DataSourceUpdateMode.OnPropertyChanged);
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
            }
        }

        private void FastThresholdForm_Load(object sender, EventArgs e)
        {

        }
    }
}

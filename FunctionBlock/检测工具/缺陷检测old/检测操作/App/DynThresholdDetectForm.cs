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
    public partial class DynThresholdDetectForm : Form
    {
        private ThresholParam _param;
        public DynThresholdDetectForm(ThresholParam param)
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
                this.掩膜宽度comboBox.DataBindings.Add("Text", ((DynThresholdParam)this._param), "DynMaskWidth", true, DataSourceUpdateMode.OnPropertyChanged);
                this.掩膜高度comboBox.DataBindings.Add("Text", ((DynThresholdParam)this._param), "DynMaskHeight", true, DataSourceUpdateMode.OnPropertyChanged);
                this.偏置comboBox .DataBindings.Add("Text", ((DynThresholdParam)this._param), "DynOffset", true, DataSourceUpdateMode.OnPropertyChanged);
                this.极性comboBox.DataBindings.Add("Text", ((DynThresholdParam)this._param), "DynLightDark", true, DataSourceUpdateMode.OnPropertyChanged);
                this.最小面积comboBox.DataBindings.Add("Text", ((DynThresholdParam)this._param), "MinArea", true, DataSourceUpdateMode.OnPropertyChanged);
                this.启用检测checkBox.DataBindings.Add(nameof(this.启用检测checkBox.Checked), (DynThresholdParam)this._param, "IsDetect", true, DataSourceUpdateMode.OnPropertyChanged);
                //this.区域填充comboBox.DataBindings.Add("Text", ((SegmentParam)this._blobParam), "IsFill", true, DataSourceUpdateMode.OnPropertyChanged);
                //this.区域连通comboBox.DataBindings.Add("Text", ((SegmentParam)this._blobParam), "IsConnection", true, DataSourceUpdateMode.OnPropertyChanged);
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
            }
        }
    }
}

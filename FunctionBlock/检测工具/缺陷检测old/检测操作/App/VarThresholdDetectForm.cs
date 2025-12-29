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
    public partial class VarThresholdDetectForm : Form
    {
       private ThresholParam _param;
        public VarThresholdDetectForm(ThresholParam param)
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
                this.掩膜宽度comboBox.DataBindings.Add("Text", ((VarThresholdParam)this._param), "VarMaskWidth", true, DataSourceUpdateMode.OnPropertyChanged);
                this.掩膜高度comboBox.DataBindings.Add("Text", ((VarThresholdParam)this._param), "VarMaskHeight", true, DataSourceUpdateMode.OnPropertyChanged);
                this.标准差因子comboBox.DataBindings.Add("Text", ((VarThresholdParam)this._param), "VarStdDevScale", true, DataSourceUpdateMode.OnPropertyChanged);
                this.灰度值差comboBox.DataBindings.Add("Text", ((VarThresholdParam)this._param), "VarAbsThreshold", true, DataSourceUpdateMode.OnPropertyChanged);
                this.极性comboBox.DataBindings.Add("Text", ((VarThresholdParam)this._param), "VarLightDark", true, DataSourceUpdateMode.OnPropertyChanged);
                this.最小面积comboBox.DataBindings.Add("Text", ((VarThresholdParam)this._param), "MinArea", true, DataSourceUpdateMode.OnPropertyChanged);
                this.启用检测checkBox.DataBindings.Add(nameof(this.启用检测checkBox.Checked), (VarThresholdParam)this._param, "IsDetect", true, DataSourceUpdateMode.OnPropertyChanged);
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
            }
        }


    }
}

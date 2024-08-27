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
    public partial class VarThresholdForm : Form
    {
       private SegmentParam _blobParam;
        public VarThresholdForm(SegmentParam param)
        {
            this._blobParam = param;
            InitializeComponent();
            BindProperty();
        }

        private void BindProperty()
        {
            try
            {
                // 创建匹配参数
                this.掩膜宽度comboBox.DataBindings.Add("Text", ((VarThresholdBlob)this._blobParam), "VarMaskWidth", true, DataSourceUpdateMode.OnPropertyChanged);
                this.掩膜高度comboBox.DataBindings.Add("Text", ((VarThresholdBlob)this._blobParam), "VarMaskHeight", true, DataSourceUpdateMode.OnPropertyChanged);
                this.标准差因子comboBox.DataBindings.Add("Text", ((VarThresholdBlob)this._blobParam), "VarStdDevScale", true, DataSourceUpdateMode.OnPropertyChanged);
                this.灰度值差comboBox.DataBindings.Add("Text", ((VarThresholdBlob)this._blobParam), "VarAbsThreshold", true, DataSourceUpdateMode.OnPropertyChanged);
                this.极性comboBox.DataBindings.Add("Text", ((VarThresholdBlob)this._blobParam), "VarLightDark", true, DataSourceUpdateMode.OnPropertyChanged);
                this.区域填充comboBox.DataBindings.Add("Text", ((SegmentParam)this._blobParam), "IsFill", true, DataSourceUpdateMode.OnPropertyChanged);
                this.区域连通comboBox.DataBindings.Add("Text", ((SegmentParam)this._blobParam), "IsConnection", true, DataSourceUpdateMode.OnPropertyChanged);
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
            }
        }
    }
}

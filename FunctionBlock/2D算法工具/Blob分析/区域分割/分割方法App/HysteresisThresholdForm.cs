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
    public partial class HysteresisThresholdForm : Form
    {
        private SegmentParam _blobParam;
        public HysteresisThresholdForm(SegmentParam param)
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
                this.低阈值comboBox.DataBindings.Add("Text", ((HysteresisThresholdBlob)this._blobParam), "HysteresisLow", true, DataSourceUpdateMode.OnPropertyChanged);
                this.高阈值comboBox.DataBindings.Add("Text", ((HysteresisThresholdBlob)this._blobParam), "HysteresisHight", true, DataSourceUpdateMode.OnPropertyChanged);
                this.最大长度comboBox.DataBindings.Add("Text", ((HysteresisThresholdBlob)this._blobParam), "HysteresisMaxLength", true, DataSourceUpdateMode.OnPropertyChanged);
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

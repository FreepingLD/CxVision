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
    public partial class FastThresholdForm : Form
    {
       private SegmentParam _blobParam;
        public FastThresholdForm(SegmentParam param)
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
                this.最小尺寸comboBox.DataBindings.Add("Text", ((FastThresholdBlob)this._blobParam), "FastMinSize", true, DataSourceUpdateMode.OnPropertyChanged);
                this.最小灰度值comboBox.DataBindings.Add("Text", ((FastThresholdBlob)this._blobParam), "FastMinGray", true, DataSourceUpdateMode.OnPropertyChanged);
                this.最大灰度值comboBox.DataBindings.Add("Text", ((FastThresholdBlob)this._blobParam), "FastMaxGray", true, DataSourceUpdateMode.OnPropertyChanged);
                this.区域填充comboBox.DataBindings.Add("Text", ((SegmentParam)this._blobParam), "IsFill", true, DataSourceUpdateMode.OnPropertyChanged);
                this.区域连通comboBox.DataBindings.Add("Text", ((SegmentParam)this._blobParam), "IsConnection", true, DataSourceUpdateMode.OnPropertyChanged);
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

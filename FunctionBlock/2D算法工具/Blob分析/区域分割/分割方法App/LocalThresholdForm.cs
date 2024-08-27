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
    public partial class LocalThresholdForm : Form
    {
      private  SegmentParam _blobParam;
        public LocalThresholdForm(SegmentParam param)
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
                this.方法comboBox.DataBindings.Add("Text", ((LocalThresholdBlob)this._blobParam), "Method", true, DataSourceUpdateMode.OnPropertyChanged);
                this.亮暗comboBox.DataBindings.Add("Text", ((LocalThresholdBlob)this._blobParam), "LocalLightDark", true, DataSourceUpdateMode.OnPropertyChanged);
                this.参数名称comboBox.DataBindings.Add("Text", ((LocalThresholdBlob)this._blobParam), "GenParamName", true, DataSourceUpdateMode.OnPropertyChanged);
                this.参数值comboBox.DataBindings.Add("Text", ((LocalThresholdBlob)this._blobParam), "GenParamValue", true, DataSourceUpdateMode.OnPropertyChanged);
                this.区域填充comboBox.DataBindings.Add("Text", ((SegmentParam)this._blobParam), "IsFill", true, DataSourceUpdateMode.OnPropertyChanged);
                this.区域连通comboBox.DataBindings.Add("Text", ((SegmentParam)this._blobParam), "IsConnection", true, DataSourceUpdateMode.OnPropertyChanged);
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

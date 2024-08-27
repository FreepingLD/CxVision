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
    public partial class BinaryThresholdForm : Form
    {
       private SegmentParam _param;
        public BinaryThresholdForm(SegmentParam function)
        {
            this._param = function;
            InitializeComponent();
            BindProperty();
        }

        private void BindProperty()
        {
            try
            {
                // 创建匹配参数
                this.方法comboBox.DataBindings.Add("Text", ((BinaryThresholdBlob)this._param), "BinaryMethod", true, DataSourceUpdateMode.OnPropertyChanged);
                this.亮暗comboBox.DataBindings.Add("Text", ((BinaryThresholdBlob)this._param), "BinaryLightDark", true, DataSourceUpdateMode.OnPropertyChanged);
                this.区域填充comboBox.DataBindings.Add("Text", ((SegmentParam)this._param), "IsFill", true, DataSourceUpdateMode.OnPropertyChanged);
                this.区域连通comboBox.DataBindings.Add("Text", ((SegmentParam)this._param), "IsConnection", true, DataSourceUpdateMode.OnPropertyChanged);
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
            }
        }
    }
}

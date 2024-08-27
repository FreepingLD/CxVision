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
    public partial class OutputRegionForm : Form
    {
        private IFunction _function;
        public OutputRegionForm(IFunction param)
        {
            this._function = param;
            InitializeComponent();
            BindProperty();
        }

        private void BindProperty()
        {
            try
            {
                // 创建匹配参数
                this.输出区域checkBox.DataBindings.Add(nameof(this.输出区域checkBox.Checked), ((Blob)this._function).BlobParam.OutParam, "IsOutputRegion", true, DataSourceUpdateMode.OnPropertyChanged);
                this.输出二值化图checkBox.DataBindings.Add(nameof(this.输出二值化图checkBox.Checked), ((Blob)this._function).BlobParam.OutParam, "IsOutputBinaryImage", true, DataSourceUpdateMode.OnPropertyChanged);
                this.输出二值化均图checkBox.DataBindings.Add(nameof(this.输出二值化均图checkBox.Checked), ((Blob)this._function).BlobParam.OutParam, "IsOutputBinaryMeanImage", true, DataSourceUpdateMode.OnPropertyChanged);
                this.模式comboBox.DataBindings.Add(nameof(this.模式comboBox.Text), ((Blob)this._function).BlobParam.OutParam, "DrawMode", true, DataSourceUpdateMode.OnPropertyChanged);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }



    }
}

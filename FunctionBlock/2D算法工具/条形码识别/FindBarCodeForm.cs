using Common;
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
    public partial class FindBarCodeForm : Form
    {
        private FindBarCodeParam _findBarCodeParam;
        public FindBarCodeForm(FindBarCodeParam Params)
        {
            this._findBarCodeParam = Params;
            InitializeComponent();
            BindProperty();
        }

        private void BindProperty()
        {
            try
            {
                // 创建匹配参数
                /////////////////////////////////////////
                this.解码个数textBox.DataBindings.Add("Text", this._findBarCodeParam, nameof(this._findBarCodeParam.stop_after_result_num), true, DataSourceUpdateMode.OnPropertyChanged);
                this.取反图像comboBox.DataBindings.Add("Text", this._findBarCodeParam, nameof(this._findBarCodeParam.InvertImage), true, DataSourceUpdateMode.OnPropertyChanged);
                this.条码类型comboBox.DataBindings.Add("Text", this._findBarCodeParam, nameof(this._findBarCodeParam.BarCodeType), true, DataSourceUpdateMode.OnPropertyChanged);
                this.解码超时textBox.DataBindings.Add("Text", this._findBarCodeParam, nameof(this._findBarCodeParam.timeout), true, DataSourceUpdateMode.OnPropertyChanged);
                this.保存方式comboBox.DataBindings.Add("Text", this._findBarCodeParam, nameof(this._findBarCodeParam.persistence), true, DataSourceUpdateMode.OnPropertyChanged);
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
            }

        }

        private void FindBarCodeForm_Load(object sender, EventArgs e)
        {

        }


    }
}

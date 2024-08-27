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
    public partial class FindDataCodeForm : Form
    {
        private FindDataCodeParam _findDataCodeParam;
        public FindDataCodeForm(FindDataCodeParam Params)
        {
            this._findDataCodeParam = Params;
            InitializeComponent();
            BindProperty();
        }

        private void BindProperty()
        {
            try
            {
                // 创建匹配参数
                /////////////////////////////////////////
                this.默认参数textBox.DataBindings.Add("Text", this._findDataCodeParam, nameof(this._findDataCodeParam.default_parameters), true, DataSourceUpdateMode.OnPropertyChanged);
                this.解码个数textBox.DataBindings.Add("Text", this._findDataCodeParam, nameof(this._findDataCodeParam.stop_after_result_num), true, DataSourceUpdateMode.OnPropertyChanged);
                this.镜像comboBox.DataBindings.Add("Text", this._findDataCodeParam, nameof(this._findDataCodeParam.mirrored), true, DataSourceUpdateMode.OnPropertyChanged);
                this.极性comboBox.DataBindings.Add("Text", this._findDataCodeParam, nameof(this._findDataCodeParam.polarity), true, DataSourceUpdateMode.OnPropertyChanged);
                this.解码超时textBox.DataBindings.Add("Text", this._findDataCodeParam, nameof(this._findDataCodeParam.timeout), true, DataSourceUpdateMode.OnPropertyChanged);
                this.保存方式comboBox.DataBindings.Add("Text", this._findDataCodeParam, nameof(this._findDataCodeParam.persistence), true, DataSourceUpdateMode.OnPropertyChanged);
                this.保存失败comboBox.DataBindings.Add("Text", this._findDataCodeParam, nameof(this._findDataCodeParam.discard_undecoded_candidates), true, DataSourceUpdateMode.OnPropertyChanged);
                this.严格模型comboBox.DataBindings.Add("Text", this._findDataCodeParam, nameof(this._findDataCodeParam.strict_model), true, DataSourceUpdateMode.OnPropertyChanged);
                this.字符解码comboBox.DataBindings.Add("Text", this._findDataCodeParam, nameof(this._findDataCodeParam.string_encoding), true, DataSourceUpdateMode.OnPropertyChanged);
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
            }

        }

        private void FindDataCodeForm_Load(object sender, EventArgs e)
        {

        }


    }
}

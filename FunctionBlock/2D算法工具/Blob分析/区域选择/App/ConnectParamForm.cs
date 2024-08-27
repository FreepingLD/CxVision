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
    public partial class ConnectParamForm : Form
    {
        private SelectParamValue _param;
        public ConnectParamForm(SelectParamValue param)
        {
            this._param = param;
            InitializeComponent();
            BindProperty();
        }

        private void BindProperty()
        {
            try
            {
                // 创建匹配参数
                SelectConnectParam param = ((SelectConnectParam)this._param);
                this.区域合并comboBox.DataBindings.Add(nameof(this.区域合并comboBox.Text), param, nameof(param.IsConnect), true, DataSourceUpdateMode.OnPropertyChanged);
                this.最小面积comboBox.DataBindings.Add(nameof(this.最小面积comboBox.Text), param, nameof(param.MinArea), true, DataSourceUpdateMode.OnPropertyChanged);
                this.最大面积comboBox.DataBindings.Add(nameof(this.最大面积comboBox.Text), param, nameof(param.MaxArea), true, DataSourceUpdateMode.OnPropertyChanged);
                this.联通距离comboBox.DataBindings.Add(nameof(this.联通距离comboBox.Text), param, nameof(param.ConnectDist), true, DataSourceUpdateMode.OnPropertyChanged);
                this.联通数量comboBox.DataBindings.Add(nameof(this.联通数量comboBox.Text), param, nameof(param.ConnectCount), true, DataSourceUpdateMode.OnPropertyChanged);
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
            }
        }

        private void IntensityParamForm_Load(object sender, EventArgs e)
        {

        }
    }
}

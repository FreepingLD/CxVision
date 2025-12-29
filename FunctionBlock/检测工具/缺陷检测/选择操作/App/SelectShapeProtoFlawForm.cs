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
    public partial class SelectShapeProtoFlawForm : Form
    {
        private SelectShapeProtoFlawParam _param;
        public SelectShapeProtoFlawForm(SelectFlawParam param)
        {
            this._param = param as SelectShapeProtoFlawParam;
            InitializeComponent();
            BindProperty();
            this.AutoScroll = true;
        }

        private void BindProperty()
        {
            try
            {
                // 创建匹配参数
                this.特征ComboBox.DataBindings.Add(nameof(this.特征ComboBox.Text), (this._param), "Features", true, DataSourceUpdateMode.OnPropertyChanged);
                //this.操作comboBox.DataBindings.Add(nameof(this.操作comboBox.Text), (this._param), "Operation", true, DataSourceUpdateMode.OnPropertyChanged);
                this.最小值comboBox.DataBindings.Add(nameof(this.最小值comboBox.Text), (this._param), "Min", true, DataSourceUpdateMode.OnPropertyChanged);
                this.最大值comboBox.DataBindings.Add(nameof(this.最大值comboBox.Text), (this._param), "Max", true, DataSourceUpdateMode.OnPropertyChanged);
                this.极性comboBox.DataBindings.Add(nameof(this.极性comboBox.Text), (this._param), "FlawPolarity", true, DataSourceUpdateMode.OnPropertyChanged);
                this.启用算法checkBox.DataBindings.Add(nameof(this.启用算法checkBox.Checked), this._param, "IsSelect", true, DataSourceUpdateMode.OnPropertyChanged);
            }
            catch (Exception ex)
            {
                new Common.UserMessageForm(ex.ToString()).ShowDialog();
            }
        }



    }
}

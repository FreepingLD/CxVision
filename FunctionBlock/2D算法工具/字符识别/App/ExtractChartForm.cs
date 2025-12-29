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
    public partial class ExtractChartForm : Form
    {
       private ExtractChartParam _param;
        public ExtractChartForm(ExtractChartParam param)
        {
            this._param = param;
            InitializeComponent();
            BindProperty();
            this.AutoScroll = true;
        }

        private void BindProperty()
        {
            try
            {
                // 创建匹配参数
                this.掩膜宽度comboBox.DataBindings.Add("Text", (this._param), "VarMaskWidth", true, DataSourceUpdateMode.OnPropertyChanged);
                this.掩膜高度comboBox.DataBindings.Add("Text", (this._param), "VarMaskHeight", true, DataSourceUpdateMode.OnPropertyChanged);
                this.标准差因子comboBox.DataBindings.Add("Text", (this._param), "VarStdDevScale", true, DataSourceUpdateMode.OnPropertyChanged);
                this.灰度值差comboBox.DataBindings.Add("Text", (this._param), "VarAbsThreshold", true, DataSourceUpdateMode.OnPropertyChanged);
                this.极性comboBox.DataBindings.Add("Text", (this._param), "VarLightDark", true, DataSourceUpdateMode.OnPropertyChanged);
                this.最小面积comboBox.DataBindings.Add("Text", (this._param), "Area", true, DataSourceUpdateMode.OnPropertyChanged);
                //this.启用检测checkBox.DataBindings.Add(nameof(this.启用检测checkBox.Checked), this._param, "IsDetect", true, DataSourceUpdateMode.OnPropertyChanged);

                //this.VarMaskWidth = 15;
                //this.VarMaskHeight = 15;
                //this.VarStdDevScale = 0.2;
                //this.VarAbsThreshold = 5;
                //this.VarLightDark = "dark";
                //this.Area = 10;
            }
            catch (Exception ex)
            {
                new Common.UserMessageForm(ex.ToString()).ShowDialog();
            }
        }

    }
}

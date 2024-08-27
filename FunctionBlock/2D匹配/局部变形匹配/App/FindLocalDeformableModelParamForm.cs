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
    public partial class FindLocalDeformableModelParamForm : Form
    {
        private FindLocalDeformableModelParam _LocalDeformableModelParam;
        public FindLocalDeformableModelParamForm(FindLocalDeformableModelParam Params)
        {
            this._LocalDeformableModelParam = Params;
            InitializeComponent();
            BindProperty();
        }

        private void BindProperty()
        {
            try
            {
                // 创建匹配参数
                /////////////////////////////////////////
                this.起始角度textBox.DataBindings.Add("Text", this._LocalDeformableModelParam, "AngleStart", true, DataSourceUpdateMode.OnPropertyChanged);
                this.角度范围textBox.DataBindings.Add("Text", this._LocalDeformableModelParam, "AngleExtent", true, DataSourceUpdateMode.OnPropertyChanged);
                /////////////////////////////////////////
                this.行最小缩放textBox.DataBindings.Add("Text", this._LocalDeformableModelParam, "ScaleRMin", true, DataSourceUpdateMode.OnPropertyChanged);
                this.行最大缩放textBox.DataBindings.Add("Text", this._LocalDeformableModelParam, "ScaleRMax", true, DataSourceUpdateMode.OnPropertyChanged);
                this.列最小缩放textBox.DataBindings.Add("Text", this._LocalDeformableModelParam, "ScaleCMin", true, DataSourceUpdateMode.OnPropertyChanged);
                this.列最大缩放textBox.DataBindings.Add("Text", this._LocalDeformableModelParam, "ScaleCMax", true, DataSourceUpdateMode.OnPropertyChanged);
                this.最小得分textBox.DataBindings.Add("Text", this._LocalDeformableModelParam, "MinScore", true, DataSourceUpdateMode.OnPropertyChanged);
                this.匹配个数textBox.DataBindings.Add("Text", this._LocalDeformableModelParam, "NumMatches", true, DataSourceUpdateMode.OnPropertyChanged);
                // this.金字塔层级comboBox.DataBindings.Add("Text", this._LocalDeformableModelParam, "NumLevels", true, DataSourceUpdateMode.OnPropertyChanged);
                this.贪婪度textBox.DataBindings.Add("Text", this._LocalDeformableModelParam, "Greediness", true, DataSourceUpdateMode.OnPropertyChanged);
                this.匹配超时textBox.DataBindings.Add("Text", this._LocalDeformableModelParam, "TimeOut", true, DataSourceUpdateMode.OnPropertyChanged);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }


    }
}

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
    public partial class FindNccModelParamForm : Form
    {
        private FindNccModelParam _nccModelParam;
        public FindNccModelParamForm(FindNccModelParam Params)
        {
            this._nccModelParam = Params;
            InitializeComponent();
            BindProperty();
        }

        private void BindProperty()
        {
            try
            {
                // 创建匹配参数
                //this.亚像素精度comboBox.DataSource = Enum.GetNames(typeof(enInterpolationMethod));
                this.匹配模式comboBox.DataSource = Enum.GetNames(typeof(enMatchMode));
                this.排序方式comboBox.DataSource = Enum.GetNames(typeof(enSortMethod));
                /////////////////////////////////////////
                this.起始角度textBox.DataBindings.Add("Text", this._nccModelParam, "AngleStart", true, DataSourceUpdateMode.OnPropertyChanged);
                this.终止角度textBox.DataBindings.Add("Text", this._nccModelParam, "AngleExtent", true, DataSourceUpdateMode.OnPropertyChanged);
                this.最小得分textBox.DataBindings.Add("Text", this._nccModelParam, "MinScore", true, DataSourceUpdateMode.OnPropertyChanged);
                this.匹配个数textBox.DataBindings.Add("Text", this._nccModelParam, "NumMatches", true, DataSourceUpdateMode.OnPropertyChanged);
                this.匹配模式comboBox.DataBindings.Add("Text", this._nccModelParam, "MatchMode", true, DataSourceUpdateMode.OnPropertyChanged);
                this.最大重叠textBox.DataBindings.Add("Text", this._nccModelParam, "MaxOverlap", true, DataSourceUpdateMode.OnPropertyChanged);
                this.亚像素精度comboBox.DataBindings.Add("Text", this._nccModelParam, "SubPixel", true, DataSourceUpdateMode.OnPropertyChanged);
                this.金字塔层级textBox.DataBindings.Add("Text", this._nccModelParam, "NumLevels", true, DataSourceUpdateMode.OnPropertyChanged);
                this.匹配超时textBox.DataBindings.Add("Text", this._nccModelParam, "TimeOut", true, DataSourceUpdateMode.OnPropertyChanged);
                this.补正类型comboBox.DataSource = Enum.GetNames(typeof(enAdjustType));  //resetShapeModel
                this.补正类型comboBox.DataBindings.Add("Text", _nccModelParam, "AdjustType", true, DataSourceUpdateMode.OnPropertyChanged);
                this.排序方式comboBox.DataBindings.Add("Text", this._nccModelParam, "SortMethod", true, DataSourceUpdateMode.OnPropertyChanged);
                this.填充空区域comboBox.DataBindings.Add("Text", this._nccModelParam, "FiilUp", true, DataSourceUpdateMode.OnPropertyChanged);
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
            }

        }


    }
}

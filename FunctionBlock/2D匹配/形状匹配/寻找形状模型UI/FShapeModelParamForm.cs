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
    public partial class FShapeModelParamForm : Form
    {
        private F_ShapeModelParam _shapeModelParam;
        public FShapeModelParamForm(F_ShapeModelParam Params)
        {
            this._shapeModelParam = Params;
            InitializeComponent();
            BindProperty();
        }
        public FShapeModelParamForm(F_ShapeModelParamBase Params)
        {
            this._shapeModelParam = (F_ShapeModelParam)Params;
            InitializeComponent();
            BindProperty();
        }
        private void BindProperty()
        {
            try
            {
                // 创建匹配参数
                this.亚像素精度comboBox.DataSource = Enum.GetNames(typeof(enInterpolationMethod));
                this.过虑方式comboBox.DataSource = Enum.GetNames(typeof(enFilterMethod));
                this.匹配模式comboBox.DataSource = Enum.GetNames(typeof(enMatchMode));
                this.排序方式comboBox.DataSource = Enum.GetNames(typeof(enSortMethod));
                this.补正类型comboBox.DataSource = Enum.GetNames(typeof(enAdjustType));  
                /////////////////////////////////////////
                this.起始角度textBox.DataBindings.Add("Text", this._shapeModelParam, "AngleStart", true, DataSourceUpdateMode.OnPropertyChanged);
                this.终止角度textBox.DataBindings.Add("Text", this._shapeModelParam, "AngleExtent", true, DataSourceUpdateMode.OnPropertyChanged);
                this.最小得分textBox.DataBindings.Add("Text", this._shapeModelParam, "MinScore", true, DataSourceUpdateMode.OnPropertyChanged);
                this.匹配个数textBox.DataBindings.Add("Text", this._shapeModelParam, "NumMatches", true, DataSourceUpdateMode.OnPropertyChanged);
                this.匹配模式comboBox.DataBindings.Add("Text", this._shapeModelParam, "MatchMode", true, DataSourceUpdateMode.OnPropertyChanged);
                this.最大重叠textBox.DataBindings.Add("Text", this._shapeModelParam, "MaxOverlap", true, DataSourceUpdateMode.OnPropertyChanged);
                this.亚像素精度comboBox.DataBindings.Add("Text", this._shapeModelParam, "SubPixel", true, DataSourceUpdateMode.OnPropertyChanged);
                this.匹配超时textBox.DataBindings.Add("Text", this._shapeModelParam, "TimeOut", true, DataSourceUpdateMode.OnPropertyChanged);
                this.贪婪度textBox.DataBindings.Add("Text", this._shapeModelParam, "Greediness", true, DataSourceUpdateMode.OnPropertyChanged);
                this.过虑方式comboBox.DataBindings.Add("Text", this._shapeModelParam, nameof(this._shapeModelParam.FilterMethod), true, DataSourceUpdateMode.OnPropertyChanged);
                this.补正类型comboBox.DataBindings.Add("Text", this._shapeModelParam, "AdjustType", true, DataSourceUpdateMode.OnPropertyChanged);
                this.排序方式comboBox.DataBindings.Add("Text", this._shapeModelParam, "SortMethod", true, DataSourceUpdateMode.OnPropertyChanged); 
                this.填充空区域comboBox.DataBindings.Add("Text", this._shapeModelParam, "FiilUp", true, DataSourceUpdateMode.OnPropertyChanged); 
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
            }

        }

        private void FShapeModelParamForm_Load(object sender, EventArgs e)
        {

        }
    }
}

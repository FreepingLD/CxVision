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
    public partial class FScaledShapeModelParamForm : Form
    {
      private F_ScaledShapeModelParam _shapeModel;
        public FScaledShapeModelParamForm(F_ScaledShapeModelParam param)
        {
            this._shapeModel = param;
            InitializeComponent();
            BindProperty();
        }
        public FScaledShapeModelParamForm(F_ShapeModelParamBase param)
        {
            this._shapeModel = (F_ScaledShapeModelParam)param;
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
                ////////////////////////////
                this.起始角度textBox.DataBindings.Add("Text", this._shapeModel, "AngleStart", true, DataSourceUpdateMode.OnPropertyChanged);
                this.终止角度textBox.DataBindings.Add("Text", this._shapeModel, "AngleExtent", true, DataSourceUpdateMode.OnPropertyChanged);
                this.最小缩放textBox.DataBindings.Add("Text", this._shapeModel, "ScaleMin", true, DataSourceUpdateMode.OnPropertyChanged);
                this.最大缩放textBox.DataBindings.Add("Text", this._shapeModel, "ScaleMax", true, DataSourceUpdateMode.OnPropertyChanged);
                this.最小得分textBox.DataBindings.Add("Text", this._shapeModel, "MinScore", true, DataSourceUpdateMode.OnPropertyChanged);
                this.匹配个数textBox.DataBindings.Add("Text", this._shapeModel, "NumMatches", true, DataSourceUpdateMode.OnPropertyChanged);
                this.匹配模式comboBox.DataBindings.Add("Text", this._shapeModel, "MatchMode", true, DataSourceUpdateMode.OnPropertyChanged);
                this.最大重叠textBox.DataBindings.Add("Text", this._shapeModel, "MaxOverlap", true, DataSourceUpdateMode.OnPropertyChanged);
                this.亚像素精度comboBox.DataBindings.Add("Text", this._shapeModel, "SubPixel", true, DataSourceUpdateMode.OnPropertyChanged);
                this.匹配超时textBox.DataBindings.Add("Text", this._shapeModel, "TimeOut", true, DataSourceUpdateMode.OnPropertyChanged);
                this.贪婪度textBox.DataBindings.Add("Text", this._shapeModel, "Greediness", true, DataSourceUpdateMode.OnPropertyChanged);
                this.过虑方式comboBox.DataBindings.Add("Text", this._shapeModel, nameof(this._shapeModel.FilterMethod), true, DataSourceUpdateMode.OnPropertyChanged);
                this.补正类型comboBox.DataSource = Enum.GetNames(typeof(enAdjustType));  //resetShapeModel
                this.补正类型comboBox.DataBindings.Add("Text", this._shapeModel, "AdjustType", true, DataSourceUpdateMode.OnPropertyChanged);
                this.填充空区域comboBox.DataBindings.Add("Text", this._shapeModel, "FiilUp", true, DataSourceUpdateMode.OnPropertyChanged);
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
            }
        }
    }
}

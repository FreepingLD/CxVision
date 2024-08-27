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
    public partial class FAnisoShapeModelParamForm : Form
    {
      private F_AnisoShapeModelParam _shapeModel;
        public FAnisoShapeModelParamForm(F_AnisoShapeModelParam param)
        {
            this._shapeModel = param;
            InitializeComponent();
            BindProperty();
        }
        public FAnisoShapeModelParamForm(F_ShapeModelParamBase param)
        {
            this._shapeModel = (F_AnisoShapeModelParam)param;
            InitializeComponent();
            BindProperty();
        }
        private void BindProperty()
        {
            try
            {
                this.亚像素精度comboBox.DataSource = Enum.GetNames(typeof(enInterpolationMethod));
                this.过虑方式comboBox.DataSource = Enum.GetNames(typeof(enFilterMethod));
                this.匹配模式comboBox.DataSource = Enum.GetNames(typeof(enMatchMode));
                this.补正类型comboBox.DataSource = Enum.GetNames(typeof(enAdjustType));  //resetShapeModel
                ///////////////////////////////////////
                this.起始角度textBox.DataBindings.Add("Text", this._shapeModel, "AngleStart", true, DataSourceUpdateMode.OnPropertyChanged);
                this.终止角度textBox.DataBindings.Add("Text", this._shapeModel, "AngleExtent", true, DataSourceUpdateMode.OnPropertyChanged);
                this.行最小缩放textBox.DataBindings.Add("Text", this._shapeModel, "ScaleRMin", true, DataSourceUpdateMode.OnPropertyChanged);
                this.行最大缩放textBox.DataBindings.Add("Text", this._shapeModel, "ScaleRMax", true, DataSourceUpdateMode.OnPropertyChanged);
                this.列最小缩放textBox.DataBindings.Add("Text", this._shapeModel, "ScaleCMin", true, DataSourceUpdateMode.OnPropertyChanged);
                this.列最大缩放textBox.DataBindings.Add("Text", this._shapeModel, "ScaleCMax", true, DataSourceUpdateMode.OnPropertyChanged);
                this.最小得分textBox.DataBindings.Add("Text", this._shapeModel, "MinScore", true, DataSourceUpdateMode.OnPropertyChanged);
                this.匹配个数textBox.DataBindings.Add("Text", this._shapeModel, "NumMatches", true, DataSourceUpdateMode.OnPropertyChanged);
                this.匹配模式comboBox.DataBindings.Add("Text", this._shapeModel, "MatchMode", true, DataSourceUpdateMode.OnPropertyChanged);
                this.最大重叠textBox.DataBindings.Add("Text", this._shapeModel, "MaxOverlap", true, DataSourceUpdateMode.OnPropertyChanged);
                this.亚像素精度comboBox.DataBindings.Add("Text", this._shapeModel, "SubPixel", true, DataSourceUpdateMode.OnPropertyChanged);
                this.匹配超时textBox.DataBindings.Add("Text", this._shapeModel, "TimeOut", true, DataSourceUpdateMode.OnPropertyChanged);
                this.贪婪度textBox.DataBindings.Add("Text", this._shapeModel, "Greediness", true, DataSourceUpdateMode.OnPropertyChanged);
                this.过虑方式comboBox.DataBindings.Add("Text", this._shapeModel, nameof(this._shapeModel.FilterMethod), true, DataSourceUpdateMode.OnPropertyChanged);
                this.补正类型comboBox.DataBindings.Add("Text", this._shapeModel, "AdjustType", true, DataSourceUpdateMode.OnPropertyChanged);
                this.填充空区域comboBox.DataBindings.Add("Text", this._shapeModel, "FiilUp", true, DataSourceUpdateMode.OnPropertyChanged);
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
            }
        }

        private void FindAnisoShapeModelForm_Load(object sender, EventArgs e)
        {

        }
    }
}

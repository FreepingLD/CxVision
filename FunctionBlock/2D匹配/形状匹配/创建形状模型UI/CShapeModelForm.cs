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
    public partial class CShapeModelForm : Form
    {
       private C_ShapeModelParam _shapeModel;
        public CShapeModelForm(C_ShapeModelParam param)
        {
            this._shapeModel = param;
            InitializeComponent();
            BindProperty();
        }
        public CShapeModelForm(C_ShapeModelParamBase param)
        {
            this._shapeModel =(C_ShapeModelParam)param;
            InitializeComponent();
            BindProperty();
        }

        private void BindProperty()
        {
            try
            {
                this.金字塔层级comboBox.DataBindings.Add("Text", this._shapeModel, "NumLevels", true, DataSourceUpdateMode.OnPropertyChanged);
                this.起始角度textBox.DataBindings.Add("Text", this._shapeModel, "AngleStart", true, DataSourceUpdateMode.OnPropertyChanged);
                this.终止角度textBox.DataBindings.Add("Text", this._shapeModel, "AngleExtent", true, DataSourceUpdateMode.OnPropertyChanged);
                this.角度步长comboBox.DataBindings.Add("Text", this._shapeModel, "AngleStep", true, DataSourceUpdateMode.OnPropertyChanged);
                ///////////////////////
                this.优化comboBox.DataBindings.Add("Text", this._shapeModel, "Optimization", true, DataSourceUpdateMode.OnPropertyChanged);
                this.极性comboBox.DataBindings.Add("Text", this._shapeModel, "Metric", true, DataSourceUpdateMode.OnPropertyChanged);
                this.对比度comboBox.DataBindings.Add("Text", this._shapeModel, "Contrast", true, DataSourceUpdateMode.OnPropertyChanged);
                this.最小轮廓长度comboBox.DataBindings.Add("Text", this._shapeModel, "MinLenght", true, DataSourceUpdateMode.OnPropertyChanged);
                this.最小对比度comboBox.DataBindings.Add("Text", this._shapeModel, "MinContrast", true, DataSourceUpdateMode.OnPropertyChanged);
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
            }
        }

        private void 最小对比度comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void CShapeModelForm_Load(object sender, EventArgs e)
        {

        }
    }
}

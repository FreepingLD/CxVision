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
    public partial class CreateLocalDeformableModelParamForm : Form
    {
        private CreateLocalDeformableModelParam _LocalDeformableModelParam;
        public CreateLocalDeformableModelParamForm(CreateLocalDeformableModelParam param)
        {
            this._LocalDeformableModelParam = param;
            InitializeComponent();
            BindProperty();
        }


        private void BindProperty()
        {
            try
            {
                this.金字塔层级comboBox.DataBindings.Add("Text", this._LocalDeformableModelParam, "NumLevels", true, DataSourceUpdateMode.OnPropertyChanged);
                this.起始角度textBox.DataBindings.Add("Text", this._LocalDeformableModelParam, "AngleStart", true, DataSourceUpdateMode.OnPropertyChanged);
                this.终止角度textBox.DataBindings.Add("Text", this._LocalDeformableModelParam, "AngleExtent", true, DataSourceUpdateMode.OnPropertyChanged);
                this.角度步长comboBox.DataBindings.Add("Text", this._LocalDeformableModelParam, "AngleStep", true, DataSourceUpdateMode.OnPropertyChanged);
                ////////////////////////////////
                this.行最小缩放textBox.DataBindings.Add("Text", this._LocalDeformableModelParam, "ScaleRMin", true, DataSourceUpdateMode.OnPropertyChanged);
                this.行最大缩放textBox.DataBindings.Add("Text", this._LocalDeformableModelParam, "ScaleRMax", true, DataSourceUpdateMode.OnPropertyChanged);
                this.行缩放步长comboBox.DataBindings.Add("Text", this._LocalDeformableModelParam, "ScaleRStep", true, DataSourceUpdateMode.OnPropertyChanged);
                ///////////////////////
                this.列最小缩放textBox.DataBindings.Add("Text", this._LocalDeformableModelParam, "ScaleCMin", true, DataSourceUpdateMode.OnPropertyChanged);
                this.列最大缩放textBox.DataBindings.Add("Text", this._LocalDeformableModelParam, "ScaleCMax", true, DataSourceUpdateMode.OnPropertyChanged);
                this.列缩放步长comboBox.DataBindings.Add("Text", this._LocalDeformableModelParam, "ScaleCStep", true, DataSourceUpdateMode.OnPropertyChanged);
                this.优化comboBox.DataBindings.Add("Text", this._LocalDeformableModelParam, "Optimization", true, DataSourceUpdateMode.OnPropertyChanged);
                this.极性comboBox.DataBindings.Add("Text", this._LocalDeformableModelParam, "Metric", true, DataSourceUpdateMode.OnPropertyChanged);
                this.对比度comboBox.DataBindings.Add("Text", this._LocalDeformableModelParam, "Contrast", true, DataSourceUpdateMode.OnPropertyChanged);
                this.最小对比度comboBox.DataBindings.Add("Text", this._LocalDeformableModelParam, "MinContrast", true, DataSourceUpdateMode.OnPropertyChanged);
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
            }
        }


        private void CShapeModelForm_Load(object sender, EventArgs e)
        {

        }
    }
}

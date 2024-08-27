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
    public partial class CreateDeformableSurfaceModelForm : Form
    {
        private C_DeformableSurfaceModelParam _shapeModel;
        public CreateDeformableSurfaceModelForm(C_DeformableSurfaceModelParam param)
        {
            this._shapeModel = param;
            InitializeComponent();
            BindProperty();
        }

        private void BindProperty()
        {
            try
            {
                this.相对采样距离textBox.DataBindings.Add("Text", this._shapeModel, "RelSamplingDistance", true, DataSourceUpdateMode.OnPropertyChanged);
                this.取反模型法向comboBox.DataBindings.Add("Text", this._shapeModel, "ModelInvertNormals", true, DataSourceUpdateMode.OnPropertyChanged);
                this.最小缩放textBox.DataBindings.Add("Text", this._shapeModel, "MinScale", true, DataSourceUpdateMode.OnPropertyChanged);
                this.最大缩放textBox.DataBindings.Add("Text", this._shapeModel, "MaxScale", true, DataSourceUpdateMode.OnPropertyChanged);
                this.最大变形角度textBox.DataBindings.Add("Text", this._shapeModel, "MaxBending", true, DataSourceUpdateMode.OnPropertyChanged);
                this.刚度textBox.DataBindings.Add("Text", this._shapeModel, "Stiffness", true, DataSourceUpdateMode.OnPropertyChanged);
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

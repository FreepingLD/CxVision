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
    public partial class CreateSurfaceModelForm : Form
    {
        private C_SurfaceModelParam _shapeModel;
        public CreateSurfaceModelForm(C_SurfaceModelParam param)
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
                this.位姿优化相对采样距离textBox.DataBindings.Add("Text", this._shapeModel, "PoseRefRelSamplingDistance", true, DataSourceUpdateMode.OnPropertyChanged);
                this.取反模型法向comboBox.DataBindings.Add("Text", this._shapeModel, "ModelInvertNormals", true, DataSourceUpdateMode.OnPropertyChanged);
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

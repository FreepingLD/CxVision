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
    public partial class FindDeformableSurfaceModelForm : Form
    {
        private F_ShapeModelParam _shapeModel;
        public FindDeformableSurfaceModelForm(F_ShapeModelParam Params)
        {
            this._shapeModel = Params;
            InitializeComponent();
            BindProperty();
        }

        private void BindProperty()
        {
            try
            {
                /////////////////////////////////////////
                this.相对采样距离textBox.DataBindings.Add("Text", this._shapeModel, "RelSamplingDistance", true, DataSourceUpdateMode.OnPropertyChanged);
                this.最小得分textBox.DataBindings.Add("Text", this._shapeModel, "MinScore", true, DataSourceUpdateMode.OnPropertyChanged);
                this.计算场景法向comboBox.DataBindings.Add("Text", this._shapeModel, "SceneNormalComputation", true, DataSourceUpdateMode.OnPropertyChanged);
                this.位姿优化相对距离textBox.DataBindings.Add("Text", this._shapeModel, "PoseRefDistThresholdRel", true, DataSourceUpdateMode.OnPropertyChanged);
                this.位姿优化相对得分距离textBox.DataBindings.Add("Text", this._shapeModel, "PoseRefScoringDistRel", true, DataSourceUpdateMode.OnPropertyChanged);
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
            }

        }


    }
}

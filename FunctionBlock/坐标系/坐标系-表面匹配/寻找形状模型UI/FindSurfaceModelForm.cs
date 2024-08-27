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
    public partial class FindSurfaceModelForm : Form
    {
        private F_SurfaceModelParam _shapeModel;
        public FindSurfaceModelForm(F_SurfaceModelParam Params)
        {
            this._shapeModel = Params;
            InitializeComponent();
            BindProperty();
        }

        private void BindProperty()
        {
            try
            {
                // 创建匹配参数
                this.得分类型comboBox.DataSource = Enum.GetNames(typeof(enScoreType));
                /////////////////////////////////////////
                this.相对采样距离textBox.DataBindings.Add("Text", this._shapeModel, "RelSamplingDistance", true, DataSourceUpdateMode.OnPropertyChanged);
                this.关键因子textBox.DataBindings.Add("Text", this._shapeModel, "KeyPointFraction", true, DataSourceUpdateMode.OnPropertyChanged);
                this.最小得分textBox.DataBindings.Add("Text", this._shapeModel, "MinScore", true, DataSourceUpdateMode.OnPropertyChanged);
                this.返回结果句柄comboBox.DataBindings.Add("Text", this._shapeModel, "ReturnResultHandle", true, DataSourceUpdateMode.OnPropertyChanged);

                this.计算场景法向comboBox.DataBindings.Add("Text", this._shapeModel, "SceneNormalComputation", true, DataSourceUpdateMode.OnPropertyChanged);
                this.稀疏位姿优化comboBox.DataBindings.Add("Text", this._shapeModel, "SparsePoseRefinement", true, DataSourceUpdateMode.OnPropertyChanged);
                this.位姿优化相对距离textBox.DataBindings.Add("Text", this._shapeModel, "PoseRefDistThresholdRel", true, DataSourceUpdateMode.OnPropertyChanged);
                this.位姿优化相对得分距离textBox.DataBindings.Add("Text", this._shapeModel, "PoseRefScoringDistRel", true, DataSourceUpdateMode.OnPropertyChanged);
               
                this.匹配个数textBox.DataBindings.Add("Text", this._shapeModel, "MatchesNum", true, DataSourceUpdateMode.OnPropertyChanged);
                this.最大相对重叠距离textBox.DataBindings.Add("Text", this._shapeModel, "MaxOverlapDistRel", true, DataSourceUpdateMode.OnPropertyChanged);
                this.得分类型comboBox.DataBindings.Add("Text", this._shapeModel, "ScoreType", true, DataSourceUpdateMode.OnPropertyChanged);
                this.位姿优化使用场景法向comboBox.DataBindings.Add("Text", this._shapeModel, "PoseRefUseSceneNormals", true, DataSourceUpdateMode.OnPropertyChanged);
                this.密集位姿优化comboBox.DataBindings.Add("Text", this._shapeModel, "DensePoseRefinement", true, DataSourceUpdateMode.OnPropertyChanged);
                this.位姿优化迭代次数textBox.DataBindings.Add("Text", this._shapeModel, "PoseRefNumSteps", true, DataSourceUpdateMode.OnPropertyChanged);
                this.密集位姿优化子采样textBox.DataBindings.Add("Text", this._shapeModel, "PoseRefSubSampling", true, DataSourceUpdateMode.OnPropertyChanged);
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
            }

        }


    }
}

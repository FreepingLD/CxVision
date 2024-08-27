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
    public partial class DoImageEnhancementForm : Form
    {
        private bool IsLoad = false;
        private IFunction _function;
        public DoImageEnhancementForm(IFunction param)
        {
            this._function = param;
            InitializeComponent();
            this.BindProperty();
        }

        private void BindProperty()
        {
            try
            {
                // 创建匹配参数
                this.方法comboBox.Items.Clear();
                foreach (var item in Enum.GetValues(typeof(enImageEnhancementMethod)))
                    this.方法comboBox.Items.Add(item);
                //this.方法comboBox.DataSource = Enum.GetNames(typeof(enImageEnhancementMethod));
                ////////////////////////////////////////////////
                this.方法comboBox.DataBindings.Add(nameof(this.方法comboBox.Text), ((ImageEnhancement)this._function).EnhancementOperator.EnhancementParam, "Method", true, DataSourceUpdateMode.OnPropertyChanged);
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
            }
        }

        private void 方法comboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {

        }


        private void AddForm(Panel MastPanel, Form form)
        {
            if (MastPanel == null) return;
            if (MastPanel.Controls.Count > 0)
                MastPanel.Controls.Clear();
            if (form == null) return;
            form.TopLevel = false;
            form.FormBorderStyle = FormBorderStyle.None;
            form.MaximizeBox = false;
            form.MinimizeBox = false;
            form.Dock = DockStyle.Fill;
            MastPanel.Controls.Add(form);
            form.Show();
        }
        private void AddForm(GroupBox groupBox, Form form)
        {
            if (groupBox == null) return;
            if (groupBox.Controls.Count > 0)
                groupBox.Controls.Clear();
            if (form == null) return;
            form.TopLevel = false;
            form.FormBorderStyle = FormBorderStyle.None;
            form.MaximizeBox = false;
            form.MinimizeBox = false;
            form.Dock = DockStyle.Fill;
            groupBox.Controls.Add(form);
            form.Show();
        }

        private void 方法comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.方法comboBox.SelectedIndex == -1) return;
                switch (this.方法comboBox.SelectedItem.ToString())
                {
                    case nameof(enImageEnhancementMethod.coherence_enhancing_diff):
                        if (this.IsLoad)
                            ((ImageEnhancement)this._function).EnhancementOperator.EnhancementParam = new CoherenceEnhancingDiffParam(this.方法comboBox.SelectedItem.ToString());
                        AddForm(this.splitContainer1.Panel2, new CoherenceEnhancingDiffForm(((ImageEnhancement)this._function).EnhancementOperator.EnhancementParam));
                        break;
                    case nameof(enImageEnhancementMethod.emphasize):
                        if (this.IsLoad)
                            ((ImageEnhancement)this._function).EnhancementOperator.EnhancementParam = new EmphasizeParam(this.方法comboBox.SelectedItem.ToString());
                        AddForm(this.splitContainer1.Panel2, new EmphasizeForm(((ImageEnhancement)this._function).EnhancementOperator.EnhancementParam));
                        break;
                    case nameof(enImageEnhancementMethod.equ_histo_image):
                        if (this.IsLoad)
                            ((ImageEnhancement)this._function).EnhancementOperator.EnhancementParam = new EquHistoImageParam(this.方法comboBox.SelectedItem.ToString());
                        AddForm(this.splitContainer1.Panel2, new EquHistoImageForm(((ImageEnhancement)this._function).EnhancementOperator.EnhancementParam));
                        break;
                    case nameof(enImageEnhancementMethod.illuminate):
                        if (this.IsLoad)
                            ((ImageEnhancement)this._function).EnhancementOperator.EnhancementParam = new IlluminateParam(this.方法comboBox.SelectedItem.ToString());
                        AddForm(this.splitContainer1.Panel2, new IlluminateForm(((ImageEnhancement)this._function).EnhancementOperator.EnhancementParam));
                        break;
                    case nameof(enImageEnhancementMethod.mean_curvature_flow):
                        if (this.IsLoad)
                            ((ImageEnhancement)this._function).EnhancementOperator.EnhancementParam = new MeanCurvatureFlowParam(this.方法comboBox.SelectedItem.ToString());
                        AddForm(this.splitContainer1.Panel2, new MeanCurvatureFlowForm(((ImageEnhancement)this._function).EnhancementOperator.EnhancementParam));
                        break;
                    case nameof(enImageEnhancementMethod.scale_image_max):
                    case nameof(enImageEnhancementMethod.scale_image):
                        if (this.IsLoad)
                            ((ImageEnhancement)this._function).EnhancementOperator.EnhancementParam = new ScaleImageMaxParam(this.方法comboBox.SelectedItem.ToString());
                        AddForm(this.splitContainer1.Panel2, new ScaleImageMaxForm(((ImageEnhancement)this._function).EnhancementOperator.EnhancementParam));
                        break;
                    case nameof(enImageEnhancementMethod.shock_filter):
                        if (this.IsLoad)
                            ((ImageEnhancement)this._function).EnhancementOperator.EnhancementParam = new ShockFilterParam(this.方法comboBox.SelectedItem.ToString());
                        AddForm(this.splitContainer1.Panel2, new ShockFilterForm(((ImageEnhancement)this._function).EnhancementOperator.EnhancementParam));
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void DoImageMorphologyForm_Load(object sender, EventArgs e)
        {
            this.IsLoad = true;
        }
    }
}

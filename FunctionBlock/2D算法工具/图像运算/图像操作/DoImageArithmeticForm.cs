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
    public partial class DoImageArithmeticForm : Form
    {
        private bool IsLoad = false;
        private IFunction _function;
        public DoImageArithmeticForm(IFunction param)
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
                foreach (var item in Enum.GetValues(typeof(enImageArithmeticMethod)))
                    this.方法comboBox.Items.Add(item);
                //this.方法comboBox.DataSource = Enum.GetNames(typeof(enImageArithmeticMethod)); // 这里不要使用数据源绑定来添加 Items 属性值
                ////////////////////////////////////////////////
                this.方法comboBox.DataBindings.Add(nameof(this.方法comboBox.Text), ((ImageArithmetic)this._function).ArithmeticOperator.ArithmeticParam, "Method", true, DataSourceUpdateMode.OnPropertyChanged);
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
            }
        }

        private void 方法comboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            //try
            //{
            //    if (this.方法comboBox.SelectedItem == null) return;
            //    switch (this.方法comboBox.SelectedItem.ToString())
            //    {
            //        case nameof(enImageMorphology.gray_closing_rect):
            //        case nameof(enImageMorphology.gray_dilation_rect):
            //        case nameof(enImageMorphology.gray_erosion_rect):
            //        case nameof(enImageMorphology.gray_opening_rect):
            //            if (this.IsLoad)
            //                ((DoImageMorphology)this._function).ImageMorphology.MorphologyParam = new ImageRectParam();
            //            AddForm(this.splitContainer1.Panel2, new ImageRectMorphologyForm(((DoImageMorphology)this._function).ImageMorphology.MorphologyParam));
            //            break;
            //        case nameof(enImageMorphology.gray_closing_shape):
            //        case nameof(enImageMorphology.gray_dilation_shape):
            //        case nameof(enImageMorphology.gray_erosion_shape):
            //        case nameof(enImageMorphology.gray_opening_shape):
            //            if (this.IsLoad)
            //                ((DoImageMorphology)this._function).ImageMorphology.MorphologyParam = new ImageShapeParam();
            //            AddForm(this.splitContainer1.Panel2, new ImageShapeMorphologyForm(((DoImageMorphology)this._function).ImageMorphology.MorphologyParam));
            //            break;                  
            //    }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.ToString());
            //}
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
                    case nameof(enImageArithmeticMethod.add_image):
                        if (this.IsLoad)
                            ((ImageArithmetic)this._function).ArithmeticOperator.ArithmeticParam = new MultAddParam(this.方法comboBox.SelectedItem.ToString());
                        AddForm(this.splitContainer1.Panel2, new MultAddParamForm(((ImageArithmetic)this._function).ArithmeticOperator.ArithmeticParam));
                        break;
                    case nameof(enImageArithmeticMethod.div_image):
                        if (this.IsLoad)
                            ((ImageArithmetic)this._function).ArithmeticOperator.ArithmeticParam = new MultAddParam(this.方法comboBox.SelectedItem.ToString());
                        AddForm(this.splitContainer1.Panel2, new MultAddParamForm(((ImageArithmetic)this._function).ArithmeticOperator.ArithmeticParam));
                        break;
                    case nameof(enImageArithmeticMethod.mult_image):
                        if (this.IsLoad)
                            ((ImageArithmetic)this._function).ArithmeticOperator.ArithmeticParam = new MultAddParam(this.方法comboBox.SelectedItem.ToString());
                        AddForm(this.splitContainer1.Panel2, new MultAddParamForm(((ImageArithmetic)this._function).ArithmeticOperator.ArithmeticParam));
                        break;
                    case nameof(enImageArithmeticMethod.sub_image):
                        if (this.IsLoad)
                            ((ImageArithmetic)this._function).ArithmeticOperator.ArithmeticParam = new MultAddParam(this.方法comboBox.SelectedItem.ToString());
                        AddForm(this.splitContainer1.Panel2, new MultAddParamForm(((ImageArithmetic)this._function).ArithmeticOperator.ArithmeticParam));
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

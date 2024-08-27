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
    public partial class SelectShapeStdForm : Form
    {
        private SelectParamValue _param;
        private object _paramList;
        public SelectShapeStdForm(SelectParamValue param)
        {
            this._param = param;
            InitializeComponent();
            BindProperty();
        }
        public SelectShapeStdForm(BindingList<SelectShapeStdParam> param)
        {
            this._paramList = param ; 
            InitializeComponent();
            BindProperty();
        }
        public SelectShapeStdForm(object param)
        {
            this._paramList = param;
            InitializeComponent();
            BindProperty();
        }
        private void BindProperty()
        {
            try
            {
                // 创建匹配参数
                //this.特征ComboBox.DataBindings.Add(nameof(this.特征ComboBox.Text), ((SelectShapeStdParam)this._param), "Features", true, DataSourceUpdateMode.OnPropertyChanged);
                //this.百分比comboBox.DataBindings.Add(nameof(this.百分比comboBox.Text), ((SelectShapeStdParam)this._param), "Percent", true, DataSourceUpdateMode.OnPropertyChanged);
                //this.合并区域checkBox.DataBindings.Add(nameof(this.合并区域checkBox.Checked), ((SelectShapeStdParam)this._param), "IsUnion", true, DataSourceUpdateMode.OnPropertyChanged);
                this.FeatureCol.Items.Clear();
                this.FeatureCol.ValueType = typeof(enSelectStdFeatures);
                foreach (enSelectStdFeatures temp in Enum.GetValues(typeof(enSelectStdFeatures)))
                    this.FeatureCol.Items.Add(temp);
                this.dataGridView1.DataSource = _paramList;
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
            }
        }

        private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false;
        }


    }
}

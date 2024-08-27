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
    public partial class SelectShapeForm : Form
    {
        private SelectParamValue _param;
        private object _paramList;
        public SelectShapeForm(SelectParamValue param)
        {
            this._param = param;
            InitializeComponent();
            BindProperty();
        }
        public SelectShapeForm(BindingList<SelectShapeParam> param)
        {
            this._paramList = param;
            InitializeComponent();
            BindProperty();
        }
        public SelectShapeForm(object param)
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
                //this.特征ComboBox.DataBindings.Add(nameof(this.特征ComboBox.Text), ((SelectShapeParam)this._param), "Features", true, DataSourceUpdateMode.OnPropertyChanged);
                //this.操作comboBox.DataBindings.Add(nameof(this.操作comboBox.Text), ((SelectShapeParam)this._param), "Operation", true, DataSourceUpdateMode.OnPropertyChanged);
                //this.最小值comboBox.DataBindings.Add(nameof(this.最小值comboBox.Text), ((SelectShapeParam)this._param), "Min", true, DataSourceUpdateMode.OnPropertyChanged);
                //this.最大值comboBox.DataBindings.Add(nameof(this.最大值comboBox.Text), ((SelectShapeParam)this._param), "Max", true, DataSourceUpdateMode.OnPropertyChanged);
                //this.合并区域checkBox.DataBindings.Add(nameof(this.合并区域checkBox.Checked), ((SelectShapeParam)this._param), "IsUnion", true, DataSourceUpdateMode.OnPropertyChanged);

                this.FeatureCol.Items.Clear();
                this.FeatureCol.ValueType = typeof(enSelectShapeFeatures);
                foreach (enSelectShapeFeatures temp in Enum.GetValues(typeof(enSelectShapeFeatures)))
                    this.FeatureCol.Items.Add(temp);
                this.OperaterCol.Items.Clear();
                this.OperaterCol.ValueType = typeof(enOperation);
                foreach (enOperation temp in Enum.GetValues(typeof(enOperation)))
                    this.OperaterCol.Items.Add(temp);
                this.dataGridView1.DataSource = this._paramList;
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

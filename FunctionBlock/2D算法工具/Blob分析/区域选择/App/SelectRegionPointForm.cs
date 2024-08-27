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
    public partial class SelectRegionPointForm : Form
    {
        private SelectParamValue _param;
        private BindingList<SelectRegionPointParam> _paramList;
        public SelectRegionPointForm(SelectParamValue param)
        {
            this._param = param;
            InitializeComponent();
            BindProperty();
        }
        public SelectRegionPointForm(BindingList<SelectRegionPointParam> param)
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
                //SelectRegionPointParam pointParam = ((SelectRegionPointParam)this._param);
                //this.行坐标ComboBox.DataBindings.Add(nameof(this.行坐标ComboBox.Text), pointParam, nameof(pointParam.Row), true, DataSourceUpdateMode.OnPropertyChanged);
                //this.列坐标comboBox.DataBindings.Add(nameof(this.列坐标comboBox.Text), pointParam, nameof(pointParam.Col), true, DataSourceUpdateMode.OnPropertyChanged);
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

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}

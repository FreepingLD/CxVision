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
    public partial class SelectRegionParamForm : Form
    {
        private SelectParamValue _param;
        public SelectRegionParamForm(SelectParamValue param)
        {
            this._param = param;
            InitializeComponent();
            BindProperty();
        }

        private void BindProperty()
        {
            try
            {
                // 创建匹配参数
                SelectRegionDistParam pointParam = ((SelectRegionDistParam)this._param);
                this.区域距离ComboBox.DataBindings.Add(nameof(this.区域距离ComboBox.Text), pointParam, nameof(pointParam.Dist), true, DataSourceUpdateMode.OnPropertyChanged);
                this.区域数量comboBox.DataBindings.Add(nameof(this.区域数量comboBox.Text), pointParam, nameof(pointParam.Count), true, DataSourceUpdateMode.OnPropertyChanged);
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
            }
        }


    }
}

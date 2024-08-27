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
    public partial class CircleMorphologyForm : Form
    {
        private RegionMorphologyParam _param;
        public CircleMorphologyForm(RegionMorphologyParam param)
        {
            this._param = param;
            InitializeComponent();
            BindProperty();
        }

        private void BindProperty()
        {
            try
            {
                // 创建匹配参数 IsFill
                this.半径ComboBox.DataBindings.Add(nameof(this.半径ComboBox.Text), ((RegionCircleParam)this._param), "Radius", true, DataSourceUpdateMode.OnPropertyChanged);
                this.区域连通comboBox.DataBindings.Add(nameof(this.区域连通comboBox.Text), ((RegionCircleParam)this._param), "IsConnection", true, DataSourceUpdateMode.OnPropertyChanged);
                this.区域填充comboBox.DataBindings.Add(nameof(this.区域填充comboBox.Text), ((RegionCircleParam)this._param), "IsFill", true, DataSourceUpdateMode.OnPropertyChanged);
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
            }
        }
    }
}

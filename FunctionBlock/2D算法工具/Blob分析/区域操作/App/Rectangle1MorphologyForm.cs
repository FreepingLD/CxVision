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
    public partial class Rectangle1MorphologyForm : Form
    {
        private RegionMorphologyParam _param;
        public Rectangle1MorphologyForm(RegionMorphologyParam param)
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
                this.宽度ComboBox.DataBindings.Add(nameof(this.宽度ComboBox.Text), ((RegionRectParam)this._param), "Width", true, DataSourceUpdateMode.OnPropertyChanged);
                this.高度ComboBox.DataBindings.Add(nameof(this.高度ComboBox.Text), ((RegionRectParam)this._param), "Height", true, DataSourceUpdateMode.OnPropertyChanged);
                this.区域连通comboBox.DataBindings.Add(nameof(this.区域连通comboBox.Text), ((RegionRectParam)this._param), "IsConnection", true, DataSourceUpdateMode.OnPropertyChanged);
                this.区域填充comboBox.DataBindings.Add(nameof(this.区域填充comboBox.Text), ((RegionRectParam)this._param), "IsFill", true, DataSourceUpdateMode.OnPropertyChanged);
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
            }
        }
    }
}

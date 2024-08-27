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
    public partial class ShapeTransForm : Form
    {
        private RegionMorphologyParam _param;
        public ShapeTransForm(RegionMorphologyParam param)
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
                this.形状ComboBox.DataSource = Enum.GetValues(typeof(enShapeTransType));
                this.形状ComboBox.DataBindings.Add(nameof(this.形状ComboBox.Text), ((ShapeTransParam)this._param), "ShapeType", true, DataSourceUpdateMode.OnPropertyChanged);
                this.区域连通comboBox.DataBindings.Add(nameof(this.区域连通comboBox.Text), ((ShapeTransParam)this._param), "IsConnection", true, DataSourceUpdateMode.OnPropertyChanged);
                this.区域填充comboBox.DataBindings.Add(nameof(this.区域填充comboBox.Text), ((ShapeTransParam)this._param), "IsFill", true, DataSourceUpdateMode.OnPropertyChanged);
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
            }
        }
    }
}

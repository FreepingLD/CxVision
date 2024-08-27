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
    public partial class SelectContourFeatureXLDForm : Form
    {
        IFunction _function;
        public SelectContourFeatureXLDForm(IFunction function)
        {
            this._function = function;
            InitializeComponent();
            BindProperty();
        }

        private void BindProperty()
        {
            try
            {
                // 创建匹配参数
                this.特征comboBox.DataSource = Enum.GetNames(typeof(SelectContoursXld.enSelectContourParams));
                this.特征comboBox.DataBindings.Add("Text", ((SelectionXLD)this._function).SelectContours, "SelectContourFeatures", true, DataSourceUpdateMode.OnPropertyChanged);
                this.最小特征值1textBox.DataBindings.Add("Text", ((SelectionXLD)this._function).SelectContours, "MinContourFeaturesValue1", true, DataSourceUpdateMode.OnPropertyChanged);
                this.最大特征值1textBox.DataBindings.Add("Text", ((SelectionXLD)this._function).SelectContours, "MaxContourFeaturesValue1", true, DataSourceUpdateMode.OnPropertyChanged);
                this.最小特征值2textBox.DataBindings.Add("Text", ((SelectionXLD)this._function).SelectContours, "MinContourFeaturesValue2", true, DataSourceUpdateMode.OnPropertyChanged);
                this.最大特征值2textBox.DataBindings.Add("Text", ((SelectionXLD)this._function).SelectContours, "MaxContourFeaturesValue2", true, DataSourceUpdateMode.OnPropertyChanged);
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
            }
        }

        private void 特征comboBox_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            switch (this.特征comboBox.SelectedItem.ToString())
            {
                case "contour_length":
                    this.最小特征值1textBox.Enabled = true;
                    this.最小特征值1textBox.Enabled = true;
                    this.最小特征值2textBox.Enabled = false;
                    this.最大特征值2textBox.Enabled = false;
                    break;
                case "maximum_extent":
                    this.最小特征值1textBox.Enabled = true;
                    this.最小特征值1textBox.Enabled = true;
                    this.最小特征值2textBox.Enabled = false;
                    this.最大特征值2textBox.Enabled = false;
                    break;
                case "direction":
                    this.最小特征值1textBox.Enabled = true;
                    this.最小特征值1textBox.Enabled = true;
                    this.最小特征值2textBox.Enabled = false;
                    this.最大特征值2textBox.Enabled = false;
                    break;
                case "curvature":
                    this.最小特征值1textBox.Enabled = true;
                    this.最小特征值1textBox.Enabled = true;
                    this.最小特征值2textBox.Enabled = true;
                    this.最大特征值2textBox.Enabled = true;
                    break;
                case "closed":
                    this.最小特征值1textBox.Enabled = false;
                    this.最小特征值1textBox.Enabled = true;
                    this.最小特征值2textBox.Enabled = false;
                    this.最大特征值2textBox.Enabled = false;
                    break;
                case "open":
                    this.最小特征值1textBox.Enabled = true;
                    this.最小特征值1textBox.Enabled = false;
                    this.最小特征值2textBox.Enabled = false;
                    this.最大特征值2textBox.Enabled = false;
                    break;
            }
        }
    }
}

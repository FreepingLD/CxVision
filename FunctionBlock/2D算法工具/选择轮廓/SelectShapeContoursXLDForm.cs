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
    public partial class SelectShapeContoursXLDForm : Form
    {
        IFunction _function;
        public SelectShapeContoursXLDForm(IFunction function)
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
                this.特征comboBox.DataSource = Enum.GetNames(typeof(SelectShapeXld.enSelectShapeXLDParams));
                this.特征操作comboBox.DataSource = Enum.GetNames(typeof(SelectShapeXld.enSelectShapeOperation));
                ///////////////////////////////////////
                this.特征comboBox.DataBindings.Add("Text", ((SelectionXLD)this._function).SelectShape, "SelectShapeFeatures", true, DataSourceUpdateMode.OnPropertyChanged);
                this.特征操作comboBox.DataBindings.Add("Text", ((SelectionXLD)this._function).SelectShape, "ShapeOperation", true, DataSourceUpdateMode.OnPropertyChanged);
                this.最大特征值textBox.DataBindings.Add("Text", ((SelectionXLD)this._function).SelectShape, "MaxShapeFeaturesValue", true, DataSourceUpdateMode.OnPropertyChanged);
                this.最小特征值textBox.DataBindings.Add("Text", ((SelectionXLD)this._function).SelectShape, "MinShapeFeaturesValue", true, DataSourceUpdateMode.OnPropertyChanged);
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
            }
        }

        private void SelectShapeContoursXLDForm_Load(object sender, EventArgs e)
        {

        }


    }
}

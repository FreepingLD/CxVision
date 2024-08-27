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
    public partial class SimplifyObjectModel3DParamForm : Form
    {
        IFunction _function;
        public SimplifyObjectModel3DParamForm(IFunction function)
        {
            this._function = function;
            InitializeComponent();
            BindProperty();
        }
        private void BindProperty()
        {
            try
            {
                // 绑定机台坐标点 ，绑定类型只能为C#使用的数据类型
                this.简化类型comboBox.DataSource = Enum.GetNames(typeof(SampleObjectModel3D.enAmount_type));
                this.简化类型comboBox.DataBindings.Add("Text", (SampleObjectModel3D)this._function, "Amount_type", true, DataSourceUpdateMode.OnPropertyChanged);
                this.简化百分比textBox.DataBindings.Add("Text", (SampleObjectModel3D)this._function, "Amount", true, DataSourceUpdateMode.OnPropertyChanged);
                this.三角形反向comboBox.DataBindings.Add("Text", (SampleObjectModel3D)this._function, "Avoid_triangle_flips", true, DataSourceUpdateMode.OnPropertyChanged);     
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
            }
        }


    }
}

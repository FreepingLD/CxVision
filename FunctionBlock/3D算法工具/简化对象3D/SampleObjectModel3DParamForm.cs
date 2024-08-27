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
    public partial class SampleObjectModel3DParamForm : Form
    {
        IFunction _function;
        public SampleObjectModel3DParamForm(IFunction function)
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
                this.采样方法comboBox.DataSource = Enum.GetNames(typeof(SampleObjectModel3D.enSampleMethod));
                this.采样距离textBox.DataBindings.Add("Text", (SampleObjectModel3D)this._function, "SampleDist", true, DataSourceUpdateMode.OnPropertyChanged);
                this.采样方法comboBox.DataBindings.Add("Text", (SampleObjectModel3D)this._function, "SampleMethod", true, DataSourceUpdateMode.OnPropertyChanged);
                this.角度偏差textBox.DataBindings.Add("Text", (SampleObjectModel3D)this._function, "Max_angle_diff", true, DataSourceUpdateMode.OnPropertyChanged);
                this.最小点数textBox.DataBindings.Add("Text", (SampleObjectModel3D)this._function, "Min_num_points", true, DataSourceUpdateMode.OnPropertyChanged);
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
            }
        }


    }
}

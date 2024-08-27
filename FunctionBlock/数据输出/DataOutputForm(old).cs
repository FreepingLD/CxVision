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
    public partial class DataOutputFormOld : Form
    {
        IFunction _function;
        public DataOutputFormOld(IFunction function,TreeNode node)
        {
            InitializeComponent();
            this._function = function;
            new ListBoxWrapClass().InitListBox(this.listBox1, node);
        }

        private void DataOutputForm_Load(object sender, EventArgs e)
        {
            this.titlelabel.Text = this._function.GetPropertyValues("名称").ToString();
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                Task.Run(() => this._function.Execute(null));
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
            }
        }
    }
}

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
    public partial class ExtractPointForm : Form
    {
        private IFunction _function;
        private TreeNode _refNode;
        public ExtractPointForm(IFunction function)
        {
            InitializeComponent();
            this._function = function;
        }
        public ExtractPointForm(TreeNode node)
        {
            InitializeComponent();
            this._refNode = node;
        }
        private void ExtractPointForm_Load(object sender, EventArgs e)
        {
            this.titlelabel.Text = this._function.GetPropertyValues("名称").ToString();
            new ListBoxWrapClass().InitListBox(this.listBox1, this._function);
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

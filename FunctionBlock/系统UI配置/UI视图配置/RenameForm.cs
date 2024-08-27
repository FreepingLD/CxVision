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
    public partial class RenameForm : Form
    {
        public string ReName { get; set; }

        public RenameForm()
        {
            InitializeComponent();
        }
        public RenameForm(string name)
        {
            InitializeComponent();
            this.ReName = name;
            this.textBox1.Text = name;
        }
        private void 确定button_Click(object sender, EventArgs e)
        {
            this.Close();
            this.DialogResult = DialogResult.OK;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                ReName = this.textBox1.Text;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void 取消Btn_Click(object sender, EventArgs e)
        {
            this.Close();
            this.DialogResult = DialogResult.Cancel;
        }

        private void RenameForm_Load(object sender, EventArgs e)
        {

        }
    }
}

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
    public partial class ResetNameForm : Form
    {

        public string oldName { get; set; }
        public string newName { get; set; }
        public bool IsOk { get; set; }
        public ResetNameForm()
        {
            InitializeComponent();
        }
        public ResetNameForm(string name)
        {
            InitializeComponent();
            this.oldName = name;
            this.旧名称textBox.Text = name;
        }
        private void 确定button_Click(object sender, EventArgs e)
        {
            this.IsOk = true;
            this.Close();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                oldName = this.旧名称textBox.Text;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void 新名称textBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                this.newName = this.新名称textBox.Text;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void 取消Btn_Click(object sender, EventArgs e)
        {
            this.IsOk = false;
            this.Close();
        }


    }
}

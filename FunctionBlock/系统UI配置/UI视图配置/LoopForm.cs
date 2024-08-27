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
    public partial class LoopForm : Form
    {
        public bool IsClose { get; set; }
        public int loopCount { get; set; } = 1;
        public LoopForm()
        {
            InitializeComponent();
            this.IsClose = false;
        }

        private void 确定button_Click(object sender, EventArgs e)
        {
            this.Close();
            this.IsClose = false;
        }

        private void LoopTextBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                int count = 1;
                int.TryParse(LoopTextBox.Text, out count);
                this.loopCount = count;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void LoopForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.IsClose = true;
        }
    }
}

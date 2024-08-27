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
    public partial class TaskForm : Form
    {
        public bool IsClose { get; set; }
        public object content { get; set; }
        public TaskForm()
        {
            InitializeComponent();
            this.IsClose = false;
        }

        private void 确定button_Click(object sender, EventArgs e)
        {
            this.Close();
            this.IsClose = true;
        }

        private void LoopTextBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (!LoopTextBox.Text.Contains("任务"))
                    this.content = "任务-" + LoopTextBox.Text;
                else
                    this.content = LoopTextBox.Text;
            }
            catch
            {

            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace userControl
{
    public partial class CheckSpilit : UserControl
    {
        public CheckSpilit()
        {
            InitializeComponent();
        }

        [Browsable(true)]
        public string Content
        {
            get { return this.spilit1.Content; }
            set { this.spilit1.Content = value; }
        }

        [Browsable(true)]
        public bool Checked
        {
            get { return this.checkBox1.Checked; }
            set { this.checkBox1.Checked = value; }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.checkBox1.Checked)
                {
                    this.checkBox1.BackgroundImage = global::userControl.Properties.Resources.展开;
                }               
                else
                {
                    this.checkBox1.BackgroundImage = global::userControl.Properties.Resources.收起;
                }                 
            }
            catch
            {

            }
        }

    }
}

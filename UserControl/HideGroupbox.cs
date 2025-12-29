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
    public partial class HideGroupbox : UserControl
    {
        public HideGroupbox()
        {
            InitializeComponent();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.checkBox1.Checked)
                {
                    this.checkBox1.BackgroundImage = global::userControl.Properties.Resources.展开;
                    this.Height = 20;
                }               
                else
                {
                    this.checkBox1.BackgroundImage = global::userControl.Properties.Resources.收起;
                    this.Height = 20 + this.PanelContent.Height;
                }                 
            }
            catch
            {

            }
        }

    }
}

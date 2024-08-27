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
    public partial class SpilitLine : UserControl
    {
        public SpilitLine()
        {
            InitializeComponent();
        }
        [Browsable(true)]
        public string Content
        {
            get { return this.Title.Text; }
            set { this.Title.Text = value; }
        }
    }
}

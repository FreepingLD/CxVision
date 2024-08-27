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
    public partial class TabletForm : Form
    {
        private string _content;
        public TabletForm()
        {
            InitializeComponent();
        }

        public TabletForm(string content)
        {
            InitializeComponent();
            this._content = content;
        }
        private void TabletForm_Load(object sender, EventArgs e)
        {
            try
            {
                if(this._content != null && this._content.Length > 0)
                {
                    this.listBox1.Items.Clear();
                    string[] value = this._content.Split(',', ';', ':');
                    int index = 1;
                    foreach (var item in value)
                    {
                        this.listBox1.Items.Add(index + ": " + item);
                        index++;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}

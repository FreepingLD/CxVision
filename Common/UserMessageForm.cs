using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Common
{
    public partial class UserMessageForm : Form
    {
        public DialogResult resultDialog;
        private bool isBreak;
        public UserMessageForm()
        {
            InitializeComponent();
            this.resultDialog = DialogResult.None;
        }

        public UserMessageForm(Form form, string content, string title = null)
        {
            InitializeComponent();
            if (title != null)
                this.Text = title;
            this.Owner = form;
            this.resultDialog = DialogResult.None;
            this.richTextBox1.Text = content;
        }

        public UserMessageForm(string content, string title = null)
        {
            InitializeComponent();
            if (title != null)
                this.Text = title;
            this.resultDialog = DialogResult.None;
            this.richTextBox1.Text = content;
        }


        private void 确定Btn_Click(object sender, EventArgs e)
        {
            this.isBreak = false;
            DialogResult = DialogResult.OK;
            this.Close();
        }

        private void 取消Btn_Click(object sender, EventArgs e)
        {
            this.isBreak = false;
            DialogResult = DialogResult.Cancel;
            this.Close();
        }

        public DialogResult ShowDialog(Form owner = null)
        {
            if (owner != null)
            {
                this.TopMost = true;
                this.Owner = owner;
            }
            else
            {
                this.TopMost = true;
                this.ShowInTaskbar = true;
            }
            this.isBreak = true;
            this.Show();
            while (isBreak)
            {
                Application.DoEvents();
            }
            return this.DialogResult;
        }

        public DialogResult ShowDialog(string content, string title = null)
        {
            if (title != null)
                this.Text = title;
            this.richTextBox1.Text = content;
            this.TopMost = true;
            this.ShowInTaskbar = true;
            this.isBreak = true;
            this.Show();
            while (isBreak)
            {
                Application.DoEvents();
            }
            return this.DialogResult;
        }

        private void UserMessageForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.resultDialog = DialogResult.None;
            this.isBreak = false;
        }



    }


}

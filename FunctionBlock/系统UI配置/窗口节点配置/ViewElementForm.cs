using Common;
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
    public partial class ViewElementForm : Form
    {
        private ViewConfigParam _viewConfigParam;
        public ViewElementForm()
        {
            InitializeComponent();
            this.TopMost = true;
            this.ShowInTaskbar = true;
            this.StartPosition = FormStartPosition.Manual;
            this.Location = System.Windows.Forms.Cursor.Position;
            foreach (var item in ViewConfigParamManager.Instance.ViewParamList)
            {
                this.listBox1.Items.Add(item.ViewName);
            }
        }
        public ViewElementForm(ViewConfigParam viewConfigParam)
        {
            InitializeComponent();
            this.TopMost = true;
            this.ShowInTaskbar = true;
            this.StartPosition = FormStartPosition.Manual;
            this.Location = System.Windows.Forms.Cursor.Position;
            if (viewConfigParam == null)
                this._viewConfigParam = new ViewConfigParam();
            else
                this._viewConfigParam = viewConfigParam;
            foreach (var item in ViewConfigParamManager.Instance.ViewParamList)
            {
                this.listBox1.Items.Add(item.ViewName);
            }
        }
        private void ViewElementForm_FormClosing(object sender, FormClosingEventArgs e)
        {

        }

 

        private void 确定button_Click(object sender, EventArgs e)
        {
            try
            {
                if(ViewConfigParamManager.Instance.Save()) new UserMessageForm().ShowDialog("保存成功!!!");
                else new UserMessageForm().ShowDialog("保存失败!!!");
            }
            catch (Exception ex)
            {
               new Common.UserMessageForm(ex.ToString()).ShowDialog();;
            }
        }

        private void 移除button_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.listBox1.SelectedIndex == -1) return;
                ViewConfigParamManager.Instance.ViewParamList.RemoveAt(this.listBox1.SelectedIndex);
                this.listBox1.Items.Remove(this.listBox1.SelectedItem);
            }
            catch(Exception ex)
            {
               new Common.UserMessageForm(ex.ToString()).ShowDialog();;
            }
        }





    }
}

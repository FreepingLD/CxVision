using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FunctionBlock
{
    public partial class PermissionForm : Form
    {
        
        List<Permission> _list;
        public PermissionForm()
        {
            InitializeComponent();
        }
        public PermissionForm(List<Permission> list)
        {
            InitializeComponent();
            this._list = list;
            this.TopMost = true;
            this.ShowInTaskbar = true;
            if (this._list.Any(p => p.Code == enPermissionCode.操作))
                this.操作checkBox.Checked = true;
            if (this._list.Any(p => p.Code == enPermissionCode.编辑))
                this.编辑checkBox.Checked = true;
            if (this._list.Any(p => p.Code == enPermissionCode.管理))
                this.管理checkBox.Checked = true;
            if (this._list.Any(p => p.Code == enPermissionCode.开发))
                this.开发checkBox.Checked = true;
        }

        private void 确定button_Click(object sender, EventArgs e)
        {
            try
            {
                this._list.Clear();
                if (this.操作checkBox.Checked)
                    this._list.Add(new Permission(enPermissionCode.操作));
                if (this.编辑checkBox.Checked)
                    this._list.Add(new Permission(enPermissionCode.编辑));
                if (this.管理checkBox.Checked)
                    this._list.Add(new Permission(enPermissionCode.管理));
                if (this.开发checkBox.Checked)
                    this._list.Add(new Permission(enPermissionCode.开发));
                this.DialogResult = DialogResult.OK;
            }
            catch
            {
            }
        }

        private void 取消Btn_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }


    }
}

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
    public partial class LoginFormNew : Form
    {
        private static object lockState = new object();
        private static LoginFormNew _Instance;

        public static LoginFormNew Instance
        {
            get
            {
                if (_Instance == null)
                {
                    lock (lockState)
                    {
                        _Instance = new LoginFormNew();
                    }
                }
                return _Instance;
            }
        }

        public LoginFormNew()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.Manual;
            this.Location = System.Windows.Forms.Cursor.Position;
            this.TopMost = true;
            this.ShowInTaskbar = true;
            this.用户名comboBox.Items.Clear();
            this.用户名comboBox.DataSource = Enum.GetValues(typeof(enUserName));
            //////////////////////////////////////////////////////////
            this.添加用户Btn.Enabled = UserManager.HasPermission(enPermissionCode.管理 | enPermissionCode.开发);
            this.删除用户Btn.Enabled = UserManager.HasPermission(enPermissionCode.管理 | enPermissionCode.开发);
            this.修改密码Btn.Enabled = UserManager.HasPermission(enPermissionCode.管理 | enPermissionCode.开发);
            this.修改权限Btn.Enabled = UserManager.HasPermission(enPermissionCode.管理 | enPermissionCode.开发);
            this.用户名comboBox.Text = UserManager.CurrentUser.UserName.ToString();
        }

        public LoginFormNew(ToolStripMenuItem item)
        {
            InitializeComponent();
            this.用户名comboBox.DataSource = Enum.GetValues(typeof(enUserName));
            this.添加用户Btn.Enabled = UserManager.HasPermission(enPermissionCode.管理 | enPermissionCode.开发);
            this.删除用户Btn.Enabled = UserManager.HasPermission(enPermissionCode.管理 | enPermissionCode.开发);
            this.修改密码Btn.Enabled = UserManager.HasPermission(enPermissionCode.管理 | enPermissionCode.开发);
            this.修改权限Btn.Enabled = UserManager.HasPermission(enPermissionCode.管理 | enPermissionCode.开发);
            this.用户名comboBox.Text = UserManager.CurrentUser.UserName.ToString();
        }

        private void 确定button_Click(object sender, EventArgs e)
        {
            try
            {
                string pass = this.密码textBox.Text;
                enUserName userName = enUserName.操作员;
                Enum.TryParse(this.用户名comboBox.SelectedItem.ToString(), out userName);
                if (UserManager.Login(userName, pass))
                {
                    this.添加用户Btn.Enabled = UserManager.HasPermission(enPermissionCode.管理 | enPermissionCode.开发);
                    this.删除用户Btn.Enabled = UserManager.HasPermission(enPermissionCode.管理 | enPermissionCode.开发);
                    this.修改密码Btn.Enabled = UserManager.HasPermission(enPermissionCode.管理 | enPermissionCode.开发);
                    this.修改权限Btn.Enabled = UserManager.HasPermission(enPermissionCode.管理 | enPermissionCode.开发);
                    new UserMessageForm().ShowDialog("登录成功");
                    this.Close();
                }
                else
                    new UserMessageForm().ShowDialog("登录失败，用户名或密码错误");
            }
            catch (Exception ex)
            {
                new Common.UserMessageForm(ex.ToString()).ShowDialog(); ;
            }
        }

        private void LoginFormNew_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                UserManager.Save();
            }
            catch
            {

            }
        }

        private void 添加用户Btn_Click(object sender, EventArgs e)
        {
            try
            {
                enUserName userName = enUserName.操作员;
                Enum.TryParse(this.用户名comboBox.SelectedItem.ToString(), out userName);
                if (UserManager.HasUser(userName))
                {
                    UserManager.UserList.Add(new User(userName, this.密码textBox.Text));
                    new UserMessageForm().ShowDialog("添加用户成功!");
                }
                else
                    new UserMessageForm().ShowDialog("当前用户名已添加，请重新指定用户名！");
            }
            catch (Exception ex)
            {
                new Common.UserMessageForm(ex.ToString()).ShowDialog(); ;
            }
        }

        private void 删除用户Btn_Click(object sender, EventArgs e)
        {
            try
            {
                enUserName userName = enUserName.操作员;
                Enum.TryParse(this.用户名comboBox.SelectedItem.ToString(), out userName);
                if (UserManager.HasUser(userName))
                {
                    UserManager.RemoveUser(userName);
                    new UserMessageForm().ShowDialog("删除用户成功!");
                }
            }
            catch (Exception ex)
            {
                new Common.UserMessageForm(ex.ToString()).ShowDialog(); ;
            }
        }

        private void 修改密码Btn_Click(object sender, EventArgs e)
        {
            try
            {
                enUserName userName = enUserName.操作员;
                Enum.TryParse(this.用户名comboBox.SelectedItem.ToString(), out userName);
                if (UserManager.HasUser(userName))
                {
                    User user = UserManager.GetUser(userName);
                    if (user != null)
                        user.Password = this.密码textBox.Text;
                    new UserMessageForm().ShowDialog("修改密码成功!");
                }
            }
            catch (Exception ex)
            {
                new Common.UserMessageForm(ex.ToString()).ShowDialog(); ;
            }
        }

        private void 修改权限Btn_Click(object sender, EventArgs e)
        {
            try
            {
                enUserName userName = enUserName.操作员;
                Enum.TryParse(this.用户名comboBox.SelectedItem.ToString(), out userName);
                if (UserManager.HasPermission(enPermissionCode.管理) || UserManager.HasPermission(enPermissionCode.开发))
                {
                    this.修改权限Btn.Enabled = true;
                    User user = UserManager.GetUser(userName);
                    if (new PermissionForm(user.Permissions).ShowDialog() == DialogResult.OK)
                        new UserMessageForm().ShowDialog("修改权限成功!");
                    else
                        new UserMessageForm().ShowDialog("修改权限失败!");
                }
            }
            catch (Exception ex)
            {
                new Common.UserMessageForm(ex.ToString()).ShowDialog(); ;
            }
        }



    }
}

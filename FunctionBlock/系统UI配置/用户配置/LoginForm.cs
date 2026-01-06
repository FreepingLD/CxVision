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
    public partial class LoginForm : Form
    {
        private static object lockState = new object();
        private static LoginForm _Instance;

        public static LoginForm Instance
        {
            get
            {
                if (_Instance == null)
                {
                    lock (lockState)
                    {
                        _Instance = new LoginForm();
                    }
                }
                return _Instance;
            }
        }

        public LoginForm()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.Manual;
            this.Location = System.Windows.Forms.Cursor.Position;
            this.TopMost = true;
            this.ShowInTaskbar = true;
            this.用户名comboBox.Items.Clear();
            this.用户名comboBox.DataSource = Enum.GetValues(typeof(enUserName));
            //////////////////////////////////////////////////////////
            switch (UserLoginParamManager.Instance.CurrentUser)
            {
                case enUserName.操作员:
                    this.添加用户Btn.Enabled = false;
                    this.删除用户Btn.Enabled = false;
                    this.修改密码Btn.Enabled = false;
                    break;
                default:
                    this.添加用户Btn.Enabled = true;
                    this.删除用户Btn.Enabled = true;
                    this.修改密码Btn.Enabled = true;
                    break;
            }
        }
        public LoginForm(ToolStripMenuItem item)
        {
            InitializeComponent();
            this.用户名comboBox.DataSource = Enum.GetValues(typeof(enUserName));
            //UserLoginParamManager.Instance.Read();
        }
        private void 确定button_Click(object sender, EventArgs e)
        {
            try
            {
                //string userName = this.用户名comboBox.SelectedItem.ToString();
                string pass = this.密码textBox.Text;
                enUserName userName = enUserName.操作员;
                Enum.TryParse(this.用户名comboBox.SelectedItem.ToString(), out userName);
                switch (userName)
                {
                    case enUserName.操作员:
                        this.添加用户Btn.Enabled = false;
                        this.删除用户Btn.Enabled = false;
                        this.修改密码Btn.Enabled = false;
                        UserLoginParamManager.Instance.CurrentUser = enUserName.操作员;
                        this.Close();
                        break;
                    case enUserName.工程师:
                        UserLoginParam loginParam = UserLoginParamManager.Instance.GetUser(enUserName.工程师);
                        if(loginParam != null)
                        {
                            if (pass == loginParam.Password)
                            {
                                this.添加用户Btn.Enabled = false;
                                this.删除用户Btn.Enabled = false;
                                this.修改密码Btn.Enabled = false;
                                ///////////////////////////////////////
                                loginParam.IsLogin = true;
                                UserLoginParamManager.Instance.CurrentUser = enUserName.工程师;
                                new UserMessageForm().ShowDialog("登录成功");
                                this.Close();
                            }
                            else
                                new UserMessageForm().ShowDialog("登录失败，密码错误");
                        }
                        else
                        {
                            new UserMessageForm().ShowDialog("指定的用户不存在!");
                        }

                        break;
                    case enUserName.开发人员:
                        loginParam = UserLoginParamManager.Instance.GetUser(enUserName.开发人员);
                        if (loginParam != null)
                        {
                            if (pass == loginParam.Password)
                            {
                                this.添加用户Btn.Enabled = true;
                                this.删除用户Btn.Enabled = true;
                                this.修改密码Btn.Enabled = true;
                                ///////////////////////////////////////
                                loginParam.IsLogin = true;
                                UserLoginParamManager.Instance.CurrentUser = enUserName.开发人员;
                                new UserMessageForm().ShowDialog("登录成功");
                                this.Close();
                            }
                            else
                                new UserMessageForm().ShowDialog("登录失败，密码错误");
                        }
                        else
                        {
                            new UserMessageForm().ShowDialog("指定的用户不存在!");
                        }
                        break;
                    case enUserName.管理员:
                        loginParam = UserLoginParamManager.Instance.GetUser(enUserName.管理员);
                        if (loginParam != null)
                        {
                            if (pass == loginParam.Password)
                            {
                                this.添加用户Btn.Enabled = false;
                                this.删除用户Btn.Enabled = false;
                                this.修改密码Btn.Enabled = true;
                                ///////////////////////////////////////
                                loginParam.IsLogin = true;
                                UserLoginParamManager.Instance.CurrentUser = enUserName.管理员;
                                new UserMessageForm().ShowDialog("登录成功");
                                this.Close();
                            }
                            else
                                new UserMessageForm().ShowDialog("登录失败，密码错误");
                        }
                        else
                        {
                            new UserMessageForm().ShowDialog("指定的用户不存在!");
                        }
                        break;
                    default:
                        throw new ArgumentNullException("指定的用户名不存在!");
                }
            }
            catch (Exception ex)
            {
               new Common.UserMessageForm(ex.ToString()).ShowDialog();;
            }
        }
        private void LoginForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                UserLoginParamManager.Instance.Save();
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
                if (UserLoginParamManager.Instance.GetUser(userName) == null)
                {
                    UserLoginParamManager.Instance.LoginParam.Add(new UserLoginParam(userName, this.密码textBox.Text));
                    new UserMessageForm().ShowDialog("添加用户成功!");
                }
                else
                    new UserMessageForm().ShowDialog("当前用户名已添加，请重新指定用户名！");
            }
            catch (Exception ex)
            {
               new Common.UserMessageForm(ex.ToString()).ShowDialog();;
            }
        }

        private void 删除用户Btn_Click(object sender, EventArgs e)
        {
            try
            {
                enUserName userName = enUserName.操作员;
                Enum.TryParse(this.用户名comboBox.SelectedItem.ToString(), out userName);
                UserLoginParam loginParam = UserLoginParamManager.Instance.GetUser(userName);
                if (loginParam != null)
                {
                    UserLoginParamManager.Instance.LoginParam.Remove(loginParam);
                    new UserMessageForm().ShowDialog("删除用户成功!");
                }
            }
            catch (Exception ex)
            {
               new Common.UserMessageForm(ex.ToString()).ShowDialog();;
            }
        }

        private void 修改密码Btn_Click(object sender, EventArgs e)
        {
            try
            {
                enUserName userName = enUserName.操作员;
                Enum.TryParse(this.用户名comboBox.SelectedItem.ToString(), out userName);
                UserLoginParam loginParam = UserLoginParamManager.Instance.GetUser(userName);
                if (loginParam != null)
                {
                    loginParam.Password = this.密码textBox.Text;
                    new UserMessageForm().ShowDialog("修改密码成功!");
                }
            }
            catch (Exception ex)
            {
               new Common.UserMessageForm(ex.ToString()).ShowDialog();;
            }
        }
    }
}

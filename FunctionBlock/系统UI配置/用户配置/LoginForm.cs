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
        public LoginForm()
        {
            InitializeComponent();
            this.用户名comboBox.Items.Clear();
            this.用户名comboBox.DataSource = Enum.GetValues(typeof(enUserName));
            //UserLoginParamManager.Instance.Read();
            //if (UserConfigParamManager.Instance.LoginParam.Count == 0)
            //    UserConfigParamManager.Instance.Init();
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
                        UserLoginParamManager.Instance.CurrentUser = enUserName.操作员;
                        //UserConfigParamManager.Instance.Save();
                        //MessageBox.Show("登录成功");
                        this.Close();
                        break;
                    case enUserName.工程师:
                        UserLoginParam loginParam = UserLoginParamManager.Instance.GetUser(enUserName.工程师);
                        if(loginParam != null)
                        {
                            if (pass == loginParam.Password)
                            {
                                loginParam.IsLogin = true;
                                UserLoginParamManager.Instance.CurrentUser = enUserName.工程师;
                                MessageBox.Show("登录成功");
                                this.Close();
                            }
                            else
                                MessageBox.Show("登录失败，密码错误");
                        }
                        else
                        {
                            MessageBox.Show("指定的用户不存在!");
                        }

                        break;
                    case enUserName.开发人员:
                        loginParam = UserLoginParamManager.Instance.GetUser(enUserName.开发人员);
                        if (loginParam != null)
                        {
                            if (pass == loginParam.Password)
                            {
                                loginParam.IsLogin = true;
                                UserLoginParamManager.Instance.CurrentUser = enUserName.开发人员;
                                MessageBox.Show("登录成功");
                                this.Close();
                            }
                            else
                                MessageBox.Show("登录失败，密码错误");
                        }
                        else
                        {
                            MessageBox.Show("指定的用户不存在!");
                        }
                        break;
                    default:
                        throw new ArgumentNullException("指定的用户名不存在!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
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
                    MessageBox.Show("添加用户成功!");
                }
                else
                    MessageBox.Show("当前用户名已添加，请重新指定用户名！");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
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
                    MessageBox.Show("删除用户成功!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
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
                    MessageBox.Show("修改密码成功!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}

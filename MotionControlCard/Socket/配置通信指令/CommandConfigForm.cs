using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MotionControlCard
{
    public partial class CommandConfigForm : Form
    {
        private CommandParam _commandParam;
        public CommandConfigForm()
        {
            InitializeComponent();
        }

        public CommandConfigForm(string motionName)
        {
            InitializeComponent();
            if (CommandConfigManger.Instance.IsContainCommandParam(motionName))
            {
                this._commandParam = CommandConfigManger.Instance.GetCommandParam(motionName);
            }
            else
            {
                CommandConfigManger.Instance.AddCommandParam(motionName);
                this._commandParam = CommandConfigManger.Instance.GetCommandParam(motionName);
            }
        }
        private void 添加Btn_Click(object sender, EventArgs e)
        {
            try
            {
                this.listBox1.Items.Add(this.命令列表comboBox.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void 删除Btn_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.listBox1.SelectedIndex >= 0)
                    this.listBox1.Items.RemoveAt(this.listBox1.SelectedIndex);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void 清空Btn_Click(object sender, EventArgs e)
        {
            try
            {
                this.listBox1.Items.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void 保存配置Btn_Click(object sender, EventArgs e)
        {
            try
            {
                if(this._commandParam == null)
                {
                    MessageBox.Show("命令对象为空!!!!");
                }
                else
                {
                    this._commandParam.ParamList.Clear();
                    foreach (var item in this.listBox1.Items)
                    {
                        this._commandParam.ParamList.Add(item.ToString());
                    }
                    CommandConfigManger.Instance.Save();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void CommandConfigForm_Load(object sender, EventArgs e)
        {
            try
            {
                this.命令列表comboBox.DataSource = SocketCommand.GetPropertyName();
                if(this._commandParam != null)
                {
                    foreach (var item in this._commandParam.ParamList)
                    {
                        this.listBox1.Items.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void 清空配置Btn_Click(object sender, EventArgs e)
        {
            try
            {
                CommandConfigManger.Instance.ClearCommandParam();
                CommandConfigManger.Instance.Save();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }





    }
}

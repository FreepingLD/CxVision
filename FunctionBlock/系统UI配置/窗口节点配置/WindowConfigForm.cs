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
    public partial class WindowConfigForm : Form
    {
        private ViewConfigParam _viewConfigParam;
        public WindowConfigForm()
        {
            InitializeComponent();
        }
        public WindowConfigForm(ViewConfigParam viewConfigParam)
        {
            InitializeComponent();
            if (viewConfigParam == null)
                this._viewConfigParam = new ViewConfigParam();
            else
                this._viewConfigParam = viewConfigParam;
            this.程序节点comboBox.Text = this._viewConfigParam.ProgramNode;
            this.程序附属节点1comboBox.Text = this._viewConfigParam.ProgramAttachNode1;
            this.程序附属节点2comboBox.Text = this._viewConfigParam.ProgramAttachNode2;
            this.示教节点comboBox.Text = this._viewConfigParam.TeachNode;
            this.对位节点comboBox.Text = this._viewConfigParam.AlignNode;
            this.拍照位comboBox.Text = this._viewConfigParam.GrabNo.ToString();
            //////////////////////////////////////////
            //this.Socket配置comboBox.Items.Clear();
            //foreach (var item in Common.SocketConnectManager.Instance.GetSocketName())
            //{
            //    this.Socket配置comboBox.Items.Add(item);
            //}
            //this.Socket配置comboBox.Text = this._viewConfigParam.SocketName;
        }
        private void WindowConfigForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            //try
            //{
            //    //ViewConfigParamManager.Instance.Save();
            //}
            //catch
            //{

            //}
        }

        private void 程序节点comboBox_DropDown(object sender, EventArgs e)
        {
            ProgramListForm form = new ProgramListForm();
            if (form.ShowDialog() == DialogResult.OK)
            {
                this.程序节点comboBox.Text = form.NodePath;
                this._viewConfigParam.ProgramNode = form.NodePath;
            }
            this.程序节点comboBox.Capture = true;
        }
        private void 程序附属节点1comboBox_DropDown(object sender, EventArgs e)
        {
            ProgramListForm form = new ProgramListForm();
            if (form.ShowDialog() == DialogResult.OK)
            {
                this.程序附属节点1comboBox.Text = form.NodePath;
                this._viewConfigParam.ProgramAttachNode1 = form.NodePath;
            }
            this.程序附属节点1comboBox.Capture = true;
        }

        private void 程序附属节点2comboBox_DropDown(object sender, EventArgs e)
        {
            ProgramListForm form = new ProgramListForm();
            if (form.ShowDialog() == DialogResult.OK)
            {
                this.程序附属节点2comboBox.Text = form.NodePath;
                this._viewConfigParam.ProgramAttachNode2 = form.NodePath;
            }
            this.程序附属节点2comboBox.Capture = true;
        }


        private void 示教节点comboBox_DropDown(object sender, EventArgs e)
        {
            int a = 10;
            this.示教节点comboBox.Items.Clear();
            ProgramListForm form = new ProgramListForm();
            if (form.ShowDialog() == DialogResult.OK)
            {
                this.示教节点comboBox.Text = form.NodePath;
                this._viewConfigParam.TeachNode = form.NodePath;
            }
            this.示教节点comboBox.Capture = true;
        }


        private void 对位节点comboBox_DropDown(object sender, EventArgs e)
        {
            ProgramListForm form = new ProgramListForm();
            if (form.ShowDialog() == DialogResult.OK)
            {
                this.对位节点comboBox.Text = form.NodePath;
                this._viewConfigParam.AlignNode = form.NodePath;
            }
            this.对位节点comboBox.Capture = true;
        }


        private void GetToolNode(TreeNode node, ComboBox comboBox)
        {
            if (node.Name.Contains("Tool"))
            {
                foreach (TreeNode item in node.Nodes)
                {
                    GetToolNode(item, comboBox);
                }
            }
            else
            {
                if (node.Parent == null)
                {
                    //string treeView = node.TreeView.Parent.Text;
                    if (!comboBox.Items.Contains(node.TreeView.Parent.Text + "." + node.FullPath.Replace("\\", ".")))
                        comboBox.Items.Add(node.TreeView.Parent.Text + "." + node.FullPath.Replace("\\", "."));
                    else
                        return;
                }
                else
                {
                    if (!comboBox.Items.Contains(node.TreeView.Parent.Text + "." + node.Parent.FullPath.Replace("\\", ".")))
                        comboBox.Items.Add(node.TreeView.Parent.Text + "." + node.Parent.FullPath.Replace("\\", "."));
                    else
                        return;
                }
            }
        }

        private void 确定button_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void 取消button_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void 拍照位comboBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (拍照位comboBox.Text == "") return;
                //int result = 0;
                //int.TryParse(拍照位comboBox.Text, out result);
                this._viewConfigParam.GrabNo = 拍照位comboBox.Text;
            }
            catch (Exception ex)
            {
               new Common.UserMessageForm(ex.ToString()).ShowDialog();;
            }
        }

        private void WindowConfigForm_Load(object sender, EventArgs e)
        {

        }

        private void Socket配置comboBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                this._viewConfigParam.SocketName = this.Socket配置comboBox.Text;
            }
            catch(Exception ex)
            {
               new Common.UserMessageForm(ex.ToString()).ShowDialog();;
            }
        }



    }
}

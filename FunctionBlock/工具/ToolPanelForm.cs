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
    public partial class ToolPanelForm : Form
    {
        private ToolForm tool;
        public ToolPanelForm()
        {
            InitializeComponent();
            this.tool = new ToolForm();
        }

        private void 图像采集button_MouseClick(object sender, MouseEventArgs e)
        {
            if(e.Button==MouseButtons.Left)
                this.tool.treeView1_NodeMouseDoubleClick(null, new TreeNodeMouseClickEventArgs(new TreeNode("图像采集"), MouseButtons.Left, 1, 0, 0));
        }

        private void 点button_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                this.tool.treeView1_NodeMouseDoubleClick(null, new TreeNodeMouseClickEventArgs(new TreeNode("点"), MouseButtons.Left, 1, 0, 0));
        }

        private void 线button_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                this.tool.treeView1_NodeMouseDoubleClick(null, new TreeNodeMouseClickEventArgs(new TreeNode("线"), MouseButtons.Left, 1, 0, 0));
        }

        private void 圆button_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                this.tool.treeView1_NodeMouseDoubleClick(null, new TreeNodeMouseClickEventArgs(new TreeNode("圆"), MouseButtons.Left, 1, 0, 0));
        }

        private void 圆弧button_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                this.tool.treeView1_NodeMouseDoubleClick(null, new TreeNodeMouseClickEventArgs(new TreeNode("圆弧"), MouseButtons.Left, 1, 0, 0));
        }

        private void 椭圆button_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                this.tool.treeView1_NodeMouseDoubleClick(null, new TreeNodeMouseClickEventArgs(new TreeNode("椭圆"), MouseButtons.Left, 1, 0, 0));
        }

        private void 椭圆弧button_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                this.tool.treeView1_NodeMouseDoubleClick(null, new TreeNodeMouseClickEventArgs(new TreeNode("椭圆弧"), MouseButtons.Left, 1, 0, 0));
        }

        private void 矩形button_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                this.tool.treeView1_NodeMouseDoubleClick(null, new TreeNodeMouseClickEventArgs(new TreeNode("矩形"), MouseButtons.Left, 1, 0, 0));
        }

        private void 宽度button_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                this.tool.treeView1_NodeMouseDoubleClick(null, new TreeNodeMouseClickEventArgs(new TreeNode("宽度"), MouseButtons.Left, 1, 0, 0));
        }

        private void 点线距离2Dbutton_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                this.tool.treeView1_NodeMouseDoubleClick(null, new TreeNodeMouseClickEventArgs(new TreeNode("点线距离"), MouseButtons.Left, 1, 0, 0));
        }

        private void 线线距离2Dbutton_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                this.tool.treeView1_NodeMouseDoubleClick(null, new TreeNodeMouseClickEventArgs(new TreeNode("线线距离"), MouseButtons.Left, 1, 0, 0));
        }

        private void 点点距离2Dbutton_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                this.tool.treeView1_NodeMouseDoubleClick(null, new TreeNodeMouseClickEventArgs(new TreeNode("点点距离"), MouseButtons.Left, 1, 0, 0));
        }

        private void 圆线距离2Dbutton_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                this.tool.treeView1_NodeMouseDoubleClick(null, new TreeNodeMouseClickEventArgs(new TreeNode("圆线距离"), MouseButtons.Left, 1, 0, 0));
        }

        private void 圆圆距离2Dbutton_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                this.tool.treeView1_NodeMouseDoubleClick(null, new TreeNodeMouseClickEventArgs(new TreeNode("圆圆距离"), MouseButtons.Left, 1, 0, 0));
        }

        private void 坐标系形状匹配button_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                this.tool.treeView1_NodeMouseDoubleClick(null, new TreeNodeMouseClickEventArgs(new TreeNode("坐标系-形状匹配"), MouseButtons.Left, 1, 0, 0));
        }

        private void 坐标系圆线button_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                this.tool.treeView1_NodeMouseDoubleClick(null, new TreeNodeMouseClickEventArgs(new TreeNode("坐标系-圆线"), MouseButtons.Left, 1, 0, 0));
        }

        private void 坐标系线button_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                this.tool.treeView1_NodeMouseDoubleClick(null, new TreeNodeMouseClickEventArgs(new TreeNode("坐标系-线"), MouseButtons.Left, 1, 0, 0));
        }

        private void 坐标系线线button_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                this.tool.treeView1_NodeMouseDoubleClick(null, new TreeNodeMouseClickEventArgs(new TreeNode("坐标系-线线"), MouseButtons.Left, 1, 0, 0));
        }

        private void 坐标系点线button_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                this.tool.treeView1_NodeMouseDoubleClick(null, new TreeNodeMouseClickEventArgs(new TreeNode("坐标系-点线"), MouseButtons.Left, 1, 0, 0));
        }

        private void 线线夹角button_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                this.tool.treeView1_NodeMouseDoubleClick(null, new TreeNodeMouseClickEventArgs(new TreeNode("线线夹角"), MouseButtons.Left, 1, 0, 0));
        }

        private void 线线交点button_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                this.tool.treeView1_NodeMouseDoubleClick(null, new TreeNodeMouseClickEventArgs(new TreeNode("线线交点"), MouseButtons.Left, 1, 0, 0));
        }

        private void 圆线交点button_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                this.tool.treeView1_NodeMouseDoubleClick(null, new TreeNodeMouseClickEventArgs(new TreeNode("圆线交点"), MouseButtons.Left, 1, 0, 0));
        }

        private void 圆圆交点button_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                this.tool.treeView1_NodeMouseDoubleClick(null, new TreeNodeMouseClickEventArgs(new TreeNode("圆圆交点"), MouseButtons.Left, 1, 0, 0));
        }

        private void 中分线button_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                this.tool.treeView1_NodeMouseDoubleClick(null, new TreeNodeMouseClickEventArgs(new TreeNode("中分线"), MouseButtons.Left, 1, 0, 0));
        }

        private void 激光点button_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                this.tool.treeView1_NodeMouseDoubleClick(null, new TreeNodeMouseClickEventArgs(new TreeNode("激光点"), MouseButtons.Left, 1, 0, 0));
        }

        private void 激光线button_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                this.tool.treeView1_NodeMouseDoubleClick(null, new TreeNodeMouseClickEventArgs(new TreeNode("激光线"), MouseButtons.Left, 1, 0, 0));
        }

        private void 对射测厚button_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                this.tool.treeView1_NodeMouseDoubleClick(null, new TreeNodeMouseClickEventArgs(new TreeNode("双激光采点测厚"), MouseButtons.Left, 1, 0, 0));
        }

        private void 点到面距离button_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                this.tool.treeView1_NodeMouseDoubleClick(null, new TreeNodeMouseClickEventArgs(new TreeNode("点到面距离"), MouseButtons.Left, 1, 0, 0));
        }

        private void 平面度button_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                this.tool.treeView1_NodeMouseDoubleClick(null, new TreeNodeMouseClickEventArgs(new TreeNode("平面度"), MouseButtons.Left, 1, 0, 0));
        }

        private void 面到面距离button_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                this.tool.treeView1_NodeMouseDoubleClick(null, new TreeNodeMouseClickEventArgs(new TreeNode("面到面距离"), MouseButtons.Left, 1, 0, 0));
        }

        private void 数据输出button_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                this.tool.treeView1_NodeMouseDoubleClick(null, new TreeNodeMouseClickEventArgs(new TreeNode("数据输出"), MouseButtons.Left, 1, 0, 0));
        }



    }
}

namespace FunctionBlock
{
    partial class ToolThickness
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("采集源");
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("激光点");
            System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("激光线");
            System.Windows.Forms.TreeNode treeNode4 = new System.Windows.Forms.TreeNode("平面度");
            System.Windows.Forms.TreeNode treeNode5 = new System.Windows.Forms.TreeNode("厚度");
            System.Windows.Forms.TreeNode treeNode6 = new System.Windows.Forms.TreeNode("体积");
            System.Windows.Forms.TreeNode treeNode7 = new System.Windows.Forms.TreeNode("获取窗口图像");
            System.Windows.Forms.TreeNode treeNode8 = new System.Windows.Forms.TreeNode("双激光采点测厚");
            System.Windows.Forms.TreeNode treeNode9 = new System.Windows.Forms.TreeNode("双激光扫描测厚");
            System.Windows.Forms.TreeNode treeNode10 = new System.Windows.Forms.TreeNode("对射校准");
            System.Windows.Forms.TreeNode treeNode11 = new System.Windows.Forms.TreeNode("数据输出");
            System.Windows.Forms.TreeNode treeNode12 = new System.Windows.Forms.TreeNode("数值计算");
            System.Windows.Forms.TreeNode treeNode13 = new System.Windows.Forms.TreeNode("点位运动");
            System.Windows.Forms.TreeNode treeNode14 = new System.Windows.Forms.TreeNode("矩形取点");
            System.Windows.Forms.TreeNode treeNode15 = new System.Windows.Forms.TreeNode("圆形取点");
            System.Windows.Forms.TreeNode treeNode16 = new System.Windows.Forms.TreeNode("合并3D对象");
            System.Windows.Forms.TreeNode treeNode17 = new System.Windows.Forms.TreeNode("保存3D对象");
            System.Windows.Forms.TreeNode treeNode18 = new System.Windows.Forms.TreeNode("获取窗口图像");
            System.Windows.Forms.TreeNode treeNode19 = new System.Windows.Forms.TreeNode("保存图像");
            System.Windows.Forms.TreeNode treeNode20 = new System.Windows.Forms.TreeNode("平滑3D轮廓对象");
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ToolThickness));
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.SuspendLayout();
            // 
            // treeView1
            // 
            this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView1.Location = new System.Drawing.Point(0, 0);
            this.treeView1.Name = "treeView1";
            treeNode1.Name = "采集源";
            treeNode1.Text = "采集源";
            treeNode2.Name = "激光点";
            treeNode2.Text = "激光点";
            treeNode3.Name = "激光线";
            treeNode3.Text = "激光线";
            treeNode4.Name = "平面度";
            treeNode4.Text = "平面度";
            treeNode5.Name = "厚度";
            treeNode5.Text = "厚度";
            treeNode6.Name = "体积";
            treeNode6.Text = "体积";
            treeNode7.Name = "获取窗口图像";
            treeNode7.Text = "获取窗口图像";
            treeNode8.Name = "双激光采点测厚";
            treeNode8.Text = "双激光采点测厚";
            treeNode9.Name = "双激光扫描测厚";
            treeNode9.Text = "双激光扫描测厚";
            treeNode10.Name = "对射校准";
            treeNode10.Text = "对射校准";
            treeNode11.Name = "数据输出";
            treeNode11.Text = "数据输出";
            treeNode12.Name = "数值计算";
            treeNode12.Text = "数值计算";
            treeNode13.Name = "点位运动";
            treeNode13.Text = "点位运动";
            treeNode14.Name = "矩形取点";
            treeNode14.Text = "矩形取点";
            treeNode15.Name = "圆形取点";
            treeNode15.Text = "圆形取点";
            treeNode16.Name = "合并3D对象";
            treeNode16.Text = "合并3D对象";
            treeNode17.Name = "保存3D对象";
            treeNode17.Text = "保存3D对象";
            treeNode18.Name = "获取窗口图像";
            treeNode18.Text = "获取窗口图像";
            treeNode19.Name = "保存图像";
            treeNode19.Text = "保存图像";
            treeNode20.Name = "平滑3D轮廓对象";
            treeNode20.Text = "平滑3D轮廓对象";
            this.treeView1.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode1,
            treeNode2,
            treeNode3,
            treeNode4,
            treeNode5,
            treeNode6,
            treeNode7,
            treeNode8,
            treeNode9,
            treeNode10,
            treeNode11,
            treeNode12,
            treeNode13,
            treeNode14,
            treeNode15,
            treeNode16,
            treeNode17,
            treeNode18,
            treeNode19,
            treeNode20});
            this.treeView1.Size = new System.Drawing.Size(174, 424);
            this.treeView1.TabIndex = 0;
            this.treeView1.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeView1_NodeMouseDoubleClick);
            // 
            // ToolThickness
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(174, 424);
            this.Controls.Add(this.treeView1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ToolThickness";
            this.Text = "工具栏";
            this.Load += new System.EventHandler(this.Tool_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView treeView1;
    }
}
namespace FunctionBlock
{
    partial class ProgramForm
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.程序tabControl = new System.Windows.Forms.TabControl();
            this.程序toolStrip = new System.Windows.Forms.ToolStrip();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton3 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton4 = new System.Windows.Forms.ToolStripButton();
            this.拖动label = new System.Windows.Forms.Label();
            this.tableLayoutPanel1.SuspendLayout();
            this.程序toolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.Controls.Add(this.程序tabControl, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.程序toolStrip, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.拖动label, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 44F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(288, 480);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // 程序tabControl
            // 
            this.程序tabControl.AllowDrop = true;
            this.tableLayoutPanel1.SetColumnSpan(this.程序tabControl, 2);
            this.程序tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.程序tabControl.Location = new System.Drawing.Point(0, 44);
            this.程序tabControl.Margin = new System.Windows.Forms.Padding(0);
            this.程序tabControl.Multiline = true;
            this.程序tabControl.Name = "程序tabControl";
            this.程序tabControl.Padding = new System.Drawing.Point(3, 3);
            this.程序tabControl.SelectedIndex = 0;
            this.程序tabControl.Size = new System.Drawing.Size(288, 436);
            this.程序tabControl.TabIndex = 1;
            this.程序tabControl.SelectedIndexChanged += new System.EventHandler(this.程序tabControl_SelectedIndexChanged);
            // 
            // 程序toolStrip
            // 
            this.程序toolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.程序toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton1,
            this.toolStripButton2,
            this.toolStripButton3,
            this.toolStripButton4});
            this.程序toolStrip.Location = new System.Drawing.Point(0, 0);
            this.程序toolStrip.Name = "程序toolStrip";
            this.程序toolStrip.Size = new System.Drawing.Size(248, 40);
            this.程序toolStrip.TabIndex = 2;
            this.程序toolStrip.Text = "toolStrip1";
            this.程序toolStrip.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.程序toolStrip_ItemClicked);
            this.程序toolStrip.MouseDown += new System.Windows.Forms.MouseEventHandler(this.程序toolStrip_MouseDown);
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.Image = global::FunctionBlock.Properties.Resources._1606739385_1_;
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(52, 37);
            this.toolStripButton1.Text = "添加(A)";
            this.toolStripButton1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.Image = global::FunctionBlock.Properties.Resources._1606739419_1_;
            this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Size = new System.Drawing.Size(53, 37);
            this.toolStripButton2.Text = "删除(D)";
            this.toolStripButton2.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            // 
            // toolStripButton3
            // 
            this.toolStripButton3.Image = global::FunctionBlock.Properties.Resources._1606742372_1_;
            this.toolStripButton3.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton3.Name = "toolStripButton3";
            this.toolStripButton3.Size = new System.Drawing.Size(51, 37);
            this.toolStripButton3.Text = "编辑(E)";
            this.toolStripButton3.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            // 
            // toolStripButton4
            // 
            this.toolStripButton4.Image = global::FunctionBlock.Properties.Resources._1606742307_1_;
            this.toolStripButton4.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton4.Name = "toolStripButton4";
            this.toolStripButton4.Size = new System.Drawing.Size(51, 37);
            this.toolStripButton4.Text = "工具(T)";
            this.toolStripButton4.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            // 
            // 拖动label
            // 
            this.拖动label.AutoSize = true;
            this.拖动label.Dock = System.Windows.Forms.DockStyle.Fill;
            this.拖动label.Location = new System.Drawing.Point(251, 0);
            this.拖动label.Name = "拖动label";
            this.拖动label.Size = new System.Drawing.Size(34, 44);
            this.拖动label.TabIndex = 3;
            this.拖动label.Text = "拖动";
            this.拖动label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.拖动label.MouseDown += new System.Windows.Forms.MouseEventHandler(this.拖动label_MouseDown);
            this.拖动label.MouseEnter += new System.EventHandler(this.拖动label_MouseEnter);
            this.拖动label.MouseLeave += new System.EventHandler(this.拖动label_MouseLeave);
            // 
            // ProgramForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(288, 480);
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "ProgramForm";
            this.Text = "ProgramForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ProgramForm_FormClosing);
            this.Load += new System.EventHandler(this.ProgramForm_Load);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ProgramForm_MouseDown);
            this.Move += new System.EventHandler(this.ProgramForm_Move);
            this.Resize += new System.EventHandler(this.ProgramForm_Resize);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.程序toolStrip.ResumeLayout(false);
            this.程序toolStrip.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TabControl 程序tabControl;
        private System.Windows.Forms.ToolStrip 程序toolStrip;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.ToolStripButton toolStripButton2;
        private System.Windows.Forms.ToolStripButton toolStripButton3;
        private System.Windows.Forms.ToolStripButton toolStripButton4;
        private System.Windows.Forms.Label 拖动label;
    }
}
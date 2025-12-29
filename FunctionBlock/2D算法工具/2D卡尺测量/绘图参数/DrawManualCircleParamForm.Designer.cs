
namespace FunctionBlock
{
    partial class DrawManualCircleParamForm
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.终止角textBox = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.起始角textBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.圆半径texBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.确认button = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.圆心YtextBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.圆心XtextBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.圆心YtextBox);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.圆心XtextBox);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.终止角textBox);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.起始角textBox);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.圆半径texBox);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.确认button);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.SetRowSpan(this.groupBox1, 2);
            this.groupBox1.Size = new System.Drawing.Size(264, 200);
            this.groupBox1.TabIndex = 62;
            this.groupBox1.TabStop = false;
            // 
            // 终止角textBox
            // 
            this.终止角textBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.终止角textBox.Location = new System.Drawing.Point(64, 117);
            this.终止角textBox.Name = "终止角textBox";
            this.终止角textBox.Size = new System.Drawing.Size(194, 22);
            this.终止角textBox.TabIndex = 87;
            this.终止角textBox.TextChanged += new System.EventHandler(this.终止角textBox_TextChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(12, 122);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(44, 12);
            this.label9.TabIndex = 86;
            this.label9.Text = "终止角:";
            // 
            // 起始角textBox
            // 
            this.起始角textBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.起始角textBox.Location = new System.Drawing.Point(64, 89);
            this.起始角textBox.Name = "起始角textBox";
            this.起始角textBox.Size = new System.Drawing.Size(194, 22);
            this.起始角textBox.TabIndex = 79;
            this.起始角textBox.TextChanged += new System.EventHandler(this.起始角textBox_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 93);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(44, 12);
            this.label2.TabIndex = 78;
            this.label2.Text = "起始角:";
            // 
            // 圆半径texBox
            // 
            this.圆半径texBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.圆半径texBox.Location = new System.Drawing.Point(64, 63);
            this.圆半径texBox.Name = "圆半径texBox";
            this.圆半径texBox.Size = new System.Drawing.Size(194, 22);
            this.圆半径texBox.TabIndex = 77;
            this.圆半径texBox.TextChanged += new System.EventHandler(this.圆半径texBox_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 67);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 12);
            this.label1.TabIndex = 76;
            this.label1.Text = "圆半径:";
            // 
            // 确认button
            // 
            this.确认button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.确认button.Location = new System.Drawing.Point(183, 162);
            this.确认button.Name = "确认button";
            this.确认button.Size = new System.Drawing.Size(78, 32);
            this.确认button.TabIndex = 62;
            this.确认button.Text = "确认";
            this.确认button.UseVisualStyleBackColor = true;
            this.确认button.Click += new System.EventHandler(this.确认button_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 264F));
            this.tableLayoutPanel1.Controls.Add(this.groupBox1, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(264, 200);
            this.tableLayoutPanel1.TabIndex = 65;
            // 
            // 圆心YtextBox
            // 
            this.圆心YtextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.圆心YtextBox.Location = new System.Drawing.Point(64, 34);
            this.圆心YtextBox.Name = "圆心YtextBox";
            this.圆心YtextBox.Size = new System.Drawing.Size(194, 22);
            this.圆心YtextBox.TabIndex = 91;
            this.圆心YtextBox.Text = "0";
            this.圆心YtextBox.TextChanged += new System.EventHandler(this.圆心YtextBox_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 38);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(40, 12);
            this.label3.TabIndex = 90;
            this.label3.Text = "圆心Y:";
            // 
            // 圆心XtextBox
            // 
            this.圆心XtextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.圆心XtextBox.Location = new System.Drawing.Point(64, 8);
            this.圆心XtextBox.Name = "圆心XtextBox";
            this.圆心XtextBox.Size = new System.Drawing.Size(194, 22);
            this.圆心XtextBox.TabIndex = 89;
            this.圆心XtextBox.Text = "0";
            this.圆心XtextBox.TextChanged += new System.EventHandler(this.圆心XtextBox_TextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 12);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(40, 12);
            this.label4.TabIndex = 88;
            this.label4.Text = "圆心X:";
            // 
            // DrawManualCircleParamForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(264, 200);
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "DrawManualCircleParamForm";
            this.ShowIcon = false;
            this.Text = "圆弧参数";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DrawParamForm_FormClosing);
            this.Load += new System.EventHandler(this.DrawCircleParamForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button 确认button;
        private System.Windows.Forms.TextBox 终止角textBox;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox 起始角textBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox 圆半径texBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox 圆心YtextBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox 圆心XtextBox;
        private System.Windows.Forms.Label label4;
    }
}
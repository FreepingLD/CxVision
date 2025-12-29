namespace FunctionBlock
{
    partial class SortRegionForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.排序模式ComboBox = new System.Windows.Forms.ComboBox();
            this.排序顺序comboBox = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.排序对象comboBox = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.spilitLine1 = new userControl.SpilitLine();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "排序模式";
            // 
            // 排序模式ComboBox
            // 
            this.排序模式ComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.排序模式ComboBox.FormattingEnabled = true;
            this.排序模式ComboBox.Location = new System.Drawing.Point(63, 29);
            this.排序模式ComboBox.Name = "排序模式ComboBox";
            this.排序模式ComboBox.Size = new System.Drawing.Size(320, 20);
            this.排序模式ComboBox.TabIndex = 5;
            this.排序模式ComboBox.SelectedIndexChanged += new System.EventHandler(this.排序模式ComboBox_SelectedIndexChanged);
            // 
            // 排序顺序comboBox
            // 
            this.排序顺序comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.排序顺序comboBox.FormattingEnabled = true;
            this.排序顺序comboBox.Items.AddRange(new object[] {
            "false",
            "true"});
            this.排序顺序comboBox.Location = new System.Drawing.Point(63, 55);
            this.排序顺序comboBox.Name = "排序顺序comboBox";
            this.排序顺序comboBox.Size = new System.Drawing.Size(320, 20);
            this.排序顺序comboBox.TabIndex = 7;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(28, 58);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 6;
            this.label2.Text = "顺序";
            // 
            // 排序对象comboBox
            // 
            this.排序对象comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.排序对象comboBox.FormattingEnabled = true;
            this.排序对象comboBox.Items.AddRange(new object[] {
            "column",
            "row"});
            this.排序对象comboBox.Location = new System.Drawing.Point(63, 81);
            this.排序对象comboBox.Name = "排序对象comboBox";
            this.排序对象comboBox.Size = new System.Drawing.Size(320, 20);
            this.排序对象comboBox.TabIndex = 9;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(4, 84);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 8;
            this.label3.Text = "排序对象";
            // 
            // spilitLine1
            // 
            this.spilitLine1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.spilitLine1.Content = "参数";
            this.spilitLine1.Location = new System.Drawing.Point(6, 1);
            this.spilitLine1.Name = "spilitLine1";
            this.spilitLine1.Size = new System.Drawing.Size(377, 20);
            this.spilitLine1.TabIndex = 10;
            // 
            // SortRegionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(383, 174);
            this.Controls.Add(this.spilitLine1);
            this.Controls.Add(this.排序对象comboBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.排序顺序comboBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.排序模式ComboBox);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "SortRegionForm";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox 排序模式ComboBox;
        private System.Windows.Forms.ComboBox 排序顺序comboBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox 排序对象comboBox;
        private System.Windows.Forms.Label label3;
        private userControl.SpilitLine spilitLine1;
    }
}
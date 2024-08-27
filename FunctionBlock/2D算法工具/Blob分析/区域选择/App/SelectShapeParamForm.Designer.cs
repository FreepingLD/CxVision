namespace FunctionBlock
{
    partial class SelectShapeParamForm
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
            this.最大值comboBox = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.最小值comboBox = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.操作comboBox = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.特征ComboBox = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // 最大值comboBox
            // 
            this.最大值comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.最大值comboBox.FormattingEnabled = true;
            this.最大值comboBox.Items.AddRange(new object[] {
            "0.0",
            "0.5",
            "1.0",
            "2.0",
            "3.0",
            "4.0",
            "5.0"});
            this.最大值comboBox.Location = new System.Drawing.Point(59, 90);
            this.最大值comboBox.Name = "最大值comboBox";
            this.最大值comboBox.Size = new System.Drawing.Size(266, 20);
            this.最大值comboBox.TabIndex = 22;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 93);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 12);
            this.label4.TabIndex = 21;
            this.label4.Text = "最大值";
            // 
            // 最小值comboBox
            // 
            this.最小值comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.最小值comboBox.FormattingEnabled = true;
            this.最小值comboBox.Items.AddRange(new object[] {
            "0.0",
            "0.5",
            "1.0",
            "2.0",
            "3.0",
            "4.0",
            "5.0"});
            this.最小值comboBox.Location = new System.Drawing.Point(59, 64);
            this.最小值comboBox.Name = "最小值comboBox";
            this.最小值comboBox.Size = new System.Drawing.Size(266, 20);
            this.最小值comboBox.TabIndex = 20;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 67);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 12);
            this.label3.TabIndex = 19;
            this.label3.Text = "最小值";
            // 
            // 操作comboBox
            // 
            this.操作comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.操作comboBox.FormattingEnabled = true;
            this.操作comboBox.Items.AddRange(new object[] {
            "and",
            "or"});
            this.操作comboBox.Location = new System.Drawing.Point(59, 38);
            this.操作comboBox.Name = "操作comboBox";
            this.操作comboBox.Size = new System.Drawing.Size(266, 20);
            this.操作comboBox.TabIndex = 18;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(24, 42);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 17;
            this.label2.Text = "操作";
            // 
            // 特征ComboBox
            // 
            this.特征ComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.特征ComboBox.FormattingEnabled = true;
            this.特征ComboBox.Items.AddRange(new object[] {
            "area",
            "row",
            "column",
            "width",
            "height",
            "ratio",
            "row1",
            "column1",
            "row2",
            "column2",
            "circularity",
            "compactness",
            "contlength",
            "convexity",
            "rectangularity",
            "ra,",
            "rb,",
            "phi,",
            "anisometry,",
            "bulkiness,",
            "struct_factor,",
            "outer_radius,",
            "inner_radius,",
            "inner_width,",
            "inner_height,",
            "dist_mean,",
            "dist_deviation",
            "roundness",
            "num_sides",
            "connect_num",
            "holes_num",
            "area_holes",
            "max_diameter",
            "orientation",
            "euler_number",
            "rect2_phi",
            "rect2_len1",
            "rect2_len2",
            "moments_m11",
            "moments_m20",
            "moments_m02",
            "moments_ia",
            "moments_ib",
            "moments_m11_invar",
            "moments_m20_invar",
            "moments_m02_invar",
            "moments_phi1",
            "moments_phi2",
            "moments_m21",
            "moments_m12",
            "moments_m03",
            "moments_m30",
            "moments_m21_invar",
            "moments_m12_invar",
            "moments_m03_invar",
            "moments_m30_invar",
            "moments_i1",
            "moments_i2",
            "moments_i3",
            "moments_i4",
            "moments_psi1",
            "moments_psi2",
            "moments_psi3",
            "moments_psi4"});
            this.特征ComboBox.Location = new System.Drawing.Point(59, 12);
            this.特征ComboBox.Name = "特征ComboBox";
            this.特征ComboBox.Size = new System.Drawing.Size(266, 20);
            this.特征ComboBox.TabIndex = 15;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(24, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 14;
            this.label1.Text = "特征";
            // 
            // SelectShapeParamForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(337, 126);
            this.Controls.Add(this.最大值comboBox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.最小值comboBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.操作comboBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.特征ComboBox);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "SelectShapeParamForm";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox 最大值comboBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox 最小值comboBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox 操作comboBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox 特征ComboBox;
        private System.Windows.Forms.Label label1;
    }
}
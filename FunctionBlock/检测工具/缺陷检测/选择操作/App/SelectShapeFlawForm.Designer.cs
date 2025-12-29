namespace FunctionBlock
{
    partial class SelectShapeFlawForm
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
            this.特征ComboBox = new System.Windows.Forms.ComboBox();
            this.启用算法checkBox = new System.Windows.Forms.CheckBox();
            this.极性comboBox = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.操作comboBox = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.最小值comboBox = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.最大值comboBox = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "特征";
            // 
            // 特征ComboBox
            // 
            this.特征ComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.特征ComboBox.FormattingEnabled = true;
            this.特征ComboBox.Items.AddRange(new object[] {
            "anisometry",
            "area",
            "area_holes",
            "bulkiness",
            "circularity",
            "column",
            "column1",
            "column2",
            "compactness",
            "connect_num",
            "contlength",
            "convexity",
            "dist_deviation",
            "dist_mean",
            "euler_number",
            "height",
            "holes_num",
            "inner_height",
            "inner_radius",
            "inner_width",
            "max_diameter",
            "moments_i1",
            "moments_i2",
            "moments_i3",
            "moments_i4",
            "moments_ia",
            "moments_ib",
            "moments_m02",
            "moments_m02_invar",
            "moments_m03",
            "moments_m03_invar",
            "moments_m11",
            "moments_m11_invar",
            "moments_m12",
            "moments_m12_invar",
            "moments_m20",
            "moments_m20_invar",
            "moments_m21",
            "moments_m21_invar",
            "moments_m30",
            "moments_m30_invar",
            "moments_phi1",
            "moments_phi2",
            "moments_psi1",
            "moments_psi2",
            "moments_psi3",
            "moments_psi4",
            "num_sides",
            "orientation",
            "outer_radius",
            "phi",
            "ra",
            "ratio",
            "rb",
            "rect2_len1",
            "rect2_len2",
            "rect2_phi",
            "rectangularity",
            "roundness",
            "row",
            "row1",
            "row2",
            "struct_factor",
            "width"});
            this.特征ComboBox.Location = new System.Drawing.Point(51, 6);
            this.特征ComboBox.Name = "特征ComboBox";
            this.特征ComboBox.Size = new System.Drawing.Size(165, 20);
            this.特征ComboBox.TabIndex = 5;
            // 
            // 启用算法checkBox
            // 
            this.启用算法checkBox.AutoSize = true;
            this.启用算法checkBox.Location = new System.Drawing.Point(51, 138);
            this.启用算法checkBox.Name = "启用算法checkBox";
            this.启用算法checkBox.Size = new System.Drawing.Size(72, 16);
            this.启用算法checkBox.TabIndex = 14;
            this.启用算法checkBox.Text = "启用算法";
            this.启用算法checkBox.UseVisualStyleBackColor = true;
            // 
            // 极性comboBox
            // 
            this.极性comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.极性comboBox.FormattingEnabled = true;
            this.极性comboBox.Items.AddRange(new object[] {
            "light",
            "dark",
            "all"});
            this.极性comboBox.Location = new System.Drawing.Point(51, 110);
            this.极性comboBox.Name = "极性comboBox";
            this.极性comboBox.Size = new System.Drawing.Size(165, 20);
            this.极性comboBox.TabIndex = 16;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(18, 114);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 15;
            this.label2.Text = "极性";
            // 
            // 操作comboBox
            // 
            this.操作comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.操作comboBox.FormattingEnabled = true;
            this.操作comboBox.Items.AddRange(new object[] {
            "and",
            "or"});
            this.操作comboBox.Location = new System.Drawing.Point(51, 32);
            this.操作comboBox.Name = "操作comboBox";
            this.操作comboBox.Size = new System.Drawing.Size(165, 20);
            this.操作comboBox.TabIndex = 18;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(18, 36);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 12);
            this.label3.TabIndex = 17;
            this.label3.Text = "操作";
            // 
            // 最小值comboBox
            // 
            this.最小值comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.最小值comboBox.FormattingEnabled = true;
            this.最小值comboBox.Items.AddRange(new object[] {
            "100",
            "200",
            "500"});
            this.最小值comboBox.Location = new System.Drawing.Point(51, 58);
            this.最小值comboBox.Name = "最小值comboBox";
            this.最小值comboBox.Size = new System.Drawing.Size(165, 20);
            this.最小值comboBox.TabIndex = 20;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 61);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 12);
            this.label4.TabIndex = 19;
            this.label4.Text = "最小值";
            // 
            // 最大值comboBox
            // 
            this.最大值comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.最大值comboBox.FormattingEnabled = true;
            this.最大值comboBox.Items.AddRange(new object[] {
            "100",
            "200",
            "500"});
            this.最大值comboBox.Location = new System.Drawing.Point(51, 84);
            this.最大值comboBox.Name = "最大值comboBox";
            this.最大值comboBox.Size = new System.Drawing.Size(165, 20);
            this.最大值comboBox.TabIndex = 22;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 87);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(41, 12);
            this.label5.TabIndex = 21;
            this.label5.Text = "最大值";
            // 
            // SelectShapeFlawForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(217, 164);
            this.Controls.Add(this.最大值comboBox);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.最小值comboBox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.操作comboBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.极性comboBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.启用算法checkBox);
            this.Controls.Add(this.特征ComboBox);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "SelectShapeFlawForm";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox 特征ComboBox;
        private System.Windows.Forms.CheckBox 启用算法checkBox;
        private System.Windows.Forms.ComboBox 极性comboBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox 操作comboBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox 最小值comboBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox 最大值comboBox;
        private System.Windows.Forms.Label label5;
    }
}
namespace FunctionBlock
{
    partial class IntensityParamForm
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
            this.平均灰度ComboBox = new System.Windows.Forms.ComboBox();
            this.标准差comboBox = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.操作comboBox = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.特征ComboBox = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 67);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "平均灰度：";
            // 
            // 平均灰度ComboBox
            // 
            this.平均灰度ComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.平均灰度ComboBox.FormattingEnabled = true;
            this.平均灰度ComboBox.Items.AddRange(new object[] {
            "max_area",
            "rectangle1",
            "rectangle2"});
            this.平均灰度ComboBox.Location = new System.Drawing.Point(74, 64);
            this.平均灰度ComboBox.Name = "平均灰度ComboBox";
            this.平均灰度ComboBox.Size = new System.Drawing.Size(228, 20);
            this.平均灰度ComboBox.TabIndex = 5;
            // 
            // 标准差comboBox
            // 
            this.标准差comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.标准差comboBox.FormattingEnabled = true;
            this.标准差comboBox.Items.AddRange(new object[] {
            "70",
            "90",
            "95",
            "100"});
            this.标准差comboBox.Location = new System.Drawing.Point(74, 90);
            this.标准差comboBox.Name = "标准差comboBox";
            this.标准差comboBox.Size = new System.Drawing.Size(228, 20);
            this.标准差comboBox.TabIndex = 9;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(22, 93);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 8;
            this.label2.Text = "标准差：";
            // 
            // 操作comboBox
            // 
            this.操作comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.操作comboBox.FormattingEnabled = true;
            this.操作comboBox.Items.AddRange(new object[] {
            "and",
            "or"});
            this.操作comboBox.Location = new System.Drawing.Point(74, 38);
            this.操作comboBox.Name = "操作comboBox";
            this.操作comboBox.Size = new System.Drawing.Size(228, 20);
            this.操作comboBox.TabIndex = 22;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(34, 42);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 12);
            this.label3.TabIndex = 21;
            this.label3.Text = "操作：";
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
            this.特征ComboBox.Location = new System.Drawing.Point(74, 12);
            this.特征ComboBox.Name = "特征ComboBox";
            this.特征ComboBox.Size = new System.Drawing.Size(228, 20);
            this.特征ComboBox.TabIndex = 20;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(34, 15);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 12);
            this.label4.TabIndex = 19;
            this.label4.Text = "特征：";
            // 
            // IntensityParamForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(303, 132);
            this.Controls.Add(this.操作comboBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.特征ComboBox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.标准差comboBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.平均灰度ComboBox);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "IntensityParamForm";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.IntensityParamForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox 平均灰度ComboBox;
        private System.Windows.Forms.ComboBox 标准差comboBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox 操作comboBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox 特征ComboBox;
        private System.Windows.Forms.Label label4;
    }
}
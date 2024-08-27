namespace FunctionBlock
{
    partial class CShapeModelForm
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
            this.角度步长comboBox = new System.Windows.Forms.ComboBox();
            this.金字塔层级comboBox = new System.Windows.Forms.ComboBox();
            this.最小对比度comboBox = new System.Windows.Forms.ComboBox();
            this.对比度comboBox = new System.Windows.Forms.ComboBox();
            this.极性comboBox = new System.Windows.Forms.ComboBox();
            this.优化comboBox = new System.Windows.Forms.ComboBox();
            this.label23 = new System.Windows.Forms.Label();
            this.label24 = new System.Windows.Forms.Label();
            this.label21 = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.终止角度textBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.起始角度textBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.最小轮廓长度comboBox = new System.Windows.Forms.ComboBox();
            this.label14 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // 角度步长comboBox
            // 
            this.角度步长comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.角度步长comboBox.FormattingEnabled = true;
            this.角度步长comboBox.Items.AddRange(new object[] {
            "auto",
            "0.0175",
            "0.0349",
            "0.0524",
            "0.0698",
            "0.0873"});
            this.角度步长comboBox.Location = new System.Drawing.Point(92, 83);
            this.角度步长comboBox.Name = "角度步长comboBox";
            this.角度步长comboBox.Size = new System.Drawing.Size(121, 20);
            this.角度步长comboBox.TabIndex = 90;
            // 
            // 金字塔层级comboBox
            // 
            this.金字塔层级comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.金字塔层级comboBox.FormattingEnabled = true;
            this.金字塔层级comboBox.Items.AddRange(new object[] {
            "0",
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10",
            "auto"});
            this.金字塔层级comboBox.Location = new System.Drawing.Point(92, 5);
            this.金字塔层级comboBox.Name = "金字塔层级comboBox";
            this.金字塔层级comboBox.Size = new System.Drawing.Size(121, 20);
            this.金字塔层级comboBox.TabIndex = 89;
            // 
            // 最小对比度comboBox
            // 
            this.最小对比度comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.最小对比度comboBox.FormattingEnabled = true;
            this.最小对比度comboBox.Items.AddRange(new object[] {
            "auto",
            "1",
            "2",
            "3",
            "5",
            "7",
            "10",
            "20",
            "30",
            "40"});
            this.最小对比度comboBox.Location = new System.Drawing.Point(92, 213);
            this.最小对比度comboBox.Name = "最小对比度comboBox";
            this.最小对比度comboBox.Size = new System.Drawing.Size(121, 20);
            this.最小对比度comboBox.TabIndex = 88;
            this.最小对比度comboBox.SelectedIndexChanged += new System.EventHandler(this.最小对比度comboBox_SelectedIndexChanged);
            // 
            // 对比度comboBox
            // 
            this.对比度comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.对比度comboBox.FormattingEnabled = true;
            this.对比度comboBox.Items.AddRange(new object[] {
            "auto",
            "auto_contrast",
            "auto_contrast_hyst",
            "auto_min_size",
            "10",
            "20",
            "30",
            "40",
            "60",
            "80",
            "100",
            "120",
            "140",
            "160"});
            this.对比度comboBox.Location = new System.Drawing.Point(92, 161);
            this.对比度comboBox.Name = "对比度comboBox";
            this.对比度comboBox.Size = new System.Drawing.Size(121, 20);
            this.对比度comboBox.TabIndex = 87;
            // 
            // 极性comboBox
            // 
            this.极性comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.极性comboBox.FormattingEnabled = true;
            this.极性comboBox.Items.AddRange(new object[] {
            "ignore_color_polarity",
            "ignore_global_polarity",
            "ignore_local_polarity",
            "use_polarity"});
            this.极性comboBox.Location = new System.Drawing.Point(92, 135);
            this.极性comboBox.Name = "极性comboBox";
            this.极性comboBox.Size = new System.Drawing.Size(121, 20);
            this.极性comboBox.TabIndex = 86;
            // 
            // 优化comboBox
            // 
            this.优化comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.优化comboBox.FormattingEnabled = true;
            this.优化comboBox.Items.AddRange(new object[] {
            "auto",
            "no_pregeneration",
            "none",
            "point_reduction_high",
            "point_reduction_low",
            "point_reduction_medium",
            "pregeneration"});
            this.优化comboBox.Location = new System.Drawing.Point(92, 109);
            this.优化comboBox.Name = "优化comboBox";
            this.优化comboBox.Size = new System.Drawing.Size(121, 20);
            this.优化comboBox.TabIndex = 85;
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Location = new System.Drawing.Point(15, 216);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(71, 12);
            this.label23.TabIndex = 84;
            this.label23.Text = "最小对比度:";
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Location = new System.Drawing.Point(39, 165);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(47, 12);
            this.label24.TabIndex = 83;
            this.label24.Text = "对比度:";
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(51, 139);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(35, 12);
            this.label21.TabIndex = 82;
            this.label21.Text = "极性:";
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Location = new System.Drawing.Point(51, 113);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(35, 12);
            this.label22.TabIndex = 81;
            this.label22.Text = "优化:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(27, 87);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 12);
            this.label3.TabIndex = 68;
            this.label3.Text = "角度步长:";
            // 
            // 终止角度textBox
            // 
            this.终止角度textBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.终止角度textBox.Location = new System.Drawing.Point(92, 56);
            this.终止角度textBox.Name = "终止角度textBox";
            this.终止角度textBox.Size = new System.Drawing.Size(121, 21);
            this.终止角度textBox.TabIndex = 67;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(27, 61);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(59, 12);
            this.label4.TabIndex = 66;
            this.label4.Text = "终止角度:";
            // 
            // 起始角度textBox
            // 
            this.起始角度textBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.起始角度textBox.Location = new System.Drawing.Point(92, 30);
            this.起始角度textBox.Name = "起始角度textBox";
            this.起始角度textBox.Size = new System.Drawing.Size(121, 21);
            this.起始角度textBox.TabIndex = 65;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(27, 35);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 12);
            this.label2.TabIndex = 64;
            this.label2.Text = "起始角度:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(83, 12);
            this.label1.TabIndex = 63;
            this.label1.Text = "金字塔层级数:";
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(218, 10);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(41, 12);
            this.label5.TabIndex = 91;
            this.label5.Text = "object";
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(218, 36);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(65, 12);
            this.label6.TabIndex = 92;
            this.label6.Text = "double(度)";
            // 
            // label7
            // 
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(218, 62);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(65, 12);
            this.label7.TabIndex = 93;
            this.label7.Text = "double(度)";
            // 
            // label8
            // 
            this.label8.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(218, 89);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(41, 12);
            this.label8.TabIndex = 94;
            this.label8.Text = "object";
            // 
            // label9
            // 
            this.label9.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(218, 116);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(41, 12);
            this.label9.TabIndex = 95;
            this.label9.Text = "string";
            // 
            // label10
            // 
            this.label10.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(218, 141);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(41, 12);
            this.label10.TabIndex = 96;
            this.label10.Text = "string";
            // 
            // label11
            // 
            this.label11.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(218, 167);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(41, 12);
            this.label11.TabIndex = 97;
            this.label11.Text = "string";
            // 
            // label12
            // 
            this.label12.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(218, 218);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(41, 12);
            this.label12.TabIndex = 98;
            this.label12.Text = "string";
            // 
            // label13
            // 
            this.label13.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(218, 193);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(41, 12);
            this.label13.TabIndex = 101;
            this.label13.Text = "string";
            // 
            // 最小轮廓长度comboBox
            // 
            this.最小轮廓长度comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.最小轮廓长度comboBox.FormattingEnabled = true;
            this.最小轮廓长度comboBox.Items.AddRange(new object[] {
            "auto",
            "auto_contrast",
            "auto_contrast_hyst",
            "auto_min_size",
            "10",
            "20",
            "30",
            "40",
            "60",
            "80",
            "100",
            "120",
            "140",
            "160"});
            this.最小轮廓长度comboBox.Location = new System.Drawing.Point(92, 187);
            this.最小轮廓长度comboBox.Name = "最小轮廓长度comboBox";
            this.最小轮廓长度comboBox.Size = new System.Drawing.Size(121, 20);
            this.最小轮廓长度comboBox.TabIndex = 100;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(4, 191);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(83, 12);
            this.label14.TabIndex = 99;
            this.label14.Text = "最小轮廓长度:";
            // 
            // CShapeModelForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(280, 298);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.最小轮廓长度comboBox);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.角度步长comboBox);
            this.Controls.Add(this.金字塔层级comboBox);
            this.Controls.Add(this.最小对比度comboBox);
            this.Controls.Add(this.对比度comboBox);
            this.Controls.Add(this.极性comboBox);
            this.Controls.Add(this.优化comboBox);
            this.Controls.Add(this.label23);
            this.Controls.Add(this.label24);
            this.Controls.Add(this.label21);
            this.Controls.Add(this.label22);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.终止角度textBox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.起始角度textBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "CShapeModelForm";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.CShapeModelForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox 角度步长comboBox;
        private System.Windows.Forms.ComboBox 金字塔层级comboBox;
        private System.Windows.Forms.ComboBox 最小对比度comboBox;
        private System.Windows.Forms.ComboBox 对比度comboBox;
        private System.Windows.Forms.ComboBox 极性comboBox;
        private System.Windows.Forms.ComboBox 优化comboBox;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox 终止角度textBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox 起始角度textBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.ComboBox 最小轮廓长度comboBox;
        private System.Windows.Forms.Label label14;
    }
}
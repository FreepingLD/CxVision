namespace FunctionBlock
{
    partial class RectangleArrayDataForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RectangleArrayDataForm));
            this.Y阵列数量numericUpDown = new System.Windows.Forms.NumericUpDown();
            this.X阵列数量numericUpDown = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.Y偏移textBox = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.X偏移textBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.Y阵列数量numericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.X阵列数量numericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // Y阵列数量numericUpDown
            // 
            this.Y阵列数量numericUpDown.Location = new System.Drawing.Point(186, 35);
            this.Y阵列数量numericUpDown.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.Y阵列数量numericUpDown.Name = "Y阵列数量numericUpDown";
            this.Y阵列数量numericUpDown.Size = new System.Drawing.Size(81, 21);
            this.Y阵列数量numericUpDown.TabIndex = 50;
            this.Y阵列数量numericUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.Y阵列数量numericUpDown.ValueChanged += new System.EventHandler(this.Y阵列数量numericUpDown_ValueChanged);
            // 
            // X阵列数量numericUpDown
            // 
            this.X阵列数量numericUpDown.Location = new System.Drawing.Point(186, 10);
            this.X阵列数量numericUpDown.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.X阵列数量numericUpDown.Name = "X阵列数量numericUpDown";
            this.X阵列数量numericUpDown.Size = new System.Drawing.Size(81, 21);
            this.X阵列数量numericUpDown.TabIndex = 49;
            this.X阵列数量numericUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.X阵列数量numericUpDown.ValueChanged += new System.EventHandler(this.X阵列数量numericUpDown_ValueChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(143, 39);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(41, 12);
            this.label5.TabIndex = 48;
            this.label5.Text = "行数量";
            // 
            // Y偏移textBox
            // 
            this.Y偏移textBox.Location = new System.Drawing.Point(56, 33);
            this.Y偏移textBox.Name = "Y偏移textBox";
            this.Y偏移textBox.Size = new System.Drawing.Size(81, 21);
            this.Y偏移textBox.TabIndex = 47;
            this.Y偏移textBox.Text = "0";
            this.Y偏移textBox.TextChanged += new System.EventHandler(this.Y偏移textBox_TextChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 37);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(35, 12);
            this.label6.TabIndex = 46;
            this.label6.Text = "Y偏移";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(143, 12);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 12);
            this.label4.TabIndex = 45;
            this.label4.Text = "列数量";
            // 
            // X偏移textBox
            // 
            this.X偏移textBox.Location = new System.Drawing.Point(56, 9);
            this.X偏移textBox.Name = "X偏移textBox";
            this.X偏移textBox.Size = new System.Drawing.Size(81, 21);
            this.X偏移textBox.TabIndex = 44;
            this.X偏移textBox.Text = "0";
            this.X偏移textBox.TextChanged += new System.EventHandler(this.X偏移textBox_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 12);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 12);
            this.label3.TabIndex = 43;
            this.label3.Text = "X偏移";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(186, 71);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(81, 23);
            this.button1.TabIndex = 51;
            this.button1.Text = "阵列(Array)";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // RectangleArrayDataForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(274, 106);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.Y阵列数量numericUpDown);
            this.Controls.Add(this.X阵列数量numericUpDown);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.Y偏移textBox);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.X偏移textBox);
            this.Controls.Add(this.label3);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "RectangleArrayDataForm";
            this.Text = "阵列数据";
            ((System.ComponentModel.ISupportInitialize)(this.Y阵列数量numericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.X阵列数量numericUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NumericUpDown Y阵列数量numericUpDown;
        private System.Windows.Forms.NumericUpDown X阵列数量numericUpDown;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox Y偏移textBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox X偏移textBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button1;
    }
}
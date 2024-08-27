namespace FunctionBlock
{
    partial class CircleArrayForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CircleArrayForm));
            this.偏置数量numericUpDown = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.偏置距离textBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.偏置数量numericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // 偏置数量numericUpDown
            // 
            this.偏置数量numericUpDown.Location = new System.Drawing.Point(197, 10);
            this.偏置数量numericUpDown.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.偏置数量numericUpDown.Name = "偏置数量numericUpDown";
            this.偏置数量numericUpDown.Size = new System.Drawing.Size(70, 21);
            this.偏置数量numericUpDown.TabIndex = 49;
            this.偏置数量numericUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(142, 13);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 45;
            this.label4.Text = "偏置数量";
            // 
            // 偏置距离textBox
            // 
            this.偏置距离textBox.Location = new System.Drawing.Point(56, 9);
            this.偏置距离textBox.Name = "偏置距离textBox";
            this.偏置距离textBox.Size = new System.Drawing.Size(71, 21);
            this.偏置距离textBox.TabIndex = 44;
            this.偏置距离textBox.Text = "0";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(2, 12);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 43;
            this.label3.Text = "偏置距离";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(182, 60);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(85, 23);
            this.button1.TabIndex = 51;
            this.button1.Text = "阵列(Array)";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // CircleArrayForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(274, 84);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.偏置数量numericUpDown);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.偏置距离textBox);
            this.Controls.Add(this.label3);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CircleArrayForm";
            this.Text = "圆形阵列";
            ((System.ComponentModel.ISupportInitialize)(this.偏置数量numericUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.NumericUpDown 偏置数量numericUpDown;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox 偏置距离textBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button1;
    }
}
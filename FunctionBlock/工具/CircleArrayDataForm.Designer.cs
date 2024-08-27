namespace FunctionBlock
{
    partial class CircleArrayDataForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CircleArrayDataForm));
            this.阵列数量numericUpDown = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.参考点YtextBox = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.参考点XtextBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.角度增量textBox = new System.Windows.Forms.TextBox();
            this.半径textBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.阵列数量numericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // 阵列数量numericUpDown
            // 
            this.阵列数量numericUpDown.Location = new System.Drawing.Point(197, 10);
            this.阵列数量numericUpDown.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.阵列数量numericUpDown.Name = "阵列数量numericUpDown";
            this.阵列数量numericUpDown.Size = new System.Drawing.Size(70, 21);
            this.阵列数量numericUpDown.TabIndex = 49;
            this.阵列数量numericUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.阵列数量numericUpDown.ValueChanged += new System.EventHandler(this.阵列数量numericUpDown_ValueChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(143, 39);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 12);
            this.label5.TabIndex = 48;
            this.label5.Text = "角度增量";
            // 
            // 参考点YtextBox
            // 
            this.参考点YtextBox.Location = new System.Drawing.Point(56, 33);
            this.参考点YtextBox.Name = "参考点YtextBox";
            this.参考点YtextBox.Size = new System.Drawing.Size(71, 21);
            this.参考点YtextBox.TabIndex = 47;
            this.参考点YtextBox.Text = "0";
            this.参考点YtextBox.TextChanged += new System.EventHandler(this.参考点YtextBox_TextChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 37);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(47, 12);
            this.label6.TabIndex = 46;
            this.label6.Text = "参考点Y";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(143, 12);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 45;
            this.label4.Text = "阵列数量";
            // 
            // 参考点XtextBox
            // 
            this.参考点XtextBox.Location = new System.Drawing.Point(56, 9);
            this.参考点XtextBox.Name = "参考点XtextBox";
            this.参考点XtextBox.Size = new System.Drawing.Size(71, 21);
            this.参考点XtextBox.TabIndex = 44;
            this.参考点XtextBox.Text = "0";
            this.参考点XtextBox.TextChanged += new System.EventHandler(this.参考点XtextBox_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 12);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(47, 12);
            this.label3.TabIndex = 43;
            this.label3.Text = "参考点X";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(186, 104);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(81, 23);
            this.button1.TabIndex = 51;
            this.button1.Text = "阵列(Array)";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // 角度增量textBox
            // 
            this.角度增量textBox.Location = new System.Drawing.Point(197, 34);
            this.角度增量textBox.Name = "角度增量textBox";
            this.角度增量textBox.Size = new System.Drawing.Size(70, 21);
            this.角度增量textBox.TabIndex = 52;
            this.角度增量textBox.Text = "0";
            this.角度增量textBox.TextChanged += new System.EventHandler(this.角度增量textBox_TextChanged);
            // 
            // 半径textBox
            // 
            this.半径textBox.Location = new System.Drawing.Point(197, 62);
            this.半径textBox.Name = "半径textBox";
            this.半径textBox.Size = new System.Drawing.Size(70, 21);
            this.半径textBox.TabIndex = 54;
            this.半径textBox.Text = "0";
            this.半径textBox.TextChanged += new System.EventHandler(this.半径textBox_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(167, 67);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 53;
            this.label1.Text = "半径";
            // 
            // CircleArrayDataForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(274, 139);
            this.Controls.Add(this.半径textBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.角度增量textBox);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.阵列数量numericUpDown);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.参考点YtextBox);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.参考点XtextBox);
            this.Controls.Add(this.label3);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CircleArrayDataForm";
            this.Text = "圆形阵列";
            ((System.ComponentModel.ISupportInitialize)(this.阵列数量numericUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.NumericUpDown 阵列数量numericUpDown;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox 参考点YtextBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox 参考点XtextBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox 角度增量textBox;
        private System.Windows.Forms.TextBox 半径textBox;
        private System.Windows.Forms.Label label1;
    }
}
namespace FunctionBlock
{
    partial class JogXYMotionControlForm
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
            this.components = new System.ComponentModel.Container();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.label3 = new System.Windows.Forms.Label();
            this.JOG速度textBox = new System.Windows.Forms.TextBox();
            this.MoveXminusbutton = new System.Windows.Forms.Button();
            this.MoveYminusbutton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.MoveZminusbutton = new System.Windows.Forms.Button();
            this.MoveXAddbutton = new System.Windows.Forms.Button();
            this.MoveYAddbutton = new System.Windows.Forms.Button();
            this.MoveZAddbutton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Interval = 50;

            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(1, 110);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 12);
            this.label3.TabIndex = 20;
            this.label3.Text = "速度";
            // 
            // JOG速度textBox
            // 
            this.JOG速度textBox.Location = new System.Drawing.Point(34, 107);
            this.JOG速度textBox.Name = "JOG速度textBox";
            this.JOG速度textBox.Size = new System.Drawing.Size(70, 21);
            this.JOG速度textBox.TabIndex = 22;
            this.JOG速度textBox.Text = "10";
            this.JOG速度textBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // MoveXminusbutton
            // 
            this.MoveXminusbutton.Location = new System.Drawing.Point(0, 42);
            this.MoveXminusbutton.Name = "MoveXminusbutton";
            this.MoveXminusbutton.Size = new System.Drawing.Size(43, 25);
            this.MoveXminusbutton.TabIndex = 4;
            this.MoveXminusbutton.Text = "- X";
            this.MoveXminusbutton.UseVisualStyleBackColor = true;
            this.MoveXminusbutton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MoveXminusbutton_MouseDown);
            this.MoveXminusbutton.MouseUp += new System.Windows.Forms.MouseEventHandler(this.MoveXminusbutton_MouseUp);
            // 
            // MoveYminusbutton
            // 
            this.MoveYminusbutton.Location = new System.Drawing.Point(40, 67);
            this.MoveYminusbutton.Name = "MoveYminusbutton";
            this.MoveYminusbutton.Size = new System.Drawing.Size(25, 33);
            this.MoveYminusbutton.TabIndex = 9;
            this.MoveYminusbutton.Text = "Y - >>";
            this.MoveYminusbutton.UseVisualStyleBackColor = true;
            this.MoveYminusbutton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MoveYminusbutton_MouseDown);
            this.MoveYminusbutton.MouseUp += new System.Windows.Forms.MouseEventHandler(this.MoveYminusbutton_MouseUp);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(112, 112);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 21;
            this.label1.Text = "mm/s";
            // 
            // MoveZminusbutton
            // 
            this.MoveZminusbutton.Location = new System.Drawing.Point(110, 68);
            this.MoveZminusbutton.Name = "MoveZminusbutton";
            this.MoveZminusbutton.Size = new System.Drawing.Size(25, 33);
            this.MoveZminusbutton.TabIndex = 10;
            this.MoveZminusbutton.Text = "Z - >>";
            this.MoveZminusbutton.UseVisualStyleBackColor = true;
            this.MoveZminusbutton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MoveZminusbutton_MouseDown);
            this.MoveZminusbutton.MouseUp += new System.Windows.Forms.MouseEventHandler(this.MoveZminusbutton_MouseUp);
            // 
            // MoveXAddbutton
            // 
            this.MoveXAddbutton.Location = new System.Drawing.Point(61, 42);
            this.MoveXAddbutton.Name = "MoveXAddbutton";
            this.MoveXAddbutton.Size = new System.Drawing.Size(43, 25);
            this.MoveXAddbutton.TabIndex = 11;
            this.MoveXAddbutton.Text = "X +";
            this.MoveXAddbutton.UseVisualStyleBackColor = true;
            this.MoveXAddbutton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MoveXAddbutton_MouseDown);
            this.MoveXAddbutton.MouseUp += new System.Windows.Forms.MouseEventHandler(this.MoveXAddbutton_MouseUp);
            // 
            // MoveYAddbutton
            // 
            this.MoveYAddbutton.Location = new System.Drawing.Point(39, 7);
            this.MoveYAddbutton.Name = "MoveYAddbutton";
            this.MoveYAddbutton.Size = new System.Drawing.Size(25, 33);
            this.MoveYAddbutton.TabIndex = 12;
            this.MoveYAddbutton.Text = "Y + >>";
            this.MoveYAddbutton.UseVisualStyleBackColor = true;
            this.MoveYAddbutton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MoveYAddbutton_MouseDown);
            this.MoveYAddbutton.MouseUp += new System.Windows.Forms.MouseEventHandler(this.MoveYAddbutton_MouseUp);
            // 
            // MoveZAddbutton
            // 
            this.MoveZAddbutton.Location = new System.Drawing.Point(110, 6);
            this.MoveZAddbutton.Name = "MoveZAddbutton";
            this.MoveZAddbutton.Size = new System.Drawing.Size(25, 33);
            this.MoveZAddbutton.TabIndex = 13;
            this.MoveZAddbutton.Text = "Z + >>";
            this.MoveZAddbutton.UseVisualStyleBackColor = true;
            this.MoveZAddbutton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MoveZAddbutton_MouseDown);
            this.MoveZAddbutton.MouseUp += new System.Windows.Forms.MouseEventHandler(this.MoveZAddbutton_MouseUp);
            // 
            // JogXYMotionControlForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(149, 133);
            this.Controls.Add(this.MoveZAddbutton);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.MoveYAddbutton);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.JOG速度textBox);
            this.Controls.Add(this.MoveZminusbutton);
            this.Controls.Add(this.MoveXminusbutton);
            this.Controls.Add(this.MoveYminusbutton);
            this.Controls.Add(this.MoveXAddbutton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "JogXYMotionControlForm";
            this.Text = "运动控制";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MotionControl_FormClosing);
            this.Load += new System.EventHandler(this.JogXYMotionControlForm_Load);
            this.SizeChanged += new System.EventHandler(this.MotionControl_SizeChanged);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button MoveZAddbutton;
        private System.Windows.Forms.Button MoveYAddbutton;
        private System.Windows.Forms.Button MoveXAddbutton;
        private System.Windows.Forms.Button MoveZminusbutton;
        private System.Windows.Forms.Button MoveYminusbutton;
        private System.Windows.Forms.Button MoveXminusbutton;
        private System.Windows.Forms.TextBox JOG速度textBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
    }
}
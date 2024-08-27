namespace FunctionBlock
{
    partial class ViewForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ViewForm));
            this.hWindowControl1 = new HalconDotNet.HWindowControl();
            this.titleLabel = new System.Windows.Forms.Label();
            this.传感器comboBox1 = new System.Windows.Forms.ComboBox();
            this.buttonClose = new System.Windows.Forms.Button();
            this.buttonMax = new System.Windows.Forms.Button();
            this.buttonMin = new System.Windows.Forms.Button();
            this.实时采集checkBox = new System.Windows.Forms.CheckBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.SignLabel = new System.Windows.Forms.Label();
            this.程序节点comboBox = new System.Windows.Forms.ComboBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.tableLayoutPanel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // hWindowControl1
            // 
            this.hWindowControl1.AllowDrop = true;
            this.hWindowControl1.BackColor = System.Drawing.Color.Gray;
            this.hWindowControl1.BorderColor = System.Drawing.Color.Gray;
            this.hWindowControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.hWindowControl1.ImagePart = new System.Drawing.Rectangle(0, 0, 640, 480);
            this.hWindowControl1.Location = new System.Drawing.Point(0, 0);
            this.hWindowControl1.Margin = new System.Windows.Forms.Padding(0);
            this.hWindowControl1.Name = "hWindowControl1";
            this.hWindowControl1.Size = new System.Drawing.Size(558, 408);
            this.hWindowControl1.TabIndex = 2;
            this.hWindowControl1.WindowSize = new System.Drawing.Size(558, 408);
            this.hWindowControl1.DragEnter += new System.Windows.Forms.DragEventHandler(this.hWindowControl1_DragEnter);
            // 
            // titleLabel
            // 
            this.titleLabel.AutoSize = true;
            this.titleLabel.BackColor = System.Drawing.Color.LightGray;
            this.tableLayoutPanel2.SetColumnSpan(this.titleLabel, 5);
            this.titleLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.titleLabel.Location = new System.Drawing.Point(2, 2);
            this.titleLabel.Margin = new System.Windows.Forms.Padding(2);
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size(479, 16);
            this.titleLabel.TabIndex = 22;
            this.titleLabel.Text = "CxVision";
            this.titleLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.titleLabel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.label1_MouseDown);
            this.titleLabel.MouseEnter += new System.EventHandler(this.label1_MouseEnter);
            this.titleLabel.MouseLeave += new System.EventHandler(this.label1_MouseLeave);
            // 
            // 传感器comboBox1
            // 
            this.传感器comboBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.传感器comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.传感器comboBox1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.传感器comboBox1.FormattingEnabled = true;
            this.传感器comboBox1.ItemHeight = 12;
            this.传感器comboBox1.Location = new System.Drawing.Point(56, 20);
            this.传感器comboBox1.Margin = new System.Windows.Forms.Padding(0);
            this.传感器comboBox1.Name = "传感器comboBox1";
            this.传感器comboBox1.Size = new System.Drawing.Size(165, 20);
            this.传感器comboBox1.TabIndex = 3;
            this.传感器comboBox1.SelectedIndexChanged += new System.EventHandler(this.传感器comboBox_SelectedIndexChanged);
            this.传感器comboBox1.SelectionChangeCommitted += new System.EventHandler(this.传感器comboBox_SelectionChangeCommitted);
            // 
            // buttonClose
            // 
            this.buttonClose.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("buttonClose.BackgroundImage")));
            this.buttonClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.buttonClose.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonClose.Location = new System.Drawing.Point(537, 0);
            this.buttonClose.Margin = new System.Windows.Forms.Padding(0);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(27, 20);
            this.buttonClose.TabIndex = 0;
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // buttonMax
            // 
            this.buttonMax.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("buttonMax.BackgroundImage")));
            this.buttonMax.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.buttonMax.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonMax.Location = new System.Drawing.Point(509, 0);
            this.buttonMax.Margin = new System.Windows.Forms.Padding(0);
            this.buttonMax.Name = "buttonMax";
            this.buttonMax.Size = new System.Drawing.Size(28, 20);
            this.buttonMax.TabIndex = 1;
            this.buttonMax.UseVisualStyleBackColor = true;
            this.buttonMax.Click += new System.EventHandler(this.buttonMax_Click);
            // 
            // buttonMin
            // 
            this.buttonMin.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("buttonMin.BackgroundImage")));
            this.buttonMin.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.buttonMin.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonMin.Location = new System.Drawing.Point(483, 0);
            this.buttonMin.Margin = new System.Windows.Forms.Padding(0);
            this.buttonMin.Name = "buttonMin";
            this.buttonMin.Size = new System.Drawing.Size(26, 20);
            this.buttonMin.TabIndex = 2;
            this.buttonMin.UseVisualStyleBackColor = true;
            this.buttonMin.Click += new System.EventHandler(this.buttonMin_Click);
            // 
            // 实时采集checkBox
            // 
            this.实时采集checkBox.Appearance = System.Windows.Forms.Appearance.Button;
            this.实时采集checkBox.BackColor = System.Drawing.Color.Lime;
            this.实时采集checkBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.实时采集checkBox.Location = new System.Drawing.Point(0, 20);
            this.实时采集checkBox.Margin = new System.Windows.Forms.Padding(0);
            this.实时采集checkBox.Name = "实时采集checkBox";
            this.实时采集checkBox.Size = new System.Drawing.Size(20, 20);
            this.实时采集checkBox.TabIndex = 19;
            this.实时采集checkBox.TabStop = false;
            this.实时采集checkBox.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.实时采集checkBox.UseVisualStyleBackColor = false;
            this.实时采集checkBox.CheckedChanged += new System.EventHandler(this.实时采集checkBox_CheckedChanged);
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 8;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 36F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 165F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 76F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 27F));
            this.tableLayoutPanel2.Controls.Add(this.SignLabel, 3, 1);
            this.tableLayoutPanel2.Controls.Add(this.程序节点comboBox, 4, 1);
            this.tableLayoutPanel2.Controls.Add(this.buttonMax, 6, 0);
            this.tableLayoutPanel2.Controls.Add(this.buttonClose, 7, 0);
            this.tableLayoutPanel2.Controls.Add(this.buttonMin, 5, 0);
            this.tableLayoutPanel2.Controls.Add(this.传感器comboBox1, 2, 1);
            this.tableLayoutPanel2.Controls.Add(this.实时采集checkBox, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.titleLabel, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.panel1, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.label1, 1, 1);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(2);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 3;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(564, 454);
            this.tableLayoutPanel2.TabIndex = 2;
            // 
            // SignLabel
            // 
            this.SignLabel.AutoSize = true;
            this.SignLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SignLabel.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.SignLabel.Location = new System.Drawing.Point(224, 20);
            this.SignLabel.Name = "SignLabel";
            this.SignLabel.Size = new System.Drawing.Size(180, 20);
            this.SignLabel.TabIndex = 3;
            this.SignLabel.Text = "label1";
            this.SignLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // 程序节点comboBox
            // 
            this.tableLayoutPanel2.SetColumnSpan(this.程序节点comboBox, 4);
            this.程序节点comboBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.程序节点comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.程序节点comboBox.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.程序节点comboBox.FormattingEnabled = true;
            this.程序节点comboBox.ItemHeight = 12;
            this.程序节点comboBox.Location = new System.Drawing.Point(407, 20);
            this.程序节点comboBox.Margin = new System.Windows.Forms.Padding(0);
            this.程序节点comboBox.Name = "程序节点comboBox";
            this.程序节点comboBox.Size = new System.Drawing.Size(157, 20);
            this.程序节点comboBox.TabIndex = 24;
            this.程序节点comboBox.DropDown += new System.EventHandler(this.程序节点comboBox_DropDown);
            this.程序节点comboBox.SelectionChangeCommitted += new System.EventHandler(this.程序节点comboBox_SelectionChangeCommitted);
            // 
            // panel1
            // 
            this.tableLayoutPanel2.SetColumnSpan(this.panel1, 8);
            this.panel1.Controls.Add(this.hWindowControl1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 43);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(558, 408);
            this.panel1.TabIndex = 25;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Location = new System.Drawing.Point(23, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(30, 20);
            this.label1.TabIndex = 26;
            this.label1.Text = "实时";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ViewForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(564, 454);
            this.ControlBox = false;
            this.Controls.Add(this.tableLayoutPanel2);
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MinimumSize = new System.Drawing.Size(100, 30);
            this.Name = "ViewForm";
            this.ShowIcon = false;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ImageViewForm_FormClosing);
            this.Load += new System.EventHandler(this.ImageViewForm_Load);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ViewForm_MouseDown);
            this.Move += new System.EventHandler(this.ViewForm_Move);
            this.Resize += new System.EventHandler(this.ViewForm_Resize);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private HalconDotNet.HWindowControl hWindowControl1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.CheckBox 实时采集checkBox;
        private System.Windows.Forms.Button buttonMin;
        private System.Windows.Forms.Button buttonMax;
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.ComboBox 传感器comboBox1;
        private System.Windows.Forms.Label titleLabel;
        private System.Windows.Forms.ComboBox 程序节点comboBox;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label SignLabel;
        private System.Windows.Forms.Label label1;
    }
}
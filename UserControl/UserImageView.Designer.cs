
namespace userControl
{
    partial class UserImageView
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.传感器comboBox = new System.Windows.Forms.ComboBox();
            this.hWindowControl1 = new HalconDotNet.HWindowControl();
            this.SuspendLayout();
            // 
            // 传感器comboBox
            // 
            this.传感器comboBox.Dock = System.Windows.Forms.DockStyle.Top;
            this.传感器comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.传感器comboBox.FormattingEnabled = true;
            this.传感器comboBox.Location = new System.Drawing.Point(0, 0);
            this.传感器comboBox.Margin = new System.Windows.Forms.Padding(0);
            this.传感器comboBox.Name = "传感器comboBox";
            this.传感器comboBox.Size = new System.Drawing.Size(545, 20);
            this.传感器comboBox.TabIndex = 0;
            // 
            // hWindowControl1
            // 
            this.hWindowControl1.BackColor = System.Drawing.Color.Black;
            this.hWindowControl1.BorderColor = System.Drawing.Color.Black;
            this.hWindowControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.hWindowControl1.ImagePart = new System.Drawing.Rectangle(0, 0, 640, 480);
            this.hWindowControl1.Location = new System.Drawing.Point(0, 20);
            this.hWindowControl1.Margin = new System.Windows.Forms.Padding(0);
            this.hWindowControl1.Name = "hWindowControl1";
            this.hWindowControl1.Size = new System.Drawing.Size(545, 462);
            this.hWindowControl1.TabIndex = 1;
            this.hWindowControl1.WindowSize = new System.Drawing.Size(545, 462);
            // 
            // UserImageView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.hWindowControl1);
            this.Controls.Add(this.传感器comboBox);
            this.Name = "UserImageView";
            this.Size = new System.Drawing.Size(545, 482);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox 传感器comboBox;
        private HalconDotNet.HWindowControl hWindowControl1;
    }
}

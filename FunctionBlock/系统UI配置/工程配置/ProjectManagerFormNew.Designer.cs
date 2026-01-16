
namespace FunctionBlock
{
    partial class ProjectManagerFormNew
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProjectManagerFormNew));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.工程配置tabControl = new System.Windows.Forms.TabControl();
            this.坐标系TabPage = new System.Windows.Forms.TabPage();
            this.标定参数tabPage = new System.Windows.Forms.TabPage();
            this.夹抓tabPage = new System.Windows.Forms.TabPage();
            this.通信配置tabPage = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel1.SuspendLayout();
            this.工程配置tabControl.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 6;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 216F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 37F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.Controls.Add(this.工程配置tabControl, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 5;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 66F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 494F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 45F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1220, 745);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // 工程配置tabControl
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.工程配置tabControl, 6);
            this.工程配置tabControl.Controls.Add(this.坐标系TabPage);
            this.工程配置tabControl.Controls.Add(this.标定参数tabPage);
            this.工程配置tabControl.Controls.Add(this.夹抓tabPage);
            this.工程配置tabControl.Controls.Add(this.通信配置tabPage);
            this.工程配置tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.工程配置tabControl.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.工程配置tabControl.Location = new System.Drawing.Point(0, 0);
            this.工程配置tabControl.Margin = new System.Windows.Forms.Padding(0);
            this.工程配置tabControl.Name = "工程配置tabControl";
            this.tableLayoutPanel1.SetRowSpan(this.工程配置tabControl, 5);
            this.工程配置tabControl.SelectedIndex = 0;
            this.工程配置tabControl.Size = new System.Drawing.Size(1220, 745);
            this.工程配置tabControl.TabIndex = 28;
            this.工程配置tabControl.SelectedIndexChanged += new System.EventHandler(this.工程配置tabControl_SelectedIndexChanged);
            // 
            // 坐标系TabPage
            // 
            this.坐标系TabPage.BackColor = System.Drawing.SystemColors.Control;
            this.坐标系TabPage.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.坐标系TabPage.Location = new System.Drawing.Point(4, 29);
            this.坐标系TabPage.Margin = new System.Windows.Forms.Padding(0);
            this.坐标系TabPage.Name = "坐标系TabPage";
            this.坐标系TabPage.Size = new System.Drawing.Size(1212, 712);
            this.坐标系TabPage.TabIndex = 0;
            this.坐标系TabPage.Text = "坐标系配置";
            // 
            // 标定参数tabPage
            // 
            this.标定参数tabPage.BackColor = System.Drawing.SystemColors.Control;
            this.标定参数tabPage.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.标定参数tabPage.Location = new System.Drawing.Point(4, 29);
            this.标定参数tabPage.Margin = new System.Windows.Forms.Padding(0);
            this.标定参数tabPage.Name = "标定参数tabPage";
            this.标定参数tabPage.Size = new System.Drawing.Size(1212, 680);
            this.标定参数tabPage.TabIndex = 1;
            this.标定参数tabPage.Text = "标定管理";
            // 
            // 夹抓tabPage
            // 
            this.夹抓tabPage.BackColor = System.Drawing.SystemColors.Control;
            this.夹抓tabPage.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.夹抓tabPage.Location = new System.Drawing.Point(4, 29);
            this.夹抓tabPage.Margin = new System.Windows.Forms.Padding(0);
            this.夹抓tabPage.Name = "夹抓tabPage";
            this.夹抓tabPage.Size = new System.Drawing.Size(1212, 680);
            this.夹抓tabPage.TabIndex = 2;
            this.夹抓tabPage.Text = "夹抓配置";
            // 
            // 通信配置tabPage
            // 
            this.通信配置tabPage.BackColor = System.Drawing.SystemColors.Control;
            this.通信配置tabPage.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.通信配置tabPage.Location = new System.Drawing.Point(4, 29);
            this.通信配置tabPage.Margin = new System.Windows.Forms.Padding(0);
            this.通信配置tabPage.Name = "通信配置tabPage";
            this.通信配置tabPage.Size = new System.Drawing.Size(1212, 680);
            this.通信配置tabPage.TabIndex = 3;
            this.通信配置tabPage.Text = "通信配置";
            // 
            // ProjectManagerFormNew
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1220, 745);
            this.ControlBox = false;
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ProjectManagerFormNew";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ReportQueryForm_FormClosing);
            this.Load += new System.EventHandler(this.ProjectManagerFormNew_Load);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ReportQueryForm_MouseDown);
            this.Move += new System.EventHandler(this.ReportQueryForm_Move);
            this.Resize += new System.EventHandler(this.ReportQueryForm_Resize);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.工程配置tabControl.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TabPage 坐标系TabPage;
        private System.Windows.Forms.TabPage 标定参数tabPage;
        private System.Windows.Forms.TabPage 夹抓tabPage;
        private System.Windows.Forms.TabPage 通信配置tabPage;
        public System.Windows.Forms.TabControl 工程配置tabControl;
    }
}

namespace FunctionBlock
{
    partial class InParamForm
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.输入参数dataGridView = new System.Windows.Forms.DataGridView();
            this.ParamTypeCol = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.ParamNameCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.WcsRect2Col = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DeletCol = new System.Windows.Forms.DataGridViewButtonColumn();
            ((System.ComponentModel.ISupportInitialize)(this.输入参数dataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // 输入参数dataGridView
            // 
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.输入参数dataGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.输入参数dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.输入参数dataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ParamTypeCol,
            this.ParamNameCol,
            this.WcsRect2Col,
            this.DeletCol});
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.输入参数dataGridView.DefaultCellStyle = dataGridViewCellStyle3;
            this.输入参数dataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.输入参数dataGridView.Location = new System.Drawing.Point(0, 0);
            this.输入参数dataGridView.Name = "输入参数dataGridView";
            this.输入参数dataGridView.RowHeadersWidth = 5;
            this.输入参数dataGridView.RowTemplate.Height = 23;
            this.输入参数dataGridView.Size = new System.Drawing.Size(368, 323);
            this.输入参数dataGridView.TabIndex = 2;
            this.输入参数dataGridView.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.输入参数dataGridView_CellContentClick);
            // 
            // ParamTypeCol
            // 
            this.ParamTypeCol.DataPropertyName = "ParamType";
            this.ParamTypeCol.HeaderText = "参数类型";
            this.ParamTypeCol.Name = "ParamTypeCol";
            // 
            // ParamNameCol
            // 
            this.ParamNameCol.DataPropertyName = "ParamName";
            this.ParamNameCol.HeaderText = "参数名称";
            this.ParamNameCol.Name = "ParamNameCol";
            // 
            // WcsRect2Col
            // 
            this.WcsRect2Col.DataPropertyName = "ParamValue";
            this.WcsRect2Col.HeaderText = "参数值";
            this.WcsRect2Col.Name = "WcsRect2Col";
            // 
            // DeletCol
            // 
            this.DeletCol.DataPropertyName = "NONE";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.NullValue = "删除";
            this.DeletCol.DefaultCellStyle = dataGridViewCellStyle2;
            this.DeletCol.HeaderText = "删除";
            this.DeletCol.Name = "DeletCol";
            this.DeletCol.Width = 60;
            // 
            // InParamForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(368, 323);
            this.Controls.Add(this.输入参数dataGridView);
            this.Name = "InParamForm";
            this.ShowIcon = false;
            this.Text = "脚本输入参数设置";
            ((System.ComponentModel.ISupportInitialize)(this.输入参数dataGridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView 输入参数dataGridView;
        private System.Windows.Forms.DataGridViewComboBoxColumn ParamTypeCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn ParamNameCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn WcsRect2Col;
        private System.Windows.Forms.DataGridViewButtonColumn DeletCol;
    }
}
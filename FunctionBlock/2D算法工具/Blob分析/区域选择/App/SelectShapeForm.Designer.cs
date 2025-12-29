namespace FunctionBlock
{
    partial class SelectShapeForm
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.ActiveCol = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.FeatureCol = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.OperaterCol = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.MinValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MaxValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.UnionCol = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Method = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TeachCol = new System.Windows.Forms.DataGridViewButtonColumn();
            this.DeleteCol = new System.Windows.Forms.DataGridViewButtonColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ActiveCol,
            this.FeatureCol,
            this.OperaterCol,
            this.MinValue,
            this.MaxValue,
            this.UnionCol,
            this.Method,
            this.TeachCol,
            this.DeleteCol});
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 0);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersWidth = 5;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(529, 621);
            this.dataGridView1.TabIndex = 23;
            this.dataGridView1.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dataGridView1_DataError);
            // 
            // ActiveCol
            // 
            this.ActiveCol.DataPropertyName = "Active";
            this.ActiveCol.HeaderText = "激活";
            this.ActiveCol.Name = "ActiveCol";
            this.ActiveCol.Width = 50;
            // 
            // FeatureCol
            // 
            this.FeatureCol.DataPropertyName = "Features";
            this.FeatureCol.HeaderText = "特征名称";
            this.FeatureCol.Name = "FeatureCol";
            // 
            // OperaterCol
            // 
            this.OperaterCol.DataPropertyName = "Operation";
            this.OperaterCol.HeaderText = "操作";
            this.OperaterCol.Name = "OperaterCol";
            this.OperaterCol.Width = 70;
            // 
            // MinValue
            // 
            this.MinValue.DataPropertyName = "Min";
            this.MinValue.HeaderText = "最小值";
            this.MinValue.Name = "MinValue";
            // 
            // MaxValue
            // 
            this.MaxValue.DataPropertyName = "Max";
            this.MaxValue.HeaderText = "最大值";
            this.MaxValue.Name = "MaxValue";
            // 
            // UnionCol
            // 
            this.UnionCol.DataPropertyName = "IsUnion";
            this.UnionCol.HeaderText = "合并";
            this.UnionCol.Name = "UnionCol";
            this.UnionCol.Width = 50;
            // 
            // Method
            // 
            this.Method.HeaderText = "方法";
            this.Method.Name = "Method";
            // 
            // TeachCol
            // 
            this.TeachCol.DataPropertyName = "NONE";
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.NullValue = "示教";
            this.TeachCol.DefaultCellStyle = dataGridViewCellStyle1;
            this.TeachCol.HeaderText = "示教";
            this.TeachCol.Name = "TeachCol";
            this.TeachCol.Width = 50;
            // 
            // DeleteCol
            // 
            this.DeleteCol.DataPropertyName = "NONE";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.NullValue = "删除";
            this.DeleteCol.DefaultCellStyle = dataGridViewCellStyle2;
            this.DeleteCol.HeaderText = "删除";
            this.DeleteCol.Name = "DeleteCol";
            this.DeleteCol.Width = 50;
            // 
            // SelectShapeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(529, 621);
            this.Controls.Add(this.dataGridView1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "SelectShapeForm";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewCheckBoxColumn ActiveCol;
        private System.Windows.Forms.DataGridViewComboBoxColumn FeatureCol;
        private System.Windows.Forms.DataGridViewComboBoxColumn OperaterCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn MinValue;
        private System.Windows.Forms.DataGridViewTextBoxColumn MaxValue;
        private System.Windows.Forms.DataGridViewCheckBoxColumn UnionCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn Method;
        private System.Windows.Forms.DataGridViewButtonColumn TeachCol;
        private System.Windows.Forms.DataGridViewButtonColumn DeleteCol;
    }
}
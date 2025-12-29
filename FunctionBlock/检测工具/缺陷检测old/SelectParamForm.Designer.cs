namespace FunctionBlock
{
    partial class SelectParamForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GroupParamForm));
            this.瑕疵类型comboBox = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.瑕疵描述textBox = new System.Windows.Forms.TextBox();
            this.添加Btn = new System.Windows.Forms.Button();
            this.删除Btn = new System.Windows.Forms.Button();
            this.保存Btn = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.单位长度缺陷数量usrCtrlCondition = new userControl.UsrCtrlCondition();
            this.直径usrCtrlCondition = new userControl.UsrCtrlCondition();
            this.紧密度usrCtrlCondition = new userControl.UsrCtrlCondition();
            this.矩形度usrCtrlCondition = new userControl.UsrCtrlCondition();
            this.圆度usrCtrlCondition = new userControl.UsrCtrlCondition();
            this.平均灰度usrCtrlCondition = new userControl.UsrCtrlCondition();
            this.灰度差异usrCtrlCondition = new userControl.UsrCtrlCondition();
            this.长宽比usrCtrlCondition = new userControl.UsrCtrlCondition();
            this.面积usrCtrlCondition = new userControl.UsrCtrlCondition();
            this.长度usrCtrlCondition = new userControl.UsrCtrlCondition();
            this.宽度usrCtrlCondition = new userControl.UsrCtrlCondition();
            this.激活checkBox = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // 瑕疵类型comboBox
            // 
            this.瑕疵类型comboBox.FormattingEnabled = true;
            this.瑕疵类型comboBox.Location = new System.Drawing.Point(71, 15);
            this.瑕疵类型comboBox.Name = "瑕疵类型comboBox";
            this.瑕疵类型comboBox.Size = new System.Drawing.Size(237, 20);
            this.瑕疵类型comboBox.TabIndex = 3;
            this.瑕疵类型comboBox.SelectedIndexChanged += new System.EventHandler(this.瑕疵类型comboBox_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 12);
            this.label1.TabIndex = 4;
            this.label1.Text = "瑕疵类型:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 49);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 12);
            this.label2.TabIndex = 5;
            this.label2.Text = "瑕疵描述:";
            // 
            // 瑕疵描述textBox
            // 
            this.瑕疵描述textBox.Location = new System.Drawing.Point(71, 45);
            this.瑕疵描述textBox.Name = "瑕疵描述textBox";
            this.瑕疵描述textBox.Size = new System.Drawing.Size(237, 21);
            this.瑕疵描述textBox.TabIndex = 6;
            // 
            // 添加Btn
            // 
            this.添加Btn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.添加Btn.Location = new System.Drawing.Point(138, 117);
            this.添加Btn.Name = "添加Btn";
            this.添加Btn.Size = new System.Drawing.Size(75, 44);
            this.添加Btn.TabIndex = 7;
            this.添加Btn.Text = "添加(Add+)";
            this.添加Btn.UseVisualStyleBackColor = true;
            this.添加Btn.Click += new System.EventHandler(this.添加Btn_Click);
            // 
            // 删除Btn
            // 
            this.删除Btn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.删除Btn.Location = new System.Drawing.Point(219, 117);
            this.删除Btn.Name = "删除Btn";
            this.删除Btn.Size = new System.Drawing.Size(75, 44);
            this.删除Btn.TabIndex = 8;
            this.删除Btn.Text = "删除(Delete)";
            this.删除Btn.UseVisualStyleBackColor = true;
            this.删除Btn.Click += new System.EventHandler(this.删除Btn_Click);
            // 
            // 保存Btn
            // 
            this.保存Btn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.保存Btn.Location = new System.Drawing.Point(300, 117);
            this.保存Btn.Name = "保存Btn";
            this.保存Btn.Size = new System.Drawing.Size(75, 44);
            this.保存Btn.TabIndex = 9;
            this.保存Btn.Text = "保存(Save)";
            this.保存Btn.UseVisualStyleBackColor = true;
            this.保存Btn.Click += new System.EventHandler(this.保存Btn_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.单位长度缺陷数量usrCtrlCondition);
            this.groupBox1.Controls.Add(this.直径usrCtrlCondition);
            this.groupBox1.Controls.Add(this.紧密度usrCtrlCondition);
            this.groupBox1.Controls.Add(this.矩形度usrCtrlCondition);
            this.groupBox1.Controls.Add(this.圆度usrCtrlCondition);
            this.groupBox1.Controls.Add(this.平均灰度usrCtrlCondition);
            this.groupBox1.Controls.Add(this.灰度差异usrCtrlCondition);
            this.groupBox1.Controls.Add(this.长宽比usrCtrlCondition);
            this.groupBox1.Controls.Add(this.面积usrCtrlCondition);
            this.groupBox1.Controls.Add(this.长度usrCtrlCondition);
            this.groupBox1.Controls.Add(this.宽度usrCtrlCondition);
            this.groupBox1.Location = new System.Drawing.Point(6, 167);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(369, 449);
            this.groupBox1.TabIndex = 10;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "分类参数";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(348, 107);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(17, 12);
            this.label6.TabIndex = 15;
            this.label6.Text = "mm";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(348, 67);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(17, 12);
            this.label5.TabIndex = 14;
            this.label5.Text = "mm";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(348, 27);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(17, 12);
            this.label4.TabIndex = 13;
            this.label4.Text = "mm";
            // 
            // 单位长度缺陷数量usrCtrlCondition
            // 
            this.单位长度缺陷数量usrCtrlCondition.Condition = "< 单位长度缺陷数量 ≦";
            this.单位长度缺陷数量usrCtrlCondition.DecimalPlaces = 2;
            this.单位长度缺陷数量usrCtrlCondition.Location = new System.Drawing.Point(4, 411);
            this.单位长度缺陷数量usrCtrlCondition.MaxValue = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.单位长度缺陷数量usrCtrlCondition.MinValue = new decimal(new int[] {
            1000000,
            0,
            0,
            -2147483648});
            this.单位长度缺陷数量usrCtrlCondition.Name = "单位长度缺陷数量usrCtrlCondition";
            this.单位长度缺陷数量usrCtrlCondition.Size = new System.Drawing.Size(340, 35);
            this.单位长度缺陷数量usrCtrlCondition.TabIndex = 12;
            this.单位长度缺陷数量usrCtrlCondition.Value = ((userControl.St_Condition)(resources.GetObject("单位长度缺陷数量usrCtrlCondition.Value")));
            this.单位长度缺陷数量usrCtrlCondition.ValueChangeEvent += new userControl.ValueChangeDele(this.单位长度缺陷数量usrCtrlCondition_ValueChangeEvent);
            // 
            // 直径usrCtrlCondition
            // 
            this.直径usrCtrlCondition.Condition = "< 直径 ≦";
            this.直径usrCtrlCondition.DecimalPlaces = 2;
            this.直径usrCtrlCondition.Location = new System.Drawing.Point(4, 374);
            this.直径usrCtrlCondition.MaxValue = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.直径usrCtrlCondition.MinValue = new decimal(new int[] {
            1000000,
            0,
            0,
            -2147483648});
            this.直径usrCtrlCondition.Name = "直径usrCtrlCondition";
            this.直径usrCtrlCondition.Size = new System.Drawing.Size(340, 35);
            this.直径usrCtrlCondition.TabIndex = 9;
            this.直径usrCtrlCondition.Value = ((userControl.St_Condition)(resources.GetObject("直径usrCtrlCondition.Value")));
            this.直径usrCtrlCondition.ValueChangeEvent += new userControl.ValueChangeDele(this.直径usrCtrlCondition_ValueChangeEvent);
            // 
            // 紧密度usrCtrlCondition
            // 
            this.紧密度usrCtrlCondition.Condition = "< 紧密度 ≦";
            this.紧密度usrCtrlCondition.DecimalPlaces = 2;
            this.紧密度usrCtrlCondition.Location = new System.Drawing.Point(4, 336);
            this.紧密度usrCtrlCondition.MaxValue = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.紧密度usrCtrlCondition.MinValue = new decimal(new int[] {
            1000000,
            0,
            0,
            -2147483648});
            this.紧密度usrCtrlCondition.Name = "紧密度usrCtrlCondition";
            this.紧密度usrCtrlCondition.Size = new System.Drawing.Size(340, 35);
            this.紧密度usrCtrlCondition.TabIndex = 8;
            this.紧密度usrCtrlCondition.Value = ((userControl.St_Condition)(resources.GetObject("紧密度usrCtrlCondition.Value")));
            this.紧密度usrCtrlCondition.ValueChangeEvent += new userControl.ValueChangeDele(this.紧密度usrCtrlCondition_ValueChangeEvent);
            // 
            // 矩形度usrCtrlCondition
            // 
            this.矩形度usrCtrlCondition.Condition = "< 矩形度 ≦";
            this.矩形度usrCtrlCondition.DecimalPlaces = 2;
            this.矩形度usrCtrlCondition.Location = new System.Drawing.Point(4, 296);
            this.矩形度usrCtrlCondition.MaxValue = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.矩形度usrCtrlCondition.MinValue = new decimal(new int[] {
            1000000,
            0,
            0,
            -2147483648});
            this.矩形度usrCtrlCondition.Name = "矩形度usrCtrlCondition";
            this.矩形度usrCtrlCondition.Size = new System.Drawing.Size(340, 35);
            this.矩形度usrCtrlCondition.TabIndex = 7;
            this.矩形度usrCtrlCondition.Value = ((userControl.St_Condition)(resources.GetObject("矩形度usrCtrlCondition.Value")));
            this.矩形度usrCtrlCondition.ValueChangeEvent += new userControl.ValueChangeDele(this.矩形度usrCtrlCondition_ValueChangeEvent);
            // 
            // 圆度usrCtrlCondition
            // 
            this.圆度usrCtrlCondition.Condition = "< 圆度 ≦";
            this.圆度usrCtrlCondition.DecimalPlaces = 2;
            this.圆度usrCtrlCondition.Location = new System.Drawing.Point(4, 256);
            this.圆度usrCtrlCondition.MaxValue = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.圆度usrCtrlCondition.MinValue = new decimal(new int[] {
            1000000,
            0,
            0,
            -2147483648});
            this.圆度usrCtrlCondition.Name = "圆度usrCtrlCondition";
            this.圆度usrCtrlCondition.Size = new System.Drawing.Size(340, 35);
            this.圆度usrCtrlCondition.TabIndex = 6;
            this.圆度usrCtrlCondition.Value = ((userControl.St_Condition)(resources.GetObject("圆度usrCtrlCondition.Value")));
            this.圆度usrCtrlCondition.ValueChangeEvent += new userControl.ValueChangeDele(this.圆度usrCtrlCondition_ValueChangeEvent);
            // 
            // 平均灰度usrCtrlCondition
            // 
            this.平均灰度usrCtrlCondition.Condition = "< 平均灰度 ≦";
            this.平均灰度usrCtrlCondition.DecimalPlaces = 2;
            this.平均灰度usrCtrlCondition.Location = new System.Drawing.Point(4, 176);
            this.平均灰度usrCtrlCondition.MaxValue = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.平均灰度usrCtrlCondition.MinValue = new decimal(new int[] {
            1000000,
            0,
            0,
            -2147483648});
            this.平均灰度usrCtrlCondition.Name = "平均灰度usrCtrlCondition";
            this.平均灰度usrCtrlCondition.Size = new System.Drawing.Size(340, 35);
            this.平均灰度usrCtrlCondition.TabIndex = 5;
            this.平均灰度usrCtrlCondition.Value = ((userControl.St_Condition)(resources.GetObject("平均灰度usrCtrlCondition.Value")));
            this.平均灰度usrCtrlCondition.ValueChangeEvent += new userControl.ValueChangeDele(this.平均灰度usrCtrlCondition_ValueChangeEvent);
            // 
            // 灰度差异usrCtrlCondition
            // 
            this.灰度差异usrCtrlCondition.Condition = "< 灰度差异 ≦";
            this.灰度差异usrCtrlCondition.DecimalPlaces = 2;
            this.灰度差异usrCtrlCondition.Location = new System.Drawing.Point(4, 216);
            this.灰度差异usrCtrlCondition.MaxValue = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.灰度差异usrCtrlCondition.MinValue = new decimal(new int[] {
            1000000,
            0,
            0,
            -2147483648});
            this.灰度差异usrCtrlCondition.Name = "灰度差异usrCtrlCondition";
            this.灰度差异usrCtrlCondition.Size = new System.Drawing.Size(340, 35);
            this.灰度差异usrCtrlCondition.TabIndex = 4;
            this.灰度差异usrCtrlCondition.Value = ((userControl.St_Condition)(resources.GetObject("灰度差异usrCtrlCondition.Value")));
            this.灰度差异usrCtrlCondition.ValueChangeEvent += new userControl.ValueChangeDele(this.灰度差异usrCtrlCondition_ValueChangeEvent);
            // 
            // 长宽比usrCtrlCondition
            // 
            this.长宽比usrCtrlCondition.Condition = "< 长宽比 ≦";
            this.长宽比usrCtrlCondition.DecimalPlaces = 2;
            this.长宽比usrCtrlCondition.Location = new System.Drawing.Point(4, 136);
            this.长宽比usrCtrlCondition.MaxValue = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.长宽比usrCtrlCondition.MinValue = new decimal(new int[] {
            1000000,
            0,
            0,
            -2147483648});
            this.长宽比usrCtrlCondition.Name = "长宽比usrCtrlCondition";
            this.长宽比usrCtrlCondition.Size = new System.Drawing.Size(340, 35);
            this.长宽比usrCtrlCondition.TabIndex = 3;
            this.长宽比usrCtrlCondition.Value = ((userControl.St_Condition)(resources.GetObject("长宽比usrCtrlCondition.Value")));
            this.长宽比usrCtrlCondition.ValueChangeEvent += new userControl.ValueChangeDele(this.长宽比usrCtrlCondition_ValueChangeEvent);
            // 
            // 面积usrCtrlCondition
            // 
            this.面积usrCtrlCondition.Condition = "< 面积 ≦";
            this.面积usrCtrlCondition.DecimalPlaces = 2;
            this.面积usrCtrlCondition.Location = new System.Drawing.Point(4, 16);
            this.面积usrCtrlCondition.MaxValue = new decimal(new int[] {
            276447232,
            23283,
            0,
            0});
            this.面积usrCtrlCondition.MinValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.面积usrCtrlCondition.Name = "面积usrCtrlCondition";
            this.面积usrCtrlCondition.Size = new System.Drawing.Size(340, 35);
            this.面积usrCtrlCondition.TabIndex = 0;
            this.面积usrCtrlCondition.Value = ((userControl.St_Condition)(resources.GetObject("面积usrCtrlCondition.Value")));
            this.面积usrCtrlCondition.ValueChangeEvent += new userControl.ValueChangeDele(this.面积usrCtrlCondition_ValueChangeEvent);
            // 
            // 长度usrCtrlCondition
            // 
            this.长度usrCtrlCondition.Condition = "< 长度 ≦";
            this.长度usrCtrlCondition.DecimalPlaces = 2;
            this.长度usrCtrlCondition.Location = new System.Drawing.Point(4, 56);
            this.长度usrCtrlCondition.MaxValue = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.长度usrCtrlCondition.MinValue = new decimal(new int[] {
            1000000,
            0,
            0,
            -2147483648});
            this.长度usrCtrlCondition.Name = "长度usrCtrlCondition";
            this.长度usrCtrlCondition.Size = new System.Drawing.Size(340, 35);
            this.长度usrCtrlCondition.TabIndex = 1;
            this.长度usrCtrlCondition.Value = ((userControl.St_Condition)(resources.GetObject("长度usrCtrlCondition.Value")));
            this.长度usrCtrlCondition.ValueChangeEvent += new userControl.ValueChangeDele(this.长度usrCtrlCondition_ValueChangeEvent);
            // 
            // 宽度usrCtrlCondition
            // 
            this.宽度usrCtrlCondition.Condition = "< 宽度 ≦";
            this.宽度usrCtrlCondition.DecimalPlaces = 2;
            this.宽度usrCtrlCondition.Location = new System.Drawing.Point(4, 96);
            this.宽度usrCtrlCondition.MaxValue = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.宽度usrCtrlCondition.MinValue = new decimal(new int[] {
            1000000,
            0,
            0,
            -2147483648});
            this.宽度usrCtrlCondition.Name = "宽度usrCtrlCondition";
            this.宽度usrCtrlCondition.Size = new System.Drawing.Size(340, 35);
            this.宽度usrCtrlCondition.TabIndex = 2;
            this.宽度usrCtrlCondition.Value = ((userControl.St_Condition)(resources.GetObject("宽度usrCtrlCondition.Value")));
            this.宽度usrCtrlCondition.ValueChangeEvent += new userControl.ValueChangeDele(this.宽度usrCtrlCondition_ValueChangeEvent);
            // 
            // 激活checkBox
            // 
            this.激活checkBox.AutoSize = true;
            this.激活checkBox.Location = new System.Drawing.Point(71, 82);
            this.激活checkBox.Name = "激活checkBox";
            this.激活checkBox.Size = new System.Drawing.Size(84, 16);
            this.激活checkBox.TabIndex = 11;
            this.激活checkBox.Text = "激活分类？";
            this.激活checkBox.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(315, 49);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 12;
            this.label3.Text = "用于添加";
            // 
            // GroupParamForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(380, 621);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.激活checkBox);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.保存Btn);
            this.Controls.Add(this.删除Btn);
            this.Controls.Add(this.添加Btn);
            this.Controls.Add(this.瑕疵描述textBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.瑕疵类型comboBox);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "GroupParamForm";
            this.ShowIcon = false;
            this.Text = "瑕疵分类配置";
            this.Load += new System.EventHandler(this.SelectParamForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private userControl.UsrCtrlCondition 面积usrCtrlCondition;
        private userControl.UsrCtrlCondition 长度usrCtrlCondition;
        private userControl.UsrCtrlCondition 宽度usrCtrlCondition;
        private System.Windows.Forms.ComboBox 瑕疵类型comboBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox 瑕疵描述textBox;
        private System.Windows.Forms.Button 添加Btn;
        private System.Windows.Forms.Button 删除Btn;
        private System.Windows.Forms.Button 保存Btn;
        private System.Windows.Forms.GroupBox groupBox1;
        private userControl.UsrCtrlCondition 紧密度usrCtrlCondition;
        private userControl.UsrCtrlCondition 矩形度usrCtrlCondition;
        private userControl.UsrCtrlCondition 圆度usrCtrlCondition;
        private userControl.UsrCtrlCondition 平均灰度usrCtrlCondition;
        private userControl.UsrCtrlCondition 灰度差异usrCtrlCondition;
        private userControl.UsrCtrlCondition 长宽比usrCtrlCondition;
        private System.Windows.Forms.CheckBox 激活checkBox;
        private userControl.UsrCtrlCondition 直径usrCtrlCondition;
        private userControl.UsrCtrlCondition 单位长度缺陷数量usrCtrlCondition;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
    }
}
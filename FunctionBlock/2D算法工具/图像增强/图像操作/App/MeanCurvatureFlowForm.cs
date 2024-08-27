﻿using FunctionBlock;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FunctionBlock
{
    public partial class MeanCurvatureFlowForm : Form
    {
        private MeanCurvatureFlowParam _param;
        public MeanCurvatureFlowForm(ImageEnhancementParam param)
        {
            this._param = param as MeanCurvatureFlowParam;
            InitializeComponent();
            BindProperty();
        }

        private void BindProperty()
        {
            try
            {
                // 创建匹配参数
                this.平滑系数ComboBox.DataBindings.Add(nameof(this.平滑系数ComboBox.Text), (this._param), nameof(this._param.Sigma), true, DataSourceUpdateMode.OnPropertyChanged);
                this.时间步长ComboBox.DataBindings.Add(nameof(this.时间步长ComboBox.Text), (this._param), nameof(this._param.Theta), true, DataSourceUpdateMode.OnPropertyChanged);
                this.迭代次数comboBox.DataBindings.Add(nameof(this.迭代次数comboBox.Text), (this._param), nameof(this._param.Iterations), true, DataSourceUpdateMode.OnPropertyChanged);

            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
            }
        }
    }
}

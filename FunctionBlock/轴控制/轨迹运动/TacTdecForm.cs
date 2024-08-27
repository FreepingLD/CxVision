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
    public partial class TacTdecForm : Form
    {
        public TacTdecForm()
        {
            InitializeComponent();
        }

        private TrackParam _trackParam;
        public TacTdecForm(TrackParam param)
        {
            InitializeComponent();
            this._trackParam = param;
        }

        private void TacTdecForm_Load(object sender, EventArgs e)
        {
            try
            {
                if (this._trackParam == null) return;
                AccDec param = this._trackParam.AccDecParam as AccDec;
                this.起始速度numericUpDown.DataBindings.Add("Text", param, nameof(param.StartVel), true, DataSourceUpdateMode.OnPropertyChanged);
                this.停止速度numericUpDown.DataBindings.Add("Text", param, nameof(param.StopVel), true, DataSourceUpdateMode.OnPropertyChanged);
                //this.运行速度numericUpDown.DataBindings.Add("Text", param, nameof(param.), true, DataSourceUpdateMode.OnPropertyChanged);
                this.加速度时间numericUpDown.DataBindings.Add("Text", param, nameof(param.Tacc), true, DataSourceUpdateMode.OnPropertyChanged);
                this.减速度时间numericUpDown.DataBindings.Add("Text", param, nameof(param.Tdec), true, DataSourceUpdateMode.OnPropertyChanged);
                this.S平滑时间numericUpDown.DataBindings.Add("Text", param, nameof(param.S_para), true, DataSourceUpdateMode.OnPropertyChanged);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}

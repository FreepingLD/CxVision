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
    public partial class TryForm : Form
    {
        private UserTryPlateHoleParam _param;
        public UserTryPlateHoleParam Param { get => _param; set => _param = value; }

        public TryForm()
        {
            InitializeComponent();
        }

        public TryForm(UserTryPlateHoleParam param)
        {
            this._param = param;
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (_param == null) return;
            this.穴位textBox.DataBindings.Add("Text", _param, nameof(_param.Describe), true, DataSourceUpdateMode.OnPropertyChanged);
            this.X_textBox.DataBindings.Add("Text", _param, nameof(_param.X), true, DataSourceUpdateMode.OnPropertyChanged);
            this.Y_textBox.DataBindings.Add("Text", _param, nameof(_param.Y), true, DataSourceUpdateMode.OnPropertyChanged);
            this.Angle_textBox.DataBindings.Add("Text", _param, nameof(_param.Angle), true, DataSourceUpdateMode.OnPropertyChanged);
            //this.Limit_X_textBox.DataBindings.Add("Text", _param, nameof(_param.LimitX), true, DataSourceUpdateMode.OnPropertyChanged);
            //this.Limit_Y_textBox.DataBindings.Add("Text", _param, nameof(_param.LimitY), true, DataSourceUpdateMode.OnPropertyChanged);
            //this.Limit_A_textBox.DataBindings.Add("Text", _param, nameof(_param.LimitAngle), true, DataSourceUpdateMode.OnPropertyChanged);
            this.夹抓comboBox.DataBindings.Add("Text", _param, nameof(_param.RobotJaw), true, DataSourceUpdateMode.OnPropertyChanged);
        }


    }
}

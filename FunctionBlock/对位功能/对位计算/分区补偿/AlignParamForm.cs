using Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static FunctionBlock.ContourModelMatch2DForm;

namespace FunctionBlock
{
    public partial class AlignParamForm : Form
    {
        private AlignMatchParam _param;
        public AlignParamForm()
        {
            InitializeComponent();
        }
        public AlignParamForm(AlignMatchParam param)
        {
            InitializeComponent();
            this._param = param;
        }
        private void AlignParamForm_Load(object sender, EventArgs e)
        {
            try
            {
                this.变换类型comboBox.DataSource = Enum.GetNames(typeof(enTransformationType));
                this.变换类型comboBox.DataBindings.Add(nameof(this.变换类型comboBox.Text), this._param, nameof(this._param.TransformationType), true, DataSourceUpdateMode.OnPropertyChanged);
                this.起始点百分比textBox.DataBindings.Add(nameof(this.起始点百分比textBox.Text), this._param, nameof(this._param.StartPercent), true, DataSourceUpdateMode.OnPropertyChanged);
                this.结束点百分比textBox.DataBindings.Add(nameof(this.结束点百分比textBox.Text), this._param, nameof(this._param.EndPercent), true, DataSourceUpdateMode.OnPropertyChanged);
                this.采样间隔textBox.DataBindings.Add(nameof(this.采样间隔textBox.Text), this._param, nameof(this._param.ResampleDist), true, DataSourceUpdateMode.OnPropertyChanged);
                /////////////////////////
            }
            catch (Exception ex)
            {
                new Common.UserMessageForm(ex.ToString()).ShowDialog();
            }
        }


    }
}

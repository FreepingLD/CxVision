using MotionControlCard;
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
    public partial class CircleParamForm : Form
    {
        private TrackParam _trackParam;
        public CircleParamForm(TrackParam param)
        {
            InitializeComponent();
            this._trackParam = param;
        }
        public CircleParamForm()
        {
            InitializeComponent();
        }

        private void CircleParamForm_Load(object sender, EventArgs e)
        {
            try
            {
                if (this._trackParam == null) return;
                drawWcsCircle _wcsCircle = this._trackParam.RoiShape as drawWcsCircle;
                this.X_textBox.DataBindings.Add("Text", _wcsCircle, nameof(_wcsCircle.X), true, DataSourceUpdateMode.OnPropertyChanged);
                this.Y_textBox.DataBindings.Add("Text", _wcsCircle, nameof(_wcsCircle.Y), true, DataSourceUpdateMode.OnPropertyChanged);
                this.Z_textBox.DataBindings.Add("Text", _wcsCircle, nameof(_wcsCircle.Z), true, DataSourceUpdateMode.OnPropertyChanged);
                this.Radius_textBox.DataBindings.Add("Text", _wcsCircle, nameof(_wcsCircle.Radius), true, DataSourceUpdateMode.OnPropertyChanged);
                //this.Y2_textBox.DataBindings.Add("Text", _wcsLine, nameof(_wcsLine.Y2), true, DataSourceUpdateMode.OnPropertyChanged);
                //this.Z2_textBox.DataBindings.Add("Text", _wcsLine, nameof(_wcsLine.Z2), true, DataSourceUpdateMode.OnPropertyChanged);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void 确定but_Click(object sender, EventArgs e)
        {
            try
            {
                double x = 0, y = 0, z = 0;
                MotionControlCard.IMotionControl _card = MotionControlCard.MotionCardManage.GetCardByCoordSysName(this._trackParam.CoordSysName.ToString());
                if (_card != null)
                {
                    _card.GetAxisPosition(this._trackParam.CoordSysName, enAxisName.X轴, out x);
                    _card.GetAxisPosition(this._trackParam.CoordSysName, enAxisName.Y轴, out y);
                    _card.GetAxisPosition(this._trackParam.CoordSysName, enAxisName.Z轴, out z);
                }
                drawWcsCircle _wcsLine = this._trackParam.RoiShape as drawWcsCircle;
                _wcsLine.X = x;
                _wcsLine.Y = y;
                _wcsLine.Z = z;
                this._trackParam.RoiShape = _wcsLine;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}

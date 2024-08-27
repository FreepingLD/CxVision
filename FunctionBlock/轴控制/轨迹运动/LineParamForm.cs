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
    public partial class LineParamForm : Form
    {
        private TrackParam _trackParam;
        public LineParamForm(TrackParam param)
        {
            InitializeComponent();
            this._trackParam = param;
        }
        public LineParamForm()
        {
            InitializeComponent();
        }

        private void LineParamForm_Load(object sender, EventArgs e)
        {
            try
            {
                if (this._trackParam == null) return;
                drawWcsLine _wcsLine = this._trackParam.RoiShape as drawWcsLine;
                this.X1_textBox.DataBindings.Add("Text", _wcsLine, nameof(_wcsLine.X1), true, DataSourceUpdateMode.OnPropertyChanged);
                this.Y1_textBox.DataBindings.Add("Text", _wcsLine, nameof(_wcsLine.Y1), true, DataSourceUpdateMode.OnPropertyChanged);
                this.Z1_textBox.DataBindings.Add("Text", _wcsLine, nameof(_wcsLine.Z1), true, DataSourceUpdateMode.OnPropertyChanged);
                this.X2_textBox.DataBindings.Add("Text", _wcsLine, nameof(_wcsLine.X2), true, DataSourceUpdateMode.OnPropertyChanged);
                this.Y2_textBox.DataBindings.Add("Text", _wcsLine, nameof(_wcsLine.Y2), true, DataSourceUpdateMode.OnPropertyChanged);
                this.Z2_textBox.DataBindings.Add("Text", _wcsLine, nameof(_wcsLine.Z2), true, DataSourceUpdateMode.OnPropertyChanged);
                //this.Theta_textBox.DataBindings.Add("Text", _wcsPoint, nameof(_wcsPoint.X), true, DataSourceUpdateMode.OnPropertyChanged);
                //this.U_textBox.DataBindings.Add("Text", _wcsPoint, nameof(_wcsPoint.X), true, DataSourceUpdateMode.OnPropertyChanged);
                //this.V_textBox.DataBindings.Add("Text", _wcsPoint, nameof(_wcsPoint.X), true, DataSourceUpdateMode.OnPropertyChanged);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void 确定but1_Click(object sender, EventArgs e)
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
                drawWcsLine _wcsLine = this._trackParam.RoiShape as drawWcsLine;
                _wcsLine.X1 = x;
                _wcsLine.Y1 = y;
                _wcsLine.Z1 = z;
                this._trackParam.RoiShape = _wcsLine;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void 确定But2_Click(object sender, EventArgs e)
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
                drawWcsLine _wcsLine = this._trackParam.RoiShape as drawWcsLine;
                _wcsLine.X2 = x;
                _wcsLine.Y2 = y;
                _wcsLine.Z2 = z;
                this._trackParam.RoiShape = _wcsLine;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }


    }
}

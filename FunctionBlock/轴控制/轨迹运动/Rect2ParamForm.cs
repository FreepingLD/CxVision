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
    public partial class Rect2ParamForm : Form
    {
        private TrackParam _trackParam;
        public Rect2ParamForm(TrackParam param)
        {
            InitializeComponent();
            this._trackParam = param;
        }
        public Rect2ParamForm()
        {
            InitializeComponent();
        }

        private void Rect2ParamForm_Load(object sender, EventArgs e)
        {
            try
            {
                if (this._trackParam == null) return;
                drawWcsRect2 _wcsRect2 = this._trackParam.RoiShape as drawWcsRect2;
                this.X_textBox.DataBindings.Add("Text", _wcsRect2, nameof(_wcsRect2.X), true, DataSourceUpdateMode.OnPropertyChanged);
                this.Y_textBox.DataBindings.Add("Text", _wcsRect2, nameof(_wcsRect2.Y), true, DataSourceUpdateMode.OnPropertyChanged);
                this.Z_textBox.DataBindings.Add("Text", _wcsRect2, nameof(_wcsRect2.Z), true, DataSourceUpdateMode.OnPropertyChanged);
                this.Deg_textBox.DataBindings.Add("Text", _wcsRect2, nameof(_wcsRect2.Deg), true, DataSourceUpdateMode.OnPropertyChanged);
                this.Len1_textBox.DataBindings.Add("Text", _wcsRect2, nameof(_wcsRect2.Length1), true, DataSourceUpdateMode.OnPropertyChanged);
                this.Len2_textBox.DataBindings.Add("Text", _wcsRect2, nameof(_wcsRect2.Length2), true, DataSourceUpdateMode.OnPropertyChanged);
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
                drawWcsRect2 _wcsRect2 = this._trackParam.RoiShape as drawWcsRect2;
                _wcsRect2.X = x;
                _wcsRect2.Y = y;
                _wcsRect2.Z = z;
                this._trackParam.RoiShape = _wcsRect2;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}

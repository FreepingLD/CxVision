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
    public partial class PointParamForm : Form
    {
        private TrackParam _trackParam;
        public PointParamForm(TrackParam param)
        {
            InitializeComponent();
            this._trackParam = param;
        }
        public PointParamForm()
        {
            InitializeComponent();
        }

        private void PointParamForm_Load(object sender, EventArgs e)
        {
            try
            {
                if (this._trackParam == null) return;
                drawWcsPoint _wcsPoint = this._trackParam.RoiShape as drawWcsPoint;
                this.X_textBox.DataBindings.Add("Text", _wcsPoint, nameof(_wcsPoint.X), true, DataSourceUpdateMode.OnPropertyChanged);
                this.Y_textBox.DataBindings.Add("Text", _wcsPoint, nameof(_wcsPoint.Y), true, DataSourceUpdateMode.OnPropertyChanged);
                this.Z_textBox.DataBindings.Add("Text", _wcsPoint, nameof(_wcsPoint.Z), true, DataSourceUpdateMode.OnPropertyChanged);
                //this.Theta_textBox.DataBindings.Add("Text", _wcsPoint, nameof(_wcsPoint.X), true, DataSourceUpdateMode.OnPropertyChanged);
                //this.U_textBox.DataBindings.Add("Text", _wcsPoint, nameof(_wcsPoint.X), true, DataSourceUpdateMode.OnPropertyChanged);
                //this.V_textBox.DataBindings.Add("Text", _wcsPoint, nameof(_wcsPoint.X), true, DataSourceUpdateMode.OnPropertyChanged);
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
                this._trackParam.RoiShape = new drawWcsPoint(x,y,z);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}

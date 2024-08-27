using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HalconDotNet;
using Common;


namespace FunctionBlock
{
    public partial class LoadTryFormOld : Form
    {
        private UserTryPlateHoleParam _param;
        public UserTryPlateHoleParam Param { get => _param; set => _param = value; }
        public View.VisualizeView drawView;
        public HWindowControl hWindowControl;
        private ImageDataClass _imageData;

        public LoadTryFormOld()
        {
            InitializeComponent();
        }

        public LoadTryFormOld(UserTryPlateHoleParam param,ImageDataClass imageData)
        {
            this._param = param;
            this._imageData = imageData;
            InitializeComponent();
        }
        public LoadTryFormOld(UserTryPlateHoleParam param,View.VisualizeView drawView, HWindowControl hWindowControl)
        {
            this._param = param;
            this.drawView = drawView;
            this.hWindowControl = hWindowControl;
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (_param == null) return;
            this.夹抓comboBox.DataSource = Enum.GetValues(typeof(enRobotJawEnum));
            this.穴位textBox.DataBindings.Add("Text", _param, nameof(_param.Describe), true, DataSourceUpdateMode.OnPropertyChanged);
            this.X_textBox.DataBindings.Add("Text", _param, nameof(_param.X), true, DataSourceUpdateMode.OnPropertyChanged);
            this.Y_textBox.DataBindings.Add("Text", _param, nameof(_param.Y), true, DataSourceUpdateMode.OnPropertyChanged);
            this.Angle_textBox.DataBindings.Add("Text", _param, nameof(_param.Angle), true, DataSourceUpdateMode.OnPropertyChanged);
            this.Limit_X_textBox.DataBindings.Add("Text", _param, nameof(_param.LimitX), true, DataSourceUpdateMode.OnPropertyChanged);
            this.Limit_Y_textBox.DataBindings.Add("Text", _param, nameof(_param.LimitY), true, DataSourceUpdateMode.OnPropertyChanged);
            this.Limit_A_textBox.DataBindings.Add("Text", _param, nameof(_param.LimitAngle), true, DataSourceUpdateMode.OnPropertyChanged);
            this.夹抓comboBox.DataBindings.Add("Text", _param, nameof(_param.RobotJaw), true, DataSourceUpdateMode.OnPropertyChanged);
        }

        private void 画图Btn_Click(object sender, EventArgs e)
        {
            try
            {
                DrawForm drawForm = new DrawForm(this._imageData, this._param.PixRoi);
                drawForm.ShowDialog();
                this._param.PixRoi = drawForm.ROI; // ((drawPixRect1)rOI).GetWcsRect1(this.drawView.CameraParam);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }






    }
}

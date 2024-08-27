using Common;
using Sensor;
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
    public partial class CamHomMatParamForm : Form
    {
        private CameraParam _cameraParam;
        private UserHomMat2D _homMat2D;
        public CamHomMatParamForm(CameraParam cameraParam)
        {
            InitializeComponent();
            this._cameraParam = cameraParam;
        }

        private void CamHomMatParamForm_Load(object sender, EventArgs e)
        {
            try
            {
                if (this._cameraParam == null || this._cameraParam.HomMat2D == null) return;
                _homMat2D = this._cameraParam.HomMat2D;
                /////////////////////////////////////////////////////
                this.C00textBox.DataBindings.Add("Text", this._homMat2D, nameof(this._homMat2D.c00), true, DataSourceUpdateMode.OnPropertyChanged);
                this.C01textBox.DataBindings.Add("Text", this._homMat2D, nameof(this._homMat2D.c01), true, DataSourceUpdateMode.OnPropertyChanged);
                this.C02textBox.DataBindings.Add("Text", this._homMat2D, nameof(this._homMat2D.c02), true, DataSourceUpdateMode.OnPropertyChanged);
                this.C10textBox.DataBindings.Add("Text", this._homMat2D, nameof(this._homMat2D.c10), true, DataSourceUpdateMode.OnPropertyChanged);
                this.C11textBox.DataBindings.Add("Text", this._homMat2D, nameof(this._homMat2D.c11), true, DataSourceUpdateMode.OnPropertyChanged);
                this.C12textBox.DataBindings.Add("Text", this._homMat2D, nameof(this._homMat2D.c12), true, DataSourceUpdateMode.OnPropertyChanged);
            }
            catch (Exception ex)
            {

            }
        }

        private void SaveButn_Click(object sender, EventArgs e)
        {
            try
            {
                this._cameraParam.Save();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void 类型comboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            try
            {
                if (this.类型comboBox.SelectedItem == null) return;
                switch(this.类型comboBox.SelectedItem.ToString())
                {
                    default:
                    case "HomMat":
                        _homMat2D = this._cameraParam?.HomMat2D;
                        if (_homMat2D == null) return;
                        /////////////////////////////////////////////////////
                        this.C00textBox.DataBindings.Clear(); 
                        this.C01textBox.DataBindings.Clear();
                        this.C02textBox.DataBindings.Clear();
                        this.C10textBox.DataBindings.Clear();
                        this.C11textBox.DataBindings.Clear();
                        this.C12textBox.DataBindings.Clear();
                        ////////////////////////////////////////////////////
                        this.C00textBox.DataBindings.Add("Text", this._homMat2D, nameof(this._homMat2D.c00), true, DataSourceUpdateMode.OnPropertyChanged);
                        this.C01textBox.DataBindings.Add("Text", this._homMat2D, nameof(this._homMat2D.c01), true, DataSourceUpdateMode.OnPropertyChanged);
                        this.C02textBox.DataBindings.Add("Text", this._homMat2D, nameof(this._homMat2D.c02), true, DataSourceUpdateMode.OnPropertyChanged);
                        this.C10textBox.DataBindings.Add("Text", this._homMat2D, nameof(this._homMat2D.c10), true, DataSourceUpdateMode.OnPropertyChanged);
                        this.C11textBox.DataBindings.Add("Text", this._homMat2D, nameof(this._homMat2D.c11), true, DataSourceUpdateMode.OnPropertyChanged);
                        this.C12textBox.DataBindings.Add("Text", this._homMat2D, nameof(this._homMat2D.c12), true, DataSourceUpdateMode.OnPropertyChanged);
                        break;
                    case "MapHomMat":
                        _homMat2D = this._cameraParam?.MapHomMat2D;
                        if (_homMat2D == null) return;
                        /////////////////////////////////////////////////////
                        this.C00textBox.DataBindings.Clear();
                        this.C01textBox.DataBindings.Clear();
                        this.C02textBox.DataBindings.Clear();
                        this.C10textBox.DataBindings.Clear();
                        this.C11textBox.DataBindings.Clear();
                        this.C12textBox.DataBindings.Clear();
                        /////////////////////////////////////////////////////
                        this.C00textBox.DataBindings.Add("Text", this._homMat2D, nameof(this._homMat2D.c00), true, DataSourceUpdateMode.OnPropertyChanged);
                        this.C01textBox.DataBindings.Add("Text", this._homMat2D, nameof(this._homMat2D.c01), true, DataSourceUpdateMode.OnPropertyChanged);
                        this.C02textBox.DataBindings.Add("Text", this._homMat2D, nameof(this._homMat2D.c02), true, DataSourceUpdateMode.OnPropertyChanged);
                        this.C10textBox.DataBindings.Add("Text", this._homMat2D, nameof(this._homMat2D.c10), true, DataSourceUpdateMode.OnPropertyChanged);
                        this.C11textBox.DataBindings.Add("Text", this._homMat2D, nameof(this._homMat2D.c11), true, DataSourceUpdateMode.OnPropertyChanged);
                        this.C12textBox.DataBindings.Add("Text", this._homMat2D, nameof(this._homMat2D.c12), true, DataSourceUpdateMode.OnPropertyChanged);
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }


    }
}

using Common;
using FunctionBlock;
using Sensor;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FunctionBlock
{
    public partial class KeyEnceLJV700ParamForm : Form
    {
        private AcqSource _acqSource;
        private ISensor _sensor;
        private FileOperate fo = new FileOperate();
        private KeyEnceLJV7000Setting keyEnceParamConfig;
        private string savePath;
        private LineLaserForm lineForm;

        public KeyEnceLJV700ParamForm(AcqSource acqSource)
        {
            InitializeComponent();
            this._acqSource = acqSource;
            this._sensor = this._acqSource.Sensor;
            this.lineForm = new LineLaserForm(this._acqSource);
            this.AddForm(this.监控panel,this.lineForm);
            this.savePath = Application.StartupPath + "\\" + "LaserParam" + "\\" + this._sensor.ConfigParam.SensorName + ".txt";
            this.keyEnceParamConfig = ((LJV7000LineLaser)this._sensor).KeyEnceLJV7000ParamConfig;
        }

        private void TriggerModeCombox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            try
            {
                string trigmode = this.触发模式Combox.SelectedItem.ToString();
                enUserTrigerMode TrigerMode;
                if (Enum.TryParse(trigmode, out TrigerMode))
                    keyEnceParamConfig.TrigMode = TrigerMode;
                //MessageBox.Show("设置成功");
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
            }
        }
        private void StilLineLaserParamForm_Load(object sender, EventArgs e)
        {
            BindProperty();
        }


        private void BindProperty()
        {
            try
            {

                this.触发模式Combox.DataSource = Enum.GetNames(typeof(enUserTrigerMode));
                this.触发源comboBox.DataSource = Enum.GetNames(typeof(enUserTriggerSource));
                /// stil线激光参数
                //////////////////
                this.触发源comboBox.DataBindings.Add("Text", keyEnceParamConfig, "TriggerSource", true, DataSourceUpdateMode.OnPropertyChanged);
                this.触发模式Combox.DataBindings.Add("Text", keyEnceParamConfig, "TrigMode", true, DataSourceUpdateMode.OnPropertyChanged);
                this.采集点数textBox.DataBindings.Add("Text", keyEnceParamConfig, "TreNum", true, DataSourceUpdateMode.OnPropertyChanged);
                //this.取消等待采集checkBox.DataBindings.Add("Checked", keyEnceParamConfig, "CancelWaite", true, DataSourceUpdateMode.OnPropertyChanged);
            }
            catch
            {

            }
        }

        private void StilLineLaserParamForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                this.keyEnceParamConfig.SaveParamConfig(this.savePath);
                if (this.lineForm != null)
                {
                    this.lineForm.Close();
                    this.lineForm.Dispose();
                }          
            }
            catch
            {

            }
        }

        private void AddForm(Panel MastPanel, Form form)
        {
            if (MastPanel == null) return;
            if (MastPanel.Controls.Count > 0)
                MastPanel.Controls.Clear();
            if (form == null) return;
            form.TopLevel = false;
            form.FormBorderStyle = FormBorderStyle.None;
            form.MaximizeBox = false;
            form.MinimizeBox = false;
            form.Dock = DockStyle.Fill;
            MastPanel.Controls.Add(form);
            form.Show();
        }

        private void 采集点数textBox_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void 采集点数textBox_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode==Keys.Enter)
            {
                int result;
                int.TryParse(this.采集点数textBox.Text,out result);
                this.keyEnceParamConfig.AcqCount = result;
            }
        }
    }
}

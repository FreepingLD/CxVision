
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
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FunctionBlock
{
    public partial class CalibrateDoubleLaserForm : Form
    {
        //private AcqSource[] source;
        private CalibrateDoubleLaser CalibrateTool;
        public CalibrateDoubleLaserForm()
        {
            InitializeComponent();
            this.CalibrateTool = new CalibrateDoubleLaser(AcqSourceManage.Instance.LaserAcqSourceList().ToArray());
        }


        private void BindProperty()
        {
            try
            {
                this.上激光comboBox.DataSource = AcqSourceManage.Instance.LaserAcqSourceList(); // GetLaserSensor();
                this.下激光comboBox.DataSource = AcqSourceManage.Instance.LaserAcqSourceList(); //GetLaserSensor();
                this.上激光comboBox.DisplayMember = "Name";
                this.下激光comboBox.DisplayMember = "Name";
                //////////////////
                this.上激光comboBox.DataBindings.Add("SelectedItem", CalibrateTool, "LaserAcqSource1", true, DataSourceUpdateMode.OnPropertyChanged); //
                this.下激光comboBox.DataBindings.Add("SelectedItem", CalibrateTool, "LaserAcqSource2", true, DataSourceUpdateMode.OnPropertyChanged);
                this.坐标间隔textBox.DataBindings.Add("Text", GlobalVariable.pConfig, "Cord_Gap", true, DataSourceUpdateMode.OnPropertyChanged);//standardThickValue
                this.标准块值textBox.DataBindings.Add("Text", GlobalVariable.pConfig, "StandardThickValue", true, DataSourceUpdateMode.OnPropertyChanged);//standardThickValue Laser1Value
                this.上激光textBox.DataBindings.Add("Text", CalibrateTool, "Laser1Value", true, DataSourceUpdateMode.OnPropertyChanged);//standardThickValue Laser1Value
                this.下激光textBox.DataBindings.Add("Text", CalibrateTool, "Laser2Value", true, DataSourceUpdateMode.OnPropertyChanged);//standardThickValue Laser1Value
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            this.CalibrateTool.Calibrate();
            this.坐标间隔textBox.Text = this.CalibrateTool.CoordDist.ToString();
        }


        private void CalibrateDoubleLaserForm_Load(object sender, EventArgs e)
        {
            BindProperty();
        }

        private void CalibrateDoubleLaserForm_FormClosing(object sender, FormClosingEventArgs e)
        {
           // new FileOperate().SaveConfigParam("ParamConfig.txt", GlobalVariable.pConfig);
            GlobalVariable.pConfig.SaveParamConfig("ParamConfig.txt");
        }
    }
}


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
    public partial class TroughCalibrateForm : Form
    {
        private IFunction _function;
        private CalibrateDoubleLaser CalibrateTool;
        public TroughCalibrateForm(TreeNode node)
        {
            InitializeComponent();
            this._function = node.Tag as IFunction;
        }

        private void CalibrateDoubleLaserForm_Load(object sender, EventArgs e)
        {
            BindProperty();
        }


        private void BindProperty()
        {
            try
            {
                TroughParam param = ((TroughCalibrate)this._function).Param;
                this.上激光comboBox.Items.Clear();
                this.上激光comboBox.Items.AddRange(AcqSourceManage.Instance.GetAcqSourceName());
                this.下激光comboBox.Items.Clear();
                this.下激光comboBox.Items.AddRange(AcqSourceManage.Instance.GetAcqSourceName());
                /////////////////////
                this.上激光comboBox.DataBindings.Add(nameof(this.上激光comboBox.Text), param, nameof(param.LaserAcqSource1), true, DataSourceUpdateMode.OnPropertyChanged);
                this.下激光comboBox.DataBindings.Add(nameof(this.下激光comboBox.Text), param, nameof(param.LaserAcqSource2), true, DataSourceUpdateMode.OnPropertyChanged);
                //////////////////
                this.上激光comboBox.DataBindings.Add("SelectedItem", param, "LaserAcqSource1", true, DataSourceUpdateMode.OnPropertyChanged); //
                this.下激光comboBox.DataBindings.Add("SelectedItem", param, "LaserAcqSource2", true, DataSourceUpdateMode.OnPropertyChanged);
                this.坐标间隔textBox.DataBindings.Add("Text", GlobalVariable.pConfig, "Cord_Gap", true, DataSourceUpdateMode.OnPropertyChanged);//standardThickValue
                this.标准块值textBox.DataBindings.Add("Text", GlobalVariable.pConfig, "StandardThickValue", true, DataSourceUpdateMode.OnPropertyChanged);//standardThickValue Laser1Value
                this.上激光textBox.DataBindings.Add("Text", param, "Laser1Value", true, DataSourceUpdateMode.OnPropertyChanged);//standardThickValue Laser1Value
                this.下激光textBox.DataBindings.Add("Text", param, "Laser2Value", true, DataSourceUpdateMode.OnPropertyChanged);//standardThickValue Laser1Value
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                this._function.Execute(null);
                this.坐标间隔textBox.Text = this.CalibrateTool.CoordDist.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }


        private void CalibrateDoubleLaserForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // new FileOperate().SaveConfigParam("ParamConfig.txt", GlobalVariable.pConfig);
            GlobalVariable.pConfig.SaveParamConfig("ParamConfig.txt");
        }
    }
}

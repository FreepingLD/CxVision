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

namespace FunctionBlock
{
    public partial class CamDistortionCalibrateForm : Form
    {
        private CameraParam CamParam;
        private Form form;

        public CamDistortionCalibrateForm()
        {
            InitializeComponent();
        }
        public CamDistortionCalibrateForm(CameraParam CamParam)
        {
            InitializeComponent();
            this.CamParam = CamParam;
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

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                switch (this.comboBox1.SelectedItem.ToString())
                {

                    case "圆形标记校正":
                        this.form = new DistortionCalibForm(this.CamParam);
                        this.AddForm(this.formPanel, this.form);
                        break;
                    case "棋盘格标记校正":
                        this.form = new GridRectificationMapForm(this.CamParam);
                        this.AddForm(this.formPanel, this.form);
                        break;
                    //case "相机拼接校正":
                    //    this.form = new MosaicParamForm(this.CamParam);
                    //    this.AddForm(this.formPanel, this.form);
                    //    break;                       
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void DistortionRectificationForm_Load(object sender, EventArgs e)
        {
            this.comboBox1.SelectedIndex = 0;
        }




    }
}

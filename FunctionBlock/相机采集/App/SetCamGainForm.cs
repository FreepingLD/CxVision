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
using System.Xml.Linq;

namespace FunctionBlock
{
    public partial class SetCamGainForm : Form
    {
        public string ReName { get; set; }
        private string _camName = "";

        public SetCamGainForm()
        {
            InitializeComponent();
            this.TopMost = true;
            this.ShowInTaskbar = true;
        }
        public SetCamGainForm(string name)
        {
            InitializeComponent();
            this.TopMost = true;
            this.ShowInTaskbar = true;
            this.ReName = name;
            this._camName = name;   
        }
        private void ResetParamForm_Load(object sender, EventArgs e)
        {
            try
            {
               string value = AcqSourceManage.Instance.GetAcqSource(this._camName)?.Sensor?.GetParam("增益").ToString();
                this.textBox1.Text = value;
                int result = 0;
                int.TryParse(this.textBox1.Text, out result);
                this.trackBar1.Value = result;
            }
            catch (Exception ex)
            {
               new Common.UserMessageForm(ex.ToString()).ShowDialog();;
            }
        }

        public SetCamGainForm(string name,string formName)
        {
            InitializeComponent();
            this.ReName = name;
            this.textBox1.Text = name;
            this.Text = formName;
        }

        private void 确定button_Click(object sender, EventArgs e)
        {
            this.Close();
            this.DialogResult = DialogResult.OK;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                ReName = this.textBox1.Text;
                int result = 0;
                int.TryParse(this.textBox1.Text, out result);
                this.trackBar1.Value = result;
                AcqSourceManage.Instance.GetAcqSource(this._camName)?.Sensor?.SetParam("增益", result);
            }
            catch(Exception ex)
            {
               new Common.UserMessageForm(ex.ToString()).ShowDialog();;
            }
        }

        private void 取消Btn_Click(object sender, EventArgs e)
        {
            this.Close();
            this.DialogResult = DialogResult.Cancel;
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            try
            {
                this.textBox1.Text = this.trackBar1.Value.ToString();
                AcqSourceManage.Instance.GetAcqSource(this._camName)?.Sensor?.SetParam("增益", this.textBox1.Text);
            }
            catch(Exception ex)
            {
               new Common.UserMessageForm(ex.ToString()).ShowDialog();; 
            }
        }


    }
}

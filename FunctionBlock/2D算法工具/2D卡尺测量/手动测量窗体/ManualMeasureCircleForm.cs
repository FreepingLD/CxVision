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
using View;

namespace FunctionBlock
{
    public partial class ManualMeasureCircleForm : Form
    {
        private IFunction _function;
        private userDrawCircleROI drawObject;
        private drawPixCircle _pixCircle;
        private drawWcsCircle _wcsCircle;

        public drawPixCircle PixCircle { get => _pixCircle; set => _pixCircle = value; }
        public drawWcsCircle WcsCircle { get => _wcsCircle; set => _wcsCircle = value; }

        public ManualMeasureCircleForm(ImageDataClass imageData, drawPixCircle pixCircle)
        {
            InitializeComponent();
            this.drawObject = new userDrawCircleROI(this.hWindowControl1, false);
            if (imageData.IsInitialized())
            {
                this.drawObject.BackImage = imageData;
                this.drawObject?.SetParam(pixCircle);
                this.drawObject.DrawingGraphicObject();
            }

        }
        private void ManualMeasureCircleForm_Load(object sender, EventArgs e)
        {

        }
        private void BindProperty()
        {
            try
            {

            }
            catch (Exception ex)
            {
                new Common.UserMessageForm(ex.ToString()).ShowDialog();
                //new UserMessageForm().ShowDialog(ex.ToString());
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
        private void AddForm(GroupBox groupBox, Form form)
        {
            if (groupBox == null) return;
            if (groupBox.Controls.Count > 0)
                groupBox.Controls.Clear();
            if (form == null) return;
            form.TopLevel = false;
            form.FormBorderStyle = FormBorderStyle.None;
            form.MaximizeBox = false;
            form.MinimizeBox = false;
            form.Dock = DockStyle.Fill;
            groupBox.Controls.Add(form);
            form.Show();
        }


 

        private void hWindowControl1_MouseMove(object sender, GrayValueInfoEventArgs e)
        {
            //if (e.GaryValue.Length > 0)
            //    this.灰度值1Label.Text = e.GaryValue[0].ToString();
            //else
            //    this.灰度值1Label.Text = 0.ToString();
            /////////////////////////////////////////////
            //if (e.GaryValue.Length > 1)
            //    this.灰度值2Label.Text = e.GaryValue[1].ToString();
            //else
            //    this.灰度值2Label.Text = 0.ToString();
            ///////////////////////////////////////////
            //if (e.GaryValue.Length > 2)
            //    this.灰度值3Label.Text = e.GaryValue[2].ToString();
            //else
            //    this.灰度值3Label.Text = 0.ToString();
            /////////////////////////////////////////////////
            //this.行坐标Label.Text = e.Row.ToString();
            //this.列坐标Label.Text = e.Col.ToString();
        }



        private void ManualMeasureCircleForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (this.drawObject != null)
                    this.drawObject.ClearDrawingObject();
            }
            catch
            {
                new Common.UserMessageForm("关闭窗体报错").ShowDialog();
                //new UserMessageForm().ShowDialog("关闭窗体报错");
            }

        }


        private void 确定button_Click(object sender, EventArgs e)
        {
            try
            {
                this._pixCircle = this.drawObject.GetDrawPixCircleParam();
                this._wcsCircle = this._pixCircle.GetWcsCircle(this.drawObject.CameraParam);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                new Common.UserMessageForm(ex.ToString()).ShowDialog();
                //new UserMessageForm().ShowDialog(ex.ToString());
            }
        }

        private void 取消button_Click(object sender, EventArgs e)
        {
            try
            {
                //this._pixPoint = this.drawObject.GetDrawPixPointParam();
                this.DialogResult = DialogResult.No;
                this.Close();
            }
            catch (Exception ex)
            {
                new Common.UserMessageForm(ex.ToString()).ShowDialog();
                //new UserMessageForm().ShowDialog(ex.ToString());
            }
        }


    }
}

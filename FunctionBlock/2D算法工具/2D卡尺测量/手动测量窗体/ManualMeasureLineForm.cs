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
    public partial class ManualMeasureLineForm : Form
    {
        private IFunction _function;
        private userDrawLineROI drawObject;
        private drawPixLine _pixLine;
        private drawWcsLine _wcsLine;

        public drawPixLine PixLine { get => _pixLine; set => _pixLine = value; }
        public drawWcsLine WcsLine { get => _wcsLine; set => _wcsLine = value; }

        public ManualMeasureLineForm(ImageDataClass imageData, drawPixLine pixLine)
        {
            InitializeComponent();
            //this.Text = text;
            this.drawObject = new userDrawLineROI(this.hWindowControl1, false);
            if (imageData.IsInitialized())
            {
                this.drawObject.BackImage = imageData;
                this.drawObject?.SetParam(pixLine);
                this.drawObject.DrawingGraphicObject();
            }

        }
        private void ManualMeasureLineForm_Load(object sender, EventArgs e)
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
               // new Common.UserMessageForm(ex.ToString()).ShowDialog();
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



        private void ManualMeasureLineForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                //this.DialogResult = DialogResult.OK;
                if (this.drawObject != null)
                    this.drawObject.ClearDrawingObject();
                //this.drawObject.GrayValueInfo -= new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
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
                this._pixLine = this.drawObject.GetDrawPixLineParam();
                this._wcsLine = this._pixLine.GetWcsLine(this.drawObject.CameraParam);
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

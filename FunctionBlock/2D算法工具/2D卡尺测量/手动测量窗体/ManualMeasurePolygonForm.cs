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
    public partial class ManualMeasurePolygonForm : Form
    {
        private IFunction _function;
        private userDrawPolygonROI drawObject;
        private drawPixPolygon _pixPolygon;

        public drawPixPolygon PixPolygon { get => _pixPolygon; set => _pixPolygon = value; }


        public ManualMeasurePolygonForm(ImageDataClass imageData, drawPixPolygon pixPolyLine)
        {
            InitializeComponent();
            this.drawObject = new userDrawPolygonROI(this.hWindowControl1, false);
            if (imageData.IsInitialized())
            {
                this.drawObject.BackImage = imageData;
                this.drawObject?.SetParam(pixPolyLine);
                this.drawObject.DrawingGraphicObject();
            }

        }
        private void ManualMeasurePolygonForm_Load(object sender, EventArgs e)
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



        private void ManualMeasurePolygonForm_FormClosing(object sender, FormClosingEventArgs e)
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
                this._pixPolygon = this.drawObject.GetDrawPixPolygonParam();
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
               // new Common.UserMessageForm(ex.ToString()).ShowDialog();
            }
        }





    }
}

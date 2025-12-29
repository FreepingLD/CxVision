using Common;
using FunctionBlock;
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
    public partial class DrawManualCircleParamForm : Form
    {
        private IFunction _function;
        public DrawingBaseMeasure drawObject, drawBaseObject;
        private Form _form;
        private double x;
        private double y;
        private double _radius;
        private double _startAngle;
        private double _endtAngle;

        public DrawManualCircleParamForm()
        {
            InitializeComponent();
        }
        public DrawManualCircleParamForm(DrawingBaseMeasure drawingBaseClass)
        {
            InitializeComponent();
            this.drawObject = drawingBaseClass;
            this._form = null;
        }
        public DrawManualCircleParamForm(IFunction function, DrawingBaseMeasure drawingClass)
        {
            InitializeComponent();
            this._function = function;
            this.drawObject = drawingClass;
            this._form = null;
        }
        public DrawManualCircleParamForm(IFunction function, Form form)
        {
            InitializeComponent();
            this._function = function;
            this._form = form;
        }

        private void DrawCircleParamForm_Load(object sender, EventArgs e)
        {
            try
            {
                object cen_x = this.drawObject.GetParam("圆心X");
                object cen_y = this.drawObject.GetParam("圆心Y");
                object Radius = this.drawObject.GetParam("半径");
                object StartAngle = this.drawObject.GetParam("起始角");
                object EndAngle = this.drawObject.GetParam("终止角");
                this.圆心XtextBox.Text = cen_x.ToString();
                this.圆心YtextBox.Text = cen_y.ToString();
                this.圆半径texBox.Text = Radius.ToString();
                this.起始角textBox.Text = StartAngle.ToString();
                this.终止角textBox.Text = EndAngle.ToString();
            }
            catch (Exception ex)
            {
                new Common.UserMessageForm(ex.ToString()).ShowDialog();
                //new UserMessageForm().ShowDialog(ex.ToString());
            }
        }



        private void DrawParamForm_FormClosing(object sender, FormClosingEventArgs e)
        {

        }


        private void 圆半径texBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                double result = 0;
                double.TryParse(圆半径texBox.Text, out result);
                this._radius = result;
            }
            catch (Exception ex)
            {
                new Common.UserMessageForm(ex.ToString()).ShowDialog();
                //new UserMessageForm().ShowDialog(ex.ToString());
            }
        }

        private void 起始角textBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                double result = 0;
                double.TryParse(起始角textBox.Text, out result);
                this._startAngle = result;
            }
            catch (Exception ex)
            {
                new Common.UserMessageForm(ex.ToString()).ShowDialog();
                //new UserMessageForm().ShowDialog(ex.ToString());
            }
        }

        private void 终止角textBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                double result = 0;
                double.TryParse(终止角textBox.Text, out result);
                this._endtAngle = result;
            }
            catch (Exception ex)
            {
                new Common.UserMessageForm(ex.ToString()).ShowDialog();
                //new UserMessageForm().ShowDialog(ex.ToString());
            }
        }

        private void 圆心XtextBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                double result = 0;
                double.TryParse(圆心XtextBox.Text, out result);
                this.x = result;
            }
            catch (Exception ex)
            {
                new Common.UserMessageForm(ex.ToString()).ShowDialog();
                //new UserMessageForm().ShowDialog(ex.ToString());
            }
        }

        private void 圆心YtextBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                double result = 0;
                double.TryParse(圆心YtextBox.Text, out result);
                this.y = result;
            }
            catch (Exception ex)
            {
                new Common.UserMessageForm(ex.ToString()).ShowDialog();
                //new UserMessageForm().ShowDialog(ex.ToString());
            }
        }

        private void 确认button_Click(object sender, EventArgs e)
        {
            try
            {
                this.drawObject.SetParam($"圆心X,{this.x}");
                this.drawObject.SetParam($"圆心Y,{this.y}");
                this.drawObject.SetParam($"半径,{this._radius}");
                this.drawObject.SetParam($"起始角,{this._startAngle}");
                this.drawObject.SetParam($"终止角,{this._endtAngle}");
                this.drawObject.SetParam("刷新");
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

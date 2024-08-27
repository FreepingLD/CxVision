using Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using MotionControlCard;
using Sensor;


namespace FunctionBlock
{
    public partial class JogXYMotionControlForm : Form
    {
        IMotionControl i_McCard;
        AutoReSizeFormControls arfc = new AutoReSizeFormControls();
        public JogXYMotionControlForm()
        {
            InitializeComponent();
            this.i_McCard = MotionCardManage.CurrentCard;
        }

        private void JogXYMotionControlForm_Load(object sender, EventArgs e)
        {

        }



        // 正向移动X轴
        private void MoveXAddbutton_MouseDown(object sender, MouseEventArgs e)
        {
            double speed = double.Parse(JOG速度textBox.Text);
            try
            {
                if (i_McCard != null)
                    i_McCard.JogAxisStart(enCoordSysName.CoordSys_1, enAxisName.X轴, speed);
            }
            catch 
            {
            }

        }
        private void MoveXAddbutton_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                if (i_McCard != null)
                    i_McCard.JogAxisStop();
            }
            catch
            { }
        }

        // 负向移动X轴
        private void MoveXminusbutton_MouseDown(object sender, MouseEventArgs e)
        {
            double speed = double.Parse(JOG速度textBox.Text);
            try
            {
                if (i_McCard != null)
                    i_McCard.JogAxisStart(enCoordSysName.CoordSys_1, enAxisName.X轴, -speed);
            }
            catch { }
        }
        private void MoveXminusbutton_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                if(i_McCard!=null)
                i_McCard.JogAxisStop();
            }
            catch
            {

            }
        }

        // 正向移动Y轴
        private void MoveYAddbutton_MouseDown(object sender, MouseEventArgs e)
        {
            double speed = double.Parse(JOG速度textBox.Text);
            try
            {
                if (i_McCard != null)
                    i_McCard.JogAxisStart(enCoordSysName.CoordSys_1, enAxisName.Y轴, speed);
            }
            catch
            { }
        }
        private void MoveYAddbutton_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                if (i_McCard != null)
                    i_McCard.JogAxisStop();
            }
            catch
            { }
        }

        // 负向移动Y轴
        private void MoveYminusbutton_MouseDown(object sender, MouseEventArgs e)
        {
            double speed = double.Parse(JOG速度textBox.Text);
            try
            {
                if (i_McCard != null)
                    i_McCard.JogAxisStart(enCoordSysName.CoordSys_1, enAxisName.Y轴, -speed);
            }
            catch
            {
            }
        }
        private void MoveYminusbutton_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                if (i_McCard != null)
                    i_McCard.JogAxisStop();
            }
            catch
            {

            }
        }
        // 正向移动Z轴
        private void MoveZAddbutton_MouseDown(object sender, MouseEventArgs e)
        {
            double speed = double.Parse(JOG速度textBox.Text);
            try
            {
                if (i_McCard != null)
                    i_McCard.JogAxisStart(enCoordSysName.CoordSys_1, enAxisName.Z轴, speed);
            }
            catch
            { }
        }
        private void MoveZAddbutton_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                if (i_McCard != null)
                    i_McCard.JogAxisStop();
            }
            catch
            {

            }
        }

        // 负向移动Z轴
        private void MoveZminusbutton_MouseDown(object sender, MouseEventArgs e)
        {
            double speed = double.Parse(JOG速度textBox.Text);
            try
            {
                if (i_McCard != null)
                    i_McCard.JogAxisStart(enCoordSysName.CoordSys_1, enAxisName.Z轴, -speed);
            }
            catch
            {

            }
        }
        private void MoveZminusbutton_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                if (i_McCard != null)
                    i_McCard.JogAxisStop();
            }
            catch
            {

            }
        }

        // 窗体关闭时处理
        private void MotionControl_FormClosing(object sender, FormClosingEventArgs e)
        {

        }

        // 尺寸改变
        private void MotionControl_SizeChanged(object sender, EventArgs e)
        {
            arfc.GetScale(this.Width, this.Height);
            arfc.ResetControlSize(this);
        }



       
    }

}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Common;
using HalconDotNet;

namespace FunctionBlock
{
    public partial class DivisionParamForm : Form
    {
        private HTuple camParam;
        public DivisionParamForm()
        {
            InitializeComponent();
        }

        public DivisionParamForm(HTuple camPara)
        {
            InitializeComponent();
            this.camParam = camPara;
            if (camPara == null) return;
            try
            {
                object[] array = camPara.ToOArr();
                this.焦距倍率textBox.Text = array[1].ToString();
                this.畸变textBox.Text = array[2].ToString();
                this.像元宽textBox.Text = array[3].ToString();
                this.像元高textBox.Text = array[4].ToString();
                this.中心点x坐标textBox.Text = array[5].ToString();
                this.中心点y坐标textBox.Text = array[6].ToString();
                this.图像宽textBox.Text = array[7].ToString();
                this.图像高textBox.Text = array[8].ToString();
            }
            catch
            {

            }
        }
        public DivisionParamForm(userCamParam camPara)
        {
            InitializeComponent();
            this.焦距倍率textBox.Text = camPara.Focus.ToString();
            this.畸变textBox.Text = camPara.Kappa.ToString();
            this.像元宽textBox.Text = camPara.Sx.ToString();
            this.像元高textBox.Text = camPara.Sy.ToString();
            this.中心点x坐标textBox.Text = camPara.Cx.ToString();
            this.中心点y坐标textBox.Text = camPara.Cy.ToString();
            this.图像宽textBox.Text = camPara.Width.ToString();
            this.图像高textBox.Text = camPara.Height.ToString();
        }
        private void 焦距textBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                double value;
                double.TryParse(焦距倍率textBox.Text, out value);
                this.camParam[1] = value;
            }
            catch (Exception ex)
            {
                MessageBox.Show("更改焦距值失败" + ex.ToString());
            }
        }

        private void 畸变textBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                double value;
                double.TryParse(畸变textBox.Text, out value);
                this.camParam[2] = value;
            }
            catch (Exception ex)
            {
                MessageBox.Show("更改畸变值失败" + ex.ToString());
            }
        }

        private void 像元宽textBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                double value;
                double.TryParse(像元宽textBox.Text, out value);
                this.camParam[3] = value;
            }
            catch (Exception ex)
            {
                MessageBox.Show("更改像元宽值失败" + ex.ToString());
            }
        }

        private void 像元高textBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                double value;
                double.TryParse(像元高textBox.Text, out value);
                this.camParam[4] = value;
            }
            catch (Exception ex)
            {
                MessageBox.Show("更改像元高值失败" + ex.ToString());
            }
        }

        private void 中心点x坐标textBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                double value;
                double.TryParse(中心点x坐标textBox.Text, out value);
                this.camParam[5] = value;
            }
            catch (Exception ex)
            {
                MessageBox.Show("更改中心点x坐标值失败" + ex.ToString());
            }
        }

        private void 中心点y坐标textBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                double value;
                double.TryParse(中心点y坐标textBox.Text, out value);
                this.camParam[6] = value;
            }
            catch (Exception ex)
            {
                MessageBox.Show("更改中心点y坐标值失败" + ex.ToString());
            }
        }

        private void 图像宽textBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                double value;
                double.TryParse(图像宽textBox.Text, out value);
                this.camParam[7] = value;
            }
            catch (Exception ex)
            {
                MessageBox.Show("更改图像宽值失败" + ex.ToString());
            }
        }

        private void 图像高textBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                double value;
                double.TryParse(图像高textBox.Text, out value);
                this.camParam[8] = value;
            }
            catch (Exception ex)
            {
                MessageBox.Show("更改图像高值失败" + ex.ToString());
            }
        }
    }
}

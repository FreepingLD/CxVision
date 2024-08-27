using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HalconDotNet;

namespace FunctionBlock
{
    public partial class PolynomialParamForm : Form
    {
        private HTuple camParam;
        public PolynomialParamForm()
        {
            InitializeComponent();
        }

        public PolynomialParamForm(HTuple camPara)
        {
            InitializeComponent();
            this.camParam = camPara;
            if (camPara == null) return;
            this.焦距倍率textBox.Text = camPara[0].O.ToString();
            this.K1textBox.Text = camPara[1].O.ToString();
            this.K2textBox.Text = camPara[2].O.ToString();
            this.K3textBox.Text = camPara[3].O.ToString();
            this.P1textBox.Text = camPara[4].O.ToString();
            this.P2textBox.Text = camPara[5].O.ToString();
            this.像元宽textBox.Text = camPara[6].O.ToString();
            this.像元高textBox.Text = camPara[7].O.ToString();
            this.中心点x坐标textBox.Text = camPara[8].O.ToString();
            this.中心点y坐标textBox.Text = camPara[9].O.ToString();
            this.图像宽textBox.Text = camPara[10].O.ToString();
            this.图像高textBox.Text = camPara[11].O.ToString();       
    }
        public PolynomialParamForm(Common.userCamParam camPara)
        {
            InitializeComponent();
            this.焦距倍率textBox.Text = camPara.Focus.ToString();
            //this.K1textBox.Text = camPara.k1.ToString();
            //this.K2textBox.Text = camPara[2].O.ToString();
            //this.K3textBox.Text = camPara[3].O.ToString();
            //this.P1textBox.Text = camPara[4].O.ToString();
            //this.P2textBox.Text = camPara[5].O.ToString();
            //this.像元宽textBox.Text = camPara[6].O.ToString();
            //this.像元高textBox.Text = camPara[7].O.ToString();
            //this.中心点x坐标textBox.Text = camPara[8].O.ToString();
            //this.中心点y坐标textBox.Text = camPara[9].O.ToString();
            //this.图像宽textBox.Text = camPara[10].O.ToString();
            //this.图像高textBox.Text = camPara[11].O.ToString();
        }
        private void 焦距textBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                double value;
                double.TryParse(焦距倍率textBox.Text, out value);
                this.camParam[0] = value;
            }
            catch (Exception ex)
            {
                MessageBox.Show("更改焦距值失败" + ex.ToString());
            }
        }

        private void K1textBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                double value;
                double.TryParse(K1textBox.Text, out value);
                this.camParam[1] = value;
            }
            catch (Exception ex)
            {
                MessageBox.Show("更改K1值失败" + ex.ToString());
            }
        }

        private void K2textBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                double value;
                double.TryParse(K2textBox.Text, out value);
                this.camParam[2] = value;
            }
            catch (Exception ex)
            {
                MessageBox.Show("更改K2值失败" + ex.ToString());
            }
        }

        private void K3textBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                double value;
                double.TryParse(K3textBox.Text, out value);
                this.camParam[3] = value;
            }
            catch (Exception ex)
            {
                MessageBox.Show("更改K3值失败" + ex.ToString());
            }
        }

        private void P1textBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                double value;
                double.TryParse(P1textBox.Text, out value);
                this.camParam[4] = value;
            }
            catch (Exception ex)
            {
                MessageBox.Show("更改P1值失败" + ex.ToString());
            }
        }

        private void P2textBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                double value;
                double.TryParse(P2textBox.Text, out value);
                this.camParam[5] = value;
            }
            catch (Exception ex)
            {
                MessageBox.Show("更改P2值失败" + ex.ToString());
            }
        }

        private void 像元宽textBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                double value;
                double.TryParse(像元宽textBox.Text, out value);
                this.camParam[6] = value;
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
                this.camParam[7] = value;
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
                this.camParam[8] = value;
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
                this.camParam[9] = value;
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
                this.camParam[10] = value;
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
                this.camParam[11] = value;
            }
            catch (Exception ex)
            {
                MessageBox.Show("更改图像高值失败" + ex.ToString());
            }
        }
    }
}

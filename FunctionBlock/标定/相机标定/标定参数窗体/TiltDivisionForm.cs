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
    public partial class TiltDivisionForm : Form
    {
        private HTuple camParam;
        public TiltDivisionForm()
        {
            InitializeComponent();
        }
        public TiltDivisionForm(HTuple camPara)
        {
            InitializeComponent();
            this.camParam = camPara;
            if (camPara == null) return;
            this.焦距倍率textBox.Text = camPara[0].O.ToString();
            this.畸变textBox.Text = camPara[1].O.ToString();
            this.倾斜textBox.Text = camPara[2].O.ToString();
            this.旋转textBox.Text = camPara[3].O.ToString();
            this.像元宽textBox.Text = camPara[4].O.ToString();
            this.像元高textBox.Text = camPara[5].O.ToString();
            this.中心点x坐标textBox.Text = camPara[6].O.ToString();
            this.中心点y坐标textBox.Text = camPara[7].O.ToString();
            this.图像宽textBox.Text = camPara[8].O.ToString();
            this.图像高textBox.Text = camPara[9].O.ToString();
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

        private void 畸变textBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                double value;
                double.TryParse(畸变textBox.Text, out value);
                this.camParam[1] = value;
            }
            catch (Exception ex)
            {
                MessageBox.Show("更改畸变值失败" + ex.ToString());
            }
        }

        private void 倾斜textBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                double value;
                double.TryParse(倾斜textBox.Text, out value);
                this.camParam[2] = value;
            }
            catch (Exception ex)
            {
                MessageBox.Show("更改倾斜值失败" + ex.ToString());
            }
        }

        private void 旋转textBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                double value;
                double.TryParse(旋转textBox.Text, out value);
                this.camParam[3] = value;
            }
            catch (Exception ex)
            {
                MessageBox.Show("更改旋转值失败" + ex.ToString());
            }
        }

        private void 像元宽textBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                double value;
                double.TryParse(像元宽textBox.Text, out value);
                this.camParam[4] = value;
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
                this.camParam[5] = value;
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
                this.camParam[6] = value;
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
                this.camParam[7] = value;
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
                this.camParam[8] = value;
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
                this.camParam[9] = value;
            }
            catch (Exception ex)
            {
                MessageBox.Show("更改图像高值失败" + ex.ToString());
            }
        }
    }
}

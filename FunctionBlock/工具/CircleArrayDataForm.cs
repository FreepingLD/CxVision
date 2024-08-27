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
using HalconDotNet;

namespace FunctionBlock
{
    public partial class CircleArrayDataForm : Form
    {
        private DataGridView dataGridView;
        private DataTable dataTable;
        public double  Ref_X { get; set; }
        public double  Ref_Y { get; set; }
        public double  Add_Deg { get; set; }
        public int ArrayNum { get; set; }
        public double Radius { get; set; }
        public CircleArrayDataForm()
        {
            InitializeComponent();
        }

        public CircleArrayDataForm(DataGridView dataGridView1, DataTable dataTable)
        {
            this.dataTable = dataTable;
            this.dataGridView = dataGridView1;
            InitializeComponent();
        }

        private void CircleArrayCoordPoint(DataTable dataTable, double[] dataPoint, double[] refPoint, double angle, double Num)
        {
            if (dataPoint == null || dataGridView == null) return;
            HTuple homMat2dIdentity, homMat2dRotate, Qx, Qy;
            double origin_x1, origin_y1, origin_x2, origin_y2;
            if (dataPoint.Length <= 6)
            {
                origin_x1 = dataPoint[0];
                origin_y1 = dataPoint[1];
                origin_x2 = dataPoint[0];
                origin_y2 = dataPoint[1];
            }
            else
            {
                origin_x1 = dataPoint[0];
                origin_y1 = dataPoint[1];
                origin_x2 = dataPoint[6];
                origin_y2 = dataPoint[7];
            }
            ////////////////////////////////////////////
            for (int i = 1; i < Num; i++)
            {
                if (dataPoint.Length <= 6)
                {
                    HOperatorSet.HomMat2dIdentity(out homMat2dIdentity);
                    HOperatorSet.HomMat2dRotate(homMat2dIdentity, angle * i * Math.PI / 180, refPoint[0], refPoint[1], out homMat2dRotate);
                    HOperatorSet.AffineTransPoint2d(homMat2dRotate, new HTuple(origin_x1 + refPoint[0]), new HTuple(origin_y1 + refPoint[1]), out Qx, out Qy);
                    //////////////
                    dataPoint[0] = Qx[0].D;
                    dataPoint[1] = Qy[0].D;
                }
                else
                {
                    HOperatorSet.HomMat2dIdentity(out homMat2dIdentity);
                    HOperatorSet.HomMat2dRotate(homMat2dIdentity, angle * i * Math.PI / 180, refPoint[0], refPoint[1], out homMat2dRotate);
                    HOperatorSet.AffineTransPoint2d(homMat2dRotate, new HTuple(origin_x1 + refPoint[0], origin_x2 + refPoint[0]), new HTuple(origin_y1 + refPoint[1], origin_y2 + refPoint[1]), out Qx, out Qy);
                    //////////////
                    dataPoint[0] = Qx[0].D;
                    dataPoint[1] = Qy[0].D;
                    dataPoint[6] = Qx[1].D;
                    dataPoint[7] = Qy[1].D;
                }
                dataTable.Rows.Add(string.Join(",", dataPoint).Split(','));
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                //double offsetAngle, ref_X, ref_Y;
                //int xNum;
                //HTuple homMat2dIdentity, homMat2dRotate, Qx = 0, Qy = 0;
                //xNum = Convert.ToInt32(this.阵列数量numericUpDown.Value);
                //double.TryParse(this.参考点XtextBox.Text, out ref_X);
                //double.TryParse(this.参考点YtextBox.Text, out ref_Y);
                //double.TryParse(this.角度增量textBox.Text, out offsetAngle);
                /////////////////////////////////////
                //for (int i = 0; i < xNum; i++)
                //{
                //    index_x.Clear();
                //    index_y.Clear();
                //    if (i == 0) continue; //选定行不变
                //    ////////////////////////////////////////////
                //    double[] x = new double[index_x.Count];
                //    double[] y = new double[index_y.Count];
                //    for (int ii = 0; ii < index_x.Count; ii++)
                //        x[ii] = coord1Point[index_x[ii]];
                //    for (int ii = 0; ii < index_x.Count; ii++)
                //        y[ii] = coord1Point[index_y[ii]];
                //    HOperatorSet.HomMat2dIdentity(out homMat2dIdentity);
                //    HOperatorSet.HomMat2dRotate(homMat2dIdentity, offsetAngle * i * Math.PI / 180, ref_X, ref_Y, out homMat2dRotate);
                //    HOperatorSet.AffineTransPoint2d(homMat2dRotate, new HTuple(x), new HTuple(y), out Qx, out Qy);
                //    for (int ii = 0; ii < index_x.Count; ii++)
                //        coord1Point[index_x[ii]] = Qx[ii].D;
                //    for (int ii = 0; ii < index_x.Count; ii++)
                //        coord1Point[index_y[ii]] = Qy[ii].D;
                //    dataTable.Rows.Add(string.Join(",", coord1Point).Split(','));
                //    Thread.Sleep(10);
                //}

                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void 参考点XtextBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                double value = 0;
                double.TryParse(this.参考点XtextBox.Text, out value);
                this.Ref_X = value;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void 参考点YtextBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                double value = 0;
                double.TryParse(this.参考点YtextBox.Text, out value);
                this.Ref_Y = value;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void 阵列数量numericUpDown_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                this.ArrayNum = (int)this.阵列数量numericUpDown.Value;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void 角度增量textBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                double value = 0;
                double.TryParse(this.角度增量textBox.Text, out value);
                this.Add_Deg = value;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void 半径textBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                double value = 0;
                double.TryParse(this.半径textBox.Text, out value);
                this.Radius = value;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}

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
    public partial class CircleArrayForm : Form
    {
        private DataGridView dataGridView;
        private DataTable dataTable;

        public CircleArrayForm()
        {
            InitializeComponent();
        }
        public CircleArrayForm(DataGridView dataGridView1, DataTable dataTable)
        {
            this.dataTable = dataTable;
            this.dataGridView = dataGridView1;
            InitializeComponent();
        }

        private void OffsetArrayCoordPoint(DataTable dataTable, double[] dataPoint, double[] refPoint, double angle, double Num)
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
                double[] coord1Point = null;
                if (this.dataGridView.CurrentRow == null) return;
                coord1Point = new double[this.dataGridView.CurrentRow.Cells.Count];
                //for (int i = 0; i < coord1Point.Length; i++)
                //{
                //    coord1Point[i] = Convert.ToDouble(this.dataGridView.CurrentRow.Cells[i].Value);
                //}
                double offsetDist;
                int xNum;
                xNum = Convert.ToInt32(this.偏置数量numericUpDown.Value);
                double.TryParse(this.偏置距离textBox.Text, out offsetDist);
                //////////////////////////////////
                for (int i = 0; i < xNum; i++)
                {
                    if (i == 0) continue; //选定行不变
                    /////////////////////////////////////////////////
                    for (int k = 0; k < this.dataGridView.Columns.Count; k++)
                    {
                        switch (this.dataGridView.Columns[k].HeaderText)
                        {
                            case "X":
                            case "X坐标":
                            case "X1":
                            case "X1坐标":
                            case "X2":
                            case "X2坐标":
                                if (!(this.dataGridView.CurrentRow.Cells[k].Value is DBNull))
                                    coord1Point[k] = Convert.ToDouble(this.dataGridView.CurrentRow.Cells[k].Value) + offsetDist;
                                break;
                            case "Y":
                            case "Y坐标":
                            case "Y1":
                            case "Y1坐标":
                            case "Y2":
                            case "Y2坐标":
                                if (!(this.dataGridView.CurrentRow.Cells[k].Value is DBNull))
                                    coord1Point[k] = Convert.ToDouble(this.dataGridView.CurrentRow.Cells[k].Value) + offsetDist;
                                break;
                            default:
                                if (!(this.dataGridView.CurrentRow.Cells[k].Value is DBNull))
                                    coord1Point[k] = Convert.ToDouble(this.dataGridView.CurrentRow.Cells[k].Value);
                                break;
                        }
                    }
                    ////////////////////////////////////////////
                    dataTable.Rows.Add(string.Join(",", coord1Point).Split(','));
                    Thread.Sleep(10);
                }


                //OffsetArrayCoordPoint(this.dataTable, coord1Point, new double[] { offsetDist, 0 }, 0, xNum);
            }
            catch (Exception e9)
            {
                MessageBox.Show(e9.ToString());
            }
        }


    }
}

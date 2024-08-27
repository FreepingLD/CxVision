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

namespace FunctionBlock
{
    public partial class RectangleArrayDataForm : Form
    {
        private DataGridView dataGridView, dataGridView2;
        private DataTable dataTable, dataTable2;

        public double OffsetX { get; set; }
        public double OffsetY { get; set; }
        public int RowCount { get; set; }
        public int ColCount { get; set; }
        public RectangleArrayDataForm()
        {
            InitializeComponent();
        }
        public RectangleArrayDataForm(DataGridView dataGridView1, DataTable dataTable)
        {
            this.dataTable = dataTable;
            this.dataGridView = dataGridView1;
            InitializeComponent();
        }
        public RectangleArrayDataForm(DataGridView dataGridView1, DataGridView dataGridView2, DataTable dataTable, DataTable dataTable2)
        {
            this.dataTable = dataTable;
            this.dataGridView = dataGridView1;
            this.dataTable2 = dataTable2;
            this.dataGridView2 = dataGridView2;
            InitializeComponent();
        }

        private void X偏移textBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                double value = 0;
                double.TryParse(this.X偏移textBox.Text, out value);
                this.OffsetX = value;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void Y偏移textBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                double value = 0;
                double.TryParse(this.Y偏移textBox.Text, out value);
                this.OffsetY = value;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void X阵列数量numericUpDown_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                this.ColCount = (int)X阵列数量numericUpDown.Value;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void Y阵列数量numericUpDown_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                this.RowCount = (int)Y阵列数量numericUpDown.Value;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void ArrayCoordPoint(DataTable dataTable, double[] dataPoint, double offsetX, double offsetY, int xNum, int yNum)
        {
            if (dataPoint == null || dataTable == null) return;
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
            /////////////////////////////////
            for (int i = 0; i < yNum; i++)
            {
                for (int j = 0; j < xNum; j++)
                {
                    if (i == 0 && j == 0) continue; //选定行不变
                    if (dataPoint.Length <= 6)
                    {
                        dataPoint[0] = origin_x1 + offsetX * j;
                        dataPoint[1] = origin_y1 + offsetY * i;
                    }
                    else
                    {
                        dataPoint[0] = origin_x1 + offsetX * j;
                        dataPoint[1] = origin_y1 + offsetY * i;
                        dataPoint[6] = origin_x2 + offsetX * j;
                        dataPoint[7] = origin_y2 + offsetY * i;
                    }
                    dataTable.Rows.Add(string.Join(",", dataPoint).Split(','));
                    Thread.Sleep(10);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                /////////////////////////////////////////////////
                // 阵列完后关闭窗体
                this.Close();
            }
            catch (Exception e9)
            {
                MessageBox.Show(e9.ToString());
            }
        }
    }
}

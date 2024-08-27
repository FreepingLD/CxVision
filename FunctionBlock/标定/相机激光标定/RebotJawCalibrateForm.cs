using AlgorithmsLibrary;
using Common;
using FunctionBlock;
using HalconDotNet;
using MotionControlCard;
using Sensor;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using View;

namespace FunctionBlock
{

    public partial class RebotJawCalibrateForm : Form
    {
        private string _camAcqSourceName;
        private string _laserAcqSourceName;
        private CancellationTokenSource cts;
        private object _objectDataModel;
        private VisualizeView drawObject; //
        private CameraParam CamParam;
        public RebotJawCalibrateForm()
        {
            InitializeComponent();
            //drawObject = new userDrawPointROI(this.hWindowControl1, false);
        }

        public RebotJawCalibrateForm(CameraParam CamParam)
        {
            InitializeComponent();
            this.CamParam = CamParam;
            //drawObject = new userDrawPointROI(this.hWindowControl1, false);
        }
        private void LaserCameraCalibrateForm_Load(object sender, EventArgs e)
        {
            BindProperty();
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

        private void BindProperty()
        {
            try
            {
                //this.相机采集源comboBox.DataSource = FunctionBlock.AcqSourceManage.Instance.GetAcqSourceName();
                //this.相机采集源comboBox.SelectedItem = AcqSourceManage.Instance.GetAcqSourceName()?.Last();
                this.坐标系名称comboBox.DataSource = Enum.GetValues(typeof(enCoordSysName));
                this.坐标系名称comboBox.SelectedItem = enCoordSysName.CoordSys_0;
                this.夹抓编号comboBox.DataSource = Enum.GetValues(typeof(enRobotJawEnum));
                this.夹抓编号comboBox.SelectedItem = enRobotJawEnum.Glue1;
                ////////////////////////////////////////////////////////
                //this.addContextMenu(this.hWindowControl1);
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
            }
        }

        private void addData(DataGridView dataGridView1, params object[] value) // 这里一定要用object类型，只有这样才可以直接添加到dataGridView1 控件上
        {
            try
            {
                dataGridView1.Rows.Add(value);
                /////////////////////////////////////////////////////////
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    dataGridView1.Rows[i].HeaderCell.Value = i.ToString();
                }
            }
            catch
            {

            }
        }
        private void deletedData(DataGridView dataGridView1)
        {
            if (dataGridView1.CurrentRow == null) return;
            dataGridView1.Rows.Remove(dataGridView1.CurrentRow);
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                dataGridView1.Rows[i].HeaderCell.Value = i.ToString();
            }
        }
        private void clearData(DataGridView dataGridView1)
        {
            dataGridView1.Rows.Clear();
        }

        private void 激光取点button_Click(object sender, EventArgs e)
        {
            CoordSysAxisParam axisParam = new CoordSysAxisParam(this.坐标系名称comboBox.SelectedItem.ToString());
            this.夹抓dataGridView.Rows.Clear();
            addData(this.夹抓dataGridView, axisParam.X, axisParam.Y, axisParam.Z);
        }
        private void 删点激光点button_Click(object sender, EventArgs e)
        {
            deletedData(this.夹抓dataGridView);
        }

        private void 清空激光点button_Click(object sender, EventArgs e)
        {
            clearData(this.夹抓dataGridView);
        }
        #region 窗口右键菜单项
        private void ClearContextMenu(HWindowControl hWindowControl)
        {
            if (hWindowControl.ContextMenuStrip != null)
                hWindowControl.ContextMenuStrip = null;
        }
        private void addContextMenu(HWindowControl hWindowControl)
        {
            if (hWindowControl.ContextMenuStrip != null) return;
            ContextMenuStrip ContextMenuStrip1 = new ContextMenuStrip();
            ContextMenuStrip1.Name = hWindowControl.Name;
            // 添加右键菜单 
            ToolStripItem[] items = new ToolStripMenuItem[]
            {
             new ToolStripMenuItem("自适应图像(Auto)"),
             new ToolStripMenuItem("保存图像(Save)") ,
             new ToolStripMenuItem("保存点云(Save)"),
             new ToolStripMenuItem("清除窗口(Clear)"),
            };

            ContextMenuStrip1.Items.AddRange(items); // 在滑赋值前不能调用 
            ContextMenuStrip1.ItemClicked += new ToolStripItemClickedEventHandler(hWindowControlContextMenuStrip_ItemClicked);
            hWindowControl.ContextMenuStrip = ContextMenuStrip1;
        }
        private void hWindowControlContextMenuStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            string name = e.ClickedItem.Text;
            try
            {
                ((ContextMenuStrip)sender).Close();
                switch (name)
                {
                    case "自适应图像(Auto)":
                        this.drawObject.AutoImage();
                        break;
                    case "保存图像":
                        SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                        saveFileDialog1.Filter = "bmp files (*.bmp)|*.bmp|All files (*.*)|*.*";
                        saveFileDialog1.FilterIndex = 0;
                        saveFileDialog1.ShowDialog();
                        if (this.drawObject.BackImage != null && this.drawObject.BackImage.Image.IsInitialized())
                            this.drawObject.BackImage.Image.WriteImage("bmp", 0, saveFileDialog1.FileName);
                        else
                            MessageBox.Show("图像内容为空");
                        break;
                    case "保存点云":
                        saveFileDialog1 = new SaveFileDialog();
                        saveFileDialog1.Filter = "ply files (*.ply)|*.ply|txt files (*.txt)|*.txt|om3 files (*.om3)|*.om3|stl files (*.stl)|*.stl|obj files (*.obj)|*.obj|dxf files (*.dxf)|*.dxf|All files (*.*)|*.*";
                        saveFileDialog1.FilterIndex = 3;
                        saveFileDialog1.ShowDialog();
                        if (this.drawObject.PointCloudModel3D != null)
                        {
                            HObjectModel3D hObjectModel3D = HObjectModel3D.UnionObjectModel3d(this.drawObject.PointCloudModel3D.ObjectModel3D, "points_surface");
                            hObjectModel3D.WriteObjectModel3d(new FileInfo(saveFileDialog1.FileName).Extension, saveFileDialog1.FileName, new HTuple(), new HTuple());
                            hObjectModel3D.Dispose();
                        }
                        break;
                    case "清除窗口(Clear)":
                        this.drawObject.ClearWindow();
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void hWindowControl1_MouseMove(object sender, GrayValueInfoEventArgs e)
        {
            if (e.GaryValue.Length > 0)
            {
                //this.hWindowControl1.HalconWindow.SetTposition((int)(e.Row), (int)e.Col);
                //this.hWindowControl1.HalconWindow.SetFont("-Consolas-" + 10 + "- *-0-*-*-1-");
                //this.hWindowControl1.HalconWindow.SetColor("red");
            }
        }

        #endregion
        private drawPixPoint pixPoint = null;
        private drawWcsPoint wcsPoint = null;
        private void 相机取点button_Click(object sender, EventArgs e)
        {

            CoordSysAxisParam axisParam = new CoordSysAxisParam(this.坐标系名称comboBox.SelectedItem.ToString());
            this.机器人dataGridView.Rows.Clear();
            addData(this.机器人dataGridView, axisParam.X, axisParam.Y, axisParam.Z);
        }

        private void 清空相机点button_Click(object sender, EventArgs e)
        {
            clearData(this.机器人dataGridView);
        }

        private void 删除相机点button_Click(object sender, EventArgs e)
        {
            deletedData(this.机器人dataGridView);
        }


        private void 保存button_Click(object sender, EventArgs e)
        {
            try
            {
                //AcqSourceManage.Instance.GetAcqSource(this._camAcqSourceName).Sensor.CameraParam.Save();
                RobotJawParaManager.Instance.Save();
                MessageBox.Show("保存成功");
            }
            catch
            {
                MessageBox.Show(new Exception().ToString());
            }
        }

        private void 标定button_Click(object sender, EventArgs e)
        {
            double laserCoord_x = 0;
            double laserCoord_y = 0;
            double laserCoord_z = 0;
            double camCoord_x = 0;
            double camCoord_y = 0;
            double camCoord_z = 0;
            try
            {
                int count = this.夹抓dataGridView.Rows.Count;
                for (int i = 0; i < 1; i++)
                {
                    laserCoord_x = (Convert.ToDouble(this.夹抓dataGridView.Rows[i].Cells[0].Value));
                    laserCoord_y = (Convert.ToDouble(this.夹抓dataGridView.Rows[i].Cells[1].Value));
                    laserCoord_z = (Convert.ToDouble(this.夹抓dataGridView.Rows[i].Cells[2].Value));
                    camCoord_x = (Convert.ToDouble(this.机器人dataGridView.Rows[i].Cells[0].Value));
                    camCoord_y = (Convert.ToDouble(this.机器人dataGridView.Rows[i].Cells[1].Value));
                    camCoord_z = (Convert.ToDouble(this.机器人dataGridView.Rows[i].Cells[2].Value));
                }
                string jawName = "";
                if (this.夹抓编号comboBox.SelectedItem == null) return;
                jawName = this.夹抓编号comboBox.SelectedItem.ToString();
                RobotJawParam jawParam = RobotJawParaManager.Instance.GetJawParam(jawName);
                jawParam.X = camCoord_x - laserCoord_x;
                jawParam.Y = camCoord_y - laserCoord_y;
                /////////////////////////////////////////////////
                this.dataGridView1.Rows.Clear();
                addData(this.dataGridView1, jawParam.X, jawParam.Y, 0);
            }
            catch (Exception ex)
            {
                MessageBox.Show("标定报错" + ex.ToString());
            }
        }

        private void CameraGlueGunCalibrateForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (cts != null)
                cts.Cancel();
        }


    private void 保存button_Click_1(object sender, EventArgs e)
    {
        try
        {
            RobotJawParaManager.Instance.Save();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.ToString());
        }
    }

    private void 加载参数button_Click_1(object sender, EventArgs e)
    {
        try
        {
            RobotJawParam jawParam = RobotJawParaManager.Instance.GetJawParam(this.夹抓编号comboBox.Text);
            this.dataGridView1.Rows.Clear();
            addData(this.dataGridView1, jawParam.X, jawParam.Y, 0);
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.ToString());
        }
    }



}
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FunctionBlock;
using HalconDotNet;
using System.Threading;
using Common;
using MotionControlCard;
using Sensor;
using View;

namespace FunctionBlock
{
    public partial class LinearCalibrateForm : Form
    {
        private AcqSource _acqSource;
        private CancellationTokenSource cts;
        private VisualizeView drawObject;
        private List<double> listStdValue_x = new List<double>();
        private List<double> listStdValue_y = new List<double>();
        private List<double> listStdValue_z = new List<double>();
        private List<double> listStdValue_theta = new List<double>();
        private List<double> listCurrentValue_x = new List<double>();
        private List<double> listCurrentValue_y = new List<double>();
        private List<double> listCurrentValue_z = new List<double>();
        private List<double> listCurrentValue_theta = new List<double>();

        private Dictionary<double, XyValuePairs> dic_x = new Dictionary<double, XyValuePairs>();
        private Dictionary<double, XyValuePairs> dic_y = new Dictionary<double, XyValuePairs>();
        public LinearCalibrateForm()
        {
            InitializeComponent();
            drawObject = new VisualizeView(this.hWindowControl1);
        }

        private void LinearCalibrateForm_Load(object sender, EventArgs e)
        {
            //SensorBase.ImageAcqComplete += new ImageAcqCompleteEventHandler(ImageAcqComplete_Event);
            InitTable();
            BindProperty();
            this.校准轴comboBox.SelectedIndex = 0; // 默认补偿X轴
            AddForm(运动pane2, new JogXYMotionControlForm());
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
        private void addCrossLine(int width, int height)
        {
            HXLDCont line1 = new HXLDCont(new HTuple(0, height), new HTuple(width * 0.5, width * 0.5));
            HXLDCont line2 = new HXLDCont(new HTuple(height * 0.5, height * 0.5), new HTuple(0, width));
            this.drawObject.AttachPropertyData.Add(line1);
            this.drawObject.AttachPropertyData.Add(line2);
        }
        private void InitTable()
        {
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.RowHeadersWidth = 90;
            this.dataGridView1.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.dataGridView1.RowHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.dataGridView1.TopLeftHeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.dataGridView1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            ///////////////////////////////////////
            for (int i = 0; i < 100; i++)
            {
                this.dataGridView1.Columns.Add("column" + i.ToString(), "Data" + i.ToString());
            }
        }

        private void ImageAcqComplete_Event(ImageAcqCompleteEventArgs e)
        {
            double x, y, z;
            if (MotionCardManage.CurrentCard != null)
            {
                MotionCardManage.CurrentCard.GetAxisPosition(MotionCardManage.CurrentCoordSys, enAxisName.X轴, out x);
                MotionCardManage.CurrentCard.GetAxisPosition(MotionCardManage.CurrentCoordSys, enAxisName.Y轴, out y);
                MotionCardManage.CurrentCard.GetAxisPosition(MotionCardManage.CurrentCoordSys, enAxisName.Z轴, out z);
                e.ImageData.Grab_X = x;
                e.ImageData.Grab_Y = y;
                e.ImageData.Grab_Z = z;
                this.drawObject.BackImage = e.ImageData;
                addCrossLine(e.ImageData.Width, e.ImageData.Height);
            }
            else
            {
                this.drawObject.BackImage = e.ImageData;
                addCrossLine(e.ImageData.Width, e.ImageData.Height);
            }

        }
        private void 相机comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.坐标系comboBox.SelectedItem == null) return;
            //this._acqSource.Sensor = (ISensor)this.相机comboBox.SelectedItem;
        }

        private void BindProperty()
        {
            try
            {
                this.坐标系comboBox.DataSource = Enum.GetValues(typeof(enCoordSysName));
                this.传感器comboBox.Items.Clear();
                this.传感器comboBox.Items.AddRange(AcqSourceManage.Instance.GetAcqSourceName());
            }
            catch (Exception ex)
            {

            }
        }

        private void LinearCalibrateForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                //SensorBase.ImageAcqComplete -= new ImageAcqCompleteEventHandler(ImageAcqComplete_Event);
                if (this.cts != null)
                    this.cts.Cancel();
            }
            catch
            {

            }
        }

        private void 添加位置button_Click(object sender, EventArgs e)
        {
            if (this._acqSource == null) return;
            try
            {
                double x, y, z, theta;
                enCoordSysName coordSysName = enCoordSysName.CoordSys_0;
                if (this.坐标系comboBox.SelectedItem == null) return;
                coordSysName = (enCoordSysName)this.坐标系comboBox.SelectedItem;
                IMotionControl _card = MotionCardManage.GetCardByCoordSysName(this.坐标系comboBox.SelectedItem.ToString());
                _card.GetAxisPosition(coordSysName, enAxisName.X轴, out x);
                _card.GetAxisPosition(coordSysName, enAxisName.Y轴, out y);
                _card.GetAxisPosition(coordSysName, enAxisName.Z轴, out z);
                _card.GetAxisPosition(coordSysName, enAxisName.Z轴, out theta);
                ///////////////////////////
                double stdValue;
                double.TryParse(this.当前标准位置textBox.Text, out stdValue);
                //////////////////////////////////////////////////////////////
                switch (this.校准轴comboBox.SelectedItem.ToString())
                {
                    case "X轴":
                        this.listCurrentValue_x.Add(x);
                        this.listStdValue_x.Add(this.listCurrentValue_x[0] + stdValue);
                        if (this.dataGridView1.Columns.Count == 0) return;
                        this.dataGridView1.Rows.Clear();
                        this.dataGridView1.Rows.Add(string.Join(",", this.listStdValue_x.ToArray()));
                        this.dataGridView1.Rows.Add(string.Join(",", this.listCurrentValue_x.ToArray()));
                        this.dataGridView1.Rows[0].HeaderCell.Value = "标准值";
                        this.dataGridView1.Rows[1].HeaderCell.Value = "机台坐标";
                        //////////////////////////////////////////////
                        //this._calibrationXyPlane.Calibrate_X.Add(x, new XyValuePairs(this.listCurrentValue_x.ToArray(), this.listStdValue_x.ToArray()));
                        break;
                    case "Y轴":
                        this.listCurrentValue_y.Add(y);
                        this.listStdValue_y.Add(this.listCurrentValue_y[0] + stdValue);
                        if (this.dataGridView1.Columns.Count == 0) return;
                        this.dataGridView1.Rows.Clear();
                        this.dataGridView1.Rows.Add(string.Join(",", this.listStdValue_y.ToArray()));
                        this.dataGridView1.Rows.Add(string.Join(",", this.listCurrentValue_y.ToArray()));
                        this.dataGridView1.Rows[0].HeaderCell.Value = "标准值";
                        this.dataGridView1.Rows[1].HeaderCell.Value = "机台坐标";
                        /////////////////////////////////////////////////
                        //this._calibrationXyPlane.Calibrate_Y.Add(y, new XyValuePairs(this.listCurrentValue_y.ToArray(), this.listStdValue_y.ToArray()));
                        break;
                    case "Z轴":
                        this.listCurrentValue_z.Add(z);
                        this.listStdValue_z.Add(this.listCurrentValue_z[0] + stdValue);
                        if (this.dataGridView1.Columns.Count == 0) return;
                        this.dataGridView1.Rows.Clear();
                        this.dataGridView1.Rows.Add(string.Join(",", this.listStdValue_z.ToArray()));
                        this.dataGridView1.Rows.Add(string.Join(",", this.listCurrentValue_z.ToArray()));
                        this.dataGridView1.Rows[0].HeaderCell.Value = "标准值";
                        this.dataGridView1.Rows[1].HeaderCell.Value = "机台坐标";
                        ///////////////////////////////////////////////////////////
                        //this._calibrationXyPlane.Calibrate_Z.Add(z, new XyValuePairs(this.listCurrentValue_z.ToArray(), this.listStdValue_z.ToArray()));
                        break;
                    case "Theta轴":
                        this.listCurrentValue_theta.Add(theta);
                        this.listStdValue_theta.Add(this.listCurrentValue_theta[0] + stdValue);
                        if (this.dataGridView1.Columns.Count == 0) return;
                        this.dataGridView1.Rows.Clear();
                        this.dataGridView1.Rows.Add(string.Join(",", this.listStdValue_theta.ToArray()));
                        this.dataGridView1.Rows.Add(string.Join(",", this.listCurrentValue_theta.ToArray()));
                        this.dataGridView1.Rows[0].HeaderCell.Value = "标准值";
                        this.dataGridView1.Rows[1].HeaderCell.Value = "机台坐标";
                        ////////////////////////////////////////////////////////////
                        //this._calibrationXyPlane.Calibrate_Theta.Add(theta, new XyValuePairs(this.listCurrentValue_theta.ToArray(), this.listStdValue_theta.ToArray()));
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void 删除位置button_Click(object sender, EventArgs e)
        {
            try
            {
                int index = this.dataGridView1.CurrentCell.ColumnIndex;
                if (index >= 0)
                {
                    switch (this.校准轴comboBox.SelectedItem.ToString())
                    {
                        case "X轴":
                            this.listCurrentValue_x.RemoveAt(index);
                            this.listStdValue_x.RemoveAt(index);
                            this.dataGridView1.Rows.Clear();
                            this.dataGridView1.Rows.Add(string.Join(",", this.listStdValue_x.ToArray()));
                            this.dataGridView1.Rows.Add(string.Join(",", this.listCurrentValue_x.ToArray()));
                            this.dataGridView1.Rows[0].HeaderCell.Value = "标准值";
                            this.dataGridView1.Rows[1].HeaderCell.Value = "机台坐标";
                            break;
                        case "Y轴":
                            this.listCurrentValue_y.RemoveAt(index);
                            this.listStdValue_y.RemoveAt(index);
                            this.dataGridView1.Rows.Clear();
                            this.dataGridView1.Rows.Add(string.Join(",", this.listStdValue_y.ToArray()));
                            this.dataGridView1.Rows.Add(string.Join(",", this.listCurrentValue_y.ToArray()));
                            this.dataGridView1.Rows[0].HeaderCell.Value = "标准值";
                            this.dataGridView1.Rows[1].HeaderCell.Value = "机台坐标";
                            break;
                        case "Z轴":
                            this.listCurrentValue_z.RemoveAt(index);
                            this.listStdValue_z.RemoveAt(index);
                            this.dataGridView1.Rows.Clear();
                            this.dataGridView1.Rows.Add(string.Join(",", this.listStdValue_z.ToArray()));
                            this.dataGridView1.Rows.Add(string.Join(",", this.listCurrentValue_z.ToArray()));
                            this.dataGridView1.Rows[0].HeaderCell.Value = "标准值";
                            this.dataGridView1.Rows[1].HeaderCell.Value = "机台坐标";
                            break;
                        case "Theta轴":
                            this.listCurrentValue_theta.RemoveAt(index);
                            this.listStdValue_theta.RemoveAt(index);
                            this.dataGridView1.Rows.Clear();
                            this.dataGridView1.Rows.Add(string.Join(",", this.listStdValue_theta.ToArray()));
                            this.dataGridView1.Rows.Add(string.Join(",", this.listCurrentValue_theta.ToArray()));
                            this.dataGridView1.Rows[0].HeaderCell.Value = "标准值";
                            this.dataGridView1.Rows[1].HeaderCell.Value = "机台坐标";
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void 清空button_Click(object sender, EventArgs e)
        {
            try
            {
                switch (this.校准轴comboBox.SelectedItem.ToString())
                {
                    case "X轴":
                        this.dataGridView1.Rows.Clear();
                        this.listStdValue_x.Clear();
                        this.listCurrentValue_x.Clear();
                        break;
                    case "Y轴":
                        this.dataGridView1.Rows.Clear();
                        this.listStdValue_y.Clear();
                        this.listCurrentValue_y.Clear();
                        break;
                    case "Z轴":
                        this.dataGridView1.Rows.Clear();
                        this.listStdValue_z.Clear();
                        this.listCurrentValue_z.Clear();
                        break;
                    case "Theta轴":
                        this.dataGridView1.Rows.Clear();
                        this.listStdValue_theta.Clear();
                        this.listCurrentValue_theta.Clear();
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private AxisCalibration _calibrationXyPlane = new AxisCalibration();
        private void 保存button_Click(object sender, EventArgs e)
        {
            try
            {
                double x = 0, y = 0, z = 0, theta = 0;
                enCoordSysName coordSysName = enCoordSysName.CoordSys_0;
                if (this.坐标系comboBox.SelectedItem == null) return;
                coordSysName = (enCoordSysName)this.坐标系comboBox.SelectedItem;
                IMotionControl _card = MotionCardManage.GetCardByCoordSysName(this.坐标系comboBox.SelectedItem.ToString());
                _card.GetAxisPosition(coordSysName, enAxisName.X轴, out x);
                _card.GetAxisPosition(coordSysName, enAxisName.Y轴, out y);
                _card.GetAxisPosition(coordSysName, enAxisName.Z轴, out z);
                _card.GetAxisPosition(coordSysName, enAxisName.Z轴, out theta);
                switch (this.校准轴comboBox.SelectedItem.ToString())
                {
                    case "X轴":
                        this._calibrationXyPlane.Calibrate_X.Add(y, new XyValuePairs(this.listCurrentValue_x.ToArray(), this.listStdValue_x.ToArray()));// 校准X轴用y作索引，是为了实现多段校准
                        break;
                    case "Y轴":
                        this._calibrationXyPlane.Calibrate_Y.Add(x, new XyValuePairs(this.listCurrentValue_y.ToArray(), this.listStdValue_y.ToArray())); // 校准Y轴用x作索引，是为了实现多段校准
                        break;
                    case "Z轴":
                        this._calibrationXyPlane.Calibrate_Z.Add(z, new XyValuePairs(this.listCurrentValue_z.ToArray(), this.listStdValue_z.ToArray()));
                        break;
                    case "Theta轴":
                        this._calibrationXyPlane.Calibrate_Theta.Add(theta, new XyValuePairs(this.listCurrentValue_theta.ToArray(), this.listStdValue_theta.ToArray()));
                        break;
                }
                string path = Application.StartupPath + "\\" + "机台校准" + "\\" + MotionCardManage.CurrentCard.Name;
                this._calibrationXyPlane.Save(path);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void 校准轴comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.dataGridView1.Columns.Count == 0) return;
                this.dataGridView1.Rows.Clear();
                switch (this.校准轴comboBox.SelectedItem.ToString())
                {
                    case "X轴":
                        this.dataGridView1.Rows.Add(this.listStdValue_x.ToArray());
                        this.dataGridView1.Rows.Add(this.listCurrentValue_x.ToArray());
                        break;
                    case "Y轴":
                        this.dataGridView1.Rows.Add(this.listStdValue_y.ToArray());
                        this.dataGridView1.Rows.Add(this.listCurrentValue_y.ToArray());
                        break;
                    case "Z轴":
                        this.dataGridView1.Rows.Add(this.listStdValue_z.ToArray());
                        this.dataGridView1.Rows.Add(this.listCurrentValue_z.ToArray());
                        break;
                    case "Theta轴":
                        this.dataGridView1.Rows.Add(this.listStdValue_theta.ToArray());
                        this.dataGridView1.Rows.Add(this.listStdValue_theta.ToArray());
                        break;
                }
                this.dataGridView1.Rows[0].HeaderCell.Value = "标准值";
                this.dataGridView1.Rows[1].HeaderCell.Value = "机台坐标";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }


        private HXLDCont GenCrossLine(HImage hImage)
        {
            HXLDCont hXLDCont = new HXLDCont();
            if (hImage != null && hImage.IsInitialized())
            {
                hXLDCont.GenEmptyObj();
                int width, height;
                hImage.GetImageSize(out width, out height);
                hXLDCont = hXLDCont.ConcatObj(new HXLDCont(new HTuple(height * 0.5, height * 0.5), new HTuple(0, width)));
                hXLDCont = hXLDCont.ConcatObj(new HXLDCont(new HTuple(0, height), new HTuple(width * 0.5, width * 0.5)));
            }
            return hXLDCont;
        }
        private CancellationTokenSource cts2;
        private void 实时采集checkBox_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                switch (this.实时采集checkBox.CheckState)
                {
                    case CheckState.Checked:
                        this.实时采集checkBox.BackColor = Color.Red;
                        cts2 = new CancellationTokenSource();
                        Dictionary<enDataItem, object> data;
                        this._acqSource = AcqSourceManage.Instance.GetAcqSource(this.传感器comboBox.SelectedItem.ToString());
                        Task.Run(() =>
                        {
                            this.drawObject.IsLiveState = true;
                            while (!cts2.IsCancellationRequested)
                            {
                                if (this._acqSource == null) return;
                                data = this._acqSource?.AcqPointData();
                                switch (this._acqSource?.Sensor.ConfigParam.SensorType)
                                {
                                    case enUserSensorType.点激光:
                                        break;
                                    case enUserSensorType.线激光:
                                        break;
                                    case enUserSensorType.面激光:
                                        break;
                                    case enUserSensorType.面阵相机:
                                        if (data?.Count > 0)
                                        {
                                            double[] axisPose = this._acqSource.GetAxisPosition();
                                            this.drawObject.BackImage = (ImageDataClass)data[enDataItem.Image];
                                            this.drawObject.BackImage.Grab_X = axisPose[0];
                                            this.drawObject.BackImage.Grab_Y = axisPose[1];
                                            this.drawObject.BackImage.Grab_Z = axisPose[2];
                                            this.drawObject.BackImage.Grab_Theta = axisPose[3];
                                            this.drawObject.AttachPropertyData.Clear();
                                            this.drawObject.AttachPropertyData.Add((this.GenCrossLine(this.drawObject.BackImage.Image)));
                                        }
                                        break;
                                }
                                Thread.Sleep(20);
                            }
                            this.drawObject.IsLiveState = false;
                        });
                        break;
                    default:
                        this.cts2.Cancel();
                        this.实时采集checkBox.BackColor = Color.Lime;
                        break;
                }
            }
            catch
            {

            }
        }
    }

}


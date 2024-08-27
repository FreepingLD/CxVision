using Common;
using HalconDotNet;
using MotionControlCard;
using Sensor;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using View;

namespace FunctionBlock
{
    public partial class Cam9PointCalibrateForm : Form
    {
        private Dictionary<string, object> listData = new Dictionary<string, object>();
        private DrawingBaseMeasure drawObject;
        private MetrolegyParamForm metrolegyParamForm;
        private TreeViewWrapClass _treeViewWrapClassNPoint;
        //private TreeViewWrapClass _treeViewWrapClassRotate;
        //private string path = @"标定程序";
        private CalibCoordConfigParamManager calibCoordConfigParamRota, calibCoordConfigParamNPoint, calibCoordConfigParamRotaV, calibCoordConfigParamRotaU;
        private BindingList<userPixPoint> CalibRotaPixList, CalibNPointPixList;
        private BindingList<userWcsPoint> CalibRotaWcsList;
        private BindingList<userWcsPoint> CalibNPointWcsList;
        private BindingList<CoordSysAxisParam> listGrabPoint = new BindingList<CoordSysAxisParam>();
        private BindingList<CoordSysAxisParam> listGrabTheta = new BindingList<CoordSysAxisParam>();
        private enCaliStateEnum CurCaliStateEnum = enCaliStateEnum.RotCali;
        private IFunction _currFunction;
        private CameraParam CamParam;
        private ImageDataClass CurrentImageData;
        private string programPath = "标定程序\\9点标定"; // 程序文件路径
        private bool isStop = false;
        private bool isLoad = false;
        public Cam9PointCalibrateForm(CameraParam CamParam)
        {
            InitializeComponent();
            this.CamParam = CamParam;
            this._treeViewWrapClassNPoint = new TreeViewWrapClass(this.N点treeView, this);
            //this._treeViewWrapClassRotate = new TreeViewWrapClass(this.旋转treeView, this);
            this.calibCoordConfigParamRota = new CalibCoordConfigParamManager();
            this.calibCoordConfigParamRotaV = new CalibCoordConfigParamManager();
            this.calibCoordConfigParamNPoint = new CalibCoordConfigParamManager();
            this.calibCoordConfigParamRotaU = new CalibCoordConfigParamManager();
            this.calibCoordConfigParamRota.Read(programPath + "\\" + this.CamParam.SensorName + "\\" + "RotaCalibConfigParam.xml");
            this.calibCoordConfigParamRotaV.Read(programPath + "\\" + this.CamParam.SensorName + "\\" + "RotaCalibConfigParamVAxis.xml");
            this.calibCoordConfigParamRotaU.Read(programPath + "\\" + this.CamParam.SensorName + "\\" + "RotaCalibConfigParamUAxis.xml");
            this.calibCoordConfigParamNPoint.Read(programPath + "\\" + this.CamParam.SensorName + "\\" + "NPointCalibConfigParam.xml");
            this.C轴旋转坐标dataGridView.DataSource = calibCoordConfigParamRota.CalibCoordParamList;
            this.V轴旋转dataGridView.DataSource = calibCoordConfigParamRotaV.CalibCoordParamList;
            this.U轴旋转dataGridView.DataSource = calibCoordConfigParamRotaU.CalibCoordParamList;
            this.N点坐标dataGridView.DataSource = calibCoordConfigParamNPoint.CalibCoordParamList;
            //this.PlCdataGridView.DataSource = calibCoordConfigParamRota.WriteAdress;

            this.drawObject = new DrawingBaseMeasure(this.hWindowControl1, false);
            BindProperty();
            this.CalibRotaPixList = new BindingList<userPixPoint>();
            this.CalibNPointPixList = new BindingList<userPixPoint>();
            this.CalibRotaWcsList = new BindingList<userWcsPoint>();
            this.CalibNPointWcsList = new BindingList<userWcsPoint>();
        }
        private void Cam9PointCalibrateForm_Load(object sender, EventArgs e)
        {
            // 注册事件
            this.addContextMenu(this.C轴旋转坐标dataGridView);
            this.addContextMenu(this.U轴旋转dataGridView);
            this.addContextMenu(this.V轴旋转dataGridView);
            this.addContextMenu(this.N点坐标dataGridView);
            this.addContextMenu(this.hWindowControl1);
            this.drawObject.GrayValueInfo += new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
            BaseFunction.ExcuteCompleted += new ExcuteCompletedEventHandler(DisplayExcuteResult);
            TreeViewWrapClass.ClickNode += new ClickNodeEventHandler(this.DisplayClickObject);
            this.metrolegyParamForm = new MetrolegyParamForm(this.drawObject);
            this._treeViewWrapClassNPoint.OpenProgram(this.programPath + "\\" + this.CamParam.SensorName);
            this.isLoad = true;

            this.AddForm(this.元素tabPage, new ElementViewForm());
        }
        private void BindProperty()
        {
            try
            {
                this.标定平面comboBox.DataSource = Enum.GetValues(typeof(enCalibPlane));
                this.标定方法comboBox.DataSource = Enum.GetValues(typeof(enCalculateMethod));
                this.标定轴comboBox.DataSource = Enum.GetValues(typeof(enCalibAxis)); // 这里不需要更改本标定坐标系，只需要显示即可
                this.运控平台comboBox.DataSource = Enum.GetValues(typeof(enMoveStage));
                this.标定轴comboBox.DataBindings.Add(nameof(this.标定轴comboBox.Text), this.CamParam.CaliParam, nameof(this.CamParam.CaliParam.CalibAxis), true, DataSourceUpdateMode.OnPropertyChanged);
                //this.取反轴comboBox.DataSource = Enum.GetValues(typeof(enInvertAxis)); // 这里不需要更改本标定坐标系，只需要显示即可
                //this.取反轴comboBox.DataBindings.Add(nameof(this.取反轴comboBox.Text), this.CamParam.CaliParam, nameof(this.CamParam.CaliParam.InvertAxis), true, DataSourceUpdateMode.OnPropertyChanged);
                this.坐标值类型comboBox.DataSource = Enum.GetValues(typeof(enCoordValueType)); // 这里不需要更改本标定坐标系，只需要显示即可
                this.坐标值类型comboBox.DataBindings.Add(nameof(this.坐标值类型comboBox.Text), this.CamParam.CaliParam, nameof(this.CamParam.CaliParam.CoordValueType), true, DataSourceUpdateMode.OnPropertyChanged);
                this.标定方法comboBox.DataBindings.Add(nameof(this.标定方法comboBox.Text), this.CamParam.CaliParam, nameof(this.CamParam.CaliParam.CalculateMethod), true, DataSourceUpdateMode.OnPropertyChanged);
                this.角度范围numericUpDown.DataBindings.Add(nameof(this.角度范围numericUpDown.Value), this.CamParam.CaliParam, nameof(this.CamParam.CaliParam.AngleRange), true, DataSourceUpdateMode.OnPropertyChanged);
                this.旋转步数numericUpDown.DataBindings.Add(nameof(this.旋转步数numericUpDown.Value), this.CamParam.CaliParam, nameof(this.CamParam.CaliParam.AngleStep), true, DataSourceUpdateMode.OnPropertyChanged);
                this.旋转方向comboBox.DataBindings.Add(nameof(this.旋转方向comboBox.Text), this.CamParam.CaliParam, nameof(this.CamParam.CaliParam.RotateDirection), true, DataSourceUpdateMode.OnPropertyChanged);
                this.运控平台comboBox.DataBindings.Add(nameof(this.运控平台comboBox.Text), this.CamParam.CaliParam, nameof(this.CamParam.CaliParam.MoveStage), true, DataSourceUpdateMode.OnPropertyChanged);
                this.V轴角度范围numericUpDown.DataBindings.Add(nameof(this.V轴角度范围numericUpDown.Value), this.CamParam.CaliParam, nameof(this.CamParam.CaliParam.AngleRangeAxisV), true, DataSourceUpdateMode.OnPropertyChanged);
                this.V轴旋转步数numericUpDown.DataBindings.Add(nameof(this.V轴旋转步数numericUpDown.Value), this.CamParam.CaliParam, nameof(this.CamParam.CaliParam.AngleStepAxisV), true, DataSourceUpdateMode.OnPropertyChanged);
                this.V轴旋转方向comboBox.DataBindings.Add(nameof(this.V轴旋转方向comboBox.Text), this.CamParam.CaliParam, nameof(this.CamParam.CaliParam.RotateDirectionAxisV), true, DataSourceUpdateMode.OnPropertyChanged);
                this.U轴角度范围numericUpDown.DataBindings.Add(nameof(this.U轴角度范围numericUpDown.Value), this.CamParam.CaliParam, nameof(this.CamParam.CaliParam.AngleRangeAxisU), true, DataSourceUpdateMode.OnPropertyChanged);
                this.U轴旋转步数numericUpDown.DataBindings.Add(nameof(this.U轴旋转步数numericUpDown.Value), this.CamParam.CaliParam, nameof(this.CamParam.CaliParam.AngleStepAxisU), true, DataSourceUpdateMode.OnPropertyChanged);
                this.U轴旋转方向comboBox.DataBindings.Add(nameof(this.U轴旋转方向comboBox.Text), this.CamParam.CaliParam, nameof(this.CamParam.CaliParam.RotateDirectionAxisU), true, DataSourceUpdateMode.OnPropertyChanged);
                this.标定平面comboBox.DataBindings.Add(nameof(this.标定平面comboBox.Text), this.CamParam.CaliParam, nameof(this.CamParam.CaliParam.CalibPlane), true, DataSourceUpdateMode.OnPropertyChanged);
                this.取反X轴checkBox.DataBindings.Add(nameof(this.取反X轴checkBox.Checked), this.CamParam.CaliParam, nameof(this.CamParam.CaliParam.InvertX), true, DataSourceUpdateMode.OnPropertyChanged);
                this.取反Y轴checkBox.DataBindings.Add(nameof(this.取反Y轴checkBox.Checked), this.CamParam.CaliParam, nameof(this.CamParam.CaliParam.InvertY), true, DataSourceUpdateMode.OnPropertyChanged);
                this.取反Z轴checkBox.DataBindings.Add(nameof(this.取反Z轴checkBox.Checked), this.CamParam.CaliParam, nameof(this.CamParam.CaliParam.InvertZ), true, DataSourceUpdateMode.OnPropertyChanged);
                ////////// 通信配置/////////////////
                if (this.CamParam.CaliParam.RotateCalibPoint == null)
                    this.CamParam.CaliParam.RotateCalibPoint = new userWcsVector();
                this.旋转中心位置坐标textBox.Text = "X:" + this.CamParam.CaliParam.RotateCalibPoint.X.ToString("f3") + "   Y:" +
                this.CamParam.CaliParam.RotateCalibPoint.Y.ToString("f3") + "   Z:" + this.CamParam.CaliParam.RotateCalibPoint.Z.ToString("f3") + "   Theta:" + this.CamParam.CaliParam.RotateCalibPoint.Angle.ToString("f3")
                + "   U:" + this.CamParam.CaliParam.RotateCalibPoint.Angle_x.ToString("f4") + "   V:" + this.CamParam.CaliParam.RotateCalibPoint.Angle_y.ToString("f4");
                /////////////////////////////////////////////////////////////////
                if (this.CamParam.CaliParam.StartCaliPoint == null)
                    this.CamParam.CaliParam.StartCaliPoint = new userWcsVector();
                //userWcsVector wcsVector = this.CamParam.NPointCalibParam.StartCaliPoint;
                this.起始点textBox.Text = "X:" + this.CamParam.CaliParam.StartCaliPoint.X.ToString("f3") +
                    "   Y:" + this.CamParam.CaliParam.StartCaliPoint.Y.ToString("f3") +
                    "   Z:" + this.CamParam.CaliParam.StartCaliPoint.Z.ToString("f4") +
                    "   Theta:" + this.CamParam.CaliParam.StartCaliPoint.Angle.ToString("f4");
                ////////////////////////////////////////////////////////////////////////
                if (this.CamParam.CaliParam.EndCalibPoint == null)
                    this.CamParam.CaliParam.EndCalibPoint = new userWcsVector();
                this.终止点textBox.Text = "X:" + this.CamParam.CaliParam.EndCalibPoint.X.ToString("f3") +
                                          "   Y:" + this.CamParam.CaliParam.EndCalibPoint.Y.ToString("f3") +
                                          "   Z:" + this.CamParam.CaliParam.EndCalibPoint.Z.ToString("f4") +
                                          "   Theta:" + this.CamParam.CaliParam.EndCalibPoint.Angle.ToString("f4");
                ////////////////////////////////////////////////////////////////////////
                this.相机轴坐标textBox.Text = "X:" + this.CamParam.CaliParam.CamAxisCoord.X.ToString("f3") +
                     "   Y:" + this.CamParam.CaliParam.CamAxisCoord.Y.ToString("f3") + "   Z:" + this.CamParam.CaliParam.CamAxisCoord.Z.ToString("f3");
                //this.角度范围numericUpDown.Value = (decimal)this.CamParam.CaliParam.AngleRange;
                //this.旋转步数numericUpDown.Value = (decimal)this.CamParam.CaliParam.AngleStep;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void 获取旋转中心button_Click(object sender, EventArgs e)
        {
            try
            {
                double X = 0, Y = 0, Z = 0, Theta = 0, U = 0, V = 0;
                Read(out X, out Y, out Z, out Theta, out U, out V);
                /////////////////////////////////////////////
                this.CamParam.CaliParam.RotateCalibPoint = new userWcsVector(X, Y, Z, U, V, Theta);
                this.旋转中心位置坐标textBox.Text = "X:" + X.ToString("f3") + "   Y:" + Y.ToString("f3") + "   Z:" + Z.ToString("f3") + "   Theta:" + Theta.ToString("f3")
                    + "   U:" + U.ToString("f3")
                    + "   V:" + V.ToString("f3");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void 获取起始点button_Click(object sender, EventArgs e)
        {
            try
            {
                double X = 0, Y = 0, Z = 0, Theta = 0, U = 0, V = 0;
                Read(out X, out Y, out Z, out Theta, out U, out V);
                ///////////////////////
                this.CamParam.CaliParam.StartCaliPoint = new userWcsVector(X, Y, Z, Theta);
                this.起始点textBox.Text = "X:" + X.ToString("f3") + "   Y:" + Y.ToString("f3") + "   Z:" + Z.ToString("f3") + "   Theta:" + Theta.ToString("f3");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void 获取终止点button_Click(object sender, EventArgs e)
        {
            try
            {
                double X = 0, Y = 0, Z = 0, Theta = 0, U = 0, V = 0;
                Read(out X, out Y, out Z, out Theta, out U, out V);
                ///////////////////////
                this.CamParam.CaliParam.EndCalibPoint = new userWcsVector(X, Y, Z, Theta);
                this.终止点textBox.Text = "X:" + X.ToString("f3") + "   Y:" + Y.ToString("f3") + "   Z:" + Z.ToString("f3") + "   Theta:" + Theta.ToString("f3");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void Read(out double X, out double Y, out double Z, out double Theta, out double U, out double V)
        {
            IMotionControl card = MotionCardManage.GetCard(this.CamParam.CaliParam.CoordSysName);
            if (card == null)
            {
                X = 0;
                Y = 0;
                Z = 0;
                Theta = 0;
                U = 0;
                V = 0;
            }
            else
            {
                MotionCardManage.GetCard(this.CamParam.CaliParam.CoordSysName).GetAxisPosition(this.CamParam.CaliParam.CoordSysName, enAxisName.X轴, out X);
                MotionCardManage.GetCard(this.CamParam.CaliParam.CoordSysName).GetAxisPosition(this.CamParam.CaliParam.CoordSysName, enAxisName.Y轴, out Y);
                MotionCardManage.GetCard(this.CamParam.CaliParam.CoordSysName).GetAxisPosition(this.CamParam.CaliParam.CoordSysName, enAxisName.Z轴, out Z);
                MotionCardManage.GetCard(this.CamParam.CaliParam.CoordSysName).GetAxisPosition(this.CamParam.CaliParam.CoordSysName, enAxisName.Theta轴, out Theta);
                MotionCardManage.GetCard(this.CamParam.CaliParam.CoordSysName).GetAxisPosition(this.CamParam.CaliParam.CoordSysName, enAxisName.U轴, out U);
                MotionCardManage.GetCard(this.CamParam.CaliParam.CoordSysName).GetAxisPosition(this.CamParam.CaliParam.CoordSysName, enAxisName.V轴, out V);
            }
        }
        private void Write(double X, double Y, double Z, double Theta)
        {
            IMotionControl card = MotionCardManage.GetCard(this.CamParam.CaliParam.CoordSysName);
            if (card != null)
                MotionCardManage.GetCard(this.CamParam.CaliParam.CoordSysName).MoveMultyAxis(this.CamParam.CaliParam.CoordSysName, enAxisName.XYZTheta轴, 0, new CoordSysAxisParam(X, Y, Z, Theta, 0, 0));
        }

        private void 旋转中心位置坐标textBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (!this.isLoad) return;
                string[] name = 旋转中心位置坐标textBox.Text.Split(new string[] { "X:", "Y:", "Z:", "Theta:", "U:", "V:" }, StringSplitOptions.RemoveEmptyEntries);
                if (name.Length < 6) return;
                double X, Y, Z, Theta, U, V;
                bool result1 = double.TryParse(name[0].Trim(), out X);
                bool result2 = double.TryParse(name[1].Trim(), out Y);
                bool result3 = double.TryParse(name[2].Trim(), out Z);
                bool result4 = double.TryParse(name[3].Trim(), out Theta);
                bool result5 = double.TryParse(name[4].Trim(), out U);
                bool result6 = double.TryParse(name[5].Trim(), out V);
                if (result1 && result2 && result3 && result4 && result5 && result6)
                    this.CamParam.CaliParam.RotateCalibPoint = new userWcsVector(X, Y, Z, U, V, Theta);
                else
                    MessageBox.Show("数据转换报错");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void 起始点textBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (!this.isLoad) return;
                string[] name = 起始点textBox.Text.Split(new string[] { "X:", "Y:", "Z:", "Theta:" }, StringSplitOptions.RemoveEmptyEntries);
                if (name.Length < 4) return;
                double X, Y, Z, Theta;
                bool result1 = double.TryParse(name[0].Trim(), out X);
                bool result2 = double.TryParse(name[1].Trim(), out Y);
                bool result3 = double.TryParse(name[2].Trim(), out Z);
                bool result4 = double.TryParse(name[3].Trim(), out Theta);
                if (result1 && result2 && result3 && result4)
                    this.CamParam.CaliParam.StartCaliPoint = new userWcsVector(X, Y, Z, Theta);
                else
                    MessageBox.Show("数据转换报错");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void 终止点textBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (!this.isLoad) return;
                string[] name = 终止点textBox.Text.Split(new string[] { "X:", "Y:", "Z:", "Theta:" }, StringSplitOptions.RemoveEmptyEntries);
                if (name.Length < 4) return;
                double X, Y, Z, Theta;
                bool result1 = double.TryParse(name[0].Trim(), out X);
                bool result2 = double.TryParse(name[1].Trim(), out Y);
                bool result3 = double.TryParse(name[2].Trim(), out Z);
                bool result4 = double.TryParse(name[3].Trim(), out Theta);
                if (result1 && result2 && result3 && result4)
                    this.CamParam.CaliParam.EndCalibPoint = new userWcsVector(X, Y, Z, Theta);
                else
                    MessageBox.Show("数据转换报错");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }

        private void CalibPlc()
        {
            TreeNode node;
            string info;
            double X, Y, Z, Theta;
            double center_Row, center_Col, error,rotaeAngle;
            this.listGrabTheta.Clear();
            this.listGrabPoint.Clear();
            List<double> list_RotaRows = new List<double>();
            List<double> list_RotaCols = new List<double>();
            List<double> list_NpointRows = new List<double>();
            List<double> list_NpointCols = new List<double>();
            List<double> list_x = new List<double>();
            List<double> list_y = new List<double>();
            List<double> list_Rx = new List<double>(); // 旋转Mark点的世界坐标 X
            List<double> list_Ry = new List<double>();//  旋转Mark点的世界坐标 Y
            //// 初始化触发信号并等待触发信号 
            CommunicationConfigParamManger.Instance.WriteValue(this.CamParam.CaliParam.CoordSysName, enCommunicationCommand.TriggerFromPlc,0);
            while (true)
            {
                if (this.isStop) return; // 控制标定停止
                Application.DoEvents();
                object value = CommunicationConfigParamManger.Instance.ReadValue(this.CamParam.CaliParam.CoordSysName, enCommunicationCommand.TriggerFromPlc);//CommunicationConfigParamManger.Instance.GetCommunicationParam(
                if (value != null && value.ToString() == "1")
                {
                    CommunicationConfigParamManger.Instance.WriteValue(this.CamParam.CaliParam.CoordSysName, enCommunicationCommand.TriggerFromPlc, 0);
                    break;
                }                    
                LoggerHelper.Info("等待PLC触发信号!!!");
            }
            switch (this.CamParam.CaliParam.CalibMethod)
            {
                case enCalibMethod.先旋转后平移:
                    // 先执行旋转
                    this.CurCaliStateEnum = enCaliStateEnum.RotCali;
                    this.CalibRotaPixList?.Clear();
                    this.CamParam.CaliParam.CalibCenterXy = new userWcsPoint(0, 0, 0); // 在开始标定前，必需将旋转中心的置0
                    CommunicationConfigParamManger.Instance.WriteValue(this.CamParam.CaliParam.CoordSysName, enCommunicationCommand.RotCali, 1);
                    CommunicationConfigParamManger.Instance.WriteValue(this.CamParam.CaliParam.CoordSysName, enCommunicationCommand.Cali9Pt, 0);
                    if (this.calibCoordConfigParamRota.CalibCoordParamList.Count == 0)
                    {
                        this.toolStripButton_Run.Enabled = true;
                        this.toolStripButton_Stop.Enabled = false;
                        MessageBox.Show("没有可供旋转的坐标点!!!");
                        return;
                    }
                    foreach (var item in this.calibCoordConfigParamRota.CalibCoordParamList)
                    {
                        Application.DoEvents();
                        if (this.isStop) return; // 控制标定停止
                        // 等待轴运动到位
                        switch (this.CamParam.CaliParam.CoordValueType)
                        {
                            case enCoordValueType.绝对坐标:
                                MotionCardManage.GetCard(this.CamParam.CaliParam.CoordSysName).MoveMultyAxis(this.CamParam.CaliParam.CoordSysName,
                                                         enAxisName.Compensation_XYTheta轴, 10, new CoordSysAxisParam(item.X, item.Y, item.Z, item.Theta, 0, 0));
                                break;
                            case enCoordValueType.相对坐标:
                                MotionCardManage.GetCard(this.CamParam.CaliParam.CoordSysName).GetAxisPosition(this.CamParam.CaliParam.CoordSysName, enAxisName.X轴, out X);
                                MotionCardManage.GetCard(this.CamParam.CaliParam.CoordSysName).GetAxisPosition(this.CamParam.CaliParam.CoordSysName, enAxisName.Y轴, out Y);
                                MotionCardManage.GetCard(this.CamParam.CaliParam.CoordSysName).GetAxisPosition(this.CamParam.CaliParam.CoordSysName, enAxisName.Theta轴, out Theta);
                                MotionCardManage.GetCard(this.CamParam.CaliParam.CoordSysName).MoveMultyAxis(this.CamParam.CaliParam.CoordSysName,
                                                         enAxisName.Compensation_XYTheta轴, 10, new CoordSysAxisParam((item.X - X), (item.Y - Y), 0, (item.Theta - Theta), 0, 0)); // 写入的是补偿轴地址
                                break;
                        }
                        Thread.Sleep(100);
                        LoggerHelper.Info("等待轴运动到指定位置:" + "X = " + item.X.ToString() + "  Y = " + item.Y.ToString() + "  Theta = " + item.Theta.ToString());
                        // 往地址中写入值后触发PLC  执行动作
                        CommunicationConfigParamManger.Instance.WriteValue(this.CamParam.CaliParam.CoordSysName, enCommunicationCommand.ResultToPlc, 3);
                        CommunicationConfigParamManger.Instance.WriteValue(this.CamParam.CaliParam.CoordSysName, enCommunicationCommand.FunctionNoToPlc, 5);
                        CommunicationConfigParamManger.Instance.WriteValue(this.CamParam.CaliParam.CoordSysName, enCommunicationCommand.TriggerToPlc, 1);
                        // 等待PLC触发采图
                        while (true)
                        {
                            if (this.isStop) return; // 控制标定停止
                            Application.DoEvents();
                            object value = CommunicationConfigParamManger.Instance.ReadValue(this.CamParam.CaliParam.CoordSysName, enCommunicationCommand.TriggerFromPlc); //CommunicationConfigParamManger.Instance.GetCommunicationParam(
                            if (value != null && value.ToString() == "1")
                            {
                                MotionCardManage.GetCard(this.CamParam.CaliParam.CoordSysName).GetAxisPosition(this.CamParam.CaliParam.CoordSysName, enAxisName.X轴, out X);
                                MotionCardManage.GetCard(this.CamParam.CaliParam.CoordSysName).GetAxisPosition(this.CamParam.CaliParam.CoordSysName, enAxisName.Y轴, out Y);
                                MotionCardManage.GetCard(this.CamParam.CaliParam.CoordSysName).GetAxisPosition(this.CamParam.CaliParam.CoordSysName, enAxisName.Theta轴, out Theta);
                                if (Math.Abs(item.X - X) > 0.05 || Math.Abs(item.Y - Y) > 0.05 || Math.Abs(item.Theta - Theta) > 0.1)
                                {
                                    MessageBox.Show("PLC 没有运动到指定位置，请确认是否到限位或轴是否有移动!");
                                    CommunicationConfigParamManger.Instance.WriteValue(this.CamParam.CaliParam.CoordSysName, enCommunicationCommand.ResultToPlc, 2);
                                    CommunicationConfigParamManger.Instance.WriteValue(this.CamParam.CaliParam.CoordSysName, enCommunicationCommand.FunctionNoToPlc, 5);
                                    CommunicationConfigParamManger.Instance.WriteValue(this.CamParam.CaliParam.CoordSysName, enCommunicationCommand.TriggerToPlc, 1);
                                    this.toolStripButton_Run.Enabled = true;
                                    this.toolStripButton_Stop.Enabled = false;
                                    return;
                                }
                                else
                                    break;
                            }
                            LoggerHelper.Info("等待PLC触发信号!!!");
                        }
                        node = this.GetExecuteNode(this.CamParam.CaliParam.CoordSysName, out info);
                        LoggerHelper.Info("接收PLC触发信号!!!" + info);
                        if (node != null)
                            ((IFunction)node.Tag)?.Execute(node);
                        else
                        {
                            this.toolStripButton_Run.Enabled = true;
                            this.toolStripButton_Stop.Enabled = false;
                            MessageBox.Show("没有获取到可执行的节点!!");
                            return;
                        }
                        CommunicationConfigParamManger.Instance.WriteValue(this.CamParam.CaliParam.CoordSysName, enCommunicationCommand.TriggerFromPlc, 0);
                    }
                    // 旋转完后，计算旋转中心坐标
                    List<double> rowsList = new List<double>();
                    List<double> colsList = new List<double>();
                    foreach (var item in this.CalibRotaPixList)
                    {
                        rowsList.Add(item.Row);
                        colsList.Add(item.Col);
                    }
                    switch (this.CamParam.CaliParam.CalculateMethod)
                    {
                        default:
                        case enCalculateMethod.拟合圆:
                            CalibrateMethod.Instance.CalculateFitCenter(rowsList.ToArray(), colsList.ToArray(), out center_Row, out center_Col);
                            rotaeAngle = this.CamParam.CaliParam.AngleRange / this.CamParam.CaliParam.AngleStep;
                            break;
                        case enCalculateMethod.两点夹角:
                            CalibrateMethod.Instance.CalculateCenter(rowsList.ToArray(), colsList.ToArray(), this.CamParam.CaliParam.AngleRange / this.CamParam.CaliParam.AngleStep, out center_Row, out center_Col, out error);
                            rotaeAngle = this.CamParam.CaliParam.AngleRange / this.CamParam.CaliParam.AngleStep;
                            break;
                        case enCalculateMethod.矩阵变换:
                            CalibrateMethod.Instance.CalculateCenterHomMatPix(rowsList.ToArray(), colsList.ToArray(), (this.CamParam.CaliParam.AngleRange / this.CamParam.CaliParam.AngleStep) * Math.PI / 180, out center_Row, out center_Col, out error);
                            rotaeAngle = this.CamParam.CaliParam.AngleRange / this.CamParam.CaliParam.AngleStep;
                            break;
                        case enCalculateMethod.N点矩阵变换:
                            CalibrateMethod.Instance.CalculateCenterHomMatPixNpoint(rowsList.ToArray(), colsList.ToArray(), this.calibCoordConfigParamRota.CalibCoordParamList.Count, out center_Row, out center_Col, out error, out rotaeAngle); //
                            break;
                    }
                    ////////////////
                    MessageBox.Show("放转中心:" + "Row = "  + center_Row.ToString("f3") + ";Col = " + center_Col.ToString("f3") + ";步长角 = " + rotaeAngle.ToString("f3"));
                    // 再执行平移
                    this.CurCaliStateEnum = enCaliStateEnum.Cali9Pt;
                    this.CalibNPointPixList?.Clear();
                    CommunicationConfigParamManger.Instance.WriteValue(this.CamParam.CaliParam.CoordSysName, enCommunicationCommand.RotCali, 0);
                    CommunicationConfigParamManger.Instance.WriteValue(this.CamParam.CaliParam.CoordSysName, enCommunicationCommand.Cali9Pt, 1);
                    if (this.calibCoordConfigParamNPoint.CalibCoordParamList.Count == 0)
                    {
                        this.toolStripButton_Run.Enabled = true;
                        this.toolStripButton_Stop.Enabled = false;
                        MessageBox.Show("没有可供移动的坐标点!!!");
                        return;
                    }
                    foreach (var item in this.calibCoordConfigParamNPoint.CalibCoordParamList)
                    {
                        if (this.isStop) return; // 控制标定停止
                        Application.DoEvents();
                        // 等待轴运动到位
                        switch (this.CamParam.CaliParam.CoordValueType)
                        {
                            case enCoordValueType.绝对坐标:
                                MotionCardManage.GetCard(this.CamParam.CaliParam.CoordSysName).MoveMultyAxis(this.CamParam.CaliParam.CoordSysName,
                                                         enAxisName.Compensation_XYTheta轴, 10, new CoordSysAxisParam(item.X, item.Y, item.Z, item.Theta, 0, 0));
                                break;
                            case enCoordValueType.相对坐标:
                                MotionCardManage.GetCard(this.CamParam.CaliParam.CoordSysName).GetAxisPosition(this.CamParam.CaliParam.CoordSysName, enAxisName.X轴, out X);
                                MotionCardManage.GetCard(this.CamParam.CaliParam.CoordSysName).GetAxisPosition(this.CamParam.CaliParam.CoordSysName, enAxisName.Y轴, out Y);
                                MotionCardManage.GetCard(this.CamParam.CaliParam.CoordSysName).GetAxisPosition(this.CamParam.CaliParam.CoordSysName, enAxisName.Theta轴, out Theta);
                                MotionCardManage.GetCard(this.CamParam.CaliParam.CoordSysName).MoveMultyAxis(this.CamParam.CaliParam.CoordSysName,
                                                         enAxisName.Compensation_XYTheta轴, 10, new CoordSysAxisParam((item.X - X), (item.Y - Y), 0, (item.Theta - Theta), 0, 0)); // 写入的是补偿轴地址
                                break;
                        }
                        Thread.Sleep(100);
                        LoggerHelper.Info("等待轴运动到指定位置:" + "X = " + item.X.ToString("f3") + "  Y = " + item.Y.ToString("f3") + "  Theta = " + item.Theta.ToString("f5"));
                        CommunicationConfigParamManger.Instance.WriteValue(this.CamParam.CaliParam.CoordSysName, enCommunicationCommand.ResultToPlc, 3);
                        CommunicationConfigParamManger.Instance.WriteValue(this.CamParam.CaliParam.CoordSysName, enCommunicationCommand.FunctionNoToPlc, 5);
                        CommunicationConfigParamManger.Instance.WriteValue(this.CamParam.CaliParam.CoordSysName, enCommunicationCommand.TriggerToPlc, 1);
                        // 等待PLC触发采图
                        while (true)
                        {
                            if (this.isStop) return; // 控制标定停止
                            Application.DoEvents();
                            object value = CommunicationConfigParamManger.Instance.ReadValue(this.CamParam.CaliParam.CoordSysName, enCommunicationCommand.TriggerFromPlc); //CommunicationConfigParamManger.Instance.GetCommunicationParam(;
                            if (value != null && value.ToString() == "1")
                            {
                                MotionCardManage.GetCard(this.CamParam.CaliParam.CoordSysName).GetAxisPosition(this.CamParam.CaliParam.CoordSysName, enAxisName.X轴, out X);
                                MotionCardManage.GetCard(this.CamParam.CaliParam.CoordSysName).GetAxisPosition(this.CamParam.CaliParam.CoordSysName, enAxisName.Y轴, out Y);
                                MotionCardManage.GetCard(this.CamParam.CaliParam.CoordSysName).GetAxisPosition(this.CamParam.CaliParam.CoordSysName, enAxisName.Theta轴, out Theta);
                                if (Math.Abs(item.X - X) > 0.05 || Math.Abs(item.Y - Y) > 0.05 || Math.Abs(item.Theta - Theta) > 0.1)
                                {
                                    MessageBox.Show("PLC 没有运动到指定位置，请确认是否到限位或轴是否有移动!");
                                    CommunicationConfigParamManger.Instance.WriteValue(this.CamParam.CaliParam.CoordSysName, enCommunicationCommand.ResultToPlc, 2);
                                    CommunicationConfigParamManger.Instance.WriteValue(this.CamParam.CaliParam.CoordSysName, enCommunicationCommand.FunctionNoToPlc, 5);
                                    CommunicationConfigParamManger.Instance.WriteValue(this.CamParam.CaliParam.CoordSysName, enCommunicationCommand.TriggerToPlc, 1);
                                    this.toolStripButton_Run.Enabled = true;
                                    this.toolStripButton_Stop.Enabled = false;
                                    return;
                                }
                                else
                                    break;
                            }
                            LoggerHelper.Info("等待PLC触发信号!!!");
                        }
                        node = this.GetExecuteNode(this.CamParam.CaliParam.CoordSysName, out info);
                        LoggerHelper.Info("接收PLC触发信号!!!" + info);
                        if (node != null)
                            ((IFunction)node.Tag)?.Execute(node);
                        else
                        {
                            this.toolStripButton_Run.Enabled = true;
                            this.toolStripButton_Stop.Enabled = false;
                            MessageBox.Show("没有获取到可执行的节点!!");
                            return;
                        }
                        CommunicationConfigParamManger.Instance.WriteValue(this.CamParam.CaliParam.CoordSysName, enCommunicationCommand.TriggerFromPlc, 0);
                    }
                    // 标定完成
                    this.toolStripButton_Run.Enabled = true;
                    this.toolStripButton_Stop.Enabled = false;
                    CommunicationConfigParamManger.Instance.WriteValue(this.CamParam.CaliParam.CoordSysName, enCommunicationCommand.FunctionNoToPlc, 5);
                    CommunicationConfigParamManger.Instance.WriteValue(this.CamParam.CaliParam.CoordSysName, enCommunicationCommand.ResultToPlc, 1);
                    CommunicationConfigParamManger.Instance.WriteValue(this.CamParam.CaliParam.CoordSysName, enCommunicationCommand.TriggerToPlc, 1);
                    HTuple rows, cols;
                    HTuple x, y;
                    CalibrateMethod.Instance.PixPointToHtuple(this.CalibNPointPixList, out rows, out cols);
                    CalibrateMethod.Instance.WcsPointToHtuple(this.calibCoordConfigParamNPoint.CalibCoordParamList, out x, out y);
                    double offset_row = center_Row - (rows.TupleMax() + rows.TupleMin()) * 0.5;
                    double offset_col = center_Col - (cols.TupleMax() + cols.TupleMin()) * 0.5; //
                    rows += offset_row; // 平移9点到旋转中心
                    cols += offset_col;
                    x -= (x.TupleMax() + x.TupleMin()) * 0.5;
                    y -= (y.TupleMax() + y.TupleMin()) * 0.5;
                    switch (this.CamParam.CaliParam.CalibAxis)
                    {
                        default:
                        case enCalibAxis.XY轴:
                            if (this.CamParam.CaliParam.InvertX)
                                x *= -1;
                            if (this.CamParam.CaliParam.InvertY)
                                y *= -1;
                            this.CamParam.HomMat2D = CalibrateMethod.Instance.NpointCalib(rows, cols, x, y, out error); // 更新了矩阵
                            break;
                        case enCalibAxis.单轴:
                        case enCalibAxis.X轴:
                        case enCalibAxis.Y轴:
                            this.CamParam.HomMat2D = CalibrateMethod.Instance.NpointCalibSingleAxis(rows, cols, x, y, this.CamParam.CaliParam.InvertX, this.CamParam.CaliParam.InvertY, out error); // 更新了矩阵
                            break;
                    }
                    this.CamParam.CaliParam.CalibCenterXy = new userWcsPoint(0, 0, 0); // 这里要置0
                    MessageBox.Show("N点标定最大误差:" + error.ToString("f3") + ";标定矩阵:" + this.CamParam.HomMat2D.ToString());
                    break;
                case enCalibMethod.先平移后旋转:
                    // 先执行平移
                    this.CurCaliStateEnum = enCaliStateEnum.Cali9Pt;
                    this.CalibNPointPixList?.Clear();
                    this.CamParam.CaliParam.CalibCenterXy = new userWcsPoint(0, 0, 0); // 在开始标定前，必需将旋转中心的置0
                    CommunicationConfigParamManger.Instance.WriteValue(this.CamParam.CaliParam.CoordSysName, enCommunicationCommand.RotCali,0);
                    CommunicationConfigParamManger.Instance.WriteValue(this.CamParam.CaliParam.CoordSysName, enCommunicationCommand.Cali9Pt, 1);
                    if (this.calibCoordConfigParamNPoint.CalibCoordParamList.Count == 0)
                    {
                        this.toolStripButton_Run.Enabled = true;
                        this.toolStripButton_Stop.Enabled = false;
                        MessageBox.Show("没有可供移动的坐标点!!!");
                        return;
                    }
                    foreach (var item in this.calibCoordConfigParamNPoint.CalibCoordParamList)
                    {
                        if (this.isStop) return; // 控制标定停止
                        Application.DoEvents();
                        switch (this.CamParam.CaliParam.CoordValueType)
                        {
                            case enCoordValueType.绝对坐标:
                                MotionCardManage.GetCard(this.CamParam.CaliParam.CoordSysName).MoveMultyAxis(this.CamParam.CaliParam.CoordSysName,
                                                         enAxisName.Compensation_XYTheta轴, 10, new CoordSysAxisParam(item.X, item.Y, item.Z, item.Theta, 0, 0));
                                break;
                            case enCoordValueType.相对坐标:
                                MotionCardManage.GetCard(this.CamParam.CaliParam.CoordSysName).GetAxisPosition(this.CamParam.CaliParam.CoordSysName, enAxisName.X轴, out X);
                                MotionCardManage.GetCard(this.CamParam.CaliParam.CoordSysName).GetAxisPosition(this.CamParam.CaliParam.CoordSysName, enAxisName.Y轴, out Y);
                                MotionCardManage.GetCard(this.CamParam.CaliParam.CoordSysName).GetAxisPosition(this.CamParam.CaliParam.CoordSysName, enAxisName.Theta轴, out Theta);
                                MotionCardManage.GetCard(this.CamParam.CaliParam.CoordSysName).MoveMultyAxis(this.CamParam.CaliParam.CoordSysName,
                                                         enAxisName.Compensation_XYTheta轴, 10, new CoordSysAxisParam((item.X - X), (item.Y - Y), 0, (item.Theta - Theta), 0, 0)); // 写入的是补偿轴地址
                                break;
                        }
                        Thread.Sleep(100);
                        LoggerHelper.Info("等待轴运动到指定位置:" + "X = " + item.X.ToString("f4") + "  Y = " + item.Y.ToString("f4"));
                        CommunicationConfigParamManger.Instance.WriteValue(this.CamParam.CaliParam.CoordSysName, enCommunicationCommand.ResultToPlc, 3);
                        CommunicationConfigParamManger.Instance.WriteValue(this.CamParam.CaliParam.CoordSysName, enCommunicationCommand.FunctionNoToPlc, 5);
                        CommunicationConfigParamManger.Instance.WriteValue(this.CamParam.CaliParam.CoordSysName, enCommunicationCommand.TriggerToPlc, 1);
                        // 等待PLC触发采图
                        while (true)
                        {
                            if (this.isStop) return; // 控制标定停止
                            Application.DoEvents();
                            object value = CommunicationConfigParamManger.Instance.ReadValue(this.CamParam.CaliParam.CoordSysName, enCommunicationCommand.TriggerFromPlc);//CommunicationConfigParamManger.Instance.GetCommunicationParam(
                            if (value != null && value.ToString() == "1")
                            {
                                MotionCardManage.GetCard(this.CamParam.CaliParam.CoordSysName).GetAxisPosition(this.CamParam.CaliParam.CoordSysName, enAxisName.X轴, out X);
                                MotionCardManage.GetCard(this.CamParam.CaliParam.CoordSysName).GetAxisPosition(this.CamParam.CaliParam.CoordSysName, enAxisName.Y轴, out Y);
                                MotionCardManage.GetCard(this.CamParam.CaliParam.CoordSysName).GetAxisPosition(this.CamParam.CaliParam.CoordSysName, enAxisName.Theta轴, out Theta);
                                if (Math.Abs(item.X - X) > 0.05 || Math.Abs(item.Y - Y) > 0.05 || Math.Abs(item.Theta - Theta) > 0.1)
                                {
                                    MessageBox.Show("PLC 没有运动到指定位置，请确认是否到限位或轴是否有移动!");
                                    CommunicationConfigParamManger.Instance.WriteValue(this.CamParam.CaliParam.CoordSysName, enCommunicationCommand.ResultToPlc, 2);
                                    CommunicationConfigParamManger.Instance.WriteValue(this.CamParam.CaliParam.CoordSysName, enCommunicationCommand.FunctionNoToPlc, 5);
                                    CommunicationConfigParamManger.Instance.WriteValue(this.CamParam.CaliParam.CoordSysName, enCommunicationCommand.TriggerToPlc, 1);
                                    this.toolStripButton_Run.Enabled = true;
                                    this.toolStripButton_Stop.Enabled = false;
                                    return;
                                }
                                else
                                    break;
                            }
                            LoggerHelper.Info("等待PLC触发信号!!!");
                        }
                        node = this.GetExecuteNode(this.CamParam.CaliParam.CoordSysName, out info);
                        LoggerHelper.Info("接收PLC触发信号!!!" + info);
                        if (node != null)
                            ((IFunction)node.Tag)?.Execute(node);
                        else
                        {
                            this.toolStripButton_Run.Enabled = true;
                            this.toolStripButton_Stop.Enabled = false;
                            MessageBox.Show("没有获取到可执行的节点!!");
                            return;
                        }
                        CommunicationConfigParamManger.Instance.WriteValue(this.CamParam.CaliParam.CoordSysName, enCommunicationCommand.TriggerFromPlc, 0);
                    }
                    // 先执行一次标定，用世界坐标来计算圆心，这样可以得到圆心的世界坐标，再使用世界点加上该值，再执行一次标定
                    error = 0;
                    CalibrateMethod.Instance.PixPointToHtuple(this.CalibNPointPixList, out rows, out cols);
                    CalibrateMethod.Instance.WcsPointToHtuple(this.calibCoordConfigParamNPoint.CalibCoordParamList, out x, out y);
                    x -= (x.TupleMax() + x.TupleMin()) * 0.5;
                    y -= (y.TupleMax() + y.TupleMin()) * 0.5;
                    switch (this.CamParam.CaliParam.CalibAxis)
                    {
                        default:
                        case enCalibAxis.XY轴:
                            if (this.CamParam.CaliParam.InvertX)
                                x *= -1;
                            if (this.CamParam.CaliParam.InvertY)
                                y *= -1;
                            this.CamParam.HomMat2D = CalibrateMethod.Instance.NpointCalib(rows, cols, x, y, out error); // 更新了矩阵
                            break;
                        case enCalibAxis.单轴:
                        case enCalibAxis.X轴:
                        case enCalibAxis.Y轴:
                            this.CamParam.HomMat2D = CalibrateMethod.Instance.NpointCalibSingleAxis(rows, cols, x, y, this.CamParam.CaliParam.InvertX, this.CamParam.CaliParam.InvertY, out error); // 更新了矩阵
                            break;
                    }
                    MessageBox.Show("N点标定最大误差:" + error.ToString("f4") + ";标定矩阵:" + this.CamParam.HomMat2D.ToString());
                    // this.MoveOriginToImageCenter(); 这里不平移到视野中心
                    // 再执行旋转
                    this.CurCaliStateEnum = enCaliStateEnum.RotCali;
                    this.CalibRotaPixList?.Clear();
                    this.CalibRotaWcsList?.Clear();
                    CommunicationConfigParamManger.Instance.WriteValue(this.CamParam.CaliParam.CoordSysName, enCommunicationCommand.RotCali, 1);
                    CommunicationConfigParamManger.Instance.WriteValue(this.CamParam.CaliParam.CoordSysName, enCommunicationCommand.Cali9Pt, 0);
                    //////////////////////////////////////////////////////////////
                    if (this.calibCoordConfigParamRota.CalibCoordParamList.Count == 0)
                    {
                        this.toolStripButton_Run.Enabled = true;
                        this.toolStripButton_Stop.Enabled = false;
                        MessageBox.Show("没有可供移动的坐标点!!!");
                        return;
                    }
                    foreach (var item in this.calibCoordConfigParamRota.CalibCoordParamList)
                    {
                        Application.DoEvents();
                        if (this.isStop) return; // 控制标定停止
                        // 等待轴运动到位
                        switch (this.CamParam.CaliParam.CoordValueType)
                        {
                            case enCoordValueType.绝对坐标:
                                MotionCardManage.GetCard(this.CamParam.CaliParam.CoordSysName).MoveMultyAxis(this.CamParam.CaliParam.CoordSysName,
                                                         enAxisName.Compensation_XYTheta轴, 10, new CoordSysAxisParam(item.X, item.Y, item.Z, item.Theta, 0, 0));
                                break;
                            case enCoordValueType.相对坐标:
                                MotionCardManage.GetCard(this.CamParam.CaliParam.CoordSysName).GetAxisPosition(this.CamParam.CaliParam.CoordSysName, enAxisName.X轴, out X);
                                MotionCardManage.GetCard(this.CamParam.CaliParam.CoordSysName).GetAxisPosition(this.CamParam.CaliParam.CoordSysName, enAxisName.Y轴, out Y);
                                MotionCardManage.GetCard(this.CamParam.CaliParam.CoordSysName).GetAxisPosition(this.CamParam.CaliParam.CoordSysName, enAxisName.Theta轴, out Theta);
                                MotionCardManage.GetCard(this.CamParam.CaliParam.CoordSysName).MoveMultyAxis(this.CamParam.CaliParam.CoordSysName,
                                                         enAxisName.Compensation_XYTheta轴, 10, new CoordSysAxisParam((item.X - X), (item.Y - Y), 0, (item.Theta - Theta), 0, 0)); // 写入的是补偿轴地址
                                break;
                        }
                        Thread.Sleep(100);
                        LoggerHelper.Info("等待轴运动到指定位置:" + "X = " + item.X.ToString("f3") + "  Y = " + item.Y.ToString("f3") + "  Theta = " + item.Theta.ToString("f3"));
                        CommunicationConfigParamManger.Instance.WriteValue(this.CamParam.CaliParam.CoordSysName, enCommunicationCommand.ResultToPlc, 3);
                        CommunicationConfigParamManger.Instance.WriteValue(this.CamParam.CaliParam.CoordSysName, enCommunicationCommand.FunctionNoToPlc, 5);
                        CommunicationConfigParamManger.Instance.WriteValue(this.CamParam.CaliParam.CoordSysName, enCommunicationCommand.TriggerToPlc, 1);
                        // 等待PLC触发采图
                        while (true)
                        {
                            if (this.isStop) return; // 控制标定停止
                            Application.DoEvents();
                            object value = CommunicationConfigParamManger.Instance.ReadValue(this.CamParam.CaliParam.CoordSysName, enCommunicationCommand.TriggerFromPlc);
                            if (value != null && value.ToString() == "1")
                            {
                                MotionCardManage.GetCard(this.CamParam.CaliParam.CoordSysName).GetAxisPosition(this.CamParam.CaliParam.CoordSysName, enAxisName.X轴, out X);
                                MotionCardManage.GetCard(this.CamParam.CaliParam.CoordSysName).GetAxisPosition(this.CamParam.CaliParam.CoordSysName, enAxisName.Y轴, out Y);
                                MotionCardManage.GetCard(this.CamParam.CaliParam.CoordSysName).GetAxisPosition(this.CamParam.CaliParam.CoordSysName, enAxisName.Theta轴, out Theta);
                                if (Math.Abs(item.X - X) > 0.05 || Math.Abs(item.Y - Y) > 0.05 || Math.Abs(item.Theta - Theta) > 0.1)
                                {
                                    MessageBox.Show("PLC 没有运动到指定位置，请确认是否到限位或轴是否有移动!");
                                    CommunicationConfigParamManger.Instance.WriteValue(this.CamParam.CaliParam.CoordSysName, enCommunicationCommand.ResultToPlc, 2);
                                    CommunicationConfigParamManger.Instance.WriteValue(this.CamParam.CaliParam.CoordSysName, enCommunicationCommand.FunctionNoToPlc, 5);
                                    CommunicationConfigParamManger.Instance.WriteValue(this.CamParam.CaliParam.CoordSysName, enCommunicationCommand.TriggerToPlc, 1);
                                    this.toolStripButton_Run.Enabled = true;
                                    this.toolStripButton_Stop.Enabled = false;
                                    return;
                                }
                                else
                                    break;
                            }
                            LoggerHelper.Info("等待PLC触发信号!!!");
                        }
                        Thread.Sleep(100);
                        node = this.GetExecuteNode(this.CamParam.CaliParam.CoordSysName, out info);
                        LoggerHelper.Info("接收PLC触发信号!!!" + info);
                        if (node != null)
                            ((IFunction)node.Tag)?.Execute(node);
                        else
                        {
                            this.toolStripButton_Run.Enabled = true;
                            this.toolStripButton_Stop.Enabled = false;
                            MessageBox.Show("没有获取到可执行的节点!!");
                            return;
                        }
                        CommunicationConfigParamManger.Instance.WriteValue(this.CamParam.CaliParam.CoordSysName, enCommunicationCommand.TriggerFromPlc, 0);
                    }
                    // 标定完成
                    this.toolStripButton_Run.Enabled = true;
                    this.toolStripButton_Stop.Enabled = false;
                    CommunicationConfigParamManger.Instance.WriteValue(this.CamParam.CaliParam.CoordSysName, enCommunicationCommand.ResultToPlc, 1);
                    CommunicationConfigParamManger.Instance.WriteValue(this.CamParam.CaliParam.CoordSysName, enCommunicationCommand.FunctionNoToPlc, 5);
                    CommunicationConfigParamManger.Instance.WriteValue(this.CamParam.CaliParam.CoordSysName, enCommunicationCommand.TriggerToPlc, 1);
                    // 旋转完一次后，执行计算标定矩阵
                    double Center_x = 0, Center_y = 0;
                    list_x = new List<double>();
                    list_y = new List<double>();
                    foreach (var item in this.CalibRotaWcsList)
                    {
                        switch (this.CamParam.CaliParam.CoordOriginType)
                        {
                            case enCoordOriginType.机械原点: // 这种模式必需将相机设置为移对象
                                list_x.Add(item.X);
                                list_y.Add(item.Y);
                                break;
                            default:
                            case enCoordOriginType.旋转中心:
                            case enCoordOriginType.IsLoading:
                                userWcsPoint wcsPoint = item.GetWcsCamPoint(); // 这种模式下使用相机坐标系下的点
                                list_x.Add(wcsPoint.X);
                                list_y.Add(wcsPoint.Y);
                                break;
                        }
                    }
                    switch (this.CamParam.CaliParam.CalculateMethod)
                    {
                        default:
                        case enCalculateMethod.拟合圆:
                            CalibrateMethod.Instance.CalculateFitCenter(list_x.ToArray(), list_y.ToArray(), out Center_x, out Center_y);
                            rotaeAngle = this.CamParam.CaliParam.AngleRange / this.CamParam.CaliParam.AngleStep;
                            break;
                        case enCalculateMethod.两点夹角:
                            CalibrateMethod.Instance.CalculateCenter(list_x.ToArray(), list_y.ToArray(), this.CamParam.CaliParam.AngleRange / this.CamParam.CaliParam.AngleStep, out Center_x, out Center_y, out error);
                            rotaeAngle = this.CamParam.CaliParam.AngleRange / this.CamParam.CaliParam.AngleStep;
                            break;
                        case enCalculateMethod.矩阵变换:
                            CalibrateMethod.Instance.CalculateCenterHomMatWcs(list_x.ToArray(), list_y.ToArray(), this.CamParam.CaliParam.AngleRange / this.CamParam.CaliParam.AngleStep, out Center_x, out Center_y, out error);
                            rotaeAngle = this.CamParam.CaliParam.AngleRange / this.CamParam.CaliParam.AngleStep;
                            break;
                        case enCalculateMethod.N点矩阵变换:
                            CalibrateMethod.Instance.CalculateCenterHomMatWcsNpoint(list_x.ToArray(), list_y.ToArray(), this.calibCoordConfigParamRota.CalibCoordParamList.Count, out Center_x, out Center_y, out error,out rotaeAngle);
                            break;
                    }
                    this.CamParam.CaliParam.CalibCenterXy = new userWcsPoint(Center_x, Center_y, 0);
                    // 标定完成
                    MessageBox.Show("标定完成,最大旋转标定误差：" + error.ToString("f3") + ";圆心X = " + Center_x.ToString("f3") + ";圆心Y = " + Center_y.ToString("f3") + ";步长角 = " + rotaeAngle.ToString("f4") + ";标定矩阵:" + this.CamParam.HomMat2D.ToString());
                    break;
                case enCalibMethod.仅N点平移:
                    this.CurCaliStateEnum = enCaliStateEnum.Cali9Pt;
                    this.CalibNPointPixList?.Clear();
                    this.CamParam.CaliParam.CalibCenterXy = new userWcsPoint(0, 0, 0); // 在开始标定前，必需将旋转中心的置0
                    CommunicationConfigParamManger.Instance.WriteValue(this.CamParam.CaliParam.CoordSysName, enCommunicationCommand.RotCali, 0);
                    CommunicationConfigParamManger.Instance.WriteValue(this.CamParam.CaliParam.CoordSysName, enCommunicationCommand.Cali9Pt, 1);
                    if (this.calibCoordConfigParamNPoint.CalibCoordParamList.Count == 0)
                    {
                        this.toolStripButton_Run.Enabled = true;
                        this.toolStripButton_Stop.Enabled = false;
                        MessageBox.Show("没有可供移动的坐标点!!!");
                        return;
                    }
                    foreach (var item in this.calibCoordConfigParamNPoint.CalibCoordParamList)
                    {
                        if (this.isStop) return;
                        Application.DoEvents();
                        // 等待轴运动到位
                        switch (this.CamParam.CaliParam.CoordValueType)
                        {
                            case enCoordValueType.绝对坐标:
                                MotionCardManage.GetCard(this.CamParam.CaliParam.CoordSysName).MoveMultyAxis(this.CamParam.CaliParam.CoordSysName,
                                                         enAxisName.Compensation_XYTheta轴, 10, new CoordSysAxisParam(item.X, item.Y, item.Z, item.Theta, 0, 0));
                                break;
                            case enCoordValueType.相对坐标:
                                MotionCardManage.GetCard(this.CamParam.CaliParam.CoordSysName).GetAxisPosition(this.CamParam.CaliParam.CoordSysName, enAxisName.X轴, out X);
                                MotionCardManage.GetCard(this.CamParam.CaliParam.CoordSysName).GetAxisPosition(this.CamParam.CaliParam.CoordSysName, enAxisName.Y轴, out Y);
                                MotionCardManage.GetCard(this.CamParam.CaliParam.CoordSysName).GetAxisPosition(this.CamParam.CaliParam.CoordSysName, enAxisName.Theta轴, out Theta);
                                MotionCardManage.GetCard(this.CamParam.CaliParam.CoordSysName).MoveMultyAxis(this.CamParam.CaliParam.CoordSysName,
                                                         enAxisName.Compensation_XYTheta轴, 10, new CoordSysAxisParam((item.X - X), (item.Y - Y), 0, (item.Theta - Theta), 0, 0)); // 写入的是补偿轴地址
                                break;
                        }
                        LoggerHelper.Info("等待轴运动到指定位置:" + "X = " + item.X.ToString() + "  Y = " + item.Y.ToString() + "  Theta = " + item.Theta.ToString());
                        CommunicationConfigParamManger.Instance.WriteValue(this.CamParam.CaliParam.CoordSysName, enCommunicationCommand.ResultToPlc, 3);
                        CommunicationConfigParamManger.Instance.WriteValue(this.CamParam.CaliParam.CoordSysName, enCommunicationCommand.FunctionNoToPlc, 5);
                        CommunicationConfigParamManger.Instance.WriteValue(this.CamParam.CaliParam.CoordSysName, enCommunicationCommand.TriggerToPlc, 1);      
                        // 等待PLC触发采图
                        while (true)
                        {
                            if (this.isStop) return; // 控制标定停止
                            Application.DoEvents();
                            object value = CommunicationConfigParamManger.Instance.ReadValue(this.CamParam.CaliParam.CoordSysName, enCommunicationCommand.TriggerFromPlc);
                            if (value != null && value.ToString() == "1")
                            {
                                MotionCardManage.GetCard(this.CamParam.CaliParam.CoordSysName).GetAxisPosition(this.CamParam.CaliParam.CoordSysName, enAxisName.X轴, out X);
                                MotionCardManage.GetCard(this.CamParam.CaliParam.CoordSysName).GetAxisPosition(this.CamParam.CaliParam.CoordSysName, enAxisName.Y轴, out Y);
                                MotionCardManage.GetCard(this.CamParam.CaliParam.CoordSysName).GetAxisPosition(this.CamParam.CaliParam.CoordSysName, enAxisName.Theta轴, out Theta);
                                if (Math.Abs(item.X - X) > 0.05 || Math.Abs(item.Y - Y) > 0.05 || Math.Abs(item.Theta - Theta) > 0.1)
                                {
                                    MessageBox.Show("PLC 没有运动到指定位置，请确认是否到限位或轴是否有移动!");
                                    CommunicationConfigParamManger.Instance.WriteValue(this.CamParam.CaliParam.CoordSysName, enCommunicationCommand.ResultToPlc, 2);
                                    CommunicationConfigParamManger.Instance.WriteValue(this.CamParam.CaliParam.CoordSysName, enCommunicationCommand.FunctionNoToPlc, 5);
                                    CommunicationConfigParamManger.Instance.WriteValue(this.CamParam.CaliParam.CoordSysName, enCommunicationCommand.TriggerToPlc, 1);
                                    this.toolStripButton_Run.Enabled = true;
                                    this.toolStripButton_Stop.Enabled = false;
                                    return;
                                }
                                else
                                    break;
                            }
                            LoggerHelper.Info("等待PLC触发信号!!!");
                        }
                        Thread.Sleep(100);
                        node = this.GetExecuteNode(this.CamParam.CaliParam.CoordSysName, out info);
                        LoggerHelper.Info("接收PLC触发信号!!!" + info);
                        if (node != null)
                            ((IFunction)node.Tag)?.Execute(node);
                        else
                        {
                            this.toolStripButton_Run.Enabled = true;
                            this.toolStripButton_Stop.Enabled = false;
                            MessageBox.Show("没有获取到可执行的节点!!");
                            return;
                        }
                        CommunicationConfigParamManger.Instance.WriteValue(this.CamParam.CaliParam.CoordSysName, enCommunicationCommand.TriggerFromPlc, 0);
                    }
                    // 标定完成
                    this.toolStripButton_Run.Enabled = true;
                    this.toolStripButton_Stop.Enabled = false;
                    CommunicationConfigParamManger.Instance.WriteValue(this.CamParam.CaliParam.CoordSysName, enCommunicationCommand.ResultToPlc, 1);
                    CommunicationConfigParamManger.Instance.WriteValue(this.CamParam.CaliParam.CoordSysName, enCommunicationCommand.FunctionNoToPlc, 5);
                    CommunicationConfigParamManger.Instance.WriteValue(this.CamParam.CaliParam.CoordSysName, enCommunicationCommand.TriggerToPlc, 1);
                    error = 0;
                    CalibrateMethod.Instance.PixPointToHtuple(this.CalibNPointPixList, out rows, out cols);
                    CalibrateMethod.Instance.WcsPointToHtuple(this.calibCoordConfigParamNPoint.CalibCoordParamList, out x, out y);
                    x -= (x.TupleMax() + x.TupleMin()) * 0.5;
                    y -= (y.TupleMax() + y.TupleMin()) * 0.5;
                    switch (this.CamParam.CaliParam.CalibAxis)
                    {
                        default:
                        case enCalibAxis.XY轴:
                            if (this.CamParam.CaliParam.InvertX)
                                x *= -1;
                            if (this.CamParam.CaliParam.InvertY)
                                y *= -1;
                            this.CamParam.HomMat2D = CalibrateMethod.Instance.NpointCalib(rows, cols, x, y, out error); // 更新了矩阵
                            break;
                        case enCalibAxis.单轴:
                        case enCalibAxis.X轴:
                        case enCalibAxis.Y轴:
                            this.CamParam.HomMat2D = CalibrateMethod.Instance.NpointCalibSingleAxis(rows, cols, x, y, this.CamParam.CaliParam.InvertX, this.CamParam.CaliParam.InvertY, out error); // 更新了矩阵
                            break;
                    }
                    this.MoveOriginToImageCenter();
                    MessageBox.Show("标定误差" + error.ToString() + "标定矩阵:" + this.CamParam.HomMat2D.ToString());
                    break;
                case enCalibMethod.标定板N点标定:
                    this.CurCaliStateEnum = enCaliStateEnum.Cali9Pt;
                    this.CalibNPointPixList?.Clear();
                    this.CamParam.CaliParam.CalibCenterXy = new userWcsPoint(0, 0, 0); // 在开始标定前，必需将旋转中心的置0
                    node = this.GetExecuteNode(this.CamParam.CaliParam.CoordSysName, out info);
                    LoggerHelper.Info("接收PLC触发信号!!!" + info);
                    if (node != null)
                        ((IFunction)node.Tag)?.Execute(node);
                    else
                    {
                        this.toolStripButton_Run.Enabled = true;
                        this.toolStripButton_Stop.Enabled = false;
                        MessageBox.Show("没有获取到可执行的节点!!");
                        return;
                    }
                    // 执行标定
                    CalibrateMethod.Instance.PixPointToHtuple(this.CalibNPointPixList, out rows, out cols);
                    CalibrateMethod.Instance.WcsPointToHtuple(this.calibCoordConfigParamNPoint.CalibCoordParamList, out x, out y);
                    this.CamParam.HomMat2D = CalibrateMethod.Instance.NpointCalib(rows, cols, x, y, out error); // 更新了矩阵
                    this.MoveOriginToImageCenter();
                    MessageBox.Show("最大标定误差" + error.ToString("f3") + "标定矩阵:" + this.CamParam.HomMat2D.ToString());
                    break;
            }
            //// 计算图像4个角点的坐标，验证标定矩阵的方向是否正确
            int width = 0, height = 0;
            HTuple wcs_x, wcs_y, wcs_z;
            this.drawObject.BackImage?.Image?.GetImageSize(out width, out height);
            HTuple row = new HTuple(0, 0, height, height);
            HTuple col = new HTuple(0, width, 0, width);
            this.CamParam.ImagePointsToWorldPlane(row, col, 0, 0, 0, out wcs_x, out wcs_y, out wcs_z);
            this.世界坐标dataGridView.Rows.Clear();
            for (int i = 0; i < wcs_x.Length; i++)
            {
                this.世界坐标dataGridView.Rows.Add(wcs_x[i].D, wcs_y[i].D);
            }
            ///////////////////////
            this.drawObject.AddViewObject(new ViewData(new userWcsCoordSystem(this.CamParam)));
        }

        private void CalibSocketNew()
        {
            string info;
            TreeNode node;
            double X, Y, Z, Theta;
            double center_Row = 0, center_Col = 0, error = 0, rotateAngle = 0;
            this.listGrabTheta.Clear();
            this.listGrabPoint.Clear();
            List<double> list_RotaRows = new List<double>();
            List<double> list_RotaCols = new List<double>();
            List<double> list_NpointRows = new List<double>();
            List<double> list_NpointCols = new List<double>();
            List<double> list_x = new List<double>();
            List<double> list_y = new List<double>();
            List<double> list_z = new List<double>();
            List<double> list_Rx = new List<double>();
            List<double> list_Ry = new List<double>();
            //// 初始化触发信号并等待触发信号 
            CommunicationConfigParamManger.Instance.WriteValue(this.CamParam.CaliParam.CoordSysName, enCommunicationCommand.TriggerFromPlc, 0);
            while (true)
            {
                if (this.isStop) return; // 控制标定停止
                Application.DoEvents();
                object value = CommunicationConfigParamManger.Instance.ReadValue(this.CamParam.CaliParam.CoordSysName, enCommunicationCommand.TriggerFromPlc);//CommunicationConfigParamManger.Instance.GetCommunicationParam(
                if (value != null && value.ToString() == "1")
                {
                    CommunicationConfigParamManger.Instance.WriteValue(this.CamParam.CaliParam.CoordSysName, enCommunicationCommand.TriggerFromPlc, 0);
                    break;
                }
                LoggerHelper.Info("等待PLC触发信号!!!");
            }
            switch (this.CamParam.CaliParam.CalibMethod)
            {
                case enCalibMethod.先旋转后平移:
                    // 先执行旋转
                    this.CurCaliStateEnum = enCaliStateEnum.RotCali;
                    this.CalibRotaPixList?.Clear();
                    this.CamParam.CaliParam.CalibCenterXy = new userWcsPoint(0, 0, 0); // 这里要置0
                    CommunicationConfigParamManger.Instance.WriteValue(this.CamParam.CaliParam.CoordSysName, enCommunicationCommand.RotCali, 1);
                    CommunicationConfigParamManger.Instance.WriteValue(this.CamParam.CaliParam.CoordSysName, enCommunicationCommand.Cali9Pt, 0);
                    if (this.calibCoordConfigParamRota.CalibCoordParamList.Count == 0)
                    {
                        this.toolStripButton_Run.Enabled = true;
                        this.toolStripButton_Stop.Enabled = false;
                        MessageBox.Show("没有可供旋转的坐标点!!");
                        return;
                    }
                    foreach (var item in this.calibCoordConfigParamRota.CalibCoordParamList)
                    {
                        if (this.isStop) return; // 控制标定停止
                        Application.DoEvents();
                        // 等待轴运动到位
                        switch (this.CamParam.CaliParam.CoordValueType)
                        {
                            default:
                            case enCoordValueType.绝对坐标:
                                MotionCardManage.GetCard(this.CamParam.CaliParam.CoordSysName).MoveMultyAxis(this.CamParam.CaliParam.CoordSysName,
                                                         enAxisName.XYTheta轴, 10, new CoordSysAxisParam(item.X, item.Y, item.Z, item.Theta, 0, 0));
                                break;
                            case enCoordValueType.相对坐标:
                                MotionCardManage.GetCard(this.CamParam.CaliParam.CoordSysName).GetAxisPosition(this.CamParam.CaliParam.CoordSysName, enAxisName.X轴, out X);
                                MotionCardManage.GetCard(this.CamParam.CaliParam.CoordSysName).GetAxisPosition(this.CamParam.CaliParam.CoordSysName, enAxisName.Y轴, out Y);
                                MotionCardManage.GetCard(this.CamParam.CaliParam.CoordSysName).GetAxisPosition(this.CamParam.CaliParam.CoordSysName, enAxisName.Theta轴, out Theta);
                                MotionCardManage.GetCard(this.CamParam.CaliParam.CoordSysName).MoveMultyAxis(this.CamParam.CaliParam.CoordSysName,
                                                         enAxisName.XYTheta轴, 10, new CoordSysAxisParam(item.X - X, item.Y - Y, 0, item.Theta - Theta, 0, 0));
                                break;
                        }
                        Thread.Sleep(100);
                        LoggerHelper.Info("等待轴运动到指定位置:" + "X = " + item.X.ToString("f3") + "  Y = " + item.Y.ToString("f3") + "  Theta = " + item.Theta.ToString("f3"));
                        CommunicationConfigParamManger.Instance.WriteValue(this.CamParam.CaliParam.CoordSysName, enCommunicationCommand.ResultToPlc, "Continue");
                        CommunicationConfigParamManger.Instance.WriteValue(this.CamParam.CaliParam.CoordSysName, enCommunicationCommand.TriggerToPlc, 1); // 表示完成
                        // 等待PLC触发采图
                        while (true)
                        {
                            if (this.isStop) return; // 控制标定停止
                            Application.DoEvents();
                            object value = CommunicationConfigParamManger.Instance.ReadValue(this.CamParam.CaliParam.CoordSysName, enCommunicationCommand.TriggerFromPlc);
                            if (value != null && value.ToString() == "1")
                            {
                                MotionCardManage.GetCard(this.CamParam.CaliParam.CoordSysName).GetAxisPosition(this.CamParam.CaliParam.CoordSysName, enAxisName.X轴, out X);
                                MotionCardManage.GetCard(this.CamParam.CaliParam.CoordSysName).GetAxisPosition(this.CamParam.CaliParam.CoordSysName, enAxisName.Y轴, out Y);
                                MotionCardManage.GetCard(this.CamParam.CaliParam.CoordSysName).GetAxisPosition(this.CamParam.CaliParam.CoordSysName, enAxisName.Theta轴, out Theta);
                                if (Math.Abs(item.X - X) > 0.05 || Math.Abs(item.Y - Y) > 0.05 || Math.Abs(item.Theta - Theta) > 0.1)
                                {
                                    this.toolStripButton_Run.Enabled = true;
                                    this.toolStripButton_Stop.Enabled = false;
                                    MessageBox.Show("PLC 没有运动到指定位置，请确认是否到限位或轴是否有移动!");
                                    CommunicationConfigParamManger.Instance.WriteValue(this.CamParam.CaliParam.CoordSysName, enCommunicationCommand.ResultToPlc, "NG");
                                    CommunicationConfigParamManger.Instance.WriteValue(this.CamParam.CaliParam.CoordSysName, enCommunicationCommand.TriggerToPlc, 1);
                                    return;
                                }
                                else
                                    break;
                            }
                            LoggerHelper.Info("等待PLC触发信号!!!");
                        }
                        Thread.Sleep(200);
                        node = this.GetExecuteNode(this.CamParam.CaliParam.CoordSysName, out info);
                        LoggerHelper.Info("接收PLC触发信号!!!" + info);
                        if (node != null)
                            ((IFunction)node.Tag)?.Execute(node);
                        else
                        {
                            this.toolStripButton_Run.Enabled = true;
                            this.toolStripButton_Stop.Enabled = false;
                            MessageBox.Show("没有获取到可执行的节点!!");
                            return;
                        }
                    }
                    // 旋转完后，计算旋转中心坐标
                    List<double> rowsList = new List<double>();
                    List<double> colsList = new List<double>();
                    foreach (var item in this.CalibRotaPixList)
                    {
                        rowsList.Add(item.Row);
                        colsList.Add(item.Col);
                    }
                    switch (this.CamParam.CaliParam.CalculateMethod)
                    {
                        default:
                        case enCalculateMethod.拟合圆:
                            CalibrateMethod.Instance.CalculateFitCenter(rowsList.ToArray(), colsList.ToArray(), out center_Row, out center_Col);
                            rotateAngle = this.CamParam.CaliParam.AngleRange / this.CamParam.CaliParam.AngleStep;
                            break;
                        case enCalculateMethod.两点夹角:
                            CalibrateMethod.Instance.CalculateCenter(rowsList.ToArray(), colsList.ToArray(), this.CamParam.CaliParam.AngleRange / this.CamParam.CaliParam.AngleStep, out center_Row, out center_Col, out error);
                            rotateAngle = this.CamParam.CaliParam.AngleRange / this.CamParam.CaliParam.AngleStep;
                            break;
                        case enCalculateMethod.矩阵变换:
                            CalibrateMethod.Instance.CalculateCenterHomMatPix(rowsList.ToArray(), colsList.ToArray(), (this.CamParam.CaliParam.AngleRange / this.CamParam.CaliParam.AngleStep) * Math.PI / 180, out center_Row, out center_Col, out error);
                            rotateAngle = this.CamParam.CaliParam.AngleRange / this.CamParam.CaliParam.AngleStep;
                            break;
                        case enCalculateMethod.N点矩阵变换:
                            CalibrateMethod.Instance.CalculateCenterHomMatPixNpoint(rowsList.ToArray(), colsList.ToArray(), this.calibCoordConfigParamRota.CalibCoordParamList.Count, out center_Row, out center_Col, out error,out rotateAngle);
                            break;
                    }
                    MessageBox.Show("旋转中心:" + "Row = " + center_Row.ToString("f3") + ";  Col = " + center_Col.ToString("f3") + ";  步长角 = " + center_Col.ToString("f3"));
                    // 再执行平移
                    this.CurCaliStateEnum = enCaliStateEnum.Cali9Pt;
                    this.CalibNPointPixList?.Clear();
                    CommunicationConfigParamManger.Instance.WriteValue(this.CamParam.CaliParam.CoordSysName, enCommunicationCommand.RotCali, 0);
                    CommunicationConfigParamManger.Instance.WriteValue(this.CamParam.CaliParam.CoordSysName, enCommunicationCommand.Cali9Pt, 1);
                    if (this.calibCoordConfigParamNPoint.CalibCoordParamList.Count == 0)
                    {
                        this.toolStripButton_Run.Enabled = true;
                        this.toolStripButton_Stop.Enabled = false;
                        MessageBox.Show("没有可供移动的坐标点!!!");
                        return;
                    }
                    foreach (var item in this.calibCoordConfigParamNPoint.CalibCoordParamList)
                    {
                        if (this.isStop) return; // 控制标定停止
                        Application.DoEvents();
                        switch (this.CamParam.CaliParam.CoordValueType)
                        {
                            default:
                            case enCoordValueType.绝对坐标:
                                MotionCardManage.GetCard(this.CamParam.CaliParam.CoordSysName).MoveMultyAxis(this.CamParam.CaliParam.CoordSysName,
                                                         enAxisName.XY轴, 10, new CoordSysAxisParam(item.X, item.Y, item.Z, item.Theta, 0, 0));
                                break;
                            case enCoordValueType.相对坐标:
                                MotionCardManage.GetCard(this.CamParam.CaliParam.CoordSysName).GetAxisPosition(this.CamParam.CaliParam.CoordSysName, enAxisName.X轴, out X);
                                MotionCardManage.GetCard(this.CamParam.CaliParam.CoordSysName).GetAxisPosition(this.CamParam.CaliParam.CoordSysName, enAxisName.Y轴, out Y);
                                MotionCardManage.GetCard(this.CamParam.CaliParam.CoordSysName).GetAxisPosition(this.CamParam.CaliParam.CoordSysName, enAxisName.Theta轴, out Theta);
                                MotionCardManage.GetCard(this.CamParam.CaliParam.CoordSysName).MoveMultyAxis(this.CamParam.CaliParam.CoordSysName,
                                                         enAxisName.XY轴, 10, new CoordSysAxisParam(item.X - X, item.Y - Y, 0, item.Theta - Theta, 0, 0));
                                break;
                        }
                        Thread.Sleep(100);
                        LoggerHelper.Info("等待轴运动到指定位置:" + "X = " + item.X.ToString("f3") + "  Y = " + item.Y.ToString("f3") + "  Theta = " + item.Theta.ToString("f3"));
                        CommunicationConfigParamManger.Instance.WriteValue(this.CamParam.CaliParam.CoordSysName, enCommunicationCommand.ResultToPlc, "Continue");
                        CommunicationConfigParamManger.Instance.WriteValue(this.CamParam.CaliParam.CoordSysName, enCommunicationCommand.TriggerToPlc, 1); // 表示完成
                        // 等待PLC触发采图
                        while (true)
                        {
                            if (this.isStop) return; // 控制标定停止
                            Application.DoEvents();
                            object value = CommunicationConfigParamManger.Instance.ReadValue(this.CamParam.CaliParam.CoordSysName, enCommunicationCommand.TriggerFromPlc);
                            if (value != null && value.ToString() == "1")
                            {
                                MotionCardManage.GetCard(this.CamParam.CaliParam.CoordSysName).GetAxisPosition(this.CamParam.CaliParam.CoordSysName, enAxisName.X轴, out X);
                                MotionCardManage.GetCard(this.CamParam.CaliParam.CoordSysName).GetAxisPosition(this.CamParam.CaliParam.CoordSysName, enAxisName.Y轴, out Y);
                                MotionCardManage.GetCard(this.CamParam.CaliParam.CoordSysName).GetAxisPosition(this.CamParam.CaliParam.CoordSysName, enAxisName.Theta轴, out Theta);
                                if (Math.Abs(item.X - X) > 0.05 || Math.Abs(item.Y - Y) > 0.05 || Math.Abs(item.Theta - Theta) > 0.1)
                                {
                                    this.toolStripButton_Run.Enabled = true;
                                    this.toolStripButton_Stop.Enabled = false;
                                    MessageBox.Show("PLC 没有运动到指定位置，请确认是否到限位或轴是否有移动!");
                                    CommunicationConfigParamManger.Instance.WriteValue(this.CamParam.CaliParam.CoordSysName, enCommunicationCommand.ResultToPlc, "NG");
                                    CommunicationConfigParamManger.Instance.WriteValue(this.CamParam.CaliParam.CoordSysName, enCommunicationCommand.TriggerToPlc, 1);
                                    return;
                                }
                                else
                                    break;
                            }
                            LoggerHelper.Info("等待PLC触发信号!!!");
                        }
                        node = this.GetExecuteNode(this.CamParam.CaliParam.CoordSysName, out info);
                        LoggerHelper.Info("接收PLC触发信号!!!" + info);
                        if (node != null)
                            ((IFunction)node.Tag)?.Execute(node);
                        else
                        {
                            this.toolStripButton_Run.Enabled = true;
                            this.toolStripButton_Stop.Enabled = false;
                            MessageBox.Show("没有获取到可执行的节点!!");
                            return;
                        }
                        CommunicationConfigParamManger.Instance.WriteValue(this.CamParam.CaliParam.CoordSysName, enCommunicationCommand.TriggerFromPlc,0);
                    }
                    // 标定完成
                    this.toolStripButton_Run.Enabled = true;
                    this.toolStripButton_Stop.Enabled = false;
                    CommunicationConfigParamManger.Instance.WriteValue(this.CamParam.CaliParam.CoordSysName, enCommunicationCommand.ResultToPlc, "OK");
                    CommunicationConfigParamManger.Instance.WriteValue(this.CamParam.CaliParam.CoordSysName, enCommunicationCommand.TriggerToPlc, 1); // 表示完成
                    HTuple rows, cols;
                    HTuple x, y;
                    CalibrateMethod.Instance.PixPointToHtuple(this.CalibNPointPixList, out rows, out cols);
                    CalibrateMethod.Instance.WcsPointToHtuple(this.calibCoordConfigParamNPoint.CalibCoordParamList, out x, out y);
                    double offset_row = center_Row - (rows.TupleMax() + rows.TupleMin()) * 0.5;
                    double offset_col = center_Col - (cols.TupleMax() + cols.TupleMin()) * 0.5; //  在做9点标定时一定要用旋转得到的圆心 - 9点标定的中心来获取偏移值，理论依据为：旋转中心在标定矩阵里的坐标应该为：x = 0,y = 0 ;  
                    rows += offset_row; // 平移9点到旋转中心
                    cols += offset_col;
                    x = x - (x.TupleMax() + x.TupleMin()) * 0.5;
                    y = y - (y.TupleMax() + y.TupleMin()) * 0.5;
                    switch (this.CamParam.CaliParam.CalibAxis)
                    {
                        default:
                        case enCalibAxis.XY轴:
                            if (this.CamParam.CaliParam.InvertX)
                                x *= -1;
                            if (this.CamParam.CaliParam.InvertY)
                                y *= -1;
                            this.CamParam.HomMat2D = CalibrateMethod.Instance.NpointCalib(rows.DArr, cols.DArr, x.DArr, y.DArr, out error); // 更新了矩阵
                            break;
                        case enCalibAxis.单轴:
                        case enCalibAxis.X轴:
                        case enCalibAxis.Y轴:
                            this.CamParam.HomMat2D = CalibrateMethod.Instance.NpointCalibSingleAxis(rows, cols, x, y, this.CamParam.CaliParam.InvertX, this.CamParam.CaliParam.InvertY, out error); // 更新了矩阵
                            break;
                    }
                    this.CamParam.CaliParam.CalibCenterXy = new userWcsPoint(0, 0, 0); // 这里要置0
                    MessageBox.Show("标定最大误差:" + error.ToString() + " 标定矩阵:" + this.CamParam.HomMat2D.ToString());
                    break;
                case enCalibMethod.先平移后旋转:
                    // 先执行平移
                    this.CurCaliStateEnum = enCaliStateEnum.Cali9Pt;
                    this.CalibNPointPixList?.Clear();
                    this.CamParam.CaliParam.CalibCenterXy = new userWcsPoint(0, 0, 0);  // 这里需要先清零
                    CommunicationConfigParamManger.Instance.WriteValue(this.CamParam.CaliParam.CoordSysName, enCommunicationCommand.RotCali, 0);
                    CommunicationConfigParamManger.Instance.WriteValue(this.CamParam.CaliParam.CoordSysName, enCommunicationCommand.Cali9Pt, 1);
                    if (this.calibCoordConfigParamNPoint.CalibCoordParamList.Count == 0)
                    {
                        this.toolStripButton_Run.Enabled = true;
                        this.toolStripButton_Stop.Enabled = false;
                        MessageBox.Show("没有可供移动的坐标点!!!");
                        return;
                    }
                    foreach (var item in this.calibCoordConfigParamNPoint.CalibCoordParamList)
                    {
                        if (this.isStop) break; // 控制标定停止
                        Application.DoEvents();
                        switch (this.CamParam.CaliParam.CoordValueType)
                        {
                            default:
                            case enCoordValueType.绝对坐标:
                                MotionCardManage.GetCard(this.CamParam.CaliParam.CoordSysName).MoveMultyAxis(this.CamParam.CaliParam.CoordSysName,
                                                         enAxisName.XYTheta轴, 10, new CoordSysAxisParam(item.X, item.Y, item.Z, item.Theta, 0, 0));
                                break;
                            case enCoordValueType.相对坐标:
                                MotionCardManage.GetCard(this.CamParam.CaliParam.CoordSysName).GetAxisPosition(this.CamParam.CaliParam.CoordSysName, enAxisName.X轴, out X);
                                MotionCardManage.GetCard(this.CamParam.CaliParam.CoordSysName).GetAxisPosition(this.CamParam.CaliParam.CoordSysName, enAxisName.Y轴, out Y);
                                MotionCardManage.GetCard(this.CamParam.CaliParam.CoordSysName).GetAxisPosition(this.CamParam.CaliParam.CoordSysName, enAxisName.Theta轴, out Theta);
                                MotionCardManage.GetCard(this.CamParam.CaliParam.CoordSysName).MoveMultyAxis(this.CamParam.CaliParam.CoordSysName,
                                                         enAxisName.XYTheta轴, 10, new CoordSysAxisParam(item.X - X, item.Y - Y, 0, item.Theta - Theta, 0, 0));
                                break;
                        }
                        Thread.Sleep(100);
                        LoggerHelper.Info("等待轴运动到指定位置:" + "X = " + item.X.ToString("f3") + "  Y = " + item.Y.ToString("f3") + "  Theta = " + item.Theta.ToString("f3"));
                        CommunicationConfigParamManger.Instance.WriteValue(this.CamParam.CaliParam.CoordSysName, enCommunicationCommand.ResultToPlc, "Continue");
                        CommunicationConfigParamManger.Instance.WriteValue(this.CamParam.CaliParam.CoordSysName, enCommunicationCommand.TriggerToPlc, 1); // 表示完成
                        // 等待PLC触发采图
                        while (true)
                        {
                            if (this.isStop) return; // 控制标定停止
                            Application.DoEvents();
                            object value = CommunicationConfigParamManger.Instance.ReadValue(this.CamParam.CaliParam.CoordSysName, enCommunicationCommand.TriggerFromPlc);
                            if (value != null && value.ToString() == "1")
                            {
                                MotionCardManage.GetCard(this.CamParam.CaliParam.CoordSysName).GetAxisPosition(this.CamParam.CaliParam.CoordSysName, enAxisName.X轴, out X);
                                MotionCardManage.GetCard(this.CamParam.CaliParam.CoordSysName).GetAxisPosition(this.CamParam.CaliParam.CoordSysName, enAxisName.Y轴, out Y);
                                MotionCardManage.GetCard(this.CamParam.CaliParam.CoordSysName).GetAxisPosition(this.CamParam.CaliParam.CoordSysName, enAxisName.Theta轴, out Theta);
                                if (Math.Abs(item.X - X) > 0.05 || Math.Abs(item.Y - Y) > 0.05 || Math.Abs(item.Theta - Theta) > 0.1)
                                {
                                    this.toolStripButton_Run.Enabled = true;
                                    this.toolStripButton_Stop.Enabled = false;
                                    MessageBox.Show("PLC 没有运动到指定位置，请确认是否到限位或轴是否有移动!");
                                    CommunicationConfigParamManger.Instance.WriteValue(this.CamParam.CaliParam.CoordSysName, enCommunicationCommand.ResultToPlc, "NG");
                                    CommunicationConfigParamManger.Instance.WriteValue(this.CamParam.CaliParam.CoordSysName, enCommunicationCommand.TriggerToPlc, 1);
                                    return;
                                }
                                else
                                    break;
                            }
                            LoggerHelper.Info("等待PLC触发信号!!!");
                        }
                        node = this.GetExecuteNode(this.CamParam.CaliParam.CoordSysName, out info);
                        LoggerHelper.Info("接收PLC触发信号!!!" + info);
                        if (node != null)
                            ((IFunction)node.Tag)?.Execute(node);
                        else
                        {
                            this.toolStripButton_Run.Enabled = true;
                            this.toolStripButton_Stop.Enabled = false;
                            MessageBox.Show("没有获取到可执行的节点!!");
                            return;
                        }
                        CommunicationConfigParamManger.Instance.WriteValue(this.CamParam.CaliParam.CoordSysName, enCommunicationCommand.TriggerFromPlc,0);
                    }
                    // 先执行一次标定，用世界坐标来计算圆心，这样可以得到圆心的世界坐标，再使用世界点加上该值，再执行一次标定
                    error = 0;
                    CalibrateMethod.Instance.PixPointToHtuple(this.CalibNPointPixList, out rows, out cols);
                    CalibrateMethod.Instance.WcsPointToHtuple(this.calibCoordConfigParamNPoint.CalibCoordParamList, out x, out y);
                    x = x - (x.TupleMax() + x.TupleMin()) * 0.5;
                    y = y - (y.TupleMax() + y.TupleMin()) * 0.5;
                    switch (this.CamParam.CaliParam.CalibAxis)
                    {
                        default:
                        case enCalibAxis.XY轴:
                            if (this.CamParam.CaliParam.InvertX)
                                x *= -1;
                            if (this.CamParam.CaliParam.InvertY)
                                y *= -1;
                            this.CamParam.HomMat2D = CalibrateMethod.Instance.NpointCalib(rows.DArr, cols.DArr, x.DArr, y.DArr, out error); // 更新了矩阵
                            break;
                        case enCalibAxis.单轴:
                        case enCalibAxis.X轴:
                        case enCalibAxis.Y轴:
                            this.CamParam.HomMat2D = CalibrateMethod.Instance.NpointCalibSingleAxis(rows, cols, x, y, this.CamParam.CaliParam.InvertX, this.CamParam.CaliParam.InvertY, out error); // 更新了矩阵
                            break;
                    }
                    MessageBox.Show("N点标定误差：" + error.ToString() + ";标定矩阵:" + this.CamParam.HomMat2D.ToString());
                    //this.MoveOriginToImageCenter();
                    // 再执行旋转
                    this.CurCaliStateEnum = enCaliStateEnum.RotCali;
                    //this.CalibRotaPixList?.Clear();
                    this.CalibRotaWcsList?.Clear();
                    CommunicationConfigParamManger.Instance.WriteValue(this.CamParam.CaliParam.CoordSysName, enCommunicationCommand.RotCali, 1);
                    CommunicationConfigParamManger.Instance.WriteValue(this.CamParam.CaliParam.CoordSysName, enCommunicationCommand.Cali9Pt, 0);
                    //////////////////////////////////////////////////////////////
                    if (this.calibCoordConfigParamRota.CalibCoordParamList.Count == 0)
                    {
                        this.toolStripButton_Run.Enabled = true;
                        this.toolStripButton_Stop.Enabled = false;
                        MessageBox.Show("没有可供移动的坐标点!!!");
                        return;
                    }
                    foreach (var item in this.calibCoordConfigParamRota.CalibCoordParamList)
                    {
                        Application.DoEvents();
                        if (this.isStop) return; // 控制标定停止
                        switch (this.CamParam.CaliParam.CoordValueType)
                        {
                            default:
                            case enCoordValueType.绝对坐标:
                                MotionCardManage.GetCard(this.CamParam.CaliParam.CoordSysName).MoveMultyAxis(this.CamParam.CaliParam.CoordSysName,
                                                         enAxisName.XYTheta轴, 10, new CoordSysAxisParam(item.X, item.Y, item.Z, item.Theta, 0, 0));
                                break;
                            case enCoordValueType.相对坐标:
                                MotionCardManage.GetCard(this.CamParam.CaliParam.CoordSysName).GetAxisPosition(this.CamParam.CaliParam.CoordSysName, enAxisName.X轴, out X);
                                MotionCardManage.GetCard(this.CamParam.CaliParam.CoordSysName).GetAxisPosition(this.CamParam.CaliParam.CoordSysName, enAxisName.Y轴, out Y);
                                MotionCardManage.GetCard(this.CamParam.CaliParam.CoordSysName).GetAxisPosition(this.CamParam.CaliParam.CoordSysName, enAxisName.Theta轴, out Theta);
                                MotionCardManage.GetCard(this.CamParam.CaliParam.CoordSysName).MoveMultyAxis(this.CamParam.CaliParam.CoordSysName,
                                                         enAxisName.XYTheta轴, 10, new CoordSysAxisParam(item.X - X, item.Y - Y, 0, item.Theta - Theta, 0, 0));
                                break;
                        }
                        Thread.Sleep(100);
                        LoggerHelper.Info("等待轴运动到指定位置:" + "X = " + item.X.ToString() + "  Y = " + item.Y.ToString() + "  Theta = " + item.Theta.ToString());
                        CommunicationConfigParamManger.Instance.WriteValue(this.CamParam.CaliParam.CoordSysName, enCommunicationCommand.ResultToPlc, "Continue");
                        CommunicationConfigParamManger.Instance.WriteValue(this.CamParam.CaliParam.CoordSysName, enCommunicationCommand.TriggerToPlc, 1); // 表示完成
                        // 等待PLC触发采图
                        while (true)
                        {
                            if (this.isStop) return; // 控制标定停止
                            Application.DoEvents();
                            object value = CommunicationConfigParamManger.Instance.ReadValue(this.CamParam.CaliParam.CoordSysName, enCommunicationCommand.TriggerFromPlc);
                            if (value != null && value.ToString() == "1")
                            {
                                MotionCardManage.GetCard(this.CamParam.CaliParam.CoordSysName).GetAxisPosition(this.CamParam.CaliParam.CoordSysName, enAxisName.X轴, out X);
                                MotionCardManage.GetCard(this.CamParam.CaliParam.CoordSysName).GetAxisPosition(this.CamParam.CaliParam.CoordSysName, enAxisName.Y轴, out Y);
                                MotionCardManage.GetCard(this.CamParam.CaliParam.CoordSysName).GetAxisPosition(this.CamParam.CaliParam.CoordSysName, enAxisName.Theta轴, out Theta);
                                if (Math.Abs(item.X - X) > 0.05 || Math.Abs(item.Y - Y) > 0.05 || Math.Abs(item.Theta - Theta) > 0.1)
                                {
                                    this.toolStripButton_Run.Enabled = true;
                                    this.toolStripButton_Stop.Enabled = false;
                                    MessageBox.Show("PLC 没有运动到指定位置，请确认是否到限位或轴是否有移动!");
                                    CommunicationConfigParamManger.Instance.WriteValue(this.CamParam.CaliParam.CoordSysName, enCommunicationCommand.ResultToPlc, "NG");
                                    CommunicationConfigParamManger.Instance.WriteValue(this.CamParam.CaliParam.CoordSysName, enCommunicationCommand.TriggerToPlc, 1);
                                    return;
                                }
                                else
                                    break;
                            }
                            LoggerHelper.Info("等待PLC触发信号!!!");
                        }
                        node = this.GetExecuteNode(this.CamParam.CaliParam.CoordSysName, out info);
                        LoggerHelper.Info("接收PLC触发信号!!!" + info);
                        if (node != null)
                            ((IFunction)node.Tag)?.Execute(node);
                        else
                        {
                            this.toolStripButton_Run.Enabled = true;
                            this.toolStripButton_Stop.Enabled = false;
                            MessageBox.Show("没有获取到可执行的节点!!");
                            return;
                        }
                        CommunicationConfigParamManger.Instance.WriteValue(this.CamParam.CaliParam.CoordSysName, enCommunicationCommand.TriggerFromPlc,0);
                    }
                    // 标定完成
                    this.toolStripButton_Run.Enabled = true;
                    this.toolStripButton_Stop.Enabled = false;
                    CommunicationConfigParamManger.Instance.WriteValue(this.CamParam.CaliParam.CoordSysName, enCommunicationCommand.ResultToPlc, "OK");
                    CommunicationConfigParamManger.Instance.WriteValue(this.CamParam.CaliParam.CoordSysName, enCommunicationCommand.TriggerToPlc, 1); // 表示完成
                    // 旋转完一次后，执行计算标定矩阵
                    double Center_x = 0, Center_y = 0;
                    list_x = new List<double>();
                    list_y = new List<double>();
                    foreach (var item in this.CalibRotaWcsList)
                    {
                        switch (this.CamParam.CaliParam.CoordOriginType)
                        {
                            case enCoordOriginType.机械原点:
                                list_x.Add(item.X);
                                list_y.Add(item.Y);
                                break;
                            default:
                            case enCoordOriginType.旋转中心:
                            case enCoordOriginType.IsLoading:
                                userWcsPoint wcsPoint = item.GetWcsCamPoint(); // 这种模式下使用相机坐标系下的点
                                list_x.Add(wcsPoint.X);
                                list_y.Add(wcsPoint.Y);
                                break;
                        }
                    }
                    switch (this.CamParam.CaliParam.CalculateMethod)
                    {
                        default:
                        case enCalculateMethod.拟合圆:
                            CalibrateMethod.Instance.CalculateFitCenter(list_x.ToArray(), list_y.ToArray(), out Center_x, out Center_y);
                            rotateAngle = this.CamParam.CaliParam.AngleRange / this.CamParam.CaliParam.AngleStep;
                            break;
                        case enCalculateMethod.两点夹角:
                            CalibrateMethod.Instance.CalculateCenter(list_x.ToArray(), list_y.ToArray(), this.CamParam.CaliParam.AngleRange / this.CamParam.CaliParam.AngleStep, out Center_x, out Center_y, out error);
                            rotateAngle = this.CamParam.CaliParam.AngleRange / this.CamParam.CaliParam.AngleStep;
                            break;
                        case enCalculateMethod.矩阵变换:
                            CalibrateMethod.Instance.CalculateCenterHomMatWcs(list_x.ToArray(), list_y.ToArray(), this.CamParam.CaliParam.AngleRange / this.CamParam.CaliParam.AngleStep, out Center_x, out Center_y, out error);
                            rotateAngle = this.CamParam.CaliParam.AngleRange / this.CamParam.CaliParam.AngleStep;
                            break;
                        case enCalculateMethod.N点矩阵变换:
                            CalibrateMethod.Instance.CalculateCenterHomMatWcsNpoint(list_x.ToArray(), list_y.ToArray(), this.calibCoordConfigParamRota.CalibCoordParamList.Count, out Center_x, out Center_y, out error, out rotateAngle);
                            break;
                    }
                    this.CamParam.CaliParam.CalibCenterXy = new userWcsPoint(Center_x, Center_y, 0);
                    MessageBox.Show("最大旋转标定误差：" + error.ToString("f3") + "圆心X = " + Center_x.ToString("f3") + "圆心Y = " + Center_y.ToString("f3") + "步长角 = " + rotateAngle.ToString("f3") + "标定矩阵:" + this.CamParam.HomMat2D.ToString());
                    break;
                case enCalibMethod.仅N点平移:
                    this.CurCaliStateEnum = enCaliStateEnum.Cali9Pt;
                    this.CalibNPointPixList?.Clear();
                    CommunicationConfigParamManger.Instance.WriteValue(this.CamParam.CaliParam.CoordSysName, enCommunicationCommand.RotCali, 0);
                    CommunicationConfigParamManger.Instance.WriteValue(this.CamParam.CaliParam.CoordSysName, enCommunicationCommand.Cali9Pt, 1);
                    if (this.calibCoordConfigParamNPoint.CalibCoordParamList.Count == 0)
                    {
                        this.toolStripButton_Run.Enabled = true;
                        this.toolStripButton_Stop.Enabled = false;
                        MessageBox.Show("没有可供移动的坐标点!!!");
                        return;
                    }
                    foreach (var item in this.calibCoordConfigParamNPoint.CalibCoordParamList)
                    {
                        if (this.isStop) return; // 控制标定停止
                        Application.DoEvents();
                        MotionCardManage.GetCard(this.CamParam.CaliParam.CoordSysName).GetAxisPosition(this.CamParam.CaliParam.CoordSysName, enAxisName.X轴, out X);
                        MotionCardManage.GetCard(this.CamParam.CaliParam.CoordSysName).GetAxisPosition(this.CamParam.CaliParam.CoordSysName, enAxisName.Y轴, out Y);
                        MotionCardManage.GetCard(this.CamParam.CaliParam.CoordSysName).GetAxisPosition(this.CamParam.CaliParam.CoordSysName, enAxisName.Theta轴, out Theta);
                        switch (this.CamParam.CaliParam.CoordValueType)
                        {
                            default:
                            case enCoordValueType.绝对坐标:
                                MotionCardManage.GetCard(this.CamParam.CaliParam.CoordSysName).MoveMultyAxis(this.CamParam.CaliParam.CoordSysName,
                                                         enAxisName.XY轴, 10, new CoordSysAxisParam(item.X, item.Y, item.Z, item.Theta, 0, 0));
                                break;
                            case enCoordValueType.相对坐标:
                                MotionCardManage.GetCard(this.CamParam.CaliParam.CoordSysName).MoveMultyAxis(this.CamParam.CaliParam.CoordSysName,
                                                         enAxisName.XY轴, 10, new CoordSysAxisParam(item.X - X, item.Y - Y, 0, item.Theta - Theta, 0, 0));
                                break;
                        }
                        Thread.Sleep(100);
                        LoggerHelper.Info("等待轴运动到指定位置:" + "X = " + item.X.ToString() + "  Y = " + item.Y.ToString() + "  Theta = " + item.Theta.ToString());
                        CommunicationConfigParamManger.Instance.WriteValue(this.CamParam.CaliParam.CoordSysName, enCommunicationCommand.ResultToPlc, "Continue");
                        CommunicationConfigParamManger.Instance.WriteValue(this.CamParam.CaliParam.CoordSysName, enCommunicationCommand.TriggerToPlc, 1); // 表示完成
                        // 等待PLC触发采图
                        while (true)
                        {
                            if (this.isStop) return; // 控制标定停止
                            Application.DoEvents();
                            object value = CommunicationConfigParamManger.Instance.ReadValue(this.CamParam.CaliParam.CoordSysName, enCommunicationCommand.TriggerFromPlc);
                            if (value != null && value.ToString() == "1")
                            {
                                MotionCardManage.GetCard(this.CamParam.CaliParam.CoordSysName).GetAxisPosition(this.CamParam.CaliParam.CoordSysName, enAxisName.X轴, out X);
                                MotionCardManage.GetCard(this.CamParam.CaliParam.CoordSysName).GetAxisPosition(this.CamParam.CaliParam.CoordSysName, enAxisName.Y轴, out Y);
                                MotionCardManage.GetCard(this.CamParam.CaliParam.CoordSysName).GetAxisPosition(this.CamParam.CaliParam.CoordSysName, enAxisName.Theta轴, out Theta);
                                if (Math.Abs(item.X - X) > 0.05 || Math.Abs(item.Y - Y) > 0.05 || Math.Abs(item.Theta - Theta) > 0.1)
                                {
                                    this.toolStripButton_Run.Enabled = true;
                                    this.toolStripButton_Stop.Enabled = false;
                                    MessageBox.Show("PLC 没有运动到指定位置，请确认是否到限位或轴是否有移动!");
                                    CommunicationConfigParamManger.Instance.WriteValue(this.CamParam.CaliParam.CoordSysName, enCommunicationCommand.ResultToPlc, "NG");
                                    CommunicationConfigParamManger.Instance.WriteValue(this.CamParam.CaliParam.CoordSysName, enCommunicationCommand.TriggerToPlc, 1);
                                    return;
                                }
                                else
                                    break;
                            }
                            LoggerHelper.Info("等待PLC触发信号!!!");
                        }
                        node = this.GetExecuteNode(this.CamParam.CaliParam.CoordSysName, out info);
                        LoggerHelper.Info("接收PLC触发信号!!!" + info);
                        if (node != null)
                            ((IFunction)node.Tag)?.Execute(node);
                        else
                        {
                            this.toolStripButton_Run.Enabled = true;
                            this.toolStripButton_Stop.Enabled = false;
                            MessageBox.Show("没有获取到可执行的节点!!");
                            return;
                        }
                        CommunicationConfigParamManger.Instance.WriteValue(this.CamParam.CaliParam.CoordSysName, enCommunicationCommand.TriggerFromPlc,0);
                    }
                    this.toolStripButton_Run.Enabled = true;
                    this.toolStripButton_Stop.Enabled = false;
                    CommunicationConfigParamManger.Instance.WriteValue(this.CamParam.CaliParam.CoordSysName, enCommunicationCommand.ResultToPlc, "OK");
                    CommunicationConfigParamManger.Instance.WriteValue(this.CamParam.CaliParam.CoordSysName, enCommunicationCommand.TriggerToPlc, 1); // 表示完成
                    error = 0;
                    CalibrateMethod.Instance.PixPointToHtuple(this.CalibNPointPixList, out rows, out cols);
                    CalibrateMethod.Instance.WcsPointToHtuple(this.calibCoordConfigParamNPoint.CalibCoordParamList, out x, out y);
                    x = x - (x.TupleMax() + x.TupleMin()) * 0.5;
                    y = y - (y.TupleMax() + y.TupleMin()) * 0.5;
                    switch (this.CamParam.CaliParam.CalibAxis)
                    {
                        default:
                        case enCalibAxis.XY轴:
                            if (this.CamParam.CaliParam.InvertX)
                                x *= -1;
                            if (this.CamParam.CaliParam.InvertY)
                                y *= -1;
                            this.CamParam.HomMat2D = CalibrateMethod.Instance.NpointCalib(rows.DArr, cols.DArr, x.DArr, y.DArr, out error); // 更新了矩阵
                            break;
                        case enCalibAxis.单轴:
                        case enCalibAxis.X轴:
                        case enCalibAxis.Y轴:
                            this.CamParam.HomMat2D = CalibrateMethod.Instance.NpointCalibSingleAxis(rows, cols, x, y, this.CamParam.CaliParam.InvertX, this.CamParam.CaliParam.InvertY, out error); // 更新了矩阵
                            break;
                    }
                    this.MoveOriginToImageCenter();
                    MessageBox.Show("标定误差" + error.ToString() + ";标定矩阵:" + this.CamParam.HomMat2D.ToString());
                    break;
                case enCalibMethod.标定板N点标定:
                    this.CurCaliStateEnum = enCaliStateEnum.Cali9Pt;
                    this.CalibNPointPixList?.Clear();
                    node = this.GetExecuteNode(this.CamParam.CaliParam.CoordSysName, out info);
                    LoggerHelper.Info("接收PLC触发信号!!!" + info);
                    if (node != null)
                        ((IFunction)node.Tag)?.Execute(node);
                    else
                    {
                        this.toolStripButton_Run.Enabled = true;
                        this.toolStripButton_Stop.Enabled = false;
                        MessageBox.Show("没有获取到可执行的节点!!");
                        return;
                    }
                    // 执行标定
                    this.toolStripButton_Run.Enabled = true;
                    this.toolStripButton_Stop.Enabled = false;
                    CalibrateMethod.Instance.PixPointToHtuple(this.CalibNPointPixList, out rows, out cols);
                    CalibrateMethod.Instance.WcsPointToHtuple(this.calibCoordConfigParamNPoint.CalibCoordParamList, out x, out y);
                    x = x - (x.TupleMax() + x.TupleMin()) * 0.5;
                    y = y - (y.TupleMax() + y.TupleMin()) * 0.5;
                    this.CamParam.HomMat2D = CalibrateMethod.Instance.NpointCalib(rows.DArr, cols.DArr, x.DArr, y.DArr, out error); // 更新了矩阵
                    this.MoveOriginToImageCenter();
                    MessageBox.Show("最大标定误差" + error.ToString() + ";标定矩阵:" + this.CamParam.HomMat2D.ToString());
                    break;
            }
            //// 计算图像4个角点的坐标，验证标定矩阵的方向是否正确
            int width = 0, height = 0;
            HTuple wcs_x, wcs_y, wcs_z;
            this.drawObject.BackImage?.Image?.GetImageSize(out width, out height);
            HTuple row = new HTuple(0, 0, height, height);
            HTuple col = new HTuple(0, width, 0, width);
            this.CamParam.ImagePointsToWorldPlane(row, col, 0, 0, 0, out wcs_x, out wcs_y, out wcs_z);

            this.世界坐标dataGridView.Rows.Clear();
            for (int i = 0; i < wcs_x.Length; i++)
            {
                this.世界坐标dataGridView.Rows.Add(wcs_x[i].D, wcs_y[i].D);
            }
            GetWcsPoint(this.CalibNPointPixList, this.CamParam, out wcs_x, out wcs_y);
            this.拍照点世界坐标dataGridView.Rows.Clear();
            for (int i = 0; i < wcs_x.Length; i++)
            {
                this.拍照点世界坐标dataGridView.Rows.Add(wcs_x[i].D + this.listGrabPoint[i].X, wcs_y[i].D + this.listGrabPoint[i].Y);
            }
            ///////////////////////
            this.drawObject.AddViewObject(new ViewData(new userWcsCoordSystem(this.CamParam)));
        }
        private void CalibSocketNewXZ()
        {
            double X, Y, Z, Theta, U, V;
            double error = 0;
            this.listGrabTheta.Clear();
            this.listGrabPoint.Clear();
            // 先执行旋转
            this.CurCaliStateEnum = enCaliStateEnum.RotCali;
            this.CalibRotaPixList?.Clear();
            this.CalibRotaWcsList?.Clear();
            this.CamParam.CaliParam.CalibCenterXz = new userWcsPoint(0, 0, 0);
            //////////////////////////////////////////////////////////////
            if (this.calibCoordConfigParamRotaV.CalibCoordParamList.Count == 0)
            {
                this.toolStripButton_Run.Enabled = true;
                this.toolStripButton_Stop.Enabled = false;
                MessageBox.Show("没有可供旋转的坐标点!!");
                return;
            }
            foreach (var item in this.calibCoordConfigParamRotaV.CalibCoordParamList)
            {
                Application.DoEvents();
                if (this.isStop) break; // 控制标定停止
                switch (this.CamParam.CaliParam.CoordValueType)
                {
                    default:
                    case enCoordValueType.绝对坐标:
                        MotionCardManage.GetCard(this.CamParam.CaliParam.CoordSysName).MoveMultyAxis(this.CamParam.CaliParam.CoordSysName,
                                                 enAxisName.XY轴, 10, new CoordSysAxisParam(item.X, item.Y, item.Z, item.Theta, item.U, item.V));
                        break;
                    case enCoordValueType.相对坐标:
                        MotionCardManage.GetCard(this.CamParam.CaliParam.CoordSysName).GetAxisPosition(this.CamParam.CaliParam.CoordSysName, enAxisName.X轴, out X);
                        MotionCardManage.GetCard(this.CamParam.CaliParam.CoordSysName).GetAxisPosition(this.CamParam.CaliParam.CoordSysName, enAxisName.Y轴, out Y);
                        MotionCardManage.GetCard(this.CamParam.CaliParam.CoordSysName).GetAxisPosition(this.CamParam.CaliParam.CoordSysName, enAxisName.Z轴, out Z);
                        MotionCardManage.GetCard(this.CamParam.CaliParam.CoordSysName).GetAxisPosition(this.CamParam.CaliParam.CoordSysName, enAxisName.Theta轴, out Theta);
                        MotionCardManage.GetCard(this.CamParam.CaliParam.CoordSysName).GetAxisPosition(this.CamParam.CaliParam.CoordSysName, enAxisName.U轴, out U);
                        MotionCardManage.GetCard(this.CamParam.CaliParam.CoordSysName).GetAxisPosition(this.CamParam.CaliParam.CoordSysName, enAxisName.V轴, out V);
                        MotionCardManage.GetCard(this.CamParam.CaliParam.CoordSysName).MoveMultyAxis(this.CamParam.CaliParam.CoordSysName,
                                                 enAxisName.XY轴, 10, new CoordSysAxisParam(item.X - X, item.Y - Y, item.Z - Z, item.Theta - Theta, item.U - U, item.V - V));
                        break;
                }
                Thread.Sleep(100);
                LoggerHelper.Info("等待轴运动到指定位置:" + "X = " + item.X.ToString("f3") + "  Y = " + item.Y.ToString("f3") + "  Theta = " + item.Theta.ToString("f3"));
                CommunicationConfigParamManger.Instance.WriteValue(this.CamParam.CaliParam.CoordSysName, enCommunicationCommand.ResultToPlc, "Continue");
                CommunicationConfigParamManger.Instance.WriteValue(this.CamParam.CaliParam.CoordSysName, enCommunicationCommand.TriggerToPlc, 1); // 表示完成
                // 等待PLC触发采图
                while (true)
                {
                    if (this.isStop) break; // 控制标定停止
                    Application.DoEvents();
                    object value = CommunicationConfigParamManger.Instance.ReadValue(this.CamParam.CaliParam.CoordSysName, enCommunicationCommand.TriggerFromPlc);
                    if (value != null && value.ToString() == "1")
                    {
                        MotionCardManage.GetCard(this.CamParam.CaliParam.CoordSysName).GetAxisPosition(this.CamParam.CaliParam.CoordSysName, enAxisName.X轴, out X);
                        MotionCardManage.GetCard(this.CamParam.CaliParam.CoordSysName).GetAxisPosition(this.CamParam.CaliParam.CoordSysName, enAxisName.Y轴, out Y);
                        MotionCardManage.GetCard(this.CamParam.CaliParam.CoordSysName).GetAxisPosition(this.CamParam.CaliParam.CoordSysName, enAxisName.Theta轴, out Theta);
                        if (Math.Abs(item.X - X) > 0.05 || Math.Abs(item.Y - Y) > 0.05 || Math.Abs(item.Theta - Theta) > 0.1)
                        {
                            MessageBox.Show("PLC 没有运动到指定位置，请确认是否到限位或轴是否有移动!");
                            CommunicationConfigParamManger.Instance.WriteValue(this.CamParam.CaliParam.CoordSysName, enCommunicationCommand.ResultToPlc, "NG");
                            CommunicationConfigParamManger.Instance.WriteValue(this.CamParam.CaliParam.CoordSysName, enCommunicationCommand.TriggerToPlc, 1);
                            return;
                        }
                        else
                            break;
                    }
                    LoggerHelper.Info("等待PLC触发信号!!!");
                }
                string info;
                TreeNode node = this.GetExecuteNode(this.CamParam.CaliParam.CoordSysName, out info);
                LoggerHelper.Info("接收PLC触发信号!!!" + info);
                if (node != null)
                    ((IFunction)node.Tag)?.Execute(node);
                else
                {
                    this.toolStripButton_Run.Enabled = true;
                    this.toolStripButton_Stop.Enabled = false;
                    MessageBox.Show("没有获取到可执行的节点!!");
                    return;
                }
            }
            // 标定完成
            this.toolStripButton_Run.Enabled = true;
            this.toolStripButton_Stop.Enabled = false;
            CommunicationConfigParamManger.Instance.WriteValue(this.CamParam.CaliParam.CoordSysName, enCommunicationCommand.ResultToPlc, "OK");
            CommunicationConfigParamManger.Instance.WriteValue(this.CamParam.CaliParam.CoordSysName, enCommunicationCommand.TriggerToPlc, 1); // 表示完成
            // 旋转完一次后，执行计算标定矩阵
            double Center_x = 0, Center_z = 0, radius = 0;
            List<double> list_x = new List<double>();
            List<double> list_y = new List<double>();
            List<double> list_z = new List<double>();
            List<double> list_angle = new List<double>();
            foreach (var item in this.CalibRotaWcsList) // 这里获得的是
            {
                list_x.Add(item.X);
                list_y.Add(item.Y);
                list_z.Add(item.Z);
            }
            foreach (var item in this.calibCoordConfigParamRotaV.CalibCoordParamList) // 这里获得的是
            {
                list_angle.Add(item.V);
            }
            switch (this.CamParam.CaliParam.CalculateMethod)
            {
                default:
                case enCalculateMethod.拟合圆:
                    CalibrateMethod.Instance.CalculateCenterXzPlane(list_x.ToArray(), list_z.ToArray(), list_angle.ToArray(), out Center_x, out Center_z, out radius);
                    break;
                case enCalculateMethod.两点夹角:
                    CalibrateMethod.Instance.CalculateCenterWcsXzPlane(list_x.ToArray(), list_z.ToArray(), list_angle.ToArray(), out Center_x, out Center_z, out error);
                    break;
                case enCalculateMethod.矩阵变换:
                    CalibrateMethod.Instance.CalculateCenterHomMatWcsXzPlane(list_x.ToArray(), list_z.ToArray(), list_angle.ToArray(), out Center_x, out Center_z, out error);
                    break;
            }
            this.CamParam.CaliParam.CalibCenterXz = new userWcsPoint(Center_x, this.CamParam.CaliParam.CalibCenterXy.Y, Center_z);
            MessageBox.Show("旋转中心:" + " X = " + Center_x.ToString() + "  Z = " + Center_z.ToString());
        }
        private void CalibSocketNewYZ()
        {
            double X, Y, Z, Theta, U, V;
            double error = 0;
            this.listGrabTheta.Clear();
            this.listGrabPoint.Clear();
            // 先执行旋转
            this.CurCaliStateEnum = enCaliStateEnum.RotCali;
            this.CalibRotaPixList?.Clear();
            this.CalibRotaWcsList?.Clear();
            this.CamParam.CaliParam.CalibCenterXz = new userWcsPoint(0, 0, 0);
            //////////////////////////////////////////////////////////////
            if (this.calibCoordConfigParamRotaU.CalibCoordParamList.Count == 0)
            {
                this.toolStripButton_Run.Enabled = true;
                this.toolStripButton_Stop.Enabled = false;
                MessageBox.Show("没有可供旋转的坐标点!!!");
                return;
            }
            foreach (var item in this.calibCoordConfigParamRotaU.CalibCoordParamList)
            {
                Application.DoEvents();
                if (this.isStop) return; // 控制标定停止
                switch (this.CamParam.CaliParam.CoordValueType)
                {
                    default:
                    case enCoordValueType.绝对坐标:
                        MotionCardManage.GetCard(this.CamParam.CaliParam.CoordSysName).MoveMultyAxis(this.CamParam.CaliParam.CoordSysName,
                                                 enAxisName.XY轴, 10, new CoordSysAxisParam(item.X, item.Y, item.Z, item.Theta, item.U, item.V));
                        break;
                    case enCoordValueType.相对坐标:
                        MotionCardManage.GetCard(this.CamParam.CaliParam.CoordSysName).GetAxisPosition(this.CamParam.CaliParam.CoordSysName, enAxisName.X轴, out X);
                        MotionCardManage.GetCard(this.CamParam.CaliParam.CoordSysName).GetAxisPosition(this.CamParam.CaliParam.CoordSysName, enAxisName.Y轴, out Y);
                        MotionCardManage.GetCard(this.CamParam.CaliParam.CoordSysName).GetAxisPosition(this.CamParam.CaliParam.CoordSysName, enAxisName.Z轴, out Z);
                        MotionCardManage.GetCard(this.CamParam.CaliParam.CoordSysName).GetAxisPosition(this.CamParam.CaliParam.CoordSysName, enAxisName.Theta轴, out Theta);
                        MotionCardManage.GetCard(this.CamParam.CaliParam.CoordSysName).GetAxisPosition(this.CamParam.CaliParam.CoordSysName, enAxisName.U轴, out U);
                        MotionCardManage.GetCard(this.CamParam.CaliParam.CoordSysName).GetAxisPosition(this.CamParam.CaliParam.CoordSysName, enAxisName.V轴, out V);
                        MotionCardManage.GetCard(this.CamParam.CaliParam.CoordSysName).MoveMultyAxis(this.CamParam.CaliParam.CoordSysName,
                                                 enAxisName.XY轴, 10, new CoordSysAxisParam(item.X - X, item.Y - Y, item.Z - Z, item.Theta - Theta, item.U - U, item.V - V));
                        break;
                }
                Thread.Sleep(100);
                LoggerHelper.Info("等待轴运动到指定位置:" + "X = " + item.X.ToString("f3") + "  Y = " + item.Y.ToString("f3") + "  Theta = " + item.Theta.ToString("f3"));
                CommunicationConfigParamManger.Instance.WriteValue(this.CamParam.CaliParam.CoordSysName, enCommunicationCommand.ResultToPlc, "Continue");
                CommunicationConfigParamManger.Instance.WriteValue(this.CamParam.CaliParam.CoordSysName, enCommunicationCommand.TriggerToPlc, 1); // 表示完成
                // 等待PLC触发采图
                while (true)
                {
                    if (this.isStop) return; // 控制标定停止
                    Application.DoEvents();
                    object value = CommunicationConfigParamManger.Instance.ReadValue(this.CamParam.CaliParam.CoordSysName, enCommunicationCommand.TriggerFromPlc);
                    if (value != null && value.ToString() == "1")
                    {
                        MotionCardManage.GetCard(this.CamParam.CaliParam.CoordSysName).GetAxisPosition(this.CamParam.CaliParam.CoordSysName, enAxisName.X轴, out X);
                        MotionCardManage.GetCard(this.CamParam.CaliParam.CoordSysName).GetAxisPosition(this.CamParam.CaliParam.CoordSysName, enAxisName.Y轴, out Y);
                        MotionCardManage.GetCard(this.CamParam.CaliParam.CoordSysName).GetAxisPosition(this.CamParam.CaliParam.CoordSysName, enAxisName.Theta轴, out Theta);
                        if (Math.Abs(item.X - X) > 0.05 || Math.Abs(item.Y - Y) > 0.05 || Math.Abs(item.Theta - Theta) > 0.1)
                        {
                            MessageBox.Show("PLC 没有运动到指定位置，请确认是否到限位或轴是否有移动!");
                            CommunicationConfigParamManger.Instance.WriteValue(this.CamParam.CaliParam.CoordSysName, enCommunicationCommand.ResultToPlc, "NG");
                            CommunicationConfigParamManger.Instance.WriteValue(this.CamParam.CaliParam.CoordSysName, enCommunicationCommand.TriggerToPlc, 1);
                            return;
                        }
                        else
                            break;
                    }
                    LoggerHelper.Info("等待PLC触发信号!!!");
                }
                Thread.Sleep(100);
                string info;
                TreeNode node = this.GetExecuteNode(this.CamParam.CaliParam.CoordSysName, out info);
                LoggerHelper.Info("接收PLC触发信号!!!" + info);
                if (node != null)
                    ((IFunction)node.Tag)?.Execute(node);
                else
                {
                    this.toolStripButton_Run.Enabled = true;
                    this.toolStripButton_Stop.Enabled = false;
                    MessageBox.Show("没有获取到可执行的节点!!");
                    return;
                }
            }
            this.toolStripButton_Run.Enabled = true;
            this.toolStripButton_Stop.Enabled = false;
            CommunicationConfigParamManger.Instance.WriteValue(this.CamParam.CaliParam.CoordSysName, enCommunicationCommand.ResultToPlc, "OK");
            CommunicationConfigParamManger.Instance.WriteValue(this.CamParam.CaliParam.CoordSysName, enCommunicationCommand.TriggerToPlc, 1); // 表示完成
            // 旋转完一次后，执行计算标定矩阵
            double Center_x = 0, Center_y = 0, Center_z = 0, radius = 0;
            List<double> list_x = new List<double>();
            List<double> list_y = new List<double>();
            List<double> list_z = new List<double>();
            List<double> list_angle = new List<double>();
            foreach (var item in this.CalibRotaWcsList) // 这里获得的是
            {
                list_x.Add(item.X);
                list_y.Add(item.Y);
                list_z.Add(item.Z);
            }
            foreach (var item in this.calibCoordConfigParamRotaU.CalibCoordParamList) // 这里获得的是
            {
                list_angle.Add(item.U);
            }
            switch (this.CamParam.CaliParam.CalculateMethod)
            {
                default:
                case enCalculateMethod.拟合圆:
                    CalibrateMethod.Instance.CalculateCenterXzPlane(list_y.ToArray(), list_z.ToArray(), list_angle.ToArray(), out Center_y, out Center_z, out radius);
                    break;
                case enCalculateMethod.两点夹角:
                    CalibrateMethod.Instance.CalculateCenterWcsXzPlane(list_y.ToArray(), list_z.ToArray(), list_angle.ToArray(), out Center_y, out Center_z, out error);
                    break;
                case enCalculateMethod.矩阵变换:
                    CalibrateMethod.Instance.CalculateCenterHomMatWcsXzPlane(list_y.ToArray(), list_z.ToArray(), list_angle.ToArray(), out Center_y, out Center_z, out error);
                    break;
            }
            this.CamParam.CaliParam.CalibCenterXz = new userWcsPoint(this.CamParam.CaliParam.CalibCenterXy.X, Center_y, Center_z);
            MessageBox.Show("旋转中心:" + " Y = " + Center_y.ToString("f4") + "  Z = " + Center_z.ToString("f4"));
        }
        public void GetWcsPoint(BindingList<userPixPoint> pixPoint, CameraParam camParam, out HTuple Qx, out HTuple Qy)
        {
            Qx = new HTuple();
            Qy = new HTuple();
            if (pixPoint == null || pixPoint.Count == 0) return;
            if (camParam == null) return;
            double wcs_x, wcs_y, wcs_z;
            foreach (var item in pixPoint)
            {
                camParam.ImagePointsToWorldPlane(item.Row, item.Col, 0, 0, 0, out wcs_x, out wcs_y, out wcs_z);
                Qx.Append(wcs_x);
                Qy.Append(wcs_y);
            }
        }

        private HXLDCont GenCoordSystem(int widthImage, int heightImage)
        {

            HXLDCont hXLDCont = new HXLDCont();
            double length_x = this.CamParam.TransPixLengthToWcsLength(widthImage);
            double length_y = this.CamParam.TransPixLengthToWcsLength(heightImage);
            double centerRow, centerCol, row1, col1, row2, col2;
            this.CamParam.WorldPointsToImagePlane(0, 0, 0, 0, out centerRow, out centerCol);
            this.CamParam.WorldPointsToImagePlane(length_x * 0.25, 0, 0, 0, out row1, out col1);
            this.CamParam.WorldPointsToImagePlane(0, length_y * 0.25, 0, 0, out row2, out col2);

            HXLDCont aroowX = GenArrowContourXld(centerRow, centerCol, row1, col1, 15, 15);
            HXLDCont aroowY = GenArrowContourXld(centerRow, centerCol, row2, col2, 15, 15);
            if (!hXLDCont.IsInitialized())
                hXLDCont.GenEmptyObj();
            hXLDCont = hXLDCont.ConcatObj(aroowX).ConcatObj(aroowY);
            return hXLDCont;
        }
        private HXLDCont GenArrowContourXld(HTuple Row1, HTuple Column1, HTuple Row2, HTuple Column2, double HeadLength, double HeadWidth)
        {
            if (Row1.Length != Row2.Length) return new HXLDCont();
            HXLDCont arrows = new HXLDCont();
            arrows.GenEmptyObj();
            HTuple Length = HMisc.DistancePp(Row1, Column1, Row2, Column2);
            HTuple ZeroLengthIndices = Length.TupleFind(0);
            if (ZeroLengthIndices != -1)
                Length[ZeroLengthIndices] = -1;
            // Calculate auxiliary variables.
            HTuple DR = 1.0 * (Row2 - Row1) / Length;
            HTuple DC = 1.0 * (Column2 - Column1) / Length;
            HTuple HalfHeadWidth = HeadWidth / 2.0;
            // Calculate end points of the arrow head.
            HTuple RowP1 = Row1 + (Length - HeadLength) * DR + HalfHeadWidth * DC;
            HTuple ColP1 = Column1 + (Length - HeadLength) * DC - HalfHeadWidth * DR;
            HTuple RowP2 = Row1 + (Length - HeadLength) * DR - HalfHeadWidth * DC;
            HTuple ColP2 = Column1 + (Length - HeadLength) * DC + HalfHeadWidth * DR;
            // Finally create output XLD contour for each input point pair
            for (int Index = 0; Index < Length.Length; Index++)
            {
                if (Length[Index].D == -1)
                    //Create_ single points for arrows with identical start and end point
                    arrows = arrows.ConcatObj(new HXLDCont(Row1[Index], Column1[Index]));
                else
                    // Create arrow contour
                    arrows = arrows.ConcatObj(new HXLDCont(new HTuple(Row1[Index].D, Row2[Index].D, RowP1[Index].D, Row2[Index].D, RowP2[Index].D, Row2[Index].D), new HTuple(Column1[Index].D, Column2[Index].D, ColP1[Index].D, Column2[Index].D, ColP2[Index].D, Column2[Index].D)));
            }
            return arrows;
        }


        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            ToolForm tool;
            switch (e.ClickedItem.Text)
            {
                case "检测工具":
                    tool = new ToolForm(this._treeViewWrapClassNPoint);
                    tool.Owner = this;
                    tool.Show();
                    break;
                case "执行":
                    switch (this.运控平台comboBox.Text)
                    {
                        case "PLC":
                            this.isStop = false;
                            this.toolStripButton_Run.Enabled = false;
                            this.toolStripButton_Stop.Enabled = true;
                            switch (this.CamParam.CaliParam.CalibPlane)
                            {
                                default:
                                case enCalibPlane.XY:
                                    this.CalibPlc();
                                    break;
                                case enCalibPlane.XZ:

                                    break;
                            }
                            break;
                        case "Socket":
                            this.isStop = false;
                            this.toolStripButton_Run.Enabled = false;
                            this.toolStripButton_Stop.Enabled = true;
                            switch (this.CamParam.CaliParam.CalibPlane)
                            {
                                default:
                                case enCalibPlane.XY:
                                    this.CalibSocketNew();
                                    break;
                                case enCalibPlane.XZ:
                                    this.CalibSocketNewXZ();
                                    break;
                                case enCalibPlane.YZ:
                                    this.CalibSocketNewYZ();
                                    break;
                            }
                            break;
                    }
                    break;

                case "停止":
                    this.isStop = true;
                    this.toolStripButton_Run.Enabled = true;
                    this.toolStripButton_Stop.Enabled = false;
                    this._treeViewWrapClassNPoint.Stop();
                    break;

                case "打开":
                    this.programPath = new FileOperate().OpenFolder();
                    if (this.programPath == null || this.programPath.Trim().Length == 0) return;
                    this._treeViewWrapClassNPoint.OpenProgram(this.programPath); //+ "\\" + this.CamParam.SensorName
                    break;

                case "保存":
                    if (this.programPath == null || this.programPath.Length == 0)
                    {
                        if (this.programPath == null || this.programPath.Length == 0) return;
                        if (this._treeViewWrapClassNPoint.SaveProgram(this.programPath + "\\" + this.CamParam.SensorName))
                            MessageBox.Show("保存成功");
                        else
                            MessageBox.Show("保存失败" + new Exception().ToString());
                    }
                    else
                    {
                        if (this._treeViewWrapClassNPoint.SaveProgram(this.programPath + "\\" + this.CamParam.SensorName)) //+ "\\" + this.CamParam.SensorName
                            MessageBox.Show("保存成功");
                        else
                            MessageBox.Show("保存失败" + new Exception().ToString());
                    }
                    break;

            }
        }

        private void 旋转坐标dataGridView_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            try
            {
                for (int i = 0; i < this.C轴旋转坐标dataGridView.Rows.Count; i++)
                {
                    this.C轴旋转坐标dataGridView.Rows[i].HeaderCell.Value = (i + 1).ToString();
                }
            }
            catch
            {

            }
        }

        private void N点坐标dataGridView_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            try
            {
                for (int i = 0; i < this.N点坐标dataGridView.Rows.Count; i++)
                {
                    this.N点坐标dataGridView.Rows[i].HeaderCell.Value = (i + 1).ToString();
                }
            }
            catch
            {

            }
        }

        private void Cam9PointCalibrateForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                this.isStop = true; // 关闭窗体停止
                this.drawObject.GrayValueInfo -= new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                BaseFunction.ExcuteCompleted -= new ExcuteCompletedEventHandler(DisplayExcuteResult);
                TreeViewWrapClass.ClickNode -= new ClickNodeEventHandler(this.DisplayClickObject);
                this.calibCoordConfigParamRota.Save(programPath + "\\" + this.CamParam.SensorName + "\\" + "RotaCalibConfigParam.xml");
                this.calibCoordConfigParamRotaU.Save(programPath + "\\" + this.CamParam.SensorName + "\\" + "RotaCalibConfigParamUAxis.xml");
                this.calibCoordConfigParamRotaV.Save(programPath + "\\" + this.CamParam.SensorName + "\\" + "RotaCalibConfigParamVAxis.xml");
                this.calibCoordConfigParamNPoint.Save(programPath + "\\" + this.CamParam.SensorName + "\\" + "NPointCalibConfigParam.xml");
                this._treeViewWrapClassNPoint?.Uinit();
            }
            catch
            {

            }
        }

        private void 生成旋转坐标button_Click(object sender, EventArgs e)
        {
            try
            {
                int result = -1;
                this.calibCoordConfigParamRota.CalibCoordParamList.Clear();
                Math.DivRem((int)this.CamParam.CaliParam.AngleStep, 2, out result);
                if (result == 1) this.CamParam.CaliParam.AngleStep += 1;   // 让旋转步数为奇数
                this.旋转步数numericUpDown.Value = (decimal)this.CamParam.CaliParam.AngleStep;
                switch (this.旋转方向comboBox.Text)
                {
                    default:
                    case "双向":
                        for (int i = (int)((this.CamParam.CaliParam.AngleStep) * -0.5); i <= (int)((this.CamParam.CaliParam.AngleStep) * 0.5); i++)
                        {
                            this.calibCoordConfigParamRota.CalibCoordParamList.Add(new CoordSysAxisParam(
                                this.CamParam.CaliParam.RotateCalibPoint.X,
                                this.CamParam.CaliParam.RotateCalibPoint.Y,
                                this.CamParam.CaliParam.RotateCalibPoint.Z,
                                this.CamParam.CaliParam.RotateCalibPoint.Angle + i * this.CamParam.CaliParam.AngleRange * 1.0 / this.CamParam.CaliParam.AngleStep
                                //this.CamParam.CaliParam.RotCenter.Angle - this.CamParam.CaliParam.AngleRange + i * this.CamParam.CaliParam.AngleRange * 2 / this.CamParam.CaliParam.AngleStep
                                ));
                        }
                        break;
                    case "正向":
                        for (int i = 0; i <= (int)((this.CamParam.CaliParam.AngleStep)); i++)
                        {
                            this.calibCoordConfigParamRota.CalibCoordParamList.Add(new CoordSysAxisParam(
                                this.CamParam.CaliParam.RotateCalibPoint.X,
                                this.CamParam.CaliParam.RotateCalibPoint.Y,
                                this.CamParam.CaliParam.RotateCalibPoint.Z,
                                this.CamParam.CaliParam.RotateCalibPoint.Angle + i * this.CamParam.CaliParam.AngleRange * 1.0 / this.CamParam.CaliParam.AngleStep
                                //this.CamParam.CaliParam.RotCenter.Angle - this.CamParam.CaliParam.AngleRange + i * this.CamParam.CaliParam.AngleRange * 2 / this.CamParam.CaliParam.AngleStep
                                ));
                        }
                        break;
                    case "负向":
                        for (int i = 0; i >= (int)((this.CamParam.CaliParam.AngleStep) * -1); i--)
                        {
                            this.calibCoordConfigParamRota.CalibCoordParamList.Add(new CoordSysAxisParam(
                                this.CamParam.CaliParam.RotateCalibPoint.X,
                                this.CamParam.CaliParam.RotateCalibPoint.Y,
                                this.CamParam.CaliParam.RotateCalibPoint.Z,
                                this.CamParam.CaliParam.RotateCalibPoint.Angle + i * this.CamParam.CaliParam.AngleRange * 1.0 / this.CamParam.CaliParam.AngleStep
                                //this.CamParam.CaliParam.RotCenter.Angle - this.CamParam.CaliParam.AngleRange + i * this.CamParam.CaliParam.AngleRange * 2 / this.CamParam.CaliParam.AngleStep
                                ));
                        }
                        break;
                }
                /////////  用绝对坐标来表示
                for (int i = 0; i < this.C轴旋转坐标dataGridView.Rows.Count; i++)
                {
                    this.C轴旋转坐标dataGridView.Rows[i].HeaderCell.Value = (i + 1).ToString();
                }

                double value = this.CamParam.CaliParam.AngleRange / this.CamParam.CaliParam.AngleStep;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void 生成N点坐标button_Click(object sender, EventArgs e)
        {
            try
            {
                this.calibCoordConfigParamNPoint.CalibCoordParamList.Clear(); // 默认为 3行3列 9个点标定
                int rowCount = (int)this.行数numericUpDown.Value;
                int colCount = (int)this.列数numericUpDown.Value;
                double step_x = (this.CamParam.CaliParam.EndCalibPoint.X - this.CamParam.CaliParam.StartCaliPoint.X) / (colCount - 1);
                double step_y = (this.CamParam.CaliParam.EndCalibPoint.Y - this.CamParam.CaliParam.StartCaliPoint.Y) / (rowCount - 1);
                for (int i = 0; i < rowCount; i++)
                {
                    for (int j = 0; j < colCount; j++)
                    {
                        this.calibCoordConfigParamNPoint.CalibCoordParamList.Add(new CoordSysAxisParam(
                            this.CamParam.CaliParam.StartCaliPoint.X + j * step_x,
                            this.CamParam.CaliParam.StartCaliPoint.Y + i * step_y,
                            this.CamParam.CaliParam.StartCaliPoint.Z,
                            this.CamParam.CaliParam.StartCaliPoint.Angle));
                    }
                }
                for (int i = 0; i < this.N点坐标dataGridView.Rows.Count; i++)
                {
                    this.N点坐标dataGridView.Rows[i].HeaderCell.Value = (i + 1).ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void 角度范围numericUpDown_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.isLoad)
                    this.CamParam.CaliParam.AngleRange = (double)this.角度范围numericUpDown.Value;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void 旋转步数numericUpDown_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.isLoad)
                    this.CamParam.CaliParam.AngleStep = (double)this.旋转步数numericUpDown.Value;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }


        #region 右键菜单项
        private void addContextMenu(DataGridView dataGridView)
        {
            ContextMenuStrip ContextMenuStrip1 = new ContextMenuStrip();
            ContextMenuStrip1.Name = dataGridView.Name;
            // 添加右键菜单 
            ToolStripItem[] items = new ToolStripMenuItem[]
            {
                new ToolStripMenuItem("删除"),
                new ToolStripMenuItem("清空"),
                new ToolStripMenuItem("矩形阵列"),
                new ToolStripMenuItem("圆形阵列"),
            };
            ContextMenuStrip1.Items.AddRange(items); // 在滑赋值前不能调用 
            ContextMenuStrip1.ItemClicked += new ToolStripItemClickedEventHandler(dataGridViewContextMenuStrip_ItemClicked);
            dataGridView.ContextMenuStrip = ContextMenuStrip1;
        }
        private void dataGridViewContextMenuStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            string name = e.ClickedItem.Text;
            try
            {
                int index = 0;
                ((ContextMenuStrip)sender).Close();
                switch (name)
                {
                    case "删除":
                        ((ContextMenuStrip)sender).Close();
                        switch (((ContextMenuStrip)sender).Name)
                        {
                            case "C轴旋转坐标dataGridView":
                                index = this.C轴旋转坐标dataGridView.CurrentRow.Index;
                                if (index >= 0)
                                    this.calibCoordConfigParamRota.CalibCoordParamList.RemoveAt(index);
                                break;
                            case "U轴旋转dataGridView":
                                index = this.U轴旋转dataGridView.CurrentRow.Index;
                                if (index >= 0)
                                    this.calibCoordConfigParamRotaU.CalibCoordParamList.RemoveAt(index);
                                break;
                            case "V轴旋转dataGridView":
                                index = this.V轴旋转dataGridView.CurrentRow.Index;
                                if (index >= 0)
                                    this.calibCoordConfigParamRotaV.CalibCoordParamList.RemoveAt(index);
                                break;
                            case "N点坐标dataGridView":
                                index = this.N点坐标dataGridView.CurrentRow.Index;
                                if (index >= 0)
                                    this.calibCoordConfigParamNPoint.CalibCoordParamList.RemoveAt(index);
                                break;
                        }
                        break;

                    //////////////////////////////////////
                    case "清空":
                        ((ContextMenuStrip)sender).Close();
                        switch (((ContextMenuStrip)sender).Name)
                        {
                            case "C轴旋转坐标dataGridView":
                                this.calibCoordConfigParamRota.CalibCoordParamList.Clear();
                                break;
                            case "U轴旋转dataGridView":
                                this.calibCoordConfigParamRotaU.CalibCoordParamList.Clear();
                                break;
                            case "V轴旋转dataGridView":
                                this.calibCoordConfigParamRotaV.CalibCoordParamList.Clear();
                                break;
                            case "N点坐标dataGridView":
                                this.calibCoordConfigParamNPoint.CalibCoordParamList.Clear();
                                break;
                        }
                        break;

                    case "矩形阵列":
                        RectangleArrayDataForm rectForm = new RectangleArrayDataForm();
                        rectForm.ShowDialog();
                        switch (((ContextMenuStrip)sender).Name)
                        {
                            case "C轴旋转坐标dataGridView":
                                DataGridViewCellCollection collection = this.C轴旋转坐标dataGridView.CurrentRow.Cells;
                                double X = Convert.ToDouble(collection["X"].Value);
                                double Y = Convert.ToDouble(collection["Y"].Value);
                                double Z = Convert.ToDouble(collection["Z"].Value);
                                double Theta = Convert.ToDouble(collection["Theta"].Value);
                                for (int i = 0; i < rectForm.RowCount; i++)
                                {
                                    for (int j = 0; j < rectForm.ColCount; j++)
                                    {
                                        this.calibCoordConfigParamRota.CalibCoordParamList.Add(new CoordSysAxisParam(X + j * rectForm.OffsetX, Y + i * rectForm.OffsetY, 0, Theta));
                                    }
                                }
                                this.C轴旋转坐标dataGridView.Rows.Remove(this.C轴旋转坐标dataGridView.CurrentRow);// 因为第一个数据重复了，所以这里需要删除悼
                                break;
                            case "N点坐标dataGridView":
                                collection = this.N点坐标dataGridView.CurrentRow.Cells;
                                X = Convert.ToDouble(collection["X"].Value);
                                Y = Convert.ToDouble(collection["Y"].Value);
                                Z = Convert.ToDouble(collection["Z"].Value);
                                Theta = Convert.ToDouble(collection["Theta"].Value);
                                for (int i = 0; i < rectForm.RowCount; i++)
                                {
                                    for (int j = 0; j < rectForm.ColCount; j++)
                                    {
                                        this.calibCoordConfigParamNPoint.CalibCoordParamList.Add(new CoordSysAxisParam(X + j * rectForm.OffsetX, Y + i * rectForm.OffsetY, 0, Theta));
                                    }
                                }
                                this.N点坐标dataGridView.Rows.Remove(this.N点坐标dataGridView.CurrentRow); // 因为第一个数据重复了，所以这里需要删除悼
                                break;
                        }
                        break;

                    case "圆形阵列":
                        CircleArrayDataForm circleForm = new CircleArrayDataForm();
                        circleForm.ShowDialog();
                        switch (((ContextMenuStrip)sender).Name)
                        {
                            case "C轴旋转坐标dataGridView":
                                DataGridViewCellCollection collection = this.C轴旋转坐标dataGridView.CurrentRow.Cells;
                                double X = Convert.ToDouble(collection["X"].Value);
                                double Y = Convert.ToDouble(collection["Y"].Value);
                                double Z = Convert.ToDouble(collection["Z"].Value);
                                double Theta = Convert.ToDouble(collection["Theta"].Value);
                                double Qx = 0, Qy = 0;
                                HHomMat2D hHomMat2D = new HHomMat2D();
                                HHomMat2D HomMat2dRotate = hHomMat2D.HomMat2dRotate(circleForm.Add_Deg * Math.PI / 180, circleForm.Ref_X, circleForm.Ref_Y);
                                for (int i = 0; i < circleForm.ArrayNum; i++)
                                {
                                    HomMat2dRotate = hHomMat2D.HomMat2dRotate(circleForm.Add_Deg * Math.PI / 180 * i, circleForm.Ref_X, circleForm.Ref_Y);
                                    Qx = HomMat2dRotate.AffineTransPoint2d(X, Y, out Qy);
                                    this.calibCoordConfigParamRota.CalibCoordParamList.Add(new CoordSysAxisParam(Qx, Qy, 0, Theta + circleForm.Add_Deg * i));
                                }
                                this.C轴旋转坐标dataGridView.Rows.Remove(this.C轴旋转坐标dataGridView.CurrentRow);
                                break;

                            case "N点坐标dataGridView":
                                collection = this.N点坐标dataGridView.CurrentRow.Cells;
                                X = Convert.ToDouble(collection["X"].Value);
                                Y = Convert.ToDouble(collection["Y"].Value);
                                Z = Convert.ToDouble(collection["Z"].Value);
                                Theta = Convert.ToDouble(collection["Theta"].Value);
                                Qx = 0;
                                Qy = 0;
                                hHomMat2D = new HHomMat2D();
                                HomMat2dRotate = hHomMat2D.HomMat2dRotate(circleForm.Add_Deg * Math.PI / 180, circleForm.Ref_X, circleForm.Ref_Y);
                                for (int i = 0; i < circleForm.ArrayNum; i++)
                                {
                                    HomMat2dRotate = hHomMat2D.HomMat2dRotate(circleForm.Add_Deg * Math.PI / 180 * i, circleForm.Ref_X, circleForm.Ref_Y);
                                    Qx = HomMat2dRotate.AffineTransPoint2d(X, Y, out Qy);
                                    this.calibCoordConfigParamNPoint.CalibCoordParamList.Add(new CoordSysAxisParam(Qx, Qy, 0, Theta + circleForm.Add_Deg * i));
                                }
                                this.N点坐标dataGridView.Rows.Remove(this.N点坐标dataGridView.CurrentRow);
                                break;
                        }
                        break;

                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void addContextMenu(HWindowControl hWindowControl)
        {
            ContextMenuStrip ContextMenuStrip1 = new ContextMenuStrip();
            ContextMenuStrip1.Name = hWindowControl.Name;

            // 添加右键菜单 
            ToolStripItem[] items = new ToolStripMenuItem[]
            {
                new ToolStripMenuItem("执行"),
                new ToolStripMenuItem("设置抓边参数"),
                new ToolStripMenuItem("------------"),
                new ToolStripMenuItem("自适应窗口"),
                new ToolStripMenuItem("清除窗口"),
                new ToolStripMenuItem("保存图像"),
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
                    case "执行":
                        switch (this._currFunction.GetType().Name)
                        {
                            case "CircleMeasure":
                                this._currFunction?.Execute(this.drawObject.GetPixCircleParam());
                                break;
                            case "CircleSectorMeasure":
                                this._currFunction?.Execute(this.drawObject.GetPixCircleSectorParam());
                                break;
                            case "EllipseMeasure":
                                this._currFunction?.Execute(this.drawObject.GetPixEllipseParam());
                                break;
                            case "EllipseSectorMeasure":
                                this._currFunction?.Execute(this.drawObject.GetPixEllipseSectorParam());
                                break;
                            case "LineMeasure":
                                this._currFunction?.Execute(this.drawObject.GetPixLineParam());
                                break;
                            case "PointMeasure":
                                this._currFunction?.Execute(this.drawObject.GetPixLineParam());
                                break;
                            case "Rectangle2Measure":
                                this._currFunction?.Execute(this.drawObject.GetPixRectangle2Param());
                                break;
                            case "WidthMeasure":
                                this._currFunction?.Execute(this.drawObject.GetPixRectangle2Param());
                                break;
                            case "CrossPointMeasure":
                                this._currFunction?.Execute(this.drawObject.GetPixLineParam());
                                break;
                        }
                        break;
                    //////////////////////////////////////
                    case "自适应窗口":
                        this.drawObject?.AutoImage();
                        break;
                    case "清除窗口":
                        this.drawObject?.ClearWindow();
                        this.listData.Clear(); // 清除窗口时,对象也清除
                        break;
                    case "设置抓边参数":
                        MetrolegyParamForm paramForm = new MetrolegyParamForm(this._currFunction, this.drawObject);
                        paramForm.Show();
                        paramForm.Owner = this;
                        break;
                    case "保存图像":
                        ((ContextMenuStrip)sender).Close();
                        SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                        saveFileDialog1.Filter = "bmp files (*.bmp)|*.bmp|All files (*.*)|*.*";
                        saveFileDialog1.FilterIndex = 0;
                        saveFileDialog1.ShowDialog();
                        if (this.drawObject.BackImage != null && this.drawObject.BackImage.Image.IsInitialized())
                            this.drawObject.BackImage.Image.WriteImage("bmp", 0, saveFileDialog1.FileName);
                        else
                            MessageBox.Show("图像内容为空");
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        #endregion

        #region 视图交互
        private void hWindowControl1_MouseMove(object sender, GrayValueInfoEventArgs e)
        {
            if (e.GaryValue.Length > 0)
                this.灰度值1Label.Text = e.GaryValue[0].ToString();
            else
                this.灰度值1Label.Text = 0.ToString();
            ///////////////////////////////////////////
            if (e.GaryValue.Length > 1)
                this.灰度值2Label.Text = e.GaryValue[1].ToString();
            else
                this.灰度值2Label.Text = 0.ToString();
            /////////////////////////////////////////
            if (e.GaryValue.Length > 2)
                this.灰度值3Label.Text = e.GaryValue[2].ToString();
            else
                this.灰度值3Label.Text = 0.ToString();
            ///////////////////////////////////////////////
            this.行坐标Label.Text = e.Row.ToString();
            this.列坐标Label.Text = e.Col.ToString();
        }
        public void DisplayExcuteResult(object sender, ExcuteCompletedEventArgs e)  // 这里一定要加一个键值对变量来收集，以免出现重合
        {
            try
            {
                if (e.DataContent == null) return;// 在图形窗口只显示世界坐标元素，像素元素在图像窗口显示
                switch (e.DataContent.GetType().Name) //这里只接受XLD轮廓或3D对象轮廓
                {
                    case "ImageDataClass":
                        this.drawObject.AttachPropertyData.Clear();
                        this.listData.Clear();
                        this.drawObject.BackImage = (ImageDataClass)e.DataContent;
                        this.CurrentImageData = this.drawObject.BackImage;
                        break;

                    case nameof(userWcsLine):
                        this.drawObject.AttachPropertyData.Clear();
                        if (this.listData.ContainsKey(e.ItemName))
                            this.listData[e.ItemName] = e.DataContent;
                        else
                            this.listData.Add(e.ItemName, e.DataContent);
                        this.drawObject.AttachPropertyData.AddRange(this.listData.Values.ToArray()); 
                        this.drawObject.DetachDrawingObjectFromWindow();
                        break;

                    case "userWcsCircle":
                        this.drawObject.AttachPropertyData.Clear();
                        if (this.listData.ContainsKey(e.ItemName))
                            this.listData[e.ItemName] = e.DataContent;
                        else
                            this.listData.Add(e.ItemName, e.DataContent);
                        this.drawObject.AttachPropertyData.AddRange(this.listData.Values.ToArray()); //(userWcsCircle)e.DataContent
                        userWcsCircle wcsCircle = ((userWcsCircle)e.DataContent);
                        //for (int i = 0; i < wcsCircle.EdgesPoint_xyz.Length; i++)
                        //{
                        //    this.drawObject.AttachPropertyData.Add(new userWcsPoint(wcsCircle.EdgesPoint_xyz[i].X, wcsCircle.EdgesPoint_xyz[i].Y, 0, wcsCircle.CamParams));
                        //}
                        this.drawObject.DetachDrawingObjectFromWindow();
                        /////////////// 记录坐标点 /////////////////
                        switch (this.CurCaliStateEnum)
                        {
                            case enCaliStateEnum.Cali9Pt:
                                this.CalibNPointPixList.Add((new userWcsPoint(wcsCircle.X, wcsCircle.Y, wcsCircle.Z, wcsCircle.Grab_x, wcsCircle.Grab_y, wcsCircle.CamParams)).GetPixPoint());
                                ///////////////////////////////////////////////////////////
                                this.CalibNPointWcsList.Add((new userWcsPoint(wcsCircle.X, wcsCircle.Y, wcsCircle.Z, wcsCircle.Grab_x, wcsCircle.Grab_y, wcsCircle.CamParams)));
                                break;
                            case enCaliStateEnum.RotCali:
                                this.CalibRotaPixList.Add((new userWcsPoint(wcsCircle.X, wcsCircle.Y, wcsCircle.Z, wcsCircle.Grab_x, wcsCircle.Grab_y, wcsCircle.CamParams)).GetPixPoint());
                                ///////////////////////////////////////////////////////////
                                this.CalibRotaWcsList.Add((new userWcsPoint(wcsCircle.X, wcsCircle.Y, wcsCircle.Z, wcsCircle.Grab_x, wcsCircle.Grab_y, wcsCircle.CamParams))); // 放转的坐标，拍照位置一定要置0
                                break;
                        }
                        break;
                    case "userWcsCircleSector":
                        this.drawObject.AttachPropertyData.Clear();
                        if (this.listData.ContainsKey(e.ItemName))
                            this.listData[e.ItemName] = e.DataContent;
                        else
                            this.listData.Add(e.ItemName, e.DataContent);
                        this.drawObject.AttachPropertyData.AddRange(this.listData.Values.ToArray());
                        userWcsCircleSector wcsCircleSector = ((userWcsCircleSector)e.DataContent);
                        //for (int i = 0; i < wcsCircleSector.EdgesPoint_xyz.Length; i++)
                        //{
                        //    this.drawObject.AttachPropertyData.Add(new userWcsPoint(wcsCircleSector.EdgesPoint_xyz[i].X, wcsCircleSector.EdgesPoint_xyz[i].Y, 0, wcsCircleSector.CamParams));
                        //}
                        this.drawObject.DetachDrawingObjectFromWindow();
                        /////////////// 记录坐标点 /////////////////
                        switch (this.CurCaliStateEnum)
                        {
                            case enCaliStateEnum.Cali9Pt:
                                wcsCircleSector = ((userWcsCircleSector)e.DataContent);
                                userPixCircleSector pixCircleSector = ((userWcsCircleSector)e.DataContent).GetPixCircleSector();
                                this.CalibNPointPixList.Add(new userPixPoint(pixCircleSector.Row, pixCircleSector.Col));
                                ///////////////////////////////////////////////////////////
                                this.CalibNPointWcsList.Add(new userWcsPoint(wcsCircleSector.X, wcsCircleSector.Y, wcsCircleSector.Z, wcsCircleSector.Grab_x, wcsCircleSector.Grab_y, wcsCircleSector.CamParams));
                                break;
                            case enCaliStateEnum.RotCali:
                                wcsCircleSector = ((userWcsCircleSector)e.DataContent);
                                pixCircleSector = ((userWcsCircleSector)e.DataContent).GetPixCircleSector();
                                this.CalibRotaPixList.Add(new userPixPoint(pixCircleSector.Row, pixCircleSector.Col));
                                ///////////////////////////////////////////////////////////
                                this.CalibRotaWcsList.Add(new userWcsPoint(wcsCircleSector.X, wcsCircleSector.Y, wcsCircleSector.Z, wcsCircleSector.Grab_x, wcsCircleSector.Grab_y, wcsCircleSector.CamParams)); // 放转拍照位坐标一定要置0
                                break;
                        }
                        break;

                    case "userWcsPoint":
                        this.drawObject.AttachPropertyData.Clear();
                        if (this.listData.ContainsKey(e.ItemName))
                            this.listData[e.ItemName] = e.DataContent;
                        else
                            this.listData.Add(e.ItemName, e.DataContent);
                        this.drawObject.AttachPropertyData.AddRange(this.listData.Values.ToArray()); // 点对象本身就是一个点，所以这里不再考虑显示子元素
                        this.drawObject.DetachDrawingObjectFromWindow();
                        switch (this.CurCaliStateEnum)
                        {
                            case enCaliStateEnum.Cali9Pt:
                                this.CalibNPointPixList.Add(((userWcsPoint)e.DataContent).GetPixPoint());
                                ///////////////////////////////////////////////////////////;
                                this.CalibNPointWcsList.Add(((userWcsPoint)e.DataContent));
                                break;
                            case enCaliStateEnum.RotCali:
                                userWcsPoint wcsPoint = ((userWcsPoint)e.DataContent);
                                this.CalibRotaPixList.Add(wcsPoint.GetPixPoint());
                                ///////////////////////////////////////////////////////////
                                //wcsPoint.Grab_x = 0;
                                //wcsPoint.Grab_y = 0;
                                this.CalibRotaWcsList.Add(wcsPoint);
                                break;
                        }
                        break;

                    case "userWcsRectangle2":
                        this.drawObject.AttachPropertyData.Clear();
                        if (this.listData.ContainsKey(e.ItemName))
                            this.listData[e.ItemName] = e.DataContent;
                        else
                            this.listData.Add(e.ItemName, e.DataContent);
                        this.drawObject.AttachPropertyData.AddRange(this.listData.Values.ToArray());
                        userWcsRectangle2 wcsRect2 = ((userWcsRectangle2)e.DataContent);
                        //for (int i = 0; i < wcsRect2.EdgesPoint_xyz.Length; i++)
                        //{
                        //    this.drawObject.AttachPropertyData.Add(new userWcsPoint(wcsRect2.EdgesPoint_xyz[i].X, wcsRect2.EdgesPoint_xyz[i].Y, 0, wcsRect2.CamParams));
                        //}
                        this.drawObject.DetachDrawingObjectFromWindow();
                        /////////////// 记录坐标点 /////////////////
                        switch (this.CurCaliStateEnum)
                        {
                            case enCaliStateEnum.Cali9Pt:
                                wcsRect2 = ((userWcsRectangle2)e.DataContent);
                                userPixRectangle2 pixRect2 = ((userWcsRectangle2)e.DataContent).GetPixRectangle2();
                                this.CalibNPointPixList.Add(new userPixPoint(pixRect2.Row, pixRect2.Col));
                                ///////////////////////////////////////////////////////////
                                this.CalibNPointWcsList.Add(new userWcsPoint(wcsRect2.X, wcsRect2.Y, wcsRect2.Z));
                                break;
                            case enCaliStateEnum.RotCali:
                                wcsRect2 = ((userWcsRectangle2)e.DataContent);
                                pixRect2 = ((userWcsRectangle2)e.DataContent).GetPixRectangle2();
                                this.CalibRotaPixList.Add(new userPixPoint(pixRect2.Row, pixRect2.Col));
                                ///////////////////////////////////////////////////////////
                                this.CalibRotaWcsList.Add(new userWcsPoint(wcsRect2.X, wcsRect2.Y, wcsRect2.Z, wcsRect2.Grab_x, wcsRect2.Grab_y, wcsRect2.CamParams)); // 放转时，XY拍照位置不变
                                break;
                        }
                        break;
                    case "userWcsCoordSystem":
                        this.drawObject.AttachPropertyData.Clear();
                        if (this.listData.ContainsKey(e.ItemName))
                            this.listData[e.ItemName] = e.DataContent;
                        else
                            this.listData.Add(e.ItemName, e.DataContent);
                        this.drawObject.AttachPropertyData.AddRange(this.listData.Values.ToArray()); //(userWcsCircle)e.DataContent
                        userWcsCoordSystem userWcsCoord  = ((userWcsCoordSystem)e.DataContent); 

                       this.drawObject.AttachPropertyData.Add(new userWcsPoint(userWcsCoord.CurrentPoint.X, userWcsCoord.CurrentPoint.Y, 0, userWcsCoord.CurrentPoint.CamParams));

                        this.drawObject.DetachDrawingObjectFromWindow();
                        /////////////// 记录坐标点 /////////////////
                        switch (this.CurCaliStateEnum)
                        {
                            case enCaliStateEnum.Cali9Pt:
                                this.CalibNPointPixList.Add((new userWcsPoint(userWcsCoord.CurrentPoint.X, userWcsCoord.CurrentPoint.Y, userWcsCoord.CurrentPoint.Z, userWcsCoord.CurrentPoint.Grab_x, userWcsCoord.CurrentPoint.Grab_y, userWcsCoord.CurrentPoint.CamParams)).GetPixPoint());
                                ///////////////////////////////////////////////////////////
                                this.CalibNPointWcsList.Add((new userWcsPoint(userWcsCoord.CurrentPoint.X, userWcsCoord.CurrentPoint.Y, userWcsCoord.CurrentPoint.Z, userWcsCoord.CurrentPoint.Grab_x, userWcsCoord.CurrentPoint.Grab_y, userWcsCoord.CurrentPoint.CamParams)));
                                break;
                            case enCaliStateEnum.RotCali:
                                this.CalibRotaPixList.Add((new userWcsPoint(userWcsCoord.CurrentPoint.X, userWcsCoord.CurrentPoint.Y, userWcsCoord.CurrentPoint.Z, userWcsCoord.CurrentPoint.Grab_x, userWcsCoord.CurrentPoint.Grab_y, userWcsCoord.CurrentPoint.CamParams)).GetPixPoint());
                                ///////////////////////////////////////////////////////////
                                this.CalibRotaWcsList.Add((new userWcsPoint(userWcsCoord.CurrentPoint.X, userWcsCoord.CurrentPoint.Y, userWcsCoord.CurrentPoint.Z, userWcsCoord.CurrentPoint.Grab_x, userWcsCoord.CurrentPoint.Grab_y, userWcsCoord.CurrentPoint.CamParams))); // 放转的坐标，拍照位置一定要置0
                                break;
                        }
                        break;
                    case nameof(XldDataClass):
                        this.drawObject.AttachPropertyData.Clear();
                        if (this.listData.ContainsKey(e.ItemName))
                            this.listData[e.ItemName] = e.DataContent;
                        else
                            this.listData.Add(e.ItemName, e.DataContent);
                        this.drawObject.AttachPropertyData.Add(e.DataContent);
                        this.drawObject.DetachDrawingObjectFromWindow();
                        break;
                    case "HXLDCont":
                        this.drawObject.AttachPropertyData.Clear();
                        if (this.listData.ContainsKey(e.ItemName))
                            this.listData[e.ItemName] = e.DataContent;
                        else
                            this.listData.Add(e.ItemName, e.DataContent);
                        this.drawObject.AttachPropertyData.Add(e.DataContent);
                        this.drawObject.DetachDrawingObjectFromWindow();
                        break;

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void 保存参数button_Click(object sender, EventArgs e)
        {
            try
            {
                this.CamParam.Save();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void 起始点textBox_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    string[] content = this.起始点textBox.Text.Split(new string[] { "X:", "Y:", "Theta:" }, StringSplitOptions.RemoveEmptyEntries);
                    if (content.Length > 0)
                        this.CamParam.CaliParam.StartCaliPoint.X = Convert.ToDouble(content[0]);
                    if (content.Length > 1)
                        this.CamParam.CaliParam.StartCaliPoint.Y = Convert.ToDouble(content[1]);
                    if (content.Length > 2)
                        this.CamParam.CaliParam.StartCaliPoint.Angle = Convert.ToDouble(content[2]);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void 终止点textBox_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    string[] content = this.终止点textBox.Text.Split(new string[] { "X:", "Y:", "Theta:" }, StringSplitOptions.RemoveEmptyEntries);
                    if (content.Length > 0)
                        this.CamParam.CaliParam.EndCalibPoint.X = Convert.ToDouble(content[0]);
                    if (content.Length > 1)
                        this.CamParam.CaliParam.EndCalibPoint.Y = Convert.ToDouble(content[1]);
                    if (content.Length > 2)
                        this.CamParam.CaliParam.EndCalibPoint.Angle = Convert.ToDouble(content[2]);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void 相机轴坐标textBox_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    string[] content = this.终止点textBox.Text.Split(new string[] { "X:", "Y:" }, StringSplitOptions.RemoveEmptyEntries);
                    if (content.Length > 0)
                        this.CamParam.CaliParam.CamAxisCoord.X = Convert.ToDouble(content[0]);
                    if (content.Length > 1)
                        this.CamParam.CaliParam.CamAxisCoord.Y = Convert.ToDouble(content[1]);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void 归一化原点checkBox_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.CamParam.CaliParam.CalibMethod == enCalibMethod.先旋转后平移) return; // 如果是先旋转后平移模式，那么将不能归一化原点
                if (this.CamParam.CaliParam.CoordOriginType == enCoordOriginType.IsLoading) return; // 上料模式
                if (!归一化原点checkBox.Checked) return;
                HTuple Qx = new HTuple();
                HTuple Qy = new HTuple();
                int width, height;
                this.drawObject.BackImage.Image.GetImageSize(out width, out height);
                double wcs_x, wcs_y, wcs_z;
                this.CamParam.ImagePointsToWorldPlane(height * 0.5, width * 0.5, 0, 0, 0, out wcs_x, out wcs_y, out wcs_z);
                if (Math.Abs(wcs_x) > 0.0005)
                    this.CamParam.HomMat2D.c02 -= wcs_x;
                if (Math.Abs(wcs_y) > 0.0005)
                    this.CamParam.HomMat2D.c12 -= wcs_y;
                //////// 平移原点后需要改变拍照点的XY坐标，这样就相当于，原点在图像中心位置来进行标定
                this.CamParam.CaliParam.StartCaliPoint.X += wcs_x;
                this.CamParam.CaliParam.StartCaliPoint.Y += wcs_y;
                this.CamParam.CaliParam.EndCalibPoint.X += wcs_x;
                this.CamParam.CaliParam.EndCalibPoint.Y += wcs_y;
                //this.CamParam.CaliParam.RotateCalibPoint.X += wcs_x;
                //this.CamParam.CaliParam.RotateCalibPoint.Y += wcs_y;
                ///////////////////////////
                this.CamParam.ImagePointsToWorldPlane(height * 0.5, width * 0.5, 0, 0, 0, out wcs_x, out wcs_y, out wcs_z);
                MessageBox.Show("图像中点/视野中心坐标 ：" + wcs_x.ToString("f5") + "  " + wcs_y.ToString("f5"));
                ///////
                this.drawObject.AddViewObject(new ViewData(new userWcsCoordSystem(this.CamParam)));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void MoveOriginToImageCenter()
        {
            try
            {
                HTuple Qx = new HTuple();
                HTuple Qy = new HTuple();
                int width, height;
                this.drawObject.BackImage.Image.GetImageSize(out width, out height);
                double wcs_x, wcs_y, wcs_z;
                this.CamParam.ImagePointsToWorldPlane(height * 0.5, width * 0.5, 0, 0, 0, out wcs_x, out wcs_y, out wcs_z);
                if (Math.Abs(wcs_x) > 0.0005)
                    this.CamParam.HomMat2D.c02 -= wcs_x;
                if (Math.Abs(wcs_y) > 0.0005)
                    this.CamParam.HomMat2D.c12 -= wcs_y;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public void DisplayClickObject(object sender, TreeNodeMouseClickEventArgs e)  //
        {
            if (e.Node.Tag == null) return;
            if (e.Button != MouseButtons.Left) return; // 点击右键时不变
            try
            {
                switch (e.Node.Tag.GetType().Name)
                {
                    case "CircleMeasure":
                        this.drawObject?.AttachPropertyData.Clear();
                        if (!(this.drawObject is userDrawCircleMeasure))
                        {
                            this.drawObject.GrayValueInfo -= new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                            this.drawObject.ClearDrawingObject();
                            this.drawObject = new userDrawCircleMeasure(this.hWindowControl1, ((CircleMeasure)e.Node.Tag).FindCircle.CirclePixPosition, ((CircleMeasure)e.Node.Tag).PixCoordSystem);
                            this.drawObject.GrayValueInfo += new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                        }
                        this.drawObject.AttachPropertyData.Clear();
                        foreach (KeyValuePair<string, object> item in this.listData)
                        {
                            if (item.Key != e.Node.Text)
                                this.drawObject.AttachPropertyData.Add(item.Value);
                        }
                        this.drawObject.SetParam(((CircleMeasure)e.Node.Tag).PixCoordSystem);
                        this.drawObject.SetParam(((CircleMeasure)e.Node.Tag).FindCircle.CirclePixPosition.AffineTransPixCircle(((CircleMeasure)e.Node.Tag).PixCoordSystem?.GetVariationHomMat2D()));
                        this.drawObject.BackImage = ((CircleMeasure)e.Node.Tag).ImageData != null ? ((CircleMeasure)e.Node.Tag).ImageData : this.CurrentImageData;
                        this.drawObject.AttachDrawingObjectToWindow();
                        this.metrolegyParamForm.drawObject = this.drawObject;
                        this._currFunction = (CircleMeasure)e.Node.Tag;
                        //DisplayClickItem(sender, e);
                        break;
                    case "CircleSectorMeasure":
                        this.drawObject?.AttachPropertyData.Clear();
                        if (!(this.drawObject is userDrawCircleSectorMeasure))
                        {
                            this.drawObject.GrayValueInfo -= new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                            this.drawObject.ClearDrawingObject();
                            this.drawObject = new userDrawCircleSectorMeasure(this.hWindowControl1, ((CircleSectorMeasure)e.Node.Tag).FindCircleSector.CircleSectorPixPosition, ((CircleSectorMeasure)e.Node.Tag).PixCoordSystem);
                            this.drawObject.GrayValueInfo += new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                        }
                        this.drawObject.AttachPropertyData.Clear();
                        foreach (KeyValuePair<string, object> item in this.listData)
                        {
                            if (item.Key != e.Node.Text)
                                this.drawObject.AttachPropertyData.Add(item.Value);
                        }
                        this.drawObject.SetParam(((CircleSectorMeasure)e.Node.Tag).PixCoordSystem);
                        this.drawObject.SetParam(((CircleSectorMeasure)e.Node.Tag).FindCircleSector.CircleSectorPixPosition.AffineTransPixCircleSector(((CircleSectorMeasure)e.Node.Tag).PixCoordSystem?.GetVariationHomMat2D()));
                        this.drawObject.BackImage = ((CircleSectorMeasure)e.Node.Tag).ImageData != null ? ((CircleSectorMeasure)e.Node.Tag).ImageData : this.CurrentImageData;
                        this.drawObject.AttachDrawingObjectToWindow();
                        this.metrolegyParamForm.drawObject = this.drawObject;
                        this._currFunction = (CircleSectorMeasure)e.Node.Tag;
                        //DisplayClickItem(sender, e);
                        break;
                    case "EllipseMeasure":
                        this.drawObject?.AttachPropertyData.Clear();
                        if (!(this.drawObject is userDrawEllipseMeasure))
                        {
                            this.drawObject.GrayValueInfo -= new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                            this.drawObject.ClearDrawingObject();
                            this.drawObject = new userDrawEllipseMeasure(this.hWindowControl1, ((EllipseMeasure)e.Node.Tag).FindEllipse.EllipsePixPosition, ((EllipseMeasure)e.Node.Tag).PixCoordSystem);
                            this.drawObject.GrayValueInfo += new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                        }
                        this.drawObject.AttachPropertyData.Clear();
                        foreach (KeyValuePair<string, object> item in this.listData)
                        {
                            if (item.Key != e.Node.Text)
                                this.drawObject.AttachPropertyData.Add(item.Value);
                        }
                        this.drawObject.SetParam(((EllipseMeasure)e.Node.Tag).PixCoordSystem);
                        this.drawObject.SetParam(((EllipseMeasure)e.Node.Tag).FindEllipse.EllipsePixPosition.AffineTransPixEllipse(((EllipseMeasure)e.Node.Tag).PixCoordSystem?.GetVariationHomMat2D()));
                        this.drawObject.BackImage = ((EllipseMeasure)e.Node.Tag).ImageData != null ? ((EllipseMeasure)e.Node.Tag).ImageData : this.CurrentImageData;
                        this.drawObject.AttachDrawingObjectToWindow();
                        this.metrolegyParamForm.drawObject = this.drawObject;
                        this._currFunction = (EllipseMeasure)e.Node.Tag;
                        //DisplayClickItem(sender, e);
                        break;
                    case "EllipseSectorMeasure":
                        this.drawObject?.AttachPropertyData.Clear();
                        if (!(this.drawObject is userDrawEllipseSectorMeasure))
                        {
                            this.drawObject.GrayValueInfo -= new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                            this.drawObject.ClearDrawingObject();
                            this.drawObject = new userDrawEllipseSectorMeasure(this.hWindowControl1, ((EllipseSectorMeasure)e.Node.Tag).FindEllipseSector.EllipseSectorPixPosition, ((EllipseSectorMeasure)e.Node.Tag).PixCoordSystem);
                            this.drawObject.GrayValueInfo += new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                        }
                        this.drawObject.AttachPropertyData.Clear();
                        foreach (KeyValuePair<string, object> item in this.listData)
                        {
                            if (item.Key != e.Node.Text)
                                this.drawObject.AttachPropertyData.Add(item.Value);
                        }
                        this.drawObject.SetParam(((EllipseSectorMeasure)e.Node.Tag).PixCoordSystem);
                        this.drawObject.SetParam(((EllipseSectorMeasure)e.Node.Tag).FindEllipseSector.EllipseSectorPixPosition.AffineTransPixEllipseSector(((EllipseSectorMeasure)e.Node.Tag).PixCoordSystem?.GetVariationHomMat2D()));
                        this.drawObject.BackImage = ((EllipseSectorMeasure)e.Node.Tag).ImageData != null ? ((EllipseSectorMeasure)e.Node.Tag).ImageData : this.CurrentImageData;
                        this.drawObject.AttachDrawingObjectToWindow();
                        this.metrolegyParamForm.drawObject = this.drawObject;
                        this._currFunction = (EllipseSectorMeasure)e.Node.Tag;
                        //DisplayClickItem(sender, e);
                        break;
                    case "LineMeasure":
                        this.drawObject?.AttachPropertyData.Clear();
                        if (!(this.drawObject is userDrawLineMeasure))
                        {
                            this.drawObject.GrayValueInfo -= new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                            this.drawObject.ClearDrawingObject();
                            this.drawObject = new userDrawLineMeasure(this.hWindowControl1, ((LineMeasure)e.Node.Tag).FindLine.LinePixPosition, ((LineMeasure)e.Node.Tag).PixCoordSystem);
                            this.drawObject.GrayValueInfo += new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                        }
                        this.drawObject.AttachPropertyData.Clear();
                        foreach (KeyValuePair<string, object> item in this.listData)
                        {
                            if (item.Key != e.Node.Text)
                                this.drawObject.AttachPropertyData.Add(item.Value);
                        }
                        this.drawObject.SetParam(((LineMeasure)e.Node.Tag).PixCoordSystem);
                        this.drawObject.SetParam(((LineMeasure)e.Node.Tag).FindLine.LinePixPosition.AffinePixLine2D(((LineMeasure)e.Node.Tag).PixCoordSystem?.GetVariationHomMat2D()));
                        this.drawObject.BackImage = ((LineMeasure)e.Node.Tag).ImageData != null ? ((LineMeasure)e.Node.Tag).ImageData : this.CurrentImageData;
                        this.drawObject.AttachDrawingObjectToWindow();
                        this.metrolegyParamForm.drawObject = this.drawObject;
                        this._currFunction = (LineMeasure)e.Node.Tag;
                        //DisplayClickItem(sender, e);
                        break;
                    case "PointMeasure":
                        this.drawObject?.AttachPropertyData.Clear();
                        if (!(this.drawObject is userDrawPointMeasure))
                        {
                            this.drawObject.GrayValueInfo -= new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                            this.drawObject.ClearDrawingObject();
                            this.drawObject = new userDrawPointMeasure(this.hWindowControl1, ((PointMeasure)e.Node.Tag).FindPoint.LinePixPosition, ((PointMeasure)e.Node.Tag).PixCoordSystem);
                            this.drawObject.GrayValueInfo += new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                        }
                        this.drawObject.AttachPropertyData.Clear();
                        foreach (KeyValuePair<string, object> item in this.listData)
                        {
                            if (item.Key != e.Node.Text)
                                this.drawObject.AttachPropertyData.Add(item.Value);
                        }
                        this.drawObject.SetParam(((PointMeasure)e.Node.Tag).PixCoordSystem);
                        this.drawObject.SetParam(((PointMeasure)e.Node.Tag).FindPoint.LinePixPosition.AffinePixLine2D(((PointMeasure)e.Node.Tag).PixCoordSystem?.GetVariationHomMat2D()));
                        this.drawObject.BackImage = ((PointMeasure)e.Node.Tag).ImageData != null ? ((PointMeasure)e.Node.Tag).ImageData : this.CurrentImageData;
                        this.drawObject.AttachDrawingObjectToWindow();
                        this.metrolegyParamForm.drawObject = this.drawObject;
                        this._currFunction = (PointMeasure)e.Node.Tag;
                        //DisplayClickItem(sender, e);
                        break;
                    case "Rectangle2Measure":
                        this.drawObject?.AttachPropertyData.Clear();
                        if (!(this.drawObject is userDrawRect2Measure))
                        {
                            this.drawObject.GrayValueInfo -= new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                            this.drawObject.ClearDrawingObject();
                            this.drawObject = new userDrawRect2Measure(this.hWindowControl1, ((Rectangle2Measure)e.Node.Tag).FindRect2.Rect2PixPosition, ((Rectangle2Measure)e.Node.Tag).PixCoordSystem);
                            this.drawObject.GrayValueInfo += new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                        }
                        this.drawObject.AttachPropertyData.Clear();
                        foreach (KeyValuePair<string, object> item in this.listData)
                        {
                            if (item.Key != e.Node.Text)
                                this.drawObject.AttachPropertyData.Add(item.Value);
                        }
                        this.drawObject.SetParam(((Rectangle2Measure)e.Node.Tag).PixCoordSystem);
                        this.drawObject.SetParam(((Rectangle2Measure)e.Node.Tag).FindRect2.Rect2PixPosition.AffineTransPixRect2(((Rectangle2Measure)e.Node.Tag).PixCoordSystem?.GetVariationHomMat2D())); //?.AffineWcsRectangle2D(((Rectangle2Measure)e.Node.Tag).WcsCoordSystem.GetVariationHomMat2D())
                        this.drawObject.BackImage = ((Rectangle2Measure)e.Node.Tag).ImageData != null ? ((Rectangle2Measure)e.Node.Tag).ImageData : this.CurrentImageData;
                        this.drawObject.AttachDrawingObjectToWindow();
                        this.metrolegyParamForm.drawObject = this.drawObject;
                        this._currFunction = (Rectangle2Measure)e.Node.Tag;
                        //DisplayClickItem(sender, e);
                        break;
                    case "WidthMeasure":
                        this.drawObject?.AttachPropertyData.Clear();
                        if (!(this.drawObject is userDrawWidthMeasure))
                        {
                            this.drawObject.GrayValueInfo -= new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                            this.drawObject.ClearDrawingObject();
                            this.drawObject = new userDrawWidthMeasure(this.hWindowControl1, ((WidthMeasure)e.Node.Tag).FindWidth.Rect2PixPosition, ((WidthMeasure)e.Node.Tag).PixCoordSystem);
                            this.drawObject.GrayValueInfo += new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                        }
                        this.drawObject.AttachPropertyData.Clear();
                        foreach (KeyValuePair<string, object> item in this.listData)
                        {
                            if (item.Key != e.Node.Text)
                                this.drawObject.AttachPropertyData.Add(item.Value);
                        }
                        this.drawObject.SetParam(((WidthMeasure)e.Node.Tag).PixCoordSystem);
                        this.drawObject.SetParam(((WidthMeasure)e.Node.Tag).FindWidth.Rect2PixPosition.AffineTransPixRect2(((WidthMeasure)e.Node.Tag).PixCoordSystem?.GetVariationHomMat2D())); //?.AffineWcsRectangle2D(((Rectangle2Measure)e.Node.Tag).WcsCoordSystem.GetVariationHomMat2D())
                        this.drawObject.BackImage = ((WidthMeasure)e.Node.Tag).ImageData != null ? ((WidthMeasure)e.Node.Tag).ImageData : this.CurrentImageData;
                        this.drawObject.AttachDrawingObjectToWindow();
                        this.metrolegyParamForm.drawObject = this.drawObject;
                        this._currFunction = (WidthMeasure)e.Node.Tag;
                        //DisplayClickItem(sender, e);
                        break;
                    case "CrossPointMeasure":
                        this.drawObject?.AttachPropertyData.Clear();
                        if (!(this.drawObject is userDrawCrossMeasure))
                        {
                            this.drawObject.GrayValueInfo -= new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                            this.drawObject.ClearDrawingObject();
                            this.drawObject = new userDrawCrossMeasure(this.hWindowControl1, ((CrossPointMeasure)e.Node.Tag).FindCrossPoint.LinePixPosition, ((CrossPointMeasure)e.Node.Tag).PixCoordSystem);
                            this.drawObject.GrayValueInfo += new GrayValueInfoEventHandler(hWindowControl1_MouseMove);
                        }
                        this.drawObject.AttachPropertyData.Clear();
                        foreach (KeyValuePair<string, object> item in this.listData)
                        {
                            if (item.Key != e.Node.Text)
                                this.drawObject.AttachPropertyData.Add(item.Value);
                        }
                        this.drawObject.SetParam(((CrossPointMeasure)e.Node.Tag).PixCoordSystem);
                        this.drawObject.SetParam(((CrossPointMeasure)e.Node.Tag).FindCrossPoint.LinePixPosition.AffinePixLine2D(((CrossPointMeasure)e.Node.Tag).PixCoordSystem.GetVariationHomMat2D()));
                        this.drawObject.BackImage = ((CrossPointMeasure)e.Node.Tag).ImageData != null ? ((CrossPointMeasure)e.Node.Tag).ImageData : this.CurrentImageData;
                        this.drawObject.AttachDrawingObjectToWindow();
                        this.metrolegyParamForm.drawObject = this.drawObject;
                        this._currFunction = (CrossPointMeasure)e.Node.Tag;
                        //DisplayClickItem(sender, e);
                        break;
                    default:

                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private CancellationTokenSource cts1;
        private void 上相机实时采集checkBox_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                switch (this.上相机实时采集checkBox.CheckState)
                {
                    case CheckState.Checked:
                        this.上相机实时采集checkBox.BackColor = Color.Red;
                        AcqSource acqSource = AcqSourceManage.Instance.GetCamAcqSource(this.CamParam.SensorName);
                        if (acqSource == null) return;
                        cts1 = new CancellationTokenSource();
                        Dictionary<enDataItem, object> data;
                        Task.Run(() =>
                        {
                            this.drawObject.IsLiveState = true;
                            while (!cts1.IsCancellationRequested)
                            {
                                data = acqSource.AcqImageData(null);
                                switch (acqSource.Sensor?.ConfigParam.SensorType)
                                {
                                    case enUserSensorType.面阵相机:
                                        if (data?.Count > 0)
                                        {
                                            this.drawObject.BackImage = (ImageDataClass)data[enDataItem.Image];
                                            this.drawObject.AttachPropertyData.Clear();
                                            this.drawObject.AttachPropertyData.Add((this.GenCrossLine(this.drawObject.BackImage.Image)));
                                        }
                                        break;
                                }
                                Thread.Sleep(100);
                            }
                            this.drawObject.IsLiveState = false;
                        });
                        break;
                    default:
                        cts1?.Cancel();
                        this.上相机实时采集checkBox.BackColor = Color.Lime;
                        break;
                }
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

        private void V轴旋转dataGridView_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            try
            {
                for (int i = 0; i < this.V轴旋转dataGridView.Rows.Count; i++)
                {
                    this.V轴旋转dataGridView.Rows[i].HeaderCell.Value = (i + 1).ToString();
                }
            }
            catch
            {

            }
        }

        private void U轴旋转dataGridView_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {

        }



        private void 生成U轴旋转坐标button_Click(object sender, EventArgs e)
        {
            try
            {
                int result = -1;
                this.calibCoordConfigParamRotaU.CalibCoordParamList.Clear();
                Math.DivRem((int)this.CamParam.CaliParam.AngleStepAxisU, 2, out result);
                if (result == 1) this.CamParam.CaliParam.AngleStepAxisU += 1;   // 让旋转步数为奇数
                this.U轴旋转步数numericUpDown.Value = (decimal)this.CamParam.CaliParam.AngleStepAxisU;
                switch (this.U轴旋转方向comboBox.Text)
                {
                    default:
                    case "双向":
                        for (int i = (int)((this.CamParam.CaliParam.AngleStepAxisU) * -0.5); i <= (int)((this.CamParam.CaliParam.AngleStepAxisU) * 0.5); i++)
                        {
                            this.calibCoordConfigParamRotaU.CalibCoordParamList.Add(new CoordSysAxisParam(
                                this.CamParam.CaliParam.RotateCalibPoint.X,
                                this.CamParam.CaliParam.RotateCalibPoint.Y,
                                this.CamParam.CaliParam.RotateCalibPoint.Z,
                                0,
                                this.CamParam.CaliParam.RotateCalibPoint.Angle + i * this.CamParam.CaliParam.AngleRangeAxisU * 1.0 / this.CamParam.CaliParam.AngleStepAxisU,
                                0
                                //this.CamParam.CaliParam.RotCenter.Angle - this.CamParam.CaliParam.AngleRange + i * this.CamParam.CaliParam.AngleRange * 2 / this.CamParam.CaliParam.AngleStep
                                ));
                        }
                        break;
                    case "正向":
                        for (int i = 0; i <= (int)((this.CamParam.CaliParam.AngleStepAxisU)); i++)
                        {
                            this.calibCoordConfigParamRotaU.CalibCoordParamList.Add(new CoordSysAxisParam(
                                this.CamParam.CaliParam.RotateCalibPoint.X,
                                this.CamParam.CaliParam.RotateCalibPoint.Y,
                                this.CamParam.CaliParam.RotateCalibPoint.Z,
                                0,
                                this.CamParam.CaliParam.RotateCalibPoint.Angle + i * this.CamParam.CaliParam.AngleRangeAxisU * 1.0 / this.CamParam.CaliParam.AngleStepAxisU,
                                0
                                //this.CamParam.CaliParam.RotCenter.Angle - this.CamParam.CaliParam.AngleRange + i * this.CamParam.CaliParam.AngleRange * 2 / this.CamParam.CaliParam.AngleStep
                                ));
                        }
                        break;
                    case "负向":
                        for (int i = 0; i >= (int)((this.CamParam.CaliParam.AngleStepAxisU) * -1); i--)
                        {
                            this.calibCoordConfigParamRotaU.CalibCoordParamList.Add(new CoordSysAxisParam(
                                this.CamParam.CaliParam.RotateCalibPoint.X,
                                this.CamParam.CaliParam.RotateCalibPoint.Y,
                                this.CamParam.CaliParam.RotateCalibPoint.Z,
                                0,
                                this.CamParam.CaliParam.RotateCalibPoint.Angle + i * this.CamParam.CaliParam.AngleRangeAxisU * 1.0 / this.CamParam.CaliParam.AngleStepAxisU,
                                0
                                //this.CamParam.CaliParam.RotCenter.Angle - this.CamParam.CaliParam.AngleRange + i * this.CamParam.CaliParam.AngleRange * 2 / this.CamParam.CaliParam.AngleStep
                                ));
                        }
                        break;
                }
                /////////  用绝对坐标来表示
                for (int i = 0; i < this.U轴旋转dataGridView.Rows.Count; i++)
                {
                    this.U轴旋转dataGridView.Rows[i].HeaderCell.Value = (i + 1).ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void 获取相机轴坐标but_Click(object sender, EventArgs e)
        {
            try
            {
                double X = 0, Y = 0, Z = 0, Theta = 0, U = 0, V = 0;
                //Read(out X, out Y, out Z, out Theta, out U, out V);
                MotionCardManage.GetCard(this.CamParam.CaliParam.CoordSysName).GetAxisPosition(this.CamParam.CaliParam.CoordSysName, enAxisName.X2轴, out X);
                MotionCardManage.GetCard(this.CamParam.CaliParam.CoordSysName).GetAxisPosition(this.CamParam.CaliParam.CoordSysName, enAxisName.Y2轴, out Y);
                ///////////////////////
                this.CamParam.CaliParam.CamAxisCoord = new userWcsPoint(X, Y, Z);
                this.相机轴坐标textBox.Text = "X:" + X.ToString("f3") + "   Y:" + Y.ToString("f3") + "   Z:" + Z.ToString("f3");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void 生成V轴旋转坐标button_Click(object sender, EventArgs e)
        {
            try
            {
                int result = -1;
                this.calibCoordConfigParamRotaV.CalibCoordParamList.Clear();
                Math.DivRem((int)this.CamParam.CaliParam.AngleStepAxisV, 2, out result);
                if (result == 1) this.CamParam.CaliParam.AngleStepAxisV += 1;   // 让旋转步数为奇数
                this.V轴旋转步数numericUpDown.Value = (decimal)this.CamParam.CaliParam.AngleStepAxisV;
                switch (this.V轴旋转方向comboBox.Text)
                {
                    default:
                    case "双向":
                        for (int i = (int)((this.CamParam.CaliParam.AngleStepAxisV) * -0.5); i <= (int)((this.CamParam.CaliParam.AngleStepAxisV) * 0.5); i++)
                        {
                            this.calibCoordConfigParamRotaV.CalibCoordParamList.Add(new CoordSysAxisParam(
                                this.CamParam.CaliParam.RotateCalibPoint.X,
                                this.CamParam.CaliParam.RotateCalibPoint.Y,
                                this.CamParam.CaliParam.RotateCalibPoint.Z,
                                0,
                                0,
                                this.CamParam.CaliParam.RotateCalibPoint.Angle + i * this.CamParam.CaliParam.AngleRangeAxisV * 1.0 / this.CamParam.CaliParam.AngleStepAxisV
                                //this.CamParam.CaliParam.RotCenter.Angle - this.CamParam.CaliParam.AngleRange + i * this.CamParam.CaliParam.AngleRange * 2 / this.CamParam.CaliParam.AngleStep
                                ));
                        }
                        break;
                    case "正向":
                        for (int i = 0; i <= (int)((this.CamParam.CaliParam.AngleStepAxisV)); i++)
                        {
                            this.calibCoordConfigParamRotaV.CalibCoordParamList.Add(new CoordSysAxisParam(
                                this.CamParam.CaliParam.RotateCalibPoint.X,
                                this.CamParam.CaliParam.RotateCalibPoint.Y,
                                this.CamParam.CaliParam.RotateCalibPoint.Z,
                                0,
                                     0,
                                this.CamParam.CaliParam.RotateCalibPoint.Angle + i * this.CamParam.CaliParam.AngleRangeAxisV * 1.0 / this.CamParam.CaliParam.AngleStepAxisV
                                //this.CamParam.CaliParam.RotCenter.Angle - this.CamParam.CaliParam.AngleRange + i * this.CamParam.CaliParam.AngleRange * 2 / this.CamParam.CaliParam.AngleStep
                                ));
                        }
                        break;
                    case "负向":
                        for (int i = 0; i >= (int)((this.CamParam.CaliParam.AngleStepAxisV) * -1); i--)
                        {
                            this.calibCoordConfigParamRotaV.CalibCoordParamList.Add(new CoordSysAxisParam(
                                this.CamParam.CaliParam.RotateCalibPoint.X,
                                this.CamParam.CaliParam.RotateCalibPoint.Y,
                                this.CamParam.CaliParam.RotateCalibPoint.Z,
                                0,
                                0,
                                this.CamParam.CaliParam.RotateCalibPoint.Angle + i * this.CamParam.CaliParam.AngleRangeAxisV * 1.0 / this.CamParam.CaliParam.AngleStepAxisV
                                //this.CamParam.CaliParam.RotCenter.Angle - this.CamParam.CaliParam.AngleRange + i * this.CamParam.CaliParam.AngleRange * 2 / this.CamParam.CaliParam.AngleStep
                                ));
                        }
                        break;
                }
                /////////  用绝对坐标来表示
                for (int i = 0; i < this.V轴旋转dataGridView.Rows.Count; i++)
                {
                    this.V轴旋转dataGridView.Rows[i].HeaderCell.Value = (i + 1).ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }


        public void DisplayClickItem(object sender, TreeNodeMouseClickEventArgs e)  // 这里一定要加一个键值对变量来收集，以免出现重合
        {
            try
            {
                if (e.Node.Tag == null) return;
                if (e.Node.Name == null) return;// 在图形窗口只显示世界坐标元素，像素元素在图像窗口显示
                this.drawObject.AttachPropertyData.Clear();
                switch (e.Node.Tag.GetType().Name)
                {
                    case "CircleMeasure":
                    case "CircleSectorMeasure":
                    case "EllipseMeasure":
                    case "EllipseSectorMeasure":
                    case "LineMeasure":
                    case "PointMeasure":
                    case "Rectangle2Measure":
                    case "WidthMeasure":
                        this.drawObject.AttachPropertyData.Clear(); // 清空附加属性
                        this.drawObject.IsDispalyAttachEdgesProperty = true;
                        // 添加需要显示的元素
                        foreach (var items in this.listData.Keys)
                        {
                            if (items.Split('-')[0] == e.Node.Name) continue; // + "-" + "0"
                            this.drawObject.AttachPropertyData.Add(this.listData[items]);
                        }
                        this.drawObject.DrawingGraphicObject();
                        break;
                }
            }
            catch (Exception he)
            {

            }
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

        public TreeNode GetExecuteNode(enCoordSysName coordSysName, out string Info)
        {
            TreeNode node = null;
            Info = "";
            bool IsOk = true;
            List<TreeNode> listTreeNode;
            listTreeNode = this._treeViewWrapClassNPoint.GetTreeViewNodeTag(); // 获取所有节点
            foreach (var item in listTreeNode)
            {
                if (item.Checked) continue; // 如果节点是禁用的，该属性为 true;
                IsOk = true;
                object oo = ((BaseFunction)item.Tag).ResultInfo;
                if (oo == null) continue;
                switch (oo.GetType().Name)
                {
                    case "BindingList`1":
                        switch (oo.GetType().GetGenericArguments()[0].Name)
                        {
                            case nameof(PlcCommunicateInfo):
                                BindingList<PlcCommunicateInfo> plcInfo = ((BaseFunction)item.Tag).ResultInfo as BindingList<PlcCommunicateInfo>;
                                foreach (var item2 in plcInfo)
                                {
                                    string value = CommunicationConfigParamManger.Instance.ReadValue(item2.CoordSysName, item2.CommunicationCommand)?.ToString();
                                    if (value == null)
                                    {
                                        IsOk = false;
                                        break;
                                    }
                                    item2.ReadValue = value;
                                    if (Info.Length == 0)
                                        Info = value;
                                    else
                                        Info += "," + value;
                                    ///////////////////////////////////////////////
                                    if (item2.IsCompare)
                                    {
                                        if (item2.TargetValue.Trim() == value.Trim() && item2.CoordSysName == coordSysName)
                                            IsOk = IsOk && true;
                                        else
                                            IsOk = IsOk && false;
                                    }
                                }
                                break;
                            default:
                                IsOk = false;
                                break;
                        }
                        break;
                    default:
                        IsOk = false;
                        break;

                }
                if (IsOk)
                {
                    node = item;
                    break;
                }
            }
            return node;
        }

        #endregion



    }
}

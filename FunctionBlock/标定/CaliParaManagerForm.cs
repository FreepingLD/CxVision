using Common;
using MotionControlCard;
using Sensor;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace FunctionBlock
{
    public partial class CaliParaManagerForm : Form
    {
        public bool _userEnable;
        public bool UserEnable
        {
            get
            {
                return _userEnable;
            }
            set
            {
                this._userEnable = value;
                if (this._userEnable)
                {
                    this.dataGridView相机.ReadOnly = false;
                    this.dataGridView激光.ReadOnly = false;
                    this.相机标定参数配置label.Enabled = true;
                    this.激光标定参数配置label.Enabled = true;
                }
                else
                {
                    this.dataGridView相机.ReadOnly = true;
                    this.dataGridView激光.ReadOnly = true;
                    this.相机标定参数配置label.Enabled = false;
                    this.激光标定参数配置label.Enabled = false;
                }
            }
        }

        // 相机标定参数是跟相机的数量挂钩的
        public CaliParaManagerForm()
        {
            InitializeComponent();
        }

        private BindingList<NinePointCalibParam> eyeHandleCaliParamListCam = new BindingList<NinePointCalibParam>();
        private BindingList<NinePointCalibParam> eyeHandleCaliParamListLaser = new BindingList<NinePointCalibParam>();
        private List<CameraParam> CameraParamList = new List<CameraParam>();
        private List<LaserParam> LaserParamList = new List<LaserParam>();
        private void CaliParaManagerForm_Load(object sender, EventArgs e)
        {
            this.CaliModelColumn.Items.Clear();
            this.CaliModelColumn.ValueType = typeof(enCamCaliModel);
            foreach (enCamCaliModel temp in Enum.GetValues(typeof(enCamCaliModel)))
                this.CaliModelColumn.Items.Add(temp);
            ////////////////////////////////////
            this.CalibMethodColumn.Items.Clear();
            this.CalibMethodColumn.ValueType = typeof(enCalibMethod);
            foreach (enCalibMethod temp in Enum.GetValues(typeof(enCalibMethod)))
                this.CalibMethodColumn.Items.Add(temp);
            //////////////////////////////////
            this.CoordSysNameColumn.ValueType = typeof(enCoordSysName);
            this.CoordSysNameColumn.Items.Clear();
            foreach (enCoordSysName temp in Enum.GetValues(typeof(enCoordSysName)))
                this.CoordSysNameColumn.Items.Add(temp);
            //////////////////////////////////////////////  这里更换为采集源是不是会更好？
            this.MapCamNameColumn.ValueType = typeof(string);
            this.MapCamNameColumn.Items.Clear();
            this.MapCamNameColumn.Items.Add("NONE");
            foreach (string temp in SensorConnectConfigParamManger.Instance.GetSensorName())
                this.MapCamNameColumn.Items.Add(temp);
            /////////////////////////////////////////////////////////
            this.CoordOriginTypeColumn.ValueType = typeof(enCoordOriginType);
            this.CoordOriginTypeColumn.Items.Clear();
            foreach (enCoordOriginType item in Enum.GetValues(typeof(enCoordOriginType)))
                this.CoordOriginTypeColumn.Items.Add(item);
            ///////////////  激光窗体配置 ///////////////////////////////////////////////
            this.CaliModelColLaser.Items.Clear();
            this.CaliModelColLaser.ValueType = typeof(enCamCaliModel);
            foreach (enCamCaliModel temp in Enum.GetValues(typeof(enCamCaliModel)))
                this.CaliModelColLaser.Items.Add(temp);
            ////////////////////////////////////
            this.CalibMethodColLaser.Items.Clear();
            this.CalibMethodColLaser.ValueType = typeof(enCalibMethod);
            foreach (enCalibMethod temp in Enum.GetValues(typeof(enCalibMethod)))
                this.CalibMethodColLaser.Items.Add(temp);
            //////////////////////////////////
            this.CoordSysNameColLaser.ValueType = typeof(enCoordSysName);
            this.CoordSysNameColLaser.Items.Clear();
            foreach (enCoordSysName temp in Enum.GetValues(typeof(enCoordSysName)))
                this.CoordSysNameColLaser.Items.Add(temp);
            //////////////////////////////////////////////  这里更换为采集源是不是会更好？
            this.MapLaserNameColumn.ValueType = typeof(string);
            this.MapLaserNameColumn.Items.Clear();
            this.MapLaserNameColumn.Items.Add("NONE");
            foreach (string temp in SensorConnectConfigParamManger.Instance.GetSensorName())
                this.MapLaserNameColumn.Items.Add(temp);
            /////////////////////////////////////////////////////////
            this.CoordOriginTypeColLaser.ValueType = typeof(enCoordOriginType);
            this.CoordOriginTypeColLaser.Items.Clear();
            foreach (enCoordOriginType item in Enum.GetValues(typeof(enCoordOriginType)))
                this.CoordOriginTypeColLaser.Items.Add(item);
            /////////////////////////////////////////////////////////
            CameraParamList.Clear();
            eyeHandleCaliParamListCam.Clear();
            LaserParamList.Clear();
            eyeHandleCaliParamListLaser.Clear();
            foreach (var item in SensorConnectConfigParamManger.Instance.ConfigParamList)
            {
                switch (item.SensorType)
                {
                    case enUserSensorType.线阵相机:
                    case enUserSensorType.面阵相机:
                        CameraParamList.Add(SensorManage.GetSensor(item.SensorName)?.CameraParam);
                        eyeHandleCaliParamListCam.Add(SensorManage.GetSensor(item.SensorName)?.CameraParam?.CaliParam);
                        break;
                    case enUserSensorType.点激光:
                    case enUserSensorType.线激光:
                    case enUserSensorType.面激光:
                        LaserParamList.Add(SensorManage.GetSensor(item.SensorName)?.LaserParam);
                        eyeHandleCaliParamListLaser.Add(SensorManage.GetSensor(item.SensorName)?.LaserParam?.CaliParam);
                        break;
                }
            }
            ////////////////////////////////////////////////////////////
            this.dataGridView相机.DataSource = eyeHandleCaliParamListCam;
            this.dataGridView激光.DataSource = eyeHandleCaliParamListLaser;
            this.DoubleBuffered = true;
            this.dataGridView相机.DoubleBuffere(true);
            this.dataGridView激光.DoubleBuffere(true);
            /////////////////////////////////////////////////////////////////
            //this.相机服务器comboBox.DataSource = SocketConnectManager.Instance.GetSocketName();
            //this.激光服务器comboBox.DataSource = SocketConnectManager.Instance.GetSocketName();

            //string nnn = CameraParamList.GetType().Name;
            //Type[] type = CameraParamList.GetType().GenericTypeArguments;
            //string nnn2 = CameraParamList.GetType().GenericTypeArguments[0].Name;

        }

        private void dataGridView相机_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    switch (dataGridView相机.Columns[e.ColumnIndex].Name)
                    {
                        case "CalibBtn":
                            CameraParam NowCaliPara = null;
                            CameraParam MapTargetNowCaliPara = null;
                            foreach (var item in CameraParamList)
                            {
                                if (eyeHandleCaliParamListCam[e.RowIndex].CamName == item.CaliParam.CamName)
                                    NowCaliPara = item; // CameraParamList[e.RowIndex];
                                if (eyeHandleCaliParamListCam[e.RowIndex].MapCamName == item.CaliParam.CamName)
                                    MapTargetNowCaliPara = item;// CameraParamList[e.RowIndex];
                            }
                            if (NowCaliPara == null) return;
                            //////////////////////////////////////////
                            switch (NowCaliPara.CaliParam.CamCaliModel)
                            {
                                case enCamCaliModel.UpDnCamCalibWcs:
                                case enCamCaliModel.映射标定_世界:
                                    CamMapCalibParamSimpleForm mapCaliNow = new CamMapCalibParamSimpleForm(NowCaliPara);
                                    if (SystemParamManager.Instance.SysConfigParam.IsFormTopMost)
                                    {
                                        mapCaliNow.TopMost = true;
                                        mapCaliNow.ShowInTaskbar = true;
                                    }
                                    //mapCaliNow.TopMost = true;
                                    //mapCaliNow.ShowInTaskbar = true;
                                    mapCaliNow.Show();
                                    break;
                                case enCamCaliModel.UpDnCamCalibPix:
                                case enCamCaliModel.映射标定_像素:
                                    if (NowCaliPara == null) return;
                                    if (MapTargetNowCaliPara == null)
                                    {
                                        new Common.UserMessageForm("未指定目标相机，不能进行映射标定!").ShowDialog();
                                        //new UserMessageForm().ShowDialog("未指定目标相机，不能进行映射标定!");
                                        return;
                                    }
                                    UpDnCamCalibSimpleForm frmCaliNow = new UpDnCamCalibSimpleForm(NowCaliPara, MapTargetNowCaliPara);
                                    //frmCaliNow.TopMost = true;
                                    //frmCaliNow.ShowInTaskbar = true;
                                    if (SystemParamManager.Instance.SysConfigParam.IsFormTopMost)
                                    {
                                        frmCaliNow.TopMost = true;
                                        frmCaliNow.ShowInTaskbar = true;
                                    }
                                    frmCaliNow.Show();
                                    break;
                                case enCamCaliModel.CaliBoardMap:
                                case enCamCaliModel.标定板映射:
                                    if (NowCaliPara == null) return;
                                    if (MapTargetNowCaliPara == null)
                                    {
                                        new Common.UserMessageForm("未指定目标相机，不能进行映射标定!").ShowDialog();
                                        //new UserMessageForm().ShowDialog("未指定目标相机，不能进行映射标定!");
                                        return;
                                    }
                                    CaliboardMapSimpleForm frmCaliBoardNow = new CaliboardMapSimpleForm(NowCaliPara, MapTargetNowCaliPara);
                                    //frmCaliBoardNow.TopMost = true;
                                    //frmCaliBoardNow.ShowInTaskbar = true;
                                    if (SystemParamManager.Instance.SysConfigParam.IsFormTopMost)
                                    {
                                        frmCaliBoardNow.TopMost = true;
                                        frmCaliBoardNow.ShowInTaskbar = true;
                                    }
                                    frmCaliBoardNow.Show();
                                    break;
                                case enCamCaliModel.NPointCali:
                                case enCamCaliModel.九点标定:
                                    CamNPointCalibParamSimpleForm npointFrmCali = new CamNPointCalibParamSimpleForm(NowCaliPara);
                                    //npointFrmCali.TopMost = true;
                                    //npointFrmCali.ShowInTaskbar = true;
                                    if (SystemParamManager.Instance.SysConfigParam.IsFormTopMost)
                                    {
                                        npointFrmCali.TopMost = true;
                                        npointFrmCali.ShowInTaskbar = true;
                                    }
                                    npointFrmCali.Show();
                                    break;
                                case enCamCaliModel.HomMat2D:
                                case enCamCaliModel.HandEyeCali:
                                case enCamCaliModel.Cali9PtCali:
                                case enCamCaliModel.手眼标定:
                                    Cam9PointCalibrateSimpleForm frmCali = new Cam9PointCalibrateSimpleForm(NowCaliPara);
                                    //frmCali.TopMost = true;
                                    //frmCali.ShowInTaskbar = true;
                                    if (SystemParamManager.Instance.SysConfigParam.IsFormTopMost)
                                    {
                                        frmCali.TopMost = true;
                                        frmCali.ShowInTaskbar = true;
                                    }
                                    frmCali.Show();
                                    break;

                                case enCamCaliModel.CaliCaliBoard:
                                case enCamCaliModel.标定板标定:
                                    CaliCaliboardSimpleForm frmCaliboard = new CaliCaliboardSimpleForm(NowCaliPara);
                                    //frmCaliboard.TopMost = true;
                                    //frmCaliboard.ShowInTaskbar = true;
                                    if (SystemParamManager.Instance.SysConfigParam.IsFormTopMost)
                                    {
                                        frmCaliboard.TopMost = true;
                                        frmCaliboard.ShowInTaskbar = true;
                                    }
                                    frmCaliboard.Show();
                                    break;

                                case enCamCaliModel.CamParamPose:
                                case enCamCaliModel.内外参标定:
                                    AreaScanDivisionCalibrateForm matrixCalibrateForm = new AreaScanDivisionCalibrateForm(NowCaliPara);
                                    //matrixCalibrateForm.TopMost = true;
                                    //matrixCalibrateForm.ShowInTaskbar = true;
                                    if (SystemParamManager.Instance.SysConfigParam.IsFormTopMost)
                                    {
                                        matrixCalibrateForm.TopMost = true;
                                        matrixCalibrateForm.ShowInTaskbar = true;
                                    }
                                    matrixCalibrateForm.Show();
                                    break;

                                case enCamCaliModel.RefPose:
                                    CameraGlueGunCalibrateForm matrixCalibrateForm2 = new CameraGlueGunCalibrateForm(NowCaliPara);
                                    //matrixCalibrateForm2.TopMost = true;
                                    //matrixCalibrateForm2.ShowInTaskbar = true;
                                    if (SystemParamManager.Instance.SysConfigParam.IsFormTopMost)
                                    {
                                        matrixCalibrateForm2.TopMost = true;
                                        matrixCalibrateForm2.ShowInTaskbar = true;
                                    }
                                    matrixCalibrateForm2.Show();
                                    break;

                                case enCamCaliModel.NPointAndMapCalib:
                                case enCamCaliModel.九点_映射标定:
                                    CamNPointMapCalibrateSimpleForm pointMapCalibrateSimpleForm = new CamNPointMapCalibrateSimpleForm(NowCaliPara);
                                    //pointMapCalibrateSimpleForm.TopMost = true;
                                    //pointMapCalibrateSimpleForm.ShowInTaskbar = true;
                                    if (SystemParamManager.Instance.SysConfigParam.IsFormTopMost)
                                    {
                                        pointMapCalibrateSimpleForm.TopMost = true;
                                        pointMapCalibrateSimpleForm.ShowInTaskbar = true;
                                    }
                                    pointMapCalibrateSimpleForm.Show();
                                    break;

                                default:
                                    frmCali = new Cam9PointCalibrateSimpleForm(NowCaliPara);   //
                                    break;
                            }
                            break;
                        case "MapCalibBtn":
                            NowCaliPara = null;
                            MapTargetNowCaliPara = null;
                            foreach (var item in CameraParamList)
                            {
                                if (eyeHandleCaliParamListCam[e.RowIndex].CamName == item.CaliParam.CamName)
                                    NowCaliPara = item; // CameraParamList[e.RowIndex];
                                if (eyeHandleCaliParamListCam[e.RowIndex].MapCamName == item.CaliParam.CamName)
                                    MapTargetNowCaliPara = item;// CameraParamList[e.RowIndex];
                            }
                            if (NowCaliPara == null) return;
                            /////////////////////////////////////////////
                            switch (NowCaliPara.CaliParam.CamCaliModel)
                            {
                                case enCamCaliModel.CaliBoardMap:
                                case enCamCaliModel.标定板映射:
                                    if (MapTargetNowCaliPara == null)
                                    {
                                        new Common.UserMessageForm("未指定目标相机，不能进行映射标定!").ShowDialog();
                                        //new UserMessageForm().ShowDialog("未指定目标相机，不能进行映射标定!");
                                        return;
                                    }
                                    CaliboardMapSimpleForm frmCaliBoardNow = new CaliboardMapSimpleForm(NowCaliPara, MapTargetNowCaliPara);
                                    //frmCaliBoardNow.TopMost = true;
                                    //frmCaliBoardNow.ShowInTaskbar = true;
                                    if (SystemParamManager.Instance.SysConfigParam.IsFormTopMost)
                                    {
                                        frmCaliBoardNow.TopMost = true;
                                        frmCaliBoardNow.ShowInTaskbar = true;
                                    }
                                    frmCaliBoardNow.Show();
                                    break;
                                default:
                                case enCamCaliModel.UpDnCamCalibWcs:
                                case enCamCaliModel.映射标定_世界:
                                case enCamCaliModel.九点标定:
                                    CamMapCalibParamSimpleForm mapCaliNow = new CamMapCalibParamSimpleForm(NowCaliPara);
                                    if (SystemParamManager.Instance.SysConfigParam.IsFormTopMost)
                                    {
                                        mapCaliNow.TopMost = true;
                                        mapCaliNow.ShowInTaskbar = true;
                                    }
                                    mapCaliNow.Show();
                                    break;
                                case enCamCaliModel.UpDnCamCalibPix:
                                case enCamCaliModel.映射标定_像素:
                                    if (MapTargetNowCaliPara == null)
                                    {
                                        new Common.UserMessageForm("未指定目标相机，不能进行映射标定!").ShowDialog();
                                           //new UserMessageForm().ShowDialog("未指定目标相机，不能进行映射标定!");
                                        return;
                                    }
                                    UpDnCamCalibSimpleForm frmCaliNow2 = new UpDnCamCalibSimpleForm(NowCaliPara, MapTargetNowCaliPara);
                                    //frmCaliNow2.TopMost = true;
                                    //frmCaliNow2.ShowInTaskbar = true;
                                    if (SystemParamManager.Instance.SysConfigParam.IsFormTopMost)
                                    {
                                        frmCaliNow2.TopMost = true;
                                        frmCaliNow2.ShowInTaskbar = true;
                                    }
                                    frmCaliNow2.Show();
                                    break;
                            }
                            break;
                        case "DistortionBtn":
                            NowCaliPara = CameraParamList[e.RowIndex];
                            CamDistortionCalibrateForm distortionForm = new CamDistortionCalibrateForm(NowCaliPara);
                            //DistortionCalibForm distortionForm = new DistortionCalibForm(NowCaliPara);
                            if (SystemParamManager.Instance.SysConfigParam.IsFormTopMost)
                            {
                                distortionForm.TopMost = true;
                                distortionForm.ShowInTaskbar = true;
                            }
                            distortionForm.Show();
                            break; // 
                        case "CalibSlantBtn":
                            NowCaliPara = CameraParamList[e.RowIndex];
                            CaliCamSlantForm slantForm = new CaliCamSlantForm(NowCaliPara);
                            if (SystemParamManager.Instance.SysConfigParam.IsFormTopMost)
                            {
                                slantForm.TopMost = true;
                                slantForm.ShowInTaskbar = true;
                            }
                            slantForm.Show();
                            break; // CalibSlantBtn
                        case "SaveBtn":
                            foreach (var item in CameraParamList)
                            {
                                if (eyeHandleCaliParamListCam[e.RowIndex].CamName == item.CaliParam.CamName)
                                {
                                    item.Save();
                                    //if (SensorManage.GetSensor(item.SensorName).ConfigParam.ConnectType == enUserConnectType.Socket)
                                    //{
                                    //    SocketBase socket1 = SocketConnectManager.Instance.GetSocket(SensorManage.GetSensor(item.SensorName).ConfigParam.ConnectAddress);
                                    //    if (socket1 != null)
                                    //    {
                                    //        if (new UserMessageForm().ShowDialog("是否同步更新服务器参数?", "更新参数") == DialogResult.OK)
                                    //        {
                                    //            SocketMessage message = new SocketMessage(enSocketInfo.写入相机参数, item.SensorName);
                                    //            message.Name = item.SensorName;
                                    //            object recive = socket1.GetDataAsync(message, true, 10000);
                                    //            message = recive as SocketMessage;
                                    //            if (message != null && message.MesContent.ToString().Trim() == "OK")
                                    //                new UserMessageForm().ShowDialog("更新相机参数成功!", "更新相机参数");
                                    //            else
                                    //                new UserMessageForm().ShowDialog("更新相机参数失败!", "更新相机参数");
                                    //        }
                                    //    }
                                    //}
                                }
                            }
                            break;
                        case "DeleteBtn":
                            foreach (var item in CameraParamList)
                            {
                                if (eyeHandleCaliParamListCam[e.RowIndex].CamName == item.SensorName)
                                {
                                    CameraParamList.Remove(item);
                                    eyeHandleCaliParamListCam.RemoveAt(e.RowIndex);
                                }
                            }
                            break;
                        case "ReadCol":
                            //if (SensorManage.GetSensor(CameraParamList[e.RowIndex].SensorName).ConfigParam.ConnectType == enUserConnectType.Socket)
                            //{
                            //    SocketBase socket = SocketConnectManager.Instance.GetSocket(SensorManage.GetSensor(CameraParamList[e.RowIndex].SensorName).ConfigParam.ConnectAddress);
                            //    if (socket != null)
                            //    {
                            //        SocketMessage message = new SocketMessage();
                            //        message.Lable = enSocketInfo.读取相机参数;
                            //        message.Name = CameraParamList[e.RowIndex].SensorName;
                            //        message.MesContent = "";
                            //        object oo = socket.GetDataAsync(message, true, 10000);
                            //        if (oo != null)
                            //        {
                            //            SocketMessage mesg = oo as SocketMessage;
                            //            if (mesg?.MesContent != null)
                            //            {
                            //                switch (mesg?.MesContent.GetType().Name)
                            //                {
                            //                    case nameof(CameraParam):
                            //                        CameraParam camParam = mesg.MesContent as CameraParam;


                            //                        //CameraParamList[e.RowIndex] = listCam;
                            //                        //eyeHandleCaliParamListCam[e.RowIndex] = CameraParamList[e.RowIndex].CaliParam; // 赋值 9 点标定参数
                            //                        //////////////////////////////
                            //                        new UserMessageForm().ShowDialog("加载相机成功");
                            //                        break;
                            //                    default:
                            //                        new UserMessageForm().ShowDialog("加载相机失败,加载对象的数据内容为非指定类型!");
                            //                        break;
                            //                }
                            //            }
                            //            else
                            //                new UserMessageForm().ShowDialog("加载失败,加载对象的数据内容为空!");
                            //        }
                            //        else
                            //            new UserMessageForm().ShowDialog("加载失败,加载对象为空!");
                            //    }
                            //    else
                            //        new UserMessageForm().ShowDialog("加载失败,网络 socket 对象为空!");
                            //}
                            break;
                            //case "WriteCol":
                            //    socket = SocketConnectManager.Instance.GetSocket(SensorManage.GetSensor(CameraParamList[e.RowIndex].SensorName).ConfigParam.ConnectAddress);
                            //    if (socket != null)
                            //    {
                            //        DialogResult dialogResult = new UserMessageForm().ShowDialog("确定要将当前相机参数下载到服务器吗?", "下载相机参数");
                            //        if (dialogResult == DialogResult.OK)
                            //        {
                            //            SocketMessage message = new SocketMessage();
                            //            message.Lable = enSocketInfo.写入相机参数;
                            //            message.Content = CameraParamList[e.RowIndex];
                            //            socket.SendDataAsync(message, true);
                            //            if (socket.WaitReceive())
                            //                new UserMessageForm().ShowDialog(socket.GetDataAsync().ToString());
                            //            else
                            //                new UserMessageForm().ShowDialog("下载相机参数到服务器失败!");
                            //        }
                            //    }
                            //    break;
                    }
                    this.dataGridView相机.Refresh();
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void dataGridView激光_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    switch (dataGridView激光.Columns[e.ColumnIndex].Name)
                    {
                        case "LaserCalibBtn":
                            LaserParam NowCaliPara = null;
                            LaserParam MapNowCaliPara = null;
                            foreach (var item in LaserParamList)
                            {
                                if (eyeHandleCaliParamListLaser[e.RowIndex].CamName == item.CaliParam.CamName)
                                    NowCaliPara = item; // CameraParamList[e.RowIndex];
                                if (eyeHandleCaliParamListLaser[e.RowIndex].MapCamName == item.CaliParam.CamName)
                                    MapNowCaliPara = item;// CameraParamList[e.RowIndex];
                            }
                            if (NowCaliPara == null) return;
                            //////////////////////////////////////////
                            switch (NowCaliPara.CaliParam.CamCaliModel)
                            {
                                //case enCamCaliModel.UpDnCamCali:
                                //    UpDnCamCaliForm frmCaliNow = new UpDnCamCaliForm(NowCaliPara, MapNowCaliPara);
                                //    frmCaliNow.ShowDialog();
                                //    break;
                                //case enCamCaliModel.NPointCali:
                                //    CamNPointCalibParamForm npointFrmCali = new CamNPointCalibParamForm(NowCaliPara);
                                //    npointFrmCali.ShowDialog();
                                //    break;
                                case enCamCaliModel.HomMat2D:
                                case enCamCaliModel.HandEyeCali:
                                case enCamCaliModel.Cali9PtCali:
                                    Laser9PointCalibrateForm frmCali = new Laser9PointCalibrateForm(NowCaliPara);
                                    frmCali.ShowDialog();
                                    break;

                                //case enCamCaliModel.CaliCaliBoard:
                                //    CaliCaliboardForm frmCaliboard = new CaliCaliboardForm(NowCaliPara);
                                //    frmCaliboard.ShowDialog();
                                //    break;

                                //case enCamCaliModel.CamParamPose:
                                //    AreaScanDivisionCalibrateForm matrixCalibrateForm = new AreaScanDivisionCalibrateForm(NowCaliPara);
                                //    matrixCalibrateForm.ShowDialog();
                                //    break;

                                //case enCamCaliModel.RefPose:
                                //    CameraGlueGunCalibrateForm matrixCalibrateForm2 = new CameraGlueGunCalibrateForm(NowCaliPara);
                                //    matrixCalibrateForm2.ShowDialog();
                                //    break;

                                default:
                                    frmCali = new Laser9PointCalibrateForm(NowCaliPara);   //
                                    break;
                            }
                            break;
                        case "LaserMapCalibBtn":

                            break;
                        //case "DistortionBtn":
                        //    NowCaliPara = LaserParamList[e.RowIndex];
                        //    CamDistortionCalibrateForm distortionForm = new CamDistortionCalibrateForm(NowCaliPara);
                        //    distortionForm.ShowDialog();
                        //    break; // 
                        //case "CalibSlantBtn":
                        //    NowCaliPara = LaserParamList[e.RowIndex];
                        //    CaliCamSlantForm slantForm = new CaliCamSlantForm(NowCaliPara);
                        //    slantForm.ShowDialog();
                        //    break; // CalibSlantBtn
                        case "LaserSaveBtn":
                            foreach (var item in LaserParamList)
                            {
                                if (eyeHandleCaliParamListLaser[e.RowIndex].CamName == item.CaliParam.CamName)
                                    item.Save();
                            }
                            break;
                        case "LaserDeleteBtn":
                            foreach (var item in LaserParamList)
                            {
                                if (eyeHandleCaliParamListLaser[e.RowIndex].CamName == item.SensorName)
                                {
                                    LaserParamList.Remove(item);
                                    eyeHandleCaliParamListLaser.RemoveAt(e.RowIndex);
                                }

                            }
                            break;
                    }
                    this.dataGridView激光.Refresh();
                }
            }
            catch (Exception ex)
            {
            }
        }
        private void CaliParaManagerForm_FormClosing(object sender, FormClosingEventArgs e)
        {

        }

        private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false;
        }

        private void 下载到服务器激光Btn_Click(object sender, EventArgs e)
        {
            try
            {
                SocketBase socket = null;//SocketConnectManager.Instance.GetSocket(this.激光服务器comboBox.Text);
                if (socket != null)
                {
                   
                    DialogResult dialogResult = new Common.UserMessageForm("确定要将当前配置下载到服务器吗?", "下载配置").ShowDialog();
                    if (dialogResult == DialogResult.OK)
                    {
                        SocketMessage message = new SocketMessage();
                        message.Lable = enSocketInfo.下载激光参数配置;
                        message.MesContent = this.eyeHandleCaliParamListLaser;
                        socket.SendDataAsync(message, true);
                        if (socket.WaitReceive())
                            new Common.UserMessageForm(socket.GetDataAsync().ToString()).ShowDialog();
                        //new UserMessageForm().ShowDialog(socket.GetDataAsync().ToString());
                        else
                            new Common.UserMessageForm("下载到服务器失败").ShowDialog();
                        //new UserMessageForm().ShowDialog("下载到服务器失败!");
                    }
                }
            }
            catch (Exception ex)
            {
                new Common.UserMessageForm("下载到服务器失败" + ex).ShowDialog();
                //new UserMessageForm().ShowDialog("下载到服务器失败" + ex);
            }
        }

        private void 加载服务器配置激光Btn_Click(object sender, EventArgs e)
        {
            try
            {
                SocketBase socket = null;// SocketConnectManager.Instance.GetSocket(this.激光服务器comboBox.Text);
                if (socket != null)
                {
                    SocketMessage message = new SocketMessage();
                    message.Lable = enSocketInfo.加载激光参数配置;
                    message.MesContent = this.eyeHandleCaliParamListLaser;
                    socket.SendDataAsync(message, true);
                    socket.WaitReceive();
                    object oo = socket.GetDataAsync();
                    if (oo != null)
                    {
                        switch (oo.GetType().Name)
                        {
                            case "BindingList`1":
                                Type[] type = oo.GetType().GenericTypeArguments;
                                if (type.Length > 0)
                                {
                                    switch (type[0].Name)
                                    {
                                        case nameof(CameraParam):
                                            BindingList<CameraParam> listCam = oo as BindingList<CameraParam>;
                                            this.CameraParamList?.Clear();
                                            this.eyeHandleCaliParamListCam?.Clear();
                                            foreach (var item in listCam)
                                            {
                                                CameraParamList.Add(item);
                                                eyeHandleCaliParamListCam.Add(item?.CaliParam);
                                            }
                                            break;
                                        case nameof(LaserParam):
                                            BindingList<LaserParam> listLaser = oo as BindingList<LaserParam>;
                                            this.LaserParamList?.Clear();
                                            this.eyeHandleCaliParamListLaser?.Clear();
                                            foreach (var item in listLaser)
                                            {
                                                LaserParamList.Add(item);
                                                eyeHandleCaliParamListLaser.Add(item?.CaliParam);
                                            }
                                            break;
                                        default:
                                            new Common.UserMessageForm("加载失败,加载的参数类型错误!").ShowDialog();
                                            //new UserMessageForm().ShowDialog("加载失败,加载的参数类型错误!");
                                            break;
                                    }
                                }
                                break;
                            case "List`1":
                                type = oo.GetType().GenericTypeArguments;
                                if (type.Length > 0)
                                {
                                    switch (type[0].Name)
                                    {
                                        case nameof(CameraParam):
                                            List<CameraParam> listCam = oo as List<CameraParam>;
                                            this.CameraParamList?.Clear();
                                            this.eyeHandleCaliParamListCam?.Clear();
                                            foreach (var item in listCam)
                                            {
                                                CameraParamList.Add(item);
                                                eyeHandleCaliParamListCam.Add(item?.CaliParam);
                                            }
                                            break;
                                        case nameof(LaserParam):
                                            List<LaserParam> listLaser = oo as List<LaserParam>;
                                            this.LaserParamList?.Clear();
                                            this.eyeHandleCaliParamListLaser?.Clear();
                                            foreach (var item in listLaser)
                                            {
                                                LaserParamList.Add(item);
                                                eyeHandleCaliParamListLaser.Add(item?.CaliParam);
                                            }
                                            break;
                                        default:
                                            new Common.UserMessageForm("加载失败,加载的参数类型错误!").ShowDialog();
                                            //new UserMessageForm().ShowDialog("加载失败,加载的参数类型错误!");
                                            break;
                                    }
                                }
                                break;
                        }
                        new Common.UserMessageForm("加载成功").ShowDialog();
                        //new UserMessageForm().ShowDialog("加载成功");
                    }
                    else
                        new Common.UserMessageForm("加载失败,加载对象为空!").ShowDialog();
                    //new UserMessageForm().ShowDialog("加载失败,加载对象为空!");
                }
            }
            catch (Exception ex)
            {
               new Common.UserMessageForm(ex.ToString()).ShowDialog();;
            }
        }

        private void 下载到服务器相机Btn_Click(object sender, EventArgs e)
        {
            try
            {
                if (new Common.UserMessageForm("是否同步更新服务器参数?", "更新参数").ShowDialog() == DialogResult.OK)
                {
                    foreach (var item in CameraParamList)
                    {
                        if (SensorManage.GetSensor(item.SensorName).ConfigParam.ConnectType == enUserConnectType.Socket)
                        {
                            SocketBase socket1 = SocketConnectManager.Instance.GetSocket(SensorManage.GetSensor(item.SensorName).ConfigParam.ConnectAddress);
                            if (socket1 != null)
                            {
                                SocketMessage message = new SocketMessage(enSocketInfo.写入相机参数);
                                message.Name = item.SensorName;
                                message.MesContent = item;
                                object recive = socket1.GetDataAsync(message, true, 10000);
                                message = recive as SocketMessage;
                                if (message != null && message.MesContent.ToString().Trim() == "OK")
                                    new Common.UserMessageForm("更新相机参数成功!", "更新相机参数").ShowDialog();
                                //new UserMessageForm().ShowDialog("更新相机参数成功!", "更新相机参数");
                                else
                                    new Common.UserMessageForm("更新相机参数失败!", "更新相机参数").ShowDialog();
                                    //new UserMessageForm().ShowDialog("更新相机参数失败!", "更新相机参数");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                new Common.UserMessageForm("下载到服务器失败" + ex).ShowDialog();
                //new UserMessageForm().ShowDialog("下载到服务器失败" + ex);
            }
        }

        private void 加载服务器配置相机Btn_Click(object sender, EventArgs e)
        {
            try
            {
                
                if (new Common.UserMessageForm("是否加载服务器参数并同步更新?", "加载参数").ShowDialog() == DialogResult.OK)
                {
                    foreach (var item in SensorManage.CameraList)
                    {
                        if (item.ConfigParam.ConnectType == enUserConnectType.Socket)
                        {
                            SocketBase socket = SocketConnectManager.Instance.GetSocket(item.ConfigParam.ConnectAddress);
                            if (socket != null)
                            {
                                SocketMessage message = new SocketMessage();
                                message.Lable = enSocketInfo.读取相机参数;
                                message.Name = item.Name;
                                object oo = socket.GetDataAsync(message, true, 20000);
                                if (oo != null)
                                {
                                    SocketMessage mesg = oo as SocketMessage;
                                    if (mesg?.MesContent != null)
                                    {
                                        switch (mesg.MesContent.GetType().Name)
                                        {
                                            case nameof(CameraParam):
                                                CameraParam camera = mesg.MesContent as CameraParam;
                                                //item.CameraParam = camera;
                                                item.CameraParam.CopyPropertyValue(camera);
                                                //////////////////////////////
                                                new Common.UserMessageForm("加载成功").ShowDialog();
                                                //new UserMessageForm().ShowDialog("加载成功");
                                                break;
                                            case nameof(LaserParam):
                                                //////////////////////////////
                                                new Common.UserMessageForm("加载成功").ShowDialog();
                                                //new UserMessageForm().ShowDialog("加载成功");
                                                break;
                                            default:
                                                new Common.UserMessageForm("加载失败,加载的参数类型错误!").ShowDialog();
                                                //new UserMessageForm().ShowDialog("加载失败,加载的参数类型错误!");
                                                break;
                                        }
                                    }
                                    else
                                        new Common.UserMessageForm("加载失败,加载对象的数据内容为空!").ShowDialog();
                                    //new UserMessageForm().ShowDialog("加载失败,加载对象的数据内容为空!");
                                }
                                else
                                    new Common.UserMessageForm("加载失败,加载对象为空!").ShowDialog();
                                //new UserMessageForm().ShowDialog("加载失败,加载对象为空!");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                new Common.UserMessageForm(ex.ToString()).ShowDialog();
            }
        }




    }
}

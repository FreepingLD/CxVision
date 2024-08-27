using Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Sensor;

namespace FunctionBlock
{
    public partial class CaliParaManagerForm : Form
    {
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
            /////////////////////////////////////////////////////
            this.dataGridView相机.DataSource = eyeHandleCaliParamListCam;
            this.dataGridView激光.DataSource = eyeHandleCaliParamListLaser;
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
                                case enCamCaliModel.UpDnCamCali:
                                case enCamCaliModel.MapCalib:
                                    if (NowCaliPara != null) return;
                                    if (MapTargetNowCaliPara == null)
                                    {
                                        MessageBox.Show("未指定目标相机，不能进行映射标定!");
                                        return;
                                    }
                                    UpDnCamCaliForm frmCaliNow = new UpDnCamCaliForm(NowCaliPara, MapTargetNowCaliPara);
                                    frmCaliNow.ShowDialog();
                                    break;
                                case enCamCaliModel.NPointCali:
                                    CamNPointCalibParamForm npointFrmCali = new CamNPointCalibParamForm(NowCaliPara);
                                    npointFrmCali.ShowDialog();
                                    break;
                                case enCamCaliModel.HomMat2D:
                                case enCamCaliModel.HandEyeCali:
                                case enCamCaliModel.Cali9PtCali:
                                    Cam9PointCalibrateForm frmCali = new Cam9PointCalibrateForm(NowCaliPara);
                                    frmCali.ShowDialog();
                                    break;

                                case enCamCaliModel.CaliCaliBoard:
                                    CaliCaliboardForm frmCaliboard = new CaliCaliboardForm(NowCaliPara);
                                    frmCaliboard.ShowDialog();
                                    break;

                                case enCamCaliModel.CamParamPose:
                                    AreaScanDivisionCalibrateForm matrixCalibrateForm = new AreaScanDivisionCalibrateForm(NowCaliPara);
                                    matrixCalibrateForm.ShowDialog();
                                    break;

                                case enCamCaliModel.RefPose:
                                    CameraGlueGunCalibrateForm matrixCalibrateForm2 = new CameraGlueGunCalibrateForm(NowCaliPara);
                                    matrixCalibrateForm2.ShowDialog();
                                    break;

                                default:
                                    frmCali = new Cam9PointCalibrateForm(NowCaliPara);   //
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
                            if (MapTargetNowCaliPara == null)
                            {
                                MessageBox.Show("未指定目标相机，不能进行映射标定!");
                                return;
                            }
                            UpDnCamCaliForm frmCaliNow2 = new UpDnCamCaliForm(NowCaliPara, MapTargetNowCaliPara);
                            frmCaliNow2.ShowDialog();
                            break;
                        case "DistortionBtn":
                            NowCaliPara = CameraParamList[e.RowIndex];
                            CamDistortionCalibrateForm distortionForm = new CamDistortionCalibrateForm(NowCaliPara);
                            //DistortionCalibForm distortionForm = new DistortionCalibForm(NowCaliPara);
                            distortionForm.ShowDialog();
                            break; // 
                        case "CalibSlantBtn":
                            NowCaliPara = CameraParamList[e.RowIndex];
                            CaliCamSlantForm slantForm = new CaliCamSlantForm(NowCaliPara);
                            slantForm.ShowDialog();
                            break; // CalibSlantBtn
                        case "SaveBtn":
                            foreach (var item in CameraParamList)
                            {
                                if (eyeHandleCaliParamListCam[e.RowIndex].CamName == item.CaliParam.CamName)
                                    item.Save();
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


    }
}

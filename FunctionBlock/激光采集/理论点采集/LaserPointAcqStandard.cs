using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MotionControlCard;
using System.Windows.Forms;
using System.Threading;
using HalconDotNet;
using Common;
using System.IO;
using Sensor;
using Command;
using AlgorithmsLibrary;
using System.ComponentModel;
using System.Data;


namespace FunctionBlock
{
    [Serializable]
    /// <summary>
    /// 激光取点采集的数据类
    /// </summary>
    public class LaserPointAcqStandard : BaseFunction, IFunction
    {
        private TransformLaserPointCloudDataHandle pointData1 = new TransformLaserPointCloudDataHandle();
        // 从机台读取的坐标值
        private DataTable coord1Table = new DataTable();
        private AcqSource _laserAcqSource;

        public AcqSource LaserAcqSource
        {
            get
            {
                return _laserAcqSource;
            }

            set
            {
                _laserAcqSource = value;
            }
        }
        public DataTable Coord1Table
        {
            get
            {
                return coord1Table;
            }

            set
            {
                coord1Table = value;
            }
        }

        public LaserPointAcqStandard(ISensor _laserSensor, IFunction coordSource)
        {
            if (_laserSensor != null)
                this._laserAcqSource = AcqSourceManage.Instance.GetAcqSource(_laserSensor.ConfigParam.SensorName); // new AcqSource(_laserSensor);
            if (coordSource != null)
                this.RefSource2.Add(coordSource.Name, coordSource);
            initTable();
        }
        private void initTable()
        {
            coord1Table.Columns.AddRange(new DataColumn[6] { new DataColumn("X坐标"), new DataColumn("Y坐标"), new DataColumn("Z坐标")
                , new DataColumn("U坐标"), new DataColumn("V坐标"), new DataColumn("W坐标") });
        }
        public userWcsCoordSystem extractRefSource2Data()
        {
            userWcsCoordSystem coordSystem = new userWcsCoordSystem();
            object object3D = null;
            // 获取所有3D对象模型
            foreach (var item in this.RefSource2.Keys)
            {
                if (item.Split('.').Length == 1)
                    object3D = this.RefSource2[item].GetPropertyValues(item);
                else
                    object3D = this.RefSource2[item].GetPropertyValues(item.Split('.')[1]);
                if (object3D != null) // 这样做是为了动态获取名称
                {
                    switch (object3D.GetType().Name)
                    {
                        case "userWcsCoordSystem":
                            coordSystem = (((userWcsCoordSystem)object3D));
                            break;
                        case "userWcsCoordSystem[]":
                            coordSystem = (((userWcsCoordSystem[])object3D))[0];
                            break;
                        case "userWcsPose3D":
                            coordSystem = (new userWcsCoordSystem(((userWcsPose)object3D)));
                            break;
                    }
                }
            }
            return coordSystem;
        }

        /// <summary>
        /// 获取当前机台位置
        /// </summary>
        /// <param name="card"></param>
        private void GetCurrentPosition(DataTable dataTable) // 保存后有意义的参数和保存后没意义的参数
        {
            double x_Tcoordinate = 0;
            double y_Tcoordinate = 0;
            double z_Tcoordinate = 0;
            double u_Tcoordinate = 0;
            double v_Tcoordinate = 0;
            double w_Tcoordinate = 0;
            try
            {
                _laserAcqSource.GetAxisPosition(enAxisName.X轴, out x_Tcoordinate);
                _laserAcqSource.GetAxisPosition(enAxisName.Y轴, out y_Tcoordinate);
                _laserAcqSource.GetAxisPosition(enAxisName.Z轴, out z_Tcoordinate);
                _laserAcqSource.GetAxisPosition(enAxisName.U轴, out u_Tcoordinate);
                _laserAcqSource.GetAxisPosition(enAxisName.V轴, out v_Tcoordinate);
                _laserAcqSource.GetAxisPosition(enAxisName.W轴, out w_Tcoordinate);
                dataTable.Rows.Add(x_Tcoordinate, y_Tcoordinate, z_Tcoordinate, u_Tcoordinate, v_Tcoordinate, w_Tcoordinate);
            }
            catch
            {
                throw new Exception();
            }
        }



        #region 实现接口,各自实现自己的逻辑，就可实现通用的逻辑
        public OperateResult Execute(params object[] param)
        {
            this.Result.Succss = false;
            Dictionary<enDataItem, object> list;
            userWcsPose laserAffinePose;
            MoveCommandParam CommandParam, affineCommandParam;
            DataRow[] dataCoord1Row = this.coord1Table.Select();
            try
            {
                pointData1.ClearData();
                this.pointData1.ClearHandle();
                this.resultDataTable.Rows.Clear();
                for (int i = 0; i < dataCoord1Row.Length; i++)
                {
                    CommandParam = new MoveCommandParam(enAxisName.XYZ轴, GlobalVariable.pConfig.MoveSpeed);
                    CommandParam.AxisParam = new CoordSysAxisParam();
                    /////////////////////////////////////////
                    if (this._laserAcqSource == null) throw new ArgumentNullException("LaserAcqSource");
                    laserAffinePose = this._laserAcqSource.Sensor.LaserParam.LaserCalibrationParam.LaserAffinePose;
                    affineCommandParam = CommandParam.TransformStanderCameraCoordToLaserCoord(extractRefSource2Data(), laserAffinePose);
                    /////////////////////////////////////////////
                    list = _laserAcqSource.AcqPointData(affineCommandParam);
                    /// 这一步是为了让激光点的坐标与相机坐标相一致
                    pointData1.TransformDataAndCalibrateData(list, _laserAcqSource.GetSensorType(), _laserAcqSource.Sensor.LaserParam.DataWidth, _laserAcqSource.Sensor.LaserParam.DataHeight, _laserAcqSource.Sensor.LaserParam.ScanAxis, CommandParam.AxisParam, this.LaserAcqSource.Sensor.LaserParam.LaserCalibrationParam);
                    ////////
                    if ( this.pointData1.Dist1DataHandle.IsInitialized())
                    {
                        LoggerHelper.Info(this.name + "->激光采集点数:=" + this.pointData1.Dist1Count.ToString());
                        LoggerHelper.Info(this.name + "->激光有效点数:=" + this.pointData1.Dist1DataHandle.GetObjectModel3dParams("num_points").I.ToString());
                    }
                    else
                    {
                        LoggerHelper.Info(this.name + "->激光采集点数:=" + this.pointData1.Dist1Count.ToString());
                        LoggerHelper.Info(this.name + "->激光采集点数:= 0");
                    }
                    if (this.pointData1.Dist1DataHandle.IsInitialized())
                    {
                        OnPointsCloudAcqComplete(this.pointData1.Dist1DataHandle, this._laserAcqSource.Sensor.ConfigParam.SensorName);
                        OnExcuteCompleted(this.name, this.pointData1.Dist1DataHandle);
                    }
                }
                //////////////////

                    if (this.pointData1.Dist1DataHandle.IsInitialized())
                        this.resultDataTable.Rows.Add(this.name , "距离1", this.pointData1.Dist1DataHandle.GetObjectModel3dParams("num_points").I, this.pointData1.Dist1Count, 0, 0, "OK");
                    else
                        this.resultDataTable.Rows.Add(this.name, "距离1", 0, this.pointData1.Dist1Count, 0, 0, "OK");

                //////////////////////////////////////////

                    if (this.pointData1.Dist2DataHandle.IsInitialized())
                        this.resultDataTable.Rows.Add(this.name , "距离2", this.pointData1.Dist2DataHandle.GetObjectModel3dParams("num_points").I, this.pointData1.Dist2Count, 0, 0, "OK");
                    else
                        this.resultDataTable.Rows.Add(this.name , "距离2", 0, this.pointData1.Dist2Count, 0, 0, "OK");
  
                //////////////////////////////////////////

                    if (this.pointData1.ThickDataHandle.IsInitialized())
                        this.resultDataTable.Rows.Add(this.name , "厚度", this.pointData1.ThickDataHandle.GetObjectModel3dParams("num_points").I, this.pointData1.ThickCount, 0, 0, "OK");
                    else
                        this.resultDataTable.Rows.Add(this.name , "厚度", 0, this.pointData1.ThickCount, 0, 0, "OK");

                pointData1.ClearData();
                //   使用发命令的方式来更新视图  
                Result.Succss = true;
            }
            catch (Exception ee)
            {
                LoggerHelper.Error(this.name + "->激光数据采集报错" + ee);
                return Result;
            }
            if (Result.Succss)
                LoggerHelper.Info(this.name + "->激光数据采集完成");
            else
                LoggerHelper.Error(this.name + "->激光数据采集失败");
            return Result;
        }

        public object GetPropertyValues(string propertyName)
        {
            switch (propertyName)
            {
                case "坐标系":
                    return this.extractRefSource2Data();
                case "坐标点1":
                case "坐标点":
                    return this.coord1Table;
                case "距离1对象":
                    return this.pointData1.Dist1DataHandle;
                case "距离2对象":
                    return this.pointData1.Dist2DataHandle;
                case "厚度对象":
                    return this.pointData1.ThickDataHandle;
                case "名称":
                    return this.name;
                case "采集源":
                    return this._laserAcqSource;
                default:
                    if (this.name == propertyName)
                        return this.pointData1.Dist1DataHandle;
                    else return null;
            }
        }

        public bool SetPropertyValues(string propertyName, params object[] value)
        {
            string name = "";
            if (value != null)
                name = value[0].ToString();
            switch (propertyName)
            {
                case "名称":
                    this.name = value[0].ToString();
                    return true;
                default:
                    return true;
            }
        }

        public void ReleaseHandle()
        {
            this.pointData1.ClearHandle();

        }
        public void Read(string path)
        {
            //throw new NotImplementedException();
        }
        public void Save(string path)
        {
           // throw new NotImplementedException();
        }
        #endregion

        public enum enShowItems
        {
            距离1对象,
            距离2对象,
            厚度对象,
        }

    }

}

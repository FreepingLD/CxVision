using Sensor;
using MotionControlCard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HalconDotNet;
using System.Threading;
using Common;
using System.IO;
using Command;
using AlgorithmsLibrary;
using System.ComponentModel;
using System.Data;


namespace FunctionBlock
{
    [Serializable]
    /// <summary>
    /// 激光扫描采集的数据类
    /// </summary>
    public class LaserScanAcqStandard : BaseFunction, IFunction
    {
        private TransformLaserPointCloudDataHandle pointData1 = new TransformLaserPointCloudDataHandle();
        private AcqSource _laserAcqSource;
        private AcqSource _camAcqSource;
        private float move_Step = 0;
        // 从机台读取的坐标值
        private DataTable coord1Table = new DataTable();
        // private DataTable coord2Table = new DataTable();
        private IFunction coordSystem;
        private double offsetDist = 0; // 插补起点相对于圆心的偏置
        private int circleNum = 1;
        private enAxisName motionType = enAxisName.XY轴直线插补;
        public float Move_Step
        {
            get
            {
                return move_Step;
            }

            set
            {
                move_Step = value;
            }
        }
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
        public AcqSource CamAcqSource
        {
            get
            {
                return _camAcqSource;
            }

            set
            {
                _camAcqSource = value;
            }
        }
        public IFunction CoordSystem
        {
            get
            {
                return coordSystem;
            }

            set
            {
                coordSystem = value;
            }
        }
        public double OffsetDist
        {
            get
            {
                return offsetDist;
            }

            set
            {
                offsetDist = value;
            }
        }
        public int CircleNum
        {
            get
            {
                return circleNum;
            }

            set
            {
                circleNum = value;
            }
        }
        public enAxisName MotionType
        {
            get
            {
                return motionType;
            }

            set
            {
                motionType = value;
            }
        }

        public LaserScanAcqStandard(ISensor _laserSensor, IFunction coordSource)
        {
            if (_laserSensor != null)
                this._laserAcqSource = AcqSourceManage.Instance.GetAcqSource(_laserSensor.ConfigParam.SensorName);// new AcqSource(_laserSensor);
            if (coordSource != null)
                this.RefSource2.Add(coordSource.Name, coordSource);
            initTable();
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
        private void initTable()
        {
            coord1Table.Columns.AddRange(new DataColumn[12] { new DataColumn("X1坐标"), new DataColumn("Y1坐标"), new DataColumn("Z1坐标")
                , new DataColumn("U1坐标"), new DataColumn("V1坐标"), new DataColumn("W1坐标") ,
            new DataColumn("X2坐标"), new DataColumn("Y2坐标"), new DataColumn("Z2坐标")
                , new DataColumn("U2坐标"), new DataColumn("V2坐标"), new DataColumn("W2坐标")});
        }





        #region 实现接口

        public OperateResult Execute(params object[] param)
        {
            this.Result.Succss = false;
            Dictionary<enDataItem, object> list;
            userWcsPose laserAffinePose;
            HalconLibrary ha = new HalconLibrary();
            MoveCommandParam CommandParam1, affineCommandParam1;
            MoveCommandParam CommandParam2, affineCommandParam2;
            DataRow[] dataCoord1Row = this.coord1Table.Select();
            try
            {
                if (this._laserAcqSource == null) throw new ArgumentNullException("LaserAcqSource");
                this.pointData1.ClearData();
                this.pointData1.ClearHandle();
                this.resultDataTable.Rows.Clear();
                for (int i = 0; i < dataCoord1Row.Length; i++)
                {
                    CommandParam1 = new MoveCommandParam(enAxisName.XYZ轴, GlobalVariable.pConfig.MoveSpeed);
                    CommandParam1.AxisParam = new CoordSysAxisParam();
                    /////////////////////////////////////////////////////////////////
                    CommandParam2 = new MoveCommandParam(this.motionType, GlobalVariable.pConfig.ScanSpeed);
                    CommandParam2.AxisParam = new CoordSysAxisParam();
                    /////////////////////////////////
                    laserAffinePose = this._laserAcqSource.Sensor.LaserParam.LaserCalibrationParam.LaserAffinePose;
                    affineCommandParam1 = CommandParam1.TransformStanderCameraCoordToLaserCoord(extractRefSource2Data(), laserAffinePose); // 
                    affineCommandParam2 = CommandParam2.TransformStanderCameraCoordToLaserCoord(extractRefSource2Data(), laserAffinePose);
                    ///////////////////////////////////////////
                    list = _laserAcqSource.AcqScanData(affineCommandParam1, affineCommandParam2);
                    /// 这一步是为了让激光点的坐标与相机坐标相一致
                    pointData1.TransformDataAndCalibrateData(list, _laserAcqSource.GetSensorType(), _laserAcqSource.NumPerLine(), _laserAcqSource.Sensor.LaserParam.ScanAxis, CommandParam1.AxisParam, CommandParam2.AxisParam, this.LaserAcqSource.Sensor.LaserParam.LaserCalibrationParam);
                    //////////////////////////////////////////
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
                        OnPointsCloudAcqComplete(this.pointData1.Dist1DataHandle, _laserAcqSource.Sensor.ConfigParam.SensorName);
                        OnExcuteCompleted(this.name, this.pointData1.Dist1DataHandle);
                    }
                }

                    if ( this.pointData1.Dist1DataHandle.IsInitialized())
                        this.resultDataTable.Rows.Add(this.name , "距离1", this.pointData1.Dist1DataHandle.GetObjectModel3dParams("num_points").I, this.pointData1.Dist1Value.Count, 0, 0, "OK");
                    else
                        this.resultDataTable.Rows.Add(this.name , "距离1", 0, this.pointData1.Dist1Value.Count, 0, 0, "OK");

                //////////////////////////////////////////////////////////

                    if ( this.pointData1.Dist2DataHandle.IsInitialized())
                        this.resultDataTable.Rows.Add(this.name , "距离2", this.pointData1.Dist2DataHandle.GetObjectModel3dParams("num_points").I, this.pointData1.Dist2Value.Count, 0, 0, "OK");
                    else
                        this.resultDataTable.Rows.Add(this.name , "距离2", 0, this.pointData1.Dist2Value.Count, 0, 0, "OK");

                //////////////////////////////////////////////////////////

                    if (this.pointData1.ThickDataHandle.IsInitialized())
                        this.resultDataTable.Rows.Add(this.name , "厚度", this.pointData1.ThickDataHandle.GetObjectModel3dParams("num_points").I, this.pointData1.ThickValue.Count, 0, 0, "OK");
                    else
                        this.resultDataTable.Rows.Add(this.name , "厚度", 0, this.pointData1.ThickValue.Count, 0, 0, "OK");

                /////////////////////////////////
                this.pointData1.ClearData();
                ////
                Result.Succss = true;
            }
            catch (Exception ex)
            {
                LoggerHelper.Error(this.name + "->数据采集报错" + ex);
                return Result;
            }
            if (Result.Succss)
                LoggerHelper.Info(this.name + "->数据采集完成"); //this.pointData1.Dist1DataHandle.GetObjectModel3dParams("num_points").I
            else
                LoggerHelper.Error(this.name + "->数据采集失败");
            return Result;
        }
        public object GetPropertyValues(string propertyName)
        {
            switch (propertyName)
            {
                case "坐标系":
                    return this.extractRefSource2Data();
                case "当前对象":
                    return this;
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
            try
            {
                switch (propertyName)
                {
                    case "名称":
                        this.name = value[0].ToString();
                        return true;
                    default:
                        return false;
                }
            }
            catch
            {
                return false;
            }
        }

        // 释放类中的对象句柄
        public void ReleaseHandle()
        {
            this.pointData1.ClearHandle();
        }

        public void Read(string path)
        {
           // throw new NotImplementedException();
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

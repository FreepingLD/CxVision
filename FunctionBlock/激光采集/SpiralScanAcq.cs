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
    public class SpiralScanAcq : BaseFunction, IFunction
    {
        private TransformLaserPointCloudDataHandle pointData1 = new TransformLaserPointCloudDataHandle();
        // 从机台读取的坐标值
        private BindingList<userWcsPoint> _AcqCoordPoint= new BindingList<userWcsPoint>();
        private string _LaserSensorName;
        private string _CamSensorName;
        private enCoordSysName _CoordSysName;

        private double tacc = 0.5;//加速时间
        private double tdec = 0.5;//减速时间
        private enAxisName moveAxis = enAxisName.XY轴圆弧插补;
        private double circleDir = 1; // 0:顺时针；1：逆时针
        private int circleCount = 1;
        private int circleRadius = 1;
        private int offset_z = 0;
        private double startPointOffset = 0;

        public double Tacc { get => tacc; set => tacc = value; }
        public double Tdec { get => tdec; set => tdec = value; }
        public enAxisName MoveAxis { get => moveAxis; set => moveAxis = value; }
        public double CircleDir { get => circleDir; set => circleDir = value; }
        public int CircleCount { get => circleCount; set => circleCount = value; }
        public int CircleRadius { get => circleRadius; set => circleRadius = value; }
        public int Offset_z { get => offset_z; set => offset_z = value; }
        public double StartPointOffset { get => startPointOffset; set => startPointOffset = value; }
        public BindingList<userWcsPoint> AcqCoordPoint { get => _AcqCoordPoint; set => _AcqCoordPoint = value; }
        public string LaserSensorName { get => _LaserSensorName; set => _LaserSensorName = value; }
        public string CamSensorName { get => _CamSensorName; set => _CamSensorName = value; }
        public enCoordSysName CoordSysName { get => _CoordSysName; set => _CoordSysName = value; }

        public SpiralScanAcq(string _laserSensorName, string _camSensorName, IFunction coordSystem)
        {
            this._LaserSensorName = _laserSensorName;
            this._CamSensorName = _camSensorName;
            this._CoordSysName = MotionCardManage.CurrentCoordSys;
            if (coordSystem != null)
                this.RefSource2.Add(coordSystem.Name, coordSystem);

            ///////////////////////////////////////////////////
            this.ResultInfo = new BindingList<AcqResultInfo>();
            ((BindingList<AcqResultInfo>)this.ResultInfo).Add(new AcqResultInfo(this.name, "X", 0));
            ((BindingList<AcqResultInfo>)this.ResultInfo).Add(new AcqResultInfo(this.name, "Y", 0));
            ((BindingList<AcqResultInfo>)this.ResultInfo).Add(new AcqResultInfo(this.name, "Dist1", 0));
            ((BindingList<AcqResultInfo>)this.ResultInfo).Add(new AcqResultInfo(this.name, "Dist2", 0));
            ((BindingList<AcqResultInfo>)this.ResultInfo).Add(new AcqResultInfo(this.name, "Thick", 0));
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




        #region 实现接口,各自实现自己的逻辑，就可实现通用的逻辑
        public OperateResult Execute(params object[] param)
        {
            this.Result.Succss = false;
            HalconLibrary ha = new HalconLibrary();
            Dictionary<enDataItem, object> list;
            MoveCommandParam CommandParam1, CommandParam2;
            CommandParam1 = new MoveCommandParam(enAxisName.XYZ轴, GlobalVariable.pConfig.ScanSpeed);
            CommandParam2 = new MoveCommandParam(this.moveAxis, GlobalVariable.pConfig.ScanSpeed);
            try
            {
                this.pointData1.ClearData();
                this.pointData1.ClearHandle();
                this.resultDataTable.Rows.Clear();
                foreach (var item in this.AcqCoordPoint)
                {
                    CommandParam2.MoveAxis = this.moveAxis;
                    CommandParam2.Tacc = this.tacc;
                    CommandParam2.Tdec = this.tdec;
                    ////// 对上都所有运动共用的参数
                    CommandParam2.CircleParam.Dir = (ushort)this.circleDir;
                    CommandParam2.CircleParam.Count = this.circleCount;
                    CommandParam2.CircleParam.Radius = this.circleRadius;
                    CommandParam2.CircleParam.OffsetValue_z = this.offset_z;
                    CommandParam2.CircleParam.StartPointOffset = this.startPointOffset;
                    switch (this.moveAxis)
                    {
                        case enAxisName.XY螺旋线插补:
                        case enAxisName.XY轴圆弧插补:
                            CommandParam1.AxisParam = new CoordSysAxisParam(item.X + this.startPointOffset, item.Y, item.Z, item.Theta, item.U, item.V);
                            /////////////////////////////////////////////////////////////////
                            CommandParam2.AxisParam = new CoordSysAxisParam(item.X + this.circleRadius, item.Y, item.Z, item.Theta, item.U, item.V);
                            CommandParam2.CircleParam.centerPosition = new double[] { item.X, item.Y, item.Z, item.Theta, item.U, item.V };
                            break;
                        case enAxisName.XYZ螺旋线插补:
                        case enAxisName.XYZ轴圆弧插补:
                            CommandParam1.AxisParam = new CoordSysAxisParam(item.X + this.startPointOffset, item.Y, item.Z, item.Theta, item.U, item.V);
                            /////////////////////////////////////////////////////////////////
                            CommandParam2.AxisParam = new CoordSysAxisParam(item.X + this.circleRadius, item.Y, item.Z, item.Theta, item.U, item.V);
                            CommandParam2.CircleParam.centerPosition = new double[] { item.X, item.Y, item.Z, item.Theta, item.U, item.V };
                            break;
                        default:
                            throw new Exception("圆弧扫描不支持该运动");
                    }
                    /////////////////////////////////////////////
                    list = AcqSourceManage.Instance.GetAcqSource(this._LaserSensorName).AcqScanData(CommandParam1, CommandParam2);
                    /// 这一步是为了让激光点的坐标与相机坐标相一致    
                    //////////////////////
                    pointData1.TransformDataAndCalibrateData(list, 
                         SensorManage.GetSensor(this._LaserSensorName).ConfigParam.SensorType,
                         SensorManage.GetSensor(this._LaserSensorName).LaserParam.DataWidth,
                         SensorManage.GetSensor(this._LaserSensorName).LaserParam.ScanAxis, 
                        CommandParam1.AxisParam, 
                        CommandParam2.AxisParam,
                         SensorManage.GetSensor(this._LaserSensorName).LaserParam.LaserCalibrationParam);
                    //////////////////////
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
                        OnPointsCloudAcqComplete(this.pointData1.Dist1DataHandle, SensorManage.GetSensor(this._LaserSensorName).ConfigParam.SensorName);
                        OnExcuteCompleted(this.name, this.pointData1.Dist1DataHandle);
                    }
                    //////////// 只包含一个产品的数据                 
                }
                ///////////////////////////////////////////////////////////

                    if (this.pointData1.Dist1DataHandle.IsInitialized())
                        this.resultDataTable.Rows.Add(this.name , "距离1", this.pointData1.Dist1DataHandle.GetObjectModel3dParams("num_points").I, this.pointData1.Dist1Count, 0, 0, "OK");
                    else
                        this.resultDataTable.Rows.Add(this.name , "距离1", 0, this.pointData1.Dist1Count, 0, 0, "OK");

                //////////////////////////////////////////

                    if ( this.pointData1.Dist2DataHandle.IsInitialized())
                        this.resultDataTable.Rows.Add(this.name , "距离2", this.pointData1.Dist2DataHandle.GetObjectModel3dParams("num_points").I, this.pointData1.Dist2Count, 0, 0, "OK");
                    else
                        this.resultDataTable.Rows.Add(this.name , "距离2", 0, this.pointData1.Dist2Count, 0, 0, "OK");

                //////////////////////////////////////////

                    if ( this.pointData1.ThickDataHandle.IsInitialized())
                        this.resultDataTable.Rows.Add(this.name , "厚度", this.pointData1.ThickDataHandle.GetObjectModel3dParams("num_points").I, this.pointData1.ThickCount, 0, 0, "OK");
                    else
                        this.resultDataTable.Rows.Add(this.name , "厚度", 0, this.pointData1.ThickCount, 0, 0, "OK");

                // 使用发命令的方式来更新视图  
                Result.Succss = true;
                if (Result.Succss)
                    LoggerHelper.Info(this.name + "激光数据采集完成");
                else
                    LoggerHelper.Error(this.name + "激光数据采集失败");
            }
            catch (Exception e)
            {
                LoggerHelper.Error(this.name + "激光数据采集报错" + e);
                return Result;
            }
            finally
            {
                /////////////////////
                pointData1.ClearData();
            }
            return Result;
        }

        public object GetPropertyValues(string propertyName)
        {
            switch (propertyName)
            {
                case "坐标系":
                    return this.extractRefSource2Data();
                case "距离1":
                case "距离1对象":
                    return this.pointData1.Dist1DataHandle;
                case "距离2":
                case "距离2对象":
                    return this.pointData1.Dist2DataHandle;
                case "厚度":
                case "厚度对象":
                    return this.pointData1.ThickDataHandle;
                case "名称":
                    return this.name;
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



    }

}

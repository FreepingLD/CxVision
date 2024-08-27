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
    public class RectangleScanAcq : BaseFunction, IFunction
    {
        private TransformLaserPointCloudDataHandle pointData1 = new TransformLaserPointCloudDataHandle();
        private BindingList<userWcsRectangle2> _AcqCoordRect2 = new BindingList<userWcsRectangle2>();
        private string _LaserSensorName;
        private string _CamSensorName;
        private enCoordSysName _CoordSysName;
        // 从机台读取的坐标值
        private IFunction coordSystem;
        private enAxisName motionType = enAxisName.XY轴矩形插补;
        private double vector_x = 0.5;
        private double vector_y = 0.5;
        private int lineCount = 1;
        private ushort interpolateMode = 0;
        private double tacc = 0.5;//加速时间
        private double tdec = 0.5;//减速时间


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

        public double Vector_x { get => vector_x; set => vector_x = value; }
        public double Vector_y { get => vector_y; set => vector_y = value; }
        public int LineCount { get => lineCount; set => lineCount = value; }
        public ushort InterpolateMode { get => interpolateMode; set => interpolateMode = value; }
        public double Tacc { get => tacc; set => tacc = value; }
        public double Tdec { get => tdec; set => tdec = value; }
        public BindingList<userWcsRectangle2> AcqCoordRect2 { get => _AcqCoordRect2; set => _AcqCoordRect2 = value; }
        public string LaserSensorName { get => _LaserSensorName; set => _LaserSensorName = value; }
        public string CamSensorName { get => _CamSensorName; set => _CamSensorName = value; }
        public enCoordSysName CoordSysName { get => _CoordSysName; set => _CoordSysName = value; }

        public RectangleScanAcq()
        {

        }
        public RectangleScanAcq(string _laserSensorName, string _camSensorName, IFunction coordSource)
        {
            this._LaserSensorName = _laserSensorName;
            this._CamSensorName = _camSensorName;
            this._CoordSysName = MotionCardManage.CurrentCoordSys;
            if (coordSource != null)
                this.RefSource2.Add(coordSource.Name, coordSource);

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


        #region 实现接口

        public OperateResult Execute(params object[] param)
        {
            this.Result.Succss = false;
            Dictionary<enDataItem, object> list;
            MoveCommandParam CommandParam1, CommandParam2;
            CommandParam1 = new MoveCommandParam(enAxisName.XYZ轴, GlobalVariable.pConfig.ScanSpeed);
            CommandParam2 = new MoveCommandParam(this.motionType, GlobalVariable.pConfig.ScanSpeed);
            double[] refCalibrateDataValue;
            try
            {
                this.pointData1.ClearData();
                this.pointData1.ClearHandle();
                this.resultDataTable.Rows.Clear();

                foreach (var item in this.AcqCoordRect2)
                {
                    CommandParam1.AxisParam = new CoordSysAxisParam(item.X, item.Y, item.Z, 0, 0, 0);
                    /////////////////////////////////////////////////////////////////
                    CommandParam2.Tacc = this.tacc;
                    CommandParam2.Tdec = this.tdec;
                    CommandParam2.Rect2Param.vector_x = this.vector_x;
                    CommandParam2.Rect2Param.vector_y = this.vector_y;
                    CommandParam2.Rect2Param.InterpolateMode = this.interpolateMode;
                    CommandParam2.Rect2Param.lineCount = this.lineCount;
                    CommandParam2.AxisParam = new CoordSysAxisParam(item.X, item.Y, item.Z, 0, 0, 0);
                    /////////////////////////////////
                    list = AcqSourceManage.Instance.GetAcqSource(this._LaserSensorName).AcqScanData(CommandParam1, CommandParam2);
                    /// 这一步是为了让激光点的坐标与相机坐标相一致
                    ///////////////////////////////////////////////////// 拼接时要注意，绕Z轴旋转后，激光线长度会变短，这时偏移的距离要减去变化的距离
                    refCalibrateDataValue = SensorManage.GetSensor(this._LaserSensorName).GetParam(enSensorParamType.Coom_激光校准参数) as double[];
                    pointData1.TransformDataAndCalibrateData(list,
                        SensorManage.GetSensor(this._LaserSensorName).ConfigParam.SensorType,
                        SensorManage.GetSensor(this._LaserSensorName).LaserParam.DataWidth,
                        SensorManage.GetSensor(this._LaserSensorName).LaserParam.ScanAxis, 
                        CommandParam1.AxisParam, 
                        CommandParam2.AxisParam,
                        refCalibrateDataValue);
                    //////////////////////////////////////////    
                    if (this.pointData1.Dist1DataHandle.IsInitialized())
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
                }
                //////////////////////////////////////////////////////

                    if (this.pointData1.Dist1DataHandle.IsInitialized())
                        this.resultDataTable.Rows.Add(this.name , "距离1", this.pointData1.Dist1DataHandle.GetObjectModel3dParams("num_points").I, this.pointData1.Dist1Value.Count, 0, 0, "OK");
                    else
                        this.resultDataTable.Rows.Add(this.name , "距离1", 0, this.pointData1.Dist1Value.Count, 0, 0, "OK");
                //////////////////////////////////////////////////////////

                    if (this.pointData1.Dist2DataHandle.IsInitialized())
                        this.resultDataTable.Rows.Add(this.name , "距离2", this.pointData1.Dist2DataHandle.GetObjectModel3dParams("num_points").I, this.pointData1.Dist2Value.Count, 0, 0, "OK");
                    else
                        this.resultDataTable.Rows.Add(this.name , "距离2", 0, this.pointData1.Dist2Value.Count, 0, 0, "OK");

                //////////////////////////////////////////////////////////

                    if (this.pointData1.ThickDataHandle.IsInitialized())
                        this.resultDataTable.Rows.Add(this.name , "厚度", this.pointData1.ThickDataHandle.GetObjectModel3dParams("num_points").I, this.pointData1.ThickValue.Count, 0, 0, "OK");
                    else
                        this.resultDataTable.Rows.Add(this.name , "厚度", 0, this.pointData1.ThickValue.Count, 0, 0, "OK");
                //////////////////////////////////////////////////////
                Result.Succss = true;
                if (Result.Succss)
                    LoggerHelper.Info(this.name + "->激光数据采集完成");
                else
                    LoggerHelper.Error(this.name + "->激光数据采集失败");
            }
          catch (Exception ex)
            {
                LoggerHelper.Error(this.name + "->激光数据采集报错" + ex);
                return Result;
            }
            finally
            {
                /////////////////////////////////
                this.pointData1.ClearData();
            }
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
          //  throw new NotImplementedException();
        }
        public void Save(string path)
        {
          //  throw new NotImplementedException();
        }

        #endregion


    }
}

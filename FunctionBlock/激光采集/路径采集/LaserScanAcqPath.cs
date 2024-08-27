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
    public class LaserScanAcqPath : BaseFunction, IFunction
    {
        private TransformLaserPointCloudDataHandle pointData1 = new TransformLaserPointCloudDataHandle();
        private AcqSource _laserAcqSource;
        private enAxisName motionType = enAxisName.XY轴直线插补;
        private double scanHeight;

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
        public double ScanHeight { get => scanHeight; set => scanHeight = value; }

        public LaserScanAcqPath(ISensor _laserSensor)
        {
            if (_laserSensor != null)
                this._laserAcqSource = AcqSourceManage.Instance.GetAcqSource(_laserSensor.ConfigParam.SensorName);//  new AcqSource(_laserSensor);
        }
        public userWcsLine[] extractRefSource1Data()
        {
            List<userWcsLine> lineList = new List<userWcsLine>();
            object object3D = null;
            // 获取所有3D对象模型
            foreach (var item in this.RefSource1.Keys)
            {
                if (item.Split('.').Length == 1)
                    object3D = this.RefSource1[item].GetPropertyValues(item);
                else
                    object3D = this.RefSource1[item].GetPropertyValues(item.Split('.')[1]);
                if (object3D != null) // 这样做是为了动态获取名称
                {
                    switch (object3D.GetType().Name)
                    {
                        case "userWcsLine":
                            lineList.Add((userWcsLine)object3D);
                            break;
                        case "userWcsLine[]":
                            lineList.AddRange(((userWcsLine[])object3D));
                            break;
                    }
                }
            }
            return lineList.ToArray();
        }

        /// <summary>
        /// 获取当前机台位置
        /// </summary>
        /// <param name="card"></param>
        public void GetCurrentPosition(DataTable dataTable) // 保存后有意义的参数和保存后没意义的参数
        {
            List<double> acqCoordinate = new List<double>();
            double x_Tcoordinate = 0;
            double y_Tcoordinate = 0;
            double z_Tcoordinate = 500;
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
            catch (Exception ex)
            {
                //throw new Exception();
            }
            // return new double[3] { x_Tcoordinate, y_Tcoordinate, z_Tcoordinate };
        }


        #region 实现接口

        public OperateResult Execute(params object[] param)
        {
            this.Result.Succss = false;
            Dictionary<enDataItem, object> list;
            userWcsPose laserAffinePose;
            MoveCommandParam CommandParam1, CommandParam2;
            userWcsLine[] lines = extractRefSource1Data();
            try
            {
                if (this.LaserAcqSource == null)
                {
                    throw new ArgumentNullException("LaserAcqSource");
                }
                this.pointData1.ClearData();
                this.pointData1.ClearHandle();
                this.resultDataTable.Rows.Clear();
                for (int i = 0; i < lines.Length; i++)
                {
                     laserAffinePose = this.LaserAcqSource.Sensor.LaserParam.LaserCalibrationParam.LaserAffinePose;
                    //////////////////////////
                    CommandParam1 = new MoveCommandParam(enAxisName.XYZ轴, GlobalVariable.pConfig.MoveSpeed);
                    /////////////////////////////////////////////////////////////////
                    CommandParam2 = new MoveCommandParam(this.motionType, GlobalVariable.pConfig.ScanSpeed);                                                                                                                                                    /////////////////////////////////
                    ////////////////////////////////
                    list = _laserAcqSource.AcqScanData(CommandParam1, CommandParam2);
                    /// 这一步是为了让激光点的坐标与相机坐标相一致
                    ///////////////////////////////////////////////////// 拼接时要注意，绕Z轴旋转后，激光线长度会变短，这时偏移的距离要减去变化的距离
                    pointData1.TransformDataAndCalibrateData(list, _laserAcqSource.Sensor.ConfigParam.SensorType, _laserAcqSource.Sensor.LaserParam.DataWidth, _laserAcqSource.Sensor.LaserParam.ScanAxis, CommandParam1.AxisParam, CommandParam2.AxisParam, this.LaserAcqSource.Sensor.LaserParam.LaserCalibrationParam);
                    //////////////////////////////////////////    
                }
                Result.Succss = true;
                if (Result.Succss)
                    LoggerHelper.Info(this.name + "->数据采集完成");
                else
                    LoggerHelper.Error(this.name + "->数据采集失败");
            }
            catch (Exception ex)
            {
                LoggerHelper.Error(this.name + "->数据采集报错" + ex);
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
                    case "采集源":
                        this._laserAcqSource = (AcqSource)value[0];
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
            //throw new NotImplementedException();
        }
        public void Save(string path)
        {
            //throw new NotImplementedException();
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

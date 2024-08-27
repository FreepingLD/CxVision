using Common;
using Light;
using MotionControlCard;
using Sensor;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;

namespace FunctionBlock
{

    /// <summary>
    /// 采集源作为一个控制器来用，用于聚合传感器、光源、运动控制
    /// </summary>
    [Serializable]
    [DefaultProperty(nameof(Name))]
    public class AcqSource
    {
        private object lockState = new object();
        public BindingList<LightParam> LightParamList { get; set; }
        public List<string> SensorName { get; set; }
        public enCoordSysName CoordSysName { get; set; }
        public enAxisName MoveAxisName { get; set; }
        public double Exposure { get; set; }

        public string Name
        {
            get;
            set;
        }

        [System.Xml.Serialization.XmlIgnore]
        public ISensor Sensor
        {
            get
            {
                if (SensorName.Count > 0)
                    return SensorManage.GetSensor(SensorName[0]); // 一个采集源只对应一个传感器、一个控制卡
                else
                    return null;
            }
        }

        [System.Xml.Serialization.XmlIgnore]
        public IMotionControl Card
        {
            get
            {
                return MotionCardManage.GetCard(this.CoordSysName);
            }
        }



        public AcqSource() //
        {
            this.Name = "采集源";
            this.Exposure = -1;
            this.SensorName = new List<string>();// 
            this.CoordSysName = enCoordSysName.CoordSys_0;
            this.MoveAxisName = enAxisName.XY轴;
            this.LightParamList = new BindingList<LightParam>();
        }

        public AcqSource(string acqSourceName) // 
        {
            this.Name = acqSourceName;
            this.Exposure = -1;
            this.SensorName = new List<string>();// 
            this.CoordSysName = enCoordSysName.CoordSys_0;
            this.MoveAxisName = enAxisName.XY轴;
            this.LightParamList = new BindingList<LightParam>();
        }

        public AcqSource(ISensor sensor) // 初始化采集源时专用
        {
            this.Name = "采集源";
            this.Exposure = -1;
            this.SensorName = new List<string>(); //  sensor?.Name; // 一定要给他赋一个初值 
            this.CoordSysName = enCoordSysName.CoordSys_0;
            this.MoveAxisName = enAxisName.XY轴;
            this.LightParamList = new BindingList<LightParam>();
            //////////////////////////////////////          
            switch (sensor?.ConfigParam.SensorType)
            {
                case enUserSensorType.点激光:
                case enUserSensorType.线激光:
                case enUserSensorType.面激光:
                    //this.SensorParam = sensor.LaserParam;
                    break;
                case enUserSensorType.线阵相机:
                case enUserSensorType.面阵相机:
                    //this.SensorParam = sensor.CameraParam;
                    break;
            }

        }
        public AcqSource(string sensorName, enCoordSysName coordSysName) // 初始化采集源时专用
        {
            this.Name = "采集源";
            this.Exposure = -1;
            this.SensorName = new List<string>();// sensorName; // 一定要给他赋一个初值 
            this.CoordSysName = coordSysName;
            ///////////////////////////////////////
        }

        public Dictionary<enDataItem, object> AcqImageData(MoveCommandParam moveParam, BindingList<LightParam> lightParam)
        {
            bool lockTaken = false;
            Dictionary<enDataItem, object> list = new Dictionary<enDataItem, object>();
            list.Add(enDataItem.Image, null);
            try
            {
                Monitor.TryEnter(this.lockState, 1000, ref lockTaken);
                if (!lockTaken) return list;
                if (this.Sensor == null) return list;
                /////////////////////////////////////
                this.Card?.MoveMultyAxis(moveParam); //moveParam.moveAxis, moveParam.moveSpeed, moveParam.targetPosition
                LoggerHelper.Info("运动到扫描起点:" + moveParam.ToString());
                // 设置光源参数
                if (this.LightParamList != null && this.LightParamList.Count > 0)
                {
                    foreach (var item in this.LightParamList)
                    {
                        item.Open();
                        item.SetLight();
                    }
                }
                else
                {
                    foreach (var item in lightParam)
                    {
                        item.Open();
                        item.SetLight();
                    }
                }
                // 设置光源曝光参数
                if (this.Exposure > 0)
                {
                    this.Sensor.SetParam("曝光", this.Exposure);
                }
                Thread.Sleep(this.Sensor.CameraParam.WaiteTime);
                LoggerHelper.Info("开始触发");
                this.SetIoOutput(true);
                this.StartTrigger();
                //Thread.Sleep(this.SensorParam.AcqWaiteTime);
                this.SetIoOutput(false);
                this.StopTrigger();
                LoggerHelper.Info("停止触发");
                list = this.Sensor.ReadData();
            }
            catch (Exception ex)
            {
                LoggerHelper.Error("数据采集失败", ex);
            }
            finally
            {
                if (lockTaken)
                    Monitor.Exit(this.lockState);
            }
            return list;
        }
        public Dictionary<enDataItem, object> AcqImageData(BindingList<LightParam> lightParam, bool isStartAcq = false, bool isStopAcq = false)
        {
            bool lockTaken = false;
            Dictionary<enDataItem, object> list = new Dictionary<enDataItem, object>();
            list.Add(enDataItem.Image, null);
            try
            {
                Monitor.TryEnter(this.lockState, 1000, ref lockTaken);
                if (!lockTaken) return list;
                if (this.Sensor == null) return list;
                /////////////////////////////////////
                if (this.LightParamList != null && this.LightParamList.Count > 0)
                {
                    foreach (var item in this.LightParamList)
                    {
                        item.Open();
                        item.SetLight();
                    }
                }
                else
                {
                    if (lightParam != null)
                    {
                        foreach (var item in lightParam)
                        {
                            item.Open();
                            item.SetLight();
                        }
                    }
                }
                // 设置光源曝光参数
                if (this.Exposure > 0)
                {
                    this.Sensor.SetParam("曝光", this.Exposure);
                }
                Thread.Sleep(this.Sensor.CameraParam.WaiteTime);
                LoggerHelper.Info("开始触发采图");
                //foreach (var item in this.SensorName)
                //{
                //    for (int i = 0; i < 3; i++)
                //    {
                //        if (isStartAcq)
                //            SensorManage.GetSensor(item).SetParam("启动采集", ""); // 只有在第一次采图时才需要清空数据
                //        if (SensorManage.GetSensor(item).StartTrigger()) break;  // 如果采图失败，则循环采集三次
                //        if (isStopAcq)
                //            SensorManage.GetSensor(item).SetParam("停止采集", ""); // 只有在第一次采图时才需要清空数据
                //    }
                //}
                this.StartTrigger();
                this.StopTrigger();
                LoggerHelper.Info("停止触发采图");
                list = this.Sensor.ReadData();
            }
            catch (Exception ex)
            {
                LoggerHelper.Error("数据采集失败", ex);
            }
            finally
            {
                if (lockTaken)
                    Monitor.Exit(this.lockState);
            }
            return list;
        }

        public Dictionary<enDataItem, object>[] AcqImageDatas(MoveCommandParam moveParam, BindingList<LightParam> lightParam)
        {
            bool lockTaken = false;
            Dictionary<enDataItem, object>[] dicList = new Dictionary<enDataItem, object>[this.SensorName.Count];
            for (int i = 0; i < this.SensorName.Count; i++)
            {
                dicList[i].Add(enDataItem.Image, null);
            }
            try
            {
                Monitor.TryEnter(this.lockState, 1000, ref lockTaken);
                if (!lockTaken) return dicList;
                if (this.Sensor == null) return dicList;
                /////////////////////////////////////
                this.Card?.MoveMultyAxis(moveParam); //moveParam.moveAxis, moveParam.moveSpeed, moveParam.targetPosition
                LoggerHelper.Info("运动到扫描起点:" + moveParam.ToString());
                if (this.LightParamList != null && this.LightParamList.Count > 0)
                {
                    foreach (var item in this.LightParamList)
                    {
                        item.Open();
                        item.SetLight();
                    }
                }
                else
                {
                    foreach (var item in lightParam)
                    {
                        item.Open();
                        item.SetLight();
                    }
                }
                // 设置光源曝光参数
                if (this.Exposure > 0)
                {
                    this.Sensor.SetParam("曝光", this.Exposure);
                }
                Thread.Sleep(this.Sensor.CameraParam.WaiteTime);
                LoggerHelper.Info("开始采集");
                //foreach (var item in this.SensorName)
                //{
                //    for (int i = 0; i < 3; i++)
                //    {
                //        if (SensorManage.GetSensor(item).StartTrigger()) break;
                //    }
                //}
                this.StartTrigger();
                this.SetIoOutput(true);
                //Thread.Sleep(this.SensorParam.AcqWaiteTime);
                this.SetIoOutput(false);
                this.StopTrigger();
                //foreach (var item in this.SensorName)
                //{
                //    SensorManage.GetSensor(item)?.StopTrigger();
                //}
                LoggerHelper.Info("停止采集");
                for (int i = 0; i < this.SensorName.Count; i++)
                {
                    dicList[i] = SensorManage.GetSensor(this.SensorName[i])?.ReadData();
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.Error("数据采集失败", ex);
            }
            finally
            {
                if (lockTaken)
                    Monitor.Exit(this.lockState);
            }
            return dicList;
        }

        public Dictionary<enDataItem, object> AcqPointData(MoveCommandParam moveParam)
        {
            bool lockTaken = false;
            Dictionary<enDataItem, object> list = new Dictionary<enDataItem, object>();
            try
            {
                Monitor.TryEnter(this.lockState, 1000, ref lockTaken);
                if (!lockTaken) return list;
                if (this.Sensor == null) return list;
                /////////////////////////////////////
                if (this.Card != null)
                    this.Card.MoveMultyAxis(moveParam);        //moveParam.moveAxis, moveParam.moveSpeed, moveParam.targetPosition
                LoggerHelper.Info("运动到扫描起点:" + moveParam.ToString());
                Thread.Sleep(this.Sensor.CameraParam.WaiteTime); // 在图像采集时一定要设置这个停顿时间
                LoggerHelper.Info("开始触发");
                this.StartTrigger();
                this.SetIoOutput(true);
                Thread.Sleep(this.Sensor.CameraParam.AcqWaiteTime);
                this.SetIoOutput(false);
                this.StopTrigger();
                LoggerHelper.Info("停止触发");
                list = this.Sensor.ReadData();
            }
            catch (Exception ex)
            {
                LoggerHelper.Error("数据采集失败", ex);
            }
            finally
            {
                if (lockTaken)
                    Monitor.Exit(this.lockState);
            }
            return list;
        }

        public Dictionary<enDataItem, object>[] AcqPointDatas(BindingList<CoordSysAxisParam> AcqCoordPointList, userWcsCoordSystem wcsCoordSystem)
        {
            bool lockTaken = false;
            Dictionary<enDataItem, object>[] dicList = new Dictionary<enDataItem, object>[this.SensorName.Count];
            try
            {
                Monitor.TryEnter(this.lockState, 1000, ref lockTaken);
                if (!lockTaken) return dicList;
                if (this.SensorName == null || this.SensorName.Count == 0) return dicList;
                /////////////////////////////////////
                MoveCommandParam CommandParam, AffineCommandParam;
                foreach (var items in AcqCoordPointList)
                {
                    CommandParam = new MoveCommandParam(this.MoveAxisName, GlobalVariable.pConfig.MoveSpeed);
                    CommandParam.AxisParam = new CoordSysAxisParam(items.X, items.Y, items.Z, items.Theta, items.U, items.V);
                    CommandParam.CoordSysName = this.CoordSysName;      // 运动坐标系里是否需要包含移动指令
                    if (wcsCoordSystem == null)
                        wcsCoordSystem = new userWcsCoordSystem();
                    AffineCommandParam = CommandParam.Affine2DCommandParam(wcsCoordSystem);
                    if (this.Card != null)
                        this.Card?.MoveMultyAxis(AffineCommandParam);
                    LoggerHelper.Info("运动到扫描起点:" + AffineCommandParam.ToString());
                    Thread.Sleep(this.Sensor.CameraParam.WaiteTime); // 在图像采集时一定要设置这个停顿时间
                    LoggerHelper.Info("开始采集");
                    //foreach (var item in this.SensorName)
                    //{
                    //    SensorManage.GetSensor(item)?.StartTrigger();
                    //}
                    this.StartTrigger();
                    this.SetIoOutput(true);
                    Thread.Sleep(this.Sensor.CameraParam.AcqWaiteTime);
                    this.SetIoOutput(false);
                    this.StopTrigger();
                    //foreach (var item in this.SensorName)
                    //{
                    //    SensorManage.GetSensor(item)?.StopTrigger();
                    //}
                    LoggerHelper.Info("停止采集");
                }
                ///////////////////////////////////////////////
                for (int i = 0; i < this.SensorName.Count; i++)
                {
                    dicList[i] = SensorManage.GetSensor(this.SensorName[i])?.ReadData();
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.Error("数据采集失败", ex);
            }
            finally
            {
                if (lockTaken)
                    Monitor.Exit(this.lockState);
            }
            return dicList;
        }

        public Dictionary<enDataItem, object>[] AcqPointDatas(MoveCommandParam moveParam)
        {
            bool lockTaken = false;
            Dictionary<enDataItem, object>[] dicList = new Dictionary<enDataItem, object>[this.SensorName.Count];
            try
            {
                Monitor.TryEnter(this.lockState, 1000, ref lockTaken);
                if (!lockTaken) return dicList;
                if (this.Sensor == null) return dicList;
                /////////////////////////////////////
                if (this.Card != null)
                    this.Card.MoveMultyAxis(moveParam);        //moveParam.moveAxis, moveParam.moveSpeed, moveParam.targetPosition
                LoggerHelper.Info("运动到扫描起点:" + moveParam.ToString());
                Thread.Sleep(this.Sensor.CameraParam.WaiteTime); // 在图像采集时一定要设置这个停顿时间
                LoggerHelper.Info("开始采集");
                this.StartTrigger();
                this.SetIoOutput(true);
                Thread.Sleep(this.Sensor.CameraParam.AcqWaiteTime);
                this.SetIoOutput(false);
                this.StopTrigger();
                LoggerHelper.Info("停止采集");
                ///////////////////////////////////////////////
                for (int i = 0; i < this.SensorName.Count; i++)
                {
                    dicList[i] = SensorManage.GetSensor(this.SensorName[i])?.ReadData();
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.Error("数据采集失败", ex);
            }
            finally
            {
                if (lockTaken)
                    Monitor.Exit(this.lockState);
            }
            return dicList;
        }
        public Dictionary<enDataItem, object> AcqPointDatas(MoveCommandParam moveParam, int index = 0)
        {
            bool lockTaken = false;
            Dictionary<enDataItem, object> dicList = new Dictionary<enDataItem, object>();
            try
            {
                Monitor.TryEnter(this.lockState, 1000, ref lockTaken);
                if (!lockTaken) return dicList;
                if (this.Sensor == null) return dicList;
                /////////////////////////////////////
                if (this.Card != null)
                    this.Card.MoveMultyAxis(moveParam);        //moveParam.moveAxis, moveParam.moveSpeed, moveParam.targetPosition
                LoggerHelper.Info("运动到扫描起点:" + moveParam.ToString());
                Thread.Sleep(this.Sensor.CameraParam.WaiteTime); // 在图像采集时一定要设置这个停顿时间
                LoggerHelper.Info("开始采集");
                this.StartTrigger();
                this.SetIoOutput(true);
                Thread.Sleep(this.Sensor.CameraParam.AcqWaiteTime);
                this.SetIoOutput(false);
                this.StopTrigger();
                LoggerHelper.Info("停止采集");
                ///////////////////////////////////////////////
                dicList = SensorManage.GetSensor(this.SensorName[0])?.ReadData();
            }
            catch (Exception ex)
            {
                LoggerHelper.Error("数据采集失败", ex);
            }
            finally
            {
                if (lockTaken)
                    Monitor.Exit(this.lockState);
            }
            return dicList;
        }
        public Dictionary<enDataItem, object> AcqPointData(string laserSensorName, MoveCommandParam moveParam)
        {
            bool lockTaken = false;
            Dictionary<enDataItem, object> list = new Dictionary<enDataItem, object>();
            try
            {
                Monitor.TryEnter(this.lockState, 1000, ref lockTaken);
                if (!lockTaken) return list;
                if (this.Sensor == null) return list;
                /////////////////////////////////////
                if (this.Card != null)
                    this.Card.MoveMultyAxis(moveParam);        //moveParam.moveAxis, moveParam.moveSpeed, moveParam.targetPosition
                LoggerHelper.Info("运动到扫描起点:" + moveParam.ToString());
                Thread.Sleep(this.Sensor.CameraParam.WaiteTime); // 在图像采集时一定要设置这个停顿时间
                LoggerHelper.Info("开始触发");
                this.StartTrigger();
                this.SetIoOutput(true);
                Thread.Sleep(this.Sensor.CameraParam.AcqWaiteTime);
                this.SetIoOutput(false);
                this.StopTrigger();
                LoggerHelper.Info("停止触发");
                list = this.Sensor.ReadData();
            }
            catch (Exception ex)
            {
                LoggerHelper.Error("数据采集失败", ex);
            }
            finally
            {
                if (lockTaken)
                    Monitor.Exit(this.lockState);
            }
            return list;
        }

        public Dictionary<enDataItem, object> AcqPointData()
        {
            bool lockTaken = false;
            Dictionary<enDataItem, object> dicList = new Dictionary<enDataItem, object>();
            try
            {
                Monitor.TryEnter(this.lockState, ref lockTaken);
                if (!lockTaken) return dicList;
                if (this.Sensor == null) return dicList;
                if (this.Sensor.CameraParam == null) return dicList;
                /////////////////////////////////////
                Thread.Sleep(this.Sensor.CameraParam.WaiteTime);
                LoggerHelper.Info(this.Sensor?.Name + "开始采集");
                this.StartTrigger();
                this.SetIoOutput(true);
                Thread.Sleep(this.Sensor.CameraParam.AcqWaiteTime); // 有很多参数是共用的
                this.SetIoOutput(false);
                this.StopTrigger();
                LoggerHelper.Info(this.Sensor?.Name + "停止采集");
                /////////////////////////////
                return this.Sensor?.ReadData();
            }
            catch (Exception ex)
            {
                LoggerHelper.Error("数据采集失败", ex);
            }
            finally
            {
                if (lockTaken)
                    Monitor.Exit(this.lockState);
            }
            return dicList;
        }
        public Dictionary<enDataItem, object>[] AcqPointDatas()
        {
            bool lockTaken = false;
            Dictionary<enDataItem, object>[] dicList = new Dictionary<enDataItem, object>[this.SensorName.Count];
            try
            {
                Monitor.TryEnter(this.lockState, ref lockTaken);
                if (!lockTaken) return dicList;
                if (this.Sensor == null) return dicList;
                if (this.Sensor.LaserParam == null) return dicList;
                /////////////////////////////////////
                Thread.Sleep(this.Sensor.LaserParam.WaiteTime);
                //foreach (var item in this.SensorName)
                //{
                //    LoggerHelper.Info(item + ":开始采集");
                //    SensorManage.GetSensor(item)?.StartTrigger();
                //}
                this.StartTrigger();
                this.SetIoOutput(true);
                Thread.Sleep(this.Sensor.CameraParam.AcqWaiteTime); // 有很多参数是共用的
                this.SetIoOutput(false);
                this.StopTrigger();
                //foreach (var item in this.SensorName)
                //{
                //    SensorManage.GetSensor(item)?.StopTrigger();
                //    LoggerHelper.Info(item + ":停止采集");
                //}
                /////////////////////////////
                for (int i = 0; i < this.SensorName.Count; i++)
                {
                    dicList[i] = SensorManage.GetSensor(this.SensorName[i])?.ReadData();
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.Error("数据采集失败", ex);
            }
            finally
            {
                if (lockTaken)
                    Monitor.Exit(this.lockState);
            }
            return dicList;
        }
        public Dictionary<enDataItem, object> AcqScanData(MoveCommandParam moveParam1, MoveCommandParam moveParam2)
        {
            bool lockTaken = false;
            Dictionary<enDataItem, object> list = new Dictionary<enDataItem, object>();
            try
            {
                Monitor.TryEnter(this.lockState, 1000, ref lockTaken);
                if (!lockTaken) return list; // 如果没有获取到锁，则返回
                if (this.Sensor == null) return list;
                ///////////////////////////////////
                if (this.Card != null)
                    this.Card.MoveMultyAxis(moveParam1); // moveParam1.moveAxis, moveParam1.moveSpeed, moveParam1.targetPosition
                LoggerHelper.Info("运动到扫描起点:" + moveParam1.ToString());
                this.StartTrigger();
                this.SetIoOutput(true);
                LoggerHelper.Info("开始触发");
                if (this.Card != null)
                    this.Card.MoveMultyAxis(moveParam2); //moveParam2.moveAxis, moveParam2.moveSpeed, moveParam2.targetPosition
                LoggerHelper.Info("运动到扫描终点:" + moveParam2.ToString());
                this.SetIoOutput(false);
                this.StopTrigger();
                LoggerHelper.Info("停止触发");
                list = this.Sensor.ReadData();
            }
            catch (Exception ex)
            {
                LoggerHelper.Error("数据采集失败", ex);
            }
            finally
            {
                if (lockTaken)
                    Monitor.Exit(this.lockState);
            }
            return list;
        }

        //
        public Dictionary<enDataItem, object>[] AcqScanDatas(MoveCommandParam moveParam1, MoveCommandParam moveParam2)
        {
            bool lockTaken = false;
            Dictionary<enDataItem, object>[] dicList = new Dictionary<enDataItem, object>[this.SensorName.Count];
            try
            {
                Monitor.TryEnter(this.lockState, 1000, ref lockTaken);
                if (!lockTaken) return dicList; // 如果没有获取到锁，则返回
                if (this.Sensor == null) return dicList;
                ///////////////////////////////////
                if (this.Card != null)
                    this.Card.MoveMultyAxis(moveParam1);
                Thread.Sleep(this.Sensor.CameraParam.WaiteTime);
                LoggerHelper.Info("运动到扫描起点:" + moveParam1.ToString());
                //foreach (var item in this.SensorName)
                //{
                //    LoggerHelper.Info(item + ":开始采集");
                //    SensorManage.GetSensor(item)?.StartTrigger();
                //}
                this.StartTrigger();
                this.SetIoOutput(true);
                LoggerHelper.Info("开始触发");
                if (this.Card != null)
                    this.Card.MoveMultyAxis(moveParam2);
                LoggerHelper.Info("运动到扫描终点:" + moveParam2.ToString());
                this.SetIoOutput(false);
                this.StopTrigger();
                //foreach (var item in this.SensorName)
                //{
                //    LoggerHelper.Info(item + ":停止采集");
                //    SensorManage.GetSensor(item)?.StopTrigger();
                //}
                ////////////////////////////////////////////
                for (int i = 0; i < this.SensorName.Count; i++)
                {
                    dicList[i] = SensorManage.GetSensor(this.SensorName[i])?.ReadData();
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.Error("数据采集失败", ex);
            }
            finally
            {
                if (lockTaken)
                    Monitor.Exit(this.lockState);
            }
            return dicList;
        }

        public Dictionary<enDataItem, object>[] AcqScanDatas(BindingList<userWcsLine> _AcqCoordLine, userWcsCoordSystem wcsCoordSystem)
        {
            bool lockTaken = false;
            Dictionary<enDataItem, object>[] dicList = new Dictionary<enDataItem, object>[this.SensorName.Count];
            try
            {
                Monitor.TryEnter(this.lockState, 1000, ref lockTaken);
                if (!lockTaken) return dicList; // 如果没有获取到锁，则返回
                if (this.SensorName == null || this.SensorName.Count == 0) return dicList;
                ///////////////////////////////////
                MoveCommandParam CommandParam1, CommandParam2, AffineCommandParam1, AffineCommandParam2;
                foreach (var items in _AcqCoordLine)
                {
                    CommandParam1 = new MoveCommandParam(this.MoveAxisName, GlobalVariable.pConfig.MoveSpeed);
                    CommandParam1.AxisParam = new CoordSysAxisParam(items.X1, items.Y1, items.Z1, items.Theta1, items.U1, items.V1);
                    CommandParam1.CoordSysName = this.CoordSysName;      // 运动坐标系里是否需要包含移动指令
                    AffineCommandParam1 = CommandParam1.Affine2DCommandParam(wcsCoordSystem);
                    CommandParam2 = new MoveCommandParam(this.MoveAxisName, GlobalVariable.pConfig.MoveSpeed);
                    CommandParam2.AxisParam = new CoordSysAxisParam(items.X2, items.Y2, items.Z2, items.Theta2, items.U2, items.V2);
                    CommandParam2.CoordSysName = this.CoordSysName;      // 运动坐标系里是否需要包含移动指令
                    AffineCommandParam2 = CommandParam2.Affine2DCommandParam(wcsCoordSystem);
                    /////////////////////////
                    if (this.Card != null)
                        this.Card.MoveMultyAxis(AffineCommandParam1);
                    Thread.Sleep(this.Sensor.CameraParam.WaiteTime);
                    LoggerHelper.Info("运动到扫描起点:" + AffineCommandParam1.ToString());
                    //foreach (var item in this.SensorName)
                    //{
                    //    LoggerHelper.Info(item + ":开始采集");
                    //    SensorManage.GetSensor(item)?.StartTrigger();
                    //}
                    this.StartTrigger();
                    this.SetIoOutput(true);
                    LoggerHelper.Info("开始触发");
                    if (this.Card != null)
                        this.Card.MoveMultyAxis(AffineCommandParam2);
                    LoggerHelper.Info("运动到扫描终点:" + AffineCommandParam2.ToString());
                    this.SetIoOutput(false);
                    //foreach (var item in this.SensorName)
                    //{
                    //    LoggerHelper.Info(item + ":停止采集");
                    //    SensorManage.GetSensor(item)?.StopTrigger();
                    //}
                    this.StopTrigger();
                }
                ////////////////////////////////////////////
                for (int i = 0; i < this.SensorName.Count; i++)
                {
                    dicList[i] = SensorManage.GetSensor(this.SensorName[i])?.ReadData();
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.Error("数据采集失败", ex);
            }
            finally
            {
                if (lockTaken)
                    Monitor.Exit(this.lockState);
            }
            return dicList;
        }
        public void GetAxisPosition(enAxisName axisName, out double position)
        {
            position = 0;
            if (this.Card != null)
                this.Card?.GetAxisPosition(this.CoordSysName, axisName, out position);
        }
        public double GetAxisPosition(enAxisName axisName)
        {
            double position = 0;
            if (this.Card != null)
                this.Card?.GetAxisPosition(this.CoordSysName, axisName, out position);
            return position;
        }

        /// <summary>
        /// 轴位置顺序 X/Y/Z/Theta/U/V
        /// </summary>
        /// <returns></returns>
        public double[] GetAxisPosition() // 保存后有意义的参数和保存后没意义的参数
        {
            double[] coordArray = null;
            double x_Tcoordinate = GetAxisPosition(enAxisName.X轴);
            double y_Tcoordinate = GetAxisPosition(enAxisName.Y轴);
            double z_Tcoordinate = GetAxisPosition(enAxisName.Z轴);
            double w_Tcoordinate = GetAxisPosition(enAxisName.Theta轴);
            double u_Tcoordinate = GetAxisPosition(enAxisName.U轴);
            double v_Tcoordinate = GetAxisPosition(enAxisName.V轴);
            coordArray = new double[6] { x_Tcoordinate, y_Tcoordinate, z_Tcoordinate, w_Tcoordinate, u_Tcoordinate, v_Tcoordinate };
            return coordArray;
        }
        public bool StartTrigger()
        {
            bool result = true;
            foreach (var item in this.SensorName)
            {
                switch (this.Sensor.ConfigParam.SensorType)
                {
                    case enUserSensorType.点激光:
                    case enUserSensorType.线激光:
                    case enUserSensorType.面激光:
                        switch (this.Sensor.LaserParam.AcqMode)
                        {
                            case enAcqMode.同步采集:
                                for (int i = 0; i < 3; i++)
                                {
                                    result = SensorManage.GetSensor(item).StartTrigger();
                                    if (result) break;  // 如果采图失败，则循环采集三次
                                }
                                break;
                            default:
                                break;
                        }
                        break;
                    case enUserSensorType.面阵相机:
                    case enUserSensorType.线阵相机:
                        switch (this.Sensor.CameraParam.AcqMode)
                        {
                            case enAcqMode.同步采集:
                                for (int i = 0; i < 3; i++)
                                {
                                    result = SensorManage.GetSensor(item).StartTrigger();
                                    if (result) break;  // 如果采图失败，则循环采集三次
                                }
                                break;
                            default:
                                break;
                        }
                        break;
                }
            }
            return result;
        }
        public bool StopTrigger()
        {
            bool result = true;
            foreach (var item in this.SensorName)
            {
                switch (this.Sensor.ConfigParam.SensorType)
                {
                    case enUserSensorType.点激光:
                    case enUserSensorType.线激光:
                    case enUserSensorType.面激光:
                        switch (this.Sensor.LaserParam.AcqMode)
                        {
                            case enAcqMode.同步采集:
                                for (int i = 0; i < 3; i++)
                                {
                                    result = SensorManage.GetSensor(item).StopTrigger();
                                    if (result) break;
                                }
                                break;
                            default:
                                break;
                        }
                        break;
                    case enUserSensorType.面阵相机:
                    case enUserSensorType.线阵相机:
                        switch (this.Sensor.CameraParam.AcqMode)
                        {
                            case enAcqMode.同步采集:
                                for (int i = 0; i < 3; i++)
                                {
                                    result = SensorManage.GetSensor(item).StopTrigger();
                                    if (result) break;
                                }
                                break;
                            default:
                                break;
                        }
                        break;
                }
            }
            return result;
        }
        public Dictionary<enDataItem, object> getData()
        {
            lock (lockState)
            {
                return this.Sensor.ReadData();
            }
        }

        public Dictionary<enDataItem, object>[] getDatas()
        {
            Dictionary<enDataItem, object>[] dicList = new Dictionary<enDataItem, object>[this.SensorName.Count];
            lock (lockState)
            {
                for (int i = 0; i < this.SensorName.Count; i++)
                {
                    dicList[i] = SensorManage.GetSensor(this.SensorName[i])?.ReadData();
                }
            }
            return dicList;
        }


        public int NumPerLine()
        {
            if (this.Sensor == null) return -1;
            switch (this.Sensor.GetType().Name)
            {
                case "LiYiPointLaser":
                    return (int)this.Sensor.LaserParam.DataWidth;//  GetParam(enSensorParamType.Coom_每线点数);
                case "CStil_P":
                    return (int)this.Sensor.LaserParam.DataWidth;// GetParam(enSensorParamType.Coom_每线点数);
                case "CStil_L":
                    return (int)this.Sensor.LaserParam.DataWidth;// GetParam(enSensorParamType.Coom_每线点数);
                case "SSZNLineLaser":
                    return (int)this.Sensor.LaserParam.DataWidth;// GetParam(enSensorParamType.Coom_每线点数);
                case "BoMingPointLaser":
                    return (int)this.Sensor.LaserParam.DataWidth;// GetParam(enPropId.每线点数);
                case "BoMingStructuredLight":
                    return (int)this.Sensor.LaserParam.DataWidth;// GetParam(enSensorParamType.Coom_每线点数);
                case "SmartRayLineLaser":
                    return (int)this.Sensor.LaserParam.DataWidth;// GetParam(enSensorParamType.Coom_每线点数);
                case "DaHengCamera":
                    return (int)this.Sensor.CameraParam.DataWidth;// GetParam(enSensorParamType.Coom_每线点数);
                case "GbsFaceWliRemote":
                    return (int)this.Sensor.LaserParam.DataWidth;// GetParam(enSensorParamType.Coom_每线点数);
                case "LJV7000LineLaser":
                    return (int)this.Sensor.LaserParam.DataWidth;// GetParam(enSensorParamType.Coom_每线点数);
                default:
                    return 0;
            }
        }
        public enUserSensorType GetSensorType()
        {
            if (this.Sensor == null) return enUserSensorType.点激光;
            switch (this.Sensor.GetType().Name)
            {
                case "LiYiPointLaser":
                    return this.Sensor.ConfigParam.SensorType;// GetParam(enSensorParamType.Coom_传感器类型);
                case "CStil_P":
                    return this.Sensor.ConfigParam.SensorType;//GetParam(enSensorParamType.Coom_传感器类型);
                case "CStil_L":
                    return this.Sensor.ConfigParam.SensorType;//GetParam(enSensorParamType.Coom_传感器类型);
                case "SSZNLineLaser":
                    return this.Sensor.ConfigParam.SensorType;//GetParam(enSensorParamType.Coom_传感器类型);
                case "BoMingPointLaser":
                    return this.Sensor.ConfigParam.SensorType;//GetParam(enPropId.传感器类型);
                case "BoMingStructuredLight":
                    return this.Sensor.ConfigParam.SensorType;//GetParam(enSensorParamType.Coom_传感器类型);
                case "SmartRayLineLaser":
                    return this.Sensor.ConfigParam.SensorType;//GetParam(enSensorParamType.Coom_传感器类型);
                case "DaHengCamera":
                    return this.Sensor.ConfigParam.SensorType;//GetParam(enSensorParamType.Coom_传感器类型);
                case "GbsFaceWliRemote":
                    return this.Sensor.ConfigParam.SensorType;//GetParam(enSensorParamType.Coom_传感器类型);
                case "LJV7000LineLaser":
                    return this.Sensor.ConfigParam.SensorType;//GetParam(enSensorParamType.Coom_传感器类型);
                default:
                    return 0;
            }
        }

        public void SetIoOutput(bool ioOutput)
        {
            if (this.Card != null)
            {
                switch (this.Sensor.ConfigParam.SensorType)
                {
                    case enUserSensorType.点激光:
                    case enUserSensorType.线激光:
                    case enUserSensorType.面激光:
                        switch (this.Sensor.LaserParam.TriggerSource)
                        {

                            case enUserTriggerSource.内部IO触发:
                                IoParam param = new IoParam(true);
                                param.IoPort = this.Sensor.LaserParam.TriggerPort;
                                param.IoOutputMode = this.Sensor.LaserParam.IoOutputMode; // LaserParam: 里的输出模式没什么用了
                                param.IoValue = 1;
                                param.IoReverseTime = 50;
                                switch (this.Sensor.LaserParam.TriggerMode)
                                {
                                    case enUserTrigerMode.TRS:
                                        param.IoOutputMode = enIoOutputMode.脉冲输出;
                                        if (this.Sensor.LaserParam.DeviceMode == enDeviceMode.主设备) // 只有主设备才执行触发
                                            this.Card?.SetParam(MotionControlCard.enParamType.IO控制, param);
                                        break;
                                    case enUserTrigerMode.TRE:
                                        if (ioOutput)
                                        {
                                            param.IoOutputMode = enIoOutputMode.脉冲输出;
                                            if (this.Sensor.LaserParam.DeviceMode == enDeviceMode.主设备) // 只有主设备才执行触发
                                                this.Card?.SetParam(MotionControlCard.enParamType.IO控制, param);
                                        }
                                        break;
                                    case enUserTrigerMode.TRN:
                                        if (ioOutput)
                                        {
                                            param.IoOutputMode = enIoOutputMode.高电平输出;
                                            param.IoValue = 1; // 高电平
                                        }
                                        else
                                        {
                                            param.IoOutputMode = enIoOutputMode.低电平输出;
                                            param.IoValue = 0; // 低电平
                                        }
                                        if (this.Sensor.LaserParam.DeviceMode == enDeviceMode.主设备) // 只有主设备才执行触发
                                            this.Card?.SetParam(MotionControlCard.enParamType.IO控制, param);
                                        break;
                                }
                                // this.Card?.SetIoOutputBit(this.Sensor.CameraParam.IoOutputMode, this.Sensor.CameraParam.TriggerPort, ioOutput);
                                break;
                            default:
                                break;
                        }
                        break;
                    case enUserSensorType.面阵相机:
                    case enUserSensorType.线阵相机:
                        switch (this.Sensor.CameraParam.TriggerSource)
                        {
                            case enUserTriggerSource.内部IO触发:
                                //this.Card?.SetIoOutputBit(this.Sensor.CameraParam.IoOutputMode, this.Sensor.CameraParam.TriggerPort, ioOutput);
                                IoParam param = new IoParam(true);
                                param.IoPort = this.Sensor.CameraParam.TriggerPort;
                                param.IoOutputMode = this.Sensor.CameraParam.IoOutputMode;
                                param.IoValue = 1;
                                param.IoReverseTime = 50;
                                switch (this.Sensor.LaserParam.TriggerMode)
                                {
                                    case enUserTrigerMode.TRS:
                                        param.IoOutputMode = enIoOutputMode.脉冲输出;
                                        this.Card?.SetParam(MotionControlCard.enParamType.IO控制, param);
                                        break;
                                    case enUserTrigerMode.TRE:
                                        if (ioOutput)
                                        {
                                            param.IoOutputMode = enIoOutputMode.脉冲输出;
                                            this.Card?.SetParam(MotionControlCard.enParamType.IO控制, param);
                                        }
                                        break;
                                    case enUserTrigerMode.TRN:
                                        if (ioOutput)
                                        {
                                            param.IoOutputMode = enIoOutputMode.高电平输出;
                                            param.IoValue = 1; // 高电平
                                        }
                                        else
                                        {
                                            param.IoOutputMode = enIoOutputMode.低电平输出;
                                            param.IoValue = 0; // 低电平
                                        }
                                        this.Card?.SetParam(MotionControlCard.enParamType.IO控制, param);
                                        break;
                                }
                                break;
                            default:
                                break;
                        }
                        break;
                }
            }
        }
        public double GetFullScale()
        {
            return (double)this.Sensor.LaserParam.MeasureRange;  // GetParam(enSensorParamType.Stil_测量范围);
        }






    }
}

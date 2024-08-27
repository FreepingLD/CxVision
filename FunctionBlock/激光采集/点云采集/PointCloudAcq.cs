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
using System.Diagnostics;

namespace FunctionBlock
{
    [Serializable]
    [DefaultProperty(nameof(Dist1DataHandle))]

    /// <summary>
    /// 激光取点采集的数据类
    /// </summary>
    public class PointCloudAcq : BaseFunction, IFunction
    {
        [NonSerialized]
        private TransformLaserPointCloudDataHandle pointData1 = new TransformLaserPointCloudDataHandle();
        private CoordSysAxisParam axisParam;
        private userWcsCoordSystem _wcsCoordSystem;
        [NonSerialized]
        private ImageDataClass _imageData = null; // 数据句柄
        public int FileIndex { get; set; }
        public PointCloudAcqParam AcqParam { get; set; }
        private TrackParam[] _trackPoint;



        [DisplayName("采集点位")]
        [DescriptionAttribute("输入属性1")]
        public TrackParam[] TrackPoint
        {
            get
            {
                try
                {
                    if (this.RefSource1.Count > 0)
                    {
                        List<TrackParam> list = new List<TrackParam>();
                        object[] oo = this.GetPropertyValue(this.RefSource1);
                        if (oo != null && oo.Length > 0)
                        {
                            foreach (var item in oo)
                            {
                                switch (item.GetType().Name)
                                {
                                    case nameof(userWcsPoint):
                                        userWcsPoint wcsPoint = item as userWcsPoint;
                                        list.Add(new TrackParam(new drawWcsPoint(wcsPoint.X, wcsPoint.Y, wcsPoint.Z)));
                                        break;
                                    case nameof(userWcsLine):
                                        userWcsLine wcsLine = item as userWcsLine;
                                        list.Add(new TrackParam(new drawWcsLine(wcsLine.X1, wcsLine.Y1, wcsLine.Z1, wcsLine.X2, wcsLine.Y2, wcsLine.Z2)));
                                        break;
                                    case nameof(userWcsCircle):
                                        userWcsCircle wcsCircle = item as userWcsCircle;
                                        list.Add(new TrackParam(new drawWcsCircle(wcsCircle.X, wcsCircle.Y, wcsCircle.Z, wcsCircle.Radius)));
                                        break;
                                    case nameof(userWcsRectangle2):
                                        userWcsRectangle2 wcsRect2 = item as userWcsRectangle2;
                                        list.Add(new TrackParam(new drawWcsRect2(wcsRect2.X, wcsRect2.Y, wcsRect2.Z, wcsRect2.Deg, wcsRect2.Length1, wcsRect2.Length2)));
                                        break;
                                    case nameof(TrackParam):
                                        list.Add((TrackParam)item);
                                        break;
                                }
                            }
                        }
                        this._trackPoint = list.ToArray();
                        list.Clear();
                    }
                    else
                        this._trackPoint = null;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return _trackPoint;
            }
            set
            {
                _trackPoint = value;
            }
        }

        [DisplayName("坐标系")]
        [DescriptionAttribute("输入属性2")]
        public userWcsCoordSystem WcsCoordSystem
        {
            get
            {
                try
                {
                    if (this.RefSource2.Count > 0)
                    {
                        object[] oo = this.GetPropertyValue(this.RefSource2);
                        if (oo != null && oo.Length > 0)
                        {
                            this._wcsCoordSystem = oo.Last() as userWcsCoordSystem;
                        }
                        else
                            this._wcsCoordSystem = new userWcsCoordSystem();
                    }
                    else
                        this._wcsCoordSystem = new userWcsCoordSystem();
                    return _wcsCoordSystem;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            set
            {
                this._wcsCoordSystem = value;
            }
        }


        [DisplayName("距离1")]
        [DescriptionAttribute("输出属性")]
        public PointCloudData Dist1DataHandle
        {
            get;
            set;
        }

        [DisplayName("距离2")]
        [DescriptionAttribute("输出属性")]
        public PointCloudData Dist2DataHandle
        {
            get;
            set;
        }

        [DisplayName("厚度")]
        [DescriptionAttribute("输出属性")]
        public PointCloudData ThickDataHandle
        {
            get;
            set;
        }

        [DisplayName("高度图")]
        [DescriptionAttribute("输出属性")]
        public ImageDataClass ImageData
        {
            get => _imageData;
            set => _imageData = value;
        }


        public CoordSysAxisParam AxisParam { get => axisParam; set => axisParam = value; }



        public PointCloudAcq(string _acqSourceName)
        {
            this.name = "点云采集";
            this.AcqParam = new PointCloudAcqParam();
            this.AcqParam.AcqSourceName = _acqSourceName;
            if (MotionCardManage.CurrentCoordSys != enCoordSysName.NONE)
                this.AcqParam.CoordSysName = MotionCardManage.CurrentCoordSys;
            else
                this.AcqParam.CoordSysName = enCoordSysName.CoordSys_0;
            this.axisParam = new CoordSysAxisParam(this.AcqParam.CoordSysName);
            ///////////////////////////////////////////////////
            this.ResultInfo = new BindingList<MeasureResultInfo>();
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name, "Width", 0));
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name, "Height", 0));
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name, "Count", 0));
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name, "Grab_X", 0));
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name, "Grab_Y", 0));
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name, "Grab_Theta", 0));
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name, "采集时间(ms)", 0));
        }
        public PointCloudAcq()
        {
            this.name = "点云采集";
            this.AcqParam = new PointCloudAcqParam();
            this.AcqParam.AcqSourceName = "";
            if (MotionCardManage.CurrentCoordSys != enCoordSysName.NONE)
                this.AcqParam.CoordSysName = MotionCardManage.CurrentCoordSys;
            else
                this.AcqParam.CoordSysName = enCoordSysName.CoordSys_0;
            this.axisParam = new CoordSysAxisParam(this.AcqParam.CoordSysName);
            ///////////////////////////////////////////////////
            this.ResultInfo = new BindingList<MeasureResultInfo>();
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name, "Width", 0));
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name, "Height", 0));
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name, "Count", 0));
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name, "Grab_X", 0));
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name, "Grab_Y", 0));
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name, "Grab_Theta", 0));
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name, "采集时间(ms)", 0));
        }

        #region 实现接口,各自实现自己的逻辑，就可实现通用的逻辑
        public OperateResult Execute(params object[] param)
        {
            this.Result.Succss = false;
            string state = "none";
            MoveCommandParam moveParam, affineMoveParam;
            Dictionary<enDataItem, object>[] list = null;
            try
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                this.pointData1.ClearData();
                this.pointData1.ClearHandle();
                ////////////////////////////////////
                if (param != null)
                {
                    foreach (var item in param)
                    {
                        switch (item.GetType().Name)
                        {
                            case nameof(PointCloudData):
                                this.Dist1DataHandle = item as PointCloudData;
                                state = "传入图像";
                                break;
                            case nameof(HObjectModel3D):
                                this.Dist1DataHandle = new PointCloudData((HObjectModel3D)item);
                                state = "传入图像";
                                break;
                            case nameof(MoveCommandParam):
                                moveParam = (MoveCommandParam)item;
                                break;
                            case nameof(userWcsCoordSystem):
                                this._wcsCoordSystem = (userWcsCoordSystem)item;
                                break;
                        }
                    }
                }
                ///////////////////////////
                switch (state)
                {
                    case "传入图像":
                        break;
                    default:
                        if (ImageAcqDevice.Instance.IsCamSource)
                        {
                            if (this.Dist1DataHandle != null && this.Dist1DataHandle.IsInitialized())
                                this.Dist1DataHandle.Dispose();
                            if (this.Dist2DataHandle != null && this.Dist2DataHandle.IsInitialized())
                                this.Dist2DataHandle.Dispose();
                            if (this.ThickDataHandle != null && this.ThickDataHandle.IsInitialized())
                                this.ThickDataHandle.Dispose();
                            //////////////////////////////////////
                            double[] X, Y, Z, Dist1, Dist2, Thick;
                            if (this.TrackPoint == null)
                            {
                                moveParam = new MoveCommandParam(true);
                                moveParam.MoveType = enMoveType.none;
                                this.axisParam = moveParam.AxisParam; // 实时使用当前位置
                                affineMoveParam = moveParam.AffineCommandParam(this._wcsCoordSystem);
                                switch (moveParam.MoveType)
                                {
                                    case enMoveType.直线运动:
                                    case enMoveType.点位运动:
                                    case enMoveType.矩形1运动:
                                    case enMoveType.圆运动:
                                    case enMoveType.none:
                                        list = AcqSourceManage.Instance.GetLaserAcqSource(this.AcqParam.AcqSourceName).AcqPointDatas(affineMoveParam);
                                        break;
                                    default:
                                        list = AcqSourceManage.Instance.GetLaserAcqSource(this.AcqParam.AcqSourceName).AcqPointDatas();
                                        break;
                                }
                                ////////////////////
                                X = list[0][enDataItem.X] as double[];
                                Y = list[0][enDataItem.Y] as double[];
                                Dist1 = list[0][enDataItem.Dist1] as double[];
                                if (X != null && X.Length > 0)
                                    this.Dist1DataHandle.Add(new HObjectModel3D(X, Y, Dist1));
                            }
                            else
                            {
                                foreach (var item in this._trackPoint)
                                {
                                    moveParam = new MoveCommandParam(true);
                                    moveParam.MoveType = item.MoveType;
                                    moveParam.MoveAxis = item.MoveAxis;
                                    moveParam.MoveSpeed = GlobalVariable.pConfig.ScanSpeed;
                                    moveParam.StartVel = item.AccDecParam.StartVel;
                                    moveParam.StopVel = item.AccDecParam.StopVel;
                                    moveParam.Tacc = item.AccDecParam.Tacc;
                                    moveParam.Tdec = item.AccDecParam.Tdec;
                                    moveParam.S_para = item.AccDecParam.S_para;
                                    moveParam.IsWait = item.IsWait;
                                    moveParam.MoveTrack = item.RoiShape;
                                    this.axisParam = moveParam.AxisParam; // 实时使用当前位置
                                    affineMoveParam = moveParam.AffineCommandParam(this._wcsCoordSystem);
                                    list = AcqSourceManage.Instance.GetLaserAcqSource(this.AcqParam.AcqSourceName).AcqPointDatas(affineMoveParam);
                                    ////////////////////
                                    X = list[0][enDataItem.X] as double[];
                                    Y = list[0][enDataItem.Y] as double[];
                                    Dist1 = list[0][enDataItem.Dist1] as double[];
                                    if (X != null && X.Length > 0)
                                        this.Dist1DataHandle.Add(new HObjectModel3D(X, Y, Dist1));
                                }
                            }
                        }
                        else // 从文件中读取
                        {
                            if (this.Dist1DataHandle != null && this.Dist1DataHandle.IsInitialized())
                                this.Dist1DataHandle.Dispose();
                            if (this.Dist2DataHandle != null && this.Dist2DataHandle.IsInitialized())
                                this.Dist2DataHandle.Dispose();
                            if (this.ThickDataHandle != null && this.ThickDataHandle.IsInitialized())
                                this.ThickDataHandle.Dispose();
                            //////////////////////////////////////////////////
                            if (ImageAcqDevice.Instance.IsFileSource)
                                this.Dist1DataHandle = this.AcqParam.ReadPointCloud(this.AcqParam.SingleFilePath, this.AcqParam.AcqSourceName);
                            else
                            {
                                if (this.AcqParam.FilePath != null)
                                {
                                    if (this.FileIndex == this.AcqParam.FilePath.Count)
                                    {
                                        this.FileIndex = 0;
                                        MessageBox.Show("对象遍历完成!!!");
                                    }
                                    this.Dist1DataHandle = this.AcqParam.ReadPointCloud(this.AcqParam.SingleFilePath, this.AcqParam.AcqSourceName);
                                    this.Dist1DataHandle.Tag = this.FileIndex;
                                    this.Dist1DataHandle.ViewWindow = this.AcqParam.ViewWindow;
                                    this.Dist1DataHandle.SensorName = this.AcqParam.LaserParam?.SensorName;
                                    this.FileIndex++;
                                }
                            }
                        }
                        break;
                }
                stopwatch.Stop();
                //////////////////////////////////// 输出值
                if (this.Dist1DataHandle != null && this.Dist1DataHandle.IsInitialized())
                {
                    ((BindingList<MeasureResultInfo>)this.ResultInfo)[0].SetValue(this.name, "Width", this.Dist1DataHandle.Width);
                    ((BindingList<MeasureResultInfo>)this.ResultInfo)[1].SetValue(this.name, "Height", this.Dist1DataHandle.Height);
                    ((BindingList<MeasureResultInfo>)this.ResultInfo)[1].SetValue(this.name, "Count", this.Dist1DataHandle.Count());
                    ((BindingList<MeasureResultInfo>)this.ResultInfo)[2].SetValue(this.name, "Grab_X", this.Dist1DataHandle.Grab_X);
                    ((BindingList<MeasureResultInfo>)this.ResultInfo)[3].SetValue(this.name, "Grab_Y", this.Dist1DataHandle.Grab_Y);
                    ((BindingList<MeasureResultInfo>)this.ResultInfo)[4].SetValue(this.name, "Grab_Theta", this.Dist1DataHandle.Grab_Theta);
                    ((BindingList<MeasureResultInfo>)this.ResultInfo)[5].SetValue(this.name, "采集时间(ms)", stopwatch.ElapsedMilliseconds);
                }
                else
                {
                    ((BindingList<MeasureResultInfo>)this.ResultInfo)[0].SetValue(this.name, "Width", 0);
                    ((BindingList<MeasureResultInfo>)this.ResultInfo)[1].SetValue(this.name, "Height", 0);
                    ((BindingList<MeasureResultInfo>)this.ResultInfo)[1].SetValue(this.name, "Count", 0);
                    ((BindingList<MeasureResultInfo>)this.ResultInfo)[2].SetValue(this.name, "Grab_X", 0);
                    ((BindingList<MeasureResultInfo>)this.ResultInfo)[3].SetValue(this.name, "Grab_Y", 0);
                    ((BindingList<MeasureResultInfo>)this.ResultInfo)[4].SetValue(this.name, "Grab_Theta", 0);
                    ((BindingList<MeasureResultInfo>)this.ResultInfo)[5].SetValue(this.name, "采集时间ms", 0);
                }
                OnExcuteCompleted(this.AcqParam.LaserParam?.SensorName, this.AcqParam.LaserParam?.ViewWindow, this.name, this.Dist1DataHandle); // 
                // 使用发命令的方式来更新视图  
                this.Result.Succss = true;
                if (this.Result.Succss)
                    LoggerHelper.Info(this.name + "点云采集完成");
                else
                    LoggerHelper.Error(this.name + "点云采集失败");
                // 更改UI字体　
                UpdataNodeElementStyle(param, this.Result.Succss);
            }
            catch (Exception ex)
            {
                LoggerHelper.Error(this.name + "点云采集报错" + ex);
                return this.Result;
            }
            finally
            {
                /////////////////////
                pointData1.ClearData();
            }
            return this.Result;
        }

        public object GetPropertyValues(string propertyName)
        {
            switch (propertyName)
            {
                case "距离1":
                case "距离1对象":
                    return this.Dist1DataHandle;
                case "距离2":
                case "距离2对象":
                    return this.Dist2DataHandle;
                case "厚度":
                case "厚度对象":
                    return this.ThickDataHandle;
                case "名称":
                case nameof(this.Name):
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
            try
            {
                this.pointData1.ClearHandle();
                if (this.Dist1DataHandle != null && this.Dist1DataHandle.IsInitialized())
                    this.Dist1DataHandle.Dispose();
                if (this.Dist2DataHandle != null && this.Dist2DataHandle.IsInitialized())
                    this.Dist2DataHandle.Dispose();
                if (this.ThickDataHandle != null && this.ThickDataHandle.IsInitialized())
                    this.ThickDataHandle.Dispose();
                OnItemDeleteEvent(this, this.name);
            }
            catch
            {
                LoggerHelper.Error(this.name + "->删除该对象报错");
            }
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





    }

}

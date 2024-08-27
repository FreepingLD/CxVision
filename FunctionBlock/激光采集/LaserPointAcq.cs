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
    [DefaultProperty("Laser1Dist1DataHandle")]
    /// <summary>
    /// 激光取点采集的数据类
    /// </summary>
    public class LaserPointAcq : BaseFunction, IFunction
    {
        private TransformLaserPointCloudDataHandle pointData1 = new TransformLaserPointCloudDataHandle();
        private BindingList<userWcsPoint> _AcqCoordPoint = new BindingList<userWcsPoint>();
        private userWcsCoordSystem _wcsCoordSystem;
        public enCoordSysName CoordSysName
        {
            get;
            set;
        }

        public BindingList<userWcsPoint> AcqCoordPointList { get => _AcqCoordPoint; set => _AcqCoordPoint = value; }

        #region 输入属性
        [DescriptionAttribute("激光采集源")]
        [DisplayName("激光采集源")]
        public AcqSource LaserAcqSource
        {
            get;
            set;
        }

        [DescriptionAttribute("相机采集源")]
        [DisplayName("相机采集源")]
        public AcqSource CamAcqSource
        {
            get;
            set;
        }

        [DisplayName("坐标系")]
        [DescriptionAttribute("输入属性3")]
        public userWcsCoordSystem WcsCoordSystem
        {
            get
            {
                try
                {
                    if (this.RefSource3.Count > 0)
                    {
                        this._wcsCoordSystem = this.GetPropertyValue(this.RefSource3).Last() as userWcsCoordSystem;
                        if (this._wcsCoordSystem == null)
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
        #endregion 

        #region 输出属性
        [DisplayName("激光1距离1")]
        [DescriptionAttribute("输出属性")]
        public HObjectModel3D Laser1Dist1DataHandle
        {
            get;
            set;
        }
        [DisplayName("激光1距离2")]
        [DescriptionAttribute("输出属性")]
        public HObjectModel3D Laser1Dist2DataHandle
        {
            get;
            set;
        }
        [DisplayName("激光1厚度")]
        [DescriptionAttribute("输出属性")]
        public HObjectModel3D Laser1ThickDataHandle
        {
            get;
            set;
        }

        [DisplayName("激光2距离1")]
        [DescriptionAttribute("输出属性")]
        public HObjectModel3D Laser2Dist1DataHandle
        {
            get;
            set;
        }
        [DisplayName("激光2距离2")]
        [DescriptionAttribute("输出属性")]
        public HObjectModel3D Laser2Dist2DataHandle
        {
            get;
            set;
        }
        [DisplayName("激光2厚度")]
        [DescriptionAttribute("输出属性")]
        public HObjectModel3D Laser2ThickDataHandle
        {
            get;
            set;
        }

        [DisplayName("激光3距离1")]
        [DescriptionAttribute("输出属性")]
        public HObjectModel3D Laser3Dist1DataHandle
        {
            get;
            set;
        }
        [DisplayName("激光3距离2")]
        [DescriptionAttribute("输出属性")]
        public HObjectModel3D Laser3Dist2DataHandle
        {
            get;
            set;
        }
        [DisplayName("激光3厚度")]
        [DescriptionAttribute("输出属性")]
        public HObjectModel3D Laser3ThickDataHandle
        {
            get;
            set;
        }

        [DisplayName("激光4距离1")]
        [DescriptionAttribute("输出属性")]
        public HObjectModel3D Laser4Dist1DataHandle
        {
            get;
            set;
        }
        [DisplayName("激光4距离2")]
        [DescriptionAttribute("输出属性")]
        public HObjectModel3D Laser4Dist2DataHandle
        {
            get;
            set;
        }
        [DisplayName("激光4厚度")]
        [DescriptionAttribute("输出属性")]
        public HObjectModel3D Laser4ThickDataHandle
        {
            get;
            set;
        }
        #endregion
        public LaserPointAcq(AcqSource _laserAcqSource, AcqSource _camAcqSource, IFunction coordSystem)
        {
            this.name = "激光点";
            this.LaserAcqSource = _laserAcqSource;
            this.CamAcqSource = _camAcqSource;
            this.CoordSysName = MotionCardManage.CurrentCoordSys;
            if (coordSystem != null)
                this.RefSource2.Add(coordSystem.Name, coordSystem);
            this.AcqCoordPointList.Add(new CoordSysAxisParam(this.CoordSysName).GetWcsPoint()); // 创建时添加一个位置点
            ///////////////////////////////////////////////////
            this.ResultInfo = new BindingList<AcqResultInfo>();
            ((BindingList<AcqResultInfo>)this.ResultInfo).Add(new AcqResultInfo(this.name, "X", 0));
            ((BindingList<AcqResultInfo>)this.ResultInfo).Add(new AcqResultInfo(this.name, "Y", 0));
            ((BindingList<AcqResultInfo>)this.ResultInfo).Add(new AcqResultInfo(this.name, "Dist1", 0));
            ((BindingList<AcqResultInfo>)this.ResultInfo).Add(new AcqResultInfo(this.name, "Dist2", 0));
            ((BindingList<AcqResultInfo>)this.ResultInfo).Add(new AcqResultInfo(this.name, "Thick", 0));
        }
        public LaserPointAcq(AcqSource _laserAcqSource, AcqSource _camAcqSource)
        {
            this.name = "激光点";
            this.LaserAcqSource = _laserAcqSource;
            this.CamAcqSource = _camAcqSource;
            this.CoordSysName = MotionCardManage.CurrentCoordSys;
            this.AcqCoordPointList.Add(new CoordSysAxisParam(this.CoordSysName).GetWcsPoint()); // 创建时添加一个位置点
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
            Dictionary<enDataItem, object>[] list=new Dictionary<enDataItem, object>[1];
            userLaserCalibrateParam refCalibrateDataValue;
            try
            {
                if (this.LaserAcqSource == null)
                {
                    throw new ArgumentNullException("未找到该传感器：" + this.LaserAcqSource);
                }
                pointData1.ClearData();
                this.pointData1.ClearHandle();
                this.resultDataTable.Rows.Clear();
                ((BindingList<AcqResultInfo>)this.ResultInfo).Clear();
                ////////////////////////////////////
                //list = this.LaserAcqSource.AcqPointDatas(this.AcqCoordPointList, this.WcsCoordSystem);
                /// 这一步是为了让激光点的坐标与相机坐标相一致    
                //////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                this.ResultInfo = AcqResultInfo.InitList(list.Length);
                for (int i = 0; i < list.Length; i++)
                {
                    switch (i)
                    {
                        default:
                        case 0:
                            if (this.Laser1Dist1DataHandle != null && this.Laser1Dist1DataHandle.IsInitialized())
                                this.Laser1Dist1DataHandle.Dispose();
                            if (this.Laser1Dist2DataHandle != null && this.Laser1Dist2DataHandle.IsInitialized())
                                this.Laser1Dist2DataHandle.Dispose();
                            if (this.Laser1ThickDataHandle != null && this.Laser1ThickDataHandle.IsInitialized())
                                this.Laser1ThickDataHandle.Dispose();
                            /////////////////////////////////////
                            this.Laser1Dist1DataHandle = new HObjectModel3D((double[])list[i][enDataItem.X], (double[])list[i][enDataItem.Y], (double[])list[i][enDataItem.Dist1]);
                            this.Laser1Dist2DataHandle = new HObjectModel3D((double[])list[i][enDataItem.X], (double[])list[i][enDataItem.Y], (double[])list[i][enDataItem.Dist2]);
                            this.Laser1ThickDataHandle = new HObjectModel3D((double[])list[i][enDataItem.X], (double[])list[i][enDataItem.Y], (double[])list[i][enDataItem.Thick]);

                            //// 输出值
                            if (this.Laser1Dist1DataHandle != null && this.Laser1Dist1DataHandle.IsInitialized())
                                ((BindingList<AcqResultInfo>)this.ResultInfo)[0].SetValue(this.name + "激光1", "采集数量", this.Laser1Dist1DataHandle.GetObjectModel3dParams("point_coord_z").TupleMean().D, this.Laser1Dist1DataHandle.GetObjectModel3dParams("num_points").I);
                            OnExcuteCompleted(this.name, this.Laser1Dist1DataHandle);
                            break;
                        case 1:
                            if (this.Laser2Dist1DataHandle != null && this.Laser2Dist1DataHandle.IsInitialized())
                                this.Laser2Dist1DataHandle.Dispose();
                            if (this.Laser2Dist2DataHandle != null && this.Laser2Dist2DataHandle.IsInitialized())
                                this.Laser2Dist2DataHandle.Dispose();
                            if (this.Laser2ThickDataHandle != null && this.Laser2ThickDataHandle.IsInitialized())
                                this.Laser2ThickDataHandle.Dispose();
                            /////////////////////////////////////
                            this.Laser2Dist1DataHandle = new HObjectModel3D((double[])list[i][enDataItem.X], (double[])list[i][enDataItem.Y], (double[])list[i][enDataItem.Dist1]);
                            this.Laser2Dist2DataHandle = new HObjectModel3D((double[])list[i][enDataItem.X], (double[])list[i][enDataItem.Y], (double[])list[i][enDataItem.Dist2]);
                            this.Laser2ThickDataHandle = new HObjectModel3D((double[])list[i][enDataItem.X], (double[])list[i][enDataItem.Y], (double[])list[i][enDataItem.Thick]);
                            //// 输出值
                            if (this.Laser2Dist1DataHandle != null && this.Laser2Dist1DataHandle.IsInitialized())
                                ((BindingList<AcqResultInfo>)this.ResultInfo)[1].SetValue(this.name + "激光2", "采集数量", this.Laser2Dist1DataHandle.GetObjectModel3dParams("point_coord_z").TupleMean().D, this.Laser2Dist1DataHandle.GetObjectModel3dParams("num_points").I);
                            OnExcuteCompleted(this.name, this.Laser2Dist1DataHandle);
                            break;
                        case 2:
                            if (this.Laser3Dist1DataHandle != null && this.Laser3Dist1DataHandle.IsInitialized())
                                this.Laser3Dist1DataHandle.Dispose();
                            if (this.Laser3Dist2DataHandle != null && this.Laser3Dist2DataHandle.IsInitialized())
                                this.Laser3Dist2DataHandle.Dispose();
                            if (this.Laser3ThickDataHandle != null && this.Laser3ThickDataHandle.IsInitialized())
                                this.Laser3ThickDataHandle.Dispose();
                            /////////////////////////////////////
                            this.Laser3Dist1DataHandle = new HObjectModel3D((double[])list[i][enDataItem.X], (double[])list[i][enDataItem.Y], (double[])list[i][enDataItem.Dist1]);
                            this.Laser3Dist2DataHandle = new HObjectModel3D((double[])list[i][enDataItem.X], (double[])list[i][enDataItem.Y], (double[])list[i][enDataItem.Dist2]);
                            this.Laser3ThickDataHandle = new HObjectModel3D((double[])list[i][enDataItem.X], (double[])list[i][enDataItem.Y], (double[])list[i][enDataItem.Thick]);
                            //// 输出值
                            if (this.Laser3Dist1DataHandle != null && this.Laser3Dist1DataHandle.IsInitialized())
                                ((BindingList<AcqResultInfo>)this.ResultInfo)[2].SetValue(this.name + "激光3", "采集数量", this.Laser3Dist1DataHandle.GetObjectModel3dParams("point_coord_z").TupleMean().D, this.Laser3Dist1DataHandle.GetObjectModel3dParams("num_points").I);
                            OnExcuteCompleted(this.name, this.Laser3Dist1DataHandle);
                            break;
                        case 3:
                            if (this.Laser4Dist1DataHandle != null && this.Laser4Dist1DataHandle.IsInitialized())
                                this.Laser4Dist1DataHandle.Dispose();
                            if (this.Laser4Dist2DataHandle != null && this.Laser4Dist2DataHandle.IsInitialized())
                                this.Laser4Dist2DataHandle.Dispose();
                            if (this.Laser4ThickDataHandle != null && this.Laser4ThickDataHandle.IsInitialized())
                                this.Laser4ThickDataHandle.Dispose();
                            /////////////////////////////////////
                            this.Laser4Dist1DataHandle = new HObjectModel3D((double[])list[i][enDataItem.X], (double[])list[i][enDataItem.Y], (double[])list[i][enDataItem.Dist1]);
                            this.Laser4Dist2DataHandle = new HObjectModel3D((double[])list[i][enDataItem.X], (double[])list[i][enDataItem.Y], (double[])list[i][enDataItem.Dist2]);
                            this.Laser4ThickDataHandle = new HObjectModel3D((double[])list[i][enDataItem.X], (double[])list[i][enDataItem.Y], (double[])list[i][enDataItem.Thick]);
                            //// 输出值
                            if (this.Laser4Dist1DataHandle != null && this.Laser4Dist1DataHandle.IsInitialized())
                                ((BindingList<AcqResultInfo>)this.ResultInfo)[3].SetValue(this.name + "激光4", "采集数量", this.Laser4Dist1DataHandle.GetObjectModel3dParams("point_coord_z").TupleMean().D, this.Laser4Dist1DataHandle.GetObjectModel3dParams("num_points").I);
                            OnExcuteCompleted(this.name, this.Laser4Dist1DataHandle);
                            break;
                    }
                }
                //////////// 只包含一个产品的数据                 

                // 使用发命令的方式来更新视图  
                this.Result.Succss = true;
                if (this.Result.Succss)
                    LoggerHelper.Info(this.name + "激光数据采集完成");
                else
                    LoggerHelper.Error(this.name + "激光数据采集失败");
            }
            catch (Exception e)
            {
                LoggerHelper.Error(this.name + "激光数据采集报错" + e);
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
            //throw new NotImplementedException();
        }
        #endregion





    }

}

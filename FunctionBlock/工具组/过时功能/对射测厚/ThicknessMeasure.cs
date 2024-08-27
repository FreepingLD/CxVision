using Common;
using MotionControlCard;
using Sensor;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FunctionBlock
{
    [Serializable]
    [DefaultProperty("Tool")] // 表示这个类是工具类
    public class ThicknessMeasure : BaseFunction, IFunction
    {
        [NonSerialized]
        private TreeNode _ParentNode;
        private BindingList<TrackMoveParam> _trackParam;
        public TreeNode ParentNode { get { return _ParentNode; } set { this._ParentNode = value; } }
        public BindingList<TrackMoveParam> TrackParam { get => _trackParam; set => _trackParam = value; }

        public ThicknessMeasure()
        {
            this._trackParam = new BindingList<TrackMoveParam>();
        }

        public OperateResult Execute(params object[] param)
        {
            this.Result.Succss = false;
            try
            {
                //////////////程序执行
                TreeView treeView = null;
                foreach (var item in param)
                {
                    if (item is TreeNode)
                    {
                        treeView = ((TreeNode)item).TreeView; // 获取树控件
                        this.ParentNode = ((TreeNode)item);
                        break;
                    }
                }
                if (treeView == null)
                {
                    LoggerHelper.Error(this.name + "->执行失败" + "treeView视图为空");
                    return this.Result;
                }
                ///////////////////////////////////////////////
                bool IsOk = true;
                if (this.ParentNode != null)
                {
                    MoveCommandParam CommandParam, AffineCommandParam;
                    IMotionControl _Card = null;
                    ISensor _Sensor = null;
                    List<PointCloudAcq> acqDeviceList = new List<PointCloudAcq>();
                    foreach (TreeNode item in ParentNode.Nodes)
                    {
                        switch (item.Tag.GetType().Name)
                        {
                            case nameof(PointCloudAcq):
                                acqDeviceList.Add(((PointCloudAcq)item.Tag));
                                break;
                        }
                    }
                    if (acqDeviceList.Count == 0) return this.Result;
                    //////////////  清空对象  /////
                    foreach (var item in acqDeviceList)
                    {
                        item.Dist1DataHandle?.ClearObjectModel3d();
                        item.Dist2DataHandle?.ClearObjectModel3d();
                        item.ThickDataHandle?.ClearObjectModel3d();
                    }
                    _Card = MotionCardManage.GetCard(acqDeviceList[0].AcqParam.CoordSysName);
                    _Sensor = AcqSourceManage.Instance.GetAcqSource(acqDeviceList[0].AcqParam.AcqSourceName).Sensor;
                    Dictionary<enDataItem, object> dicList;// = new Dictionary<enDataItem, object>[acqDeviceList.Count];
                    foreach (var item2 in this._trackParam)
                    {
                        if (!item2.IsActive) continue; // 如果没有激活，则不执行
                        CommandParam = new MoveCommandParam();
                        CommandParam.MoveType = item2.MoveType;
                        CommandParam.MoveSpeed = GlobalVariable.pConfig.MoveSpeed;
                        CommandParam.MoveAxis = enAxisName.XY轴;
                        CommandParam.CoordSysName = acqDeviceList[0].AcqParam.CoordSysName;      // 运动坐标系里是否需要包含移动指令
                        switch (item2.MoveType)
                        {
                            case enMoveType.点位运动:
                                drawWcsPoint wcsPoint = item2.RoiShape as drawWcsPoint;
                                if (wcsPoint != null)
                                {
                                    CommandParam.AxisParam = new CoordSysAxisParam(wcsPoint.X, wcsPoint.Y, 0, 0, 0, 0);
                                    CommandParam.AxisParam2 = new CoordSysAxisParam(wcsPoint.X, wcsPoint.Y, 0, 0, 0, 0);
                                }
                                break;
                            case enMoveType.直线运动:
                                drawWcsLine wcLine = item2.RoiShape as drawWcsLine;
                                if (wcLine != null)
                                {
                                    CommandParam.AxisParam = new CoordSysAxisParam(wcLine.X1, wcLine.Y1, 0, 0, 0, 0);
                                    CommandParam.AxisParam2 = new CoordSysAxisParam(wcLine.X2, wcLine.Y2, 0, 0, 0, 0);
                                }
                                break;
                        }
                        ////////////////////////////////////////////////////////////
                        AffineCommandParam = CommandParam;
                        foreach (var item in acqDeviceList)
                        {
                            if (item.WcsCoordSystem != null)
                                AffineCommandParam = CommandParam.Affine2DCommandParam(item.WcsCoordSystem);
                        }
                        if (_Card != null)
                            _Card?.MoveMultyAxis(AffineCommandParam);
                        LoggerHelper.Info(this.name + "运动到扫描起点:" + AffineCommandParam.ToString());
                        foreach (var item in acqDeviceList)
                        {
                            Thread.Sleep(AcqSourceManage.Instance.GetAcqSource(item.AcqParam.AcqSourceName).Sensor.CameraParam.WaiteTime); // 采集停顿时间
                        }
                        // 在图像采集时一定要设置这个停顿时间
                        LoggerHelper.Info(this.name + "开始采集");
                        foreach (var item in acqDeviceList)
                        {
                            AcqSourceManage.Instance.GetAcqSource(item.AcqParam.AcqSourceName).Sensor?.StartTrigger();
                        }
                        AcqSourceManage.Instance.GetAcqSource(acqDeviceList[0].AcqParam.AcqSourceName).SetIoOutput(true);
                        foreach (var item in acqDeviceList)
                        {
                            Thread.Sleep(AcqSourceManage.Instance.GetAcqSource(item.AcqParam.AcqSourceName).Sensor.CameraParam.AcqWaiteTime); // 采集等待时间
                        }
                        AcqSourceManage.Instance.GetAcqSource(acqDeviceList[0].AcqParam.AcqSourceName).SetIoOutput(false);
                        foreach (var item in acqDeviceList)
                        {
                            AcqSourceManage.Instance.GetAcqSource(item.AcqParam.AcqSourceName).Sensor?.StopTrigger();
                        }
                        LoggerHelper.Info(this.name + "停止采集");
                        foreach (var item in acqDeviceList)
                        {
                            dicList = AcqSourceManage.Instance.GetAcqSource(item.AcqParam.AcqSourceName).Sensor?.ReadData();
                            switch (item2.MoveType)
                            {
                                case enMoveType.点位运动:
                                    double[] dist1 = (double[])dicList[enDataItem.Dist1];
                                    double[] dist2 = (double[])dicList[enDataItem.Dist2];
                                    double[] thick = (double[])dicList[enDataItem.Thick];
                                    item.Dist1DataHandle.Add(new HalconDotNet.HObjectModel3D(AffineCommandParam.AxisParam.X, AffineCommandParam.AxisParam.Y, dist1.Average()));
                                    break;
                                case enMoveType.直线运动:
                                    item.Dist1DataHandle = (dicList[enDataItem.Dist1Modle] as PointCloudData);
                                    break;
                            }
                        }
                        LoggerHelper.Info(this.name + "获取数据完成");
                        ///////
                        foreach (TreeNode item in ParentNode.Nodes)
                        {
                            switch (item.Tag.GetType().Name)
                            {
                                case nameof(ImageAcq):
                                case nameof(PointCloudAcq):
                                    if (treeView != null)
                                        treeView.Invoke(new Action(() => treeView.SelectedNode = item));
                                    break;
                                default:
                                    if (treeView != null)
                                        treeView.Invoke(new Action(() => treeView.SelectedNode = item));
                                    this.Result = ((IFunction)item.Tag).Execute(item);
                                    if (!this.Result.Succss)
                                    {
                                        this.Result.ErrorMessage += this.Name + "." + item.Text + "->" + "执行失败";
                                        IsOk = false;
                                    }
                                    break;
                            }
                        }
                    }
                }
                this.Result.Succss = IsOk;
                if (this.ParentNode != null)
                    treeView?.Invoke(new Action(() => this.ParentNode.Collapse()));
            }
            catch (Exception ex)
            {
                LoggerHelper.Error(this.name + "->执行错误" + ex);
                this.Result.Succss = false;
            }
            if (this.Result.Succss)
                LoggerHelper.Info(this.name + "->执行成功");
            else
                LoggerHelper.Error(this.name + "->执行失败");
            // 更改UI字体　
            UpdataNodeElementStyle(param, this.Result.Succss);
            return this.Result;
        }

        public object GetPropertyValues(string propertyName)
        {
            switch (propertyName)
            {
                case "名称":
                case nameof(this.Name):
                    return this.name;
                default:
                    return ""; // this.FeaturePoint;
            }
        }

        public bool SetPropertyValues(string propertyName, params object[] value)
        {
            switch (propertyName)
            {
                case "名称":
                case nameof(this.Name):
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
                OnItemDeleteEvent(this, this.name);
            }
            catch (Exception ex)
            {
                LoggerHelper.Error(this.name + "删除节点出错" + ex.ToString());
            }
        }
        public void Read(string path)
        {
            // throw new NotImplementedException();
        }
        public void Save(string path)
        {
            //throw new NotImplementedException();
        }

    }
}

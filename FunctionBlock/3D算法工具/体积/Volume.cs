using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HalconDotNet;
using Sensor;
using MotionControlCard;
using System.Windows.Forms;
using System.IO;
using AlgorithmsLibrary;
using System.ComponentModel;
using Common;
using System.Data;

namespace FunctionBlock
{
    /// <summary>
    /// 体积的计算是需要基准面的
    /// </summary>
    [Serializable]
    [DefaultProperty(nameof(Value))]
    public class Volume : BaseFunction, IFunction
    {
        private double _Value; // 测量结果
        [NonSerialized]
        private PointCloudData _dataObjectModel = null;

        [NonSerialized]
        private PointCloudData _refPlaneHandle3D = null;

        [DisplayName("体积")]
        [DescriptionAttribute("输出属性")]
        public double Value { get => _Value; set => _Value = value; }

        [DisplayName("输入对象3D")]
        [DescriptionAttribute("输入属性1")]
        public PointCloudData DataObjectModel
        {
            get
            {
                try
                {
                    if (this.RefSource1.Count > 0)
                    {
                        List<PointCloudData> listObjectModel3D = new List<PointCloudData>();
                        object[] oo = this.GetPropertyValue(this.RefSource1);
                        if (oo != null && oo.Length > 0)
                        {
                            foreach (var item in oo)
                            {
                                switch (item.GetType().Name)
                                {
                                    case nameof(HTuple):
                                        this._dataObjectModel = (new PointCloudData(new HObjectModel3D(((HTuple)item).IP)));
                                        break;
                                    case nameof(HObjectModel3D):
                                        this._dataObjectModel = (new PointCloudData((HObjectModel3D)item));
                                        break;
                                    case "HObjectModel3D[]":
                                        this._dataObjectModel = (new PointCloudData(((HObjectModel3D[])item)[0]));
                                        break;
                                    case nameof(PointCloudData):
                                        this._dataObjectModel = ((PointCloudData)item);
                                        break;
                                    case "PointCloudData[]":
                                        this._dataObjectModel = ((PointCloudData[])item)[0];
                                        break;
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return this._dataObjectModel;
            }
            set
            {
                this._dataObjectModel = value;
            }
        }

        [DisplayName("参考平面")]
        [DescriptionAttribute("输入属性2")]
        public PointCloudData RefPlaneHandle3D
        {
            get
            {
                try
                {
                    if (this.RefSource2.Count > 0)
                    {
                        List<PointCloudData> listObjectModel3D = new List<PointCloudData>();
                        object[] oo = this.GetPropertyValue(this.RefSource2);
                        if (oo != null && oo.Length > 0)
                        {
                            foreach (var item in oo)
                            {
                                switch (item.GetType().Name)
                                {
                                    case nameof(HTuple):
                                        this._refPlaneHandle3D = (new PointCloudData(new HObjectModel3D(((HTuple)item).IP)));
                                        break;
                                    case nameof(HObjectModel3D):
                                        this._refPlaneHandle3D = (new PointCloudData((HObjectModel3D)item));
                                        break;
                                    case "HObjectModel3D[]":
                                        this._refPlaneHandle3D = (new PointCloudData(((HObjectModel3D[])item)[0]));
                                        break;
                                    case nameof(PointCloudData):
                                        this._refPlaneHandle3D = ((PointCloudData)item);
                                        break;
                                    case "PointCloudData[]":
                                        this._refPlaneHandle3D = ((PointCloudData[])item)[0];
                                        break;
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return this._refPlaneHandle3D;
            }
            set
            {
                this._refPlaneHandle3D = value;
            }
        }

        public VolumeParam Param { get; set; }
        public Volume()
        {
            this.Param = new VolumeParam();
            this.ResultInfo = new BindingList<MeasureResultInfo>();
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo());
        }




        #region 实现接口

        public OperateResult Execute(params object[] param)
        {
            this.Result.Succss = false;
            try
            {
                if (this._dataObjectModel != null && this._dataObjectModel.IsInitialized())
                    this._dataObjectModel.Dispose();
                this.Result.Succss = this.Param.CalculateVolumeRelativePlane(this.DataObjectModel.ObjectModel3D, this.RefPlaneHandle3D.ObjectModel3D, out this._Value);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[0].SetValue(this.name, "平面度", this._Value);
            }
            catch (Exception ex)
            {
                LoggerHelper.Error(this.name + "->执行错误" + ex);
                return this.Result;
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
                case "计算结果":
                default:
                case nameof(this.Value):
                    return this.Value; //
                case "输入对象":
                    return this.DataObjectModel; //
                case "参考平面":
                    return this.RefPlaneHandle3D; //
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
                this.DataObjectModel?.ClearObjectModel3d();
                this.RefPlaneHandle3D?.ClearObjectModel3d();
                OnItemDeleteEvent(this, this.name);
            }
            catch
            {
                LoggerHelper.Error(this.name + "->删除该对象报错");
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
        #endregion


    }
}

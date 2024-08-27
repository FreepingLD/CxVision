using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HalconDotNet;
using System.Drawing;
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
    [Serializable]
    [DefaultProperty(nameof(CropObjectModel))]
    public class CropObjectModel3D : BaseFunction, IFunction
    {
        [NonSerialized]
        private PointCloudData _dataHandle3D = null;

        private userWcsCoordSystem _wcsCoordSystem = null;
        public BindingList<CropParam> ParamList { get; set; }

        public CropMethod ParamMethod { get; set; }
        public CropObjectModel3D()
        {
            this.ParamMethod = new CropMethod();
            this.ParamList = new BindingList<CropParam>();
        }

        [DisplayName("输入对象3D")]
        [DescriptionAttribute("输入属性1")]
        public PointCloudData DataHandle3D
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
                                        this._dataHandle3D = (new PointCloudData(new HObjectModel3D(((HTuple)item).IP)));
                                        break;
                                    case nameof(HObjectModel3D):
                                        this._dataHandle3D = (new PointCloudData((HObjectModel3D)item));
                                        break;
                                    case "HObjectModel3D[]":
                                        this._dataHandle3D = (new PointCloudData(((HObjectModel3D[])item)[0]));
                                        break;
                                    case nameof(PointCloudData):
                                        this._dataHandle3D = ((PointCloudData)item);
                                        break;
                                    case "PointCloudData[]":
                                        this._dataHandle3D = ((PointCloudData[])item)[0];
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
                return this._dataHandle3D;
            }
            set
            {
                this._dataHandle3D = value;
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
                    if (this.RefSource1.Count > 0)
                    {
                        object[] oo = this.GetPropertyValue(this.RefSource1);
                        if (oo != null && oo.Length > 0)
                        {
                            foreach (var item in oo)
                            {
                                switch (item.GetType().Name)
                                {
                                    case "userWcsPose":
                                        _wcsCoordSystem = new userWcsCoordSystem((userWcsPose)item);
                                        break;
                                    case "userWcsCoordSystem":
                                        _wcsCoordSystem = ((userWcsCoordSystem)item);
                                        break;
                                    case "userWcsCoordSystem[]":
                                        _wcsCoordSystem = ((userWcsCoordSystem[])item)[0];
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
                return this._wcsCoordSystem;
            }
            set
            {
                this._wcsCoordSystem = value;
            }
        }

        [DisplayName("输出对象3D")]
        [DescriptionAttribute("输出属性")]
        public PointCloudData CropObjectModel { get; set; }




        #region 实现接口
        public OperateResult Execute(params object[] param)
        {
            this.Result.Succss = false;
            try
            {
                HObjectModel3D [] outObjectMOdel;
                if (this._dataHandle3D != null && this._dataHandle3D.IsInitialized())
                    this._dataHandle3D.ClearObjectModel3d();
                Result.Succss = this.ParamMethod.CropObjectModel3D(this.DataHandle3D.ObjectModel3D, this.ParamList, 
                                                                   this._dataHandle3D.LaserParams.CamParam, 
                                                                   this._dataHandle3D.LaserParams.CamPose,
                                                                   out outObjectMOdel);
                ////////////////////////////////////////////////////////////////////////////////////
                this.CropObjectModel = new PointCloudData(outObjectMOdel);
                this.CropObjectModel.LaserParams = this._dataHandle3D.LaserParams;
                this.CropObjectModel.SensorName = this._dataHandle3D.SensorName;
                this.CropObjectModel.ViewWindow = this._dataHandle3D.ViewWindow;
                OnExcuteCompleted(this.CropObjectModel.SensorName, this.CropObjectModel.ViewWindow, this.name, this.CropObjectModel);
            }
            catch (Exception ex)
            {
                LoggerHelper.Error(this.name + "->执行错误" + ex);
                return Result;
            }
            if (Result.Succss)
                LoggerHelper.Info(this.name + "->执行成功");
            else
                LoggerHelper.Error(this.name + "->执行失败");
            // 更改UI字体　
            UpdataNodeElementStyle(param, Result.Succss);
            return Result;
        }
        public object GetPropertyValues(string propertyName)
        {
            switch (propertyName)
            {
                default:
                case "名称":
                case "Name":
                    return this.name;
                case "输出对象3D":
                case "输出对象":
                case "裁剪对象3D":
                    return this.CropObjectModel; //
                case "输入对象3D":
                case "输入对象":
                    return this.DataHandle3D; //
            }
        }
        public bool SetPropertyValues(string propertyName, params object[] value)
        {
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
                this._dataHandle3D?.ClearObjectModel3d();
                this.CropObjectModel?.ClearObjectModel3d();
            }
            catch
            {
                LoggerHelper.Error(this.name + "->删除该对象报错");
            }
            finally
            {
                OnItemDeleteEvent(this, this.name);
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

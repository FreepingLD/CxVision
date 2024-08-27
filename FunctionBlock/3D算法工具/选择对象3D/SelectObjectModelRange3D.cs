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
using Common;
using System.ComponentModel;

namespace FunctionBlock
{
    /// <summary>
    /// 体积的计算是需要基准面的
    /// </summary>
    [Serializable]
    [DefaultProperty(nameof(SelectObjectModel))]
    public class SelectObjectModelRange3D : BaseFunction, IFunction
    {
        [NonSerialized]
        private PointCloudData _dataObjectModel = null;

        public SelectObjectParam SelectParam { get ; set ; }

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

        [DisplayName("输出对象3D")]
        [DescriptionAttribute("输出属性")]
        public PointCloudData SelectObjectModel{ get ; set ; }


        public SelectObjectModelRange3D()
        {
            this.SelectParam = new SelectObjectParam();
        }


        #region 实现接口

        public OperateResult Execute(params object[] param)
        {
            this.Result.Succss = false;
            try
            {  
                HObjectModel3D[] selectHandle3D = null;
                this.SelectObjectModel?.ClearObjectModel3d(); // 赋值前，清空之前的对象
                this.Result.Succss = this.SelectParam.SelectObjectModel3D(this.DataObjectModel.ObjectModel3D, out selectHandle3D);
                this.SelectObjectModel = new PointCloudData(selectHandle3D);
                this.SelectObjectModel.SensorName = this._dataObjectModel?.SensorName;
                this.SelectObjectModel.ViewWindow = this._dataObjectModel?.ViewWindow;
                OnExcuteCompleted(this.SelectObjectModel.SensorName, this.SelectObjectModel.ViewWindow, this.name, this.SelectObjectModel);
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
                default:
                case "名称":
                case nameof(this.Name):
                    return this.name;
                case "选择对象3D":
                    return this.SelectObjectModel;
                case "输入对象3D":
                    return this.DataObjectModel;
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
                /////////////////////             
                default:
                    return true;
            }
        }
        public void ReleaseHandle()
        {
            try
            {
                this.DataObjectModel?.ClearObjectModel3d();
                this.SelectObjectModel?.ClearObjectModel3d();
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
          //  throw new NotImplementedException();
        }
        public void Save(string path)
        {
           // throw new NotImplementedException();
        }
        #endregion



    }
}

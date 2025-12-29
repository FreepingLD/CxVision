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
using System.Diagnostics;

namespace FunctionBlock
{
    [Serializable]
    [DefaultProperty(nameof(RectifyHandle3D))]
    public class RectifyObjectModel3D : BaseFunction, IFunction
    {
        [NonSerialized]
        private PointCloudData _dataHandle3D = null;

        private userWcsCoordSystem _wcsCoordSystem = null;
        public BindingList<RectifyParam> ParamList { get; set; }

        public RectifyMethod ParamMethod { get; set; }


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
        public PointCloudData RectifyHandle3D { get; set; }

        [DisplayName("输出位姿3D")]
        [DescriptionAttribute("输出属性")]
        public userWcsPose Pose { get; set; }

        public RectifyObjectModel3D()
        {
            this.ParamMethod = new RectifyMethod();
            this.ParamList = new BindingList<RectifyParam>();
            this.ResultInfo = new BindingList<MeasureResultInfo>();
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name, "Width", 0));
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name, "Height", 0));
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name, "Grab_X", 0));
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name, "Grab_Y", 0));
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name, "Grab_Theta", 0));
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name, "采集时间(ms)", 0));
        }
        #region 实现接口
        public OperateResult Execute(params object[] param)
        {
            this.Result.Succss = false;
            try
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                HObjectModel3D outObjectMOdel;
                userWcsPose pose3D;
                // this._dataHandle3D?.ClearObjectModel3d(); // 这里不能释放
                Result.Succss = this.ParamMethod.RectifyObjectModel3D(this.DataHandle3D.ObjectModel3D, this.ParamList, 
                                                                      this._dataHandle3D.LaserParams.CamParam, 
                                                                      this._dataHandle3D.LaserParams.CamPose,
                                                                      out outObjectMOdel, out pose3D);
                ////////////////////////////////////////////////////////////////////////////////////
                this.RectifyHandle3D = new PointCloudData(outObjectMOdel);
                this.Pose = pose3D;
                this.RectifyHandle3D.SensorName = this._dataHandle3D.SensorName;
                this.RectifyHandle3D.ViewWindow = this._dataHandle3D.ViewWindow;
                this.RectifyHandle3D.Width = this._dataHandle3D.Width;
                this.RectifyHandle3D.Height = this._dataHandle3D.Height;
                this.RectifyHandle3D.Grab_X = this._dataHandle3D.Grab_X;
                this.RectifyHandle3D.Grab_Y = this._dataHandle3D.Grab_Y;
                this.RectifyHandle3D.Grab_Theta = this._dataHandle3D.Grab_Theta;
                OnExcuteCompleted(this.RectifyHandle3D.SensorName, this.RectifyHandle3D.ViewWindow, this.name, this.RectifyHandle3D);
                stopwatch.Stop();
                //////////////////////////////////// 输出值
                if (this.RectifyHandle3D != null && this.RectifyHandle3D.IsInitialized())
                {
                    ((BindingList<MeasureResultInfo>)this.ResultInfo)[0].SetValue(this.name, "Width", this.RectifyHandle3D.Width);
                    ((BindingList<MeasureResultInfo>)this.ResultInfo)[1].SetValue(this.name, "Height", this.RectifyHandle3D.Height);
                    ((BindingList<MeasureResultInfo>)this.ResultInfo)[2].SetValue(this.name, "Grab_X", this.RectifyHandle3D.Grab_X);
                    ((BindingList<MeasureResultInfo>)this.ResultInfo)[3].SetValue(this.name, "Grab_Y", this.RectifyHandle3D.Grab_Y);
                    ((BindingList<MeasureResultInfo>)this.ResultInfo)[4].SetValue(this.name, "Grab_Theta", this.RectifyHandle3D.Grab_Theta);
                    ((BindingList<MeasureResultInfo>)this.ResultInfo)[5].SetValue(this.name, "采集时间(ms)", stopwatch.ElapsedMilliseconds);
                }
                else
                {
                    ((BindingList<MeasureResultInfo>)this.ResultInfo)[0].SetValue(this.name, "Width", 0);
                    ((BindingList<MeasureResultInfo>)this.ResultInfo)[1].SetValue(this.name, "Height", 0);
                    ((BindingList<MeasureResultInfo>)this.ResultInfo)[2].SetValue(this.name, "Grab_X", 0);
                    ((BindingList<MeasureResultInfo>)this.ResultInfo)[3].SetValue(this.name, "Grab_Y", 0);
                    ((BindingList<MeasureResultInfo>)this.ResultInfo)[4].SetValue(this.name, "Grab_Theta", 0);
                    ((BindingList<MeasureResultInfo>)this.ResultInfo)[5].SetValue(this.name, "采集时间ms", 0);
                }
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
                case "矫正对象3D":
                    return this.RectifyHandle3D; //
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
                this.RectifyHandle3D?.ClearObjectModel3d();
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

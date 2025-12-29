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
using Light;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using System.Diagnostics;

namespace FunctionBlock
{
    [Serializable]
    [DefaultProperty(nameof(WcsCircleSector))]
    public class ManualCircleSectorMeasure : BaseFunction, IFunction
    {
        [NonSerialized]
        private ImageDataClass _imageData;
        private FindManualCircleSector _findCrossPoint = new FindManualCircleSector();
        private userWcsCoordSystem _wcsCoordSystem;
        private userPixCoordSystem _pixCoordSystem;
        private MoveCommandParam CommandParam;
        private string _AcqSourceName;
        [NonSerialized]
        private userWcsCircleSector _wcsCircleSector;
        [NonSerialized]
        private userWcsCircleSector _camWcsPoint;
        private bool _isAcqImage = false;
        private bool _isUpdateImage = false;
        public FindManualCircleSector FindCircleSector { get => _findCrossPoint; set => _findCrossPoint = value; }


        [DisplayName("采集源")]
        public string AcqSourceName { get => _AcqSourceName; set => _AcqSourceName = value; }

        [DisplayName("输入图像")]
        [DescriptionAttribute("输入属性1")]
        public ImageDataClass ImageData
        {
            get
            {
                try
                {
                    if (this.RefSource1.Count > 0)
                    {
                        object[] oo = this.GetPropertyValue(this.RefSource1);
                        if (oo != null && oo.Length > 0)
                            this._imageData = oo.Last() as ImageDataClass;
                        this._isUpdateImage = false;
                    }
                    else
                    {
                        if (this._AcqSourceName != null && this._AcqSourceName != "NONE" && this._isAcqImage)
                        {
                            MoveCommandParam affineCommandParam = CommandParam.Affine2DCommandParam(this.PixCoordSystem.GetWcsCoordSystem());
                            Dictionary<enDataItem, object> dic = AcqSourceManage.Instance.GetAcqSource(this._AcqSourceName)?.AcqImageData(this.LightParam);
                            if (dic != null && dic.Count > 0)
                                this._imageData = dic[enDataItem.Image] as ImageDataClass;
                            else
                                return null;
                            CoordSysAxisPosParam currentPosition = new CoordSysAxisPosParam(AcqSourceManage.Instance.GetAcqSource(this._AcqSourceName).CoordSysName);
                            if (this._imageData != null)
                            {
                                this._imageData.Grab_X = currentPosition.X;
                                this._imageData.Grab_Y = currentPosition.Y;
                                this._imageData.Grab_Z = currentPosition.Z;
                                this._imageData.Grab_Theta = currentPosition.Theta;
                            }
                            this._isUpdateImage = true;
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return _imageData;
            }
            set
            {
                _imageData = value;
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

        [DisplayName("坐标系")]
        [DescriptionAttribute("输入属性2")]
        public userPixCoordSystem PixCoordSystem
        {
            get
            {
                try
                {
                    if (this.RefSource2.Count > 0)
                    {
                        object[] oo = this.GetPropertyValue(this.RefSource2);
                        foreach (var item in oo)
                        {
                            if (item != null)
                            {
                                switch (item.GetType().Name)
                                {
                                    case nameof(userPixCoordSystem):
                                        this._pixCoordSystem = item as userPixCoordSystem;
                                        break;
                                    case nameof(userWcsCoordSystem):
                                        this._pixCoordSystem = ((userWcsCoordSystem)item).GetPixCoordSystem();
                                        break;
                                    default:
                                        this._pixCoordSystem = new userPixCoordSystem();
                                        break;
                                }
                            }
                        }
                    }
                    else
                        this._pixCoordSystem = new userPixCoordSystem();
                    return _pixCoordSystem;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            set
            {
                this._pixCoordSystem = value;
            }
        }

        [DisplayName("点对象")]
        [DescriptionAttribute("输出属性")]
        public userWcsCircleSector WcsCircleSector { get { return _wcsCircleSector; } set { this._wcsCircleSector = value; } }



        /// <summary>
        /// 这里应该使用采集源而不是相机名
        /// </summary>
        /// <param name="camName"></param>
        /// <param name="coordSystem"></param>
        public ManualCircleSectorMeasure(string acqSourceName, IFunction coordSystem)
        {
            this._AcqSourceName = acqSourceName;
            if (this._AcqSourceName != null)
            {
                if (AcqSourceManage.Instance.GetAcqSource(this._AcqSourceName) != null)
                {
                    this.CommandParam = new MoveCommandParam(AcqSourceManage.Instance.GetAcqSource(this._AcqSourceName).MoveAxisName, GlobalVariable.pConfig.MoveSpeed);
                    this.CommandParam.AxisParam = new CoordSysAxisPosParam(AcqSourceManage.Instance.GetAcqSource(this._AcqSourceName).CoordSysName); // 初始化点位
                }
                else
                {
                    this.CommandParam = new MoveCommandParam(enAxisName.XY轴, GlobalVariable.pConfig.MoveSpeed);
                    this.CommandParam.AxisParam = new CoordSysAxisPosParam(enCoordSysName.CoordSys_0); // 初始化点位
                }
            }
            if (LightConnectManage.CurrentLight != null)
            {
                foreach (var item in LightConnectManage.CurrentLight.LightParamList)
                {
                    this.LightParam.Add(item.Clone());
                }
            }
            /////////
            this.ResultInfo = new BindingList<OcrResultInfo>();
        }

        public ManualCircleSectorMeasure(IFunction imageSource, IFunction coordSystem)
        {
            if (this.GetDefautPropertyName(imageSource) != null)
            {
                this.RefSource1.Add(this.GetDefautPropertyName(imageSource), imageSource);
            }
            if (this.GetDefautPropertyName(coordSystem) != null)
            {
                this.RefSource2.Add(this.GetDefautPropertyName(coordSystem), coordSystem);
            }
            /////////
            this.ResultInfo = new BindingList<OcrResultInfo>();
        }
        public ManualCircleSectorMeasure()
        {
            this.ResultInfo = new BindingList<OcrResultInfo>();
        }



        #region 实现接口
        public OperateResult Execute(params object[] param)
        {
            this.Result.Succss = false;
            try
            {
                this._isAcqImage = true;  
                string index = "";
                this._pixCoordSystem = null; // 判断传入的参数中是否包含坐标系，如果包含则使用该坐标系
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                userPixCoordSystem offsetCoordSys = new userPixCoordSystem();
                if (param != null)
                {
                    foreach (var item in param)
                    {
                        if (item == null) continue;
                        switch (item.GetType().Name)
                        {
                            case nameof(userWcsCircleSector):
                                this._findCrossPoint.CircleSectorWcsPosition = (userWcsCircleSector)item;
                                break;
                            case nameof(userPixCircleSector):
                                this._findCrossPoint.CircleSectorPixPosition = (userPixCircleSector)item;
                                break;
                            case nameof(userPixCoordSystem):
                                offsetCoordSys = (userPixCoordSystem)item;
                                break;
                            case nameof(String):
                                if (item.ToString().Split('=').Length > 0)
                                    index = item.ToString().Split('=').Last();
                                break;
                        }
                    }
                }
                if (this._pixCoordSystem == null)
                    this.Result.Succss = this._findCrossPoint.FindCrossPointMethod(this.ImageData, this.PixCoordSystem, offsetCoordSys);
                else
                    this.Result.Succss = this._findCrossPoint.FindCrossPointMethod(this.ImageData, this._pixCoordSystem, offsetCoordSys);
                this._wcsCircleSector = this._findCrossPoint.FitCircleSector.Clone();
                ///////////////////////////////////////////////
                this.CreateResultInfo(13);
                userWcsCircleSector camPoint = this._findCrossPoint.FitCircleSector.GetWcsCamCircleSector();
                stopwatch.Stop();         
                ((BindingList<OcrResultInfo>)this.ResultInfo)[0].SetValue(this.name, "X", $"this._wcsCircleSector.X");
                ((BindingList<OcrResultInfo>)this.ResultInfo)[1].SetValue(this.name, "Y", $"this._wcsCircleSector.Y");
                ((BindingList<OcrResultInfo>)this.ResultInfo)[2].SetValue(this.name, "Z", $"this._wcsCircleSector.Z");
                ((BindingList<OcrResultInfo>)this.ResultInfo)[3].SetValue(this.name, "Cam_X", $" camPoint.X");
                ((BindingList<OcrResultInfo>)this.ResultInfo)[4].SetValue(this.name, "Cam_Y", $" camPoint.Y");
                ///////////////////////////////////////////////
                ((BindingList<OcrResultInfo>)this.ResultInfo)[5].SetValue(this.name, "Grab_X", $"this._wcsCircleSector.Grab_X");
                ((BindingList<OcrResultInfo>)this.ResultInfo)[6].SetValue(this.name, "Grab_Y", $"this._wcsCircleSector.Grab_Y");
                ((BindingList<OcrResultInfo>)this.ResultInfo)[7].SetValue(this.name, "Grab_Z", $"this._wcsCircleSector.Grab_Z");
                ((BindingList<OcrResultInfo>)this.ResultInfo)[8].SetValue(this.name, "Grab_Theta", $"this._wcsCircleSector.Grab_Theta");
                ((BindingList<OcrResultInfo>)this.ResultInfo)[9].SetValue(this.name, "Grab_U", $"this._wcsCircleSector.Grab_U");
                ((BindingList<OcrResultInfo>)this.ResultInfo)[10].SetValue(this.name, "Grab_V", $"this._wcsCircleSector.Grab_V");
                if (this.Result.Succss)
                    ((BindingList<OcrResultInfo>)this.ResultInfo)[11].SetValue(this.name, "执行结果", "1");
                else
                    ((BindingList<OcrResultInfo>)this.ResultInfo)[11].SetValue(this.name, "执行结果", "0");
                ((BindingList<OcrResultInfo>)this.ResultInfo)[12].SetValue(this.name, "Time(ms)", $"stopwatch.ElapsedMilliseconds");
                /////////
                LoggerHelper.Info(this.name + ":" + this._findCrossPoint.FitCircleSector.ToString(), this._wcsCircleSector?.CamName);
                if (this._isUpdateImage) // 只有在采集时才更新图像
                    OnExcuteCompleted(this._imageData?.CamName, this._imageData?.ViewWindow, this.name, this._imageData);
                OnExcuteCompleted(this._wcsCircleSector?.CamName, this._wcsCircleSector?.ViewWindow, this.name, this._wcsCircleSector);
            }
            catch (Exception ex)
            {
                this.Result.Succss = false;
                LoggerHelper.Error(this.name + "执行报错" + ex, this._wcsCircleSector?.CamName);
            }
            finally
            {
                this._isAcqImage = false;
                // 更改UI字体　
                UpdataNodeElementStyle(param, this.Result.Succss);
            }
            if (this.Result.Succss)
                LoggerHelper.Info(this.name + "->执行成功", this._wcsCircleSector?.CamName);
            else
                LoggerHelper.Error(this.name + "->执行失败", this._wcsCircleSector?.CamName);

            return this.Result;
        }
        public object GetPropertyValues(string propertyName)
        {
            switch (propertyName)
            {
                case "名称":
                case nameof(this.Name):
                    return this.name;
                case nameof(this.ImageData):
                    return this._imageData;
                case nameof(this.ResultInfo):
                    return this.ResultInfo;
                default:
                case nameof(this.FindCircleSector):
                    return this._findCrossPoint.FitCircleSector;
            }
        }
        public bool SetPropertyValues(string propertyName, params object[] value)
        {
            HalconLibrary ha = new HalconLibrary();
            switch (propertyName)
            {
                case "名称":
                    this.name = value[0].ToString();
                    return true;
                case "move":
                case "Move":
                    enCoordSysName coordSysName = AcqSourceManage.Instance.GetAcqSource(this._AcqSourceName).CoordSysName;
                    CommunicationConfigParamManger.Instance.WriteValue(coordSysName, enCommunicationCommand.Compensation_X, this.CommandParam.AxisParam.X);
                    CommunicationConfigParamManger.Instance.WriteValue(coordSysName, enCommunicationCommand.Compensation_Y, this.CommandParam.AxisParam.Y);
                    CommunicationConfigParamManger.Instance.WriteValue(coordSysName, enCommunicationCommand.FunctionNoToPlc, "Move");
                    CommunicationConfigParamManger.Instance.WriteValue(coordSysName, enCommunicationCommand.TriggerToPlc, 1);
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
            catch
            {
                LoggerHelper.Error(this.name + "->删除该对象报错");
            }
        }
        public void Read(string path)
        {

        }
        public void Save(string path)
        {

        }

        #endregion



    }
}

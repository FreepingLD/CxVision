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
using System.Runtime.InteropServices;
using System.Data;
using Light;
using System.Diagnostics;

namespace FunctionBlock
{
    [Serializable]
    [DefaultProperty(nameof(WcsLine))]
    public class LineMeasure : BaseFunction, IFunction
    {
        [NonSerialized]
        private ImageDataClass _imageData;
        private string _AcqSourceName;
        private FindLine _findLine = new FindLine();
        private userWcsCoordSystem _wcsCoordSystem;
        private userPixCoordSystem _pixCoordSystem;
        private MoveCommandParam CommandParam;


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
                    }
                    else
                    {
                        if (this._AcqSourceName != null)
                        {
                            MoveCommandParam affineCommandParam = CommandParam.Affine2DCommandParam(this.WcsCoordSystem);
                            this._imageData = AcqSourceManage.Instance.GetAcqSource(this._AcqSourceName)?.AcqImageData(affineCommandParam, this.LightParam)[enDataItem.Image] as ImageDataClass;
                            CoordSysAxisParam currentPosition = new CoordSysAxisParam(AcqSourceManage.Instance.GetAcqSource(this._AcqSourceName).CoordSysName);
                            if (this._imageData != null)
                            {
                                this._imageData.Grab_X = currentPosition.X;
                                this._imageData.Grab_Y = currentPosition.Y;
                                this._imageData.Grab_Z = currentPosition.Z;
                                this._imageData.Grab_Theta = currentPosition.Theta;
                            }
                        }
                        //else
                        //    this._imageData = null;
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

        public FindLine FindLine { get => _findLine; set => _findLine = value; }


        [DisplayName("直线对象")]
        [DescriptionAttribute("输出属性")]
        public userWcsLine WcsLine { get; set; }

        [DisplayName("采集源")]
        public string AcqSourceName { get => _AcqSourceName; set => _AcqSourceName = value; }





        public LineMeasure()
        {
            InitBindingTable();
        }
        public LineMeasure(string acqSourceName, IFunction coordSystem)
        {
            this._AcqSourceName = acqSourceName;
            if (this._AcqSourceName != null)
            {
                this.CommandParam = new MoveCommandParam(AcqSourceManage.Instance.GetAcqSource(this._AcqSourceName).MoveAxisName, GlobalVariable.pConfig.MoveSpeed);
                this.CommandParam.AxisParam = new CoordSysAxisParam(AcqSourceManage.Instance.GetAcqSource(this._AcqSourceName).CoordSysName); // 初始化点位
            }
            if (LightConnectManage.CurrentLight != null)
            {
                foreach (var item in LightConnectManage.CurrentLight.LightParamList)
                {
                    this.LightParam.Add(item.Clone());
                }
            }
            /////////////////////////////////////////////////
            InitBindingTable();
        }
        public LineMeasure(IFunction imageSource, IFunction coordSystem)
        {
            ////////////////////////////////
            InitBindingTable();
        }
        private void InitBindingTable()
        {
            if (this.ResultInfo == null)
            {
                this.ResultInfo = new BindingList<MeasureResultInfo>();
                ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name, "X1", 0));
                ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name, "Y1", 0));
                ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name, "Z1", 0));
                ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name, "X2", 0));
                ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name, "Y2", 0));
                ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name, "Z2", 0));
                ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name, "Cam_X1", 0));
                ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name, "Cam_Y1", 0));
                ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name, "Cam_X2", 0));
                ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name, "Cam_Y2", 0));
                //////////////拍照坐标
                ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name, "Grab_X", 0));
                ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name, "Grab_Y", 0));
                ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name, "Grab_Z", 0));
                ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name, "Grab_Theat", 0));
                ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name, "Grab_U", 0));
                ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name, "Grab_V", 0));
                ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name, "Angle", 0));
                ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name, "执行结果", 0));
                ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name, "Time", 0));
                //
                //((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name, "Bolb结果", 0));
            }
        }


        #region 实现接口
        public OperateResult Execute(params object[] param)
        {
            this.Result.Succss = false;
            try
            {
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
                            case nameof(userWcsLine):
                                this._findLine.LineWcsPosition = (userWcsLine)item;
                                break;
                            case nameof(userPixLine):
                                this._findLine.LinePixPosition = (userPixLine)item;
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
                    this.Result.Succss = this._findLine.FindLineMethod(this.ImageData, this.PixCoordSystem, offsetCoordSys);
                else
                    this.Result.Succss = this._findLine.FindLineMethod(this.ImageData, this._pixCoordSystem, offsetCoordSys);
                this.WcsLine = this._findLine.FitLine;
                this.WcsLine.CamName = this._imageData.CamName;
                this.WcsLine.ViewWindow = this._imageData.ViewWindow;
                stopwatch.Stop();
                ///////////////////////////////////////////
                this.InitBindingTable();
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[0].SetValue(this.name + index, "X1", this._findLine.FitLine.X1);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[1].SetValue(this.name + index, "Y1", this._findLine.FitLine.Y1);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[2].SetValue(this.name + index, "Z1", this._findLine.FitLine.Z1);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[3].SetValue(this.name + index, "X2", this._findLine.FitLine.X2);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[4].SetValue(this.name + index, "Y2", this._findLine.FitLine.Y2);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[5].SetValue(this.name + index, "Z2", this._findLine.FitLine.Z2);
                //////////
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[6].SetValue(this.name + index, "Cam_X1", this._findLine.FitLine.X1 - this._imageData.Grab_X);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[7].SetValue(this.name + index, "Cam_Y1", this._findLine.FitLine.Y1 - this._imageData.Grab_Y);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[8].SetValue(this.name + index, "Cam_X2", this._findLine.FitLine.X2 - this._imageData.Grab_X);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[9].SetValue(this.name + index, "Cam_Y2", this._findLine.FitLine.Y2 - this._imageData.Grab_Y);
                /////////////////////////////////////////
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[10].SetValue(this.name + index, "Grab_X", this._imageData.Grab_X);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[11].SetValue(this.name + index, "Grab_Y", this._imageData.Grab_Y);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[12].SetValue(this.name + index, "Grab_Z", this._imageData.Grab_Z);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[13].SetValue(this.name + index, "Grab_Theat", this._imageData.Grab_Theta);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[14].SetValue(this.name + index, "Grab_U", this._imageData.Grab_U);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[15].SetValue(this.name + index, "Grab_V", this._imageData.Grab_V);
                if (((BindingList<MeasureResultInfo>)this.ResultInfo).Count > 16)
                    ((BindingList<MeasureResultInfo>)this.ResultInfo)[16].SetValue(this.name + index, "Angle", this.WcsLine.Angle);
                else
                    ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo());
                if (this.Result.Succss)
                    ((BindingList<MeasureResultInfo>)this.ResultInfo)[17].SetValue(this.name, "执行结果", 1);
                else
                    ((BindingList<MeasureResultInfo>)this.ResultInfo)[17].SetValue(this.name, "执行结果", 0);
                if (((BindingList<MeasureResultInfo>)this.ResultInfo).Count > 18)
                    ((BindingList<MeasureResultInfo>)this.ResultInfo)[18].SetValue(this.name + index, "Time", stopwatch.ElapsedMilliseconds);
                else
                    ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo());
                /////////
                LoggerHelper.Info(this.name + ":" + this._findLine.FitLine.ToString());
                OnExcuteCompleted(this.WcsLine.CamName, this.WcsLine?.ViewWindow, this.name + index, this.WcsLine);
            }
            catch (Exception ex)
            {
                LoggerHelper.Error(this.name + "->执行报错" + ex);
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
                case nameof(Name):
                    return this.name;
                case nameof(this.ResultInfo):
                    return this.ResultInfo;
                case nameof(WcsLine):
                default:
                    return this.WcsLine;
            }
        }
        public bool SetPropertyValues(string propertyName, params object[] value)
        {
            HalconLibrary ha = new HalconLibrary();
            switch (propertyName)
            {
                case "名称":
                case nameof(Name):
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

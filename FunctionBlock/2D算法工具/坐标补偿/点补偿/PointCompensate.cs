using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlgorithmsLibrary;
using HalconDotNet;
using System.Data;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Common;
using System.Diagnostics;
using MotionControlCard;
using System.Windows.Media.Animation;

namespace FunctionBlock
{
    [Serializable]
    [DefaultProperty(nameof(WcsPoint))]
    public class PointCompensate : BaseFunction, IFunction
    {
        private userWcsPoint _wcsPoint;
        private userWcsPoint _WcsPoint1;


        [DisplayName("输出点")]
        [DescriptionAttribute("输出属性")]
        public userWcsPoint WcsPoint { get => _wcsPoint; set => _wcsPoint = value; }


        [DisplayName("输入点")]
        [DescriptionAttribute("输入属性1")]
        public userWcsPoint WcsPoint1
        {
            get
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
                                case nameof(userWcsPoint):
                                    this._WcsPoint1 = item as userWcsPoint;
                                    break;
                                case nameof(userWcsVector):
                                    userWcsVector wcsVector = item as userWcsVector;
                                    this._WcsPoint1 = new userWcsPoint(wcsVector.X, wcsVector.Y, wcsVector.Z, wcsVector.CamParams);
                                    this._WcsPoint1.Grab_x = wcsVector.Grab_x;
                                    this._WcsPoint1.Grab_y = wcsVector.Grab_y;
                                    this._WcsPoint1.Grab_theta = wcsVector.Grab_theta;
                                    break;
                                case nameof(userPixPoint):
                                    userPixPoint pixPoint = item as userPixPoint;
                                    this._WcsPoint1 = pixPoint.GetWcsPoint();
                                    break;
                                case nameof(userWcsRectangle2):
                                    userWcsRectangle2 wcsRect2 = item as userWcsRectangle2;
                                    this._WcsPoint1 = new userWcsPoint(wcsRect2.X, wcsRect2.Y, 0, wcsRect2.CamParams);
                                    this._WcsPoint1.Grab_x = wcsRect2.Grab_x;
                                    this._WcsPoint1.Grab_y = wcsRect2.Grab_y;
                                    this._WcsPoint1.Grab_theta = wcsRect2.Grab_theta;
                                    break;
                                case nameof(userWcsCircle):
                                    userWcsCircle wcsCircle = item as userWcsCircle;
                                    this._WcsPoint1 = new userWcsPoint(wcsCircle.X, wcsCircle.Y, 0, wcsCircle.CamParams);
                                    this._WcsPoint1.Grab_x = wcsCircle.Grab_x;
                                    this._WcsPoint1.Grab_y = wcsCircle.Grab_y;
                                    this._WcsPoint1.Grab_theta = wcsCircle.Grab_theta;
                                    break;
                                case nameof(userWcsCircleSector):
                                    userWcsCircleSector wcsCircleSector = item as userWcsCircleSector;
                                    this._WcsPoint1 = new userWcsPoint(wcsCircleSector.X, wcsCircleSector.Y, wcsCircleSector.Z, wcsCircleSector.CamParams);
                                    this._WcsPoint1.Grab_x = wcsCircleSector.Grab_x;
                                    this._WcsPoint1.Grab_y = wcsCircleSector.Grab_y;
                                    this._WcsPoint1.Grab_theta = wcsCircleSector.Grab_theta;
                                    break;
                                case nameof(userWcsEllipse):
                                    userWcsEllipse wcsEllipse = item as userWcsEllipse;
                                    this._WcsPoint1 = new userWcsPoint(wcsEllipse.X, wcsEllipse.Y, wcsEllipse.Z, wcsEllipse.CamParams);
                                    this._WcsPoint1.Grab_x = wcsEllipse.Grab_x;
                                    this._WcsPoint1.Grab_y = wcsEllipse.Grab_y;
                                    this._WcsPoint1.Grab_theta = wcsEllipse.Grab_theta;
                                    break;
                                case nameof(userWcsEllipseSector):
                                    userWcsEllipseSector wcsEllipseSector = item as userWcsEllipseSector;
                                    this._WcsPoint1 = new userWcsPoint(wcsEllipseSector.X, wcsEllipseSector.Y, wcsEllipseSector.Z, wcsEllipseSector.CamParams);
                                    this._WcsPoint1.Grab_x = wcsEllipseSector.Grab_x;
                                    this._WcsPoint1.Grab_y = wcsEllipseSector.Grab_y;
                                    this._WcsPoint1.Grab_theta = wcsEllipseSector.Grab_theta;
                                    break;
                            }
                        }
                    }
                }
                else
                    this._WcsPoint1 = null;
                return this._WcsPoint1;
            }
            set
            {
                this._WcsPoint1 = value;
            }
        }


        public CompensationData Param { get; set; }


        public PointCompensate()
        {
            this.Param = new CompensationData();
            this.ResultInfo = new BindingList<MeasureResultInfo>();
        }




        #region 实现接口

        public OperateResult Execute(params object[] param)
        {
            int grabNo = 0;
            this.Result.Succss = false;
            this.Result.ErrorMessage = "";
            try
            {
                string index = "";
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                if (param != null)
                {
                    foreach (var item in param)
                    {
                        if (item == null) continue;
                        switch (item.GetType().Name)
                        {
                            case nameof(String):
                                if (item.ToString().Split('=').Length > 0)
                                    index = item.ToString().Split('=').Last();
                                break;
                        }
                    }
                }
                if (this.WcsPoint1 != null)
                {
                    this.Result.Succss = true;
                    this._wcsPoint = this._WcsPoint1.Clone();
                    string _grabNo = CommunicationConfigParamManger.Instance.ReadValue(this._WcsPoint1.CamParams.CaliParam.CoordSysName, enCommunicationCommand.GrabNo)?.ToString();                 
                    int.TryParse(_grabNo, out grabNo);
                    switch (grabNo)
                    {
                        case 1:
                            this._wcsPoint.X += this.Param.X1;
                            this._wcsPoint.Y += this.Param.Y1;
                            this._wcsPoint.Theta += this.Param.Angle1;
                            break;
                        case 2:
                            this._wcsPoint.X += this.Param.X2;
                            this._wcsPoint.Y += this.Param.Y2;
                            this._wcsPoint.Theta += this.Param.Angle2;
                            break;
                        case 3:
                            this._wcsPoint.X += this.Param.X3;
                            this._wcsPoint.Y += this.Param.Y3;
                            this._wcsPoint.Theta += this.Param.Angle3;
                            break;
                        case 4:
                            this._wcsPoint.X += this.Param.X4;
                            this._wcsPoint.Y += this.Param.Y4;
                            this._wcsPoint.Theta += this.Param.Angle4;
                            break;
                        case 5:
                            this._wcsPoint.X += this.Param.X5;
                            this._wcsPoint.Y += this.Param.Y5;
                            this._wcsPoint.Theta += this.Param.Angle5;
                            break;
                        case 6:
                            this._wcsPoint.X += this.Param.X6;
                            this._wcsPoint.Y += this.Param.Y6;
                            this._wcsPoint.Theta += this.Param.Angle6;
                            break;
                        case 7:
                            this._wcsPoint.X += this.Param.X7;
                            this._wcsPoint.Y += this.Param.Y7;
                            this._wcsPoint.Theta += this.Param.Angle7;
                            break;
                        case 8:
                            this._wcsPoint.X += this.Param.X8;
                            this._wcsPoint.Y += this.Param.Y8;
                            this._wcsPoint.Theta += this.Param.Angle8;
                            break;
                    }
                }
                else
                {
                    this.Result.Succss = false;
                    this._wcsPoint = new userWcsPoint();
                }
                stopwatch.Stop();
                this.CreateResultInfo(9);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[0].SetValue(this.name, "X", this._wcsPoint.X);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[1].SetValue(this.name, "Y", this._wcsPoint.Y);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[2].SetValue(this.name, "Z", this._wcsPoint.Z);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[3].SetValue(this.name, "Theta", this._wcsPoint.Z);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[4].SetValue(this.name, "GrabNo", grabNo);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[5].SetValue(this.name, "Grab_x", this._wcsPoint.Grab_x);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[6].SetValue(this.name, "Grab_y", this._wcsPoint.Grab_y);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[7].SetValue(this.name, "Grab_theta", this._wcsPoint.Grab_theta);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[8].SetValue(this.name, "Time(ms)", stopwatch.ElapsedMilliseconds);
                OnExcuteCompleted(this._wcsPoint.CamName, this.name, this._wcsPoint);
            }
            catch (Exception ex)
            {
                LoggerHelper.Error(this.name + "->执行报错", ex);
                this.Result.Succss = false;
            }
            if (this.Result.Succss)
                LoggerHelper.Info(this.name + "->执行成功:" + this._wcsPoint.ToString());
            else
                LoggerHelper.Error(this.name + "->执行失败:" + this._wcsPoint.ToString());
            // 更改UI字体　
            UpdataNodeElementStyle(param, this.Result.Succss);
            return this.Result;
        }
        public object GetPropertyValues(string propertyName)
        {
            switch (propertyName)
            {
                case "名称":
                default:
                case nameof(Name):
                    return this.name;
                case nameof(WcsPoint):
                    return this._wcsPoint;
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
                default:
                    return true;
            }
        }
        public void ReleaseHandle()
        {
            try
            {
                if (this._wcsPoint != null)
                {
                    OnItemDeleteEvent(this, this.name);
                }
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

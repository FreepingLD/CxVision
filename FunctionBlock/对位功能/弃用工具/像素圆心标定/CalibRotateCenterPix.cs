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
    [DefaultProperty(nameof(PixCircle))]
    /// <summary>
    /// 激光取点采集的数据类
    /// </summary>
    public class CalibRotateCenterPix : BaseFunction, IFunction
    {
        public userPixPoint[] _sourcePoint;
        public userPixPoint[] _targetPoint;
        private userPixCircle _pixCircle;

        [DisplayName("圆")]
        [DescriptionAttribute("输出属性")]
        public userPixCircle PixCircle { get => this._pixCircle; set => this._pixCircle = value; }


        [DisplayName("输入点1")]
        [DescriptionAttribute("输入属性1")]
        public userPixPoint[] SourcePoint
        {
            get
            {
                if (this.RefSource1.Count > 0)
                {
                    object[] oo = this.GetPropertyValue(this.RefSource1);
                    List<userPixPoint> list = new List<userPixPoint>();
                    foreach (var item in oo)
                    {
                        switch (item.GetType().Name)
                        {
                            case nameof(userWcsPoint):
                                userWcsPoint wcsPoint = item as userWcsPoint;
                                list.Add(wcsPoint.GetPixPoint());
                                break;
                            case nameof(userWcsCircle):
                                userWcsCircle wcsCircle = item as userWcsCircle;
                                userPixCircle pixCircle = wcsCircle.GetPixCircle();
                                list.Add(new userPixPoint(pixCircle.Row, pixCircle.Col, pixCircle.Grab_x, pixCircle.Grab_y, pixCircle.Grab_theta, pixCircle.CamParams));
                                break;
                            case nameof(userWcsCircleSector):
                                userWcsCircleSector wcsCircleSector = item as userWcsCircleSector;
                                userPixCircleSector pixCircleSector = wcsCircleSector.GetPixCircleSector();
                                list.Add(new userPixPoint(pixCircleSector.Row, pixCircleSector.Col, pixCircleSector.Grab_x, pixCircleSector.Grab_y, pixCircleSector.Grab_theta, pixCircleSector.CamParams));
                                break;
                            case nameof(userWcsRectangle2):
                                userWcsRectangle2 wcsRect2 = item as userWcsRectangle2;
                                userPixRectangle2 pixRect2 = wcsRect2.GetPixRectangle2();
                                list.Add(new userPixPoint(pixRect2.Row, pixRect2.Col, pixRect2.Grab_x, pixRect2.Grab_y, pixRect2.Grab_theta, pixRect2.CamParams));
                                break;
                            case nameof(userWcsCoordSystem):
                                userWcsCoordSystem wcsCoord = item as userWcsCoordSystem;
                                userPixCoordSystem pixCoordSystem = wcsCoord.GetPixCoordSystem();
                                list.Add(new userPixPoint(pixCoordSystem.CurrentPoint.Row, pixCoordSystem.CurrentPoint.Col, pixCoordSystem.CurrentPoint.Grab_x, pixCoordSystem.CurrentPoint.Grab_y, pixCoordSystem.CurrentPoint.Grab_theta, pixCoordSystem.CurrentPoint.CamParams));
                                break;
                            case nameof(userWcsVector):
                                userWcsVector wcsVector = item as userWcsVector;
                                userPixVector pixVector = new userPixVector(); 
                                list.Add(new userPixPoint(pixVector.Row, pixVector.Col, pixVector.Grab_x, pixVector.Grab_y, pixVector.Grab_theta, pixVector.CamParams));
                                break;
                            default:
                                throw new ArgumentException("参数类型错误");
                        }
                    }
                    this._sourcePoint = list.ToArray();
                    list.Clear();
                }
                else
                    this._sourcePoint = new userPixPoint[0];
                return this._sourcePoint;
            }
            set
            {
                this._sourcePoint = value;
            }
        }

        [DisplayName("输入点2")]
        [DescriptionAttribute("输入属性2")]
        public userPixPoint[] TargetPoint
        {
            get
            {
                if (this.RefSource2.Count > 0)
                {
                    object[] oo = this.GetPropertyValue(this.RefSource2);
                    List<userPixPoint> list = new List<userPixPoint>();
                    foreach (var item in oo)
                    {
                        switch (item.GetType().Name)
                        {
                            case nameof(userWcsPoint):
                                userWcsPoint wcsPoint = item as userWcsPoint;
                                list.Add(wcsPoint.GetPixPoint());
                                break;
                            case nameof(userWcsCircle):
                                userWcsCircle wcsCircle = item as userWcsCircle;
                                userPixCircle pixCircle = wcsCircle.GetPixCircle();
                                list.Add(new userPixPoint(pixCircle.Row, pixCircle.Col, pixCircle.Grab_x, pixCircle.Grab_y, pixCircle.Grab_theta, pixCircle.CamParams));
                                break;
                            case nameof(userWcsCircleSector):
                                userWcsCircleSector wcsCircleSector = item as userWcsCircleSector;
                                userPixCircleSector pixCircleSector = wcsCircleSector.GetPixCircleSector();
                                list.Add(new userPixPoint(pixCircleSector.Row, pixCircleSector.Col, pixCircleSector.Grab_x, pixCircleSector.Grab_y, pixCircleSector.Grab_theta, pixCircleSector.CamParams));
                                break;
                            case nameof(userWcsRectangle2):
                                userWcsRectangle2 wcsRect2 = item as userWcsRectangle2;
                                userPixRectangle2 pixRect2 = wcsRect2.GetPixRectangle2();
                                list.Add(new userPixPoint(pixRect2.Row, pixRect2.Col, pixRect2.Grab_x, pixRect2.Grab_y, pixRect2.Grab_theta, pixRect2.CamParams));
                                break;
                            case nameof(userWcsCoordSystem):
                                userWcsCoordSystem wcsCoord = item as userWcsCoordSystem;
                                userPixCoordSystem pixCoordSystem = wcsCoord.GetPixCoordSystem();
                                list.Add(new userPixPoint(pixCoordSystem.CurrentPoint.Row, pixCoordSystem.CurrentPoint.Col, pixCoordSystem.CurrentPoint.Grab_x, pixCoordSystem.CurrentPoint.Grab_y, pixCoordSystem.CurrentPoint.Grab_theta, pixCoordSystem.CurrentPoint.CamParams));
                                break;
                            case nameof(userWcsVector):
                                userWcsVector wcsVector = item as userWcsVector;
                                userPixVector pixVector = new userPixVector();
                                list.Add(new userPixPoint(pixVector.Row, pixVector.Col, pixVector.Grab_x, pixVector.Grab_y, pixVector.Grab_theta, pixVector.CamParams));
                                break;
                            default:
                                throw new ArgumentException("参数类型错误");
                        }
                    }
                    this._targetPoint = list.ToArray();
                    list.Clear();
                }
                else
                    this._targetPoint = new userPixPoint[0];
                return this._targetPoint;
            }
            set
            {
                this._targetPoint = value;
            }
        }

        public CalibRotateCenterPix()
        {
            this.ResultInfo = new BindingList<MeasureResultInfo>();
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name, "Row", 0));
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name, "Col", 0));
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name, "旋转角度", 0));
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name, "Time(ms)", 0));
        }



        #region 实现接口,各自实现自己的逻辑，就可实现通用的逻辑
        public OperateResult Execute(params object[] param)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            this.Result.Succss = false;
            try
            {
                this.Result.Succss = CalculateMethod.CalculatePix(this.SourcePoint, this.TargetPoint, out this._pixCircle);
                stopwatch.Stop();
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[0].SetValue(this.name, "Row", this._pixCircle.Row);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[1].SetValue(this.name, "Col", this._pixCircle.Col);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[2].SetValue(this.name, "旋转角度", this._pixCircle.Grab_theta);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[3].SetValue(this.name, "Time(ms)", stopwatch.ElapsedMilliseconds);
                // 使用发命令的方式来更新视图  
                if (this.Result.Succss)
                    LoggerHelper.Info(this.name + "圆心标定成功->" + this._pixCircle.ToString());
                else
                    LoggerHelper.Error(this.name + "圆心标定失败");
            }
            catch (Exception ex)
            {
                LoggerHelper.Error(this.name + "圆心标定：" + "报错" + ex);
                this.Result.Succss = false;
            }
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
                default:
                    return this.name;
            }
        }

        public bool SetPropertyValues(string propertyName, params object[] value)
        {
            string name = "";
            if (value != null)
                name = value[0].ToString();
            switch (propertyName)
            {
                default:
                case "名称":
                case nameof(this.Name):
                    this.name = value[0].ToString();
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

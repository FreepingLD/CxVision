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
    [Serializable]
    [DefaultProperty(nameof(Line))]
    public class NPointsFitLine : BaseFunction, IFunction
    {
        private userWcsLine _Line;
        private userWcsPoint[] _WcsPoint;

        public LineFitParam FitParam
        {
            get;
            set;
        }


        [DisplayName("输出直线对象")]
        [DescriptionAttribute("输出属性")]
        public userWcsLine Line { get => _Line; set => _Line = value; }

        [DisplayName("输入点对象")]
        [DescriptionAttribute("输入属性1")]
        public userWcsPoint[] WcsPoint
        {
            get
            {
                try
                {
                    if (this.RefSource1.Count > 0)
                    {
                        var oo = this.GetPropertyValue(this.RefSource1);
                        List<userWcsPoint> listPoint = new List<userWcsPoint>();
                        foreach (var item in oo)
                        {
                            switch (item.GetType().Name)
                            {
                                case nameof(userWcsPoint):
                                    listPoint.Add((userWcsPoint)item);
                                    break;
                                case nameof(userWcsCircle):
                                    userWcsCircle wcsCircle = item as userWcsCircle;
                                    userWcsPoint wcsPoint = new userWcsPoint(wcsCircle.X, wcsCircle.Y, wcsCircle.Z, wcsCircle.CamParams);
                                    wcsPoint.Grab_x = wcsCircle.Grab_x;
                                    wcsPoint.Grab_y = wcsCircle.Grab_y;
                                    wcsPoint.Grab_z = wcsCircle.Grab_z;
                                    wcsPoint.Grab_theta = wcsCircle.Grab_theta;
                                    wcsPoint.CamName = wcsCircle.CamName;
                                    wcsPoint.ViewWindow = wcsCircle.ViewWindow;
                                    listPoint.Add(wcsPoint);
                                    break;
                                case nameof(userWcsCircleSector):
                                    userWcsCircleSector wcsCircleSector = item as userWcsCircleSector;
                                    wcsPoint = new userWcsPoint(wcsCircleSector.X, wcsCircleSector.Y, wcsCircleSector.Z, wcsCircleSector.CamParams);
                                    wcsPoint.Grab_x = wcsCircleSector.Grab_x;
                                    wcsPoint.Grab_y = wcsCircleSector.Grab_y;
                                    wcsPoint.Grab_z = wcsCircleSector.Grab_z;
                                    wcsPoint.Grab_theta = wcsCircleSector.Grab_theta;
                                    wcsPoint.CamName = wcsCircleSector.CamName;
                                    wcsPoint.ViewWindow = wcsCircleSector.ViewWindow;
                                    listPoint.Add(wcsPoint);
                                    break;
                                case nameof(userWcsRectangle2):
                                    userWcsRectangle2 wcsRectangle2 = item as userWcsRectangle2;
                                    wcsPoint = new userWcsPoint(wcsRectangle2.X, wcsRectangle2.Y, wcsRectangle2.Z, wcsRectangle2.CamParams);
                                    wcsPoint.Grab_x = wcsRectangle2.Grab_x;
                                    wcsPoint.Grab_y = wcsRectangle2.Grab_y;
                                    wcsPoint.Grab_z = wcsRectangle2.Grab_z;
                                    wcsPoint.Grab_theta = wcsRectangle2.Grab_theta;
                                    wcsPoint.CamName = wcsRectangle2.CamName;
                                    wcsPoint.ViewWindow = wcsRectangle2.ViewWindow;
                                    listPoint.Add(wcsPoint);
                                    break;
                                case nameof(userWcsEllipse):
                                    userWcsEllipse wcsEllipse = item as userWcsEllipse;
                                    wcsPoint = new userWcsPoint(wcsEllipse.X, wcsEllipse.Y, wcsEllipse.Z, wcsEllipse.CamParams);
                                    wcsPoint.Grab_x = wcsEllipse.Grab_x;
                                    wcsPoint.Grab_y = wcsEllipse.Grab_y;
                                    wcsPoint.Grab_z = wcsEllipse.Grab_z;
                                    wcsPoint.Grab_theta = wcsEllipse.Grab_theta;
                                    wcsPoint.CamName = wcsEllipse.CamName;
                                    wcsPoint.ViewWindow = wcsEllipse.ViewWindow;
                                    listPoint.Add(wcsPoint);
                                    break;
                                case nameof(userWcsEllipseSector):
                                    userWcsEllipseSector wcsEllipseSector = item as userWcsEllipseSector;
                                    wcsPoint = new userWcsPoint(wcsEllipseSector.X, wcsEllipseSector.Y, wcsEllipseSector.Z, wcsEllipseSector.CamParams);
                                    wcsPoint.Grab_x = wcsEllipseSector.Grab_x;
                                    wcsPoint.Grab_y = wcsEllipseSector.Grab_y;
                                    wcsPoint.Grab_z = wcsEllipseSector.Grab_z;
                                    wcsPoint.Grab_theta = wcsEllipseSector.Grab_theta;
                                    wcsPoint.CamName = wcsEllipseSector.CamName;
                                    wcsPoint.ViewWindow = wcsEllipseSector.ViewWindow;
                                    listPoint.Add(wcsPoint);
                                    break;
                                case nameof(userWcsPolyLine):
                                    userWcsPolyLine wcsPolyLine  = item as userWcsPolyLine;
                                    for (int i = 0; i < wcsPolyLine.X.Count; i++)
                                    {
                                        wcsPoint = new userWcsPoint(wcsPolyLine.X[i], wcsPolyLine.Y[i], wcsPolyLine.Z[i], wcsPolyLine.CamParams);
                                        wcsPoint.Grab_x = wcsPolyLine.Grab_x;
                                        wcsPoint.Grab_y = wcsPolyLine.Grab_y;
                                        wcsPoint.Grab_z = wcsPolyLine.Grab_z;
                                        wcsPoint.Grab_theta = wcsPolyLine.Grab_theta;
                                        wcsPoint.CamName = wcsPolyLine.CamName;
                                        wcsPoint.ViewWindow = wcsPolyLine.ViewWindow;
                                        listPoint.Add(wcsPoint);
                                    }
                                    break;
                            }
                        }
                        this._WcsPoint = listPoint.ToArray();
                        listPoint.Clear();
                    }
                    else
                        this._WcsPoint = null;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return _WcsPoint;
            }
            set { _WcsPoint = value; }
        }
        public NPointsFitLine()
        {
            this.FitParam = new LineFitParam();
            this.ResultInfo = new BindingList<MeasureResultInfo>();
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo());
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo());
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo());
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo());
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo());
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo());
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo());
        }




        #region  实现接口的部分
        public OperateResult Execute(params object[] param)
        {
            this.Result.Succss = false;
            try
            {
                this.Result.Succss = GeometryFitMethod.Instance.NPointFitLine(this.WcsPoint,this.FitParam,out this._Line);
                this.CreateResultInfo(7);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[0].SetValue(this.name, "X1", this._Line.X1);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[1].SetValue(this.name, "Y1", this._Line.Y1);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[2].SetValue(this.name, "Z1", this._Line.Z1);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[3].SetValue(this.name, "X2", this._Line.X2);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[4].SetValue(this.name, "Y2", this._Line.Y2);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[5].SetValue(this.name, "Z2", this._Line.Z2);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[6].SetValue(this.name, "角度", this._Line.Angle);
                OnExcuteCompleted(this.WcsPoint[0].CamName, this.WcsPoint[0].CamParams?.ViewWindow, this.name, this._Line);
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
                case "Name":
                    return this.name;
                case nameof(this.Line):
                    return this._Line; //
                default:
                        return this._Line;
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
                OnItemDeleteEvent(this, this.name);
            }
            catch
            {

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

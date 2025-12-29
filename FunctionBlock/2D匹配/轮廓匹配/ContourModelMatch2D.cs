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
    [DefaultProperty(nameof(WcsCoordSystem))]
    public class ContourModelMatch2D : BaseFunction, IFunction
    {
        private userWcsCoordSystem _wcsCoordSystem;
        private userPixCoordSystem _pixCoordSystem;
        private userWcsPolyLine _wcsPolyLine;
        private userWcsPoint[] _curTrackPoint;
        private userWcsPoint[] _stdTrackPoint;
        private double[] _dataError;

        [DisplayName("输入轨迹")]
        [DescriptionAttribute("输入属性1")]
        public userWcsPoint[] CurTrackPoint
        {
            get
            {
                if (this.RefSource1.Count > 0)
                {
                    object[] oo = this.GetPropertyValue(this.RefSource1);
                    List<userWcsPoint> listPoint = new List<userWcsPoint>();
                    if (oo != null && oo.Length > 0)
                    {
                        foreach (var item in oo)
                        {
                            switch (item.GetType().Name)
                            {
                                case nameof(userWcsPoint):
                                    listPoint.Add(item as userWcsPoint);
                                    break;
                                case nameof(userWcsVector):
                                    userWcsVector wcsVector = item as userWcsVector;
                                    listPoint.Add(new userWcsPoint(wcsVector.X, wcsVector.Y, wcsVector.Z, wcsVector.CamParams));
                                    break;
                                case "userWcsVector[]":
                                    userWcsVector[] wcsVectorA = item as userWcsVector[];
                                    foreach (var item2 in wcsVectorA)
                                    {
                                        listPoint.Add(new userWcsPoint(item2.X, item2.Y, item2.Z, item2.CamParams));
                                    }
                                    break;
                                case "userWcsPoint[]":
                                    listPoint.AddRange(item as userWcsPoint[]);
                                    break;
                                case nameof(userWcsLine):
                                    userWcsLine wcsLine = item as userWcsLine;
                                    listPoint.AddRange(wcsLine.GetFitWcsPoint());
                                    break;
                                case nameof(userWcsCircle):
                                    userWcsCircle wcsCircle = item as userWcsCircle;
                                    listPoint.AddRange(wcsCircle.GetInterpolateWcsPoint(wcsCircle.EdgesPoint_xyz.Length));
                                    break;
                                case nameof(userWcsCircleSector):
                                    userWcsCircleSector wcsCircleSector = item as userWcsCircleSector;
                                    listPoint.AddRange(wcsCircleSector.GetInterpolateWcsPoint(wcsCircleSector.EdgesPoint_xyz.Length));
                                    break;
                                case nameof(userWcsRectangle2):
                                    userWcsRectangle2 wcsRec2 = item as userWcsRectangle2;
                                    double[] Px = new double[] { -1, 1, 1, -1 };
                                    double[] Py = new double[] { 1, 1, -1, -1 };
                                    HTuple Qx = null, Qy = null;
                                    HHomMat2D hHomMat2D = new HHomMat2D();
                                    hHomMat2D.VectorAngleToRigid(0, 0, 0, wcsRec2.X, wcsRec2.Y, wcsRec2.Deg * Math.PI / 180);
                                    Qx = hHomMat2D.AffineTransPoint2d(Px, Py, out Qy);
                                    if (Qx != null)
                                    {
                                        for (int i = 0; i < Qx.Length; i++)
                                        {
                                            listPoint.Add(new userWcsPoint(Qx[i].D, Qy[i].D, wcsRec2.Z, wcsRec2.CamParams));
                                        }
                                    }
                                    break;
                                case nameof(userWcsPolyLine):
                                    userWcsPolyLine wcsPolyLine = item as userWcsPolyLine;
                                    for (int i = 0; i < wcsPolyLine.X.Count; i++)
                                    {
                                        listPoint.Add(new userWcsPoint(wcsPolyLine.X[i], wcsPolyLine.Y[i], 0, wcsPolyLine.CamParams));
                                    }
                                    break;
                                case nameof(userWcsPolygon):
                                    userWcsPolygon wcsPolygon = item as userWcsPolygon;
                                    for (int i = 0; i < wcsPolygon.X.Count; i++)
                                    {
                                        listPoint.Add(new userWcsPoint(wcsPolygon.X[i], wcsPolygon.Y[i], 0, wcsPolygon.CamParams));
                                    }
                                    break;
                            }
                        }
                    }
                    this._curTrackPoint = listPoint.ToArray();
                    listPoint.Clear();
                }
                return this._curTrackPoint;
            }
            set
            {
                this._curTrackPoint = value;
            }
        }

        [DisplayName("模板轨迹")]
        [DescriptionAttribute("输入属性2")]
        public userWcsPoint[] StdTrackPoint
        {
            get
            {
                return this._stdTrackPoint;
            }
            set
            {
                this._stdTrackPoint = value;
            }
        }

        [DisplayName("坐标系")]
        [DescriptionAttribute("输出属性")]
        public userWcsCoordSystem WcsCoordSystem { get => _wcsCoordSystem; set => _wcsCoordSystem = value; }

        public double[] DataError { get { return this._dataError; } set { this._dataError = value; } }
        public userPixCoordSystem PixCoordSystem { get => _pixCoordSystem; set => _pixCoordSystem = value; }
        public ContourMatchParam Param { get; set; }
        public userWcsPolyLine WcsPolyLine { get => _wcsPolyLine; set => _wcsPolyLine = value; }

        /// <summary>
        /// 这里应该使用采集源而不是相机名
        /// </summary>
        /// <param name="camName"></param>
        /// <param name="coordSystem"></param>
        public ContourModelMatch2D(string acqSourceName)
        {
            this.Param = new ContourMatchParam();
            this.ResultInfo = new BindingList<OcrResultInfo>();
        }

        public ContourModelMatch2D()
        {
            this.Param = new ContourMatchParam();
            this.ResultInfo = new BindingList<OcrResultInfo>();
        }



        #region 实现接口
        public OperateResult Execute(params object[] param)
        {
            this.Result.Succss = false;
            try
            {
                string index = "";
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
                this.Result.Succss = ContourMatchMethod.ContourMatch(this.CurTrackPoint, this.StdTrackPoint,this.Param,out this._wcsCoordSystem,out this._wcsPolyLine,out this._dataError);
                this._pixCoordSystem = this._wcsCoordSystem.GetPixCoordSystem();    
                ///////////////////////////////////////////////
                stopwatch.Stop();
                this.CreateResultInfo(5);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[0].SetValue(this.name, "Row", this._pixCoordSystem.CurrentPoint.Row);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[0].Std_Value = this._pixCoordSystem.ReferencePoint.Row;
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[1].SetValue(this.name, "Col", this._pixCoordSystem.CurrentPoint.Col);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[1].Std_Value = this._pixCoordSystem.ReferencePoint.Col;
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[2].SetValue(this.name, "Rad", this._pixCoordSystem.CurrentPoint.Rad);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[2].Std_Value = this._pixCoordSystem.ReferencePoint.Rad;
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[3].SetValue(this.name, "X", Math.Round(this._wcsCoordSystem.CurrentPoint.X, 5));
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[4].SetValue(this.name, "Y", Math.Round(this._wcsCoordSystem.CurrentPoint.Y, 5));;
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[5].SetValue(this.name, "Z", Math.Round(this._wcsCoordSystem.CurrentPoint.Z, 5));
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[6].SetValue(this.name, "Deg", this._wcsCoordSystem.CurrentPoint.Angle);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[7].SetValue(this.name, "Grab_X", this._wcsCoordSystem.Grab_x);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[8].SetValue(this.name, "Grab_Y", this._wcsCoordSystem.Grab_y);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[9].SetValue(this.name, "Grab_Theta", this._wcsCoordSystem.Grab_Theta);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[4].SetValue(this.name, "Time(ms)", stopwatch.ElapsedMilliseconds);
                ////////////////////////////////////////////////////
                OnExcuteCompleted(this._wcsPolyLine.CamName, this._wcsPolyLine.ViewWindow, this.name, this._wcsPolyLine);
            }
            catch (Exception ex)
            {
                this.Result.Succss = false;
                LoggerHelper.Error(this.name + "执行报错" + ex);
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
                case nameof(this.ResultInfo):
                    return this.ResultInfo;           
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

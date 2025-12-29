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
    [DefaultProperty(nameof(WcsPolyLine))]
    public class TrackTrans : BaseFunction, IFunction
    {
        private userWcsPolyLine _wcsPolyLine;

        private userWcsPoint[] _trackPoint;

        [DisplayName("输入轨迹")]
        [DescriptionAttribute("输入属性1")]
        public userWcsPoint[] TrackPoint
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
                                    listPoint.AddRange(wcsCircle.GetFitWcsPoint());
                                    break;
                                case nameof(userWcsCircleSector):
                                    userWcsCircleSector wcsCircleSector = item as userWcsCircleSector;
                                    listPoint.AddRange(wcsCircleSector.GetFitWcsPoint());
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
                    this._trackPoint = listPoint.ToArray();
                    listPoint.Clear();
                }
                return this._trackPoint;
            }
            set
            {
                this._trackPoint = value;
            }
        }


        [DisplayName("输出轨迹")]
        [DescriptionAttribute("输出属性")]
        public userWcsPolyLine WcsPolyLine { get { return _wcsPolyLine; } set { this._wcsPolyLine = value; } }

        public TrackTransParam TransParam { get; set; }

        /// <summary>
        /// 这里应该使用采集源而不是相机名
        /// </summary>
        /// <param name="camName"></param>
        /// <param name="coordSystem"></param>
        public TrackTrans(string acqSourceName)
        {
            this.TransParam = new TrackTransParam();
            InitBindingTable();
        }

        public TrackTrans()
        {
            this.TransParam = new TrackTransParam();
            InitBindingTable();
        }
        private void InitBindingTable()
        {
            if (this.ResultInfo == null)
            {
                this.ResultInfo = new BindingList<OcrResultInfo>();
            }
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
                this._wcsPolyLine?.Clear();
                this.Result.Succss = TrackTransMethod.TransTrack(this.TrackPoint,this.TransParam,out this._wcsPolyLine);
                ///////////////////////////////////////////////
                stopwatch.Stop();
                this.CreateResultInfo(4);
                ((BindingList<OcrResultInfo>)this.ResultInfo)[0].SetValue(this.name, "轨迹X", string.Join(",", this.WcsPolyLine.X.ToArray()));
                ((BindingList<OcrResultInfo>)this.ResultInfo)[1].SetValue(this.name, "轨迹Y", string.Join(",", this.WcsPolyLine.Y.ToArray()));
                ((BindingList<OcrResultInfo>)this.ResultInfo)[2].SetValue(this.name, "轨迹Z", string.Join(",", this.WcsPolyLine.Z.ToArray()));
                ((BindingList<OcrResultInfo>)this.ResultInfo)[3].SetValue(this.name, "Time", stopwatch.ElapsedMilliseconds.ToString());
                ////////////////////////////////////////////////////
                OnExcuteCompleted(this.name + index, this.WcsPolyLine);
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

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
    [DefaultProperty(nameof(TrackPoint))]
    /// <summary>
    /// 激光取点采集的数据类
    /// </summary>
    public class ContourExport : BaseFunction, IFunction
    {
        [NonSerialized]
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
                        ////////////////
                        this._trackPoint = listPoint.ToArray();
                        listPoint.Clear();
                    }
                    else
                        this._trackPoint = null;
                }
                return this._trackPoint;
            }
            set
            {
                this._trackPoint = value;
            }
        }

        public ContourExportParam ExportParam { get; set; }
        public ContourExport()
        {
            this.ExportParam = new ContourExportParam();
            this.ResultInfo = new BindingList<ReadDataCommand>();
        }


        #region 实现接口,各自实现自己的逻辑，就可实现通用的逻辑
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
                this.Result.Succss = ContourExportMethod.ExportTrack(this.TrackPoint, this.ExportParam);
                //OnExcuteCompleted(this.name + index, this.WcsPolyLine);
            }
            catch (Exception e)
            {
                LoggerHelper.Error(this.name + "-读取轨迹数据：" + "报错" + e);
                this.Result.Succss = false;
            }
            if (this.Result.Succss)
                LoggerHelper.Info(this.name + "-读取轨迹数据：" + "成功");
            else
                LoggerHelper.Error(this.name + "-读取轨迹数据：" + "失败");
            // 更改UI字体　
            UpdataNodeElementStyle(param, this.Result.Succss);
            return this.Result;
        }

        public object GetPropertyValues(string propertyName)
        {
            switch (propertyName)
            {
                case "值":
                case nameof(this.TrackPoint):
                    return this.TrackPoint;

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
            catch (Exception ex)
            {
                LoggerHelper.Error(this.name + "删除节点出错" + ex.ToString());
            }
        }
        public void Read(string path)
        {
            //throw new NotImplementedException();
        }
        public void Save(string path)
        {
            // throw new NotImplementedException();
        }

        #endregion



    }

}

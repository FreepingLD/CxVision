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
using System.Diagnostics;

namespace FunctionBlock
{

    /// <summary>
    /// 将点去数据生成3D对象模型，以便于后续算子操作
    /// </summary>
    [Serializable]
    [DefaultProperty(nameof(WcsPolyLine))]
    public class TrackOffset : BaseFunction, IFunction
    {
        private userWcsPolyLine _wcsPolyLine;

        private userWcsPoint[] _trackPoint;

        [DisplayName("轨迹点")]
        [DescriptionAttribute("输出属性")]
        public userWcsPolyLine WcsPolyLine { get => _wcsPolyLine; set => _wcsPolyLine = value; }


        [DisplayName("输入点")]
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
                        ////////////////////
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

        public TrackOffsetParam Param { get; set; }



        public TrackOffset()
        {
            this.Param = new TrackOffsetParam();
            this.ResultInfo = new BindingList<OcrResultInfo>();
            ((BindingList<OcrResultInfo>)this.ResultInfo).Add(new OcrResultInfo());
            ((BindingList<OcrResultInfo>)this.ResultInfo).Add(new OcrResultInfo());
            ((BindingList<OcrResultInfo>)this.ResultInfo).Add(new OcrResultInfo());
            ((BindingList<OcrResultInfo>)this.ResultInfo).Add(new OcrResultInfo());
        }


        #region 实现接口

        public OperateResult Execute(params object[] param)
        {
            this.Result.Succss = true;
            try
            {
                string index = "";
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                ///// 检测传入的参数是否包含有图像 ///
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
                            case nameof(Double):
                            case nameof(Int32):
                                index = item.ToString();
                                break;
                        }
                    }
                }
                this.Result.Succss = TrackCalculateMethod.OffsetTrack(this.TrackPoint, this.Param, out this._wcsPolyLine);
                ///////////////////////////////////////////
                stopwatch.Stop();
                ((BindingList<OcrResultInfo>)this.ResultInfo)[0].SetValue(this.name, "轨迹数量", this._wcsPolyLine.Count().ToString());
                ((BindingList<OcrResultInfo>)this.ResultInfo)[1].SetValue(this.name, "Track_X", string.Join(",", this._wcsPolyLine.X));
                ((BindingList<OcrResultInfo>)this.ResultInfo)[2].SetValue(this.name, "Track_Y", string.Join(",", this._wcsPolyLine.Y));
                ((BindingList<OcrResultInfo>)this.ResultInfo)[3].SetValue(this.name, "时间(ms)", stopwatch.ElapsedMilliseconds.ToString());
                ////////////////////////////////////////////
                OnExcuteCompleted(this.name + index, this._wcsPolyLine);
            }
            catch (Exception ex)
            {
                LoggerHelper.Error(this.name + "->执行错误", ex);
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
                case nameof(this.Name):
                    return this.name;
                case "点对象":
                default:
                case nameof(this.WcsPolyLine):
                    return this.WcsPolyLine; //
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

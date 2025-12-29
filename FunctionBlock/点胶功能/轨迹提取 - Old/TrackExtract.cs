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
    public class TrackExtract : BaseFunction, IFunction
    {
        private userWcsPolyLine _wcsPolyLine;
        private userWcsPoint[] _trackPoint;
        private userWcsCoordSystem _wcsCoordSystem;
        private TreeNode _refNode;

        [DisplayName("轨迹点")]
        [DescriptionAttribute("输出属性")]
        public userWcsPolyLine WcsPolyLine { get => _wcsPolyLine; set => _wcsPolyLine = value; }


        [DisplayName("输入轨迹点")]
        [DescriptionAttribute("输入属性1")]
        public userWcsPoint[] TrackPoint
        {
            get
            {
                if (this.RefSource1.Count > 0)
                {
                    TreeNodeCollection nodes = this._refNode.FirstNode.Nodes;
                    string[] keyValues = new string[nodes.Count];
                    for (int i = 0; i < nodes.Count; i++)
                        keyValues[i] = nodes[i].Text;
                    List<userWcsPoint> listPoint = new List<userWcsPoint>();
                    ////////////////////////////////////////////////////////
                    for (int i = 0; i < keyValues.Length; i++)
                    {
                        object value = this.GetPropertyValue(this.RefSource1, keyValues[i]);
                        switch (value?.GetType().Name)
                        {
                            case nameof(userWcsPoint):
                                userWcsPoint wcsPoint = value as userWcsPoint;
                                listPoint.Add(wcsPoint);
                                break;
                            case nameof(userWcsVector):
                                userWcsVector wcsVector = value as userWcsVector;
                                listPoint.Add(new userWcsPoint(wcsVector.X, wcsVector.Y, wcsVector.Z, wcsVector.CamParams));
                                break;
                            case "userWcsVector[]":
                                userWcsVector[] wcsVectorA = value as userWcsVector[];
                                foreach (var item2 in wcsVectorA)
                                {
                                    listPoint.Add(new userWcsPoint(item2.X, item2.Y, item2.Z, item2.CamParams));
                                }
                                break;
                            case "userWcsPoint[]":
                                userWcsPoint[] wcsPoints = value as userWcsPoint[];
                                foreach (var item2 in wcsPoints)
                                {
                                    listPoint.Add(new userWcsPoint(item2.X, item2.Y, item2.Z, item2.CamParams));
                                }
                                break;
                            case nameof(userWcsLine):
                                userWcsLine wcsLine = value as userWcsLine;
                                listPoint.AddRange(wcsLine.GetFitWcsPoint());
                                break;
                            case nameof(userWcsCircle):
                                userWcsCircle wcsCircle = value as userWcsCircle;
                                if(wcsCircle != null)
                                {
                                    foreach (var item in wcsCircle.EdgesPoint_xyz)
                                    {
                                        listPoint.Add(new userWcsPoint(item.X, item.Y, item.Z, item.CamParams));
                                    }
                                }
                                break;
                            case nameof(userWcsCircleSector):
                                userWcsCircleSector wcsCircleSector = value as userWcsCircleSector;
                                if (wcsCircleSector != null)
                                {
                                    foreach (var item in wcsCircleSector.EdgesPoint_xyz)
                                    {
                                        listPoint.Add(new userWcsPoint(item.X, item.Y, item.Z, item.CamParams));
                                    }
                                }
                                break;
                            case nameof(userWcsRectangle2):
                                userWcsRectangle2 wcsRec2 = value as userWcsRectangle2;
                                double[] Px = new double[] { -1, 1, 1, -1 };
                                double[] Py = new double[] { 1, 1, -1, -1 };
                                HTuple Qx = null, Qy = null;
                                HHomMat2D hHomMat2D = new HHomMat2D();
                                hHomMat2D.VectorAngleToRigid(0, 0, 0, wcsRec2.X, wcsRec2.Y, wcsRec2.Deg * Math.PI / 180);
                                Qx = hHomMat2D.AffineTransPoint2d(Px, Py, out Qy);
                                if (Qx != null)
                                {
                                    for (int k = 0; k < Qx.Length; k++)
                                    {
                                        listPoint.Add(new userWcsPoint(Qx[k].D, Qy[k].D, wcsRec2.Z));
                                    }
                                }
                                break;
                            case nameof(userWcsPolyLine):
                                userWcsPolyLine wcsPolyLine = value as userWcsPolyLine;
                                for (int k = 0; k < wcsPolyLine.X.Count; k++)
                                {
                                    listPoint.Add(new userWcsPoint(wcsPolyLine.X[k], wcsPolyLine.Y[k], 0));
                                }
                                break;
                            case nameof(userWcsPolygon):
                                userWcsPolygon wcsPolygon = value as userWcsPolygon;
                                for (int k = 0; k < wcsPolygon.X.Count; k++)
                                {
                                    listPoint.Add(new userWcsPoint(wcsPolygon.X[k], wcsPolygon.Y[k], 0));
                                }
                                break;
                        }

                    }
                    ////////////////////////////////////////////////////////
                    this._trackPoint = listPoint.ToArray();
                    listPoint.Clear();
                }
                else
                    this._trackPoint = null;
                return this._trackPoint;
            }
            set
            {
                this._trackPoint = value;
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
                            foreach (var item in oo)
                            {
                                switch (item.GetType().Name)
                                {
                                    case nameof(userPixCoordSystem):
                                        this._wcsCoordSystem = ((userPixCoordSystem)item).GetWcsCoordSystem();
                                        break;
                                    case nameof(userWcsCoordSystem):
                                        this._wcsCoordSystem = ((userWcsCoordSystem)item);
                                        break;
                                }
                            }
                        }
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

        public BindingList<TrackExtractParam> Param { get; set; }





        public TrackExtract()
        {
            this.Param = new BindingList<TrackExtractParam>();
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
                            case nameof(TreeNode):
                                this._refNode = item as TreeNode;
                                break;
                        }
                    }
                }
                this.Result.Succss = TrackCalculateMethod.ExtractTrack(this.TrackPoint, this.WcsCoordSystem, Param, out this._wcsPolyLine);
                ///////////////////////////////////////////
                stopwatch.Stop();
                this.CreateResultInfo(4);
                ((BindingList<OcrResultInfo>)this.ResultInfo)[0].SetValue(this.name, "轨迹点数", $"{this._wcsPolyLine.Count()}");
                ((BindingList<OcrResultInfo>)this.ResultInfo)[1].SetValue(this.name, "轨迹_X", $"{string.Join(",", this._wcsPolyLine.X)}");
                ((BindingList<OcrResultInfo>)this.ResultInfo)[2].SetValue(this.name, "轨迹_Y", $"{string.Join(",", this._wcsPolyLine.Y)}");
                ((BindingList<OcrResultInfo>)this.ResultInfo)[3].SetValue(this.name, "时间(ms)", $"{stopwatch.ElapsedMilliseconds}");
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

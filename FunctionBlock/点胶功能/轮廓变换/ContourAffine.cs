using Common;
using HalconDotNet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Security.Policy;
using System.Windows.Forms;

namespace FunctionBlock
{

    /// <summary>
    /// 将点去数据生成3D对象模型，以便于后续算子操作
    /// </summary>
    [Serializable]
    [DefaultProperty(nameof(WcsPolyLine))]
    public class ContourAffine : BaseFunction, IFunction
    {
        [NonSerialized]
        private TreeNode _refNode;
        private userWcsPolyLine _wcsPolyLine;
        private userWcsPoint[] _wcsPoint;
        private userWcsPoint[] _wcsTargetPoint;
        private userWcsCoordSystem[] _wcsCoordSystem;

        [DisplayName("输出轮廓")]
        [DescriptionAttribute("输出属性")]
        public userWcsPolyLine WcsPolyLine { get => _wcsPolyLine; set => _wcsPolyLine = value; }


        [DisplayName("输入轨迹")]
        [DescriptionAttribute("输入属性1")]
        public userWcsPoint[] WcsPoint
        {
            get
            {
                try
                {
                    if (this.RefSource1.Count > 0)
                    {
                        TreeNodeCollection nodes = this._refNode.FirstNode.Nodes;
                        string[] keyValues = new string[nodes.Count];
                        for (int i = 0; i < nodes.Count; i++)
                            keyValues[i] = nodes[i].Name;
                        ////////////////////////////////////////////////
                        List<userWcsPoint> listPoint = new List<userWcsPoint>();
                        for (int i = 0; i < keyValues.Length; i++)
                        {
                            var value = this.GetPropertyValue(this.RefSource1, keyValues[i]);
                            switch (value?.GetType().Name)
                            {
                                case nameof(userWcsPoint):
                                    userWcsPoint wcsPoint = value as userWcsPoint;
                                    listPoint.Add(wcsPoint);
                                    break;
                                case "userWcsPoint[]":
                                    userWcsPoint[] wcsPoints = value as userWcsPoint[];
                                    if (wcsPoints != null)
                                    {
                                        foreach (var item in wcsPoints)
                                        {
                                            listPoint.Add(item);
                                        }
                                    }
                                    break;
                                case nameof(userWcsLine):
                                    userWcsLine wcsLine = value as userWcsLine;
                                    listPoint.Add(new userWcsPoint(wcsLine.X1, wcsLine.Y1, 0, wcsLine.Grab_x, wcsLine.Grab_y, wcsLine.CamParams));
                                    listPoint.Add(new userWcsPoint(wcsLine.X2, wcsLine.Y2, 0, wcsLine.Grab_x, wcsLine.Grab_y, wcsLine.CamParams));
                                    break;
                                case nameof(userWcsCircle):
                                    userWcsCircle wcsCircle = value as userWcsCircle;
                                    if (wcsCircle != null && wcsCircle.EdgesPoint_xyz != null)
                                    {
                                        foreach (var item in wcsCircle.EdgesPoint_xyz)
                                        {
                                            listPoint.Add(item);
                                        }
                                    }
                                    break;
                                case nameof(userWcsCircleSector):
                                    userWcsCircleSector wcsCircleSector = value as userWcsCircleSector;
                                    if (wcsCircleSector != null && wcsCircleSector.EdgesPoint_xyz != null)
                                    {
                                        foreach (var item in wcsCircleSector.EdgesPoint_xyz)
                                        {
                                            listPoint.Add(item);
                                        }
                                    }
                                    break;

                                case nameof(userWcsPolyLine):
                                    userWcsPolyLine wcsPolyLine = value as userWcsPolyLine;
                                    for (int k = 0; k < wcsPolyLine.X.Count; k++)
                                    {
                                        listPoint.Add(new userWcsPoint(wcsPolyLine.X[k], wcsPolyLine.Y[k], 0, wcsPolyLine.Grab_x, wcsPolyLine.Grab_y, wcsPolyLine.CamParams));
                                    }
                                    break;
                                case nameof(userWcsPolygon):
                                    userWcsPolygon wcsPolygon = value as userWcsPolygon;
                                    for (int k = 0; k < wcsPolygon.X.Count; k++)
                                    {
                                        userWcsPoint wcsPoint1 = new userWcsPoint(wcsPolygon.X[k], wcsPolygon.Y[k], 0, wcsPolygon.Grab_x, wcsPolygon.Grab_y, wcsPolygon.CamParams);
                                        listPoint.Add(wcsPoint1);
                                    }
                                    break;
                                default:
                                    new UserMessageForm().ShowDialog("不受支持的数据类型!");
                                    break;
                            }
                        }
                        this._wcsPoint = listPoint.ToArray();
                        listPoint.Clear();
                    }
                    else
                        this._wcsPoint = null;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return _wcsPoint;
            }
            set { _wcsPoint = value; }
        }

        [DisplayName("坐标系")]
        [DescriptionAttribute("输入属性2")]
        public userWcsCoordSystem[] WcsCoordSystem
        {
            get
            {
                try
                {
                    if (this.RefSource2.Count > 0)
                    {
                        TreeNodeCollection nodes = this._refNode.Nodes[1].Nodes;
                        string[] keyValues = new string[nodes.Count];
                        for (int i = 0; i < nodes.Count; i++)
                            keyValues[i] = nodes[i].Name;
                        ////////////////////////////////////////////////
                        List<userWcsCoordSystem> listCooreSys = new List<userWcsCoordSystem>();
                        for (int i = 0; i < keyValues.Length; i++)
                        {
                            var value = this.GetPropertyValue(this.RefSource2, keyValues[i]);
                            switch (value?.GetType().Name)
                            {
                                case "userWcsCoordSystem[]":
                                    userWcsCoordSystem[] wcsCoordSystems = value as userWcsCoordSystem[];
                                    foreach (var item in wcsCoordSystems)
                                    {
                                        listCooreSys.Add(item);
                                    }
                                    break;
                                case "userPixCoordSystem[]":
                                    userPixCoordSystem[] pixCoordSystems = value as userPixCoordSystem[];
                                    foreach (var item in pixCoordSystems)
                                    {
                                        listCooreSys.Add(item.GetWcsCoordSystem());
                                    }
                                    break;
                                case nameof(userWcsCoordSystem):
                                    listCooreSys.Add(((userWcsCoordSystem)value));
                                    break;
                                case nameof(userPixCoordSystem):
                                    listCooreSys.Add(((userPixCoordSystem)value).GetWcsCoordSystem());
                                    break;
                                case nameof(userWcsVector):
                                    userWcsVector wcsVector = value as userWcsVector;
                                    listCooreSys.Add(new userWcsCoordSystem(new userWcsVector(), wcsVector.Clone()));
                                    break;
                                default:
                                    new UserMessageForm().ShowDialog("不受支持的数据类型!");
                                    break;
                            }

                        }
                        ////////////////////////////
                        this._wcsCoordSystem = listCooreSys.ToArray();
                        listCooreSys.Clear();
                    }
                    else
                        this._wcsCoordSystem = null;
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


        [DisplayName("目标轨迹")]
        [DescriptionAttribute("输入属性3")]
        public userWcsPoint[] WcsTargetPoint
        {
            get
            {
                try
                {
                    if (this.RefSource3.Count > 0)
                    {
                        TreeNodeCollection nodes = this._refNode.Nodes[2].Nodes;
                        string[] keyValues = new string[nodes.Count];
                        for (int i = 0; i < nodes.Count; i++)
                            keyValues[i] = nodes[i].Name;
                        ////////////////////////////////////////////////
                        List<userWcsPoint> listPoint = new List<userWcsPoint>();
                        for (int i = 0; i < keyValues.Length; i++)
                        {
                            var value = this.GetPropertyValue(this.RefSource3, keyValues[i]);
                            switch (value?.GetType().Name)
                            {
                                case nameof(userWcsPoint):
                                    userWcsPoint wcsPoint = value as userWcsPoint;
                                    listPoint.Add(wcsPoint);
                                    break;
                                case "userWcsPoint[]":
                                    userWcsPoint[] wcsPoints = value as userWcsPoint[];
                                    if (wcsPoints != null)
                                    {
                                        foreach (var item in wcsPoints)
                                        {
                                            listPoint.Add(item);
                                        }
                                    }
                                    break;
                                case nameof(userWcsLine):
                                    userWcsLine wcsLine = value as userWcsLine;
                                    listPoint.Add(new userWcsPoint(wcsLine.X1, wcsLine.Y1, 0, wcsLine.Grab_x, wcsLine.Grab_y, wcsLine.CamParams));
                                    listPoint.Add(new userWcsPoint(wcsLine.X2, wcsLine.Y2, 0, wcsLine.Grab_x, wcsLine.Grab_y, wcsLine.CamParams));
                                    break;
                                case nameof(userWcsCircle):
                                    userWcsCircle wcsCircle = value as userWcsCircle;
                                    if (wcsCircle != null && wcsCircle.EdgesPoint_xyz != null)
                                    {
                                        foreach (var item in wcsCircle.EdgesPoint_xyz)
                                        {
                                            listPoint.Add(item);
                                        }
                                    }
                                    break;
                                case nameof(userWcsCircleSector):
                                    userWcsCircleSector wcsCircleSector = value as userWcsCircleSector;
                                    if (wcsCircleSector != null && wcsCircleSector.EdgesPoint_xyz != null)
                                    {
                                        foreach (var item in wcsCircleSector.EdgesPoint_xyz)
                                        {
                                            listPoint.Add(item);
                                        }
                                    }
                                    break;

                                case nameof(userWcsPolyLine):
                                    userWcsPolyLine wcsPolyLine = value as userWcsPolyLine;
                                    for (int k = 0; k < wcsPolyLine.X.Count; k++)
                                    {
                                        listPoint.Add(new userWcsPoint(wcsPolyLine.X[k], wcsPolyLine.Y[k], 0, wcsPolyLine.Grab_x, wcsPolyLine.Grab_y, wcsPolyLine.CamParams));
                                    }
                                    break;
                                case nameof(userWcsPolygon):
                                    userWcsPolygon wcsPolygon = value as userWcsPolygon;
                                    for (int k = 0; k < wcsPolygon.X.Count; k++)
                                    {
                                        userWcsPoint wcsPoint1 = new userWcsPoint(wcsPolygon.X[k], wcsPolygon.Y[k], 0, wcsPolygon.Grab_x, wcsPolygon.Grab_y, wcsPolygon.CamParams);
                                        listPoint.Add(wcsPoint1);
                                    }
                                    break;
                            }
                        }
                        this._wcsTargetPoint = listPoint.ToArray();
                        listPoint.Clear();
                    }
                    else
                        this._wcsTargetPoint = null;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return _wcsTargetPoint;
            }
            set { _wcsTargetPoint = value; }
        }



        public ContourAffineParam Param;

        public ContourAffine()
        {
            this.Param = new ContourAffineParam();
            this.ResultInfo = new BindingList<OcrResultInfo>();
        }




        #region 实现接口

        public OperateResult Execute(params object[] param)
        {
            this.Result.Succss = true;
            try
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                ///// 检测传入的参数是否包含有图像 ///
                if (param != null)
                {
                    foreach (var item in param)
                    {
                        if (item != null)
                        {
                            switch (item.GetType().Name)
                            {

                                case nameof(String):
                                    string index = "1";
                                    if (item.ToString().Split('=').Length > 0)
                                        index = item.ToString().Split('=').Last();
                                    break;
                                case nameof(TreeNode):
                                    this._refNode = item as TreeNode;
                                    break;
                            }
                        }
                    }
                }
                this.Result.Succss = this.Param.Affine(this.WcsPoint, this.WcsCoordSystem, out _wcsPolyLine);
                #region 测试
                //double[] x = new double[this.WcsPoint.Length];
                //double[] y = new double[this.WcsPoint.Length];
                //double[] Qx, Qy, Angle;
                //userWcsCoordSystem wcsSystem = wcsSystem = new userWcsCoordSystem();
                //for (int i = 0; i < this._wcsPoint.Length; i++)
                //{
                //    x[i] = this._wcsPoint[i].X;
                //    y[i] = this._wcsPoint[i].Y;
                //}
                //////////////////////////////////////////////////
                //double sy, phi, theta, tx, ty;
                //this.WcsCoordSystem[0].GetHomMat2D().HomMat2dToAffinePar(out sy, out phi, out theta, out tx, out ty);
                //this.Result.Succss = this.Param.AffineTransPoint(x, y, tx, ty, phi * 180.0 / Math.PI, "逆时针", out Qx, out Qy, out Angle);
                //this._wcsPolyLine = new userWcsPolyLine();
                //for (int i = 0; i < Qx.Length; i++)
                //{
                //    this._wcsPolyLine.Add(Qx[i], Qy[i]);
                //}
                #endregion
                stopwatch.Stop();
                this.CreateResultInfo(5);
                ///////////////////////////////////////////
                ((BindingList<OcrResultInfo>)this.ResultInfo)[0].SetValue(this.name, "轨迹点数", this._wcsPolyLine.X.Count.ToString());
                ((BindingList<OcrResultInfo>)this.ResultInfo)[1].SetValue(this.name, "轨迹X", string.Join(",", this._wcsPolyLine.X.ToArray()));
                ((BindingList<OcrResultInfo>)this.ResultInfo)[2].SetValue(this.name, "轨迹Y", string.Join(",", this._wcsPolyLine.Y.ToArray()));
                ((BindingList<OcrResultInfo>)this.ResultInfo)[3].SetValue(this.name, "轨迹Z", string.Join(",", this._wcsPolyLine.Z.ToArray()));
                ((BindingList<OcrResultInfo>)this.ResultInfo)[4].SetValue(this.name, "Time(ms)", stopwatch.ElapsedMilliseconds.ToString());
                ////////////////////////////////////////////
                OnExcuteCompleted(this._wcsPolyLine.CamName, this._wcsPolyLine?.ViewWindow, this.name, this._wcsPolyLine.GetHObjectModel3D());
            }
            catch (Exception ex)
            {
                LoggerHelper.Error(this.name + "->执行错误", ex);
                this.Result.Succss = false;
            }
            if (this.Result.Succss)
                LoggerHelper.Info(this.name + "->执行成功：" + this._wcsPolyLine.ToString());
            else
                LoggerHelper.Error(this.name + "->执行失败：" + this._wcsPolyLine.ToString());
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
                case "直线对象":
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
                case nameof(TreeNode):
                    this._refNode = value[0] as TreeNode;
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

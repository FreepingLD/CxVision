using Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;

namespace FunctionBlock
{

    /// <summary>
    /// 将点去数据生成3D对象模型，以便于后续算子操作
    /// </summary>
    [Serializable]
    [DefaultProperty(nameof(WcsPolyLine))]
    public class ContourOffset : BaseFunction, IFunction
    {
        [NonSerialized]
        private TreeNode _refNode;
        private userWcsPolyLine _wcsPolyLine;
        private userWcsLine _offsetDist;
        private userWcsPoint[] _wcsPoint;
        public ContourOffsetParam OffsetParam { get; set; }

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
                                case nameof(userWcsLine):
                                    userWcsLine wcsLine  = value as userWcsLine;
                                    listPoint.Add(new userWcsPoint(wcsLine.X1, wcsLine.Y1, 0, wcsLine.Grab_x, wcsLine.Grab_y, wcsLine.CamParams));
                                    listPoint.Add(new userWcsPoint(wcsLine.X2, wcsLine.Y2, 0, wcsLine.Grab_x, wcsLine.Grab_y, wcsLine.CamParams));
                                    break;
                                case nameof(userWcsCircle):
                                    userWcsCircle wcsCircle= value as userWcsCircle;
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
                                    userWcsPolygon wcsPolygon  = value as userWcsPolygon;
                                    for (int k = 0; k < wcsPolygon.X.Count; k++)
                                    {
                                        userWcsPoint wcsPoint1 = new userWcsPoint(wcsPolygon.X[k], wcsPolygon.Y[k], 0, wcsPolygon.Grab_x, wcsPolygon.Grab_y, wcsPolygon.CamParams);
                                        listPoint.Add(wcsPoint1);
                                    }
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

        [DisplayName("偏置距离")]
        [DescriptionAttribute("输入属性2")]
        public userWcsLine OffsetDist
        {
            get
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
                                case nameof(userWcsLine):
                                    this._offsetDist = item as userWcsLine;
                                    break;
                                case nameof(Double):
                                    double value = Convert.ToDouble(item);
                                    this._offsetDist = new userWcsLine(0, 0, 0, value, 0, 0);
                                    break;
                            }
                        }
                    }
                    else
                        this._offsetDist = null;
                }
                else
                    this._offsetDist = null;
                ////////////////////////////////
                return this._offsetDist;
            }
            set
            {
                this._offsetDist = value;
            }
        }


        public ContourOffset()
        {
            this.OffsetParam = new ContourOffsetParam();
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
                this.OffsetParam.Scale = 1;
                ///// 检测传入的参数是否包含有图像 ///
                if (param != null)
                {
                    foreach (var item in param)
                    {
                        if (item != null)
                        {
                            switch (item.GetType().Name)
                            {
                                case nameof(Int32):
                                    int result = 0;
                                    if (int.TryParse(item.ToString(), out result))
                                        this.OffsetParam.Scale = result; // 如果循环，那么则倍增 
                                    if (this.OffsetParam.Scale == 0) this.OffsetParam.Scale = 1; // 这个值不能为0
                                    break;
                                case nameof(String):
                                    string index = "1";
                                    if (item.ToString().Split('=').Length > 0)
                                        index = item.ToString().Split('=').Last();
                                    if (int.TryParse(index, out result))
                                        this.OffsetParam.Scale = result;
                                    if (this.OffsetParam.Scale == 0) this.OffsetParam.Scale = 1; // 这个值不能为0
                                    break;
                                case nameof(TreeNode):
                                    this._refNode = item as TreeNode;
                                    break;
                            }
                        }
                    }
                }
                this.Result.Succss = this.OffsetParam.OffsetContour(this.WcsPoint, this.OffsetDist, out _wcsPolyLine);
                stopwatch.Stop();
                this.CreateResultInfo(5);
                ///////////////////////////////////////////
                ((BindingList<OcrResultInfo>)this.ResultInfo)[0].SetValue(this.name, "轨迹点数", this._wcsPolyLine.X.Count.ToString());
                ((BindingList<OcrResultInfo>)this.ResultInfo)[1].SetValue(this.name, "轨迹X", string.Join(",", this._wcsPolyLine.X.ToArray()));
                ((BindingList<OcrResultInfo>)this.ResultInfo)[2].SetValue(this.name, "轨迹Y", string.Join(",", this._wcsPolyLine.Y.ToArray()));
                ((BindingList<OcrResultInfo>)this.ResultInfo)[3].SetValue(this.name, "轨迹Z", string.Join(",", this._wcsPolyLine.Z.ToArray()));
                ((BindingList<OcrResultInfo>)this.ResultInfo)[4].SetValue(this.name, "Time(ms)", stopwatch.ElapsedMilliseconds.ToString());
                ////////////////////////////////////////////
                OnExcuteCompleted(this._wcsPolyLine.CamName, this._wcsPolyLine?.ViewWindow, this.name + this.OffsetParam.Scale.ToString(), this._wcsPolyLine);
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

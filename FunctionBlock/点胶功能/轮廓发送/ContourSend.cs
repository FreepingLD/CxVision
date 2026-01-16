using Common;
using MotionControlCard;
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
    [DefaultProperty(nameof(WcsPoint))]
    public class ContourSend : BaseFunction, IFunction
    {
        [NonSerialized]
        private TreeNode _refNode;
        private userWcsPoint[] _wcsPoint;
        private userWcsCoordSystem[] _wcsCoordSystem;
        private userWcsPolyLine _wcsPolyLine;

        [DisplayName("发送轮廓")]
        [DescriptionAttribute("输出属性")]
        public userWcsPolyLine WcsPolyLine { get => _wcsPolyLine; set => _wcsPolyLine = value; }


        [DisplayName("输入轮廓")]
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
                                        userWcsPoint wcsPoint1 = new userWcsPoint(wcsPolyLine.X[k], wcsPolyLine.Y[k], 0, wcsPolyLine.Grab_x, wcsPolyLine.Grab_y, wcsPolyLine.CamParams);
                                        wcsPoint1.CamName = wcsPolyLine.CamName;
                                        wcsPoint1.ViewWindow = wcsPolyLine.ViewWindow;
                                        listPoint.Add(wcsPoint1);
                                    }
                                    break;
                                case nameof(userWcsPolygon):
                                    userWcsPolygon wcsPolygon = value as userWcsPolygon;
                                    for (int k = 0; k < wcsPolygon.X.Count; k++)
                                    {
                                        userWcsPoint wcsPoint1 = new userWcsPoint(wcsPolygon.X[k], wcsPolygon.Y[k], 0, wcsPolygon.Grab_x, wcsPolygon.Grab_y, wcsPolygon.CamParams);
                                        wcsPoint1.CamName = wcsPolygon.CamName;
                                        wcsPoint1.ViewWindow = wcsPolygon.ViewWindow;
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


        public ContourSendParam Param;

        public ContourSend()
        {
            this.Param = new ContourSendParam();
            this.ResultInfo = new BindingList<OcrResultInfo>();
        }




        #region 实现接口

        public OperateResult Execute(params object[] param)
        {
            this.Result.Succss = true;
            bool isOk = true;
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
                /////////////////////////////////////////////////////
                double[] angle;
                string pointOrder;
                this._wcsPolyLine = new userWcsPolyLine();
                List<string> list = new List<string>();
                userWcsPoint[] _sourceWcsPoint = new userWcsPoint[0];
                switch(this.Param.InterMethod)
                {
                    // 
                    default:
                    case "NONE":
                        _sourceWcsPoint = this.WcsPoint;
                        break;
                    case "等间隔":
                        this.Param.LineInterpretationByStep(this.WcsPoint, this.Param.InterParam, out _sourceWcsPoint);
                        break;
                    case "等数量":
                        this.Param.LineInterpretationByCount(this.WcsPoint, (int)this.Param.InterParam, out _sourceWcsPoint);
                        break;
                }
                if (this.Param.EnableCAngleCalCulate)
                {
                    this.Param.CalculateNormal(_sourceWcsPoint, this.Param.Orientation, out angle);
                    // 测试代码
                    //double[] Qx, Qy;
                    //double[] x = new double[this._wcsPoint.Length];
                    //double[] y = new double[this._wcsPoint.Length];
                    //for (int i = 0; i < this._wcsPoint.Length; i++)
                    //{
                    //    x[i] = this._wcsPoint[i].X;
                    //    y[i] = this._wcsPoint[i].Y;
                    //}
                    //this.Param.AffineTransPoint(x, y, 0, 0, 0,  this.Param.Orientation , out Qx, out Qy, out angle);
                }
                else
                    angle = new double[_sourceWcsPoint.Length];
                ////////////////////////////////////////////////////////////////////////////////
                for (int i = 0; i < _sourceWcsPoint.Length; i++)
                {
                    userWcsPoint item = _sourceWcsPoint[i];
                    if (this.Param.EnableCAngleCalCulate)
                    {
                        if (this.Param.InvertAngle)
                            item.Theta = (angle[i] - this.Param.OffsetAngle) * -1 + this.Param.InitAngle;
                        else
                            item.Theta = (angle[i] - this.Param.OffsetAngle) + this.Param.InitAngle;
                    }
                    else
                        item.Theta = 0.0;
                    ///////////////////////////////////////////////////
                    list.Add(string.Join(",", Math.Round(item.X, 5), Math.Round(item.Y, 5), Math.Round(item.Z, 5), Math.Round(item.U, 5), Math.Round(item.V, 5), Math.Round(item.Theta, 5)));
                    this._wcsPolyLine.Add(item.X, item.Y, item.Z, item.U, item.V, item.Theta);
                    this._wcsPolyLine.CamName = item.CamParams?.SensorName;
                    this._wcsPolyLine.ViewWindow = this.Param.ViewWindow;
                }
                string value = string.Join(";", list.ToArray()) + ",A";
                int len = value.Length;
                isOk = CommunicationConfigParamManger.Instance.WriteValue(this.Param.CoordSysName, this.Param.LengthAdress, (int)(len * 0.5) + 2);
                this.Result.Succss = isOk && CommunicationConfigParamManger.Instance.WriteValue(this.Param.CoordSysName, this.Param.Adress, value);
                stopwatch.Stop();
                this.CreateResultInfo(9);
                ///////////////////////////////////////////
                ((BindingList<OcrResultInfo>)this.ResultInfo)[0].SetValue(this.name, "轮廓点数", _sourceWcsPoint.Length.ToString());
                ((BindingList<OcrResultInfo>)this.ResultInfo)[1].SetValue(this.name, "长度", (len * 0.5 + 1).ToString());
                ((BindingList<OcrResultInfo>)this.ResultInfo)[2].SetValue(this.name, "X", string.Join(",", this._wcsPolyLine.X.ToArray()));
                ((BindingList<OcrResultInfo>)this.ResultInfo)[3].SetValue(this.name, "Y", string.Join(",", this._wcsPolyLine.Y.ToArray()));
                ((BindingList<OcrResultInfo>)this.ResultInfo)[4].SetValue(this.name, "Z", string.Join(",", this._wcsPolyLine.Z.ToArray()));
                ((BindingList<OcrResultInfo>)this.ResultInfo)[5].SetValue(this.name, "A", string.Join(",", this._wcsPolyLine.U.ToArray()));
                ((BindingList<OcrResultInfo>)this.ResultInfo)[6].SetValue(this.name, "B", string.Join(",", this._wcsPolyLine.V.ToArray()));
                ((BindingList<OcrResultInfo>)this.ResultInfo)[7].SetValue(this.name, "C", string.Join(",", this._wcsPolyLine.W.ToArray()));
                ((BindingList<OcrResultInfo>)this.ResultInfo)[8].SetValue(this.name, "Time(ms)", stopwatch.ElapsedMilliseconds.ToString());
                ////////////////////////////////////////////
                OnExcuteCompleted(this._wcsPolyLine.CamName, this._wcsPolyLine?.ViewWindow, this.name, this._wcsPolyLine.GetHObjectModel3D());
            }
            catch (Exception ex)
            {
                LoggerHelper.Error(this.name + "->执行错误", ex, this._wcsPolyLine.CamName);
                this.Result.Succss = false;
            }
            if (this.Result.Succss)
                LoggerHelper.Info(this.name + $"->执行成功,发送轮廓数量:{this._wcsPoint.Length}", this._wcsPolyLine.CamName);
            else
                LoggerHelper.Error(this.name + "->执行失败", this._wcsPolyLine.CamName);
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

        }
        public void Save(string path)
        {

        }

        #endregion





    }
}

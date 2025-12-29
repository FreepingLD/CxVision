using AlgorithmsLibrary;
using Common;
using HalconDotNet;
using MotionControlCard;
using Sensor;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FunctionBlock
{
    [Serializable]
    [DefaultProperty(nameof(WcsCircleSector))]
    public class FitCircleSector : BaseFunction, IFunction
    {
        [NonSerialized]
        private TreeNode _refNode = null;
        private userWcsCircleSector _wcsCircleSector;
        private userWcsPoint[] _wcsPoint;
        public CircleFitParam FitParam
        {
            get;
            set;
        }

        [DisplayName("圆弧对象")]
        [DescriptionAttribute("输出属性")]
        public userWcsCircleSector WcsCircleSector { get => _wcsCircleSector; set => _wcsCircleSector = value; }


        [DisplayName("输入点")]
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
                                case nameof(userWcsCircleSector):
                                    userWcsCircleSector wcsCircleSector = value as userWcsCircleSector;
                                    switch (this.FitParam.PointOrder)
                                    {
                                        default:
                                        case "逆时针方向": // 
                                            if (wcsCircleSector.PointOrder == "positive")
                                            {
                                                foreach (var item3 in wcsCircleSector.EdgesPoint_xyz)
                                                {
                                                    listPoint.Add(item3);
                                                }
                                            }
                                            else
                                            {
                                                for (int k = wcsCircleSector.EdgesPoint_xyz.Length - 1; k >= 0; k--)
                                                {
                                                    var item3 = wcsCircleSector.EdgesPoint_xyz[k];
                                                    listPoint.Add(item3);
                                                }
                                            }
                                            break;
                                        case "顺时针方向":
                                            if (wcsCircleSector.PointOrder == "negative")
                                            {
                                                foreach (var item3 in wcsCircleSector.EdgesPoint_xyz)
                                                {
                                                    listPoint.Add(item3);
                                                }
                                            }
                                            else
                                            {
                                                for (int k = wcsCircleSector.EdgesPoint_xyz.Length - 1; k >= 0; k--)
                                                {
                                                    var item3 = wcsCircleSector.EdgesPoint_xyz[k];
                                                    listPoint.Add(item3);
                                                }
                                            }
                                            break;
                                    }
                                    break;
                                case nameof(userWcsPoint):
                                    userWcsPoint wcsPoint = value as userWcsPoint;
                                    listPoint.Add(wcsPoint);
                                    break;
                                case nameof(userWcsPolyLine):
                                    userWcsPolyLine wcsPolyLine = value as userWcsPolyLine;
                                    for (int k = 0; k < wcsPolyLine.X.Count; k++)
                                    {
                                        listPoint.Add(new userWcsPoint(wcsPolyLine.X[k], wcsPolyLine.Y[k], 0, wcsPolyLine.Grab_x, wcsPolyLine.Grab_y, wcsPolyLine.CamParams));
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
        public FitCircleSector()
        {
            this.FitParam = new CircleFitParam();
            this.ResultInfo = new BindingList<OcrResultInfo>();
        }


        #region  实现接口的部分
        public OperateResult Execute(params object[] param)
        {
            this.Result.Succss = false;
            try
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Restart();
                if (param != null)
                {
                    foreach (var item in param)
                    {
                        switch (item?.GetType().Name)
                        {
                            case nameof(TreeNode):
                                this._refNode = item as TreeNode;
                                break;
                        }
                    }
                }
                this.Result.Succss = GeometryFitMethod.Instance.FitCircleSector(this.WcsPoint, this.FitParam, out this._wcsCircleSector);
                double[] x = new double[this._wcsCircleSector.EdgesPoint_xyz.Length];
                double[] y = new double[this._wcsCircleSector.EdgesPoint_xyz.Length];
                for (int i = 0; i < this._wcsCircleSector.EdgesPoint_xyz.Length; i++)
                {
                    x[i] = Math.Round(this._wcsCircleSector.EdgesPoint_xyz[i].X, 5);
                    y[i] = Math.Round(this._wcsCircleSector.EdgesPoint_xyz[i].Y, 5);
                }
                stopwatch.Stop();
                this.CreateResultInfo(6);
                ((BindingList<OcrResultInfo>)this.ResultInfo)[0].SetValue(this.name, "圆心X", $"{this._wcsCircleSector.X}");
                ((BindingList<OcrResultInfo>)this.ResultInfo)[1].SetValue(this.name, "圆心Y", $"{this._wcsCircleSector.Y}");
                ((BindingList<OcrResultInfo>)this.ResultInfo)[2].SetValue(this.name, "圆心Z", $"{this._wcsCircleSector.Z}");
                ((BindingList<OcrResultInfo>)this.ResultInfo)[3].SetValue(this.name, "半径", $"{this._wcsCircleSector.Radius}");
                ((BindingList<OcrResultInfo>)this.ResultInfo)[4].SetValue(this.name, "直径", $"{this._wcsCircleSector.Radius * 2}");
                ((BindingList<OcrResultInfo>)this.ResultInfo)[3].SetValue(this.name, "轨迹X", string.Join(",", x));
                ((BindingList<OcrResultInfo>)this.ResultInfo)[4].SetValue(this.name, "轨迹Y", string.Join(",", y));
                ((BindingList<OcrResultInfo>)this.ResultInfo)[5].SetValue(this.name, "Time(ms)", $"{stopwatch.ElapsedMilliseconds}");
                //////////////////////////////////////////
                OnExcuteCompleted(this._wcsCircleSector?.CamName, this._wcsCircleSector?.ViewWindow, this.name, this._wcsCircleSector); // 在图形窗口显示
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
                case nameof(this.WcsCircleSector):
                    return this._wcsCircleSector; //
                default:
                    return this._wcsCircleSector;
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

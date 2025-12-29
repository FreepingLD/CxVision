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
using System.Diagnostics;

namespace FunctionBlock
{
    [Serializable]
    [DefaultProperty(nameof(ReduceContour))]
    public class ContourReduce : BaseFunction, IFunction
    {
        [NonSerialized]
        private TreeNode _refNode = null;
        [NonSerialized]
        private userWcsPolyLine _wcsPolyLine;
        [NonSerialized]
        private userWcsPolyLine _reduceContour;
        [NonSerialized]
        private userWcsCoordSystem _wcsCoordSystem;

        [DisplayName("输出轮廓")]
        [DescriptionAttribute("输出属性")]
        public userWcsPolyLine ReduceContour { get => _reduceContour; set => _reduceContour = value; }

        [DisplayName("输入轮廓")]
        [DescriptionAttribute("输入属性1")]
        public userWcsPolyLine WcsPolyLine
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
                        this._wcsPolyLine = new userWcsPolyLine();
                        for (int i = 0; i < keyValues.Length; i++)
                        {
                            var value = this.GetPropertyValue(this.RefSource1, keyValues[i]);
                            switch (value?.GetType().Name)
                            {
                                case nameof(userWcsPoint):
                                    userWcsPoint wcsPoint = value as userWcsPoint;
                                    if (wcsPoint != null)
                                    {
                                        this._wcsPolyLine.Add(wcsPoint.X, wcsPoint.Y);
                                    }
                                    break;
                                case nameof(userWcsLine):
                                    userWcsLine wcsLine = value as userWcsLine;
                                    if (wcsLine != null)
                                    {
                                        this._wcsPolyLine.Add(wcsLine.X1, wcsLine.Y1);
                                        this._wcsPolyLine.Add(wcsLine.X2, wcsLine.Y2);
                                    }
                                    break;
                                case nameof(userWcsCircle):
                                    userWcsCircle wcsCircle = value as userWcsCircle;
                                    if (wcsCircle != null && wcsCircle.EdgesPoint_xyz != null)
                                    {
                                        foreach (var item in wcsCircle.EdgesPoint_xyz)
                                        {
                                            this._wcsPolyLine.Add(item.X, item.Y);
                                        }
                                    }
                                    break;
                                case nameof(userWcsCircleSector):
                                    userWcsCircleSector wcsCircleSector = value as userWcsCircleSector;
                                    if (wcsCircleSector.EdgesPoint_xyz != null)
                                    {
                                        foreach (var item in wcsCircleSector.EdgesPoint_xyz)
                                        {
                                            this._wcsPolyLine.Add(item.X, item.Y);
                                        }
                                    }
                                    break;
                                case nameof(userWcsEllipse):
                                    userWcsEllipse wcsEllipse = value as userWcsEllipse;
                                    if (wcsEllipse != null && wcsEllipse.EdgesPoint_xyz != null)
                                    {
                                        foreach (var item in wcsEllipse.EdgesPoint_xyz)
                                        {
                                            this._wcsPolyLine.Add(item.X, item.Y);
                                        }
                                    }
                                    break;
                                case nameof(userWcsEllipseSector):
                                    userWcsEllipseSector wcsEllipseSector = value as userWcsEllipseSector;
                                    if (wcsEllipseSector != null && wcsEllipseSector.EdgesPoint_xyz != null)
                                    {
                                        foreach (var item in wcsEllipseSector.EdgesPoint_xyz)
                                        {
                                            this._wcsPolyLine.Add(item.X, item.Y);
                                        }
                                    }
                                    break;
                                case nameof(userWcsPolyLine):
                                    userWcsPolyLine wcsPolyLine = value as userWcsPolyLine;
                                    if (wcsPolyLine != null)
                                    {
                                        this._wcsPolyLine.AddRange(wcsPolyLine.X.ToArray(), wcsPolyLine.Y.ToArray());
                                    }
                                    break;
                                case nameof(userWcsPolygon):
                                    userWcsPolygon wcsPolygon = value as userWcsPolygon;
                                    if (wcsPolygon != null)
                                    {
                                        this._wcsPolyLine.AddRange(wcsPolygon.X.ToArray(), wcsPolygon.Y.ToArray());
                                    }
                                    break;
                            }
                        }
                    }
                    else
                        this._wcsPolyLine = null;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return this._wcsPolyLine;
            }
            set
            {
                this._wcsPolyLine = value;
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
                                    case nameof(userWcsCoordSystem):
                                        this._wcsCoordSystem = ((userWcsCoordSystem)item);
                                        break;
                                    case nameof(userPixCoordSystem):
                                        this._wcsCoordSystem = ((userPixCoordSystem)item).GetWcsCoordSystem();
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

        public BindingList<ReduceContourParam> Param { get; set; }


        public ContourReduce()
        {
            this.Param = new BindingList<ReduceContourParam>();
            this.ResultInfo = new BindingList<OcrResultInfo>();
        }


        #region 实现接口
        public OperateResult Execute(params object[] param)
        {
            this.Result.Succss = false;
            try
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Restart();
                userWcsCoordSystem wcsCoordSystem = null;
                if (param != null)
                {
                    foreach (var item in param)
                    {
                        switch (item.GetType().Name)
                        {
                            case nameof(userWcsCoordSystem):
                                wcsCoordSystem = ((userWcsCoordSystem)item);
                                break;
                            case nameof(userPixCoordSystem):
                                wcsCoordSystem = ((userPixCoordSystem)item).GetWcsCoordSystem();
                                break;
                            case nameof(TreeNode):
                                this._refNode = item as TreeNode;
                                break;
                        }
                    }
                }
                if (wcsCoordSystem == null)
                    Result.Succss = ContourReduceMethod.ReduceContour(this.WcsPolyLine, this.WcsCoordSystem, this.Param, out this._reduceContour);
                else
                    Result.Succss = ContourReduceMethod.ReduceContour(this.WcsPolyLine, wcsCoordSystem, this.Param, out this._reduceContour);
                stopwatch.Stop();
                this.CreateResultInfo(3);
                ((BindingList<OcrResultInfo>)this.ResultInfo)[0].SetValue(this.name, "X", string.Join(",", this._reduceContour.X.ToString()));
                ((BindingList<OcrResultInfo>)this.ResultInfo)[1].SetValue(this.name, "Y", string.Join(",", this._reduceContour.Y.ToString()));
                ((BindingList<OcrResultInfo>)this.ResultInfo)[2].SetValue(this.name, "Time(ms)", $"{stopwatch.ElapsedMilliseconds}");
                OnExcuteCompleted(this.name, this._reduceContour);
            }
            catch (Exception ex)
            {
                this.Result.Succss = false;
                LoggerHelper.Error(this.name + "->执行错误：", ex);
            }
            if (Result.Succss)
                LoggerHelper.Info(this.name + "->执行成功：");
            else
                LoggerHelper.Error(this.name + "->执行失败：");
            // 更改UI字体　
            UpdataNodeElementStyle(param, Result.Succss);
            return Result;
        }

        public object GetPropertyValues(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(this.Name):
                case "名称":
                    return this.name;
                case "图像对象":
                case "输出对象":
                    return this._reduceContour; //
                default:
                    if (this.name == propertyName)
                        return this._reduceContour;
                    else return null;
            }
        }
        public bool SetPropertyValues(string propertyName, params object[] value)
        {
            switch (propertyName)
            {
                case "名称":
                    this.name = value[0].ToString();
                    return true;
                case "TreeNode":
                case "treeNode":
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

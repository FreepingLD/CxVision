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
    [Serializable]
    [DefaultProperty(nameof(WcsPolygon))]
    public class FitPolygon : BaseFunction, IFunction
    {
        [NonSerialized]
        private TreeNode _refNode = null;
        private userWcsPolygon _wcsPolygon;
        private userWcsPoint[] _wcsPoint;

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
                        for (int k = 0; k < keyValues.Length; k++)
                        {
                            var value = this.GetPropertyValue(this.RefSource1, keyValues[k]);
                            switch (value?.GetType().Name)
                            {
                                case nameof(userWcsLine):
                                    userWcsLine wcsLine = value as userWcsLine;
                                    if (wcsLine != null && wcsLine.EdgesPoint_xyz != null)
                                    {
                                        foreach (var item2 in wcsLine.EdgesPoint_xyz)
                                            listPoint.Add(item2);
                                    }
                                    break;
                                case nameof(userWcsPoint):
                                    listPoint.Add((userWcsPoint)value);
                                    break;
                                case nameof(userWcsVector):
                                    userWcsVector wcsVector = value as userWcsVector;
                                    listPoint.Add(new userWcsPoint(wcsVector.X, wcsVector.Y, wcsVector.Z, wcsVector.Grab_x, wcsVector.Grab_y, wcsVector.CamParams));
                                    break;
                                case nameof(userWcsCircle):
                                    userWcsCircle wcsCircle = value as userWcsCircle;
                                    if (wcsCircle.EdgesPoint_xyz != null && wcsCircle.EdgesPoint_xyz.Length > 0)
                                        listPoint.AddRange(wcsCircle.GetLineInterpretationPointByCount(wcsCircle.EdgesPoint_xyz.Length));
                                    else
                                        listPoint.AddRange(wcsCircle.GetLineInterpretationPointByStep(0.02));
                                    break;
                                case nameof(userWcsCircleSector):
                                    userWcsCircleSector wcsCircleSector = value as userWcsCircleSector;
                                    if (wcsCircleSector.EdgesPoint_xyz != null && wcsCircleSector.EdgesPoint_xyz.Length > 0)
                                        listPoint.AddRange(wcsCircleSector.GetLineInterpretationPointByCount(wcsCircleSector.EdgesPoint_xyz.Length));
                                    else
                                        listPoint.AddRange(wcsCircleSector.GetLineInterpretationPointByStep(0.02));
                                    break;
                                case nameof(userWcsPolyLine):
                                    userWcsPolyLine wcsPolyLine = value as userWcsPolyLine;
                                    for (int i = 0; i < wcsPolyLine.X.Count; i++)
                                    {
                                        listPoint.Add(new userWcsPoint(wcsPolyLine.X[i], wcsPolyLine.Y[i], 0, wcsPolyLine.Grab_x, wcsPolyLine.Grab_y, wcsPolyLine.CamParams));
                                    }
                                    break;
                                case nameof(userWcsPolygon):
                                    userWcsPolygon wcsPolygon = value as userWcsPolygon;
                                    for (int i = 0; i < wcsPolygon.X.Count; i++)
                                    {
                                        listPoint.Add(new userWcsPoint(wcsPolygon.X[i], wcsPolygon.Y[i], 0, wcsPolygon.Grab_x, wcsPolygon.Grab_y, wcsPolygon.CamParams));
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

        [DisplayName("多边形对象")]
        [DescriptionAttribute("输出属性")]
        public userWcsPolygon WcsPolygon { get => _wcsPolygon; set => _wcsPolygon = value; }

        public FitPolygon()
        {
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
                this.Result.Succss = GeometryFitMethod.Instance.FitPolygon(this.WcsPoint, out this._wcsPolygon);
                stopwatch.Stop();
                this.CreateResultInfo(5);
                ((BindingList<OcrResultInfo>)this.ResultInfo)[0].SetValue(this.name, "Count", this._wcsPolygon.X.Count.ToString());
                ((BindingList<OcrResultInfo>)this.ResultInfo)[1].SetValue(this.name, "X", string.Join(",", this._wcsPolygon.X));
                ((BindingList<OcrResultInfo>)this.ResultInfo)[2].SetValue(this.name, "Y", string.Join(",", this._wcsPolygon.Y));
                ((BindingList<OcrResultInfo>)this.ResultInfo)[3].SetValue(this.name, "Z", string.Join(",", this._wcsPolygon.Z));
                ((BindingList<OcrResultInfo>)this.ResultInfo)[4].SetValue(this.name, "Time(ms)", stopwatch.ElapsedMilliseconds.ToString());
                OnExcuteCompleted(this._wcsPolygon?.CamName, this._wcsPolygon?.CamParams?.ViewWindow, this._wcsPolygon); // 
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
                case nameof(this.WcsPolygon):
                    return this._wcsPolygon; //
                default:
                    return this._wcsPolygon;
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

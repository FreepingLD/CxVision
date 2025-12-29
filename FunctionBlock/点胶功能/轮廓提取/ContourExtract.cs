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
    public class ContourExtract : BaseFunction, IFunction
    {
        [NonSerialized]
        private TreeNode _refNode;
        private userWcsPolyLine _wcsPolyLine;
        private userWcsPolyLine _wcsPolyLineAffine;
        private userWcsPoint[] _curTrackPoint;
        private userWcsPolyLine _stdWcsPolyLine;
        private int _anomalyCount;

        [DisplayName("提取轨迹")]
        [DescriptionAttribute("输出属性")]
        public userWcsPolyLine WcsPolyLine { get => _wcsPolyLine; set => _wcsPolyLine = value; }


        [DisplayName("输入轨迹点")]
        [DescriptionAttribute("输入属性1")]
        public userWcsPoint[] CurTrackPoint
        {
            get
            {
                if (this.RefSource1.Count > 0)
                {
                    TreeNodeCollection nodes = this._refNode.FirstNode.Nodes;
                    string[] keyValues = new string[nodes.Count];
                    for (int i = 0; i < nodes.Count; i++)
                        keyValues[i] = nodes[i].Name;
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
                                if (wcsCircle != null)
                                {
                                    double count = (Math.PI * wcsCircle.Radius * 2) / this.Param.InterStep;
                                    listPoint.AddRange(wcsCircle.GetInterpolateWcsPoint((int)count));
                                }
                                break;
                            case nameof(userWcsCircleSector):
                                userWcsCircleSector wcsCircleSector = value as userWcsCircleSector;
                                if (wcsCircleSector != null)
                                {
                                    double count = wcsCircleSector.GetXLD().LengthXld().D / this.Param.InterStep;
                                    listPoint.AddRange(wcsCircleSector.GetInterpolateWcsPoint((int)count));
                                }
                                break;
                            case nameof(userWcsRectangle2):
                                userWcsRectangle2 wcsRec2 = value as userWcsRectangle2;
                                if (wcsRec2 != null)
                                    listPoint.AddRange(wcsRec2.GetInterpolateWcsPoint()); // 
                                break;
                            case nameof(userWcsEllipseSector):
                                userWcsEllipseSector wcsEllipseSector = value as userWcsEllipseSector;
                                if (wcsEllipseSector != null)
                                {
                                    double count = wcsEllipseSector.GetXLD().LengthXld().D / this.Param.InterStep;
                                    listPoint.AddRange(wcsEllipseSector.GetInterpolateWcsPoint((int)count));
                                }         
                                break;
                            case nameof(userWcsEllipse):
                                userWcsEllipse wcsEllipse = value as userWcsEllipse;
                                if (wcsEllipse != null)
                                {
                                    double count = wcsEllipse.GetXLD().LengthXld().D / this.Param.InterStep;
                                    listPoint.AddRange(wcsEllipse.GetInterpolateWcsPoint((int)count));
                                }
                                break;
                            case nameof(userWcsPolyLine):
                                userWcsPolyLine wcsPolyLine = value as userWcsPolyLine;
                                if (wcsPolyLine != null)
                                    listPoint.AddRange(wcsPolyLine.GetLineInterpretationPoint(this.Param.InterStep));
                                break;
                            case nameof(userWcsPolygon):
                                userWcsPolygon wcsPolygon = value as userWcsPolygon;
                                if (wcsPolygon != null)
                                    listPoint.AddRange(wcsPolygon.GetLineInterpretationPoint(this.Param.InterStep));
                                break;
                        }

                    }
                    ////////////////////////////////////////////////////////
                    this._curTrackPoint = listPoint.ToArray();
                    listPoint.Clear();
                }
                else
                    this._curTrackPoint = null;
                return this._curTrackPoint;
            }
            set
            {
                this._curTrackPoint = value;
            }
        }

        [DisplayName("标准轨迹")]
        [DescriptionAttribute("输出属性")]
        public userWcsPoint[] StdTrackPoint { get; set; }



        public ContourExtractParam Param { get; set; }
        public userWcsPolyLine WcsPolyLineAffine { get => _wcsPolyLineAffine; set => _wcsPolyLineAffine = value; }

        public ContourExtract()
        {
            this.Param = new ContourExtractParam();
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
                stopwatch.Restart();
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
                this._wcsPolyLine = new userWcsPolyLine();
                this.Result.Succss = ContourExtractMethod.ExtractTrack(this.CurTrackPoint, this.StdTrackPoint, Param, out this._wcsPolyLine, out this._wcsPolyLineAffine, out this._anomalyCount);
                ///////////////////////////////////////////
                stopwatch.Stop();
                this.CreateResultInfo(5);
                ((BindingList<OcrResultInfo>)this.ResultInfo)[0].SetValue(this.name, "轨迹点数", $"{this._wcsPolyLine.Count()}");
                ((BindingList<OcrResultInfo>)this.ResultInfo)[1].SetValue(this.name, "轨迹_X", $"{string.Join(",", this._wcsPolyLine.X)}");
                ((BindingList<OcrResultInfo>)this.ResultInfo)[2].SetValue(this.name, "轨迹_Y", $"{string.Join(",", this._wcsPolyLine.Y)}");
                ((BindingList<OcrResultInfo>)this.ResultInfo)[3].SetValue(this.name, "异常点数", $"{this._anomalyCount}");
                ((BindingList<OcrResultInfo>)this.ResultInfo)[3].LimitDown = this.Param.AnomalyCount;
                ((BindingList<OcrResultInfo>)this.ResultInfo)[3].LimitUp = this.Param.AnomalyCount;
                ((BindingList<OcrResultInfo>)this.ResultInfo)[4].SetValue(this.name, "Time(ms)", $"{stopwatch.ElapsedMilliseconds}");
                ////////////////////////////////////////////
                OnExcuteCompleted(this._wcsPolyLine.CamName, this._wcsPolyLine.ViewWindow, this.name + index, this._wcsPolyLine);
                //// 弹窗提示报警
                if (this._anomalyCount > this.Param.AnomalyCount)
                    new AnomalyExtractForm(this).ShowDialog();
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

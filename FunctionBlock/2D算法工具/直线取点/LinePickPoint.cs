using AlgorithmsLibrary;
using Common;
using HalconDotNet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FunctionBlock
{
    [Serializable]
    [DefaultProperty(nameof(WcsVector))]
    public class LinePickPoint : BaseFunction, IFunction, INotifyPropertyChanged
    {
        [NonSerialized]
        private TreeNode _refNode = null;
        private userWcsVector[] _wcsVector;
        private userWcsLine[] _wcsLine;
        private userWcsPoint _wcsPoint;
        private double _length = 1;

        [DisplayName("捨取点")]
        [DescriptionAttribute("输出属性")]
        public userWcsVector[] WcsVector { get => _wcsVector; set => _wcsVector = value; }

        [DisplayName("参考点")]
        [DescriptionAttribute("输入属性1")]
        public userWcsPoint WcsPoint
        {
            get
            {
                if (this.RefSource1.Count > 0)
                {
                    object[] oo = this.GetPropertyValue(this.RefSource1);
                    if (oo != null && oo.Length > 0)
                    {
                        foreach (var item in oo)
                        {
                            switch (item.GetType().Name)
                            {
                                case nameof(userWcsLine):
                                    userWcsLine wcsLine = item as userWcsLine;
                                    this._wcsPoint = new userWcsPoint(wcsLine.X1, wcsLine.Y1, wcsLine.Z1, wcsLine.Grab_x, wcsLine.Grab_y, wcsLine.CamParams);
                                    break;
                                case nameof(userPixLine):
                                    userPixLine pixLine = item as userPixLine;
                                    wcsLine = pixLine.GetWcsLine();
                                    this._wcsPoint = new userWcsPoint(wcsLine.X1, wcsLine.Y1, wcsLine.Z1, wcsLine.Grab_x, wcsLine.Grab_y, wcsLine.CamParams);
                                    break;
                                case nameof(userWcsPoint):
                                    this._wcsPoint = (item as userWcsPoint).Clone();
                                    break;
                                case nameof(userPixPoint):
                                    userPixPoint pixPoint = item as userPixPoint;
                                    this._wcsPoint = pixPoint.GetWcsPoint();
                                    break;
                                case nameof(userWcsVector):
                                    userWcsVector wcsVector = item as userWcsVector;
                                    this._wcsPoint = new userWcsPoint(wcsVector.X, wcsVector.Y, wcsVector.Z, wcsVector.Grab_x, wcsVector.Grab_y, wcsVector.CamParams);
                                    break;
                                case nameof(userWcsCoordSystem):
                                    userWcsCoordSystem coordSystem = item as userWcsCoordSystem;
                                    this._wcsPoint = new userWcsPoint(coordSystem.CurrentPoint.X, coordSystem.CurrentPoint.Y, coordSystem.CurrentPoint.Z,
                                                     coordSystem.CurrentPoint.Grab_x, coordSystem.CurrentPoint.Grab_y, coordSystem.CurrentPoint.CamParams);
                                    break;
                            }
                        }
                    }
                }
                else
                    this._wcsPoint = null;
                return this._wcsPoint;
            }
            set
            {
                this._wcsPoint = value;
            }
        }

        [DisplayName("输入直线")]
        [DescriptionAttribute("输入属性2")]
        public userWcsLine[] WcsLine
        {
            get
            {
                if (this.RefSource2.Count > 0)
                {
                    TreeNodeCollection nodes = this._refNode.Nodes[1].Nodes;
                    string[] keyValues = new string[nodes.Count];
                    for (int i = 0; i < nodes.Count; i++)
                        keyValues[i] = nodes[i].Name;
                    List<userWcsLine> list = new List<userWcsLine>();
                    for (int i = 0; i < keyValues.Length; i++)
                    {
                        var value = this.GetPropertyValue(this.RefSource2, keyValues[i]);
                        switch (value?.GetType().Name)
                        {
                            case nameof(userPixLine):
                                list.Add((value as userPixLine).GetWcsLine());
                                break;
                            case nameof(userWcsLine):
                                list.Add((value as userWcsLine).Clone());
                                break;
                            default:
                                throw new ArgumentException("参数类型错误");
                        }
                    }
                    this._wcsLine = list.ToArray();
                    list.Clear();
                }
                else
                    this._wcsLine = null;
                return this._wcsLine;
            }
            set
            {
                this._wcsLine = value;
            }
        }

        public double Length { get => _length; set => _length = value; }

        public LinePickPoint()
        {
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
                /////////////////////////////////////////////////////////////
                userWcsLine[] wcsLines = this.WcsLine;
                userWcsPoint wcsPoint = this.WcsPoint;
                if (wcsLines != null)
                {
                    // 如果输入的是两条线，则直接求交点
                    if (wcsPoint == null && wcsLines.Length > 1)
                    {
                        userWcsVector wcsVector;
                        HalconLibrary.IntersectionPoint(wcsLines[0], wcsLines[1], out wcsVector);
                        wcsPoint = new userWcsPoint(wcsVector.X, wcsVector.Y, wcsVector.Z, wcsVector.Grab_x, wcsVector.Grab_y, wcsVector.CamParams);
                    }
                    /////////////////////////////////////////////////////////
                    this._wcsVector = new userWcsVector[wcsLines.Length];
                    int index = 0;
                    foreach (var item in wcsLines)
                    {
                        double mid_x = (item.X1 + item.X2) * 0.5;
                        double mid_y = (item.Y1 + item.Y2) * 0.5;
                        double phi = Math.Atan2(mid_y - wcsPoint.Y, mid_x - wcsPoint.X);
                        double x = wcsPoint.X + this._length * Math.Cos(phi);
                        double y = wcsPoint.Y + this._length * Math.Sin(phi);
                        this._wcsVector[index] = new userWcsVector(x, y, item.Z1, phi * 180 / Math.PI, item.CamParams);
                        this._wcsVector[index].Grab_x = item.Grab_x;
                        this._wcsVector[index].Grab_y = item.Grab_y;
                        this._wcsVector[index].CamName = item.CamName;
                        this._wcsVector[index].ViewWindow = item.ViewWindow;
                        index++;
                    }
                }
                else
                    this._wcsVector = new userWcsVector[0];
                double[] point_x = new double[this._wcsVector.Length];
                double[] point_y = new double[this._wcsVector.Length];
                double[] point_angle = new double[this._wcsVector.Length];
                for (int i = 0; i < this._wcsVector.Length; i++)
                {
                    point_x[i] = this._wcsVector[i].X;
                    point_y[i] = this._wcsVector[i].Y;
                    point_angle[i] = this._wcsVector[i].Angle;
                }
                stopwatch.Stop();
                this.CreateResultInfo(4);
                ((BindingList<OcrResultInfo>)this.ResultInfo)[0].SetValue(this.name, "X", string.Join(",", point_x));
                ((BindingList<OcrResultInfo>)this.ResultInfo)[1].SetValue(this.name, "Y", string.Join(",", point_y));
                ((BindingList<OcrResultInfo>)this.ResultInfo)[2].SetValue(this.name, "Angle", string.Join(",", point_angle));
                ((BindingList<OcrResultInfo>)this.ResultInfo)[3].SetValue(this.name, "Time(ms)", stopwatch.ElapsedMilliseconds.ToString());
                for (int i = 0; i < this._wcsVector.Length; i++)
                {
                    OnExcuteCompleted(this._wcsVector[i].CamName, this._wcsVector[i].ViewWindow, $"{this.name}_{i}", this._wcsVector[i]);
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.Error(this.name + "->执行报错", ex);
                this.Result.Succss = false;
            }
            if (this.Result.Succss)
                LoggerHelper.Info(this.name + "执行成功");
            else
                LoggerHelper.Error(this.name + "执行失败");
            // 更改UI字体　
            UpdataNodeElementStyle(param, this.Result.Succss);
            return this.Result;
        }
        public object GetPropertyValues(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(Name):
                    return this.name;
                case nameof(WcsVector):
                default:
                    return this._wcsVector;
            }
        }
        public bool SetPropertyValues(string propertyName, params object[] value)
        {
            HalconLibrary ha = new HalconLibrary();
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

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
using System.Threading;

namespace FunctionBlock
{
    [Serializable]
    [DefaultProperty(nameof(WcsOutPoint))]
    public class WaferFindDie : BaseFunction, IFunction
    {
        private userWcsPoint _wcsOutPoint;
        private userWcsPoint[] _searchWcsPoint;
        private userWcsPoint[] _matchWcsPoint;
        private userWcsPoint _refWcsPoint;

        [DisplayName("参考点")]
        [DescriptionAttribute("输入属性1")]
        public userWcsPoint RefWcsPoint
        {
            get
            {
                try
                {
                    if (this.RefSource1.Count > 0)
                    {
                        var oo = this.GetPropertyValue(this.RefSource1);
                        foreach (var item in oo)
                        {
                            switch (item.GetType().Name)
                            {
                                case nameof(userWcsPoint):
                                    this._refWcsPoint = ((userWcsPoint)item);
                                    break;
                                case nameof(userWcsRectangle2):
                                    userWcsRectangle2 wcsRect2 = item as userWcsRectangle2;
                                    this._refWcsPoint = (new userWcsPoint(wcsRect2.X, wcsRect2.Y, wcsRect2.Z, wcsRect2.Grab_x, wcsRect2.Grab_y, wcsRect2.CamParams));
                                    break;
                                case nameof(userWcsCircle):
                                    userWcsCircle wcsCircle = item as userWcsCircle;
                                    this._refWcsPoint = (new userWcsPoint(wcsCircle.X, wcsCircle.Y, wcsCircle.Z, wcsCircle.Grab_x, wcsCircle.Grab_y, wcsCircle.CamParams));
                                    break;
                                case nameof(userWcsCoordSystem):
                                    userWcsCoordSystem coordSystem = item as userWcsCoordSystem;
                                    this._refWcsPoint = (new userWcsPoint(coordSystem.CurrentPoint.X, coordSystem.CurrentPoint.Y, coordSystem.CurrentPoint.Z,
                                        coordSystem.CurrentPoint.Grab_x,
                                        coordSystem.CurrentPoint.Grab_y,
                                        coordSystem.CurrentPoint.CamParams));
                                    break;
                            }
                        }
                    }
                    else
                        this._refWcsPoint = null;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return this._refWcsPoint;
            }
            set { this._refWcsPoint = value; }
        }

        [DisplayName("搜索点")]
        [DescriptionAttribute("输入属性2")]
        public userWcsPoint[] SearchWcsPoint
        {
            get
            {
                try
                {
                    if (this.RefSource2.Count > 0)
                    {
                        var oo = this.GetPropertyValue(this.RefSource2);
                        List<userWcsPoint> listPoint = new List<userWcsPoint>();
                        foreach (var item in oo)
                        {
                            switch (item.GetType().Name)
                            {
                                case nameof(userWcsLine):
                                    userWcsLine wcsLine = item as userWcsLine;
                                    listPoint.Add(new userWcsPoint(wcsLine.X1, wcsLine.Y1, wcsLine.Z1, wcsLine.Grab_x, wcsLine.Grab_y, wcsLine.CamParams));
                                    listPoint.Add(new userWcsPoint(wcsLine.X2, wcsLine.Y2, wcsLine.Z2, wcsLine.Grab_x, wcsLine.Grab_y, wcsLine.CamParams));
                                    break;
                                case nameof(userWcsPoint):
                                    userWcsPoint wcsPoint = item as userWcsPoint;
                                    listPoint.Add(new userWcsPoint(wcsPoint.X, wcsPoint.Y, wcsPoint.Z, wcsPoint.Grab_x, wcsPoint.Grab_y, wcsPoint.CamParams));
                                    break;
                                case nameof(userWcsRectangle2):
                                    userWcsRectangle2 wcsRect2 = item as userWcsRectangle2;
                                    listPoint.Add(new userWcsPoint(wcsRect2.X, wcsRect2.Y, wcsRect2.Z, wcsRect2.Grab_x, wcsRect2.Grab_y, wcsRect2.CamParams));
                                    break;
                                case nameof(userWcsCircle):
                                    userWcsCircle wcsCircle = item as userWcsCircle;
                                    listPoint.Add(new userWcsPoint(wcsCircle.X, wcsCircle.Y, wcsCircle.Z, wcsCircle.Grab_x, wcsCircle.Grab_y, wcsCircle.CamParams));
                                    break;
                                case nameof(userWcsEllipse):
                                    userWcsEllipse wcsEllipse = item as userWcsEllipse;
                                    listPoint.Add(new userWcsPoint(wcsEllipse.X, wcsEllipse.Y, wcsEllipse.Z, wcsEllipse.Grab_x, wcsEllipse.Grab_y, wcsEllipse.CamParams));
                                    break;
                                case nameof(userWcsCoordSystem):
                                    userWcsCoordSystem coordSystem = item as userWcsCoordSystem;
                                    listPoint.Add(new userWcsPoint(coordSystem.CurrentPoint.X, coordSystem.CurrentPoint.Y, coordSystem.CurrentPoint.Z,
                                        coordSystem.CurrentPoint.Grab_x,
                                        coordSystem.CurrentPoint.Grab_y,
                                        coordSystem.CurrentPoint.CamParams));
                                    break;
                            }
                        }
                        this._searchWcsPoint = listPoint.ToArray();
                        listPoint.Clear();
                    }
                    else
                        this._searchWcsPoint = null;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return _searchWcsPoint;
            }
            set { _searchWcsPoint = value; }
        }

        public WaferFindDieParam Param
        {
            get;
            set;
        }

        [DisplayName("输出点")]
        [DescriptionAttribute("输出属性")]
        public userWcsPoint WcsOutPoint { get => _wcsOutPoint; set => _wcsOutPoint = value; }


        public WaferFindDie()
        {
            this.Param = new WaferFindDieParam();
            InitBindingTable();
        }
        private void InitBindingTable()
        {
            if (this.ResultInfo == null)
            {
                this.ResultInfo = new BindingList<OcrResultInfo>();
                ((BindingList<OcrResultInfo>)this.ResultInfo).Add(new OcrResultInfo(this.name, "X", ""));
                ((BindingList<OcrResultInfo>)this.ResultInfo).Add(new OcrResultInfo(this.name, "Y", ""));
                ((BindingList<OcrResultInfo>)this.ResultInfo).Add(new OcrResultInfo(this.name, "Z", ""));
            }
        }


        #region  实现接口的部分
        public OperateResult Execute(params object[] param)
        {
            this.Result.Succss = false;
            this.Result.ErrorMessage = "";
            try
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                userWcsArrow wcsArrow;
                this.Result.Succss = WaferFindDieMethod.FindDieMethod(this.RefWcsPoint, this.SearchWcsPoint, this.Param, out this._matchWcsPoint, out this._wcsOutPoint);
                if (this.Result.Succss)
                {
                    switch (this.Param.RefObject)
                    {
                        case enRefObject.示教点:
                        case enRefObject.当前点:
                            wcsArrow = new userWcsArrow(this._refWcsPoint.X, this._refWcsPoint.Y, this._refWcsPoint.Z, this._wcsOutPoint.X, this._wcsOutPoint.Y, this._wcsOutPoint.Z, this._wcsOutPoint.CamParams);
                            wcsArrow.CamName = this._refWcsPoint.CamName;
                            wcsArrow.ViewWindow = this._refWcsPoint.ViewWindow;
                            wcsArrow.Grab_x = this._refWcsPoint.Grab_x;
                            wcsArrow.Grab_y = this._refWcsPoint.Grab_y;
                            wcsArrow.Grab_z = this._refWcsPoint.Grab_z;
                            wcsArrow.Grab_theta = this._refWcsPoint.Grab_theta;
                            break;
                        default:
                            wcsArrow = new userWcsArrow(0, 0, 0, this._wcsOutPoint.X, this._wcsOutPoint.Y, this._wcsOutPoint.Z, this._wcsOutPoint.CamParams);
                            wcsArrow.CamName = this._wcsOutPoint.CamName;
                            wcsArrow.ViewWindow = this._wcsOutPoint.ViewWindow;
                            wcsArrow.Grab_x = this._wcsOutPoint.Grab_x;
                            wcsArrow.Grab_y = this._wcsOutPoint.Grab_y;
                            wcsArrow.Grab_z = this._wcsOutPoint.Grab_z;
                            wcsArrow.Grab_theta = this._wcsOutPoint.Grab_theta;
                            break;
                    }
                }
                else
                {
                    wcsArrow = new userWcsArrow();
                    this._wcsOutPoint = new userWcsPoint();
                }
                this.CreateResultInfo(11);
                stopwatch.Stop();
                ((BindingList<OcrResultInfo>)this.ResultInfo)[0].SetValue(this.name, "X", Math.Round(this._wcsOutPoint.X, 5).ToString());
                ((BindingList<OcrResultInfo>)this.ResultInfo)[1].SetValue(this.name, "Y", Math.Round(this._wcsOutPoint.Y, 5).ToString());
                ((BindingList<OcrResultInfo>)this.ResultInfo)[2].SetValue(this.name, "Z", Math.Round(this._wcsOutPoint.Z, 5).ToString());
                ((BindingList<OcrResultInfo>)this.ResultInfo)[3].SetValue(this.name, "Theta", Math.Round(this._wcsOutPoint.Theta, 5).ToString());
                ((BindingList<OcrResultInfo>)this.ResultInfo)[4].SetValue(this.name, "CurRowIndex", this.Param.DieIndex.CurRowIndex.ToString());
                ((BindingList<OcrResultInfo>)this.ResultInfo)[5].SetValue(this.name, "CurColIndex", this.Param.DieIndex.CurColIndex.ToString());
                ((BindingList<OcrResultInfo>)this.ResultInfo)[6].SetValue(this.name, "TargetRowIndex", this.Param.DieIndex.TargetRowIndex.ToString());
                ((BindingList<OcrResultInfo>)this.ResultInfo)[7].SetValue(this.name, "TargetColIndex", this.Param.DieIndex.TargetColIndex.ToString());
                if (this.Param.DieIndex.CurRowIndex == this.Param.DieIndex.TargetRowIndex && this.Param.DieIndex.CurColIndex == this.Param.DieIndex.TargetColIndex)
                    ((BindingList<OcrResultInfo>)this.ResultInfo)[8].SetValue(this.name, "State", "Done");
                else
                    ((BindingList<OcrResultInfo>)this.ResultInfo)[8].SetValue(this.name, "State", "Continue");
                if (this.Result.Succss)
                    ((BindingList<OcrResultInfo>)this.ResultInfo)[9].SetValue(this.name, "Result", "1");
                else
                    ((BindingList<OcrResultInfo>)this.ResultInfo)[9].SetValue(this.name, "Result", "0");
                ((BindingList<OcrResultInfo>)this.ResultInfo)[10].SetValue(this.name, "Time(ms)", stopwatch.ElapsedMilliseconds.ToString());
                if (this.Param.IsOutputMatchPoint)
                {
                    int i = 0;
                    foreach (var item in this._matchWcsPoint)
                    {
                        if (item != null)
                            OnExcuteCompleted(item.CamName, item.CamParams?.ViewWindow, this.name + i, item);
                        i++;
                    }
                }
                OnExcuteCompleted(wcsArrow.CamName, wcsArrow.ViewWindow, this.name, wcsArrow);
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
            return this.Result;
        }

        public object GetPropertyValues(string propertyName)
        {
            switch (propertyName)
            {
                case "名称":
                case "Name":
                    return this.name;
                case "输入点":
                    return this.SearchWcsPoint;
                case "匹配点":
                    return this._matchWcsPoint;
                case "参考点":
                    return this._refWcsPoint;
                case "目标点":
                    return this._wcsOutPoint;
                case nameof(this.WcsOutPoint):
                    return this.WcsOutPoint; //
                default:
                    return this.WcsOutPoint;
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

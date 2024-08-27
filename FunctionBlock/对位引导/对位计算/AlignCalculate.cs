using AlgorithmsLibrary;
using Common;
using MotionControlCard;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionBlock
{
    [Serializable]
    [DefaultProperty(nameof(AddXYTheta))]
    public class AlignCalculate : BaseFunction, IFunction
    {
        private userWcsVector _addXYTheta;
        [DisplayName("补偿值")]
        [DescriptionAttribute("输出属性")]
        public userWcsVector AddXYTheta { get => this._addXYTheta; set => this._addXYTheta = value; }

        private userWcsVector[] _targetPoint;
        private userWcsVector[] _sourcePoint;
        private userWcsVector[] _affinePoint;
        private userWcsVector[] _targetRotatePoint;
        private userWcsVector[] _sourceRotatePoint;

        [DisplayName("输入目标点")]
        [DescriptionAttribute("输入属性1")]
        public userWcsVector[] TargetPoint
        {
            get
            {
                if (this.RefSource1.Count > 0)
                {
                    object[] oo = this.GetPropertyValue(this.RefSource1);
                    List<userWcsVector> list = new List<userWcsVector>();
                    foreach (var item in oo)
                    {
                        switch (item.GetType().Name)
                        {
                            case nameof(userWcsVector):
                                list.Add(item as userWcsVector);
                                break;
                            case nameof(userWcsCircle):
                                userWcsCircle wcsCircle = item as userWcsCircle;
                                list.Add(new userWcsVector(wcsCircle.X, wcsCircle.Y, wcsCircle.Z, 0, wcsCircle.Grab_x, wcsCircle.Grab_y, wcsCircle.CamParams));
                                break;
                            case nameof(userWcsCircleSector):
                                userWcsCircleSector wcsCircleSector = item as userWcsCircleSector;
                                list.Add(new userWcsVector(wcsCircleSector.X, wcsCircleSector.Y, wcsCircleSector.Z, 0, wcsCircleSector.Grab_x, wcsCircleSector.Grab_y, wcsCircleSector.CamParams));
                                break;
                            case nameof(userWcsRectangle2):
                                userWcsRectangle2 wcsRect2 = item as userWcsRectangle2;
                                list.Add(new userWcsVector(wcsRect2.X, wcsRect2.Y, wcsRect2.Z, 0, wcsRect2.Grab_x, wcsRect2.Grab_y, wcsRect2.CamParams));
                                break;
                            case nameof(userWcsCoordSystem):
                                userWcsCoordSystem wcsCoord = item as userWcsCoordSystem;
                                list.Add(new userWcsVector(wcsCoord.CurrentPoint.X, wcsCoord.CurrentPoint.Y, wcsCoord.CurrentPoint.Z, 0, wcsCoord.CurrentPoint.CamParams));
                                break;
                            case nameof(userWcsPoint):
                                userWcsPoint wcsPoint   = item as userWcsPoint;
                                list.Add(new userWcsVector(wcsPoint.X, wcsPoint.Y, wcsPoint.Z,0, wcsPoint.CamParams));
                                break;
                            default:
                                throw new ArgumentException("参数类型错误");
                        }
                    }
                    this._targetPoint = list.ToArray();
                    list.Clear();
                }
                else
                    this._targetPoint = new userWcsVector[0];
                return this._targetPoint;
            }
            set
            {
                this._targetPoint = value;
            }
        }

        [DisplayName("输入源点")]
        [DescriptionAttribute("输入属性2")]
        public userWcsVector[] SourcePoint
        {
            get
            {
                if (this.RefSource2.Count > 0)
                {
                    object[] oo = this.GetPropertyValue(this.RefSource2);
                    List<userWcsVector> list = new List<userWcsVector>();
                    foreach (var item in oo)
                    {
                        switch (item.GetType().Name)
                        {
                            case nameof(userWcsVector):
                                list.Add(item as userWcsVector);
                                break;
                            case nameof(userWcsCircle):
                                userWcsCircle wcsCircle = item as userWcsCircle;
                                list.Add(new userWcsVector(wcsCircle.X, wcsCircle.Y, wcsCircle.Z, 0, wcsCircle.Grab_x, wcsCircle.Grab_y, wcsCircle.CamParams));
                                break;
                            case nameof(userWcsCircleSector):
                                userWcsCircleSector wcsCircleSector = item as userWcsCircleSector;
                                list.Add(new userWcsVector(wcsCircleSector.X, wcsCircleSector.Y, wcsCircleSector.Z, 0, wcsCircleSector.Grab_x, wcsCircleSector.Grab_y, wcsCircleSector.CamParams));
                                break;
                            case nameof(userWcsRectangle2):
                                userWcsRectangle2 wcsRect2 = item as userWcsRectangle2;
                                list.Add(new userWcsVector(wcsRect2.X, wcsRect2.Y, wcsRect2.Z, 0, wcsRect2.Grab_x, wcsRect2.Grab_y, wcsRect2.CamParams));
                                break;
                            case nameof(userWcsCoordSystem):
                                userWcsCoordSystem wcsCoord = item as userWcsCoordSystem;
                                list.Add(new userWcsVector(wcsCoord.CurrentPoint.X, wcsCoord.CurrentPoint.Y, wcsCoord.CurrentPoint.Z, 0, wcsCoord.CurrentPoint.CamParams));
                                break;
                            case nameof(userWcsPoint):
                                userWcsPoint wcsPoint  = item as userWcsPoint;
                                list.Add(new userWcsVector(wcsPoint.X, wcsPoint.Y, wcsPoint.Z,0, wcsPoint.CamParams));
                                break;
                            default:
                                throw new ArgumentException("参数类型错误");
                        }
                    }
                    this._sourcePoint = list.ToArray();
                    list.Clear();
                }
                else
                    this._sourcePoint = new userWcsVector[0];
                return this._sourcePoint;
            }
            set
            {
                this._sourcePoint = value;
            }
        }

        [DisplayName("变换点")]
        [DescriptionAttribute("输出属性")]
        public userWcsVector[] AffinePoint
        {
            get
            {
                return this._affinePoint;
            }
            set
            {
                this._affinePoint = value;
            }
        }
        public CompensationParam Param { get; set; }

        [DisplayName("目标旋转点")]
        [DescriptionAttribute("输出属性")]
        public userWcsVector[] TargetRotatePoint 
        {
            get => _targetRotatePoint; 
            set => _targetRotatePoint = value; 
        }

        [DisplayName("源旋转点")]
        [DescriptionAttribute("输出属性")]
        public userWcsVector[] SourceRotatePoint 
        { 
            get => _sourceRotatePoint;
            set => _sourceRotatePoint = value;
        }


        public AlignCalculate()
        {
            this.Param = new CompensationParam();
            //////////////////////////////////////////////////////////////////////////////
            this.ResultInfo = new BindingList<MeasureResultInfo>();
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo());
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo());
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo());
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo());
            //((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo());
            //((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo());
            //((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo());
        }


        public OperateResult Execute(params object[] param)
        {
            this.Result.Succss = true;
            try
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                Result.Succss = AlignMethod.CalculateAlign2(this.TargetPoint, this.SourcePoint, this.Param,out _affinePoint, out this._addXYTheta);
                stopwatch.Stop();
                ////////////////////////// 补偿值 //////////////////////////////////////////////////////////////////
                //((BindingList<MeasureResultInfo>)this.ResultInfo)[0].SetValue(this.name, "Compensation_x", this.Param.Add_X);
                //((BindingList<MeasureResultInfo>)this.ResultInfo)[1].SetValue(this.name, "Compensation_y", this.Param.Add_Y);
                //((BindingList<MeasureResultInfo>)this.ResultInfo)[2].SetValue(this.name, "Compensation_angle", this.Param.Add_Angle);
                /////////////////////偏移值 //////////////////////////////////////////////////////////////////
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[0].SetValue(this.name, "Add_x", this._addXYTheta.X);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[1].SetValue(this.name, "Add_y", this._addXYTheta.Y);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[2].SetValue(this.name, "Add_theta", this._addXYTheta.Angle);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[3].SetValue(this.name, "Time(ms)", stopwatch.ElapsedMilliseconds);
                OnExcuteCompleted("补偿值", this.Param?.ViewWindow, this.name, this._addXYTheta);
                OnExcuteCompleted("目标位置", this.Param?.ViewWindow, this.name, this._targetPoint);
                OnExcuteCompleted("当前位置", this.Param?.ViewWindow, this.name, this._sourcePoint);
            }
            catch (Exception ex)
            {
                Result.Succss = false;
                LoggerHelper.Error(this.name + "->执行错误" + ex);
                return Result;
            }
            if (Result.Succss)
                LoggerHelper.Info(this.name + "->执行成功");
            else
                LoggerHelper.Error(this.name + "->执行失败");
            // 更改UI字体　
            UpdataNodeElementStyle(param, Result.Succss);
            return Result;
        }
        public object GetPropertyValues(string propertyName)
        {
            switch (propertyName)
            {
                case "名称":
                case nameof(this.Name):
                    return this.name;
                default:
                    return this.AddXYTheta;
            }
        }
        public bool SetPropertyValues(string propertyName, params object[] value)
        {
            switch (propertyName)
            {
                case "名称":
                case nameof(this.Name):
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
            catch (Exception ex)
            {
                LoggerHelper.Error(this.name + "删除节点出错" + ex.ToString());
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

    }
}

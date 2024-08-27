using AlgorithmsLibrary;
using Common;
using MotionControlCard;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionBlock
{
    [Serializable]
    [DefaultProperty(nameof(AddXYTheta))] 
    public class AlignmentGuidedCalculation : BaseFunction, IFunction
    {
        private userWcsVector _addXYTheta;

        [DisplayName("对位补偿值")]
        [DescriptionAttribute("输出属性")]
        public userWcsVector AddXYTheta { get=> this._addXYTheta; set=>this._addXYTheta = value; }

        private userWcsPoint[] _refPoint;
        private userWcsPoint[] _curPoint;

        [DisplayName("输入参考点")]
        [DescriptionAttribute("输入属性1")]
        public userWcsPoint[] RefPoint
        {
            get
            {
                if (this.RefSource1.Count > 0)
                {
                    object[] oo = this.GetPropertyValue(this.RefSource1);
                    List<userWcsPoint> list = new List<userWcsPoint>();
                    foreach (var item in oo)
                    {
                        switch (item.GetType().Name)
                        {
                            case nameof(userWcsPoint):
                                list.Add(item as userWcsPoint);
                                break;
                            case nameof(userWcsCircle):
                                userWcsCircle wcsCircle = item as userWcsCircle;
                                list.Add(new userWcsPoint(wcsCircle.X, wcsCircle.Y, wcsCircle.Z, wcsCircle.Grab_x, wcsCircle.Grab_y, wcsCircle.CamParams));
                                break;
                            case nameof(userWcsCircleSector):
                                userWcsCircleSector wcsCircleSector = item as userWcsCircleSector;
                                list.Add(new userWcsPoint(wcsCircleSector.X, wcsCircleSector.Y, wcsCircleSector.Z, wcsCircleSector.Grab_x, wcsCircleSector.Grab_y, wcsCircleSector.CamParams));
                                break;
                            case nameof(userWcsRectangle2):
                                userWcsRectangle2 wcsRect2 = item as userWcsRectangle2;
                                list.Add(new userWcsPoint(wcsRect2.X, wcsRect2.Y, wcsRect2.Z, wcsRect2.Grab_x, wcsRect2.Grab_y, wcsRect2.CamParams));
                                break;
                            case nameof(userWcsCoordSystem):
                                userWcsCoordSystem wcsCoord = item as userWcsCoordSystem;
                                list.Add(new userWcsPoint(wcsCoord.CurrentPoint.X, wcsCoord.CurrentPoint.Y, wcsCoord.CurrentPoint.Z, wcsCoord.CurrentPoint.CamParams));
                                break;
                            default:
                                throw new ArgumentException("参数类型错误");
                        }
                    }
                    this._refPoint = list.ToArray();
                    list.Clear();
                }
                else
                    this._refPoint = new userWcsPoint[0];
                return this._refPoint;
            }
            set
            {
                this._refPoint = value;
            }
        }

        [DisplayName("输入当前点")]
        [DescriptionAttribute("输入属性2")]
        public userWcsPoint[] CurPoint
        {
            get
            {
                if (this.RefSource2.Count > 0)
                {
                    object[] oo = this.GetPropertyValue(this.RefSource2);
                    List<userWcsPoint> list = new List<userWcsPoint>();
                    foreach (var item in oo)
                    {
                        switch (item.GetType().Name)
                        {
                            case nameof(userWcsPoint):
                                list.Add(item as userWcsPoint);
                                break;
                            case nameof(userWcsCircle):
                                userWcsCircle wcsCircle = item as userWcsCircle;
                                list.Add(new userWcsPoint(wcsCircle.X, wcsCircle.Y, wcsCircle.Z, wcsCircle.Grab_x, wcsCircle.Grab_y, wcsCircle.CamParams));
                                break;
                            case nameof(userWcsCircleSector):
                                userWcsCircleSector wcsCircleSector = item as userWcsCircleSector;
                                list.Add(new userWcsPoint(wcsCircleSector.X, wcsCircleSector.Y, wcsCircleSector.Z, wcsCircleSector.Grab_x, wcsCircleSector.Grab_y, wcsCircleSector.CamParams));
                                break;
                            case nameof(userWcsRectangle2):
                                userWcsRectangle2 wcsRect2 = item as userWcsRectangle2;
                                list.Add(new userWcsPoint(wcsRect2.X, wcsRect2.Y, wcsRect2.Z, wcsRect2.Grab_x, wcsRect2.Grab_y, wcsRect2.CamParams));
                                break;
                            case nameof(userWcsCoordSystem):
                                userWcsCoordSystem wcsCoord = item as userWcsCoordSystem;
                                list.Add(new userWcsPoint(wcsCoord.CurrentPoint.X, wcsCoord.CurrentPoint.Y, wcsCoord.CurrentPoint.Z, wcsCoord.CurrentPoint.CamParams));
                                break;
                            default:
                                throw new ArgumentException("参数类型错误");
                        }
                    }
                    this._curPoint = list.ToArray();
                    list.Clear();
                }
                else
                    this._curPoint = new userWcsPoint[0];
                return this._curPoint;
            }
            set
            {
                this._curPoint = value;
            }
        }

        public AlignmentGuidedParam Param { get; set; }



        public AlignmentGuidedCalculation()
        {
            this.Param = new AlignmentGuidedParam();
            //////////////////////////////////////////////////////////////////////////////
            this.ResultInfo = new BindingList<MeasureResultInfo>();
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo());
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo());
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo());
        }


        public OperateResult Execute(params object[] param)
        {
            this.Result.Succss = false;
            try
            {
                Result.Succss = AlignmentGuidedMethod.CalculateOffset(this.RefPoint, this.CurPoint, this.Param, out this._addXYTheta);
                /////////////////////////////
                // 偏移值
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[0].SetValue(this.name, "AddTx", this.AddXYTheta.X);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[1].SetValue(this.name, "AddTy", this.AddXYTheta.Y);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[2].SetValue(this.name, "AddTheta", this.AddXYTheta.Angle);
            }
            catch (Exception ex)
            {
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
                default:
                case "名称":
                    return this.name;
                    //default:
                    //    return this.FeaturePoint;
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

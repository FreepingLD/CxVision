using Common;
using HalconDotNet;
using MotionControlCard;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FunctionBlock
{
    [Serializable]
    [DefaultProperty(nameof(AddXYTheta))]
    public class RobotLoad : BaseFunction, IFunction
    {
        private userWcsVector _wcsVector;
        private userWcsVector _addXYTheta;
        private UserTryPlateHoleParam[] _tryPlatformParam;
        private int _jawHole = 0;

        [DisplayName("补偿值")]
        [DescriptionAttribute("输出属性")]
        public userWcsVector AddXYTheta { get => this._addXYTheta; set => this._addXYTheta = value; }

        [DisplayName("输入图像")]
        [DescriptionAttribute("输入属性1")]
        public userWcsVector WcsVector
        {
            get
            {
                try
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
                                    case nameof(userWcsVector):
                                        this._wcsVector = item as userWcsVector;
                                        break;
                                    case nameof(userWcsPoint):
                                        userWcsPoint wcsPoint = item as userWcsPoint;
                                        this._wcsVector = new userWcsVector(wcsPoint.X, wcsPoint.Y, wcsPoint.Z, 0, wcsPoint.CamParams);
                                        break;
                                    case nameof(userWcsCoordSystem):
                                        userWcsCoordSystem coordSystem = item as userWcsCoordSystem;
                                        this._wcsVector = new userWcsVector(coordSystem.CurrentPoint.X, coordSystem.CurrentPoint.Y, coordSystem.CurrentPoint.Z, coordSystem.CurrentPoint.Angle, coordSystem.CurrentPoint.CamParams);
                                        break;
                                    case nameof(userWcsRectangle2):
                                        userWcsRectangle2 wcsRect2 = item as userWcsRectangle2;
                                        this._wcsVector = new userWcsVector(wcsRect2.X, wcsRect2.Y, wcsRect2.Z, wcsRect2.Deg, wcsRect2.CamParams);
                                        break;
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return _wcsVector;
            }
            set
            {
                _wcsVector = value;
            }
        }

        [DisplayName("穴位参数")]
        [DescriptionAttribute("输入属性2")]
        public UserTryPlateHoleParam[] TryPlatformParam
        {
            get
            {
                if (this.RefSource2.Count > 0)
                {
                    object[] oo = this.GetPropertyValue(this.RefSource2);
                    foreach (var item in oo)
                    {
                        switch (item.GetType().Name)
                        {
                            case "UserTryPlatformParam[]":
                                _tryPlatformParam = item as UserTryPlateHoleParam[];
                                break;
                            case "UserTryPlatformParam":
                                _tryPlatformParam = new UserTryPlateHoleParam[1] { item as UserTryPlateHoleParam };
                                break;
                            default:
                                throw new ArgumentException("参数类型错误");
                        }
                    }
                }
                else
                    this._tryPlatformParam = new UserTryPlateHoleParam[0];
                return this._tryPlatformParam;
            }
            set
            {
                this._tryPlatformParam = value;
            }
        }


        public RobotParam Param { get; set; }



        public RobotLoad()
        {
            this.Param = new RobotParam();
            //////////////////////////////////////////////////////////////////////////////
            this.ResultInfo = new BindingList<MeasureResultInfo>();
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo());
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo());
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo());
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo());
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo());
        }


        public OperateResult Execute(params object[] param)
        {
            this.Result.Succss = true;
            try
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                HXLDCont hXLDCont = new HXLDCont();
                UserTryPlateHoleParam HoleParam;
                Result.Succss = RobotLayOffMethod.JawLoadPose(this.WcsVector, this.TryPlatformParam, this.Param, out this._addXYTheta, out HoleParam);
                this._jawHole = 0;
                int.TryParse(HoleParam.Describe.Replace("穴位", ""), out this._jawHole);
                stopwatch.Stop();
                ////////////////////////// 补偿值 //////////////////////////////////////////////////////////////////
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[0].SetValue(this.name, "Add_x", this._addXYTheta.X);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[0].Std_Value = HoleParam.X;
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[0].LimitUp = Math.Abs(Param.LimitP_X);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[0].LimitDown = Math.Abs(Param.LimitN_X);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[1].SetValue(this.name, "Add_y", this._addXYTheta.Y);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[1].Std_Value = HoleParam.Y;
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[1].LimitUp = Math.Abs(Param.LimitP_Y);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[1].LimitDown = Math.Abs(Param.LimitN_Y);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[2].SetValue(this.name, "Add_theta", this._addXYTheta.Angle);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[2].Std_Value = HoleParam.Angle;
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[2].LimitUp = Math.Abs(Param.LimitP_Angle);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[2].LimitDown = Math.Abs(Param.LimitN_Angle);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[3].SetValue(this.name, "夹抓穴位", this._jawHole);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[3].Std_Value = 1;
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[4].SetValue(this.name, "Time(ms)", stopwatch.ElapsedMilliseconds);
                OnExcuteCompleted(this._wcsVector.CamName, this._wcsVector.ViewWindow, this.name, hXLDCont);
                OnExcuteCompleted("", this.Param.ViewWindow, this.name + "参数", this.Param);
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

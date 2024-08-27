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
    public class RobotLayOff : BaseFunction, IFunction
    {
        [NonSerialized]
        private ImageDataClass _imageData;
        private userWcsVector _addXYTheta;
        private UserTryPlateHoleParam[] _tryPlatformParam;
        private int _jawHole = 0;

        [DisplayName("补偿值")]
        [DescriptionAttribute("输出属性")]
        public userWcsVector AddXYTheta { get => this._addXYTheta; set => this._addXYTheta = value; }

        [DisplayName("输入图像")]
        [DescriptionAttribute("输入属性1")]
        public ImageDataClass ImageData
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
                                    case nameof(ImageDataClass):
                                        this._imageData = item as ImageDataClass;
                                        break;
                                    case nameof(HImage):
                                        this._imageData = new ImageDataClass((HImage)item);
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
                return _imageData;
            }
            set
            {
                _imageData = value;
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



        public RobotLayOff()
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
                Result.Succss = RobotLayOffMethod.JawLayOffPose(this.ImageData, this.TryPlatformParam, this.Param, out this._addXYTheta, out hXLDCont, out HoleParam);
                this._jawHole = 0;
                int.TryParse(HoleParam.Describe.Replace("穴位", ""), out this._jawHole);
                stopwatch.Stop();
                ////////////////////////// 补偿值 //////////////////////////////////////////////////////////////////
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[0].SetValue(this.name, "Add_x", this._addXYTheta.X);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[0].Std_Value = HoleParam.X;
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[0].LimitUp = Math.Abs(HoleParam.LimitX);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[0].LimitDown = Math.Abs(HoleParam.LimitX) * -1;
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[1].SetValue(this.name, "Add_y", this._addXYTheta.Y);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[1].Std_Value = HoleParam.Y;
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[1].LimitUp = Math.Abs(HoleParam.LimitY);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[1].LimitDown = Math.Abs(HoleParam.LimitY) * -1;
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[2].SetValue(this.name, "Add_theta", this._addXYTheta.Angle);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[2].Std_Value = HoleParam.Angle;
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[2].LimitUp = Math.Abs(HoleParam.LimitAngle);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[2].LimitDown = Math.Abs(HoleParam.LimitAngle) * -1;
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[3].SetValue(this.name, "夹抓穴位", this._jawHole);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[3].Std_Value = 1;
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[4].SetValue(this.name, "Time(ms)", stopwatch.ElapsedMilliseconds);
                OnExcuteCompleted(this._imageData?.CamName, this._imageData?.ViewWindow, this.name, hXLDCont);
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

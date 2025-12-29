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
    [DefaultProperty(nameof(AffinePoint))]
    public class PointAffine : BaseFunction, IFunction
    {
        private userWcsPoint _affinePoint;

        private userWcsPoint _WcsPoint;

        public UserHomMat2D _homMat2D;

       [DisplayName("输出点")]
        [DescriptionAttribute("输出属性")]
        public userWcsPoint AffinePoint { get => _affinePoint; set => _affinePoint = value; }


        [DisplayName("输入点")]
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
                                case nameof(userWcsPoint):
                                    this._WcsPoint = item as userWcsPoint;
                                    break;
                            }
                        }
                    }
                }
                return this._WcsPoint;
            }
            set
            {
                this._WcsPoint = value;
            }
        }

        [DisplayName("变换矩阵")]
        [DescriptionAttribute("输入属性2")]
        public UserHomMat2D HomMat2D
        {
            get
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
                                case nameof(UserHomMat2D):
                                    this._homMat2D = item as UserHomMat2D;
                                    break;
                                case nameof(HHomMat2D):
                                    this._homMat2D = new UserHomMat2D(item as HHomMat2D);
                                    break;
                            }
                        }
                    }
                }
                this._homMat2D = new UserHomMat2D();
                return this._homMat2D;
            }
            set
            {
                this._homMat2D = value;
            }
        }

        public enRobotJawEnum Jaw { get; set; }


        public PointAffine()
        {
            this.Jaw = enRobotJawEnum.NONE;
            this.ResultInfo = new BindingList<MeasureResultInfo>();
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo());
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo());
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo());
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo());
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo());
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo());
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo());
        }


        #region 实现接口

        public OperateResult Execute(params object[] param)
        {
            this.Result.Succss = true;
            try
            {
                string index = "";
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
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
                        }
                    }
                }
                if (this.WcsPoint != null)
                {
                    RobotJawParam JawParam = RobotJawParaManager.Instance.GetJawParam(this.Jaw);
                    this._homMat2D = new UserHomMat2D(JawParam.GetHomMat2D());
                    this._affinePoint = this.WcsPoint?.AffineWcsPoint(this._homMat2D.GetHTuple());
                    this.Result.Succss = true;
                }
                else
                    this.Result.Succss = false;
                this.Result.Succss = true;
                stopwatch.Stop();
                ///////////////////////////////////////////
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[0].SetValue(this.name, "X", this._affinePoint.X);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[1].SetValue(this.name, "Y", this._affinePoint.Y);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[2].SetValue(this.name, "Z", this._affinePoint.Z);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[3].SetValue(this.name, "Grab_x", this._affinePoint.Grab_x);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[4].SetValue(this.name, "Grab_y", this._affinePoint.Grab_y);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[5].SetValue(this.name, "Grab_theta", this._affinePoint.Grab_theta);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[6].SetValue(this.name, "时间(ms)", stopwatch.ElapsedMilliseconds);
                ////////////////////////////////////////////
                OnExcuteCompleted(this.WcsPoint.CamName, this.WcsPoint?.ViewWindow, this.name + index, this._affinePoint);
            }
            catch (Exception ex)
            {
                LoggerHelper.Error(this.name + "->执行错误", ex);
                this.Result.Succss = false;
            }
            if (this.Result.Succss)
                LoggerHelper.Info(this.name + "->执行成功：" + this._affinePoint.ToString());
            else
                LoggerHelper.Error(this.name + "->执行失败：" + this._affinePoint.ToString());
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
                case nameof(this.AffinePoint):
                    return this.AffinePoint; //
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

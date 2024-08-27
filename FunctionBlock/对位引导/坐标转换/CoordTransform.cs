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
    [DefaultProperty(nameof(HomMat2D))]
    public class CoordTransform : BaseFunction, IFunction
    {

        private userWcsPoint _WcsPoint;

        public UserHomMat2D _homMat2D;

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
                                case nameof(userWcsCircle):
                                    userWcsCircle wcsCircle = item as userWcsCircle;
                                    this._WcsPoint = new userWcsPoint(wcsCircle.X, wcsCircle.Y, wcsCircle.Z, wcsCircle.CamParams);
                                    break;
                                case nameof(userWcsCircleSector):
                                    userWcsCircleSector wcsCircleSector  = item as userWcsCircleSector;
                                    this._WcsPoint = new userWcsPoint(wcsCircleSector.X, wcsCircleSector.Y, wcsCircleSector.Z, wcsCircleSector.CamParams);
                                    break;
                                case nameof(userWcsVector):
                                    userWcsVector wcsVector  = item as userWcsVector;
                                    this._WcsPoint = new userWcsPoint(wcsVector.X, wcsVector.Y, wcsVector.Z, wcsVector.CamParams);
                                    break;
                                case nameof(userWcsEllipse):
                                    userWcsEllipse wcsEllipse  = item as userWcsEllipse;
                                    this._WcsPoint = new userWcsPoint(wcsEllipse.X, wcsEllipse.Y, wcsEllipse.Z, wcsEllipse.CamParams);
                                    break;
                                case nameof(userWcsEllipseSector):
                                    userWcsEllipseSector wcsEllipseSector  = item as userWcsEllipseSector;
                                    this._WcsPoint = new userWcsPoint(wcsEllipseSector.X, wcsEllipseSector.Y, wcsEllipseSector.Z, wcsEllipseSector.CamParams);
                                    break;
                                case nameof(userWcsRectangle2):
                                    userWcsRectangle2 wcsRectangle2  = item as userWcsRectangle2;
                                    this._WcsPoint = new userWcsPoint(wcsRectangle2.X, wcsRectangle2.Y, wcsRectangle2.Z, wcsRectangle2.CamParams);
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
        [DescriptionAttribute("输出属性")]
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

        public GuidedCalibParam Param { get; set; }

        public CoordTransform()
        {
            this.Param = new GuidedCalibParam();
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
            RobotJawParam JawParam = new RobotJawParam();
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
                    //this.Result.Succss = this.Param.calculate(this.WcsPoint);
                    //JawParam = RobotJawParaManager.Instance.GetJawParam(this.Param.Jaw);
                    this._homMat2D = new UserHomMat2D(JawParam.GetHomMat2D());
                }
                else
                    this.Result.Succss = false;
                stopwatch.Stop();
                ///////////////////////////////////////////
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[0].SetValue(this.name, "X", JawParam.X);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[1].SetValue(this.name, "Y", JawParam.Y);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[2].SetValue(this.name, "Z", JawParam.Z);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[3].SetValue(this.name, "Angle", JawParam.Angle);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[4].SetValue(this.name, "U", JawParam.U);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[5].SetValue(this.name, "V", JawParam.V);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[6].SetValue(this.name, "Time(ms)", stopwatch.ElapsedMilliseconds);
                ////////////////////////////////////////////
                //OnExcuteCompleted(this.WcsPoint.CamName, this.WcsPoint?.ViewWindow, this.name + index, this._affinePoint);
            }
            catch (Exception ex)
            {
                LoggerHelper.Error(this.name + "->执行错误", ex);
                this.Result.Succss = false;
            }
            if (this.Result.Succss)
                LoggerHelper.Info(this.name + "->执行成功：" + JawParam.ToString());
            else
                LoggerHelper.Error(this.name + "->执行失败：" + JawParam.ToString());
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
                case "变换矩阵":
                default:
                case nameof(this.HomMat2D):
                    return this.HomMat2D; //
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

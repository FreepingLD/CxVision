using Common;
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
    public class VectorAlign : BaseFunction, IFunction
    {
        private userWcsVector _addXYTheta;
        [DisplayName("补偿值")]
        [DescriptionAttribute("输出属性")]
        public userWcsVector AddXYTheta { get => this._addXYTheta; set => this._addXYTheta = value; }

        private userPixVector _plateTeachVector;
        private userPixVector _plateCurVector;
        private userPixVector _bandTeachVector;
        private userPixVector _bandCurVector;


        [DisplayName("补偿平台示教向量")]
        [DescriptionAttribute("输入属性1")]
        public userPixVector PlateTeachVector
        {
            get
            {
                if (this.RefSource1.Count > 0)
                {
                    object[] oo = this.GetPropertyValue(this.RefSource1);
                    foreach (var item in oo)
                    {
                        switch (item.GetType().Name)
                        {
                            case nameof(userPixVector):
                                _plateTeachVector = (item as userPixVector);
                                break;
                            case nameof(userWcsVector):
                                _plateTeachVector = (item as userWcsVector).GetPixVector();
                                break;
                            case nameof(userWcsCircle):
                                userPixCircle pixCircle = (item as userWcsCircle).GetPixCircle();
                                _plateTeachVector = new userPixVector(pixCircle.Row, pixCircle.Col, 0, pixCircle.Grab_x, pixCircle.Grab_y, pixCircle.CamParams);
                                break;
                            case nameof(userWcsCircleSector):
                                userPixCircleSector pixCircleSector = (item as userWcsCircleSector).GetPixCircleSector();
                                _plateTeachVector = new userPixVector(pixCircleSector.Row, pixCircleSector.Col, 0, pixCircleSector.Grab_x, pixCircleSector.Grab_y, pixCircleSector.CamParams);
                                break;
                            case nameof(userWcsRectangle2):
                                userPixRectangle2 wcsRect2 = (item as userWcsRectangle2).GetPixRectangle2();
                                _plateTeachVector = new userPixVector(wcsRect2.Row, wcsRect2.Col,  0, wcsRect2.Grab_x, wcsRect2.Grab_y, wcsRect2.CamParams);
                                break;
                            case nameof(userWcsCoordSystem):
                                userPixCoordSystem wcsCoord = (item as userWcsCoordSystem).GetPixCoordSystem();
                                _plateTeachVector = new userPixVector(wcsCoord.CurrentPoint.Row, wcsCoord.CurrentPoint.Col, 0, wcsCoord.CurrentPoint.Grab_x, wcsCoord.CurrentPoint.Grab_y, wcsCoord.CurrentPoint.CamParams);
                                break;
                            case nameof(userWcsPoint):
                                userPixPoint wcsPoint = (item as userWcsPoint).GetPixPoint();
                                _plateTeachVector = new userPixVector(wcsPoint.Row, wcsPoint.Col, 0, wcsPoint.Grab_x, wcsPoint.Grab_y, wcsPoint.CamParams);
                                break;
                            default:
                                throw new ArgumentException("参数类型错误");
                        }
                    }
                }
                else
                    this._plateTeachVector = new userPixVector();
                return this._plateTeachVector;
            }
            set
            {
                this._plateTeachVector = value;
            }
        }

        [DisplayName("补偿平台当前向量")]
        [DescriptionAttribute("输入属性2")]
        public userPixVector PlateCurVector
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
                            case nameof(userPixVector):
                                _plateCurVector = (item as userPixVector);
                                break;
                            case nameof(userWcsVector):
                                _plateCurVector = (item as userWcsVector).GetPixVector();
                                break;
                            case nameof(userWcsCircle):
                                userPixCircle pixCircle = (item as userWcsCircle).GetPixCircle();
                                _plateCurVector = new userPixVector(pixCircle.Row, pixCircle.Col, 0, pixCircle.Grab_x, pixCircle.Grab_y, pixCircle.CamParams);
                                break;
                            case nameof(userWcsCircleSector):
                                userPixCircleSector pixCircleSector = (item as userWcsCircleSector).GetPixCircleSector();
                                _plateCurVector = new userPixVector(pixCircleSector.Row, pixCircleSector.Col, 0, pixCircleSector.Grab_x, pixCircleSector.Grab_y, pixCircleSector.CamParams);
                                break;
                            case nameof(userWcsRectangle2):
                                userPixRectangle2 wcsRect2 = (item as userWcsRectangle2).GetPixRectangle2();
                                _plateCurVector = new userPixVector(wcsRect2.Row, wcsRect2.Col, 0, wcsRect2.Grab_x, wcsRect2.Grab_y, wcsRect2.CamParams);
                                break;
                            case nameof(userWcsCoordSystem):
                                userPixCoordSystem wcsCoord = (item as userWcsCoordSystem).GetPixCoordSystem();
                                _plateCurVector = new userPixVector(wcsCoord.CurrentPoint.Row, wcsCoord.CurrentPoint.Col, 0, wcsCoord.CurrentPoint.Grab_x, wcsCoord.CurrentPoint.Grab_y, wcsCoord.CurrentPoint.CamParams);
                                break;
                            case nameof(userWcsPoint):
                                userPixPoint wcsPoint = (item as userWcsPoint).GetPixPoint();
                                _plateCurVector = new userPixVector(wcsPoint.Row, wcsPoint.Col, 0, wcsPoint.Grab_x, wcsPoint.Grab_y, wcsPoint.CamParams);
                                break;
                            default:
                                throw new ArgumentException("参数类型错误");
                        }
                    }
                }
                else
                    this._plateCurVector = new userPixVector();
                return this._plateCurVector;
            }
            set
            {
                this._plateCurVector = value;
            }
        }

        [DisplayName("固定平台示教向量")]
        [DescriptionAttribute("输入属性3")]
        public userPixVector BandTeachVector
        {
            get
            {
                if (this.RefSource3.Count > 0)
                {
                    object[] oo = this.GetPropertyValue(this.RefSource3);
                    foreach (var item in oo)
                    {
                        switch (item.GetType().Name)
                        {
                            case nameof(userPixVector):
                                _bandTeachVector = (item as userPixVector);
                                break;
                            case nameof(userWcsVector):
                                _bandTeachVector = (item as userWcsVector).GetPixVector();
                                break;
                            case nameof(userWcsCircle):
                                userPixCircle pixCircle = (item as userWcsCircle).GetPixCircle();
                                _bandTeachVector = new userPixVector(pixCircle.Row, pixCircle.Col, 0, pixCircle.Grab_x, pixCircle.Grab_y, pixCircle.CamParams);
                                break;
                            case nameof(userWcsCircleSector):
                                userPixCircleSector pixCircleSector = (item as userWcsCircleSector).GetPixCircleSector();
                                _bandTeachVector = new userPixVector(pixCircleSector.Row, pixCircleSector.Col, 0, pixCircleSector.Grab_x, pixCircleSector.Grab_y, pixCircleSector.CamParams);
                                break;
                            case nameof(userWcsRectangle2):
                                userPixRectangle2 wcsRect2 = (item as userWcsRectangle2).GetPixRectangle2();
                                _bandTeachVector = new userPixVector(wcsRect2.Row, wcsRect2.Col, 0, wcsRect2.Grab_x, wcsRect2.Grab_y, wcsRect2.CamParams);
                                break;
                            case nameof(userWcsCoordSystem):
                                userPixCoordSystem wcsCoord = (item as userWcsCoordSystem).GetPixCoordSystem();
                                _bandTeachVector = new userPixVector(wcsCoord.CurrentPoint.Row, wcsCoord.CurrentPoint.Col, 0, wcsCoord.CurrentPoint.Grab_x, wcsCoord.CurrentPoint.Grab_y, wcsCoord.CurrentPoint.CamParams);
                                break;
                            case nameof(userWcsPoint):
                                userPixPoint wcsPoint = (item as userWcsPoint).GetPixPoint();
                                _bandTeachVector = new userPixVector(wcsPoint.Row, wcsPoint.Col, 0, wcsPoint.Grab_x, wcsPoint.Grab_y, wcsPoint.CamParams);
                                break;
                            default:
                                throw new ArgumentException("参数类型错误");
                        }
                    }
                }
                else
                    this._bandTeachVector = new userPixVector();
                return this._bandTeachVector;
            }
            set
            {
                this._bandTeachVector = value;
            }
        }

        [DisplayName("固定平台当前向量")]
        [DescriptionAttribute("输入属性4")]
        public userPixVector BandCurVector
        {
            get
            {
                if (this.RefSource4.Count > 0)
                {
                    object[] oo = this.GetPropertyValue(this.RefSource4);
                    foreach (var item in oo)
                    {
                        switch (item.GetType().Name)
                        {
                            case nameof(userPixVector):
                                _bandCurVector = (item as userPixVector);
                                break;
                            case nameof(userWcsVector):
                                _bandCurVector = (item as userWcsVector).GetPixVector();
                                break;
                            case nameof(userWcsCircle):
                                userPixCircle pixCircle = (item as userWcsCircle).GetPixCircle();
                                _bandCurVector = new userPixVector(pixCircle.Row, pixCircle.Col, 0, pixCircle.Grab_x, pixCircle.Grab_y, pixCircle.CamParams);
                                break;
                            case nameof(userWcsCircleSector):
                                userPixCircleSector pixCircleSector = (item as userWcsCircleSector).GetPixCircleSector();
                                _bandCurVector = new userPixVector(pixCircleSector.Row, pixCircleSector.Col, 0, pixCircleSector.Grab_x, pixCircleSector.Grab_y, pixCircleSector.CamParams);
                                break;
                            case nameof(userWcsRectangle2):
                                userPixRectangle2 wcsRect2 = (item as userWcsRectangle2).GetPixRectangle2();
                                _bandCurVector = new userPixVector(wcsRect2.Row, wcsRect2.Col, 0, wcsRect2.Grab_x, wcsRect2.Grab_y, wcsRect2.CamParams);
                                break;
                            case nameof(userWcsCoordSystem):
                                userPixCoordSystem wcsCoord = (item as userWcsCoordSystem).GetPixCoordSystem();
                                _bandCurVector = new userPixVector(wcsCoord.CurrentPoint.Row, wcsCoord.CurrentPoint.Col, 0, wcsCoord.CurrentPoint.Grab_x, wcsCoord.CurrentPoint.Grab_y, wcsCoord.CurrentPoint.CamParams);
                                break;
                            case nameof(userWcsPoint):
                                userPixPoint wcsPoint = (item as userWcsPoint).GetPixPoint();
                                _bandCurVector = new userPixVector(wcsPoint.Row, wcsPoint.Col, 0, wcsPoint.Grab_x, wcsPoint.Grab_y, wcsPoint.CamParams);
                                break;
                            default:
                                throw new ArgumentException("参数类型错误");
                        }
                    }
                }
                else
                    this._bandCurVector = new userPixVector();
                return this._bandCurVector;
            }
            set
            {
                this._bandCurVector = value;
            }
        }


        public CompensationParam Param { get; set; }




        public VectorAlign()
        {
            this.Param = new CompensationParam();
            //////////////////////////////////////////////////////////////////////////////
            this.ResultInfo = new BindingList<MeasureResultInfo>();
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
                Result.Succss = AlignMethod.CalculateAlign(this.PlateTeachVector, this.PlateCurVector, this.BandTeachVector, this.BandCurVector, this.Param, out this._addXYTheta);
                /// 更新示教坐标的拍照位,这是为了将当前拍照坐标用于计算
                this._plateTeachVector.Grab_x = this._plateCurVector.Grab_x;
                this._plateTeachVector.Grab_y = this._plateCurVector.Grab_y;
                this._bandTeachVector.Grab_x = this._bandCurVector.Grab_x;
                this._bandTeachVector.Grab_y = this._bandCurVector.Grab_y;
                /// 输出对位坐标信息
                LoggerHelper.Info(this.name + "->补偿平台示教坐标Pix：" + this._plateTeachVector.ToString());
                LoggerHelper.Info(this.name + "->补偿平台示教坐标Wcs：" + this._plateTeachVector.GetWcsVector().ToString());
                LoggerHelper.Info(this.name + "->补偿平台当前坐标Pix：" + this._plateCurVector.ToString());
                LoggerHelper.Info(this.name + "->补偿平台当前坐标Wcs：" + this._plateCurVector.GetWcsVector().ToString());
                LoggerHelper.Info(this.name + "->固定平台示教坐标Pix：" + this._bandTeachVector.ToString());
                LoggerHelper.Info(this.name + "->固定平台示教坐标Wcs：" + this._bandTeachVector.GetWcsVector().ToString());
                LoggerHelper.Info(this.name + "->固定平台当前坐标Pix：" + this._bandCurVector.ToString());
                LoggerHelper.Info(this.name + "->固定平台当前坐标Wcs：" + this._bandCurVector.GetWcsVector().ToString());
                ////////////////////////// 补偿值 //////////////////////////////////////////////////////////////////
                stopwatch.Stop();
                /////////////////////偏移值 //////////////////////////////////////////////////////////////////
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[0].SetValue(this.name, "Add_x", this.AddXYTheta.X);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[1].SetValue(this.name, "Add_y", this.AddXYTheta.Y);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[2].SetValue(this.name, "Add_theta", this.AddXYTheta.Angle);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[3].SetValue(this.name, "Time(ms)", stopwatch.ElapsedMilliseconds);
                OnExcuteCompleted("CompensationParam", this.Param?.ViewWindow, this.name, this.Param); // 这个主要用于可以在其他地方来修改补偿值
            }
            catch (Exception ex)
            {
                Result.Succss = false;
                LoggerHelper.Error(this.name + "->执行错误" + ex);
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

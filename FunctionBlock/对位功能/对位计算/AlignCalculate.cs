using AlgorithmsLibrary;
using Common;
using MotionControlCard;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FunctionBlock
{
    [Serializable]
    [DefaultProperty(nameof(AddXYTheta))]
    public class AlignCalculate : BaseFunction, IFunction
    {
        [NonSerialized]
        private TreeNode _refNode = null;

        private userWcsVector _addXYTheta;
        [DisplayName("补偿值")]
        [DescriptionAttribute("输出属性")]
        public userWcsVector AddXYTheta { get => this._addXYTheta; set => this._addXYTheta = value; }

        private userPixVector[] _targetPoint;
        private userPixVector[] _sourcePoint;
        private userWcsVector[] _affinePoint;
        private userWcsVector[] _targetRotatePoint;
        private userWcsVector[] _sourceRotatePoint;

        [DisplayName("输入目标点")]
        [DescriptionAttribute("输入属性1")]
        public userPixVector[] TargetPoint
        {
            get
            {
                if (this.RefSource1.Count > 0)
                {
                    TreeNodeCollection nodes = this._refNode.FirstNode.Nodes;
                    string[] keyValues = new string[nodes.Count];
                    for (int i = 0; i < nodes.Count; i++)
                        keyValues[i] = nodes[i].Name;
                    List<userPixVector> list = new List<userPixVector>();
                    for (int i = 0; i < keyValues.Length; i++)
                    {
                        var value = this.GetPropertyValue(this.RefSource1, keyValues[i]);
                        switch (value?.GetType().Name)
                        {
                            case nameof(userPixVector):
                                list.Add(value as userPixVector);
                                break;
                            case nameof(userWcsPoint):
                                userPixPoint wcsPoint = (value as userWcsPoint).GetPixPoint();
                                list.Add(new userPixVector(wcsPoint.Row, wcsPoint.Col, 0, wcsPoint.Grab_x, wcsPoint.Grab_y, wcsPoint.Grab_theta, wcsPoint.CamParams));
                                break;
                            case nameof(userWcsVector):
                                list.Add((value as userWcsVector).GetPixVector());
                                break;
                            case nameof(userWcsCircle):
                                userPixCircle pixCircle = (value as userWcsCircle).GetPixCircle();
                                list.Add(new userPixVector(pixCircle.Row, pixCircle.Col, 0, pixCircle.Grab_x, pixCircle.Grab_y, pixCircle.Grab_theta, pixCircle.CamParams));
                                break;
                            case nameof(userWcsCircleSector):
                                userPixCircleSector pixCircleSector = (value as userWcsCircleSector).GetPixCircleSector();
                                list.Add(new userPixVector(pixCircleSector.Row, pixCircleSector.Col, 0, pixCircleSector.Grab_x, pixCircleSector.Grab_y, pixCircleSector.Grab_theta, pixCircleSector.CamParams));
                                break;
                            case nameof(userWcsRectangle2):
                                userPixRectangle2 pixRect2 = (value as userWcsRectangle2).GetPixRectangle2();
                                list.Add(new userPixVector(pixRect2.Row, pixRect2.Col, pixRect2.Rad, pixRect2.Grab_x, pixRect2.Grab_y, pixRect2.Grab_theta, pixRect2.CamParams));
                                break;
                            case nameof(userWcsCoordSystem):
                                userPixCoordSystem wcsCoord = (value as userWcsCoordSystem).GetPixCoordSystem();
                                list.Add(new userPixVector(wcsCoord.CurrentPoint.Row, wcsCoord.CurrentPoint.Col, wcsCoord.CurrentPoint.Rad, wcsCoord.CurrentPoint.Grab_x, wcsCoord.CurrentPoint.Grab_y, wcsCoord.CurrentPoint.Grab_theta, wcsCoord.CurrentPoint.CamParams));
                                break;

                            default:
                                throw new ArgumentException("参数类型错误");
                        }
                    }
                    this._targetPoint = list.ToArray();
                    list.Clear();
                }
                else
                    this._targetPoint = new userPixVector[0];
                return this._targetPoint;
            }
            set
            {
                this._targetPoint = value;
            }
        }

        [DisplayName("输入源点")]
        [DescriptionAttribute("输入属性2")]
        public userPixVector[] SourcePoint
        {
            get
            {
                if (this.RefSource2.Count > 0)
                {
                    TreeNodeCollection nodes = this._refNode.Nodes[1].Nodes;
                    string[] keyValues = new string[nodes.Count];
                    for (int i = 0; i < nodes.Count; i++)
                        keyValues[i] = nodes[i].Name;
                    List<userPixVector> list = new List<userPixVector>();
                    for (int i = 0; i < keyValues.Length; i++)
                    {
                        var value = this.GetPropertyValue(this.RefSource2, keyValues[i]);
                        switch (value?.GetType().Name)
                        {
                            case nameof(userPixVector):
                                list.Add(value as userPixVector);
                                break;
                            case nameof(userWcsPoint):
                                userPixPoint wcsPoint = (value as userWcsPoint).GetPixPoint();
                                list.Add(new userPixVector(wcsPoint.Row, wcsPoint.Col, 0, wcsPoint.Grab_x, wcsPoint.Grab_y, wcsPoint.Grab_theta, wcsPoint.CamParams));
                                break;
                            case nameof(userWcsVector):
                                list.Add((value as userWcsVector).GetPixVector());
                                break;
                            case nameof(userWcsCircle):
                                userPixCircle pixCircle = (value as userWcsCircle).GetPixCircle();
                                list.Add(new userPixVector(pixCircle.Row, pixCircle.Col, 0, pixCircle.Grab_x, pixCircle.Grab_y, pixCircle.Grab_theta, pixCircle.CamParams));
                                break;
                            case nameof(userWcsCircleSector):
                                userPixCircleSector pixCircleSector = (value as userWcsCircleSector).GetPixCircleSector();
                                list.Add(new userPixVector(pixCircleSector.Row, pixCircleSector.Col, 0, pixCircleSector.Grab_x, pixCircleSector.Grab_y, pixCircleSector.Grab_theta, pixCircleSector.CamParams));
                                break;
                            case nameof(userWcsRectangle2):
                                userPixRectangle2 pixRect2 = (value as userWcsRectangle2).GetPixRectangle2();
                                list.Add(new userPixVector(pixRect2.Row, pixRect2.Col, pixRect2.Rad, pixRect2.Grab_x, pixRect2.Grab_y, pixRect2.Grab_theta, pixRect2.CamParams));
                                break;
                            case nameof(userWcsCoordSystem):
                                userPixCoordSystem wcsCoord = (value as userWcsCoordSystem).GetPixCoordSystem();
                                list.Add(new userPixVector(wcsCoord.CurrentPoint.Row, wcsCoord.CurrentPoint.Col, wcsCoord.CurrentPoint.Rad, wcsCoord.CurrentPoint.Grab_x, wcsCoord.CurrentPoint.Grab_y, wcsCoord.CurrentPoint.Grab_theta, wcsCoord.CurrentPoint.CamParams));
                                break;

                            default:
                                throw new ArgumentException("参数类型错误");
                        }
                    }
                    this._sourcePoint = list.ToArray();
                    list.Clear();
                }
                else
                    this._sourcePoint = new userPixVector[0];
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

        public ZoneCompensationParam ZoneParam { get; set; }
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
            this.ZoneParam = new ZoneCompensationParam();
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
                ///////////////////////////////////////////
                if (this.Param.IsZoneCompensation)
                {
                    string _grabNo = CommunicationConfigParamManger.Instance.ReadValue(this.Param.CoordSysName, enCommunicationCommand.GrabNo).ToString();
                    switch (_grabNo)
                    {
                        case "1":
                            this.Param.Add_X = this.ZoneParam.X1;
                            this.Param.Add_Y = this.ZoneParam.Y1;
                            this.Param.Add_Angle = this.ZoneParam.Angle1;
                            break;
                        case "2":
                            this.Param.Add_X = this.ZoneParam.X2;
                            this.Param.Add_Y = this.ZoneParam.Y2;
                            this.Param.Add_Angle = this.ZoneParam.Angle2;
                            break;
                        case "3":
                            this.Param.Add_X = this.ZoneParam.X3;
                            this.Param.Add_Y = this.ZoneParam.Y3;
                            this.Param.Add_Angle = this.ZoneParam.Angle3;
                            break;
                        case "4":
                            this.Param.Add_X = this.ZoneParam.X4;
                            this.Param.Add_Y = this.ZoneParam.Y4;
                            this.Param.Add_Angle = this.ZoneParam.Angle4;
                            break;
                    }
                }
                Result.Succss = AlignMethod.CalculateAlign2(this.TargetPoint, this.SourcePoint, this.Param, out _affinePoint, out this._addXYTheta);               
                ////////////////////////// 补偿值 //////////////////////////////////////////////////////////////////
                if (this._targetPoint != null && this._targetPoint.Length > 0)
                {
                    int minLength = Math.Min(this._targetPoint.Length, this._sourcePoint.Length);
                    for (int i = 0; i < minLength; i++)
                    {
                        // 这里不能更新目标点的拍照坐标，映射的坐标计算要在别的地方计算
                        LoggerHelper.Info(this.name + "->目标点坐标Pix：" + this._targetPoint[i].ToString(), this._targetPoint[i].CamName);
                        LoggerHelper.Info(this.name + "->目标点坐标Wcs：" + this._targetPoint[i].GetWcsVector().ToString(), this._targetPoint[i].CamName);
                        LoggerHelper.Info(this.name + "->源点坐标Pix：" + this._sourcePoint[i].ToString(), this._sourcePoint[i].CamName);
                        LoggerHelper.Info(this.name + "->源点坐标Wcs：" + this._sourcePoint[i].GetWcsVector().ToString(), this._sourcePoint[i].CamName);
                    }
                }
                else
                {
                    for (int i = 0; i < this._sourcePoint.Length; i++)
                    {
                        // 这里不能更新目标点的拍照坐标，映射的坐标计算要在别的地方计算
                        LoggerHelper.Info(this.name + "->源点坐标Pix：" + this._sourcePoint[i].ToString(), this._sourcePoint[i].CamName);
                        LoggerHelper.Info(this.name + "->源点坐标Wcs：" + this._sourcePoint[i].GetWcsVector().ToString(), this._sourcePoint[i].CamName);
                    }
                }
                this.CreateResultInfo(4);
                stopwatch.Stop();
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[0].SetValue(this.name, "Add_x", this._addXYTheta.X);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[1].SetValue(this.name, "Add_y", this._addXYTheta.Y);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[2].SetValue(this.name, "Add_theta", this._addXYTheta.Angle);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[3].SetValue(this.name, "Time(ms)", stopwatch.ElapsedMilliseconds);
                OnExcuteCompleted("补偿参数", this.Param?.ViewWindow, this.name, this.Param); // 这个主要用于可以在其他地方来修改补偿值
                OnExcuteCompleted("分区补偿参数", this.Param?.ViewWindow, this.name, this.ZoneParam);
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

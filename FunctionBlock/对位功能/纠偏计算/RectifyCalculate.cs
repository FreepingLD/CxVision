using Common;
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
    // 用于将平台校正到示教位置
    public class RectifyCalculate : BaseFunction, IFunction
    {
        [NonSerialized]
        private TreeNode _refNode = null;
        private userWcsVector _addXYTheta;
        private userWcsVector _curWcsVector;
        private userWcsVector _teachWcsVector;

        private userWcsCoordSystem[] _wcsCoordSystem;
        private userWcsCoordSystem _wcsRectifyCoordSystem;

        [DisplayName("特征点")]
        [DescriptionAttribute("输入属性1")]
        public userWcsCoordSystem[] WcsCoordSystem
        {
            get
            {
                try
                {
                    if (this.RefSource1.Count > 0)
                    {
                        TreeNodeCollection nodes = this._refNode.FirstNode.Nodes;
                        string[] keyValues = new string[nodes.Count];
                        for (int i = 0; i < nodes.Count; i++)
                            keyValues[i] = nodes[i].Name;
                        ////////////////////////////////////////////////
                        List<userWcsCoordSystem> listPoint = new List<userWcsCoordSystem>();
                        for (int i = 0; i < keyValues.Length; i++)
                        {
                            var value = this.GetPropertyValue(this.RefSource1, keyValues[i]);
                            switch (value?.GetType().Name)
                            {
                                case "userWcsCoordSystem[]":
                                    userWcsCoordSystem[] wcsCoordSystems = value as userWcsCoordSystem[];
                                    foreach (var item in wcsCoordSystems)
                                    {
                                        listPoint.Add(item);
                                    }
                                    break;
                                case "userPixCoordSystem[]":
                                    userPixCoordSystem[] pixCoordSystems = value as userPixCoordSystem[];
                                    foreach (var item in pixCoordSystems)
                                    {
                                        listPoint.Add(item.GetWcsCoordSystem());
                                    }
                                    break;
                                case nameof(userWcsCoordSystem):
                                    userWcsCoordSystem wcsCoordSystem = value as userWcsCoordSystem;
                                    if (wcsCoordSystem != null)
                                        listPoint.Add(wcsCoordSystem);
                                    break;
                                case nameof(userPixCoordSystem):
                                    userPixCoordSystem pixCoordSystem = value as userPixCoordSystem;
                                    if (pixCoordSystem != null)
                                        listPoint.Add(pixCoordSystem.GetWcsCoordSystem());
                                    break;
                                case nameof(userWcsVector):
                                    userWcsVector wcsVector = value as userWcsVector;
                                    userPixVector pixVector = new userPixVector();
                                    if (wcsVector != null)
                                        pixVector = wcsVector.GetPixVector();
                                    pixCoordSystem = new userPixCoordSystem(pixVector, pixVector);
                                    listPoint.Add(pixCoordSystem.GetWcsCoordSystem());
                                    break;
                                case nameof(userWcsPoint):
                                    userWcsPoint wcsPoint = value as userWcsPoint;
                                    pixVector = new userPixVector();
                                    if (wcsPoint != null)
                                    {
                                        userPixPoint pixPoint = wcsPoint.GetPixPoint();
                                        pixVector = new userPixVector(pixPoint.Row, pixPoint.Col, 0, pixPoint.CamParams);
                                        pixVector.Grab_x = pixPoint.Grab_x;
                                        pixVector.Grab_y = pixPoint.Grab_y;
                                        pixVector.Grab_theta = pixPoint.Grab_theta;
                                    }
                                    pixCoordSystem = new userPixCoordSystem(pixVector, pixVector);
                                    listPoint.Add(pixCoordSystem.GetWcsCoordSystem());
                                    break;
                            }
                        }
                        ///////////////////////////////////////////////
                        this._wcsCoordSystem = listPoint.ToArray();
                        listPoint.Clear();
                    }
                    else
                        this._wcsCoordSystem = null;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return _wcsCoordSystem;
            }
            set { _wcsCoordSystem = value; }
        }

        [DisplayName("补偿值")]
        [DescriptionAttribute("输出属性")]
        public userWcsVector AddXYTheta { get => this._addXYTheta; set => this._addXYTheta = value; }


        [DisplayName("当前向量")]
        [DescriptionAttribute("输出属性")]
        public userWcsVector CurWcsVector { get => this._curWcsVector; set => this._curWcsVector = value; }

        [DisplayName("示教向量")]
        [DescriptionAttribute("输出属性")]
        public userWcsVector TeachWcsVector { get => this._teachWcsVector; set => this._teachWcsVector = value; }
        
        [DisplayName("纠偏坐标系")]
        [DescriptionAttribute("输出属性")]
        public userWcsCoordSystem WcsRectifyCoordSystem { get => this._wcsRectifyCoordSystem; set => this._wcsRectifyCoordSystem = value; }

        public RectifyCalculateParam Param { get; set; }

        public ZoneRectifyParam ZoneParam { get; set; }

        public RectifyCalculate()
        {
            this.ZoneParam = new ZoneRectifyParam();
            this.Param = new RectifyCalculateParam();
            this.ResultInfo = new BindingList<MeasureResultInfo>();
        }


        public OperateResult Execute(params object[] param)
        {
            this.Result.Succss = true;
            double maxError = 0;
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
                ////////////////////////////////////
                if (this.Param.IsZoneCompensation)
                {
                    string _grabNo = CommunicationConfigParamManger.Instance.ReadValue(this.Param.CoordSysName, enCommunicationCommand.StationNum).ToString();
                    switch (_grabNo)
                    {
                        case "1":
                            this.Param.Angle = this.ZoneParam.Angle1;
                            break;
                        case "2":
                            this.Param.Angle = this.ZoneParam.Angle2;
                            break;
                        case "3":
                            this.Param.Angle = this.ZoneParam.Angle3;
                            break;
                        case "4":
                            this.Param.Angle = this.ZoneParam.Angle4;
                            break;
                    }
                }
                Result.Succss = AlignMethod.Rectify(this.WcsCoordSystem, this.Param, out this._addXYTheta,out this._curWcsVector,out this._teachWcsVector,out maxError);
                this._wcsRectifyCoordSystem = new userWcsCoordSystem(this._teachWcsVector,this._curWcsVector);
                ////////////////////////// 补偿值 //////////////////////////////////////////////////////////////////
                stopwatch.Stop();
                this.CreateResultInfo(12);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[0].SetValue(this.name, "Add_x", Math.Round(this.AddXYTheta.X, 5));
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[1].SetValue(this.name, "Add_y", Math.Round(this.AddXYTheta.Y, 5));
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[2].SetValue(this.name, "Add_theta", Math.Round(this.AddXYTheta.Angle, 5));
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[3].SetValue(this.name, "当前_x", Math.Round(this._curWcsVector.X, 5));
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[4].SetValue(this.name, "当前_y", Math.Round(this._curWcsVector.Y, 5));
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[5].SetValue(this.name, "当前_theta", Math.Round(this._curWcsVector.Angle, 5));
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[6].SetValue(this.name, "示教_x", Math.Round(this._teachWcsVector.X, 5));
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[7].SetValue(this.name, "示教_y", Math.Round(this._teachWcsVector.Y, 5));
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[8].SetValue(this.name, "示教_theta", Math.Round(this._teachWcsVector.Angle, 5));
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[9].SetValue(this.name, "目标角度", this.Param.Angle);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[10].SetValue(this.name, "最大误差", maxError);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[11].SetValue(this.name, "Time(ms)", stopwatch.ElapsedMilliseconds);
                OnExcuteCompleted("RectifyCalculateParam", this.Param?.ViewWindow, this.name, this.Param);
                OnExcuteCompleted("ZoneRectifyParam", this.Param?.ViewWindow, this.name, this.ZoneParam);
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

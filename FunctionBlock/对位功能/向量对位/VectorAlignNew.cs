using Common;
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
    public class VectorAlignNew : BaseFunction, IFunction
    {
        [NonSerialized]
        private TreeNode _refNode = null;

        private userWcsVector _addXYTheta;
        [DisplayName("补偿值")]
        [DescriptionAttribute("输出属性")]
        public userWcsVector AddXYTheta { get => this._addXYTheta; set => this._addXYTheta = value; }

        private userPixCoordSystem[] _targetPixCoordSystem;

        private userPixCoordSystem[] _sourcePixCoordSystem;


        [DisplayName("目标点")]
        [DescriptionAttribute("输入属性1")]
        public userPixCoordSystem[] TargetPixCoordSystem
        {
            get
            {
                if (this.RefSource1.Count > 0)
                {
                    TreeNodeCollection nodes = this._refNode.FirstNode.Nodes;
                    string[] keyValues = new string[nodes.Count];
                    for (int i = 0; i < nodes.Count; i++)
                        keyValues[i] = nodes[i].Name;
                    List<userPixCoordSystem> list = new List<userPixCoordSystem>();
                    for (int i = 0; i < keyValues.Length; i++)
                    {
                        object value = this.GetPropertyValue(this.RefSource1, keyValues[i]);
                        switch (value?.GetType().Name)
                        {
                            case "userWcsCoordSystem[]":
                                userWcsCoordSystem[] wcsCoordSystems = value as userWcsCoordSystem[];
                                foreach (var item in wcsCoordSystems)
                                {
                                    list.Add(item.GetPixCoordSystem());
                                }
                                break;
                            case nameof(userWcsCoordSystem):
                                userWcsCoordSystem wcsCoordSystem = value as userWcsCoordSystem;
                                list.Add(wcsCoordSystem.GetPixCoordSystem());
                                break;
                            case nameof(userPixCoordSystem):
                                userPixCoordSystem pixCoordSystem = value as userPixCoordSystem;
                                list.Add(pixCoordSystem);
                                break;
                            default:
                                new UserMessageForm().ShowDialog(this.name + "PlateCurPoint:参数类型错误");
                                break;
                        }
                    }
                    ///////////////////////////////////////////////
                    this._targetPixCoordSystem = list.ToArray();
                    list.Clear();
                }
                else
                    this._targetPixCoordSystem = null;
                return this._targetPixCoordSystem;
            }
            set
            {
                this._targetPixCoordSystem = value;
            }
        }


        [DisplayName("源点")]
        [DescriptionAttribute("输入属性2")]
        public userPixCoordSystem[] SourcePixCoordSystem
        {
            get
            {
                if (this.RefSource2.Count > 0)
                {
                    TreeNodeCollection nodes = this._refNode.Nodes[1].Nodes; // 第二个节点
                    string[] keyValues = new string[nodes.Count];
                    for (int i = 0; i < nodes.Count; i++)
                        keyValues[i] = nodes[i].Name;
                    List<userPixCoordSystem> list = new List<userPixCoordSystem>();
                    for (int i = 0; i < keyValues.Length; i++)
                    {
                        object value = this.GetPropertyValue(this.RefSource2, keyValues[i]);
                        switch (value?.GetType().Name)
                        {
                            case "userWcsCoordSystem[]":
                                userWcsCoordSystem[] wcsCoordSystems = value as userWcsCoordSystem[];
                                foreach (var item in wcsCoordSystems)
                                {
                                    list.Add(item.GetPixCoordSystem());
                                }
                                break;
                            case nameof(userWcsCoordSystem):
                                userWcsCoordSystem wcsCoordSystem = value as userWcsCoordSystem;
                                list.Add(wcsCoordSystem.GetPixCoordSystem());
                                break;
                            case nameof(userPixCoordSystem):
                                userPixCoordSystem pixCoordSystem = value as userPixCoordSystem;
                                list.Add(pixCoordSystem);
                                break;
                            default:
                                new UserMessageForm().ShowDialog(this.name + "  SourcePixCoordSystem:参数类型错误");
                                break;
                        }
                    }
                    ///////////////////////////////////////////////
                    this._sourcePixCoordSystem = list.ToArray();
                    list.Clear();
                }
                else
                    this._sourcePixCoordSystem = null;
                return this._sourcePixCoordSystem;
            }
            set
            {
                this._sourcePixCoordSystem = value;
            }
        }


        public CompensationParam Param { get; set; }




        public VectorAlignNew()
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
                Result.Succss = AlignMethod.CalculateAlign(this.SourcePixCoordSystem, this.TargetPixCoordSystem, this.Param, out this._addXYTheta);
                ////////////////////////// 补偿值 //////////////////////////////////////////////////////////////////
                stopwatch.Stop();
                this.CreateResultInfo(4);
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
                case nameof(this.AddXYTheta):
                    return this.AddXYTheta;
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

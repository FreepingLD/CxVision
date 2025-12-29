using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MotionControlCard;
using System.Windows.Forms;
using System.Threading;
using HalconDotNet;
using Common;
using System.IO;
using Sensor;
using Command;
using AlgorithmsLibrary;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;

namespace FunctionBlock
{
    [Serializable]
    [DefaultProperty(nameof(WcsPolyLine))]
    /// <summary>
    /// 从文件中读取轨迹 
    /// </summary>
    public class ContourRead : BaseFunction, IFunction
    {
        [NonSerialized]
        private TreeNode _refNode;
        private userWcsPolyLine _wcsPolyLine;
        private userWcsCoordSystem[] _wcsCoordSystem;


        [DisplayName("坐标系")]
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
                        List<userWcsCoordSystem> listCooreSys = new List<userWcsCoordSystem>();
                        for (int i = 0; i < keyValues.Length; i++)
                        {
                            var value = this.GetPropertyValue(this.RefSource1, keyValues[i]);
                            switch (value?.GetType().Name)
                            {
                                case nameof(userWcsCoordSystem):
                                    //this._wcsCoordSystem = ((userWcsCoordSystem)value);
                                    listCooreSys.Add(((userWcsCoordSystem)value));
                                    break;
                                case nameof(userPixCoordSystem):
                                    //this._wcsCoordSystem = ((userPixCoordSystem)value).GetWcsCoordSystem();
                                    listCooreSys.Add(((userPixCoordSystem)value).GetWcsCoordSystem());
                                    break;
                                case nameof(userWcsVector):
                                    userWcsVector wcsVector = value as userWcsVector;
                                    //this._wcsCoordSystem = new userWcsCoordSystem();
                                    //this._wcsCoordSystem.CurrentPoint = wcsVector.Clone();
                                    listCooreSys.Add(new userWcsCoordSystem(new userWcsVector(), wcsVector.Clone()));
                                    break;
                            }

                        }
                        ////////////////////////////
                        this._wcsCoordSystem = listCooreSys.ToArray();
                        listCooreSys.Clear();
                    }
                    else
                        this._wcsCoordSystem = null;
                    return this._wcsCoordSystem;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            set
            {
                this._wcsCoordSystem = value;
            }
        }
        [DisplayName("输出轨迹")]
        [DescriptionAttribute("输出属性")]
        public userWcsPolyLine WcsPolyLine { get { return _wcsPolyLine; } set { this._wcsPolyLine = value; } }

        public ContourReadParam ReadParam { get; set; }

        public ContourRead()
        {
            this.ReadParam = new ContourReadParam();
            this.ResultInfo = new BindingList<OcrResultInfo>();
        }


        #region 实现接口,各自实现自己的逻辑，就可实现通用的逻辑
        public OperateResult Execute(params object[] param)
        {
            this.Result.Succss = false;
            try
            {
                string index = "";
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                userPixCoordSystem offsetCoordSys = new userPixCoordSystem();
                if (param != null)
                {
                    foreach (var item in param)
                    {
                        if (item == null) continue;
                        switch (item.GetType().Name)
                        {
                            case nameof(userPixCoordSystem):
                                offsetCoordSys = (userPixCoordSystem)item;
                                break;
                            case nameof(String):
                                if (item.ToString().Split('=').Length > 0)
                                    index = item.ToString().Split('=').Last();
                                break;
                            case nameof(TreeNode):
                                this._refNode = item as TreeNode;
                                break;
                        }
                    }
                }
                this._wcsPolyLine?.Clear();
                this.Result.Succss = ContourReadMethod.ReadTrack(this.ReadParam, out this._wcsPolyLine);
                ///////////////////////////////////////////////
                stopwatch.Stop();
                this.CreateResultInfo(4);
                ((BindingList<OcrResultInfo>)this.ResultInfo)[0].SetValue(this.name, "轨迹X", string.Join(",", this.WcsPolyLine.X.ToArray()));
                ((BindingList<OcrResultInfo>)this.ResultInfo)[1].SetValue(this.name, "轨迹Y", string.Join(",", this.WcsPolyLine.Y.ToArray()));
                ((BindingList<OcrResultInfo>)this.ResultInfo)[2].SetValue(this.name, "轨迹Z", string.Join(",", this.WcsPolyLine.Z.ToArray()));
                ((BindingList<OcrResultInfo>)this.ResultInfo)[3].SetValue(this.name, "Time(ms)", stopwatch.ElapsedMilliseconds.ToString());
                ////////////////////////////////////////////////////
                OnExcuteCompleted(this._wcsPolyLine.CamName, this._wcsPolyLine?.ViewWindow, this.name, this._wcsPolyLine);
            }
            catch (Exception e)
            {
                LoggerHelper.Error(this.name + "-读取轨迹数据：" + "报错" + e);
                this.Result.Succss = false;
            }
            if (this.Result.Succss)
                LoggerHelper.Info(this.name + "-读取轨迹数据：" + "成功");
            else
                LoggerHelper.Error(this.name + "-读取轨迹数据：" + "失败");
            // 更改UI字体　
            UpdataNodeElementStyle(param, this.Result.Succss);
            return this.Result;
        }

        public object GetPropertyValues(string propertyName)
        {
            switch (propertyName)
            {
                case "轨迹":
                case nameof(this.WcsPolyLine):
                    return this._wcsPolyLine;

                case "名称":
                case nameof(this.Name):
                default:
                    return this.name;
            }
        }

        public bool SetPropertyValues(string propertyName, params object[] value)
        {
            string name = "";
            if (value != null)
                name = value[0].ToString();
            switch (propertyName)
            {
                default:
                case "名称":
                case nameof(this.Name):
                    this.name = value[0].ToString();
                    return true;
                case nameof(TreeNode):
                    this._refNode = value[0] as TreeNode;
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
            // throw new NotImplementedException();
        }

        #endregion



    }

}

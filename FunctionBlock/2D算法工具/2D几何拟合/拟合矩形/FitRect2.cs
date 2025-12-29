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
    [Serializable]
    [DefaultProperty(nameof(WcsRect2))]
    public class FitRect2 : BaseFunction, IFunction
    {
        [NonSerialized]
        private TreeNode _refNode = null;
        private userWcsRectangle2 _wcsRect2;
        private userWcsPoint[] _wcsPoint;

        [DisplayName("输入点")]
        [DescriptionAttribute("输入属性1")]
        public userWcsPoint[] WcsPoint
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
                        List<userWcsPoint> listPoint = new List<userWcsPoint>();
                        for (int k = 0; k < keyValues.Length; k++)
                        {
                            var value = this.GetPropertyValue(this.RefSource1, keyValues[k]);
                            switch (value?.GetType().Name)
                            {
                                case nameof(userWcsLine):
                                    userWcsLine wcsLine = value as userWcsLine;
                                    if (wcsLine != null && wcsLine.EdgesPoint_xyz != null)
                                    {
                                        foreach (var item2 in wcsLine.EdgesPoint_xyz)
                                            listPoint.Add(item2);
                                    }
                                    break;
                                case nameof(userWcsPoint):
                                    listPoint.Add((userWcsPoint)value);
                                    break;
                                case nameof(userWcsPolyLine):
                                    userWcsPolyLine wcsPolyLine = value as userWcsPolyLine;
                                    for (int i = 0; i < wcsPolyLine.X.Count; i++)
                                    {
                                        listPoint.Add(new userWcsPoint(wcsPolyLine.X[i], wcsPolyLine.Y[i], 0));
                                    }
                                    break;
                                case nameof(userWcsPolygon):
                                    userWcsPolygon wcsPolygon = value as userWcsPolygon;
                                    for (int i = 0; i < wcsPolygon.X.Count; i++)
                                    {
                                        listPoint.Add(new userWcsPoint(wcsPolygon.X[i], wcsPolygon.Y[i], 0));
                                    }
                                    break;
                            }
                        }
                        this._wcsPoint = listPoint.ToArray();
                        listPoint.Clear();
                    }
                    else
                        this._wcsPoint = null;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return _wcsPoint;
            }
            set { _wcsPoint = value; }
        }
        public Rect2FitParam FitParam
        {
            get;
            set;
        }

        [DisplayName("矩形对象")]
        [DescriptionAttribute("输出属性")]
        public userWcsRectangle2 WcsRect2 { get => _wcsRect2; set => _wcsRect2 = value; }

        public FitRect2()
        {
            this.FitParam = new Rect2FitParam();
            this.ResultInfo = new BindingList<MeasureResultInfo>();
        }


        #region  实现接口的部分
        public OperateResult Execute(params object[] param)
        {
            this.Result.Succss = false;
            try
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                this.Result.Succss = GeometryFitMethod.Instance.FitRect2(this.WcsPoint, this.FitParam, out this._wcsRect2);
                stopwatch.Stop();
                this.CreateResultInfo(7);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[0].SetValue(this.name, "X", this._wcsRect2.X);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[1].SetValue(this.name, "Y", this._wcsRect2.Y);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[2].SetValue(this.name, "Z", this._wcsRect2.Z);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[3].SetValue(this.name, "角度", this._wcsRect2.Deg);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[4].SetValue(this.name, "半宽", this._wcsRect2.Length1);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[5].SetValue(this.name, "半高", this._wcsRect2.Length2);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[6].SetValue(this.name, "Time", stopwatch.ElapsedMilliseconds);
                OnExcuteCompleted(this.name, this._wcsRect2); //this.WcsPoint[0].CamName, this.WcsPoint[0].CamParams?.ViewWindow, 
            }
            catch (Exception ex)
            {
                LoggerHelper.Error(this.name + "->执行错误" + ex);
                this.Result.Succss = false;
            }
            if (this.Result.Succss)
                LoggerHelper.Info(this.name + "->执行成功");
            else
                LoggerHelper.Error(this.name + "->执行失败");
            // 更改UI字体　
            UpdataNodeElementStyle(param, this.Result.Succss);
            return this.Result;
        }

        public object GetPropertyValues(string propertyName)
        {
            switch (propertyName)
            {
                case "名称":
                case "Name":
                    return this.name;
                case nameof(this.WcsRect2):
                    return this._wcsRect2; //
                default:
                    return this._wcsRect2;
            }
        }
        public bool SetPropertyValues(string propertyName, params object[] value)
        {
            switch (propertyName)
            {
                case "名称":
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
            catch
            {

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

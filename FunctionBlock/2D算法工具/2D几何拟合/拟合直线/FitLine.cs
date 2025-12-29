using Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Forms;

namespace FunctionBlock
{
    [Serializable]
    [DefaultProperty(nameof(WcsLine))]
    public class FitLine : BaseFunction, IFunction
    {
        [NonSerialized]
        private TreeNode _refNode = null;
        private userWcsLine _wcsLine;
        private userWcsPoint[] _wcsPoint;
        public LineFitParam FitParam
        {
            get;
            set;
        }

        [DisplayName("直线对象")]
        [DescriptionAttribute("输出属性")]
        public userWcsLine WcsLine { get => _wcsLine; set => _wcsLine = value; }


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
                        for (int i = 0; i < keyValues.Length; i++)
                        {
                            var value = this.GetPropertyValue(this.RefSource1, keyValues[i]);
                            switch (value?.GetType().Name)
                            {
                                case nameof(userWcsLine):
                                    userWcsLine wcsLine = value as userWcsLine;
                                    if(wcsLine != null && wcsLine.EdgesPoint_xyz != null)
                                    {
                                        foreach (var item in wcsLine.EdgesPoint_xyz)
                                            listPoint.Add(item);
                                    }
                                    break;
                                case nameof(userWcsPoint):
                                    userWcsPoint wcsPoint = value as userWcsPoint;
                                    listPoint.Add(wcsPoint);
                                    break;
                                case nameof(userWcsPolyLine):
                                    userWcsPolyLine wcsPolyLine = value as userWcsPolyLine;
                                    for (int k = 0; k < wcsPolyLine.X.Count; k++)
                                    {
                                        listPoint.Add(new userWcsPoint(wcsPolyLine.X[k], wcsPolyLine.Y[k], 0, wcsPolyLine.Grab_x, wcsPolyLine.Grab_y, wcsPolyLine.CamParams));
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
        public FitLine()
        {
            this.FitParam = new LineFitParam();
            this.ResultInfo = new BindingList<OcrResultInfo>();
        }


        #region  实现接口的部分
        public OperateResult Execute(params object[] param)
        {
            this.Result.Succss = false;
            try
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Restart();
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
                this.Result.Succss = GeometryFitMethod.Instance.FitLine(this.WcsPoint, this.FitParam, out this._wcsLine);
                stopwatch.Stop();
                this.CreateResultInfo(8);
                ((BindingList<OcrResultInfo>)this.ResultInfo)[0].SetValue(this.name, "X1", $"{this._wcsLine.X1}");
                ((BindingList<OcrResultInfo>)this.ResultInfo)[1].SetValue(this.name, "Y1", $"{this._wcsLine.Y1}");
                ((BindingList<OcrResultInfo>)this.ResultInfo)[2].SetValue(this.name, "X2", $"{this._wcsLine.X2}");
                ((BindingList<OcrResultInfo>)this.ResultInfo)[3].SetValue(this.name, "Y2", $"{this._wcsLine.Y2}");
                ((BindingList<OcrResultInfo>)this.ResultInfo)[4].SetValue(this.name, "角度", $"{this._wcsLine.Angle}");
                ((BindingList<OcrResultInfo>)this.ResultInfo)[5].SetValue(this.name, "轨迹X", string.Join(",", this._wcsLine.X1, this._wcsLine.X2));
                ((BindingList<OcrResultInfo>)this.ResultInfo)[6].SetValue(this.name, "轨迹Y", string.Join(",", this._wcsLine.Y1, this._wcsLine.Y2));
                ((BindingList<OcrResultInfo>)this.ResultInfo)[7].SetValue(this.name, "Time(ms)", $"{stopwatch.ElapsedMilliseconds}");
                //////////////////////////////////////////
                OnExcuteCompleted( this.name, this._wcsLine); // 在图形窗口显示  this._wcsLine?.CamName, this._wcsLine?.ViewWindow,
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
                case nameof(this.WcsLine):
                    return this._wcsLine;
                default:
                    return this._wcsLine;
            }
        }
        public bool SetPropertyValues(string propertyName, params object[] value)
        {
            try
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
            catch (Exception ex)
            {
                LoggerHelper.Error($"Error: {ex}"); 
                return false;
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

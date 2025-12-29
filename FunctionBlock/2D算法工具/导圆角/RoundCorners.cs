using AlgorithmsLibrary;
using Common;
using FunctionBloc;
using HalconDotNet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FunctionBlock
{
    [Serializable]
    [DefaultProperty(nameof(WcsPolyLine))]
    public class RoundCorners : BaseFunction, IFunction, INotifyPropertyChanged
    {
        [NonSerialized]
        private TreeNode _refNode = null;
        private userWcsPolyLine _wcsPolyLine;
        private userWcsLine _WcsLine1;
        private userWcsLine _WcsLine2;


        [DisplayName("导角轨迹")]
        [DescriptionAttribute("输出属性")]
        public userWcsPolyLine WcsPolyLine { get => _wcsPolyLine; set => _wcsPolyLine = value; }

        [DisplayName("输入直线1")]
        [DescriptionAttribute("输入属性1")]
        public userWcsLine WcsLine1
        {
            get
            {
                if (this.RefSource1.Count > 0)
                {
                    this._WcsLine1 = this.GetPropertyValue(this.RefSource1)[0] as userWcsLine;
                    if (this._WcsLine1 == null)
                        this._WcsLine1 = new userWcsLine();
                }
                else
                    this._WcsLine1 = new userWcsLine();
                return this._WcsLine1;
            }
            set
            {
                this._WcsLine1 = value;
            }
        }

        [DisplayName("输入直线2")]
        [DescriptionAttribute("输入属性2")]
        public userWcsLine WcsLine2
        {
            get
            {
                if (this.RefSource2.Count > 0)
                {
                    this._WcsLine2 = this.GetPropertyValue(this.RefSource2)[0] as userWcsLine;
                    if (this._WcsLine2 == null)
                        this._WcsLine2 = new userWcsLine();
                }
                else
                    this._WcsLine2 = new userWcsLine();
                return this._WcsLine2;
            }
            set
            {
                this._WcsLine2 = value;
            }
        }

        public RoundCornerParam Param { get; set; }

        public RoundCorners()
        {
            this.Param = new RoundCornerParam();
            this.ResultInfo = new BindingList<OcrResultInfo>();
        }




        #region 实现接口

        public OperateResult Execute(params object[] param)
        {
            this.Result.Succss = false;
            try
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                string index = "";
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
                            case nameof(TreeNode):
                                this._refNode = item as TreeNode;
                                break;
                        }
                    }
                }
                this.Result.Succss = RoundCornerMethod.LineRoundCorner(this.WcsLine1, this.WcsLine2, this.Param, out this._wcsPolyLine);
                stopwatch.Stop();
                this.CreateResultInfo(5);
                ((BindingList<OcrResultInfo>)this.ResultInfo)[0].SetValue(this.name, "X", string.Join(",", this._wcsPolyLine.X.ToArray()));
                ((BindingList<OcrResultInfo>)this.ResultInfo)[1].SetValue(this.name, "Y", string.Join(",", this._wcsPolyLine.Y.ToArray()));
                ((BindingList<OcrResultInfo>)this.ResultInfo)[2].SetValue(this.name, "Z", string.Join(",", this._wcsPolyLine.Z.ToArray()));
                ((BindingList<OcrResultInfo>)this.ResultInfo)[3].SetValue(this.name, "PointOrder", this.Param.PointOrder);
                ((BindingList<OcrResultInfo>)this.ResultInfo)[4].SetValue(this.name, "Time(ms)", stopwatch.ElapsedMilliseconds.ToString());
                OnExcuteCompleted(this._wcsPolyLine.CamName, this._wcsPolyLine?.ViewWindow, this.name, this._wcsPolyLine);
            }
            catch (Exception ex)
            {
                LoggerHelper.Error(this.name + "->执行报错", ex);
                this.Result.Succss = false;
            }
            if (this.Result.Succss)
                LoggerHelper.Info(this.name + "执行成功:" + string.Format("x={0},y={1},z={2}", this._wcsPolyLine.X, this._wcsPolyLine.Y, 0));
            else
                LoggerHelper.Error(this.name + "执行失败:" + string.Format("x={0},y={1},z={2}", this._wcsPolyLine.X, this._wcsPolyLine.Y, 0));
            // 更改UI字体　
            UpdataNodeElementStyle(param, this.Result.Succss);
            return this.Result;
        }
        public object GetPropertyValues(string propertyName)
        {
            switch (propertyName)
            {
                default:
                case "名称":
                case nameof(this.Name):
                    return this.name;
                case "输出对象":
                case nameof(this.WcsPolyLine):
                    return this._wcsPolyLine;
            }
        }
        public bool SetPropertyValues(string propertyName, params object[] value)
        {
            HalconLibrary ha = new HalconLibrary();
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

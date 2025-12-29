using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HalconDotNet;
using System.Drawing;
using Sensor;
using MotionControlCard;
using System.Windows.Forms;
using System.IO;
using AlgorithmsLibrary;
using System.ComponentModel;
using Common;
using System.Data;
using Light;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using System.Diagnostics;

namespace FunctionBlock
{
    [Serializable]
    [DefaultProperty(nameof(WcsPolyLine))]
    public class ContourPick : BaseFunction, IFunction
    {
        private BindingList<userWcsTrackPoint> _trackPoint;
        private string _acqSourceName;
        private userWcsPolyLine _wcsPolyLine;
        private userWcsCoordSystem _wcsCoordSystem;

        [DisplayName("采集源")]
        public string AcqSourceName { get => _acqSourceName; set => _acqSourceName = value; }

        [DisplayName("捨取点")]
        public BindingList<userWcsTrackPoint> TrackPoint { get => _trackPoint; set => _trackPoint = value; }

        [DisplayName("轨迹点")]
        [DescriptionAttribute("输出属性")]
        public userWcsPolyLine WcsPolyLine { get { return _wcsPolyLine; } set { this._wcsPolyLine = value; } }

        [DisplayName("坐标系")]
        [DescriptionAttribute("输入属性1")]
        public userWcsCoordSystem WcsCoordSystem
        {
            get
            {
                try
                {
                    if (this.RefSource2.Count > 0)
                    {
                        object[] oo = this.GetPropertyValue(this.RefSource2);
                        if (oo != null && oo.Length > 0)
                        {
                            this._wcsCoordSystem = oo.Last() as userWcsCoordSystem;
                        }
                        else
                            this._wcsCoordSystem = new userWcsCoordSystem();
                    }
                    else
                        this._wcsCoordSystem = new userWcsCoordSystem();
                    return _wcsCoordSystem;
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

        public ContourPickParam PullParam { get; set; }

        /// <summary>
        /// 这里应该使用采集源而不是相机名
        /// </summary>
        /// <param name="camName"></param>
        /// <param name="coordSystem"></param>
        public ContourPick(string acqSourceName)
        {
            this._acqSourceName = acqSourceName;
            this._trackPoint = new BindingList<userWcsTrackPoint>();
            this.PullParam = new ContourPickParam();
            InitBindingTable();
        }

        public ContourPick()
        {
            this._trackPoint = new BindingList<userWcsTrackPoint>();
            this.PullParam = new ContourPickParam();
            InitBindingTable();
        }
        private void InitBindingTable()
        {
            if (this.ResultInfo == null)
            {
                this.ResultInfo = new BindingList<OcrResultInfo>();
            }
        }


        #region 实现接口
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
                        }
                    }
                }
                this._wcsPolyLine?.Clear();
                this._wcsPolyLine = new userWcsPolyLine();
                this._wcsPolyLine.CamParams = AcqSourceManage.Instance.GetAcqSource(this.AcqSourceName)?.Sensor?.CameraParam; 
                this._wcsPolyLine.CamName = AcqSourceManage.Instance.GetAcqSource(this.AcqSourceName)?.Sensor?.CameraParam.SensorName;
                this._wcsPolyLine.ViewWindow = AcqSourceManage.Instance.GetAcqSource(this.AcqSourceName)?.Sensor?.CameraParam.ViewWindow;
                for (int i = 0; i < this._trackPoint.Count; i++)  // 以相机的机械坐标为基准点 
                {
                    this._wcsPolyLine.Add(this._trackPoint[i].X, this._trackPoint[i].Y, this._trackPoint[i].Z);
                }
                this._wcsPolyLine = this._wcsPolyLine.AffineWcsPolyLine(this.WcsCoordSystem.GetVariationHomMat2D());
                this.Result.Succss = true;
                ///////////////////////////////////////////////
                stopwatch.Stop();
                this.CreateResultInfo(4);
                ((BindingList<OcrResultInfo>)this.ResultInfo)[0].SetValue(this.name, "轨迹X", string.Join(",", this.WcsPolyLine.X.ToArray()));
                ((BindingList<OcrResultInfo>)this.ResultInfo)[1].SetValue(this.name, "轨迹Y", string.Join(",", this.WcsPolyLine.Y.ToArray()));
                ((BindingList<OcrResultInfo>)this.ResultInfo)[2].SetValue(this.name, "轨迹Z", string.Join(",", this.WcsPolyLine.Z.ToArray()));
                ((BindingList<OcrResultInfo>)this.ResultInfo)[3].SetValue(this.name, "Time(ms)", stopwatch.ElapsedMilliseconds.ToString());
                ////////////////////////////////////////////////////
                OnExcuteCompleted(this.WcsPolyLine.CamName, this.WcsPolyLine.ViewWindow,this.name + index, this.WcsPolyLine);
            }
            catch (Exception ex)
            {
                this.Result.Succss = false;
                LoggerHelper.Error(this.name + "执行报错" + ex, this.WcsPolyLine.CamName);
            }
            if (this.Result.Succss)
                LoggerHelper.Info(this.name + "->执行成功", this.WcsPolyLine.CamName);
            else
                LoggerHelper.Error(this.name + "->执行失败", this.WcsPolyLine.CamName);
            // 更改UI字体　
            UpdataNodeElementStyle(param, this.Result.Succss);
            return this.Result;
        }
        public object GetPropertyValues(string propertyName)
        {
            switch (propertyName)
            {
                default:
                    return this._wcsPolyLine;
                case "名称":
                case nameof(this.Name):
                    return this.name;
                case nameof(this.ResultInfo):
                    return this.ResultInfo;           
            }
        }
        public bool SetPropertyValues(string propertyName, params object[] value)
        {
            HalconLibrary ha = new HalconLibrary();
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

        }
        public void Save(string path)
        {

        }

        #endregion



    }
}

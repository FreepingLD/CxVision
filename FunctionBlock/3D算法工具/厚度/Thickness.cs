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

namespace FunctionBlock
{

    /// <summary>
    /// 满足平面与非平面的厚度计算
    /// </summary>
    [Serializable]
    public class Thickness : BaseFunction, IFunction
    {
        private userWcsThick _wcsThick;
        [NonSerialized]
        private PointCloudData _pointCloudModel1;
        [NonSerialized]
        private PointCloudData _pointCloudModel2;


        [DisplayName("输入点云1")]
        [DescriptionAttribute("输入属性1")]
        public PointCloudData PointCloudModel1
        {
            get
            {
                if (this.RefSource1.Count > 0)
                {
                    object[] oo = this.GetPropertyValue(this.RefSource1);
                    if (oo != null && oo.Length > 0)
                        this._pointCloudModel1 = oo[0] as PointCloudData;
                    if (this._pointCloudModel1 == null)
                        this._pointCloudModel1 = new PointCloudData();
                }
                else
                    this._pointCloudModel1 = new PointCloudData();
                return this._pointCloudModel1;
            }
            set
            {
                this._pointCloudModel1 = value;
            }
        }

        [DisplayName("输入点云2")]
        [DescriptionAttribute("输入属性2")]
        public PointCloudData PointCloudModel2
        {
            get
            {
                if (this.RefSource2.Count > 0)
                {
                    object[] oo = this.GetPropertyValue(this.RefSource2);
                    if (oo != null && oo.Length > 0)
                        this._pointCloudModel2 = oo[0] as PointCloudData;
                    if (this._pointCloudModel2 == null)
                        this._pointCloudModel2 = new PointCloudData();
                }
                else
                    this._pointCloudModel2 = new PointCloudData();
                return this._pointCloudModel2;
            }
            set
            {
                this._pointCloudModel2 = value;
            }
        }

        public ThicknessParam Param { get; set; }

        public Thickness()
        {
            this.ResultInfo = new BindingList<MeasureResultInfo>();
            this.Param = new ThicknessParam();
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo());
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo());
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo());
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo());
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo());
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo());
        }



        #region  实现接口的部分
        public OperateResult Execute(params object[] param)
        {
            this.Result.Succss = false;
            try
            {
                Result.Succss = this.Param.Distance2D(this.PointCloudModel1.ObjectModel3D[0], this.PointCloudModel2.ObjectModel3D[0], out this._wcsThick);
                /////////////////////////////////////////////////
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[0].SetValue(this.name, "厚度", this._wcsThick.Thick);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[1].SetValue(this.name, "激光1距离", this._wcsThick.Dist1);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[2].SetValue(this.name, "激光2距离", this._wcsThick.Dist2);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[3].SetValue(this.name, "X", this._wcsThick.X);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[4].SetValue(this.name, "Y", this._wcsThick.Y);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[5].SetValue(this.name, "Theta", this._wcsThick.Grab_theta);
                if (((BindingList<MeasureResultInfo>)this.ResultInfo)[0].State == "OK")
                    this._wcsThick.Result = "OK";
                else
                    this._wcsThick.Result = "NG";
                OnExcuteCompleted(this._pointCloudModel1.SensorName, this._pointCloudModel1.ViewWindow, this.name, this._wcsThick); //+ ".数据标签"
                /////////////////////////////////////
            }
            catch (Exception ex)
            {
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
                    return this.name;
                case "厚度":
                case "坐标位置":
                default:
                    return this._wcsThick; //

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
            try
            {
                //MoveParamManager.Instance.Read();
            }
            catch (Exception ex)
            {
                LoggerHelper.Error(ex.ToString());
            }
        }
        public void Save(string path)
        {
            try
            {
               // MoveParamManager.Instance.Save();
            }
            catch (Exception ex)
            {
                LoggerHelper.Error(ex.ToString());
            }
        }

        #endregion



    }

}

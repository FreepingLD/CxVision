using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HalconDotNet;
using Sensor;
using MotionControlCard;
using System.Windows.Forms;
using Common;
using System.IO;
using System.ComponentModel;
using AlgorithmsLibrary;

namespace FunctionBlock
{
    [Serializable]
    [DefaultProperty(nameof(WcsPose))]
    public class CreatePose3D : BaseFunction, IFunction
    { 
        private userWcsPose _WcsPose;

        public CreatePose3DParam CreateParam { get; set; }

        [DisplayName("输出3D位姿")]
        [DescriptionAttribute("输出属性")]
        public userWcsPose WcsPose { get => _WcsPose; set => _WcsPose = value; }

        public CreatePose3D()
        {
            this.CreateParam = new CreatePose3DParam();
            this.ResultInfo = new BindingList<MeasureResultInfo>();
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name));
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name));
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name));
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name));
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name));
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name));
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name));
        }


        #region  实现接口的部分

        public OperateResult Execute(params object[] param)
        {
            this.Result.Succss = false;
            try
            {
                this.Result.Succss = this.CreateParam.Create3DPose( out this._WcsPose);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[0].SetValue(this.name, "Tx", this._WcsPose.Tx);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[1].SetValue(this.name, "Ty", this._WcsPose.Ty);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[2].SetValue(this.name, "Tz", this._WcsPose.Tz);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[3].SetValue(this.name, "Rx", this._WcsPose.Rx);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[4].SetValue(this.name, "Ry", this._WcsPose.Ry);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[5].SetValue(this.name, "Rz", this._WcsPose.Rz);
                ((BindingList<MeasureResultInfo>)this.ResultInfo)[6].SetValue(this.name, "Type", this._WcsPose.Type);
                OnExcuteCompleted(this.name, this._WcsPose);
            }
            catch (Exception ex)
            {
                LoggerHelper.Error(this.name + "->执行错误" + ex);
                return this.Result;
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
            try
            {
                switch (propertyName)
                {
                    case "名称":
                    case nameof(this.Name):
                        return this.name;
                    default:
                    case "3D坐标系":
                    case nameof(this.WcsPose):
                        return this._WcsPose;
                }
            }
            catch
            {
                throw new Exception();
            }

        }

        public bool SetPropertyValues(string propertyName, params object[] value)
        {
            try
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
            catch
            {
                throw new Exception();
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
        public enum enShowItems
        {
            输出对象,
        }
    }


}

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
    [DefaultProperty(nameof(DataHandle3D))]
    public class CreatePrimitive3D :BaseFunction,  IFunction
    {
        [NonSerialized]
        private HObjectModel3D  dataHandle3D = null;
        [NonSerialized]
        private userWcsPose _WcsPose = null;

        public PrimitiveParam Param { get; set; }


        [DisplayName("输入3D位姿")]
        [DescriptionAttribute("输入属性1")]
        public userWcsPose WcsPose
        {
            get
            {
                try
                {
                    if (this.RefSource1.Count > 0)
                    {
                        var oo = this.GetPropertyValue(this.RefSource1).Last();
                        if (oo != null)
                        {
                            switch (oo.GetType().Name)
                            {
                                case nameof(userWcsPose):
                                    this._WcsPose = oo as userWcsPose;
                                    break;
                                case nameof(userWcsCoordSystem):
                                    this._WcsPose = ((userWcsCoordSystem)oo).GetUserWcsPose3D();
                                    break;
                            }
                        }
                        else
                            this._WcsPose = new userWcsPose();
                    }
                    else
                    {

                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return _WcsPose;
            }
            set
            {
                _WcsPose = value;
            }
        }

        [DisplayName("输出3D对象")]
        [DescriptionAttribute("输出属性")]
        public HObjectModel3D DataHandle3D { get => dataHandle3D; set => dataHandle3D = value; }


        public CreatePrimitive3D()
        {
            this.Param = new PlanePrimitiveParam();
        }


        #region  实现接口的部分

        public OperateResult Execute(params object[] param)
        {
            this.Result.Succss = false;
            try
            {
                ClearHandle(this.dataHandle3D);
                this.Result.Succss = this.Param.CreatePrimitive(this.WcsPose,  out this.dataHandle3D);
                OnExcuteCompleted(this.name, this.dataHandle3D);
            }
            catch(Exception ee)
            {
                LoggerHelper.Error(this.name + "->执行错误" + ee);
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
                    case "3D对象":
                    default:
                    case nameof(this.DataHandle3D):
                        return this.dataHandle3D;
                }
            }
            catch
            {
                throw new Exception();
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
                if (this.dataHandle3D.IsInitialized())
                    this.dataHandle3D.Dispose();
            }
            catch
            {
                LoggerHelper.Error(this.name + "->删除该对象报错");
            }
            finally
            {
                OnItemDeleteEvent(this, this.name);
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


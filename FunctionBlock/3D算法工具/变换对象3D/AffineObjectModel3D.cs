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

    [Serializable]
    public class AffineObjectModel3D : BaseFunction, IFunction
    {
        [NonSerialized]
        private HObjectModel3D dataHandle3D = null;

        [NonSerialized]
        private HObjectModel3D affineDataHandle3D = null;

        [NonSerialized]
        private userWcsPose _WcsPose = null;

        [DisplayName("输入3D对象")]
        [DescriptionAttribute("输入属性1")]
        public HObjectModel3D DataHandle3D
        {
            get
            {
                try
                {
                    if (this.RefSource1.Count > 0)
                        this.dataHandle3D = this.GetPropertyValue(this.RefSource1).Last() as HObjectModel3D;
                    else
                    {
                        this.dataHandle3D = null;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return dataHandle3D;
            }
            set
            {
                dataHandle3D = value;
            }
        }

        [DisplayName("输入3D位姿")]
        [DescriptionAttribute("输入属性2")]
        public userWcsPose WcsPose
        {
            get
            {
                try
                {
                    if (this.RefSource2.Count > 0)
                    {
                        var oo = this.GetPropertyValue(this.RefSource2).Last();
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
        public HObjectModel3D AffineDataHandle3D { get => affineDataHandle3D; set => affineDataHandle3D = value; }

        public AffineObjectModelParam AffineParam { get; set; }

        public AffineObjectModel3D()
        {
            this.AffineParam = new AffineObjectModelParam();
            this.ResultInfo = new BindingList<MeasureResultInfo>();
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo(this.name));
        }





        #region 实现接口
        public OperateResult Execute(params object[] param)
        {
            this.Result.Succss = false;
            try
            {
                ClearHandle(this.affineDataHandle3D);
                this.Result.Succss = this.AffineParam.AffineTransObjectModel3D(this.DataHandle3D, this.WcsPose, out this.affineDataHandle3D);
                if (this.affineDataHandle3D != null)
                {
                    ((BindingList<MeasureResultInfo>)this.ResultInfo)[0].SetValue(this.name, "点云数量", this.affineDataHandle3D.GetObjectModel3dParams("num_points").I);
                    this.Result.Succss = true;
                }
                OnExcuteCompleted(this.name, this.affineDataHandle3D);
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
            switch (propertyName)
            {
                case "名称":
                case nameof(this.Name):
                    return this.name;
                case "变换3D对象":
                case "3D对象":
                case nameof(this.AffineDataHandle3D):
                    return this.dataHandle3D; //
                default:
                    if (this.name == propertyName)
                        return this.dataHandle3D;
                    else return null;
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
                /////////////////////             
                default:
                    return true;
            }
        }
        public void ReleaseHandle()
        {
            try
            {
                if (this.affineDataHandle3D != null)
                {
                    this.affineDataHandle3D.Dispose();
                }
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

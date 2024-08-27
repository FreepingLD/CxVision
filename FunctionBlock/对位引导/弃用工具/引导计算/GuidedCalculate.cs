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


namespace FunctionBlock
{

    [Serializable]
    [DefaultProperty(nameof(HomMat2D))]
    /// <summary>
    /// 激光取点采集的数据类
    /// </summary>
    public class GuidedCalculate : BaseFunction, IFunction
    {

        private UserHomMat2D _homMat2D;

        [DisplayName("变换矩阵")]
        [DescriptionAttribute("输出属性")]
        public UserHomMat2D HomMat2D { get => _homMat2D; set => _homMat2D = value; }


        public enRobotJawEnum Jaw { get; set; }

        public GuidedCalculate()
        {
            this.Jaw = enRobotJawEnum.NONE;
            this._homMat2D = new UserHomMat2D();
            this.ResultInfo = new BindingList<MeasureResultInfo>();
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo());
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo());
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo());
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo());
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo());
            ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo());
            //((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo());
            //((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo());
            //((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo());
        }


        #region 实现接口,各自实现自己的逻辑，就可实现通用的逻辑
        public OperateResult Execute(params object[] param)
        {
            this.Result.Succss = false;
            try
            {
                bool isOk = true;
                RobotJawParam JawParam = RobotJawParaManager.Instance.GetJawParam(this.Jaw.ToString());
                this.HomMat2D = new UserHomMat2D(JawParam.GetHomMat2D());
                /////////////////////////
                if (((BindingList<MeasureResultInfo>)this.ResultInfo).Count > 0)
                    ((BindingList<MeasureResultInfo>)this.ResultInfo)[0].SetValue(this.name, "C00", this.HomMat2D.c00);
                else
                    ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo());
                if (((BindingList<MeasureResultInfo>)this.ResultInfo).Count > 1)
                    ((BindingList<MeasureResultInfo>)this.ResultInfo)[1].SetValue(this.name, "C01", this.HomMat2D.c01);
                else
                    ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo());
                if (((BindingList<MeasureResultInfo>)this.ResultInfo).Count > 2)
                    ((BindingList<MeasureResultInfo>)this.ResultInfo)[2].SetValue(this.name, "C02_Tx", this.HomMat2D.c02);
                else
                    ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo());
                if (((BindingList<MeasureResultInfo>)this.ResultInfo).Count > 3)
                    ((BindingList<MeasureResultInfo>)this.ResultInfo)[3].SetValue(this.name, "C10", this.HomMat2D.c10);
                else
                    ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo());
                if (((BindingList<MeasureResultInfo>)this.ResultInfo).Count > 4)
                    ((BindingList<MeasureResultInfo>)this.ResultInfo)[4].SetValue(this.name, "C11", this.HomMat2D.c11);
                else
                    ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo());
                if (((BindingList<MeasureResultInfo>)this.ResultInfo).Count > 5)
                    ((BindingList<MeasureResultInfo>)this.ResultInfo)[5].SetValue(this.name, "C12_Ty", this.HomMat2D.c12);
                else
                    ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo());
                ///////////////////////////////////////////////////////////////////////////////////
                this.Result.Succss = isOk;
                if (this.Result.Succss)
                    LoggerHelper.Info(this.name + "-引导计算:" + this.HomMat2D + "->成功");
                else
                    LoggerHelper.Error(this.name + "-引导计算:" + this.HomMat2D + "->失败");
            }
            catch (Exception ex)
            {
                LoggerHelper.Error(this.name + "引导计算：" + "报错" + ex);
            }
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
            // throw new NotImplementedException();
        }
        public void Save(string path)
        {
            // throw new NotImplementedException();
        }
        #endregion




    }

}

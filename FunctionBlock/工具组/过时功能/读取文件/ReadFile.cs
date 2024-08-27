using System;
using System.Collections.Generic;
using HalconDotNet;
using System.Windows.Forms;
using AlgorithmsLibrary;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Common;
using Sensor;
using System.IO;
using System.Linq;

namespace FunctionBlock
{
    [Serializable]
    [DefaultProperty( nameof(CoordPointData))]
    public class ReadFile : BaseFunction, IFunction
    {
        [NonSerialized]
        private CoordPoint[] _CoordPointData = null;
        private int index = 0;

        [DisplayName("输出坐标")]
        [DescriptionAttribute("输出属性")]
        public CoordPoint [] CoordPointData { get => _CoordPointData; set => _CoordPointData = value; }

        public ReadFileParam ReadParam { get; set; }


        public ReadFile()
        {
            //FunctionManage.ImageSourceList.Add(this);
            this.ReadParam = new ReadFileParam();
            this.ResultInfo = new BindingList<MeasureResultInfo>();
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
                if (param == null) this.index = 0; // param不能为空
                else
                {
                    if (param.Length == 1)
                        int.TryParse(param[0].ToString(), out this.index);
                    if (param.Length == 2)
                        int.TryParse(param[1].ToString(), out this.index);
                }
                if (this.ReadParam.FilePath.Count > this.index)
                    this._CoordPointData = this.ReadParam.ReadFile(this.ReadParam.FilePath[this.index]);// 读取第I个路径的图像
                else
                    this._CoordPointData = this.ReadParam.ReadFile(this.ReadParam.FilePath[0]);
                OnExcuteCompleted(this.name, this._CoordPointData);
                this.Result.Succss = true;
            }
            catch (Exception ex)
            {
                LoggerHelper.Error(this.name + "->执行错误", ex);
                this.Result.Succss = false;
            }
            if (this.Result.Succss)
                LoggerHelper.Info(this.name + "执行成功：");
            else
                LoggerHelper.Error(this.name + "执行失败：");
            // 更改UI字体　
            UpdataNodeElementStyle(param, this.Result.Succss);
            return this.Result;
        }
        public object GetPropertyValues(string propertyName)
        {
            switch (propertyName.Trim()) // 这个类只作一个图像输出
            {
                case "名称":
                    return this.name;
                default:
                    return this._CoordPointData;
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
            //throw new NotImplementedException();
        }
        public void Save(string path)
        {
            //throw new NotImplementedException();
        }
        #endregion


    }
}

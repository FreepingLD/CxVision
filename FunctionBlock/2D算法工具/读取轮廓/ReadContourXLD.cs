using System;
using System.Collections.Generic;
using HalconDotNet;
using System.Windows.Forms;
using AlgorithmsLibrary;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Common;

namespace FunctionBlock
{
    [Serializable]
    [DefaultProperty(nameof(XldContour))]
    public class ReadContourXLD : BaseFunction, IFunction
    {
        [NonSerialized]
        private XldDataClass xldContour = null;
        private userWcsPoint[] wcsPoint;
        private int index = 0;

        [DisplayName("输出轮廓")]
        [DescriptionAttribute("输出属性")]
        public XldDataClass XldContour { get => xldContour; set => xldContour = value; }
        public ReadXldParam ReadParam { get; set; }

        [DisplayName("轮廓点")]
        [DescriptionAttribute("输出属性")]
        public userWcsPoint[] WcsVector { get => wcsPoint; set => wcsPoint = value; }

        public ReadContourXLD()
        {
            this.ReadParam = new ReadXldParam();
            this.ResultInfo = new BindingList<MeasureResultInfo>();
        }

        #region  实现接口的部分
        public OperateResult Execute(params object[] param)
        {
            this.Result.Succss = false;
            try
            {
                HXLDCont hXLDCont;
                if (this.ReadParam.FilePath.Count >= this.index)
                    this.index++;
                else
                    this.index = 0;
                this.Result.Succss = this.ReadParam.ReadHXLDCon(this.ReadParam.FilePath[this.index], out hXLDCont);// 读取第I个路径的图像
                this.xldContour = new XldDataClass(hXLDCont);
                if (hXLDCont != null && hXLDCont.IsInitialized())
                {
                    HTuple rows, cols;
                    hXLDCont.GetContourXld(out rows, out cols);
                    this.wcsPoint = new userWcsPoint[rows.Length];
                    for (int i = 0; i < rows.Length; i++)
                    {
                        this.wcsPoint[i] = new userWcsPoint(rows[i], cols[i], 0);
                    }
                }
                //////////////////////////////////////////////
                OnExcuteCompleted(this.name, this.xldContour);
            }
            catch (Exception ee)
            {
                LoggerHelper.Error(this.name + "->执行错误", ee);
                return this.Result;
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
            switch (propertyName.Trim())
            {
                case "XLD轮廓":
                case "输入对象":
                    return this.XldContour;
                case "名称":
                case nameof(this.Name):
                    return this.name;
                case "当前对象":
                    return this.index;
                default:
                case nameof(this.XldContour):
                    return this.XldContour;
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

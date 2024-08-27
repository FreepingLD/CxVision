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
using AlgorithmsLibrary;
using System.ComponentModel;

namespace FunctionBlock
{
    [Serializable]
    [DefaultProperty(nameof(XldContData))]
    public class SaveContXLD : BaseFunction, IFunction
    {
        [NonSerialized]
        private XldDataClass _xldData;

        public SaveXldParam SaveParam { get; set; }

        [DisplayName("输入轮廓")]
        [DescriptionAttribute("输入属性1")]
        public XldDataClass XldContData
        {
            get
            {
                try
                {
                    if (this.RefSource1.Count > 0)
                    {
                        if (this._xldData == null)
                            this._xldData = new XldDataClass();
                        else
                            this._xldData?.Clear();
                        object[] oo = this.GetPropertyValue(this.RefSource1);
                        if (oo != null && oo.Length > 0)
                        {
                            foreach (var item in oo)
                            {
                                if (item != null)
                                {
                                    switch (item.GetType().Name)
                                    {
                                        case nameof(userWcsCircle):
                                            this._xldData.AddXLDCont(((userWcsCircle)item).GetXLD());
                                            break;
                                        case nameof(userWcsCircleSector):
                                            this._xldData.AddXLDCont(((userWcsCircleSector)item).GetPixCircleSector().GetXLD());
                                            break;
                                        case nameof(userWcsRectangle2):
                                            this._xldData.AddXLDCont(((userWcsRectangle2)item).GetPixRectangle2().GetXLD());
                                            break;
                                        case nameof(userWcsRectangle1):
                                            this._xldData.AddXLDCont(((userWcsRectangle1)item).GetPixRectangle1().GetXLD());
                                            break;
                                        case nameof(userWcsLine):
                                            this._xldData.AddXLDCont(((userWcsLine)item).GetPixLine().GetXLD());
                                            break;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        this._xldData = null;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return _xldData;
            }
            set
            {
                _xldData = value;
            }
        }

        public SaveContXLD()
        {
            this.SaveParam = new SaveXldParam();
        }


        #region  实现接口的部分
        public OperateResult Execute(params object[] param)
        {
            this.Result.Succss = false;
            try
            {
                this.Result.Succss = this.SaveParam.SaveXld(this.XldContData);
            }
            catch (Exception ex)
            {
                LoggerHelper.Error(this.name + "->执行错误", ex);
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
                case nameof(this.SaveParam):
                    return this.XldContData;
                default:
                    break;
            }
            return null;
        }

        public bool SetPropertyValues(string propertyName, params object[] value)
        {
            switch (propertyName)
            {
                case "名称":
                    this.name = value[0].ToString();
                    return true;
                //case "文件扩展名":
                //    this.extendName = value[0].ToString();
                //    return true;
                //case "添加日期时间":
                //    this.addDataTime = (bool)value[0];
                //    return true;
                //case "文件路径":
                //    this.folderPath = value.ToString();
                //    return true;
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

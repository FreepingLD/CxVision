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
    [DefaultProperty(nameof(FilePath))]
    public class WriteFile : BaseFunction, IFunction
    {
        [NonSerialized]
        private CoordPoint[] _CoordPoint;

        public WriteFileParam WriteParam { get; set; }

        [DisplayName("坐标点")]
        [DescriptionAttribute("输入属性1")]
        public CoordPoint [] CoordPoint
        {
            get
            {
                try
                {
                    if (this.RefSource1.Count > 0)
                    {
                        object [] oo = this.GetPropertyValue(this.RefSource1);
                        this._CoordPoint = new CoordPoint[oo.Length];
                        for (int i = 0; i < oo.Length; i++)
                        {
                            this._CoordPoint[i] = (CoordPoint)oo[i];
                        }
                    }
                    else
                    {
                        this._CoordPoint = null;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return _CoordPoint;
            }
            set
            {
                _CoordPoint = value;
            }
        }

        [DisplayName("文件路径")]
        [DescriptionAttribute("输出属性")]
        public string FilePath { get { return this.WriteParam.FilePath; } set { } }



        public WriteFile()
        {
            this.WriteParam = new WriteFileParam();
        }


        #region  实现接口的部分

        public OperateResult Execute(params object[] param)
        {
            this.Result.Succss = false;
            try
            {
                this.Result.Succss = this.WriteParam.WriteFile( this.CoordPoint);
            }
            catch (Exception ex)
            {
                LoggerHelper.Error(this.name + "->执行错误", ex);
                this.Result.Succss = false;
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
                default:
                case nameof(this._CoordPoint):
                    return this._CoordPoint;
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

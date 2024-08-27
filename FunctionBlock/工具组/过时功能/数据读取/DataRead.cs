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
    [DefaultProperty(nameof(ReadContent))]
    /// <summary>
    /// 激光取点采集的数据类
    /// </summary>
    public class DataRead : BaseFunction, IFunction
    {
        [DescriptionAttribute("绑定属性")]
        public BindingList<ReadCommunicateCommand> ReadDataList { get; set; }

        [DisplayName("读取数据")]
        [DescriptionAttribute("输出属性")]
        public string ReadContent { get; set; }



        public DataRead()
        {
            this.ReadDataList = new BindingList<ReadCommunicateCommand>();
        }


        #region 实现接口,各自实现自己的逻辑，就可实现通用的逻辑
        public OperateResult Execute(params object[] param)
        {
            this.Result .Succss= false;
            try
            {
                IMotionControl _card;
                if (this.ReadDataList == null || this.ReadDataList.Count == 0) return this.Result;
                _card = MotionCardManage.GetCard(this.ReadDataList[0].CoordSysName);
                if (_card == null)
                {
                    LoggerHelper.Info(this.name + "读取数据失败：" + "设备为Null");
                    return this.Result;
                }
                string[] value = new string[0];
                switch (_card.GetType().Name)
                {
                    case nameof(SocketClientDevice): // Socket通信跟其他通信方式的处理不一样
                        string readValue = _card?.ReadValue(enDataTypes.String, "", 1).ToString().Replace("\r","");
                         value = readValue.Split(',', ';', ':', ' ','\r');
                        break;
                    default:
                        value = new string[this.ReadDataList.Count];
                        for (int i = 0; i < this.ReadDataList.Count; i++)
                        {
                            //value[i] = _card.ReadValue(this.ReadDataList[i].DataType, this.ReadDataList[i].Adress, this.ReadDataList[i].Length).ToString();
                        }
                        break;
                }
                if (value.Length != this.ReadDataList.Count)
                {
                    throw new ArgumentException("读取的内容分组长度与要读取的数量不相等");
                }
                ///////// 给读取的内容赋值 ///////////////
                bool isOk = true;
                this.ReadContent = ""; // 赋值之前先清空
                for (int i = 0; i < this.ReadDataList.Count; i++)
                {
                    this.ReadDataList[i].ReadValue = value[i];
                    // 读取值与目标值比较
                    if (this.ReadDataList[i].IsCompare)
                    {
                        isOk = isOk && (this.ReadDataList[i].TargetValue == value[i]);
                    }
                    /// 输出需要的内容
                    if (this.ReadDataList[i].IsOutput)
                    {
                        if (this.ReadContent.Length > 0)
                            this.ReadContent += "," + value[i];
                        else
                            this.ReadContent = value[i];
                    }
                    // 复位放在后面，因为需要先比较
                    if(this.ReadDataList[i].IsReset)
                    {
                        //_card.WriteValue(this.ReadDataList[i].DataType, this.ReadDataList[i].Adress, 0);
                    }
                }
                this.Result.Succss = isOk;
                if (this.Result.Succss)
                    LoggerHelper.Info(this.name + "读取数据：" + this.ReadContent + "成功");
                else
                    LoggerHelper.Error(this.name + "读取数据：" + this.ReadContent + "失败");
            }
            catch (Exception e)
            {
                LoggerHelper.Error(this.name + "读取数据：" + "报错" + e);
                return this.Result;
            }
            return this.Result;
        }

        public object GetPropertyValues(string propertyName)
        {
            switch (propertyName)
            {
                case "值":
                case nameof(this.ReadContent):
                    return this.ReadContent;

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
            ///throw new NotImplementedException();
        }
        public void Save(string path)
        {
            //throw new NotImplementedException();
        }

        #endregion



    }

}

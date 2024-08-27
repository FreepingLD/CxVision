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
    [DefaultProperty("WriteData")]
    /// <summary>
    /// 激光取点采集的数据类
    /// </summary>
    public class DataWrite : BaseFunction, IFunction
    {
        private string _WriteData;

        [DescriptionAttribute("绑定属性")]
        public BindingList<WriteCommunicateCommand> WriteDataList { get; set; }

        [DisplayName("写入数据")]
        [DescriptionAttribute("输入属性1")]
        public string WriteData
        {
            get
            {
                try
                {
                    if (this.RefSource1.Count > 0)
                    {
                        this._WriteData = this.GetResultInfo(this.RefSource1);
                    }
                    else
                        this._WriteData = null;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return _WriteData;
            }
            set { _WriteData = value; }
        }


        public DataWrite()
        {
            this.WriteDataList = new BindingList<WriteCommunicateCommand>();
            this.WriteDataList.Add(new WriteCommunicateCommand());
        }

        private void InitBindingTable()
        {
            if (this.WriteDataList == null || this.WriteDataList.Count == 0)
            {
                this.WriteDataList = new BindingList<WriteCommunicateCommand>();
                this.WriteDataList.Add(new WriteCommunicateCommand());
            }
        }

        #region 实现接口,各自实现自己的逻辑，就可实现通用的逻辑
        public OperateResult Execute(params object[] param)
        {
            this.Result.Succss = false;
            try
            {
                IMotionControl _card;
                if (this.WriteDataList == null || this.WriteDataList.Count == 0)
                    _card = MotionCardManage.GetCard(enCoordSysName.CoordSys_0);
                else
                    _card = MotionCardManage.GetCard(this.WriteDataList[0].CoordSysName);
                if (_card == null)
                {
                    LoggerHelper.Info(this.name + "写入数据失败：" + "写入设备为Null");
                    return this.Result;
                }
                bool isOk = true;
                string content = "";
                string writeContent = this.WriteData;
                switch (_card.GetType().Name)
                {
                    case nameof(SocketClientDevice): // Socket通信跟其他通信方式的处理不一样    
                        foreach (var item in WriteDataList)
                        {
                            if (content.Length == 0)
                                content = item.WriteValue;
                            else
                                content += "," + item.WriteValue;
                        }
                        if (content != null && content.Trim().Length > 0)
                            isOk = isOk && _card.WriteValue(enDataTypes.String, "", content + "," + writeContent); //
                        else
                            isOk = isOk && _card.WriteValue(enDataTypes.String, "", writeContent); //
                        break;
                    default:
                        foreach (var item in WriteDataList)
                        {
                            //isOk = isOk && _card.WriteValue(item.DataType, item.Adress, item.WriteValue);
                        }
                        break;
                }
                this.Result.Succss = isOk;
                this.InitBindingTable();
                if (content != null && content.Trim().Length > 0)
                    this.WriteDataList[0].SetValue(content + "," + writeContent);
                else
                    this.WriteDataList[0].SetValue(writeContent);
                // 使用发命令的方式来更新视图  
                if (this.Result.Succss)
                    LoggerHelper.Info(this.name + "写入数据:" + this.WriteData + "成功");
                else
                    LoggerHelper.Error(this.name + "写入数据:" + this.WriteData + "失败");
            }
            catch (Exception ex)
            {
                LoggerHelper.Error(this.name + "写入数据：" + "报错" + ex);
                this.Result.Succss = false;
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
            //throw new NotImplementedException();
        }

        #endregion




    }

}

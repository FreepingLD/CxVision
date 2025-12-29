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
    public class SaveDataPlc : BaseFunction, IFunction
    {
        [NonSerialized]
        private object[] _saveContent;

        [DisplayName("保存内容")]
        [DescriptionAttribute("输出属性")]
        public object[] SaveContent { get { return _saveContent; } set { _saveContent = value; } }
        public SaveDataPlcParam SaveParam { get; set; }


        [DisplayName("文件路径")]
        [DescriptionAttribute("输出属性")]
        public string FilePath { get { return this.SaveParam.FilePath; } set { } }



        public SaveDataPlc()
        {
            this.ResultInfo = new BindingList<WriteCommunicateCommand>();
            this.SaveParam = new SaveDataPlcParam();
        }


        #region  实现接口的部分

        public OperateResult Execute(params object[] param)
        {
            this.Result.Succss = false;
            try
            {
                List<object> dataList = new List<object>();
                List<string> imagePathList = new List<string>();
                List<string> resultList = new List<string>();
                List<string> titelContent = new List<string>();
                object readValue = "";
                object readStateValue = "";
                string state = "OK";
                string panel_ID = "NONE";
                BindingList<WriteCommunicateCommand> SaveDataList = (BindingList<WriteCommunicateCommand>)this.ResultInfo;
                foreach (var item in SaveDataList)
                {
                    switch (item.CommunicationCommand)
                    {
                        case enCommunicationCommand.MemoryInfo:
                            switch (item.FlagBit)
                            {
                                // 读取图像路径
                                case enFlag.ImagePath:
                                    readValue = MemoryManager.Instance.GetValue(item.DataSource, enFlag.ImagePath);
                                    if (readValue != null)
                                    {
                                        imagePathList.Add(readValue.ToString());
                                    }
                                    break;
                                case enFlag.测量值:
                                case enFlag.测量值_结果:
                                default:
                                    titelContent.Add(item.Describe);
                                    readValue = MemoryManager.Instance.GetValue(item.DataSource, enFlag.测量值);
                                    if (readValue != null)
                                    {
                                        dataList.Add(readValue);
                                    }
                                    else
                                    {
                                        dataList.Add("NULL");
                                    }
                                    readStateValue = MemoryManager.Instance.GetValue(item.DataSource, enFlag.结果);
                                    if (readStateValue != null)
                                    {
                                        resultList.Add(readStateValue.ToString());
                                        if (readStateValue.ToString() == "NG" || readStateValue.ToString() == "ng")
                                            state = "NG";
                                    }
                                    else
                                    {
                                        resultList.Add("OK");
                                        state = "NG";
                                    }
                                    break;
                            }
                            break;
                        case enCommunicationCommand.ProductID:
                            titelContent.Add(item.Describe);
                            object value = CommunicationConfigParamManger.Instance.ReadValue(item.CoordSysName, item.CommunicationCommand); // 读取附加信息
                            if (value != null && value.ToString().Length > 0)
                                panel_ID = value.ToString();
                            else
                                panel_ID = "NONE";
                            item.WriteValue = value?.ToString();
                            dataList.Add(panel_ID);
                            break;
                        default:
                            value = CommunicationConfigParamManger.Instance.ReadValue(item.CoordSysName, item.CommunicationCommand); // 读取附加信息
                            if (value != null)
                                dataList.Add(value);
                            break;
                    }
                }
                dataList.Add(state);
                this.SaveContent = dataList.ToArray();
                dataList.Clear();
                this.Result.Succss = this.SaveParam.WriteData(panel_ID, imagePathList.ToArray(), titelContent.ToArray(), this.SaveContent, resultList.ToArray());
                titelContent.Clear();
                imagePathList.Clear();
                resultList.Clear();
            }
            catch (Exception ex)
            {
                LoggerHelper.Fatal(this.name + "->执行错误", ex);
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
                case nameof(this._saveContent):
                    return this._saveContent;
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

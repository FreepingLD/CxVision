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
using System.Diagnostics;

namespace FunctionBlock
{
    [Serializable]
    [DefaultProperty(nameof(FilePath))]
    public class SaveDataFtp : BaseFunction, IFunction
    {
        [NonSerialized]
        private object[] _saveContent;

        [DisplayName("读取内容")]
        [DescriptionAttribute("输出属性")]
        public object[] SaveContent { get { return _saveContent; } set { _saveContent = value; } }
        public SaveDataFtpParam SaveParam { get; set; }


        [DisplayName("文件路径")]
        [DescriptionAttribute("输出属性")]
        public string FilePath { get { return this.SaveParam.FilePath; } set { } }

        public BindingList<WriteCommunicateCommand> SaveDataList { get; set; }


        public SaveDataFtp()
        {
            this.SaveDataList = new BindingList<WriteCommunicateCommand>();
            this.ResultInfo = new BindingList<OcrResultInfo>();
            ((BindingList<OcrResultInfo>)this.ResultInfo).Add(new OcrResultInfo(this.name, "X", "0"));
            ((BindingList<OcrResultInfo>)this.ResultInfo).Add(new OcrResultInfo(this.name, "Y", "0"));
            ((BindingList<OcrResultInfo>)this.ResultInfo).Add(new OcrResultInfo(this.name, "Z", "0"));
            ((BindingList<OcrResultInfo>)this.ResultInfo).Add(new OcrResultInfo(this.name, "Z", "0"));
            this.SaveParam = new SaveDataFtpParam();
        }


        #region  实现接口的部分

        public OperateResult Execute(params object[] param)
        {
            this.Result.Succss = false;
            this.Result.ExcuteState = enExcuteState.NONE;
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            try
            {
                if (this.SaveDataList == null)
                    this.SaveDataList = new BindingList<WriteCommunicateCommand>();
                if (this.ResultInfo == null)
                {
                    this.ResultInfo = new BindingList<OcrResultInfo>();
                    ((BindingList<OcrResultInfo>)this.ResultInfo).Add(new OcrResultInfo(this.name, "X", "0"));
                }
                /////////////////////////////////////////////////////////////
                List<object> dataList = new List<object>();
                List<string> imagePathList = new List<string>();
                object readValue = "";
                object readStateValue = "";
                string panel_ID = "123456789";
                foreach (var item in SaveDataList)
                {
                    switch (item.CommunicationCommand)
                    {
                        case enCommunicationCommand.MemoryInfo: // 表示从内存中取数据
                            switch (item.FlagBit)
                            {
                                // 读取图像路径
                                case enFlag.ImagePath:
                                    readValue = MemoryManager.Instance.GetValue(item.DataSource, enFlag.测量值);
                                    if (readValue != null)
                                    {
                                        imagePathList.Add(readValue.ToString());
                                    }
                                    break;
                                case enFlag.测量值:
                                case enFlag.测量值_结果:
                                default:
                                    readValue = MemoryManager.Instance.GetValue(item.DataSource, enFlag.测量值);
                                    readStateValue = MemoryManager.Instance.GetValue(item.DataSource, enFlag.结果);
                                    dataList.Add(string.Join(",", item.Describe, readValue, readStateValue));
                                    break;
                            }
                            break;
                        case enCommunicationCommand.ProductID:
                            object value = CommunicationConfigParamManger.Instance.ReadValue(item.CoordSysName, item.CommunicationCommand); // 读取附加信息
                            if (value != null && value.ToString().Length > 0)
                                panel_ID = value.ToString();
                            else
                                panel_ID = "123456789";
                            item.WriteValue = value?.ToString();
                            break;
                        default:
                            value = CommunicationConfigParamManger.Instance.ReadValue(item.CoordSysName, item.CommunicationCommand); // 读取附加信息
                            if (value != null)
                                dataList.Add(value);
                            break;
                    }
                }
                this.SaveContent = dataList.ToArray();
                this.Result.Succss = this.SaveParam.WriteData(panel_ID, imagePathList.ToArray(), dataList.ToArray());
                dataList.Clear();
                imagePathList.Clear();
                stopwatch.Stop();
                ////////////////////////////////////////
                ((BindingList<OcrResultInfo>)this.ResultInfo)[0].SetValue(this.name, "图像路径", this.SaveParam.RemoteImagePath);
                ////////////////////////////////////////
                if (((BindingList<OcrResultInfo>)this.ResultInfo).Count < 2)
                    ((BindingList<OcrResultInfo>)this.ResultInfo).Add(new OcrResultInfo(this.name, "文件路径", "0"));
                ((BindingList<OcrResultInfo>)this.ResultInfo)[1].SetValue(this.name, "文件路径", this.SaveParam.RemotePanelPath);
                ////////////////////////////////////////
                if (((BindingList<OcrResultInfo>)this.ResultInfo).Count < 3)
                    ((BindingList<OcrResultInfo>)this.ResultInfo).Add(new OcrResultInfo(this.name, "Dmy路径", "0"));
                ((BindingList<OcrResultInfo>)this.ResultInfo)[2].SetValue(this.name, "Dmy路径", this.SaveParam.RemoteDmyPath);
                ////////////////////////////////////////
                if (((BindingList<OcrResultInfo>)this.ResultInfo).Count < 4)
                    ((BindingList<OcrResultInfo>)this.ResultInfo).Add(new OcrResultInfo(this.name, "Time(ms)", "0"));
                ((BindingList<OcrResultInfo>)this.ResultInfo)[3].SetValue(this.name, "Time(ms)", stopwatch.ElapsedMilliseconds.ToString());
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

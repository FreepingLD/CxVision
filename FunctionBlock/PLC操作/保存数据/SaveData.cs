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
    [DefaultProperty(nameof(SaveContent))]
    public class SaveDataNew : BaseFunction, IFunction
    {
        [NonSerialized]
        private object[] _saveContent;

        [DisplayName("保存内容")]
        [DescriptionAttribute("输出属性")]
        public object[] SaveContent { get { return _saveContent; } set { _saveContent = value; } }
        public SaveDataNewParam SaveParam { get; set; }

        public BindingList<SaveDataCommand> SaveDataList { get; set; }

        public SaveDataNew()
        {
            this.SaveParam = new SaveDataNewParam();
            this.SaveDataList = new BindingList<SaveDataCommand>();
            this.ResultInfo = new BindingList<OcrResultInfo>();
            ((BindingList<OcrResultInfo>)this.ResultInfo).Add(new OcrResultInfo());
            ((BindingList<OcrResultInfo>)this.ResultInfo).Add(new OcrResultInfo());
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
                List<object> dataList = new List<object>();
                List<object> stdValueDataList = new List<object>();
                List<object> upToleranceDataList = new List<object>();
                List<object> downToleranceDataList = new List<object>();
                List<string> titelContent = new List<string>();
                object readValue = "";
                object readStateValue = "";
                string state = "OK";
                BindingList<SaveDataCommand> SaveDataList = (BindingList<SaveDataCommand>)this.SaveDataList;
                foreach (var item in SaveDataList)
                {
                    if (!item.IsActive) continue;
                    switch (item.CommunicationCommand)
                    {
                        case enCommunicationCommand.MemoryInfo:
                            if (item.Describe == null)
                                titelContent.Add("NONE");
                            else
                                titelContent.Add(item.Describe);
                            readValue = MemoryManager.Instance.GetValue(item.DataSource, enFlag.测量值);
                            if (readValue != null)
                            {
                                dataList.Add(readValue);
                                item.SaveValue = readValue.ToString();
                            }
                            else
                            {
                                dataList.Add("NULL");
                                item.SaveValue = "NULL";
                            }
                            stdValueDataList.Add(MemoryManager.Instance.GetValue(item.DataSource, enFlag.标准值));
                            upToleranceDataList.Add(MemoryManager.Instance.GetValue(item.DataSource, enFlag.上偏差));
                            downToleranceDataList.Add(MemoryManager.Instance.GetValue(item.DataSource, enFlag.下偏差));
                            //////////////////////////////////
                            string OKNG = MemoryManager.Instance.GetValue(item.DataSource, enFlag.结果).ToString();
                            if (OKNG == "NG" || OKNG == "ng")
                                state = "NG";
                            break;
                        default:
                            if (item.Describe == null)
                                titelContent.Add("NONE");
                            else
                                titelContent.Add(item.Describe);
                            stdValueDataList.Add(0);
                            upToleranceDataList.Add(0);
                            downToleranceDataList.Add(0);
                            readValue = CommunicationConfigParamManger.Instance.ReadValue(item.CoordSysName, item.CommunicationCommand); // 读取附加信息
                            if (readValue != null)
                            {
                                dataList.Add(readValue);
                                item.SaveValue = readValue.ToString();
                            }
                            else
                            {
                                dataList.Add("NULL");
                                item.SaveValue = "NULL";
                            }
                            break;
                    }
                }
                titelContent.Add("Result");
                stdValueDataList.Insert(0, "StdValue");
                upToleranceDataList.Insert(0, "UpTolerance");
                downToleranceDataList.Insert(0, "DownTolerance");
                dataList.Add(state);
                this.SaveContent = dataList.ToArray();
                dataList.Clear();
                this.Result.Succss = this.SaveParam.WriteData(titelContent.ToArray(), stdValueDataList.ToArray(), upToleranceDataList.ToArray(), downToleranceDataList.ToArray(), this.SaveContent);
                titelContent.Clear();
                stdValueDataList.Clear();
                upToleranceDataList.Clear();
                downToleranceDataList.Clear();
                ///////////////////////////
                stopwatch.Stop();
                ((BindingList<OcrResultInfo>)this.ResultInfo)[0].SetValue(this.name, "保存内容", string.Join(",", this.SaveContent));
                ((BindingList<OcrResultInfo>)this.ResultInfo)[1].SetValue(this.name, "Time(ms)", stopwatch.ElapsedMilliseconds.ToString());
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

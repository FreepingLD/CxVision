using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HalconDotNet;
using Sensor;
using MotionControlCard;
using System.Windows.Forms;
using System.Data;
using Common;
using System.ComponentModel;
using AlgorithmsLibrary;
using System.Drawing;
using System.Reflection;

namespace FunctionBlock
{
    [Serializable]
    [DefaultProperty(nameof(DataList))]
    public class OutputDataOld : BaseFunction, IFunction
    {
        public static event DataSendEventHandler DataSend;
        private List<string> dataList = new List<string>();
        private List<string> upToleranceList = new List<string>();
        private List<string> downToleranceList = new List<string>();
        private List<string> stdValueList = new List<string>();

        private string resultState = "";
        private List<System.Drawing.Point> point = new List<System.Drawing.Point>();

        public List<IFunction> _DataList;

        [DisplayName("输出数据")]
        [DescriptionAttribute("输入属性1")]
        public List<IFunction> DataList
        {
            get
            {
                try
                {
                    if (this.RefSource1.Count > 0)
                    {
                        this._DataList.Clear();
                        foreach (var item in this.RefSource1.Values)
                        {
                            if (item is IFunction)
                                this._DataList.Add((IFunction)item);
                        }
                    }
                    else
                        this._DataList = new List<IFunction>();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return _DataList;
            }
            set
            {
                _DataList = value;
            }
        }


        public OutputDataOld()
        {
            this._DataList = new List<IFunction>();
            this.ResultInfo = new BindingList<MeasureResultInfo>();
            DataPrefixConfigManager.Instance.Read();
        }
        private void GetResultValue()
        {
            this.resultState = "OK";
            this.dataList.Clear();
            this.upToleranceList.Clear();
            this.downToleranceList.Clear();
            this.stdValueList.Clear();
            ////////////////////////////////////////////  初始化表头 //////////////////
            this.dataList.Add("数据");
            this.upToleranceList.Add("上偏差:");
            this.downToleranceList.Add("下偏差:");
            this.stdValueList.Add("标准值:");
            ///// 初始化头
            this.dataList.Add(DateTime.Now.ToString());  // 时间作为标准加入
            this.upToleranceList.Add("0");
            this.downToleranceList.Add("0");
            this.stdValueList.Add("0");
            ////////////////////////////////////////////
            DataPrefixConfig dataPrefixConfig = DataPrefixConfigManager.Instance.DataItemParamList[0];
            Type type = dataPrefixConfig.GetType();
            PropertyInfo[] propertyInfo = type.GetProperties();
            for (int i = 0; i < propertyInfo.Length; i++)
            {
                PropertyInfo singlePropertyInfo = type.GetProperty("DataItem" + (i + 1).ToString());
                if (singlePropertyInfo == null) continue;
                string propertyValue = singlePropertyInfo.GetValue(dataPrefixConfig).ToString();
                switch (propertyValue)
                {
                    case "NONE":
                        break;
                    default:
                        this.dataList.Add(propertyValue);
                        this.upToleranceList.Add("0");
                        this.downToleranceList.Add("0");
                        this.stdValueList.Add("0");
                        break;
                }
            }
            ////////////////////////////////////////////  提取管制数据
            foreach (var item in this.RefSource1.Values)
            {
                string name = ((BaseFunction)item).ResultInfo.GetType().Name;
                switch (((BaseFunction)item).ResultInfo.GetType().Name)
                {
                    case "BindingList`1":
                        switch (((BaseFunction)item).ResultInfo.GetType().GetGenericArguments()[0].Name)
                        {
                            case nameof(MeasureResultInfo):
                                BindingList<MeasureResultInfo> measureResultInfo = ((BaseFunction)item).ResultInfo as BindingList<MeasureResultInfo>;
                                if (measureResultInfo == null) break;
                                foreach (var itemTemp in measureResultInfo)
                                {
                                    if (itemTemp.IsOutput)
                                    {
                                        if (itemTemp.IsContainProperty("Mea_Value"))
                                            dataList.Add(itemTemp.Mea_Value.ToString());
                                        else
                                            dataList.Add("");
                                        //////////////////////////////////////////////
                                        if (itemTemp.IsContainProperty("Std_Value"))
                                            stdValueList.Add(itemTemp.Std_Value.ToString());
                                        else
                                            stdValueList.Add("0");
                                        //////////////////////////////////////////////
                                        if (itemTemp.IsContainProperty("LimitUp"))
                                            upToleranceList.Add(itemTemp.LimitUp.ToString());
                                        else
                                            upToleranceList.Add("0");
                                        //////////////////////////////////////////////
                                        if (itemTemp.IsContainProperty("LimitDown"))
                                            downToleranceList.Add(itemTemp.LimitDown.ToString());
                                        else
                                            downToleranceList.Add("0");
                                        //////////////////////////////////////////////
                                        if (itemTemp.State == "NG")
                                            this.resultState = "NG";
                                    }
                                }
                                break;
                        }
                        break;
                }
            }
            this.dataList.Add(this.resultState); // 将结果判定添加进去
        }

        private void GetResultValue2(List<IFunction> DataList)
        {
            this.resultState = "OK";
            this.dataList.Clear();
            this.upToleranceList.Clear();
            this.downToleranceList.Clear();
            this.stdValueList.Clear();
            ////////////////////////////////////////////  初始化表头 //////////////////
            this.dataList.Add("数据");
            this.upToleranceList.Add("上偏差:");
            this.downToleranceList.Add("下偏差:");
            this.stdValueList.Add("标准值:");
            ///// 初始化头
            this.dataList.Add(DateTime.Now.ToString());  // 时间作为标准加入
            this.upToleranceList.Add("0");
            this.downToleranceList.Add("0");
            this.stdValueList.Add("0");
            ////////////////////////////////////////////
            DataPrefixConfig dataPrefixConfig = DataPrefixConfigManager.Instance.DataItemParamList[0];
            Type type = dataPrefixConfig.GetType();
            PropertyInfo[] propertyInfo = type.GetProperties();
            for (int i = 0; i < propertyInfo.Length; i++)
            {
                PropertyInfo singlePropertyInfo = type.GetProperty("DataItem" + (i + 1).ToString());
                if (singlePropertyInfo == null) continue;
                string propertyValue = singlePropertyInfo.GetValue(dataPrefixConfig).ToString();
                switch (propertyValue)
                {
                    case "NONE":
                        break;
                    default:
                        this.dataList.Add(propertyValue);
                        this.upToleranceList.Add("0");
                        this.downToleranceList.Add("0");
                        this.stdValueList.Add("0");
                        break;
                }
            }
            ////////////////////////////////////////////  提取管制数据
            foreach (var item in DataList)
            {
                string name = ((BaseFunction)item).ResultInfo.GetType().Name;
                switch (((BaseFunction)item).ResultInfo.GetType().Name)
                {
                    case "BindingList`1":
                        switch (((BaseFunction)item).ResultInfo.GetType().GetGenericArguments()[0].Name)
                        {
                            case nameof(MeasureResultInfo):
                                BindingList<MeasureResultInfo> measureResultInfo = ((BaseFunction)item).ResultInfo as BindingList<MeasureResultInfo>;
                                if (measureResultInfo == null) break;
                                foreach (var itemTemp in measureResultInfo)
                                {
                                    if (itemTemp.IsOutput)
                                    {
                                        dataList.Add((itemTemp.Mea_Value * itemTemp.Scale + itemTemp.Compensate).ToString());
                                        stdValueList.Add(itemTemp.Std_Value.ToString());
                                        upToleranceList.Add(itemTemp.LimitUp.ToString());
                                        downToleranceList.Add(itemTemp.LimitDown.ToString());
                                        if (itemTemp.State == "NG")
                                            this.resultState = "NG";
                                    }
                                }
                                break;
                        }
                        break;
                }
            }
            this.dataList.Add(this.resultState); // 将结果判定添加进去
        }


        private void OnDataSend(DataSendEventArgs e)
        {
            var eventHandler = DataSend;
            if (eventHandler != null)
                eventHandler(this, e);
        }


        #region  实现接口的部分
        public OperateResult Execute(params object[] param)
        {
            this.Result.Succss = false;
            try
            {
                GetResultValue2(this.DataList);
                OnDataSend(new DataSendEventArgs(this.dataList, this.upToleranceList, this.downToleranceList, this.stdValueList));
                this.Result.Succss = true;
            }
            catch (Exception ex)
            {
                LoggerHelper.Error(this.name + "->执行错误" + ex);
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
                    return this.name;
                case "计算结果":
                    return this.dataList; //
                default:
                    if (this.name == propertyName)
                        return this.dataList;
                    else return null;
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
            // throw new NotImplementedException();
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

using Common;
using HalconDotNet;
using Light;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FunctionBlock
{
    [Serializable]
    public class BaseFunction : INotifyPropertyChanged
    {
        protected string name = "";
        private string dataSignName = "";
        private BindingList<LightParam> _lightParam; // 存储当前的光源参数
        public static event PointCloudAcqCompleteEventHandler PointsCloudAcqComplete;
        public static event ExcuteCompletedEventHandler ExcuteCompleted; // 将该对象的执行结果显示出来
        public event PropertyChangedEventHandler PropertyChanged;
        public static event ItemDeleteEventHandler ItemDelete;
        protected DataTable resultDataTable = new DataTable();
        private object resultInfo;

        private Dictionary<string, IFunction> refSource1 = new Dictionary<string, IFunction>();

        private Dictionary<string, IFunction> refSource2 = new Dictionary<string, IFunction>();

        private Dictionary<string, IFunction> refSource3 = new Dictionary<string, IFunction>();

        private Dictionary<string, IFunction> refSource4 = new Dictionary<string, IFunction>();

        public Dictionary<string, IFunction> RefSource1
        {
            get
            {
                return refSource1;
            }
            set
            {
                refSource1 = value;
            }
        }
        public Dictionary<string, IFunction> RefSource2 // 键引用属性名，值引用属性对应的对象
        {
            get
            {
                return refSource2;
            }
            set
            {
                refSource2 = value;
            }
        }
        public Dictionary<string, IFunction> RefSource3
        {
            get
            {
                return refSource3;
            }
            set
            {
                refSource3 = value;
            }
        }
        public Dictionary<string, IFunction> RefSource4
        {
            get
            {
                return refSource4;
            }
            set
            {
                refSource4 = value;
            }
        }
        public DataTable ResultDataTable
        {
            get
            {
                return resultDataTable;
            }
            set
            {
                resultDataTable = value;
            }
        }

        [DisplayName("执行结果")]
        [DescriptionAttribute("输出属性")]
        public OperateResult Result { get; set; }

        public string DataSignName { get => dataSignName; set => dataSignName = value; }
        public string Name { get => name; set => name = value; }
        public BindingList<LightParam> LightParam { get => _lightParam; set => _lightParam = value; }


        [DisplayName("结果信息")]
        [DescriptionAttribute("输出属性")]
        public object ResultInfo { get => resultInfo; set => resultInfo = value; }

        public BaseFunction()  // 默认基类型
        {
            /////////////////////////////
            this.Result = new OperateResult();
            this.LightParam = new BindingList<LightParam>(); // 最好都初始化变量，  
        }
        protected void CreateResultInfo(int count)
        {
            if (this.ResultInfo != null)
            {
                switch (this.ResultInfo.GetType().GetGenericArguments()[0].Name)
                {
                    case nameof(MeasureResultInfo):
                        if (((BindingList<MeasureResultInfo>)this.ResultInfo).Count <= count)
                        {
                            int initValue = ((BindingList<MeasureResultInfo>)this.ResultInfo).Count;
                            for (int i = initValue; i < count; i++)
                            {
                                try
                                {
                                    ((BindingList<MeasureResultInfo>)this.ResultInfo).Add(new MeasureResultInfo());
                                }
                                catch
                                {
                                }
                            }
                        }
                        else
                        {
                            int initValue = ((BindingList<MeasureResultInfo>)this.ResultInfo).Count;
                            for (int i = initValue; i > count; i--)
                            {
                                try
                                {
                                    ((BindingList<MeasureResultInfo>)this.ResultInfo).RemoveAt(i - 1);
                                }
                                catch
                                {
                                }
                            }
                        }
                        break;
                    case nameof(OcrResultInfo):
                        if (((BindingList<OcrResultInfo>)this.ResultInfo).Count <= count)
                        {
                            int initValue = ((BindingList<OcrResultInfo>)this.ResultInfo).Count;
                            for (int i = initValue; i < count; i++)
                            {
                                try
                                {
                                    ((BindingList<OcrResultInfo>)this.ResultInfo).Add(new OcrResultInfo());
                                }
                                catch
                                {
                                }
                            }
                        }
                        else
                        {
                            int initValue = ((BindingList<OcrResultInfo>)this.ResultInfo).Count;
                            for (int i = initValue; i > count; i--)
                            {
                                try
                                {
                                    ((BindingList<OcrResultInfo>)this.ResultInfo).RemoveAt(i - 1);
                                }
                                catch
                                {
                                }
                            }
                        }
                        break;
                }
            }

        }

        protected bool[] GetResultState(Dictionary<string, IFunction> sourceData)
        {
            bool[] result = new bool[sourceData.Count];
            foreach (var item in sourceData)
            {
            }
            return result;
        }
        protected object[] GetPropertyValue(Dictionary<string, IFunction> sourceData)
        {
            List<object> valueList = new List<object>();
            object value = null;
            PropertyInfo propertyInfo;
            foreach (var item in sourceData)
            {
                if (item.Value != null)
                {
                    // 方法一，通过反射来获取属性值
                    propertyInfo = item.Value.GetType().GetProperty(item.Key.Split('.').Last());  // 获取对象的指定属性
                    if (propertyInfo == null) continue;
                    value = propertyInfo.GetValue(item.Value);
                    // 方法二，通过调用获取属性方法来获取属性值
                    //value = GlobalProgram.ProgramItems[item.Key.Substring(0, item.Key.LastIndexOf('.'))].GetPropertyValues(item.Key.Split('.').Last());
                    ////////////////////////////////////
                    if (value != null)
                    {
                        switch (value.GetType().Name)
                        {
                            case "BindingList`1": // 表示类型为 绑定集合类
                                switch (value.GetType().GetGenericArguments()[0].Name)
                                {
                                    case nameof(ReadCommunicateCommand):
                                        BindingList<ReadCommunicateCommand> readPLcAdress = value as BindingList<ReadCommunicateCommand>;
                                        if (readPLcAdress != null)
                                        {
                                            foreach (var item2 in readPLcAdress)
                                            {
                                                if (item2.IsOutput)
                                                    valueList.Add(item2.ReadValue);
                                            }
                                        }
                                        break;
                                    case nameof(MeasureResultInfo):
                                        BindingList<MeasureResultInfo> measureResultInfo = value as BindingList<MeasureResultInfo>;
                                        if (measureResultInfo != null)
                                        {
                                            foreach (var item2 in measureResultInfo)
                                            {
                                                if (item2.IsOutput)
                                                    valueList.Add(item2.Mea_Value * item2.Scale);
                                            }
                                        }
                                        break;
                                    case nameof(OcrResultInfo):
                                        BindingList<OcrResultInfo> ocrResultInfo = value as BindingList<OcrResultInfo>;
                                        if (ocrResultInfo != null)
                                        {
                                            foreach (var item2 in ocrResultInfo)
                                            {
                                                if (item2.IsOutput)
                                                    valueList.Add(item2.Mea_Value);
                                            }
                                        }
                                        break;
                                    default:
                                        break;
                                }
                                break;
                            case nameof(Result):
                                valueList.Add(((OperateResult)value).Succss);
                                break;
                            default:
                                ///// 这里最好区分下数组对象跟非数组对象
                                if (value != null && value is object[])
                                    valueList.AddRange(value as object[]);
                                else
                                    valueList.Add(value);
                                break;
                        }
                    }
                }
            }
            return valueList.ToArray();
        }

        protected string GetResultInfo(Dictionary<string, IFunction> sourceData)
        {
            List<object> valueList = new List<object>();
            BaseFunction function;
            foreach (var item in sourceData)
            {
                function = item.Value as BaseFunction;
                DescriptionAttribute descriptionCustomAttribute;
                PropertyInfo[] propertyInfos = item.Value?.GetType().GetProperties(); // 获取属性集合
                object bindingProperty = null;
                foreach (var item2 in propertyInfos)
                {
                    descriptionCustomAttribute = Attribute.GetCustomAttribute(item2, typeof(DescriptionAttribute)) as DescriptionAttribute;  // 获取属性的描述特性
                    if (descriptionCustomAttribute != null && descriptionCustomAttribute.Description == "结果信息")
                    {
                        bindingProperty = item2.GetValue(item.Value);
                        if (bindingProperty != null)
                            break;
                    }
                }
                if (bindingProperty != null) // 表示有数据写入到绑定集合中
                {
                    switch (bindingProperty.GetType().GetGenericArguments()[0].Name)
                    {
                        case nameof(ReadCommunicateCommand):
                            BindingList<ReadCommunicateCommand> readPLcAdress = bindingProperty as BindingList<ReadCommunicateCommand>;
                            if (readPLcAdress != null)
                            {
                                foreach (var item2 in readPLcAdress)
                                {
                                    if (item2.IsOutput)
                                        valueList.Add(item2.ReadValue);
                                }
                            }
                            break;
                        case nameof(MeasureResultInfo):
                            BindingList<MeasureResultInfo> measureResultInfo = bindingProperty as BindingList<MeasureResultInfo>;
                            if (measureResultInfo != null)
                            {
                                foreach (var item2 in measureResultInfo)
                                {
                                    if (item2.IsOutput)
                                        valueList.Add(item2.Mea_Value);
                                }
                            }
                            break;
                        case nameof(OcrResultInfo):
                            BindingList<OcrResultInfo> ocrResultInfo = bindingProperty as BindingList<OcrResultInfo>;
                            if (ocrResultInfo != null)
                            {
                                foreach (var item2 in ocrResultInfo)
                                {
                                    if (item2.IsOutput)
                                        valueList.Add(item2.Mea_Value);
                                }
                            }
                            break;
                        default:
                            PropertyInfo propertyInfo = item.Value.GetType().GetProperty(item.Key.Split('.').Last());  // 获取对象的指定属性
                            if (propertyInfo == null) continue;
                            object value = propertyInfo.GetValue(item.Value);
                            switch (value.GetType().Name)
                            {
                                case "Double[]":
                                    double[] temp = (double[])value;
                                    foreach (var temp2 in temp)
                                        valueList.Add(temp2);
                                    break;
                                case "Double":
                                    valueList.Add((double)value);
                                    break;
                                case "Object[]":
                                    valueList.AddRange(value as object[]);
                                    break;
                                default:
                                    valueList.Add(value);
                                    break;
                            }
                            break;
                    }
                }
                else
                {
                    PropertyInfo propertyInfo = item.Value.GetType().GetProperty(item.Key.Split('.').Last());  // 获取对象的指定属性
                    if (propertyInfo == null) continue;
                    object value = propertyInfo.GetValue(item.Value);
                    switch (value.GetType().Name)
                    {
                        case "Double[]":
                            double[] temp = (double[])value;
                            foreach (var temp2 in temp)
                                valueList.Add(temp2);
                            break;
                        case "Double":
                            valueList.Add((double)value);
                            break;
                        case "Object[]":
                            valueList.AddRange(value as object[]);
                            break;
                        default:
                            valueList.Add(value);
                            break;
                    }
                }
            }
            return string.Join(",", valueList.ToArray());
        }
        protected string GetDefautPropertyName(object sourceData)
        {
            if (sourceData == null)
                throw new ArgumentNullException("sourceData");
            ///////////////////////////////////////////
            DefaultPropertyAttribute attribute = Attribute.GetCustomAttribute(sourceData.GetType(), typeof(DefaultPropertyAttribute)) as DefaultPropertyAttribute;
            if (attribute == null)
            {
                throw new ArgumentException("指定的对象未定义默认属性");
            }
            else
                return attribute.Name;
        }
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null) //
        {
            var eventHandler = this.PropertyChanged;
            if (eventHandler != null)
                eventHandler(this, new PropertyChangedEventArgs(propertyName));
        }
        protected void OnItemDeleteEvent(object itemValue, string itemName)
        {
            if (ItemDelete != null)
            {
                ItemDelete(this, new ItemDeleteEventArgs(itemValue, itemName));
                MemoryManager.Instance.RemoveOf(this.Name);
                //MemoryManager.Instance.Remove(this.Name + "." + "Result" + "." + "IsEmpty");
            }
        }
        public void OnExcuteCompleted(string propertyName, object propertyValue) // 这个引发事件的方法只是对源方法的封装 dataSourceName
        {
            if (ExcuteCompleted != null)
                ExcuteCompleted(this, new ExcuteCompletedEventArgs(propertyName, propertyValue)); //这里的this指：谁调用这个方法，this就指向谁
        }
        public void OnExcuteCompleted(string camName, string propertyName, object propertyValue) // 这个引发事件的方法只是对源方法的封装 
        {
            if (ExcuteCompleted != null)
                ExcuteCompleted(this, new ExcuteCompletedEventArgs(camName, propertyName, propertyValue)); //这里的this指：谁调用这个方法，this就指向谁
        }
        public void OnExcuteCompleted(string camName, string viewName, string propertyName, object propertyValue) // 这个引发事件的方法只是对源方法的封装 
        {
            if (ExcuteCompleted != null)
                ExcuteCompleted(this, new ExcuteCompletedEventArgs(camName, viewName, propertyName, propertyValue)); //这里的this指：谁调用这个方法，this就指向谁
        }
        protected void OnPointsCloudAcqComplete(HObjectModel3D propertyVlaue, string propertyName)
        {
            if (PointsCloudAcqComplete != null && propertyVlaue != null)
            {
                PointsCloudAcqComplete(this, new PointCloudAcqCompleteEventArgs(propertyName, propertyVlaue));
            }
        }
        protected void OnImageAcqComplete(ImageDataClass imageData, string sensorVlaue)
        {
            //if (ImageAcqComplete != null && imageData != null)
            //{
            //    ImageAcqComplete(new ImageAcqCompleteEventArgs(imageData, sensorVlaue), OnEventCompleted, nameof(PointsCloudAcqComplete));
            //}
        }
        private void OnEventCompleted(IAsyncResult ar)
        {
            LoggerHelper.Info(ar.AsyncState.ToString() + " 异步执行完成");
        }
        protected void ClearHandle(HObjectModel3D[] hObjectModel3Ds)
        {
            try
            {
                if (hObjectModel3Ds != null)
                {
                    foreach (var item in hObjectModel3Ds)
                    {
                        if (item != null && item.IsInitialized())
                            item.Dispose();
                    }
                }
            }
            catch
            {
            }
        }
        protected void ClearHandle(HObjectModel3D hObjectModel3Ds)
        {
            try
            {
                if (hObjectModel3Ds != null && hObjectModel3Ds.IsInitialized())
                    hObjectModel3Ds.Dispose();
            }
            catch
            {
            }
        }

        protected void UpdataNodeElementStyle(object param, bool result)
        {
            if (param != null)
            {
                object[] oo = param as object[];
                TreeNode node;
                foreach (var item in oo)
                {
                    if (result)
                    {
                        if (item is TreeNode)
                        {
                            node = ((TreeNode)item);
                            node.ForeColor = Color.Black;
                        }
                    }
                    else
                    {
                        if (item is TreeNode)
                        {
                            node = ((TreeNode)item);
                            node.ForeColor = Color.Red;
                        }
                    }
                }
            }
            /////////////////////////////////////////////////
            //MemoryManager.Instance.AddValue(this.Name + "." + "Result" + "." + "Succss", this.Result.Succss);
            //MemoryManager.Instance.AddValue(this.Name + "." + "Result" + "." + "IsEmpty", this.Result.IsEmpty);
        }



    }

    public class OprateResult
    {
        public bool Succss { get; set; }
        public bool IsEmpty { get; set; }
    }
}

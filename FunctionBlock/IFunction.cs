using Sensor;
using FunctionBlock;
using MotionControlCard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HalconDotNet;
using System.Windows.Forms;
using System.ComponentModel;
using Common;

namespace FunctionBlock
{
    // 执行程序运行时的计算,所有的算子都要来实现这个接口，以便于统一
    public interface IFunction
    {

        /// <summary>
        /// 运行程序项目  
        /// </summary>
        /// <param name="axisName"></param>
        /// <param name="speed"></param>
        OperateResult Execute(params object[] param);

        /// <summary>
        /// 获取属性名对应的值
        /// </summary>
        /// <returns></returns>
        object GetPropertyValues(string propertyName);

        /// <summary>
        /// 设置属性名对应的属性值
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="value"></param>
        bool SetPropertyValues(string propertyName, params object[] value);

        void ReleaseHandle();

        string Name { get; set; }

        void Read(string path);
        void Save(string path);

    }

    [Serializable]
    public class OperateResult
    {
        public bool Succss { get; set; }
        public bool IsEmpty { get; set; }
        public string ErrorMessage { get; set; }
        public enExcuteState ExcuteState { get; set; }

        /// <summary>
        /// 用于判断标签的状态
        /// </summary>
        public string LableResult { get; set; }

        public string Path { get; set; }

        public object  DataContent { get; set; }


        public OperateResult()
        {
            this.Succss = false;
            this.IsEmpty = false;
            this.ErrorMessage = "";
            this.ExcuteState = enExcuteState.NONE;
            this.LableResult = "";
            this.Path = "";
            this.DataContent = null;
        }

    }


    public enum enExcuteState
    {
        NONE,
        中断,
        换盘,
        继续,
    }

}

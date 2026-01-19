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

    /// <summary>
    /// 给脚本调用提供一个统一的操作接口
    /// </summary>
    public interface IScript
    {

        /// <summary>
        /// 运行脚本 
        /// </summary>
        /// <param name="axisName"></param>
        /// <param name="speed"></param>
        bool RunScript(IFunction function, params object[] param);

        /// <summary>
        /// 获取属性名对应的值
        /// </summary>
        /// <returns></returns>
        object GetScriptPropertyValues(string propertyName);

        /// <summary>
        /// 设置属性名对应的属性值
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="value"></param>
        bool SetScriptPropertyValues(string propertyName, object value);

    }


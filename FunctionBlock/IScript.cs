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
    /// <summary>
    /// 脚本接口
    /// </summary>
    public interface IScript
    {

        /// <summary>
        /// 运行程序项目  
        /// </summary>
        /// <param name="axisName"></param>
        /// <param name="speed"></param>
        OperateResult RunScript(params object[] param);

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

    }



}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FunctionBlock
{
    [Serializable]
   public class SystemParam
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public SystemParam()
        {
            this.ShieldDetect = false;
            this.Language = "zh";
        }

        /// <summary>
        /// 屏蔽检测
        /// </summary>
        public bool ShieldDetect
        {
            set;
            get;
        }

        /// <summary>
        /// 当前文化语言
        /// </summary>
        public string Language
        {
            set;
            get;
        }

        /// <summary>
        /// 定义全局的数据存储路径
        /// </summary>
        public string DataSavePath
        {
            set;
            get;
        }

        /// <summary>
        /// 机台自动运行状态
        /// </summary>
        public bool IsAutoRun { get; set; }

        public enInterruptType InterruptSingle { get; set; }

        public string ProjectName { get; set; }

        /// <summary>
        /// 启用卡尺手动定位，用于定位失败时，手动确认抓取位置
        /// </summary>
        public bool  EnableManualCalliper { get; set; }

        public bool DisablePageSwitch { get; set; }
        /// <summary>
        /// 同步夹爪参数
        /// </summary>
        public bool IsSynJawParam { get; set; }

        /// <summary>
        /// 同步相机参数
        /// </summary>
        public bool IsSynCamParam { get; set; }

        public string GlobalSocketName { get; set; } = "NONE";
        public uint ColumCount { get; set; } = 1;

        // 启用弹窗
        public bool EnablePopUpWindows { get; set; } = false;

        public bool IsFormTopMost { get; set; } = false;

        public bool IsInitSensor { get; set; } = true;


    }

    public enum enInterruptType
    {
        NONE,
        PLC复位中断,
        用户复位中断,
    }


}

﻿using System;
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

    }

    public enum enInterruptType
    {
        NONE,
        PLC复位中断,
    }



}

using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionBlock
{
    [Serializable]
    public class ScaleParam
    {

        /// <summary>
        /// 构造函数
        /// </summary>
        public ScaleParam()
        {
            this.Radius = 5;
            this.StepDist = 10;
            this.IsShowCrossMark = true;
            this.IsShowCircleMark = false;
            this.IsShowScaleMark = false;
            this.Color = enColor.orange;
        }

        public enColor Color
        {
            set;
            get;
        }
        /// <summary>
        /// 屏蔽检测
        /// </summary>
        public bool IsShowCircleMark
        {
            set;
            get;
        }

        public bool IsShowScaleMark
        {
            set;
            get;
        }

        public bool IsShowCrossMark
        {
            set;
            get;
        }

        /// <summary>
        /// 当前文化语言
        /// </summary>
        public double  Radius
        {
            set;
            get;
        }

        /// <summary>
        /// 定义全局的数据存储路径
        /// </summary>
        public double  StepDist
        {
            set;
            get;
        }



    }
}

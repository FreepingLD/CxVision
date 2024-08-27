using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionBlock
{
    public class AlignGuidedParam
    {
        public AlignGuidedParam()
        {
            this.JawName = enRobotJawEnum.Jaw1;
            this.X = 0;
            this.Y = 0;
            this.Describe = "";
            this.IsActive = true;
        }

        /// <summary> 夹爪枚举</summary>
        public enRobotJawEnum JawName { get; set; }

        /// <summary> 夹爪中心相对与旋转中心的偏移量X</summary>
        public double X { get; set; }

        /// <summary> 夹爪中心相对与旋转中心的偏移量Y</summary>
        public double Y { get; set; }

        public string Describe { get; set; }

        /// <summary> 是否激活该夹爪 </summary>
        public bool IsActive { get; set; }


    }


}

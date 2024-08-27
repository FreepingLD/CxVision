using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionBlock
{
    public class VariationModelParam
    {
        public int Widht
        {
            get;
            set;
        }

        public int Height
        {
            get;
            set;
        }

        public enMode Mode
        {
            get;
            set;
        }

        public enImageType ImageType
        {
            get;
            set;
        }



        public VariationModelParam()
        {
            /// 模型匹配参数
            Widht = 640;
            Height = 480;
            Mode = enMode.direct;
            ImageType = enImageType.Byte;
        }

    }
    public enum enImageType
    {
        Byte,
        int2,
        uint2,
    }

    public enum enMode
    {
        standard,
        robust,
        direct
    }


}

using HalconDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionBlock
{
    [Serializable]
    public class DoImageArithmetic
    {
        public ImageArithmeticParam ArithmeticParam { get; set; }
        public ImageArithmeticMethod ArithmeticMethod { get; set; }
        public DoImageArithmetic()
        {
            ArithmeticParam = new MultAddParam();
            ArithmeticMethod = new ImageArithmeticMethod();
        }


        public bool Do(HImage hImage1, HImage hImage2, out HImage MorImage)
        {
            MorImage = new HImage();
            if (hImage1 == null)
                throw new ArgumentNullException("hImage1");
            if (hImage2 == null)
                throw new ArgumentNullException("hImage2");
            if (ArithmeticParam == null)
                throw new ArgumentNullException("ArithmeticParam");
            ////////////////////////////////////////////////
            MorImage = ArithmeticMethod.ArithmeticImage(hImage1, hImage2, this.ArithmeticParam);
            return true;
        }


    }
}

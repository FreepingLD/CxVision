using HalconDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionBlock
{
    [Serializable]
    public class DoImageEnhancement
    {
        public ImageEnhancementParam EnhancementParam { get; set; }
        public ImageEnhancementMethod EnhancementMethod { get; set; }
        
        public DoImageEnhancement()
        {
            this.EnhancementParam = new EmphasizeParam();
            this.EnhancementMethod = new ImageEnhancementMethod();
        }


        public bool Do(HImage hImage,out HImage MorImage)
        {
            MorImage = new HImage();
            if (hImage == null)
                throw new ArgumentNullException("hImage");
            if (EnhancementParam == null)
                throw new ArgumentNullException("EnhancementParam");
            ////////////////////////////////////////////////
            MorImage = this.EnhancementMethod.ImageEnhancement(hImage, this.EnhancementParam);
            return true;
        }



    }
}

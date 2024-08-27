using HalconDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionBlock
{
    [Serializable]
    public class DoImageFilter
    {
        public ImageFilterParam FilterParam { get; set; }
        public ImageFilterMethod FilterMethod { get; set; }
        
        public DoImageFilter()
        {
            this.FilterParam = new ImageFilterParam();
            this.FilterMethod = new ImageFilterMethod();
        }


        public bool Do(HImage hImage,out HImage MorImage)
        {
            MorImage = new HImage();
            if (hImage == null)
                throw new ArgumentNullException("hImage");
            if (FilterParam == null)
                throw new ArgumentNullException("FilterParam");
            ////////////////////////////////////////////////
            MorImage = this.FilterMethod.ImageFilter(hImage, this.FilterParam);
            return true;
        }


    }
}

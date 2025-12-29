using HalconDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionBlock
{
    [Serializable]
    public class DoImageZoom
    {
        public ImageZoomParam ZoomParam { get; set; }
        public ImageZoomMethod ZoomMethod { get; set; }
        
        public DoImageZoom()
        {
            this.ZoomParam = new ZoomImageFactorParam();
            this.ZoomMethod = new ImageZoomMethod();
        }


        public bool Do(HImage hImage,out HImage MorImage)
        {
            MorImage = new HImage();
            if (hImage == null)
                throw new ArgumentNullException("hImage");
            if (ZoomParam == null)
                throw new ArgumentNullException("EnhancementParam");
            ////////////////////////////////////////////////
            MorImage = this.ZoomMethod.ImageZoom(hImage, this.ZoomParam);
            return true;
        }



    }
}

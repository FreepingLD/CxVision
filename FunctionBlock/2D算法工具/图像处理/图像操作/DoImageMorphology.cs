using HalconDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionBlock
{
    [Serializable]
    public class DoImageMorphology
    {
        public ImageMorphologyParam MorphologyParam { get; set; }

        private ImageMorphologyMethod _ImageMorphologyMethod;
        public DoImageMorphology()
        {
            _ImageMorphologyMethod = new ImageMorphologyMethod();
            MorphologyParam = new ImageRectParam();
        }

        public bool Do(HImage hImage,out HImage MorImage)
        {
            MorImage = new HImage();
            if (hImage == null)
                throw new ArgumentNullException("hImage");
            if (MorphologyParam == null)
                throw new ArgumentNullException("MorphologyParam");
            ////////////////////////////////////////////////
            MorImage = _ImageMorphologyMethod.ImageMorphology(hImage, this.MorphologyParam);
            return true;
        }





    }
}

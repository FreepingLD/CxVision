using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionBlock
{
    public class CreateVariationModelMethod
    {


        public HalconDotNet.HVariationModel create_variation_model(enVariationModelMethod VariationModelMethod, int width, int height, string type, string mode)
        {

            HalconDotNet.HVariationModel hVariationModel;
            switch (VariationModelMethod)
            {
                default:
                case enVariationModelMethod.单张图像:
                    hVariationModel =  new HalconDotNet.HVariationModel(width, height, type, "direct");
                    break;
                case enVariationModelMethod.多张图像:
                    hVariationModel = new HalconDotNet.HVariationModel(width, height, type, mode);
                    break;
            }
            return hVariationModel;
        }


    }

    public enum enVariationModelMethod
    {
        单张图像,
        多张图像,
    }
}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Light
{
    [Serializable]
    public class LightFactory
    {

        /// <summary>
        /// 获取传感器对象 
        /// </summary>
        /// <param name="lightType"></param>
        /// <returns></returns>
        public static ILightControl GetLight(enLightType lightType)
        {
            switch (lightType)
            {
                case enLightType.沃德谱:// "沃德谱":
                    return new WoDePuLight();
                case enLightType.IO板卡: // "IO板卡":
                    return new PDCM602454();
                case enLightType.盟拓: // "IO板卡":
                    return new MentuoLight();
                case enLightType.OPT: 
                    return new OptLight();
                case enLightType.PPX盘鑫: 
                    return new PpxLight();
                case enLightType.NONE: // "NONE":
                default:
                    return null; 
            }
        }

    }

    /// <summary>
    /// 光源品牌
    /// </summary>
    public enum enLightBrand
    {
        NONE,
        沃德谱,
        OPT,
        IO板卡,
        盟拓,
        PPX盘鑫,
    }


}

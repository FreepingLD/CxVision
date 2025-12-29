using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionBlock
{
    public class FunctionManage
    {

        private static List<IFunction> coordSystemList = new List<IFunction>();
        private static List<IFunction> imageSourceList = new List<IFunction>();
        public static List<IFunction> CoordSystemList { get => coordSystemList; set => coordSystemList = value; }
        public static List<IFunction> ImageSourceList { get => imageSourceList; set => imageSourceList = value; }

        public IFunction CurrentCoordSystem
        {
            get
            {
                if (coordSystemList.Count > 0)
                    return coordSystemList.Last();
                else
                    return null;
            }
        }
        public IFunction CurrentImageSource
        {
            get
            {
                if (imageSourceList.Count > 0)
                    return imageSourceList.Last();
                else
                    return null;
            }
        }
        public IFunction GetCurrentCoordSystem()
        {
            if (coordSystemList.Count > 0)
                return coordSystemList.Last();
            else
                return null;
        }
        public IFunction GetCurrentImageSource()
        {
            if (imageSourceList.Count > 0)
                return imageSourceList.Last();
            else
                return null;
        }




    }
}

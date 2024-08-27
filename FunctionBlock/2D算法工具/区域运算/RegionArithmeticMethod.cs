using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HalconDotNet;
using System.Drawing;
using Sensor;
using MotionControlCard;
using System.Windows.Forms;
using System.IO;
using AlgorithmsLibrary;
using System.ComponentModel;
using Common;

namespace FunctionBlock
{
    [Serializable]
    public class RegionArithmeticMethod
    {
        public static bool ArithmeticRegion(HRegion hRegion1, HRegion hRegion2, RegionArithmeticParam param, out HRegion tarRegion)
        {
            bool result = false;
            HRegion tempRegion = new HRegion();
            tarRegion = new HRegion();
            if (hRegion1 == null)
                throw new ArgumentNullException("hRegion1");
            if (hRegion2 == null)
                throw new ArgumentNullException("hRegion2");
            if (param == null)
                throw new ArgumentNullException("param");
            /////////////////////////////////
            switch (param.Method)
            {
                case enRegionArithmeticMethod.交集:
                    tempRegion = hRegion1.Union1().Intersection(hRegion2.Union1()).Connection();
                    break;
                case enRegionArithmeticMethod.差集:
                    tempRegion = hRegion1.Union1().Difference(hRegion2.Union1()).Connection();
                    break;
                case enRegionArithmeticMethod.补集:
                    tempRegion = hRegion1.Union1().Complement().Connection();
                    break;
                case enRegionArithmeticMethod.并集:
                    tempRegion = hRegion1.Union1().Union2(hRegion2.Union1()).Connection();
                    break;
            }
            /////////////////////////////////////
            switch (param.RegionOperate)
            {
                case enRegionOperate.opening_rectangle1:
                    tarRegion = tempRegion.OpeningRectangle1(param.Width, param.Height);
                    break;
                case enRegionOperate.closing_rectangle1:
                    tarRegion = tempRegion.ClosingRectangle1(param.Width, param.Height);
                    break;
                default:
                    tarRegion = tempRegion.Clone();
                    break;
            }
            tempRegion?.Dispose();
            result = true;
            return result;
        }



    }

}

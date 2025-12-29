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
    public class ContourReduceMethod
    {


        public static bool ReduceContour(userWcsPolyLine polyLine,userWcsCoordSystem wcsCoordSystem, BindingList<ReduceContourParam> Param, out userWcsPolyLine wcsPolyLine)
        {
            bool result = false;
            wcsPolyLine = new userWcsPolyLine();
            if (polyLine == null)
                throw new ArgumentNullException("polyLine 对象为空或没有初始化");
            if (Param == null)
                throw new ArgumentNullException(" Param ");
            /////////////////////////////////////////////////////////////////////
            foreach (var item in Param)
            {
                WcsROI roi = item.RoiShape.AffineWcsROI(wcsCoordSystem.GetHomMat2D()); 

                HXLDCont cont = new HXLDCont(); 
            }
            result = true;
            /////////////////////////////
            return result;
        }





    }
}

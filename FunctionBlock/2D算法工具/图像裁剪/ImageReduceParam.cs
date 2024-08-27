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
    public class ImageReduceParam 
    {

        public  bool InvertRegion { get; set; }
        public userPixCoordSystem PixCoordSys { get; set; }
        public BindingList<drawPixRect2> ReduceRegion { get; set; }


        public ImageReduceParam()
        {
            this.InvertRegion = false;
            this.PixCoordSys = new userPixCoordSystem();
            this.ReduceRegion = new BindingList<drawPixRect2>();
        }

 








    }
}

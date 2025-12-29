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
    public class LinesGauss 
    {
        private double sigma=1.5;
        private double low=3;
        private double high=8;
        private string lightDark= "light";
        private string completeJunctions= "false";
        private string extractWidth= "true";
        private string lineModel= "bar-shaped";

        public double Sigma { get => sigma; set => sigma = value; }
        public double High { get => high; set => high = value; }
        public string LightDark { get => lightDark; set => lightDark = value; }
        public string CompleteJunctions { get => completeJunctions; set => completeJunctions = value; }
        public string ExtractWidth { get => extractWidth; set => extractWidth = value; }
        public string LineModel { get => lineModel; set => lineModel = value; }
        public double Low { get => low; set => low = value; }


        //
        public static string[] LightDarkParam = { "dark", "light" };

        public static string[] ExtractWidthParam = { "false", "true" };

        public static string[] LineModelParam = { "bar-shaped", "gaussian", "none", "parabolic" };

        public static string[] CompleteJunctionsParam = { "false", "true" };


        public  HXLDCont lines_gauss(HImage image)
        {
            HXLDCont Edges = null;
            if (image == null) return new HXLDCont();
            Edges = image.LinesGauss(Sigma,  Low, High, LightDark, ExtractWidth, LineModel, CompleteJunctions);
            return Edges;
        }



    }
}

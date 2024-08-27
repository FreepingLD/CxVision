using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HalconDotNet;
using Sensor;
using MotionControlCard;
using System.Windows.Forms;
using System.IO;
using AlgorithmsLibrary;
using System.ComponentModel;
using Common;
using System.Data;

namespace FunctionBlock
{
    [Serializable]
    public class PointToPointDistParam 
    {
        public PointToPointDistParam()
        {

        }

        public bool CalculatePointToPointDist(userWcsPoint point1, userWcsPoint point2, out double maxDist, out double levelDist, out double verticalDist, out userWcsLine distLine)
        {
            bool result = false;
            if (point1 == null)
            {
                throw new ArgumentNullException("point1");
            }
            if (point2 == null)
            {
                throw new ArgumentNullException("point2");
            }
            ///////////////////////////////////////////////////
            maxDist = Math.Sqrt((point1.X - point2.X) * (point1.X - point2.X) + (point1.Y - point2.Y) * (point1.Y - point2.Y) + (point1.Z - point2.Z) * (point1.Z - point2.Z));
            levelDist = Math.Abs(point1.X - point2.X);
            verticalDist = Math.Abs(point1.Y - point2.Y);
            distLine = new userWcsLine(point1.X, point1.Y, point1.Z, point2.X, point2.Y, point2.Z, point1.CamParams);
            result = true;
            return result;
        }




    }
}

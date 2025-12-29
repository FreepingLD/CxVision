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
    public class PointToLineDistParam 
    {


        public PointToLineDistParam()
        {

        }


        public bool CalculatePointToLineDist2D(userWcsPoint Point, userWcsLine Line, out double distValue, out userWcsLine distLine)
        {
            bool result = false;
            if (Point == null)
            {
                throw new ArgumentNullException("Point");
            }
            if (Line == null)
            {
                throw new ArgumentNullException("Line");
            }
            double Proj_y, Proj_x;
            /////////////////////////////////
            HMisc.ProjectionPl(Point.Y, Point.X, Line.Y1, Line.X1, Line.Y2, Line.X2, out Proj_y, out Proj_x);
            distLine = new userWcsLine(Point.X, Point.Y, Point.Z, Proj_x, Proj_y, Point.Z, Point.CamParams);
            distValue = HMisc.DistancePl(Point.Y, Point.X, Line.Y1, Line.X1, Line.Y2, Line.X2);
            result = true;
            return result;
        }




    }
}

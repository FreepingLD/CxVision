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
    public class LineToLineDistParam
    {
       
        public LineToLineDistParam()
        {

        }

        private bool CalculateLineToLineDist2D(userWcsLine Line1, userWcsLine Line2, out double _maxDist, out double _minDist, out double _meanDist, out userWcsLine distLine)
        {
            bool result = false;
            if (Line1 == null)
            {
                throw new ArgumentNullException("Line1");
            }
            if (Line2 == null)
            {
                throw new ArgumentNullException("Line2");
            }
            double Proj_y, Proj_x;
            ///////////////////////////////////////////////////
            HMisc.ProjectionPl((Line1.Y1 + Line1.Y2) * 0.5, (Line1.X1 + Line1.X2) * 0.5, Line2.Y1, Line2.X1, Line2.Y2, Line2.X2, out Proj_y, out Proj_x);
            distLine = new userWcsLine((Line1.X1 + Line1.X2) * 0.5, (Line1.Y1 + Line1.Y2) * 0.5, (Line1.Z1 + Line1.Z2) * 0.5, Proj_x, Proj_y, (Line1.Z1 + Line1.Z2) * 0.5, Line1.CamParams);
            HTuple dist = HMisc.DistancePl(new HTuple(Line1.Y1, Line1.Y2), new HTuple(Line1.X1, Line1.X2), new HTuple(Line2.Y1), new HTuple(Line2.X1), new HTuple(Line2.Y2), new HTuple(Line2.X2));
            _maxDist = dist.TupleMax().D;
            _minDist = dist.TupleMin().D;
            _meanDist = dist.TupleMean().D;
            ///
            result = true;
            return result;
        }






    }
}

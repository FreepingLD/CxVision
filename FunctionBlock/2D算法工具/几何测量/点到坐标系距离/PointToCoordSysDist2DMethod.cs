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
    public class PointToCoordSysDist2DMethod
    {
        public PointToCoordSysDist2DMethod()
        {

        }

        public static bool PointToCoordSysDist2D(userWcsPoint point1, userWcsCoordSystem wcsCoordSystem, out double levelDist, out double verticalDist, out userWcsLine distLine, out userWcsLine distLine2)
        {
            bool result = false;
            if (point1 == null)
            {
                throw new ArgumentNullException("point1");
            }
            if (wcsCoordSystem == null)
            {
                throw new ArgumentNullException("wcsCoordSystem");
            }
            ///////////////////////////////////////////////////
            double x = wcsCoordSystem.CurrentPoint.X;
            double y = wcsCoordSystem.CurrentPoint.Y;
            double angle_x = wcsCoordSystem.CurrentPoint.Angle;
            double angle_y = wcsCoordSystem.CurrentPoint.Angle + 90;
            double Qx, Qy;
            HHomMat2D hHomMat2D = new HHomMat2D();
            hHomMat2D.VectorAngleToRigid(0, 0, 0, 0, 0, angle_x * Math.PI / 180);
            Qx = hHomMat2D.AffineTransPoint2d(10, 0, out Qy);
            double x1 = x + Qx;
            double y1 = y + Qy;
            hHomMat2D = new HHomMat2D();
            hHomMat2D.VectorAngleToRigid(0, 0, 0, 0, 0, angle_y * Math.PI / 180);
            Qx = hHomMat2D.AffineTransPoint2d(10, 0, out Qy);
            double x2 = x + Qx;
            double y2 = y + Qy;
            /////////////////////////////////////////////////
            levelDist = HMisc.DistancePl(point1.X, point1.Y, x, y, x1, y1);
            verticalDist = HMisc.DistancePl(point1.X, point1.Y, x, y, x2, y2);
            double proj_x1, proj_y1, proj_x2, proj_y2;
            HMisc.ProjectionPl(point1.X, point1.Y, x, y, x1, y1, out proj_x1, out proj_y1);
            HMisc.ProjectionPl(point1.X, point1.Y, x, y, x2, y2, out proj_x2, out proj_y2);
            /////////////////////////////////////////////////
            distLine = new userWcsLine(point1.X, point1.Y, point1.Z, proj_x1, proj_y1, 0, point1.CamParams);
            distLine2 = new userWcsLine(point1.X, point1.Y, point1.Z, proj_x2, proj_y2, 0, point1.CamParams);
            distLine2.Color = enColor.yellow;
            result = true;
            return result;
        }




    }
}

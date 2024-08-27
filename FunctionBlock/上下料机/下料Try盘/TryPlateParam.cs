using Common;
using MotionControlCard;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FunctionBlock
{
    [Serializable]
    public class TryPlateParam
    {
        public int RowCount { get; set; }
        public int ColCount { get; set; }
        public string CamName { get; set; }
        public string ViewWindow { get; set; }
        
        public BindingList<UserTryPlateHoleParam> CoordsList { get; set; }


        public TryPlateParam()
        {
            this.RowCount = 1;
            this.ColCount = 1;
            this.CamName = "Cam1";
            this.ViewWindow = "NONE";
            this.CoordsList = new BindingList<UserTryPlateHoleParam>();
        }






    }

    [Serializable]
    public class UserTryPlateHoleParam
    {
        [DisplayNameAttribute("穴位")]
        public string Describe { get; set; } = "NONE";

        [DisplayNameAttribute("X")]
        public double X { get; set; }
        [DisplayNameAttribute("Y")]
        public double Y { get; set; }
        [DisplayNameAttribute("Angle")]
        public double Angle { get; set; }

        [DisplayNameAttribute("Max_X")]
        public double LimitX { get; set; }

        [DisplayNameAttribute("Max_Y")]
        public double LimitY { get; set; }

        [DisplayNameAttribute("Max_Angle")]
        public double LimitAngle { get; set; }

        public enRobotJawEnum RobotJaw { get; set; } = enRobotJawEnum.Jaw1;

        public PixROI PixRoi { get; set; }

        public string Result { get; set; } = "NG";

        public UserTryPlateHoleParam Affine(HalconDotNet.HHomMat2D hHomMat2D)
        {
            if (hHomMat2D == null) return this;
            UserTryPlateHoleParam param = new UserTryPlateHoleParam();
            HalconDotNet.HTuple Qx, Qy;
            double sy, phi1, theta, tx, ty;
            hHomMat2D.HomMat2dToAffinePar(out sy, out phi1, out theta, out tx, out ty);
            Qx = hHomMat2D.AffineTransPoint2d(this.X, this.Y, out Qy);
            param.Describe = this.Describe;
            param.X = this.X;
            param.Y = this.Y;
            param.Angle = phi1 * 180 / Math.PI;
            param.LimitX = this.LimitX;
            param.LimitY = this.LimitY;
            param.LimitAngle = this.LimitAngle;
            param.RobotJaw = this.RobotJaw;

            return param;
        }



    }

    public enum enRefGrabPose
    {
        当前位置,
        标定位置,
    }





}

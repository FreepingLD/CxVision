using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using HalconDotNet;

namespace FunctionBlock
{
    public class RobotJawParam
    {
        public RobotJawParam()
        {
            this.JawName = enRobotJawEnum.夹抓1;
            this.CoordSysName = enCoordSysName.CoordSys_0;
            this.X = 0;
            this.Y = 0;
            this.Z = 0;
            this.Describe = "";
            this.IsActive = true;
            this.Angle = 0;
            this.U = 0;
            this.V = 0;
            this.Add_X = 0;
            this.Add_Y = 0;
        }

        /// <summary> 夹爪枚举</summary>
        public enRobotJawEnum JawName { get; set; }

        public enCoordSysName CoordSysName { get; set; }
        /// <summary> 夹爪中心相对与旋转中心的偏移量X</summary>
        public double X { get; set; }

        /// <summary> 夹爪中心相对与旋转中心的偏移量Y</summary>
        public double Y { get; set; }
        public double Z { get; set; }
        /// <summary> 夹爪中心相对与旋转中心的偏移量Y</summary>
        public double Angle { get; set; }

        public double U { get; set; }
        public double V { get; set; }
        public string Describe { get; set; }

        /// <summary> 是否激活该夹爪 </summary>
        public bool IsActive { get; set; }

        public double Add_X { get; set; }
        public double Add_Y { get; set; }
        public HHomMat2D GetHomMat2D()
        {
            HHomMat2D hHomMat2D = new HHomMat2D();
            hHomMat2D.VectorAngleToRigid(this.X - this.Add_X, this.Y - this.Add_Y, this.Angle * Math.PI / 180, 0, 0, 0);
            return hHomMat2D;
        }
        public HHomMat3D GetHomMat3D()
        {
            HPose hPose = new HPose(this.X, this.Y, this.Z, this.U, this.V, this.Angle, "Rp + T", "gba", "point");
            return hPose.PoseToHomMat3d();
        }
        public override string ToString()
        {
            return string.Join(",", this.X, this.Y, this.Z, this.Angle, this.U, this.V);
        }



    }

    public enum enRobotJawEnum
    {
        //所有的夹爪均可抓取
        NONE = 0,
        All,
        夹抓1,
        夹抓2,
        夹抓3,
        夹抓4,
        夹抓5,
        胶枪1,
        胶枪2,
        胶枪3,
        胶枪4,
        胶枪5,
        激光1,
        激光2,
        激光3,
        激光4,
        激光5,
        顶针1,
        顶针2,
    }
}

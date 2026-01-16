using HalconDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Common
{

    [Serializable]
    /// <summary>
    /// 类型序列化时一定要有一个无参构造函数
    /// </summary>
    public class UvwPlatformParam
    {
        // X1(U) 轴初始位置
        public double U_x { get; set; }
        public double U_y { get; set; }
        // X2(V) 轴初始位置
        public double V_x { get; set; }
        public double V_y { get; set; }
        // Y(W) 轴初始位置
        public double W_x { get; set; }
        public double W_y { get; set; }
        // Theta X1
        public double U_Angle { get; set; }
        // Theta X2
        public double V_Angle { get; set; }
        // Theta Y
        public double W_Angle { get; set; }
        // 旋转半径R1
        public double R1 { get; set; }
        // 旋转半径R2
        public double R2 { get; set; }
        public double Rot_X { get; set; }
        public double Rot_Y { get; set; }
        public double Rot_Deg { get; set; }

        public bool Enable { get; set; }
        public UvwPlatformParam()
        {
            // X1(U) 轴初始位置
            this.U_x = 98;
            this.U_y = 600;
            // X2(V) 轴初始位置
            this.V_x = -98;
            this.V_y = -600;
            // Y(W) 轴初始位置
            this.W_x = 0;
            this.W_y = 0;
            // Theta X1
            this.U_Angle = 80.724;
            // Theta X2
            this.V_Angle = 260.724;
            // Theta Y
            this.W_Angle = 0;
            // 旋转半径R1
            this.R1 = 607.95;
            // 旋转半径R2
            this.R2 = 0;

            this.Rot_X = 0;
            this.Rot_Y = 0;
            this.Rot_Deg = 0;

            this.Enable = false;
        }
        public UvwPlatformParam(bool isInit = true)
        {
            // X1(U) 轴初始位置,
            this.U_x = -400;
            this.U_y = 400;
            // X2(V)  (,-)
            this.V_x = 400;
            this.V_y = -400;
            // Y(W) 轴初始位置 (,)
            this.W_x = -400;
            this.W_y = -400;
            // Theta X1
            this.U_Angle = 135;
            // Theta X2
            this.V_Angle = 315;
            // Theta Y
            this.W_Angle = 225;
            // 旋转半径R1
            this.R1 = 565.685;
            // 旋转半径R2
            this.R2 = 0;

            this.Rot_X = 0;
            this.Rot_Y = 0;
            this.Rot_Deg = 0;

            this.Enable = false;
        }

        /// <summary>
        /// 通过矩阵来计算
        /// </summary>
        /// <param name="wcsVector"></param>
        /// <param name="U"></param>
        /// <param name="V"></param>
        /// <param name="W"></param>
        public void VectorToUVW(userWcsVector wcsVector, out double U, out double V, out double W)
        {
            HHomMat2D hHomMat2D = new HHomMat2D();
            hHomMat2D.VectorAngleToRigid(0.0, 0.0, 0.0, wcsVector.X, wcsVector.Y, wcsVector.Angle * Math.PI / 180);
            double UAdjX = 0, UAdjY = 0;
            UAdjX = hHomMat2D.AffineTransPoint2d(this.U_x, this.U_y, out UAdjY);
            double VAdjX = 0, VAdjY = 0;
            VAdjX = hHomMat2D.AffineTransPoint2d(this.V_x, this.V_y, out VAdjY);
            double WAdjX = 0, WAdjY = 0;
            WAdjX = hHomMat2D.AffineTransPoint2d(this.W_x, this.W_y, out WAdjY);
            /////////////////////////////
            U = UAdjX - U_x;
            V = VAdjX - V_x;
            W = WAdjY - W_y;
        }

        /// <summary>
        /// 通过矩阵来计算
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="angle"></param>
        /// <param name="U"></param>
        /// <param name="V"></param>
        /// <param name="W"></param>
        public void VectorToUVW(double x, double y, double angle, out double U, out double V, out double W)
        {
            HHomMat2D hHomMat2D = new HHomMat2D();
            hHomMat2D.VectorAngleToRigid(0.0, 0.0, 0.0, x, y, angle * Math.PI / 180);
            double UAdjX = 0, UAdjY = 0;
            UAdjX = hHomMat2D.AffineTransPoint2d(this.U_x, this.U_y, out UAdjY);
            double VAdjX = 0, VAdjY = 0;
            VAdjX = hHomMat2D.AffineTransPoint2d(this.V_x, this.V_y, out VAdjY);
            double WAdjX = 0, WAdjY = 0;
            WAdjX = hHomMat2D.AffineTransPoint2d(this.W_x, this.W_y, out WAdjY);
            /////////////////////////////
            U = UAdjX - U_x;
            V = VAdjX - V_x;
            W = WAdjY - W_y;
        }

        /// <summary>
        /// 绕指定点旋转指定角度
        /// </summary>
        /// <param name="Px"></param>
        /// <param name="Py"></param>
        /// <param name="angle"></param>
        /// <param name="U"></param>
        /// <param name="V"></param>
        /// <param name="W"></param>
        public void RotateToUVW(double Px, double Py, double angle, out double U, out double V, out double W)
        {
            HHomMat2D hHomMat2D = new HHomMat2D();
            hHomMat2D.VectorAngleToRigid(Px, Py, 0.0, Px, Py, angle * Math.PI / 180);
            double UAdjX = 0, UAdjY = 0;
            UAdjX = hHomMat2D.AffineTransPoint2d(this.U_x, this.U_y, out UAdjY);
            double VAdjX = 0, VAdjY = 0;
            VAdjX = hHomMat2D.AffineTransPoint2d(this.V_x, this.V_y, out VAdjY);
            double WAdjX = 0, WAdjY = 0;
            WAdjX = hHomMat2D.AffineTransPoint2d(this.W_x, this.W_y, out WAdjY);
            /////////////////////////////
            U = UAdjX - U_x;
            V = VAdjX - V_x;
            W = WAdjY - W_y;
        }

        /// <summary>
        /// 通过公式来计算
        /// </summary>
        /// <param name="angle"></param>
        /// <param name="U"></param>
        /// <param name="V"></param>
        /// <param name="W"></param>
        public void GetUvwCoord(double angle, out double U, out double V, out double W)
        {
            /////////////////////////
            U = this.R1 * Math.Cos((this.U_Angle + angle) * Math.PI / 180.0) - this.R1 * Math.Cos(this.U_Angle * Math.PI / 180.0);// 对应 X1轴
            V = this.R1 * Math.Cos((this.V_Angle + angle) * Math.PI / 180.0) - this.R1 * Math.Cos(this.V_Angle * Math.PI / 180.0); // 对应 X2轴
            W = this.R1 * Math.Sin((this.W_Angle + angle) * Math.PI / 180.0) - this.R1 * Math.Sin(this.W_Angle * Math.PI / 180.0); // 对应 Y轴
        }

        /// <summary>
        /// 通过公式来计算
        /// </summary>
        /// <param name="angle"></param>
        /// <param name="U"></param>
        /// <param name="V"></param>
        /// <param name="W"></param>
        public void GetUvwCoord(double x, double y, double angle, out double U, out double V, out double W)
        {
            /////////////////////////
            U = this.R1 * Math.Cos((this.U_Angle + angle) * Math.PI / 180.0) - this.R1 * Math.Cos(this.U_Angle * Math.PI / 180.0) + x;// 对应 X1轴
            V = this.R1 * Math.Cos((this.V_Angle + angle) * Math.PI / 180.0) - this.R1 * Math.Cos(this.V_Angle * Math.PI / 180.0) + x; // 对应 X2轴
            W = this.R1 * Math.Sin((this.W_Angle + angle) * Math.PI / 180.0) - this.R1 * Math.Sin(this.W_Angle * Math.PI / 180.0) + y; // 对应 Y轴
        }

    }
}

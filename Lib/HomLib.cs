using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HalconDotNet;

namespace Lib
{
    public class HomLib
    {
        public HHomMat2D GetHomMat2D(double [] Px, double[] Py, double[] Qx, double[] Qy)
        {
            HHomMat2D hHomMat2D = new HHomMat2D();
            if (Px == null)
                throw new ArgumentNullException("Px");
            if (Py == null)
                throw new ArgumentNullException("Py");
            if (Qx == null)
                throw new ArgumentNullException("Qx");
            if (Qy == null)
                throw new ArgumentNullException("Qy");
            if (Qx.Length != Px.Length || Qy.Length != Py.Length)
                throw new ArgumentException("数组长度不相等");
            hHomMat2D.VectorToHomMat2d(Px, Py, Qx,Qy);
            return hHomMat2D;
        }
        public void GetHomMat2DXYTheta(HHomMat2D hHomMat2D,out double x,out double y,out double angle)
        {
            double Sx, Sy, Phi, Theta, Tx, Ty;
            if (hHomMat2D == null)
                throw new ArgumentNullException("hHomMat2D");
            Sx = hHomMat2D.HomMat2dToAffinePar(out Sy, out Phi, out Theta, out Tx, out Ty);
            x = Tx;
            y = Ty;
            angle = Phi * 180 / Math.PI;
        }


    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MotionControlCard;
using System.Windows.Forms;
using System.Threading;
using HalconDotNet;
using Common;
using System.IO;
using Sensor;
using Command;
using AlgorithmsLibrary;
using System.ComponentModel;
using System.Data;


namespace FunctionBlock
{
    [Serializable]
    /// <summary>
    /// 激光取点采集的数据类
    /// </summary>
    public class CoordAffineParam3D
    {

        public  bool MapCoord(userWcsPoint[] sourcePoint, UserHomMat3D homMat3D, out userWcsPoint[] affinePoint)
        {
            bool result = false;
            if (sourcePoint == null)
            {
                throw new ArgumentNullException("sourcePoint");
            }
            if (homMat3D == null)
            {
                throw new ArgumentNullException("homMat3D");
            }
            HTuple Qx = new HTuple();
            HTuple Qy = new HTuple();
            HTuple Qz = new HTuple();
            double[] Px = new double[sourcePoint.Length];
            double[] Py = new double[sourcePoint.Length];
            double[] Pz = new double[sourcePoint.Length];
            HHomMat3D hHomMat3D = homMat3D.GetHHomMat3D();
            affinePoint = new userWcsPoint[sourcePoint.Length];
            ////////////////////////////////////////////////////////////////
            for (int i = 0; i < sourcePoint.Length; i++)
            {
                Px[i] = sourcePoint[i].X;
                Py[i] = sourcePoint[i].Y;
                Pz[i] = sourcePoint[i].Z;
            }
            Qx = hHomMat3D.AffineTransPoint3d(Px, Py, Pz, out Qy, out Qz);
            for (int i = 0; i < Qx.Length; i++)
            {
                affinePoint[i] = new userWcsPoint();
                affinePoint[i].X = Qx[i];
                affinePoint[i].Y = Qy[i];
                affinePoint[i].Z = Qz[i];
            }
            result = true;
            return result;
        }

    }

}

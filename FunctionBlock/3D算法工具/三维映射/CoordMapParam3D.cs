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
    public class CoordMapParam3D
    {
        public enTransformationType TransformationType { get; set; } = enTransformationType.affine;

        public UserHomMat3D GenMapMatrix(userWcsPoint[] sourcePoint, userWcsPoint[] targetPoint)
        {
            if (sourcePoint == null)
            {
                throw new ArgumentNullException("sourcePoint");
            }
            if (targetPoint == null)
            {
                throw new ArgumentNullException("targetPoint");
            }
            if (sourcePoint.Length != targetPoint.Length)
            {
                throw new ArgumentException("点对长度不相等");
            }
            UserHomMat3D homMat3D = new UserHomMat3D(true);
            double[] Px = new double[sourcePoint.Length];
            double[] Py = new double[sourcePoint.Length];
            double[] Pz = new double[sourcePoint.Length];
            double[] Qx = new double[sourcePoint.Length];
            double[] Qy = new double[sourcePoint.Length];
            double[] Qz = new double[sourcePoint.Length];
            for (int i = 0; i < sourcePoint.Length; i++)
            {
                Px[i] = sourcePoint[i].X;
                Py[i] = sourcePoint[i].Y;
                Pz[i] = sourcePoint[i].Z;
                Qx[i] = targetPoint[i].X;
                Qy[i] = targetPoint[i].Y;
                Qz[i] = targetPoint[i].Z;
            }
            HHomMat3D hHomMat3D = new HHomMat3D();
            hHomMat3D.VectorToHomMat3d(this.TransformationType.ToString().ToString(),Px, Py, Pz, Qx, Qy, Qz);
            homMat3D = new UserHomMat3D(hHomMat3D);
            return homMat3D;
        }


    }



}

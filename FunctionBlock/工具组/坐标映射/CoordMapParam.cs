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
    public class CoordMapParam
    {
        public string MapDirection { get; set; }

        public int MapCount { get; set; }



        public CoordPoint ConvertToCoordPoint(BindingList<CoordSysAxisParam> coordSysAxes)
        {
            if(coordSysAxes == null)
            {
                throw new ArgumentNullException("coordSysAxes");
            }
            CoordPoint coordPoint = new CoordPoint(coordSysAxes.Count);
            for (int i = 0; i < coordSysAxes.Count; i++)
            {
                coordPoint.X[i] = coordSysAxes[i].X;
                coordPoint.Y[i] = coordSysAxes[i].Y;
            }
            return coordPoint;
        }
        public UserHomMat2D GenMapMatrix(CoordPoint  CoordPoint_P, CoordPoint CoordPoint_Q)
        {
            if(CoordPoint_P == null)
            {
                throw new ArgumentNullException("CoordPoint_P");
            }
            if (CoordPoint_Q == null)
            {
                throw new ArgumentNullException("CoordPoint_Q");
            }
            if (CoordPoint_P.X.Length != CoordPoint_Q.X.Length)
            {
                throw new ArgumentException("点对长度不相等");
            }
            UserHomMat2D homMat2D = new UserHomMat2D(true);
            HTuple Px = new HTuple();
            HTuple Py = new HTuple();
            HTuple Qx = new HTuple();
            HTuple Qy = new HTuple();

            HHomMat2D hHomMat2D = new HHomMat2D();
            hHomMat2D.VectorToHomMat2d(CoordPoint_P.X, CoordPoint_P.Y, CoordPoint_Q.X, CoordPoint_Q.Y);
            //hHomMat2D.VectorToRigid(CoordPoint_P.X, CoordPoint_P.Y, CoordPoint_Q.X, CoordPoint_Q.Y);
            homMat2D = new UserHomMat2D(hHomMat2D);
            return homMat2D;
        }


    }

}

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
    public class CoordAffineParam
    {
        public bool MapCoord(CoordPoint[] CoordPoint, UserHomMat2D homMat2D, out CoordPoint[] CoordPointMap)
        {
            if (CoordPoint == null)
            {
                throw new ArgumentNullException("CoordPoint_P");
            }
            if (homMat2D == null)
            {
                throw new ArgumentNullException("homMat2D");
            }
            HTuple Qx = new HTuple();
            HTuple Qy = new HTuple();
            List<double> list_x = new List<double>();
            List<double> list_y = new List<double>();
            HHomMat2D hHomMat2D = homMat2D.GetHHomMat();
            CoordPointMap = new CoordPoint[CoordPoint.Length];
            ////////////////////////////////////////////////////////////////
            for (int i = 0; i < CoordPoint.Length; i++)
            {
                CoordPointMap[i] = new CoordPoint(CoordPoint[i].X.Length);
                if (CoordPoint[i].X.Length > 4)
                {
                    Qx = hHomMat2D.AffineTransPoint2d(CoordPoint[i].X, CoordPoint[i].Y, out Qy);
                    CoordPointMap[i].Sign = CoordPoint[i].Sign;
                    CoordPointMap[i].Count = CoordPoint[i].Count;
                    CoordPointMap[i].Row = CoordPoint[i].Row;
                    CoordPointMap[i].Col = CoordPoint[i].Col;
                    CoordPointMap[i].X = Qx.DArr;
                    CoordPointMap[i].Y = Qy.DArr;
                }
                else
                {
                    CoordPointMap[i].Sign = CoordPoint[i].Sign;
                    CoordPointMap[i].Count = CoordPoint[i].Count;
                    CoordPointMap[i].Row = CoordPoint[i].Row;
                    CoordPointMap[i].Col = CoordPoint[i].Col;
                    CoordPointMap[i].X = CoordPoint[i].X;
                    CoordPointMap[i].Y = CoordPoint[i].Y;
                }

            }
            return true;
        }



    }

}

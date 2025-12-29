using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HalconDotNet;
using System.Drawing;
using Sensor;
using MotionControlCard;
using System.Windows.Forms;
using System.IO;
using AlgorithmsLibrary;
using System.ComponentModel;
using Common;
using System.Data;
using Light;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using System.Diagnostics;

namespace FunctionBlock
{
    [Serializable]
    public class TrackComposeMethod
    {

        /// <summary>
        /// 合并轨迹点的Z值
        /// </summary>
        /// <param name="camTrackPoint"></param>
        /// <param name="laserTrackPoint"></param>
        /// <param name="composeParam"></param>
        /// <param name="wcsPolyLine"></param>
        /// <returns></returns>
        public static bool ComposeTrack(userWcsPoint[] camTrackPoint, userWcsPoint[] laserTrackPoint, TrackComposeParam composeParam, out userWcsPolyLine wcsPolyLine)
        {
            bool result = false;
            wcsPolyLine = new userWcsPolyLine();
            if (camTrackPoint == null) throw new ArgumentNullException("camTrackPoint");
            if (laserTrackPoint == null) throw new ArgumentNullException("laserTrackPoint");
            if (composeParam == null) throw new ArgumentNullException("composeParam");
            if (camTrackPoint.Length == 0) return false;
            if (laserTrackPoint.Length == 0) return false;
            int count = camTrackPoint.Length;
            double[] Z = new double[laserTrackPoint.Length];
            for (int i = 0; i < laserTrackPoint.Length; i++)
            {
                Z[i] = laserTrackPoint[i].Z;
            }
            double min_x, max_x;
            HTuple XValues, YValues;
            HFunction1D hFunction1D = new HFunction1D(Z);
            hFunction1D.XRangeFunct1d(out min_x, out max_x);
            HFunction1D hFunction1DSample = hFunction1D.SampleFunct1d(min_x, max_x, (max_x - min_x) / (count - 1), "constant");
            hFunction1DSample.Funct1dToPairs(out XValues, out YValues);
            int index = 0;
            wcsPolyLine.CamParams = camTrackPoint[0].CamParams;
            foreach (var item in camTrackPoint)
            {
                wcsPolyLine.Add(item.X, item.Y, YValues[index].D);
            }
            result = true;
            return result;
        }




    }


}

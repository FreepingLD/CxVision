using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionBlock
{
    [Serializable]
    public class TrackStackMethod
    {
        public static bool StackTrack(userWcsPoint[] wcsPoint,TrackStackParam param, out userWcsPolyLine wcsPolyLine)
        {
            bool result = false;
            wcsPolyLine = new userWcsPolyLine();
            if (wcsPoint == null) throw new ArgumentNullException("wcsPoint");
            if (param == null) throw new ArgumentNullException("param");
            if (wcsPoint.Length == 0) return false;
            wcsPolyLine.CamParams = wcsPoint[0].CamParams;
            for (int i = 0; i < param.Count; i++)
            {
                foreach (var item in wcsPoint)
                {
                    wcsPolyLine.Add(item.X, item.Y, item.Z + i* param.Dist);
                }              
            }
            result = true;
            return result;
        }

    }


}

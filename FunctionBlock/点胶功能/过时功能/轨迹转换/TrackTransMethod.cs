using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace FunctionBlock
{
    [Serializable]
    public class TrackTransMethod
    {

        public static bool TransTrack(userWcsPoint[] wcsPoint, TrackTransParam param, out userWcsPolyLine wcsPolyLine)
        {
            bool result = false;
            wcsPolyLine = new userWcsPolyLine();
            if (wcsPoint == null) throw new ArgumentNullException("wcsPoint");
            if (param == null) throw new ArgumentNullException("param");
            if (wcsPoint.Length == 0) return false;
            CameraParam camera = wcsPoint[0].CamParams;
            wcsPolyLine.CamParams = camera;
            double wcs_x, wcs_y, wcs_z;
            foreach (var item in wcsPoint)
            {
                userWcsPoint wcsPointTemp = new userWcsPoint(item.X, item.Y, item.Z, item.Grab_x, item.Grab_y, camera);
                userPixPoint pixPoint = wcsPointTemp.GetPixPoint();
                /// 这里需要用新的映射方法来转换一次   坐标类型这里需强制为:映射变换
                camera.ImagePointsToWorldPlane(pixPoint.Row, pixPoint.Col, pixPoint.Grab_x, pixPoint.Grab_y, pixPoint.Grab_z,enCoordOriginType.映射变换, param.Method, out wcs_x, out wcs_y, out wcs_z);
                wcsPolyLine.Add(wcs_x, wcs_y, wcs_z);
            }
            result = true;
            return result;
        }

    }


}

using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionBlock
{
    public class CoordTransformParam
    {
        public enRobotJawEnum Jaw { get; set; } = enRobotJawEnum.Jaw1;
        public string CamName { get; set; } = "Cam1";



        public bool calculate(userWcsPoint wcsPoint)
        {
            bool result = false;
            if (wcsPoint == null)
                throw new ArgumentNullException("wcsPoint");
            CameraParam camParam = Sensor.SensorManage.GetSensor(this.CamName)?.CameraParam;
            RobotJawParam jawParam = RobotJawParaManager.Instance.GetJawParam(this.Jaw);
            if(camParam != null && jawParam != null)
            {
                double wcs_x = 0, wcs_y = 0, wcs_z = 0;
                camParam.ImageCenterPointsToWorldPlane(wcsPoint.Grab_x, wcsPoint.Grab_y, wcsPoint.Grab_z, out wcs_x, out wcs_y, out wcs_z); // 表示图像中心点在标定坐标系中的位置 
                jawParam.X = wcs_x - wcsPoint.X;  // 胶枪的位置
                jawParam.Y = wcs_y - wcsPoint.Y;
                jawParam.Z = wcs_z - wcsPoint.Z;
                result = true;
            }
            return result;
        }



    }
}

using Sensor;
using MotionControlCard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HalconDotNet;
using System.Threading;
using Common;
using System.IO;
using Command;
using AlgorithmsLibrary;
using System.ComponentModel;
using System.Data;


namespace FunctionBlock
{
    [Serializable]
    /// <summary>
    /// 激光扫描采集的数据类
    /// </summary>
    public class CalibrateCameraPose
    {
        private double imageWidth = 2048;
        private double imageHeight = 1536;
        public string Method
        {
            get
            {
                return method;
            }

            set
            {
                method = value;
            }
        }
        public double Threshold
        {
            get
            {
                return threshold;
            }

            set
            {
                threshold = value;
            }
        }

        public double ImageWidth
        {
            get
            {
                return imageWidth;
            }

            set
            {
                imageWidth = value;
            }
        }

        public double ImageHeight
        {
            get
            {
                return imageHeight;
            }

            set
            {
                imageHeight = value;
            }
        }

        private string method = "iterative";
        private double threshold = 120;

        public void GetCurrentPosition(AcqSource _acqSource, out double x_Tcoordinate, out double y_Tcoordinate, out double z_Tcoordinate) // 保存后有意义的参数和保存后没意义的参数
        {
            try
            {
                _acqSource.GetAxisPosition(enAxisName.X轴, out x_Tcoordinate);
                _acqSource.GetAxisPosition(enAxisName.Y轴, out y_Tcoordinate);
                _acqSource.GetAxisPosition(enAxisName.Z轴, out z_Tcoordinate);
            }
            catch
            {
                throw new Exception();
            }
        }
        public void VectorToPose(HTuple WorldX, HTuple WorldY, HTuple WorldZ, HTuple ImageRow, HTuple ImageColumn, HTuple CameraParam,  out HTuple camPose)
        {
            camPose = null;
            HTuple quality = -1;
            HTuple Pose = null, homMat3d, homMat3dRotate;
            if (CameraParam == null) MessageBox.Show("相机内参不能为空");
            switch (this.method)
            {
                case "analytic":
                    HOperatorSet.VectorToPose(WorldX, WorldY, WorldZ, ImageRow, ImageColumn, CameraParam, "analytic", "error", out Pose, out quality);
                    break;
                case "iterative":
                    HOperatorSet.VectorToPose(WorldX, WorldY, WorldZ, ImageRow, ImageColumn, CameraParam, "iterative", "error", out Pose, out quality);
                    break;
                case "planar_analytic":
                    HOperatorSet.VectorToPose(WorldX, WorldY, new HTuple(), ImageRow, ImageColumn, CameraParam, "planar_analytic", "error", out Pose, out quality);
                    break;
            }
            HOperatorSet.PoseToHomMat3d(Pose, out homMat3d);
            HOperatorSet.HomMat3dRotateLocal(homMat3d, Math.PI, "x", out homMat3dRotate);
            HOperatorSet.HomMat3dToPose(homMat3dRotate, out camPose);
            ///////微调坐标系的旋转,使相机外参与机台坐标系平行
            camPose[3] = 180;
            camPose[4] = 0;
            camPose[5] = 0;
        }
        public void FindCircle(HImage image,HWindow window, out double center_Row, out double center_Column)
        {
            HTuple Row, Column, Radius, StartPhi, EndPhi, PointOrder;
            center_Row = 0;
            center_Column = 0;
            HXLDCont circle = image.ThresholdSubPix(this.threshold).UnionCocircularContoursXld(0.5, 0.1, 0.2, 30, 10, 10, "true", 3);
            HTuple length = circle.LengthXld();
            circle = circle.SelectShapeXld("contlength", "or", length.TupleMax().D - 100, length.TupleMax().D + 100);
            ///////////////////////
            circle.FitCircleContourXld("algebraic", -1, 0, 0, 3, 2, out Row, out Column, out Radius, out StartPhi, out EndPhi, out PointOrder);
            circle.GenCircleContourXld(Row, Column, Radius, StartPhi, EndPhi, PointOrder, 0.001);
            window.SetColor("red");
            window.DispObj(circle);
            if (Row!=null && Row.Length>0)
            {
                center_Row = Row[0].D;
                center_Column = Column[0].D;
            }
        }
        public enum enMethod
        {
            analytic,
            iterative,
            planar_analytic,
        }
        public void DisplayCoordSystem(HWindow window,HTuple camPara,HTuple camPose, HTuple X,HTuple Y, HTuple Z)
        {
            HTuple objectModel3d;
            try
            {
                // camPose位姿与对象位姿是互为相反的
                window.ClearWindow();
                HOperatorSet.GenObjectModel3dFromPoints(X, Y, Z, out objectModel3d);
                window.SetPart(0, 0, camPara[7].D - 1, camPara[6].D - 1);
                HOperatorSet.DispObjectModel3d(window, objectModel3d, new HTuple(), new HTuple(), new HTuple("lut", "intensity", "disp_pose"), new HTuple("rainbow", "coord_z","true"));
                HOperatorSet.ClearObjectModel3d(objectModel3d);
            }
            catch
            {

            }

        }




    }
}

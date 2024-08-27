using Common;
using HalconDotNet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    [Serializable]
    [System.Xml.Serialization.XmlInclude(typeof(CaliParam))]
    public class CaliParam
    {
        public enCoordSysName CoordSysName { get; set; }
        public enCoordValueType CoordValueType { get; set; } //
        public enCamCaliModel CamCaliModel { get; set; }
        /// <summary> 相机是否随着X轴移动</summary>
        public bool IsMoveX { get; set; }
        /// <summary> 相机是否随着Y轴移动 </summary>
        public bool IsMoveY { get; set; }
        public string Describe { get; set; }

        /// <summary>标定起始点X   </summary> 
        public userWcsVector StartCaliPoint { get; set; }
        /// <summary>标定终止点 </summary>
        public userWcsVector EndCalibPoint { get; set; }
        /// <summary>标定方法 </summary>
        public enCalibMethod CalibMethod { get; set; }

        /// <summary> 相机标定矩阵 </summary>
        public UserHomMat2D HomMat2D { get; set; }

        /// <summary> 相对位姿标定矩阵 </summary>
        public UserHomMat2D RelativeHomMat2D { get; set; }

        public object NONE { get; set; }

        public CaliParam()
        {
            this.CoordSysName = enCoordSysName.CoordSys_1;
            this.CoordValueType = enCoordValueType.相对坐标;
            this.CamCaliModel = enCamCaliModel.Cali9PtCali;
            this.IsMoveX = false;
            this.IsMoveY = false;
            this.HomMat2D = new UserHomMat2D(true);
            this.RelativeHomMat2D = new UserHomMat2D(true);
            this.Describe = "NONE";
            this.CalibMethod = enCalibMethod.先旋转后平移;
            this.StartCaliPoint = new userWcsVector();
            this.EndCalibPoint = new userWcsVector();
        }



    }





}

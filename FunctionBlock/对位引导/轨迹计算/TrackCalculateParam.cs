using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionBlock
{
    [Serializable]
    public class TrackCalculateParam
    {
        public enSortPoint SortMethod { get; set; }
        public string RotateAngle_x { get; set; }
        public string RotateAngle_y { get; set; }
        public string RotateAngle_z { get; set; }
        public string Mode { get; set; }
        public double Distance { get; set; }
        public enRobotJawEnum Jaw { get; set; }
        public bool IsShowNormalCont { get; set; }
        public bool IsInterpretation { get; set; }
        public double InterpretationDist { get; set; }


        public TrackCalculateParam()
        {
            this.SortMethod = enSortPoint.NONE;
            this.RotateAngle_x = "0";
            this.RotateAngle_y = "0";
            this.RotateAngle_z = "0";
            this.Mode = "regression_normal";
            this.Distance = 0.01;
            this.Jaw = enRobotJawEnum.NONE;
            this.IsInterpretation = false;
            this.IsShowNormalCont = false;
            this.InterpretationDist = 0.1;
        }


    }
    public enum enSortPoint
    {
        NONE,
        角度升序,
        角度降序,
        距离升序,
        距离降序,
    }



}

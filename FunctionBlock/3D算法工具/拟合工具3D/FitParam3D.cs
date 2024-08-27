using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionBlock
{
    public class FitParam3D
    {
        private string primitiveType = "";
        private string fittingAlgorithm = "least_squares_huber";
        private string output_point_coord = "false";

        public string PrimitiveType { get => primitiveType; set => primitiveType = value; }
        public string FittingAlgorithm { get => fittingAlgorithm; set => fittingAlgorithm = value; }
        public string OutputPointCoord { get => output_point_coord; set => output_point_coord = value; }
    }

    public class FitSphereParam3D : FitParam3D
    {
        public FitSphereParam3D()
        {
            this.PrimitiveType = "sphere";
        }
    }
    public class FitCylinderParam3D : FitParam3D
    {
        public FitCylinderParam3D()
        {
            this.PrimitiveType = "cylinder";
        }
    }
    public class FitPlaneParam3D : FitParam3D
    {
        public FitPlaneParam3D()
        {
            this.PrimitiveType = "plane";
        }
    }
    public class FitBoxParam3D : FitParam3D
    {
        public FitBoxParam3D()
        {
            this.PrimitiveType = "box";
        }
    }
    public class FitProfileParam3D : FitParam3D
    {
        public enFitCoordPoint FitCoordPoint { get; set; }
        public string FitMethod { get; set; }
        public FitProfileParam3D()
        {
            this.PrimitiveType = "Profile";
            this.FitCoordPoint = enFitCoordPoint.XZ;
            this.FitMethod = "algebraic";
        }
    }
    public enum enFitAlgorithm
    {
        least_squares,
        least_squares_huber,
        least_squares_tukey,
    }

}

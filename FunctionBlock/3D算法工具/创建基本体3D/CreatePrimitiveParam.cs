using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HalconDotNet;
using Sensor;
using MotionControlCard;
using System.Windows.Forms;
using Common;
using System.IO;
using System.ComponentModel;
using AlgorithmsLibrary;

namespace FunctionBlock
{
    [Serializable]
    public class PlanePrimitiveParam : PrimitiveParam
    {
        // plane
        private double min_xExtent = -10;
        private double min_yExtent = -10;
        private double max_xExtent = 10;
        private double max_yExtent = 10;
        public double Min_xExtent
        {
            get
            {
                return min_xExtent;
            }

            set
            {
                min_xExtent = value;
            }
        }
        public double Min_yExtent
        {
            get
            {
                return min_yExtent;
            }

            set
            {
                min_yExtent = value;
            }
        }
        public double Max_xExtent
        {
            get
            {
                return max_xExtent;
            }

            set
            {
                max_xExtent = value;
            }
        }
        public double Max_yExtent
        {
            get
            {
                return max_yExtent;
            }

            set
            {
                max_yExtent = value;
            }
        }

        public PlanePrimitiveParam()
        {
            this.PrimitiveType = enPrimitiveType.gen_plane_object_model_3d.ToString();
        }
        public override bool CreatePrimitive(userWcsPose wcsPose, out HObjectModel3D objectModel)
        {
            objectModel = new HObjectModel3D();
            objectModel.GenPlaneObjectModel3d(wcsPose.GetHPose(), new HTuple(this.max_xExtent, this.min_xExtent, this.min_xExtent, this.max_xExtent), new HTuple(this.max_yExtent, this.max_yExtent, this.min_yExtent, this.min_yExtent));
            return true;
        }
    }
    [Serializable]
    public class SpherePrimitiveParam : PrimitiveParam
    {
        // sphere
        private double radius = 10;
        public double Radius
        {
            get
            {
                return radius;
            }

            set
            {
                radius = value;
            }
        }

        public SpherePrimitiveParam()
        {
            this.PrimitiveType = enPrimitiveType.gen_sphere_object_model_3d.ToString();
        }
        public override bool CreatePrimitive(userWcsPose wcsPose, out HObjectModel3D objectModel)
        {
            objectModel = new HObjectModel3D();
            objectModel.GenSphereObjectModel3d(wcsPose.GetHPose(), this.radius);
            return true;
        }
    }
    [Serializable]
    public class CylinderPrimitiveParam : PrimitiveParam
    {
        // cylinder
        private double radius = 10;
        private double minExtent = 0;
        private double maxExtent = 10;
        public double MinExtent
        {
            get
            {
                return minExtent;
            }

            set
            {
                minExtent = value;
            }
        }
        public double MaxExtent
        {
            get
            {
                return maxExtent;
            }

            set
            {
                maxExtent = value;
            }
        }
        public double Radius { get => radius; set => radius = value; }

        public CylinderPrimitiveParam()
        {
            this.PrimitiveType = enPrimitiveType.gen_cylinder_object_model_3d.ToString();
        }
        public override bool CreatePrimitive(userWcsPose wcsPose, out HObjectModel3D objectModel)
        {
            objectModel = new HObjectModel3D();
            objectModel.GenCylinderObjectModel3d(wcsPose.GetHPose(), this.radius, this.minExtent, this.maxExtent);
            return true;
        }


    }
    [Serializable]
    public class BoxPrimitiveParam : PrimitiveParam
    {
        // box
        private double length_x = 10;
        private double length_y = 10;
        private double length_z = 10;
        public double Length_x
        {
            get
            {
                return length_x;
            }

            set
            {
                length_x = value;
            }
        }
        public double Length_y
        {
            get
            {
                return length_y;
            }

            set
            {
                length_y = value;
            }
        }
        public double Length_z
        {
            get
            {
                return length_z;
            }

            set
            {
                length_z = value;
            }
        }

        public BoxPrimitiveParam()
        {
            this.PrimitiveType = enPrimitiveType.gen_box_object_model_3d.ToString();
        }

        public override bool CreatePrimitive(userWcsPose wcsPose, out HObjectModel3D objectModel)
        {
            objectModel = new HObjectModel3D();
            objectModel.GenBoxObjectModel3d(wcsPose.GetHPose(), this.length_x, this.length_y, this.length_z);
            return true;
        }


    }


    [Serializable]
    public class PrimitiveParam
    {

        private string primitiveType = "gen_plane_object_model_3d";
        public string PrimitiveType
        {
            get
            {
                return primitiveType;
            }

            set
            {
                primitiveType = value;
            }
        }







        public virtual bool CreatePrimitive(userWcsPose wcsPose, out HObjectModel3D objectModel)
        {
            throw new NotImplementedException();
        }







    }
    public enum enPrimitiveType
    {
        gen_plane_object_model_3d,
        gen_sphere_object_model_3d,
        gen_cylinder_object_model_3d,
        gen_box_object_model_3d,
    }

}


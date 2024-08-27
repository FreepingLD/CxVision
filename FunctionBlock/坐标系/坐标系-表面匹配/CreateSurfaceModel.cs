using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HalconDotNet;

namespace FunctionBlock
{
    [Serializable]
    public class CreateSurfaceModel
    {

        public HSurfaceModel create_surface_model(HObjectModel3D surfaceObject, C_SurfaceModelParam SurfaceModelParam)
        {
            if (surfaceObject == null)
                throw new ArgumentNullException("surfaceObject");
            HSurfaceModel hSurfaceModel = surfaceObject.CreateSurfaceModel(SurfaceModelParam.RelSamplingDistance, new HTuple("model_invert_normals", "pose_ref_rel_sampling_distance"),
                new HTuple(SurfaceModelParam.ModelInvertNormals, SurfaceModelParam.PoseRefRelSamplingDistance));
            return hSurfaceModel;
        }

        public HDeformableSurfaceModel create_deformable_surface_model(HObjectModel3D surfaceObject, C_DeformableSurfaceModelParam DeformableSurfaceModelParam)
        {
            if (surfaceObject == null)
                throw new ArgumentNullException("surfaceObject");
            HDeformableSurfaceModel hSurfaceModel = surfaceObject.CreateDeformableSurfaceModel(
                DeformableSurfaceModelParam.RelSamplingDistance,
                new HTuple("model_invert_normals",  "scale_min", "scale_max", "bending_max", "stiffness"),
                new HTuple(DeformableSurfaceModelParam.ModelInvertNormals, DeformableSurfaceModelParam.MinScale, 
                DeformableSurfaceModelParam.MaxScale, DeformableSurfaceModelParam.MaxBending, DeformableSurfaceModelParam.Stiffness));
            return hSurfaceModel;
        }


    }
}

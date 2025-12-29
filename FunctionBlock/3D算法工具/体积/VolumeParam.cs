using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HalconDotNet;
using Sensor;
using MotionControlCard;
using System.Windows.Forms;
using System.IO;
using AlgorithmsLibrary;
using System.ComponentModel;
using Common;
using System.Data;

namespace FunctionBlock
{
    /// <summary>
    /// 体积的计算是需要基准面的
    /// </summary>
    [Serializable]
    public class VolumeParam
    {
        private double _Value; // 测量结果

        [DisplayName("体积")]
        [DescriptionAttribute("输出属性")]
        public double Value { get => _Value; set => _Value = value; }


        public int GreedyKNN { get; set; }

        public int GreedyHoleFilling { get; set; }

        public VolumeParam()
        {
            this.GreedyKNN = 20;
            this.GreedyHoleFilling = 200;
        }

        public bool CalculateVolumeRelativePlane(HObjectModel3D targetObjectModel, HObjectModel3D planeObjectModel, out double volume)
        {
            bool result = false;
            volume = 0;
            HObjectModel3D TriangulatedObjectModel3D = null;
            int ParamValue = 0;
            HTuple num = 0;
            HPose pose;
            if (targetObjectModel == null) throw new ArgumentNullException("planeObjectModel1", "参数为空或长度为0");
            if (planeObjectModel == null) throw new ArgumentNullException("planeObjectModel2", "参数为空或长度为0");
            HalconLibrary ha = new HalconLibrary();
            ///////////////////////////////////////////////////////
            ha.GetPlaneObjectModel3DPose(planeObjectModel, out pose);
            num = targetObjectModel.GetObjectModel3dParams("num_triangles");
            // 计算体积  
            if (num[0].D == 0) // 如果没有三角化，则先三角化处理
            {
                TriangulatedObjectModel3D = targetObjectModel.TriangulateObjectModel3d("greedy", new HTuple("greedy_kNN", "greedy_hole_filling"), new HTuple(this.GreedyKNN, this.GreedyHoleFilling), out ParamValue);
                volume = TriangulatedObjectModel3D.VolumeObjectModel3dRelativeToPlane(pose, "unsigned", "false");
                if (TriangulatedObjectModel3D.IsInitialized())
                {
                    TriangulatedObjectModel3D.Dispose();
                }
            }
            else
                volume = targetObjectModel.VolumeObjectModel3dRelativeToPlane(pose, "unsigned", "false");
            result = true;
            return result;
        }
        public bool CalculateVolumeRelativePlane(HObjectModel3D[] targetObjectModel, HObjectModel3D[] planeObjectModel, out double volume)
        {
            bool result = false;
            volume = 0;
            HObjectModel3D TriangulatedObjectModel3D = null;
            HObjectModel3D unionTargetObjectModel3D1 = null;
            HObjectModel3D unionPlaneObjectModel3D2 = null;
            int ParamValue = 0;
            HTuple num = 0;
            HPose pose;
            if (targetObjectModel == null) throw new ArgumentNullException("planeObjectModel1", "参数为空或长度为0");
            if (planeObjectModel == null) throw new ArgumentNullException("planeObjectModel2", "参数为空或长度为0");
            HalconLibrary ha = new HalconLibrary();
            ///////////////////////////////////////////////////////
            try
            {
                unionTargetObjectModel3D1 = HObjectModel3D.UnionObjectModel3d(targetObjectModel, "points_surface");
                unionPlaneObjectModel3D2 = HObjectModel3D.UnionObjectModel3d(planeObjectModel, "points_surface");
                ha.GetPlaneObjectModel3DPose(unionPlaneObjectModel3D2, out pose);
                num = unionTargetObjectModel3D1.GetObjectModel3dParams("num_triangles");
                // 计算体积  
                if (num[0].D == 0) // 如果没有三角化，则先三角化处理
                {
                    TriangulatedObjectModel3D = unionTargetObjectModel3D1.TriangulateObjectModel3d("greedy", new HTuple("greedy_kNN", "greedy_hole_filling"), new HTuple(40, 200), out ParamValue);
                    volume = TriangulatedObjectModel3D.VolumeObjectModel3dRelativeToPlane(pose, "unsigned", "false");
                    if (TriangulatedObjectModel3D.IsInitialized())
                    {
                        TriangulatedObjectModel3D.Dispose();
                    }
                }
                else
                    volume = unionTargetObjectModel3D1.VolumeObjectModel3dRelativeToPlane(pose, "unsigned", "false");
                result = true;
            }
            catch (Exception ex)
            {

            }
            finally
            {
                TriangulatedObjectModel3D?.ClearObjectModel3d();
                unionTargetObjectModel3D1?.ClearObjectModel3d();
                unionPlaneObjectModel3D2?.ClearObjectModel3d();
            }
            return result;
        }



    }
}

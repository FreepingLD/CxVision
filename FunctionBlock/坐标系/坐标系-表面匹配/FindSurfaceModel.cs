using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HalconDotNet;
using Common;


namespace FunctionBlock
{
    [Serializable]
    public class FindSurfaceModel
    {
        public HSurfaceMatchingResult[] find_surface_model(HSurfaceModel hSurfaceModel, HObjectModel3D hObjectModel3D, F_SurfaceModelParam findSurfaceModelParam, out SurfaceMatchingResult MatchingResult)
        {
            if (hSurfaceModel == null)
                throw new ArgumentNullException("hSurfaceModel");
            if (!hSurfaceModel.IsInitialized())
                throw new ArgumentNullException("hSurfaceModel 未初始化");
            MatchingResult = new SurfaceMatchingResult();
            HTuple score;
            HSurfaceMatchingResult[] hSurfaceMatchingResult;
            HPose[] pose = hSurfaceModel.FindSurfaceModel(
                hObjectModel3D,
                findSurfaceModelParam.RelSamplingDistance,
                findSurfaceModelParam.KeyPointFraction,
                findSurfaceModelParam.MinScore,
                findSurfaceModelParam.ReturnResultHandle.ToString().ToLower(),
                new HTuple("num_matches", "max_overlap_dist_rel", "scene_normal_computation", "sparse_pose_refinement", "score_type", "pose_ref_use_scene_normals", "dense_pose_refinement",
                "pose_ref_num_steps", "pose_ref_sub_sampling", "pose_ref_dist_threshold_rel", "pose_ref_scoring_dist_rel"),
                new HTuple(findSurfaceModelParam.MatchesNum, findSurfaceModelParam.MaxOverlapDistRel, findSurfaceModelParam.SceneNormalComputation, findSurfaceModelParam.SparsePoseRefinement,
               nameof(findSurfaceModelParam.ScoreType), findSurfaceModelParam.PoseRefUseSceneNormals, findSurfaceModelParam.DensePoseRefinement, findSurfaceModelParam.PoseRefNumSteps,
               findSurfaceModelParam.PoseRefSubSampling, findSurfaceModelParam.PoseRefDistThresholdRel, findSurfaceModelParam.PoseRefScoringDistRel),
                out score,
                out hSurfaceMatchingResult);
            ///////////////////////////////////////////////////
            MatchingResult = new SurfaceMatchingResult(pose.Length);
            for (int i = 0; i < pose.Length; i++)
            {
                MatchingResult.WcsPose3D[i] = new userWcsPose(pose[i]);
                MatchingResult.MatchScore[i] = score[i].D;
            }
            return hSurfaceMatchingResult;
        }

        public HDeformableSurfaceMatchingResult[] find_deformable_surface_model(HDeformableSurfaceModel hDeformableSurfaceModel, HObjectModel3D hObjectModel3D, F_DeformableSurfaceModelParam DeformableSurfaceModelParam, out SurfaceMatchingResult MatchingResult)
        {
            if (hDeformableSurfaceModel == null)
                throw new ArgumentNullException("hSurfaceModel");
            if (!hDeformableSurfaceModel.IsInitialized())
                throw new ArgumentNullException("hSurfaceModel 未初始化");
            MatchingResult = new SurfaceMatchingResult();
            HTuple score, pose;
            HDeformableSurfaceMatchingResult[] hDeformableSurfaceMatchingResult = null;

            score = hDeformableSurfaceModel.FindDeformableSurfaceModel(hObjectModel3D,
                DeformableSurfaceModelParam.RelSamplingDistance,
                new HTuple(DeformableSurfaceModelParam.MinScore),
                new HTuple("scene_normal_computation", "pose_ref_num_steps", "pose_ref_dist_threshold_rel", "PoseRefScoringDistRel"),
                new HTuple(DeformableSurfaceModelParam.SceneNormalComputation, DeformableSurfaceModelParam.PoseRefNumSteps, DeformableSurfaceModelParam.PoseRefDistThresholdRel, DeformableSurfaceModelParam.PoseRefScoringDistRel),
                out hDeformableSurfaceMatchingResult);
            //////////////////////////////////////////////////////////////
            if (hDeformableSurfaceMatchingResult != null)
            {
                MatchingResult = new SurfaceMatchingResult(hDeformableSurfaceMatchingResult.Length);
                for (int i = 0; i < hDeformableSurfaceMatchingResult.Length; i++)
                {
                    pose = hDeformableSurfaceMatchingResult[i]?.GetDeformableSurfaceMatchingResult("rigid_pose", 0); // The parameter ResultIndex is ignored
                    MatchingResult.WcsPose3D[i] = new userWcsPose(pose);
                    MatchingResult.MatchScore[i] = score[i].D;
                }
            }
            return hDeformableSurfaceMatchingResult;
        }


    }
}

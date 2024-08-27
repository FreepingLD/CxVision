using Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionBlock
{
    [Serializable]
    public class F_SurfaceModelParam : F_SurfaceModelParamBase
    {
        // 主要参数
        public double KeyPointFraction
        {
            get;
            set;
        }
        public bool ReturnResultHandle
        {
            get;
            set;
        }

        // 其他参数
        public int MatchesNum
        {
            get;
            set;
        }
        public double MaxOverlapDistRel
        {
            get;
            set;
        }

        // 稀疏位姿细化
        public bool SparsePoseRefinement
        {
            get;
            set;
        }
        public enScoreType ScoreType
        {
            get;
            set;
        }
        public bool PoseRefUseSceneNormals
        {
            get;
            set;
        }
        // 密集位姿细化
        public bool DensePoseRefinement
        {
            get;
            set;
        }
        public int PoseRefSubSampling
        {
            get;
            set;
        }


        public F_SurfaceModelParam()
        {
            this.RelSamplingDistance = 0.03;
            this.KeyPointFraction = 0.2;
            this.MinScore = 0;
            this.ReturnResultHandle = false;
            ////////
            this.SceneNormalComputation = enNormalComputationMethod.fast;
            this.PoseRefDistThresholdRel = 0.25;
            this.PoseRefScoringDistRel = 0.03;
            /////
            this.MatchesNum = 1;
            this.MaxOverlapDistRel =0.5;
            this.SparsePoseRefinement = true;
            this.ScoreType = enScoreType.model_point_fraction;
            this.PoseRefUseSceneNormals = false;

            this.DensePoseRefinement = true;
            this.PoseRefNumSteps = 5;
            this.PoseRefSubSampling = 2;

            /////////////////////////////
            this.SearchRegion = new BindingList<CropParam>();
        }

    }

    [Serializable]
    public class F_DeformableSurfaceModelParam: F_SurfaceModelParamBase
    {
        public F_DeformableSurfaceModelParam()
        {
            this.RelSamplingDistance = 0.05;
            this.MinScore = 0;
            this.SceneNormalComputation = enNormalComputationMethod.fast;
            this.PoseRefNumSteps = 25;
            this.PoseRefDistThresholdRel = 0.25;
            this.PoseRefScoringDistRel = 0.03;
            this.SearchRegion = new BindingList<CropParam>();
        }

    }
    public class F_SurfaceModelParamBase
    {
        public double RelSamplingDistance
        {
            get;
            set;
        }
        public enNormalComputationMethod SceneNormalComputation
        {
            get;
            set;
        }
        public double MinScore
        {
            get;
            set;
        }
        public double PoseRefNumSteps // 位姿细化的迭代次数
        {
            get;
            set;
        }
        public double PoseRefDistThresholdRel
        {
            get;
            set;
        }
        public double PoseRefScoringDistRel
        {
            get;
            set;
        }
        public BindingList<CropParam> SearchRegion
        {
            get;
            set;
        }
    }

    [Serializable]
    public class C_SurfaceModelParam : C_SurfaceModelParamBase
    {
        public double PoseRefRelSamplingDistance
        {
            get;
            set;

        }
        public C_SurfaceModelParam()
        {
            this.PoseRefRelSamplingDistance = 0.01;
            this.RelSamplingDistance = 0.05;
            this.ModelInvertNormals = "fasle";
            this.TemplateRegion = new BindingList<CropParam>();
        }
    }

    [Serializable]
    public class C_DeformableSurfaceModelParam : C_SurfaceModelParamBase
    {
        public  double MinScale { get; set; }
        public double MaxScale { get; set; }
        public double MaxBending { get; set; }
        public double Stiffness { get; set; }
        public C_DeformableSurfaceModelParam()
        {
            this.RelSamplingDistance = 0.05;
            this.ModelInvertNormals = "fasle";
            this.MinScale = 1;
            this.MaxScale = 1;
            this.MaxBending = 20;
            this.Stiffness = 0.5;

            this.TemplateRegion = new BindingList<CropParam>();
        }

    }

    [Serializable]
    public class C_SurfaceModelParamBase
    {
        public string ModelInvertNormals
        {
            get;
            set;
        }

        public double RelSamplingDistance
        {
            get;
            set;
        }

        public BindingList<CropParam> TemplateRegion
        {
            get;
            set;
        }


    }

    [Serializable]
    public struct SurfaceMatchingResult
    {
        public userWcsPose[] WcsPose3D;
        public double[] MatchScore;

        public SurfaceMatchingResult(int lenght)
        {
            this.WcsPose3D = new userWcsPose[lenght];
            this.MatchScore = new double[lenght];
        }
    }

    public enum enBoolea
    {
        True,
        False,  
    }
    public enum enNormalComputationMethod
    {
       fast,
       mls,
    }
    public enum enScoreType
    {
        num_scene_points,
        num_model_points,
        model_point_fraction,
    }


}

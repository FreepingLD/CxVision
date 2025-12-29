using HalconDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionBlock
{
    [Serializable]
    public class DeformableSurfaceMatch
    {
        private HDeformableSurfaceModel _DeformableSurfaceModel;
        private CreateSurfaceModel _createSurfaceModel;
        private FindSurfaceModel _findSurfaceModel;
        public C_DeformableSurfaceModelParam C_DeformableSurfaceModelParam { get; set; }
        public F_DeformableSurfaceModelParam F_DeformableSurfaceModelParam { get; set; }

        private SurfaceMatchingResult _MatchingResult;
        public SurfaceMatchingResult MatchingResult { get { return _MatchingResult; } set { this._MatchingResult = value; } }
        public HDeformableSurfaceMatchingResult[] DeformableSurfaceMatchingResult { get; set; }
        public HDeformableSurfaceModel DeformableSurfaceModel { get => _DeformableSurfaceModel; set => _DeformableSurfaceModel = value; }

        private HObjectModel3D _SampleModel = null;
        public HObjectModel3D SampleModel
        {
            get
            {
                if (this._DeformableSurfaceModel != null && this._DeformableSurfaceModel.IsInitialized())
                {
                    if (this._SampleModel != null && this._SampleModel.IsInitialized())
                        this._SampleModel.ClearHandle();
                    this._SampleModel = new HObjectModel3D(this._DeformableSurfaceModel.GetDeformableSurfaceModelParam("sampled_model").H);
                }
                return this._SampleModel;
            }
        }

        private HObjectModel3D _SampleScene = null;
        public HObjectModel3D SampleScene
        {
            get
            {
                if (DeformableSurfaceMatchingResult != null)
                {
                    if (this._SampleScene != null && this._SampleScene.IsInitialized())
                        this._SampleScene.ClearHandle();
                    this._SampleScene = new HObjectModel3D(GetDeformableSurfaceMatchingResult("sampled_scene").H);
                }
                return this._SampleScene;
            }
        }

        private HObjectModel3D _DeformedModel = null;
        public HObjectModel3D DeformedModel
        {
            get
            {
                if (DeformableSurfaceMatchingResult != null)
                {
                    if (this._DeformedModel != null && this._DeformedModel.IsInitialized())
                        this._DeformedModel.ClearHandle();
                    this._DeformedModel = new HObjectModel3D(GetDeformableSurfaceMatchingResult("deformed_model").H);
                }
                return this._DeformedModel;
            }
        }

        private HObjectModel3D _DeformedSampledModel = null;
        public HObjectModel3D DeformedSampledModel
        {
            get
            {
                if (DeformableSurfaceMatchingResult != null)
                {
                    if (this._DeformedSampledModel != null && this._DeformedSampledModel.IsInitialized())
                        this._DeformedSampledModel.ClearHandle();
                    this._DeformedSampledModel = new HObjectModel3D(GetDeformableSurfaceMatchingResult("deformed_sampled_model").H);
                }
                return this._DeformedSampledModel;
            }
        }


        public DeformableSurfaceMatch()
        {
            this._createSurfaceModel = new CreateSurfaceModel();
            this._findSurfaceModel = new FindSurfaceModel();
            this.C_DeformableSurfaceModelParam = new C_DeformableSurfaceModelParam();
            this.F_DeformableSurfaceModelParam = new F_DeformableSurfaceModelParam();
            this.DeformableSurfaceMatchingResult = new HDeformableSurfaceMatchingResult[0];
        }



        public HTuple GetDeformableSurfaceMatchingResult(string paramName = "sampled_scene")
        {
            HTuple value = new HTuple();
            if (DeformableSurfaceMatchingResult != null)
            {
                foreach (var item in DeformableSurfaceMatchingResult)
                    if (item.IsInitialized())
                        value.Append(item.GetDeformableSurfaceMatchingResult(paramName, 0));
                    else
                        return null;
            }
            return value;
        }
        public bool CreateDeformableSurfaceModel(HObjectModel3D surfaceObject, C_DeformableSurfaceModelParam SurfaceModelParam)
        {
            bool result = false;
            try
            {
                if (this._DeformableSurfaceModel != null && this._DeformableSurfaceModel.IsInitialized())
                    this._DeformableSurfaceModel.Dispose();
                this._DeformableSurfaceModel = this._createSurfaceModel.create_deformable_surface_model(surfaceObject, SurfaceModelParam);
                result = true;
            }
            catch
            {
                result = false;
            }
            return result;
        }

        public bool FindDeformableSurfaceModel(HObjectModel3D hObjectModel3D, F_DeformableSurfaceModelParam findSurfaceModelParam)
        {
            bool result = false;
            try
            {
                if (DeformableSurfaceMatchingResult != null)
                {
                    foreach (var item in DeformableSurfaceMatchingResult)
                    {
                        if (item.IsInitialized())
                            item.Dispose();
                    }
                }
                this.DeformableSurfaceMatchingResult = this._findSurfaceModel.find_deformable_surface_model(this._DeformableSurfaceModel, hObjectModel3D, findSurfaceModelParam, out _MatchingResult);
                result = true;
            }
            catch
            {
                result = false;
            }
            return result;
        }

        public bool ClearHandle()
        {
            bool result = false;
            try
            {
                if (DeformableSurfaceMatchingResult != null)
                    HDeformableSurfaceMatchingResult.ClearDeformableSurfaceMatchingResult(DeformableSurfaceMatchingResult);
                ///////////////////////////////////////
                if (this._SampleModel != null)
                    this._SampleModel.ClearHandle();
                if (this._SampleScene != null)
                    this._SampleScene.ClearHandle();
                if (this._DeformedModel != null)
                    this._DeformedModel.ClearHandle();
                if (this._DeformedSampledModel!= null)
                    this._DeformedSampledModel.ClearHandle();
            }
            catch
            {

            }
            return result;
        }
    }
}

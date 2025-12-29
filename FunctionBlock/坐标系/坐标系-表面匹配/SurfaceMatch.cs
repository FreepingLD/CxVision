using HalconDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionBlock
{
    [Serializable]
    public class SurfaceMatch
    {
        private HSurfaceModel surfaceModel;
        private CreateSurfaceModel _createSurfaceModel;
        private FindSurfaceModel _findSurfaceModel;
        public C_SurfaceModelParam C_SurfaceModelParam { get; set; }
        public F_SurfaceModelParam F_SurfaceModelParam { get; set; }

        private SurfaceMatchingResult _MatchingResult;
        public SurfaceMatchingResult MatchingResult { get { return _MatchingResult; } set { this._MatchingResult = value; } }
        public HSurfaceMatchingResult[] SurfaceMatchingResult { get; set; }
        public HSurfaceModel SurfaceModel { get => surfaceModel; set => surfaceModel = value; }

        private HObjectModel3D _SampleModel = null;
        public HObjectModel3D SampleModel
        {
            get
            {
                if (this.surfaceModel != null && this.surfaceModel.IsInitialized())
                {
                    if (this._SampleModel != null && this._SampleModel.IsInitialized())
                        this._SampleModel.ClearHandle();
                    this._SampleModel = new HObjectModel3D(this.surfaceModel.GetSurfaceModelParam("sampled_model").H);
                }
                return this._SampleModel;
            }
        }

        private HObjectModel3D _SampleScene = null;
        public HObjectModel3D SampleScene
        {
            get
            {
                if (SurfaceMatchingResult != null)
                {
                    if (this._SampleScene != null && this._SampleScene.IsInitialized())
                        this._SampleScene.ClearHandle();
                    this._SampleScene = new HObjectModel3D(GetSurfaceMatchingResult("sampled_scene").H);
                }
                return this._SampleScene;
            }
        }

        private HObjectModel3D _KeyPoints = null;
        public HObjectModel3D KeyPoints
        {
            get
            {
                if (SurfaceMatchingResult != null)
                {
                    if (this._KeyPoints != null && this._KeyPoints.IsInitialized())
                        this._KeyPoints.ClearHandle();
                    this._KeyPoints = new HObjectModel3D(GetSurfaceMatchingResult("key_points").H);
                }
                return this._KeyPoints;
            }
        }


        public SurfaceMatch()
        {
            this._createSurfaceModel = new CreateSurfaceModel();
            this._findSurfaceModel = new FindSurfaceModel();
            this.C_SurfaceModelParam = new C_SurfaceModelParam();
            this.F_SurfaceModelParam = new F_SurfaceModelParam();
            this.SurfaceMatchingResult = new HSurfaceMatchingResult[0];
        }


        public HTuple GetSurfaceMatchingResult(string paramName = "sampled_scene")
        {
            HTuple value = new HTuple();
            if (SurfaceMatchingResult != null)
            {
                foreach (var item in SurfaceMatchingResult)
                    if (item.IsInitialized())
                        value.Append(item.GetSurfaceMatchingResult(paramName, 0));
                    else
                        return null;
            }
            return value;
        }

        public bool CreateSurfaceModel(HObjectModel3D surfaceObject, C_SurfaceModelParam SurfaceModelParam)
        {
            bool result = false;
            HObjectModel3D unionObjectModel3D = null;
            HObjectModel3D[] reduceObjectModel3D = null;
            try
            {
                if (this.surfaceModel != null && this.surfaceModel.IsInitialized())
                    this.surfaceModel.Dispose();
                //new Rectangle2CropAlgorith().ReduceObjectModel3dByRectangle2(surfaceObject, SurfaceModelParam.TemplateRegion.ToArray(), out reduceObjectModel3D);
                unionObjectModel3D = HObjectModel3D.UnionObjectModel3d(reduceObjectModel3D, "points_surface");
                this.surfaceModel = this._createSurfaceModel.create_surface_model(surfaceObject, SurfaceModelParam);
                result = true;
            }
            catch
            {
                result = false;
            }
            finally
            {
                unionObjectModel3D?.Dispose();
                if (reduceObjectModel3D != null)
                {
                    foreach (var item in reduceObjectModel3D)
                    {
                        item?.Dispose();
                    }
                }

            }
            return result;
        }

        public bool CreateSurfaceModel(HObjectModel3D[] surfaceObject, C_SurfaceModelParam SurfaceModelParam)
        {
            bool result = false;
            HObjectModel3D unionObjectModel3D = null;
            HObjectModel3D unionObjectModel3D2 = null;
            HObjectModel3D[] reduceObjectModel3D = null;
            try
            {
                if (this.surfaceModel != null && this.surfaceModel.IsInitialized())
                    this.surfaceModel.Dispose();
                unionObjectModel3D = HObjectModel3D.UnionObjectModel3d(surfaceObject, "points_surface");
                //new Rectangle2CropAlgorith().ReduceObjectModel3dByRectangle2(unionObjectModel3D, SurfaceModelParam.TemplateRegion.ToArray(), out reduceObjectModel3D);
                unionObjectModel3D2 = HObjectModel3D.UnionObjectModel3d(reduceObjectModel3D, "points_surface");
                this.surfaceModel = this._createSurfaceModel.create_surface_model(unionObjectModel3D2, SurfaceModelParam);
                result = true;
            }
            catch
            {
                result = false;
            }
            finally
            {
                unionObjectModel3D?.Dispose();
                unionObjectModel3D2?.Dispose();
                if (reduceObjectModel3D != null)
                {
                    foreach (var item in reduceObjectModel3D)
                    {
                        item?.Dispose();
                    }
                }
            }
            return result;
        }

        public bool FindSurfaceModel(HObjectModel3D hObjectModel3D, F_SurfaceModelParam findSurfaceModelParam)
        {
            bool result = false;
            HObjectModel3D unionObjectModel3D = null;
            HObjectModel3D[] reduceObjectModel3D = null;
            try
            {
                if (SurfaceMatchingResult != null)
                {
                    foreach (var item in SurfaceMatchingResult)
                    {
                        if (item.IsInitialized())
                            item.Dispose();
                    }
                }
                //new Rectangle2CropAlgorith().ReduceObjectModel3dByRectangle2(hObjectModel3D, findSurfaceModelParam.SearchRegion.ToArray(), out reduceObjectModel3D);
                unionObjectModel3D = HObjectModel3D.UnionObjectModel3d(reduceObjectModel3D, "points_surface");
                this.SurfaceMatchingResult = this._findSurfaceModel.find_surface_model(this.surfaceModel, unionObjectModel3D, findSurfaceModelParam, out _MatchingResult);
                result = true;
            }
            catch
            {
                result = false;
            }
            finally
            {
                unionObjectModel3D?.Dispose();
                if (reduceObjectModel3D != null)
                {
                    foreach (var item in reduceObjectModel3D)
                    {
                        item?.Dispose();
                    }
                }

            }
            return result;
        }

        public bool FindSurfaceModel(HObjectModel3D[] hObjectModel3D, F_SurfaceModelParam findSurfaceModelParam)
        {
            bool result = false;
            HObjectModel3D unionObjectModel3D = null;
            HObjectModel3D unionObjectModel3D2 = null;
            HObjectModel3D[] reduceObjectModel3D = null;
            try
            {
                if (SurfaceMatchingResult != null)
                {
                    foreach (var item in SurfaceMatchingResult)
                    {
                        if (item.IsInitialized())
                            item.Dispose();
                    }
                }
                unionObjectModel3D = HObjectModel3D.UnionObjectModel3d(hObjectModel3D, "points_surface");
                //new Rectangle2CropAlgorith().ReduceObjectModel3dByRectangle2(unionObjectModel3D, findSurfaceModelParam.SearchRegion.ToArray(), out reduceObjectModel3D);
                unionObjectModel3D2 = HObjectModel3D.UnionObjectModel3d(reduceObjectModel3D, "points_surface");
                this.SurfaceMatchingResult = this._findSurfaceModel.find_surface_model(this.surfaceModel, unionObjectModel3D2, findSurfaceModelParam, out _MatchingResult);
                result = true;
            }
            catch
            {
                result = false;
            }
            finally
            {
                unionObjectModel3D?.Dispose();
                unionObjectModel3D2?.Dispose();
                if (reduceObjectModel3D != null)
                {
                    foreach (var item in reduceObjectModel3D)
                    {
                        item?.Dispose();
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 释放对象中的句柄
        /// </summary>
        /// <returns></returns>
        public bool ClearHandle()
        {
            bool result = false;
            try
            {
                if (SurfaceMatchingResult != null)
                    HSurfaceMatchingResult.ClearSurfaceMatchingResult(SurfaceMatchingResult);
                if (this._SampleModel != null)
                    this._SampleModel.ClearHandle();
                ////////////////////////
                if (this._SampleScene != null)
                    this._SampleScene.ClearHandle();
                ////////////////////////////
                if (this._KeyPoints != null)
                    this._KeyPoints.ClearHandle();
            }
            catch
            {
            }
            return result;
        }
    }
}

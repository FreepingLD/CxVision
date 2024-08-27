using HalconDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionBlock
{
    [Serializable]
    public class CreateLocalDeformableModelMethod
    {
        [NonSerialized]
        private HImage templateImage;
        private HXLDCont templateXld;
        [NonSerialized]
        private HRegion templateRegion;
        public HImage TemplateImage { get => templateImage; set => templateImage = value; }
        public HXLDCont TemplateXld { get => templateXld; set => templateXld = value; }

        [NonSerialized]
        private HXLDCont _ShapeModelContour;
        public HXLDCont ShapeModelContour
        {
            get
            {
                //this._ShapeModelContour = HShapeModelID?.GetShapeModelContours(1);
                return this._ShapeModelContour;
            }
            set
            {
                this._ShapeModelContour = value;
            }
        }

        public HRegion TemplateRegion { get => templateRegion; set => templateRegion = value; }
        public HDeformableModel create_local_deformable_model(HImage image, CreateLocalDeformableModelParam _LocalDeformableModelParam)
        {
            HDeformableModel localModelID = null;
            if (image == null || !image.IsInitialized())
            {
                throw new ArgumentNullException("image对象为空或没有初始化");
            }
            if (_LocalDeformableModelParam == null)
            {
                throw new ArgumentNullException("_LocalDeformableModelParam 对象为空或没有初始化");
            }
            if (_LocalDeformableModelParam.TemplateRegion.Count == 0)
            {
                throw new ArgumentNullException("模板区域数量 = 0，请先设置模板区域");
            }
            /////////////////////////////////////////
            if (_LocalDeformableModelParam.TemplateRegion.Count > 0)
            {
                HRegion hRegion = new HRegion();
                hRegion.GenEmptyRegion();
                foreach (var item in _LocalDeformableModelParam.TemplateRegion)
                {
                    hRegion = hRegion.ConcatObj(item.GetRegion());
                }
                if (image.CountChannels().D > 1)
                    this.templateImage = image.AccessChannel(1).ReduceDomain(hRegion.Union1());
                else
                    this.templateImage = image.ReduceDomain(hRegion.Union1());
                this.templateRegion?.Dispose();
                this.templateRegion = hRegion.Union1();
                hRegion?.Dispose();
            }
            else
            {
                if (image.CountChannels().D > 1)
                    this.templateImage = image.AccessChannel(1);
                else
                    this.templateImage = image;
            }
            //////////////////////////////
            HTuple MinContrast = new HTuple();
            int result = 0;
            if (int.TryParse(_LocalDeformableModelParam.MinContrast, out result))
                MinContrast = result;
            else
                MinContrast = _LocalDeformableModelParam.MinContrast;
            HTuple Contrast = new HTuple();
            result = 0;
            if (int.TryParse(_LocalDeformableModelParam.Contrast, out result))
                Contrast = result;
            else
                Contrast = _LocalDeformableModelParam.Contrast;
            HTuple numlevels = new HTuple();
            result = 0;
            if (int.TryParse(_LocalDeformableModelParam.NumLevels, out result))
                numlevels = result;
            else
                numlevels = _LocalDeformableModelParam.NumLevels;
            ////////////////////////////////////////////////////////
            localModelID = new HDeformableModel(this.templateImage,
                                                numlevels,
                                                _LocalDeformableModelParam.AngleStart * Math.PI / 180.0,
                                                (_LocalDeformableModelParam.AngleExtent - _LocalDeformableModelParam.AngleStart) * Math.PI / 180.0,
                                                new HTuple("auto"),
                                                _LocalDeformableModelParam.ScaleRMin,
                                                _LocalDeformableModelParam.ScaleRMax,
                                                new HTuple("auto"),
                                                _LocalDeformableModelParam.ScaleCMin,
                                                _LocalDeformableModelParam.ScaleCMax,
                                                new HTuple("auto"),
                                                new HTuple(_LocalDeformableModelParam.Optimization),
                                                new HTuple(_LocalDeformableModelParam.Metric),
                                                Contrast,
                                                MinContrast,
                                                new HTuple(),
                                                new HTuple()
                                                ); 
            return localModelID;
        }

        public HDeformableModel create_local_deformable_model_xld(HXLDCont xld, CreateLocalDeformableModelParam _LocalDeformableModelParam)
        {
            HDeformableModel localModelID = null;
            if (xld == null || !xld.IsInitialized())
            {
                throw new ArgumentNullException("xld 对象为空或没有初始化");
            }
            if (_LocalDeformableModelParam == null)
            {
                throw new ArgumentNullException("_LocalDeformableModelParam 对象为空或没有初始化");
            }
            this.templateXld = xld;
            //////////////////////////////
  
            HTuple MinContrast = new HTuple();
            int result = 0;
            if (int.TryParse(_LocalDeformableModelParam.MinContrast, out result))
                MinContrast = result;
            else
                MinContrast = _LocalDeformableModelParam.MinContrast;
            HTuple numlevels = new HTuple();
            result = 0;
            if (int.TryParse(_LocalDeformableModelParam.NumLevels, out result))
                numlevels = result;
            else
                numlevels = _LocalDeformableModelParam.NumLevels;
            /////////////////////////////////////////////////
            localModelID = new HDeformableModel(this.templateXld,
                                                numlevels,
                                                _LocalDeformableModelParam.AngleStart * Math.PI / 180.0,
                                                (_LocalDeformableModelParam.AngleExtent - _LocalDeformableModelParam.AngleStart) * Math.PI / 180.0,
                                                new HTuple("auto"),
                                                _LocalDeformableModelParam.ScaleRMin,
                                                _LocalDeformableModelParam.ScaleRMax,
                                                new HTuple("auto"),
                                                _LocalDeformableModelParam.ScaleCMin,
                                                _LocalDeformableModelParam.ScaleCMax,
                                                new HTuple("auto"),
                                                new HTuple(_LocalDeformableModelParam.Optimization),
                                                new HTuple(_LocalDeformableModelParam.Metric),
                                                MinContrast,
                                                new HTuple(),
                                                new HTuple()
                                                );
            return localModelID;
        }





    }


}

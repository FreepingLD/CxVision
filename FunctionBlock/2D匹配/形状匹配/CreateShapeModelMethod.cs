using HalconDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace FunctionBlock
{
    [Serializable]
    public class CreateShapeModelMethod
    {
        [NonSerialized]
        private HImage templateImage;
        [NonSerialized]
        private HRegion templateRegion;
        [NonSerialized]
        private HXLDCont templateCountXld;
        [XmlIgnore]
        public HImage TemplateImage { get => templateImage; set => templateImage = value; }

        /// <summary>
        /// 通过模板区域来设置模型的参考点
        /// </summary>
        [XmlIgnore]
        public HRegion TemplateRegion { get => templateRegion; set => templateRegion = value; }
        public HXLDCont TemplateCountXld { get => templateCountXld; set => templateCountXld = value; }

        public HShapeModel create_aniso_shape_model(HImage image, C_AnisoShapeModelParam c_anisoModelParam)
        {
            HShapeModel shapeModelID = null;
            if (image == null || !image.IsInitialized())
            {
                throw new ArgumentNullException("image对象为空或没有初始化");
            }
            if (c_anisoModelParam.TemplateRegion.Count == 0)
            {
                throw new ArgumentNullException("模板区域数量 == 0，请先设置模板区域");
            }
            ///////////////////////////////////////
            if (c_anisoModelParam.TemplateRegion.Count > 0)
            {
                HRegion hRegion = new HRegion();
                hRegion.GenEmptyRegion();
                foreach (var item in c_anisoModelParam.TemplateRegion)
                {
                    hRegion = hRegion.ConcatObj(item.RoiShape.GetRegion());
                }
                if (image.CountChannels().D > 1)
                    this.templateImage = image.AccessChannel(1).ReduceDomain(hRegion.Union1());
                else
                    this.templateImage = image.ReduceDomain(hRegion.Union1());
                ////////////
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
                /////保存模版区域
                int width, height;
                image.GetImageSize(out width, out height);
                this.templateRegion?.Dispose();
                this.templateRegion = new HRegion(0, 0, height * 1.0, width * 1.0);
            }
            //////////////////////////////
            HTuple MinContrast = new HTuple();
            int result = 0;
            if (int.TryParse(c_anisoModelParam.MinContrast, out result))
                MinContrast = result;
            else
                MinContrast = c_anisoModelParam.MinContrast;
            //////////////////////////
            shapeModelID = new HShapeModel(this.templateImage,
            new HTuple(c_anisoModelParam.NumLevels),
            c_anisoModelParam.AngleStart * Math.PI / 180,
            (c_anisoModelParam.AngleExtent - c_anisoModelParam.AngleStart) * Math.PI / 180,
            new HTuple(c_anisoModelParam.AngleStep),
            c_anisoModelParam.ScaleRMin,
            c_anisoModelParam.ScaleRMax,
            new HTuple(c_anisoModelParam.ScaleRStep),
            c_anisoModelParam.ScaleCMin,
            c_anisoModelParam.ScaleCMax,
            new HTuple(c_anisoModelParam.ScaleCStep),
            new HTuple(c_anisoModelParam.Optimization.Split(',', ';', ':')),
            c_anisoModelParam.Metric,
            new HTuple(c_anisoModelParam.Contrast,
            c_anisoModelParam.Contrast,
            c_anisoModelParam.MinLenght),
            MinContrast);
            return shapeModelID;
        }

        public HShapeModel[] create_aniso_shape_models(HImage image, C_AnisoShapeModelParam c_anisoModelParam)
        {
            HShapeModel[] shapeModelID = null;
            if (image == null || !image.IsInitialized())
            {
                throw new ArgumentNullException("image对象为空或没有初始化");
            }
            if (c_anisoModelParam.TemplateRegion.Count == 0)
            {
                throw new ArgumentNullException("模板区域数量 == 0，请先设置模板区域");
            }
            ///////////////////////////////////////
            // 先确定需要创建几个模型
            List<enModelSign> listCount = new List<enModelSign>();
            foreach (var item in c_anisoModelParam.TemplateRegion)
            {
                if (!listCount.Contains(item.ModelSign))
                {
                    listCount.Add(item.ModelSign);
                }
            }
            this.templateRegion?.Dispose();
            shapeModelID = new HShapeModel[listCount.Count];
            for (int i = 0; i < listCount.Count; i++)
            {
                HRegion hRegion = new HRegion();
                hRegion.GenEmptyRegion();
                foreach (var item in c_anisoModelParam.TemplateRegion)
                {
                    if (listCount[i] == item.ModelSign)
                        hRegion = hRegion.ConcatObj(item.RoiShape.GetRegion());
                }
                if (image.CountChannels().D > 1)
                    this.templateImage = image.AccessChannel(1).ReduceDomain(hRegion.Union1());
                else
                    this.templateImage = image.ReduceDomain(hRegion.Union1());
                ///////////////////////////////////////////////
                if (this.templateRegion != null && this.templateRegion.IsInitialized())
                    this.templateRegion = this.templateRegion.ConcatObj(hRegion.Union1());
                else
                    this.templateRegion = hRegion.Union1();
                hRegion?.Dispose();
                //////////////////////////////
                HTuple MinContrast = new HTuple();
                int result = 0;
                if (int.TryParse(c_anisoModelParam.MinContrast, out result))
                    MinContrast = result;
                else
                    MinContrast = c_anisoModelParam.MinContrast;
                //////////////////////////
                shapeModelID[i] = new HShapeModel(this.templateImage,
                new HTuple(c_anisoModelParam.NumLevels),
                c_anisoModelParam.AngleStart * Math.PI / 180,
                (c_anisoModelParam.AngleExtent - c_anisoModelParam.AngleStart) * Math.PI / 180,
                new HTuple(c_anisoModelParam.AngleStep),
                c_anisoModelParam.ScaleRMin,
                c_anisoModelParam.ScaleRMax,
                new HTuple(c_anisoModelParam.ScaleRStep),
                c_anisoModelParam.ScaleCMin,
                c_anisoModelParam.ScaleCMax,
                new HTuple(c_anisoModelParam.ScaleCStep),
                new HTuple(c_anisoModelParam.Optimization.Split(',', ';', ':')),
                c_anisoModelParam.Metric,
                new HTuple(c_anisoModelParam.Contrast,
                c_anisoModelParam.Contrast,
                c_anisoModelParam.MinLenght),
                MinContrast);
            }
            return shapeModelID;
        }
        public HShapeModel create_aniso_shape_model_xld(HImage image, HXLDCont XldCont, C_AnisoShapeModelParamXLD c_anisoModelParamXld)
        {
            HShapeModel shapeModelID = null;
            if (image == null || !image.IsInitialized())
            {
                throw new ArgumentNullException("image对象为空或没有初始化");
            }

            ///////////////////////////////////////////
            if (XldCont == null || !XldCont.IsInitialized())
            {
                throw new ArgumentNullException("XldCont对象为空或没有初始化");
            }
            ////////////////////////////
            shapeModelID = new HShapeModel(XldCont,
                new HTuple(c_anisoModelParamXld.NumLevels),
                c_anisoModelParamXld.AngleStart * Math.PI / 180,
                (c_anisoModelParamXld.AngleExtent - c_anisoModelParamXld.AngleStart) * Math.PI / 180,
                new HTuple(c_anisoModelParamXld.AngleStep),
                c_anisoModelParamXld.ScaleRMin,
                c_anisoModelParamXld.ScaleRMax,
                new HTuple(c_anisoModelParamXld.ScaleRStep),
                c_anisoModelParamXld.ScaleCMin,
                c_anisoModelParamXld.ScaleCMax,
                new HTuple(c_anisoModelParamXld.ScaleCStep),
                new HTuple(c_anisoModelParamXld.Optimization.Split(',', ';', ':')),
                c_anisoModelParamXld.Metric,
                c_anisoModelParamXld.MinContrast
                );
            return shapeModelID;
        }

        public HShapeModel[] create_aniso_shape_models_xld(HImage image, C_AnisoShapeModelParamXLD c_anisoModelParamXld)
        {
            HShapeModel[] shapeModelID = null;
            if (image == null || !image.IsInitialized())
            {
                throw new ArgumentNullException("image对象为空或没有初始化");
            }
            if (c_anisoModelParamXld == null)
            {
                throw new ArgumentNullException("c_anisoModelParamXld 对象为空或没有初始化");
            }
            if (c_anisoModelParamXld.TemplateRegion.Count == 0) // 必需设置模板区域
            {
                throw new ArgumentNullException("模板区域数量 = 0，请先设置模板区域");
            }
            ////////////////////////////
            // 先确定需要创建几个模型
            List<enModelSign> listCount = new List<enModelSign>();
            foreach (var item in c_anisoModelParamXld.TemplateRegion)
            {
                if (!listCount.Contains(item.ModelSign))
                {
                    listCount.Add(item.ModelSign);
                }
            }
            this.templateRegion?.Dispose();
            shapeModelID = new HShapeModel[listCount.Count];
            for (int i = 0; i < listCount.Count; i++)
            {
                HRegion hRegion = new HRegion();
                hRegion.GenEmptyRegion();
                HXLDCont XldCont = new HXLDCont();
                XldCont.GenEmptyObj();
                foreach (var item in c_anisoModelParamXld.TemplateRegion)
                {
                    if (listCount[i] == item.ModelSign)
                    {
                        hRegion = hRegion.ConcatObj(item.RoiShape.GetRegion());
                        XldCont = XldCont.ConcatObj(item.RoiShape.GetXLD());
                    }
                }
                if (this.templateRegion != null && this.templateRegion.IsInitialized())
                    this.templateRegion = this.templateRegion.ConcatObj(hRegion.Union1());
                else
                    this.templateRegion = hRegion.Union1();
                //////////////////////////////////////////////////
                shapeModelID[i] = new HShapeModel(
                XldCont,
                new HTuple(c_anisoModelParamXld.NumLevels),
                c_anisoModelParamXld.AngleStart * Math.PI / 180,
                (c_anisoModelParamXld.AngleExtent - c_anisoModelParamXld.AngleStart) * Math.PI / 180,
                new HTuple(c_anisoModelParamXld.AngleStep),
                c_anisoModelParamXld.ScaleRMin,
                c_anisoModelParamXld.ScaleRMax,
                new HTuple(c_anisoModelParamXld.ScaleRStep),
                c_anisoModelParamXld.ScaleCMin,
                c_anisoModelParamXld.ScaleCMax,
                new HTuple(c_anisoModelParamXld.ScaleCStep),
                new HTuple(c_anisoModelParamXld.Optimization.Split(',', ';', ':')),
                c_anisoModelParamXld.Metric,
                c_anisoModelParamXld.MinContrast);
                ///////////////////////////////////
                XldCont?.Dispose();
                hRegion?.Dispose();
            }
            return shapeModelID;
        }
        public HShapeModel create_scaled_shape_model(HImage image, C_ScaledShapeModelParam cssmParam)
        {
            HShapeModel shapeModelID = null;
            if (image == null || !image.IsInitialized())
            {
                throw new ArgumentNullException("image对象为空或没有初始化");
            }
            if (cssmParam.TemplateRegion.Count == 0)
            {
                throw new ArgumentNullException("模板区域数量 == 0，请先设置模板区域");
            }
            ////////////////////////////////////////////////
            if (cssmParam.TemplateRegion.Count > 0)
            {
                HRegion hRegion = new HRegion();
                hRegion.GenEmptyRegion();
                foreach (var item in cssmParam.TemplateRegion)
                {
                    hRegion = hRegion.ConcatObj(item.RoiShape.GetRegion());
                }
                if (image.CountChannels().D > 1)
                    this.templateImage = image.AccessChannel(1).ReduceDomain(hRegion.Union1());
                else
                    this.templateImage = image.ReduceDomain(hRegion.Union1());
                ////////////
                this.templateRegion?.Dispose();
                this.templateRegion = hRegion.Union1();
            }
            else
            {
                if (image.CountChannels().D > 1)
                    this.templateImage = image.AccessChannel(1);
                else
                    this.templateImage = image;
                /////保存模版区域
                int width, height;
                image.GetImageSize(out width, out height);
                this.templateRegion?.Dispose();
                this.templateRegion = new HRegion(0, 0, height * 1.0, width * 1.0);
            }
            ///////////////////////////////////////////////////////
            int result = 0;
            HTuple MinContrast = new HTuple();
            if (int.TryParse(cssmParam.MinContrast, out result))
                MinContrast = result;
            else
                MinContrast = cssmParam.MinContrast;
            ////////////////////////////////////////////
            shapeModelID = new HShapeModel(this.templateImage,
                new HTuple(cssmParam.NumLevels),
                cssmParam.AngleStart * Math.PI / 180,
                (cssmParam.AngleExtent - cssmParam.AngleStart) * Math.PI / 180,
            new HTuple(cssmParam.AngleStep),
            cssmParam.ScaleMin, cssmParam.ScaleMax,
            new HTuple(cssmParam.ScaleStep),
            new HTuple(cssmParam.Optimization.Split(',', ';', ':')), cssmParam.Metric,
            new HTuple(cssmParam.Contrast,
            cssmParam.Contrast, cssmParam.MinLenght), MinContrast);
            return shapeModelID;
        }

        public HShapeModel[] create_scaled_shape_models(HImage image, C_ScaledShapeModelParam cssmParam)
        {
            HShapeModel[] shapeModelID = null;
            if (image == null || !image.IsInitialized())
            {
                throw new ArgumentNullException("image对象为空或没有初始化");
            }
            if (cssmParam.TemplateRegion.Count == 0)
            {
                throw new ArgumentNullException("模板区域数量 == 0，请先设置模板区域");
            }
            ////////////////////////////////////////////////
            // 先确定需要创建几个模型
            List<enModelSign> listCount = new List<enModelSign>();
            foreach (var item in cssmParam.TemplateRegion)
            {
                if (!listCount.Contains(item.ModelSign))
                {
                    listCount.Add(item.ModelSign);
                }
            }
            this.templateRegion?.Dispose();
            shapeModelID = new HShapeModel[listCount.Count];
            for (int i = 0; i < listCount.Count; i++)
            {
                HRegion hRegion = new HRegion();
                hRegion.GenEmptyRegion();
                foreach (var item in cssmParam.TemplateRegion)
                {
                    if (listCount[i] == item.ModelSign)
                        hRegion = hRegion.ConcatObj(item.RoiShape.GetRegion());
                }
                if (image.CountChannels().D > 1)
                    this.templateImage = image.AccessChannel(1).ReduceDomain(hRegion.Union1());
                else
                    this.templateImage = image.ReduceDomain(hRegion.Union1());
                ///////////////////////////////////////////////
                if (this.templateRegion != null && this.templateRegion.IsInitialized())
                    this.templateRegion = this.templateRegion.ConcatObj(hRegion.Union1());
                else
                    this.templateRegion = hRegion.Union1();
                hRegion?.Dispose();
                ///////////////////////////////////////////////////////
                int result = 0;
                HTuple MinContrast = new HTuple();
                if (int.TryParse(cssmParam.MinContrast, out result))
                    MinContrast = result;
                else
                    MinContrast = cssmParam.MinContrast;
                ////////////////////////////////////////////
                shapeModelID[i] = new HShapeModel(this.templateImage,
                    new HTuple(cssmParam.NumLevels),
                    cssmParam.AngleStart * Math.PI / 180,
                    (cssmParam.AngleExtent - cssmParam.AngleStart) * Math.PI / 180,
                new HTuple(cssmParam.AngleStep),
                cssmParam.ScaleMin, cssmParam.ScaleMax,
                new HTuple(cssmParam.ScaleStep),
                new HTuple(cssmParam.Optimization.Split(',', ';', ':')), cssmParam.Metric,
                new HTuple(cssmParam.Contrast,
                cssmParam.Contrast, cssmParam.MinLenght), MinContrast);
            }
            return shapeModelID;
        }
        public HShapeModel create_scaled_shape_model_xld(HImage image, HXLDCont XldCont, C_ScaledShapeModelParamXLD cssmXldParam)
        {
            HShapeModel shapeModelID = null;
            if (image == null || !image.IsInitialized())
            {
                throw new ArgumentNullException("image对象为空或没有初始化");
            }
            ////////////////////////////////////////////////////////////////
            if (XldCont == null || !XldCont.IsInitialized())
            {
                throw new ArgumentNullException("XldCont对象为空或没有初始化");
            }
            ////////////////////////////////////////////////////////
            shapeModelID = new HShapeModel(XldCont,
                new HTuple(cssmXldParam.NumLevels),
                cssmXldParam.AngleStart * Math.PI / 180,
                (cssmXldParam.AngleExtent - cssmXldParam.AngleStart) * Math.PI / 180,
                new HTuple(cssmXldParam.AngleStep),
                cssmXldParam.ScaleMin,
                cssmXldParam.ScaleMax,
                new HTuple(cssmXldParam.ScaleStep),
                new HTuple(cssmXldParam.Optimization.Split(',', ';', ':')),
                cssmXldParam.Metric, cssmXldParam.MinContrast
                );
            return shapeModelID;
        }
        public HShapeModel[] create_scaled_shape_models_xld(HImage image, C_ScaledShapeModelParamXLD cssmXldParam)
        {
            HShapeModel[] shapeModelID = null;
            if (image == null || !image.IsInitialized())
            {
                throw new ArgumentNullException("image对象为空或没有初始化");
            }
            if (cssmXldParam == null)
            {
                throw new ArgumentNullException("cssmXldParam 对象为空或没有初始化");
            }
            if (cssmXldParam.TemplateRegion.Count == 0) // 必需设置模板区域
            {
                throw new ArgumentNullException("模板区域数量 = 0，请先设置模板区域");
            }
            ////////////////////////////////////////////////////////
            // 先确定需要创建几个模型
            List<enModelSign> listCount = new List<enModelSign>();
            foreach (var item in cssmXldParam.TemplateRegion)
            {
                if (!listCount.Contains(item.ModelSign))
                {
                    listCount.Add(item.ModelSign);
                }
            }
            this.templateRegion?.Dispose();
            shapeModelID = new HShapeModel[listCount.Count];
            for (int i = 0; i < listCount.Count; i++)
            {
                HRegion hRegion = new HRegion();
                hRegion.GenEmptyRegion();
                HXLDCont XldCont = new HXLDCont();
                XldCont.GenEmptyObj();
                foreach (var item in cssmXldParam.TemplateRegion)
                {
                    if (listCount[i] == item.ModelSign)
                    {
                        hRegion = hRegion.ConcatObj(item.RoiShape.GetRegion());
                        XldCont = XldCont.ConcatObj(item.RoiShape.GetXLD());
                    }
                }
                if (this.templateRegion != null && this.templateRegion.IsInitialized())
                    this.templateRegion = this.templateRegion.ConcatObj(hRegion.Union1());
                else
                    this.templateRegion = hRegion.Union1();
                /////////////////////////////////////////////////////
                shapeModelID[i] = new HShapeModel(
                    XldCont,
                    new HTuple(cssmXldParam.NumLevels),
                    cssmXldParam.AngleStart * Math.PI / 180,
                    (cssmXldParam.AngleExtent - cssmXldParam.AngleStart) * Math.PI / 180,
                    new HTuple(cssmXldParam.AngleStep),
                    cssmXldParam.ScaleMin,
                    cssmXldParam.ScaleMax,
                    new HTuple(cssmXldParam.ScaleStep),
                    new HTuple(cssmXldParam.Optimization.Split(',', ';', ':')),
                    cssmXldParam.Metric, cssmXldParam.MinContrast);
                ///////////////////////////////////
                XldCont?.Dispose();
                hRegion?.Dispose();
            }
            return shapeModelID;
        }
        public HShapeModel create_shape_model(HImage image, C_ShapeModelParam csmParam)
        {
            HShapeModel shapeModelID = null;
            if (image == null || !image.IsInitialized())
            {
                throw new ArgumentNullException("image对象为空或没有初始化");
            }
            if (csmParam.TemplateRegion.Count == 0) // 必需设置模板区域，防止使用整个图像来创建模板，导致软件卡死
            {
                throw new ArgumentNullException("模板区域数量 == 0，请先设置模板区域");
            }
            if (csmParam.TemplateRegion.Count > 0)
            {
                HRegion hRegion = new HRegion();
                hRegion.GenEmptyRegion();
                foreach (var item in csmParam.TemplateRegion)
                {
                    hRegion = hRegion.ConcatObj(item.RoiShape.GetRegion());
                }
                if (image.CountChannels().D > 1)
                    this.templateImage = image.AccessChannel(1).ReduceDomain(hRegion.Union1());
                else
                    this.templateImage = image.ReduceDomain(hRegion.Union1());
                ////////////
                this.templateRegion?.Dispose();
                this.templateRegion = hRegion.Union1();
                hRegion?.Dispose();
            }
            ////////////////////////////////////////////////////  参数以字符串的形式来传递，再解析字符串
            HTuple MinContrast = new HTuple();
            int result = 0;
            if (int.TryParse(csmParam.MinContrast, out result))
                MinContrast = result;
            else
                MinContrast = csmParam.MinContrast;
            ///////////////////////////////////
            shapeModelID = new HShapeModel(this.templateImage,
                                           new HTuple(csmParam.NumLevels),
                                           csmParam.AngleStart * Math.PI / 180,
                                           (csmParam.AngleExtent - csmParam.AngleStart) * Math.PI / 180,
                                           new HTuple(csmParam.AngleStep),
                                           new HTuple(csmParam.Optimization.Split(',', ';', ':')),
                                           csmParam.Metric,
                                           new HTuple(csmParam.Contrast, csmParam.Contrast, csmParam.MinLenght),
                                           MinContrast);
            /////////////////
            return shapeModelID;
        }

        public HShapeModel[] create_shape_models(HImage image, C_ShapeModelParam csmParam)
        {
            HShapeModel[] shapeModelID = null;
            if (image == null || !image.IsInitialized())
            {
                throw new ArgumentNullException("image对象为空或没有初始化");
            }
            if (csmParam.TemplateRegion.Count == 0) // 必需设置模板区域，防止使用整个图像来创建模板，导致软件卡死
            {
                throw new ArgumentNullException("模板区域数量 == 0，请先设置模板区域");
            }
            // 先确定需要创建几个模型
            List<enModelSign> listCount = new List<enModelSign>();
            foreach (var item in csmParam.TemplateRegion)
            {
                if (!listCount.Contains(item.ModelSign))
                {
                    listCount.Add(item.ModelSign);
                }
            }
            this.templateRegion?.Dispose();
            shapeModelID = new HShapeModel[listCount.Count];
            for (int i = 0; i < listCount.Count; i++)
            {
                HRegion hRegion = new HRegion();
                hRegion.GenEmptyRegion();
                foreach (var item in csmParam.TemplateRegion)
                {
                    if (listCount[i] == item.ModelSign)
                        hRegion = hRegion.ConcatObj(item.RoiShape.GetRegion());
                }
                if (image.CountChannels().D > 1)
                    this.templateImage = image.AccessChannel(1).ReduceDomain(hRegion.Union1());
                else
                    this.templateImage = image.ReduceDomain(hRegion.Union1());
                ///////////////////////////////////////////////
                if (this.templateRegion != null && this.templateRegion.IsInitialized())
                    this.templateRegion = this.templateRegion.ConcatObj(hRegion.Union1());
                else
                    this.templateRegion = hRegion.Union1();
                hRegion?.Dispose();
                ////////////////////////////////////////////////////  参数以字符串的形式来传递，再解析字符串
                HTuple MinContrast = new HTuple();
                int result = 0;
                if (int.TryParse(csmParam.MinContrast, out result))
                    MinContrast = result;
                else
                    MinContrast = csmParam.MinContrast;
                ///////////////////////////////////
                shapeModelID[i] = new HShapeModel(this.templateImage,
                                                  new HTuple(csmParam.NumLevels),
                                                  csmParam.AngleStart * Math.PI / 180,
                                                  (csmParam.AngleExtent - csmParam.AngleStart) * Math.PI / 180,
                                                  new HTuple(csmParam.AngleStep),
                                                  new HTuple(csmParam.Optimization.Split(',', ';', ':')),
                                                  csmParam.Metric,
                                                  new HTuple(csmParam.Contrast, csmParam.Contrast, csmParam.MinLenght),
                                                  MinContrast);
            }
            /////////////////
            return shapeModelID;
        }

        public HShapeModel create_shape_model_xld(HImage image, HXLDCont XldCont, C_ShapeModelParamXLD csmXldParam)
        {
            HShapeModel shapeModelID;
            if (image == null || !image.IsInitialized())
            {
                throw new ArgumentNullException("image对象为空或没有初始化");
            }
            ///////////////////////////////////////////
            if (XldCont == null || !XldCont.IsInitialized())
            {
                throw new ArgumentNullException("XldCont 对象为空或没有初始化");
            }
            ///////////////////////////////////////////
            shapeModelID = new HShapeModel(XldCont,
                new HTuple(csmXldParam.NumLevels),
                csmXldParam.AngleStart * Math.PI / 180,
                (csmXldParam.AngleExtent - csmXldParam.AngleStart) * Math.PI / 180,
                new HTuple(csmXldParam.AngleStep),
                new HTuple(csmXldParam.Optimization.Split(',', ';', ':')),
                csmXldParam.Metric,
                csmXldParam.MinContrast);
            ///////////////////////////////
            return shapeModelID;
        }

        public HShapeModel[] create_shape_models_xld(HImage image, C_ShapeModelParamXLD csmXldParam)
        {
            HShapeModel[] shapeModelID;
            if (image == null || !image.IsInitialized())
            {
                throw new ArgumentNullException("image对象为空或没有初始化");
            }
            if (csmXldParam == null)
            {
                throw new ArgumentNullException("csmXldParam 对象为空或没有初始化");
            }
            if (csmXldParam.TemplateRegion.Count == 0) // 必需设置模板区域
            {
                throw new ArgumentNullException("模板区域数量 = 0，请先设置模板区域");
            }
            ///////////////////////////////////////////
            // 先确定需要创建几个模型
            List<enModelSign> listCount = new List<enModelSign>();
            foreach (var item in csmXldParam.TemplateRegion)
            {
                if (!listCount.Contains(item.ModelSign))
                {
                    listCount.Add(item.ModelSign);
                }
            }
            this.templateRegion?.Dispose();
            this.templateCountXld?.Dispose();
            shapeModelID = new HShapeModel[listCount.Count];
            for (int i = 0; i < listCount.Count; i++)
            {
                HRegion hRegion = new HRegion();
                hRegion.GenEmptyRegion();
                HXLDCont XldCont = new HXLDCont();
                XldCont.GenEmptyObj();
                foreach (var item in csmXldParam.TemplateRegion)
                {
                    if (listCount[i] == item.ModelSign)
                    {
                        hRegion = hRegion.ConcatObj(item.RoiShape.GetRegion());
                        XldCont = XldCont.ConcatObj(item.RoiShape.GetXLD());
                    }
                }
                if (this.templateRegion != null && this.templateRegion.IsInitialized())
                    this.templateRegion = this.templateRegion.ConcatObj(hRegion.Union1());
                else
                    this.templateRegion = hRegion.Union1();
                //////////////////////////////////////////////////////////
                if (this.templateCountXld != null && this.templateCountXld.IsInitialized())
                    this.templateCountXld = this.templateCountXld.ConcatObj(hRegion.Union1());
                else
                    this.templateCountXld = XldCont.CopyObj(1,-1);
                /////////////////////////////////////////////////////
                shapeModelID[i] = new HShapeModel(
                    XldCont,
                    new HTuple(csmXldParam.NumLevels),
                    csmXldParam.AngleStart * Math.PI / 180,
                    (csmXldParam.AngleExtent - csmXldParam.AngleStart) * Math.PI / 180,
                    new HTuple(csmXldParam.AngleStep),
                    new HTuple(csmXldParam.Optimization.Split(',', ';', ':')),
                    csmXldParam.Metric,
                    csmXldParam.MinContrast);
                /////////////////////////////////
                XldCont?.Dispose();
                hRegion?.Dispose();
            }
            ///////////////////////////////
            return shapeModelID;
        }

    }


}

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
    public class CreateNccModelMethod
    {
        [NonSerialized]
        private HImage templateImage;

        [NonSerialized]
        private HRegion templateRegion;
        [XmlIgnore]
        public HImage TemplateImage { get => templateImage; set => templateImage = value; }

        /// <summary>
        /// 通过模板区域来设置模型的参考点
        /// </summary>
        [XmlIgnore]
        public HRegion TemplateRegion { get => templateRegion; set => templateRegion = value; }

        public HNCCModel create_ncc_model(HImage image, CreateNccModelParam _NccModelParam)
        {
            HNCCModel nccModelID = null;
            if (image == null || !image.IsInitialized())
            {
                throw new ArgumentNullException("image对象为空或没有初始化");
            }
            if (_NccModelParam.TemplateRegion.Count == 0)
            {
                throw new ArgumentNullException("模板区域数量 = 0，请先设置模板区域");
            }
            /////////////////////////////////////////
            this.templateRegion?.Dispose();
            if (_NccModelParam.TemplateRegion.Count > 0)
            {
                HRegion hRegion = new HRegion();
                hRegion.GenEmptyRegion();
                foreach (var item in _NccModelParam.TemplateRegion)
                {
                    hRegion = hRegion.ConcatObj(item.RoiShape.GetRegion());
                }
                if (this.templateRegion != null && this.templateRegion.IsInitialized())
                    this.templateRegion = this.templateRegion.ConcatObj(hRegion.Union1()); // 将各个模型区域添加进来
                else
                    this.templateRegion = hRegion.Union1();
                if (image.CountChannels().D > 1)
                    this.templateImage = image.AccessChannel(1).ReduceDomain(hRegion.Union1());
                else
                    this.templateImage = image.ReduceDomain(hRegion.Union1());
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
            nccModelID = new HNCCModel(this.templateImage,
                                       new HTuple(_NccModelParam.NumLevels),
                                      _NccModelParam.AngleStart * Math.PI / 180.0,
                                      (_NccModelParam.AngleExtent - _NccModelParam.AngleStart) * Math.PI / 180.0,
                                      new HTuple(_NccModelParam.AngleStep),
                                      _NccModelParam.Metric);
            return nccModelID;
        }

        public HNCCModel[] create_ncc_models(HImage image, CreateNccModelParam _NccModelParam)
        {
            HNCCModel[] nccModelID = null;
            if (image == null || !image.IsInitialized())
            {
                throw new ArgumentNullException("image对象为空或没有初始化");
            }
            if (_NccModelParam.TemplateRegion.Count == 0)
            {
                throw new ArgumentNullException("模板区域数量 = 0，请先设置模板区域");
            }
            // 先确定需要创建几个模型
            List<enModelSign> listCount = new List<enModelSign>();
            foreach (var item in _NccModelParam.TemplateRegion)
            {
                if (!listCount.Contains(item.ModelSign))
                {
                    listCount.Add(item.ModelSign);
                }
            }
            this.templateRegion?.Dispose();
            nccModelID = new HNCCModel[listCount.Count];
            for (int i = 0; i < listCount.Count; i++)
            {
                HRegion hRegion = new HRegion();
                hRegion.GenEmptyRegion();
                foreach (var item in _NccModelParam.TemplateRegion)
                {
                    if (listCount[i] == item.ModelSign)
                        hRegion = hRegion.ConcatObj(item.RoiShape.GetRegion());
                }
                if (this.templateRegion != null && this.templateRegion.IsInitialized())
                    this.templateRegion = this.templateRegion.ConcatObj(hRegion.Union1()); // 将各个模型区域添加进来
                else
                    this.templateRegion = hRegion.Union1();
                if (image.CountChannels().D > 1)
                    this.templateImage = image.AccessChannel(1).ReduceDomain(hRegion.Union1());
                else
                    this.templateImage = image.ReduceDomain(hRegion.Union1());
                hRegion?.Dispose();
                ////////////////////////////////////////////////////////////////////////////////
                nccModelID[i] = new HNCCModel(this.templateImage,
                                              new HTuple(_NccModelParam.NumLevels),
                                              _NccModelParam.AngleStart * Math.PI / 180.0,
                                              (_NccModelParam.AngleExtent - _NccModelParam.AngleStart) * Math.PI / 180.0,
                                              new HTuple(_NccModelParam.AngleStep),
                                              _NccModelParam.Metric);
            }
            return nccModelID;
        }


    }


}

using Common;
using HalconDotNet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionBlock
{
    [Serializable]
    public class OcrMethod
    {
        [NonSerialized]
        private HDlModelOcr hDlModelOcr;
        public OcrMethod()
        {

        }

        public bool CreateDeepOcrMode(DeepOcrCreateParam CreateParam)
        {
            if (this.hDlModelOcr != null && this.hDlModelOcr.IsInitialized()) this.hDlModelOcr.ClearHandle();
            this.hDlModelOcr = this.CreateOcrMode(CreateParam);
            if (this.hDlModelOcr.IsInitialized()) return true;
            else return false;
        }

        public bool ApplyDeepOcr(HImage hImage, userPixCoordSystem pixCoordSystem, DeepOcrRecognitionParam RecognizeParam, out string[] word)
        {
            bool result = false;
            word = new string[RecognizeParam.ReduceParam.Count];
            if (hImage == null) return false;
            if (!hImage.IsInitialized()) return false;
            if (pixCoordSystem == null) pixCoordSystem = new userPixCoordSystem();
            int index = 0;
            foreach (var item in RecognizeParam.ReduceParam)
            {
                int width, height;
                HRegion hRegion = item.RoiShape.AffinePixROI(pixCoordSystem.GetHomMat2D()).GetRegion();
                HImage hImageReduce = hImage.ReduceDomain(hRegion);
                HImage hImageCrop = hImageReduce.CropDomain();
                hImageCrop.GetImageSize(out width, out height);
                hDlModelOcr.SetDeepOcrParam("recognition_image_width", new HTuple(RecognizeParam.RecognitionChartWidth));
                string value = "";
                if (RecognizeParam.InvertImage)
                {
                    if (hImageCrop.CountChannels() > 1)
                        value = this.ApplyDeepOcr(this.hDlModelOcr, hImageCrop.Rgb1ToGray().InvertImage());
                    else
                        value = this.ApplyDeepOcr(this.hDlModelOcr, hImageCrop.InvertImage());
                }
                else
                {
                    if (hImageCrop.CountChannels() > 1)
                        value = this.ApplyDeepOcr(this.hDlModelOcr, hImageCrop.Rgb1ToGray());
                    else
                        value = this.ApplyDeepOcr(this.hDlModelOcr, hImageCrop);
                }
                word[index] = value;
                index++;
            }
            result = true;
            return result;
        }
        private  HDlModelOcr CreateOcrMode(DeepOcrCreateParam param)
        {
            HDlModelOcr hDlModelOcr = new HDlModelOcr();
            try
            {
                hDlModelOcr.CreateDeepOcr("mode", param.DeepOcrMode.ToString());
                HDlDevice[] hDlDevice = HDlDevice.QueryAvailableDlDevices(new HTuple("runtime", "runtime"), new HTuple("gpu", "cpu"));
                hDlModelOcr.SetDeepOcrParam("recognition_image_width", new HTuple(param.RecognitionChartWidth));
                //hDlModelOcr.SetDeepOcrParam("recognition_image_height", new HTuple(param.RecognitionChartHeight));
                for (int i = 0; i < hDlDevice.Length; i++)
                {
                    hDlModelOcr.SetDeepOcrParam("device", hDlDevice[i]);
                }
            }
            catch
            {
                hDlModelOcr?.ClearHandle();
                throw new Exception();
            }
            return hDlModelOcr;
        }


        private string ApplyDeepOcr(HDlModelOcr hDlModelOcr, HImage hImage)
        {
            string RecognizedWord = "";
            HDict[] hDict = hDlModelOcr.ApplyDeepOcr(hImage, "recognition");
            foreach (var item in hDict)
            {
                HTuple hTuple = item.GetDictParam("key_exists", "word");
                if (hTuple.I > 0)
                {
                    HTuple tuple = item.GetDictTuple("word");
                    RecognizedWord = tuple.S;
                }
            }
            return RecognizedWord;
        }



        public void ClearDlModelOcr()
        {
            if (this.hDlModelOcr != null && this.hDlModelOcr.IsInitialized())
                this.hDlModelOcr.ClearHandle();
            ///////////////////////////////
        }


        public void SaveDlModelOcr(string filePath)
        {
            if (this.hDlModelOcr != null && this.hDlModelOcr.IsInitialized())
                this.hDlModelOcr.WriteDeepOcr(filePath);
            //////////////////////////////////////////////////
        }

        public void ReadDlModelOcr(string filePath)
        {
            if (this.hDlModelOcr == null)
                this.hDlModelOcr = new HDlModelOcr();
            else
            {
                if (this.hDlModelOcr.IsInitialized())
                    this.hDlModelOcr.ClearHandle();
                this.hDlModelOcr = new HDlModelOcr();
            }
            if (File.Exists(filePath))
                this.hDlModelOcr.ReadDeepOcr(filePath);
        }


    }

}

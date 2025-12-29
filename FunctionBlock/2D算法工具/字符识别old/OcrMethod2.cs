using Common;
using HalconDotNet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionBlock
{
    [Serializable]
    public class OcrMethod2
    {
        public static HDlModelOcr CreateOcrMode(DeepOcrCreateParam param)
        {
            HDlModelOcr hDlModelOcr = new HDlModelOcr();
            try
            {
                hDlModelOcr.CreateDeepOcr("mode", param.DeepOcrMode.ToString());
                HDlDevice[] hDlDevice = HDlDevice.QueryAvailableDlDevices(new HTuple("runtime", "runtime"), new HTuple("gpu", "cpu"));
                hDlModelOcr.SetDeepOcrParam("recognition_image_width", new HTuple(param.RecognitionChartWidth));
                hDlModelOcr.SetDeepOcrParam("recognition_image_height", new HTuple(param.RecognitionChartHeight));
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


        public static string ApplyDeepOcr(HDlModelOcr hDlModelOcr, HImage hImage)
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



    }
}

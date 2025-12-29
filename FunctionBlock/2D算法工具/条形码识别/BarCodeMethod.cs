using HalconDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FunctionBlock
{
    [Serializable]
    public class BarCodeMethod
    {
        private static object lockState = new object();
        private static BarCodeMethod _Instance = null;
        private BarCodeMethod()
        {

        }

        public static BarCodeMethod Instance
        {
            get
            {
                if (_Instance == null)
                {
                    lock (lockState)
                    {
                        if (_Instance == null)
                            _Instance = new BarCodeMethod();
                    }
                }
                return _Instance;
            }
        }


        public bool find_bar_code_2d(HBarCode hBarCode2D, HImage hImage, FindBarCodeParam param, out BarCodeResult codeResult)
        {
            if (hImage == null)
                throw new ArgumentNullException(" hImage ");
            if (hBarCode2D == null || !hBarCode2D.IsInitialized())
                throw new ArgumentNullException(" hBarCode2D 为空或未被初始化");
            codeResult = new BarCodeResult();
            //////////////////////////////////////////////////////////////////
            HTuple decodedDataStrings;
            HRegion hXLDCont;
            HTuple GenParamName = new string[]
            {
                nameof(param.stop_after_result_num),
                nameof(param.timeout),
                nameof(param.persistence)
            };
            HTuple GenParamValue = new HTuple
            (
                param.stop_after_result_num,
                param.timeout,
                param.persistence
            );
            if (GenParamName.Length != GenParamValue.Length)
                throw new ArgumentException("参数名称与参数值不相等");
            //////////////////////////////////////////////////////////////////////////////
            hBarCode2D.SetBarCodeParam(GenParamName, GenParamValue);
            if (param.InvertImage)
                hXLDCont = hBarCode2D.FindBarCode(hImage.InvertImage(), param.BarCodeType, out decodedDataStrings);
            else
                hXLDCont = hBarCode2D.FindBarCode(hImage, param.BarCodeType, out decodedDataStrings);
            codeResult.Content = decodedDataStrings.S;
            codeResult.SymbolXLDs = hXLDCont.GenContourRegionXld("border");
            ////////////////////////////////////////////////
            return true;
        }

        public bool train_bar_code_2d(HBarCode hBarCode2D, HImage hImage, FindBarCodeParam param, out BarCodeResult codeResult)
        {
            if (hImage == null)
                throw new ArgumentNullException(" hImage ");
            if (hBarCode2D == null || !hBarCode2D.IsInitialized())
                throw new ArgumentNullException(" hBarCode2D 为空或未被初始化");
            codeResult = new BarCodeResult();
            //////////////////////////////////////////////////////////////////
            HTuple decodedDataStrings;
            HXLDCont hXLDCont;
            //////////////////////////////////////////////////////////////////////////////
            hBarCode2D.SetBarCodeParam("train", "all");
            hXLDCont = hBarCode2D.FindBarCode(hImage, param.BarCodeType, out decodedDataStrings);
            codeResult.Content = decodedDataStrings.S;
            codeResult.SymbolXLDs = hXLDCont;
            ////////////////////////////////////////////////
            return true;
        }




    }
}

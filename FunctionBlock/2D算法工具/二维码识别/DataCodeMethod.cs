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
    public class DataCodeMethod
    {
        private static object lockState = new object();
        private static DataCodeMethod _Instance = null;
        private DataCodeMethod()
        {

        }

        public static DataCodeMethod Instance
        {
            get
            {
                if (_Instance == null)
                {
                    lock (lockState)
                    {
                        if (_Instance == null)
                            _Instance = new DataCodeMethod();
                    }
                }
                return _Instance;
            }
        }

        public HDataCode2D create_data_code_2d_model(enSymbolType SymbolType)
        {
            HDataCode2D hDataCode2D = new HDataCode2D();
            //////////////////////////////////////////////////////////////////////////////
            switch (SymbolType)
            {
                default:
                case enSymbolType.GS1_DataMatrix:
                    hDataCode2D = new HDataCode2D("GS1 DataMatrix", new HTuple(), new HTuple());
                    break;
                case enSymbolType.Data_Matrix_ECC_200:
                    hDataCode2D = new HDataCode2D("Data Matrix ECC 200", new HTuple(), new HTuple());
                    break;
                case enSymbolType.GS1_Aztec_Code:
                    hDataCode2D = new HDataCode2D("GS1 Aztec Code", new HTuple(), new HTuple());
                    break;
                case enSymbolType.Aztec_Code:
                    hDataCode2D = new HDataCode2D("Aztec Code", new HTuple(), new HTuple());
                    break;
                case enSymbolType.Micro_QR_Code:
                    hDataCode2D = new HDataCode2D("Micro QR Code", new HTuple(), new HTuple());
                    break;
                case enSymbolType.PDF417:
                    hDataCode2D = new HDataCode2D("PDF417", new HTuple(), new HTuple());
                    break;
                case enSymbolType.GS1_QR_Code:
                    hDataCode2D = new HDataCode2D("GS1 QR Code", new HTuple(), new HTuple());
                    break;
                case enSymbolType.QR_Code:
                    hDataCode2D = new HDataCode2D("QR Code", new HTuple(), new HTuple());
                    break;
                case enSymbolType.DotCode:
                    hDataCode2D = new HDataCode2D("DotCode", new HTuple(), new HTuple());
                    break;
                case enSymbolType.GS1_DotCode:
                    hDataCode2D = new HDataCode2D("GS1 DotCode", new HTuple(), new HTuple());
                    break;
            }
            ////////////////////////////////////////////////
            return hDataCode2D;
        }

        public bool find_data_code_2d(HDataCode2D hDataCode2D, HImage hImage, FindDataCodeParam param, out DataCodeResult codeResult)
        {
            if (hImage == null)
                throw new ArgumentNullException(" hImage ");
            if (hDataCode2D == null || !hDataCode2D.IsInitialized())
                throw new ArgumentNullException(" hDataCode2D 为空或未被初始化");
            codeResult = new DataCodeResult();
            //////////////////////////////////////////////////////////////////
            HTuple resultHandles, decodedDataStrings;
            HXLDCont hXLDCont;
            HTuple GenParamName = new string[]
            {
                nameof(param.polarity),
                nameof(param.mirrored),
                nameof(param.persistence),
                nameof(param.discard_undecoded_candidates),
                nameof(param.strict_model),
                nameof(param.string_encoding),
                nameof(param.timeout)
            };
            HTuple GenParamValue = new HTuple
            (
                param.polarity,
                param.mirrored,
                param.persistence,
                param.discard_undecoded_candidates,
                param.strict_model,
                param.string_encoding,
                param.timeout
            );
            if (GenParamName.Length != GenParamValue.Length)
                throw new ArgumentException("参数名称与参数值不相等");
            //////////////////////////////////////////////////////////////////////////////
            hDataCode2D.SetDataCode2dParam(GenParamName, GenParamValue);
            hXLDCont = hDataCode2D.FindDataCode2d(hImage, new HTuple(nameof(param.stop_after_result_num)), new HTuple(param.stop_after_result_num), out resultHandles, out decodedDataStrings);
            codeResult.Content = decodedDataStrings.S;
            codeResult.SymbolXLDs = hXLDCont;
            ////////////////////////////////////////////////
            return true;
        }

        public bool train_data_code_2d(HDataCode2D hDataCode2D, HImage hImage, FindDataCodeParam param, out DataCodeResult codeResult)
        {
            if (hImage == null)
                throw new ArgumentNullException(" hImage ");
            if (hDataCode2D == null || !hDataCode2D.IsInitialized())
                throw new ArgumentNullException(" hDataCode2D 为空或未被初始化");
            codeResult = new DataCodeResult();
            //////////////////////////////////////////////////////////////////
            HTuple resultHandles, decodedDataStrings;
            HXLDCont hXLDCont;
            HTuple GenParamName = new string[]
            {
                nameof(param.polarity),
                nameof(param.mirrored),
                nameof(param.persistence),
                nameof(param.discard_undecoded_candidates),
                nameof(param.strict_model),
                nameof(param.string_encoding),
                nameof(param.timeout)
            };
            HTuple GenParamValue = new HTuple
            (
                param.polarity,
                param.mirrored,
                param.persistence,
                param.discard_undecoded_candidates,
                param.strict_model,
                param.string_encoding,
                param.timeout
            );
            if (GenParamName.Length != GenParamValue.Length)
                throw new ArgumentException("参数名称与参数值不相等");
            //////////////////////////////////////////////////////////////////////////////
            hDataCode2D.SetDataCode2dParam(GenParamName, GenParamValue);
            hXLDCont = hDataCode2D.FindDataCode2d(hImage, "train", "all", out resultHandles, out decodedDataStrings);
            codeResult.Content = decodedDataStrings.S;
            codeResult.SymbolXLDs = hXLDCont;
            ////////////////////////////////////////////////
            return true;
        }




    }
}

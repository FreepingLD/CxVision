using HalconDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionBlock
{
    [Serializable]
    public class CreateDataCodeParam
    {
        public string SymbolType { get; set; }

        public CreateDataCodeParam()
        {
            this.SymbolType = "Data Matrix ECC 200";

        }
    }

    [Serializable]
    public class FindDataCodeParam
    {
        public string default_parameters { get; set; } // 这个参数为只读参数
        public string polarity { get; set; }
        public string mirrored { get; set; }

        // find_data_code_2d
        public int stop_after_result_num { get; set; } // 解码的数量
        public int timeout { get; set; }
        public int persistence { get; set; }
        public string discard_undecoded_candidates { get; set; }
        public string strict_model { get; set; }
        public string string_encoding { get; set; }

        // Aztec 不支持这个参数
        public string strict_quiet_zone { get; set; }

        public FindDataCodeParam()
        {
            this.default_parameters = "standard_recognition";
            this.polarity = "dark_on_light";
            this.mirrored = "any";
            this.persistence = 0;
            this.discard_undecoded_candidates = "no";
            this.strict_model = "yes";
            this.string_encoding = "latin1";
            this.timeout = 5000;
            this.stop_after_result_num = 1;
        }




    }
    [Serializable]
    public class DataCodeParam
    {
        public virtual string default_parameters { get; set; }
        public string trained { get; set; }
        public string polarity { get; set; }
        public string mirrored { get; set; }

        // find_data_code_2d
        public int stop_after_result_num { get; set; } // 解码的数量
        public int persistence { get; set; }
        public string discard_undecoded_candidates { get; set; }
        public string strict_model { get; set; }
        public string string_encoding { get; set; }
        public int timeout { get; set; }

        public DataCodeParam()
        {
            this.default_parameters = "standard_recognition";
            this.trained = "all";
            this.polarity = "dark_on_light";
            this.mirrored = "any";
            this.persistence = 0;
            this.discard_undecoded_candidates = "no";
            this.strict_model = "yes";
            this.string_encoding = "latin1";
            this.timeout = 5000;
            this.stop_after_result_num = 1;
        }
    }
    [Serializable]
    public class ECC200DataCodeParam : DataCodeParam
    {
        private string _default_parameters;
        public override string default_parameters
        {
            get
            {
                return _default_parameters;
            }
            set
            {
                _default_parameters = value;
            }
        }
        public int symbol_cols_min { get; set; }
        public int symbol_cols_max { get; set; }
        public int symbol_rows_min { get; set; }
        public int symbol_rows_max { get; set; }
        public int module_size_min { get; set; }
        public int module_size_max { get; set; }
        public string module_gap_min { get; set; }
        public string module_gap_max { get; set; }
        public string small_modules_robustness { get; set; }
        public string finder_pattern_tolerance { get; set; }
        public double quality_isoiec15415_aperture_size { get; set; }
        public string candidate_selection { get; set; }
        public string symbol_shape { get; set; } // 这个参数一变，其他参数也会跟着变
        public double slant_max { get; set; }
        public string contrast_tolerance { get; set; }
        public string module_grid { get; set; }
        public string decoding_scheme { get; set; }
        // find data code
        public string strict_quiet_zone { get; set; }




        public ECC200DataCodeParam()
        {
            this.symbol_shape = "any";
            this.symbol_cols_min = 10;
            this.symbol_cols_max = 144;
            this.symbol_rows_min = 8;
            this.symbol_rows_max = 144;
            this.module_size_min = 6;
            this.module_size_max = 20;
            this.module_gap_min = "no";
            this.module_gap_max = "no";
            this.small_modules_robustness = "low";
            this.slant_max = 0.1745;
            this.finder_pattern_tolerance = "low";
            this.contrast_tolerance = "low";
            this.module_grid = "fixed";
            this.strict_quiet_zone = "no";
            this.quality_isoiec15415_aperture_size = 0.8;
            this.candidate_selection = "default";
            this.decoding_scheme = "default";
        }

    }
    [Serializable]
    public class QRDataCodeParam : DataCodeParam
    {
        private string _default_parameters;
        public override string default_parameters
        {
            get
            {
                return _default_parameters;
            }
            set
            {
                _default_parameters = value;
            }
        }
        public string model_type { get; set; }
        public int version_min { get; set; }
        public int version_max { get; set; }
        public int symbol_size_min { get; set; }
        public int symbol_size_max { get; set; } // 这个参数一变，其他参数也会跟着变
        public int contrast_min { get; set; }
        public int module_size_min { get; set; }
        public int module_size_max { get; set; }
        public string module_gap_min { get; set; }
        public string module_gap_max { get; set; }
        public string small_modules_robustness { get; set; }

        public int position_pattern_min { get; set; }
        public double quality_isoiec15415_aperture_size { get; set; }

        // find data code
        public string strict_quiet_zone { get; set; }
        public QRDataCodeParam()
        {
            this.model_type = "any";
            this.version_min = 1;
            this.version_max = 40;
            this.symbol_size_min = 21;
            this.symbol_size_max = 177;
            this.contrast_min = 30;
            this.module_size_min = 6;
            this.module_size_max = 20;
            this.module_gap_min = "no";
            this.module_gap_max = "small";
            this.small_modules_robustness = "low";
            this.position_pattern_min = 3;
            this.strict_quiet_zone = "no";
            this.quality_isoiec15415_aperture_size = 0.8;
        }
    }
    [Serializable]
    public class MicroQRDataCodeParam : DataCodeParam
    {
        private string _default_parameters;
        public override string default_parameters
        {
            get
            {
                return _default_parameters;
            }
            set
            {
                _default_parameters = value;
            }
        }
        public int version_min { get; set; }
        public int version_max { get; set; }
        public int symbol_size_min { get; set; }
        public int symbol_size_max { get; set; } // 这个参数一变，其他参数也会跟着变
        public int contrast_min { get; set; }
        public int module_size_min { get; set; }
        public int module_size_max { get; set; }
        public string module_gap_min { get; set; }
        public string module_gap_max { get; set; }
        public string small_modules_robustness { get; set; }
        public double quality_isoiec15415_aperture_size { get; set; }
        // find data code
        public string strict_quiet_zone { get; set; }
        public MicroQRDataCodeParam()
        {
            this.version_min = 1;
            this.version_max = 4;
            this.symbol_size_min = 11;
            this.symbol_size_max = 17;
            this.contrast_min = 30;
            this.module_size_min = 6;
            this.module_size_max = 20;
            this.module_gap_min = "no";
            this.module_gap_max = "small";
            this.small_modules_robustness = "low";
            this.strict_quiet_zone = "no";
            this.quality_isoiec15415_aperture_size = 0.8;
        }
    }
    [Serializable]
    public class PDF417DataCodeParam : DataCodeParam
    {
        private string _default_parameters;
        public override string default_parameters
        {
            get
            {
                return _default_parameters;
            }
            set
            {
                _default_parameters = value;
            }
        }
        public int symbol_cols_min { get; set; }
        public int symbol_cols_max { get; set; }
        public int symbol_rows_min { get; set; }
        public int symbol_rows_max { get; set; } // 这个参数一变，其他参数也会跟着变
        public int contrast_min { get; set; }

        public int module_width_min { get; set; }
        public int module_width_max { get; set; }
        public int module_aspect_min { get; set; }
        public int module_aspect_max { get; set; }
        public double quality_isoiec15415_aperture_size { get; set; }
        // find data code
        public string strict_quiet_zone { get; set; }
        public PDF417DataCodeParam()
        {
            this.symbol_cols_min = 1;
            this.symbol_cols_max = 20;
            this.symbol_rows_min = 5;
            this.symbol_rows_max = 45;
            this.contrast_min = 30;
            this.module_width_min = 3;
            this.module_width_max = 15;
            this.module_aspect_min = 1;
            this.module_aspect_max = 4;
            this.strict_quiet_zone = "no";
            this.quality_isoiec15415_aperture_size = 0.8;
        }
    }

    [Serializable]
    public class AztecDataCodeParam : DataCodeParam
    {
        private string _default_parameters;
        public override string default_parameters
        {
            get
            {
                return _default_parameters;
            }
            set
            {
                _default_parameters = value;
            }
        }
        public string format { get; set; }
        public int symbol_size_min { get; set; }
        public int symbol_size_max { get; set; }
        public int contrast_min { get; set; }
        public int module_size_min { get; set; }
        public int module_size_max { get; set; }
        public string module_gap_min { get; set; }
        public string module_gap_max { get; set; }
        public string small_modules_robustness { get; set; }
        public string finder_pattern_tolerance { get; set; }
        public int additional_levels { get; set; }
        public double quality_isoiec15415_aperture_size { get; set; }

        public AztecDataCodeParam()
        {
            this.format = "compact full_range";
            this.symbol_size_min = 11;
            this.symbol_size_max = 151;
            this.contrast_min = 30;
            this.module_size_min = 6;
            this.module_size_max = 20;
            this.module_gap_min = "no";
            this.module_gap_max = "small";
            this.small_modules_robustness = "low";
            this.finder_pattern_tolerance = "low";
            this.additional_levels = 0;
            this.quality_isoiec15415_aperture_size = 0.8;
        }
    }
    [Serializable]
    public class DotDataCodeParam : DataCodeParam
    {
        private string _default_parameters;
        public override string default_parameters
        {
            get
            {
                return _default_parameters;
            }
            set
            {
                _default_parameters = value;
            }
        }
        public int symbol_cols_min { get; set; }
        public int symbol_cols_max { get; set; }
        public int symbol_rows_min { get; set; }
        public int symbol_rows_max { get; set; }
        public int module_size_min { get; set; }
        public int module_size_max { get; set; }
        public string module_gap_min { get; set; }
        public string module_gap_max { get; set; }
        public string candidate_selection { get; set; }
        public string max_allowed_error_correction { get; set; }
        // find data code
        public string strict_quiet_zone { get; set; }
        public DotDataCodeParam()
        {
            this.symbol_cols_min = 8;
            this.symbol_cols_max = 999;
            this.symbol_rows_min = 4;
            this.symbol_rows_max = 998;
            this.module_size_min = 4;
            this.module_size_max = 100;
            this.module_gap_min = "no";
            this.module_gap_max = "small";
            this.strict_quiet_zone = "no";
            this.candidate_selection = "default";
            this.max_allowed_error_correction = "1.0";
        }
    }


    [Serializable]
    public class SetDataCodeParam
    {
        public string GenParamName { get; set; }
        public string GenParamValue { get; set; }

        public SetDataCodeParam()
        {
            this.GenParamName = "polarity";
            this.GenParamValue = "light_on_dark";
        }
    }

    public class DataCodeResult
    {
        public string Content { get; set; }
        public HXLDCont SymbolXLDs { get; set; }
    }

    public enum enSymbolType
    {
        Aztec_Code,
        Data_Matrix_ECC_200,
        GS1_Aztec_Code,
        GS1_DataMatrix,
        GS1_QR_Code,
        Micro_QR_Code,
        PDF417,
        QR_Code,
        DotCode,
        GS1_DotCode,
    }



}

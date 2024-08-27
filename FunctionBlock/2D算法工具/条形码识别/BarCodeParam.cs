using HalconDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionBlock
{
    [Serializable]
    public class FindBarCodeParam
    {
        public string BarCodeType { get; set; }
        public bool InvertImage { get; set; }
        public int timeout { get; set; }
        public int persistence { get; set; }
        public int stop_after_result_num { get; set; }

        public FindBarCodeParam()
        {
            this.BarCodeType = "auto";
            this.InvertImage = false;
            this.timeout = 5000;
            this.persistence = 0;
            this.stop_after_result_num = 0;
        }




    }
    [Serializable]
    public class BarCodeParam
    {
        public double element_size_min { get; set; }
        public double element_size_max { get; set; }
        public string element_size_variable { get; set; }
        public int barcode_height_min { get; set; }
        public int barcode_width_min { get; set; } // 解码的数量
        public int num_scanlines { get; set; }
        public int min_identical_scanlines { get; set; }
        public string majority_voting { get; set; }
        public double orientation { get; set; }
        public double orientation_tol { get; set; }
        public string quiet_zone { get; set; }
        public string start_stop_tolerance { get; set; }
        public int min_code_length { get; set; }
        public string merge_scanlines { get; set; }
        public string small_elements_robustness { get; set; }
        public double meas_thresh { get; set; }
        public double meas_thresh_abs { get; set; }
        public int contrast_min { get; set; }
        public string check_char { get; set; }
        public string composite_code { get; set; }
        public string upce_encodation { get; set; }

        public BarCodeParam()
        {
            this.element_size_min = 2.0;
            this.element_size_max = 8.0;
            this.element_size_variable = "false";
            this.barcode_height_min = -1;
            this.barcode_width_min = -1;
            this.num_scanlines = 0;
            this.min_identical_scanlines = 2;
            this.majority_voting = "false";
            this.orientation = 0;
            this.orientation_tol = 90;
            this.quiet_zone = "false";
            this.start_stop_tolerance = "high";
            this.min_code_length = 0;
            this.merge_scanlines = "true";
            this.small_elements_robustness = "true";
            this.meas_thresh = 0.05;
            this.meas_thresh_abs = 5.0;
            this.contrast_min = 0;
            this.check_char = "absent";
            this.composite_code = "none";
            this.upce_encodation = "ucc-12";
        }
    }


    public class BarCodeResult
    {
        public string Content { get; set; }
        public HXLDCont SymbolXLDs { get; set; }
    }



}

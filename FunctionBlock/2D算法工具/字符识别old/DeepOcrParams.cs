using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace FunctionBlock
{
    [Serializable]
    public class DeepOcrParam
    {
        public int detection_image_height { get; set; }
        public int detection_image_width { get; set; }
    }
    //[Serializable]
    //public class DeepOcrDetectionParam:DeepOcrParam
    //{
    //    public string detection_device { get; set; }
    //    //public int detection_image_height { get; set; }
    //    //public int detection_image_width { get; set; }
    //    public double detection_min_character_score { get; set; }
    //    public double detection_min_link_score { get; set; }
    //    public int detection_min_word_area { get; set; }
    //    public double detection_min_word_score { get; set; }
    //    public int detection_orientation { get; set; }
    //    public bool detection_sort_by_line { get; set; }
    //    public int device { get; set; }
    //    public int recognition_device { get; set; }
    //}

    //[Serializable]
    //public class DeepOcrRecognitionParam : DeepOcrParam
    //{
    //    public string recognition_device { get; set; }
    //}


    [Serializable]
    public enum enDeepOcrMode
    {
        auto,
        recognition,
        detection,
    }

    [Serializable]
    public class DeepOcrCreateParam
    {
        public enDeepOcrMode DeepOcrMode { get; set; }
        public int RecognitionChartWidth { get; set; }
        public int RecognitionChartHeight { get; set; }

        public DeepOcrCreateParam()
        {
            this.DeepOcrMode = enDeepOcrMode.recognition;
            this.RecognitionChartWidth = 300;
            this.RecognitionChartHeight = 32;           
        }

    }



   
    [Serializable]
    public class DeepOcrRecognitionParam
    {
        public BindingList<ReduceParam> ReduceParam { get; set; }
        public bool InvertImage { get; set; }
        public int RecognitionChartWidth { get; set; }
        public DeepOcrRecognitionParam()
        {
            this.ReduceParam = new BindingList<ReduceParam>();
            this.InvertImage = false;
            this.RecognitionChartWidth = 100;
        }
    }


}

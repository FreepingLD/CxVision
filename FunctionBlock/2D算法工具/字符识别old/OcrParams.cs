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
    public class OcrParam
    {
        [DisplayName("取反图像")]
        public bool InvertImage { get; set; }

        //[Category("区域分割")]
        //[DisplayName("最小分割阈值")]
        //public double MinThreshold { get; set; }

        //[Category("区域分割")]
        //[DisplayName("最大分割阈值")]
        //public double MaxThreshold { get; set; }

        //[Category("区域操作")]
        //[DisplayName("区域排序")]
        //public enOrderMethod SortRegion { get; set; }
        //[Category("区域操作")]
        //[DisplayName("区域处理")]
        //public enRegionOperate RegionOperate { get; set; }
        //[Category("区域操作")]
        //[DisplayName("掩膜宽度")]
        //public int MaskWidth { get; set; }
        //[Category("区域操作")]
        //[DisplayName("掩膜高度")]
        //public int MaskHeight { get; set; }
        //[Category("区域操作")]
        //[DisplayName("最小面积")]
        //public int MinArea { get; set; }

        //[Category("区域操作")]
        //[DisplayName("最大面积")]
        //public int MaxArea { get; set; }

        //[DisplayName("字符区域")]
        //public BindingList<ReduceParam> ChartRegion { get; set; }

        public OcrParam()
        {
            InvertImage = false;
            //MinThreshold = 5;
            //MaxThreshold = 50;
            //SortRegion = enOrderMethod.column;
            //RegionOperate = enRegionOperate.NONE;
            //MaskWidth = 5;
            //MaskHeight = 5;
            //MinArea = 100;
            //MaxArea = 10000000;
            //this.ChartRegion = new BindingList<ReduceParam>();
        }

    }
    public class OcrCnnParam : OcrParam
    {
       [Category("Ocr参数")]
       [Description(
            "Universal_NoRej.occ,"+
            "Universal_Rej.occ,"+
            "Universal_0 - 9_NoRej.occ,"+
            "Universal_0 - 9_Rej.occ,"+
            "Universal_0 - 9 + _NoRej.occ,"+
            "Universal_0 - 9 + _Rej.occ,"+
            "Universal_0 - 9A - Z_NoRej.occ,"+
            "Universal_0 - 9A - Z_Rej.occ," +
            "Universal_0 - 9A - Z + _NoRej.occ,"+
            "Universal_0 - 9A - Z + _Rej.occ,"+
            "Universal_A - Z + _NoRej.occ,"+
            "Universal_A - Z + _Rej.occ")]
        [DisplayName("字体")]
        public string OcrFontName { get; set; }
        [Category("Ocr参数")]
        [DisplayName("Ocr方法")]
        public enOcrMethod OcrMethod { get; set; }

        [Category("OcrWord参数")]
        [DisplayName("类数量")]
        public int NumClasses { get; set; }
        [DisplayName("期望单词")]

        [Category("OcrWord参数")]
        public string Expression { get; set; }

        [DisplayName("字符校正类数量")]
        [Category("OcrWord参数")]
        public int NumAlternatives { get; set; }

        [Category("OcrWord参数")]
        [DisplayName("最大校正字符数")]
        public int NumCorrections { get; set; }

        public OcrCnnParam()
        {
            OcrMethod = enOcrMethod.MultipleCharacters;
            OcrFontName = "Universal_0 - 9A - Z_Rej.occ";
            NumClasses = 1;
        }

    }
    public class OcrKnnParam : OcrParam
    {
        [Category("Ocr参数")]
        [DisplayName("字体")]
        public string OcrFontName { get; set; }
        [DisplayName("Ocr方法")]
        [Category("Ocr参数")]
        public enOcrMethod OcrMethod { get; set; }
        [DisplayName("类数量")]
        [Category("OcrWord参数")]
        public int NumClasses { get; set; }
        [DisplayName("字符临近个数")]
        [Category("OcrWord参数")]
        public int NumNeighbors { get; set; } //NumClasses
        [DisplayName("期望单词")]
        [Category("OcrWord参数")]
        public string Expression { get; set; }

        [DisplayName("字符校正类数量")]
        [Category("OcrWord参数")]
        public int NumAlternatives { get; set; }

        [DisplayName("最大校正字符数")]
        [Category("OcrWord参数")]
        public int NumCorrections { get; set; }

        public OcrKnnParam()
        {
            OcrMethod = enOcrMethod.MultipleCharacters;
            OcrFontName = "Universal_0-9A-Z_NoRej.occ";
            NumClasses = 1;
            NumNeighbors = 1;
        }
    }
    public class OcrSvmParam : OcrParam
    {
        [DisplayName("字体")]
        [Category("Ocr参数")]
        public string OcrFontName { get; set; }
        [DisplayName("Ocr方法")]
        [Category("Ocr参数")]
        public enOcrMethod OcrMethod { get; set; }

        [DisplayName("类数量")]
        [Category("OcrWord参数")]
        public int NumClasses { get; set; }

        [DisplayName("期望单词")]
        [Category("OcrWord参数")]
        public string Expression { get; set; }

        [DisplayName("字符校正类数量")]
        [Category("OcrWord参数")]
        public int NumAlternatives { get; set; }

        [DisplayName("最大校正字符数")]
        [Category("OcrWord参数")]
        public int NumCorrections { get; set; }

        public OcrSvmParam()
        {
            OcrMethod = enOcrMethod.MultipleCharacters;
            OcrFontName = "font_characters_ocr";
            NumClasses = 1;
        }
    }
    public class OcrMlpParam : OcrParam
    {
        [Category("Ocr参数")]
        [Description(
            "Document_0-9_NoRej.omc,"+
            "Document_0-9_Rej.omc,"+
            "Document_0-9A-Z_NoRej.omc,"+
            "Document_0-9A-Z_Rej.omc,"+
            "Document_A-Z+_NoRej.omc,"+
            "Document_A-Z+_Rej.omc,"+
            "Document_NoRej.omc,"+
            "Document_Rej.omc,"+
            "DotPrint_0-9_NoRej.omc,"+
            "DotPrint_0-9_Rej.omc," +
            "DotPrint_0-9+_NoRej.omc," +
            "DotPrint_0-9+_Rej.omc," +
            "DotPrint_0-9A-Z_NoRej.omc," +
            "DotPrint_0-9A-Z_Rej.omc," +
            "DotPrint_A-Z+_NoRej.omc," +
            "DotPrint_A-Z+_Rej.omc," +
            "DotPrint_NoRej.omc," +
            "DotPrint_Rej.omc,"+
            "HandWritten_0-9_NoRej.omc,"+
            "HandWritten_0-9_Rej.omc,"+
            "Industrial_0-9_NoRej.omc," +
            "Industrial_0-9_Rej.omc," +
            "Industrial_0-9+_NoRej.omc," +
            "Industrial_0-9+_Rej.omc," +
            "Industrial_0-9A-Z_NoRej.omc," +
            "Industrial_0-9A-Z_Rej.omc," +
            "Industrial_A-Z+_NoRej.omc," +
            "Industrial_A-Z+_Rej.omc," +
            "Industrial_NoRej.omc," +
            "Industrial_Rej.omc," +
            "OCRA_0-9_NoRej.omc," +
            "OCRA_0-9_Rej.omc," +
            "OCRA_0-9A-Z_NoRej.omc," +
            "OCRA_0-9A-Z_Rej.omc," +
            "OCRA_A-Z+_NoRej.omc" +
            "OCRA_A-Z+_Rej.omc" +
            "OCRA_NoRej.omc," +
            "OCRA_Rej.omc," +
            "OCRB_0-9_NoRej.omc," +
            "OCRB_0-9_Rej.omc,"+
            "OCRB_0-9A-Z_NoRej.omc," +
            "OCRB_0-9A-Z_Rej.omc," +
            "OCRB_A-Z+_NoRej.omc," +
            "OCRB_A-Z+_Rej.omc," +
            "OCRB_NoRej.omc," +
            "OCRB_passport_NoRej.omc," +
            "OCRB_passport_Rej.omc," +
            "OCRB_Rej.omc," +
            "Pharma_0-9_NoRej.omc," +
            "Pharma_0-9_Rej.omc," +
            "Pharma_0-9+_NoRej.omc," +
            "Pharma_0-9+_Rej.omc," +
            "harma_0-9A-Z_NoRej.omc," +
            "Pharma_0-9A-Z_Rej.omc," +
            "Pharma_NoRej.omc," +
            "Pharma_Rej.omc," +
            "SEMI_NoRej.omc," +
            "SEMI_Rej.omc"
            )]
        [DisplayName("字体")]

        public string OcrFontName { get; set; }
        [DisplayName("Ocr方法")]
        [Category("Ocr参数")]
        public enOcrMethod OcrMethod { get; set; }
        [DisplayName("类数量")]
        [Category("OcrWord参数")]
        public int NumClasses { get; set; }
        [DisplayName("期望单词")]
        [Category("OcrWord参数")]
        public string Expression { get; set; }
        [DisplayName("字条校正类数量")]
        [Category("OcrWord参数")]
        public int NumAlternatives { get; set; }
        [DisplayName("最大校正字符数")]
        [Category("OcrWord参数")]
        public int NumCorrections { get; set; }

        public OcrMlpParam()
        {
            OcrMethod = enOcrMethod.MultipleCharacters;
            OcrFontName = "Industrial_0-9A-Z_Rej.omc";
            NumClasses = 1;
        }
    }

    public class OcrTextModelParam : OcrParam
    {
        [DisplayName("字体")]
        public string OcrFontName { get; set; }
        [DisplayName("最小对比度")]
        public int MinContrast { get; set; }
        [DisplayName("极性")]
        public enPolarity Polarity { get; set; }
        public string EliminateBorderBlobs { get; set; }
        public string AddFragments { get; set; }
        public int SeparateTouchingChars { get; set; }
        [DisplayName("最小字符高度")]
        public int MinCharHeight { get; set; }
        [DisplayName("最大字符高度")]
        public int MaxCharHeight { get; set; }
        [DisplayName("最小字符宽度")]
        public int MinCharWidth { get; set; }
        [DisplayName("最大字符宽度")]
        public int MaxCharWidth { get; set; }
        public int MinStrokeWidth { get; set; }
        public int MaxStrokeWidth { get; set; }

        public OcrTextModelParam()
        {
            OcrFontName = "Industrial_0-9A-Z_Rej.omc";
        }

    }

    [Serializable]
    public enum enOcrMethod
    {
        SingleCharacter,
        MultipleCharacters,
        Word,
    }
    [Serializable]
    public enum enOrderMethod
    {
        none,
        row,
        column,
    }
    [Serializable]
    public enum enRegionOperate
    {
        NONE,
        closing_rectangle1,
        opening_rectangle1,
    }
    [Serializable]
    public enum enOcrModel
    {
        OcrMLP,
        OcrCNN,
        OcrKNN,
        OcrSVM,
        OcrTextMode,
        DeepOCR,
    }
    [Serializable]
    public enum enPolarity
    {
        dark_on_light,
        light_on_dark,
        both
    }




}

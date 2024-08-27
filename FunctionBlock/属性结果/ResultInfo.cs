using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionBlock
{

    /// <summary>
    /// 基类只作为一个指针来引用
    /// </summary>
    [Serializable]
    public class ResultInfo
    {
        //[DisplayNameAttribute("类型名称")]
        //public enResultInfoType TypeNameInfo { get; set; }
    }


    public enum enResultInfoType
    {
        NONE,
        ReadPLcAdress,
        WritePLcAdress,
        WaitePLcAdress,
        AcqResultInfo,
        MeasureResultInfo,
        DataCodeResultInfo,
        CompensationResultInfo,
        Match2DResultInfo,
        OcrResultInfo,
        StringResultInfo,
        EventResultInfo,
        PlcResultInfo,
        CoordSysAxisParam,
    }
    public enum enFlag
    {
        NONE,
        OK,
        NG,
        OK_NG,
        Int1_2,
        Int0,
        Int1,
        Int2,
        JudgeData,
        测量值,   
        标准值,   
        上偏差,   
        下偏差,    
        结果,      
        测量值_标准值,    
        测量值_结果,    
        测量值_标准值_结果,   
        测量值_标准值_上偏差_下偏差_结果,    
        数据标签,
        测量值_标准值_上偏差,
        测量值_标准值_上偏差_结果,
        测量值_标准值_下偏差_结果,
        X,
        Y,
        Angle,
        ImagePath,
        Glue1,
        Glue2,
    }

}

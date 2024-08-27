using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionBlock
{
    [Serializable]
    public class ScriptParam
    {

       public string ScriptPath { get; set; }
        public enScriptType ScriptType{ get; set; }
        public BindingList<InParam> InParam { get; set; }

        public BindingList<OutParam> OutParam { get; set; }

        public ScriptParam()
        {
            this.ScriptPath = "";
            this.ScriptType = enScriptType.Halcon;
            this.InParam = new BindingList<InParam>();
            this.OutParam = new BindingList<OutParam>();
        }

    }
    [Serializable]
    public class InParam
    {
        public enHParamType ParamType { get; set; }
        public string ParamName { get; set; }
        public string ParamValue { get; set; }
    }
    [Serializable]
    public class OutParam
    {
        public enHParamType ParamType { get; set; }
        public string ParamName { get; set; }
        public string ParamValue { get; set; }
    }
    [Serializable]
    public enum enScriptType
    {
        Halcon,
        CSharp,
    }
    [Serializable]
    public enum enHParamType
    {
        NONE,
        HImage,
        HObject,
        HRegion,
        Int,
        Double,
        String,
    }

}

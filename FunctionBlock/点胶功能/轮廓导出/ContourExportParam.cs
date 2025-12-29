using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionBlock
{
    [Serializable]
    public class ContourExportParam
    {
        public string Path { get; set; }
        public string Extension { get; set; }
        public string DataType { get; set; }

        public ContourExportParam()
        {
            this.Path = "";
            this.Extension = "*.txt";
            this.DataType = "all";
        }


    }



}

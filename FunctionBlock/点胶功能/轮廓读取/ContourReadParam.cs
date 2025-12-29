using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionBlock
{
    [Serializable]
    public class ContourReadParam
    {
        public string Path { get; set; }
        public string Extension { get; set; }
        public double Offset_X { get; set; }
        public double Offset_Y { get; set; }


        public ContourReadParam()
        {
            this.Path = "";
            this.Extension = "*.txt";
            this.Offset_X= 0;   
            this.Offset_Y= 0;   
        }


    }



}

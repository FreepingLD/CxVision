using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionBlock
{
    [Serializable]
    public class OcrResult
    {
        public bool Result { get; set; }
        public string Character { get; set; }
        public List<double> Score { get; set; } 

        public OcrResult()
        {
            this.Character = "";
            this.Result = false;
            this.Score = new List<double>();

        }

    }
}

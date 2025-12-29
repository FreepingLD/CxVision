using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionBlock
{
    [Serializable]
    public class ArrayParam
    {
        public double RowOffset { get; set; }
        public double ColOffset { get; set; }
        public double RowCount { get; set; }
        public double ColCount { get; set; }

        public ArrayParam()
        {
            this.RowOffset = 0;
            this.ColOffset = 0;
            this.RowCount = 1;
            this.ColCount = 1;
 
        }
    }
}

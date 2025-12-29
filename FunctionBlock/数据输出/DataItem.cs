using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionBlock
{
    [Serializable]
    public class DataItem
    {
        public string Describe { get; set; }
        public string StdValue { get; set; }
        public string LimitUpTolerance { get; set; }
        public string LimitDownTolerance { get; set; }
        public string Value { get; set; }
        public string Result { get; set; }

    }

}

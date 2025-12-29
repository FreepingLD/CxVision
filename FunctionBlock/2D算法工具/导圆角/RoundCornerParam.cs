using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace FunctionBlock
{
    [Serializable]
    public class RoundCornerParam
    {
        public double Radius { get; set; }
        public int InterPoint { get; set; }
        public string PointOrder { get; set; }

        public RoundCornerParam()
        {
            this.Radius = 5;
            this.InterPoint = 10;
            this.PointOrder = "positive";
        }
    }
}

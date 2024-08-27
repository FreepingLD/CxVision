using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionBlock
{

    [Serializable]
    public class AlignmentGuidedParam
    {
        public double Add_X { get; set; }
        public double Add_Y { get; set; }
        public double Add_Theta { get; set; }
        public enRefObject RefObject { get; set; }
        public enAlignmentMethod AlignmentMethod { get; set; }


    }




}

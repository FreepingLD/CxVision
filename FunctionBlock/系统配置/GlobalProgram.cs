using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionBlock
{

    public class GlobalProgram
    {
        private static Dictionary<string, IFunction> _programItems = new Dictionary<string, IFunction>();
        public static Dictionary<string, IFunction> ProgramItems
        {
            get => _programItems; set => _programItems = value;
        }

    }


}

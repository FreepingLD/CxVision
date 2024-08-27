using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionBlock
{

   public class ProgramConfigParam
    {
        public ProgramConfigParam(string name)
        {
            this.Name = name;
        }
        public ProgramConfigParam()
        {

        }

        public string Name
        {
            set;
            get;
        }

        public string ProgramPath
        {
            set;
            get;
        }

        public bool IsAuto
        {
            set;
            get;
        }


    }



}

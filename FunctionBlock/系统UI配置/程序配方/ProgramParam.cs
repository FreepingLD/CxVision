using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionBlock
{
    [Serializable]
    public class ProgramParam
    {
        public ProgramParam(string path)
        {
            this.ProgramPath = path;
        }
        public ProgramParam()
        {

        }

        public string ProgramPath
        {
            set;
            get;
        }

        public bool IsActive
        {
            set;
            get;
        }
        public string Describe
        {
            set;
            get;
        }
        public string NONE
        {
            set;
            get;
        }


    }



}

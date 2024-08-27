using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    [Serializable]
   public  class dataTableConfig
    {
        private List<string> name = new List<string>(); //

        public List<string> Name
        {
            get
            {
                return name;
            }

            set
            {
                name = value;
            }
        }



    }
}

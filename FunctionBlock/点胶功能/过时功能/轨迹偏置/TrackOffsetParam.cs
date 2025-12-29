using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionBlock
{
    [Serializable]
    public class TrackOffsetParam
    {
        public enSortPoint SortMethod { get; set; }
        public double Distance { get; set; }



        public TrackOffsetParam()
        {
            this.SortMethod = enSortPoint.NONE;
            this.Distance = 0.01;
        }


    }




}

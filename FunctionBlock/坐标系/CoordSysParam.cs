using Common;
using System;

namespace FunctionBlock
{
    [Serializable]
    public class CoordSysParam
    {
        private bool isInit = false;
        private enOrigionType origionType = enOrigionType.交点;
        public enOrigionType OrigionType
        {
            get
            {
                return origionType;
            }

            set
            {
                origionType = value;
            }
        }
        public bool IsInit { get => isInit; set => isInit = value; }

        /// <summary>
        /// 补正类型
        /// </summary>
        public enAdjustType AdjustType { get; set; }
        public double RefPoint_Row { get; set; }
        public double RefPoint_Col { get; set; }
        public double RefPoint_Rad { get; set; }

        public CoordSysParam()
        {
            this.AdjustType = enAdjustType.XYTheta;
        }



    }
}

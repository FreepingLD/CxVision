using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionBlock
{
    [Serializable]
    public class CalibCoordConfigParam
    {
        
        /// <summary>
        /// 构造函数
        /// </summary>
        public CalibCoordConfigParam(double X, double Y, double Z, double Theta)
        {
            this.X = X;
            this.Y = Y;
            this.Z = Z;
            this.Theta = Theta;


        }
        public CalibCoordConfigParam()
        {

        }
        public double X
        {
            set;
            get;
        }
        public double Y
        {
            set;
            get;
        }
        public double Z
        {
            set;
            get;
        }
        public double Theta
        {
            set;
            get;
        }



    }


}
